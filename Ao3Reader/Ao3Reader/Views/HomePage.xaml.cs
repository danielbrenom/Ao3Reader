﻿using Ao3Reader.ViewModels;
using Xamarin.Forms.Xaml;

namespace Ao3Reader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<HomePageVm>();
        }
    }
}