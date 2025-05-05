using CwcExim.Areas.Export.Models;  
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
namespace CwcExim.Areas.Export.Controllers
{
    public class Dnd_CWCExportController : BaseController
    {
      


        #region CCINEntry
        public ActionResult CCINEntry(int Id = 0, int PartyId = 0)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetCCINShippingLineForPage("",0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstShippingLine = Jobject["LstShippingLine"];
                ViewBag.SLAState = Jobject["State"];
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            ObjER.GetVehicleForCCIN();
            if(ObjER.DBResponse.Data!=null)
            {
                ViewBag.ListVehicle = ObjER.DBResponse.Data;
            }

            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }


            ObjER.GetSBList();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = ObjER.DBResponse.Data;
            }
            /*
            ObjER.GetCCINPartyList();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.lstParty = ObjER.DBResponse.Data;
            }*/
            List<Godown> lstGodown = new List<Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
               ViewBag.lstGodown = ObjER.DBResponse.Data;
            }
           // ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ObjER.GetCCINEntryForEdit(Id);
                if (ObjER.DBResponse.Data != null)
                {
                    objCCINEntry = (Dnd_CCINEntry)ObjER.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetPortByCountry(int CountryId)
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetPortOfLoadingForCCIN(CountryId);
            if (ObjRR.DBResponse.Status > 0)
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult LoadCCINShippingLine(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCCINShippingLineByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGateEntryDetForCCIN(int EntryId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCCINByGateEntryId(EntryId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCCINCharges(CCINEntryId, PartyId, Weight, FOB, CargoType);
            objCCINEntry = (Dnd_CCINEntry)objExport.DBResponse.Data;
            ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntry(Dnd_CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
           // ModelState.Remove("ExporterId");
           // ModelState.Remove("ExporterName");
            ModelState.Remove("PartyId");
            ModelState.Remove("PartyName");
            if (ModelState.IsValid)
            {
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
                // IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                // string XML = Utility.CreateXML(PostPaymentChargeList);
                // objCCINEntry.PaymentSheetModelJson = XML;
                objER.AddEditCCINEntry(objCCINEntry);
                return Json(objER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };

                var errors =
                from item in ModelState
                where item.Value.Errors.Count > 0
                select item.Key;
                var keys = errors.ToArray();

                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult ViewCCINEntry(int Id)
        {
            Dnd_CCINEntry objCCINEntry  = new Dnd_CCINEntry();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetCCINEntry(Id);
            if (objER.DBResponse.Data != null)
                objCCINEntry = (Dnd_CCINEntry)objER.DBResponse.Data;
            return PartialView("ViewCCINEntry", objCCINEntry);
        }


        [HttpGet]
        public ActionResult ListOfCCINEntry()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        #region CCINEntry Search
        [HttpGet]
        public ActionResult ListOfCCINEntrySearch(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryForSearch(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView("ListOfCCINEntry", lstCCINEntry);
        }




        #endregion

        [HttpGet]
        public JsonResult LoadMoreCCINEntryList(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> LstJO = new List<Dnd_CCINEntry>();
            ObjCR.GetAllCCINEntryForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntry(int CCINEntryId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region CCINEntry Approval
        public ActionResult CCINEntryApproval(int Id = 0)
        {
            ExportRepository objER = new ExportRepository();
            objER.GetShippingLine();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            }
            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = objER.DBResponse.Data;
            }
            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfCHA = objER.DBResponse.Data;
            }
            objER.GetAllCommodity();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfCommodity = objER.DBResponse.Data;
            }

            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfPort = ObjRR.DBResponse.Data;
            }
            ObjRR.GetSBList();
            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = ObjRR.DBResponse.Data;
            }
            ObjRR.GetCCINPartyList();
            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.lstParty = ObjRR.DBResponse.Data;
            }

            Dnd_ExportRepository objRepository = new Dnd_ExportRepository();
            objRepository.GetAllCCINEntry();
            if (objRepository.DBResponse.Data != null)
            {
                ViewBag.ListOfCCINNo = (List<Dnd_CCINEntry>)objRepository.DBResponse.Data;
            }

            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                Dnd_ExportRepository rep = new Dnd_ExportRepository();
                rep.GetCCINEntryById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (Dnd_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetCCINEntryApprovalDetails(int CCINEntryId)
        {
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();
            if (CCINEntryId > 0)
            {
                Dnd_ExportRepository rep = new Dnd_ExportRepository();
                rep.GetCCINEntryById(CCINEntryId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (Dnd_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
                else
                {
                    objCCINEntry = null;
                }
            }
            return Json(objCCINEntry, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddEditCCINEntryApproval(int Id, bool IsApproved)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.AddEditCCINEntryApproval(Id, IsApproved);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntryApproval()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryApprovalForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryApprovalList(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> LstJO = new List<Dnd_CCINEntry>();
            ObjCR.GetAllCCINEntryApprovalForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Carting Application
        [HttpGet]
        public ActionResult CreateCartingApplication()
        {
            //User RightsList---------------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //------------------------------------------------------------------------------------------------------

            CartingApplication objApp = new CartingApplication();
            objApp.ApplicationDate = DateTime.Now.ToString("dd/MM/yyyy");
            CHN_ExportRepository objRepo = new CHN_ExportRepository();
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
            return PartialView(objApp);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditCartingApplication(CartingApplication objCA)
        {
            if (ModelState.IsValid)
            {
                objCA.lstShipping = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ShippingDetails>>(objCA.StringifyData);
                string XML = Utility.CreateXML(objCA.lstShipping);
                CHN_ExportRepository objER = new CHN_ExportRepository();
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
            CHN_ExportRepository objER = new CHN_ExportRepository();
            objER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<CartingList>)objER.DBResponse.Data;
            return PartialView(lstCartingApp);
        }
        [HttpGet]
        public ActionResult ViewCartingApp(int CartingAppId)
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
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
            CHN_ExportRepository objER = new CHN_ExportRepository();
            if (CartingAppId > 0)
                objER.DeleteCartingApp(CartingAppId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult EditCartingApp(int CartingAppId)
        {
            CartingApplication objCA = new CartingApplication();
            CHN_ExportRepository objER = new CHN_ExportRepository();
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
                CHN_ExportRepository objER = new CHN_ExportRepository();
                objER.PrintCartingApp(CartingAppId);
                if (objER.DBResponse.Data != null)
                {
                    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                    if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf"))
                        System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApp" + CartingAppId + ".pdf");
                    lstCA = (List<PrintCA>)objER.DBResponse.Data;
                    string Html = "";
                    Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'>CARTING APPLICATION</label></td></tr></tbody></table></td></tr>  <tr><th><br/><br/></th></tr><tr><th style='padding-bottom:20px;text-align:left;'>Carting No.:</th><td colspan='2' style='padding-bottom:20px;'>" + lstCA[0].CartingNo + "</td><th></th><th style='padding-bottom:20px;text-align:left;'>Carting Date:</th><td colspan='2' style='padding-bottom:20px;'>" + lstCA[0].CartingDt + "</td></tr><tr><th style='padding-bottom:20px;text-align:left;'>CHA Name:</th><td colspan='6' style='padding-bottom:20px;text-align:left;'>" + lstCA[0].CHAName + "</td></tr><tr><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Shipping Bill No.</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Shipping Bill Date</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Exporter</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Commodity</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Marks & No</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>No of Packages</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Weight</th></tr></thead><tbody>";
                    lstCA.ForEach(item =>
                    {
                        Html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ShipBillNo + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ShipBillDate + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Exporter + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Commodity + "</td><td style='text-align:left;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.MarksAndNo + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.NoOfUnits + "</td><td style='text-align:right;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Weight + "</td></tr>";
                    });
                    Html += "</tbody></table>";
                    Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                    using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
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
        #endregion

        #region Carting Register
        [HttpGet]
        public ActionResult CreateCartingRegister()
        {
            //User RightsList---------------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //------------------------------------------------------------------------------------------------------


            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Dnd_CartingRegister objCR = new Dnd_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd-MM-yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<ApplicationNoDet>)objER.DBResponse.Data;



            objER.ListOfChaForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            objER.ListOfExporterForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            objER.ListOfShippingLinePartyCode("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
               // ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }


            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            objER.GetAllCommodityForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }


            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<Dnd_CartingRegister> lstcart = new List<Dnd_CartingRegister>();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAllCartingForPage(0);
            if (objER.DBResponse.Data != null)
                lstcart = (List<Dnd_CartingRegister>)objER.DBResponse.Data;

            return PartialView(lstcart);
        }
        [HttpGet]
        public JsonResult LoadMoreCartingList(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CartingRegister> LstJO = new List<Dnd_CartingRegister>();
            ObjCR.GetAllCartingForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CartingRegister>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCartingSearch(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CartingRegister> lstCart = new List<Dnd_CartingRegister>();
            objER.GetAllCartingEntryForSearch(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCart = (List<Dnd_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", lstCart);
        }

        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            Dnd_CartingRegister objCR = new Dnd_CartingRegister();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "view");
            if (objER.DBResponse.Data != null)
                objCR = (Dnd_CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_CartingRegister ObjCartingReg = new Dnd_CartingRegister();
            GodownRepository ObjGR = new GodownRepository();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Dnd_CartingRegister)ObjER.DBResponse.Data;
                }
            }

            //***************************************************************************
          

            List<Godown> lstGodown = new List<Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


           

            ObjER.GetAllCommodity();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;




            ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            ObjER.ListOfShippingLinePartyCode("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            //***************************************************************************
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            Dnd_CartingRegister objCR = new Dnd_CartingRegister();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (Dnd_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(Dnd_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                objCR.ApplicationDate = Convert.ToDateTime(objCR.ApplicationDate).ToString("dd/MM/yyyy");
                //List<int> lstLocation = new List<int>();
                IList<Dnd_CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_CartingRegisterDtl>>(objCR.XMLData);

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

                foreach (var item in LstCartingDtl)
                {
                    item.ShippingDate = string.IsNullOrEmpty(item.ShippingDate.ToString()) ? Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd")
                        : Convert.ToDateTime(item.ShippingDate).ToString("yyyy-MM-dd");
                }


                string XML = Utility.CreateXML(LstCartingDtl);
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
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
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }


        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.GodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.GodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddShortCargoDetail(ShortCargoDetails objSC, int CartingRegisterId, int CartingRegisterDtlId, string ShippingBillNo,string shortCargoEdit)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<ShortCargoDetails> lstShortCargoDetails = new List<ShortCargoDetails>();
            objSC.CartingDate = Convert.ToDateTime(objSC.CartingDate).ToString("yyyy-MM-dd");
            lstShortCargoDetails.Add(objSC);
            objER.AddShortCargoDetail(Utility.CreateXML(lstShortCargoDetails), CartingRegisterId, CartingRegisterDtlId, ShippingBillNo, shortCargoEdit);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult GetGodownDetail(int ShippingLineId)
        {
           Dnd_ShippingLineForPage objCR = new Dnd_ShippingLineForPage();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetGodownDetail(ShippingLineId);

            if (objER.DBResponse.Data != null)
                objCR = (Dnd_ShippingLineForPage)objER.DBResponse.Data;
            return Json(new {data=objCR ,status= objER.DBResponse.Status},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetShortCargoDetails(int CartingRegisterId)
        {           
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetShortCargoDetails(CartingRegisterId);
            
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeletEShortCargoEntry(int ShortCargoDtlId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (ShortCargoDtlId > 0)
                objER.DeleteShortCargoEntry(ShortCargoDtlId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            Dnd_StuffingRequest ObjSR = new Dnd_StuffingRequest();
            CHN_ExportRepository ObjCER = new CHN_ExportRepository();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();


          /*  ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<Dnd_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }*/
           /* ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }*/
            /* ObjER.GetShippingLine();
             if (ObjER.DBResponse.Data != null)
             { ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
             else
             { ViewBag.ShippingLineList = null; }*/
           /* ObjER.ListOfShippingLinePartyCode("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }

            ObjER.ListOfForwarderForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstFwd = Jobject["lstFwd"];
                ViewBag.FwdState = Jobject["State"];
            }
            else
            {
                ViewBag.lstFwd = null;
            }

            ObjER.ListOfMainlineForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstMln = Jobject["lstMln"];
                ViewBag.MlnState = Jobject["State"];
            }
            else
            {
                ViewBag.lstMln = null;
            }*/
            /*  ObjER.GetAllContainerNo();
               if (ObjER.DBResponse.Data != null)
               {
                   ViewBag.ContainerList = new SelectList((List<Dnd_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                   ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
               }
               else
                   ViewBag.ContainerList = null;*/

            /*ObjCER.ListOfPOD();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListOfPOD = null;*/

            /*ObjCER.ListOfCityForStuffingReq();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListCity = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListCity = null;*/

            /*If User is External Or Non CWC User*/
            bool Exporter, CHA;
            Exporter = ((Login)Session["LoginUser"]).Exporter;
            CHA = ((Login)Session["LoginUser"]).CHA;
           /* if (CHA == true)
            {
                ObjSR.CHA = ((Login)Session["LoginUser"]).Name;
                ObjSR.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
            }
            else
            {
                ObjCER.ListOfCHA();
                if (ObjCER.DBResponse.Data != null)
                    ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjCER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                else
                    ViewBag.ListOfCHA = null;
            }*/
           
           Dnd_EntryThroughGateRepository ObjETR = new Dnd_EntryThroughGateRepository();
              ObjETR.ListContainerClass();
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ContClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ContClass = null;
            }
           
            ObjSR.RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView("CreateStuffingRequest", ObjSR);
        }

        [HttpGet]
        public JsonResult ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCartingRegisterNo(int ShippingLineId,string StuffRefType)
        {
            if (ShippingLineId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.GetCartRegForStuffingReq(ShippingLineId, StuffRefType);
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetContainerNo(int ShippingLineId)
        {
            if (ShippingLineId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.GetContainerNo(ShippingLineId);
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
     
        public ActionResult AddEditStuffingReq(Dnd_StuffingRequest ObjStuffing)
        {
            ModelState.Remove("CHAId");
            ModelState.Remove("ForwarderId");
            ModelState.Remove("ViaId");
            if (ModelState.IsValid)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                IList<Dnd_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<Dnd_StuffingRequestDtl>>(ObjStuffing.StuffingXML);                
                IList<Dnd_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<Dnd_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);                
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
            Dnd_StuffingRequest ObjStuffing = new Dnd_StuffingRequest();
            CHN_ExportRepository ObjCER = new CHN_ExportRepository();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();

           /* ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }*/
            /* ObjER.GetShippingLine();
             if (ObjER.DBResponse.Data != null)
             { ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
             else
             { ViewBag.ShippingLineList = null; }*/
            /*ObjER.ListOfShippingLinePartyCode("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }

            ObjER.ListOfForwarderForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstFwd = Jobject["lstFwd"];
                ViewBag.FwdState = Jobject["State"];
            }
            else
            {
                ViewBag.lstFwd = null;
            }

            ObjER.ListOfMainlineForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstMln = Jobject["lstMln"];
                ViewBag.MlnState = Jobject["State"];
            }
            else
            {
                ViewBag.lstMln = null;
            }*/

            /*ObjCER.ListOfPOD();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListOfPOD = null;

            ObjCER.ListOfCityForStuffingReq();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListCity = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListCity = null;*/


            if (StuffinfgReqId > 0)
            {
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_StuffingRequest)ObjER.DBResponse.Data;
                }
                /*If User is External Or Non CWC User*/
               /* bool Exporter, CHA;
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
                    ObjCER.ListOfCHA();
                    if (ObjCER.DBResponse.Data != null)
                        ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjCER.DBResponse.Data, "CHAEximTraderId", "CHAName");
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
                    ObjCER.ListOfExporter();
                    if (ObjCER.DBResponse.Data != null)
                        ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjCER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                    else
                        ViewBag.ListOfExporter = null;
                }*/
                Dnd_EntryThroughGateRepository ObjETR = new Dnd_EntryThroughGateRepository();
                ObjETR.ListContainerClass();
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.ContClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ObjETR.DBResponse.Data;
                }
                else
                {
                    ViewBag.ContClass = null;
                }
             
            }
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            Dnd_StuffingRequest ObjStuffing = new Dnd_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.DeleteStuffingRequest(StuffinfgReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetCartRegDetForStuffingReq(int CartingRegisterId,string StuffRefType)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Kdl_GetCartRegDetForStuffingReq(CartingRegisterId, StuffRefType);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }

        [HttpGet]
        public ActionResult LoadStuffingReqList(int Page)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchStuffingReq(string ContNo)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.SearchStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode,string ContainerNo)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            // StuffingReqContainerDtl ObjSRD = new StuffingReqContainerDtl();
            ObjER.GetContainerNoDet(CFSCode, ContainerNo);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ObjSRD = (StuffingReqContainerDtl)ObjER.DBResponse.Data;
            //}
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetForeignLinerForStuffingReq()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetForeignLinerList();
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Container Stuffing
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjCS = new Dnd_ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }

            /*ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            ObjER.ListOfShippingLinePartyCode("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }*/

            ObjER.ListOfPOL();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }

            /*ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPortOfDischarge = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);

            }
            else
            {
                ViewBag.ListOfPortOfDischarge = null;
            }*/

            return PartialView(ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }

        [HttpGet]
        public ActionResult LoadContainerStuffingList(int Page)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchContainerStuffing(string ContNo)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.SearchContainerStuffing(ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
                }

                ObjER.ListOfChaForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstCHA = null;
                }
                ObjER.ListOfExporterForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstExp = Jobject["lstExp"];
                    ViewBag.ExpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstExp = null;
                }
                ObjER.ListOfShippingLinePartyCode("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }


                ObjER.ListOfPOL();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
                }
                else
                {
                    ViewBag.ListOfPOD = null;
                }
                //ObjER.ListOfPOD();
                //if (ObjER.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfPortOfDischarge = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);

                //}
                //else
                //{
                //    ViewBag.ListOfPortOfDischarge = null;
                //}
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<Dnd_ContainerStuffingDtl> LstStuffing = new List<Dnd_ContainerStuffingDtl>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(Dnd_ContainerStuffing ObjStuffing)
        {
           // ModelState.Remove("GENSPPartyCode");
           // ModelState.Remove("GREPartyCode");
           // ModelState.Remove("INSPartyCode");
           // ModelState.Remove("HandalingPartyCode");
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<Dnd_ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML);                                                                       
                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };

                var errors =
                from item in ModelState
                where item.Value.Errors.Count > 0
                select item.Key;
                var keys = errors.ToArray();
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteContainerStuffingDet(int ContainerStuffingId)
        {
            if (ContainerStuffingId > 0)
            {
                CHN_ExportRepository ObjER = new CHN_ExportRepository();
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
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuff(Dnd_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "",
            Area = "", PortName = "", PortDestination = "", Remarks = "",CargoType="", PortDischarge,Via="",Vessel="";

            String Consignee = ""; int SerialNo = 1;
            if (ObjStuffing.LstStuffing.Count() > 0)
            {
                ObjStuffing.LstStuffing.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    ShippingBillNo = (ShippingBillNo == "" ? ((item.ShippingBillNo) + " ") : (item.ShippingBillNo + "<br/>" + item.ShippingBillNo + " "));
                    /*   if (ShippingBillNo == "")
                           ShippingBillNo = item.ShippingBillNo + " ";
                       else
                           ShippingBillNo += "," + item.ShippingBillNo; */
                });

                ObjStuffing.LstStuffing.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
                {

                    ShippingDate = (ShippingDate == "" ? (item.ShippingDate) : (item.ShippingDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { EntryNo = x.EntryNo }).Distinct().ToList().ForEach(item =>
                {

                    EntryNo = (EntryNo == "" ? (item.EntryNo) : (item.EntryNo));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
                {

                    InDate = (InDate == "" ? (item.InDate) : (item.InDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                ObjStuffing.LstStuffing.Select(x => new { PortName = x.PortName }).Distinct().ToList().ForEach(item =>
                {
                    if (PortName == "")
                        PortName = item.PortName;
                    else
                        PortName += "," + item.PortName;
                });
                ObjStuffing.LstStuffing.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });


                ObjStuffing.LstStuffing.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });
                ObjStuffing.LstStuffing.Select(x => new { Consignee = x.Consignee }).Distinct().ToList().ForEach(item =>
                {
                    if (Consignee == "")
                        Consignee = item.Consignee;
                    else
                        Consignee += "," + item.Consignee;
                });

                ObjStuffing.LstStuffing.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                {

                    if (ShippingLine == "")
                        ShippingLine = item.ShippingLine;
                    else
                        ShippingLine += "," + item.ShippingLine;
                });

                ObjStuffing.LstStuffing.Select(x => new { Remarks = x.Remarks }).Distinct().ToList().ForEach(item =>
                {

                    if (Remarks == "")
                        Remarks = item.Remarks;
                    else
                        Remarks += "," + item.Remarks;
                });
                ObjStuffing.LstStuffing.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                {
                    if (CHA == "")
                        CHA = item.CHA;
                    else
                        CHA += "," + item.CHA;
                });

                //StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                //Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                StuffQuantity = (ObjStuffing.LstStuffing.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffing.Sum(x => x.StuffQuantity)).ToString() : "";

                ObjStuffing.LstStuffing.ToList().ForEach(item =>
                {
                  
                    Commodity = (Commodity == "" ? (item.CommodityName) : Commodity == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));
               
                });

                ObjStuffing.LstCont.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                    CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));
                    //SerialNo++;
                });

              
                ObjStuffing.LstStuffing.Select(x => new { CustomSeal = x.CustomSeal }).Distinct().ToList().ForEach(item =>
                {

                    if (CustomSeal == "")
                        CustomSeal = item.CustomSeal;
                    else
                        CustomSeal += "<br/>" + item.CustomSeal;
                });

                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";

                Html += "<thead>";

                Html += "<tr><td colspan='4'>";
                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
                Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
                Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + ObjStuffing.CompanyAddress + "</span><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER STUFFING SHEET</label><br/><label style='font-size: 14px;'> <b>Shed No :</b> " + ObjStuffing.GodownName + "</label></td>";
                Html += "<td width='12%' align='right' valign='top'>";
                Html += "<table style='width:100%;' cellspacing='0' cellpadding='0' valign='top'><tbody>";
                Html += "<tr><td style='border:1px solid #333;' valign='top'>";
                Html += "<div valign='top' style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CFSCHN/69</div>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</thead>";

                Html += "<tbody>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='8%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='8%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>CFS Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='15%' style='margin:0 0 10px;'><b>Size :</b> <u>" + ObjStuffing.Size + "</u></td>  <td colspan='3' width='35%' style='margin:0 0 10px; text-align: right;'><b>Shipping Line :</b> <u>" + ShippingLine + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Via No. :</b> <u>" + ObjStuffing.Via + "    " + ObjStuffing.Vessel + "</u> </td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Port Of Discharge :</b> <u>" + ObjStuffing.PODischarge + "</u></td> </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Cont Type</b> <u>" + CargoType + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Name : </b> <u>" + ObjStuffing.Mainline + "</u></td>  </tr></tbody></table> </td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='8' style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>S. No.</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Entry No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>InDate</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb Date</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Exporter</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Consignee</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Comdty</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Pkts</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Gr Wt</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>FOB</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Remarks</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                ObjStuffing.LstStuffing.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>" + SerialNo++ + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.EntryNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.InDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingBillNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.Exporter + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Consignee + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.CommodityName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffQuantity + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffWeight + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Fob + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Remarks + "</td></tr>";
                });
                //LOOP END

                Html += "<tr><th style='padding:3px;text-align:center;width:5%;'>Total :</th>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:10%;'></td>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.StuffQuantity) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.StuffWeight) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.Fob) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'></th></tr>";

                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td colspan='3' width='25%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>Signature and designation</td>";
                Html += "<td colspan='5' width='41.66666666666667%' style='padding:3px;text-align:left;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>1</td><td colspan='2' width='85%' style='font-size:11px;'>All activities including those at incomming and in process stages have been satisfactorily completed.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>2</td><td colspan='2' width='85%' style='font-size:11px;'>All the necessary records have been completed and verified with Date and seal.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>3</td><td colspan='2' width='85%' style='font-size:11px;'>Cargo/Containers delivered in good condition.</td></tr>";
                Html += "</tbody></table>";
                Html += "</td>";
                Html += "<td colspan='1' width='8.333333333333333%'></td>";
                Html += "<td colspan='4' width='33.33333333333333%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>The container is allowed to be sent to Jawaharlal Nehru Port Officer on behalf of Jawahar Custom's House</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td style='text-align:left;'>Shed Asst. <br/> CFS Dronagiri</td><td style='text-align:left;'>Shed I/C <br/>CWC-CFS,D-NODE</td><td style='text-align:center;'>(On behalf of Jawahar Custom's House)</td></tr></tbody></table></td></tr>";
                Html += "</tbody></table>";


            }

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            // if (Convert.ToInt32(Session["BranchId"]) == 1)
            // {
            //Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>";

            //Html += "<tr><td colspan='4'>";
            //Html += "<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>";
            //Html += "<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>";
            //Html += "<td width='8%' align='right'>";
            //Html += "<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>";
            //Html += "<tr><td style='border:1px solid #333;'>";
            //Html += "<div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/15</div>";
            //Html += "</td></tr>";
            //Html += "</tbody></table>";
            //Html += "</td></tr>";
            //Html += "</tbody></table>";
            //Html += "</td></tr>";

            //Html += "<tr><td colspan='4'>";
            //Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            //Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            //Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally<br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 14px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER STUFFING SHEET(FCL/LCL)</label></td>";
            //Html += "<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>";
            //Html += "</tbody></table>";
            //Html += "</td></tr>";

            //Html += "</thead>";
            //Html += "<tbody>  <tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><th colspan='1' width='3%' style='padding:3px;text-align:left;'>Sl.No</th><td colspan='10' width='5%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Date</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr></tbody></table></td></tr>   <tr><td colspan='4'><p style='font-size:9pt; margin:0 0 10px;'><b>Container No.</b> <u>" + ContainerNo + "</u> <b>CFS Code No.</b> <u>" + CFSCode + "</u> <b>Godown No / Bay No.</b> <u>" + ObjStuffing.GodownName + "</u> </p></td></tr>      <tr><td colspan='4'><p style='font-size:9pt; margin:0 0 10px;'><b>Shhiping Agent</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> <b>Shipping Line</b> <u>" + ShippingLine + "</u> <b>Size</b> <u>" + ObjStuffing.Size + "</u> <b>Shipping Line Seal No.</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> </p></td></tr>      <tr><td colspan='4'><p style='font-size:9pt; margin:0 0 10px;'><b>Vessel</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> <b>Country of Origin</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> <b>VIA No.</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> <b>Voyage</b> <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u> <b>Customs Seal No.</b> <u>" + CustomSeal + "" + "</u> </p></td></tr>  <tr><td colspan='4' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Shipping Bill No. and Date</th> <th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Exporter</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Consignee</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Name Of Goods</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Carting No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>No.of Pkgs Stuffed</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Gross Weight in Mts</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Value as per S/B INR(in lacs)</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Consignee + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>Carting No - [DATA]</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='20' style='border:1px solid #000;'><table cellspacing='0' cellpadding='5' style='width:100%; margin: 0; padding: 5px; font-size:8pt;'><tbody><tr><td colspan='16' width='80%'></td><th style='border-right:1px solid #000;padding:3px;text-align:right;'>Grand Total</th><td style='padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='12' style='border:1px solid #000;padding:3px;text-align:left;'>Variation observed if Any</td></tr> <tr><td colspan='12'><span><br/></span></td></tr><tr><td colspan='12' style='padding:3px;text-align:left;'>Signature and designation <br/> with date & Seal</td></tr></tbody></table></td></tr><tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;'>Representative/Surveyor <br/> Shipping Agent/Line/CHA</td><td style='text-align:left;'>Shed Asst. <br/> CWC.CFS</td><td style='text-align:left;'>Shed I/c <br/> CWC.CFS</td><td style='text-align:center;'>Rep/Surveyor of Handling & <br/> Transport Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            //// }
            //// else
            //// {
            ////    Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            ////}
            //// Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>Container Freight Station<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'>FORMAT FOR STUFFING SHEET(FCL/LCL)<br/><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>CFS Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-CFS</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {

                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        }
        //[NonAction]
        //public string GeneratePdfForContainerStuff(Chn_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        //{
        //    string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        //    string Html = "";
        //    string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
        //    StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "";
        //    int SerialNo = 1;
        //    if (ObjStuffing.LstStuffingDtl.Count() > 0)
        //    {
        //        ObjStuffing.LstStuffingDtl.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (ShippingBillNo == "")
        //                ShippingBillNo = item.ShippingBillNo;
        //            else
        //                ShippingBillNo += "," + item.ShippingBillNo;
        //        });

        //        ObjStuffing.LstStuffingDtl.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (ShippingDate == "")
        //                ShippingDate = item.ShippingDate;
        //            else
        //                ShippingDate += "," + item.ShippingDate;
        //        });

        //        ObjStuffing.LstStuffingDtl.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (Exporter == "")
        //                Exporter = item.Exporter;
        //            else
        //                Exporter += "," + item.Exporter;
        //        });
        //        ObjStuffing.LstStuffingDtl.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (ShippingLine == "")
        //                ShippingLine = item.ShippingLine;
        //            else
        //                ShippingLine += "," + item.ShippingLine;
        //        });
        //        ObjStuffing.LstStuffingDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (CHA == "")
        //                CHA = item.CHA;
        //            else
        //                CHA += "," + item.CHA;
        //        });

        //        StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
        //        Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
        //        StuffQuantity = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity)).ToString() : "";
        //        ObjStuffing.LstStuffingDtl.ToList().ForEach(item =>
        //        {
        //            SLNo = SLNo + SerialNo + "<br/>";
        //            CFSCode = (CFSCode == "" ? (item.CFSCode) : (CFSCode + "<br/>" + item.CFSCode));
        //            ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
        //            CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : (CustomSeal + "<br/>" + item.CustomSeal));
        //            Commodity = (Commodity == "" ? (item.CommodityName) : (Commodity + "<br/>" + item.CommodityName));
        //            SerialNo++;
        //        });
        //        SLNo.Remove(SLNo.Length - 1);
        //    }

        //    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //    }

        //    Html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='width:20%;text-align:right;'><img src='IMGSRC' style='width:50%;' /></th><th style='width:60%;font-weight:600;text-align:center;'><span style='font-size:10pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/>(A Govt. of India Undertaking)</th><th style='text-align:center;'>ICD Patparganj-Delhi<br/><br/>_____________________</th></tr></thead><tbody><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>S.No</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><td rowspan='2' style='width:60%;font-weight:600;text-align:center;'><span style='border-bottom:1px solid #000;'>CONTAINER STUFFING SHEET</span></td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Date</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Size</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.Size + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Godown</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.GodownName + "</td></tr></tbody></table></td></tr><tr><td colspan='3'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill No.</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingBillNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Bill Date:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingDate + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Gross Weight:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffWeight + "</td></tr><tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Exporter:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Exporter + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Shipping Line:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ShippingLine + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>FOBValue(INR):</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Fob + "</td></tr>			<tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Consignee/CHA:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CHA + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Destination:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>No. of Packages Stuffed:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + StuffQuantity + "</td></tr>   <tr><td style='border:1px solid #000;padding:3px;text-align:left;font-weight:600;'>Via:</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ObjStuffing.ContVia + "</td></tr>  </tbody></table></td></tr><tr><td colspan='3' style='padding-top:20pt;'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>S. No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>ICD Code</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Container / CBT No.</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Commodity</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Customs Seal</th><th colspan='10' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Tally</th><th style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Total</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:3px;text-align:center;'>" + SLNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CFSCode + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + Commodity + "</td><td style='border:1px solid #000;padding:3px;text-align:left;'>" + CustomSeal + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:center;'>-</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + "" + "</td></tr><tr><td colspan='10'></td><td colspan='4' style='border:1px solid #000;padding:3px;text-align:center;font-weight:600;'>Grand Total</td><td style='border:1px solid #000;padding:3px;text-align:right;'>" + StuffQuantity + "</td></tr><tr><td colspan='15' style='padding:3px;text-align:left;'>Variation observed if Any</td></tr></tbody></table></td></tr><tr><td colspan='3' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:center;'>Signature & <br/>Designation with<br/>Date and Seal</td><td style='text-align:center;'>Rep./Surveyor/S.<br/>Agent/Line/CHA</td><td style='text-align:center;'>Shed I/C CWC-ICD</td><td style='text-align:center;'>Rep./Surveyor of H & T<br/>Contractor</td><td style='text-align:center;'>Preventive Officer of Customs</td></tr></tbody></table></td></tr></tbody></table>";
        //    Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
        //    using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
        //    {
        //        Rh.GeneratePDF(Path, Html);
        //    }
        //    return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        //}

        [HttpGet]
        public JsonResult ListOfGREParty()
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.ListOfGREParty();
            //List<Chn_ContainerStuffing> objImp = new List<Chn_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                ((List<Chn_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GREPartyId = item.GREPartyId, GREPartyCode = item.GREPartyCode });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGroundRentEmpty(String StuffingDate, String ArrayOfContainer, int GREPartyId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGroundRentEmpty(StuffingDate, ContainerStuffingXML, GREPartyId);
            Chn_ContainerStuffing objImp = new Chn_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (Chn_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfINSParty()
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.ListOfINSParty();
            //List<Chn_ContainerStuffing> objImp = new List<Chn_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<Chn_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<Chn_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { INSPartyId = item.INSPartyId, INSPartyCode = item.INSPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateINS(String StuffingDate, String ArrayOfContainer, int INSPartyId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }
            objImport.CalculateINS(StuffingDate, ContainerStuffingXML, INSPartyId);
            Chn_ContainerStuffing objImp = new Chn_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (Chn_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfSTOParty()
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.ListOfSTOParty();
            //List<Chn_ContainerStuffing> objImp = new List<Chn_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<Chn_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<Chn_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { STOPartyId = item.STOPartyId, STOPartyCode = item.STOPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateSTO(String StuffingDate, String ArrayOfContainer, int STOPartyId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateSTO(StuffingDate, ContainerStuffingXML, STOPartyId);
            Chn_ContainerStuffing objImp = new Chn_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (Chn_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfHandalingParty()
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.ListOfHandalingParty();
            //List<Chn_ContainerStuffing> objImp = new List<Chn_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<Chn_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<Chn_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { HandalingPartyId = item.HandalingPartyId, HandalingPartyCode = item.HandalingPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateHandaling(String StuffingDate, String Origin, String Via, String ArrayOfContainer, int HandalingPartyId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateHandaling(StuffingDate, Origin, Via, ContainerStuffingXML, HandalingPartyId);
            Chn_ContainerStuffing objImp = new Chn_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (Chn_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListOfGENSPParty()
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.ListOfGENSPParty();
            //List<Chn_ContainerStuffing> objImp = new List<Chn_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<Chn_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<Chn_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { GENSPPartyId = item.GENSPPartyId, GENSPPartyCode = item.GENSPPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGENSP(String StuffingDate, String ArrayOfContainer, int GENSPPartyId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGENSP(StuffingDate, ContainerStuffingXML, GENSPPartyId);
            Chn_ContainerStuffing objImp = new Chn_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (Chn_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }



            ObjIR.GetContainerForMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LstContainerNo = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "ContainerStuffingId", "Container");
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }
            ObjIR.GetShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjIR.GetPaymentParty();
            if (ObjIR.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            //ObjIR.ListOfGodown();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            //}
            //else
            //{
            //    ViewBag.ListOfGodown = null;
            //}


            ObjIR.GetLocationForInternalMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LocationNoList = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
            }
            else
            {
                ViewBag.LocationNoList = null;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetInternalMovementList()
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            ObjIR.GetAllInternalMovement();
            List<PPG_ContainerMovement> LstMovement = new List<PPG_ContainerMovement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_ContainerMovement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            PPG_ContainerMovement ObjInternalMovement = new PPG_ContainerMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_ContainerMovement)ObjIR.DBResponse.Data;
                //ObjIR.ListOfGodown();
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                //}
                //else
                //{
                //    ViewBag.ListOfGodown = null;
                //}
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovement(int MovementId)
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            PPG_ContainerMovement ObjInternalMovement = new PPG_ContainerMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_ContainerMovement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetConDetails(int ContainerStuffingDtlId, String ContainerNo)
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            ObjIR.GetConDetForMovement(ContainerStuffingDtlId, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                PPG_ContainerMovement ObjInternalMovement = new PPG_ContainerMovement();
                ObjInternalMovement = (PPG_ContainerMovement)ObjIR.DBResponse.Data;
                ViewBag.ShippingBill = new SelectList((List<PPG_ShippingBill>)ObjInternalMovement.ShipBill, "shippingBillNo", "shippingBillNo");
            }
            else
            {
                ViewBag.ShippingBill = null;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int ContainerStuffingDtlId, int ContainerStuffingId, string ContainerNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int portvalue, decimal tareweight, int InvoiceId = 0)
        {

            CHN_ExportRepository objChrgRepo = new CHN_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(ContainerStuffingDtlId, ContainerStuffingId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, ctype, portvalue, tareweight, InvoiceId);

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

            var Output = (PPG_MovementInvoice)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = MovementDate;
            Output.Module = "EXPMovement";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                //if (!Output.CHAName.Contains(item.CHAName))
                // Output.CHAName += item.CHAName + ", ";
                if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                    Output.ImporterExporter += item.ImporterExporter + ", ";
                //if (!Output.BOENo.Contains(item.BOENo))
                //    Output.BOENo += item.BOENo + ", ";
                //if (!Output.BOEDate.Contains(item.BOEDate))
                //    Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
                    Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                //if (!Output.CartingDate.Contains(item.CartingDate))
                //    Output.CartingDate += item.CartingDate + ", ";
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

            if (ctype == "W")
                PPGTentativeInvoice.InvoiceObjW = Output;

            if (ctype == "GR")
                PPGTentativeInvoice.InvoiceObjGR = Output;

            if (ctype == "FMC")
                PPGTentativeInvoice.InvoiceObjFMC = Output;

            return Json(Output, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult MovementInvoicePrint(string InvoiceNo)
        {
            CHN_ExportRepository objGPR = new CHN_ExportRepository();
            if (InvoiceNo == "")
            {
                return Json(new { Status = -1, Message = "Error" });

            }
            else
            {
                objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "EXPMovement");
                PPG_Movement_Invoice objGP = new PPG_Movement_Invoice();
                string FilePath = "";
                if (objGPR.DBResponse.Data != null)
                {
                    objGP = (PPG_Movement_Invoice)objGPR.DBResponse.Data;
                    FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                    return Json(new { Status = 1, Message = FilePath });
                }

            }





            return Json(new { Status = -1, Message = "Error" });



        }
        private string GeneratingPDFInvoiceMovement(PPG_Movement_Invoice objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/MovementInvoice" + InvoiceId.ToString() + ".pdf";
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
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
            html.Append("<br />MOVEMENT OF EXPORT CONTAINER");
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
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            //   html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Week</th>");

            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                // html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.CartingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DestuffingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                //     html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //   html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");

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
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
            html.Append("<tbody>");
            i = 1;
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");

            html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody>");

            html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; width: 100px;'>Total :</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalCGST.ToString("0.00") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalSGST.ToString("0.00") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalIGST.ToString("0.00") + "</th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.ShippingLineName.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/MovementInvoice" + InvoiceId.ToString() + ".pdf";
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalPaymentSheet(FormCollection objForm)
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

                //var invoiceData = JsonConvert.DeserializeObject<PPGInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //string ContWiseCharg = "";

                //foreach (var item in invoiceData.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
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
                //if (invoiceData.lstCfsCodewiseRateHT != null)
                //{
                //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                //}
                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");
                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //   int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<PPG_Movement_Invoice>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceDataa = JsonConvert.DeserializeObject<PPG_Movement_Invoice>(objForm["PaymentSheetModelJsonn"].ToString());

                var invoiceDataaa = JsonConvert.DeserializeObject<PPG_Movement_Invoice>(objForm["PaymentSheetModelJsonnn"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

                string ContainerXMLL = "";
                string ChargesXMLL = "";
                string ContWiseChargg = "";
                string OperationCfsCodeWiseAmtXMLL = "";
                string ContainerXMLLL = "";
                string ChargesXMLLL = "";
                string ContWiseCharggg = "";
                string OperationCfsCodeWiseAmtXMLLL = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                foreach (var item in invoiceDataa.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }
                foreach (var item in invoiceDataaa.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }

                if (invoiceDataa.lstPostPaymentCont != null)
                {
                    ContainerXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentCont);
                }
                if (invoiceDataa.lstPostPaymentChrg != null)
                {
                    ChargesXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrg);
                }
                if (invoiceDataa.lstContWiseAmount != null)
                {
                    ContWiseChargg = Utility.CreateXML(invoiceDataa.lstContWiseAmount);
                }
                if (invoiceDataa.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXMLL = Utility.CreateXML(invoiceDataa.lstOperationCFSCodeWiseAmount);
                }

                if (invoiceDataaa.lstPostPaymentCont != null)
                {
                    ContainerXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentCont);
                }
                if (invoiceDataaa.lstPostPaymentChrg != null)
                {
                    ChargesXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrg);
                }
                if (invoiceDataaa.lstContWiseAmount != null)
                {
                    ContWiseCharggg = Utility.CreateXML(invoiceDataaa.lstContWiseAmount);
                }
                if (invoiceDataaa.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXMLLL = Utility.CreateXML(invoiceDataaa.lstOperationCFSCodeWiseAmount);
                }

                CHN_ExportRepository objChargeMaster = new CHN_ExportRepository();
                objChargeMaster.AddEditInvoiceMovement(invoiceData, invoiceDataa, invoiceDataaa, ContainerXML, ContainerXMLL, ContainerXMLLL, ChargesXML, ChargesXMLL, ChargesXMLLL, ContWiseCharg, ContWiseChargg, ContWiseCharggg, OperationCfsCodeWiseAmtXML, OperationCfsCodeWiseAmtXMLL, OperationCfsCodeWiseAmtXMLLL, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPMovement");
                PPG_InvoiceList inv = new PPG_InvoiceList();
                inv = (PPG_InvoiceList)objChargeMaster.DBResponse.Data;

                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    invoiceData.InvoiceNo = inv.invoiceno;
                    invoiceData.invoicenoo = inv.invoicenoo;
                    invoiceData.invoicenooo = inv.invoicenooo;
                    invoiceData.MovementNo = inv.MovementNo;
                }
                invoiceDataa.ROAddress = (invoiceDataa.ROAddress).Replace("|_br_|", "<br/>");
                invoiceDataa.CompanyAddress = (invoiceDataa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    invoiceDataa.InvoiceNo = inv.invoicenoo;
                }
                invoiceDataaa.ROAddress = (invoiceDataaa.ROAddress).Replace("|_br_|", "<br/>");
                invoiceDataaa.CompanyAddress = (invoiceDataaa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    invoiceDataaa.InvoiceNo = inv.invoicenooo;
                }

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
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovement(PPG_ContainerMovement ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                CHN_ExportRepository ObjIR = new CHN_ExportRepository();
                ObjIR.AddEditImpInternalMovement(ObjInternalMovement);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }

        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DelInternalMovement(int MovementId)
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();

            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }


        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Loaded Container Invoice
        [HttpGet]
        public ActionResult CreateLoadedContainerPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetLoadedContainerRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetLoadedPaymentSheetContainer(int StuffingReqId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetContainerForLoadedContainerPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
        //    int PartyId, int IsLock, int IsReefer, string PlugInDateTime, string PlugOutDateTime, int InvoiceId = 0)        
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
            int PartyId, int IsLock,int IsGroundRent, int InvoiceId = 0)      
            {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }
            Dnd_ExportRepository objChrgRepo = new Dnd_ExportRepository();
            //objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, InvoiceDate, InvoiceType, ContainerXML, PayeeId, PartyId, IsLock, IsReefer, PlugInDateTime, PlugOutDateTime, InvoiceId);
            objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, InvoiceDate, InvoiceType, ContainerXML, PayeeId, PartyId, IsLock, IsGroundRent, InvoiceId);
            var Output = (DND_ExpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPLod";

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
                obj.ContainerClass = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerClass.ToString();
                obj.PayMode = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().PayMode;
                obj.ExportType = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ExportType;
                obj.IsODC= Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.ODC);
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
                    Output.ArrivalDate += item.ArrivalDate;

                Output.TotalNoOfPackages = Output.lstPostPaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPostPaymentCont.Sum(o => o.GrossWt);
                Output.TotalWtPerUnit = Output.lstPostPaymentCont.Sum(o => o.WtPerUnit);
                Output.TotalSpaceOccupied = Output.lstPostPaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPostPaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPostPaymentCont.Sum(o => o.CIFValue) + Output.lstPostPaymentCont.Sum(o => o.Duty);
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
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadedContPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<DND_ExpPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

                foreach (var item in invoiceData.lstPSCont)
                {
                    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                foreach (var item in invoiceData.lstOperationContwiseAmt)
                {
                    if (item.DocumentDate != "")
                    {
                        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        item.DocumentType = "";
                    }
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
                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditInvoiceContLoaded(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);

                //return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Export Destuffing

        public ActionResult CreateExportDestuffing(int DestuffingEntryId=0)
        {
            Dnd_ExportDestuffing ObjDestuffing = new Dnd_ExportDestuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = ObjER.DBResponse.Data;
            //}
            ObjER.GetCCINShippingLineForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstShippingLine = Jobject["LstShippingLine"];
                ViewBag.SLAState = Jobject["State"];
            }
            else
            {
                ViewBag.LstShippingLine = null;
                ViewBag.SLAState = null;
            }

            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
                ViewBag.ExpState = null;

            }

            GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.GodownList = ObjGR.DBResponse.Data;
            }
            else
            {
                ViewBag.GodownList = null;
            }
            ObjER.ListOfCommodity();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ListOfCommodity = ObjER.DBResponse.Data;
            else
                ViewBag.ListOfCommodity = null;
            if (DestuffingEntryId > 0)
            {

                ObjER.GetDestuffingEntryDetailsById(DestuffingEntryId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjDestuffing = (Dnd_ExportDestuffing)ObjER.DBResponse.Data;
                }
            }
            else
            {
                ObjDestuffing.Destuffingdate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            return PartialView(ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoForExportDestuffing()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainersForExpDestuffing();
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSBDetForExportDestuffing(string CFSCode)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetSBDetForExpDestuffing(CFSCode);
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public JsonResult AddEditExpDestuffing(Dnd_ExportDestuffing objDestuff, String lstDestuffDetail)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
                List<Dnd_ExportDestuffDetails> lstDestuff = new List<Dnd_ExportDestuffDetails>();
                if (lstDestuffDetail.Length > 0)
                {
                    lstDestuff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ExportDestuffDetails>>(lstDestuffDetail);
                    lstDestuff.ForEach(x => x.SBDate = Convert.ToDateTime(x.SBDate).ToString("yyyy-MM-dd"));
                    DestuffingEntryXML = Utility.CreateXML(lstDestuff);
                }
                 objER.AddEditExpDestuffingEntry(objDestuff, DestuffingEntryXML);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }


        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntry(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }



        [HttpGet]
        public ActionResult DestuffingEntrySr(string search)
        {

            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.DestuffingEntrySr(search);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);



            //Dnd_ExportRepository objER = new Dnd_ExportRepository();
            //List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            //objER.LoadedContReqSr(0, search);
            ////objER.GetAllEIRData1(0, ContNo);
            //if (objER.DBResponse.Data != null)
            //    lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            //return PartialView("DestuffingEntryList", lstCont);
        }
        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntry(int Page)
        {
            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntry(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
                return Json(LstDestuffing, JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        public ActionResult ViewExportDestuffingEntry(int DestuffingEntryId)
        {
            Dnd_ExportDestuffing ObjDestuffing = new Dnd_ExportDestuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetDestuffingEntryDetailsById(DestuffingEntryId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjDestuffing = (Dnd_ExportDestuffing)ObjER.DBResponse.Data;
            }
            return PartialView(ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
                ObjIR.DelDestuffingEntry(DestuffingEntryId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetDestuffingEntryForPrint(int DestuffingId)
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetExportDestuffingForPrint(DestuffingId);
            string Path = "";
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                Path = GeneratePDFForDestuffSheet(ds, DestuffingId);
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(new { Status = 0, Data = "No Record Found" }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public string GeneratePDFForDestuffSheet(DataSet ds, int DestuffingId)
        {

            List<dynamic> lstSBhdr = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstSBhdtl = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);

            StringBuilder objSB = new StringBuilder();
            /*
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
             {
                 Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
             }
             if (System.IO.File.Exists(Path))
             {
                 System.IO.File.Delete(Path);
             }*/
            

            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: right;' colspan='12'>");
            objSB.Append("<h1 style='font-size: 12px; line-height: 20px; font-weight: 300;margin: 0; padding: 0;'>");
            objSB.Append("</h1></td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>" + lstSBhdr[0].CompanyAddress + "</label><br/><label style='font-size: 16px; font-weight:bold;'>EXPORT DESTUFFING SHEET</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            objSB.Append("<tr><th colspan='6' style='font-size:13px;width:50%'>SHED CODE: <span style='font-size:12px;font-weight:normal;'>" + lstSBhdr[0].GodownName + "</span></th>");
            objSB.Append("<th colspan='6' style='font-size:13px;text-align:right;width:50%'>AS ON: <span style='font-size:12px;font-weight:normal;'>" + lstSBhdr[0].DestuffingEntryDate + "</span></th></tr>");
            objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td style='text-align: left;'>");
            //objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>SHED CODE: <label>" + ObjDestuff.GodownName + "</label>");
            //objSB.Append("</span></td><td colspan='3' style='text-align: center;'>");
            //objSB.Append("<span style='display: block; font-size: 14px; line-height: 22px;  padding-bottom: 10px; font-weight:bold;'>DESTUFFING SHEET</span>");
            //objSB.Append("</td><td colspan='2' style='text-align: left;'><span style='display: block; font-size: 11px; padding-bottom: 10px;'>");
            //objSB.Append("AS ON: <label>" + ObjDestuff.DestuffingEntryDateTime + "</label></span></td></tr>");
           // var FOB = lstSBhdtl.Sum(x => x.FOB);
            //var GrossWeight = lstSBhdtl.Sum(x => x.GrossWeight);
           // var Area = lstSBhdtl.Sum(x => x.Area);


            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table style='width:100%; margin: 0;margin-bottom: 10px;'><tbody>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>DESTUFF SHEET NO.:</label> <span>" + lstSBhdr[0].DestuffingEntryNo + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px;width:33.33333333333333%;'><label style='font-weight: bold;'> DATE OF DESTUFFING : </label> <span>" + lstSBhdr[0].DestuffingEntryDate + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px;width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>Container / CBT No: </label> <span>" + lstSBhdr[0].ContainerNo + "</span></td></tr>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>Size:</label> <span>" + lstSBhdr[0].Size + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>ICD Code: </label> <span>" + lstSBhdr[0].CFSCode + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>In Date : </label> <span>" + lstSBhdr[0].GateInDate + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>Custom Seal No. :</label> <span>" + lstSBhdr[0].CustomSealNo + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>SLA : </label> <span>" + lstSBhdr[0].ShippingLine + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>Sla Seal no. : </label> <span>" + lstSBhdr[0].SLASealNo + "</span></td></tr>");

            objSB.Append("<tr><td colspan='12' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Remarks : </label> <span>" + lstSBhdr[0].Remarks + "</span></td></tr>");

            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table style='border:1px solid #000; font-size:8pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SR No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SB No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SB Date</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Exporter</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Cargo</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>Cargo Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>No. Pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Gr Wt</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>FOB</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>CUM</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Area</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Commodity</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Location</th>");
            objSB.Append("</tr></thead>");

            objSB.Append("<tfoot><tr>");
            objSB.Append("<td colspan='6' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-weight: bold; text-align: center; padding: 5px;'></td>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(lstSBhdtl.Sum(x => x.NoOfPackages)) + "</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: left; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<td colspan='2' style='border-bottom: 1px solid #000; text-align: left;'></td>");
            objSB.Append("</tr></tfoot>");
            objSB.Append("<tbody>");
            int Serial = 1;
            lstSBhdtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + Serial + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SBNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SBDate + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Exporter + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.CargoDescription + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.CargoType + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.NoOfPackages + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.GrossWeight + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.FOB + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.CUM + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.Area + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.CommodityName + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.GodownWiseLctnNames + "</td>");

                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr><tr>");
            objSB.Append("<td colspan='12' style=' font-size: 11px; padding-top: 15px; text-align: left;'>*GOODS RECEIVED ON S/C &amp; S/W BASIC - CWC IS NOT RESPONSIBLE FOR SHORT LANDING &amp; LEAKAGES IF ANY</td>");
            objSB.Append("</tr><tr><td colspan='12' style=' font-size: 12px; text-align: left;padding-top: 15px;'>Signature &amp; Designation :</td></tr></tbody>");
            objSB.Append("</table></td></tr>");
            objSB.Append("<tr><td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>H &amp; T Agent</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Consignee</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Shipping Line</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>ICD</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Customs</td></tr>");
            objSB.Append("</tbody></table>");

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ExpDestuffingSheet" + DestuffingId + ".pdf";

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {

                RH.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/ExpDestuffingSheet" + DestuffingId + ".pdf";
        }



        #endregion


        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objER.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objER.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            objER.ListOfChaForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            objER.ListOfExporterForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            objER.ListOfShippingLinePartyCode("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            objER.GetAllCommodityForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
        /*    Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                List<Port> lstport = (List<Port>)ObjRR.DBResponse.Data;
                ViewBag.ListOfPort = lstport;

                /*lstport = lstport.Where(m => m.PortName == "ICD PPG").ToList();
                if (lstport.Count > 0)
                {
                    ViewBag.PortName = lstport[0].PortName;
                    ViewBag.PortId = lstport[0].PortId;
                }
                else
                {
                    ViewBag.PortName = "";
                    ViewBag.PortId = 0;
                }*/

            //}
            Dnd_ExportRepository ExpRep = new Dnd_ExportRepository();
            ExpRep.ListContainerClass();
            if (ExpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ExpRep.DBResponse.Data;
            }
        
            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetVesselInfoForLoadContReq(int ViaId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetVesselDet(ViaId);
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetContainerForLoadContReq()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetContainerListForLoadCont();
           
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetViaForLoadContReq()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetViaList();
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetContainerDetForLoadContReq(string CFSCode)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetContainerDetForLoadCont(CFSCode);
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewLoadContainerRequest(int LoadContReqId)
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            Dnd_LoadContReq ObjContReq = new Dnd_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Dnd_LoadContReq)ObjRR.DBResponse.Data;
            }
            Dnd_ExportRepository ExpRep = new Dnd_ExportRepository();
            ExpRep.ListContainerClass();
            if (ExpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ExpRep.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            Dnd_LoadContReq ObjContReq = new Dnd_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Dnd_LoadContReq)ObjRR.DBResponse.Data;
                ObjRR.ListOfChaForPage("", 0);
                if (ObjRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstCHA = null;
                }
                ObjRR.ListOfExporterForPage("", 0);
                if (ObjRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstExp = Jobject["lstExp"];
                    ViewBag.ExpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstExp = null;
                }
                ObjRR.ListOfShippingLinePartyCode("", 0);
                if (ObjRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }
                ObjRR.ListOfCommodity();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ListOfCommodity = ObjRR.DBResponse.Data;
                else
                    ViewBag.ListOfCommodity = null;
            }
            ObjRR.GetPortOfLoading();
          /*  if (ObjRR.DBResponse.Data != null)
            {
                List<Port> lstport = (List<Port>)ObjRR.DBResponse.Data;
                ViewBag.ListOfPort = lstport;

                /*lstport = lstport.Where(m => m.PortName == "ICD PPG").ToList();
                if (lstport.Count > 0)
                {
                    ViewBag.PortName = lstport[0].PortName;
                    ViewBag.PortId = lstport[0].PortId;
                }
                else
                {
                    ViewBag.PortName = "";
                    ViewBag.PortId = 0;
                }*/

           // }
            Dnd_ExportRepository ExpRep = new Dnd_ExportRepository();
            ExpRep.ListContainerClass();
            if (ExpRep.DBResponse.Data != null)
            {
                //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ImpRep.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstContainerClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ExpRep.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            objER.GetAllLoadedContainerRq(0);
            //objER.LoadedContReqSr(0, search);
            if (objER.DBResponse.Data != null)
                lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequestSr(string search)
        {

            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            objER.LoadedContReqSr(0, search);
            //objER.GetAllEIRData1(0, ContNo);
            if (objER.DBResponse.Data != null)
                lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            return PartialView("ListLoadContainerRequest",lstCont);
        }
        [HttpGet]
        public JsonResult LoadMoreContainerRequestList(int Page)
        {
            Dnd_ExportRepository ETGR = new Dnd_ExportRepository();
            //ETGR.GetAllLoadedCntRqData(Page);
            ETGR.GetAllLoadedContainerRq(Page);
            List<ListLoadContReq> ListEIR = new List<ListLoadContReq>();

            if (ETGR.DBResponse.Data != null)
            
                ListEIR = (List<ListLoadContReq>)ETGR.DBResponse.Data;
            
            return Json(ListEIR, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(Dnd_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
                string XML = "";
                if (objReq.StringifyData != null)
                {
                    XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<Dnd_LoadContReqDtl>>(objReq.StringifyData));
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
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
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
        public JsonResult GetForeignLinerForLoadContReq()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetForeignLinerListLCR();
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Back To Town Cargo
        [HttpGet]
        public ActionResult CreateBTTCargo()
        {
            //User RightsList---------------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //------------------------------------------------------------------------------------------------------

            return PartialView();
        }

        [HttpGet]
        public ActionResult ListOfBTTCargo()
        {
            List<Dnd_BTTCargoEntry> lstBTTCargoEntry = new List<Dnd_BTTCargoEntry>();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetBTTCargoEntry();
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<Dnd_BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }





        [HttpGet]
        public ActionResult ListOfBTTCargoSearch(String Search)
        {
            List<Dnd_BTTCargoEntry> lstBTTCargoEntry = new List<Dnd_BTTCargoEntry>();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetBTTCargoEntrySearch(Search);
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<Dnd_BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView("ListOfBTTCargo",lstBTTCargoEntry);
        }

        [HttpGet]
        public JsonResult LoadPartyist(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfpartybttForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddBTTCargo()
        {



            Dnd_BTTCargoEntry objBTTCargoEntry = new Dnd_BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy");

            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            /*
            objExport.GetCartingAppList(0);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);
            */
            //objExport.GetCHAListForBTT();
            //if (objExport.DBResponse.Data != null)
            //    objBTTCargoEntry.lstCHAList = (List<Dnd_CHAList>)objExport.DBResponse.Data;

            objExport.ListOfpartybttForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }



            return PartialView(objBTTCargoEntry);
        }


        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetCartingNoForBTT(string RefType)
        {
            Dnd_BTTCargoEntry objBTTCargoEntry = new Dnd_BTTCargoEntry();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCartingNoForBTT(RefType);
            if (objExport.DBResponse.Data != null)
            {
                return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditBTTCargo(int BTTCargoEntryId)
        {
            Dnd_BTTCargoEntry objBTTCargoEntry = new Dnd_BTTCargoEntry();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Dnd_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.Dnd_BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

           /* objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);
        */
            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<Dnd_CHAList>)objExport.DBResponse.Data;
            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult ViewBTTCargo(int BTTCargoEntryId)
        {
            Dnd_BTTCargoEntry objBTTCargoEntry = new Dnd_BTTCargoEntry();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Dnd_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.Dnd_BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Dnd_BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.Dnd_BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public JsonResult GetCartingDetailList(int CartingId, string RefType)
        {
            try
            {
                Dnd_ExportRepository objExport = new Dnd_ExportRepository();
                if (CartingId > 0)
                    objExport.GetCartingDetailList(CartingId, RefType);
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
                Dnd_ExportRepository objExport = new Dnd_ExportRepository();
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
        public JsonResult AddEditBTTCargo(Dnd_BTTCargoEntry objBTT,string RefType="")
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objBTT.lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Dnd_BTTCargoEntryDtl>>(objBTT.Dnd_BTTCargoEntryDtlJS);
                    string XML = Utility.CreateXML(objBTT.lstBTTCargoEntryDtl);
                    Dnd_ExportRepository objExport = new Dnd_ExportRepository();
                    objExport.AddEditBTTCargoEntry(objBTT, XML, BranchId, ((Login)(Session["LoginUser"])).Uid, RefType);
                    ModelState.Clear();
                    return Json(objExport.DBResponse);
                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = 0, Message = ErrorMessage };
                    return Json(Err);
                    //var Err = new { Status = -1, Message = "Error" };
                    //return Json(Err);
                }
            }
            catch (Exception ex)
            {

                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region BTT Invoice
        [HttpGet]
        public ActionResult CreateBTTPaymentSheet(string type = "Tax")
        {          
            ViewData["InvType"] = type;
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCartingApplicationForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetShipBillNo(int StuffingReqId, string StuffingReqNo)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetShipBillForPaymentSheet(StuffingReqId, StuffingReqNo);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType,
            List<PaymentSheetContainer> lstPaySheetContainer,int Escort,int PartyId,
            int InvoiceId = 0)
        {
            //AppraisementId ----> StuffingReqID

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Dnd_ExportRepository objPpgRepo = new Dnd_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetBTTPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, Escort, InvoiceId, PartyId);
            var Output = (DndInvoiceBTT)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "BTT";

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
                    Output.lstPostPaymentCont.Add(new DNDExpInvoiceContainerBase
                    {
                        CargoType = item.CargoType,
                        CartingDate = item.CartingDate,
                        CFSCode = item.CFSCode,
                        CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        DeliveryType = item.DeliveryType,
                        DestuffingDate = item.DestuffingDate,
                        Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        Reefer = item.Reefer,
                        RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = item.StuffingDate,
                        WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        HeavyScrap = item.HeavyScrap,
                        //StuffCUM = item.StuffCUM
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
        public JsonResult AddEditBTTPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                //var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceData = JsonConvert.DeserializeObject<DndInvoiceBTT>(objForm["PaymentSheetModelJson"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string CargoXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = null;
                    item.StuffingDate = null;
                    item.DestuffingDate = null;
                    item.CartingDate = item.CartingDate;
                    item.SpaceOccupiedUnit = "SQM";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditBTTInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", CargoXML);

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

        public ActionResult ListOfExpInvoiceBTT(string Module ,string InvoiceNo = null, string InvoiceDate = null)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.ListOfExpInvoiceBTT(Module, InvoiceNo, InvoiceDate);
            List<Dnd_ListOfExpInvoiceBTT> obj = new List<Dnd_ListOfExpInvoiceBTT>();
            if (objER.DBResponse.Data != null)
                obj = (List<Dnd_ListOfExpInvoiceBTT>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoiceBTT", obj);
        }


        [HttpGet]
        public JsonResult CHABTTSearchByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //#region Export Destuffing
        //[HttpGet]
        //public ActionResult CreateExportDestuffing(string type = "Tax")
        //{
        //    AccessRightsRepository ACCR = new AccessRightsRepository();
        //    ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
        //    ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

        //    ViewData["InvType"] = type;
        //    Ppg_EntryThroughGateRepository objImport = new Ppg_EntryThroughGateRepository();
        //    CHN_ExportRepository objER = new CHN_ExportRepository();

        //    //Shipping Line List----------------------------------------------------------------
        //    objImport.ListOfShippingLine();
        //    if (objImport.DBResponse.Data != null)
        //    {
        //        ViewBag.ShippingLineList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
        //    }
        //    else
        //    {
        //        ViewBag.ShippingLineList = null;
        //    }

        //    //CHA List-------------------------------------------------------------------------
        //    objER.ListOfCHA();
        //    if (objER.DBResponse.Data != null)
        //        ViewBag.CHAList = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //    else
        //        ViewBag.CHAList = null;

        //    //Party List----------------------------------------------------------------------
        //    objER.GetPaymentParty();
        //    if (objER.DBResponse.Status > 0)
        //        ViewBag.PaymentParty = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //    else
        //        ViewBag.PaymentParty = null;

        //    //Containers List-----------------------------------------------------------------
        //    objER.GetContainersForExpDestuffing();
        //    if (objER.DBResponse.Status > 0)
        //        ViewBag.ContainersList = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //    else
        //        ViewBag.ContainersList = null;



        //    return PartialView();
        //}

        //[HttpGet]
        //public JsonResult GetChargesExportDestuffing(int ContainerStuffingId)
        //{
        //    CHN_ExportRepository objER = new CHN_ExportRepository();
        //    ExportDestuffing obj = new ExportDestuffing();
        //    objER.GetChargesForExpDestuffing(ContainerStuffingId);
        //    if (objER.DBResponse.Status > 0)
        //    {
        //        obj.lstCharges = (List<ExportDestuffingCharges>)objER.DBResponse.Data;
        //        obj.ContainerStuffingId = ContainerStuffingId;
        //        obj.Total = obj.lstCharges.Sum(o => o.TotalAmount);
        //        return Json(obj, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //[HttpPost]
        //public JsonResult GetExportDestuffingPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,
        //    List<PaymentSheetContainer> lstPaySheetContainer,
        //    int InvoiceId = 0)
        //{
        //    //AppraisementId ----> ContainerStuffingDtlId

        //    string XMLText = "";
        //    if (lstPaySheetContainer != null)
        //    {
        //        XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
        //    }

        //    CHN_ExportRepository objPpgRepo = new CHN_ExportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    objPpgRepo.GetExportDestuffingPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId);
        //    var Output = (PpgInvoiceExpDestuf)objPpgRepo.DBResponse.Data;

        //    Output.InvoiceDate = InvoiceDate;
        //    Output.Module = "EXPDestuf";
        //    Output.PayeeId = Output.PartyId;
        //    Output.PayeeName = Output.PartyName;
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
        //            Output.BOEDate += item.BOEDate + ", ";
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
        //            Output.lstPostPaymentCont.Add(new PpgPostPaymentContainerExpDestuf
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


        //        Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
        //        Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
        //        Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Taxable);
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
        //        Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
        //        Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
        //    });



        //    return Json(Output, JsonRequestBehavior.AllowGet);
        //}





        //[HttpPost]
        //[CustomValidateAntiForgeryToken]
        //public JsonResult AddEditExportDestuffing(PpgInvoiceExpDestuf objForm)
        //{
        //    try
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"]);

        //        var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
        //        string ContainerXML = "";
        //        string ChargesXML = "";
        //        string ContWiseCharg = "";
        //        string OperationCfsCodeWiseAmtXML = "";
        //        string CargoXML = "";
        //        foreach (var item in invoiceData.lstPostPaymentCont)
        //        {
        //            item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
        //            item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //            item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
        //            item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
        //        }

        //        if (invoiceData.lstPostPaymentCont != null)
        //        {
        //            ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
        //        }
        //        if (invoiceData.lstPostPaymentChrg != null)
        //        {
        //            ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
        //        }
        //        if (invoiceData.lstContWiseAmount != null)
        //        {
        //            ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
        //        }
        //        if (invoiceData.lstOperationCFSCodeWiseAmount != null)
        //        {
        //            OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
        //        }
        //        //if (invoiceData.lstPreInvoiceCargo != null)
        //        //{
        //        //    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
        //        //}
        //        CHN_ExportRepository objChargeMaster = new CHN_ExportRepository();
        //        objChargeMaster.AddEditExpDestufInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId,
        //            ((Login)(Session["LoginUser"])).Uid, "EXPDestuf", CargoXML);

        //        invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data).Split(',')[0];
        //        invoiceData.ExportDestuffingNo = Convert.ToString(objChargeMaster.DBResponse.Data).Split(',')[1];

        //        objChargeMaster.DBResponse.Data = invoiceData;
        //        return Json(objChargeMaster.DBResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        var Err = new { Status = -1, Message = "Error" };
        //        return Json(Err);
        //    }
        //}

        //#endregion

        #region Cargo Shifting
        public ActionResult CreateCargoShifting()
        {
            //--------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //-------------------------------------------------------------------------------
           
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();           
            GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();
            Dnd_CargoShiftingShipBillDetails objCR = new Dnd_CargoShiftingShipBillDetails();

            objExport.GetAllAppNoCargoShifting();
            if (objExport.DBResponse.Data != null)
                objCR.lstAppNo = (List<Dnd_ApplicationNoDet>)objExport.DBResponse.Data;

            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownList = lstGodown;

            //objExport.getOnlyRightsGodown();
            //List<Godown> lstGodownF = new List<Godown>();
            //if (objExport.DBResponse.Data != null)
            //{
            //    lstGodownF = (List<Godown>)objExport.DBResponse.Data;
            //}
            //ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);
            //-------------------------------------------------------------------------------

            return PartialView(objCR);
        }

        public JsonResult GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetShipBillDetails(ShippingLineId, ShiftingType, GodownId);
            if (objER.DBResponse.Status > 0)
            {
                return Json(objER.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddEditCargoShifting(Dnd_CargoShiftingShipBillDetails objForm)
        {
            try
            {
                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditCargoShift(objForm,((Login)(Session["LoginUser"])).Uid);

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        public JsonResult GetApplicationDetForCargoShifting(int CartingAppId)
        {
            Dnd_CargoShiftingShipBillDetails objCR = new Dnd_CargoShiftingShipBillDetails();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAppDetForCargoShifting(CartingAppId);
            if (objER.DBResponse.Data != null)
                objCR = (Dnd_CargoShiftingShipBillDetails)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListShiftRegister()
        {
            List<Dnd_CargoShiftingShipBillDetails> lstcart = new List<Dnd_CargoShiftingShipBillDetails>();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAllShiftingForPage(0);
            if (objER.DBResponse.Data != null)
                lstcart = (List<Dnd_CargoShiftingShipBillDetails>)objER.DBResponse.Data;

            return PartialView("ListOfShiftingRegister",lstcart);
        }

        [HttpGet]
        public JsonResult LoadMoreShiftingList(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CargoShiftingShipBillDetails> LstJO = new List<Dnd_CargoShiftingShipBillDetails>();
            ObjCR.GetAllShiftingForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CargoShiftingShipBillDetails>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfShiftingSearch(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CargoShiftingShipBillDetails> lstCart = new List<Dnd_CargoShiftingShipBillDetails>();
            objER.GetAllShiftEntryForSearch(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCart = (List<Dnd_CargoShiftingShipBillDetails>)objER.DBResponse.Data;
            return PartialView("ListOfShiftingRegister", lstCart);
        }
        #endregion

        #region Tentative Invoice(Container Movement)
        [HttpGet]
        public ActionResult CreateTentativeContainerMovement()
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }



            ObjIR.GetContainerForMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LstContainerNo = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "ContainerStuffingId", "Container");
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }
            ObjIR.GetShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjIR.GetPaymentParty();
            if (ObjIR.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            //ObjIR.ListOfGodown();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            //}
            //else
            //{
            //    ViewBag.ListOfGodown = null;
            //}


            ObjIR.GetLocationForInternalMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LocationNoList = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
            }
            else
            {
                ViewBag.LocationNoList = null;
            }
            return PartialView();
        }





        [NonAction]
        public string GeneratingTentativePDFforContainerMovement(PPG_MovementInvoice invoiceDataobj)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            //   List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            //   List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            var FileName = "";
            if (invoiceDataobj.AllTotal > 0)
            {
                List<string> lstSB = new List<string>();
                StringBuilder html = new StringBuilder();

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + invoiceDataobj.CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("<br />");
                html.Append("</td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + invoiceDataobj.CompanyGstNo + "</label></span></td></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span></span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + invoiceDataobj.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + invoiceDataobj.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + invoiceDataobj.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + invoiceDataobj.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + invoiceDataobj.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + invoiceDataobj.PartyGST + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + invoiceDataobj.RequestNo + "</b> ");
                html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;

                foreach (PpgPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.GrossWeight.ToString("0.000") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (obj.CargoType == 0 ? "" : (obj.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                }

                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + invoiceDataobj.ShippingLineName + " </td></tr>");
                html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
                html.Append("<td style='font-size: 12px;'>OBL No :  </td>");
                html.Append("<td style='font-size: 12px;'>Item No. : </td>");
                html.Append("<td style='font-size: 12px;'>BOE No : " + invoiceDataobj.BOENo + " </td>");
                html.Append("<td style='font-size: 12px;'>BOE Date : " + invoiceDataobj.BOEDate + " </td>");
                html.Append("</tr>");
                html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + invoiceDataobj.ImporterExporter + " </td>");
                html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + invoiceDataobj.TotalValueOfCargo.ToString("0.00") + " </td></tr>");
                html.Append("<tr><td style='font-size: 12px;' colspan='5'>CHA Name : " + invoiceDataobj.CHAName + " </td></tr>");
                html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + invoiceDataobj.TotalNoOfPackages.ToString() + " </td>");
                html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + invoiceDataobj.TotalGrossWt.ToString("0.000") + " </td>");
                html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) :  </td>");
                html.Append("<td></td>");
                html.Append("<td></td>");
                html.Append("</tr>");
                html.Append("</table>");
                html.Append("</td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                foreach (PpgPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + obj.ChargeType + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + obj.ChargeName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + obj.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.Taxable.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + obj.Taxable.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + obj.Total.ToString("0") + "</td></tr>");
                    i = i + 1;
                }
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + invoiceDataobj.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + invoiceDataobj.InvoiceAmt.ToString("0") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalIGST.ToString("0") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(invoiceDataobj.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
                html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
                html.Append("<label style='font-weight: bold;'></label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                /***************/

                lstSB.Add(html.ToString());
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }





        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintTentativeMovementInvoice(string ctype)
        {

            int BranchId = Convert.ToInt32(Session["BranchId"]);
            var invoiceData = PPGTentativeInvoice.InvoiceObjW;

            if (ctype == "W")
                invoiceData = PPGTentativeInvoice.InvoiceObjW;
            if (ctype == "GR")
                invoiceData = PPGTentativeInvoice.InvoiceObjGR;
            if (ctype == "FMC")
                invoiceData = PPGTentativeInvoice.InvoiceObjFMC;


            string Path = GeneratingTentativePDFforContainerMovement(invoiceData);
            return Json(new { Status = 1, Message = Path });


        }
        #endregion

        #region Export Container RR Credit Debit
        [HttpGet]
        public ActionResult CreateRRCreditDebitPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            CHN_ExportRepository objExport = new CHN_ExportRepository();
            objExport.GetInvoiceNoForCreditDebitRR();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            /*objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;*/

            objExport.GetShippingLine();
            if (objExport.DBResponse.Data != null)
                ViewBag.ShippingLine = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ShippingLine = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSelectedInvoiceDetails(int InvoiceId)
        {
            CHN_ExportRepository objExport = new CHN_ExportRepository();
            objExport.GetContainerDetForExpRR(InvoiceId);
            PpgRRCreditDebitInvoiceDetails result = new PpgRRCreditDebitInvoiceDetails();
            if (objExport.DBResponse.Status > 0)
                result = (PpgRRCreditDebitInvoiceDetails)objExport.DBResponse.Data;
            else
                result = null;
            return Json(new { Data = result, Status = (result == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExportRRPaymentSheet(int InvoiceId, int ShippingLineId)
        {
            CHN_ExportRepository objChrgRepo = new CHN_ExportRepository();
            objChrgRepo.GetEXPRRCDSheetInvoice(InvoiceId, ShippingLineId);
            int Status = 0;
            PpgInvoiceRRCreditDebit Output = new PpgInvoiceRRCreditDebit();
            if (objChrgRepo.DBResponse.Data != null)
            {
                Output = (PpgInvoiceRRCreditDebit)objChrgRepo.DBResponse.Data;
                Status = 1;
            }
            else Output = null;
            Output.Module = "EXPRRCD";

            Output.TotalNoOfPackages = Output.lstPostPaymentContRRCD.Sum(o => o.NoOfPackages);
            Output.TotalGrossWt = Output.lstPostPaymentContRRCD.Sum(o => o.GrossWt);
            Output.TotalWtPerUnit = Output.lstPostPaymentContRRCD.Sum(o => o.WtPerUnit);
            Output.TotalSpaceOccupied = Output.lstPostPaymentContRRCD.Sum(o => o.SpaceOccupied);
            Output.TotalSpaceOccupiedUnit = Output.lstPostPaymentContRRCD.FirstOrDefault().SpaceOccupiedUnit;
            Output.TotalValueOfCargo = Output.lstPostPaymentContRRCD.Sum(o => o.CIFValue)
                + Output.lstPostPaymentContRRCD.Sum(o => o.Duty);


            Output.TotalAmt = Output.lstPostPaymentChrgRRCD.Sum(o => o.Total);
            Output.TotalDiscount = Output.lstPostPaymentChrgRRCD.Sum(o => o.Discount);
            Output.TotalTaxable = Output.lstPostPaymentChrgRRCD.Sum(o => o.Total);
            Output.TotalCGST = Output.lstPostPaymentChrgRRCD.Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstPostPaymentChrgRRCD.Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstPostPaymentChrgRRCD.Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstPostPaymentChrgRRCD.Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstPostPaymentChrgRRCD.Sum(o => o.Total);
            Output.RoundUp = 0;
            Output.InvoiceAmt = Output.lstPostPaymentChrgRRCD.Sum(o => o.Total);


            return Json(new { Data = Output, Status = Status }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditExportRRCreditDebitModule(PpgInvoiceRRCreditDebit objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = objForm;
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string CargoXML = "";
                foreach (var item in invoiceData.lstPostPaymentContRRCD)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate.Split(' ')[0];
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstPostPaymentContRRCD != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentContRRCD);
                }
                if (invoiceData.lstPostPaymentChrgRRCD != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgRRCD);
                }
                CHN_ExportRepository objChargeMaster = new CHN_ExportRepository();
                objChargeMaster.AddEditExportRRCreditDebitModule(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, objForm.Module, CargoXML);
                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }



        /*
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult LoadContInvoicePrint(string InvoiceNo)
        {
            CHN_ExportRepository objGPR = new CHN_ExportRepository();
            objGPR.GetInvoiceDetailsForContLoadedPrintByNo(InvoiceNo, "EXPLod");
            PPG_MovementInvoice objGP = new PPG_MovementInvoice();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {


                objGP = (PPG_MovementInvoice)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceContLoaded(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceContLoaded(PPG_MovementInvoice objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/ContLoadedInvoice" + InvoiceId.ToString() + ".pdf";
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
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
            html.Append("<br />Container Loaded Invoice");
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
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
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
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;''>Description</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
            html.Append("<tbody>");
            i = 1;
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }

            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");

            html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; width: 100px;'>Total :</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalCGST.ToString("0.00") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalSGST.ToString("0.00") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + objGP.TotalIGST.ToString("0.00") + "</th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/ContLoadedInvoice" + InvoiceId.ToString() + ".pdf";
        }*/
        #endregion

        #region Export Train Summary Upload

        [HttpGet]
        public ActionResult TrainSummaryUpload()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            //PortRepository objImport = new PortRepository();
            //objImport.GetAllPort();
            //if (objImport.DBResponse.Status > 0)
            //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckUpload(string TrainNo)
        {
            int status = 0;
            List<ppgExportTrainSummaryUpload> TrainSummaryUploadList = new List<ppgExportTrainSummaryUpload>();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase File = Request.Files[0];
                string extension = Path.GetExtension(File.FileName);
                if (extension == ".xls" || extension == ".xlsx")
                {
                    DataTable dt = Utility.GetExcelData(File);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["CTRNO"])))
                                    {
                                        ppgExportTrainSummaryUpload objTrainSummaryUpload = new ppgExportTrainSummaryUpload();
                                        objTrainSummaryUpload.Wagon = Convert.ToString(dr["WAGNNO"]);
                                        objTrainSummaryUpload.ContainerNo = Convert.ToString(dr["CTRNO"]);
                                        objTrainSummaryUpload.SZ = Convert.ToString(dr["SZ"]);
                                        objTrainSummaryUpload.Status = Convert.ToString(dr["ST"]);
                                        objTrainSummaryUpload.SLine = Convert.ToString(dr["Onbehalfof"]);
                                        objTrainSummaryUpload.TW = Convert.ToDecimal(dr["TWT"]);
                                        objTrainSummaryUpload.CW = Convert.ToDecimal(dr["CRGWT"]);
                                        objTrainSummaryUpload.GW = Convert.ToDecimal(dr["TOTALWT"]);
                                        // objTrainSummaryUpload.PKGS = Convert.ToInt32(dr["PKGS"]);
                                        objTrainSummaryUpload.Commodity = Convert.ToString(dr["COMODITY"]);
                                        objTrainSummaryUpload.LineSeal = Convert.ToString(dr["LINESEAL"]);
                                        objTrainSummaryUpload.CustomSeal = Convert.ToString(dr["CUSSEAL"]);
                                        objTrainSummaryUpload.Shipper = Convert.ToString(dr["MAINL"]);
                                        // objTrainSummaryUpload.CHA = Convert.ToString(dr["CHA"]);
                                        //  objTrainSummaryUpload.CRRSBillingParty = Convert.ToString(dr["CRRSBillingParty"]);
                                        //  objTrainSummaryUpload.BillingParty = Convert.ToString(dr["BillingParty"]);
                                        //  objTrainSummaryUpload.StuffingMode = Convert.ToString(dr["StuffingMode"]);
                                        //   objTrainSummaryUpload.SBillNo = Convert.ToString(dr["SBillNo"]);
                                        //   objTrainSummaryUpload.Date = Convert.ToDateTime(dr["Date"]);

                                        objTrainSummaryUpload.Origin = Convert.ToString(dr["ORIGIN"]);
                                        objTrainSummaryUpload.POL = Convert.ToString(dr["LOADPORT"]);
                                        objTrainSummaryUpload.POD = dr.ItemArray[13].ToString();
                                        //  objTrainSummaryUpload.DepDate = Convert.ToDateTime(dr["DepDate"]);
                                        TrainSummaryUploadList.Add(objTrainSummaryUpload);
                                    }
                                    else
                                    {
                                        TrainSummaryUploadList = null;
                                        status = -5;
                                        return Json(new { Status = status, Data = TrainSummaryUploadList }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                                string TrainSummaryUploadXML = Utility.CreateXML(TrainSummaryUploadList);

                                CHN_ExportRepository objImport = new CHN_ExportRepository();
                                objImport.CheckTrainSummaryUpload(TrainNo, TrainSummaryUploadXML);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = (List<ppgExportTrainSummaryUpload>)objImport.DBResponse.Data;
                                }
                                else
                                {
                                    status = -6;

                                }
                            }
                            catch (Exception ex)
                            {
                                status = -2;
                            }
                        }
                        else
                        {
                            status = -1;
                        }
                    }
                    else
                    {
                        status = -6;
                    }
                }
                else
                {
                    status = -3;
                }

            }
            else
            {
                status = -4;
            }

            if (status < 0)
            {
                TrainSummaryUploadList = null;
            }

            return Json(new { Status = status, Data = TrainSummaryUploadList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveUploadData(ppgExportTrainSummaryUpload objTrainSummaryUpload)
        {
            int data = 0;
            if (objTrainSummaryUpload.TrainSummaryList != null)
                objTrainSummaryUpload.TrainSummaryUploadList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ppgExportTrainSummaryUpload>>(objTrainSummaryUpload.TrainSummaryList);
            if (objTrainSummaryUpload.TrainSummaryUploadList.Count > 0)
            {
                string TrainSummaryUploadXML = Utility.CreateXML(objTrainSummaryUpload.TrainSummaryUploadList);
                CHN_ExportRepository objImport = new CHN_ExportRepository();
                objImport.AddUpdateTrainSummaryUpload(objTrainSummaryUpload, TrainSummaryUploadXML);
                if (objImport.DBResponse.Status > 0)
                {
                    data = Convert.ToInt32(objImport.DBResponse.Data);
                }
                else
                {
                    data = 0;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfTrainSummary()
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
            List<ppgExportTrainSummaryUpload> lstCargoSeize = new List<ppgExportTrainSummaryUpload>();
            objER.ListOfTrainSummary();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<ppgExportTrainSummaryUpload>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            CHN_ExportRepository objImport = new CHN_ExportRepository();
            objImport.GetTrainSummaryDetails(TrainSummaryUploadId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult CheckUpload(TrainSummaryUpload trainSummaryUpload)
        //{

        //    DateTime dtime = Utility.StringToDateConversion(trainSummaryUpload.TrainDate, "dd/MM/yyyy hh:mm");
        //    string TrainDate = dtime.ToString("yyyy-MM-dd") + " " + trainSummaryUpload.TrainDate.Split(' ')[1] + ":00";
        //    trainSummaryUpload.TrainDate = TrainDate;

        //    int data = 0;

        //    if (dtime > DateTime.Now)
        //    {
        //        data = -1;
        //    }
        //    else
        //    {
        //        Ppg_ImportRepository objImport = new Ppg_ImportRepository();
        //        objImport.AddUpdateTrainSummaryUpload(trainSummaryUpload, "CHECK");
        //        if (objImport.DBResponse.Status > 0)
        //        {
        //            data = Convert.ToInt32(objImport.DBResponse.Data);
        //            trainSummaryUpload.TrainSummaryUploadId = data;
        //        }
        //        else
        //        {
        //            data = -2;
        //        }
        //    }

        //    Session["trainSummaryUpload"] = trainSummaryUpload;

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult UploadData()
        //{
        //    TrainSummaryUpload trainSummaryUpload = (TrainSummaryUpload)Session["trainSummaryUpload"];
        //    int data = 0;

        //    if (Request.Files.Count > 0)
        //    {
        //        HttpPostedFileBase File = Request.Files[0];
        //        string extension = Path.GetExtension(File.FileName);
        //        if (extension == ".xls" || extension == ".xlsx")
        //        {
        //            DataTable dt = Utility.GetExcelData(File);
        //            if (dt != null)
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();
        //                        foreach (DataRow dr in dt.Rows)
        //                        {
        //                            TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
        //                            objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
        //                            objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
        //                            objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
        //                            objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
        //                            objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
        //                            objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
        //                            objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
        //                            objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Ct_Tare"]);
        //                            objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
        //                            objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
        //                            objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
        //                            objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
        //                            objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
        //                            objTrainSummaryUpload.Genhaz = String.IsNullOrWhiteSpace(Convert.ToString(dr["Genhaz"])) ? "GEN" : Convert.ToString(dr["Genhaz"]);

        //                            TrainSummaryUploadList.Add(objTrainSummaryUpload);
        //                        }

        //                        string TrainSummaryUploadXML = Utility.CreateXML(TrainSummaryUploadList);

        //                        Ppg_ImportRepository objImport = new Ppg_ImportRepository();
        //                        objImport.AddUpdateTrainSummaryUpload(trainSummaryUpload, "SAVE", TrainSummaryUploadXML);
        //                        if (objImport.DBResponse.Status > 0)
        //                        {
        //                            data = Convert.ToInt32(objImport.DBResponse.Data);
        //                        }
        //                        else
        //                        {
        //                            data = 0;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        data = -2;
        //                    }
        //                }
        //                else
        //                {
        //                    data = -1;
        //                }
        //            }
        //            else
        //            {
        //                data = 0;
        //            }
        //        }
        //        else
        //        {
        //            data = -3;
        //        }

        //    }
        //    else
        //    {
        //        data = -4;
        //    }

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Job Order by Train
        [HttpGet]

        public ActionResult CreateJobOrder()
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }

            //objIR.GetAllTrainNo();
            //if (objIR.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfTrain = objIR.DBResponse.Data;
            //    ViewBag.ListOfTrainPick = JsonConvert.SerializeObject(objIR.DBResponse.Data);

            //}

            //   objIR.GetAllTrainNoforReset();
            //   if (objIR.DBResponse.Data != null)
            // {
            //      ViewBag.ListOfTrainReset = objIR.DBResponse.Data;
            //   ViewBag.ListOfTrainPick = JsonConvert.SerializeObject(objIR.DBResponse.Data);

            //   }


            // objIR.ListOfCHA();
            //  if (objIR.DBResponse.Data != null)
            //      ViewBag.ListOfCHA = objIR.DBResponse.Data;
            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            PPGExportJobOrder objJO = new PPGExportJobOrder();
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstpickup = (List<PPGPickupModel>)objIR.DBResponse.Data;
            }

            //objIR.GetAllPortForJobOrderTrasport();
            //if (objIR.DBResponse.Data != null)
            //{
            //    objJO.lstPort = (List<TransformList>)objIR.DBResponse.Data;
            //}

            // objJO.JobOrderDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yy hh:mm"));
            objJO.JobOrderDate = DateTime.Now;
            // objJO.JobOrderDate =Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy hh:mm")) ;
            objJO.TrainDate = DateTime.Now;
            return PartialView(objJO);
        }

        [HttpGet]
        public ActionResult GetAllTrainNo()
        {
            CHN_ExportRepository ObjIR = new CHN_ExportRepository();
            ObjIR.GetAllTrainNo();
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            CHN_ExportRepository objRepo = new CHN_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            CHN_ExportRepository objRepo = new CHN_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            IList<PPG_ImportJobOrderList> lstIJO = new List<PPG_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderDetails(string ContainerNo)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            IList<PPG_ImportJobOrderList> lstIJO = new List<PPG_ImportJobOrderList>();
            objIR.GetAllImpJO(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderDetails", lstIJO);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            CHN_ExportRepository ObjCR = new CHN_ExportRepository();
            List<PPG_ImportJobOrderList> LstJO = new List<PPG_ImportJobOrderList>();
            ObjCR.GetAllImpJO(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<PPG_ImportJobOrderList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPort()
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetAllPortForJobOrderTrasport();
            PPGExportJobOrder objJO = new PPGExportJobOrder();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstPort = (List<TransformList>)objIR.DBResponse.Data;
            }
            return Json(objJO.lstPort, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            PPGExportJobOrder objImp = new PPGExportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (PPGExportJobOrder)objIR.DBResponse.Data;
            ViewBag.jdate = objImp.JobOrderDate;
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<PPGPickupModel>)objIR.DBResponse.Data;
            }
            //objIR.ListOfShippingLine();
            //if (objIR.DBResponse.Data != null)
            //    ViewBag.ListOfShippingLine = objIR.DBResponse.Data;

            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }

            objIR.GetAllTrainNo();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfTrain = objIR.DBResponse.Data;
            objIR.GetAllPortForJobOrderTrasport();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstPort = (List<TransformList>)objIR.DBResponse.Data;
            }

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int ImpJobOrderId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            PPGExportJobOrder objImp = new PPGExportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (PPGExportJobOrder)objIR.DBResponse.Data;

            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<PPGPickupModel>)objIR.DBResponse.Data;
            }
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            objIR.GetAllPortForJobOrderTrasport();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstPort = (List<TransformList>)objIR.DBResponse.Data;
            }
            return PartialView(objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(PPGExportJobOrder objImp, String FormOneDetails)
        {

            List<PPG_TrainDtl> lstDtl = new List<PPG_TrainDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_TrainDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
                //if (FormOneDetails.Count > 0)
                //    XML = Utility.CreateXML(FormOneDetails);
            }

            objIR.AddEditImpJO(objImp, XML);
            return Json(objIR.DBResponse);


        }
        [HttpGet]
        public JsonResult GetTrainDetl(int TrainSumId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetTrainDtl(TrainSumId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetTrainDetailsOnEditMode(int ImpJobOrderId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetTrainDetailsOnEditMode(ImpJobOrderId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.GetYardWiseLocation(YardId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Job Order Print
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJO(int ImpJobOrderId)
        {
            CHN_ExportRepository objIR = new CHN_ExportRepository();
            objIR.GetImportJODetailsFrPrint(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                PPGPrintJOModel objMdl = new PPGPrintJOModel();
                objMdl = (PPGPrintJOModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, ImpJobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForJO(PPGPrintJOModel objMdl, int ImpJobOrderId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", TrainNo = "", TrainDate = "", ContainerLoadType = "", CargoType = ""; int Count = 0;
            int Count40 = 0;
            int Count20 = 0;
            string Sline = "";
            string Html = "";
            string CompanyAddress = "";
            StringBuilder Pages = new StringBuilder();
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
                Sline = (Sline == "" ? (item.Sline) : (Sline + "<br/>" + item.Sline));
                ContainerLoadType = (ContainerLoadType == "" ? (item.ContainerLoadType) : (ContainerLoadType + "<br/>" + item.ContainerLoadType));
                CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));
                Serial = (Serial == "") ? (++Count).ToString() : (Serial + "<br/>" + (++Count).ToString());
            });

            Count20 = objMdl.lstDet.ToList().Where(item => item.ContainerSize == "20").Count();
            Count40 = objMdl.lstDet.ToList().Where(item => item.ContainerSize == "40").Count();

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }

            String Type = "Road";
            if ((Convert.ToInt32(Session["BranchId"])) == 1)
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else if (Type.Equals("Train"))
            {
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'>Job Order For MOVEMENT OF Export LDD CONTAINERS</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("<tr><td colspan='12'><br/></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%' style='font-size:11px;'><b>TO :</b> <br/> M/s SFA--L</td> <td colspan='6' width='50%' style='font-size:11px; text-align: right;'><b>DATE :</b>Dynamic</td></tr>");
                Pages.Append("<tr><td colspan='12'><br/><br/></td></tr>");
                Pages.Append("<tr><th colspan='12' style='font-size:12px;'>Please arrange movement of following export Loaded/Empty Containerrs from Dynamic to Dynamic</th></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:0; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>S.N</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>ICD Code</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Container No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Size</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>S/line</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>On behalf of</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Cust Seal</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Sla Seal</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>POL</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>POD</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Tr wt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:0;width:8%;'>Crg wt</th></tr></thead>");

                Pages.Append("<tbody>");
                Pages.Append("<tr><td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:0;width:8%;'>Dynamic</td></tr>");
                i++;
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:90%; border-collapse:collapse; text-align:center; margin-top:20px; font-size:8pt;'>");
                Pages.Append("<thead><tr><th colspan='12' style='text-align:left;'>SUMMARY REPORT</th></tr>");
                Pages.Append("<tr><th style='border:1px solid #000;border-right:0;width:10%;'></th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>20</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>40</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>TOTAL</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:1px solid #000;width:10%;'>TEUS</th></tr></thead>");
                Pages.Append("<tbody>");

                Pages.Append("<tr><th style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>JNPT</th>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>GTIL</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>NSICT</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>SUB TOTAL</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>MND</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>PBR</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>TOTAL</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; text-align:center; margin-top:20px; font-size:8pt;'><tbody>");
                Pages.Append("<tr><td width='10%' valign='top' align='right'>REMARKS :</td><td colspan='2' width='85%' style='text-align: left;'>H & T Contracter is required to transport the Export Loaded Containers from ICD-PPG to ICD-LONI with in 12hrs i.e from 22:00 p.m to 10:00 a.m</td></tr>");
                Pages.Append("<tr><td width='10%' valign='top' align='right'>COPY TO :</td><td colspan='2' width='85%' style='text-align: left;'>1. Suman Forwarding Agency Pvt Ltd. - ICD Ppg <br/> 2. SAM/AM - A/C ICD Ppg <br/>  3. Anil William Thomas Security Agency. - ICD Ppg <br/>  4. Manager ICD LONI (In Duplicate)</td></tr>");
                Pages.Append("</tbody></table>");
            }
            else
            {
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'>Job Order For MOVEMENT OF Export LDD CONTAINERS</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("<tr><td colspan='12'><br/></td></tr>");
                Pages.Append("<tr><td colspan='6' width='50%' style='font-size:11px;'><b>TO :</b> <br/> M/s VOLVO-PVT</td> <td colspan='6' width='50%' style='font-size:11px; text-align: right;'><b>DATE :</b>Dynamic</td></tr>");
                Pages.Append("<tr><td colspan='12'><br/><br/></td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:0; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>S.N</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>ICD Code</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Container No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Size</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>S/line</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>On behalf of</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Cust Seal</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Sla Seal</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>POL</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>POD</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Tr wt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:0;width:8%;'>Crg wt</th></tr></thead>");

                Pages.Append("<tbody>");
                Pages.Append("<tr><td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Dynamic</td>");
                Pages.Append("<td style='border-bottom:1px solid #000;border-right:0;width:8%;'>Dynamic</td></tr>");
                i++;
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:90%; border-collapse:collapse; text-align:center; margin-top:20px; font-size:8pt;'>");
                Pages.Append("<thead><tr><th colspan='12' style='text-align:left; font-size:10pt;'>SUMMARY REPORT</th></tr>");
                Pages.Append("<tr><th style='border:1px solid #000;border-right:0;width:10%;'></th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>20</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>40</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:0;width:10%;'>TOTAL</th>");
                Pages.Append("<th style='border:1px solid #000;border-right:1px solid #000;width:10%;'>TEUS</th></tr></thead>");
                Pages.Append("<tbody>");

                Pages.Append("<tr><th style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>MND</th>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-right:0;border-top:0;width:10%;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;'>Dynamic</td></tr>");

                Pages.Append("<tr><th style='border:1px solid #000;border-top:0;border-right:0;width:10%;font-size:9pt;'>TOTAL</th>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;font-size:9pt;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;font-size:9pt;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:0;width:10%;font-size:9pt;'>Dynamic</td>");
                Pages.Append("<td style='border:1px solid #000;border-top:0;border-right:1px solid #000;width:10%;font-size:9pt;'>Dynamic</td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; text-align:center; margin-top:20px; font-size:8pt;'><tbody>");
                Pages.Append("<tr><td width='10%' valign='top' align='right'>COPY TO :</td><td colspan='2' width='85%' style='text-align: left;'>1. Suman Forwarding Agency Pvt Ltd. - ICD Ppg <br/> 2. SAM/AM - A/C ICD Ppg <br/>  3. Anil William Thomas Security Agency. - ICD Ppg</td></tr>");
                Pages.Append("</tbody></table>");
            }

            //Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>  <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>IMPORT JOB ORDER</span></th></tr></tbody></table></td></tr></thead>   <tbody> <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td style='text-align:left; width:50%;'>Job Order No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;width:50%;'>Job Order Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr></tbody></table></td></tr>   <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th>TO,</th></tr> <tr><th>The Manager(CT/OPERATIONS)</th></tr> <tr><td><span><br/></span></td></tr> <tr><td>" + objMdl.FromLocation + "</td></tr> <tr><td><span>&nbsp;&nbsp;</span>SIR,</td></tr> <tr><td><span>&nbsp;&nbsp;</span>YOU ARE REQUESTED TO KINDLY ARRANGE TO DELIVER THE FOLLOWING IMPORT CONTAINERS / CBT TO ICD PATPARGANJ, DELHI.</td></tr> </tbody></table></td></tr>      <tr><td colspan='12' style='text-align:center;'><br/></td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center; width:25px;'>SL.NO</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>CONTAINER / CBT NO.</th><th style='border:1px solid #000;padding:5px;text-align:center; width:60px;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>G/HAZ</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>SLA CODE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Train No</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Train DATE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Origin</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>F/L</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + CargoType + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.TrainNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.TrainDate + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.FromLocation + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerLoadType + "</td></tr></tbody> </table></td></tr> <tr><td><span><br/></span></td></tr> <tr><td colspan='1'></td><th colspan='10'>TOTAL CONTAINERS / CBT : 20x " + Count20 + " + 40x " + Count40 + "</th></tr> <tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td colspan='2'></td><td width='15%' valign='top' align='right'>Note :</td><td colspan='2' width='85%'>THE FOLLOWING CONTAINERS / CBT ARE REQUIRED TO BE SCANNED BEFORE ITS DELIVERY FROM THE PORT AS DESIRED BY THE CUSTOM SCANNING DIVISION</td></tr></tbody></table></td></tr>   <tr><td colspan='12'><span><br/><br/></span></td></tr> <tr><td colspan='12' style='text-align:right;'>FOR MANAGER <br/> ICD PATPARGANJ</td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody> <tr><td><span><br/></span></td></tr>   <tr><td colspan='12'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to <br/> 1- M/S Suman Forwarding Agency Pvt.Ltd - for arranging movement of the Containers / CBT from " + objMdl.FromLocation + " within time. failing which dwell time charges as per procedure will be debited to your account as per claim receive from line.<br/> 2-The Preventive Office, Customs,ICD Patparganj.</td></tr></tbody></table></td></tr>    </tbody></table></td></tr></tbody></table>";

            // string Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>CONTAINER FREIGHT STATION<br/>18, COAL DOCK ROAD, KOLKATA - 700 043</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderNo+"</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderDate+"</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> "+objMdl.FromLocation+" </span> to<span style='border-bottom:1px solid #000;'> "+objMdl.ToLocation+" </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>"+ Serial + "</td><td style='border:1px solid #000;padding:5px;'>"+ContainerNo+"</td><td style='border:1px solid #000;padding:5px;'>"+Size+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ShippingLineName+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ContainerType+"</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.GeneratePDF(Path, Pages.ToString());
            }
            return "/Docs/" + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
        }
        #endregion
        #endregion

        #region Export Payment Sheet
        [HttpGet]
        public ActionResult CreateExportPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            Dnd_ExportRepository objExp = new Dnd_ExportRepository();
            objExp.GetContStuffingForPaymentSheet();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objExp.GetPaymentParty();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            Dnd_ExportRepository objImport = new Dnd_ExportRepository();
            objImport.GetContDetForPaymentSheet(AppraisementId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
        //    int PartyId, int PayeeId, int IsLock, int IsReefer , string PlugInDateTime, string PlugOutDateTime, int InvoiceId = 0)
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
            int PartyId, int PayeeId, int IsLock,int IsCompositeTariff, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            Dnd_ExportRepository objPpgRepo = new Dnd_ExportRepository();
            //objPpgRepo.GetExportPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId,IsLock, IsReefer, PlugInDateTime, PlugOutDateTime);
            objPpgRepo.GetExportPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId, IsLock, IsCompositeTariff);
            var Output = (DND_ExpPaymentSheet)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXP";

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
                obj.IsODC = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.ODC);
                obj.PayMode= Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().PayMode;
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
                    item.StuffingDate = Convert.ToDateTime(item.StuffingDate).ToString("yyyy-MM-dd");
                    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
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

                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditExpInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXP");

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
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<DNDListOfExpInvoice> obj = new List<DNDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DNDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion

        #region seal checking Payment Sheet
        public ActionResult ListOfExpInvoiceSealChecking(string InvoiceNo = null, string InvoiceDate = null)
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
            objER.ListOfExpInvoice("EXPSEALCHEKING", InvoiceNo, InvoiceDate);
            List<CHNListOfExpInvoice> obj = new List<CHNListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CHNListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion


        #region Ship Bill Amendment

        public ActionResult Amendment()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAmenSBList();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNoAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfSBNoAmendment = null;
            }
            //objER.ListOfExporter();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfExporterForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfExporterForAmendment = null;
            //}

            //objER.ListOfCha();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfChaForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfChaForAmendment = null;
            //}
            //objER.ListOfShi();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfShiForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfShiForAmendment = null;
            //}
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.ListOfShippingLinePartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            objIR.ListOfChaForPage("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            //objER.ListOfPOD();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfPODForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfPODForAmendment = null;
            //}
            //objER.GetAllChaForPageAmendment("", 0);
            //if (objER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.State = Jobject["State"];
            //}
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            objER.GetInvoiceListForShipbillAmend();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfInv = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfInv = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSBDetailsBySBNo(string SBid, string SbDate)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.GetAmenSBDetailsBySbNoDate(SBid, SbDate);
            if (obj.DBResponse.Status > 0)
            {
                return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }



        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SaveAmendement(List<OldInfoSb> vm, List<NewInfoSb> newvm, string Date, string AmendmentNO, int InvoiceId, string InvoiceNo, string InvoiceDate, string FlagMerger)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            if (FlagMerger == "Split" && vm.Count > 1)
            {
                return Json(new { Message = "Only One Shipbill Split", Status = 0 }, JsonRequestBehavior.AllowGet);

            }
            else if (FlagMerger == "Merger" && vm.Count == 1)
            {
                return Json(new { Message = "Merge Operation Can't Be Done With Single Shipping Bill", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string OldInfoSbXml = Utility.CreateXML(vm);
                string NewInfoSbSbXml = Utility.CreateXML(newvm);

                obj.AddEditAmendment(OldInfoSbXml, NewInfoSbSbXml, Date, InvoiceId, InvoiceNo, InvoiceDate, FlagMerger);


                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpGet]
        public JsonResult SearchChaByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllChaForPageAmendment(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadChaAmendmentList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.GetAllChaForPageAmendment(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmendDetails(string AmendNo)
        {

            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.GetAmenSBDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmendDetailsByAmendNo(string AmendNo)
        {

            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.GetAmenDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SubmitAmedData(Dnd_AmendmentViewModel vm)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.AddEditShipAmendment(vm);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListofAmendData()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_AmendmentViewModel> lstdata = new List<Dnd_AmendmentViewModel>();
            objER.ListForShipbillAmend();
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Dnd_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }
        [HttpGet]
        public ActionResult ViewAmendData(int id)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            Dnd_AmendmentViewModel obj = new Dnd_AmendmentViewModel();
            objER.GetShipbillAmendDet(id);
            if (objER.DBResponse.Status == 1)
            {
                obj = (Dnd_AmendmentViewModel)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ViewMergeSplitData(string AmendNo)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_Amendment> obj = new List<Dnd_Amendment>();
            objER.GetAmenDetailsByAmendNO(AmendNo);
            if (objER.DBResponse.Status == 1)
            {
                obj = (List<Dnd_Amendment>)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ListofMergeSplitAmendData()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_AmendmentViewModel> lstdata = new List<Dnd_AmendmentViewModel>();
            objER.GetAmenSBDetailsByAmendNO("");
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Dnd_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        public JsonResult GetAllCommodityDetailsForAmendmend(string CommodityName, int Page)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAllCommodityForPageAmendment(CommodityName, Page);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllChaDetailsForAmendmend(string ChaName, int Page)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAllChaForPageAmendment(ChaName, Page);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);

        }


     

        #endregion

        #region Bond Advance Payment Sheet
        public ActionResult ListOfInvoiceBondAdv(string InvoiceNo = null, string InvoiceDate = null)
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
            objER.ListOfExpInvoice("BNDAdv", InvoiceNo, InvoiceDate);
            List<CHNListOfExpInvoice> obj = new List<CHNListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CHNListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion

        #region Bond Delivery Payment Sheet
        public ActionResult ListOfInvoiceBondDelivery(string InvoiceNo = null, string InvoiceDate = null)
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
            objER.ListOfExpInvoice("BND", InvoiceNo, InvoiceDate);
            List<CHNListOfExpInvoice> obj = new List<CHNListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CHNListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public JsonResult LoadShippingLineList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchShippingLineByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadExporterList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfExporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchExporterByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfExporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadForwarderList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfForwarderForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadMainlineList(string PartyCode, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfMainlineForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchForwarderByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfForwarderForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchMainlineByPartyCode(string PartyCode)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfMainlineForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfDischargeList(string PortName, int Page)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfPortOfDischargeForPage(PortName, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PortOfDischargeSearchByPortName(string PortName)
        {
            Dnd_ExportRepository objRepo = new Dnd_ExportRepository();
            objRepo.ListOfPortOfDischargeForPage(PortName, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #region VesselInformation
        public ActionResult CreateVesselInformation()
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                List<Port> lstport = (List<Port>)ObjRR.DBResponse.Data;
                ViewBag.ListOfPort = lstport;

                /*lstport = lstport.Where(m => m.PortName == "ICD PPG").ToList();
                if (lstport.Count > 0)
                {
                    ViewBag.PortName = lstport[0].PortName;
                    ViewBag.PortId = lstport[0].PortId;
                }
                else
                {
                    ViewBag.PortName = "";
                    ViewBag.PortId = 0;
                }*/

            }
           
            Dnd_VesselInf objEntryThroughGate = new Dnd_VesselInf();
            ObjRR.GetTime();
            if (ObjRR.DBResponse.Data != null)
            {
                objEntryThroughGate = (Dnd_VesselInf)ObjRR.DBResponse.Data;
               // string ExitTime = objEntryThroughGate.EntryDateTime;
               // string[] ExitTimeArray = ExitTime.Split(' ');
               // objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
               // string[] str = strTime.Split();
               // string SystemTime = objEntryThroughGate.SystemDateTime;
               // string[] SystemTimeArray = SystemTime.Split(' ');
                //objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }

            return PartialView();
        }
        [HttpGet]
        public ActionResult GetVesselList()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_VesselInf> LstVessel = new List<Dnd_VesselInf>();
            ObjER.GetAllVesselInformation();
            if (ObjER.DBResponse.Data != null)
            {
                LstVessel = (List<Dnd_VesselInf>)ObjER.DBResponse.Data;
            }
            return PartialView(LstVessel);
        }

        [HttpGet]
        public ActionResult GetVesselListSearch(string Search)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_VesselInf> LstVessel = new List<Dnd_VesselInf>();
            ObjER.GetAllVesselInformationSearch(Search);
            if (ObjER.DBResponse.Data != null)
            {
                LstVessel = (List<Dnd_VesselInf>)ObjER.DBResponse.Data;
            }
            return PartialView("GetVesselList",LstVessel);
        }


        [HttpGet]
        public ActionResult ViewVesselInfo(int VesselId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_VesselInf ObjVI = new Dnd_VesselInf();
            if (VesselId > 0)
            {
                ObjER.GetVesselInformation(VesselId);
                if (ObjER.DBResponse.Data != null)
               {
                    ObjVI = (Dnd_VesselInf)ObjER.DBResponse.Data;
               }
            }
            return PartialView(ObjVI);
        }
        [HttpGet]
        public ActionResult EditVesselInfo(int VesselId)
        {
             
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetPortOfLoading();
            if (ObjER.DBResponse.Data != null)
            {
                List<Port> lstport = (List<Port>)ObjER.DBResponse.Data;
                ViewBag.ListOfPort = lstport;


            }
            Dnd_VesselInf objEntryThroughGate = new Dnd_VesselInf();
            ObjER.GetTime();
            if (ObjER.DBResponse.Data != null)
            {
                objEntryThroughGate = (Dnd_VesselInf)ObjER.DBResponse.Data;
               // string ExitTime = objEntryThroughGate.EntryDateTime;
               // string[] ExitTimeArray = ExitTime.Split(' ');
               // objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                //ViewBag.strTime = objEntryThroughGate.Time;
               // string SystemTime = objEntryThroughGate.SystemDateTime;
               // string[] SystemTimeArray = SystemTime.Split(' ');
               // objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            Dnd_VesselInf ObjVI = new Dnd_VesselInf();
            if (VesselId > 0)
            {
                ObjER.GetVesselInformation(VesselId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjVI = (Dnd_VesselInf)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjVI);
        }

        [HttpPost]
        public JsonResult AddEditVesselInformation(Dnd_VesselInf objvi)
        {


            objvi.IPAddress = Request.UserHostAddress;
            TryUpdateModel<Dnd_VesselInf>(objvi);

            Dnd_ExportRepository objIR = new Dnd_ExportRepository();

           

            objIR.AddVesselInformation(objvi);

            

            return Json(objIR.DBResponse);

           
        }

        [HttpPost]

        public ActionResult DeleteVesselInfo(int VesselId)
        {
            if (VesselId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.DeleteVesselInformation(VesselId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }




        #endregion

        #region EIR

        public ActionResult CreateExportEIR()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerForEIR();
            if(ObjER.DBResponse!=null)
            {
                ViewBag.LstCFS = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.LstCFS = null;
            }
            ObjER.GetPortOfLoading();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPort = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.ListOfPort = null;
            }
                return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExportEIR(DndExportEIR objETG)
        {
            if (ModelState.IsValid)
            {
                string ExitTime = objETG.GateExitTime;
                string PortInTime = objETG.PortIntime;
                ExitTime = ExitTime.Replace("PM", " PM").Replace("AM", " AM");
                PortInTime = PortInTime.Replace("PM", " PM").Replace("AM", " AM");

                string strExitDateTime = objETG.GateExitDate + " " + ExitTime;
                string strPortInDateTime = objETG.PortInDate + " " + PortInTime;
                objETG.GateExitDate = strExitDateTime;
                objETG.PortInDate = strPortInDateTime;



                Dnd_ExportRepository objETGR = new Dnd_ExportRepository();

                objETGR.AddEditExportEIR(objETG);

                ModelState.Clear();
                return Json(objETGR.DBResponse);


            }
            else
            {
                string m = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage));
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetAllEIRData()
        {
            Dnd_ExportRepository ETGR = new Dnd_ExportRepository();
            ETGR.GetAllEIRData(0,"");
            List<DndExportEIR> ListEIR = new List<DndExportEIR>();

            if (ETGR.DBResponse.Data != null)
            {
                ListEIR = (List<DndExportEIR>)ETGR.DBResponse.Data;
            }
            return PartialView("ExportEIRList", ListEIR);

        }

        [HttpGet]
        public JsonResult LoadMoreEIRList(int Page)
        {
            Dnd_ExportRepository ETGR = new Dnd_ExportRepository();
            ETGR.GetAllEIRData(Page, "");
            List<DndExportEIR> ListEIR = new List<DndExportEIR>();

            if (ETGR.DBResponse.Data != null)
            {
                ListEIR = (List<DndExportEIR>)ETGR.DBResponse.Data;
            }
            return Json(ListEIR, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult SearchEIRData(string ContNo)
        {
            Dnd_ExportRepository ETGR = new Dnd_ExportRepository();
            ETGR.GetAllEIRData(0, ContNo);
            List<DndExportEIR> ListEIR = new List<DndExportEIR>();

            if (ETGR.DBResponse.Data != null)
            {
                ListEIR = (List<DndExportEIR>)ETGR.DBResponse.Data;
            }
            return PartialView("ExportEIRList", ListEIR);

        }


        [HttpGet]
        public ActionResult EditExportEIR(int EIRId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            DndExportEIR ObjIR = new DndExportEIR();
            ObjER.GetPortOfLoading();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPort = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.ListOfPort = null;
            }
            if(EIRId>0)
            {
                ObjER.VieEditEIRData(EIRId);
                if(ObjER.DBResponse.Data!=null)
                {
                    ObjIR = (DndExportEIR)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjIR);
        }


        [HttpGet]
        public ActionResult ViewExportEIR(int EIRId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            DndExportEIR ObjIR = new DndExportEIR();
            if (EIRId > 0)
            {
                ObjER.VieEditEIRData(EIRId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjIR = (DndExportEIR)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjIR);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteExportEIR(int EIRId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (EIRId > 0)
                objER.DeleteExportEIR(EIRId);
            if (objER.DBResponse.Status == 1)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
        #region CCINEntry(Spl Operation)
        public ActionResult CCINEntrySpl(int Id = 0, int PartyId = 0)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetCCINShippingLineForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstShippingLine = Jobject["LstShippingLine"];
                ViewBag.SLAState = Jobject["State"];
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            ObjER.GetVehicleForCCIN();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListVehicle = ObjER.DBResponse.Data;
            }

            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }


            ObjER.GetSBList();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = ObjER.DBResponse.Data;
            }
            /*
            ObjER.GetCCINPartyList();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.lstParty = ObjER.DBResponse.Data;
            }*/
            List<Godown> lstGodown = new List<Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.lstGodown = ObjER.DBResponse.Data;
            }
            // ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ObjER.GetCCINEntryForEdit(Id);
                if (ObjER.DBResponse.Data != null)
                {
                    objCCINEntry = (Dnd_CCINEntry)ObjER.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

       

       
       


        [HttpGet]
        public ActionResult GetSBDetailsBySBIdSpl(int SBId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGateEntryDetForCCINSpl(int EntryId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCCINByGateEntryId(EntryId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCCINChargesSpl(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetCCINCharges(CCINEntryId, PartyId, Weight, FOB, CargoType);
            objCCINEntry = (Dnd_CCINEntry)objExport.DBResponse.Data;
            ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntrySpl(Dnd_CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            ModelState.Remove("ExporterId");
            ModelState.Remove("ExporterName");
            ModelState.Remove("PartyId");
            ModelState.Remove("PartyName");
            if (ModelState.IsValid)
            {
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
                // IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                // string XML = Utility.CreateXML(PostPaymentChargeList);
                // objCCINEntry.PaymentSheetModelJson = XML;
                objER.AddEditCCINEntrySpl(objCCINEntry);
                return Json(objER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };

                var errors =
                from item in ModelState
                where item.Value.Errors.Count > 0
                select item.Key;
                var keys = errors.ToArray();

                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult ViewCCINEntrySpl(int Id)
        {
            Dnd_CCINEntry objCCINEntry = new Dnd_CCINEntry();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetCCINEntrySpl(Id);
            if (objER.DBResponse.Data != null)
                objCCINEntry = (Dnd_CCINEntry)objER.DBResponse.Data;
            return PartialView("ViewCCINEntrySpl", objCCINEntry);
        }


        [HttpGet]
        public ActionResult ListOfCCINEntrySpl()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryForPageSpl(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        #region CCINEntry Search
        [HttpGet]
        public ActionResult ListOfCCINEntrySearchSpl(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryForSearchSpcl(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView("ListOfCCINEntrySpl", lstCCINEntry);
        }




        #endregion

        [HttpGet]
        public JsonResult LoadMoreCCINEntryListSpl(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> LstJO = new List<Dnd_CCINEntry>();
            ObjCR.GetAllCCINEntryForPageSpl(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntrySpl(int CCINEntryId)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }

        #endregion


        #region Export Destuffing(Spl Operation)

        public ActionResult CreateExportDestuffingSpl(int DestuffingEntryId = 0)
        {
            Dnd_ExportDestuffing ObjDestuffing = new Dnd_ExportDestuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = ObjER.DBResponse.Data;
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            ObjER.GetCCINShippingLineForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstShippingLine = Jobject["LstShippingLine"];
                ViewBag.SLAState = Jobject["State"];
            }
            else
            {
                ViewBag.LstShippingLine = null;
                ViewBag.SLAState = null;
            }
            //ObjER.ListOfExporter();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ListOfExporter = ObjER.DBResponse.Data;
            //else
            //    ViewBag.ListOfExporter = null;


            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
                ViewBag.ExpState = null;

            }


            ObjER.ListOfSplCCINNo();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ListOfSplCCINNo = ObjER.DBResponse.Data;
            else
                ViewBag.ListOfSplCCINNo = null;
            GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.GodownList = ObjGR.DBResponse.Data;
            }
            else
            {
                ViewBag.GodownList = null;
            }
            ObjER.ListOfCommodity();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ListOfCommodity = ObjER.DBResponse.Data;
            else
                ViewBag.ListOfCommodity = null;
            if (DestuffingEntryId > 0)
            {

                ObjER.GetDestuffingEntryDetailsByIdSpl(DestuffingEntryId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjDestuffing = (Dnd_ExportDestuffing)ObjER.DBResponse.Data;
                }
            }
            else
            {
                ObjDestuffing.Destuffingdate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            return PartialView(ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoForExportDestuffingSpl()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainersForExpDestuffingSpcl();
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSBDetForExportDestuffingSpl(string CFSCode)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetSBDetForExpDestuffingSpcl(CFSCode);
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult AddEditExpDestuffingSpl(Dnd_ExportDestuffing objDestuff, String lstDestuffDetail)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                Dnd_ExportRepository objER = new Dnd_ExportRepository();
                List<Dnd_ExportDestuffDetails> lstDestuff = new List<Dnd_ExportDestuffDetails>();
                if (lstDestuffDetail.Length > 0)
                {
                    lstDestuff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ExportDestuffDetails>>(lstDestuffDetail);
                    lstDestuff.ForEach(x => x.SBDate = Convert.ToDateTime(x.SBDate).ToString("yyyy-MM-dd"));
                    DestuffingEntryXML = Utility.CreateXML(lstDestuff);
                }
                objER.AddEditExpDestuffingEntrySpl(objDestuff, DestuffingEntryXML);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }


        [HttpGet]
        public ActionResult GetDestuffingEntryListSpl()
        {
            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntrySpl(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("GetDestuffingEntryListSpl", LstDestuffing);
        }


        [HttpGet]
        public ActionResult GetDestuffingEntryListSplSearch(string Search)
        {
            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntrySplserch(Search);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("GetDestuffingEntryListSpl", LstDestuffing);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntrySpl(int Page)
        {
            Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
            List<Dnd_ExportDestuffingList> LstDestuffing = new List<Dnd_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntrySpl(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return Json(LstDestuffing, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ViewExportDestuffingEntrySpl(int DestuffingEntryId)
        {
            Dnd_ExportDestuffing ObjDestuffing = new Dnd_ExportDestuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetDestuffingEntryDetailsByIdSpl(DestuffingEntryId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjDestuffing = (Dnd_ExportDestuffing)ObjER.DBResponse.Data;
            }
            return PartialView(ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntrySpl(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                Dnd_ExportRepository ObjIR = new Dnd_ExportRepository();
                ObjIR.DelDestuffingEntry(DestuffingEntryId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetDestuffingEntryForPrintSpl(int DestuffingId)
        {
            Dnd_ExportRepository ObjRR = new Dnd_ExportRepository();
            ObjRR.GetExportDestuffingForPrintSpl(DestuffingId);
            string Path = "";
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                Path = GeneratePDFForDestuffSheetSpl(ds, DestuffingId);
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);

            }
            else
                return Json(new { Status = 0, Data = "No Record Found" }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public string GeneratePDFForDestuffSheetSpl(DataSet ds, int DestuffingId)
        {

            List<dynamic> lstSBhdr = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstSBhdtl = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);

            StringBuilder objSB = new StringBuilder();
            /*
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
             {
                 Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
             }
             if (System.IO.File.Exists(Path))
             {
                 System.IO.File.Delete(Path);
             }*/


            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: right;' colspan='12'>");
            objSB.Append("<h1 style='font-size: 12px; line-height: 20px; font-weight: 300;margin: 0; padding: 0;'>");
            objSB.Append("</h1></td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>" + lstSBhdr[0].CompanyAddress + "</label><br/><label style='font-size: 16px; font-weight:bold;'>EXPORT DESTUFFING SHEET(Spl Operation)</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            objSB.Append("<tr><th colspan='6' style='font-size:13px;width:50%'>SHED CODE: <span style='font-size:12px;font-weight:normal;'>" + lstSBhdr[0].GodownName + "</span></th>");
            objSB.Append("<th colspan='6' style='font-size:13px;text-align:right;width:50%'>AS ON: <span style='font-size:12px;font-weight:normal;'>" + lstSBhdr[0].DestuffingEntryDate + "</span></th></tr>");
            objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td style='text-align: left;'>");
            //objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>SHED CODE: <label>" + ObjDestuff.GodownName + "</label>");
            //objSB.Append("</span></td><td colspan='3' style='text-align: center;'>");
            //objSB.Append("<span style='display: block; font-size: 14px; line-height: 22px;  padding-bottom: 10px; font-weight:bold;'>DESTUFFING SHEET</span>");
            //objSB.Append("</td><td colspan='2' style='text-align: left;'><span style='display: block; font-size: 11px; padding-bottom: 10px;'>");
            //objSB.Append("AS ON: <label>" + ObjDestuff.DestuffingEntryDateTime + "</label></span></td></tr>");
            // var FOB = lstSBhdtl.Sum(x => x.FOB);
            //var GrossWeight = lstSBhdtl.Sum(x => x.GrossWeight);
            // var Area = lstSBhdtl.Sum(x => x.Area);


            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table style='width:100%; margin: 0;margin-bottom: 10px;'><tbody>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>DESTUFF SHEET NO.:</label> <span>" + lstSBhdr[0].DestuffingEntryNo + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px;width:33.33333333333333%;'><label style='font-weight: bold;'> DATE OF DESTUFFING : </label> <span>" + lstSBhdr[0].DestuffingEntryDate + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px;width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>Container / CBT No: </label> <span>" + lstSBhdr[0].ContainerNo + "</span></td></tr>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>Size:</label> <span>" + lstSBhdr[0].Size + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>ICD Code: </label> <span>" + lstSBhdr[0].CFSCode + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>In Date : </label> <span>" + lstSBhdr[0].GateInDate + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr><td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>Custom Seal No. :</label> <span>" + lstSBhdr[0].CustomSealNo + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;'><label style='font-weight: bold;'>SLA : </label> <span>" + lstSBhdr[0].ShippingLine + "</span></td>");
            objSB.Append("<td colspan='4' style='font-size: 11px; padding-bottom:15px; width:33.33333333333333%;text-align:right;'><label style='font-weight: bold;'>Sla Seal no. : </label> <span>" + lstSBhdr[0].SLASealNo + "</span></td></tr>");

            objSB.Append("<tr><td colspan='12' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Remarks : </label> <span>" + lstSBhdr[0].Remarks + "</span></td></tr>");

            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table style='border:1px solid #000; font-size:8pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SR No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SB No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>New SB No.</th>");

            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SB Date</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Exporter</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Cargo</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>Cargo Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>No. Pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Gr Wt</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>FOB</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>CUM</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Area</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Commodity</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Location</th>");
            objSB.Append("</tr></thead>");

            objSB.Append("<tfoot><tr>");
            objSB.Append("<td colspan='7' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-weight: bold; text-align: center; padding: 5px;'></td>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(lstSBhdtl.Sum(x => x.NoOfPackages)) + "</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: left; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");
            objSB.Append("<td colspan='2' style='border-bottom: 1px solid #000; text-align: left;'></td>");
            objSB.Append("</tr></tfoot>");
            objSB.Append("<tbody>");
            int Serial = 1;
            lstSBhdtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + Serial + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SBNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.NewSBNo + "</td>");

                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SBDate + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Exporter + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.CargoDescription + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.CargoType + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.NoOfPackages + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.GrossWeight + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.FOB + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.CUM + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.Area + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.CommodityName + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.GodownWiseLctnNames + "</td>");

                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr><tr>");
            objSB.Append("<td colspan='12' style=' font-size: 11px; padding-top: 15px; text-align: left;'>*GOODS RECEIVED ON S/C &amp; S/W BASIC - CWC IS NOT RESPONSIBLE FOR SHORT LANDING &amp; LEAKAGES IF ANY</td>");
            objSB.Append("</tr><tr><td colspan='12' style=' font-size: 12px; text-align: left;padding-top: 15px;'>Signature &amp; Designation :</td></tr></tbody>");
            objSB.Append("</table></td></tr>");
            objSB.Append("<tr><td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>H &amp; T Agent</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Consignee</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Shipping Line</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>ICD</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Customs</td></tr>");
            objSB.Append("</tbody></table>");

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ExpDestuffingSheet" + DestuffingId + ".pdf";

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {

                RH.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/ExpDestuffingSheet" + DestuffingId + ".pdf";
        }



        #endregion

        #region Re-Movement Invoice
        [HttpGet]
        public ActionResult CreateReMovementInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetReMovementContainerRequestForInvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetReMovementPaymentSheetContainer(int StuffingReqId)
        {
            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            objExport.GetContainerForReMOvementInvoice(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
        //    int PartyId, int IsLock, int IsReefer, string PlugInDateTime, string PlugOutDateTime, int InvoiceId = 0)        
        public JsonResult GetReMovementPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
            int PartyId, int IsLock, int IsGroundRent, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }
            Dnd_ExportRepository objChrgRepo = new Dnd_ExportRepository();
            //objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, InvoiceDate, InvoiceType, ContainerXML, PayeeId, PartyId, IsLock, IsReefer, PlugInDateTime, PlugOutDateTime, InvoiceId);
            objChrgRepo.GetReMovementInvoice(StuffingReqId, InvoiceDate, InvoiceType, ContainerXML, PayeeId, PartyId, IsLock, IsGroundRent, InvoiceId);
            var Output = (DND_ExpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPLod";

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
                obj.ContainerClass = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerClass.ToString();
                obj.PayMode = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().PayMode;
                obj.ExportType = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ExportType;
                obj.IsODC = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.ODC);
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
                    Output.ArrivalDate += item.ArrivalDate;

                Output.TotalNoOfPackages = Output.lstPostPaymentCont.Sum(o => o.NoOfPackages);
                Output.TotalGrossWt = Output.lstPostPaymentCont.Sum(o => o.GrossWt);
                Output.TotalWtPerUnit = Output.lstPostPaymentCont.Sum(o => o.WtPerUnit);
                Output.TotalSpaceOccupied = Output.lstPostPaymentCont.Sum(o => o.SpaceOccupied);
                Output.TotalSpaceOccupiedUnit = Output.lstPostPaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPostPaymentCont.Sum(o => o.CIFValue) + Output.lstPostPaymentCont.Sum(o => o.Duty);
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
                Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditReMovementInvoice(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<DND_ExpPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

                foreach (var item in invoiceData.lstPSCont)
                {
                    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
                }
                foreach (var item in invoiceData.lstOperationContwiseAmt)
                {
                    if (item.DocumentDate != "")
                    {
                        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        item.DocumentType = "";
                    }
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
                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditReMovementInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);

                //return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Ship Bill Amendment without Invoice

        public ActionResult AmendmentWI()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAmenSBList();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNoAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfSBNoAmendment = null;
            }
            //objER.ListOfExporter();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfExporterForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfExporterForAmendment = null;
            //}

            objER.ListOfCha();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfChaForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfChaForAmendment = null;
            }
            //objER.ListOfShi();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfShiForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfShiForAmendment = null;
            //}
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.ListOfShippingLinePartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            objIR.ListOfChaForPage("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            //objER.ListOfPOD();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfPODForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfPODForAmendment = null;
            //}
            //objER.GetAllChaForPageAmendment("", 0);
            //if (objER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstCHA = Jobject["lstCHA"];
            //    ViewBag.State = Jobject["State"];
            //}
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            //objER.GetInvoiceListForShipbillAmend();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfInv = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfInv = null;
            //}
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSBDetailsBySBNoWI(string SBid, string SbDate)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.GetAmenSBDetailsBySbNoDate(SBid, SbDate);
            if (obj.DBResponse.Status > 0)
            {
                return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }



        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SaveAmendementWI(List<OldInfoSb> vm, List<NewInfoSb> newvm, string Date, string AmendmentNO, int InvoiceId, string InvoiceNo, string InvoiceDate, string FlagMerger)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            if (FlagMerger == "Split" && vm.Count > 1)
            {
                return Json(new { Message = "Only One Shipbill Split", Status = 0 }, JsonRequestBehavior.AllowGet);

            }
            else if (FlagMerger == "Merger" && vm.Count == 1)
            {
                return Json(new { Message = "Merge Operation Can't Be Done With Single Shipping Bill", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string OldInfoSbXml = Utility.CreateXML(vm);
                string NewInfoSbSbXml = Utility.CreateXML(newvm);

                obj.AddEditAmendment(OldInfoSbXml, NewInfoSbSbXml, Date, InvoiceId, InvoiceNo, InvoiceDate, FlagMerger);


                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

            }

        }        

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SubmitAmedDataWI(Dnd_AmendmentViewModel vm)
        {
            Dnd_ExportRepository obj = new Dnd_ExportRepository();
            obj.AddEditShipAmendmentWI(vm);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListofAmendDataWI()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_AmendmentViewModel> lstdata = new List<Dnd_AmendmentViewModel>();
            objER.ListForShipbillAmendWI();
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Dnd_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }
        
        [HttpGet]
        public ActionResult ViewMergeSplitDataWI(string AmendNo)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_Amendment> obj = new List<Dnd_Amendment>();
            objER.GetAmenDetailsByAmendNO(AmendNo);
            if (objER.DBResponse.Status == 1)
            {
                obj = (List<Dnd_Amendment>)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ListofMergeSplitAmendDataWI()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_AmendmentViewModel> lstdata = new List<Dnd_AmendmentViewModel>();
            objER.GetAmenSBDetailsByAmendNO("");
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Dnd_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        #endregion

        #region CCINEntry Search Approval
        [HttpGet]
        public ActionResult ListOfCCINEntrySearchApproval(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CCINEntry> lstCCINEntry = new List<Dnd_CCINEntry>();
            objER.GetAllCCINEntryForSearchApproval(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Dnd_CCINEntry>)objER.DBResponse.Data;
            return PartialView("ListOfCCINEntryApproval", lstCCINEntry);
        }




        #endregion

        #region Stuffing Request For ITP Container

        [HttpGet]
        public ActionResult CreateStuffingRequestITP()
        {
            Dnd_StuffingRequest ObjSR = new Dnd_StuffingRequest();
            CHN_ExportRepository ObjCER = new CHN_ExportRepository();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();


            /*  ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
              if (ObjER.DBResponse.Data != null)
              {
                  ViewBag.CartingRegNoList = new SelectList((List<Dnd_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
              }
              else
              {
                  ViewBag.CartingRegNoList = null;
              }*/
            /* ObjER.ListOfChaForPage("", 0);
             if (ObjER.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.lstCHA = Jobject["lstCHA"];
                 ViewBag.CHAState = Jobject["State"];
             }
             else
             {
                 ViewBag.lstCHA = null;
             }*/
            /* ObjER.GetShippingLine();
             if (ObjER.DBResponse.Data != null)
             { ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
             else
             { ViewBag.ShippingLineList = null; }*/
            /* ObjER.ListOfShippingLinePartyCode("", 0);
             if (ObjER.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                 ViewBag.State = Jobject["State"];
             }
             else
             {
                 ViewBag.ShippingLineList = null;
             }
             ObjER.ListOfExporterForPage("", 0);
             if (ObjER.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.lstExp = Jobject["lstExp"];
                 ViewBag.ExpState = Jobject["State"];
             }
             else
             {
                 ViewBag.lstExp = null;
             }

             ObjER.ListOfForwarderForPage("", 0);
             if (ObjER.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.lstFwd = Jobject["lstFwd"];
                 ViewBag.FwdState = Jobject["State"];
             }
             else
             {
                 ViewBag.lstFwd = null;
             }

             ObjER.ListOfMainlineForPage("", 0);
             if (ObjER.DBResponse.Data != null)
             {
                 var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                 var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                 ViewBag.lstMln = Jobject["lstMln"];
                 ViewBag.MlnState = Jobject["State"];
             }
             else
             {
                 ViewBag.lstMln = null;
             }*/
            /*  ObjER.GetAllContainerNo();
               if (ObjER.DBResponse.Data != null)
               {
                   ViewBag.ContainerList = new SelectList((List<Dnd_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                   ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
               }
               else
                   ViewBag.ContainerList = null;*/

            /*ObjCER.ListOfPOD();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListOfPOD = null;*/

            /*ObjCER.ListOfCityForStuffingReq();
            if (ObjCER.DBResponse.Data != null)
            {
                ViewBag.ListCity = ObjCER.DBResponse.Data;
            }
            else
                ViewBag.ListCity = null;*/

            /*If User is External Or Non CWC User*/
            bool Exporter, CHA;
            Exporter = ((Login)Session["LoginUser"]).Exporter;
            CHA = ((Login)Session["LoginUser"]).CHA;
            /* if (CHA == true)
             {
                 ObjSR.CHA = ((Login)Session["LoginUser"]).Name;
                 ObjSR.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
             }
             else
             {
                 ObjCER.ListOfCHA();
                 if (ObjCER.DBResponse.Data != null)
                     ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjCER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                 else
                     ViewBag.ListOfCHA = null;
             }*/

            Dnd_EntryThroughGateRepository ObjETR = new Dnd_EntryThroughGateRepository();
            ObjETR.ListContainerClass();
            if (ObjETR.DBResponse.Data != null)
            {
                ViewBag.ContClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ObjETR.DBResponse.Data;
            }
            else
            {
                ViewBag.ContClass = null;
            }

            ObjSR.RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView("CreateStuffingRequestITP", ObjSR);
        }

        [HttpGet]
        public JsonResult ShippinglineDtlAfterEmptyContITP(string CFSCode)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCartingRegisterNoITP(int ShippingLineId, string StuffRefType)
        {
            if (ShippingLineId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.GetCartRegForStuffingReqITP(ShippingLineId, StuffRefType);
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetContainerNoITP(int ShippingLineId)
        {
            if (ShippingLineId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.GetContainerNo(ShippingLineId);
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]

        public ActionResult AddEditStuffingReqITP(Dnd_StuffingRequest ObjStuffing)
        {
            ModelState.Remove("CHAId");
            ModelState.Remove("ForwarderId");
            ModelState.Remove("ViaId");
            if (ModelState.IsValid)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                IList<Dnd_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<Dnd_StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<Dnd_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<Dnd_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
                string StuffingXML = Utility.CreateXML(LstStuffing);
                string StuffingContrXML = Utility.CreateXML(LstStuffConatiner);
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditStuffingRequestITP(ObjStuffing, StuffingXML, StuffingContrXML);
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
        public ActionResult EditStuffingRequestITP(int StuffinfgReqId)
        {
            Dnd_StuffingRequest ObjStuffing = new Dnd_StuffingRequest();
            CHN_ExportRepository ObjCER = new CHN_ExportRepository();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            
            if (StuffinfgReqId > 0)
            {
                ObjER.Kdl_GetStuffingRequestITP(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_StuffingRequest)ObjER.DBResponse.Data;
                }
                
                Dnd_EntryThroughGateRepository ObjETR = new Dnd_EntryThroughGateRepository();
                ObjETR.ListContainerClass();
                if (ObjETR.DBResponse.Data != null)
                {
                    ViewBag.ContClass = (List<CwcExim.Areas.Export.Models.ContainerClass>)ObjETR.DBResponse.Data;
                }
                else
                {
                    ViewBag.ContClass = null;
                }

            }
            return PartialView("EditStuffingRequestITP", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequestITP(int StuffinfgReqId)
        {
            Dnd_StuffingRequest ObjStuffing = new Dnd_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.Kdl_GetStuffingRequestITP(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequestITP", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequestITP(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.DeleteStuffingRequest(StuffinfgReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetCartRegDetForStuffingReqITP(int CartingRegisterId, string StuffRefType)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Kdl_GetCartRegDetForStuffingReqITP(CartingRegisterId, StuffRefType);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqListITP()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.GetAllStuffingRequestITP(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestListITP", LstStuffing);
        }

        [HttpGet]
        public ActionResult LoadStuffingReqListITP(int Page)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchStuffingReqITP(string ContNo)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Dnd_StuffingRequest> LstStuffing = new List<Dnd_StuffingRequest>();
            ObjER.SearchStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerDetITP(string CFSCode, string ContainerNo)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            // StuffingReqContainerDtl ObjSRD = new StuffingReqContainerDtl();
            ObjER.GetContainerNoDet(CFSCode, ContainerNo);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ObjSRD = (StuffingReqContainerDtl)ObjER.DBResponse.Data;
            //}
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetForeignLinerForStuffingReqITP()
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetForeignLinerList();
            if (objER.DBResponse.Data != null)
            {
                return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Container Stuffing ITP Container
        public ActionResult CreateContainerStuffingITP()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjCS = new Dnd_ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            ObjER.GetReqNoForContainerStuffingITP(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }

            /*ObjER.ListOfChaForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                ViewBag.lstCHA = null;
            }
            ObjER.ListOfExporterForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstExp = Jobject["lstExp"];
                ViewBag.ExpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstExp = null;
            }
            ObjER.ListOfShippingLinePartyCode("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }*/

            ObjER.ListOfPOL();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }

            /*ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPortOfDischarge = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);

            }
            else
            {
                ViewBag.ListOfPortOfDischarge = null;
            }*/

            return PartialView(ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReqITP(int StuffingReqId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerNoByStuffingReqITP(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReqITP(int StuffingReqId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerDetForStuffingITP(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingListITP()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetAllContainerStuffingITP(((Login)Session["LoginUser"]).Uid, 0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingListITP", LstStuffing);
        }

        [HttpGet]
        public ActionResult LoadContainerStuffingListITP(int Page)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetAllContainerStuffingITP(((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchContainerStuffingITP(string ContNo)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Dnd_ContainerStuffing> LstStuffing = new List<Dnd_ContainerStuffing>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.SearchContainerStuffingITP(ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Dnd_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingListITP", LstStuffing);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffingITP(int ContainerStuffingId)
        {
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingITP(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffingITP", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffingITP(int ContainerStuffingId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingITP(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
                }

                ObjER.ListOfChaForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstCHA = null;
                }
                ObjER.ListOfExporterForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstExp = Jobject["lstExp"];
                    ViewBag.ExpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstExp = null;
                }
                ObjER.ListOfShippingLinePartyCode("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }


                ObjER.ListOfPOL();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
                }
                else
                {
                    ViewBag.ListOfPOD = null;
                }
                //ObjER.ListOfPOD();
                //if (ObjER.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfPortOfDischarge = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);

                //}
                //else
                //{
                //    ViewBag.ListOfPortOfDischarge = null;
                //}
            }
            return PartialView("EditContainerStuffingITP", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoListITP(int StuffingReqId)
        {
            List<Dnd_ContainerStuffingDtl> LstStuffing = new List<Dnd_ContainerStuffingDtl>();
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            ObjER.GetContainerNoByStuffingReqITP(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDetITP(Dnd_ContainerStuffing ObjStuffing)
        {
            // ModelState.Remove("GENSPPartyCode");
            // ModelState.Remove("GREPartyCode");
            // ModelState.Remove("INSPartyCode");
            // ModelState.Remove("HandalingPartyCode");
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<Dnd_ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffingITP(ObjStuffing, ContainerStuffingXML);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };

                var errors =
                from item in ModelState
                where item.Value.Errors.Count > 0
                select item.Key;
                var keys = errors.ToArray();
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteContainerStuffingDetITP(int ContainerStuffingId)
        {
            if (ContainerStuffingId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                ObjER.DeleteContainerStuffingITP(ContainerStuffingId);
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
        public JsonResult PrintContainerStuffingITP(int ContainerStuffingId)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ContainerStuffing ObjStuffing = new Dnd_ContainerStuffing();
            ObjER.GetContainerStuffForPrintITP(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Dnd_ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuffITP(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuffITP(Dnd_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "",
            Area = "", PortName = "", PortDestination = "", Remarks = "", CargoType = "", PortDischarge, Via = "", Vessel = "";

            String Consignee = ""; int SerialNo = 1;
            if (ObjStuffing.LstStuffing.Count() > 0)
            {
                ObjStuffing.LstStuffing.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    ShippingBillNo = (ShippingBillNo == "" ? ((item.ShippingBillNo) + " ") : (item.ShippingBillNo + "<br/>" + item.ShippingBillNo + " "));
                    /*   if (ShippingBillNo == "")
                           ShippingBillNo = item.ShippingBillNo + " ";
                       else
                           ShippingBillNo += "," + item.ShippingBillNo; */
                });

                ObjStuffing.LstStuffing.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
                {

                    ShippingDate = (ShippingDate == "" ? (item.ShippingDate) : (item.ShippingDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { EntryNo = x.EntryNo }).Distinct().ToList().ForEach(item =>
                {

                    EntryNo = (EntryNo == "" ? (item.EntryNo) : (item.EntryNo));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
                {

                    InDate = (InDate == "" ? (item.InDate) : (item.InDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffing.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                ObjStuffing.LstStuffing.Select(x => new { PortName = x.PortName }).Distinct().ToList().ForEach(item =>
                {
                    if (PortName == "")
                        PortName = item.PortName;
                    else
                        PortName += "," + item.PortName;
                });
                ObjStuffing.LstStuffing.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });


                ObjStuffing.LstStuffing.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });
                ObjStuffing.LstStuffing.Select(x => new { Consignee = x.Consignee }).Distinct().ToList().ForEach(item =>
                {
                    if (Consignee == "")
                        Consignee = item.Consignee;
                    else
                        Consignee += "," + item.Consignee;
                });

                ObjStuffing.LstStuffing.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                {

                    if (ShippingLine == "")
                        ShippingLine = item.ShippingLine;
                    else
                        ShippingLine += "," + item.ShippingLine;
                });

                ObjStuffing.LstStuffing.Select(x => new { Remarks = x.Remarks }).Distinct().ToList().ForEach(item =>
                {

                    if (Remarks == "")
                        Remarks = item.Remarks;
                    else
                        Remarks += "," + item.Remarks;
                });
                ObjStuffing.LstStuffing.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                {
                    if (CHA == "")
                        CHA = item.CHA;
                    else
                        CHA += "," + item.CHA;
                });

                //StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                //Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                StuffQuantity = (ObjStuffing.LstStuffing.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffing.Sum(x => x.StuffQuantity)).ToString() : "";

                ObjStuffing.LstStuffing.ToList().ForEach(item =>
                {

                    Commodity = (Commodity == "" ? (item.CommodityName) : Commodity == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));

                });

                ObjStuffing.LstCont.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                    CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));
                    //SerialNo++;
                });


                ObjStuffing.LstStuffing.Select(x => new { CustomSeal = x.CustomSeal }).Distinct().ToList().ForEach(item =>
                {

                    if (CustomSeal == "")
                        CustomSeal = item.CustomSeal;
                    else
                        CustomSeal += "<br/>" + item.CustomSeal;
                });

                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";

                Html += "<thead>";

                Html += "<tr><td colspan='4'>";
                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
                Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
                Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + ObjStuffing.CompanyAddress + "</span><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER STUFFING SHEET</label><br/><label style='font-size: 14px;'> <b>Shed No :</b> " + ObjStuffing.GodownName + "</label></td>";
                Html += "<td width='12%' align='right' valign='top'>";
                Html += "<table style='width:100%;' cellspacing='0' cellpadding='0' valign='top'><tbody>";
                Html += "<tr><td style='border:1px solid #333;' valign='top'>";
                Html += "<div valign='top' style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CFSCHN/69</div>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</thead>";

                Html += "<tbody>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='8%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='8%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>CFS Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='15%' style='margin:0 0 10px;'><b>Size :</b> <u>" + ObjStuffing.Size + "</u></td>  <td colspan='3' width='35%' style='margin:0 0 10px; text-align: right;'><b>Shipping Line :</b> <u>" + ShippingLine + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Via No. :</b> <u>" + ObjStuffing.Via + "    " + ObjStuffing.Vessel + "</u> </td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Port Of Discharge :</b> <u>" + ObjStuffing.PODischarge + "</u></td> </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Cont Type</b> <u>" + CargoType + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Name : </b> <u>" + ObjStuffing.Mainline + "</u></td>  </tr></tbody></table> </td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='8' style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>S. No.</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Entry No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>InDate</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb No / ITP OBL</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb / ITP OBL Date</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Exporter</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Consignee</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Comdty</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Pkts</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Gr Wt</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>FOB</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Remarks</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                ObjStuffing.LstStuffing.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>" + SerialNo++ + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.EntryNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.InDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingBillNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.Exporter + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Consignee + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.CommodityName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffQuantity + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffWeight + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Fob + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Remarks + "</td></tr>";
                });
                //LOOP END

                Html += "<tr><th style='padding:3px;text-align:center;width:5%;'>Total :</th>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:10%;'></td>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.StuffQuantity) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.StuffWeight) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffing.AsEnumerable().Sum(item => item.Fob) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'></th></tr>";

                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td colspan='3' width='25%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>Signature and designation</td>";
                Html += "<td colspan='5' width='41.66666666666667%' style='padding:3px;text-align:left;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>1</td><td colspan='2' width='85%' style='font-size:11px;'>All activities including those at incomming and in process stages have been satisfactorily completed.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>2</td><td colspan='2' width='85%' style='font-size:11px;'>All the necessary records have been completed and verified with Date and seal.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:10px;'>3</td><td colspan='2' width='85%' style='font-size:11px;'>Cargo/Containers delivered in good condition.</td></tr>";
                Html += "</tbody></table>";
                Html += "</td>";
                Html += "<td colspan='1' width='8.333333333333333%'></td>";
                Html += "<td colspan='4' width='33.33333333333333%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>The container is allowed to be sent to Jawaharlal Nehru Port Officer on behalf of Jawahar Custom's House</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td style='text-align:left;'>Shed Asst. <br/> CFS Dronagiri</td><td style='text-align:left;'>Shed I/C <br/>CWC-CFS,D-NODE</td><td style='text-align:center;'>(On behalf of Jawahar Custom's House)</td></tr></tbody></table></td></tr>";
                Html += "</tbody></table>";


            }

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {

                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        }

        #endregion

        #region Export Payment Sheet ITP
        [HttpGet]
        public ActionResult CreateExportPaymentSheetITP(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            Dnd_ExportRepository objExp = new Dnd_ExportRepository();
            objExp.GetContStuffingForPaymentSheetITP();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objExp.GetPaymentParty();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainerITP(int AppraisementId)
        {
            Dnd_ExportRepository objImport = new Dnd_ExportRepository();
            objImport.GetContDetForPaymentSheetITP(AppraisementId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
        //    int PartyId, int PayeeId, int IsLock, int IsReefer , string PlugInDateTime, string PlugOutDateTime, int InvoiceId = 0)
        public JsonResult GetContainerPaymentSheetITP(string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
            int PartyId, int PayeeId, int IsLock, int IsCompositeTariff, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            Dnd_ExportRepository objPpgRepo = new Dnd_ExportRepository();
            //objPpgRepo.GetExportPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId,IsLock, IsReefer, PlugInDateTime, PlugOutDateTime);
            objPpgRepo.GetExportPaymentSheetITP(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId, IsLock, IsCompositeTariff);
            var Output = (DND_ExpPaymentSheet)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXP";

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
                obj.IsODC = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.ODC);
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
        public JsonResult AddEditContPaymentSheetITP(FormCollection objForm)
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
                    item.StuffingDate = Convert.ToDateTime(item.StuffingDate).ToString("yyyy-MM-dd");
                    item.ArrivalDate = Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
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

                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditExpInvoiceITP(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "ITPEXP");

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
        public ActionResult ListOfExpInvoiceITP(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.ListOfExpInvoiceITP(Module, InvoiceNo, InvoiceDate);
            List<DNDListOfExpInvoice> obj = new List<DNDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DNDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoiceITP", obj);
        }
        #endregion

        #region Cargo Shifting Approval
        public ActionResult CreateCargoShiftingApproval()
        {
            //--------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //-------------------------------------------------------------------------------

            Dnd_ExportRepository objExport = new Dnd_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();
            Dnd_CargoShiftingShipBillDetails objCR = new Dnd_CargoShiftingShipBillDetails();

            objExport.GetAllAppNoCargoShiftingApproval();
            if (objExport.DBResponse.Data != null)
                objCR.lstAppNo = (List<Dnd_ApplicationNoDet>)objExport.DBResponse.Data;

            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownList = lstGodown;

            //objExport.getOnlyRightsGodown();
            //List<Godown> lstGodownF = new List<Godown>();
            //if (objExport.DBResponse.Data != null)
            //{
            //    lstGodownF = (List<Godown>)objExport.DBResponse.Data;
            //}
            //ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);
            //-------------------------------------------------------------------------------

            return PartialView(objCR);
        }

        public JsonResult GetShipBillDetailsApproval(int ShippingLineId, int ShiftingType, int GodownId)
        {
            CHN_ExportRepository objER = new CHN_ExportRepository();
            objER.GetShipBillDetails(ShippingLineId, ShiftingType, GodownId);
            if (objER.DBResponse.Status > 0)
            {
                return Json(objER.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddEditCargoShiftingApproval(Dnd_CargoShiftingShipBillDetails objForm)
        {
            try
            {
                Dnd_ExportRepository objChargeMaster = new Dnd_ExportRepository();
                objChargeMaster.AddEditCargoShiftApproval(objForm, ((Login)(Session["LoginUser"])).Uid);

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        public JsonResult GetApplicationDetForCargoShiftingApproval(int CargoShiftingId)
        {
            Dnd_CargoShiftingShipBillDetails objCR = new Dnd_CargoShiftingShipBillDetails();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAppDetForCargoShiftingApproval(CargoShiftingId);
            if (objER.DBResponse.Data != null)
                objCR = (Dnd_CargoShiftingShipBillDetails)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListShiftRegisterApproval()
        {
            List<Dnd_CargoShiftingShipBillDetails> lstcart = new List<Dnd_CargoShiftingShipBillDetails>();
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            objER.GetAllShiftingForPageApproval(0);
            if (objER.DBResponse.Data != null)
                lstcart = (List<Dnd_CargoShiftingShipBillDetails>)objER.DBResponse.Data;

            return PartialView("ListOfShiftingRegisterApproval", lstcart);
        }

        [HttpGet]
        public JsonResult LoadMoreShiftingListApproval(int Page)
        {
            Dnd_ExportRepository ObjCR = new Dnd_ExportRepository();
            List<Dnd_CargoShiftingShipBillDetails> LstJO = new List<Dnd_CargoShiftingShipBillDetails>();
            ObjCR.GetAllShiftingForPageApproval(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CargoShiftingShipBillDetails>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfShiftingSearchApproval(string search)
        {
            Dnd_ExportRepository objER = new Dnd_ExportRepository();
            List<Dnd_CargoShiftingShipBillDetails> lstCart = new List<Dnd_CargoShiftingShipBillDetails>();
            objER.GetAllShiftEntryForSearchApproval(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCart = (List<Dnd_CargoShiftingShipBillDetails>)objER.DBResponse.Data;
            return PartialView("ListOfShiftingRegisterApproval", lstCart);
        }
        #endregion
    }
}