using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Controllers;
using CwcExim.Areas.Export.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using CwcExim.Models;
using CwcExim.Filters;
using System.Configuration;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using SCMTRLibrary;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CwcExim.Areas.Export.Controllers
{
    public class Kol_CWCExportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Export/Kol_CWCExport
        public ActionResult Index()
        {
            return View();
        }


        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfCHA = objER.DBResponse.Data;
            else
                ViewBag.ListOfCHA = null;
            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfExporter = objER.DBResponse.Data;
            else
                ViewBag.ListOfExporter = null;
            objER.GetShippingLine();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            else
                ViewBag.ListOfShippingLine = null;
            objER.ListOfCommodity();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfCommodity = objER.DBResponse.Data;
            else
                ViewBag.ListOfCommodity = null;
            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");

            List<CwcExim.Areas.GateOperation.Models.container> Lstcontainer = new List<CwcExim.Areas.GateOperation.Models.container>();

            objER.GetLoadedContainer();


            if (objER.DBResponse.Data != null)
            {
                Lstcontainer = (List<CwcExim.Areas.GateOperation.Models.container>)objER.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

            ExportRepository objEpR = new ExportRepository();
            objEpR.ListOfPackUQCForPage("", 0);
            if (objEpR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objEpR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            else
            {
                ViewBag.lstPackUQC = null;
            }
            return PartialView();
        }
        [HttpGet]
        public ActionResult ViewLoadContainerRequest(int LoadContReqId)
        {
            Kol_ExportRepository ObjRR = new Kol_ExportRepository();
            LoadContReq ObjContReq = new LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (LoadContReq)ObjRR.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            Kol_ExportRepository ObjRR = new Kol_ExportRepository();
            Kol_LoadContReq ObjContReq = new Kol_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Kol_LoadContReq)ObjRR.DBResponse.Data;
                ObjRR.ListOfCHA();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ListOfCHA = ObjRR.DBResponse.Data;
                else
                    ViewBag.ListOfCHA = null;
                ObjRR.ListOfExporter();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ListOfExporter = ObjRR.DBResponse.Data;
                else
                    ViewBag.ListOfExporter = null;
                ObjRR.GetShippingLine();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ListOfShippingLine = ObjRR.DBResponse.Data;
                else
                    ViewBag.ListOfShippingLine = null;
                ObjRR.ListOfCommodity();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ListOfCommodity = ObjRR.DBResponse.Data;
                else
                    ViewBag.ListOfCommodity = null;
            }
            ExportRepository objER = new ExportRepository();
            objER.ListOfPackUQCForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            else
            {
                ViewBag.lstPackUQC = null;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest()
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            List<LoadContReq> lstCont = new List<LoadContReq>();
            objER.ListOfLoadCont();
            if (objER.DBResponse.Data != null)
                lstCont = (List<LoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(Kol_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                Kol_ExportRepository objER = new Kol_ExportRepository();

             List<Kol_LoadContReqDtl> LstLoadContReqDtl =  JsonConvert.DeserializeObject<List<Kol_LoadContReqDtl>>(objReq.StringifyData);

                //Commented BY Vineet Dated 21122022 Start
                //var LstStuffingSBSorted = from c in LstLoadContReqDtl group c by c.ShippingBillNo into grp select grp.Key;



                //// LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

                //foreach (var a in LstStuffingSBSorted)
                //{
                //    int vPaketTo = 0;
                //    int vPaketFrom = 1;
                //    foreach (var i in LstLoadContReqDtl)
                //    {

                //        if (i.ShippingBillNo == a)
                //        {
                //            vPaketTo = vPaketTo + i.NoOfUnits;
                //            i.PacketsTo = vPaketTo;
                //            i.PacketsFrom = vPaketFrom;
                //            vPaketFrom = 1 + vPaketTo;
                //        }
                //    }

                //}

                //Commented BY Vineet Dated 21122022 End

                string XML = "";
                if (objReq.StringifyData != null)
                {
                    XML = Utility.CreateXML(LstLoadContReqDtl);// Utility.CreateXML(JsonConvert.DeserializeObject<List<Kol_LoadContReqDtl>>(LstLoadContReqDtl));
                }
                objER.AddEditLoadContDetails(objReq, XML);
                return Json(objER.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteLoadContReq(int LoadContReqId)
        {
            if (LoadContReqId > 0)
            {
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
                ObjER.DelLoadContReqhdr(LoadContReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        public JsonResult GetContainerForLoaded(string ContainerName)
        {
            if (ContainerName != "")
            {
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
                ObjER.GetAutoPopulateLoadedData(ContainerName);

                return Json(ObjER.DBResponse.Data, JsonRequestBehavior.AllowGet);                
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);                
            }

        }
        #endregion
        #region Container Stuffing
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            return PartialView("/" + "/Areas/Export/Views/Kol_CWCExport/CreateContainerStuffing.cshtml", ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqDtlId, string CFSCode)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqDtlId, CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<ContainerStuffing> LstStuffing = new List<ContainerStuffing>();
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetAllContainerStuffing();
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("/Areas/Export/Views/Kol_CWCExport/ContainerStuffingList.cshtml", LstStuffing);
        }
        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            ContainerStuffing ObjStuffing = new ContainerStuffing();
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            //List<ContainerStuffingDtl> LstStuffingSBSorted = new List<ContainerStuffingDtl>();
            if (ModelState.IsValid)
            {

                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    IList<ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerStuffingDtl>>(ObjStuffing.StuffingXML);

                    // Commented BY vineet Dated 211220222 start
                    //var LstStuffingSBSorted = from c in LstStuffing group c by c.ShippingBillNo into grp select grp.Key;



                    // LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

                    //foreach (var a in LstStuffingSBSorted)
                    //{
                    //    int vPaketTo = 0;
                    //    int vPaketFrom = 1;
                    //    foreach(var i in LstStuffing)
                    //    {

                    //        if(i.ShippingBillNo==a)
                    //        {
                    //            vPaketTo = vPaketTo + i.StuffQuantity;                               
                    //            i.PacketsTo = vPaketTo;
                    //            i.PacketsFrom = vPaketFrom;
                    //            vPaketFrom = 1 + vPaketTo;
                    //        }
                    //    }

                    //}

                    // Commented BY vineet Dated 211220222 End

                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }
               
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "", EquipmentSealType="", FinalDestinationLocation="";
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
                ObjStuffing.LstStuffingDtl.Select(x => new { CFSCode = x.CFSCode }).Distinct().ToList().ForEach(item =>
                {
                //    ObjStuffing.LstStuffingDtl.ToList().ForEach(item =>
                //{
                    SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : (CFSCode + "<br/>" + item.CFSCode));
                    SerialNo++;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
                {
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                    });


            ObjStuffing.LstStuffingDtl.Select(x => new { CustomSeal = x.CustomSeal }).Distinct().ToList().ForEach(item =>
            {
                CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : (CustomSeal + "<br/>" + item.CustomSeal));
            });
                ObjStuffing.LstStuffingDtl.Select(x => new { EquipmentSealType = x.EquipmentSealType }).Distinct().ToList().ForEach(item =>
                {
                    EquipmentSealType = (EquipmentSealType == "" ? (item.EquipmentSealType) : (EquipmentSealType + "<br/>" + item.EquipmentSealType));
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { CommodityName = x.CommodityName }).Distinct().ToList().ForEach(item =>
                {
                    Commodity = (Commodity == "" ? (item.CommodityName) : (Commodity + "<br/>" + item.CommodityName));
                });
              
               
                SLNo.Remove(SLNo.Length - 1);
            }

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            if (Convert.ToInt32(Session["BranchId"]) == 1)
            {
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station-Kandla Port<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingDate + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr>  <tr><td colspan='3' style='padding-top:20pt;text-align:left; font-size:7pt; width:100%;'><b>Final Destination location :</b>" + ObjStuffing.FinalDestinationLocation + "</td></tr>  <tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>EquipmentSealType</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + EquipmentSealType + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingDate + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr>  <tr><td colspan='3' style='padding-top:20pt;text-align:left; font-size:7pt; width:100%;'><b>Final Destination location :</b>" + ObjStuffing.FinalDestinationLocation + "</td></tr> <tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>EquipmentSealType</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + EquipmentSealType + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
            // Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        }
        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianCode)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.ListOfFinalDestination(CustodianCode);
            List<Kol_FinalDestination> lstFinalDestination = new List<Kol_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<Kol_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }
        
        #endregion


        #region LeoEntry
        public ActionResult LeoEntry()
        {
            return PartialView();
        }
        public JsonResult SearchMCIN(string SBNo, string SBDATE)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetMCIN(SBNo, SBDATE);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLEO(LEOPage objLEOPage)
        {


            Kol_ExportRepository objER = new Kol_ExportRepository();

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
            Kol_ExportRepository objER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();





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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();





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
            Kol_ExportRepository objER = new Kol_ExportRepository();
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

        #region Container Stuffing Amendment

        [HttpGet]
        public ActionResult AmendmentContainerStuffing()
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();

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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.ListOfStuffingNoForAmendment();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetStuffingDetailsForAmendment(int ContainerStuffingId)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            Kol_ContainerStuffing ObjStuffing = new Kol_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingDetails(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Kol_ContainerStuffing)ObjER.DBResponse.Data;
                }

            }

            return Json(ObjStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditAmendmentContainerStuffing(Kol_ContainerStuffing ObjStuffing)
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

                Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            List<Kol_ContainerStuffing> LstStuffing = new List<Kol_ContainerStuffing>();
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(0, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Kol_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView(LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadAmendmentContainerStuffingList(int Page, string SearchValue = "")
        {
            List<Kol_ContainerStuffing> LstStuffing = new List<Kol_ContainerStuffing>();
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(Page, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Kol_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
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
        public ActionResult CreateLoadedContainerPaymentSheetTab(string type = "Tax")
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
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight, List<PaymentSheetContainer> lstPaySheetContainer, decimal Distance=0, decimal MechanicalWeight=0, decimal ManualWeight=0, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetLoadedContPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName, Weight,
                InvoiceType, ContainerXML, Distance, MechanicalWeight, ManualWeight, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
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

                var invoiceData = JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                decimal MechanicalWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["MechanicalWeight"]) ? "0" : objForm["MechanicalWeight"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["ManualWeight"]) ? "0" : objForm["ManualWeight"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Distance"]) ? "0" : objForm["Distance"]);
                string ExportUnder = Convert.ToString(objForm["SEZValue"]);

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
                objChargeMaster.AddEditLoadedInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod", ExportUnder, MechanicalWeight, ManualWeight, Distance);
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
        #region Carting Register
        [HttpGet]
        public ActionResult CreateCartingRegister()
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            Kol_CartingRegister objCR = new Kol_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd-MM-yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<ApplicationNoDet>)objER.DBResponse.Data;
            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<Kol_CartingRegister> objCR = new List<Kol_CartingRegister>();
            Kol_ExportRepository objER = new Kol_ExportRepository();
            objER.GetAllRegisterDetails();
            if (objER.DBResponse.Data != null)
                objCR = (List<Kol_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            Kol_CartingRegister objCR = new Kol_CartingRegister();
            Kol_ExportRepository objER = new Kol_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId);
            if (objER.DBResponse.Data != null)
                objCR = (Kol_CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            Kol_CartingRegister ObjCartingReg = new Kol_CartingRegister();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Kol_CartingRegister)ObjER.DBResponse.Data;
                }
            }
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            Kol_CartingRegister objCR = new Kol_CartingRegister();
            Kol_ExportRepository objER = new Kol_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (Kol_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(Kol_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                //List<int> lstLocation = new List<int>();
                IList<CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CartingRegisterDtl>>(objCR.XMLData);

                //foreach (var item in LstCartingDtl)
                //{
                //    if (item.LocationDetails != "" && item.LocationDetails != null)
                //    {
                //        string[] data = item.LocationDetails.Split(',');
                //        foreach (string lctn in data)
                //            lstLocation.Add(Convert.ToInt32(lctn));
                //    }
                //}

                //string ClearLcoationXML = null;
                //if (objCR.ClearLocation != "" && objCR.ClearLocation != null)
                //{
                //    string[] data = objCR.ClearLocation.Split(',');
                //    List<int> lstClearLocation = new List<int>();
                //    foreach (var elem in data)
                //        lstClearLocation.Add(Convert.ToInt32(elem));
                //    ClearLcoationXML = Utility.CreateXML(lstClearLocation);
                //}
                // string LocationXML = Utility.CreateXML(lstLocation);
                string XML = Utility.CreateXML(LstCartingDtl);
                Kol_ExportRepository objER = new Kol_ExportRepository();
                objCR.Uid = ((Login)Session["LoginUser"]).Uid;
                objER.AddEditCartingRegister(objCR, XML /*, LocationXML, ClearLcoationXML*/);
                return Json(objER.DBResponse);
            }
            else
            {
                var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrMsg };
                return Json(Err);
            }
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCartingRegister(int CartingRegisterId)
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            Kol_ExportRepository objExp = new Kol_ExportRepository();
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
            Kol_ExportRepository objExport = new Kol_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            Kol_ExportRepository objExport = new Kol_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            Kol_ExportRepository objExport = new Kol_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            Kol_ExportRepository objExport = new Kol_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Kol_ExportRepository objCR = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
                Kol_ExportRepository ObjER = new Kol_ExportRepository();

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
                            Kol_ExportRepository objExport = new Kol_ExportRepository();
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
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
                
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
                        //    Kol_ExportRepository objExport = new Kol_ExportRepository();
                        //    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");

                    }
                    Kol_ExportRepository objExport = new Kol_ExportRepository();
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



        #region  Loaded Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateLoadContainerStuffingApproval()
        {
            Kol_ExportRepository objExp = new Kol_ExportRepository();
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
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
                Kol_ExportRepository objCR = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetLoadContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion

        #region Loaded Container Send ASR
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult LoadedContainerSendASR(int ContainerStuffingId)
        {
            int k = 0;
            int j = 1;
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            //PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetLoadedContainerCIMASRDetails(ContainerStuffingId, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;
                string Filenm = Convert.ToString(ds.Tables[6].Rows[0]["FileName"]);
                string JsonFile = StuffingCIMACRJsonFormat.StuffingCIMACRJson(ds);

                string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMASR"];
                string FileName = strFolderName + Filenm;
                if (!Directory.Exists(strFolderName))
                {
                    Directory.CreateDirectory(strFolderName);
                }

                System.IO.File.Create(FileName).Dispose();

                System.IO.File.WriteAllText(FileName, JsonFile);
                string output = "";
                //For Block If Develpoment Done Then Unlock
                using (FileStream fs = System.IO.File.OpenRead(FileName))
                {
                    log.Info("FTP File read process has began");
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    output = FtpFileManager.UploadFileToFtp("/test/SCMTR/Inbound", Filenm, buffer, "5000", FileName);
                    log.Info("FTP File read process has ended");
                }
                if (output == "Success")
                {
                    Kol_ExportRepository objExport = new Kol_ExportRepository();
                    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);
                    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                }
                log.Info("FTP File upload has been end");
                return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        #endregion
        #region ACTUAL ARRIVAL DATE AND TIME 
        [HttpGet]
        public ActionResult ActualArrivalDateTime()
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            Kol_ActualArrivalDatetime objActualArrival = new Kol_ActualArrivalDatetime();

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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchContainer(string ContainerBoxSearch, int Page)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditActualArrivalDatetime(Kol_ActualArrivalDatetime objActualArrivalDatetime)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            List<Kol_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Kol_ActualArrivalDatetime>();
            Kol_ExportRepository objER = new Kol_ExportRepository();
            //objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, 0);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Kol_ActualArrivalDatetime>)objER.DBResponse.Data;
            return PartialView(lstActualArrivalDatetime);
        }

        [HttpGet]
        public JsonResult EditActualArrivalDatetime(int actualArrivalDatetimeId)
        {
            List<Kol_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Kol_ActualArrivalDatetime>();
            Kol_ExportRepository objER = new Kol_ExportRepository();
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, actualArrivalDatetimeId);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Kol_ActualArrivalDatetime>)objER.DBResponse.Data;
            //return Json(lstActualArrivalDatetime);
            return Json(lstActualArrivalDatetime, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SendAT(string CFSCode)
        {


            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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

        #region  Loaded Container SF Send

        [HttpGet]
        public ActionResult CreateLoadContainerSF()
        {
            Kol_ExportRepository objExp = new Kol_ExportRepository();
            Kol_LoadContSF ObjPC = new Kol_LoadContSF();
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
            Kol_LoadContSF ObjStuffing = new Kol_LoadContSF();

            if (LoadContReqId > 0)
            {
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
                ObjER.GetLoadContainerStuffingSFById(LoadContReqId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Kol_LoadContSF)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingSF(Kol_LoadContSF objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Kol_ExportRepository objCR = new Kol_ExportRepository();
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
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            List<Kol_LoadContSF> LstStuffingApproval = new List<Kol_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Kol_LoadContSF>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingSF", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingSFist(int Page, string SearchValue = "")
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            var LstStuffingApproval = new List<Kol_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Kol_LoadContSF>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingSF(int ApprovalId)
        {
            Kol_LoadContSF ObjStuffing = new Kol_LoadContSF();
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetLoadContainerStuffingSFById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Kol_LoadContSF)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
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
                Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
                        Kol_ExportRepository objExport = new Kol_ExportRepository();
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
        #region Load Container Request Update
        public ActionResult LoadedContainerRequestUpdate(int LoadedRequestId = 0)
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            Kol_LoadContReq ObjContReq = new Kol_LoadContReq();
            if (LoadedRequestId > 0)
            {

                objER.GetLoadContDetailsForUpdate(LoadedRequestId);
                if (objER.DBResponse.Data != null)
                {
                    ObjContReq = (Kol_LoadContReq)objER.DBResponse.Data;
                }
            }

            /*   objER.ListOfCHA();
               if (objER.DBResponse.Data != null)
                   ViewBag.ListOfCHA = objER.DBResponse.Data;
               else
                   ViewBag.ListOfCHA = null;
               objER.ListOfExporter();
               if (objER.DBResponse.Data != null)
                   ViewBag.ListOfExporter = objER.DBResponse.Data;
               else
                   ViewBag.ListOfExporter = null;
               objER.GetShippingLine();
               if (objER.DBResponse.Data != null)
                   ViewBag.ListOfShippingLine = objER.DBResponse.Data;
               else
                   ViewBag.ListOfShippingLine = null;
               objER.GetAllCommodityForPage("", 0);
               if (objER.DBResponse.Data != null)
               {
                   var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                   var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                   ViewBag.LstCommodity = Jobject["LstCommodity"];
                   ViewBag.CommodityState = Jobject["State"];
               }*/
               ExportRepository objEpR = new ExportRepository();
               objEpR.ListOfPackUQCForPage("", 0);
               if (objEpR.DBResponse.Data != null)
               {
                   var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objEpR.DBResponse.Data);
                   var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                   ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                   ViewBag.PackUQCState = Jobject["State"];
               }
               else
               {
                   ViewBag.lstPackUQC = null;
               }
               


            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(ObjContReq);
        }
         
        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateLoadContReq(Kol_LoadContReq objReq)
        {
           
            if (ModelState.IsValid)
            {
                Kol_ExportRepository objER = new Kol_ExportRepository();

                List<Kol_LoadContReqDtl> LstLoadContReqDtl = JsonConvert.DeserializeObject<List<Kol_LoadContReqDtl>>(objReq.StringifyData);
                var LstStuffingSBSorted = from c in LstLoadContReqDtl group c by c.ShippingBillNo into grp select grp.Key;



                // LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

                foreach (var a in LstStuffingSBSorted)
                {
                    int vPaketTo = 0;
                    int vPaketFrom = 1;
                    foreach (var i in LstLoadContReqDtl)
                    {

                        if (i.ShippingBillNo == a)
                        {
                            vPaketTo = vPaketTo + i.NoOfUnits;
                            i.PacketsTo = vPaketTo;
                            i.PacketsFrom = vPaketFrom;
                            vPaketFrom = 1 + vPaketTo;
                        }
                    }

                }
                /*  string XML = "";
                  if (objReq.StringifyData != null)
                  {
                      XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<Kol_LoadContReqDtl>>(objReq.StringifyData));
                  }
                  objER.UpdateLoadContDetails(objReq, XML);
                  return Json(objER.DBResponse);*/
                string XML = "";
                if (objReq.StringifyData != null)
                {
                    XML = Utility.CreateXML(LstLoadContReqDtl);// Utility.CreateXML(JsonConvert.DeserializeObject<List<Kol_LoadContReqDtl>>(LstLoadContReqDtl));
                }
                objER.UpdateLoadContDetails(objReq, XML);
                return Json(objER.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }


        [HttpGet]
        public ActionResult ListLoadContainerRequestForUpdate(int Page = 0, string ShippbillNo = "")
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            List<LoadContReq> lstCont = new List<LoadContReq>();
            objER.LoadContDetailsForUpdate(Page, ShippbillNo);
            if (objER.DBResponse.Data != null)
                lstCont = (List<LoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        public ActionResult ListLoadMoreLoadContainerRequest(int Page, string ShippbillNo)
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            List<LoadContReq> lstCont = new List<LoadContReq>();
            objER.LoadContDetailsForUpdate(Page, ShippbillNo);
            if (objER.DBResponse.Data != null)
                lstCont = (List<LoadContReq>)objER.DBResponse.Data;
            return Json(lstCont, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchListLoadContainerRequestforUpdate(string ShippbillNo, int Page = 0)
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            List<LoadContReq> lstCont = new List<LoadContReq>();
            objER.SearchListLoadContainerRequest(Page, ShippbillNo);
            if (objER.DBResponse.Data != null)
                lstCont = (List<LoadContReq>)objER.DBResponse.Data;
            return PartialView("ListLoadContainerRequestForUpdate", lstCont);
        }

        [HttpGet]
        public JsonResult GetContainerListLoadContReqUpdate(string CONTCBT)
        {
            Kol_ExportRepository objCR = new Kol_ExportRepository();
            objCR.GetContainerListLoadContReqUpdate(CONTCBT);
            return Json(objCR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCHA(string CHAName)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.ListOfCHA(CHAName);
            IList<CHA> lstCHA = new List<CHA>();
            if (ObjER.DBResponse.Data != null)
            {
                lstCHA = (List<CHA>)ObjER.DBResponse.Data;
            }

            return Json(lstCHA, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetExporter(string ExporterName)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.ListOfExporter(ExporterName);
            IList<Exporter> lstExporter = new List<Exporter>();
            if (ObjER.DBResponse.Data != null)
            {
                lstExporter = (List<Exporter>)ObjER.DBResponse.Data;
            }

            return Json(lstExporter, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetShippingLine(string ShippingName)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetShippingLine(ShippingName);
            IList<ShippingLine> lstExporter = new List<ShippingLine>();
            if (ObjER.DBResponse.Data != null)
            {
                lstExporter = (List<ShippingLine>)ObjER.DBResponse.Data;
            }

            return Json(lstExporter, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLoadedContainerRequestNoForUpdate(int LoadedRequestId = 0)
        {
            Kol_ExportRepository objER = new Kol_ExportRepository();
            Kol_LoadContReq ObjContReq = new Kol_LoadContReq();
           
            objER.GetLoadedContainerRequestNoForUpdate();
            IList<LoadContReq> ListOfRequestNo = new List<LoadContReq>();
            if (objER.DBResponse.Data != null)
            {
                ListOfRequestNo = (List<LoadContReq>)objER.DBResponse.Data;
            }
           

            return Json(ListOfRequestNo, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCommodity(string ComName)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ObjER.GetCommodity(ComName);
            IList<Areas.Export.Models.Commodity> lstCHA = new List<Areas.Export.Models.Commodity>();
            if (ObjER.DBResponse.Data != null)
            {
                lstCHA = (List<Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;
            }

            return Json(lstCHA, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Container Stuffing Upadate
        [HttpGet]
        public ActionResult CreateContStuffSealUpdate(int ContainerStuffingId = 0)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
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
            return PartialView("CreateContStuffSealUpdate", ObjStuffing);
        }
             [HttpGet]
        public JsonResult GetStuffingNoForUpdate(int ContainerStuffingId = 0)
        {
            Kol_ExportRepository ObjER = new Kol_ExportRepository();
            ContainerStuffing ObjStuffing = new ContainerStuffing();

            ObjER.GetContstuffNoForSealUpdate();
            
            IList<ContainerStuffing> ListOfStuffingNo = new List<ContainerStuffing>();
            if (ObjER.DBResponse.Data != null)
            {
                ListOfStuffingNo = (List<ContainerStuffing>)ObjER.DBResponse.Data;
            }


            return Json(ListOfStuffingNo, JsonRequestBehavior.AllowGet);
        }


        

           [HttpPost]
           [ValidateAntiForgeryToken]
           public JsonResult AddEditContainerStuffingDetUpdate(ContainerStuffing ObjStuffing)
           {
               ModelState.Remove("HandalingPartyCode");
               ModelState.Remove("STOPartyCode");
               ModelState.Remove("INSPartyCode");
               ModelState.Remove("GREPartyCode");
               //List<ContainerStuffingDtl> LstStuffingSBSorted = new List<ContainerStuffingDtl>();
               if (ModelState.IsValid)
               {

                   string ContainerStuffingXML = "";
                   if (ObjStuffing.StuffingXML != null)
                   {
                       IList<ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerStuffingDtl>>(ObjStuffing.StuffingXML);

                       // Commented BY vineet Dated 211220222 start
                       //var LstStuffingSBSorted = from c in LstStuffing group c by c.ShippingBillNo into grp select grp.Key;



                       // LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

                       //foreach (var a in LstStuffingSBSorted)
                       //{
                       //    int vPaketTo = 0;
                       //    int vPaketFrom = 1;
                       //    foreach(var i in LstStuffing)
                       //    {

                       //        if(i.ShippingBillNo==a)
                       //        {
                       //            vPaketTo = vPaketTo + i.StuffQuantity;                               
                       //            i.PacketsTo = vPaketTo;
                       //            i.PacketsFrom = vPaketFrom;
                       //            vPaketFrom = 1 + vPaketTo;
                       //        }
                       //    }

                       //}

                       // Commented BY vineet Dated 211220222 End

                       ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                   }

                   Kol_ExportRepository ObjER = new Kol_ExportRepository();
                   ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                   ObjER.AddEditContainerStuffingUpdate(ObjStuffing, ContainerStuffingXML);
                   return Json(ObjER.DBResponse);
               }
               else
               {
                   var ErrMsg = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                   var Err = new { Status = 0, Message = ErrMsg };
                   return Json(Err);
               }
           }



        #endregion

        #region Update ContainerPkgUpdt
        [HttpGet]
        public ActionResult LoadedContainerPkgUpdt()
        {
            //ViewData["InvType"] = type;
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

        public JsonResult LoadContainerSbLists(string StuffingReqId, int Page)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetLoaderSBNO(StuffingReqId, Page);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public ActionResult GetLoadedSBNoBySBNo(int SBNo)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetLoadeSbNo(SBNo);
            return Json(objRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLoadedAllData(int SBNo, int LoaderNo)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetLoaderContData(SBNo, LoaderNo);
            return Json(objRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateLoadedRqtData(Kol_LoadShipBillNo objload)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.UpdateLoadedRqtdata(objload);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
        #region UpdateCUM
        [HttpGet]
        public ActionResult UpdateCUM()
        {


            return PartialView();
        }

        [HttpGet]


        public JsonResult GetCarRegister()
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetCrtRegisterNo(0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCarRegisterCUM(int CartingAppDtlId)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetCrtRegisterNoCUM(CartingAppDtlId);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCUMData(Kol_CUMUpdateModel model)
        {
            Kol_ExportRepository objRepo = new Kol_ExportRepository();
            objRepo.GetCrtRegisterNoCUM(model);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion 


    }
}