using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GroupModel : ModelBase<GroupModel, group>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        #region details
        public int iGroupCode { get; set; }
        [Required(ErrorMessage = "ادخال اسم المجموعه ")]
        public string sGroupName { get; set; }
        public Nullable<bool> bIsActive { get; set; }
        #endregion
        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="Id">Group Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.groups.Remove(db.groups.FirstOrDefault(x => x.groupCode == Id));
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
        /// <summary>
        /// Edit Group
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Group Code</param>
        /// <returns>Edit Done Or  Not</returns>
        internal override bool bEdit(GroupModel newObj, int Id)
        {
            try
            {
                group model = db.groups.FirstOrDefault(x => x.groupCode == Id);
                if (model == null)
                    return false;
                model.groupName = newObj.sGroupName;
                model.isActive = newObj.bIsActive;
                model.userUpdateCode = newObj.inUserUpdateCode;
                model.dateUpdate = DateTime.Now;
                model.ipUpdate = newObj.sIpUpdate;

                if (db.SaveChanges() > 0)
                    return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Save Group
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(GroupModel newObj)
        {
            try
            {
                group modal = new group();
                modal.groupName = newObj.sGroupName;
                modal.isActive = true;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;

                db.groups.Add(modal);
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

        internal override bool bSave(GroupModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Object Of Entity Framework 'group' To Object Of 'groupModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'group'</param>
        /// <returns>Object Of 'groupModel'</returns>
        internal override GroupModel ConvertEFToObjectBasic(group lEf)
        {
            GroupModel OgroupModel = new GroupModel();
            if (lEf != null)
            {
                OgroupModel.iGroupCode = lEf.groupCode;
                OgroupModel.sGroupName = lEf.groupName;
                OgroupModel.bIsActive = lEf.isActive;
                OgroupModel.inUserInsertCode = lEf.userInsertCode;
                OgroupModel.dtDateInsert = lEf.dateInsert;
                OgroupModel.sIpInsert = lEf.ipInsert;
                OgroupModel.inUserUpdateCode = lEf.userUpdateCode;
                OgroupModel.dtDateUpdate = lEf.dateUpdate;
                OgroupModel.sIpUpdate = lEf.ipUpdate;
            }
            return OgroupModel;
        }
        /// <summary>
        /// Convert List Of Entity Framework 'group' To List Of 'groupModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'group'</param>
        /// <returns>List Of 'groupModel'</returns>
        internal override List<GroupModel> ConvertEFsToObjectsBasic(List<group> lEf)
        {
            List<GroupModel> LgroupModel = new List<GroupModel>();
            if (lEf != null)
            {
                foreach (group oEF in lEf)
                {
                    LgroupModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LgroupModel;
        }
        /// <summary>
        /// Convert Object Of Entity Framework 'group' To Object Of 'groupModel'
        /// </summary>
        /// <param name="ef">Object Of Entity Framework 'group'</param>
        /// <returns>Object Of 'groupModel'</returns>
        internal override GroupModel ConvertEFToObject(group ef)
        {
            GroupModel OgroupModel = new GroupModel();
            if (ef != null)
            {
                OgroupModel.iGroupCode = ef.groupCode;
                OgroupModel.sGroupName = ef.groupName;
                OgroupModel.bIsActive = ef.isActive;
                OgroupModel.inUserInsertCode = ef.userInsertCode;
                OgroupModel.dtDateInsert = ef.dateInsert;
                OgroupModel.sIpInsert = ef.ipInsert;
                OgroupModel.inUserUpdateCode = ef.userUpdateCode;
                OgroupModel.dtDateUpdate = ef.dateUpdate;
                OgroupModel.sIpUpdate = ef.ipUpdate;
            }

            return OgroupModel;
        }
        /// <summary>
        /// Convert List Of Entity Framework 'group' To List Of 'groupModel'
        /// </summary>
        /// <param name="EF">List Of Entity Framework 'group'</param>
        /// <returns>List Of 'groupModel'</returns>
        internal override List<GroupModel> ConvertEFsToObjects(List<group> lEf)
        {
            List<GroupModel> LgroupModel = new List<GroupModel>();
            if (lEf != null)
            {
                foreach (group oEF in lEf)
                {
                    LgroupModel.Add(this.ConvertEFToObject(oEF));
                }
            }
            return LgroupModel;
        }
        /// <summary>
        /// Convert Object Of  'groupModel' To Object Of Entity Framework 'group'
        /// </summary>
        /// <param name="bl"> Object Of  'groupModel' </param>
        /// <returns>Object Of Entity Framework 'group'</returns>
        internal override group ConvertObjectToEF(GroupModel bl)
        {
            group OgroupModel = new group();
            if (bl != null)
            {
                OgroupModel.groupCode = bl.iGroupCode;
                OgroupModel.groupName = bl.sGroupName;
                OgroupModel.isActive = bl.bIsActive;
                OgroupModel.userInsertCode = bl.inUserInsertCode;
                OgroupModel.dateInsert = bl.dtDateInsert;
                OgroupModel.ipInsert = bl.sIpInsert;
                OgroupModel.userUpdateCode = bl.inUserUpdateCode;
                OgroupModel.dateUpdate = bl.dtDateUpdate;
                OgroupModel.ipUpdate = bl.sIpUpdate;
            }
            return OgroupModel;
        }
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns>List Of Group Model</returns>
        internal override List<GroupModel> GetAll()
        {
            List<GroupModel> LgroupModel = new List<GroupModel>();
            List<group> LgroupEF = db.groups.ToList();

            if (LgroupEF != null)
            {
                LgroupModel = this.ConvertEFsToObjectsBasic(LgroupEF);
            }
            return LgroupModel;
        }

        internal override List<GroupModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get One Group
        /// </summary>
        /// <param name="Id">Group Code </param>
        /// <returns>Group</returns>
        internal override GroupModel GetById(int Id)
        {
            GroupModel OgroupModel = new GroupModel();
            group Ogroup = db.groups.FirstOrDefault(x => x.groupCode == Id);
            if (Ogroup != null)
            {
                OgroupModel = this.ConvertEFToObjectBasic(Ogroup);
            }
            return OgroupModel;
        }
        /// <summary>
        /// Search For Groups
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Groups</returns>
        internal override List<GroupModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetGroup(searchObjs[0]).ToList();

                List<GroupModel> LgroupModel = new List<GroupModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        GroupModel ogroupModellEF = new GroupModel();
                        ogroupModellEF.iGroupCode = item.groupCode;
                        ogroupModellEF.sGroupName = item.groupName;
                        ogroupModellEF.bIsActive = item.isActive;

                        LgroupModel.Add(ogroupModellEF);
                    }
                }

                return LgroupModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
