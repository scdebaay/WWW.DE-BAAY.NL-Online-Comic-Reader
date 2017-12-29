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

        public ActionResult Read(string folder, string file, string page, string size)
        {
            Comic comic = new Comic();
            comic.requestedFolder = folder;
            comic.requestedFile = file;
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

            string pageRequest = "&page=" + comic.page.ToString();
            string imageSourceParams = pageRequest + "&size=" + comic.size;
            ViewBag.ImageSource = config.ApiLocation + "?file=" + comic.requestedFolder + "/" + @comic.requestedFile + imageSourceParams;
            return View("Comic", comic);
        }
    }
}
