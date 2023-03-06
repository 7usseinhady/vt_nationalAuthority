using apiWSDLs.wsdls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAccessLayer.Models
{
    public class LoginModel
    {
        public List<string> lLogData { get; set; }
        public UserInfoModel userInfo { get; set; }
    }

    public class UserInfoModel
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        public int iUserCode { get; set; }
        public Nullable<int> iContractorCode { get; set; }
        public string sContractorName { get; set; }
        public string sContractorNumber { get; set; }
        public int iUserType { get; set; } // الادمن - الموظف - المقاول - جهه الاسناد
        public bool logState { get; set; } // mobile api (login)


        /// <summary>
        ///   Get Contractor User Data. 
        /// </summary>
        /// <param name="sUserName"> User Name. </param>
        /// <param name="sPassword"> Password. </param>
        /// <returns> Model Of User Info. </returns>
        public UserInfoModel GetContractorUserData(string sUserName, string sPassword)
        {
            var models = db.spGetContractorUserData(sUserName, sPassword).FirstOrDefault();

            UserInfoModel oUserInfoModel = new UserInfoModel();

            if (models != null)
            {
                oUserInfoModel.iUserCode = models.userCode;
                oUserInfoModel.iContractorCode = models.contCode;
                oUserInfoModel.sContractorName = models.contName;
                oUserInfoModel.sContractorNumber = models.contNum;
                oUserInfoModel.iUserType = 2; // 2 contractor
                oUserInfoModel.logState = true;
            }
            else
                oUserInfoModel.logState = false;

            return oUserInfoModel;
        }

    }
    public class ContractorProcessesModel
    {
        public List<ContProcessesModel> lContProcesses { get; set; }
    }
    public class Logs
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Request Logs.
        /// </summary>
        /// <param name="httpRequestMessage"> Represents A HTTP Requese Message. </param>
        /// <param name="ApiKey"> API KEY. </param>
        /// <param name="ApiValue"> API Value. </param>
        /// <param name="ipAddress"> IP Address. </param>
        /// <returns> Bool. </returns>
        public bool reqLogs(HttpRequestMessage httpRequestMessage, string ApiKey, string ApiValue, string ipAddress)
        {
            // Get apiUserCode If This User Signed
            bool foundUser = false;
            int? inApiUserCode = null;
            DateTime currentDate = DateTime.Now.Date;

            if (db.apiUsers.Where(user => user.apiUserKey == ApiKey && user.apiUserValue == ApiValue && ((currentDate >= user.dateFrom && currentDate <= user.dateTo) || (currentDate >= user.dateFrom && user.dateTo == null))).Count() > 0)
            {
                var apiUsers = db.apiUsers.FirstOrDefault(user => user.apiUserKey == ApiKey && user.apiUserValue == ApiValue && ((currentDate >= user.dateFrom && currentDate <= user.dateTo) || (currentDate >= user.dateFrom && user.dateTo == null)));
                if (apiUsers != null)
                    inApiUserCode = apiUsers.apiUserCode;
                if (inApiUserCode > 0)
                    foundUser = true;
            }

            apiUserRequest oApiUserRequest = new apiUserRequest();
            if (inApiUserCode != null)
                oApiUserRequest.apiUserCode = inApiUserCode;
            else
            {
                oApiUserRequest.apiUserKey = ApiKey;
                oApiUserRequest.apiUserValue = ApiValue;
            }

            oApiUserRequest.apiUserRequestURL = httpRequestMessage.RequestUri.ToString();
            oApiUserRequest.apiUserRequestIP = ipAddress;
            oApiUserRequest.apiUserRequestDate = DateTime.Now;
            db.apiUserRequests.Add(oApiUserRequest);

            if (db.SaveChanges() > 0 && foundUser)
                return true;

            return false;
        }

    }
    public class Results
    {
        public string result { get; set; }
        public bool isTrue { get; set; }
    }
    public class ContProcessesModel
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        public int iProcessCode { get; set; }
        public string sProcessName { get; set; }


        /// <summary>
        ///   Get Contractor Processes.
        /// </summary>
        /// <param name="userCode"> User Code. </param>
        /// <returns> List Of Contractor Processes Model. </returns>
        public List<ContProcessesModel> GetContractorProcesses(int userCode)
        {
            var lEf = db.spGetContractorProcesses(userCode).ToList();

            List<ContProcessesModel> LContProcessesModel = new List<ContProcessesModel>();
            if (lEf.Count > 0)
            {
                foreach (var item in lEf)
                {
                    ContProcessesModel oContProcessModel = new ContProcessesModel();
                    oContProcessModel.iProcessCode = item.processCode;
                    oContProcessModel.sProcessName = item.processName;

                    LContProcessesModel.Add(oContProcessModel);
                }
            }
            return LContProcessesModel;
        }

    }

    public class WorkersAttendance
    {
        public List<WAttendanceModel> lWorkerAttendance { get; set; }
        public WAttendanceModel oWorkerAttendance { get; set; }
    }
    public class WAttendanceModel
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        public int iWorkerCode { get; set; }
        public int iCardID { get; set; }
        public int iProcessCode { get; set; }
        public int iUserCode { get; set; }
        public string sQR { get; set; }
        public string sWorkerName { get; set; }
        public Nullable<DateTime> dtAttendanceDate { get; set; }
        public Nullable<DateTime> dtFromAttendanceDate { get; set; }
        public Nullable<DateTime> dtToAttendanceDate { get; set; }
        public readonly nationalIDData oNationalIDData = new nationalIDData();
        public readonly coverData oCoverData = new coverData();


        /// <summary>
        ///   Search In Workers Attendance By Process Code , Date From Attend And Date End Attend.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="dateFrom"> Date From Attend. </param>
        /// <param name="toFrom"> Date To Attend. </param>
        /// <returns> List Of Workers Attendance Model. </returns>
        public List<WAttendanceModel> GetWorkerAttendansSearch(int processCode, DateTime? dateFrom, DateTime? toFrom)
        {
            var lwAttendance = db.spAPIWorkers(processCode, dateFrom, toFrom).ToList();
            List<WAttendanceModel> lwAttendanceModel = new List<WAttendanceModel>();

            if (lwAttendance.Count > 0)
            {
                foreach (var workerAttendance in lwAttendance)
                {
                    WAttendanceModel oWAttendance = new WAttendanceModel();
                    oWAttendance.iWorkerCode = workerAttendance.workerCode;
                    oWAttendance.iProcessCode = workerAttendance.processCode;
                    oWAttendance.sWorkerName = workerAttendance.workerName;
                    oWAttendance.sQR = Convert.ToDateTime(workerAttendance.dateAttendence).Date.ToString("MM/dd/yyyy") + " " + workerAttendance.workerName;
                    oWAttendance.dtAttendanceDate = workerAttendance.dateAttendence;
                    oWAttendance.dtFromAttendanceDate = dateFrom;
                    oWAttendance.dtToAttendanceDate = toFrom;

                    lwAttendanceModel.Add(oWAttendance);
                }
            }

            return lwAttendanceModel;
        }


        /// <summary>
        ///   List Of Workers Attendance Will Be Attend In Process. 
        /// </summary>
        /// <param name="lwAttendance"> List OF Workers Attendance. </param>
        /// <returns> Object Of Results. </returns>
        public Results PostWorkerAttendans(List<WAttendanceModel> lwAttendance)
        {
            try
            {
                string attendanceFaild = "";
                Results res = new Results();
                res.result = attendanceFaild;

                if (lwAttendance.Count > 0)
                {
                    string sQRs = "", dTime = "";
                    int processCode = lwAttendance[0].iProcessCode;
                    int userCode = lwAttendance[0].iUserCode;

                    string day = lwAttendance[0].dtAttendanceDate.Value.Day.ToString();
                    string month = lwAttendance[0].dtAttendanceDate.Value.Month.ToString();
                    day = day.Length == 1 ? "0" + day : day;
                    month = month.Length == 1 ? "0" + month : month;

                    // Check If This QR NAtionalID Or Card هلف على عامل عامل متحضر عن طريق الكارت ولا الرقم القومى
                    foreach (var workerAttendance in lwAttendance)
                    {
                        var data = "";
                        string worker_data = "";
                        StringBuilder y = new StringBuilder();
                        try
                        {
                            // National ID فى حاله انه متحضر عن طريق الرقم قومى  
                            if (Regex.IsMatch(workerAttendance.sQR, @"^\d+$") && workerAttendance.sQR.Length == 14)
                            {
                                /// هنا عن طريق الويسدل هشوف الرقم القومى في التأمينات لو في بيانات هناك هجيبها هعمل ابديت
                                /// بيها او لو مش موجود عندى هدخله عن طريق الاستورد الى عندى 

                                worker_data = oNationalIDData.nat_data(workerAttendance.sQR); // National ID الرقم القومى

                                #region Worker Update Data تحديث بيانات العامل
                                if ((!worker_data.Contains("ER") && !worker_data.Contains("Error")))
                                {
                                    string[] allWorkerData = worker_data.Split('#');
                                    string[] cover_data = oCoverData.cover_data(allWorkerData[1]);
                                    for (int i = 0; i < cover_data.Length; i++)
                                    {
                                        y.Append(cover_data[i] + @"\");
                                    }
                                    db.worker_data_update_ims(y.ToString(), userCode);
                                }
                                #endregion

                                #region هنا هخزن نتيجه الويزدل الجايه من التأمينات
                                string[] workerDataSplit = worker_data.Split('#');
                                if (workerDataSplit.Length == 1) // Error
                                    db.spUpdateResponseInsuranceWsdlWorker(workerAttendance.sQR, worker_data, worker_data, DateTime.Now, userCode);
                                else if (worker_data.Contains("**M1#")) // جايب داتا
                                    db.spUpdateResponseInsuranceWsdlWorker(workerAttendance.sQR, worker_data, null, DateTime.Now, userCode);
                                else // Err-National OR Any Error
                                    db.spUpdateResponseInsuranceWsdlWorker(workerAttendance.sQR, worker_data, workerDataSplit[3], DateTime.Now, userCode);
                                #endregion


                                #region هنا فيه ايرور في الويسدل
                                if (y.ToString().Contains("OPEN") || y.ToString().Contains("GOV"))
                                {
                                    string natID = workerAttendance.sQR;
                                    worker Worker = db.workers.Where(x => x.workerNationalID == natID).FirstOrDefault();

                                    if (Worker != null)
                                    {
                                        Worker.insWsdlResponse = String.IsNullOrEmpty(y.ToString()) ? "Error" : y.ToString();
                                        Worker.insWsdlDate = DateTime.Now;
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    string natID = workerAttendance.sQR;
                                    worker Worker = db.workers.Where(x => x.workerNationalID == natID).FirstOrDefault();

                                    if (Worker != null)
                                    {
                                        Worker.insWsdlResponse = null;
                                        Worker.insWsdlDate = null;
                                        db.SaveChanges();
                                    }
                                }

                                if (y.ToString().Contains("GOV"))
                                {
                                    data = "GOV";

                                    string[] workerName = y.ToString().Split('#');
                                    res.result = String.IsNullOrEmpty(res.result) ? "عفوا , العامل " + workerName[12] + " ليس مسجل بقطاع 4 . \n\r " : res.result + "عفوا , العامل " + workerName[12] + " ليس مسجل بقطاع 4 . \n\r ";
                                }

                                #endregion

                            }
                        }
                        catch
                        {
                            data = "Connection Not Found";
                        }

                        if (data == "") // كارت او رقم قومى
                        {
                            sQRs = String.IsNullOrEmpty(sQRs) ? workerAttendance.sQR : (sQRs + "$" + workerAttendance.sQR);
                            dTime = String.IsNullOrEmpty(dTime) ? Convert.ToDateTime(workerAttendance.dtAttendanceDate).ToString() : (dTime + "$" + Convert.ToDateTime(workerAttendance.dtAttendanceDate).ToString());
                        }
                    }

                    if (!String.IsNullOrEmpty(sQRs))
                    {
                        var resultAttendance = db.spWorkerAttendanceApp(processCode, userCode, null, sQRs, dTime).FirstOrDefault();

                        #region First Attend In Month WSDL ويزدل اول تحضير للعامل بالشهر
                        if (resultAttendance != null && (!String.IsNullOrEmpty(resultAttendance.workersNationalIdAttended)))
                        {

                            string curDay = DateTime.Now.Day.ToString();
                            string curMonth = DateTime.Now.Month.ToString();
                            curDay = curDay.Length == 1 ? "0" + curDay : curDay;
                            curMonth = curMonth.Length == 1 ? "0" + curMonth : curMonth;

                        }
                        #endregion
                        if (resultAttendance != null)
                        {
                            resultAttendance.resutAttendance = string.IsNullOrEmpty(resultAttendance.resutAttendance) ? "" : resultAttendance.resutAttendance;
                            res.result = !String.IsNullOrEmpty(res.result) ? (res.result + (resultAttendance.resutAttendance)) : resultAttendance.resutAttendance;
                        }
                    }
                }
                else
                    res.result = "برجاء ادخال العمال لكى يتم التحضير بنجاح.";

                return res;
            }
            catch (Exception ex)
            {
                Results res = new Results();
                res.result = ex.InnerException.Message;
                return res;
            }
        }

    }
}