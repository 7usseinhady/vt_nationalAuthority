using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;

namespace vt_nationalAuthority.Controllers
{
    public class LoginController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        // GET: Login
        /// <summary>
        /// Page Of Loging
        /// </summary>
        /// <returns>View Page</returns>
        [OutputCache(Duration = 60, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult vLoginIndex()
        {
            Session.Abandon();
            TempData["LoginErrorMessage"] = "";
            return View();
        }
        /// <summary>
        /// Autherization For Data Loging
        /// </summary>
        /// <param name="userModel">Data For User Loging</param>
        /// <returns>Check Permisions For User</returns>
        public ActionResult Autherize(UserModel userModel)
        {
            try
            {
                Session["checkLevel4"] = null;
                var userDetails = db.users.Where(user => user.userName == userModel.sUserName && user.password == userModel.sPassword).FirstOrDefault();
                if (userDetails == null)
                {
                    TempData["LoginErrorMessage"] = generalVariables.UserLoginFailed;
                    return View("vLoginIndex", userModel);
                }
                else if (userDetails.isActive == false)
                {
                    TempData["LoginErrorMessage"] = generalVariables.UserNotActive;
                    return View("vLoginIndex", userModel);
                }
                else if (userDetails.officeInsuranceCode == null && userDetails.contractorCode == null && userDetails.referenceSideCode == null) // هنا موظف الشركه بتاعتنا
                {
                    Session["uc"] = userDetails.userCode;
                    Session["UserName"] = userDetails.userName;
                    Response.Cookies["uc"].Value = userDetails.userCode.ToString();
                    return RedirectToAction("CompanyIndex", "Company");
                }
                else
                {
                    Session["uc"] = userDetails.userCode;
                    Session["UserName"] = userDetails.userName;
                    Response.Cookies["uc"].Value = userDetails.userCode.ToString();
                    return RedirectToAction("CheckUserPermission", new { UserCode = userDetails.userCode });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Check The Permisions For User
        /// </summary>
        /// <param name="UserCode">User Code</param>
        /// <returns>Own Page For User</returns>
        public ActionResult CheckUserPermission(int UserCode)
        {
            try
            {
                Session["areaOfficePermission"] = null;
                Session["AreaSearch"] = null;
                Session["contractor"] = null;

                // 0 => Not Muted || 1 => Muted
                Response.Cookies["isMuted"].Value = Request.Cookies["isMuted"] != null ? (Request.Cookies["isMuted"].Value == "" ? "0" : Request.Cookies["isMuted"].Value) : "0";

                var userDetails = db.spGetUserDetails(UserCode.ToString()).FirstOrDefault();

                if (userDetails != null)
                {

                    int uc = Convert.ToInt32(Session["uc"].ToString());

                    Session["oc"] = userDetails.officeInsuranceID;
                    Session["ac"] = userDetails.areaID;
                    Session["uImage"] = userDetails.imageURL;

                    // first check if user is admin

                    if (userDetails.isAdmin == true) // admin
                    {
                        Session["employee"] = 1;
                        Session["officeCode"] = db.users.FirstOrDefault(x => x.userCode == uc).officeInsuranceCode;
                        Session["areaOfficePermission"] = 1;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (userDetails.officeInsuranceCode != null) //موظف التأمينات
                    {
                        Session["employee"] = 1;
                        List<string> Offices = new List<string>();
                        Offices = db.GetAreaOfficePermission(UserCode).ToList();
                        if (!String.IsNullOrEmpty(Offices[0]))
                            Session["areaOfficePermission"] = Offices[0] + "," + userDetails.officeInsuranceCode;
                        else
                            Session["areaOfficePermission"] = userDetails.officeInsuranceCode;

                        Session["officeCode"] = db.users.FirstOrDefault(x => x.userCode == uc).officeInsuranceCode;
                        return RedirectToAction("vInsuranceEmployeeIndex", "InsuranceEmployee");
                    }
                    else if (userDetails.referenceSideCode != null || userDetails.contractorCode != null) //مقاول - جهة الاسناد
                    {
                        Session["officeCode"] = null;

                        if (userDetails.referenceSideCode != null) // جهه اسناد
                            Session["contractor"] = "جهه الاسناد : " + userDetails.refSideName;
                        else if (userDetails.contractorCode != null) // مقاول
                            Session["contractor"] = "مقاول : " + userDetails.contName;
                        return RedirectToAction("vContractorIndex", "Contractor", new { cp = 1 });
                    }
                    else
                        return RedirectToAction("vLoginIndex", "Login");
                }
                else
                    return RedirectToAction("vLoginIndex", "Login");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Logout Page
        /// </summary>
        /// <returns>Loging Page</returns>
        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
                return RedirectToAction("vLoginIndex", "Login");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}