using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComicReaderWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Comic", // Route name
                "Comic/Read/{folder}/{file}/{page}", //{*size}", // URL with parameters
                new
                {
                    controller = "Comic",
                    action = "Read",
                    folder = "",
                    file = "",
                    page = "0"//,
                    //size = UrlParameter.Optional
                } // Parameter defaults
            );

            routes.MapRoute(
                "Home", // Route name
                "Home/Index/{page}", // URL with parameters
                new { controller = "Home", action = "Index", page = "1" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            Context.Items["IIS_WasUrlRewritten"] = "false";
        }
    }
}