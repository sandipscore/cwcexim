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
    public partial class VLDA_CWCImportController
    {
        [HttpGet]
        public ActionResult ListOfMergeDeliveryApplication()
        {

            Wfld_ImportRepository ObjIR = new Wfld_ImportRepository();
            List<WFLDMergeDeliveryApplicationList> LstDelivery = new List<WFLDMergeDeliveryApplicationList>();
            ObjIR.GetAllDeliveryMergeApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<WFLDMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryMergeApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreMergeDataForDeliveryApp(int Page)
        {
            Wfld_ImportRepository ObjCR = new Wfld_ImportRepository();
            List<WFLDMergeDeliveryApplicationList> LstJO = new List<WFLDMergeDeliveryApplicationList>();
            ObjCR.GetAllDeliveryMergeApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLDMergeDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGST(int PartyID)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.GetGSTValue(PartyID);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        // [HttpPost]
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
        public JsonResult GetSingleClickDeliveryPaymentSheet(string InvoiceType, int DestuffingAppId, string DeliveryDate, string InvoiceDate, int InvoiceId, List<WFLDDeliveryApplicationDtl> DeliveryXML,
           int PartyId, int PayeeId, int vehicleNo, int vehicleNoUn, bool IsInsured, bool BillToParty, bool Transporter, bool ImportToBond, int Cargo,bool OpenDestuffing,bool DestuffingContainer, List<WFLDDeliveryGodownDtl> GodownXML, int OTHours = 0, int OblFlag = 0, string SEZ = "")
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

            VLDA_ImportRepository objChrgRepo = new VLDA_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheetSingle(InvoiceType, DestuffingAppId, DeliveryDate, InvoiceDate, InvoiceId, XMLText, PartyId, PayeeId, vehicleNo, vehicleNoUn, IsInsured, BillToParty, Transporter, ImportToBond, Cargo,OpenDestuffing,DestuffingContainer, XMLGodown, OTHours, OblFlag, SEZ);
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
            var Output = (VLDAInvoiceGodown)objChrgRepo.DBResponse.Data;
            VLDAtentativeinvoice.InvoiceObj = (VLDAInvoiceGodown)objChrgRepo.DBResponse.Data;
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
                    Output.lstPostPaymentCont.Add(new VLDAPostPaymentContainer
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
            VLDAtentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSingleMergeDeliveryApplication(VLDAMergeDeliveryIssueViewModel ObjDelivery)
        // WFLDMergeDeliveryIssueViewModel ObjDelivery)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (ModelState.IsValid)
            {
                VLDA_ImportRepository ObjIR = new VLDA_ImportRepository();
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
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<VLDADeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryAppDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                //if (ObjDelivery.DeliApp.DeliveryOrdDtlXml !=null)
                //{
                //    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<WFLDDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                //    DeliveryOrdDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                //}
                if (ObjDelivery.DeliApp.DeliveryGodownDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryGodownDtl = JsonConvert.DeserializeObject<List<VLDADeliveryGodownDtl>>(ObjDelivery.DeliApp.DeliveryGodownDtlXml);
                    DeliveryGodownDtlXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryGodownDtl);
                }

                if (ObjDelivery.DeliApp.lstPrePaymentContXML != "" && ObjDelivery.DeliApp.lstPrePaymentContXML != null)
                {
                    ObjDelivery.DeliApp.lstPrePaymentCont = JsonConvert.DeserializeObject<List<VLDAPreInvoiceContainer>>(ObjDelivery.DeliApp.lstPrePaymentContXML);
                    lstPrePaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPrePaymentCont);
                }



                if (ObjDelivery.DeliApp.lstPostPaymentContXML != "" && ObjDelivery.DeliApp.lstPostPaymentContXML != null)
                {
                    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<VLDAPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                }
                //if (ObjDelivery.DeliApp.lstPostPaymentContXML != "")
                //{
                //    ObjDelivery.DeliApp.lstPostPaymentCont = JsonConvert.DeserializeObject<List<WFLDPostPaymentContainer>>(ObjDelivery.DeliApp.lstPostPaymentContXML);
                //    lstPostPaymentContXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentCont);
                //}

                if (ObjDelivery.DeliApp.lstPostPaymentChrgXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<VLDAPostPaymentChrg>>(ObjDelivery.DeliApp.lstPostPaymentChrgXML);
                    lstPostPaymentChrgXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrg);
                }
                if (ObjDelivery.DeliApp.lstContWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstContWiseAmount = JsonConvert.DeserializeObject<List<VLDAContainerWiseAmount>>(ObjDelivery.DeliApp.lstContWiseAmountXML);
                    lstContWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstContWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML != "")
                {
                    ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<VLDAOperationCFSCodeWiseAmount>>(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmountXML);
                    lstOperationCFSCodeWiseAmountXML = Utility.CreateXML(ObjDelivery.DeliApp.lstOperationCFSCodeWiseAmount);
                }
                if (ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML != "")
                {
                    ObjDelivery.DeliApp.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<VLDADeliPostPaymentChargebreakupdate>>(ObjDelivery.DeliApp.lstPostPaymentChrgBreakupXML);
                    lstPostPaymentChrgBreakupXML = Utility.CreateXML(ObjDelivery.DeliApp.lstPostPaymentChrgBreakup);
                }
                if (ObjDelivery.DeliApp.lstInvoiceCargoXML != "")
                {
                    ObjDelivery.DeliApp.lstInvoiceCargo = JsonConvert.DeserializeObject<List<VLDAInvoiceCargo>>(ObjDelivery.DeliApp.lstInvoiceCargoXML);
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
           
        }

        [HttpGet]
        public JsonResult GetCHANAMEForSingleClick()
        {

            Wfld_ImportRepository ObjIR = new Wfld_ImportRepository();

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
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfChaForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPayeeListforSingleClick(string PartyCode, int Page)
        {
            Wfld_ImportRepository objRepo = new Wfld_ImportRepository();
            objRepo.ListOfPayeeForPageforSingleClick(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
    }
}