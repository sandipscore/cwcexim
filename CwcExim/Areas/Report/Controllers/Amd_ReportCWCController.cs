using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Filters;
using CwcExim.UtilityClasses;
using System.Reflection;
using System.Text;
using CwcExim.Areas.Import.Models;
using System.Data;
using CwcExim.Areas.Export.Models;
using System.Drawing;
using EinvoiceLibrary;
using System.Threading.Tasks;
using CwcExim.Areas.Import.Controllers;
using CwcExim.Areas.CashManagement.Controllers;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Export.Controllers;

namespace CwcExim.Areas.Report.Controllers
{

    public class Amd_ReportCWCController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }
        public Amd_ReportCWCController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            EffectVersion = Convert.ToDecimal(objCompanyDetails.EffectVersion);
            EffectVersionLogoFile = objCompanyDetails.VersionLogoFile;


        }

        #region DailyCashBookReport
        [HttpGet]
        public ActionResult DailyCashBookReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDailyCashBookReport(string PeriodFrom, string PeriodTo)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_DailyCashBook> LstDailyCashBook = new List<VRN_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBook(PeriodFrom, PeriodTo);
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<VRN_DailyCashBook>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                LstDailyCashBook.Add(new VRN_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = LstDailyCashBook.ToList().Sum(o => o.Area),
                    STO = LstDailyCashBook.ToList().Sum(o => o.STO),
                    INS = LstDailyCashBook.ToList().Sum(o => o.INS),
                    GRE = LstDailyCashBook.ToList().Sum(o => o.GRE),
                    GRL = LstDailyCashBook.ToList().Sum(o => o.GRL),
                    //Reefer = LstDailyCashBook.ToList().Sum(o => o.Reefer),
                    //EscCharge = LstDailyCashBook.ToList().Sum(o => o.EscCharge),
                    //Print = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    //Royality = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    //HT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.HT)),

                    CstmExam = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CstmExam)),
                    Weighment = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Weighment)),
                    CstmCl = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CstmCl)),
                    CBSC = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CBSC)),
                    EPCCh = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EPCCh)),
                    OT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.OT)),

                    EGM = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    Documentation = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Taxable)),
                    Cgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)),
                    Roundoff = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    Tds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CrTds))

                });
                ObjRR.DBResponse.Data = LstDailyCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateDailyCashBookReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "DailyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                //using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                //{

                //    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                //}
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/DailyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }





        #endregion

        #region DailyCashBookReportWithSD
        [HttpGet]
        public ActionResult DailyCashBookReportWithSD()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDailyCashBookReportWithSD(DailyCashBookAmd ObjDailyCashBook)
        {
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            List<DailyCashBookAmd> LstDailyCashBook = new List<DailyCashBookAmd>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBookSD(ObjDailyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<DailyCashBookAmd>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();

                LstDailyCashBook.Add(new DailyCashBookAmd()
                {
                    /*ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",*/

                    InvoiceDate = "<strong>Total</strong>",
                    InvoiceNo = "",
                    InvoiceType = "",
                    PartyName = "",
                    PayeeName = "",
                    ModeOfPay = "",
                    ChqNo = "",
                    GenSpace = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GenSpace)).ToString(),
                    StorageCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.StorageCharge)).ToString(),
                    Insurance = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Insurance)).ToString(),
                    GroundRentEmpty = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentEmpty)).ToString(),
                    GroundRentLoaded = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentLoaded)).ToString(),
                    MfCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MfCharge)).ToString(),
                    EntryCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EntryCharge)).ToString(),
                    Fumigation = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Fumigation)).ToString(),
                    OtherCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.OtherCharge)).ToString(),
                    Misc = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Misc)).ToString(),
                    Cgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString(),
                    Sgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString(),
                    Igst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString(),

                    MiscExcess = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MiscExcess)).ToString(),
                    TotalCash = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)).ToString(),
                    TotalCheque = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString(),
                    TotalOthers = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString(),
                    Tds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Tds)).ToString(),
                    CrTds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CrTds)).ToString(),
                    TotalPDA = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalPDA)).ToString(),
                    Remarks = ""

                });
                ObjRR.DBResponse.Data = LstDailyCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateDailyCashBookReportWithSDPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "DailyCashBookReportWithSD.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                //using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                //{

                //    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                //}
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/DailyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }
        #endregion

        #region MonthlyCashBookReport



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult MonthlyCashBookReport()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //Kdl_ReportRepository ObjReport = new Kdl_ReportRepository();
            //ObjReport.GetDistricts(ObjLogin.Uid);
            //if (ObjReport.DBResponse.Data != null)
            //{
            //    var JObj = JObject.Parse(JsonConvert.SerializeObject(ObjReport.DBResponse.Data));
            //    ViewBag.DistList = new SelectList(JObj["Dist"], "DistrictId", "DistrictName");
            //}

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMonthlyCashBookReport(VRN_MonthlyCashBook ObjMonthlyCashBook)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_DailyCashBook> LstDailyCashBook = new List<VRN_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthlyCashBook(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {

                VRN_MonthlyCashBook VRNCashBook = new VRN_MonthlyCashBook();
                VRNCashBook = (VRN_MonthlyCashBook)ObjRR.DBResponse.Data;

                //  LstDailyCashBook = (List<VRN_DailyCashBook>)ObjRR.DBResponse.Data;
                VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                VRNCashBook.lstCashReceipt.Add(new VRN_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Area),
                    STO = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.STO),
                    INS = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.INS),
                    GRE = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRE),
                    GRL = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRL),
                    Reefer = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Reefer),
                    EscCharge = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.EscCharge),
                    Print = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    Royality = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    HT = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.HT)),
                    EGM = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    Documentation = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.Taxable))),
                    Cgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.Igst))),
                    Roundoff = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    TotalSD = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalSD)),
                    OpCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpCash)),
                    OpChq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpChq)),
                    cloCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.cloCash)),
                    clochq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.clochq)),

                    Tds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.CrTds))

                });
                ObjRR.DBResponse.Data = VRNCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateMonthlyCashBookPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "MonthlyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/MonthlyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 20f, 20f, 20f, true))
                {
                    ObjPdf.HeadOffice = "";
                    ObjPdf.HOAddress = "";
                    ObjPdf.ZonalOffice = "";
                    ObjPdf.ZOAddress = "";
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/MonthlyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }





        #endregion

        #region Party Ledger Report

        public ActionResult PartyLedgerStatement()
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet("", 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstParty = Jobject["LstParty"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }

        [HttpGet]
        public JsonResult OASearchPartyLedgerParty(string PartyCode)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult OALoadPartyLedgerParty(string PartyCode, int Page)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPartyLedgerReport(string FromDate, string ToDate, int PartyId)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();

            string Fdt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetPartyLedgerStatement(PartyId, Fdt, Tdt);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePDFForPartyLedgerDetails(FormCollection Fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "OnAcStatementPartyLedger.pdf";
                Pages[0] = Fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report/OnAcStatementPartyLedger/";
                Pages[0] = Fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg")).Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                //Pages[0] = Pages[0].ToString();

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 30f, 30f, 30f, 30f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }

                return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/OnAcStatementPartyLedger/" + FileName });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion

        #region SD Details Report

        public ActionResult SDDetailsReport()
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet("", 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstParty = Jobject["LstParty"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }


        [HttpGet]
        public JsonResult SearchParty(string PartyCode)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadParty(string PartyCode, int Page)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSDDetReport(string FromDate, string ToDate, int PartyId)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();

            string Fdt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetSDDetStatement(PartyId, Fdt, Tdt);
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                VRN_SDDetailsStatement SDData = new VRN_SDDetailsStatement();
                SDData = (VRN_SDDetailsStatement)ObjRR.DBResponse.Data;

                Path = GeneratePDFSDDetReport(SDData, FromDate, ToDate);
            }
            return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public string GeneratePDFSDDetReport(VRN_SDDetailsStatement SDData, string FromDate, string ToDate)
        {
            try
            {
                var FileName = "SdReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                //***************************************************************************************

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + SDData.CompanyAddress + "</span><br/><label style='font-size: 14px; font-weight:bold;'>SD STATEMENT</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append(" </td></tr>");
                Pages.Append("</thead>");

                Pages.Append(" <tbody>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Party :</b>" + SDData.PartyName + "</td></tr>");
                Pages.Append(" <tr><td colspan='12' style='font-size:12px;'><b>Folio No :</b> " + SDData.PartyCode + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>CWC GST No :</b> " + SDData.CompanyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Party GST No :</b> " + SDData.PartyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Period :</b> " + FromDate + " - " + ToDate + "</td></tr>");
                Pages.Append("</tbody></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>Sl. No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>Invoice No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Invoice Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>Receipt No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Receipt Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Pay Receipt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Transaction Type</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:12%;'>Transaction Amount</th></tr></thead>");
                Pages.Append("<tbody>");
                SDData.lstInvc.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>" + item.InvoiceNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.InvoiceDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>" + item.ReceiptNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.ReceiptDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:right;'>" + item.ReceiptAmt.ToString() + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.TranType + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.TranAmt.ToString() + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });
                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='border-right:1px solid #000;width:8%;text-align:right;'>Balance :</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + SDData.UtilizationAmount + "</th>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse;font-size:9pt;'><tbody>");
                Pages.Append("<tr><td><span><br/><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12'><p><b>N.B :</b> This is a computer generated statement. Doesn't require any signature.</p></td></tr>");
                Pages.Append("</tbody></table>");

                //***************************************************************************************
                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region SD A/c Statement

        [HttpGet]
        public ActionResult SDStatement()
        {
            var Months = new Dictionary<int, string>();
            for (Int16 i = 1; i <= 12; i++)
            {
                Months.Add(i, System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames[i - 1]);
            }
            ViewBag.Months = new SelectList((IEnumerable)Months, "Key", "Value", DateTime.Today.Month);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSDStatement(ChnSDStatement ObjSDStatement)
        {
            if (ModelState.IsValid)
            {
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();
                ObjRR.GetPDAStatement(ObjSDStatement.Month, ObjSDStatement.Year);
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePDFForPDAStatement(FormCollection Fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "SDStatement.pdf";
                Pages[0] = Fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report/SDStatement/";
                Pages[0] = Pages[0].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }

                return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SDStatement/" + FileName });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        #endregion

        #region PDSummary



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult PDASummaryReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPdSummaryReport(VRN_PdSummary ObjPdSummary, int drpType)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_PdSummary> LstPdSummary = new List<VRN_PdSummary>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PdSummaryReport(ObjPdSummary, drpType);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstPdSummary = (List<VRN_PdSummary>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePdSummaryPDF(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "SdSummary.pdf";
                Pages[0] = fc["Page"].ToString();

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/SdSummary/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SdSummary/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion

        #region E-Invoice Excel Generation

        [HttpGet]
        public ActionResult RegisterOfEInvoice()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult EInvoiceExcel(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                var excelName = "";
                var ObjRR = new Amd_ReportRepository();
                ObjRR.GetRegisterofEInvoice(date1, date2);

                excelName = "EInvoice.xls";

                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xls");
                    using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                    {
                        exl.AddCell("A1", "No data found");
                        exl.Save();
                        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EInvoice.xls");
                }
            }
            // return null;
        }

        #endregion

        #region Bulk IRN Generation

        [HttpGet]
        public ActionResult BulkIRNGeneration()
        {
            //WFLD_ReportRepository objPpgRepo = new WFLD_ReportRepository();
            //objPpgRepo.GetBulkIrnDetails();
            //if (objPpgRepo.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objPpgRepo.DBResponse.Data);
            //else
            //    ViewBag.InvoiceList = null;
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBulkIrnDetails()
        {
            Amd_ReportRepository objPpgRepo = new Amd_ReportRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (AMD_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            Amd_CWCExportController objvrnImp = new Amd_CWCExportController();
            Amd_CashManagementController objvrnCash = new Amd_CashManagementController();

            try
            {
                var invoiceData = JsonConvert.DeserializeObject<AMD_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            var result = await objvrnImp.GetIRNForYardInvoice(item.InvoiceNo, item.SupplyType);
                        }
                        else if (item.InvoiceType == "C")
                        {
                            var result1 = await objvrnCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                        }
                        else if (item.InvoiceType == "D")
                        {
                            var result2 = await objvrnCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                return Json(new { Status = 1, Message = "IRN Generated" });

            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region E04
        public ActionResult E04DetailsReport()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetE04List()
        {
            Amd_ReportRepository ObjER = new Amd_ReportRepository();
            List<Amd_E04Report> LstE04 = new List<Amd_E04Report>();
            ObjER.ListofE04Report(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Amd_E04Report>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfE04DetailsReport", LstE04);
        }
        [HttpGet]
        public JsonResult LoadMoreE04List(int Page)
        {
            Amd_ReportRepository ObjER = new Amd_ReportRepository();
            var LstE04 = new List<Amd_E04Report>();
            ObjER.ListofE04Report(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Amd_E04Report>)ObjER.DBResponse.Data;
            }
            return Json(LstE04, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewE04DetailsReport(int ID)
        {
            Amd_E04Report ObjE04 = new Amd_E04Report();
            Amd_ReportRepository ObjER = new Amd_ReportRepository();
            ObjER.GetE04DetailById(ID);
            if (ObjER.DBResponse.Data != null)
            {
                ObjE04 = (Amd_E04Report)ObjER.DBResponse.Data;
            }
            return PartialView(ObjE04);
        }


        [HttpGet]
        public JsonResult GetE04Search(string SB_No = "", string SB_Date = "", string Exp_Name = "")
        {
            Amd_ReportRepository ObjER = new Amd_ReportRepository();
            List<Amd_E04Report> LstE04 = new List<Amd_E04Report>();
            ObjER.GetE04DetailSearch(SB_No, SB_Date, Exp_Name);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Amd_E04Report>)ObjER.DBResponse.Data;
            }
            //return PartialView("ListOfE04DetailsReport", LstE04);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Stuffing Acknowledgement Search       

        [HttpGet]
        public ActionResult StfAckSearch()
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();

            ObjGR.GetAllContainerNoForContstufserach("", 0);

            if (ObjGR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstStuffing = Jobject["LstStuffing"];
                ViewBag.StateCont = Jobject["State"];
            }
            else
            {
                ViewBag.LstStuffing = null;
            }


            ObjGR.GetAllShippingBillNoForContstufserach("", 0);
            if (ObjGR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstStuff = Jobject["LstStuff"];
                ViewBag.StateShipbill = Jobject["State"];
            }
            else
            {
                ViewBag.LstStuff = null;
            }

            return PartialView();
        }
        [HttpGet]
        public JsonResult SearchContainerNo(string cont)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchShipbill(string shipbill)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadContainerLists(string cont, int Page)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadShipbillLists(string shipbill, int Page)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getcontstuffingacksearch(string container, string shipbill, string cfscode)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_ContStufAckRes> Lststufack = new List<Amd_ContStufAckRes>();
            ObjGR.GetStufAckResult(container, shipbill, cfscode);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        







        #region ASR Acknowledgement Search       

        [HttpGet]
        public ActionResult StuffingASRAckSearch()
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();

            //ObjGR.GetAllShippingBillNoForASRACK("", 0);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstStuff = Jobject["LstStuff"];
            //    ViewBag.StateShipbill = Jobject["State"];
            //}
            //else
            //{
            //    ViewBag.LstStuff = null;
            //}

            return PartialView();
        }

        [HttpGet]
        public JsonResult ASRSearchContainerNo(string cont)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetCotainerNoForASRAck(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadContainerLists(string cont, int Page)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetCotainerNoForASRAck(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRSearchShipbill(string shipbill)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadShipbillLists(string shipbill, int Page)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetASRACKStatusSearch(string shipbill, string CFSCode, string container)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_ContStufAckRes> Lststufack = new List<Amd_ContStufAckRes>();
            ObjGR.GetASRAckResult(shipbill, CFSCode, container);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DP Acknowledment Serach  

        [HttpGet]
        public ActionResult DPAckSearch()
        {
            return PartialView();
        }

        public JsonResult GetGatePassNoDPForAckSearch(string GatePassNo = "", int Page = 0)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_GatePassDPAckSearch> lstDPGPAck = new List<Amd_GatePassDPAckSearch>();
            ObjGR.GetGatePassNoDPForAckSearch(GatePassNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTGPAck = (List<Amd_GatePassDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDPAckSearch(string ContainerNo = "", int Page = 0)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_ContDPAckSearch> lstDPContACK = new List<Amd_ContDPAckSearch>();
            ObjGR.GetContainerNoForDPAckSearch(ContainerNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTContACK = (List<Amd_ContDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDPAckSearch(int GatePassId, string ContainerNo, int GatePassdtlId = 0)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_DPAckRes> lstDPAckRes = new List<Amd_DPAckRes>();
            ObjGR.GetDPAckSearch(GatePassId, ContainerNo, GatePassdtlId);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDPAckRes = (List<Amd_DPAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DT Acknowledment Serach  

        [HttpGet]
        public ActionResult DTAckSearch()
        {
            return PartialView();
        }

        public JsonResult GetGatePassNoDTForAckSearch(string GatePassNo = "", int Page = 0)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_GatePassDTAckSearch> lstDTGPAck = new List<Amd_GatePassDTAckSearch>();
            ObjGR.GetGatePassNoDTForAckSearch(GatePassNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDTAckSearch(string ContainerNo = "", int Page = 0)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_ContDTAckSearch> lstDTContACK = new List<Amd_ContDTAckSearch>();
            ObjGR.GetContainerNoForDTAckSearch(ContainerNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDTAckSearch(int GatePassId, string ContainerNo)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_DTAckRes> lstDTAckRes = new List<Amd_DTAckRes>();
            ObjGR.GetDTAckSearch(GatePassId, ContainerNo);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDTAckRes = (List<Amd_DTAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Stuffing Loaded Search
        [HttpGet]
        public ActionResult StfLoadSearch()
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();

            /* ObjGR.GetAllContainerNoForContstufserach("", 0);

             if (ObjGR.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.LstStuffing = Jobject["LstStuffing"];
                 ViewBag.StateCont = Jobject["State"];
             }
             else
             {
                 ViewBag.LstStuffing = null;
             }


             ObjGR.GetAllShippingBillNoForContstufserach("", 0);
             if (ObjGR.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.LstStuff = Jobject["LstStuff"];
                 ViewBag.StateShipbill = Jobject["State"];
             }
             else
             {
                 ViewBag.LstStuff = null;
             }*/

            return PartialView();
        }
        /* [HttpGet]
         public JsonResult SearchContainerNo(string cont)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllContainerNoForContstufserach(cont, 0);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }
         [HttpGet]
         public JsonResult SearchShipbill(string shipbill)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }
         [HttpGet]
         public JsonResult LoadContainerLists(string cont, int Page)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllContainerNoForContstufserach(cont, Page);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }


         [HttpGet]
         public JsonResult LoadShipbillLists(string shipbill, int Page)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }*/
        [HttpGet]
        public JsonResult getloadstufsearch(string jobno)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_loadstuf> Lststufack = new List<Amd_loadstuf>();
            ObjGR.GetStufloadResult(jobno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_loadstuf>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufasrsearch(string jobasrno)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_loadstufasr> Lststufack = new List<Amd_loadstufasr>();
            ObjGR.GetStufloadasrResult(jobasrno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_loadstufasr>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufdpsearch(string jobdpno)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_loadstufdp> Lststufack = new List<Amd_loadstufdp>();
            ObjGR.GetStufloaddpResult(jobdpno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_loadstufdp>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufdtsearch(string jobdtno)
        {
            Amd_ReportRepository ObjGR = new Amd_ReportRepository();
            List<Amd_loadstufdt> Lststufack = new List<Amd_loadstufdt>();
            ObjGR.GetStufloaddpResult(jobdtno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Amd_loadstufdt>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Bond STORAGE COLLECTION REPORT 
        [HttpGet]
        public ActionResult BondStorageCollectionReport()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BondStorageCollectionReport(string FromDate, string ToDate)
        {
            try
            {
                var ObjRR = new Amd_ReportRepository();
                ObjRR.BondStorageCollectionReport(FromDate, ToDate);
                if (ObjRR.DBResponse.Data != null)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    string Path = BondStorageCollectionReportPDF(ds, FromDate, ToDate);
                    return Json(new { Status = 1, Message = Path });

                }
                return Json(new { Status = -1, Message = "No data found" });
            }
            catch
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [NonAction]
        public string BondStorageCollectionReportPDF(DataSet ds, string FromDate, string ToDate)
        {
            try
            {
                var FileName = "BondStorageCollectionReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:7pt;'>" + ds.Tables[0].Rows[0]["CompanyAddress"].ToString() + "<br/>Email : " + ds.Tables[0].Rows[0]["EmailAddress"].ToString() + "</span><br/><label style='font-size: 7pt; font-weight:bold;'>Bond Storage Collection REGISTER</label><br/><label style='font-size: 7pt;'><b>From Date :</b> " + FromDate + " - <b>To Date :</b> " + ToDate + "</label></td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                decimal StoAmount = 0;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:0; font-size:7pt; text-align: center;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 5%;'>Sl No.</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Date</th>");
                Pages.Append("<th style='border-bottom: 1px solid #000; width: 10%;'>Storage Amount</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");
                ds.Tables[0].AsEnumerable().ToList().ForEach(item =>
                {
                    Pages.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item["Date"].ToString() + " </td>");
                    Pages.Append("<td style='border-bottom: 1px solid #000;'>" + item["Amount"].ToString() + "</td>");
                    Pages.Append("</tr>");
                    StoAmount = StoAmount + Convert.ToDecimal(item["Amount"]);
                    i++;
                });
                Pages.Append("<tr>");
                Pages.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:left;'>TOTAL</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + StoAmount + "</th></tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                //if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                //{
                //    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                //}
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region WorkSlip
        public ActionResult WorkSlipReport()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetWorkSlipReport(string PeriodFrom, string PeriodTo, int CasualLabour)
        {
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            string FilePath = "";
            string fdt = Convert.ToDateTime(PeriodFrom).ToString("yyyy-MM-dd");
            string tdt = Convert.ToDateTime(PeriodTo).ToString("yyyy-MM-dd");
            ObjRR.WorkSlipDetailsForPrint(fdt, tdt, CasualLabour, objLogin.Uid);//, objLogin.Uid
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                FilePath = GeneratingPDFforWS(ds, Convert.ToDateTime(PeriodFrom).ToString("dd-MMM-yy"), Convert.ToDateTime(PeriodTo).ToString("dd-MMM-yy"));
                return Json(new { Status = 1, Data = FilePath });
            }
            else
                return Json(new { Status = -1, Data = "No Record Found." });
        }

        [NonAction]
        public string GeneratingPDFforWS(DataSet ds, string dt, string dtt)
        {
            List<dynamic> objWs = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            List<dynamic> objImport = objWs.Where(o => o.Operation == "Import").ToList();
            List<dynamic> objExport = objWs.Where(o => o.Operation == "Export").ToList();
            List<dynamic> objGeneral = objWs.Where(o => o.Operation == "General").ToList();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td colspan='12'>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 12px;'>CFS C-9, MIDC Indl. Estate, <br/> Ambad, NASIK- 422010</label><br/><label style='font-size: 12px;'>Inland Container Depot.</label><br/><label style='font-size: 14px; font-weight:bold;'>Workslip</label></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><th align='left' colspan='4' width='30%' style='font-size:13px;'>WorkSlip No : " + objImport[0].WorkSlipNo + "</th><th align='left' colspan='8' width='70%' style='font-size:13px;'>From Date of Operation : " + dt + "</th><th align='left' colspan='8' width='70%' style='font-size:13px;'>To Date of Operation : " + dtt + "</th></tr>");
            html.Append("</tbody></table></td></tr>");

            //IMPORT
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><th width='5%' style='font-size:14px;' valign='top' align='right'>A).</th>");
            html.Append("<th colspan='2' width='95%' align='left' style='font-size:14px;'>IMPORT OPERATIONS :-:- (Refer Clause XXI for details)</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            //loop start
            objImport.ForEach(item =>
            {
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%' valign='top' align='left' style='font-size:12px;'>" + item.WSClause + "</td>");
                html.Append("<td colspan='2' width='95%' style='font-size:12px;'>" + item.Heading + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%'></td><th width='31%'  align='left' style='font-size:12px;'>" + item.CFSCodeCont20 + "</th><td width='30%'></td>");
                html.Append("<th colspan='5' width='40%' style='font-size:12px;' align='left'>" + item.CFSCodeCont40 + "</th></tr>");

                html.Append("<tr><td width='5%'></td>");
                html.Append("<td width='31%' valign='top'>");
                if (item.WSClause != "5" && item.WSClause != "6" && item.WSClause != "12" && item.WSClause != "13" && item.WSClause != "14")
                {
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='80%' align='left'>SUMMARY ITEM " + item.WSClause + "</th><th width='10%' align='left'>" + item.Count20 + "</th><th width='10%' align='right'>+</th>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td>");
                    html.Append("<td width='30%'>");
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='30%' align='left'>" + item.Count40 + "</th><th width='40%' align='left'>= " + item.CountTotal + "</th><th width='40%' align='left'>TEUS</th>");
                    html.Append("</tr></tbody></table>");
                }
                html.Append("</td>");
                html.Append("<td colspan='5' width='40%'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

            });
            //Loop end

            //EXPORT
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><th width='5%' style='font-size:14px;' valign='top' align='right'>B).</th>");
            html.Append("<th colspan='2' width='95%' align='left' style='font-size:14px;'>EXPORT OPERATIONS :-:- (Refer Clause XXI for details)</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            //loop start
            objExport.ForEach(item =>
            {
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%' valign='top' align='left' style='font-size:12px;'>" + item.WSClause + "</td>");
                html.Append("<td colspan='2' width='95%' style='font-size:12px;'>" + item.Heading + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%'></td><th width='31%'  align='left' style='font-size:12px;'>" + item.CFSCodeCont20 + "</th><td width='30%'></td>");
                html.Append("<th colspan='5' width='40%' style='font-size:12px;' align='left'>" + item.CFSCodeCont40 + "</th></tr>");

                html.Append("<tr><td width='5%'></td>");
                html.Append("<td width='31%' valign='top'>");
                if (item.WSClause != "5" && item.WSClause != "6" && item.WSClause != "12" && item.WSClause != "13" && item.WSClause != "14")
                {
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='80%' align='left'>SUMMARY ITEM " + item.WSClause + "</th><th width='10%' align='left'>" + item.Count20 + "</th><th width='10%' align='right'>+</th>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td>");
                    html.Append("<td width='30%'>");
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='30%' align='left'>" + item.Count40 + "</th><th width='40%' align='left'>= " + item.CountTotal + "</th><th width='40%' align='left'>TEUS</th>");
                    html.Append("</tr></tbody></table>");
                }
                html.Append("</td>");
                html.Append("<td colspan='5' width='40%'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

            });
            //Loop end

            //GENERAL
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><th width='5%' style='font-size:14px;' valign='top' align='right'>C).</th>");
            html.Append("<th colspan='2' width='95%' align='left' style='font-size:14px;'>GENERAl OPERATIONS :-</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            //loop start
            objGeneral.ForEach(item =>
            {
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%' valign='top' align='left' style='font-size:12px;'>" + item.WSClause + "</td>");
                html.Append("<td colspan='2' width='95%' style='font-size:12px;'>" + item.Heading + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='5%'></td><th width='31%'  align='left' style='font-size:12px;'>" + item.CFSCodeCont20 + "</th><td width='30%'></td>");
                html.Append("<th colspan='5' width='40%' style='font-size:12px;' align='left'>" + item.CFSCodeCont40 + "</th></tr>");

                html.Append("<tr><td width='5%'></td>");
                html.Append("<td width='31%' valign='top'>");

                if (item.WSClause != "5" && item.WSClause != "6" && item.WSClause != "12" && item.WSClause != "13" && item.WSClause != "14")
                {
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='80%' align='left'>SUMMARY ITEM " + item.WSClause + "</th><th width='10%' align='left'>" + item.Count20 + "</th><th width='10%' align='right'>+</th>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td>");
                    html.Append("<td width='30%'>");
                    html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
                    html.Append("<th width='30%' align='left'>" + item.Count40 + "</th><th width='40%' align='left'>= " + item.CountTotal + "</th><th width='40%' align='left'>TEUS</th>");
                    html.Append("</tr></tbody></table>");
                }
                html.Append("</td>");
                html.Append("<td colspan='5' width='40%'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

            });
            //Loop end

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='font-size:12px; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td><br/><br/><br/></td></tr>");
            html.Append("<tr><th colspan='2'>Name & Sign of H & T Contractor</th>");
            html.Append("<th colspan='1'></th>");
            html.Append("<th colspan='2'>Name & Sign of Supdt.Imp./Exp/Wh/C.U.</th>");
            html.Append("<th colspan='1'></th>");
            html.Append("<th colspan='2'>Name & Sign of S.A.M(Imp.) S.A.M. [Imp.][Exp.] Wh.</th>");
            html.Append("<th colspan='1'></th>");
            html.Append("<th colspan='2'>Name & Sign of Manager ICD</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            lstSB.Add(html.ToString());

            var FileName = "WorkSlipReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 40f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        #endregion    

        public JsonResult GetBulkReceiptReportAddMoneySD(BulkReceiptReport ObjBulkReceiptReport)
        {
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            ObjRR.GetPDACashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkReceiptReportAddMoneySD(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }
        [NonAction]
        public string GenerateBulkReceiptReportAddMoneySD(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
           // List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
           // List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");


                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                //html.Append("<tr><td colspan='9' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td><td colspan='3' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Folio No : </label><span>" + item.PartyCode + "</span></td></tr>");
                html.Append("<tr><td colspan='9' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td><td colspan='3' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'> </label><span>" + "" + "</span></td></tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address : </label><span>" + item.PartyAddress + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr>");


                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                //html.Append("<tr>");
                //html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                //html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }


        #region Bulk Invoice

        [HttpGet]
        public ActionResult BulkInvoiceReportForExternalUser()
        {

            //   ExportRepository objExport = new ExportRepository();
            //    objExport.GetPaymentParty();
            //  if (objExport.DBResponse.Status > 0)
            //      ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //   else
            //      ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetImpPartyBulkParty()
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetPaymentPartyforBulkReport();

            if (ObjRR.DBResponse.Data != null)
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReportForExternalUser(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjBulkInvoiceReport.PartyId = objLogin.EximTraderId;
            //When Module is selected All Condition against a party

            if (ObjBulkInvoiceReport.InvoiceModule == "All")
            {
                string ModuleName = "";
                //delete all the files in the folder before creating it
                if (System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    string deletelocation = Server.MapPath("~/Docs/All/") + Session.SessionID + "/";
                    DeleteDirectory(deletelocation);
                }

                //get all the distinct invoice module and invoices list with respect to party and date range 
                ObjRR.ModuleListWithInvoice(ObjBulkInvoiceReport);
                if (ObjRR.DBResponse.Status == 1)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    List<dynamic> lstModule = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
                    IList lstDistModule = lstModule.Select(x => x.Module).Distinct().ToList();
                    string FilePath = "";


                    foreach (string Mod in lstDistModule)
                    {
                        //lstModule.Where(x => x.Module == Mod).ToList().ForEach(itemInv =>
                        //{
                        //    String ModuleName= itemInv.Module;
                        switch (Mod)
                        {
                            //Here ds is list of invoice of a module between two dates 
                            case "IMPYard":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "ASSESSMENT SHEET FCL";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforYard((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "IMPDeli":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "ASSESSMENT SHEET LCL";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforGodown((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EXPLod":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "LOADED CONTAINER PAYMENT SHEET";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforExportLoadedCont((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "BTT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "BTT PAYMENT SHEET";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforBTT((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EC":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "EMPTY CONTAINER PAYMENT SHEET";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforEmptyContainer((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "ECTrns":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "EMPTY CONTAINER TRANSFER";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforEmptyContainer((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            default:
                                FilePath = GeneratingBulkPDFforVRNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                                break;
                        }
                        //});

                    }
                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }

            else
            {
                // List<PpgInvoiceGate> PpgInvoiceGateList = ObjRR.GetBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid
                ObjRR.GenericBulkInvoiceDetailsForPrintForExternalUser(ObjBulkInvoiceReport);//, objLogin.Uid

                if (ObjRR.DBResponse.Status == 1)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    //string FilePath = GeneratingBulkPDF(PpgInvoiceGateList, ObjBulkInvoiceReport.InvoiceModuleName);
                    string FilePath = "";
                    switch (ObjBulkInvoiceReport.InvoiceModule)
                    {
                        case "IMPYard":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforYard(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "IMPDeli":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforGodown(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXPSEALCHEKING":
                            FilePath = GeneratingBulkPDFforVRNExpSealChecking(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "BTT":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforBTT(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXP":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforExport(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "MiscInv":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforMisc(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "BND":
                            FilePath = GeneratingBulkPDFforBND(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "BNDAdv":
                            FilePath = GeneratingBulkPDFforBondHdb(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "EXPLod":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforExportLoadedCont(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EC":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        default:
                            FilePath = GeneratingBulkPDFforVRNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                    }

                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
        }
        [NonAction]
        public string GeneratingBulkPDFforVRNExpSealChecking(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>Seal Checking Payment Sheet</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyGSTNo.Substring(0, 2) + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr><tr><td><hr /></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;

                    /***************/
                    html.Append("</tbody></table></td></tr>");
                    html.Append("<tr><td>");
                    html.Append("</td></tr>");


                    html.Append("<tr><td>");
                    html.Append("<table cellspacing='0' cellpadding='5' style='border: 1px solid #000;width:100%; margin: 0;'><tbody>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Previous Seal No. :</label> <span>" + elem.PresentSealNo + "</span></td>");
                    html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>New Seal No. : </label> <span>" + elem.NewSealNo + "</span></td></tr>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Detention From :</label> <span>" + elem.Days + " Days</span></td>");
                    html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Lock Provided : </label> <span>" + elem.LockProvided + "</span></td></tr>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Custom Examinatiom :</label> <span>" + elem.ExamRequired + "</span></td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td></tr>");
                });
                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin-top: 20px;'>Container/Cargo Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + totamt.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");

                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-bottom:0; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 170px;'><span>&nbsp;&nbsp;</span>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th></tr>");


                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'><span>&nbsp;&nbsp;&nbsp;</span>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'><span>&nbsp;&nbsp;&nbsp;</span>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<span><br/></span>");
                html.Append("<p style='display: block; font-size: 11px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span>" + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span>" + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid #333; border-bottom: 0; border-right: 0; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
                html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
                html.Append("<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforBTT(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                VRN_ExportRepository rep = new VRN_ExportRepository();
                VRN_BTTCargoDet objCargoDet = new VRN_BTTCargoDet();
                rep.GetCargoDetBTTById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCargoDet = (VRN_BTTCargoDet)rep.DBResponse.Data;
                }
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");


                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 1px solid #000; width:100%; font-size: 6pt;' cellspacing='0' cellpadding='5'><tbody><tr>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + item.ExporterImporterName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> " + item.CHAName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b>" + item.ShippingLineName + "</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");


                int i = 1;
                Decimal totamt = 0;

                //------------------------------------------            
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Cargo Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Commodity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.ShippingBillNo.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.ShippingBillDate.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.CommodityName.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.NoOfUnits.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.GrossWeight.ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; text-align:right;'>" + objCargoDet.Fob.ToString() + "</td>");
                html.Append("</tr>");

                /***************/
                html.Append("</tbody></table></td></tr>");
                //---------------------

                html.Append("<tr><td>");
                html.Append("</td></tr>");
                //------------------------------------------                

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center; ' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b>" + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Exporter</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }
                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforExport(DataSet ds, string InvoiceModuleName, string All)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstShippingBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table cellpadding='5' style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");


                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Exporter Name</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int j = 1;

                lstShippingBill.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + j + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Exporter + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.NoOfUnits + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + elem.FobValue + "</td>");
                    html.Append("</tr>");
                    j = j + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforMisc(DataSet ds, string InvoiceModuleName, string All)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                int i = 1; Decimal totamt = 0;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b>" + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");
                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        [NonAction]
        public string GeneratingBulkPDFforBND(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstStorageDate = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 6pt; margin-top: 8px;'>Bond No :" + item.StuffingReqNo + "</b></th>");
                html.Append("<td colspan='6' width='50%' style='font-size:6pt; text-align: right'><label width='15%'><b>SAC validity :</b></label> From <u>" + lstContainer[0].SacApprovedDate + "</u> to <u>" + lstContainer[0].SacValidityDate + "</u></td></tr>");
                html.Append("<tr><th colspan='2' width='20%' style='text-align: left; font-size: 6pt; margin-top: 15px;' valign='top'>Storage Period :</th>");
                html.Append("<td colspan='10' width='80%'><table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='0'><tbody>");

                var SacId = 0;
                var DeliId = 0;
                if (lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).Count() > 0)
                {
                    SacId = lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).ToList().Select(y => y.SacId).ToArray()[0];
                    DeliId = lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).ToList().Select(y => y.SacDelivryId).ToArray()[0];
                }

                lstStorageDate.Where(x => x.SacId == SacId && x.InvoiceId <= item.InvoiceId && x.SacDelivryId <= DeliId).ToList().ForEach(storage =>
                {
                    int j = 1;
                    html.Append("<tr><td colspan='12' cellpadding='5' valign='top'>From <u>" + storage.StorageFromDt + "</u> to <u>" + storage.StorageToDt + "</u></td></tr>");
                    j = j + 1;
                });
                html.Append("</tbody></table></td></tr>");


                html.Append("<tr><td colspan='12'>");
                int i = 1;
                Decimal totamt = 0;
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(a =>
                {
                    html.Append("<tr><td colspan='6' width='30%'>Bond No.</td><td>:</td><td colspan='6' width='70%'>" + a.BondNo.ToString().TrimEnd(',') + " </td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Bond Date</td><td>:</td><td colspan='6' width='70%'>" + a.BondDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL No.</td><td>:</td><td colspan='6' width='70%'>" + a.BOLNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL Date</td><td>:</td><td colspan='6' width='70%'>" + a.BOLDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + a.BOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE Date.</td><td>:</td><td colspan='6' width='70%'>" + a.BOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB No.</td><td>:</td><td colspan='6' width='70%'>" + a.AWBNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB Date</td><td>:</td><td colspan='6' width='70%'>" + a.AWBDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Godown No.</td><td>:</td><td colspan='6' width='70%'>" + a.GodownName.ToString().TrimEnd(',') + "</td></tr>");
                });
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='border-left: 1px solid #000; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                lstContainer.Where(z => z.InvoiceId == item.InvoiceId).ToList().ForEach(b =>
                {
                    html.Append("<tr><td colspan='6' width='40%'>Ex-BOE No.</td><td>:</td><td colspan='6' width='60%'>" + b.ExBOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Ex-BOE Date.</td><td>:</td><td colspan='6' width='60%'>" + b.ExBOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Importer</td><td>:</td><td colspan='6' width='60%'>" + item.ExporterImporterName.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CHA Name</td><td>:</td><td colspan='6' width='60%'>" + item.CHAName.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CIF Value</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIF)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>DUTY</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIF) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>No Of Pkg</td><td>:</td><td colspan='6' width='60%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                });
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());

            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforBondHdb(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 7pt;margin-top: 10px;'>SAC No :" + item.StuffingReqNo + "</b></th>");

                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<td colspan='6' width='50%' style='font-size:7pt;'><b>SAC validity :</b> From <u>" + elem.SacApprovedDate + "</u> to <u>" + elem.SacValidityDate + "</u></td></tr>");
                });

                html.Append("<tr><td colspan='12'>");
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='border-right: 1px solid #000; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL No.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOLNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL Date</td><td>:</td><td colspan='6' width='70%'>" + elem.BOLDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE Date.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB No.</td><td>:</td><td colspan='6' width='70%'>" + elem.AWBNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB Date</td><td>:</td><td colspan='6' width='70%'>" + elem.AWBDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Godown No.</td><td>:</td><td colspan='6' width='70%'>" + elem.GodownName + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><td colspan='6' width='40%'>CHA Name</td><td>:</td><td colspan='6' width='60%'>" + item.CHAName + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CIF Value</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>DUTY</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(z => z.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>No Of Pkg</td><td>:</td><td colspan='6' width='60%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("</tr></tbody></table>");
                    html.Append("</td></tr>");

                    html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 6pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                    html.Append("<thead><tr>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                    html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                    html.Append("<tbody>");
                    i = 1;
                    /*Charges Bind*/
                });
                i = 1;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforExportLoadedCont(DataSet ds, string InvoiceModuleName, string All)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstShippingBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Container/Cargo Details :</b></th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");
                //----------------------
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Exporter Name</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int j = 1;

                lstShippingBill.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + j + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Exporter + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.NoOfUnits + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:right;'>" + elem.FobValue + "</td>");
                    html.Append("</tr>");
                    j = j + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                //---------------------

                html.Append("<tr><td>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Exporter</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        public string GeneratingBulkPDFforVRNAll(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            var distinctValues = ds.Tables[3].AsEnumerable()
                       .Select(row => new
                       {
                           SACCode = row.Field<string>("SACCode")

                       })
                       .Distinct();


            List<string> lstSB = new List<string>();

            lstInvoice.ToList().ForEach(item =>
            {
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                /*Header Part*/





                Decimal CTax = 0;
                if (item.TotalIGST > 0)
                {
                    CTax = item.TotalIGST;
                }
                else
                {
                    CTax = item.TotalCGST;
                }
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

                html.Append("<tr style='text-align: center;'><td colspan='2'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2></td></tr>");

                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td style='border:1px solid #333; border-bottom: none;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='vertical-align: top;'><img style='width: 100%;' src='logo.png'/></td>");
                html.Append("<td style='padding: 0 10px;'>");
                html.Append("<h1 style='font-size: 16px; margin:0; padding: 0;'>Central Warehousing Corporation</h1>");
                html.Append("<label style='font-size: 13px; text-transform:uppercase;'>(A Govt. of India Undertaking) </label>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>Principle Place of Business-RO Hyderabad,</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>1st Floor Warehousing Sadan,</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>Nampally, Behind Gandhi Bhavan</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>GSTIN/UIN: 36AAACC1206D2ZG</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>State Name: Telangana, Code: 36</h6>");
                html.Append("<h6 style='font-size: 11px; margin:0; font-weight:normal;'>Contact: 040-23773751,8142456715,9966483827,7337516165</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>E-Mail: cwccfs@gmail.com</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>www.cewacor.nic.in</h6>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("<td valign='top' cellspacing='0' cellpadding='0' style='border:1px solid #333; border-left: none; padding:0; border-bottom: none;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Invoice No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.InvoiceNo + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; padding: 10px; font-size: 12px;'>Dated<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.InvoiceDate + "</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Delivery Note<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Account</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Mode/Terms of Payment<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>15 Days</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Supplier's Ref.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Cfs Kukarpally</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Other Reference(s)<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase;margin: 0; padding: 0;'>Tn56h2594</h2></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");

                html.Append("<tr valign='top' cellpadding='0'>");
                html.Append("<td style='border:1px solid #333; border-bottom: none;'>");
                html.Append("<br/>");
                html.Append("<p style='display: block; font-size: 12px; margin:0; padding: 0 10px;'>Consignee</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; font-weight: bold; margin:0;'>" + item.PartyName + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'>" + item.PartyAddress + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'></p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>GSTIN/UIN:</span>" + item.PartyGSTNo + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>Pan/It No:</span> </p>");
                html.Append("<p style='display: block; font-size: 13px; margin:0; padding: 0 10px;'><span style='display: inline-block; width: 100px;'>State Name:</span> " + item.PartyState + "</p>");
                html.Append("</td>");
                int i = 1;
                String cfscd = "";
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    cfscd = cfscd + elem.CFSCode + ",";
                });

                html.Append("<td valign='top' cellspacing='0' cellpadding='0' style='border:1px solid #333; border-left: 0; border-top: 0;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top: 0; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Buyer's Order No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + cfscd + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top: 0; padding: 10px; font-size: 12px;'>Dated<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'></h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Despatch Document No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Be No: " + item.BOENo + " ," + item.BOEDate + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Delivery Note Date<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.DeliveryDate + "</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>despatched througn<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'></h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Destination<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase;margin: 0; padding: 0;'></h2></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");

                html.Append("<tr>");
                html.Append("<td style='border:1px solid #333; padding: 10px;'>");
                html.Append("<p style='display: block; font-size: 12px; margin:0;'>Buyer(if other than consignee)</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; font-weight: bold; margin:0;'> " + item.PayeeName + " </p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'>" + item.PartyAddress + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'></p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'>" + item.PartyState + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>GSTIN/UIN:</span> " + item.PartyGSTNo + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>Pan/It No:</span> </p>");
                html.Append("<p style='display: block; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>State Name:</span> " + item.PartyState + "</p>");
                html.Append("</td>");
                html.Append("<td style='border:1px solid #333; border-top: 0; border-left: none; padding:10px; vertical-align: top;'>");
                html.Append("<span style='display: block; font-size: 12px;'>Terms of Delivery</span>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + item.TotalNoOfPackages.ToString() + "</h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + item.TotalGrossWt.ToString("0.000") + "</h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Godown no: </h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>No of Grids: </h2>");
                html.Append("</td>");
                html.Append("</tr>");

                /***************/
                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; border-top: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>SL No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 300px;'>Particulars</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>HSN/SAC</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>GST Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Quantity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>per</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width:100px;'>Amount</th>");
                html.Append("</tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                Decimal cgstamt = 0;
                Decimal sgstamt = 0;
                Decimal igstamt = 0;
                Decimal tot = 0;
                String[] Sac;
                Sac = new string[50];
                Decimal[] Taxa;
                Decimal[] CenPer;
                Decimal[] StatePer;
                CenPer = new Decimal[50];
                StatePer = new Decimal[50];
                Taxa = new decimal[50];
                Decimal[] StateTax;
                StateTax = new decimal[50];
                Decimal[] CentralTax;
                CentralTax = new decimal[50];
                Decimal[] TotTax;
                TotTax = new decimal[50];
                Decimal Tamt = 0;
                Decimal Tcen = 0;
                Decimal Tstate = 0;
                Decimal TTax = 0;
                int count = 0;
                distinctValues.ToList().ForEach(data1 =>
                {
                    Sac[count] = data1.SACCode;
                    count++;
                });

                for (int ii = 0; ii < count; ii++)
                {
                    Taxa[ii] = 0;
                    CentralTax[ii] = 0;
                    StateTax[ii] = 0;
                    TotTax[ii] = 0;
                    lstCharges.ToList().ForEach(data =>
                    {


                        if (data.SACCode == Sac[ii] && data.InoviceId == item.InvoiceId)
                        {
                            Taxa[ii] = Taxa[ii] + data.Taxable;
                            cgstamt = cgstamt + data.CGSTAmt;
                            sgstamt = sgstamt + data.SGSTAmt;
                            igstamt = igstamt + data.IGSTAmt;
                            if (data.IGSTAmt > 0)
                            {
                                CenPer[ii] = data.IGSTPer;
                                StatePer[ii] = data.SGSTPer;
                                CentralTax[ii] = CentralTax[ii] + data.IGSTAmt;
                                StateTax[ii] = StateTax[ii] + (data.SGSTAmt + data.CGSTAmt);
                                TotTax[ii] = TotTax[ii] + (CentralTax[ii] + StateTax[ii]);
                                Tamt = Tamt + Taxa[ii];
                                Tcen = Tcen + CentralTax[ii];
                                Tstate = Tstate + StateTax[ii];
                                TTax = TTax + TotTax[ii];
                            }

                            else
                            {
                                CenPer[ii] = data.SGSTPer;
                                StatePer[ii] = data.CGSTPer;
                                CentralTax[ii] = CentralTax[ii] + data.SGSTAmt;
                                StateTax[ii] = StateTax[ii] + (data.CGSTAmt);
                                TotTax[ii] = TotTax[ii] + (CentralTax[ii] + StateTax[ii]);
                                Tamt = Tamt + Taxa[ii];
                                Tcen = Tcen + CentralTax[ii];
                                Tstate = Tstate + StateTax[ii];
                                TTax = TTax + TotTax[ii];

                            }
                        }
                    });
                }

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    Decimal gstper = 0;
                    gstper = data.CGSTPer + data.SGSTPer;

                    tot = data.Total;
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left; padding:0 20px 0 60px; font-weight:bold;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + gstper + " %</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'></td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'></td>");
                        html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + data.Taxable.ToString("0") + "</td></tr>");
                        i = i + 1;
                    }
                });
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'> " + i.ToString() + " </td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>CGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalCGST.ToString("0") + "</td></tr>");
                i = i + 1;
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>SGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalSGST.ToString("0") + "</td></tr>");
                i = i + 1;
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>IGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalIGST.ToString("0") + "</td></tr>");

                html.Append("<tr><td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: right; padding:0 20px 0 60px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: left;'></td>");
                html.Append("<td style=' border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 18px; text-align: right; font-weight: bold;'>" + item.InvoiceAmt.ToString("0") + " &#8377;</td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr>");

                html.Append("<tr cellspacing='0' cellpadding='0'><td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-top:0; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; border-bottom: 1px solid #000; text-align: left;'>Amount Chargeable(in words)<br/><span style='font-size: 13px; font-weight:bold;'>" + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</span></td>");
                html.Append("<td style='font-size: 13px; border-bottom: 1px solid #000; text-align: right; vertical-align: top;'><em>E. & o.E</em></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //Group OF CHARGES SECTION START
                // distinctValues.ToList().ForEach(item => item.SACCode=
                // {

                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-top: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 160px;'>HSN/SAC</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Taxable Value</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Central Tax</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>State Tax</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Total Tax Amount</th>");
                html.Append("</tr>");

                html.Append("<tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                //int j = 1;
                /*Charges Bind*/
                // lstCharges.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(data =>
                //{

                for (int ii = 0; ii < count; ii++)
                {
                    if (Taxa[ii] > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; font-size: 12px; text-align: left;'>" + Sac[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + Taxa[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + CenPer[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + CentralTax[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + StatePer[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + StateTax[ii] + " </td>");
                        html.Append("<td style='font-size: 12px; text-align: right;'>" + (CentralTax[ii] + StateTax[ii]) + "</td></tr>");
                    }
                }
                // });
                html.Append("<tr><td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>Total</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + item.TotalTaxable + "</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + (CTax) + "</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + (item.TotalSGST) + "</td>");
                html.Append("<td style='border-top: 1px solid #000; font-size: 13px; text-align: right; font-weight: bold;'>" + (item.TotalCGST + item.TotalSGST + item.TotalIGST) + "</td></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-left: 1px solid #000; border-right: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr style='margin: 0 0 40px;'>");
                html.Append("<td colspan='2' style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Tax Amount(in words) : <span style='font-size:14px; font-weight:bold;'>" + objCurr.changeCurrencyToWords((item.TotalCGST + item.TotalSGST + item.TotalIGST).ToString("0")) + "</span></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; border-left: 1px solid #333; border-right: 1px solid #333; border-bottom: 1px solid #333;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("<p style='font-size: 13px; margin:0; padding: 0 0 0 5px;'>Company's PAN: <b>AAACC1206D</b></p>");
                html.Append("<p style='font-size: 12px; margin:0; padding: 0 0 0 5px;'>Declaration</p>");
                html.Append("<p style='font-size: 12px; margin:0; padding: 0 0 0 5px;'>We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct</p>");
                html.Append("</td>");
                html.Append("<td style='padding-right: 0; padding-bottom: 0;'>");
                html.Append("<p style='font-size: 12px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; border-right: 0;  width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr>");
                html.Append("<td><p align='right' style='font-size: 13px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p><span><br/><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><p align='right' style='font-size: 13px; margin:0;'>Authorised Signatory</p></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%;' cellspacing='0' cellpadding='2'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='text-align:center; font-size: 11px; padding:0;'>SUBJECT TO HYDERABAD JURISDICTION</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='text-align:center; font-size: 11px;'>This is a Computer Generated Invoice</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("</td></tr>");

                html.Append("</tbody></table>");
                /***************/
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforGodown(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);
            //List<dynamic> lstReassesbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            int cargotype = 0;
            string Container = "";
            string dt = "";
            string dttype = "Date of Seal Cutting";
            Einvoice obj = new Einvoice(new HeaderParam(), "");


            //lstCargoDetail.ToList().ForEach(item =>
            //{
            //    cargotype = (int)item.CargoType;
            //});

            //lstInvoice.ToList().ForEach(item =>
            //{
            //    Ppg_ReportRepository rep = new Ppg_ReportRepository();
            //    PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
            //    rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
            //    if (rep.DBResponse.Data != null)
            //    {
            //        objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
            //    }



            //    lstReassesment.ForEach(itemm =>
            //    {
            //        if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
            //        {
            //            Container = "(Re-Assessment)";

            //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
            //            dt = repp.GetPreviousInvDate(itemm.cfscode);
            //            dttype = "Previous Delivery Date";


            //        }

            //    });


            //    lstReassesbulk.ForEach(data =>
            //    {
            //        if (data.invoiceid == item.InvoiceId)
            //        {
            //            Container = "(Re-Assessment)";
            //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
            //            dt = repp.GetPreviousInvDate(data.cfscode);
            //            dttype = "Previous Delivery Date";

            //        }
            //    });
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :" + item.StuffingReqNo + "</b></th></tr>");
                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Container / CBT Details :</b></th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Destuffing</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Days</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Week</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>" + dttype + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; width:8%;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    if (dt != "")
                    {
                        elem.SealCuttingDt = dt;
                    }
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DestuffingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1).ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1)) / 7)).ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.SealCuttingDt.ToString() + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOENo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOEDate).FirstOrDefault() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%' style='border-left: 1px solid #000;'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container / CBT Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());

            });
            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        //[NonAction]
        //public string GeneratingBulkPDFforEmptyContainer(DataSet ds, string InvoiceModuleName, string All)
        //{
        //    var FileName = "";
        //    var location = "";
        //    CurrencyToWordINR objCurr = new CurrencyToWordINR();
        //    List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
        //    List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
        //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
        //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
        //    List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
        //    List<string> lstSB = new List<string>();
        //    List<dynamic> lstReassesmentec = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
        //    List<dynamic> lstReassesmentecbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
        //    string Container = "";
        //    string cfscode = "";





        //    lstInvoice.ToList().ForEach(item =>
        //    {
        //        VRN_ReportRepository rep = new VRN_ReportRepository();
        //        VRN_SDBalancePrint objSDBalance = new VRN_SDBalancePrint();
        //        rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
        //        if (rep.DBResponse.Data != null)
        //        {
        //            objSDBalance = (VRN_SDBalancePrint)rep.DBResponse.Data;
        //        }
        //        cfscode = "";
        //        lstContainer.ForEach(dat =>
        //        {
        //            if (dat.InoviceId == item.InvoiceId)

        //                cfscode = dat.CFSCode;


        //        });
        //        Container = "";


        //        string dt = "";
        //        string dttype = "Date of Destuffing";
        //        lstReassesmentec.ForEach(itemm =>
        //        {
        //            if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
        //            {
        //                Container = "(Re-Assessment)";

        //                VRN_ReportRepository repp = new VRN_ReportRepository();
        //                dt = repp.GetPreviousInvDate(itemm.cfscode);
        //                dttype = "Previous Delivery Date";


        //            }

        //        });


        //        lstReassesmentecbulk.ForEach(data =>
        //        {
        //            if (data.invoiceid == item.InvoiceId)
        //            {
        //                Container = "(Re-Assessment)";
        //                VRN_ReportRepository repp = new VRN_ReportRepository();
        //                dt = repp.GetPreviousInvDate(data.cfscode);
        //                dttype = "Previous Delivery Date";

        //            }
        //        });


        //        StringBuilder html = new StringBuilder();
        //        /*Header Part*/
        //        html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
        //        html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
        //        html.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
        //        html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
        //        html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
        //        html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label><label style = 'font-size: 14px; font-weight:bold;' > " + Container + "</label>");
        //        html.Append("</td></tr>");

        //        html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
        //        html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
        //        html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

        //        html.Append("<tr><td>");
        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
        //        html.Append("<td colspan='5' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='6' width='40%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");
        //        html.Append("</tr></tbody></table>");
        //        html.Append("</td></tr>");



        //        html.Append("<tr><td><hr /></td></tr>");
        //        html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Assessment No :" + item.StuffingReqNo + "</b> ");
        //        html.Append("<br /><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container Details :</b> </th></tr>");
        //        html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:8pt;' cellspacing='0' cellpadding='8'>");
        //        html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>SR No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>ICD Code</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Container No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Size</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Arrival</th>");

        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + dttype + "</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Delivery</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Days</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Week</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Seal Cutting</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; text-align: center;'>Cargo Type</th>");
        //        html.Append("</tr></thead><tbody>");
        //        /*************/
        //        /*Container Bind*/
        //        int i = 1;
        //        Decimal totamt = 0;
        //        lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
        //        {
        //            if (dt != "")
        //            {
        //                elem.DestuffingEntryDate = dt;
        //            }
        //            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + i.ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.CFSCode + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ContainerNo + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.Size + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ArrivalDate + "</td>");
        //            // html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item.CartingDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.DestuffingEntryDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + item.DeliveryDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1)) / 7)).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.SealCuttingDt.ToString() + "</td>");
        //            html.Append("<td style='border-bottom: 1px solid #000; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
        //            html.Append("</tr>");
        //            i = i + 1;
        //        });
        //        /***************/
        //        html.Append("</tbody></table></td></tr>");
        //        html.Append("<tr><td>");
        //        html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");

        //        html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + item.BOENo + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'></td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("</tr></tbody></table>");

        //        html.Append("</td></tr>");

        //        html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
        //        html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<thead><tr>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
        //        html.Append("<tbody>");
        //        i = 1;
        //        /*Charges Bind*/

        //        lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
        //        {
        //            if (data.Taxable > 0)
        //            {
        //                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0") + "</td></tr>");
        //                i = i + 1;
        //                totamt = totamt + data.Taxable;
        //            }
        //        });
        //        html.Append("</tbody>");
        //        html.Append("</table></td></tr></tbody></table>");


        //        html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
        //        html.Append("<tbody>");

        //        html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + totamt.ToString("0") + "</th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0") + "</th></tr><tr>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0") + "</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0") + "</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0") + "</th></tr>");

        //        html.Append("<tr>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
        //        html.Append("</tr>");
        //        html.Append("<tr>");
        //        html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
        //        html.Append("</tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
        //        html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
        //        html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");


        //        html.Append("<tr><td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Signature CHA / Importer</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
        //        html.Append("</tr>");
        //        html.Append("<tr>");
        //        html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
        //        html.Append("</tr>");
        //        html.Append("</tbody></table>");

        //        html.Append("</td></tr></tbody></table>");
        //        /***************/

        //        html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
        //        lstSB.Add(html.ToString());
        //        Container = "";
        //        dt = "";
        //    });
        //    if (All != "All")
        //    {
        //        FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
        //        location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
        //        if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //        }
        //    }
        //    else
        //    {
        //        FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
        //        location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
        //        if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
        //        }
        //    }
        //    /*if (System.IO.File.Exists(location))
        //    {
        //        System.IO.File.Delete(location);
        //    }*/
        //    using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
        //    {
        //        rp.HeadOffice = "";
        //        rp.HOAddress = "";
        //        rp.ZonalOffice = "";
        //        rp.ZOAddress = "";
        //        rp.GeneratePDF(location, lstSB);
        //    }
        //    return "/Docs/" + Session.SessionID + "/" + FileName;
        //}

        [NonAction]
        public string GeneratingBulkPDFforEmptyContainer(DataSet ds, string InvoiceModuleName, string All)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);

            StringBuilder html = new StringBuilder();
            decimal totamt = 0;
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                /*Header Part*/
                html.Clear();
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Assessment No :" + item.StuffingReqNo + "</b></th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container Details :</b></th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC_IMG", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }


        [NonAction]
        public string GeneratingBulkPDFforYard(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            string dtype = "Date of Arrival";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstOBL = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            string Container = "";
            string cfscode = "";
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            //lstReasses.ForEach(item =>
            //{
            //    if (item.cfscode != "")
            //        Container = "(Re-Assessment)";

            //});

            lstInvoice.ToList().ForEach(item =>
            {
                VRN_ReportRepository rep = new VRN_ReportRepository();
                VRN_SDBalancePrint objSDBalance = new VRN_SDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (VRN_SDBalancePrint)rep.DBResponse.Data;
                }



                lstContainer.ForEach(dat =>
                {
                    if (dat.InoviceId == item.InvoiceId)

                        cfscode = dat.CFSCode;
                    else

                        cfscode = "";

                });
                Container = "";

                upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 10px;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 10px;'>Container/CBT Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:30px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:40px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>" + dtype + " </th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Weight</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; width:80px;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                //string OblNo = "";
                string RMS = "";
                string Movement = "";
                int i = 1;
                int flagvalue = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    //get no of days and weeks for IMPYard Payment Sheet
                    VRN_DaysWeeks objDaysWeeks = new VRN_DaysWeeks();

                    if (Container == "(Re-Assessment)")
                    {
                        elem.ArrivalDate = item.StuffingReqDate;
                        flagvalue = 1;
                    }

                    rep.GetDaysWeeksForIMPYard(item.InvoiceId, elem.CFSCode, flagvalue);
                    if (rep.DBResponse.Data != null)
                    {
                        objDaysWeeks = (VRN_DaysWeeks)rep.DBResponse.Data;

                    }

                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt.ToString("0.000") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objDaysWeeks.Days + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");

                    //OblNo = OblNo + elem.OBL_No + ".";
                    RMS = elem.RMS;
                    Movement = elem.Movement;
                    i = i + 1;
                });
                string OblNo = "";
                if (ds.Tables[6].Rows.Count > 0)
                {
                    OblNo = ds.Tables[6].Rows[0]["OBL_No"].ToString();
                }

                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + OblNo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + item.BOENo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + item.BOEDate + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>RMS</td><td>:</td><td colspan='6' width='70%'>" + RMS + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Movement</td><td>:</td><td colspan='6' width='70%'>" + Movement + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/CBT Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                decimal totalamt = 0;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    i = i + 1;
                    totalamt = totalamt + data.Taxable;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totalamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html = html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });

            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
            }

            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 40f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateBulkInvoiceReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "BulkInvoice.pdf";
                Pages[0] = fc["Page"].ToString();

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/BulkInvoice/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                {

                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/BulkInvoice/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }


        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWiseForExternal(string FromDate, string ToDate, string invoiceType, int PartyId)
        {
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            List<invoiceLIst> LstinvoiceLIst = new List<invoiceLIst>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetInvoiceListExternalUser(FromDate, ToDate, invoiceType, objLogin.EximTraderId);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Download()
        {
            String session = Session.SessionID;
            VRN_ReportCWCController obj = new VRN_ReportCWCController();

            string fileSavePath = "";
            fileSavePath = Server.MapPath("~/Docs/All/") + Session.SessionID;
            var filesCol = obj.GetFile(fileSavePath).ToList();
            string FileList = "";

            for (int i = 0; i < filesCol.Count; i++)
            {
                FileList = FileList + "/Docs/All/" + Session.SessionID + "/" + (filesCol[i].FileName) + ",";
            }
            var ObjResult = new { Status = 1, Message = FileList };
            return Json(ObjResult);
        }

        [NonAction]
        public List<FileList> GetFile(string fileSavePath)
        {
            List<FileList> listFiles = new List<FileList>();
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                listFiles.Add(new FileList()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + "/" + item.Name
                });
                i = i + 1;
            }
            return listFiles;
        }

        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    System.IO.File.Delete(file);
                }
            }
        }
        public string LoadImage(string img)
        {
            ////data:image/gif;base64,
            ////this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String(img);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            // image = Image.FromStream(ms);
            //}






            if (img != "")
            {

                string strm = img;

                //this is a simple white background image
                var myfilename = string.Format(@"{0}", Guid.NewGuid());

                //Generate unique filename
                string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                var bytess = Convert.FromBase64String(strm);
                using (var imageFile = new FileStream(filepath, FileMode.Create))
                {
                    imageFile.Write(bytess, 0, bytess.Length);
                    imageFile.Flush();
                }


                string targetpath = Server.MapPath("~/Docs/QRCode/") + myfilename + "crop" + ".jpeg";
                String newfilepath = Utility.ResizeImage(filepath, targetpath);
                return newfilepath;
            }
            else
            {
                return "";

            }
        }

        public string ResizeImage(String Source, String Target)
        {
            // string imgPath = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\Images1\49dc6397-31bb-43d7-b4d0-2db8f04f57c8.png";
            // string imgPathTarget = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\InvQrcode2.png";
            Bitmap source = new Bitmap(Source);
            Bitmap t = CropWhiteSpace(source);
            t.Save(Target);

            return (Target);
        }
        public Bitmap CropWhiteSpace(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int white = 0xffffff;

            Func<int, bool> allWhiteRow = r =>
            {
                for (int i = 0; i < w; ++i)
                    if ((bmp.GetPixel(i, r).ToArgb() & white) != white)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = c =>
            {
                for (int i = 0; i < h; ++i)
                    if ((bmp.GetPixel(c, i).ToArgb() & white) != white)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (!allWhiteRow(row))
                    break;
                topmost = row;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (!allWhiteRow(row))
                    break;
                bottommost = row;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (!allWhiteColumn(col))
                    break;
                leftmost = col;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (!allWhiteColumn(col))
                    break;
                rightmost = col;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                    new RectangleF(0, 0, croppedWidth, croppedHeight),
                    new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                    GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                ex);
            }
        }


        #endregion

        #region Bulk Receipt  Report
        [HttpGet]
        public ActionResult BulkReceiptReportForExternalUser()
        {
            return PartialView();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkReceiptReportForExternalUser(BulkReceiptReport ObjBulkReceiptReport)
        {
            Amd_ReportRepository ObjRR = new Amd_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetBulkCashreceiptForExternalUser(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber,objLogin.EximTraderId);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTDSDepositReport(BulkReceiptReport ObjBulkReceiptReport)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateTDSDepositReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkSDReceiptReport(BulkReceiptReport ObjBulkReceiptReport)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkSDReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }
        [NonAction]
        public string GenerateBulkReceiptReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");


                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='9' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td><td colspan='3' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Folio No : </label><span>" + item.PartyCode + "</span></td></tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address : </label><span>" + item.PartyAddress + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [NonAction]
        public string GenerateTDSDepositReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Certificate No.</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [NonAction]
        public string GenerateBulkSDReceiptReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>SD RECEIPT</label>");
                html.Append("</td></tr>");


                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>SD Balance</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetChequeBounceReceiptReport(String ReceiptNo)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetChequeBounceCashreceipt(ReceiptNo);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulk(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }


        [NonAction]
        public string GenerateBulk(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '>");
                html.Append("<tbody><tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr></tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                var InvoiceIds = item.InvoiceId.Split(',');
                var InvoiceNos = item.InvoiceNo.Split(',');
                var InvoiceAmts = item.Amt.Split(',');
                i = 0;
                foreach (var Invoice in InvoiceNos)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                    html.Append("</tr>");

                    i = i + 1;
                }

                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop

                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");

                    i = i + 1;
                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.InvoiceAmt.ToString() + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 80px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");
                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");


                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }
        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfReceiptDateWiseForExternalUser(string FromDate, string ToDate)
        {
            ReportRepository ObjRR = new ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetReceiptListForExternalUser(FromDate, ToDate, objLogin.EximTraderId);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BulkCreaditNoteReport
        [HttpGet]
        public ActionResult BulkCreaditNoteReportForExternalUser()
        {

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNoteForExternalUser(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            WFLD_ReportRepository objRepo = new WFLD_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            Login objLogin = (Login)Session["LoginUser"];
            objRepo.PrintDetailsForBulkCRNoteForExternalUser(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "C", objLogin.EximTraderId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR, "Credit Note");
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Record Not Found" });
            }
        }
        #endregion
        #region BulkDebitnoteReport
        [HttpGet]
        public ActionResult BulkDebitnoteReportForExternalUser()
        {

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDRNoteForExternalUser(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            WFLD_ReportRepository objRepo = new WFLD_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            Login objLogin = (Login)Session["LoginUser"];
            objRepo.PrintDetailsForBulkCRNoteForExternalUser(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "D",objLogin.EximTraderId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR, "Debit Note");
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Record Not Found" });
            }
        }

        //    public string GenerateCRNotePDF(PrintModelOfBulkCrCompany objCR)
        //    {
        //        string Note = "";
        //        string SACCode = "", note = "", fileName = "";
        //        objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (SACCode == "")
        //                SACCode = item.SACCode;
        //            else
        //                SACCode = SACCode + "," + item.SACCode;
        //        });

        //            note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
        //        fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
        //        string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
        //        if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //        {
        //            Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //        }
        //        if (System.IO.File.Exists(Path + "/" + fileName))
        //        {
        //            System.IO.File.Delete(Path + "/" + fileName);

        //        }
        //        objCR.lstCrParty.ToList().ForEach(item =>
        //        {
        //            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + item.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + item.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + item.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + item.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + item.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + item.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + item.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + item.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + item.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
        //        string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
        //        string tr = "";
        //        int Count = 1;
        //        decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
        //            objCR.lstCharges.Where(y => y.CRNoteId == item.CRNoteId).ToList().ForEach(data =>
        //           {
        //                tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
        //            IGSTAmt += item.IGSTAmt;
        //            CGSTAmt += item.CGSTAmt;
        //            SGSTAmt += item.SGSTAmt;
        //            Count++;
        //        });
        //        string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
        //        string Remarks = objCR.Remarks;
        //        string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
        //        html = html + htmltable + tr + tfoot;
        //        using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //        {
        //            RH.GeneratePDF(Path + "/" + fileName, html);
        //        }
        //        return "/Docs/" + Session.SessionID + "/" + fileName;
        //    }


        //}

        [NonAction]
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR, String CRDR)
        {


            List<string> lstSB = new List<string>();

            string SACCode = "";
            objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            {
                if (SACCode == "")
                    SACCode = item.SACCode;
                else
                    SACCode = SACCode + "," + item.SACCode;
            });

            objCR.lstCrParty.ToList().ForEach(item =>
            {

                //    /*Header Part*/
                StringBuilder html = new StringBuilder();

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td colspan='9' width='100%' valign='top' align='center'><center><label align='center' style='font-size: 14px; font-weight:bold;text-align:center;'> " + CRDR + "</label></center></td></tr>");

                html.Append("</tbody></table>");

                html.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='6' cellspacing='0' style='width:50%; border-right: 1px solid #000;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Provider</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b>" + objCR.CompanyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Warehouse Address:</b>" + objCR.CompanyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + objCR.CompCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + objCR.CompStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b>" + objCR.CompStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN:</b> " + objCR.CompGstIn + "</td></tr>");
                html.Append("<tr><td cellpadding='10'><b>PAN:</b>" + objCR.CompPan + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' cellspacing='0' Valign='top' style='width:50%;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Receiver</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b> " + item.PartyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Address:</b> " + item.PartyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + item.PartyCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + item.PartyStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b> " + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN(if registered):</b> " + item.PartyGSTIN + "</td></tr>");
                html.Append("<tr><td cellpadding='10'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='6' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Debit/Credit Note Serial No :</b> " + item.CRNoteNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Accounting Code of</b> " + SACCode + "</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Date of Issue :</b> " + Convert.ToDateTime(item.CRNoteDate).ToString("dd/MM/yyyy") + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Description of Services :</b> Other Storage & Warehousing Services</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Original Bill of Supply/Tax Invoice No :</b>  " + item.InvoiceNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Date :</b>  " + Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy") + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Sl No.</th>");
                html.Append("<th colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Particulars</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Value</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>Total Amount</th></tr>");
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Reason for increase/decrease in original invoice</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                int i = 1;
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                decimal total = 0;
                decimal TTax = 0;
                objCR.lstCharges.Where(y => y.CRNoteId == item.CRNoteId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>" + data.ChargeName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTAmt + "</td>");

                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + data.Total + "</td></tr>");
                    i = i + 1;
                    IGSTAmt += data.IGSTAmt;
                    TTax += data.Taxable;
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + TTax + " </td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Round Up:</b>" + item.RoundUp + "</td></tr>");

                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in figure):</b>" + item.GrandTotal + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in words):</b> " + AmountInWord + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Remarks:</b>" + item.Remarks + "</td></tr>");

                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><th>Note:</th></tr>");
                html.Append("<tr><td colspan='12'><b>1.</b> The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</td></tr>");
                html.Append("<tr><td colspan='12'><b>2.</b> Credit Note is to be issued where excess amount cliamed in original invoice.</td></tr>");
                html.Append("<tr><td colspan='12'><b>3.</b> Debit Note is to be issued where less amount claimed in original invoice.</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><td colspan='6' width='80%'></td>");
                html.Append("<th colspan='5' width='50%'>Signature: ____________________________ <br/><br/> Name of the Signatory: __________________ <br/><br/> Designation/Status: ____________________</th></tr>");

                html.Append("<tr><td colspan='6' width='50%'>To, <br/> ____________________________ <br/>____________________________<br/>____________________________<br/><br/> Copy To: <br/> Duplicate Copy for RM, CWC,RO - <br/> 2.Triplicate Copy for Warehouse</td>");
                html.Append("<th colspan='6' width='50%'></th></tr>");


                html.Append("</tbody></table>");
                // html.Append("<div style='margin-top:10px;'><br/><br/><br/><br/></div>");
                // html.Append("<div style='margin-top:10px;'><br/><br/></div>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());

            });



            var FileName = "BulkDebitReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        #endregion

        #region   Tally Response Report
        public ActionResult TallyResponseReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TallyResponseReport(TallyResponse vm)
        {
            WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            List<TallyResponse> lstData = new List<TallyResponse>();

            ObjRR.GetTallyResponse(vm);//, objLogin.Uid
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {

                lstData = (List<TallyResponse>)ObjRR.DBResponse.Data;
                Path = GenerateTallyResponseReport(lstData, vm.FromDate, vm.ToDate);
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "No Record Found.." }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GenerateTallyResponseReport(List<TallyResponse> lstData, string FromDate, string ToDate)
        {
            try
            {

                CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
                WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
                ObjRR.getCompanyDetails();
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                var FileName = "TallyResponseReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td valign='top' width='10%'><img align='right' src='IMGSRC'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>CFS C-9, MIDC Indl. Estate < br/> Ambad ,  NASIK- 422010</span><br/><label style='font-size: 7pt;'>Email - rmmum@cewacor.nic.in</ label>");
                Pages.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>Tally Response Report</label> <br/> <label style='font-size: 7pt; font-weight:bold;' ><b>From Date :</b> " + FromDate + " <b>To Date :</b> " + ToDate + "</label>");
                Pages.Append("</td>");
                Pages.Append("<td width='10%' valign='top'><img align='right' src='ISO_IMG'/></td>");
                Pages.Append("<td width='10%' valign='top'><img align='right' src='SWACHBHARAT'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; margin-bottom:10px; border-collapse:collapse; border:1px solid #000; border-bottom:0; text-align: center; font-size:7pt;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Sl.No.</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Date</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 15%;'>Bill of Supply</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 12%;'>Tax Invoice</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 5%;'>Debit Note</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 12%;'>Credit Note</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 15%;'>Receipt Voucher</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");


                lstData.ForEach(item =>
                {

                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Date + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Bill + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Invoice + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Dr + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Cr + " </td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Receipt + "</td>");
                    Pages.Append("</tr>");
                    i++;

                });



                // Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='font-size:10pt; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td><br/></td></tr>");
                Pages.Append("<tr><th colspan='12' style='text-align:left;'>* Contact with Helpdesk (CWC) team for resolution.</th></tr>");
                Pages.Append("</thead></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                Pages.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));



                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 30f, 20f, 30f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                //if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        #region Account Report Export Cargo In General Carting
        [HttpGet]
        public ActionResult ExportCargoReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetCargoReport(string AsOnDate)
        {
            if (ModelState.IsValid)
            {
                DSR_ReportRepository ObjRR = new DSR_ReportRepository();
                ObjRR.GetCargoExport(AsOnDate);
                string Path = "";
                List<Hdb_ExpCargo> lstData = new List<Hdb_ExpCargo>();
                lstData = (List<Hdb_ExpCargo>)ObjRR.DBResponse.Data;
                Path = GeneratePDFExpCarReport(lstData, AsOnDate);

                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFExpCarReport(List<Hdb_ExpCargo> lstData, string Date)
        {
            try
            {
                var FileName = "ExportCargoReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 14px; padding-bottom: 10px;'></span><br/><span style='font-size: 14px; padding-bottom: 10px;'>CFS C-9, MIDC Indl. Estate,Ambad, NASIK- 422010</span><br/><label style='font-size: 14px;'></label><br/><label style='font-size: 14px;'>As On Date - " + Date + "</label><br/><label style='font-size: 14px;'></label>");
                Pages.Append("<br /><label style='font-size: 16px; font-weight:bold;'>Accrued Income Report for Export Cargo In General Carting</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                //Pages.Append("<tr><td colspan='12'>");
                //Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS Kukatpally-Hyderabad</span><br/><span style='font-size:12px;'><b>As On Date - </b> " + Date + "</span><br/><label style='font-size: 14px; font-weight:bold;'>Account Report for Export Cargo In General Carting</label></td></tr>");
                //Pages.Append("</tbody></table>");
                //Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>S No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>Entry No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>In Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Sb No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Sb Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Shed</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Storage Type</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>FCL / LCL</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Area</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>No Of Days</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>No Of Week</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Storage Amt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:12%;text-align:right;'>Insurance Amt</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");
                lstData.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>" + item.EntryNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.InDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.SbNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.SbDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.Shed + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.StorageType + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.FCLLCL + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.Area + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>" + item.NoOfDays + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>" + item.NoOfWeek + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>" + item.GeneralAmount + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.InsuranceAmount + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='width:3%;'>Total :</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:8%;'></th>");
                Pages.Append("<th style='border-right:1px solid #000;width:8%;'>" + lstData.AsEnumerable().Sum(item => item.Area) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:6%;'>" + lstData.AsEnumerable().Sum(item => item.NoOfDays) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:6%;'>" + lstData.AsEnumerable().Sum(item => item.NoOfWeek) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:6%;'>" + lstData.AsEnumerable().Sum(item => item.GeneralAmount) + "</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + lstData.AsEnumerable().Sum(item => item.InsuranceAmount) + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }


        #endregion

        #region Accounts Report for Import Cargo
        [HttpGet]
        public ActionResult ImportCargoAccountsReport()
        {
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllGodown();
            if (ObjYR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = (List<CwcExim.Areas.Master.Models.HDBGodown>)ObjYR.DBResponse.Data;
            }
            else ViewBag.ListOfGodown = null;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetImpCargo(string AsOnDate, string GodownId, string GodownName)
        {
            if (ModelState.IsValid)
            {
                Hdb_ReportRepository ObjRR = new Hdb_ReportRepository();
                ObjRR.GetImpCargo(AsOnDate, GodownId);
                string Path = "";
                List<Hdb_ImpCargo> lstData = new List<Hdb_ImpCargo>();
                lstData = (List<Hdb_ImpCargo>)ObjRR.DBResponse.Data;
                Path = GeneratePDFImpCargo(lstData, AsOnDate, GodownName);

                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFImpCargo(List<Hdb_ImpCargo> lstData, string Date, string GodownName)
        {
            try
            {
                var FileName = "ImpCargoReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 14px; padding-bottom: 10px;'></span><br/><span style='font-size: 14px; padding-bottom: 10px;'>CFS C-9, MIDC Indl. Estate,Ambad, NASIK- 422010</span><br/><label style='font-size: 14px;'></label><br/><label style='font-size: 14px;'>As On Date - " + Date + "</label><br/><label style='font-size: 14px;'>Shed Cd - " + GodownName + "</label>");
                Pages.Append("<br /><label style='font-size: 16px; font-weight:bold;'>Account Report for Import Cargo</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                //Pages.Append("<tr><td colspan='12'>");
                //Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:14px;'>CFS Kukatpally-Hyderabad</span><br/><span style='font-size:14px;'><b>As On Date - </b> " + Date + "</span><br/><span style='font-size:14px;'><b>Shed Cd - </b>" + GodownName + "</span><br/><label style='font-size: 16px; font-weight:bold;'>Account Report for Import Cargo</label></td></tr>");
                //Pages.Append("</tbody></table>");
                //Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>S No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:12%;'>OBL No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Dstf Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Entry No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Entry Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Size</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>FCL / LCL</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Storage Type</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>Item No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>Pkg</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>Rcvd Pkg</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Gr Wt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Area</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Slot No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Remarks</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Total Wk</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Storage Amount</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:12%;text-align:right;'>Insurance Amount</th></tr></thead>");
                Pages.Append("<tbody>");
                lstData.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:12%;'>" + item.BOLNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>" + item.DestuffingEntryDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.CFSCode + "</td>");

                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.EntryDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.Size + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.LCLFCL + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.StorageArea + "</td>");

                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>" + item.CommodityAlias + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>" + item.NoOfUnits + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>" + item.NoOfUnitsRec + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.Weight.ToString("0.00") + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.Area.ToString("0.00") + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.LocationName + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.Remarks + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + item.TotWk + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + item.Amount + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.InsuranceAmount + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='2' style='width:3%;'>Total :</th>");
                Pages.Append("<th style='width:6%;'></th>");
                Pages.Append("<th style='width:7%;'></th>");
                Pages.Append("<th style='width:7%;'></th>");
                Pages.Append("<th style='width:7%;'></th>");
                Pages.Append("<th style='width:7%;'></th>");
                Pages.Append("<th style='width:7%;'></th>");
                Pages.Append("<th style='width:4%;'></th>");
                Pages.Append("<th style='border-right:1px solid #000;width:4%;'>" + lstData.AsEnumerable().Sum(item => item.NoOfUnits) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:4%;'>" + lstData.AsEnumerable().Sum(item => item.NoOfUnitsRec) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:8%;'>" + lstData.AsEnumerable().Sum(item => item.Weight) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:7%;'>" + lstData.AsEnumerable().Sum(item => item.Area) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:7%;'></th>");
                Pages.Append("<th style='border-right:1px solid #000;width:7%;'></th>");
                Pages.Append("<th style='border-right:1px solid #000;width:5%;'>" + lstData.AsEnumerable().Sum(item => item.TotWk) + "</th>");
                Pages.Append("<th style='border-right:1px solid #000;width:5%;'>" + lstData.AsEnumerable().Sum(item => item.Amount) + "</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + lstData.AsEnumerable().Sum(item => item.InsuranceAmount) + "</th>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }



        #endregion

        //Added by Susmita Patra-30-05-2024
        #region TDS Deduction Report
        public ActionResult TdsDeductionReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetTdsDeductionReport(Amd_TDSDeductionReport vm)
        {

            Response.AppendHeader("content-disposition", "attachment;filename=TDSDeductionReport.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            Amd_ReportRepository obj = new Amd_ReportRepository();
            obj.TdsDeductionExcelRpt(vm.PeriodFrom, vm.PeriodTo);
            DataSet ds = new DataSet();
            ds = (DataSet)obj.DBResponse.Data;
            Response.Write(TdsDeductionReportExcelPDF(ds, vm.PeriodFrom, vm.PeriodTo));
            Response.End();


            return Content("xc");
        }

        private string TdsDeductionReportExcelPDF(DataSet ds, string FromDate, string ToDate)
        {


            List<dynamic> lstTdsDeductionRptSummary = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            var i = 1;
            var FileName = "TdsDeductionReport.pdf";
            string html = "<html><body>";

            //Report of TDS Deduction Report
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; font-size:8pt;'><tbody>";
            html += "<tr><td colspan='12' valign='top' align='center'><h1 style='font-size: 20px; margin: 0; padding: 0;'>TDS Deduction Report</h1></td></tr>";
            html += "<tr><td colspan='10' valign='top' align='center'><h1 style='font-size: 20px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION, DELHI : RO AMBAD(" + FromDate + " to " + ToDate + ") </h1></td></tr>";
            html += "<tr><td colspan='12'><table cellspacing='0' cellpadding='15' style='text-align: center; border: 1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt;'>";

            html += "<thead>";

            html += "<tr>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>SL NO.</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>Branch Name</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>Name Of Depositor</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>TAN Number of Depositor</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>Invoice No</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>Invoice Date</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>Invoice Amount</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>TDS Amount</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000;'>TDS Deduction Date</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            int totalInvAmt = 0;
            int totalTdsAmt = 0;

            lstTdsDeductionRptSummary.ToList().ForEach(item =>
            {
                try
                {

                    html += "<tr>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + i + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.BranchName + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.DepositorName + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.TanNumber + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.InvoiceNo + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.InvoiceDate + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.Amount + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.TDS + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + item.ReceiptDate + "</td>";
                    html += "</tr>";

                    totalInvAmt = totalInvAmt + Convert.ToInt32(item.Amount);
                    totalTdsAmt = totalTdsAmt + Convert.ToInt32(item.TDS);

                    i++;

                }
                catch (Exception Ex)
                {

                }
            });

            html += "<tr>";
            html += "<th colspan='6' style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: right;'>TOTAL :</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + totalInvAmt + "</th>";
            html += "<th style='border-right: 1px solid #000; border-top: 1px solid #000; text-align: center;'>" + totalTdsAmt + "</th>";
            html += "<th style='border-top: 1px solid #000; text-align: center;'></th>";
            html += "</tr>";
            html += "</tbody>";
            html += "</table></td></tr>";
            html += "</tbody></table>";

            html += "</body></html>";
            string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/HtmlReport";

            if (!Directory.Exists(LocalDirectory))
            {
                Directory.CreateDirectory(LocalDirectory);
            }
            if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
            {
                System.IO.File.Delete(LocalDirectory + "/" + FileName);
            }

            System.IO.File.WriteAllText(LocalDirectory + "/" + FileName, html);


            FileInfo fi = new FileInfo(LocalDirectory + "/" + FileName);



            //Open a file for Read\Write
            FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);


            StreamReader sr = new StreamReader(fs);

            //Use the ReadToEnd method to read all the content from file
            string fileContent = sr.ReadToEnd();

            //Close the StreamReader object after operation
            sr.Close();
            // fs.Close();

            return fileContent;
        }
        #endregion
    }
}
