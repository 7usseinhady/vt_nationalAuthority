using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class CardsRequestsRequest : RequestBase<CardsRequestModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();
         public   List<CardsRequestModel> LcompanySearch { get; set; }
        public CardsRequestModel ORequestById { get; set; }
        /// <summary>
        /// Get List Of Cards Request
        /// </summary>
        public override void GetInit()
        {
            this.OModel = new CardsRequestModel();
            this.LModels = new CardsRequestModel().GetAll();
        }
        /// <summary>
        /// Get List Of Cards Request By Parameter
        /// </summary>
        /// <param name="sStr">String Need For Filter</param>
        public  void GetInit(string sStr)
        {
            this.OModel = new CardsRequestModel();
            this.LModels = new CardsRequestModel().GetAll(sStr);
        }
        /// <summary>
        /// Get One Card Request
        /// </summary>
        /// <param name="Id">Card Request Code</param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new CardsRequestModel().GetById(Id);
            this.LModels = new CardsRequestModel().GetAll(Id);
        }
        /// <summary>
        /// Get Request To Company
        /// </summary>
        /// <param name="id">Card Request Id</param>
        public void GetRrequestByID(int id)
        {
            this.ORequestById = new CardsRequestModel().lSearchCompanyByID(id);

        }
        /// <summary>
        /// Get List Of Card Requests
        /// </summary>
        /// <param name="Id">ID Need For Filter The List</param>
        public override void GetList(string Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Card Request
        /// </summary>
        /// <param name="Id">Card Request Code</param>
        public override void vDelete(int Id)
        {
            this.OModel = new CardsRequestModel();

            if (this.OModel.bDelete(Id))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetInit();
        }
        /// <summary>
        /// Deleting Method 
        /// </summary>
        /// <param name="IdObj">Parameter Need For Deleting </param>
        /// <param name="id">Code Need For Ddeleting</param>
        public override void vDelete(int IdObj, string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Worker From Card Request
        /// </summary>
        /// <param name="CardRequest">Worker Card Request Code</param>
        public  void vDeleteWorkerRequest(int CardRequest)
        {
            this.OModel = new CardsRequestModel();

            if (this.OModel.bDeleteWorkerRequest(CardRequest))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetInitObject(CardRequest);
        }
        /// <summary>
        /// Editing Method
        /// </summary>
        /// <param name="newObj">Data Need For Editing</param>
        /// <param name="Id">Id Need For Edit</param>
        public override void vEdit(CardsRequestModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public override void vSave(CardsRequestModel newObj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="Id">Id Need For Save</param>
        public override void vSave(CardsRequestModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Card Request
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="lstr">Workers Codes For Request</param>
        public  void vSave(CardsRequestModel newObj, List<string> lstr)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            newObj.sIpInsert = this.sIpAddress;

            this.OModel = new CardsRequestModel();
            if (this.OModel.bSave(newObj,lstr))
            {
                bIsSaved = true;
            }
            else
                bIsSaved = false;

            GetInit();
        }
        /// <summary>
        /// Edit Status For Request
        /// </summary>
        /// <param name="newObj">Data Need For Edit Status</param>
        public void EditStatusRequest(CardsRequestModel newObj)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            newObj.sIpUpdate = this.sIpAddress;

            this.ORequestById = new CardsRequestModel();
            if (this.ORequestById.EditStatusRequest(newObj))
            {
                bIsEdit = true;
            }
            else
                bIsEdit = false;

        }
        /// <summary>
        /// Search For Card Requests
        /// </summary>
        /// <param name="searchObjs">List Of Data Need For Search</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.OModel = new CardsRequestModel();
            this.LModels = new CardsRequestModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Search For Card Requests In Company Module
        /// </summary>
        /// <param name="SearchObj">List Of Data Need For Search</param>
        public void vSearchCompany(List<string> SearchObj)
        {
            this.LcompanySearch = new CardsRequestModel().lSearchCompany(SearchObj);
        }
        /// <summary>
        /// Searching Method
        /// </summary>
        /// <param name="SearchObj">List Of Data Need For Search</param>
        public  void vSearchList(List<string> SearchObj)
        {
            throw new NotImplementedException();
        }
    }
}
