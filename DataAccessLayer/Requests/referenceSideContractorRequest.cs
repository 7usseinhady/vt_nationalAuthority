using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of Reference Sides - Contractors.  
    /// </summary>
    public class ReferenceSideContractorRequest : RequestBase<ReferenceSideContractorModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        /// <summary>
        ///   Get All Reference Sides - Contractors.
        /// </summary>
        public override void GetInit()
        {
            this.LModels = new ReferenceSideContractorModel().GetAll(-1);
        }

        /// <summary>
        ///   Get Object Of 'Reference Side - Contractor'.
        /// </summary>
        /// <param name="Id"> 'Reference Side - Contractor' Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new ReferenceSideContractorModel().GetById(Id);
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
            this.LModels = new ReferenceSideContractorModel().lSearch(searchObjs);
        }


        /// <summary>
        ///   Save Object Data In Database Then Get All Reference Sides - Contractors.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        public override void vSave(ReferenceSideContractorModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();
            newObj.dtDateInsert = DateTime.Now;
            this.OModel = new ReferenceSideContractorModel();
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
        public override void vSave(ReferenceSideContractorModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database Then Get All Reference Sides - Contractors.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of 'Reference Side - Contractor' Will Be Edit. </param>
        public override void vEdit(ReferenceSideContractorModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new ReferenceSideContractorModel();
            if (this.OModel.bEdit(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;

            GetInit();
        }


        /// <summary>
        ///   Delete Special 'Reference Side - Contractor' Then Get All Reference Sides - Contractors.
        /// </summary>
        /// <param name="Id"> Code Of 'Reference Side - Contractor' Will Be Delete. </param>
        public override void vDelete(int Id)
        {
            this.OModel = new ReferenceSideContractorModel();

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