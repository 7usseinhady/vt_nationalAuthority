using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    public class apiGroupController : ApiController
    {
        // GET: apiGroup
        GroupRequest oRequest = new GroupRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        /// Get List Of Groups
        /// </summary>
        /// <returns> Request. </returns>
        public GroupRequest GetGroup()
        {
            oRequest.GetInit();
            return oRequest;
        }

        /// <summary>
        /// Get One Group
        /// </summary>
        /// <param name="sStr">Group Code</param>
        /// <returns> Request. </returns>
        public GroupRequest GetGroup(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetInitObject(Convert.ToInt32(lString[0]));

            return oRequest;
        }


        /// <summary>
        /// Search For Group
        /// </summary>
        /// <param name="lStr">Data Need For Search</param>
        /// <returns> Request. </returns>
        public GroupRequest PostSearchGroup([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        /// Save Group
        /// </summary>
        /// <param name="oModalRequest">Data Need For Save</param>
        /// <returns> Request. </returns>
        public GroupRequest PostSaveGroup([FromBody]GroupRequest oModalRequest)
        {
            oRequest.vSave(oModalRequest.OModel);
            return oRequest;
        }


        /// <summary>
        /// Edit Group
        /// </summary>
        /// <param name="oNewDocumentTypes">Data Need For Edit</param>
        /// <param name="sStr">Group Code</param>
        /// <returns> Request. </returns>
        public GroupRequest PostEditGroup([FromBody]GroupRequest oNewDocumentTypes, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vEdit(oNewDocumentTypes.OModel, Convert.ToInt32(lString[0]));
            return oRequest;
        }


        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="sStr">Group Code</param>
        /// <returns> Request. </returns>
        public GroupRequest DeleteGroup([FromUri]string sStr)
        {
            lString =  generalMethod.lSplitString(sStr, ',');
            oRequest.vDelete(Convert.ToInt32(lString[0]));
            return  oRequest;
        }

        #region GroupUser

        /// <summary>
        /// Get All Users In Group
        /// </summary>
        /// <param name="sStr">Group Code</param>
        /// <returns> Request. </returns>
        public GroupRequest GetGroupUser(string sStr)
        {
            oRequest.GetInitObjectUG(Convert.ToInt32(sStr));

            return oRequest;
        }


        /// <summary>
        /// Save Users In Group
        /// </summary>
        /// <param name="oNewObje">Data Need For Save</param>
        /// <param name="sStr">Users Codes</param>
        /// <returns> Request. </returns>
        public GroupRequest PostSaveGroupUsers([FromBody]GroupRequest oNewObje, [FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.SaveGU(oNewObje.OgroupUserModel, lString);
            return oRequest;
        }


        /// <summary>
        /// Delete Group User And Get All Users In Group
        /// </summary>
        /// <param name="sStr"> String Of Code Of User In Group And Group Code. </param>
        /// <returns> Request. </returns>
        public GroupRequest DeleteUserGroup([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            for (int i = 1; i < lString.Count; i++)
            {
                oRequest.vDeleteGU(Convert.ToInt32(lString[i-1]), Convert.ToInt32(lString[lString.Count - 1]));
            }
            return oRequest;
        }
        #endregion
        #region GroupPermission

        /// <summary>
        /// Get All Ppermisions For Group
        /// </summary>
        /// <param name="sStr">Group Code</param>
        /// <returns> Request. </returns>
        public GroupRequest GetAllFunctions(string sStr)
        {
            oRequest.GetAllFunction(sStr);
            return oRequest;
        }


        /// <summary>
        /// Save Group Permisions
        /// </summary>
        /// <param name="oNewObje">Data Need For Save</param>
        /// <param name="sStr"> String Of Group Code and Permisiond Need For Save. </param>
        /// <returns> Request. </returns>
        public GroupRequest PostSaveGroupPerm([FromBody]GroupRequest oNewObje, [FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            List<int> New = new List<int>();
            New = lString.Skip(1).Select(s => int.Parse(s)).ToList();
            oRequest.SaveGroupPermission(oNewObje.OfunctionModel,Convert.ToInt32(lString[0]),New);
            return oRequest;
        }
        #endregion
    }
}