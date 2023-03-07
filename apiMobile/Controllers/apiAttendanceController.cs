using DataAccessLayer.Models;
using System;
using System.Web.Http;

namespace ApiMobile.Controllers
{
    public class apiAttendanceController : ApiController
    {
        /// <summary>
        ///   List Of Workers Attendance Will Be Attend In Process. 
        /// </summary>
        /// <param name="lWAttendance"> List Of Workers Attendance. </param>
        /// <returns> Object Of Results</returns>
        /// 
        [HttpPost]
        public results PostWorkerAttendance([FromBody] workersAttendance lWAttendance)
        {
            try
            {
                results sResult = new wAttendanceModel().PostWorkerAttendans(lWAttendance.lWorkerAttendance);
                return sResult;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        ///   Search In Workers Attendance By Process Code , Date From Attend And Date End Attend.
        /// </summary>
        /// <param name="wAttenadance"> Object Of Worker Attendance. </param>
        /// <returns> Object OF Workers Attendance.  </returns>
        [HttpPost]
        public workersAttendance PostWorkerAttendanceSearch([FromBody]workersAttendance wAttenadance)
        {
            try
            {
                workersAttendance workersAttendance = new workersAttendance();
                workersAttendance.lWorkerAttendance = new wAttendanceModel().GetWorkerAttendansSearch(wAttenadance.oWorkerAttendance.iProcessCode, wAttenadance.oWorkerAttendance.dtFromAttendanceDate, wAttenadance.oWorkerAttendance.dtToAttendanceDate);
                return workersAttendance;
            }
            catch
            {
                return null;
            }
        }

    }
}