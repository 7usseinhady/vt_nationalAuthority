using apiWSDLs.openCoverWSDL;
using System;

namespace apiWSDLs.wsdls
{
    public class openCoverWsdl
    {
        /// <summary>
        ///   Open Cover For Worker For This Month.
        /// </summary>
        /// <param name="iInsuranceNum"> Insurance Number. </param>
        /// <param name="date"> Cover Date. </param>
        /// <param name="areaID"> Area ID. </param>
        /// <param name="officeID"> Office ID. </param>
        /// <param name="mahara"> Worker Career. </param>
        /// <param name="processNumber"> Process Number. </param>
        /// <returns> WSDL Response. </returns>
        public string[] openCover(string iInsuranceNum,string date , string areaID ,string officeID , string mahara , string processNumber)
        {
            CNTCOVOperationRequest sreq = new CNTCOVOperationRequest();
            CNTCOVOperationResponse srsp = new CNTCOVOperationResponse();
            CNTCOVPortTypeClient call = new CNTCOVPortTypeClient();

            DFHCOMMAREA dfhCom = new DFHCOMMAREA();
            DFHCOMMAREA1 dfhCom2 = new DFHCOMMAREA1();
            dfhcommarea_buffer cmBuffr = new dfhcommarea_buffer();

            string[] status = new string[2] { "-100", "Failed WSDL" };
            try
            {

                cmBuffr.payer_number = iInsuranceNum;
                cmBuffr.date1 = date;
                cmBuffr.zone1 = areaID;
                cmBuffr.ofic1 = officeID;
                cmBuffr.mahara1 = "43";
                cmBuffr.cnt_num = processNumber;

                dfhCom.buffer = cmBuffr;
                dfhCom2 = call.CNTCOVOperation(dfhCom); 
                
                status[0] = dfhCom2.buffer.processing_statusdescc;
                status[1] = dfhCom2.buffer.processing_statusdesc;
            }
            catch
            {
                return status;
            }

            return status;
        }
    }
}