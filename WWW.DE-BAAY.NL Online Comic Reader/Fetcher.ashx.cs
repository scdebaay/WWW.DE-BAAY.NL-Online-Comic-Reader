using System;
using System.Web;
using System.Web.Configuration;
using System.IO;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine;
using System.Drawing;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader
{
    /// <summary>
    /// Summary description for Comic
    /// </summary>
    public class Fetcher : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ApiCall call = new ApiCall(context);
            call.ProcessRequest();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}