using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Process Users. 
    /// </summary>
    public class apiProcessUsersController : ApiController
    {
        // Get : apiProcessUsers مستخدمين العمليه
        ProcessUsersRequest oRequest = new ProcessUsersRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;

        
        /// <summary>
        ///   Get All Process Users.
        /// </summary>
        /// <param name="sStr"> Process Code. </param>
        /// <returns> View. </returns>
        public ProcessUsersRequest GetProcessUsers([FromUri] string sStr)
        {
            oRequest.GetList(sStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewProcessStop"> New Request. </param>
        /// <returns> Request. </returns>
        public ProcessUsersRequest PostSaveProcessUsers([FromBody]ProcessUsersRequest oNewProcessStop)
        {
            oRequest.vSave(oNewProcessStop.OModel);
            return oRequest;
        }

       
        /// <summary>
        ///   Delete Special Users From Process Users. 
        /// </summary>
        /// <param name="sStr"> String Of Process Code , User Code And Users Will Be Delete. </param>
        /// <returns></returns>
        public ProcessUsersRequest DeleteProcessUsers([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(sStr.Substring((lString[0].Length + lString[1].Length) + 1), (lString[0] + "," + lString[1]));
            return oRequest;
        }

    }
}