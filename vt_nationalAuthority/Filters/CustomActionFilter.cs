using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.Models;
using System.Windows;
using System.Web.UI;
using System.Web.Routing;
using DataAccessLayer;

namespace vt_nationalAuthority.Filters
{

    public class CustomActionFilter : ActionFilterAttribute
    {

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        public string function_code { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string permTempData { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //db.CheckModuleUserPermisiom(int.Parse(HttpContext.Current.Session["uc"].ToString()), 1).Count
           //if (db.CheckModuleUserPermisiom(7, 1).Count <= 0)
           //{
           //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = Controller, action = Action }));
           //    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
           //    filterContext.Controller.TempData.Remove("perm");
           //    filterContext.Controller.TempData.Add("perm",  clsop.permission );
           //}
        }
    }

}