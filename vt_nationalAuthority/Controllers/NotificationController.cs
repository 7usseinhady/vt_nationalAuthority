using DataAccessLayer;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vt_nationalAuthority.Controllers
{
    public class NotificationController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static int? insPageNumber;
        // GET: Notification
        /// <summary>
        /// Get More For Notifications
        /// </summary>
        /// <param name="inPage">Current Page Number</param>
        /// <param name="formCollection">Data Need For Search About Notifications</param>
        /// <returns>List Of Notifications </returns>
        public ActionResult MoreNotication(int? inPage,FormCollection formCollection)
        {
            try
            {
                insPageNumber = inPage;
                int? user_code = int.Parse(Session["uc"].ToString());
                var model = new List<GetNotifications_Result>();
                if(formCollection.Count == 0)
                    model = db.GetNotifications(user_code,null, "0001-01-01").ToList();
                else
                {
                    string type = "";
                    string date = "";
                    if (formCollection["ddlSpecialScreen"] == null)
                        type = null;
                    else if (formCollection["ddlSpecialScreen"].ToString() == "2") // process
                        type = "5,9,12";
                    else if (formCollection["ddlSpecialScreen"].ToString() == "3") // process request
                        type = "4 , 13";
                    else if (formCollection["ddlSpecialScreen"].ToString() == "4") // process stop
                        type = "6,14";
                    else if (formCollection["ddlSpecialScreen"].ToString() == "5") // worker
                        type = "15";
                    else if (formCollection["ddlSpecialScreen"].ToString() == "6")
                        type = "16";
                    if (formCollection["ddlSpecialScreen"] == null)
                        date = "0001-01-01";
                    else if (!String.IsNullOrEmpty(formCollection["txtDate"].ToString()))
                        date = formCollection["txtDate"].ToString();
                    model = db.GetNotifications(user_code, type,date).ToList();
                }
                List<int> codes = new List<int> { 2, 3, 4, 5, 6 };
                ViewBag.SpecialScreen = new SelectList(db.CheckModuleUserPermisiom(user_code, 1).Where(x => codes.Contains(x.functionCode) ).ToList(), "functionCode", "functionName");
                return View(model.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}