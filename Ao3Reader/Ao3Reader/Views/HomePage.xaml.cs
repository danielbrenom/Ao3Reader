using Ao3Reader.Extensions;
using Ao3Reader.ViewModels;
using Xamarin.Forms.Xaml;

namespace Ao3Reader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        protected HomePageVm Context;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<HomePageVm>();
            Context = BindingContext.getAs<HomePageVm>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.HasNavigatedHere();
        }
    }
}