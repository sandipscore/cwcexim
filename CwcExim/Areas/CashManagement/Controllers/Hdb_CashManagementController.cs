using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Bond.Models;
using CwcExim.DAL;
using CwcExim.Areas.Report.Models;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System.Text;

using System.IO;
using EinvoiceLibrary;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CCA.Util;
using System.Collections.Specialized;
using System.Net;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class Hdb_CashManagementController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: CashManagement/Hdb_CashManagement
        #region ---- Payment Receipt/Cash Receipt ----
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }


        public Hdb_CashManagementController()
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
        [HttpGet]
        public ActionResult CashReceipt(int PartyId = 0, string PartyName = "",string Tan="", string Type = "INVOICE")
        {
            Hdb_CashReceiptModel ObjCashReceipt = new Hdb_CashReceiptModel();

            var objRepo = new Hdb_CashManagementRepository();
            if (PartyId == 0)
            {
                /* objRepo.GetPartyList();
                 if (objRepo.DBResponse.Data != null)
                     ViewBag.Party = ((Hdb_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                 else
                     ViewBag.Invoice = null;
                 */
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                objExport.GetPaymentPartyForPage("", 0);
                if (objExport.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstParty = Jobject["lstParty"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.lstParty = null;
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
                new SelectListItem { Text = "SECURITY DEPOSIT", Value = "SECURITYDEPOSIT"},
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
                /*  objRepo.GetPartyList();
                  if (objRepo.DBResponse.Data != null)
                      ViewBag.Party = ((Hdb_CashReceiptModel)objRepo.DBResponse.Data).PartyDetail;
                  else
                      ViewBag.Invoice = null;
                      */
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                objExport.GetPaymentPartyForPage("", 0);
                if (objExport.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstParty = Jobject["lstParty"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.lstParty = null;
                }
                objRepo.GetCashRcptDetails(PartyId, PartyName, Tan, Type);
                if (objRepo.DBResponse.Data != null)
                {
                    ObjCashReceipt = (Hdb_CashReceiptModel)objRepo.DBResponse.Data;
                    // ViewBag.PayByDet =((Kol_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail;
                    ViewBag.Pay = JsonConvert.SerializeObject(((Hdb_CashReceiptModel)objRepo.DBResponse.Data).PayByDetail);
                    ViewBag.PdaAdjust = JsonConvert.SerializeObject(((Hdb_CashReceiptModel)objRepo.DBResponse.Data).PdaAdjustdetail);
                    ViewBag.Container = JsonConvert.SerializeObject(((Hdb_CashReceiptModel)objRepo.DBResponse.Data).ContainerDetail);
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
                new SelectListItem { Text = "SECURITY DEPOSIT", Value = "SECURITYDEPOSIT"},
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



                return PartialView(ObjCashReceipt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashReceipt(Hdb_CashReceiptModel ObjCashReceipt)
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
            var objRepo = new Hdb_CashManagementRepository();
            objRepo.AddCashReceipt(ObjCashReceipt, xml);
            return Json(objRepo.DBResponse);
        }

        [HttpGet]
        public JsonResult CashReceiptPrint(int CashReceiptId)
        {
            var objRepo = new Hdb_CashManagementRepository();
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
            var objRepo = new Hdb_CashManagementRepository();
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

        #region Delivery Payment Sheet Edit

        [HttpGet]
        public ActionResult EditDeliveryInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryPaymentSheet(FormCollection objForm)
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

                //ImportRepository objImport = new ImportRepository();
                //objImport.AddEditContainerInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objImport.DBResponse);

                var invoiceData = JsonConvert.DeserializeObject<CwcExim.Areas.Import.Models.Hdb_PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CfsWiseCharg = "";
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
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    CfsWiseCharg = Utility.CreateXML(result);

                    //  CfsWiseCharg = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }

                HDB_ImportRepository objChargeMaster = new HDB_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");

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
        public JsonResult GetDeliveryInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();
                objCharge.GetAllCharges();
                Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();
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

        #region Yard Invoice Edit
        [HttpGet]
        public ActionResult EditYardInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(FormCollection objForm)
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

                //ImportRepository objImport = new ImportRepository();
                //objImport.AddEditContainerInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                //return Json(objImport.DBResponse);

                var invoiceData = JsonConvert.DeserializeObject<CwcExim.Areas.Import.Models.Hdb_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
                // var invoiceData = objForm;
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
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                HDB_ImportRepository objChargeMaster = new HDB_ImportRepository();
                objChargeMaster.AddEditContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPYard");
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
        public JsonResult GetYardInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                objCharge.GetAllCharges();
                Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();
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


        #region Load Container Payment Sheet Edit
        [HttpGet]
        public ActionResult EditLoadContainerInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

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
                Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadedContPaymentSheet(FormCollection objForm)
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

                var invoiceData = JsonConvert.DeserializeObject<Hdb_LoadedContainerPayment>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CfsWiseCharg = "";
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
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    CfsWiseCharg = Utility.CreateXML(result);

                    // CfsWiseCharg = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }

                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg,"", BranchId, ((Login)(Session["LoginUser"])).Uid,"", "EXPLod");

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

        #region BTT Invoice Edit
        [HttpGet]
        public ActionResult EditBTTInvoice()
        {
            ImportRepository objImport = new ImportRepository();
            Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();

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
                Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBTTPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<HDBInvoiceBTT>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
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
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditBTTInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", CargoXML);
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
                Kdl_CashManagementRepository objCashManagement = new Kdl_CashManagementRepository();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPaymentSheet(FormCollection objForm)
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

                var invoiceData = JsonConvert.DeserializeObject<HDBInvoiceExp>(objForm["PaymentSheetModelJson"].ToString());
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
                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditExpInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML,"", BranchId, ((Login)(Session["LoginUser"])).Uid);
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

        #region-- ADD MONEY TO PD --

        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            var model = new AddMoneyToPDModelHdb();
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
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            var objRepo = new Hdb_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            ViewBag.ServerDate = Utility.GetServerDate();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(AddMoneyToPDModelHdb m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new Hdb_CashManagementRepository();
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

        [HttpGet]
        public ActionResult ListOfPDReceipt()
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            List<ListOfPDReceipt> lstNote = new List<ListOfPDReceipt>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfPDReceipt>)objRepo.DBResponse.Data;
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
        #endregion

        #region Miscellaneous Invoice
        [HttpGet]
        public ActionResult CreateMiscInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            //Hdb_CashManagementRepository objCR = new Hdb_CashManagementRepository();
            //objCR.GetPaymentPartyForMiscInvoice();
            //if (objCR.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objCR.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPartyForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objExport.GetPaymentPayerForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPayer = Jobject["lstPayer"];
                ViewBag.StatePayer = Jobject["StatePayer"];
            }
            else
            {
                ViewBag.lstPayer = null;
            }

            /*objCR.PurposeListForMiscInvc();
            if (objCR.DBResponse.Data != null)
            {*/
            List<SelectListItem> lstItem = new List<SelectListItem>();
            lstItem.Add(new SelectListItem { Text ="Printing",Value= "Printing" });
            lstItem.Add(new SelectListItem { Text = "Banking", Value = "Banking" });
            lstItem.Add(new SelectListItem { Text = "Photocopy", Value = "Photocopy" });
            lstItem.Add(new SelectListItem { Text = "ChequeReturn", Value = "ChequeReturn" });
            lstItem.Add(new SelectListItem { Text = "Pest Control", Value = "PCS" });
            lstItem.Add(new SelectListItem { Text = "LWB Charges", Value = "LWB" });
            lstItem.Add(new SelectListItem { Text = "Ground Rent Empty", Value = "GRE" });
            lstItem.Add(new SelectListItem { Text = "Ground Rent Loaded", Value = "GRL" });
            lstItem.Add(new SelectListItem { Text = "Table Space", Value = "TableSpace" });
            lstItem.Add(new SelectListItem { Text = "Palletization/Packing Charge", Value = "Packing" });
            lstItem.Add(new SelectListItem { Text = "Labeling(permission) Charge", Value = "Labeling" });
            lstItem.Add(new SelectListItem { Text = "Dishonoured Cheque Penalty", Value = "Penalty" });
            lstItem.Add(new SelectListItem { Text = "IGM Filing", Value = "IGMFiling" });
            lstItem.Add(new SelectListItem { Text = "BE/SB Filing", Value = "SBFiling" });            
            lstItem.Add(new SelectListItem { Text = "Survey Fee", Value = "Survey" });

            lstItem.Add(new SelectListItem { Text = "A-1", Value = "A-1" });
            lstItem.Add(new SelectListItem { Text = "A-2", Value = "A-2" });
            lstItem.Add(new SelectListItem { Text = "A-3", Value = "A-3" });
            lstItem.Add(new SelectListItem { Text = "A-4", Value = "A-4" });
            lstItem.Add(new SelectListItem { Text = "A-5", Value = "A-5" });
            lstItem.Add(new SelectListItem { Text = "A-6a", Value = "A-6a" });
            lstItem.Add(new SelectListItem { Text = "A-7", Value = "A-7" });
            lstItem.Add(new SelectListItem { Text = "B-8", Value = "B-8" });
            lstItem.Add(new SelectListItem { Text = "B-9", Value = "B-9" });
            lstItem.Add(new SelectListItem { Text = "B-10", Value = "B-10" });
            lstItem.Add(new SelectListItem { Text = "B-11", Value = "B-11" });
            lstItem.Add(new SelectListItem { Text = "B-12", Value = "B-12" });
            lstItem.Add(new SelectListItem { Text = "C-13", Value = "C-13" });
            lstItem.Add(new SelectListItem { Text = "C-14A", Value = "C-14A" });
            lstItem.Add(new SelectListItem { Text = "C-14B", Value = "C-14B" });
            lstItem.Add(new SelectListItem { Text = "C-14C", Value = "C-14C" });
            lstItem.Add(new SelectListItem { Text = "C-15", Value = "C-15" });
            lstItem.Add(new SelectListItem { Text = "C-16A", Value = "C-16A" });
            lstItem.Add(new SelectListItem { Text = "C-16B", Value = "C-16B" });
            lstItem.Add(new SelectListItem { Text = "C-16C", Value = "C-16C" });
            lstItem.Add(new SelectListItem { Text = "C-17a", Value = "C-17a" });
            lstItem.Add(new SelectListItem { Text = "C-17-i", Value = "C-17-i" });
            lstItem.Add(new SelectListItem { Text = "C-17-ii", Value = "C-17-ii" });
            lstItem.Add(new SelectListItem { Text = "C-17-iii", Value = "C-17-iii" });
            lstItem.Add(new SelectListItem { Text = "C-17-iv", Value = "C-17-iv" });
            lstItem.Add(new SelectListItem { Text = "C-17-v", Value = "C-17-v" });
            lstItem.Add(new SelectListItem { Text = "C-17-vi", Value = "C-17-vi" });
            lstItem.Add(new SelectListItem { Text = "C-17-vii", Value = "C-17-vii" });
            lstItem.Add(new SelectListItem { Text = "C-17-viii", Value = "C-17-viii" });
            lstItem.Add(new SelectListItem { Text = "C-17-1", Value = "C-17-1" });
            lstItem.Add(new SelectListItem { Text = "C-17-2", Value = "C-17-2" });
            lstItem.Add(new SelectListItem { Text = "C-18", Value = "C-18" });
            lstItem.Add(new SelectListItem { Text = "C-19", Value = "C-19" });
            lstItem.Add(new SelectListItem { Text = "B9", Value = "B9" });
            lstItem.Add(new SelectListItem { Text = "A-6(b)-(i)", Value = "A-6(b)-(i)" });
            lstItem.Add(new SelectListItem { Text = "A-6(b)-(ii)", Value = "A-6(b)-(ii)" });
            lstItem.Add(new SelectListItem { Text = "A-6b", Value = "A-6b" });


            ViewBag.PurposeList =(List<SelectListItem>) lstItem /*(List<SelectListItem>)objCR.DBResponse.Data*/;
            /*}*/
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetMiscInvoiceAmount(string purpose, string InvoiceType, int PartyId, decimal Amount,string ExportUnder)
        {
            Hdb_CashManagementRepository objChargeMaster = new Hdb_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, InvoiceType, PartyId, Amount, ExportUnder);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMiscInvoice(FormCollection objForm/*, string GatePass = ""*/)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<Hdb_MiscInvModel>(objForm["MiscInvModelJson"].ToString());
                Hdb_CashManagementRepository objChargeMaster = new Hdb_CashManagementRepository();
                objChargeMaster.AddMiscInv(invoiceData, BranchId, ((Login)(Session["LoginUser"])).Uid, "MiscInv"/*,GatePass*/);

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

        #region Credit Note

        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Hdb_CashManagementRepository objPpgRepo = new Hdb_CashManagementRepository();



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

                Import.Models.Hdb_IrnB2CDetails q1 = new Import.Models.Hdb_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                var irnb2cobj = (Import.Models.Hdb_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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
                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
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

                        Import.Models.Hdb_IrnB2CDetails q1 = new Import.Models.Hdb_IrnB2CDetails();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                        var irnb2cobj = (Import.Models.Hdb_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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
                            idata.mc = irnb2cobj.mc; ;
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
        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            /*objRepo.GetInvoiceNoForCreaditNote("C");
            if (objRepo.DBResponse.Data != null)
                ViewBag.InvoiceNo = objRepo.DBResponse.Data;
            else
                ViewBag.InvoiceNo = null;*/
           /* objRepo.GetInvoiceNoForCreaditNote("C");
            objRepo.GetInvoiceNoForCreaditNote("", 0);
            if (objRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstStuffing = Jobject["LstStuffing"];
                ViewBag.StateCont = Jobject["State"];
            }
            else
            {
                ViewBag.LstStuffing = null;
            }*/
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
        public JsonResult GetInvoiceDetailsForCreaditNote(int InvoiceId,string InvoiceType)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId, InvoiceType);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
       [HttpGet]
        public JsonResult GetInvoiceNoForDebitCreditNote(String CRDR)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetInvoiceNoForCreaditNote(CRDR);
            /* if (objRepo.DBResponse.Data != null)
                 return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
             else
                 return Json(new { Status = -1, Message = "Error" });*/
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        /* [HttpGet]
          public JsonResult GetInvoiceNoForDebitCreditNote(String CRDR)
          {
              Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
              objRepo.GetInvoiceNoForCreaditNote(CRDR);
              if (objRepo.DBResponse.Data != null)
                   return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
               else
                   return Json(new { Status = -1, Message = "Error" });
              var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
              jsonResult.MaxJsonLength = int.MaxValue;
              return jsonResult;
          }*/
        [HttpGet]
        public JsonResult SearchInvoice(string invoiceno)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetAllInvoicenoforcreditnote(invoiceno, 0);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadInvoiceLists(string invoiceno, int Page)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetAllInvoicenoforcreditnote(invoiceno, Page);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(PpgCreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt == 0)
                {
                    return Json(new { Status = -1, Message = "Zero value credit not can not be saved." });
                }
                else
                {
                    Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
        public JsonResult GenerateCreditNoteExistingReport(String Id,String CRDR)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
           
            objRepo.generateCreditNoteExisting(Id, CRDR);
           
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ListOfCRNote()
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");

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
            html = html.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(html.ToString());
            using (var RH = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f))
            {
                RH.GeneratePDF(Path + "/" + fileName, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + fileName;
        }
        //public string GenerateCRNotePDF(PrintModelOfCr objCR, int CRNoteId, string Note)
        //{
        //    string SACCode = "", note = "", fileName = "";
        //    objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
        //    {
        //        if (SACCode == "")
        //            SACCode = item.SACCode;
        //        else
        //            SACCode = SACCode + "," + item.SACCode;
        //    });
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
        //    string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>HSN/SAC Code</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
        //    string tr = "";
        //    int Count = 1;
        //    decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
        //    objCR.lstCharges.ToList().ForEach(item =>
        //    {
        //        tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SACCode + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
        //        IGSTAmt += item.IGSTAmt;
        //        CGSTAmt += item.CGSTAmt;
        //        SGSTAmt += item.SGSTAmt;
        //        Count++;
        //    });
        //    string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
        //    string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr>         <tr><td colspan='10' style='border:1px solid #000;border-right:0;padding:5px;font-size:7pt;'><span style='font-weight:600;text-align:left;'>Round Up</span></td><td colspan='1' style='border:1px solid #000;border-left:0;padding:5px;font-size:7pt;text-align:right;'>" + objCR.RoundUp + "</td></tr><tr><td colspan='11' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='11' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
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

        #region Debit Note
        [HttpGet]
        public ActionResult CreateDebitNote()
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
                if (objCR.TotalAmt == 0)
                {
                    return Json(new { Status = -1, Message = "Zero value debit note can not be saved." });
                }
                else
                {
                    Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }

        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            Hdb_CashManagementRepository objRepo = new Hdb_CashManagementRepository();
            objRepo.GetChargesListForCrDb(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        #endregion

        #region Container Debit Invoice

        [HttpGet]
        public ActionResult CreateDebitInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetPaymentPartyForImportInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }


        [HttpGet]
        public JsonResult ListOfContainer()
        {
            Hdb_CashManagementRepository objcash = new Hdb_CashManagementRepository();
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


        [HttpGet]
        public JsonResult GetAmount(int ChargeId, String ChargeName, String Size, string FromDate, string ToDate)
        {
            Hdb_CashManagementRepository objcash = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objcash = new Hdb_CashManagementRepository();
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

        public JsonResult GetDebitInvoice(string InvoiceType, int PartyId, decimal TotalChrgAmount, string Charges)
        {

            Hdb_CashManagementRepository objChargeMaster = new Hdb_CashManagementRepository();
            objChargeMaster.GetDebitInvoice(InvoiceType, PartyId, TotalChrgAmount, Charges);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDebitInvoice(Hdb_DebitInvoice DebitModel)
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
                    Hdb_CashManagementRepository objChargeMaster = new Hdb_CashManagementRepository();
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

        #region Refund From SD

        [HttpGet]
        public ActionResult RefundFromPDA()
        {
            var PaymentMode = new SelectList(new[]
          {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                new SelectListItem { Text = "IMPS", Value = "IMPS"},
                 new SelectListItem { Text = "RTGS", Value = "RTGS"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;

            var objRepo = new Hdb_CashManagementRepository();
            objRepo.GetPartyDetailsRefund();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);

            var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.currentDate = currentDate;
            ViewBag.ServerDate = Utility.GetServerDate(); 
            HDB_AddMoneyToPDModelRefund addMoneyToPDModelRefund = new  HDB_AddMoneyToPDModelRefund();
            return PartialView();
        }

        [HttpGet]
        public ActionResult SDRefundList()
        {
            List<HDB_SDRefundList> ObjSd = new List<HDB_SDRefundList>();
            Hdb_CashManagementRepository ObjCR = new Hdb_CashManagementRepository();
            ObjCR.GetSDRefundList();
            if (ObjCR.DBResponse.Data != null)
                ObjSd = (List<HDB_SDRefundList>)ObjCR.DBResponse.Data;
            return PartialView(ObjSd);
        }

        [HttpGet]
        public ActionResult ViewSDRefund(int PdaAcId)
        {
            HDB_AddMoneyToPDModelRefund ObjSD = new HDB_AddMoneyToPDModelRefund();
            Hdb_CashManagementRepository objCR = new Hdb_CashManagementRepository();
            objCR.ViewSDRefund(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (HDB_AddMoneyToPDModelRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveRefundFromPDA(HDB_AddMoneyToPDModelRefund m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in m.Details.Where(o => o.Amount > 0).ToList())
                    {
                        item.Type = String.IsNullOrWhiteSpace(item.Type) ? "###" : item.Type;
                        item.Bank = String.IsNullOrWhiteSpace(item.Bank) ? "###" : item.Bank;
                        item.InstrumentNo = String.IsNullOrWhiteSpace(item.InstrumentNo) ? "###" : item.InstrumentNo;

                        if (String.IsNullOrWhiteSpace(item.Date))
                        {
                            item.Date = "null";
                        }
                        else
                        {
                            DateTime dt = DateTime.ParseExact(item.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            String ReceiptDate = dt.ToString("yyyy-MM-dd");
                            item.Date = ReceiptDate;
                        }

                    }
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    xml = xml.Replace(">###<", "><");
                   


                    var objRepo = new Hdb_CashManagementRepository();
                    objRepo.RefundFromPDA(m, ((Login)(Session["LoginUser"])).Uid,xml);
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
                Hdb_CashManagementRepository ObjRR = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Hdb_CashManagementRepository objCR = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objCR = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Hdb_CashManagementRepository objCancelInv = new Hdb_CashManagementRepository();
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

            Hdb_CashManagementRepository ObjIR = new Hdb_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

        }


        #endregion

        #region Direct Online Payment
        public ActionResult DirectOnlinePayment()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DirectPaymentVoucher(HDB_DirectOnlinePayment objDOP)
        {
            Hdb_CashManagementRepository ObjIR = new Hdb_CashManagementRepository();
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
            List<HDB_DirectOnlinePayment> lstDOP = new List<HDB_DirectOnlinePayment>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();

            obj.GetOnlinePayAckList(((Login)(Session["LoginUser"])).Uid, OrderId);
            lstDOP = (List<HDB_DirectOnlinePayment>)obj.DBResponse.Data;
            return PartialView(lstDOP);
        }

        [HttpPost]
        public JsonResult ConfirmPayment(HDB_DirectOnlinePayment vm)
        {
            vm.OrderId = Convert.ToInt64(Session["OrderId"].ToString());

            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.OnlinePaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetOnlinePaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptList(int Pages)
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
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

        public JsonResult ListOfPendingInvoice(string Type)
        {
            Hdb_CashManagementRepository objcancle = new Hdb_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            objcancle.ListOfPendingInvoice(Uid, Type);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OnlinePaymentAgainstInvoice(HDB_OnlinePaymentAgainstInvoice objDOP)
        {
            Hdb_CashManagementRepository ObjIR = new Hdb_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<HDB_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
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
            List<HDB_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<HDB_OnlinePaymentAgainstInvoice>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<HDB_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
            IList<HDB_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<HDB_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<HDB_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Online Payment Receipt Against Invoice

        public ActionResult OnlinePaymentReceiptAgainstInvoice()
        {
            return PartialView();
        }

        public ActionResult OnlinePaymentReceiptDetailsAgainstInvoice(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetOnlinePaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlinePaymentReceiptListAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion


     

        #region Online On Account Payment Receipt

        public ActionResult OnlineOAPaymentReceipt()
        {
            return PartialView();
        }

        public ActionResult OnlineOAPaymentReceiptDetails(string PeriodFrom, string PeriodTo)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.OnlineOAPaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlineOAPaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetOnlineOAPaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlineOAPaymentReceiptList(int Pages)
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
            IList<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            objIR.GetOnlineOAPaymentReceiptList("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<OnlinePaymentReceipt>)objIR.DBResponse.Data);
            return Json(lstOPReceipt, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Transaction Status Enquiry

      
        public ActionResult TransactionStatusEnquiryCcAvn()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<JsonResult> GetTransactionStatusEnquiry(TransactionStatusEnquiry vm)
        {
            log.Info("GetTransactionStatusEnquiry START");
            Hdb_CashManagementRepository objIR = null;
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
                objIR = new Hdb_CashManagementRepository();
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
                    objIR = new Hdb_CashManagementRepository();
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

        public JsonResult GetInvoiceListForTransactionStatusEnquiry(string InvoiceNo, int Page)
        {
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetInvoiceNoForTransactionStatusEnquiry(InvoiceNo, Page);
            List<TransactionStatusEnquiry> lstCancel = new List<TransactionStatusEnquiry>();
            if (obj.DBResponse.Data != null)
            {
                lstCancel = (List<TransactionStatusEnquiry>)obj.DBResponse.Data;
            }

            return Json(lstCancel, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public async Task<JsonResult> GetTransactionStatusEnquiryCcAvn_1(TransactionStatusEnquiry vm)
        //{
        //    try
        //    {
        //        /*https://logintest.ccavenue.com/apis/servlet/DoWebTrans?enc_request=b9e1ba391affc7ec9690764a6db47e71165c1dd1d98826661aea6b8c9b1043a2&access_code=AVXH61IL68AO51HXOA&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2*/
               

        //        //string accessCode = "AVXH61IL68AO51HXOA";//from avenues
        //        //string workingKey = "F52847EDA0C715911416D9054623BB3E";// from avenues
        //        string accessCode = UPIConfiguration.AccessCode;
        //        string workingKey = UPIConfiguration.WorkingKey;

        //        //reference_no=tracking_id    AND order_no=order_id

        //        string orderStatusQueryJson = "{ \"reference_no\":\"311007894457\", \"order_no\":\"637774256280319861\" }"; //Ex. { "reference_no":"CCAvenue_Reference_No" , "order_no":"123456"} 
        //        string encJson = "";

        //        string queryUrl = "https://logintest.ccavenue.com/apis/servlet/DoWebTrans";


        //        CCACrypto ccaCrypto = new CCACrypto();
        //        encJson = ccaCrypto.Encrypt(orderStatusQueryJson, workingKey);

        //        // make query for the status of the order to ccAvenues change the command param as per your need
        //        string authQueryUrlParam = "enc_request=" + encJson + "&access_code=" + accessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON&version=1.2";

        //        // Url Connection
        //        String message = postPaymentRequestToGateway(queryUrl, authQueryUrlParam);
        //        //Response.Write(message);
        //        NameValueCollection param = getResponseMap(message);
        //        String status = "";
        //        String encResJson = "";
        //        if (param != null && param.Count == 2)
        //        {
        //            for (int i = 0; i < param.Count; i++)
        //            {
        //                if ("status".Equals(param.Keys[i]))
        //                {
        //                    status = param[i];
        //                }
        //                if ("enc_response".Equals(param.Keys[i]))
        //                {
        //                    encResJson = param[i];
        //                    //Response.Write(encResXML);
        //                }
        //            }
        //            if (!"".Equals(status) && status.Equals("0"))
        //            {
        //                String ResJson = ccaCrypto.Decrypt(encResJson, workingKey);
        //                Response.Write(ResJson);
        //            }
        //            else if (!"".Equals(status) && status.Equals("1"))
        //            {
        //                Console.WriteLine("failure response from ccAvenues: " + encResJson);
        //            }

        //        }

        //    }
        //    catch (Exception exp)
        //    {
        //        Response.Write("Exception " + exp);

        //    }
        //    return Json(new { Status = "", Message = "" }, JsonRequestBehavior.DenyGet);
        //    // return Json(new { Status = MsgStatus, Message = MsgStatusDescription }, JsonRequestBehavior.DenyGet);
        //}


        [HttpPost]
        public async Task<JsonResult> GetTransactionStatusEnquiryCcAvn(TransactionStatusEnquiry vm)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionStatusEnquiry CcAvn START");
            Hdb_CashManagementRepository objIR = null;
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
                objIR = new Hdb_CashManagementRepository();
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
                        log.Info("After Decryption ResJson :" + ResJson);
                        Hdb_CashManagementRepository objCash = new Hdb_CashManagementRepository();
                        objCash.AddCcAvnueTransactionUpdate(ccAvnResponse);
                       // log.Info("After Decryption ResJson :" + ccAvnResponse);
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
        #endregion

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
            List<Hdb_AcknowledgementViewList> lstOPReceipt = new List<Hdb_AcknowledgementViewList>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.AcknowledgementViewList(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<Hdb_AcknowledgementViewList>)obj.DBResponse.Data;
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
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
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
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
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



            return Json("",JsonRequestBehavior.AllowGet);
        }




        public async Task<JsonResult> GetTransactionDetailsBQR(string OrderId)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionStatusEnquiry BQR START");
            Hdb_CashManagementRepository objIR = null;
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
                        Hdb_CashManagementRepository objCash = new Hdb_CashManagementRepository();
                        if (string.IsNullOrEmpty(ccAvnResponse.error_code))
                        {
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


        #region Acknowledgement View List For CWC User
        public ActionResult AcknowledgementViewCWC()
        {
            return PartialView();
        }

        public ActionResult AcknowledgementViewListCWC(string PeriodFrom, string PeriodTo)
        {
            if (String.IsNullOrEmpty(PeriodFrom))
            {
                PeriodFrom = null;
            }
            if (String.IsNullOrEmpty(PeriodTo))
            {
                PeriodTo = null;
            }
            List<Hdb_AcknowledgementViewList> lstOPReceipt = new List<Hdb_AcknowledgementViewList>();
            Hdb_CashManagementRepository obj = new Hdb_CashManagementRepository();
            obj.AcknowledgementViewListCWC(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<Hdb_AcknowledgementViewList>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }
        #endregion

        #region Pull CCAvenue
        public async Task<JsonResult> GetAllCCAvenueDataPull()
        {
            Hdb_CashManagementRepository objIR = new Hdb_CashManagementRepository();
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



            return Json("",JsonRequestBehavior.AllowGet);
        }




        public async Task<JsonResult> GetTransactionDetailsCCAvenue(string OrderId)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            log.Info("GetTransactionPull CCAvenue START");
            Hdb_CashManagementRepository objIR = null;
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
                        Hdb_CashManagementRepository objCash = new Hdb_CashManagementRepository();
                        if (string.IsNullOrEmpty(ccAvnResponse.error_code))
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