using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Users
{
    public class AreaOfficeController : Controller
    {
        // GET: AreaOffice
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        AreaOfficeRequest oRequest = new AreaOfficeRequest();
        ConnectionApi conApi = new ConnectionApi();
        private readonly GeneralMethods generalMethod = new GeneralMethods();
        static int? insPageNumber;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        static int? code;
        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public AreaOfficeController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Page of Offices And Areas
        /// </summary>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <param name="inPage">Current Number Page</param>
        /// <returns>View Page</returns>
        public ActionResult vAreaOfficeIndex(string areas, string Offices, int? inPage)
        {
            try
            {
                insPageNumber = String.IsNullOrEmpty(inPage.ToString()) ? 1 : inPage;

                //insPageNumber = (insPageNumber == null ? 1 : inPage);

                if (String.IsNullOrEmpty(areas) && String.IsNullOrEmpty(Offices))
                    oRequest = conApi.connectionApiGetList<AreaOfficeRequest>("apiAreaOffice", "GetALLArea", null);
                else
                {
                    List<string> lSearch = new List<string>();
                    lSearch.Add(String.IsNullOrEmpty(areas) ? null : areas);
                    lSearch.Add(String.IsNullOrEmpty(Offices) ? null : Offices);
                    oRequest = conApi.connectionApiSearchList<AreaOfficeRequest>("apiAreaOffice", "PostSearchAreas", lSearch);
                }
                return View(oRequest.LModels.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Page Of Areas And Offices Permissions
        /// </summary>
        /// <param name="OffCode">Office Code</param>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View Page Of Offices And Areas</returns>
        public ActionResult vAreaOfficeSetting(int? OffCode, string areas, string Offices)
        {
            try
            {
                code = (OffCode == null ? code : OffCode);
                TempData["areaName"] = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == code).area.areaName;
                TempData["OfficeName"] = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == code).officeInsuranceName;

                if (String.IsNullOrEmpty(areas) && String.IsNullOrEmpty(Offices))
                    oRequest = conApi.connectionApiGetList<AreaOfficeRequest>("apiAreaOffice", "GetAllAreaPermission", code.ToString());
                else
                {
                    List<string> Lsearch = new List<string>();
                    Lsearch.Add(code.ToString());
                    Lsearch.Add(String.IsNullOrEmpty(areas) ? null : areas);
                    Lsearch.Add(String.IsNullOrEmpty(Offices) ? null : Offices);
                    oRequest = conApi.connectionApiSearchList<AreaOfficeRequest>("apiAreaOffice", "PostSearchAreaOffice", Lsearch);

                }

                return View(oRequest.LModels);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Permission For Office  And Area
        /// </summary>
        /// <param name="areas">List Of Areas Filter Pages</param>
        /// <param name="Offices">List Of Offices Filter Pages</param>
        /// <returns>View List of Permissions</returns>
        public ActionResult vAreaOfficeContractorIndex(string areas, string Offices)
        {
            if (String.IsNullOrEmpty(areas) && String.IsNullOrEmpty(Offices))
                oRequest = conApi.connectionApiGetList<AreaOfficeRequest>("apiAreaOffice", "GetALLAreaContractor", null);
            else
            {
                List<string> Lsearch = new List<string>();
                Lsearch.Add(null);
                Lsearch.Add(String.IsNullOrEmpty(areas) ? null : areas);
                Lsearch.Add(String.IsNullOrEmpty(Offices) ? null : Offices);
                oRequest = conApi.connectionApiSearchList<AreaOfficeRequest>("apiAreaOffice", "PostSearchAreaOffice", Lsearch);

            }
            return View(oRequest.LModels);
        }
        /// <summary>
        /// Save And Edit Permissions
        /// </summary>
        /// <param name="permissionValue">Permissions Codes</param>
        /// <returns>True For Save Correct ,  False If Have Issue</returns>
        public bool EditOfficePermission(string permissionValue)
        {
            try
            {
                List<string> Lstr = new List<string>();
                List<string> listCodes = new List<string>();
                string OfficeCode = "";
                listCodes = generalMethod.lSplitString(permissionValue, ',');
                Lstr.AddRange(listCodes.Distinct());
                for (int i = 0; i < Lstr.Count; i++)
                {
                    if (Lstr[i].Contains("officeInsurance"))
                    {
                        List<string> Spliter = new List<string>();
                        Spliter = generalMethod.lSplitString(Lstr[i], '-');
                        OfficeCode = Spliter[0] + "," + OfficeCode;
                    }
                }
                string lareaOfficePerm = "";
                lareaOfficePerm = code + "," + OfficeCode;
                List<string> lstring = new List<string>();
                lstring = generalMethod.lSplitString(lareaOfficePerm, ',');
                AreaOfficeRequest Omodel = new AreaOfficeRequest();
                Omodel.OModel = new DataAccessLayer.Models.PermissionAreaOfficeModel();
                int userCode = Convert.ToInt32(Session["uc"].ToString());
                Omodel.OModel.inUserInsertCode = userCode;
                Omodel.OModel.inUserUpdateCode = userCode;
                Omodel.OModel.Codes = lstring;
                oRequest = conApi.connectionApiPost<AreaOfficeRequest>("apiAreaOffice", "PostSavePermission", Omodel, null);
                if (oRequest.bIsSaved)
                {
                    TempData["msg"] = generalVariables.SaveDone;
                    return true;
                }
                else
                {
                    TempData["msg"] = generalVariables.SaveNotDone;
                    return false;
                }
            }
            catch
            {
                TempData["msg"] = generalVariables.SaveNotDone;
                return false;
            }
        }
        /// <summary>
        /// Edit Insurance Offices For Contractors
        /// </summary>
        /// <param name="Values">Offices Codes</param>
        /// <returns>True For Save Correct ,  False If Have Issue</returns>
        public bool EditOfficeContractor(string Values)
        {
            try
            {
                List<string> Lstr = new List<string>();
                Lstr.AddRange(generalMethod.lSplitString(Values, ',').Distinct());

                // Update All Offices Contractor And Set New Offices Contractors
                db.spUpdateOfficesContractorFalse();

                for (int i = 0; i < Lstr.Count; i++)
                {
                    if (Lstr[i].Contains("officeInsurance"))
                    {
                        List<string> Spliter = new List<string>();
                        Spliter = generalMethod.lSplitString(Lstr[i], '-');
                        int value = 0;
                        if (int.TryParse(Spliter[0], out value))
                        {
                            int code = Convert.ToInt32(Spliter[0]);
                            officeInsurance Office = db.officeInsurances.FirstOrDefault(x => x.officeInsuranceCode == code);
                            Office.contractor = true;
                            db.SaveChanges();
                        }
                    }
                }

                if (db.SaveChanges() > 0)
                {
                    TempData["msg"] = generalVariables.SaveDone;
                    return true;
                }
                else
                {
                    TempData["msg"] = generalVariables.SaveNotDone;
                    return false;
                }
            }
            catch
            {
                TempData["msg"] = generalVariables.SaveNotDone;
                return false;
            }
        }
    }
}