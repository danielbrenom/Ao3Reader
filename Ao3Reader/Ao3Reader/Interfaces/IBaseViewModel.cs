using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Ao3Reader.Interfaces
{
    public interface IBaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Task HandleNavigation(IReadOnlyDictionary<string, object> parameters);
    }
}