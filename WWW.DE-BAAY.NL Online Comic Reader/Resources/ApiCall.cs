using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.IO;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine;
using FolderCrawler;
using System.Xml.Linq;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiCall
    {
        public ApiCall(HttpContext context)
        {
            this.httpContext = context;
            this.httpRequest = httpContext.Request;
            this.configuration = new ApiConfiguration(context);
        }

        public void ProcessRequest()
        {
            if (httpContext.Request.QueryString.Count != 0)
            {
                string request = httpContext.Request.QueryString.AllKeys[0].ToString();
                switch (request)
                {
                    case "file":
                        try
                        {
                            this.RequestedfileName = httpContext.Request.QueryString["file"];
                            this.RequestedfileExists = false;
                            this.Requestedfile = new FileInfo(configuration.serverPath + configuration.comicFolder + RequestedfileName);
                            File.Exists(Requestedfile.FullName);
                            this.RequestedfileExists = true;
                            Utility.DetermineRequestedFileType(httpContext, RequestedfileName);
                            switch (httpContext.Response.ContentType)
                            {
                                case "Application/x-cbr":
                                    FetchComic responseComic = new FetchComic(RequestedfileName, httpContext, configuration);
                                    httpContext.Response.Clear();
                                    httpContext.Response.ClearContent();
                                    httpContext.Response.ClearHeaders();
                                    httpContext.Response.Buffer = true;
                                    httpContext.Response.ContentType = responseComic.PageType;
                                    httpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                                    httpContext.Response.Cache.SetNoStore();
                                    //httpContext.Response.AddHeader("Content-Length", responseComic.result.GetLength(0).ToString());
                                    //httpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + responseComic.PageName);
                                    //httpContext.Response.AddHeader("Pragma", "public");
                                    httpContext.Response.BinaryWrite(responseComic.result);
                                    httpContext.Response.Flush();
                                    httpContext.Response.End();
                                    break;
                                case "Image/png":
                                case "Image/jpg":
                                case "Image/gif":
                                    FetchImage responseImage = new FetchImage(RequestedfileName, httpContext, configuration);
                                    httpContext.Response.Clear();
                                    httpContext.Response.ClearContent();
                                    httpContext.Response.ClearHeaders();
                                    httpContext.Response.Buffer = true;
                                    httpContext.Response.ContentType = responseImage.ImageType;
                                    httpContext.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                                    httpContext.Response.Cache.SetNoStore();
                                    //httpContext.Response.AddHeader("Content-Length", responseComic.result.GetLength(0).ToString());
                                    //httpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + responseComic.PageName);
                                    //httpContext.Response.AddHeader("Pragma", "public");
                                    httpContext.Response.BinaryWrite(responseImage.result);
                                    httpContext.Response.Flush();
                                    httpContext.Response.End();
                                    break;

                                default:
                                    httpContext.Response.ContentType = configuration.defaultFileType;
                                    httpContext.Response.WriteFile(configuration.defaultFile);
                                    break;
                            }

                        }
                        catch
                        {
                            break;
                        }
                        break;

                    case "folder":
                        {
                            try
                            {
                                this.RequestedFolder = httpContext.Request.QueryString["folder"].ToString();
                                if (httpContext.Request.QueryString["pageLimit"] != null)
                                {
                                    int limit;
                                    string str_limit = httpContext.Request.QueryString["pageLimit"].ToString();
                                    if (int.TryParse(str_limit, out limit))
                                    {
                                        this.pageLimit = limit;
                                    }
                                    else
                                    {
                                        this.pageLimit = (int)configuration.pageLimit.ToInt();
                                    }

                                }
                                else
                                {
                                    this.pageLimit = (int)configuration.pageLimit.ToInt();
                                }
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
                                else
                                {
                                    this.page = 1;
                                }
                                string requestedDir = String.Empty;
                                switch (RequestedFolder)
                                {
                                    case "comic":
                                        requestedDir = configuration.comicFolder;
                                        break;
                                    case "file":
                                        requestedDir = configuration.fileFolder;
                                        break;
                                    case "media":
                                        requestedDir = configuration.mediaFolder;
                                        break;
                                    case "image":
                                        requestedDir = configuration.imageFolder;
                                        break;
                                    case "music":
                                        requestedDir = configuration.musicFolder;
                                        break;
                                    case "epub":
                                        requestedDir = configuration.epubFolder;
                                        break;
                                }
                                DirectoryInfo dir = new DirectoryInfo(requestedDir);
                                XDocument xmlFolderResult = new XDocument(FolderCrawler.FolderCrawler.GetDirectoryXml(dir, dir, this.pageLimit, this.page));
                                httpContext.Response.ContentType = "Application/xml";
                                httpContext.Response.Write(xmlFolderResult);
                                break;
                            }
                            catch
                            {
                                break;
                            }
                        }
                }
            }
            else
            {
                httpContext.Response.ContentType = this.configuration.defaultFileType;
                httpContext.Response.WriteFile(this.configuration.defaultFile);
            }
        }

        private ApiConfiguration configuration { set; get; }
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