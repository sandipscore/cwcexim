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
    public class Ppg_CWCExportController : BaseController
    {

        // GET: Export/Ppg_CWCExport
        #region CCINEntry
        public ActionResult CCINEntry(int Id = 0, int PartyId = 0)
        {
            ExportRepository objER = new ExportRepository();
         //   objER.GetCCINShippingLine();
         //   if (objER.DBResponse.Data != null)
         //   {
         //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
           //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
          //  }
            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = objER.DBResponse.Data;
            }
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            Ppg_ExportRepository ObjRR = new Ppg_ExportRepository();

            ObjER.ListOfPackUQCForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }

            //ObjRR.GetPortOfLoading();
            //if (ObjRR.DBResponse.Data != null)
            //{
            //    List<Port> lstport = (List<Port>)ObjRR.DBResponse.Data;
            //    ViewBag.ListOfPort = lstport;

            //    lstport = lstport.Where(m => m.PortName == "ICD PPG").ToList();
            //    if (lstport.Count > 0)
            //    {
            //        ViewBag.PortName = lstport[0].PortName;
            //        ViewBag.PortId = lstport[0].PortId;
            //    }
            //    else
            //    {
            //        ViewBag.PortName = "";
            //        ViewBag.PortId = 0;
            //    }

            //}
            //  ObjRR.GetSBList();
            //  if (ObjRR.DBResponse.Data != null)
            //   {
            //       ViewBag.ListOfSBNo = ObjRR.DBResponse.Data;
            //  }
            //ObjRR.GetCCINPartyList();
            //if (ObjRR.DBResponse.Data != null)
            //{
            //    ViewBag.lstParty = ObjRR.DBResponse.Data;
            //}

            CCINEntry objCCINEntry = new CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                rep.GetCCINEntryForEdit(Id, PartyId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }


        [HttpGet]
        public JsonResult GetShippingBill()
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetSBList();
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            }
            return Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadPackUQCList(string PartyCode, int Page)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPackUQCByCode(string PartyCode)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInvParty()
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            
            objRepo.GetCCINPartyList();
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.lstParty = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            }
           
            return Json(ViewBag.lstParty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCommodity()
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage("", 0);
            if (objRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Newtonsoft.Json.JsonConvert.SerializeObject(objRepo.DBResponse.Data); 
                ViewBag.CommodityState = Jobject["State"];
            }
            
            return Json(ViewBag.LstCommodity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCHAName()
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
           
            objRepo.ListOfCHA();
           
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.ListOfCHA = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            }
            return Json(ViewBag.ListOfCHA, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetPortOfDest()
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();

            objRepo.GetPortOfLoading();
            if (objRepo.DBResponse.Data != null)
            {
                List<Port> lstport = (List<Port>)objRepo.DBResponse.Data;
                ViewBag.ListOfPort = JsonConvert.SerializeObject(objRepo.DBResponse.Data); 

                lstport = lstport.Where(m => m.PortName == "ICD PPG").ToList();
                if (lstport.Count > 0)
                {
                    ViewBag.PortName = lstport[0].PortName;
                    ViewBag.PortId = lstport[0].PortId;
                }
                else
                {
                    ViewBag.PortName = "";
                    ViewBag.PortId = 0;
                }

            }
           
            return Json(ViewBag.ListOfPort, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetShippingLine()
        {
            ExportRepository objER = new ExportRepository();
            objER.GetCCINShippingLine();
               if (objER.DBResponse.Data != null)
               {
                  ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
                ViewBag.ListOfShippingLine = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString(); 
              }
            return Json(ViewBag.ListOfShippingLine, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            CCINEntry objCCINEntry = new CCINEntry();
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetCCINCharges(CCINEntryId, PartyId, Weight, FOB, CargoType);
            objCCINEntry = (CCINEntry)objExport.DBResponse.Data;
            ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntry(CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            if (ModelState.IsValid)
            {
                Ppg_ExportRepository objER = new Ppg_ExportRepository();
                IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                string XML = Utility.CreateXML(PostPaymentChargeList);
                objCCINEntry.PaymentSheetModelJson = XML;
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
        public ActionResult ListOfCCINEntry()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<CCINEntry> lstCCINEntry = new List<CCINEntry>();
            objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryList(int Page)
        {
            Ppg_ExportRepository ObjCR = new Ppg_ExportRepository();
            List<CCINEntry> LstJO = new List<CCINEntry>();
            ObjCR.GetAllCCINEntryForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntry(int CCINEntryId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
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

            Ppg_ExportRepository ObjRR = new Ppg_ExportRepository();
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

            Ppg_ExportRepository objRepository = new Ppg_ExportRepository();
            objRepository.GetAllCCINEntry("A");
            if (objRepository.DBResponse.Data != null)
            {
                ViewBag.ListOfCCINNo = (List<CCINEntry>)objRepository.DBResponse.Data;
            }

            CCINEntry objCCINEntry = new CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                rep.GetCCINEntryById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetCCINEntryApprovalDetails(int CCINEntryId)
        {
            CCINEntry objCCINEntry = new CCINEntry();
            if (CCINEntryId > 0)
            {
                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                rep.GetCCINEntryById(CCINEntryId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntry)rep.DBResponse.Data;
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.AddEditCCINEntryApproval(Id, IsApproved);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntryApproval()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<CCINEntry> lstCCINEntry = new List<CCINEntry>();
            objER.GetAllCCINEntryApprovalForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryApprovalList(int Page)
        {
            Ppg_ExportRepository ObjCR = new Ppg_ExportRepository();
            List<CCINEntry> LstJO = new List<CCINEntry>();
            ObjCR.GetAllCCINEntryApprovalForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<CCINEntry>)ObjCR.DBResponse.Data;
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
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
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
                Ppg_ExportRepository objER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<CartingList>)objER.DBResponse.Data;
            return PartialView(lstCartingApp);
        }
        [HttpGet]
        public ActionResult ViewCartingApp(int CartingAppId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            if (CartingAppId > 0)
                objER.DeleteCartingApp(CartingAppId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult EditCartingApp(int CartingAppId)
        {
            CartingApplication objCA = new CartingApplication();
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
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
                Ppg_ExportRepository objER = new Ppg_ExportRepository();
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


            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Ppg_CartingRegister objCR = new Ppg_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd/MM/yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<ApplicationNoDet>)objER.DBResponse.Data;


            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = (List<CHA>)objER.DBResponse.Data;
            //}

            //ExportRepository objER1 = new ExportRepository();
            //objER1.GetCCINShippingLine();
            //if (objER1.DBResponse.Data != null)
            //{
            //    ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER1.DBResponse.Data).ToString();
            //    ViewBag.ListOfShippingLine = objER1.DBResponse.Data;
            //}

            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            //objER.ListOfExporter();
            //if (objER.DBResponse.Data != null)
            //    ViewBag.ExporterList = (List<Exporter>)objER.DBResponse.Data;

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
        /*  [HttpGet]
          public ActionResult ListCartingRegister()
          {
              List<Ppg_CartingRegister> objCR = new List<Ppg_CartingRegister>();
              Ppg_ExportRepository objER = new Ppg_ExportRepository();
              objER.GetAllRegisterDetails(((Login)(Session["LoginUser"])).Uid);
              if (objER.DBResponse.Data != null)
                  objCR = (List<Ppg_CartingRegister>)objER.DBResponse.Data;

              return PartialView("ListCartingRegister", objCR);
          }*/
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<Ppg_CartingRegister> lstcart = new List<Ppg_CartingRegister>();
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetAllCartingForPage(((Login)(Session["LoginUser"])).Uid,0);
            if (objER.DBResponse.Data != null)
                lstcart = (List<Ppg_CartingRegister>)objER.DBResponse.Data;

            return PartialView(lstcart);
        }
        [HttpGet]
        public JsonResult LoadMoreCartingList(int Page)
        {
            Ppg_ExportRepository ObjCR = new Ppg_ExportRepository();
            List<Ppg_CartingRegister> LstJO = new List<Ppg_CartingRegister>();
            ObjCR.GetAllCartingForPage(((Login)(Session["LoginUser"])).Uid,Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Ppg_CartingRegister>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            Ppg_CartingRegister objCR = new Ppg_CartingRegister();
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "view");
            if (objER.DBResponse.Data != null)
                objCR = (Ppg_CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            Ppg_CartingRegister ObjCartingReg = new Ppg_CartingRegister();
            GodownRepository ObjGR = new GodownRepository();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Ppg_CartingRegister)ObjER.DBResponse.Data;
                }
            }

            //***************************************************************************
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CHAList = (List<CHA>)ObjER.DBResponse.Data;


            List<Godown> lstGodown = new List<Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ExporterList = (List<Exporter>)ObjER.DBResponse.Data;

            ObjER.GetAllCommodity();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;


            ExportRepository objER1 = new ExportRepository();
            objER1.GetCCINShippingLine();
            if (objER1.DBResponse.Data != null)
            {
                ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER1.DBResponse.Data).ToString();
                ViewBag.ListOfShippingLine = objER1.DBResponse.Data;
            }

            //***************************************************************************
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            Ppg_CartingRegister objCR = new Ppg_CartingRegister();
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (Ppg_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(Ppg_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                objCR.ApplicationDate = Convert.ToDateTime(objCR.ApplicationDate).ToString("dd/MM/yyyy");
                //List<int> lstLocation = new List<int>();
                IList<Ppg_CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Ppg_CartingRegisterDtl>>(objCR.XMLData);

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
                Ppg_ExportRepository objER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }


        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.GodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.GodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddShortCargoDetail(ShortCargoDetails objSC,int CartingRegisterId,int CartingRegisterDtlId,string ShippingBillNo)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<ShortCargoDetails> lstShortCargoDetails = new List<ShortCargoDetails>();
            objSC.CartingDate = Convert.ToDateTime(objSC.CartingDate).ToString("yyyy-MM-dd");
            lstShortCargoDetails.Add(objSC);
            objER.AddShortCargoDetail(Utility.CreateXML(lstShortCargoDetails),CartingRegisterId,CartingRegisterDtlId,ShippingBillNo);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            PPG_StuffingRequest ObjSR = new PPG_StuffingRequest();
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();

            ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<PPG_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
            //ObjER.ListOfCHA();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //{ ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
            //else
            //{ ViewBag.ShippingLineList = null; }

            //ObjER.GetForwarder();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ForwarderList = new SelectList((List<ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            //else
            //    ViewBag.ForwarderList = null;

            ObjER.GetAllContainerNo();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<PPG_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
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

            ObjSR.RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView("CreateStuffingRequest", ObjSR);
        }

        [HttpGet]
        public JsonResult ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStuffingReq(PPG_StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                IList<PPG_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<PPG_StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<PPG_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<PPG_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
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
            PPG_StuffingRequest ObjStuffing = new PPG_StuffingRequest();
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
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
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //else
            //    ViewBag.ShippingLineList = null;

            //ObjER.GetForwarder();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ForwarderList = new SelectList((List<ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            //else
            //    ViewBag.ForwarderList = null;


            if (StuffinfgReqId > 0)
            {
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_StuffingRequest)ObjER.DBResponse.Data;
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

                ObjER.GetAllContainerNo();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ContainerList = new SelectList((List<PPG_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                    ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                }
                else
                    ViewBag.ContainerList = null;


                ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.CartingRegNoList = new SelectList((List<PPG_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
                }
                else
                {
                    ViewBag.CartingRegNoList = null;
                }



            }
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            PPG_StuffingRequest ObjStuffing = new PPG_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Kdl_GetCartRegDetForStuffingReq(CartingRegisterId);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            List<PPG_StuffingRequest> LstStuffing = new List<PPG_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }
        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            PPG_ContainerStuffing ObjCS = new PPG_ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy");
            //ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.LstRequestNo = new SelectList((List<PPG_ContainerStuffing>)ObjER.DBResponse.Data, "StuffingReqId", "StuffingReqNo");
            //}
            //else
            //{
            //    ViewBag.LstRequestNo = null;
            //}
           // ObjER.ListOfCHA();
           // if (ObjER.DBResponse.Data != null)
          //  {
           //     ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
          //  }
          //  else
          //  {
          //      ViewBag.CHAList = null;
           // }
            //ObjER.ListOfExporter();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
            //}
            //else
            //{
            //    ViewBag.ListOfExporter = null;
            //}
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}
            return PartialView("/" + "/Areas/Export/Views/Ppg_CWCExport/CreateContainerStuffing.cshtml", ObjCS);
        }



        [HttpGet]
        public JsonResult GetRequestNo()
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
           
            ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo =Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data).ToString();
             }
            else
            {
                ViewBag.LstRequestNo = null;
            }
            return Json(ViewBag.LstRequestNo, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetShippingLineForStuffing()
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();

                    
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            return Json(ViewBag.ShippingLineList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetExporterForStuffing()
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();

            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfExporter = null;
            }
           
            return Json(ViewBag.ListOfExporter, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHANameForStuffing()
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();

            ObjER.ListOfCHA();
             if (ObjER.DBResponse.Data != null)
              {
                 ViewBag.CHAList = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
              }
              else
              {
                  ViewBag.CHAList = null;
             }
            
            return Json(ViewBag.CHAList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<PPG_ContainerStuffing> LstStuffing = new List<PPG_ContainerStuffing>();
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("/Areas/Export/Views/Ppg_CWCExport/ContainerStuffingList.cshtml", LstStuffing);
        }
        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            PPG_ContainerStuffing ObjStuffing = new PPG_ContainerStuffing();
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (PPG_ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            PPG_ContainerStuffing ObjStuffing = new PPG_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_ContainerStuffing)ObjER.DBResponse.Data;
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
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(PPG_ContainerStuffing ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                string GREOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GREOperationCFSCodeWiseAmt != null)
                {
                    List<GREOperationCFSCodeWiseAmt> LstStuffingGRE1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREOperationCFSCodeWiseAmt>>(ObjStuffing.GREOperationCFSCodeWiseAmt.ToString());
                    GREOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGRE1);
                }

                string GREContainerWiseAmtXML = "";
                if (ObjStuffing.GREContainerWiseAmt != null)
                {
                    List<GREContainerWiseAmt> LstStuffingGRE2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREContainerWiseAmt>>(ObjStuffing.GREContainerWiseAmt.ToString());
                    GREContainerWiseAmtXML = Utility.CreateXML(LstStuffingGRE2);
                }

                string INSOperationCFSCodeWiseAmtLstXML = "";
                if (ObjStuffing.INSOperationCFSCodeWiseAmt != null)
                {
                    List<INSOperationCFSCodeWiseAmt> LstStuffingINS1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSOperationCFSCodeWiseAmt>>(ObjStuffing.INSOperationCFSCodeWiseAmt.ToString());
                    INSOperationCFSCodeWiseAmtLstXML = Utility.CreateXML(LstStuffingINS1);
                }

                string INSContainerWiseAmtXML = "";
                if (ObjStuffing.INSContainerWiseAmt != null)
                {
                    List<INSContainerWiseAmt> LstStuffingINS2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSContainerWiseAmt>>(ObjStuffing.INSContainerWiseAmt.ToString());
                    INSContainerWiseAmtXML = Utility.CreateXML(LstStuffingINS2);
                }

                string STOContainerWiseAmtXML = "";
                if (ObjStuffing.STOinvoicecargodtl != null)
                {
                    List<STOinvoicecargodtl> LstStuffingSTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOinvoicecargodtl>>(ObjStuffing.STOinvoicecargodtl.ToString());
                    STOContainerWiseAmtXML = Utility.CreateXML(LstStuffingSTO2);
                }
                string STOOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.STOOperationCFSCodeWiseAmt != null)
                {
                    List<STOOperationCFSCodeWiseAmt> LstStuffingSTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOOperationCFSCodeWiseAmt>>(ObjStuffing.STOOperationCFSCodeWiseAmt.ToString());
                    STOOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingSTO1);
                }

                string HNDOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.HNDOperationCFSCodeWiseAmt != null)
                {
                    List<HNDOperationCFSCodeWiseAmt> LstStuffingHND = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HNDOperationCFSCodeWiseAmt>>(ObjStuffing.HNDOperationCFSCodeWiseAmt.ToString());
                    HNDOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingHND);
                }

                string GENSPOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GENSPOperationCFSCodeWiseAmt != null)
                {
                    List<GENSPOperationCFSCodeWiseAmt> LstStuffingGENSP = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GENSPOperationCFSCodeWiseAmt>>(ObjStuffing.GENSPOperationCFSCodeWiseAmt.ToString());
                    GENSPOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGENSP);
                }

                string ShippingBillAmtXML = "";
                if (ObjStuffing.PPG_ShippingBillAmt != null)
                {
                    List<PPG_ShippingBillNo> LstShippingBill = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNo>>(ObjStuffing.PPG_ShippingBillAmt.ToString());
                    ShippingBillAmtXML = Utility.CreateXML(LstShippingBill);
                }

                string ShippingBillAmtGenXML = "";
                if (ObjStuffing.PPG_ShippingBillAmtGen != null)
                {
                    List<PPG_ShippingBillNoGen> LstShippingBillGen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoGen>>(ObjStuffing.PPG_ShippingBillAmtGen.ToString());
                    ShippingBillAmtGenXML = Utility.CreateXML(LstShippingBillGen);
                }
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                  INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML);
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
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            PPG_ContainerStuffing ObjStuffing = new PPG_ContainerStuffing();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PPG_ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuff(PPG_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "",CargoType="", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "", Area = "", PortName = "", PortDestination = "", Remarks = "" ,chargetype="",total="";

            String Consignee = ""; int SerialNo = 1;
            if (ObjStuffing.LstppgStuffingDtl.Count() > 0)
            {
                ObjStuffing.LstppgStuffingDtl.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    ShippingBillNo = (ShippingBillNo == "" ? ((item.ShippingBillNo) + " ") : (item.ShippingBillNo + "<br/>" + item.ShippingBillNo + " "));
                    /*   if (ShippingBillNo == "")
                           ShippingBillNo = item.ShippingBillNo + " ";
                       else
                           ShippingBillNo += "," + item.ShippingBillNo; */
                });

                ObjStuffing.LstppgStuffingDtl.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
                {

                    ShippingDate = (ShippingDate == "" ? (item.ShippingDate) : (item.ShippingDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { EntryNo = x.EntryNo }).Distinct().ToList().ForEach(item =>
                {

                    EntryNo = (EntryNo == "" ? (item.EntryNo) : (item.EntryNo));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
                {

                    InDate = (InDate == "" ? (item.InDate) : (item.InDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { PortName = x.PortName }).Distinct().ToList().ForEach(item =>
                {
                    if (PortName == "")
                        PortName = item.PortName;
                    else
                        PortName += "," + item.PortName;
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });

                ObjStuffing.LstppgStuffingDtl.Select(x => new { Consignee = x.Consignee }).Distinct().ToList().ForEach(item =>
                {
                    if (Consignee == "")
                        Consignee = item.Consignee;
                    else
                        Consignee += "," + item.Consignee;
                });

                ObjStuffing.LstppgStuffingDtl.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                {

                    if (ShippingLine == "")
                        ShippingLine = item.ShippingLine;
                    else
                        ShippingLine += "," + item.ShippingLine;
                });

                ObjStuffing.LstppgStuffingDtl.Select(x => new { Remarks = x.Remarks }).Distinct().ToList().ForEach(item =>
                {

                    if (Remarks == "")
                        Remarks = item.Remarks;
                    else
                        Remarks += "," + item.Remarks;
                });
                ObjStuffing.LstppgStuffingDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                {
                    if (CHA == "")
                        CHA = item.CHA;
                    else
                        CHA += "," + item.CHA;
                });
                ObjStuffing.LstppgCharge.Select(x => new { chargetype = x.chargetype }).Distinct().ToList().ForEach(item =>
                {
                    if (chargetype == "")
                        chargetype = item.chargetype;
                    else
                        chargetype += "," + item.chargetype;
                });
                ObjStuffing.LstppgCharge.Select(x => new { total = x.total }).Distinct().ToList().ForEach(item =>
                {
                    if (total == "")
                        total = item.total;
                    else
                        total += "," + item.total;
                });
                //StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                //Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                StuffQuantity = (ObjStuffing.LstppgStuffingDtl.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity)).ToString() : "";
                ObjStuffing.LstppgStuffingDtl.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                   // CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                   // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                    CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : CustomSeal == item.CustomSeal ? CustomSeal : (CustomSeal + "<br/>" + item.CustomSeal));
                    Commodity = (Commodity == "" ? (item.CommodityName) : Commodity == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));
                    //SerialNo++;
                });
                //SLNo.Remove(SLNo.Length - 1);
                ObjStuffing.LstppgShipDtl.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo +'-'+item.CargoType : (ContainerNo + '-' + item.CargoType + "<br/>" + item.ContainerNo + '-' + item.CargoType));
                     ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));

                     CargoType = (CargoType == "" ? (item.CargoType) :(CargoType + "<br/>" + item.CargoType));

                    //SerialNo++;
                });


                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";

                Html += "<thead>";

                Html += "<tr><td colspan='4'>";
                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
                Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
                Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD PATPARGANJ DELHI</span><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER STUFFING SHEET</label><br/><label style='font-size: 14px;'> <b>Shed No :</b> " + ObjStuffing.GodownName + "</label></td>";
                Html += "<td width='12%' align='right' valign='top'>";
                Html += "<table style='width:100%;' cellspacing='0' cellpadding='0' valign='top'><tbody>";
                Html += "<tr><td style='border:1px solid #333;' valign='top'>";
                Html += "<div valign='top' style='padding: 5px 0; font-size: 12px; text-align: center;'>F/ICDPPG/09</div>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</thead>";

                Html += "<tbody>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                /*Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='8%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='8%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No & Type:</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>ICD Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size :</b> <u>" + ObjStuffing.Size + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Forwarder Name :</b> <u>" + ObjStuffing.ForwarderName + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='1' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line </b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";
                */

                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='8%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='8%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>ICD Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Cont Type :</b> <u>" + CargoType + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Forwarder Name :</b> <u>" + ObjStuffing.ForwarderName + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size</b> <u>" + ObjStuffing.Size + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line </b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";



                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='8' style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>S. No.</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Entry No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>InDate</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb Date</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Exporter</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>Comdty</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Pkts</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Gr Wt</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>FOB</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Area</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Remarks</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                ObjStuffing.LstppgStuffingDtl.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>" + SerialNo++ + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.EntryNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.InDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingBillNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.Exporter + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.CommodityName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffQuantity + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.StuffWeight + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Fob + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.Area + "</td>";
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
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.StuffQuantity) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.StuffWeight) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.Fob) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'></th>";
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
                Html += "<td colspan='4' width='33.33333333333333%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>The container is allowed to be moved to gateway ports</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td colspan='3' width='25%' style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td colspan='3' width='25%' style='text-align:left;'>Shed Asst. <br/> ICD PATPARGANJ</td><td colspan='3' width='25%' style='text-align:left;'>Shed I/C <br/> ICD PATPARGANJ</td><td colspan='3' width='25%' style='text-align:center;'>Customs <br/> ICD PATPARGANJ</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><span><br/></span></td></tr>";

                ObjStuffing.LstppgCharge.ToList().ForEach(item =>
                {
                    Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + item.chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>" + item.total + "</label></td> <td colspan='3' width='25%' style='text-align:center;'>" + item.Invoiceno + "</td> <td colspan='3' width='25%' style='text-align:left;'>" + item.InvoiceDate + "</td><td colspan='3' width='25%' style='text-align:left;'>" + item.eximtraderalias + "</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                });
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
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
        //public string GeneratePdfForContainerStuff(PPG_ContainerStuffing ObjStuffing, int ContainerStuffingId)
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
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
            objImport.ListOfGREParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                ((List<PPG_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GREPartyId = item.GREPartyId, GREPartyCode = item.GREPartyCode });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGroundRentEmpty(String StuffingDate, String ArrayOfContainer, int GREPartyId)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGroundRentEmpty(StuffingDate, ContainerStuffingXML, GREPartyId);
            PPG_ContainerStuffing objImp = new PPG_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfINSParty()
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
            objImport.ListOfINSParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { INSPartyId = item.INSPartyId, INSPartyCode = item.INSPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateINS(String StuffingDate, String ArrayOfContainer, int INSPartyId)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }
            objImport.CalculateINS(StuffingDate, ContainerStuffingXML, INSPartyId);
            PPG_ContainerStuffing objImp = new PPG_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfSTOParty()
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
            objImport.ListOfSTOParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { STOPartyId = item.STOPartyId, STOPartyCode = item.STOPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateSTO(String StuffingDate, String ArrayOfContainer, int STOPartyId)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateSTO(StuffingDate, ContainerStuffingXML, STOPartyId);
            PPG_ContainerStuffing objImp = new PPG_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfHandalingParty()
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
            objImport.ListOfHandalingParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { HandalingPartyId = item.HandalingPartyId, HandalingPartyCode = item.HandalingPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateHandaling(String StuffingDate, String Origin, String Via, String ArrayOfContainer, int HandalingPartyId,String  CargoType)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateHandaling(StuffingDate, Origin, Via, ContainerStuffingXML, HandalingPartyId, CargoType);
            PPG_ContainerStuffing objImp = new PPG_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListOfGENSPParty()
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
            objImport.ListOfGENSPParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { GENSPPartyId = item.GENSPPartyId, GENSPPartyCode = item.GENSPPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGENSP(String StuffingDate, String ArrayOfContainer, int GENSPPartyId)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGENSP(StuffingDate, ContainerStuffingXML, GENSPPartyId);
            PPG_ContainerStuffing objImp = new PPG_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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



            //ObjIR.GetContainerForMovement();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.LstContainerNo = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "ContainerStuffingId", "Container");
            //}
            //else
            //{
            //    ViewBag.LstRequestNo = null;
            //}
            //ObjIR.GetShippingLine();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //}
            //else
            //{
            //    ViewBag.ShippingLineList = null;
            //}

            //ObjIR.GetPaymentParty();
            //if (ObjIR.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            //ObjIR.ListOfGodown();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            //}
            //else
            //{
            //    ViewBag.ListOfGodown = null;
            //}


            //ObjIR.GetLocationForInternalMovement();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.LocationNoList = new SelectList((List<PPG_ContainerMovement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
            //}
            //else
            //{
            //    ViewBag.LocationNoList = null;
            //}
            return PartialView();
        }



        [HttpGet]
        public ActionResult GetInternalMovementList()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
            ObjIR.GetAllInternalMovement();
            List<PPG_ContainerMovement> LstMovement = new List<PPG_ContainerMovement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_ContainerMovement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public JsonResult GetContainerNoForMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();

            ObjIR.GetContainerForMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LstContainerNo = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            else
            {
                ViewBag.LstContainerNo = null;
            }
           
            return Json(ViewBag.LstContainerNo, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetLocationForMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();



            ObjIR.GetLocationForInternalMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.LocationNoList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            else
            {
                ViewBag.LocationNoList = null;
            }
           

            return Json(ViewBag.LocationNoList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShippingForMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();

            ObjIR.GetShippingLine();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }



          

            return Json(ViewBag.ShippingLineList, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetPartyForMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
            ObjIR.GetPaymentParty();
            if (ObjIR.DBResponse.Status > 0)
               ViewBag.PaymentParty = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            else
               ViewBag.PaymentParty = null;
           




            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int portvalue,decimal tareweight,string cargotype, int InvoiceId = 0)
        {

            Ppg_ExportRepository objChrgRepo = new Ppg_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(ContainerStuffingDtlId, ContainerStuffingId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, ctype, portvalue, tareweight,cargotype, InvoiceId);

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
            Ppg_ExportRepository objGPR = new Ppg_ExportRepository();
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
                string ChargesBreakupXML = "";
                string ChargesBreakupXMLL = "";
                string ChargesBreakupXMLLL = "";
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

                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                if (invoiceDataa.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrgBreakup);
                }
                if (invoiceDataaa.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrgBreakup);
                }

                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditInvoiceMovement(invoiceData, invoiceDataa, invoiceDataaa, ContainerXML, ContainerXMLL, ContainerXMLLL, ChargesXML, ChargesXMLL, ChargesXMLLL, ContWiseCharg, ContWiseChargg, ContWiseCharggg, OperationCfsCodeWiseAmtXML, OperationCfsCodeWiseAmtXMLL, OperationCfsCodeWiseAmtXMLLL, ChargesBreakupXML, ChargesBreakupXMLL, ChargesBreakupXMLLL, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPMovement");
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
                Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();

            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }


        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            //Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            //objExport.GetLoadedContainerRequestForPaymentSheet();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.StuffingReqList = null;

            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public JsonResult GetRequestNoForLoadedContainer()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetLoadedContainerRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;


            return Json(ViewBag.StuffingReqList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPartyForLoadedContainer()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetLoadedPaymentSheetContainer(int StuffingReqId)
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetContainerForLoadedContainerPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,int PartyId,string SEZ,int LocationId, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            String[] dt = InvoiceDate.Split(' ');

            Ppg_ExportRepository objChrgRepo = new Ppg_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, dt[0], InvoiceType, ContainerXML, PayeeId,PartyId, LocationId, InvoiceId,SEZ);

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

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPLod";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
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



            return Json(Output, JsonRequestBehavior.AllowGet);

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
                string SEZ = Convert.ToString(objForm["SEZ1"]);
                var invoiceData = JsonConvert.DeserializeObject<PPG_MovementInvoice>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditInvoiceContLoaded(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod", SEZ,"");

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
        public JsonResult AddEditLoadedContPaymentSheetForEdit(Export.Models.PPG_MovementInvoice objForm)
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

                //      var invoiceData = JsonConvert.DeserializeObject<PPG_MovementInvoice>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceData = objForm;

              string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditInvoiceContLoaded(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod","");

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
        public JsonResult LoadContInvoicePrint(string InvoiceNo)
        {
            Ppg_ExportRepository objGPR = new Ppg_ExportRepository();
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
        }

        [HttpGet]
        public ActionResult ListOfContainerLoadedInv(int Page = 0)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.ListOfLoadedInv(Page);
            List<Ppg_ContMovementList> LstMovement = new List<Ppg_ContMovementList>();
            if (objER.DBResponse.Data != null)
                LstMovement = ((List<Ppg_ContMovementList>)objER.DBResponse.Data);
            if (Page == 0) return PartialView(LstMovement);
            else return Json(LstMovement, JsonRequestBehavior.AllowGet);
        }



        #endregion


         
        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objER.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objER.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objER.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }




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
            objER.GetAllCommodityForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            Ppg_ExportRepositoryV2 ObjER = new Ppg_ExportRepositoryV2();


            ObjER.ListOfPackUQCForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
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
            Ppg_ExportRepository ObjRR = new Ppg_ExportRepository();
            PPG_LoadContReq ObjContReq = new PPG_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (PPG_LoadContReq)ObjRR.DBResponse.Data;
            }
          
         
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            Ppg_ExportRepository ObjRR = new Ppg_ExportRepository();
            PPG_LoadContReq ObjContReq = new PPG_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (PPG_LoadContReq)ObjRR.DBResponse.Data;
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
            Ppg_ExportRepositoryV2 ObjER = new Ppg_ExportRepositoryV2();


            ObjER.ListOfPackUQCForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            objER.ListOfLoadCont();
            if (objER.DBResponse.Data != null)
                lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(PPG_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                Ppg_ExportRepository objER = new Ppg_ExportRepository();

                List<Ppg_LoadContReqDtl> LstLoadContReqDtl = JsonConvert.DeserializeObject<List<Ppg_LoadContReqDtl>>(objReq.StringifyData);

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

                string XML = "";
                if (objReq.StringifyData != null)
                {
                    //XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<Ppg_LoadContReqDtl>>(objReq.StringifyData));
                    XML = Utility.CreateXML(LstLoadContReqDtl);
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
                Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
                ObjER.DelLoadContReqhdr(LoadContReqId);
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
        public JsonResult PrintLoadedRequest(int LoadContReqId)
        {
            Ppg_ExportRepository ObjER = new Ppg_ExportRepository();
            PPG_LoadContReq ObjContReq = new PPG_LoadContReq();
            ObjER.GetLoadedContPrint(LoadContReqId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjContReq = (PPG_LoadContReq)ObjER.DBResponse.Data;
                string Path = GeneratePdfForLoadedCont(ObjContReq, LoadContReqId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForLoadedCont(PPG_LoadContReq objDet, int LoadContReqId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/LoadedRequest" + LoadContReqId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingBillDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Size = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CargoType = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "", Area = "", PortName = "", PortDestination = "", Remarks = "", chargetype = "", total = "";

            String Consignee = ""; int SerialNo = 1;
            
            if (objDet.lstppgContDtl.Count() > 0)
            {
                objDet.lstppgContDtl.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    ShippingBillNo = (ShippingBillNo == "" ? ((item.ShippingBillNo) + " ") : (item.ShippingBillNo + "<br/>" + item.ShippingBillNo + " "));
                    /*   if (ShippingBillNo == "")
                           ShippingBillNo = item.ShippingBillNo + " ";
                       else
                           ShippingBillNo += "," + item.ShippingBillNo; */
                });
                objDet.lstppgContDtl.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
                {
                    if (item.CargoType == 1)
                        CargoType = (CargoType == "" ? "Haz" : CargoType + ",Haz");
                    else if (item.CargoType == 2)
                        CargoType = (CargoType == "" ? "Non-Haz" : CargoType + ",Non-Haz");
                });
                objDet.lstppgContDtl.Select(x => new { ShippingBillDate = x.ShippingBillDate }).Distinct().ToList().ForEach(item =>
                {

                    ShippingBillDate = (ShippingBillDate == "" ? (item.ShippingBillDate) : (item.ShippingBillDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
              /*  ObjStuffing.LstppgStuffingDtl.Select(x => new { EntryNo = x.EntryNo }).Distinct().ToList().ForEach(item =>
                {

                    EntryNo = (EntryNo == "" ? (item.EntryNo) : (item.EntryNo));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
               /* });*/
              /*  ObjStuffing.LstppgStuffingDtl.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
                {

                    InDate = (InDate == "" ? (item.InDate) : (item.InDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
             /*   });*/
                objDet.lstppgContDtl.Select(x => new { Exporter = x.ExporterName }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                objDet.lstppgContDtl.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
                {
                    if (Size == "")
                        Size = item.Size;
                    else
                        Size += "," + item.Size;
                });
                objDet.lstppgContDtl.Select(x => new { CustomSeal = x.CustomSeal }).Distinct().ToList().ForEach(item =>
                {
                    if (CustomSeal == "")
                        CustomSeal = item.CustomSeal;
                    else
                        CustomSeal += "," + item.CustomSeal;
                });
                /*  ObjStuffing.LstppgStuffingDtl.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                  {
                      if (PortDestination == "")
                          PortDestination = item.PortDestination;
                      else
                          PortDestination += "," + item.PortDestination;
                  });*/

                /*   ObjStuffing.LstppgStuffingDtl.Select(x => new { Consignee = x.Consignee }).Distinct().ToList().ForEach(item =>
                   {
                       if (Consignee == "")
                           Consignee = item.Consignee;
                       else
                           Consignee += "," + item.Consignee;
                   });

                   ObjStuffing.LstppgStuffingDtl.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                   {

                       if (ShippingLine == "")
                           ShippingLine = item.ShippingLine;
                       else
                           ShippingLine += "," + item.ShippingLine;
                   });*/

                /*  ObjStuffing.LstppgStuffingDtl.Select(x => new { Remarks = x.Remarks }).Distinct().ToList().ForEach(item =>
                  {

                      if (Remarks == "")
                          Remarks = item.Remarks;
                      else
                          Remarks += "," + item.Remarks;
                  });
                  ObjStuffing.LstppgStuffingDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                  {
                      if (CHA == "")
                          CHA = item.CHA;
                      else
                          CHA += "," + item.CHA;
                  });
                  ObjStuffing.LstppgCharge.Select(x => new { chargetype = x.chargetype }).Distinct().ToList().ForEach(item =>
                  {
                      if (chargetype == "")
                          chargetype = item.chargetype;
                      else
                          chargetype += "," + item.chargetype;
                  });
                  ObjStuffing.LstppgCharge.Select(x => new { total = x.total }).Distinct().ToList().ForEach(item =>
                  {
                      if (total == "")
                          total = item.total;
                      else
                          total += "," + item.total;
                  });*/
                //StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                //Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                //  StuffQuantity = (ObjStuffing.LstppgStuffingDtl.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity)).ToString() : "";
                /* ObjStuffing.LstppgStuffingDtl.ToList().ForEach(item =>
                 {
                     //SLNo = SLNo + SerialNo + "<br/>";
                     // CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                objDet.lstppgContDtl        // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                     CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : CustomSeal == item.CustomSeal ? CustomSeal : (CustomSeal + "<br/>" + item.CustomSeal));
                     Commodity = (Commodity == "" ? (item.CommodityName) : Commodity == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));
                     //SerialNo++;
                 });*/
                //SLNo.Remove(SLNo.Length - 1);
                objDet.lstppgContDtl.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                   // CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo +'-'+item.CargoType : (ContainerNo + '-' + item.CargoType + "<br/>" + item.ContainerNo + '-' + item.CargoType));
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));

                   // CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));

                    //SerialNo++;
                });


                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";

                Html += "<thead>";

                Html += "<tr><td colspan='4'>";
                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
                Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
                Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD PATPARGANJ DELHI</span><br/><label style='font-size: 14px; font-weight:bold;'>LOADED CONTAINER REQUEST SHEET</label><br/><label style='font-size: 14px;'></label></td>";
                Html += "<td width='12%' align='right' valign='top'>";
                Html += "<table style='width:100%;' cellspacing='0' cellpadding='0' valign='top'><tbody>";
                Html += "<tr><td style='border:1px solid #333;' valign='top'>";
                Html += "<div valign='top' style='padding: 5px 0; font-size: 12px; text-align: center;'>F/ICDPPG/09</div>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</thead>";

                Html += "<tbody>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                /*Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='8%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='8%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No & Type:</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>ICD Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size :</b> <u>" + ObjStuffing.Size + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Forwarder Name :</b> <u>" + ObjStuffing.ForwarderName + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='1' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line </b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";
                */

                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='10' width='20%' style='padding:3px;text-align:left;'>Container / CBT Request No :</th><td colspan='10' width='40%' style='padding:3px;text-align:left;'>" + objDet.LoadContReqNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Container / CBT Request  Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + objDet.LoadContReqDate + "</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>CHA Name :</b> <u>" + objDet.CHAName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Final Destination :</b> <u>" + objDet.FinalDestinationLocation + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>CargoType </b> <u>" + CargoType + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Remarks :</b> <u>" + objDet.Remarks + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Movement</b> <u>" + objDet.Movement + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size</b> <u>" + Size + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Custom Examination Type </b> <u>" + objDet.CustomExaminationType + "</u></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Via :</b> <u>" + objDet.Via + "</u></td>   <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Origin :</b> <u>"+  objDet.Origin + " </u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> </tr></tbody></table> </td></tr>";



                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='8' style='border:1px solid #000;width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>S. No.</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb No</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Sb Date</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Exporter</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Shippingline</th>";
                // Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>CustomSeal</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Package Type</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>PACKUQC</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Gr Wt</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Pkts</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>FOB</th>";

              //  Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Cargo Type</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Equipment Seal Type</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Equipment Status</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Equipment QUC</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Cargo Description</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>Container Load Type</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                objDet.lstppgContDtl.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;padding:3px;text-align:center;width:5%;'>" + SerialNo++ + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingBillNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingBillDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ExporterName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingLineName + "</td>";
                    // Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.CustomSeal + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.PackageType + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:15%;'>" + item.PackUQCDescription + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.GrossWt + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.NoOfUnits + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.FobValue + "</td>";
        
                   // Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + CargoType + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.EquipmentSealType + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.EquipmentStatus + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.EquipmentQUC + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.CargoDescription + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.ContLoadType + "</td></tr>";
                });
                //LOOP END

                Html += "<tr><th style='padding:3px;text-align:center;width:5%;'>Total :</th>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:8%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<td style='padding:3px;text-align:left;width:15%;'></td>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + objDet.lstppgContDtl.AsEnumerable().Sum(item => item.GrossWt) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + objDet.lstppgContDtl.AsEnumerable().Sum(item => item.NoOfUnits) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + objDet.lstppgContDtl.AsEnumerable().Sum(item => item.FobValue) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'></th>";
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
                Html += "<td colspan='4' width='33.33333333333333%' style='padding:3px;text-align:left;font-size:11px;' valign='top'>The container is allowed to be moved to gateway ports</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td colspan='3' width='25%' style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td colspan='3' width='25%' style='text-align:left;'>Shed Asst. <br/> ICD PATPARGANJ</td><td colspan='3' width='25%' style='text-align:left;'>Shed I/C <br/> ICD PATPARGANJ</td><td colspan='3' width='25%' style='text-align:center;'>Customs <br/> ICD PATPARGANJ</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><span><br/></span></td></tr>";
                Html += "</tbody></table>";
                /*  ObjStuffing.LstppgCharge.ToList().ForEach(item =>
                  {
                      Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + item.chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>" + item.total + "</label></td> <td colspan='3' width='25%' style='text-align:center;'>" + item.Invoiceno + "</td> <td colspan='3' width='25%' style='text-align:left;'>" + item.InvoiceDate + "</td><td colspan='3' width='25%' style='text-align:left;'>" + item.eximtraderalias + "</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                  });*/
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + chargetype + " <span>&nbsp;&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>Dynamic</label></td> <td colspan='3' width='25%' style='text-align:center;'>Dynamic</td> <td colspan='3' width='25%' style='text-align:left;'>" + total + "</td><td colspan='3' width='25%' style='text-align:left;'>Dynamic</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
                // Html += "</tbody></table>";


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
            return "/Docs/" + Session.SessionID + "/LoadedRequest" + LoadContReqId + ".pdf";
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
            List<BTTCargoEntry> lstBTTCargoEntry = new List<BTTCargoEntry>();
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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

            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

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
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

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
        public JsonResult GetCartingDetailList(int CartingId,string Flag)
        {
            try
            {
                Ppg_ExportRepository objExport = new Ppg_ExportRepository();
                if (CartingId > 0)
                    objExport.GetCartingDetailList(CartingId, Flag);
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
                Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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
                    Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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

        #region BTT Invoice
        [HttpGet]
        public ActionResult CreateBTTPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            //Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            //objExport.GetCartingApplicationForPaymentSheet();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.StuffingReqList = null;

            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;


            return PartialView();
        }



        [HttpGet]
        public JsonResult GetStuffindNoForBTT()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetCartingApplicationForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            return Json(ViewBag.StuffingReqList, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetPartyForBTT()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetPaymentSheetShipBillNo(int StuffingReqId)
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetShipBillForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,int PartyId,
            List<PaymentSheetContainer> lstPaySheetContainer,
            int InvoiceId = 0)
        {
            //AppraisementId ----> StuffingReqID

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Ppg_ExportRepository objPpgRepo = new Ppg_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetBTTPaymentSheet(InvoiceDate, AppraisementId, TaxType, PartyId,XMLText, InvoiceId);
            var Output = (PpgInvoiceBTT)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new PpgPostPaymentContainerBTT
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
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditBTTPaymentSheet(PpgInvoiceBTT objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string CargoXML = "";
                string ChargesBreakupXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }

                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditBTTInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", CargoXML);

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

        #region Export Destuffing
        [HttpGet]
        public ActionResult CreateExportDestuffing(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            Ppg_EntryThroughGateRepository objImport = new Ppg_EntryThroughGateRepository();
            Ppg_ExportRepository objER = new Ppg_ExportRepository();

            //Shipping Line List----------------------------------------------------------------
            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Data != null)
            {
                ViewBag.ShippingLineList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            //CHA List-------------------------------------------------------------------------
            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
                ViewBag.CHAList = JsonConvert.SerializeObject(objER.DBResponse.Data);
            else
                ViewBag.CHAList = null;

            //Party List----------------------------------------------------------------------
            objER.GetPaymentParty();
            if (objER.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objER.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            //Containers List-----------------------------------------------------------------
            objER.GetContainersForExpDestuffing();
            if (objER.DBResponse.Status > 0)
                ViewBag.ContainersList = JsonConvert.SerializeObject(objER.DBResponse.Data);
            else
                ViewBag.ContainersList = null;



            return PartialView();
        }

        [HttpGet]
        public JsonResult GetChargesExportDestuffing(int ContainerStuffingId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            ExportDestuffing obj = new ExportDestuffing();
            objER.GetChargesForExpDestuffing(ContainerStuffingId);
            if (objER.DBResponse.Status > 0)
            {
                obj.lstCharges = (List<ExportDestuffingCharges>)objER.DBResponse.Data;
                obj.ContainerStuffingId = ContainerStuffingId;
                obj.Total = obj.lstCharges.Sum(o => o.TotalAmount);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetExportDestuffingPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,
            List<PaymentSheetContainer> lstPaySheetContainer,
            int InvoiceId = 0)
        {
            //AppraisementId ----> ContainerStuffingDtlId

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Ppg_ExportRepository objPpgRepo = new Ppg_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetExportDestuffingPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId);
            var Output = (PpgInvoiceExpDestuf)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPDestuf";
            Output.PayeeId = Output.PartyId;
            Output.PayeeName = Output.PartyName;
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
                    Output.lstPostPaymentCont.Add(new PpgPostPaymentContainerExpDestuf
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
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditExportDestuffing(PpgInvoiceExpDestuf objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string CargoXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                //if (invoiceData.lstPreInvoiceCargo != null)
                //{
                //    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                //}
                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditExpDestufInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId,
                    ((Login)(Session["LoginUser"])).Uid, "EXPDestuf", CargoXML);

                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data).Split(',')[0];
                invoiceData.ExportDestuffingNo = Convert.ToString(objChargeMaster.DBResponse.Data).Split(',')[1];

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

        #region Cargo Shifting
        public ActionResult CreateCargoShifting(string type = "Tax")
        {
            //--------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //-------------------------------------------------------------------------------
            ViewData["InvType"] = type;
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            //--------------------------------------------------------------------------------
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            //objExport.GetShippingLineForInvoice();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.ShippingLine = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.ShippingLine = null;
            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------
            //GodownRepository ObjGR = new GodownRepository();
            //List<Godown> lstGodown = new List<Godown>();

            //ObjGR.GetAllGodown();
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstGodown = (List<Godown>)ObjGR.DBResponse.Data;
            //}
            //ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);

            //objExport.getOnlyRightsGodown();
            //List<Godown> lstGodownF = new List<Godown>();
            //if (objExport.DBResponse.Data != null)
            //{
            //    lstGodownF = (List<Godown>)objExport.DBResponse.Data;
            //}
            //ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);
            //-------------------------------------------------------------------------------

            return PartialView();
        }


        [HttpGet]
        public JsonResult GetShippingForCargo()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetShippingLineForInvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.ShippingLine = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ShippingLine = null;

            return Json(ViewBag.ShippingLine, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetFromGodownForCargo()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.getOnlyRightsGodown();
           // GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();

           // ObjGR.GetAllGodown();
            if (objExport.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objExport.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);

            

            return Json(ViewBag.GodownList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetToGodownForCargo()
        {
            //Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            //objExport.getOnlyRightsGodown();
            List<Godown> lstGodownF = new List<Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodownF = (List<Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);

            return Json(ViewBag.GodownListF, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPartyForCargo()
        {
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
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
        public JsonResult GetCargoShiftingPaymentSheet(string InvoiceDate, int ShippingLineId, string TaxType, int PayeeId,
            List<CargoShiftingShipBillDetails> lstShipbills,
            //string Shipbills,
            int InvoiceId = 0)
        {

            //List<CargoShiftingShipBillDetails> lstShipbills = new List<CargoShiftingShipBillDetails>();
            //var lstShipbills1 = Newtonsoft.Json.JsonConvert.DeserializeObject<CargoShiftingShipBillDetails>(Shipbills);
            string XMLText = "";
            if (lstShipbills != null)
            {
                XMLText = Utility.CreateXML(lstShipbills.Where(o => o.IsChecked == true).ToList());
            }
            Ppg_ExportRepository objPpgRepo = new Ppg_ExportRepository();
            objPpgRepo.GetCargoShiftingPaymentSheet(InvoiceDate, ShippingLineId, XMLText, InvoiceId, TaxType, PayeeId);
            var Output = (PpgInvoiceCargoShifting)objPpgRepo.DBResponse.Data;

            Output.InvoiceType = TaxType;
            Output.InvoiceDate = Convert.ToDateTime(InvoiceDate).ToString("dd/MM/yyyy HH:mm");
            Output.TotalNoOfPackages = lstShipbills.Where(o => o.IsChecked == true).Sum(o => (int)o.ActualQty);
            Output.TotalGrossWt = lstShipbills.Where(o => o.IsChecked == true).Sum(o => o.ActualWeight);
            Output.TotalSpaceOccupied = lstShipbills.Where(o => o.IsChecked == true).Sum(o => o.SQM);
            Output.TotalSpaceOccupiedUnit = "SQM";
            Output.TotalWtPerUnit = (Output.TotalNoOfPackages == 0) ? 0 : (Output.TotalGrossWt) / (Output.TotalNoOfPackages);

            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddEditCargoShifting(PpgInvoiceCargoShifting objForm, List<CargoShiftingShipBillDetails> lstShipbills,
            int FromGodownId, int ToGodownId, int ToShippingId, int ShiftingType, int FromShippingLineId)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string CartingRgisterDtlXML = "";
                string ChargesXML = "";
                string ChargesBreakXML = "";
                string OperationCfsCodeWiseAmtXML = "";
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (lstShipbills.Count > 0)
                {
                    CartingRgisterDtlXML = Utility.CreateXML(lstShipbills.Where(x => x.IsChecked == true).Select(x => x.CartingRegisterDtlId).ToList());
                }
                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
                objChargeMaster.AddEditCargoShiftInvoice(invoiceData, ChargesXML, ChargesBreakXML, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid,
                    CartingRgisterDtlXML, FromGodownId, ToGodownId, ToShippingId, ShiftingType, FromShippingLineId);

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Tentative Invoice(Container Movement)
        [HttpGet]
        public ActionResult CreateTentativeContainerMovement()
        {
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objChrgRepo = new Ppg_ExportRepository();
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
                Ppg_ExportRepository objChargeMaster = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objGPR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjRR = new Ppg_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
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
                                        objTrainSummaryUpload.ForeignLiner = Convert.ToString(dr["ForeignLiner"]);
                                        objTrainSummaryUpload.VesselName = Convert.ToString(dr["VesselName"]);
                                        objTrainSummaryUpload.VesselNo = Convert.ToString(dr["VesselNo"]);
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

                                Ppg_ExportRepository objImport = new Ppg_ExportRepository();
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
                Ppg_ExportRepository objImport = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<ppgExportTrainSummaryUpload> lstCargoSeize = new List<ppgExportTrainSummaryUpload>();
            objER.ListOfTrainSummary();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<ppgExportTrainSummaryUpload>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            Ppg_ExportRepository objImport = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository ObjIR = new Ppg_ExportRepository();
            ObjIR.GetAllTrainNo();
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
            IList<PPG_ImportJobOrderList> lstIJO = new List<PPG_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderDetails(string ContainerNo)
        {
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
            IList<PPG_ImportJobOrderList> lstIJO = new List<PPG_ImportJobOrderList>();
            objIR.GetAllImpJO(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderDetails", lstIJO);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Ppg_ExportRepository ObjCR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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
            Ppg_ExportRepository objIR = new Ppg_ExportRepository();
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

        #region Ship Bill Amendment

        public ActionResult Amendment()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetAmenSBList();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNoAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfSBNoAmendment = null;
            }
            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporterForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfExporterForAmendment = null;
            }

            objER.ListOfShippingLineName();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfShippingForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfShippingForAmendment = null;
            }


            objER.GetPortOfLoading();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfPortLoadingForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfPortLoadingForAmendment = null;
            }
            

            objER.GetInvoiceListForShipbillAmend();
            if(objER.DBResponse.Data!=null)
            {
                ViewBag.ListOfInv = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfInv = null;
            }


            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSBDetailsBySBNo(string SBid, string SbDate)
        {
            Ppg_ExportRepository obj = new Ppg_ExportRepository();
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
        public JsonResult SaveAmendement(List<OldInfoSb> vm, List<NewInfoSb> newvm, string Date, string AmendmentNO,int InvoiceId,string InvoiceNo,string InvoiceDate,string FlagMerger)
        {
            Ppg_ExportRepository obj = new Ppg_ExportRepository();
            if (FlagMerger== "Split" && vm.Count>1)
            {
                return Json(new { Message = "Only One Shipbill Split", Status = 0 }, JsonRequestBehavior.AllowGet);
               
            }
            else if(FlagMerger == "Merger" && vm.Count==1)
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


        public JsonResult GetAmendDetails(string AmendNo)
        {

            Ppg_ExportRepository obj = new Ppg_ExportRepository();
            obj.GetAmenSBDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmendDetailsByAmendNo(string AmendNo)
        {

            Ppg_ExportRepository obj = new Ppg_ExportRepository();
            obj.GetAmenDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SubmitAmedData(Ppg_AmendmentViewModel vm)
        {
            Ppg_ExportRepository obj = new Ppg_ExportRepository();
            obj.AddEditShipAmendment(vm);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListofAmendData()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<Ppg_AmendmentViewModel> lstdata = new List<Ppg_AmendmentViewModel>();
            objER.ListForShipbillAmend();
            if (objER.DBResponse.Status==1)
            {
                lstdata = (List<Ppg_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }
        [HttpGet]
        public ActionResult ViewAmendData(int id)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            Ppg_AmendmentViewModel obj = new Ppg_AmendmentViewModel();
            objER.GetShipbillAmendDet(id);
            if (objER.DBResponse.Status == 1)
            {
                obj = (Ppg_AmendmentViewModel)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ViewMergeSplitData(string AmendNo)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<Ppg_Amendment> obj = new List<Ppg_Amendment>();
            objER.GetAmenDetailsByAmendNO(AmendNo);
            if (objER.DBResponse.Status == 1)
            {
                obj = (List<Ppg_Amendment>)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ListofMergeSplitAmendData()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<Ppg_AmendmentViewModel> lstdata = new List<Ppg_AmendmentViewModel>();
            objER.GetAmenSBDetailsByAmendNO("");
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Ppg_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        public JsonResult GetAllCommodityDetailsForAmendmend(string CommodityName,int Page)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.GetAllCommodityForPageAmendment(CommodityName, Page);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
            
        }




        #endregion

        #region Cargo shifting approval
        [HttpGet]
        public ActionResult CargoShiftingApproval()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult CargoShiftingNoForApproval()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.CargoShiftingNoForApproval();
            return Json(objER.DBResponse,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CargoShiftingDetForApproval(int CargoShiftingId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.CargoShiftingDetForApproval(CargoShiftingId);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost,CustomValidateAntiForgeryToken]
        public JsonResult AddEditCargoShiftingApproval(int CargoShiftingId,int IsApproved,string ShiftingDate)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            objER.AddEditCargoShiftingApproval(CargoShiftingId,IsApproved,ShiftingDate);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListofCargoShiftingApproved()
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            List<Ppg_CargoShiftingApproval> objAppr = new List<Ppg_CargoShiftingApproval>();
            objER.ListofCargoShiftingApproved();
            if (objER.DBResponse.Data != null)
                objAppr = (List<Ppg_CargoShiftingApproval>)objER.DBResponse.Data;
            return PartialView("ListOfCargoShiftApproval", objAppr);
        }
        [HttpGet]
        public ActionResult ViewCargoShiftingApproval(int CargoShiftingId)
        {
            Ppg_ExportRepository objER = new Ppg_ExportRepository();
            Ppg_CargoShiftingApproval objAppr = new Ppg_CargoShiftingApproval();
            objER.CargoShiftingApprovalDet(CargoShiftingId);
            if (objER.DBResponse.Data != null)
                objAppr = (Ppg_CargoShiftingApproval)objER.DBResponse.Data;
            return PartialView(objAppr);
        }
        #endregion




        #region Loaded Container Payment Sheet Edit

        [HttpGet]
        public ActionResult EditLoadedContainerInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

         //   objImport.GetInvoiceForEdit("EXPLod");
           // if (objImport.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
          //  else
            //    ViewBag.InvoiceList = null;
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }



        public JsonResult AddEditContPaymentSheet(Areas.Import.Models.PpgInvoiceYard objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, 0, "EXPLod");

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



        [HttpGet]
        public JsonResult GetLoadedContainerInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Ppg_ExportRepository objCashManagement = new Ppg_ExportRepository();
                objCashManagement.GetLoadedContainerInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

                    IList<PaymentSheetContainer> containers = new List<PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });
                    //******Get Container By ReqId****************************************************//
                    List<PaymentSheetContainer> containersAll = new List<PaymentSheetContainer>();
                    objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Yard");
                    if (objImportRepo.DBResponse.Status == 1)
                    {

                        containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                        containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                        {
                            containers.Add(new PaymentSheetContainer
                            {
                                ContainerNo = item.ContainerNo,
                                ArrivalDt = item.ArrivalDt,
                                CFSCode = item.CFSCode,
                                IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                                Selected = false,
                                Size = item.Size
                            });
                        });

                    }
                    //*********************************************************************************//

                    /***************BOL PRINT******************/
                    var BOL = "";
                    objCharge.GetBOLForEmptyCont("Yard", objPostPaymentSheet.RequestId);
                    if (objCharge.DBResponse.Status == 1)
                        BOL = objCharge.DBResponse.Data.ToString();
                    /************************************/
                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers, BOL = BOL }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}