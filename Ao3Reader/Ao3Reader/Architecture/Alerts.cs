using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Ao3Reader.Interfaces;
using Xamarin.Forms;

namespace Ao3Reader.Architecture
{
    public class Alerts : IAlert
    {
        private static NavigationPage CurrentPage => Application.Current.MainPage as NavigationPage;

        public async Task CallAlertAsync(string title, string message, AlertAction button)
        {
            await CurrentPage.DisplayAlert("Error", "Not able to navigate to destination page", button?.Text ?? "OK");
            button?.ActionButton?.Invoke();
        }

        public async Task CallAlertAsync(string title, string message, AlertAction confirmButton,
            AlertAction cancelButton)
        {
            var choice = await CurrentPage.DisplayAlert("Error", "Not able to navigate to destination page",
                confirmButton?.Text ?? "OK", cancelButton?.Text ?? "Cancel");
            if (choice)
                confirmButton?.ActionButton?.Invoke();
            else
                cancelButton?.ActionButton?.Invoke();
        }

        public async Task CallAlertAsync(Exception ex, bool popup = false)
        {
            if (popup)
                await CurrentPage.DisplayAlert("Error", "An error occured", "OK");
            else
                await CallToastAsync("An error occured", TimeSpan.FromSeconds(4), Color.Red, Color.White);
        }


        public async Task CallToastAsync(string message, TimeSpan? exibTime = null, Color? backgroundColor = null,
            Color? fontColor = null, string icon = null)
        {
            var time = exibTime ?? TimeSpan.FromSeconds(5);
            using (CallToast(message, time, backgroundColor, fontColor, icon))
            {
                await Task.Delay(time);
            }
        }

        private static IDisposable CallToast(string message, TimeSpan exibTime, Color? backgroundColor = null,
            Color? fontColor = null, string icon = null)
        {
            var toast = new ToastConfig(message);
            toast.SetDuration(exibTime);
            if (backgroundColor.HasValue)
                toast.SetBackgroundColor(backgroundColor.Value);
            if (fontColor.HasValue)
                toast.SetMessageTextColor(fontColor.Value);
            if (icon == string.Empty)
                toast.SetIcon(icon);
            return UserDialogs.Instance.Toast(toast);
        }
    }

    public class AlertAction
    {
        public string Text { get; }
        public Action ActionButton { get; }

        public AlertAction(string texto)
        {
            Text = texto;
        }

        public AlertAction(string text, Action action) : this(text)
        {
            ActionButton = action;
        }
    }
}
