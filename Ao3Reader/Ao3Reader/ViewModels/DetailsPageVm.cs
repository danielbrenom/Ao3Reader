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
    public class DetailsPageVm : BaseViewModel
    {
        private readonly IWorksService _worksService;
        private readonly IFavoriteService _favoriteService;
        public ICommand SelectChapter { get; }
        public ICommand FavoriteCommand { get; }

        public DetailsPageVm(INavigator navigator, IAlert alert, IWorksService worksService,
            IFavoriteService favoriteService) : base(navigator, alert)
        {
            _worksService = worksService;
            _favoriteService = favoriteService;
            SelectChapter = new Command(async () => await LoadChapter());
            FavoriteCommand = new Command<WorkIndexing>(async (work) => await FavoriteWork(work.getAs<Work>()));
        }

        public ChapterListing SelectedChapter { get; set; }
        public WorkIndexing Work { get; set; }
        public ObservableCollection<ChapterListing> Chapters { get; set; } = new ObservableCollection<ChapterListing>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public bool FinishedLoad { get; set; } = true;
        public bool WorkFavorited { get; set; }

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            if (parameters is null)
                return;
            if (parameters.TryGetValue("work", out var work) && work is Work)
            {
                var result = await _worksService.GetWork(((Work) work).WorkId);
                Work = work.getAs<WorkIndexing>();
                Work.Tags.ForEach(tag => Tags.Add(new Tag {Name = tag}));
                result.Chapters.ForEach(Chapters.Add);
                WorkFavorited = await _favoriteService.CheckFavorites(work.getAs<Work>());
                FinishedLoad = false;
            }
        }

        private async Task LoadChapter()
        {
            if (SelectedChapter is null)
                return;
            try
            {
                var chapter = SelectedChapter;
                await Navigator.NavigateToAsync("ChapterReading",
                    new Dictionary<string, object> {{"chapter", chapter}, {"work", Work}});
            }
            catch (Exception ex)
            {
                await Alerts.CallAlertAsync(ex);
            }
            finally
            {
                SelectedChapter = null;
            }
        }

        private async Task FavoriteWork(Work work)
        {
            if (!await _favoriteService.CheckFavorites(work))
            {
                await _favoriteService.AddToFavorites(work);
                WorkFavorited = true;
                await Alerts.CallToastAsync("Added to favorites", null, Color.PaleGreen, Color.White);
            }
            else
            {
                await _favoriteService.RemoveFromFavorites(work);
                WorkFavorited = false;
                await Alerts.CallToastAsync("Removed from favorites", null, Color.Salmon, Color.White);
            }
        }
    }
}