using CwcExim.Areas.CashManagement.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using EinvoiceLibrary;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class Wlj_CashManagementController : BaseController
    {
        #region Credit Note

        #region Credit Note IRN Generation

        public async Task<JsonResult> GetGenerateIRNCreditNote(String CrNoteNo, String SupplyType, String Type, String CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Wlj_CashManagementRepository objPpgRepo = new Wlj_CashManagementRepository();



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

                Import.Models.Wlj_IrnB2CDetails q1 = new Import.Models.Wlj_IrnB2CDetails();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                var irnb2cobj = (Import.Models.Wlj_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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

                        Import.Models.Wlj_IrnB2CDetails q1 = new Import.Models.Wlj_IrnB2CDetails();
                        //   QrCodeData qdt = new QrCodeData();
                        objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNoteDN(CrNoteNo, Type, CRDR);
                        var irnb2cobj = (Import.Models.Wlj_IrnB2CDetails)objPpgRepo.DBResponse.Data;

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


        #endregion
        [HttpGet]
        public ActionResult CreateCreditNote()
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            objRepo.GetInvoiceDetailsForCreaditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetDebitNoteDetailsForCreaditNote(int InvoiceId)
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();

            if (InvoiceType == "Invoice")
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
                    Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("C");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchCreditNote(string Search)
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
                    Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.ListOfCRNote("D");
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView(lstNote);
        }
        [HttpGet]
        public ActionResult SearchDebitNote(string Search)
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            List<ListOfCRNote> lstNote = new List<ListOfCRNote>();
            objRepo.SearchCreditDebitNote("D", Search);
            if (objRepo.DBResponse.Data != null)
                lstNote = (List<ListOfCRNote>)objRepo.DBResponse.Data;
            return PartialView("ListOfDRNote", lstNote);
        }
        [HttpGet]
        public JsonResult GetInvoiceDetailsForDeditNote(int InvoiceId)
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            objRepo.GetInvoiceDetailsForDeditNote(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [HttpGet]
        public JsonResult GetChargesForDeditNote(int InvoiceId)
        {
            Wlj_CashManagementRepository objRepo = new Wlj_CashManagementRepository();
            objRepo.GetChargesListForCrDb(InvoiceId);
            if (objRepo.DBResponse.Data != null)
                return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        #endregion

        #region Miscellaneous Invoice
        [HttpGet]
        public ActionResult CreateMiscInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Wlj_CashManagementRepository ObjCash = new Wlj_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc(5);
            if (ObjCash.DBResponse.Status > 0)
                ViewBag.PaymentParty = (List<WljPaymentPartyName>)ObjCash.DBResponse.Data;
            else
                ViewBag.PaymentParty = null;





            ObjCash.GetPaymentPayerMisc();
            if (ObjCash.DBResponse.Status > 0)
            {
                ViewBag.PaymentPayer = (List<WljPaymentPartyName>)ObjCash.DBResponse.Data;
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
            Wlj_CashManagementRepository ObjCash = new Wlj_CashManagementRepository();
            ObjCash.GetPaymentPartyMisc(PTYpe);
            return Json(ObjCash.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMiscInvoiceAmount(string purpose, string purposecode, string SAC, float GST, string InvoiceType, int PartyId, decimal Amount,string SEZ)
        {
            Wlj_CashManagementRepository objChargeMaster = new Wlj_CashManagementRepository();
            objChargeMaster.GetMiscInvoiceAmount(purpose, purposecode, SAC, GST, InvoiceType, PartyId, Amount,SEZ);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInvoiceAmount(string purpose, string Size)
        {
            Wlj_CashManagementRepository objChargeMaster = new Wlj_CashManagementRepository();
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
                var invoiceData = JsonConvert.DeserializeObject<WljMiscPostModel>(objForm["MiscInvModelJson"].ToString());
                Wlj_CashManagementRepository objChargeMaster = new Wlj_CashManagementRepository();
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
        public ActionResult ListOfMiscInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Wlj_CashManagementRepository objER = new Wlj_CashManagementRepository();
            objER.ListOfMiscInvoice(Module, InvoiceNo, InvoiceDate);
            List<WljListOfMiscInvoice> obj = new List<WljListOfMiscInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WljListOfMiscInvoice>)objER.DBResponse.Data;
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

        #region Cancel Invoice


        [HttpGet]
        public ActionResult CancelInvoice()
        {
            CancelInvoice cin = new CancelInvoice();
            var InvoiceNo = "";
            Wlj_CashManagementRepository objcancle = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objcancle = new Wlj_CashManagementRepository();

            objcancle.ViewDetailsOfCancleInvoice(InvoiceId);
            if (objcancle.DBResponse.Data != null)
                cin = (CancelInvoice)objcancle.DBResponse.Data;
            return PartialView(cin);
        }


        [HttpGet]
        public ActionResult LstOfCancleInvoice(string InvoiceNo = "", int Page = 0)
        {
            Wlj_CashManagementRepository objCR = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objCR = new Wlj_CashManagementRepository();
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
            Wlj_CashManagementRepository objcancle = new Wlj_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfCancleInvoice(string InvoiceNo)
        {
            Wlj_CashManagementRepository objcancle = new Wlj_CashManagementRepository();

            objcancle.ListOfCancleInvoice(InvoiceNo);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvcDetForCancleInvoice(int InvoiceId = 0)
        {
            Wlj_CashManagementRepository objcancle = new Wlj_CashManagementRepository();

            objcancle.DetailsOfCancleInvoice(InvoiceId);
            return Json(objcancle.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCancelIRNForInvoice(string Irn, string CancelReason, string CancelRemark)
        {

            Wlj_CashManagementRepository objCancelInv = new Wlj_CashManagementRepository();
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

            Wlj_CashManagementRepository ObjIR = new Wlj_CashManagementRepository();
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
    }
}