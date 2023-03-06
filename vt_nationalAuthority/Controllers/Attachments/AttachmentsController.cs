using DataAccessLayer;
using DataAccessLayer.Requests;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vt_nationalAuthority.Models;

namespace vt_nationalAuthority.Controllers.Attachments
{
    /// <summary>
    ///   Controller Of Attachments.
    /// </summary>
    public class AttachmentsController : Controller
    {
        // GET: Attachments المرفقات
        ConnectionApi conApi = new ConnectionApi();
        DocumentTypeRequest oDocumentTypeRequest = new DocumentTypeRequest();
        FilesPDF file = new FilesPDF();
        static bool bIsSearch = false;
        static bool bIsSaveFile = false;
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();


        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public AttachmentsController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        /// <summary>
        ///   Partial View Of All Attachments Of Special Process.
        /// </summary>
        /// <param name="cd4"> Code Of Module Insurance Officer. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentIndex(int? cd4, bool? cp)
        {
            string summary = "*";
            if (cd4 != null || cp == true)
            {
                int uc = Convert.ToInt32(Session["uc"].ToString());
                TempData["checkLevel4"] = db.CheckModuleUserPermisiom(uc, cd4).ToList();
                bIsSearch = false;
            }

            if (!bIsSearch)
            {
                string[] nestedFolders = { "Processes", Session["processCode"].ToString(), "pAttachments" };
                spGetProcessDataForAttachment_Result processData = new spGetProcessDataForAttachment_Result();

                // جهه اسناد - مقاول 
                if (Session["contractor"] != null)
                {
                    processData = Session["contractor"].ToString().Contains("جهه الاسناد : ") ? db.spGetProcessDataForAttachment(-1, Convert.ToInt32(Session["uc"].ToString())).FirstOrDefault() : // Reference Side جهه اسناد
                                                                                                db.spGetProcessDataForAttachment(Convert.ToInt32(Session["processCode"].ToString()), Convert.ToInt32(Session["uc"].ToString())).FirstOrDefault(); // Contractor المقاول

                    summary = processData.contractorCode.ToString();
                }

                TempData["dataTable"] = file.fillAttachFolders("Attachments", nestedFolders, summary);
            }

            return PartialView();
        }

        /// <summary>
        ///   All Attachments Of Special Process.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult vAttachmentsShow()
        {
            return View();
        }

        /// <summary>
        ///   All Attachments Of Special Process.
        /// </summary>
        /// <param name="inPage"> Number Of Page. </param>
        /// <param name="cp"> Check If This Is The First Time This Page Is clicked Or Not. </param>
        /// <returns> View. </returns>
        public ActionResult vAttachmentsIndex(int? inPage, bool? cp)
        {
            var lDocumentTyes = conApi.connectionApiGetList<DocumentTypeRequest>("apiDocumentType", "GetDocumentTypes", null);

            if (lDocumentTyes == null)
            {
                lDocumentTyes = new DocumentTypeRequest();
                lDocumentTyes.LModels = new List<DataAccessLayer.Models.DocumentTypeModel>();
            }

            if (lDocumentTyes.LModels == null)
                lDocumentTyes.LModels = new List<DataAccessLayer.Models.DocumentTypeModel>();

            return View(lDocumentTyes.LModels.ToPagedList(inPage ?? 1, Convert.ToInt32(generalVariables.PageSize)));
        }


        /// <summary>
        ///   Show Partial View To Add Attachment.
        /// </summary>
        /// <param name="nFolder"> Nested Folders. </param>
        /// <param name="actionName"> Action Name. </param>
        /// <param name="controllerName"> Controller Name. </param>
        /// <param name="screenType"> Screen Typr. </param>
        /// <returns> Partial view. </returns>
        public ActionResult _vpAttachmentAdd(string nFolder, string actionName, string controllerName, string screenType)
        {
            // عشان اول مارفع فايل واحد من ضمن عدد من الفايلات هغيره بحيث ماحفظش ف الداتابيز
            bIsSaveFile = false;
            string[] nFold = nFolder.Contains("/") ? nFolder.Split('/') : nFolder.Split(',');

            viewBags(null, Convert.ToInt32(nFold[1]));
            TempData["nFolder"] = nFolder;
            TempData["actionName"] = actionName;
            TempData["controllerName"] = controllerName;
            TempData["screenType"] = screenType; // 1. عمليه  2. طلبات العمليات
            TempData["screenCode"] = nFolder.Contains("pExtracts") || nFolder.Contains("pStops") || nFolder.Contains("Injuries") ? nFold[3] : nFold[1]; // كود العمليه - عامل - كارت - المستخلص

            return PartialView();
        }

        /// <summary>
        ///   Save Attachments In Folders.
        /// </summary>
        /// <param name="nFolder"> Nested Folders. </param>
        /// <param name="packageAttachmentsName"> Package Attachment Name. </param>
        /// <param name="attachmentType"> Attachment Type. </param>
        /// <param name="actionName"> Action Name. </param>
        /// <param name="controllerName"> Controller Name. </param>
        /// <param name="contRefSide"> Reference Side - Contractor. </param>
        /// <param name="screenType">Screen Type. </param>
        /// <param name="screenCode"> Screen Code. </param>
        /// <returns> View. </returns>
        public ActionResult apply_upload(string nFolder, string packageAttachmentsName, int? attachmentType, string actionName, string controllerName, string contRefSide, string screenType, string screenCode)
        {
            string[] nestedFolders;
            string[] nFold = nFolder.Contains("/") ? nFolder.Split('/') : nFolder.Split(',');

            #region المستخلصات
            if (nFolder.Contains("pExtracts"))
            {
                var contractorCode = db.spGetContractorCodeByExtracts(Convert.ToInt32(nFold[3])).FirstOrDefault().Value;
                nestedFolders = (nFolder + "," + contractorCode + "$$" + Session["uc"].ToString() + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',');
            }
            #endregion
            #region ايقاف العمليه
            else if (nFolder.Contains("pStops"))
                nestedFolders = (nFolder + "," + contRefSide + "$$" + Session["uc"].ToString() + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',');
            #endregion
            #region الاصابات
            else if (nFolder.Contains("Injuries"))
                nestedFolders = (nFolder + "," + screenCode + "$$" + Session["uc"].ToString() + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',');
            #endregion
            #region مرفقات العمليه
            else if (nFolder.Contains("pAttachments"))
            {
                spGetProcessDataForAttachment_Result processData = new spGetProcessDataForAttachment_Result();

                // جهه اسناد - مقاول 
                if (Session["contractor"] != null)
                {
                    processData = Session["contractor"].ToString().Contains("جهه الاسناد : ") ? db.spGetProcessDataForAttachment(Convert.ToInt32(nFold[1]), -1).FirstOrDefault() : // جهه اسناد
                                                                                                db.spGetProcessDataForAttachment(Convert.ToInt32(nFold[1]), Convert.ToInt32(Session["uc"].ToString())).FirstOrDefault(); // مقاول
                    nestedFolders = (nFolder + "," + processData.contractorCode + "$$" + processData.referenceSideCode + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',');
                }
                else // موظف تأمينات
                {
                    processData = db.spGetProcessDataForAttachment(Convert.ToInt32(nFold[1]), -2).FirstOrDefault();
                    nestedFolders = Convert.ToInt32(contRefSide) < 0 ? (nFolder + "," + processData.contractorCode + "$$-1$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',') : // جهه اسناد
                                                                       (nFolder + "," + contRefSide + "$$" + (processData.referenceSideCode * -1) + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(','); // مقاول
                }
            }
            #endregion
            #region باقى الشاشات
            else
                nestedFolders = (nFolder + "," + Session["uc"].ToString() + "$$" + Session["uc"].ToString() + "$$" + Session["uc"].ToString() + "$$" + attachmentType + "$$" + packageAttachmentsName).Split(',');
            #endregion

            string[] message = { };
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase fileUpload = Request.Files[i];
                message = file.addFileWithSpecialExtention("Attachments", nestedFolders, Request.Files[i], file.imagesPdf, Session["uc"].ToString(), Request.Files[i].FileName).Split('/');
                // هحفظ فى الداتابيز
                if (message[0] == "تم الحفظ" && !bIsSaveFile)
                {
                    #region DataBase

                    attachment model = new attachment();
                    model.screenTypeCode = Convert.ToInt32(screenType); // نوع الشاشه : عمليات طلبات عمال
                    model.screenCode = Convert.ToInt32(screenCode); // كود العمليه
                    model.seen = false;
                    model.userInsertCode = Convert.ToInt32(Session["uc"].ToString()); // كود المدخل
                    model.dateInsert = DateTime.Now; // تاريخ الادخال

                    #region Officer الموظف
                    if ((Session["contractor"] == null && !actionName.Contains("Worker") && !actionName.Contains("vCardsRequestsIndex"))) // موظف ومش فى شاشه العمال ولا طلب الكروت
                    {
                        if (Convert.ToInt32(contRefSide) < 0) // جهه الاسناد
                            model.refSideCode = Convert.ToInt32(Convert.ToInt32(contRefSide) * -1);
                        else if (nFolder.Contains("pExtracts")) // المستخلصات
                            model.contractorCode = db.spGetContractorCodeByExtracts(Convert.ToInt32(nFold[3])).FirstOrDefault().Value;
                        else if (nFolder.Contains("pStops") || nFolder.Contains("Injuries")) // ايقاف العمليه - الاصابات
                            model.contractorCode = null;
                        else // مقاول
                            model.contractorCode = Convert.ToInt32(contRefSide);

                        db.attachments.Add(model);
                        if (db.SaveChanges() > 0)
                            bIsSaveFile = !bIsSaveFile;
                    }
                    #endregion
                    #region Contractor - Reference Side المقاول - جهه الاسناد
                    else if (Session["contractor"] != null)
                    {
                        if (actionName.Contains("vWorkersShow")) // العمال عند شاشه المقاولين
                        {
                            int processCode = Convert.ToInt32(Session["processCode"]);
                            model.officeCode = db.processes.FirstOrDefault(x => x.processCode == processCode).officeInsuranceCode;
                        }
                        else if (nFolder.Contains("pExtracts")) // المستخلصات
                        {
                            int processCode = Convert.ToInt32(Session["processCode"].ToString());
                            model.officeCode = db.processes.FirstOrDefault(x => x.processCode == processCode).officeInsuranceCode;
                        }
                        else
                            model.officeCode = db.processes.FirstOrDefault(x => x.processCode == model.screenCode).officeInsuranceCode;

                        db.attachments.Add(model);
                        if (db.SaveChanges() > 0)
                            bIsSaveFile = !bIsSaveFile;
                    }
                    #endregion

                    #endregion
                }
            }
            if (Session["contractor"] != null && actionName.Contains("vWorkersShow")) // العمال عند شاشه المقاولين
                return RedirectToAction(actionName, controllerName, new { ProcessId = Session["processCode"].ToString(), inPage = 1 });

            return RedirectToAction(actionName, controllerName);
        }


        /// <summary>
        ///   Show Partial View Of Search.
        /// </summary>
        /// <param name="uc"> User code. </param>
        /// <param name="attachTypeCode"> Attachment Type Code. </param>
        /// <param name="docName"> Document Name. </param>
        /// <param name="docuFullName"> Document Full Name. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentSearchOrEdit(int? uc, int? attachTypeCode, string docName, string docuFullName)
        {
            TempData["uc"] = uc;
            TempData["attachTypeCode"] = attachTypeCode;
            TempData["docName"] = docName;
            TempData["contRefSide"] = docName;
            viewBags(attachTypeCode, null);
            return PartialView();
        }

        /// <summary>
        ///   Search With Special Parameters. 
        /// </summary>
        /// <param name="attachmentName"> Attachment Name. </param>
        /// <param name="attachmentType"> Attachment Type. </param>
        /// <param name="contRefSide"> Reference Side - Contractor. </param>
        /// <returns> View. </returns>
        [HttpPost]
        public ActionResult _vpAttachmentSearchOrEdit(string attachmentName, string attachmentType, string contRefSide)
        {
            string summary = "*";
            string[] nestedFolders = { "Processes", Session["processCode"].ToString(), "pAttachments" };

            // جهه اسناد - مقاول
            if (Session["contractor"] != null)
                summary = Session["contractor"].ToString().Contains("جهه الاسناد : ") ? "*$$-1" :  // هجيب جهه الاسناد وليس المستخدم
                                                                                         db.spGetUserDetails(Session["uc"].ToString()).FirstOrDefault().contractorCode.ToString(); // هجيب المقاول الاصلى وليس المستخدم

            // Search
            if (String.IsNullOrEmpty(Request.Form["docName"]))
            {
                bIsSearch = true;
                TempData["dataTable"] = file.searchFolders("Attachments", nestedFolders, attachmentName, attachmentType, summary);
            }
            else if (!String.IsNullOrEmpty(Request.Form["uc"])) // Edit
            {
                string newFolderName = "";
                spGetProcessDataForAttachment_Result processData = new spGetProcessDataForAttachment_Result();
                // جهه اسناد - مقاول 
                if (Session["contractor"] != null)
                {
                    // جهه اسناد
                    if (Session["contractor"].ToString().Contains("جهه الاسناد : "))
                        processData = Session["contractor"].ToString().Contains("جهه الاسناد : ") ? db.spGetProcessDataForAttachment(Convert.ToInt32(@Session["processCode"].ToString()), -1).FirstOrDefault() : // Reference Side جهه اسناد
                                                                                                    db.spGetProcessDataForAttachment(Convert.ToInt32(@Session["processCode"].ToString()), Convert.ToInt32(Session["uc"].ToString())).FirstOrDefault(); // Contractor المقاول

                    newFolderName = processData.contractorCode + "$$" + processData.referenceSideCode + "$$" + Request.Form["uc"] + "$$" + attachmentType.ToString() + "$$" + Request.Form["attachmentName"];
                }
                else // موظف تأمينات
                {
                    processData = db.spGetProcessDataForAttachment(Convert.ToInt32(@Session["processCode"].ToString()), -2).FirstOrDefault();
                    newFolderName = contRefSide + "$$" + processData.referenceSideCode + "$$" + Request.Form["uc"] + "$$" + attachmentType.ToString() + "$$" + Request.Form["attachmentName"];
                }

                string oldFolderName = processData.contractorCode + "$$" + processData.referenceSideCode + "$$" + Request.Form["uc"] + "$$" + Request.Form["attachTypeCode"] + "$$" + Request.Form["docName"];

                if (oldFolderName == newFolderName)
                    TempData["msg"] = generalVariables.changeFilderName;
                else if (file.editFolders("Attachments", nestedFolders, oldFolderName, newFolderName))
                    TempData["msg"] = generalVariables.EditDone;
                else
                    TempData["msg"] = generalVariables.EditNotDone;

                TempData["dataTable"] = file.fillAttachFolders("Attachments", nestedFolders, summary);
                bIsSearch = false;
            }

            return RedirectToAction("vAttachmentsIndex");
        }


        /// <summary>
        ///   Show Partial View Of Files Search.
        /// </summary>
        /// <param name="nFolder"> Nested Folders. </param>
        /// <param name="actionName"> Action Name. </param>
        /// <param name="controllerName"> Controller Name. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="attachTypeCode"> Attachment Type Code. </param>
        /// <param name="docName"> Document Name. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentSearchFiles(string nFolder, string actionName, string controllerName, int? uc, int? attachTypeCode, string docName)
        {
            TempData["nFolder"] = nFolder;

            string[] nestedFolders = nFolder.Split(',');
            TempData["dt"] = file.fillAttach("Attachments", nestedFolders, "*");

            return PartialView();
        }


        /// <summary>
        ///   Show Partial View Of Folders Search.
        /// </summary>
        /// <param name="nFolder"> Nested Folders. </param>
        /// <param name="actionName"> Action Name. </param>
        /// <param name="controllerName"> Controller Name. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="attachTypeCode"> Attachment Type Code. </param>
        /// <param name="docName"> Document Name. </param>
        /// <param name="notActive"> Not Active. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentSearch(string nFolder, string actionName, string controllerName, int? uc, int? attachTypeCode, string docName, string notActive)
        {
            string[] nestedFolders = nFolder.Contains("/") ? nFolder.Split('/') : nFolder.Split(',');
            String summary = "*";
            // المستخلصات
            if (nFolder.Contains("pExtracts"))
            {
                if (Session["contractor"] != null) // Contractor مقاول
                    summary = db.spGetUserDetails(Session["uc"].ToString()).FirstOrDefault().contractorCode.ToString(); // هجيب المقاول الاصلى وليس المستخدم
            }
            // مرفقات العمليه
            else if (nFolder.Contains("Processes"))
            {
                // مقاول - جهه اسناد
                if (Session["contractor"] != null)
                    summary = Session["contractor"].ToString().Contains("جهه الاسناد : ") ? "*$$-1" : // هجيب جهه الاسناد وليس المستخدم
                                                                                             db.spGetUserDetails(Session["uc"].ToString()).FirstOrDefault().contractorCode.ToString(); // هجيب المقاول الاصلى وليس المستخدم
            }

            if (notActive != "green" && !String.IsNullOrEmpty(notActive))
                Session["procStopActive"] = 1;
            else
                Session["procStopActive"] = null;

            TempData["nFolder"] = nFolder;
            TempData["controllerName"] = controllerName;
            TempData["dt"] = file.foldersWithFiles("Attachments", nestedFolders, summary);

            return PartialView();
        }

        /// <summary>
        ///   Show Partial View Of Search. 
        /// </summary>
        /// <param name="nFolder"> Nested Folders. </param>
        /// <param name="actionName"> Action Name. </param>
        /// <param name="controllerName"> Controller Name. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="attachTypeCode"> Attachment Type Code. </param>
        /// <param name="docName"> Document Name. </param>
        /// <param name="notActive"> Not Active. </param>
        /// <returns> Partial View. </returns>
        public ActionResult _vpAttachmentScreens(string nFolder, string actionName, string controllerName, int? uc, int? attachTypeCode, string docName, string notActive)
        {
            TempData["nFolder"] = nFolder;
            string[] nestedFolders = nFolder.Contains("/") ? nFolder.Split('/') : nFolder.Split(',');
            TempData["dt"] = file.filesFolder("Attachments", nestedFolders, "*");

            return PartialView();
        }


        /// <summary>
        ///   Delete File.
        /// </summary>
        /// <param name="filePath"> File Path. </param>
        /// <param name="uc"> User Code. </param>
        /// <param name="attachTypeCode"> Attachment Type Code. </param>
        /// <param name="docName"> Document Name. </param>
        /// <returns> Json. </returns>
        public JsonResult vDeleteFile(string filePath, int? uc, int? attachTypeCode, string docName)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string[] nestedFolders = filePath.Split(',');

            if (file.deleteFile("Attachments", nestedFolders, docName))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///   Delete Folder.
        /// </summary>
        /// <param name="filePath"> Folder Path. </param>
        /// <param name="docName"> Document Name. </param>
        /// <returns> Json. </returns>
        public JsonResult vDeleteFolder(string filePath, string docName)
        {
            db.Configuration.ProxyCreationEnabled = false;

            string[] nestedFolders = filePath.Split(',');

            if (file.deleteFolders("Attachments", nestedFolders, docName))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///   Cancel Search.
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult _vpSearchCancel()
        {
            bIsSearch = false;
            TempData["msg"] = generalVariables.SearchCancel;
            return RedirectToAction("vAttachmentsIndex");
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        void viewBags(int? attachTypeCode, int? processCode)
        {
            // Attachment Type نوع المرفق
            ViewBag.attachmentType = !String.IsNullOrEmpty(attachTypeCode.ToString()) ? new SelectList(db.attachmentTypes.AsEnumerable(), "attachmentTypeCode", "attachmentTypeName", attachTypeCode) : // Edit
                                                                                         new SelectList(db.attachmentTypes.AsEnumerable(), "attachmentTypeCode", "attachmentTypeName"); // Add
            // Contractors - Reference Side جهه الاسناد ومقاولين العمليه
            ViewBag.contRefSide = new SelectList(db.spGetContractorsInProcess(processCode, "-1").AsEnumerable(), "referenceSideContractorCode", "referenceSideContractorName");
        }

    }
}