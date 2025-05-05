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

namespace CwcExim.Areas.Auction.Controllers
{
    public class Dnd_AuctionInvoiceController : BaseController
    {
        // GET: Auction/AuctionInvoice

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public Dnd_AuctionInvoiceController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;

        }

        [HttpGet]
        public ActionResult CreateAuctionInvoice()
        {
            Dnode_AuctionRepository objRepo = new Dnode_AuctionRepository();
            objRepo.GetBIDListForInvoice(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<Dnd_EMDReceived>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;
            Dnd_EMDReceived ObjEMDReceived = new Dnd_EMDReceived();
            for (var i = 0; i < 6; i++)
            {
                ObjEMDReceived.CashReceiptDetail.Add(new Dnd_CashReceipt());
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
        public JsonResult AddEditAssesmentInvoice(Dnd_AuctionInvoice objAuctionInvoice)
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
                Dnode_AuctionRepository objER = new Dnode_AuctionRepository();
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
            Dnode_AuctionRepository objRepo = new Dnode_AuctionRepository();
            objRepo.GetAssessmentInvoiceList(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<Dnd_AuctionInvoice>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBidDetails(int BIDId)
        {
            Dnd_AuctionInvoice objAuctionInvoice = new Dnd_AuctionInvoice();
            Dnode_AuctionRepository objRepo = new Dnode_AuctionRepository();
            objRepo.GetBIDDetailsForInvoice(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((Dnd_AuctionInvoice)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBidDetailsByInvoice(int BIDId)
        {
            List<Dnd_AuctionInvoice> objAuctionInvoice = new List<Dnd_AuctionInvoice>();
            Dnode_AuctionRepository objRepo = new Dnode_AuctionRepository();
            objRepo.GetAssessmentInvoiceListByID(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((List<Dnd_AuctionInvoice>)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAuctionCharges(int BIDId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, string CustomDuty, string OT, string ValuersCharge, string AuctionCharge, string MISC)
        {


            Dnode_AuctionRepository objChrgRepo = new Dnode_AuctionRepository();
            objChrgRepo.GetAuctionCharges(BIDId, InvoiceId, InvoiceDate, FreeUpTo, HSNCode, Convert.ToDecimal(CustomDuty == "" ? "0" : CustomDuty), Convert.ToDecimal(OT == "" ? "0" : OT), Convert.ToDecimal(ValuersCharge == "" ? "0" : ValuersCharge), Convert.ToDecimal(AuctionCharge == "" ? "0" : AuctionCharge), Convert.ToDecimal(MISC == "" ? "0" : MISC));
            return Json(objChrgRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddEditInvoice(Dnd_AuctionInvoice objAuctionInvoice)
        {
            try
            {
                Dnode_AuctionRepository objER = new Dnode_AuctionRepository();
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



        public JsonResult GetBulkInvoiceReport(Dnd_AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            string FilePath = "";
            string ModuleName = "";
            Dnd_AuctionRepository ObjRR = new Dnd_AuctionRepository();
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

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS Dronagiri-Mumbai</span><br/><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 12px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;border-top: 1px solid #000;padding-top:5px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + lstInvoice[0].InvoiceNo + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;border-top: 1px solid #000;padding-top:5px;'><label style='font-weight: bold;'>Tax Invoice Date : </label> <span>" + lstInvoice[0].InvoiceDate + "</span></td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;'><label style='font-weight: bold;'>Party Name : </label><span>" + lstInvoice[0].Party + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;'><label style='font-weight: bold;'>State :</label> <span>" + lstInvoice[0].StateName + "</span> </td></tr>");
            html.Append("<tr><td colspan='8' width='70%' style='font-size: 8pt;border-bottom: 1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + lstInvoice[0].Address + "</span></td>");
            html.Append("<td colspan='4' width='30%' style='font-size: 8pt;border-bottom: 1px solid #000;padding-bottom:5px;'><label style='font-weight: bold;'>Party GST No :</label> <span>" + lstInvoice[0].GstNo + "</span></td></tr>");
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
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Total Dues</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>EMD Paid Earlier</th>");
            html.Append("<th style='text-align:center;padding:10px;border:1px solid #000;border-bottom:0;border-left:0;width:8%;'>Adv.Paid Earlier</th>");
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
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].TotalDue + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].EMDPaid + "</td>");
            html.Append("<td style='text-align:center;border:1px solid #000;border-bottom:0;border-left:0;padding:10px;'>" + lstInvoice[0].AdvancePaid + "</td>");
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
            html.Append("<tr><th><span><br/><br/></span></th></tr>");
            html.Append("<tr><th style='font-size: 8pt; text-align: right;'>Signature of the bidder</th></tr>");
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
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }




        //Assessment Sheet
        [HttpPost]
        public JsonResult GetAssessmentSheetReport(string InvoiceNo)
        {

            string FilePath = "";

            Dnode_AuctionRepository ObjRR = new Dnode_AuctionRepository();
            Login objLogin = (Login)Session["LoginUser"];


            //ObjBulkInvoiceReport.All = "All";
            ObjRR.GenericBulkInvoiceDetailsForPrint(InvoiceNo);
            FilePath = GeneratingBulkPDFforAuction((DataSet)ObjRR.DBResponse.Data, "AUCTION ASSESSMENT SHEET", "");

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
            lstInvoice.ToList().ForEach(item =>
            {
                Dnd_ReportRepository rep = new Dnd_ReportRepository();
                DndSDBalancePrint objSDBalance = new DndSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (DndSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                int i = 1;
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("<br />" + InvoiceModuleName);
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='12'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Assessment Sheet No</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].AssessmentSheet + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].Party + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].Address + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].GstNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>BID No.</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].BidNo + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:9pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Tax Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Date</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].AssesmentDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].StateName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Shed No/Godown No.</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].GodownName + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>A. PARTICULARS OF CONTAINER :</b> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Date of Auction</th>");
                // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Free Dt. From</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Free Dt Upto</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Delivery Valid Upto</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:center;'>No Week</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                i = 1;
                lstContainerBidderDetails.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Arrival + "</td>");
                    //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.FeeDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.FreeUpto + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Days + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + elem.Weeks + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>B. PARTICULARS OF CARGO :</b> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>IGM No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>OBL No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>BOE No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>BOE Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>CIF/FOB Value</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Duty</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Cargo Description</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>No Of Pkg.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Total Gr Wt.(In Kg)</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Total Area</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Type Of Cargo</th>");

                html.Append("</tr></thead><tbody>");
                /*************/
                i = 1;
                lstContainer.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.OBL + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.SB + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Boe + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.BoeDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CIFFOB + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Duty + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CargoDescription + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Noofpkg + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Weight + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Area + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + elem.CommodityType + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 13px;'>Container Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr>");
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
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
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Rate.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0") + "</td></tr>");
                    Rate = Rate + Convert.ToDecimal(data.Rate.ToString("0"));
                    Taxable = Taxable + Convert.ToDecimal(data.Taxable.ToString("0"));
                    CGSTPer = CGSTPer + Convert.ToDecimal(data.CGSTPer.ToString("0"));
                    CGSTAmt = CGSTAmt + Convert.ToDecimal(data.CGSTAmt.ToString("0"));
                    SGSTPer = SGSTPer + Convert.ToDecimal(data.SGSTPer.ToString("0"));
                    SGSTAmt = SGSTAmt + Convert.ToDecimal(data.SGSTAmt.ToString("0"));
                    IGSTPer = IGSTPer + Convert.ToDecimal(data.IGSTPer.ToString("0"));
                    IGSTAmt = IGSTAmt + Convert.ToDecimal(data.IGSTAmt.ToString("0"));
                    Total = Total + Convert.ToDecimal(data.Total.ToString("0"));
                    i = i + 1;
                });
                html.Append("<tr><th colspan='4' style='border-right: 1px solid #000; font-size: 10px; text-align: left; width: 100px;'>TOTAL</th>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + Taxable + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='font-size: 10px; text-align: center; width: 100px;'>" + Total + "</td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table></td></tr>");
                html.Append("<tr>");
                html.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th><span><br/><br/></span></th></tr>");
                html.Append("<tr><th colspan='7' width='70%' style='font-size: 8pt;'>RECEIPT/DD No.</th> <th colspan='3' width='30%' style='font-size: 8pt;'>Mode of Receipt.</th></tr>");
                html.Append("<tr><th><span><br/></span></th></tr>");
                html.Append("<tr><th colspan='12' style='font-size: 8pt; text-align: right;'>For Central Warehousing Corporation<br/>Authorized Signatories</th></tr>");
                html.Append("<tr><th><span><br/><br/></span></th></tr>");
                html.Append("<tr><th colspan='12' style='font-size: 8pt; text-align: right;'>Signature of the bidder</th></tr>");
                html.Append("</tbody></table>");

                /****************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;



        }


        public ActionResult GetAssessmentList()
        {
            Dnode_AuctionRepository obj = new Dnode_AuctionRepository();
            obj.GetAssessmentList();
            List<Dnd_AuctionInvoice> lstInvoice = new List<Dnd_AuctionInvoice>();
            lstInvoice = ((List<Dnd_AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetAssessmentList", lstInvoice);
        }


        public ActionResult GetInvoiceList()
        {
            Dnode_AuctionRepository obj = new Dnode_AuctionRepository();
            obj.GetInvoiceList();
            List<Dnd_AuctionInvoice> lstInvoice = new List<Dnd_AuctionInvoice>();
            lstInvoice = ((List<Dnd_AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetInvoiceList", lstInvoice);
        }

        public ActionResult BulkInvoicePrint()
        {
            Dnd_AuctionRepository _Ar = new Dnd_AuctionRepository();
            List<Models.Dnd_PartyDetails> lstPartyDetails = new List<Models.Dnd_PartyDetails>();
            _Ar.GetBidPartyAndDetails();
            if (_Ar.DBResponse.Data != null)
            {
                lstPartyDetails = (List<Models.Dnd_PartyDetails>)_Ar.DBResponse.Data;
            }

            ViewBag.PaymentParty = Newtonsoft.Json.JsonConvert.SerializeObject(lstPartyDetails);
            return PartialView();
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType)
        {
            Dnode_AuctionRepository ObjRR = new Dnode_AuctionRepository();
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
        public JsonResult GetBulkInvoiceReportForAuction(Dnd_BulkInvoiceReport ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            Dnd_AuctionRepository Obj = new Dnd_AuctionRepository();
            Dnode_AuctionRepository ObjRR = new Dnode_AuctionRepository();


            Dnd_AuctionInvoiceViewModel ObjBulk = new Dnd_AuctionInvoiceViewModel();
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

    }
}