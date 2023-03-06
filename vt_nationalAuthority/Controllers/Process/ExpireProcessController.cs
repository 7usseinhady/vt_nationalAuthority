using DataAccessLayer;
using ApiNationalAuthority.Models;
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
    ///   Controller Of Expire Process.
    /// </summary>
    public class ExpireProcessController : Controller
    {
        // GET: ExpireProcess العمليات الموشكه على الانتهاء
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        GeneralMethods generalMethods = new GeneralMethods();
        static ProcessRequest oProcessRequest = new ProcessRequest();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static bool bISSearch = false;

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public ExpireProcessController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Expire Processes.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="s"> No Search. </param>
        /// <param name="cd"> Code Of Module Insurance Officer. </param>
        /// <param name="pNum"> Process Number. </param>
        /// <param name="pCode"> Process Code. </param>
        /// <param name="type"> Type Of Notification. </param>
        /// <returns> Veiw. </returns>
        public ActionResult vExpireProcess(int? inPage, int? cp, string areas, string Offices, int? s, int? cd,string pNum, int? pCode,int?type)
        {
            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                if (pCode != null)
                {
                    db.UpdateNotificationSeen(uc, pCode, type);
                    if (Session["CheckModule"] == null)
                        Session["CheckModule"] = db.CheckModuleUserPermisiom(uc, 1).ToList();
                }
                if (cd != null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();

                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                if (!String.IsNullOrEmpty(pNum) || pCode != null)
                {
                    bISSearch = true;
                    return RedirectToAction("vSearchNotify", "ExpireProcess", new { processNum = pNum, areas = areas, Offices = Offices, procCode = pCode, screenName = "vProcessShowIndex" });
                }
                else if (cp == 1 || s == 1) // s=1 جايه من البحث 
                {
                    bISSearch = false;
                    Session["processCode"] = null;
                }

                // مفيش سيرش 
                if (!bISSearch)
                {
                    List<string> searchProcess = new List<string>();
                    searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                    searchProcess.Add(!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                    oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchExpireProcess", searchProcess);
                }

                if (oProcessRequest == null)
                {
                    oProcessRequest = new ProcessRequest();
                    oProcessRequest.LModels = new List<DataAccessLayer.Models.ProcessModel>();
                }
                if (oProcessRequest.LModels == null)
                    oProcessRequest.LModels = new List<DataAccessLayer.Models.ProcessModel>();

                return View(oProcessRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpSearchProcess(string areas, string Offices)
        {
            ViewBags();

            TempData["areas"] = areas;
            TempData["Offices"] = Offices;

            return PartialView();
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="processNum"> Process Number. </param>
        /// <param name="DateStartProcess"> Date Start Process. </param>
        /// <param name="DateEndProcess"> Date End Process. </param>
        /// <param name="ProcessName"> Process Name. </param>
        /// <param name="processType"> List Of Process Types. </param>
        /// <param name="fullSiteAddress"> Full Site Address. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="procUser"> Process Users. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpSearchProcess(string processNum, DateTime? DateStartProcess, DateTime? DateEndProcess, string ProcessName, string[] processType, string fullSiteAddress, string areas, string Offices, string procUser)
        {
            try
            {
                bISSearch = true;

                string sProcessType = generalMethods.sConcatString(processType, ','); // نوع العمليه

                string spParameters = "";
                spParameters += (!String.IsNullOrEmpty(processNum) ? " AND (CONCAT(codes.area.areaID,codes.officeInsurance.officeInsuranceID,process.processYear,processNumber) LIKE '%" + processNum + "%')" : ""); // رقم العمليه
                spParameters += (!String.IsNullOrEmpty(ProcessName) ? " AND (process.processName LIKE '%" + ProcessName + "%')" : ""); // اسم العمليه
                spParameters += (!String.IsNullOrEmpty(sProcessType) ? " AND (process.processTypeCode IN (select * from [codes].[UTILfn_Split]('" + sProcessType + "', ',')))" : ""); // نوع العمليه
                spParameters += (!String.IsNullOrEmpty(fullSiteAddress) ? " AND(CONCAT(processSite.buildingNumber, '-', processSite.siteAddress, '-', goverment.govermentName, '-', center.centerName, '-', village.villageName) LIKE '%" + fullSiteAddress + "%')" : ""); // عنوان العمليه بالكامل
                spParameters += ((DateStartProcess != null) ? " AND CONVERT(DATE, process.dateStartProcess) LIKE CONVERT(DATE, '" + DateStartProcess.ToString() + "')" : ""); //  ت بدء العمليه
                spParameters += ((DateEndProcess != null) ? " AND CONVERT(DATE,process.dateEndProcess) LIKE CONVERT(DATE, '" + DateEndProcess.ToString() + "')" : ""); //  ت انتهاء العمليه
                spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب
                spParameters += (!String.IsNullOrEmpty(procUser) ? (procUser == "1" ? " AND (procUser = 0 OR procUser IS NULL)" : " AND procUser <> 0") : ""); // مستخدمين العمليه

                List<string> searchProcess = new List<string>();
                searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                searchProcess.Add(spParameters); // Stored Parameters

                oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchExpireProcess", searchProcess);
                return RedirectToAction("vExpireProcess", new { areas = areas, Offices = Offices });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Search In Notifications. 
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
                string spParameters = "";
                spParameters += (!String.IsNullOrEmpty(processNum) ? " AND (CONCAT(codes.area.areaID,codes.officeInsurance.officeInsuranceID,process.processYear,processNumber) LIKE '%" + processNum + "%')" : ""); // رقم العمليه
                spParameters += ((procCode != null) ? " AND (process.processCode LIKE '" + procCode + "')" : ""); // كود العمليه
                spParameters += (!String.IsNullOrEmpty(Offices) ? " AND (process.officeInsuranceCode IN (select * from [codes].[UTILfn_Split]('" + Offices + "',',')))" : (!String.IsNullOrEmpty(areas) ? " AND (process.officeInsuranceCode IN (SELECT * FROM [codes].[spGetPermissionAreaOffice]('" + areas + "'," + Session["officeCode"].ToString() + ")))" : " AND (process.officeInsuranceCode = " + Session["officeCode"].ToString() + ")")); //  بحث المناطق والمكاتب

                List<string> searchProcess = new List<string>();
                searchProcess.Add(Session["uc"].ToString()); // كود المستخدم
                searchProcess.Add(spParameters); // Stored Parameters

                oProcessRequest = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchExpireProcess", searchProcess);
                return RedirectToAction("../ExpireProcess/vExpireProcess", new { areas = areas, Offices = Offices, cd = 4 });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel(string areas, string Offices)
        {
            try
            {
                bISSearch = false;
                TempData["msg"] = generalVariables.SearchCancel;
                return RedirectToAction("vExpireProcess", new { areas = areas, Offices = Offices });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of ID.  
        /// </summary>
        /// <param name="inProcessCode"> Process Code. </param>
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpEditPeriodProcess(int? inProcessCode, string areas, string Offices)
        {
            try
            {
                oProcessRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetMainProcessData", inProcessCode.ToString());
                oProcessRequest.OModel.sDateEndProcessRequired = oProcessRequest.OModel.sDateEndProcess;
                TempData["areas"] = areas;
                TempData["Offices"] = Offices;

                return PartialView(oProcessRequest);
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
        /// <param name="areas"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="Offices"> Codes Of Offices That Selected In The Search. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpEditPeriodProcess(ProcessRequest oModalRequest, string areas, string Offices)
        {
            try
            {
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oModalRequest.OModel.sDateEndProcess = oModalRequest.OModel.sDateEndProcessRequired;
                oProcessRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostChangeEndDateProcess", oModalRequest, null);

                if (oProcessRequest.OModel.bIsEdit)
                    TempData["msg"] = generalVariables.EditDone;
                else
                    TempData["msg"] = generalVariables.EditNotDone;

                return RedirectToAction("vExpireProcess", new { areas = areas, Offices = Offices });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        public void ViewBags()
        {
            // نوع العمليه
            ViewBag.processType = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
            // مستخدمين العمليه
            ViewBag.procUser = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="تحتوى", Value = "0" },
                new SelectListItem{ Text="لا تحتوى", Value = "1"}}, "Value", "Text", 1);
        }
    }
}