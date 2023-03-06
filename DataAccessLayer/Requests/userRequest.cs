using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class UserRequest : RequestBase<UserModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        public List<AreaModel> LareaModel { get; set; }
        public List<ReferenceSideContractorModel> LreferenceSideContractorModel { get; set; }
        public List<OfficeInsuranceModel> LofficeInsuranceModel { get; set; }
        public List<DataDrob> data = new List<DataDrob>();

        /// <summary>
        /// Get List Of All Users
        /// </summary>
        public override void GetInit()
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.LModels = new UserModel().GetAll();
        }
        /// <summary>
        /// Get All Users In Inurance Authority
        /// </summary>
        public void GetUserGroup()
        {
            this.LModels = new UserModel().GetAllUserGroup();

        }
        /// <summary>
        /// Get One User
        /// </summary>
        /// <param name="Id">User Code</param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new UserModel().GetById(Id);
        }
        /// <summary>
        /// Get Legal Entity For User
        /// </summary>
        /// <param name="Id">User Code</param>
        public void GetInitObjectLegalEntity(int Id)
        {
            this.OModel = new UserModel().GetByIdLegalEntity(Id);
        }
        /// <summary>
        /// Get Users By User Type
        /// </summary>
        /// <param name="Id">User Type</param>
        public override void GetList(string Id)
        {

            if (Convert.ToInt32(Id) == 1) //مكاتب التأمينات
            {
                this.LareaModel = new AreaModel().GetAll();
                for (int i = 0; i < LareaModel.Count; i++)
                    data.Add(new DataDrob() { Value = LareaModel[i].iAreaCode, Text = LareaModel[i].sAreaName });
            }
            else if (Convert.ToInt32(Id) == 2 || Convert.ToInt32(Id) == 3) //جهة الاسناد  //المقاولين
            {
                this.LreferenceSideContractorModel = new ReferenceSideContractorModel().GetAllCont();
                for (int i = 0; i < LreferenceSideContractorModel.Count; i++)
                    data.Add(new DataDrob() { Value = LreferenceSideContractorModel[i].iReferenceSideContractorCode, Text = LreferenceSideContractorModel[i].sReferenceSideContractorName });
            }
        }
        /// <summary>
        ///  Get List Of All User for special User
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <returns> Request. </return>
        public void GetInitByUser(int userCode)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.LModels = new UserModel().GetAllByUser(userCode);
        }
        /// <summary>
        /// Edit User Data
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">User Code</param>
        public override void vEdit(UserModel newObj, int Id)
        {
            newObj.sIpUpdate = generalMethod.vIPAddress();
            this.OModel = new UserModel();
            if (this.OModel.bEdit(newObj, Id))
                bIsEdit = true;
            else
                bIsEdit = false;

            GetInit();
        }
        /// <summary>
        /// Save User
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public override void vSave(UserModel newObj)
        {
            newObj.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new UserModel();
            int iUserCode = 0;
            if (this.OModel.bSave(newObj))
            {
                bIsSaved = true;
                iUserCode = this.OModel.iUserCode;
            }
            else
                bIsSaved = false;

            GetInit();
            this.OModel.iUserCode = iUserCode;
        }
        /// <summary>
        /// Saving Methos
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="Id">Code</param>
        public override void vSave(UserModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get Offices Insurance By Areas
        /// </summary>
        /// <param name="searchObjs">Areas Codes</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.LofficeInsuranceModel = new OfficeInsuranceModel().lSearch(searchObjs);
            for (int i = 0; i < LofficeInsuranceModel.Count; i++)
                data.Add(new DataDrob() { Value = LofficeInsuranceModel[i].iOfficeInsuranceCode, Text = LofficeInsuranceModel[i].sOfficeInsuranceIDName });

        }
        /// <summary>
        /// Search For Users
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        public void vSearchList(List<string> searchObjs)
        {
            this.OModel = new UserModel();
            this.LModels = new UserModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="Id">Delete Code</param>
        public override void vDelete(int Id)
        {
            this.OModel = new UserModel();
            string sImageURL = null;
            if (this.OModel.bDelete(Id))
            {
                bIsDeleted = true;
                sImageURL = this.OModel.sImageURL;
            }
            else
                bIsDeleted = false;

            GetInit();
            this.OModel.sImageURL = sImageURL;
        }
        /// <summary>
        /// Deleting Method
        /// </summary>
        /// <param name="IdObj"></param>
        /// <param name="id"></param>
        //public override void vDelete(int IdObj, string id)
        public override void vDelete(int IdObj, string id)
        {
            this.OModel = new UserModel();
            string sImageURL = null;
            if (this.OModel.bDelete(IdObj))
            {
                bIsDeleted = true;
                sImageURL = this.OModel.sImageURL;
            }
            else
                bIsDeleted = false;

            GetInitByUser(Convert.ToInt32(id));
            this.OModel.sImageURL = sImageURL;
        }
        /// <summary>
        /// Update Image For User
        /// </summary>
        /// <param name="newObj">Data Need For Update</param>
        /// <param name="Id">User Code</param>
        public void vUpdateImage(UserModel newObj, int Id)
        {
            this.OModel = new UserModel();
            if (this.OModel.updateImage(newObj, Id))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;
        }
        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="newObj">Data Need For Change Password</param>
        public void vChangePassword(UserModel newObj)
        {
            try
            {
                this.OModel = new UserModel();
                if (this.OModel.bChangePassword(newObj))
                    this.OModel.bIsEdit = true;
                else
                    this.OModel.bIsEdit = false;
            }
            catch { this.OModel.bIsEdit = false; }
        }
    }
}
