using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vt_nationalAuthority.signalr.hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// Step For Work Signal-R In Project ASP.Net MVC
        /// </summary>
        /// <param name="name">Name Of User</param>
        /// <param name="message">Message For Notification</param>
        public void send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }
    }
}