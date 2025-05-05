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
using CwcExim.Areas.Export.Models;
using System.Text;
using EinvoiceLibrary;
using System.Threading.Tasks;
using System.Drawing;
using EinvoiceLibrary;
using CwcExim.Areas.CashManagement.Models;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Net.Http;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using CCA.Util;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class Kol_CashManagementController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        #region--PAYMENT VOUCHER--

        [HttpGet]
        public ActionResult PaymentVoucher()
        {
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetPaymentVoucherCreateInfo();
            ViewData["COMGST"] = ((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).UserGST;
            ViewBag.Expenses = JsonConvert.SerializeObject(((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).Expenses);
            ViewBag.ExpHSN = JsonConvert.SerializeObject(((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).ExpHSN);
            ViewBag.HSN = JsonConvert.SerializeObject(((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).HSN);
            ViewBag.Parties = ((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).Party;
            ViewData["InvoiceNo"] = ((Kol_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).VoucherId;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PaymentVoucherPrint(int PVId)
        {

            Kol_CashManagementRepository ObjRR = new Kol_CashManagementRepository();
            Kol_NewPaymentValucherModel LstSeal = new Kol_NewPaymentValucherModel();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PaymentVoucherPrint(PVId);//, objLogin.Uid
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                //LstSeal = (List<Kol_NewPaymentValucherModel>)ObjRR.DBResponse.Data;

                // LstSeal = (List<Kol_NewPaymentValucherModel>)ObjRR.DBResponse.Data;
                LstSeal = (Kol_NewPaymentValucherModel)ObjRR.DBResponse.Data;
                Path = GeneratePaymentPDF(LstSeal, PVId);
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        public string conversion(decimal amount)
        {
            double m = Convert.ToInt64(Math.Floor(Convert.ToDouble(amount)));
            double l = Convert.ToDouble(amount);

            double j = (l - m) * 100;


            var beforefloating = ConvertNumbertoWords(Convert.ToInt64(m));
            var afterfloating = ConvertNumbertoWords(Convert.ToInt64(j));



            var Content = beforefloating + ' ' + " RUPEES" + ' ' + afterfloating + ' ' + " PAISE only";
            return Content;
        }

        [NonAction]
        public string GeneratePaymentPDF(Kol_NewPaymentValucherModel LstSeal, int PVId)
        {

            try
            {
                 

                // string FtpIdPath = "";
                //string LocalIdPath = "";
                // var Pages = new string[1];
                var FileName = "Paymentvoucher.pdf";
                //  string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                // Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>Principal Place of Business:___________________</ label><br/><label style='font-size: 14px; font-weight:bold;'>Payment Voucher</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%; border:1px solid #000; font-size:10pt;'><tbody>");
                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>Details of Service Provider</b></td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>Details of Service Receiver</b></td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>Name : </b>"+ LstSeal.CompanyName + "</td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>Name :" + LstSeal.Party + "</b></td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>Warehouse Address : </b>" + LstSeal.CompanyAddress + "</td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>Address : </b> " + LstSeal.Address + "</td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>City : </b>" + LstSeal.CompanyCity + "</td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>City : </b>" + LstSeal.City + " </td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>State : </b>" + LstSeal.CompanyState + "</td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>State : </b>" + LstSeal.State + "</td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>State Code : </b>" + LstSeal.CompanyStateCode + " </td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>State Code : </b>" + LstSeal.StateCode + " </td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>GSTIN : </b>" + LstSeal.CompanyGST + " </td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'><b>GSTIN (if registered) : </b> " + LstSeal.GSTNo + "</td></tr>");

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-right: 1px solid #000;'><b>PAN : </b> " + LstSeal.CompanyPan + " </td>");
                Pages.Append("<td colspan='6' width='50%' cellpadding='5'></td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='0' style='margin-top:15px; width:100%; font-size:10pt;'><tbody>");
                Pages.Append("<tr><td colspan='12'><u><b>Serial No:</b> " + LstSeal.VoucherNo + " </u></td></tr>");
                Pages.Append("<tr><td colspan='12'><span><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12'><u><b>Date:</b>" + LstSeal.PaymentDate + " </u> </td></tr>");
                Pages.Append("<tr><td colspan='12'><span><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12'><u><b>Purpose:</b>" + LstSeal.Purpose + " </u> </td></tr>");
                Pages.Append("<tr><td colspan='12'><span><br/><br/></span></td></tr>");
                Pages.Append("<tr><th colspan='12' style='font-size:16px;'>For Payment under Reverse Charge</th></tr>");
                Pages.Append("</tbody></table>");

                int Count = 1;
             
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                // string AmountInWord = ConvertNumbertoWords((long)LstSeal.TotalAmount);
                string AmountInWord = conversion(Math.Round(LstSeal.TotalAmount, 2));
                Pages.Append("<table cellspacing='0' cellpadding='5' style='margin-top:15px; width:100%; border:1px solid #000; border-bottom:0; font-size:9pt;<thead>'>");
                Pages.Append("<tr><th style='width:3%; text-align:center; border-right:1px solid #000; border-bottom:1px solid #000;'>Sl.No.</th>");
                Pages.Append("<th style='width:15%; text-align:center; border-right:1px solid #000; border-bottom:1px solid #000;'>Description of Services</th>");
                Pages.Append("<th style='width:10%; text-align:center; border-right:1px solid #000; border-bottom:1px solid #000;'>Service Accounting Code</th>");
                Pages.Append("<th style='width:8%; text-align:center; border-bottom:1px solid #000;'>Amount Paid</th></tr>");
                Pages.Append("<tbody>");
                LstSeal.expcharges.ToList().ForEach(item =>
                   {
                       IGSTAmt += item.IGST;
                       CGSTAmt += item.CGST;
                       SGSTAmt += item.SGST;



                       Pages.Append("<tr><td style='width:3%; border-right: 1px solid #000; border-bottom:1px solid #000;'>" + Count + "</td>");
                       Pages.Append("<td style='width:15%; border-right:1px solid #000; border-bottom:1px solid #000;'>" + item.ExpenseHead + "</td>");
                       Pages.Append("<td style='width:10%; border-right:1px solid #000; border-bottom:1px solid #000;'>" + item.Expensecode + "</td>");
                       Pages.Append("<td style='width:8%; border-bottom:1px solid #000; text-align:right;'>" + Math.Round(item.Amount, 2) + "</td></tr>");
                       Count++;
                   });  
                       
                       Pages.Append("</tbody></table>");

                       Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%; border:1px solid #000; font-size:9pt;'><tbody><tr>'>");
                       Pages.Append("<td colspan='6' width='50%' valign='bottom' style='border-right:1px solid #000;'>");
                       Pages.Append("<p style='margin:0; padding:5px;'><u>Total Invoice Value (in figure)" + Math.Round(LstSeal.TotalAmount, 2) + "</u></p>");
                       Pages.Append("<p style='margin:0; padding:5px;'><u>Total Invoice Value (in words)" + AmountInWord + "</u></p>");
                       Pages.Append("</td>");

                       Pages.Append("<td colspan='6' width='50%'>");
                       Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-size:9pt;'><tbody>");
                       Pages.Append("<tr><td colspan='7' width='70%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>Total Taxable Value</td>");
                       Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalAmounts, 2) + "</th></tr>");

                       Pages.Append("<tr><td colspan='7' width='70%' cellpadding='0' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>");
                       Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%;font-size:9pt;'><tbody>");
                       Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>IGST</td> <td colspan='6' width='50%' cellpadding='5'>" +LstSeal.TotalIGST + "</td></tr>");
                       Pages.Append("</tbody></table>");
                       Pages.Append("</td>");
                       Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalIGSTAmt,2) + "</th></tr>");

                       Pages.Append("<tr><td colspan='7' width='70%' cellpadding='0' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>");
                       Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%;font-size:9pt;'><tbody>");
                       Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>CGST</td> <td colspan='6' width='50%' cellpadding='5'>" + LstSeal.TotalCGST + "</td></tr>");
                       Pages.Append("</tbody></table>");
                       Pages.Append("</td>");
                       Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalCGSTAmt,2) + "</th></tr>");

                       Pages.Append("<tr><td colspan='7' width='70%' cellpadding='0' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>");
                       Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%;font-size:9pt;'><tbody>");
                       Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>SGST</td> <td colspan='6' width='50%' cellpadding='5'>" + LstSeal.TotalSGST + "</td></tr>");
                       Pages.Append("</tbody></table>");
                       Pages.Append("</td>");
                       Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalSGSTAmt,2) + "</th></tr>");

                       Pages.Append("<tr><td colspan='7' width='70%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>Total Invoice Amount</td>");
                       Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalAmount, 2) + "</th></tr>");
                     //  Count++;
                   
                Pages.Append("</tbody></table>");
                Pages.Append("</td>");
                Pages.Append("</tr></tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='margin:10px 0 0;width:100%; font-size:9pt;'><tbody>");
                Pages.Append("<tr><td colspan='12' style='border-bottom: 1px solid #000;'><b>Narration :</b> " + LstSeal.Narration + " </td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-size:9pt;'><tbody>");
                Pages.Append("<tr><td colspan='12'><span><br/><br/><br/><br/><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%'></td><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'>Signature :</td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%'></td><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;'>Name of the Signatory :</td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%'></td><td colspan='6' width='50%' cellpadding='5'>Designation/Status :</td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-size:9pt;'><tbody>");
                Pages.Append("<tr><td colspan='12'><span><br/><br/><br/><br/><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12' cellpadding='5'><span>TO,</span></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5'>________________________________________________</td><td colspan='6' width='50%'></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5'>________________________________________________</td><td colspan='6' width='50%'></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5'>________________________________________________</td><td colspan='6' width='50%'></td></tr>");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PaymentVoucher(Kol_NewPaymentValucherModel m)
        {




            if (ModelState.IsValid)
            {
                var objRepo = new Kol_CashManagementRepository();

                objRepo.AddNewPaymentVoucher(m);
                return Json(objRepo.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF(FormCollection fc)
        {
            try
            {
                var pages = new string[1];
                pages[0] = fc["page"].ToString();
                var fileName = "PaymentVoucher" + ".pdf";
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/PaymentVoucher/" + fc["PVHeadId"].ToString() + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/PaymentVoucher/" + fc["PVHeadId"].ToString() + "/" + fileName }, JsonRequestBehavior.DenyGet);// Data = fileName 
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }
        public ActionResult GetPaymentVoucherList()
        {
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetPaymentVoucherList();
            IEnumerable<Kol_NewPaymentValucherModel> lstPaymentVou = (IEnumerable<Kol_NewPaymentValucherModel>)objRepo.DBResponse.Data;
            if (lstPaymentVou != null)
            {
                return PartialView(lstPaymentVou);
            }
            else
            {
                return PartialView(new List<Kol_NewPaymentValucherModel>());
            }
        }

        #endregion

        #region-- ADD MONEY TO PD --

        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            var model = new AddMoneyToPDModel();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new ReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDITNOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            ViewBag.ServerDate = Utility.GetServerDate();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(AddMoneyToPDModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Kol_CashManagementRepository();
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
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
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


        public ActionResult AddMoneyToPDList(string SearchValue,int Pages=0)
        {
            List<Kdl_AddMoneyToPDList> lstAddMoneyToPDList = new List<Kdl_AddMoneyToPDList>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetAddmoneyToPDList(SearchValue, Pages);
            if(obj.DBResponse.Data!=null)
            {
                lstAddMoneyToPDList = (List<Kdl_AddMoneyToPDList>)obj.DBResponse.Data;
            }
            return PartialView(lstAddMoneyToPDList);
        }

        #endregion

        #region ---- Payment Receipt/Cash Receipt ----

        [HttpGet]
        public ActionResult CashReceipt(int InvoiceId = 0, string InvoiceNo = "")
        {
            Kol_CashReceiptModel ObjCashReceipt = new Kol_CashReceiptModel();
            var objRepo = new Kol_CashManagementRepository();
            if (InvoiceId == 0)
            {
                objRepo.GetInvoiceList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Invoice = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
                else
                    ViewBag.Invoice = null;
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                // ObjCashReceipt.InvoiceDate = DateTime.Today.ToString("dd/MM/yyyy");
                for (var i = 0; i < 6; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                return PartialView(ObjCashReceipt);
            }
            else
            {
                objRepo.GetInvoiceList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Invoice = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetails(InvoiceId, InvoiceNo);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (Kol_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                for (var i = 0; i < 6; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new CashReceipt());
                }

                var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                objRepo.GetCashRcptPrint(InvoiceId);
                if (objRepo.DBResponse.Data != null)
                {
                    ViewBag.CashPrint = JsonConvert.SerializeObject(((PostPaymentSheet)objRepo.DBResponse.Data));
                }
                else
                {
                    ViewBag.CashPrint = null;
                }
                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashReceipt(Kol_CashReceiptModel ObjCashReceipt)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
                // ObjCashReceipt.BranchId = Convert.ToInt32(Session["BranchId"]);
                var objRepo = new Kol_CashManagementRepository();
                objRepo.AddCashReceipt(ObjCashReceipt, xml);
                return Json(objRepo.DBResponse);
            }
            else
            {
                string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err, JsonRequestBehavior.DenyGet);
            }

        }

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
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }
        #endregion


        #region Edit cash Receipt
        //[HttpGet]
        public ActionResult EditCashReceiptPaymentMode()//int InvoiceId = 0, string InvoiceNo = ""
        {
            Kol_CashReceiptModel ObjCashReceipt = new Kol_CashReceiptModel();
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetReceiptList();
            List<EditReceiptPayment> lstEditReceiptPayment = new List<EditReceiptPayment>();

            if (objRepo.DBResponse.Data != null)
            {
                lstEditReceiptPayment = (List<EditReceiptPayment>)objRepo.DBResponse.Data;

                // ViewBag.lstReceiptPayment = lstEditReceiptPayment;
                ViewBag.lstReceiptPayment = JsonConvert.SerializeObject(((List<EditReceiptPayment>)objRepo.DBResponse.Data));
            }
            else
            {
                ViewBag.lstReceiptPayment = "";

            }
            //PDAListAndAddress lstPdaAdjustEdit = new PDAListAndAddress();
            objRepo.GetReceiptPDAList();
            if (objRepo.DBResponse.Data != null)
            {
                var lstPdaAdjustEdit = (PDAListAndAddress)objRepo.DBResponse.Data;
                ViewBag.listPdaAdjustEdit = JsonConvert.SerializeObject(lstPdaAdjustEdit._PdaAdjustEdit);
                ViewBag.PayByDetail = JsonConvert.SerializeObject(lstPdaAdjustEdit.PayByDetail);
            }
            else
            {
                ViewBag.listPdaAdjustEdit = "";

            }



            //if (InvoiceId == 0)
            //{
            //objRepo.GetInvoiceList();
            //if (objRepo.DBResponse.Data != null)
            //    ViewBag.Invoice = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
            //else
            //    ViewBag.Invoice = null;
            ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
            // ObjCashReceipt.InvoiceDate = DateTime.Today.ToString("dd/MM/yyyy");
            for (var i = 0; i < 6; i++)
            {
                ObjCashReceipt.CashReceiptDetail.Add(new CashReceipt());
            }

            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;




            return PartialView();
            //}
            //else
            //{
            //    objRepo.GetInvoiceList();
            //    if (objRepo.DBResponse.Data != null)
            //        ViewBag.Invoice = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
            //    else
            //        ViewBag.Invoice = null;
            //    objRepo.GetCashRcptDetails(InvoiceId, InvoiceNo);
            //    if (objRepo.DBResponse.Data != null)
            //    {
            //        ObjCashReceipt = (Kol_CashReceiptModel)objRepo.DBResponse.Data;
            //        // ViewBag.PayByDet =((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
            //        ViewBag.Pay = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
            //        ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
            //        ViewBag.Container = JsonConvert.SerializeObject(((Kol_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
            //    }
            //    else
            //    {
            //        ViewBag.Pay = null;
            //        ViewBag.PdaAdjust = null;
            //        ViewBag.Container = null;
            //    }

            //    for (var i = 0; i < 6; i++)
            //    {
            //        ObjCashReceipt.CashReceiptDetail.Add(new CashReceipt());
            //    }

            //    var PaymentMode = new SelectList(new[]
            //   {
            //    new SelectListItem { Text = "--- Select ---", Value = ""},
            //    new SelectListItem { Text = "CASH", Value = "CASH"},
            //    new SelectListItem { Text = "NEFT", Value = "NEFT"},
            //    new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
            //    new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
            //    new SelectListItem { Text = "PO", Value = "PO"},
            //    new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            //}, "Value", "Text");
            //    ViewBag.PaymentMode = PaymentMode;

            //    objRepo.GetCashRcptPrint(InvoiceId);
            //    if (objRepo.DBResponse.Data != null)
            //    {
            //        ViewBag.CashPrint = JsonConvert.SerializeObject(((PostPaymentSheet)objRepo.DBResponse.Data));
            //    }
            //    else
            //    {
            //        ViewBag.CashPrint = null;
            //    }
            //    return PartialView(ObjCashReceipt);
            //}
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult GetListOfPayments(int CashReceiptId, int InvoiceId = 0)
        {
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetReceiptDtlsList(CashReceiptId);
            List<CashReceiptEditDtls> lStCashReceiptEditDtls = new List<CashReceiptEditDtls>();

            if (objRepo.DBResponse.Data != null)
            {
                lStCashReceiptEditDtls = (List<CashReceiptEditDtls>)objRepo.DBResponse.Data;
            }
            //ViewBag.lStCashReceiptEditDtls = lStCashReceiptEditDtls;

            var PaymentMode = new SelectList(new[]
       {
                //new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;
            //ViewBag.PaymentMode=new SelectList((IEnumerable)PaymentMode,"Key","Value",)

            if (lStCashReceiptEditDtls.Count < 6)
            {
                for (var i = lStCashReceiptEditDtls.Count; i < 6; i++)
                {
                    lStCashReceiptEditDtls.Add(new CashReceiptEditDtls
                    {
                        // Amount = 0
                    });
                }
            }
            lStCashReceiptEditDtls.ForEach(m => Convert.ToDecimal(m.Amount));

            ViewBag.sumOfAmount = ((lStCashReceiptEditDtls.Sum(x => x.Amount) > 0) ? lStCashReceiptEditDtls.Sum(x => x.Amount).ToString() : "");


            objRepo.GetEditCashRcptPrint(InvoiceId);
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.CashPrint = JsonConvert.SerializeObject(((PostPaymentSheet)objRepo.DBResponse.Data));
            }
            else
            {
                ViewBag.CashPrint = null;
            }

            return PartialView(lStCashReceiptEditDtls);
        }

        public ActionResult SaveEditedCashReceipt(EditReceiptPayment objEditReceiptPayment)
        {

            var str = objEditReceiptPayment.receiptTableJson.Replace("--- Select ---", "");
            objEditReceiptPayment.CashReceiptDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CashReceiptEditDtls>>(str);
            objEditReceiptPayment.CashReceiptDetail.ToList().ForEach(item =>
            {
                if (item.Amount == 0 && string.IsNullOrEmpty(item.DraweeBank) && string.IsNullOrEmpty(item.PaymentMode) && string.IsNullOrEmpty(item.InstrumentNo) && string.IsNullOrEmpty(item.Date))
                {
                    objEditReceiptPayment.CashReceiptDetail.Remove(item);
                }
            });   

            //var XML = Utility.CreateXML(objEditReceiptPayment.CashReceiptDetail);
            var XML = Utility.CreateXML(objEditReceiptPayment.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
            //var XMLContent = Utility.CreateXML(LstExitThroughGateDetails);
            objEditReceiptPayment.receiptTableJson = XML;
            var objRepo = new Kol_CashManagementRepository();
            objRepo.SaveEditedCashRcpt(objEditReceiptPayment, ((Login)(Session["LoginUser"])).Uid);

            return Json(objRepo.DBResponse);

        }
        #endregion

        #region-- RECEIVE VOUCHER --

        [HttpGet]
        public ActionResult ReceivedVoucher()
        {
            var objRepo = new Kol_CashManagementRepository();
            //objRepo.GetPaymentVoucherCreateInfo();
            ViewData["InvoiceNo"] = objRepo.ReceiptVoucherNo();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReceiptVoucher(ReceiptVoucherModel m)
        {
            if (ModelState.IsValid)
            {
                var objRepo = new Kol_CashManagementRepository();
                objRepo.AddNewReceiptVoucher(m);
                return Json(new { Status = true, Message = "Receipt Saved Successfully", Data = "CWC/RV/" + objRepo.DBResponse.Data.ToString().PadLeft(7, '0') + "/" + DateTime.Today.Year.ToString(), Id = objRepo.DBResponse.Data.ToString() }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateReceiptVoucher(FormCollection Fc)
        {
            try
            {
                var pages = new string[1];
                var fileName = "ReceiptVoucher" + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/ReceiptVoucher/" + Fc["ReceiptId"].ToString() + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
                Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
                ObjRR.getCompanyDetails();
                string HeadOffice = "", HOAddress = "", ZonalOffice = "", ZOAddress = "";
                if (ObjRR.DBResponse.Data != null)
                {
                    objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                    ZonalOffice = objCompanyDetails.CompanyName;
                    ZOAddress = objCompanyDetails.CompanyAddress;
                }
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 10f, 40f, 40f, true))
                {
                    rh.HeadOffice = HeadOffice;
                    rh.HOAddress = HOAddress;
                    rh.ZonalOffice = ZonalOffice;
                    rh.ZOAddress = ZOAddress;
                    rh.GeneratePDF(PdfDirectory + fileName, Fc["Page1"].ToString());
                }
                return Json(new { Status = 1, Message = "/Docs/ReceiptVoucher/" + Fc["ReceiptId"].ToString() + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "" }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult GetReceiptVoucherList()
        {
            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetReceiptVoucherList();
            IEnumerable<ReceiptVoucherModel> lstRcptVou = (IEnumerable<ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
                return PartialView(lstRcptVou);
            }
            else
            {
                return PartialView(new List<ReceiptVoucherModel>());
            }
        }

        #endregion

        #region Credit Note

        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Kol_CashManagementRepository objPpgRepo = new Kol_CashManagementRepository();



            if (SupplyType == "B2C")
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

                    Eobj = new Einvoice();
                    IrnModel m1 = new IrnModel();

                    QrCodeInfo q1 = new QrCodeInfo();
                    //   QrCodeData qdt = new QrCodeData();
                    objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                    var OutputData = (QrCodeData)objPpgRepo.DBResponse.Data;

                    m1.DocumentNo = OutputData.DocNo;
                    m1.DocumentDate = OutputData.DocDt;
                    m1.SupplierGstNo = OutputData.SellerGstin;
                    m1.DocumentType = OutputData.DocTyp;
                    String IRN = Eobj.GenerateB2cIrn(m1);
                    OutputData.Irn = IRN;
                    OutputData.IrnDt = OutputData.DocDt;
                    OutputData.iss = "NIC";
                    q1.Data = OutputData;
                    B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                    objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);
                }

            }
            // var Images = LoadImage(ERes.QRCodeImageBase64);

            return Json(objPpgRepo.DBResponse);
        }

        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetInvoiceNoForCreaditNote("C");
            if (objRepo.DBResponse.Data != null)
                ViewBag.InvoiceNo = objRepo.DBResponse.Data;
            else
                ViewBag.InvoiceNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForCreaditNote(int InvoiceId)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(CreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
                string XML = "";
                if (objCR.ChargesJson != null)
                {
                    List<InvoiceCarges> lstCharges = JsonConvert.DeserializeObject<List<InvoiceCarges>>(objCR.ChargesJson);
                    lstCharges = lstCharges.Where(x => x.RetValue > Convert.ToDecimal(0) && x.Taxable > (Convert.ToDecimal(0)) && x.Taxable >= x.RetValue).ToList();
                    XML = Utility.CreateXML(lstCharges);
                    objRepo.AddCreditNote(objCR, XML, "C");
                }
                return Json(objRepo.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public ActionResult ListOfCRNote()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNoteSingle(int CRNoteId, string Note)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            PrintModelOfCr objCR = new PrintModelOfCr();
            objRepo.PrintDetailsForCRNote(CRNoteId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfCr)objRepo.DBResponse.Data;
                string Path = GenerateCRNotePDF(objCR, CRNoteId, Note);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }

        [NonAction]
        public string GenerateCRNotePDF(PrintModelOfCr objCR, int CRNoteId, string Note)
        {
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
            html.Append("<td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");
            //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td>");
            html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + objCR.irn + " </td></tr>");
            html.Append("</tbody></table></td>");


            if (objCR.SignedQRCode == "")
            { }
            else
            {
                if (objCR.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");

                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");

                }
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
        public string LoadImage(string img)
        {
            ////data:image/gif;base64,
            ////this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String(img);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}





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
        //[NonAction]
        //public string GenerateCRNotePDF(PrintModelOfCr objCR, int CRNoteId, string Note)
        //{
        //    string SACCode = "", note = "", fileName = "";
        //    objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
        //     {
        //         if (SACCode == "")
        //             SACCode = item.SACCode;
        //         else
        //             SACCode = SACCode + "," + item.SACCode;
        //     });
        //    note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
        //    fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
        //    string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
        //    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //    }
        //    if (System.IO.File.Exists(Path + "/" + fileName))
        //    {
        //        System.IO.File.Delete(Path + "/" + fileName);
        //    }
        //    string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
        //    string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
        //    string tr = "";
        //    int Count = 1;
        //    decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
        //    objCR.lstCharges.ToList().ForEach(item =>
        //    {
        //        tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
        //        IGSTAmt += item.IGSTAmt;
        //        CGSTAmt += item.CGSTAmt;
        //        SGSTAmt += item.SGSTAmt;
        //        Count++;
        //    });
        //    string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
        //    string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
        //    html = html + htmltable + tr + tfoot;
        //    using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //    {
        //        RH.GeneratePDF(Path + "/" + fileName, html);
        //    }
        //    return "/Docs/" + Session.SessionID + "/" + fileName;
        //}
        [NonAction]
        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
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

        #region Cash Collection Against Bounced Cheque
        [HttpGet]
        public ActionResult CashColAgnBncCheque()
        {
            CashColAgnBncChq objCashColAgnBncChq = new CashColAgnBncChq();
            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.ImpExpCha = objExport.DBResponse.Data;
            else
                ViewBag.ImpExpCha = null;
            var PaymentMode = new SelectList(new[]
               {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCashColAgnBncCheque(CashColAgnBncChq objCashColAgnBncChq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
                    objRepo.AddEditCashCollectionChq(objCashColAgnBncChq);
                    int Data = Convert.ToInt32(objRepo.DBResponse.Data);
                    string Message = objRepo.DBResponse.Message;
                    int Status = objRepo.DBResponse.Status;
                    if (objRepo.DBResponse.Status == 1)
                    {
                        objRepo.GetInvoiceAndCashReceipt(Convert.ToInt32(Data));
                        objRepo.UpdateCCInvoiceAndCashReceipt(Convert.ToInt32(Data), InvoiceHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data),
                            CashReceiptHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data));
                    }
                    return Json(objRepo.DBResponse);
                }
                else
                {
                    var Err = new { Status = -1, Message = "Error" };
                    return Json(Err);
                }
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [NonAction]
        public string InvoiceHtml(CashColAgnBncChqPrint actualJson)
        {
            var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft' /></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstCompanyDetails[0].CompanyName + "<br />" + actualJson.lstCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.lstCashColAgnBncChqHeader[0].InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstCashColAgnBncChqHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstCashColAgnBncChqHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstCashColAgnBncChqHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
            html1 += "<td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstCashColAgnBncChqHeader[0].PartyState.ToString()) ? "" : actualJson.lstCashColAgnBncChqHeader[0].PartyState) + "</span></td></tr><tr><td>State Code</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstCashColAgnBncChqHeader[0].PartyStateCode.ToString()) ? "" : actualJson.lstCashColAgnBncChqHeader[0].PartyStateCode) + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
            html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstCashColAgnBncChqHeader[0].InvoiceNo + "</span></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + Convert.ToDateTime(actualJson.lstCashColAgnBncChqHeader[0].InvoiceDate).ToString("dd/MM/yyyy") + "</span></b></td>";
            html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";
            html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
            html1 += "Shipping Line. <span></span><br/>CFS Code No. <span></span><br/>Date & Time of Arrival (FN/AN): <span></span><br/>Date & Time of Destuffing (FN/AN)/ Delivery. <span></span><br/>Name of Importer. <span></span><br/>Bill of Entry No. <span></span><br/>Name of CHA. <span></span><br/>No of Packages. <span></span><br/>Total Gross Weight. <span></span><br/>Gross Weight per Package. <span></span><br/>Storage space occupied() <span></span><br/>Chargeable period for Storage space.";
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery </td></tr><tr><td>(c) Customs Examination Date </td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
            html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span></span></td></tr></tbody></table>";

            var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstCashColAgnBncChqHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + Convert.ToDateTime(actualJson.lstCashColAgnBncChqHeader[0].InvoiceDate).ToString("dd/MM/yyyy") + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;width:20%;'>Particulars</th><th style='border:1px solid #000;'>SAC</th><th style='border:1px solid #000;'>Value</th><th style='border:1px solid #000;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
            var cwcCharges = actualJson.lstCashColAgnBncChqCharge.Where(o => o.ChargeType == "CWC");
            foreach (var item in cwcCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            }
            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].CWCTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>0</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].AllTotal, 2) + "</span></td></tr>";
            html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].RoundUp, 2) + "</span></td></tr>";
            html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].InvoiceAmt, 2) + "</span></td></tr>";
            html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstCashColAgnBncChqHeader[0].InvoiceAmt, 2))) + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstCashColAgnBncChqHeader[0].Remarks + "</span></b></td></tr>";
            html2 += "</tbody><tfoot><tr><td colspan='4'>WAI/TA/JTA/JS/SUPTD</td><td colspan='4'>AM (A/cs)</td><td colspan='4' style='text-align:center;'>Manager (CFS)</td></tr></tfoot></table>";
            return html1 + "<>" + html2;
        }

        [NonAction]
        public string CashReceiptHtml(CashColAgnBncChqPrint actualJson)
        {
           
            var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft'/></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstCompanyDetails[0].CompanyName + "<br />" + actualJson.lstCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>CASH RECEIPT</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstCashColAgnBncChqHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstCashColAgnBncChqHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstCashColAgnBncChqHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
            html1 += "<td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstCashColAgnBncChqHeader[0].PartyState.ToString()) ? "" : actualJson.lstCashColAgnBncChqHeader[0].PartyState) + "</span></td></tr><tr><td>State Code</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstCashColAgnBncChqHeader[0].PartyStateCode.ToString()) ? "" : actualJson.lstCashColAgnBncChqHeader[0].PartyStateCode) + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstCashColAgnBncChqHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
            html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstCashColAgnBncChqHeader[0].InvoiceNo + "</span><br/><b>Cash Receipt No.<span>" + actualJson.lstCashColAgnBncChqHeader[0].ReceiptNo + "</span></b></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + Convert.ToDateTime(actualJson.lstCashColAgnBncChqHeader[0].InvoiceDate).ToString("dd/MM/yyyy") + "</span></b></td>";
            html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";

            html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
            html1 += "Shipping Line. <span></span><br/>CFS Code No. <span></span><br/>Date & Time of Arrival (FN/AN): <span></span><br/><span></span><span></span><span></span><br/>Name of CHA. <span></span><br/>No of Packages. <span></span><br/>Total Gross Weight. <span></span><br/>Gross Weight per Package. <span></span><br/>Storage space occupied() <span></span><br/>Chargeable period for Storage space.";
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) </td></tr><tr><td>(b) Date of Delivery</td></tr><tr><td>(c) Customs Examination Date</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
            html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span></span></td></tr></tbody></table>";

            var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstCashColAgnBncChqHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + Convert.ToDateTime(actualJson.lstCashColAgnBncChqHeader[0].InvoiceDate).ToString("dd/MM/yyyy") + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;'>Particulars</th><th style='border:1px solid #000;text-align:center;'>SAC</th><th style='border:1px solid #000;text-align:center;'>Value</th><th style='border:1px solid #000;text-align:center;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;text-align:center;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
            var cwcCharges = actualJson.lstCashColAgnBncChqCharge.Where(o => o.ChargeType == "CWC");
            foreach (var item in cwcCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0.00</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            }
            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + actualJson.lstCashColAgnBncChqHeader[0].CWCTDS + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>0</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].AllTotal) + "</span></td></tr>";
            html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].RoundUp, 2) + "</span></td></tr>";
            html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].InvoiceAmt) + "</span></td></tr>";
            html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstCashColAgnBncChqHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstCashColAgnBncChqHeader[0].InvoiceAmt, 2))) + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstCashColAgnBncChqHeader[0].Remarks + "</span></b></td></tr>";

            var html4 = "<table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;'><tr><td><table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;margin-top:30px;border:1px solid #000;border-collapse: collapse;'> <thead><tr><th style='border:1px solid #000;padding:10px;text-align:center;'>Mode</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Drawee Bank</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Instrument No</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Date</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Amount</th></tr></thead><tbody>";
            var t1 = "";
            t1 += "<tr><td style='border:1px solid #000;padding:5px;'>" + actualJson.lstCashColAgnBncChqPayMode[0].PayMode + "</td>";
            t1 += "<td style='border:1px solid #000;padding:5px;'>" + actualJson.lstCashColAgnBncChqPayMode[0].DraweeBank + "</td>";
            t1 += "<td style='border:1px solid #000;padding:5px;'>" + actualJson.lstCashColAgnBncChqPayMode[0].InstrumentNo + "</td>";
            t1 += "<td style='border:1px solid #000;padding:5px;text-align:center;'>" + actualJson.lstCashColAgnBncChqPayMode[0].Date + "</td>";
            t1 += "<td style='border:1px solid #000;padding:5px;text-align:right;'>" + Math.Round(actualJson.lstCashColAgnBncChqPayMode[0].Amount, 2) + "</td></tr>";
            html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span></span></b>";

            html2 += "<tr><td colspan='12'>" + html4 + "</td></tr>";
            html2 += "</tbody><tfoot><tr><td colspan='4'><br/><br/><br/><br/><br/><br/>CASHIER</td><td colspan='4'><br/><br/><br/><br/><br/><br/>AM (A/cs)</td><td colspan='4' style='text-align:center;'><br/><br/><br/><br/><br/><br/>Manager (CFS)</td></tr></tfoot></table>";
            return html1 + "<>" + html2;
        }

        #endregion

        #region Debit Note
        [HttpGet]
        public ActionResult CreateDebitNote()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetInvoiceNoForCreaditNote("D");
            if (objRepo.DBResponse.Data != null)
                ViewBag.InvoiceNo = objRepo.DBResponse.Data;
            else
                ViewBag.InvoiceNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDebitNote(CreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
                string XML = "";
                if (objCR.ChargesJson != null)
                {
                    List<InvoiceCarges> lstCharges = JsonConvert.DeserializeObject<List<InvoiceCarges>>(objCR.ChargesJson);
                    lstCharges = lstCharges.Where(x => x.RetValue > Convert.ToDecimal(0)).ToList();
                    XML = Utility.CreateXML(lstCharges);
                    objRepo.AddCreditNote(objCR, XML, "D");
                }
                return Json(objRepo.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public ActionResult ListOfDRNote()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        #endregion

        #region Generate Print

        [HttpGet]
        public string GeneratePrint(int ID, string Type = "Other")
        {
            /*
             * CC -> Cash Collection against bounced cheque
             * EX -> Export
             * "" -> Import and Bond
             * */
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            switch (Type)
            {
                case "CC":
                    objRepo.GetInvoiceAndCashReceipt(Convert.ToInt32(ID));
                    objRepo.UpdateCCInvoiceAndCashReceipt(Convert.ToInt32(ID), InvoiceHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data),
                        CashReceiptHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data));
                    break;
                case "EX":
                    objRepo.GetInvoiceCRForPrint(Convert.ToInt32(ID));
                    objRepo.UpdateCCInvoiceAndCashReceipt(Convert.ToInt32(ID), ExportInvoiceHtml((GenerateInvoiceCRPrint)objRepo.DBResponse.Data),
                        ExportCashReceiptHtml((GenerateInvoiceCRPrint)objRepo.DBResponse.Data));
                    break;
                default:
                    objRepo.GetInvoiceCRForPrint(Convert.ToInt32(ID));
                    objRepo.UpdateCCInvoiceAndCashReceipt(Convert.ToInt32(ID), BondInvoiceHtml((GenerateInvoiceCRPrint)objRepo.DBResponse.Data),
                        BondCashReceiptHtml((GenerateInvoiceCRPrint)objRepo.DBResponse.Data));
                    break;
            }
            return "";
        }

        [NonAction]
        public string BondInvoiceHtml(GenerateInvoiceCRPrint actualJson)
        {
            var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft' /></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].CompanyName + "<br />" + actualJson.lstInvoiceCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstInvoiceCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.lstInvoiceHeader[0].InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstInvoiceHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstInvoiceHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstInvoiceHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
            html1 += "<td><span>" + actualJson.lstInvoiceHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyState + "</span></td></tr><tr><td>State Code</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyStateCode + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
            html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></b></td>";
            html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";
            foreach (var item in actualJson.lstInvoiceContainers)
            {
                html1 += "<tr><td style='border:1px solid #000;text-align:center;'><span>" + item.ContainerNo + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Size + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.ArrivalDateTime + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + (item.CargoType == 1 ? "Haz" : "Non-Haz") + "</span></td></tr>";
            };
            html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
            html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].ArrivalDate + "</span><br/>Date & Time of Destuffing (FN/AN)/ Delivery. <span>" + actualJson.lstInvoiceHeader[0].DestuffingDate + "</span><br/>Name of Importer. <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>Bill of Entry No. <span>" + actualJson.lstInvoiceHeader[0].BOENo + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery" + actualJson.lstInvoiceHeader[0].DeliveryDate + "</td></tr><tr><td>(c) Customs Examination Date " + actualJson.lstInvoiceHeader[0].CstmExaminationDate + "</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
            html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

            var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;width:20%;'>Particulars</th><th style='border:1px solid #000;'>SAC</th><th style='border:1px solid #000;'>Value</th><th style='border:1px solid #000;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
            var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC" && o.Amount > 0);
            foreach (var item in cwcCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            };
            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].CWCTDS) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            var htCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "HT" && o.Amount > 0);
            foreach (var item in htCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            };
            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>H&T TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].HTTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";

            html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>0</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].AllTotal, 2) + "</span></td></tr>";
            html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].RoundUp, 2) + "</span></td></tr>";
            html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2) + "</span></td></tr>";
            html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstInvoiceHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstInvoiceHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2))).ToUpper() + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstInvoiceHeader[0].Remarks + "</span></b></td></tr>";

            html2 += "</tbody><tfoot><tr><td colspan='4'>WAI/TA/JTA/JS/SUPTD</td><td colspan='4'>AM (A/cs)</td><td colspan='4' style='text-align:center;'>Manager (CFS)</td></tr></tfoot></table>";
            return html1 + "<>" + html2;
        }

        [NonAction]
        public string BondCashReceiptHtml(GenerateInvoiceCRPrint actualJson)
        {
            String BoEDate = actualJson.lstInvoiceHeader[0].BOENo + " " + actualJson.lstInvoiceHeader[0].BOEDate;
            if (!string.IsNullOrEmpty(actualJson.lstInvoiceHeader[0].ReceiptNo))
            {
                var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft'/></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].CompanyName + "<br />" + actualJson.lstInvoiceCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstInvoiceCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>CASH RECEIPT</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstInvoiceHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstInvoiceHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstInvoiceHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
                html1 += "<td><span>" + actualJson.lstInvoiceHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstInvoiceHeader[0].PartyState.ToString()) ? "" : actualJson.lstInvoiceHeader[0].PartyState) + "</span></td></tr><tr><td>State Code</td><td><span>" + (string.IsNullOrEmpty(actualJson.lstInvoiceHeader[0].PartyStateCode.ToString()) ? "" : actualJson.lstInvoiceHeader[0].PartyStateCode) + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
                html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span><br/><b>Cash Receipt No.<span>" + actualJson.lstInvoiceHeader[0].ReceiptNo + "</span></b></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + Convert.ToDateTime(actualJson.lstInvoiceHeader[0].ReceiptDate).ToString("dd/MM/yyyy") + "</span></b></td>";
                html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";

                foreach (var item in actualJson.lstInvoiceContainers)
                {
                    html1 += "<tr><td style='border:1px solid #000;text-align:center;'><span>" + item.ContainerNo + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Size + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.ArrivalDateTime + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + (item.CargoType == 1 ? "Haz" : "Non-Haz") + "</span></td></tr>";
                };
                html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
                html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].ArrivalDate + "</span><br/>Date & Time of Destuffing (FN/AN)/ Delivery. <span>" + actualJson.lstInvoiceHeader[0].DestuffingDate + "</span><br/>Name of Importer. <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>Bill of Entry No./Dated <span>" + BoEDate + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
                html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery" + actualJson.lstInvoiceHeader[0].DeliveryDate + "</td></tr><tr><td>(c) Customs Examination Date " + actualJson.lstInvoiceHeader[0].CstmExaminationDate + "</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
                html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

                var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + Convert.ToDateTime(actualJson.lstInvoiceHeader[0].ReceiptDate).ToString("dd/MM/yyyy") + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;'>Particulars</th><th style='border:1px solid #000;text-align:center;'>SAC</th><th style='border:1px solid #000;text-align:center;'>Value</th><th style='border:1px solid #000;text-align:center;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;text-align:center;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
                var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC" && o.Amount > 0);
                foreach (var item in cwcCharges)
                {
                    html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0.00</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
                }
                html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].CWCTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
                var htCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "HT" && o.Amount > 0);
                foreach (var item in htCharges)
                {
                    html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
                };
                html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>H&T TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].HTTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
                html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>0</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].AllTotal) + "</span></td></tr>";
                html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].RoundUp, 2) + "</span></td></tr>";
                html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt) + "</span></td></tr>";
                html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstInvoiceHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstInvoiceHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2))) + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstInvoiceHeader[0].Remarks + "</span></b></td></tr>";

                var html4 = "<table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;'><tr><td><table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;margin-top:30px;border:1px solid #000;border-collapse: collapse;'> <thead><tr><th style='border:1px solid #000;padding:10px;text-align:center;'>Mode</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Drawee Bank</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Instrument No</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Date</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Amount</th></tr></thead><tbody>";
                var t1 = "";
                foreach (var item in actualJson.lstInvoiceCRDetails)
                {
                    t1 += "<tr><td style='border:1px solid #000;padding:5px;'>" + item.PayMode + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;'>" + item.DraweeBank + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;'>" + item.InstrumentNo + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;text-align:center;'>" + item.Date + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;text-align:right;'>" + Math.Round(item.Amount, 2) + "</td></tr>";
                }

                //
                if (actualJson.lstInvoiceHeader[0].PdaAdjust == 1)
                {
                    var t2 = "<tr><td colspan='4' style='border:1px solid #000;padding:5px;text-align:right;'><b>PDA Adjusted:</b></td><td style='border:1px solid #000;padding:5px;text-align:right;'>" + actualJson.lstInvoiceHeader[0].PdaAdjustedAmount.ToString() + "</td></tr>";
                    t1 += t2;
                }


                if (actualJson.lstInvoiceHeader[0].PdaAdjust == 1)
                {
                    html4 += t1 + "</tbody></table></td></tr></table><b>PDA Opening:</b> <span>" + actualJson.lstInvoiceHeader[0].PdaOpening.ToString() + "</span><br/><br/><b>PDA Closing:</b> <span>" + actualJson.lstInvoiceHeader[0].PdaClosing.ToString() + "</span><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";
                }
                else
                {
                    html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";
                }

                //
                //html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";

                html2 += "<tr><td colspan='12'>" + html4 + "</td></tr>";
                html2 += "</tbody><tfoot><tr><td colspan='4'><br/><br/><br/><br/><br/><br/>CASHIER</td><td colspan='4'><br/><br/><br/><br/><br/><br/>AM (A/cs)</td><td colspan='4' style='text-align:center;'><br/><br/><br/><br/><br/><br/>Manager (CFS)</td></tr></tfoot></table>";
                return html1 + "<>" + html2;
            }
            else
                return "";
        }

        [NonAction]
        public string ExportInvoiceHtml(GenerateInvoiceCRPrint actualJson)
        {
            var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft' /></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].CompanyName + "<br />" + actualJson.lstInvoiceCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstInvoiceCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>" + (actualJson.lstInvoiceHeader[0].InvoiceType == "Tax" ? "TAX INVOICE" : "BILL OF SUPPLY") + "</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstInvoiceHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstInvoiceHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstInvoiceHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
            html1 += "<td><span>" + actualJson.lstInvoiceHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyState + "</span></td></tr><tr><td>State Code</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyStateCode + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
            html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></b></td>";
            html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";
            foreach (var item in actualJson.lstInvoiceContainers)
            {
                html1 += "<tr><td style='border:1px solid #000;text-align:center;'><span>" + item.ContainerNo + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Size + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.ArrivalDateTime + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + (item.CargoType == 1 ? "Haz" : "Non-Haz") + "</span></td></tr>";
            };
            html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
            html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Carting (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].CartingDate + "</span><br/>Date & Time of Stuffing (FN/AN)/ Delivery. <span>" + actualJson.lstInvoiceHeader[0].StuffingDate + " / " + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span><br/>Name of Exporter. <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>S.B. No. <span>" + actualJson.lstInvoiceHeader[0].BOENo + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Stuffing(FN/AN) " + actualJson.lstInvoiceHeader[0].StuffingDate + "</td></tr><tr><td>(b) Date of Delivery " + actualJson.lstInvoiceHeader[0].DeliveryDate + "</td></tr><tr><td>(c) Customs Examination Date </td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
            html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

            var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;width:20%;'>Particulars</th><th style='border:1px solid #000;'>SAC</th><th style='border:1px solid #000;'>Value</th><th style='border:1px solid #000;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
            var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC");
            foreach (var item in cwcCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            };

            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].CWCTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            var htCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "HT");
            foreach (var item in htCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            };

            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>H&T TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].HTTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TDS, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'>0.00</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].AllTotal, 2) + "</span></td></tr>";
            html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].RoundUp, 2) + "</span></td></tr>";
            html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2) + "</span></td></tr>";
            html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstInvoiceHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstInvoiceHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2))).ToUpper() + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstInvoiceHeader[0].Remarks + "</span></b></td></tr>";
            html2 += "</tbody><tfoot><tr><td colspan='4'>WAI/TA/JTA/JS/SUPTD</td><td colspan='4'>AM (A/cs)</td><td colspan='4' style='text-align:center;'>Manager (CFS)</td></tr></tfoot></table>";
            return html1 + "<>" + html2;
        }

        [NonAction]
        public string ExportCashReceiptHtml(GenerateInvoiceCRPrint actualJson)
        {
            String BoEDate = actualJson.lstInvoiceHeader[0].BOENo + " " + actualJson.lstInvoiceHeader[0].BOEDate;
            if (!string.IsNullOrEmpty(actualJson.lstInvoiceHeader[0].ReceiptNo))
            {
                var html1 = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:20%;'><img style='width:50%;' src='IMGLeft'/></th><th style='width:60%;'></th><th style='width:20%;'><img style='width:70%;' src='IMGRight' /></th></tr><tr><th colspan='3' style='text-align:center;vertical-align:bottom;'>CENTRAL WAREHOUSING CORPORATION<br />(A GOVT. OF INDIA UNDERTAKING)</th></tr></thead><tbody><tr><td colspan='2' style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].ROAddress + "</td><td style='font-weight:600;'>" + actualJson.lstInvoiceCompanyDetails[0].CompanyName + "<br />" + actualJson.lstInvoiceCompanyDetails[0].CompanyAddress + "<br />" + actualJson.lstInvoiceCompanyDetails[0].EmailAddress + "</td></tr><tr><td colspan='3' style='text-align:center;padding-bottom:20pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>CASH RECEIPT</span></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td style='width:30%;'>GSTIN-<span>" + actualJson.lstInvoiceHeader[0].CompGST + "</span><br />PAN NO.- <span>" + actualJson.lstInvoiceHeader[0].CompPAN + "</span><br />STATE CODE : <span>" + actualJson.lstInvoiceHeader[0].CompStateCode + "</span></td><td style='width:50%;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:left;'>Details of Receiver ( Billed to)</th></tr></thead><tbody><tr><td style='width:45%;'>Name</td>";
                html1 += "<td><span>" + actualJson.lstInvoiceHeader[0].PartyName + "</span></td></tr><tr><td>Address</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyAddress + "</span></td></tr><tr><td>State</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyState + "</span></td></tr><tr><td>State Code</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyStateCode + "</span></td></tr><tr><td>GSTIN/ Unique ID</td><td><span>" + actualJson.lstInvoiceHeader[0].PartyGSTNo + "</span></td></tr></tbody></table>";
                html1 += "</td><td></td></tr><tr><td colspan='2'><b>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span><br/><b>Cash Receipt No.<span>" + actualJson.lstInvoiceHeader[0].ReceiptNo + "</span></b></b></td><td style='vertical-align:top;text-align: center;'><b>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></b></td>";
                html1 += "</tr><tr><td colspan='2'><table><tr><td style='vertical-align:top;width:50%;'><table style='width:100%;float: left;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;'><thead><tr><th style='border:1px solid #000;'>CONTAINER NO.</th><th style='border:1px solid #000;'>SIZE</th><th style='border:1px solid #000;'>ARRIVAL DT.</th><th style='border:1px solid #000;'>NON-HAZ</th></tr></thead><tbody>";
                foreach (var item in actualJson.lstInvoiceContainers)
                {
                    html1 += "<tr><td style='border:1px solid #000;text-align:center;'><span>" + item.ContainerNo + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.Size + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + item.ArrivalDateTime + "</span></td><td style='border:1px solid #000;text-align:center;'><span>" + (item.CargoType == 1 ? "Haz" : "Non-Haz") + "</span></td></tr>";
                };
                html1 += "</tbody></table></td><td style='vertical-align:top;width:50%;'><table style='width:100%;float:right;font-size:8pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;margin-top:20pt;border-left-style: none;'><tbody><tr><td>";
                html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].ArrivalDate + "</span><br/>" + actualJson.lstInvoiceHeader[0].CartingDate + " <span>" + actualJson.lstInvoiceHeader[0].StuffingDate + "</span><br/>Name of Exporter:  <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>S.B. No./Dated <span>" + BoEDate + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
                html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Stuffing(FN/AN) " + actualJson.lstInvoiceHeader[0].StuffingDate + "</td></tr><tr><td>(b) Date of Delivery" + actualJson.lstInvoiceHeader[0].DeliveryDate + "</td></tr><tr><td>(c) Customs Examination Date</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
                html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

                var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;'>Particulars</th><th style='border:1px solid #000;text-align:center;'>SAC</th><th style='border:1px solid #000;text-align:center;'>Value</th><th style='border:1px solid #000;text-align:center;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;text-align:center;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
                var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC");
                foreach (var item in cwcCharges)
                {
                    html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
                };

                html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].CWCTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
                var htCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "HT");
                foreach (var item in htCharges)
                {
                    html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
                };

                html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>H&T TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].HTTDS, 2) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
                html2 += "<tr><td colspan='12'>&nbsp;</td></tr><tr><td>Grand Total</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + /*actualJson.TDS.toFixed(2) +*/ "</span></td><td style='border:1px solid #000;text-align:right;'>0</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalCGST, 2) + "</span></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalSGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].TotalIGST, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].AllTotal) + "</span></td></tr>";
                html2 += "<tr><td>Round Up</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].RoundUp, 2) + "</span></td></tr>";
                html2 += "<tr><td>Grand Total(Rounded)</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt) + "</span></td></tr>";
                html2 += "<tr><td colspan='12'>TDS Deduction : " + Math.Round(actualJson.lstInvoiceHeader[0].TDS, 2) + "<br/>TDS Collection : " + Math.Round(actualJson.lstInvoiceHeader[0].TDSCol, 2) + "<br/><br/>FIGURE IN WORDS: <span>" + ConvertNumbertoWords(Convert.ToInt64(Math.Round(actualJson.lstInvoiceHeader[0].InvoiceAmt, 2))) + "</span></td></tr><tr><td colspan='12' style='padding:50pt 0pt;'><b>REMARKS:<span>" + actualJson.lstInvoiceHeader[0].Remarks + "</span></b></td></tr>";

                var html4 = "<table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;'><tr><td><table style='width:100%;font-size:8pt;font-family:verdana,sans-serif;margin-top:30px;border:1px solid #000;border-collapse: collapse;'> <thead><tr><th style='border:1px solid #000;padding:10px;text-align:center;'>Mode</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Drawee Bank</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Instrument No</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Date</th><th style='border:1px solid #000;padding:10px;text-align:center;'>Amount</th></tr></thead><tbody>";
                var t1 = "";
                foreach (var item in actualJson.lstInvoiceCRDetails)
                {
                    t1 += "<tr><td style='border:1px solid #000;padding:5px;'>" + item.PayMode + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;'>" + item.DraweeBank + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;'>" + item.InstrumentNo + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;text-align:center;'>" + item.Date + "</td>";
                    t1 += "<td style='border:1px solid #000;padding:5px;text-align:right;'>" + Math.Round(item.Amount, 2) + "</td></tr>";
                }
                if (actualJson.lstInvoiceHeader[0].PdaAdjust == 1)
                {
                    var t2 = "<tr><td colspan='4' style='border:1px solid #000;padding:5px;text-align:right;'><b>PDA Adjusted:</b></td><td style='border:1px solid #000;padding:5px;text-align:right;'>" + actualJson.lstInvoiceHeader[0].PdaAdjustedAmount.ToString() + "</td></tr>";
                    t1 += t2;
                }


                if (actualJson.lstInvoiceHeader[0].PdaAdjust == 1)
                {
                    html4 += t1 + "</tbody></table></td></tr></table><b>PDA Opening:</b> <span>" + actualJson.lstInvoiceHeader[0].PdaOpening.ToString() + "</span><br/><br/><b>PDA Closing:</b> <span>" + actualJson.lstInvoiceHeader[0].PdaClosing.ToString() + "</span><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";
                }
                else
                {
                    html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";
                }

                html2 += "<tr><td colspan='12'>" + html4 + "</td></tr>";
                html2 += "</tbody><tfoot><tr><td colspan='4'><br/><br/><br/><br/><br/><br/>CASHIER</td><td colspan='4'><br/><br/><br/><br/><br/><br/>AM (A/cs)</td><td colspan='4' style='text-align:center;'><br/><br/><br/><br/><br/><br/>Manager (CFS)</td></tr></tfoot></table>";
                return html1 + "<>" + html2;
            }
            else
                return "";
        }

        #endregion

        #region Miscellaneous Invoice
        [HttpGet]
        public ActionResult CreateMiscInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Kol_CashManagementRepository ObjCash = new Kol_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc(5);
            if (ObjCash.DBResponse.Status > 0)
                ViewBag.PaymentParty = (List<KolPaymentPartyName>)ObjCash.DBResponse.Data;
            else
                ViewBag.PaymentParty = null;





            ObjCash.GetPaymentPayerMisc();
            if (ObjCash.DBResponse.Status > 0)
            {
                ViewBag.PaymentPayer = (List<KolPaymentPartyName>)ObjCash.DBResponse.Data;
                ViewBag.Payer = JsonConvert.SerializeObject(ObjCash.DBResponse.Data);
            }
            else
            {
                ViewBag.PaymentPayer = null;
                ViewBag.Payer = null;

            }





            ObjCash.PurposeListForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                ViewBag.PurposeList = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.PurposeList = null;
            }

            ObjCash.SACForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                ViewBag.Sac = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.Sac = null;
            }
            return PartialView();
        }

        [HttpGet]
        public JsonResult LoadPartyonSelection(int PTYpe)
        {
            Kol_CashManagementRepository ObjCash = new Kol_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc(PTYpe);
            return Json(ObjCash.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMiscInvoiceAmount(string purpose, string purposecode, string SAC, float GST, string InvoiceType,string SEZ, int PartyId, decimal Amount)
        {
            Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, purposecode, SAC, GST, InvoiceType,SEZ, PartyId, Amount);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceAmount(string purpose, string Size)
        {
            Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
            objChargeMaster.GetMiscAmount(purpose, Size);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<KolMiscPostModel>(objForm["MiscInvModelJson"].ToString());
                Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
                objChargeMaster.AddMiscInv(invoiceData, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv", ExportUnder);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }



       





        [HttpGet]
        public ActionResult ListOfMiscInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Kol_CashManagementRepository objER = new Kol_CashManagementRepository();
            objER.ListOfMiscInvoice(Module, InvoiceNo, InvoiceDate);
            List<KolListOfMiscInvoice> obj = new List<KolListOfMiscInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<KolListOfMiscInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfMiscInvoice", obj);
        }


        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDFformisc(FormCollection fc)
        {
            try
            {
                // var pages = new string[2];
                var pages = new string[1];
                var type = fc["type"].ToString();
                var id = fc["id"].ToString();
                pages[0] = fc["page"].ToString();
                // pages[1] = fc["npage"].ToString();
                var ImgLeft = Server.MapPath("~/Content/Images/CWCPDF.PNG");
                var ImgRight = Server.MapPath("~/Content/Images/SwachhBharat-Logo.png");
                pages[0] = fc["page"].ToString().Replace("IMGLeft", ImgLeft).Replace("IMGRight", ImgRight);
                if (id == "")
                {
                    //string ad = "AB1CE12354F";
                    //Random rnd = new Random();
                    //id = new string(Enumerable.Repeat(ad, ad.Length).Select(s => s[rnd.Next(ad.Length)]).ToArray());
                    id = ((Login)(Session["LoginUser"])).Uid.ToString() + "" + DateTime.Now.ToString().Replace('/', '_').Replace(' ', '_').Replace(':', '_');
                }
                id = id.Replace('/', '_');
                var fileName = id + ".pdf";
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                {
                    rh.GeneratePDF(PdfDirectory + fileName, pages);
                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);// Data = fileName 
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion

        #region Miscellaneous Invoice Multiple Charges
        [HttpGet]
        public ActionResult CreateMiscInvoiceMultiChrg(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Kol_CashManagementRepository ObjCash = new Kol_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc(5);
            if (ObjCash.DBResponse.Status > 0)
                ViewBag.PaymentParty = (List<KolPaymentPartyName>)ObjCash.DBResponse.Data;
            else
                ViewBag.PaymentParty = null;


            ObjCash.GetPaymentPayerMisc();
            if (ObjCash.DBResponse.Status > 0)
            {
                ViewBag.PaymentPayer = (List<KolPaymentPartyName>)ObjCash.DBResponse.Data;
                ViewBag.Payer = JsonConvert.SerializeObject(ObjCash.DBResponse.Data);
            }
            else
            {
                ViewBag.PaymentPayer = null;
                ViewBag.Payer = null;

            }
            

            ObjCash.PurposeListForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                ViewBag.PurposeList = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.PurposeList = null;
            }

            ObjCash.SACForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                ViewBag.Sac = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.Sac = null;
            }
            return PartialView();
        }
        

        [HttpPost]
        public JsonResult GetMiscInvoiceAmountMultiChrg(string purpose, string purposecode, string SAC, float GST, string InvoiceType, string SEZ, int PartyId, decimal Amount)
        {
            Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, purposecode, SAC, GST, InvoiceType, SEZ, PartyId, Amount);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceAmountMultiChrg(string purpose, string Size)
        {
            Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
            objChargeMaster.GetMiscAmount(purpose, Size);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoiceMultiChrg(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);
                var invoiceData = JsonConvert.DeserializeObject<KolMiscPostModel>(objForm["MiscInvModelJson"].ToString());

                IList<kol_MiscInvDtl> lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<kol_MiscInvDtl>>(objForm["BTTCargoEntryDtlJS"].ToString());
                string XML = Utility.CreateXML(lstBTTCargoEntryDtl);

                //var ChargesData = JsonConvert.DeserializeObject<kol_MiscInvDtl>(objForm["BTTCargoEntryDtlJS"].ToString());
                Kol_CashManagementRepository objChargeMaster = new Kol_CashManagementRepository();
                objChargeMaster.AddMiscInvMultiChrg(invoiceData, XML, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv", ExportUnder);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region

        [HttpGet]
        public ActionResult RefundFromPDA()
        {

            var objRepo = new Kol_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(AddMoneyToPDModelRefund m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Kol_CashManagementRepository();
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
                //using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                //{
                //    rh.GeneratePDF(PdfDirectory + fileName, pages);
                //}
                CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
                Kol_CashManagementRepository ObjRR = new Kol_CashManagementRepository();
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


        #endregion

        #region Yard Invoice Edit
        [HttpGet]
        public ActionResult EditYardInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("IMPYard");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetYardInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);

                    //
                    var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                    actual.ForEach(item =>
                    {
                        if (objPostPaymentSheet.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                        {
                            objPostPaymentSheet.ActualApplicable.Add(item.OperationCode);
                        }
                    });
                    //

                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Yard");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//

                    /***************BOL PRINT*****************/
                    /*var BOL = "";
                    objCharge.GetBOLForEmptyCont("Yard", objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                        BOL = objCharge.DBResponse.Data.ToString();*/
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
        #endregion

        #region Destuffing Payment Sheet Edit
        [HttpGet]
        public ActionResult EditDestufInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("IMPDest");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDestufInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Dest");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//
                    /**************BOL PRINT********************/
                    /*var BOL = "";
                    objCharge.GetBOL(objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                    {
                        BOL = objCharge.DBResponse.Data.ToString();
                    }*/
                    /*****************************************/
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
        #endregion

        #region Delivery Payment Sheet Edit

        [HttpGet]
        public ActionResult EditDeliveryInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("IMPDeli");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDeliveryInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Delv");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }

                    //****BOE List**************************************************************************
                    IList<Import.Models.PaymentSheetBOE> lstBOE = new List<Import.Models.PaymentSheetBOE>();
                    objPostPaymentSheet.lstContWiseAmount.ToList().ForEach(item =>
                    {
                        lstBOE.Add(new Import.Models.PaymentSheetBOE
                        {
                            CFSCode = item.CFSCode,
                            LineNo = item.LineNo,
                            Selected = true,
                            BOEDate = "",
                            BOENo = ""
                        });
                    });
                    //******Get BOE By ReqId****************************************************//
                    List<Import.Models.PaymentSheetBOE> BOEAll = new List<Import.Models.PaymentSheetBOE>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Delv");
                    if (objImportRepo.DBResponse.Status == 1)
                    {
                        BOEAll = JsonConvert.DeserializeObject<List<Import.Models.PaymentSheetBOE>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));

                        //------------------------------------------------------
                        //BOE NO/BOE Date  Setting to lstBOE
                        BOEAll.ForEach(i =>
                        {
                            foreach (Import.Models.PaymentSheetBOE j in lstBOE)
                            {
                                if (i.CFSCode == j.CFSCode && i.LineNo == j.LineNo)
                                {
                                    j.BOENo = i.BOENo;
                                    j.BOEDate = i.BOEDate;
                                }
                            }
                        });
                        //--------------------------------------------------------
                        BOEAll.Where(o1 => !lstBOE.Any(o2 => o1.CFSCode == o2.CFSCode && o1.LineNo == o2.LineNo)).ToList().ForEach(item =>
                        {
                            lstBOE.Add(new Import.Models.PaymentSheetBOE
                            {
                                BOEDate = item.BOEDate,
                                BOENo = item.BOENo,
                                CFSCode = item.CFSCode,
                                LineNo = item.LineNo,
                                Selected = false,
                                // Size = item.Size
                            });
                        });

                    }

                    //*********************************************************************************//
                    /**************BOL PRINT********************/
                    var BOL = "";
                    objCharge.GetBOLForDeliverApp(objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                    {
                        BOL = objCharge.DBResponse.Data.ToString();
                    }
                    /*****************************************/
                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers, BOL = BOL, BOELIST = lstBOE }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Empty Container PaymentSheet Edit

        [HttpGet]
        public ActionResult EditEmptyContainerInvoice(string type = "YARD")
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
            if (type == "YARD")
            {
                objCashManagement.GetInvoiceForEdit("ECYard");
                ViewBag.SelectedType = "YARD";
            }
            else
            {
                objCashManagement.GetInvoiceForEdit("ECGodn");
                ViewBag.SelectedType = "GODOWN";

            }
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetEmptyContainerInvoiceDetails(int InvoiceId, string InvoiceFor)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, InvoiceFor == "Yard" ? "ECYard" : "ECGodn");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//
                    /*****************BOL PRINT*************/
                    /*var BOL = "";
                    objCharge.GetBOLForEmptyCont(InvoiceFor, objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                        BOL = objCharge.DBResponse.Data.ToString();*/
                    /***************************************/
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

        #endregion

        //****************************************************************************
        #region Export Invoice Edit
        [HttpGet]
        public ActionResult EditExportInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("EXP");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetExportInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges

                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ExportPS == 1).Select(o => new { Clause = o.Clause });
                    //var ApplicableHT = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5" || item1.OperationCode == "6")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstStorPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }

                    var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                    actual.ForEach(item =>
                    {
                        if (objPostPaymentSheet.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                        {
                            objPostPaymentSheet.ActualApplicable.Add(item.OperationCode);
                        }
                    });

                    var sortedString = JsonConvert.SerializeObject(objPostPaymentSheet.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);

                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    ExportRepository objExportRepo = new ExportRepository();
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "EXP");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//


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
        #endregion

        #region Load Container Payment Sheet Edit
        [HttpGet]
        public ActionResult EditLoadContainerInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("EXPLod");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetLoadContainerInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    ExportRepository objExportRepo = new ExportRepository();
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "LOADED");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//
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
        #endregion

        #region BTT Invoice Edit
        [HttpGet]
        public ActionResult EditBTTInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("BTT");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBTTInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    objPostPaymentSheet.CompGST = CompGST;
                    var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    objPostPaymentSheet.CompStateCode = CompStateCode;
                    var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);

                    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.ImportYardPS == 1).Select(o => new { Clause = o.Clause });
                    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    var HTCharges = 0M;
                    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    {
                        foreach (var item1 in item.ToList())
                        {
                            if (item1.OperationCode == "5")
                            {
                                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstPostPaymentCont.Where(o => o.Size == item1.Size && o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                            }
                            else
                            {
                                if (item1.OperationType == 5)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 4)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                                }
                                else if (item1.OperationType == 6)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                                }
                                else if (item1.OperationType == 7)
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                                }
                                else
                                {
                                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType), 2);
                                }
                            }
                        }
                        var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                        objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        {
                            ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                            Clause = item.Key,
                            ChargeName = item.FirstOrDefault().OperationSDesc,
                            ChargeType = "HT1",
                            SACCode = item.FirstOrDefault().SacCode,
                            Quantity = 0,
                            Rate = 0M,
                            Amount = HTCharges,
                            Discount = 0,
                            Taxable = HTCharges,
                            CGSTPer = cgst,
                            SGSTPer = sgst,
                            IGSTPer = igst,
                            CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                            SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                            IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                            Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                                (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                                (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                            ClauseOrder = clzOrder
                        });
                        HTCharges = 0M;
                    }
                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion

                    //#region H&T Charges
                    //try
                    //{
                    //    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    //    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    //    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    //    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);
                    //    var SelectedHT = objGenericCharges.ApplicableHT.Where(o => o.BTT == 1).Select(o => new { Clause = o.Clause });
                    //    var ApplicableHT = (Convert.ToInt32(System.Web.HttpContext.Current.Session["BranchId"]) == 1 ? (objGenericCharges.HTChargeRent.ToList()) : (objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList()));
                    //    var HTCharges = 0M;
                    //    foreach (var item in ApplicableHT.GroupBy(o => o.OperationCode).ToList())
                    //    {
                    //        foreach (var item1 in item.ToList())
                    //        {
                    //            if (item1.OperationCode == "5" || item1.OperationCode == "6" || item1.OperationCode == "11" || item1.OperationCode == "12")
                    //            {
                    //                HTCharges += Math.Round(item1.RateCWC * (Convert.ToDecimal(objPostPaymentSheet.lstStorPostPaymentCont.Where(o => o.CargoType == item1.CommodityType).Sum(o => o.GrossWt)) / 100), 2);
                    //            }
                    //            else if ((item1.OperationCode == "XXI-7" || item1.OperationCode == "XXI-2") && item1.OperationType == 5)
                    //            {
                    //                HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                    //            }
                    //            else
                    //            {
                    //                if (item1.OperationType == 5)
                    //                {
                    //                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 2 && o.HeavyScrap == 0), 2);
                    //                }
                    //                else if (item1.OperationType == 4)
                    //                {
                    //                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 1), 2);
                    //                }
                    //                else if (item1.OperationType == 6)
                    //                {
                    //                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.RMS == 0 && o.AppraisementPerct == 1 && o.HeavyScrap == 0), 2);
                    //                }
                    //                else if (item1.OperationType == 7)
                    //                {
                    //                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size && o.CargoType == item1.CommodityType && o.HeavyScrap == 1), 2);
                    //                }
                    //                else
                    //                {
                    //                    HTCharges += Math.Round(item1.RateCWC * objPostPaymentSheet.lstPostPaymentCont.Count(o => o.Size == item1.Size), 2);
                    //                }
                    //            }
                    //        }
                    //        //var clzOrder = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    //        //objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //        //{
                    //        //    ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                    //        //    Clause = item.Key,
                    //        //    ChargeName = item.FirstOrDefault().OperationSDesc,
                    //        //    ChargeType = "HT",
                    //        //    SACCode = item.FirstOrDefault().SacCode,
                    //        //    Quantity = 0,
                    //        //    Rate = 0M,
                    //        //    Amount = HTCharges,
                    //        //    Discount = 0,
                    //        //    Taxable = HTCharges,
                    //        //    CGSTPer = cgst,
                    //        //    SGSTPer = sgst,
                    //        //    IGSTPer = igst,
                    //        //    CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                    //        //    SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                    //        //    IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                    //        //    //Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                    //        //    Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                    //        //    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                    //        //    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                    //        //    (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                    //        //    ClauseOrder = clzOrder
                    //        //});
                    //        if (item.Key == "6")
                    //        {
                    //            //    var clzOrder1 = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder > 0 ? objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == item.Key).ClauseOrder : 0;
                    //            //    objPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //            //    {
                    //            //        ChargeId = objPostPaymentSheet.lstPostPaymentChrg.Count + 1,
                    //            //        Clause = item.Key,
                    //            //        ChargeName = item.FirstOrDefault().OperationSDesc,
                    //            //        ChargeType = "HT",
                    //            //        SACCode = item.FirstOrDefault().SacCode,
                    //            //        Quantity = 0,
                    //            //        Rate = 0M,
                    //            //        Amount = HTCharges,
                    //            //        Discount = 0,
                    //            //        Taxable = HTCharges,
                    //            //        CGSTPer = cgst,
                    //            //        SGSTPer = sgst,
                    //            //        IGSTPer = igst,
                    //            //        CGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                    //            //        SGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                    //            //        IGSTAmt = Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                    //            //        //Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges,
                    //            //        Total = objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                    //            //    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2)) +
                    //            //    (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2))) :
                    //            //    (HTCharges + (Math.Round(objPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2)))) : HTCharges,
                    //            //        ClauseOrder = clzOrder1
                    //            //    });
                    //        }
                    //        HTCharges = 0M;
                    //    }

                    //    var actual = objGenericCharges.HTChargeRent.Where(o => SelectedHT.Any(m => m.Clause.ToString().ToUpper() == o.OperationCode.ToString().ToUpper())).ToList();
                    //    actual.ForEach(item =>
                    //    {
                    //        if (objPostPaymentSheet.ActualApplicable.Count(o => o.Equals(item.OperationCode)) == 0)
                    //        {
                    //            objPostPaymentSheet.ActualApplicable.Add(item.OperationCode);
                    //        }
                    //    });
                    //    var sortedString = JsonConvert.SerializeObject(objPostPaymentSheet.lstPostPaymentChrg.OrderBy(o => o.ClauseOrder));
                    //    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    //    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //#endregion

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    ExportRepository objExportRepo = new ExportRepository();
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "BTT");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//
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
        #endregion

        //***************************************************************************
        #region Bond Advance Paymentsheet Edit
        [HttpGet]
        public ActionResult EditBondAdvanceInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("BNDadv");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondAdvInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                BondRepository objImportRepo = new BondRepository();

                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEditBond(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    BondPostPaymentSheet objPostPaymentSheet = (BondPostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);


                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion




                    return Json(new { Status = 1, Data = objPostPaymentSheet }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSacDetailsById(int SacId)
        {
            BondRepository objRepo = new BondRepository();
            objRepo.GetSACNoForAdvBondPaymentSheetById(SacId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        #endregion

        #region Bond Unloading Paymentsheet Edit

        [HttpGet]
        public ActionResult EditBondUnloadingInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("BNDUnld");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondUnloadingInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                BondRepository objImportRepo = new BondRepository();

                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEditBond(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    BondPostPaymentSheet objPostPaymentSheet = (BondPostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);


                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion




                    return Json(new { Status = 1, Data = objPostPaymentSheet }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSACForBondUnloadingById(int SacId)
        {
            BondRepository objRepo = new BondRepository();
            objRepo.GetSACForBondUnloadingById(SacId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        #endregion

        #region Bond Delivery Paymentsheet Edit
        [HttpGet]
        public ActionResult EditBondDeliveryInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("BND");
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondDeliveryInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                BondRepository objImportRepo = new BondRepository();

                objCharge.GetAllCharges();
                Kol_CashManagementRepository objCashManagement = new Kol_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEditBond(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    BondPostPaymentSheet objPostPaymentSheet = (BondPostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    //var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    //objPostPaymentSheet.CompGST = CompGST;
                    //var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    //objPostPaymentSheet.CompStateCode = CompStateCode;
                    //var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    //objPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    #region H&T Charges
                    var GSTType = objPostPaymentSheet.PartyStateCode == objPostPaymentSheet.CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.HTChargeRent.FirstOrDefault().SacCode).Gst);


                    var HT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT1").OrderBy(o => o.ClauseOrder).ToList();
                    var NonHT1Types = objPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType != "HT1").ToList();
                    NonHT1Types.AddRange(HT1Types);
                    var sortedString = JsonConvert.SerializeObject(NonHT1Types);
                    objPostPaymentSheet.lstPostPaymentChrg.Clear();
                    objPostPaymentSheet.lstPostPaymentChrg = JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(sortedString);
                    #endregion




                    return Json(new { Status = 1, Data = objPostPaymentSheet }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSACForBondDeliveryById(int SacId)
        {
            BondRepository objRepo = new BondRepository();
            objRepo.GetSACForBondDeliveryById(SacId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        #endregion

        #region Save Invoice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "", CargoXML);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateExpPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
                Decimal Weight = Convert.ToDecimal(objForm[17]);
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXP", Weight,"");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateBttExpPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
                Decimal Weight = Convert.ToDecimal(objForm[17]);
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", Weight,"");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateExpLoadedPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
               
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditLoadedInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod","", 0, 0, 0);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateDeliPaymentSheet(FormCollection objForm)
        {
            try
            {
                Decimal Weight = Convert.ToDecimal(objForm[18]);
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", Weight, CargoXML);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateEmptyPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                Decimal Weight = Convert.ToDecimal(objForm[18]);
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditECInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "", Weight, "","");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateDestuffingPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditDestuffingInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "", "", 0, 0,0,0, CargoXML);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePaymentSheetBond(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<BondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                BondRepository objChargeMaster = new BondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDadv");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateYardPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CargoXML = "";
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
                    ContainerXML = (Utility.CreateXML(invoiceData.lstPostPaymentCont)).Replace("T00:00:00", string.Empty);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmount != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditYardInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "","",0,0,0,0, CargoXML);
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
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

        //***************************************************************************

        #region BankDeposit
        public ActionResult BankDeposit()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddEditBankDeposit(KolBankDeposit obj)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            string varXml = Utility.CreateXML(obj.ExpensesDetails);
            objRepo.AddEditBankDeposit(obj, varXml,((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBankDeposit(int Id)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.DeleteBankDeposit(Id, ((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetBankDepositList()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetBankDepositList();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNEFTForBankDeposit(string dt)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetNEFTForBankDeposit(Convert.ToDateTime(dt));
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpenseHeadWithBalance()
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.ExpenseBankDeposit();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReceiptVoucherBalance(string HeadId, string DSNo)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.ReceiptVoucherBalance(HeadId, DSNo);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDepositListById(int id)
        {
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            objRepo.GetBankDepositList(id);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Cancel Invoice


        [HttpGet]
        public ActionResult CancelInvoice()
        {
            CancelInvoice cin = new CancelInvoice();
            var InvoiceNo = "";
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();
            objcancle.ListOfCancleInvoice(InvoiceNo);

            if (objcancle.DBResponse.Data != null)
            {
                ViewBag.lstInvoice = new SelectList((List<CancelInvoice>)objcancle.DBResponse.Data, "InvoiceId", "InvoiceNo");
            }
            else
            {
                ViewBag.lstInvoice = null;
            }
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult ViewCancelInvoice(int InvoiceId)
        {
            CancelInvoice cin = new CancelInvoice();
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Kol_CashManagementRepository objCR = new Kol_CashManagementRepository();
            objCR.LstOfCancleInvoice(InvoiceNo, Page);
            List<CancelInvoice> lstInvoice = new List<Models.CancelInvoice>();
            if (objCR.DBResponse.Data != null)
            {
                lstInvoice = (List<CancelInvoice>)objCR.DBResponse.Data;
            }

            return PartialView("LstOfCancleInvoice", lstInvoice);
        }

        [HttpGet]
        public ActionResult GetLoadMoreCancleInvoiceList(string InvoiceNo = "", int Page = 0)
        {
            Kol_CashManagementRepository objCR = new Kol_CashManagementRepository();
            objCR.LstOfCancleInvoice(InvoiceNo, Page);
            List<CancelInvoice> lstInvoice = new List<Models.CancelInvoice>();
            if (objCR.DBResponse.Data != null)
            {
                lstInvoice = (List<CancelInvoice>)objCR.DBResponse.Data;
            }
            return Json(lstInvoice, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public ActionResult LstOfCancleInvoice(string invoiceno)
        //{
        //    WFLD_CashManagementRepository objCR = new WFLD_CashManagementRepository();
        //    objCR.LstOfCancleInvoice(invoiceno);
        //    List<CancelInvoice> lstInvoice = new List<Models.CancelInvoice>();
        //    if (objCR.DBResponse.Data != null)
        //        lstInvoice = (List<CancelInvoice>)objCR.DBResponse.Data;
        //    return PartialView(lstInvoice);
        //}


        public JsonResult SearchCancleInvoice(string InvoiceNo)
        {
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Kol_CashManagementRepository objCancelInv = new Kol_CashManagementRepository();
            objCancelInv.GetHeaderIRNForInvoice();

            HeaderParam Hp = (HeaderParam)objCancelInv.DBResponse.Data;

            //string jsonEInvoice = JsonConvert.SerializeObject(Output);
            string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

            Einvoice Eobj = new Einvoice(Hp, "");
            CancelIrnResponse ERes = await Eobj.CancelEinvoice(Irn, CancelReason, CancelRemark);

            return Json(ERes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCancleInvoice(CancelInvoice objCancelInvoice)
        {

            Kol_CashManagementRepository ObjIR = new Kol_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

        }


        #endregion

        #region BulkDebitnoteReport
        [HttpGet]
        public ActionResult BulkDebitnoteReport()
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
        public JsonResult PrintDRNote(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            objRepo.PrintDetailsForBulkCRNote(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "D");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
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
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR)
        {
            Einvoice obj = new Einvoice();
            string note = "Debit Note";

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
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr>");

                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                //html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwcwfdcfs@gmail.com</label></td>");
                //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO_IMG'/></td></tr>");
                html.Append("<tr><td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");

                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + "  </td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                }







                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src=''/> </td>");

                html.Append("</tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                //html.Append("<br /><label style='font-size: 14px; font-weight:bold;'></label>");
                //html.Append("</td></tr>");

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
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
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
        #region BulkCreaditNoteReport
        [HttpGet]
        public ActionResult BulkCreaditNoteReport()
        {

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }



        [NonAction]
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR, string note)
        {
            Einvoice obj = new Einvoice();

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
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr>");

                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                // html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwcwfdcfs@gmail.com</label></td>");
                //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO_IMG'/></td></tr>");
                html.Append("<tr><td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");

                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + "  </td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                }







                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src=''/> </td>");

                html.Append("</tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                //html.Append("<br /><label style='font-size: 14px; font-weight:bold;'></label>");
                //html.Append("</td></tr>");

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
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
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

                //html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
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
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            Kol_CashManagementRepository objRepo = new Kol_CashManagementRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            string Note = "CREDIT NOTE";

            objRepo.PrintDetailsForBulkCRNote(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "C");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR, Note);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }
        #endregion
       

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


        #region Direct Online Payment
        public ActionResult DirectOnlinePayment()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DirectPaymentVoucher(Kol_DirectOnlinePayment objDOP)
        {
            Kol_CashManagementRepository ObjIR = new Kol_CashManagementRepository();
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
            List<Kol_DirectOnlinePayment> lstDOP = new List<Kol_DirectOnlinePayment>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();

            obj.GetOnlinePayAckList(((Login)(Session["LoginUser"])).Uid, OrderId);
            lstDOP = (List<Kol_DirectOnlinePayment>)obj.DBResponse.Data;
            return PartialView(lstDOP);
        }

        [HttpPost]
        public JsonResult ConfirmPayment(Kol_DirectOnlinePayment vm)
        {
            vm.OrderId = Convert.ToInt64(Session["OrderId"].ToString());

            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetOnlineConfirmPayment(Convert.ToDecimal(vm.TotalPaymentAmount), vm.OrderId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Online Payment Receipt

        public ActionResult OnlinePaymentReceipt()
        {
            return PartialView();
        }

        public ActionResult OnlinePaymentReceiptDetails(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.OnlinePaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetOnlinePaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptList(int Pages)
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlinePaymentReceiptList("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Online Payment Against Invoice

        public ActionResult OnlinePaymentAgainstInvoice()
        {
            return PartialView();
        }

        public JsonResult ListOfPendingInvoice()
        {
            Kol_CashManagementRepository objcancle = new Kol_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            objcancle.ListOfPendingInvoice(Uid);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnlinePaymentAgainstInvoice(Kol_OnlinePaymentAgainstInvoice objDOP)
        {
            Kol_CashManagementRepository ObjIR = new Kol_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<Kol_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
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
            List<Kol_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Kol_OnlinePaymentAgainstInvoice>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<Kol_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
            IList<Kol_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Kol_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<Kol_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
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
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetOnlinePaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlinePaymentReceiptListAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Online Payment Receipt

        public ActionResult OnlineOAPaymentReceipt()
        {
            return PartialView();
        }

        public ActionResult OnlineOAPaymentReceiptDetails(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.OnlineOAPaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlineOAPaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.GetOnlineOAPaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlineOAPaymentReceiptList(int Pages)
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlineOAPaymentReceiptList("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Transaction Status Enquiry

        public ActionResult TransactionStatusEnquiry()
        {
            return PartialView();
        }
        public ActionResult TransactionStatusEnquiryCcAvn()
        {
            return PartialView();
        }

        public JsonResult GetInvoiceListForTransactionStatusEnquiry(string InvoiceNo, int Page)
        {
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetInvoiceNoForTransactionStatusEnquiry(InvoiceNo, Page);
            List<TransactionStatusEnquiry> lstCancel = new List<TransactionStatusEnquiry>();
            if (obj.DBResponse.Data != null)
            {
                lstCancel = (List<TransactionStatusEnquiry>)obj.DBResponse.Data;
            }

            return Json(lstCancel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetTransactionStatusEnquiry(TransactionStatusEnquiry vm)
        {
            log.Info("GetTransactionStatusEnquiry START");
            Kol_CashManagementRepository objIR = null;
            var environment = System.Configuration.ConfigurationSettings.AppSettings["Environment"].ToString();
            string apiUrl = "";

            log.Info("Environment :" + environment);
            if (environment == "P")
            {
                apiUrl = QRApiEndPoints.GetEndpoint("PQRUrl");
            }
            else
            {
                apiUrl = QRApiEndPoints.GetEndpoint("TQRUrl");
            }
            log.Info("apiUrl :" + apiUrl);
            log.Info("After url");

            string certpath = System.Configuration.ConfigurationSettings.AppSettings["QRDSCPATH"].ToString();

            RijndaelManaged objrij = new RijndaelManaged();
            objrij.GenerateIV();
            objrij.GenerateKey();
            ClsEncryptionDecryption security = new ClsEncryptionDecryption();
            string mKey = "";
            String encMsg = "";
            string MerchantId = "";
            try
            {
                objIR = new Kol_CashManagementRepository();
                objIR.GetQRRequestDetails();
                DataSet ds = new DataSet();

                if (objIR.DBResponse.Status == 1)
                {
                    ds = (DataSet)objIR.DBResponse.Data;
                }

                //string k = "5931b25ba408d674059f817c283c9431";
                mKey = ds.Tables[0].Rows[0]["mKey"].ToString();
                MerchantId = ds.Tables[0].Rows[0]["MerchantID"].ToString();
                //string sk= Encoding.Unicode.GetString(objrij.Key);
                string v = ByteArrayToHexString(objrij.IV);

                string reqString = TransactionJsonFormat.BindQRTransactionJson(ds, vm.InvoiceNo);
                log.Info("Before Encryption ReqString" + reqString);
                encMsg = security.Encryption(reqString, mKey);
                log.Info("After Encryption EncMsg" + encMsg);
                //string decMsg = security.Decryption(encMsg, mKey);                

            }
            catch (Exception e)
            {


                string s = e.Message;
                log.Error("Err  :" + s);
            }
            TokenResponse tr = new TokenResponse();

            X509Certificate2 clientCertificate = new X509Certificate2();
            clientCertificate.Import(certpath);

            WebRequestHandler handler = new WebRequestHandler();
            handler.ClientCertificates.Add(clientCertificate);

            //var RMgs = ReadJsonFromFile();

            //log.Info("Bind Response Data for Save start");
            //var QRTStr = QRTResponseData.BindQRTResponseData(RMgs.ToString());
            //string JSON_XML = "";
            //if (QRTStr!=null)
            //{
            //    JSON_XML = Utility.CreateXML(QRTStr);
            //}

            //log.Info("Bind Response Data for Save end");

            //if(JSON_XML!="")
            //{
            //    log.Info("Response Data for Save start");
            //    Ppg_CashManagementRepository objAD = new Ppg_CashManagementRepository();
            //    objAD.AddQRTransactionAck(JSON_XML, vm.InvoiceId);
            //    log.Info("Response Data for Save end");
            //}

            string MsgStatus = "", MsgStatusDescription = "";
            using (var client = new HttpClient(handler))
            {

                string tmpData = "{\"requestMsg\":\"" + encMsg + "\",\"pgMerchantId\":\"" + MerchantId + "\"}";
                var data = new StringContent("{\"requestMsg\":\"" + encMsg + "\",\"pgMerchantId\":\"" + MerchantId + "\"}", Encoding.UTF8, "application/json");

                log.Info("Complete Request Msg   :" + tmpData);
                log.Info("apiUrl   :" + apiUrl);
                HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                string content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);
                log.Info("after apiUrl   :" + apiUrl);
                var V = joResponse["V"];
                var requestMsg = joResponse["requestMsg"];
                var keyid = joResponse["keyid"];
                var pgMerchantId = joResponse["pgMerchantId"];

                var vArray = Encoding.ASCII.GetBytes(V.ToString());

                log.Info("Before Decryption RequestMsg :" + requestMsg);

                var requestMsgDecrypt = security.Decryption(requestMsg.ToString(), mKey, V.ToString());
                log.Info("After Decryption RequestMsgDecrypt :" + requestMsgDecrypt);

                //--------------------------------------------

                var resultMsg = JsonConvert.DeserializeObject<QRTResponseJson>(requestMsgDecrypt);
                log.Info("Bind Response Data for Save start");
                var QRTStr = QRTResponseData.BindQRTResponseData(resultMsg);
                string JSON_XML = "";
                if (QRTStr != null)
                {
                    JSON_XML = Utility.CreateXML(QRTStr);
                }

                log.Info("Bind Response Data for Save end");

                if (JSON_XML != "")
                {
                    log.Info("Response Data for Save start");
                    objIR = new Kol_CashManagementRepository();
                    objIR.AddQRTransactionAck(JSON_XML, vm.InvoiceId);
                    log.Info("Response Data for Save end");
                }
                //--------------------------------------------
                if (objIR.DBResponse.Status == 1)
                {
                    MsgStatus = resultMsg.apiResp.status;
                    MsgStatusDescription = resultMsg.apiResp.statusDescription;


                    log.Info("DB AND API MSG START:" + "\r\n");

                    log.Info("API MSG :" + "\r\n" + "MsgStatus  >" + MsgStatus + "\r\n" + "MsgStatusDescription  >" + MsgStatusDescription);

                    log.Info("DB MSG  :" + "Status :" + objIR.DBResponse.Status + "\r\n" + "Message :" + objIR.DBResponse.Message);


                    log.Info("DB AND API MSG END:");

                }
                else
                {
                    MsgStatus = "Fail";
                    MsgStatusDescription = "Transaction Fail";

                    log.Error("DB AND API ERROR START:" + "\r\n");
                    log.Error("API MSG :" + "\r\n" + "MsgStatus  >" + MsgStatus + "\r\n" + "MsgStatusDescription  >" + MsgStatusDescription);

                    log.Error("DB MSG  :" + "Status :" + objIR.DBResponse.Status + "\r\n" + "Message :" + objIR.DBResponse.Message);


                    log.Error("DB AND API ERROR END:");



                }
            }

            return Json(new { Status = MsgStatus, Message = MsgStatusDescription }, JsonRequestBehavior.DenyGet);
            //return Json("");
        }
        [HttpPost]
        public async Task<JsonResult> GetTransactionStatusEnquiryCcAvn_1(TransactionStatusEnquiry vm)
        {
            try
            {
                /*https://logintest.ccavenue.com/apis/servlet/DoWebTrans?enc_request=b9e1ba391affc7ec9690764a6db47e71165c1dd1d98826661aea6b8c9b1043a2&access_code=AVXH61IL68AO51HXOA&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2*/


                string accessCode = "AVXH61IL68AO51HXOA";//from avenues
                string workingKey = "F52847EDA0C715911416D9054623BB3E";// from avenues

                //reference_no=tracking_id    AND order_no=order_id

                string orderStatusQueryJson = "{ \"reference_no\":\"311007894457\", \"order_no\":\"637774256280319861\" }"; //Ex. { "reference_no":"CCAvenue_Reference_No" , "order_no":"123456"} 
                string encJson = "";

                string queryUrl = "https://logintest.ccavenue.com/apis/servlet/DoWebTrans";


                CCACrypto ccaCrypto = new CCACrypto();
                encJson = ccaCrypto.Encrypt(orderStatusQueryJson, workingKey);

                // make query for the status of the order to ccAvenues change the command param as per your need
                string authQueryUrlParam = "enc_request=" + encJson + "&access_code=" + accessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2";

                // Url Connection
                String message = postPaymentRequestToGateway(queryUrl, authQueryUrlParam);
                //Response.Write(message);
                NameValueCollection param = getResponseMap(message);
                String status = "";
                String encResJson = "";
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
                        String ResJson = ccaCrypto.Decrypt(encResJson, workingKey);
                        Response.Write(ResJson);
                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        Console.WriteLine("failure response from ccAvenues: " + encResJson);
                    }

                }

            }
            catch (Exception exp)
            {
                Response.Write("Exception " + exp);

            }
            return Json(new { Status = "", Message = "" }, JsonRequestBehavior.DenyGet);
            // return Json(new { Status = MsgStatus, Message = MsgStatusDescription }, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public async Task<JsonResult> GetTransactionStatusEnquiryCcAvn(TransactionStatusEnquiry vm)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionStatusEnquiry CcAvn START");
            Kol_CashManagementRepository objIR = null;
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
                objIR = new Kol_CashManagementRepository();
                objIR.GetQRRequestDetails();
                DataSet ds = new DataSet();

                if (objIR.DBResponse.Status == 1)
                {
                    ds = (DataSet)objIR.DBResponse.Data;
                }


                WorkingKey = UPIConfiguration.WorkingKey;
                MerchantId = ds.Tables[0].Rows[0]["MerchantID"].ToString();


                string OrderId = "";
                objIR.GetOrderNoForTransactionStatusEnquiry(vm.InvoiceId);
                if (objIR.DBResponse.Status == 1)
                {
                    OrderId = objIR.DBResponse.Data.ToString();
                }

                string reqString = TransactionJsonFormat.BindCcavnTransactionJson(vm.InvoiceId, OrderId);
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
                //https://logintest.ccavenue.com/apis/servlet/DoWebTrans?enc_request=&access_code=&request_type=JSON&response_type=JSON&command=orderStatusTracker&version=1.2 
                //enc_request=63957FB55DD6E7B968A7588763E08B240878046EF2F520C44BBC63FB9CCE726209A4734877F5904445591304ABB2F5E598B951E39EAFB9A24584B00590ADB077ADE5E8C444EAC5A250B1EA96F68D22E44EA2515401C2CD753DBA91BD0E7DFE7341BE1E7B7550&access_code=8JXENNSSBEZCU8KQ&command=confirmOrder&request_type=XML&response_type=XML&version=1.1
                //string tmpData = "{\"requestMsg\":\"" + encMsg + "\",\"pgMerchantId\":\"" + MerchantId + "\"}";
                //var data = new StringContent("{\"requestMsg\":\"" + encMsg + "\",\"pgMerchantId\":\"" + MerchantId + "\"}", Encoding.UTF8, "application/json");
                //string authQueryUrlParam = "Enc_request=" + encMsg + "&access_code=" + UPIConfiguration.AccessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2";
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
                        ResJson = ccaCrypto.Decrypt(encResJson, UPIConfiguration.WorkingKey); // security.Decryption(encResJson, UPIConfiguration.WorkingKey);
                        CcAvnResponseJsonModel ccAvnResponse = JsonConvert.DeserializeObject<CcAvnResponseJsonModel>(ResJson);
                        Kol_CashManagementRepository objCash = new Kol_CashManagementRepository();
                        objCash.AddCcAvnueTransactionUpdate(ccAvnResponse);

                        return Json(new { Status = objCash.DBResponse.Status, Message = ccAvnResponse.error_code == "" ? MsgStatusDescription : ccAvnResponse.error_desc, resdata = ResJson }, JsonRequestBehavior.AllowGet);
                        //Response.Write(ResJson);
                    }
                    else if (!"".Equals(status) && status.Equals("1"))
                    {
                        MsgStatus = "Fail";
                        MsgStatusDescription = "Fail";
                        return Json(new { Status = 0, Message = "failure response from ccAvenues: ", resdata = "" }, JsonRequestBehavior.AllowGet);
                        //Console.WriteLine("failure response from ccAvenues: " + encResJson);
                    }

                }


                log.Info("After Decryption ResJson :" + ResJson);

            }

            return Json(new { Status = MsgStatus, Message = MsgStatusDescription }, JsonRequestBehavior.DenyGet);
            //return Json("");
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

        [HttpPost, ValidateInput(false)]
        // [CustomValidateAntiForgeryToken]
        public JsonResult GenerateDeStuffingReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                string OrderId = fc["OrderId"].ToString();
                var Pages = new string[1];
                var FileName = OrderId + "ReceiptOrderId.pdf";
                Pages[0] = fc["Page"].ToString();

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + OrderId + "/Report/Receipt/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = "";
                    ObjPdf.HOAddress = "";
                    ObjPdf.ZonalOffice = "";
                    ObjPdf.ZOAddress = "";
                    ObjPdf.GeneratePDFWithoutFooter(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + OrderId + "/Report/Receipt/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }


        #region Acknowledgement View List
        public ActionResult AcknowledgementView()
        {
            return PartialView();
        }

        public ActionResult AcknowledgementViewList(string PeriodFrom, string PeriodTo)
        {
            if (String.IsNullOrEmpty(PeriodFrom))
            {
                PeriodFrom = null;
            }
            if (String.IsNullOrEmpty(PeriodTo))
            {
                PeriodTo = null;
            }
            List<Kol_AcknowledgementViewList> lstOPReceipt = new List<Kol_AcknowledgementViewList>();
            Kol_CashManagementRepository obj = new Kol_CashManagementRepository();
            obj.AcknowledgementViewList(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<Kol_AcknowledgementViewList>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }
        #endregion

        #region Pull
        public async Task<JsonResult> GetAllBQRDataPull()
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
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



            return Json("", JsonRequestBehavior.AllowGet);
        }




        public async Task<JsonResult> GetTransactionDetailsBQR(string OrderId)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionStatusEnquiry BQR START");
            Kol_CashManagementRepository objIR = null;
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


                String status = "";
                String encResJson = "";
                String ResJson = "";


                NameValueCollection param = getResponseMap(message);
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
                        if (string.IsNullOrEmpty(ccAvnResponse.error_code))
                        {
                            Amd_CashManagementRepository objCash = new Amd_CashManagementRepository();
                            objCash.AddPaymentGatewayResponseBQR(ccAvnResponse);
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
        #endregion

        #region Pull CCAvenue
        public async Task<JsonResult> GetAllCCAvenueDataPull()
        {
            Kol_CashManagementRepository objIR = new Kol_CashManagementRepository();
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
            Amd_CashManagementRepository objIR = null;
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
                        Kol_CashManagementRepository objCash = new Kol_CashManagementRepository();
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
        #endregion

        #region BQR Payment Receipt Invoice

        public ActionResult BQRPaymentReceiptAgainstInvoice()
        {
            return PartialView();
        }

        public ActionResult BQRPaymentReceiptDetailsAgainstInvoice(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            WFLD_CashManagementRepository obj = new WFLD_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            WFLD_CashManagementRepository obj = new WFLD_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            WFLD_CashManagementRepository objIR = new WFLD_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetBQRPaymentReceiptListAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}