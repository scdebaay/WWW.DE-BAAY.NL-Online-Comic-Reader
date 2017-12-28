using System;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Web.Hosting;
using System.Xml.Serialization;

namespace ComicReaderWeb.Models
{
    public class ComicPage
    {
        public ComicPage(WebConfiguration config) : this(config, 1)
        {}

        public ComicPage(WebConfiguration config, int? current)
        {
            this.currentPage = (int)current;
            this.conf = config;
            XDocument request = XDocument.Load(config.ApiLocation + "?folder=comic&page=1&pageLimit=" + config.pageLimit);

            int files;
            Int32.TryParse(request.Root.Attribute("files").Value.ToString(), out files);
            this.Files = files;

            int pages;
            Int32.TryParse(request.Root.Attribute("totalPages").Value.ToString(), out pages);
            this.totalPages = pages;

            int pageLimitInt;
            Int32.TryParse(config.pageLimit, out pageLimitInt);
            this.pageLimit = pageLimitInt;

            request = XDocument.Load(conf.ApiLocation + "?folder=comic&page=1&pageLimit=" + (this.Files + 1));
            XElement tempresult;
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
            argList.AddParam("requestUrl", "", conf.SiteRoot);
            argList.AddParam("apiLocation", "", conf.ApiLocation);
            XPathDocument comicList = new XPathDocument(result);

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