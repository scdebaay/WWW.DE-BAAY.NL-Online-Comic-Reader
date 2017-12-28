using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComicReaderWeb.Models;

namespace ComicReaderWeb.Controllers
{
    public class ComicController : Controller
    {
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);
        //
        // GET: /Comic/
        public ActionResult Comic(string file)
        {
            Comic comic = new Comic();
            comic.requestedfile = file;
            string pageRequest = comic.pageparam + comic.page.ToString();
            string imageSourceParams = pageRequest + comic.sizeparam + comic.size;
            ViewBag.ImageSource = config.ApiLocation + "?file=" + @comic.requestedfile + imageSourceParams;
            return View("Comic", comic);
        }

        [HttpGet]
        public ActionResult ComicPrevious(Comic comic)
        {
            comic.page--;
            string pageRequest = comic.pageparam + comic.page.ToString();
            string imageSourceParams = pageRequest + comic.sizeparam + comic.size;
            ViewBag.ImageSource = config.ApiLocation + "?file=" + @comic.requestedfile + imageSourceParams;
            return View("Comic", comic);
        }

        [HttpGet]
        public ActionResult ComicNext(Comic comic)
        {
            comic.page++;
            string pageRequest = comic.pageparam + comic.page.ToString();
            string imageSourceParams = pageRequest + comic.sizeparam + comic.size;
            ViewBag.ImageSource = config.ApiLocation + "?file=" + @comic.requestedfile + imageSourceParams;
            return View("Comic", comic);
        }

    }
}
