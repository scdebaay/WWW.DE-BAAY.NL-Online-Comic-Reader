using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using ComicReaderWeb.Models;
using System.Dynamic;

namespace ComicReaderWeb.Controllers
{
    public class HomeController : Controller
    {
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);
        //
        // GET: /Home/
        public ActionResult Index(int? page)
        {
            var currentPage = page ?? 1;

            ComicPage comicpage = new ComicPage(config, currentPage);

            ViewBag.Table = comicpage.ResultTables;
            return View("Index", comicpage);
        }

        [HttpGet]
        public ActionResult IndexPrevious(int? page)
        {
            if (page >= 1)
            {
                page--;
            }            
            ComicPage comicpage = new ComicPage(config, page);
            ViewBag.Table = comicpage.ResultTables;
            return View("Index", comicpage);
        }

        [HttpGet]
        public ActionResult IndexNext(int? page)
        {
            //if (page <= comicpage.totalPages)
            //{
                page++;
            //}
            ComicPage comicpage = new ComicPage(config, page);
            ViewBag.Table = comicpage.ResultTables;
            return View("Index", comicpage);
        }

    }
}
