using System;
using System.Reflection;
using Ao3Reader.Configuration;
using Ao3Reader.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace Ao3Reader
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IServiceCollection Container { get; set; }

        public static void Init()
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("Ao3Reader.appsettings.json");
            var host = new HostBuilder().ConfigureHostConfiguration(c =>
                {
                    c.AddCommandLine(new string[] {$"ContentRoot={FileSystem.AppDataDirectory}"});
                    c.AddJsonStream(stream);
                }).ConfigureServices(ConfigureServices)
                .ConfigureLogging(log => log.AddConsole(o => o.DisableColors = true))
                .Build();
            ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            Container = services;
            if (context.HostingEnvironment.IsDevelopment())
            {
                ServiceFactory.ConfigureMock(services);
            }
            else
            {
                ServiceFactory.Configure(services);
            }
        }
    }
}