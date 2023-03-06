using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    /// <summary>
    ///   Api Of Reference Sides - Contractors. 
    /// </summary>
    public class apiReferenceSideContractorController : ApiController
    {
        // Get : apiReferenceSideContractor جهه الاسناد - المقاولين

        ReferenceSideContractorRequest oRequest = new ReferenceSideContractorRequest();

        /// <summary>
        ///   Get All Reference Sides - Contractors.
        /// </summary>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest GetRefSideCont()
        {
            oRequest.GetInit();
            return oRequest;
        }

        /// <summary>
        ///   Get Object Of 'Reference Side - Contractor'.
        /// </summary>
        /// <param name="sStr"> 'Reference Side - Contractor' Code.</param>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest GetRefSideCont(string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr));

            return oRequest;
        }


        /// <summary>
        ///   Search With Special Parameters.
        /// </summary>
        /// <param name="lStr"> List Of Special Parameters That Will Search On It. </param>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest PostSearchRefSideCont([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;
        }


        /// <summary>
        ///   Save Object Data In Database.
        /// </summary>
        /// <param name="oNewRefSideCont"> New Request </param>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest PostSaveRefSideCont([FromBody]ReferenceSideContractorRequest oNewRefSideCont)
        {
            oRequest.vSave(oNewRefSideCont.OModel);
            return oRequest;
        }


        /// <summary>
        ///   Edit Object Data In Database.
        /// </summary>
        /// <param name="oNewRefSideCont"> Edit Request </param>
        /// <param name="sStr"> Code Of 'Reference Side - Contractor' Will Be Edit. </param>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest PostEditRefSideCont([FromBody]ReferenceSideContractorRequest oNewRefSideCont, [FromUri]string sStr)
        {
            oRequest.vEdit(oNewRefSideCont.OModel, 0);
            return oRequest;
        }


        /// <summary>
        ///   Delete Special 'Reference Side - Contractor'.
        /// </summary>
        /// <param name="sStr"> Code Of 'Reference Side - Contractor' Will Be Delete.  </param>
        /// <returns> Request. </returns>
        public ReferenceSideContractorRequest DeleteRefSideCont([FromUri] string sStr)
        {
            oRequest.vDelete(Convert.ToInt32(sStr));
            return oRequest;
        }

    }
}
