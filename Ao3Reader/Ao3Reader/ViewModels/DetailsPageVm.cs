using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Ao3Domain.Models.Data;
using Ao3Reader.Extensions;
using Ao3Reader.Interfaces;
using Ao3Reader.Models;
using Xamarin.Forms;

namespace Ao3Reader.ViewModels
{
    public class DetailsPageVm: BaseViewModel
    {
        private readonly IWorksService _worksService;
        public ICommand SelectChapter { get; }

        public DetailsPageVm(INavigator navigator, IWorksService worksService) : base(navigator)
        {
            _worksService = worksService;
            SelectChapter = new Command(async ()=> await LoadChapter());
        }
        public ChapterListing SelectedChapter { get; set; }
        public WorkIndexing Work { get; set; }
        public ObservableCollection<ChapterListing> Chapters { get; set; } = new ObservableCollection<ChapterListing>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public bool FinishedLoad { get; set; } = true;

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            if (parameters is null)
                return;
            if (parameters.TryGetValue("work", out var work) && work is Work)
            {
                var result = await _worksService.GetWork(((Work)work).WorkId);
                Work = work.getAs<WorkIndexing>();
                Work.Tags.ForEach(tag => Tags.Add(new Tag{Name = tag}));
                result.Chapters.ForEach(Chapters.Add);
                FinishedLoad = false;
            }
        }

        private async Task LoadChapter()
        {
            if (SelectedChapter is null)
                return;
            try
            {
                var chapter = SelectedChapter as ChapterListing;
                await Navigator.NavigateToAsync("ChapterReading", new Dictionary<string, object> {{"chapter", chapter},{"work", Work}});
            }
            catch (Exception ex)
            {
                await Navigator.ShowAlert(ex);
            }
            finally
            {
                SelectedChapter = null;
                //OnPropertyChanged(nameof(SelectedChapter));
            }
        }
    }
}