using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using DataAccessLayer;
using System.Configuration;
using System.Data.SqlClient;

namespace vt_nationalAuthority.App_Code
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        string _un, _kind;
        /// <summary>
        /// Send Notification For Users
        /// </summary>
        /// <param name="un">User Code</param>
        /// <param name="kind">User Type</param>
        public void SendNotification(string un, string kind)
        {
            if (un != "")
            {
                _un = un;
                _kind = kind;
                string sqlcommand = "";
                if (kind == "Contractor")
                {
                    sqlcommand = @"SELECT  processNotesCode, notes FROM  process.processNotes
                                   SELECT  processCode FROM  process.process
                                   SELECT  processSubContractorCode FROM process.processSubContractor
                                   SELECT  processAccept, dateAccept FROM  process.process
                                   SELECT  processUserCode FROM process.processUsers
                                   SELECT [attachmentsCode] FROM [codes].[attachments] WHERE seen =0
                                   ";
                }
                else if (kind == "EmployeeProcess")
                {
                    sqlcommand = @"SELECT   processCode FROM  process.process WHERE seenProcessEmployeeProcess = 0 
                                   SELECT  processOppositionCode FROM   process.processOpposition WHERE process.processOpposition.seen =0
                                   SELECT  [attachmentsCode] FROM [codes].[attachments] WHERE seen =0  ";
                }
                else if (kind == "EmployeeProcessStop")
                {
                    sqlcommand = @"SELECT   processCode,seenProcessEmployee, dateInsert FROM  process.process
                                   SELECT  [attachmentsCode] FROM [codes].[attachments] WHERE seen =0  ";
                }
                string constr = ConfigurationManager.ConnectionStrings["vt_authorityInsuranceConnection"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand(sqlcommand, con);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();

                    cmd.Notification = null;
                    SqlDependency sqlDep = new SqlDependency(cmd);
                    sqlDep.OnChange += SqlDep_OnChange;
                    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    if (kind == "Contractor")
                    {
                        if (GetNotificationCount() != 0)
                            context.Clients.User(_un).count_notification(GetNotificationCount());
                    }
                    else if (kind == "EmployeeProcess")
                    {
                        if (GetNotificationCountProcess() != 0)
                            context.Clients.User(_un).count_notificationProcess(GetNotificationCountProcess());
                    }
                    else if (kind == "EmployeeProcessStop")
                    {
                        if (GetNotificationCountProcessStop() != 0)
                            context.Clients.User(_un).count_notificationProcessStop(GetNotificationCountProcessStop());
                    }
                    SqlDependency.Start(constr);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //nothing for now
                    }
                }
            }
        }
        /// <summary>
        /// Function Built In ASP.Net Application To Sense Change In SQL Server 
        /// </summary>
        /// <param name="sender">Object Parameter</param>
        /// <param name="e">SqlNotificationEventArgs</param>
        private void SqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SendNotification(_un, _kind);
            }
        }
        /// <summary>
        /// Get Count Of Notification For User Not Seen
        /// </summary>
        /// <returns>Number Of Notifications</returns>
        protected int GetNotificationCount()
        {
            int count = 0;
            int userCode = Convert.ToInt32(_un);
            count = db.GetNotifications(userCode,null, "0001-01-01").Where(x => x.seen == false).ToList().Count;
            return count;
        }
        /// <summary>
        /// Get Notification Count Of Process Not Seen
        /// </summary>
        /// <returns>Number Of Notifications</returns>
        protected int GetNotificationCountProcess()
        {
            try
            {
                int count = 0;
                int userCode = Convert.ToInt32(_un);
                count = db.GetNotifications(userCode,null, "0001-01-01").Where(x => x.seen == false).ToList().Count;
                return count;
            }
            catch (Exception ex)
            {
throw;
            }
           
        }
        /// <summary>
        /// Get Count Of Process Stop Not Seen
        /// </summary>
        /// <returns>Number Of Notifications</returns>
        protected int GetNotificationCountProcessStop()
        {
            int count = 0;
            int userCode = Convert.ToInt32(_un);
            count = db.GetEmployeeNotificationProcessStop(userCode).Where(x => x.seenProcessEmployee == false).ToList().Count;
            return count;
        }
    }
}