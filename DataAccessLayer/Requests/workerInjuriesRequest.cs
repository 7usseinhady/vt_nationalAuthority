using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class WorkerInjuriesRequest : RequestBase<WorkerInjuriesModel>
    {
        // workerInjuries اصابات العامل
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        public WorkerModel oWorkerModel { get; set; }

        /// <summary>
        /// Get Method
        /// </summary>
        public override void GetInit()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get One Worker Injuries Then Get Worker Data.
        /// </summary>
        /// <param name="Id">Worker Injuries Code</param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new WorkerInjuriesModel().GetById(Id);
            GetWorkerData(Id);
        }
        /// <summary>
        /// Get List Of Worker Injuries Then Get Worker Data.
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public override void GetList(string Id)
        {
            this.LModels = new WorkerInjuriesModel().GetAll(Convert.ToInt32(Id));
            GetWorkerData(Convert.ToInt32(Id));
        }
        /// <summary>
        /// Get Worker Information
        /// </summary>
        /// <param name="Id">Worker Code</param>
        public void GetWorkerData(int Id)
        {
            oWorkerModel = new WorkerModel();
            oWorkerModel.vGetMainWorkerData(Id);
            this.oWorkerModel = oWorkerModel;
        }
        /// <summary>
        /// Search For Worker Injuries Then Get Worker Data.
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new WorkerInjuriesModel().lSearch(searchObjs);
            GetWorkerData(Convert.ToInt32(searchObjs[0]));
        }
        /// <summary>
        /// Save Worker Injuries.
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public override void vSave(WorkerInjuriesModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new WorkerInjuriesModel();
            if (this.OModel.bSave(newObj))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;

            GetList(newObj.iWorkerCode.ToString());
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(WorkerInjuriesModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Worker Injuries.
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Worker Injurie Code</param>
        public override void vEdit(WorkerInjuriesModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new WorkerInjuriesModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            GetList(newObj.iWorkerCode.ToString());
        }
        /// <summary>
        /// Delete Method
        /// </summary>
        /// <param name="Id"></param>
        public override void vDelete(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Worker Injuries
        /// </summary>
        /// <param name="IdObj">Worker Injurie Code</param>
        /// <param name="id">Worker Code</param>
        public override void vDelete(int IdObj, string id)
        {
            this.OModel = new WorkerInjuriesModel();

            if (this.OModel.bDelete(IdObj))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetList(id);
        }
    }
}
