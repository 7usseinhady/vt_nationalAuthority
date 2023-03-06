using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.App_Code;

namespace vt_nationalAuthority.Controllers
{
    public class jsonFunctionsController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        nat_wsdl nat = new nat_wsdl();
        cover_wsdl cover = new cover_wsdl();
        /// <summary>
        /// Get Worker Derails Need For Worker Attendance
        /// </summary>
        /// <param name="Num">National Id Or Insurance Number</param>
        /// <param name="tybe">Check If National Id Or Inusrance Authority</param>
        /// <param name="processCode">Process Code</param>
        /// <param name="WorkerReqCard">Code Request Card For Worker</param>
        /// <returns>Worker Deatils</returns>
        public JsonResult GetWorkerDetails(string Num, string tybe, int? processCode, string WorkerReqCard)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                string y = "", worker_data = "";
                bool check = false;
                int uc = Convert.ToInt32(Session["uc"].ToString());
                if (Num.Length == 14) // الرقم القومى
                {
                    worker_data = nat.nat_data(Num);
                    if ((!worker_data.Contains("ER") && !worker_data.Contains("Error")))
                    {
                        check = true;
                        string[] x = worker_data.Split('#');
                        string[] cover_data = cover.cover_data(x[1]);
                        for (int i = 0; i < cover_data.Length; i++)
                        {
                            y += cover_data[i] + @"\";
                        }
                        db.worker_data_update_ims(y, uc);
                    }
                }
                else if (Num.Length == 9) // الرقم التأمينى
                {
                    string[] cover_data = cover.cover_data(Num);
                    if (!cover_data[0].Contains("ER"))
                    {
                        check = true;
                        for (int i = 0; i < cover_data.Length; i++)
                        {
                            y += cover_data[i] + @"\";
                        }
                        db.worker_data_update_ims(y, uc);
                    }
                }
                /// هنا فيه ايرور في الويسدل
                if (y.Contains("GOV") || y.Contains("OPEN") || check == false)
                {
                    object data;
                    if (worker_data.Contains("ERR-NATIONAL"))
                    {
                        try
                        {
                            //string path = "http://10.240.111.78/nat_api/api/NationaId/GetInfoNationalId?NationalId=" + Num;
                            //string NationalInfo = conApi.connectionApiGetWithLocalHost<string>(path);
                            // data = "CreateInsur";
                            var data2 = new List<GetWorkerInformation_Result>();

                            if (tybe == "national")
                            {
                                data2 = db.GetWorkerInformation(Num, null, null, processCode).ToList();
                            }
                            if (tybe == "insurance")
                            {
                                data2 = db.GetWorkerInformation(null, Num, null, processCode).ToList();
                            }
                            if (data2.Count == 0)
                            {
                                db.SetWorkerNationalOnly(Num);
                                data2 = db.GetWorkerInformation(Num, null, null, processCode).ToList();
                            }
                            data = data2;
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        catch
                        {
                            data = "ERR-NATIONAL";
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }

                    else if (y.Contains("OPEN"))
                        data = "OPEN";
                    else
                        data = "Error";
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(tybe) && String.IsNullOrEmpty(WorkerReqCard))
                        {
                            var data = new List<GetWorkerInformation_Result>();

                            if (tybe == "national")
                                data = db.GetWorkerInformation(Num, null, null, processCode).ToList();
                            if (tybe == "insurance")
                                data = db.GetWorkerInformation(null, Num, null, processCode).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else if (!String.IsNullOrEmpty(WorkerReqCard))
                        {
                            var data = new List<GetWorkerCardInformation_Result>();
                            if (tybe == "national")
                                data = db.GetWorkerCardInformation(Num, null).ToList();
                            if (tybe == "insurance")
                                data = db.GetWorkerCardInformation(null, Num).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var data = new List<GetWorkerInformation_Result>();
                            /// هنا بقي هجيب تفاصيل العامل 
                            data = db.GetWorkerInformation(Num, null, null, processCode).ToList();
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch
                    {
                        var data = "error";
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                var data = "error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Get Notifications For User
        /// </summary>
        /// <returns>List Of Notifications</returns>
        public JsonResult GetNotification()
        {
            try
            {
                int? user_code = 0;
                user_code = int.Parse(Session["uc"].ToString());
                var model = db.GetNotifications(user_code, null, "0001-01-01").ToList();
                var userDetails = db.users.Where(user => user.userCode == user_code).FirstOrDefault();
                return Json(model.OrderByDescending(x => x.dateInsert).Take(20), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Get Insurance Number For Contractors Or Refrense Side
        /// </summary>
        /// <param name="ID">Contractors Or Refrense Side Code</param>
        /// <returns>Return Insurance Number</returns>
        public JsonResult GetInsurNumber(int ID)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var data = db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorCode == ID).referenceSideContractorInsuranceNum;

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Set Muted Cookie
        /// </summary>
        /// <returns>Message Done Or Error</returns>
        public JsonResult setIsMutedCookie()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                // 0 => Not Muted || 1 => Muted
                string sCookieValue = Request.Cookies["isMuted"].Value;
                Response.Cookies["isMuted"].Value = String.IsNullOrEmpty(sCookieValue) ? "1" : (sCookieValue == "0" ? "1" : "0");
                return Json("Done", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        ////// تحضير عمال
        ////public ActionResult vWorkerAttendance()
        ////{
        ////    try
        ////    {
        ////        //GetWorkerAttendans();
        ////    }
        ////    catch
        ////    {

        ////    }
        ////    return View();
        ////}
        ////public void GetWorkerAttendans()
        ////{
        ////    try
        ////    {
        ////        List<orascomWorker> workers = db.orascomWorkers.ToList();

        ////        if (workers.Count > 0)
        ////        {
        ////            string sQR = "", dTime = "",sProcessCode= "";
        ////            //int iProcessCode = 47148;
        ////            int iUserCode = 3674;

        ////            string day = DateTime.Now.Day.ToString();
        ////            string month = DateTime.Now.Month.ToString();
        ////            day = day.Length == 1 ? "0" + day : day;
        ////            month = month.Length == 1 ? "0" + month : month;
        ////            int iDateAttendance = int.Parse(DateTime.Now.Year.ToString() + month + day);


        ////            #region هلف على عامل عامل ولو قطاع 4 هتم تحضيره
        ////            for (int i = 0; i <1; i++)
        ////            {
        ////                var data = "";
        ////                string y = "", worker_data = "";

        ////                try
        ////                {
        ////                    #region REHAB ويزدل الرقم القومى والتأمينى

        ////                    /// هنا عن طريق الويسدل هشوف الرقم القومى في التأمينات لو في بيانات هناك هجيبها هعمل ابديت
        ////                    /// بيها او لو مش موجود عندى هدخله عن طريق الاستورد الى عندى 

        ////                    worker_data = nat.nat_data(workers[i].الرقم_القومى.ToString()); // National ID الرقم القومى

        ////                    #region Worker Update Data تحديث بيانات العامل
        ////                    if ((!worker_data.Contains("ER") && !worker_data.Contains("Error")))
        ////                    {
        ////                        string[] allWorkerData = worker_data.Split('#');
        ////                        string[] cover_data = cover.cover_data(allWorkerData[1]);
        ////                        for (int ii = 0; ii < cover_data.Length; ii++)
        ////                        {
        ////                            y += cover_data[ii] + @"\";
        ////                        }
        ////                        db.worker_data_update_ims(y, iUserCode);
        ////                    }
        ////                    #endregion

        ////                    #region هنا هخزن نتيجه الويزدل الجايه من التأمينات

        ////                    string[] workerDataSplit = worker_data.Split('#');

        ////                    if (workerDataSplit.Length == 1) // Error
        ////                    {
        ////                        db.spUpdateResponseInsuranceWsdlWorker(workers[i].الرقم_القومى.ToString(), worker_data, worker_data, DateTime.Now, iUserCode);
        ////                    }
        ////                    else if (worker_data.Contains("**M1#")) // جايب داتا
        ////                    {
        ////                        db.spUpdateResponseInsuranceWsdlWorker(workers[i].الرقم_القومى.ToString(), worker_data, null, DateTime.Now, iUserCode);
        ////                    }
        ////                    else // Err-National OR Any Error
        ////                    {
        ////                        db.spUpdateResponseInsuranceWsdlWorker(workers[i].الرقم_القومى.ToString(), worker_data, workerDataSplit[3], DateTime.Now, iUserCode);
        ////                    }

        ////                    #endregion

        ////                    #region هنا فيه ايرور في الويسدل
        ////                    if (y.Contains("OPEN") || y.Contains("GOV"))
        ////                    {
        ////                        data = "OPEN";

        ////                        string natID = workers[i].الرقم_القومى.ToString();
        ////                        worker Worker = db.workers.Where(x => x.workerNationalID == natID).FirstOrDefault();

        ////                        if (Worker != null)
        ////                        {
        ////                            Worker.insWsdlResponse = String.IsNullOrEmpty(y) ? "Error":y;
        ////                            Worker.insWsdlDate = DateTime.Now;

        ////                            db.SaveChanges();
        ////                        }
        ////                    }
        ////                    #endregion

        ////                    #endregion

        ////                }
        ////                catch
        ////                {
        ////                    data = "Connection Not Found";
        ////                }

        ////                if (data == "") // كارت او رقم قومى
        ////                {
        ////                    sProcessCode = String.IsNullOrEmpty(sProcessCode) ? workers[i].processCode.ToString() : (sProcessCode + "$" + workers[i].processCode.ToString());
        ////                    sQR = String.IsNullOrEmpty(sQR) ? workers[i].الرقم_القومى.ToString() : (sQR + "$" + workers[i].الرقم_القومى.ToString());
        ////                    dTime = String.IsNullOrEmpty(dTime) ? DateTime.Now.ToString() : (dTime + "$" + Convert.ToDateTime(DateTime.Now.ToString()));
        ////                }
        ////            }
        ////            #endregion

        ////            #region ويزدل اول تحضير للعامل بالشهر
        ////            if (!String.IsNullOrEmpty(sQR))
        ////            {
        ////                var resultAttendance = db.spWorkerAttendanceApp(sProcessCode, iUserCode, null, sQR, dTime).FirstOrDefault();

        ////                #region First Attend In Month WSDL ويزدل اول تحضير للعامل بالشهر
        ////                if (!String.IsNullOrEmpty(resultAttendance.workersNationalIdAttended))
        ////                {
        ////                    workerModel oWorkerModel = new workerModel();

        ////                    //long processNumber = 123456789123456; //Convert.ToInt64(resultAttendance.processNumber); //  123456789123456
        ////                    // long processNumber = Convert.ToInt64(resultAttendance.processNumber); //  123456789123456
        ////                    //string[] ReturnStatus = oWorkerModel.saveFirstAttInMonthInWsdl("1", processNumber, iDateAttendance, Convert.ToInt32(resultAttendance.contractorInsuranceNum), resultAttendance.contractorLegalEntity.ToString(),
        ////                    //                                                               resultAttendance.workersNationalIdAttended, resultAttendance.workersCareerIDAttended, resultAttendance.workerAttendanceCodes, 0, 0, 0);

        ////                    string curDay = DateTime.Now.Day.ToString();
        ////                    string curMonth = DateTime.Now.Month.ToString();
        ////                    curDay = curDay.Length == 1 ? "0" + curDay : curDay;
        ////                    curMonth = curMonth.Length == 1 ? "0" + curMonth : curMonth;
        ////                    int curDateAttendance = int.Parse(DateTime.Now.Year.ToString() + curMonth + curDay);

        ////                    //string[] ReturnStatus = oWorkerModel.saveFirstAttInMonthInWsdl("1", resultAttendance.processNumber, curDateAttendance, Convert.ToInt32(resultAttendance.contractorInsuranceNum), resultAttendance.contractorLegalEntity.ToString(),
        ////                    //                                                                   resultAttendance.workersNationalIdAttended, resultAttendance.workersCareerIDAttended, resultAttendance.workerAttendanceCodes, 0, 0, 0);
        ////                }
        ////                #endregion

        ////            }
        ////            #endregion
        ////        }

        ////    }
        ////    catch
        ////    {
        ////        results res = new results();
        ////        res.result = ex.InnerException.Message;
        ////    }
        ////}

        //public void orsWorkers()
        //{
        //    nat_wsdl nat = new nat_wsdl();
        //    List<orascomWorker> orascomWorkers = db.orascomWorkers.ToList();
        //    try
        //    {
        //        for (int i = 0; i < orascomWorkers.Count; i++)
        //        {
        //            string worker_data = nat.nat_data(orascomWorkers[i].الرقم_القومى.ToString()); // National ID الرقم القومى

        //            string[] workerDataSplit = worker_data.Split('#');
        //            orascomWorker orascomWorker = db.orascomWorkers.Find(orascomWorkers[i].م);

        //            if (workerDataSplit.Length == 1)
        //            {
        //                orascomWorker.wsdlError = worker_data;
        //                orascomWorker.wsdlResponse = worker_data;
        //            }
        //            else
        //            {
        //                orascomWorker.wsdlError = workerDataSplit[3];
        //                orascomWorker.wsdlResponse = worker_data;
        //            }
        //            orascomWorker.wsdlDate = DateTime.Now;

        //            db.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        //public void orsWorkers2()
        //{
        //    nat_wsdl nat = new nat_wsdl();
        //    List<orascomWorkers2> orascomWorkers2 = db.orascomWorkers2.ToList();
        //    try
        //    {
        //        for (int i = 0; i < orascomWorkers2.Count; i++)
        //        {
        //            string worker_data = nat.nat_data(orascomWorkers2[i].الرقم_القومى.ToString()); // National ID الرقم القومى

        //            string[] workerDataSplit = worker_data.Split('#');
        //            orascomWorkers2 orascomWorker2 = db.orascomWorkers2.Find(orascomWorkers2[i].م);

        //            if (workerDataSplit.Length == 1)
        //            {
        //                orascomWorker2.wsdlError = worker_data;
        //                orascomWorker2.wsdlResponse = worker_data;
        //            }
        //            else
        //            {
        //                orascomWorker2.wsdlError = workerDataSplit[3];
        //                orascomWorker2.wsdlResponse = worker_data;
        //            }
        //            orascomWorker2.wsdlDate = DateTime.Now;

        //            db.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
    }
}