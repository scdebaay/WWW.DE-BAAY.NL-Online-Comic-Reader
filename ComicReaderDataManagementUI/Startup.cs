using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using System.IO.Abstractions;
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
        /// <summary>
        /// Startup class which implements Caliburn Micro VM Framework, but also implements MS DI.
        /// </summary>
        public Startup()
        {
            Initialize();
        }
        
        /// <summary>
        /// Backing field for IHost implementation
        /// </summary>
        IHost host; 
        /// <summary>
        /// Startup method to build up MS DI container. The services configures are of the CM MV Framework type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
               services.AddTransient<IAboutViewModel, AboutViewModel>();
               services.AddTransient<IFileSystem, FileSystem>();
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
                    DisplayRootViewForAsync<MainViewModel>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                }                
            }        
        }

        /// <summary>
        /// Caliburn Micro GetInstance Method override. Maps the CM GetInstance method to the MS DI GetService Method
        /// </summary>
        /// <param name="service">Type, requested service type</param>
        /// <param name="key">String, key for the requested service</param>
        /// <returns>Object, representing the requested service from the Host object</returns>
        protected override object GetInstance(Type service, string key)
        {
            return host.Services.GetService(service);
        }

        /// <summary>
        /// Caliburn Micro GetAllInstances Method override. Maps the CM GetAllInstances method to the MS DI GetServices Method
        /// </summary>
        /// <param name="service">Type, requested service type</param>
        /// <returns>IEnumerable<object>, representing an IEnumerable of Object, requested services from the Host object</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return host.Services.GetServices(service);
        }
    }
}
