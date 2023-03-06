using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Document Types. 
    /// </summary>
    public class apiDocumentTypeController : ApiController
    {
        // GET: apiDocumentType نوع المستند
        DocumentTypeRequest oRequest = new DocumentTypeRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        ///   Get All Document Types.
        /// </summary>
        /// <returns> Request. </returns>
        public DocumentTypeRequest GetDocumentTypes()
        {
            oRequest.GetInit();
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of Document Type.
        /// </summary>
        /// <param name="sStr"> Document Type Code. </param>
        /// <returns> Request. </returns>
        public DocumentTypeRequest GetDocumentTypes(string sStr)
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
        public DocumentTypeRequest PostSearchDocumentTypes([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewDocumentType"> New Request. </param>
        /// <returns> Request. </returns>
        public DocumentTypeRequest PostSaveDocumentTypes([FromBody]DocumentTypeRequest oNewDocumentType)
        {
            oRequest.vSave(oNewDocumentType.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewDocumentTypes"> Edit Request </param>
        /// <param name="sStr"> Code Of Document Type Will Be Edit. </param>
        /// <returns> Request. </returns>
        public DocumentTypeRequest PostEditDocumentTypes([FromBody]DocumentTypeRequest oNewDocumentTypes, [FromUri]string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vEdit(oNewDocumentTypes.OModel, Convert.ToInt32(lString[0]));
            return oRequest;
        }


        /// <summary>
        ///   Delete Special Document Type.
        /// </summary>
        /// <param name="sStr"> Code Of Document Type Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public DocumentTypeRequest DeleteDocumentTypes([FromUri] string sStr)
        {
            lString = generalMethod.lSplitString(sStr, ',');

            oRequest.vDelete(Convert.ToInt32(lString[0]));
            return oRequest;
        }

    }
}