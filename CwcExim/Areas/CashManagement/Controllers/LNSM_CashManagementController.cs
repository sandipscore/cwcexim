using CwcExim.Areas.CashManagement.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using CwcExim.Areas.Report.Models;
using CwcExim.Areas.Import.Models;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using EinvoiceLibrary;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.X509;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Data;
using System.Collections.Specialized;
using System.Net;
using CCA.Util;
using System.Configuration;
using System.Diagnostics;

namespace CwcExim.Areas.CashManagement.Controllers
{

    public class LNSM_CashManagementController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        private string strQRCode = "000201010211021645992400041983220415545927000419832061661000500041983220827PUNB0042000420001210003366226410010A00000052401230514200A0000151.mab@pnb27260010A00000052401080601414A5204939953033565802IN5923CENTRAL WAREHOUSING COR6008GHAZIPUR6106110096621207080601414A6304b8e4";

        public LNSM_CashManagementController()
        {           
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;

        }

        #region Payment Receipt/Cash Receipt 

        [HttpGet]
        public ActionResult CashReceipt(int PartyId = 0, string PartyName = "", string Type = "INVOICE")
        {
            LNSM_CashReceiptModel ObjCashReceipt = new LNSM_CashReceiptModel();

            var objRepo = new LNSM_CashManagementRepository();
            if (PartyId == 0)
            {                
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                   ViewBag.Invoice = null;
                    for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new LNSM_CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                ViewBag.CashReceiptInvoiveMappingList = null;
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;
                ViewBag.ServerDate = Utility.GetServerDate();
                return PartialView(ObjCashReceipt);
            }
            else
            {                
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetails(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (LNSM_CashReceiptModel)objRepo.DBResponse.Data;                    
                    ViewBag.Pay = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new LNSM_CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;
               
                ViewBag.CashReceiptInvoiveMappingList = JsonConvert.SerializeObject(ObjCashReceipt.CashReceiptInvoiveMappingList);
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;
                if (Type == "CREDITNOTE")
                {
                    ModelState.Remove("TotalPaymentReceipt");

                }
                ViewBag.ServerDate = Utility.GetServerDate();
                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCashReceipt(LNSM_CashReceiptModel ObjCashReceipt)
        {
            List<LNSM_CashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<LNSM_CashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<LNSM_CashReceiptInvoiveMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.InvoiceDate = dt.ToString("yyyy-MM-dd");
                ObjCashReceipt.InvoiceValue = ObjCashReceipt.InvoiceValue + item.InvoiceAmt;
                if(item.InvoiceAmt<item.AdjustmentAmt)
                {
                    ObjCashReceipt.BalanceAmt =item.AdjustmentAmt- item.InvoiceAmt;
                }
               
            }

            ObjCashReceipt.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
            var xml = "";
            if (ObjCashReceipt.Type == "CREDITNOTE")
            {
                xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount != null).ToList());
            }
            else
            {
                xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
            }
           
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.AddCashReceipt(ObjCashReceipt, xml);
            if (objRepo.DBResponse.Status == 1)
            {
                GenerateCashReceiptService();
            }

            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrint(int CashReceiptId)
        {
            var objRepo = new LNSM_CashManagementRepository();
            var model = new PostPaymentSheet();
            objRepo.GetCashRcptPrint(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UpdatePrintHtml(FormCollection fc)
        //{
        //    int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
        //    string htmlPrint = fc["htmlPrint"].ToString();
        //    var objRepo = new LNSM_CashManagementRepository();
        //    objRepo.UpdatePrintHtml(CashReceiptId, htmlPrint);
        //    return Json(objRepo.DBResponse.Status, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateCashReceiptPDF(FormCollection fc)
        {
            try
            {
                var pages = new string[2];
                var type = fc["type"].ToString();
                var id = fc["id"].ToString();
                pages[0] = fc["page"].ToString();
                pages[1] = fc["npage"].ToString();
                var fileName = id + ".pdf";
                var ImgLeft = Server.MapPath("~/Content/Images/CWCPDF.PNG");
                var ImgRight = Server.MapPath("~/Content/Images/SwachhBharat-Logo.png");
                pages[0] = fc["page"].ToString().Replace("IMGLeft", ImgLeft).Replace("IMGRight", ImgRight);               
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.HeadOffice = this.HeadOffice;
                    rh.HOAddress = this.HOAddress;
                    rh.ZonalOffice = this.ZonalOffice;
                    rh.ZOAddress = this.ZOAddress;
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpGet]
        public JsonResult PayeeSearchByPartyCode(string PartyCode)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.ListOfPayeeForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayeeList(string PartyCode, int Page)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.ListOfPayeeForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CashReceiptList(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetCashReceiptList(ReceiptNo);
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            lstCashReceipt = (List<LNSM_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("CashReceiptList", lstCashReceipt);
        }

        [HttpGet]
        public ActionResult GetLoadMoreCashReceiptList(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMoreCashReceiptList(ReceiptNo, Page);
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            if (objCR.DBResponse.Data != null)
            {
                lstCashReceipt = (List<LNSM_CashReceiptModel>)objCR.DBResponse.Data;
            }
            return Json(lstCashReceipt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkReceiptReport(LNSM_BulkReceipt ObjBulkReceiptReport)
        {
            LNSM_CashManagementRepository ObjRR = new LNSM_CashManagementRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkReceiptReport(ds);
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
        #endregion

        #region ADD MONEY TO PD 

        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            var model = new LNSM_AddMoneyToPDModel();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new LNSM_ReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            ViewBag.ServerDate = Utility.GetServerDate();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(LNSM_AddMoneyToPDModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new LNSM_CashManagementRepository();
                    objRepo.AddMoneyToPD(m.PartyId, Convert.ToDateTime(m.TransDate), xml);
                    return Json(objRepo.DBResponse, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                    var Err = new { Statua = -1, Messgae = "Error" };
                    return Json(Err, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new { Status = 0, Message = "Some error occurs !!" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF1(FormCollection fc)
        {
            try
            {
                var pages = new string[1];
                pages[0] = fc["page"].ToString();
                var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/AddMoneyReceipt/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                pages[0] = pages[0].Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Data = fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        public ActionResult AddMoneyToPDList(int Page = 0)
        {
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetAddMoneyToPDList(Page);
            ViewBag.State = 0;
            List<LNSM_AddMoneyToPDListModel> lstAddMoneyToPD = new List<LNSM_AddMoneyToPDListModel>();
            lstAddMoneyToPD = (List<LNSM_AddMoneyToPDListModel>)objRepo.DBResponse.Data;
            ViewBag.State = objRepo.DBResponse.Message;

            return PartialView(lstAddMoneyToPD);

        }

        public JsonResult GetAddMoneyToPDListLoadMore(int Page = 0)
        {
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetAddMoneyToPDList(Page);
            ViewBag.State = 0;
            List<LNSM_AddMoneyToPDListModel> lstAddMoneyToPD = new List<LNSM_AddMoneyToPDListModel>();
            lstAddMoneyToPD = (List<LNSM_AddMoneyToPDListModel>)objRepo.DBResponse.Data;
            ViewBag.State = objRepo.DBResponse.Message;

            return Json(new { Data = lstAddMoneyToPD, State = objRepo.DBResponse.Message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAddMoneyToPDListBySearch(string ReceiptNo)
        {
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetSearchAddMoneyToPDList(ReceiptNo);
            List<LNSM_AddMoneyToPDListModel> lstAddMoneyToPD = new List<LNSM_AddMoneyToPDListModel>();
            lstAddMoneyToPD = (List<LNSM_AddMoneyToPDListModel>)objRepo.DBResponse.Data;

            return PartialView("AddMoneyToPDList", lstAddMoneyToPD);

        }
        #endregion

        #region Payment Adjust Through SD 

        [HttpGet]
        public ActionResult CashReceiptSD(int PartyId = 0, string PartyName = "", string Type = "INVOICE")
        {
            LNSM_CashReceiptModel ObjCashReceipt = new LNSM_CashReceiptModel();

            var objRepo = new LNSM_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetPartyListSD();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new LNSM_CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                ViewBag.CashReceiptInvoiveMappingList = null;
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;

                return PartialView(ObjCashReceipt);
            }
            else
            {
                objRepo.GetPartyListSD();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetailsSD(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (LNSM_CashReceiptModel)objRepo.DBResponse.Data;
                    
                    ViewBag.Pay = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((LNSM_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new LNSM_CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                
                ViewBag.CashReceiptInvoiveMappingList = JsonConvert.SerializeObject(ObjCashReceipt.CashReceiptInvoiveMappingList);
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;
                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashReceiptSD(LNSM_CashReceiptModel ObjCashReceipt)
        {
            List<LNSM_CashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<LNSM_CashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<LNSM_CashReceiptInvoiveMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.InvoiceDate = dt.ToString("yyyy-MM-dd");
                ObjCashReceipt.InvoiceValue = ObjCashReceipt.InvoiceValue + item.InvoiceAmt;
            }

            ObjCashReceipt.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
            var xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());            
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.AddCashReceiptSD(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrintSD(int CashReceiptId)
        {
            var objRepo = new LNSM_CashManagementRepository();
            var model = new LNSM_PostPaymentSheet();
            objRepo.GetCashRcptPrintSD(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (LNSM_PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UpdatePrintHtmlSD(FormCollection fc)
        //{
        //    int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
        //    string htmlPrint = fc["htmlPrint"].ToString();
        //    var objRepo = new LNSM_CashManagementRepository();
        //    objRepo.UpdatePrintHtmlSD(CashReceiptId, htmlPrint);
        //    return Json(objRepo.DBResponse.Status, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateCashReceiptPDFSD(FormCollection fc)
        {
            try
            {
                var pages = new string[2];
                var type = fc["type"].ToString();
                var id = fc["id"].ToString();
                pages[0] = fc["page"].ToString();
                pages[1] = fc["npage"].ToString();
                var fileName = id + ".pdf";
                var ImgLeft = Server.MapPath("~/Content/Images/CWCPDF.PNG");
                var ImgRight = Server.MapPath("~/Content/Images/SwachhBharat-Logo.png");
                pages[0] = fc["page"].ToString().Replace("IMGLeft", ImgLeft).Replace("IMGRight", ImgRight);               
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.HeadOffice = this.HeadOffice;
                    rh.HOAddress = this.HOAddress;
                    rh.ZonalOffice = this.ZonalOffice;
                    rh.ZOAddress = this.ZOAddress;
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkSDReceiptReport(LNSM_BulkReceiptReport ObjBulkReceiptReport)
        {
            LNSM_CashManagementRepository ObjRR = new LNSM_CashManagementRepository();
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


        [HttpGet]
        public ActionResult CashReceiptSDList(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetCashReceiptSDList(ReceiptNo);
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            lstCashReceipt = (List<LNSM_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("CashReceiptSDList", lstCashReceipt);
        }

        [HttpGet]
        public ActionResult GetLoadMoreCashReceiptSDList(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMoreCashReceiptSDList(ReceiptNo, Page);
            List<LNSM_CashReceiptModel> lstCashReceipt = new List<LNSM_CashReceiptModel>();
            if (objCR.DBResponse.Data != null)
            {
                lstCashReceipt = (List<LNSM_CashReceiptModel>)objCR.DBResponse.Data;
            }
            return Json(lstCashReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Cheque Return

        [HttpGet]
        public ActionResult ChequeReturn()
        {            
            return PartialView();
        }

        [HttpGet]
        public JsonResult LoadChequeParty(string PartyCode, int Page)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.GetPartyDetail("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByChequePartyCode(string PartyCode)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.GetPartyDetail(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetChequeDetail(int PartyId)
        {

            LNSM_CashManagementRepository objChe = new LNSM_CashManagementRepository();
            List<dynamic> objImp2 = new List<dynamic>();
            objChe.GetChequeNo(PartyId);
            if (objChe.DBResponse.Data != null)
            {
                ((List<LNSM_ChequeDetail>)objChe.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { Id = item.Id, Cheque = item.Cheque });
                });
            }

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchChequeNo(int partyid, string ChequeNo)
        {
            LNSM_CashManagementRepository objImport = new LNSM_CashManagementRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            objImport.GetChequeDetail(partyid, ChequeNo);

            if (objImport.DBResponse.Data != null)
            {
                ((List<LNSM_ChequeDetail>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { Id = item.Id, SdNo = item.Cheque });
                });
            }

            return Json(objImp2, JsonRequestBehavior.AllowGet);
           
        }




        [HttpGet]
        public JsonResult GetChequeDetails(string ChequeNo)
        {
            try
            {

                LNSM_CashManagementRepository objCh = new LNSM_CashManagementRepository();
                objCh.GetChequeDetails(ChequeNo);
                return Json(objCh.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddChequeReturn(LNSM_ChequeReturn m)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var objRepo = new LNSM_CashManagementRepository();
                    objRepo.AddChequeReturn(m.PartyId, m.ChequeReturnDate, m.SdNo, Convert.ToString(m.Balance), m.ChequeNo, m.DraweeBank, m.Narration, m.ChequeDate, m.Amount, m.AdjustedBalance);
                    return Json(objRepo.DBResponse, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                    var Err = new { Statua = -1, Messgae = "Error" };
                    return Json(Err, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new { Status = 0, Message = "Some error occurs !!" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public ActionResult ListOfChequeReturn(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository objER = new LNSM_CashManagementRepository();
            List<LNSM_ChequeReturn> lstChequeReturn = new List<LNSM_ChequeReturn>();
            objER.GetAllChequeReturn(ReceiptNo);
            if (objER.DBResponse.Data != null)
                lstChequeReturn = (List<LNSM_ChequeReturn>)objER.DBResponse.Data;
            return PartialView(lstChequeReturn);
        }

        [HttpGet]
        public ActionResult GetLoadMoreChequeReturnList(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMoreChequeReturnList(ReceiptNo, Page);
            List<LNSM_ChequeReturn> lstChequeReturn = new List<LNSM_ChequeReturn>();
            if (objCR.DBResponse.Data != null)
            {
                lstChequeReturn = (List<LNSM_ChequeReturn>)objCR.DBResponse.Data;
            }
            return Json(lstChequeReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateChequePDF(FormCollection fc)
        {
            try
            {
                var pages = new string[1];
                pages[0] = fc["page"].ToString();
                var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/AddMoneyReceipt/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                pages[0] = pages[0].Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Data = fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }


        #endregion

        #region Refund From SD

        [HttpGet]
        public ActionResult RefundFromPDA()
        {

            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            return PartialView();
        }

        [HttpGet]
        public ActionResult SDRefundList(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetSDRefundList(ReceiptNo);
            List<LNSM_SDRefundList> lstSDR = new List<LNSM_SDRefundList>();
            lstSDR = (List<LNSM_SDRefundList>)obj.DBResponse.Data;
            return PartialView(lstSDR);
        }
        [HttpGet]
        public ActionResult GetLoadMoreSDRefundList(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMoreSDRefundList(ReceiptNo, Page);
            List<LNSM_SDRefundList> lstSDR = new List<LNSM_SDRefundList>();
            if (objCR.DBResponse.Data != null)
            {
                lstSDR = (List<LNSM_SDRefundList>)objCR.DBResponse.Data;
            }
            return Json(lstSDR, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewSDRefund(int PdaAcId)
        {
            LNSM_ViewSDRefund ObjSD = new LNSM_ViewSDRefund();
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.ViewSDRefund(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (LNSM_ViewSDRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(LNSM_PDARefundModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    var objRepo = new LNSM_CashManagementRepository();
                    objRepo.RefundFromPDA(m, ((Login)(Session["LoginUser"])).Uid);
                    return Json(objRepo.DBResponse, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                    var Err = new { Statua = -1, Messgae = "Error" };
                    return Json(Err, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new { Status = 0, Message = "Some error occurs !!" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF2(FormCollection fc)
        {

            try
            {
                var pages = new string[1];
                pages[0] = fc["page"].ToString();
                var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/PdaRefundReceipt/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
               
                CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
                LNSM_CashManagementRepository ObjRR = new LNSM_CashManagementRepository();
                ObjRR.getCompanyDetails();
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                HeadOffice = ""; //objCompanyDetails.CompanyName;
                HOAddress = "";//objCompanyDetails.RoAddress;
                ZonalOffice = objCompanyDetails.CompanyName;
                ZOAddress = objCompanyDetails.CompanyAddress;

                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(PdfDirectory + fileName, pages);

                }

                return Json(new { Status = 1, Data = fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetRefundSDReport(string ReceiptNo="")
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            LNSM_ViewSDRefund ObjSD = new LNSM_ViewSDRefund();
            objCR.GetSDRefundDetails(ReceiptNo);
            if (objCR.DBResponse.Status == 1)
            {
                ObjSD = (LNSM_ViewSDRefund)objCR.DBResponse.Data;
                string Path = GenerateRefundSDReport(ObjSD);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });

        }

        [NonAction]
        public string GenerateRefundSDReport(LNSM_ViewSDRefund dt)
        {
            List<string> lstSB = new List<string>();

            StringBuilder html = new StringBuilder();

            html.Append("<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse: collapse;'>");
            html.Append("<thead><tr><th colspan='12' style='text-align:center;vertical-align:bottom;font-size:8pt;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)<br/><span style='border-bottom:1px solid #000;font-weight:600;'>SD Closure<br/><br/></span></th></tr></thead>");

            html.Append("<tbody><tr><td colspan='12'><table style='width:100%;font-size:9pt;'><tbody><tr><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Receipt No.:</b> <span>" + dt.RecieptNo + "</span></td><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Dated :</b> <span>" + dt.ClosureDate + "</span></td>	</tr><tr><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Party Name :</b> <span>" + dt.PartyName + "</span></td><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Opening Balance :</b> <span>" + dt.OpeningAmt + "</span></td></tr><tr><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Refund Amount :</b> <span>" + dt.RefundAmt + "</span></td><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Closing Balance :</b> <span>" + dt.ClosingAmt + "</span></td></tr><tr><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Bank Name :</b> <span>" + dt.BankName + "</span></td><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Branch Name :</b> <span>" + dt.Branch + "</span></td></tr><tr><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Cheque No.:</b> <span>" + dt.ChqNo + "</span></td><td colspan='6' style='padding:5px 7px;border:1px solid #000;'><b>Cheque Date :</b><span>" + dt.ChqDate + "</span></td></tr><tr><td colspan='12' style='padding:5px 7px;border:1px solid #000;'><b>Address:</b> <span>" + dt.PartyAddress + "</span></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr><tr><th colspan='12'>Note : As per request SD A/c. closed.</th></tr> <tr><td colspan='12' style='text-align: right;'><b>Signature :</b> __________________________ <br/><br/> <b>Name of the signatory :</b> __________________________ <br/><br/> <b>Designation :</b> __________________________ </td></tr>");
            html.Append("<tr><td><br/><br/><br/><br/><br/></td></tr>  <tr><td colspan='12'> <b>To,</b> <br/> <span>&nbsp;&nbsp;</span> Manager, <br/> <span>&nbsp;&nbsp;</span> Central Warehousing Corporation, <br/> <span>&nbsp;&nbsp;</span> regional office Delhi</td></tr>");

            html.Append("<tr><td><br/><br/></td></tr>  <tr><th colspan='12'>Copied To :</th></tr>");
            html.Append("<tr><td colspan='12'><table style='width:100%;font-size:9pt;'><tbody><tr><td><br/></td></tr><tr><td width='3%' valign='top'>1.</td><td colspan='2' width='85%' style='line-height:22px;'>" + dt.PartyName + "-" + dt.PartyAddress + "</td></tr></tbody></table></td></tr>");
            html.Append("</tbody></table><br/><br/>");

            lstSB.Add(html.ToString());

            var type = "PdaRefundReceipt";
            var id = "PdaRefundReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
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


        #region Party Wise TDS Deposit

        public ActionResult PartyWiseTDSDeposit(int PartyId = 0, string PartyName = "")
        {
            LNSM_PartyWiseTDSDeposit objPartyWiseTDSDeposit = new LNSM_PartyWiseTDSDeposit();
            var objRepo = new LNSM_CashManagementRepository();

            if (PartyId == 0)
            {
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                    ViewBag.Invoice = null;
            }

            //if (Id > 0)
            //{
            //    objRepo.GetPartyWiseTDSDepositDetails(Id);
            //    if (objRepo.DBResponse.Data != null)
            //    {
            //        objPartyWiseTDSDeposit = (LNSM_PartyWiseTDSDeposit)objRepo.DBResponse.Data;
            //    }
            //}
            else
            {
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                    ViewBag.Invoice = null;

                objRepo.GetPartyWiseTDSRcptDetails(PartyId, PartyName);
                if (objRepo.DBResponse.Data != null)
                {
                    objPartyWiseTDSDeposit = (LNSM_PartyWiseTDSDeposit)objRepo.DBResponse.Data;
                }

                ViewBag.CashReceiptTDSMappingList = JsonConvert.SerializeObject(objPartyWiseTDSDeposit.CertificateMappingList);
                objPartyWiseTDSDeposit.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ViewBag.ServerDate = Utility.GetServerDate();
            }            

            return PartialView(objPartyWiseTDSDeposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPartyWiseTDSDeposit(LNSM_PartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            if (ModelState.IsValid)
            {
                LNSM_CashManagementRepository objER = new LNSM_CashManagementRepository();
                List<LNSM_TDSCertificateMapping> CashReceiptInvDtlsList = (List<LNSM_TDSCertificateMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(objPartyWiseTDSDeposit.CashReceiptInvDtlsHtml, typeof(List<LNSM_TDSCertificateMapping>));

                foreach (var item in CashReceiptInvDtlsList)
                {
                    DateTime dt = DateTime.ParseExact(item.ReceiptDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    item.ReceiptDate = dt.ToString("yyyy-MM-dd");
                }

                objPartyWiseTDSDeposit.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
                objER.AddEditPartyWiseTDSDeposit(objPartyWiseTDSDeposit);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }
       

        [HttpGet]
        public ActionResult ListOfPartyWiseTDSDeposit(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository objER = new LNSM_CashManagementRepository();
            List<LNSM_PartyWiseTDSDeposit> lstPartyWiseTDSDeposit = new List<LNSM_PartyWiseTDSDeposit>();
            objER.GetAllPartyWiseTDSDeposit(ReceiptNo);
            if (objER.DBResponse.Data != null)
                lstPartyWiseTDSDeposit = (List<LNSM_PartyWiseTDSDeposit>)objER.DBResponse.Data;
            return PartialView(lstPartyWiseTDSDeposit);
        }

        [HttpGet]
        public ActionResult GetLoadMorePartyWiseTDSDeposit(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMorePartyWiseTDSDeposit(ReceiptNo, Page);
            List<LNSM_PartyWiseTDSDeposit> lstPartyWiseTDSDeposit = new List<LNSM_PartyWiseTDSDeposit>();
            if (objCR.DBResponse.Data != null)
            {
                lstPartyWiseTDSDeposit = (List<LNSM_PartyWiseTDSDeposit>)objCR.DBResponse.Data;
            }
            return Json(lstPartyWiseTDSDeposit, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeletePartyWiseTDSDeposit(int PartyWiseTDSDepositId)
        {
            LNSM_CashManagementRepository objER = new LNSM_CashManagementRepository();
            if (PartyWiseTDSDepositId > 0)
                objER.DeletePartyWiseTDSDeposit(PartyWiseTDSDepositId);
            return Json(objER.DBResponse);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTDSDepositReport(LNSM_BulkReceiptReport ObjBulkReceiptReport)
        {
            LNSM_CashManagementRepository ObjRR = new LNSM_CashManagementRepository();
            ObjRR.GetTDSBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateTDSDepositReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [NonAction]
        public string GenerateTDSDepositReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);           
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
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>TDS DEPOSIT RECEIPT</label>");
                html.Append("</td></tr>");               

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td colspan='9' width='70%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>TDS Deposit No.</label> <span>" + item.DepositNo + "</span></td>");
                html.Append("<td colspan='3' width='30%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Deposit Date : </label> <span>" + item.DepositDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address : </label><span>" + item.PartyAddress + "</span></td></tr>");

                html.Append("<tr><td colspan='9' width='70%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>Certificate No.</label> <span>" + item.CertificateNo + "</span></td>");
                html.Append("<td colspan='3' width='30%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Certificate Date : </label> <span>" + item.CertificateDate + "</span></td>");
                html.Append("</tr>");

                html.Append("<tr><td colspan='9' width='70%' style = 'font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>Financial Year</label> <span>" + item.FinYarFrom + " - " + item.FinYarTo + "</span></td>");
                html.Append("<td colspan='3' width='30%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>TDS Quarter : </label> <span>" + item.TdsQuarter + "</span></td>");
                html.Append("</tr>");

                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Certificate Amount : </label><span>" + item.CertificateAmount + "</span></td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr>");
                
                //Receipt
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Receipt No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Receipt Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Receipt Amount</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Deposit Amount</th>");
                html.Append("</tr>");              
                html.Append("</thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstContainer.Where(z => z.TdsHdrId == item.Id).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.ReceiptNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.ReceiptDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.ReceiptAmount + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DepositAmount + "</td>");                    
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.DepositAmount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total TDS Deducted :</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt.ToString() + "</td></tr>");

                html.Append("<tr>");
                html.Append("<td colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Amount As Per TDS Certificate :</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.CertificateAmount + "</td></tr>");

                //Total
                html.Append("<tr>");
                html.Append("<td colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Unadjusted Amount :</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalUnadjustedAmount + "</td></tr></tbody></table>");
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


        #region Direct Online Payment
        public ActionResult DirectOnlinePayment()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DirectPaymentVoucher(LNSM_DirectOnlinePayment objDOP)
        {
            LNSM_CashManagementRepository ObjIR = new LNSM_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            objDOP.Area = "DirectOnlinePayment";

            ObjIR.AddDirectPaymentVoucher(objDOP, Uid);
            Session["OrderId"] = objDOP.OrderId;
            log.Info("Response save end");
            objDOP.Name = ((Login)Session["LoginUser"]).Name;
            ObjIR.DBResponse.Data = objDOP;
            return Json(ObjIR.DBResponse);

        }

        public ActionResult DirectOnlinePaymentList(long OrderId = 0)
        {
            List<LNSM_DirectOnlinePayment> lstDOP = new List<LNSM_DirectOnlinePayment>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();

            obj.GetOnlinePayAckList(((Login)(Session["LoginUser"])).Uid, OrderId);
            lstDOP = (List<LNSM_DirectOnlinePayment>)obj.DBResponse.Data;
            return PartialView(lstDOP);
        }

        [HttpPost]
        public JsonResult ConfirmPayment(LNSM_DirectOnlinePayment vm)
        {
            vm.OrderId = Convert.ToInt64(Session["OrderId"].ToString());

            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetOnlineConfirmPayment(Convert.ToDecimal(vm.TotalPaymentAmount), vm.OrderId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Online Payment Against Invoice

        public ActionResult OnlinePaymentAgainstInvoice()
        {
            return PartialView();
        }

        public JsonResult ListOfPendingInvoice(string Type)
        {
            LNSM_CashManagementRepository objcancle = new LNSM_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            objcancle.ListOfPendingInvoice(Uid, Type);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnlinePaymentAgainstInvoice(LNSM_OnlinePaymentAgainstInvoice objDOP)
        {
            LNSM_CashManagementRepository ObjIR = new LNSM_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<LNSM_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
                if (lstInvoiceDetailsList != null)
                {
                    InvoiceListXML = Utility.CreateXML(lstInvoiceDetailsList);
                }
            }
            ObjIR.AddEditOnlinePaymentAgainstInvoice(objDOP, Uid, InvoiceListXML);
            Session["OrderId"] = objDOP.OrderId;
            log.Info("Response save end");
            return Json(ObjIR.DBResponse);

        }

        public ActionResult OnlinePaymentAgainstInvoiceList(string SearchValue)
        {
            List<LNSM_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<LNSM_OnlinePaymentAgainstInvoice>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<LNSM_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            LNSM_CashManagementRepository objIR = new LNSM_CashManagementRepository();
            IList<LNSM_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<LNSM_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<LNSM_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Online Payment Receipt

        public ActionResult OnlinePaymentReceipt()
        {
            return PartialView();
        }

        public ActionResult OnlinePaymentReceiptDetails(string PeriodFrom, string PeriodTo)
        {
            List<LNSM_OnlinePaymentReceipt> lstOPReceipt = new List<LNSM_OnlinePaymentReceipt>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.OnlinePaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<LNSM_OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }


        public JsonResult GeneratedCashReceipt(List<LNSM_OnlinePaymentReceipt> lstOnlinePaymentReceipt)
        {
            string OnlinePaymetXML = "";
            if (lstOnlinePaymentReceipt != null)
            {
              
                    OnlinePaymetXML = Utility.CreateXML(lstOnlinePaymentReceipt);
              
            }
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GeneratedOnlinePaymentReceiptDetails(OnlinePaymetXML, ((Login)(Session["LoginUser"])).Uid);



            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnlinePaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetOnlinePaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptList(int Pages)
        {
            LNSM_CashManagementRepository objIR = new LNSM_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlinePaymentReceiptList("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region Online Payment Receipt Invoice

        public ActionResult OnlinePaymentReceiptAgainstInvoice()
        {
            return PartialView();
        }

        public ActionResult OnlinePaymentReceiptDetailsAgainstInvoice(string PeriodFrom, string PeriodTo)
        {
            List<LNSM_OnlinePaymentReceipt> lstOPReceipt = new List<LNSM_OnlinePaymentReceipt>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<LNSM_OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public JsonResult GeneratedCashReceiptAginstInvoice(List<LNSM_OnlinePaymentReceipt> lstOnlinePaymentReceipt)
        {
            string OnlinePaymetXML = "";
            if (lstOnlinePaymentReceipt != null)
            {

                OnlinePaymetXML = Utility.CreateXML(lstOnlinePaymentReceipt);

            }
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GeneratedOnlinePaymentReceiptDetailsAgainstInvoice(OnlinePaymetXML, ((Login)(Session["LoginUser"])).Uid);



            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
           LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetOnlinePaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlinePaymentReceiptListAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BQR Payment Receipt Invoice

        public ActionResult BQRPaymentReceiptAgainstInvoice()
        {
            return PartialView();
        }

        public ActionResult BQRPaymentReceiptDetailsAgainstInvoice(string PeriodFrom, string PeriodTo)
        {
            List<LNSM_OnlinePaymentReceipt> lstOPReceipt = new List<LNSM_OnlinePaymentReceipt>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<LNSM_OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public JsonResult GeneratedBQRCashReceiptAginstInvoice(List<LNSM_OnlinePaymentReceipt> lstOnlinePaymentReceipt)
        {
            string OnlinePaymetXML = "";
            if (lstOnlinePaymentReceipt != null)
            {

                OnlinePaymetXML = Utility.CreateXML(lstOnlinePaymentReceipt);

            }
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GeneratedOnlinePaymentReceiptDetailsBRQ(OnlinePaymetXML, ((Login)(Session["LoginUser"])).Uid);



            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            LNSM_CashManagementRepository objIR = new LNSM_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetBQRPaymentReceiptListAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Pull
        public async Task<JsonResult> GetAllBQRDataPull()
        {
            LNSM_CashManagementRepository objIR = new LNSM_CashManagementRepository();
            List<OnlinePaymentReceipt> lstInvoiceList = new List<OnlinePaymentReceipt>();
            objIR.BQRPaymentReceiptPullData();
            if (objIR.DBResponse.Status == 1)
            {
                lstInvoiceList = (List<OnlinePaymentReceipt>)objIR.DBResponse.Data;
            }
            foreach (var lstData in lstInvoiceList)
            {
                try
                {
                    var Result = await GetTransactionDetailsBQR(lstData.ReferenceNo);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }



            return Json("");
        }




        public async Task<JsonResult> GetTransactionDetailsBQR(string OrderId)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionStatusEnquiry BQR START");
            LNSM_CashManagementRepository objIR = null;
            var environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            string apiUrl = "";

            log.Info("Environment :" + environment);
            if (environment == "P")
            {
                apiUrl = TransStatusEnqCcAvnApiEndPoints.GetEndpoint("PCcAvnUrl");
            }
            else
            {
                apiUrl = TransStatusEnqCcAvnApiEndPoints.GetEndpoint("TCcAvnUrl");
            }
            log.Info("apiUrl :" + apiUrl);
            log.Info("After url");

            //string certpath = System.Configuration.ConfigurationSettings.AppSettings["QRDSCPATH"].ToString();


            ClsEncryptionDecryption security = new ClsEncryptionDecryption();
            string WorkingKey = "";
            String encMsg = "";
            string MerchantId = "";
            try
            {



                WorkingKey = UPIConfiguration.WorkingKeyBQR;





                string reqString = TransactionJsonFormat.BindCcavnTransactionJson(0, OrderId);
                log.Info("Before Encryption ReqString" + reqString);


                encMsg = ccaCrypto.Encrypt(reqString, WorkingKey);


                log.Info("After Encryption EncMsg" + encMsg);
                //string decMsg = security.Decryption(encMsg, mKey);                

            }
            catch (Exception e)
            {


                string s = e.Message;
                log.Error("Err  :" + s);
            }
            TokenResponse tr = new TokenResponse();

            //X509Certificate2 clientCertificate = new X509Certificate2();
            //clientCertificate.Import(certpath);

            //WebRequestHandler handler = new WebRequestHandler();
            //handler.ClientCertificates.Add(clientCertificate);



            string MsgStatus = "", MsgStatusDescription = "";
            using (var client = new HttpClient())   //handler
            {

                string authQueryUrlParam = "enc_request=" + encMsg + "&access_code=" + UPIConfiguration.AccessCodeBQR + "&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2";
                // Url Connection
                String message = postPaymentRequestToGateway(apiUrl, authQueryUrlParam);
                //Response.Write(message);
                NameValueCollection param = getResponseMap(message);
                String status = "";
                String encResJson = "";
                String ResJson = "";
                if (param != null && param.Count == 2)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        if ("status".Equals(param.Keys[i]))
                        {
                            status = param[i];
                        }
                        if ("enc_response".Equals(param.Keys[i]))
                        {
                            encResJson = param[i];
                            //Response.Write(encResXML);
                        }
                    }
                    if (!"".Equals(status) && status.Equals("0"))
                    {
                        MsgStatus = "Success";
                        MsgStatusDescription = "Success";
                        ResJson = ccaCrypto.Decrypt(encResJson, UPIConfiguration.WorkingKeyBQR);
                        CcAvnResponseJsonModel ccAvnResponse = JsonConvert.DeserializeObject<CcAvnResponseJsonModel>(ResJson);
                        LNSM_CashManagementRepository objCash = new LNSM_CashManagementRepository();
                        objCash.AddPaymentGatewayResponseBQR(ccAvnResponse);


                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        MsgStatus = "Fail";
                        MsgStatusDescription = "Fail";

                    }

                }


                log.Info("After Decryption ResJson :" + ResJson);

            }

            return Json("");

        }
        #endregion

        #region Pull CCAvenue
        public async Task<JsonResult> GetAllCCAvenueDataPull()
        {
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
            List<OnlinePaymentReceipt> lstInvoiceList = new List<OnlinePaymentReceipt>();
            objIR.CCAvenuePaymentReceiptPullData();
            if (objIR.DBResponse.Status == 1)
            {
                lstInvoiceList = (List<OnlinePaymentReceipt>)objIR.DBResponse.Data;
            }
            foreach (var lstData in lstInvoiceList)
            {
                try
                {
                    var Result = await GetTransactionDetailsCCAvenue(lstData.ReferenceNo);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }



            return Json("", JsonRequestBehavior.AllowGet);
        }




        public async Task<JsonResult> GetTransactionDetailsCCAvenue(string OrderId)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionPull CCAvenue START");
            Ppg_CashManagementRepository objIR = null;
            var environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            string apiUrl = "";

            log.Info("Environment :" + environment);
            if (environment == "P")
            {
                apiUrl = TransStatusEnqCcAvnApiEndPoints.GetEndpoint("PCcAvnUrl");
            }
            else
            {
                apiUrl = TransStatusEnqCcAvnApiEndPoints.GetEndpoint("TCcAvnUrl");
            }
            log.Info("apiUrl :" + apiUrl);
            log.Info("After url");

            //string certpath = System.Configuration.ConfigurationSettings.AppSettings["QRDSCPATH"].ToString();


            ClsEncryptionDecryption security = new ClsEncryptionDecryption();
            string WorkingKey = "";
            String encMsg = "";
            string MerchantId = "";
            try
            {



                WorkingKey = UPIConfiguration.WorkingKey;





                string reqString = TransactionJsonFormat.BindCcavnTransactionJson(0, OrderId);
                log.Info("Before Encryption ReqString" + reqString);


                encMsg = ccaCrypto.Encrypt(reqString, WorkingKey);


                log.Info("After Encryption EncMsg" + encMsg);
                //string decMsg = security.Decryption(encMsg, mKey);                

            }
            catch (Exception e)
            {


                string s = e.Message;
                log.Error("Err  :" + s);
            }
            TokenResponse tr = new TokenResponse();

            //X509Certificate2 clientCertificate = new X509Certificate2();
            //clientCertificate.Import(certpath);

            //WebRequestHandler handler = new WebRequestHandler();
            //handler.ClientCertificates.Add(clientCertificate);



            string MsgStatus = "", MsgStatusDescription = "";
            using (var client = new HttpClient())   //handler
            {

                string authQueryUrlParam = "enc_request=" + encMsg + "&access_code=" + UPIConfiguration.AccessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2";
                // Url Connection
                String message = postPaymentRequestToGateway(apiUrl, authQueryUrlParam);
                //Response.Write(message);
                NameValueCollection param = getResponseMap(message);
                String status = "";
                String encResJson = "";
                String ResJson = "";
                if (param != null && param.Count == 2)
                {
                    for (int i = 0; i < param.Count; i++)
                    {
                        if ("status".Equals(param.Keys[i]))
                        {
                            status = param[i];
                        }
                        if ("enc_response".Equals(param.Keys[i]))
                        {
                            encResJson = param[i];
                            //Response.Write(encResXML);
                        }
                    }
                    if (!"".Equals(status) && status.Equals("0"))
                    {
                        MsgStatus = "Success";
                        MsgStatusDescription = "Success";
                        ResJson = ccaCrypto.Decrypt(encResJson, UPIConfiguration.WorkingKey);
                        log.Error("Responce Json CCAvenue Pull" + ResJson);
                        CcAvnResponseJsonModel ccAvnResponse = JsonConvert.DeserializeObject<CcAvnResponseJsonModel>(ResJson);
                        Ppg_CashManagementRepository objCash = new Ppg_CashManagementRepository();
                        if (ccAvnResponse.error_code == "")
                        {
                            objCash.AddPaymentGatewayResponseCCAvenue(ccAvnResponse);
                        }



                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        MsgStatus = "Fail";
                        MsgStatusDescription = "Fail";

                    }

                }


                log.Info("After Decryption ResJson :" + ResJson);

            }

            return Json("");

        }


        private string postPaymentRequestToGateway(String queryUrl, String urlParam)
        {

            String message = "";
            try
            {
                StreamWriter myWriter = null;// it will open a http connection with provided url
                WebRequest objRequest = WebRequest.Create(queryUrl);//send data using objxmlhttp object
                objRequest.Method = "POST";
                //objRequest.ContentLength = TranRequest.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(urlParam);//send data
                myWriter.Close();//closed the myWriter object

                // Getting Response
                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();//receive the responce from objxmlhttp object 
                using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    message = sr.ReadToEnd();
                    //Response.Write(message);
                }
            }
            catch (Exception exception)
            {
                Console.Write("Exception occured while connection." + exception);
            }
            return message;

        }

        private NameValueCollection getResponseMap(String message)
        {
            NameValueCollection Params = new NameValueCollection();
            if (message != null || !"".Equals(message))
            {
                string[] segments = message.Split('&');
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);
                    }
                }
            }
            return Params;
        }





        private string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public string Data_Hex_Asc(string Data)
        {
            string decString = Data;// "0123456789";
            byte[] bytes = Encoding.Default.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        private JObject ReadJsonFromFile()
        {
            JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@"D:\CWC Work\CwcExim\Content\SandboxInvoice.json"));
            JObject o2 = null;
            // read JSON directly from a file
            using (StreamReader file = System.IO.File.OpenText(@"D:\CWC Work\CwcExim\Content\SandboxInvoice.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                o2 = (JObject)JToken.ReadFrom(reader);
            }
            return o1;

        }
        #endregion


        #region Calling Service 

        public void GenerateCashReceiptService()
        {
            try
            {

                string executablePath = ConfigurationManager.AppSettings["CashReceiptServiceKey"].ToString();

                string arguments = "GenerateCashReceipt";

                /*ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                Process process = new Process();
                process.StartInfo = processStartInfo;*/
                //process.Start();

                //Task.Run(() => process.Start());

                // process.WaitForExit();





                Process process = new Process();

                // Set the StartInfo properties
                process.StartInfo.FileName = executablePath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                // Start the process as a background task
                Task.Run(() => process.Start());

            }
            catch (Exception ex)
            {
                log.Error("Generate Cash Receipt Service Error :" + ex.Message + "\r\n" + ex.StackTrace);
            }



        }
        #endregion

        #region OnAccount  Add Money
        [HttpGet]
        public ActionResult AddMoneyToOA()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            LNSM_CashManagementRepository ObjRepo = new LNSM_CashManagementRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            LNSMOASearchEximTraderData obj = new LNSMOASearchEximTraderData();
            ObjRepo.OAGetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((LNSMOASearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((LNSMOASearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new LNSMOAAddMoney();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new LNSMOAReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
                 new SelectListItem { Text = "BANK GURANTEE", Value = "BANKGURANTEE"},
                 new SelectListItem { Text = "POS", Value = "POS"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView("AddMoneyToOA", model);
        }

        [HttpGet]
        public ActionResult ListOfOAAddMoney(string ReceiptNo = "")
        {
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetAllOAAddMoneyList(ReceiptNo, 0);
            IEnumerable<LNSMOAAddMoney> lstAddMoneyToOA = (IEnumerable<LNSMOAAddMoney>)objRepo.DBResponse.Data;
            if (lstAddMoneyToOA != null)
            {
                return PartialView(lstAddMoneyToOA);
            }
            else
            {
                return PartialView(new List<LNSMOAAddMoney>());
            }
        }

        [HttpGet]
        public JsonResult LoadMoreListOfOAAddMoney(string ReceiptNo = "", int Page = 0)
        {
            var objRepo = new LNSM_CashManagementRepository();
            objRepo.GetAllOAAddMoneyList(ReceiptNo, Page);
            IEnumerable<LNSMOAAddMoney> lstAddMoneyToOA = (IEnumerable<LNSMOAAddMoney>)objRepo.DBResponse.Data;
            if (lstAddMoneyToOA != null)
            {
                return Json(lstAddMoneyToOA, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(lstAddMoneyToOA, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditOAAddMoney(LNSMOAAddMoney ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                LNSM_CashManagementRepository ObjSDR = new LNSM_CashManagementRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSD.Uid = ObjLogin.Uid;
                ObjSDR.AddOAAddMoney(ObjSD, xml);
                ModelState.Clear();
                return Json(ObjSDR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        [HttpGet]

        public JsonResult OALoadEximtradeList(string PartyCode, int Page)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.OAGetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult OASearchByPartyCode(string PartyCode)
        {
            LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
            objRepo.OASearchByPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Adjustment of Advance Payment/ Credit Note/ TDS 

        [HttpGet]
        public ActionResult AdjustmentCashReceipt(int PartyId = 0, string PartyName = "")
        {
            LNSM_AdjustmentCashReceiptModel ObjCashReceipt = new LNSM_AdjustmentCashReceiptModel();

            var objRepo = new LNSM_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                    ViewBag.Invoice = null;                            

                ViewBag.AdjCashReceiptInvoiveMappingList = null;
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");                
                ViewBag.ServerDate = Utility.GetServerDate();
                return PartialView(ObjCashReceipt);
            }
            else
            {
                objRepo.ListOfPayeeForPage("", 0);

                if (objRepo.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPayee = Jobject["lstPayee"];
                    ViewBag.PayeeState = Jobject["State"];
                }
                else
                    ViewBag.Invoice = null;

                objRepo.GetAdjustmentCashRcptDetails(PartyId, PartyName);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (LNSM_AdjustmentCashReceiptModel)objRepo.DBResponse.Data;                    
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                ViewBag.AdjCashReceiptInvoiveMappingList = JsonConvert.SerializeObject(ObjCashReceipt.AdjCashReceiptInvoiveMappingList);
                ViewBag.AdjustInvoiveMappingList = JsonConvert.SerializeObject(ObjCashReceipt.AdjustmentMappingList);
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");               
                ViewBag.ServerDate = Utility.GetServerDate();
                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAdjustmentCashReceipt(LNSM_AdjustmentCashReceiptModel ObjCashReceipt)
        {
            List<LNSM_AdjustmentCashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<LNSM_AdjustmentCashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<LNSM_AdjustmentCashReceiptInvoiveMapping>));
            List<LNSM_AdjustmentCashReceiptMapping> AdjustCashReceiptInvDtlsList = (List<LNSM_AdjustmentCashReceiptMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.AdjustReceiptInvDtlsHtml, typeof(List<LNSM_AdjustmentCashReceiptMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.ReceiptDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.ReceiptDate = dt.ToString("yyyy-MM-dd"); 
            }

            foreach (var item in AdjustCashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.DocDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.DocDate = dt.ToString("yyyy-MM-dd");
            }

            ObjCashReceipt.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
            ObjCashReceipt.AdjustReceiptInvDtlsHtml = Utility.CreateXML(AdjustCashReceiptInvDtlsList);

            //var xml = "";
            
            //xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
            

            var objRepo = new LNSM_CashManagementRepository();
            objRepo.AddAdjustmentCashReceipt(ObjCashReceipt);
            //if (objRepo.DBResponse.Status == 1)
            //{
            //    GenerateCashReceiptService();
            //}

            return Json(objRepo.DBResponse);
        }

        //[HttpGet]
        //public JsonResult CashReceiptPrint(int CashReceiptId)
        //{
        //    var objRepo = new LNSM_CashManagementRepository();
        //    var model = new PostPaymentSheet();
        //    objRepo.GetCashRcptPrint(CashReceiptId);
        //    if (objRepo.DBResponse.Data != null)
        //    {
        //        model = (PostPaymentSheet)objRepo.DBResponse.Data;
        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost, ValidateInput(false)]
        //public JsonResult GenerateCashReceiptPDF(FormCollection fc)
        //{
        //    try
        //    {
        //        var pages = new string[2];
        //        var type = fc["type"].ToString();
        //        var id = fc["id"].ToString();
        //        pages[0] = fc["page"].ToString();
        //        pages[1] = fc["npage"].ToString();
        //        var fileName = id + ".pdf";
        //        var ImgLeft = Server.MapPath("~/Content/Images/CWCPDF.PNG");
        //        var ImgRight = Server.MapPath("~/Content/Images/SwachhBharat-Logo.png");
        //        pages[0] = fc["page"].ToString().Replace("IMGLeft", ImgLeft).Replace("IMGRight", ImgRight);
        //        string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
        //        if (!Directory.Exists(PdfDirectory))
        //            Directory.CreateDirectory(PdfDirectory);
        //        using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //        {
        //            rh.HeadOffice = this.HeadOffice;
        //            rh.HOAddress = this.HOAddress;
        //            rh.ZonalOffice = this.ZonalOffice;
        //            rh.ZOAddress = this.ZOAddress;
        //            rh.GeneratePDF(PdfDirectory + fileName, pages);
        //        }
        //        return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
        //    }

        //}

        //[HttpGet]
        //public JsonResult PayeeSearchByPartyCode(string PartyCode)
        //{
        //    LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
        //    objRepo.ListOfPayeeForPage(PartyCode, 0);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult LoadPayeeList(string PartyCode, int Page)
        //{
        //    LNSM_CashManagementRepository objRepo = new LNSM_CashManagementRepository();
        //    objRepo.ListOfPayeeForPage(PartyCode, Page);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult AdjustCashReceiptList(string ReceiptNo = "")
        {
            LNSM_CashManagementRepository obj = new LNSM_CashManagementRepository();
            obj.GetAdjustCashReceiptList(ReceiptNo);
            List<LNSM_AdjustmentCashReceiptModel> lstCashReceipt = new List<LNSM_AdjustmentCashReceiptModel>();
            lstCashReceipt = (List<LNSM_AdjustmentCashReceiptModel>)obj.DBResponse.Data;
            return PartialView("AdjustmentCashReceiptList", lstCashReceipt);
        }

        [HttpGet]
        public ActionResult GetLoadMoreAdjustCashReceiptList(string ReceiptNo = "", int Page = 0)
        {
            LNSM_CashManagementRepository objCR = new LNSM_CashManagementRepository();
            objCR.GetLoadMoreAdjustCashReceiptList(ReceiptNo, Page);
            List<LNSM_AdjustmentCashReceiptModel> lstCashReceipt = new List<LNSM_AdjustmentCashReceiptModel>();
            if (objCR.DBResponse.Data != null)
            {
                lstCashReceipt = (List<LNSM_AdjustmentCashReceiptModel>)objCR.DBResponse.Data;
            }
            return Json(lstCashReceipt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkAdjustReceiptReport(LNSM_BulkReceipt ObjBulkReceiptReport)
        {
            LNSM_CashManagementRepository ObjRR = new LNSM_CashManagementRepository();
            ObjRR.GetAdjustBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateAdjustBulkReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [NonAction]
        public string GenerateAdjustBulkReceiptReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> objReceipt = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            int i = 0;
            objReceipt.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();

                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>ADJUSTMENT CASH RECEIPT</label>");
                html.Append("</td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12' width='100%'><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='6' width='50%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='6' width='50%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address : </label><span>" + item.PartyAddress + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td colspan='12'><hr/></td></tr><tr><td colspan='12'>");

                //Invoice Nos and Amounts
                html.Append("<table style='margin-bottom: 10px; border: 1px solid #000; border-bottom: 0; width:100%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Document No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Document Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Document Type</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 150px;'>Adjust Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                decimal totalAmt = 0;
                lstInvoice.ToList().ForEach(item1 =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item1.ReceiptNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item1.ReceiptDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item1.ReceiptType + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item1.Amount + "</td>");
                    html.Append("</tr>");
                    totalAmt = totalAmt + item1.Amount;
                });

                //Total
                html.Append("<tr>");
                html.Append("<td colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Open Document Amount</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalAmt + "</td></tr></tbody></table>");

                //Banks
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Doc Type</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Doc No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 100px;'>Doc Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px; width: 150px;'>Adjust Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                decimal totalpaymentreceiptAmt = 0;
                lstContainer.ToList().ForEach(item2 =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item2.DocType + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item2.DocNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item2.DocDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item2.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + item2.Amount;
                });

                html.Append("<tr>");
                html.Append("<td colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr>");


                html.Append("</tbody></table></td></tr>");
                html.Append("</tbody></table>");

                //Total
                
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
            var type = "bulkadjustreport";
            var id = "BulkAdjustReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
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
    }
}
