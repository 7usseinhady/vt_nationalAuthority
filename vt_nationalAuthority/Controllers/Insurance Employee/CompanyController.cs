using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace vt_nationalAuthority.Controllers.Insurance_Employee
{
    public class CompanyController : Controller
    {
        ConnectionApi conApi = new ConnectionApi();
        GeneralMethods generalMethods = new GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        CardsRequestsRequest oCardsRequestsRequest = new CardsRequestsRequest();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        // GET: Company
        /// <summary>
        ///  Get All CardRequest From Inurance Authority To Company
        /// </summary>
        /// <param name="formCollections">Data Of Request</param>
        /// <param name="status">Current Status Of Request</param>
        /// <returns>View Page</returns>
        public ActionResult CompanyIndex(FormCollection formCollections,string[] status)
        {
            try
            {
                ViewBag.status = new SelectList(db.cardsStatus.ToList(), "cardsStatusCode", "cardsStatusName");
                List<string> SearchList = new List<string>();
                SearchList.Add(formCollections["dateFrom"] == null ? null : formCollections["dateFrom"].ToString());
                SearchList.Add(formCollections["dateTo"] == null ? null : formCollections["dateTo"].ToString());
                SearchList.Add(status == null ? null : generalMethods.sConcatString(status, ','));
                SearchList.Add(formCollections["requestName"] == null ? null : formCollections["requestName"].ToString());
                oCardsRequestsRequest = conApi.connectionApiSearchList<CardsRequestsRequest>("apiCardsRequests", "PostSearchCompany", SearchList);
                return View(oCardsRequestsRequest.LcompanySearch.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /// <summary>
        /// Edit Current Status Of Request
        /// </summary>
        /// <param name="ID">Request Code</param>
        /// <returns>Edit Page</returns>
        public ActionResult EditRequestStatus(int ID)
        {
            try
            {
                CardsRequestsRequest CardRequest = new CardsRequestsRequest();
                CardRequest = conApi.connectionApiGetList<CardsRequestsRequest>("apiCardsRequests", "GetRequestId", ID.ToString());
                ViewBag.statusEdit = new SelectList(db.cardsStatus.ToList(), "cardsStatusCode", "cardsStatusName", CardRequest.ORequestById.iCardsStatusCode);
                ViewBag.CardName = CardRequest.ORequestById.sCardsRequestName;
                return PartialView(CardRequest);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Save Edit Current Status Of Request
        /// </summary>
        /// <param name="oModalRequest">Data Of Current Status</param>
        /// <param name="statusEdit">Status Of Request</param>
        /// <returns>View Page Of Requests</returns>
        [HttpPost]
        public ActionResult EditRequestStatus(CardsRequestsRequest oModalRequest,string statusEdit)
        {
            try
            {
                oModalRequest.ORequestById.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                oModalRequest.ORequestById.iCardsStatusCode = Convert.ToInt32(statusEdit);
                oCardsRequestsRequest = conApi.connectionApiPost<CardsRequestsRequest>("apiCardsRequests", "PostEditCardRequest", oModalRequest, null);
                if (oCardsRequestsRequest.bIsEdit)
                    TempData["msg"] = generalVariables.EditDone;
                else
                    TempData["msg"] = generalVariables.EditNotDone;

                return RedirectToAction("CompanyIndex", new { inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /// <summary>
        /// Save Recive Request
        /// </summary>
        /// <param name="Id">Request Code</param>
        /// <returns>View Page of Requests</returns>
        public ActionResult ReciveRequest(int Id)
        {
            try
            {
                companyCardRequest model = new companyCardRequest();
                model.cardsRequestCode = Id;
                model.cardsStatusCode = 2;
                model.dateInsert = DateTime.Now;
                db.companyCardRequests.Add(model);
                if (db.SaveChanges() > 0)
                    TempData["msg"] = generalVariables.SaveDone;
                else
                    TempData["msg"] = generalVariables.SaveNotDone;
                return RedirectToAction("CompanyIndex");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}