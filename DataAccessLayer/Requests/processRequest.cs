using DataAccessLayer.Abstracts;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Requests
{
    /// <summary>
    ///   Request Of ' Processes - Expire Processes - Request Processes - Processes Of Reference side  - Processes Of Contractor '. 
    /// </summary>
    public class ProcessRequest : RequestBase<ProcessModel>
    {
        // process العمليات
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        public List<ReferenceSideContractorModel> lrefConModel = new List<ReferenceSideContractorModel>(); // جهه الاسناد

        public ReferenceSideContractorModel oRefSideCont = new ReferenceSideContractorModel(); // جهه الاسناد

        public ProcessSubContractorModel oProcessSubContractorMode { get; set; }  // المقاولين (رئيسي - مقاولين من باطن)
        public ProcessSiteModel oProcessSiteModel { get; set; } // موقع العمليه
        public ProcessUserLettersSiteModel oProcessUserLettersSiteModel { get; set; } // عنوان المخطار للمراسله
        public List<ProcessSubContractorModel> LoProcessSubContractorMode { get; set; }


        public List<ProcessNotesModel> LprocessNotes = new List<ProcessNotesModel>();


        public ProcessNotesModel OprocessNotesModel { get; set; }


        public List<MissionSubContractorModel> LmissionSubContractor = new List<MissionSubContractorModel>();
        public ProcessOppositionModel oprocessOppositionModel { get; set; }


        /// <summary>
        ///   Get All Processes.
        /// </summary>
        public override void GetInit()
        {
            this.OModel = new ProcessModel();
            this.LModels = new ProcessModel().GetAll();
        }

        /// <summary>
        ///   Get Object Of Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        public override void GetInitObject(int Id)
        {
            this.OModel = new ProcessModel().GetById(Id);
            this.oProcessSiteModel = new ProcessSiteModel().GetById(Id);
            this.oProcessUserLettersSiteModel = new ProcessUserLettersSiteModel().GetById(Id);
        }

        /// <summary>
        ///   Get Object Of Process.
        /// </summary>
        /// <param name="Id"> Process Code. </param>
        /// <param name="x"></param>
        public void GetInitObject(int Id, bool x)
        {
            this.LModels = new ProcessModel().GetAll(Id, true);
            this.OModel = new ProcessModel();
            this.OModel.sProcessStopNotActive = this.OModel.GetProcessStopNotActive(Id);
        }

        /// <summary>
        ///   Get All Processes ' reference sides - Contractors'.
        /// </summary>
        /// <param name="Id"> User code. </param>
        public override void GetList(string Id)
        {
            this.LModels = new ProcessModel().GetAll(Convert.ToInt32(Id));
        }

        /// <summary>
        ///   Get All Processes.
        /// </summary>
        /// <param name="Id"> User Code. </param>
        public void vGetOfficeProcess(string Id) // Process Office عمليات موظف التأمينات  
        {
            this.LModels = new ProcessModel().vGetOfficeProcess(Id);
        }

        /// <summary>
        ///   Get All Request Processes.
        /// </summary>
        /// <param name="Id"> User Code. </param>
        public void vGetRequestProcesses(string Id) // Request Process طلبات العمليات
        {
            this.LModels = new ProcessModel().vGetRequestProcesses(Id);
        }

        /// <summary>
        ///   Get All Expire Processes.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        public void vGetExpireProcesses(string processCode) // Expire Process العمليات الموشكه على الانتهاء
        {
            this.LModels = new ProcessModel().vGetExpireProcesses(processCode);
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="searchObjs"> List Of Special Parameters That Will Search On It. </param>
        public override void vSearch(List<string> searchObjs) // Contractors المقاولين
        {
            this.LModels = new ProcessModel().lSearch(searchObjs);
        }

        /// <summary>
        ///   Search With Special Parameters ' Notification'.
        /// </summary>
        /// <param name="SearchObj"> List Of Special Parameters That Will Search On It. </param>
        public void vSearchNotify(List<string> SearchObj) // Contractors Notify اشعارات المقاولين
        {
            this.LModels = new ProcessModel().osearchNotify(SearchObj);
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="SearchObj"> List Of Special Parameters That Will Search On It. </param>
        public void vSearchOfficeProcess(List<string> SearchObj)// Process Office عمليات موظف التأمينات
        {
            this.LModels = new ProcessModel().lSearchOfficeProcess(SearchObj);
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="SearchObj"> List Of Special Parameters That Will Search On It. </param>
        public void vSearchRequestProcess(List<string> SearchObj)  // Request Process طلبات العمليات
        {
            this.LModels = new ProcessModel().lSearchRequestProcess(SearchObj);
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="SearchObj"> List Of Special Parameters That Will Search On It. </param>
        public void vSearchExpireProcess(List<string> SearchObj)// Expire Process العمليات الموشكه على الانتهاء
        {
            this.LModels = new ProcessModel().lSearchExpireProcess(SearchObj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        public override void vSave(ProcessModel newObj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vSave(ProcessModel newObj, int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///   Save Object Of Process.
        /// </summary>
        /// <param name="oProcessRequest"> New Requset. </param>
        public void vSave(ProcessRequest oProcessRequest)
        {
            oProcessRequest.OModel.sIpInsert = generalMethod.vIPAddress();

            this.OModel = new ProcessModel();
            this.oProcessSiteModel = new ProcessSiteModel();
            this.oProcessUserLettersSiteModel = new ProcessUserLettersSiteModel();
            this.oProcessSubContractorMode = new ProcessSubContractorModel();

            if (this.OModel.bSave(oProcessRequest.OModel))
            {
                // Process Saved
                this.OModel.bIsSaved = true;
                oProcessRequest.OModel.iProcessCode = this.OModel.iProcessCode;
                oProcessRequest.oProcessSubContractorMode = new ProcessSubContractorModel();

                #region Save Process Sub Contractor المقاول الرئيسي

                if (oProcessRequest.OModel.inReferenceSideCode != null && oProcessRequest.OModel.inReferenceSideCode != 0) // اختار جهه اسناد عن طريق المقاول الرئيسي
                {
                    int userCode = Convert.ToInt32(oProcessRequest.OModel.inUserInsertCode.ToString());
                    UserModel user = new UserModel();
                    user = user.GetById(userCode);
                    oProcessRequest.oProcessSubContractorMode.iContractorCode = Convert.ToInt32(user.iContractorCode.ToString());
                }
                else
                    oProcessRequest.oProcessSubContractorMode.iContractorCode = Convert.ToInt32(oProcessRequest.OModel.inContractorCode.ToString());

                oProcessRequest.oProcessSubContractorMode.bnContractorType = true; // مقاول رئيسي
                oProcessRequest.oProcessSubContractorMode.bSubContractorAccept = true;
                oProcessRequest.oProcessSubContractorMode.sContractorProcessNumber = "00"; // مقاول رئيسي
                oProcessRequest.oProcessSubContractorMode.inUserInsertCode = oProcessRequest.OModel.inUserInsertCode;
                oProcessRequest.oProcessSubContractorMode.sIpInsert = oProcessRequest.OModel.sIpInsert;

                if (this.oProcessSubContractorMode.bSave(oProcessRequest.oProcessSubContractorMode, this.OModel.iProcessCode))
                    this.oProcessSubContractorMode.bIsSaved = true;
                else
                    this.oProcessSubContractorMode.bIsSaved = false;

                #endregion
                #region Save ProcessSite عنوان العملية

                if (this.oProcessSiteModel != null)
                {
                    oProcessRequest.oProcessSiteModel.sIpInsert = oProcessRequest.OModel.sIpInsert;
                    oProcessRequest.oProcessSiteModel.dtDateInsert = oProcessRequest.OModel.dtDateInsert;
                    oProcessRequest.oProcessSiteModel.inUserInsertCode = oProcessRequest.OModel.inUserInsertCode;

                    if (this.oProcessSiteModel.bSave(oProcessRequest.oProcessSiteModel, oProcessRequest.OModel.iProcessCode))
                        this.oProcessSiteModel.bIsSaved = true;
                    else
                        this.oProcessSiteModel.bIsSaved = false;
                }
                else
                    this.oProcessSiteModel.bIsSaved = true;

                #endregion
                #region Save ProcessUserLettersSite عنوان المختار للمراسله

                if (this.oProcessUserLettersSiteModel != null)
                {
                    oProcessRequest.oProcessUserLettersSiteModel.sIpInsert = oProcessRequest.OModel.sIpInsert;
                    oProcessRequest.oProcessUserLettersSiteModel.dtDateInsert = oProcessRequest.OModel.dtDateInsert;
                    oProcessRequest.oProcessUserLettersSiteModel.inUserInsertCode = oProcessRequest.OModel.inUserInsertCode;

                    if (this.oProcessUserLettersSiteModel.bSave(oProcessRequest.oProcessUserLettersSiteModel, oProcessRequest.OModel.iProcessCode))
                        this.oProcessUserLettersSiteModel.bIsSaved = true;
                    else
                        this.oProcessUserLettersSiteModel.bIsSaved = false;
                }
                else
                    this.oProcessUserLettersSiteModel.bIsSaved = true;

                #endregion
            }
            else
                this.OModel.bIsSaved = false;
        }

        /// <summary>
        ///   Save Object Of Sub contractor Of Process.
        /// </summary>
        /// <param name="oProcessRequest"> New Request. </param>
        public void vSaveSubContractorByUser(ProcessRequest oProcessRequest) //اخطار مقاولين من باطن
        {
            this.oProcessSubContractorMode = new ProcessSubContractorModel();

            int userCode = Convert.ToInt32(oProcessRequest.oProcessSubContractorMode.inUserInsertCode.ToString());
            UserModel user = new UserModel();
            user = user.GetById(userCode);

            oProcessRequest.oProcessSubContractorMode.iContractorCode = Convert.ToInt32(user.iContractorCode.ToString());
            oProcessRequest.oProcessSubContractorMode.bnContractorType = false; // مقاول من باطن
            oProcessRequest.oProcessSubContractorMode.sContractorProcessNumber = "01"; // مقاول من باطن
            int? insertUserCode = oProcessRequest.oProcessSubContractorMode.inUserInsertCode;
            oProcessRequest.oProcessSubContractorMode.inUserInsertCode = insertUserCode;
            oProcessRequest.oProcessSubContractorMode.sIpInsert = generalMethod.vIPAddress();

            if (this.oProcessSubContractorMode.bSave(oProcessRequest.oProcessSubContractorMode, oProcessRequest.OModel.iProcessCode))
                this.oProcessSubContractorMode.bIsSaved = true;
            else
                this.oProcessSubContractorMode.bIsSaved = false;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="Id"></param>
        public override void vEdit(ProcessModel newObj, int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oProcessRequest"> New Request. </param>
        /// <param name="Id"> Process Code. </param>
        public void vEdit(ProcessRequest oProcessRequest, int Id)
        {
            oProcessRequest.OModel.sIpUpdate = generalMethod.vIPAddress();

            this.OModel = new ProcessModel();
            this.oProcessSiteModel = new ProcessSiteModel();
            this.oProcessUserLettersSiteModel = new ProcessUserLettersSiteModel();

            if (this.OModel.bEdit(oProcessRequest.OModel, Id))
            {
                // Process Edited
                this.OModel.bIsEdit = true;

                oProcessRequest.oProcessSubContractorMode = new ProcessSubContractorModel();
                this.oProcessSubContractorMode = new ProcessSubContractorModel();
                #region Save Process Sub Contractor المقاول الرئيسي

                // جهه الاسناد عدلت المقاول الرئيسي
                if (oProcessRequest.OModel.inContractorCode != null && oProcessRequest.OModel.inContractorCode != 0)
                {
                    ProcessSubContractorModel oProcessSubContractorModel = new ProcessSubContractorModel();
                    oProcessSubContractorModel.GetProcessMainContByProcessId(oProcessRequest.OModel.iProcessCode); // المقاول الرئيسي
                    oProcessRequest.oProcessSubContractorMode.iProcessSubContractorCode = oProcessSubContractorModel.iProcessSubContractorCode; // كود المقاول الرئيسي

                    oProcessRequest.oProcessSubContractorMode.iContractorCode = Convert.ToInt32(oProcessRequest.OModel.inContractorCode.ToString());
                    oProcessRequest.oProcessSubContractorMode.inUserUpdateCode = oProcessRequest.OModel.inUserUpdateCode;
                    oProcessRequest.oProcessSubContractorMode.sIpUpdate = oProcessRequest.OModel.sIpUpdate;

                    if (this.oProcessSubContractorMode.bEdit(oProcessRequest.oProcessSubContractorMode, oProcessRequest.OModel.iProcessCode, true))
                        this.oProcessSubContractorMode.bIsEdit = true;
                    else
                        this.oProcessSubContractorMode.bIsEdit = false;
                }
                else
                {
                    this.oProcessSubContractorMode.bIsEdit = true;
                }
                #endregion
                #region Edit ProcessSite عنوان العملية
                if (this.oProcessSiteModel != null)
                {
                    oProcessRequest.oProcessSiteModel.sIpUpdate = oProcessRequest.OModel.sIpUpdate;
                    oProcessRequest.oProcessSiteModel.dtDateUpdate = oProcessRequest.OModel.dtDateUpdate;
                    oProcessRequest.oProcessSiteModel.inUserUpdateCode = oProcessRequest.OModel.inUserUpdateCode;

                    if (this.oProcessSiteModel.bEdit(oProcessRequest.oProcessSiteModel, Id))
                        this.oProcessSiteModel.bIsEdit = true;
                    else
                        this.oProcessSiteModel.bIsEdit = false;
                }
                else
                    this.oProcessSiteModel.bIsEdit = true;
                #endregion
                #region Edit ProcessUserLettersSite عنوان المختار للمراسله

                if (this.oProcessUserLettersSiteModel != null)
                {
                    oProcessRequest.oProcessUserLettersSiteModel.sIpUpdate = oProcessRequest.OModel.sIpUpdate;
                    oProcessRequest.oProcessUserLettersSiteModel.dtDateUpdate = oProcessRequest.OModel.dtDateUpdate;
                    oProcessRequest.oProcessUserLettersSiteModel.inUserUpdateCode = oProcessRequest.OModel.inUserUpdateCode;

                    if (this.oProcessUserLettersSiteModel.bEdit(oProcessRequest.oProcessUserLettersSiteModel, Id))
                        this.oProcessUserLettersSiteModel.bIsEdit = true;
                    else
                        this.oProcessUserLettersSiteModel.bIsEdit = false;
                }
                else
                    this.oProcessUserLettersSiteModel.bIsEdit = true;

                #endregion
            }
            else
                this.OModel.bIsEdit = false;
        }


        /// <summary>
        ///   Change Date End Process.
        /// </summary>
        /// <param name="oProcessRequest"> New Request. </param>
        public void vChangeEndDateProcess(ProcessRequest oProcessRequest)
        {
            this.OModel = new ProcessModel();
            oProcessRequest.OModel.sIpUpdate = generalMethod.vIPAddress();
            if (this.OModel.bChangeEndDateProcess(oProcessRequest.OModel))
                this.OModel.bIsEdit = true;
            else
                this.OModel.bIsEdit = false;
        }

        /// <summary>
        ///   Save Data In DataBase.
        /// </summary>
        /// <param name="SearchObj"> List Of Details Will Be Save. </param>
        public void vAcceptProcess(List<string> SearchObj)
        {
            SearchObj.Add(generalMethod.vIPAddress()); // عنوان الجهاز اثناء الموافقه على العمليه

            this.OModel = new ProcessModel();
            string acceptProcess = this.OModel.bAcceptProcess(SearchObj);
            if (acceptProcess != "عفوا , الرجاء تغيير رقم العمليه لوجود عمليه بنفس هذا الرقم" && acceptProcess != "عفوا , الرجاء تغيير رقم الوارد لوجود عمليه بنفس هذا الرقم")
            {
                this.OModel.bIsSaved = true;
                this.OModel.sProcessName = acceptProcess;
            }
            else
            {
                this.OModel.bIsSaved = false;
                this.OModel.sProcessNotes = acceptProcess;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public override void vDelete(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="IdObj"> Process Code. </param>
        /// <param name="id"> User Code. </param>
        public override void vDelete(int IdObj, string id)
        {
            this.OModel = new ProcessModel();

            if (this.OModel.bDelete(IdObj))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            GetList(id);
        }

        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="userCode"> User Code. </param>
        public void vDeleteProcess(int processCode, int userCode)
        {
            this.OModel = new ProcessModel();

            if (this.OModel.bDeleteProcess(processCode, userCode))
                this.OModel.bIsDeleted = true;
            else
                this.OModel.bIsDeleted = false;

            this.LModels = new ProcessModel().vGetRequestProcesses(userCode.ToString());

        }


        /// <summary>
        ///   Get 'Reference Side - Contractor' Data By Insurance Number.
        /// </summary>
        /// <param name="Id"> Insurance Number Of 'Reference Side - Contractor'. </param>
        public void GetRefSideContByInsurNum(string Id)
        {
            this.oRefSideCont.vRefSideContByInsurNum(Id);
        }

        /// <summary>
        ///   Get 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> Insurance Number. </param>
        public void GetRefSideContByInsurNumLegalEntity(string legalEntity, string insNum)
        {
            this.oRefSideCont.GetRefSideContByInsurNumLegalEntity(Convert.ToInt32(legalEntity), insNum);
        }

        /// <summary>
        ///   Get List Of 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="legalEntity"> 'Reference Side - Contractor' Legal Entity. </param>
        /// <param name="insNum"> Insurance Number. </param>
        public void lGetRefSideContByInsurNumLegalEntity(string legalEntity, string insNum)
        {
            this.lrefConModel = new ReferenceSideContractorModel().lGetRefSideContByInsurNumLegalEntity(Convert.ToInt32(legalEntity), insNum);
        }

        /// <summary>
        ///   Get Insurance Number Of 'Reference Side - Contractor' By 'Reference Side - Contractor' Code.
        /// </summary>
        /// <param name="Id"> 'Reference Side - Contractor' Code. </param>
        public void GetInsurNumByRefSideCont(string Id)
        {
            this.oRefSideCont.vInsurNumByRefSideCont(Id);
        }

        /// <summary>
        ///     Get Process Data By Process Number.
        /// </summary>
        /// <param name="processCode"> Process Code. </param>
        /// <param name="ProcessNum"> Process Number. </param>
        public void vGetMainProcessData(string processCode, string ProcessNum)
        {
            this.OModel = new ProcessModel();
            this.OModel.vGetMainProcessData(processCode, ProcessNum);
        }

        /// <summary>
        ///   Get Main Process Data By Process Number , Worker code And Date Injury. 
        /// </summary>
        /// <param name="workerCode"> Worker Code. </param>
        /// <param name="ProcessNum"> Process Number. </param>
        /// <param name="dtStartinjury"> Date Start Injury. </param>
        public void getMainProcessByPNumAndwCodeAndDateInjury(int workerCode, string ProcessNum, DateTime? dtStartinjury)
        {
            this.OModel = new ProcessModel();
            this.OModel.getMainProcessByPNumAndwCodeAndDateInjury(workerCode, ProcessNum, dtStartinjury);
        }

        #region subContractor
        /// <summary>
        /// Save Sub-Contractor In Process
        /// </summary>
        /// <param name="NewObj">Data Need For Save</param>
        /// <param name="lstr">List Of Missions Codes</param>
        public void SaveSubContractor(ProcessSubContractorModel NewObj, List<string> lstr)
        {


            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            NewObj.sIpInsert = this.sIpAddress;

            this.oProcessSubContractorMode = new ProcessSubContractorModel();
            if (this.oProcessSubContractorMode.SaveSub(NewObj, NewObj.iProcessCode, lstr))
            {
                oProcessSubContractorMode.bIsSaved = true;
                this.bIsSaved = true;
            }
            else
            {
                oProcessSubContractorMode.bIsSaved = false;
                this.bIsSaved = false;

            }

            this.LoProcessSubContractorMode = new ProcessSubContractorModel().GetAll(NewObj.iProcessCode, null);
        }
        /// <summary>
        /// Edit Sub-Contractor In Process
        /// </summary>
        /// <param name="NewObj">Data Need For Edit</param>
        /// <param name="lstr">List Of Missions Codes</param>
        public void EditSubContractor(ProcessSubContractorModel NewObj, List<string> lstr)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            NewObj.sIpUpdate = this.sIpAddress;

            this.oProcessSubContractorMode = new ProcessSubContractorModel();
            if (this.oProcessSubContractorMode.EditSub(NewObj, NewObj.iProcessCode, lstr))
            {
                this.bIsEdit = true;
            }
            else
                this.bIsEdit = false;

            this.LoProcessSubContractorMode = new ProcessSubContractorModel().GetAll(NewObj.iProcessCode, null);

        }
        /// <summary>
        /// Save Accept Of Sub-Contractor In Process
        /// </summary>
        /// <param name="NewObj">Data Need For Save</param>
        public void acceptSubContractor(ProcessSubContractorModel NewObj)
        {
            NewObj.ipAcceptSubContractor = generalMethod.vIPAddress();  // عنوان الجهاز اثناء الموافقه على المقاول من باطن
            this.oProcessSubContractorMode = new ProcessSubContractorModel();
            string acceptProcess = this.oProcessSubContractorMode.bAcceptSubContractor(NewObj);
            if (acceptProcess == "1")
            {
                this.oProcessSubContractorMode.bIsSaved = true;
            }
            else
                this.oProcessSubContractorMode.bIsSaved = false;
        }
        /// <summary>
        /// Get All Contractors
        /// </summary>
        /// <param name="Id">Process Code</param>
        /// <param name="Contractors">Contractors Code</param>
        public void GetSubContractors(int Id, List<int> Contractors)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;

            this.oProcessSubContractorMode = new ProcessSubContractorModel();
            this.LoProcessSubContractorMode = new ProcessSubContractorModel().GetAll(Id, Contractors);
        }
        /// <summary>
        /// Get One Contractor In Process
        /// </summary>
        /// <param name="id">Contractor Code In Process</param>
        public void GetOsubContractor(int id)
        {
            this.oProcessSubContractorMode = new ProcessSubContractorModel().GetById(id);
        }
        /// <summary>
        /// Delete Sub-Contractor In Process
        /// </summary>
        /// <param name="id">Contractor Code In Process</param>
        /// <param name="ProcessId">Process Code</param>
        public void DeleteSubContractor(int id, int ProcessId)
        {
            this.oProcessSubContractorMode = new ProcessSubContractorModel();

            if (this.oProcessSubContractorMode.bDelete(id))
                this.bIsDeleted = true;
            else
                this.bIsDeleted = false;

            GetSubContractors(ProcessId, null);
        }
        /// <summary>
        /// Search For Sub-Contractors
        /// </summary>
        /// <param name="lStr">List Of Data Need For Search</param>
        public void SearchSubContractors(List<string> lStr)
        {
            this.oProcessSubContractorMode = new ProcessSubContractorModel();
            this.LoProcessSubContractorMode = new ProcessSubContractorModel().ListSearchContractors(lStr);
        }
        /// <summary>
        /// Gel All Missions For Sub-Contractor In Processs
        /// </summary>
        /// <param name="Id">Contractor Code In Process</param>
        public void GetMissionSubContractor(int Id)
        {
            this.LmissionSubContractor = new MissionSubContractorModel().GetAll(Id);
        }
        /// <summary>
        /// Get Contractor By Name
        /// </summary>
        /// <param name="name">Name Need For Search</param>
        public void GetContractors(string name)
        {
            this.lrefConModel = new ReferenceSideContractorModel().GetContractors(name);
        }
        #endregion
        #region Notes
        /// <summary>
        /// Get All Notes In Process
        /// </summary>
        /// <param name="Id">Process Code</param>
        public void GetProcessNotes(int Id)
        {
            this.LprocessNotes = new ProcessNotesModel().GetAll(Id);
        }
        /// <summary>
        /// Save Notes In Process
        /// </summary>
        /// <param name="newObj">Data Need For Save Notes</param>
        public void SaveNotes(ProcessNotesModel newObj)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            newObj.sIpInsert = this.sIpAddress;
            this.OprocessNotesModel = new ProcessNotesModel();
            if (this.OprocessNotesModel.bSave(newObj))
            {
                this.OprocessNotesModel.bIsSaved = true;
            }
            else
                this.OprocessNotesModel.bIsSaved = false;
        }
        #endregion
        #region Opposition
        /// <summary>
        /// Get All Opposition / Exemption 
        /// </summary>
        /// <param name="processId">Process Code</param>
        public void GetAllOpossition(int processId)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            this.oprocessOppositionModel = new ProcessOppositionModel().GetById(processId);
        }
        /// <summary>
        /// Save Opposition / Exemption 
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public void SaveOpossition(ProcessOppositionModel newObj)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            newObj.sIpInsert = this.sIpAddress;
            newObj.sIpUpdate = this.sIpAddress;
            this.oprocessOppositionModel = new ProcessOppositionModel();
            if (this.oprocessOppositionModel.bSave(newObj))
            {
                this.oprocessOppositionModel.bIsSaved = true;
            }
            else
                this.oprocessOppositionModel.bIsSaved = false;

        }
        /// <summary>
        /// Edit Opposition / Exemption 
        /// </summary>
        /// <param name="NewObj">Data Need For Edit</param>
        public void EditOpossition(ProcessOppositionModel NewObj)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            NewObj.sIpUpdate = this.sIpAddress;

            this.oprocessOppositionModel = new ProcessOppositionModel();
            if (this.oprocessOppositionModel.bEdit(NewObj, (int)NewObj.iProcessCode))
            {
                this.oprocessOppositionModel.bIsEdit = true;
            }
            else
                this.oprocessOppositionModel.bIsEdit = false;

        }
        #endregion
        #region Min Max
        /// <summary>
        /// Save Minimum And Maximum Workers In Process
        /// </summary>
        /// <param name="newObj">Data Need For Save</param>
        public void SaveMinMax(ProcessModel newObj)
        {
            string sIpAddress = generalMethod.vIPAddress();
            this.sIpAddress = sIpAddress == "0" ? null : sIpAddress;
            newObj.sIpUpdate = this.sIpAddress;

            this.OModel = new ProcessModel();
            if (this.OModel.SaveMinMax(newObj))
            {
                this.OModel.bIsEdit = true;
            }
            else
                this.OModel.bIsEdit = false;
        }
        /// <summary>
        /// Get Data For Minimum And Maximum
        /// </summary>
        /// <param name="id">Process Code</param>
        public void GetMinMax(int id)
        {
            this.OModel = new ProcessModel().GetMinMax(id);
        }
        #endregion
    }
}
