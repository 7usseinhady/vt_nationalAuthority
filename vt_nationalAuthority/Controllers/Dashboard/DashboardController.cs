using System;
using System.Collections.Generic;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Dashboard
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public DashboardController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        public ActionResult Index()
        {
            var extensions = new List<SelectListItem>
            {
              new SelectListItem{ Text="PDF", Value = "1" },
              new SelectListItem{ Text="Word", Value = "2" },
              new SelectListItem{ Text="XML", Value = "3" },
            };
            var list = new List<SelectListItem>{
                 new SelectListItem{ Text="ersaa nageh shahat", Value = "1" },
                 new SelectListItem{ Text="rehab hamdy youssef", Value = "2" },
                 new SelectListItem{ Text="ahmed maher Ali", Value = "3", Selected = true },
              };
            ViewBag.users = list;
            ViewBag.extensions = extensions;
            return View();
        }
    }
}