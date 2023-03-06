using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vt_nationalAuthority
{
    public class userProvider : IUserIdProvider
    {
        /// <summary>
        /// Get User Code From Cookies To Using In Signal-R
        /// </summary>
        /// <param name="request">Request Fo Get User Id</param>
        /// <returns>User Code</returns>
        public string GetUserId(IRequest request)
        {
            string username = null;
            string name = HttpContext.Current.User.Identity.Name;
            username = request.Cookies["uc"].Value.ToString();

            return username;
        }
    }
}