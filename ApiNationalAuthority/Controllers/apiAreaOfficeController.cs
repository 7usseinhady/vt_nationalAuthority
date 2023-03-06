using ApiNationalAuthority.Models;
using DataAccessLayer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiNationalAuthority.Controllers
{
    public class apiAreaOfficeController : ApiController
    {
        //apiAreaOffice
        AreaOfficeRequest oRequest = new AreaOfficeRequest();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        List<string> lString;


        /// <summary>
        /// Get One Permisions Of Office And Area
        /// </summary>
        /// <param name="sStr">Permision Code</param>
        /// <returns> Request. </returns>
        public AreaOfficeRequest GetAllAreaPermission(string sStr)
        {
            oRequest.GetInitObject(Convert.ToInt32(sStr));
            return oRequest;
        }


        /// <summary>
        /// Save Permisions For Offices
        /// </summary>
        /// <param name="oNewPerm">Data Need For Save</param>
        /// <returns> Request. </returns>
        public AreaOfficeRequest PostSavePermission([FromBody]AreaOfficeRequest oNewPerm)
        {
            lString = oNewPerm.OModel.Codes;
            List<int> OfficeCodes = new List<int>();

            OfficeCodes = lString.Skip(1).Select(s => s == "" ? 0 : int.Parse(s)).ToList();

            oRequest.Save(oNewPerm.OModel, Convert.ToInt32(lString[0]), OfficeCodes);
            return oRequest;
        }

        /// <summary>
        /// Get All Areas
        /// </summary>
        /// <returns> Request. </returns>
        public AreaOfficeRequest GetALLArea()
        {
            oRequest.GetALLAreas();
            return oRequest;
        }


        /// <summary>
        /// Get All Areas For Contractors
        /// </summary>
        /// <returns> Request. </returns>
        public AreaOfficeRequest GetALLAreaContractor()
        {
            oRequest.GetALLAreasContractor();
            return oRequest;
        }


        /// <summary>
        /// Search For Offices And Areas
        /// </summary>
        /// <param name="lStr">Data Need For Search</param>
        /// <returns> Request. </returns>
        public AreaOfficeRequest PostSearchAreas([FromBody] List<string> lStr)
        {
            oRequest.vSearch(lStr);
            return oRequest;

        }

        /// <summary>
        /// Search For Permision For Office
        /// </summary>
        /// <param name="lStr">List Of Strings Need For Search</param>
        /// <returns> Request. </returns>
        public AreaOfficeRequest PostSearchAreaOffice([FromBody] List<string> lStr)
        {
            oRequest.vSearchAreaOffice(lStr);
            return oRequest;

        }

    }
}