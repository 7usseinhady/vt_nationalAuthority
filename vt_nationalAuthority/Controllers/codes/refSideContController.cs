using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.codes
{
    /// <summary>
    ///   Controller Of Reference Sides - Contractors.
    /// </summary>
    public class refSideContController : Controller
    {
        // GET: refSideCont
        ConnectionApi conApi = new ConnectionApi();
        static ReferenceSideContractorRequest oRefSideContReq = new ReferenceSideContractorRequest();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        static int? cp_, cd_;

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public refSideContController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Reference Sides - Contractors.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="IDDelete"> ID Of 'Reference Side - Contractor' Will Be Delete. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <returns> View. </returns>
        public ActionResult vRefSideCont(int? inPage, int? IDDelete, int? cp, int? cd)
        {
            insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

            cp_ = cp;
            cd_ = cd;

            if (IDDelete != null) // Delete
            {
                oRefSideContReq = conApi.connectionApiDelete<ReferenceSideContractorRequest>("apiReferenceSideContractor", "DeleteRefSideCont", IDDelete.ToString());
                TempData["msg"] = oRefSideContReq.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
            }
            else if (cp == 1) // Index
            {
                oRefSideContReq = conApi.connectionApiGetList<ReferenceSideContractorRequest>("apiReferenceSideContractor", "GetRefSideCont", null);
            }

            if (oRefSideContReq == null)
            {
                oRefSideContReq = new ReferenceSideContractorRequest();
                oRefSideContReq.LModels = new List<DataAccessLayer.Models.ReferenceSideContractorModel>();
            }

            if (oRefSideContReq.LModels == null)
                oRefSideContReq.LModels = new List<DataAccessLayer.Models.ReferenceSideContractorModel>();

            return View(oRefSideContReq.LModels.OrderBy(x => x.sReferanceSideContractorNum).ToPagedList(insPageNumber ?? 1, iPageSize));
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <returns> Partial View. </returns>
        public ActionResult _vpRefSideContSearch()
        {
            viewBags();
            return PartialView();
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="refSideContName">'Reference Side - Contractor' Name. </param>
        /// <param name="refSideContInsNum"> 'Reference Side - Contractor' Insurance Number. </param>
        /// <param name="legalEntity"> Legal Entity Of 'Reference Side - Contractor'. </param>
        /// <param name="activation"> Activation Of 'Reference Side - Contractor'. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpRefSideContSearch(string refSideContName, string refSideContInsNum, string legalEntity, string activation)
        {
            List<string> RefSideConts = new List<string>();
            RefSideConts.Add(String.IsNullOrEmpty(refSideContName) ? "-1" : refSideContName);
            RefSideConts.Add(String.IsNullOrEmpty(refSideContInsNum) ? "-1" : refSideContInsNum);
            RefSideConts.Add(String.IsNullOrEmpty(legalEntity) ? "-1" : legalEntity);
            RefSideConts.Add(String.IsNullOrEmpty(activation) ? "-1" : activation);

            oRefSideContReq = conApi.connectionApiSearchList<ReferenceSideContractorRequest>("apiReferenceSideContractor", "PostSearchRefSideCont", RefSideConts);

            return RedirectToAction("vRefSideCont", new { cp = 0 });
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel()
        {
            oRefSideContReq.LModels = null;
            TempData["msg"] = generalVariables.SearchCancel;

            return RedirectToAction("vRefSideCont", new { inPage = 1, cp = 1, cd = 83 });
        }


        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of ID.  
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="ID"> 'Reference Side - Contractor' Code For Get Object Data. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpRefSideContAddOrEdit(int? inPage, int? ID)
        {
            viewBags();

            ReferenceSideContractorRequest oRefSideCont = new ReferenceSideContractorRequest();

            // Edit 
            if (ID != null)
            {
                oRefSideCont = conApi.connectionApiGetList<ReferenceSideContractorRequest>("apiReferenceSideContractor", "GetRefSideCont", ID.ToString());
                return PartialView(oRefSideCont);
            }

            // Insert
            return PartialView(oRefSideCont);
        }

        /// <summary>
        ///  When User Clicked On : 
        ///         Add Button : Insert Data.
        ///        Edit Button : Edit Data.  
        /// </summary>
        /// <param name="oModalRequest"> Data Of Request That Save In Database. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpRefSideContAddOrEdit(ReferenceSideContractorRequest oModalRequest)
        {
            oModalRequest.OModel.sReferanceSideContractorNum = Convert.ToInt32(oModalRequest.OModel.sReferanceSideContractorNum).ToString("000000000");


            if (oModalRequest.OModel.iReferenceSideContractorCode > 0) // Edit
            {
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]); // كود موظف التعديل
                oRefSideContReq = conApi.connectionApiPost<ReferenceSideContractorRequest>("apiReferenceSideContractor", "PostEditRefSideCont", oModalRequest, oModalRequest.OModel.iReferenceSideContractorCode.ToString());
            }
            else // Save
            {
                oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"]); // كود موظف الادخال
                oRefSideContReq = conApi.connectionApiPost<ReferenceSideContractorRequest>("apiReferenceSideContractor", "PostSaveRefSideCont", oModalRequest, null);
            }

            TempData["msg"] = oRefSideContReq.OModel.dyError;
            return RedirectToAction("vRefSideCont", new { inPage = 1, cp = 1, cd = 83 });
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        void viewBags()
        {
            ViewBag.legalEntity = new SelectList(db.legalEntities.ToList(), "legalEntityCode", "legalEntityName");
            ViewBag.activation = new SelectList(new List<SelectListItem>
                                                 {
                                                     new SelectListItem { Text = "فعال", Value = "1"},
                                                     new SelectListItem { Text = "غير فعال", Value ="0"},
                                                 }, "Value", "Text");
        }
    }
}