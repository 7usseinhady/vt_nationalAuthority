using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Attendance
{
    public class previousPeriodController : Controller
    {
        ConnectionApi conApi = new ConnectionApi();
        GeneralMethods generalMethods = new GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        WorkerRequest oWorkerRequest = new WorkerRequest();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);

        // GET: previousPeriod
        static int? ProcessCode;
        static int? WorkerID;
        /// <summary>
        ///  Constructor Function , going to Login Page when session for User End
        /// </summary>
        public previousPeriodController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// View Pervious period for workers
        /// </summary>
        /// <returns>view page</returns>
        public PartialViewResult _vpPreviousPeriod()
        {
            return PartialView();
        }
        /// <summary>
        /// View Pervious period for workers in Month
        /// </summary>
        /// <param name="processID">process code</param>
        /// <param name="workerCode">worker code</param>
        /// <returns>view page</returns>
        public PartialViewResult _vpPeriodMonth(int? processID, int? workerCode)
        {
            ProcessCode = (processID != null ? processID : null);
            WorkerID = (workerCode != null ? workerCode : null);
            return PartialView();
        }
        /// <summary>
        /// show workers attendance during month/year
        /// </summary>
        /// <param name="Month">month number</param>
        /// <param name="year">year number</param>
        /// <param name="inPage">page is showing</param>
        /// <returns>workers</returns>
        public PartialViewResult _vpWorkerDays(int? Month, string year, int? inPage)
        {
            insPageNumber = 1;
            if (String.IsNullOrEmpty(year) && String.IsNullOrEmpty(Month.ToString()))
            {
                return PartialView();
            }
            else
            {
                int startMonth = 0, EndMonth = 0;
                string startYear = "", endYear = "";
                if (Month == 1)
                {
                    startMonth = 12;
                    startYear = (Convert.ToInt32(year) - 1).ToString();
                }
                else
                {
                    startMonth = (int)Month - 1;
                    startYear = year;
                }
                if (Month == 12)
                {
                    EndMonth = 1;
                    endYear = (Convert.ToInt32(year) + 1).ToString();
                }
                else
                {
                    EndMonth = (int)Month + 1;
                    endYear = year;
                }
                List<string> AttandanceSearch = new List<string>();
                AttandanceSearch.Add(ProcessCode.ToString());
                AttandanceSearch.Add(WorkerID.ToString());
                AttandanceSearch.Add(startYear);
                AttandanceSearch.Add(endYear);
                AttandanceSearch.Add(startMonth.ToString());
                AttandanceSearch.Add(EndMonth.ToString());
                oWorkerRequest = conApi.connectionApiSearchList<WorkerRequest>("apiWorker", "PostSearchWorkerAttendance", AttandanceSearch);
            }

            if (oWorkerRequest == null)
            {
                oWorkerRequest = new WorkerRequest();
                oWorkerRequest.LworkerAttendanceModel = new List<DataAccessLayer.Models.WorkerAttendanceModel>();
            }
            if (oWorkerRequest.LworkerAttendanceModel == null)
                oWorkerRequest.LworkerAttendanceModel = new List<DataAccessLayer.Models.WorkerAttendanceModel>();

            return PartialView(oWorkerRequest.LworkerAttendanceModel.ToPagedList(insPageNumber ?? 1, 31));
        }
    }
}