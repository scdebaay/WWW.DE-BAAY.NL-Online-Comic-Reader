
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
            if (Session["currentPageKey"] == null)
            {
                Session["currentPageKey"] = 1;
            }
            else
            {
                if (page == null) //returning from Comic to current Page, page is noet set, thus null
                {
                    int currentPageJump;
                    System.Int32.TryParse(Session["currentPageKey"].ToString(), out currentPageJump);
                    page = currentPageJump;
                }
                else
                {
                    Session["currentPageKey"] = page;
                }                
            }
            int currentPage;
            System.Int32.TryParse(Session["currentPageKey"].ToString(), out currentPage);
            int newPage = page ?? 1;
            if (currentPage > newPage && currentPage >= 1)
            {
                currentPage--;
                Session["currentPageKey"] = currentPage;
            }
            if (currentPage < newPage)
            {
                currentPage++;
                Session["currentPageKey"] = currentPage;
            }
            ComicPage comicpage = new ComicPage(config, currentPage);
            ViewBag.Table = comicpage.ResultTables;
            return View("Index", comicpage);
        }

    }
}
