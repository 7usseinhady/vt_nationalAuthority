using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAccessLayer.Models
{
    /// <summary>
    ///   Model Of ' Processes - Expire Processes - Request Processes - Processes Of Reference side  - Processes Of Contractor '. 
    /// </summary>
    public class ProcessModel : ModelBase<ProcessModel, process> // العمليات
    {
        #region Process Fields العمليه
        public int iProcessCode { get; set; } // كود العملية
        [Required(ErrorMessage = "برجاء ادخال سنه العمليه ")]
        public Nullable<int> inProcessYear { get; set; }
        public string sAreaIDName { get; set; } // الرقم التعريفى للمنطقه
        public string sOfficeIDName { get; set; } // الرقم التعريفى للمكتب
        [Required(ErrorMessage = "برجاء ادخال اسم العمليه ")]
        public string sProcessName { get; set; } // اسم العملية
        [Required(ErrorMessage = "برجاء ادخال رقم العمليه ")]
        [MinLength(5, ErrorMessage = "رقم العمليه 5 ارقام فقط")]
        public string sProcessNumber { get; set; } // رقم العمليه (سنه / رقم العمليه)
        [Required(ErrorMessage = "برجاء اختيار نوع العمليه ")]
        public Nullable<int> iProcessTypeCode { get; set; } // كود نوع العمليه
        public string sProcessTypeName { get; set; } // اسم نوع العمليه
        public string sFullSiteAddress { get; set; } // عنوان العمليه بالكامل
        [Required(ErrorMessage = "برجاء ادخال تاريخ بدء العمليه ")]
        public string sDateStartProcess { get; set; } // تاريخ بدء العمليه (المقايسه)
        [Required(ErrorMessage = "برجاء ادخال تاريخ نهايه العمليه ")]
        public string sDateEndProcessRequired { get; set; } // تاريخ نهايه العمليه (المقايسه)
        public string sDateEndProcess { get; set; } // تاريخ نهايه العمليه (المقايسه)
        public string sRealDateStartProcess { get; set; } // تاريخ بدء العمليه (الفعلى)
        public string sRealDateEndProcess { get; set; } // تاريخ نهايه العمليه (الفعلى)
        public Nullable<int> inReferenceSideCode { get; set; } // كود جهه الاسناد
        [Required(ErrorMessage = "برجاء ادخال الرقم التأمينى لجهه الاسناد ")]
        public string sReferanceSideNum { get; set; } // اسم جهه الاسناد
        public Nullable<int> iReferanceSideLegalEntityCode { get; set; } // الكيان القانونى لجهه الاسناد
        public string sReferanceSideLegalEntityName { get; set; } // الكيان القانونى لجهه الاسناد
        public string sReferanceSideName { get; set; } // الرقم التأمينى لجهه الاسناد
        public Nullable<int> inContractorCode { get; set; } // كود المقاول الرئيسي
        public string sContractorName { get; set; } // اسم المقاول الرئيسي
        public Nullable<int> iContractorLegalEntityCode { get; set; } // الكيان القانونى للمقاول الرئيسي
        public string sContractorLegalEntityName { get; set; } // الكيان القانونى للمقاول الرئيسي
        [Required(ErrorMessage = "برجاء ادخال رقم التأمينى للمقاول الرئيسي ")]
        public string sContractorNum { get; set; } // رقم المقاول الرئيسي
        public string sIncommingNumber { get; set; } // رقم الوارد
        public string sMoband { get; set; } // مبنده - غير مبنده
        public Nullable<bool> bnIsLimited { get; set; } // محدده المده - غير محدده المده
        public Nullable<Decimal> dnFullBudget { get; set; } // المبلغ الكلى للعمليه (مقايسه)
        public Nullable<Decimal> dnRealFullBudget { get; set; } // المبلغ الكلى للعمليه (الفعلى)
        public Nullable<int> inDocumentTypeCode { get; set; } // كود نوع المستند
        public string sDateDocument { get; set; } // تاريخ المستند
        public Nullable<int> inMinNumber { get; set; } // الحد الادنى
        public Nullable<int> inMaxNumber { get; set; } // الحد الاقصى
        public string sNameOwnerPermision { get; set; } // اسم صاحب الترخيص
        public string sAaddressOwnerPermision { get; set; } // عنوان صاحب الترخيص
        public Nullable<bool> bnProcessAccept { get; set; } // الموافق (0- لم يتم الموافقه - 1- تم الموافقه)
        public Nullable<DateTime> dtDateAccept { get; set; } // تاريخ الموافقه
        public Nullable<int> inUserCodeAccept { get; set; } // كود الموظف اللى وافق ع العملية
        public string sColor { get; set; } // لون العمليه
        public Nullable<int> inProcessUser { get; set; } // عدد مستخدمين العمليه
        public string sRegisterBySelf { get; set; } // تم تسجيل العمليه عن طريق (المستخدم بنفسه - ولا حد سجله - ولا الداتا اللى راجعه من ال         
        public string sProcessOpposition { get; set; } // ليها اعترض / اعفاء ولا
        public string sProcessNotes { get; set; } // ليها ملاحظات ولا
        public string sOfficeId { get; set; } // id مكتب
        public string sOfficeName { get; set; } //اسم المكتب
        [Required(ErrorMessage = "برجاء اختيار المنطقه ")]
        public Nullable<int> iAreaCode { get; set; } // كود نوع العمليه
        public string sAreaId { get; set; } // id المنطقه
        public string sAreaName { get; set; } // اسم المنطقه
        public string sProcessStopNotActive { get; set; } // العمليه موقوفه - مثبته
        public string sWorkerAlreadyNumber { get; set; }
        public string sContractorInsuranceNumber { get; set; }
        public string sRefernceInsuranceNumber { get; set; }
        public string sRealFullBudget { get; set; }
        [Required(ErrorMessage = "برجاء اختيار المكتب ")]
        public Nullable<int> iOfficeCode { get; set; }
        public string sDateInsert { get; set; }
        public string sProNum1 { get; set; }
        public string sProNum2 { get; set; }
        public string sProNum3 { get; set; }
        public string sProNum4 { get; set; }
        public string sReferneceSideName { get; set; }
        public string sReferenceLegalName { get; set; }
        public Nullable<bool> bIsLimited { get; set; }
        public Nullable<decimal> decFullBudget { get; set; }
        public string sDocumentName { get; set; }
        [Required(ErrorMessage = "برجاء ادخال الحد الادنى للعمال ")]
        public Nullable<int> iMinNumber { get; set; }
        public string sAddressOwnerPerm { get; set; }
        [Required(ErrorMessage = "برجاء ادخال الحد الاقصى  للعمال ")]
        public Nullable<int> iMaxNumber { get; set; }
        public string sContractorLegalName { get; set; }
        public string sContractorProcessNumber { get; set; }
        public string sContractorPhone { get; set; }
        public string sContractorEmail { get; set; }
        public string sContractorFax { get; set; }
        public string sContractorCount { get; set; }
        public string sProcessSite { get; set; }
        #endregion

        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override List<ProcessModel> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Get All Processes ' Reference Side - Contractor'. 
        /// </summary>
        /// <param name="Id"> User Code. </param>
        /// <returns> List Of Process Model. </returns>
        internal override List<ProcessModel> GetAll(int Id) // Contractors المقاولين
        {
            var LDisplayProcessEF = db.spGetProcessesRefSideCont(Id, "", "", "", "-1", "-1", "-1", "-1", 2, -1, new DateTime(1, 1, 1), new DateTime(1, 1, 1), -1, null, "-1", "-1", "-1", -1, null, "-1", "-1", "-1", "", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "", "-1", "", "-1", "", "", "", new DateTime(1, 1, 1)).ToList();

            List<ProcessModel> LProcessModel = new List<ProcessModel>();
            if (LDisplayProcessEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LDisplayProcessEF);

            return LProcessModel;
        }

        /// <summary>
        ///    Get Object Of Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> List Of Process Model. </returns>
        internal override ProcessModel GetById(int Id) // Edit Proccess (Contractors)
        {
            ProcessModel OProcessModel = new ProcessModel();

            var OProcess = db.spGetProcess(Id).FirstOrDefault();
            if (OProcess != null)
                OProcessModel = this.ConvertEFToObjectBasic(OProcess);

            return OProcessModel;
        }

        /// <summary>
        ///   Get All Processses.
        /// </summary>
        /// <param name="Id"> User Code. </param>
        /// <returns> List Of Process Model. </returns>
        internal List<ProcessModel> vGetOfficeProcess(string Id) // Process Office عمليات موظف التأمينات  
        {
            string[] lString = Id.Split(',');
            int uc = Convert.ToInt32(lString[0]);
            var LProcessOfficeEF = db.spGetOfficeProcess(uc, "-1").ToList();

            List<ProcessModel> LProcessModel = new List<ProcessModel>();
            if (LProcessOfficeEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LProcessOfficeEF);

            return LProcessModel;
        }

        /// <summary>
        ///   Get All Request Processes.
        /// </summary>
        /// <param name="Id"> User Code. </param>
        /// <returns> List Of Process Model. </returns>
        public List<ProcessModel> vGetRequestProcesses(string Id)// Request Process طلبات العمليات
        {
            int uc = Convert.ToInt32(Id);
            var LRequestProcessEF = db.spGetRequestProcess(uc, "").ToList();

            List<ProcessModel> LProcessModel = new List<ProcessModel>();
            if (LRequestProcessEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LRequestProcessEF);

            return LProcessModel;
        }

        /// <summary>
        ///   Get All Expire Processes.
        /// </summary>
        /// <param name="Id"> User Code. </param>
        /// <returns> List Of Process Model.  </returns>
        public List<ProcessModel> vGetExpireProcesses(string Id) // Expire Process العمليات الموشكه على الانتهاء
        {
            var LExpireProcessEF = new List<spGetExpireProcesses_Result>();
            List<ProcessModel> LProcessModel = this.ConvertEFsToObjectsBasic(LExpireProcessEF);

            return LProcessModel;
        }

        /// <summary>
        ///   Check If Process Active Or Not.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <returns> String. </returns>
        internal string GetProcessStopNotActive(int Id) // Edit Proccess (Contractors)
        {
            var count = db.spGetProcessStopNotActive(Id).FirstOrDefault();

            if (count == "1")
                return "عمليه منتهيه";
            else if (count == "2")
                return "عمليه موقوفه";

            return null;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        internal override List<ProcessModel> lSearch(List<string> searchObjs) // Search Contractors بحث المقاولين
        {
            try
            {
                int uc = Convert.ToInt32(searchObjs[0]);
                byte? isLimit = Convert.ToByte(searchObjs[7]);
                decimal? fullBudget = Convert.ToDecimal(searchObjs[8]);
                DateTime? dtStartProcess = Convert.ToDateTime(searchObjs[9]);
                DateTime? dtEndProcess = Convert.ToDateTime(searchObjs[10]);
                DateTime? dtDocument = Convert.ToDateTime(searchObjs[38]);
                int? siteBuildingNum = Convert.ToInt32(searchObjs[11]);
                int? userLetterBuildingNum = Convert.ToInt32(searchObjs[16]);

                var LDisplayProcessEF = db.spGetProcessesRefSideCont(uc, "", searchObjs[1], searchObjs[2], searchObjs[3], searchObjs[4], searchObjs[5], searchObjs[6], isLimit, fullBudget, dtStartProcess, dtEndProcess, siteBuildingNum, searchObjs[12], searchObjs[13], searchObjs[14], searchObjs[15], userLetterBuildingNum,
                    searchObjs[17], searchObjs[18], searchObjs[19], searchObjs[20], searchObjs[21], searchObjs[22], searchObjs[23], searchObjs[24], searchObjs[25], searchObjs[26], searchObjs[27], searchObjs[28]
                    , searchObjs[29], searchObjs[30], searchObjs[31], searchObjs[32], searchObjs[33], searchObjs[34], searchObjs[35], searchObjs[37], dtDocument).ToList();

                List<ProcessModel> LProcessModel = new List<ProcessModel>();
                if (LDisplayProcessEF != null)
                    LProcessModel = this.ConvertEFsToObjectsBasic(LDisplayProcessEF);

                return LProcessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Search With Special Parameters 'Notification'.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        internal List<ProcessModel> osearchNotify(List<string> searchObjs) // Contractors  المقاولين عن طريق المزيد بالاشعارات
        {
            int uc = Convert.ToInt32(searchObjs[0]);
            var LDisplayProcessEF = db.spGetProcessesRefSideCont(uc, searchObjs[1], "", "", "-1", "-1", "-1", "-1", 2, -1, new DateTime(1, 1, 1), new DateTime(1, 1, 1), -1, null, "-1", "-1", "-1", -1, null, "-1", "-1", "-1", "", "", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "", "-1", "", "-1", "", "", "", new DateTime(1, 1, 1)).ToList();

            List<ProcessModel> LProcessModel = new List<ProcessModel>();
            if (LDisplayProcessEF != null)
                LProcessModel = this.ConvertEFsToObjectsBasic(LDisplayProcessEF);

            return LProcessModel;
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        internal List<ProcessModel> lSearchOfficeProcess(List<string> searchObjs) // Search  Office Process بحث عمليات موظف التأمينات  
        {
            try
            {
                int uc = Convert.ToInt32(searchObjs[0]);
                var LOfficeProcessEF = db.spGetOfficeProcess(uc, searchObjs[1]).ToList();

                List<ProcessModel> LProcessModel = new List<ProcessModel>();
                if (LOfficeProcessEF != null)
                    LProcessModel = this.ConvertEFsToObjectsBasic(LOfficeProcessEF);

                return LProcessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        internal List<ProcessModel> lSearchRequestProcess(List<string> searchObjs) // Search  Request Process بحث طلبات العمليات  
        {
            try
            {
                int uc = Convert.ToInt32(searchObjs[0]);
                var LRequestProcessEF = db.spGetRequestProcess(uc, searchObjs[1]).ToList();

                List<ProcessModel> LProcessModel = new List<ProcessModel>();
                if (LRequestProcessEF != null)
                    LProcessModel = this.ConvertEFsToObjectsBasic(LRequestProcessEF);

                return LProcessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> List Of Process Model. </returns>
        internal List<ProcessModel> lSearchExpireProcess(List<string> searchObjs) // Expire Process العمليات الموشكه على الانتهاء
        {
            try
            {
                int uc = Convert.ToInt32(searchObjs[0]);

                var LDisplayProcessEF = db.spGetExpireProcesses(uc, searchObjs[1]).ToList();
                List<ProcessModel> LProcessModel = new List<ProcessModel>();
                if (LDisplayProcessEF != null)
                    LProcessModel = this.ConvertEFsToObjectsBasic(LDisplayProcessEF);

                return LProcessModel;
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
        internal override bool bSave(ProcessModel newObj) // Contractors المقاولين
        {
            try
            {
                process modal = new process();
                modal.processName = newObj.sProcessName; // اسم العملية
                modal.processTypeCode = newObj.iProcessTypeCode; // كود نوع العملية
                modal.officeInsuranceCode = newObj.iOfficeCode; // كود مكتب 
                if (newObj.sDateStartProcess != null)
                    modal.dateStartProcess = Convert.ToDateTime(newObj.sDateStartProcess); // تاريخ بدء العمليه (المقايسه)
                if (newObj.sDateEndProcess != null)
                    modal.dateEndProcess = Convert.ToDateTime(newObj.sDateEndProcess); // تاريخ نهايه العمليه (المقايسه)

                // جهه الاسناد اللى دخلت
                if (newObj.inReferenceSideCode == null || newObj.inReferenceSideCode == 0)
                {
                    int userCode = Convert.ToInt32(newObj.inUserInsertCode.ToString());
                    UserModel user = new UserModel().GetById(userCode);
                    modal.referenceSideCode = Convert.ToInt32(user.iReferenceSideCode.ToString());
                }
                else
                    modal.referenceSideCode = newObj.inReferenceSideCode;

                modal.isLimited = newObj.bnIsLimited;  // محدده المده - غير محدده المده
                modal.fullBudget = newObj.bnIsLimited == true ? newObj.dnFullBudget : null;  // المبلغ الكلى للعمليه (مقايسه)
                modal.documentTypeCode = newObj.inDocumentTypeCode; // كود نوع المستند
                if (newObj.sDateDocument != null)
                    modal.dateDocument = Convert.ToDateTime(newObj.sDateDocument); // تاريخ المستند
                modal.nameOwnerPermision = newObj.sNameOwnerPermision; // اسم صاحب الترخيص
                modal.addressOwnerPermision = newObj.sAaddressOwnerPermision; // عنوان صاحب الترخيص

                modal.userInsertCode = newObj.inUserInsertCode; // كود موظف الادخال
                modal.dateInsert = dtServerTime; // تاريخ الادخال
                modal.ipInsert = newObj.sIpInsert; // عنوان الجهاز فى الادخال

                modal.seenProcessEmployeeProcess = false;
                modal.seen = false;
                modal.seenAcceptProcess = false;
                modal.seenProcessEmployee = false;

                db.processes.Add(modal);
                if (db.SaveChanges() > 0)
                {
                    this.iProcessCode = modal.processCode; // كود العملية
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
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        internal override bool bSave(ProcessModel newObj, int Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="newObj"> Edit Model. </param>
        /// <param name="Id"> Code Of Process Will Be Edit. </param>
        /// <returns> Edit Done Or Not. </returns>
        internal override bool bEdit(ProcessModel newObj, int Id) // Contractors المقاولين
        {
            try
            {
                process modal = db.processes.FirstOrDefault(x => x.processCode == Id);

                if (modal != null)
                {
                    modal.processName = newObj.sProcessName; // اسم العملية
                    modal.processTypeCode = newObj.iProcessTypeCode; // كود نوع العملية
                    modal.officeInsuranceCode = newObj.iOfficeCode; // كود المكتب

                    if (newObj.sDateStartProcess != null)
                        modal.dateStartProcess = Convert.ToDateTime(newObj.sDateStartProcess); // تاريخ بدء العمليه (المقايسه)
                    if (newObj.sDateEndProcess != null)
                        modal.dateEndProcess = Convert.ToDateTime(newObj.sDateEndProcess);  // تاريخ نهايه العمليه (المقايسه)
                    else
                        modal.dateEndProcess = null;

                    // جهه الاسناد اللى دخلت
                    if (newObj.inReferenceSideCode != null && newObj.inReferenceSideCode != 0)
                        modal.referenceSideCode = newObj.inReferenceSideCode;

                    modal.isLimited = newObj.bnIsLimited; // محدده المده
                    modal.fullBudget = newObj.bnIsLimited == true ? newObj.dnFullBudget : null; // المبلغ الكلى للعمليه (مقايسه)
                    modal.documentTypeCode = newObj.inDocumentTypeCode; // كود نوع المستند
                    if (newObj.sDateDocument != null)
                        modal.dateDocument = Convert.ToDateTime(newObj.sDateDocument); // تاريخ المستند
                    modal.nameOwnerPermision = newObj.sNameOwnerPermision; // اسم صاحب الترخيص
                    modal.addressOwnerPermision = newObj.sAaddressOwnerPermision; // عنوان صاحب الترخيص

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
        ///   Change Date End Process. 
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> Change Done Or Not.  </returns>
        internal bool bChangeEndDateProcess(ProcessModel newObj) //Expire Process العمليات الموشكه على الانتهاء (تعديل المده) 
        {
            try
            {
                process modal = db.processes.FirstOrDefault(x => x.processCode == newObj.iProcessCode);

                if (modal != null)
                {
                    modal.dateEndProcess = Convert.ToDateTime(newObj.sDateEndProcess); // تاريخ نهايه العمليه (المقايسه)
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
        ///   Save Data In DataBase.
        /// </summary>
        /// <param name="Objs"> List Of Details Will Be Save. </param>
        /// <returns> String. </returns>
        internal string bAcceptProcess(List<string> Objs) // Accept Process الموافقه على الطلب (طلبات العمليات)
        {
            try
            {
                int procCode = Convert.ToInt32(Objs[0]);
                int officCode = Convert.ToInt32(Objs[1]);
                int processYear = Convert.ToInt32(Objs[2]);
                int userAcceptCode = Convert.ToInt32(Objs[4]);
                var acceptprocess = db.spAcceptProcess(procCode, officCode, processYear, Objs[3], userAcceptCode, Objs[6], Objs[5]).ToList();

                if (acceptprocess.Count > 0)
                    return acceptprocess[0];

                return "رقم العمليه موجود سابقا";
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="Id"> Code Of Process Will Be Delete. </param>
        /// <returns> Delete Done Or Not. </returns>
        internal override bool bDelete(int Id) // Contractors المقاولين
        {
            try
            {
                var rowCountDelete = db.spDeleteProcess(Id);
                if (rowCountDelete != null && rowCountDelete.ToString() != "0")
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="userCode"> User Code. </param>
        /// <returns> Delete Done Or Not. </returns>
        public bool bDeleteProcess(int Id, int userCode)
        {
            try
            {
                process Oprocess = db.processes.FirstOrDefault(x => x.processCode == Id);
                if (Oprocess == null)
                    return false;

                Oprocess.deleteProcess = 1; // كود العملية
                Oprocess.userDeleteCode = userCode;
                Oprocess.dateDelete = dtServerTime;
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
        ///    Convert From Entity Framework To Model 'Stored Procedure'.
        /// </summary>
        /// <param name="EF"> Entity Framework Of Process 'Stored Procedure'. </param>
        /// <returns> Model Of Process. </returns>
        private ProcessModel ConvertEFToObjectBasic(spGetProcess_Result EF)
        {
            ProcessModel OProcessModel = new ProcessModel();
            if (EF != null)
            {
                OProcessModel.iProcessCode = EF.processCode; // كود العملية
                OProcessModel.sProcessName = EF.processName; // اسم العملية
                OProcessModel.sIncommingNumber = String.IsNullOrEmpty(EF.incommingNumber) ? "--------" : EF.incommingNumber; // رقم الوارد
                OProcessModel.sProcessNumber = String.IsNullOrEmpty(EF.processNumber) ? "-----" : EF.processNumber; // رقم العملية

                OProcessModel.sAreaIDName = EF.areaID + EF.officeInsuranceID + EF.processYear + EF.processNumber;

                OProcessModel.iProcessTypeCode = EF.processTypeCode; // نوع العملية

                // الرقم التأمينى لجهه الاسناد
                OProcessModel.inReferenceSideCode = EF.RefSideCode; // جهه الاسناد
                OProcessModel.sReferanceSideName = EF.RefSideName; //اسم جهه الاسناد
                OProcessModel.iReferanceSideLegalEntityCode = EF.legalEntityRefSide; // الرقم التأمينى لجهه الاسناد
                OProcessModel.sReferanceSideNum = EF.RefSideNum; // الرقم التأمينى لجهه الاسناد

                // المقاول الرئيسي
                OProcessModel.inContractorCode = EF.ContactorCode;  // كود المقاول الرئيسي
                OProcessModel.sContractorName = EF.ContractorName; // اسم المقاول الرئيسي
                OProcessModel.iContractorLegalEntityCode = EF.legalEntityCont;// الرقم التأمينى للمقاول الرئيسي
                OProcessModel.sContractorNum = EF.ContractorNum;// الرقم التأمينى للمقاول الرئيسي

                OProcessModel.inDocumentTypeCode = EF.documentTypeCode; // نوع المستند
                OProcessModel.sDateDocument = EF.dateDocument; // تاريخ المستند
                OProcessModel.bnIsLimited = EF.isLimited; // محدد المده
                OProcessModel.dnFullBudget = EF.fullBudget; // المبلغ الكلى للعمليه (مقايسه)
                OProcessModel.sDateStartProcess = EF.dateStartProcess; // تاريخ بدء العمليه (المقايسه)
                OProcessModel.sDateEndProcess = EF.dateEndProcess; // تاريخ نهايه العمليه (المقايسه)
                OProcessModel.sNameOwnerPermision = EF.nameOwnerPermision; // اسم صاحب الترخيص
                OProcessModel.sAaddressOwnerPermision = EF.addressOwnerPermision; // عنوان صاحب الترخيص

                OProcessModel.iOfficeCode = EF.officeInsuranceCode; // كود مكتب العمليه
                OProcessModel.iAreaCode = EF.areaCode; // كود منطقه العمليه
            }
            return OProcessModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override ProcessModel ConvertEFToObjectBasic(process lEf)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <returns></returns>
        internal override ProcessModel ConvertEFToObject(process ef)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        internal override process ConvertObjectToEF(ProcessModel bl)
        {
            throw new NotImplementedException();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessModel> ConvertEFsToObjectsBasic(List<process> lEf)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Process 'Stored Procedure'. </param>
        /// <returns> List Of Process Model. </returns>
        private List<ProcessModel> ConvertEFsToObjectsBasic(List<spGetOfficeProcess_Result> LOfficeProcessEF) // Process Office عمليات موظف التأمينات 
        {
            List<ProcessModel> LProcessModel = new List<ProcessModel>();

            if (LOfficeProcessEF.Count > 0)
            {
                foreach (var item in LOfficeProcessEF)
                {
                    ProcessModel oProcessModel = new ProcessModel();
                    oProcessModel.iProcessCode = item.processCode; // كود العمليه
                    oProcessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "--------" : item.incommingNumber; // رقم الوارد
                    oProcessModel.sAreaIDName = item.areaID; //اسم المنطقه
                    oProcessModel.sOfficeIDName = item.officeInsuranceID; //اسم المكتب
                    oProcessModel.sProcessName = item.processName; // اسم العمليه
                    oProcessModel.sProcessTypeName = item.processTypeName; // اسم نوع العملية
                    oProcessModel.sProcessNumber = String.IsNullOrEmpty(item.processNumber) ? "-----" : item.processNumber; // رقم العمليه
                    oProcessModel.sDateStartProcess = item.dateStartProcess; // تاريخ بدء العمليه (المقايسه)
                    oProcessModel.sDateEndProcess = item.dateEndProcess; // تاريخ نهايه العمليه (المقايسه)
                    oProcessModel.sRealDateStartProcess = item.realDateStartProcess; // تاريخ بدء العمليه (الفعلى)
                    oProcessModel.sRealDateEndProcess = item.realDateEndProcess; // تاريخ نهايه العمليه (الفعلى)
                    oProcessModel.dnFullBudget = item.fullBudget; // المبلغ الكلى للعمليه (مقايسه)
                    oProcessModel.dnRealFullBudget = item.realFullBudget; // المبلغ الكلى للعمليه (الفعلى
                    oProcessModel.inContractorCode = item.ContactorCode; // كود المقاول الرئيسي
                    oProcessModel.sContractorName = item.ContractorName; //اسم المقاول الرئيسي
                    oProcessModel.inReferenceSideCode = item.RefSideCode; // كود جهه الاسناد
                    oProcessModel.sReferanceSideName = item.RefSideName; // اسم جهه الاسناد
                    oProcessModel.sMoband = item.MOBAND; // مبنده - غير مبنده
                    oProcessModel.sColor = item.color; // اللون       
                    oProcessModel.inProcessUser = item.procUser;// عدد مستخدمين العمليه

                    LProcessModel.Add(oProcessModel);
                }
            }
            return LProcessModel;
        }
        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework From Process Of 'Reference Side - Contractor' 'Stored Procedure'. </param>
        /// <returns> List Of Process Model. </returns>
        private List<ProcessModel> ConvertEFsToObjectsBasic(List<spGetProcessesRefSideCont_Result> LDisplayProcessEF) // Contractors المقاولين
        {
            List<ProcessModel> LProcessModel = new List<ProcessModel>();

            if (LDisplayProcessEF.Count > 0)
            {
                foreach (var item in LDisplayProcessEF)
                {
                    ProcessModel oProcessModel = new ProcessModel();
                    oProcessModel.iProcessCode = item.processCode; // كود العمليه
                    oProcessModel.sProcessNumber = String.IsNullOrEmpty(item.processNumber) ? "-----" : item.processNumber; // رقم العمليه
                    oProcessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "--------" : item.incommingNumber; // رقم الوارد
                    oProcessModel.sAreaIDName = item.areaID; //اسم المنطقه
                    oProcessModel.sOfficeIDName = item.officeInsuranceID; //اسم المكتب
                    oProcessModel.sProcessName = item.processName; // اسم العمليه
                    oProcessModel.sProcessTypeName = item.processTypeName; // اسم نوع العملية
                    oProcessModel.sDateStartProcess = item.dateStartProcess; // تاريخ بدء العمليه (المقايسه)
                    oProcessModel.sDateEndProcess = item.dateEndProcess; // تاريخ نهايه العمليه (المقايسه)
                    oProcessModel.sDateDocument = item.dateDocument; // تاريخ المستند
                    oProcessModel.dnFullBudget = item.fullBudget; // المبلغ الكلى للعمليه (مقايسه)
                    oProcessModel.inContractorCode = item.ContactorCode; // كود المقاول الرئيسي
                    oProcessModel.sContractorName = item.ContractorName; //اسم المقاول الرئيسي
                    oProcessModel.inReferenceSideCode = item.RefSideCode; // كود جهه الاسناد
                    oProcessModel.sReferanceSideName = item.RefSideName; // اسم جهه الاسناد
                    oProcessModel.sMoband = item.MOBAND; // مبنده - غير مبنده
                    oProcessModel.sColor = item.color; // اللون
                    oProcessModel.sContractorFax = item.stopActiveProcess; // العمليه موقوفه - منتهيه
                    oProcessModel.sRegisterBySelf = item.registerBySelf.ToString(); // تم تسجيل العمليه عن طريق (المستخدم بنفسه - ولا حد سجله - ولا الداتا اللى راجعه من ال WSDL)

                    // خاصه بجزء مسح العمليه ف حاله عدم الموافقه على العمليه بعد
                    // اذا واجد اعتراض - اعفاء / ملاحظات امن التأمينات ولسه ماتمش الموافقه فبردوا كده مش هيقدر يمسح لانه تم فعلى الرد على العمليه من جهه التأمينات
                    oProcessModel.sProcessOpposition = item.ProcessOpposition; // هل ليها اعترض- اعفاء (عشان مايقدرش يمسح العمليه لو فيه)
                    oProcessModel.sProcessNotes = item.ProcessNotes; // هل فيه ملاحظات من التأمينات ولا (لو فيه مايقدرش يمسح العمليه)

                    oProcessModel.inProcessUser = 1;
                    LProcessModel.Add(oProcessModel);
                }
            }
            return LProcessModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="lEf"> List Of Entity Framework Of Request Process 'Stored Procedure'. </param>
        /// <returns> List Of Process Model. </returns>
        private List<ProcessModel> ConvertEFsToObjectsBasic(List<spGetRequestProcess_Result> LRequestProcessEF) // Request Process طلبات العمليات
        {
            List<ProcessModel> LProcessModel = new List<ProcessModel>();

            if (LRequestProcessEF.Count > 0)
            {
                foreach (var item in LRequestProcessEF)
                {
                    ProcessModel oProcessModel = new ProcessModel();
                    oProcessModel.iProcessCode = item.processCode; // كود العمليه
                    oProcessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "--------" : item.incommingNumber; // رقم الوارد
                    oProcessModel.sProcessName = item.processName; // اسم العمليه
                    oProcessModel.iProcessTypeCode = item.processTypeCode; // كود نوع العملية
                    oProcessModel.sProcessTypeName = item.processTypeName; // اسم نوع العملية
                    oProcessModel.sDateStartProcess = item.dateStartProcess; // تاريخ بدء العمليه (المقايسه)
                    oProcessModel.sDateEndProcess = item.dateEndProcess; // تاريخ نهايه العمليه (المقايسه)
                    oProcessModel.dnFullBudget = item.fullBudget; // المبلغ الكلى للعمليه (مقايسه)
                    oProcessModel.inContractorCode = item.contractorCode; // كود المقاول الرئيسي
                    oProcessModel.sContractorName = item.contractorName; //اسم المقاول الرئيسي
                    oProcessModel.inReferenceSideCode = item.RefSideCode; // كود جهه الاسناد
                    oProcessModel.sReferanceSideName = item.RefSideName; // اسم جهه الاسناد
                    oProcessModel.sMoband = item.MOBAND; // مبنده - غير مبنده
                    oProcessModel.sColor = item.color; // اللون                    
                    oProcessModel.inProcessUser = item.procUser;// عدد مستخدمين العمليه
                    oProcessModel.sDateInsert = item.dateInsert;// عدد مستخدمين العمليه

                    LProcessModel.Add(oProcessModel);
                }
            }
            return LProcessModel;
        }

        /// <summary>
        ///   Convert From List Of Entity Framework To List Of Model 'Stored Procedure'. 
        /// </summary>
        /// <param name="LExpireProcessEF"> List Of Entity Framework Of Exipre Process 'Stored Procedure'. </param>
        /// <returns> List Of Process Model. </returns>
        private List<ProcessModel> ConvertEFsToObjectsBasic(List<spGetExpireProcesses_Result> LExpireProcessEF) // Expire Process العمليات الموشكه على الانتهاء
        {
            List<ProcessModel> LProcessModel = new List<ProcessModel>();

            if (LExpireProcessEF.Count > 0)
            {
                foreach (var item in LExpireProcessEF)
                {
                    ProcessModel oProcessModel = new ProcessModel();
                    oProcessModel.iProcessCode = item.processCode; // كود العمليه
                    oProcessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "--------" : item.incommingNumber; // رقم الوارد
                    oProcessModel.sAreaIDName = item.areaID; //اسم المنطقه 
                    oProcessModel.sOfficeIDName = item.officeInsuranceID; //اسم المكتب
                    oProcessModel.sProcessNumber = String.IsNullOrEmpty(item.processNumber) ? "-----" : item.processNumber;// رقم العمليه
                    oProcessModel.sProcessName = item.processName; // اسم العمليه
                    oProcessModel.iProcessTypeCode = item.processTypeCode; // كود نوع العملية
                    oProcessModel.sProcessTypeName = item.processTypeName; // اسم نوع العملية
                    oProcessModel.sDateStartProcess = item.dateStartProcess; // تاريخ بدء العمليه (المقايسه)
                    oProcessModel.sDateEndProcess = item.dateEndProcess; // تاريخ نهايه العمليه (المقايسه)
                    oProcessModel.sFullSiteAddress = item.fullAddress; // عنوان الموقع بالكامل
                    oProcessModel.inProcessUser = item.procUser;// عدد مستخدمين العمليه

                    LProcessModel.Add(oProcessModel);
                }
            }
            return LProcessModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lEf"></param>
        /// <returns></returns>
        internal override List<ProcessModel> ConvertEFsToObjects(List<process> lEf)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///   Get Object Of Process Date.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="processNumber"> Process Number. </param>
        public void vGetMainProcessData(string processCode, string processNumber)
        {
            int pCode = Convert.ToInt32(processCode);
            // محتاج تعديل عشان انا باخد اول عمليه هترجع فى حاله ان لاقى عمليات كتير 
            var modal = db.spGetMainProcessData(pCode, processNumber).ToList();

            if (modal.Count == 1)
            {
                this.iProcessCode = modal[0].processCode; // كود العمليه
                this.sIncommingNumber = String.IsNullOrEmpty(modal[0].incommingNumber) ? "--------" : modal[0].incommingNumber; // رقم الوارد
                this.sProcessName = modal[0].processName; // اسم العمليه
                this.sProcessNumber = String.IsNullOrEmpty(modal[0].processNumber) ? "-----" : modal[0].processNumber; // رقم العملية
                this.iAreaCode = modal[0].areaCode; // كود المنطقه
                this.sAreaIDName = modal[0].areaID; // اسم المنطقه
                this.iOfficeCode = modal[0].officeInsuranceCode; // كود المكتب
                this.sOfficeIDName = modal[0].officeInsuranceID; // اسم المكتب
                this.inContractorCode = modal[0].ContractorCode; // كودالمقاول الرئيسي
                this.sContractorName = modal[0].ContractorName; // اسم المقاول الرئيسي
                this.sContractorNum = modal[0].contractorInsuranceNum; // الرقم التأمينى للمقاول الرئيسي
                this.sContractorLegalEntityName = modal[0].contractorLegalEntityName; //  الكيان القانونى المقاول الرئيسي
                this.inReferenceSideCode = modal[0].referenceSideContractorCode; // كود جهه الاسناد
                this.sReferanceSideNum = modal[0].referenceSideInsuranceNum; // الرقم التأمينى لجهه الاسناد
                this.sReferanceSideName = modal[0].referenceSideName; // اسم جهه الاسناد
                this.sReferanceSideLegalEntityName = modal[0].referenceSideContractorLegalEntityName; // الكيان القانونى جهه الاسناد
                this.sContractorFax = modal[0].contFax; // فاكس المقاول
                this.sContractorPhone = modal[0].contPhone; // هاتف المقاول
                this.sContractorEmail = modal[0].contEmail; // ايميل المقاول

                this.sDateStartProcess = modal[0].dateStartProcess == null ? null : modal[0].dateStartProcess.ToString(); // تاريخ بدء العمليه (المقايسه)
                this.sDateEndProcess = modal[0].dateEndProcess == null ? null : modal[0].dateEndProcess.ToString(); // تاريخ نهايه العمليه (المقايسه)

                this.inDocumentTypeCode = modal[0].documentTypeCode; // كود نوع المستند
                this.sDocumentName = String.IsNullOrEmpty(modal[0].documentTypeName) ? "-----" : modal[0].documentTypeName; // اسم المستند
                this.sDateDocument = String.IsNullOrEmpty(modal[0].dateDocument) ? "-----" : modal[0].dateDocument; // تاريخ المستند

                this.iProcessTypeCode = modal[0].processTypeCode; // كود نوع العمليه
                this.sProcessTypeName = modal[0].processTypeName; // اسم نوع العملية
                this.sFullSiteAddress = modal[0].fullAddress; // عنوان العمليه بالكامل
                this.sProcessStopNotActive = GetProcessStopNotActive(Convert.ToInt32(modal[0].processType));
            }
            else if (modal.Count == 0)
                this.sAddressOwnerPerm = "عفوا , لا يوجد عمليه بهذا الرقم. ";
            else if (modal.Count >= 1)
                this.sAddressOwnerPerm = "عفوا , هذا الرقم مكرر لاكثر من عمليه .. الرجاء ادخال الرقم لعمليه واحده. ";

        }

        /// <summary>
        ///   Get Main Process Data By Process Number , Worker code And Date Injury. 
        /// </summary>
        /// <param name="workerCode"> Worker code. </param>
        /// <param name="processNumber"> Process Number. </param>
        /// <param name="dtStartinjury"> Date Start Injury. </param>
        public void getMainProcessByPNumAndwCodeAndDateInjury(int workerCode, string processNumber, DateTime? dtStartinjury)
        {
            int wCode = Convert.ToInt32(workerCode);
            var modal = db.spGetMainProcessByPNumAndWcodeAndDateInjury(processNumber, wCode, dtStartinjury).ToList();

            if (modal != null && modal.Count == 1)
            {
                this.iProcessCode = modal[0].processCode; // كود العمليه
                this.sProcessNumber = "Done"; // رقم العملية
                this.sDateStartProcess = modal[0].dateAttendence; // تاريخ الاصابه
                this.sContractorName = modal[0].ContractorName; // اسم المقاول الرئيسي بالعمليه
                this.iProcessTypeCode = modal[0].processTypeCode; // كود نوع العمليه
                this.sProcessTypeName = modal[0].processTypeName; // اسم نوع العملية
                this.sFullSiteAddress = modal[0].fullAddress; // عنوان العمليه بالكامل
            }
            else if (modal.Count > 1)
                this.sProcessNumber = "عفوا ,هذا الرقم مسجل اكثر من مره..";
            else if (modal.Count == 0)
                this.sProcessNumber = "عفوا ,هذا العامل لم يتم تحضيره  هذا اليوم بهذه العمليه..";
        }

        /// <summary>
        ///   Get Worker In Process.
        /// </summary>
        /// <param name="ID"> Worker Code. </param>
        /// <returns> List Of Process Model. </returns>
        public List<ProcessModel> GetWorkerProcess(string ID)
        {
            try
            {
                var models = db.GetWorkerProcess(Convert.ToInt32(ID)).ToList();
                List<ProcessModel> LprocessModel = new List<ProcessModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        ProcessModel OprocessModel = new ProcessModel();
                        OprocessModel.sOfficeIDName = item.officeInsuranceID;
                        OprocessModel.sAreaIDName = item.areaID;
                        OprocessModel.sProcessNumber = item.procNumber;
                        OprocessModel.sProcessName = item.processName;
                        OprocessModel.sProcessTypeName = item.processTypeName;
                        OprocessModel.sDateStartProcess = item.dateStartProcess;
                        OprocessModel.sDateEndProcess = item.dateEndProcess;
                        OprocessModel.sReferneceSideName = item.RefrenceSideName;
                        OprocessModel.sContractorName = item.MainContractor;
                        LprocessModel.Add(OprocessModel);
                    }
                }
                return LprocessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Save Minimum And Maximum Workers In Process.
        /// </summary>
        /// <param name="newObj"> New Model. </param>
        /// <returns> Save Done Or Not. </returns>
        public bool SaveMinMax(ProcessModel newObj)
        {
            try
            {
                process model = db.processes.FirstOrDefault(x => x.processCode == newObj.iProcessCode);
                if (model == null)
                    return false;

                model.userUpdateCode = newObj.inUserUpdateCode;
                model.ipUpdate = newObj.sIpUpdate;
                model.dateUpdate = DateTime.Now;
                model.minNumber = newObj.iMinNumber;
                model.maxNumber = newObj.iMaxNumber;

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
        ///   Get Data For Minimum And Maximum.
        /// </summary>
        /// <param name="id"> Process Code. </param>
        /// <returns> Process Model. </returns>
        public ProcessModel GetMinMax(int id)
        {
            try
            {
                var models = db.processes.Where(x => x.processCode == id).ToList();
                ProcessModel LprocessModel = new ProcessModel();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        LprocessModel.iMinNumber = item.minNumber;
                        LprocessModel.iMaxNumber = item.maxNumber;
                        LprocessModel.sProcessName = item.processName;
                    }
                }
                return LprocessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Get Details Of Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="x"></param>
        /// <returns> List Of Process Code. </returns>
        public List<ProcessModel> GetAll(int Id, bool x)
        {
            try
            {
                var models = db.GetProcessDetails(Id).ToList();
                List<ProcessModel> LprocessModel = new List<ProcessModel>();

                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        ProcessModel OprocessModel = new ProcessModel();
                        OprocessModel.iProcessCode = (int)item.ProcessCode;
                        OprocessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "-----" : item.incommingNumber; // رقم الوارد
                        OprocessModel.sProcessName = item.ProcessName;
                        OprocessModel.sProNum1 = item.ProNum1;
                        OprocessModel.sProNum2 = item.ProNum2;
                        OprocessModel.sProNum3 = item.ProNum3;
                        OprocessModel.sProNum4 = item.ProNum4;
                        OprocessModel.sProcessTypeName = item.ProcessTypeName;
                        OprocessModel.sDateStartProcess = item.DateStartProcess;
                        OprocessModel.sDateEndProcess = item.DateEndProcess;
                        OprocessModel.sReferneceSideName = item.referneceSideName;
                        OprocessModel.sReferenceLegalName = item.ReferenceLegalName;
                        OprocessModel.bIsLimited = item.IsLimited;
                        OprocessModel.decFullBudget = item.FullBudget;
                        OprocessModel.sDocumentName = item.DocumentName;
                        OprocessModel.sDateDocument = item.dateDocument;
                        OprocessModel.iMinNumber = item.MinNumber;
                        OprocessModel.sAddressOwnerPerm = item.AddressOwnerPerm;
                        OprocessModel.iMaxNumber = item.MaxNumber;
                        OprocessModel.sContractorName = item.ContractorName;
                        OprocessModel.sContractorLegalName = item.ContractorLegalName;
                        OprocessModel.sContractorProcessNumber = item.ContractorProcessNumber;
                        OprocessModel.sProcessSite = item.ProcessSite;
                        OprocessModel.sContractorPhone = item.ContractorPhone;
                        OprocessModel.sContractorEmail = item.ContractorEmail;
                        OprocessModel.sContractorFax = item.ContractorFax;
                        OprocessModel.sContractorCount = item.ContractorCount;
                        OprocessModel.sOfficeId = item.OfficeId;
                        OprocessModel.sOfficeName = item.OfficeName;
                        OprocessModel.sAreaId = item.areaId;
                        OprocessModel.sAreaName = item.areaName;
                        OprocessModel.sWorkerAlreadyNumber = item.workerAlreadyNumber;
                        OprocessModel.sContractorInsuranceNumber = item.ContractorInsuranceNum;
                        OprocessModel.sRefernceInsuranceNumber = item.referenceSideInsuranceNum;
                        OprocessModel.sRealFullBudget = item.realFullBudget;
                        LprocessModel.Add(OprocessModel);
                    }
                }
                return LprocessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Get Details Of Process
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="y"></param>
        /// <returns> Process Model. </returns>
        public ProcessModel GetById(int Id, bool y)
        {
            try
            {
                var models = db.GetProcessDetails(Id).ToList();
                ProcessModel LprocessModel = new ProcessModel();
                if (models.Count > 0)
                {
                    foreach (var item in models)
                    {
                        LprocessModel.iProcessCode = (int)item.ProcessCode;
                        LprocessModel.sIncommingNumber = String.IsNullOrEmpty(item.incommingNumber) ? "-----" : item.incommingNumber; // رقم الوارد
                        LprocessModel.sProcessName = item.ProcessName;
                        LprocessModel.sProNum1 = item.ProNum1;
                        LprocessModel.sProNum2 = item.ProNum2;
                        LprocessModel.sProNum3 = item.ProNum3;
                        LprocessModel.sProNum4 = item.ProNum4;
                        LprocessModel.sProcessTypeName = item.ProcessTypeName;
                        LprocessModel.sDateStartProcess = item.DateStartProcess;
                        LprocessModel.sDateEndProcess = item.DateEndProcess;
                        LprocessModel.sReferneceSideName = item.referneceSideName;
                        LprocessModel.sReferenceLegalName = item.ReferenceLegalName;
                        LprocessModel.bIsLimited = item.IsLimited;
                        LprocessModel.decFullBudget = item.FullBudget;
                        LprocessModel.sDocumentName = item.DocumentName;
                        LprocessModel.iMinNumber = item.MinNumber;
                        LprocessModel.sAddressOwnerPerm = item.AddressOwnerPerm;
                        LprocessModel.iMaxNumber = item.MaxNumber;
                        LprocessModel.sContractorName = item.ContractorName;
                        LprocessModel.sContractorLegalName = item.ContractorLegalName;
                        LprocessModel.sContractorProcessNumber = item.ContractorProcessNumber;
                        LprocessModel.sProcessSite = item.ProcessSite;
                        LprocessModel.sContractorPhone = item.ContractorPhone;
                        LprocessModel.sContractorEmail = item.ContractorEmail;
                        LprocessModel.sContractorFax = item.ContractorFax;
                        LprocessModel.sContractorCount = item.ContractorCount;
                        LprocessModel.sOfficeId = item.OfficeId;
                        LprocessModel.sOfficeName = item.OfficeName;
                        LprocessModel.sAreaId = item.areaId;
                        LprocessModel.sAreaName = item.areaName;
                        LprocessModel.sWorkerAlreadyNumber = item.workerAlreadyNumber;
                        LprocessModel.sContractorInsuranceNumber = item.ContractorInsuranceNum;
                        LprocessModel.sRefernceInsuranceNumber = item.referenceSideInsuranceNum;
                        LprocessModel.sRealFullBudget = item.realFullBudget;
                    }
                }
                return LprocessModel;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

    }
}