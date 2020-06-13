using Caliburn.Micro;
using ComicReaderClassLibrary.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class AboutViewModel : Screen, IAboutViewModel
    {
        private readonly IConfiguration _configuration;
        public AboutViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _ = GetApiVersionAsync();
            SetApplicationVersion();
            SetApiLocation();
            SetReaderLocation();
        }

        private string _applicationVersion;
        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set 
            { 
                _applicationVersion = value;
                NotifyOfPropertyChange(nameof(ApplicationVersion));
            }
        }

        private string _apiName;
        public string ApiName
        {
            get
            {
                return _apiName;
            }
            set
            {
                _apiName = value;
                NotifyOfPropertyChange(nameof(ApiName));
            }
        }

        private string _apiVersion;
        public string ApiVersion
        {
            get
            {
                return _apiVersion;
            }
            set
            {
                _apiVersion = value;
                NotifyOfPropertyChange(nameof(ApiVersion));
            }
        }

        private string _libraryName;
        public string LibraryName
        {
            get
            {
                return _libraryName;
            }
            set
            {
                _libraryName = value;
                NotifyOfPropertyChange(nameof(LibraryName));
            }
        }

        private string _libraryVersion;
        public string LibraryVersion
        {
            get
            {
                return _libraryVersion;
            }
            set
            {
                _libraryVersion = value;
                NotifyOfPropertyChange(nameof(LibraryVersion));
            }
        }

        private string _apiLocation;
        public string ApiLocation
        {
            get
            {
                return _apiLocation;
            }
            set
            {
                _apiLocation = value;
                NotifyOfPropertyChange(nameof(ApiLocation));
            }
        }

        private string _readerLocation;
        public string ReaderLocation
        {
            get
            {
                return _readerLocation;
            }
            set
            {
                _readerLocation = value;
                NotifyOfPropertyChange(nameof(ReaderLocation));
            }
        }

        private void SetApplicationVersion()
        {
            ApplicationVersion = $"{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void SetApiLocation()
        { 
            ApiLocation = $"{_configuration["ApiLocation"]}";
        }

        private void SetReaderLocation()
        {
            ReaderLocation = $"{_configuration["ComicReaderUrl"]}";
        }
        
        private async Task GetApiVersionAsync()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_configuration["ApiLocation"])
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = $"{client.BaseAddress}Version";
            HttpResponseMessage response = await client.GetAsync(request);
            client.Dispose();
            response.EnsureSuccessStatusCode();
            IAboutDataModel versionResult = JsonConvert.DeserializeObject<AboutDataModel>(response.Content.ReadAsStringAsync().Result);
            ApiName = versionResult.ApiName;
            ApiVersion = versionResult.ApiVersion;
            LibraryName = versionResult.LibraryName;
            LibraryVersion = versionResult.LibraryVersion;
        }
    }
}
