using apiWSDLs.wsdls;
using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class WorkerModel : ModelBase<WorkerModel, worker>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        private readonly firstAttInMonthWsdl ofirstAttInMonthWsdl = new firstAttInMonthWsdl();

        #region details
        public Nullable<int> iWorkerCode { get; set; }
        public string sWorkerName { get; set; }
        public string sWorkerNationalID { get; set; }
        public string sWorkerInsuranceNum { get; set; }
        public Nullable<int> iTypeAttendence { get; set; }
        public string sLastAttendance { get; set; }
        public string sLastPeriod { get; set; }
        public Nullable<int> iCareerCode { get; set; }
        public string sCareerName { get; set; }
        public string sSkillDegreeName { get; set; }
        public Nullable<bool> bIsActive { get; set; }
        public string sGoverName { get; set; }
        public string sVillage { get; set; }
        public string sCenter { get; set; }
        public string sAddress { get; set; }
        public string sMedicalStatusName { get; set; }
        public string[] lsWorkerAttendanceWSDL { get; set; }
        #endregion
        /// <summary>
        /// Delete Worker 
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                worker Oworker = db.workers.FirstOrDefault(x => x.workerCode == Id);
                if (Oworker != null)
                {
                    Oworker.isDelete = true; // كود العملية
                    if (db.SaveChanges() > 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Delete Worker And De-Active His Card
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <param name="userCode">User Code Is Deleted</param>
        /// <returns>Delete Done Or Not</returns>
        public bool bDelete(int Id, int userCode)
        {
            try
            {
                worker Oworker = db.workers.FirstOrDefault(x => x.workerCode == Id);
                if (Oworker != null)
                {
                    Oworker.isDelete = true; // كود العملية
                    Oworker.userCodeDelete = userCode;
                    Oworker.dateDelete = dtServerTime;
                    if (db.SaveChanges() > 0)
                    {
                        var cardCode = db.workerDetails.FirstOrDefault(x => x.workerCode == Id);
                        if (cardCode != null)
                        {
                            card oCard = db.cards.FirstOrDefault(x => x.cardCode == cardCode.cardCode);
                            if (oCard != null)
                            {
                                oCard.isActive = false;
                                db.SaveChanges();
                            }
                        }
                        return true;
                    }

                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        internal override bool bEdit(WorkerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(WorkerModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(WorkerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerModel> ConvertEFsToObjects(List<worker> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerModel> ConvertEFsToObjectsBasic(List<worker> lEf)
        {
            throw new NotImplementedException();
        }

        internal override WorkerModel ConvertEFToObject(worker ef)
        {
            throw new NotImplementedException();
        }

        internal override WorkerModel ConvertEFToObjectBasic(worker lEf)
        {
            throw new NotImplementedException();
        }

        internal override worker ConvertObjectToEF(WorkerModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Workers In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <returns>List Of Workers</returns>
        internal override List<WorkerModel> GetAll(int Id)
        {
            try
            {
                int? id = null;
                if (Id != 0)
                    id = Id;
                var models = db.GetWorkerDetails(id, null, null, null, null, null).ToList();
                List<WorkerModel> LworkerModel = new List<WorkerModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerModel OworkerModellEF = new WorkerModel();
                        OworkerModellEF.iWorkerCode = item.workerCode;
                        OworkerModellEF.sWorkerName = item.workerName;
                        OworkerModellEF.sWorkerNationalID = item.workerNationalID;
                        OworkerModellEF.sWorkerInsuranceNum = item.workerInsuranceNum;
                        OworkerModellEF.iTypeAttendence = item.typeAttendence;
                        OworkerModellEF.sLastAttendance = item.LastAttendance;
                        OworkerModellEF.sLastPeriod = item.LastPeriod;
                        OworkerModellEF.iCareerCode = item.careerCode;
                        OworkerModellEF.sCareerName = item.careerName;
                        OworkerModellEF.sSkillDegreeName = item.skillDegreeName;
                        OworkerModellEF.bIsActive = item.isActive;
                        OworkerModellEF.sMedicalStatusName = item.medicalStatusName;
                        LworkerModel.Add(OworkerModellEF);
                    }
                }

                return LworkerModel;
            }
            catch
            {
                return new List<WorkerModel>();
            }
        }
        /// <summary>
        /// Get List Of Workers 
        /// </summary>
        /// <param name="lstr">Data Need For Filters Workers</param>
        /// <returns>List Of Workers</returns>
        public List<WorkerModel> GetAll(List<string> lstr)
        {
            try
            {
                List<GetWorkerDetails_Result> models;
                if (String.IsNullOrEmpty(lstr[3]))
                    models = db.GetWorkerDetails(Convert.ToInt32(lstr[0]), null, null, lstr[1], Convert.ToInt32(lstr[2]), null).ToList();
                else
                    models = db.GetWorkerDetails(Convert.ToInt32(lstr[0]), null, null, lstr[1], Convert.ToInt32(lstr[2]), Convert.ToInt32(lstr[3])).ToList();

                List<WorkerModel> LworkerModel = new List<WorkerModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerModel OworkerModellEF = new WorkerModel();
                        OworkerModellEF.iWorkerCode = item.workerCode;
                        OworkerModellEF.sWorkerName = item.workerName;
                        OworkerModellEF.sWorkerNationalID = item.workerNationalID;
                        OworkerModellEF.sWorkerInsuranceNum = item.workerInsuranceNum;
                        OworkerModellEF.iTypeAttendence = item.typeAttendence;
                        OworkerModellEF.sLastAttendance = item.LastAttendance;
                        OworkerModellEF.sLastPeriod = item.LastPeriod;
                        OworkerModellEF.iCareerCode = item.careerCode;
                        OworkerModellEF.sCareerName = item.careerName;
                        OworkerModellEF.sSkillDegreeName = item.skillDegreeName;
                        OworkerModellEF.bIsActive = item.isActive;
                        OworkerModellEF.sMedicalStatusName = item.medicalStatusName;
                        LworkerModel.Add(OworkerModellEF);
                    }
                }

                return LworkerModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get List Of Workers
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <param name="areas">Areas Codes</param>
        /// <param name="Offices">Offices Codes</param>
        /// <param name="UserCode">User Code</param>
        /// <param name="workerCode">Worker Code</param>
        /// <returns>List Of Workers</returns>
        public List<WorkerModel> GetAll(int Id, string areas, string Offices, int UserCode, string workerCode)
        {
            try
            {
                int? id = null;
                if (Id != 0)
                    id = Id;
                var models = new List<GetWorkerDetails_Result>();
                if (String.IsNullOrEmpty(workerCode))
                    models = db.GetWorkerDetails(id, Offices == "" ? null : Offices, areas == "" ? null : areas, null, UserCode, null).ToList();
                else
                    models = db.GetWorkerDetails(id, Offices == "" ? null : Offices, areas == "" ? null : areas, null, UserCode, Convert.ToInt32(workerCode)).ToList();

                List<WorkerModel> LworkerModel = new List<WorkerModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerModel OworkerModellEF = new WorkerModel();

                        OworkerModellEF.iWorkerCode = item.workerCode;
                        OworkerModellEF.sWorkerName = item.workerName;
                        OworkerModellEF.sWorkerNationalID = item.workerNationalID;
                        OworkerModellEF.sWorkerInsuranceNum = item.workerInsuranceNum;
                        OworkerModellEF.iTypeAttendence = item.typeAttendence;
                        OworkerModellEF.sLastAttendance = item.LastAttendance;
                        OworkerModellEF.sLastPeriod = item.LastPeriod;
                        OworkerModellEF.iCareerCode = item.careerCode;
                        OworkerModellEF.sCareerName = item.careerName;
                        OworkerModellEF.sSkillDegreeName = item.skillDegreeName;
                        OworkerModellEF.bIsActive = item.isActive;
                        OworkerModellEF.sMedicalStatusName = item.medicalStatusName;
                        LworkerModel.Add(OworkerModellEF);
                    }
                }

                return LworkerModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get One Worker
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <returns>Worker Data</returns>
        internal override WorkerModel GetById(int Id)
        {
            WorkerModel OworkerModel = new WorkerModel();
            List<GetOneWork_Result> model = db.GetOneWork(Id).ToList();
            OworkerModel.iWorkerCode = model[0].workerCode;
            OworkerModel.sWorkerName = model[0].workerName;  // اسم العامل
            OworkerModel.sWorkerNationalID = model[0].workerNationalID; // الرقم القومى
            OworkerModel.sWorkerInsuranceNum = model[0].workerInsuranceNum; // الرقم التأمينى

            // المهنه
            OworkerModel.sCareerName = model[0].careerName;
            OworkerModel.sAddress = model[0].govermentName + " , " + model[0].careerName + " , " + model[0].villageName;

            return OworkerModel;
        }
        /// <summary>
        /// Search For Workers 
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Workers </returns>
        internal override List<WorkerModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetWorkers(searchObjs[0], searchObjs[2], searchObjs[1], searchObjs[3], searchObjs[4], searchObjs[7], searchObjs[6], searchObjs[5], Convert.ToInt32(searchObjs[8])).ToList();
                List<WorkerModel> LworkerModel = new List<WorkerModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerModel OworkerModellEF = new WorkerModel();
                        OworkerModellEF.iWorkerCode = (int)item.workerCode;
                        OworkerModellEF.sWorkerName = item.workerName;
                        OworkerModellEF.sWorkerNationalID = item.workerNationalID;
                        OworkerModellEF.sWorkerInsuranceNum = item.workerInsuranceNum;
                        OworkerModellEF.iTypeAttendence = item.typeAttendence;
                        OworkerModellEF.sLastAttendance = item.LastAttendance;
                        OworkerModellEF.sLastPeriod = item.LastPeriod;
                        OworkerModellEF.iCareerCode = item.careerCode;
                        OworkerModellEF.sCareerName = item.careerName;
                        OworkerModellEF.sSkillDegreeName = item.skillDegreeName;
                        OworkerModellEF.sMedicalStatusName = item.medicalStatusName;
                        LworkerModel.Add(OworkerModellEF);
                    }
                }

                return LworkerModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        ///   Get Main Worker Data.
        /// </summary>
        /// <param name="workerCode"> Worker Code. </param>
        public void vGetMainWorkerData(int? workerCode)
        {
            int wCode = Convert.ToInt32(workerCode);
            var modal = db.spGetMainWorkerData(wCode).FirstOrDefault();

            if (modal != null)
            {
                this.iWorkerCode = modal.workerCode; // كود العامل
                this.sWorkerName = modal.workerName; // اسم العامل
                this.sWorkerNationalID = modal.workerNationalID; // الرقم القومى للعامل
                this.sWorkerInsuranceNum = modal.workerInsuranceNum; // الرقم التأمينى
            }
        }

        #region WSDLs 

        /// <summary>
        /// اول حضور للعامل في الشهر "تغطيه" للتأمينات
        /// </summary>
        /// <param name="firstTypeOf">First Type Of 1.Attendance , 2.MedicalInsurance , 3.ManPower , 4.Empty  حضور 1 - تأمين صحى 2 - قوى عامله 3 - فاضى 4</param>
        /// <param name="processNumber">Process Number رقم العمليه</param>
        /// <param name="dateAttend">Date Attend For Attendance , MedicalInsurance , ManPower  تاريخ حضور - تأمين صحى - قوى عامله.</param>
        /// <param name="contInsuranceNumber">Contractor Insurance Number رقم المقاول بالعمليه</param>
        /// <param name="contLegalEntityCode">Legal Contractor 1.Individual , 2.Corporation  كيان المنشأه 1. فرد 2.منشأه</param>
        /// <param name="workersNationalIdAttended">Workers National ID  الارقام القوميه للعمال للعامل</param>
        /// <param name="workersCareerIDAttended">Workers Career ID ارقام المهن للعمال</param>
        /// <param name="workersAttendanceCodes">Workers Attendance Codes اكواد التحضير للعمال</param>
        public string[] saveFirstAttInMonthInWsdl(long processNumber, int dateAttend, int contInsuranceNumber, string contLegalEntityCode,
            string workersNationalIdAttended, string workersCareerIDAttended, string workersAttendanceCodes)
        {
            string[] ReturnStatus = new string[] { "0", "0" };
            try
            {
                string[] workerAttendedNationalIDs = workersNationalIdAttended.Split(','); // Workers National ID الارقام القوميه للعمال
                string[] workerAttendanceCodes = workersAttendanceCodes.Split(','); // Workers Attendance Codes اكواد التحضير للعمال

                string wAttendanceCodes = "", statusCodes = "", statusDesc = "", statusDate = "";

                // هنا بقي هنادى على الويسدل الى هبعتلها اول حضور للعامل في الشهر ده 
                for (int i = 0; i < workerAttendedNationalIDs.Length; i++)
                {
                    string procNum = processNumber + "00";
                    string[] resultSaveFirstAttInMonthInWsdl = ofirstAttInMonthWsdl.saveFirstAttInMonthInWsdl("1", workerAttendedNationalIDs[i], dateAttend.ToString(),
                        contLegalEntityCode, contInsuranceNumber.ToString(), procNum, "000000", "0", "000000", "00000000");

                    //Save Result In DataBase جزء خاص بتخزين النتيجه فى الداتا بيز
                    wAttendanceCodes = wAttendanceCodes == "" ? workerAttendanceCodes[i] : (wAttendanceCodes + "$" + workerAttendanceCodes[i]); // Worker Attecndance Codes اكواد حضور العامل
                    statusCodes = statusCodes == "" ? resultSaveFirstAttInMonthInWsdl[0] : (statusCodes + "$" + resultSaveFirstAttInMonthInWsdl[0]); // Status Code كود الحاله
                    statusDesc = statusDesc == "" ? resultSaveFirstAttInMonthInWsdl[1] : (statusDesc + "$" + resultSaveFirstAttInMonthInWsdl[1]); // Status Description وصف الحاله
                    statusDate = statusDate == "" ? DateTime.Now.ToString() : (statusDate + "$" + DateTime.Now.ToString()); // Status Description وصف الحاله
                }

                // Update Worker Attendance ابديت الحضور
                db.spUpdateWorkerAttendance(wAttendanceCodes, statusCodes, statusDesc, statusDate);

                return ReturnStatus;
            }
            catch
            {
                return ReturnStatus;
            }
        }

        #endregion

    }
}
