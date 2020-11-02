using System;
using System.Threading.Tasks;
using Ao3Reader.Architecture;
using Xamarin.Forms;

namespace Ao3Reader.Interfaces
{
    public interface IAlert
    {
        public Task CallAlertAsync(string title, string message, AlertAction button);

        public Task CallAlertAsync(string title, string message, AlertAction confirmButton,
            AlertAction cancelButton);

        public Task CallAlertAsync(Exception ex, bool popup = false);

        public Task CallToastAsync(string message, TimeSpan? exibTime = null, Color? backgroundColor = null,
            Color? fontColor = null, string icon = null);
    }
}