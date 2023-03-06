using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class WorkerRequest : RequestBase<WorkerModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        public List<WorkerContactModel> LworkerContactModel = new List<WorkerContactModel>();

        public List<WorkerAttendanceModel> LworkerAttendanceModel = new List<WorkerAttendanceModel>();


        public MedicalInsuranceModel OmedicalInsuranceModel = new MedicalInsuranceModel();


        public WorkerContactModel OworkerContactModel = new WorkerContactModel();


        public ManPowerModel OmanPowerModel = new ManPowerModel();

        public List<ProcessModel> LprocessModel = new List<ProcessModel>();



        public List<CardWorkerStopActiveModel> LCardsWorkModel = new List<CardWorkerStopActiveModel>();

        public CardWorkerStopActiveModel OcardsWorkModel { get; set; }
        /// <summary>
        /// Get Method
        /// </summary>
        public override void GetInit()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get One Worker
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new WorkerModel().GetById(Id);
        }
        /// <summary>
        /// Get Workers In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        public override void GetList(string Id)
        {
            this.LModels = new WorkerModel().GetAll(Convert.ToInt32(Id));
        }
        /// <summary>
        /// Get List Of Workers 
        /// </summary>
        /// <param name="lstr">List Of Data Need For Filter Workers</param>
        public void GetList(List<string> lstr)
        {
            this.LModels = new WorkerModel().GetAll(lstr);
        }
        /// <summary>
        /// Get List Of Workers 
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <param name="areas">Areas Codes</param>
        /// <param name="Offices">Offices Codes</param>
        /// <param name="UserCode">User Code</param>
        /// <param name="workerCode">Worker Code</param>
        public void GetList(string Id, string areas, string Offices, string UserCode, string workerCode)
        {
            this.LModels = new WorkerModel().GetAll(Convert.ToInt32(Id), areas, Offices, Convert.ToInt32(UserCode), workerCode);
        }
        /// <summary>
        /// Delete Worker 
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public override void vDelete(int Id)
        {
            this.OModel = new WorkerModel();

            if (this.OModel.bDelete(Id))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetList("0");
        }
        /// <summary>
        /// Delete Worker
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <param name="UserCode">User Code Is Deleted</param>
        public void vDelete(int Id, int UserCode)
        {
            this.OModel = new WorkerModel();

            if (this.OModel.bDelete(Id, UserCode))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetList("0");
        }
        /// <summary>
        /// Deleting Method
        /// </summary>
        /// <param name="IdObj"></param>
        /// <param name="id"></param>
        public override void vDelete(int IdObj, string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Editing Method
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vEdit(WorkerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj"></param>
        public override void vSave(WorkerModel newObj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(WorkerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Workers
        /// </summary>
        /// <param name="searchObjs">List Of Data Need For Search</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new WorkerModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Searching Method
        /// </summary>
        /// <param name="SearchObj"></param>
        public void vSearchList(List<string> SearchObj)
        {
            throw new NotImplementedException();
        }
        #region Phone
        /// <summary>
        /// Get Phones For Worker
        /// </summary>
        /// <param name="id">Worker Code</param>
        public void GetPhones(int id)
        {
            this.LworkerContactModel = new WorkerContactModel().GetAll(id);
        }
        /// <summary>
        /// Delete Phone For Worker
        /// </summary>
        /// <param name="id">Phone Code</param>
        /// <param name="workerCode">Worker Code</param>
        public void DeletePhone(int id, int workerCode)
        {
            this.LworkerContactModel = new WorkerContactModel().GetAll(id);

            if (this.OworkerContactModel.bDelete(id))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetPhones(workerCode);
        }
        /// <summary>
        /// Save Phone For Worker
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public void SavePhones(WorkerContactModel newObj)
        {

            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OworkerContactModel = new WorkerContactModel();
            if (this.OworkerContactModel.bSave(newObj))
                bIsSaved = true;
            else
                bIsSaved = false;
        }
        #endregion
        #region attendance
        /// <summary>
        /// Search For Worker Attendance
        /// </summary>
        /// <param name="searchObjs">List Of  Data Need For Search</param>
        public void vSearchAttendance(List<string> searchObjs)
        {
            this.LworkerAttendanceModel = new WorkerAttendanceModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Get All Worker Attendance
        /// </summary>
        /// <param name="searchObjs">List Of Data Need To Get Worker Attendance</param>
        public void vGetAttendance(List<string> searchObjs)
        {
            this.LworkerAttendanceModel = new WorkerAttendanceModel().lGetAttendance(searchObjs);
        }
        #endregion
        #region medicla insurance 
        /// <summary>
        /// Save Worker Medical Insurance 
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public void SaveMI(MedicalInsuranceModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OmedicalInsuranceModel = new MedicalInsuranceModel();
            if (this.OmedicalInsuranceModel.bSave(newObj))
                bIsSaved = true;
            else
                bIsSaved = false;
        }

        #endregion
        #region Man Power
        /// <summary>
        /// Save Worker Man-Power
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public void SaveManPower(ManPowerModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OmanPowerModel = new ManPowerModel();
            if (this.OmanPowerModel.bSave(newObj))
                bIsSaved = true;
            else
                bIsSaved = false;
        }
        #endregion
        #region Worker Process
        /// <summary>
        /// Get Worker Process
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public void GetWorkerProcess(string Id)
        {
            this.LprocessModel = new ProcessModel().GetWorkerProcess(Id);
        }
        #endregion
        #region cards work stop active
        /// <summary>
        /// Get All Worker Cards Status
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public void GetCardStopActive(string Id)
        {
            this.LCardsWorkModel = new CardWorkerStopActiveModel().GetAll(Convert.ToInt32(Id));
        }
        /// <summary>
        /// Get Worker Card Status
        /// </summary>
        /// <param name="Id">Worker Card Status Code</param>
        public void GetObjCardStatus(string Id)
        {
            this.OcardsWorkModel = new CardWorkerStopActiveModel().GetById(Convert.ToInt32(Id));
        }
        /// <summary>
        /// Save Worker Card Status Code
        /// </summary>
        /// <param name="newObj">Data Need For Save Worker Card Status</param>
        public void SaveCardStatus(CardWorkerStopActiveModel newObj)
        {
            newObj.sIpInsert = this.sIpAddress;
            this.OcardsWorkModel = new CardWorkerStopActiveModel();
            if (this.OcardsWorkModel.bSave(newObj))
                bIsSaved = true;
            else
                bIsSaved = false;
            GetCardStopActive(newObj.iWorkerCode.ToString());
        }
        /// <summary>
        /// Edit Worker Card Status Code
        /// </summary>
        /// <param name="newObj">Data Need For Edit Worker Card Status</param>
        /// <param name="id">Worker Card Status Code</param>
        public void EditCardStatus(CardWorkerStopActiveModel newObj, int id)
        {
            newObj.sIpUpdate = this.sIpAddress;
            this.OcardsWorkModel = new CardWorkerStopActiveModel();
            if (this.OcardsWorkModel.bEdit(newObj, id))
                bIsEdit = true;
            else
                bIsEdit = false;

            GetCardStopActive(newObj.iWorkerCode.ToString());
        }
        /// <summary>
        /// Search For Worker Cards Status
        /// </summary>
        /// <param name="lstr">List Of Data Need For Search</param>
        public void SearchCardStatus(List<string> lstr)
        {
            this.LCardsWorkModel = new CardWorkerStopActiveModel().lSearch(lstr);
        }
        #endregion
    }
}
