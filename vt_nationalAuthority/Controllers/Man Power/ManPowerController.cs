using System.Collections.Generic;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Man_Power
{
    public class ManPowerController : Controller
    {
        // GET: ManPower
        public ManPowerController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        public ActionResult vManPowerIndex()
        {
            return View();
        }
        public PartialViewResult _vpManPowerIndex()
        {
            return PartialView();
        }
        public PartialViewResult _vpManPowerSearch()
        {
            viewBags();
            return PartialView();
        }
        public PartialViewResult _vpManPowerSendToInsurance()
        {
            viewBags();
            return PartialView();
        }
        // المهارات السابقه
        public PartialViewResult _vpManPowerLastLevels()
        {
            return PartialView();
        }
        void viewBags()
        {
            ViewBag.Jops = new List<SelectListItem>
            {
                new SelectListItem{ Text="نجار", Value = "1" },
                new SelectListItem{ Text="سباك", Value = "2" },
                new SelectListItem{ Text="محار", Value = "3"},
            };
            ViewBag.MahrDegree = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="محدود", Value = "1" },
                new SelectListItem{ Text="متوسط", Value = "2" },
                new SelectListItem{ Text="ماهر", Value = "3"}}, "Value", "Text", 1);
            ViewBag.MedicalInsurance = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="لائق", Value = "1" },
                new SelectListItem{ Text="معلق", Value = "2" },
                new SelectListItem{ Text="اصابه جزئيه", Value = "3"},
                new SelectListItem{ Text="اصابه كليه", Value = "2" }, }, "Value", "Text", 1);
        }
    }
}