using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class MissionSubContractorModel : ModelBase<MissionSubContractorModel, missionSubContractor>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iMissionSubContractorCode { get; set; }
        public int iProcessSubContractorCode { get; set; }
        public int iMissionTypeCode { get; set; }
        public string sBandNumber { get; set; }
        public string sProcessName { get; set; }
        public string sProcessTypeName { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(MissionSubContractorModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(MissionSubContractorModel newObj)
        {
            throw new NotImplementedException();
        }

        internal override bool bSave(MissionSubContractorModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<MissionSubContractorModel> ConvertEFsToObjects(List<missionSubContractor> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From List Of Entity Framwork ' missionSubContractor ' To missionSubContractor Model
        /// </summary>
        /// <param name="lEf">List Of Entity Framwork ' missionSubContractor ' </param>
        /// <returns>List Of missionSubContractor Model</returns>
        internal override List<MissionSubContractorModel> ConvertEFsToObjectsBasic(List<missionSubContractor> lEf)
        {
            List<MissionSubContractorModel> LmissionSubContractorModel = new List<MissionSubContractorModel>();
            if (lEf != null)
            {
                foreach (missionSubContractor oEF in lEf)
                {
                    LmissionSubContractorModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LmissionSubContractorModel;
        }

        internal override MissionSubContractorModel ConvertEFToObject(missionSubContractor ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From   Entity Framwork ' missionSubContractor ' To missionSubContractor Model
        /// </summary>
        /// <param name="lEf">Entity Framwork ' missionSubContractor ' </param>
        /// <returns> missionSubContractor Model</returns>

        internal override MissionSubContractorModel ConvertEFToObjectBasic(missionSubContractor lEf)
        {
            MissionSubContractorModel omissionSubContractorModel = new MissionSubContractorModel();
            if (lEf != null)
            {
                omissionSubContractorModel.iProcessSubContractorCode = lEf.processSubContractorCode;
                omissionSubContractorModel.iMissionSubContractorCode = lEf.missionSubContractorCode; // كود العملية
                omissionSubContractorModel.iMissionTypeCode = lEf.processTypeCode;
                omissionSubContractorModel.sProcessName = lEf.processSubContractor.process.processName;
                omissionSubContractorModel.sProcessTypeName = lEf.processType.processTypeName;
            }
            return omissionSubContractorModel;
        }

        internal override missionSubContractor ConvertObjectToEF(MissionSubContractorModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<MissionSubContractorModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get Al Mission From Sub-Contractor
        /// </summary>
        /// <param name="Id">Sub-Contractor Code</param>
        /// <returns>List Of Missions</returns>
        internal override List<MissionSubContractorModel> GetAll(int Id)
        {
            List<MissionSubContractorModel> LmissionSubContractorModel = new List<MissionSubContractorModel>();
            List<missionSubContractor> LmissionSubContractorEF = db.missionSubContractors.Where(x => x.processSubContractorCode == Id).ToList();

            if (LmissionSubContractorEF != null)
            {
                LmissionSubContractorModel = this.ConvertEFsToObjectsBasic(LmissionSubContractorEF);
            }
            return LmissionSubContractorModel;
        }

        internal override MissionSubContractorModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<MissionSubContractorModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Missions From  Sub-Contractor
        /// </summary>
        /// <param name="SubContrctCode">Sub Contractor Code</param>
        /// <param name="lstr">list Of Code Of Missions </param>
        /// <returns>Save Dobe Or Not</returns>
        public bool SaveList(int SubContrctCode, List<string> lstr)
        {
            try
            {
                int y = 0;
                for (int i = 0; i < lstr.Count; i++)
                {
                    missionSubContractor newMisiion = new missionSubContractor();
                    newMisiion.processSubContractorCode = SubContrctCode;
                    newMisiion.processTypeCode = Convert.ToInt32(lstr[i]);
                    db.missionSubContractors.Add(newMisiion);
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
    }
}
