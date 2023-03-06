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
    ///   Controller Of Process Stop.
    /// </summary>
    public class processStopController : Controller
    {
        // GET: processStop ايقاف العمليه
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        static ProcessStopRequest oProcessStopRequest = new ProcessStopRequest();
        static bool bISSearch = false;
        static int? insPageNumber;
        static string sScreenName;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public processStopController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Reasons Of Stopping Process. 
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="inPagging"> Number Of Page. </param>
        /// <param name="IDDelete"> ID Of Reason Of Stopping Process Will Be Delete. </param>
        /// <param name="oModel"> Object Of Reason Of Stopping Process. </param>
        /// <param name="reasonProcessStop"> Reason For Stopping Process. </param>
        /// <param name="dateStartProcessStop"> Date Start For Stopping Process. </param>
        /// <param name="dateEndProcessStop"> Date End For Stopping Process. </param>
        /// <param name="cd"> Code Of Module Insurance Officer. </param>
        /// <param name="sn"> Screen Name. </param>
        /// <returns> View. </returns>
        public ActionResult vProcessStop(int? pc, int? inPage, int? inPagging, string IDDelete, ProcessStopModel oModel, string reasonProcessStop, DateTime? dateStartProcessStop, DateTime? dateEndProcessStop, int? cd, int? sn)
        {
            try
            {
                if (!String.IsNullOrEmpty(sn.ToString()))
                    sScreenName = sn.ToString();

                int uc = Convert.ToInt32(Session["uc"].ToString());
                if (cd != null)
                    TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();

                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                if (pc != null)
                    Session["processCode"] = pc;

                if (!String.IsNullOrEmpty(IDDelete)) // Delete
                {
                    bISSearch = false;
                    oProcessStopRequest = conApi.connectionApiDelete<ProcessStopRequest>("apiProcessStop", "DeleteProcessStop", IDDelete.ToString() + ',' + Session["processCode"].ToString());

                    TempData["msg"] = oProcessStopRequest.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
                }
                else if (!bISSearch) // مفيش سيرش 
                    oProcessStopRequest = conApi.connectionApiGetList<ProcessStopRequest>("apiProcessStop", "GetProcessStop", Session["processCode"].ToString());
                else if (bISSearch)
                    oProcessStopRequest.OModel = conApi.connectionApiGetList<ProcessStopRequest>("apiProcessStop", "GetProcessData", Session["processCode"].ToString()).OModel;

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Partial View Of Reasons Of Stopping Process.
        /// </summary>
        /// <returns> Partial View. </returns>
        public PartialViewResult _vpProcessStop()
        {
            try
            {
                TempData["sProcessName"] = oProcessStopRequest.oProcessModel.sProcessName;
                TempData["sAreaID"] = oProcessStopRequest.oProcessModel.sAreaIDName;
                TempData["sOfficeID"] = oProcessStopRequest.oProcessModel.sOfficeIDName;
                TempData["sProcessNumber"] = oProcessStopRequest.oProcessModel.sProcessNumber;
                TempData["sDateStart"] = oProcessStopRequest.oProcessModel.sDateStartProcess;
                TempData["sDateEnd"] = oProcessStopRequest.oProcessModel.sDateEndProcess;

                if (oProcessStopRequest == null)
                {
                    oProcessStopRequest = new ProcessStopRequest();
                    oProcessStopRequest.LModels = new List<DataAccessLayer.Models.ProcessStopModel>();
                }
                if (oProcessStopRequest.LModels == null)
                    oProcessStopRequest.LModels = new List<DataAccessLayer.Models.ProcessStopModel>();

                TempData["sScreenName"] = !String.IsNullOrEmpty(sScreenName) ? (sScreenName.ToString() == "1" ? "العمليات" : "العمليات الموشكه على الانتهاء") : "العمليات";

                return PartialView(oProcessStopRequest.LModels.OrderByDescending(X => X.sDateStartProcessStop).ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch (Exception ex)
            {
throw;
            }
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public PartialViewResult _vpProcessStopSearch(int? pc)
        {
            try
            {
                ProcessStopModel oModel = new ProcessStopModel();
                var pc_ = pc == null ? "0" : pc.ToString();

                oModel.iProcessCode = Convert.ToInt32(pc_);
                return PartialView(oModel);
            }

            catch (Exception ex)
            {
throw;
            }
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="oModel"> Fields Of Search. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpProcessStopSearch(int? pc, ProcessStopModel oModel)
        {
            try
            {
                bISSearch = true;

                List<string> processStop = new List<string>();
                processStop.Add(oModel.iProcessCode.ToString()); // كود العمليه
                processStop.Add(String.IsNullOrEmpty(oModel.sProcessStopReason) ? "" : oModel.sProcessStopReason); // سبب الايقاف
                processStop.Add(oModel.sDateStartProcessStop == null ? null : oModel.sDateStartProcessStop.ToString()); // تاريخ بدء ايقاف العمليه
                processStop.Add(oModel.sDateEndProcessStop == null ? null : oModel.sDateEndProcessStop.ToString()); // تاريخ نهايه بدء العمليه

                oProcessStopRequest = conApi.connectionApiSearchList<ProcessStopRequest>("apiProcessStop", "PostSearchProcessStop", processStop);
                return RedirectToAction("vProcessStop", new { pc = oModel.iProcessCode });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel(int? pc)
        {
            try
            {
                bISSearch = false;
                TempData["msg"] = generalVariables.SearchCancel;
                return RedirectToAction("vProcessStop", new { pc });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
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

            // العمليات الموشكه على لانتهاء
            return RedirectToAction("../ExpireProcess/vExpireProcess", new { cp = 1, cd = 4 });
        }


        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of ID.  
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="ID"> Reason Of Stopping Process Code For Get Object Data. </param>
        /// <param name="pc"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public PartialViewResult _vpProcessStopAddOrEdit(int? inPage, int? ID, int? pc)
        {
            try
            {
                insPageNumber = inPage == null ? 1 : inPage;
                ProcessStopRequest oProcessStopReq = new ProcessStopRequest();
                if (oProcessStopReq.OModel == null)
                    oProcessStopReq.OModel = new DataAccessLayer.Models.ProcessStopModel();

                if (pc != null)
                    oProcessStopReq.OModel.iProcessCode = Convert.ToInt32(pc.ToString());

                oProcessStopReq = ID != null ? conApi.connectionApiGetList<ProcessStopRequest>("apiProcessStop", "GetProcessStopByID", ID.ToString()) :  // Edit 
                                               conApi.connectionApiGetList<ProcessStopRequest>("apiProcessStop", "GetProcessStopPrevByID", Session["processCode"].ToString()); // insert

                return PartialView(oProcessStopReq);
            }

            catch (Exception ex)
            {
throw;
            }
        }

        /// <summary>
        ///  When User Clicked On : 
        ///         Add Button : Insert Data.
        ///        Edit Button : Edit Data.  
        /// </summary>
        /// <param name="oModalRequest"> Data Of Request That Save In Database. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpProcessStopAddOrEdit(ProcessStopRequest oModalRequest)
        {
            try
            {
                if (oModalRequest.OModel.iProcessStopCode > 0) // Edit
                {
                    string id = oModalRequest.OModel.iProcessStopCode.ToString();
                    oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]);
                    oProcessStopRequest = conApi.connectionApiPost<ProcessStopRequest>("apiProcessStop", "PostEditProcessStop", oModalRequest, id);

                    TempData["msg"] = oProcessStopRequest.OModel.bIsEdit ? generalVariables.EditDone : generalVariables.WorkerAttendanceFound;
                }
                else // Save
                {
                    oModalRequest.OModel.iProcessCode = Convert.ToInt32(Session["processCode"].ToString());
                    oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"]);
                    oProcessStopRequest = conApi.connectionApiPost<ProcessStopRequest>("apiProcessStop", "PostSaveProcessStop", oModalRequest, null);

                    TempData["msg"] = oProcessStopRequest.OModel.bIsSaved ? generalVariables.SaveDone : generalVariables.WorkerAttendanceFound;
                }

                bISSearch = false;
                return RedirectToAction("vProcessStop", new { pc = oModalRequest.OModel.iProcessCode, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>

    }
}