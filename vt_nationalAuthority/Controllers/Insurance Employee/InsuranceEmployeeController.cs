using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.App_Code;
using vt_nationalAuthority.Models;


namespace vt_nationalAuthority.Controllers.Insurance_Employee
{
    public class InsuranceEmployeeController : Controller
    {
        // GET: InsuranceEmployee
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        UserRequest oUserRequest = new UserRequest();
        WorkerRequest oWorkerRequest = new WorkerRequest();
        GeneralMethods generalMethods = new GeneralMethods();
        nat_wsdl nat = new nat_wsdl();
        cover_wsdl cover = new cover_wsdl();
        att_wsdl_cls attendance_wsdl = new att_wsdl_cls();
        static int? cp_, cd_;
        static int? ProcessCode;
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public InsuranceEmployeeController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Show Module Of Insurance Authority
        /// </summary>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page</returns>
        public ActionResult vInsuranceEmployeeIndex(string areas, string Offices)
        {
            int uc = Convert.ToInt32(Session["uc"].ToString());
            Session["CheckModule"] = db.CheckModuleUserPermisiom(uc, 1).ToList();
            return View();
        }
        /// <summary>
        /// Going To Show Process
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult vProcessShowIndex()
        {
            return View();
        }
        /// <summary>
        /// Going To Show Process Request
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult vProcessRequest()
        {
            TempData["screenName"] = "vProcessRequest";
            return View();
        }
        /// <summary>
        /// Going To Workers Class To Add Or Search For Worker
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult vWorkerAddSearch()
        {
            viewBags();
            return View();
        }
        /// <summary>
        /// Going To Search For Workers
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <returns>View Workers</returns>
        public ActionResult _vpWorkerSearch(int? ProcessId)
        {
            try
            {
                int pc = (ProcessId != null ? Convert.ToInt32(ProcessId) : Convert.ToInt32(Session["processCode"].ToString()));
                ViewBag.subCont = new SelectList(db.spGetContractorsInProcess(pc, "0").ToList(), "processSubContractorCode", "referenceSideContractorName");
                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Search For Workers
        /// </summary>
        /// <param name="main">Code Of Main Contractors</param>
        /// <param name="sub">Code Of Sub Contractors</param>
        /// <param name="screen">Name Of Current Page</param>
        /// <returns>View Page Of Workers</returns>
        [HttpPost]
        public ActionResult _vpWorkerSearch(string main, string[] sub, string screen)
        {
            try
            {
                string SUB = "", MainConCode = "", Contractors = "";
                if (!String.IsNullOrEmpty(main))
                {
                    if (Convert.ToBoolean(main) == true)
                        MainConCode = db.processSubContractors.FirstOrDefault(x => x.processCode == ProcessCode && x.contractorType == true).processSubContractorCode.ToString();
                }
                if (sub != null)
                    SUB = generalMethods.sConcatString(sub, ',');
                if (!String.IsNullOrEmpty(MainConCode))
                    Contractors = MainConCode;
                if (!String.IsNullOrEmpty(SUB))
                {
                    if (!String.IsNullOrEmpty(Contractors))
                        Contractors = Contractors + ",";
                    Contractors = Contractors + SUB;
                }
                return RedirectToAction(screen, new { ProcessId = ProcessCode, Cont = Contractors });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Add Worker Attendance In Insurance
        /// </summary>
        /// <param name="paths">Current Path Of Page</param>
        /// <param name="ID">Worker Code</param>
        /// <param name="cp">Level Code Of Pages</param>
        /// <param name="cd">Code Of Module</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <param name="pCode">Process Code</param>
        /// <returns>View Page</returns>
        public PartialViewResult _vpAddWorkers(string paths, int? ID, int? cp, int? cd, string[] areas, string[] Offices, int? pCode)
        {
            try
            {
                if (cp != null) cp_ = cp;
                if (cd != null) cd_ = cd;
                TempData["path"] = (paths == null ? null : paths);
                return PartialView();
            }
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Show Process Details
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="cd">Code of Module</param>
        /// <returns>View Page</returns>
        public ActionResult vProcessDetailsInsurEmp(int? ProcessId, int? cd)
        {
            try
            {
                Session["processCode"] = ProcessId == null ? Convert.ToInt32(Session["processCode"].ToString()) : ProcessId;
                if (cd != null)
                {
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    TempData["CheckModuleLevel3"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                }
                ProcessCode = ProcessId == null ? Convert.ToInt32(Session["processCode"].ToString()) : ProcessId;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Show Filter Of Areas And Offices
        /// </summary>
        /// <returns>View Filters</returns>
        public ActionResult _vpOfficeAreaa()
        {
            viewBags();
            return PartialView();
        }
        /// <summary>
        /// Filter Areas And Offices
        /// </summary>
        /// <param name="formCollection">Data Need For Filter</param>
        /// <param name="areas">List Of Areas Code</param>
        /// <param name="Offices">List Of Offices Code</param>
        /// <returns>View Current Page</returns>
        [HttpPost]
        public ActionResult _vpOfficeAreaa(FormCollection formCollection, string[] areas, string[] Offices)
        {
            try
            {
                Session["SearchOffice"] = (Offices == null ? null : generalMethods.sConcatString(Offices, ','));
                Session["AreaSearch"] = (areas == null ? null : generalMethods.sConcatString(areas, ','));
                if (formCollection.Count != 0)
                {
                    string action = formCollection["action"].ToString() == null ? null : formCollection["action"].ToString();
                    string controller = formCollection["controller"].ToString() == null ? null : formCollection["controller"].ToString();
                    return RedirectToAction(action, controller, new { areas = (Session["AreaSearch"] == null ? null : Session["AreaSearch"].ToString()), Offices = (Session["SearchOffice"] == null ? null : Session["SearchOffice"].ToString()), s = 1 });
                }
                else
                    return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get Data Need In DropDown List
        /// </summary>
        void viewBags()
        {
            try
            {
                int userCode = Convert.ToInt32(Session["uc"].ToString());
                List<int> OfficesPerm, areaCode, OfficeCode = new List<int>();
                //insurance employee
                if (Session["areaOfficePermission"] != null)
                {
                    OfficesPerm = Session["areaOfficePermission"].ToString().Split(',').Select(int.Parse).ToList();
                    string AreaCodesSearch = Session["AreaSearch"] == null ? "-1" : Session["AreaSearch"].ToString();
                    areaCode = Session["AreaSearch"] == null ? null : Session["AreaSearch"].ToString().Split(',').Select(int.Parse).ToList();
                    OfficeCode = Session["SearchOffice"] == null ? null : Session["SearchOffice"].ToString().Split(',').Select(int.Parse).ToList();
                    ViewBag.areas = new SelectList(db.spAllAreas(userCode, -1, null, null).ToList(), "areaCode", "areaIDName", areaCode == null ? null : areaCode);
                    if (Session["areaOfficePermission"].ToString() == "1")
                        ViewBag.Offices = new SelectList(db.GetOfficeInsurance(AreaCodesSearch, null).ToList(), "officeInsuranceCode", "officeInsuranceIDName", OfficeCode == null ? null : OfficeCode);
                    else
                        ViewBag.Offices = new SelectList(db.GetOfficeInsurance(AreaCodesSearch, null).Where(x => OfficesPerm.Contains(x.officeInsuranceCode)).ToList(), "officeInsuranceCode", "officeInsuranceIDName", OfficeCode == null ? null : OfficeCode);

                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Page Of Workers In Insurance Employee Module
        /// </summary>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <param name="workerCode">Worker Code</param>
        /// <returns>View Page</returns>
        public ActionResult vpWorkers(string areas, string Offices, int? workerCode)
        {
            TempData["areas"] = areas == null ? null : areas;
            TempData["Offices"] = Offices == null ? null : Offices;
            TempData["workerCode"] = (workerCode == null ? null : workerCode);
            return View();
        }
        /// <summary>
        /// Worker Cards 
        /// </summary>
        /// <param name="WorkerCode">Worker Code</param>
        /// <param name="Name">Worker Name </param>
        /// <returns>View Page</returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _vpWorkerCards(int? WorkerCode, string Name)
        {
            try
            {
                List<GetWorkerCardsNumbers_Result> modal = new List<GetWorkerCardsNumbers_Result>();
                if (WorkerCode != null)
                {
                    Session["workerCardCode"] = WorkerCode;
                    modal = db.GetWorkerCardsNumbers(WorkerCode).ToList();
                    var tuple = new Tuple<GetWorkerCardsNumbers_Result, List<GetWorkerCardsNumbers_Result>>(new GetWorkerCardsNumbers_Result(), modal.ToList());
                    return PartialView(tuple);
                }
                if (WorkerCode == null && Name == null)
                {
                    int workerCards = Convert.ToInt32(Session["workerCardCode"].ToString());
                    modal = db.GetWorkerCardsNumbers(workerCards).Where(x => x.cardsRequestName.Contains(Session["workercardsName"].ToString())).ToList();
                    var tuple = new Tuple<GetWorkerCardsNumbers_Result, List<GetWorkerCardsNumbers_Result>>(new GetWorkerCardsNumbers_Result(), modal.ToList());
                    return PartialView(tuple);
                }
                else
                {
                    Session["workercardsName"] = Name;
                    int workerCards = Convert.ToInt32(Session["workerCardCode"].ToString());
                    modal = db.GetWorkerCardsNumbers(workerCards).Where(x => x.cardsRequestName.Contains(Name)).ToList();
                    var tuple = new Tuple<GetWorkerCardsNumbers_Result, List<GetWorkerCardsNumbers_Result>>(new GetWorkerCardsNumbers_Result(), modal.ToList());
                    return PartialView(tuple);
                }

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Show Contractors In Process
        /// </summary>
        /// <param name="ProcessId"> Process Code</param>
        /// <param name="Cont">Contractors Codes</param>
        /// <returns>View Page</returns>
        public ActionResult vpContractorsShow(int? ProcessId, string Cont)
        {
            ProcessCode = ProcessId == null ? Convert.ToInt32(Session["processCode"].ToString()) : ProcessId;
            return View();
        }
        /// <summary>
        /// Accept Sub Contractor In Process
        /// </summary>
        /// <param name="inPage">Current Number Page</param>
        /// <param name="subContAccept">Sub-Contractors Codes is Accept</param>
        /// <param name="processCode">Pprocess Code</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page Of Contractors</returns>
        public ActionResult _vpAcceptSubContractor(int? inPage, string subContAccept, int processCode, string areas, string Offices)
        {
            try
            {
                if (!String.IsNullOrEmpty(subContAccept)) // Sub Contractor Accept
                {
                    ProcessRequest oProcessRequest = new ProcessRequest();
                    oProcessRequest.oProcessSubContractorMode = new DataAccessLayer.Models.ProcessSubContractorModel();
                    oProcessRequest.oProcessSubContractorMode.iProcessCode = processCode;
                    oProcessRequest.oProcessSubContractorMode.iProcessSubContractorCode = Convert.ToInt32(subContAccept);
                    oProcessRequest.oProcessSubContractorMode.userCodeAccept = Convert.ToInt32(Session["uc"].ToString());
                    oProcessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostAcceptSubContractor", oProcessRequest, null);
                    TempData["msg"] = (oProcessRequest.oProcessSubContractorMode.bIsSaved ? generalVariables.SubContractorAccept : generalVariables.SubContractorNotAccept);
                }
                return RedirectToAction("vpContractorsShow", "InsuranceEmployee", new { ProcessId = processCode, cd4 = 17, inPage = inPage });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Search Of Contractors In Process Details
        /// </summary>
        /// <returns>Page of Search Contractors</returns>
        public ActionResult _vpSearchContractor()
        {
            return RedirectToAction("_vpSearchContractor", "Contractor", new { });
        }
        /// <summary>
        /// Show Mission of Contractors
        /// </summary>
        /// <param name="ID">Contractor Code</param>
        /// <returns>Page of Missions</returns>
        public ActionResult _vpMissionContractor(int? ID)
        {
            return RedirectToAction("_vpMissionContractor", "Contractor", new { ID = ID });
        }
        /// <summary>
        /// Show Workers In Process Details , In Insurance Employee Module
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="Cont">Contractors Code</param>
        /// <returns>View Page</returns>
        public ActionResult vpWorkersShow(int? ProcessId, string Cont)
        {
            try
            {
                ProcessCode = ProcessId == null ? (String.IsNullOrEmpty(Session["processCode"].ToString()) ? 0 : Convert.ToInt32(Session["processCode"].ToString())) : Convert.ToInt32(ProcessId);
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Add Worker Phone
        /// </summary>
        /// <param name="ID">Worker Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vpWorkerPhones_(int ID)
        {
            return RedirectToAction("_vpWorkerPhones", "Workers", new { ID = ID, screen = "InsuranceEmployee" });
        }
        /// <summary>
        /// Attachment Show
        /// </summary>
        /// <returns>View Page</returns>
        public ActionResult vpAttachmentShow()
        {
            return View();
        }
        /// <summary>
        /// Page of Worker Process
        /// </summary>
        /// <param name="ID">Worker Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vpWorkerProces(int? ID)
        {
            return RedirectToAction("_vpWorkerProcess", "Workers", new { ID = ID });
        }
        /// <summary>
        /// Show Worker Phone In Process Details
        /// </summary>
        /// <param name="ID">Worker </param>
        /// <returns>View Page</returns>
        public ActionResult _vpWorkerPhones(int? ID)
        {
            return RedirectToAction("_vpWorkerPhones", "Workers", new { ID = ID });

        }
        /// <summary>
        /// Get Areas Filter By Office 
        /// </summary>
        /// <param name="ID">List Of OfficessCodes</param>
        /// <returns>List Of Areas</returns>
        public JsonResult GetAreas(string[] ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string AreaList = generalMethods.sConcatString(ID, ',');
                var data = db.GetOfficeInsurance(AreaList, 0);
                if (Session["areaOfficePermission"] != null)
                {
                    if (Session["areaOfficePermission"].ToString() == "1")
                        return Json(data, JsonRequestBehavior.AllowGet);
                    else
                    {
                        List<int> OfficesPerm = new List<int>();
                        OfficesPerm = Session["areaOfficePermission"].ToString().Split(',').Select(int.Parse).ToList();
                        return Json(data.Where(x => OfficesPerm.Contains(x.officeInsuranceCode)), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Show Opposition For Process
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vpOpposition(int ProcessId)
        {
            return RedirectToAction("_vpOppositionIndex", "Opposition", new { ProcessId = ProcessId, screen = "vProcessShowIndex" });
        }
        /// <summary>
        /// Minimum And Maximum Workers In Process
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vpMinMax(int ProcessId)
        {
            return RedirectToAction("_vpMinMax", "Process", new { processID = ProcessId });
        }
        /// <summary>
        /// Previous Period Of Worker In Process
        /// </summary>
        /// <param name="workerId">Worker Code</param>
        /// <param name="processId">Process Code</param>
        /// <returns>View Page</returns>
        public ActionResult _vppreviousPeriod(int workerId, int processId)
        {
            return RedirectToAction("_vpPeriodMonth", "previousPeriod", new { processID = processId, workerCode = workerId });
        }
        /// <summary>
        /// Notes Of Process
        /// </summary>
        /// <param name="processID">Process Code</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <param name="Screen">Current Page</param>
        /// <returns>View Page</returns>
        public ActionResult _vpProcessNotes(int? processID, string areas, string Offices, string Screen)
        {
            return RedirectToAction("_vpNotesInsurance", "Process", new { processID = processID, screen = Screen, areas = areas, Offices = Offices });
        }
        /// <summary>
        /// Contractors In Process Request
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <returns>View Page</returns>
        public ActionResult ProcessReqContractors(int ProcessId)
        {
            return RedirectToAction("_vpContractors", "Contractor", new { processID = ProcessId, screen = "InsuranceEmployee" });
        }
        /// <summary>
        /// Get Process Details
        /// </summary>
        /// <param name="ProcessNum">Process Number</param>
        /// <param name="NationalId">National Id </param>
        /// <param name="Date">Date Of Process</param>
        /// <returns>Data Of Process</returns>
        public JsonResult getProcessDet(string ProcessNum, string NationalId, string Date)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string processNumber = ProcessNum.Substring(8, 5);
                string officeId = ProcessNum.Substring(2, 2);
                string areaId = ProcessNum.Substring(0, 2);
                int officeInsuranceCode = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceID == officeId && x.area.areaID == areaId).officeInsuranceCode;
                string processYear = ProcessNum.Substring(4, 4);
                int pc = db.processes.FirstOrDefault(p => p.processNumber == processNumber && p.processYear == processYear && p.officeInsuranceCode == officeInsuranceCode).processCode;
                /// first check if worker have any preventives to attend  
                var model = new List<GetWorkerInformation_Result>();
                if (String.IsNullOrEmpty(Date))
                    model = db.GetWorkerInformation(NationalId, null, null, pc).ToList();
                else
                {
                    DateTime date = Convert.ToDateTime(Date);
                    model = db.GetWorkerInformation(NationalId, null, date, pc).ToList();
                }
                if (model[0].msg != null)
                {
                    var data = model[0].msg;
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    //check first if process is end
                    bool check = db.processes.Any(x => x.processNumber == processNumber &&
                                                       x.processYear == processYear && x.officeInsuranceCode == officeInsuranceCode &&
                                                        x.realDateEndProcess != null);
                    if (check == true)
                    {
                        var data = "عفوا هذه العمليه منتهيه المده";
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        var data = (from p in db.processes
                                    join type in db.processTypes on p.processTypeCode equals type.processTypeCode
                                    join Site in db.processSites on p.processCode equals Site.processCode
                                    into joined
                                    from j in joined.DefaultIfEmpty()
                                    from Site in joined.DefaultIfEmpty()
                                    where p.processNumber == processNumber && p.processYear == processYear && p.officeInsuranceCode == officeInsuranceCode
                                    select new
                                    {
                                        p.processCode,
                                        p.processName,
                                        siteAddress = j.siteAddress,
                                        type.processTypeName
                                    }).FirstOrDefault();

                        if (Session["processCode"] == null)
                            Session["processCode"] = data.processCode;

                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Save Worker Attendance
        /// </summary>
        /// <param name="formcollection">Data Of Worker Attendance</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page Of Worker</returns>
        public ActionResult _vpSaveAttendance(FormCollection formcollection, string areas, string Offices)
        {
            try
            {
                int workerCode = 0, processCode = 0;
                string national = formcollection["WNatNum"].ToString(); // االرقم القومى
                string insuranceNum = formcollection["WInNum"].ToString(); // الرقم التأمينى
                string screen = (formcollection["screen"] == null ? null : formcollection["screen"].ToString());
                workerCode = (from wor in db.workers
                              where (wor.workerNationalID == national)
                              select new
                              {
                                  wor.workerCode
                              }).FirstOrDefault().workerCode;
                if (formcollection["PrNum"] == null)
                    processCode = Convert.ToInt32(Session["processCode"].ToString());
                else
                {
                    string processNumber = formcollection["PrNum"].ToString().Substring(8, 5);
                    processCode = (from p in db.processes
                                   where (p.processNumber == processNumber)
                                   select new
                                   {
                                       p.processCode
                                   }).FirstOrDefault().processCode;
                }
                workerAttendance newObj = new workerAttendance();
                newObj.processCode = processCode;
                newObj.workerCode = workerCode;
                newObj.typeAttendence = 0;
                newObj.userInsertCode = Convert.ToInt32(Session["uc"].ToString());
                newObj.dateInsert = DateTime.Now;
                newObj.dateAttendence = (formcollection["DAtten"] == null ? DateTime.Now : Convert.ToDateTime(formcollection["DAtten"].ToString()));
                db.workerAttendances.Add(newObj);
                if (db.SaveChanges() > 0)
                {
                    #region send workers to wsdl  
                    /// هنا لو العامل او حضور ليه في الشهر هبعته للتأمينات
                    /// لو مش اول حضور ليه خلاص مجرد هحفظه عندى فقط
                    /// هنا هشوف العامل ده حالته ايه ف الشهر ده
                    ///return 0 >> not save in this month
                    /// return 1 >> have more attend in this month 
                    /// return ' date ' >> this is first attend in this month and send to WSDL

                    var CheckAttend = new List<checkWorkerAttendance_Result>();
                    CheckAttend = db.checkWorkerAttendance(workerCode).ToList();
                    if (CheckAttend[0].Attend != "0" || CheckAttend[0].Attend != "1")
                    {
                        try
                        {
                            // هنا هجيب التفاصيل الى هبعتها للويسدل
                            var data = new List<GetWorkerAttendInsurance_Result>();
                            data = db.GetWorkerAttendInsurance(workerCode).ToList();
                            long processNumber = long.Parse( data[0].areaID + data[0].officeInsuranceID + data[0].processYear + data[0].processNumber + data[0].contractorProcessNumber);
                            int Date = int.Parse(CheckAttend[0].Attend.Replace("-", ""));
                            long CareerCode = Convert.ToInt64(data[0].careerID.ToString());
                            int ContractorInsNum = int.Parse(data[0].referenceSideContractorInsuranceNum);
                            long workerNationalID = Convert.ToInt64(data[0].workerNationalID);
                            // هنا بقي هنادى على الويسدل الى هبعتلها اول حضور للعامل في الشهر ده 
                            string[] ReturnStatus = attendance_wsdl.att_data("1", workerNationalID, Date, data[0].legalEntityCode.ToString(),
                               ContractorInsNum, processNumber,Convert.ToInt32(CareerCode), 0, 0, Date);
                            if (ReturnStatus[0] == "1")
                                TempData["msg"] = generalVariables.SaveDone;
                            else if (ReturnStatus[0] == "2")
                                //TempData["msg"] = generalVariables.AttendInsuranceNotSent;
                                TempData["msg"] = generalVariables.SaveDone;
                            else
                                TempData["msg"] = generalVariables.SaveNotDone;
                          db.spUpdateWorkerAttendance(newObj.workerAttendanceCode.ToString(), ReturnStatus[0], ReturnStatus[1], DateTime.Now.ToString());

                        }
                        catch
                        {
                            TempData["msg"] = generalVariables.AttendInsuranceNotSent;
                        }
                    }
                    #endregion
                }
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
                if (String.IsNullOrEmpty(screen))
                    return RedirectToAction("vpWorkers");
                else if (screen == "_vpAddWorkers")
                    return RedirectToAction("vpWorkers", "InsuranceEmployee", new { areas = areas, cp = cp_, cd = cd_, Offices = Offices });
                else
                    return RedirectToAction("vAttendanceIndex", "attendance");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
