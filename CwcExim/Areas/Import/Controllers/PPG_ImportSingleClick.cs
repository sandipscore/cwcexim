using System;
using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;

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

namespace CwcExim.Areas.Import.Controllers
{ 
    public partial class Ppg_CWCImportController
    {
        [HttpGet]
        public ActionResult ListOfMergeDeliveryApplication(string Module, string InvoiceNo = null,string multi=null, string InvoiceDate = null, int Page = 0)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            List<PPGMergeDeliveryApplicationList> LstDelivery = new List<PPGMergeDeliveryApplicationList>();
            ObjIR.GetAllDeliveryMergeApplication(((Login)(Session["LoginUser"])).Uid,Module, InvoiceNo,multi, InvoiceDate, 0);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<PPGMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreMergeDataForDeliveryApp(string Module, string InvoiceNo = null, string multi = null, string InvoiceDate = null, int Page=0)
        {
            Ppg_ImportRepository ObjCR = new Ppg_ImportRepository();
            List<PPGMergeDeliveryApplicationList> LstJO = new List<PPGMergeDeliveryApplicationList>();
            ObjCR.GetAllDeliveryMergeApplication(((Login)(Session["LoginUser"])).Uid,Module, InvoiceNo, multi, InvoiceDate,Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<PPGMergeDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGST(int PartyID)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.GetGSTValue(PartyID);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSingleClickDeliveryPaymentSheet(string InvoiceType, int DestuffingAppId,string DeliveryDate, int InvoiceId,List<PPGDeliveryApplicationDtl> DeliveryXML,
           int PartyId, int PayeeId,string SEZ, int OTHours = 0,int OblFlag=0)
        {
            string XMLText = "";
            if (DeliveryXML != null)
            {
                XMLText = Utility.CreateXML(DeliveryXML);
            }

            Ppg_ImportRepository objChrgRepo = new Ppg_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheetSingle(InvoiceType, DestuffingAppId, DeliveryDate, InvoiceId, XMLText, PartyId,PayeeId, SEZ, OTHours, OblFlag);

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

            var Output = (PPGInvoiceGodown)objChrgRepo.DBResponse.Data;
            tentativeinvoice.InvoiceObj = (PPGInvoiceGodown)objChrgRepo.DBResponse.Data;
            Output.InvoiceDate = DateTime.Now.Date.ToString() ;
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
                Output.RoundUp = 0;
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);

            });
            tentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSingleMergeDeliveryApplication(MergeDeliveryIssueViewModel ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                string ChargesBreakupXML = "";
                if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<PPGDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<PPGDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                    DeliveryOrdXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                }
                

                  ObjIR.AddEditSingleMergeDeliveryApplication(ObjDelivery, DeliveryXml, DeliveryOrdXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetCHANAMEForSingleClick()
        {

            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();

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
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfChaForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

    }
}