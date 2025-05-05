using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using EinvoiceLibrary;
using CwcExim.Areas.Import.Models;
using System.Threading.Tasks;

namespace CwcExim.Areas.Auction.Controllers
{
    public class HDB_AuctionInvoiceController : BaseController
    {
        // GET: Auction/HDB_AuctionInvoice
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public decimal EffectVersion { get; set; }

        public string EffectVersionLogoFile { get; set; }

        public HDB_AuctionInvoiceController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Hdb_ReportRepository ObjRR = new Hdb_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            EffectVersion = Convert.ToDecimal(objCompanyDetails.EffectVersion);
            EffectVersionLogoFile = objCompanyDetails.VersionLogoFile.ToString();

        }

        [HttpGet]
        public ActionResult CreateAuctionInvoice()
        {
            HDB_AuctionInvoiceRepository objRepo = new HDB_AuctionInvoiceRepository();
            objRepo.GetBIDListForInvoice(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<HDB_EMDReceived>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;
            HDB_EMDReceived ObjEMDReceived = new HDB_EMDReceived();
            for (var i = 0; i < 6; i++)
            {
                ObjEMDReceived.CashReceiptDetail.Add(new CashReceipt());
            }

            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "NEFT/RTGS", Value = "NEFT/RTGS"},
                new SelectListItem { Text = "DD", Value = "DD"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;

            return PartialView();
        }
        [HttpPost]
        public JsonResult AddEditAssesmentInvoice(HDB_AuctionInvoice objAuctionInvoice)
        {
            try
            {
                foreach (var item in objAuctionInvoice.CashReceiptDetail.Where(o => o.Amount > 0).ToList())
                {
                    item.PaymentMode = String.IsNullOrWhiteSpace(item.PaymentMode) ? "###" : item.PaymentMode;
                    item.DraweeBank = String.IsNullOrWhiteSpace(item.DraweeBank) ? "###" : item.DraweeBank;
                    item.InstrumentNo = String.IsNullOrWhiteSpace(item.InstrumentNo) ? "###" : item.InstrumentNo;

                    if (String.IsNullOrWhiteSpace(item.Date))
                    {
                        item.Date = "###";
                    }
                    else
                    {
                        DateTime dt = DateTime.ParseExact(item.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        String ReceiptDate = dt.ToString("yyyy-MM-dd");
                        item.Date = ReceiptDate;
                    }

                }
                var xml = Utility.CreateXML(objAuctionInvoice.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
                xml = xml.Replace(">###<", "><");
                HDB_AuctionInvoiceRepository objER = new HDB_AuctionInvoiceRepository();
                // IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objAuctionInvoice.PaymentSheetModelJson);
                // string XML = Utility.CreateXML(PostPaymentChargeList);
                objAuctionInvoice.PaymentSheetModelJson = xml;
                objER.AddEditInvoice(objAuctionInvoice);
                return Json(objER.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }



        [HttpGet]
        public ActionResult CreateContPaymentSheet()
        {
            HDB_AuctionInvoiceRepository objRepo = new HDB_AuctionInvoiceRepository();
            objRepo.GetAssessmentInvoiceList(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<HDB_AuctionInvoice>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBidDetails(int BIDId)
        {
            HDB_AuctionInvoice objAuctionInvoice = new HDB_AuctionInvoice();
            HDB_AuctionInvoiceRepository objRepo = new HDB_AuctionInvoiceRepository();
            objRepo.GetBIDDetailsForInvoice(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((HDB_AuctionInvoice)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBidDetailsByInvoice(int BIDId)
        {
            List<HDB_AuctionInvoice> objAuctionInvoice = new List<HDB_AuctionInvoice>();
            HDB_AuctionInvoiceRepository objRepo = new HDB_AuctionInvoiceRepository();
            objRepo.GetAssessmentInvoiceListByID(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((List<HDB_AuctionInvoice>)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAuctionCharges(int BIDId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, string CustomDuty, string OT, string ValuersCharge, string AuctionCharge, string MISC, string AssesmentType, decimal AuctionHandling,string ExportUnder)
        {


            HDB_AuctionInvoiceRepository objChrgRepo = new HDB_AuctionInvoiceRepository();
            objChrgRepo.GetAuctionCharges(BIDId, InvoiceId, InvoiceDate, FreeUpTo, HSNCode, Convert.ToDecimal(CustomDuty == "" ? "0" : CustomDuty), Convert.ToDecimal(OT == "" ? "0" : OT), Convert.ToDecimal(ValuersCharge == "" ? "0" : ValuersCharge), Convert.ToDecimal(AuctionCharge == "" ? "0" : AuctionCharge), Convert.ToDecimal(MISC == "" ? "0" : MISC), AssesmentType, AuctionHandling,ExportUnder);
            return Json(objChrgRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddEditInvoice(HDB_AuctionInvoice objAuctionInvoice)
        {
            try
            {
                HDB_AuctionInvoiceRepository objER = new HDB_AuctionInvoiceRepository();
                IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objAuctionInvoice.PaymentSheetModelJson);
                string XML = Utility.CreateXML(PostPaymentChargeList);
                objAuctionInvoice.PaymentSheetModelJson = XML;
                objER.AddEditAuctionInvoice(objAuctionInvoice);
                return Json(objER.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }



        public JsonResult GetBulkInvoiceReport(HDB_AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            string FilePath = "";
            string ModuleName = "";
            HDB_AuctionRepository ObjRR = new HDB_AuctionRepository();
            Login objLogin = (Login)Session["LoginUser"];
            ObjBulkInvoiceReport.InvoiceModule = "AUC";
            ModuleName = "Auction";
            //ObjBulkInvoiceReport.All = "All";
            ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
            FilePath = GeneratingAssessmentSheetReportAuction((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);

            return Json(new { Status = 1, Data = FilePath });
            //}
            //else
            //    return Json(new { Status = -1, Data = "No Record Found." });
        }

        [NonAction]
        public string GeneratingBulkPDFforAuction(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            #region for Bid Details

            StringBuilder html = new StringBuilder();
            int i = 1;
            /*Header Part*/
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            /* html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
             html.Append("<tr><td colspan='12'>");
             html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
             html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
             html.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Hyderabad-37</span><br/><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label></td></tr>");
             html.Append("</tbody></table>");
             html.Append("</td></tr>");
             html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 12px; text-transform: uppercase; padding-bottom: 10px;'>");
             html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
             html.Append("</thead></table>");*/
            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwccfs@gmail.com</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + lstInvoice[0].irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(lstInvoice[0].SignedQRCode)) + "'/> </td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");
            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;border-top: 1px solid #000;padding-top:5px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + lstInvoice[0].InvoiceNo + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;border-top: 1px solid #000;padding-top:5px;'><label style='font-weight: bold;'>Tax Invoice Date : </label> <span>" + lstInvoice[0].InvoiceDate + "</span></td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;'><label style='font-weight: bold;'>Party Name : </label><span>" + lstInvoice[0].Party + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;'><label style='font-weight: bold;'>State :</label> <span>" + lstInvoice[0].StateName + "</span> </td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;border-bottom: 1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + lstInvoice[0].Address + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;border-bottom: 1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Party GST No :</label> <span>" + lstInvoice[0].GstNo + "</span></td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;padding-bottom:5px;'><label style='font-weight: bold;'>Under :</label> <span>" + lstInvoice[0].ExportUnder + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>Supply Type :</label> <span>" + lstInvoice[0].SupplyType + "</span></td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;border-bottom:1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Place Of Supply :</label> <span>" + lstInvoice[0].StateName + "(" + lstInvoice[0].PartyStateCode + ")</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 7pt;border-bottom:1px solid #000;'><label style='font-weight: bold;'>Is Service :</label> <span>Yes</span></td></tr>");


            html.Append("</tbody></table></td></tr>");
            html.Append("</tbody></table>");

            html.Append("<table cellspacing='0' cellpadding='6' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td colspan='12'><table style='width:100%;' cellspacing='0'><tbody>");
            html.Append("<tr><td colspan='4' width='30%' style='font-size: 9pt;'><label style='font-weight: bold;'>Auction Asm No. :</label> <span></span></td>");
            html.Append("<td colspan='2' width='20%' style='font-size: 9pt;'><label style='font-weight: bold;'>Date : </label> <span>" + lstInvoice[0].AuctionDate + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 9pt;'><label style='font-weight: bold;'>Bid No. : </label> <span>" + lstInvoice[0].BidNo + "</span></td>");
            html.Append("<td colspan='2' width='20%' style='font-size: 9pt;'><label style='font-weight: bold;'>Shed no. : </label> <span>" + lstInvoice[0].GodownName + "</span></td></tr>");
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><td colspan='12' width='100%' style='font-size: 9pt;'><label style='font-weight: bold;'>S/Line :</label> <span>" + lstInvoice[0].SLine + "</span></td></tr>");
            html.Append("</tbody></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 1px solid #000; font-size:8pt;'>");
            html.Append("<thead>");
            html.Append("<tr><th><span><br/></span></th></tr>");
            html.Append("<tr><th colspan='12' style='font-size:9pt;'>A. Auction Details :</th></tr>");
            html.Append("<tr><th style='text-align:left;padding:10px;border:1px solid #000;border-bottom:0;width:3%;'>Sr. No.</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Bid Amount</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>GST% & <br/> HSN Code</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>CGST</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>SGST</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>IGST</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>CESS</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Total Dues</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>EMD Adjustment</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Adv.Adjustment</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Auction Charges</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Amount Receipt No. & Date</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Net Amount Payable</th></tr>");
            html.Append("</thead>");

            html.Append("<tbody><tr>");
            html.Append("<td style='text-align:left;border:1px solid #000;border-bottom:0;padding:10px;'>1</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].BidAmount + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].GSTPer + "/" + lstInvoice[0].HSNCode + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].CGSTAmount + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].SGSTAmount + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].IGSTAmount + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].Cess + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].TotalDue + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].EmdAdjust + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].AdvAdjust + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].AuctionCharges + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].EmdRcvdNo + "/" + lstInvoice[0].EmdRcvdDate + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].NetAmountPay + "</td>");
            html.Append("</tr></tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody>");
            html.Append("<tr><th><span><br/></span></th></tr>");
            html.Append("<tr><th style='border-bottom: 1px solid #000; border-top:1px solid #000; font-size: 8pt; text-align: left; padding: 5px;' colspan='12'>Total Invoice (In Word) :" + objCurr.changeCurrencyToWords(lstInvoice[0].NetAmountPay.ToString("0")) + "</th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><th><span><br/><br/></span></th></tr>");
            html.Append("<tr><th style='font-size: 8pt;'>RECEIPT/DD No. " + lstInvoice[0].InstrumentNo + "</th></tr>");
            html.Append("<tr><th><span><br/></span></th></tr>");
            html.Append("<tr><th style='font-size: 8pt; text-align: right;'>For Central Warehousing Corporation<br/>Authorized Signatories</th></tr>");
            html.Append("</tbody></table>");

            /***************/

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));




            #endregion






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
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.Version = EffectVersion;
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {



            HDB_AuctionRepository objPpgRepo = new HDB_AuctionRepository();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForProductSell(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    if (ERes.Status == 1)
                    {
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
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
                    Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                    irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    var dt = DateTime.Now;
                    Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                    Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                    objQR.Irn = ERes;
                    objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                    objQR.iss = "NIC";
                    objQR.ItemCnt = irnb2cobj.ItemCnt;
                    objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                    objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                    objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                    objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                    objQR.IFSC = irnb2cobj.IFSC;
                    objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                    objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                    objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                    objQR.IGST = irnb2cobj.IGST;
                    objQR.CESS = irnb2cobj.CESS;

                    objQR.CGST = irnb2cobj.CGST;
                    objQR.SGST = irnb2cobj.SGST;
                    obj.Data = (Ppg_QrCodeData)objQR;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(obj);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                IrnModel irnModelObj = new IrnModel();
                irnModelObj.DocumentDate = irnb2cobj.DocDt;
                irnModelObj.DocumentNo = irnb2cobj.DocNo;
                irnModelObj.DocumentType = irnb2cobj.DocTyp;
                irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                var dt = DateTime.Now;
                Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                objQR.Irn = ERes;
                objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                objQR.iss = "NIC";
                objQR.ItemCnt = irnb2cobj.ItemCnt;
                objQR.MainHsnCode = irnb2cobj.MainHsnCode;

                objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                objQR.IFSC = irnb2cobj.IFSC;
                objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                objQR.IGST = irnb2cobj.IGST;
                objQR.CESS = irnb2cobj.CESS;

                objQR.CGST = irnb2cobj.CGST;
                objQR.SGST = irnb2cobj.SGST;
                obj.Data = (Ppg_QrCodeData)objQR;
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(obj);
                IrnResponse objERes = new IrnResponse();
                objERes.irn = ERes;
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);

            }


            return Json(objPpgRepo.DBResponse);
        }


        //Assessment Sheet
        [HttpPost]
        public JsonResult GetAssessmentSheetReport(string InvoiceNo)
        {

            string FilePath = "";

            HDB_AuctionInvoiceRepository ObjRR = new HDB_AuctionInvoiceRepository();
            Login objLogin = (Login)Session["LoginUser"];


            //ObjBulkInvoiceReport.All = "All";
            ObjRR.GenericBulkInvoiceDetailsForPrint(InvoiceNo);
            FilePath = GeneratingBulkPDFforAuction((DataSet)ObjRR.DBResponse.Data, "AUCTION SALES INVOICE", "");
           // FilePath = GeneratingAssessmentSheetReportAuction((DataSet)ObjRR.DBResponse.Data, "AUCTION SALES INVOICE", "");
            return Json(new { Status = 1, Data = FilePath });
            //}
            //else
            //    return Json(new { Status = -1, Data = "No Record Found." });
        }

        [NonAction]
        public string GeneratingAssessmentSheetReportAuction(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstBidderDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
            List<dynamic> lstContainerBidderDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);

            List<string> lstSB = new List<string>();

            #region for Bid Details
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                HDB_AuctionRepository rep = new HDB_AuctionRepository();
                HDBSDBalancePrint objSDBalance = new HDBSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (HDBSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                int i = 1;
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwccfs@gmail.com</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + lstInvoice[0].irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(lstInvoice[0].SignedQRCode)) + "'/> </td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                             
                html.Append("<tr><td colspan='3'>");

                html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td><h2 style='font-size: 13px;text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td><table style='width:100%;' cellspacing='0'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + lstBidderDetails[0].InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>Tax Invoice Date : </label> <span>" + lstBidderDetails[0].InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;'><label style='font-weight: bold;'>Auction Sales Invoice : </label><span>" + lstBidderDetails[0].AssessmentSheet + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>Date :</label> <span>" + lstBidderDetails[0].AssesmentDate + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;'><label style='font-weight: bold;'>Party Name :</label> <span>" + lstBidderDetails[0].Party + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>State :</label> <span>" + lstBidderDetails[0].StateName + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;'><label style='font-weight: bold;'>Party Address :</label> <span>" + lstBidderDetails[0].Address + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>Shed No/Godown No. :</label> <span>" + lstBidderDetails[0].GodownName + "</span></td></tr>");
                html.Append("<tr><td colspan='12' width='100%' style='font-size: 7pt;'><label style='font-weight: bold;'>Party GST :</label> <span>" + lstBidderDetails[0].GstNo + "</span></td></tr>");


                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;padding-bottom:5px;'><label style='font-weight: bold;'>Under :</label> <span>" + lstInvoice[0].ExportUnder + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;'><label style='font-weight: bold;'>Supply Type :</label> <span>" + lstInvoice[0].SupplyType + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 7pt;border-bottom:1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Place Of Supply :</label> <span>" + lstInvoice[0].PartyState + "(" + lstInvoice[0].PartyStateCode + ")</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 7pt;border-bottom:1px solid #000;'><label style='font-weight: bold;'>Is Service :</label> <span>Yes</span></td></tr>");


                html.Append("<tr><td colspan='12' width='100%' style='font-size: 7pt;border-bottom:1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>BID No. :</label> <span>" + lstBidderDetails[0].BidNo + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt;'>A. PARTICULARS OF CONTAINER :</th></tr>");

                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Date of Auction</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Free Dt Upto</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Delivery Valid Upto</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 7%;'>No Week</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                i = 1;
                lstContainerBidderDetails.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Arrival + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.FreeUpto + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Days + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Weeks + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");



                html.Append("<tr><th style='text-align: left; font-size: 7pt;'>B. PARTICULARS OF CARGO :</th></tr>");

                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>IGM No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>OBL No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>BOE No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>BOE Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>CIF/FOB Value</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Duty</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Cargo Description</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>No Of Pkg.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Total Gr Wt.(In Kg)</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Total Area</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Type Of Cargo</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                i = 1;
                lstContainer.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.OBL + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.SB + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Boe + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.BoeDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CIFFOB + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Duty + "</td>");

                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CargoDescription + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Noofpkg + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Weight + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Area + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CommodityType + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin:0;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>SR No.</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 120px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");

                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                decimal Taxable = 0M;
                decimal CGSTPer = 0M;
                decimal CGSTAmt = 0M;
                decimal SGSTPer = 0M;
                decimal SGSTAmt = 0M;
                decimal IGSTPer = 0M;
                decimal IGSTAmt = 0M;
                decimal Total = 0M;
                decimal Rate = 0M;

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + i.ToString() + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>" + data.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>" + data.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 30px;'>" + data.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 120px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    Rate = Rate + Convert.ToDecimal(data.Taxable.ToString("0.00"));
                    Taxable = Taxable + Convert.ToDecimal(data.Taxable.ToString("0.00"));
                    CGSTPer = CGSTPer + Convert.ToDecimal(data.CGSTPer.ToString("0"));
                    CGSTAmt = CGSTAmt + Convert.ToDecimal(data.CGSTAmt.ToString("0.00"));
                    SGSTPer = SGSTPer + Convert.ToDecimal(data.SGSTPer.ToString("0"));
                    SGSTAmt = SGSTAmt + Convert.ToDecimal(data.SGSTAmt.ToString("0.00"));
                    IGSTPer = IGSTPer + Convert.ToDecimal(data.IGSTPer.ToString("0"));
                    IGSTAmt = IGSTAmt + Convert.ToDecimal(data.IGSTAmt.ToString("0.00"));
                    Total = Total + Convert.ToDecimal(data.Total.ToString("0.00"));
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                //html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 150px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: center; width: 130px;'>" + Taxable + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 7pt; text-align: center; width: 120px;'>" + Total + "</th></tr><tr>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: center; width: 50px;'>" + CGSTAmt + "</th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: center; width: 50px;'>" + SGSTAmt + "</th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: center; width: 50px;'>" + IGSTAmt + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Round Up :</th>");
                //html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 150px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 7pt; text-align: center; width: 120px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Total(Rounded) :</th>");
                //html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 150px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 7pt; text-align: center; width: 120px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 30px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : <span style='font-size:7pt;'>" + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</span></th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th><span><br/><br/></span></th></tr>");
                html.Append("<tr><th colspan='7' width='70%' style='font-size: 7pt;'>RECEIPT/DD No.</th> <th colspan='3' width='30%' style='font-size: 7pt;'>Mode of Receipt.</th></tr>");
                html.Append("<tr><th><span><br/></span></th></tr>");
                html.Append("<tr><th colspan='12' style='font-size: 7pt; text-align: right;'>For Central Warehousing Corporation<br/>Authorized Signatories</th></tr>");
                html.Append("</tbody></table>");

                /****************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                lstSB.Add(html.ToString());
            });

            #endregion








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
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.Version = EffectVersion;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;



        }


        public ActionResult GetAssessmentList()
        {
            HDB_AuctionInvoiceRepository obj = new HDB_AuctionInvoiceRepository();
            obj.GetAssessmentList();
            List<HDB_AuctionInvoice> lstInvoice = new List<HDB_AuctionInvoice>();
            lstInvoice = ((List<HDB_AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetAssessmentList", lstInvoice);
        }


        public ActionResult GetInvoiceList()
        {
            HDB_AuctionInvoiceRepository obj = new HDB_AuctionInvoiceRepository();
            obj.GetInvoiceList();
            List<HDB_AuctionInvoice> lstInvoice = new List<HDB_AuctionInvoice>();
            lstInvoice = ((List<HDB_AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetInvoiceList", lstInvoice);
        }

        public ActionResult BulkInvoicePrint()
        {
            HDB_AuctionRepository _Ar = new HDB_AuctionRepository();
            List<Models.HDB_PartyDetails> lstPartyDetails = new List<Models.HDB_PartyDetails>();
            _Ar.GetBidPartyAndDetails();
            if (_Ar.DBResponse.Data != null)
            {
                lstPartyDetails = (List<Models.HDB_PartyDetails>)_Ar.DBResponse.Data;
            }

            ViewBag.PaymentParty = Newtonsoft.Json.JsonConvert.SerializeObject(lstPartyDetails);
            return PartialView();
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType)
        {
            HDB_AuctionInvoiceRepository ObjRR = new HDB_AuctionInvoiceRepository();
            List<invoiceLIst> LstinvoiceLIst = new List<invoiceLIst>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetInvoiceList(FromDate, ToDate, invoiceType);//, objLogin.Uid
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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReportForAuction(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            HDB_AuctionRepository Obj = new HDB_AuctionRepository();
            HDB_AuctionInvoiceRepository ObjRR = new HDB_AuctionInvoiceRepository();


            HDB_AuctionInvoiceViewModel ObjBulk = new HDB_AuctionInvoiceViewModel();
            ObjBulk.InvoiceModule = ObjBulkInvoiceReport.InvoiceModule;
            ObjBulk.InvoiceModuleName = ObjBulkInvoiceReport.InvoiceModuleName;
            ObjBulk.InvoiceNumber = ObjBulkInvoiceReport.InvoiceNumber;
            ObjBulk.PartyId = Convert.ToString(ObjBulkInvoiceReport.PartyId);
            ObjBulk.PeriodFrom = ObjBulkInvoiceReport.PeriodFrom;
            ObjBulk.PeriodTo = ObjBulkInvoiceReport.PeriodTo;
            ObjBulk.All = ObjBulkInvoiceReport.All;


            Login objLogin = (Login)Session["LoginUser"];
            string FilePath = "";


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




                    foreach (string Mod in lstDistModule)
                    {
                        //lstModule.Where(x => x.Module == Mod).ToList().ForEach(itemInv =>
                        //{


                        ObjBulkInvoiceReport.InvoiceModule = Mod;
                        ModuleName = "Auction";
                        ObjBulkInvoiceReport.All = "All";
                        Obj.GenericBulkInvoiceDetailsForPrint(ObjBulk);
                        FilePath = GeneratingAssessmentSheetReportAuction((DataSet)Obj.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);

                        //});

                    }
                    if (FilePath == null || FilePath == "")
                    {
                        return Json(new { Status = -1, Data = "No Record Found." });

                    }
                    else
                    {
                        return Json(new { Status = 1, Data = FilePath });
                    }

                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }

            else
            {
                // List<PpgInvoiceGate> PpgInvoiceGateList = ObjRR.GetBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid
                Obj.GenericBulkInvoiceDetailsForPrint(ObjBulk);//, objLogin.Uid

                if (Obj.DBResponse.Status == 1)
                {

                    DataSet ds = (DataSet)Obj.DBResponse.Data;
                    //string FilePath = GeneratingBulkPDF(PpgInvoiceGateList, ObjBulkInvoiceReport.InvoiceModuleName);


                    ObjBulkInvoiceReport.All = "";
                    FilePath = GeneratingAssessmentSheetReportAuction(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);



                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
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

        public JsonResult Download()
        {
            String session = Session.SessionID;
            //Ppg_ReportCWCController obj = new Ppg_ReportCWCController();
            //////int CurrentFileID = Convert.ToInt32(FileID); 
            string fileSavePath = "";
            fileSavePath = Server.MapPath("~/Docs/All/") + Session.SessionID;
            var filesCol = GetFile(fileSavePath).ToList();
            string FileList = "";
            //string FileList = "/Docs/All/" + Session.SessionID + "/";
            for (int i = 0; i < filesCol.Count; i++)
            {
                FileList = FileList + "/Docs/All/" + Session.SessionID + "/" + (filesCol[i].FileName) + ",";
            }
            var ObjResult = new { Status = 1, Message = FileList };
            return Json(ObjResult);
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
        #region Bulk IRN Generation

        [HttpGet]
        public ActionResult BulkIRNAuctionGeneration()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBulkIrnDetails()
        {
            HDB_AuctionRepository objPpgRepo = new HDB_AuctionRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (HDB_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            //Ppg_CWCImportController objPpgIrn = new Ppg_CWCImportController();

            try
            {

                var invoiceData = JsonConvert.DeserializeObject<HDB_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            //  var result = await GetIRNForBulkInvoice(item.InvoiceNo, item.SupplyType);
                            var result = await GetIRNForYardInvoicee(item.InvoiceNo, item.SupplyType);
                        }
                        /*  if (item.InvoiceType == "C")
                          {
                              var result1 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                          }
                          if (item.InvoiceType == "D")
                          {
                              var result2 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
                          }*/
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
        public async Task<JsonResult> GetIRNForYardInvoicee(String InvoiceNo, string SupplyType)
        {



            HDB_AuctionRepository objPpgRepo = new HDB_AuctionRepository();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForProductSell(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    if (ERes.Status == 1)
                    {
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
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
                    Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                    irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                    if (irnb2cobj.pa == "" || irnb2cobj.mtid == "")
                    {
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        var dt = DateTime.Now;
                        Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                        Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                        objQR.Irn = ERes;
                        objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                        objQR.iss = "NIC";
                        objQR.ItemCnt = irnb2cobj.ItemCnt;
                        objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                        objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                        objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                        objQR.IFSC = irnb2cobj.IFSC;
                        objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                        objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                        objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                        objQR.IGST = irnb2cobj.IGST;
                        objQR.CESS = irnb2cobj.CESS;

                        objQR.CGST = irnb2cobj.CGST;
                        objQR.SGST = irnb2cobj.SGST;
                        obj.Data = (Ppg_QrCodeData)objQR;
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        objresponse = Eobj.GenerateB2cQRCode(obj);
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
                        idata.ver = irnb2cobj.ver.ToString();
                        idata.mode = irnb2cobj.mode;
                        idata.mode = irnb2cobj.mode;
                        idata.tr = irnb2cobj.tr;
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
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        var dt = DateTime.Now;
                        Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                        Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                        objQR.Irn = ERes;
                        objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                        objQR.iss = "NIC";

                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        objresponse = Eobj.GenerateB2cQRCode(idata);
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
                Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                if (irnb2cobj.pa == "" || irnb2cobj.mtid == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    var dt = DateTime.Now;
                    Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                    Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                    objQR.Irn = ERes;
                    objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                    objQR.iss = "NIC";
                    objQR.ItemCnt = irnb2cobj.ItemCnt;
                    objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                    objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                    objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                    objQR.IFSC = irnb2cobj.IFSC;
                    objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                    objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                    objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                    objQR.IGST = irnb2cobj.IGST;
                    objQR.CESS = irnb2cobj.CESS;

                    objQR.CGST = irnb2cobj.CGST;
                    objQR.SGST = irnb2cobj.SGST;
                    obj.Data = (Ppg_QrCodeData)objQR;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(obj);
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
                    idata.ver = irnb2cobj.ver.ToString();
                    idata.mode = irnb2cobj.mode;
                    idata.mode = irnb2cobj.mode;
                    idata.tr = irnb2cobj.tr;
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
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    var dt = DateTime.Now;
                    Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                    Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                    objQR.Irn = ERes;
                    objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                    objQR.iss = "NIC";

                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
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


        #endregion

    }
}