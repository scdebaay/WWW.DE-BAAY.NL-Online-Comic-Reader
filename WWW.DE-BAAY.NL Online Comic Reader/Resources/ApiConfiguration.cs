using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiConfiguration
    {
        /// <summary>
        /// Constructor for the ApiConfiguration object
        /// </summary>
        /// <param name="context">HttpContext is passed in for processing</param>
        public ApiConfiguration(HttpContext context)
        {
            SetSettings();
            //We need a defaultFileType to be passed back to the HttpResponse.
            //This can be either based on the settings in the web.config or the request passed in.
            string requestedFileName = context.Request.QueryString["file"];
            ContType requestedFile = new ContType(this.defaultFileType);
            if (requestedFileName != null)
            {
                requestedFile = new ContType(requestedFileName);
            }
            else 
            {
                requestedFile = new ContType(this.defaultFileType);
            }
            this.defaultFileType = requestedFile.Extension;
            //Based on the media type requested currentFolder is set to either comicFolder or imageFolder.
            this.currentFolder = findFolder(requestedFile.FolderType);
            //If the path is not relative to the Api App folder, serverPath property is set.
            if (Path.IsPathRooted(currentFolder))
            {
                this.serverPath = String.Empty;
            }
            else
            {
                this.serverPath = HttpRuntime.AppDomainAppPath;
            }
        }

        /// <summary>
        /// Iterate through the appSettings keys in the web.config and fill the ApiConfiguration properties.
        /// </summary>
        private void SetSettings()
        {
            foreach (string key in WebConfigurationManager.AppSettings)
            {
                if (WebConfigurationManager.AppSettings[key].ToString() != null)
                {
                    try
                    {
                        Type thistype = this.GetType();
                        PropertyInfo prop = thistype.GetProperty(key);
                        prop.SetValue(this, WebConfigurationManager.AppSettings[key].ToString(), null);
                    }
                    catch 
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Based on the requested file extension the folder to browse is set. For now this can be either Comic or Image
        /// </summary>
        /// <param name="requestedFileFolder">Folder type is passed in and the path is returned.</param>
        /// <returns></returns>
        private string findFolder(string requestedFileFolder)
        {
            return this.GetType().GetProperty(requestedFileFolder).GetValue(this, null).ToString();
        }

        public string mediaFolder { private set; get; }
        public string imageFolder { private set; get; }
        public string musicFolder { private set; get; }
        public string fileFolder { private set; get; }        
        public string comicFolder { private set; get; }
        public string epubFolder { private set; get; }
        public string defaultFile { private set; get; }
        public string pageLimit { private set; get; }
        private string currentFolder;
        public string defaultFileType { private set; get; }
        public string serverPath { private set; get; }
    }
}