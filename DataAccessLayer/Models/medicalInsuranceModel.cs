using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class MedicalInsuranceModel:ModelBase<MedicalInsuranceModel, medicalInsurance>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iMedicalInsuranceCode { get; set;}
        public int iWorkerCode { get; set;}
        public Nullable<int> iMedicalStatusCode { get; set;}
        public Nullable<DateTime> dDateStart { get; set;}
        public Nullable<DateTime> dDateEnd { get; set; }
        #endregion
        /// <summary>
        /// Save Worker Medical Insurance
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(MedicalInsuranceModel newObj)
        {
            try
            {
                medicalInsurance modal = new medicalInsurance();
                modal.workerCode = newObj.iWorkerCode;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;
                db.medicalInsurances.Add(modal);
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

        internal override bool bSave(MedicalInsuranceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(MedicalInsuranceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<MedicalInsuranceModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }

        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override MedicalInsuranceModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<MedicalInsuranceModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<MedicalInsuranceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        internal override MedicalInsuranceModel ConvertEFToObject(medicalInsurance ef)
        {
            throw new NotImplementedException();
        }

        internal override MedicalInsuranceModel ConvertEFToObjectBasic(medicalInsurance lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<MedicalInsuranceModel> ConvertEFsToObjects(List<medicalInsurance> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<MedicalInsuranceModel> ConvertEFsToObjectsBasic(List<medicalInsurance> lEf)
        {
            throw new NotImplementedException();
        }

        internal override medicalInsurance ConvertObjectToEF(MedicalInsuranceModel bl)
        {
            throw new NotImplementedException();
        }
      

    }
}
