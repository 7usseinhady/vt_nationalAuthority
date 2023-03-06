using apiWSDLs.srvRefContRefSidesData;

namespace apiWSDLs.wsdls
{
    public class contRefSidesData
    {
        /// <summary>
        ///   Contractor - reference Side Data. 
        /// </summary>
        /// <param name="sContRefSideInsuranceNumber"> 'Contractor - Reference Side' Insurance Number. </param>
        /// <returns> List Of String 'Contractor - Reference Side' Data. </returns>
        public string[] contRefSideData(string sContRefSideInsuranceNumber)
        {
            string[] contRefSideData = new string[3];

            CNTV04OperationRequest sreq = new CNTV04OperationRequest();
            CNTV04OperationResponse srsp = new CNTV04OperationResponse();
            CNTV04PortTypeClient call = new CNTV04PortTypeClient();
            COMMAREA02 cm2 = new COMMAREA02();
            Commarea_buffer__01[] cmBuffr = new Commarea_buffer__01[3];
            cm2.arc_gpsbin_comm = sContRefSideInsuranceNumber;

            cmBuffr = call.CNTV04Operation(cm2);

            for (int i = 0; i < cmBuffr.Length; i++)
            {
                contRefSideData[i] = cmBuffr[i].comm_area_01;
            }

            return contRefSideData;
        }
    }
}