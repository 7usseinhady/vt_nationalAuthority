using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class GroupUserModel : ModelBase<GroupUserModel, groupUser>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iGroupUserCode { get; set; }
        public int iGroupCode { get; set; }
        public int iUserCode { get; set; }
        public string sUserName { get; set; }
        #endregion
        /// <summary>
        /// Delete User From Group
        /// </summary>
        /// <param name="Id">User Code In Group</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                var UserDetails = db.groupUsers.FirstOrDefault(x => x.groupUserCode == Id);
                if (UserDetails == null)
                    return false;

                if (UserDetails.groupCode == 1) // هنا لو همسح مستخدم من جروب الادمن هروح اعمل ابديت ليه في جدول المستخدمين
                {
                    user user_ = db.users.FirstOrDefault(x => x.userCode == UserDetails.userCode);
                    if (user_ == null)
                        return false;

                    user_.isAdmin = false;
                    db.SaveChanges();
                }
                db.groupUsers.Remove(db.groupUsers.FirstOrDefault(x => x.groupUserCode == Id));
                if (db.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        internal override bool bEdit(GroupUserModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save User In Group
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(GroupUserModel newObj)
        {
            try
            {
                groupUser modal = new groupUser();
                modal.groupCode = newObj.iGroupCode;
                modal.userCode = iUserCode;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;

                db.groupUsers.Add(modal);
                if (db.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        internal override bool bSave(GroupUserModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save List Of Users In Group
        /// </summary>
        /// <param name="newObj">Data Need In Save</param>
        /// <param name="lstr">Lis Of Users Code</param>
        /// <returns>Save Done Or Not</returns>
        public bool Save(GroupUserModel newObj, List<string> lstr)
        {
            try
            {
                lstr = lstr.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                int y = 0;
                for (int i = 0; i < lstr.Count; i++)
                {
                    groupUser modal = new groupUser();
                    modal.groupCode = newObj.iGroupCode;
                    modal.userCode = Convert.ToInt32(lstr[i]);
                    modal.userInsertCode = newObj.inUserInsertCode;
                    modal.dateInsert = dtServerTime;
                    modal.ipInsert = newObj.sIpInsert;

                    db.groupUsers.Add(modal);
                    y = db.SaveChanges();
                    if (y > 0 && newObj.iGroupCode == 1)
                    {
                        int userCode = Convert.ToInt32(lstr[i]);
                        // هنا لو انا هدخل مستخدم في جروب الأدمن هروح اعمل ابديت ليه في جدول المستخدمين                       
                        user OldUser = db.users.FirstOrDefault(x => x.userCode == userCode);
                        if (OldUser == null)
                            return false;
                        OldUser.isAdmin = true;
                        OldUser.userUpdateCode = Convert.ToInt32(lstr[i]);
                        OldUser.dateUpdate = DateTime.Now;
                        OldUser.ipUpdate = newObj.sIpUpdate;
                        db.SaveChanges();
                    }
                }
                if (y > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        internal override List<GroupUserModel> ConvertEFsToObjects(List<groupUser> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert List Of Entity Framework 'groupUser' To List Of 'groupUserModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'groupUser'</param>
        /// <returns>List Of 'groupUserModel'</returns>

        internal override List<GroupUserModel> ConvertEFsToObjectsBasic(List<groupUser> lEf)
        {
            List<GroupUserModel> LgroupUserModel = new List<GroupUserModel>();
            if (lEf != null)
            {
                foreach (groupUser oEF in lEf)
                {
                    LgroupUserModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LgroupUserModel;
        }

        internal override GroupUserModel ConvertEFToObject(groupUser ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Object Of Entity Framework 'groupUser' To Object Of 'groupUserModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'groupUser'</param>
        /// <returns>Object Of 'groupUserModel'</returns>
        internal override GroupUserModel ConvertEFToObjectBasic(groupUser lEf)
        {
            GroupUserModel OgroupUserModel = new GroupUserModel();
            if (lEf != null)
            {
                OgroupUserModel.iGroupUserCode = lEf.groupUserCode;
                OgroupUserModel.iGroupCode = lEf.groupCode;
                OgroupUserModel.iUserCode = lEf.userCode;
                OgroupUserModel.inUserInsertCode = lEf.userInsertCode;
                OgroupUserModel.dtDateInsert = lEf.dateInsert;
                OgroupUserModel.sIpInsert = lEf.ipInsert;
                OgroupUserModel.inUserUpdateCode = lEf.userUpdateCode;
                OgroupUserModel.dtDateUpdate = lEf.dateUpdate;
                OgroupUserModel.sIpUpdate = lEf.ipUpdate;
                OgroupUserModel.sUserName = lEf.user.userName;
            }
            return OgroupUserModel;
        }

        internal override groupUser ConvertObjectToEF(GroupUserModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<GroupUserModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get List Of Users In Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        /// <returns>List Of Users</returns>
        internal override List<GroupUserModel> GetAll(int Id)
        {
            List<GroupUserModel> LgroupUserModel = new List<GroupUserModel>();
            List<groupUser> LgroupUserEF = db.groupUsers.Where(x => x.groupCode == Id).OrderBy(x => x.user.userFullName).ToList();
            if (LgroupUserEF != null)
            {
                LgroupUserModel = this.ConvertEFsToObjectsBasic(LgroupUserEF);
            }
            return LgroupUserModel;
        }
        /// <summary>
        /// Get Users In Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        /// <returns>Users Group</returns>
        internal override GroupUserModel GetById(int Id)
        {
            GroupUserModel OgroupUserModel = new GroupUserModel();
            groupUser Ogroup = db.groupUsers.FirstOrDefault(x => x.groupCode == Id);
            if (Ogroup != null)
            {
                OgroupUserModel = this.ConvertEFToObjectBasic(Ogroup);
            }
            return OgroupUserModel;
        }

        internal override List<GroupUserModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
    }
}
