using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using CwcExim.Filters;
using System.IO;
using System.Web;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using CwcExim.Areas.Master.Models;
using System.Text;
using CwcExim.Areas.GateOperation.Models;
using CwcExim.Areas.Report.Models;
using System.Threading;
using EinvoiceLibrary;
using System.Threading.Tasks;   

namespace CwcExim.Areas.Import.Controllers
{
    public class Hdb_CWCImportController : BaseController
    {

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public Hdb_CWCImportController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Hdb_ReportRepository ObjRR = new Hdb_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;


        }
        #region Yard Payment Sheet

        [HttpGet]
        public ActionResult CreateContPaymentSheet(string type = "Tax")

        {
            ViewData["InvType"] = type;
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
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

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult GetContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
        //    int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST,
        //    int PayeeId, string PayeeName, List<Hdb_PaymentSheetContainer> lstPaySheetContainer,
        //    int InvoiceId = 0)
        //public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,
        //    List<PaymentSheetContainer> lstPaySheetContainer,
        //    int InvoiceId = 0)
        //{
        //    string XMLText = "";
        //    if (lstPaySheetContainer != null)
        //    {
        //        XMLText = Utility.CreateXML(lstPaySheetContainer);
        //    }

        //    //ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
        //    HDB_ImportRepository objImport = new HDB_ImportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    objChrgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
        //            PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceId);
        //    var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
        //    Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
        //    Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
        //    Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
        //    Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
        //    Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
        //    Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
        //    Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
        //    Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
        //    Output.CWCTDS = 0;
        //    Output.HTTDS = 0;
        //    Output.TDS = 0;
        //    Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
        //    Output.RoundUp = 0;
        //    Output.InvoiceAmt = Output.AllTotal;
        //    /**********************BOL PRINT****************/
        //    var BOL = "";
        //    objChrgRepo.GetBOLForEmptyCont("Yard", Output.RequestId);
        //    if (objChrgRepo.DBResponse.Status == 1)
        //        BOL = objChrgRepo.DBResponse.Data.ToString();
        //    /***********************************************/
        //    return Json(new { Output, BOL });
        //}

        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, int PartyId,
            List<Hdb_PaymentSheetContainer> lstPaySheetContainer, int CasualLabour,string ExportUnder,string Printing,int Distance=0,
            int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            HDB_ImportRepository objHdbRepo = new HDB_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objHdbRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, InvoiceId, PartyId, CasualLabour,ExportUnder, Printing,Distance);
            var Output = (Hdb_InvoiceYard)objHdbRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
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
                    Output.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        StuffCUM = item.StuffCUM,
                        ISODC=item.ISODC
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
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        private void CalculateCWCChargesContainer(Hdb_PaymentSheetFinalModel finalModel, Hdb_PaySheetChargeDetails baseModel)
        {
            try
            {
                //var A = JsonConvert.DeserializeObject<PaySheetChargeDetails>("{   \"lstPSContainer\": [     {       \"CFSCode\": \"CFSCode8\",       \"ContainerNo\": \"CONT1234\",       \"Size\": \"20\",       \"IsReefer\": false,       \"Insured\": \"No\"     },     {       \"CFSCode\": \"CFSCode9\",       \"ContainerNo\": \"CONT0001\",       \"Size\": \"40\",       \"IsReefer\": false,       \"Insured\": \"Yes\"     }   ],   \"lstEmptyGR\": [     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"lstLoadedGR\": [     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 3,       \"RentAmount\": 0.10,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 4,       \"DaysRangeTo\": 15,       \"RentAmount\": 380.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 16,       \"DaysRangeTo\": 999,       \"RentAmount\": 500.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"InsuranceRate\": 12.15,   \"lstStorageRent\": [     {       \"CFSCode\": \"CFSCode9\",       \"ActualCUM\": 1500.00,       \"ActualSQM\": 1800.00,       \"ActualWeight\": 10000.00,       \"StuffCUM\": 225.000,       \"StuffSQM\": 270.000,       \"StuffWeight\": 1500.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     },     {       \"CFSCode\": \"CFSCode8\",       \"ActualCUM\": 5000.00,       \"ActualSQM\": 2500.00,       \"ActualWeight\": 5000.00,       \"StuffCUM\": 2000.000,       \"StuffSQM\": 1000.000,       \"StuffWeight\": 2000.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     }   ],   \"RateSQMPerWeek\": 456.00,   \"RateSQMPerMonth\": 56.00,   \"RateCUMPerWeek\": 4566.00,   \"RateMTPerDay\": 56.00 }");
                var EGRAmt = 0m;
                //baseModel.lstEmptyGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                //{
                //    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                //    {
                //        var grp = x.GroundRentPeriod;
                //        var drf = x.DaysRangeFrom;
                //        var drt = x.DaysRangeTo;
                //        if (grp >= drt)
                //        {
                //            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                //        }
                //        else
                //        {
                //            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                //            break;
                //        }
                //    }
                //});
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
                //baseModel.lstStorageRent.ToList().ForEach(item =>
                //{
                //    var Amt1 = item.StuffCUM * item.StorageWeeks * baseModel.RateCUMPerWeek;
                //    var Amt2 = item.StuffWeight * item.StorageDays * baseModel.RateMTPerDay;
                //    var Amt3 = item.StorageDays < 30 ? (item.StuffSQM * item.StorageWeeks * baseModel.RateSQMPerWeek)
                //                                    : ((item.StuffSQM * item.StorageMonths * baseModel.RateSQMPerMonth) + (item.StuffSQM * item.StorageMonthWeeks * baseModel.RateSQMPerWeek));
                //    STAmt += Enumerable.Max(new[] { Amt1, Amt2, Amt3 });
                //});
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

                var invoiceData = JsonConvert.DeserializeObject<Hdb_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult InvoicePrint(string InvoiceNo)
        {
            Hdb_ReportRepository objGPR = new Hdb_ReportRepository();
            objGPR.GetInvoiceDetailsPrintByNo(InvoiceNo, "IMPYard");
            if (objGPR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)objGPR.DBResponse.Data;
                PpgInvoiceYard objGP = new PpgInvoiceYard();
                string FilePath = "";
                FilePath = GeneratingInvoicePrint(ds, "IMPYard");
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        public string GeneratingInvoicePrint(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            var distinctValues = ds.Tables[3].AsEnumerable()
                         .Select(row => new
                         {
                             SACCode = row.Field<string>("SACCode")

                         })
                         .Distinct();



            List<string> lstSB = new List<string>();

            lstInvoice.ToList().ForEach(item =>
            {
                System.Text.StringBuilder html = new System.Text.StringBuilder();
                /*Header Part*/

                Decimal CTax = 0;
                if (item.TotalIGST > 0)
                {
                    CTax = item.TotalIGST;
                }
                else
                {
                    CTax = item.TotalCGST;
                }
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

                html.Append("<tr style='text-align: center;'><td colspan='2'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2></td></tr>");

                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td style='border:1px solid #333; border-bottom: none;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='vertical-align: top;'><img style='width: 100%;' src='logo.png'/></td>");
                html.Append("<td style='padding: 0 10px;'>");
                html.Append("<h1 style='font-size: 16px; margin:0; padding: 0;'>Central Warehousing Corporation</h1>");
                html.Append("<label style='font-size: 13px; text-transform:uppercase;'>(A Govt. of India Undertaking) </label>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>Principle Place of Business-RO Hyderabad,</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>1st Floor Warehousing Sadan,</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>Nampally, Behind Gandhi Bhavan</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>GSTIN/UIN: 36AAACC1206D2ZG</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>State Name: Telangana, Code: 36</h6>");
                html.Append("<h6 style='font-size: 11px; margin:0; font-weight:normal;'>Contact: 040-23773751,8142456715,9966483827,7337516165</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>E-Mail: cwccfs@gmail.com</h6>");
                html.Append("<h6 style='font-size: 12px; margin:0; font-weight:normal;'>www.cewacor.nic.in</h6>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("<td valign='top' cellspacing='0' cellpadding='0' style='border:1px solid #333; border-left: none; padding:0; border-bottom: none;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Invoice No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.InvoiceNo + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; padding: 10px; font-size: 12px;'>Dated<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.InvoiceDate + "</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Delivery Note<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Account</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Mode/Terms of Payment<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>15 Days</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Supplier's Ref.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Cfs Kukarpally</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Other Reference(s)<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase;margin: 0; padding: 0;'>Tn56h2594</h2></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");

                html.Append("<tr valign='top' cellpadding='0'>");
                html.Append("<td style='border:1px solid #333; border-bottom: none;'>");
                html.Append("<br/>");
                html.Append("<p style='display: block; font-size: 12px; margin:0; padding: 0 10px;'>Consignee</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; font-weight: bold; margin:0;'>" + item.PartyName + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'>" + item.PartyAddress + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'></p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>GSTIN/UIN:</span>" + item.PartyGSTNo + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; padding: 0 10px; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>Pan/It No:</span> </p>");
                html.Append("<p style='display: block; font-size: 13px; margin:0; padding: 0 10px;'><span style='display: inline-block; width: 100px;'>State Name:</span> " + item.PartyState + "</p>");
                html.Append("</td>");
                int i = 1;
                String cfscd = "";
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    cfscd = cfscd + elem.CFSCode + ",";
                });

                html.Append("<td valign='top' cellspacing='0' cellpadding='0' style='border:1px solid #333; border-left: 0; border-top: 0;'>");
                html.Append("<table cellspacing='0' style='width:100%;'><tbody>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top: 0; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Buyer's Order No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + cfscd + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top: 0; padding: 10px; font-size: 12px;'>Dated<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'></h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>Despatch Document No.<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Be No: " + item.BOENo + " ," + item.BOEDate + "</h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Delivery Note Date<br/><h2 style='font-size: 13px; padding-bottom: 10px; margin: 0; padding: 0;'>" + item.DeliveryDate + "</h2></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; border-right:1px solid #333; padding: 10px; font-size: 12px;'>despatched througn<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'></h2></td>");
                html.Append("<td valign='top' style='width:50%; border-top:1px solid #333; padding: 10px; font-size: 12px;'>Destination<br/><h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase;margin: 0; padding: 0;'></h2></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");

                html.Append("<tr>");
                html.Append("<td style='border:1px solid #333; padding: 10px;'>");
                html.Append("<p style='display: block; font-size: 12px; margin:0;'>Buyer(if other than consignee)</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; font-weight: bold; margin:0;'> " + item.PayeeName + " </p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'>" + item.PartyAddress + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'></p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'>" + item.PartyState + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>GSTIN/UIN:</span> " + item.PartyGSTNo + "</p>");
                html.Append("<p style='display: block; text-transform:uppercase; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>Pan/It No:</span> </p>");
                html.Append("<p style='display: block; font-size: 13px; margin:0;'><span style='display: inline-block; width: 100px;'>State Name:</span> " + item.PartyState + "</p>");
                html.Append("</td>");
                html.Append("<td style='border:1px solid #333; border-top: 0; border-left: none; padding:10px; vertical-align: top;'>");
                html.Append("<span style='display: block; font-size: 12px;'>Terms of Delivery</span>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + item.TotalNoOfPackages.ToString() + "</h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>" + item.TotalGrossWt.ToString("0.000") + "</h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>Godown no: </h2>");
                html.Append("<h2 style='font-size: 13px; padding-bottom: 10px; text-transform:uppercase; margin: 0; padding: 0;'>No of Grids: </h2>");
                html.Append("</td>");
                html.Append("</tr>");

                /***************/
                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; border-top: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>SL No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 300px;'>Particulars</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>HSN/SAC</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>GST Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Quantity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>per</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width:100px;'>Amount</th>");
                html.Append("</tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                Decimal cgstamt = 0;
                Decimal sgstamt = 0;
                Decimal igstamt = 0;
                Decimal tot = 0;
                String[] Sac;
                Sac = new string[50];
                Decimal[] Taxa;
                Decimal[] CenPer;
                Decimal[] StatePer;
                CenPer = new Decimal[50];
                StatePer = new Decimal[50];
                Taxa = new decimal[50];
                Decimal[] StateTax;
                StateTax = new decimal[50];
                Decimal[] CentralTax;
                CentralTax = new decimal[50];
                Decimal[] TotTax;
                TotTax = new decimal[50];
                Decimal Tamt = 0;
                Decimal Tcen = 0;
                Decimal Tstate = 0;
                Decimal TTax = 0;
                int count = 0;
                distinctValues.ToList().ForEach(data1 =>
                {
                    Sac[count] = data1.SACCode;
                    count++;
                });

                for (int ii = 0; ii < count; ii++)
                {
                    Taxa[ii] = 0;
                    CentralTax[ii] = 0;
                    StateTax[ii] = 0;
                    TotTax[ii] = 0;
                    lstCharges.ToList().ForEach(data =>
                    {

                        cgstamt = cgstamt + data.CGSTAmt;
                        sgstamt = sgstamt + data.SGSTAmt;
                        igstamt = igstamt + data.IGSTAmt;
                        if (data.SACCode == Sac[ii])
                        {
                            Taxa[ii] = Taxa[ii] + data.Taxable;

                            if (data.IGSTAmt > 0)
                            {
                                CenPer[ii] = data.IGSTPer;
                                StatePer[ii] = data.SGSTPer;
                                CentralTax[ii] = CentralTax[ii] + data.IGSTAmt;
                                StateTax[ii] = StateTax[ii] + (data.SGSTAmt + data.CGSTAmt);
                                TotTax[ii] = TotTax[ii] + (CentralTax[ii] + StateTax[ii]);
                                Tamt = Tamt + Taxa[ii];
                                Tcen = Tcen + CentralTax[ii];
                                Tstate = Tstate + StateTax[ii];
                                TTax = TTax + TotTax[ii];
                            }

                            else
                            {
                                CenPer[ii] = data.SGSTPer;
                                StatePer[ii] = data.CGSTPer;
                                CentralTax[ii] = CentralTax[ii] + data.SGSTAmt;
                                StateTax[ii] = StateTax[ii] + (data.CGSTAmt);
                                TotTax[ii] = TotTax[ii] + (CentralTax[ii] + StateTax[ii]);
                                Tamt = Tamt + Taxa[ii];
                                Tcen = Tcen + CentralTax[ii];
                                Tstate = Tstate + StateTax[ii];
                                TTax = TTax + TotTax[ii];

                            }
                        }
                    });
                }

                lstCharges.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    Decimal gstper = 0;
                    gstper = data.CGSTPer + data.SGSTPer;

                    tot = data.Total;
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left; padding:0 20px 0 60px; font-weight:bold;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + gstper + " %</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'></td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'></td>");
                        html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + data.Taxable.ToString("0") + "</td></tr>");
                        i = i + 1;
                    }
                });
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'> " + i.ToString() + " </td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>CGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalCGST.ToString("0") + "</td></tr>");
                i = i + 1;
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>SGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalSGST.ToString("0") + "</td></tr>");
                i = i + 1;
                html.Append("<tr><td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right; padding:0 20px 0 60px; font-weight:bold; text-transform:uppercase;'>IGST</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: right;'>9</td>");
                html.Append("<td style='border-right: 1px solid #000; font-size: 11px; text-align: left;'>%</td>");
                html.Append("<td style='font-size: 11px; text-align: right; font-weight: bold;'>" + item.TotalIGST.ToString("0") + "</td></tr>");

                html.Append("<tr><td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: right; padding:0 20px 0 60px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-top: 1px solid #000; border-bottom: 1px solid #000;  font-size: 13px; text-align: left;'></td>");
                html.Append("<td style=' border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 18px; text-align: right; font-weight: bold;'>" + item.InvoiceAmt.ToString("0") + " &#8377;</td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr>");

                html.Append("<tr cellspacing='0' cellpadding='0'><td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-top:0; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; border-bottom: 1px solid #000; text-align: left;'>Amount Chargeable(in words)<br/><span style='font-size: 13px; font-weight:bold;'>" + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</span></td>");
                html.Append("<td style='font-size: 13px; border-bottom: 1px solid #000; text-align: right; vertical-align: top;'><em>E. & o.E</em></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //Group OF CHARGES SECTION START
                // distinctValues.ToList().ForEach(item => item.SACCode=
                // {

                html.Append("<tr cellspacing='0' cellpadding='0'>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='border: 1px solid #000; border-top: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 160px;'>HSN/SAC</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Taxable Value</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Central Tax</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>State Tax</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Total Tax Amount</th>");
                html.Append("</tr>");

                html.Append("<tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                //int j = 1;
                /*Charges Bind*/
                // lstCharges.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(data =>
                //{

                for (int ii = 0; ii < count; ii++)
                {
                    if (Taxa[ii] > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; font-size: 12px; text-align: left;'>" + Sac[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + Taxa[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + CenPer[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + CentralTax[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + StatePer[ii] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; font-size: 12px; text-align: right;'>" + StateTax[ii] + " </td>");
                        html.Append("<td style='font-size: 12px; text-align: right;'>" + (CentralTax[ii] + StateTax[ii]) + "</td></tr>");
                    }
                }
                // });
                html.Append("<tr><td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>Total</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + item.TotalTaxable + "</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + (CTax) + "</td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'></td>");
                html.Append("<td style='border-top: 1px solid #000; border-right: 1px solid #000; font-weight: bold; font-size: 13px; text-align: right;'>" + (item.TotalSGST) + "</td>");
                html.Append("<td style='border-top: 1px solid #000; font-size: 13px; text-align: right; font-weight: bold;'>" + (item.TotalCGST + item.TotalSGST + item.TotalIGST) + "</td></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-left: 1px solid #000; border-right: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr style='margin: 0 0 40px;'>");
                html.Append("<td colspan='2' style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Tax Amount(in words) : <span style='font-size:14px; font-weight:bold;'>" + objCurr.changeCurrencyToWords((item.TotalCGST + item.TotalSGST + item.TotalIGST).ToString("0")) + "</span></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; border-left: 1px solid #333; border-right: 1px solid #333; border-bottom: 1px solid #333;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='2' style='padding:0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr>");
                html.Append("<td>");
                html.Append("<p style='font-size: 13px; margin:0; padding: 0 0 0 5px;'>Company's PAN: <b>AAACC1206D</b></p>");
                html.Append("<p style='font-size: 12px; margin:0; padding: 0 0 0 5px;'>Declaration</p>");
                html.Append("<p style='font-size: 12px; margin:0; padding: 0 0 0 5px;'>We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct</p>");
                html.Append("</td>");
                html.Append("<td style='padding-right: 0; padding-bottom: 0;'>");
                html.Append("<p style='font-size: 12px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>Bank Name:</span> Punjab National Bank</p>");
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>A/c No:</span> 4737002100000318</p>");
                html.Append("<p style='font-size: 13px; margin:0; font-weight: bold;'><span style='width: 150px; font-weight: normal;'>Branch & IFS Code:</span> Balanagar & PUNB0473700</p>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; border-right: 0;  width:100%;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr>");
                html.Append("<td><p align='right' style='font-size: 13px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p><span><br/><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><p align='right' style='font-size: 13px; margin:0;'>Authorised Signatory</p></td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%;' cellspacing='0' cellpadding='2'><tbody>");
                html.Append("<tr>");
                html.Append("<td style='text-align:center; font-size: 11px; padding:0;'>SUBJECT TO HYDERABAD JURISDICTION</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='text-align:center; font-size: 11px;'>This is a Computer Generated Invoice</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("</td></tr>");

                html.Append("</tbody></table>");
                /***************/
                lstSB.Add(html.ToString());
            });
            var FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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

        //public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, String SupplyType)
        //{
        //    Einvoice Eobj;
        //    IrnResponse ERes = null;

        //    HDB_ImportRepository objPpgRepo = new HDB_ImportRepository();



        //    if (SupplyType == "B2C")
        //    {
        //        Eobj = new Einvoice();
        //        IrnModel m1 = new IrnModel();

        //        QrCodeInfo q1 = new QrCodeInfo();
        //        //   QrCodeData qdt = new QrCodeData();
        //        objPpgRepo.GetIRNForB2CInvoice(InvoiceNo, "INV");
        //        var Output = (QrCodeData)objPpgRepo.DBResponse.Data;

        //        m1.DocumentNo = Output.DocNo;
        //        m1.DocumentDate = Output.DocDt;
        //        m1.SupplierGstNo = Output.SellerGstin;
        //        m1.DocumentType = Output.DocTyp;
        //        String IRN = Eobj.GenerateB2cIrn(m1);
        //        Output.Irn = IRN;
        //        Output.IrnDt = Output.DocDt;
        //        Output.iss = "NIC";
        //        q1.Data = Output;
        //        String QRCode = Eobj.GenerateB2cQRCode(q1);
        //        objPpgRepo.AddEditIRNB2C(IRN, QRCode, InvoiceNo);

        //        return Json(objPpgRepo.DBResponse.Status);
        //        //   IrnResponse ERes = await Eobj.GenerateB2cIrn();
        //    }

        //    else
        //    {
        //        objPpgRepo.GetIRNForYard(InvoiceNo);
        //        var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

        //        objPpgRepo.GetHeaderIRNForYard();

        //        HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

        //        string jsonEInvoice = JsonConvert.SerializeObject(Output);

        //        string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
        //        Eobj = new Einvoice(Hp, jsonEInvoice);
        //        ERes = await Eobj.GenerateEinvoice();
        //        if (ERes.Status == 1)
        //        {
        //            objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
        //        }

        //        return Json(ERes.Status);
        //    }
        //    // var Images = LoadImage(ERes.QRCodeImageBase64);

        //    return Json(ERes.Status);
        //}
        /*     public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo)
             {


                 Wfld_ImportRepository objPpgRepo = new Wfld_ImportRepository();
                 //objChrgRepo.GetAllCharges();
                 objPpgRepo.GetIRNForYard(InvoiceNo);
                 var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                 objPpgRepo.GetHeaderIRNForYard();

                 HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                 string jsonEInvoice = JsonConvert.SerializeObject(Output);
                 string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                 Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);
                 IrnResponse ERes = await Eobj.GenerateEinvoice();
                 // var Images = LoadImage(ERes.QRCodeImageBase64);
                 objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);

                 return Json(objPpgRepo.DBResponse);
             }*/
        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {


            HDB_ImportRepository objPpgRepo = new HDB_ImportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

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
                Hdb_IrnB2CDetails irnb2cobj = new Hdb_IrnB2CDetails();
                irnb2cobj = (Hdb_IrnB2CDetails)objPpgRepo.DBResponse.Data;
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










        #endregion

        #region Destuffing Payment Sheet
        [HttpGet]
        public ActionResult CreateDestuffingPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeStuffingRequestForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetDestuffingCont(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDestuffingContForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDestuffingPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,
            List<PaymentSheetContainer> lstPaySheetContainer, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDestuffingPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceId);

            var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.TDS = 0;
            Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.AllTotal;
            /**************BOL PRINT**********************/
            objChrgRepo.GetBOL(Output.RequestId);
            var BOL = "";
            if (objChrgRepo.DBResponse.Status == 1)
            {
                BOL = objChrgRepo.DBResponse.Data.ToString();
            }
            /********************************************/
            return Json(new { Output, BOL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingPaymentSheet(FormCollection objForm)
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

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.StuffingDate;
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.DestuffingDate;
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : item.CartingDate;
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

                ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDest");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
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

        #region Delivery Payment Sheet
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetDeliveryApplicationForImpPaymentSheet();
            if (ObjIR.DBResponse.Status > 0)
              ViewBag.StuffingReqList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
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

            return PartialView();
        }

        /*
        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryCont(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryContForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }
        */

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult InvoicePrint_Godown(string InvoiceNo)
        {
            Hdb_ReportRepository objGPR = new Hdb_ReportRepository();
            objGPR.GetInvoiceDetailsPrintByNo(InvoiceNo, "IMPDeli");
            if (objGPR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)objGPR.DBResponse.Data;
                PpgInvoiceYard objGP = new PpgInvoiceYard();
                string FilePath = "";
                FilePath = GeneratingInvoicePrint(ds, "IMPDeli");
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }





        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryBOE(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetBOEForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.BOEList = null;

            return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);

            //ImportRepository objImport = new ImportRepository();
            //string XMLText = "";
            //if (lstPaySheetContainer != null)
            //{
            //    XMLText = Utility.CreateXML(lstPaySheetContainer);
            //}

            //objImport.GetBOEForPaymentSheet(XMLText);
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.BOEList = null;

            //return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour,
            List<PaymentSheetBOE> lstPaySheetBOE,string ExportUnder,int Distance=0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }

            HDB_ImportRepository objChrgRepo = new HDB_ImportRepository();

            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetGodownPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, CasualLabour, InvoiceType, XMLText, ExportUnder, Distance, InvoiceId);

            var Output = (Hdb_PostPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPDeli";
            Output.InvoiceType = InvoiceType;
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
                if (!Output.BOLNo.Contains(item.BOLNo))
                    Output.BOLNo += item.BOLNo + ", ";
                if (!Output.BOLDate.Contains(item.BOLDate))
                    Output.BOLDate += item.BOLDate + ", ";
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
                    Output.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        StuffCUM = item.StuffCUM,
                        ISODC = item.ISODC,

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
            });



            // return Json(Output, JsonRequestBehavior.AllowGet);
            /**********BOL PRINT**************/
            var BOL = "";
            objChrgRepo.GetBOLForDeliverApp(Output.RequestId);
            if (objChrgRepo.DBResponse.Status == 1)
                BOL = objChrgRepo.DBResponse.Data.ToString();
            /********************************/
            return Json(new { Output, BOL });
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GodownInvoicePrint(string InvoiceNo)
        {
            HDB_ImportRepository objGPR = new HDB_ImportRepository();
            objGPR.GetInvoiceDetailsForGodownPrintByNo(InvoiceNo, "IMPDeli");
            PpgInvoiceYard objGP = new PpgInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (PpgInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceGodown(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        private string GeneratingPDFInvoiceGodown(PpgInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
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
            html.Append("<br />ASSESSMENT SHEET LCL");
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
            return "/Docs/" + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GodownInvoicePrint_HDB(string InvoiceNo)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetInvoiceDetailsForGodownPrintByNo(InvoiceNo, "IMPDeli");
            PpgInvoiceYard objGP = new PpgInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (PpgInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceGodown_Hdb(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceGodown_Hdb(PpgInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
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
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" + objGP.CompanyAddress + "</span>");
            //  html.Append("<br />" + InvoiceModuleName);
            html.Append("</td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
            html.Append("</tr>");
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

            //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Carting</th>");
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
                //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.CartingDate + "</td>");
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
            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
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

                var invoiceData = JsonConvert.DeserializeObject<Hdb_PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CfsWiseCharg = "", CargoXML="";
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
                if(invoiceData.lstPreInvoiceCargo!=null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                HDB_ImportRepository objChargeMaster = new HDB_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);

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

        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
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

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetEmptyCont(string InvoiceFor, int AppraisementId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId,
            List<Hdb_PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, int CasualLabour, string ExportUnder,int Distance=0,int InvoiceId = 0)
        {
            //string XMLText = "";
            //if (lstPaySheetContainer != null)
            //{
            //    XMLText = Utility.CreateXML(lstPaySheetContainer);
            //}

            // ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //HDB_ImportRepository objChrgRepo = new HDB_ImportRepository();
            ////objChrgRepo.GetAllCharges();
            //objChrgRepo.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
            //        PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceFor, InvoiceId);

            //var Output = (Hdb_PostPaymentSheet)objChrgRepo.DBResponse.Data;
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
            ///***************BOL PRINT*************/
            //objChrgRepo.GetBOLForEmptyCont(InvoiceFor, Output.RequestId);
            //var BOL = "";
            //if (objChrgRepo.DBResponse.Status == 1)
            //    BOL = objChrgRepo.DBResponse.Data.ToString();
            ///************************************/
            //return Json(new { Output, BOL });

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            HDB_ImportRepository objHdbRepo = new HDB_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objHdbRepo.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, InvoiceId, InvoiceFor, CasualLabour, ExportUnder, Distance);
            var Output = (Hdb_InvoiceYard)objHdbRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            if (InvoiceFor == "YARD")
            {
                Output.Module = "IMPYard";
            }
            else
            {
                Output.Module = "IMPDeli";
            }

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
                    Output.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        StuffCUM = item.StuffCUM,
                        ISODC=item.ISODC
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
                var invoiceFor = objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<Hdb_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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

                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();    
                string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                HDB_ImportRepository objChargeMaster = new HDB_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objChargeMaster.AddEditEmptyContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);


                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
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
        public ActionResult ListOfExpInvoice(string Module,string modulenext, string InvoiceNo = null, string InvoiceDate = null)
        {
           HDB_ImportRepository objER = new HDB_ImportRepository();
            objER.ListOfExpInvoice(Module, modulenext, InvoiceNo, InvoiceDate);
            List< CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice > obj = new List<CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CwcExim.Areas.Export.Models.Hdb_ListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        #endregion

        #region Tentative Invoice
        [HttpGet]
        public ActionResult TentativeImpInvoice()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateContPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateDestuffingPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeStuffingRequestForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheetTab(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheetTab(string type = "YARD:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            ImportRepository objImport = new ImportRepository();
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }
        #endregion

        #region Delivery Application

        [HttpGet]
        public ActionResult CreateDeliveryApplication()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            //Hdb_DeliveryApplication objda = new Hdb_DeliveryApplication();
            //BondRepository ObjBR = new BondRepository();
            ObjIR.GetDestuffEntryNo();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.DestuffingEntryNoList = ObjIR.DBResponse.Data;
            else
                ViewBag.DestuffingEntryNoList = null;

            ObjIR.ListOfForwarder();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.lstTSAForwarder = (List<TSAForwarder>)ObjIR.DBResponse.Data;
            else
                ViewBag.lstTSAForwarder = null;

            //ObjIR.ListOfCHA();
            //if (ObjIR.DBResponse.Data != null)
            //    ViewBag.CHAList = new SelectList((IList<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //else
            //    ViewBag.CHAList = null;
            //ObjBR.ListOfImporter();
            //if (ObjBR.DBResponse.Data != null)
            //    ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
            //else
            //    ViewBag.ImporterList = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditDeliveryApplication(int DeliveryId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            // BondRepository ObjBR = new BondRepository();
            Hdb_DeliveryApplication ObjDelivery = new Hdb_DeliveryApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (Hdb_DeliveryApplication)ObjIR.DBResponse.Data;
                //ObjIR.ListOfCHA();
                //if (ObjIR.DBResponse.Data != null)
                //    ViewBag.CHAList = new SelectList((IList<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                //else
                //    ViewBag.CHAList = null;
                //ObjBR.ListOfImporter();
                //if (ObjBR.DBResponse.Data != null)
                //    ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
                //else
                //    ViewBag.ImporterList = null;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ViewDeliveryApplication(int DeliveryId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            Hdb_DeliveryApplication ObjDelivery = new Hdb_DeliveryApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (Hdb_DeliveryApplication)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryApplication()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DeliveryApplicationList> LstDelivery = new List<Hdb_DeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication(0);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<Hdb_DeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryApplicationList", LstDelivery);
        }

        [HttpGet]
        public ActionResult DeliveryFCLOBL(string OBLNo = "")
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DeliveryApplicationList> LstDelivery = new List<Hdb_DeliveryApplicationList>();

            ObjIR.GetDeliveryFCLByOBL(OBLNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDelivery = (List<Hdb_DeliveryApplicationList>)ObjIR.DBResponse.Data;
            }

            return PartialView(LstDelivery);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDeliveryFCL(int Page)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DeliveryApplicationList> LstDelivery = new List<Hdb_DeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDelivery = (List<Hdb_DeliveryApplicationList>)ObjIR.DBResponse.Data;
            }

            return Json(LstDelivery, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryApplication(Hdb_DeliveryApplication ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                string DeliveryXml = "";
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<Hdb_DeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.LstDeliveryAppDtl);
                }
                ObjIR.AddEditDeliveryApplication(ObjDelivery, DeliveryXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetBOEDetForDeliveryApp(int DestuffingEntryDtlId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetImporterList()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.ListOfImporter();
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHAList()
        {
            BondRepository ObjBR = new BondRepository();
            ObjBR.ListOfCHA();
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetForwarderList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfForwarder();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Destuffing Entry LCL

        [HttpGet]
        public ActionResult CreateDestuffingEntry(string type = "CONT")
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            ObjDestuffing.DODate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd-MM-yyyy");
            ViewData["InvType"] = type;
            ObjIR.GetContrNoForDestuffingEntry(type, Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<Hdb_DestuffingEntry>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
                ViewBag.ContList = JsonConvert.SerializeObject((List<Hdb_DestuffingEntry>)ObjIR.DBResponse.Data);

                //JsonConvert.SerializeObject(objImport.DBResponse.Data);
            }
            else
            {
                ViewBag.ContainerList = null;
            }
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllGodown();
            if (ObjYR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = (List<CwcExim.Areas.Master.Models.HDBGodown>)ObjYR.DBResponse.Data;
            }
            ObjIR.ListOfForwarder();
            if (ObjIR.DBResponse.Data != null)
                ObjDestuffing.lstTSAForwarder = (List<TSAForwarder>)ObjIR.DBResponse.Data;
            //ObjIR.ListOfCHA();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjIR.ListOfShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            return PartialView("CreateDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(string CFSCode)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContrDetForDestuffingEntry_Hdb(CFSCode, Convert.ToInt32(Session["BranchId"]));

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()

        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            ObjIR.GetAllDestuffingEntry(Convert.ToInt32(Session["BranchId"]),0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Hdb_DestuffingEntryList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetHdb_DestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Hdb_DestuffingEntry)ObjIR.DBResponse.Data;
                }
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.GetAllGodown();
                if (ObjYR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = (List<CwcExim.Areas.Master.Models.HDBGodown>)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("EditDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingEntry(int DestuffingEntryId)
        {
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetHdb_DestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Hdb_DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntry", ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
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
        public JsonResult AddEditDestuffingEntry(Hdb_DestuffingEntry ObjDestuffing)
        {
            ModelState.Remove("IsInsured");
            ModelState.Remove("ForwarderId");
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                // List<int> LstGodown = new List<int>();
                // string GodownXML = "";
                // string ClearLcoationXML = null;
                List<Hdb_DestuffingEntry> LstDestuffingEntry = new List<Hdb_DestuffingEntry>();

                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_DestuffingEntry>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                    //if (LstDestuffingEntry.Count > 0)
                    //{
                    //    foreach (var item in LstDestuffingEntry)
                    //    {
                    //        string[] GodownList = item.LocationDetails.Split(',');
                    //        foreach (string Data in GodownList)
                    //            LstGodown.Add(Convert.ToInt32(Data));

                    //    }
                    //    GodownXML = Utility.CreateXML(LstGodown);
                    //}
                }
                ////if (ObjDestuffing.GodownWiseLocationIds != null && ObjDestuffing.GodownWiseLocationIds != "")
                ////{
                ////    string[] GodownList = ObjDestuffing.GodownWiseLocationIds.Split(',');
                ////    foreach (string Data in GodownList)
                ////        LstGodown.Add(Convert.ToInt32(Data));
                ////    GodownXML = Utility.CreateXML(LstGodown);
                ////}

                //if (ObjDestuffing.ClearLocation != "" && ObjDestuffing.ClearLocation != null)
                //{
                //    string[] data = ObjDestuffing.ClearLocation.Split(',');
                //    List<int> LstClearLocation = new List<int>();
                //    foreach (var elem in data)
                //        LstClearLocation.Add(Convert.ToInt32(elem));
                //    ClearLcoationXML = Utility.CreateXML(LstClearLocation);
                //}

                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjDestuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditDestuffingEntry_Hdb(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]));
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
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Destuffing Entry FCL

        [HttpGet]
        public ActionResult CreateDestuffingEntryFCL(string type = "CONT")
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            ObjDestuffing.DODate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd-MM-yyyy");
            ViewData["InvType"] = type;
            ObjIR.GetContrNoForDestuffingEntryFCL(type, Convert.ToInt32(Session["BranchId"]));
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<Hdb_DestuffingEntry>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
                ViewBag.ContList = JsonConvert.SerializeObject((List<Hdb_DestuffingEntry>)ObjIR.DBResponse.Data);

                //JsonConvert.SerializeObject(objImport.DBResponse.Data);
            }
            else
            {
                ViewBag.ContainerList = null;
            }
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllGodown();
            if (ObjYR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = (List<CwcExim.Areas.Master.Models.HDBGodown>)ObjYR.DBResponse.Data;
            }
            ObjIR.ListOfForwarder();
            if (ObjIR.DBResponse.Data != null)
                ObjDestuffing.lstTSAForwarder = (List<TSAForwarder>)ObjIR.DBResponse.Data;
            //ObjIR.ListOfCHA();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjIR.ListOfShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            return PartialView("CreateDestuffingEntryFCL", ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntryFCL(string CFSCode)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContrDetForDestuffingEntry_HdbFCL(CFSCode, Convert.ToInt32(Session["BranchId"]));

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryListFCL()

        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            ObjIR.GetAllDestuffingEntryFCL(Convert.ToInt32(Session["BranchId"]),0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Hdb_DestuffingEntryList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryListFCL", LstDestuffing);
        }

        [HttpGet]
        public ActionResult DestuffingFCLContainer(string ContainerName = "")
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();

            ObjIR.GetDestuffingFCLByContainer(ContainerName);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Hdb_DestuffingEntryList>)ObjIR.DBResponse.Data;
            }

            return PartialView(LstDestuffing);
           
        }


        [HttpGet]
        public ActionResult DestuffinLCLContainer(string ContainerName = "")
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();

            ObjIR.GetDestuffingLCLByContainer(ContainerName);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Hdb_DestuffingEntryList>)ObjIR.DBResponse.Data;
            }

            return PartialView(LstDestuffing);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffFCL(int Page)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_DestuffingEntryList> LstDestuffing = new List<Hdb_DestuffingEntryList>();
            ObjIR.GetAllDestuffingEntryFCL(Convert.ToInt32(Session["BranchId"]), Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Hdb_DestuffingEntryList>)ObjIR.DBResponse.Data;
            }

            return Json(LstDestuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntryFCL(int DestuffingEntryId)
        {
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetHdb_DestuffingEntryFCL(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Hdb_DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditDestuffingEntryFCL", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingEntryFCL(int DestuffingEntryId)
        {
            Hdb_DestuffingEntry ObjDestuffing = new Hdb_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetHdb_DestuffingEntryFCL(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Hdb_DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntryFCL", ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntryFCL(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.DelDestuffingEntryFCL(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingEntryFCL(Hdb_DestuffingEntry ObjDestuffing)
        {
            ModelState.Remove("IsInsured");
            ModelState.Remove("ForwarderId");
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                // List<int> LstGodown = new List<int>();
                // string GodownXML = "";
                // string ClearLcoationXML = null;
                List<Hdb_DestuffingEntry> LstDestuffingEntry = new List<Hdb_DestuffingEntry>();

                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_DestuffingEntry>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                    //if (LstDestuffingEntry.Count > 0)
                    //{
                    //    foreach (var item in LstDestuffingEntry)
                    //    {
                    //        string[] GodownList = item.LocationDetails.Split(',');
                    //        foreach (string Data in GodownList)
                    //            LstGodown.Add(Convert.ToInt32(Data));

                    //    }
                    //    GodownXML = Utility.CreateXML(LstGodown);
                    //}
                }
                ////if (ObjDestuffing.GodownWiseLocationIds != null && ObjDestuffing.GodownWiseLocationIds != "")
                ////{
                ////    string[] GodownList = ObjDestuffing.GodownWiseLocationIds.Split(',');
                ////    foreach (string Data in GodownList)
                ////        LstGodown.Add(Convert.ToInt32(Data));
                ////    GodownXML = Utility.CreateXML(LstGodown);
                ////}

                //if (ObjDestuffing.ClearLocation != "" && ObjDestuffing.ClearLocation != null)
                //{
                //    string[] data = ObjDestuffing.ClearLocation.Split(',');
                //    List<int> LstClearLocation = new List<int>();
                //    foreach (var elem in data)
                //        LstClearLocation.Add(Convert.ToInt32(elem));
                //    ClearLcoationXML = Utility.CreateXML(LstClearLocation);
                //}

                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjDestuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditDestuffingEntry_HdbFCL(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]));
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


        //[HttpGet]
        //public JsonResult GetGodownWiseLocation(int GodownId)
        //{
        //    HDB_ImportRepository objIR = new HDB_ImportRepository();
        //    objIR.GodownWiseLocation(GodownId);
        //    object objLctn = null;
        //    if (objIR.DBResponse.Data != null)
        //        objLctn = objIR.DBResponse.Data;
        //    return Json(objLctn, JsonRequestBehavior.AllowGet);
        //}
        #endregion


        #region Form One LCL

        [HttpGet]
        public ActionResult Hdb_CreateFormOne()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddFormOne()
        {
            Hdb_FormOneLclModel objFormOne = new Hdb_FormOneLclModel();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objImport.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objImport.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }
            objImport.ListOfShippingLines();
            if (objImport.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objImport.DBResponse.Data;
            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;
            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;
            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;
            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfForwarder();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstTSAForwarder = (List<TSAForwarder>)objImport.DBResponse.Data;

            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;
            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();

            objImport.GetContainer(BranchId);
            if (objImport.DBResponse.Data != null)
            {
                Lstcontainer = (List<ContainerRefGateEntry>)objImport.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;



            return PartialView(objFormOne);
        }


        public JsonResult GetFieldsForContainer(string ContainerName)
        {
            if (ContainerName != "")
            {
                HDB_ImportRepository ObjGOR = new HDB_ImportRepository();
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ObjGOR.GetAutoPopulateData(ContainerName, BranchId);
                return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
                // ViewBag.JSONResult = ObjGOR.DBResponse.Data;
                //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                //objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                //string strDate = objEntryThroughGate.ReferenceDate;
                //string[] arrayDate = strDate.Split(' ');
                //objEntryThroughGate.ReferenceDate = arrayDate[0];
                //ViewBag.strTime = objEntryThroughGate.EntryTime;

                //return Json(objEntryThroughGate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
                // ViewBag.JSONResult = "Error";
                //return PartialView("CreateEntryThroughGate");
            }

        }


        [HttpGet]
        public ActionResult Hdb_GetFormOneList(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<Hdb_FormOneLclModel> lstFormOne = new List<Hdb_FormOneLclModel>();
                HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
                objImportRepo.GetFormOne(0);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Hdb_FormOneLclModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<Hdb_FormOneLclModel> lstFormOne = new List<Hdb_FormOneLclModel>();
                HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
                objImportRepo.GetFormOneByContainer(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Hdb_FormOneLclModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForLCL(int Page)
        {
            IEnumerable<Hdb_FormOneLclModel> lstFormOne = new List<Hdb_FormOneLclModel>();
            HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
            objImportRepo.GetFormOne(Page);
            if (objImportRepo.DBResponse.Data != null)
                lstFormOne = (IEnumerable<Hdb_FormOneLclModel>)objImportRepo.DBResponse.Data;

            return Json(lstFormOne, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOne(Hdb_FormOneLclModel objFormOne)
        {
            ModelState.Remove("ShippingLineName");
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ModelState.Remove("CargoType");
                if (ModelState.IsValid)
                {
                    // objFormOne.FormOneDetailsJS.Replace("\"DateOfLanding: \":\"\"", "\"DateOfLanding\":\"null\"");
                    objFormOne.lstFormOneDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_FormoneLclDetailModel>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetails.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetails);
                    HDB_ImportRepository objImport = new HDB_ImportRepository();
                    objImport.AddEditFormOne(objFormOne, BranchId, XML, ((Login)(Session["LoginUser"])).Uid);
                    ModelState.Clear();
                    return Json(objImport.DBResponse);
                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1 };
                    return Json(Err);
                }
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditFormOne(int FormOneId)
        {
            Hdb_FormOneLclModel objFormOne = new Hdb_FormOneLclModel();
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);


            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Hdb_FormOneLclModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetails != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetails);

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;
            objImport.ListOfShippingLines();
            if (objImport.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objImport.DBResponse.Data;

            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;

            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;

            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfForwarder();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstTSAForwarder = (List<TSAForwarder>)objImport.DBResponse.Data;

            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;

            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();

            objImport.GetContainer(BranchId);
            if (objImport.DBResponse.Data != null)
            {
                Lstcontainer = (List<ContainerRefGateEntry>)objImport.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult ViewFormOne(int FormOneId)
        {
            Hdb_FormOneLclModel objFormOne = new Hdb_FormOneLclModel();
            HDB_ImportRepository objImport = new HDB_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Hdb_FormOneLclModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetails != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetails);
            return PartialView(objFormOne);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOne(int FormOneId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOne(FormOneId);
            return Json(objImport.DBResponse);
        }
        [HttpGet]
        public JsonResult GetICEGateData(string ContainerNo, string Size)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetICEGateData(ContainerNo, Size);
            return Json((List<Hdb_FormoneLclDetailModel>)objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PrintFormOne(int FormOneId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            if (FormOneId > 0)
                objImport.FormOnePrint(FormOneId);
            var model = (Hdb_FormOnePrintModel)objImport.DBResponse.Data;
            var printableData1 = (IList<Hdb_FormOnePrintDetailModel>)model.lstFormOnePrintDetail;

            var fileName = model.FormOneNo + ".pdf";
            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
            string Path = GeneratePDFForForm1(printableData1, FormOneId);



            //   string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

            //if (!Directory.Exists(PdfDirectory))
            //  Directory.CreateDirectory(PdfDirectory);



            //   var cPdf = new CustomPdfGeneratorKdl();
            //   cPdf.Generate(PdfDirectory + "/" + fileName, model.ShippingLineNo, printableData1);

            return Json(new { Status = 1, FileUrl = Path }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public string GeneratePDFForForm1(IList<Hdb_FormOnePrintDetailModel> LstJobOrder, int FormoneId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/Form-1LCL" + FormoneId + ".pdf";

            List<PrintJobOrder> List = new List<PrintJobOrder>();
            //int Count = 0;
            // string Sline = "";
            StringBuilder Html = new StringBuilder();
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }



            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            /*  if ((Convert.ToInt32(Session["BranchId"])) == 1)
              {
                  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td width='20%' style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";
              }
              else
              {
                  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='90%' valign='top' align='center'><h1 style='font-size: 18px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + CompanyAddress + "</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></thead><tbody><tr><td style='text-align:left;' colspan='2'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right; width:20%;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='4' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='4' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Loaded Container / Loaded CBT </td></tr><tr><td colspan='4' style='text-align:left;'><span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> <span style='border-bottom:1px solid #000;'> " + objMdl.ToLocation + " </span></td></tr><tr><td colspan='4'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER/CBT NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='4'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr></tr><tr><span><br/><br/><br/><br/><br/><br/></span></tr><tr><td colspan='6' style='padding-top:150px;'>Copy to:- <span></span></td><td colspan='6' style='padding-top:150px;text-align:right;'>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
              } */

            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody> <tr><th colspan='12' style='font-size: 12px; text-align: right;'>CONTROLLED</th></tr><tr><td colspan='12'>");
            Html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody> <tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td><td width='16%' align='right'> <table style='width:100%;' cellspacing='0' cellpadding='0'><tbody> <tr><td style='border:1px solid #333;'>");
            Html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Doc No. F/CD/CFS/20</div></td></tr></tbody></table> </td></tr></tbody></table> </td></tr><tr><td colspan='12'> <table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            Html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally<br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>PNR REGISTER for Import Containers</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            Html.Append("<thead><tr>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:5%;'>Sl. No.</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>Date of receipt of containers</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>PNR No & Date</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>Name of Importer</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Name of Shipping Lines</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Name of Forwarder</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>Cargos</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Container No.</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>Size</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Customs Seal No.</th>");
            Html.Append("<th style='border-bottom:1px solid #000;text-align:center;padding:10px;width:10%;'>Shipping Line real No.</th>");
            Html.Append("</tr></thead>");
            Html.Append("<tbody>");

            int i = 1;
            LstJobOrder.ToList().ForEach(item =>
            {
                Html.Append("<tr><td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:5%;'>" + i + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>" + item.FormOneDate + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'></td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>" + item.ImpName + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.ShippingLineName + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>" + item.ForwarderName + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>" + item.HazType + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.ContainerNo + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>" + item.ContainerSize + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.SealNo + "</td>");
                Html.Append("<td style='text-align:center;border-bottom:1px solid #000;padding:10px;width:10%;'></td>");
                Html.Append("</tr>");

                //Html.Append("<tr><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:5%;'>" + i + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:13%;'>" + item.FormOneDate + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'></td><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:13%;'>" + item.ImpName + "</td>");
                //Html.Append("<td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'>" + item.ShippingLineName + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:8%;'>"+item.ForwarderName+"</td><td style='text-align:center;border:1px solid #000;padding:10px;width:8%;'>" + item.HazType + "</td><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:10%;'>" + item.ContainerNo + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:8%;'>" + item.ContainerSize + "</td>");
                //Html.Append("<td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'>" + item.SealNo + "</td><td style='text-align:center;border:1px solid #000;padding:10px;border-right:none;width:10%;'></td></tr>");

                i = i + 1;
            });

            Html.Append("</tbody>");
            Html.Append("</table></td></tr></tbody></table>");


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = this.HeadOffice;
                rh.HOAddress = this.HOAddress;
                rh.ZonalOffice = this.ZonalOffice;
                rh.ZOAddress = this.ZOAddress;
                rh.GeneratePDF(Path, Html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/Form-1LCL" + FormoneId + ".pdf";
        }




        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateFormOne(FormCollection fc)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            try
            {
                var pages = new string[1];
                var fileName = fc["FormOne"].ToString() + ".pdf";
                string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4Landscape))
                {
                    rh.GeneratePDF(PdfDirectory + "/" + fileName, fc["Page1"].ToString());
                }
                return Json(new { Status = 1, Message = "/Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "" }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion

        #region-- CUSTOM PDF EGENRATOR --

        class CustomPdfGeneratorKdl
        {
            private readonly iTextSharp.text.Document _document;
            private iTextSharp.text.Document Document { get { return new iTextSharp.text.Document(); } }

            private IEnumerable<string> Split(string str, int chunkSize)
            {
                return Enumerable.Range(0, str.Length / chunkSize)
                    .Select(i => str.Substring(i * chunkSize, chunkSize));
            }

            public CustomPdfGeneratorKdl()
            {
                _document = Document;
                _document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                _document.SetMargins(10f, 10f, 10f, 10f);
            }

            public void Generate(string filePath, string shippingLine, IList<Hdb_FormOnePrintDetailModel> data)
            {
                try
                {
                    var pageHeight = 0f;
                    var Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, new FileStream(filePath, FileMode.Create));

                    /*var img = iTextSharp.text.Image.GetInstance(@"D:\RAHULZ\RKZ\pdfconsole\pdfconsole\bin\Debug\scan0002.jpg");
                    img.ScaleToFit(_document.PageSize.Width, _document.PageSize.Height);
                    img.SetAbsolutePosition(0, 0);
                    Writer.PageEvent = new ImageBackgroundHelper(img);*/

                    var font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.BaseColor.BLACK);

                    _document.Open();

                    var haz = data.Any(o => o.HazType == "HAZ") ? "HAZ" : "NON-HAZ";
                    var refer = data.Any(o => o.ReferType == "/ REEFER") ? " / REEFER" : "";
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(haz + refer, font), 590, 545, 0);

                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(shippingLine, iTextSharp.text.FontFactory.GetFont("Arial", 9, 1, iTextSharp.text.BaseColor.BLACK)), 640, 470, 0);

                    var table = new PdfPTable(8);
                    var width = new[] { 148f, 120f, 54f, 40f, 130f, 94f, 80f, 84f };
                    table.SetWidthPercentage(width, iTextSharp.text.PageSize.A4.Rotate());

                    IList<string> addeditems = new List<string>();
                    var pcb = Writer.DirectContent;

                    data.GroupBy(o => o.LineNo).ToList().ForEach(groupedLine =>
                    {
                        foreach (var item in groupedLine)
                        {
                            var val1 = addeditems.Any(o => o == item.VesselName) ? "" : item.VesselName;
                            var val2 = addeditems.Any(o => o == item.VoyageNo) ? "" : item.VoyageNo;
                            var cell1 = new PdfPCell(new Phrase(val1 + Environment.NewLine + val2, font));
                            cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell1.BorderWidth = 0;
                            table.AddCell(cell1);

                            var val3 = addeditems.Any(o => o == item.ContainerNo) ? "" : item.ContainerNo;
                            var val4 = addeditems.Any(o => o == item.SealNo) ? "" : item.SealNo;
                            var cell2 = new PdfPCell(new Phrase(val3 + Environment.NewLine + val4, font));
                            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell2.BorderWidth = 0;
                            table.AddCell(cell2);

                            var val5 = addeditems.Any(o => o == item.Type) ? "" : item.Type;
                            var cell3 = new PdfPCell(new Phrase(val5, font));
                            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell3.BorderWidth = 0;
                            table.AddCell(cell3);

                            var val6 = addeditems.Any(o => o == item.LineNo) ? "" : item.LineNo;
                            var cell4 = new PdfPCell(new Phrase(val6, font));
                            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell4.BorderWidth = 0;
                            table.AddCell(cell4);

                            var name_address = item.ImpName + Environment.NewLine + item.ImpAddress + Environment.NewLine + item.ImpName2 + Environment.NewLine + item.ImpAddress2;
                            var val7 = addeditems.Any(o => o == name_address) ? "" : name_address;
                            var cell5 = new PdfPCell(new Phrase(val7, font));
                            cell5.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell5.BorderWidth = 0;
                            table.AddCell(cell5);

                            var val8 = addeditems.Any(o => o == item.CargoDesc) ? "" : item.CargoDesc;
                            var cell6 = new PdfPCell(new Phrase(val8, font));
                            cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell6.BorderWidth = 0;
                            table.AddCell(cell6);

                            var val9 = addeditems.Any(o => o == item.DateOfLanding) ? "" : item.DateOfLanding;
                            var cell7 = new PdfPCell(new Phrase(val9, font));
                            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell7.BorderWidth = 0;
                            table.AddCell(cell7);

                            var val10 = "";
                            var cell8 = new PdfPCell(new Phrase(val10, font));
                            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell8.BorderWidth = 0;
                            table.AddCell(cell8);

                            table.CompleteRow();
                            pageHeight += table.CalculateHeights();

                            if (pageHeight > 800)
                            {
                                table.WriteSelectedRows(0, -1, 45, 360, pcb);
                                _document.NewPage();
                                pageHeight = 0f;
                                table.Rows.Clear();
                            }

                            addeditems.Add(item.LineNo);
                            if (!addeditems.Any(o => o == item.VesselName))
                                addeditems.Add(item.VesselName);
                            if (!addeditems.Any(o => o == item.VoyageNo))
                                addeditems.Add(item.VoyageNo);
                            if (!addeditems.Any(o => o == item.ContainerNo))
                                addeditems.Add(item.ContainerNo);
                            if (!addeditems.Any(o => o == item.SealNo))
                                addeditems.Add(item.SealNo);
                            if (!addeditems.Any(o => o == item.Type))
                                addeditems.Add(item.Type);
                            if (!addeditems.Any(o => o == name_address))
                                addeditems.Add(name_address);
                            if (!addeditems.Any(o => o == item.CargoDesc))
                                addeditems.Add(item.CargoDesc);
                            if (!addeditems.Any(o => o == item.DateOfLanding))
                                addeditems.Add(item.DateOfLanding);
                        }
                        foreach (var item in groupedLine)
                        {
                            addeditems.Remove(item.ContainerNo);
                            addeditems.Remove(item.SealNo);
                        }
                    });

                    table.WriteSelectedRows(0, -1, 45, 360, pcb);

                    if (_document.IsOpen())
                        _document.Close();
                }
                catch (Exception e)
                {
                    if (_document.IsOpen())
                        _document.Close();
                }
            }

            public void Generate1(string filePath, string shippingLine, IList<Kdl_FormOnePrintDetailModel> data)
            {
                var Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_document, new FileStream(filePath, FileMode.Create));

                var img = iTextSharp.text.Image.GetInstance(@"D:\RAHULZ\RKZ\pdfconsole\pdfconsole\bin\Debug\scan0002.jpg");
                img.ScaleToFit(_document.PageSize.Width, _document.PageSize.Height);
                img.SetAbsolutePosition(0, 0);
                Writer.PageEvent = new ImageBackgroundHelper(img);

                var font = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.BaseColor.BLACK);

                _document.Open();

                var haz = data.Any(o => o.HazType == "HAZ") ? "HAZ" : "NON-HAZ";
                var refer = data.Any(o => o.ReferType == "/ REEFER") ? " / REEFER" : "";
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(haz + refer, font), 590, 545, 0);

                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(shippingLine, iTextSharp.text.FontFactory.GetFont("Arial", 9, 1, iTextSharp.text.BaseColor.BLACK)), 640, 470, 0);

                var VesselName = data.FirstOrDefault().VesselName;
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(VesselName, font), 50, 324, 0);
                var VoyageNo = data.FirstOrDefault().VoyageNo;
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(VoyageNo, font), 50, 312, 0);
                var RotationNo = data.FirstOrDefault().RotationNo;
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(RotationNo, font), 50, 300, 0);

                var Type = data.FirstOrDefault().Type;
                iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(Type, font), 325, 300, 0);

                //var LineNo = data.FirstOrDefault().LineNo;
                //iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(LineNo, font), 380, 300, 0);

                List<string> splitedText = new List<string>();
                var yAxis = 350f;

                var ImpName = data.FirstOrDefault().ImpName;
                if (ImpName.Length > 21)
                {
                    ImpName = ImpName.PadRight((ImpName.Length - 1) + (21 - (ImpName.Length % 21)), ' ') + ".";
                    splitedText = Split(ImpName, 21).ToList();
                }
                else
                {
                    splitedText.Add(ImpName);
                }
                splitedText.ToList().ForEach(item =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                    yAxis -= 10f;
                });
                splitedText.Clear();

                yAxis -= 5f;
                var ImpAddress = data.FirstOrDefault().ImpAddress;
                if (ImpAddress.Length > 21)
                {
                    ImpAddress = ImpAddress.PadRight((ImpAddress.Length - 1) + (21 - (ImpAddress.Length % 21)), ' ') + ".";
                    splitedText = Split(ImpAddress, 21).ToList();
                }
                else
                {
                    splitedText.Add(ImpAddress);
                }
                splitedText.ToList().ForEach(item =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                    yAxis -= 10f;
                });
                splitedText.Clear();

                //Second Name
                var ImpName2 = string.IsNullOrEmpty(data.FirstOrDefault().ImpName2) ? "" : data.FirstOrDefault().ImpName2;
                if (!string.IsNullOrEmpty(ImpName2) && ImpName != ImpName2)
                {
                    yAxis -= 5f;
                    if (ImpName2.Length > 21)
                    {
                        ImpName2 = ImpName2.PadRight((ImpName2.Length - 1) + (21 - (ImpName2.Length % 21)), ' ') + ".";
                        splitedText = Split(ImpName2, 21).ToList();
                    }
                    else
                    {
                        splitedText.Add(ImpName2);
                    }
                    splitedText.ToList().ForEach(item =>
                    {
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                        yAxis -= 10f;
                    });
                    splitedText.Clear();

                    yAxis -= 5f;
                    var ImpAddress2 = data.FirstOrDefault().ImpAddress2;
                    if (ImpAddress2.Length > 21)
                    {
                        ImpAddress2 = ImpAddress2.PadRight((ImpAddress2.Length - 1) + (21 - (ImpAddress2.Length % 21)), ' ') + ".";
                        splitedText = Split(ImpAddress2, 21).ToList();
                    }
                    else
                    {
                        splitedText.Add(ImpAddress2);
                    }
                    splitedText.ToList().ForEach(item =>
                    {
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 410, yAxis, 0);
                        yAxis -= 10f;
                    });
                    splitedText.Clear();
                }

                yAxis = 350f;
                var CargoDesc = data.FirstOrDefault().CargoDesc;
                if (CargoDesc.Length > 16)
                {
                    CargoDesc = CargoDesc.PadRight((CargoDesc.Length - 1) + (16 - (CargoDesc.Length % 16)), ' ') + ".";
                    splitedText = Split(CargoDesc, 16).ToList();
                }
                else
                {
                    splitedText.Add(CargoDesc);
                }
                splitedText.ToList().ForEach(item =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(item, font), 540, yAxis, 0);
                    yAxis -= 10f;
                });

                //var DateOfLanding = data.FirstOrDefault().DateOfLanding;
                //iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(DateOfLanding, font), 635, 300, 0);

                yAxis = 350f;
                int counter = 0;
                IList<string> printedLines = new List<string>();
                data.GroupBy(o => o.LineNo).ToList().ForEach(groupedItem =>
                {
                    iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(groupedItem.Key, font), 380, yAxis, 0);
                    groupedItem.ToList().ForEach(item =>
                    {
                        var containerNo = item.ContainerNo;
                        var sealNo = item.SealNo;
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(containerNo, font), 200, yAxis, 0);
                        yAxis -= 10f;
                        iTextSharp.text.pdf.ColumnText.ShowTextAligned(Writer.DirectContent, iTextSharp.text.Element.ALIGN_LEFT, new iTextSharp.text.Phrase(sealNo, font), 200, yAxis, 0);
                        yAxis -= 15f;
                        counter += 1;
                        if (counter == 10)
                        {
                            yAxis = 350f;
                            _document.NewPage();
                            counter = 0;
                        }
                    });
                });

                if (_document.IsOpen())
                    _document.Close();
            }
        }

        #endregion

        #region Job Order
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            HDBMasterRepository objPort = new HDBMasterRepository();
            Hdb_ImportJobOrder objjoborder = new Hdb_ImportJobOrder();
            List<HDBPort> LstPort = new List<HDBPort>();
            string Container_CBT = "CONT";
            objIR.GetAllBlNofromFormOne(Container_CBT);
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfBlNo = objIR.DBResponse.Data;
            objPort.GetAllPort();
            if (objPort.DBResponse.Data != null)
                ViewBag.ListOfPort = objPort.DBResponse.Data;
            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            Hdb_ImportJobOrder objJO = new Hdb_ImportJobOrder();
            objJO.JobOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Operation = null;
            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();

            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            objPort.GetAllYard();
            if (objPort.DBResponse.Data != null)
                ViewBag.ListOfYard = (List<CwcExim.Areas.Master.Models.HDBYard>)objPort.DBResponse.Data;
            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            IList<Hdb_ImportJobOrderList> lstIJO = new List<Hdb_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Hdb_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }
        [HttpGet]
        public ActionResult ContainerJobOrderContainer(string ContainerName = "")
        {
           
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                IList<Hdb_ImportJobOrderList> lstIJO = new List<Hdb_ImportJobOrderList>();
                ObjIR.GetJobOrderOneByContainer(ContainerName);
                if (ObjIR.DBResponse.Data != null)
                    lstIJO = ((List<Hdb_ImportJobOrderList>)ObjIR.DBResponse.Data);

                return PartialView(lstIJO);
           
        }



        [HttpGet]
        public JsonResult LoadListMoreDataForJobOrder(int Page)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            IList<Hdb_ImportJobOrderList> lstIJO = new List<Hdb_ImportJobOrderList>();
            objIR.GetAllImpJO(Page);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Hdb_ImportJobOrderList>)objIR.DBResponse.Data);

            return Json(lstIJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int JobOrderId)
        {
            Hdb_ImportJobOrder objImp = new Hdb_ImportJobOrder();
            List<HDBPort> LstPort = new List<HDBPort>();
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            HDBMasterRepository objPort = new HDBMasterRepository();
            objIR.GetImpJODetails(JobOrderId);
            if (objIR.DBResponse.Data != null)
                objImp = (Hdb_ImportJobOrder)objIR.DBResponse.Data;

            objPort.GetAllPort();
            if (objPort.DBResponse.Data != null)
                ViewBag.ListOfPort = objPort.DBResponse.Data;

            objIR.ListOfCHA();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;

            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();

            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            YardRepository ObjYR = new YardRepository();
            ObjYR.GetAllYard();
            if (ObjYR.DBResponse.Data != null)
                ViewBag.ListOfYard = (List<CwcExim.Models.Yard>)ObjYR.DBResponse.Data;
            //objIR.GetYardWiseLocation(objImp.ToYardId);
            //if (objIR.DBResponse.Data != null)
            //    ViewBag.ListOfYardWiseLctn = (List<CwcExim.Areas.Master.Models.HDBYardWiseLocation>)objIR.DBResponse.Data;

            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int JobOrderId)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.GetImpJODetails(JobOrderId);
            Hdb_ImportJobOrder objImp = new Hdb_ImportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (Hdb_ImportJobOrder)objIR.DBResponse.Data;


            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            ViewBag.BranchId = Convert.ToInt32(Session["BranchId"]);
            return PartialView(objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(Hdb_ImportJobOrder objImp)
        {
            //if (ModelState.IsValid)
            //{
            List<Hdb_ImportJobOrderDtl> lstDtl = new List<Hdb_ImportJobOrderDtl>();
            List<Hdb_ImportClauseDtl> lstCDtl = new List<Hdb_ImportClauseDtl>();

            List<int> lstLctn = new List<int>();
            string XML = "", lctnXML = "";
            String CXML = "";
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            if (objImp.JobOrderDetailsJS != null)
            {
                
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_ImportJobOrderDtl>>(objImp.JobOrderDetailsJS);
                foreach(var item in lstDtl)
                {
                    if (item.GodownId == null)
                        item.GodownId = 0;
                }
                    if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            if (objImp.JobOrderClauseJS != null)
            {
                lstCDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_ImportClauseDtl>>(objImp.JobOrderClauseJS);
                if (lstCDtl.Count > 0)
                    CXML = Utility.CreateXML(lstCDtl);
            }
            /*if (objImp.YardWiseLocationIds != null)
            {
                string[] lctns = objImp.YardWiseLocationIds.Split(',');
                foreach (string data in lctns)
                    lstLctn.Add(Convert.ToInt32(data));
                lctnXML = Utility.CreateXML(lstLctn);
            }*/
            objIR.AddEditImpJO(objImp, XML, lctnXML, CXML);
            return Json(objIR.DBResponse);
            //}
            //else
            //{
            //    var Err = new { Status = -1, Message = "Error" };
            //    return Json(Err);
            //}

        }
        public JsonResult GetForm1Details(int FormOneId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetForm1Details(FormOneId);
            return Json((List<Hdb_ImportJobOrderDtl>)objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBlNoDetails(int FormOneId)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.GetBlNoDtl(FormOneId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.GetYardWiseLocation(YardId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFormOneList(string Container_CBT)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetAllBlNofromFormOne(Container_CBT);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownList()
        {
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllGodown();
            return Json(ObjYR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Custom Appraisement

        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            Hdb_CustomAppraisement ObjAppraisement = new Hdb_CustomAppraisement();
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd-MM-yyyy");
            //ObjIR.GetContnrNoForCustomAppraise();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ContainerList = new SelectList((List<Hdb_CustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
            //}
            //ObjIR.ListOfCHA();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjIR.ListOfShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            return PartialView("CreateCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CreateCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public JsonResult LoadShipbillForApprismnt()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.ListOfShippingLine();

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult LoadCHAForApprismnt()
        {


            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.ListOfCHA();
           
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public JsonResult LoadCustomapprism()
        {


            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContnrNoForCustomAppraise();

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode, string LineNo)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCustomAppraisementList()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_CustomAppraisement> LstAppraisement = new List<Hdb_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Hdb_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }



        [HttpGet]
        public ActionResult ApprisementFCLContainer(string ContainerName = "")
        {

            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_CustomAppraisement> LstAppraisement = new List<Hdb_CustomAppraisement>();

            ObjIR.GetApprisementFCLByContainer(ContainerName);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Hdb_CustomAppraisement>)ObjIR.DBResponse.Data;
            }

            return PartialView(LstAppraisement);

        }

        [HttpGet]
        public JsonResult LoadListMoreDataforCustomApprisement(int Page)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_CustomAppraisement> LstAppraisement = new List<Hdb_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Hdb_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return Json(LstAppraisement, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            Hdb_CustomAppraisement ObjAppraisement = new Hdb_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (Hdb_CustomAppraisement)ObjIR.DBResponse.Data;
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
                }
            }
            return PartialView("EditCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/EditCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public ActionResult ViewCustomAppraisement(int CustomAppraisementId)
        {
            Hdb_CustomAppraisement ObjAppraisement = new Hdb_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (Hdb_CustomAppraisement)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/ViewCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
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
        public JsonResult AddEditCustomAppraisement(Hdb_CustomAppraisement ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<Hdb_CustomAppraisementDtl> LstAppraisement = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = UtilityClasses.Utility.CreateXML(LstAppraisement);
                }

                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCustomAppraisement(ObjAppraisement, AppraisementXML);
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

        #endregion

        #region  Custom Appraisement Work Order

        [HttpGet]
        public ActionResult CreateCstmAppraiseWorkOrder()
        {
            Hdb_CstmAppraiseWorkOrder ObjAppraisement = new Hdb_CstmAppraiseWorkOrder();
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            //Hdb_YardRepository ObjYR = new YardRepository();
            ObjAppraisement.WorkOrderDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetAppraisementNoForWorkOrdr();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.AppraisementList = new SelectList((List<Hdb_CstmAppraiseWorkOrder>)ObjIR.DBResponse.Data, "CustomAppraisementId", "AppraisementNo");
            }
            else
            {
                ViewBag.AppraisementList = null;
            }
            ObjIR.GetAllYard();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.YardList = new SelectList((List<HDBYard>)ObjIR.DBResponse.Data, "YardId", "YardName");
            }
            else
            {
                ViewBag.YardList = null;
            }
            return PartialView("CreateCstmAppraiseWorkOrder", ObjAppraisement);
        }

        [HttpGet]
        public JsonResult GetAppraisementList(int CustomAppraisementId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetCstmAppraiseDtlForWorkOrdr(CustomAppraisementId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCstmAppraiseWorkOrderList()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_CstmAppraiseWorkOrder> LstAppraisement = new List<Hdb_CstmAppraiseWorkOrder>();
            ObjIR.GetAllCstmAppraiseWorkOrder();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Hdb_CstmAppraiseWorkOrder>)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraiseWorkOrderList", LstAppraisement);
        }

        [HttpGet]
        public ActionResult EditCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            Hdb_CstmAppraiseWorkOrder ObjWorkOrder = new Hdb_CstmAppraiseWorkOrder();
            if (CstmAppraiseWorkOrderId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjWorkOrder = (Hdb_CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                    //ObjIR.GetYardWiseLocation(ObjWorkOrder.YardId);
                    //if (ObjIR.DBResponse.Data != null)
                    //{
                    //    ViewBag.YardList = new SelectList((List<Areas.Import.Models.YardWiseLocation>)ObjIR.DBResponse.Data, "YardId", "YardName");
                    //}
                    //else
                    //{
                    //    ViewBag.YardList = null;
                    //}
                }
            }
            return PartialView("EditCstmAppraiseWorkOrder", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            Hdb_CstmAppraiseWorkOrder ObjWorkOrder = new Hdb_CstmAppraiseWorkOrder();
            if (CstmAppraiseWorkOrderId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjWorkOrder = (Hdb_CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCstmAppraiseWorkOrder", ObjWorkOrder);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCstmAppraiseWorkOrder(int CstmAppraiseWorkOrderId)
        {
            if (CstmAppraiseWorkOrderId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.DelCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCstmAppraiseWorkOrder(Hdb_CstmAppraiseWorkOrder ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                //List<int> LstYard = new List<int>();
                //string YardXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<Hdb_CustomAppraisementDtl> LstAppraisement = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hdb_CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                //if (ObjAppraisement.YardWiseLocationIds != null)
                //{
                //    string[] YardList = ObjAppraisement.YardWiseLocationIds.Split(',');
                //    foreach (string Data in YardList)
                //        LstYard.Add(Convert.ToInt32(Data));
                //    YardXML = Utility.CreateXML(LstYard);
                //}
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCstmAppraiseWorkOrdr(ObjAppraisement, AppraisementXML /*, YardXML*/ );
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
        public JsonResult PrintCustomAppraisement(int CstmAppraiseWorkOrderId)
        {
            Hdb_CstmAppraiseWorkOrder ObjCustom = new Hdb_CstmAppraiseWorkOrder();
            string FilePath = "";
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetCstmAppraiseWOForPreview(CstmAppraiseWorkOrderId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjCustom = (Hdb_CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
                FilePath = GeneratePDFForCstmAppraiseWO(ObjCustom, CstmAppraiseWorkOrderId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }

        [NonAction]
        private string GeneratePDFForCstmAppraiseWO(Hdb_CstmAppraiseWorkOrder ObjCustom, int CstmAppraiseWorkOrderId)
        {
            string Html = "";
            string CHA = "", Importer = "", NoOfPackages = "", GrossWeight = "", Vessel = "", ContainerNo = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            ObjCustom.LstAppraisementDtl.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item =>
            {
                if (Importer == "")
                    Importer = item.Importer;
                else
                    Importer += "," + item.Importer;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
            {
                if (CHA == "")
                    CHA = item.CHA;
                else
                    CHA += "," + item.CHA;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "," + item.Vessel;
            });
            ObjCustom.LstAppraisementDtl.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerNo == "")
                    ContainerNo = item.ContainerNo;
                else
                    ContainerNo += "," + item.ContainerNo;
            });
            GrossWeight = ((ObjCustom.LstAppraisementDtl.Sum(x => x.GrossWeight) > 0) ? ObjCustom.LstAppraisementDtl.Sum(x => x.GrossWeight).ToString() + " KG" : "");
            NoOfPackages = ((ObjCustom.LstAppraisementDtl.Sum(x => x.NoOfPackages) > 0) ? ObjCustom.LstAppraisementDtl.Sum(x => x.NoOfPackages).ToString() : "");
            var Location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId + "/CustomAppraisementWorkOrder.pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID + "/" + CstmAppraiseWorkOrderId);
            }
            //if (System.IO.File.Exists(Location))
            //{
            //    System.IO.File.Delete(Location);
            //}
            string DeliveryTpe = ((ObjCustom.DeliveryType == 1) ? "Cargo Delivery" : "Container Delivery");
            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:14pt;'>केंद्रीय भंडारण निगम</span><br/><span>भारत सरकार के उपक्रम</span><br/><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No. ..................</th><th style='text-align:right;'>Container Freight Station<br/>18, Coal Dock Road,<br/>Kolkata - 700 043</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To सेवा मे,</th><th style='text-align:right;'>Dated "+DateTime.Now.ToString("dd/MM/yyyy")+"</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;'>कार्य-आदेश <br/><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir, महोदय,</td></tr><tr><td style='width:5%;'></td><td>कृपया तुरंत नीचे उल्लेखित कार्य निष्पादित करने की व्यवस्था करें:<br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'>1.</td><td>आयातक, निर्यातक का नाम<br/>Importer's / Exporter's Name "+Importer+"</td></tr><tr> <td style='vertical-align: bottom;'>2.</td> <td>सीएए का नाम<br/>CHA's Name "+CHA+"</td></tr><tr><td style='vertical-align: bottom;'>3.</td><td>डिलिवरी / डिस्टफिंग / कार्टिंग / स्टफ़िंग<br/>Custom Appraisement.</td></tr><tr><td style='vertical-align: bottom;'>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td>पैकेट की संख्या</td><td>भ।र</td></tr><tr><td>No. of packages "+NoOfPackages+"</td><td> Weight "+GrossWeight+"</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'>5.</td><td>ट्रक नं<br/> Truck No. "+ Vessel + "</td></tr><tr><td style='vertical-align: bottom;'>6.</td> <td>स्थान<br/>Location "+ObjCustom.YardWiseLctnNames+"</td></tr><tr><td style='vertical-align: bottom;'>7.</td><td>कंटेनर<br/>Container no. :"+ContainerNo+"</td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>हस्ताक्षर<br/>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + ObjCustom.AppraisementNo + "</th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,</th><th style='text-align:right;'>Dated: " + DateTime.Now.ToString("dd/MM/yyyy") + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + Importer + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHA + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Custom Appraisement " + DeliveryTpe + ".</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfPackages + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: " + ObjCustom.YardWiseLctnNames + "</td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : " + ContainerNo + "</td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var ReportingHelper = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                ReportingHelper.GeneratePDF(Location, Html);
            }
            return "/Docs/" + Session.SessionID + "/" + CstmAppraiseWorkOrderId + "/CustomAppraisementWorkOrder.pdf";
        }

        #endregion

        #region Custom Appraisement Approval
        [HttpGet]
        public ActionResult ListOfApprsmntAppr()
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.NewCustomeAppraisement(0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfApp = Jobject["lstApproval"];

                // ViewBag.ListOfApp =objIR.DBResponse.Data;
                // var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.ListOfApp = jobject["Details"];
            }
            else
            {
                ViewBag.ListOfApp = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult LoadMoredata(int Skip, string status)
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            if (status == "N")
            {
                objIR.NewCustomeAppraisement(Skip);
            }
            else
            {
                objIR.ApprovalHoldCustomAppraisement(Skip, status);
            }
            TempData["Status"] = status;

            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadApprovalPage(int CstmAppraiseWorkOrderId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            Hdb_CstmAppraiseWorkOrder ObjWorkOrder = new Hdb_CstmAppraiseWorkOrder();
            ObjIR.GetCstmAppraiseWorkOrder(CstmAppraiseWorkOrderId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjWorkOrder = (Hdb_CstmAppraiseWorkOrder)ObjIR.DBResponse.Data;
            }
            //string Status = TempData["Status"].ToString();
            //TempData.Keep(Status);
            ViewBag.Status = TempData.Peek("Status").ToString();

            return PartialView("CstmAppraisementApproval", ObjWorkOrder);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApproval(int CstmAppraiseWorkOrderId, int IsApproved)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.UpdateCustomApproval(CstmAppraiseWorkOrderId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

        #region Issue Slip

        [HttpGet]
        public ActionResult CreateIssueSlip()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetInvoiceNoForIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<HdbIssueSlip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            }
            else
            {
                ViewBag.InvoiceNoList = null;
            }
            return PartialView("CreateIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public JsonResult GetInvcDetForIssueSlip(int InvoiceId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssueSlipList()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            ObjIR.GetAllIssueSlip(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<HdbIssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }
        [HttpGet]
        public ActionResult GetIssueListPartyCode(string PartyCode)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            ObjIR.GetAllIssueSlipsearch(PartyCode);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<HdbIssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }
        [HttpGet]
        public JsonResult LoadListMoreDataForIssueSlipFCL(int Page)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<HdbIssueSlip> LstIssueSlip = new List<HdbIssueSlip>();
            ObjIR.GetAllIssueSlip(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<HdbIssueSlip>)ObjIR.DBResponse.Data;
            }

            return Json(LstIssueSlip, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public ActionResult EditIssueSlip(int IssueSlipId)
        {
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
            if (IssueSlipId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (HdbIssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewIssueSlip(int IssueSlipId)
        {
            HdbIssueSlip ObjIssueSlip = new HdbIssueSlip();
            if (IssueSlipId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (HdbIssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewIssueSlip", ObjIssueSlip);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteIssueSlip(int IssueSlipId)
        {
            if (IssueSlipId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.DelIssueSlip(IssueSlipId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditIssueSlip(HdbIssueSlip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIssueSlip.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditIssueSlip(ObjIssueSlip);
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

        #endregion

        #region Form One FCL

        [HttpGet]
        public ActionResult Hdb_CreateFormOneFCL()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddFormOneFCL()
        {
            Hdb_FormOneModel objFormOne = new Hdb_FormOneModel();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");
            objFormOne.BLDate = DateTime.Now.ToString("dd/MM/yyyy");
            objFormOne.TSADate = DateTime.Now.ToString("dd/MM/yyyy");
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objImport.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objImport.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;
            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;
            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;
            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfForwarder();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstTSAForwarder = (List<TSAForwarder>)objImport.DBResponse.Data;

            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;
            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();

            objImport.GetContainer(BranchId);
            if (objImport.DBResponse.Data != null)
            {
                Lstcontainer = (List<ContainerRefGateEntry>)objImport.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;
            return PartialView(objFormOne);
        }
        public JsonResult GetFieldsForContainerFCL(string ContainerName)
        {
            if (ContainerName != "")
            {
                HDB_ImportRepository ObjGOR = new HDB_ImportRepository();
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ObjGOR.GetAutoPopulateDataFCL(ContainerName, BranchId);
                return Json(ObjGOR.DBResponse.Data, JsonRequestBehavior.AllowGet);
                // ViewBag.JSONResult = ObjGOR.DBResponse.Data;
                //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                //objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                //string strDate = objEntryThroughGate.ReferenceDate;
                //string[] arrayDate = strDate.Split(' ');
                //objEntryThroughGate.ReferenceDate = arrayDate[0];
                //ViewBag.strTime = objEntryThroughGate.EntryTime;

                //return Json(objEntryThroughGate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
                // ViewBag.JSONResult = "Error";
                //return PartialView("CreateEntryThroughGate");
            }

        }
        [HttpGet]
        public ActionResult Hdb_GetFormOneListFCL(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
                HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
                objImportRepo.GetFormOneFCL(0);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Hdb_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
                HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
                objImportRepo.GetFormOneByContainerFCL(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Hdb_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }



        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            IEnumerable<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
            HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
            objImportRepo.GetFormOneFCL(Page);
            if (objImportRepo.DBResponse.Data != null)
                lstFormOne = (IEnumerable<Hdb_FormOneModel>)objImportRepo.DBResponse.Data;

            return Json(lstFormOne, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOneFCL(Hdb_FormOneModel objFormOne)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ModelState.Remove("CargoType");
                if (ModelState.IsValid)
                {
                    // objFormOne.FormOneDetailsJS.Replace("\"DateOfLanding: \":\"\"", "\"DateOfLanding\":\"null\"");
                    objFormOne.lstFormOneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_FormOneDetailModel>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetail.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetail);
                    HDB_ImportRepository objImport = new HDB_ImportRepository();
                    objImport.AddEditFormOneFCL(objFormOne, BranchId, XML, ((Login)(Session["LoginUser"])).Uid);
                    ModelState.Clear();
                    return Json(objImport.DBResponse);
                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1 };
                    return Json(Err);
                }
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditFormOneFCL(int FormOneId)
        {
            Hdb_FormOneModel objFormOne = new Hdb_FormOneModel();
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);

            objImport.GetFormOneByIdFCL(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Hdb_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;

            objImport.ListOfPOD();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;

            objImport.ListOfCHA();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;

            objImport.ListOfImporter();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfForwarder();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstTSAForwarder = (List<TSAForwarder>)objImport.DBResponse.Data;

            objImport.ListOfCommodity();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.Commodity>)objImport.DBResponse.Data;

            List<ContainerRefGateEntry> Lstcontainer = new List<ContainerRefGateEntry>();

            objImport.GetContainer(BranchId);

            if (objImport.DBResponse.Data != null)
            {
                Lstcontainer = (List<ContainerRefGateEntry>)objImport.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult ViewFormOneFCL(int FormOneId)
        {
            Hdb_FormOneModel objFormOne = new Hdb_FormOneModel();
            HDB_ImportRepository objImport = new HDB_ImportRepository();

            objImport.GetFormOneByIdFCL(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (Hdb_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);
            return PartialView(objFormOne);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOneFCL(int FormOneId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOneFCL(FormOneId);
            return Json(objImport.DBResponse);
        }
        [HttpGet]
        public JsonResult GetICEGateDataFCL(string ContainerNo, string Size)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.GetICEGateDataFCL(ContainerNo, Size);
            return Json((List<Hdb_FormOneDetailModel>)objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PrintFormOneFCL(int FormOneId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            if (FormOneId > 0)
                objImport.FormOnePrintFCL(FormOneId);
            var model = (Hdb_FormOnePrintModel)objImport.DBResponse.Data;
            var printableData1 = (IList<Hdb_FormOnePrintDetailModel>)model.lstFormOnePrintDetail;

            var fileName = model.FormOneNo + ".pdf";
            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
            string Path = GeneratePDFForForm1FCL(printableData1, FormOneId);



            //   string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

            //if (!Directory.Exists(PdfDirectory))
            //  Directory.CreateDirectory(PdfDirectory);



            //   var cPdf = new CustomPdfGeneratorKdl();
            //   cPdf.Generate(PdfDirectory + "/" + fileName, model.ShippingLineNo, printableData1);

            return Json(new { Status = 1, FileUrl = Path }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public string GeneratePDFForForm1FCL(IList<Hdb_FormOnePrintDetailModel> LstJobOrder, int FormoneId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/Form-1FCL" + FormoneId + ".pdf";

            List<PrintJobOrder> List = new List<PrintJobOrder>();
            //int Count = 0;
            // string Sline = "";
            StringBuilder Html = new StringBuilder();
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }



            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            //if (System.IO.File.Exists(Path))
            //{
            //    System.IO.File.Delete(Path);
            //}
            /*  if ((Convert.ToInt32(Session["BranchId"])) == 1)
              {
                  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td width='20%' style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";
              }
              else
              {
                  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='90%' valign='top' align='center'><h1 style='font-size: 18px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + CompanyAddress + "</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></thead><tbody><tr><td style='text-align:left;' colspan='2'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right; width:20%;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='4' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='4' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Loaded Container / Loaded CBT </td></tr><tr><td colspan='4' style='text-align:left;'><span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> <span style='border-bottom:1px solid #000;'> " + objMdl.ToLocation + " </span></td></tr><tr><td colspan='4'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER/CBT NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='4'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr></tr><tr><span><br/><br/><br/><br/><br/><br/></span></tr><tr><td colspan='6' style='padding-top:150px;'>Copy to:- <span></span></td><td colspan='6' style='padding-top:150px;text-align:right;'>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
              } */

            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody> <tr><th colspan='12' style='font-size: 12px; text-align: right;'>CONTROLLED</th></tr><tr><td colspan='12'>");
            Html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody> <tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td><td width='16%' align='right'> <table style='width:100%;' cellspacing='0' cellpadding='0'><tbody> <tr><td style='border:1px solid #333;'>");
            Html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Doc No. F/CD/CFS/20</div></td></tr></tbody></table> </td></tr></tbody></table> </td></tr><tr><td colspan='12'> <table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            Html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally<br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>PNR REGISTER for Import Containers</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("<tr><td colspan='12'><table style='border:1px solid #000; border-bottom:0; width:100%; font-size:8pt; font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            Html.Append("<thead><tr>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:5%;'>Sl. No.</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>Date of receipt of containers</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>PNR No & Date</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>Name of Importer</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Name of Shipping Lines</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>Cargo</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Container No.</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>Size</th>");
            Html.Append("<th style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>Customs Seal No.</th>");
            Html.Append("<th style='text-align:center;border-bottom:1px solid #000;padding:10px;width:10%;'>Shipping Line real No.</th>");
            Html.Append("</tr></thead>");

            Html.Append("<tbody>");

            int i = 1;
            LstJobOrder.ToList().ForEach(item =>
            {
                Html.Append("<tr>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:5%;'>" + i + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>" + item.FormOneDate + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'></td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:13%;'>" + item.ImpName + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.ShippingLineName + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>" + item.HazType + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.ContainerNo + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:8%;'>" + item.ContainerSize + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'>" + item.SealNo + "</td>");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;width:10%;'></td>");
                Html.Append("</tr>");


                //Html.Append("<tr><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:5%;'>" + i + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:13%;'>" + item.FormOneDate + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'></td><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:13%;'>" + item.ImpName + "</td>");
                //Html.Append("<td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'>" + item.ShippingLineName + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:8%;'>" + item.HazType + "</td><td style='text-align:center;border:1px solid #000;border-left:none;padding:10px;width:10%;'>" + item.ContainerNo + "</td><td style='text-align:center;border:1px solid #000;padding:10px;width:8%;'>" + item.ContainerSize + "</td>");
                //Html.Append("<td style='text-align:center;border:1px solid #000;padding:10px;width:10%;'>" + item.SealNo + "</td><td style='text-align:center;border:1px solid #000;padding:10px;border-right:none;width:10%;'></td></tr>");

                i = i + 1;
            });

            Html.Append("</tbody>");
            Html.Append("</table></td></tr></tbody></table>");


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = this.HeadOffice;
                rh.HOAddress = this.HOAddress;
                rh.ZonalOffice = this.ZonalOffice;
                rh.ZOAddress = this.ZOAddress;
                rh.GeneratePDF(Path, Html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/Form-1FCL" + FormoneId + ".pdf";
        }


        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateFormOneFCL(FormCollection fc)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            try
            {
                var pages = new string[1];
                var fileName = fc["FormOne"].ToString() + ".pdf";
                string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4Landscape))
                {
                    rh.GeneratePDF(PdfDirectory + "/" + fileName, fc["Page1"].ToString());
                }
                return Json(new { Status = 1, Message = "/Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "" }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion

        #region Container Indent 
        [HttpGet]
        public ActionResult CreateContainerIndent()
        {
            HDB_ImportRepository objIR = new HDB_ImportRepository();
            objIR.ListofForm1();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListofForm1 =JsonConvert.SerializeObject(objIR.DBResponse.Data);
            else ViewBag.ListofForm1 = null;
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetCntrDetForForm1(int IndentId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContainerDetails(IndentId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ContainerIndentList()
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_ListContainerIndent> LstCont = new List<Hdb_ListContainerIndent>();
            ObjIR.ListofContainerIndent(0);
            if (ObjIR.DBResponse.Data != null)
                LstCont = (List<Hdb_ListContainerIndent>)ObjIR.DBResponse.Data;
            return PartialView(LstCont);
        }
        [HttpGet]
        public ActionResult ContainerIndentContainer(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<Hdb_FormOneModel> lstFormOne = new List<Hdb_FormOneModel>();
                HDB_ImportRepository objImportRepo = new HDB_ImportRepository();
                objImportRepo.GetFormOneFCL(0);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<Hdb_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                List<Hdb_ListContainerIndent> LstCont = new List<Hdb_ListContainerIndent>();
                ObjIR.GetIndentOneByContainer(ContainerName);
                if (ObjIR.DBResponse.Data != null)
                    LstCont = (List<Hdb_ListContainerIndent>)ObjIR.DBResponse.Data;

                return PartialView(LstCont);
            }
        }



        [HttpGet]
        public JsonResult LoadListMoreDataForIndent(int Page)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            List<Hdb_ListContainerIndent> LstCont = new List<Hdb_ListContainerIndent>();
            ObjIR.ListofContainerIndent(Page);
            if (ObjIR.DBResponse.Data != null)
                LstCont = (List<Hdb_ListContainerIndent>)ObjIR.DBResponse.Data;

            return Json(LstCont, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditContainerIndent(int IndentId)
        {
            Hdb_ContainerIndent objCont = new Hdb_ContainerIndent();
            if (IndentId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetContainerIndentDet(IndentId);
                if (ObjIR.DBResponse.Data != null)
                    objCont = (Hdb_ContainerIndent)ObjIR.DBResponse.Data;
            }
            return PartialView(objCont);
        }
        [HttpGet]
        public ActionResult ViewContainerIndent(int IndentId)
        {
            Hdb_ContainerIndent objCont = new Hdb_ContainerIndent();
            if (IndentId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.GetContainerIndentDet(IndentId);
                if (ObjIR.DBResponse.Data != null)
                    objCont = (Hdb_ContainerIndent)ObjIR.DBResponse.Data;
            }
            return PartialView(objCont);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteContainerIndent(int IndentId)
        {
            if (IndentId > 0)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.DeleteContainerIndent(IndentId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditContainerIndent(Hdb_ContainerIndent ObjCI)
        {
            if (ModelState.IsValid)
            {
                HDB_ImportRepository ObjIR = new HDB_ImportRepository();
                ObjIR.AddEditContainerIndent(ObjCI, Convert.ToInt32(Session["BranchId"]), Convert.ToInt32(((Login)(Session["LoginUser"])).Uid));
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
        public JsonResult PrintContainerIndent(int IndentId)
        {
            HDB_ImportRepository ObjIR = new HDB_ImportRepository();
            ObjIR.GetContainerIndentForPrint(IndentId);
            if (ObjIR.DBResponse.Data != null)
            {
                Hdb_ContainerIndent ObjIndent = new Hdb_ContainerIndent();
                ObjIndent = (Hdb_ContainerIndent)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForContainerIndent(ObjIndent, IndentId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePDFForContainerIndent(Hdb_ContainerIndent ObjInd, int IndentId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerIndent" + IndentId + ".pdf";
            StringBuilder objSB = new StringBuilder();
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            /**************************************************/
            /*******************Html Bind *********************/
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            objSB.Append("<tr><td width='65%' style='font-size: 10px;'></td>");
            objSB.Append("<td width='18%' align='right'>");
            objSB.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            objSB.Append("<tr><td style='border:1px solid #333;'>");
            objSB.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/37-RMR-H</div>");
            objSB.Append("</td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally<br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER INDENT FORM</label></td>");
            objSB.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            objSB.Append("<tr><th style='font-size:13px;' align='left' width='5%'>No.</th>");
            objSB.Append("<td style='font-size:12px;'><u>" + ObjInd.IndentNo + "</u></td><th style='font-size:13px; text-align:right;'>Date</th>");
            objSB.Append("<td style='font-size:12px; width:10%;'><u>" + ObjInd.IndentDate + "</u></td></tr>");
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12' style='font-size: 12px; font-weight: bold;'>");
            objSB.Append("<span>TO</span><br/>");
            objSB.Append("<span>The Manager,</span>");
            objSB.Append("<h4 style='margin: 0; line-height: normal; '>Inland Container Depot,</h4> ");
            objSB.Append("<h4 style='margin: 0; line-height: normal; '>Sanathnagar,</h4>");
            objSB.Append("<h4 style='margin: 0; line-height: normal; '>HYDERABAD.</h4>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td><span><br/></span></td></tr>  <tr><td>Sir,</td></tr>  <tr><td><span><br/></span></td></tr>");
            objSB.Append("<tr><td colspan='12'>kindly arrange to load the following container on our trailers to transport the same to CFS, Kukatpally for Destuffing / Factory Destuffing on <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u></td></tr>");

            objSB.Append("<tr>");
            objSB.Append("<td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'>");
            objSB.Append("<tbody>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr><td width='4%'></td><td colspan='1' width='20%'></td>");
            objSB.Append("<td colspan='2' width='70%'><table style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr><td width='3%'></td><td colspan='1' width='40%'></td><td width='5%'></td><th colspan='2' valign='top' width='40%' align='center'>Size</th></tr>");
            objSB.Append("</tbody></table></td></tr>");
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td width='4%' valign='top' align='right'>1.</td>");
            objSB.Append("<td colspan='1' valign='top' width='20%'>Container No.</td>");
            objSB.Append("<td colspan='2' valign='top' width='70%'>");
            objSB.Append("<table style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0; '>");
            objSB.Append("<tbody>");

            //Container bind
            int i = 1;
            ObjInd.lstContainerDetails.ForEach(item => {
                objSB.Append("<tr>");
                objSB.Append("<td width='3%' valign='top' align='left'>" + i + ")</td>");
                objSB.Append("<td colspan='1' valign='top' width='40%' style='border-bottom:1px solid #000; padding-bottom:5px;'>" + item.ContainerNo + "</td>");
                objSB.Append("<td width='5%'></td>");
                objSB.Append("<td colspan='2' valign='top' width='40%' style='border-bottom:1px solid #000; padding-bottom:5px;'>" + item.ContainerSize + "</td>");
                objSB.Append("</tr>");
                i++;
            });
            objSB.Append("</tbody></table></td></tr>");
            objSB.Append("</tbody></table></td></tr>");

            string CHAName = "", ImpName = "";
            ObjInd.lstContainerDetails.Select(x => x.CHAName).Distinct().ToList().ForEach(elem =>
            {
                if (CHAName == "")
                    CHAName = elem;
                else
                    CHAName += "," + elem;
            });
            ObjInd.lstContainerDetails.Select(x => x.IMPName).Distinct().ToList().ForEach(elem =>
            {
                if (ImpName == "")
                    ImpName = elem;
                else
                    ImpName += "," + elem;
            });

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td width='4%' valign='top' align='right'>2.</td>");
            objSB.Append("<td colspan='1' valign='top' width='20%'>CHA's Name : M/s.</td>");
            objSB.Append("<td colspan='2' valign='top' width='70%' style='border-bottom:1px solid #000; padding-bottom:5px;'>" + CHAName + "</td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td width='4%' valign='top' align='right'>3.</td>");
            objSB.Append("<td colspan='1' valign='top' width='20%'>Importer's Name : M/s.</td>");
            objSB.Append("<td colspan='2' valign='top' width='70%' style='border-bottom:1px solid #000; padding-bottom:5px;'>" + ImpName + "</td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt; font-family: Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td width='4%' valign='top' align='right'>4.</td>");
            objSB.Append("<td colspan='1' valign='top' width='20%'>Trailer No.</td>");
            objSB.Append("<td colspan='2' valign='top' width='70%'>");
            objSB.Append("<table style='width:100%; font-size:9pt; font-family:Arial, Helvetica, sans-serif; margin:0;'>");
            objSB.Append("<tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td colspan='5' style='40%'>" + ObjInd.TrailerNo.ToString() + "</td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody>");
            objSB.Append("</table>");
            objSB.Append("</td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody></table></td></tr>");
            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing ='0' cellpadding ='5' style ='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>");
            objSB.Append("<tbody>");
            objSB.Append("<tr>");
            objSB.Append("<td width='4%' valign='top' align='right' style='font-size:13px; padding:5px;'>5.</td>");
            objSB.Append("<td colspan='11' valign='top' width='90%' style='font-size:13px; padding:5px 0;'> ICD a) Time in ___________ Date _____________ <span> &nbsp; &nbsp; &nbsp; &nbsp;</span> b) Time out __________ Date _______________</td>");
            objSB.Append("</tr>");
            objSB.Append("<tr>");
            objSB.Append("<td width='1%' valign='top' align='right' style='font-size:13px; padding:5px;'>6.</td>");
            objSB.Append("<td colspan='11' valign='top' width='90%' style='font-size:13px; padding:5px 0;'>Remarks, if any <u>" + ObjInd.Remarks + "</u></td>");
            objSB.Append("</tr>");
            objSB.Append("</tbody>");
            objSB.Append("</table>");
            objSB.Append("</td>");
            objSB.Append("</tr>");

            objSB.Append("<tr><td><br/><br/></td></tr>");
            objSB.Append("<tr><td colspan='12' style='font-size:12px; text-align:right;'>yours faithfully,</td></tr>");
            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family:Arial, Helvetica, sans-serif; margin:0;'><tbody>");
            objSB.Append("<tr><th align='left' colspan='6' style='font-size:12px;' width='50%'>Signature of Handling<br/> Contractor </th><th align='right' colspan='6' style='font-size:12px;' width='50%' > MANAGER, CFS.</th></tr>");
            objSB.Append("</tbody></table></td></tr>");
            objSB.Append("</tbody></table>");

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            objSB.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            //objSB.Append("</tr></tbody></table></td></tr>");
            /**************************************************/

            StringBuilder objSB2 = new StringBuilder();
            objSB2.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            objSB2.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:13px;'>From</th>");
            objSB2.Append("<td style='font-size:12px;'><u></u></td><th style='font-size:13px; text-align:right;'>Date</th>");
            objSB2.Append("<td style='font-size:12px; width:10%;'><u>"+ObjInd.IndentDate.ToString()+"</u></td></tr></tbody></table></td></tr>");

            objSB2.Append("<tr><td colspan='12' style='font-size: 12px;'>");
            objSB2.Append("<b> M/s </b> "+ CHAName + "");
            objSB2.Append("</td></tr>");

            objSB2.Append("<tr><td colspan='12' style='font-size:12px;'>");
            objSB2.Append("<span>TO</span><br/><span>The Manager,</span><h4 style='margin:0; line-height: normal;'>Container Freight Station, Kukatpally</h4><h4 style='margin:0; line-height: normal;'>IDPL Road, Hyderabad - 37,</h4>");
            objSB2.Append("</td></tr>");

            objSB2.Append("<tr><td colspan='12' style='font-size:12px;' align='center'>Sub : Movement of Import Containers from ICD, Sanathnagar to CFS, Kukatpally - Reg</td></tr>");
            objSB2.Append("<tr><td><span><br/></span></td></tr>  <tr><td style='font-size:12px;'>Sir,</td></tr>  <tr><td><span><br/></span></td></tr>");
            objSB2.Append("<tr><td colspan='12' style='font-size:12px;'><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>We request you to kindly arrange transportation of the following Import Containers from ICD Sanathnagar to CFS Kukatpally for Destuffing Factory Destuffing on <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u></td></tr>");

            objSB2.Append("<tr><td colspan='12'>");
            objSB2.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            objSB2.Append("<thead><tr>");
            objSB2.Append("<th style='text-align:center;border-top:1px solid #000;border-bottom:1px solid #000;padding:10px; width:50px;'>Sl.No.</th>");
            objSB2.Append("<th colspan='4' style='text-align:center;border-top:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Containers Nos.</th>");
            objSB2.Append("<th colspan='4' style='text-align:center;border-top:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Name of the Importer</th>");
            objSB2.Append("<th colspan='4' style='text-align:center;border-top:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Details of the Cargo</th>");
            objSB2.Append("</tr></thead>");
            objSB2.Append("<tbody>");

            i = 1;
            ObjInd.lstContainerDetails.ForEach(item =>
            {
                objSB2.Append("<tr><td style='text-align:center;padding:10px;width:50px;'>" + i + "</td>");
                objSB2.Append("<td colspan='4' style='text-align:center;padding:10px;'>" + item.ContainerNo + "</td>");
                objSB2.Append("<td colspan='4' style='text-align:center;padding:10px;'>" + item.IMPName + "</td>");
                objSB2.Append("<td colspan='4' style='text-align:center;padding:10px;'>" + item.CargoDesc+"</td></tr>");
                i++;
            });
            objSB2.Append("<tr><th colspan='12' width='100%' style='text-align:center;border-top:1px solid #000;border-bottom:1px solid #000;padding:10px;'>After transportation of the above containers, kindly arrange destuffing of the same at CFS, Kukatpally / Factory Destuffing at <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u></th></tr>");
            objSB2.Append("</tbody>");            
            objSB2.Append("</table></td></tr>");

            objSB2.Append("<tr><td><br/></td></tr>");
            objSB2.Append("<tr><td colspan='12' style='font-size:12px;'>Thanking You Sir,</td></tr>");
            objSB2.Append("<tr><td align='left' colspan='10' style='font-size:12px;' width='90%'>Yours faithfully<br/>for M/s "+ CHAName + " </td>");
            objSB2.Append("<th align='right' colspan='2' style='font-size:13px;border-bottom:1px solid #000;' width='10%'>Permitted</th></tr>");

            objSB2.Append("<tr><td><br/><br/><br/><br/><br/></td></tr>");

            objSB2.Append("<tr><th align='left' colspan='6' style='font-size:12px;' width='50%'>(AUTHORISED SIGNATORY)</th>");
            objSB2.Append("<th align='right' colspan='6' style='text-align:right;font-size:11px;' width='50%'>SUPERINTENDENT <br/> CUSTOMS, CFS, KKP</th></tr>");

            objSB2.Append("<tr><td colspan='12'><br/><br/><br/></td></tr>");
            objSB2.Append("<tr><th align='left' colspan='12' style='font-size:12px;'>COPY TO</th></tr>");

            objSB2.Append("<tr><td colspan='12' style='font-size: 12px;'><h4 style='margin:0; line-height: normal;'>The Manager,</h4><h4 style='margin:0; line-height: normal;'>Inland Container Depot,</h4><h4 style='margin:0; line-height: normal;'>Sanathnagar,HYDERABAD.</h4></td></tr>");

            objSB2.Append("</tbody></table>");

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
            List<string> lst = new List<string>();
            lst.Add(objSB.ToString());
            lst.Add(objSB2.ToString());
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f,false,true))
            {
                RH.HeadOffice = HeadOffice;
                RH.HOAddress = HOAddress;
                RH.ZonalOffice = ZonalOffice;
                RH.ZOAddress = ZOAddress;
                RH.GeneratePDF(Path,lst);
            }
            return "/Docs/" + Session.SessionID + "/ContainerIndent" + IndentId + ".pdf";
        }
        #endregion

        #region Empty Container Payment Sheet Gate Out

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheetGateOut(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            HDB_ImportRepository objImport = new HDB_ImportRepository();

            //objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.StuffingReqList = null;

            objImport.GetEmptyContainerListForInvoiceGateOut();
            if (objImport.DBResponse.Status > 0)
                ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.EmptyContList = null;

            //objImport.GetPaymentPartyForImportInvoiceGateOut();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
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

            return PartialView();
        }

        public JsonResult GetPaymentSheetEmptyContGateOut(string InvoiceFor, int AppraisementId)
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            //objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            objImport.GetEmptyContByEntryIdGateOut(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheetGateOut(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId,
         List<Hdb_PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor,string ExportUnder, int InvoiceId = 0)
        {           
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            HDB_ImportRepository objImport = new HDB_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objImport.GetEmptyContPaymentSheetGateOut(InvoiceDate, AppraisementId, InvoiceType, XMLText, InvoiceId, InvoiceFor, PartyId, ExportUnder);
            var Output = (Hdb_InvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            /*if (InvoiceFor == "YARD")
            {
                Output.Module = "IMPYard";
            }
            else
            {
                Output.Module = "IMPDeli";
            }*/
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
                    Output.lstPostPaymentCont.Add(new Hdb_PostPaymentContainer
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
                        StuffCUM = item.StuffCUM,
                        ISODC=item.ISODC
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
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECDeliveryPaymentSheetGateOut(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);                
                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<Hdb_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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

                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "EC";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                HDB_ImportRepository objImport = new HDB_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objImport.AddEditEmptyContPaymentSheetGateOut(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);


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

        #region OBL Amendment Start
        public ActionResult OBLAmendment()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult OBLAmendmentDetail(string OBLNo="")
        {
            HDB_ImportRepository objImport = new HDB_ImportRepository();
            objImport.OBLAmendmentDetail(OBLNo);           
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOBLAmendmentList(string SearchValue = "", int Page = 0)
        {
            HDB_ImportRepository ObjCR = new HDB_ImportRepository();
            List<Hdb_OblAmendment> lstOBLAmendment = new List<Hdb_OblAmendment>();
            ObjCR.GetOBLAmendmentList(SearchValue, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstOBLAmendment = (List<Hdb_OblAmendment>)ObjCR.DBResponse.Data;
            }
            return PartialView("OBLAmendmentList", lstOBLAmendment);
        }


        [HttpGet]
        public ActionResult GetLoadMoreOBLAmendmentList(string SearchValue = "", int Page = 0)
        {
            HDB_ImportRepository ObjCR = new HDB_ImportRepository();
            List<Hdb_OblAmendment> lstOBLAmendment = new List<Hdb_OblAmendment>();
            ObjCR.GetOBLAmendmentList(SearchValue, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstOBLAmendment = (List<Hdb_OblAmendment>)ObjCR.DBResponse.Data;
            }
            return Json(lstOBLAmendment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImporterForAmendment(int Page, string PartyCode)
        {
            HDB_ImportRepository obj = new HDB_ImportRepository();
            obj.GetImporterForAmendment(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]       
        public JsonResult AddEditOBLAmendment(Hdb_OblAmendment obj)
        {
            if (ModelState.IsValid)
            {
                HDB_ImportRepository objER = new HDB_ImportRepository();
                objER.AddEditOBLAmendment(obj);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        #endregion

    }
}



