using System.Web;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader
{
    /// <summary>
    /// Initial call for the handler. Requests are passed on to the API Call object in the resources folder for further processing.
    /// This is standard handler implementation.
    /// <summary>
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