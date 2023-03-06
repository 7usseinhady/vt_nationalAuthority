using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Extracts. 
    /// </summary>
    public class ExtractsRequest : RequestBase<ExtractsModel>
    {
        // extracts المستخلصات
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        public ExtractsModel oExtractModel { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        public override void GetInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get Object Of Extract.
        /// </summary>
        /// <param name="Id"> Extract Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new ExtractsModel().GetById(Id);
        }

        /// <summary>
        ///   Get All Extracts For Special Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public override void GetList(string Id)
        {
            this.LModels = new ExtractsModel().GetAll(Convert.ToInt32(Id));
        }

        /// <summary>
        ///   Get All Extracts For Special Process Based On User.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="uc"> User Code. </param>
        public void GetList(string Id, string uc)
        {
            this.LModels = new ExtractsModel().GetAll(Convert.ToInt32(Id), uc);
        }


        /// <summary>
        ///   Search With Special Parameters And Get Main Process Data.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LModels = new ExtractsModel().lSearch(searchObjs);
        }


        /// <summary>
        ///   Save Object Data In Database Then Get All Extracts.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(ExtractsModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new ExtractsModel();
            if (this.OModel.bSave(newObj))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;

            // المقاول
            if (newObj.sIpUpdate == "contractor")
                GetList(newObj.iProcessCode.ToString(), newObj.inUserInsertCode.ToString());
            else if (newObj.sIpUpdate == "officeEmployee") // موظف التأمينات
                GetList(newObj.iProcessCode.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(ExtractsModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database Then Get All Extracts.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Extract Will Be Edit. </param>
        public override void vEdit(ExtractsModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new ExtractsModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            // المقاول
            if (newObj.sIpInsert == "contractor")
                GetList(newObj.iProcessCode.ToString(), newObj.inUserUpdateCode.ToString());
            else if (newObj.sIpInsert == "officeEmployee") // موظف التأمينات
                GetList(newObj.iProcessCode.ToString());
        }


        /// <summary>
        ///   Accept Extract.
        /// </summary>
        /// <param name="oExtractRequest"> New Request. </param>
        public void vAcceptExtract(ExtractsRequest oExtractRequest)// التأكيد على المستخلص 
        {
            this.OModel = new ExtractsModel();
            oExtractRequest.OModel.sIpInsert = generalMethod.vIPAddress(); // عنوان الجهاز اثناء الموافقه على العمليه

            if (this.OModel.bAcceptExtract(oExtractRequest.OModel))
                this.OModel.bIsSaved = true;
            else
                this.OModel.bIsSaved = false;
        }


        /// <summary>
        ///   Confirm Of Payment On Extract.
        /// </summary>
        /// <param name="oExtractRequest"> New Request. </param>
        public void vPaidExtract(ExtractsRequest oExtractRequest) // التأكيد تسديد الاشتراك على المستخلص
        {
            this.OModel = new ExtractsModel();
            oExtractRequest.OModel.sIpUpdate = generalMethod.vIPAddress(); // عنوان الجهاز اثناء التأكيد تسديد الاشتراك على العمليه

            if (this.OModel.bPaidExtract(oExtractRequest.OModel))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;
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
        ///   Delete Special Extract Then Get All Extracts.
        /// </summary>
        /// <param name="IdObj"> Extract Code. </param>
        /// <param name="id"> Process Code. </param>
        public override void vDelete(int IdObj, string id)
        {
            this.OModel = new ExtractsModel();

            if (this.OModel.bDelete(IdObj))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetList(id);
        }

        /// <summary>
        ///   Delete Special Extract Then Get All Extracts.
        /// </summary>
        /// <param name="IdObj"> Extract Code. </param>
        /// <param name="id"> Process Code. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="contractor"> Check this User Is Contractor Or Officer. </param>
        public void vDeleteoBJ(int IdObj, string id, int uc, bool? contractor)
        {
            this.OModel = new ExtractsModel();

            if (this.OModel.bDelete(IdObj, uc, generalMethod.vIPAddress()))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            // المقاول
            if (contractor == true)
                GetList(id, uc.ToString());
            else // موظف التأمينات
                GetList(id);
        }

    }
}
