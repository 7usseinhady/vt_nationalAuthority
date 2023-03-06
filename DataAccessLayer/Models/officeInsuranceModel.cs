using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class OfficeInsuranceModel : ModelBase<OfficeInsuranceModel, officeInsurance>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iOfficeInsuranceCode { get; set; }
        public string sOfficeInsuranceName { get; set; }
        public string sOfficeInsuranceIDName { get; set; }
        public Nullable<int> iAreaCode { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(OfficeInsuranceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(OfficeInsuranceModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(OfficeInsuranceModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert  List Of Entity Framework 'officeInsurance' To Llist Of  officeInsuranceModel
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'officeInsurance'</param>
        /// <returns>List Of officeInsuranceModel</returns>
        internal override List<OfficeInsuranceModel> ConvertEFsToObjects(List<officeInsurance> lEf)
        {
            List<OfficeInsuranceModel> LofficeInsuranceModel = new List<OfficeInsuranceModel>();
            if (lEf != null)
            {
                foreach (officeInsurance oEF in lEf)
                {
                    LofficeInsuranceModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LofficeInsuranceModel;
        }

        internal override List<OfficeInsuranceModel> ConvertEFsToObjectsBasic(List<officeInsurance> lEf)
        {
            throw new NotImplementedException();
        }

        internal override OfficeInsuranceModel ConvertEFToObject(officeInsurance ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From Entity Framework 'officeInsurance' To officeInsuranceModel
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'officeInsurance'</param>
        /// <returns>Object Of officeInsuranceModel</returns>
        internal override OfficeInsuranceModel ConvertEFToObjectBasic(officeInsurance lEf)
        {
            OfficeInsuranceModel OofficeInsuranceModel = new OfficeInsuranceModel();
            if (lEf != null)
            {
                OofficeInsuranceModel.iOfficeInsuranceCode = lEf.officeInsuranceCode;
                OofficeInsuranceModel.sOfficeInsuranceName = lEf.officeInsuranceName;
                OofficeInsuranceModel.iAreaCode = lEf.areaCode;

            }
            return OofficeInsuranceModel;
        }

        internal override officeInsurance ConvertObjectToEF(OfficeInsuranceModel bl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Of Offices Insurance
        /// </summary>
        /// <returns>List Of Offices Insurance</returns>
        internal override List<OfficeInsuranceModel> GetAll()
        {
            List<OfficeInsuranceModel> LofficeInsuranceModel = new List<OfficeInsuranceModel>();
            List<officeInsurance> LofficeInsuranceEF = db.officeInsurances.ToList();
            if (LofficeInsuranceEF != null)
                LofficeInsuranceModel = this.ConvertEFsToObjects(LofficeInsuranceEF);
            return LofficeInsuranceModel;
        }

        internal override List<OfficeInsuranceModel> GetAll(int Id)
        {
           throw new NotImplementedException();
        }

        internal override OfficeInsuranceModel GetById(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Search For Offices Insurance
        /// </summary>
        /// <param name="searchObjs">Data Need For Search</param>
        /// <returns>List Of Offices Insurance</returns>
        internal override List<OfficeInsuranceModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetOfficeInsurance(searchObjs[0],null).ToList();
                List<OfficeInsuranceModel> LOfficeInsuranceModel = new List<OfficeInsuranceModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        OfficeInsuranceModel oOfficeInsuranceModelEF = new OfficeInsuranceModel();
                        oOfficeInsuranceModelEF.iOfficeInsuranceCode = item.officeInsuranceCode;
                        oOfficeInsuranceModelEF.sOfficeInsuranceName = item.officeInsuranceName;
                        oOfficeInsuranceModelEF.sOfficeInsuranceIDName = item.officeInsuranceIDName;

                        LOfficeInsuranceModel.Add(oOfficeInsuranceModelEF);
                    }
                }

                return LOfficeInsuranceModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
