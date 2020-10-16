using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ao3Domain.Models.Data;
using Xamarin.Forms;
using Ao3Reader.Interfaces;

namespace Ao3Reader.ViewModels
{
    public class HomePageVm : BaseViewModel
    {
        private IWorksService Service { get; }
        public ICommand RefreshDiscover { get; }
        public ICommand HistorySelected { get; }
        public ICommand SearchCommand { get; }

        public HomePageVm(INavigator navigator, IWorksService worksService) : base(navigator)
        {
            Title = "Home";
            Service = worksService;
            RefreshDiscover = new Command(async () => await ReloadDiscover());
            HistorySelected = new Command(async () => await ShowHistoryDetails());
            SearchCommand = new Command<string>(async (search) => await SearchHistories(search));
        }
        
        public bool DiscoverRefreshing { get; set; }
        public bool HasHistories { get; set; }
        public ObservableCollection<Work> DiscoverWorks { get; set; } = new ObservableCollection<Work>();
        public object SelectedHistory { get; set; }
        
        public string SearchEntry { get; set; }

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            try
            {
                var works = await Service.LoadDiscoverWorks();
                works.ForEach(DiscoverWorks.Add);
            }
            catch (Exception e)
            {
                await Navigator.ShowAlert(e);
            }
            finally
            {
                HasHistories = DiscoverWorks.Any();
            }
        }

        private async Task ReloadDiscover()
        {
            if (SearchEntry == string.Empty)
            {
                await DiscoverHistories();
            }
            else
            {
                await SearchHistories(SearchEntry);
            }
        }

        private async Task DiscoverHistories()
        {
            try
            {
                DiscoverWorks.Clear();
                var works = await Service.LoadDiscoverWorks();
                works.ForEach(DiscoverWorks.Add);
            }
            catch (Exception ex)
            {
                await Navigator.ShowAlert(ex);
            }
            finally
            {
                HasHistories = DiscoverWorks.Any();
                DiscoverRefreshing = false;
            }
        }
        
        private async Task SearchHistories(string search)
        {
            try
            {
                DiscoverWorks.Clear();
                var works = await Service.SearchWorks(search);
                works.ForEach(DiscoverWorks.Add);
            }
            catch (Exception ex)
            {
                await Navigator.ShowAlert(ex);
            }
            finally
            {
                HasHistories = DiscoverWorks.Any();
                DiscoverRefreshing = false;
            }
        }

        private async Task ShowHistoryDetails()
        {
            if (SelectedHistory is null)
                return;
            try
            {
                var work = SelectedHistory as Work;
                SelectedHistory = null;
                await Navigator.NavigateToAsync("WorkDetails", new Dictionary<string, object> {{"work", work}});
            }
            catch (Exception ex)
            {
                await Navigator.ShowAlert(ex);
            }
        }
    }
}