using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using System;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Process
{
    /// <summary>
    ///   Controller Of Process Users.
    /// </summary>
    public class ProcessUsersController : Controller
    {
        // GET: ProcessUsers مستخدمين العمليه
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        static ProcessUsersRequest oProcessUsersRequest = new ProcessUsersRequest();
        static string sScreenName;

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public ProcessUsersController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Process Users.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="cd"> Code Of Module Insurance Officer. </param>
        /// <param name="sn"> Screen Name. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessUser(int? pc, int? cd, int? sn)
        {
            try
            {
                if (!String.IsNullOrEmpty(sn.ToString()))
                    sScreenName = sn.ToString();

                if (pc != null)
                    Session["processCode"] = pc;

                if (cd != null)
                {
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                }

                string contOROfficer = Session["officeCode"] != null ? Session["processCode"].ToString() + ",-1" : Session["processCode"].ToString() + "," + Session["uc"].ToString();
                oProcessUsersRequest = conApi.connectionApiGetList<ProcessUsersRequest>("apiProcessUsers", "GetProcessUsers", contOROfficer);

                if (oProcessUsersRequest.LModels != null)
                    TempData["lMainUsers"] = oProcessUsersRequest.LModels.ToList();

                TempData["lSubUsers"] = oProcessUsersRequest.lProcsessUserModel.ToList();
                TempData["sProcessName"] = oProcessUsersRequest.oProcessModel.sProcessName;
                TempData["sAreaID"] = oProcessUsersRequest.oProcessModel.sAreaIDName;
                TempData["sOfficeID"] = oProcessUsersRequest.oProcessModel.sOfficeIDName;
                TempData["sProcessNumber"] = oProcessUsersRequest.oProcessModel.sProcessNumber;
                TempData["sScreenName"] = sScreenName == "1" ? "العمليات" : (sScreenName == "2" ? "طلبات العمليات" : "المستخدمين");

                viewBags();
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Save User On Process.
        /// </summary>
        /// <param name="mainUser"> Main User. </param>
        /// <param name="subUser"> Sub User. </param>
        public void vProcessUserSave(string mainUser, string subUser)
        {
            try
            {
                oProcessUsersRequest.OModel = new ProcessUsersModel();
                oProcessUsersRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                oProcessUsersRequest.OModel.inProcessCode = Convert.ToInt32(Session["processCode"].ToString()); // كود العمليه
                string sProcessName = db.processes.FirstOrDefault(x => x.processCode == oProcessUsersRequest.OModel.inProcessCode).processName;
                int? procCode = oProcessUsersRequest.OModel.inProcessCode;
                string contractorType = "";

                // mainContractor مقاولين رئيسين
                if (!String.IsNullOrEmpty(mainUser))
                {
                    oProcessUsersRequest.OModel.inUserCode = Convert.ToInt32(mainUser);// كود المستخدم
                    oProcessUsersRequest.OModel.bnContractorType = true;// نوع المقاول  1=> رئيسي
                    contractorType = " ( كمقاول رئيسي )";
                }
                else if (!String.IsNullOrEmpty(subUser)) // subContractor مقاولين من باطن
                {
                    oProcessUsersRequest.OModel.inUserCode = Convert.ToInt32(subUser);// كود المستخدم
                    oProcessUsersRequest.OModel.bnContractorType = false;// نوع المقاول 0 => باطن 
                    contractorType = " ( كمقاول فرعى )";
                }

                // هجيب انهى مستخدمين للموظف ولا للمقاول الخاصين بشركته
                oProcessUsersRequest.OModel.sIpUpdate = Session["officeCode"] != null ? Session["processCode"].ToString() + ",-1" : Session["processCode"].ToString() + "," + Session["uc"].ToString();
                oProcessUsersRequest = conApi.connectionApiPost<ProcessUsersRequest>("apiProcessUsers", "PostSaveProcessUsers", oProcessUsersRequest, null);
                if (oProcessUsersRequest.OModel.bIsSaved)
                {
                    TempData["msg"] = generalVariables.SaveDone;

                    //if (!String.IsNullOrEmpty(oProcessUsersRequest.OModel.oUsers.phone1))
                    //{
                    //    //send SMS to Worker
                    //    #region SMS
                    //    ServiceSoapClient sms = new ServiceSoapClient();
                    //    string message = "لقد تم تسجيلك  : " + contractorType + "  بعمليه : " + sProcessName + " على منظومه التأمين على عمال المقاولات المؤقتين " + " \n https://moqawlty.nosi.gov.eg ";

                    //    // return 0 Send Done
                    //    int returnVod = sms.SendSMSWithDLR("Social Security Dev", "9ad1O9S9x9", message, "A", "Insurance", oProcessUsersRequest.OModel.oUsers.phone1);

                    //    // Update Sms Process Users ابديت نتيجه الرساله
                    //    db.spUpdateSmsProcessUsers(oProcessUsersRequest.OModel.iProcessUserCode, returnVod, DateTime.Now);
                    //    #endregion
                    //}
                }
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
            }
            catch
            {

            }
        }

        /// <summary>
        ///   Delete Users From Process Users.
        /// </summary>
        /// <param name="formCollection"> Check This Users From Main Contractors Or Sub. </param>
        /// <param name="mainSubContractor"> Check This Users From Main Contractors Or Sub. </param>
        /// <param name="mainUser"> Codes Of Main Users Codes. </param>
        /// <param name="subUser"> Codes Of Sub Users Code. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult vProcessUserDelete(FormCollection formCollection, bool mainSubContractor, string mainUser, string subUser)
        {
            try
            {
                if (!String.IsNullOrEmpty(mainUser) || !String.IsNullOrEmpty(subUser))
                    vProcessUserSave(mainUser, subUser);
                else
                {
                    string DeleteCode = "";
                    // مقاول رئيسي
                    if (mainSubContractor)
                    {
                        if (!string.IsNullOrEmpty(formCollection["chkMainContractors"]))
                        {
                            string[] temp = formCollection["chkMainContractors"].Split(',');
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] != "false")
                                {
                                    DeleteCode += temp[i] + ',';  // users code
                                }
                            }
                        }
                    }
                    else if (!mainSubContractor) // مقاول من باطن
                    {
                        if (!string.IsNullOrEmpty(formCollection["chkSubContractors"]))
                        {
                            string[] temp = formCollection["chkSubContractors"].Split(',');
                            for (int i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] != "false")
                                {
                                    DeleteCode += temp[i] + ',';   // users code
                                }
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(DeleteCode))
                    {
                        TempData["msg"] = generalVariables.UserNotFound;
                        return RedirectToAction("vProcessUser");
                    }
                    else
                    {

                        // هجيب انهى مستخدمين للموظف ولا للمقاول الخاصين بشركته
                        string parmeterDelete = Session["officeCode"] != null ? Session["processCode"].ToString() + ",-1" + "," + DeleteCode :
                                                                                Session["processCode"].ToString() + "," + Session["uc"].ToString() + "," + DeleteCode;
                        oProcessUsersRequest = conApi.connectionApiDelete<ProcessUsersRequest>("apiProcessUsers", "DeleteProcessUsers", parmeterDelete);
                        if (oProcessUsersRequest.OModel.bIsDeleted)
                            TempData["msg"] = generalVariables.DeleteDone;
                        else
                            TempData["msg"] = generalVariables.DeleteNotDone;
                    }
                }
            }
            catch
            {
                TempData["msg"] = generalVariables.DeleteNotDone;
            }


            return RedirectToAction("vProcessUser");
        }


        /// <summary>
        ///   Back To Speical Page.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult _vpBack()
        {
            // عمليات الموظف
            if (sScreenName == "1")
                return RedirectToAction("../Process/vProcessShowIndex", new { cp = 1, cd = 2 });

            // المقاول
            if (sScreenName == "3")
                return RedirectToAction("../Contractor/vContractorIndex", new { cp = 1 });


            // طلبات العمليات
            return RedirectToAction("../Process/vProcessRequest", new { cp = 1, cd = 3 });
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        public void viewBags()
        {
            string contOROfficer = Session["officeCode"] != null ? "-1" : Session["uc"].ToString();

            ViewBag.usersMainContractor = new SelectList(db.spGetUsersNotInProcess(Convert.ToInt32(Session["processCode"].ToString()), 1, Convert.ToInt32(contOROfficer)).ToList(), "userCode", "userFullName");
            ViewBag.usersSubContractor = new SelectList(db.spGetUsersNotInProcess(Convert.ToInt32(Session["processCode"].ToString()), 0, Convert.ToInt32(contOROfficer)).ToList(), "userCode", "userFullName");
        }


        /// <summary>
        ///   Add User.
        /// </summary>
        /// <param name="ID"> USer Code. </param>
        /// <returns> View. </returns>
        public ActionResult _vpUsersAdd(int? ID)
        {
            UserRequest User = new UserRequest();

            int uc = Convert.ToInt32(Session["uc"].ToString());
            int? contCode = db.users.FirstOrDefault(user => user.userCode == uc).contractorCode; // Contractor Or Officer
            dynamic contractorProces = contCode == null ? db.spGetContractorsInProcess(Convert.ToInt32(Session["processCode"].ToString()), "0,1").ToList() : // Officer
                 db.spGetContractorsInProcess(Convert.ToInt32(Session["processCode"].ToString()), "0,1").Where(cont => cont.referenceSideContractorCode == contCode).ToList(); // Contractor

            ViewBag.users = new SelectList(contractorProces, "referenceSideContractorCode", "referenceSideContractorName");
            TempData["addFromProcessScreenUsers"] = "../ProcessUsers/vProcessUser";
            return PartialView("../Users/_vpUsersAdd", User);
        }

    }
}