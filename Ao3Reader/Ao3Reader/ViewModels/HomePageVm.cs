using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ao3Domain.Models.Data;
using Xamarin.Forms;
using Ao3Reader.Interfaces;
using Xamarin.Essentials;

namespace Ao3Reader.ViewModels
{
    public class HomePageVm : BaseViewModel
    {
        private IWorksService Service { get; }
        private ILocalStorage LocalStorage { get; }
        public ICommand RefreshDiscover { get; }
        public ICommand RefreshFavorites { get; }
        public ICommand HistorySelected { get; }
        public ICommand SearchCommand { get; }

        public HomePageVm(INavigator navigator, IAlert alert, IWorksService worksService, ILocalStorage localStorage) :
            base(navigator, alert)
        {
            Title = "Home";
            Service = worksService;
            LocalStorage = localStorage;
            RefreshDiscover = new Command(async () => await ReloadDiscover());
            RefreshFavorites = new Command(async () => await ReloadFavorites());
            HistorySelected = new Command(async () => await ShowHistoryDetails());
            SearchCommand = new Command<string>(async (search) => await SearchHistories(search));
        }

        #region Discover Props

        public bool DiscoverRefreshing { get; set; }
        public bool HasHistories { get; set; }
        public bool LoadingHistories { get; set; }
        public ObservableCollection<Work> DiscoverWorks { get; set; } = new ObservableCollection<Work>();
        public string SearchEntry { get; set; }

        #endregion

        #region Favorite Props

        public bool FavoritesRefreshing { get; set; }
        public bool HasFavorites { get; set; }
        public bool LoadingFavorites { get; set; }
        public ObservableCollection<Work> FavoriteWorks { get; set; } = new ObservableCollection<Work>();

        #endregion

        public object SelectedHistory { get; set; }

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            try
            {
                LoadingHistories = HasHistories = true;
                await ManageDiscover();
                await ManageFavorites();
            }
            catch (Exception e)
            {
                await Alerts.CallAlertAsync(e);
            }
            finally
            {
                LoadingHistories = false;
                HasHistories = DiscoverWorks.Any();
            }
        }

        private async Task ManageDiscover()
        {
            var works = await Service.LoadDiscoverWorks();
            works.ForEach(DiscoverWorks.Add);
        }

        private async Task ManageFavorites()
        {
            if (await Permissions.RequestAsync<Permissions.StorageRead>() == PermissionStatus.Granted)
            {
                await LocalStorage.LoadFile();
                LocalStorage.GetStorage().FavoriteWorks.ForEach(FavoriteWorks.Add);
            }
            else
            {
                await Alerts.CallToastAsync("Storage access permission must be granted for favorite functions",
                    TimeSpan.FromSeconds(7), Color.Salmon, Color.White, "alert");
            }
        }

        private async Task ReloadDiscover()
        {
            LoadingHistories = HasHistories = true;
            if (SearchEntry == string.Empty || SearchEntry is null)
            {
                await DiscoverHistories();
            }
            else
            {
                await SearchHistories(SearchEntry);
            }
        }

        private async Task ReloadFavorites()
        {
            try
            {
                LoadingFavorites = HasFavorites = true;
                FavoriteWorks.Clear();
                await Task.Run(() => { LocalStorage.GetStorage().FavoriteWorks.ForEach(FavoriteWorks.Add); });
            }
            finally
            {
                HasFavorites = FavoriteWorks.Any();
                LoadingFavorites = false;
                FavoritesRefreshing = false;
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
                await Alerts.CallAlertAsync(ex);
            }
            finally
            {
                HasHistories = DiscoverWorks.Any();
                LoadingHistories = false;
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
                await Alerts.CallAlertAsync(ex);
            }
            finally
            {
                HasHistories = DiscoverWorks.Any();
                LoadingHistories = false;
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
                await Alerts.CallAlertAsync(ex);
            }
        }
    }
}