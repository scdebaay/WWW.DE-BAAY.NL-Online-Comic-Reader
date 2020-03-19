using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ComicReaderWeb.Models
{
    public class ComicList
    {
        public ComicList(WebConfiguration config)
        {
            //Request a response of available comics from the API. pageLimit is read from the configuration and only the first page is requested.
            //We only need the root element, though, to read the available number of Comics and the number of pages avaiable.
            this.Request = XDocument.Load(config.ApiLocation + "?folder=comic&page=1&pageLimit=" + config.pageLimit);
            //Set the Files property to the number of files from the request.
            Int32.TryParse(this.Request.Root.Attribute("files").Value.ToString(), out int files);
            this.Files = files;
            //Set the totalPages property to the number of avaiable pages from the request.
            Int32.TryParse(this.Request.Root.Attribute("totalPages").Value.ToString(), out int pages);
            this.TotalPages = pages;
            //Set the local pageLimitInt to the number set in the config.
            Int32.TryParse(config.pageLimit, out int pageLimitInt);
            this.PageLimit = pageLimitInt;
            //Request a result from the API condensed in 1 document.
            this.Request = XDocument.Load(config.ApiLocation + "?folder=comic&page=1&pageLimit=" + (this.Files + 1));
        }

        public int Files
        { private set; get; }
        public int TotalPages
        { private set; get; }
        public int PageLimit
        { private set; get; }
        public XDocument Request
        { private set; get; }
    }
}