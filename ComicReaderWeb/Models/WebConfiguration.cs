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
            string baseURL = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath ;
            string querySlash = baseURL[baseURL.Length - 1].ToString();
            if ( querySlash == "/")
            {
                this.SiteRoot = context.Request.Url.ToString(); 
            }
            else
            {
                this.SiteRoot = baseURL + "/";
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

        public string ApiLocation { private set; get; }
        public string SiteRoot { private set; get; }
        public string pageLimit { private set; get; }
        public string defaultComicSize { private set; get; }
    }
}