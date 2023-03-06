using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class WorkerInjuriesModel : ModelBase<WorkerInjuriesModel, workerInjury>
    {
        #region Worker Injuries Fields اصابات العامل
        public int iWorkerInjuriesCode { get; set; } // كود اصابه العامل
        public int iWorkerCode { get; set; } // كود العامل
        public Nullable<int> inProcessCode { get; set; } // كود العمليه
        [Required(ErrorMessage = "برجاء ادخال رقم العمليه ")]
        public string sProcessNum { get; set; } // رقم العمليه
        public string sProcessType { get; set; } // نوع العمليه
        public string sProcessSite { get; set; } // موقع العمليه
        public string sProcessMainContractor { get; set; } // المقاول الرئيسي للعمليه
        public string sAreaID { get; set; } // الرقم التعريفى للمنطقه
        public string sOfficeID { get; set; } // الرقم التعريفى للمكتب
        [Required(ErrorMessage = "برجاء ادخال تاريخ بدء الاصابه ")]
        public string sDateStartInjury { get; set; } // تاريخ بدء الاصابه
        public string sDateEndInjury { get; set; } // تاريخ نهايه الاصابه
        public Nullable<DateTime> dtDateStartInjury { get; set; } // تاريخ بدء الاصابه
        public Nullable<DateTime> dtDateEndInjury { get; set; } // تاريخ نهايه الاصابه
        [Required(ErrorMessage = "برجاء ادخال سبب الاصابه ")]
        public string sInjuryReason { get; set; } // سبب الاصابه
        public string sInjuryNote { get; set; } // ملاحظات الاصابه
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<WorkerInjuriesModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get All Worker Injuries.
        /// </summary>
        /// <param name="Id"> Worker Code. </param>
        /// <returns> List Of Worker Injuries Model. </returns>
        internal override List<WorkerInjuriesModel> GetAll(int Id)
        {
            var LWorkerInjuriesEF = db.spGetWorkerInjuries(-1, Id, "-1", "-1", "-1", "-1", "-1", new DateTime(1, 1, 1), new DateTime(1, 1, 1)).ToList();

            List<WorkerInjuriesModel> LWorkerInjuriesModel = new List<WorkerInjuriesModel>();

            if (LWorkerInjuriesEF != null)
            {
                LWorkerInjuriesModel = this.ConvertEFsToObjectsBasic(LWorkerInjuriesEF);
            }

            return LWorkerInjuriesModel;
        }

        /// <summary>
        ///   Get Object Of Worker Injury.
        /// </summary>
        /// <param name="Id"> Worker Injury Code. </param>
        /// <returns> Worker Injuries Midel. </returns>
        internal override WorkerInjuriesModel GetById(int Id)
        {
            WorkerInjuriesModel OWorkerInjuriesModel = new WorkerInjuriesModel();
            spGetWorkerInjuries_Result OWorkerInjury = db.spGetWorkerInjuries(Id, -1, "-1", "-1", "-1", "-1", "-1", new DateTime(1, 1, 1), new DateTime(1, 1, 1)).FirstOrDefault();

            if (OWorkerInjury != null)
            {
                OWorkerInjuriesModel = this.ConvertEFToObjectBasic(OWorkerInjury);
            }

            return OWorkerInjuriesModel;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Worker Injuries Model. </returns>
        internal override List<WorkerInjuriesModel> lSearch(List<string> searchObjs)
        {
            try
            {
                int workerCode = Convert.ToInt32(searchObjs[0]);
                DateTime? dtStartWorkerInjury = Convert.ToDateTime(searchObjs[6]);
                DateTime? dtEndWorkerInjury = Convert.ToDateTime(searchObjs[7]);

                var LWorkerInjuriesEF = db.spGetWorkerInjuries(-1, workerCode, searchObjs[1], searchObjs[2], searchObjs[3], searchObjs[4], searchObjs[5], dtStartWorkerInjury, dtEndWorkerInjury).ToList();

                List<WorkerInjuriesModel> LWorkerInjuriesModel = new List<WorkerInjuriesModel>();

                if (LWorkerInjuriesEF != null)
                {
                    LWorkerInjuriesModel = this.ConvertEFsToObjectsBasic(LWorkerInjuriesEF);
                }

                return LWorkerInjuriesModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(WorkerInjuriesModel newObj)
        {
            try
            {
                workerInjury modal = new workerInjury();
                modal.workerCode = newObj.iWorkerCode; // كود العامل
                modal.processCode = (int)newObj.inProcessCode; // كود العمليه
                modal.injuriesReasons = newObj.sInjuryReason; // سبب الاصابه
                modal.injuriesNotes = newObj.sInjuryNote; // ملاحظات الاصابه
                modal.dateStartInjuries = Convert.ToDateTime(newObj.sDateStartInjury); // تاريخ بدايه الاصابه 
                if (newObj.sDateEndInjury == null) // تاريخ نهايه الاصابه
                    modal.dateEndInjuries = null;
                else
                    modal.dateEndInjuries = Convert.ToDateTime(newObj.sDateEndInjury);
                modal.userInsertCode = newObj.inUserInsertCode; // كود المدخل
                modal.dateInsert = dtServerTime; // وقت الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                db.workerInjuries.Add(modal);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(WorkerInjuriesModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Worker Injuries Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(WorkerInjuriesModel newObj, int Id)
        {
            try
            {
                workerInjury modal = db.workerInjuries.FirstOrDefault(x => x.workerInjuriesCode == Id);
                if (modal != null)
                {
                    modal.processCode = (int)newObj.inProcessCode; // كود العمليه
                    modal.injuriesReasons = newObj.sInjuryReason; // سبب الاصابه
                    modal.injuriesNotes = newObj.sInjuryNote; // ملاحظات الاصابه
                    modal.dateStartInjuries = Convert.ToDateTime(newObj.sDateStartInjury); // تاريخ بدايه الاصابه 
                    if (newObj.sDateEndInjury == null) // تاريخ نهايه الاصابه
                        modal.dateEndInjuries = null;
                    else
                        modal.dateEndInjuries = Convert.ToDateTime(newObj.sDateEndInjury);
                    modal.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                    modal.dateUpdate = dtServerTime; // تاريخ التعديل
                    modal.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                    if (db.SaveChanges() > 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///   Delete Special Extract.
        /// </summary>
        /// <param name="Id"> Code Of Worker Injuries Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                db.workerInjuries.Remove(db.workerInjuries.FirstOrDefault(x => x.workerInjuriesCode == Id));
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

        /// <summary>
        ///   Convert From Entity Framework To Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="EF"> Entity Framework Of Worker Injuries 'Stored Procedure'. </param>
        /// <returns> Model Of Worker Injuries. </returns>
        private WorkerInjuriesModel ConvertEFToObjectBasic(spGetWorkerInjuries_Result EF)
        {
            WorkerInjuriesModel oWorkerInjuriesModel = new WorkerInjuriesModel();
            if (EF != null)
            {
                oWorkerInjuriesModel.iWorkerInjuriesCode = EF.workerInjuriesCode; // كود الاصابه للعامل
                oWorkerInjuriesModel.iWorkerCode = EF.workerCode; // كود العامل
                oWorkerInjuriesModel.inProcessCode = EF.processCode; // كود العمليه
                oWorkerInjuriesModel.sProcessNum = EF.processNumber; // رقم العمليه
                oWorkerInjuriesModel.sProcessMainContractor = EF.referenceSideContractorName; // المقاول الرئيسي للعمليه
                oWorkerInjuriesModel.sInjuryReason = EF.injuriesReasons; // سبب الاصابه
                oWorkerInjuriesModel.sInjuryNote = EF.injuriesNotes; // ملاحظات الاصابه
                oWorkerInjuriesModel.sDateStartInjury = EF.dateStartInjuries.ToString(); // تاريخ بدايه الاصابه 
                oWorkerInjuriesModel.sDateEndInjury = EF.dateEndInjuries == null ? null : EF.dateEndInjuries.ToString(); // تاريخ نهايه الاصابه
                oWorkerInjuriesModel.sAreaID = EF.areaID; // المنطقه
                oWorkerInjuriesModel.sOfficeID = EF.officeID; // المكتب
                oWorkerInjuriesModel.sProcessType = EF.processTypeName; // نوع العمليه
                oWorkerInjuriesModel.sProcessSite = EF.fullAddress; // موقع العمليه
            }

            return oWorkerInjuriesModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Worker Injuries 'Stored Procedure'. </param>
        /// <returns> List Of Worker Injuries Model. </returns>
        private List<WorkerInjuriesModel> ConvertEFsToObjectsBasic(List<spGetWorkerInjuries_Result> lEf)
        {
            List<WorkerInjuriesModel> LworkerInjuriesModel = new List<WorkerInjuriesModel>();

            if (lEf.Count > 0)
            {
                foreach (var item in lEf)
                {
                    LworkerInjuriesModel.Add(ConvertEFToObjectBasic(item));
                }
            }
            return LworkerInjuriesModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<WorkerInjuriesModel> ConvertEFsToObjectsBasic(List<workerInjury> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override WorkerInjuriesModel ConvertEFToObjectBasic(workerInjury lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<WorkerInjuriesModel> ConvertEFsToObjects(List<workerInjury> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override WorkerInjuriesModel ConvertEFToObject(workerInjury ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override workerInjury ConvertObjectToEF(WorkerInjuriesModel bl)
        {
            throw new NotImplementedException();
        }

    }
}
