using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vt_nationalAuthority.NAT_WSDL;
namespace vt_nationalAuthority.App_Code
{
    public class nat_wsdl
    {
        /// <summary>
        ///   National ID Data Card بيانات بطاقه الرقم القومى
        /// </summary>
        /// <param name="parm">National ID الرقم القومى</param>
        /// <returns> String Of National ID Data Card. </returns>
        public string nat_data(string parm)
        {
            //long parm = 0;

            NAT_WSDL.CNTV02OperationRequest sreq = new CNTV02OperationRequest();
            NAT_WSDL.CNTV02OperationResponse srsp = new CNTV02OperationResponse();
            NAT_WSDL.CNTV02PortTypeClient call = new CNTV02PortTypeClient();
            NAT_WSDL.COMMAREA cm = new COMMAREA();
            NAT_WSDL.COMMAREA2 cm2 = new COMMAREA2();
            NAT_WSDL.commarea_buffer__01 cmBuffr = new commarea_buffer__01();

            string data = "";

            try
            {
                cmBuffr.comm_area_01 = parm;
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