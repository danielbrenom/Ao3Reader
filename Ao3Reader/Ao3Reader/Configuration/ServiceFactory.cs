using Ao3Reader.Architecture;
using Ao3Reader.Interfaces;
using Ao3Reader.Services;
using Ao3Reader.ViewModels;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ao3Reader.Configuration
{
    public static class ServiceFactory
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddAutoMapper(typeof(Startup));
            serviceCollection.AddSingleton<IConfigurationManager, ConfigurationManager>();
            serviceCollection.AddSingleton<INavigator, Navigator>();
            serviceCollection.AddSingleton<IAlert, Alerts>();
            serviceCollection.AddSingleton<ILocalStorage, LocalStorage>();
            InjectServices(serviceCollection);
            InjectViewModels(serviceCollection);
        }
        
        public static void ConfigureMock(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IConfigurationManager, ConfigurationManager>();
            serviceCollection.AddSingleton<INavigator, Navigator>();
            serviceCollection.AddSingleton<IAlert, Alerts>();
            serviceCollection.AddSingleton<ILocalStorage, LocalStorage>();
            InjectMockServices(serviceCollection);
            InjectViewModels(serviceCollection);
        }
        private static void InjectServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpService, HttpService>();
            serviceCollection.AddTransient<IWorksService, WorksService>();
            serviceCollection.AddTransient<IFavoriteService, FavoriteService>();
        }

        private static void InjectMockServices(IServiceCollection serviceCollection){}


        private static void InjectViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<HomePageVm>();
            serviceCollection.AddTransient<DetailsPageVm>();
            serviceCollection.AddTransient<ReaderPageVm>();
        }
    }
}