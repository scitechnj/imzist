using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Imzist.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Item",
                url: "{location}/Item/{id}",
                defaults: new { controller = "Item", action = "Index"}
                );

            routes.MapRoute(
                name: "Listings",
                url: "{location}/Listings/{category}",
                defaults: new { controller = "Listings", action = "Index", category = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{location}/{controller}/{action}/{id}",
                defaults: new { location="", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}