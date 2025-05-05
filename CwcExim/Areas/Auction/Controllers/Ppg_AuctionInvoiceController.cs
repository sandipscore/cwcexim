using CwcExim.Areas.Auction.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Controllers
{
    public class Ppg_AuctionInvoiceController : BaseController
    {
        // GET: Auction/AuctionInvoice

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }

        public Ppg_AuctionInvoiceController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
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
            Ppg_AuctionRepository objRepo = new Ppg_AuctionRepository();
            objRepo.GetBIDListForInvoice(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<EMDReceived>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;
            EMDReceived ObjEMDReceived = new EMDReceived();
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
        public JsonResult AddEditAssesmentInvoice(AuctionInvoice objAuctionInvoice)
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
                Ppg_AuctionRepository objER = new Ppg_AuctionRepository();
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
            Ppg_AuctionRepository objRepo = new Ppg_AuctionRepository();
            objRepo.GetAssessmentInvoiceList(0);
            if (objRepo.DBResponse.Data != null)
                ViewBag.EMDReceivedList = ((List<AuctionInvoice>)objRepo.DBResponse.Data);
            else
                ViewBag.EMDReceivedList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBidDetails(int BIDId)
        {
            AuctionInvoice objAuctionInvoice = new AuctionInvoice();
            Ppg_AuctionRepository objRepo = new Ppg_AuctionRepository();
            objRepo.GetBIDDetailsForInvoice(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((AuctionInvoice)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBidDetailsByInvoice(int BIDId)
        {
            List<AuctionInvoice> objAuctionInvoice = new List<AuctionInvoice>();
            Ppg_AuctionRepository objRepo = new Ppg_AuctionRepository();
            objRepo.GetAssessmentInvoiceListByID(BIDId);
            if (objRepo.DBResponse.Data != null)
                objAuctionInvoice = ((List<AuctionInvoice>)objRepo.DBResponse.Data);
            else
                objAuctionInvoice = null;

            //container / cfs fetch



            return Json(objAuctionInvoice, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAuctionCharges(int BIDId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, string CustomDuty, string OT, string ValuersCharge, string AuctionCharge, string MISC, string SEZ)
        {


            Ppg_AuctionRepository objChrgRepo = new Ppg_AuctionRepository();
            objChrgRepo.GetAuctionCharges(BIDId, InvoiceId, InvoiceDate, FreeUpTo, HSNCode, Convert.ToDecimal(CustomDuty == "" ? "0" : CustomDuty), Convert.ToDecimal(OT == "" ? "0" : OT), Convert.ToDecimal(ValuersCharge == "" ? "0" : ValuersCharge), Convert.ToDecimal(AuctionCharge == "" ? "0" : AuctionCharge), Convert.ToDecimal(MISC == "" ? "0" : MISC), SEZ);
            return Json(objChrgRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAuctionChargesforEdit(int BIDId, int InvoiceId, string InvoiceDate, string FreeUpTo, string HSNCode, string CustomDuty, string OT, string ValuersCharge, string AuctionCharge, string MISC)
        {


            Ppg_AuctionRepository objChrgRepo = new Ppg_AuctionRepository();
            objChrgRepo.GetAuctionChargesForEdit(BIDId, InvoiceId, InvoiceDate, FreeUpTo, HSNCode, Convert.ToDecimal(CustomDuty == "" ? "0" : CustomDuty), Convert.ToDecimal(OT == "" ? "0" : OT), Convert.ToDecimal(ValuersCharge == "" ? "0" : ValuersCharge), Convert.ToDecimal(AuctionCharge == "" ? "0" : AuctionCharge), Convert.ToDecimal(MISC == "" ? "0" : MISC));
            return Json(objChrgRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddEditInvoice(AuctionInvoice objAuctionInvoice)
        {
            try
            {
                Ppg_AuctionRepository objER = new Ppg_AuctionRepository();
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

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditInvoiceforEdit(AuctionInvoice objAuctionInvoice)
        {
            try
            {
                Ppg_AuctionRepository objER = new Ppg_AuctionRepository();
                IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objAuctionInvoice.PaymentSheetModelJson);
                string XML = Utility.CreateXML(PostPaymentChargeList);
                objAuctionInvoice.PaymentSheetModelJson = XML;
                objER.AddEditAuctionInvoiceForEdit(objAuctionInvoice);
                return Json(objER.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }


        public JsonResult GetBulkInvoiceReport(AuctionInvoiceViewModel ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            string FilePath = "";
            string ModuleName = "";
            AuctionRepository ObjRR = new AuctionRepository();
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
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCargo = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
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
            #region for Bid Details

            upiQRInfo.InvoiceDate = Convert.ToString(lstInvoice[0].DocDt);
            upiQRInfo.invoiceNo = Convert.ToString(lstInvoice[0].InvoiceNo);
            upiQRInfo.InvoiceName = Convert.ToString(lstInvoice[0].Party);
            upiQRInfo.mam = Convert.ToDecimal(lstInvoice[0].InvoiceAmt);
            upiQRInfo.am = Convert.ToDecimal(lstInvoice[0].InvoiceAmt);
            upiQRInfo.CGST = Convert.ToDecimal(lstInvoice[0].CGST);
            upiQRInfo.SGST = Convert.ToDecimal(lstInvoice[0].SGST);
            upiQRInfo.IGST = Convert.ToDecimal(lstInvoice[0].IGST);
            upiQRInfo.GSTPCT = Convert.ToDecimal(lstInvoice[0].IGSTPer);
            upiQRInfo.QRexpire = Convert.ToString(lstInvoice[0].DocDt);
            upiQRInfo.tr = Convert.ToString(lstInvoice[0].InvoiceId);

            Einvoice Eobj = new Einvoice();
            B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
            objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
            IrnResponse objERes = new IrnResponse();
            objERes.SignedQRCode = objresponse.QrCodeBase64;
            objERes.SignedInvoice = objresponse.QrCodeJson;
            objERes.SignedQRCode = objresponse.QrCodeJson;

            StringBuilder html = new StringBuilder();
            int i = 1;
            /*Header Part*/

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
            html.Append("</tr>");
            html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b>  " + lstInvoice[0].irn + " </td></tr>");
            html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
            html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
            html.Append("</tbody></table></td>");
            if (lstInvoice[0].SupplyType == "B2B")
            {
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(lstInvoice[0].SignedQRCode)) + "'/> </td>");
            }
            else
                {
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
            }
            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='border: 1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");            
            html.Append("<tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>TAX INVOICE</h2> </td></tr>");

            //blocl1//
            html.Append("<tr><td style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

            html.Append("<td colspan='5' width='50%'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'> " + lstInvoice[0].InvoiceNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'> " + lstInvoice[0].Party + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'> " + lstInvoice[0].Address + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("<td colspan='6' width='40%'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Tax Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].InvoiceDate + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].StateName + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Party GST NO.</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].GstNo + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");
            //blocl1 End//  

            //blocl2//
            html.Append("<tr><td style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].GstStateCode + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>No</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].SupplyType + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            //blocl2 End//

            //blocl3//
            html.Append("<tr><td style='border-bottom: 1px solid #000;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td colspan='4' width='33.33333333333333%'><b>Bid Date :</b>" + lstInvoice[0].BidDate + "</td>");
            html.Append("<td colspan='4' width='33.33333333333333%'><b>Bid No :</b>" + lstInvoice[0].BidNo + "</td>");
            html.Append("<td colspan='4' width='33.33333333333333%'><b>Shed No :</b>" + lstInvoice[0].GodownName + "</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td colspan='12' width='100%'><b>S/Line :</b>" + lstInvoice[0].SLine + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            //block3 End//          

            //block4// 
            html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 10px; font-weight: bold;'>A. PARTICULARS OF CONTAINER: </th></tr>");

            html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>ICD Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Auction</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Free Dt Upto</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Delivery Valid Upto</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No of Days</th>");
            html.Append("<th style='border-bottom: 1px solid #000;'>No Week </th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].CFSCode + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].ContainerNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].Size + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].AuctionDate + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].FreeUpto + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].DeliveryValidUpto + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstContainer[0].TotalDays + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000;'>" + lstContainer[0].TotalWeeks + "</td>");
            html.Append("</tr>");

            html.Append("</tbody></table></td></tr>");
            //block4 End//  

            //block5// 
            html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0; font-weight: bold;'>B. PARTICULARS OF CARGO: </th></tr>");

            html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>IGM NO. </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>OBL No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>BOE No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>BOE Date</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>BID Value</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Duty</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Cargo Description</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No of Pkg</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Total Gr Wt.(In Kg) </th>");
            html.Append("<th style='border-bottom: 1px solid #000;'>Total Area</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].IGMNO + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].OBLNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].SBNo + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].Boe + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].BoeDate + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].BidAmount + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].Duty + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].CommodityName + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].Noofpkg + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstCargo[0].Weight + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000;'>" + lstCargo[0].Area + "</td>");
            html.Append("</tr>");

            html.Append("</tbody></table></td></tr>");
            //block5 End//  

            //block6// 
            html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0; font-weight: bold;'>C. Auction Details </ th></tr>");

            html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Bid Amount </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>GST% & HSN Code </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>CGST </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SGST </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>IGST</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Total Dues </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>EMD Paid Earlier </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Adv.Paid Earlier </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Auction Charges </th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Amount Receipt No. & Date </th>");
            html.Append("<th style='border-bottom: 1px solid #000;'>Net Amount Payable</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>1</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].BidAmount + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].GSTPer + "/"+lstInvoice[0].HSNCode + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].CGSTAmount + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].SGSTAmount + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].IGSTAmount + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].TotalDue + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].EMDPaid + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].AdvancePaid + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].AuctionCharges + "</td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + lstInvoice[0].EmdRcvdNo + "/" + lstInvoice[0].EmdRcvdDate + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000;'>" + lstInvoice[0].NetAmountPay + "</td>");
            html.Append("</tr>");

            html.Append("</tbody></table></td></tr>");
            //block5 End// 
            
            html.Append("<tr><th cellpadding='5' style='font-size:6pt;'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(lstInvoice[0].TotalDue.ToString("0.00")) + "</th></tr>");
            html.Append("<tr><th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: left; padding: 5px;' colspan='13'>Remarks : " + lstInvoice[0].Remarks + "</th></tr>");
            html.Append("</tbody></table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; font-size: 6pt; width:100%;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='12' style='text-align: center; border-bottom: 1px solid #000;'>RECEIPT</th></tr>");
            html.Append("<tr><td colspan='12'><b>RECEIVED Rs.</b> " + objCurr.changeCurrencyToWords(lstInvoice[0].NetAmountPay.ToString("0.00")) + " </td></tr>");
            html.Append("<tr><th colspan='7' width='70%' style='font-size: 6pt;'>RECEIPT/DD No. <span>" + lstMode[0].CashReceiptNo + "</span></th> <th colspan='3' width='30%' style='font-size: 8pt;'>Mode of Receipt. <span>" + lstMode[0].PayMode + "</span></th></tr>");
            html.Append("</tbody></table>");

            html.Append("<table style='width:100%; border: 1px solid #333; font-size:6pt;' cellspacing='0' cellpadding='0'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td cellpadding='5' style='text-align: left; width: 50%; vertical-align: bottom;'>Signature of the bidder </td>");
            html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
            html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='0'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td style='border: 1px solid #333; border-top: 0; border-bottom: 0; border-right: 0; padding: 0 15px;'>");
            html.Append("<p style='text-align: right; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
            html.Append("<span><br/><br/></span>");
            html.Append("<p style='text-align: right; margin:0;'>Authorised Signatory</p>");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</td></tr>");

            if (lstInvoice[0].SupplyType != "B2C")
            {
                html.Append("<tr>");
                html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                html.Append("</tr>");
            }

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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, html.ToString());
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


        //Assessment Sheet
        [HttpPost]
        public JsonResult GetAssessmentSheetReport(string InvoiceNo)
        {

            string FilePath = "";

            Ppg_AuctionRepository ObjRR = new Ppg_AuctionRepository();
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
            Einvoice obj = new Einvoice(new HeaderParam(), "");
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

            #region for Bid Details
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
                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                //rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                //if (rep.DBResponse.Data != null)
                //{
                //    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                //}
                StringBuilder html = new StringBuilder();
                int i = 1;
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SupplyType == "B2C")
                {

                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12' style='border: 1px solid #000; border-bottom: 0;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                lstBidderDetails.Where(k => k.InvoiceId == item.InvoiceId).ToList().ForEach(idata =>
                {
                    html.Append("<tr><td colspan='12' style='border: 1px solid #000; border-top: 0;'>");

                    html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                    html.Append("<td colspan='5' width='50%'>");
                    html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + idata.InvoiceNo + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Assessment Sheet No</th><th>:</th><td colspan='6' width='70%'>" + idata.AssessmentSheet + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + idata.Party + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + idata.Address + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + idata.GstNo + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>BID No.</th><th>:</th><td colspan='6' width='70%'>" + idata.BidNo + "</td></tr>");

                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("<td colspan='6' width='40%'>");
                    html.Append("<table style='width:100%; font-size:9pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><th colspan='6' width='50%'>Tax Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + idata.InvoiceDate + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Date</th><th>:</th><td colspan='6' width='70%'>" + idata.AssesmentDate + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + idata.StateName + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Shed No/Godown No.</th><th>:</th><td colspan='6' width='50%'>" + idata.GodownName + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td></tr>");
                });

                html.Append("<tr><td colspan='12' style='border: 1px solid #000; border-top: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='12' width='100%' style='padding-bottom:5px;'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table></td></tr>");

                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>A. PARTICULARS OF CONTAINER :</b> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Date of Auction</th>");
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
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container Charges</b> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
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
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>TOTAL</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + Taxable + "</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + CGSTAmt + "</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + SGSTAmt + "</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + IGSTAmt + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + Total + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr>");

                html.Append("<tr><td><br/></td></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
     
                html.Append("</tr>");

                html.Append("<tr><td><br/></td></tr>");

                html.Append("<tr><th><span><br/><br/></span></th></tr>");
                html.Append("<tr><th colspan='7' width='70%' style='font-size: 8pt;'>RECEIPT/DD No.</th> <th colspan='3' width='30%' style='font-size: 8pt;'>Mode of Receipt.</th></tr>");
                html.Append("<tr><th><span><br/></span></th></tr>");
                html.Append("<tr><th colspan='12' style='font-size: 8pt; text-align: right;'>For Central Warehousing Corporation<br/>Authorized Signatories</th></tr>");
                html.Append("<tr><th><span><br/><br/></span></th></tr>");
                html.Append("<tr><th colspan='12' style='font-size: 8pt; text-align: right;'>Signature of the bidder</th></tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;

                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;



        }


        public ActionResult GetAssessmentList()
        {
            Ppg_AuctionRepository obj = new Ppg_AuctionRepository();
            obj.GetAssessmentList();
            List<AuctionInvoice> lstInvoice = new List<AuctionInvoice>();
            lstInvoice = ((List<AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetAssessmentList", lstInvoice);
        }


        public ActionResult GetInvoiceList()
        {
            Ppg_AuctionRepository obj = new Ppg_AuctionRepository();
            obj.GetInvoiceList();
            List<AuctionInvoice> lstInvoice = new List<AuctionInvoice>();
            lstInvoice = ((List<AuctionInvoice>)obj.DBResponse.Data);
            return PartialView("GetInvoiceList", lstInvoice);
        }

        public ActionResult BulkInvoicePrint()
        {
            AuctionRepository _Ar = new AuctionRepository();
            List<Models.PartyDetails> lstPartyDetails = new List<Models.PartyDetails>();
            _Ar.GetBidPartyAndDetails();
            if (_Ar.DBResponse.Data != null)
            {
                lstPartyDetails = (List<Models.PartyDetails>)_Ar.DBResponse.Data;
            }

            ViewBag.PaymentParty = Newtonsoft.Json.JsonConvert.SerializeObject(lstPartyDetails);
            return PartialView();
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType)
        {
            Ppg_AuctionRepository ObjRR = new Ppg_AuctionRepository();
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
            AuctionRepository Obj = new AuctionRepository();
            Ppg_AuctionRepository ObjRR = new Ppg_AuctionRepository();


            AuctionInvoiceViewModel ObjBulk = new AuctionInvoiceViewModel();
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


        #region Auction Invoice  Edit

        [HttpGet]
        public ActionResult EditAuctionInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            // Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("IMPDeli");
            //  if (objImport.DBResponse.Status > 0)
            //     ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            // else
            //    ViewBag.InvoiceList = null;

            //       objImport.GetPaymentPartyForEditImport();
            //      if (objImport.DBResponse.Status > 0)
            //        ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //    else
            //     ViewBag.PaymentParty = null;

            return PartialView();
        }





        [HttpGet]
        public JsonResult GetAucInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_AuctionRepository objImport = new Ppg_AuctionRepository();
                objImport.GetAuctionInvoiceDetailsForEdit(InvoiceId);
                if (objImport.DBResponse.Status == 1)
                {
                    //PpgInvoiceYard objPostPaymentSheet = (PpgInvoiceYard)objCashManagement.DBResponse.Data;
                    CwcExim.Areas.Import.Models.PPGInvoiceGodown objPostPaymentSheet = (CwcExim.Areas.Import.Models.PPGInvoiceGodown)objImport.DBResponse.Data; //new PPGInvoiceGodown();

                    IList<CwcExim.Areas.Import.Models.PPGPaymentSheetBOE> containers = new List<CwcExim.Areas.Import.Models.PPGPaymentSheetBOE>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new CwcExim.Areas.Import.Models.PPGPaymentSheetBOE
                        {
                            CFSCode = item.CFSCode,
                            LineNo = item.LineNo,
                            BOENo = objPostPaymentSheet.BOENo,

                            Selected = true,

                        });
                    });
                    //******Get Container By ReqId****************************************************//



                    //*********************************************************************************//

                    /***************BOL PRINT******************/
                    // var BOL = "";
                    // objCharge.GetBOLForEmptyCont("Yard", objPostPaymentSheet.RequestId);
                    //  if (objCharge.DBResponse.Status == 1)
                    //     BOL = objCharge.DBResponse.Data.ToString();
                    /************************************/
                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }





        public JsonResult AddEditAuctionPaymentSheet(CwcExim.Areas.Import.Models.PPGInvoiceGodown objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                String CargoXML = "";
                string ChargesBreakupXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstInvoiceCargo);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                objChargeMaster.AddEditInvoiceGodown(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);

                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }











        #endregion



        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {



            Ppg_AuctionRepository objPpgRepo = new Ppg_AuctionRepository();
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
        #region Bulk IRN Generation

        [HttpGet]
        public ActionResult BulkIRNAuctionGeneration()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBulkIrnDetails()
        {
            Ppg_AuctionRepository objPpgRepo = new Ppg_AuctionRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (PPG_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            //Ppg_CWCImportController objPpgIrn = new Ppg_CWCImportController();

            try
            {

                var invoiceData = JsonConvert.DeserializeObject<PPG_BulkIRN>(objForm["PaymentSheetModelJson"]);

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



            Ppg_AuctionRepository objPpgRepo = new Ppg_AuctionRepository();
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