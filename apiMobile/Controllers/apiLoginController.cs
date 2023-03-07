using DataAccessLayer.Models;
using System;
using System.Web.Http;

namespace ApiMobile.Controllers
{
    public class apiLoginController : ApiController
    {
        /// <summary>
        ///   Check User Login Information.
        /// </summary>
        /// <param name="oLogin"> Object Of Login Model. </param>
        /// <returns> Model Of Login. </returns>
        [HttpPost]
        public loginModel chkUserLogin([FromBody] loginModel oLogin)
        {
            try
            {
                loginModel oLoginModel = new loginModel();
                oLoginModel.userInfo = new userInfoModel().GetContractorUserData(oLogin.lLogData[0], oLogin.lLogData[1]);
                return oLoginModel;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        ///   Update 'Reference Side - Contractor' Data.
        /// </summary>
        [HttpPost]
        public void updatContractors()
        {
            try
            {
                referenceSideContractorModel refSide = new referenceSideContractorModel();
                refSide.vUpdateContractors();
            }
            catch (Exception ex)
            {
throw;
            }
        }

    }
}