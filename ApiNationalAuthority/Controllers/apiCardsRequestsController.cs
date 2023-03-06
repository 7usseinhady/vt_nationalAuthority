using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    public class apiCardsRequestsController : ApiController
    {
        //apiCardsRequests
        CardsRequestsRequest OCardsRequestsRequest = new CardsRequestsRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;

        /// <summary>
        /// Get List Of Cards Request By Parameter
        /// </summary>
        /// <param name="sStr">String Need For Filter</param>
        /// <returns> Request. </returns>
        [HttpGet]
        public CardsRequestsRequest GetAllCardRequest(string sStr)
        {
            OCardsRequestsRequest.GetInit(sStr);
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Get List Of Cards Request By Parameter
        /// </summary>
        /// <returns> Request. </returns>
        public CardsRequestsRequest GetAllCardReq()
        {
            OCardsRequestsRequest.GetInit(null);
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Get One Card Request
        /// </summary>
        /// <param name="sStr">Card Request Code</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest GetCardById(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OCardsRequestsRequest.GetInitObject(Convert.ToInt32(lString[0]));
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Get Request To Company
        /// </summary>
        /// <param name="sStr">Card Request Id</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest GetRequestId(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OCardsRequestsRequest.GetRrequestByID(Convert.ToInt32(lString[0]));
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Delete Card Request
        /// </summary>
        /// <param name="sStr">Card Request Code</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest DeleteCard([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OCardsRequestsRequest.vDelete(Convert.ToInt32(lString[0]));
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Delete Worker From Card Request
        /// </summary>
        /// <param name="sStr">Worker Card Request Code</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest DeleteWorkerRequest([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OCardsRequestsRequest.vDeleteWorkerRequest(Convert.ToInt32(lString[0]));
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Save Card Request
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save</param>
        /// <param name="sStr">Workers Codes For Request</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest PostSaveCardRequest([FromBody]CardsRequestsRequest oModalRequest, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OCardsRequestsRequest.vSave(oModalRequest.OModel, lString);
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Edit Status For Request
        /// </summary>
        /// <param name="oModalRequest">Data Need For Edit Status</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest PostEditCardRequest([FromBody]CardsRequestsRequest oModalRequest)
        {
            OCardsRequestsRequest.EditStatusRequest(oModalRequest.ORequestById);
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Search For Card Requests
        /// </summary>
        /// <param name="lStr">List Of Data Need For Search</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest PostSearchCardRequest([FromBody] List<string> lStr)
        {
            OCardsRequestsRequest.vSearch(lStr);
            return OCardsRequestsRequest;
        }


        /// <summary>
        /// Search For Card Requests In Company Module
        /// </summary>
        /// <param name="lStr">List Of Data Need For Search</param>
        /// <returns> Request. </returns>
        public CardsRequestsRequest PostSearchCompany([FromBody] List<string> lStr)
        {
            OCardsRequestsRequest.vSearchCompany(lStr);
            return OCardsRequestsRequest;
        }
    }
}