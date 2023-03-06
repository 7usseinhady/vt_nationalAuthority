using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of Extracts. 
    /// </summary>
    public class ExtractsModel : ModelBase<ExtractsModel, extractProcess> // المستخلصات
    {
        #region Extracts Process Fields مستخلصات العمليه
        public int iExtractProcessCode { get; set; } // كود مستخلص العمليه
        [Required(ErrorMessage = "برجاء ادخال اسم المستخلص ")]
        public string sExtractName { get; set; } // اسم المستخلص
        public int iProcessCode { get; set; } // كود العمليه الرئيسيه
        public Nullable<int> inOfficeCode { get; set; } // كود المكتب
        public string sOfficeName { get; set; } //اسم المكتب
        public string sOfficeID { get; set; } // الرقم التعريفى للمكتب
        public Nullable<int> inAreaCode { get; set; } //  كود المنطقه 
        public string sAreaName { get; set; } //اسم المنطقه
        public string sAreaID { get; set; } // الرقم التعريفى المنطقه
        public string sExtractProcessID { get; set; } // رقم المستخلص
        [Required(ErrorMessage = "برجاء ادخال المبلغ ")]
        public Nullable<decimal> dnBudget { get; set; } // المبلغ المدفوع
        public Nullable<int> inExtractTypeCode { get; set; } //  نوع المستخلص 0=> دورى 1=> ختامى
        public string sExtractTypeName { get; set; } //  اسم نوع المستخلص
        [Required(ErrorMessage = "برجاء اختيار المقاول ")]
        public Nullable<int> inProcessSubContractorCode { get; set; } // كود المقاول اللى انهى المستخلص
        public string sProcessSubContractorName { get; set; } // اسم المقاول اللى انهى المستخلص
        [Required(ErrorMessage = "برجاء اختيار المهمه المنتهيه ")]
        public Nullable<int> inMissionExpiredTypeCode { get; set; } //كود المهمه المستنده اليه 
        public string sMissionExpiredTypeName { get; set; } //اسم المهمه المستنده اليه 
        public string sRecieverContractor { get; set; } // مندوب المقاول
        [Required(ErrorMessage = "برجاء ادخال تاريخ المستخلص ")]
        public string sDateExtract { get; set; } // تاريخ المستخلص
        public Nullable<bool> bIsPaid { get; set; } // مسدد - غير مسدد
        public Nullable<bool> bIsDelete { get; set; } // 0=> لم يتم الحذف 1=> تم الحذف
        public Nullable<bool> bnAcceptExtract { get; set; } // التأكيد (0- لم يتم التأكيد / 1- تم التأكيد)
        public Nullable<int> inUserAcceptExtract { get; set; } // كود الموظف اللى أكد على المستخلص
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        ///   Get All Extracts.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> List Of Extracts Model. </returns>
        internal override List<ExtractsModel> GetAll(int Id)
        {
            List<spGetSpecialExtract_Result> LExtractProcessEF = db.spGetSpecialExtract(-1, Id, "-1", "-1", "", "", -1, "-1", "-1", new DateTime(1, 1, 1), new DateTime(1, 1, 1), "-1").ToList();

            List<ExtractsModel> LExtractsModel = new List<ExtractsModel>();
            if (LExtractProcessEF != null)
                LExtractsModel = this.ConvertEFsToObjectsBasic(LExtractProcessEF);

            return LExtractsModel;
        }

        /// <summary>
        ///    Get All Extracts 'Contractor'.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="uc"> User Code. </param>
        /// <returns> List Of Extracts Model. </returns>
        internal List<ExtractsModel> GetAll(int Id, string uc)
        {
            int? contractCode = 0;

            // المقاول
            if (!String.IsNullOrEmpty(uc))
            {
                int userCode = Convert.ToInt32(uc);
                UserModel user = new UserModel().GetById(userCode);
                contractCode = Convert.ToInt32(user.iContractorCode.ToString());

                var processSubContractors = db.processSubContractors.FirstOrDefault(x => x.processCode == Id && x.contractorCode == contractCode);

                if (processSubContractors != null)
                {
                    contractCode = processSubContractors.processSubContractorCode;
                }
                else
                    contractCode = 0;
            }

            List<spGetSpecialExtract_Result> LExtractProcessEF = db.spGetSpecialExtract(-1, Id, "-1", "-1", "", "", -1, "-1", "-1", new DateTime(1, 1, 1), new DateTime(1, 1, 1), contractCode.ToString()).ToList();

            List<ExtractsModel> LExtractsModel = new List<ExtractsModel>();
            if (LExtractProcessEF != null)
                LExtractsModel = this.ConvertEFsToObjectsBasic(LExtractProcessEF);

            return LExtractsModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ExtractsModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get Object Of Extract.
        /// </summary>
        /// <param name="Id"> Extract Code. </param>
        /// <returns> Extract Model. </returns>
        internal override ExtractsModel GetById(int Id)
        {
            ExtractsModel OExtractsModel = new ExtractsModel();
            spGetSpecialExtract_Result OExtractProcess = db.spGetSpecialExtract(Id, -1, "-1", "-1", "", "", -1, "-1", "-1", new DateTime(1, 1, 1), new DateTime(1, 1, 1), "-1").FirstOrDefault();
            if (OExtractProcess != null)
                OExtractsModel = this.ConvertEFToObjectBasic(OExtractProcess);

            return OExtractsModel;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Reasons Of Extracts Model. </returns>
        internal override List<ExtractsModel> lSearch(List<string> searchObjs)
        {
            try
            {
                int extractProcessCode = Convert.ToInt32(searchObjs[0]); // كود المستخلص
                int processCode = Convert.ToInt32(searchObjs[1]); // كود العمليه
                decimal dBudget = Convert.ToDecimal(searchObjs[6]); // قيمه المستخلص
                DateTime? dtDateExtract = Convert.ToDateTime(searchObjs[9]); // تاريخ المستخلص
                DateTime? dtDateExtractTo = String.IsNullOrEmpty(searchObjs[10].ToString()) ? new DateTime(2100, 1, 1) : Convert.ToDateTime(searchObjs[10]); // تاريخ المستخلص

                // مقاول
                if (searchObjs[11].Length >= 4 && searchObjs[11].Substring(0, 4) == "cont")
                {
                    int userCode = Convert.ToInt32(searchObjs[11].Substring(4));
                    UserModel user = new UserModel().GetById(userCode);
                    int? contractCode = Convert.ToInt32(user.iContractorCode.ToString());
                    var processSubContractors = db.processSubContractors.FirstOrDefault(x => x.processCode == processCode && x.contractorCode == contractCode);
                    if (processSubContractors != null)
                        searchObjs[11] = processSubContractors.processSubContractorCode.ToString(); // كود المقاول 
                    else
                        searchObjs[11] = "-1";
                }

                var LExtractProcessEF = db.spGetSpecialExtract(extractProcessCode, processCode, searchObjs[2], searchObjs[3], searchObjs[4], searchObjs[5], dBudget, searchObjs[7], searchObjs[8], dtDateExtract, dtDateExtractTo, searchObjs[11]).ToList();

                List<ExtractsModel> LExtractsModel = new List<ExtractsModel>();
                if (LExtractProcessEF != null)
                    LExtractsModel = this.ConvertEFsToObjectsBasic(LExtractProcessEF);

                return LExtractsModel;
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
        internal override bool bSave(ExtractsModel newObj)
        {
            try
            {
                extractProcess modal = new extractProcess();
                modal.processCode = newObj.iProcessCode; // كود العمليه الرئيسية
                modal.officeInsuranceCode = newObj.inOfficeCode; // كود المكتب

                // المقاول
                if (newObj.inProcessSubContractorCode == null)
                {
                    int userCode = Convert.ToInt32(newObj.inUserInsertCode.ToString());
                    UserModel user = new UserModel().GetById(userCode);
                    int? contractCode = Convert.ToInt32(user.iContractorCode.ToString());
                    var processMainContractors = db.processSubContractors.FirstOrDefault(x => x.processCode == newObj.iProcessCode && x.contractorCode == contractCode);
                    if (processMainContractors != null)
                        modal.processSubContractorCode = processMainContractors.processSubContractorCode; // كود المقاول اللى انهى المستخلص
                }
                else // موظف التأمينات
                {
                    modal.processSubContractorCode = newObj.inProcessSubContractorCode;
                    var user = db.users.FirstOrDefault(x => x.userCode == newObj.inUserInsertCode);
                    if (user != null)
                        modal.officeInsuranceCode = user.officeInsuranceCode;// كود المكتب الخاص بالموظف
                }

                modal.extractName = newObj.sExtractName; // اسم المستخلص

                // رقم المستخلص
                if (String.IsNullOrEmpty(newObj.sExtractProcessID))
                    modal.extractProcessID = null;
                else
                    modal.extractProcessID = Convert.ToInt32(newObj.sExtractProcessID);

                modal.budget = newObj.dnBudget; // المبلغ المدفوع
                modal.extractTypeCode = newObj.inExtractTypeCode; //  نوع المستخلص 0=> دورى 1=> ختامى
                modal.missionExpiredTypeCode = newObj.inMissionExpiredTypeCode; //كود المهمه المستنده اليه
                modal.recieverContractor = newObj.sRecieverContractor; // مندوب المقاول

                // تاريخ المستخلص
                if (String.IsNullOrEmpty(newObj.sDateExtract))
                    modal.dateExtract = null;
                else
                    modal.dateExtract = Convert.ToDateTime(newObj.sDateExtract);

                modal.userInsertCode = newObj.inUserInsertCode; // كود المدخل
                modal.dateInsert = dtServerTime; // وقت الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                db.extractProcesses.Add(modal);
                if (db.SaveChanges() > 0)
                    return true;

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
        internal override bool bSave(ExtractsModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Extract Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ExtractsModel newObj, int Id)
        {
            try
            {
                extractProcess modal = db.extractProcesses.FirstOrDefault(x => x.extractProcessCode == Id);
                if (modal != null)
                {
                    modal.processCode = newObj.iProcessCode; // كود العمليه الرئيسية
                    modal.officeInsuranceCode = newObj.inOfficeCode; // كود المكتب
                    modal.extractName = newObj.sExtractName; // اسم المستخلص

                    // رقم المستخلص
                    if (String.IsNullOrEmpty(newObj.sExtractProcessID))
                        modal.extractProcessID = null;
                    else
                        modal.extractProcessID = Convert.ToInt32(newObj.sExtractProcessID);

                    // المقاول
                    if (newObj.inProcessSubContractorCode != null) // موظف التأمينات
                        modal.processSubContractorCode = newObj.inProcessSubContractorCode;

                    modal.budget = newObj.dnBudget; // المبلغ المدفوع
                    modal.extractTypeCode = newObj.inExtractTypeCode; //  نوع المستخلص 0=> دورى 1=> ختامى
                    modal.missionExpiredTypeCode = newObj.inMissionExpiredTypeCode; //كود المهمه المستنده اليه
                    modal.recieverContractor = newObj.sRecieverContractor; // مندوب المقاول

                    // تاريخ المستخلص
                    if (String.IsNullOrEmpty(newObj.sDateExtract))
                        modal.dateExtract = null;
                    else
                        modal.dateExtract = Convert.ToDateTime(newObj.sDateExtract);

                    modal.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                    modal.dateUpdate = dtServerTime; // تاريخ التعديل
                    modal.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                    if (db.SaveChanges() > 0)
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///   Accept Extract.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <returns> Accept Done Or Not. </returns>
        internal bool bAcceptExtract(ExtractsModel newObj) // Accept Extract التأكيد على المسنخلص
        {
            try
            {
                extractProcess modal = db.extractProcesses.FirstOrDefault(x => x.extractProcessCode == newObj.iExtractProcessCode);

                if (modal != null)
                {
                    modal.extractAccept = true;// تم التأكيد على المستخلص
                    modal.dateAccept = dtServerTime; // تاريخ التأكيد
                    modal.userCodeAccept = newObj.inUserUpdateCode; // كود الموظف اللى أكد على الستخلص
                    modal.ipAcceptExtract = newObj.sIpInsert; // عنوان الجهاز اثناء التأكيد على المستخلص 

                    if (db.SaveChanges() > 0)
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///   Confirm Of Payment On Extract.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <returns> Confirm Done Or Not. </returns>
        internal bool bPaidExtract(ExtractsModel newObj) // التأكيد تسديد الاشتراك على المستخلص
        {
            try
            {
                extractProcess modal = db.extractProcesses.FirstOrDefault(x => x.extractProcessCode == newObj.iExtractProcessCode);

                if (modal != null)
                {
                    modal.isPaid = true;// تم التأكيد تسديد الاشتراك على المستخلص
                    modal.userUpdateCode = newObj.inUserUpdateCode; // كود موظف التعديل
                    modal.dateUpdate = dtServerTime; // تاريخ التعديل
                    modal.ipUpdate = newObj.sIpUpdate; // عنوان الجهاز فى التعديل

                    if (db.SaveChanges() > 0)
                        return true;

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
        /// <param name="Id"> Code Of Extract Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id)
        {
            try
            {
                extractProcess modal = db.extractProcesses.FirstOrDefault(x => x.extractProcessCode == Id);
                if (modal == null)
                    return false;
                modal.isDelete = true;

                if (db.SaveChanges() > 0)
                    return true;

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
        /// <param name="Id"> Extract Code. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="sIpUpdate"> Ip Address Update. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal bool bDelete(int Id, int uc, string sIpUpdate)
        {
            try
            {
                extractProcess modal = db.extractProcesses.FirstOrDefault(x => x.extractProcessCode == Id);
                if (modal == null)
                    return false;

                modal.isDelete = true;
                modal.userUpdateCode = uc; // كود موظف التعديل
                modal.dateUpdate = dtServerTime; // تاريخ التعديل
                modal.ipUpdate = sIpUpdate; // عنوان الجهاز فى التعديل

                if (db.SaveChanges() > 0)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }


        ///   Convert From Entity Framework To Model. 
        /// </summary>
        /// <param name="EF"> Entity Framework Of Extracts 'Stored Procedure'. </param>
        /// <returns> Model Of Extract. </returns>
        private ExtractsModel ConvertEFToObjectBasic(spGetSpecialExtract_Result EF)
        {
            ExtractsModel OExtractProcesseModel = new ExtractsModel();
            if (EF != null)
            {
                OExtractProcesseModel.iExtractProcessCode = EF.extractProcessCode; // كود المستخلص
                OExtractProcesseModel.iProcessCode = EF.processCode; // كود العمليه الرئيسية
                OExtractProcesseModel.inOfficeCode = EF.officeInsuranceCode; // كود المكتب
                OExtractProcesseModel.sOfficeID = EF.officeInsuranceID; // الرقم التعريفى للمكتب
                OExtractProcesseModel.sOfficeName = EF.officeInsuranceName; // اسم المكتب
                OExtractProcesseModel.sExtractName = EF.extractName; // اسم المستخلص
                OExtractProcesseModel.sExtractProcessID = EF.extractProcessID; // رقم المستخلص
                OExtractProcesseModel.dnBudget = EF.budget; // المبلغ المدفوع
                OExtractProcesseModel.inExtractTypeCode = EF.extractTypeCode; //  نوع المستخلص 0=> دورى 1=> ختامى
                OExtractProcesseModel.sExtractTypeName = EF.extractTypeName; // اسم نوع المستخلص
                OExtractProcesseModel.inProcessSubContractorCode = EF.processSubContractorCode; // كود المقاول اللى انهى المستخلص
                OExtractProcesseModel.sProcessSubContractorName = EF.referenceSideContractorName; // اسم المقاول اللى انهى المستخلص
                OExtractProcesseModel.inMissionExpiredTypeCode = EF.missionExpiredTypeCode; //كود المهمه المستنده اليه
                OExtractProcesseModel.sMissionExpiredTypeName = EF.processTypeName; //اسم المهمه المستنده اليه
                OExtractProcesseModel.sRecieverContractor = EF.recieverContractor; // مندوب المقاول
                OExtractProcesseModel.sDateExtract = EF.dateExtract.ToString(); // تاريخ المستخلص
                OExtractProcesseModel.bIsPaid = EF.isPaid; // مسدد - غير مسدد 
                OExtractProcesseModel.inUserAcceptExtract = EF.userCodeAccept; // كود الموظف اللى أكد على المستخلص
                OExtractProcesseModel.inUserInsertCode = EF.userInsertCode; // كود موظف الادخال
            }
            return OExtractProcesseModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Extracts 'Stored Procedure'. </param>
        /// <returns> List Of Extracts Model. </returns>
        private List<ExtractsModel> ConvertEFsToObjectsBasic(List<spGetSpecialExtract_Result> lEf)
        {
            List<ExtractsModel> LExtractsModel = new List<ExtractsModel>();

            if (lEf.Count > 0)
            {
                foreach (var item in lEf)
                {
                    ExtractsModel oextractsModel = this.ConvertEFToObjectBasic(item);
                    LExtractsModel.Add(oextractsModel);
                }
            }
            return LExtractsModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ExtractsModel ConvertEFToObject(extractProcess ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override extractProcess ConvertObjectToEF(ExtractsModel bl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override ExtractsModel ConvertEFToObjectBasic(extractProcess lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ExtractsModel> ConvertEFsToObjectsBasic(List<extractProcess> lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ExtractsModel> ConvertEFsToObjects(List<extractProcess> lEf)
        {
            throw new NotImplementedException();
        }

    }
}
