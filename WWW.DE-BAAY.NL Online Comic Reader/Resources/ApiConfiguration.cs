using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Reflection;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiConfiguration
    {
        public ApiConfiguration(HttpContext context)
        {
            SetSettings();
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
            this.currentFolder = findFolder(requestedFile.FolderType);
            if (Path.IsPathRooted(currentFolder))
            {
                this.serverPath = String.Empty;
            }
            else
            {
                this.serverPath = HttpRuntime.AppDomainAppPath;
            }
        }

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