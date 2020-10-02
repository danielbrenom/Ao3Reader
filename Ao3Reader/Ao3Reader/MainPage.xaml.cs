using System;
using Ao3Reader.Models;
using Xamarin.Forms;

namespace Ao3Reader
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            masterPage.listView.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is MasterPageItem item))
                return;
            Detail = new NavigationPage((Page) Activator.CreateInstance(item.TargetType));
            masterPage.listView.SelectedItem = null;
            IsPresented = false;
        }
    }
}