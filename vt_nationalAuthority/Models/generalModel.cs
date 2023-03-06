using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace vt_nationalAuthority.Models
{
    public class generalModel
    {
        public static string vIPAddress()
        {
            //string host = Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["MS_HttpContext"]).HostName;
            string host = Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).HostName;
            IPAddress[] hostIPs = Dns.GetHostAddresses(host);
            foreach (IPAddress hostIP in hostIPs)
            {
                if (hostIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return hostIP.ToString();
                }
            }
            return "0";
        }
    }
}