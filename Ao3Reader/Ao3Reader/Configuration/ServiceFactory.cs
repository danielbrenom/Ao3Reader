using Ao3Reader.Interfaces;
using Ao3Reader.Services;
using Ao3Reader.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Ao3Reader.Configuration
{
    public static class ServiceFactory
    {
        public static void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IConfigurationManager, ConfigurationManager>();
            InjectServices(serviceCollection);
            InjectViewModels(serviceCollection);
        }
        
        public static void ConfigureMock(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddSingleton<IConfigurationManager, ConfigurationManager>();
            InjectMockServices(serviceCollection);
            InjectViewModels(serviceCollection);
        }
        private static void InjectServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpService, HttpService>();
            serviceCollection.AddTransient<IWorksService, WorksService>();
        }

        private static void InjectMockServices(IServiceCollection serviceCollection){}


        private static void InjectViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<HomePageVm>();
        }
    }
}