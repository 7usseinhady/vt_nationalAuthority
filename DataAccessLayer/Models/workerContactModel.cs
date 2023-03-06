using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class WorkerContactModel : ModelBase<WorkerContactModel, workerContact>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        #region details
        public int iWorkerContactCode { get; set; }
        public int iWorkerCode { get; set; }
        [Required(ErrorMessage = "برجاء ادخال رقم الهاتف ")]
        [StringLength(11, ErrorMessage = "رقم الهاتف لا يقل عن 8 ارقام ولا يزيد عن 11 رقم", MinimumLength = 8)]
        public string sPhone { get; set; }
        #endregion
        /// <summary>
        /// Delete Worker Contact
        /// </summary>
        /// <param name="Id">Worker Contact Code</param>
        /// <returns>Delete Done Or Not</returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                workerContact oldContact = db.workerContacts.FirstOrDefault(x => x.workerContactCode == Id);
                if (oldContact == null)
                    return false;

                oldContact.Freez = true;
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

        internal override bool bEdit(WorkerContactModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Save Worker Contacts
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(WorkerContactModel newObj)
        {
            try
            {
                workerContact modal = new workerContact();
                modal.workerCode = newObj.iWorkerCode;
                modal.phone = newObj.sPhone;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.Freez = false;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;
                db.workerContacts.Add(modal);
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

        internal override bool bSave(WorkerContactModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerContactModel> ConvertEFsToObjects(List<workerContact> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert List Of Entity Framwork 'workerContact' To List Of workerContactModel
        /// </summary>
        /// <param name="lEf"> List Of Entity Framwork 'workerContact'</param>
        /// <returns>List Of workerContactModel</returns>
        internal override List<WorkerContactModel> ConvertEFsToObjectsBasic(List<workerContact> lEf)
        {
            List<WorkerContactModel> LworkerContactModel = new List<WorkerContactModel>();
            if (lEf != null)
            {
                foreach (workerContact oEF in lEf)
                {
                    LworkerContactModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LworkerContactModel;
        }

        internal override WorkerContactModel ConvertEFToObject(workerContact ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Object Of Entity Framwork 'workerContact' To Object Of workerContactModel
        /// </summary>
        /// <param name="lEf"> Object Of Entity Framwork 'workerContact'</param>
        /// <returns>Object Of workerContactModel</returns>
        internal override WorkerContactModel ConvertEFToObjectBasic(workerContact lEf)
        {
            WorkerContactModel OworkerContactModel = new WorkerContactModel();
            if (lEf != null)
            {
                OworkerContactModel.iWorkerContactCode = lEf.workerContactCode;
                OworkerContactModel.iWorkerCode = lEf.workerCode;
                OworkerContactModel.sPhone = lEf.phone;
                OworkerContactModel.inUserInsertCode = lEf.userInsertCode;
                OworkerContactModel.dtDateInsert = lEf.dateInsert;
                OworkerContactModel.sIpInsert = lEf.ipInsert;

            }
            return OworkerContactModel;
        }

        internal override workerContact ConvertObjectToEF(WorkerContactModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerContactModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Worker Contacts
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <returns>List Of Contacts</returns>
        internal override List<WorkerContactModel> GetAll(int Id)
        {
            List<WorkerContactModel> LworkerContactModel = new List<WorkerContactModel>();
            List<workerContact> LworkerContactEF = db.workerContacts.Where(x => x.workerCode == Id && x.Freez == false).ToList();

            if (LworkerContactEF != null)
            {
                LworkerContactModel = this.ConvertEFsToObjectsBasic(LworkerContactEF);
            }
            return LworkerContactModel;
        }

        internal override WorkerContactModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<WorkerContactModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }
    }
}
