using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ApiNationalAuthority
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index_", id = UrlParameter.Optional },
                 namespaces: new[] { "ApiNationalAuthority.Controllers" }
            //defaults: new { controller = "Home", action = "Index_", id = UrlParameter.Optional }
            );


            //            routes.MapRoute(
            //    "Default",
            //    "{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    new[] { "ApiNationalAuthority.Controllers" }
            //);
        }
    }
}
