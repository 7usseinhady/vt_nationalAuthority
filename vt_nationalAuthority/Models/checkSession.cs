using System.Net;
using System.Web;

namespace vt_nationalAuthority.Models
{
    public static class CheckSession
    {
        /// <summary>
        ///   Navigate To Special Path IF Session Of User Code End.
        /// </summary>
        /// <param name="path"> Special Path. </param>
        /// <returns> Bool. </returns>
        public static bool bCheckUserCode(string path)
        {
            if (HttpContext.Current.Session["uc"] == null)
            {
                HttpContext.Current.Response.Redirect(path);
                return false;
            }
            return true;
        }

        /// <summary>
        ///   Get Ip Address Of Current Device.
        /// </summary>
        public static void vIPAddress()
        {
            string host = Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).HostName;
            IPAddress[] hostIPs = Dns.GetHostAddresses(host);
            foreach (IPAddress hostIP in hostIPs)
            {
                if (hostIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    HttpContext.Current.Session["IPAddress"] = hostIP.ToString();
                }
            }
        }

    }
}