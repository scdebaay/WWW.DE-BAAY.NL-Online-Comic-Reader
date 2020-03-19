using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ComicReaderWeb.Models
{
    public class ComicPage
    {
        public ComicPage(WebConfiguration config, ComicList list) : this(config, list, 1) 
            //Initialize ComicPage object using the current configuration and currentPage parameter set to 1 if it is not passed to the constructor.
        {}

        public ComicPage(WebConfiguration config, ComicList list, int? current)
        {
            //Request a response of available comics from the API. pageLimit is read from the configuration and only the first page is requested.
            //We only need the root element, though, to read the available number of Comics and the number of pages avaiable.
            this.conf = config;
            this.currentPage = (int)current;
            this.comicList = list;
            this.Files = list.Files;
            this.totalPages = list.TotalPages;
            this.pageLimit = list.PageLimit;

            XElement tempresult;
            //Stop browsing past the maximum number of available pages
            if (this.currentPage <= this.totalPages)
            {
                tempresult = preResult(this.comicList.Request, this.pageLimit, this.currentPage);
            }
            else
            {
                tempresult = preResult(this.comicList.Request, this.pageLimit, this.totalPages);
            }

            //convert XDocument to TextReader for conversion into XPathDocument
            TextReader result = new StringReader(tempresult.ToString());
            //Prepare the result table
            StringWriter resultTable = new StringWriter();
            XsltArgumentList argList = new XsltArgumentList();
            //Pass requestUrl and apiLocation as parameters to the XslStylesheet. Needed to build links to the actual Comics
            argList.AddParam("requestUrl", "", config.SiteRoot);
            argList.AddParam("apiLocation", "", config.ApiLocation);
            //Build the comicList to pass to the XslStylesheet.
            XPathDocument XDoccomicList = new XPathDocument(result);
            //Transform the comicList and set ResultTables property with the result.
            XslCompiledTransform transform = new XslCompiledTransform(true);
            var trans = HostingEnvironment.MapPath(@"~\Views\Home\FolderTable.xslt");
            transform.Load(trans);
            transform.Transform(XDoccomicList, argList, resultTable);
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
        public HtmlString ResultTables
        { private set; get; }
        public ComicList comicList
        { get; set; }
        public WebConfiguration conf
        { get; private set; }

        //Parse the result with pageLimit and currentPage parameters to enable paged browsing of the ComicPages.
        private XElement preResult(XDocument result, int pageLimit, int currentPage)
        {
            //Extract all attribute values from incoming XDocument
            var preResultName = result.Root.Attribute("name").Value.ToString();
            var preResultFiles = result.Root.Attribute("files").Value.ToString();
            var preResultTotalPages = result.Root.Attribute("totalPages").Value.ToString();
            var preResultCurrentPage = result.Root.Attribute("currentPage").Value.ToString();
            var preResultNameSpace = result.Root.Attribute("xmlns").Value.ToString();
            //Define Namespace manager to be used to construct the XDocument to return to the caller
            XmlNamespaceManager r = new XmlNamespaceManager(new NameTable());
            r.AddNamespace("debaay", preResultNameSpace);

            //Construct the XDocument
            var preResult = new XElement("folder", 
                new XAttribute("name",preResultName),
                new XAttribute("files", preResultFiles),
                new XAttribute("totalPages", preResultTotalPages),
                new XAttribute("currentPage", preResultCurrentPage),
                new XAttribute(XNamespace.Xmlns + "debaay", preResultNameSpace));
            XElement[] allFileElements = result.XPathSelectElements("//debaay:file",r).ToArray();
            XElement[] selectedFiles = allFileElements.Skip((currentPage - 1) * pageLimit).Take(pageLimit).ToArray();
            foreach (var file in selectedFiles)
            {
                preResult.Add(file);
            }
            //Return the result
            return preResult;
        }
    }
}