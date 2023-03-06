using apiWSDLs.attMonth;
using System;

namespace apiWSDLs.wsdls
{
    public class firstAttInMonthWsdl
    {
        /// <summary>
        ///   The First Attendance Of The Worker In The Month.
        /// </summary>
        /// <param name="firstTypOf">First Type Of 1.Attendance , 2.MedicalInsurance , 3.ManPower , 4.Empty  حضور 1 - تأمين صحى 2 - قوى عامله 3 - فاضى 4</param>
        /// <param name="workerNationalId">Worker National ID  رقم قومى للعامل</param>
        /// <param name="dateAttend">Date Attend For Attendance , MedicalInsurance , ManPower  تاريخ حضور - تأمين صحى - قوى عامله.</param>
        /// <param name="legalContractor">Legal Contractor 1.Individual , 2.Corporation  كيان المنشأه 1. فرد 2.منشأه</param>
        /// <param name="contractorInsuranceNumber">Contractor Insurance Number رقم المقاول بالعمليه</param>
        /// <param name="processNumber">Process Number رقم العمليه</param>
        /// <param name="workerCareerID">Worker Career ID رقم مهنه العامل</param>
        /// <param name="resultCode">Result Code Of MedicalInsurance , ManPower نتيجه التأمين الصحى - القوى العامله</param>
        /// <param name="resultPlace">Result Place Of MedicalInsurance , ManPower مكان التأمين الصحى - القوى العامله</param>
        /// <param name="resultDate">Result Date Of MedicalInsurance , ManPower تاريخ نتيجه التأمين الصحى - القوى العامله</param>
        public string[] saveFirstAttInMonthInWsdl(string firstTypOf, string workerNationalId, string dateAttend, string legalContractor, string contractorInsuranceNumber,
                     string processNumber, string workerCareerID, string resultCode, string resultPlace, string resultDate)
        {
            CNTV06OperationRequest oReq = new CNTV06OperationRequest();
            CNTV06OperationResponse oRsp = new CNTV06OperationResponse();
            CNTV06PortTypeClient oClient = new CNTV06PortTypeClient();
            DFHCOMMAREA dfhCom = new DFHCOMMAREA();
            COMMAREA comm = new COMMAREA();

            string[] status = new string[2] { "-100", "Failed WSDL" };

            try
            {
                // workerCareerID =  workerCareerID.ToString().Length == 9 ? Convert.ToInt32(workerCareerID.ToString().Substring(2, 6)) : workerCareerID;

                comm.cnttran_code_b = firstTypOf; // حضور 1 - تأمين صحى 2 - قوى عامله 3 - فاضى 4
                comm.cnttran_number_b = workerNationalId; // رقم قومى للعامل
                comm.cnttran_date_b = dateAttend; // تاريخ حضور - تأمين صحى - قوى عامله 
                comm.cnt_number_kind_b = legalContractor; // كيان المنشأه 1. فرد 2.منشأه
                comm.cnt_number_b = contractorInsuranceNumber; // رقم المقاول
                comm.cnt_operation_num_b = processNumber; // رقم المقاول بالعمليه
                comm.cnt_number_job_b = "000000"; // رقم مهنه العامل
                comm.cnt_result_code_b = "0"; // نتيجه التأمين الصحى / القوى العامله
                comm.cnt_result_place_b = "000000"; // مكان التأمين الصحى / الوى العامله
                comm.work_date_b = "00000000"; // تاريخ نتيجه التأمين الصحى / القوى العامله
                dfhCom = oClient.CNTV06Operation(comm);

                status[0] = String.IsNullOrEmpty(dfhCom.processing_statuscode) ? "0" : dfhCom.processing_statuscode; //status code تم الكتابه 1 - لم يتم الكتابه 2
                status[1] = dfhCom.processing_statusdesc; // status description وصف الحاله
            }
            catch
            {
                return status;
            }
            return status;
        }

    }
}