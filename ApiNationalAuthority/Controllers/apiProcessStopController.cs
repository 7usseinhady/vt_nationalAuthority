using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Process Srop. 
    /// </summary>
    public class apiProcessStopController : ApiController
    {
        // Get : apiProcessStop ايقاف العمليه
        ProcessStopRequest oRequest = new ProcessStopRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        ///   Get All Reasons Of Stopping Process.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest GetProcessStop([FromUri] string sStr)
        {
            oRequest.GetList(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Reason Of Stopping Process.
        /// </summary>
        /// <param name="sStr"> Reason Of Stopping Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest GetProcessStopByID([FromUri] string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Get Last Object From Reasons Of Stopping Process 
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest GetProcessStopPrevByID([FromUri] string sStr)
        {
            oRequest.GetProcessStopPrev(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Main Process
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest GetProcessData([FromUri] string sStr)
        {
            oRequest.GetProcessData(Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest PostSearchProcessStop([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewProcessStop"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest PostSaveProcessStop([FromBody]ProcessStopRequest oNewProcessStop)
        {
            oRequest.vSave(oNewProcessStop.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewProcessStop"> Edit Request </param>
        /// <param name="sStr"> Code Of Reason Of Stopping Process Will Be Edit. </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest PostEditProcessStop([FromBody]ProcessStopRequest oNewProcessStop, [FromUri]string sStr)
        {
            oRequest.vEdit(oNewProcessStop.OModel, Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Reason Of Stopping Process.
        /// </summary>
        /// <param name="sStr"> Code Of Reason Of Stopping Process Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public ProcessStopRequest DeleteProcessStop([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]),lString[1]);
            return oRequest;
        }

    }
}