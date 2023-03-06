using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Document Types.
    /// </summary>
    public class DocumentTypeRequest : RequestBase<DocumentTypeModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();

         
        /// <summary>
        ///   Get All Document Types.
        /// </summary>
        public override void GetInit()
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.OModel = new DocumentTypeModel();
            this.LModels = new DocumentTypeModel().GetAll();
        }

        /// <summary>
        ///   Get Object Of Document Type.
        /// </summary>
        /// <param name="Id"> Document Type Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new DocumentTypeModel().GetById(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public override void GetList(string Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Search With Special Parameters And Get Main Process Data.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new DocumentTypeModel().lSearch(searchObjs);
        }


        /// <summary>
        ///   Save Object Data In Database Then Get All Document Types.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(DocumentTypeModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new DocumentTypeModel();
            if (this.OModel.bSave(newObj))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;

            GetInit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(DocumentTypeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database Then Get All Document Types.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Document Type Will Be Edit. </param>
        public override void vEdit(DocumentTypeModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new DocumentTypeModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            GetInit();
        }


        /// <summary>
        ///   Delete Special Document Type Then Get All Document Types.
        /// </summary>
        /// <param name="Id"> Code Of Document Type Will Be Delete. </param>
        public override void vDelete(int Id)
        {
            this.OModel = new DocumentTypeModel();

            if (this.OModel.bDelete(Id))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetInit();
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
    }
}
