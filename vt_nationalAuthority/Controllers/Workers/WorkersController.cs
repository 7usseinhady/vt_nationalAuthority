using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.App_Code;
using vt_nationalAuthority.Models;
using vt_nationalAuthority.SMS;

namespace vt_nationalAuthority.Controllers.Workers
{
    public class WorkersController : Controller
    {
        ConnectionApi conApi = new ConnectionApi();
        ApiNationalAuthority.Models.GeneralMethods generalMethods = new ApiNationalAuthority.Models.GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        WorkerRequest oWorkerRequest = new WorkerRequest();
        DataAccessLayer.Models.GeneralMethods generalMethod = new DataAccessLayer.Models.GeneralMethods();
        nat_wsdl nat = new nat_wsdl();
        cover_wsdl cover = new cover_wsdl();

        static WorkerRequest Model;
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static int? ProcessCode, workerCode,pW;
        static string contrators_;
        static string Action_, Controller_;
        static int? cd4_, cd_;
        static string Areas_, Offices_;
        static string DateAttendance;
        // GET: Workers
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public WorkersController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Get Data That Need In DropDown Lists
        /// </summary>
        public void ViewBags()
        {
            ViewBag.Jops = new SelectList(db.careers.ToList(), "careerCode", "careerName");
            ViewBag.MahrDegree = new SelectList(db.skillDegrees.ToList(), "skillDegreeCode", "skillDegreeName");
            ViewBag.MedicalInsurance = new SelectList(db.medicalStatus.ToList(), "medicalStatusCode", "medicalStatusName");
            ViewBag.AddMedicalInsurance_ = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="لائق", Value = "1" },
                new SelectListItem{ Text="معلق", Value = "2" },
                new SelectListItem{ Text="اصابه جزئيه", Value = "3"},
                new SelectListItem{ Text="اصابه كليه", Value = "2" }, }, "Value", "Text", 1);
            ViewBag.StopProcessReason_ = new SelectList(new List<SelectListItem>
            {
                    new SelectListItem{ Text="محدود", Value = "1" },
                new SelectListItem{ Text="متوسط", Value = "2" },
                new SelectListItem{ Text="ماهر", Value = "3"}}, "Value", "Text", 1);


        }
        /// <summary>
        /// Page of Show Workers
        /// </summary>
        /// <returns>View Ppage</returns>
        public ActionResult vWorkersIndex()
        {
            Model = new WorkerRequest();
            return View();
        }
        /// <summary>
        ///  Workers Show
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="inPage">Current Page Number</param>
        /// <returns>View Page</returns>
        public ActionResult vWorkersShow(int? ProcessId, int? inPage)
        {
            return View();
        }
        /// <summary>
        /// Get List Of Workers
        /// </summary>
        /// <param name="Paths">Current Path Is Open In Browse</param>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="inPage">Current Page Number</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <param name="Cont"> List Of Contractors Codes</param>
        /// <param name="cd"> Code Of Module</param>
        /// <param name="cd4"> Code Of Level Page For Permissiond</param>
        /// <param name="workerCode"> Worker Code</param>
        /// <returns>List Of Workers</returns>
        public PartialViewResult _vpWorkersIndex(string Paths, int? ProcessId, int? inPage, string areas, string Offices, string Cont, int? cd, int? cd4,int?workerCode)
        {
            try
            {
                if(workerCode != null)
                {
                    int userCode = Convert.ToInt32(Session["uc"].ToString());
                    db.UpdateNotificationSeen(userCode, workerCode, 15);
                }
                Areas_ = areas;
                Offices_ = Offices;
                /////////////////////////////////////////////////// NO DELETE OR EDIT
                //// دى عشان بفضي موديل البحث عشان بيفضل محتفظ بقيمته و بفضيه في وقت معين
                if (cd != null)
                    cd4_ = null;
                if (cd4 != null)
                    cd_ = null;
                if (cd4_ == null && cd_ == null)
                {
                    Model = null;
                    Model = new WorkerRequest();
                    Model.LModels = null;
                }
                if (cd != null)
                    cd_ = cd;
                if (cd4 != null)
                    cd4_ = cd4;
                ////////////////////////////////////////////

                TempData["action"] = (String.IsNullOrEmpty(Paths) ? null : Paths);
                int uc = Convert.ToInt32(Session["uc"].ToString());
                if (cd != null && cd4 == null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                else if (cd == null && cd4 != null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd4).ToList();

                ViewBags();
                contrators_ = Cont;
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;
                ProcessCode = ProcessId;
                if (Model == null)
                    Model = new WorkerRequest();
                if (Model.LModels != null)
                    oWorkerRequest.LModels = Model.LModels;
                else
                {
                    List<string> Search = new List<string>();
                    string id = ProcessId == null ? "0" : ProcessId.ToString();
                    Search.Add(id);
                    // موظف تأمينات             
                    if (Session["areaOfficePermission"] != null)
                    {
                        // هنا سرش بمنطقه و مكتب 
                        if (!String.IsNullOrEmpty(areas) || !String.IsNullOrEmpty(Offices))
                        {
                            Search.Add(areas);
                            if (Session["SearchOffice"] != null)
                                Search.Add(Session["SearchOffice"].ToString());
                            else
                                Search.Add(Offices);
                            Search.Add(Session["uc"].ToString());
                            Search.Add(workerCode == null ? null : workerCode.ToString());
                            oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostWorkerDetails", Search);
                        } // سرش بمقاول
                        else if (!String.IsNullOrEmpty(Cont))
                        {
                            Search.Add(Cont);
                            Search.Add(Session["uc"].ToString());
                            Search.Add(workerCode == null ? null : workerCode.ToString());
                            oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostGetWorkerDetails", Search);
                        } // من غير سرش خالص
                        else
                        {
                            Search.Add(null);
                            Search.Add(Session["uc"].ToString());
                            Search.Add(workerCode == null ? null : workerCode.ToString());
                            oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostGetWorkerDetails", Search);
                        }
                    }
                    // العمال في المقاول 
                    else
                    {
                        Search.Add(null); //Search[1] contractors
                        Search.Add(Session["uc"].ToString()); //search[2] users
                        Search.Add(null); //search[3] office
                        Search.Add(null); //search[4] areas
                        Search.Add(workerCode == null ? null : workerCode.ToString());
                        oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostGetWorkerDetails", Search);

                    }
                }

                int x = iPageSize;
                if (oWorkerRequest == null)
                {
                    oWorkerRequest = new WorkerRequest();
                    oWorkerRequest.LModels = new List<WorkerModel>();
                }

                if (oWorkerRequest.LModels == null)
                    oWorkerRequest.LModels = new List<WorkerModel>();
                TempData["totalPages"] = Convert.ToInt32(Math.Ceiling((double)oWorkerRequest.LModels.ToList().Count / iPageSize));
                return PartialView(oWorkerRequest.LModels.OrderByDescending(y=>y.sLastAttendance).ToPagedList(insPageNumber ?? 1, iPageSize));
            }

            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Pageto Search For Worker
        /// </summary>
        /// <param name="ID">Worker Code If Want Get One Worker</param>
        /// <param name="act">Current Action-Result</param>
        /// <param name="contr">Current Controller Name </param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page</returns>
        public ActionResult _vpWorkersData(int? ID, string act, string contr, string areas, string Offices)
        {
            try
            {
                Action_ = act;
                Controller_ = contr;

                ViewBags();
                Model.LModels = null;
                WorkerRequest WorkerDetails = new WorkerRequest();
                WorkerDetails = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorker", ID.ToString());
                return PartialView(WorkerDetails);
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Search For Workers
        /// </summary>
        /// <param name="formCollection">Data Need For Search</param>
        /// <param name="MedicalInsurance">Medical Insurance Status</param>
        /// <param name="MahrDegree">Skill Degree Status</param>
        /// <param name="jops">Jop Code</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>List Of Workers</returns>
        [HttpPost]
        public ActionResult _vpWorkersData(FormCollection formCollection, string[] MedicalInsurance, string[] MahrDegree, string[] jops, string areas, string Offices)
        {
            try
            {
                List<string> WorkerDetails = new List<string>();
                if (Model == null)
                {
                    Model = new WorkerRequest();
                    Model.LModels = new List<DataAccessLayer.Models.WorkerModel>();
                }
                WorkerDetails.Add(formCollection["WorkerName"].ToString() == "" ? null : formCollection["WorkerName"].ToString()); // اسم العامل 
                WorkerDetails.Add(formCollection["WorkerSecNum"].ToString() == "" ? null : formCollection["WorkerSecNum"].ToString()); // الرقم التأمينى للعامل
                WorkerDetails.Add(formCollection["WorkerNatNum"].ToString() == "" ? null : formCollection["WorkerNatNum"].ToString()); // الرقم القومى للعامل
                WorkerDetails.Add(formCollection["DateAttendFrom"].ToString() == "" ? null : formCollection["DateAttendFrom"].ToString()); // تاريخ الحضور من
                WorkerDetails.Add(formCollection["DateAttendTo"].ToString() == "" ? null : formCollection["DateAttendTo"].ToString()); // تاريخ الحضور الى
                string MedInsur = (MedicalInsurance != null ? generalMethods.sConcatString(MedicalInsurance, ',') : null),
                       MahDeg = (MahrDegree != null ? generalMethods.sConcatString(MahrDegree, ',') : null),
                       Jop = (jops != null ? generalMethods.sConcatString(jops, ',') : null);
                WorkerDetails.Add(MedInsur); // الحاله الصحيه
                WorkerDetails.Add(MahDeg); // درجة المهاره
                WorkerDetails.Add(Jop); // الوظيفه
                WorkerDetails.Add(ProcessCode == null ? null : ProcessCode.ToString());
                oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostSearchWorkers", WorkerDetails);
                Model.LModels = oWorkerRequest.LModels;

                if (Action_ == "vpWorkersShow")
                    return RedirectToAction(Action_, Controller_, new { ProcessId = ProcessCode, Cont = contrators_, cd4 = cd4_ });
                else if (Action_ == "vpWorkers")
                    return RedirectToAction(Action_, Controller_, new { areas = areas, cd = cd_, cp = Request.QueryString["cp"], Offices = Offices });
                else if (Action_ == "vWorkersShow")
                    return RedirectToAction(Action_, Controller_, new { ProcessId = ProcessCode, inPage = insPageNumber });
                else
                    return RedirectToAction(Action_, Controller_);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Cancel Search For Workers
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="Act">Current Action Result</param>
        /// <param name="Contr">Current Controller</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page Of Workers</returns>
        public ActionResult vCancelSearchWorker(int? ProcessId, string Act, string Contr, string areas, string Offices)
        {
            try
            {
                Session["SearchOffice"] = null;
                Model = null;
                Model = new WorkerRequest();
                Model.LModels = null;
                TempData["msg"] = generalVariables.SearchCancel;

                if (Act == "vWorkersShow")
                    return RedirectToAction(Act, Contr, new { ProcessId = ProcessId, inPage = 1 });
                else if (Act == "vpWorkers")
                    return RedirectToAction(Act, Contr, new { areas = areas, cd = cd_, inPage = 1, Offices = Offices });
                else if (Act == "vpWorkersShow")
                    return RedirectToAction(Act, Contr, new { ProcessId = ProcessId, cd4 = cd4_, inPage = 1 });
                else
                    return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get Worker Process
        /// </summary>
        /// <param name="ID">Worker Code</param>
        /// <returns>List Of Process</returns>
        public ActionResult _vpWorkerProcess(int ID)
        {
            try
            {
                if (ID != 0)
                {
                    pW = ID;
                    oWorkerRequest = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorkerProcess", ID.ToString());
                    TempData["totalPages"] = Convert.ToInt32(Math.Ceiling((double)oWorkerRequest.LprocessModel.ToList().Count / iPageSize));
                }
                if (oWorkerRequest.LprocessModel == null)
                      oWorkerRequest.LprocessModel = new List<ProcessModel>();

                return PartialView(oWorkerRequest.LprocessModel.ToPagedList(1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get The Next Worker Process In The List 
        /// </summary>
        /// <param name="pageIndex">Page Number</param>
        /// <returns>List Of Process</returns>
        public JsonResult GetWorkersProcess(int pageIndex)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                int startIndex = (pageIndex - 1) * iPageSize;
                oWorkerRequest = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorkerProcess", pW.ToString());
                oWorkerRequest.TotalPages = Convert.ToInt32(Math.Ceiling((double)oWorkerRequest.LprocessModel.ToList().Count / iPageSize));
                oWorkerRequest.CurrentPage = pageIndex;
                oWorkerRequest.LprocessModel = oWorkerRequest.LprocessModel.Skip(startIndex).Take(iPageSize).ToList();
                return Json(oWorkerRequest, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Get Worker Attendance
        /// </summary>
        /// <param name="year">Year Number</param>
        /// <param name="Month">Month Number</param>
        /// <param name="day">Day Number</param>
        /// <param name="WorkerName">Worker Name</param>
        /// <returns>List Of Attendance</returns>
        public ActionResult _vpWorkerAttendance(string year, int? Month, string day, string WorkerName)
        {
            try
            {
                insPageNumber = 1;

                string Date = year + '/' + Month + '/' + day;
                if (!String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(day))
                    DateAttendance = Date;
                else
                    Date = DateAttendance;
                List<string> AttandanceSearch = new List<string>();
                AttandanceSearch.Add(ProcessCode == null ? Session["processCode"].ToString() : ProcessCode.ToString());
                AttandanceSearch.Add(String.IsNullOrEmpty(WorkerName) ? null : WorkerName);
                AttandanceSearch.Add(Date);
                oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostGetAttendance", AttandanceSearch);

                if (oWorkerRequest == null)
                {
                    oWorkerRequest = new WorkerRequest();
                    oWorkerRequest.LworkerAttendanceModel = new List<DataAccessLayer.Models.WorkerAttendanceModel>();
                }

                if (oWorkerRequest.LworkerAttendanceModel == null)
                    oWorkerRequest.LworkerAttendanceModel = new List<DataAccessLayer.Models.WorkerAttendanceModel>();

                return PartialView(oWorkerRequest.LworkerAttendanceModel.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /// <summary>
        /// Page Of Worker Phones
        /// </summary>
        /// <param name="ID">Worker Code</param>
        /// <param name="IDDelete">Check If Want Delete Phone Number</param>
        /// <param name="screen">Current Page Open</param>
        /// <returns>List Of Phone Numbers</returns>
        public ActionResult _vpWorkerPhones(int? ID, int? IDDelete, string screen)
        {
            try
            {
                TempData["screen"] = screen;
                if (ID != null)
                    workerCode = ID;
                if (IDDelete != null)
                {
                    oWorkerRequest = conApi.connectionApiDelete<WorkerRequest>("apiWorker", "DeleteWorkerPhone", IDDelete.ToString() + ',' + workerCode.ToString());
                    if (oWorkerRequest.bIsDeleted)
                        TempData["msg"] = generalVariables.DeleteDone;
                    else
                        TempData["msg"] = generalVariables.DeleteNotDone;
                    workerCode = null;
                    return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
                }
                else if (ID != null)
                {
                    workerCode = ID;
                    oWorkerRequest = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorkerPhones", ID.ToString());
                }
                if (oWorkerRequest.LworkerContactModel == null)
                    oWorkerRequest.LworkerContactModel = new List<WorkerContactModel>();

                var tuple = new Tuple<WorkerContactModel, List<WorkerContactModel>>(new WorkerContactModel(), oWorkerRequest.LworkerContactModel);
                return PartialView(tuple);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get And Save Worker Phone Numbers
        /// </summary>
        /// <param name="model">Data Of Phone Number</param>
        /// <returns>View Page Of  Workers</returns>
        [HttpPost]
        public ActionResult _vpWorkerPhones([Bind(Prefix = "Item1")] WorkerContactModel model)
        {
            try
            {
                WorkerRequest newObj = new WorkerRequest();
                newObj.OworkerContactModel.iWorkerCode = (int)workerCode;
                newObj.OworkerContactModel.sPhone = model.sPhone;
                newObj.OworkerContactModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                newObj.OworkerContactModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostSaveWorkerPhone", newObj, null);
                if (oWorkerRequest.bIsSaved)
                    TempData["msg"] = generalVariables.SaveDone;
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
                return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Check If Worker Phone Save Before.
        /// </summary>
        /// <returns>True :  Save Before ,  False : Not Save Before That</returns>
        public JsonResult checkSave()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = "";
            int check = 0;
             check = db.workerContacts.Where(x => x.workerCode == workerCode && x.Freez == false).ToList().Count;
            if(check >0 )
            {
                data = "1";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = "0";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Save Medical Insurance Status For Worker
        /// </summary>
        /// <param name="WnatNum">Worker National Id Number</param>
        /// <param name="WMIPhone">Worker Medical Insurance Phone Number</param>
        /// <returns>View Page Of Workers</returns>
        [HttpPost]
        public ActionResult _vpWorkerMedicalInsurance(string WnatNum,string WMIPhone)
        {
            try
            {
                if (WnatNum != "")
                {
                    int WorkerCode = 0;
                    var Workercode = db.workers.Where(x => x.workerNationalID == WnatNum).FirstOrDefault();
                    if (Workercode != null)
                    {
                        WorkerCode = Convert.ToInt32(Workercode.workerCode);
                        WorkerRequest NewObj = new WorkerRequest();
                        NewObj.OmedicalInsuranceModel.iWorkerCode = WorkerCode;
                        NewObj.OmedicalInsuranceModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                        NewObj.OmedicalInsuranceModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                        oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostSaveMedicalInsurance", NewObj, null);
                        if (oWorkerRequest.bIsSaved)
                        {
                            if (oWorkerRequest.bIsSaved)
                            {
                                //send SMS to Worker
                                #region SMS
                                ServiceSoapClient sms = new ServiceSoapClient();
                                string password = "9ad1O9S9x9";
                                string userName = "Social Security Dev";
                                string message = "برجاء التوجه للتأمين الصحى , للكشف عن حالتك الصحيه";
                                int return_ = sms.SendSMSWithDLR(userName, password, message, "A", "Insurance", "01157899393");
                                #endregion
                                //save Worker Contact
                                WorkerRequest newObj = new WorkerRequest();
                                newObj.OworkerContactModel.iWorkerCode = WorkerCode;
                                newObj.OworkerContactModel.sPhone = WMIPhone;
                                newObj.OworkerContactModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                                oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostSaveWorkerPhone", newObj, null);
                                if (oWorkerRequest.bIsSaved && return_ == 0)
                                    TempData["msg"] = generalVariables.SaveDone;
                                else
                                {
                                    if (oWorkerRequest.bIsSaved)
                                        db.workerContacts.Remove(db.workerContacts.FirstOrDefault(x => x.workerCode == WorkerCode && x.phone == WMIPhone));
                                    TempData["msg"] = generalVariables.SaveNotDone;
                                }
                            }
                            else
                                TempData["msg"] = generalVariables.SaveNotDone;
                        }
                        else
                            TempData["msg"] = generalVariables.SaveNotDone;
                    }
                    else
                        TempData["msg"] = generalVariables.NationalID;

                }
                return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Check If Number Insert In Save Medical Status For Worker The Same Number For Worker In Insurance
        /// </summary>
        /// <param name="nationalID">National Id For Worker</param>
        /// <param name="Number">Phone Number For Worker</param>
        /// <returns>True :  The Same Number , False : Different Number</returns>
        public JsonResult checkeWorkerPhoneNumber (string nationalID , string Number)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = "";
            int WorkerCode = db.workers.FirstOrDefault(x => x.workerNationalID == nationalID).workerCode;
            var list = db.workerContacts.Where(x => x.workerCode == WorkerCode && x.Freez == false).ToList();
            if (list.Count > 0)
            {
                if (list[0].phone == Number)
                {
                    data = "1";
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    data = "0";
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
               return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Save Main Power Status For Worker
        /// </summary>
        /// <param name="AddWorkerMainPower">Data Need For Main Power Status</param>
        /// <returns>View Page Of Workers</returns>
        [HttpPost]
        public ActionResult _vpWorkerManPower(string AddWorkerMainPower)
        {
            try
            {
                if (AddWorkerMainPower != "")
                {
                    int WorkerCode = 0;
                    var workercode = db.workers.Where(x => x.workerNationalID == AddWorkerMainPower).FirstOrDefault();
                    if (workercode != null)
                    {
                        int uc = Convert.ToInt32(Session["uc"].ToString());
                        WorkerCode = Convert.ToInt32(workercode.workerCode);
                        WorkerRequest NewObj = new WorkerRequest();
                        NewObj.OmanPowerModel.iWorkerCode = WorkerCode;
                        NewObj.OmanPowerModel.inUserInsertCode = uc;
                        NewObj.OmanPowerModel.inUserUpdateCode = uc;
                        oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostSaveManPower", NewObj, null);
                        if (oWorkerRequest.bIsSaved)
                            TempData["msg"] = generalVariables.SaveDone;
                        else
                            TempData["msg"] = generalVariables.SaveNotDone;
                    }
                    else
                        TempData["msg"] = generalVariables.NationalID;
                }

                return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Delete Worker 
        /// </summary>
        /// <param name="IDDelete">Worker Code Want To Delete</param>
        /// <returns>View Page Of Workers</returns>
        public ActionResult _vpWorkerCancel(int? IDDelete)
        {
            try
            {
                if (IDDelete != null)
                {
                    oWorkerRequest = conApi.connectionApiDelete<WorkerRequest>("apiWorker", "DeleteWorker", IDDelete.ToString() + ',' + Session["uc"].ToString());
                    if (oWorkerRequest.bIsDeleted)
                        TempData["msg"] = generalVariables.DeleteDone;
                    else
                        TempData["msg"] = generalVariables.DeleteNotDone;
                    return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
                }
                else
                    return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = Areas_, cd = cd_, Offices = Offices_, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get List Of Previous Period For Woker 
        /// </summary>
        /// <param name="workerId">Worker Code</param>
        /// <param name="processId">Process Code</param>
        /// <returns>ist of Previous Period</returns>
        public ActionResult _vppreviousPeriod(int workerId, int processId)
        {
            return RedirectToAction("_vpPeriodMonth", "previousPeriod", new { processID = processId, workerCode = workerId });
        }
        /// <summary>
        /// Get Workers Details
        /// </summary>
        /// <param name="pageIndex">Current Number Of Page</param>
        /// <returns>List of Workers</returns>
        public JsonResult GetWorkers(int pageIndex)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                int startIndex = (pageIndex - 1) * iPageSize;
                List<string> Search = new List<string>();
                Search.Add(ProcessCode.ToString()); //processCode
                Search.Add(null); //Search[1] contractors
                Search.Add(Session["uc"].ToString()); //search[2] users
                Search.Add(null); //search[3] office
                Search.Add(null); //search[4] areas
                oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostGetWorkerDetails", Search);
                oWorkerRequest.TotalPages = Convert.ToInt32(Math.Ceiling((double)oWorkerRequest.LModels.ToList().Count / iPageSize));
                oWorkerRequest.CurrentPage = pageIndex;
                oWorkerRequest.LModels = oWorkerRequest.LModels.Skip(startIndex).Take(iPageSize).ToList();
                return Json(oWorkerRequest, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
