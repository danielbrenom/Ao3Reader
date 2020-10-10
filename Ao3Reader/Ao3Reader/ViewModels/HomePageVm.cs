using System.Collections.ObjectModel;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;

namespace Ao3Reader.ViewModels
{
    public class HomePageVm : BaseViewModel
    {
        private IWorksService Service { get; }
        public HomePageVm(IWorksService worksService)
        {
            Title = "Home";
            Service = worksService;
        }
        
        public ObservableCollection<Work> DiscoverWorks { get; set; } = new ObservableCollection<Work>();

        public override async void HasNavigatedHere()
        {
            if (HasNavigated)
                return;
            HasNavigated = true;
            var works = await Service.LoadDiscoverWorks();
            works.ForEach(DiscoverWorks.Add);
        }
    }
}