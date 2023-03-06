using apiWSDLs.srvRefCoverData;
using System;

namespace apiWSDLs.wsdls
{
    public class coverData
    {
        /// <summary>
        ///   Insurance Number.
        /// </summary>
        /// <param name="sInsuranceNumber"> Insurance Number. </param>
        /// <returns> List Of String Of ' Insurance Number' Data. </returns>
        public string[] cover_data(string sInsuranceNumber)
        {
            CNTV05OperationRequest sreq = new CNTV05OperationRequest();
            CNTV05OperationResponse srsp = new CNTV05OperationResponse();
            CNTV05PortTypeClient call = new CNTV05PortTypeClient();
            COMMAREA2 cm2 = new COMMAREA2();
            Commarea_buffer__01[] cmBuffr = new Commarea_buffer__01[3];

            string[] data = new string[2];

            try
            {
                cm2.ws_arc_insnum_comm = sInsuranceNumber;
                cmBuffr = call.CNTV05Operation(cm2);

                for (int i = 0; i <= cmBuffr.Length; i++)
                    data[i] = cmBuffr[i].comm_area_01;
            }
            catch
            {
                return data;
            }

            return data;
        }
    }
}