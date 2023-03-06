using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Attachment Type. 
    /// </summary>
    public class AttachmentTypeRequest : RequestBase<AttachmentTypeModel>
    {
        // AttachmentType نوع المرفقات
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        /// <summary>
        ///   Get All Attachment Types.
        /// </summary>
        public override void GetInit()
        {
            this.LModels = new AttachmentTypeModel().GetAll();
        }

        /// <summary>
        ///   Get Object Of Attachment Type.
        /// </summary>
        /// <param name="Id"> Attachment Type Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new AttachmentTypeModel().GetById(Id);
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
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new AttachmentTypeModel().lSearch(searchObjs);
        }


        /// <summary>
        ///   Save Object Data In Database Then Get All Attachment types.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(AttachmentTypeModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new AttachmentTypeModel();
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
        public override void vSave(AttachmentTypeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database Then Get All Attachment Types.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Attachment Type Will Be Edit. </param>
        public override void vEdit(AttachmentTypeModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new AttachmentTypeModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            GetInit();
        }

        /// <summary>
        ///   Delete Special Attachment Type Then Get All Attachment Types.
        /// </summary>
        /// <param name="Id"> Code Of Attachment Type Will Be Delete. </param>
        public override void vDelete(int Id)
        {
            this.OModel = new AttachmentTypeModel();

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
