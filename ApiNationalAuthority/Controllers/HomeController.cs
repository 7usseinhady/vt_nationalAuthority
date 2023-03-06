using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiNationalAuthority.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Index_()
        {
            ViewBag.Title = "Home Page";

            return View();
            //return View("Index");
        }
    }
}
