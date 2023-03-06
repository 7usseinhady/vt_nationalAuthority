using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    public class apiUsersController : ApiController
    {
        UserRequest oRequest = new UserRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        /// Get List Of All Users
        /// </summary>
        ///<returns> Request. </returns>
        public UserRequest GetUsers()
        {
            oRequest.GetInit();
            return oRequest;
        }


        /// <summary>
        /// Get One User
        /// </summary>
        /// <param name="sStr">User Code</param>
        /// <returns> Request. </returns>
        public UserRequest GetUsers(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetInitObject(Convert.ToInt32(lString[0]));

            return oRequest;
        }

        /// <summary>
        ///  Get List Of All User for special User
        /// </summary>
        /// <param name="sStr">User Code</param>
        /// <returns> Request. </returns>
        public UserRequest GetUsersByUser(string sStr)
        {
            oRequest.GetInitByUser(Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        /// Get Legal Entity For User
        /// </summary>
        /// <param name="sStr">User Code</param>
        /// <returns> Request. </returns>
        public UserRequest GetUserLegalEntity(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetInitObjectLegalEntity(Convert.ToInt32(lString[0]));

            return oRequest;
        }


        /// <summary>
        /// Get All Users In Inurance Authority
        /// </summary>
        /// <returns> Request. </returns>
        public UserRequest GetUserGroup()
        {
            oRequest.GetUserGroup();
            return oRequest;

        }



        /// <summary>
        /// Save User
        /// </summary>
        /// <param name="oNewUser">Data Need For Save</param>
        /// <returns> Request. </returns>
        public UserRequest PostSaveUsers([FromBody]UserRequest oNewUser)
        {
            oRequest.vSave(oNewUser.OModel);
            //GetDocumentTypes();
            return oRequest;
        }



        /// <summary>
        /// Edit User Data
        /// </summary>
        /// <param name="oNewUser">Data Need For Edit</param>
        /// <param name="sStr">User Code</param>
        /// <returns> Request. </returns>
        public UserRequest PostEditUsers([FromBody]UserRequest oNewUser, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vEdit(oNewUser.OModel, Convert.ToInt32(lString[0]));
            //GetDocumentTypes();
            return oRequest;
        }


        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="sStr">Delete Code</param>
        /// <returns> Request. </returns>
        public UserRequest DeleteUsers([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]));

            //lString = generalMethod.lSplitString(sStr, ',');

            //oRequest.vDelete(Convert.ToInt32(lString[0]) , lString[1]);
            return oRequest;
        }



        /// <summary>
        /// Search For Users
        /// </summary>
        /// <param name="lStr">Data Need For Search</param>
        /// <returns> Request. </returns>
        public UserRequest PostSearchUsers([FromBody] List<string> lStr)
        {
            oRequest.vSearchList(lStr);
            var model = oRequest;
            return oRequest;
        }


        /// <summary>
        /// Get Users By User Type
        /// </summary>
        /// <param name="sStr">User Type</param>
        /// <returns> Request. </returns>
        public UserRequest GetUsersType(string sStr)
        {
            oRequest.GetList(sStr);
            return oRequest;
        }


        /// <summary>
        /// Get Offices Insurance By Areas
        /// </summary>
        /// <param name="lStr">Areas Codes</param>
        /// <returns> Request. </returns>
        public UserRequest PostArea([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        /// Update Image For User
        /// </summary>
        /// <param name="oNewUser">Data Need For Update</param>
        /// <param name="sStr">User Code</param>
        /// <returns> Request. </returns>
        public UserRequest PostUpdateImage([FromBody]UserRequest oNewUser, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vUpdateImage(oNewUser.OModel, Convert.ToInt32(lString[0]));
            return oRequest;
        }


        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="oNewUser">Data Need For Change Password</param>
        /// <returns> Request. </returns>
        public UserRequest PostChangeassword([FromBody]UserRequest oNewUser)
        {
            oRequest.vChangePassword(oNewUser.OModel);
            return oRequest;
        }
    }
}