using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class AreaModel : ModelBase<AreaModel, area>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iAreaCode { get; set; }
        public string sAreaName { get; set; }
        public string sAreaID { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(AreaModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(AreaModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(AreaModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From List Of Entity Framework 'area' To List Of 'areaModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framework 'area'</param>
        /// <returns>List Of 'areaModel'</returns>
        internal override List<AreaModel> ConvertEFsToObjects(List<area> lEf)
        {
            List<AreaModel> LareaModel = new List<AreaModel>();
            if (lEf != null)
            {
                foreach (area oEF in lEf)
                {
                    LareaModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LareaModel;
        }

        internal override List<AreaModel> ConvertEFsToObjectsBasic(List<area> lEf)
        {
            throw new NotImplementedException();
        }

        internal override AreaModel ConvertEFToObject(area ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert  Object Of Entity Framework 'area' To Object Of 'areaModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Framework 'area'</param>
        /// <returns>Object Of 'areaModel'</returns>
        internal override AreaModel ConvertEFToObjectBasic(area lEf)
        {
            AreaModel OareaModel = new AreaModel();
            if (lEf != null)
            {
                OareaModel.iAreaCode = lEf.areaCode;
                OareaModel.sAreaName = lEf.areaName;
            }
            return OareaModel;
        }

        internal override area ConvertObjectToEF(AreaModel bl)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get List Of Areas
        /// </summary>
        /// <returns>List Of Areas</returns>
        internal override List<AreaModel> GetAll()
        {
            List<AreaModel> LareaModel = new List<AreaModel>();
            List<area> LareaEF = db.areas.ToList();
            if (LareaEF != null)
                LareaModel = this.ConvertEFsToObjects(LareaEF);
            return LareaModel;
        }

        internal override List<AreaModel> GetAll(int Id)
        {
            throw new NotImplementedException();
        }

        internal override AreaModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<AreaModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }

    }
}
