using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.Resources
{
    public class AboutDataModel : IAboutDataModel
    {
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
            }
        }
    }
}
