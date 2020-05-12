using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;
using ComicReaderDataManagementUI.Settings;
using System.Windows;
using System.Linq;
using NLog.Extensions.Logging;

namespace ComicReaderDataManagementUI
{
    class Startup : BootstrapperBase
    {
        public Startup()
        {
            Initialize();
        }
        IHost host; 
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            host = new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddSingleton<IWindowManager, WindowManager>();
               services.AddSingleton<IEventAggregator, EventAggregator>();
               services.AddSingleton<IConfiguration, Configuration>();
               services.AddTransient<ISqlUiDbConnection, SqlUiDbConnection>();
               services.AddTransient<ILanguageViewModel, LanguageViewModel>();
               services.AddTransient<ITypeViewModel, TypeViewModel>();
               services.AddTransient<ISeriesViewModel, SeriesViewModel>();
               services.AddTransient<ISubSeriesViewModel, SubSeriesViewModel>();
               services.AddTransient<IPublisherViewModel, PublisherViewModel>();
               services.AddTransient<IGenreViewModel, GenreViewModel>();
               services.AddTransient<IAuthorViewModel, AuthorViewModel>();
               services.AddLogging();
               GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => services.AddTransient(viewModelType, viewModelType));
           })
           .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog() // NLog: Setup NLog for Dependency injection
                    .Build();  
            
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    DisplayRootViewFor<MainViewModel>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                }                
            }        
        }

        protected override object GetInstance(Type service, string key)
        {
            return host.Services.GetService(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return host.Services.GetServices(service);
        }
    }
}
