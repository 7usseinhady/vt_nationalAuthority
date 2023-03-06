using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.App_Code;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Insurance_Employee
{
    public class CardsRequestsController : Controller
    {
        nat_wsdl nat = new nat_wsdl();
        cover_wsdl cover = new cover_wsdl();
        ConnectionApi conApi = new ConnectionApi();
        GeneralMethods generalMethods = new GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        WorkerRequest oWorkerRequest = new WorkerRequest();
        CardsRequestsRequest oCardsRequestsRequest = new CardsRequestsRequest();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static WorkerRequest model;
        static int? WorkerID;
        static string Area, Office, search;
        static CardsRequestsRequest SModel;
        static int cd_;

        // GET: CardsRequests
        /// <summary>
        ///  Constructor Function , going to Login Page when session for User End
        /// </summary>
        public CardsRequestsController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        #region CardsRequests
        /// <summary>
        /// Card Requests page
        /// </summary>
        /// <param name="areas">codes of areas for search</param>
        /// <param name="Offices">codes of offices for search</param>
        /// <returns>view page</returns>
        public ActionResult vCardsRequestsIndex(string areas, string Offices)
        {

            if (SModel == null && search == null)
                SModel = new CardsRequestsRequest();
            return View();
        }
        /// <summary>
        /// show and Search for card request
        /// </summary>
        /// <param name="areas">codes of areas for search</param>
        /// <param name="Offices">codes of offices for  search</param>
        /// <param name="inPage">page number</param>
        /// <param name="formCollection">Data need for search</param>
        /// <param name="status">status for card request</param>
        /// <param name="cd">code for module Insurance Authority</param>
        /// <param name="IDDelete">Card Code Want To Delete</param>
        /// <returns>list of Card Requests</returns>
        public ActionResult _vpCardsRequests(string areas, string Offices, int? inPage, FormCollection formCollection, string[] status, int? cd, int? IDDelete)
        {
            try
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                if (Area == null || !String.IsNullOrEmpty(areas))
                    Area = areas;
                if (Office == null || !String.IsNullOrEmpty(Offices))
                    Office = Offices;
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;
                List<string> lstrSearch = new List<string>();
                // index
                if (String.IsNullOrEmpty(Area) && String.IsNullOrEmpty(Office) && formCollection.Count == 0)
                {
                    // هنا لما بكون عامله سرش بيرجع تانى ع الفانكشن دى ف يرجع بالداتا الى معمول عليها سرش
                    if (!String.IsNullOrEmpty(search))
                    {
                        oCardsRequestsRequest.LModels = SModel.LModels;
                        return PartialView(oCardsRequestsRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
                    }
                    else
                    {
                        // Check if user ia admin 
                        var userDetails = db.users.Where(x => x.userCode == uc).FirstOrDefault();
                        if (userDetails.isAdmin == true)
                            oCardsRequestsRequest = conApi.connectionApiGetList<CardsRequestsRequest>("apiCardsRequests", "GetAllCardReq", null);
                        else
                        {
                            string off = (Office == null ? Session["areaOfficePermission"].ToString() : Office + "," + Session["areaOfficePermission"].ToString());
                            oCardsRequestsRequest = conApi.connectionApiGetList<CardsRequestsRequest>("apiCardsRequests", "GetAllCardRequest", off);
                        }
                    }
                }
                else //search
                {
                    if (Session["SearchOffice"] != null)
                        lstrSearch.Add(Session["SearchOffice"].ToString());
                    else
                    {
                        if (Session["areaOfficePermission"].ToString() == "1") // هنا لما بكون ادمن السيشن دى مش بتتملى عشان بيكون متاح عنده كل حاجه
                            lstrSearch.Add(null);
                        else
                            lstrSearch.Add(Office == null ? Session["areaOfficePermission"].ToString() : Office + "," + Session["areaOfficePermission"].ToString()); // office code
                    }

                    lstrSearch.Add(Area == null ? null : Area);  //area code
                    lstrSearch.Add(status == null ? null : generalMethods.sConcatString(status, ',')); // status code
                    lstrSearch.Add(formCollection["dateRecive"] == null ? null : formCollection["dateRecive"].ToString()); // date recieve
                    lstrSearch.Add(formCollection["dateDelivery"] == null ? null : formCollection["dateDelivery"].ToString()); // date delivery
                    lstrSearch.Add(formCollection["dateRequest"] == null ? null : formCollection["dateRequest"].ToString()); // date request
                    lstrSearch.Add(formCollection["requestName"] == null ? null : formCollection["requestName"].ToString()); // request name 
                    oCardsRequestsRequest = conApi.connectionApiSearchList<CardsRequestsRequest>("apiCardsRequests", "PostSearchCardRequest", lstrSearch);
                    SModel.LModels = oCardsRequestsRequest.LModels;
                    if (formCollection.Count != 0)
                    {
                        search = "search";
                        return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office });
                    }
                }
                if (oCardsRequestsRequest == null)
                {
                    oCardsRequestsRequest = new CardsRequestsRequest();
                    oCardsRequestsRequest.LModels = new List<DataAccessLayer.Models.CardsRequestModel>();
                }
                return PartialView(oCardsRequestsRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Page to Inser / Edit Card Request
        /// </summary>
        /// <param name="CardId">Card Request Code need for Edit request</param>
        /// <returns>view page for insert / edit</returns>
        public ActionResult _vpCardsRequestsAddOrEdit(int? CardId)
        {
            try
            {
                viewBags();
                CardsRequestsRequest CardRequest = new CardsRequestsRequest();
                if (CardId != null) // Edit 
                    CardRequest = conApi.connectionApiGetList<CardsRequestsRequest>("apiCardsRequests", "GetCardById", CardId.ToString());
                return PartialView(CardRequest);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// function that insert /  edit card request data
        /// </summary>
        /// <param name="oModalRequest">data for card request </param>
        /// <param name="workerId">string of workers codes</param>
        /// <param name="hNationalID">string of workers national Ids</param>
        /// <param name="hInsuranceNumber">string of workers insurance numbers</param>
        /// <param name="hWorkerCareer">string of workers careers codes</param>
        /// <param name="hSkillDegree">string of workers skills degrees codes</param>
        /// <returns>view page of Card Request</returns>
        [HttpPost]
        public ActionResult _vpCardsRequestsAddOrEdit(CardsRequestsRequest oModalRequest, string workerId, string hNationalID, string hInsuranceNumber, string hWorkerCareer, string hSkillDegree)
        {
            try
            {
                //هنا حفظ طلبية كروت جديده و مافيش عمال فيها
                if (String.IsNullOrEmpty(workerId) && oModalRequest.OModel.iCardsRequestCode == 0)
                {
                    TempData["msg"] = generalVariables.CardRequestError;
                    return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office, inPage = insPageNumber, cd = 6 });
                }
                else if (oModalRequest.OModel.iCardsRequestCode > 0)
                {
                    int Count = db.cardsRequestWorkers.Where(x => x.cardsRequestCode == oModalRequest.OModel.iCardsRequestCode).ToList().Count;
                    // هنا عملت تعديل و روحت استغبيت و مسحت العمال الى عندى 
                    if (Count == 0 && String.IsNullOrEmpty(workerId))
                    {
                        TempData["msg"] = generalVariables.CardRequestError;
                        return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office, inPage = insPageNumber, cd = 6 });
                    }
                }
                oModalRequest.OModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                oModalRequest.OModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oCardsRequestsRequest = conApi.connectionApiPost<CardsRequestsRequest>("apiCardsRequests", "PostSaveCardRequest", oModalRequest, workerId);
                TempData["msg"] = (oCardsRequestsRequest.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone);
                return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office, inPage = insPageNumber, cd = 6 });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// view page of cards requests
        /// </summary>
        /// <returns>view page</returns>
        public PartialViewResult _vpCardsRequestsSearch()
        {
            ViewBag.status = new SelectList(db.cardsStatus.ToList(), "cardsStatusCode", "cardsStatusName");
            return PartialView();
        }
        /// <summary>
        /// function for delete cards requests
        /// </summary>
        /// <param name="IDDelete">card request Code Want To Delete</param>
        /// <returns>view page of card request</returns>
        public ActionResult vCardsRequestDelete(int? IDDelete)
        {
            try
            {
                oCardsRequestsRequest = conApi.connectionApiDelete<CardsRequestsRequest>("apiCardsRequests", "DeleteCard", IDDelete.ToString());
                TempData["msg"] = (oCardsRequestsRequest.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone);
                return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// cancel cards requests search
        /// </summary>
        /// <param name="cp">code for cards requests page</param>
        /// <param name="cd">code for module Insurance Authority</param>
        /// <returns>view page of cards requests</returns>
        public ActionResult vCancelSearchCardRequest(int? cp, int? cd)
        {
            try
            {
                SModel = new CardsRequestsRequest();
                SModel.LModels = null;
                search = null;
                Session["SearchOffice"] = null;
                return RedirectToAction("vCardsRequestsIndex", "CardsRequests", new { areas = Area, cp = cp, cd = cd, inPage = 1, Offices = Office });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// delete worker for card request
        /// </summary>
        /// <param name="IDDelete">worker Code Want To Delete From Request</param>
        /// <returns>view page of cards requests</returns>
        public ActionResult vcardRequestDeleteWorker(int? IDDelete)
        {
            try
            {
                oCardsRequestsRequest = conApi.connectionApiDelete<CardsRequestsRequest>("apiCardsRequests", "DeleteWorkerRequest", IDDelete.ToString());
                TempData["msg"] = (oCardsRequestsRequest.bIsDeleted ? generalVariables.DeleteDone : generalVariables.DeleteNotDone);
                return RedirectToAction("vCardsRequestsIndex", new { areas = Area, Offices = Office, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
        #region تفعيل / ايقاف الكارت
        /// <summary>
        /// page of Active / DeActive card request for worker
        /// </summary>
        /// <param name="WorkId">worker code</param>
        /// <param name="cd">code of module insurance authority</param>
        /// <param name="screen">current page name</param>
        /// <returns>view page</returns>
        public ActionResult vCardsRequestsDeActivationIndex(int? WorkId, int? cd, string screen)
        {
            try
            {
                if (cd != null)
                    cd_ = (int)cd;
                if (model == null || screen != null)
                {
                    model = new WorkerRequest();
                    model.LCardsWorkModel = null;
                }
                int uc = Convert.ToInt32(Session["uc"].ToString());
                TempData["checkLevel2"] = db.CheckModuleUserPermisiom(uc, cd).ToList();
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// view page Active / DeActive card request for worker
        /// </summary>
        /// <param name="WorkerCode">worker code</param>
        /// <param name="inPage">paage number</param>
        /// <returns>list of card status</returns>
        public ActionResult _vpCardsRequestsDeActivation(int? WorkerCode, int? inPage)
        {
            try
            {
                WorkerID = WorkerCode;
                insPageNumber = inPage;
                if (WorkerCode != null)
                    Session["workerCode"] = WorkerCode;
                if (WorkerCode == null)
                    WorkerCode = Convert.ToInt32(Session["workerCode"].ToString());
                oWorkerRequest = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorker", WorkerCode.ToString());
                TempData["sWorkerName"] = oWorkerRequest.OModel.sWorkerName;
                TempData["sWorkerInsuranceNum"] = oWorkerRequest.OModel.sWorkerInsuranceNum;
                TempData["sWorkerNationalID"] = oWorkerRequest.OModel.sWorkerNationalID;
                var data = db.workerDetails.Where(x => x.workerCode == WorkerCode).OrderByDescending(x => x.workerDetailsCode).FirstOrDefault();
                if (data.cardCode != null)
                    TempData["isAtive"] = db.cards.FirstOrDefault(x => x.cardCode == data.cardCode).isActive;
                else
                {
                    TempData["isAtive"] = null;
                    oWorkerRequest.LCardsWorkModel = null;
                }
                if (model.LCardsWorkModel == null)
                {
                    oWorkerRequest = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetWorkerCardsStatus", WorkerCode.ToString());
                    if (oWorkerRequest == null)
                    {
                        oWorkerRequest = new WorkerRequest();
                        oWorkerRequest.LCardsWorkModel = new List<DataAccessLayer.Models.CardWorkerStopActiveModel>();
                    }
                }
                else
                    oWorkerRequest.LCardsWorkModel = model.LCardsWorkModel;
                if (oWorkerRequest.LCardsWorkModel.Count > 0)
                    Session["StartDate"] = oWorkerRequest.LCardsWorkModel.OrderByDescending(x => x.iCardWorkerStopActiveCode).Take(1).FirstOrDefault().sDateStartStopActive;
                return PartialView(oWorkerRequest.LCardsWorkModel.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// view page for Insert / Edit status of worker card
        /// </summary>
        /// <param name="ID">code of status card</param>
        /// <returns>Data of card status if Edit Or empty if Insert Status</returns>
        public ActionResult _vpCardsRequestsDeActivationAddOrEdit(int? ID)
        {
            try
            {
                WorkerRequest CardsWork = new WorkerRequest();                 
                if (ID != null)// Edit
                    CardsWork = conApi.connectionApiGetList<WorkerRequest>("apiWorker", "GetObjWorkerCardsStatus", ID.ToString());
                return PartialView(CardsWork);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// insert / edit status of card for workers
        /// </summary>
        /// <param name="oModalRequest">Data for status card</param>
        /// <returns>view page of card Active/DeActive</returns>
        [HttpPost]
        public ActionResult _vpCardsRequestsDeActivationAddOrEdit(WorkerRequest oModalRequest)
        {
            try
            {
                oModalRequest.OcardsWorkModel.iWorkerCode = (int)WorkerID;
                oModalRequest.OcardsWorkModel.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oModalRequest.OcardsWorkModel.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                if (oModalRequest.OcardsWorkModel.iCardWorkerStopActiveCode > 0) // Edit
                {
                    oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostEditWorkerCardsStatus", oModalRequest, oModalRequest.OcardsWorkModel.iCardWorkerStopActiveCode.ToString());
                    TempData["msg"] = (oWorkerRequest.bIsEdit? generalVariables.EditDone: generalVariables.EditNotDone);
                }
                else // Save
                {
                    oWorkerRequest = conApi.connectionApiPost<WorkerRequest>("apiWorker", "PostSaveWorkerCardsStatus", oModalRequest, null);
                    TempData["msg"] = (oWorkerRequest.bIsSaved ? generalVariables.SaveDone : generalVariables.SaveNotDone);
                }
                return RedirectToAction("vCardsRequestsDeActivationIndex", new { WorkId = WorkerID, inPage = insPageNumber, cd = cd_ });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// page for search of Card worker status
        /// </summary>
        /// <returns>view page </returns>
        public ActionResult _vpCardsRequestsDeActivationSearch()
        {
            return PartialView();
        }
        /// <summary>
        /// search for worker card status
        /// </summary>
        /// <param name="formCollection">Data for worker card search</param>
        /// <returns>view page of worker card Active/DeActive</returns>
        [HttpPost]
        public ActionResult _vpCardsRequestsDeActivationSearch(FormCollection formCollection)
        {
            try
            {
                List<string> CardsSearch = new List<string>();
                CardsSearch.Add(WorkerID.ToString());
                CardsSearch.Add(formCollection["officeName"].ToString() == "" ? null : formCollection["officeName"].ToString().Split('\t')[0]);
                CardsSearch.Add(formCollection["areaName"].ToString() == "" ? null : formCollection["areaName"].ToString().Split('\t')[0]);
                CardsSearch.Add(formCollection["dateFrom"].ToString() == "" ? null : formCollection["dateFrom"].ToString().Split('\t')[0]);
                CardsSearch.Add(formCollection["dateTo"].ToString() == "" ? null : formCollection["dateTo"].ToString().Split('\t')[0]);
                CardsSearch.Add(formCollection["reason"].ToString() == "" ? null : formCollection["reason"].ToString().Split('\t')[0]);
                CardsSearch.Add(formCollection["notes"].ToString() == "" ? null : formCollection["notes"].ToString().Split('\t')[0]);
                oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostSearchCardStatus", CardsSearch);
                model.LCardsWorkModel = oWorkerRequest.LCardsWorkModel;
                return RedirectToAction("vCardsRequestsDeActivationIndex", new { WorkId = WorkerID, inPage = 1, cd = cd_ });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// cancel search of worker card status 
        /// </summary>
        /// <param name="WorkId">worker card</param>
        /// <param name="cd">code of module insurance authority</param>
        /// <returns>view page of worker card Active/DeActive </returns>
        public ActionResult vCancelStopCardSearch(int? WorkId, int? cd)
        {
            model = new WorkerRequest();
            model.LCardsWorkModel = null;
            return RedirectToAction("vCardsRequestsDeActivationIndex", "CardsRequests", new { WorkId = WorkId, cd = cd, inPage = 1 });
        }
        #endregion
        /// <summary>
        /// Data need in DropDown lists
        /// </summary>
        void viewBags()
        {
            // مستوى المهاره
            ViewBag.skillDegree = new SelectList(db.skillDegrees.ToList(), "skillDegreeCode", "skillDegreeName");
            // career المهن
            ViewBag.career = new SelectList(db.careers.ToList(), "careerCode", "careerName");
        }
    }
}
