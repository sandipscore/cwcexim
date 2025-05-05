using CwcExim.Areas.Export.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CwcExim.Filters;
using SCMTRLibrary;
using System.Data;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CwcExim.Areas.Export.Controllers
{
    public class Kdl_CWCExportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Payment Sheet

        [HttpGet]
        public ActionResult CreatePaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ExportRepository objExport = new ExportRepository();
            objExport.GetStuffingRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int StuffingReqId)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetContainerForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer,decimal Weighment=0, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetExportPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML, InvoiceId,Weighment);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total);
            Output.HTTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            return Json(Output);
        }

        [NonAction]
        private void CalculateCWCCharges(PaymentSheetFinalModel finalModel, PaySheetChargeDetails baseModel)
        {
            try
            {
                //var A = JsonConvert.DeserializeObject<PaySheetChargeDetails>("{   \"lstPSContainer\": [     {       \"CFSCode\": \"CFSCode8\",       \"ContainerNo\": \"CONT1234\",       \"Size\": \"20\",       \"IsReefer\": false,       \"Insured\": \"No\"     },     {       \"CFSCode\": \"CFSCode9\",       \"ContainerNo\": \"CONT0001\",       \"Size\": \"40\",       \"IsReefer\": false,       \"Insured\": \"Yes\"     }   ],   \"lstEmptyGR\": [     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"lstLoadedGR\": [     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 3,       \"RentAmount\": 0.10,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 4,       \"DaysRangeTo\": 15,       \"RentAmount\": 380.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 16,       \"DaysRangeTo\": 999,       \"RentAmount\": 500.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"InsuranceRate\": 12.15,   \"lstStorageRent\": [     {       \"CFSCode\": \"CFSCode9\",       \"ActualCUM\": 1500.00,       \"ActualSQM\": 1800.00,       \"ActualWeight\": 10000.00,       \"StuffCUM\": 225.000,       \"StuffSQM\": 270.000,       \"StuffWeight\": 1500.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     },     {       \"CFSCode\": \"CFSCode8\",       \"ActualCUM\": 5000.00,       \"ActualSQM\": 2500.00,       \"ActualWeight\": 5000.00,       \"StuffCUM\": 2000.000,       \"StuffSQM\": 1000.000,       \"StuffWeight\": 2000.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     }   ],   \"RateSQMPerWeek\": 456.00,   \"RateSQMPerMonth\": 56.00,   \"RateCUMPerWeek\": 4566.00,   \"RateMTPerDay\": 56.00 }");
                var EGRAmt = 0m;
                baseModel.lstEmptyGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                {
                    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                    {
                        var grp = x.GroundRentPeriod;
                        var drf = x.DaysRangeFrom;
                        var drt = x.DaysRangeTo;
                        if (grp >= drt)
                        {
                            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                        }
                        else
                        {
                            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                            break;
                        }
                    }
                });
                baseModel.lstLoadedGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                {
                    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                    {
                        var grp = x.GroundRentPeriod;
                        var drf = x.DaysRangeFrom;
                        var drt = x.DaysRangeTo;
                        if (grp >= drt)
                        {
                            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                        }
                        else
                        {
                            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                            break;
                        }
                    }
                });
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Amount = EGRAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Ground Rent").Total = EGRAmt;

                var STAmt = 0m;
                baseModel.lstStorageRent.ToList().ForEach(item =>
                {
                    var Amt1 = item.StuffCUM * item.StorageWeeks * baseModel.RateCUMPerWeek;
                    var Amt2 = item.StuffWeight * item.StorageDays * baseModel.RateMTPerDay;
                    var Amt3 = item.StorageDays < 30 ? (item.StuffSQM * item.StorageWeeks * baseModel.RateSQMPerWeek)
                                                    : ((item.StuffSQM * item.StorageMonths * baseModel.RateSQMPerMonth) + (item.StuffSQM * item.StorageMonthWeeks * baseModel.RateSQMPerWeek));
                    STAmt += Enumerable.Max(new[] { Amt1, Amt2, Amt3 });
                });
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Storage Charge").Amount = STAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Storage Charge").Total = STAmt;

                var INSAmt = 0m;
                baseModel.lstInsuranceCharges.Where(o => o.IsInsured == 0).ToList().ForEach(item =>
                {
                    INSAmt += item.FOB * baseModel.InsuranceRate * item.StorageWeeks;
                });
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Insurance").Amount = INSAmt;
                finalModel.lstChargesType.FirstOrDefault(o => o.ChargeName == "Insurance").Total = INSAmt;

            }
            catch (Exception e)
            {

            }

            //return finalModel;
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

                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXP", ExportUnder,"");
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

        #region Tentative Invoice
        [HttpGet]
        public ActionResult TentativeExpInvoice()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CreatePaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ExportRepository objExport = new ExportRepository();
            objExport.GetStuffingRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }
        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            StuffingRequest ObjSR = new StuffingRequest();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetCartRegNoForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            else
                ViewBag.ShippingLineList = null;
            ObjER.GetAllContainerNo();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ContainerList = new SelectList((List<StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
            else
                ViewBag.ContainerList = null;
            /*If User is External Or Non CWC User*/
            bool Exporter, CHA;
            Exporter = ((Login)Session["LoginUser"]).Exporter;
            CHA = ((Login)Session["LoginUser"]).CHA;
            if (CHA == true)
            {
                ObjSR.CHA = ((Login)Session["LoginUser"]).Name;
                ObjSR.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
            }
            else
            {
                ObjER.ListOfCHA();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                else
                    ViewBag.ListOfCHA = null;
            }
            if (Exporter == true)
            {
                ViewData["IsExporter"] = true;
                ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
            }
            else
            {
                ViewData["IsExporter"] = false;
                ObjER.ListOfExporter();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                else
                    ViewBag.ListOfExporter = null;
            }

            ObjER.ListOfPackUQCForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            else
            {
                ViewBag.lstPackUQC = null;
            }

            ObjSR.RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView("CreateStuffingRequest", ObjSR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStuffingReq(StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                ExportRepository ObjER = new ExportRepository();
                IList<StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
                string StuffingXML = Utility.CreateXML(LstStuffing);
                string StuffingContrXML = Utility.CreateXML(LstStuffConatiner);
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditStuffingRequest(ObjStuffing, StuffingXML, StuffingContrXML);
                ModelState.Clear();
                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditStuffingRequest(int StuffinfgReqId)
        {
            StuffingRequest ObjStuffing = new StuffingRequest();
            ExportRepository ObjER = new ExportRepository();
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            else
                ViewBag.CHAList = null;
            ObjER.ListOfExporter();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
            //else
            //    ViewBag.ListOfExporter = null;
            //ObjER.ListOfCHA();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            //else
            //    ViewBag.ListOfCHA = null;
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            else
                ViewBag.ShippingLineList = null;
            if (StuffinfgReqId > 0)
            {
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (StuffingRequest)ObjER.DBResponse.Data;
                }
                /*If User is External Or Non CWC User*/
                bool Exporter, CHA;
                Exporter = ((Login)Session["LoginUser"]).Exporter;
                CHA = ((Login)Session["LoginUser"]).CHA;
                if (CHA == true)
                {
                    ViewData["IsCHA"] = true;
                    ViewData["CHA"] = ((Login)Session["LoginUser"]).Name;
                    ViewData["CHAId"] = ((Login)Session["LoginUser"]).EximTraderId;
                    //ObjStuffing.CHA = ((Login)Session["LoginUser"]).Name;
                    // ObjStuffing.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                }
                else
                {
                    ViewData["IsCHA"] = false;
                    ObjER.ListOfCHA();
                    if (ObjER.DBResponse.Data != null)
                        ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                    else
                        ViewBag.ListOfCHA = null;
                }
                if (Exporter == true)
                {
                    ViewData["IsExporter"] = true;
                    ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                    ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
                }
                else
                {
                    ViewData["IsExporter"] = false;
                    ObjER.ListOfExporter();
                    if (ObjER.DBResponse.Data != null)
                        ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                    else
                        ViewBag.ListOfExporter = null;
                }

                ObjER.ListOfPackUQCForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                    ViewBag.PackUQCState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstPackUQC = null;
                }
            }
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            StuffingRequest ObjStuffing = new StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.DeleteStuffingRequest(StuffinfgReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetCartRegDetForStuffingReq(int CartingRegisterId)
        {
            ExportRepository objER = new ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Kdl_GetCartRegDetForStuffingReq(CartingRegisterId);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            ExportRepository ObjER = new ExportRepository();
            List<StuffingRequest> LstStuffing = new List<StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }
        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            ExportRepository ObjER = new ExportRepository();
            // StuffingReqContainerDtl ObjSRD = new StuffingReqContainerDtl();
            ObjER.GetContainerNoDet(CFSCode);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ObjSRD = (StuffingReqContainerDtl)ObjER.DBResponse.Data;
            //}
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Stuffing
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            ExportRepository ObjER = new ExportRepository();
            ContainerStuffing ObjCS = new ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            ObjER.GetReqNoForContainerStuffing();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = new SelectList((List<ContainerStuffing>)ObjER.DBResponse.Data, "StuffingReqId", "StuffingReqNo");
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
            }
            else
            {
                ViewBag.ListOfExporter = null;
            }
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            return PartialView("/" + "/Areas/Export/Views/Kdl_CWCExport/CreateContainerStuffing.cshtml", ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqDtlId, string CFSCode)
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqDtlId, CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<ContainerStuffing> LstStuffing = new List<ContainerStuffing>();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetAllContainerStuffing();
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("/Areas/Export/Views/Kdl_CWCExport/ContainerStuffingList.cshtml", LstStuffing);
        }
        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            ContainerStuffing ObjStuffing = new ContainerStuffing();
            ExportRepository ObjER = new ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            ExportRepository ObjER = new ExportRepository();
            ContainerStuffing ObjStuffing = new ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (ContainerStuffing)ObjER.DBResponse.Data;
                }
                ObjER.ListOfCHA();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                }
                else
                {
                    ViewBag.CHAList = null;
                }
                ObjER.ListOfExporter();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                }
                else
                {
                    ViewBag.ListOfExporter = null;
                }
                ObjER.GetShippingLine();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(ContainerStuffing ObjStuffing)
        {
            ModelState.Remove("HandalingPartyCode");
            ModelState.Remove("STOPartyCode");
            ModelState.Remove("INSPartyCode");
            ModelState.Remove("GREPartyCode");
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    IList<ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerStuffingDtl>>(ObjStuffing.StuffingXML);

                    var LstStuffingSBSorted = from c in LstStuffing group c by c.ShippingBillNo into grp select grp.Key;



                    // LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

                    foreach (var a in LstStuffingSBSorted)
                    {
                        int vPaketTo = 0;
                        int vPaketFrom = 1;
                        foreach (var i in LstStuffing)
                        {

                            if (i.ShippingBillNo == a)
                            {
                                vPaketTo = vPaketTo + i.StuffQuantity;
                                i.PacketsTo = vPaketTo;
                                i.PacketsFrom = vPaketFrom;
                                vPaketFrom = 1 + vPaketTo;
                            }
                        }

                    }

                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }
                ExportRepository ObjER = new ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrMsg };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteContainerStuffingDet(int ContainerStuffingId)
        {
            if (ContainerStuffingId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.DeleteContainerStuffing(ContainerStuffingId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintContainerStuffing(int ContainerStuffingId)
        {
            ExportRepository ObjER = new ExportRepository();
            ContainerStuffing ObjStuffing = new ContainerStuffing();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [NonAction]
        public string GeneratePdfForContainerStuff(ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "", EquipmentSealType="";
            int SerialNo = 1;
            if (ObjStuffing.LstStuffingDtl.Count() > 0)
            {
                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    if (ShippingBillNo == "")
                        ShippingBillNo = item.ShippingBillNo;
                    else
                        ShippingBillNo += "," + item.ShippingBillNo;
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
                {
                    if (ShippingDate == "")
                        ShippingDate = item.ShippingDate;
                    else
                        ShippingDate += "," + item.ShippingDate;
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                {
                    if (ShippingLine == "")
                        ShippingLine = item.ShippingLine;
                    else
                        ShippingLine += "," + item.ShippingLine;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                {
                    if (CHA == "")
                        CHA = item.CHA;
                    else
                        CHA += "," + item.CHA;
                });

                StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                StuffQuantity = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity)).ToString() : "";
                ObjStuffing.LstStuffingDtl.ToList().ForEach(item =>
                {
                    SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : (CFSCode + "<br/>" + item.CFSCode));
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                    CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : (CustomSeal + "<br/>" + item.CustomSeal));
                    EquipmentSealType = (EquipmentSealType == "" ? (item.EquipmentSealType) : (EquipmentSealType + "<br/>" + item.EquipmentSealType));
                    Commodity = (Commodity == "" ? (item.CommodityName) : (Commodity + "<br/>" + item.CommodityName));
                    SerialNo++;
                });
                SLNo.Remove(SLNo.Length - 1);
            }

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            if (Convert.ToInt32(Session["BranchId"]) == 1)
            {
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station-Kandla Port<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr>    <tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Final Destination Location</td><td style='border:1px solid #000;padding:3px;text-align:left;'>"+ObjStuffing.FinalDestinationLocation+"</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'></td><td style='border:1px solid #000;padding:3px;text-align:left;'></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'></td><td style='border:1px solid #000;padding:3px;text-align:left;'></td></tr> </tbody> </table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Equipment Seal Type</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td> <td style='border:1px solid #000;padding:3px;text-align:left;'>"+ EquipmentSealType + "</td> <td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr>  <tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Final Destination Location</td><td style='border:1px solid #000;padding:3px;text-align:left;'>"+ ObjStuffing.FinalDestinationLocation + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'></td><td style='border:1px solid #000;padding:3px;text-align:left;'></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'></td><td style='border:1px solid #000;padding:3px;text-align:left;'></td></tr> </tbody> </table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Equipment Seal Type</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td> <td style='border:1px solid #000;padding:3px;text-align:left;'>"+ EquipmentSealType + "</td> <td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
            // Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        }
        public JsonResult GetFinalDestination(string CustodianCode)
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.ListOfFinalDestination(CustodianCode);
            List<Kdl_FinalDestination> lstFinalDestination = new List<Kdl_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<Kdl_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Stuffing Amendment

        [HttpGet]
        public ActionResult AmendmentContainerStuffing()
        {
            ExportRepository ObjER = new ExportRepository();

            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
            }
            else
            {
                ViewBag.ListOfExporter = null;
            }
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            return PartialView("AmendmentContainerStuffing");
        }

        [HttpGet]
        public JsonResult GetStuffingNoForAmendment()
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.ListOfStuffingNoForAmendment();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetStuffingDetailsForAmendment(int ContainerStuffingId)
        {
            ExportRepository ObjER = new ExportRepository();
            Kdl_ContainerStuffing ObjStuffing = new Kdl_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingDetails(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Kdl_ContainerStuffing)ObjER.DBResponse.Data;
                }

            }

            return Json(ObjStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditAmendmentContainerStuffing(Kdl_ContainerStuffing ObjStuffing)
        {
            ModelState.Remove("HandalingPartyCode");
            ModelState.Remove("STOPartyCode");
            ModelState.Remove("INSPartyCode");
            ModelState.Remove("GREPartyCode");
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";

                if (ObjStuffing.StuffingXML != null)
                {
                    List<ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                ExportRepository ObjER = new ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditAmendmentContainerStuffing(ObjStuffing, ContainerStuffingXML);

                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrMsg };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult AmendmentContainerStuffingList(string SearchValue = "")
        {
            List<Kdl_ContainerStuffing> LstStuffing = new List<Kdl_ContainerStuffing>();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(0, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Kdl_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView(LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadAmendmentContainerStuffingList(int Page, string SearchValue = "")
        {
            List<Kdl_ContainerStuffing> LstStuffing = new List<Kdl_ContainerStuffing>();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(Page, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Kdl_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stuffing Work Order

        [HttpGet]
        public ActionResult CreateStuffingWorkOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddStuffingWorkOrder()
        {
            StuffingWorkOrder objWorkOrder = new StuffingWorkOrder();
            objWorkOrder.WorkOrderDate = DateTime.Now.ToString("dd/MM/yyyy");

            ExportRepository objExport = new ExportRepository();
            objExport.GetStuffingRequestList(0);
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstStuffingNoList = (List<StuffingNoList>)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingNoList != null)
                objWorkOrder.StuffingNoListJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingNoList);

            objExport.ListOfGodown();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstGodownList = (List<GodownList>)objExport.DBResponse.Data;
            objExport.ListOfCommodity();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstCommodity = (List<Export.Models.Commodity>)objExport.DBResponse.Data;

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public ActionResult GetStuffingWorkOrderList()
        {
            List<StuffingWorkOrder> lstWorkOrder = new List<StuffingWorkOrder>();
            ExportRepository objExport = new ExportRepository();
            objExport.GetStuffingWorkOrder();
            if (objExport.DBResponse.Data != null)
                lstWorkOrder = (List<StuffingWorkOrder>)objExport.DBResponse.Data;

            return PartialView(lstWorkOrder);
        }

        [HttpGet]
        public ActionResult EditStuffingWorkOrder(int WorkOrderID)
        {
            StuffingWorkOrder objWorkOrder = new StuffingWorkOrder();
            ExportRepository objExport = new ExportRepository();

            objExport.GetStuffingWorkOrderById(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder = (StuffingWorkOrder)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingWorkOrderDtl != null)
                objWorkOrder.StuffingWorkOrderDtlJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingWorkOrderDtl);

            /*objExport.GetStuffingRequestList(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstStuffingNoList = (List<StuffingNoList>)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingNoList != null)
                objWorkOrder.StuffingNoListJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingNoList);

            objExport.ListOfGodown();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstGodownList = (List<GodownList>)objExport.DBResponse.Data;
            objExport.ListOfCommodity();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstCommodity = (List<Export.Models.Commodity>)objExport.DBResponse.Data;*/

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewStuffingWorkOrder(int WorkOrderID)
        {
            StuffingWorkOrder objWorkOrder = new StuffingWorkOrder();
            ExportRepository objExport = new ExportRepository();

            objExport.GetStuffingWorkOrderById(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder = (StuffingWorkOrder)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingWorkOrderDtl != null)
                objWorkOrder.StuffingWorkOrderDtlJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingWorkOrderDtl);

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public JsonResult GetContainerListByStuffingReqId(int StuffingReqID)
        {
            try
            {
                ExportRepository objExport = new ExportRepository();
                if (StuffingReqID > 0)
                    objExport.GetContainerListByStuffingReqId(StuffingReqID);
                return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditStuffingWorkOrder(StuffingWorkOrder objStuffing)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objStuffing.lstStuffingWorkOrderDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<StuffingWorkOrderDtl>>(objStuffing.StuffingWorkOrderDtlJS);
                    string XML = Utility.CreateXML(objStuffing.lstStuffingWorkOrderDtl);
                    ExportRepository objExport = new ExportRepository();
                    objExport.AddEditStuffingWorkOrder(objStuffing, XML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                    ModelState.Clear();
                    return Json(objExport.DBResponse);
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
        public JsonResult DeleteStuffingWorkOrder(int WorkOrderID)
        {
            try
            {
                ExportRepository objExport = new ExportRepository();
                if (WorkOrderID > 0)
                    objExport.DeleteStuffingWorkOrder(WorkOrderID);
                return Json(objExport.DBResponse);
            }
            catch (Exception)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PreviewStuffingWorkOrder(int StuffingWorkOrderId)
        {
            if (StuffingWorkOrderId > 0)
            {
                string Path = "";
                ExportRepository objER = new ExportRepository();
                List<StuffingWOPrint> lstWODetails = new List<StuffingWOPrint>();
                objER.GetDetailsStufffingWOForPrint(StuffingWorkOrderId);
                if (objER.DBResponse.Data != null)
                {
                    lstWODetails = (List<StuffingWOPrint>)objER.DBResponse.Data;
                    Path = GeneratePdfForStuffingWO(lstWODetails, StuffingWorkOrderId);
                }
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePdfForStuffingWO(List<StuffingWOPrint> lstWO, int StuffingWorkOrderId)
        {
            string CHAName = "", ExpName = "";
            string ContainerNumber = "";
            string CompanyAddress = lstWO[0].CompanyAddress;
            string WODate = DateTime.Now.ToString("dd/MM/yyyy");
            int NoOfUnit = lstWO.Select(x => x.NoOfUnits).Sum();
            string GrossWeight = lstWO.Select(x => x.Weight).Sum().ToString() + " KG";
            string WorkOrderNo = lstWO[0].WorkOrderNo.ToString();
            string To = "";
            lstWO.Select(x => new { CHAName = x.CHAName }).Distinct().ToList().ForEach(item =>
            {
                if (CHAName == "")
                    CHAName = item.CHAName;
                else
                    CHAName += " ," + item.CHAName;
            });
            lstWO.Select(x => new { ExpName = x.ExpName }).Distinct().ToList().ForEach(item =>
            {
                if (ExpName == "")
                    ExpName = item.ExpName;
                else
                    ExpName += " ," + item.ExpName;
            });
            lstWO.Select(x => new { Container = x.ContainerNo + "-" + x.Size }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerNumber == "")
                    ContainerNumber = item.Container;
                else
                    ContainerNumber += " ," + item.Container;
            });
            if (Convert.ToInt32(Session["BranchId"]) == 1)
                To = "M/S Abrar Forwarders<br/>Gandhidham Kutch";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CWorkOrder" + StuffingWorkOrderId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + WorkOrderNo + " </th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,<br/>" + To + "</th><th style='text-align:right;'>Dated: " + WODate + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ExpName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Stuffing " + lstWO[0].StuffingReqNo + "</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfUnit + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: </td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : " + ContainerNumber + " </td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/CWorkOrder" + StuffingWorkOrderId + ".pdf";
        }
        #endregion

        #region BTT Payment Sheet
        [HttpGet]
        public ActionResult CreateBTTPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ExportRepository objExport = new ExportRepository();
            objExport.GetCartingApplicationForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetShipBillNo(int StuffingReqId)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetShipBillForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer
            ,decimal Weighment=0, int InvoiceId=0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetBTTPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, SEZ,StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML,InvoiceId, Weighment);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total);
            Output.HTTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            return Json(Output);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBTTPaymentSheet(FormCollection objForm)
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


                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", ExportUnder,"");
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

        #region Loaded Container Invoice
        [HttpGet]
        public ActionResult CreateLoadedContainerPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ExportRepository objExport = new ExportRepository();
            objExport.GetLoadedContainerRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public JsonResult GetLoadedPaymentSheetContainer(int StuffingReqId)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetContainerForLoadedContainerPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string SEZ, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer,Decimal Weightment=0, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetLoadedContPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML, InvoiceId, Weightment);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total);
            Output.HTTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            return Json(Output);
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
                string SEZ = Convert.ToString(objForm["SEZValue"]);

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                invoiceData.RoundUp = Convert.ToDecimal(objForm["RoundUp"]);

                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod", SEZ,"");
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

        #region Shipping Bill Amendment
        [HttpGet]
        public ActionResult ShippingBillAmendment()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult LoadShippingList()
        {
            ExportRepository objER = new ExportRepository();
            List<ShippingBillList> objList = new List<ShippingBillList>();
            objER.ListOfShippingBill();
            if (objER.DBResponse.Data != null)
                objList = (List<ShippingBillList>)objER.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateShippingBillAmendment(Shipbill objSB)
        {
            ExportRepository objER = new ExportRepository();
            if (ModelState.IsValid)
            {
                objER.AddShippingBillLog(objSB);
                return Json(objER.DBResponse);
            }
            return Json(new { Status = -1, Message = "Error" });
        }
        #endregion

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            ExportRepository objExp = new ExportRepository();
            objExp.GetContStuffingForApproval();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExp.GetPortOfCallForPage("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPortOfCall = Jobject["lstPortOfCall"];
                ViewBag.StatePortOfCall = Jobject["StatePortOfCall"];
            }
            else
            {
                ViewBag.lstPortOfCall = null;
            }

            objExp.GetNextPortOfCallForPage("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstNextPortOfCall = Jobject["lstNextPortOfCall"];
                ViewBag.StateNextPortOFCall = Jobject["StateNextPortOFCall"];
            }
            else
            {
                ViewBag.lstNextPortOfCall = null;
            }


            return PartialView();
        }

        [HttpGet]
        public JsonResult SearchPortOfCallByPortCode(string PortCode)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                ExportRepository objCR = new ExportRepository();
                objCR.AddEditContainerStuffingApproval(objPortOfCall, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }


        [HttpGet]
        public ActionResult GetContainerStuffingApprovalList()
        {
            ExportRepository ObjER = new ExportRepository();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            ObjER.ListofContainerStuffingApproval(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfContainerStuffingApproval", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadContainerStuffingApprovalList(int Page)
        {
            ExportRepository ObjER = new ExportRepository();
            var LstStuffingApproval = new List<PortOfCall>();
            ObjER.ListofContainerStuffingApproval(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffingApproval(int ApprovalId)
        {
            PortOfCall ObjStuffing = new PortOfCall();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingApprovalSearch(string SearchValue = "")
        {
            ExportRepository ObjER = new ExportRepository();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            ObjER.GetAllContainerStuffingApprovalSearch(SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfContainerStuffingApproval", LstStuffingApproval);
        }

        #endregion

        #region Send SF       

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> SendSF(int ContainerStuffingId)
        {
            try
            {
                log.Error("SendSF Method Start .....");

                int k = 0;
                int j = 1;
                ExportRepository ObjER = new ExportRepository();

                log.Error("Repository Call .....");
                ObjER.GetCIMSFDetails(ContainerStuffingId, "F");
                log.Error("Repository Done .....");
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;
                    log.Error("Read File Name .....");

                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        string Filenm = Convert.ToString(dr["FileName"]);


                        log.Error("call Class Libarary File .....");
                        string JsonFile = StuffingSBJsonFormat.ContStuffingDetJson(ds, Convert.ToString(dr["ContainerNo"]));

                        if (JsonFile == "")
                        {
                            throw new InvalidOperationException("");
                        }
                        
                        log.Error("call Class Libarary DOne .....");


                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMSF"];

                        log.Error("Done Ppg_ReportfileCIMSF .....");
                        string FileName = strFolderName + Filenm;
                        if (!Directory.Exists(strFolderName))
                        {
                            Directory.CreateDirectory(strFolderName);
                        }


                        System.IO.File.Create(FileName).Dispose();

                        System.IO.File.WriteAllText(FileName, JsonFile);
                        string output = "";

                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFile"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFile"];
                        string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                        string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                        string DSCPASSWORD = Convert.ToString(ds.Tables[6].Rows[0]["DSCPASSWORD"]);

                        log.Error("Done All key  .....");
                        if (!Directory.Exists(OUTJsonFile))
                        {
                            Directory.CreateDirectory(OUTJsonFile);
                        }

                        DECSignedModel decSignedModel = new DECSignedModel();
                        decSignedModel.InJsonFile = InJsonFile + Filenm;
                        decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                        decSignedModel.ArchiveInJsonFile = "No";
                        decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                        decSignedModel.DSCPATH = DSCPATH;
                        decSignedModel.DSCPASSWORD = DSCPASSWORD;

                        string FinalOutPutPath = OUTJsonFile + Filenm;

                        string StrJson = JsonConvert.SerializeObject(decSignedModel);

                        #endregion
                        log.Error("Json String Before submit:" + StrJson);

                        var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);

                            //For Block If Develpoment Done Then Unlock

                        }

                        log.Error("output: " + output);
                        //if (output == "Success")
                        //{
                            ExportRepository objExport = new ExportRepository();
                            objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                            //return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");
                    }


                    return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = "CIM SF File Send Fail." });
            }

            

        }

        #endregion

        #region  Send ASR 
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> SendASR(int ContainerStuffingId)
        {
            try
            {
                int k = 0;
                int j = 1;
                ExportRepository ObjER = new ExportRepository();

                ObjER.GetCIMASRDetails(ContainerStuffingId, "F");
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;

                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        string Filenm = Convert.ToString(dr["FileName"]);

                        string JsonFile = StuffingCIMACRJsonFormat.StuffingCIMACRJson(ds, Convert.ToInt32(dr["ContainerStuffingDtlId"]));
                        if (JsonFile == "")
                        {
                            throw new InvalidOperationException("");
                        }

                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMASR"];
                        log.Error("Done Ppg_ReportfileCIMASR .....");
                        string FileName = strFolderName + Filenm;
                        if (!Directory.Exists(strFolderName))
                        {
                            Directory.CreateDirectory(strFolderName);
                        }


                        System.IO.File.Create(FileName).Dispose();

                        System.IO.File.WriteAllText(FileName, JsonFile);
                        string output = "";

                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileASR"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileASR"];
                        string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                        string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                        string DSCPASSWORD = Convert.ToString(ds.Tables[7].Rows[0]["DSCPASSWORD"]);

                        log.Error("Done All key  .....");
                        if (!Directory.Exists(OUTJsonFile))
                        {
                            Directory.CreateDirectory(OUTJsonFile);
                        }

                        DECSignedModel decSignedModel = new DECSignedModel();
                        decSignedModel.InJsonFile = InJsonFile + Filenm;
                        decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                        decSignedModel.ArchiveInJsonFile = "No";
                        decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                        decSignedModel.DSCPATH = DSCPATH;
                        decSignedModel.DSCPASSWORD = DSCPASSWORD;

                        string FinalOutPutPath = OUTJsonFile + Filenm;

                        string StrJson = JsonConvert.SerializeObject(decSignedModel);

                        #endregion
                        log.Error("Json String Before submit:" + StrJson);

                        var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);

                            //For Block If Develpoment Done Then Unlock

                        }


                        log.Error("output: " + output);


                        //if (output == "Success")
                        //{
                        //    ExportRepository objExport = new ExportRepository();
                        //    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");

                    }
                    ExportRepository objExport = new ExportRepository();
                    objExport.GetCIMASRDetailsUpdateStatus(ContainerStuffingId);

                    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "CIM ASR File Send Fail." });
            }

        }

        #endregion

        #region LeoEntry
        public ActionResult LeoEntry()
        {
            return PartialView();
        }
        public JsonResult SearchMCIN(string SBNo, string SBDATE)
        {
            ExportRepository objRepo = new ExportRepository();
            objRepo.GetMCIN(SBNo, SBDATE);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLEO(LEOPage objLEOPage)
        {


            ExportRepository objER = new ExportRepository();

            //  objCCINEntry.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
            //  string PostPaymentXML = Utility.CreateXML(objCCINEntry.lstPostPaymentChrg);


            // IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
            // string XML = Utility.CreateXML(PostPaymentChargeList);
            // objCCINEntry.PaymentSheetModelJson = XML;
            objER.AddEditLEOEntry(objLEOPage);
            return Json(objER.DBResponse);
        }


        [HttpGet]
        public ActionResult ListOfLEO()
        {
            ExportRepository objER = new ExportRepository();
            List<LEOPage> lstLEOPage = new List<LEOPage>();
            objER.GetAllLEOEntryForPage();
            if (objER.DBResponse.Data != null)
                lstLEOPage = (List<LEOPage>)objER.DBResponse.Data;
            return PartialView(lstLEOPage);
        }




        [HttpGet]
        public ActionResult EditLEO(int Id)
        {
            LEOPage ObjLEOPage = new LEOPage();
            ExportRepository ObjER = new ExportRepository();





            if (Id > 0)
            {
                ObjER.GetAllLEOEntryBYID(Id);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjLEOPage = (LEOPage)ObjER.DBResponse.Data;
                }

            }
            return PartialView("EditLEO", ObjLEOPage);
        }

        [HttpGet]
        public ActionResult ViewLEO(int Id)
        {
            LEOPage ObjLEOPage = new LEOPage();
            ExportRepository ObjER = new ExportRepository();





            if (Id > 0)
            {
                ObjER.GetAllLEOEntryBYID(Id);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjLEOPage = (LEOPage)ObjER.DBResponse.Data;
                }

            }
            return PartialView("ViewLEO", ObjLEOPage);
        }



        [HttpGet]
        public ActionResult LEOSERCH(string SearchValue)
        {
            ExportRepository objER = new ExportRepository();
            List<LEOPage> lstLEOPage = new List<LEOPage>();
            objER.GetAllLEOEntryBYSBMCIN(SearchValue);
            if (objER.DBResponse.Data != null)
                lstLEOPage = (List<LEOPage>)objER.DBResponse.Data;
            return PartialView("ListOfLEO", lstLEOPage);
        }


        [HttpGet]
        public JsonResult LoadMoreLEO(int Page, string SearchValue)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> LstJO = new List<WFLD_CCINEntry>();
            ObjCR.GetAllCCINEntryForPage(Page, SearchValue);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLD_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteLEO(int CCINEntryId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }
        #endregion


        #region  Loaded Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateLoadContainerStuffingApproval()
        {
            ExportRepository objExp = new ExportRepository();
            PortOfCall ObjPC = new PortOfCall();
            ObjPC.ApprovalDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            objExp.GetLoadedContainerStuffingForApproval();
            if (objExp.DBResponse.Status > 0)
                ViewBag.LoadContainerReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.LoadContainerReqList = null;

            objExp.GetPortOfCallForPage("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPortOfCall = Jobject["lstPortOfCall"];
                ViewBag.StatePortOfCall = Jobject["StatePortOfCall"];
            }
            else
            {
                ViewBag.lstPortOfCall = null;
            }

            objExp.GetNextPortOfCallForPage("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstNextPortOfCall = Jobject["lstNextPortOfCall"];
                ViewBag.StateNextPortOFCall = Jobject["StateNextPortOFCall"];
            }
            else
            {
                ViewBag.lstNextPortOfCall = null;
            }


            return PartialView(ObjPC);
        }

        [HttpGet]
        public ActionResult EditLoadContainerStuffingApproval(int ApprovalId)
        {
            PortOfCall ObjStuffing = new PortOfCall();

            if (ApprovalId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.GetLoadContainerStuffingApprovalById(ApprovalId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                ExportRepository objCR = new ExportRepository();
                objCR.AddEditLoadContainerStuffingApproval(objPortOfCall, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }


        [HttpGet]
        public ActionResult GetLoadContainerStuffingApprovalList(string SearchValue = "")
        {
            ExportRepository ObjER = new ExportRepository();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            ObjER.ListofLoadContainerStuffingApproval(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingApproval", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingApprovalList(int Page, string SearchValue = "")
        {
            ExportRepository ObjER = new ExportRepository();
            var LstStuffingApproval = new List<PortOfCall>();
            ObjER.ListofLoadContainerStuffingApproval(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingApproval(int ApprovalId)
        {
            PortOfCall ObjStuffing = new PortOfCall();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetLoadContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion        

        #region  Loaded Container Send ASR 
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> LoadedContainerSendASR(int ContainerStuffingId)
        {

            try
            {
                int k = 0;
                int j = 1;
                ExportRepository ObjER = new ExportRepository();
                //PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
                ObjER.GetLoadedContainerCIMASRDetails(ContainerStuffingId, "F");
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;

                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        string Filenm = Convert.ToString(dr["FileName"]);

                        string JsonFile = StuffingCIMACRJsonFormat.StuffingCIMACRJson(ds, Convert.ToInt32(dr["ContainerStuffingDtlId"]));
                        if (JsonFile == "")
                        {
                            throw new InvalidOperationException("Logfile cannot be read-only");
                        }




                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMASR"];
                        log.Error("Done Ppg_ReportfileCIMSF .....");
                        string FileName = strFolderName + Filenm;
                        if (!Directory.Exists(strFolderName))
                        {
                            Directory.CreateDirectory(strFolderName);
                        }


                        System.IO.File.Create(FileName).Dispose();

                        System.IO.File.WriteAllText(FileName, JsonFile);
                        string output = "";

                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileASR"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileASR"];
                        string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                        string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                        string DSCPASSWORD = Convert.ToString(ds.Tables[7].Rows[0]["DSCPASSWORD"]);

                        log.Error("Done All key  .....");
                        if (!Directory.Exists(OUTJsonFile))
                        {
                            Directory.CreateDirectory(OUTJsonFile);
                        }

                        DECSignedModel decSignedModel = new DECSignedModel();
                        decSignedModel.InJsonFile = InJsonFile + Filenm;
                        decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                        decSignedModel.ArchiveInJsonFile = "No";
                        decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                        decSignedModel.DSCPATH = DSCPATH;
                        decSignedModel.DSCPASSWORD = DSCPASSWORD;

                        string FinalOutPutPath = OUTJsonFile + Filenm;

                        string StrJson = JsonConvert.SerializeObject(decSignedModel);

                        #endregion
                        log.Error("Json String Before submit:" + StrJson);

                        var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);

                            //File Upload Fail
                            //if (status.ToString() == "Success")
                            //{
                            //    using (FileStream fs = System.IO.File.OpenRead(FinalOutPutPath))
                            //    {
                            //        log.Error("File Open:" + OUTJsonFile);
                            //        log.Info("FTP File read process has began");
                            //        byte[] buffer = new byte[fs.Length];
                            //        fs.Read(buffer, 0, buffer.Length);
                            //        fs.Close();
                            //        string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
                            //        log.Error("SCMTRPath:" + SCMTRPath);
                            //        // log.Error("SCMTR File Name:" + OUTJsonFile+ Filenm);
                            //        output = FtpFileManager.UploadFileToFtp(SCMTRPath, Filenm, buffer, "5000", FinalOutPutPath);
                            //        log.Info("FTP File read process has ended");
                            //    }
                            //}
                            //else
                            //{
                            //    output = "Error";
                            //}

                            //For Block If Develpoment Done Then Unlock

                        }






                        log.Error("output: " + output);









                        //if (output == "Success")
                        //{
                        //    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                        //    objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");

                    }
                    Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);

                    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "CIM ASR File Send Fail." });
            }





        }

        #endregion


        #region ACTUAL ARRIVAL DATE AND TIME 
        [HttpGet]
        public ActionResult ActualArrivalDateTime()
        {
            ExportRepository ObjER = new ExportRepository();
           Kdl_ActualArrivalDatetime objActualArrival = new Kdl_ActualArrivalDatetime();

            ObjER.GetContainerNoForActualArrival("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ContainerNoList = Jobject["ContainerList"];
                ViewBag.StateCont = Jobject["State"];
            }
            else
            {
                ViewBag.ContainerNoList = null;
            }
            objActualArrival.ArrivalDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return PartialView(objActualArrival);
        }
        [HttpGet]
        public JsonResult LoadArrivalDatetimeContainerList(string ContainerBoxSearch, int Page)
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchContainer(string ContainerBoxSearch, int Page)
        {
           ExportRepository ObjER = new ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditActualArrivalDatetime(Kdl_ActualArrivalDatetime objActualArrivalDatetime)
        {
            ExportRepository ObjER = new ExportRepository();
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime dateValue;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!DateTime.TryParseExact(objActualArrivalDatetime.ArrivalDateTime, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out dateValue))
                    {
                        return Json(new { Status = -1, Message = "Invallid Date format" });
                    }

                    ObjER.AddEditActualArrivalDatetime(objActualArrivalDatetime);
                }

                return Json(ObjER.DBResponse);

            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ListOfArrivalDatetime()
        {
            List<Kdl_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Kdl_ActualArrivalDatetime>();
            ExportRepository objER = new ExportRepository();
            //objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, 0);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Kdl_ActualArrivalDatetime>)objER.DBResponse.Data;
            return PartialView(lstActualArrivalDatetime);
        }

        [HttpGet]
        public JsonResult EditActualArrivalDatetime(int actualArrivalDatetimeId)
        {
            List<Kdl_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Kdl_ActualArrivalDatetime>();
            ExportRepository objER = new ExportRepository();
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, actualArrivalDatetimeId);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Kdl_ActualArrivalDatetime>)objER.DBResponse.Data;
            //return Json(lstActualArrivalDatetime);
            return Json(lstActualArrivalDatetime, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SendAT(string CFSCode)
        {


            ExportRepository ObjER = new ExportRepository();
            ObjER.GetATDetails(CFSCode, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                string Filenm = Convert.ToString(ds.Tables[7].Rows[0]["FileName"]);

                string JsonFile = ATJsonFormat.ATJsonCreation(ds);
                // string Filenm = Convert.ToString(ds.Tables[7].Rows[0]["FileName"]);



                string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMAT"];
                string FileName = strFolderName + Filenm;
                if (!Directory.Exists(strFolderName))
                {
                    Directory.CreateDirectory(strFolderName);
                }


                System.IO.File.Create(FileName).Dispose();

                System.IO.File.WriteAllText(FileName, JsonFile);
                string output = "";
                #region Digital Signature

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileAT"];
                string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileAT"];
                string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                string DSCPASSWORD = Convert.ToString(ds.Tables[8].Rows[0]["DSCPASSWORD"]);

                log.Error("Done All key  .....");
                if (!Directory.Exists(OUTJsonFile))
                {
                    Directory.CreateDirectory(OUTJsonFile);
                }

                DECSignedModel decSignedModel = new DECSignedModel();
                decSignedModel.InJsonFile = InJsonFile + Filenm;
                decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                decSignedModel.ArchiveInJsonFile = "No";
                decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                decSignedModel.DSCPATH = DSCPATH;
                decSignedModel.DSCPASSWORD = DSCPASSWORD;

                string FinalOutPutPath = OUTJsonFile + Filenm;
                log.Error("Json String Before SerializeObject:");

                string StrJson = JsonConvert.SerializeObject(decSignedModel);
                log.Error("Json String After SerializeObject:" + StrJson);

                #endregion
                log.Error("Json String Before submit:" + StrJson);

                var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    try
                    {
                        log.Error("Json String Before Post api url:" + apiUrl);
                        HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                        log.Error("Json String after Post:");
                        string content = await response.Content.ReadAsStringAsync();
                        log.Error("After Return Response:" + content);
                        //log.Info(content);
                        JObject joResponse = JObject.Parse(content);
                        log.Error("After Return Response Value:" + joResponse);
                        var status = joResponse["Status"];
                        log.Error("Status:" + status);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.StackTrace + ":" + ex.Message);
                    }




                }



                return Json(new { Status = 1, Message = "CIM AT File Send Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Send Loaded SF

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> SendLoadedSF(int ContainerStuffingId)
        {
            try
            {
                log.Error("SendSF Method Start .....");

                int k = 0;
                int j = 1;
                ExportRepository ObjER = new ExportRepository();
                // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
                log.Error("Repository Call .....");
                ObjER.GetLoadedCIMSFDetails(ContainerStuffingId, "F");
                log.Error("Repository Done .....");
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;
                    log.Error("Read File Name .....");
                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        string Filenm = Convert.ToString(dr["FileName"]);


                        log.Error("call Class Libarary File .....");
                        string JsonFile = StuffingSBJsonFormat.ContStuffingDetJson(ds, Convert.ToString(dr["ContainerNo"]));
                        if (JsonFile == "")
                        {
                            throw new InvalidOperationException("Logfile cannot be read-only");
                        }
                        log.Error("call Class Libarary DOne .....");


                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMSF"];

                        log.Error("Done Ppg_ReportfileCIMSF .....");
                        string FileName = strFolderName + Filenm;
                        if (!Directory.Exists(strFolderName))
                        {
                            Directory.CreateDirectory(strFolderName);
                        }


                        System.IO.File.Create(FileName).Dispose();

                        System.IO.File.WriteAllText(FileName, JsonFile);
                        string output = "";




                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFile"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFile"];
                        string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                        string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                        string DSCPASSWORD = Convert.ToString(ds.Tables[6].Rows[0]["DSCPASSWORD"]);

                        log.Error("Done All key  .....");
                        if (!Directory.Exists(OUTJsonFile))
                        {
                            Directory.CreateDirectory(OUTJsonFile);
                        }

                        DECSignedModel decSignedModel = new DECSignedModel();
                        decSignedModel.InJsonFile = InJsonFile + Filenm;
                        decSignedModel.OUTJsonFile = OUTJsonFile + Filenm;
                        decSignedModel.ArchiveInJsonFile = "No";
                        decSignedModel.ArchiveInJsonFilePath = ArchiveInJsonFilePath;
                        decSignedModel.DSCPATH = DSCPATH;
                        decSignedModel.DSCPASSWORD = DSCPASSWORD;

                        string FinalOutPutPath = OUTJsonFile + Filenm;

                        string StrJson = JsonConvert.SerializeObject(decSignedModel);

                        #endregion
                        log.Error("Json String Before submit:" + StrJson);

                        var data = new StringContent(StrJson, Encoding.UTF8, "application/json");
                        using (var client = new HttpClient())
                        {
                            log.Error("Json String Before Post api url:" + apiUrl);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            log.Error("Json String after Post:");
                            string content = await response.Content.ReadAsStringAsync();
                            log.Error("After Return Response:" + content);
                            //log.Info(content);
                            JObject joResponse = JObject.Parse(content);
                            log.Error("After Return Response Value:" + joResponse);
                            var status = joResponse["Status"];
                            log.Error("Status:" + status);




                            //For Block If Develpoment Done Then Unlock

                        }







                        log.Error("output: " + output);
                        //  if (output == "Success")
                        // {
                        ExportRepository objExport = new ExportRepository();
                        objExport.GetLoadedCIMSFDetailsUpdateStatus(ContainerStuffingId);
                        //return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                        //  }
                    }
                    log.Info("FTP File upload has been end");
                    return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });

                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "CIM SF File Send Fail." });

            }

        }
        #endregion

        #region  Loaded Container SF Send

        [HttpGet]
        public ActionResult CreateLoadContainerSF()
        {
            ExportRepository objExp = new ExportRepository();
            Kdl_LoadContSF ObjPC = new Kdl_LoadContSF();
            objExp.GetLoadedContainerStuffingForSF();
            if (objExp.DBResponse.Status > 0)
                ViewBag.LoadContainerReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.LoadContainerReqList = null;

            return PartialView(ObjPC);
        }

        [HttpGet]
        public ActionResult EditLoadContainerSF(int LoadContReqId)
        {
            Kdl_LoadContSF ObjStuffing = new Kdl_LoadContSF();

            if (LoadContReqId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.GetLoadContainerStuffingSFById(LoadContReqId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Kdl_LoadContSF)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingSF(Kdl_LoadContSF objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                ExportRepository objCR = new ExportRepository();
                objCR.AddEditLoadContainerStuffingSF(objPortOfCall, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }


        [HttpGet]
        public ActionResult GetLoadContainerStuffingSFList(string SearchValue = "")
        {
            ExportRepository ObjER = new ExportRepository();
            List<Kdl_LoadContSF> LstStuffingApproval = new List<Kdl_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Kdl_LoadContSF>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingSF", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingSFList(int Page, string SearchValue = "")
        {
            ExportRepository ObjER = new ExportRepository();
            var LstStuffingApproval = new List<Kdl_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Kdl_LoadContSF>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingSF(int ApprovalId)
        {
            Kdl_LoadContSF ObjStuffing = new Kdl_LoadContSF();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetLoadContainerStuffingSFById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Kdl_LoadContSF)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion        





    }
}