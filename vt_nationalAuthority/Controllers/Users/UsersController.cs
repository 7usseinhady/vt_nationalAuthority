using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.Models;
using vt_nationalAuthority.SMS;

namespace vt_nationalAuthority.Controllers.Users
{
    public class UsersController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        GeneralMethods generalMethods = new GeneralMethods();
        ConnectionApi conApi = new ConnectionApi();
        static UserRequest oUserRequest = new UserRequest();
        // static userRequest Model;
        static bool bISSearch = false;
        static int? insPageNumber;
        int iPageSize = 18;
        FilesPDF file = new FilesPDF();
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public UsersController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Data Need In DropDown Lists
        /// </summary>
        void viewBags()
        {
            ViewBag.display = "none";


            ViewBag.FilterUserType = new SelectList(new List<SelectListItem>
            {   new SelectListItem{ Text=" ", Value = "0" }, }, "Value", "Text");
            ViewBag.OFFICE = new SelectList(db.officeInsurances.ToList(), "officeInsuranceCode", "officeInsuranceName");

            //ViewBag.FilterUserTypeList = new SelectList(new List<SelectListItem>
            //{   new SelectListItem{ Text=" ", Value = "0" }, }, "Value", "Text");

        }
        /// <summary>
        /// Page Of Users
        /// </summary>
        /// <param name="inPage">Current Number Page</param>
        /// <param name="IDDelete">User Code Want To Delete</param>
        /// <param name="Search">Check if Data For Search</param>
        /// <returns>View Page Of Users</returns>
        // GET: Users
        public ActionResult vUsersIndex(int? inPage, int? IDDelete, string Search, int? cp)
        {
            try
            {
                //if (Model == null)
                //    Model = new userRequest();
               // var oUserReq = new userRequest();
                TempData["done"] = 0;

                //insPageNumber = (inPage == null ? insPageNumber : inPage);
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                if (IDDelete != null) // Delete
                {
                    bISSearch = false;

                    oUserRequest = conApi.connectionApiDelete<UserRequest>("apiUsers", "DeleteUsers", IDDelete.ToString());
                    if (oUserRequest.bIsDeleted)
                    {
                        TempData["msg"] = generalVariables.DeleteDone;
                        if (oUserRequest.OModel.sImageURL != null)
                        {
                            string[] nestedFolders = { };
                            string[] fileName = oUserRequest.OModel.sImageURL.Split('/');
                            file.deleteFile("UserImage", nestedFolders, fileName[2]);
                        }
                    }
                    else
                        TempData["msg"] = generalVariables.DeleteNotDone;
                }

                if (cp == 1) // first click
                {
                    bISSearch = false;
                }

                if (!bISSearch)
                //    oUserReq.LModels = Model.LModels;
                //else
                {
                    int uc;
                    // Admin
                    if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                        uc = -1;
                    else
                        uc = Convert.ToInt32(Session["uc"].ToString());

                    oUserRequest = conApi.connectionApiGetList<UserRequest>("apiUsers", "GetUsersByUser", uc.ToString());
                }

                if (oUserRequest == null)
                {
                    oUserRequest = new UserRequest();
                    oUserRequest.LModels = new List<DataAccessLayer.Models.UserModel>();
                }

                if (oUserRequest.LModels == null)
                    oUserRequest.LModels = new List<DataAccessLayer.Models.UserModel>();
                return View(oUserRequest.LModels.ToPagedList(insPageNumber ?? 1, this.iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Page Of Save / Edit User
        /// </summary>
        /// <param name="ID">User Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vpUsersAdd(int? ID)
        {
            try
            {
                viewBags();

                ViewBag.legalEntity = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="منشأه", Value = "2" },
                new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", 2);

                UserRequest User = new UserRequest();

                // Edit 
                if (ID != null)
                {
                    User = conApi.connectionApiGetList<UserRequest>("apiUsers", "GetUserLegalEntity", ID.ToString());
                    if (User.OModel.iReferenceSideCode != null) // جهة اسناد
                    {
                        TempData["refSideContCode"] = User.OModel.oRefSideCont.iReferenceSideContractorCode;
                        TempData["refSideContName"] = User.OModel.oRefSideCont.sReferenceSideContractorName;
                        TempData["refSideContNum"] = User.OModel.oRefSideCont.sReferanceSideContractorNum;
                        ViewBag.legalEntity = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="منشأه", Value = "2" },
                new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", User.OModel.oRefSideCont.legalEntityCode);

                        ViewBag.display = "none";

                        // Admin
                        if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                            ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName", 2);
                        else
                            ViewBag.UsersTypes = new SelectList(db.userTypes.Where(x => x.userTypeCode != 1).ToList(), "userTypeCode", "userTypeName", 2);

                        //ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName", 2);
                        ViewBag.FilterUserType = new SelectList(db.spGetRefSideContractors().Where(x => x.referenceSideContractorCode != -1).ToList(), "referenceSideContractorCode", "referenceSideContractorName", User.OModel.iReferenceSideCode);
                    }
                    else if (User.OModel.iContractorCode != null) // المقاولين
                    {
                        TempData["refSideContCode"] = User.OModel.oRefSideCont.iReferenceSideContractorCode;
                        TempData["refSideContName"] = User.OModel.oRefSideCont.sReferenceSideContractorName;
                        TempData["refSideContNum"] = User.OModel.oRefSideCont.sReferanceSideContractorNum;
                        ViewBag.legalEntity = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="منشأه", Value = "2" },
                new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", User.OModel.oRefSideCont.legalEntityCode);

                        ViewBag.display = "none";

                        // Admin
                        if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                            ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName", 3);
                        else
                            ViewBag.UsersTypes = new SelectList(db.userTypes.Where(x => x.userTypeCode != 1).ToList(), "userTypeCode", "userTypeName", 3);

                        //ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName", 3);
                        ViewBag.FilterUserType = new SelectList(db.spGetRefSideContractors().Where(x => x.referenceSideContractorCode != -1).ToList(), "referenceSideContractorCode", "referenceSideContractorName", User.OModel.iContractorCode);
                    }
                    else if (User.OModel.iOfficeInsuranceCode != null) // موظف التأمينات
                    {
                        ViewBag.display = "flex";
                        ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName", 1);
                        int CODE = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == User.OModel.iOfficeInsuranceCode).areaCode;
                        ViewBag.FilterUserType = new SelectList(db.areas.ToList(), "areaCode", "areaName", CODE);
                        ViewBag.OFFICE = new SelectList(db.officeInsurances.ToList(), "officeInsuranceCode", "officeInsuranceName", User.OModel.iOfficeInsuranceCode);
                    }
                    return PartialView(User);
                }
                else // Insert
                {
                    ViewBag.display = "";

                    // Admin
                    if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                        ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName");
                    else
                        ViewBag.UsersTypes = new SelectList(db.userTypes.Where(x => x.userTypeCode != 1).ToList(), "userTypeCode", "userTypeName");

                    // ViewBag.UsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName");
                    return PartialView(User);
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save /  Edit Users
        /// </summary>
        /// <param name="oModalRequest">Data For User</param>
        /// <param name="formCollection">Other Data For User</param>
        /// <param name="ddlUserType">Area Code</param>
        /// <param name="insNumCode">Name Of Contrator Or Refrence Side</param>
        /// <param name="UserType">User Type Code</param>
        /// <param name="contractorCode">Contractor Code</param>
        /// <param name="uploadFile">Path Of User Image</param>
        /// <returns>View Page Of Users</returns>
        [HttpPost]
        public ActionResult _vpUsersAdd(UserRequest oModalRequest, FormCollection formCollection, string ddlUserType, string insNumCode, string UserType, string contractorCode, HttpPostedFileBase uploadFile)
        {
            try
            {
                bISSearch = false;

                //oModalRequest.OModel.iUserCode = Convert.ToInt32(Session["uc"].ToString());
                if (UserType == "2" || oModalRequest.OModel.iUserTypes == 2) //جهة اسناد
                {
                    oModalRequest.OModel.iReferenceSideCode = Convert.ToInt32(insNumCode);
                    oModalRequest.OModel.iContractorCode = null;
                    oModalRequest.OModel.iOfficeInsuranceCode = null;
                }
                else if (UserType == "3" || oModalRequest.OModel.iUserTypes == 3 || !String.IsNullOrEmpty(contractorCode)) //مقاول 
                {
                    oModalRequest.OModel.iContractorCode = !String.IsNullOrEmpty(insNumCode) ? Convert.ToInt32(insNumCode) : Convert.ToInt32(contractorCode);
                    oModalRequest.OModel.iOfficeInsuranceCode = null;
                    oModalRequest.OModel.iReferenceSideCode = null;
                }
                if (oModalRequest.OModel.iUserCode > 0) // Edit
                {
                    oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());

                    string id = oModalRequest.OModel.iUserCode.ToString();
                    oUserRequest = conApi.connectionApiPost<UserRequest>("apiUsers", "PostEditUsers", oModalRequest, id);
                    if (oUserRequest.bIsEdit)
                    {
                        TempData["msg"] = generalVariables.EditDone;
                    }
                    else
                        TempData["msg"] = generalVariables.EditNotDone;

                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        string[] nestedFolders = { };
                        string message = file.addFileWithoutNum("UserImage", nestedFolders, uploadFile, id.ToString(), file.images);

                        if (message == "تم الحفظ/Save File")
                        {
                            //TempData["msg"] = generalVariables.EditNotDone;
                            oModalRequest.OModel.sImageURL = ((uploadFile != null && uploadFile.ContentLength > 0) ? Path.GetExtension(uploadFile.FileName) : null);
                            oUserRequest = conApi.connectionApiPost<UserRequest>("apiUsers", "PostUpdateImage", oModalRequest, id);

                        }
                        else
                        {
                            TempData["msg"] = generalVariables.SaveWithoutImage;

                            //if (!oUserRequest.OModel.bIsEdit)
                            //    TempData["msg"] = generalVariables.SaveWithoutImage;
                        }
                    }
                }
                else // Save
                {
                    oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());

                    oUserRequest = conApi.connectionApiPost<UserRequest>("apiUsers", "PostSaveUsers", oModalRequest, null);
                    if (oUserRequest.bIsSaved)
                    {
                        TempData["msg"] = generalVariables.SaveDone;


                        if (!String.IsNullOrEmpty(oUserRequest.OModel.oUsers.phone1))
                        {
                            //send SMS to Worker
                            //#region SMS
                            //ServiceSoapClient sms = new ServiceSoapClient();
                            ////string message = "لقد تم تسجيلك على منظومه التأمين على عمال المقاولات المؤقتين " + " https://moqawlty.nosi.gov.eg Username: " + oModalRequest.OModel.sUserName + " Password: " + oModalRequest.OModel.sPassword;

                            //string message = oModalRequest.OModel.iOfficeInsuranceCode != null ? (" لقد تم تسجيلك على منظومه التأمين على عمال المقاولات المؤقتين كموظف تأمينات  " + " \n https://moqawlty.nosi.gov.eg \n UserName: " + oModalRequest.OModel.sUserName + " \n Password: " + oModalRequest.OModel.sPassword) : // موظف التأمينات
                            //                (oModalRequest.OModel.iContractorCode != null ? (" لقد تم تسجيلك على منظومه التأمين على عمال المقاولات المؤقتين كمقاول" + " \n https://moqawlty.nosi.gov.eg \n UserName: " + oModalRequest.OModel.sUserName + " \n Password: " + oModalRequest.OModel.sPassword) : // مقاول
                            //                                 (" لقد تم تسجيلك على منظومه التأمين على عمال المقاولات المؤقتين كجهه اسناد" + " \n https://moqawlty.nosi.gov.eg \n UserName: " + oModalRequest.OModel.sUserName + " \n Password: " + oModalRequest.OModel.sPassword)); // جهه الاسناد                                     

                            //// return 0 Send Done
                            //int returnVod = sms.SendSMSWithDLR("Social Security Dev", "9ad1O9S9x9", message, "A", "Insurance", oUserRequest.OModel.oUsers.phone1);

                            //// Update User Sms ابديت نتيجه الرساله
                            //db.spUpdateSmsUsers(oUserRequest.OModel.iUserCode, returnVod, DateTime.Now);
                            //#endregion
                        }


                        if (uploadFile != null && uploadFile.ContentLength > 0)
                        {
                            string[] nestedFolders = { };
                            string message = file.addFileWithoutNum("UserImage", nestedFolders, uploadFile, oUserRequest.OModel.iUserCode.ToString(), file.images);

                            if (message == "تم الحفظ/Save File")
                            {
                                //TempData["msg"] = generalVariables.SaveDone;
                                oModalRequest.OModel.sImageURL = ((uploadFile != null && uploadFile.ContentLength > 0) ? Path.GetExtension(uploadFile.FileName) : null);
                                oUserRequest = conApi.connectionApiPost<UserRequest>("apiUsers", "PostUpdateImage", oModalRequest, oUserRequest.OModel.iUserCode.ToString());
                            }
                            else
                            {
                                TempData["msg"] = generalVariables.SaveWithoutImage;
                            }
                        }
                    }
                    else
                        TempData["msg"] = generalVariables.SaveNotDone;
                }

                var screen = formCollection["addFromProcessScreenUsers"];

                if (!String.IsNullOrEmpty(screen))
                    return RedirectToAction(screen);

                return RedirectToAction("vUsersIndex");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /// <summary>
        ///Page Of Add Contrator Or Refrence Side
        /// </summary>
        /// <returns>View Page</returns>
        // اضافة جهه اسناد - مقاول
        public ActionResult _vpRefSideContAdd()
        {
            try
            {
                ViewBag.legalEntity = new SelectList(db.legalEntities.ToList(), "legalEntityCode", "legalEntityName");
                ReferenceSideContractorRequest refSideCont = new ReferenceSideContractorRequest();
                return PartialView(refSideCont);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Add Contrator Or Refrence Side
        /// </summary>
        /// <param name="oModalRequest">Data of Contractor Or Refrence Side</param>
        /// <param name="formCollection">Other Data Of Contractor Or Refrence Side </param>
        /// <param name="ddlUserType">Code User Type</param>
        /// <param name="UserType">User Type Id</param>
        /// <param name="contractorCode">Contractor Code</param>
        /// <param name="uploadFile">Image Path</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _vpRefSideContAdd(ReferenceSideContractorRequest oModalRequest, FormCollection formCollection, string ddlUserType, string UserType, string contractorCode, HttpPostedFileBase uploadFile)
        {
            try
            {
                bISSearch = false;

                ReferenceSideContractorRequest oRefSideContReq = new ReferenceSideContractorRequest();
                oModalRequest.OModel.sReferanceSideContractorNum = Convert.ToInt32(oModalRequest.OModel.sReferanceSideContractorNum).ToString("000000000");
                oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());

                oRefSideContReq = conApi.connectionApiPost<ReferenceSideContractorRequest>("apiReferenceSideContractor", "PostSaveRefSideCont", oModalRequest, null);
                //if (oRefSideContReq.bIsSaved)
                //{
                //    TempData["msg"] = generalVariables.SaveDone;
                //}
                //else
                //    TempData["msg"] = generalVariables.SaveNotDone;

                TempData["msg"] = oRefSideContReq.OModel.dyError;

                var screen = formCollection["addFromProcessScreenUsers"];

                if (!String.IsNullOrEmpty(screen))
                    return RedirectToAction(screen);

                return RedirectToAction("vUsersIndex");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /// <summary>
        /// Page For Search About User
        /// </summary>
        /// <returns>Vview Page</returns>
        public ActionResult _vpUserSearch()
        {
            try
            {
                ViewBag.legalEntity = new SelectList(new List<SelectListItem> {
                     new SelectListItem{ Text="منشأه", Value = "2" },
                     new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", 0);

                // Admin
                if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                    ViewBag.SearchUsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName");
                else
                    ViewBag.SearchUsersTypes = new SelectList(db.userTypes.Where(x => x.userTypeCode != 1).ToList(), "userTypeCode", "userTypeName");

                //ViewBag.SearchUsersTypes = new SelectList(db.userTypes.ToList(), "userTypeCode", "userTypeName");
                ViewBag.FilterUserTypeList = new List<SelectListItem> { new SelectListItem { Text = " ", Value = "0" } };
                ViewBag.OFFICEList = new SelectList(db.officeInsurances.ToList(), "officeInsuranceCode", "officeInsuranceName");
                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Search About User
        /// </summary>
        /// <param name="formCollection">Data For Search</param>
        /// <param name="ddlUsTy">User Type</param>
        /// <param name="legalEntity">Legal Entity</param>
        /// <param name="insNum">Insurance Number</param>
        /// <param name="txtCont">Name Of Contractor Or Refrence Side</param>
        /// <returns>View Page Of Users</returns>
        [HttpPost]
        public ActionResult _vpUserSearch(FormCollection formCollection, string[] ddlUsTy, string legalEntity, string insNum, string txtCont)
        {
            try
            {
                bISSearch = true;

                //Model = new userRequest();
                List<string> UserDetails = new List<string>();
                string FilterUserType = null;
                UserDetails.Add(formCollection["ddlTU"] == "0" ? null : formCollection["ddlTU"].ToString());
                UserDetails.Add(formCollection["txtFUName"] == "" ? null : formCollection["txtFUName"].ToString());
                UserDetails.Add(formCollection["txtUName"] == "" ? null : formCollection["txtUName"].ToString());
                UserDetails.Add(formCollection["txtNatId"] == "" ? null : formCollection["txtNatId"].ToString());
                UserDetails.Add(formCollection["isAct"] == "false" ? null : formCollection["isAct"].ToString());
                UserDetails.Add(formCollection["txtAdd"] == "" ? null : formCollection["txtAdd"].ToString());
                UserDetails.Add(formCollection["txtEma"] == "" ? null : formCollection["txtEma"].ToString());
                UserDetails.Add(formCollection["txtPh1"] == "" ? null : formCollection["txtPh1"].ToString());
                UserDetails.Add(formCollection["txtPh2"] == "" ? null : formCollection["txtPh2"].ToString());
                if (ddlUsTy != null)
                    FilterUserType = generalMethods.sConcatString(ddlUsTy, ',');
                UserDetails.Add(FilterUserType);

                UserDetails.Add(String.IsNullOrEmpty(legalEntity) ? "-1" : legalEntity);
                UserDetails.Add(String.IsNullOrEmpty(insNum) ? "-1" : insNum);
                UserDetails.Add(String.IsNullOrEmpty(txtCont) ? "-1" : txtCont);

                string uc;
                // Admin
                if (Session["areaOfficePermission"] != null && Session["areaOfficePermission"].ToString() == "1")
                    uc = "-1";
                else
                    uc = Session["uc"].ToString();

                UserDetails.Add(uc);

                oUserRequest = conApi.connectionApiSearchList<UserRequest>("apiUsers", "PostSearchUsers", UserDetails);
                //Model.LModels = oUserRequest.LModels;
                return RedirectToAction("vUsersIndex", new { Search = "Search" });
                //return RedirectToAction("vUsersIndex");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get the Data Is Filter By UserType
        /// If Insurance Employee Get : Offices List , Areas List
        /// If Contractor Or Refrence Side Get : Legal Entity And Insurance Number And Name 
        /// </summary>
        /// <param name="ID">User Type Id</param>
        /// <returns>List Of Data</returns>
        public JsonResult getUserType(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var data = conApi.connectionApiGetList<UserRequest>("apiUsers", "GetUsersType", ID.ToString());
                return Json(data.data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Check For User Name For Add new User Or Edit User
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>Data Of Check</returns>
        public JsonResult checkUserName(string userName)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                bool check = db.users.Any(x => x.userName == userName);
                if (check == true)
                {
                    var data = "error";
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = "sucess";
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                var data = "error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Get Areas By Office Code
        /// </summary>
        /// <param name="ID">Ofice Code</param>
        /// <returns></returns>
        public JsonResult getArea(string[] ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string AreaList = generalMethods.sConcatString(ID, ',');

                List<string> AreaTypes = new List<string>();
                AreaTypes.Add(AreaList);

                var data = conApi.connectionApiSearchList<UserRequest>("apiUsers", "PostArea", AreaTypes);
                return Json(data.data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("");
        }
        /// <summary>
        /// Cancel Search For User
        /// </summary>
        /// <returns>View Page Of Users</returns>
        public ActionResult _vpSearchCancel()
        {
            bISSearch = false;

            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction("vUsersIndex");
        }
        /// <summary>
        /// View User Module Property 
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult vUsersGroupIndex()
        {
            return View();
        }

    }
}