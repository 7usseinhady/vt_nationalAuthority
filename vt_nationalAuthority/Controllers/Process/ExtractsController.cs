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
    ///   Controller Of Extracts.
    /// </summary>
    public class ExtractsController : Controller
    {
        // GET: Extracts المستخلصات
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        static ExtractsRequest oExtractsRequest = new ExtractsRequest();
        static bool bISSearch = false;
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);


        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public ExtractsController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Extracts.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="inPagging"> Number Of Page. </param>
        /// <param name="IDDelete"> ID Of Extract Will Be Delete. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <returns> View. </returns>
        public ActionResult vExtractsShow(int? pc, int? inPage, int? inPagging, string IDDelete, bool? cp)
        {
            try
            {
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                // Choose Page From nav bar
                if (cp == true)
                    bISSearch = false;

                TempData["Layout"] = Session["officeCode"] == null ? "~/Views/Shared/_contractorLayout.cshtml" :  // المقاول
                                                                     "~/Views/Shared/ProcessLayout.cshtml"; // موظف التأمينات

                // Delete
                if (!String.IsNullOrEmpty(IDDelete))
                {
                    bISSearch = false;

                    oExtractsRequest = Session["officeCode"] == null ? conApi.connectionApiDelete<ExtractsRequest>("apiExtracts", "DeleteExtracts", IDDelete.ToString() + ',' + Session["processCode"].ToString() + ',' + Session["uc"].ToString() + ",true") : // المقاول
                                                   conApi.connectionApiDelete<ExtractsRequest>("apiExtracts", "DeleteExtracts", IDDelete.ToString() + ',' + Session["processCode"].ToString() + ',' + Session["uc"].ToString()); // موظف التأمينات

                    TempData["msg"] = oExtractsRequest.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
                }
                else if (!bISSearch) // مفيش سيرش 
                {
                    oExtractsRequest = Session["officeCode"] == null ? conApi.connectionApiGetList<ExtractsRequest>("apiExtracts", "GetExtracts", Session["processCode"].ToString() + ',' + Session["uc"].ToString()) : // المقاول
                                                                       conApi.connectionApiGetList<ExtractsRequest>("apiExtracts", "GetExtracts", Session["processCode"].ToString()); // موظف التأمينات
                }

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        ///   Partial View Of Extracts.
        /// </summary>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <param name="cd4"> Code Of Module Insurance Officer. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpExtractsIndex(bool? cp, int? cd4)
        {
            try
            {
                // المقاول
                if (cd4 != null && Session["officeCode"] != null)
                {
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    TempData["checkLevel4"] = db.CheckModuleUserPermisiom(uc, cd4).ToList();
                }

                if (oExtractsRequest == null)
                {
                    oExtractsRequest = new ExtractsRequest();
                    oExtractsRequest.LModels = new List<DataAccessLayer.Models.ExtractsModel>();
                }
                if (oExtractsRequest.LModels == null)
                    oExtractsRequest.LModels = new List<DataAccessLayer.Models.ExtractsModel>();

                return PartialView(oExtractsRequest.LModels.OrderByDescending(x => x.sDateExtract).ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public PartialViewResult _vpExtractsSearch(int? pc)
        {
            viewBags(Convert.ToInt32(Session["processCode"].ToString()), true);
            TempData["pc"] = Convert.ToInt32(Session["processCode"].ToString());
            return PartialView();
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="office"> Codes Of Offices That Selected In The Search. </param>
        /// <param name="area"> Codes Of Areas That Selected In The Search. </param>
        /// <param name="extractName"> Extract Name. </param>
        /// <param name="recieverName"> Reciever Name. </param>
        /// <param name="budget"> Budget. </param>
        /// <param name="extractTypes"> List Of Extracts Types. </param>
        /// <param name="processTypes"> List Of Processes Types. </param>
        /// <param name="fromDate"> Date From. </param>
        /// <param name="toDate"> Date To. </param>
        /// <param name="processSubContractorCode"> List Of Process Sub Contractors Codes. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpExtractsSearch(int? pc, string[] office, string[] area, string extractName, string recieverName, string budget, string[] extractTypes, string[] processTypes,
            DateTime? fromDate, DateTime? toDate, string[] processSubContractorCode)
        {
            try
            {
                bISSearch = true;

                List<string> extractProcess = new List<string>();
                extractProcess.Add("-1"); // كود المستخلص
                extractProcess.Add(Session["processCode"].ToString()); // كود العمليه

                // اسم المكتب
                if (office == null)
                    extractProcess.Add("-1");
                else
                    extractProcess.Add(String.Join(",", office));

                // اسم المنطقه
                if (area == null)
                    extractProcess.Add("-1");
                else
                    extractProcess.Add(String.Join(",", area));

                extractProcess.Add(String.IsNullOrEmpty(extractName) ? "" : extractName); // اسم المستخلص
                extractProcess.Add(String.IsNullOrEmpty(recieverName) ? "" : recieverName); // اسم المندوب
                extractProcess.Add(String.IsNullOrEmpty(budget) ? "-1" : budget); // قيمه المستخلص

                // نوع المستخلص
                if (extractTypes == null)
                    extractProcess.Add("-1");
                else
                    extractProcess.Add(String.Join(",", extractTypes));

                // المهمه المنتهيه
                if (processTypes == null)
                    extractProcess.Add("-1");
                else
                    extractProcess.Add(String.Join(",", processTypes));

                // تاريخ المستخلص
                // Dates التواريخ
                if (fromDate == null && toDate == null)
                {
                    extractProcess.Add(null);
                    extractProcess.Add("");
                }
                else if (fromDate != null && toDate != null)
                {
                    extractProcess.Add(fromDate.ToString());
                    extractProcess.Add(toDate.ToString());
                }
                else if (fromDate != null)
                {
                    extractProcess.Add(fromDate.ToString());
                    extractProcess.Add("");
                }
                else if (toDate != null)
                {
                    extractProcess.Add(toDate.ToString());
                    extractProcess.Add("");
                }

                // المقاولين
                // المقاول
                if (Session["officeCode"] == null)
                    extractProcess.Add("cont" + Session["uc"].ToString());
                else // موظف التأمينات
                {
                    if (processSubContractorCode == null)
                        extractProcess.Add("-1");
                    else
                        extractProcess.Add(String.Join(",", processSubContractorCode));
                }

                oExtractsRequest = conApi.connectionApiSearchList<ExtractsRequest>("apiExtracts", "PostSearchExtracts", extractProcess);
                return RedirectToAction("vExtractsShow", new { pc = Convert.ToInt32(Session["processCode"].ToString()) });
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
            bISSearch = false;
            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction("vExtractsShow", new { pc = Convert.ToInt32(Session["processCode"].ToString()) });
        }


        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of ID.         
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="ID"> Extract Code. </param>
        /// <param name="pc"> Process Code. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpExtractsAddOrEdit(int? inPage, int? ID, int? pc)
        {
            try
            {
                viewBags(Convert.ToInt32(Session["processCode"].ToString()), false);
                insPageNumber = inPage == null ? 1 : inPage;

                ExtractsRequest oExtractReq = new ExtractsRequest();
                if (oExtractReq.OModel == null)
                    oExtractReq.OModel = new DataAccessLayer.Models.ExtractsModel();

                oExtractReq.OModel.iProcessCode = Convert.ToInt32(Session["processCode"].ToString());

                // Edit 
                if (ID != null)
                    oExtractReq = conApi.connectionApiGetList<ExtractsRequest>("apiExtracts", "GetExtractsByID", ID.ToString());

                if (Session["officeCode"] == null)// المقاول
                {
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    oExtractReq.OModel.sProcessSubContractorName = db.referenceSideContractors.FirstOrDefault(y => y.referenceSideContractorCode == (db.users.FirstOrDefault(x => x.userCode == uc).contractorCode)).referenceSideContractorName;
                }

                return PartialView(oExtractReq);
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
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpExtractsAddOrEdit(ExtractsRequest oModalRequest)
        {
            try
            {
                if (oModalRequest.OModel.iExtractProcessCode > 0) // Edit
                {
                    string id = oModalRequest.OModel.iExtractProcessCode.ToString();
                    oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]);
                    oModalRequest.OModel.sIpInsert = Session["officeCode"] == null ? "contractor" : // المقاول
                                                                                     "officeEmployee"; // موظف التأمينات

                    oExtractsRequest = conApi.connectionApiPost<ExtractsRequest>("apiExtracts", "PostEditExtracts", oModalRequest, id);

                    TempData["msg"] = oExtractsRequest.OModel.bIsEdit ? generalVariables.EditDone : generalVariables.EditNotDone;
                }
                else // Save
                {
                    oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"]);
                    oModalRequest.OModel.sIpUpdate = Session["officeCode"] == null ? "contractor" : // المقاول
                                                                  "officeEmployee"; // موظف التأمينات

                    oExtractsRequest = conApi.connectionApiPost<ExtractsRequest>("apiExtracts", "PostSaveExtracts", oModalRequest, null);
                    TempData["msg"] = oExtractsRequest.OModel.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone;
                }
                bISSearch = false;
                return RedirectToAction("vExtractsShow", new { pc = oModalRequest.OModel.iProcessCode, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Accept Extract.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="extractCode"> Extract Code. </param>
        /// <returns> View. </returns>
        public ActionResult _vpAcceptExtract(int? inPage, int? extractCode)
        {
            try
            {
                insPageNumber = inPage == null ? 1 : inPage;

                ExtractsRequest oModalRequest = new ExtractsRequest();
                oModalRequest.OModel = new ExtractsModel();

                oModalRequest.OModel.iExtractProcessCode = Convert.ToInt32(extractCode.ToString());
                oModalRequest.OModel.iProcessCode = Convert.ToInt32(Session["processCode"].ToString());
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]);

                oExtractsRequest = conApi.connectionApiPost<ExtractsRequest>("apiExtracts", "PostAcceptExtract", oModalRequest, "");

                TempData["msg"] = oExtractsRequest.OModel.bIsSaved ? generalVariables.ExtractAccept : generalVariables.ExtractNotAccept;
                return RedirectToAction("vExtractsShow");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        ///   Confirm OF Payment On Extract. 
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="extractCode"> Extract Code. </param>
        /// <returns> View. </returns>
        public ActionResult _vpPaidExtract(int? inPage, int? extractCode)
        {
            insPageNumber = inPage == null ? 1 : inPage;

            ExtractsRequest oModalRequest = new ExtractsRequest();
            oModalRequest.OModel = new ExtractsModel();

            oModalRequest.OModel.iExtractProcessCode = Convert.ToInt32(extractCode.ToString());
            oModalRequest.OModel.iProcessCode = Convert.ToInt32(Session["processCode"].ToString());
            oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]);

            oExtractsRequest = conApi.connectionApiPost<ExtractsRequest>("apiExtracts", "PostPaidExtract", oModalRequest, "");

            TempData["msg"] = oExtractsRequest.OModel.bIsEdit ? generalVariables.ExtractPaid : generalVariables.ExtractNotPaid;
            return RedirectToAction("vExtractsShow");
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        /// <param name="pc"> Process Code. </param>
        /// <param name="searchOrAdd"> Check This View For Search Or Add Extract. </param>
        void viewBags(int? pc, bool searchOrAdd)
        {
            // نوع المستخلص
            ViewBag.extractTypes = new SelectList(db.extractTypes.ToList(), "extractTypeCode", "extractTypeName");

            // المهمه المنتهيه
            //if (searchOrAdd)// search
            //{
            int uc = Convert.ToInt32(Session["uc"].ToString());
            ViewBag.processTypes = new SelectList(db.spGetMissionOfExtracts(pc, uc, 0).ToList(), "processTypeCode", "processTypeName");

            //if (Session["officeCode"] == null) // جهه اسناد - مقاول
            //{

            //}
            //else
            //    ViewBag.processTypes = new SelectList(db.spGetMissionOfExtracts(pc, uc, 0).ToList(), "processTypeCode", "processTypeName");
            //}

            //else // add
            //{
            //if (Session["officeCode"] == null) // جهه اسناد - مقاول
            //{
            //int uc = Convert.ToInt32(Session["uc"].ToString());
            //ViewBag.processTypes = new SelectList(db.spGetMissionOfExtracts(pc, uc, 0).ToList(), "processTypeCode", "processTypeName");
            //}
            //else
            //    ViewBag.processTypes = new SelectList(db.processTypes.Where(procType => procType.processTypeCode == -1).ToList(), "processTypeCode", "processTypeName");// new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
            ////}

            // المنطقه
            ViewBag.area = new SelectList(db.areas.ToList(), "areaCode", "areaName");
            // المقاولين بالعمليه
            ViewBag.contractors = new SelectList(db.spGetContractorsInProcess(pc, "-1").ToList(), "processSubContractorCode", "referenceSideContractorName");
        }


        /// <summary>
        ///   Get Mission Of Extract.
        /// </summary>
        /// <param name="contCode"> Contractor Code. </param>
        /// <returns> Json. </returns>
        public JsonResult MissionOfExtracts(int? contCode)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                int pc = Convert.ToInt32(Session["processCode"].ToString());

                var data = db.spGetMissionOfExtracts(pc, 0, contCode).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}