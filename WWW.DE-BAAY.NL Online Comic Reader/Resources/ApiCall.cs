using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Xml.Linq;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiCall
    {
        /// <summary>
        /// Constructor: HttpContext object is fed in as a parameter and split into httpContext and Request.
        /// Context is used to call configuration object using the context parameters.
        /// </summary>
        /// <param name="context">HttpContext oject passed on from the initial Fetcher object.</param>
        public ApiCall(HttpContext context)
        {
            this.httpContext = context;
            this.httpRequest = httpContext.Request;
        }

        /// <summary>
        /// After the ApiCall object is created, the request can be processed. 
        /// No parameters needed as all that is needed is created on initialization of the ApiCall object.
        /// </summary>
        public void ProcessRequest()
        {
            //If a querystring was passed to the Fetcher object, it was passed into the ApiCall and we try to process it.
            if (httpContext.Request.QueryString.Count != 0)
            {
                //The querystring should be formatted as either:
                //?file=<filepath><filename>&further parameters
                //or
                //?folder=<folderpath>&further parameters.
                //These two types of requests determine further processing. The first querystring key is converted to one string.
                string request = httpContext.Request.QueryString.AllKeys[0].ToString();

                switch (request)
                {
                    //First querystring key is "file", a file is requested.
                    case "file":
                        try
                        {
                            //Ascertain whether the requested file exists.
                            this.RequestedfileName = httpContext.Request.QueryString["file"];
                            this.RequestedfileExists = false;
                            this.Requestedfile = new FileInfo( ApiConfiguration.ServerPath(RequestedfileName) + ApiConfiguration.ComicFolder + RequestedfileName);
                            this.RequestedfileExists = File.Exists(Requestedfile.FullName);
                            //Attempt to determine the requested filetype based on extension. The type is set in the response.
                            //This should allow the browser to handle filetype accordingly.
                            Utility.DetermineRequestedFileType(httpContext, RequestedfileName);
                            switch (httpContext.Response.ContentType)
                            {
                                //Trying to return the filetype as CBR, to be able to use response within CBR/CBZ aware apps.
                                case "Application/x-cbr":
                                    //Create a FetchComic object which contains the requested page in byte format.
                                    FetchComic responseComic = new FetchComic(RequestedfileName, httpContext);
                                    httpContext.Response.Clear();
                                    httpContext.Response.ClearContent();
                                    httpContext.Response.ClearHeaders();
                                    httpContext.Response.Buffer = true;
                                    httpContext.Response.ContentType = responseComic.PageType;
                                    httpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                                    httpContext.Response.Cache.SetNoStore();
                                    //This is failing for now:
                                    //httpContext.Response.AddHeader("Content-Length", responseComic.result.GetLength(0).ToString());
                                    //httpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + responseComic.PageName);
                                    //httpContext.Response.AddHeader("Pragma", "public");
                                    //Write the resulting page to the response stream.
                                    httpContext.Response.BinaryWrite(responseComic.result);
                                    httpContext.Response.Flush();
                                    httpContext.Response.End();
                                    break;
                                //If an image of type png, jpg or gif was requested, fetch the image and write it to the repsonse stream.
                                case "Image/png":
                                case "Image/jpg":
                                case "Image/gif":
                                    //Create a FetchImage object which contains the requested image in byte format.
                                    FetchImage responseImage = new FetchImage(RequestedfileName, httpContext);
                                    httpContext.Response.Clear();
                                    httpContext.Response.ClearContent();
                                    httpContext.Response.ClearHeaders();
                                    httpContext.Response.Buffer = true;
                                    httpContext.Response.ContentType = responseImage.ImageType;
                                    httpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                                    httpContext.Response.Cache.SetNoStore();
                                    //This is failing for now:
                                    //httpContext.Response.AddHeader("Content-Length", responseComic.result.GetLength(0).ToString());
                                    //httpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + responseComic.PageName);
                                    //httpContext.Response.AddHeader("Pragma", "public");
                                    //Write the resulting image to the response stream.
                                    httpContext.Response.BinaryWrite(responseImage.result);
                                    httpContext.Response.Flush();
                                    httpContext.Response.End();
                                    break;
                                //If all else fails, the request the defaultFileType and defaultFile 
                                //from the app configuration and pass it to the response
                                default:
                                    httpContext.Response.ContentType = ApiConfiguration.DefaultFileType;
                                    httpContext.Response.WriteFile(ApiConfiguration.DefaultFile);
                                    break;
                            }

                        }
                        catch
                        {
                            //No error handling for now. There is always a response.
                            break;
                        }
                        break;
                    //First querysting key is "folder", a folder was requested.
                    case "folder":
                        {
                            //A folder request should look like:
                            //?folder=<path>&pageLimit=<int>&page=<int> to implement paging of the result.
                            //A folder request will send a directory listing in XML format.
                            try
                            {
                                this.RequestedFolder = httpContext.Request.QueryString["folder"].ToString();
                                //pageLimit is set. Return the amount of files in the listing per page as requested.
                                if (httpContext.Request.QueryString["pageLimit"] != null)
                                {
                                    int limit;
                                    string str_limit = httpContext.Request.QueryString["pageLimit"].ToString();
                                    //If the pageLimit cannot be parsed into an int, then return the app configured value.
                                    //There must always be a response.
                                    if (int.TryParse(str_limit, out limit))
                                    {
                                        this.pageLimit = limit;
                                    }
                                    else
                                    {
                                        this.pageLimit = ApiConfiguration.PageLimit;
                                    }

                                }
                                //PageLimit is not set, return the amount of items per page as configured in app settings.
                                else
                                {
                                    this.pageLimit = ApiConfiguration.PageLimit;
                                }
                                //Page is set, return the requested page, if the value can be parsed into an int.
                                //Otherwise, send the first page.
                                if (httpContext.Request.QueryString["page"] != null)
                                {
                                    int start = 0;
                                    string str_start = httpContext.Request.QueryString["page"].ToString();
                                    if (int.TryParse(str_start, out start))
                                    {
                                        this.page = start;
                                    }
                                    else
                                    {
                                        this.page = 1;
                                    }
                                }
                                //Page is not set, return the first page.
                                else
                                {
                                    this.page = 1;
                                }
                                string requestedDir = String.Empty;
                                switch (RequestedFolder)
                                {
                                    case "comic":
                                        requestedDir = ApiConfiguration.ComicFolder;
                                        break;
                                    case "file":
                                        requestedDir = ApiConfiguration.FileFolder;
                                        break;
                                    case "media":
                                        requestedDir = ApiConfiguration.MediaFolder;
                                        break;
                                    case "image":
                                        requestedDir = ApiConfiguration.ImageFolder;
                                        break;
                                    case "music":
                                        requestedDir = ApiConfiguration.MusicFolder;
                                        break;
                                    case "epub":
                                        requestedDir = ApiConfiguration.EpubFolder;
                                        break;
                                }
                                //Create a DirectoryInfo object using the requestedDir to create an XML directory listing.
                                DirectoryInfo dir = new DirectoryInfo(requestedDir);
                                XDocument XFolderResult = new XDocument(
                                    new XDeclaration("1.0", "utf-8", "yes"),
                                    FolderCrawler.FolderCrawler.GetDirectoryXml(dir, dir, this.pageLimit, this.page));
                                
                                //If the parameter JSON was provided, output formatted in JSON, else in XML.
                                if (httpContext.Request.QueryString["json"] != null)
                                {
                                    var xmlFolderResult = XFolderResult.ToXmlDocument();
                                    string jsonFolderResult = JsonConvert.SerializeXmlNode(xmlFolderResult);

                                    //Write the resulting JSON to the response.
                                    httpContext.Response.ContentType = "Application/json";
                                    httpContext.Response.Write(jsonFolderResult);
                                }
                                else
                                {
                                    //Write the resulting XML to the response.
                                    httpContext.Response.ContentType = "Application/xml";
                                    httpContext.Response.Write(XFolderResult);                                    
                                }
                                break;
                            }
                            catch
                            {
                                break;
                            }
                        }
                }
            }
            //If no querystring was present, we pass back the defaultFileType and defaultFile set in the configuration.
            //The call will always result in a valid Http response.
            else
            {
                httpContext.Response.ContentType = ApiConfiguration.DefaultFileType;
                httpContext.Response.WriteFile(ApiConfiguration.DefaultFile);
            }
        }

        //private ApiConfiguration configuration { set; get; }
        readonly HttpRequest httpRequest;
        readonly HttpContext httpContext;
        public bool RequestedfileExists { private set; get; }
        public FileInfo Requestedfile { private set; get; }
        public string RequestedfileType { private set; get; }
        public string RequestedfileName { private set; get; }
        public string RequestedFolder { private set; get; }
        public int pageLimit { private set; get; }
        public int page { private set; get; }
        public HttpResponse response { private set; get; }

    }


}