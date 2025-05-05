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

namespace CwcExim.Areas.Export.Controllers
{
    public class CWCExportController : BaseController
    {
        #region Carting Application
        [HttpGet]
        public ActionResult CreateCartingApplication()
        {
            CartingApplication objApp = new CartingApplication();
            objApp.ApplicationDate = DateTime.Now.ToString("dd/MM/yyyy");
            ExportRepository objRepo = new ExportRepository();
            objRepo.ListOfCHA();
            if (objRepo.DBResponse.Data != null)
                objApp.lstCHANames = (List<CHA>)objRepo.DBResponse.Data;
            objRepo.ListOfExporter();
            if (objRepo.DBResponse.Data != null)
                objApp.lstExporter = (List<Exporter>)objRepo.DBResponse.Data;
            objRepo.GetAllCommodity();
            if (objRepo.DBResponse.Data != null)
                objApp.lstCommodity = (List<CwcExim.Areas.Export.Models.Commodity>)objRepo.DBResponse.Data;
            /*If User is External Or Non CWC User*/
            bool Exporter, CHA;
            Exporter = ((Login)Session["LoginUser"]).Exporter;
            CHA = ((Login)Session["LoginUser"]).CHA;
            if (CHA == true)
            {
                objApp.CHAName = ((Login)Session["LoginUser"]).Name;
                objApp.CHAEximTraderId = ((Login)Session["LoginUser"]).EximTraderId;
            }
            if (Exporter == true)
            {
                ViewData["IsExporter"] = true;
                ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
            }
            else
                ViewData["IsExporter"] = false;
            objRepo.ListOfPackUQCForPage("", 0);
            if (objRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            else
            {
                ViewBag.lstPackUQC = null;
            }
            //objRepo.GetSBList();
            //if (objRepo.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfSBNo = objRepo.DBResponse.Data;
            //}

            return PartialView(objApp);
        }

        public JsonResult GeSBNoForCartingApp()
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.GetSBList();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfSBNo = null;
            }
            
            return Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditCartingApplication(CartingApplication objCA)
        {
            if (ModelState.IsValid)
            {
                objCA.lstShipping = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ShippingDetails>>(objCA.StringifyData);
                string XML = Utility.CreateXML(objCA.lstShipping);
                ExportRepository objER = new ExportRepository();
                objCA.StringifyData = XML;
                objER.AddEditCartingApp(objCA, ((Login)(Session["LoginUser"])).Uid);
                ModelState.Clear();
                return Json(objER.DBResponse);
            }
            else
            {
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult ListOfCartingApp()
        {
            List<CartingList> lstCartingApp = new List<CartingList>();
            ExportRepository objER = new ExportRepository();
            objER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<CartingList>)objER.DBResponse.Data;
            return PartialView(lstCartingApp);
        }
        [HttpGet]
        public ActionResult ViewCartingApp(int CartingAppId)
        {
            ExportRepository objER = new ExportRepository();
            objER.GetCartingApp(CartingAppId);
            CartingApplication objCA = new CartingApplication();
            if (objER.DBResponse.Data != null)
                objCA = (CartingApplication)objER.DBResponse.Data;
            return PartialView(objCA);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCartingApp(int CartingAppId)
        {
            ExportRepository objER = new ExportRepository();
            if (CartingAppId > 0)
                objER.DeleteCartingApp(CartingAppId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult EditCartingApp(int CartingAppId)
        {
            CartingApplication objCA = new CartingApplication();
            ExportRepository objER = new ExportRepository();
            if (CartingAppId > 0)
            {
                objER.GetCartingApp(CartingAppId);
                if (objER.DBResponse.Data != null)
                    objCA = (CartingApplication)objER.DBResponse.Data;
                objER.ListOfCHA();
                if (objER.DBResponse.Data != null)
                    objCA.lstCHANames = (List<CHA>)objER.DBResponse.Data;
                objER.ListOfExporter();
                if (objER.DBResponse.Data != null)
                    objCA.lstExporter = (List<Exporter>)objER.DBResponse.Data;
                objER.GetAllCommodity();
                if (objER.DBResponse.Data != null)
                    objCA.lstCommodity = (List<CwcExim.Areas.Export.Models.Commodity>)objER.DBResponse.Data;
                /*If User is External Or Non CWC User*/
                bool Exporter, CHA;
                Exporter = ((Login)Session["LoginUser"]).Exporter;
                CHA = ((Login)Session["LoginUser"]).CHA;
                if (CHA == true)
                {
                    objCA.CHAName = ((Login)Session["LoginUser"]).Name;
                    objCA.CHAEximTraderId = ((Login)Session["LoginUser"]).EximTraderId;
                }
                if (Exporter == true)
                {
                    ViewData["IsExporter"] = true;
                    ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                    ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
                }
                else
                    ViewData["IsExporter"] = false;
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
                /*************************************/
            }
            return PartialView(objCA);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCartingApp(int CartingAppId)
        {
            if (CartingAppId > 0)
            {
                List<PrintCA> lstCA = new List<PrintCA>();
                ExportRepository objER = new ExportRepository();
                objER.PrintCartingApp(CartingAppId);
                if (objER.DBResponse.Data != null)
                {
                    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                    if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf"))
                        System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf");
                    lstCA = (List<PrintCA>)objER.DBResponse.Data;
                    string Html = "";
                    Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='7' style='text-align:center;font-size:12pt;padding-bottom:25px;'>Carting Application<br/><br/></th></tr><tr><th style='padding-bottom:20px;text-align:left;'>Carting No.:</th><td colspan='2' style='padding-bottom:20px;'>" + lstCA[0].CartingNo + "</td><th></th><th style='padding-bottom:20px;text-align:left;'>Carting Date:</th><td colspan='2' style='padding-bottom:20px;'>" + lstCA[0].CartingDt + "</td></tr><tr><th style='padding-bottom:20px;text-align:left;'>CHA Name:</th><td colspan='6' style='padding-bottom:20px;text-align:left;'>" + lstCA[0].CHAName + "</td></tr><tr><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Shipping Bill No.</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Shipping Bill Date</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Exporter</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Commodity</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Marks & No</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>No of Packages</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Weight</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>SCMTRPackageType</th></tr></thead><tbody>";
                    lstCA.ForEach(item =>
                    {
                        Html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ShipBillNo + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ShipBillDate + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Exporter + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Commodity + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.MarksAndNo + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.NoOfUnits + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Weight + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.SCMTRPackageType + "</td></tr>";
                    });
                    Html += "</tbody></table>";
                    using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                    {
                        rh.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf", Html);
                    }
                    return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf" });
                }
                else
                    return Json(new { Status = -1, Message = "Error" });

            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        [HttpGet]
        public JsonResult LoadPackUQCList(string PartyCode, int Page)
        {
            ExportRepository objRepo = new ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPackUQCByCode(string PartyCode)
        {
            ExportRepository objRepo = new ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Back To Town Cargo
        [HttpGet]
        public ActionResult CreateBTTCargo()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ListOfBTTCargo()
        {
            List<BTTCargoEntry> lstBTTCargoEntry = new List<BTTCargoEntry>();
            ExportRepository objExport = new ExportRepository();
            objExport.GetBTTCargoEntry();
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult AddBTTCargo()
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy");

            ExportRepository objExport = new ExportRepository();
            objExport.GetCartingAppList(0);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<CHAList>)objExport.DBResponse.Data;

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult EditBTTCargo(int BTTCargoEntryId)
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            ExportRepository objExport = new ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<CHAList>)objExport.DBResponse.Data;
            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult ViewBTTCargo(int BTTCargoEntryId)
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            ExportRepository objExport = new ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public JsonResult GetCartingDetailList(int CartingId)
        {
            try
            {
                ExportRepository objExport = new ExportRepository();
                if (CartingId > 0)
                    objExport.GetCartingDetailList(CartingId);
                return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteBTTCargo(int BTTCargoEntryId)
        {
            try
            {
                ExportRepository objExport = new ExportRepository();
                if (BTTCargoEntryId > 0)
                    objExport.DeleteBTTCargoEntry(BTTCargoEntryId);
                return Json(objExport.DBResponse);
            }
            catch (Exception)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBTTCargo(BTTCargoEntry objBTT)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objBTT.lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<BTTCargoEntryDtl>>(objBTT.BTTCargoEntryDtlJS);
                    string XML = Utility.CreateXML(objBTT.lstBTTCargoEntryDtl);
                    ExportRepository objExport = new ExportRepository();
                    objExport.AddEditBTTCargoEntry(objBTT, XML, BranchId, ((Login)(Session["LoginUser"])).Uid);
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
        #endregion

        #region Carting Work Order

        [HttpGet]
        public ActionResult CreateCartingWorkOrder()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            CartingWorkOrder ObjWorkOrder = new CartingWorkOrder();
            ObjWorkOrder.WorkOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
            ExportRepository ObjER = new ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetCartingNoForWorkOrder(BranchId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjWorkOrder.LstCarting = (List<CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ObjWorkOrder.LstGodown = (List<Godown>)ObjGR.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/CWCExport/CreateCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult GetCartingWorkOrderList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            List<CartingWorkOrder> LstWorkOrder = new List<CartingWorkOrder>();
            ExportRepository ObjER = new ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingWorkOrder(BranchId);
            if (ObjER.DBResponse.Data != null)
            {
                LstWorkOrder = (List<CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/CWCExport/CartingWorkOrderList.cshtml", LstWorkOrder);
        }

        [HttpGet]
        public ActionResult EditCartingWorkOrder(int CartingWorkOrderId)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            CartingWorkOrder ObjWorkOrder = new CartingWorkOrder();
            ExportRepository ObjER = new ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (CartingWorkOrderId > 0)
            {
                ObjER.GetCartingWorkOrder(CartingWorkOrderId, BranchId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjWorkOrder = (CartingWorkOrder)ObjER.DBResponse.Data;
                    ObjGR.GetAllGodown();
                    if (ObjGR.DBResponse.Data != null)
                    {
                        ObjWorkOrder.LstGodown = (List<Godown>)ObjGR.DBResponse.Data;
                    }
                }
            }
            return View("/Areas/Export/Views/CWCExport/EditCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewCartingWorkOrder(int CartingWorkOrderId)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            CartingWorkOrder ObjWorkOrder = new CartingWorkOrder();
            ExportRepository ObjER = new ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (CartingWorkOrderId > 0)
            {
                ObjER.GetCartingWorkOrder(CartingWorkOrderId, BranchId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjWorkOrder = (CartingWorkOrder)ObjER.DBResponse.Data;
                }
            }
            return View("/Areas/Export/Views/CWCExport/ViewCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public JsonResult GetCartingDetForWorkOrder(int CartingAppId)
        {
            if (CartingAppId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ObjER.GetCartingDetailForWorkOrder(CartingAppId, BranchId);
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteCartingWorkOrder(int CartingWorkOrderId)
        {
            if (CartingWorkOrderId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ObjER.DeleteCartingWorkOrder(CartingWorkOrderId, BranchId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        public JsonResult AddEditCartingWorkOrderDet(CartingWorkOrder ObjWorkOrder)
        {
            if (ModelState.IsValid)
            {
                ObjWorkOrder.Remarks = ObjWorkOrder.Remarks == null ? null : ObjWorkOrder.Remarks.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjWorkOrder.Uid = ObjLogin.Uid;
                ExportRepository ObjER = new ExportRepository();
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                ObjWorkOrder.BranchId = BranchId;
                ObjER.AddEditCartingWorkOrder(ObjWorkOrder);
                return Json(ObjER.DBResponse);
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
        public JsonResult PreviewCartingWorkOrder(int CartingWorkOrderId)
        {
            if (CartingWorkOrderId > 0)
            {
                string Path = "";
                ExportRepository objER = new ExportRepository();
                List<CartingWOPrint> lstWODetails = new List<CartingWOPrint>();
                objER.GetDetailsForPrint(CartingWorkOrderId);
                if (objER.DBResponse.Data != null)
                {
                    lstWODetails = (List<CartingWOPrint>)objER.DBResponse.Data;
                    Path = GeneratePdfForWO(lstWODetails, CartingWorkOrderId);
                }
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePdfForWO(List<CartingWOPrint> lstWO, int CartingWorkOrderId)
        {
            string CHAName = "", ExpName = "";
            string CompanyAddress = lstWO[0].CompanyAddress;
            string WODate = DateTime.Now.ToString("dd/MM/yyyy");
            int NoOfUnit = lstWO.Select(x => x.NoOfUnits).Sum();
            string GrossWeight = lstWO.Select(x => x.Weight).Sum().ToString() + " KG";
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
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CWorkOrder" + CartingWorkOrderId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            if (Convert.ToInt32(Session["BranchId"]) == 1)
                To = "M/S Abrar Forwarders<br/>Gandhidham Kutch";
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,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'/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + lstWO[0].WorkOrderNo +"</th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,<br/>"+To+"</th><th style='text-align:right;'>Dated: " + WODate + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ExpName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Carting "+lstWO[0].CartingNo+"</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfUnit + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: "+lstWO[0].GodownName+"</td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : </td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/CWorkOrder" + CartingWorkOrderId + ".pdf";
        }
        #endregion

        #region Carting Register
        [HttpGet]
        public ActionResult CreateCartingRegister()
        {
            ExportRepository objER = new ExportRepository();
            CartingRegister objCR = new CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd-MM-yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<ApplicationNoDet>)objER.DBResponse.Data;
            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<CartingRegister> objCR = new List<CartingRegister>();
            ExportRepository objER = new ExportRepository();
            objER.GetAllRegisterDetails();
            if (objER.DBResponse.Data != null)
                objCR = (List<CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            CartingRegister objCR = new CartingRegister();
            ExportRepository objER = new ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId);
            if (objER.DBResponse.Data != null)
                objCR = (CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            ExportRepository ObjER = new ExportRepository();
            CartingRegister ObjCartingReg = new CartingRegister();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (CartingRegister)ObjER.DBResponse.Data;
                }
            }
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            CartingRegister objCR = new CartingRegister();
            ExportRepository objER = new ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(CartingRegister objCR)
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
                ExportRepository objER = new ExportRepository();
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
            ExportRepository objER = new ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
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

            ObjER.ListOfForeignLiner();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ForeignLinerList = new SelectList((List<ForeignLiner>)ObjER.DBResponse.Data, "ForeignLinerName", "ForeignLinerName");
            else
                ViewBag.ForeignLinerList = null;


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

            ObjER.ListOfForeignLiner();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ForeignLinerList = new SelectList((List<ForeignLiner>)ObjER.DBResponse.Data, "ForeignLinerName", "ForeignLinerName");
            else
                ViewBag.ForeignLinerList = null;

            if (StuffinfgReqId > 0)
            {
                ObjER.GetStuffingRequest(StuffinfgReqId, 0, 0);
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
            return PartialView("EditStuffingReq", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            StuffingRequest ObjStuffing = new StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                ExportRepository ObjER = new ExportRepository();
                ObjER.GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingReq", ObjStuffing);
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
                objER.GetCartRegDetForStuffingReq(CartingRegisterId);
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
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy");
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
            return PartialView("/" + "/Areas/Export/Views/CWCExport/CreateContainerStuffing.cshtml", ObjCS);
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
            return PartialView("/Areas/Export/Views/CWCExport/ContainerStuffingList.cshtml", LstStuffing);
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
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "";
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
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station-Kandla Port<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingDate + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingDate + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            }
           // Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
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

            objExport.GetStuffingRequestList(WorkOrderID);
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
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,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'/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + WorkOrderNo +" </th><th style='text-align:right;'>Container Freight Station<br/>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,<br/>"+To+"</th><th style='text-align:right;'>Dated: " + WODate + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ExpName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Stuffing "+lstWO[0].StuffingReqNo+"</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfUnit + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: </td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : " + ContainerNumber + " </td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/CWorkOrder" + StuffingWorkOrderId + ".pdf";
        }
        #endregion

        #region Job Order
        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            ExportRepository objER = new ExportRepository();
            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfCHA = new SelectList((List<CHA>)objER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            objER.GetShippingLine();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = new SelectList((List<ShippingLine>)objER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            JobOrder objJO = new JobOrder();
            objJO.JobOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrder()
        {
            ExportRepository objER = new ExportRepository();
            List<JobOrderList> lstJO = new List<JobOrderList>();
            objER.GetAllJobOdrder();
            if (objER.DBResponse.Data != null)
                lstJO = (List<JobOrderList>)objER.DBResponse.Data;
            return PartialView(lstJO);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int JobOrderId)
        {
            ExportRepository objER = new ExportRepository();
            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfCHA = new SelectList((List<CHA>)objER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            objER.GetShippingLine();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = new SelectList((List<ShippingLine>)objER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            objER.GetJobOrderDetails(JobOrderId);
            JobOrder objJO = new JobOrder();
            if (objER.DBResponse.Data != null)
                objJO = (JobOrder)objER.DBResponse.Data;
            return PartialView(objJO);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int JobOrderId)
        {
            ExportRepository objER = new ExportRepository();
            objER.GetJobOrderDetails(JobOrderId);
            JobOrder objJO = new JobOrder();
            if (objER.DBResponse.Data != null)
                objJO = (JobOrder)objER.DBResponse.Data;
            return PartialView(objJO);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int JobOrderId)
        {
            ExportRepository objER = new ExportRepository();
            if (JobOrderId > 0)
                objER.DeleteJobOrder(JobOrderId);
            return Json(objER.DBResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(JobOrder objJO)
        {
            if (ModelState.IsValid)
            {
                string XMLText = "";
                ExportRepository objER = new ExportRepository();
                List<JobOrderDetails> lstJOD = new List<JobOrderDetails>();
                if (objJO.StringifiedText != null)
                {
                    lstJOD = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JobOrderDetails>>(objJO.StringifiedText);
                    XMLText = Utility.CreateXML(lstJOD);
                }
                objER.AddEditJobOrder(objJO, XMLText);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }
        #region Job Order Print
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJO(int JobOrderId)
        {
            ExportRepository objIR = new ExportRepository();
            objIR.GetExpJODetailsFrPrint(JobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                PrintJobOrderModel objMdl = new PrintJobOrderModel();
                objMdl = (PrintJobOrderModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, JobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForJO(PrintJobOrderModel objMdl, int JobOrderId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ExpJobOrder" + JobOrderId + ".pdf";
            string ContainerNo = "", Size = "", Serial = ""; int Count = 0;
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
            objMdl.lstDet.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                Size = (Size == "" ? (item.ContainerSize) : (Size + "<br/>" + item.ContainerSize));
                Serial = (Serial == "") ? (++Count).ToString() : (Serial + "<br/>" + (++Count).ToString());
            });
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            if ((Convert.ToInt32(Session["BranchId"])) == 1)
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> " + objMdl.ToLocation + " </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            }

            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ExpJobOrder" + JobOrderId + ".pdf";
        }
        #endregion
        #endregion

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
        public JsonResult GetPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetExportPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,"", StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objExport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstPSContainer.ToList().ForEach(item => {
            //    item.Fob = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).Fob;
            //    item.WtPerPackage = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).WtPerPackage;
            //});

            ////model.lstStorageRent.Aggregate

            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCCharges(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
        }




        [HttpPost]
        public JsonResult GetExpPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
        int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,decimal Weight, List<PaymentSheetContainer> lstPaySheetContainer, decimal Distance = 0, decimal MechanicalWeight = 0, decimal ManualWeight = 0, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetExportPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,Weight,
                InvoiceType, ContainerXML, Distance, MechanicalWeight, ManualWeight, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objExport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstPSContainer.ToList().ForEach(item => {
            //    item.Fob = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).Fob;
            //    item.WtPerPackage = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).WtPerPackage;
            //});

            ////model.lstStorageRent.Aggregate

            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCCharges(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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
                Decimal Weight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Weight"]) ? "0" : objForm["Weight"]);
                decimal MechanicalWeight =  Convert.ToDecimal(string.IsNullOrEmpty(objForm["MechanicalWeight"]) ? "0" : objForm["MechanicalWeight"]);
                decimal ManualWeight = Convert.ToDecimal(string.IsNullOrEmpty(objForm["ManualWeight"]) ? "0" : objForm["ManualWeight"]);
                decimal Distance = Convert.ToDecimal(string.IsNullOrEmpty(objForm["Distance"]) ? "0" : objForm["Distance"]);
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
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXP", Weight, ExportUnder, MechanicalWeight, ManualWeight, Distance);
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
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer,
            int InvoiceId=0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetBTTPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,"", StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objExport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstPSContainer.ToList().ForEach(item => {
            //    item.Fob = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).Fob;
            //    item.WtPerPackage = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).WtPerPackage;
            //});

            ////model.lstStorageRent.Aggregate

            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCCharges(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
        }




        [HttpPost]
        public JsonResult GetBTTInvPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
          int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName,decimal Weight, List<PaymentSheetContainer> lstPaySheetContainer, decimal Distance = 0, decimal MechanicalWeight = 0, decimal ManualWeight = 0,
          int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetBTTPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,Weight,
                InvoiceType, ContainerXML, Distance, MechanicalWeight, ManualWeight, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objExport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstPSContainer.ToList().ForEach(item => {
            //    item.Fob = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).Fob;
            //    item.WtPerPackage = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).WtPerPackage;
            //});

            ////model.lstStorageRent.Aggregate

            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCCharges(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
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
                Decimal Weight = Convert.ToDecimal(objForm["Weight"]);
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
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", Weight, ExportUnder);
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

        [HttpGet]
        public ActionResult CreateBTTPaymentSheetTab(string type = "Tax")
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
        #endregion

        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            ExportRepository objER = new ExportRepository();
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
         


            objER.ListOfPackUQCForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpGet]
        public ActionResult ViewLoadContainerRequest(int LoadContReqId)
        {
            ExportRepository ObjRR = new ExportRepository();
            Kdl_LoadContReq ObjContReq = new Kdl_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Kdl_LoadContReq)ObjRR.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            ExportRepository ObjRR = new ExportRepository();
            Kdl_LoadContReq ObjContReq = new Kdl_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Kdl_LoadContReq)ObjRR.DBResponse.Data;
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
                ObjRR.ListOfPackUQCForPage("", 0);
                if (ObjRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                    ViewBag.PackUQCState = Jobject["State"];
                }
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest()
        {
            ExportRepository objER = new ExportRepository();
            List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            objER.ListOfLoadCont();
            if (objER.DBResponse.Data != null)
                lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(Kdl_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                ExportRepository objER = new ExportRepository();
                string XML = "";
                //if (objReq.StringifyData != null)
                //{
                //    XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<Kdl_LoadContReqDtl>>(objReq.StringifyData));
                //}
                List<Kdl_LoadContReqDtl> LstLoadContReqDtl = JsonConvert.DeserializeObject<List<Kdl_LoadContReqDtl>>(objReq.StringifyData);

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

             //   string XML = "";
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
                ExportRepository ObjER = new ExportRepository();
                ObjER.DelLoadContReqhdr(LoadContReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianName = "")
        {
            ExportRepository ObjER = new ExportRepository();
            ObjER.ListOfFinalDestination(CustodianName);
            List<Kdl_FinalDestination> lstFinalDestination = new List<Kdl_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<Kdl_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, decimal Distance=0, decimal MechanicalWeight=0, decimal ManualWeight=0, int InvoiceId=0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetLoadedContPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType,SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML,  InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
        }



        [HttpPost]
        public JsonResult GetLoadedContPaymentSheetTab(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
           int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<PaymentSheetContainer> lstPaySheetContainer, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            objChrgRepo.GetLoadedContPaymentSheetTab(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
                InvoiceType, ContainerXML, InvoiceId);

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
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod", ExportUnder);
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


        #region Rake Entry

        public ActionResult RakeEntry()
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy");

            ExportRepository objExport = new ExportRepository();
            objExport.GetRakCartingAppList(0);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<CHAList>)objExport.DBResponse.Data;
            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult ListOfRakeMovementCargo()
        {
            List<BTTCargoEntry> lstBTTCargoEntry = new List<BTTCargoEntry>();
            ExportRepository objExport = new ExportRepository();
            objExport.GetRakeMovementCargoEntry();
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditRakeCargo(BTTCargoEntry objBTT)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objBTT.lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<BTTCargoEntryDtl>>(objBTT.BTTCargoEntryDtlJS);
                    string XML = Utility.CreateXML(objBTT.lstBTTCargoEntryDtl);
                    ExportRepository objExport = new ExportRepository();
                    objExport.AddEditRakeCargoEntry(objBTT, XML, BranchId, ((Login)(Session["LoginUser"])).Uid);
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


        [HttpGet]
        public ActionResult EditRakeCargo(int BTTCargoEntryId)
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            ExportRepository objExport = new ExportRepository();

            objExport.GetRakeCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<CHAList>)objExport.DBResponse.Data;
            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult ViewRakeCargo(int BTTCargoEntryId)
        {
            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            ExportRepository objExport = new ExportRepository();

            objExport.GetRakeCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            return PartialView(objBTTCargoEntry);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteRakeCargo(int BTTCargoEntryId)
        {
            try
            {
                ExportRepository objExport = new ExportRepository();
                if (BTTCargoEntryId > 0)
                    objExport.DeleteRakeCargoEntry(BTTCargoEntryId);
                return Json(objExport.DBResponse);
            }
            catch (Exception)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion
        #region RAK Payment Sheet
        [HttpGet]
        public ActionResult CreateRakPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ExportRepository objExport = new ExportRepository();
            objExport.GetCartingApplicationForRakPaymentSheet();
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
        public JsonResult GetRakPaymentSheetShipBillNo(int StuffingReqId)
        {
            ExportRepository objExport = new ExportRepository();
            objExport.GetShipBillForRakPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

    




        [HttpPost]
        public JsonResult GetRakInvPaymentSheet(string InvoiceDate, string InvoiceType, string SEZ, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
          int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal Weight, List<PaymentSheetContainer> lstPaySheetContainer, decimal Distance = 0, decimal MechanicalWeight = 0, decimal ManualWeight = 0,
          int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            ExportRepository objChrgRepo = new ExportRepository();
            objChrgRepo.GetRakPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, SEZ, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName, Weight,
                InvoiceType, ContainerXML, Distance, MechanicalWeight, ManualWeight, InvoiceId);

            return Json(objChrgRepo.DBResponse.Data);
            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentSheet(InvoiceDate, StuffingReqId, XMLText);
            //var model = (PaySheetChargeDetails)objExport.DBResponse.Data;

            //var CWCChargeModel = new PaymentSheetFinalModel();
            //CWCChargeModel.lstPSContainer = model.lstPSContainer;
            //CWCChargeModel.lstPSContainer.ToList().ForEach(item => {
            //    item.Fob = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).Fob;
            //    item.WtPerPackage = model.lstStorageRent.FirstOrDefault(o => o.CFSCode == item.CFSCode).WtPerPackage;
            //});

            ////model.lstStorageRent.Aggregate

            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 1,
            //    ChargeType = "CWC",
            //    ChargeName = "Ground Rent"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 2,
            //    ChargeType = "CWC",
            //    ChargeName = "Storage Charge"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 3,
            //    ChargeType = "CWC",
            //    ChargeName = "Insurance"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 4,
            //    ChargeType = "CWC",
            //    ChargeName = "Weighment"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 5,
            //    ChargeType = "CWC",
            //    ChargeName = "Entry Fees"
            //});
            //CWCChargeModel.lstChargesType.Add(new ChargesType()
            //{
            //    DBChargeID = 0,
            //    ChargeId = "C" + 6,
            //    ChargeType = "CWC",
            //    ChargeName = "Miscellaneous"
            //});
            //var _Index = 1;
            //model.lstPSHTCharge.ToList().ForEach(item =>
            //{
            //    CWCChargeModel.lstChargesType.Add(new ChargesType()
            //    {
            //        DBChargeID = item.ChargeId,
            //        ChargeId = "H" + _Index,
            //        ChargeType = "HT",
            //        ChargeName = item.ChargeName,
            //        Amount = 0,
            //        Total = 0
            //    });
            //    _Index += 1;
            //});
            //CalculateCWCCharges(CWCChargeModel, model);
            //string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
            //var Err = new { Status = -1, Messgae = "Error" };
            //CWCChargeModel.AllTotal = CWCChargeModel.lstChargesType.Sum(o => o.Total);
            //CWCChargeModel.Invoice = CWCChargeModel.lstChargesType.Sum(o => o.Total) - CWCChargeModel.RoundUp;
            //return Json(CWCChargeModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditRakPaymentSheet(FormCollection objForm)
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
                Decimal Weight = Convert.ToDecimal(objForm["Weight"]);
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

                ExportRepository objChargeMaster = new ExportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "RAK", Weight, ExportUnder);
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
    }
}