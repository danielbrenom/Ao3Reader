using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly IReadHistoryService _historyService;
        public ICommand SelectChapter { get; }
        public ICommand FavoriteCommand { get; }

        public DetailsPageVm(INavigator navigator, IAlert alert, IWorksService worksService,
            IFavoriteService favoriteService, IReadHistoryService readHistoryService) : base(navigator, alert)
        {
            _worksService = worksService;
            _favoriteService = favoriteService;
            _historyService = readHistoryService;
            SelectChapter = new Command(async () => await LoadChapter());
            FavoriteCommand = new Command<WorkIndexing>(async (work) => await FavoriteWork(work.getAs<Work>()));
        }

        public ChapterListing SelectedChapter { get; set; }
        public WorkIndexing Work { get; set; }
        public ObservableCollection<ChapterListingView> Chapters { get; set; } = new ObservableCollection<ChapterListingView>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public bool FinishedLoad { get; set; } = true;
        public bool WorkFavorited { get; set; }

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            try
            {
                if (parameters is null)
                    return;
                if (parameters.TryGetValue("work", out var work) && work is Work)
                {
                    var result = await _worksService.GetWork(((Work) work).WorkId);
                    if (!await _historyService.CheckHistory(result.WorkId))
                        await _historyService.InitializeHistory(result);
                    var history = await _historyService.GetWorkHistory(result.WorkId);
                    Work = work.getAs<WorkIndexing>();
                    Work.Tags.ForEach(tag => Tags.Add(new Tag {Name = tag}));
                    result.Chapters.ForEach(chapter =>
                    {
                        var ch = chapter.getAs<ChapterListingView>();
                        ch.ChapterRead = history.ChapterRead[ch.Id];
                        Chapters.Add(ch);
                    });
                    WorkFavorited = await _favoriteService.CheckFavorites(work.getAs<Work>());
                }
            }
            finally
            {
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
                Chapters.First(ch => ch.Id == chapter.Id).ChapterRead = true;
                //await _historyService.MarkChapter(Work.WorkId, SelectedChapter.Id);
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