using DataAccessLayer;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Attendance
{
    public class attendanceController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        // GET: attendance
        /// <summary>
        ///  Constructor Function , going to Login Page when session for User End
        /// </summary>
        public attendanceController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Going to Function show Attendance workers
        /// </summary>
        /// <returns>page attendance workers</returns>
        public ActionResult vAttendanceIndex()
        {
            return View();
        }
        /// <summary>
        ///  page workers attendance for process details in Contrators users
        /// </summary>
        /// <returns>Show page</returns>
        public PartialViewResult _vpAttendance()
        {
            return PartialView();
        }
        /// <summary>
        /// show page Manual Worker attendance
        /// </summary>
        /// <returns>show page</returns>
        public PartialViewResult AttendanceManual()
        {
            return PartialView();
        }
        /// <summary>
        /// going to function _vpAddWorkers to registration  worker attendance in insurance
        /// </summary>
        /// <param name="pCode">process code  </param>
        /// <returns>function registration worker attendance</returns>
        public ActionResult WorkerAttend(int?pCode)
        {
            return RedirectToAction("_vpAddWorkers", "InsuranceEmployee", new { paths = "attendance" , pCode =pCode});
        }
        /// <summary>
        /// going to function _vpWorkerAttendance for search worker attendance by his name
        /// </summary>
        /// <param name="formCollection">Data is needing for search</param>
        /// <returns>search for worker attendance</returns>
        [HttpPost]
        public ActionResult SearchWorkerAttendance(FormCollection formCollection)
        {
            return RedirectToAction("_vpWorkerAttendance", "Workers", new { WorkerName = formCollection["WorkerName"] });
        }
    }
}
