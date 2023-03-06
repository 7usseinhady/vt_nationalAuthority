using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    public class ProcessSubContractorModel : ModelBase<ProcessSubContractorModel, processSubContractor>
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        private readonly MissionSubContractorModel OmissionSubContractorModel = new MissionSubContractorModel();

        #region Process Sub Contractor Fields المقاولين (رئيسي - من باطن) بالعمليه
        public int iProcessSubContractorCode { get; set; }
        public int iProcessCode { get; set; }
        [Required(ErrorMessage = "برجاء اختيار المقاول ")]
        public Nullable<int> iContractorCode { get; set; }
        public Nullable<bool> bnContractorType { get; set; }
        public string sContractorProcessNumber { get; set; }
        public Nullable<bool> bnIsActive { get; set; }
        public Nullable<bool> bnIsFreeze { get; set; }
        public Nullable<bool> bnIsDelete { get; set; }
        public string sContractorName { get; set; }
        public string iContractorNationalNumber { get; set; }
        public Nullable<bool> bSubContractorAccept { get; set; }
        public Nullable<System.DateTime> dateAccept { get; set; }
        public Nullable<int> userCodeAccept { get; set; }
        public string ipAcceptSubContractor { get; set; }
        #endregion



        /// <summary>
        ///   Get Object Of Sub Contractor In Process.
        /// </summary>
        /// <param name="Id"> Sub Contractor Code In Process. </param>
        /// <returns> Process Sub Contractor Model. </returns>
        internal override ProcessSubContractorModel GetById(int Id)
        {
            processSubContractor OProcessSubContractoer = db.processSubContractors.FirstOrDefault(x => x.processSubContractorCode == Id);
            ProcessSubContractorModel oProcessSubContractorModel = new ProcessSubContractorModel();

            if (OProcessSubContractoer != null)
            {
                oProcessSubContractorModel = this.ConvertEFToObjectBasic(OProcessSubContractoer);
            }
            return oProcessSubContractorModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ProcessSubContractorModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get All Sub Contractors In Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> List Of Process Sub Contractors Model. </returns>
        internal override List<ProcessSubContractorModel> GetAll(int Id)
        {
            List<ProcessSubContractorModel> LprocessSubContractorModel = new List<ProcessSubContractorModel>();
            List<processSubContractor> LprocessSubContractorEF = db.processSubContractors.OrderBy(y => y.processSubContractorCode).Where(x => x.processCode == Id && x.isDelete == false && x.isActive == true && x.isFreeze == false).ToList();

            if (LprocessSubContractorEF != null)
            {
                LprocessSubContractorModel = this.ConvertEFsToObjectsBasic(LprocessSubContractorEF);
            }
            return LprocessSubContractorModel;
        }

        /// <summary>
        ///   Get Sub contractors By Special Process And Special Sub Contractors. 
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="Contractors"> List Of Sub Contractors Code. </param>
        /// <returns> List Of Process Sub Contractors Model. </returns>
        public List<ProcessSubContractorModel> GetAll(int Id, List<int> Contractors)
        {
            List<ProcessSubContractorModel> LprocessSubContractorModel = new List<ProcessSubContractorModel>();
            List<processSubContractor> LprocessSubContractorEF = db.processSubContractors.OrderBy(y => y.processSubContractorCode).
                                                                   Where(x => x.processCode == Id && x.isDelete == false && x.isActive == true
                                                                         && x.isFreeze == false).ToList();
            if (Contractors != null)
                LprocessSubContractorEF = LprocessSubContractorEF.Where(x => Contractors.Contains(x.processSubContractorCode)).ToList();
            if (LprocessSubContractorEF != null)
            {
                LprocessSubContractorModel = this.ConvertEFsToObjectsBasic(LprocessSubContractorEF);
            }
            return LprocessSubContractorModel;
        }

        /// <summary>
        ///   Get Main Contractor In Special Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Process Sub Contractor Model. </returns>
        internal ProcessSubContractorModel GetProcessMainContByProcessId(int Id)
        {
            processSubContractor OProcessSubContractoer = db.processSubContractors.FirstOrDefault(x => x.processCode == Id && x.contractorType == true);
            ProcessSubContractorModel oProcessSubContractorModel = new ProcessSubContractorModel();

            if (OProcessSubContractoer != null)
            {
                oProcessSubContractorModel = this.ConvertEFToObjectBasic(OProcessSubContractoer);
            }
            return oProcessSubContractorModel;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchObjs"></param>
        /// <returns></returns>
        internal override List<ProcessSubContractorModel> lSearch(List<string> searchObjs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="Lsearch"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Process Sub Contractor Model. </returns>
        public List<ProcessSubContractorModel> ListSearchContractors(List<string> Lsearch)
        {
            try
            {
                int? code = Convert.ToInt32(Lsearch[0]);
                var models = db.GetSubContractors(Lsearch[1], Lsearch[2], Lsearch[3], code, null).ToList();

                List<ProcessSubContractorModel> LgroupModel = new List<ProcessSubContractorModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        ProcessSubContractorModel oprocessSubContractorModellEF = new ProcessSubContractorModel();
                        oprocessSubContractorModellEF.iProcessSubContractorCode = item.processSubContractorCode;
                        oprocessSubContractorModellEF.iProcessCode = item.processCode; // كود العملية
                        oprocessSubContractorModellEF.iContractorCode = item.contractorCode;
                        oprocessSubContractorModellEF.bnContractorType = item.contractorType;
                        oprocessSubContractorModellEF.sContractorProcessNumber = item.contractorProcessNumber;
                        oprocessSubContractorModellEF.bnIsActive = item.isActive;
                        oprocessSubContractorModellEF.bnIsFreeze = item.isFreeze;
                        oprocessSubContractorModellEF.bnIsDelete = item.isDelete;
                        oprocessSubContractorModellEF.iContractorNationalNumber = item.referenceSideContractorInsuranceNum;
                        oprocessSubContractorModellEF.sContractorName = item.referenceSideContractorName;

                        LgroupModel.Add(oprocessSubContractorModellEF);
                    }
                }

                return LgroupModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <returns></returns>
        internal override bool bSave(ProcessSubContractorModel newObj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Save Done Or Not. </returns>
        internal override bool bSave(ProcessSubContractorModel newObj, int Id)
        {
            try
            {
                processSubContractor oProcssSubContractor = new processSubContractor();
                oProcssSubContractor.processCode = Id; // كود العملية
                oProcssSubContractor.contractorCode = newObj.iContractorCode; // كود المقاول 
                oProcssSubContractor.contractorType = newObj.bnContractorType; // نوع المقاول 0 => باطن  1=> رئيسي
                oProcssSubContractor.contractorProcessNumber = newObj.sContractorProcessNumber; // رقم المقاول بالعمليه (البند)
                oProcssSubContractor.subContractorAccept = newObj.bSubContractorAccept; // تم الموافقه عليه كمقاول رئيسي

                oProcssSubContractor.isActive = newObj.bnIsActive; // فعال (0- تم التأكيد / 1- لسه)
                oProcssSubContractor.isFreeze = newObj.bnIsFreeze; // تجميد المقاول0=> لم يتم   1=> تم التجميد
                oProcssSubContractor.isDelete = newObj.bnIsDelete; // حذف (0- حذف / 1- لسه)

                //oProcssSubContractor.isActive = newObj.bnIsActive == null ? false : newObj.bnIsActive; // فعال (0- تم التأكيد / 1- لسه)
                //oProcssSubContractor.isFreeze = newObj.bnIsFreeze == null ? false : newObj.bnIsFreeze; // تجميد المقاول0=> لم يتم   1=> تم التجميد
                //oProcssSubContractor.isDelete = newObj.bnIsDelete == null ? false : newObj.bnIsDelete; // حذف (0- حذف / 1- لسه)

                oProcssSubContractor.userInsertCode = newObj.inUserInsertCode;
                oProcssSubContractor.dateInsert = dtServerTime;
                oProcssSubContractor.ipInsert = newObj.sIpInsert;
                oProcssSubContractor.seen = false;
                oProcssSubContractor.isActive = true;
                oProcssSubContractor.isDelete = false;
                oProcssSubContractor.isFreeze = false;
                db.processSubContractors.Add(oProcssSubContractor);
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
        ///   Save Sub Contractor With His Missions In Process.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <param name="lstr"> List Of MissioCodes n Of Sub Contractor. </param>
        /// <returns> Save Done Or Not. </returns>
        public bool SaveSub(ProcessSubContractorModel newObj, int Id, List<string> lstr)
        {
            try
            {
                processSubContractor oProcssSubContractor = new processSubContractor();
                oProcssSubContractor.processCode = Id; // كود العملية
                oProcssSubContractor.contractorCode = newObj.iContractorCode; // كود المقاول 
                oProcssSubContractor.contractorType = newObj.bnContractorType; // نوع المقاول 0 => باطن  1=> رئيسي

                // مقاول من باطن لم يتم الموافقه عليه بعد
                if (newObj.bnContractorType == false)
                    oProcssSubContractor.subContractorAccept = false;

                oProcssSubContractor.contractorProcessNumber = newObj.sContractorProcessNumber; // رقم المقاول بالعمليه (البند)
                oProcssSubContractor.isActive = true; // فعال (0- تم التأكيد / 1- لسه)
                oProcssSubContractor.isFreeze = false; // تجميد المقاول0=> لم يتم   1=> تم التجميد
                oProcssSubContractor.isDelete = false; // حذف (0- حذف / 1- لسه)
                oProcssSubContractor.seen = false;
                oProcssSubContractor.userInsertCode = newObj.inUserInsertCode;
                oProcssSubContractor.dateInsert = dtServerTime;
                oProcssSubContractor.ipInsert = newObj.sIpInsert;
                oProcssSubContractor.seenEmployeeInsurance = false;
                oProcssSubContractor.seenMainContractor = false;
                db.processSubContractors.Add(oProcssSubContractor);
                if (db.SaveChanges() > 0)
                {
                    int id = oProcssSubContractor.processSubContractorCode;
                    this.bIsSaved = OmissionSubContractorModel.SaveList(id, lstr);

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///    Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ProcessSubContractorModel newObj, int Id)
        {
            try
            {
                processSubContractor oProcssSubContractor = db.processSubContractors.FirstOrDefault(x => x.processSubContractorCode == Id);
                if (oProcssSubContractor == null)
                    return false;

                oProcssSubContractor.processCode = Id; // كود العملية
                oProcssSubContractor.contractorType = newObj.bnContractorType; // نوع المقاول 0 => باطن  1=> رئيسي
                oProcssSubContractor.contractorProcessNumber = newObj.sContractorProcessNumber; // رقم المقاول بالعمليه (البند)
                oProcssSubContractor.isActive = newObj.bnIsActive; // فعال (0- تم التأكيد / 1- لسه)
                oProcssSubContractor.isFreeze = newObj.bnIsFreeze; // تجميد المقاول0=> لم يتم   1=> تم التجميد
                oProcssSubContractor.isDelete = newObj.bnIsDelete; // حذف (0- حذف / 1- لسه)

                oProcssSubContractor.userInsertCode = newObj.inUserInsertCode;
                oProcssSubContractor.dateInsert = dtServerTime;
                oProcssSubContractor.ipInsert = newObj.sIpInsert;

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
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <param name="bContractorType"> Contractor Type. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal bool bEdit(ProcessSubContractorModel newObj, int Id, bool bContractorType)
        {
            try
            {
                processSubContractor oProcssSubContractor = db.processSubContractors.FirstOrDefault(x => x.processCode == Id && bContractorType);
                if (oProcssSubContractor == null)
                    return false;

                oProcssSubContractor.contractorCode = newObj.iContractorCode;
                oProcssSubContractor.isActive = newObj.bnIsActive; // فعال (0- تم التأكيد / 1- لسه)
                oProcssSubContractor.isFreeze = newObj.bnIsFreeze; // تجميد المقاول0=> لم يتم   1=> تم التجميد
                oProcssSubContractor.isDelete = newObj.bnIsDelete; // حذف (0- حذف / 1- لسه)
                oProcssSubContractor.userUpdateCode = newObj.inUserUpdateCode;
                oProcssSubContractor.dateUpdate = dtServerTime;
                oProcssSubContractor.ipUpdate = newObj.sIpUpdate;

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
        ///   Edit Sub Contractor With His Missions.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Process Code. </param>
        /// <param name="lstr"> List Of MissioCodes n Of Sub Contractor. </param>
        /// <returns> Edit Done Or Not. </returns>
        public bool EditSub(ProcessSubContractorModel newObj, int Id, List<string> lstr)
        {
            try
            {
                processSubContractor oProcssSubContractor = db.processSubContractors.FirstOrDefault(x => x.processSubContractorCode == newObj.iProcessSubContractorCode);
                if (oProcssSubContractor == null)
                    return false;

                oProcssSubContractor.processCode = Id; // كود العملية
                oProcssSubContractor.contractorCode = newObj.iContractorCode; // كود المقاول 
                oProcssSubContractor.contractorType = newObj.bnContractorType; // نوع المقاول 0 => باطن  1=> رئيسي
                oProcssSubContractor.contractorProcessNumber = newObj.sContractorProcessNumber; // رقم المقاول بالعمليه (البند)
                oProcssSubContractor.userUpdateCode = newObj.inUserUpdateCode;
                oProcssSubContractor.dateInsert = dtServerTime;
                oProcssSubContractor.ipInsert = newObj.sIpUpdate;
                if (db.SaveChanges() > 0)
                {
                    IEnumerable<missionSubContractor> list = db.missionSubContractors.Where(x => x.processSubContractorCode == newObj.iProcessSubContractorCode).ToList();
                    db.missionSubContractors.RemoveRange(list);
                    if (db.SaveChanges() > 0)
                    {
                        this.bIsEdit = OmissionSubContractorModel.SaveList(newObj.iProcessSubContractorCode, lstr);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///   Accept Sub Contractor In Process.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> String. </returns>
        internal string bAcceptSubContractor(ProcessSubContractorModel newObj)
        {
            try
            {
                var acceptSubContractor = db.spAcceptSubContractor(newObj.iProcessCode, newObj.iProcessSubContractorCode, newObj.userCodeAccept, newObj.ipAcceptSubContractor).ToList();

                if (acceptSubContractor.Count > 0)
                    return acceptSubContractor[0];

                return null;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        ///   Delete Special Sub Contractor In Process.
        /// </summary>
        /// <param name="Id"> Code Of Process Sub Contractor Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                int y = 0;
                IEnumerable<missionSubContractor> list = db.missionSubContractors.Where(x => x.processSubContractorCode == Id).ToList();
                db.missionSubContractors.RemoveRange(list);
                if (db.SaveChanges() > 0)
                {
                    db.processSubContractors.Remove(db.processSubContractors.FirstOrDefault(x => x.processSubContractorCode == Id));
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


        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Process Sub Contractor. </param>
        /// <returns> List Of Process Sub Contractor Model. </returns>
        internal override List<ProcessSubContractorModel> ConvertEFsToObjectsBasic(List<processSubContractor> lEf)
        {
            List<ProcessSubContractorModel> LprocessSubContractorModel = new List<ProcessSubContractorModel>();
            if (lEf != null)
            {
                foreach (processSubContractor oEF in lEf)
                {
                    LprocessSubContractorModel.Add(this.ConvertEFToObjectBasic(oEF));
                }
            }
            return LprocessSubContractorModel;
        }

        /// <summary>
        ///    Convert From Entity Framework To Model .
        /// </summary>
        /// <param name="lEf"> Entity Framework Of Process Sub Contractor. </param>
        /// <returns> Model Of Process Sub Contractor. </returns>
        internal override ProcessSubContractorModel ConvertEFToObjectBasic(processSubContractor lEf)
        {
            ProcessSubContractorModel oProcessSubContractorModel = new ProcessSubContractorModel();
            if (lEf != null)
            {
                oProcessSubContractorModel.iProcessSubContractorCode = lEf.processSubContractorCode; // كود المقاول  بالعمليه
                oProcessSubContractorModel.iProcessCode = lEf.processCode; // كود العملية
                oProcessSubContractorModel.iContractorCode = lEf.contractorCode; // عقار المقاول بالعمليه
                oProcessSubContractorModel.bnContractorType = lEf.contractorType; // نوع المقاول
                oProcessSubContractorModel.sContractorProcessNumber = lEf.contractorProcessNumber; // رقم المقاول بالعمليه
                oProcessSubContractorModel.bnIsActive = lEf.isActive; // تفعيل المقاول
                oProcessSubContractorModel.bnIsFreeze = lEf.isFreeze; // موقوف ولا
                oProcessSubContractorModel.bnIsDelete = lEf.isDelete; // ممسوح ولا
                oProcessSubContractorModel.iContractorNationalNumber = lEf.referenceSideContractor.referenceSideContractorInsuranceNum; // الرقم التأمينى للمقاول
                oProcessSubContractorModel.sContractorName = lEf.referenceSideContractor.referenceSideContractorName; // اسم المقاول 
                oProcessSubContractorModel.inUserInsertCode = lEf.userInsertCode; // كود مدخل البيانات
                oProcessSubContractorModel.bSubContractorAccept = lEf.subContractorAccept; // تم الموافقه على هذا المقاول ولا 

            }
            return oProcessSubContractorModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override processSubContractor ConvertObjectToEF(ProcessSubContractorModel bl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessSubContractorModel> ConvertEFsToObjects(List<processSubContractor> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessSubContractorModel ConvertEFToObject(processSubContractor ef)
        {
            throw new NotImplementedException();
        }

    }
}
