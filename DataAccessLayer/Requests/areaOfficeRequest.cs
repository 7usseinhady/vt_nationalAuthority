using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class AreaOfficeRequest : RequestBase<PermissionAreaOfficeModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();

        /// <summary>
        /// Get List Of Permissions Of Offices And Areas
        /// </summary>
        public override void GetInit()
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.OModel = new PermissionAreaOfficeModel();
            this.LModels = new PermissionAreaOfficeModel().GetAll();
        }
        /// <summary>
        /// Get One Permisions Of Office And Area
        /// </summary>
        /// <param name="Id">Permision Code</param>
        public override void GetInitObject(int Id)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.OModel = new PermissionAreaOfficeModel();
            this.LModels = new PermissionAreaOfficeModel().GetAll(Id);
        }
        /// <summary>
        /// Get List Of Permisions By Special Parameter
        /// </summary>
        /// <param name="Id">Special Parameter</param>
        public override void GetList(string Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Permision By Special Parameter
        /// </summary>
        /// <param name="Id">Special Parameter</param>
        public override void vDelete(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Permision
        /// </summary>
        /// <param name="IdObj">Spacial Id</param>
        /// <param name="id">Permision Code</param>
        public override void vDelete(int IdObj, string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Permisions For Office And Area
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Permision Code</param>
        public override void vEdit(PermissionAreaOfficeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Permisions For Office And Area
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public override void vSave(PermissionAreaOfficeModel newObj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Permisions For Office And Area By ID
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="Id">ID</param>
        public override void vSave(PermissionAreaOfficeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Offices And Areas
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.OModel = new PermissionAreaOfficeModel();
            this.LModels = new PermissionAreaOfficeModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Get All Areas
        /// </summary>
        public void GetALLAreas()
        {
            this.LModels = new PermissionAreaOfficeModel().GetAll();
        }
        /// <summary>
        /// Get All Areas For Contractors
        /// </summary>
        public void GetALLAreasContractor()
        {
            this.LModels = new PermissionAreaOfficeModel().GetAllContractor();
        }
        /// <summary>
        /// Search For Permisions Of Offices And Areas 
        /// </summary>
        /// <param name="SearchObj">Data Need For Search</param>
        public void vSearchList(List<string> SearchObj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Permisions For Offices
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="OfficeCode">Office Code Is Take Permision</param>
        /// <param name="officeCodes">Offices Codes They Gives Permisions </param>
        public void Save(PermissionAreaOfficeModel newObj, int OfficeCode, List<int> officeCodes)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();
            this.OModel = new PermissionAreaOfficeModel();
            if (this.OModel.bSave(newObj, OfficeCode, officeCodes))
                bIsSaved = true;
            else
                bIsSaved = false;
            GetInit();
        }
        /// <summary>
        /// Search For Permision For Office
        /// </summary>
        /// <param name="lstr">List Of Strings Need For Search</param>
        public void vSearchAreaOffice(List<string> lstr)
        {
            this.OModel = new PermissionAreaOfficeModel();
            this.LModels = new PermissionAreaOfficeModel().lSearchAreaOffice(lstr);
        }


    }
}
