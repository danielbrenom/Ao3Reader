using Ao3Reader.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFShimmerLayout.Controls;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Ao3Reader
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ShimmerLayout.Init(Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density);
            //Startup.Init();
            Startup.ServiceProvider.GetService<INavigator>().BeginNavigation("HomePage");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}