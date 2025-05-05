using CCA.Util;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class Amd_CashManagementController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: CashManagement/Amd_CashManagement
        #region-- ADD MONEY TO PD --


        [HttpGet]
        public ActionResult AddMoneyToPD()
        {
            //     AccessRightsRepository ACCR = new AccessRightsRepository();
            //    ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            //    ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            var model = new VRN_AddMoneyToPD();
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
            var objRepo = new VRN_CashManagementRepository();
            objRepo.GetPartyDetails();
            ViewBag.Parties = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMoneyToPD(VRN_AddMoneyToPD m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var xml = Utility.CreateXML(m.Details.Where(o => o.Amount > 0).ToList());
                    var objRepo = new VRN_CashManagementRepository();
                    objRepo.AddMoneyToPD(m.PartyId, Convert.ToDateTime(m.TransDate), xml,m.TDSDeduction, m.DepositAmount);
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
        public ActionResult AddMoneyToPDList()
        {
            List<VRN_ReceiptDetails> lstCashReceiptModel = new List<VRN_ReceiptDetails>();
            VRN_CashManagementRepository obj = new VRN_CashManagementRepository();
            obj.GetAddMoneyToPDList();
            lstCashReceiptModel = (List<VRN_ReceiptDetails>)obj.DBResponse.Data;
            return PartialView("AddMoneyToPDList", lstCashReceiptModel);
        }

        [HttpGet]
        public ActionResult ViewAddToSD(int PdaAcId)
        {
            VRN_AddMoneyToPDModelRefund ObjSD = new VRN_AddMoneyToPDModelRefund();
            VRN_CashManagementRepository objCR = new VRN_CashManagementRepository();
            objCR.ViewAddToSD(PdaAcId);
            if (objCR.DBResponse.Data != null)
                ObjSD = (VRN_AddMoneyToPDModelRefund)objCR.DBResponse.Data;
            return PartialView(ObjSD);
        }
        #endregion


        #region Cancel Invoice


        [HttpGet]
        public ActionResult CancelInvoice()
        {
            CancelInvoice cin = new CancelInvoice();
            var InvoiceNo = "";
            Amd_CashManagementRepository objcancle = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objcancle = new Amd_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Amd_CashManagementRepository objCR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objCR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objcancle = new Amd_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Amd_CashManagementRepository objcancle = new Amd_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Amd_CashManagementRepository objcancle = new Amd_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Amd_CashManagementRepository objCancelInv = new Amd_CashManagementRepository();
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

            Amd_CashManagementRepository ObjIR = new Amd_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditCancleInvoice(objCancelInvoice, Uid);
            //ModelState.Clear();
            return Json(ObjIR.DBResponse);

        }


        #endregion

        #region Credit Note

        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Amd_CashManagementRepository objPpgRepo = new Amd_CashManagementRepository();

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

                Export.Models.AMD_IrnB2CDetails q1 = new Export.Models.AMD_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                var irnb2cobj = (Export.Models.AMD_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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

                        Export.Models.AMD_IrnB2CDetails q1 = new Export.Models.AMD_IrnB2CDetails();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                        var irnb2cobj = (Export.Models.AMD_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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

        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddCreditNote(VRN_CreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value credit note can not be saved." });
                }
                else
                {
                    Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            List<VRN_ListOfCRNote> lstNote = new List<VRN_ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<VRN_ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchCreditNote(string Search)
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            List<VRN_ListOfCRNote> lstNote = new List<VRN_ListOfCRNote>();
            objRepo.SearchCreditDebitNote("C", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<VRN_ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfCRNote", lstNote);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(int CRNoteId, string Note)
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            VRN_PrintModelOfCr objCR = new VRN_PrintModelOfCr();
            objRepo.PrintDetailsForCRNote(CRNoteId);
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (VRN_PrintModelOfCr)objRepo.DBResponse.Data;
                string Path = GenerateCRNotePDF(objCR, CRNoteId, Note);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }
        [NonAction]
        public string GenerateCRNotePDF(VRN_PrintModelOfCr objCR, int CRNoteId, string Note)
        {
            string SACCode = "", note = "", fileName = "";
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
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + objCR.irn + " </td></tr><tr><td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(objCR.SignedQRCode)) + "'/> </td></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + objCR.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + objCR.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + objCR.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + objCR.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + objCR.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(objCR.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
            string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
            string tr = "";
            int Count = 1;
            decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0, tot = 0;

            objCR.lstCharges.ToList().ForEach(item =>
            {
                tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
                IGSTAmt += item.IGSTAmt;
                CGSTAmt += item.CGSTAmt;
                SGSTAmt += item.SGSTAmt;
                tot += item.Taxable;
                Count++;
            });
            string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
            string Remarks = objCR.Remarks;
            string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + tot + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Round Up</span> <span>" + objCR.RoundUp + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
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




        //[HttpGet]
        //public ActionResult ViewAddToSD(int PdaAcId)
        //{
        //    VRN_AddMoneyToPDModelRefund ObjSD = new VRN_AddMoneyToPDModelRefund();
        //    VRN_CashManagementRepository objCR = new VRN_CashManagementRepository();
        //    objCR.ViewAddToSD(PdaAcId);
        //    if (objCR.DBResponse.Data != null)
        //        ObjSD = (VRN_AddMoneyToPDModelRefund)objCR.DBResponse.Data;
        //    return PartialView(ObjSD);
        //}
        #endregion

        #region Debit Note
        [HttpGet]
        public ActionResult CreateDebitNote()
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
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
        public JsonResult AddDebitNote(VRN_CreditNote objCR)
        {
            if (ModelState.IsValid)
            {
                if (objCR.TotalAmt <= 0)
                {
                    return Json(new { Status = -1, Message = "Zero or Negative value debit note can not be saved." });
                }
                else
                {
                    Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            List<VRN_ListOfCRNote> lstNote = new List<VRN_ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<VRN_ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchDebitNote(string Search)
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            List<VRN_ListOfCRNote> lstNote = new List<VRN_ListOfCRNote>();
            objRepo.SearchCreditDebitNote("D", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<VRN_ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfDRNote", lstNote);
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            Amd_CashManagementRepository objRepo = new Amd_CashManagementRepository();
            objRepo.GetChargesListForCrDb(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        #endregion

        public string LoadImage(string img)
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
        public JsonResult OnlinePaymentAgainstInvoice(Amd_OnlinePaymentAgainstInvoice objDOP)
        {
            Amd_CashManagementRepository ObjIR = new Amd_CashManagementRepository();
            var Uid = ((Login)Session["LoginUser"]).EximTraderId;
            log.Info("Response save start");
            objDOP.OrderId = DateTime.Now.Ticks;
            objDOP.TransId = Convert.ToDecimal(DateTime.Now.Ticks);
            string InvoiceListXML = "";
            if (objDOP.lstInvoiceDetails != null)
            {
                var lstInvoiceDetailsList = JsonConvert.DeserializeObject<List<Amd_OnlineInvoiceDetails>>(objDOP.lstInvoiceDetails.ToString());
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
            List<Amd_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Amd_OnlinePaymentAgainstInvoice>();
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.GetOnlinePaymentAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<Amd_OnlinePaymentAgainstInvoice>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);

        }
        [HttpGet]
        public ActionResult OnlinePaymentAgainstInvoiceListDetails(int Pages = 0)
        {
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
            IList<Amd_OnlinePaymentAgainstInvoice> lstOPReceipt = new List<Amd_OnlinePaymentAgainstInvoice>();
            objIR.GetOnlinePaymentAgainstInvoice("", Pages);
            if (objIR.DBResponse.Data != null)
                lstOPReceipt = ((List<Amd_OnlinePaymentAgainstInvoice>)objIR.DBResponse.Data);
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
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.OnlinePaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlinePaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.GetOnlinePaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlinePaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.OnlineOAPaymentReceiptDetails(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult OnlineOAPaymentReceiptList(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.GetOnlineOAPaymentReceiptList(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadOnlineOAPaymentReceiptList(int Pages)
        {
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objIR = null;
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
                objIR = new Amd_CashManagementRepository();
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
                    objIR = new Amd_CashManagementRepository();
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
                objIR = new Amd_CashManagementRepository();
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
                        Amd_CashManagementRepository objCash = new Amd_CashManagementRepository();
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
            List<Amd_AcknowledgementViewList> lstOPReceipt = new List<Amd_AcknowledgementViewList>();
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.AcknowledgementViewList(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<Amd_AcknowledgementViewList>)obj.DBResponse.Data;
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
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.BQRPaymentReceiptDetailsAgainstInvoice(PeriodFrom, PeriodTo, ((Login)(Session["LoginUser"])).Uid);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        public ActionResult BQRPaymentReceiptListAgainstInvoice(string SearchValue)
        {
            List<OnlinePaymentReceipt> lstOPReceipt = new List<OnlinePaymentReceipt>();
            Amd_CashManagementRepository obj = new Amd_CashManagementRepository();
            obj.GetBQRPaymentReceiptListAgainstInvoice(SearchValue, 0);
            lstOPReceipt = (List<OnlinePaymentReceipt>)obj.DBResponse.Data;
            return PartialView(lstOPReceipt);
        }

        [HttpGet]
        public ActionResult LoadBQRPaymentReceiptListAgainstInvoice(int Pages = 0)
        {
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
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
            Amd_CashManagementRepository objIR = new Amd_CashManagementRepository();
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
                        Amd_CashManagementRepository objCash = new Amd_CashManagementRepository();
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