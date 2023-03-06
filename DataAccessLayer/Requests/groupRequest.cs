using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    public class GroupRequest : RequestBase<GroupModel>
    {
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        public GroupUserModel OgroupUserModel { get; set; }

        public List<GroupUserModel> LgroupUserModel = new List<GroupUserModel>();


        public List<FunctionModel> LfunctionModel = new List<FunctionModel>();

        public FunctionModel OfunctionModel = new FunctionModel();
        


        /// <summary>
        /// Get List Of Groups
        /// </summary>
        public override void GetInit()
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.OModel = new GroupModel();
            this.LModels = new GroupModel().GetAll();
        }
        /// <summary>
        /// Get One Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new GroupModel().GetById(Id);
        }
        /// <summary>
        /// Get List Of Groups
        /// </summary>
        /// <param name="Id">ID Need For Filter Groups</param>
        public override void GetList(string Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        public override void vDelete(int Id)
        {
            OModel = new GroupModel();
            if (this.OModel.bDelete(Id))
            {
                bIsDeleted = true;
                GetInit();
            }
            else
            {
                bIsDeleted = false;
                GetInit();
            }

        }
        /// <summary>
        /// Deleting Function
        /// </summary>
        /// <param name="IdObj">ID Need Foe Delete</param>
        /// <param name="id">Code Need For Delete</param>
        public override void vDelete(int IdObj, string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Group
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Group Code</param>
        public override void vEdit(GroupModel newObj, int Id)
        {
            newObj.sIpUpdate = this.sIpAddress;
            this.OModel = new GroupModel();
            if (this.OModel.bEdit(newObj, Id))
                bIsEdit = true;
            else
                bIsEdit = false;

            GetInit();
        }
        /// <summary>
        /// Save Group
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public override void vSave(GroupModel newObj)
        {
            newObj.sIpInsert = this.sIpAddress;

            this.OModel = new GroupModel();
            if (this.OModel.bSave(newObj))
                bIsSaved = true;
            else
                bIsSaved = false;

            GetInit();
        }
        /// <summary>
        /// Saving Method
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="Id">ID Need For Save</param>
        public override void vSave(GroupModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Group
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        public override void vSearch(List<string> searchObjs)
        {
            this.OModel = new GroupModel();
            this.LModels = new GroupModel().lSearch(searchObjs);
        }
        /// <summary>
        /// Searching Method
        /// </summary>
        /// <param name="SearchObj">List Of Data Need In Search</param>
        public void vSearchList(List<string> SearchObj)
        {
            throw new NotImplementedException();
        }
        #region GroupUser
        /// <summary>
        /// Get All Users In Group
        /// </summary>
        public void GetGroupUser()
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.OgroupUserModel = new GroupUserModel();
            this.LgroupUserModel = new GroupUserModel().GetAll();
        }
        /// <summary>
        /// Get All Users In Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        public void GetInitObjectUG(int Id)
        {
            this.OgroupUserModel = new GroupUserModel().GetById(Id);
            this.LgroupUserModel = new GroupUserModel().GetAll(Id);
        }
        /// <summary>
        /// Delete User In Group
        /// </summary>
        /// <param name="Id">Code Of User In Group</param>
        public void vDeleteGU(int Id)
        {
            this.OgroupUserModel = new GroupUserModel();
            if (this.OgroupUserModel.bDelete(Id))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetGroupUser();
        }
        /// <summary>
        /// Delete Group User And Get All Users In Group
        /// </summary>
        /// <param name="Id">Code Of User In Group</param>
        /// <param name="Code">Group Code</param>
        public void vDeleteGU(int Id, int Code)
        {
            this.OgroupUserModel = new GroupUserModel();
            if (this.OgroupUserModel.bDelete(Id))
                bIsDeleted = true;
            else
                bIsDeleted = false;

            GetInitObjectUG(Code);
        }
        /// <summary>
        /// Save Users In Group
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="lstr">Users Codes</param>
        public void SaveGU(GroupUserModel newObj, List<string> lstr)
        {
            newObj.sIpInsert = this.sIpAddress;

            this.OgroupUserModel = new GroupUserModel();
            if (this.OgroupUserModel.Save(newObj, lstr))
                bIsSaved = true;
            else
                bIsSaved = false;

            GetInitObjectUG(newObj.iGroupCode);
        }
        #endregion
        #region GroupPermision
        /// <summary>
        /// Get All Ppermisions For Group
        /// </summary>
        /// <param name="sStr">Group Code</param>
        public void GetAllFunction(string sStr)
        {
            LfunctionModel = new FunctionModel().GetAll(Convert.ToInt32(sStr));
        }
        /// <summary>
        /// Save Group Permisions
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="Permission">Permisiond Need For Save</param>
        public void SaveGroupPermission(FunctionModel newObj, int groupCode, List<int> Permission)
        {
            newObj.sIpInsert = this.sIpAddress;
            this.OfunctionModel = new FunctionModel();
            if (this.OfunctionModel.bSave(newObj, groupCode, Permission))
                bIsSaved = true;
            else
                bIsSaved = false;
            GetAllFunction(groupCode.ToString());
        }
        #endregion
    }
}
