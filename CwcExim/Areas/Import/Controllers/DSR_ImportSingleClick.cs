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
    public partial class DSR_CWCImportController
    {
        [HttpGet]
        public ActionResult ListOfMergeDeliveryApplication(string InvoiceNo = null, string InvoiceDate = null)
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSRMergeDeliveryApplicationList> LstDelivery = new List<DSRMergeDeliveryApplicationList>();
            ObjIR.GetAllDeliveryMergeApplication(0, ((Login)(Session["LoginUser"])).Uid, InvoiceNo, InvoiceDate);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DSRMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreMergeDataForDeliveryApp(int Page, string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSRMergeDeliveryApplicationList> LstJO = new List<DSRMergeDeliveryApplicationList>();
            ObjCR.GetAllDeliveryMergeApplication(Page, ((Login)(Session["LoginUser"])).Uid,InvoiceNo,InvoiceDate);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DSRMergeDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchMergeDeliveryApplication(string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSRMergeDeliveryApplicationList> LstDelivery = new List<DSRMergeDeliveryApplicationList>();
            ObjIR.GetSearchDeliveryMergeApplication(0, InvoiceNo, InvoiceDate);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DSRMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }


        [HttpGet]
        public JsonResult GetGST(int PartyID)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.GetGSTValue(PartyID);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
        //  int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
        //  string PayeeName, List<PPGPaymentSheetBOE> lstPaySheetBOE, int OTHours = 0, int InvoiceId = 0)
        //{
        //    string XMLText = "";
        //    if (lstPaySheetBOE != null)
        //    {
        //        XMLText = Utility.CreateXML(lstPaySheetBOE);
        //    }

        //    Ppg_ImportRepository objChrgRepo = new Ppg_ImportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    objChrgRepo.GetDeliveryPaymentSheet_Patparganj(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
        //            PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceId, OTHours);

        //    //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
        //    //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
        //    //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
        //    //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
        //    //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
        //    //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
        //    //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
        //    //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
        //    //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
        //    //Output.CWCTDS = 0;
        //    //Output.HTTDS = 0;
        //    //Output.TDS = 0;
        //    //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
        //    //Output.RoundUp = 0;
        //    //Output.InvoiceAmt = Output.AllTotal;
        //    //return Json(Output);

        //    var Output = (PPGInvoiceGodown)objChrgRepo.DBResponse.Data;
        //    tentativeinvoice.InvoiceObj = (PPGInvoiceGodown)objChrgRepo.DBResponse.Data;
        //    Output.InvoiceDate = InvoiceDate;
        //    Output.Module = "IMPDeli";

        //    Output.lstPrePaymentCont.ToList().ForEach(item =>
        //    {
        //        if (!Output.ShippingLineName.Contains(item.ShippingLineName))
        //            Output.ShippingLineName += item.ShippingLineName + ", ";
        //        if (!Output.CHAName.Contains(item.CHAName))
        //            Output.CHAName += item.CHAName + ", ";
        //        if (!Output.ImporterExporter.Contains(item.ImporterExporter))
        //            Output.ImporterExporter += item.ImporterExporter + ", ";
        //        if (!Output.BOENo.Contains(item.BOENo))
        //            Output.BOENo += item.BOENo + ", ";
        //        if (!Output.BOEDate.Contains(item.BOEDate))
        //            Output.BOEDate += item.BOEDate;
        //        if (!Output.CFSCode.Contains(item.CFSCode))
        //            Output.CFSCode += item.CFSCode + ", ";
        //        if (!Output.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
        //            Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
        //        if (!Output.DestuffingDate.Contains(item.DestuffingDate))
        //            Output.DestuffingDate += item.DestuffingDate + ", ";
        //        if (!Output.StuffingDate.Contains(item.StuffingDate))
        //            Output.StuffingDate += item.StuffingDate + ", ";
        //        if (!Output.CartingDate.Contains(item.CartingDate))
        //            Output.CartingDate += item.CartingDate + ", ";
        //        if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
        //        {
        //            Output.lstPostPaymentCont.Add(new PpgPostPaymentContainer
        //            {
        //                CargoType = item.CargoType,
        //                CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
        //                CFSCode = item.CFSCode,
        //                CIFValue = item.CIFValue,
        //                ContainerNo = item.ContainerNo,
        //                ArrivalDate = item.ArrivalDate,
        //                ArrivalTime = item.ArrivalTime,
        //                DeliveryType = item.DeliveryType,
        //                DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
        //                Duty = item.Duty,
        //                GrossWt = item.GrossWeight,
        //                Insured = item.Insured,
        //                NoOfPackages = item.NoOfPackages,
        //                Reefer = item.Reefer,
        //                RMS = item.RMS,
        //                Size = item.Size,
        //                SpaceOccupied = item.SpaceOccupied,
        //                SpaceOccupiedUnit = item.SpaceOccupiedUnit,
        //                StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
        //                WtPerUnit = item.WtPerPack,
        //                AppraisementPerct = item.AppraisementPerct,
        //                HeavyScrap = item.HeavyScrap,
        //                StuffCUM = item.StuffCUM
        //            });
        //        }


        //        Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
        //        Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
        //        Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
        //        Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
        //        Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
        //        Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
        //            + Output.lstPrePaymentCont.Sum(o => o.Duty);


        //        Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
        //        Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => o.CGSTAmt);
        //        Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => o.SGSTAmt);
        //        Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => o.IGSTAmt);
        //        Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.HTTotal = 0;
        //        Output.CWCTDS = 0;
        //        Output.HTTDS = 0;
        //        Output.CWCTDSPer = 0;
        //        Output.HTTDSPer = 0;
        //        Output.TDS = 0;
        //        Output.TDSCol = 0;
        //        Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
        //        Output.RoundUp = 0;
        //        Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);

        //    });
        //    tentativeinvoice.InvoiceObj = Output;
        //    return Json(Output, JsonRequestBehavior.AllowGet);




        //}

        [HttpPost]
        public JsonResult GetSingleClickDeliveryPaymentSheet(string InvoiceType, int DestuffingAppId,string DeliveryDate, int InvoiceId,List<DSRDeliveryApplicationDtl> DeliveryXML,
           int PartyId ,int PayeeId,int vehicleNo,bool IsInsured,           
           bool BillToParty, bool PrivateMovement, bool Amendment, bool CWCMovement, string HandlingType, string SEZ, int OTHours = 0,int OblFlag=0
            , int IsBond = 0 , int IsInterShifting = 0, int IsLiftOn = 0, int IsLiftOff = 0, int IsReworking = 0, int IsWeighment = 0)
        {
            string XMLText = "";
            if (DeliveryXML != null)
            {
                XMLText = Utility.CreateXML(DeliveryXML);
            }

            DSR_ImportRepository objChrgRepo = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheetSingle(InvoiceType, DestuffingAppId, DeliveryDate, InvoiceId, XMLText, PartyId, PayeeId, 
                vehicleNo, IsInsured,  BillToParty, PrivateMovement, Amendment, CWCMovement, HandlingType, SEZ, OTHours, OblFlag, IsBond, IsInterShifting, IsLiftOn, IsLiftOff, IsReworking, IsWeighment);
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
            var Output = (DSRInvoiceGodown)objChrgRepo.DBResponse.Data;
            DSRtentativeinvoice.InvoiceObj = (DSRInvoiceGodown)objChrgRepo.DBResponse.Data;
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
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);

            });
            DSRtentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSingleMergeDeliveryApplication(DSRMergeDeliveryIssueViewModel ObjDelivery)
        //DSRMergeDeliveryIssueViewModel ObjDelivery)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                string DeliveryAppDtlXml = "";
                //string DeliveryOrdDtlXml = "";
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
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DSRDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryAppDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                //if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                //{
                //    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject< List<DSRDeliveryOrdDtl >> (ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                //    DeliveryOrdDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                //}
                if (ObjDelivery.DeliApp.DeliveryGodownDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryGodownDtl = JsonConvert.DeserializeObject<List<DSRDeliveryGodownDtl>>(ObjDelivery.DeliApp.DeliveryGodownDtlXml);
                    DeliveryGodownDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryGodownDtl);
                }

                if (ObjDelivery.DeliApp.lstPrePaymentContXML != "")
                {
                    ObjDelivery.DeliApp.lstPrePaymentCont = JsonConvert.DeserializeObject<List<DSRPreInvoiceContainer>>(ObjDelivery.DeliApp.lstPrePaymentContXML);
                    lstPrePaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPrePaymentCont);
                }



                if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DSRPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                }
                //if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                //{
                //    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DSRPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                //    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                //}

                if (ObjDelivery.DeliApp.lstPostPaymentChrgXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<DSRPostPaymentChrg>>(ObjDelivery.DeliApp.lstPostPaymentChrgXML);
                    lstPostPaymentChrgXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrg);
                }
                if (ObjDelivery.DeliApp.lstContWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstContWiseAmount = JsonConvert.DeserializeObject<List<DSRContainerWiseAmount>>(ObjDelivery.DeliApp.lstContWiseAmountXML);
                    lstContWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstContWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<DSROperationCFSCodeWiseAmount>>(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML);
                    lstOperationCFSCodeWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<DSRDeliPostPaymentChargebreakupdate>>(ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML);
                    lstPostPaymentChrgBreakupXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrgBreakup);
                }
                if (ObjDelivery.DeliApp.lstInvoiceCargoXML != "")
                {
                    ObjDelivery.DeliApp.lstInvoiceCargo = JsonConvert.DeserializeObject<List<DSRInvoiceCargo>>(ObjDelivery.DeliApp.lstInvoiceCargoXML);
                    lstInvoiceCargoXML = Utility.CreateXML(ObjDelivery.DeliApp.lstInvoiceCargo);
                }

               


                ObjIR.AddEditSingleMergeDeliveryApplication(ObjDelivery,
                    DeliveryAppDtlXml, /*DeliveryOrdDtlXml,*/ lstPrePaymentContXML, lstPostPaymentContXML, 
                    lstPostPaymentChrgXML, lstContWiseAmountXML, lstOperationCFSCodeWiseAmountXML, lstPostPaymentChrgBreakupXML, lstInvoiceCargoXML, DeliveryGodownDtlXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
            //var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
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
            //    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DSRDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
            //    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
            //}


            //if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
            //{
            //    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DSRDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
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
            //  DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();

            //objChargeMaster.AddEditSingleMergeDeliveryApplication(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, 
            //    ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML, DeliveryXml, DeliveryOrdXml);

            //invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
            //invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
            //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
            //objChargeMaster.DBResponse.Data = invoiceData;
            // return Json(objChargeMaster.DBResponse);
        }

        [HttpGet]
        public JsonResult GetCHANAMEForSingleClick()
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

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
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

    }
}