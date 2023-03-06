using DataAccessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using vt_nationalAuthority.Models;

namespace vt_attendance.Controllers.reports
{
    public class ReportsController : Controller
    {
        private readonly vt_authorityInsuranceEntities db = new vt_authorityInsuranceEntities();

        /// <summary>
        ///   Constructor Function : Go To Login Page When User Session End.
        /// </summary>
        public ReportsController()
        {
            CheckSession.bCheckUserCode("~/Login/vLoginIndex");
        }


        // GET: report
        /// <summary>
        ///   Report Properties.
        /// </summary>
        /// <param name="reportName"> Report Name. </param>
        public void report_public(string reportName)
        {
            var v = Request.QueryString;
            var v_ = Request.QueryString.Count;

            ReportViewer rptViewer = new ReportViewer();
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            //rptViewer.SizeToReportContent = true;
            rptViewer.ZoomMode = ZoomMode.PageWidth;
            rptViewer.Width = Unit.Pixel(1000);
            rptViewer.Height = Unit.Pixel(1000);
            rptViewer.ShowParameterPrompts = false;
            rptViewer.ShowBackButton = true;
            //rptViewer.AsyncRendering = true;

            ReportParameter[] param = new ReportParameter[Request.QueryString.Count - 1];
            for (int lop = 1; lop < Request.QueryString.Count; lop++)
            {
                string value = Request.QueryString[lop].ToString();
                string name = Request.QueryString.GetKey(lop);

                param[lop - 1] = new ReportParameter(name, value);
            }

            rptViewer.ServerReport.ReportServerUrl = new Uri("http://tfs/reportserver");
            rptViewer.ServerReport.ReportPath = "/vt_nationalAuthorityReports/" + reportName;
            rptViewer.ServerReport.SetParameters(param);
            rptViewer.ServerReport.Refresh();
            rptViewer.ServerReport.Timeout = 99999;
            ViewBag.ReportViewer = rptViewer;
        }

        /// <summary>
        ///   Report View. 
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult vReportsIndex()
        {
            return View();
        }


        /// <summary>
        ///   Call Report.
        /// </summary>
        /// <param name="reportName"> Report Name. </param>
        /// <returns> View. </returns>
        public ActionResult callReport(string reportName)
        {
            report_public(reportName);
            return View();
        }

        
        /// <summary>
        ///   بيان بطلبات العمليات
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repRequestsProesses()
        {
            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.repRef = new SelectList(db.GetContractorRefrence("1").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            // حاله العمليه
            ViewBag.processStatus = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="الكل", Value = "-1" },
                new SelectListItem{ Text="تم الموافقه", Value = "1" },
                new SelectListItem{ Text="لم يتم الموافقه بعد", Value = "0"}}, "Value", "Text", 1);

            return View();
        }


        /// <summary>
        ///   بيان بالعمليات الساريه
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repActiveProesses()
        {
            ViewBagsOffices();

            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.repRef = new SelectList(db.GetContractorRefrence("1").ToList(), "referenceSideContractorCode", "referenceSideContractorName");

            return View();
        }


        /// <summary>
        ///   بيان بالعمليات الشهريه
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repRegMonthProesses()
        {
            ViewBagsOffices();

            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.repRef = new SelectList(db.GetContractorRefrence("1").ToList(), "referenceSideContractorCode", "referenceSideContractorName");

            return View();
        }


        /// <summary>
        ///   بيان بالاشتراكات العمليات
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repExtracts()
        {
            ViewBagsOffices();

            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.repRef = new SelectList(db.GetContractorRefrence("1").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            // نوع المستخلص
            ViewBag.extractTypes = new SelectList(db.extractTypes.ToList(), "extractTypeCode", "extractTypeName");
            // تسديد المستخلص
            ViewBag.extractPaid = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="الكل", Value = "-1" },
                new SelectListItem{ Text="مسدد", Value = "1" },
                new SelectListItem{ Text="غير مسدد", Value = "0"}}, "Value", "Text", 1);
            return View();
        }


        /// <summary>
        ///   تقرير بيان بطباعه طلبات الكروت  
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult vpReportCardRequests()
        {
            ViewBagsOffices();

            // حالات الكروت
            ViewBag.Status = new SelectList(db.cardsStatus.ToList(), "cardsStatusCode", "cardsStatusName");
            return PartialView();
        }


        /// <summary>
        ///   بيان بحركات المستخدمين
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repTransactios()
        {
            ViewBagsOffices();

            int uc = Convert.ToInt32(Session["uc"].ToString());
            // المستخدمين
            ViewBag.Users = new SelectList(db.spGetUsersOffice(uc, 1).ToList(), "userCode", "userFullName");

            // الشاشه
            ViewBag.screen = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="الكل", Value = "-1" },
                new SelectListItem{ Text="العمليات", Value = "1" },
                new SelectListItem{ Text="طلبات العمليات", Value = "2" },
                new SelectListItem{ Text="ايقاف العمليه", Value = "3" },
                new SelectListItem{ Text="مستخدمين العمليه", Value = "4" },
                new SelectListItem{ Text="المستخلصات", Value = "5" },
                new SelectListItem{ Text="العمال", Value = "6" },
                new SelectListItem{ Text="الاصابات", Value = "7" },
                new SelectListItem{ Text="طلبات الكروت", Value = "8" },
                new SelectListItem{ Text="ايقاف / تفعيل الكارت", Value = "9"}}, "Value", "Text", 1);

            // الحركات
            ViewBag.Actions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="الكل", Value = "-1" },
                new SelectListItem{ Text="تسجيل -اضافه - حفظ", Value = "1" },
                new SelectListItem{ Text="تعديل", Value = "2" },
                new SelectListItem{ Text="قبول الطلب", Value = "3" },
                new SelectListItem{ Text="تسجيل الهواتف للعمال", Value = "4" },
                new SelectListItem{ Text="استبعاد العامل", Value = "5" },
                new SelectListItem{ Text="تعديل حضور العامل", Value = "6" },
                new SelectListItem{ Text="ارسال الى التأمين الصحى", Value = "7" },
                new SelectListItem{ Text="ارسال الى القوى العامله", Value = "8" },
                new SelectListItem{ Text="ايقاف الكارت", Value = "9" },
                new SelectListItem{ Text="تفعيل الكارت", Value = "10" },
                new SelectListItem{ Text="ملاحظات التأمينات", Value = "11"}}, "Value", "Text", 1);

            return View();
        }


        /// <summary>
        ///   Get Data Of Drop Down Lists.  
        /// </summary>
        public void ViewBagsOffices()
        {
            List<int> OfficesPerm = new List<int>();
            if (Session["areaOfficePermission"] == null)
            {
                ViewBag.areas = new SelectList(db.spAllAreas(null, null, null, null).ToList(), "areaCode", "areaIDName");
                ViewBag.Offices = new SelectList(db.GetOfficeInsurance("-1", null).ToList(), "officeInsuranceCode", "officeInsuranceIDName");
            }
            else
            {
                OfficesPerm = Session["areaOfficePermission"].ToString().Split(',').Select(int.Parse).ToList();
                ViewBag.areas = new SelectList(db.spAllAreas(null, null, Session["AreaSearch"] == null ? null : Session["AreaSearch"].ToString(), null).ToList(), "areaCode", "areaIDName");
                ViewBag.Offices = new SelectList(db.GetOfficeInsurance("-1", null).Where(x => OfficesPerm.Contains(x.officeInsuranceCode)).ToList(), "officeInsuranceCode", "officeInsuranceIDName");
            }
        }

        #region Workers العمال

        /// <summary>
        ///   بيان ببحضور العمال
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerAttendance()
        {
            return View();
        }


        /// <summary>
        ///   بيان بالمدد الشهريه للعامل
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerAttendanceMonthly()
        {
            return View();
        }


        /// <summary>
        ///   بيان بالعمال الغير مؤمن عليهم
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerWithoutInsuNum()
        {
            return View();
        }


        /// <summary>
        ///   بيان بالعمال غير المؤقتين 
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerOpenGov()
        {
            ViewBag.workerGovOpen = new SelectList(
                                    new List<SelectListItem>
                                    {
                                        new SelectListItem { Text = "مفتوح المده", Value = "1"},
                                        new SelectListItem { Text = "قطاع حكومى", Value = "2"},
                                    }, "Value", "Text"); ;

            return View();
        }


        /// <summary>
        ///   بيان بالعمال التى تم تحضيرهم مع المقاولين
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerContractor()
        {
            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.statusAttend = new SelectList(db.statusFirstAttendMonthWSDLs.ToList(), "statusFirstAttendMonthWSDLCode", "statusFirstAttendMonthWSDLDescription");
            return View();
        }


        /// <summary>
        ///   بيان بالعمال التى تم تغطيتهم
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repWorkerContractorCover()
        {
            // جهه اسناد - مقاول
            ViewBag.repCont = new SelectList(db.GetContractorRefrence("0").ToList(), "referenceSideContractorCode", "referenceSideContractorName");
            ViewBag.statusCover = new SelectList(db.statusCoverWSDLs.ToList(), "statusCoverWSDLCode", "statusCoverWSDLDescription");
            return View();
        }



        public ActionResult repTotalWorkersProcesses()
        {
            return View();
        }
        #endregion

        #region Users المستخدمين

        /// <summary>
        ///   استعلام عن تسجيل المستخدمين
        /// </summary>
        /// <returns> View. </returns>
        public ActionResult repGetUsers()
        {
            int uc = Convert.ToInt32(Session["uc"].ToString());
            // المستخدمين
            ViewBag.Users = new SelectList(db.spGetUsersOffice(uc, 1).ToList(), "userCode", "userFullName");

            // نوع المستخدم
            ViewBag.userType = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text="مكتب", Value = "0" },
                new SelectListItem{ Text="مقاول", Value = "1" },
                new SelectListItem{ Text="جهه اسناد", Value = "2"}}, "Value", "Text", 1);

            return View();
        }

        #endregion

    }
}
