using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace vt_nationalAuthority
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string connString = ConfigurationManager.ConnectionStrings["vt_authorityInsuranceConnection"].ConnectionString;
        /// <summary>
        /// Function Run When Application Start
        /// </summary>
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Start SqlDependency with application initialization
            SqlDependency.Start(connString);
        }
        /// <summary>
        /// Function Run When Application End
        /// </summary>
        protected void Application_End()
        {
            //Free the dependency
            SqlDependency.Stop(connString);
        }
    }
}
