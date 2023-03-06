using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace DataAccessLayer.Models
{
    public class ProcessOppositionModel : ModelBase<ProcessOppositionModel, processOpposition>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iProcessOppositionCode { get; set; }
        public Nullable<int> iProcessCode { get; set; }
        public Nullable<int> iOppositionTypeCode { get; set; }
        public string sIoppositionTypeName { get; set; }
        [Required(ErrorMessage = "برجاء ادخال السبب أولا ")]
        public string sProcessOppositionReason { get; set; }
        public string sProcessOppositionNotes { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Opposition /  Exemption In Process
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Process Code</param>
        /// <returns>Edit Done Or Not</returns>
        internal override bool bEdit(ProcessOppositionModel newObj, int Id)
        {
            try
            {
                processOpposition modal = db.processOppositions.FirstOrDefault(x => x.processCode == Id);
                if (modal == null)
                    return false;
                modal.processCode = newObj.iProcessCode;
                modal.oppositionTypeCode = newObj.iOppositionTypeCode;
                modal.processOppositionNotes = newObj.sProcessOppositionNotes;
                modal.processOppositionReason = newObj.sProcessOppositionReason;
                modal.userUpdateCode = newObj.inUserUpdateCode;
                modal.dateUpdate = DateTime.Now;
                modal.ipUpdate = newObj.sIpUpdate;

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
        /// Save  Opposition /  Exemption In Process
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(ProcessOppositionModel newObj)
        {
            bool exists = db.processOppositions.Any(t => t.processCode == newObj.iProcessCode);
            if (exists) // edit
            {
                try
                {
                    processOpposition modal = db.processOppositions.FirstOrDefault(x => x.processCode == newObj.iProcessCode);
                    if (modal == null)
                        return false;
                    modal.processCode = newObj.iProcessCode;
                    modal.oppositionTypeCode = newObj.iOppositionTypeCode;
                    modal.processOppositionNotes = newObj.sProcessOppositionNotes;
                    modal.processOppositionReason = newObj.sProcessOppositionReason;
                    modal.userUpdateCode = (int)newObj.inUserUpdateCode;
                    modal.dateUpdate = DateTime.Now;
                    modal.ipUpdate = newObj.sIpUpdate;

                    if (db.SaveChanges() > 0)
                        return true;
                    else return false;
                }
                catch
                {
                    return false;
                }
            }
            else //save new
            {
                try
                {

                    processOpposition modal = new processOpposition();
                    modal.processCode = newObj.iProcessCode;
                    modal.oppositionTypeCode = newObj.iOppositionTypeCode;
                    modal.processOppositionNotes = newObj.sProcessOppositionNotes;
                    modal.processOppositionReason = newObj.sProcessOppositionReason;
                    modal.userInsertCode = newObj.inUserInsertCode;
                    modal.dateInsert = dtServerTime;
                    modal.ipInsert = newObj.sIpInsert;
                    modal.seen = false;
                    db.processOppositions.Add(modal);
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
        }

        internal override bool bSave(ProcessOppositionModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<ProcessOppositionModel> ConvertEFsToObjects(List<processOpposition> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert List Of Entity Framwork 'processOpposition' To List Of 'processOppositionModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Framwork 'processOpposition'</param>
        /// <returns>List Of 'processOppositionModel'</returns>
        internal override List<ProcessOppositionModel> ConvertEFsToObjectsBasic(List<processOpposition> lEf)
        {
            List<ProcessOppositionModel> LprocessOppositionModel = new List<ProcessOppositionModel>();
            if (lEf != null)
            {
                foreach (processOpposition oEF in lEf)
                {
                    LprocessOppositionModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LprocessOppositionModel;
        }

        internal override ProcessOppositionModel ConvertEFToObject(processOpposition ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Objcet Of Entity Framwork 'processOpposition' To Objcet Of 'processOppositionModel'
        /// </summary>
        /// <param name="lEf">Objcet Of Entity Framwork 'processOpposition'</param>
        /// <returns>Objcet Of 'processOppositionModel'</returns>
        internal override ProcessOppositionModel ConvertEFToObjectBasic(processOpposition lEf)
        {

            ProcessOppositionModel OprocessOppositionModel = new ProcessOppositionModel();
            if (lEf != null)
            {
                OprocessOppositionModel.iProcessOppositionCode = lEf.processOppositionCode;
                OprocessOppositionModel.iProcessCode = lEf.processCode;
                OprocessOppositionModel.iOppositionTypeCode = lEf.oppositionTypeCode;
                OprocessOppositionModel.sIoppositionTypeName = lEf.oppositionType.oppositionTypeName;
                OprocessOppositionModel.sProcessOppositionReason = lEf.processOppositionReason;
                OprocessOppositionModel.sProcessOppositionNotes = lEf.processOppositionNotes;
                OprocessOppositionModel.inUserInsertCode = lEf.userInsertCode;
                OprocessOppositionModel.dtDateInsert = lEf.dateInsert;
                OprocessOppositionModel.sIpInsert = lEf.ipInsert;
                OprocessOppositionModel.inUserUpdateCode = lEf.userUpdateCode;
                OprocessOppositionModel.dtDateUpdate = lEf.dateUpdate;
                OprocessOppositionModel.sIpUpdate = lEf.ipUpdate;
            }
            return OprocessOppositionModel;
        }

        internal override processOpposition ConvertObjectToEF(ProcessOppositionModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<ProcessOppositionModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get List Of Opposition /  Exemption In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <returns>List Of Opposition /  Exemption</returns>
        internal override List<ProcessOppositionModel> GetAll(int Id)
        {
            List<ProcessOppositionModel> LprocessOppositionModel = new List<ProcessOppositionModel>();
            List<processOpposition> LprocessOppositionEF = db.processOppositions.Where(x => x.processCode == Id).ToList();
            if (LprocessOppositionEF != null)
            {
                LprocessOppositionModel = this.ConvertEFsToObjectsBasic(LprocessOppositionEF);
            }
            return LprocessOppositionModel;
        }
        /// <summary>
        /// Get One Opposition /  Exemption In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <returns>Opposition /  Exemption</returns>
        internal override ProcessOppositionModel GetById(int Id)
        {
            ProcessOppositionModel OprocessOppositionModel = new ProcessOppositionModel();
            processOpposition EF = db.processOppositions.FirstOrDefault(x => x.processCode == Id);
            if (EF != null)
            {
                OprocessOppositionModel.iProcessOppositionCode = EF.processOppositionCode;
                OprocessOppositionModel.iProcessCode = EF.processCode;
                OprocessOppositionModel.iOppositionTypeCode = EF.oppositionTypeCode;
                OprocessOppositionModel.sIoppositionTypeName = EF.oppositionType.oppositionTypeName;
                OprocessOppositionModel.sProcessOppositionReason = EF.processOppositionReason;
                OprocessOppositionModel.sProcessOppositionNotes = EF.processOppositionNotes;
                OprocessOppositionModel.inUserInsertCode = EF.userInsertCode;
                OprocessOppositionModel.dtDateInsert = EF.dateInsert;
                OprocessOppositionModel.sIpInsert = EF.ipInsert;
                OprocessOppositionModel.inUserUpdateCode = EF.userUpdateCode;
                OprocessOppositionModel.dtDateUpdate = EF.dateUpdate;
                OprocessOppositionModel.sIpUpdate = EF.ipUpdate;
            }
            return OprocessOppositionModel;
        }

        internal override List<ProcessOppositionModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
    }
}
