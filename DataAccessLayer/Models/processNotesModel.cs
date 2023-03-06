using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class ProcessNotesModel : ModelBase<ProcessNotesModel, processNote>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        #region details
        public int iProcessNotesCode { get; set; }
        public int iProcessCode { get; set; }
        [Required(ErrorMessage = "برجاء ادخال الملاحظات أولا ")]
        public string sNotes { get; set; }
        public string sDate { get; set; }
        public string sTime { get; set; }
        public string sUserInser { get; set; }
        public string sOfficeUser { get; set; }
        public string sAreaUser { get; set; }
        #endregion
        /// <summary>
        /// Save Notes In Process
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        /// <returns>Save Done Or Not</returns>
        internal override bool bSave(ProcessNotesModel newObj)
        {
            try
            {
                processNote modal = new processNote();
                modal.processCode = newObj.iProcessCode;
                modal.notes = newObj.sNotes;
                modal.userInsertCode = newObj.inUserInsertCode;
                modal.dateInsert = dtServerTime;
                modal.ipInsert = newObj.sIpInsert;
                modal.seen = false;
                db.processNotes.Add(modal);
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

        internal override bool bSave(ProcessNotesModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override bool bEdit(ProcessNotesModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        internal override List<ProcessNotesModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }

        internal override bool bDelete(int Id)
        {
            throw new NotImplementedException();
        }

        internal override ProcessNotesModel GetById(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get All Notes In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <returns>List Of Notes</returns>
        internal override List<ProcessNotesModel> GetAll(int Id)
        {
            List<ProcessNotesModel> LprocessNotesModel = new List<ProcessNotesModel>();
            List<processNote> LprocessNotesEF = db.processNotes.Where(x => x.processCode == Id).OrderByDescending(x => x.dateInsert).ToList();

            if (LprocessNotesEF != null)
            {
                LprocessNotesModel = this.ConvertEFsToObjectsBasic(LprocessNotesEF);
            }
            return LprocessNotesModel;
        }
        /// <summary>
        /// Get All Notes In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <returns>List Of Notes</returns>
        internal List<ProcessNotesModel> GetAll(int? Id)
        {
            List<ProcessNotesModel> LprocessNotesModel = new List<ProcessNotesModel>();
            List<spGetNotesProcess_Result> LprocessNotesEF = db.spGetNotesProcess(Id).ToList();

            if (LprocessNotesEF != null)
            {
                LprocessNotesModel = this.ConvertEFsToObjectsBasic(LprocessNotesEF);
            }
            return LprocessNotesModel;
        }

        internal override List<ProcessNotesModel> GetAll()
        {
            throw new NotImplementedException();
        }

        internal override ProcessNotesModel ConvertEFToObject(processNote ef)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From Entity Framework 'processNote' To 'processNotesModel'
        /// </summary>
        /// <param name="lEf">Object Of Entity Frammwork 'processNote' </param>
        /// <returns>Object of processNotesModel</returns>
        internal override ProcessNotesModel ConvertEFToObjectBasic(processNote lEf)
        {
            ProcessNotesModel oProcessNotesModel = new ProcessNotesModel();
            if (lEf != null)
            {
                oProcessNotesModel.iProcessNotesCode = lEf.processNotesCode; // كود عنوان العملية
                oProcessNotesModel.iProcessCode = lEf.processCode; // كود العملية
                oProcessNotesModel.sNotes = lEf.notes; // الملاحظه على العمليه
                oProcessNotesModel.sDate = lEf.dateInsert.ToString(); // تاريخ ارسال الملاحظه
                oProcessNotesModel.sUserInser = lEf.user.userName; // اسم المستخدم الى دخل الملاحظه

                var officeInsurances_ = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == lEf.user.officeInsuranceCode);
                if (officeInsurances_ != null)
                    oProcessNotesModel.sOfficeUser = officeInsurances_.officeInsuranceName; //مكتب المستخدم

                var officeInsurances = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == lEf.user.officeInsuranceCode);
                if (officeInsurances != null)
                {
                    int areaCode = officeInsurances.areaCode;

                    var areas = db.areas.FirstOrDefault(x => x.areaCode == areaCode);
                    if (areas != null)
                        oProcessNotesModel.sAreaUser = areas.areaName; // اسم المنطقه
                }


            }
            return oProcessNotesModel;
        }
        /// <summary>
        /// Convert From Entity Framework 'spGetNotesProcess_Result' To 'processNotesModel'
        /// </summary>
        /// <param name="EF">Object Of Entity Frammwork 'spGetNotesProcess_Result' </param>
        /// <returns>Object of processNotesModel</returns>
        internal ProcessNotesModel ConvertEFToObjectBasic(spGetNotesProcess_Result EF)
        {
            ProcessNotesModel oProcessNotesModel = new ProcessNotesModel();
            if (EF != null)
            {
                oProcessNotesModel.iProcessNotesCode = EF.processNotesCode; // كود ملاحظه العمليه
                oProcessNotesModel.iProcessCode = EF.processCode; // كود العملية
                oProcessNotesModel.sNotes = EF.notes; // الملاحظه على العمليه
                oProcessNotesModel.sDate = EF.dateInsert; // تاريخ ارسال الملاحظه
                oProcessNotesModel.sTime = EF.timeInsert; // وقت ارسال الملاحظه
                oProcessNotesModel.sUserInser = EF.userName; // اسم المستخدم الى دخل الملاحظه
                oProcessNotesModel.sOfficeUser = EF.officeIDName; //مكتب المستخدم
                oProcessNotesModel.sAreaUser = EF.areaIDName; // اسم المنطقه
            }
            return oProcessNotesModel;
        }

        internal override List<ProcessNotesModel> ConvertEFsToObjects(List<processNote> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert From List Of Entity Framework 'processNote'  To List Of 'processNotesModel'
        /// </summary>
        /// <param name="lEf">List Of Entity Frammwork 'processNote' </param>
        /// <returns>List of processNotesModel</returns>
        internal override List<ProcessNotesModel> ConvertEFsToObjectsBasic(List<processNote> lEf)
        {
            List<ProcessNotesModel> LprocessNotesModel = new List<ProcessNotesModel>();
            if (lEf != null)
            {
                foreach (processNote oEF in lEf)
                {
                    LprocessNotesModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LprocessNotesModel;
        }
        /// <summary>
        /// Convert From List Of Entity Framework 'spGetNotesProcess_Result'  To List Of 'processNotesModel'
        /// </summary>
        /// <param name="lEF">List Of Entity Frammwork 'spGetNotesProcess_Result' </param>
        /// <returns>List of processNotesModel</returns>
        internal List<ProcessNotesModel> ConvertEFsToObjectsBasic(List<spGetNotesProcess_Result> lEF)
        {
            List<ProcessNotesModel> LprocessNotesModel = new List<ProcessNotesModel>();
            if (lEF != null)
            {
                foreach (var oEF in lEF)
                {
                    LprocessNotesModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LprocessNotesModel;
        }

        internal override processNote ConvertObjectToEF(ProcessNotesModel bl)
        {
            throw new NotImplementedException();
        }

    }
}
