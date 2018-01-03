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
    public class ComicPage
    {
        public ComicPage(WebConfiguration config) : this(config, 1) 
            //Initialize ComicPage object using the current configuration and currentPage parameter set to 1 if it is not passed to the constructor.
        {}

        public ComicPage(WebConfiguration config, int? current)
        {
            //Request a response of available comics from the API. pageLimit is read from the configuration and only the first page is requested.
            //We only need the root element, though, to read the available number of Comics and the number of pages avaiable.
            this.currentPage = (int)current;
            this.conf = config;
            XDocument request = XDocument.Load(config.ApiLocation + "?folder=comic&page=1&pageLimit=" + config.pageLimit);
            //Set the Files property to the number of files from the request.
            int files;
            Int32.TryParse(request.Root.Attribute("files").Value.ToString(), out files);
            this.Files = files;
            //Set the totalPages property to the number of avaiable pages from the request.
            int pages;
            Int32.TryParse(request.Root.Attribute("totalPages").Value.ToString(), out pages);
            this.totalPages = pages;
            //Set the local pageLimitInt to the number set in the config.
            int pageLimitInt;
            Int32.TryParse(config.pageLimit, out pageLimitInt);
            this.pageLimit = pageLimitInt;
            //Request a result from the API condensed in 1 document.
            request = XDocument.Load(conf.ApiLocation + "?folder=comic&page=1&pageLimit=" + (this.Files + 1));
            //Initialize a temporary result using the request as one page, the configured pageLimit and the currentPage.
            XElement tempresult;
            //Stop browsing past the maximum number of available pages
            if (this.currentPage <= pages)
            {
                tempresult = preResult(request, pageLimit, this.currentPage);
            }
            else
            {
                tempresult = preResult(request, pageLimit, pages);
            }

            //convert XDocument to TextReader for conversion into XPathDocument
            TextReader result = new StringReader(tempresult.ToString());
            //Prepare the result table
            StringWriter resultTable = new StringWriter();
            XsltArgumentList argList = new XsltArgumentList();
            //Pass requestUrl and apiLocation as parameters to the XslStylesheet. Needed to build links to the actual Comics
            argList.AddParam("requestUrl", "", conf.SiteRoot);
            argList.AddParam("apiLocation", "", conf.ApiLocation);
            //Build the comicList to pass to the XslStylesheet.
            XPathDocument comicList = new XPathDocument(result);
            //Transform the comicList and set ResultTables property with the result.
            XslCompiledTransform transform = new XslCompiledTransform();
            var trans = HostingEnvironment.MapPath(@"~\Views\Home\FolderTable.xslt");
            transform.Load(trans);
            transform.Transform(comicList, argList, resultTable);
            HtmlString resultTables = new HtmlString(resultTable.ToString());
            this.ResultTables = resultTables;
        }

        public int Files
        { private set; get; }
        public int currentPage
        { set; get; }
        //Derive previousPage property from currentPage and totalPages property. No page lower than 1.
        public int previousPage
        { get {
                if (this.currentPage - 1 >= 1)
                {
                    if (this.currentPage >= this.totalPages)
                    {
                        return this.totalPages - 1;
                    }
                    else
                    {
                        return this.currentPage - 1;
                    }
                    
                }
                else
                {
                    return 1;
                }
                
            } }
        //Derive nextPage property from currentPage and totalPages property. No page higher than totalPages.
        public int nextPage
        { get {
                if (this.currentPage + 1 >= this.totalPages)
                {
                    return this.totalPages;
                }
                else
                {
                    return this.currentPage + 1;
                }
            } }
        public int totalPages
        { private set; get; }
        public int pageLimit
        { private set; get; }
        public WebConfiguration conf
        { set; get; }
        public HtmlString ResultTables
        { private set; get; }

        //Parse the result with pageLimit and currentPage parameters to enable paged browsing of the ComicPages.
        private XElement preResult(XDocument result, int pageLimit, int currentPage)
        {
            var preResult = new XElement("folder");
            XElement[] allFileElements = result.XPathSelectElements("//file").ToArray();
            XElement[] selectedFiles = allFileElements.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToArray();
            foreach (var file in selectedFiles)
            {
                preResult.Add(file);
            }
            return preResult;
        }
    }
}