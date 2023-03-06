using apiWSDLs.srvRefNationalData;
using System;

namespace apiWSDLs.wsdls
{
    public class nationalIDData
    {
        /// <summary>
        ///   National ID Data Card بيانات بطاقه الرقم القومى
        /// </summary>
        /// <param name="sNationalID">National ID الرقم القومى</param>
        /// <returns> String Of National ID Data Card. </returns>
        public string nat_data(string sNationalID)
        {
            CNTV02OperationRequest sreq = new CNTV02OperationRequest();
            CNTV02OperationResponse srsp = new CNTV02OperationResponse();
            CNTV02PortTypeClient call = new CNTV02PortTypeClient();
            COMMAREA cm = new COMMAREA();
            COMMAREA2 cm2 = new COMMAREA2();
            commarea_buffer__01 cmBuffr = new commarea_buffer__01();

            string data = "";

            try
            {
                cmBuffr.comm_area_01 = sNationalID;
                cm.buffer_01 = cmBuffr;
                cm2 = call.CNTV02Operation(cm);
                data = cm2.ws_arc_insnum_comm;
            }
            catch
            {
                data = "Error";
            }

            return data;
        }

    }
}