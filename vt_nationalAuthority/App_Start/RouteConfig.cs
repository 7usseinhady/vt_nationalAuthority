using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace vt_nationalAuthority
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Process", action = "vProcessIndex", id = UrlParameter.Optional }
                defaults: new { controller = "Login", action = "vLoginIndex", id = UrlParameter.Optional },
                   namespaces: new[] { "vt_nationalAuthority.Controllers" }
            );
        }
    }
}
