using Newtonsoft.Json;
using Resources;
using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Xml.Linq;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiCall
    {
        //Start logger
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Constructor: HttpContext object is fed in as a parameter and split into httpContext and Request.
        /// Context is used to call configuration object using the context parameters.
        /// </summary>
        /// <param name="context">HttpContext oject passed on from the initial Fetcher object.</param>
        public ApiCall(HttpContext context)
        {
            httpContext = context;
            httpRequest = context.Request;
        }

        /// <summary>
        /// After the ApiCall object is created, the request can be processed. 
        /// No parameters needed as all that is needed is created on initialization of the ApiCall object.
        /// </summary>
        public void ProcessRequest()
        {
            //If a querystring was passed to the Fetcher object, it was passed into the ApiCall and we try to process it.
            if (httpRequest.QueryString.HasKeys())
            {
                //The querystring should be formatted as either:
                //?file=<filepath><filename>&further parameters
                //or
                //?folder=<folderpath>&further parameters.
                //These two types of requests determine further processing. The first querystring key is converted to one string.
                string request = httpRequest.QueryString.AllKeys[0].ToString();

                switch (request)
                {
                    //First querystring key is "file", a file is requested.
                    case "file":
                        try
                        {
                            //Ascertain whether the requested file exists.
                            RequestedfileName = httpRequest.QueryString["file"];
                            RequestedfileExists = false;
                            Requestedfile = new FileInfo($"{ApiConfiguration.ServerPath(RequestedfileName)}{ApiConfiguration.ComicFolder}{RequestedfileName}");
                            RequestedfileExists = File.Exists(Requestedfile.FullName);
                            //Attempt to determine the requested filetype based on extension. The type is set in the response.
                            //This should allow the browser to handle filetype accordingly.
                            httpContext.Response.ContentType = Utility.DetermineRequestedFileType(RequestedfileName);
                            if (RequestedfileExists)
                            {
                                switch (httpContext.Response.ContentType)
                                {
                                    //Trying to return the filetype as CBR, to be able to use response within CBR/CBZ aware apps.
                                    case "Application/x-cbr":
                                        //Grab essential parameters from the querystring to fetch the comic page
                                        int? page = httpRequest.QueryString["page"].ToInt();
                                        int? size = httpRequest.QueryString["size"].ToInt();
                                        //Create a FetchComic object which contains the requested page in byte format.
                                        FetchedComic responseComic = new FetchedComic(RequestedfileName, page, size);
                                        SendResponse(responseComic.result, responseComic.PageType);
                                        break;
                                    //If an image of type png, jpg or gif was requested, fetch the image and write it to the repsonse stream.
                                    case "Image/png":
                                    case "Image/jpg":
                                    case "Image/gif":
                                        int? imagesize = httpRequest.QueryString["size"].ToInt();
                                        //Create a FetchImage object which contains the requested image in byte format.
                                        FetchedImage responseImage = new FetchedImage(RequestedfileName, imagesize);
                                        SendResponse(responseImage.result, responseImage.ImageType);
                                        break;
                                    //If all else fails, the request the defaultFileType and defaultFile 
                                    //from the app configuration and pass it to the response
                                    default:
                                        httpContext.Response.ContentType = ApiConfiguration.DefaultFileType;
                                        httpContext.Response.WriteFile(ApiConfiguration.DefaultFile);
                                        break;
                                }
                            }
                            else
                            {
                                Logger.Error($"File does not exist {RequestedfileName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            //Debug.WriteLine($"An error occurred in file processing: {ex.Message}");
                            //Debug.WriteLine($"StackTeace: {ex.StackTrace}");
                            Logger.Error($"An error occurred in file processing: {ex.Message}");
                            Logger.Error($"StackTeace: {ex.StackTrace}");
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
                                RequestedFolder = httpRequest.QueryString["folder"].ToString();
                                //pageLimit is set. Return the amount of files in the listing per page as requested.
                                if (httpRequest.QueryString["pageLimit"] != null)
                                {
                                    int limit;
                                    string str_limit = httpRequest.QueryString["pageLimit"].ToString();
                                    //If the pageLimit cannot be parsed into an int, then return the app configured value.
                                    //There must always be a response.
                                    if (int.TryParse(str_limit, out limit))
                                    {
                                        pageLimit = limit;
                                    }
                                    else
                                    {
                                        pageLimit = ApiConfiguration.PageLimit;
                                    }
                                }
                                //PageLimit is not set, return the amount of items per page as configured in app settings.
                                else
                                {
                                    pageLimit = ApiConfiguration.PageLimit;
                                }
                                //Page is set, return the requested page, if the value can be parsed into an int.
                                //Otherwise, send the first page.
                                if (httpRequest.QueryString["page"] != null)
                                {
                                    int start = 0;
                                    string str_start = httpRequest.QueryString["page"].ToString();
                                    if (int.TryParse(str_start, out start))
                                    {
                                        page = start;
                                    }
                                    else
                                    {
                                        page = 1;
                                    }
                                }
                                //Page is not set, return the first page.
                                else
                                {
                                    page = 1;
                                }
                                string requestedDir = string.Empty;
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
                                    FolderCrawler.GetDirectoryXml(dir, dir, pageLimit, page));

                                //If the parameter JSON was provided, output formatted in JSON, else in XML.
                                if (httpRequest.QueryString["json"] != null)
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
                            catch (Exception ex)
                            {
                                //Debug.WriteLine($"An error occurred in folder processing: {ex.Message}");
                                //Debug.WriteLine($"StackTeace: {ex.StackTrace}");
                                Logger.Error($"An error occurred in folder processing: {ex.Message}");
                                Logger.Error($"StackTeace: {ex.StackTrace}");
                                break;
                            }
                        }
                    case "action":
                        {
                            if (httpRequest.QueryString["action"] == "version")
                            {
                                httpContext.Response.Write($"API Version: {typeof(ApiCall).Assembly.GetName().Name} version {typeof(ApiCall).Assembly.GetName().Version}");
                            }                            
                            break;
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

        //Private function that takes in a bytearray and a filetype and sends that response to the browser.
        private void SendResponse(byte[] processResult, string fileType)
        {
            httpContext.Response.Clear();
            httpContext.Response.ClearContent();
            httpContext.Response.ClearHeaders();
            httpContext.Response.Buffer = true;
            httpContext.Response.ContentType = fileType;
            httpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            httpContext.Response.Cache.SetNoStore();
            httpContext.Response.BinaryWrite(processResult); //Write the resulting page to the response stream.
            httpContext.Response.Flush();
            httpContext.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            httpContext.ApplicationInstance.CompleteRequest(); //Better than using.End(). Avoids Exception.
        }

        private readonly HttpRequest httpRequest;
        private readonly HttpContext httpContext;
        private bool RequestedfileExists { set; get; }
        private FileInfo Requestedfile { set; get; }
        private string RequestedfileType { set; get; }
        private string RequestedfileName { set; get; }
        private string RequestedFolder { set; get; }
        private int pageLimit { set; get; }
        private int page { set; get; }
        private HttpResponse response { set; get; }
    }


}