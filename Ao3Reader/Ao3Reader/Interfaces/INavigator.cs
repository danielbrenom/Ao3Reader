using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ao3Reader.Interfaces
{
    public interface INavigator
    {
        public void BeginNavigation(string pageName, IReadOnlyDictionary<string, object> parameters = null,
            bool hasNavBar = true);
        public Task NavigateToAsync(string pageName, IReadOnlyDictionary<string, object> parameters);
        public Task ReturnAsync();
    }
}