using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Views.codes
{
    /// <summary>
    ///   Controller Of Attachment Type.
    /// </summary>
    public class attachmentTypeController : Controller
    {
        ConnectionApi conApi = new ConnectionApi();
        static AttachmentTypeRequest oAttachmentTypeRequest = new AttachmentTypeRequest();
        GeneralMethods generalMethods = new GeneralMethods();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public attachmentTypeController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Get All Attachment Types.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="IDDelete"> ID Of Attachment Type Will Be Delete. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <returns> View. </returns>
        public ActionResult vAttachmentType(int? inPage, int? IDDelete, bool? cp)
        {
            insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

            if (IDDelete != null) // Delete
            {
                oAttachmentTypeRequest = conApi.connectionApiDelete<AttachmentTypeRequest>("apiAttachmentType", "DeleteAttachmentTypes", IDDelete.ToString());
                TempData["msg"] = oAttachmentTypeRequest.OModel.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone;
            }
            else if (oAttachmentTypeRequest.LModels == null || (cp == true)) // Index
                oAttachmentTypeRequest = conApi.connectionApiGetList<AttachmentTypeRequest>("apiAttachmentType", "GetAttachmentTypes", null);

            if (oAttachmentTypeRequest == null)
            {
                oAttachmentTypeRequest = new AttachmentTypeRequest();
                oAttachmentTypeRequest.LModels = new List<DataAccessLayer.Models.AttachmentTypeModel>();
            }

            if (oAttachmentTypeRequest.LModels == null)
                oAttachmentTypeRequest.LModels = new List<DataAccessLayer.Models.AttachmentTypeModel>();

            return View(oAttachmentTypeRequest.LModels.OrderBy(x => x.sAttachmentTypeName).ToPagedList(insPageNumber ?? 1, iPageSize));
        }


        /// <summary>
        ///   Show Partial View Of Search. 
        /// </summary>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentTypeSearch()
        {
            viewBags();
            return PartialView();
        }

        /// <summary>
        ///   Search With Special Parameters. 
        /// </summary>
        /// <param name="attachmentTypeSearch"> Attachment Type Name. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpAttachmentTypeSearch(string attachmentTypeSearch)
        {
            if (attachmentTypeSearch != null)
            {
                List<string> attachmentTypes = new List<string>();
                attachmentTypes.Add(attachmentTypeSearch);

                oAttachmentTypeRequest = conApi.connectionApiSearchList<AttachmentTypeRequest>("apiAttachmentType", "PostSearchAttachmentTypes", attachmentTypes);
            }
            return RedirectToAction("vAttachmentType");
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel()
        {
            oAttachmentTypeRequest.LModels = null;
            TempData["msg"] = generalVariables.SearchCancel;

            return RedirectToAction("vAttachmentType");
        }


        /// <summary>
        ///   Show Partial View For User When Clicked On : 
        ///         Add Button : Make Insert His Data.
        ///        Edit Button : Get Object Data Of ID.  
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="ID"> Attachment Type Code For Get Object Data </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentTypeAddOrEdit(int? inPage, int? ID)
        {
            viewBags();

            AttachmentTypeRequest archiveType = new AttachmentTypeRequest();

            // Edit 
            if (ID != null)
            {
                archiveType = conApi.connectionApiGetList<AttachmentTypeRequest>("apiAttachmentType", "GetAttachmentTypes", ID.ToString());
                return PartialView(archiveType);
            }

            // Insert
            return PartialView(archiveType);
        }

        /// <summary>
        ///  When User Clicked On : 
        ///         Add Button : Insert Data.
        ///        Edit Button : Edit Data.  
        /// </summary>
        /// <param name="oModalRequest"> Data Of Request That Save In Database. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpAttachmentTypeAddOrEdit(AttachmentTypeRequest oModalRequest)
        {
            if (oModalRequest.OModel.iAttachmentTypeCode > 0) // Edit
            {
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"]); // كود موظف التعديل
                oAttachmentTypeRequest = conApi.connectionApiPost<AttachmentTypeRequest>("apiAttachmentType", "PostEditAttachmentTypes", oModalRequest, oModalRequest.OModel.iAttachmentTypeCode.ToString());
                TempData["msg"] = oAttachmentTypeRequest.OModel.bIsEdit ? generalVariables.EditDone : generalVariables.EditNotDone;
            }
            else // Save
            {
                oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"]); // كود موظف الادخال
                oAttachmentTypeRequest = conApi.connectionApiPost<AttachmentTypeRequest>("apiAttachmentType", "PostSaveAttachmentTypes", oModalRequest, null);
                TempData["msg"] = oAttachmentTypeRequest.OModel.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone;
            }

            return RedirectToAction("vAttachmentType");
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        void viewBags()
        {
            ViewBag.attachmentType = new SelectList(db.attachmentTypes.ToList(), "attachmentTypeCode", "attachmentTypeName");
        }

    }
}