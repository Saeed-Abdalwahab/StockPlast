using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StockPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
            name: "Reports-view|export",
            url: "Reports/{action}",
            defaults: new { controller = "Reports", action = "Index" },
            constraints: new { action = "viewreport|exportreport" }
        );

            routes.MapRoute(
                name: "Reports",
                url: "Reports/{id}",
                defaults: new { controller = "Reports", action = "Index" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
