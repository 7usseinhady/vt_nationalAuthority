using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class FunctionModel : ModelBase<FunctionModel, function>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        static int GroupCode;
        #region details
        public int iFunctionCode { get; set; }
        public string sFunctionName { get; set; }
        public Nullable<int> iParentFunctionCode { get; set; }
        public int iModuleCode { get; set; }
        public string sFunctionPath { get; set; }
        public int iTreeColorCode { get; set; }
        public string sTreeColorName { get; set; }
        public int iCountGroupCode { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(FunctionModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(FunctionModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(FunctionModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Permisions For Group
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <param name="GroupCode">Group Code</param>
        /// <param name="Permissions">Permisions Codes</param>
        /// <returns>Save Done Or Not</returns>
        public bool bSave(FunctionModel newObj, int GroupCode, List<int> Permissions)
        {
            try
            {
                db.groupPermissions.RemoveRange(db.groupPermissions.Where(x => x.groupCode == GroupCode));
                db.SaveChanges();
                int y = 0;
                for (int i = 0; i < Permissions.Count - 1; i++)
                {
                    groupPermission newGroup = new groupPermission();
                    newGroup.functionCode = Permissions[i];
                    newGroup.groupCode = GroupCode;
                    newGroup.userInsertCode = newObj.inUserInsertCode;
                    newGroup.dateInsert = dtServerTime;
                    newGroup.ipInsert = newObj.sIpInsert;
                    db.groupPermissions.Add(newGroup);
                    y = db.SaveChanges();
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

        internal override List<FunctionModel> ConvertEFsToObjects(List<function> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert List Of Entity Framework 'function' To List Of 'functionModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'function'</param>
        /// <returns>List Of 'functionModel'</returns>
        internal override List<FunctionModel> ConvertEFsToObjectsBasic(List<function> lEf)
        {
            List<FunctionModel> LfunctionModel = new List<FunctionModel>();
            if (lEf != null)
            {
                foreach (function oEF in lEf)
                {
                    LfunctionModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LfunctionModel;
        }

        internal override FunctionModel ConvertEFToObject(function ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Object Of Entity Framework 'function' To Object Of 'functionModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'function'</param>
        /// <returns>Object Of 'functionModel'</returns>
        internal override FunctionModel ConvertEFToObjectBasic(function lEf)
        {
            FunctionModel OfunctionModel = new FunctionModel();
            if (lEf != null)
            {
                OfunctionModel.iFunctionCode = lEf.functionCode;
                OfunctionModel.iParentFunctionCode = lEf.parentFunctionCode;
                OfunctionModel.sFunctionName = lEf.functionName;
                OfunctionModel.sFunctionPath = lEf.functionPath;
                OfunctionModel.iModuleCode = lEf.moduleCode;
                OfunctionModel.iTreeColorCode = lEf.treeColorCode;
                OfunctionModel.sTreeColorName = lEf.treeColor.treeColorName.ToString();
                OfunctionModel.iCountGroupCode = lEf.groupPermissions.Count(x => x.functionCode == lEf.functionCode && x.groupCode == GroupCode);
            }
            return OfunctionModel;
        }

        internal override function ConvertObjectToEF(FunctionModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<FunctionModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Functions 
        /// </summary>
        /// <param name="Id">Group Code</param>
        /// <returns>List Of Functions</returns>
        internal override List<FunctionModel> GetAll(int Id)
        {
            GroupCode = Id;
            List<FunctionModel> LfunctionModel = new List<FunctionModel>();
            List<function> LfunctionEF = db.functions.Where(x => x.functionCode != 0 && x.parentFunctionCode != null).OrderBy(a => a.parentFunctionCode).ToList();

            if (LfunctionEF != null)
            {
                LfunctionModel = this.ConvertEFsToObjectsBasic(LfunctionEF);
            }
            return LfunctionModel;
        }

        internal override FunctionModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<FunctionModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
    }
}
