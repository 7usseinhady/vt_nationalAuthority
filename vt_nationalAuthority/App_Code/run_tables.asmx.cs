using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace vt_nationalAuthority.App_Code
{
    /// <summary>
    /// Summary description for run_tables
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class run_tables : System.Web.Services.WebService
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        string user_code = HttpContext.Current.Request.Cookies["uc"].Value;
        public run_tables()
        {
        }
        /// <summary>
        /// Get Notifications For User
        /// </summary>
        [WebMethod]
        public void GetNotification()
        {
            int userCode = Convert.ToInt32(user_code);
            var model = db.GetNotifications(userCode,null, "0001-01-01").ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(model));
        }
        private void InitializeComponent()
        {

        }
    }
}
