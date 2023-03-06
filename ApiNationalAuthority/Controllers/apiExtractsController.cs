using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Extracts. 
    /// </summary>
    public class apiExtractsController : ApiController
    {
        // Get : apiExtracts المستخلصات
        ExtractsRequest oRequest = new ExtractsRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        ///   Get All Extracts.
        /// </summary>
        /// <param name="sStr"> String Of Process Code And User Code. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest GetExtracts([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            // المقاول
            if (lString.Count == 2)
                oRequest.GetList(lString[0], lString[1]);
            else // موظف التأمينات
                oRequest.GetList(lString[0]);

            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Extract.
        /// </summary>
        /// <param name="sStr"> Extract Code. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest GetExtractsByID([FromUri] string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest PostSearchExtracts([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewExtracts"> New Request. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest PostSaveExtracts([FromBody]ExtractsRequest oNewExtracts)
        {
            oRequest.vSave(oNewExtracts.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewExtracts"> Edit Request </param>
        /// <param name="sStr"> Code Of Extract Will Be Edit. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest PostEditExtracts([FromBody]ExtractsRequest oNewExtracts, [FromUri]string sStr)
        {
            oRequest.vEdit(oNewExtracts.OModel, Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Accept Extract.
        /// </summary>
        /// <param name="oNewProcess"> New Request. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest PostAcceptExtract([FromBody]ExtractsRequest oNewProcess) // التأكيد على المستخلص
        {
            oRequest.vAcceptExtract(oNewProcess);
            return oRequest;
        }


        /// <summary>
        ///   Confirm Of Payment On Extract.
        /// </summary>
        /// <param name="oNewProcess"> New Request. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest PostPaidExtract([FromBody]ExtractsRequest oNewProcess) // التأكيد تسديد الاشتراك على المستخلص
        {
            oRequest.vPaidExtract(oNewProcess);
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Extract.
        /// </summary>
        /// <param name="sStr"> String Of Extract Code , Process Code And User Code. </param>
        /// <returns> Request. </returns>
        public ExtractsRequest DeleteExtracts([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            // المقاول
            if (lString.Count == 4)
                oRequest.vDeleteoBJ(Convert.ToInt32(lString[0]), lString[1], Convert.ToInt32(lString[2]), true);
            else // موظف التأمينات
                oRequest.vDeleteoBJ(Convert.ToInt32(lString[0]), lString[1], Convert.ToInt32(lString[2]), false);

            return oRequest;
        }

    }
}