using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Settings;
using ComicReaderDataManagementUI.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ComicReaderDataManagementUI
{
    /// <summary>
    /// Example bootstrapper class for use with Caliburn Micro VM Framework. not used as this does not implement MS DI.
    /// </summary>
    class Bootstrapper : BootstrapperBase
    {
        SimpleContainer _container = new SimpleContainer();
        public IConfiguration Configuration { get; private set; }
        
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container);
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .PerRequest<ISqlUiDbConnection, SqlUiDbConnection>();
            _container.Singleton<IConfiguration, Configuration>();
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));

        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {   
            DisplayRootViewForAsync<MainViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        //protected override void BuildUp(object instance)
        //{
        //    _container.BuildUp(instance);
        //}
    }
}
