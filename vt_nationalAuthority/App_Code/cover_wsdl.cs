using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vt_nationalAuthority.COVER_WSDL;

namespace vt_nationalAuthority.App_Code
{
    public class cover_wsdl
    {
        /// <summary>
        ///   Insurance Number.
        /// </summary>
        /// <param name="parm"> Insurance Number. </param>
        /// <returns> List Of String Of ' Insurance Number' Data. </returns>
        public string[] cover_data(string parm)
        {
            COVER_WSDL.CNTV05OperationRequest sreq = new CNTV05OperationRequest();
            COVER_WSDL.CNTV05OperationResponse srsp = new CNTV05OperationResponse();
            COVER_WSDL.CNTV05PortTypeClient call = new CNTV05PortTypeClient();
            COVER_WSDL.COMMAREA2 cm2 = new COMMAREA2();
            COVER_WSDL.Commarea_buffer__01[] cmBuffr = new Commarea_buffer__01[3];

            string[] data = new string[2];

            try
            {
                cm2.ws_arc_insnum_comm = parm;
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