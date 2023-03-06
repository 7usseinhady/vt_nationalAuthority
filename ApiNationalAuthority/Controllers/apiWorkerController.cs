using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    public class apiWorkerController : ApiController
    {
        //apiWorker
        WorkerRequest OworkerRequest = new WorkerRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        /// Get Workers In Process
        /// </summary>
        /// <param name="sStr">Process Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetWorkerDetails(string sStr)
        {
            OworkerRequest.GetList(sStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Get List Of Workers 
        /// </summary>
        /// <param name="lStr">List Of Data Need For Filter Workers</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostGetWorkerDetails([FromBody] List<string> lStr)
        {
            OworkerRequest.GetList(lStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Get List Of Workers 
        /// </summary>
        /// <param name="lStr"> List Of Process Code , Areas Codes , Offices Codes , User Code And Worker Code. </param>
        /// <returns> Request. </returns>
        public WorkerRequest PostWorkerDetails([FromBody]  List<string> lStr)
        {
            string wk = (lStr[4] == null ? null : lStr[4]);
            OworkerRequest.GetList(lStr[0], lStr[1], lStr[2], lStr[3], wk);
            return OworkerRequest;
        }


        /// <summary>
        /// Get Phones For Worker
        /// </summary>
        /// <param name="sStr">Worker Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetWorkerPhones(string sStr)
        {
            OworkerRequest.GetPhones(Convert.ToInt32(sStr));
            return OworkerRequest;
        }


        /// <summary>
        /// Delete Phone For Worker
        /// </summary>
        /// <param name="sStr"> String Of Phone Code And Worker Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest DeleteWorkerPhone([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OworkerRequest.DeletePhone(Convert.ToInt32(lString[0]), Convert.ToInt32(lString[1]));
            return OworkerRequest;
        }


        /// <summary>
        /// Save Phone For Worker
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSaveWorkerPhone([FromBody]WorkerRequest oModalRequest)
        {
            OworkerRequest.SavePhones(oModalRequest.OworkerContactModel);
            return OworkerRequest;
        }


        /// <summary>
        /// Search For Worker Attendance
        /// </summary>
        /// <param name="lStr">List Of  Data Need For Search</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSearchWorkerAttendance([FromBody] List<string> lStr)
        {
            OworkerRequest.vSearchAttendance(lStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Get All Worker Attendance
        /// </summary>
        /// <param name="lStr">List Of Data Need To Get Worker Attendance</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostGetAttendance([FromBody] List<string> lStr)
        {
            OworkerRequest.vGetAttendance(lStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Save Worker Medical Insurance 
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSaveMedicalInsurance([FromBody]WorkerRequest oModalRequest)
        {
            OworkerRequest.SaveMI(oModalRequest.OmedicalInsuranceModel);
            return OworkerRequest;
        }


        /// <summary>
        /// Get One Worker
        /// </summary>
        /// <param name="sStr">Worker Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetWorker(string sStr)
        {
            OworkerRequest.GetInitObject(Convert.ToInt32(sStr));
            return OworkerRequest;

        }


        /// <summary>
        /// Search For Workers
        /// </summary>
        /// <param name="lStr">List Of Data Need For Search</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSearchWorkers([FromBody] List<string> lStr)
        {
            OworkerRequest.vSearch(lStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Save Worker Man-Power
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSaveManPower([FromBody] WorkerRequest oModalRequest)
        {
            OworkerRequest.SaveManPower(oModalRequest.OmanPowerModel);
            return OworkerRequest;
        }


        /// <summary>
        /// Get Worker Process
        /// </summary>
        /// <param name="sStr">Worker Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetWorkerProcess(string sStr)
        {
            OworkerRequest.GetWorkerProcess(sStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Delete Worker
        /// </summary>
        /// <param name="sStr"> String Of Worker Code and User Code Is Deleted</param>
        /// <returns> Request. </returns>
        public WorkerRequest DeleteWorker([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            OworkerRequest.vDelete(Convert.ToInt32(lString[0]), Convert.ToInt32(lString[1]));
            return OworkerRequest;
        }

        #region Card Stop Active

        /// <summary>
        /// Get All Worker Cards Status
        /// </summary>
        /// <param name="sStr">Worker Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetWorkerCardsStatus(string sStr)
        {
            OworkerRequest.GetCardStopActive(sStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Get Worker Card Status
        /// </summary>
        /// <param name="sStr">Worker Card Status Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest GetObjWorkerCardsStatus(string sStr)
        {
            OworkerRequest.GetObjCardStatus(sStr);
            return OworkerRequest;
        }


        /// <summary>
        /// Save Worker Card Status Code
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save Worker Card Status</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSaveWorkerCardsStatus([FromBody] WorkerRequest oModalRequest)
        {
            OworkerRequest.SaveCardStatus(oModalRequest.OcardsWorkModel);
            return OworkerRequest;
        }


        /// <summary>
        /// Edit Worker Card Status Code
        /// </summary>
        /// <param name="oModalRequest">Data Need For Edit Worker Card Status</param>
        /// <param name="sStr">Worker Card Status Code</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostEditWorkerCardsStatus([FromBody] WorkerRequest oModalRequest, [FromUri]string sStr)
        {
            OworkerRequest.EditCardStatus(oModalRequest.OcardsWorkModel, Convert.ToInt32(sStr));
            return OworkerRequest;
        }


        /// <summary>
        /// Search For Worker Cards Status
        /// </summary>
        /// <param name="lStr">List Of Data Need For Search</param>
        /// <returns> Request. </returns>
        public WorkerRequest PostSearchCardStatus([FromBody] List<string> lStr)
        {
            OworkerRequest.SearchCardStatus(lStr);
            return OworkerRequest;
        }
        #endregion

        //// Esraa Nageh
        //#region WSDLs

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="sStr"></param>
        //    /// <returns></returns>
        //[HttpPost]
        //public workerRequest PostWorkerAttendanceWSDL([FromUri]string sStr)
        //{
        //    OworkerRequest.workerAttendanceWSDL(sStr);
        //    return OworkerRequest;
        //}
        //#endregion

    }
}
