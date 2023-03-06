using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Requests;
using System;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Opposition
{
    public class OppositionController : Controller
    {
        // GET: Opposition
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        ConnectionApi conApi = new ConnectionApi();
        ApiNationalAuthority.Models.GeneralMethods generalMethods = new ApiNationalAuthority.Models.GeneralMethods();
        ProcessRequest oRequest = new ProcessRequest();
        static int ProcessCode;
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public OppositionController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Show Opposition / Exemption 
        /// </summary>
        /// <param name="ProcessId">Process Code</param>
        /// <param name="screen">Ccurrent Page</param>
        /// <param name="notActive">Status Of Process </param>
        /// <returns>View Page</returns>
        public ActionResult _vpOppositionIndex(int ProcessId, string screen, string notActive)
        {
            try
            {
                TempData["screen"] = (String.IsNullOrEmpty(screen) ? null : screen);
                ProcessCode = ProcessId;
                oRequest = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetAllOpposition", ProcessId.ToString());
                if (oRequest.oprocessOppositionModel != null)
                {
                    TempData["type"] = oRequest.oprocessOppositionModel.iOppositionTypeCode;
                    TempData["Reason"] = oRequest.oprocessOppositionModel.sProcessOppositionReason;
                    TempData["notes"] = oRequest.oprocessOppositionModel.sProcessOppositionNotes;
                }

                if (notActive != "green" && notActive != null)
                    Session["procStopActive"] = 1;
                else
                    Session["procStopActive"] = null;

                return PartialView(oRequest);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save Opposition / Exemption  
        /// </summary>
        /// <param name="formCollection">Data of Opposition / Exemption </param>
        /// <param name="model">Data Of Opposition / Exemption </param>
        /// <returns>View Page Of Process In Contractors</returns>
        [HttpPost]
        public ActionResult _vpSave(FormCollection formCollection, ProcessRequest model)
        {
            try
            {
                string action = formCollection["action"].ToString();
                ProcessRequest newObj = new ProcessRequest();
                newObj.oprocessOppositionModel = new ProcessOppositionModel();
                newObj.oprocessOppositionModel.iProcessCode = ProcessCode;
                int type = 0;
                type = (formCollection["Status"] == "Oppos" ? 1 : 2);
                newObj.oprocessOppositionModel.iOppositionTypeCode = type;
                newObj.oprocessOppositionModel.sProcessOppositionNotes = model.oprocessOppositionModel.sProcessOppositionNotes;
                newObj.oprocessOppositionModel.sProcessOppositionReason = model.oprocessOppositionModel.sProcessOppositionReason;
                newObj.oprocessOppositionModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                newObj.oprocessOppositionModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oRequest = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveOpposition", newObj, null);
                if (oRequest.oprocessOppositionModel.bIsSaved)
                    TempData["msg"] = generalVariables.SaveDone;
                else
                    TempData["msg"] = generalVariables.SaveNotDone;

                if (action == "vContractorIndex" || TempData["screen"] == null)
                    return RedirectToAction("vContractorIndex", "Contractor");
                else
                    return PartialView();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// View Reply Of Opposition / Exemption  
        /// </summary>
        /// <returns>View Reply</returns>
        public ActionResult _vpReplayInsurance()
        {
            return PartialView();
        }
    }
}