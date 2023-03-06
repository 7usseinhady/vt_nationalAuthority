using DataAccessLayer.Requests;
using ApiNationalAuthority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of ' Processes - Expire Processes - Request Processes - Processes Of Reference side  - Processes Of Contractor '. 
    /// </summary>
    public class apiProcessController : ApiController
    {
        ProcessRequest oRequest = new ProcessRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        ///   Get All Processes.
        /// </summary>
        /// <returns> Request. </returns>
        public ProcessRequest GetProcess()
        {
            oRequest.GetInit();
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Process.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetProcess(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetInitObject(Convert.ToInt32(lString[0]));

            return oRequest;
        }

        /// <summary>
        ///   Get All Users Of Process.
        /// </summary>
        /// <param name="sStr"> User Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetProcessUserCode(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetList(lString[0]);

            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Process 
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetMainProcessData(string sStr)
        {
            oRequest.vGetMainProcessData(sStr, null);
            return oRequest;
        }

        /// <summary>
        ///   Get All Processes.
        /// </summary>
        /// <param name="sStr"> User Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetOfficeProcess(string sStr) // Process Office عمليات موظف التأمينات  
        {
            oRequest.vGetOfficeProcess(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get All Request Processes.
        /// </summary>
        /// <param name="sStr"> User Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetRequestProcesses(string sStr) // Request Process طلبات العمليات
        {
            oRequest.vGetRequestProcesses(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get All Expire Processes.
        /// </summary>
        /// <param name="sStr"> User Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetExpireProcesses(string sStr)  // Expire Process العمليات الموشكه على الانتهاء
        {
            oRequest.vGetExpireProcesses(sStr);
            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchProcess([FromBody] List<string> lStr) // Contractors المقاولين
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }

        /// <summary>
        ///   Search With Special Parameters 'Notification'.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchNotifyProcess([FromBody] List<string> lStr) // Contractors NOTIFICATION اشعارات المقاولين
        {
            oRequest.vSearchNotify(lStr);
            return oRequest;
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchOfficeProcess([FromBody] List<string> lStr)// Process Office عمليات موظف التأمينات  
        {
            oRequest.vSearchOfficeProcess(lStr);
            return oRequest;
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchRequestProcess([FromBody] List<string> lStr)// Request Process طلبات العمليات
        {
            oRequest.vSearchRequestProcess(lStr);
            return oRequest;
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchExpireProcess([FromBody] List<string> lStr)// Expire Process العمليات الموشكه على الانتهاء
        {
            oRequest.vSearchExpireProcess(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewProcess"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveProcess([FromBody]ProcessRequest oNewProcess) // Contractors المقاولين
        {
            oRequest.vSave(oNewProcess);
            return oRequest;
        }

        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewProcess"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveSubContractorProcessByUser([FromBody]ProcessRequest oNewProcess) //اخطار مقاولين من باطن
        {
            oRequest.vSaveSubContractorByUser(oNewProcess);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewProcessStop"> Edit Request </param>
        /// <param name="sStr"> Code Of Process Will Be Edit. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostEditProcess([FromBody]ProcessRequest oNewProcess, [FromUri]string sStr) // Contractors المقاولين
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vEdit(oNewProcess, Convert.ToInt32(lString[0]));
            return oRequest;
        }


        /// <summary>
        ///   Change Date End Process.
        /// </summary>
        /// <param name="oNewProcess"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostChangeEndDateProcess([FromBody]ProcessRequest oNewProcess) //Expire Process العمليات الموشكه على الانتهاء (تعديل المده)
        {
            oRequest.vChangeEndDateProcess(oNewProcess);
            return oRequest;
        }

        /// <summary>
        ///   Save Data In DataBase.
        /// </summary>
        /// <param name="lStr"> List Of Details Will Be Save. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostAcceptProcess([FromBody]List<string> lStr) // الموافقه على العمليه
        {
            oRequest.vAcceptProcess(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Process.
        /// </summary>
        /// <param name="sStr"> Code Of Process Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public ProcessRequest DeleteProcess([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]), lString[1]);
            return oRequest;
        }


        /// <summary>
        ///   Get 'Reference Side - Contractor' Data By Insurance Number.
        /// </summary>
        /// <param name="sStr"> Insurance Number Of 'Reference Side - Contractor'. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetRefSideContByInsurNum(string sStr)
        {
            oRequest.GetRefSideContByInsurNum(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="sStr"> String Of Insurance Number And Legal Entity. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetRefSideContByInsurNumLegalEntity(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.GetRefSideContByInsurNumLegalEntity(lString[0], lString[1]);
            return oRequest;
        }

        /// <summary>
        ///   Get Insurance Number Of 'Reference Side - Contractor' By 'Reference Side - Contractor' Code.
        /// </summary>
        /// <param name="sStr"> 'Reference Side - Contractor' Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetInsurNumByRefSideCont(string sStr)
        {
            oRequest.GetInsurNumByRefSideCont(sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get List Of 'Reference Side - Contractor' Data By Insurance Number And Legal Entity.
        /// </summary>
        /// <param name="sStr"> String Of Insurance Number And Legal Entity. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetRefSideContByInsurNumLegalEntityList(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.lGetRefSideContByInsurNumLegalEntity(lString[0], lString[1]);
            return oRequest;
        }

        /// <summary>
        ///   Get Process Data By Process Number.
        /// </summary>
        /// <param name="sStr"> Process Number. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetMainProcessByProcessNum(string sStr)
        {
            oRequest.vGetMainProcessData("-1", sStr);
            return oRequest;
        }

        /// <summary>
        ///   Get Main Process Data By Process Number , Worker code And Date Injury. 
        /// </summary>
        /// <param name="sStr"> string Of Process Number , Worker code And Date Injury. </param>
        /// <returns> Request. </returns>
        public ProcessRequest getMainProcessByPNumAndwCodeAndDateInjury(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.getMainProcessByPNumAndwCodeAndDateInjury(Convert.ToInt32(lString[0]), lString[1], Convert.ToDateTime(lString[2]));
            return oRequest;
        }

        #region rehab

        /// <summary>
        ///    Get Details Of Process.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetProcessDetails(string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr), true);
            return oRequest;
        }

        #region subContractor

        /// <summary>
        ///   Save Sub-Contractor In Process
        /// </summary>
        /// <param name="oModalRequest"> New Request. </param>
        /// <param name="sStr"> String Of Missions Codes</param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveSubContractor([FromBody]ProcessRequest oModalRequest, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.SaveSubContractor(oModalRequest.oProcessSubContractorMode, lString);

            return oRequest;
        }

        /// <summary>
        ///   Edit Sub-Contractor In Process.
        /// </summary>
        /// <param name="oModalRequest"> Edit Request. </param>
        /// <param name="sStr"> String Of Missions Codes. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostEditSubContractor([FromBody]ProcessRequest oModalRequest, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.EditSubContractor(oModalRequest.oProcessSubContractorMode, lString);
            return oRequest;
        }


        /// <summary>
        ///   Save Accept Of Sub-Contractor In Process.
        /// </summary>
        /// <param name="oModalRequest"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostAcceptSubContractor([FromBody]ProcessRequest oModalRequest)
        {
            oRequest.acceptSubContractor(oModalRequest.oProcessSubContractorMode);
            return oRequest;
        }

        /// <summary>
        ///   Get All Sub Contractors.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetSubContractors(string sStr)
        {
            try
            {
                lString = generalMethod.lSplitString(sStr, ',');
                if (lString.Count == 1)
                    oRequest.GetSubContractors(Convert.ToInt32(lString[0]), null);
                else
                {
                    List<int> New = new List<int>();
                    New = lString.Skip(1).Select(s => int.Parse(s)).ToList();
                    oRequest.GetSubContractors(Convert.ToInt32(lString[0]), New);
                }
                return oRequest;
            }
            catch (Exception ex)
            {
throw;
            }
        }

        /// <summary>
        ///   Get Object Of Sub contractor.
        /// </summary>
        /// <param name="sStr"> Contractor Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetOsubContractor(string sStr)
        {
            oRequest.GetOsubContractor(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Delete Sub Contractor.
        /// </summary>
        /// <param name="sStr"> String Of Contractor Code And Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest DeleteSubContractor([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.DeleteSubContractor(Convert.ToInt32(lString[0]), Convert.ToInt32(lString[1]));
            return oRequest;
        }

        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSearchSubContractor([FromBody] List<string> lStr)
        {
            oRequest.SearchSubContractors(lStr);
            return oRequest;

        }

        /// <summary>
        ///   Gel All Missions For Sub-Contractor In Processs.
        /// </summary>
        /// <param name="sStr"> Contractor Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetMissionSubContractor(string sStr)
        {
            oRequest.GetMissionSubContractor(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Get All Reference Sides - Contractors With Spacial Name.
        /// </summary>
        /// <param name="sStr"> 'Reference Side - Contractor' Name.  </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetContractors([FromUri]string sStr)
        {
            oRequest.GetContractors(sStr);
            return oRequest;
        }

        #endregion

        #region Notes

        /// <summary>
        ///   Get All Notes In Process
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetProcessNotes(string sStr)
        {
            oRequest.GetProcessNotes(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Save Notes In Process.
        /// </summary>
        /// <param name="oModalRequest"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveNotes([FromBody]ProcessRequest oModalRequest)
        {
            oRequest.SaveNotes(oModalRequest.OprocessNotesModel);
            return oRequest;
        }

        #endregion

        #region Opposition

        /// <summary>
        ///   Get All Opposition / Exemption. 
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetAllOpposition(string sStr)
        {
            oRequest.GetAllOpossition(Convert.ToInt32(sStr));
            return oRequest;
        }

        /// <summary>
        ///   Save Opposition / Exemption.
        /// </summary>
        /// <param name="oModalRequest"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveOpposition([FromBody]ProcessRequest oModalRequest)
        {
            oRequest.SaveOpossition(oModalRequest.oprocessOppositionModel);
            return oRequest;
        }

        /// <summary>
        ///   Edit Opposition / Exemption.
        /// </summary>
        /// <param name="oModalRequest"> Edit Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostEditOpposition([FromBody]ProcessRequest oModalRequest)
        {
            oRequest.EditOpossition(oModalRequest.oprocessOppositionModel);
            return oRequest;
        }

        #endregion

        #region Min Max

        /// <summary>
        ///    Save Minimum And Maximum Workers In Process.
        /// </summary>
        /// <param name="oModalRequest"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessRequest PostSaveMinMax([FromBody]ProcessRequest oModalRequest)
        {
            oRequest.SaveMinMax(oModalRequest.OModel);
            return oRequest;
        }

        /// <summary>
        ///   Get Data For Minimum And Maximum.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest GetMinMax(string sStr)
        {
            oRequest.GetMinMax(Convert.ToInt32(sStr));
            return oRequest;
        }

        #endregion

        /// <summary>
        ///  Delete Special Process.
        /// </summary>
        /// <param name="sStr"> String Of Process Code And User Code. </param>
        /// <returns> Request. </returns>
        public ProcessRequest DeleteProcessRequest([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.vDeleteProcess(Convert.ToInt32(lString[0]), Convert.ToInt32(lString[1]));
            return oRequest;
        }
        #endregion
    }
}