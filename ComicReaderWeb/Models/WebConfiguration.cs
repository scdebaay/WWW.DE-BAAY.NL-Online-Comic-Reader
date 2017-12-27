using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Reflection;
using System.IO;
using System.Web.Mvc;
using ComicReaderWeb.Models;

namespace ComicReaderWeb.Models
{
    public class WebConfiguration : Controller
    {
        public WebConfiguration(System.Web.HttpContext context)
        {
            SetSettings();
            this.SiteRoot = GetBaseUrl();
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

        public string GetBaseUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public string ApiLocation { private set; get; }
        public string SiteRoot { private set; get; }
        public string pageLimit { private set; get; }
        public string defaultComicSize { private set; get; }
    }
}