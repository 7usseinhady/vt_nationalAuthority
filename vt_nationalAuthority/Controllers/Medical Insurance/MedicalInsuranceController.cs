using System.Collections.Generic;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Medical_Insurance
{
    public class MedicalInsuranceController : Controller
    {
        // GET: MedicalInsurance
        public MedicalInsuranceController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        public ActionResult vMedicalInsuranceIndex()
        {
            return View();
        }
        public PartialViewResult _vpMedicalInsuranceIndex()
        {
            //viewBags();
            return PartialView();
        }
        public PartialViewResult _vpMedicalInsuranceSearch()
        {
            viewBags();
            return PartialView();
        }
        public PartialViewResult _vpMedicalInsuranceSendToInsurance()
        {
            viewBags();
            return PartialView();
        }
        // المهارات السابقه
        public PartialViewResult _vpMedicalInsuranceLastLevels()
        {
            return PartialView();
        }
        void viewBags()
        {
            ViewBag.healthStatus = new List<SelectListItem>
            {
                new SelectListItem{ Text="لائق", Value = "1" },
                new SelectListItem{ Text="غير لائق", Value = "2" }
            };
        }
    }
}