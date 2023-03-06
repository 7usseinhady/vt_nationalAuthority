using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vt_nationalAuthority.Controllers
{
    public class welcomeHomeController : Controller
    {
        // GET: welcomeHome
        // Default Controller In ASP.Net MVC Project
        public ActionResult welcomeHomeIndex()
        {
            return View();
        }

    }
}
