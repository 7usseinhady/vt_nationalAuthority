using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class PermissionAreaOfficeModel : ModelBase<PermissionAreaOfficeModel, permissionAreaOffice>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        public List<string> Codes = new List<string>();
        #region details 
        public int iFunctionCode { get; set; }
        public string sFunctionName { get; set; }
        public Nullable<int> iParentCode { get; set; }
        public string sTable { get; set; }
        public int iCountGroup { get; set; }
        public int iAreaCode { get; set; }
        public string sAreaID { get; set; }
        public string sAreaName { get; set; }
        public int iOfficeCode { get; set; }
        public string sOfficeID { get; set; }
        public string sOfficeName { get; set; }
        public bool bContractor { get; set; }

        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(PermissionAreaOfficeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(PermissionAreaOfficeModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(PermissionAreaOfficeModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Permisions Of Areas And Offices~
        /// </summary>
        /// <param name="newObj">Data Need For Save Permisions</param>
        /// <param name="officeCode">Office Code Take Permisions</param>
        /// <param name="officeCodes">List Of Offices Code</param>
        /// <returns>Save Done Ot Not</returns>
        public bool bSave(PermissionAreaOfficeModel newObj, int officeCode, List<int> officeCodes)
        {
            try
            {
                var off = (from wor in db.groupAreaOfficeSettings
                           where wor.officeInsuranceCode == officeCode
                           select new
                           {
                               wor.groupAreaOfficeSettingCode
                           }).FirstOrDefault();

                if (off == null)
                    return false;

                string code = off.groupAreaOfficeSettingCode.ToString();

                int groupCode = (code == "" ? 0 : Convert.ToInt32(code));

                if (groupCode != 0)
                {
                    db.permissionAreaOffices.RemoveRange(db.permissionAreaOffices.Where(x => x.groupAreaOfficeSettingCode == groupCode));
                    db.SaveChanges();
                }
                int y = 0;
                for (int i = 0; i < officeCodes.Count - 1; i++)
                {
                    if (officeCodes[i] != 0)
                    {
                        permissionAreaOffice newPerm = new permissionAreaOffice();
                        newPerm.groupAreaOfficeSettingCode = groupCode;
                        newPerm.officeInsuranceCode = officeCodes[i];
                        newPerm.userInsertCode = newObj.inUserInsertCode;
                        newPerm.ipInsert = newObj.sIpInsert;
                        newPerm.dateInsert = dtServerTime;
                        db.permissionAreaOffices.Add(newPerm);
                        y = db.SaveChanges();
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

        internal override List<PermissionAreaOfficeModel> ConvertEFsToObjects(List<permissionAreaOffice> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<PermissionAreaOfficeModel> ConvertEFsToObjectsBasic(List<permissionAreaOffice> lEf)
        {
            throw new NotImplementedException();
        }

        internal override PermissionAreaOfficeModel ConvertEFToObject(permissionAreaOffice ef)
        {
            throw new NotImplementedException();
        }

        internal override PermissionAreaOfficeModel ConvertEFToObjectBasic(permissionAreaOffice lEf)
        {
            throw new NotImplementedException();
        }

        internal override permissionAreaOffice ConvertObjectToEF(PermissionAreaOfficeModel bl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Permisions
        /// </summary>
        /// <returns>List Of Permisions</returns>
        internal override List<PermissionAreaOfficeModel> GetAll()
        {
            try
            {
                var models = db.GetAllAreas(null, null).ToList();
                List<PermissionAreaOfficeModel> LAllAreasModel = new List<PermissionAreaOfficeModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        PermissionAreaOfficeModel OAllAreaslEF = new PermissionAreaOfficeModel();
                        OAllAreaslEF.iAreaCode = item.areaCode;
                        OAllAreaslEF.sAreaID = item.areaID;
                        OAllAreaslEF.sAreaName = item.areaName;
                        OAllAreaslEF.iOfficeCode = item.officeInsuranceCode;
                        OAllAreaslEF.sOfficeID = item.officeInsuranceID;
                        OAllAreaslEF.sOfficeName = item.officeInsuranceName;
                        LAllAreasModel.Add(OAllAreaslEF);
                    }
                }
                return LAllAreasModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get Permisions For Special Office Insurance
        /// </summary>
        /// <param name="Id">Office Code</param>
        /// <returns>List Of Permisions</returns>
        internal override List<PermissionAreaOfficeModel> GetAll(int Id)
        {
            int GroupCode = 0;
            bool exists = db.groupAreaOfficeSettings.Any(t => t.officeInsuranceCode == Id);
            if (!exists)
            {
                groupAreaOfficeSetting newGroup = new groupAreaOfficeSetting();
                newGroup.officeInsuranceCode = Id;
                var areas = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == Id);
                if (areas != null)
                    newGroup.areaCode = areas.areaCode;
                db.groupAreaOfficeSettings.Add(newGroup);
                db.SaveChanges();
                GroupCode = newGroup.groupAreaOfficeSettingCode;
            }
            else
            {
                var areaOff = (from wor in db.groupAreaOfficeSettings
                               where wor.officeInsuranceCode == Id
                               select new
                               {
                                   wor.groupAreaOfficeSettingCode
                               }).FirstOrDefault();
                if (areaOff != null)
                    GroupCode = areaOff.groupAreaOfficeSettingCode;
            }

            var models = db.GetAreaOffice(null, null).ToList();
            List<PermissionAreaOfficeModel> LpermissionAreaOfficeModel = new List<PermissionAreaOfficeModel>();
            if (models.Count > 0)
            {
                foreach (var item in models)
                {
                    PermissionAreaOfficeModel OpermissionAreaOfficeModellEF = new PermissionAreaOfficeModel();
                    OpermissionAreaOfficeModellEF.iFunctionCode = item.FunctionCode;
                    OpermissionAreaOfficeModellEF.sFunctionName = item.FunctionName;
                    OpermissionAreaOfficeModellEF.iParentCode = item.ParentCode;
                    OpermissionAreaOfficeModellEF.sTable = item.table_;

                    OpermissionAreaOfficeModellEF.iCountGroup = db.permissionAreaOffices.Where(x => x.officeInsuranceCode == item.FunctionCode && x.groupAreaOfficeSettingCode == GroupCode).Count();


                    LpermissionAreaOfficeModel.Add(OpermissionAreaOfficeModellEF);
                }
            }
            return LpermissionAreaOfficeModel;
        }
        /// <summary>
        /// Get All Permisions For Office Conatrctors
        /// </summary>
        /// <returns>List Of Permisiond</returns>
        public List<PermissionAreaOfficeModel> GetAllContractor()
        {
            try
            {
                var models = db.GetAreaOffice(null, null).ToList();
                List<PermissionAreaOfficeModel> LAllAreasModel = new List<PermissionAreaOfficeModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        PermissionAreaOfficeModel OpermissionAreaOfficeModellEF = new PermissionAreaOfficeModel();
                        OpermissionAreaOfficeModellEF.iFunctionCode = item.FunctionCode;
                        OpermissionAreaOfficeModellEF.sFunctionName = item.FunctionName;
                        OpermissionAreaOfficeModellEF.iParentCode = item.ParentCode;
                        OpermissionAreaOfficeModellEF.sTable = item.table_;
                        OpermissionAreaOfficeModellEF.bContractor = (bool)item.contractor;
                        LAllAreasModel.Add(OpermissionAreaOfficeModellEF);
                    }
                }
                return LAllAreasModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        internal override PermissionAreaOfficeModel GetById(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Offices Permisions
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Permisions</returns>
        internal override List<PermissionAreaOfficeModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetAllAreas(searchObjs[0], searchObjs[1]).ToList();
                List<PermissionAreaOfficeModel> LAllAreasModel = new List<PermissionAreaOfficeModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        PermissionAreaOfficeModel OAllAreaslEF = new PermissionAreaOfficeModel();
                        OAllAreaslEF.iAreaCode = item.areaCode;
                        OAllAreaslEF.sAreaName = item.areaName;
                        OAllAreaslEF.sAreaID = item.areaID;
                        OAllAreaslEF.iOfficeCode = item.officeInsuranceCode;
                        OAllAreaslEF.sOfficeID = item.officeInsuranceID;
                        OAllAreaslEF.sOfficeName = item.officeInsuranceName;
                        LAllAreasModel.Add(OAllAreaslEF);
                    }
                }
                return LAllAreasModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Search For Permisions For Office And Area
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Permisions</returns>
        public List<PermissionAreaOfficeModel> lSearchAreaOffice(List<string> searchObjs)
        {
            try
            {
                int GroupCode = 0;
                int code = 0;
                if (searchObjs[0] != null)
                {
                    code = Convert.ToInt32(searchObjs[0]);
                    var off = (from wor in db.groupAreaOfficeSettings
                               where wor.officeInsuranceCode == code
                               select new
                               {
                                   wor.groupAreaOfficeSettingCode
                               }).FirstOrDefault();
                    if (off != null)
                        GroupCode = off.groupAreaOfficeSettingCode;

                }
                var models = db.GetAreaOffice(searchObjs[1], searchObjs[2]).ToList();
                List<PermissionAreaOfficeModel> LpermissionAreaOfficeModel = new List<PermissionAreaOfficeModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        PermissionAreaOfficeModel OpermissionAreaOfficeModellEF = new PermissionAreaOfficeModel();
                        OpermissionAreaOfficeModellEF.iFunctionCode = item.FunctionCode;
                        OpermissionAreaOfficeModellEF.sFunctionName = item.FunctionName;
                        OpermissionAreaOfficeModellEF.iParentCode = item.ParentCode;
                        OpermissionAreaOfficeModellEF.sTable = item.table_;

                        OpermissionAreaOfficeModellEF.iCountGroup = db.permissionAreaOffices.Where(x => x.officeInsuranceCode == item.FunctionCode && x.groupAreaOfficeSettingCode == GroupCode).Count();


                        LpermissionAreaOfficeModel.Add(OpermissionAreaOfficeModellEF);
                    }
                }
                return LpermissionAreaOfficeModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
