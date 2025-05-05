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
    public partial class Dnd_CWCImportController
    {
        [HttpGet]
        public ActionResult ListOfMergeDeliveryApplication()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<DNDMergeDeliveryApplicationList> LstDelivery = new List<DNDMergeDeliveryApplicationList>();
            ObjIR.GetAllDeliveryMergeApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DNDMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreMergeDataForDeliveryApp(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<DNDMergeDeliveryApplicationList> LstJO = new List<DNDMergeDeliveryApplicationList>();
            ObjCR.GetAllDeliveryMergeApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DNDMergeDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            objER.ListOfDeliveryInvoiceSearch(Module, InvoiceNo, InvoiceDate);
            List<DNDMergeDeliveryApplicationList> obj = new List<DNDMergeDeliveryApplicationList>();
            if (objER.DBResponse.Data != null)
                obj = (List<DNDMergeDeliveryApplicationList>)objER.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", obj);
        }
        [HttpGet]
        public JsonResult GetGST(int PartyID)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.GetGSTValue(PartyID);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSingleClickDeliveryPaymentSheet(string InvoiceType, int DestuffingAppId, string DeliveryDate, int InvoiceId, List<DNDDeliveryApplicationDtl> DeliveryXML,
           int PartyId, int PayeeId, int OTHours = 0, int OblFlag = 0, int Movement = 0, int Distance = 0, int IsBond = 0)
        {
            string XMLText = "";
            if (DeliveryXML != null)
            {
                XMLText = Utility.CreateXML(DeliveryXML);
            }

            Dnd_ImportRepository objChrgRepo = new Dnd_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheetSingle(InvoiceType, DestuffingAppId, DeliveryDate, InvoiceId, XMLText, PartyId, PayeeId, OTHours, OblFlag, Movement, Distance, IsBond);

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

            var Output = (DNDInvoiceGodown)objChrgRepo.DBResponse.Data;
            DNDtentativeinvoice.InvoiceObj = (DNDInvoiceGodown)objChrgRepo.DBResponse.Data;
            Output.InvoiceDate = DateTime.Now.Date.ToString();
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
                    Output.lstPostPaymentCont.Add(new DNDPostPaymentContainer
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
            DNDtentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSingleMergeDeliveryApplication(DND_MergeDeliveryIssueViewModel ObjDelivery)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DNDDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryAppDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DNDDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                    DeliveryOrdDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                }
                //if (ObjDelivery.DeliApp.DeliveryGodownDtlXml != "")
                //{
                //    ObjDelivery.DeliApp.LstDeliveryGodownDtl = JsonConvert.DeserializeObject<List<DNDDeliveryGodownDtl>>(ObjDelivery.DeliApp.DeliveryGodownDtlXml);
                //    DeliveryGodownDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryGodownDtl);
                //}

                if (ObjDelivery.DeliApp.lstPrePaymentContXML != "")
                {
                    ObjDelivery.DeliApp.lstPrePaymentCont = JsonConvert.DeserializeObject<List<DNDPreInvoiceContainer>>(ObjDelivery.DeliApp.lstPrePaymentContXML);
                    lstPrePaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPrePaymentCont);
                }



                if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DNDPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                }
                //if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                //{
                //    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DNDPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                //    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                //}

                if (ObjDelivery.DeliApp.lstPostPaymentChrgXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<DNDPostPaymentChrg>>(ObjDelivery.DeliApp.lstPostPaymentChrgXML);
                    lstPostPaymentChrgXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrg);
                }
                if (ObjDelivery.DeliApp.lstContWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstContWiseAmount = JsonConvert.DeserializeObject<List<DNDContainerWiseAmount>>(ObjDelivery.DeliApp.lstContWiseAmountXML);
                    lstContWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstContWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<DNDOperationCFSCodeWiseAmount>>(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML);
                    lstOperationCFSCodeWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<DNDDeliPostPaymentChargebreakupdate>>(ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML);
                    lstPostPaymentChrgBreakupXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrgBreakup);
                }
                if (ObjDelivery.DeliApp.lstInvoiceCargoXML != "")
                {
                    ObjDelivery.DeliApp.lstInvoiceCargo = JsonConvert.DeserializeObject<List<DNDInvoiceCargo>>(ObjDelivery.DeliApp.lstInvoiceCargoXML);
                    lstInvoiceCargoXML = Utility.CreateXML(ObjDelivery.DeliApp.lstInvoiceCargo);
                }


                ObjIR.AddEditSingleMergeDeliveryApplication(ObjDelivery,
                    DeliveryAppDtlXml, DeliveryOrdDtlXml, lstPrePaymentContXML, lstPostPaymentContXML,
                    lstPostPaymentChrgXML, lstContWiseAmountXML, lstOperationCFSCodeWiseAmountXML, lstPostPaymentChrgBreakupXML, lstInvoiceCargoXML);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }


            //if (ModelState.IsValid)
            //{
            //    Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            //    string DeliveryXml = "";
            //    string DeliveryOrdXml = "";
            //    string ChargesBreakupXML = "";
            //    if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
            //    {
            //        ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DNDDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
            //        DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
            //    }


            //    if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
            //    {
            //        ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DNDDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
            //        DeliveryOrdXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
            //    }


            //    ObjIR.AddEditSingleMergeDeliveryApplication(ObjDelivery, DeliveryXml, DeliveryOrdXml);
            //    return Json(ObjIR.DBResponse);
            //}
            //else
            //{
            //    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
            //    return Json(new { Status = 1, Message = ErrorMessage });
            //}
        }

        [HttpGet]
        public JsonResult GetCHANAMEForSingleClick()
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForSingleMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Dnd_CHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAListforSingleClick(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfChaForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadCHAListsforSingleClick(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfChaForPagesforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

    }
}