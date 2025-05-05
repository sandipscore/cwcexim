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


namespace CwcExim.Controllers
{
    public class ChnEximTdrReportController : Controller
    {
        // GET: ChnEximTdrReport
        
            private string HeadOffice { get; set; }
            private string HOAddress { get; set; }
            public string ZonalOffice { get; set; }
            public string ZOAddress { get; set; }
            
            public ActionResult SdAccountStatement()
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
                    Login ObjLogin = (Login)Session["LoginUser"];
                    CHN_ExtReportRepository ObjRR = new CHN_ExtReportRepository();
                    ObjRR.GetPDAStatement(ObjLogin.Uid, ObjSDStatement.Month, ObjSDStatement.Year);
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
                    string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Eximtrader/SDStatement/";
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
                    //using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                    //{
                    //    Rh.GeneratePDF(LocalDirectory + FileName, Pages);
                    //}
                    return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Eximtrader/SDStatement/" + FileName });
                }
                catch
                {
                    return Json(new { Status = 0, Message = "Error" });
                }
            }
        #region Bulk Invoice
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
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
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
        public ActionResult EximTdrBlkInvcReport()
        {
             
            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType)
        {
            CHN_ExtReportRepository ObjRR = new CHN_ExtReportRepository();
            List<invoiceLIst> LstinvoiceLIst = new List<invoiceLIst>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetInvoiceList(FromDate, ToDate, invoiceType,objLogin.Uid);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                //LstinvoiceLIst = (List<invoiceLIst>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }

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
            lstInvoice.ToList().ForEach(item =>
            {
                CHN_ExportRepository rep = new CHN_ExportRepository();
                CHNBTTCargoDet objCargoDet = new CHNBTTCargoDet();
                rep.GetCargoDetBTTById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCargoDet = (CHNBTTCargoDet)rep.DBResponse.Data;
                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>EXPORT BTT PAYMENT SHEET</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");


                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CFS Madhavvaram-Chennai</span>");
                //html.Append("<br />EXPORT BTT PAYMENT SHEET");
                //html.Append("</td></tr>");
                html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:9pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%; font-size:9pt; border:1px solid #333; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody><tr>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + item.ExporterImporterName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> " + item.CHAName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b>" + item.ShippingLineName + "</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                //html.Append("<span>" + item.PartyName + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>Invoice Generated By :</label> <span>" + item.PaymentMode + "</span></td></tr>");
                //html.Append("</tbody> ");
                //html.Append("</table></td></tr>");

                //html.Append("<tr><td><hr/></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%; font-size:8pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });

                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:8pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:5%;'>Entry No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>SB No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>Commodity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>No of pkg</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>GR WT</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:center; width:10%;'>FOB</th>");
                html.Append("</tr></thead><tbody>");
                i = 1;

                // objCargoDet.ForEach(elem =>
                // {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:5%;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'> " + objCargoDet.ShippingBillNo.ToString() + " </td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.ShippingBillDate.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.CommodityName.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.NoOfUnits.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.GrossWeight.ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; text-align:center; width:10%;'> " + objCargoDet.Fob.ToString() + "</td>");
                html.Append("</tr>");
                //   i = i + 1;
                // });
                html.Append("</tbody></table></td></tr>");


                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px; margin-top: 20px;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>Total</th></tr><tr>");
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
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 150px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style=' border: 1px solid #000;border-bottom:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 10px; text-align: center; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th></tr>");
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        public JsonResult GetBulkInvoiceReport(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            // if (ObjBulkInvoiceReport.InvoiceNumber == null)
            // {
            //   ObjBulkInvoiceReport.InvoiceNumber = "";
            // }
            CHN_ReportRepository ObjRR = new CHN_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            // if (ObjBulkInvoiceReport.InvoiceNumber == null)
            //  {
            ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
            //  }
            //  else
            //  {
            // List<PpgInvoiceGate> PpgInvoiceGateList = ObjRR.GetBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid
            //     ObjRR.InvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid
            //  }
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                //string FilePath = GeneratingBulkPDF(PpgInvoiceGateList, ObjBulkInvoiceReport.InvoiceModuleName);
                string FilePath = "";
                switch (ObjBulkInvoiceReport.InvoiceModule)
                {
                    case "EXPSEALCHEKING":
                        FilePath = GeneratingBulkPDFforChnExpSealChecking(ds, ObjBulkInvoiceReport.InvoiceModuleName);
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
                    default:
                        FilePath = GeneratingBulkPDFforCHNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                        break;
                }

                return Json(new { Status = 1, Data = FilePath });
            }
            else
                return Json(new { Status = -1, Data = "No Record Found." });

        }
        [NonAction]
        public string GeneratingBulkPDFforCHNAll(DataSet ds, string InvoiceModuleName)
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
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        public string GeneratingBulkPDFforExportLoadedCont(DataSet ds, string InvoiceModuleName, string All)
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
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>LOADED CONTAINER PAYMENT SHEET</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr><tr><td><hr /></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");

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
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");

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

                html.Append("<table style=' border: 1px solid #000; border-bottom:0; width:100%;'cellspacing='0' cellpadding='5'> ");
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='font-size: 12px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 12px;'>Email - " + objCompany[0].EmailAddress + "</label>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>Bond Advance Paymentsheet</label>");
                html.Append("</td>");
                html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr><tr><td colspan='12'><hr /></td></tr>");
                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 12px;margin-top: 10px;'>SAC No :" + item.StuffingReqNo + "</b></th>");

                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<td colspan='6' width='50%' style='font-size:12px;'><b>SAC validity :</b> From <u>" + elem.SacApprovedDate + "</u> to <u>" + elem.SacValidityDate + "</u></td></tr>");
                });

                //html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                //html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                //html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>CFS Code</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Size</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Carting</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>No of Days</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>No of Week</th>");
                //html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                //html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/

                // lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                //{
                //    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + i + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + item.DeliveryDate + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(item.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + Convert.ToDateTime(elem.DeliveryDate)+ "</td>");
                //    html.Append("<td style='border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                //    html.Append("</tr>");
                //    i = i + 1;
                //});
                /***************/
                //html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");

                //html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + item.ShippingLineName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
                //html.Append("<td style='font-size: 12px;'>OBL No :  </td>");
                //html.Append("<td style='font-size: 12px;'>Item No. : " + lstContainer.Where(x => x.InvoiceId == item.InvoiceId).Count().ToString() + "</td>");
                //html.Append("<td style='font-size: 12px;'>BOE No : " + item.BOENo.ToString().TrimEnd(',') + " </td>");
                //html.Append("<td style='font-size: 12px;'>BOE Date : " + item.BOEDate.ToString().TrimEnd(',') + " </td>");
                //html.Append("</tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + item.ExporterImporterName + " </td>");
                //html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>CHA Name : " + item.CHAName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + item.TotalNoOfPackages.ToString() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + item.TotalGrossWt.ToString("0.000") + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) : " + item.TotalSpaceOccupied.ToString("0.000") + "</td>");
                //html.Append("<td></td>");
                //html.Append("<td></td>");
                //html.Append("</tr>");
                //html.Append("</table>");
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
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
                    html.Append("<table style='border-left: 1px solid #000; width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
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

                    html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container/Cargo Charges :</h3> </th></tr>");
                    html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                    html.Append("<thead><tr>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
                    //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
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
                });
                i = 1;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
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
                //html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='font-size: 12px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 12px;'>Email - " + objCompany[0].EmailAddress + "</label>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>Bond Delivery Paymentsheet</label>");
                html.Append("</td>");
                html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + " </h2> </td></tr>");
                html.Append("<tr><td colspan='12'><table style='width:100%; font-size: 12px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='line-height: 26px;'><label style='font-weight: bold;'></label> <span></span></td>");
                html.Append("<td colspan='4' width='30%' style='line-height: 26px;'><label style='font-weight: bold;'>Delivery Date : </label> <span>" + item.DeliveryDate + "</span></td></tr>");

                html.Append("<tr><td colspan='8' width='70%' style='line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr><tr><td colspan='12'><hr style='margin-top:0;margin-bottom:0;' /></td></tr>");
                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 12px;margin-top: 8px;'>Bond No :" + item.StuffingReqNo + "</b></th>");
                html.Append("<td colspan='6' width='50%' style='font-size:12px; text-align: right'><label width='15%'><b>SAC validity :</b></label> From <u>" + lstContainer[0].SacApprovedDate + "</u> to <u>" + lstContainer[0].SacValidityDate + "</u></td></tr>");
                html.Append("<tr><th colspan='2' width='20%' style='text-align: left; font-size: 12px;margin-top: 15px;' valign='top'>Storage Period :</th>");
                html.Append("<td colspan='10' width='80%'><table style='width:100%; font-size:12px;' cellspacing='0' cellpadding='0'><tbody>");

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
                html.Append("<table style='width:100%; font-size:8pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Bond No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BondNo.ToString().TrimEnd(',') + " </td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Bond Date</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BondDate.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOL No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BOLNo.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOL Date</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BOLDate.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BOENo.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].BOEDate.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>AWB No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].AWBNo.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>AWB Date</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].AWBDate.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Godown No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer[0].GodownName.ToString().TrimEnd(',') + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='border-left: 1px solid #000; width:100%; font-size:8pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='40%'>Ex-BOE No.</td><td>:</td><td colspan='6' width='60%'>" + lstContainer[0].ExBOENo.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>Ex-BOE Date.</td><td>:</td><td colspan='6' width='60%'>" + lstContainer[0].ExBOEDate.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>Importer</td><td>:</td><td colspan='6' width='60%'>" + lstInvoice[0].ExporterImporterName.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>CHA Name</td><td>:</td><td colspan='6' width='60%'>" + lstInvoice[0].CHAName.ToString().TrimEnd(',') + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>CIF Value</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIF)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>DUTY</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>Total</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIF) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>No Of Pkg</td><td>:</td><td colspan='6' width='60%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='40%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 12px;margin-top: 10px;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>MISCELLANEOUS INVOICE</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
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

                //html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");

                /*html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");*/
                /*************/
                /*Container Bind*/
                /*int i = 1;
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
                });*/
                /***************/
                /*html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");*/

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
                int i = 1; Decimal totamt = 0;
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>EXPORT PAYMENT SHEET</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; border-bottom: 1px solid #000; padding-bottom:5px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; border-bottom: 1px solid #000; padding-bottom:5px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 12px; margin-top: 10px;'>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 12px; margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
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
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 12px;margin-top: 20px;'>Container/Cargo Charges :</h3> </th></tr><tr><td>");
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

                html.Append("<table style=' border: 1px solid #000;border-bottom:0; width:100%;'cellspacing='0' cellpadding='5'> ");
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

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; font-size: 11px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
                html.Append("</td>");

                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0;'>Authorised Signatory</p>");
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        [NonAction]
        public string GeneratingBulkPDFforChnExpSealChecking(DataSet ds, string InvoiceModuleName)
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
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyGSTNo == "" ? item.PartyGSTNo : item.PartyGSTNo.Substring(0, 2) + "</span></td></tr>");
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        #endregion
    }
}

