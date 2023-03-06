using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class WorkerAttendanceModel : ModelBase<WorkerAttendanceModel, workerAttendance>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        #region details
        public int iWorkerCode { get; set; }
        public string sLastAttendance { get; set; }
        public string sTime_ { get; set; }
        public string sCareerName { get; set; }
        public string sSkillDegreeName { get; set; }
        public string sWorkerName { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(WorkerAttendanceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(WorkerAttendanceModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(WorkerAttendanceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerAttendanceModel> ConvertEFsToObjects(List<workerAttendance> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerAttendanceModel> ConvertEFsToObjectsBasic(List<workerAttendance> lEf)
        {
            throw new NotImplementedException();
        }

        internal override WorkerAttendanceModel ConvertEFToObject(workerAttendance ef)
        {
            throw new NotImplementedException();
        }

        internal override WorkerAttendanceModel ConvertEFToObjectBasic(workerAttendance lEf)
        {
            throw new NotImplementedException();
        }

        internal override workerAttendance ConvertObjectToEF(WorkerAttendanceModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerAttendanceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerAttendanceModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        internal override WorkerAttendanceModel GetById(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Worker Attendance
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Worker Attendance</returns>
        internal override List<WorkerAttendanceModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetWorkerAttendance( Convert.ToInt32(searchObjs[0]) , Convert.ToInt32(searchObjs[1]),searchObjs[2],searchObjs[3],searchObjs[4],searchObjs[5] ).ToList();

                List<WorkerAttendanceModel> LworkerAttendanceModel = new List<WorkerAttendanceModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerAttendanceModel oWorkerAttendanceModel = new WorkerAttendanceModel();
                        oWorkerAttendanceModel.iWorkerCode = (int)item.workerCode;
                        oWorkerAttendanceModel.sLastAttendance = item.LastAttendance;
                        oWorkerAttendanceModel.sTime_ = item.Time_;
                        oWorkerAttendanceModel.sCareerName = item.careerName;
                        oWorkerAttendanceModel.sSkillDegreeName = item.skillDegreeName;
                        oWorkerAttendanceModel.sWorkerName = item.workerName;
                        LworkerAttendanceModel.Add(oWorkerAttendanceModel);
                    }
                }

                return LworkerAttendanceModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get Worker Attendance In Process
        /// </summary>
        /// <param name="SObj">Data Need For Get Attendance</param>
        /// <returns>List Of Worker Attendance</returns>
        public List<WorkerAttendanceModel> lGetAttendance(List<string> SObj)
        {
            try
            {
                var models = db.GetAttendance(Convert.ToInt32(SObj[0]),SObj[1], SObj[2]).ToList();

                List<WorkerAttendanceModel> LworkerAttendanceModel = new List<WorkerAttendanceModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        WorkerAttendanceModel oWorkerAttendanceModel = new WorkerAttendanceModel();
                        oWorkerAttendanceModel.iWorkerCode = (int)item.workerCode;
                        oWorkerAttendanceModel.sLastAttendance = item.LastAttendance.ToString();
                        oWorkerAttendanceModel.sTime_ = item.Time_;
                        oWorkerAttendanceModel.sCareerName = item.careerName;
                        oWorkerAttendanceModel.sSkillDegreeName = item.skillDegreeName;
                        oWorkerAttendanceModel.sWorkerName = item.workerName;
                        LworkerAttendanceModel.Add(oWorkerAttendanceModel);
                    }
                }

                return LworkerAttendanceModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
