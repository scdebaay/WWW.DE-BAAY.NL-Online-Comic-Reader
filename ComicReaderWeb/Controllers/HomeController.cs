
using System.Web.Mvc;
using ComicReaderWeb.Models;

namespace ComicReaderWeb.Controllers
{
    public class HomeController : Controller
    {
        WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);
        //
        // GET: /Home/
        public ActionResult Index(int? page)
        {
            int currentPage = 1;
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
                else
                {
                    Session["currentPageKey"] = page;
                    System.Int32.TryParse(Session["currentPageKey"].ToString(), out currentPage);
                }                
            }
            ComicPage comicpage = new ComicPage(config, currentPage);
            ViewBag.Table = comicpage.ResultTables;
            return View("Index", comicpage);
        }

    }
}
