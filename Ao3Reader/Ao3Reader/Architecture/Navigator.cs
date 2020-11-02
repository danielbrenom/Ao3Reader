using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ao3Reader.Interfaces;
using Ao3Reader.Views;
using Xamarin.Forms;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace Ao3Reader.Architecture
{
    public class Navigator : INavigator
    {
        private static NavigationPage CurrentPage => Application.Current.MainPage as NavigationPage;
        private IAlert Alerts;

        public void NavigateTo(bool hasNavigationBar = true)
        {
        }

        public void BeginNavigation(string pageName, IReadOnlyDictionary<string , object> parameters = null, bool hasNavBar = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page = GetPageTo(pageName);
                    Application.Current.MainPage = hasNavBar ? new NavigationPage(page) : page;
                    await ((IBaseViewModel) page.BindingContext).HandleNavigation(parameters);
                }
                catch (Exception)
                {
                    await Alerts.CallAlertAsync("Error", "Not able to navigate to destination page", new AlertAction("OK"));
                }
            });
        }

        public async Task NavigateToAsync(string pageName, IReadOnlyDictionary<string , object> parameters = null)
        {
            try
            {
                var page = GetPageTo(pageName);
                await CurrentPage.Navigation.PushAsync(page, true);
                await ((IBaseViewModel) page.BindingContext).HandleNavigation(parameters);
            }
            catch (Exception ex)
            {
                await CurrentPage.Navigation.PopAsync();
                await Alerts.CallAlertAsync("Error", "Not able to navigate to destination page", new AlertAction("OK"));
            }
        }

        private Page GetPageTo(string pageName)
        {
            return pageName switch
            {
                nameof(HomePage) => new HomePage(),
                nameof(WorkDetails) => new WorkDetails(),
                nameof(ChapterReading) => new ChapterReading(),
                _ => null
            };
        }
    }
}