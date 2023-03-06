using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Process Stop. 
    /// </summary>
    public class ProcessStopRequest : RequestBase<ProcessStopModel>
    {
        // processStop ايقاف العمليه
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        public ProcessModel oProcessModel { get; set; }

        /// <summary>
        ///   Get All Reasons Of Stopping Process And Get Main Process Data.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public override void GetList(string Id)
        {
            this.LModels = new ProcessStopModel().GetAll(Convert.ToInt32(Id));
            GetProcessData(Convert.ToInt32(Id));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void GetInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get Object Of Reason Of Stopping Process And Get Main Process Data.
        /// </summary>
        /// <param name="Id"> Reason Of Stopping Process Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new ProcessStopModel().GetById(Id);
            GetProcessData(Id);
        }

        /// <summary>
        ///   Get Last Object From Reasons Of Stopping Process For Validation.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public void GetProcessStopPrev(int Id)
        {
            this.OModel = new ProcessStopModel().GetProcessStopPrevById(Id);
        }
      
        /// <summary>
        ///   Get Main Process Data.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public void GetProcessData(int Id)
        {
            oProcessModel = new ProcessModel();
            oProcessModel.vGetMainProcessData(Id.ToString(), null);
            this.oProcessModel = oProcessModel;
        }


        /// <summary>
        ///   Search With Special Parameters And Get Main Process Data.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new ProcessStopModel().lSearch(searchObjs);
            GetProcessData(Convert.ToInt32(searchObjs[0]));
        }


        /// <summary>
        ///   Save Object Data In Database Then Get All Reasons Of Stopping Process.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(ProcessStopModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new ProcessStopModel();
            if (this.OModel.bSave(newObj))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;

            GetList(newObj.iProcessCode.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(ProcessStopModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database Then Get All Reasons Of Stopping Process.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Reason Of Stopping Procss Will Be Edit. </param>
        public override void vEdit(ProcessStopModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new ProcessStopModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            GetList(newObj.iProcessCode.ToString());
        }


        /// <summary>
        ///   Delete Special Reason Of Stopping Procss Then Get All Reasons Of Stopping Process.
        /// </summary>
        /// <param name="Id"> Code Of Reason Of Stopping Procss Will Be Delete. </param>
        public override void vDelete(int Id)
        {
            this.OModel = new ProcessStopModel();

            if (this.OModel.bDelete(Id))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetInit();
        }

        /// <summary>
        ///   Delete Special Reason Of Stopping Procss Then Get All Reasons Of Stopping Process
        /// </summary>
        /// <param name="IdObj"> Code Of Reason Of Stopping Procss Will Be Delete. </param>
        /// <param name="id"> Process Code. </param>
        public override void vDelete(int IdObj, string id)
        {
            this.OModel = new ProcessStopModel();

            if (this.OModel.bDelete(IdObj))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetList(id);
        }

    }
}
