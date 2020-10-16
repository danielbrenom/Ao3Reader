using Ao3Reader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ao3Reader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkDetails
    {
        public WorkDetails()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<DetailsPageVm>();
        }
    }
}