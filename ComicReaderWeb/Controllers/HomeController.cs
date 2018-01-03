
using ComicReaderWeb.Models;
using System.Web.Mvc;

namespace ComicReaderWeb.Controllers
{
    public class HomeController : Controller
    {
        //Initialize application by creating a configuration object.
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);
        //
        // GET: /Home/
        public ActionResult Index(int? page)
        {
            //Initialize currentPage int. Set to 1, but 1 is not influential.
            int currentPage = 1;
            //Check whether the Session key currentPageKey is set. If it is not, set to 1, then read it back into currentPage property.
            if (Session["currentPageKey"] == null)
            {
                Session["currentPageKey"] = 1;
            }
            else
            {
                if (page == null) //returning from Comic to current Page, page is not set, thus null
                {
                    System.Int32.TryParse(Session["currentPageKey"].ToString(), out currentPage);
                }
                else //page is set, so, update the currentPageKey and then read it back into the currentPage property.
                {
                    Session["currentPageKey"] = page;
                    System.Int32.TryParse(Session["currentPageKey"].ToString(), out currentPage);
                }                
            }
            //Call the ComicPage model using the configuration object and the currentPage property.
            ComicPage comicpage = new ComicPage(config, currentPage);
            //Pass the ComicPage object result table, containing a partial html div detailing available comics
            ViewBag.Table = comicpage.ResultTables;
            //return the view and the ComicPage object to the view. ComicPage object is needed in view to pass current object parameters as currentPage.
            return View("Index", comicpage);
        }

    }
}
