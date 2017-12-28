using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Http;
using AttributeRouting;
using ComicReaderWeb.Models;


namespace ComicReaderWeb.Controllers
{
    public class ComicController : Controller
    {
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);

        public ActionResult Read(string folder, string file, string page, string size)
        {
            Comic comic = new Comic();
            comic.requestedfile = folder + "\\" + file;
            int pageInt;
            if (String.IsNullOrEmpty(page))
            {
                comic.page = 0;                
            }
            else
            {
                Int32.TryParse(page, out pageInt);
                comic.page = pageInt;
            }

            int sizeInt;
            if (!String.IsNullOrEmpty(size))
            {
                Int32.TryParse(size, out sizeInt);
                comic.size = sizeInt;
            }

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
