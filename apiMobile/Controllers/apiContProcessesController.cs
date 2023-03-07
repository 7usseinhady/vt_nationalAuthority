using DataAccessLayer.Models;
using System.Web.Http;

namespace ApiMobile.Controllers
{
    public class apiContProcessesController : ApiController
    {
        /// <summary>
        ///   Get All Processes Of Contractor.
        /// </summary>
        /// <param name="iUcode"> User Code. </param>
        /// <returns> Model Of Contractor Process. </returns>
        [HttpPost]
        public contractorProcessesModel PostContProcesses([FromUri] int iUcode)
        {
            try
            {
                contractorProcessesModel oContractorProcessesModel = new contractorProcessesModel();
                oContractorProcessesModel.lContProcesses = new contProcessesModel().GetContractorProcesses(iUcode);
                return oContractorProcessesModel;
            }
            catch
            {
                return null;
            }
        }

    }
}