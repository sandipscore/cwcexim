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
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Auction.Models;
using System.IO.Compression;
using EinvoiceLibrary;
using System.Threading.Tasks;

namespace CwcExim.Areas.Report.Controllers
{
    public class TUT_ReportCWCController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }

        private string strQRCode = "000201010211021645992400041983220415545927000419832061661000500041983220827PUNB0042000420001210003366226410010A00000052401230514200A0000151.mab@pnb27260010A00000052401080601414A5204939953033565802IN5923CENTRAL WAREHOUSING COR6008GHAZIPUR6106110096621207080601414A6304b8e4";
        // GET: Report/LGS_ReportCWC

        public TUT_ReportCWCController()
        {
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            EffectVersion = Convert.ToDecimal(objCompanyDetails.EffectVersion);
            EffectVersionLogoFile = objCompanyDetails.VersionLogoFile.ToString();
        }


        #region Monthly SD Book

        [HttpGet]
        public ActionResult MonthlySDBookReport()
        {
            ViewBag.CompanyName = ZonalOffice;
            return PartialView();
        }
        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public ActionResult GetMonthlySDBookReport(string PeriodFrom, string PeriodTo)
        {

            LNSM_DailyCashBook ObjMonthlyCashBook = new LNSM_DailyCashBook();
            ObjMonthlyCashBook.PeriodFrom = PeriodFrom;
            ObjMonthlyCashBook.PeriodTo = PeriodTo;


            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyCashBook> LstMonthlyCashBook = new List<LNSM_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthSDBookReport(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstMonthlyCashBook = (List<LNSM_DailyCashBook>)ObjRR.DBResponse.Data;
                LstMonthlyCashBook.Add(new LNSM_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    ChqNo = "",
                    CRNo = "",
                    Depositor = "",
                    InvoiceDate = "",
                    InvoiceNo = "",
                    InvoiceType = "",
                    ModeOfPay = "",
                    PartyName = "",
                    PayeeName = "",
                    //PeriodFrom = "",
                    //PeriodTo = "",
                    Remarks = "",
                    TotalOthers = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString(),

                    MISC = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MISC)).ToString(),
                    MFCHRG = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MFCHRG)).ToString(),
                    INS = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.INS)).ToString(),
                    GRL = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GRL)).ToString(),
                    TPT = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TPT)).ToString(),
                    EIC = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EIC)).ToString(),
                    THCCHRG = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.THCCHRG)).ToString(),
                    GRE = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GRE)).ToString(),
                    RENT = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.RENT)).ToString(),
                    RRCHRG = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.RRCHRG)).ToString(),
                    STO = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.STO)).ToString(),
                    TAC = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TAC)).ToString(),
                    TIS = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TIS)).ToString(),
                    GENSPACE = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GENSPACE)).ToString(),





                    Cgst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString(),
                    Sgst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString(),
                    Igst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString(),
                    // Misc = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Misc)).ToString(),
                    // MiscExcess = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MiscExcess)).ToString(),
                    TotalCash = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)).ToString(),
                    TotalCheque = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString(),
                    Tds = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Tds)).ToString(),
                    CrTds = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CrTds)).ToString(),
                    TFUCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TFUCharge)).ToString()
                });
                ObjRR.DBResponse.Data = LstMonthlyCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateMonthlySDBookPDF(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "MonthlySDBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/MonthlySDBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 30f, 30f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/MonthlySDBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult MonthlySDBookDetailExcel(LNSM_DailyCashBook fc)
        {

            Session["CompanayName"] = ZonalOffice;

            List<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();
            LNSM_DailyCashBook ObjPV = new LNSM_DailyCashBook();
            ObjPV.PeriodFrom = (fc.PeriodFrom.ToString());
            ObjPV.PeriodTo = (fc.PeriodTo.ToString());
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.MonthlySDBookXls(ObjPV);
            string excelName = "";
            excelName = "MonthySDBook" + "_" + ".xls";
            if (ObjRR.DBResponse.Data != null)
            {
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            else
            {

                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }


        #endregion

        #region DailyCashBookReport                

        [HttpGet]
        public ActionResult DailyCashBookReport()
        {
            ViewBag.CompanyName = ZonalOffice;
            return PartialView();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public ActionResult GetDailyCashBookReport(LNSM_DailyCashBook ObjDailyCashBook)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBook(ObjDailyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<LNSM_DailyCashBook>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();

                LstDailyCashBook.Add(new LNSM_DailyCashBook()
                {

                    InvoiceDate = "<strong>Total</strong>",
                    InvoiceNo = "",
                    InvoiceType = "",
                    PartyName = "",
                    PayeeName = "",
                    ModeOfPay = "",
                    ChqNo = "",
                    CRNo = "",
                    ReceiptDate = "",

                    MISC = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MISC)).ToString(),
                    MFCHRG = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MFCHRG)).ToString(),
                    INS = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.INS)).ToString(),
                    GRL = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GRL)).ToString(),
                    TPT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TPT)).ToString(),
                    EIC = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EIC)).ToString(),
                    THCCHRG = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.THCCHRG)).ToString(),
                    GRE = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GRE)).ToString(),
                    RENT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.RENT)).ToString(),
                    RRCHRG = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.RRCHRG)).ToString(),
                    STO = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.STO)).ToString(),
                    TAC = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TAC)).ToString(),
                    TIS = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TIS)).ToString(),
                    GENSPACE = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GENSPACE)).ToString(),


                    Cgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString(),
                    Sgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString(),
                    Igst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString(),


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
        public JsonResult GenerateDailyCashBookReportPDF(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "DailyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 30f, 30f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/DailyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }



        [HttpGet]
        public ActionResult DailyCashBookReportXLS()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //Loni_ReportRepository ObjReport = new Loni_ReportRepository();
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
        public FileResult DailyCashBookDetailExcel(LNSM_DailyCashBook fc)
        {

            Session["CompanayName"] = ZonalOffice;

            List<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();
            LNSM_DailyCashBook ObjPV = new LNSM_DailyCashBook();
            ObjPV.PeriodFrom = (fc.PeriodFrom.ToString());
            ObjPV.PeriodTo = (fc.PeriodTo.ToString());
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.DailyCashBooKXls(ObjPV);
            string excelName = "";
            excelName = "DailyCashBookWithSD" + "_" + ".xls";
            if (ObjRR.DBResponse.Data != null)
            {
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            else
            {

                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }
        #endregion

        #region DailyCashBookReportCash
        [HttpGet]
        public ActionResult DailyCashBookReportCash()
        {
            ViewBag.CompanyName = ZonalOffice;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDailyCashBookReportCash(LNSM_DailyCashBook ObjDailyCashBook)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyCashBook> LstDailyCashBook = new List<LNSM_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBookCash(ObjDailyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<LNSM_DailyCashBook>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();

                LstDailyCashBook.Add(new LNSM_DailyCashBook()
                {
                    InvoiceDate = "<strong>Total</strong>",
                    InvoiceNo = "",
                    InvoiceType = "",
                    PartyName = "",
                    PayeeName = "",
                    ModeOfPay = "",
                    ChqNo = "",
                    //GenSpace = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GenSpace)).ToString(),
                    //StorageCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.StorageCharge)).ToString(),
                    //Insurance = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Insurance)).ToString(),
                    //GroundRentEmpty = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentEmpty)).ToString(),
                    //GroundRentLoaded = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentLoaded)).ToString(),
                    //MfCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MfCharge)).ToString(),
                    //EntryCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EntryCharge)).ToString(),
                    //Fumigation = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Fumigation)).ToString(),
                    //OtherCharge = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.OtherCharge)).ToString(),
                    // Misc = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Misc)).ToString(),
                    Cgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString(),
                    Sgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString(),
                    Igst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString(),
                    //   MiscExcess = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MiscExcess)).ToString(),
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
        public JsonResult GenerateDailyCashBookReportPDFCash(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "DailyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 30f, 30f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }

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

        [HttpGet]
        public ActionResult MonthlyCashBookReport()
        {
            ViewBag.CompanyName = ZonalOffice;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMonthlyCashBookReport(LNSM_DailyCashBook ObjMonthlyCashBook)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyCashBook> LstMonthlyCashBook = new List<LNSM_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthlyCashBook(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstMonthlyCashBook = (List<LNSM_DailyCashBook>)ObjRR.DBResponse.Data;
                LstMonthlyCashBook.Add(new LNSM_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",

                    //GenSpace = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GenSpace)).ToString(),
                    //StorageCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.StorageCharge)).ToString(),
                    //Insurance = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Insurance)).ToString(),
                    //GroundRentEmpty = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentEmpty)).ToString(),
                    //GroundRentLoaded = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.GroundRentLoaded)).ToString(),
                    //MfCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MfCharge)).ToString(),
                    //ThcCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.ThcCharge)).ToString(),

                    //RRCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.RRCharge)).ToString(),
                    //FACCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.FACCharge)).ToString(),

                    //EntryCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EntryCharge)).ToString(),
                    //Fumigation = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Fumigation)).ToString(),
                    //OtherCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.OtherCharge)).ToString(),
                    Cgst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)).ToString(),
                    Sgst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)).ToString(),
                    Igst = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)).ToString(),
                    // Misc = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Misc)).ToString(),
                    //  MiscExcess = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.MiscExcess)).ToString(),
                    TotalCash = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)).ToString(),
                    TotalCheque = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)).ToString(),
                    TotalOthers = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalOthers)).ToString(),
                    TotalPDA = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalPDA)).ToString(),
                    Tds = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Tds)).ToString(),
                    CrTds = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CrTds)).ToString(),
                    TFUCharge = LstMonthlyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TFUCharge)).ToString()
                });
                ObjRR.DBResponse.Data = LstMonthlyCashBook;
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
                var Pages = new string[1];
                var FileName = "MonthlyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/MonthlyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 30f, 30f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
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

        #region ChequeDdPoSdSummaryReport
        [HttpGet]
        public ActionResult ChequeDdPoSdSummaryReport()
        {
            return PartialView();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult GetChequeDdPoSdSummaryReport(string PeriodFrom, string PeriodTo, string Type)
        {
            LNSM_CashChequeDDSummary ObjChequeSummary = new LNSM_CashChequeDDSummary();
            ObjChequeSummary.PeriodFrom = PeriodFrom;
            ObjChequeSummary.PeriodTo = PeriodTo;
            ObjChequeSummary.Type = Type;

            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_CashChequeDDSummary> LstChequeSummary = new List<LNSM_CashChequeDDSummary>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.ChequeCashDDPOSummary(ObjChequeSummary);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstChequeSummary = (List<LNSM_CashChequeDDSummary>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateChequeDdPoSdSummaryReportPDF(FormCollection fc)
        {
            try
            {

                var Pages = new string[1];
                var FileName = "ChequeSummary.pdf";
                Pages[0] = fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/ChequeSummary/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/ChequeSummary/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult GetChequeDdPoSdSummaryReportExcel(LNSM_CashChequeDDSummary ObjChequeSummary)
        {
            Session["CompanayName"] = ZonalOffice;
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();

            ObjRR.ChequeCashDDPOSummaryExcel(ObjChequeSummary);//, objLogin.Uid
            string excelName = "";
            excelName = "ChequeDdPoSdSummaryReport" + "_" + ".xls";
            if (ObjRR.DBResponse.Data != null)
            {
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            else
            {

                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }


        #endregion

        #region SD Details Report
        public ActionResult SDDetailsReport()
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
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
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadParty(string PartyCode, int Page)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public JsonResult GetSDDetReport(string FromDate, string ToDate, int PartyId)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();

            string Fdt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetSDDetStatement(PartyId, Fdt, Tdt);
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                LNSM_SDDetailsStatement SDData = new LNSM_SDDetailsStatement();
                SDData = (LNSM_SDDetailsStatement)ObjRR.DBResponse.Data;

                Path = GeneratePDFSDDetReport(SDData, FromDate, ToDate);
            }
            return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult GetSDDetReportExcel(FormCollection fc)
        {
            Session["CompanayName"] = ZonalOffice;
            LNSM_SDDetReport ObjPV = new LNSM_SDDetReport();
            ObjPV.FromDate = (fc["FromDate"].ToString());
            ObjPV.ToDate = (fc["ToDate"].ToString());
            ObjPV.PartyId = (Convert.ToInt32(fc["PartyId"]));

            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            string Fdt = Convert.ToDateTime(ObjPV.FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ObjPV.ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetSDDetStatementExcel(ObjPV);

            string Path = "";
            string excelName = "";
            excelName = "SDStatementDetail" + "_" + ".xls";
            if (ObjRR.DBResponse.Data == null)
                ObjRR.DBResponse.Data = string.Empty;

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


        [NonAction]
        public string GeneratePDFSDDetReport(LNSM_SDDetailsStatement SDData, string FromDate, string ToDate)
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
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + ZonalOffice + "</span><br/><label style='font-size: 14px; font-weight:bold;'>SD STATEMENT</label></td></tr>");
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
                Pages.Append("<th colspan='7' style='border-right:1px solid #000;width:8%;text-align:right;'>Invoice Utilization Balance :</th>");
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

        #region DailyPdaActivityReport

        [HttpGet]
        public ActionResult DailyPdaActivityReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult GetDailyPdaActivityReportExcel(LNSM_DailyPdaActivityReport ObjDailyPdaActivityReport)
        {
            Session["CompanayName"] = ZonalOffice;
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyPdaActivityReport> LstDailyPdaActivityReport = new List<LNSM_DailyPdaActivityReport>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyPdaActivityExcel(ObjDailyPdaActivityReport);//, objLogin.Uid
            string Path = "";
            string excelName = "";
            excelName = "DailyPdaActivityReport" + "_" + ".xls";
            if (ObjRR.DBResponse.Data == null)
                ObjRR.DBResponse.Data = string.Empty;

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

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public ActionResult GetDailyPdaActivityReport(string PeriodFrom, string PeriodTo)
        {
            LNSM_DailyPdaActivityReport ObjDailyPdaActivityReport = new LNSM_DailyPdaActivityReport();
            ObjDailyPdaActivityReport.PeriodFrom = PeriodFrom;
            ObjDailyPdaActivityReport.PeriodTo = PeriodTo;
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_DailyPdaActivityReport> LstDailyPdaActivityReport = new List<LNSM_DailyPdaActivityReport>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyPdaActivity(ObjDailyPdaActivityReport);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyPdaActivityReport = (List<LNSM_DailyPdaActivityReport>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateDailyPdaActivityReportPDF(FormCollection fc)
        {
            try
            {

                string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                var Pages = new string[1];
                var FileName = "DailySdActivityReport.pdf";
                Pages[0] = fc["Page"].ToString();

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailySdActivityReport/";
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
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/DailySdActivityReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion

        #region PDASummaryUtilizationReport

        [HttpGet]
        public ActionResult PDASummaryUtilizationReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPdSummaryUtilizationReport(LNSM_PdSummary ObjPdSummary, int drpType)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<LNSM_PdSummary> LstPdSummary = new List<LNSM_PdSummary>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PdSummaryUtilizationReport(ObjPdSummary, drpType);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstPdSummary = (List<LNSM_PdSummary>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }


        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePdSummaryUtilizationPDF(FormCollection fc)
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

        #region CashReceiptInvoiceLedgerReport Partywise
        public ActionResult CashReceiptInvoiceLedgerReport()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]),
                Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            LNSM_ReportRepository objImport = new LNSM_ReportRepository();
            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpPost]
        public JsonResult GetCashReceiptInvoiceLedgerReport(int partyId, string fromdate, string todate)
        {
            try
            {
                LNSM_ReportRepository Repo = new LNSM_ReportRepository();
                Repo.GetCrInvLedgerReport(partyId, Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"), Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), ZonalOffice, ZOAddress);
                object obj = Repo.DBResponse;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "", Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region register of outward supply

        [HttpGet]
        public ActionResult RegisterOfOutwardSupply()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult RegisterOfOutwardSupply(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                var Type = fc["ddlType"].ToString();
                var excelName = "";
                var ObjRR = new LNSM_ReportRepository();
                ObjRR.GetRegisterofOutwardSupply(date1, date2, Type);

                if (Type == "Inv") { Type = "Invoice"; }
                if (Type == "C") { Type = "Credit"; }
                if (Type == "D") { Type = "Debit"; }
                if (Type == "Unpaid") { Type = "Unpaid"; }
                if (Type == "CancelInv") { Type = "Cancel Invoice"; }

                excelName = "RegisterofOutwardSupply" + "_" + Type + ".xlsx";

                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
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
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xlsx");
                }
            }
            // return null;
        }

        #endregion

        #region Invoice Information
        [HttpGet]
        public ActionResult InvoiceInformation()
        {

            LNSM_ReportRepository objParty = new LNSM_ReportRepository();
            //objParty.GetInvPaymentParty();
            //if (objParty.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objParty.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetInvPaymentParty(int Page = 0, string SearchValue = "")
        {
            LNSM_ReportRepository objRepo = new LNSM_ReportRepository();
            objRepo.GetInvPaymentParty(Page, SearchValue);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllInvoiceInformation(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {

            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_InvoiceInformation> lstInv = new List<LNSM_InvoiceInformation>();
            ObjGR.GetAllInvoiceInformation(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_InvoiceInformation>)ObjGR.DBResponse.Data;
            return PartialView("ListOfInvoiceInformation", lstInv);

        }

        [HttpGet]
        public ActionResult GetLoadMoreInvoiceInformation(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_InvoiceInformation> lstInv = new List<LNSM_InvoiceInformation>();
            ObjGR.GetAllInvoiceInformation(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_InvoiceInformation>)ObjGR.DBResponse.Data;
            return Json(lstInv, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceInformationPrint(LNSM_InvoiceInformation ObjBulkInvoiceReport)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GenericInvoiceDetailsForPrint(ObjBulkInvoiceReport);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string FilePath = "";
                FilePath = GeneratingInvoicePDF(ds);
                return Json(new { Status = 1, Data = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Data = "No Record Found." });
            }

        }

        [NonAction]
        public string GeneratingInvoicePDF(DataSet ds)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");

            var BQRFileName = "";
            var BQRlocation = "";

            var FileName = "";
            var location = "";

            var SEZis = "";
            string dtype = "Date of Arrival";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstOBLShipBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<dynamic> objBond = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[5]);

            List<string> lstSB = new List<string>();
            string Container = "";
            string cfscode = "";
            string FDType = "";
            //lstReasses.ForEach(item =>
            //{
            //    if (item.cfscode != "")
            //        Container = "(Re-Assessment)";

            //});

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
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            lstInvoice.ToList().ForEach(item =>
            {
                IrnResponse objERes = new IrnResponse();
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);

                //if (!System.IO.File.Exists(BQRlocation))
                //{

                //    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                //    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                //    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                //    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                //    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                //    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                //    upiQRInfo.billing_email = Convert.ToString(item.Email);
                //    //for Bulk

                //    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                //    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                //    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                //    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                //    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                //    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                //    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                //    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                //    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                //    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                //    upiQRInfo.tr = Convert.ToString(item.InvoiceId);

                //    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                //    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                //    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                //    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                //    Einvoice Eobj = new Einvoice();
                //    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                //    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);

                //    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                //    {
                //        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                //    }

                //    string strm = objresponse.QrCodeBase64;

                //    //this is a simple white background image
                //    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                //    //Generate unique filename

                //    var bytess = Convert.FromBase64String(strm);
                //    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                //    {
                //        imageFile.Write(bytess, 0, bytess.Length);
                //        imageFile.Flush();
                //    }

                //    LNSM_ReportRepository BQRObj = new LNSM_ReportRepository();
                //    BQRObj.AddEditBQRCode(item.InvoiceId, BQRFileName, 0);

                //    objERes.SignedQRCode = objresponse.QrCodeBase64;
                //    objERes.SignedInvoice = objresponse.QrCodeJson;
                //    objERes.SignedQRCode = objresponse.QrCodeJson;
                //}


                //lstContainer.ForEach(dat =>
                //{
                //    if (dat.InoviceId == item.InvoiceId)

                //        cfscode = dat.CFSCode;
                //    else

                //        cfscode = "";



                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='77%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + FDType + "</label><label style='font-size: 7pt; font-weight:bold;' ></ label><br/></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + BQRlocation + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img width='150px' height='150px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 0;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + "</h2> </td></tr>");


                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Reference No</th><th>:</th><td colspan='6' width='70%'>" + item.RefInvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Train No</th><th>:</th><td colspan='6' width='50%'>" + item.TrainNo + "</td></tr>");

                html.Append("</tbody></table>");

                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Reference Date</th><th>:</th><td colspan='6' width='50%'>" + item.RefInvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Application No</th><th>:</th><td colspan='6' width='50%'>" + item.ApplicationNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Reference No 2</th><th>:</th><td colspan='6' width='50%'>" + item.ReferencesNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'></th></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //Container Details
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt;margin-top: 0; margin-bottom: 0;'><b>Container / CBT Details :</b></h3> </th></tr>");
                //html.Append("<tr><b>Container / CBT Details :</b></tr>");               
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>Sl No</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>Cont No</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 70px;'>Size</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Type</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Quantity</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Gross WT</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Service Type</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>From Location</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>To Location</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>From Date</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Arrival Date</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 90px;'>Narration</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Commodity</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {                    
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:3%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + elem.ContNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + elem.CargoType + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + elem.Quantity + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + elem.GrossWt + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.ServiceType + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.FromLocation + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.ToLocation + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.FromDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 120px;'>" + elem.Arrivaldate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.Narration + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + elem.Commodity + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                //html.Append("<tr><td>");


                string OblNo = "";
                string RMS = "";
                //int i = 1;
                int flagvalue = 0;

                html.Append("<tr><th><h3 style='text-align: left; font-size: 6pt;margin-top: 0; margin-bottom: 0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 70px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 200px;'>Cont No</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 70px;'>Size</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>Type</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>Cargo WT</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>Gross WT</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>Service Type</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>From Location</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>To Location</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>From Date</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>To Date</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 90px;'>Narration</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>Commodity</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 70px;'>Qty</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Amount</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>UGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.ContNo + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Size + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Type + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.CargoWT + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.GrossWt + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.ServiceType + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.FromLocation + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.ToLocation + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.FromDate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.ToDate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Narration + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Commodity + "</td>");

                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Quantity + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 100px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: center; width: 50px;'>" + data.UGSTPer.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 4pt; text-align: right; width: 50px;'>" + data.UGSTAmount.ToString("0.00") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: right; font-size: 4pt; width: 100px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:4pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                

                html.Append("<tr>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 120px;'>Total</th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 80px;'>" + item.TotalTaxable.ToString("0.00") + "</th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'>" + item.TotalTaxable.ToString("0.00") + "</th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='font-size: 4pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 4pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 4pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 4pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 4pt; text-align: center; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.00") + " </th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 4pt; text-align: center; width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; margin-bottom: 10px; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                //html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='12' width='100%'>Remarks:<label style='font-weight: bold;'>" + item.Remarks.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='3' width='25%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");

                html.Append("<tr><th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/>(Authorized Signatories)</th></tr>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:7pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");

                }

                html.Append("<tr>");
                html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>This tax invoice is system generated, hence does not require any sign.</p></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });

            //if (All != "All")
            //{
            FileName = "InvoiceInfo" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 10f, 10f, false, true))
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

        public string LoadImage(string img)
        {

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

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public JsonResult GetInvoiceInformationPrint(LNSM_InvoiceInformation ObjBulkInvoiceReport)
        //{
        //    if (ObjBulkInvoiceReport.InvoiceNumber == null)
        //    {
        //        ObjBulkInvoiceReport.InvoiceNumber = "";
        //    }
        //    LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();


        //    //AuctionInvoiceViewModel ObjBulk = new AuctionInvoiceViewModel();
        //    //ObjBulk.InvoiceModule = ObjBulkInvoiceReport.InvoiceModule;
        //    //ObjBulk.InvoiceModuleName = ObjBulkInvoiceReport.InvoiceModuleName;
        //    //ObjBulk.InvoiceNumber = ObjBulkInvoiceReport.InvoiceNumber;
        //    //ObjBulk.PartyId = Convert.ToString(ObjBulkInvoiceReport.PartyId);
        //    //ObjBulk.PeriodFrom = ObjBulkInvoiceReport.PeriodFrom;
        //    //ObjBulk.PeriodTo = ObjBulkInvoiceReport.PeriodTo;
        //    //ObjBulk.All = ObjBulkInvoiceReport.All;


        //    Login objLogin = (Login)Session["LoginUser"];
        //    //When Module is selected All Condition against a party

        //    //if (ObjBulkInvoiceReport.InvoiceModule == "All")


        //        ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);

        //        if (ObjRR.DBResponse.Status == 1)
        //        {
        //            DataSet dss = (DataSet)ObjRR.DBResponse.Data;
        //            DataSet ds = (DataSet)ObjRR.DBResponse.Data;

        //            string FilePath = "";
        //            switch (ObjBulkInvoiceReport.InvoiceModule)
        //            {
        //                case "IMPYard":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforYard(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                case "IMPDeli":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforGodown(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                case "EXPLod":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                case "EXPMovement":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforContainerMovement(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                //case "CCIN":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforCCINEntry(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                case "BTT":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforBTT(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                //case "AUC":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingAssessmentSheetReportAuction(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                case "EC":
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //                //case "ECTrns":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                //case "IMPSC":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforSealCutting(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                //case "EXPCRGSHFT":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforCargoSF(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                //case "Fumigation":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforFumigationInvoice(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                //case "EXPCS":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    //FilePath = GeneratingBulkPDFforContainerStuffingInvoice(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                //case "IMPTS":
        //                //    ObjBulkInvoiceReport.All = "";
        //                //    ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
        //                //    //FilePath = GeneratingBulkPDFforTrainSummary(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                //    break;
        //                default:
        //                    ObjBulkInvoiceReport.All = "";
        //                    //FilePath = GeneratingBulkPDFforGE(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
        //                    break;
        //            }

        //            return Json(new { Status = 1, Data = FilePath });
        //        }
        //        else
        //        {
        //            return Json(new { Status = -1, Data = "No Record Found." });
        //        }

        //    //}
        //}

        #endregion

        #region Daily Transaction Report
        //public ActionResult DailyTransactionReport2()
        //{

        //    GodownRepository ObjGR = new GodownRepository();
        //    ObjGR.GetAllGodown();
        //    List<CwcExim.Models.Godown> LstGodown = new List<CwcExim.Models.Godown>();
        //    if (ObjGR.DBResponse.Data != null)
        //    {
        //        LstGodown = (List<CwcExim.Models.Godown>)ObjGR.DBResponse.Data;

        //        LstGodown.Add(new CwcExim.Models.Godown { GodownId = 99, GodownName = "Yard" });
        //    }
        //    return PartialView(LstGodown);
        //}
        public ActionResult DailyTransactionReport()
        {

            //LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            ObjGR.GetAllGodown();
            List<LNSM_Godown> LstGodown = new List<LNSM_Godown>();
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    LstGodown = (List<LNSM_Godown>)ObjGR.DBResponse.Data;

            //    //LstGodown.Add(new LNSM_Godown { GodownId = 99, GodownName = "Yard" }
            //    );
            //}

            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<LNSM_Godown>)ObjGR.DBResponse.Data;

                LstGodown.Add(new LNSM_Godown { GodownId = 99, GodownName = "Yard" });
            }

            return PartialView(LstGodown);
        }

        [HttpPost]
        public JsonResult GetDailyTransactioReport(string DTRDate, string DTRToDate, string Module, string GodownName, int GodownId = 0)
        {
            if (GodownId > 0)
            {
                LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
                Login objLogin = (Login)Session["LoginUser"];
                string FilePath = "";
                string dt = Convert.ToDateTime(DTRDate).ToString("yyyy-MM-dd");

                string dt1 = Convert.ToDateTime(DTRToDate).ToString("yyyy-MM-dd");

                if (Module == "Import")
                {
                    ObjRR.DTRForPrint(dt, GodownId);//, objLogin.Uid
                    if (ObjRR.DBResponse.Status == 1)
                    {
                        DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                        if (GodownName == "Yard")
                        {
                            FilePath = GeneratingPDFforDTRYard(ds, Convert.ToDateTime(dt).ToString("dd-MMM-yy"), GodownName);
                        }
                        else
                        {
                            FilePath = GeneratingPDFforDTR(ds, Convert.ToDateTime(dt).ToString("dd-MMM-yy"), GodownName);
                        }
                        return Json(new { Status = 1, Data = FilePath });
                    }
                    else
                        return Json(new { Status = -1, Data = "No Record Found." });
                }
                else
                {
                    ObjRR.DTRForExport(dt, dt1, GodownId);//, objLogin.Uid
                    if (ObjRR.DBResponse.Status == 1)
                    {
                        FilePath = GeneratingPDFForDTRExp((LNSM_DTRExp)ObjRR.DBResponse.Data, Convert.ToDateTime(dt).ToString("dd-MMM-yy"), GodownName);
                        return Json(new { Status = 1, Data = FilePath });
                    }
                    else
                        return Json(new { Status = -1, Data = "No Record Found." });
                }
            }
            else if (Module == "Import")
            {
                LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
                string FilePath = "";
                string dt = Convert.ToDateTime(DTRDate).ToString("yyyy-MM-dd");

                ObjRR.DTRForImportFCL(dt);
                if (ObjRR.DBResponse.Status == 1)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    FilePath = GeneratingPDFforDTRImpFCL(ds, dt);
                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
            else
            {
                return Json(new { Status = -1, Data = "Select Godown" });
            }
        }

        [NonAction]
        public string GeneratingPDFforDTR(DataSet ds, string dt, string gname)
        {
            //List<dynamic> lstOpening = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstCont = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstCargo = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstReceipt = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);

            //List<dynamic> lstClosing = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            String Shed = "";
            //lstCont.Select(x => new { Shed = x.Shed }).Distinct().ToList().ForEach(item =>
            //{
            //    if (Shed == "")
            //        Shed = item.Shed;
            //    else
            //        Shed += "," + item.Shed;
            //});

            //var sumobl = lstCont.AsEnumerable()
            //            .Sum(r => r.NoObl);

            //var sumNoPkg = lstCont.AsEnumerable()
            //            .Sum(r => r.NoPkg);

            //var sumGrWt = lstCont.AsEnumerable()
            //            .Sum(r => (decimal)r.GrWt);

            //var sumNoBoe = lstCont.AsEnumerable()
            //            .Sum(r => r.NoBoe);

            //var sumArea = lstCont.AsEnumerable()
            //            .Sum(r => (decimal)r.Area);


            var sumobl = 0;
            var sumNoPkg = 0;
            var sumGrWt = 0;
            var sumNoBoe = 0;
            var sumArea = 0;
            var SlNo = 1;

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            html.Append("<tr><td colspan='12' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 12px;'>" + ZonalOffice + "</label><br/><label style='font-size: 12px;'>For Workslip</label><br/><label style='font-size: 14px; font-weight:bold;'>Daily Transaction Report</label></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'><b>SHED :<span>&nbsp;</span></b>" + gname + "</td></tr>");
            html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            html.Append("<tr><th style='font-size:13px; text-align:left;' width='30%'>Daily Transaction Report (Import Unit) as on date :</th><td style='font-size:12px;' width='57%'>" + dt + "</td>");
            html.Append("<th style='font-size:13px; text-align:right;'>Date : <span>&nbsp;</span></th><td style='font-size:12px;'>" + dt + "</td></tr></tbody></table></td></tr>");

            //html.Append("<tr><td colspan='12' style='font-size:13px;'><b>Daily Transaction Report (Import Unit) as on date :</b> " + dt + "</td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>DESTUFFING OF CONTAINERS :</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr>");
            html.Append("<th style='text-align:left;padding:10px;border-bottom:1px solid #000;width:3%;'>SL No</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:12%;'>Container No.</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:5%;'>Ct Sz</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:12%;'>ICD Code</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>In Date</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Sla Cd</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Origin</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Ob</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Pkg</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Gr Wt</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Boe</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:11%;'>Shed</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Area</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Seal Cut.Date</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            //lstCont.ForEach(item =>
            foreach (var item in lstCont)
            {

                html.Append("<tr>");
                html.Append("<td style='text-align:left;padding:10px;width:3%;'>" + SlNo + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:12%;'>" + item.ContainerNo + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:5%;'>" + item.Size + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:12%;'>" + item.CFSCode + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.InDate + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.SlaCd + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.Origin + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoObl + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoPkg + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.GrWt + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoBoe + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:11%;'>" + item.Shed + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.Area + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.SealCuttingDate + "</td>");
                html.Append("</tr>");
                sumobl += item.NoObl;
                sumGrWt += item.GrWt;
                sumNoBoe += item.NoBoe;
                sumArea += item.Area;
            };
            html.Append("<tr><th colspan='7' style='border-top:1px solid #000;text-align:left;padding:10px;'>Total :-</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumobl + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumNoPkg + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumGrWt + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumNoBoe + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;'></th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumArea + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;'></th></tr>");
            html.Append("</tbody>");
            html.Append("</table></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>CARGO DELIVERED FROM THE GODOWN :</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border:1px solid #000;border-bottom:none;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>No of Boe</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>No of Pkg</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>Gr Weight</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>Area</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody><tr>");

            var cargoNoBoe = 0;
            var cargoNoPkg = 0;
            decimal cargoGrWt = 0;
            decimal cargoArea = 0;
            int Openingboe = 0;
            int Openingpkg = 0;
            decimal Openingwgt = 0;
            decimal Openingarea = 0;
            //lstOpening.ForEach(item =>
            //{
            //    Openingboe = item.Openingboe;
            //    Openingpkg = item.Openingpkg;
            //    Openingwgt = item.Openingwgt;
            //    Openingarea = item.Openingarea;

            //});

            int Closingboe = 0;
            int Closingpkg = 0;
            decimal Closingwgt = 0;
            decimal Closingarea = 0;
            //lstClosing.ForEach(item =>
            //{
            //    Closingboe = (int)item.Closingboe;
            //    Closingpkg = (int)item.Closingpkg;
            //    Closingwgt = item.Closingwgt;
            //    Closingarea = item.Closingarea;

            //});

            //lstCargo.ForEach(item =>
            foreach (var item2 in lstCargo)
            {

                cargoNoBoe = Convert.ToInt32(item2.NoBoe);
                cargoNoPkg = Convert.ToInt32(item2.NoPkg);
                cargoGrWt = Convert.ToInt32(item2.GrWt);
                cargoArea = Convert.ToInt32(item2.Area);
                html.Append("<td style='text-align:left;padding:10px;border-right:1px solid #000;'>" + item2.NoBoe + "</td>");
                html.Append("<td style='text-align:center;padding:10px;border-right:1px solid #000;'>" + item2.NoPkg + "</td>");
                html.Append("<td style='text-align:center;padding:10px;border-right:1px solid #000;'>" + item2.GrWt + "</td>");
                html.Append("<td style='text-align:center;padding:10px;'>" + item2.Area + "</td>");

            };
            html.Append("</tr></tbody>");
            html.Append("</table></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>SUMMARY</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>OPENING BALANCE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");

            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");

            //html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingboe + "</td> ");
            //html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingpkg + "</td> ");
            //html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingwgt + "</td>");
            //html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + Openingarea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>RECEIPT</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");

            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumobl + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumNoPkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumGrWt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + sumArea + "</td></tr>");

            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>ISSUE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoNoBoe + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoNoPkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoGrWt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + cargoArea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>CLOSING BALANCE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingboe + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingpkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingwgt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + Closingarea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td><br/><br/><br/><br/><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th colspan='3' width='25%' style='text-align:center;padding:10px;'>WA/JS/TA</th> ");
            html.Append("<th colspan='1' width='8.333333333333333%'></th>");
            html.Append("<th colspan='3' width='25%' style='text-align:center;padding:10px;'>SIO/SUPTD</th>");
            html.Append("<th colspan='1' width='8.333333333333333%'></th>");
            html.Append("<th colspan='3' width='25%' style='text-align:center;padding:10px;'>SAM(IMPORT)</th></tr>");
            html.Append("</tbody></table></td></tr>");

            html.Append("</tbody></table>");
            lstSB.Add(html.ToString());

            var FileName = "DTR" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 40f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }


        [NonAction]
        public string GeneratingPDFforDTRYard(DataSet ds, string dt, string gname)
        {
            List<dynamic> lstOpening = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstCont = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstCargo = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstReceipt = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);

            List<dynamic> lstClosing = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            String Shed = "";
            lstCont.Select(x => new { Shed = x.Shed }).Distinct().ToList().ForEach(item =>
            {
                if (Shed == "")
                    Shed = item.Shed;
                else
                    Shed += "," + item.Shed;
            });

            var sumobl = lstCont.AsEnumerable()
                        .Sum(r => r.NoObl);

            var sumNoPkg = lstCont.AsEnumerable()
                        .Sum(r => r.NoPkg);

            var sumGrWt = lstCont.AsEnumerable()
                        .Sum(r => (decimal)r.GrWt);

            var sumNoBoe = lstCont.AsEnumerable()
                        .Sum(r => r.NoBoe);

            var sumArea = lstCont.AsEnumerable()
                        .Sum(r => (decimal)r.Area);


            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            html.Append("<tr><td colspan='12' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 12px;'>" + ZonalOffice + "</label><br/><label style='font-size: 12px;'>For Workslip</label><br/><label style='font-size: 14px; font-weight:bold;'>Daily Transaction Report</label></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'><b>SHED :<span>&nbsp;</span></b>" + gname + "</td></tr>");
            html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            html.Append("<tr><th style='font-size:13px; text-align:left;' width='30%'>Daily Transaction Report (Import Unit) as on date :</th><td style='font-size:12px;' width='57%'>" + dt + "</td>");
            html.Append("<th style='font-size:13px; text-align:right;'>Date : <span>&nbsp;</span></th><td style='font-size:12px;'>" + dt + "</td></tr></tbody></table></td></tr>");

            //html.Append("<tr><td colspan='12' style='font-size:13px;'><b>Daily Transaction Report (Import Unit) as on date :</b> " + dt + "</td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>CUSTOM APPRAISEMENT OF CONTAINERS :</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr>");
            html.Append("<th style='text-align:left;padding:10px;border-bottom:1px solid #000;width:3%;'>SL No</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:12%;'>Container No.</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:5%;'>Ct Sz</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:12%;'>ICD Code</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>In Date</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Sla Cd</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Origin</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Ob</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Pkg</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Gr Wt</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>No Boe</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:11%;'>Shed</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Area</th>");
            html.Append("<th style='text-align:center;padding:10px;border-bottom:1px solid #000;width:10%;'>Appraisement Date</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody>");

            lstCont.ForEach(item =>
            {
                html.Append("<tr>");
                html.Append("<td style='text-align:left;padding:10px;width:3%;'>" + item.SlNo + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:12%;'>" + item.ContainerNo + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:5%;'>" + item.Size + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:12%;'>" + item.CFSCode + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.InDate + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.SlaCd + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.Origin + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoObl + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoPkg + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.GrWt + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.NoBoe + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:11%;'>" + item.Shed + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.Area + "</td>");
                html.Append("<td style='text-align:center;padding:10px;width:10%;'>" + item.SealCuttingDate + "</td>");
                html.Append("</tr>");
            });
            html.Append("<tr><th colspan='7' style='border-top:1px solid #000;text-align:left;padding:10px;'>Total :-</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumobl + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumNoPkg + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumGrWt + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumNoBoe + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;'></th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;width:10%;'>" + sumArea + "</th>");
            html.Append("<th style='border-top:1px solid #000;text-align:center;padding:10px;'></th></tr>");
            html.Append("</tbody>");
            html.Append("</table></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>CONTAINER DELIVERED FROM THE YARD :</td></tr>");

            html.Append("<tr><td colspan='12'><table style='border:1px solid #000;border-bottom:none;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>No of Boe</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>No of Pkg</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>Gr Weight</th>");
            html.Append("<th style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>Area</th>");
            html.Append("</tr></thead>");
            html.Append("<tbody><tr>");

            int cargoNoBoe = 0;
            int cargoNoPkg = 0;
            decimal cargoGrWt = 0;
            decimal cargoArea = 0;
            int Openingboe = 0;
            int Openingpkg = 0;
            decimal Openingwgt = 0;
            decimal Openingarea = 0;
            lstOpening.ForEach(item =>
            {
                Openingboe = item.Openingboe;
                Openingpkg = item.Openingpkg;
                Openingwgt = item.Openingwgt;
                Openingarea = item.Openingarea;

            });

            int Closingboe = 0;
            int Closingpkg = 0;
            decimal Closingwgt = 0;
            decimal Closingarea = 0;
            lstClosing.ForEach(item =>
            {
                Closingboe = (int)item.Closingboe;
                Closingpkg = (int)item.Closingpkg;
                Closingwgt = item.Closingwgt;
                Closingarea = item.Closingarea;

            });

            lstCargo.ForEach(item =>
            {
                cargoNoBoe = item.NoBoe;
                cargoNoPkg = item.NoPkg;
                cargoGrWt = item.GrWt;
                cargoArea = item.Area;
                html.Append("<td style='text-align:left;padding:10px;border-right:1px solid #000;'>" + item.NoBoe + "</td>");
                html.Append("<td style='text-align:center;padding:10px;border-right:1px solid #000;'>" + item.NoPkg + "</td>");
                html.Append("<td style='text-align:center;padding:10px;border-right:1px solid #000;'>" + item.GrWt + "</td>");
                html.Append("<td style='text-align:center;padding:10px;'>" + item.Area + "</td>");

            });
            html.Append("</tr></tbody>");
            html.Append("</table></td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='font-size:13px;'>SUMMARY</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>OPENING BALANCE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingboe + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingpkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Openingwgt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + Openingarea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>RECEIPT</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumobl + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumNoPkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + sumGrWt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + sumArea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>ISSUE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoNoBoe + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoNoPkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + cargoGrWt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + cargoArea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("<td colspan='3' width='25%' valign='top' style='padding:0;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            html.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>CLOSING BALANCE</th></tr>");
            html.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Obl</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No Pkg</th> ");
            html.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight</th>");
            html.Append("<th width='10%' style='text-align:center;padding:10px;'>Area</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></thead>");
            html.Append("<tbody><tr><td colspan='12'>");
            html.Append("<table cellspacing='0' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingboe + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingpkg + "</td> ");
            html.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Closingwgt + "</td>");
            html.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + Closingarea + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody>");
            html.Append("</table></td>");

            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td><br/><br/><br/><br/><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            html.Append("<tr><th colspan='3' width='25%' style='text-align:center;padding:10px;'>WA/JS/TA</th> ");
            html.Append("<th colspan='1' width='8.333333333333333%'></th>");
            html.Append("<th colspan='3' width='25%' style='text-align:center;padding:10px;'>SIO/SUPTD</th>");
            html.Append("<th colspan='1' width='8.333333333333333%'></th>");
            html.Append("<th colspan='3' width='25%' style='text-align:center;padding:10px;'>SAM(IMPORT)</th></tr>");
            html.Append("</tbody></table></td></tr>");

            html.Append("</tbody></table>");
            lstSB.Add(html.ToString());

            var FileName = "DTR" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 40f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingPDFForDTRExp(LNSM_DTRExp obj, string dt, string GodownName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
            sb.Append("<tr><td colspan='12'>");
            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            sb.Append("<td width='200%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + ZonalOffice + "</span><br/><label style='font-size: 14px; font-weight:bold;'>STOCK REGISTER CUM DAILY TRANSACTION REPORT</label></td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td colspan='12'><br/><br/></td></tr>");
            sb.Append("<tr><td colspan='12'>");
            sb.Append("<table cellspacing='0' cellpadding='0' style='font-size:12px; width:100%;'><tbody>");
            sb.Append("<tr><td colspan='4' width='33.33333333333333%'><b>Shed No :</b> " + GodownName + " </td>  <td colspan='4' width='33.33333333333333%' style='text-align:center;'><b>DATE :</b> " + dt + " </td>  <td colspan='4' width='33.33333333333333%' style='text-align:right;'><b>PRINTED ON :</b> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " </td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td colspan='12'><br/></td></tr>");
            sb.Append("</thead></table>");

            sb.Append("<table cellspacing='0' cellpadding='0' style='font-size:8pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            //LOOP Start//Carting Details
            /*
            obj.lstCartingDetails.Select(a => new { ShippingLineId = a.ShippingLineId, ShippingLineName = a.ShippingLineName, SLA = a.SLA }).Distinct().ToList().ForEach(item =>
            {
                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' cellpadding='10' style='font-size:8pt; width:100%;'><tbody>");
                sb.Append("<tr><th width='8%'>Sla</th><th width='8%'>" + item.SLA + "</th><th colspan='10'>" + item.ShippingLineName + "</th></tr>");

                sb.Append("<tr><th width='8%' style='border-bottom: 1px solid #000;'>Entry No</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Sb No</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Sb Date</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Exporter</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Cargo</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>No PKG</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Gr Wt</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Fob</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Slot</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>G/R</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Area</th>");
                sb.Append("<th width='10%' style='border-bottom: 1px solid #000;'>Remarks</th></tr>");

                //LOOP Start//
                obj.lstCartingDetails.Where(m => m.ShippingLineId == item.ShippingLineId).ToList().ForEach(elem =>
                {
                    sb.Append("<tr><td width='8%' style='border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.ExporterName + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.CargoDescription + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ActualQty + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ActualWeight + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.FobValue + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.Slot + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.GR + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.Area + "</td>");
                    sb.Append("<td width='10%' style='border-bottom: 1px solid #000;'>" + elem.Remarks + "</td></tr>");
                });
                //LOOP End//

                sb.Append("<tr><th width='8%' valign='bottom'>Sub Total :</th>");
                sb.Append("<td width='8%' valign='bottom'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='11%'></td>");
                sb.Append("<td width='11%'></td>");
                sb.Append("<td width='8%' valign='bottom'>" + obj.lstCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.ActualQty) + "</td>");
                sb.Append("<td width='8%' valign='bottom'>" + obj.lstCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.ActualWeight) + "</td>");
                sb.Append("<td width='11%' valign='bottom'>" + obj.lstCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.FobValue) + "</td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='10%'></td></tr>");

                sb.Append("<tr><th width='8%' style='border-bottom: 1px solid #000;'>Sub Count :</th>");
                sb.Append("<td colspan='11' style='border-bottom: 1px solid #000;'>" + obj.lstCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Count() + "</td></tr>");

                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");
            });*/
            //LOOP End//

            //LOOP Start//Short Cargo Details
            obj.lstShortCartingDetails.Select(a => new { ShippingLineId = a.ShippingLineId, ShippingLineName = a.ShippingLineName, SLA = a.SLA }).Distinct().ToList().ForEach(item =>
            {
                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' cellpadding='10' style='font-size:8pt; width:100%;'><tbody>");
                sb.Append("<tr><th width='8%'>Sla</th><th width='8%'>" + item.SLA + "</th><th colspan='10'>" + item.ShippingLineName + "</th></tr>");

                sb.Append("<tr><th width='8%' style='border-bottom: 1px solid #000;'>Entry No</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Sb No</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Sb Date</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Exporter</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Cargo</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>No PKG</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Gr Wt</th>");
                sb.Append("<th width='11%' style='border-bottom: 1px solid #000;'>Fob</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Slot</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>G/R</th>");
                sb.Append("<th width='8%' style='border-bottom: 1px solid #000;'>Area</th>");
                sb.Append("<th width='10%' style='border-bottom: 1px solid #000;'>Remarks</th></tr>");

                //LOOP Start//
                obj.lstShortCartingDetails.Where(m => m.ShippingLineId == item.ShippingLineId).ToList().ForEach(elem =>
                {
                    sb.Append("<tr><td width='8%' style='border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.ExporterName + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.CargoDescription + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ActualQty + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.ActualWeight + "</td>");
                    sb.Append("<td width='11%' style='border-bottom: 1px solid #000;'>" + elem.FobValue + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.Slot + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.GR + "</td>");
                    sb.Append("<td width='8%' style='border-bottom: 1px solid #000;'>" + elem.Area + "</td>");
                    sb.Append("<td width='10%' style='border-bottom: 1px solid #000;'>" + elem.Remarks + "</td></tr>");
                });
                //LOOP End//

                sb.Append("<tr><th width='8%' valign='bottom'>Sub Total :</th>");
                sb.Append("<td width='8%' valign='bottom'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='11%'></td>");
                sb.Append("<td width='11%'></td>");
                sb.Append("<td width='8%' valign='bottom'>" + obj.lstShortCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.ActualQty) + "</td>");
                sb.Append("<td width='8%' valign='bottom'>" + obj.lstShortCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.ActualWeight) + "</td>");
                sb.Append("<td width='11%' valign='bottom'>" + obj.lstShortCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Sum(y => y.FobValue) + "</td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='8%'></td>");
                sb.Append("<td width='10%'></td></tr>");

                sb.Append("<tr><th width='8%' style='border-bottom: 1px solid #000;'>Sub Count :</th>");
                sb.Append("<td colspan='11' style='border-bottom: 1px solid #000;'>" + obj.lstShortCartingDetails.Where(x => x.ShippingLineId == item.ShippingLineId).Count() + "</td></tr>");

                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");
            });
            //LOOP End//

            sb.Append("<tr cellpadding='10'><th width='8%' valign='bottom' style='font-size:9pt;'>Grand :</th>");
            sb.Append("<th width='8%' valign='bottom' style='font-size:9pt;'>" + obj.lstCartingDetails.Count() + "</th>");
            sb.Append("<th width='8%'></th>");
            sb.Append("<th width='11%'></th>");
            sb.Append("<th width='11%'></th>");
            sb.Append("<th width='8%' valign='bottom' style='font-size:9pt;'>" + (/*obj.lstCartingDetails.Sum(m => m.ActualQty) +*/ obj.lstShortCartingDetails.Sum(x => x.ActualQty)) + "</th>");
            sb.Append("<th width='8%' valign='bottom' style='font-size:9pt;'>" + (/*obj.lstCartingDetails.Sum(m => m.ActualWeight) +*/ obj.lstShortCartingDetails.Sum(x => x.ActualWeight)) + "</th>");
            sb.Append("<th width='11%' valign='bottom' style='font-size:9pt;'>" + (/*obj.lstCartingDetails.Sum(m => m.FobValue) +*/ obj.lstShortCartingDetails.Sum(x => x.FobValue)) + "</th>");
            sb.Append("<th width='8%'></th>");
            sb.Append("<th width='8%'></th>");
            sb.Append("<th width='8%'></th>");
            sb.Append("<th width='10%'></th></tr>");
            sb.Append("</tbody></table>");

            //CARGO SHIFTING DETAIL//
            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:8pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th colspan='15' style='text-align:center; font-size:10pt;'>CARGO SHIFTING DETAIL</th></tr>");
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Entry No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb Date</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Exporter</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Cargo</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>No PKG</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Gr Wt</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Fob</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Slot</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>G/R</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Area</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>F-Shed</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>T-Shed</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>F-Sla</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000;'>T-Sla</th></tr>");
            //LOOP Start//
            obj.lstCargoShifting.ToList().ForEach(item =>
            {
                sb.Append("<tr><td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CFSCode + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillNo + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillDate + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ExporterName + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CargoDescription + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ActualQty + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ActualWeight + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FobValue + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000;border-right: 1px solid #000;'>" + item.LocationName + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.GR + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.SQM + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FromGodown + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ToGodown + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FromShippingLine + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000;'>" + item.ToShippingLine + "</td></tr>");
            });
            //LOOP End//
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Total :</th>");
            sb.Append("<th colspan='4' width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoShifting.Count() + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoShifting.Sum(y => y.ActualQty) + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoShifting.Sum(y => y.ActualWeight) + "</th>");
            sb.Append("<th colspan='8' width='11%' style='border-top: 1px solid #000;'>" + obj.lstCargoShifting.Sum(y => y.FobValue) + "</th></tr>");
            sb.Append("</tbody></table>");


            //CARGO ACCEPTING DETAIL//
            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:8pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th colspan='15' style='text-align:center; font-size:10pt;'>CARGO ACCEPTING DETAIL</th></tr>");
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Entry No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb Date</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Exporter</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Cargo</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>No PKG</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Gr Wt</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Fob</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Slot</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>G/R</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Area</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>F-Shed</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>T-Shed</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>F-Sla</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000;'>T-Sla</th></tr>");
            //LOOP Start//
            obj.lstCargoAccepting.ToList().ForEach(item =>
            {
                sb.Append("<tr><td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CFSCode + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillNo + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillDate + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ExporterName + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CargoDescription + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ActualQty + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ActualWeight + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FobValue + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.LocationName + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.GR + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.SQM + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FromGodown + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ToGodown + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.FromShippingLine + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000;'>" + item.ToShippingLine + "</td></tr>");
            });
            //LOOP End//
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Total :</th>");
            sb.Append("<th colspan='4' width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoAccepting.Count() + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoAccepting.Sum(y => y.ActualQty) + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstCargoAccepting.Sum(y => y.ActualWeight) + "</th>");
            sb.Append("<th colspan='8' width='11%' style='border-top: 1px solid #000;'>" + obj.lstCargoAccepting.Sum(y => y.FobValue) + "</th></tr>");
            sb.Append("</tbody></table>");


            //BACK TO TOWN DETAIL//
            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:8pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th colspan='11' style='text-align:center; font-size:10pt;'>BACK TO TOWN DETAIL</th></tr>");
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Entry No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Sb Date</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Exporter</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Cargo</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>No PKG</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Gr Wt</th>");
            sb.Append("<th width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Fob</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Slot</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>G/R</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000;'>Area</th></tr>");
            //LOOP Start//
            obj.lstBTTDetails.ToList().ForEach(item =>
            {
                sb.Append("<tr><td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CFSCode + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillNo + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillDate + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ExporterName + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CargoDescription + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.BTTQuantity + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.BTTWeight + "</td>");
                sb.Append("<td width='11%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.Fob + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.LocationName + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.GR + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000;'>Dynamic</td></tr>");
            });
            //LOOP End//
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Total :</th>");
            sb.Append("<th width='8%' colspan='4' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstBTTDetails.Count() + "</th>");
            sb.Append("<th  width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstBTTDetails.Sum(a => a.BTTQuantity) + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstBTTDetails.Sum(a => a.BTTWeight) + "</th>");
            sb.Append("<th width='8%' colspan='4' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstBTTDetails.Sum(a => a.Fob) + "</th></tr>");
            //sb.Append("<th colspan='4' width='11%' style='border-top: 1px solid #000;'>Dynamic</th>");
            sb.Append("</tbody></table>");


            //STUFFING DETAIL//
            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:8pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th colspan='11' style='text-align:center; font-size:10pt;'>STUFFING DETAIL</th></tr>");
            sb.Append("<tr><th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>S.R. No</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>S.R. Date</th>");
            sb.Append("<th width='9%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>ICD Code</th>");
            sb.Append("<th width='9%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Container No</th>");
            sb.Append("<th width='5%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Size</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>SLA</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>In Date</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>No SB</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>No of Unit</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Gr Wt</th>");
            sb.Append("<th width='12%' style='border-top: 1px solid #000;'>FOB</th></tr>");
            //LOOP Start//
            obj.lstStuffingDetails.ToList().ForEach(item =>
            {
                sb.Append("<tr><td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.StuffingNo + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.StuffingDate + "</td>");
                sb.Append("<td width='9%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CFSCode + "</td>");
                sb.Append("<td width='9%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ContainerNo + "</td>");
                sb.Append("<td width='5%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.Size + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.SLA + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.EntryDateTime + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingBillNo + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.StuffQuantity + "</td>");
                sb.Append("<td width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.StuffWeight + "</td>");
                sb.Append("<td width='12%' style='border-top: 1px solid #000;'>" + item.Fob + "</td></tr>");
            });

            //LOOP End//
            sb.Append("<tr><th colspan='7' width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>Total :</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstStuffingDetails.Sum(z => z.ShippingBillNo) + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstStuffingDetails.Sum(z => z.StuffQuantity) + "</th>");
            sb.Append("<th width='8%' style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + obj.lstStuffingDetails.Sum(z => z.StuffWeight) + "</th>");
            sb.Append("<th width='12%' style='border-top: 1px solid #000;'>" + obj.lstStuffingDetails.Sum(z => z.Fob) + "</th></tr>");
            sb.Append("</tbody></table>");


            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:8pt;width:100%;'><tbody><tr>");
            sb.Append("<td colspan='6' width='60%' valign='top'>");
            sb.Append("<table cellspacing='0' cellpadding='10' style='font-size:8pt; width:100%;'><tbody>");
            sb.Append("<tr><th colspan='5' style='text-align:center; font-size:10pt;'>SUMMARY</th></tr>");
            sb.Append("<tr><th width='10%' style='border: 1px solid #000; border-right: 0;'>DESC</th>");
            sb.Append("<th width='8%' style='border: 1px solid #000;'>SBS</th>");
            sb.Append("<th width='9%' style='border: 1px solid #000; border-left: 0;'>PKGS</th>");
            sb.Append("<th width='9%' style='border: 1px solid #000; border-left: 0;'>WT</th>");
            sb.Append("<th width='12%' style='border: 1px solid #000; border-left: 0;'>FOB</th></tr>");
            //LOOP Start//
            sb.Append("<tr><td width='10%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>CARGO CARTED</td>");
            sb.Append("<td width='8%' style='border: 1px solid #000; border-top: 0;'>" + obj.lstCartingDetails.Count() + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + (/*obj.lstCartingDetails.Sum(x => x.ActualQty) +*/ obj.lstShortCartingDetails.Sum(x => x.ActualQty)) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + (/*obj.lstCartingDetails.Sum(x => x.ActualWeight) +*/ obj.lstShortCartingDetails.Sum(x => x.ActualWeight)) + "</td>");
            sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + (/*obj.lstCartingDetails.Sum(x => x.FobValue) +*/ obj.lstShortCartingDetails.Sum(x => x.FobValue)) + "</td></tr>");

            sb.Append("<tr><td width='10%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>CARGO SHIFTED</td>");
            sb.Append("<td width='8%' style='border: 1px solid #000; border-top: 0;'>" + obj.lstCargoShifting.Count() + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoShifting.Sum(x => x.ActualQty) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoShifting.Sum(x => x.ActualWeight) + "</td>");
            sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoShifting.Sum(x => x.FobValue) + "</td></tr>");

            sb.Append("<tr><td width='10%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>CARGO ACCEPTED</td>");
            sb.Append("<td width='8%' style='border: 1px solid #000; border-top: 0;'>" + obj.lstCargoAccepting.Count() + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoAccepting.Sum(x => x.ActualQty) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoAccepting.Sum(x => x.ActualWeight) + "</td>");
            sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstCargoAccepting.Sum(x => x.FobValue) + "</td></tr>");

            sb.Append("<tr><td width='10%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>CARGO BTT</td>");
            sb.Append("<td width='8%' style='border: 1px solid #000; border-top: 0;'>" + obj.lstBTTDetails.Count() + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstBTTDetails.Sum(x => x.BTTQuantity) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstBTTDetails.Sum(x => x.BTTWeight) + "</td>");
            sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstBTTDetails.Sum(x => x.BTTQuantity) + "</td></tr>");

            sb.Append("<tr><td width='10%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>CARGO STUFFED</td>");
            sb.Append("<td width='8%' style='border: 1px solid #000; border-top: 0;'>" + obj.lstStuffingDetails.Sum(x => x.ShippingBillNo) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstStuffingDetails.Sum(x => x.StuffQuantity) + "</td>");
            sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstStuffingDetails.Sum(x => x.StuffWeight) + "</td>");
            sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.lstStuffingDetails.Sum(x => x.Fob) + "</td></tr>");
            //LOOP End//
            sb.Append("</tbody></table>");
            sb.Append("</td>");

            //sb.Append("<td colspan='6' width='40%'>");
            //sb.Append("<table cellspacing='0' cellpadding='10' style='font-size:8pt; width:100%;'><tbody>");
            //sb.Append("<tr><th colspan='4' style='text-align:center; font-size:10pt;'>OPENING BALANCE</th></tr>");
            //sb.Append("<tr><th width='8%' style='border: 1px solid #000;border-right: 0;'>SBS</th>");
            //sb.Append("<th width='9%' style='border: 1px solid #000;'>PKGS</th>");
            //sb.Append("<th width='9%' style='border: 1px solid #000; border-left: 0;'>WT</th>");
            //sb.Append("<th width='12%' style='border: 1px solid #000; border-left: 0;'>FOB</th></tr>");
            ////LOOP Start//
            //sb.Append("<tr><td width='8%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>" + obj.StockOpening.Count() + "</td>");
            //sb.Append("<td width='9%' style='border: 1px solid #000; border-top: 0;'>" + obj.StockOpening.Sum(x => x.Units) + "</td>");
            //sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.StockOpening.Sum(x => x.Weight) + "</td>");
            //sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.StockOpening.Sum(x => x.Fob) + "</td></tr>");
            ////LOOP End//

            //sb.Append("<tr><th colspan='4' style='text-align:center; font-size:10pt;'>CLOSING BALANCE</th></tr>");
            //sb.Append("<tr><th width='8%' style='border: 1px solid #000;border-right: 0;'>SBS</th>");
            //sb.Append("<th width='9%' style='border: 1px solid #000;'>PKGS</th>");
            //sb.Append("<th width='9%' style='border: 1px solid #000; border-left: 0;'>WT</th>");
            //sb.Append("<th width='12%' style='border: 1px solid #000; border-left: 0;'>FOB</th></tr>");

            ////LOOP Start//
            //sb.Append("<tr><td width='8%' style='border: 1px solid #000; border-right: 0; border-top: 0;'>" + obj.StockClosing.Count() + "</td>");
            //sb.Append("<td width='9%' style='border: 1px solid #000; border-top: 0;'>" + obj.StockClosing.Sum(x => x.Units) + "</td>");
            //sb.Append("<td width='9%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.StockClosing.Sum(x => x.Weight) + "</td>");
            //sb.Append("<td width='12%' style='border: 1px solid #000; border-left: 0; border-top: 0;'>" + obj.StockClosing.Sum(x => x.Fob) + "</td></tr>");
            ////LOOP End//
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr></tbody></table>");

            sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            var FileName = "DTR" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 40f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, sb.ToString());
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        public string GeneratingPDFforDTRImpFCL(DataSet ds, string dt)
        {
            DateTime date = DateTime.Parse(dt);
            List<dynamic> lstCont = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstCount = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);

            List<string> lstSB = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
            sb.Append("<tr><td colspan='12'>");
            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            sb.Append("<td width='200%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:14px;'>" + ZonalOffice + "</span><br/><label style='font-size: 16px; font-weight:bold;'>Daily Transaction Report of FCL Containers(Import)</label></td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td colspan='12' style='text-align:right; font-size:14px;'><b>Date :</b> " + date.ToString("dd/MM/yyyy") + "</td></tr>");
            sb.Append("</thead></table>");

            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; font-size:9pt; border:1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th width='5%' style='border-right: 1px solid #000;'>S.No.</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>ICD Code</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Container In Date</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Container No.</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Size</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Shipping Line</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>FD/DD</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>No. of OBLs</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>No. of Package</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Weight</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Gatepass No.</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>Importer</th>");
            sb.Append("<th width='10%' style='border-right: 1px solid #000;'>CHA Name</th>");
            sb.Append("<th width='10%'>STATUS(EXAM/RMS EXAM/RMS)</th></tr>");

            int k = 1;
            //LOOP Start//
            lstCont.ForEach(item =>
            {
                sb.Append("<tr><td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + k + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CFSCode + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.InDate + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ContainerNo + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.Size + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.ShippingLine + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.DDFD + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.NoOfOBL + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.NoOfPkg + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.GrWt + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.GatePassNo + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.Importer + "</td>");
                sb.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000;'>" + item.CHA + "</td>");
                sb.Append("<td style='border-top: 1px solid #000;'>" + item.RMS + "</td></tr>");
                k++;
            });
            //LOOP End//
            sb.Append("</tbody></table>");

            int Total20FD = Convert.ToInt32(lstCount[0].Total20FD);
            int Total40FD = Convert.ToInt32(lstCount[0].Total40FD);
            int FDTues = Convert.ToInt32((lstCount[0].Total20FD) + ((lstCount[0].Total40FD) * 2));
            int Total20DD = Convert.ToInt32(lstCount[0].Total20DD);
            int Total40DD = Convert.ToInt32(lstCount[0].Total40DD);
            int DDTues = Convert.ToInt32((lstCount[0].Total20DD) + ((lstCount[0].Total40DD) * 2));

            sb.Append("<table cellspacing='0' cellpadding='10' style='margin-top:20px; width:100%; font-family: Arial, Helvetica, sans-serif;'><tbody>");
            sb.Append("<tr><th colspan='12' style='font-size:14px;'>SUMMARY:</th></tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='6' style='width:50%;'><table cellspacing='0' cellpadding='10' style='width:100%; font-size:8pt; border:1px solid #000;'><tbody>");
            sb.Append("<tr><td colspan='6' style='width:50%;' style='border-right: 1px solid #000;'><b>20'FD CONTAINERS :</b> <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + Total20FD + " </td> <td colspan='6' style='width:50%;'><b>40'FD CONTAINERS :</b> <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + Total40FD + " </td></tr>");
            sb.Append("<tr><th colspan='12' style='font-size:8pt; border-top: 1px solid #000;'>TOTAL FD TEUs :<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + FDTues + "</th></tr>");
            sb.Append("</tbody></table></td>");

            sb.Append("<td colspan='6' style='width:50%;'><table cellspacing='0' cellpadding='10' style='width:100%; font-size:8pt; border:1px solid #000;'><tbody>");
            sb.Append("<tr><td colspan='6' style='width:50%;' style='border-right: 1px solid #000;'><b>20'DD CONTAINERS :</b> <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + Total20DD + " </td> <td colspan='6' style='width:50%;'><b>40'DD CONTAINERS :</b> <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + Total40DD + " </td></tr>");
            sb.Append("<tr><th colspan='12' style='font-size:8pt; border-top: 1px solid #000;'>TOTAL DD TEUs :<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + DDTues + "</th></tr>");
            sb.Append("</tbody></table></td>");
            sb.Append("</tr>");
            sb.Append("</tbody></table>");
            sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            lstSB.Add(sb.ToString());

            var FileName = "DTR" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/
            using (var rp = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 40f, 20f, 20f, false, true))
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

        #region Consuldate register of outward supply

        [HttpGet]
        public ActionResult ConslRegisterOfOutwardSupply()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult ConslRegisterOfOutwardSupply(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                //var Type = fc["ddlType"].ToString();
                var Type = "INV";
                var excelName = "";
                var ObjRR = new LNSM_ReportRepository();
                //var ObjRR = new Ppg_ReportRepository();
                ObjRR.ConsolGetRegisterofOutwardSupply(date1, date2);

                //if (Type == "Inv") { Type = "Invoice"; }
                //if (Type == "C") { Type = "Credit"; }
                //if (Type == "D") { Type = "Debit"; }
                //if (Type == "Unpaid") { Type = "Unpaid"; }
                //if (Type == "CancelInv") { Type = "Cancel Invoice"; }

                excelName = "ConslRegisterofOutwardSupply" + "_" + Type + ".xlsx";

                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
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
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ConslRegisterofOutwardSupply.xlsx");
                }
            }
            // return null;
        }

        #endregion

        #region Ageing Report
        public ActionResult AgeingReport()
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GetAllPartyForAgeing("", 0);
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
        public JsonResult SearchPartyForAgeing(string PartyCode)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GetAllPartyForAgeing(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyForAgeing(string PartyCode, int Page)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.GetAllPartyForAgeing(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult GetAgeingStatement(LNSM_AgeingDetails ageingDetails)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            List<WFLD_PartyLedCons> LstPdSummary = new List<WFLD_PartyLedCons>();
            var excelName = "";
            try
            {

                Login objLogin = (Login)Session["LoginUser"];
                ObjRR.AgeingStatement(ageingDetails.AgeingDate, ageingDetails.PartyId);
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string varExcelFile = GeneratePDFLclDailyReport(ds);
                excelName = "AgeingStatement" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xls";

                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/Excel")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/Excel"));
                }
                var excelFile = Server.MapPath("~/Docs/Excel/" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".xlsx");
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                    {
                        System.IO.StreamWriter writer = System.IO.File.AppendText(excelFile);

                        writer.WriteLine(varExcelFile);
                        writer.Close();
                    }
                }

                return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            catch
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xls");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PartyLedgerConsolidate.xls");
                }
            }

        }



        [NonAction]
        public string GeneratePDFLclDailyReport(DataSet ds)
        {
            try
            {

                var FileName = "LCLDELIVERYReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                List<dynamic> PartyList = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
                List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; margin-bottom:10px; border-collapse:collapse; border:1px solid #000;  text-align: center; font-size:7pt;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 3%;'>Customer Code</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>Customer name</ th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Credit Term</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>Sales Team</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Sales Team Head</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>CCT Team</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Invoice Number</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Invoice Date</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>Due date</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Not Due</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>0 to 30</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>31 to 60</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>61 to 90</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 7%;'>91 to 180</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 7%;'>181 to 365</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>&lt; 365</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 7%;'>Total Amount</th>");

                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");
                decimal TotalBetween0to30 = 0M;
                decimal TotalBetween31to60 = 0M;
                decimal TotalBetween61to90 = 0M;
                decimal TotalBetween91to180 = 0M;
                decimal TotalBetween181to365 = 0M;
                decimal TotalGet365 = 0M;
                decimal TotalTotalAmount = 0M;



                PartyList.ForEach(y =>
                {

                    decimal Between0to30 = 0M;
                    decimal Between31to60 = 0M;
                    decimal Between61to90 = 0M;
                    decimal Between91to180 = 0M;
                    decimal Between181to365 = 0M;
                    decimal Get365 = 0M;
                    decimal TotalAmount = 0M;




                    lstInvoice.Where(x => x.EximTraderId == y.EximTraderId).ToList().ForEach(item =>
                    {
                        Pages.Append("<tr>");

                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.CustomerCode + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Customername + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.CreditTerm + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.SalesTeam + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.SalesTeamHead + " </td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.CCTTeam + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.InvoiceNumber + " </td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.InvoiceDate + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Duedate + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.NotDue + " </td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Between0to30 + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Between31to60 + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Between61to90 + " </td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Between91to180 + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Between181to365 + " </td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Get365 + "</td>");
                        Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.TotalAmount + "</td>");
                        Pages.Append("</tr>");

                        Between0to30 = Between0to30 + item.Between0to30;
                        Between31to60 = Between31to60 + item.Between31to60;
                        Between61to90 = Between61to90 + item.Between61to90;
                        Between91to180 = Between91to180 + item.Between91to180;
                        Between181to365 = Between181to365 + item.Between181to365;
                        Get365 = Get365 + item.Get365;
                        TotalAmount = TotalAmount + item.TotalAmount;




                        TotalBetween0to30 = TotalBetween0to30 + item.Between0to30;
                        TotalBetween31to60 = TotalBetween31to60 + item.Between31to60;
                        TotalBetween61to90 = TotalBetween61to90 + item.Between61to90;
                        TotalBetween91to180 = TotalBetween91to180 + item.Between91to180;
                        TotalBetween181to365 = TotalBetween181to365 + item.Between181to365;
                        TotalGet365 = TotalGet365 + item.Get365;
                        TotalTotalAmount = TotalTotalAmount + item.TotalAmount;




                    });
                    Pages.Append("<tr>");

                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>Total</th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'></th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Between0to30 + "</th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Between31to60 + "</th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Between61to90 + " </th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Between91to180 + "</th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Between181to365 + " </th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + Get365 + "</th>");
                    Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;font-weight: bold;'>" + TotalAmount + "</th>");
                    Pages.Append("</tr>");

                });
                Pages.Append("<tr>");

                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Grand Total</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalBetween0to30 + "</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalBetween31to60 + "</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalBetween61to90 + " </th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalBetween91to180 + "</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalBetween181to365 + " </th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalGet365 + "</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + TotalTotalAmount + "</th>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                return Pages.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion



        #region Credit Note 
        [HttpGet]
        public ActionResult CreditNote()
        {

            LNSM_ReportRepository objParty = new LNSM_ReportRepository();
            //objParty.GetInvPaymentParty();
            //if (objParty.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objParty.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetAllCreditNote(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {

            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_CreditNote> lstInv = new List<LNSM_CreditNote>();
            ObjGR.GetAllCreditNote(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_CreditNote>)ObjGR.DBResponse.Data;
            return PartialView("ListOfCreditNote", lstInv);

        }
        [HttpGet]
        public ActionResult GetLoadMoreCreditNote(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_CreditNote> lstInv = new List<LNSM_CreditNote>();
            ObjGR.GetAllCreditNote(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_CreditNote>)ObjGR.DBResponse.Data;
            return Json(lstInv, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetCreditNotePrint(string CRNoteId)
        {
            LGS_ReportRepository objRepo = new LGS_ReportRepository();
            PrintPDFModelOfCr objCR = new PrintPDFModelOfCr();
            objRepo.PrintPDFForCRNote(CRNoteId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintPDFModelOfCr)objRepo.DBResponse.Data;
                string Path = GenerateCRNotePDF(objCR, Convert.ToInt32(CRNoteId));
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }


        public string GenerateCRNotePDF(PrintPDFModelOfCr objCR, int CRNoteId)
        {
            string Note = "C";
            UpiQRCodeInfoCR upiQRInfo = new UpiQRCodeInfoCR();
            upiQRInfo.ver = objCR.ver;

            upiQRInfo.tn = objCR.tn;
            upiQRInfo.tier = objCR.tier;
            upiQRInfo.tid = objCR.tid;
            upiQRInfo.qrMedium = objCR.qrMedium;
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCR.pn;
            upiQRInfo.pa = objCR.pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCR.mtid;
            upiQRInfo.msid = objCR.msid;
            upiQRInfo.mode = objCR.mode;
            upiQRInfo.mid = objCR.mid;
            upiQRInfo.mc = objCR.mc;
            upiQRInfo.gstIn = objCR.gstIn;

            upiQRInfo.InvoiceDate = Convert.ToString(objCR.QRInvoiceDate);
            upiQRInfo.invoiceNo = Convert.ToString(objCR.CRNoteNo);
            upiQRInfo.InvoiceName = Convert.ToString(objCR.PartyName);
            upiQRInfo.mam = Convert.ToDecimal(objCR.mam);
            upiQRInfo.am = Convert.ToDecimal(objCR.am);
            upiQRInfo.CGST = Convert.ToDecimal(objCR.CGST);
            upiQRInfo.SGST = Convert.ToDecimal(objCR.SGST);
            upiQRInfo.IGST = Convert.ToDecimal(objCR.IGST);
            upiQRInfo.GSTPCT = Convert.ToDecimal(objCR.GSTPCT);
            upiQRInfo.QRexpire = Convert.ToString(objCR.QRInvoiceDate);
            upiQRInfo.tr = Convert.ToString(objCR.CrNoteId);

            //Einvoice Eobj = new Einvoice();
            //B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
            //objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
            //IrnResponse objERes = new IrnResponse();
            //objERes.SignedQRCode = objresponse.QrCodeBase64;
            //objERes.SignedInvoice = objresponse.QrCodeJson;
            //objERes.SignedQRCode = objresponse.QrCodeJson;

            //string SACCode = "", note = "", fileName = "";
            //objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            //{
            //    if (SACCode == "")
            //        SACCode = item.SACCode;
            //    else
            //        SACCode = SACCode + "," + item.SACCode;
            //});
            //note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
            //fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
            //string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
            //if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            //{
            //    Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            //}
            //if (System.IO.File.Exists(Path + "/" + fileName))
            //{
            //    System.IO.File.Delete(Path + "/" + fileName);
            //}



            //string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Place Of Supply:<span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
            //string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
            //string tr = "";
            //int Count = 1;
            //decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
            //objCR.lstCharges.ToList().ForEach(item =>
            //{
            //    tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
            //    IGSTAmt += item.IGSTAmt;
            //    CGSTAmt += item.CGSTAmt;
            //    SGSTAmt += item.SGSTAmt;
            //    Count++;
            //});
            //string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            //string Remarks = objCR.Remarks;
            //string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
            //html = html + htmltable + tr + tfoot;
            //using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            //{
            //    RH.GeneratePDF(Path + "/" + fileName, html);
            //}
            //return "/Docs/" + Session.SessionID + "/" + fileName;


            string SACCode = "", note = "", fileName = "";
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("");
            Einvoice obj = new Einvoice();
            objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            {
                if (SACCode == "")
                    SACCode = item.SACCode;
                else
                    SACCode = SACCode + "," + item.SACCode;
            });
            note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
            fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
            string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path + "/" + fileName))
            {
                System.IO.File.Delete(Path + "/" + fileName);
            }
            //string html = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td><td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br /><span style='font-size: 7pt; padding-bottom: 10px;'>107-109 , EPIP Zone , KIADB Industrial Area <br/> Whitefield , Bengaluru - 560066 </span> <br/><span style='font-size: 7pt; padding-bottom: 10px;'>Email - cwcwfdcfs@gmail.com</span><br /><label style='font-size: 7pt; font-weight:bold;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span></label><br/><label style='font-size: 7pt; font-weight:bold;'>" + note + "</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr>     <tr><th colspan='6' style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th colspan='6' style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Name:</b> " + objCR.CompanyName + "</td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Name:</b> <span>" + objCR.PartyName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Warehouse Address:</b> <span>" + objCR.CompanyAddress + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Address:</b> <span>" + objCR.PartyAddress + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>City:</b> <span>" + objCR.CompCityName + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>City:</b> <span>" + objCR.PartyCityName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State:</b> <span>" + objCR.CompStateName + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State:</b> <span>" + objCR.PartyStateName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State Code:</b> <span>" + objCR.CompStateCode + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State Code:</b> <span>" + objCR.PartyStateCode + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>GSTIN:</b> <span>" + objCR.CompGstIn + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><span><b>GSTIN(if registered):</b>" + objCR.PartyGSTIN + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>PAN:</b><span>" + objCR.CompPan + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Place Of Supply:</b><span>" + objCR.PartyStateName + "</span></td></tr> <tr><td colspan='6' style='text-align:left;padding:8px;'><b>Debit/Credit Note Serial No:</b> <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/><b>Date of Issue:</b> <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td colspan='6' style='text-align:left;padding:8px;'><b>Accounting Code of</b> <span>" + SACCode + "</span><br/><br/><b>Description of Services:</b> <span>Other Storage & Warehousing Services</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Original Bill of Supply/Tax Invoice No:</b> <span>" + objCR.InvoiceNo + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><span><b>Date:</b>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr>";


            html.Append("<table style='width: 100%; font-size: 7pt; font-family: Verdana, Arial, San-serif; border-collapse: collapse;'>");
            html.Append("<thead>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

            html.Append("<tr>");

            html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td valign='top'><img align='right' src='IMGSRC' width='80'/></td>");
            html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 16px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 9pt;'>(A Govt. of India Undertaking) </label><br /><span style='font-size: 9pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span><br/><label style='font-size: 9pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'><u>" + ZonalOffice + "</u></span></label><br /><label style='font-size: 9pt; font-weight: bold;'>" + note + "</label></td></tr>");
            //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td></tr>");            
            html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + objCR.irn + " </td></tr>");
            html.Append("</tbody></table></td>");


            if (objCR.SignedQRCode == "")
            { }
            else
            {
                //if (objCR.SupplyType == "B2C")
                //{
                //    //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");
                //    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                //    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                //    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                //    html.Append("</tbody></table></td>");

                //}
                //else
                //{
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");

                //}
            }




            html.Append("</tr>");

            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            //html += "<tr>";
            //html += "<td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90' /></td>";
            //html += "<td colspan='8' width='90%' valign='top' align='center'>";
            //html += "<h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>";
            //html += "<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />";
            //html += "<span style='font-size: 7pt; padding-bottom: 10px;'>";
            //html += "107-109 , EPIP Zone , KIADB Industrial Area <br />";
            //html += "Whitefield , Bengaluru - 560066";
            //html += "</span>";
            //html += "<br />";
            //html += "<span style='font-size: 7pt; padding-bottom: 10px;'>Email - cwcwfdcfs@gmail.com</span><br />";
            //html += "<label style='font-size: 7pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br />";
            //html += "<label style='font-size: 7pt; font-weight: bold;'>' + note + '</label>";
            //html += "</td>";
            //html += "<td valign='top'><img align='right' src='ISO' width='100' /></td>";
            //html += "</tr>";

            html.Append("<tr>");
            html.Append("<th colspan='6' style='border: 1px solid #000; text-align: center; padding: 8px; width: 50%;'>Details of Service Provider</th>");
            html.Append("<th colspan='6' style='border: 1px solid #000; text-align: center; padding: 8px; width: 50%;'>Details of Service Receiver</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Name:</b> " + objCR.CompanyName + "</td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Name:</b> <span>" + objCR.PartyName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Warehouse Address:</b> <span>" + objCR.CompanyAddress + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Address:</b> <span>" + objCR.PartyAddress + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>City:</b> <span>" + objCR.CompCityName + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>City:</b> <span>" + objCR.PartyCityName + "</span></td>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State:</b> <span>" + objCR.CompStateName + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State:</b> <span>" + objCR.PartyStateName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State Code:</b> <span>" + objCR.CompStateCode + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State Code:</b> <span>" + objCR.PartyStateCode + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>GSTIN:</b> <span>" + objCR.CompGstIn + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'>");
            html.Append("<span><b>GSTIN(if registered):</b>" + objCR.PartyGSTIN + "</span>");
            html.Append("</td>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>PAN:</b><span>" + objCR.CompPan + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Place Of Supply:</b><span>" + objCR.PartyStateName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            html.Append("<b>Debit/Credit Note Serial No:</b> <span style='border-bottom: 1px solid #000;'>" + objCR.CRNoteNo + "</span><br />");
            html.Append("<br />");
            html.Append("<b>Date of Issue:</b> <span style='border-bottom: 1px solid #000;'>" + objCR.CRNoteDate + "</span>");
            html.Append("</td>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            //html.Append("<b>Accounting Code of</b> <span>" + SACCode + "</span><br />");
            //html.Append("<br />");
            //html.Append("<b>Description of Services:</b> <span>Other Storage & Warehousing Services</span>");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'><b>Original Bill of Supply/Tax Invoice No:</b> <span>" + objCR.InvoiceNo + "</span></td>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            html.Append("<span><b>Date:</b>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span>");
            html.Append("</td>");
            html.Append("</tr>");
            //html.Append("</tbody>");
            //html.Append("</table>");

            html.Append("<tr>");
            html.Append("<td colspan='12'>");
            html.Append("<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'>");
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>HSN</th>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            //string tr = "";
            int Count = 1;
            decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0, Taxable = 0;
            objCR.lstCharges.ToList().ForEach(item =>
            {
                html.Append("<tr>");
                html.Append("<td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.SACCode + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td>");
                html.Append("</tr>");

                IGSTAmt += item.IGSTAmt;
                CGSTAmt += item.CGSTAmt;
                SGSTAmt += item.SGSTAmt;
                Taxable += item.Taxable;
                Count++;
            });
            string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            string Remarks = objCR.Remarks;
            string PayeeName = objCR.PayeeName;
            html.Append("<tr>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:left;padding:5px;'>Total</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;'>" + Taxable + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</th>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<th colspan='4' style='border:1px solid #000;border-right:0;text-align:left;padding:5px;'>Round Up</th>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<th style='border:1px solid #000;border-left:0;text-align:right;padding:5px;'>" + objCR.RoundUp + "</th>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<th colspan='4' style='border:1px solid #000;border-right:0;text-align:left;padding:5px;'>Total Debit/Credit Note Value (in figure)</th>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<th style='border:1px solid #000;border-left:0;text-align:right;padding:5px;'>" + objCR.GrandTotal + "</th>");
            html.Append("</tr>");

            //tfoot +="<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr>";
            html.Append("<tr><td colspan='11' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr>");
            html.Append("<tr><td colspan='11' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr>");
            html.Append("<tr><td colspan='11' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Payer Name:</span> <span>" + PayeeName + "</span></td></tr>");
            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</td>");
            html.Append("</tr>");

            //html.Append("<tr>");
            //html.Append("<td colspan='12' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/>");
            //html.Append("<span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span>");
            //html.Append("</td>");
            //html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td colspan='12' style='text-align:right;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td>");
            html.Append("</tr>");

            //html.Append("<tr>");
            //html.Append("<td colspan='12' style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td>");
            //html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td align='left' valign='top' colspan='12' width='100%'><p style='font-size:7pt; font-weight:bold;'>This credit note is system generated, hence does not require any sign.</p></td>");
            html.Append("</tr>");


            html.Append("</tbody>");
            html.Append("</table>");

            //html = html + htmltable +tr+ tfoot;

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html = html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(html.ToString());
            using (var RH = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f))
            {
                RH.GeneratePDF(Path + "/" + fileName, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + fileName;
        }

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
            LNSM_ReportRepository objPpgRepo = new LNSM_ReportRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (LNSM_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            try
            {

                var invoiceData = JsonConvert.DeserializeObject<LNSM_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            var result = await GetIRNForBulkInvoice(item.InvoiceNo, item.SupplyType);

                        }
                        if (item.InvoiceType == "C")
                        {
                            var result1 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                        }
                        if (item.InvoiceType == "D")
                        {
                            var result2 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                }
                //foreach (var item in invoiceData.lstOperationContwiseAmt)
                //{
                //    if (item.DocumentDate != "")
                //        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
                //} 
                return Json(new { Status = 1, Message = "IRN Generated" });

            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        public async Task<JsonResult> GetIRNForBulkInvoice(String InvoiceNo, string SupplyType)
        {

            LNSM_ReportRepository objRepo = new LNSM_ReportRepository();
            //objChrgRepo.GetAllCharges();
            LNSM_ReportRepository objPpgRepo = new LNSM_ReportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
                    log.Info("Before calling GenerateEinvoice");
                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    log.Info("after calling GenerateEinvoice");
                    if (ERes.Status == 0)
                    {
                        log.Info(ERes.ErrorDetails.ErrorMessage);
                        log.Info(ERes.ErrorDetails.ErrorCode);
                        log.Info("Invoice No:" + InvoiceNo);
                        log.Info(ERes.Status);
                    }


                    if (ERes.Status == 1)
                    {
                        log.Info("Before calling AddEditIRNB2C");
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
                        log.Info("after calling AddEditIRNB2C");
                    }
                    else
                    {
                        objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                        objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                    }
                }
                else
                {

                    Einvoice Eobj = new Einvoice();
                    objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                    WFLD_IrnB2CDetails irnb2cobj = new WFLD_IrnB2CDetails();
                    irnb2cobj = (WFLD_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                    if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                    {
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                        objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }
                    else
                    {
                        var tn = "GST QR";
                        UpiQRCodeInfo idata = new UpiQRCodeInfo();
                        idata.ver = irnb2cobj.ver;
                        idata.mode = irnb2cobj.mode;
                        idata.mode = irnb2cobj.mode;
                        idata.tr = irnb2cobj.tr;
                        idata.tid = irnb2cobj.tid;
                        idata.tn = tn;
                        idata.pa = irnb2cobj.pa;
                        idata.pn = irnb2cobj.pn;
                        idata.mc = irnb2cobj.mc;
                        idata.am = irnb2cobj.TotInvVal;
                        idata.mam = irnb2cobj.TotInvVal;
                        idata.mid = irnb2cobj.mid;
                        idata.msid = irnb2cobj.msid;
                        idata.orgId = irnb2cobj.orgId;
                        idata.mtid = irnb2cobj.mtid;
                        idata.CESS = irnb2cobj.CESS;
                        idata.CGST = irnb2cobj.CGST;
                        idata.SGST = irnb2cobj.SGST;
                        idata.IGST = irnb2cobj.IGST;
                        idata.GSTIncentive = irnb2cobj.GSTIncentive;
                        idata.GSTPCT = irnb2cobj.GSTPCT;
                        idata.qrMedium = irnb2cobj.qrMedium;
                        idata.invoiceNo = irnb2cobj.DocNo;
                        idata.InvoiceDate = irnb2cobj.DocDt;
                        idata.InvoiceName = irnb2cobj.InvoiceName;
                        idata.QRexpire = irnb2cobj.DocDt;
                        idata.pinCode = irnb2cobj.pinCode;
                        idata.tier = irnb2cobj.tier;
                        idata.gstIn = irnb2cobj.gstIn;
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        objresponse = Eobj.GenerateB2cQRCode(idata);
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }
                }

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                LNSM_IrnB2CDetails irnb2cobj = new LNSM_IrnB2CDetails();
                irnb2cobj = (LNSM_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                    objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }
                else
                {
                    var tn = "GST QR";
                    UpiQRCodeInfo idata = new UpiQRCodeInfo();
                    idata.ver = irnb2cobj.ver;
                    idata.mode = irnb2cobj.mode;
                    idata.mode = irnb2cobj.mode;
                    idata.tr = irnb2cobj.tr;
                    idata.tid = irnb2cobj.tid;
                    idata.tn = tn;
                    idata.pa = irnb2cobj.pa;
                    idata.pn = irnb2cobj.pn;
                    idata.mc = irnb2cobj.mc;
                    idata.am = irnb2cobj.TotInvVal;
                    idata.mam = irnb2cobj.TotInvVal;
                    idata.mid = irnb2cobj.mid;
                    idata.msid = irnb2cobj.msid;
                    idata.orgId = irnb2cobj.orgId;
                    idata.mtid = irnb2cobj.mtid;
                    idata.CESS = irnb2cobj.CESS;
                    idata.CGST = irnb2cobj.CGST;
                    idata.SGST = irnb2cobj.SGST;
                    idata.IGST = irnb2cobj.IGST;
                    idata.GSTIncentive = irnb2cobj.GSTIncentive;
                    idata.GSTPCT = irnb2cobj.GSTPCT;
                    idata.qrMedium = irnb2cobj.qrMedium;
                    idata.invoiceNo = irnb2cobj.DocNo;
                    idata.InvoiceDate = irnb2cobj.DocDt;
                    idata.InvoiceName = irnb2cobj.InvoiceName;
                    idata.QRexpire = irnb2cobj.DocDt;
                    idata.pinCode = irnb2cobj.pinCode;
                    idata.tier = irnb2cobj.tier;
                    idata.gstIn = irnb2cobj.gstIn;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }

            }


            return Json(objPpgRepo.DBResponse);
        }

        public async Task<JsonResult> GetGenerateIRNForBulkCreditNote(string CrNoteNo, string SupplyType, string Type, string CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            LNSM_ReportRepository objPpgRepo = new LNSM_ReportRepository();



            if (SupplyType == "B2C" && CRDR == "C")
            {
                Eobj = new Einvoice();
                IrnModel m1 = new IrnModel();

                QrCodeInfo q1 = new QrCodeInfo();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                var Output = (QrCodeData)objPpgRepo.DBResponse.Data;

                m1.DocumentNo = Output.DocNo;
                m1.DocumentDate = Output.DocDt;
                m1.SupplierGstNo = Output.SellerGstin;
                m1.DocumentType = Output.DocTyp;
                String IRN = Eobj.GenerateB2cIrn(m1);
                Output.Irn = IRN;
                Output.IrnDt = Output.DocDt;
                Output.iss = "NIC";
                q1.Data = Output;
                B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);

                //   return Json(objPpgRepo.DBResponse.Status);
                //   IrnResponse ERes = await Eobj.GenerateB2cIrn();
            }
            else if (SupplyType == "B2C" && CRDR == "D")
            {
                Eobj = new Einvoice();
                IrnModel m1 = new IrnModel();

                LNSM_IrnB2CDetails q1 = new LNSM_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                var irnb2cobj = (LNSM_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string irn = Eobj.GenerateB2cIrn(irnModelObj);
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                    objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = irn;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNB2CCreditDebitNote(irn, objresponse, CrNoteNo, CRDR);
                }
                else
                {
                    var tn = "GST QR";
                    UpiQRCodeInfo idata = new UpiQRCodeInfo();
                    idata.ver = irnb2cobj.ver;
                    idata.mode = irnb2cobj.mode;
                    idata.mode = irnb2cobj.mode;
                    idata.tr = irnb2cobj.tr;
                    idata.tid = irnb2cobj.tid;
                    idata.tn = tn;
                    idata.pa = irnb2cobj.pa;
                    idata.pn = irnb2cobj.pn;
                    idata.mc = irnb2cobj.mc;
                    idata.am = irnb2cobj.TotInvVal;
                    idata.mam = irnb2cobj.TotInvVal;
                    idata.mid = irnb2cobj.mid;
                    idata.msid = irnb2cobj.msid;
                    idata.orgId = irnb2cobj.orgId;
                    idata.mtid = irnb2cobj.mtid;
                    idata.CESS = irnb2cobj.CESS;
                    idata.CGST = irnb2cobj.CGST;
                    idata.SGST = irnb2cobj.SGST;
                    idata.IGST = irnb2cobj.IGST;
                    idata.GSTIncentive = irnb2cobj.GSTIncentive;
                    idata.GSTPCT = irnb2cobj.GSTPCT;
                    idata.qrMedium = irnb2cobj.qrMedium;
                    idata.invoiceNo = irnb2cobj.DocNo;
                    idata.InvoiceDate = irnb2cobj.DocDt;
                    idata.InvoiceName = irnb2cobj.InvoiceName;
                    idata.QRexpire = irnb2cobj.DocDt;
                    idata.pinCode = irnb2cobj.pinCode;
                    idata.tier = irnb2cobj.tier;
                    idata.gstIn = irnb2cobj.gstIn;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string IRN = Eobj.GenerateB2cIrn(irnModelObj);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = IRN;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, objresponse, CrNoteNo, CRDR);
                }
            }
            else
            {
                objPpgRepo.GetIRNForDebitCredit(CrNoteNo, Type, CRDR);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;
                if (Output.BuyerDtls.Gstin != "" && Output.BuyerDtls.Gstin != null)
                {

                    objPpgRepo.GetHeaderIRNForCreditDebitNote();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);

                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
                    Eobj = new Einvoice(Hp, jsonEInvoice);
                    ERes = await Eobj.GenerateEinvoice();
                    if (ERes.Status == 1)
                    {
                        objPpgRepo.AddEditIRNResponsecForCreditDebitNote(ERes, CrNoteNo, CRDR);
                    }
                    else
                    {
                        objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                        objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                    }
                }
                else
                {

                    if (SupplyType == "B2C" && CRDR == "C")
                    {
                        Eobj = new Einvoice();
                        IrnModel m1 = new IrnModel();

                        QrCodeInfo q1 = new QrCodeInfo();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                        var Output1 = (QrCodeData)objPpgRepo.DBResponse.Data;

                        m1.DocumentNo = Output1.DocNo;
                        m1.DocumentDate = Output1.DocDt;
                        m1.SupplierGstNo = Output1.SellerGstin;
                        m1.DocumentType = Output1.DocTyp;
                        String IRN = Eobj.GenerateB2cIrn(m1);
                        Output1.Irn = IRN;
                        Output1.IrnDt = Output1.DocDt;
                        Output1.iss = "NIC";
                        q1.Data = Output1;
                        B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                        objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);

                        //   return Json(objPpgRepo.DBResponse.Status);
                        //   IrnResponse ERes = await Eobj.GenerateB2cIrn();
                    }
                    else if (SupplyType == "B2C" && CRDR == "D")
                    {
                        Eobj = new Einvoice();
                        IrnModel m1 = new IrnModel();

                        LNSM_IrnB2CDetails q1 = new LNSM_IrnB2CDetails();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                        var irnb2cobj = (LNSM_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                        if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                        {
                            IrnModel irnModelObj = new IrnModel();
                            irnModelObj.DocumentDate = irnb2cobj.DocDt;
                            irnModelObj.DocumentNo = irnb2cobj.DocNo;
                            irnModelObj.DocumentType = irnb2cobj.DocTyp;
                            irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                            string irn = Eobj.GenerateB2cIrn(irnModelObj);
                            B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                            string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                            objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                            IrnResponse objERes = new IrnResponse();
                            objERes.irn = irn;
                            objERes.SignedQRCode = objresponse.QrCodeBase64;
                            objERes.SignedInvoice = objresponse.QrCodeJson;
                            objERes.SignedQRCode = objresponse.QrCodeJson;

                            objPpgRepo.AddEditIRNB2CCreditDebitNote(irn, objresponse, CrNoteNo, CRDR);
                        }
                        else
                        {
                            var tn = "GST QR";
                            UpiQRCodeInfo idata = new UpiQRCodeInfo();
                            idata.ver = irnb2cobj.ver;
                            idata.mode = irnb2cobj.mode;
                            idata.mode = irnb2cobj.mode;
                            idata.tr = irnb2cobj.tr;
                            idata.tid = irnb2cobj.tid;
                            idata.tn = tn;
                            idata.pa = irnb2cobj.pa;
                            idata.pn = irnb2cobj.pn;
                            idata.mc = irnb2cobj.mc;
                            idata.am = irnb2cobj.TotInvVal;
                            idata.mam = irnb2cobj.TotInvVal;
                            idata.mid = irnb2cobj.mid;
                            idata.msid = irnb2cobj.msid;
                            idata.orgId = irnb2cobj.orgId;
                            idata.mtid = irnb2cobj.mtid;
                            idata.CESS = irnb2cobj.CESS;
                            idata.CGST = irnb2cobj.CGST;
                            idata.SGST = irnb2cobj.SGST;
                            idata.IGST = irnb2cobj.IGST;
                            idata.GSTIncentive = irnb2cobj.GSTIncentive;
                            idata.GSTPCT = irnb2cobj.GSTPCT;
                            idata.qrMedium = irnb2cobj.qrMedium;
                            idata.invoiceNo = irnb2cobj.DocNo;
                            idata.InvoiceDate = irnb2cobj.DocDt;
                            idata.InvoiceName = irnb2cobj.InvoiceName;
                            idata.QRexpire = irnb2cobj.DocDt;
                            idata.pinCode = irnb2cobj.pinCode;
                            idata.tier = irnb2cobj.tier;
                            idata.gstIn = irnb2cobj.gstIn;
                            B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                            objresponse = Eobj.GenerateB2cQRCode(idata);
                            IrnModel irnModelObj = new IrnModel();
                            irnModelObj.DocumentDate = irnb2cobj.DocDt;
                            irnModelObj.DocumentNo = irnb2cobj.DocNo;
                            irnModelObj.DocumentType = irnb2cobj.DocTyp;
                            irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                            string IRN = Eobj.GenerateB2cIrn(irnModelObj);
                            IrnResponse objERes = new IrnResponse();
                            objERes.irn = IRN;
                            objERes.SignedQRCode = objresponse.QrCodeBase64;
                            objERes.SignedInvoice = objresponse.QrCodeJson;
                            objERes.SignedQRCode = objresponse.QrCodeJson;

                            objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, objresponse, CrNoteNo, CRDR);
                        }
                    }
                }

            }
            // var Images = LoadImage(ERes.QRCodeImageBase64);

            return Json(objPpgRepo.DBResponse);
        }

        #endregion



        #region Irn Response
        [HttpGet]
        public ActionResult IrnResponse()
        {

            LNSM_ReportRepository objParty = new LNSM_ReportRepository();

            return PartialView();
        }
        [HttpGet]
        public ActionResult GetAllResponse(string PeriodFrom = "", string PeriodTo = "")
        {

            IRNResponseModel model = new IRNResponseModel();
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<IRNResponseModel> ListIRN = new List<IRNResponseModel>();
            ObjGR.GetIRNResponse(PeriodFrom, PeriodTo);

            if (ObjGR.DBResponse.Data != null)
                ListIRN = (List<IRNResponseModel>)ObjGR.DBResponse.Data;


            return PartialView("ListOfResponse", ListIRN);

        }

        #endregion

        #region Bulk Invoice

        [HttpGet]
        public ActionResult BulkInvoiceReport()
        {



            return PartialView();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReport(LNSM_InvoiceInformation ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.BulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
            string FilePath = "";
            DataSet ds = new DataSet();
            if (ObjRR.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjRR.DBResponse.Data;
                FilePath = GeneratingInvoicePDF(ds);
                return Json(new { Status = 1, Data = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Data = "No Record Found." });
            }
        }


        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    System.IO.File.Delete(file);
                }
                //Delete all child Directories
                //foreach (string directory in Directory.GetDirectories(path))
                //{
                //    DeleteDirectory(directory);
                //}
                ////Delete a Directory
                //Directory.Delete(path);
            }
        }


        #endregion

        #region JSON Response Status
        public ActionResult JsonResponseStatus()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetJsonResponseStatus(string PeriodFrom = "", string PeriodTo = "", string IntType = "", string Status = "")
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_JsonRespnStatusModel> ListIRN = new List<LNSM_JsonRespnStatusModel>();
            ObjGR.GetJSONResponse(PeriodFrom, PeriodTo, IntType, Status);

            if (ObjGR.DBResponse.Data != null)
                ListIRN = (List<LNSM_JsonRespnStatusModel>)ObjGR.DBResponse.Data;

            return PartialView("ListOfJSONResponseStatus", ListIRN);


        }
        [HttpGet]
        public ActionResult GetJsonStringFile(string InvoiceId)
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_JsonRespnStatusModel> ListIRN = new List<LNSM_JsonRespnStatusModel>();
            //string jsonString = "{'name':'John','age':30}";
            string jsonString = string.Empty;
            int InvoiceIds = 0;

            ObjGR.GetJsonString(InvoiceId);
            if (ObjGR.DBResponse.Data != null)
            {
                ListIRN = (List<LNSM_JsonRespnStatusModel>)ObjGR.DBResponse.Data;
                jsonString = ListIRN[0].jsonEInvoice;


            }
            else
            {
                var myErrorObj = new
                {
                    Message = "No Data Found",
                    Status = 0

                };

                // Convert the object to a JSON string
                var myJSON = JsonConvert.SerializeObject(myErrorObj);
                jsonString = myJSON.ToString();
            }
            string iso8601Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var FileName = InvoiceIds + "_" + "JsaonText.json";
            string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/Jsaon/";
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);




            // Create a StreamWriter object to write to the text file
            StreamWriter writer = new StreamWriter(LocalDirectory + FileName);

            // Write the JSON string to the text file
            writer.Write(jsonString);

            // Close the StreamWriter object
            writer.Close();

            // Create a FileStreamResult object to allow the user to download the file
            FileStream fileStream = new FileStream(LocalDirectory + FileName, FileMode.Open);
            FileStreamResult fileStreamResult = new FileStreamResult(fileStream, "json/plain");
            fileStreamResult.FileDownloadName = iso8601Timestamp + "_" + InvoiceId + "_" + "JsaonText.json";

            return fileStreamResult;


        }

        public ActionResult GetJsonResponseStringFile(string InvoiceId)
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_JsonRespnStatusModel> ListIRN = new List<LNSM_JsonRespnStatusModel>();
            //string jsonString = "{'name':'John','age':30}";
            string jsonString = string.Empty;


            try
            {
                ObjGR.GetJsonResponseStringFile(InvoiceId);

                if (ObjGR.DBResponse.Data != null)
                {
                    ListIRN = (List<LNSM_JsonRespnStatusModel>)ObjGR.DBResponse.Data;
                    //jsonString = ListIRN[0].jsonEInvoice;

                    var myObj = new
                    {
                        InvoiceId = "" + ListIRN[0].InvoiceId + "",
                        InvoiceNo = "" + ListIRN[0].InvoiceNumber + "",
                        StatusCode = "" + ListIRN[0].StatusCode + "",
                        StatusMessage = "" + ListIRN[0].Message + "",
                        SendStatus = "" + ListIRN[0].SendStatus + ""
                    };
                    // Convert the object to a JSON string
                    var myJSON = JsonConvert.SerializeObject(myObj);
                    jsonString = Convert.ToString(myJSON);
                }
                else
                {
                    var myErrorObj = new
                    {
                        Message = "No Data Found",
                        Status = 0

                    };

                    // Convert the object to a JSON string
                    var myJSONErr = JsonConvert.SerializeObject(myErrorObj);
                    jsonString = myJSONErr.ToString();
                    //jsonString = "{'Message':'No Data Found','Status':0}";
                }
            }
            catch (Exception ex)
            {

            }
            string iso8601Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var FileName = InvoiceId + "_" + "JsaonResponse.json";
            string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/Jsaon/";
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);
            //string fileName = "NewJson.txt";



            // Create a StreamWriter object to write to the text file
            StreamWriter writer = new StreamWriter(LocalDirectory + FileName);

            // Write the JSON string to the text file
            writer.Write(jsonString);

            // Close the StreamWriter object
            writer.Close();

            // Create a FileStreamResult object to allow the user to download the file
            FileStream fileStream = new FileStream(LocalDirectory + FileName, FileMode.Open);
            FileStreamResult fileStreamResult = new FileStreamResult(fileStream, "json/plain");
            fileStreamResult.FileDownloadName = iso8601Timestamp + "_" + InvoiceId + "_" + "JsaonResponse.json";

            return fileStreamResult;


        }

        #endregion
        #region CashRecipt
        public ActionResult GetReciptJsonStringFile(string InvoiceId)
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_JsonRespnStatusModel> ListIRN = new List<LNSM_JsonRespnStatusModel>();
            //string jsonString = "{'name':'John','age':30}";
            string jsonString = string.Empty;
            int InvoiceIds = 0;

            ObjGR.GetReciptJsonString(InvoiceId);
            if (ObjGR.DBResponse.Data != null)
            {
                ListIRN = (List<LNSM_JsonRespnStatusModel>)ObjGR.DBResponse.Data;
                jsonString = ListIRN[0].jsonEInvoice;


            }
            else
            {
                var myErrorObj = new
                {
                    Message = "No Data Found",
                    Status = 0

                };

                // Convert the object to a JSON string
                var myJSON = JsonConvert.SerializeObject(myErrorObj);
                jsonString = myJSON.ToString();
            }
            string iso8601Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var FileName = InvoiceIds + "_" + "CashReciptJsaonText.json";
            string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/Jsaon/";
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);




            // Create a StreamWriter object to write to the text file
            StreamWriter writer = new StreamWriter(LocalDirectory + FileName);

            // Write the JSON string to the text file
            writer.Write(jsonString);

            // Close the StreamWriter object
            writer.Close();

            // Create a FileStreamResult object to allow the user to download the file
            FileStream fileStream = new FileStream(LocalDirectory + FileName, FileMode.Open);
            FileStreamResult fileStreamResult = new FileStreamResult(fileStream, "json/plain");
            fileStreamResult.FileDownloadName = iso8601Timestamp + "_" + InvoiceId + "_" + "CashReciptJsaonText.json";

            return fileStreamResult;


        }
        public ActionResult GetJsonResponseCashReciptString(string InvoiceId)
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_JsonRespnStatusModel> ListIRN = new List<LNSM_JsonRespnStatusModel>();
            //string jsonString = "{'name':'John','age':30}";
            string jsonString = string.Empty;


            try
            {
                ObjGR.GetJsonResponseCashReciptStringFile(InvoiceId);

                if (ObjGR.DBResponse.Data != null)
                {
                    ListIRN = (List<LNSM_JsonRespnStatusModel>)ObjGR.DBResponse.Data;
                    //jsonString = ListIRN[0].jsonEInvoice;

                    var myObj = new
                    {
                        InvoiceId = "" + ListIRN[0].InvoiceId + "",
                        InvoiceNo = "" + ListIRN[0].InvoiceNumber + "",
                        StatusCode = "" + ListIRN[0].StatusCode + "",
                        StatusMessage = "" + ListIRN[0].Message + "",

                    };
                    // Convert the object to a JSON string
                    var myJSON = JsonConvert.SerializeObject(myObj);
                    jsonString = Convert.ToString(myJSON);
                }
                else
                {
                    var myErrorObj = new
                    {
                        Message = "No Data Found",
                        Status = 0

                    };

                    // Convert the object to a JSON string
                    var myJSONErr = JsonConvert.SerializeObject(myErrorObj);
                    jsonString = myJSONErr.ToString();
                    //jsonString = "{'Message':'No Data Found','Status':0}";
                }
            }
            catch (Exception ex)
            {

            }
            string iso8601Timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var FileName = InvoiceId + "_" + "JsaonResponseCashRecipt.json";
            string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/Jsaon/";
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);
            //string fileName = "NewJson.txt";



            // Create a StreamWriter object to write to the text file
            StreamWriter writer = new StreamWriter(LocalDirectory + FileName);

            // Write the JSON string to the text file
            writer.Write(jsonString);

            // Close the StreamWriter object
            writer.Close();

            // Create a FileStreamResult object to allow the user to download the file
            FileStream fileStream = new FileStream(LocalDirectory + FileName, FileMode.Open);
            FileStreamResult fileStreamResult = new FileStreamResult(fileStream, "json/plain");
            fileStreamResult.FileDownloadName = iso8601Timestamp + "_" + InvoiceId + "_" + "JsaonResponseCashRecipt.json";

            return fileStreamResult;


        }

        #endregion

        #region   Tally Response Report 
        //25-OCT-2021
        public ActionResult TallyResponseReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TallyResponseReport(TallyResponse vm)
        {
            Ppg_ReportRepositoryV2 ObjRR = new Ppg_ReportRepositoryV2();
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
                Ppg_ReportRepositoryV2 ObjRR = new Ppg_ReportRepositoryV2();
                ObjRR.getCompanyDetails();
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                var FileName = "TallyResponseReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td valign='top' width='10%'><img align='right' src='IMGSRC'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>107-109 , EPIP Zone , KIADB Industrial Area <br/>" + objCompanyDetails.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompanyDetails.EmailAddress + "</label>");
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
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 15%;'>Bill Of Supply</th>");
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

        #region DTR Rail Summary 

        [HttpGet]
        public ActionResult DailyCashBookReportRailSum()
        {


            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult DailyCashBookReportRailSumExcel(LNSM_DTRRailSummary fc)
        {

            Session["CompanayName"] = ZonalOffice;

            List<LNSM_DTRRailSummary> LstDailyCashBook = new List<LNSM_DTRRailSummary>();
            LNSM_DTRRailSummary ObjPV = new LNSM_DTRRailSummary();
            ObjPV.PeriodFrom = (fc.PeriodFrom.ToString());
            ObjPV.PeriodTo = (fc.PeriodTo.ToString());
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            ObjRR.DTRRailSummary(ObjPV);
            string excelName = "";
            excelName = "DailyTransactionRailSumm" + "_" + ".xls";
            if (ObjRR.DBResponse.Data != null)
            {
                return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            else
            {

                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
            }

        }
        #endregion


        #region Outstanding Amount Report

        [HttpGet]
        public ActionResult OutstandingAmountReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OutstandingAmountReport(LNSM_OutstandingAmountReport ObjPV)
        {
            if (ModelState.IsValid)
            {
                LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
                ObjRR.OutstandingAmountReport(Convert.ToDateTime(ObjPV.FromDate), Convert.ToDateTime(ObjPV.ToDate));
                string Path = "";
                if (ObjRR.DBResponse.Data != null)
                {
                    List<LNSM_OutstandingAmountReport> lstData = (List<LNSM_OutstandingAmountReport>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFOutstandingAmountReport(lstData, ObjPV.FromDate, ObjPV.ToDate);
                }
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFOutstandingAmountReport(List<LNSM_OutstandingAmountReport> lstData, String FromDate, String ToDate)
        {
            try
            {
                var FileName = "OutstandingAmountReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + ZonalOffice.ToString() + "</span><br/><label style='font-size: 14px; font-weight:bold;'>Outstanding Amount Report</label><br/><label style='font-size: 12px;'><b>From Date :</b> " + FromDate + " - <b>To Date :</b> " + ToDate + "</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>S.NO.</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>BILLING DATE</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>BILLING NO.</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>PARTY NAME</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>PARTY CODE</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>MONTH</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>AREA</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>RATE(Per SQM)</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>AMOUNT RECEIVABLE</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>SGST</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>CGST</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>IGST</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>TOTAL AMOUNT RECEIVABLE</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 600px;'>REMARKS</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");
                lstData.ForEach(item =>
                {
                    Pages.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.BillingDate + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.BillingNo + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.PartyName + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.PartyCode + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.Month + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.Area + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.SQM + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.AmountReceivalbe + " </td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.SGST + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.CGST + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.IGST + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>" + item.TotalAmount + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 600px;'>" + item.Remarks + "</td></tr>");
                    i++;
                });

                Pages.Append("<tr><th colspan='8' style='border-right: 1px solid #000; font-size: 10px; text-align: left; width: 100px;'>TOTAL</th>");
                Pages.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + lstData.Sum(item => item.AmountReceivalbe) + "</td>");
                Pages.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + lstData.Sum(item => item.SGST) + "</td>");
                Pages.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + lstData.Sum(item => item.CGST) + "</td>");
                Pages.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + lstData.Sum(item => item.IGST) + "</td>");
                Pages.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + lstData.Sum(item => item.TotalAmount) + "</td><td></td></tr>");

                Pages.Append("</tbody></table>");

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
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 40f, 40f, 40f, 40f, false, true))
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


        #region Bulk Invoice For External User

        [HttpGet]
        public ActionResult BulkInvoiceReportExternalUser()
        {
            Login objLogin = (Login)Session["LoginUser"];

            ViewBag.UserName = objLogin.Name;
            ViewBag.UserId = objLogin.EximTraderId;


            return PartialView();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReportExternalUser(LNSM_InvoiceInformation ObjBulkInvoiceReport)
        {

            Login objLogin = (Login)Session["LoginUser"];

            ObjBulkInvoiceReport.PartyId = objLogin.EximTraderId;

            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            // Login objLogin = (Login)Session["LoginUser"];
            ObjRR.BulkInvoiceDetailsForPrintUser(ObjBulkInvoiceReport);
            string FilePath = "";
            DataSet ds = new DataSet();
            if (ObjRR.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjRR.DBResponse.Data;
                FilePath = GeneratingInvoicePDF(ds);
                return Json(new { Status = 1, Data = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Data = "No Record Found." });
            }
        }





        #endregion

        #region SD Details Report
        public ActionResult SDDetailsReportUser()
        {
            Login objLogin = (Login)Session["LoginUser"];



            LNSM_SDDetReport vm = new LNSM_SDDetReport();
            vm.PartyId = objLogin.EximTraderId;
            vm.Party = objLogin.Name;

            return PartialView(vm);
        }



        #endregion

        #region Bulk Receipt  Report For External User
        [HttpGet]
        public ActionResult BulkReceiptReportForExternalUser()
        {
            return PartialView();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkReceiptReportForExternalUser(BulkReceiptReport ObjBulkReceiptReport)
        {
            LNSM_ReportRepository ObjRR = new LNSM_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetBulkCashreceiptForExternalUser(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber, objLogin.EximTraderId);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }


        public ActionResult ListOfReceiptDateWiseForExternalUser(string FromDate, string ToDate)
        {
            ReportRepository ObjRR = new ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetReceiptListForExternalUser(FromDate, ToDate, objLogin.EximTraderId);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
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
        #endregion


        #region Debit Note 
        [HttpGet]
        public ActionResult DebitNote()
        {

            LNSM_ReportRepository objParty = new LNSM_ReportRepository();
            //objParty.GetInvPaymentParty();
            //if (objParty.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objParty.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetAllDebitNote(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {

            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_CreditNote> lstInv = new List<LNSM_CreditNote>();
            ObjGR.GetAllDebitNote(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_CreditNote>)ObjGR.DBResponse.Data;
            return PartialView("ListOfDebitNote", lstInv);

        }
        [HttpGet]
        public ActionResult GetLoadMoreDebitNote(int Page = 0, string PeriodFrom = "", string PeriodTo = "", string invNo = "", string PartyGSTNo = "", string PartyName = "", string RefInvoiceNo = "")
        {
            LNSM_ReportRepository ObjGR = new LNSM_ReportRepository();
            List<LNSM_CreditNote> lstInv = new List<LNSM_CreditNote>();
            ObjGR.GetAllDebitNote(Page, PeriodFrom, PeriodTo, invNo, PartyGSTNo, PartyName, RefInvoiceNo);

            if (ObjGR.DBResponse.Data != null)
                lstInv = (List<LNSM_CreditNote>)ObjGR.DBResponse.Data;
            return Json(lstInv, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetDebitNotePrint(string CRNoteId)
        {
            LNSM_ReportRepository objRepo = new LNSM_ReportRepository();
            PrintPDFModelOfCr objCR = new PrintPDFModelOfCr();
            objRepo.PrintPDFForDebitNote(CRNoteId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintPDFModelOfCr)objRepo.DBResponse.Data;
                string Path = GenerateDebitPDF(objCR, Convert.ToInt32(CRNoteId));
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }


        public string GenerateDebitPDF(PrintPDFModelOfCr objCR, int CRNoteId)
        {
            string Note = "D";
            UpiQRCodeInfoCR upiQRInfo = new UpiQRCodeInfoCR();
            upiQRInfo.ver = objCR.ver;

            upiQRInfo.tn = objCR.tn;
            upiQRInfo.tier = objCR.tier;
            upiQRInfo.tid = objCR.tid;
            upiQRInfo.qrMedium = objCR.qrMedium;
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCR.pn;
            upiQRInfo.pa = objCR.pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCR.mtid;
            upiQRInfo.msid = objCR.msid;
            upiQRInfo.mode = objCR.mode;
            upiQRInfo.mid = objCR.mid;
            upiQRInfo.mc = objCR.mc;
            upiQRInfo.gstIn = objCR.gstIn;

            upiQRInfo.InvoiceDate = Convert.ToString(objCR.QRInvoiceDate);
            upiQRInfo.invoiceNo = Convert.ToString(objCR.CRNoteNo);
            upiQRInfo.InvoiceName = Convert.ToString(objCR.PartyName);
            upiQRInfo.mam = Convert.ToDecimal(objCR.mam);
            upiQRInfo.am = Convert.ToDecimal(objCR.am);
            upiQRInfo.CGST = Convert.ToDecimal(objCR.CGST);
            upiQRInfo.SGST = Convert.ToDecimal(objCR.SGST);
            upiQRInfo.IGST = Convert.ToDecimal(objCR.IGST);
            upiQRInfo.GSTPCT = Convert.ToDecimal(objCR.GSTPCT);
            upiQRInfo.QRexpire = Convert.ToString(objCR.QRInvoiceDate);
            upiQRInfo.tr = Convert.ToString(objCR.CrNoteId);

            //Einvoice Eobj = new Einvoice();
            //B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
            //objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
            //IrnResponse objERes = new IrnResponse();
            //objERes.SignedQRCode = objresponse.QrCodeBase64;
            //objERes.SignedInvoice = objresponse.QrCodeJson;
            //objERes.SignedQRCode = objresponse.QrCodeJson;

            //string SACCode = "", note = "", fileName = "";
            //objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            //{
            //    if (SACCode == "")
            //        SACCode = item.SACCode;
            //    else
            //        SACCode = SACCode + "," + item.SACCode;
            //});
            //note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
            //fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
            //string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
            //if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            //{
            //    Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            //}
            //if (System.IO.File.Exists(Path + "/" + fileName))
            //{
            //    System.IO.File.Delete(Path + "/" + fileName);
            //}



            //string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Place Of Supply:<span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
            //string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
            //string tr = "";
            //int Count = 1;
            //decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
            //objCR.lstCharges.ToList().ForEach(item =>
            //{
            //    tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
            //    IGSTAmt += item.IGSTAmt;
            //    CGSTAmt += item.CGSTAmt;
            //    SGSTAmt += item.SGSTAmt;
            //    Count++;
            //});
            //string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            //string Remarks = objCR.Remarks;
            //string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
            //html = html + htmltable + tr + tfoot;
            //using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            //{
            //    RH.GeneratePDF(Path + "/" + fileName, html);
            //}
            //return "/Docs/" + Session.SessionID + "/" + fileName;


            string SACCode = "", note = "", fileName = "";
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("");
            Einvoice obj = new Einvoice();
            objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            {
                if (SACCode == "")
                    SACCode = item.SACCode;
                else
                    SACCode = SACCode + "," + item.SACCode;
            });
            note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
            fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
            string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path + "/" + fileName))
            {
                System.IO.File.Delete(Path + "/" + fileName);
            }
            //string html = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td><td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br /><span style='font-size: 7pt; padding-bottom: 10px;'>107-109 , EPIP Zone , KIADB Industrial Area <br/> Whitefield , Bengaluru - 560066 </span> <br/><span style='font-size: 7pt; padding-bottom: 10px;'>Email - cwcwfdcfs@gmail.com</span><br /><label style='font-size: 7pt; font-weight:bold;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span></label><br/><label style='font-size: 7pt; font-weight:bold;'>" + note + "</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr>     <tr><th colspan='6' style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th colspan='6' style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Name:</b> " + objCR.CompanyName + "</td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Name:</b> <span>" + objCR.PartyName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Warehouse Address:</b> <span>" + objCR.CompanyAddress + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Address:</b> <span>" + objCR.PartyAddress + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>City:</b> <span>" + objCR.CompCityName + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>City:</b> <span>" + objCR.PartyCityName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State:</b> <span>" + objCR.CompStateName + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State:</b> <span>" + objCR.PartyStateName + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State Code:</b> <span>" + objCR.CompStateCode + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>State Code:</b> <span>" + objCR.PartyStateCode + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>GSTIN:</b> <span>" + objCR.CompGstIn + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><span><b>GSTIN(if registered):</b>" + objCR.PartyGSTIN + "</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>PAN:</b><span>" + objCR.CompPan + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Place Of Supply:</b><span>" + objCR.PartyStateName + "</span></td></tr> <tr><td colspan='6' style='text-align:left;padding:8px;'><b>Debit/Credit Note Serial No:</b> <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/><b>Date of Issue:</b> <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td colspan='6' style='text-align:left;padding:8px;'><b>Accounting Code of</b> <span>" + SACCode + "</span><br/><br/><b>Description of Services:</b> <span>Other Storage & Warehousing Services</span></td></tr> <tr><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><b>Original Bill of Supply/Tax Invoice No:</b> <span>" + objCR.InvoiceNo + "</span></td><td colspan='6' style='border:1px solid #000;text-align:left;padding:8px;'><span><b>Date:</b>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr>";


            html.Append("<table style='width: 100%; font-size: 7pt; font-family: Verdana, Arial, San-serif; border-collapse: collapse;'>");
            html.Append("<thead>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

            html.Append("<tr>");

            html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>_________<u>" + ZonalOffice + "</u>_____________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");
            //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td>");
            html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + objCR.irn + " </td></tr>");
            html.Append("</tbody></table></td>");


            if (objCR.SignedQRCode == "")
            { }
            else
            {
                //if (objCR.SupplyType == "B2C")
                //{
                //    //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");
                //    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                //    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                //    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                //    html.Append("</tbody></table></td>");

                //}
                //else
                //{
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");

                //}
            }




            html.Append("</tr>");

            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            //html += "<tr>";
            //html += "<td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90' /></td>";
            //html += "<td colspan='8' width='90%' valign='top' align='center'>";
            //html += "<h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>";
            //html += "<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />";
            //html += "<span style='font-size: 7pt; padding-bottom: 10px;'>";
            //html += "107-109 , EPIP Zone , KIADB Industrial Area <br />";
            //html += "Whitefield , Bengaluru - 560066";
            //html += "</span>";
            //html += "<br />";
            //html += "<span style='font-size: 7pt; padding-bottom: 10px;'>Email - cwcwfdcfs@gmail.com</span><br />";
            //html += "<label style='font-size: 7pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br />";
            //html += "<label style='font-size: 7pt; font-weight: bold;'>' + note + '</label>";
            //html += "</td>";
            //html += "<td valign='top'><img align='right' src='ISO' width='100' /></td>";
            //html += "</tr>";

            html.Append("<tr>");
            html.Append("<th colspan='6' style='border: 1px solid #000; text-align: center; padding: 8px; width: 50%;'>Details of Service Provider</th>");
            html.Append("<th colspan='6' style='border: 1px solid #000; text-align: center; padding: 8px; width: 50%;'>Details of Service Receiver</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Name:</b> " + objCR.CompanyName + "</td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Name:</b> <span>" + objCR.PartyName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Warehouse Address:</b> <span>" + objCR.CompanyAddress + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Address:</b> <span>" + objCR.PartyAddress + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>City:</b> <span>" + objCR.CompCityName + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>City:</b> <span>" + objCR.PartyCityName + "</span></td>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State:</b> <span>" + objCR.CompStateName + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State:</b> <span>" + objCR.PartyStateName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State Code:</b> <span>" + objCR.CompStateCode + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>State Code:</b> <span>" + objCR.PartyStateCode + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>GSTIN:</b> <span>" + objCR.CompGstIn + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'>");
            html.Append("<span><b>GSTIN(if registered):</b>" + objCR.PartyGSTIN + "</span>");
            html.Append("</td>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>PAN:</b><span>" + objCR.CompPan + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Place Of Supply:</b><span>" + objCR.PartyStateName + "</span></td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            html.Append("<b>Debit/Credit Note Serial No:</b> <span style='border-bottom: 1px solid #000;'>" + objCR.CRNoteNo + "</span><br />");
            html.Append("<br />");
            html.Append("<b>Date of Issue:</b> <span style='border-bottom: 1px solid #000;'>" + objCR.CRNoteDate + "</span>");
            html.Append("</td>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            html.Append("<b>Accounting Code of</b> <span>" + SACCode + "</span><br />");
            html.Append("<br />");
            html.Append("<b>Description of Services:</b> <span>Other Storage & Warehousing Services</span>");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'><b>Original Bill of Supply/Tax Invoice No:</b> <span>" + objCR.InvoiceNo + "</span></td>");
            html.Append("<td colspan='6' style='text-align: left; padding: 8px;'>");
            html.Append("<span><b>Date:</b>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span>");
            html.Append("</td>");
            html.Append("</tr>");
            //html.Append("</tbody>");
            //html.Append("</table>");

            html.Append("<tr>");
            html.Append("<td colspan='12'>");
            html.Append("<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'>");
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th>");
            html.Append("<th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th>");
            html.Append("</tr>)");
            html.Append("<tr>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            //string tr = "";
            int Count = 1;
            decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0, Taxable = 0;
            objCR.lstCharges.ToList().ForEach(item =>
            {
                html.Append("<tr>");
                html.Append("<td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td>");
                html.Append("<td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td>");
                html.Append("</tr>");

                IGSTAmt += item.IGSTAmt;
                CGSTAmt += item.CGSTAmt;
                SGSTAmt += item.SGSTAmt;
                Taxable += item.Taxable;
                Count++;
            });
            string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            string Remarks = objCR.Remarks;
            string PayeeName = objCR.PayeeName;
            html.Append("<tr>");
            html.Append("<th colspan='2' style='border:1px solid #000;text-align:left;padding:5px;'>Total</th>");
            html.Append("<th style='border:1px solid #000;text-align:center;padding:5px;'>" + Taxable + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'></th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</th>");
            html.Append("<th style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</th>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<th colspan='4' style='border:1px solid #000;border-right:0;text-align:left;padding:5px;'>Total Debit/Credit Note Value (in figure)</th>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<td style='border:1px solid #000;border-right:0;border-left:0;text-align:right;padding:5px;'></td>");
            html.Append("<th style='border:1px solid #000;border-left:0;text-align:right;padding:5px;'>" + objCR.GrandTotal + "</th>");
            html.Append("</tr>");

            //tfoot +="<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr>";
            html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr>");
            html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr>");
            html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Payer Name:</span> <span>" + PayeeName + "</span></td></tr>");
            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td colspan='12' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/>");
            html.Append("<span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span>");
            html.Append("</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td colspan='12' style='text-align:right;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td colspan='12' style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>This credit note is system generated, hence does not require any sign.</p></td>");
            html.Append("</tr>");


            html.Append("</tbody>");
            html.Append("</table>");

            //html = html + htmltable +tr+ tfoot;

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html = html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(html.ToString());
            using (var RH = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f))
            {
                RH.GeneratePDF(Path + "/" + fileName, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + fileName;
        }



        #endregion
    }
}