using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class CardWorkerStopActiveModel : ModelBase<CardWorkerStopActiveModel, cardWorkerStopActive>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iCardWorkerStopActiveCode { get; set; }
        public int iWorkerCode { get; set; }
        [Required(ErrorMessage = "برجاء ادخال السبب ")]
        public string sReasons { get; set; }
        public string sNotes { get; set; }
        public string sOfficeInsuranceName { get; set; }
        [Required(ErrorMessage = "برجاء ادخال التاريخ ")]
        public Nullable<DateTime> dDateStartStopActive { get; set; }
        public Nullable<DateTime> dDateEndStopActive { get; set; }
        [Required(ErrorMessage = "برجاء ادخال التاريخ ")]
        public string sDateStartStopActive { get; set; }
        public string sDateEndStopActive { get; set; }
        #endregion
        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Edit Worker Card Status
        /// </summary>
        /// <param name="newObj">Data Need For Edit</param>
        /// <param name="Id">Worker Card Status Code </param>
        /// <returns>Edit Done Or Not</returns>
        internal override bool bEdit(CardWorkerStopActiveModel newObj, int Id)
        {
            try
            {
                cardWorkerStopActive model = db.cardWorkerStopActives.First(x => x.cardWorkerStopActiveCode == Id);
                model.workerCode = newObj.iWorkerCode;
                model.reasons = newObj.sReasons;
                model.notes = newObj.sNotes;
                model.dateStartStopActive = Convert.ToDateTime(newObj.sDateStartStopActive);
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
        /// Save Worker Card Status
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(CardWorkerStopActiveModel newObj)
        {
            try
            {
                // save new record
                cardWorkerStopActive modal = new cardWorkerStopActive();
                cardWorkerStopActive LastRow = new cardWorkerStopActive();
                

                bool exists = false;
                var LastWorkerDetails = db.workerDetails.OrderByDescending(x => x.workerDetailsCode).FirstOrDefault(y => y.workerCode == newObj.iWorkerCode);

                if (LastWorkerDetails == null)
                    return false;

                var LastRowStop = db.cardWorkerStopActives.OrderByDescending(p => p.cardWorkerStopActiveCode).FirstOrDefault(x => x.workerCode == newObj.iWorkerCode && x.dateEndStopActive == null);

                int? cardCode = LastWorkerDetails.cardCode;

                //   هنا العامل خد كارت جديد و معاه كارت قديم و ليه داتا في جدول ايقاف / تفعيل الكارت للكارت القديم
                if (LastRowStop != null && LastRowStop.cardCode != LastWorkerDetails.cardCode)
                {
                    card CardWorker = db.cards.FirstOrDefault(x => x.cardCode == LastRowStop.cardCode);
                    modal.workerCode = newObj.iWorkerCode;
                    modal.reasons = newObj.sReasons;
                    modal.notes = newObj.sNotes;
                    if (CardWorker != null && CardWorker.isActive == false)
                        modal.cardCode = LastWorkerDetails.cardCode;
                    else if (CardWorker != null && CardWorker.isActive == true)
                        modal.cardCode = LastRowStop.cardCode;
                    modal.dateStartStopActive = Convert.ToDateTime(newObj.sDateStartStopActive);
                    modal.userInsertCode = newObj.inUserInsertCode;
                    modal.dateInsert = dtServerTime;
                    modal.ipInsert = newObj.sIpInsert;

                    db.cardWorkerStopActives.Add(modal);
                    if (db.SaveChanges() > 0 && CardWorker != null)
                    {
                        // change the status of card
                        if (CardWorker.isActive == false)
                            CardWorker.isActive = true;
                        else if (CardWorker.isActive == true)
                            CardWorker.isActive = false;
                        if (db.SaveChanges() > 0)
                        {
                            //update the end date for last caed
                            LastRowStop.dateEndStopActive = Convert.ToDateTime(newObj.sDateStartStopActive);
                            if (db.SaveChanges() > 0)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    }

                }
                if (cardCode != null)
                {
                    exists = db.cardWorkerStopActives.Any(t => t.workerCode == newObj.iWorkerCode && t.cardCode == cardCode);
                    if (exists) // have card stop
                        LastRow = db.cardWorkerStopActives.OrderByDescending(p => p.cardWorkerStopActiveCode).FirstOrDefault(x => x.workerCode == newObj.iWorkerCode && x.cardCode == cardCode);

                    modal.workerCode = newObj.iWorkerCode;
                    modal.reasons = newObj.sReasons;
                    modal.notes = newObj.sNotes;
                    modal.cardCode = cardCode;
                    modal.dateStartStopActive = Convert.ToDateTime(newObj.sDateStartStopActive);
                    modal.userInsertCode = newObj.inUserInsertCode;
                    modal.dateInsert = dtServerTime;
                    modal.ipInsert = newObj.sIpInsert;

                    db.cardWorkerStopActives.Add(modal);
                    if (db.SaveChanges() > 0)
                    {
                        // change the status of card
                        card CardWorker = db.cards.FirstOrDefault(x => x.cardCode == cardCode);
                        if (CardWorker != null && CardWorker.isActive == false)
                            CardWorker.isActive = true;
                        else if (CardWorker != null && CardWorker.isActive == true)
                            CardWorker.isActive = false;

                        if (db.SaveChanges() > 0)
                        {
                            if (exists && LastRow != null)
                            {
                                //update the end date for last caed                   
                                LastRow.dateEndStopActive = Convert.ToDateTime(newObj.sDateStartStopActive);
                                if (db.SaveChanges() > 0)
                                    return true;
                                else
                                    return false;
                            }
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }


                else
                    return false;
            }
            catch
            {
                return false;
            }

        }

        internal override bool bSave(CardWorkerStopActiveModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<CardWorkerStopActiveModel> ConvertEFsToObjects(List<cardWorkerStopActive> lEf)
        {
            throw new NotImplementedException();
        }

        internal override List<CardWorkerStopActiveModel> ConvertEFsToObjectsBasic(List<cardWorkerStopActive> lEf)
        {
            throw new NotImplementedException();
        }

        internal override CardWorkerStopActiveModel ConvertEFToObject(cardWorkerStopActive ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert Object Of  Entity Framework 'cardWorkerStopActive' To Object Of  'cardWorkerStopActiveModel' 
        /// </summary>
        /// <param name="lEf">Object Of  Entity Framework 'cardWorkerStopActive'</param>
        /// <returns>Object Of  'cardWorkerStopActiveModel' </returns>
        internal override CardWorkerStopActiveModel ConvertEFToObjectBasic(cardWorkerStopActive lEf)
        {
            CardWorkerStopActiveModel OcardWorkerStopActiveModel = new CardWorkerStopActiveModel();
            if (lEf != null)
            {
                OcardWorkerStopActiveModel.iCardWorkerStopActiveCode = lEf.cardWorkerStopActiveCode;
                OcardWorkerStopActiveModel.iWorkerCode = lEf.workerCode;
                OcardWorkerStopActiveModel.sReasons = lEf.reasons;
                OcardWorkerStopActiveModel.sNotes = lEf.notes;
                OcardWorkerStopActiveModel.sDateStartStopActive = lEf.dateStartStopActive.ToString();
                OcardWorkerStopActiveModel.sDateEndStopActive = lEf.dateEndStopActive.ToString();
                OcardWorkerStopActiveModel.inUserInsertCode = lEf.userInsertCode;
                OcardWorkerStopActiveModel.dtDateInsert = lEf.dateInsert;
                OcardWorkerStopActiveModel.sIpInsert = lEf.ipInsert;
                OcardWorkerStopActiveModel.inUserUpdateCode = lEf.userUpdateCode;
                OcardWorkerStopActiveModel.dtDateUpdate = lEf.dateUpdate;
                OcardWorkerStopActiveModel.sIpUpdate = lEf.ipUpdate;
            }
            return OcardWorkerStopActiveModel;
        }

        internal override cardWorkerStopActive ConvertObjectToEF(CardWorkerStopActiveModel bl)
        {
            throw new NotImplementedException();
        }

        internal override List<CardWorkerStopActiveModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///Get List Of Worker Card Status
        /// </summary>
        /// <param name="Id">Worker Code</param>
        /// <returns>List Of Worker Card Status</returns>
        internal override List<CardWorkerStopActiveModel> GetAll(int Id)
        {
            try
            {
                var models = db.GetCardWorkerStopActive(Id, null, null, null, null, null, null).ToList();
                List<CardWorkerStopActiveModel> LcardWorkerStopActiveModel = new List<CardWorkerStopActiveModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardWorkerStopActiveModel OcardWorkerStopActiveModellEF = new CardWorkerStopActiveModel();
                        OcardWorkerStopActiveModellEF.iCardWorkerStopActiveCode = item.cardWorkerStopActiveCode;
                        OcardWorkerStopActiveModellEF.sReasons = item.reasons;
                        OcardWorkerStopActiveModellEF.sNotes = item.notes;
                        OcardWorkerStopActiveModellEF.dDateStartStopActive = Convert.ToDateTime(item.dateStartStopActive);
                        OcardWorkerStopActiveModellEF.dDateEndStopActive = Convert.ToDateTime(item.dateEndStopActive);
                        OcardWorkerStopActiveModellEF.sDateStartStopActive = item.dateStartStopActive;
                        OcardWorkerStopActiveModellEF.sDateEndStopActive = item.dateEndStopActive;
                        OcardWorkerStopActiveModellEF.sOfficeInsuranceName = item.officeInsuranceName;
                        LcardWorkerStopActiveModel.Add(OcardWorkerStopActiveModellEF);
                    }
                }
                return LcardWorkerStopActiveModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Get One Worker Card Status
        /// </summary>
        /// <param name="Id">Worker Card Status Code</param>
        /// <returns>Object Of Card Worker Stop / Active</returns>
        internal override CardWorkerStopActiveModel GetById(int Id)
        {
            CardWorkerStopActiveModel OcardWorkerStopActiveModel = new CardWorkerStopActiveModel();
            cardWorkerStopActive OcardWorkerStopActive = db.cardWorkerStopActives.FirstOrDefault(x => x.cardWorkerStopActiveCode == Id);
            if (OcardWorkerStopActive != null)
            {
                OcardWorkerStopActiveModel = this.ConvertEFToObjectBasic(OcardWorkerStopActive);
            }
            return OcardWorkerStopActiveModel;
        }
        /// <summary>
        /// Search For Worker Card Status
        /// </summary>
        /// <param name="searchObjs">List Need For Search</param>
        /// <returns>List Of Worker Card Status </returns>
        internal override List<CardWorkerStopActiveModel> lSearch(List<string> searchObjs)
        {
            try
            {
                var models = db.GetCardWorkerStopActive(Convert.ToInt32(searchObjs[0]), searchObjs[1], searchObjs[2], searchObjs[5], searchObjs[6], searchObjs[3], searchObjs[4]).ToList();
                List<CardWorkerStopActiveModel> LcardWorkerStopActiveModel = new List<CardWorkerStopActiveModel>();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        CardWorkerStopActiveModel OcardWorkerStopActiveModellEF = new CardWorkerStopActiveModel();
                        OcardWorkerStopActiveModellEF.iCardWorkerStopActiveCode = item.cardWorkerStopActiveCode;
                        OcardWorkerStopActiveModellEF.sReasons = item.reasons;
                        OcardWorkerStopActiveModellEF.sNotes = item.notes;
                        OcardWorkerStopActiveModellEF.dDateStartStopActive = Convert.ToDateTime(item.dateStartStopActive);
                        OcardWorkerStopActiveModellEF.dDateEndStopActive = Convert.ToDateTime(item.dateEndStopActive);
                        OcardWorkerStopActiveModellEF.sDateStartStopActive = item.dateStartStopActive;
                        OcardWorkerStopActiveModellEF.sDateEndStopActive = item.dateEndStopActive;
                        OcardWorkerStopActiveModellEF.sOfficeInsuranceName = item.officeInsuranceName;
                        LcardWorkerStopActiveModel.Add(OcardWorkerStopActiveModellEF);
                    }
                }
                return LcardWorkerStopActiveModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
