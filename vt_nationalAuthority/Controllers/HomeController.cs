using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using System;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.App_Code;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers
{
    public class HomeController : Controller
    {
        UserRequest OuserRequest = new UserRequest();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        att_wsdl_cls attendance_wsdl = new att_wsdl_cls();
        //Home
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public HomeController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");

        }
        /// <summary>
        /// Home Index View
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult Index()
        {
            return View();
        }
        // Default Function In Controller Asp.Net MVC
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        // Default Function In Controller Asp.Net MVC
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// Autherize For Logout
        /// </summary>
        /// <returns>Loging Page</returns>
        public ActionResult Autherize_logout()
        {
            return RedirectToAction("vLoginIndex", "Login");
        }
        /// <summary>
        /// Error Page
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// Page Of Change Password
        /// </summary>
        /// <param name="screen">Current Page Open</param>
        /// <returns>View Page</returns>
        public ActionResult _ChangePassword(string screen)
        {
            TempData["screen"] = screen;


            UserModel user = new UserModel();
            return PartialView(user);
        }
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="currentPassword">Current Password</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmNewPassword">Confirm For New Password</param>
        /// <returns>Message For Change</returns>
        public JsonResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                string password = db.users.FirstOrDefault(user => user.userCode == uc).password;

                if (password == currentPassword)
                {
                    OuserRequest.OModel = new DataAccessLayer.Models.UserModel();

                    if (newPassword != confirmNewPassword)
                        TempData["msg"] = generalVariables.passwordNotEqual;
                    else
                    {
                        OuserRequest.OModel.iUserCode = Convert.ToInt32(Session["uc"].ToString());
                        OuserRequest.OModel.sPassword = newPassword;
                        OuserRequest = conApi.connectionApiPost<UserRequest>("apiUsers", "PostChangeassword", OuserRequest, null);

                        if (OuserRequest.OModel.bIsEdit)
                            return Json("تم تغيير كلمه المرور بنجاح ", JsonRequestBehavior.AllowGet);

                        return Json("عفوا, لما يتم تغيير كلمه المرور , الرجاء المحاوله مره اخرى", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json("الرجاء جعل كلمه المرور الجديده تطابق تأكيد كلمه المرور الحالية", JsonRequestBehavior.AllowGet); return Json(TempData["msg"], JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}

