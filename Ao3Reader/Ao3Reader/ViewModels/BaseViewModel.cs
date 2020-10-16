using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Ao3Reader.Interfaces;

namespace Ao3Reader.ViewModels
{
    public abstract class BaseViewModel : IBaseViewModel, INotifyPropertyChanged
    {
        #region Properties

        public bool InternetAvailable { get; set; }

        public bool IsBusy { get; set; }

        protected string Title { get; set; }

        protected bool HasNavigated { get; set; }
        protected INavigator Navigator { get; }

        #endregion
        
        #region Abstracts
        
        public abstract Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null);
        
        #endregion

        protected BaseViewModel(INavigator navigator)
        {
            Navigator = navigator;
        }

        #region INotifyPropertyChanged

        protected void SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            backingStore = value;
            onChanged?.Invoke();
            //OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        // {
        //     var changed = PropertyChanged;
        //
        //     changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // }

        #endregion
    }
}