using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;

namespace CwcExim.Areas.Auction.Controllers
{
    public class Dnd_EMDReceivedController : BaseController
    {
        // GET: Auction/EMDReceived

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public Dnd_EMDReceivedController()
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
        public ActionResult EMDReceived(int InvoiceId = 0, string InvoiceNo = "")
        {
            
            Dnd_BidderRepository objBidderRepository = new Dnd_BidderRepository();
            objBidderRepository.GetBidderForAuction(0);
            ViewBag.BidderList = (objBidderRepository.DBResponse.Data);
            Dnd_EMDReceived ObjEMDReceived = new Dnd_EMDReceived();
            Dnd_AuctionRepository objRepo = new Dnd_AuctionRepository();
            objRepo.GetBIDList(Convert.ToInt32(Session["BranchId"]));
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((Dnd_EMDReceived)objRepo.DBResponse.Data).BIDDetail;
            else
                ViewBag.EMDReceivedList = null;
            ObjEMDReceived.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
            // ObjCashReceipt.InvoiceDate = DateTime.Today.ToString("dd/MM/yyyy");
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

            return PartialView(ObjEMDReceived);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEMDReceived(Dnd_EMDReceived ObjEMDReceived)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in ObjEMDReceived.CashReceiptDetail.Where(o => o.Amount > 0).ToList())
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
                var xml = Utility.CreateXML(ObjEMDReceived.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
                xml = xml.Replace(">###<", "><");
                var objRepo = new Dnd_AuctionRepository();
                objRepo.SaveEMDReceived(ObjEMDReceived, xml);
                return Json(objRepo.DBResponse);
            }
            else
            {
                string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err, JsonRequestBehavior.DenyGet);
            }

        }

        public ActionResult GetBidDetails(int BIDId)
        {
            Dnd_EMDReceived objEMDReceived = new Dnd_EMDReceived();
            Dnd_AuctionRepository objRepo = new Dnd_AuctionRepository();
            objRepo.GetBIDDetails(BIDId);
            if (objRepo.DBResponse.Data != null)
                objEMDReceived = ((Dnd_EMDReceived)objRepo.DBResponse.Data);
            else
                objEMDReceived = null;
            return Json(objEMDReceived, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEMDReceivedList() 
        {
            Dnd_AuctionRepository ObjAR = new Dnd_AuctionRepository();
            ObjAR.GetEMDReceivedList();
            List<Dnd_EMDReceived> LstEMDReceived = new List<Dnd_EMDReceived>();

            if (ObjAR.DBResponse.Data != null)
            {
                LstEMDReceived = (List<Dnd_EMDReceived>)ObjAR.DBResponse.Data;
            }
            return View("EMDReceivedList", LstEMDReceived);
        }

        [HttpPost]
        public JsonResult GenerateCashReceiptPDF(string RecNo)
        {
            try
            {
                Dnd_AuctionRepository ObjAR = new Dnd_AuctionRepository();
                ObjAR.GetEMDReceived(RecNo);
                List<Dnd_AucBidFinalizationHdr> LstEMDReceived = new List<Dnd_AucBidFinalizationHdr>();
                string path = "";
                if (ObjAR.DBResponse.Data != null)
                {
                    LstEMDReceived = (List<Dnd_AucBidFinalizationHdr>)ObjAR.DBResponse.Data;
                    path = GenerateBulkReceiptReport(LstEMDReceived);
                }

                return Json(new { Status = 1, Message = path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.AllowGet);
            }

        }



        [NonAction]
        public string GenerateBulkReceiptReport(List<Dnd_AucBidFinalizationHdr> vm)
        {
          
            int i = 0;
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            CurrencyToWordINR objCurr = new CurrencyToWordINR();




            //Page Header
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</ h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>ICD Patparganj - Delhi</ span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>EMD RECEIPT</label>");
                html.Append("</td></tr>");

                //html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                //html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + vm[0].EmdReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + vm[0].BidDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + vm[0].Party + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>BID No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>BID Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Advance Amount</th>");
                html.Append("</tr></thead><tbody>");

            //Loop

           
                html.Append("<tr>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + vm[0].BidNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + vm[0].RefDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + vm[0].EmdAmount + "</td>");
                html.Append("</tr>");

              



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
               // decimal totalpaymentreceiptAmt = 0;
               // i = 1;
               foreach(var ii in vm)
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + ii.PaymentMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + ii.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + ii.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + ii.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + ii.Amount + "</td>");
                    html.Append("</tr>");
                    

                }
            
                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + vm.Sum(x=>x.Amount) + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(Convert.ToString(vm.Sum(x => x.Amount))) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'></td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
          
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
    }
}