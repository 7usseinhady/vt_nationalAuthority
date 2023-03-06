using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Attachment Type. 
    /// </summary>
    public class apiAttachmentTypeController : ApiController
    {
        AttachmentTypeRequest oRequest = new AttachmentTypeRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;

        /// <summary>
        ///   Get All Attachment Types.
        /// </summary>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest GetAttachmentTypes()
        {
            oRequest.GetInit();
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Attachment Type.
        /// </summary>
        /// <param name="sStr"> Attachment Type Code.</param>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest GetAttachmentTypes(string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');
            oRequest.GetInitObject(Convert.ToInt32(lString[0]));

            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest PostSearchAttachmentTypes([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewAttachmentType"> New Request </param>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest PostSaveAttachmentTypes([FromBody]AttachmentTypeRequest oNewAttachmentType)
        {
            oRequest.vSave(oNewAttachmentType.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewAttachmentTypes"> Edit Request </param>
        /// <param name="sStr"> Code Of Attachment Type Will Be Edit. </param>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest PostEditAttachmentTypes([FromBody]AttachmentTypeRequest oNewAttachmentTypes, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vEdit(oNewAttachmentTypes.OModel, Convert.ToInt32(lString[0]));
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Attachment Type.
        /// </summary>
        /// <param name="sStr"> Code Of Attachment Type Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public AttachmentTypeRequest DeleteAttachmentTypes([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]));
            return oRequest;
        }
    }
}