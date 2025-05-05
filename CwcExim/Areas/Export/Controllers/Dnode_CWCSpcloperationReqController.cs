using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Filters;
using CwcExim.Areas.Export.Models;
using CwcExim.Controllers;
using Newtonsoft.Json;
using CwcExim.UtilityClasses;
using System.Globalization;

namespace CwcExim.Areas.Export.Controllers
{
    public class Dnode_CWCSpcloperationReqController : BaseController
    {
        // GET: Export/Dnode_CWCSpecialReport

        #region Special Operation Request
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult LoadContainerList(string CFSCode,int OperationType, int Page)
        {
           // Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
            SpeclOperationRepository ObjRR = new SpeclOperationRepository();
           // ObjRR.ListOfContainerWithCFSCode("", 0);
            ObjRR.ListOfContainerWithCFSCode(CFSCode, OperationType, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByCFSCode(string CFSCode)
        {
            // Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
            SpeclOperationRepository ObjRR = new SpeclOperationRepository();
            // ObjRR.ListOfContainerWithCFSCode("", 0);
            ObjRR.ListOfContainerWithCFSCode(CFSCode,0,0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteSpcloperationReq(int SpclOprtnReqId)
        {
            if (SpclOprtnReqId > 0)
            {
                SpeclOperationRepository ObjETR = new SpeclOperationRepository();
                ObjETR.DeleteSpcloperationReq(SpclOprtnReqId);
                return Json(ObjETR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpPost]
        public ActionResult AddEditSpecialOperationreq(SpecialOperationReq specl)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            SpeclOperationRepository sspecl = new SpeclOperationRepository();
            sspecl.AddEditSpecialOperati(specl, ObjLogin.Uid);
            //ObjEximTrader.Uid = ObjLogin.Uid;
            return Json(sspecl.DBResponse);
        }



        [HttpGet]
        public ActionResult EditSpclReq(int SpclOprtnReqId)
        {
            SpeclOperationRepository ObjRR = new SpeclOperationRepository();
            ObjRR.ListOfContainerWithCFSCode("", 0, 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainer = Jobject["lstContainer"];
                ViewBag.State = Jobject["State"];
            }
             SpeclOperationRepository ImpRep = new SpeclOperationRepository();
            //Ppg_EntryThroughGateRepository objImport = new Ppg_EntryThroughGateRepository();

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
            }
            ImpRep.ListOperaionType();
            if (ImpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstOperationType = (List<ContainerClass.OperationType>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerClass();
            if (ImpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerType();
            if (ImpRep.DBResponse.Data != null)
            {
                // var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerType = (List<ContainerClass.ContainerType>)ImpRep.DBResponse.Data;
            }
            SpeclOperationRepository ObjETR = new SpeclOperationRepository();
            SpecialOperationReq objSpclReq = new  SpecialOperationReq();
            ObjETR.GetSpeclOperation(SpclOprtnReqId);
            if (ObjETR.DBResponse.Data != null)
            {
                objSpclReq = (SpecialOperationReq)ObjETR.DBResponse.Data;
            }

            return View("EditSpclReq", objSpclReq);
        }




        [HttpGet]
        public ActionResult SpclreqList()
        {
            SpeclOperationRepository ObjETR = new SpeclOperationRepository();
            List<SpecialOperationReq> LstEximTrader = new List<SpecialOperationReq>();
            ObjETR.GetAllSpeclOperation();
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<SpecialOperationReq>)ObjETR.DBResponse.Data;
            }
            return PartialView("SpclreqList",LstEximTrader);
           // return PartialView();
         }


        [HttpGet]
        public ActionResult SpclreqListSearch(string search)
        {
            SpeclOperationRepository ObjETR = new SpeclOperationRepository();
            List<SpecialOperationReq> LstEximTrader = new List<SpecialOperationReq>();
            ObjETR.GetAllSpeclOperationSearch(search);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<SpecialOperationReq>)ObjETR.DBResponse.Data;
            }
            return PartialView("SpclreqList", LstEximTrader);
            // return PartialView();
        }

        [HttpGet]
        public ActionResult ViewOperation(int SpclOprtnReqId)
        {
            SpeclOperationRepository ObjRR = new SpeclOperationRepository();
            ObjRR.ListOfContainerWithCFSCode("", 0, 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainer = Jobject["lstContainer"];
                ViewBag.State = Jobject["State"];
            }


            SpeclOperationRepository ImpRep = new SpeclOperationRepository();
            //Ppg_EntryThroughGateRepository objImport = new Ppg_EntryThroughGateRepository();

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];

            }
            ImpRep.ListOperaionType();
            if (ImpRep.DBResponse.Data != null)
            {
                ViewBag.lstOperationType = (List<ContainerClass.OperationType>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerClass();
            if (ImpRep.DBResponse.Data != null)
            {
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerType();
            if (ImpRep.DBResponse.Data != null)
            {
                // var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerType = (List<ContainerClass.ContainerType>)ImpRep.DBResponse.Data;
            }
            SpeclOperationRepository ObjETR = new SpeclOperationRepository();
            SpecialOperationReq objSpclReq = new SpecialOperationReq();
            ObjETR.GetSpeclOperation(SpclOprtnReqId);
            if (ObjETR.DBResponse.Data != null)
            {
                objSpclReq = (SpecialOperationReq)ObjETR.DBResponse.Data;
            }

            return View("VIewSpclOperation", objSpclReq);
        }
        public ActionResult SpclOperationRequest()
        {
            SpeclOperationRepository ObjRR = new SpeclOperationRepository();
            ObjRR.ListOfContainerWithCFSCode("", 0, 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainer = Jobject["lstContainer"];
                ViewBag.State = Jobject["State"];
            }


            SpeclOperationRepository ImpRep = new SpeclOperationRepository();
            //Ppg_EntryThroughGateRepository objImport = new Ppg_EntryThroughGateRepository();

            ImpRep.ListOfShippingLinePartyCode("", 0);

            if (ImpRep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];

            }
            ImpRep.ListOperaionType();
            if (ImpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstOperationType = (List<ContainerClass.OperationType>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerClass();
            if (ImpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ImpRep.DBResponse.Data;
            }
            ImpRep.ListContainerType();
            if (ImpRep.DBResponse.Data != null)
            {
                // var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerType = (List<ContainerClass.ContainerType>)ImpRep.DBResponse.Data;
            }
            List<SpecialOperationReq> LstSpecialOper = new List<SpecialOperationReq>();
            ImpRep.GetAllSpeclOperation();
            if (ImpRep.DBResponse.Data != null)
            {
                LstSpecialOper = (List<SpecialOperationReq>)ImpRep.DBResponse.Data;
            }
            return PartialView();
        }
        #endregion

        #region Special Request Invoice

        [HttpGet]
        public ActionResult CreateSpclOperationInvoice(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            SpeclOperationRepository objExp = new SpeclOperationRepository();
            objExp.GetContStuffingForPaymentSheet();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objExp.GetPaymentParty();
            if (objExp.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            SpeclOperationRepository objImport = new SpeclOperationRepository();
            objImport.GetContDetForPaymentSheet(AppraisementId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
            int PartyId, int PayeeId, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            SpeclOperationRepository objPpgRepo = new SpeclOperationRepository();
            objPpgRepo.GetExportPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId);
            var Output = (DND_ExpPaymentSheet)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "SO";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new DND_ExpContainer(); 
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
                obj.ContainerClass = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerClass;
                obj.PayMode = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().PayMode;
                obj.SDBalance = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().SDBalance;
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
                Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DND_ExpPaymentSheet>(objForm["PaymentSheetModelJson"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

                foreach (var item in invoiceData.lstPSCont)
                {
                    // item.StuffingDate = Convert.ToDateTime(item.StuffingDate).ToString("yyyy-MM-dd");
                    item.StuffingDate = item.StuffingDate == null ? null : item.StuffingDate;
                    item.ArrivalDate = DateTime.ParseExact(item.ArrivalDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss"); 
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

                SpeclOperationRepository objChargeMaster = new SpeclOperationRepository();
                objChargeMaster.AddEditExpInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "SO");

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
        [HttpGet] 
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            SpeclOperationRepository objER = new SpeclOperationRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<DNDListOfExpInvoice> obj = new List<DNDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DNDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion




    }
}