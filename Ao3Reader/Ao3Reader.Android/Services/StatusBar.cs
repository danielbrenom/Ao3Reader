using Android.App;
using Android.Views;
using Ao3Reader.Android.Services;
using Ao3Reader.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBar))]
namespace Ao3Reader.Android.Services
{
    public class StatusBar: IStatusBar
    {
        private WindowManagerFlags _originalFlags;
        public void HideStatusBar()
        {
            var activity = (Activity) Application.Context;
            var attrs = activity.Window.Attributes;
            _originalFlags = attrs.Flags;
            attrs.Flags = WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void ShowStatusBar()
        {
            var activity = (Activity) Application.Context;
            var attrs = activity.Window.Attributes;
            attrs.Flags = _originalFlags;
            activity.Window.Attributes = attrs;
        }
    }
}