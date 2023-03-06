using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class ManPowerModel : ModelBase<ManPowerModel, manPower>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iManPowerCode { get; set; }
        public int iWorkerCode { get; set; }
        public int iSkillDegreeCode { get; set; }
        public string sWorkerJob { get; set; }
        public DateTime dDateStart { get; set; }
        public DateTime dDateEnd { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(ManPowerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Man-Power For Worker
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(ManPowerModel newObj)
        {
            try
            {
                manPower modal = new manPower();
                modal.workerCode = newObj.iWorkerCode;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;
                db.manPowers.Add(modal);
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

        internal override bool bSave(ManPowerModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<ManPowerModel> ConvertEFsToObjects(List<manPower> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<ManPowerModel> ConvertEFsToObjectsBasic(List<manPower> lEf)
        {
            throw new NotImplementedException();
        }

        internal override ManPowerModel ConvertEFToObject(manPower ef)
        {
            throw new NotImplementedException();
        }

        internal override ManPowerModel ConvertEFToObjectBasic(manPower lEf)
        {
            throw new NotImplementedException();
        }

        internal override manPower ConvertObjectToEF(ManPowerModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<ManPowerModel> GetAll()
        {
            throw new NotImplementedException();
        }

        internal override List<ManPowerModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        internal override ManPowerModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<ManPowerModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
    }
}
