using CwcExim.Areas.Export.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using EinvoiceLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Auction.Controllers
{
    public class Kol_AuctionInvoiceController : BaseController
    {
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }
        // GET: Auction/Kol_AuctionInvoice

        public Kol_AuctionInvoiceController()
        {
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            EffectVersion = Convert.ToDecimal(objCompanyDetails.EffectVersion);
            EffectVersionLogoFile = objCompanyDetails.VersionLogoFile.ToString();
        }
        public ActionResult CreateAuctionInvoice()
        {
            Kol_AuctionRepository objImport = new Kol_AuctionRepository();
            
            objImport.GetAppraismentRequestForPaymentSheet(0);
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            ImportRepository objImport1 = new ImportRepository();
            objImport1.GetPaymentParty();
            if (objImport1.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport1.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId, string Type = "I")
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId, Type);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAuctionCharges(string InvoiceDate, string InvoiceType, string SEZ, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName, List<Import.Models.PaymentSheetContainer> lstPaySheetContainer,decimal OtherCharges, int GSTPer,string CargoDesc,string HSNCode,string DeliveryDate,  int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            Kol_AuctionRepository objChrgRepo = new Kol_AuctionRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetYardPaymentSheet(InvoiceDate, InvoiceType, AppraisementId, DeliveryType, SEZ, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, XMLText, OtherCharges, GSTPer, CargoDesc, HSNCode);
            var Output = (WFLD_ExpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "Auction";
            Output.InvoiceType = "Tax";
            Output.PayeeId = PayeeId;
            Output.PayeeName = PayeeName;
            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WFLD_ExpContainer();
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


                

                //Output.InvoiceAmt = (Output.lstPostPaymentChrg.Sum(o => o.Total));
                //Output.RoundUp = 0;
            });
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
            return Json(Output, JsonRequestBehavior.AllowGet);
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult AddEditAuctionInvoice(FormCollection objForm )
        {
            var invoiceData = JsonConvert.DeserializeObject<WFLD_ExpPaymentSheet>(objForm["PaymentSheetModelJson"]);
            string ContainerXML = "";
            string ChargesXML = "";
            string ContWiseCharg = "";
            string OperationCfsCodeWiseAmtXML = "";

            //foreach (var item in invoiceData.lstPSCont)
            //{
            //    item.StuffingDate = Convert.ToDateTime(item.StuffingDate).ToString("yyyy-MM-dd");
            //    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //foreach (var item in invoiceData.lstOperationContwiseAmt)
            //{
            //    if (item.DocumentDate != "")
            //        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
            //}


            invoiceData.SEZ = Convert.ToString(objForm["SEZ"]);
            invoiceData.Remarks = Convert.ToString(objForm["Remarks"]);

            string CargoDesc= Convert.ToString(objForm["CargoDescription"]);
            string HSNCode= Convert.ToString(objForm["HSNCode"]);
            int GSTPer= Convert.ToInt32(objForm["GST"]);



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
            Kol_AuctionRepository objChargeMaster = new Kol_AuctionRepository();
            objChargeMaster.AddEditAuctionInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, 1, ((Login)(Session["LoginUser"])).Uid, "Auc",CargoDesc,HSNCode,GSTPer);

            /*invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
            invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");*/
            invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


            objChargeMaster.DBResponse.Data = invoiceData;
            return Json(objChargeMaster.DBResponse);
            
        }


        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {



            Kol_AuctionRepository objPpgRepo = new Kol_AuctionRepository();
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





    }
}