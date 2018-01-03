using System;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ComicReaderWeb.Models
{
    public class WebConfiguration : Controller
    {
        public WebConfiguration(System.Web.HttpContext context)
        {
            //Call SetSettings() method to parse settings in the root web.config.
            //properties set: ApiLocation, pageLimit, defaultComicSize
            SetSettings();
            //Call GetBaseURL() helper to determine whether the app is running in the Root of a site or an application folder.
            this.SiteRoot = GetBaseUrl();
        }

        private void SetSettings()
        {
            //Cycle through <appSettings> section in root web.config and set properties accordingly.
            foreach (string key in WebConfigurationManager.AppSettings)
            {
                if (WebConfigurationManager.AppSettings[key].ToString() != null)
                {
                    //If a key is found for which no property can be set, just continue.
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

        public string GetBaseUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/") //App is not in siteroot, so running in it's own App folder.
            { appUrl = appUrl + "/"; } //Set appUrl to include App folder reference.
            //rebuild baseUrl with or without App folder reference.
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public string ApiLocation { private set; get; }
        public string SiteRoot { private set; get; }
        public string pageLimit { private set; get; }
        public string defaultComicSize { private set; get; }
    }
}