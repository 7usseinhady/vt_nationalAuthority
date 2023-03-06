using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vt_nationalAuthority.att_wsdl;

namespace vt_nationalAuthority.App_Code
{
    public class att_wsdl_cls
    {
        /// <summary>
        ///   The First Attendance Of The Worker In The Month.
        /// </summary>
        /// <param name="cnttran_code_b">First Type Of 1.Attendance , 2.MedicalInsurance , 3.ManPower , 4.Empty  حضور 1 - تأمين صحى 2 - قوى عامله 3 - فاضى 4</param>
        /// <param name="workerNationalId">Worker National ID  رقم قومى للعامل</param>
        /// <param name="dateAttend">Date Attend For Attendance , MedicalInsurance , ManPower  تاريخ حضور - تأمين صحى - قوى عامله.</param>
        /// <param name="legalContractor">Legal Contractor 1.Individual , 2.Corporation  كيان المنشأه 1. فرد 2.منشأه</param>
        /// <param name="contractorInsuranceNumber">Contractor Insurance Number رقم المقاول بالعمليه</param>
        /// <param name="processNumber">Process Number رقم العمليه</param>
        /// <param name="careerID">Worker Career ID رقم مهنه العامل</param>
        /// <param name="cnt_result_code_b">Result Code Of MedicalInsurance , ManPower نتيجه التأمين الصحى - القوى العامله</param>
        /// <param name="cnt_result_place_b">Result Place Of MedicalInsurance , ManPower مكان التأمين الصحى - القوى العامله</param>
        /// <param name="work_date_b">Result Date Of MedicalInsurance , ManPower تاريخ نتيجه التأمين الصحى - القوى العامله</param>
        public string[] att_data(string cnttran_code_b, long workerNationalId, int dateAttend, string legalContractor, int contractorInsuranceNumber,
                     long processNumber,int careerID, short cnt_result_code_b, int cnt_result_place_b, int work_date_b)
        {
            att_wsdl.CNTV06OperationRequest oReq = new CNTV06OperationRequest();
            att_wsdl.CNTV06OperationResponse oRsp = new CNTV06OperationResponse();
            att_wsdl.CNTV06PortTypeClient oClient = new CNTV06PortTypeClient();
            att_wsdl.DFHCOMMAREA dfhCom = new DFHCOMMAREA();
            att_wsdl.COMMAREA comm = new COMMAREA();

            string[] status = new string[2];

            try
            {
                // 1 حضور
                // 2 تأمين صحى
                // 3 قوى عامله
                // 4 فاضى

                comm.cnttran_code_b = cnttran_code_b;
                comm.cnttran_number_b = workerNationalId; // رقم قومى للعامل
                comm.cnttran_date_b = dateAttend; // تاريخ حضور / تأمين صحى / قوى عامله
                comm.cnt_number_kind_b = legalContractor; // رقم مقاول 1 فرد 2 منشأه
                comm.cnt_number_b = contractorInsuranceNumber; // رقم المقاول
                comm.cnt_operation_num_b = processNumber; // رقم العمليه
                comm.cnt_number_job_b = careerID;
                comm.cnt_result_code_b = cnt_result_code_b; // نتيجه التأمين الصحى / القوى العامله
                comm.cnt_result_place_b = cnt_result_place_b; // مكان التأمين الصحى / الوى العامله
                comm.work_date_b = work_date_b; // تاريخ نتيجه التأمين الصحى / القوى العامله
                dfhCom = oClient.CNTV06Operation(comm);
                // 1 , تم الكتابه 
                //2 لم يتم الكتابه
                status[0] = dfhCom.processing_statuscode; //status code
                status[1] = dfhCom.processing_statusdesc; // status description
                                                          //var result = oClient.CNTV06OperationAsync(dfhCom);
            }
            catch
            {

            }

            return status;
        }
    }
}