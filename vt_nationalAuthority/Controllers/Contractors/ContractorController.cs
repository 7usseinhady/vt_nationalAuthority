using ApiNationalAuthority.Models;
using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Contractors
{
    public class ContractorController : Controller
    {
        ProcessRequest OprocessRequst, LSearchProcessRequst = new ProcessRequest();
        ConnectionApi conApi = new ConnectionApi();
        GeneralMethods generalMethods = new GeneralMethods();
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();
        static int? ProcessCode, insPageNumber, cd4_;
        static string contractors_, Screen_, screenSearch;
        static ProcessRequest Model;
        int iPageSize = Convert.ToInt32(generalVariables.PageSize);
        // GET: Contractor
        /// <summary>
        ///  Constructor Function , going to Login Page when session for User End
        /// </summary>
        public ContractorController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }
        /// <summary>
        /// Data for Dropdown lists 
        /// </summary>
        void ViewBags()
        {

            ViewBag.OperContractor = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
            ViewBag.hidContactors_ = new SelectList(db.referenceSideContractors.ToList(), "referenceSideContractorCode", "referenceSideContractorName");

        }
        /// <summary>
        /// Data for process in contractors and refrence side
        /// </summary>
        /// <param name="inPage">page number is showing </param>
        /// <param name="cp">contractor code</param>
        /// <param name="pCode">process code</param>
        /// <param name="pNum">page number</param>
        /// <returns>view page</returns>
        public ActionResult vContractorIndex(int? inPage, int? cp, int? pCode, string pNum)
        {
            try
            {
                Session["processCode"] = null;
                TempData["userContractor"] = false;
                TempData["userReferenceSide"] = false;
                var user = conApi.connectionApiGetList<UserRequest>("apiUsers", "GetUsers", Session["uc"].ToString()); 
                if (user != null)
                {
                    TempData["userContractor"] = user.OModel.iContractorCode != null ? true : false;
                    TempData["userReferenceSide"] = user.OModel.iReferenceSideCode != null ? true : false;
                }
                Model = new ProcessRequest();
                Model.LoProcessSubContractorMode = null;
                TempData["cp"] = cp;
                TempData["pCode"] = pCode;
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// show contractors in process
        /// </summary>
        /// <param name="ProcessId">process code</param>
        /// <param name="inPage">page number</param>
        /// <param name="Cont">user contractor</param>
        /// <param name="screen">previous page </param>
        /// <param name="cd4">contractor permissiom</param>
        /// <returns>contractors</returns>
        public ActionResult _vpContractors(int? ProcessId, int? inPage, string Cont, string screen, int? cd4)
        {
            try
            {
                if (cd4 != null)
                {
                    cd4_ = cd4;
                    int uc = Convert.ToInt32(Session["uc"].ToString());
                    TempData["checkLevel4"] = db.CheckModuleUserPermisiom(uc, cd4).ToList();
                }
                TempData["screen"] = screen;
                Screen_ = (screen == "InsuranceEmployee" ? "" : screen);
                contractors_ = Cont;
                insPageNumber = inPage;
                ProcessCode = (ProcessId == null ? Convert.ToInt32(Session["processCode"].ToString()) : ProcessId);
                if (Model == null)
                    Model = new ProcessRequest();
                ViewBag.OperContractor = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
                TempData["hidContactors"] = new SelectList(db.referenceSideContractors.ToList(), "referenceSideContractorCode", "referenceSideContractorName");
                TempData["hidContactorsedit"] = new SelectList(db.referenceSideContractors.ToList(), "referenceSideContractorCode", "referenceSideContractorName");
                if (ProcessId != null)
                {
                    if (!String.IsNullOrEmpty(Cont))
                        OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetSubContractors", ProcessCode.ToString() + "," + Cont);

                    else if (Model.LoProcessSubContractorMode == null)
                        OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetSubContractors", ProcessCode.ToString());
                    else
                    {
                         OprocessRequst = new ProcessRequest();
                         OprocessRequst.LoProcessSubContractorMode = new List<DataAccessLayer.Models.ProcessSubContractorModel>();
                         OprocessRequst.LoProcessSubContractorMode = Model.LoProcessSubContractorMode;
                    }
               
                }
                if (OprocessRequst == null)
                {
                    OprocessRequst = new ProcessRequest();
                    OprocessRequst.LoProcessSubContractorMode = new List<DataAccessLayer.Models.ProcessSubContractorModel>();
                }
                if (OprocessRequst.LoProcessSubContractorMode == null)
                    OprocessRequst.LoProcessSubContractorMode = new List<DataAccessLayer.Models.ProcessSubContractorModel>();
                TempData["totalPages"] = Convert.ToInt32(Math.Ceiling((double)OprocessRequst.LoProcessSubContractorMode.ToList().Count / iPageSize));

                return PartialView(OprocessRequst.LoProcessSubContractorMode.ToPagedList(insPageNumber ?? 1, iPageSize));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        ///page to insertt / Edit hidden contractors
        /// </summary>
        /// <param name="ID">code hidden contractor</param>
        /// <returns>data of hidden contractor if Edit /  empty if insert</returns>
        public ActionResult _vpContractorData(int? ID)
        {
            try
            {
                TempData["hidContactors"] = "";
                ViewBag.hidContactors_ = new SelectList(db.referenceSideContractors.ToList(), "referenceSideContractorCode", "referenceSideContractorName");
                ViewBag.OperContractor = new SelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName");
                TempData["hidContactors"] = new SelectList(db.referenceSideContractors.ToList(), "referenceSideContractorCode", "referenceSideContractorName");
                if (ID != null)
                {
                    var users = db.missionSubContractors.Where(x => x.processSubContractorCode == ID).Select(x => x.processTypeCode).ToList();
                    ViewBag.OperContractor = new MultiSelectList(db.processTypes.ToList(), "processTypeCode", "processTypeName", users);
                    OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetOsubContractor", ID.ToString());
                    TempData["iContractorNationalNumber"] = OprocessRequst.oProcessSubContractorMode.iContractorNationalNumber;
                    TempData["sContractorName"] = OprocessRequst.oProcessSubContractorMode.sContractorName;
                    return PartialView(OprocessRequst);
                }
                else
                {
                    return PartialView(OprocessRequst);
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// function to insert / Edit hidden contractors
        /// </summary>
        /// <param name="NewObject">data of hidden contractor</param>
        /// <param name="LhidContrSearch">codes of hidden contractors</param>
        /// <param name="OperContractor">mission for hidden contractor</param>
        /// <param name="formCollection">Continue Data for hidden contractor</param>
        /// <param name="submit">submit for insert / Edit OR search for searching</param>
        /// <returns>hidden contractor page</returns>
        [HttpPost]
        public ActionResult _vpContractorData(ProcessRequest NewObject, string[] LhidContrSearch, string[] OperContractor, FormCollection formCollection, string submit)
        {
            try
            {
                string screen = ""; //ContNatNum
                NewObject.oProcessSubContractorMode.inUserInsertCode = Convert.ToInt32(Session["uc"].ToString());
                NewObject.oProcessSubContractorMode.inUserUpdateCode = Convert.ToInt32(Session["uc"].ToString());
                if (!string.IsNullOrEmpty(formCollection["screen"]))
                    screen = formCollection["screen"].ToString();
                if (submit == "Search")
                    return RedirectToAction("_vpDeleteSearchContractor", new { LhidContr = LhidContrSearch, LOperContr = OperContractor, txtNatContr = formCollection["ContNatNum"].ToString(), Action = "_vpContractors" });
                else
                {
                    string InsurNum = Convert.ToInt32(formCollection["contractorInsNum"].ToString()).ToString("000000000");
                    NewObject.oProcessSubContractorMode.iContractorCode = db.referenceSideContractors.FirstOrDefault(x => x.referenceSideContractorInsuranceNum == InsurNum).referenceSideContractorCode;
                    if (NewObject.oProcessSubContractorMode.iProcessSubContractorCode > 0) //edit
                    {
                        NewObject.oProcessSubContractorMode.iProcessCode = (int)ProcessCode;
                        NewObject.oProcessSubContractorMode.bnContractorType = false;
                        string Mission = "";
                        if (OperContractor != null)
                            Mission = generalMethods.sConcatString(OperContractor, ',');
                        OprocessRequst = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostEditSubContractor", NewObject, Mission);
                        if (OprocessRequst.bIsEdit)
                            TempData["msg"] = generalVariables.EditDone;
                        else
                            TempData["msg"] = generalVariables.EditNotDone;
                    }
                    else
                    {
                        NewObject.oProcessSubContractorMode.iProcessCode = (int)ProcessCode;
                        NewObject.oProcessSubContractorMode.bnContractorType = false;
                        string Mission = "";
                        if (OperContractor != null)
                            Mission = generalMethods.sConcatString(OperContractor, ',');
                        OprocessRequst = conApi.connectionApiPost<ProcessRequest>("apiProcess", "PostSaveSubContractor", NewObject, Mission);
                        if (OprocessRequst.bIsSaved)
                            TempData["msg"] = generalVariables.SaveDone;
                        else
                            TempData["msg"] = generalVariables.SaveNotDone;
                    }
                    if (screen == "_vpContractorData")
                        return RedirectToAction("vContractorShow", new { ProcessId = ProcessCode, inPage = insPageNumber });
                    else
                        return RedirectToAction("vContractorIndex");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }


        }
        //Delete
        /// <summary>
        /// Delete hidden contractor from process
        /// </summary>
        /// <param name="IDDelete">Contractor Code Want To Delete From Process</param>
        /// <param name="LhidContr">list of codes of hidden contractors</param>
        /// <param name="LOperContr">list of codes of missiong hidden contractors </param>
        /// <param name="txtNatContr">national Id for hidden contractor</param>
        /// <returns>hidden contractor page</returns>
        public ActionResult _vpDeleteSearchContractor(int? IDDelete, string[] LhidContr, string[] LOperContr, string txtNatContr)
        {
            try
            {
                if (IDDelete != null)
                {
                    int processCode = db.processSubContractors.FirstOrDefault(x => x.processSubContractorCode == IDDelete).processCode;
                    OprocessRequst = conApi.connectionApiDelete<ProcessRequest>("apiProcess", "DeleteSubContractor", IDDelete.ToString() + ',' + processCode.ToString());
                    if (OprocessRequst.bIsDeleted)
                        TempData["msg"] = generalVariables.DeleteDone;
                    else
                        TempData["msg"] = generalVariables.DeleteNotDone;
                    return RedirectToAction("vContractorShow", new { ProcessId = processCode, inPage = insPageNumber });
                }
                else //search
                {
                    List<string> ContractorSearch = new List<string>();
                    ContractorSearch.Add(ProcessCode.ToString());
                    ContractorSearch.Add(LhidContr == null ? (String.IsNullOrEmpty(contractors_) ? null : contractors_) : generalMethods.sConcatString(LhidContr, ','));
                    ContractorSearch.Add(LOperContr == null ? null : generalMethods.sConcatString(LOperContr, ','));
                    ContractorSearch.Add(String.IsNullOrEmpty(txtNatContr) ? null : txtNatContr);
                    LSearchProcessRequst = conApi.connectionApiSearchList<ProcessRequest>("apiProcess", "PostSearchSubContractor", ContractorSearch);
                    Model.LoProcessSubContractorMode = LSearchProcessRequst.LoProcessSubContractorMode;
                    if (!String.IsNullOrEmpty(screenSearch)) //هنا اتملت من السرش الى في المقاولين في تفاصيل العمليه في موظف التأمينات
                    {
                        if (screenSearch == "vContractorShow")
                        {
                            return RedirectToAction("vContractorShow", new { ProcessId = ProcessCode, inPage = insPageNumber });
                        }
                        else
                            return RedirectToAction("vpContractorsShow", "InsuranceEmployee", new { ProcessId = ProcessCode, Cont = contractors_, inPage = insPageNumber, cd4 = cd4_ });
                    }
                   else
                        return RedirectToAction("vpContractorsShow", "InsuranceEmployee", new { ProcessId = ProcessCode, Cont = contractors_, inPage = insPageNumber, cd4 = cd4_ });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        //Search
        /// <summary>
        /// search for hidden contractors
        /// </summary>
        /// <param name="screen">current page is open</param>
        /// <returns>hidden contractors</returns>
        public ActionResult _vpSearchContractor(string screen)
        {
            screenSearch = screen;
            ViewBag.OperContractor = new SelectList(db.GetMissionSubContractor(ProcessCode).ToList(), "processTypeCode", "processTypeName");
            ViewBag.hidContactors_ = new SelectList(db.spGetContractorsInProcess(ProcessCode, "0").ToList(), "processSubContractorCode", "referenceSideContractorName");
            return PartialView();
        }
        /// <summary>
        /// page to show  hidden contractor
        /// </summary>
        /// <param name="inPage">page number</param>
        /// <param name="ProcessId">process code</param>
        /// <returns></returns>
        public ActionResult vContractorShow(int? inPage, int? ProcessId)
        {
            insPageNumber = inPage;
            ProcessCode = ProcessId;
            return View();
        }
        /// <summary>
        /// function to cancel search
        /// </summary>
        /// <param name="ProcessId">process code</param>
        /// <param name="Act">action name</param>
        /// <param name="Contr">controller name</param>
        /// <returns>return to current page is open</returns>
        public ActionResult vpCancelSearch(int? ProcessId, string Act, string Contr)
        {
            try
            {
                Model = null;
                Model = new ProcessRequest();
                Model.LoProcessSubContractorMode = null;
                TempData["msg"] = generalVariables.SearchCancel;
                if (Act == "vContractorShow")
                    return RedirectToAction(Act, Contr, new { inPage = 1, ProcessId = ProcessId });
                else
                    return RedirectToAction(Act, Contr, new { ProcessId = ProcessId, cd4 = cd4_, inPage = insPageNumber });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// show mission for hidden contractor
        /// </summary>
        /// <param name="ID">hidden contractor code in  process</param>
        /// <returns>list of mission</returns>
        public ActionResult _vpMissionContractor(int? ID)
        {
            try
            {
                if (ID != null)
                {
                    OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetMissionSubContractor", ID.ToString());
                }
                return PartialView(OprocessRequst.LmissionSubContractor);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// Notes for process show to contractors
        /// </summary>
        /// <param name="processID">process code</param>
        /// <returns>function that get notes for process</returns>
        public ActionResult _vpProcessNotes(int? processID)
        {
            if (processID != null)
                return RedirectToAction("_vpNotesInsurance", "Process", new { processID = processID });
            else
                return RedirectToAction("vContractorIndex");

        }
        /// <summary>
        /// Opposition / Exemption for process
        /// </summary>
        /// <param name="ProcessId">process code</param>
        /// <param name="notActive">check of Opposition / Exemption </param>
        /// <returns>going to function is insert Opposition / Exemption</returns>
        public ActionResult _vpOpposition(int ProcessId, string notActive)
        {
            return RedirectToAction("_vpOppositionIndex", "Opposition", new { ProcessId = ProcessId, screen = "", notActive = notActive });
        }
        /// <summary>
        /// workers registration in process
        /// </summary>
        /// <param name="processID">process code</param>
        /// <returns>page that return workers</returns>
        public ActionResult WrokersProcess(int processID)
        {
            return RedirectToAction("_vpWorkersIndex", "Workers", new { processID = processID, Paths = "vContractorIndex" });
        }
        /// <summary>
        /// Get hidden contractor data when search for it in Insert /  Edit
        /// </summary>
        /// <param name="pageIndex">page number</param>
        /// <param name="name">contractor name</param>
        /// <returns>contractors Data</returns>
        public JsonResult GetContractorData(int pageIndex, string name)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {

                int startIndex = (pageIndex - 1) * iPageSize;
                if (String.IsNullOrEmpty(name))
                    OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetSubContractors", ProcessCode.ToString());
                else
                    OprocessRequst = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetContractors", name.Trim());
                OprocessRequst.CurrentPage = pageIndex;

                List<int?> list = new List<int?>();
                list = db.processSubContractors.Where(x => x.processCode == ProcessCode && x.contractorType == false).Select(y => y.contractorCode).ToList();

                if (String.IsNullOrEmpty(name))
                {
                    OprocessRequst.TotalPages = Convert.ToInt32(Math.Ceiling((double)OprocessRequst.LoProcessSubContractorMode.ToList().Count / iPageSize));
                    OprocessRequst.LoProcessSubContractorMode = OprocessRequst.LoProcessSubContractorMode.Where(x => !list.Contains(x.iContractorCode)).ToList();
                    OprocessRequst.LoProcessSubContractorMode = OprocessRequst.LoProcessSubContractorMode.Skip(startIndex).Take(iPageSize).ToList();
                }
                else
                {
                    OprocessRequst.TotalPages = Convert.ToInt32(Math.Ceiling((double)OprocessRequst.lrefConModel.ToList().Count / iPageSize));
                    OprocessRequst.lrefConModel = OprocessRequst.lrefConModel.Where(x => !list.Contains(x.iReferenceSideContractorCode)).ToList();
                    OprocessRequst.lrefConModel = OprocessRequst.lrefConModel.Skip(startIndex).Take(iPageSize).ToList();
                }
                return Json(OprocessRequst, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Find Reference Side And Contractors 
        /// </summary>
        /// <param name="ID">Insurance Number</param>
        /// <returns>Data of  Reference Side And Contractors</returns>
        public JsonResult GetContractorInsur(string ID) 
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                if (ID.Length < 9)
                    ID = Convert.ToInt32(ID).ToString("000000000");             
                var data = conApi.connectionApiGetList<ProcessRequest>("apiProcess", "GetRefSideContByInsurNum", String.IsNullOrEmpty(ID) ? "null" : ID);                
                return Json(data.oRefSideCont.dataDrob, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var data = "Error";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}