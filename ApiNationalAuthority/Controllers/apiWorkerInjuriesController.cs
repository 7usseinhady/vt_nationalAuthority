using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Worker Injuries. 
    /// </summary>
    public class apiWorkerInjuriesController : ApiController
    {
        // Get : apiWorkerInjuries اصابات العامل
        WorkerInjuriesRequest oRequest = new WorkerInjuriesRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;

        
        /// <summary>
        ///   Get All Of Worker Injuries.
        /// </summary>
        /// <param name="sStr"> Worker Code. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest GetWorkerInjuries([FromUri] string sStr)
        {
            oRequest.GetList(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Worker Injury.
        /// </summary>
        /// <param name="sStr"> Worker Injury Code. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest GetWorkerInjuriesByID([FromUri] string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Get Worker Data.
        /// </summary>
        /// <param name="sStr"> Worker Code. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest GetWorkerData([FromUri] string sStr)
        {
            oRequest.GetWorkerData(Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest PostSearchWorkerInjuries([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewWorkerInjuries"> New Request. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest PostSaveWorkerInjuries([FromBody]WorkerInjuriesRequest oNewWorkerInjuries)
        {
            oRequest.vSave(oNewWorkerInjuries.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewWorkerInjuries"> Edit Request </param>
        /// <param name="sStr"> Code Of Worker Injury Will Be Edit. </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest PostEditWorkerInjuries([FromBody]WorkerInjuriesRequest oNewWorkerInjuries, [FromUri]string sStr)
        {
            oRequest.vEdit(oNewWorkerInjuries.OModel, Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Worker Injury.
        /// </summary>
        /// <param name="sStr"> Code Of Worker Injury Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public WorkerInjuriesRequest DeleteWorkerInjuries([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]),lString[1]);
            return oRequest;
        }

    }
}