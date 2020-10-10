using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ao3Reader.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties

        private bool mIsInternetAvailable;

        public bool InternetAvailable
        {
            get => mIsInternetAvailable;
            set => SetProperty(ref mIsInternetAvailable, value);
        }

        private bool mIsBusy;

        public bool IsBusy
        {
            get => mIsBusy;
            set => SetProperty(ref mIsBusy, value);
        }

        private string mTitle = string.Empty;

        protected string Title
        {
            get => mTitle;
            set => SetProperty(ref mTitle, value);
        }

        protected bool HasNavigated { get; set; }

        #endregion

        protected BaseViewModel()
        {
        }

        public abstract void HasNavigatedHere();

        #region INotifyPropertyChanged
        private void SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}