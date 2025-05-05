using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using CwcExim.Filters;

using DynamicExcel;
using System.Text;
using CwcExim.Areas.Report.Models;
using System.Threading.Tasks;
using EinvoiceLibrary;
using System.Drawing;

namespace CwcExim.Areas.Import.Controllers
{
    public class VIZ_CWCImportController : Controller
    {
        // GET: Import/VIZ_CWCImport
        private string CompanyEmail = "";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        


       

        #region YardInvoice
        [HttpGet]
        public ActionResult CreateYardPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            /*Wfld_ImportRepository objimp = new Wfld_ImportRepository();
            objimp.GetImpPaymentPartyForPage("",0);
            if (objimp.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objimp.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;*/


            return PartialView();
        }


        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            VIZ_ImportRepository objER = new VIZ_ImportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<VIZListOfExpInvoice> obj = new List<VIZListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<VIZListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

       



        [HttpGet]
        public JsonResult LoadPartyListFCL(int Page)
        {
            VIZ_ImportRepository objimp = new VIZ_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult LoadPayeeListFCL(int Page)
        {
            VIZ_ImportRepository objimp = new VIZ_ImportRepository();
            objimp.GetImpPaymentPayeeForFCLPage("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchByPartyCodeFCL(string PartyCode)
        {
            VIZ_ImportRepository objimp = new VIZ_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult SearchByPayeeCodeFCL(string PartyCode)
        {
            VIZ_ImportRepository objimp = new VIZ_ImportRepository();
            objimp.GetImpPaymentPayeeForFCLPage(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, string DeliveryDate, int AppraisementId, string TaxType,
            List<PaymentSheetContainer> lstPaySheetContainer, string SEZ, int OTHours = 0, int PartyId = 0, int PayeeId = 0,
            int InvoiceId = 0, int isdirect = 0, int NoOfVehicles = 1, int Distance = 0, int OwnMovement = 0, int InsuredParty = 0, int YardToBond = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            VIZ_ImportRepository objPpgRepo = new VIZ_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetYardPaymentSheet(InvoiceDate, DeliveryDate, AppraisementId, TaxType, XMLText, InvoiceId, OTHours, PartyId,
                PayeeId, isdirect, NoOfVehicles, Distance, OwnMovement, InsuredParty, YardToBond, SEZ);
            var Output = (VIZInvoiceYard)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "IMPYard";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                if (!Output.BOEDate.Contains(item.BOEDate))
                    Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                    Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new VIZPostPaymentContainer
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });
                }


                Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
                Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
                Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                    + Output.lstPrePaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
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
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;

                Output.InvoiceAmt = (Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = 0;
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(String InvoiceObj, int Vehicle, int IsDirect, int IsYardtoBond, string SEZ, int BillToParty, int Distance, int OTHours, int IsTransporter, int IsInsured)
        {
            try
            {
                //PpgInvoiceYard objForm;
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<VIZInvoiceYard>(InvoiceObj);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";




                if (invoiceData.lstPostPaymentContXML != null)
                {
                    invoiceData.lstPostPaymentCont = JsonConvert.DeserializeObject<List<VIZPostPaymentContainer>>(invoiceData.lstPostPaymentContXML);
                    foreach (var item in invoiceData.lstPostPaymentCont)
                    {
                        item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                        item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                        item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                        item.LineNo = string.IsNullOrEmpty(item.LineNo) ? "-" : item.LineNo;
                        item.BOENo = string.IsNullOrEmpty(item.BOENo) ? "-" : item.BOENo;


                    }
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);

                }
                if (invoiceData.lstPostPaymentChrgXML != null)
                {
                    // ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                    invoiceData.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<VIZPostPaymentChrg>>(invoiceData.lstPostPaymentChrgXML);
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmountXML != null)
                {
                    // ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                    invoiceData.lstContWiseAmount = JsonConvert.DeserializeObject<List<VIZContainerWiseAmount>>(invoiceData.lstContWiseAmountXML);
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                }
                if (invoiceData.lstOperationCFSCodeWiseAmountXML != null)
                {

                    invoiceData.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<VIZOperationCFSCodeWiseAmount>>(invoiceData.lstOperationCFSCodeWiseAmountXML);
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                    //  OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakupXML != null)
                {

                    invoiceData.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<VIZPostPaymentChargebreakupdate>>(invoiceData.lstPostPaymentChrgBreakupXML);
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                    //ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                VIZ_ImportRepository objChargeMaster = new VIZ_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, Vehicle, ((Login)(Session["LoginUser"])).Uid, IsDirect, IsYardtoBond, SEZ, BillToParty, Distance, OTHours, IsTransporter, IsInsured, "IMPYard", "");

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

        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {


            VIZ_ImportRepository objPpgRepo = new VIZ_ImportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    log.Info("after calling GenerateEinvoice");
                    if (ERes.Status == 0)
                    {
                        log.Info(ERes.ErrorDetails.ErrorMessage);
                        log.Info(ERes.ErrorDetails.ErrorCode);
                        log.Info("Invoice No:" + InvoiceNo);
                        log.Info(ERes.Status);
                    }
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
                    VIZ_IrnB2CDetails irnb2cobj = new VIZ_IrnB2CDetails();
                    irnb2cobj = (VIZ_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                    if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                    {
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                        objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
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
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
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
                VIZ_IrnB2CDetails irnb2cobj = new VIZ_IrnB2CDetails();
                irnb2cobj = (VIZ_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                    objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
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
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
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



        //public Image LoadImage(string stringPath)
        //{
        //    //data:image/gif;base64,
        //    //this image is a single pixel (black)
        //    byte[] bytes = Convert.FromBase64String(stringPath);

        //    Image image;
        //    using (MemoryStream ms = new MemoryStream(bytes))
        //    {
        //        image = Image.FromStream(ms);
        //    }

        //    return image;
        //}
        /*
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult YardInvoicePrint(string InvoiceNo)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetInvoiceDetailsForPrintByNo(InvoiceNo, "IMPYard");
            PpgInvoiceYard objGP = new PpgInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (PpgInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFYard(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFYard(PpgInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/YardInvoice" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CFS Whitefield</span>");
            html.Append("<br />ASSESSMENT SHEET FCL");
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
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate)- Convert.ToDateTime(objGP.ArrivalDate)).TotalDays+1).ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (container.CargoType == 1 ? "Haz" : "Non-Haz") + "</td>");
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
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalAmt.ToString("0") + "</td>");
            html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            html.Append("Total Invoice (In Word) :");
            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='7'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");
            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CFS Whitefield</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/YardInvoice" + InvoiceId.ToString() + ".pdf";
        }
        */
        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
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


        public JsonResult GetCHANameForYardPaymentSheet(string PartyCode, int Page)
        {
            List<VIZCHAForPage> lstCHAName = new List<VIZCHAForPage>();
            VIZ_ImportRepository obj = new VIZ_ImportRepository();
            obj.ListOfChaForPage(PartyCode, Page);
            var objList = obj.DBResponse.Data;

            if (obj.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(obj.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                var CHAState = Jobject["lstCHA"];
                return Json(CHAState.ToString(), JsonRequestBehavior.AllowGet);

            }
            return Json("", JsonRequestBehavior.AllowGet);



        }


        //[HttpGet]
        //public JsonResult GetBillToPartyDetails(string PartyId)
        //{
        //    Wfld_ImportRepository objimp = new Wfld_ImportRepository();
        //    objimp.GetBillToPartyDetails(PartyId);
        //    return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region LCL INVOICE AFTER DESTUFFING

        public ActionResult LclInvoiceAfterDestuffing(string type = "Tax")
        {
            ViewData["InvType"] = type;




            return PartialView();

        }

        public JsonResult GetPaymentSheetDestuffingNo()
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetDeStuffingRequestForImpPaymentSheet();
            List<VIZ_DestuffingPaymentSheet> lstDestuffing = new List<VIZ_DestuffingPaymentSheet>();
            if (objImport.DBResponse.Status > 0)
            {
                lstDestuffing = (List<VIZ_DestuffingPaymentSheet>)objImport.DBResponse.Data;
            }



            //ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //   else
            //  ViewBag.StuffingReqList = null;


            return Json(lstDestuffing, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetPaymentSheetDestuffingCont(int AppraisementId)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetDestuffingContForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDestuffingPaymentSheet(string InvoiceDate, string InvoiceType, string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, int PayeeId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST,
            string PayeeName, List<CHN_DestuffingConntainer> lstPaySheetContainer, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            VIZ_ImportRepository objChrgRepo = new VIZ_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDestuffingPaymentSheet(InvoiceDate, AppraisementId, SEZ, PartyId, PayeeId, InvoiceType, XMLText, InvoiceId);
            var Output = (VIZ_ImpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPDestuff";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new VIZ_ExpContainer();
                obj.CFSCode = item;
                obj.ContainerNo = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerNo;
                obj.Size = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().Size.ToString();
                obj.IsReefer = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Reefer);
                obj.Insured = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Insured);
                obj.RMS = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.RMS);
                obj.CargoType = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Min(x => x.CargoType);
                obj.ArrivalDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ArrivalDate;
                obj.StuffingDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().StuffingDate;
                obj.NoOfPackages = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.NoOfPackages);
                obj.GrossWt = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.GrossWt);
                obj.WtPerUnit = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.WtPerUnit);
                obj.SpaceOccupied = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.SpaceOccupied);
                obj.SpaceOccupiedUnit = "SQM";
                obj.CIFValue = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.CIFValue);
                obj.Duty = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.Duty);
                obj.PltBox = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.PltBox);
                obj.ParkDays = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ParkDays;
                obj.LockDays = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().LockDays;
                Output.lstPSCont.Add(obj);
            });

            Output.lstPostPaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                if (!Output.BOEDate.Contains(item.BOEDate))
                    Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                /* if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                 {
                     Output.lstPostPaymentCont.Add(new PpgPostPaymentContainer
                     {
                         CargoType = item.CargoType,
                         CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                         CFSCode = item.CFSCode,
                         CIFValue = item.CIFValue,
                         ContainerNo = item.ContainerNo,
                         ArrivalDate = item.ArrivalDate,
                         ArrivalTime = item.ArrivalTime,
                         DeliveryType = item.DeliveryType,
                         DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                         Duty = item.Duty,
                         GrossWt = item.GrossWeight,
                         Insured = item.Insured,
                         NoOfPackages = item.NoOfPackages,
                         Reefer = item.Reefer,
                         RMS = item.RMS,
                         Size = item.Size,
                         SpaceOccupied = item.SpaceOccupied,
                         SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                         StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                         WtPerUnit = item.WtPerPack,
                         AppraisementPerct = item.AppraisementPerct,
                         HeavyScrap = item.HeavyScrap,
                         StuffCUM = item.StuffCUM
                     });
                 }*/


                Output.TotalNoOfPackages = Output.lstPostPaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPostPaymentCont.Sum(o => o.GrossWt);
                Output.TotalWtPerUnit = Output.lstPostPaymentCont.Sum(o => o.WtPerUnit);
                Output.TotalSpaceOccupied = Output.lstPostPaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = "SQM";
                Output.TotalValueOfCargo = Output.lstPostPaymentCont.Sum(o => o.CIFValue) + Output.lstPostPaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
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
                Output.InvoiceType = InvoiceType;
                Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<VIZ_ImpPaymentSheet>(objForm["PaymentSheetModelJson"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

                foreach (var item in invoiceData.lstPSCont)
                {
                    //item.StuffingDate = Convert.ToDateTime(item.StuffingDate).ToString("yyyy-MM-dd");
                    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                foreach (var item in invoiceData.lstOperationContwiseAmt)
                {
                    if (item.DocumentDate != "")
                        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
                }

                if (invoiceData.lstPSCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPSCont);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContwiseAmt != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContwiseAmt);
                }
                if (invoiceData.lstOperationContwiseAmt != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationContwiseAmt);
                }

                VIZ_ImportRepository objChargeMaster = new VIZ_ImportRepository();
                objChargeMaster.AddEditDestuffingInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDestuff");

                /*invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");*/
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

        public ActionResult ListOfDestuffingInvoice()
        {
            VIZ_ImportRepository obj = new VIZ_ImportRepository();
            List<VIZListOfExpInvoice> lstExportInvoice = new List<VIZListOfExpInvoice>();
            obj.GetAllDestuffingPaymentSheet();
            lstExportInvoice = (List<VIZListOfExpInvoice>)obj.DBResponse.Data;
            return PartialView("ListOfDestuffingInvoice", lstExportInvoice);
        }

        #endregion

        #region DeliveryPayment Sheet

        [HttpGet]
        public ActionResult MergeSingleDeliAppPaymentSheetIssueSlip(string type = "Tax")
        {
            ViewData["InvType"] = type;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDestuffingNo()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNo(((Login)(Session["LoginUser"])).Uid);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo });
                });

            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetImporterName()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.ListOfImporterForMerge();

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Importer>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { ImporterId = item.ImporterId, ImporterName = item.ImporterName });
                });

            }
            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHANAME()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<VIZCHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode, BillToParty = item.BillToParty, IsInsured = item.IsInsured, IsTransporter = item.IsTransporter, InsuredFrmdate = item.InsuredFrmdate, InsuredTodate = item.InsuredTodate });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCHANAMEForPayment()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.GetImpPaymentPartyForMergePage("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<ImpPartyForpage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { PartyId = item.PartyId, PartyName = item.PartyName, Address = item.Address, State = item.State, StateCode = item.StateCode, GSTNo = item.GSTNo, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfMergeDeliveryApplication()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            List<VIZMergeDeliveryApplicationList> LstDelivery = new List<VIZMergeDeliveryApplicationList>();
            ObjIR.GetAllDeliveryMergeApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<VIZMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreMergeDataForDeliveryApp(int Page)
        {
            VIZ_ImportRepository ObjCR = new VIZ_ImportRepository();
            List<VIZMergeDeliveryApplicationList> LstJO = new List<VIZMergeDeliveryApplicationList>();
            ObjCR.GetAllDeliveryMergeApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<VIZMergeDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGST(int PartyID)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.GetGSTValue(PartyID);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSingleClickDeliveryPaymentSheet(string InvoiceType, int DestuffingAppId, string DeliveryDate, string InvoiceDate, int InvoiceId, List<VIZDeliveryApplicationDtl> DeliveryXML,
           int PartyId, int PayeeId, int vehicleNo, int vehicleNoUn, bool IsInsured, bool BillToParty, bool Transporter, bool ImportToBond, int Cargo, List<VIZDeliveryGodownDtl> GodownXML, int OTHours = 0, int OblFlag = 0, string SEZ = "")
        {
            string XMLText = "";
            if (DeliveryXML != null)
            {
                XMLText = Utility.CreateXML(DeliveryXML);
            }

            string XMLGodown = "";
            if (GodownXML != null)
            {
                XMLGodown = Utility.CreateXML(GodownXML);
            }

            VIZ_ImportRepository objChrgRepo = new VIZ_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheetSingle(InvoiceType, DestuffingAppId, DeliveryDate, InvoiceDate, InvoiceId, XMLText, PartyId, PayeeId, vehicleNo, vehicleNoUn, IsInsured, BillToParty, Transporter, ImportToBond, Cargo, XMLGodown, OTHours, OblFlag, SEZ);
            #region Comment
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
            #endregion
            var Output = (VIZInvoiceGodown)objChrgRepo.DBResponse.Data;
            VIZtentativeinvoice.InvoiceObj = (VIZInvoiceGodown)objChrgRepo.DBResponse.Data;
            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            // Output.InvoiceDate = DateTime.Now.Date.ToString() ;
            Output.Module = "IMPDeli";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";

                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                if (!Output.BOEDate.Contains(item.BOEDate))
                    Output.BOEDate += item.BOEDate;
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                    Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new VIZPostPaymentContainer
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });
                }


                Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
                Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
                Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                    + Output.lstPrePaymentCont.Sum(o => o.Duty);


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
                // Output.RoundUp = 0;
              
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp =Math.Ceiling(Output.InvoiceAmt) - Output.AllTotal;
            });
            VIZtentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSingleMergeDeliveryApplication(VIZMergeDeliveryIssueViewModel ObjDelivery)
        // VIZMergeDeliveryIssueViewModel ObjDelivery)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                string DeliveryAppDtlXml = "";
                string DeliveryOrdDtlXml = "";
                string DeliveryGodownDtlXml = "";

                string lstPrePaymentContXML = "";
                string lstPostPaymentContXML = "";
                string lstPostPaymentChrgXML = "";
                string lstContWiseAmountXML = "";
                string lstOperationCFSCodeWiseAmountXML = "";
                string lstPostPaymentChrgBreakupXML = "";
                string lstInvoiceCargoXML = "";





                if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<VIZDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryAppDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                //if (ObjDelivery.DeliApp.DeliveryOrdDtlXml !=null)
                //{
                //    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<VIZDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                //    DeliveryOrdDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                //}
                if (ObjDelivery.DeliApp.DeliveryGodownDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryGodownDtl = JsonConvert.DeserializeObject<List<VIZDeliveryGodownDtl>>(ObjDelivery.DeliApp.DeliveryGodownDtlXml);
                    DeliveryGodownDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryGodownDtl);
                }

                if (ObjDelivery.DeliApp.lstPrePaymentContXML != "" && ObjDelivery.DeliApp.lstPrePaymentContXML != null)
                {
                    ObjDelivery.DeliApp.lstPrePaymentCont = JsonConvert.DeserializeObject<List<VIZPreInvoiceContainer>>(ObjDelivery.DeliApp.lstPrePaymentContXML);
                    lstPrePaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPrePaymentCont);
                }



                if (ObjDelivery.DeliApp.lstPostPaymentContXML != "" && ObjDelivery.DeliApp.lstPostPaymentContXML != null)
                {
                    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<VIZPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                }
                //if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                //{
                //    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<VIZPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                //    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                //}

                if (ObjDelivery.DeliApp.lstPostPaymentChrgXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<VIZPostPaymentChrg>>(ObjDelivery.DeliApp.lstPostPaymentChrgXML);
                    lstPostPaymentChrgXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrg);
                }
                if (ObjDelivery.DeliApp.lstContWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstContWiseAmount = JsonConvert.DeserializeObject<List<VIZContainerWiseAmount>>(ObjDelivery.DeliApp.lstContWiseAmountXML);
                    lstContWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstContWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<VIZOperationCFSCodeWiseAmount>>(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML);
                    lstOperationCFSCodeWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<VIZDeliPostPaymentChargebreakupdate>>(ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML);
                    lstPostPaymentChrgBreakupXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrgBreakup);
                }
                if (ObjDelivery.DeliApp.lstInvoiceCargoXML != "")
                {
                    ObjDelivery.DeliApp.lstInvoiceCargo = JsonConvert.DeserializeObject<List<VIZInvoiceCargo>>(ObjDelivery.DeliApp.lstInvoiceCargoXML);
                    lstInvoiceCargoXML = Utility.CreateXML(ObjDelivery.DeliApp.lstInvoiceCargo);
                }




                ObjIR.AddEditSingleMergeDeliveryApplication(ObjDelivery,
                    DeliveryAppDtlXml, DeliveryOrdDtlXml, lstPrePaymentContXML, lstPostPaymentContXML,
                    lstPostPaymentChrgXML, lstContWiseAmountXML, lstOperationCFSCodeWiseAmountXML, lstPostPaymentChrgBreakupXML, lstInvoiceCargoXML, DeliveryGodownDtlXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
            //var invoiceData = JsonConvert.DeserializeObject<VIZInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
            //string ContainerXML = "";
            //string ChargesXML = "";
            //string ContWiseCharg = "";
            //string OperationCfsCodeWiseAmtXML = "", CargoXML = "";
            //string ChargesBreakupXML = "";

            //foreach (var item in invoiceData.lstPostPaymentCont)
            //{
            //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
            //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
            //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
            //}

            //foreach (var item in invoiceData.lstInvoiceCargo)
            //{
            //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? "1900-01-01" : item.StuffingDate;
            //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? "1900-01-01" : item.DestuffingDate;
            //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? "1900-01-01" : item.CartingDate;
            //    //  item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
            //}


            //string DeliveryXml = "";
            //string DeliveryOrdXml = "";
            //string ChargesBreakupXML = "";
            //if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
            //{
            //    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<VIZDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
            //    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
            //}


            //if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
            //{
            //    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<VIZDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
            //    DeliveryOrdXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
            //}

            //if (invoiceData.lstPostPaymentCont != null)
            //{
            //    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
            //}
            //if (invoiceData.lstPostPaymentChrg != null)
            //{
            //    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
            //}
            //if (invoiceData.lstContWiseAmount != null)
            //{
            //    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
            //}
            //if (invoiceData.lstOperationCFSCodeWiseAmount != null)
            //{
            //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
            //}
            //if (invoiceData.lstInvoiceCargo != null)
            //{
            //    CargoXML = Utility.CreateXML(invoiceData.lstInvoiceCargo);
            //}
            //if (invoiceData.lstPostPaymentChrgBreakup != null)
            //{
            //    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
            //}
            //  VIZ_ImportRepository objChargeMaster = new VIZ_ImportRepository();

            //objChargeMaster.AddEditSingleMergeDeliveryApplication(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, 
            //    ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML, DeliveryXml, DeliveryOrdXml);

            //invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
            //invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
            //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
            //objChargeMaster.DBResponse.Data = invoiceData;
            // return Json(objChargeMaster.DBResponse);
        }
        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBOEDetForDeliveryApp(int DestuffingEntryDtlId)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOfGodownByOBL(string OBL)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();


            ObjIR.ListOfGodownByOBL(OBL);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Areas.Import.Models.WFLDGodownListWithDestiffDetails>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }



            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);


        }


        [HttpGet]
        public JsonResult GetCHANAMEForSingleClick()
        {

            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForSingleMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<CHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAListforSingleClick(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfChaForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeListforSingleClick(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfPayeeForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCIFFromOOC(String BOE, String BOEDT)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetCIFFromOOCDelivery(BOE, BOEDT);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintIssueSlip(int IssueSlipId)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetIssueSlipForPreview(IssueSlipId);
            if (ObjIR.DBResponse.Data != null)
            {
                VIZ_Issueslip ObjIssueSlip = new VIZ_Issueslip();
                ObjIssueSlip = (VIZ_Issueslip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForIssueSlip(ObjIssueSlip, IssueSlipId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForIssueSlip(VIZ_Issueslip ObjIssueSlip, int IssueSlipId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", BOEDate = "", BOENo = "", Vessel = "", CHA = "", Importer = "", ShippingLine = "",
            CargoDescription = "", MarksNo = "", Weight = "", LineNo = "", Rotation = "", ArrivalDate = "", DestuffingDate = "", Location = "";
            int Count = 0;
            // decimal MarksNo = 0, Weight=0;
            string Html = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            ObjIssueSlip.LstIssueSlipRpt.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                Size = (Size == "" ? (item.Size) : (Size + "<br/>" + item.Size));
                // BOEDate = (BOEDate == "" ? (item.BOEDate) : (BOEDate + "<br/>" + item.BOEDate));
                //  BOENo = (BOENo == "" ? (item.BOENo) : (BOENo + "<br/>" + item.BOENo));
                //  Vessel = (Vessel == "" ? (item.Vessel) : (Vessel + "<br/>" + item.Vessel));
                // CHA = (CHA == "" ? (item.CHA) : (CHA + "<br/>" + item.CHA));
                // Importer = (Importer == "" ? (item.Importer) : (Importer + "<br/>" + item.Importer));
                // CargoDescription = (CargoDescription == "" ? (item.CargoDescription) : (CargoDescription + "<br/>" + item.CargoDescription));
                MarksNo = (MarksNo == "" ? (item.MarksNo) : (MarksNo + "<br/>" + item.MarksNo));
                // Weight = (Weight == "" ? (item.Weight) : (Weight + "<br/>" + item.Weight));
                //  LineNo = (LineNo == "" ? (item.LineNo) : (LineNo + "<br/>" + item.LineNo));
                //  Rotation = (Rotation == "" ? (item.Rotation) : (Rotation + "<br/>" + item.Rotation));
                // ArrivalDate = (ArrivalDate == "" ? (item.ArrivalDate) : (ArrivalDate + "<br/>" + item.ArrivalDate));
                // DestuffingDate = (DestuffingDate == "" ? (item.DestuffingDate) : (DestuffingDate + "<br/>" + item.DestuffingDate));
                // Location = (Location == "" ? (item.Location) : (Location + "<br/>" + item.Location));
                // ShippingLine = (ShippingLine == "" ? (item.ShippingLine) : (ShippingLine + "<br/>" + item.ShippingLine));
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
            {
                if (CHA == "")
                    CHA = item.CHA;
                else
                    CHA += "<br/>" + item.CHA;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Weight = x.Weight }).Distinct().ToList().ForEach(item =>
            {
                if (Weight == "" || Weight == "0.0000")
                    Weight = item.Weight;
                else
                    Weight += "<br/>" + item.Weight;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { LineNo = x.LineNo }).Distinct().ToList().ForEach(item =>
            {
                if (LineNo == "")
                    LineNo = item.LineNo;
                else
                    LineNo += "<br/>" + item.LineNo;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Rotation = x.Rotation }).Distinct().ToList().ForEach(item =>
            {
                if (Rotation == "")
                    Rotation = item.Rotation;
                else
                    Rotation += "<br/>" + item.Rotation;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Location = x.Location }).Distinct().ToList().ForEach(item =>
            {
                if (Location == "")
                    Location = item.Location;
                else
                    Location += "<br/>" + item.Location;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item =>
            {
                if (Importer == "")
                    Importer = item.Importer;
                else
                    Importer += "<br/>" + item.Importer;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "<br/>" + item.CargoDescription;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ArrivalDate = x.ArrivalDate }).Distinct().ToList().ForEach(item =>
            {
                if (ArrivalDate == "")
                    ArrivalDate = item.ArrivalDate;
                else
                    ArrivalDate += "<br/>" + item.ArrivalDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "<br/>" + item.DestuffingDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLine == "")
                    ShippingLine = item.ShippingLine;
                else
                    ShippingLine += "<br/>" + item.ShippingLine;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "<br/>" + item.Vessel;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOEDate = x.BOEDate }).Distinct().ToList().ForEach(item =>
            {
                if (BOEDate == "")
                    BOEDate = item.BOEDate;
                else
                    BOEDate += "<br/>" + item.BOEDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOENo = x.BOENo }).Distinct().ToList().ForEach(item =>
            {
                if (BOENo == "")
                    BOENo = item.BOENo;
                else
                    BOENo += "<br/>" + item.BOENo;
            });
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }

            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS VISAKHAPATNAM</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>C.H.A Name.</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + Weight + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'><span style='border-bottom:1px solid #000;'></span></td><td style='text-align:right;'><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>Issue Slip Of Container Freight Station.</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/></td></tr><tr><td colspan='2' style='text-align:left;'><span style='border-bottom:1px solid #000;'></span><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry No.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + BOENo + "</td><td style='border:1px solid #000;padding:5px;'>" + BOEDate + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td><span></span></td><td><br/><br/></td></tr></tbody></table></td></tr></tbody></table>";


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
        }


        [HttpGet]
        public JsonResult LoadImporterCustList(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfImporterCustForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchImporterCustByPartyCode(string PartyCode)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfImporterCustForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult SearchPayeeNameByPartyCode(string PartyCode)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetImpPaymentPayeeForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            /*
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            */
            //objImport.GetEmptyContainerListForInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.EmptyContList = null;

            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            objImport.GetImpPaymentPartyForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objImport.GetImpPaymentPayeeForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPayee = Jobject["lstParty"];
                ViewBag.StatePayee = Jobject["State"];
            }
            else
            {
                ViewBag.lstPayee = null;
            }

            return PartialView();
        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetEmptyContainerListForInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.EmptyContList = null;

            return Json(ViewBag.EmptyContList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeLists(string PartyCode, int Page)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetImpPaymentPayeeForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }





        /*
                [HttpGet]
                public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
                {
                    Wfld_ImportRepository objImport = new Wfld_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, 0);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

                [HttpGet]
                public JsonResult LoadPartyLists(string PartyCode, int Page)
                {
                    Wfld_ImportRepository objImport = new Wfld_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, Page);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

        */





        [HttpGet]
        public JsonResult PartyBinding()
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.GetPaymentPartyForImportInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetEmptyCont(string InvoiceFor, int AppraisementId)
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            //objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            objImport.GetEmptyContByEntryId(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId,
            List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, string SEZ,string MovementType)
        {

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId, SEZ, MovementType);
            var Output = (VIZInvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;

            Output.Module = "EC";
            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                if (!Output.BOEDate.Contains(item.BOEDate))
                    Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new VIZPostPaymentContainer
                    {
                        CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        StuffCUM = item.StuffCUM
                    });
                }


                Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
                Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
                Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                    + Output.lstPrePaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
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
                Output.InvoiceAmt =Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Math.Ceiling(Output.InvoiceAmt) - Output.AllTotal;
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECDeliveryPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<VIZInvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
                string SEZ = "";
                SEZ = objForm["SEZ1"].ToString();

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
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "EC";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                VIZ_ImportRepository objImport = new VIZ_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objImport.AddEditEmptyContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor, SEZ);


                invoiceData.InvoiceNo = Convert.ToString(objImport.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                objImport.DBResponse.Data = invoiceData;
                return Json(objImport.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region Destuffing LCL
        [HttpGet]

        public ActionResult CreateDestuffingEntry()
        {

            VIZ_ExportRepository ObjER = new VIZ_ExportRepository();
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            VIZDestuffingEntry ObjDestuffing = new VIZDestuffingEntry();
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            ObjIR.GetContrNoForDestuffingEntry(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ObjIR.DBResponse.Data));

            }
            else
            {
                ViewBag.ContainerList = null;
            }

            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            

            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.VIZGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            return PartialView("GetDestuffingEntry", ObjDestuffing);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(int TallySheetId, String CFSCode)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            ObjIR.GetContrDetForDestuffingEntry(TallySheetId, Convert.ToInt32(Session["BranchId"]), CFSCode);

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOBLDetForDestuffingEntry(int TallySheetId, String CFSCode)
        {
            VIZ_ImportRepository ObjOBL = new VIZ_ImportRepository();
            ObjOBL.GetOBLforDestuffingEntry(TallySheetId, Convert.ToInt32(Session["BranchId"]), CFSCode);
            return Json(ObjOBL.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReservationValidate(int ImporterId, string DestuffingDate)
        {


            VIZ_ImportRepository objResValidate = new VIZ_ImportRepository();
            objResValidate.GetReservationValidate(ImporterId, DestuffingDate);
            return Json(objResValidate.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntry(int Page)
        {
            VIZ_ImportRepository ObjCR = new VIZ_ImportRepository();
            List<VIZ_DestuffingList> LstJO = new List<VIZ_DestuffingList>();
            ObjCR.GetAllDestuffingEntry(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<VIZ_DestuffingList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            List<VIZ_DestuffingList> LstDestuffing = new List<VIZ_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<VIZ_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {

            VIZDestuffingEntry ObjDestuffing = new VIZDestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                VIZ_ExportRepository ObjER = new VIZ_ExportRepository();
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.ListOfCHA();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                }
                else
                {
                    ViewBag.CHAList = null;
                }
                ObjER.GetAllCommodity();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;
                ObjIR.ListOfShippingLine();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "Edit");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (VIZDestuffingEntry)ObjIR.DBResponse.Data;
                }

                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.VIZGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }

            }
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView("EditDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingEntry(int DestuffingEntryId)
        {
            VIZDestuffingEntry ObjDestuffing = new VIZDestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "View");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (VIZDestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntry", ObjDestuffing);
        }



        [HttpGet]

        public JsonResult GetGodownLocationById(int GodownId)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.GetGodownLocationById(GodownId);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public JsonResult ListOfGodownData()
        {
            VIZ_ImportRepository objImport = new VIZ_ImportRepository();
            objImport.ListOfGodownRights(((Login)(Session["LoginUser"])).Uid);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            List<VIZGodownList> objImp = new List<VIZGodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<VIZGodownList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingEntry(VIZDestuffingEntry ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                List<VIZ_DestuffingEntryDtl> LstDestuffingEntry = new List<VIZ_DestuffingEntryDtl>();
                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VIZ_DestuffingEntryDtl>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                }
                //string DestufGodownEntryXML = "";
                //List<WFLD_DestufGodownDetails> LstDestufGodownEntry = new List<WFLD_DestufGodownDetails>();
                //if(ObjDestuffing.DestufGodownEntryXML!=null)
                //{
                //    LstDestufGodownEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_DestufGodownDetails>>(ObjDestuffing.DestufGodownEntryXML);
                //    DestufGodownEntryXML = Utility.CreateXML(LstDestufGodownEntry);
                //}

                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.AddEditDestuffingEntry(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]), Convert.ToInt32(((Login)(Session["LoginUser"])).Uid));
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDestuffingSheet(int DestuffingEntryId)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetDestuffEntryForPrint(DestuffingEntryId);
            if (ObjIR.DBResponse.Data != null)
            {
                VIZ_DestuffingSheet ObjDestuff = new VIZ_DestuffingSheet();
                ObjDestuff = (VIZ_DestuffingSheet)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForDestuffSheet(ObjDestuff, DestuffingEntryId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePDFForDestuffSheet(VIZ_DestuffingSheet ObjDestuff, int DestuffingEntryId)
        {
            CompanyDetailsForReport objCompanyDetailsVal = new CompanyDetailsForReport();
            VIZ_ReportRepository ObjRRR = new VIZ_ReportRepository();
            ObjRRR.getCompanyDetails();
            objCompanyDetailsVal = (CompanyDetailsForReport)ObjRRR.DBResponse.Data;
            CompanyEmail = objCompanyDetailsVal.EmailAddress;
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
            StringBuilder objSB = new StringBuilder();
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: right;' colspan='12'>");
            objSB.Append("<h1 style='font-size: 12px; line-height: 20px; font-weight: 300;margin: 0; padding: 0;'>");
            objSB.Append("</h1></td></tr>");

            //objSB.Append("<tr><td colspan='12'>");
            //objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            //objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            //objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 7pt;'>CFS Whitefield</label><br/><label style='font-size: 7pt; font-weight:bold;'>DESTUFFING SHEET</label></td></tr>");
            //objSB.Append("</tbody></table>");
            //objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
            objSB.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
            objSB.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>CFS Visakhapatnam</span><br/><label style='font-size: 7pt;'>Email - " + CompanyEmail + "</label>");
            objSB.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>DESTUFFING SHEET</label>");
            objSB.Append("</td>");
            objSB.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt;'><tbody>");
            objSB.Append("<tr><td style='width:70%;'><b>SHED CODE :</b>" + ObjDestuff.GodownName + "</td> <td style='width:30%; text-align:right;'><b>AS ON :</b>" + ObjDestuff.DestuffingEntryDateTime + "</td></tr>");
            //objSB.Append("<th style='text-align:right;'>AS ON:</th><td style='font-size:12px; width:10%;'>" + ObjDestuff.DestuffingEntryDateTime + "</td>");
            objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td style='text-align: left;'>");
            //objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>SHED CODE: <label>" + ObjDestuff.GodownName + "</label>");
            //objSB.Append("</span></td><td colspan='3' style='text-align: center;'>");
            //objSB.Append("<span style='display: block; font-size: 14px; line-height: 22px;  padding-bottom: 10px; font-weight:bold;'>DESTUFFING SHEET</span>");
            //objSB.Append("</td><td colspan='2' style='text-align: left;'><span style='display: block; font-size: 11px; padding-bottom: 10px;'>");
            //objSB.Append("AS ON: <label>" + ObjDestuff.DestuffingEntryDateTime + "</label></span></td></tr>");
            objSB.Append("<tr><td><span><br/></span></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellpadding='0' style='width:100%; margin: 0; margin-bottom: 10px; font-size:7pt;'><tbody>");
            objSB.Append("<tr><td colspan='2' width='35%'><label style='font-weight: bold;'>DESTUFF SHEET NO.:</label> <span>" + ObjDestuff.DestuffingEntryNo + "</span></td>");
            objSB.Append("<td colspan='4' width='40%'><label style='font-weight: bold;'>START DATE OF DESTUFFING : </label> <span>" + ObjDestuff.StartDate + "</span></td>");
            objSB.Append("<td colspan='5' width='31.5%'><label style='font-weight: bold;'>FINAL DATE OF DESTUFFING : </label> <span>" + ObjDestuff.DestuffingEntryDate + "</span></td></tr>");
            objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td colspan='12'><table style='width:100%; margin: 0; font-size:7pt;'><tbody>");
            //objSB.Append("<tr><td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><b>Container / CBT No.</b> <span>" + ObjDestuff.ContainerNo + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><b>Size : </b> <span>" + ObjDestuff.Size + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><b>In Date : </b> <span>" + ObjDestuff.GateInDate + "</span></td>");
            //objSB.Append("<td colspan='3' style='font-size: 7pt; padding-bottom:15px;'><b>Custom Seal No. : </b> <span>" + ObjDestuff.CustomSealNo + "</span></td>");
            //objSB.Append("<td colspan='3' style='font-size: 7pt; padding-bottom:15px;'><b>Sla Seal no. : </b> <span>" + ObjDestuff.SlaSealNo + "</span></td></tr>");
            //objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td colspan='12'><table style='width:100%; margin: 0; margin-bottom: 10px; font-size:7pt;'><tbody>");
            //objSB.Append("<tr><td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>ICD Code</label> <span>" + ObjDestuff.CFSCode + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>IGM No. : </label> <span>" + ObjDestuff.IGMNo + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>OBL Type: </label> <span>" + ObjDestuff.MovementType + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>SLA : </label> <span>" + ObjDestuff.ShippingLine + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>POL : </label> <span>" + ObjDestuff.POL + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 7pt; padding-bottom:15px;'><label style='font-weight: bold;'>POD : </label> <span>" + ObjDestuff.POD + "</span></td></tr>");
            //objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%; margin-bottom: 10px; font-size:7pt; border:1px solid #000; border-bottom:0; border-collapse:collapse;'>");
            objSB.Append("<thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Container / CBT No</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 5%;'>Size</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>In Date</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Customs Seal No</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Sla Seal No</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>ICD Code</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>IGM No</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>OBL Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>SLA</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>POL</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; width: 8%;'>POD</th>");
            objSB.Append("</tr></thead>");

            objSB.Append("<tbody>");
            objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.ContainerNo + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.Size + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.GateInDate + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.CustomSealNo + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.SlaSealNo + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.CFSCode + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.IGMNo + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.MovementType + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.ShippingLine + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ObjDestuff.POL + "</td>");
            objSB.Append("<td style='border-bottom: 1px solid #000;'>" + ObjDestuff.POD + "</td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody></table></td></tr>");


            objSB.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='text-align: center; border:1px solid #000; font-size:7pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 3%;'>#</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>TSA No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Item / Line No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>OBL No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>SLA</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>Importer</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 13%;'>Cargo Description</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>PKG Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Gen / Haz</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>CBM</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>No Of Pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Pkg Rec</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Un Reserved Wt</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Reserved Wt</th>");

            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 6%;'>Slot No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>UnReserved Area</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 8%;'>Reserved Area</th>");

            objSB.Append("<th style=' border-bottom: 1px solid #000; width: 13%;'>Remarks</th>");
            objSB.Append("</tr></thead>");
            objSB.Append("<tfoot><tr>");
            objSB.Append("<td colspan='9' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-weight: bold;'></td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.CBM)) + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.NoOfPkg)) + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.PkgRec)) + "</td>");
            objSB.Append("<td colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Weight)) + "</td>");
            objSB.Append("<td colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.RWeight)) + "</td>");
            objSB.Append("<td colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");

            objSB.Append("<td colspan='1' style='border-right: 1px solid #000;border-bottom: 1px solid #000; text-align: center;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Area)) + "</td>");
            objSB.Append("<td colspan='1' style='border-right: 1px solid #000;border-bottom: 1px solid #000; text-align: center;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.revArea)) + "</td>");
            objSB.Append("<td colspan='1' style='border-right: 1px solid #000;border-bottom: 1px solid #000; text-align: center;'></td>");

            objSB.Append("</tr></tfoot>");
            objSB.Append("<tbody>");
            int Serial = 1;
            ObjDestuff.lstDtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Serial + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.TSANO + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.LineNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.OblNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.ShippingLine + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Importer + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Cargo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Type + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.GeneralHazardous + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.CBM + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.NoOfPkg + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.PkgRec + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Weight + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.RWeight + "</td>");

                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.GodownWiseLctnNames + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Area + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.revArea + "</td>");

                objSB.Append("<td style='border-bottom: 1px solid #000;'>" + item.Remarks.Replace("&", " and ").ToString() + "</td>");
                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12' style='font-size: 7pt; padding-top: 15px; text-align: left;'><b>*GOODS RECEIVED ON SAID TO CONTAIN AND SAID TO WEIGHT BASIS -CWC IS NOT RESPONSIBLE FOR DAMAGES, SHORT LANDING AND LEAKAGES IF ANY IN TRANSIT FROM DESPATCHING END.</b></td></tr>");
            objSB.Append("<tr><th colspan='12' style='font-size: 7pt; text-align: left;padding-top: 15px;'>Signature &amp; Designation :</th></tr></tbody>");
            objSB.Append("</table></td></tr>");
            objSB.Append("<tr><th colspan='2' style='font-size: 7pt; width: 20%; text-align: center;padding-top: 100px;'>H &amp; T Agent</th>");
            objSB.Append("<th colspan='2' style='font-size: 7pt; width: 20%; text-align: center;padding-top: 100px;'>Consignee</th>");
            objSB.Append("<th colspan='2' style='font-size: 7pt; width: 20%; text-align: center;padding-top: 100px;'>Shipping Line</th>");
            objSB.Append("<th colspan='2' style='font-size: 7pt; width: 20%; text-align: center;padding-top: 100px;'>CWC <br/>CFS</th>");
            objSB.Append("<th colspan='2' style='font-size: 7pt; width: 20%; text-align: center;padding-top: 100px;'>Customs</th></tr>");
            objSB.Append("</tbody></table>");
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            VIZ_ReportRepository ObjRR = new VIZ_ReportRepository();
            ObjRR.getCompanyDetails();
            string HeadOffice = "", HOAddress = "", ZonalOffice = "", ZOAddress = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                ZonalOffice = objCompanyDetails.CompanyName;
                ZOAddress = objCompanyDetails.CompanyAddress;
            }

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            objSB.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 20f, 20f, 20f, false, true))
            {
                RH.HeadOffice = HeadOffice;
                RH.HOAddress = HOAddress;
                RH.ZonalOffice = ZonalOffice;
                RH.ZOAddress = ZOAddress;

                //RH.HeadOffice = ZonalOffice;
                //RH.HOAddress = "(A Govt.Of India Undertaking)";
                //RH.ZonalOffice = ZOAddress;
                //RH.ZOAddress = "";

                RH.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
        }
        [HttpGet]
        public JsonResult GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            VIZ_ImportRepository objIR = new VIZ_ImportRepository();
            objIR.GetCIFandDutyForBOE(BOENo, BOEDate);
            if (objIR.DBResponse.Data != null)
                return Json(new { Status = 1, Message = "Success", Data = objIR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = 0, Message = "No Data", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            VIZ_ExportRepository objRepo = new VIZ_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            VIZ_ExportRepository objRepo = new VIZ_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryListSearch(string ContainerNo)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            List<VIZ_DestuffingList> LstDestuffing = new List<VIZ_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(((Login)(Session["LoginUser"])).Uid, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<VIZ_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }
        #endregion
        #region Custom Appraisement Application
        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            VIZ_CustomAppraisement ObjAppraisement = new VIZ_CustomAppraisement();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjAppraisement.AppraisementDateCheck = DateTime.Now.ToString("MM/dd/yyyy");

            ObjIR.GetContnrNoForCustomAppraise();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<VIZ_CustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
            }
            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjIR.ListOfImporterCustForPage("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.ImpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstImporter = null;
            }
            return PartialView("CreateCustomAppraisement", ObjAppraisement);


        }

    

        [HttpGet]
        public JsonResult SearchByPartyCodeForCustApr(string PartyCode)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeListForCustApr(string PartyCode, int Page)
        {
            VIZ_ImportRepository objRepo = new VIZ_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetBOEDetail(String BOENo)
        {
            VIZ_CustomAppraisement objOBLContainer = new VIZ_CustomAppraisement();
            VIZ_ImportRepository rep = new VIZ_ImportRepository();
            rep.GetBOEDetail(BOENo);
            VIZ_CustomAppraisementBOECont obj = (VIZ_CustomAppraisementBOECont)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOBLContainer(String OBlNo)
        {
            VIZ_CustomAppraisement objOBLContainer = new VIZ_CustomAppraisement();
            VIZ_ImportRepository rep = new VIZ_ImportRepository();
            rep.GetOBLContainer(OBlNo);
            if (rep.DBResponse.Data != null)
            {
                List<VIZ_CustomAppraisementOBLCont> obj = (List<VIZ_CustomAppraisementOBLCont>)rep.DBResponse.Data;
                if (obj.Count > 0)
                {
                    ViewBag.ContainerList = new SelectList((List<VIZ_CustomAppraisementOBLCont>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                }
                else
                {
                    rep.GetContnrNoForCustomAppraise();
                    if (rep.DBResponse.Data != null)
                    {
                        ViewBag.ContainerList = new SelectList((List<VIZ_CustomAppraisementDtl>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                    }
                }
            }
            return Json(objOBLContainer, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetBOEDetail(String BOENo)
        //{
        //    Wfld_ImportRepository rep = new Wfld_ImportRepository();
        //    rep.GetBOEDetail(BOENo);
        //    PPGCustomAppraisementBOECont obj = (PPGCustomAppraisementBOECont)rep.DBResponse.Data;
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetContainerBOL(String CFSCode)
        {
           VIZ_CustomAppraisement objOBLContainer = new VIZ_CustomAppraisement();
            VIZ_ImportRepository rep = new VIZ_ImportRepository();
            rep.GetContainerOBL(CFSCode);
            List<VIZ_CustomAppraisementOBLCont> obj = (List<VIZ_CustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode, string LineNo)
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.DelCustomAppraisement(CustomAppraisementId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCustomAppraisement(VIZ_CustomAppraisement ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                string CAOrdXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<VIZ_CustomAppraisementDtl> LstAppraisement = JsonConvert.DeserializeObject<List<VIZ_CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                if (ObjAppraisement.CAOrdDtlXml != null)
                {
                    List<VIZ_CustomAppraisementOrdDtl> LstCAOrd = JsonConvert.DeserializeObject<List<VIZ_CustomAppraisementOrdDtl>>(ObjAppraisement.CAOrdDtlXml);
                    CAOrdXML = Utility.CreateXML(LstCAOrd);
                }

                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCustomAppraisement(ObjAppraisement, AppraisementXML, CAOrdXML);
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ViewCustomAppraisement(int CustomAppraisementId)
        {
            VIZ_CustomAppraisement ObjAppraisement = new VIZ_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (VIZ_CustomAppraisement)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/ViewCustomAppraisement.cshtml", ObjAppraisement);
        }


        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            VIZ_CustomAppraisement ObjAppraisement = new VIZ_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (VIZ_CustomAppraisement)ObjIR.DBResponse.Data;
                    ObjIR.ListOfCHA();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                    }
                    else
                    {
                        ViewBag.CHAList = null;
                    }
                    ObjIR.ListOfShippingLine();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                    }
                    else
                    {
                        ViewBag.ShippingLineList = null;
                    }

                    ObjIR.ListOfImporterForm();
                    if (ObjIR.DBResponse.Data != null)
                        ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/EditCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public ActionResult GetCustomAppraisementList()
        {
            VIZ_ImportRepository ObjIR = new VIZ_ImportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.MenuRights = Jobjectt["lstMenu"];
            }


            List<VIZ_CustomAppraisement> LstAppraisement = new List<VIZ_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<VIZ_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }
        #endregion
    }
}