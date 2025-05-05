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

namespace CwcExim.Areas.CashManagement.Controllers
{

    public class Ppg_CashManagementController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region-- ADD MONEY TO PD --


        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

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
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            var objRepo = new Ppg_CashManagementRepository();
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
                    var objRepo = new Ppg_CashManagementRepository();
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


        #endregion

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        private string strQRCode = "000201010211021645992400041983220415545927000419832061661000500041983220827PUNB0042000420001210003366226410010A00000052401230514200A0000151.mab@pnb27260010A00000052401080601414A5204939953033565802IN5923CENTRAL WAREHOUSING COR6008GHAZIPUR6106110096621207080601414A6304b8e4";


        public Ppg_CashManagementController()
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

        #region ---- Payment Receipt/Cash Receipt ----

        [HttpGet]
        public ActionResult CashReceipt(int PartyId = 0, string PartyName = "", string Type = "INVOICE")
        {
            Kol_CashReceiptModel ObjCashReceipt = new Kol_CashReceiptModel();

            var objRepo = new Ppg_CashManagementRepository();
            if (PartyId == 0)
            {
                // objRepo.GetPartyList();
                //  if (objRepo.DBResponse.Data != null)
                //   ViewBag.Party = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                // else
                //  ViewBag.Invoice = null;
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
                // objRepo.GetPartyList();
                // if (objRepo.DBResponse.Data != null)
                //   ViewBag.Party = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                // else
                //   ViewBag.Invoice = null;
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

                for (var i = 0; i < 7; i++)
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
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                //objRepo.GetCashRcptPrint(PartyId);
                //if (objRepo.DBResponse.Data != null)
                //{
                //    ViewBag.CashPrint = JsonConvert.SerializeObject(((PostPaymentSheet)objRepo.DBResponse.Data));
                //}
                //else
                //{
                //    ViewBag.CashPrint = null;
                //}

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
        public ActionResult AddCashReceipt(Kol_CashReceiptModel ObjCashReceipt)
        {
            List<CashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<CashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<CashReceiptInvoiveMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.InvoiceDate = dt.ToString("yyyy-MM-dd");
                ObjCashReceipt.InvoiceValue = ObjCashReceipt.InvoiceValue + item.InvoiceAmt;
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

            // ObjCashReceipt.BranchId = Convert.ToInt32(Session["BranchId"]);
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.AddCashReceipt(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrint(int CashReceiptId)
        {
            var objRepo = new Ppg_CashManagementRepository();
            var model = new PostPaymentSheet();
            objRepo.GetCashRcptPrint(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePrintHtml(FormCollection fc)
        {
            int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
            string htmlPrint = fc["htmlPrint"].ToString();
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.UpdatePrintHtml(CashReceiptId, htmlPrint);
            return Json(objRepo.DBResponse.Status, JsonRequestBehavior.AllowGet);
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
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.ListOfPayeeForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayeeList(string PartyCode, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.ListOfPayeeForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region kol Cash Management Copy




        #region--PAYMENT VOUCHER--

        [HttpGet]
        public ActionResult PaymentVoucher()
        {
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPaymentVoucherCreateInfo();
            ViewData["COMGST"] = ((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).UserGST;
            ViewBag.Expenses = JsonConvert.SerializeObject(((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).Expenses);
            ViewBag.ExpHSN = JsonConvert.SerializeObject(((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).ExpHSN);
            ViewBag.HSN = JsonConvert.SerializeObject(((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).HSN);
            ViewBag.Parties = ((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).Party;
            ViewData["InvoiceNo"] = ((Ppg_PaymentVoucherInfo)objRepo.DBResponse.Data).VoucherId;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PaymentVoucherPrint(int PVId)
        {

            Ppg_CashManagementRepository ObjRR = new Ppg_CashManagementRepository();
            Ppg_PaymentVoucher LstSeal = new Ppg_PaymentVoucher();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PaymentVoucherPrint(PVId);//, objLogin.Uid
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                //LstSeal = (List<Kol_NewPaymentValucherModel>)ObjRR.DBResponse.Data;

                // LstSeal = (List<Kol_NewPaymentValucherModel>)ObjRR.DBResponse.Data;
                LstSeal = (Ppg_PaymentVoucher)ObjRR.DBResponse.Data;
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
        public string GeneratePaymentPDF(Ppg_PaymentVoucher LstSeal, int PVId)
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

                Pages.Append("<tr><td colspan='6' width='50%' cellpadding='5' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'><b>Name : </b>" + LstSeal.CompanyName + "</td>");
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
                Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>IGST</td> <td colspan='6' width='50%' cellpadding='5'>" + LstSeal.TotalIGST + "</td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td>");
                Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalIGSTAmt, 2) + "</th></tr>");

                Pages.Append("<tr><td colspan='7' width='70%' cellpadding='0' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>");
                Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%;font-size:9pt;'><tbody>");
                Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>CGST</td> <td colspan='6' width='50%' cellpadding='5'>" + LstSeal.TotalCGST + "</td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td>");
                Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalCGSTAmt, 2) + "</th></tr>");

                Pages.Append("<tr><td colspan='7' width='70%' cellpadding='0' style='border-bottom: 1px solid #000;border-right: 1px solid #000;'>");
                Pages.Append("<table cellspacing='0' cellpadding='0' style='width:100%;font-size:9pt;'><tbody>");
                Pages.Append("<tr><td colspan='6' cellpadding='5' style='border-right: 1px solid #000;' width='50%'>SGST</td> <td colspan='6' width='50%' cellpadding='5'>" + LstSeal.TotalSGST + "</td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td>");
                Pages.Append("<th colspan='3' width='30%' cellpadding='5' style='border-bottom: 1px solid #000;text-align:right;'>" + Math.Round(LstSeal.TotalSGSTAmt, 2) + "</th></tr>");

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
        public JsonResult PaymentVoucher(Ppg_PaymentVoucher m)
        {




            if (ModelState.IsValid)
            {
                var objRepo = new Ppg_CashManagementRepository();

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
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPaymentVoucherList();
            IEnumerable<Ppg_PaymentVoucher> lstPaymentVou = (IEnumerable<Ppg_PaymentVoucher>)objRepo.DBResponse.Data;
            if (lstPaymentVou != null)
            {
                return PartialView(lstPaymentVou);
            }
            else
            {
                return PartialView(new List<Ppg_PaymentVoucher>());
            }
        }

        #endregion

     

        #region Edit cash Receipt
        //[HttpGet]
        public ActionResult EditCashReceiptPaymentMode()//int InvoiceId = 0, string InvoiceNo = ""
        {
            Kol_CashReceiptModel ObjCashReceipt = new Kol_CashReceiptModel();
            var objRepo = new Ppg_CashManagementRepository();
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
            var objRepo = new Ppg_CashManagementRepository();
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
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.SaveEditedCashRcpt(objEditReceiptPayment, ((Login)(Session["LoginUser"])).Uid);

            return Json(objRepo.DBResponse);

        }
        #endregion

        #region-- RECEIVE VOUCHER --

        [HttpGet]
        public ActionResult ReceivedVoucher()
        {
            var objRepo = new Ppg_CashManagementRepository();
            //objRepo.GetPaymentVoucherCreateInfo();
            ViewData["InvoiceNo"] = objRepo.ReceiptVoucherNo();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReceiptVoucher(Ppg_ReceiptVoucherModel vm)
            {
            if(vm.InstrumentDate!=null)
            {
                try
                {
                 DateTime dt=   DateTime.ParseExact(vm.InstrumentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
              catch(Exception ex)
                {
                    ModelState.AddModelError("InstrumentDate", ex.Message);
                }
               

            }
           
           

            if (ModelState.IsValid)
            {
                var objRepo = new Ppg_CashManagementRepository();
                objRepo.AddNewReceiptVoucher(vm);
                return Json(new { Status = true, Message = "Received Saved Successfully", Data = "CWC/RV/" + objRepo.DBResponse.Data.ToString().PadLeft(7, '0') + "/" + DateTime.Today.Year.ToString(), Id = objRepo.DBResponse.Data.ToString() }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err, JsonRequestBehavior.AllowGet);
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
                using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 10f, 40f, 40f))
                {
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
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetReceiptVoucherList();
            IEnumerable<Ppg_ReceiptVoucherModel> lstRcptVou = (IEnumerable<Ppg_ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
                return PartialView(lstRcptVou);
            }
            else
            {
                return PartialView(new List<Ppg_ReceiptVoucherModel>());
            }
        }

        #endregion

        #region Credit Note
        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetAllInvoicenoforcreditnote("", 0);
            if (objRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstInvcNo = Jobject["lstInvcNo"];
                ViewBag.StateCr = Jobject["State"];
            }
            else
            {
                ViewBag.lstInvcNo = null;
            }
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpGet]
        public JsonResult SearchInvoice(string invoiceno)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            // List<SealCutting> LstJO = new List<SealCutting>();
            objRepo.GetAllInvoicenoforcreditnote(invoiceno, 0);
            // var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);

            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpGet]
        public JsonResult LoadcreditInvoiceLists(string invoiceno, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetAllInvoicenoforcreditnote(invoiceno, Page);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForCreaditNote(int InvoiceId)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetDebitNoteDetailsForCreaditNote(int InvoiceId)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetDebitNoteDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }

        //Get InvoiceNo against Invoice or Debit Note
        [HttpGet]
        public JsonResult GetInvoiceNoCreaditNote(string InvoiceType)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();

            if(InvoiceType=="Invoice")
            objRepo.GetInvoiceNoForCreaditNote("C");
            else
            objRepo.GetInvoiceNoAgainstDebitForCreaditNote();
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(PpgCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value credit note can not be saved." });
                }
                else
                {
                    Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
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
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public ActionResult ListOfCRNote()
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchCreditNote(string Search)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.SearchCreditDebitNote("C", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfCRNote", lstNote);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(int CRNoteId, string Note)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
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

            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
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

            Einvoice Eobj = new Einvoice();
            B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
            objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
            IrnResponse objERes = new IrnResponse();
            objERes.SignedQRCode = objresponse.QrCodeBase64;
            objERes.SignedInvoice = objresponse.QrCodeJson;
            objERes.SignedQRCode = objresponse.QrCodeJson;

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
                    //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td>");
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");

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
            if (objCR.SupplyType != "B2C")
            {
                html.Append("<tr>");
                html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                html.Append("</tr>");
            }

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
        public JsonResult AddCashColAgnBncCheque(PPG_CashColAgnBncChq objCashColAgnBncChq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
                    objRepo.AddEditCashCollectionChq(objCashColAgnBncChq);
                    int Data = Convert.ToInt32(objRepo.DBResponse.Data);
                    string Message = objRepo.DBResponse.Message;
                    int Status = objRepo.DBResponse.Status;
                    if (objRepo.DBResponse.Status == 1)
                    {
                        objRepo.GetInvoiceAndCashReceipt(Convert.ToInt32(Data));
                        //   objRepo.UpdateCCInvoiceAndCashReceipt(Convert.ToInt32(Data), InvoiceHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data)
                        //     CashReceiptHtml((CashColAgnBncChqPrint)objRepo.DBResponse.Data));
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





        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult ChqBounceInvoicePrint(string InvoiceNo)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetInvoiceDetailsForChqBouncePrintByNo(InvoiceNo, "CC");
            PpgInvoiceYard objGP = new PpgInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (PpgInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceChqBounce(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        private string GeneratingPDFInvoiceChqBounce(PpgInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/ChqBounceInvoice" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            System.Text.StringBuilder html = new System.Text.StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
            html.Append("<br />Payment aganist Cheque Bounce");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
            html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + objGP.InvoiceNo + "</span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + objGP.InvoiceDate + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
            html.Append("<span>" + objGP.PartyName + "</span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + objGP.PartyState + "</span> </td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
            html.Append("Party Address :</label> <span>" + objGP.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
            html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + objGP.PartyStateCode + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + objGP.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Carting</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            //   html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Week</th>");

            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.CartingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DestuffingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                //     html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");

                // html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (container.CargoType == 1 ? "Haz" : "Non-Haz") + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            html.Append("</tbody></table></td></tr>");

            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 12px;'>Shipping Line: " + objGP.ShippingLineName + " <br />");
            html.Append("Shipping No:  <br />");
            html.Append("OBL No:   &nbsp;&nbsp; ItemNo&nbsp;  BOE No&nbsp; : " + objGP.BOENo + "&nbsp;&nbsp;BOE Date: " + objGP.BOEDate + " <br />");
            html.Append("Importer:" + objGP.ImporterExporter + "   &nbsp;&nbsp; VALUE:" + objGP.lstPostPaymentCont.Sum(o => o.CIFValue).ToString() + "&nbsp;&nbsp;DUTY:" + objGP.lstPostPaymentCont.Sum(o => o.Duty).ToString() + "");
            html.Append("&nbsp;=&nbsp;" + (objGP.lstPostPaymentCont.Sum(o => o.CIFValue) + objGP.lstPostPaymentCont.Sum(o => o.Duty)).ToString() + "<br />");
            html.Append("CHA Name:&nbsp;" + objGP.CHAName + "<br />");
            html.Append("No Of Pkg:&nbsp;" + objGP.TotalNoOfPackages.ToString() + "&nbsp;Total Gross Wt.&nbsp;" + objGP.TotalGrossWt.ToString("0.00") + "<br />");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</table>");
            html.Append("</td></tr>");

            html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
            html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SR No.</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Charge Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Description</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>HSN Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Taxable Amt.</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>CGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>IGST</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Total</th></tr><tr>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th></tr></thead>");
            html.Append("<tbody>");
            i = 1;
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }

            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;width:50%;'>Total :</th>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>");
            html.Append("Total Invoice (In Word) :");







            //   html.Append("</tbody>");
            //   html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            //   html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            //  html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            //  html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            //  html.Append("Total Invoice (In Word) :");
            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='6'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");

            //  html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            //   html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='7'>Amount of Tax Subject of Reverse :");
            //  html.Append("0</th>");
            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/ChqBounceInvoice" + InvoiceId.ToString() + ".pdf";
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
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetAllInvoicenofordebitnote("", 0);
            if (objRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstInvcNo = Jobject["lstInvcNo"];
                ViewBag.StateCr = Jobject["State"];
            }
            else
            {
                ViewBag.lstInvcNo = null;
            }
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");



            return PartialView();
        }
        [HttpGet]
        public JsonResult SearchdebitInvoice(string invoiceno)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            //  List<SealCutting> LstJO = new List<SealCutting>();
            objRepo.GetAllInvoicenofordebitnote(invoiceno, 0);
            // var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);

            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpGet]
        public JsonResult LoadInvoicedebitLists(string invoiceno, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetAllInvoicenofordebitnote(invoiceno, Page);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddDebitNote(PpgCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value debit note can not be saved." });
                }
                else
                {
                    Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
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
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public ActionResult ListOfDRNote()
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchDebitNote(string Search)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.SearchCreditDebitNote("D", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfDRNote", lstNote);
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetChargesListForCrDb(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
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
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
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
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</td></tr><tr><td>(c) Customs Examination Date " + actualJson.lstInvoiceHeader[0].CstmExaminationDate + "</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
            html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

            var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;width:20%;'>Particulars</th><th style='border:1px solid #000;'>SAC</th><th style='border:1px solid #000;'>Value</th><th style='border:1px solid #000;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
            var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC");
            foreach (var item in cwcCharges)
            {
                html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
            };
            html2 += "<tr><td style='border:1px solid #000;padding-left:25px;'>CWC TDS</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;text-align:center;'>" + Math.Round(actualJson.lstInvoiceHeader[0].CWCTDS) + "</td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td><td style='border:1px solid #000;'></td></tr>";
            var htCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "HT");
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
                html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].ArrivalDate + "</span><br/>Date & Time of Destuffing (FN/AN)/ Delivery. <span>" + actualJson.lstInvoiceHeader[0].DestuffingDate + "</span><br/>Name of Importer. <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>Bill of Entry No. <span>" + actualJson.lstInvoiceHeader[0].BOENo + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
                html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Destuffing(FN/AN)</td></tr><tr><td>(b) Date of Delivery" + actualJson.lstInvoiceHeader[0].InvoiceDate + "</td></tr><tr><td>(c) Customs Examination Date " + actualJson.lstInvoiceHeader[0].CstmExaminationDate + "</td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
                html1 += "<tr><td colspan='3'>Value of Cargo (CIF Value + Duty + Penalty) Rs.<span>" + actualJson.lstInvoiceHeader[0].TotalValueOfCargo + "</span></td></tr></tbody></table>";

                var html2 = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='6' style='vertical-align:top;text-align:left;'>Invoice No. <span>" + actualJson.lstInvoiceHeader[0].InvoiceNo + "</span></th><th colspan='6' style='text-align:right;padding-right:20pt;padding-bottom:50pt;'>Date: <span>" + Convert.ToDateTime(actualJson.lstInvoiceHeader[0].ReceiptDate).ToString("dd/MM/yyyy") + "</span></th></tr><tr><th style='border:1px solid #000;text-align:center;'>Particulars</th><th style='border:1px solid #000;text-align:center;'>SAC</th><th style='border:1px solid #000;text-align:center;'>Value</th><th style='border:1px solid #000;text-align:center;'>TDS</th><th style='border:1px solid #000;text-align:center;'>Discount</th><th colspan='6' style='border:1px solid #000;text-align:center;'>Taxes</th><th style='border:1px solid #000;text-align:center;'>Total Amount</th></tr><tr><th style='border:1px solid #000;border-bottom:none;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th rowspan='2' style='border:1px solid #000;'></th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>CGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>SGST</th><th style='border:1px solid #000;'>Rate</th><th style='border:1px solid #000;'>IGST</th><th style='border:1px solid #000;'></th></tr><tr><th style='border:1px solid #000;border-top:none;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th><th style='border:1px solid #000;'></th></tr></thead><tbody>";
                var cwcCharges = actualJson.lstInvoiceCharges.Where(o => o.ChargeType == "CWC");
                foreach (var item in cwcCharges)
                {
                    html2 += "<tr><td style='border:1px solid #000;'>" + item.Clause + ". " + item.ChargeName + "</td><td style='border:1px solid #000;'><span>" + item.SACCode + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Amount, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span></span></td><td style='border:1px solid #000;text-align:center;'><span>0.00</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.CGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.CGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.SGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.SGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + item.IGSTPer + "%</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.IGSTAmt, 2) + "</span></td><td style='border:1px solid #000;text-align:right;'><span>" + Math.Round(item.Total, 2) + "</span></td></tr>";
                }
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
                html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";

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
            html1 += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;margin-top:10pt;'><tr><td>(a) Date & Time of Stuffing(FN/AN) " + actualJson.lstInvoiceHeader[0].StuffingDate + "</td></tr><tr><td>(b) Date of Delivery " + actualJson.lstInvoiceHeader[0].InvoiceDate + "</td></tr><tr><td>(c) Customs Examination Date </td></tr></table></td></tr></tbody></table></td></tr></table></td></tr></tbody></table></td></tr>";
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
                html1 += "Shipping Line. <span>" + actualJson.lstInvoiceHeader[0].ShippingLinaName + "</span><br/>CFS Code No. <span>" + actualJson.lstInvoiceHeader[0].CFSCode + "</span><br/>Date & Time of Arrival (FN/AN): <span>" + actualJson.lstInvoiceHeader[0].ArrivalDate + "</span><br/>" + actualJson.lstInvoiceHeader[0].CartingDate + " <span>" + actualJson.lstInvoiceHeader[0].StuffingDate + "</span><br/>Name of Exporter:  <span>" + actualJson.lstInvoiceHeader[0].ExporterImporterName + "</span><br/>S.B. No. <span>" + actualJson.lstInvoiceHeader[0].BOENo + "</span><br/>Name of CHA. <span>" + actualJson.lstInvoiceHeader[0].CHAName + "</span><br/>No of Packages. <span>" + actualJson.lstInvoiceHeader[0].TotalNoOfPackages + "</span><br/>Total Gross Weight. <span>" + actualJson.lstInvoiceHeader[0].TotalGrossWt + "</span><br/>Gross Weight per Package. <span>" + actualJson.lstInvoiceHeader[0].TotalWtPerUnit + "</span><br/>Storage space occupied(" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupiedUnit + ") <span>" + actualJson.lstInvoiceHeader[0].TotalSpaceOccupied + "</span><br/>Chargeable period for Storage space.";
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
                html4 += t1 + "</tbody></table></td></tr></table><br/><b>CASHIER REMARKS:<span>" + actualJson.lstInvoiceHeader[0].CashierRemarks + "</span></b>";

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
            ImportRepository objImport = new ImportRepository();

            //objImport.GetPaymentParty();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            //PurposeListForInvc ObjPurpose = new PurposeListForInvc();
            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            Ppg_CashManagementRepository ObjCash = new Ppg_CashManagementRepository();
            ObjCash.PurposeListForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                lstPurpose = (List<SelectListItem>)ObjCash.DBResponse.Data;
                //lstPurpose.Add(new SelectListItem {Text = "Printing", Value="Printing" });
                //lstPurpose.Add(new SelectListItem { Text = "Banking", Value = "Banking" });
                //lstPurpose.Add(new SelectListItem { Text = "Photocopy", Value = "Photocopy" });
                //lstPurpose.Add(new SelectListItem { Text = "Cheque Return", Value = "Cheque Return" });
                //lstPurpose.Add(new SelectListItem { Text = "Others", Value = "Others" });
                ViewBag.PurposeList = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.PurposeList = null;
            }


            return PartialView();
        }


        [HttpGet]
        public JsonResult GetParty()
        {
            ImportRepository objImport = new ImportRepository();

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId, decimal Amount,string SEZ)
        {
            Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, InvoiceType, PartyId, Amount,SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }

        //  [HttpPost]

        //public JsonResult GetMisc( string InvoiceType, int PartyId, Decimal Amount)
        //{


        //    Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
        //    objChargeMaster.GetDebitInvoice(InvoiceType, PartyId, Amount);
        //    return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);


                var invoiceData = JsonConvert.DeserializeObject<MiscInvModel>(objForm["MiscInvModelJson"].ToString());



                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.AddMiscInv(invoiceData, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv");

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


        #region Refund From SD

        [HttpGet]
        public ActionResult RefundFromPDA()
        {

            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            return PartialView();
        }

        [HttpGet]
        public ActionResult SDRefundList()
        {
            List<PPGSDRefundList> ObjSd = new List<PPGSDRefundList>();
            Ppg_CashManagementRepository ObjCR = new Ppg_CashManagementRepository();
            ObjCR.GetSDRefundList();
            if (ObjCR.DBResponse.Data != null)
                ObjSd = (List<PPGSDRefundList>)ObjCR.DBResponse.Data;
            return PartialView(ObjSd);
        }

        [HttpGet]
        public ActionResult ViewSDRefund(int PdaAcId)
        {
            ViewSDRefund ObjSD = new ViewSDRefund();
            Ppg_CashManagementRepository objCR = new Ppg_CashManagementRepository();
            objCR.ViewSDRefund(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (ViewSDRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(PPGAddMoneyToPDModelRefund m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Ppg_CashManagementRepository();
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
                Ppg_CashManagementRepository ObjRR = new Ppg_CashManagementRepository();
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

        #endregion

        #region Yard Invoice Edit
        [HttpGet]
        public ActionResult EditYardInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

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

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetYardInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PpgInvoiceYard objPostPaymentSheet = (PpgInvoiceYard)objCashManagement.DBResponse.Data;

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

                    /***************BOL PRINT******************/
                    var BOL = "";
                    objCharge.GetBOLForEmptyCont("Yard", objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                        BOL = objCharge.DBResponse.Data.ToString();
                    /************************************/
                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers, BOL = BOL }, JsonRequestBehavior.AllowGet);
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


        #region Delivery Invoice Edit

        [HttpGet]
        public ActionResult EditDeliveryInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

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
        public JsonResult GetDeliInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetDeliInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    //PpgInvoiceYard objPostPaymentSheet = (PpgInvoiceYard)objCashManagement.DBResponse.Data;
                    PPGInvoiceGodown objPostPaymentSheet = (PPGInvoiceGodown)objCashManagement.DBResponse.Data; //new PPGInvoiceGodown();

                    IList<PPGPaymentSheetBOE> containers = new List<PPGPaymentSheetBOE>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PPGPaymentSheetBOE
                        {
                            CFSCode = item.CFSCode,
                            //LineNo = item.LineNo,
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





        public JsonResult AddEditDeliPaymentSheet(PPGInvoiceGodown objForm)
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

        #region Loaded Container Payment Sheet Edit

        [HttpGet]
        public ActionResult EditLoadedContainerInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

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



        public JsonResult AddEditContPaymentSheet(PpgInvoiceYard objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
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
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, 0, "EXPLod","");

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


        #region Edit Container Movement

        [HttpGet]
        public ActionResult EditContainerMovementInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            objCashManagement.GetInvoiceForEdit("EXPMovement");
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



        public JsonResult EditContMovementPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditContainerMovementInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

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

        [HttpGet]
        public JsonResult GetContainerMovementInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetContainerMovementInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = (Areas.Export.Models.PPG_MovementInvoice)objCashManagement.DBResponse.Data;

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


                    /***************BOL PRINT******************/
                    var BOL = "";
                    /************************************/
                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers, BOL = BOL }, JsonRequestBehavior.AllowGet);
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



        #region Cancel Receipt
        [HttpGet]
        public ActionResult GetCancelReceipt()
        {
            Ppg_CashManagementRepository objRR = new Ppg_CashManagementRepository();
            objRR.ListOfReceiptForCancel();
            if (objRR.DBResponse.Data != null)
                ViewBag.ReceiptList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(objRR.DBResponse.Data));
            else ViewBag.ReceiptList = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ListOfCancelledReceipt()
        {
            Ppg_CashManagementRepository objRR = new Ppg_CashManagementRepository();
            List<CancelReceiptList> lstreceipt = new List<CancelReceiptList>();
            objRR.CancelReceiptList();
            if (objRR.DBResponse.Data != null)
                lstreceipt = (List<CancelReceiptList>)objRR.DBResponse.Data;
            return PartialView(lstreceipt);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult UpdateCancelReceipt(CancelReceipt objReceipt)
        {
            if (ModelState.IsValid)
            {
                Ppg_CashManagementRepository objRR = new Ppg_CashManagementRepository();
                objRR.UpdateCancelReceipt(objReceipt, Convert.ToInt32(((Login)(Session["LoginUser"])).Uid));
                if (objRR.DBResponse.Status == 1)
                    return Json(objRR.DBResponse);
                else return Json(new { Status = 0 });
            }
            else return Json(new { Status = 0 });
        }
        [HttpGet]
        public JsonResult GetReceiptDetails(int ReceiptId)
        {
            Ppg_CashManagementRepository objRR = new Ppg_CashManagementRepository();
            objRR.DetailsOfReceiptForCancel(ReceiptId);
            dynamic objReceipt = null;
            if (objRR.DBResponse.Data != null)
                objReceipt = objRR.DBResponse.Data;
            return Json(new { Data = objReceipt }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fumigation Invoice
        [HttpGet]
        public ActionResult CreateFumigationInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetChemical();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ChemicalLst = null;


            return PartialView();
        }
        [HttpPost]

        public JsonResult GetFumigation(string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Chemical> ChemicalDetails,string SEZ)
        {
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }
            Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
            objChargeMaster.GetFumigation(FumigationChargeType, InvoiceType, PartyId, size, XMLText,SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFumigationInvoice(ppg_FumigationInvoice FumigationModel)
        {

            ModelState.Remove("FumigationChargeType");
            ModelState.Remove("Container");
            if (ModelState.IsValid)
            {
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                    IList<Chemical> LstChemical = JsonConvert.DeserializeObject<IList<Chemical>>(FumigationModel.ChemicalXML);
                    string ChemicalXml = Utility.CreateXML(LstChemical);
                    Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                    objChargeMaster.AddEditFumigationInv(FumigationModel, ChemicalXml, BranchId, ((Login)(Session["LoginUser"])).Uid, "Fumigation");


                    FumigationModel.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                    objChargeMaster.DBResponse.Data = FumigationModel;
                    return Json(objChargeMaster.DBResponse);
                }
                catch (Exception ex)
                {
                    var Err = new { Status = -1, Message = "Error" };
                    return Json(Err);
                }
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        public JsonResult GetContainersForFumigation()
        {
            Ppg_CashManagementRepository repo = new Ppg_CashManagementRepository();
            repo.GetContainersForFumigation();
            return Json(repo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Rent Invoice

        [HttpGet]
        public ActionResult CreateRentInvoice()
        {
            Ppg_CashManagementRepository objCash = new Ppg_CashManagementRepository();


            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            Ppg_CashManagementRepository ObjCash = new Ppg_CashManagementRepository();
            ObjCash.PurposeListForChargeInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                lstPurpose = (List<SelectListItem>)ObjCash.DBResponse.Data;
                //lstPurpose.Add(new SelectListItem {Text = "Printing", Value="Printing" });
                //lstPurpose.Add(new SelectListItem { Text = "Banking", Value = "Banking" });
                //lstPurpose.Add(new SelectListItem { Text = "Photocopy", Value = "Photocopy" });
                //lstPurpose.Add(new SelectListItem { Text = "Cheque Return", Value = "Cheque Return" });
                //lstPurpose.Add(new SelectListItem { Text = "Others", Value = "Others" });
                ViewBag.ChargeList = JsonConvert.SerializeObject(lstPurpose);
            }
            objCash.GetPaymentParty();
            if (objCash.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objCash.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditRentInvoice(PPG_RentInvoice objForm, String Month, int Year)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string RentDetailsXML = "";
                string ChargeDetailsXML = "";

                if (invoiceData.lstPrePaymentCont != null)
                {
                    RentDetailsXML = Utility.CreateXML(invoiceData.lstPrePaymentCont);
                }
                if (invoiceData.lstPpgRentInvoiceCharge != null)
                {
                    ChargeDetailsXML = Utility.CreateXML(invoiceData.lstPpgRentInvoiceCharge);
                }



                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.AddEditRentInv(invoiceData, RentDetailsXML, ChargeDetailsXML, BranchId, ((Login)(Session["LoginUser"])).Uid, Month, Year, "Rent");

                // invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
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
        public JsonResult GetMonthYearRentData(string Month, int Year, int Flag, int PartyId)
        {

            Ppg_CashManagementRepository objPpgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetRentDet(Month, Year, Flag, PartyId);
            var Output = (PPG_RentInvoice)objPpgRepo.DBResponse.Data;




            return Json(Output, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Reservation
        public ActionResult ReservationInvoice(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetReservationParties();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ReservationParties = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ReservationParties = null;


            GodownRepository gdObj = new GodownRepository();
            gdObj.GetAllGodown();
            if (gdObj.DBResponse.Status > 0)
                ViewBag.GodownList = JsonConvert.SerializeObject(gdObj.DBResponse.Data);
            else
                ViewBag.GodownList = null;


            return PartialView();
        }
        public JsonResult GetReservationInvoices(string month, int year, int mode)
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetReservationInvoices(month, year, mode);
            return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddEditReservationInvoice(List<ReservationPartyDetails> objs, string month, string year, string type = "Tax")
        {
            try
            {

                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string dtlslXml = Utility.CreateXML(objs);
                Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();

                if (objs.Where(o => o.InvoiceId > 0).Count() > 0) //Count > 0 => Update
                {
                    objChe.AddEditInvoiceReservation(dtlslXml, 2, type, BranchId, ((Login)(Session["LoginUser"])).Uid, month, year);
                }
                else
                {
                    objChe.AddEditInvoiceReservation(dtlslXml, 1, type, BranchId, ((Login)(Session["LoginUser"])).Uid, month, year);
                }

                return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = ex.Message.ToString(), Data = "" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Cheque Deposit
        public ActionResult CreateChequeDeposit()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewBag.CurDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;
            return PartialView();
        }

        public ActionResult ListChequeDeposit()
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetChequeDepositsAll();
            List<ChequeDepositList> lst = new List<ChequeDepositList>();
            if (objChe.DBResponse.Data != null)
                lst = (List<ChequeDepositList>)objChe.DBResponse.Data;
            return PartialView(lst);
        }
        public ActionResult EditChequeDeposit(int Id)
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetChequeDeposit(Id);
            if (objChe.DBResponse.Data != null)
            {
                ViewBag.ChqDetails = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            }
            else
            {
                ViewBag.ChqDetails = null;
            }

            ViewBag.CurDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;

            return PartialView();
        }

        public ActionResult ViewChequeDeposit(int Id)
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetChequeDeposit(Id);
            if (objChe.DBResponse.Data != null)
            {
                ViewBag.ChqDetails = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            }
            else
            {
                ViewBag.ChqDetails = null;
            }

            return PartialView();
        }

        public JsonResult DeleteChequeDeposit(int Id)
        {
            try
            {
                string dtlslXml = Utility.CreateXML("");
                Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();

                //Mode= 1>>Add    2>>Update  3>>Delete
                objChe.AddEditChequeDeposit(dtlslXml, 3, ((Login)(Session["LoginUser"])).Uid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Id);

                return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = ex.Message.ToString(), Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult PrintChequeDeposit(int Id)
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetChequeDeposit(Id);

            var obj = (ChequeDeposit)objChe.DBResponse.Data;

            StringBuilder html = new StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td style='text-align: center;'>");
            html.Append("<h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + ZonalOffice + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" + ZOAddress + "</span>");
            html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cheque Deposit</h2>");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td>");
            html.Append("<table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td>");
            html.Append("<table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'>");
            html.Append("< label style = 'font-weight: bold;' > Deposit No:</ label > < span >" + obj.ChequeDepositNo + " </ span > ");
            html.Append("</td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Date : </label> <span>" + obj.EntryDate + "</span></td>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td>");
            html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' align='center' cellspacing='0' cellpadding='5'>");
            html.Append("<thead>");
            html.Append("<tr>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SL No</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Bank</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Ac/No</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Cheque / Draft / Po No</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Mode</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
            int i = 1;
            obj.ChequeDetails.ForEach(item =>
            {
                html.Append("<tr>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.ChequeDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.BankName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.AccountNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.ChequeNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.Mode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + item.Amount.ToString("0.00") + "</td>");
                html.Append("</tr>");

                i++;
            });



            html.Append("<tr>");

            html.Append("<td colspan='6' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.ChequeDetails.Sum(o => o.Amount).ToString("0.00") + "</td>");
            html.Append("</tr>");

            html.Append("</tbody>");
            html.Append("</table>");
            html.Append("</td>");
            html.Append("</tr>");

            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");



            var FileName = "Chequedeposit" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }

            return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + FileName });
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SaveChequeDeposit(ChequeDeposit obj)
        {
            try
            {
                obj.ChequeDetails.ForEach(item =>
                {
                    item.ChequeDate = Convert.ToDateTime(item.ChequeDate).ToString("yyyy-MM-dd HH:mm");
                });
                string dtlslXml = Utility.CreateXML(obj.ChequeDetails);
                Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();

                //Mode= 1>>Add    2>>Update
                objChe.AddEditChequeDeposit(dtlslXml, obj.ChequeDepositId == 0 ? 1 : 2, ((Login)(Session["LoginUser"])).Uid, obj.EntryDate, obj.ChequeDepositId);

                return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = ex.Message.ToString(), Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Cash Deposit To Bank

        public ActionResult CashDepositToBank()
        {
            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;

            CashDepositToBank objCashDepositToBank = new CashDepositToBank();
            objCashDepositToBank.DepositDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            objChe.GetCashDepositBalance(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            if (objChe.DBResponse.Status > 0)
                objCashDepositToBank.BalanceAmount = Convert.ToDecimal(objChe.DBResponse.Data);

            return PartialView(objCashDepositToBank);
        }

        [HttpGet]
        public JsonResult GetCashDepositBalance(string TransactionDate)
        {
            try
            {
                decimal BalanceAmount = 0;
                Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
                objChe.GetCashDepositBalance(TransactionDate);
                if (objChe.DBResponse.Status > 0)
                    BalanceAmount = Convert.ToDecimal(objChe.DBResponse.Data);
                return Json(BalanceAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCashDepositToBank(CashDepositToBank obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ppg_CashManagementRepository objER = new Ppg_CashManagementRepository();

                    objER.SaveCashDepositToBank(obj);
                    return Json(objER.DBResponse);
                }
                else
                {
                    var data = new { Status = -1 };
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = ex.Message.ToString(), Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Party wise SD Balance

        public ActionResult PartyWiseSDBalance()
        {
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyList();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfParty = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPartyWiseSDBalance(int PartyId, string BalanceDate)
        {
            try
            {
                decimal BalanceAmount = 0;
                Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
                objChe.GetPartyWiseSDBalance(PartyId, BalanceDate);
                if (objChe.DBResponse.Status > 0)
                    BalanceAmount = Convert.ToDecimal(objChe.DBResponse.Data);
                return Json(BalanceAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Cargo Shifting Invoice Edit
        public ActionResult EditCargoShifting(string type = "Tax")
        {
            //--------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //-------------------------------------------------------------------------------
            ViewData["InvType"] = type;
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            //--------------------------------------------------------------------------------
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            objExport.GetShippingLineForInvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.ShippingLine = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ShippingLine = null;
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            GodownRepository ObjGR = new GodownRepository();
            List<CwcExim.Models.Godown> lstGodown = new List<CwcExim.Models.Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodown = (List<CwcExim.Models.Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);
            //-------------------------------------------------------------------------------
            Ppg_CashManagementRepository objCash = new Ppg_CashManagementRepository();
            objCash.GetCargoShiftingNos();
            if (objCash.DBResponse.Data != null)
            {
                ViewBag.ListOfCS = JsonConvert.SerializeObject(objCash.DBResponse.Data);
            }
            else ViewBag.ListOfCS = null;
            return PartialView();
        }
        public JsonResult GetCargoShiftingDetailsInv(int CargoShiftingId)
        {
            Ppg_CashManagementRepository objCR = new Ppg_CashManagementRepository();
            objCR.GetCargoShiftingDetailsInv(CargoShiftingId);
            if (objCR.DBResponse.Data != null)
            {
                return Json(objCR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Data = "", Status = 0 });
        }
        #endregion

        #region Container Debit Invoice

        [HttpGet]
        public ActionResult CreateDebitInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            //ImportRepository objImport = new ImportRepository();

            //objImport.GetPaymentParty();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            //Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            //objChe.GetChemical();
            //if (objChe.DBResponse.Status > 0)
            //    ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            //else
            //    ViewBag.ChemicalLst = null;


            return PartialView();
        }


        [HttpGet]
        public JsonResult ListOfContainer()
        {
            Ppg_CashManagementRepository objcash = new Ppg_CashManagementRepository();
            objcash.GetContainerList();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_Container>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { Id = item.id, ContainerNo = item.ContainerNo, size = item.size, CFSCode = item.CFSCode, InDate = item.In_Date });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }



        //[HttpGet]
        //public JsonResult ListOfParty()
        //{
        //    Ppg_CashManagementRepository objcash = new Ppg_CashManagementRepository();
        //    objcash.ListOfGREParty();
        //    //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
        //    List<dynamic> objImp2 = new List<dynamic>();
        //    if (objcash.DBResponse.Data != null)
        //        ((List<PartyDet>)objcash.DBResponse.Data).ToList().ForEach(item => {
        //            objImp2.Add(new { PartyId = item.PartyId, PartyName = item.PartyName,GstNo=item.GstNo,StateCode=item.StateCode });
        //        });

        //    return Json(objImp2, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult LoadParty(string PartyCode, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyList("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayee(string PartyCode, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPayeeList("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPayeeCode(string PartyCode)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPayeeList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAmount(int ChargeId, String ChargeName, String Size, string FromDate, string ToDate)
        {
            Ppg_CashManagementRepository objcash = new Ppg_CashManagementRepository();
            objcash.GetAmountForCharges(ChargeId, ChargeName, Size, FromDate, ToDate);
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            Decimal Amount = 0;
            if (objcash.DBResponse.Data != null)
            {
                Amount = (Decimal)objcash.DBResponse.Data;
            }
            return Json(Amount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCharge()
        {
            Ppg_CashManagementRepository objcash = new Ppg_CashManagementRepository();
            objcash.ListOfChargesName();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<Charge>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { ChargeId = item.ChargeId, ChargeName = item.ChargeName });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetDebitInvoice(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges,string SEZ)
        {

            Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
            objChargeMaster.GetDebitInvoice(InvoiceType, PartyId, TotalChrgAmount, Charges, SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDebitInvoice(PPG_DebitInvoice DebitModel)
        {

            ModelState.Remove("ChargeId");
            ModelState.Remove("ChargeName");
            if (ModelState.IsValid)
            {
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                    IList<Charge> LstChemical = JsonConvert.DeserializeObject<IList<Charge>>(DebitModel.ChargeXML);
                    string ChargeXML = Utility.CreateXML(LstChemical);
                    Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                    objChargeMaster.AddEditDebitInvoice(DebitModel, ChargeXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "INVDEBT");


                    DebitModel.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                    objChargeMaster.DBResponse.Data = DebitModel;
                    return Json(objChargeMaster.DBResponse);
                }
                catch (Exception ex)
                {
                    var Err = new { Status = -1, Message = "Error" };
                    return Json(Err);
                }
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        #endregion



        #region ---- Payment Adjust Through SD ----

        [HttpGet]
        public ActionResult CashReceiptSD(int PartyId = 0, string PartyName = "", string Type = "INVOICE")
        {
            PPG_CashReceiptModel ObjCashReceipt = new PPG_CashReceiptModel();

            var objRepo = new Ppg_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetPartyListSD();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((PPG_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;

                for (var i = 0; i < 7; i++)
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
                    ViewBag.Party = ((Kol_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetailsSD(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (PPG_CashReceiptModel)objRepo.DBResponse.Data;
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

                for (var i = 0; i < 7; i++)
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
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                //objRepo.GetCashRcptPrint(PartyId);
                //if (objRepo.DBResponse.Data != null)
                //{
                //    ViewBag.CashPrint = JsonConvert.SerializeObject(((PostPaymentSheet)objRepo.DBResponse.Data));
                //}
                //else
                //{
                //    ViewBag.CashPrint = null;
                //}

                ViewBag.CashReceiptInvoiveMappingList = JsonConvert.SerializeObject(ObjCashReceipt.CashReceiptInvoiveMappingList);
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;
                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashReceiptSD(PPG_CashReceiptModel ObjCashReceipt)
        {
            List<CashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<CashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<CashReceiptInvoiveMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.InvoiceDate = dt.ToString("yyyy-MM-dd");
                ObjCashReceipt.InvoiceValue = ObjCashReceipt.InvoiceValue + item.InvoiceAmt;
            }

            ObjCashReceipt.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
            var xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
            // ObjCashReceipt.BranchId = Convert.ToInt32(Session["BranchId"]);
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.AddCashReceiptSD(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrintSD(int CashReceiptId)
        {
            var objRepo = new Ppg_CashManagementRepository();
            var model = new PostPaymentSheet();
            objRepo.GetCashRcptPrintSD(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePrintHtmlSD(FormCollection fc)
        {
            int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
            string htmlPrint = fc["htmlPrint"].ToString();
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.UpdatePrintHtmlSD(CashReceiptId, htmlPrint);
            return Json(objRepo.DBResponse.Status, JsonRequestBehavior.AllowGet);
        }


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
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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


        #endregion


        #region-- Cheque Return --

        [HttpGet]
        public ActionResult ChequeReturn()
        {
            //AccessRightsRepository ACCR = new AccessRightsRepository();
            //  ChequeReturn obj = new ChequeReturn();
            //   ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            //   var objRepo = new Ppg_CashManagementRepository();

            // objRepo.GetPartyDetail();
            //  ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);



            //objChe.GetChequeDetail(PartyId);
            //ViewBag.PaymentParty = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            // objRe.GetChequeDetail();
            // 
            // objRepo.GetPartyDetail();
            //  ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            return PartialView();

        }

        [HttpGet]
        public JsonResult LoadChequeParty(string PartyCode, int Page)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyDetail("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByChequePartyCode(string PartyCode)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyDetail(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult GetChequeDetail(int PartyId)
        {



            Ppg_CashManagementRepository objChe = new Ppg_CashManagementRepository();
            List<dynamic> objImp2 = new List<dynamic>();
            objChe.GetChequeNo(PartyId);
            if (objChe.DBResponse.Data != null)
            {
                ((List<ChequeDetail>)objChe.DBResponse.Data).ToList().ForEach(item =>
               {
                   objImp2.Add(new { Id = item.Id, Cheque = item.Cheque });
               });
            }




            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchChequeNo(int partyid, string ChequeNo)
        {
            Ppg_CashManagementRepository objImport = new Ppg_CashManagementRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            objImport.GetChequeDetail(partyid, ChequeNo);

            if (objImport.DBResponse.Data != null)
            {
                ((List<ChequeDetail>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { Id = item.Id, SdNo = item.Cheque });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
            //    Ppg_CashManagementRepository objImport = new Ppg_CashManagementRepository();
            //  objImport.GetChequeDetail(partyid);
            //  return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetChequeDetails(string ChequeNo)
        {
            try
            {

                Ppg_CashManagementRepository objCh = new Ppg_CashManagementRepository();
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
        public JsonResult AddChequeReturn(ChequeReturn m)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var objRepo = new Ppg_CashManagementRepository();
                    objRepo.AddChequeReturn(m.PartyId, m.ChequeReturnDate, m.SdNo, Convert.ToString(m.Balance), m.ChequeNo, m.DraweeBank,m.Narration, m.ChequeDate, m.Amount, m.AdjustedBalance);
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
        public ActionResult ListOfChequeReturn()
        {
            Ppg_CashManagementRepository objER = new Ppg_CashManagementRepository();
            List<ChequeReturn> lstPartyWiseTDSDeposit = new List<ChequeReturn>();
            objER.GetAllChequeReturn();
            if (objER.DBResponse.Data != null)
                lstPartyWiseTDSDeposit = (List<ChequeReturn>)objER.DBResponse.Data;
            return PartialView(lstPartyWiseTDSDeposit);
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


        #region Inter Balance Transfer From SD Party

        [HttpGet]
        public ActionResult InterBalanceTransferSD()
        {
            PPGInterBalanceTransfer InterBalaObj = new PPGInterBalanceTransfer();
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetPartyDetailsInterBalance();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            InterBalaObj.TransferDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.ServerDate = Utility.GetServerDate();
            return PartialView(InterBalaObj);
        }

        [HttpGet]
        public ActionResult InterBalanceTransferList()
        {
            List<PPGInterBalanceTransfer> ObjSd = new List<PPGInterBalanceTransfer>();
            Ppg_CashManagementRepository ObjCR = new Ppg_CashManagementRepository();
            ObjCR.GetSDBalanceTransferList();
            if (ObjCR.DBResponse.Data != null)
                ObjSd = (List<PPGInterBalanceTransfer>)ObjCR.DBResponse.Data;
            return PartialView(ObjSd);
        }

        [HttpGet]
        public ActionResult ViewInterBalanceTransfer(int ID)
        {
            PPGInterBalanceTransfer ObjSD = new PPGInterBalanceTransfer();
            Ppg_CashManagementRepository objCR = new Ppg_CashManagementRepository();
            objCR.ViewInterBalanceTransferSD(ID);
            if (objCR.DBResponse.Data != null)
                ObjSD = (PPGInterBalanceTransfer)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveInterBalanceTransfer(PPGInterBalanceTransfer m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Ppg_CashManagementRepository();
                    objRepo.InterBalanceTransferSDParty(m, ((Login)(Session["LoginUser"])).Uid);
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
        public JsonResult GenerateInterBalanceTransfer(FormCollection fc)
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
                Ppg_CashManagementRepository ObjRR = new Ppg_CashManagementRepository();
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

        #region Edit cash Receipt
        //[HttpGet]
        public ActionResult EditCashReceiptForPaymentMode()//int InvoiceId = 0, string InvoiceNo = ""
        {
            PPG_CashReceiptModel ObjCashReceipt = new PPG_CashReceiptModel();
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetReceiptListforEdit();
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
            objRepo.GetReceiptPDAListForEdit();
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
            for (var i = 0; i < 7; i++)
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
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDITNOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;




            return PartialView();
         
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult GetListOfPaymentsMode(int CashReceiptId, int InvoiceId = 0)
        {
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.GetReceiptDtlsListForEdit(CashReceiptId);
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
                new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                new SelectListItem { Text = "CREDITNOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.PaymentMode = PaymentMode;
            //ViewBag.PaymentMode=new SelectList((IEnumerable)PaymentMode,"Key","Value",)

            if (lStCashReceiptEditDtls.Count < 6)
            {
                for (var i = lStCashReceiptEditDtls.Count; i < 7; i++)
                {
                    lStCashReceiptEditDtls.Add(new CashReceiptEditDtls
                    {
                        // Amount = 0
                    });
                }
            }
            lStCashReceiptEditDtls.ForEach(m => Convert.ToDecimal(m.Amount));

            ViewBag.sumOfAmount = ((lStCashReceiptEditDtls.Sum(x => x.Amount) > 0) ? lStCashReceiptEditDtls.Sum(x => x.Amount).ToString() : "");


            objRepo.GetEditCashRcptPrintForEdit(InvoiceId);
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

        public ActionResult SaveEditedCashReceiptPaymentMode(EditReceiptPayment objEditReceiptPayment)
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
            var objRepo = new Ppg_CashManagementRepository();
            objRepo.SaveEditedCashRcptForEdit(objEditReceiptPayment, ((Login)(Session["LoginUser"])).Uid);

            return Json(objRepo.DBResponse);

        }
        #endregion



        #region Fumigation Payment Sheet Edit

        [HttpGet]
        public ActionResult EditFumigationInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ImportRepository objImport = new ImportRepository();


            //   objImport.GetInvoiceForEdit("EXPLod");
            // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //    ViewBag.InvoiceList = null;
            //   Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }



       



        [HttpGet]
        public JsonResult GetFumigationInvoiceDetails(int InvoiceId)
        {
            try
            {
               // ChargeMasterRepository objCharge = new ChargeMasterRepository();
              //  ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetFumigationInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

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








        [HttpPost]
        public JsonResult GetFumigationPaymentSheetForEdit(string InvoiceDate,string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Chemical> ChemicalDetails ,int InvoiceId = 0)
        {
            
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }

            Ppg_CashManagementRepository objChrgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetFumiPaymentSheetInvoice(InvoiceDate,FumigationChargeType, InvoiceType, PartyId, size, XMLText, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (CwcExim.Areas.Export.Models.PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "Fumigation";

           

            //    Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
             //   Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
              //  Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
              //  Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
              //  Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
              //  Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
                Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
                Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
                Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.HTTotal = 0;
                Output.CWCTDS = 0;
                Output.HTTDS = 0;
                Output.CWCTDSPer = 0;
                Output.HTTDSPer = 0;
                Output.TDS = 0;
                Output.TDSCol = 0;
                Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.RoundUp = 0;
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);

            



            return Json(Output, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditFumiPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditFumiInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "Fumigation");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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




        #region Container Debit Payment Sheet Edit

        [HttpGet]
        public ActionResult EditContDebitInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("EXPLod");
            // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //    ViewBag.InvoiceList = null;
            //   Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objCashManagement.GetPaymentPartyforContDebit();
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }







        [HttpGet]
        public JsonResult GetContDebitInvoiceDetails(int InvoiceId)
        {
            try
            {
                // ChargeMasterRepository objCharge = new ChargeMasterRepository();
                //  ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetContDebitInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

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








        [HttpPost]
        public JsonResult GetContDebitPaymentSheetForEdit(string InvoiceDate, string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Charge> ChemicalDetails, int InvoiceId = 0)
        {

            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }

            Ppg_CashManagementRepository objChrgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GeContDebitPaymentSheetInvoice(InvoiceDate, FumigationChargeType, InvoiceType, PartyId, size, XMLText, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (CwcExim.Areas.Export.Models.PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "INVDEBT";



            //    Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
            //   Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
            //  Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
            //  Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
            //  Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
            //  Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
            //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);





            return Json(Output, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditContDebitPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditContDebitInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "INVDEBT");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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


        #region Misc. Invoice  Payment Sheet Edit

        [HttpGet]
        public ActionResult EditMiscInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("EXPLod");
            // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //    ViewBag.InvoiceList = null;
            //   Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objCashManagement.GetPaymentPartyforMiscInv();
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }







        [HttpGet]
        public JsonResult GetMiscInvoiceDetails(int InvoiceId)
        {
            try
            {
                // ChargeMasterRepository objCharge = new ChargeMasterRepository();
                //  ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetMIscInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

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








        [HttpPost]
        public JsonResult GetMIscInvPaymentSheetForEdit(string InvoiceDate, string FumigationChargeType, string InvoiceType, int PartyId, string size, String Purpose,decimal Amount, int InvoiceId = 0)
        {

            string XMLText = "";
            

            Ppg_CashManagementRepository objChrgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GeMIscInvPaymentSheetInvoice(InvoiceDate, FumigationChargeType, InvoiceType, PartyId, size, XMLText,  Purpose, Amount, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (CwcExim.Areas.Export.Models.PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "MiscInv";



            //    Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
            //   Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
            //  Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
            //  Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
            //  Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
            //  Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
            //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);





            return Json(Output, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditMiscInvPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditMiscInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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



        #region Rent Invoice  Payment Sheet Edit

        [HttpGet]
        public ActionResult EditRentnvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("EXPLod");
            // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //    ViewBag.InvoiceList = null;
            //   Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objCashManagement.GetPaymentPartyforRentInv();
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }







        [HttpGet]
        public JsonResult GetRentInvoiceDetails(int InvoiceId)
        {
            try
            {
                // ChargeMasterRepository objCharge = new ChargeMasterRepository();
                //  ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetRentInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

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








        [HttpPost]
        public JsonResult GetRentInvPaymentSheetForEdit(string InvoiceDate, string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Charge> ChemicalDetails, int InvoiceId = 0)
        {

            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }

            Ppg_CashManagementRepository objChrgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetRentInvPaymentSheetInvoice(InvoiceDate, FumigationChargeType, InvoiceType, PartyId, size, XMLText, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (CwcExim.Areas.Export.Models.PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "Rent";



            //    Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
            //   Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
            //  Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
            //  Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
            //  Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
            //  Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
            //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);





            return Json(Output, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditRentInvPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditRentInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "Rent");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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



        #region Reservation Invoice  Payment Sheet Edit

        [HttpGet]
        public ActionResult EditReservationInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("EXPLod");
            // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //    ViewBag.InvoiceList = null;
            //   Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objCashManagement.GetPaymentPartyforResInv();
            if (objCashManagement.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objCashManagement.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }







        [HttpGet]
        public JsonResult GetResInvoiceDetails(int InvoiceId)
        {
            try
            {
                // ChargeMasterRepository objCharge = new ChargeMasterRepository();
                //  ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();
                objCashManagement.GetResInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

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








        [HttpPost]
        public JsonResult GetResInvPaymentSheetForEdit(string InvoiceDate, string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Charge> ChemicalDetails, int InvoiceId = 0)
        {

            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }

            Ppg_CashManagementRepository objChrgRepo = new Ppg_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetResInvPaymentSheetInvoice(InvoiceDate, FumigationChargeType, InvoiceType, PartyId, size, XMLText, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (CwcExim.Areas.Export.Models.PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "RESERV";



            //    Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
            //   Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
            //  Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
            //  Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
            //  Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
            //  Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
            //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);





            return Json(Output, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditResInvPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Ppg_CashManagementRepository objChargeMaster = new Ppg_CashManagementRepository();
                objChargeMaster.EditResInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "RESERV");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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

        #region Cancel Invoice


        [HttpGet]
        public ActionResult CancelInvoice()
        {
            CancelInvoice cin = new CancelInvoice();
            var InvoiceNo = "";
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();
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
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Ppg_CashManagementRepository objCR = new Ppg_CashManagementRepository();
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
            Ppg_CashManagementRepository objCR = new Ppg_CashManagementRepository();
            objCR.LstOfCancleInvoice(InvoiceNo, Page);
            List<CancelInvoice> lstInvoice = new List<Models.CancelInvoice>();
            if (objCR.DBResponse.Data != null)
            {
                lstInvoice = (List<CancelInvoice>)objCR.DBResponse.Data;
            }
            return Json(lstInvoice, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult SearchCancleInvoice(string InvoiceNo)
        {
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Ppg_CashManagementRepository objCancelInv = new Ppg_CashManagementRepository();
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

            Ppg_CashManagementRepository ObjIR = new Ppg_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

        }


        #endregion

        #region Credit Note IRN Generation

        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Ppg_CashManagementRepository objPpgRepo = new Ppg_CashManagementRepository();



            if (SupplyType == "B2C" && CRDR== "C")
            {
                Eobj = new Einvoice();
                IrnModel m1 = new IrnModel();

                Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                var Output = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;

                m1.DocumentNo = Output.InvoiceDate;
                m1.DocumentDate = Output.InvoiceDate;
                m1.SupplierGstNo = Output.SupplierGstNo;
                m1.DocumentType = Type;
                String IRN = Eobj.GenerateB2cIrn(m1);
                Output.Irn = IRN;
                Output.IrnDt = Output.InvoiceDate;
                Output.iss = "NIC";
                q1.Data = Output;
                B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);

                //   return Json(objPpgRepo.DBResponse.Status);
                //   IrnResponse ERes = await Eobj.GenerateB2cIrn();
            }
            else if(SupplyType == "B2C" && CRDR == "D")
            {
                Eobj = new Einvoice();
                IrnModel m1 = new IrnModel();
                Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                var Output = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;
                if (Output.pa == "" || Output.mtid == "")
                {
                    

                    m1.DocumentNo = Output.InvoiceDate;
                    m1.DocumentDate = Output.InvoiceDate;
                    m1.SupplierGstNo = Output.SupplierGstNo;
                    m1.DocumentType = Type;
                    String IRN = Eobj.GenerateB2cIrn(m1);
                    Output.Irn = IRN;
                    Output.IrnDt = Output.InvoiceDate;
                    Output.iss = "NIC";
                    q1.Data = Output;
                    B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                    objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);
                }
                else
                {
                    var tn = "GST QR";
                    UpiQRCodeInfo idata = new UpiQRCodeInfo();
                    idata.ver = Output.ver.ToString();
                    idata.mode = Output.mode;
                    idata.mode = Output.mode;
                    idata.tr = Output.tr;
                    idata.tid = Output.tid;
                    idata.tn = tn;
                    idata.pa = Output.pa;
                    idata.pn = Output.pn;
                    idata.mc = Output.mc;
                    idata.am = Output.InvoiceValue;
                    idata.mam = Output.InvoiceValue;
                    idata.mid = Output.mid;
                    idata.msid = Output.msid;
                    idata.orgId = Output.orgId;
                    idata.mtid = Output.mtid;
                    idata.CESS = Output.CESS;
                    idata.CGST = Output.CGST;
                    idata.SGST = Output.SGST;
                    idata.IGST = Output.IGST;
                    idata.GSTIncentive = Output.GSTIncentive;
                    idata.GSTPCT = Output.GSTPCT;
                    idata.qrMedium = Output.qrMedium;
                    idata.invoiceNo = Output.InvoiceNo;
                    idata.InvoiceDate = Output.InvoiceDate;
                    idata.InvoiceName = Output.InvoiceName;
                    idata.QRexpire = Output.InvoiceDate;
                    idata.pinCode = Output.pinCode;
                    idata.tier = Output.tier;


                    idata.gstIn = Output.gstIn;
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = Output.InvoiceDate;
                    irnModelObj.DocumentNo = Output.InvoiceNo;
                    irnModelObj.DocumentType = Type;
                    irnModelObj.SupplierGstNo = Output.gstIn;
                    string irn = Eobj.GenerateB2cIrn(irnModelObj);
                    var dt = DateTime.Now;
                    Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                    Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                    objQR.Irn = irn;
                    objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                    objQR.iss = "NIC";

                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = irn;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;
                    objPpgRepo.AddEditIRNB2CCreditDebitNote(irn, objresponse, CrNoteNo, CRDR);
                   
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

                        Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                        var Output1 = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;

                        m1.DocumentNo = Output1.InvoiceDate;
                        m1.DocumentDate = Output1.InvoiceDate;
                        m1.SupplierGstNo = Output1.SupplierGstNo;
                        m1.DocumentType = Type;
                        String IRN = Eobj.GenerateB2cIrn(m1);
                        Output1.Irn = IRN;
                        Output1.IrnDt = Output1.InvoiceDate;
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
                        Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                        var Output2 = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;
                        if (Output2.pa == "" || Output2.mtid == "")
                        {


                            m1.DocumentNo = Output2.InvoiceDate;
                            m1.DocumentDate = Output2.InvoiceDate;
                            m1.SupplierGstNo = Output2.SupplierGstNo;
                            m1.DocumentType = Type;
                            String IRN = Eobj.GenerateB2cIrn(m1);
                            Output2.Irn = IRN;
                            Output2.IrnDt = Output2.InvoiceDate;
                            Output2.iss = "NIC";
                            q1.Data = Output2;
                            B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(q1);
                            objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);
                        }
                        else
                        {
                            var tn = "GST QR";
                            UpiQRCodeInfo idata = new UpiQRCodeInfo();
                            idata.ver = Output2.ver.ToString();
                            idata.mode = Output2.mode;
                            idata.mode = Output2.mode;
                            idata.tr = Output2.tr;
                            idata.tid = Output2.tid;
                            idata.tn = tn;
                            idata.pa = Output2.pa;
                            idata.pn = Output2.pn;
                            idata.mc = Output2.mc;
                            idata.am = Output2.InvoiceValue;
                            idata.mam = Output2.InvoiceValue;
                            idata.mid = Output2.mid;
                            idata.msid = Output2.msid;
                            idata.orgId = Output2.orgId;
                            idata.mtid = Output2.mtid;
                            idata.CESS = Output2.CESS;
                            idata.CGST = Output2.CGST;
                            idata.SGST = Output2.SGST;
                            idata.IGST = Output2.IGST;
                            idata.GSTIncentive = Output2.GSTIncentive;
                            idata.GSTPCT = Output2.GSTPCT;
                            idata.qrMedium = Output2.qrMedium;
                            idata.invoiceNo = Output2.InvoiceNo;
                            idata.InvoiceDate = Output2.InvoiceDate;
                            idata.InvoiceName = Output2.InvoiceName;
                            idata.QRexpire = Output2.InvoiceDate;
                            idata.pinCode = Output2.pinCode;
                            idata.tier = Output2.tier;


                            idata.gstIn = Output2.gstIn;
                            IrnModel irnModelObj = new IrnModel();
                            irnModelObj.DocumentDate = Output2.InvoiceDate;
                            irnModelObj.DocumentNo = Output2.InvoiceNo;
                            irnModelObj.DocumentType = Type;
                            irnModelObj.SupplierGstNo = Output2.gstIn;
                            string irn = Eobj.GenerateB2cIrn(irnModelObj);
                            var dt = DateTime.Now;
                            Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                            Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                            objQR.Irn = irn;
                            objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                            objQR.iss = "NIC";

                            B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                            objresponse = Eobj.GenerateB2cQRCode(idata);
                            IrnResponse objERes = new IrnResponse();
                            objERes.irn = irn;
                            objERes.SignedQRCode = objresponse.QrCodeBase64;
                            objERes.SignedInvoice = objresponse.QrCodeJson;
                            objERes.SignedQRCode = objresponse.QrCodeJson;
                            objPpgRepo.AddEditIRNB2CCreditDebitNote(irn, objresponse, CrNoteNo, CRDR);

                        }
                    }
                }

            }
            // var Images = LoadImage(ERes.QRCodeImageBase64);

            return Json(objPpgRepo.DBResponse);
        }


        #endregion

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


        #region UPI QR Payment Acknowledgement
        public ActionResult UPIQRPaymentAcknowledgement()
        {
            return PartialView();
        }
        #endregion

      

        #region BankDeposit
        public ActionResult BankDeposit()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddEditBankDeposit(PpgBankDeposit obj)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            string varXml = Utility.CreateXML(obj.ExpensesDetails);
            objRepo.AddEditBankDeposit(obj, varXml, ((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBankDeposit(int Id)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.DeleteBankDeposit(Id, ((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDepositList()
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetBankDepositList();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNEFTForBankDeposit(string dt)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetNEFTForBankDeposit(Convert.ToDateTime(dt));
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpenseHeadWithBalance()
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.ExpenseBankDeposit();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReceiptVoucherBalance(string HeadId, string DSNo)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.ReceiptVoucherBalance(HeadId, DSNo);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDepositListById(int id)
        {
            Ppg_CashManagementRepository objRepo = new Ppg_CashManagementRepository();
            objRepo.GetBankDepositList(id);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
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
            DSR_CashManagementRepository objIR = null;
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
                objIR = new DSR_CashManagementRepository();
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
                    objIR = new DSR_CashManagementRepository();
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
            WFLD_CashManagementRepository objIR = null;
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
                objIR = new WFLD_CashManagementRepository();
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
                        WFLD_CashManagementRepository objCash = new WFLD_CashManagementRepository();
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

        #region Direct Online Payment
        public ActionResult DirectOnlinePayment()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DirectPaymentVoucher(Ppg_DirectOnlinePayment objDOP)
        {
            Ppg_CashManagementRepository ObjIR = new Ppg_CashManagementRepository();
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
            List<Ppg_DirectOnlinePayment> lstDOP = new List<Ppg_DirectOnlinePayment>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();

            obj.GetOnlinePayAckList(((Login)(Session["LoginUser"])).Uid, OrderId);
            lstDOP = (List<Ppg_DirectOnlinePayment>)obj.DBResponse.Data;
            return PartialView(lstDOP);
        }

        [HttpPost]
        public JsonResult ConfirmPayment(Ppg_DirectOnlinePayment vm)
        {
            vm.OrderId = Convert.ToInt64(Session["OrderId"].ToString());

            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
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
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.OnlinePaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.GetOnlinePaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptList(int Pages)
        {
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
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
            Ppg_CashManagementRepository objcancle = new Ppg_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            objcancle.ListOfPendingInvoice(Uid);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnlinePaymentAgainstInvoice(Ppg_OnlinePaymentAgainstInvoice objDOP)
        {
            Ppg_CashManagementRepository ObjIR = new Ppg_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<Ppg_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
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
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            List<Ppg_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Ppg_OnlinePaymentAgainstInvoice>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(Uid, SearchValue, 0);
            lstOPReceipt = (List<Ppg_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
            IList<Ppg_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Ppg_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice(Uid,"", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<Ppg_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
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
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
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


        #region Online Payment Receipt

        public ActionResult OnlineOAPaymentReceipt()
        {
            return PartialView();
        }

        public ActionResult OnlineOAPaymentReceiptDetails(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.OnlineOAPaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlineOAPaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.GetOnlineOAPaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlineOAPaymentReceiptList(int Pages)
        {
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlineOAPaymentReceiptList("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
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
            if(String.IsNullOrEmpty(PeriodFrom))
            {
                PeriodFrom = null;
            }
            if(String.IsNullOrEmpty(PeriodTo))
            {
                PeriodTo = null;
            }
            List<Ppg_AcknowledgementViewList> lstOPReceipt = new List<Ppg_AcknowledgementViewList>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.AcknowledgementViewList(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<Ppg_AcknowledgementViewList>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
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
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Ppg_CashManagementRepository obj = new Ppg_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
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
            Ppg_CashManagementRepository objIR = new Ppg_CashManagementRepository();
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
                        Ppg_CashManagementRepository objCash = new Ppg_CashManagementRepository();
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
                        if(ccAvnResponse.error_code=="")
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
    }
}
