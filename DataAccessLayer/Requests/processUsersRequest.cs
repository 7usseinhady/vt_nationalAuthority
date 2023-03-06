using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Process Users. 
    /// </summary>
    public class ProcessUsersRequest : RequestBase<ProcessUsersModel>
    {
        //  processUsers مستخدمين العمليه
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        public List<ProcessUsersModel> lProcsessUserModel { get; set; }
        public ProcessModel oProcessModel { get; set; }


        /// <summary>
        ///   Get All Process Users Then Get Main Process Data.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public override void GetList(string Id)
        {
            string[] lParam = Id.Split(',');
            this.LModels = new ProcessUsersModel().GetAll(Convert.ToInt32(lParam[0]), 1, Convert.ToInt32(lParam[1])); // المقاولين الرئيسين
            this.lProcsessUserModel = new ProcessUsersModel().GetAll(Convert.ToInt32(lParam[0]), 0, Convert.ToInt32(lParam[1])); // المقاولين من باطن
            GetProcessData(Convert.ToInt32(lParam[0]));
        }

        /// <summary>
        ///   Get Object Of Process User Then Get Main Process Data.
        /// </summary>
        /// <param name="Id"> Process User Code. </param>
        public void GetProcessData(int Id)
        {
            if (oProcessModel == null)
                oProcessModel = new ProcessModel();
            oProcessModel.vGetMainProcessData(Id.ToString(), null);
            this.oProcessModel = oProcessModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void GetInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public override void GetInitObject(int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchObjs"></param>
        public override void vSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///    Save Object Data In Database Then Get All Process Users.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(ProcessUsersModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new ProcessUsersModel();
            if (this.OModel.bSave(newObj))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;

            GetList(newObj.sIpUpdate.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(ProcessUsersModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vEdit(ProcessUsersModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public override void vDelete(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdObj"></param>
        /// <param name="id"></param>
        public override void vDelete(int IdObj, string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Delete Special Procss User Then Get All Process Users.
        /// </summary>
        /// <param name="IdObj"> Code Of Procss User Will Be Delete. </param>
        /// <param name="id"> Process Code. </param>
        public void vDelete(string IdObj, string id)
        {
            this.OModel = new ProcessUsersModel();

            if (this.OModel.bDelete(IdObj))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetList(id);
        }
    }
}
