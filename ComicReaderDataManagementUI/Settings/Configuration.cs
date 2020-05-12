using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ComicReaderDataManagementUI.Settings
{
    public class Configuration : IConfiguration
    {
        public string this[string key] { get { return _builder[key]; } set { _builder[key] = value; } }

        private readonly IConfiguration _builder;
        public Configuration() { 
        IConfigurationBuilder builder = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _builder = builder.Build();
        }
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _builder.GetChildren();            
        }

        public IChangeToken GetReloadToken()
        {
            return _builder.GetReloadToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            return _builder.GetSection(key);
        }
    }
}
