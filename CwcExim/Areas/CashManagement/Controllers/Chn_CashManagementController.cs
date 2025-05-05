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
using EinvoiceLibrary;
using System.Threading.Tasks;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class Chn_CashManagementController : BaseController
    {
        // GET: CashManagement/Chn_CashManagement

        #region-- ADD MONEY TO PD --

        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            var model = new Chn_AddMoneyToPDModel();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new Chn_ReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(Chn_AddMoneyToPDModel m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Chn_CashManagementRepository();
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
        public ActionResult ListAddToMoneyCashReceipt()
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            obj.GetAddToMoneyCashReceiptList(0);
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("ListAddToMoneyCashReceipt", lstCashReceipt);
        }
        [HttpGet]
        public JsonResult LoadMoreAddToMoneyList(int Page)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> LstJO = new List<Chn_CashReceiptModel>();
            obj.GetAddToMoneyCashReceiptList(Page);
            if (obj.DBResponse.Data != null)
            {
                LstJO = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAddToMoneyListSearch(string ReceiptNo)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            obj.GetAllAddToMoneyReceiptNo(ReceiptNo);
            if (obj.DBResponse.Data != null)
            {
                lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return PartialView("ListAddToMoneyCashReceipt", lstCashReceipt);
        }

        #endregion

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }


        public Chn_CashManagementController()
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
            Chn_CashReceiptModel ObjCashReceipt = new Chn_CashReceiptModel();

            var objRepo = new Chn_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetPartyList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
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
                objRepo.GetPartyList();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetails(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (Chn_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
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
        public ActionResult AddCashReceipt(Chn_CashReceiptModel ObjCashReceipt)
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.AddCashReceipt(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrint(int CashReceiptId)
        {
            var objRepo = new Chn_CashManagementRepository();
            var model = new PostPaymentSheet();
            objRepo.GetCashRcptPrint(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult CashReceiptList()
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            obj.GetCashReceiptList(0);
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("CashReceiptList", lstCashReceipt);
        }
        public JsonResult LoadMoreCashReceiptList(int Page)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> LstJO = new List<Chn_CashReceiptModel>();
            obj.GetCashReceiptList(Page);
            if (obj.DBResponse.Data != null)
            {
                LstJO = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCashReceiptListSearch(string ReceiptNo)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            obj.GetAllCashReceiptNo(ReceiptNo);
            if (obj.DBResponse.Data != null)
            {
                lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return PartialView("CashReceiptList", lstCashReceipt);
        }
        [HttpPost]
        public JsonResult UpdatePrintHtml(FormCollection fc)
        {
            int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
            string htmlPrint = fc["htmlPrint"].ToString();
            var objRepo = new Chn_CashManagementRepository();
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


        #endregion

        #region kol Cash Management Copy


        #region-- PAYMENT VOUCHER --

        [HttpGet]
        public ActionResult PaymentVoucher()
        {
            var objRepo = new Chn_CashManagementRepository();
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
        [ValidateAntiForgeryToken]
        public JsonResult PaymentVoucher(Chn_NewPaymentValucherModel m)
        {
            if (ModelState.IsValid)
            {
                var objRepo = new Chn_CashManagementRepository();
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetPaymentVoucherList();
            IEnumerable<Chn_NewPaymentValucherModel> lstPaymentVou = (IEnumerable<Chn_NewPaymentValucherModel>)objRepo.DBResponse.Data;
            if (lstPaymentVou != null)
            {
                return PartialView(lstPaymentVou);
            }
            else
            {
                return PartialView(new List<Chn_NewPaymentValucherModel>());
            }
        }

        #endregion

        #region Edit cash Receipt
        //[HttpGet]
        public ActionResult EditCashReceiptPaymentMode()//int InvoiceId = 0, string InvoiceNo = ""
        {
            Chn_CashReceiptModel ObjCashReceipt = new Chn_CashReceiptModel();
            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetReceiptList();
            List<Chn_EditReceiptPayment> lstEditReceiptPayment = new List<Chn_EditReceiptPayment>();

            if (objRepo.DBResponse.Data != null)
            {
                lstEditReceiptPayment = (List<Chn_EditReceiptPayment>)objRepo.DBResponse.Data;

                // ViewBag.lstReceiptPayment = lstEditReceiptPayment;
                ViewBag.lstReceiptPayment = JsonConvert.SerializeObject(((List<Chn_EditReceiptPayment>)objRepo.DBResponse.Data));
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
            //    ViewBag.Invoice = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
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
            //        ViewBag.Invoice = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).InvoiceDetail;
            //    else
            //        ViewBag.Invoice = null;
            //    objRepo.GetCashRcptDetails(InvoiceId, InvoiceNo);
            //    if (objRepo.DBResponse.Data != null)
            //    {
            //        ObjCashReceipt = (Chn_CashReceiptModel)objRepo.DBResponse.Data;
            //        // ViewBag.PayByDet =((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
            //        ViewBag.Pay = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
            //        ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
            //        ViewBag.Container = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
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
            var objRepo = new Chn_CashManagementRepository();
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

        public ActionResult SaveEditedCashReceipt(Chn_EditReceiptPayment objEditReceiptPayment)
        {

            var str = objEditReceiptPayment.receiptTableJson.Replace("--- Select ---", "");
            objEditReceiptPayment.CashReceiptDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Chn_CashReceiptEditDtls>>(str);
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.SaveEditedCashRcpt(objEditReceiptPayment, ((Login)(Session["LoginUser"])).Uid);

            return Json(objRepo.DBResponse);

        }
        #endregion

        #region-- RECEIVE VOUCHER --

        [HttpGet]
        public ActionResult ReceivedVoucher()
        {
            var objRepo = new Chn_CashManagementRepository();
            //objRepo.GetPaymentVoucherCreateInfo();
            ViewData["InvoiceNo"] = objRepo.ReceiptVoucherNo();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReceiptVoucher(Chn_ReceiptVoucherModel m)
        {
            if (ModelState.IsValid)
            {
                var objRepo = new Chn_CashManagementRepository();
                objRepo.AddNewReceiptVoucher(m);
                return Json(new { Status = true, Message = "Received Saved Successfully", Data = "CWC/RV/" + objRepo.DBResponse.Data.ToString().PadLeft(7, '0') + "/" + DateTime.Today.Year.ToString(), Id = objRepo.DBResponse.Data.ToString() }, JsonRequestBehavior.DenyGet);
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetReceiptVoucherList();
            IEnumerable<Chn_ReceiptVoucherModel> lstRcptVou = (IEnumerable<Chn_ReceiptVoucherModel>)objRepo.DBResponse.Data;
            if (lstRcptVou != null)
            {
                return PartialView(lstRcptVou);
            }
            else
            {
                return PartialView(new List<Chn_ReceiptVoucherModel>());
            }
        }

        #endregion

        #region Credit Note
        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(ChnCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt == 0)
                {
                    return Json(new { Status = -1, Message = "Zero value credit not can not be saved." });
                }
                else
                {
                    Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(int CRNoteId, string Note)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
            string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
            string tr = "";
            int Count = 1;
            decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
            objCR.lstCharges.ToList().ForEach(item =>
            {
                tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
                IGSTAmt += item.IGSTAmt;
                CGSTAmt += item.CGSTAmt;
                SGSTAmt += item.SGSTAmt;
                Count++;
            });
            string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
            html = html + htmltable + tr + tfoot;
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                RH.GeneratePDF(Path + "/" + fileName, html);
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
        public JsonResult AddCashColAgnBncCheque(Chn_CashColAgnBncChq objCashColAgnBncChq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
        public JsonResult AddDebitNote(ChnCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt == 0)
                {
                    return Json(new { Status = -1, Message = "Zero value debit note can not be saved." });
                }
                else
                {
                    Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }

        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
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

        #endregion

        #region Miscellaneous Invoice
        [HttpGet]
        public ActionResult CreateMiscInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Chn_CashManagementRepository ObjCash = new Chn_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc();
            if (ObjCash.DBResponse.Status > 0)
                ViewBag.PaymentParty =(List<PaymentPartyName>)ObjCash.DBResponse.Data;
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
            Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, InvoiceType, PartyId, Amount,SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<CHNMiscPostModel>(objForm["MiscInvModelJson"].ToString());
                Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
                objChargeMaster.AddMiscInv(invoiceData, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv");
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion


        //#region Refund From SD

        //[HttpGet]
        //public ActionResult RefundFromPDA()
        //{

        //    var objRepo = new Chn_CashManagementRepository();
        //    objRepo.GetPartyDetailsRefund();
        //    ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

        //    var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
        //    ViewBag.currentDate = currentDate;
        //    return PartialView();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult SaveRefundFromPDA(Chn_AddMoneyToPDModelRefund m)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
        //            var objRepo = new Chn_CashManagementRepository();
        //            objRepo.RefundFromPDA(m, ((Login)(Session["LoginUser"])).Uid);
        //            return Json(objRepo.DBResponse, JsonRequestBehavior.DenyGet);
        //        }
        //        else
        //        {
        //            string m1 = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
        //            var Err = new { Statua = -1, Messgae = "Error" };
        //            return Json(Err, JsonRequestBehavior.DenyGet);
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Status = 0, Message = "Some error occurs !!" }, JsonRequestBehavior.DenyGet);
        //    }
        //}

        //[HttpPost, ValidateInput(false)]
        //public JsonResult GeneratePDF2(FormCollection fc)
        //{


        //    try
        //    {
        //        var pages = new string[1];
        //        pages[0] = fc["page"].ToString();
        //        var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
        //        string PdfDirectory = Server.MapPath("~/Docs") + "/PdaRefundReceipt/";
        //        if (!Directory.Exists(PdfDirectory))
        //            Directory.CreateDirectory(PdfDirectory);
        //        //using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //        //{
        //        //    rh.GeneratePDF(PdfDirectory + fileName, pages);
        //        //}
        //        CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
        //        Chn_CashManagementRepository ObjRR = new Chn_CashManagementRepository();
        //        ObjRR.getCompanyDetails();
        //        objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
        //        HeadOffice = ""; //objCompanyDetails.CompanyName;
        //        HOAddress = "";//objCompanyDetails.RoAddress;
        //        ZonalOffice = objCompanyDetails.CompanyName;
        //        ZOAddress = objCompanyDetails.CompanyAddress;

        //        using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
        //        {
        //            ObjPdf.HeadOffice = this.HeadOffice;
        //            ObjPdf.HOAddress = this.HOAddress;
        //            ObjPdf.ZonalOffice = this.ZonalOffice;
        //            ObjPdf.ZOAddress = this.ZOAddress;
        //            ObjPdf.GeneratePDF(PdfDirectory + fileName, pages);

        //        }

        //        return Json(new { Status = 1, Data = fileName }, JsonRequestBehavior.DenyGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
        //    }

        //}


        //#endregion

        

        #region Yard Invoice Edit
        [HttpGet]
        public ActionResult EditYardInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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
                Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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
                Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();
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





        //public JsonResult AddEditDeliPaymentSheet(PPGInvoiceGodown objForm)
        //{
        //    try
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"]);

        //        var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
        //        string ContainerXML = "";
        //        string ChargesXML = "";
        //        string ContWiseCharg = "";
        //        string OperationCfsCodeWiseAmtXML = "";
        //        String CargoXML = "";
        //        foreach (var item in invoiceData.lstPostPaymentCont)
        //        {
        //            item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
        //            item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
        //            item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
        //        }

        //        if (invoiceData.lstPostPaymentCont != null)
        //        {
        //            ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
        //        }
        //        if (invoiceData.lstPostPaymentChrg != null)
        //        {
        //            ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
        //        }
        //        if (invoiceData.lstContWiseAmount != null)
        //        {
        //            ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
        //        }
        //        if (invoiceData.lstOperationCFSCodeWiseAmount != null)
        //        {
        //            OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
        //        }
        //        if (invoiceData.lstInvoiceCargo != null)
        //        {
        //            CargoXML = Utility.CreateXML(invoiceData.lstInvoiceCargo);
        //        }
        //        Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
        //        objChargeMaster.AddEditInvoiceGodown(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);

        //        invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


        //        objChargeMaster.DBResponse.Data = invoiceData;
        //        return Json(objChargeMaster.DBResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Err = new { Status = -1, Message = "Error" };
        //        return Json(Err);
        //    }
        //}











        #endregion

        #region Loaded Container Payment Sheet Edit

        [HttpGet]
        public ActionResult EditLoadedContainerInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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



        //public JsonResult AddEditContPaymentSheet(PpgInvoiceYard objForm)
        //{
        //    try
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"]);

        //        var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
        //        string ContainerXML = "";
        //        string ChargesXML = "";
        //        string ContWiseCharg = "";
        //        string OperationCfsCodeWiseAmtXML = "";

        //        foreach (var item in invoiceData.lstPostPaymentCont)
        //        {
        //            item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
        //            item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
        //            item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
        //        }

        //        if (invoiceData.lstPostPaymentCont != null)
        //        {
        //            ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
        //        }
        //        if (invoiceData.lstPostPaymentChrg != null)
        //        {
        //            ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
        //        }
        //        if (invoiceData.lstContWiseAmount != null)
        //        {
        //            ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
        //        }
        //        if (invoiceData.lstOperationCFSCodeWiseAmount != null)
        //        {
        //            OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
        //        }

        //        Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
        //        objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, 0, "EXPLod");

        //        invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


        //        objChargeMaster.DBResponse.Data = invoiceData;
        //        return Json(objChargeMaster.DBResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Err = new { Status = -1, Message = "Error" };
        //        return Json(Err);
        //    }
        //}
        #endregion


        #region Edit Container Movement

        [HttpGet]
        public ActionResult EditContainerMovementInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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
                Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
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
                Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();
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


        #region Export Invoice Edit
        [HttpGet]
        public ActionResult EditExportInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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
                Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();

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
                Chn_CashManagementRepository objCashManagement = new Chn_CashManagementRepository();
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


        #region Cancel Receipt
        [HttpGet]
        public ActionResult GetCancelReceipt()
        {
            Chn_CashManagementRepository objRR = new Chn_CashManagementRepository();
            objRR.ListOfReceiptForCancel();
            if (objRR.DBResponse.Data != null)
                ViewBag.ReceiptList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(objRR.DBResponse.Data));
            else ViewBag.ReceiptList = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ListOfCancelledReceipt()
        {
            Chn_CashManagementRepository objRR = new Chn_CashManagementRepository();
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
                Chn_CashManagementRepository objRR = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRR = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
            objChargeMaster.GetFumigation(FumigationChargeType, InvoiceType, PartyId, size, XMLText);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFumigationInvoice(Chn_FumigationInvoice FumigationModel)
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
                    Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository repo = new Chn_CashManagementRepository();
            repo.GetContainersForFumigation();
            return Json(repo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Rent Invoice

        [HttpGet]
        public ActionResult CreateRentInvoice()
        {
            Chn_CashManagementRepository objCash = new Chn_CashManagementRepository();

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

                if (invoiceData.lstPrePaymentCont != null)
                {
                    RentDetailsXML = Utility.CreateXML(invoiceData.lstPrePaymentCont);
                }
                Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
                objChargeMaster.AddEditRentInv(invoiceData, RentDetailsXML, BranchId, ((Login)(Session["LoginUser"])).Uid, Month, Year, "Rent");

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
        public JsonResult GetMonthYearRentData(string Month, int Year, int Flag)
        {

            Chn_CashManagementRepository objPpgRepo = new Chn_CashManagementRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetRentDet(Month, Year, Flag);
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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
                Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();

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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;
            return PartialView();
        }

        public ActionResult ListChequeDeposit()
        {
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
            objChe.GetChequeDepositsAll();
            List<ChequeDepositList> lst = new List<ChequeDepositList>();
            if (objChe.DBResponse.Data != null)
                lst = (List<ChequeDepositList>)objChe.DBResponse.Data;
            return PartialView(lst);
        }
        public ActionResult EditChequeDeposit(int Id)
        {
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
                Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();

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
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
                Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();

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

        public ActionResult Chn_CashDepositToBank()
        {
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
            objChe.GetMstBank();
            if (objChe.DBResponse.Status > 0)
                ViewBag.Banks = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.Banks = null;

            Chn_CashDepositToBank objChn_CashDepositToBank = new Chn_CashDepositToBank();
            objChn_CashDepositToBank.DepositDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            objChe.GetCashDepositBalance(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            if (objChe.DBResponse.Status > 0)
                objChn_CashDepositToBank.BalanceAmount = Convert.ToDecimal(objChe.DBResponse.Data);

            return PartialView(objChn_CashDepositToBank);
        }

        [HttpGet]
        public JsonResult GetCashDepositBalance(string TransactionDate)
        {
            try
            {
                decimal BalanceAmount = 0;
                Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
        public JsonResult SaveChn_CashDepositToBank(Chn_CashDepositToBank obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Chn_CashManagementRepository objER = new Chn_CashManagementRepository();

                    objER.SaveChn_CashDepositToBank(obj);
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyList();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfParty = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPartyWiseSDBalance(int PartyId, string BalanceDate)
        {
            try
            {
                decimal BalanceAmount = 0;
                Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objCash = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objCR = new Chn_CashManagementRepository();
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
            //Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
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
        //    Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyList("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAmount(int ChargeId, String ChargeName, String Size, string FromDate, string ToDate)
        {
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
            objcash.ListOfChargesName();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<Charge>)objcash.DBResponse.Data).ToList().ForEach(item => {
                    objImp2.Add(new { ChargeId = item.ChargeId, ChargeName = item.ChargeName });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetDebitInvoice(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges)
        {

            Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
            objChargeMaster.GetDebitInvoice(InvoiceType, PartyId, TotalChrgAmount, Charges);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDebitInvoice(Chn_DebitInvoice DebitModel)
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
                    Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
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
            Chn_CashReceiptModel ObjCashReceipt = new Chn_CashReceiptModel();

            var objRepo = new Chn_CashManagementRepository();
            if (PartyId == 0)
            {
                objRepo.GetPartyListSD();
                if (objRepo.DBResponse.Data != null)
                    ViewBag.Party = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
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
                    ViewBag.Party = ((Chn_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                else
                    ViewBag.Invoice = null;
                objRepo.GetCashRcptDetailsSD(PartyId, PartyName, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (Chn_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((Chn_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
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
        public ActionResult AddCashReceiptSD(Chn_CashReceiptModel ObjCashReceipt)
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
            var objRepo = new Chn_CashManagementRepository();
            objRepo.AddCashReceiptSD(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrintSD(int CashReceiptId)
        {
            var objRepo = new Chn_CashManagementRepository();
            var model = new PostPaymentSheet();
            objRepo.GetCashRcptPrintSD(CashReceiptId);
            if (objRepo.DBResponse.Data != null)
            {
                model = (PostPaymentSheet)objRepo.DBResponse.Data;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListCashReceiptSD()
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            obj.GetCashReceiptSDList(0);
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            return PartialView("ListCashReceiptSD", lstCashReceipt);
        }
        [HttpGet]
        public JsonResult LoadMoreCashList(int Page)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> LstJO = new List<Chn_CashReceiptModel>();
            obj.GetCashReceiptSDList(Page);
            if (obj.DBResponse.Data != null)
            {
                LstJO = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCashReceiptSDListSearch(string ReceiptNo)
        {
            Chn_CashManagementRepository obj = new Chn_CashManagementRepository();
            List<Chn_CashReceiptModel> lstCashReceipt = new List<Chn_CashReceiptModel>();
            obj.GetAllReceiptNo(ReceiptNo);
            if (obj.DBResponse.Data != null)
            {
                lstCashReceipt = (List<Chn_CashReceiptModel>)obj.DBResponse.Data;
            }
            return PartialView("ListCashReceiptSD", lstCashReceipt);
        }
        [HttpPost]
        public JsonResult UpdatePrintHtmlSD(FormCollection fc)
        {
            int CashReceiptId = Convert.ToInt32(fc["CashReceiptId"].ToString());
            string htmlPrint = fc["htmlPrint"].ToString();
            var objRepo = new Chn_CashManagementRepository();
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

        #region Refund From SD

        [HttpGet]
        public ActionResult RefundFromPDA()
        {

            var objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            return PartialView();
        }

        [HttpGet]
        public ActionResult SDRefundList()
        {
            List<ChnSDRefundList> ObjSd = new List<ChnSDRefundList>();
            Chn_CashManagementRepository ObjCR = new Chn_CashManagementRepository();
            ObjCR.GetSDRefundList();
            if (ObjCR.DBResponse.Data != null)
                ObjSd = (List<ChnSDRefundList>)ObjCR.DBResponse.Data;
            return PartialView(ObjSd);
        }
         
        [HttpGet]
        public ActionResult ViewSDRefund(int PdaAcId)
        {
            ChnViewSDRefund ObjSD = new ChnViewSDRefund();
            Chn_CashManagementRepository objCR = new Chn_CashManagementRepository();
            objCR.ViewSDRefund(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (ChnViewSDRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(ChnAddMoneyToPDModelRefund m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //  var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Chn_CashManagementRepository();
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

        #region Cancel Invoice


        [HttpGet]
        public ActionResult CancelInvoice()
        {
            CancelInvoice cin = new CancelInvoice();
            var InvoiceNo = "";
            Chn_CashManagementRepository objcancle = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objcancle = new Chn_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Chn_CashManagementRepository objCR = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objCR = new Chn_CashManagementRepository();
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
            Chn_CashManagementRepository objcancle = new Chn_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Chn_CashManagementRepository objcancle = new Chn_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Chn_CashManagementRepository objcancle = new Chn_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Chn_CashManagementRepository objCancelInv = new Chn_CashManagementRepository();
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

            Chn_CashManagementRepository ObjIR = new Chn_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

        }


        #endregion


        #region Reservation Invoice (Individual)

        [HttpGet]
        public ActionResult CreateReservationInvIndividual()
        {




            return PartialView();
        }


        [HttpGet]
        public JsonResult ListOfGodown(string SpaceType)
        {
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
            objcash.GetGodownListforReservation(SpaceType);
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<CHN_Godown>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { GodownId = item.GodownId, GodownName = item.GodownName, OperationType = item.OperationType });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult LoadPartyForReservation(string PartyCode, int Page, int GodownId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyListforReservation("", GodownId, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCodeReservation(string PartyCode, int GodownId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPartyListforReservation(PartyCode, GodownId, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeReservation(string PartyCode, int Page, int GodownId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPayeeListReservation("", Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPayeeCodeReservation(string PartyCode, int GodownId)
        {
            Chn_CashManagementRepository objRepo = new Chn_CashManagementRepository();
            objRepo.GetPayeeListReservation(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAmountReservation(int ChargeId, String ChargeName, String Size, string FromDate, string ToDate)
        {
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
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
        public JsonResult ListOfChargeReservation()
        {
            Chn_CashManagementRepository objcash = new Chn_CashManagementRepository();
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
        [ValidateAntiForgeryToken]
        public JsonResult AddEditRevInvoiceIndividual(CHN_ReservationIndividual DebitModel)
        {

            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                IList<CHN_charges> ReservationCharges = JsonConvert.DeserializeObject<IList<CHN_charges>>(DebitModel.ReservationChargesXML);


                string dtlslXml = Utility.CreateXML(ReservationCharges);
                Chn_CashManagementRepository objChargeMaster = new Chn_CashManagementRepository();
                objChargeMaster.AddEditInvoiceReservationIndividual(DebitModel, dtlslXml, BranchId, ((Login)(Session["LoginUser"])).Uid, "RESINDIV");


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






        [HttpGet]

        public JsonResult CreateReservationInvoicesIndividual(string DeliveryDate, string SpaceType, int Godown_Id, string Godown_Name, string Godown_Type, string From_Date, string To_Date, decimal SQM, int PartyId, string CargoType, string SEZ, int longTerm = 0)
        {
            Chn_CashManagementRepository objChe = new Chn_CashManagementRepository();
            objChe.CreateReservationInvoicesIndividual(DeliveryDate, SpaceType, Godown_Id, Godown_Name, Godown_Type, From_Date, To_Date, SQM, PartyId, CargoType, SEZ, longTerm);
            return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion


        [HttpPost]
        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Chn_CashManagementRepository objPpgRepo = new Chn_CashManagementRepository();

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

                Import.Models.CHN_IrnB2CDetails q1 = new Import.Models.CHN_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDR(CrNoteNo, Type, CRDR);
                var irnb2cobj = (Import.Models.CHN_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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

                        Import.Models.CHN_IrnB2CDetails q1 = new Import.Models.CHN_IrnB2CDetails();
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
    }
}