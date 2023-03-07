using System;
using DataAccessLayer.Models;
using System.Web.Http;

namespace apiMobile.Controllers
{
    public class apiSingUpController : ApiController
    {
       /// <summary>
       ///   Sign Up User.
       /// </summary>
       /// <param name="oSignUp"> Object Of User Info Will Be Sign Up. </param>
       /// <returns> Model Of Login. </returns>
        [HttpPost]
        public loginModel signUp([FromBody] loginModel oSignUp)
        {
            try
            {
                loginModel oLoginModel = new loginModel();
                oLoginModel.userInfo = new userInfoModel().GetContractorUserData(oSignUp.lLogData[0], oSignUp.lLogData[1]);

                // Not Found
                if (!oLoginModel.userInfo.logState) // Register
                {
                    userModel userRegister = new userModel();
                    userRegister.sUserFullName = oSignUp.lLogData[0];
                    userRegister.sUserName = oSignUp.lLogData[0];
                    userRegister.sPassword = oSignUp.lLogData[1];
                    userRegister.bIsActive = true;
                    userRegister.iContractorCode = -1;
                    // Register Done
                    if (userRegister.pbSave(userRegister))
                    {
                        oLoginModel.userInfo = new userInfoModel().GetContractorUserData(oSignUp.lLogData[0], oSignUp.lLogData[1]);
                    }

                }
                return oLoginModel;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        ///   Return Reults.
        /// </summary>
        /// <param name="isTrue"> Object Of Results. </param>
        /// <returns> Object Of Results. </returns>
        public results isTrue([FromBody]results isTrue)
        {
            try
            {
                return isTrue;
            }
            catch
            {
                return null;
            }

        }

    }
}