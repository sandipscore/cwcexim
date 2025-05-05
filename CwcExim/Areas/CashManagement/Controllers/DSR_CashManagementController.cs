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

using System.Text;
using System.Globalization;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Bond.Models;
using CwcExim.DAL;
using System.Threading.Tasks;
using EinvoiceLibrary;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Data;
using CCA.Util;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Configuration;

namespace CwcExim.Areas.CashManagement.Controllers
{
 
    public class DSR_CashManagementController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region-- ADD MONEY TO PD --


        [HttpGet] 
        public ActionResult AddMoneyToPD()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            var model = new DSRAddMoneyToPDModel();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new CashReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
                  new SelectListItem { Text = "BANK GUARANTEE", Value = "BANKGUARANTEE"},//New Payment Mode introduce on 28/05/2019

            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView(model);
        }

        public ActionResult GetAddMoneyToPDList(int Page=0)
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetAddMoneyToPDList(Page);
            ViewBag.State = 0;
            IEnumerable<DSRAddMoneyToPDModel> lstAddMoneyToPD = (IEnumerable<DSRAddMoneyToPDModel>)objRepo.DBResponse.Data;
            ViewBag.State = objRepo.DBResponse.Message;
            if (lstAddMoneyToPD != null)
            {
                return PartialView(lstAddMoneyToPD);
            }
            else
            {
                return PartialView(new List<DSRAddMoneyToPDModel>());
            }
        }


        public JsonResult GetAddMoneyToPDListLoadMore(int Page = 0)
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetAddMoneyToPDList(Page);
            ViewBag.State = 0;
            IEnumerable<DSRAddMoneyToPDModel> lstAddMoneyToPD = (IEnumerable<DSRAddMoneyToPDModel>)objRepo.DBResponse.Data;
            ViewBag.State = objRepo.DBResponse.Message;
            if (lstAddMoneyToPD != null)
            {
                return Json(new { Data = lstAddMoneyToPD ,State = objRepo.DBResponse.Message },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Data = lstAddMoneyToPD, State = objRepo.DBResponse.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAddMoneyToPDListBySearch(string ReceiptNo)
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetAddMoneyToPDList(ReceiptNo);
          
            IEnumerable <DSRAddMoneyToPDModel> lstAddMoneyToPD = (IEnumerable<DSRAddMoneyToPDModel>)objRepo.DBResponse.Data;
            if (lstAddMoneyToPD != null)
            {
                return PartialView("GetAddMoneyToPDList", lstAddMoneyToPD);
            }
            else
            {
                return PartialView("GetAddMoneyToPDList", new List<DSRAddMoneyToPDModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(DSRAddMoneyToPDModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new DSR_CashManagementRepository();
                    objRepo.AddMoneyToPD(m.PartyId, Convert.ToDateTime(m.TransDate), xml,Convert.ToDecimal(m.TDSDeduction), Convert.ToDecimal(m.DepositAmount));
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

        // GET: CashManagement/DSR_CashManagement
        public ActionResult Index()
        {
            return View();
        }

        public DSR_CashManagementController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */


            DSRCompanyDetailsForReport objCompanyDetails = new DSRCompanyDetailsForReport();
            DSR_ReportRepository ObjRR = new DSR_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (DSRCompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;

        }


        #region-- PAYMENT VOUCHER --

        [HttpGet]
        public ActionResult PaymentVoucher()
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetPaymentVoucherCreateInfo();
            ViewData["COMGST"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).UserGST;
            ViewData["COMGstStateCode"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).GstStateCode;
            ViewData["COMStateName"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).StateName;
            ViewData["COMAddress"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).CompanyAddress;
            ViewData["COMCityName"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).CityName; 
            ViewData["COMPanNo"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).PanNo; 

            ViewBag.Expenses = JsonConvert.SerializeObject(((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).Expenses);
            ViewBag.ExpHSN = JsonConvert.SerializeObject(((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).ExpHSN);
            ViewBag.HSN = JsonConvert.SerializeObject(((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).HSN);
            ViewBag.PartyDetails = JsonConvert.SerializeObject(((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).Party);
            ViewBag.Parties = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).Party;
            ViewData["InvoiceNo"] = ((DSR_PaymentVoucherCreateInfoModel)objRepo.DBResponse.Data).VoucherId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PaymentVoucher(DSR_NewPaymentValucherModel m)
        {
            if (ModelState.IsValid)
            {
                var objRepo = new DSR_CashManagementRepository();
                objRepo.AddNewPaymentVoucher(m);
                return Json(new { Status = true, Message = "Payment Saved Successfully", Data = "CWC/PV/" + objRepo.DBResponse.Data.ToString().PadLeft(7, '0') + "/" + DateTime.Today.Year.ToString(), Id = objRepo.DBResponse.Data.ToString() }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF(FormCollection fc)
        {
            try
            {
                var pages = new string[1];
                pages[0] = fc["page"].ToString();
                pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
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
        public ActionResult GetPaymentVoucherList(string SearchValue="",int Page=0)
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetPaymentVoucherList(SearchValue, Page);
            IEnumerable<DSR_NewPaymentValucherModel> lstPaymentVou = (IEnumerable<DSR_NewPaymentValucherModel>)objRepo.DBResponse.Data;
            if (lstPaymentVou != null)
            {
                return PartialView(lstPaymentVou);
            }
            else
            {
                return PartialView(new List<DSR_NewPaymentValucherModel>());
            }
        }

        public ActionResult GetAmountForPaymentVoucher(string Purpose)
        {
            if (Purpose != "")
            {
                var objRepo = new DSR_CashManagementRepository();
                objRepo.GetAmountForPaymentVoucher(Purpose);
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetLoadMorePaymentVoucherList(string SearchValue = "", int Page = 0)
        {
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            List<DSR_NewPaymentValucherModel> lstPaymentVoucher = new List<DSR_NewPaymentValucherModel>();
            ObjCR.GetPaymentVoucherList(SearchValue, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstPaymentVoucher = (List<DSR_NewPaymentValucherModel>)ObjCR.DBResponse.Data;
            }
            return Json(lstPaymentVoucher, JsonRequestBehavior.AllowGet);
        }
        #endregion





        #region ---- Payment Receipt/Cash Receipt ----

        [HttpGet]
        public ActionResult CashReceipt(int PartyId = 0, string PartyName = "", string Type = "INVOICE")
        {
            DSR_CashReceiptModel ObjCashReceipt = new DSR_CashReceiptModel();
            
            var objRepo = new DSR_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetCashPartyList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new DSRCashReceipt());
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
                //new SelectListItem { Text = "ON ACCOUNT", Value = "ONACCOUNT"},
            }, "Value", "Text");
                ViewBag.PaymentMode = PaymentMode;

                ViewBag.CashReceiptInvoiveMappingList = null;
                ObjCashReceipt.ReceiptDate = DateTime.Today.ToString("dd/MM/yyyy");
                ObjCashReceipt.Type = Type;
                ViewBag.ServerDate = Utility.GetServerDate();
                ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
                return PartialView(ObjCashReceipt);
            }
            else
            {
                objRepo.GetCashPartyList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetails(PartyId, PartyName, Type);
                
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (DSR_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new DSRCashReceipt());
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
                 //new SelectListItem { Text = "ON ACCOUNT", Value = "ONACCOUNT"},
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
                ViewBag.ServerDate = Utility.GetServerDate();
                ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
                if (Type == "CREDITNOTE")
                {
                    ModelState.Remove("TotalPaymentReceipt");

                }
                //objRepo.GetOnAccountBalance(PartyId);
                //if (objRepo.DBResponse.Data != null)
                //    ViewBag.BalOnAccount = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).BalOnAccount;
                //else
                //    ViewBag.BalOnAccount = 0.00;

                return PartialView(ObjCashReceipt);
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashReceipt(DSR_CashReceiptModel ObjCashReceipt)
        {
            List<DSRCashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<DSRCashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<DSRCashReceiptInvoiveMapping>));

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
            var objRepo = new DSR_CashManagementRepository();
            objRepo.AddCashReceipt(ObjCashReceipt, xml);          
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrint(int CashReceiptId)
        {
            var objRepo = new DSR_CashManagementRepository();
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
            var objRepo = new DSR_CashManagementRepository();
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

        public ActionResult CashReceiptList(string ReceiptNo = "")
        {
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetCashReceiptList(ReceiptNo);
            List<DSR_CashReceiptModel> lstCashReceipt = new List<DSR_CashReceiptModel>();
            lstCashReceipt = (List<DSR_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("CashReceiptList", lstCashReceipt);
        }

        public ActionResult GetLoadMoreCashReceiptList(string ReceiptNo = "", int Page = 0)
        {
            DSR_CashManagementRepository objCR = new DSR_CashManagementRepository();
            objCR.GetLoadMoreCashReceiptList(ReceiptNo, Page);
            List<DSR_CashReceiptModel> lstCashReceipt = new List<DSR_CashReceiptModel> ();
            if (objCR.DBResponse.Data != null)
            {
                lstCashReceipt = (List<DSR_CashReceiptModel>)objCR.DBResponse.Data;
            }
            return Json(lstCashReceipt, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region kol Cash Management Copy



        #region Edit cash Receipt
        //[HttpGet]
        public ActionResult EditCashReceiptPaymentMode()//int InvoiceId = 0, string InvoiceNo = ""
        {
            DSR_CashReceiptModel ObjCashReceipt = new DSR_CashReceiptModel();
            var objRepo = new DSR_CashManagementRepository();
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
                ObjCashReceipt.CashReceiptDetail.Add(new DSRCashReceipt());
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
            var objRepo = new DSR_CashManagementRepository();
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
            var objRepo = new DSR_CashManagementRepository();
            objRepo.SaveEditedCashRcpt(objEditReceiptPayment, ((Login)(Session["LoginUser"])).Uid);

            return Json(objRepo.DBResponse);

        }
        #endregion

        #region-- RECEIVE VOUCHER --

        [HttpGet]
        public ActionResult ReceivedVoucher()
        {
            var objRepo = new DSR_CashManagementRepository();            
            objRepo.GetReceivedVoucherCreateInfo();
            ViewData["ComName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyName;
            ViewData["COMGST"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyGSTNo;
            ViewData["COMGstStateCode"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).GstStateCode;
            ViewData["COMStateName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).StateName;
            ViewData["COMAddress"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyAddress;
            ViewData["COMCityName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CityName;
            ViewData["COMPanNo"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).PanNo;

            ViewData["InvoiceNo"] = objRepo.ReceiptVoucherNo();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReceiptVoucher(DSR_ReceiptVoucherModel vm)
        {
            if (vm.InstrumentDate != null)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(vm.InstrumentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("InstrumentDate", ex.Message);
                }


            }


            if (ModelState.IsValid)
            {
                var objRepo = new DSR_CashManagementRepository();
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
                pages[0] = Fc["page1"].ToString();
                pages[0] = Fc["Page1"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
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
            var objRepo = new DSR_CashManagementRepository();
            //GetCompanyInfo(objRepo);
            objRepo.GetReceiptVoucherList(0);
            ViewBag.State = 0;
            IEnumerable<DSR_ReceiptVoucherModel> lstRcptVou = (IEnumerable<DSR_ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
                ViewBag.State = objRepo.DBResponse.Message;
                return PartialView(lstRcptVou);
            }
            else
            {
                return PartialView(new List<DSR_ReceiptVoucherModel>());
            }
        }

        private void GetCompanyInfo(DSR_CashManagementRepository objRepo)
        {
            objRepo.GetReceivedVoucherCreateInfo();
            ViewData["ComName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyName;
            ViewData["COMGST"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyGSTNo;
            ViewData["COMGstStateCode"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).GstStateCode;
            ViewData["COMStateName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).StateName;
            ViewData["COMAddress"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CompanyAddress;
            ViewData["COMCityName"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).CityName;
            ViewData["COMPanNo"] = ((DSR_ReceiptVoucherModel)objRepo.DBResponse.Data).PanNo;
            ViewData["InvoiceNo"] = objRepo.ReceiptVoucherNo();
        }

        public JsonResult GetReceiptVoucherListByPage(int Page)
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetReceiptVoucherList(Page);
            //ViewBag.State = 0;
            IEnumerable<DSR_ReceiptVoucherModel> lstRcptVou = (IEnumerable<DSR_ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
               // ViewBag.State = objRepo.DBResponse.Message;
                return Json(new {Data= lstRcptVou,State= objRepo.DBResponse.Message },JsonRequestBehavior.AllowGet);
            }
            else
            {
               
                return Json(new { Data = "", State = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetReceiptVoucherListBySearch(string VoucherNo)
        {
            var objRepo = new DSR_CashManagementRepository();
            //GetCompanyInfo(objRepo);
            objRepo.GetReceiptVoucherList(VoucherNo);
            IEnumerable<DSR_ReceiptVoucherModel> lstRcptVou = (IEnumerable<DSR_ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
                return PartialView("GetReceiptVoucherList", lstRcptVou);
            }
            else
            {
                return PartialView("GetReceiptVoucherList", new List<DSR_ReceiptVoucherModel>());
            }
        }
        #endregion

        #region Credit Note
        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetInvoiceNoForCreaditNote("C");
            if (objRepo.DBResponse.Data != null)
                ViewBag.InvoiceNo = objRepo.DBResponse.Data;
            else
                ViewBag.InvoiceNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }



        //Get InvoiceNo against Invoice or Debit Note
        [HttpGet]
        public JsonResult GetInvoiceNoCreaditNote(string InvoiceType)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();

            if (InvoiceType == "Invoice")
                objRepo.GetInvoiceNoForCreaditNote("C");
            else
                objRepo.GetInvoiceNoAgainstDebitForCreaditNote();
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDebitNoteDetailsForCreaditNote(int InvoiceId)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetDebitNoteDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetInvoiceDetailsForCreaditNote(int InvoiceId)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId,"C");
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(DSRCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value credit note can not be saved." });
                }
                else
                {
                    DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchCreditNote(string Search)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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

            html.Append("<table style='width: 100%; font-size: 7pt; font-family: Verdana, Arial, San-serif; border-collapse: collapse;'>");
            html.Append("<thead>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

            html.Append("<tr>");

            html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</ span><br/><label style='font-size: 7pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 7pt; font-weight: bold;'>" + note + "</label></td></tr>");
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
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'></td>");
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
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'><b>Original Bill of Supply/Tax Invoice No:</b> <span>" + objCR.InvoiceNo + "</span></td>");
            html.Append("<td colspan='6' style='border: 1px solid #000; text-align: left; padding: 8px;'>");
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
            html.Append("<th colspan='4' style='border:1px solid #000;border-right:0;text-align:left;padding:5px;'>Round Up</th>");
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
            html.Append("<th style='border:1px solid #000;border-left:0;text-align:right;padding:5px;'>" + objCR.GrandTotal + "</th>");
            html.Append("</tr>");

            //tfoot +="<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr>";
            html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr>");
            html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr>");
            //html.Append("<tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Payer Name:</span> <span>" + PayeeName + "</span></td></tr>");
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
            html.Append("<td></td>");
            html.Append("<td colspan='10' style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td>");
            html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td colspan='12' style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            //html = html + htmltable +tr+ tfoot;

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html = html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(html.ToString());
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
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

        [HttpPost]
        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            DSR_CashManagementRepository objPpgRepo = new DSR_CashManagementRepository();

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

                Import.Models.DSR_IrnB2CDetails q1 = new Import.Models.DSR_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDR(CrNoteNo, Type, CRDR);
                var irnb2cobj = (Import.Models.DSR_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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

                        Import.Models.DSR_IrnB2CDetails q1 = new Import.Models.DSR_IrnB2CDetails();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDR(CrNoteNo, Type, CRDR);
                        var irnb2cobj = (Import.Models.DSR_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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
       [HttpPost]
      
        #endregion

        #region Cash Collection Against Bounced Cheque
        [HttpGet]
        public ActionResult CashColAgnBncCheque()
        {
            DSR_CashColAgnBncChq objCashColAgnBncChq = new DSR_CashColAgnBncChq();
            DSR_CashManagementRepository objExport = new DSR_CashManagementRepository();
            objExport.GetPaymentPartyBncCheque();
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
        public JsonResult AddCashColAgnBncCheque(DSR_CashColAgnBncChq objCashColAgnBncChq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_InvoiceRepository objGPR = new DSR_InvoiceRepository();
            objGPR.GetInvoiceDetailsForChqBouncePrintByNo(InvoiceNo, "CC");
            DSRInvoiceYard objGP = new DSRInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (DSRInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceChqBounce(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        private string GeneratingPDFInvoiceChqBounce(DSRInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var SEZis = "";
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

            html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr>");
            html.Append("<td width='80%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objGP.CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>Payment aganist Return Cheque</label></td>");
            html.Append("</tr>");
            html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> "+objGP.ExportUnder + " </td></tr>");
            html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
            html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");
            html.Append("</tbody></table></td>");
            html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objGP.ApproveOn)) + "'/> </td>");
            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            //html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            //html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASRATH</span>");
            //html.Append("<br />Payment aganist Return Cheque");
            //html.Append("</td></tr>");
            //html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            //html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");

            html.Append("<tr><td><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");

            html.Append("<tr><td>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
            html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000; padding-bottom:5px;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + objGP.InvoiceNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + objGP.PartyName + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + objGP.PartyAddress + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>"+objGP.PartyGST + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + objGP.PartyState + "(" + objGP.PartyStateCode + ")</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("<td colspan='6' width='40%' style='border-bottom: 1px solid #000; padding-bottom:5px;'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + objGP.InvoiceDate + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + objGP.PartyState + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + objGP.PartyStateCode + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + objGP.PaymentMode + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>"+objGP.ShippingLineName + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");

            //html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
            //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + objGP.InvoiceNo + "</span></td>");
            //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + objGP.InvoiceDate + "</span></td></tr>");
            //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
            //html.Append("<span>" + objGP.PartyName + "</span></td>");
            //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + objGP.PartyState + "</span> </td></tr>");
            //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
            //html.Append("Party Address :</label> <span>" + objGP.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
            //html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + objGP.PartyStateCode + "</span></td></tr>");
            //html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGST + "</span></td>");
            //html.Append("</tr></tbody> ");
            //html.Append("</table></td></tr>");

            html.Append("<tr><td><table style='border: 1px solid #000; width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr><td><label style='font-weight: bold;'>Cheque No :</label> <span>" + objGP.lstBounceChequeDtl[0].BncChequeNo + "</span></td>");
            html.Append("<td><label style='font-weight: bold;'>Cheque Date : </label> <span>" + objGP.lstBounceChequeDtl[0].BncChequeDate + "</span></td></tr>");
            html.Append("<tr><td><label style='font-weight: bold;'>Cheque Amount : </label><span>" + objGP.lstBounceChequeDtl[0].BncChequeAmt + "</span></td>");
            html.Append("<td><label style='font-weight: bold;'>Bounced Date :</label> <span>" + objGP.lstBounceChequeDtl[0].BounceDate + "</span> </td></tr>"); 
            html.Append("</table></td></tr>");

            html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt;'>Bounced Cheque Charges :</h3> </th></tr><tr><td>");
            html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 70px;'>SR No.</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Charge Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px;'>Taxable Amt.</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px;'>Total</th></tr>");
            html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Amt.</th></tr></thead>");
            html.Append("<tbody>");
            int i = 1;
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }

            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 70px;'>Total :</th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 24%;'></th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 80px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 7pt; text-align: right; width: 100px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + objGP.TotalCGST.ToString("0.00") + "</th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + objGP.TotalSGST.ToString("0.00") + "</th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + objGP.TotalIGST.ToString("0.00") + "</th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Round Up :</th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 24%;'></th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 80px;'></th>");
            html.Append("<th rowspan='2' style='width: 130px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 7pt; text-align: right; width: 100px;'>"+ objGP.TotalAmt.ToString() + "</th></tr><tr>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; border-bottom:0; width: 100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Total Invoice :</th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 24%;'></th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 80px;'></th>");
            html.Append("<th rowspan='2' style='width: 130px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 7pt; text-align: right; width: 100px;'>" + objGP.TotalAmt.ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='8'>");
            html.Append("<tbody>");
            html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='8' width='75%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
            html.Append("<td style='font-size: 7pt; text-align: left;' colspan='3' width='25%'>Payer Code:<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr>");
            html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='8' width='75%'>Payer Name:<label style='font-weight: bold;'>" + objGP.PayeeName.ToString() + "</label></td></tr>");
            html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Signature CHA / Importer</td>");
            html.Append("<td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Assistant <br/>(Signature)</td>");
            html.Append("<td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/><br/><br/>(Authorized Signatories)</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

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

        [HttpGet]
        public ActionResult GetBounceChequeInvoiceList(string CashReceiptDate = null, string CashReceiptNumber = null)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSR_CashColAgnBncChq> lstBncChequeInvoice = new List<DSR_CashColAgnBncChq>();
            
            
            ObjRI.GetBounceChequeInvoiceList(CashReceiptDate, CashReceiptNumber);
            if (ObjRI.DBResponse.Data != null)
            {
                lstBncChequeInvoice = (List<DSR_CashColAgnBncChq>)ObjRI.DBResponse.Data;
            }
            return PartialView("CashColBounceCheeckList", lstBncChequeInvoice);
        }

        #endregion

        #region Debit Note
        [HttpGet]
        public ActionResult CreateDebitNote()
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
        public JsonResult AddDebitNote(DSRCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value debit note can not be saved." });
                }
                else
                {
                    DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchDebitNote(string Search)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.SearchCreditDebitNote("D", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfDRNote", lstNote);
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository ObjCash = new DSR_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc();
            if (ObjCash.DBResponse.Status > 0)
                ViewBag.PaymentParty = (List<DSRPaymentPartyName>)ObjCash.DBResponse.Data;
            else
                ViewBag.PaymentParty = null;

            ObjCash.PurposeListForMiscInvc();
            if (ObjCash.DBResponse.Data != null)
            {
                ViewBag.PurposeList = (List<SelectListItem>)ObjCash.DBResponse.Data;
            }
            else
            {
                ViewBag.PurposeList = null;
            }
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId, decimal Amount,string SEZ)
        {
            DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, InvoiceType, PartyId, Amount, SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<DSRMiscPostModel>(objForm["MiscInvModelJson"].ToString());
                DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
                objChargeMaster.AddMiscInv(invoiceData, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv");
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ListOfMiscInvoice(string Module,string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_CashManagementRepository objER = new DSR_CashManagementRepository();
            objER.ListOfMiscInvoice(Module,InvoiceNo, InvoiceDate);
            List<DSRListOfMiscInvoice> obj = new List<DSRListOfMiscInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfMiscInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfMiscInvoice", obj);
        }

        #endregion

         #region Refund From SD

        [HttpGet]
        public ActionResult RefundFromPDA()
        {

            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            return PartialView();
        }

        [HttpGet]
        public ActionResult SDRefundList()
        {
            List<DSR_SDRefundList> ObjSd = new List<DSR_SDRefundList>();
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            ObjCR.GetSDRefundList();
            if (ObjCR.DBResponse.Data != null)
                ObjSd = (List<DSR_SDRefundList>)ObjCR.DBResponse.Data;
            return PartialView(ObjSd);
        }

        [HttpGet]
        public ActionResult ViewSDRefund(int PdaAcId)
        {
            DSR_ViewSDRefund ObjSD = new DSR_ViewSDRefund();
            DSR_CashManagementRepository objCR = new DSR_CashManagementRepository();
            objCR.ViewSDRefund(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (DSR_ViewSDRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(DSR_AddMoneyToPDModelRefund m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository ObjRR = new DSR_CashManagementRepository();
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

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
                objCashManagement.GetYardInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    DSRInvoiceYard objPostPaymentSheet = (DSRInvoiceYard)objCashManagement.DBResponse.Data;

                    IList<Export.Models.PaymentSheetContainer> containers = new List<Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new Export.Models.PaymentSheetContainer
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
                    List<Export.Models.PaymentSheetContainer> containersAll = new List<Export.Models.PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Yard");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<Export.Models.PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new Export.Models.PaymentSheetContainer
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

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
                objCashManagement.GetDeliInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    //DSRInvoiceYard objPostPaymentSheet = (DSRInvoiceYard)objCashManagement.DBResponse.Data;
                    DSRInvoiceGodown objPostPaymentSheet = (DSRInvoiceGodown)objCashManagement.DBResponse.Data; //new DSRInvoiceGodown();

                    IList<DSRPaymentSheetBOE> containers = new List<DSRPaymentSheetBOE>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new DSRPaymentSheetBOE
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

        public JsonResult AddEditDeliPaymentSheet(DSRInvoiceGodown objForm)
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

                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
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

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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

        public JsonResult AddEditContPaymentSheet(DSRInvoiceYard objForm)
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
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, 0,"", "EXPLod", "", "", 0, "",0,0);

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

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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

        public JsonResult EditContMovementPaymentSheet(Export.Models.DSR_MovementInvoice objForm)
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

                //DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
                objCashManagement.GetContainerMovementInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Export.Models.DSR_MovementInvoice objPostPaymentSheet = (Areas.Export.Models.DSR_MovementInvoice)objCashManagement.DBResponse.Data;

                    IList<Export.Models.PaymentSheetContainer> containers = new List<Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new Export.Models.PaymentSheetContainer
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
            DSR_CashManagementRepository objRR = new DSR_CashManagementRepository();
            objRR.ListOfReceiptForCancel();
            if (objRR.DBResponse.Data != null)
                ViewBag.ReceiptList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(objRR.DBResponse.Data));
            else ViewBag.ReceiptList = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ListOfCancelledReceipt()
        {
            DSR_CashManagementRepository objRR = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository objRR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            objChe.GetChemical();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ChemicalLst = null;


            return PartialView();
        }
        [HttpPost]

        public JsonResult GetFumigation(string FumigationChargeType, string InvoiceType, int PartyId, string size, List<Chemical> ChemicalDetails)
        {
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }
            DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
            objChargeMaster.GetFumigation(FumigationChargeType, InvoiceType, PartyId, size, XMLText);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFumigationInvoice(DSR_FumigationInvoice FumigationModel)
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
                    DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository repo = new DSR_CashManagementRepository();
            repo.GetContainersForFumigation();
            return Json(repo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Rent Invoice

        [HttpGet]
        public ActionResult CreateRentInvoice()
        {
            DSR_CashManagementRepository objCash = new DSR_CashManagementRepository();


            List<SelectListItem> lstPurpose = new List<SelectListItem>();
            DSR_CashManagementRepository ObjCash = new DSR_CashManagementRepository();
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
        public JsonResult AddEditRentInvoice(DSR_RentInvoice objForm, String Month, int Year)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string RentDetailsXML = "";
                string ChargeDetailsXML = "";

                if (invoiceData.lstRentDetails != null)
                {
                    RentDetailsXML = Utility.CreateXML(invoiceData.lstRentDetails);
                }
                if (invoiceData.lstRentInvoiceCharge != null)
                {
                    ChargeDetailsXML = Utility.CreateXML(invoiceData.lstRentInvoiceCharge);
                }



                DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
                objChargeMaster.AddEditRentInv(invoiceData, RentDetailsXML, ChargeDetailsXML, BranchId, ((Login)(Session["LoginUser"])).Uid, Month, Year, "Rent");
                
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
        public JsonResult GetMonthYearRentData(string Month, int Year, int Flag)
        {

            DSR_CashManagementRepository objPpgRepo = new DSR_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetRentDet(Month, Year, Flag);
            var Output = (DSR_RentInvoice)objPpgRepo.DBResponse.Data;


            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRentInvoiceList(int Page=0)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSRRentDetails> lstRentInvoice = new List<DSRRentDetails>();
            ObjRI.GetRentInvoicesList(Page);
            ViewBag.State = 0;
            if (ObjRI.DBResponse.Data != null)
            {
                ViewBag.State = ObjRI.DBResponse.Message;
               lstRentInvoice = (List<DSRRentDetails>)ObjRI.DBResponse.Data;
            }
            return PartialView("RentInvoiceList", lstRentInvoice);
        }


        [HttpGet]
        public JsonResult GetLoadmoreRentInvoiceList(int Page = 0)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSRRentDetails> lstRentInvoice = new List<DSRRentDetails>();
            ObjRI.GetRentInvoicesList(Page);
            
            if (ObjRI.DBResponse.Data != null)
            {
               
                lstRentInvoice = (List<DSRRentDetails>)ObjRI.DBResponse.Data;
                return Json(new {data= lstRentInvoice,State = ObjRI.DBResponse.Message },JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = lstRentInvoice, State = ObjRI.DBResponse.Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRentInvoiceListBySearch(string InvoiceNo)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSRRentDetails> lstRentInvoice = new List<DSRRentDetails>();
            ObjRI.GetRentInvoicesList(InvoiceNo);
            if (ObjRI.DBResponse.Data != null)
            {
                lstRentInvoice = (List<DSRRentDetails>)ObjRI.DBResponse.Data;
            }
            return PartialView("RentInvoiceList", lstRentInvoice);
        }

        #endregion

        #region Reservation
        public ActionResult ReservationInvoice(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
           

            return PartialView();
        }

        public JsonResult GetReservationParties(string month, int year)
        {            
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            objChe.GetReservationParties(month, year);
            //if (objChe.DBResponse.Status > 0)
            //    ViewBag.ReservationParties = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            //else
            //    ViewBag.ReservationParties = null;

            return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReservationInvoices(string month, int year, int mode)
        {
            
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();

            objChe.GetReservationInvoices(month, year, mode);
            
            return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetReservationInvoiceList(string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSRReservationPartyDetails> lstReservationInvoice = new List<DSRReservationPartyDetails>();
            ObjRI.GetReservationInvoicesList(InvoiceNo, InvoiceDate); 
            if (ObjRI.DBResponse.Data != null)
            {
                lstReservationInvoice = (List<DSRReservationPartyDetails>)ObjRI.DBResponse.Data;
            }
            return PartialView("ReservationList", lstReservationInvoice);
        }

        //[HttpGet]
        //public JsonResult GetReservationInvoiceList(string month, int year, int mode)//string month, int year, int mode
        //{
        //    DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
        //    objChe.GetReservationInvoices("Aug", 2019, 1);

        //    return PartialView("ReservationList", objChe.DBResponse);
        //}

        public JsonResult CreateReservationInvoices(string month, int year,int pid, int mode,string invoicedate,string SEZ)
        {
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();            
            objChe.CreateReservationInvoices(month, year, pid, mode, Convert.ToDateTime(invoicedate).ToString("yyyy-MM-dd"), SEZ);
            return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddEditReservationInvoice(List<DSRReservationPartyDetails> objs, string month, string year, string SEZ, string type = "Tax")
        {
            try
            {

                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string dtlslXml = Utility.CreateXML(objs);
                DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();

                objChe.AddEditInvoiceReservation(dtlslXml, type, BranchId, ((Login)(Session["LoginUser"])).Uid, month, year,SEZ);

                //if (objs.Where(o => o.InvoiceId > 0).Count() > 0) //Count > 0 => Update
                //{
                //    objChe.AddEditInvoiceReservation(dtlslXml, 2, type, BranchId, ((Login)(Session["LoginUser"])).Uid, month, year);
                //}
                //else
                //{
                //    objChe.AddEditInvoiceReservation(dtlslXml, 1, type, BranchId, ((Login)(Session["LoginUser"])).Uid, month, year);
                //}

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
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;
            return PartialView();
        }

        public ActionResult ListChequeDeposit()
        {
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            objChe.GetChequeDepositsAll();
            List<ChequeDepositList> lst = new List<ChequeDepositList>();
            if (objChe.DBResponse.Data != null)
                lst = (List<ChequeDepositList>)objChe.DBResponse.Data;
            return PartialView(lst);
        }
        public ActionResult EditChequeDeposit(int Id)
        {
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();

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
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();

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
            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
                DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
                    DSR_CashManagementRepository objER = new DSR_CashManagementRepository();

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
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyList();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfParty = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPartyWiseSDBalance(int PartyId, string BalanceDate)
        {
            try
            {
                decimal BalanceAmount = 0;
                DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
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
            DSR_ExportRepository objExport = new DSR_ExportRepository();

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
            DSR_CashManagementRepository objCash = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objCR = new DSR_CashManagementRepository();
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
            //DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            //objChe.GetChemical();
            //if (objChe.DBResponse.Status > 0)
            //    ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            //else
            //    ViewBag.ChemicalLst = null;


            return PartialView();
        }
        public ActionResult EditContainerDebitInvoice(int InvoiceId) 
        {
            ViewData["InvType"] = "Tax";
            return PartialView("EditContainerDebitInvoice");
        }
        public ActionResult ViewContainerDebitInvoice(int InvoiceId)
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            DSR_DebitInvoice ContainerDebitInvoice = new DSR_DebitInvoice();
            ObjRI.GetContainerDebitInvoiceDetails(InvoiceId);
            if (ObjRI.DBResponse.Data != null)
            {
                ContainerDebitInvoice = (DSR_DebitInvoice)ObjRI.DBResponse.Data;
            }
            
            return PartialView("ViewContainerDebitInvoice", ContainerDebitInvoice);
        }
        public ActionResult GetContainerDebitInvoiceList()
        {
            DSR_CashManagementRepository ObjRI = new DSR_CashManagementRepository();
            List<DSR_DebitInvoice> lstContainerDebitInvoice = new List<DSR_DebitInvoice>();
            ObjRI.GetContainerDebitInvoiceList();
            if (ObjRI.DBResponse.Data != null)
            {
                lstContainerDebitInvoice = (List<DSR_DebitInvoice>)ObjRI.DBResponse.Data;
            }


            return PartialView("DebitInvoiceList", lstContainerDebitInvoice);
        }

        [HttpGet]
        public JsonResult ListOfContainer()
        {
            DSR_CashManagementRepository objcash = new DSR_CashManagementRepository();
            objcash.GetContainerList();
            //List<DSR_ContainerStuffing> objImp = new List<DSR_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                //objImp = (List<DSR_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<DSR_Container>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { Id = item.id, ContainerNo = item.ContainerNo, size = item.size, CFSCode = item.CFSCode, InDate = item.In_Date });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult ListOfParty()
        //{
        //    DSR_CashManagementRepository objcash = new DSR_CashManagementRepository();
        //    objcash.ListOfGREParty();
        //    //List<DSR_ContainerStuffing> objImp = new List<DSR_ContainerStuffing>();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyList("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayee(string PartyCode, int Page)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPayeeList("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPayeeCode(string PartyCode)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPayeeList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAmount(int ChargeId, String ChargeName, String Size, string FromDate, string ToDate)
        {
            DSR_CashManagementRepository objcash = new DSR_CashManagementRepository();
           // objcash.GetAmountForCharges(ChargeId, ChargeName, Size, FromDate, ToDate);
            
            Decimal Amount = 0;
            //if (objcash.DBResponse.Data != null)
            //{
               // Amount = (Decimal)objcash.DBResponse.Data;
            //}
            return Json(Amount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCharge()
        {
            DSR_CashManagementRepository objcash = new DSR_CashManagementRepository();
            objcash.ListOfChargesName();
            //List<DSR_ContainerStuffing> objImp = new List<DSR_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<DSRCharge>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { ChargeId = item.ChargeId, ChargeName = item.ChargeName,ChargeType= item.ChargeType,SacCode=item.SacCode });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetDebitInvoice(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges,string SEZ)
        {

            DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
            objChargeMaster.GetDebitInvoice(InvoiceType, PartyId, TotalChrgAmount, Charges, SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDebitInvoice(DSR_DebitInvoice DebitModel)
        {

            ModelState.Remove("ChargeId");
            ModelState.Remove("ChargeName");
            if (ModelState.IsValid)
            {
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                    IList<DSRCharge> LstChemical = JsonConvert.DeserializeObject<IList<DSRCharge>>(DebitModel.ChargeXML);
                    string ChargeXML = Utility.CreateXML(LstChemical);
                    DSR_CashManagementRepository objChargeMaster = new DSR_CashManagementRepository();
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
            DSR_CashReceiptModel ObjCashReceipt = new DSR_CashReceiptModel();

            var objRepo = new DSR_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetPartyListSD();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new DSRCashReceipt());
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
                    ViewBag.Party = ((DSR_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetailsSD(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (DSR_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((DSR_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
                }
                else
                {
                    ViewBag.Pay = null;
                    ViewBag.PdaAdjust = null;
                    ViewBag.Container = null;
                }

                for (var i = 0; i < 7; i++)
                {
                    ObjCashReceipt.CashReceiptDetail.Add(new DSRCashReceipt());
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
        public ActionResult AddCashReceiptSD(DSR_CashReceiptModel ObjCashReceipt)
        {
            List<DSRCashReceiptInvoiveMapping> CashReceiptInvDtlsList = (List<DSRCashReceiptInvoiveMapping>)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjCashReceipt.CashReceiptInvDtlsHtml, typeof(List<DSRCashReceiptInvoiveMapping>));

            foreach (var item in CashReceiptInvDtlsList)
            {
                DateTime dt = DateTime.ParseExact(item.InvoiceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                item.InvoiceDate = dt.ToString("yyyy-MM-dd");
                ObjCashReceipt.InvoiceValue = ObjCashReceipt.InvoiceValue + item.InvoiceAmt;
            }

            ObjCashReceipt.CashReceiptInvDtlsHtml = Utility.CreateXML(CashReceiptInvDtlsList);
            var xml = Utility.CreateXML(ObjCashReceipt.CashReceiptDetail.Where(o => o.Amount > 0).ToList());
            // ObjCashReceipt.BranchId = Convert.ToInt32(Session["BranchId"]);
            var objRepo = new DSR_CashManagementRepository();
            objRepo.AddCashReceiptSD(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrintSD(int CashReceiptId)
        {
            var objRepo = new DSR_CashManagementRepository();
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
            var objRepo = new DSR_CashManagementRepository();
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
            return PartialView();
        }

        [HttpGet]
        public JsonResult LoadChequeParty(string PartyCode, int Page)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyDetail("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByChequePartyCode(string PartyCode)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetPartyDetail(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChequeDetail(int PartyId)
        {



            DSR_CashManagementRepository objChe = new DSR_CashManagementRepository();
            List<dynamic> objImp2 = new List<dynamic>();
            objChe.GetChequeNo(PartyId);
            if (objChe.DBResponse.Data != null)
            {
                ((List<DSR_ChequeDetail>)objChe.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { Id = item.Id, Cheque = item.Cheque });
                });
            }




            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchChequeNo(int partyid, string ChequeNo)
        {
            DSR_CashManagementRepository objImport = new DSR_CashManagementRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            objImport.GetChequeDetail(partyid, ChequeNo);

            if (objImport.DBResponse.Data != null)
            {
                ((List<DSR_ChequeDetail>)objImport.DBResponse.Data).ToList().ForEach(item =>
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

                DSR_CashManagementRepository objCh = new DSR_CashManagementRepository();
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
        public JsonResult AddChequeReturn(DSR_ChequeReturn m)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var objRepo = new DSR_CashManagementRepository();
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
        public ActionResult ListOfChequeReturn()
        {
            DSR_CashManagementRepository objER = new DSR_CashManagementRepository();
            List<DSR_ChequeReturn> lstPartyWiseTDSDeposit = new List<DSR_ChequeReturn>();
            objER.GetAllChequeReturn();
            if (objER.DBResponse.Data != null)
                lstPartyWiseTDSDeposit = (List<DSR_ChequeReturn>)objER.DBResponse.Data;
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

        [HttpGet]
        public ActionResult EditBondDeliPaymentSheet(string type = "Tax")
        {
            ExportRepository objImport = new ExportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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

            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForDelBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
            {
                var BondList = (IList<BondSacDetails>)objBond.DBResponse.Data;
                TempData["SACList"] = BondList;

                IList<BondSacDetails> DistinctSacList = new List<BondSacDetails>();
                BondList.ToList().ForEach(item =>
                {
                    if (!DistinctSacList.Any(o => o.DepositAppId == item.DepositAppId))
                    {
                        DistinctSacList.Add(item);
                    }
                });
                ViewBag.SACList = JsonConvert.SerializeObject(DistinctSacList);

            }
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondDelPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ExportRepository objExport = new ExportRepository();
                //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objExport.DBResponse);

                var invoiceData = JsonConvert.DeserializeObject<BondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                BondRepository objChargeMaster = new BondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BND");
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

        [HttpGet]
        public JsonResult GetBondDelPaymentSheetDetails(int InvoiceId)
        {

            var objBondPostPaymentSheet = new BondPostPaymentSheet();
            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();

            try
            {

                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
                objCashManagement.GetBondInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    objBondPostPaymentSheet = (BondPostPaymentSheet)objCashManagement.DBResponse.Data;

                    BondRepository objBond = new BondRepository();
                    objBond.GetSACNoForDelBondPaymentSheet();
                    if (objBond.DBResponse.Status > 0)
                    {
                        List<BondSacDetails> BondSacDetailsList = (List<BondSacDetails>)objBond.DBResponse.Data;
                        objBondPostPaymentSheet.UptoDate = BondSacDetailsList.Where(x => x.DepositAppId == objBondPostPaymentSheet.RequestId).Select(x => x.ValidUpto).FirstOrDefault();
                        objBondPostPaymentSheet.Area = BondSacDetailsList.Where(x => x.DepositAppId == objBondPostPaymentSheet.RequestId).Select(x => x.AreaReserved).FirstOrDefault();
                        objBondPostPaymentSheet.Weight = BondSacDetailsList.Where(x => x.DepositAppId == objBondPostPaymentSheet.RequestId).Select(x => x.Weight).FirstOrDefault();
                    }

                    DataAccessLayerBaseClass DataAccess = DataAccessLayerFactory.GetDataAccessLayer();
                    var objGenericCharges = (GenericChargesModel)DataAccess.ExecuteDynamicSet<GenericChargesModel>("GetAllCharges");
                    var objBondList = ((IList<BondSacDetails>)TempData.Peek("SACList")).Where(o => o.DepositAppId == objBondPostPaymentSheet.RequestId).ToList();

                    objBondPostPaymentSheet.CIFValue = objBondList.ToList().Sum(o => o.CIFValue);
                    objBondPostPaymentSheet.Duty = objBondList.ToList().Sum(o => o.Duty);
                    objBondPostPaymentSheet.Units = objBondList.ToList().Sum(o => o.Units);
                    objBondPostPaymentSheet.TotalGrossWt = objBondList.ToList().Sum(o => o.Weight);
                    objBondPostPaymentSheet.TotalNoOfPackages = objBondList.ToList().Sum(o => o.Units);
                    objBondPostPaymentSheet.TotalWtPerUnit = (objBondList.ToList().Sum(o => o.Weight) / objBondList.ToList().Sum(o => o.Units));

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objBondPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objBondPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objBondPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objBondPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objBondPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objBondPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objBondPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objBondPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objBondPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objBondPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    objBondPostPaymentSheet.CompGST = CompGST;
                    var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    objBondPostPaymentSheet.CompStateCode = CompStateCode;
                    var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    objBondPostPaymentSheet.CompPAN = CompPAN;
                    #endregion

                    var GSTType = objBondPostPaymentSheet.PartyStateCode == CompStateCode;

                    #region Storage Charge
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var ActualStorage = 0M;
                    var validDate = DateTime.ParseExact(objBondPostPaymentSheet.UptoDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var exitDate = DateTime.ParseExact(objBondPostPaymentSheet.RequestDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    var TotalDays = 0;
                    if (exitDate > validDate)
                    {
                        TotalDays = Convert.ToInt32((exitDate - validDate).TotalDays + 1);
                        var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / 7);
                        ActualStorage = Math.Round(objBondPostPaymentSheet.Area * TotalWeeks * objGenericCharges.StorageRent.Where(o => o.WarehouseType == 3).FirstOrDefault().RateSqMPerWeek, 2);
                    }
                    //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //{
                    //    ChargeId = 1,
                    //    Clause = "4",
                    //    ChargeName = "Storage Charges",
                    //    ChargeType = "CWC",
                    //    SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                    //    Quantity = 0,
                    //    Rate = 0M,
                    //    Amount = ActualStorage,
                    //    Discount = 0M,
                    //    Taxable = ActualStorage,
                    //    CGSTPer = cgst,
                    //    SGSTPer = sgst,
                    //    IGSTPer = igst,
                    //    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                    //    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                    //    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                    //    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                    //        (Math.Round(ActualStorage * (cgst / 100), 2)) +
                    //        (Math.Round(ActualStorage * (sgst / 100), 2))) :
                    //        (ActualStorage + (Math.Round(ActualStorage * (igst / 100), 2)))) : ActualStorage
                    //});
                    #endregion

                    #region Insurance
                    var Inscgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var Inssgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst) / 2;
                    var Insigst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.InsuranceRate.FirstOrDefault().SacCode).Gst);
                    var Insurance = 0M;

                    objBondList.ToList().ForEach(item =>
                    {
                        TotalDays = Convert.ToInt32((DateTime.ParseExact(item.DeliveryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) -
                                    DateTime.ParseExact(item.UnloadedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);
                        Insurance += Math.Round(item.IsInsured == 0 ? Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (item.CIFValue + item.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000) : 0, 2);
                    });

                    //Insurance += Math.Ceiling(Convert.ToDecimal(TotalDays) / Convert.ToDecimal(7)) * (objBondPostPaymentSheet.CIFValue + objBondPostPaymentSheet.Duty) * ((objGenericCharges.InsuranceRate.FirstOrDefault().ChargeInRs) / 1000);            
                    //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //{
                    //    ChargeId = 2,
                    //    Clause = "5",
                    //    ChargeName = "Insurance Charges",
                    //    ChargeType = "CWC",
                    //    SACCode = objGenericCharges.InsuranceRate.FirstOrDefault().SacCode,
                    //    Quantity = 0,
                    //    Rate = 0M,
                    //    Amount = Insurance,
                    //    Discount = 0,
                    //    Taxable = Insurance,
                    //    CGSTPer = cgst,
                    //    SGSTPer = sgst,
                    //    IGSTPer = igst,
                    //    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (cgst / 100)) : 0) : 0, 2),
                    //    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance * (sgst / 100)) : 0) : 0, 2),
                    //    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (Insurance * (igst / 100))) : 0, 2),
                    //    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (Insurance +
                    //        (Math.Round(Insurance * (cgst / 100), 2)) +
                    //        (Math.Round(Insurance * (sgst / 100), 2))) :
                    //        (Insurance + (Math.Round(Insurance * (igst / 100), 2)))) : Insurance
                    //});
                    #endregion

                    #region Other Charges
                    try
                    {
                        var Ocgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                        var Osgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst) / 2;
                        var Oigst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.OtherCharges.FirstOrDefault().SacCode).Gst);
                        //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                        //{
                        //    ChargeId = 2,
                        //    Clause = "6",
                        //    ChargeName = "Others",
                        //    ChargeType = "CWC",
                        //    SACCode = objGenericCharges.OtherCharges.FirstOrDefault().SacCode,
                        //    Quantity = 0,
                        //    Rate = 0M,
                        //    Amount = 0M,
                        //    Discount = 0M,
                        //    Taxable = 0M,
                        //    CGSTPer = Ocgst,
                        //    SGSTPer = Osgst,
                        //    IGSTPer = Oigst,
                        //    CGSTAmt = 0M,
                        //    SGSTAmt = 0M,
                        //    IGSTAmt = 0M,
                        //    Total = 0M
                        //});
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region H & T Charges
                    var htc = objGenericCharges.HTChargeRent.FirstOrDefault(o => o.OperationCode == "5");

                    objBondPostPaymentSheet.Weight = objBondList.ToList().Sum(o => o.Weight);
                    var HTCharges = Math.Round((objBondPostPaymentSheet.Weight * htc.RateCWC), 2);
                    //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //{
                    //    ChargeId = 4,
                    //    Clause = "5",
                    //    ChargeName = htc.OperationSDesc,
                    //    ChargeType = "HT",
                    //    SACCode = htc.SacCode,
                    //    Quantity = 0,
                    //    Rate = 0M,
                    //    Amount = HTCharges,
                    //    Discount = 0M,
                    //    Taxable = HTCharges,
                    //    CGSTPer = cgst,
                    //    SGSTPer = sgst,
                    //    IGSTPer = igst,
                    //    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0, 2),
                    //    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0, 2),
                    //    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0, 2),
                    //    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges +
                    //        (Math.Round(HTCharges * (cgst / 100), 2)) +
                    //        (Math.Round(HTCharges * (sgst / 100), 2))) :
                    //        (HTCharges + (Math.Round(HTCharges * (igst / 100), 2)))) : HTCharges
                    //});
                    #endregion
                    //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //{
                    //    ChargeId = 2,
                    //    Clause = "5",
                    //    ChargeName = htc.OperationSDesc,
                    //    ChargeType = "HT",
                    //    SACCode = htc.SacCode,
                    //    Quantity = 0,
                    //    Rate = 0M,
                    //    Amount = HTCharges,
                    //    Discount = 0M,
                    //    Taxable = HTCharges,
                    //    CGSTPer = cgst,
                    //    SGSTPer = sgst,
                    //    IGSTPer = igst,
                    //    CGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (cgst / 100)) : 0) : 0,
                    //    SGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges * (sgst / 100)) : 0) : 0,
                    //    IGSTAmt = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (HTCharges * (igst / 100))) : 0,
                    //    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (HTCharges + (HTCharges * (cgst / 100)) + (HTCharges * (sgst / 100))) : (HTCharges + (HTCharges * (igst / 100)))) : HTCharges
                    //});

                    objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                    objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                    objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                    objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                    objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                    objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                    objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                    objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                    objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                    objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                    objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
                    objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
                    objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
                    objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
                    objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
                    objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
                    objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

                    //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                    //    InvoiceType);
                    return Json(new { Status = 1, Data = objBondPostPaymentSheet }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }

        }

        #region Export Invoice Edit
        [HttpGet]
        public ActionResult EditExportInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
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

                    IList<Export.Models.PaymentSheetContainer> containers = new List<Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new Export.Models.PaymentSheetContainer
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
                    List<Export.Models.PaymentSheetContainer> containersAll = new List<Export.Models.PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "EXP");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<Export.Models.PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new Export.Models.PaymentSheetContainer
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
        #endregion

        #region Load Container Payment Sheet Edit
        [HttpGet]
        public ActionResult EditLoadContainerInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
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

                    IList<Export.Models.PaymentSheetContainer> containers = new List<Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new Export.Models.PaymentSheetContainer
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
                    List<Export.Models.PaymentSheetContainer> containersAll = new List<Export.Models.PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "LOADED");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<Export.Models.PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new Export.Models.PaymentSheetContainer
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

        #region Bond Advanced Edit
        [HttpGet]
        public ActionResult EditBondAdvanced()
        {
            ImportRepository objImport = new ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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


            BondRepository objBond = new BondRepository();
            objBond.GetSACNoForAdvBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondAdvancedInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                objCharge.GetAllCharges();
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
                objCashManagement.GetInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    PostPaymentSheet objBondPostPaymentSheet = (PostPaymentSheet)objCashManagement.DBResponse.Data;
                    GenericChargesModel objGenericCharges = (GenericChargesModel)objCharge.DBResponse.Data;

                    //  var objBondPostPaymentSheet = new BondPostPaymentSheet();

                    #region Company Details
                    var _ROAddress = objGenericCharges.CompanyDetails.FirstOrDefault().ROAddress;
                    objBondPostPaymentSheet.ROAddress = _ROAddress;
                    var _CompanyId = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyId;
                    objBondPostPaymentSheet.CompanyId = _CompanyId ?? (int)_CompanyId;
                    var _CompanyName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyName;
                    objBondPostPaymentSheet.CompanyName = _CompanyName;
                    var _CompanyShortName = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyShortName;
                    objBondPostPaymentSheet.CompanyShortName = _CompanyShortName;
                    var _CompanyAddress = objGenericCharges.CompanyDetails.FirstOrDefault().CompanyAddress;
                    objBondPostPaymentSheet.CompanyAddress = _CompanyAddress;
                    var _PhoneNo = objGenericCharges.CompanyDetails.FirstOrDefault().PhoneNo;
                    objBondPostPaymentSheet.PhoneNo = _PhoneNo;
                    var _FaxNumber = objGenericCharges.CompanyDetails.FirstOrDefault().FaxNumber;
                    objBondPostPaymentSheet.FaxNumber = _FaxNumber;
                    var _EmailAddress = objGenericCharges.CompanyDetails.FirstOrDefault().EmailAddress;
                    objBondPostPaymentSheet.EmailAddress = _EmailAddress;
                    var _StateId = objGenericCharges.CompanyDetails.FirstOrDefault().StateId;
                    objBondPostPaymentSheet.StateId = _StateId ?? (int)_StateId;
                    var _CityId = objGenericCharges.CompanyDetails.FirstOrDefault().CityId;
                    objBondPostPaymentSheet.CityId = _CityId ?? (int)_CityId;

                    var CompGST = objGenericCharges.CompanyDetails.FirstOrDefault().GstIn;
                    objBondPostPaymentSheet.CompGST = CompGST;
                    var CompStateCode = objGenericCharges.CompanyDetails.FirstOrDefault().StateCode;
                    objBondPostPaymentSheet.CompStateCode = CompStateCode;
                    var CompPAN = objGenericCharges.CompanyDetails.FirstOrDefault().Pan;
                    objBondPostPaymentSheet.CompPAN = CompPAN;
                    #endregion


                    var GSTType = objBondPostPaymentSheet.PartyStateCode == CompStateCode;
                    var cgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var sgst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst) / 2;
                    var igst = (objGenericCharges.SACGST.FirstOrDefault(o => o.SacCode == objGenericCharges.StorageRent.FirstOrDefault().SacCode).Gst);
                    var ActualStorage = 0M;

                    BondRepository objBond = new BondRepository();
                    objBond.GetSACNoForAdvBondPaymentSheet();
                    if (objBond.DBResponse.Status > 0)
                    {
                        List<BondSacDetails> BondSacDetailsList = (List<BondSacDetails>)objBond.DBResponse.Data;
                        objBondPostPaymentSheet.UptoDate = BondSacDetailsList.Where(x => x.DepositAppId == objBondPostPaymentSheet.RequestId).Select(x => x.ValidUpto).FirstOrDefault();
                        objBondPostPaymentSheet.Area = BondSacDetailsList.Where(x => x.DepositAppId == objBondPostPaymentSheet.RequestId).Select(x => x.AreaReserved).FirstOrDefault();
                    }
                    var TotalDays = Convert.ToInt32((DateTime.ParseExact(objBondPostPaymentSheet.UptoDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) -
                                    DateTime.ParseExact(objBondPostPaymentSheet.RequestDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).TotalDays + 1);

                    var TotalWeeks = Math.Ceiling(Convert.ToDecimal(TotalDays) / 7);

                    ActualStorage = Math.Round(objBondPostPaymentSheet.Area * TotalWeeks * objGenericCharges.StorageRent.Where(o => o.WarehouseType == 3).FirstOrDefault().RateSqMPerWeek, 2);

                    //objBondPostPaymentSheet.lstPostPaymentChrg.Add(new PostPaymentCharge()
                    //{
                    //    ChargeId = 1,
                    //    Clause = "4",
                    //    ChargeName = "Storage Charges",
                    //    ChargeType = "CWC",
                    //    SACCode = objGenericCharges.StorageRent.FirstOrDefault().SacCode,
                    //    Quantity = 0,
                    //    Rate = 0M,
                    //    Amount = ActualStorage,
                    //    Discount = 0M,
                    //    Taxable = ActualStorage,
                    //    CGSTPer = cgst,
                    //    SGSTPer = sgst,
                    //    IGSTPer = igst,
                    //    CGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (cgst / 100)) : 0) : 0, 2),
                    //    SGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage * (sgst / 100)) : 0) : 0, 2),
                    //    IGSTAmt = Math.Round(objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? 0 : (ActualStorage * (igst / 100))) : 0, 2),
                    //    Total = objBondPostPaymentSheet.InvoiceType == "Tax" ? (GSTType ? (ActualStorage +
                    //        (Math.Round(ActualStorage * (cgst / 100), 2)) +
                    //        (Math.Round(ActualStorage * (sgst / 100), 2))) :
                    //        (ActualStorage + (Math.Round(ActualStorage * (igst / 100), 2)))) : ActualStorage
                    //});


                    objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                    objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
                    objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
                    objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
                    objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
                    objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
                    objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
                    objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

                    objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
                    objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
                    objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
                    objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
                    objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
                    objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
                    objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
                    objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
                    objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

                    //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                    //    InvoiceType);

                    return Json(new { Status = 1, Data = objBondPostPaymentSheet }, JsonRequestBehavior.AllowGet);
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
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondAdvPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //if (formData.lstPSContainer != null)
                //{
                //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
                //}
                //if (formData.lstChargesType != null)
                //{
                //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
                //}

                //ExportRepository objExport = new ExportRepository();
                //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objExport.DBResponse);

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

        #endregion

        #region BTT Invoice Edit
        [HttpGet]
        public ActionResult EditBTTInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();

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
                DSR_CashManagementRepository objCashManagement = new DSR_CashManagementRepository();
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

                    

                    IList<Export.Models.PaymentSheetContainer> containers = new List<Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new Export.Models.PaymentSheetContainer
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
                    List<Export.Models.PaymentSheetContainer> containersAll = new List<Export.Models.PaymentSheetContainer>();
                    objExportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "BTT");
                    if (objExportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<Export.Models.PaymentSheetContainer>>(JsonConvert.SerializeObject(objExportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new Export.Models.PaymentSheetContainer
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

        #region BankDeposit
        public ActionResult BankDeposit()
        {
           
            return PartialView();
        }

        public JsonResult GetBankAccount()
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            objRepo.GetBankAccount(BranchId);
                        
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
          
        }

        [HttpPost]
        public JsonResult AddEditBankDeposit(DSRBankDeposit obj)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.AddEditBankDeposit(obj, ((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBankDeposit(int Id)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.DeleteBankDeposit(Id, ((Login)(Session["LoginUser"])).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDepositList(string SearchValue)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetBankDepositList(SearchValue);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNEFTForBankDeposit(string dt)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.GetNEFTForBankDeposit(Convert.ToDateTime(dt));
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetDepositDetails(string DepositDate)
        //{
        //    DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
        //    objRepo.GetDepositDetails(DepositDate);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Empty Container Payment Sheet

        [HttpPost]
        public ActionResult AddEditECDeliveryPaymentSheetNew(FormCollection objForm)
        {
            return PartialView();
        }

        #endregion

        #region OnAccount  Add Money
        [HttpGet]
        public ActionResult AddMoneyToOA()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            DSR_CashManagementRepository ObjRepo = new DSR_CashManagementRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            DSROASearchEximTraderData obj = new DSROASearchEximTraderData();
            ObjRepo.OAGetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((DSROASearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((DSROASearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new DSROAAddMoney();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new DSROAReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            return PartialView("AddMoneyToOA", model);
        }

        [HttpGet]
        public ActionResult ListOfOAAddMoney()
        {
            var objRepo = new DSR_CashManagementRepository();
            objRepo.GetAllOAAddMoneyList();
            IEnumerable<DSROAAddMoney> lstAddMoneyToOA = (IEnumerable<DSROAAddMoney>)objRepo.DBResponse.Data;
            if (lstAddMoneyToOA != null)
            {
                return PartialView(lstAddMoneyToOA);
            }
            else
            {
                return PartialView(new List<DSROAAddMoney>());
            }
        }
        

               

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditOAAddMoney(DSROAAddMoney ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                DSR_CashManagementRepository ObjSDR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.OAGetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult OASearchByPartyCode(string PartyCode)
        {
            DSR_CashManagementRepository objRepo = new DSR_CashManagementRepository();
            objRepo.OASearchByPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        #endregion

        //[NonAction]
        //public string ConvertNumbertoWords(long number)
        //{
        //    if (number == 0) return "ZERO";
        //    if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
        //    string words = "";
        //    if ((number / 1000000) > 0)
        //    {
        //        words += ConvertNumbertoWords(number / 100000) + " LAKES ";
        //        number %= 1000000;
        //    }
        //    if ((number / 1000) > 0)
        //    {
        //        words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
        //        number %= 1000;
        //    }
        //    if ((number / 100) > 0)
        //    {
        //        words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
        //        number %= 100;
        //    }
        //    //if ((number / 10) > 0)  
        //    //{  
        //    // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
        //    // number %= 10;  
        //    //}  
        //    if (number > 0)
        //    {
        //        //if (words != "") words += "AND ";
        //        var unitsMap = new[]
        //        {
        //    "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        //};
        //        var tensMap = new[]
        //        {
        //    "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        //};
        //        if (number < 20) words += unitsMap[number];
        //        else
        //        {
        //            words += tensMap[number / 10];
        //            if ((number % 10) > 0) words += " " + unitsMap[number % 10];
        //        }
        //    }
        //    return words;
        //}

        #region Revenue Stamp Purchase
        public ActionResult RevenueStampPurchase()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetRevenueStampPurchaseList(string PurchaseDate=null,int Page=0)
        {
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            List<DSRRevenueStamp> lstRevenueStamp = new List<DSRRevenueStamp>();
            ObjCR.GetRevenueStampPurchase(PurchaseDate, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstRevenueStamp = (List<DSRRevenueStamp>)ObjCR.DBResponse.Data;
            }
            return PartialView("RevenueStampPurchaseList", lstRevenueStamp);
        }

        [HttpGet]
        public ActionResult GetLoadMoreRevenueStampPurchaseList(string PurchaseDate = null, int Page=0)
        {
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            List<DSRRevenueStamp> lstRevenueStamp = new List<DSRRevenueStamp>();
            ObjCR.GetRevenueStampPurchase(PurchaseDate, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstRevenueStamp = (List<DSRRevenueStamp>)ObjCR.DBResponse.Data;
            }
            return Json(lstRevenueStamp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditRevenueStampPurchase(DSRRevenueStamp ObjRevenueStamp)
        {
        
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
               
            Login ObjLogin = (Login)Session["LoginUser"];
            ObjRevenueStamp.Uid = ObjLogin.Uid;
            ObjCR.AddEditRevenueStampPurchase(ObjRevenueStamp);
           

            return Json(ObjCR.DBResponse);
           
        }

        [HttpGet]
        public ActionResult EditRevenueStampPurchase(int RSPurchaseId)
        {
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            DSRRevenueStamp ObjRevenueStamp = new DSRRevenueStamp();
            if (RSPurchaseId > 0)
            {
                ObjCR.GetRevenueStampPurchaseById(RSPurchaseId,0);
                if (ObjCR.DBResponse.Data != null)
                {                   
                    ObjRevenueStamp = (DSRRevenueStamp)ObjCR.DBResponse.Data;
                    ObjRevenueStamp.Operation = "Edit";                    
                }
            }
            return PartialView("RevenueStampPurchase", ObjRevenueStamp);
        }

        [HttpGet]
        public ActionResult ViewRevenueStampPurchase(int RSPurchaseId)
        {
            DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
            DSRRevenueStamp ObjRevenueStamp = new DSRRevenueStamp();
            if (RSPurchaseId > 0)
            {
                ObjCR.GetRevenueStampPurchaseById(RSPurchaseId,0);
                if (ObjCR.DBResponse.Data != null)
                {                    
                    ObjRevenueStamp = (DSRRevenueStamp)ObjCR.DBResponse.Data;
                    ObjRevenueStamp.Operation = "View";
                }
            }
            return PartialView("RevenueStampPurchase", ObjRevenueStamp);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteRevenueStampPurchase(int RSPurchaseId)
        {
            if (RSPurchaseId > 0)
            {
                DSR_CashManagementRepository ObjCR = new DSR_CashManagementRepository();
                ObjCR.DeleteRevenueStampPurchase(RSPurchaseId);
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
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
            DSR_CashManagementRepository objcancle = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objcancle = new DSR_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            DSR_CashManagementRepository objCR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objCR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objcancle = new DSR_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            DSR_CashManagementRepository objcancle = new DSR_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            DSR_CashManagementRepository objcancle = new DSR_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            DSR_CashManagementRepository objCancelInv = new DSR_CashManagementRepository();
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

            DSR_CashManagementRepository ObjIR = new DSR_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

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

            if(img!="")
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


       

        #region Direct Online Payment
        public ActionResult DirectOnlinePayment()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DirectPaymentVoucher(DSR_DirectOnlinePayment objDOP)
        {
            DSR_CashManagementRepository ObjIR = new DSR_CashManagementRepository();
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
            List<DSR_DirectOnlinePayment> lstDOP = new List<DSR_DirectOnlinePayment>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();

            obj.GetOnlinePayAckList(((Login)(Session["LoginUser"])).Uid, OrderId);
            lstDOP = (List<DSR_DirectOnlinePayment>)obj.DBResponse.Data;
            return PartialView(lstDOP);
        }

        [HttpPost]
        public JsonResult ConfirmPayment(DSR_DirectOnlinePayment vm)
        {
            vm.OrderId = Convert.ToInt64(Session["OrderId"].ToString());

            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.OnlinePaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetOnlinePaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptList(int Pages)
        {
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
            WFLD_CashManagementRepository objcancle = new WFLD_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            objcancle.ListOfPendingInvoice(Uid);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnlinePaymentAgainstInvoice(DSR_OnlinePaymentAgainstInvoice objDOP)
        {
            DSR_CashManagementRepository ObjIR = new DSR_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<DSR_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
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
            List<DSR_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<DSR_OnlinePaymentAgainstInvoice>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<DSR_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
            IList<DSR_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<DSR_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<DSR_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
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
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetOnlinePaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.OnlineOAPaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlineOAPaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetOnlineOAPaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlineOAPaymentReceiptList(int Pages)
        {
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
            List<DSR_AcknowledgementViewList> lstOPReceipt = new List<DSR_AcknowledgementViewList>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.AcknowledgementViewList(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<DSR_AcknowledgementViewList>)obj.DBResponse.Data;
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
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            DSR_CashManagementRepository obj = new DSR_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objIR = null;
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
                            DSR_CashManagementRepository objCash = new DSR_CashManagementRepository();
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
            DSR_CashManagementRepository objIR = new DSR_CashManagementRepository();
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
                        DSR_CashManagementRepository objCash = new DSR_CashManagementRepository();
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


      

    }
}