using ComicReaderWeb.Models;
using System;
using System.Web.Mvc;


namespace ComicReaderWeb.Controllers
{
    public class ComicController : Controller
    {
        //Initialize ComicController by creating a configuration object. We need it to determine the default size for any comic to be displayed.
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);

        //Get Comic/Read with parameters folder, file, page and size.
        public ActionResult Read(string folder, string file, string page, string size)
        {
            //Create a new Comic object based on the Comic Model using the acquired parameters.
            Comic comic = new Comic();
            //Set ComicPage object properties to incoming parameters.
            comic.requestedFolder = folder;
            comic.requestedFile = file;
            //Initialize an integer to put the page parameter in.
            int pageInt;
            //If page is not passed to the controller, we are assuming we want to see the coverpage, this set to 0.
            if (String.IsNullOrEmpty(page))
            {
                comic.page = 0;
            }
            else //page is passed to the controller, so we request the specified page.
            {
                Int32.TryParse(page, out pageInt);
                comic.page = pageInt;
            }

            //If size is not passed to the controller try to read it from the Session and it that fails, read it from the configuration.
            if (String.IsNullOrEmpty(size))
            {
                if (Session["currentSizeKey"] == null)//Session key is empty, read it from config.
                {
                    Session["currentSizeKey"] = config.defaultComicSize; //Set Session Key.
                }
                size = Session["currentSizeKey"].ToString(); //Read current size from Session.
                int sizeInt; //Initialize an integer to parse the size into.
                Int32.TryParse(size, out sizeInt);
                comic.size = sizeInt;
            }
            else //Size was passed as a parameter. Session needs updating.
            {
                int sizeInt; //Initialize an integer to parse the size into.
                Int32.TryParse(size, out sizeInt);
                comic.size = sizeInt;
                Session["currentSizeKey"] = comic.size;
            }
            
            string pageRequestParams = "&page=" + comic.page.ToString() + "&size=" + comic.size; //Build the pageRequest to be passed to the API.
            //Set the ImageSource property to the API location requesting the Comic with current page and size parameters.
            ViewBag.ImageSource = config.ApiLocation + "?file=" + comic.requestedFolder + "/" + comic.requestedFile + pageRequestParams;
            return View("Comic", comic); //Also pass Comic model to view to have all paramters available.
        }
    }
}
