using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Process
{
    /// <summary>
    ///   Controller Of 'Processes - Request Processes - Processes Of Reference side  - Processes Of Contractor '.
    /// </summary>
    public class ProcessController : Controller
    {
        // GET: Process العمليات
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        static ProcessRequest oProcessRequest = new ProcessRequest();
        static bool bISSearch = false;
        ApiNationalAuthority.Models.GeneralMethods generalMethods = new ApiNationalAuthority.Models.GeneralMethods();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static int iProcessCode;
        static int PC;
        static int? cp_, cd_;
        ProcessRequest OprocessRequest = new ProcessRequest();
        static ProcessRequest Omodel;
        static int ProcessId;

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public ProcessController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Display Buttons Of ' Processes - Request Processes - Processes Of Reference side  - Processes Of Contractor '.
        /// </summary>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices Selected In The Search. </param>
        /// <param name="s"> No Search. </param>
        /// <param name="cd"> Code Of Module Insurance Officer. </param>
        /// <param name="pCode"> Process Code. </param>
        /// <param name="pNum"> Process Number. </param>
        /// <param name="type"> Type Of Notification. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessIndexShow(int? cp, string areas, string Offices, int? s, int? cd, int? pCode, string pNum, int? type)
        {
            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                db.UpdateNotificationSeen(uc, pCode, type);

                if (Session["CheckModule"] == null)
                    Session["CheckModule"] = db.CheckModuleUserPermisiom(uc, 1).ToList();

                cp_ = cp;
                cd_ = cd;

                if (cd != null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();

                // get from notification جايه من المزيد الخاص بالاشعارات
                if (!String.IsNullOrEmpty(pNum) || pCode != null)
                {
                    bISSearch = true;
                    return RedirectToAction("vSearchNotify", "Process", new { processNum = pNum, areas = areas, Offices = Offices, procCode = pCode, screenName = "../Contractor/vContractorIndex" });
                }
                else if (cp == 1 || s == 1) // s = 1 جايه من البحث 
                    bISSearch = false;

                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Display Data Of ' Process - Request Process - Processes Of Reference side  - Processes Of Contractor '
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="screenName"> Screen Name. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="cd"> Code Of Module Insurance Officer. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessTable(int? inPage, string screenName, string areas, string Offices, int? cd)
        {
            try
            {
                cd_ = cd;
                int uc = Convert.ToInt32(Session["uc"].ToString());
                if (cd != null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();

                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;
                areas = (Session["AreaSearch"] == null ? areas : Session["AreaSearch"].ToString());
                Offices = (Session["SearchOffice"] == null ? Offices : Session["SearchOffice"].ToString());

                // مفيش سيرش 
                if (!bISSearch)
                {
                    List<string> searchProcess = new List<string>();
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم

                    if (screenName.Contains("vContractorIndex")) // Contractors موديول المقاولين 
                        oProcessRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetProcessUserCode", uc.ToString());
                    else if (screenName.Contains("vProcessRequest")) // Request Process طلبات العمليات
                    {
                        bool? isAdmin = db.users.FirstOrDefault(user => user.userCode == uc).isAdmin;
                        if (isAdmin == true)
                            searchProcess.Add((!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))  AND (process.deleteProcess = 0 OR process.deleteProcess IS NULL )" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + "))) AND (process.deleteProcess = 0 OR process.deleteProcess IS NULL )" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ") AND (process.deleteProcess = 0 OR process.deleteProcess IS NULL )"))); //  بحث المناطق والمكاتب
                        else
                            searchProcess.Add(" AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ") AND (process.deleteProcess = 0 OR process.deleteProcess IS NULL )"); //  بحث المناطق والمكاتب

                        searchProcess.Add("");
                        oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchRequestProcess", searchProcess);

                        return PartialView(oProcessRequest.LModels.OrderByDescending(x => x.sDateInsert).ToPagedList(insPageNumber ?? 1, iPageSize));
                    }
                    else if (screenName.Contains("vProcessShowIndex")) // Process Office عمليات موظف التأمينات  
                    {
                        searchProcess.Add(!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                        oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchOfficeProcess", searchProcess);
                    }
                }

                if (oProcessRequest == null)
                {
                    oProcessRequest = new ProcessRequest();
                    oProcessRequest.LModels = new List<ProcessModel>();
                }
                if (oProcessRequest.LModels == null)
                    oProcessRequest.LModels = new List<ProcessModel>();

            }
            catch (Exception ex)
            {
                TempData["checkdata"] = ex;
            }
            return PartialView(oProcessRequest.LModels.OrderByDescending(x => x.sDateStartProcess).ToPagedList(insPageNumber ?? 1, iPageSize));
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <param name="screenName"> Screen Name. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpSearchProcess(string screenName, string areas, string Offices)
        {
            try
            {
                if (screenName.Contains("vProcessRequest"))
                    ViewBags("-2", "-2", "-2", "-2", "-2", "-2");
                else
                    viewBagSearch();

                TempData["screenName"] = screenName;
                TempData["areas"] = areas;
                TempData["Offices"] = Offices;

                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Show View Of Search.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult vSearchProcess()
        {
            return View();
        }

        /// <summary>
        ///   Search With Special Parameters. 
        /// </summary>
        /// <param name="procCode"> Process Code. </param>
        /// <param name="processName"> Process Name. </param>
        /// <param name="processNum"> Process Number. </param>
        /// <param name="processType"> List Of Process Type. </param>
        /// <param name="referenceSide"> List Of Reference Sides. </param>
        /// <param name="mainContractor"> List Of Main Contractors. </param>
        /// <param name="processStatus"> List Of Process Status. </param>
        /// <param name="OperationNumber"> Operation Number. </param>
        /// <param name="documentType"> List Of Document Types. </param>
        /// <param name="isLimited"> Check Processes Is Limited Or Not. </param>
        /// <param name="fullBudget"> Full Budget Of Processes. </param>
        /// <param name="dateStartProcess"> Date Start Process. </param>
        /// <param name="dateEndProcess"> Date End Process. </param>
        /// <param name="dateDocument"> Date Document. </param>
        /// <param name="buildingNumberProcessSite"> Building Number Process Site. </param>
        /// <param name="siteAddressProcessSite">Site Address Process Site. </param>
        /// <param name="govermentCodeProcessSite"> List Of Goverment Code Of Process Site. </param>
        /// <param name="centerCodeProcessSite"> List Of Center Code Of Process Site. </param>
        /// <param name="villageCodeProcessSite"> List Of Village Code Of Process Site. </param>
        /// <param name="buildingNumberProcessUserLettersSite"> Building Number Process User Letters Site. </param>
        /// <param name="siteAddressProcessUserLettersSite">>Site Address Process User Letters Site. </param>
        /// <param name="govermentCodeProcessUserLettersSite"> List Of Goverment Code Of Process User Letters Site. </param>
        /// <param name="centerCodeProcessUserLettersSite"> List Of Center Code Of Process User Letters Site. </param>
        /// <param name="villageCodeProcessUserLettersSite"> List Of Village Code Of Process User Letters Site. </param>
        /// <param name="nameOwnerPermision"> Name Of Owner Permission. </param>
        /// <param name="addressOwnerPermision"> Address Of Owner Permission. </param>
        /// <param name="subContractors"> List Of Sub Contractors Codes. </param>
        /// <param name="subContractorNum"> Insurance Number Of Sub Contractors. </param>
        /// <param name="missionSubContractor"> List Of Mission Sub Contractors Codes. </param>
        /// <param name="workers"> List Of Workers Codes. </param>
        /// <param name="WorkerInsurNum"> Insurance Number Of Workers. </param>
        /// <param name="WorkerNatIDNum"> National ID Of Workers. </param>
        /// <param name="careerWorker"> List Of Careers Workers Codes. </param>
        /// <param name="RefSideNum"> Insurance Number Of Reference Side. </param>
        /// <param name="RefSideName"> Reference Side Name. </param>
        /// <param name="ContractotNum"> Insurance Number Of Contractor. </param>
        /// <param name="ContractotName"> Contractor Name. </param>
        /// <param name="subContractorName"> Sub Contractor Name. </param>
        /// <param name="WorkerName"> Worker Name. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="refContractor"> Notifier. </param>
        /// <param name="procUser"> Process Users. </param>
        /// <param name="incommingNumber"> Incomming Number. </param>
        /// <param name="isExclude"> Check If This Process Is Exclude Or Not. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpSearchProcess(int? procCode, string processName, string processNum, string[] processType, string[] referenceSide, string[] mainContractor, string[] processStatus,
                  string OperationNumber, string[] documentType, bool? isLimited, string fullBudget, DateTime? dateStartProcess, DateTime? dateEndProcess, DateTime? dateDocument,
                  int? buildingNumberProcessSite, string siteAddressProcessSite, string[] govermentCodeProcessSite, string[] centerCodeProcessSite, string[] villageCodeProcessSite,
                  int? buildingNumberProcessUserLettersSite, string siteAddressProcessUserLettersSite, string[] govermentCodeProcessUserLettersSite, string[] centerCodeProcessUserLettersSite, string[] villageCodeProcessUserLettersSite, string nameOwnerPermision, string addressOwnerPermision,
                  string[] subContractors, string subContractorNum, string[] missionSubContractor, string[] workers, string WorkerInsurNum, string WorkerNatIDNum, string[] careerWorker,
                  string RefSideNum, string RefSideName, string ContractotNum, string ContractotName, string subContractorName, string WorkerName, string areas, string Offices, string refContractor, string procUser, string incommingNumber,
                  bool? isExclude)
        {
            try
            {
                bISSearch = true;

                string sProcessType = generalMethods.sConcatString(processType, ','); // نوع العمليه
                string sReferenceSide = generalMethods.sConcatString(referenceSide, ','); // كود جهه الاسناد 
                string sMainContractor = generalMethods.sConcatString(mainContractor, ','); // كود المقاول الرئيسي
                string sDocumentType = generalMethods.sConcatString(documentType, ','); // نوع المستند
                string sGovermentCodeProcessSite = generalMethods.sConcatString(govermentCodeProcessSite, ','); // محافظه العمليه
                string sCenterCodeProcessSite = generalMethods.sConcatString(centerCodeProcessSite, ','); // مركز - قسم العمليه
                string sVillageCodeProcessSite = generalMethods.sConcatString(villageCodeProcessSite, ','); // قريه -شياخه العملية
                string sGovermentCodeProcessUserLettersSite = generalMethods.sConcatString(govermentCodeProcessUserLettersSite, ','); // محافظه  المخطار للمراسله
                string sCenterCodeProcessUserLettersSite = generalMethods.sConcatString(centerCodeProcessUserLettersSite, ','); // مركز - قسم  المخطار للمراسله
                string sVillageCodeProcessUserLettersSite = generalMethods.sConcatString(villageCodeProcessUserLettersSite, ','); // قريه -شياخه  المخطار للمراسله
                string sSubContractors = generalMethods.sConcatString(subContractors, ','); // اكواد المقاول من الباطن
                string sMissionSubContractor = generalMethods.sConcatString(missionSubContractor, ','); // المهمه المسنده اليه
                string sWorkers = generalMethods.sConcatString(workers, ','); // اسم العامل
                string sCareerWorker = generalMethods.sConcatString(careerWorker, ','); // مهنة العامل
                string sProcessStatus = generalMethods.sConcatString(processStatus, ','); // حاله العمليه

                var sOffice = Request.Form["Offices"];
                var oArea = Request.Form["areas"];
                var screenName = Request.Form["screenName"];

                string spParameters = "";
                List<string> searchProcess = new List<string>();

                // المقاول
                if (screenName == "../Contractor/vContractorIndex")
                {
                    #region search contractors
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(String.IsNullOrEmpty(processName) ? "" : processName); // اسم العمليه
                    searchProcess.Add(String.IsNullOrEmpty(processNum) ? "" : processNum); // رقم العمليه
                    searchProcess.Add(String.IsNullOrEmpty(sProcessType) ? "-1" : sProcessType); // نوع العمليه
                    searchProcess.Add(String.IsNullOrEmpty(sReferenceSide) ? "-1" : sReferenceSide); // جهه الاسناد
                    searchProcess.Add(String.IsNullOrEmpty(sMainContractor) ? "-1" : sMainContractor); // المقاول الرئيسي
                    searchProcess.Add(String.IsNullOrEmpty(sDocumentType) ? "-1" : sDocumentType); // نوع المستند
                    searchProcess.Add(isLimited == false ? "2" : "1"); // محدده المده
                    searchProcess.Add(String.IsNullOrEmpty(fullBudget) ? "-1" : fullBudget); // القيمه الكليه للعمليه
                    searchProcess.Add(dateStartProcess == null ? new DateTime(1, 1, 1).ToString() : dateStartProcess.ToString()); // ت بدء العمليه
                    searchProcess.Add(dateEndProcess == null ? new DateTime(1, 1, 1).ToString() : dateEndProcess.ToString()); // ت انتهاء العمليه
                    searchProcess.Add(buildingNumberProcessSite == null ? "-1" : buildingNumberProcessSite.ToString()); // رقم العقار لموقع العمليه
                    searchProcess.Add(String.IsNullOrEmpty(siteAddressProcessSite) ? null : siteAddressProcessSite); // عنوان موقع العمليه
                    searchProcess.Add(String.IsNullOrEmpty(sGovermentCodeProcessSite) ? "-1" : sGovermentCodeProcessSite); // محافظه العمليه
                    searchProcess.Add(String.IsNullOrEmpty(sCenterCodeProcessSite) ? "-1" : sCenterCodeProcessSite); // مركز - قسم العمليه
                    searchProcess.Add(String.IsNullOrEmpty(sVillageCodeProcessSite) ? "-1" : sVillageCodeProcessSite); // قريه -شياخه العمليه
                    searchProcess.Add(buildingNumberProcessUserLettersSite == null ? "-1" : buildingNumberProcessUserLettersSite.ToString()); // رقم العقار للمخطار للمراسله
                    searchProcess.Add(String.IsNullOrEmpty(siteAddressProcessUserLettersSite) ? null : siteAddressProcessUserLettersSite); // عنوان للمخطار للمراسله
                    searchProcess.Add(String.IsNullOrEmpty(sGovermentCodeProcessUserLettersSite) ? "-1" : sGovermentCodeProcessUserLettersSite); // محافظه المخطار للمراسله
                    searchProcess.Add(String.IsNullOrEmpty(sCenterCodeProcessUserLettersSite) ? "-1" : sCenterCodeProcessUserLettersSite); // مركز - قسم المخطار للمراسله
                    searchProcess.Add(String.IsNullOrEmpty(sVillageCodeProcessUserLettersSite) ? "-1" : sVillageCodeProcessUserLettersSite); // قريه -شياخه المخطار للمراسله
                    searchProcess.Add(String.IsNullOrEmpty(nameOwnerPermision) ? "" : nameOwnerPermision); // اسم صاحب الترخيص
                    searchProcess.Add(String.IsNullOrEmpty(addressOwnerPermision) ? "" : addressOwnerPermision); // عنوان صاحب الترخيص
                    searchProcess.Add(String.IsNullOrEmpty(sSubContractors) ? "-1" : sSubContractors); // اسم المقاول من الباطن
                    searchProcess.Add(String.IsNullOrEmpty(sMissionSubContractor) ? "-1" : sMissionSubContractor); // المهمه المسنده اليه
                    searchProcess.Add(String.IsNullOrEmpty(sWorkers) ? "-1" : sWorkers); // كود العامل
                    searchProcess.Add(String.IsNullOrEmpty(sCareerWorker) ? "-1" : sCareerWorker); // مهنة العامل
                    searchProcess.Add(String.IsNullOrEmpty(WorkerNatIDNum) ? "-1" : WorkerNatIDNum); // الرقم القومى للعامل
                    searchProcess.Add(String.IsNullOrEmpty(WorkerInsurNum) ? "-1" : Convert.ToInt32(WorkerInsurNum).ToString("000000000")); // الرقم التأمينى للعامل
                    searchProcess.Add(String.IsNullOrEmpty(RefSideNum) ? "-1" : Convert.ToInt32(RefSideNum).ToString("000000000")); // الرقم التأمينى لجهه الاسناد
                    searchProcess.Add(String.IsNullOrEmpty(RefSideName) ? "" : RefSideName); // اسم جهه الاسناد 
                    searchProcess.Add(String.IsNullOrEmpty(ContractotNum) ? "-1" : Convert.ToInt32(ContractotNum).ToString("000000000")); // الرقم التأمينى للمقاول الرئيسي  
                    searchProcess.Add(String.IsNullOrEmpty(ContractotName) ? "" : ContractotName); // اسم المقاول الرئيسي  
                    searchProcess.Add(String.IsNullOrEmpty(subContractorNum) ? "-1" : Convert.ToInt32(subContractorNum).ToString("000000000")); // الرقم التأمينى للمقاول من باطن   
                    searchProcess.Add(String.IsNullOrEmpty(subContractorName) ? "" : subContractorName); // اسم المقاول من باطن  
                    searchProcess.Add(String.IsNullOrEmpty(WorkerName) ? "" : WorkerName); // اسم العامل  
                    searchProcess.Add(String.IsNullOrEmpty(sProcessStatus) ? "-1" : sProcessStatus); // حاله العمليه 
                    searchProcess.Add(String.IsNullOrEmpty(incommingNumber) ? "" : incommingNumber); // رقم الوارد 
                    searchProcess.Add(dateDocument == null ? new DateTime(1, 1, 1).ToString() : dateDocument.ToString()); // ت المستند
                    #endregion
                }
                else
                {
                    #region search officer insurance
                    spParameters += ((buildingNumberProcessSite != null) ? " AND (processSite.buildingNumber LIKE '" + buildingNumberProcessSite + "')" : ""); // رقم العقار لموقع العمليه
                    spParameters += (!String.IsNullOrEmpty(siteAddressProcessSite) ? " AND (processSite.siteAddress LIKE '%" + siteAddressProcessSite + "%')" : ""); // عنوان موقع العمليه
                    spParameters += (!String.IsNullOrEmpty(sGovermentCodeProcessSite) ? " AND (processSite.govermentCode IN (select * from [codes].[UTILfn_Split]('" + sGovermentCodeProcessSite + "',',')))" : ""); // محافظه العمليه
                    spParameters += (!String.IsNullOrEmpty(sCenterCodeProcessSite) ? " AND (processSite.centerCode IN (select * from [codes].[UTILfn_Split]('" + sCenterCodeProcessSite + "',',')))" : ""); // مركز - قسم العمليه
                    spParameters += (!String.IsNullOrEmpty(sVillageCodeProcessSite) ? " AND (processSite.villageCode IN (select * from [codes].[UTILfn_Split]('" + sVillageCodeProcessSite + "',',')))" : ""); // قريه -شياخه العمليه
                    spParameters += ((buildingNumberProcessUserLettersSite != null) ? " AND (processUserLettersSite.buildingNumber LIKE '" + buildingNumberProcessUserLettersSite + "')" : ""); // رقم العقار للمخطار للمراسله
                    spParameters += (!String.IsNullOrEmpty(siteAddressProcessUserLettersSite) ? " AND (processUserLettersSite.siteAddress LIKE '%" + siteAddressProcessUserLettersSite + "%')" : ""); // عنوان للمخطار للمراسله
                    spParameters += (!String.IsNullOrEmpty(sGovermentCodeProcessUserLettersSite) ? " AND (processUserLettersSite.govermentCode IN (select * from [codes].[UTILfn_Split]('" + sGovermentCodeProcessUserLettersSite + "',',')))" : ""); // محافظه المخطار للمراسله
                    spParameters += (!String.IsNullOrEmpty(sCenterCodeProcessUserLettersSite) ? " AND (processUserLettersSite.centerCode IN (select * from [codes].[UTILfn_Split]('" + sCenterCodeProcessUserLettersSite + "',',')))" : ""); // مركز - قسم المخطار للمراسله
                    spParameters += (!String.IsNullOrEmpty(sVillageCodeProcessUserLettersSite) ? " AND (processUserLettersSite.villageCode IN (select * from [codes].[UTILfn_Split]('" + sVillageCodeProcessUserLettersSite + "',',')))" : "");  // قريه -شياخه المخطار للمراسله
                    spParameters += (!String.IsNullOrEmpty(nameOwnerPermision) ? " AND (process.nameOwnerPermision LIKE '%" + nameOwnerPermision + "%')" : ""); // اسم صاحب الترخيص
                    spParameters += (!String.IsNullOrEmpty(addressOwnerPermision) ? " AND (process.addressOwnerPermision LIKE '%" + addressOwnerPermision + "%')" : ""); // عنوان صاحب الترخيص
                    spParameters += (!String.IsNullOrEmpty(sSubContractors) ? " AND (SubContractors.contractorCode IN(select * from [codes].[UTILfn_Split]('" + sSubContractors + "', ',')) AND SubContractors.contractorType = 0)" : ""); // كود المقاول من الباطن
                    spParameters += (!String.IsNullOrEmpty(sMissionSubContractor) ? " AND (missionSubContractor.processTypeCode IN (select * from [codes].[UTILfn_Split]('" + sMissionSubContractor + "',',')))" : ""); // المهمه المسنده اليه
                    spParameters += (!String.IsNullOrEmpty(sWorkers) ? " AND (workerAttendance.workerCode IN (select * from [codes].[UTILfn_Split]('" + sWorkers + "',',')))" : ""); // كود العامل
                    spParameters += (!String.IsNullOrEmpty(sCareerWorker) ? " AND (workerDetails.careerCode IN (select * from [codes].[UTILfn_Split]('" + sCareerWorker + "',',')))" : ""); // مهنة العامل
                    spParameters += (!String.IsNullOrEmpty(WorkerNatIDNum) ? " AND (worker.workerNationalID LIKE '" + WorkerNatIDNum + "')" : ""); // الرقم القومى للعامل
                    spParameters += (!String.IsNullOrEmpty(WorkerInsurNum) ? " AND (worker.workerInsuranceNum LIKE '" + WorkerInsurNum + "')" : ""); // الرقم التأمينى للعامل
                    spParameters += (!String.IsNullOrEmpty(WorkerName) ? " AND (worker.workerName LIKE '%" + WorkerName + "%')" : ""); // اسم العامل

                    spParameters += (!String.IsNullOrEmpty(sProcessStatus) ? @" AND ((1 IN (select * from [codes].[UTILfn_Split]('" + sProcessStatus + @"',',')) AND (CONVERT(DATE,GETDATE()) <= CONVERT(DATE,ISNULL(dateEndProcess,DATEADD(DAY,1,CONVERT(DATE,GETDATE()))))) AND (CONVERT(DATE,GETDATE()) NOT BETWEEN CONVERT(DATE,ISNULL(TBL1.dateStartProcessStop,DATEADD(DAY,1,CONVERT(DATE,GETDATE())))) AND CONVERT(DATE,ISNULL(TBL1.dateEndProcessStop,DATEADD(DAY,1,CONVERT(DATE,GETDATE())))))) 
                                                                          OR (2 IN(select * from [codes].[UTILfn_Split]('" + sProcessStatus + @"', ',')) AND(CONVERT(DATE, GETDATE()) > CONVERT(DATE, dateEndProcess)))
                                                                          OR (3 IN(select * from [codes].[UTILfn_Split]('" + sProcessStatus + "', ',')) AND(CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE, TBL1.dateStartProcessStop) AND CONVERT(DATE, TBL1.dateEndProcessStop)) AND(CONVERT(DATE, GETDATE()) <= CONVERT(DATE, ISNULL(dateEndProcess,GETDATE())))))" : ""); // حاله العمليه  

                    spParameters += (!String.IsNullOrEmpty(processName) ? " AND (process.processName LIKE '%" + processName + "%')" : ""); // اسم العمليه
                    spParameters += (!String.IsNullOrEmpty(processNum) ? " AND (CONCAT(codes.area.areaID,codes.officeInsurance.officeInsuranceID,process.processYear,processNumber) LIKE '%" + processNum + "%')" : ""); // رقم العمليه
                    spParameters += (!String.IsNullOrEmpty(sProcessType) ? " AND (process.processTypeCode IN (select * from [codes].[UTILfn_Split]('" + sProcessType + "', ',')))" : ""); // نوع العمليه
                    spParameters += (!String.IsNullOrEmpty(sReferenceSide) ? " AND (refSide.referenceSideContractorCode IN (select * from [codes].[UTILfn_Split]('" + sReferenceSide + "',',')))" : ""); // جهه الاسناد
                    spParameters += (!String.IsNullOrEmpty(RefSideName) ? " AND (refSide.referenceSideContractorName LIKE '%" + RefSideName + "%')" : "");// اسم جهه الاسناد 
                    spParameters += (!String.IsNullOrEmpty(RefSideNum)) ? " AND (refSide.referenceSideContractorInsuranceNum LIKE '" + Convert.ToInt32(RefSideNum).ToString("000000000") + "')" : ""; // الرقم التامينى لجهه الاسناد
                    spParameters += (!String.IsNullOrEmpty(sMainContractor) ? " AND (mainContractors.contractorCode IN (select * from [codes].[UTILfn_Split]('" + sMainContractor + "', ','))) " : ""); //  المقاول الرئيسي
                    spParameters += (!String.IsNullOrEmpty(ContractotName) ? " AND (contractors.referenceSideContractorName LIKE '%" + ContractotName + "%')" : ""); // اسم المقاول الرئيسي
                    spParameters += (!String.IsNullOrEmpty(ContractotNum)) ? " AND (contractors.referenceSideContractorInsuranceNum LIKE '" + Convert.ToInt32(ContractotNum).ToString("000000000") + "')" : ""; // الرقم التامينى للمقاول الرئيسي
                    spParameters += (!String.IsNullOrEmpty(subContractorName) ? " AND ((ontractors.referenceSideContractorName LIKE '%" + subContractorName + "%')" : ""); // اسم المقاول من باطن 
                    spParameters += (!String.IsNullOrEmpty(subContractorNum)) ? " AND (contractors.referenceSideContractorInsuranceNum LIKE '" + Convert.ToInt32(subContractorNum).ToString("000000000") + "')" : ""; // الرقم التامينى للمقاول من باطن
                    spParameters += (!String.IsNullOrEmpty(sDocumentType) ? " AND (process.documentTypeCode IN (select * from [codes].[UTILfn_Split]('" + sDocumentType + "',',')))" : ""); // نوع المستند
                    spParameters += ((isLimited == true) ? " AND (process.fullBudget IS NOT NULL)" : "");  // محدده المده
                    spParameters += (!String.IsNullOrEmpty(fullBudget) ? ((isLimited == true) ? " AND (process.fullBudget LIKE '" + fullBudget + "')" : "") : ""); // القيمه الكليه للعمليه
                    spParameters += ((dateStartProcess != null) ? " AND CONVERT(DATE, process.dateStartProcess) LIKE CONVERT(DATE, '" + dateStartProcess.ToString() + "')" : ""); //  ت بدء العمليه
                    spParameters += ((dateEndProcess != null) ? " AND CONVERT(DATE,process.dateEndProcess) LIKE CONVERT(DATE, '" + dateEndProcess.ToString() + "')" : ""); //  ت انتهاء العمليه
                    spParameters += (!String.IsNullOrEmpty(incommingNumber) ? " AND ( process.incommingNumber LIKE '%" + incommingNumber + "%')" : ""); // رقم الوارد
                    spParameters += ((dateDocument != null) ? " AND CONVERT(DATE, process.dateDocument) LIKE CONVERT(DATE, '" + dateDocument.ToString() + "')" : ""); //  ت المستند
                    spParameters += ((isExclude == true) ? " AND (process.deleteProcess = 1)" : " AND (process.deleteProcess = 0 OR process.deleteProcess IS NULL )");  // عمليه مستبعده


                    if (screenName != "../Process/vProcessRequest") //Process Office عمليات موظف التأمينات
                        spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                    else
                    {
                        int uc = Convert.ToInt32(Session["uc"].ToString());
                        bool? isAdmin = db.users.FirstOrDefault(user => user.userCode == uc).isAdmin;
                        if (isAdmin == true)
                            spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                        else
                            spParameters += " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")"; //  بحث المناطق والمكاتب
                    }

                    spParameters += (!String.IsNullOrEmpty(refContractor) ? ((refContractor == "1" ? " AND (SubContractors.contractorType = 1 AND SubContractors.contractorCode = user_.contractorCode)" : " AND (process.referenceSideCode = user_.referenceSideCode)")) : ""); // مقدم الا خطار 
                    spParameters += (!String.IsNullOrEmpty(procUser) ? (procUser == "1" ? " AND (procUser = 0 OR procUser IS NULL)" : " AND procUser <> 0") : ""); // مستخدمين العمليه
                    spParameters += ((procCode != null) ? " AND (process.processCode LIKE '" + procCode + "')" : ""); // كود العمليه

                    if (!String.IsNullOrEmpty(sMissionSubContractor) || !String.IsNullOrEmpty(subContractorName) || !String.IsNullOrEmpty(subContractorNum)) // search by sun contractor
                        spParameters += "AND (mainContractors.contractorType = 0)";
                    else if (screenName != "../Process/vProcessRequest")
                        spParameters += "AND (mainContractors.contractorType = 1)";

                    #endregion
                }

                if (screenName == "../Contractor/vContractorIndex") // Contractors المقاولين
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchProcess", searchProcess);
                else if (screenName == "../Process/vProcessRequest") // Request Process طلبات لعمليات
                {
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(spParameters); // Stored Parameters
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchRequestProcess", searchProcess);
                }
                else if (screenName == "../Process/vProcessShowIndex") // Process Office عمليات موظف التأمينات  
                {
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(spParameters); // Stored Parameters
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchOfficeProcess", searchProcess);
                }

                if (screenName == "../Process/vProcessShowIndex") // بحث عن عمليه في العمليات موظف التأمينات
                    return RedirectToAction("vProcessShowIndex", "Process", new { areas = areas, inPage = 1, cp = 0, Offices = Offices });
                else
                    return RedirectToAction(screenName, new { areas = areas, Offices = Offices });
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel(string screenName, string areas, string Offices)
        {
            bISSearch = false;
            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction(screenName, new { areas = areas, Offices = Offices });
        }


        #region Module Conractor  المقاولين

        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of Process Code.  
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="referanceSideContractorNum"> Insurance Number Of 'Reference Side - Contractor'. </param>
        /// <param name="inProcessCode"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAddProcessIndex(int? inPage, bool? referanceSideContractorNum, int? inProcessCode, string screenName)
        {
            try
            {
                insPageNumber = inPage == null ? 1 : inPage;

                ViewBags("-1", "", "", "-1", "", "");
                ViewBag.areas = new SelectList(db.spGetAreasContractor().ToList(), "areaCode", "areaIdName");
                ViewBag.Offices = new SelectList(db.spGetOfficesContractor(-1).Where(office => office.officeInsuranceCode == -2), "officeInsuranceCode", "officeIdName");

                ViewBag.legalEntity = new SelectList(new List<SelectListItem> {
                new SelectListItem{ Text="منشأه", Value = "2" },
                new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", -1);
                ProcessRequest process = new ProcessRequest();

                // Edit 
                if (inProcessCode != null)
                {
                    process = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetProcess", inProcessCode.ToString());
                    ViewBag.areas = new SelectList(db.spGetAreasContractor().ToList(), "areaCode", "areaIdName", process.OModel.iAreaCode);
                    ViewBag.Offices = new SelectList(db.spGetOfficesContractor(process.OModel.iAreaCode), "officeInsuranceCode", "officeIdName");

                    // مقاول 
                    if (referanceSideContractorNum == true)
                        ViewBag.legalEntity = new SelectList(db.legalEntities.ToList(), "legalEntityCode", "legalEntityName", process.OModel.iReferanceSideLegalEntityCode);
                    else // جهه اسناد
                        ViewBag.legalEntity = new SelectList(db.legalEntities.ToList(), "legalEntityCode", "legalEntityName", process.OModel.iContractorLegalEntityCode);

                    ViewBags(process.oProcessSiteModel.inGovermentCodeProcessSite.ToString(), process.oProcessSiteModel.inCenterCodeProcessSite.ToString(), process.oProcessSiteModel.inVillageCodeProcessSite.ToString(),
                        process.oProcessUserLettersSiteModel.inGovermentCodeProcessUserLettersSite.ToString(), process.oProcessUserLettersSiteModel.inCenterCodeProcessUserLettersSite.ToString(),
                        process.oProcessUserLettersSiteModel.inVillageCodeProcessUserLettersSite.ToString());

                    process.OModel.sIpInsert = screenName;
                }

                // الرقم التأمينى لجهه الاسناد - المقاول الرئيسي
                TempData["referanceSideContractor"] = referanceSideContractorNum;

                return PartialView(process);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///  When User Clicked On : 
        ///         Add Button : Insert Data.
        ///        Edit Button : Edit Data.  
        /// </summary>
        /// <param name="oModalRequest"> Data Of Request That Save In Database. </param>
        /// <param name="bnIsLimited"> Check Process Is Limited Or Not. </param>
        /// <param name="officeAccept"> Code Of Office That Accept This Process. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpAddProcessIndex(ProcessRequest oModalRequest, bool? bnIsLimited, string officeAccept)
        {
            try
            {
                if (oModalRequest.OModel.iProcessCode > 0) // Edit
                {
                    string id = oModalRequest.OModel.iProcessCode.ToString();
                    oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                    oModalRequest.OModel.bnIsLimited = bnIsLimited;

                    oProcessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostEditProcess", oModalRequest, id);
                    if (oProcessRequest.OModel.bIsEdit)
                    {
                        string msg = "تم تعديل العمليه بنجاح .";

                        if (!oProcessRequest.oProcessSubContractorMode.bIsEdit)
                            msg += " لكن هناك خطأ بتعديل المقاول الرئيسي للعملية . ";

                        if (!oProcessRequest.oProcessSiteModel.bIsEdit)
                            msg += " لكن هناك خطأ بتعديل عنوان العملية . ";

                        if (!oProcessRequest.oProcessUserLettersSiteModel.bIsEdit)
                            msg += " لكن هناك خطأ بتعديل عنوان المخطار للمراسله . ";

                        TempData["msg"] = "<script>sAlert('" + msg + "',0);</script>";
                    }
                    else
                        TempData["msg"] = generalVariables.EditNotDone;
                }
                else // Save
                {
                    oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                    oModalRequest.OModel.bnIsLimited = bnIsLimited;

                    oProcessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveProcess", oModalRequest, null);
                    if (oProcessRequest.OModel.bIsSaved)
                    {
                        string msg = "تم حفظ العمليه بنجاح .";

                        if (!oProcessRequest.oProcessSubContractorMode.bIsSaved)
                            msg += " لكن هناك خطأ بحفظ المقاول الرئيسي للعملية . ";

                        if (!oProcessRequest.oProcessSiteModel.bIsSaved)
                            msg += " لكن هناك خطأ بحفظ عنوان العملية . ";

                        if (!oProcessRequest.oProcessUserLettersSiteModel.bIsSaved)
                            msg += " لكن هناك خطأ بحفظ عنوان المخطار للمراسله . ";

                        TempData["msg"] = "<script>sAlert('" + msg + "',0);</script>";
                    }
                    else
                        TempData["msg"] = generalVariables.SaveNotDone;
                }

                bISSearch = false;

                if (oModalRequest.OModel.sIpInsert != null)
                    return RedirectToAction("vProcessShowIndex", "Process", new { inPage = 1, cp = 0, cd = 2 });

                return RedirectToAction("vContractorIndex", "Contractor");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="IDDelete"> ID Of Process Will Be Delete. </param>
        /// <returns> View. </returns>
        public ActionResult _vpDeleteProcess(int? inPage, string IDDelete)
        {
            try
            {
                if (!String.IsNullOrEmpty(IDDelete)) // Delete
                {
                    oProcessRequest = conApi.connectionApiDelete<ProcessRequest>("apiProcess", "DeleteProcess", IDDelete + ',' + Session["uc"].ToString());

                    TempData["msg"] = oProcessRequest.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
                    bISSearch = false;
                }

                return RedirectToAction("vContractorIndex", "Contractor");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Sub Contractor By User.
        /// </summary>
        /// <returns> Partial View. </returns>
        public ActionResult _vpSubContractorByUser()
        {
            return PartialView();
        }

        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="processNum"> Process Number. </param>
        /// <returns> View. </returns>
        [HttpPost]
         public ActionResult _vpSubContractorByUser(string processNum)
        {
            oProcessRequest.oProcessSubContractorMode = new DataAccessLayer.Models.ProcessSubContractorModel();
            oProcessRequest.oProcessSubContractorMode.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
            oProcessRequest.OModel = new DataAccessLayer.Models.ProcessModel();
            oProcessRequest.OModel.iProcessCode = Convert.ToInt32(Request.Form["processCode"]);

            oProcessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveSubContractorProcessByUser", oProcessRequest, null);

            TempData["msg"] = oProcessRequest.oProcessSubContractorMode.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone;
            return RedirectToAction("vContractorIndex", "Contractor");
        }

        #endregion

        #region  Process Office عمليات موظف التأمينات

        /// <summary>
        ///   Get All Processes.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="pNum"> Process Number. </param>
        /// <param name="pCode"> Process Code. </param>
        /// <param name="type"> Type Of Notification. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessShowIndex(int? inPage, int? cp, string areas, string Offices, string pNum, int? pCode, int? type)
        {
            try
            {
                cp_ = cp;
                if (pCode != null)
                {
                    int user_code = Convert.ToInt32(Session["uc"].ToString());
                    db.UpdateNotificationSeen(user_code, pCode, type);
                    if (Session["CheckModule"] == null)
                        Session["CheckModule"] = db.CheckModuleUserPermisiom(user_code, 1).ToList();
                }
                if (!String.IsNullOrEmpty(pNum) || pCode != null)
                {
                    bISSearch = true;
                    return RedirectToAction("vSearchNotify", "Process", new { processNum = pNum, areas = areas, Offices = Offices, procCode = pCode, screenName = "vProcessShowIndex" });
                }
                // اول مره يدخل الصفحه دى (داس على الرئيسيه)
                else if (cp == 1)
                {
                    bISSearch = false;
                    Session["processCode"] = null;
                }

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///    Back Process To Process Requests Page.
        /// </summary>
        /// <param name="procCode"> Process Code. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        public ActionResult vBackProcessToProcessRequest(int? procCode, string areas, string Offices)
        {
            db.spBackProcessToProceeRequest(procCode, Convert.ToInt32(Session["uc"].ToString()), null);
            TempData["msg"] = generalVariables.BackProcessDone;
            return RedirectToAction("vProcessShowIndex", "Process", new { areas = areas, inPage = 1, cp = 0, Offices = Offices, cd = 2 });
        }
        #endregion

        #region ProcessRequest طلبات العمليات

        /// <summary>
        ///   Get All Request Processes. 
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="pNum"> Process Number. </param>
        /// <param name="pCode"> Process Code. </param>
        /// <param name="type"> Type Of Notification. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessRequest(int? inPage, int? cp, string areas, string Offices, string pNum, int? pCode, int? type)
        {
            try
            {
                TempData["areas"] = areas;
                TempData["Offices"] = Offices;
                cp_ = cp;
                if (pCode != null)
                {
                    int user_code = Convert.ToInt32(Session["uc"].ToString());
                    db.UpdateNotificationSeen(user_code, pCode, type);
                    if (Session["CheckModule"] == null)
                        Session["CheckModule"] = db.CheckModuleUserPermisiom(user_code, 1).ToList();
                }
                if (!String.IsNullOrEmpty(pNum) || pCode != null)
                {
                    bISSearch = true;
                    return RedirectToAction("vSearchNotify", "Process", new { processNum = pNum, areas = areas, Offices = Offices, procCode = pCode, screenName = "vProcessRequest" });
                }
                // اول مره يدخل الصفحه دى (داس على الرئيسيه)
                else if (cp == 1)
                {
                    bISSearch = false;
                    Session["processCode"] = null;
                }

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///    Show View Of Accept Process.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        public ActionResult _vpAcceptProcess(int processCode, string areas, string Offices)
        {
            try
            {
                ProcessRequest process = new ProcessRequest();
                process = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetMainProcessData", processCode.ToString());

                process.OModel.sProcessNumber = null;
                ViewBag.areas = new SelectList(db.spAllAreas(0, 1, "", null).ToList(), "areaCode", "areaIDName", process.OModel.iAreaCode);
                ViewBag.Offices = new SelectList(db.GetOfficeInsurance(process.OModel.iAreaCode.ToString(), null), "officeInsuranceCode", "officeInsuranceIDName", process.OModel.iOfficeCode);

                TempData["areas"] = areas;
                TempData["Offices"] = Offices;

                return View(process);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Accept Process In Database.
        /// </summary>
        /// <param name="oModalRequest"> Data Of Request That Save In Database. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        [HttpPost]        
        public ActionResult _vpAcceptProcess(ProcessRequest oModalRequest, string areas, string Offices)
        {
            try
            {
                List<string> processFullNmber = new List<string>();
                processFullNmber.Add(oModalRequest.OModel.iProcessCode.ToString()); // Process Code
                processFullNmber.Add(oModalRequest.OModel.iOfficeCode.ToString()); // Office Code
                processFullNmber.Add(oModalRequest.OModel.inProcessYear.ToString()); // Process Year
                processFullNmber.Add(oModalRequest.OModel.sProcessNumber); // Process Number
                processFullNmber.Add(Session["uc"].ToString()); // User Accept Code
                processFullNmber.Add(Convert.ToInt32(oModalRequest.OModel.sIncommingNumber).ToString("00000000")); // Incomming Number

                oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostAcceptProcess", processFullNmber);

                TempData["msg"] = oProcessRequest.OModel.bIsSaved ? "<script>sAlertBasic('تم الموافقه على الطلب بنجاح ورقم العمليه " + oProcessRequest.OModel.sProcessName + ".',0);</script>" :
                                                                    "<script>sAlertBasic('" + oProcessRequest.OModel.sProcessNotes + "',1);</script>";

                return RedirectToAction("vProcessRequest", new { areas = areas, Offices = Offices, cp = 1, cd = 3 });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Exclusion Process.
        /// </summary>
        /// <param name="IDDelete"> ID Of Process Will Be Exclude. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        public ActionResult _vpProcessCancel(int IDDelete, string areas, string Offices)
        {
            try
            {
                oProcessRequest = conApi.connectionApiDelete<ProcessRequest>("apiProcess", "DeleteProcessRequest", IDDelete.ToString() + ',' + Session["uc"].ToString());
                if (oProcessRequest.bIsDeleted)
                    TempData["msg"] = generalVariables.processCancel;
                else
                    TempData["msg"] = generalVariables.processCancelNot;
                return RedirectToAction("vProcessRequest", new { areas = areas, Offices = Offices, cp = 1, cd = 3 });
            }
            catch { return RedirectToAction("Error", "Home"); }
        }


        /// <summary>
        ///   Search In 'Reference Side - Contractors'.
        /// </summary>
        /// <returns> Partial View. </returns>
        public ActionResult _vpSearchRefSideCont()
        {
            return PartialView();
        }

        #endregion

        #region Process Details تفاصيل العملية

        /// <summary>
        ///   Details Of Process.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="ProcessId"> Process code. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessIndex(int? pc, int? ProcessId) // تفاصيل العمليه
        {
            try
            {
                bISSearch = false;
                Session["processCode"] = ProcessId == null ? Convert.ToInt32(Session["processCode"].ToString()) : ProcessId;
                iProcessCode = (ProcessId == null ? 0 : (int)ProcessId);
                Omodel = new ProcessRequest();
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Details Of Process 'Modal'.
        /// </summary>
        /// <param name="inProcessCode"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessDetail(int? inProcessCode)
        {
            try
            {
                ProcessModel oProcessModel = new ProcessModel();

                if (inProcessCode != null)
                    oProcessModel.vGetMainProcessData(inProcessCode.ToString(), null);

                return PartialView(oProcessModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Details Of Process.
        /// </summary>
        /// <param name="ProcessID"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessDetails(int? ProcessID)
        {
            try
            {
                if (Omodel == null)
                {
                    Omodel = new ProcessRequest();
                    Omodel.LModels = new List<ProcessModel>();
                    Session["procStopActive"] = null;
                }

                iProcessCode = (ProcessID == null ? (Convert.ToInt32(Session["processCode"].ToString())) : (int)ProcessID);
                if (iProcessCode != 0)
                {
                    OprocessRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetProcessDetails", iProcessCode.ToString());
                    if (OprocessRequest == null)
                        OprocessRequest = new ProcessRequest();

                    Omodel.LModels = OprocessRequest.LModels;
                    Session["procStopActive"] = OprocessRequest.OModel.sProcessStopNotActive;
                }
                if (OprocessRequest.LModels == null)
                    OprocessRequest.LModels = new List<ProcessModel>();

                return PartialView(OprocessRequest.LModels);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Details Of Process.
        /// </summary>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessDetailsModel()
        {
            try
            {
                if (iProcessCode != 0)
                    return PartialView(Omodel.LModels);

                return PartialView(OprocessRequest.LModels);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region Ajex Methods

        /// <summary>
        ///   Get 'Reference Side - Contractor' Data By Insurance Number.
        /// </summary>
        /// <param name="ID"> Insurance Number Of 'Reference Side - Contractor'. </param>
        /// <returns> Json. </returns>
        public JsonResult getRefSideContByInsurNum(string ID) // Find Reference Side And Contractors By Insurance Number
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                if (ID.Length < 9)
                    ID = Convert.ToInt32(ID).ToString("000000000");

                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetRefSideContByInsurNum", String.IsNullOrEmpty(ID) ? "null" : ID);
                return Json(data.oRefSideCont.dataDrob, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get Insurance Number Of 'Reference Side - Contractor' By 'Reference Side - Contractor' Code.
        /// </summary>
        /// <param name="ID"> 'Reference Side - Contractor' Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getInsurNumByRefSideCont(string ID) // Find Insurance Number By Reference Side And Contractors 
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetInsurNumByRefSideCont", ID);
                return Json(data.oRefSideCont.sReferanceSideContractorNum, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary> 
        ///   Get 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> Insurance Number. </param>
        /// <returns> Json. </returns>
        public JsonResult getRefSideContByInsurNumLegalEntity(string legalEntity, string insNum) // Find Reference Side And Contractors By Insurance Number
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                if (insNum.Length < 9)
                    insNum = Convert.ToInt32(insNum).ToString("000000000");

                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetRefSideContByInsurNumLegalEntity", legalEntity + "," + insNum);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///    Get List Of 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> Insurance Number. </param>
        /// <returns> Json. </returns>
        public JsonResult getRefSideContByInsurNumLegalEntityList(string legalEntity, string insNum) // Find Reference Side And Contractors By Insurance Number
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                if (insNum.Length < 9)
                    insNum = Convert.ToInt32(insNum).ToString("000000000");

                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetRefSideContByInsurNumLegalEntityList", legalEntity + "," + insNum);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get Process Data By Process Number.
        /// </summary>
        /// <param name="ID"> Process Number. </param>
        /// <returns> Json. </returns>
        public JsonResult getMainProcessByProcessNum(string ID) // Find Main Data Of Process By Process Number 
        {
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetMainProcessByProcessNum", ID);

                if (data == null)
                    return Json(data, JsonRequestBehavior.AllowGet);

                return Json(data.OModel, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get Main Process Data By Process Number , Worker code And Date Injury.
        /// </summary>
        /// <param name="processNum"> Process Number. </param>
        /// <param name="dateStartInjury"> Date Start Injury. </param>
        /// <param name="wc"> Worker Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getMainProcessByPNumAndwCodeAndDateInjury(string processNum, string dateStartInjury, string wc) // Find Main Data Of Process By Process Number And Worker Code And Date Injury 
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "getMainProcessByPNumAndwCodeAndDateInjury", wc + "," + processNum + "," + dateStartInjury);

                if (data == null)
                    return Json(data, JsonRequestBehavior.AllowGet);

                return Json(data.OModel, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get All Offices In Special Area.
        /// </summary>
        /// <param name="areaCode"> Area Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getOfficesByArea(string areaCode) // Get Offices By Area Code
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var data = db.GetOfficeInsurance(areaCode, null).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get All Offices In Special Area For Contractors.
        /// </summary>
        /// <param name="areaCode"> Area Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getOfficesByAreaContractor(int areaCode) // Get Offices By Area Code for Contractor
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var data = db.spGetOfficesContractor(areaCode).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get All Centers In Special Government.
        /// </summary>
        /// <param name="govCode"> Government Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getCenterByGovernment(string[] govCode) // Get Center By Government Code
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string govCodes = String.Join(",", govCode);
                var data = db.spGetCenterByGovernment(govCodes).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///   Get All Villages In Special Center.
        /// </summary>
        /// <param name="centerCode"> Center Code. </param>
        /// <returns> Json. </returns>
        public JsonResult getVillageByCenter(int? centerCode) // Get Village By Center Code
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string centerCodes = String.Join(",", centerCode);
                var data = db.spGetVillagebyCenter(centerCodes).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region General Methods

        /// <summary>
        ///   Get Data Of Drop Down Lists.
        /// </summary>
        /// <param name="govCode"> Government Code. </param>
        /// <param name="centerCode"> Center Code. </param>
        /// <param name="villagecode"> Village Code. </param>
        /// <param name="govLetterSiteCode"> Government Letter Site Code. </param>
        /// <param name="centerLetterSiteCode"> Center Letter site Code. </param>
        /// <param name="villageLetterSitecode"> Village Letter Site Code. </param>
        public void ViewBags(string govCode, string centerCode, string villagecode, string govLetterSiteCode, string centerLetterSiteCode, string villageLetterSitecode)
        {
            // المحافظه عنوان العمليه
            ViewBag.GovermentSite = new SelectList(db.spGetGoverments().ToList(), "govermentCode", "govermentIDName", govCode);
            // المركز - القسم عنوان العمليه
            ViewBag.CenterSite = new SelectList(db.spGetCenterByGovernment(govCode).ToList(), "centerCode", "centerName", centerCode);
            // الشياخه - القريه عنوان العمليه
            ViewBag.VallageSite = new SelectList(db.spGetVillagebyCenter(villagecode).ToList(), "villageCode", "villageName", villagecode);

            // نوع العمليه
            ViewBag.processType = new SelectList(db.spGetProcessTypes().ToList(), "processTypeCode", "processIDName");
            // نوع المستند
            ViewBag.documentType = new SelectList(db.documentTypes.ToList(), "documentTypeCode", "documentTypeName");
            // المحافظه
            ViewBag.GovermentLetterSite = new SelectList(db.spGetGoverments().ToList(), "govermentCode", "govermentIDName", govLetterSiteCode);
            // المركز - القسم
            ViewBag.CenterLetterSite = new SelectList(db.spGetCenterByGovernment(govLetterSiteCode).ToList(), "centerCode", "centerName", centerLetterSiteCode);
            // الشياخه - القريه
            ViewBag.VallageLetterSite = new SelectList(db.spGetVillagebyCenter(centerLetterSiteCode).ToList(), "villageCode", "villageName", villageLetterSitecode);
            // حاله العمليه
            ViewBag.processStatus = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="سارى", Value = "1" },
                new SelectListItem{ Text="منتهى", Value = "2"},
                new SelectListItem{ Text="موقوف", Value = "3"}}, "Value", "Text", 1);
            // مقدم الاخطار
            ViewBag.refContractor = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="مقاول رئيسي", Value = "1" },
                new SelectListItem{ Text="جهه اسناد", Value = "2"}}, "Value", "Text", 1);
            // مستخدمين العمليه
            ViewBag.procUser = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="تحتوى", Value = "0"},
                new SelectListItem{ Text="لا تحتوى", Value = "1" }}, "Value", "Text", 1);

            // الكيان القانونى
            ViewBag.legalEntity = new SelectList(new List<SelectListItem> {
                new SelectListItem{ Text="منشأه", Value = "2" },
                new SelectListItem{ Text="فرد", Value = "1" }}, "Value", "Text", -1);
        }

        /// <summary>
        ///   Get Data Of Drop Down Lists For Search.
        /// </summary>
        public void viewBagSearch()
        {
            ViewBags("-2", "-2", "-2", "-2", "-2", "-2");

            // العمال
            ViewBag.workers = new SelectList(db.workers.ToList(), "workerCode", "workerName");
            //مهن العاملين
            ViewBag.careerWorker = new SelectList(db.careers.ToList(), "careerCode", "careerName");
        }

        #endregion

        #region Notification الاشعارات

        /// <summary>
        ///   Get Notification For Current Page. 
        /// </summary>
        /// <param name="procCode"> Process Code. </param>
        /// <param name="processNum"> Process Number. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="screenName"> Screen Name. </param>
        /// <returns> View. </returns>
        public ActionResult vSearchNotify(int? procCode, string processNum, string areas, string Offices, string screenName)
        {
            try
            {
                bISSearch = true;

                var sOffice = Offices;
                var oArea = areas;
                var scnName = Request.Form["screenName"];
                string spParameters = "";
                List<string> searchProcess = new List<string>();

                // المقاول
                if (screenName == "../Contractor/vContractorIndex")
                {
                    #region search contractors
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم        
                    searchProcess.Add(String.IsNullOrEmpty(procCode.ToString()) ? "" : procCode.ToString()); // كود العمليه
                    searchProcess.Add(String.IsNullOrEmpty(processNum) ? "" : processNum); // رقم العمليه

                    #endregion
                }
                else
                {
                    #region search officer insurance

                    spParameters += (!String.IsNullOrEmpty(processNum) ? " AND (CONCAT(codes.area.areaID,codes.officeInsurance.officeInsuranceID,process.processYear,processNumber) LIKE '%" + processNum + "%')" : ""); // رقم العمليه
                    spParameters += ((procCode != null) ? " AND (process.processCode LIKE '" + procCode + "')" : ""); // كود العمليه

                    if (screenName != "vProcessRequest") //Process Office عمليات موظف التأمينات
                    {
                        spParameters += " AND (mainContractors.contractorType = 1) ";
                        spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                    }
                    else // Process Request طلبات العمليات
                    {
                        int uc = Convert.ToInt32(Session["uc"].ToString());
                        bool? isAdmin = db.users.FirstOrDefault(user => user.userCode == uc).isAdmin;
                        if (isAdmin == true)
                            spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                        else
                            spParameters += " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")"; //  بحث المناطق والمكاتب
                    }
                    #endregion
                }

                if (screenName == "../Contractor/vContractorIndex") // Contractors المقاولين
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchNotifyProcess", searchProcess);
                else if (screenName == "vProcessRequest") // Request Process طلبات لعمليات
                {
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(spParameters); // Stored Parameters
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchRequestProcess", searchProcess);
                }
                else if (screenName == "vProcessShowIndex") // Process Office عمليات موظف التأمينات  
                {
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(spParameters); // Stored Parameters
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchOfficeProcess", searchProcess);
                }

                if (screenName == "vProcessShowIndex") // بحث عن عمليه في العمليات موظف التأمينات
                    return RedirectToAction("vProcessShowIndex", "Process", new { areas = areas, inPage = 1, cp = 0, Offices = Offices, cd = 2 });

                return RedirectToAction(screenName, new { areas = areas, Offices = Offices, cd = 3 });
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region Rehab

        /// <summary>
        ///   Get 'Reference Sides - Contractors'.
        /// </summary>
        /// <param name="refSideContractor"> 'Reference Sides - Contractors' Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpRefSideContractor(int? refSideContractor)
        {
            try
            {
                TempData["refSideContractor"] = refSideContractor;

                if (refSideContractor != null)
                {
                    // جهه الاسناد
                    ViewBag.referanceSide = new SelectList(db.referenceSideContractors.Where(x => x.isActive != false).ToList(), "referenceSideContractorCode", "referenceSideContractorName");
                }
                else
                    ViewBag.referanceSide = new SelectList("", "");
                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Get Process Type.
        /// </summary>
        /// <param name="processType"> Process type Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpProcessType(int? processType) // Process Type نوع العملية
        {
            try
            {
                if (processType != null)
                {
                    // نوع العمليه
                    ViewBag.processType = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
                }
                else
                    ViewBag.processType = new SelectList("", "");

                return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Show Partial View Of Notes Insurences For Special Process.
        /// </summary>
        /// <param name="processID"> Process Code. </param>
        /// <param name="screen"> Screen Name. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpNotesInsurance(int? processID, string screen, string areas, string Offices)
        {
            try
            {
                Session["screen"] = null;
                TempData["screen"] = screen;
                if (!String.IsNullOrEmpty(screen))
                    Session["screen"] = screen;
                if (processID != null)
                {
                    ProcessId = (int)processID;
                    OprocessRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetProcessNotes", processID.ToString());
                }
                if (OprocessRequest.LprocessNotes == null)
                    OprocessRequest.LprocessNotes = new List<ProcessNotesModel>();
                var tuple = new Tuple<ProcessNotesModel, List<ProcessNotesModel>>(new ProcessNotesModel(), OprocessRequest.LprocessNotes);
                return PartialView(tuple);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Save Notes Of Process.
        /// </summary>
        /// <param name="formCollection"> Form Collection Of Area Code And Office Code. </param>
        /// <param name="model">  Data Of Model That Save In Database. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpSaveNotes(FormCollection formCollection, [Bind(Prefix = "Item1")] ProcessNotesModel model)
        {
            try
            {
                ProcessRequest NewObj = new ProcessRequest();
                NewObj.OprocessNotesModel = new DataAccessLayer.Models.ProcessNotesModel();
                NewObj.OprocessNotesModel.iProcessCode = ProcessId;
                NewObj.OprocessNotesModel.sNotes = model.sNotes;
                NewObj.OprocessNotesModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString()); ;

                //PostSaveNotes
                OprocessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveNotes", NewObj, null);
                if (OprocessRequest.OprocessNotesModel.bIsSaved)
                    TempData["msg"] = generalVariables.SaveDone;
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
                if (Session["screen"] != null)
                {
                    if (Session["screen"].ToString() == "processRequest")
                        return RedirectToAction("vProcessRequest", "Process", new { areas = formCollection["areas"].ToString(), cp = cp_, cd = cd_, Offices = formCollection["Offices"].ToString(), inPage = insPageNumber });
                    else if (Session["screen"].ToString() == "process")
                        return RedirectToAction("vProcessShowIndex", "Process", new { areas = formCollection["areas"].ToString(), cp = cp_, cd = cd_, Offices = formCollection["Offices"].ToString(), inPage = insPageNumber });
                }
                return RedirectToAction("vProcessRequest", "Process", new { areas = formCollection["areas"].ToString(), cp = cp_, cd = cd_, Offices = formCollection["Offices"].ToString(), inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Get Min - Max Of Number Of Workers.
        /// </summary>
        /// <param name="processID"> Process Code. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="notActive"> Check This Process Is Active Or Not. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpMinMax(int processID, string areas, string Offices, string notActive)
        {
            try
            {
                PC = processID;
                OprocessRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetMinMax", processID.ToString());

                if (notActive != "green" && !String.IsNullOrEmpty(notActive))
                    Session["procStopActive"] = 1;
                else
                    Session["procStopActive"] = null;

                return PartialView(OprocessRequest);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Save Min - Max OF Number Of Workers.
        /// </summary>
        /// <param name="OnewRequest"> Data Of Request Will Be Save. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpSaveMinMax(ProcessRequest OnewRequest, string areas, string Offices)
        {
            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                ProcessRequest model = new ProcessRequest();
                model.OModel = new DataAccessLayer.Models.ProcessModel();
                model.OModel.iProcessCode = PC;
                model.OModel.iMinNumber = OnewRequest.OModel.iMinNumber;
                model.OModel.iMaxNumber = OnewRequest.OModel.iMaxNumber;
                model.OModel.inUserUpdateCode = uc;

                OprocessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveMinMax", model, null);
                if (OprocessRequest.OModel.bIsEdit)
                    TempData["msg"] = generalVariables.EditDone;
                else
                    TempData["msg"] = generalVariables.EditNotDone;

                return RedirectToAction("vProcessShowIndex", "Process", new { areas = areas.ToString(), cp = cp_, cd = cd_, Offices = Offices.ToString(), inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region rehab process cancel

        /// <summary>
        ///   Exclusion Process.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessCancel(int? inPage, string areas, string Offices)
        {
            areas = (Session["AreaSearch"] == null ? areas : Session["AreaSearch"].ToString());
            Offices = (Session["SearchOffice"] == null ? Offices : Session["SearchOffice"].ToString());
            insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;
            return View();
        }

        #endregion
    }
}