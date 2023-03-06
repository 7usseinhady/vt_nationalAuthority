using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Workers
{
    public class InjuriesController : Controller
    {
        // GET: Injuries

        ConnectionApi conApi = new ConnectionApi();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ApiNationalAuthority.Models.GeneralMethods generalMethods = new ApiNationalAuthority.Models.GeneralMethods();
        static WorkerInjuriesRequest oWorkerInjuriesRequest = new WorkerInjuriesRequest();
        static bool bISSearch = false;
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public InjuriesController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Show The Injuries Of Worker
        /// </summary>
        /// <param name="wc">Worker Code</param>
        /// <param name="inPage">Current Page Number</param>
        /// <param name="IDDelete">Injuri Code Want To Delete</param>
        /// <param name="oModel">Data Of Injuries</param>
        /// <param name="reasonWorkerInjuries">Reason Worker Injuries</param>
        /// <param name="dateStartWorkerInjuries"> Date Start Worker Injuries</param>
        /// <param name="dateEndWorkerInjuries">Date End Worker Injuries</param>
        /// <param name="cd">Code Module</param>
        /// <returns>View Page Of List Worker Injuries</returns>
        public ActionResult vInjuriesIndex(int? wc, int? inPage, string IDDelete, WorkerInjuriesModel oModel, string reasonWorkerInjuries, DateTime? dateStartWorkerInjuries, DateTime? dateEndWorkerInjuries, int? cd)
        {
            try
            {
                insPageNumber = insPageNumber == null ? 1 : inPage;

                if (cd != null)
                {
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                }

                if (wc != null)
                    Session["workerCode"] = wc;

                if (!String.IsNullOrEmpty(IDDelete)) // Delete
                {
                    bISSearch = false;
                    oWorkerInjuriesRequest = conApi.connectionApiDelete<WorkerInjuriesRequest>("apiWorkerInjuries", "DeleteWorkerInjuries", IDDelete.ToString() + ',' + Session["workerCode"].ToString());

                    TempData["msg"] = oWorkerInjuriesRequest.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
                }

                else if (!bISSearch) // مفيش سيرش 
                    oWorkerInjuriesRequest = conApi.connectionApiGetList<WorkerInjuriesRequest>("apiWorkerInjuries", "GetWorkerInjuries", Session["workerCode"].ToString());
                else if (bISSearch)
                    oWorkerInjuriesRequest.oWorkerModel = conApi.connectionApiGetList<WorkerInjuriesRequest>("apiWorkerInjuries", "GetWorkerData", Session["workerCode"].ToString()).oWorkerModel;

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Get List Of Worker Injuries
        /// </summary>
        /// <returns>View Page</returns>
        public PartialViewResult _vpInjuriesIndex()
        {
            TempData["sWorkerName"] = oWorkerInjuriesRequest.oWorkerModel.sWorkerName;
            TempData["sWorkerNationalID"] = oWorkerInjuriesRequest.oWorkerModel.sWorkerNationalID;
            TempData["sWorkerInsuranceNum"] = oWorkerInjuriesRequest.oWorkerModel.sWorkerInsuranceNum;

            if (oWorkerInjuriesRequest == null)
            {
                oWorkerInjuriesRequest = new WorkerInjuriesRequest();
                oWorkerInjuriesRequest.LModels = new List<DataAccessLayer.Models.WorkerInjuriesModel>();
            }

            if (oWorkerInjuriesRequest.LModels == null)
                oWorkerInjuriesRequest.LModels = new List<DataAccessLayer.Models.WorkerInjuriesModel>();

            return PartialView("_vpInjuriesIndex", oWorkerInjuriesRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
        }
        /// <summary>
        /// Page For Save / Edit Worker Injuries
        /// </summary>
        /// <param name="inPage">Current Page Number</param>
        /// <param name="ID">Injuri Code</param>
        /// <param name="wc">Worker Code</param>
        /// <returns>View Page</returns>
        public PartialViewResult _vpAddOrEditInjuries(int? inPage, int? ID, int? wc)
        {
            try
            {
                insPageNumber = insPageNumber == null ? 1 : inPage;

                WorkerInjuriesRequest oWorkerInjuriesReq = new WorkerInjuriesRequest();
                if (oWorkerInjuriesReq.OModel == null)
                    oWorkerInjuriesReq.OModel = new DataAccessLayer.Models.WorkerInjuriesModel();

                // Add
                if (wc != null)
                    oWorkerInjuriesReq.OModel.iWorkerCode = Convert.ToInt32(Convert.ToInt32(Session["workerCode"].ToString()));

                // Edit 
                if (ID != null)
                    oWorkerInjuriesReq = conApi.connectionApiGetList<WorkerInjuriesRequest>("apiWorkerInjuries", "GetWorkerInjuriesByID", ID.ToString());

                return PartialView(oWorkerInjuriesReq);
            }
            catch (Exception ex)
            {
throw;
            }
        }
        /// <summary>
        /// Save / Edit Worker Injurie
        /// </summary>
        /// <param name="oModalRequest">Data For Injurie</param>
        /// <returns>View Page Of Worker Injuries</returns>
        [HttpPost]
        public ActionResult _vpAddOrEditInjuries(WorkerInjuriesRequest oModalRequest)
        {
            try
            {
                if (oModalRequest.OModel.iWorkerInjuriesCode > 0) // Edit
                {
                    string id = oModalRequest.OModel.iWorkerInjuriesCode.ToString();
                    oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]);
                    oWorkerInjuriesRequest = conApi.connectionApiPost<WorkerInjuriesRequest>("apiWorkerInjuries", "PostEditWorkerInjuries", oModalRequest, id);

                    TempData["msg"] = oWorkerInjuriesRequest.OModel.bIsEdit ? generalVariables.EditDone : generalVariables.EditNotDone;
                }
                else // Save
                {
                    oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"]);
                    oWorkerInjuriesRequest = conApi.connectionApiPost<WorkerInjuriesRequest>("apiWorkerInjuries", "PostSaveWorkerInjuries", oModalRequest, null);

                    TempData["msg"] = oWorkerInjuriesRequest.OModel.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone;
                }

                bISSearch = false;
                return RedirectToAction("vInjuriesIndex", new { wc = Convert.ToInt32(Session["workerCode"].ToString()), inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Page For Search Worker Injuries
        /// </summary>
        /// <param name="wc">Worker Code</param>
        /// <returns>View Page</returns>
        public PartialViewResult _vpSearchInjuries(int? wc)
        {
            WorkerInjuriesModel oModel = new WorkerInjuriesModel();
            oModel.iWorkerCode = Convert.ToInt32(Session["workerCode"].ToString());

            ViewBags();
            return PartialView(oModel);
        }
        /// <summary>
        /// Search For Worker Injuries
        /// </summary>
        /// <param name="oModel">Data Need For Search</param>
        /// <param name="processType">Process Types Codes</param>
        /// <returns>List Of Worker Injuries</returns>
        [HttpPost]
        public ActionResult _vpSearchInjuries(WorkerInjuriesModel oModel, string[] processType)
        {
            try
            {
                bISSearch = true;
                string sProcessType = generalMethods.sConcatString(processType, ','); // نوع العمليه

                List<string> lWorkerInjuries = new List<string>();
                lWorkerInjuries.Add(Session["workerCode"].ToString()); // كود العامل
                lWorkerInjuries.Add(String.IsNullOrEmpty(oModel.sProcessNum) ? "-1" : oModel.sProcessNum); // رقم العمليه   
                lWorkerInjuries.Add(String.IsNullOrEmpty(sProcessType) ? "-1" : sProcessType); // نوع العمليه
                lWorkerInjuries.Add(String.IsNullOrEmpty(oModel.sProcessSite) ? "-1" : oModel.sProcessSite); // عنوان العمليه
                lWorkerInjuries.Add(String.IsNullOrEmpty(oModel.sProcessMainContractor) ? "-1" : oModel.sProcessMainContractor); // المقاول الرئيسي للعمليه
                lWorkerInjuries.Add(String.IsNullOrEmpty(oModel.sInjuryReason) ? "-1" : oModel.sInjuryReason); // سبب الاصابه
                lWorkerInjuries.Add(oModel.sDateStartInjury == null ? null : oModel.sDateStartInjury.ToString()); // تاريخ بدء الاصابه
                lWorkerInjuries.Add(oModel.sDateEndInjury == null ? null : oModel.sDateEndInjury.ToString()); // تاريخ نهايه الاصابه

                oWorkerInjuriesRequest = conApi.connectionApiSearchList<WorkerInjuriesRequest>("apiWorkerInjuries", "PostSearchWorkerInjuries", lWorkerInjuries);
                return RedirectToAction("vInjuriesIndex", new { wc = Convert.ToInt32(Session["workerCode"].ToString()) });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Search Cancel
        /// </summary>
        /// <param name="wc">Worker Code</param>
        /// <returns>View Page Of Worker Injuries</returns>
        public ActionResult _vpSearchCancel(int? wc)
        {
            bISSearch = false;
            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction("vInjuriesIndex", new { wc = Convert.ToInt32(Session["workerCode"].ToString()) });
        }
        /// <summary>
        /// Get Reference Side Or Contractors
        /// </summary>
        /// <param name="ID"> Insurance Number</param>
        /// <returns>Data For Reference Side Or Contractors </returns>
        public JsonResult getProcessDataByInsurNum(string ID) 
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
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
        /// Get Data Need In DropDown List
        /// </summary>
        public void ViewBags()
        {
            // نوع العمليه
            ViewBag.processType = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
        }
    }
}