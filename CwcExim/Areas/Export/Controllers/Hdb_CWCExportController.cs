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
using System.Text;
using CwcExim.Areas.Master.Models;
using EinvoiceLibrary;
using System.Threading.Tasks;
using SCMTRLibrary;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Globalization;
//using CwcExim.Areas.Import.Models;
//using CwcExim.Areas.Import.Models;

namespace CwcExim.Areas.Export.Controllers
{
    public class Hdb_CWCExportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Export/Hdb_CWCExport
        #region Carting Application
        [HttpGet]
        public ActionResult CreateCartingApplication()
        {
            Hdb_CartingApplication objApp = new Hdb_CartingApplication();
            GodownRepository ObjGR = new GodownRepository();
            objApp.ApplicationDate = DateTime.Now.ToString("dd/MM/yyyy");
            Hdb_ExportRepository objRepo = new Hdb_ExportRepository();
            objRepo.ListOfCHA();
            if (objRepo.DBResponse.Data != null)
                objApp.lstCHANames = (List<Hdb_CHA>)objRepo.DBResponse.Data;
            objRepo.ListOfExporter();
            if (objRepo.DBResponse.Data != null)
                objApp.lstExporter = (List<Hdb_Exporter>)objRepo.DBResponse.Data;
            objRepo.GetAllCommodity();
            if (objRepo.DBResponse.Data != null)
                objApp.lstCommodity = (List<CwcExim.Areas.Export.Models.Hdb_Commodity>)objRepo.DBResponse.Data;
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
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Godown>)ObjGR.DBResponse.Data, "GodownId", "GodownName");
            }
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
            return PartialView(objApp);
        }

        public JsonResult GetGstNoForCartApp(int ExpEximTraderId)
        {
            Hdb_ExportRepository ObjHR = new Hdb_ExportRepository();
            ObjHR.GetGstNoForCartApp(ExpEximTraderId);
            return Json(ObjHR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditCartingApplication(Hdb_CartingApplication objCA)
        {
            if (ModelState.IsValid)
            {
                objCA.lstShipping = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_ShippingDetails>>(objCA.StringifyData);
                string XML = Utility.CreateXML(objCA.lstShipping);
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
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
            List<Hdb_CartingList> lstCartingApp = new List<Hdb_CartingList>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid,0);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<Hdb_CartingList>)objER.DBResponse.Data;
            return PartialView(lstCartingApp);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataCartingApp(int Page)
        {
            IEnumerable<Hdb_CartingList> LstWorkOrder = new List<Hdb_CartingList>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<Hdb_CartingList>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCartingAppCHA(String ShiipingChA)
        {
            List<Hdb_CartingList> lstCartingApp = new List<Hdb_CartingList>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllCartingAppforSearch(ShiipingChA,((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<Hdb_CartingList>)objER.DBResponse.Data;
            return PartialView("ListOfCartingApp",lstCartingApp);
        }

        [HttpGet]
        public ActionResult ListOfCartingAppDate(String CartingDate)
        {
            List<Hdb_CartingList> lstCartingApp = new List<Hdb_CartingList>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllCartingAppforSearchDate(CartingDate,((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<Hdb_CartingList>)objER.DBResponse.Data;
            return PartialView("ListOfCartingApp",lstCartingApp);
        }
        [HttpGet]
        public ActionResult ViewCartingApp(int CartingAppId)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetCartingApp(CartingAppId);
            Hdb_CartingApplication objCA = new Hdb_CartingApplication();
            if (objER.DBResponse.Data != null)
                objCA = (Hdb_CartingApplication)objER.DBResponse.Data;
            return PartialView(objCA);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCartingApp(int CartingAppId)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (CartingAppId > 0)
                objER.DeleteCartingApp(CartingAppId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult EditCartingApp(int CartingAppId)
        {
            Hdb_CartingApplication objCA = new Hdb_CartingApplication();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (CartingAppId > 0)
            {
                objER.GetCartingApp(CartingAppId);
                if (objER.DBResponse.Data != null)
                    objCA = (Hdb_CartingApplication)objER.DBResponse.Data;
                objER.ListOfCHA();
                if (objER.DBResponse.Data != null)
                    objCA.lstCHANames = (List<Hdb_CHA>)objER.DBResponse.Data;
                objER.ListOfExporter();
                if (objER.DBResponse.Data != null)
                    objCA.lstExporter = (List<Hdb_Exporter>)objER.DBResponse.Data;
                objER.GetAllCommodity();
                if (objER.DBResponse.Data != null)
                    objCA.lstCommodity = (List<CwcExim.Areas.Export.Models.Hdb_Commodity>)objER.DBResponse.Data;
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

        [HttpGet]
        public ActionResult GetIceGateSBList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.IcegateSBList();
            if (ObjER.DBResponse.Status > 0)
            {
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetIceGateSBDet(string SBNo,int Id)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetIceGateSBDet(SBNo, Id);
            if(ObjER.DBResponse.Status>0)
            {
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetForwarderList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfForwarder();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadPackUQCList(string PartyCode, int Page)
        {
            Hdb_ExportRepository objRepo = new Hdb_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPackUQCByCode(string PartyCode)
        {
            Hdb_ExportRepository objRepo = new Hdb_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Carting Register
        [HttpGet]
        public ActionResult CreateCartingRegister()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Hdb_CartingRegister objCR = new Hdb_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd-MM-yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<Hdb_ApplicationNoDet>)objER.DBResponse.Data;
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Godown>)ObjGR.DBResponse.Data, "GodownId", "GodownName");
            }
            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<Hdb_CartingRegister> objCR = new List<Hdb_CartingRegister>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllRegisterDetails(0);
            if (objER.DBResponse.Data != null)
                objCR = (List<Hdb_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", objCR);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataCartingRegister(int Page)
        {
            IEnumerable<Hdb_CartingRegister> LstWorkOrder = new List<Hdb_CartingRegister>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllRegisterDetails(Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<Hdb_CartingRegister>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListCartingRegisterByDate(string CartingDate)
        {
            List<Hdb_CartingRegister> objCR = new List<Hdb_CartingRegister>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllRegisterDetailsByDate(CartingDate);
            if (objER.DBResponse.Data != null)
                objCR = (List<Hdb_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult ListCartingRegisterByCHA(string ShiipingChA)
        {
            List<Hdb_CartingRegister> objCR = new List<Hdb_CartingRegister>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllRegisterDetailsByCHA(ShiipingChA);
            if (objER.DBResponse.Data != null)
                objCR = (List<Hdb_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            Hdb_CartingRegister objCR = new Hdb_CartingRegister();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId);
            if (objER.DBResponse.Data != null)
                objCR = (Hdb_CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Hdb_CartingRegister ObjCartingReg = new Hdb_CartingRegister();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Hdb_CartingRegister)ObjER.DBResponse.Data;
                }
                ObjGR.GetAllGodown();
                if (ObjGR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Godown>)ObjGR.DBResponse.Data, "GodownId", "GodownName");
                }
            }
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            Hdb_CartingRegister objCR = new Hdb_CartingRegister();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (Hdb_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(Hdb_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                //List<int> lstLocation = new List<int>();
                IList<Hdb_CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_CartingRegisterDtl>>(objCR.XMLData);

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
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            Hdb_ExportRepository objIR = new Hdb_ExportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCartingRegister(int CartingRegisterId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.PrintCartingReg(CartingRegisterId);
            if (ObjER.DBResponse.Data != null)
            {
                List<PrintCartingReg> LstCart = new List<PrintCartingReg>();
                LstCart = (List<PrintCartingReg>)ObjER.DBResponse.Data;
                string Path = GeneratePdfForCartReg(LstCart, CartingRegisterId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 1, Message = "Error" });
            }
        }

        [NonAction]
        public string GeneratePdfForCartReg(List<PrintCartingReg> LstCart, int CartingRegisterId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CartingRegister" + CartingRegisterId + ".pdf";
            var Html = "";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /* if (System.IO.File.Exists(Path))
             {
                   System.IO.File.Delete(Path);
             } */
            int Index = 1;
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td width='80%' style='font-size: 10px;'></td><td width='10%' align='right'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='border:1px solid #333;'><div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/07</div></td></tr></tbody></table></td></tr></tbody></table></td></tr> <tr><td colspan='12'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Government of India Undertaking)</label><br/><span style='font-size:14px;'>Container Freight Station, Kukatpally <br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 14px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 16px; font-weight:bold;'>CARTING REGISTER</label></td><td valign='top' padding: 0 0 0 10px;><img align='right' src='ISO' width='100'/></td></tr></tbody></table></td></tr></tbody></table></td></tr>   <tr><td colspan='12'><table style='border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td><table style='width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px; padding:10px;'>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 130px; padding-top:10px;'>Carting No.<br/>& <br/>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 110px; padding:10px;'>S.B.<br/> No. & <br/>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px; padding:10px;'>Name of Exporter</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>Name CHA</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 130px; padding:10px;'>Cargo</th><th colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px;'><div style='padding:10px;'>Receipt</div></th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding:10px; width: 60px;'>FOB Value</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding:10px; width: 60px;'>Weight</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; padding:10px; text-align: center; width: 60px;'>Cargo Dect. & Ack. No.</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>As per S.B</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>Actual Recvd</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>Balance</th></tr></thead> <tbody> ";
            foreach (PrintCartingReg item in LstCart)
            {
                Html += "<tr style='margin-bottom:10px;'> <td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 80px;'>" + item.RegisterDate + "</td><td style='border-right: 1px solid #000; font-size: 12px; text-align: center; width: 130px;'><div style='padding:5px;'>" + item.CartingRegisterNo + "</div></td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 110px;'><div style='padding:5px;'>" + item.ShipBillNo + "</div></td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>" + item.Exporter + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>" + item.CHA + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 130px;'>" + item.CargoDescription + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.SBUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.ActualUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.BalanceUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.FobValue + "</td><td style='border-right: 1px solid #000;padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.Weight + "</td><td style='padding:10px; font-size: 12px; text-align: center; width: 50px;'>        </td></tr>";
                Index++;
            }
            Html += "</tbody> </table> </td></tr></tbody> </table> </td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            //using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/CartingRegister" + CartingRegisterId + ".pdf";
        }

        #endregion

        #region Container Stuffing
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDBContainerStuffing ObjCS = new HDBContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            ObjER.GetReqNoForContainerStuffing();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = new SelectList((List<HDBContainerStuffing>)ObjER.DBResponse.Data, "StuffingReqId", "StuffingReqNo");
            }
            else
            {
                ViewBag.LstRequestNo = null;
            }
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = new SelectList((List<Hdb_Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
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
            ObjER.ListOfForwarder();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ForwarderList = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.ForwarderList = null;
            }
            return PartialView("CreateContainerStuffing", ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetAllContainerStuffing(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<HDBContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadListMoreContainerStuffing(int Page)
        {
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllContainerStuffing(Page);
            if (ObjER.DBResponse.Data != null)
                LstStuffing = (List<HDBContainerStuffing>)ObjER.DBResponse.Data; ;

            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (HDBContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (HDBContainerStuffing)ObjER.DBResponse.Data;
                }
                ObjER.ListOfCHA();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.CHAList = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                }
                else
                {
                    ViewBag.CHAList = null;
                }
                ObjER.ListOfExporter();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ListOfExporter = new SelectList((List<Hdb_Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
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
                ObjER.ListOfForwarder();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ForwarderList = ObjER.DBResponse.Data;
                }
                else
                {
                    ViewBag.ForwarderList = null;
                }
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(HDBContainerStuffing ObjStuffing)
        {

            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    IList<HDBContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<HDBContainerStuffingDtl>>(ObjStuffing.StuffingXML);

                    var LstStuffingSBSorted = from c in LstStuffing group c by c.ShippingBillNo into grp select grp.Key;

                    foreach (var a in LstStuffingSBSorted)
                    {
                        int vPaketTo = 0;
                        int vPaketFrom = 1;
                        foreach (var i in LstStuffing)
                        {

                            if (i.ShippingBillNo == a)
                            {
                                vPaketTo = vPaketTo + i.StuffQuantity;
                                i.PacketsTo = vPaketTo;
                                i.PacketsFrom = vPaketFrom;
                                vPaketFrom = 1 + vPaketTo;
                            }
                        }

                    }



                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.DeleteContainerStuffing(ContainerStuffingId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }


        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianCode)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfFinalDestination(CustodianCode);
            List<HDB_FinalDestination> lstFinalDestination = new List<HDB_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<HDB_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Container Stuffing Amendment

        [HttpGet]
        public ActionResult AmendmentContainerStuffing()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();

            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            ObjER.ListOfExporter();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = new SelectList((List<Hdb_Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
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
            ObjER.ListOfForwarder();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ForwarderList = ObjER.DBResponse.Data;
            }
            else
            {
                ViewBag.ForwarderList = null;
            }
            return PartialView("AmendmentContainerStuffing");
        }

        [HttpGet]
        public JsonResult GetStuffingNoForAmendment()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfStuffingNoForAmendment();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetStuffingDetailsForAmendment(int ContainerStuffingId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDBContainerStuffing ObjStuffing = new HDBContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingDetails(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (HDBContainerStuffing)ObjER.DBResponse.Data;
                }

            }

            return Json(ObjStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditAmendmentContainerStuffing(HDBContainerStuffing ObjStuffing)
        {

            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";

                if (ObjStuffing.StuffingXML != null)
                {
                    List<HDBContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HDBContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(0, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<HDBContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView(LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadAmendmentContainerStuffingList(int Page, string SearchValue = "")
        {
            List<HDBContainerStuffing> LstStuffing = new List<HDBContainerStuffing>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(Page, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<HDBContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Packing Application
        [HttpGet]
        public ActionResult CreatePackingApplication()
        {
            Hdb_PackingApplication objApp = new Hdb_PackingApplication();
            objApp.EntryDate = DateTime.Now.ToString("dd/MM/yyyy");

            return PartialView(objApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPackingApplication(Hdb_PackingApplication objFC)
        {
            if (ModelState.IsValid)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                objFC.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditPackingApplication(objFC);
                ModelState.Clear();
                return Json(ObjER.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditPacking(int Packingapplicationid)
        {
            Hdb_PackingApplication objFC = new Hdb_PackingApplication();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            if (Packingapplicationid > 0)
            {
                ObjER.GetPacking(Packingapplicationid);
                if (ObjER.DBResponse.Data != null)
                    objFC = (Hdb_PackingApplication)ObjER.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpGet]
        public ActionResult ListOfPackingApplication()
        {
            List<Hdb_PackingApplication> lstPackingApp = new List<Hdb_PackingApplication>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllPackingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstPackingApp = (List<Hdb_PackingApplication>)objER.DBResponse.Data;
            return PartialView(lstPackingApp);
        }
        [HttpGet]
        public ActionResult ViewPacking(int Packingapplicationid)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetPacking(Packingapplicationid);
            Hdb_PackingApplication objCA = new Hdb_PackingApplication();
            if (objER.DBResponse.Data != null)
                objCA = (Hdb_PackingApplication)objER.DBResponse.Data;
            return PartialView(objCA);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeletePacking(int Packingapplicationid)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (Packingapplicationid > 0)
                objER.DeletePacking(Packingapplicationid);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            Hdb_StuffingRequest ObjSR = new Hdb_StuffingRequest();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetCartRegNoForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<Hdb_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
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
            ObjER.GetAllContainerNo();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ContainerList = new SelectList((List<Hdb_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
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
                    ViewBag.ListOfCHA = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
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
                    ViewBag.ListOfExporter = new SelectList((List<Hdb_Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
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
        public ActionResult AddEditStuffingReq(Hdb_StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                IList<Hdb_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<Hdb_StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<Hdb_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<Hdb_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
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
            Hdb_StuffingRequest ObjStuffing = new Hdb_StuffingRequest();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CHAList = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
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
            if (StuffinfgReqId > 0)
            {
                ObjER.Hdb_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Hdb_StuffingRequest)ObjER.DBResponse.Data;
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
                        ViewBag.ListOfCHA = new SelectList((List<Hdb_CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
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
                        ViewBag.ListOfExporter = new SelectList((List<Hdb_Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
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
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            Hdb_StuffingRequest ObjStuffing = new Hdb_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.Hdb_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Hdb_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Hdb_GetCartRegDetForStuffingReq(CartingRegisterId);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            List<Hdb_StuffingRequest> LstStuffing = new List<Hdb_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Hdb_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }


        [HttpGet]
        public JsonResult LoadListMoreDataStuffingReq(int Page)
        {
            IEnumerable<Hdb_StuffingRequest> LstWorkOrder = new List<Hdb_StuffingRequest>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid,Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<Hdb_StuffingRequest>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            // StuffingReqContainerDtl ObjSRD = new StuffingReqContainerDtl();
            ObjER.GetContainerNoDet(CFSCode);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ObjSRD = (StuffingReqContainerDtl)ObjER.DBResponse.Data;
            //}
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPortList()
        {
            HDBMasterRepository objPort = new HDBMasterRepository();
            objPort.GetAllPort();
            return Json(objPort.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetForwarderLists()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfForwarder();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            HDBLoadContReq ObjLR = new HDBLoadContReq();
            ObjLR.LoadContReqDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
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
            return PartialView(ObjLR);
        }


        [HttpGet]
        public ActionResult ViewLoadContainerRequest(int LoadContReqId)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            HDBLoadContReq ObjContReq = new HDBLoadContReq();
            objER.GetLoadContDetails(LoadContReqId);
            if (objER.DBResponse.Data != null)
            {
                ObjContReq = (HDBLoadContReq)objER.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            HDBLoadContReq ObjContReq = new HDBLoadContReq();
            objER.GetLoadContDetails(LoadContReqId);
            if (objER.DBResponse.Data != null)
            {
                ObjContReq = (HDBLoadContReq)objER.DBResponse.Data;
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
                else
                {
                    ViewBag.lstPackUQC = null;
                }
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            List<HDBListLoadContReq> lstCont = new List<HDBListLoadContReq>();
            objER.ListOfLoadCont(0);
            if (objER.DBResponse.Data != null)
                lstCont = (List<HDBListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }



        [HttpGet]
        public JsonResult LoadListMoreDataLoadContainerRequest(int Page)
        {
            IEnumerable<HDBListLoadContReq> LstWorkOrder = new List<HDBListLoadContReq>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.ListOfLoadCont(Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<HDBListLoadContReq>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(HDBLoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
                string XML = "";

                List<HDBLoadContReqDtl> LstLoadContReqDtl = JsonConvert.DeserializeObject<List<HDBLoadContReqDtl>>(objReq.StringifyData);

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



                if (objReq.StringifyData != null)
                {
                    XML = Utility.CreateXML(LstLoadContReqDtl);
                 //   XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<HDBLoadContReqDtl>>(objReq.StringifyData));
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
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
                objER.DelLoadContReqhdr(LoadContReqId);
                return Json(objER.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region BTT Payment Sheet
        [HttpGet]
        public ActionResult CreateBTTPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetCartingApplicationForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

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

            //objExport.GetPaymentPartyForExportnvoice();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            //objExport.GetPayeeForExportnvoice();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentPayee = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentPayee = null;



            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPaymentSheetShipBillNo(int StuffingReqId)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetShipBillForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour,
            List<CwcExim.Areas.Import.Models.PaymentSheetContainer> lstPaySheetContainer,string ExportUnder="")
        {

            int InvoiceId = 0;
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetBTTPaymentSheet(InvoiceDate, StuffingReqId, InvoiceType, XMLText, InvoiceId, PartyId, CasualLabour, ExportUnder);
            var Output = (HDBInvoiceBTT)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new HDBPostPaymentContainerBTT
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
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBTTPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<HDBInvoiceBTT>(objForm["PaymentSheetModelJson"].ToString());
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
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                if (invoiceData.lstPreInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstPreInvoiceCargo);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
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
        #endregion

        #region Loaded Container Invoice
        [HttpGet]
        public ActionResult CreateLoadedContainerPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetLoadedContainerRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objExport.GetPaymentPartyForExportnvoice();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            //objExport.GetPayeeForExportnvoice();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentPayee = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentPayee = null;

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
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, List<CwcExim.Areas.Export.Models.PaymentSheetContainer> lstPaySheetContainer, int CasualLabour, int Distance,string ExportUnder,string Under, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }

            Hdb_ExportRepository objChrgRepo = new Hdb_ExportRepository();

            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetLoadedPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, ContainerXML, CasualLabour,Distance, ExportUnder, Under, InvoiceId);

            var Output = (Hdb_LoadedContainerPayment)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPLod";
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
                    Output.lstPostPaymentCont.Add(new CwcExim.Areas.Import.Models.Hdb_PostPaymentContainer
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
            return Json(Output);
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

                var invoiceData = JsonConvert.DeserializeObject<Hdb_LoadedContainerPayment>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string CfsWiseCharg = "";
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

                    // CfsWiseCharg = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                string ForeignPortCode = objForm["ForeignDestPortCode"];
                string Distance = objForm["Distance"];
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                invoiceData.LEODate = invoiceData.LEODate == "" ? null : Convert.ToDateTime(invoiceData.LEODate).ToString("yyyy-MM-dd");
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, ForeignPortCode, BranchId, ((Login)(Session["LoginUser"])).Uid,Distance, "EXPLod");

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

        public JsonResult InvoicePrint_Loaded(string InvoiceNo)
        {
            Hdb_ReportRepository objGPR = new Hdb_ReportRepository();
            objGPR.GetInvoiceDetailsPrintByNo(InvoiceNo, "EXPLod");
            if (objGPR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)objGPR.DBResponse.Data;
                CwcExim.Areas.Import.Models.PpgInvoiceYard objGP = new CwcExim.Areas.Import.Models.PpgInvoiceYard();
                string FilePath = "";
                FilePath = GeneratingInvoicePrint(ds, "EXPLod");
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


        #endregion

        #region Export Payment Sheet

        [HttpGet]
        public ActionResult CreatePaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetStuffingRequestForPaymentSheet();
            if (objExport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

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


            /*objExport.GetPaymentPartyForExportnvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            objExport.GetPayeeForExportnvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentPayee = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentPayee = null;*/
            return PartialView();
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int StuffingReqId)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetContainerForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour, int Distance,
            List<PaymentSheetContainer> lstPaySheetContainer,string ExportUnder="", int InvoiceId = 0)
        {

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetExportPaymentSheet(InvoiceDate, StuffingReqId, InvoiceType, XMLText, InvoiceId, PartyId, CasualLabour, Distance, ExportUnder);
            var Output = (HDBInvoiceExp)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "Exp";

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
                    Output.lstPostPaymentCont.Add(new HDBPostPaymentContainer
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
            return Json(Output);
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
                string ForeignPortCode = objForm["ForeignDestPortCode"];

                var invoiceData = JsonConvert.DeserializeObject<HDBInvoiceExp>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("1900-01-01 00:00:00") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
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
                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    var result = invoiceData.lstOperationCFSCodeWiseAmount.Where(o => invoiceData.lstPostPaymentChrg.Select(s => s.Clause).ToList().Contains(o.Clause)).ToList();
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditExpInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ForeignPortCode, BranchId, ((Login)(Session["LoginUser"])).Uid);
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

        #region Party Bond

        [HttpGet]
        public ActionResult CreatePartyBond()
        {
            return PartialView("CreatePartyBond");
        }

        [HttpGet]
        public ActionResult GetPartyBondList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            List<PartyBondList> LstPartyBond = new List<PartyBondList>();
            ObjER.GetAllPartyBond();
            if (ObjER.DBResponse.Data != null)
            {
                LstPartyBond = (List<PartyBondList>)ObjER.DBResponse.Data;
            }
            return PartialView("PartyBondList", LstPartyBond);
        }
        [HttpGet]
        public ActionResult EditPartyBond(int PartyBondId)
        {
            PartyBond ObjPartyBond = new PartyBond();
            if (PartyBondId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.GetPartyBond(PartyBondId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjPartyBond = (PartyBond)ObjER.DBResponse.Data;
                }
            }
            return PartialView("EditPartyBond", ObjPartyBond);
        }

        [HttpGet]
        public ActionResult ViewPartyBond(int PartyBondId)
        {
            PartyBond ObjPartyBond = new PartyBond();
            if (PartyBondId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.GetPartyBond(PartyBondId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjPartyBond = (PartyBond)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewPartyBond", ObjPartyBond);
        }

        [HttpGet]
        public JsonResult GetExporterList()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.ListOfExporter();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPartyBond(PartyBond ObjPartyBond)
        {

            if (ModelState.IsValid)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.AddEditPartyBond(ObjPartyBond);
                ModelState.Clear();
                return Json(ObjER.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion

        #region Reefer Plugin Request Payment Sheet
        [HttpGet]
        public ActionResult CreateReeferPluginRequest(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetInvoiceNo();
            if (objExport.DBResponse.Data != null)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;
            //objExport.GetPaymentPayee();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            objExport.GetPaymentPayerForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.PaymentPayee = Jobject["lstPayer"];
                ViewBag.StatePayer = Jobject["StatePayer"];
            }
            else
            {
                ViewBag.lstPayer = null;
            }

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetReeferPluginRequest(int InvoiceId)
        {
            Hdb_ExportRepository ObjIR = new Hdb_ExportRepository();
            ObjIR.GetReeferPluginRequest(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReeferPaymentSheet(string InvoiceDate, string InvoiceType, int PartyId, List<Hdb_ContainerDetails> lstPaySheetContainer, int InvoiceId,int PayeeId,string PayeeName,
            string ExportUnder="" , int Distance = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                lstPaySheetContainer.ForEach(x =>
                {
                    x.PlugInDatetime = Convert.ToDateTime(x.PlugInDatetime).ToString("yyyy-MM-dd HH:mm:ss");
                    x.PlugOutDatetime = Convert.ToDateTime(x.PlugOutDatetime).ToString("yyyy-MM-dd HH:mm:ss");
                });
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }
            Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
            objPpgRepo.GetReeferPaymentSheet(InvoiceDate, InvoiceType, PartyId, XMLText, InvoiceId, PartyId,Distance ,ExportUnder);
            var Output = (Hdb_ReeferInv)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPREF";

            Output.InvoiceType = InvoiceType;
            Output.PayeeId = PayeeId;
            Output.PayeeName = PayeeName;
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
                    Output.lstPostPaymentCont.Add(new Hdb_PostPaymentContainerRef
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
        public JsonResult AddEditReeferPaymentSheet(Hdb_ReeferInv objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd HH:mm:ss");
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("1900-01-01 00:00:00") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                    item.PlugInDatetime = Convert.ToDateTime(item.PlugInDatetime).ToString("yyyy-MM-dd HH:mm:ss");
                    item.PlugOutDatetime = Convert.ToDateTime(item.PlugOutDatetime).ToString("yyyy-MM-dd HH:mm:ss");
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

                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditReeferInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid);

                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
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

        #region Back To Town Cargo
        [HttpGet]
        public ActionResult CreateBTTCargo()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ListOfBTTCargo()
        {
            List<Hdb_BTTCargoEntry> lstBTTCargoEntry = new List<Hdb_BTTCargoEntry>();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetBTTCargoEntry(0);
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<Hdb_BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }


        [HttpGet]
        public JsonResult LoadListMoreDataBTTCargo(int Page)
        {
            IEnumerable<Hdb_BTTCargoEntry> LstWorkOrder = new List<Hdb_BTTCargoEntry>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetBTTCargoEntry(Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<Hdb_BTTCargoEntry>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddBTTCargo()
        {
            Hdb_BTTCargoEntry objBTTCargoEntry = new Hdb_BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy");

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetCartingAppList(0);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Hdb_BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<Hdb_CHAList>)objExport.DBResponse.Data;

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult EditBTTCargo(int BTTCargoEntryId)
        {
            Hdb_BTTCargoEntry objBTTCargoEntry = new Hdb_BTTCargoEntry();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Hdb_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Hdb_BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            objExport.GetCHAListForBTT();
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCHAList = (List<Hdb_CHAList>)objExport.DBResponse.Data;
            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult ViewBTTCargo(int BTTCargoEntryId)
        {
            Hdb_BTTCargoEntry objBTTCargoEntry = new Hdb_BTTCargoEntry();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Hdb_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Hdb_BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public JsonResult GetCartingDetailList(int CartingId)
        {
            try
            {
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
        public JsonResult AddEditBTTCargo(Hdb_BTTCargoEntry objBTT)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objBTT.lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_BTTCargoEntryDtl>>(objBTT.BTTCargoEntryDtlJS);
                    string XML = Utility.CreateXML(objBTT.lstBTTCargoEntryDtl);
                    Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
            Hdb_CartingWorkOrder ObjWorkOrder = new Hdb_CartingWorkOrder();
            ObjWorkOrder.WorkOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDBMasterRepository ObjGR = new HDBMasterRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetCartingNoForWorkOrder(BranchId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjWorkOrder.LstCarting = (List<Hdb_CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ObjWorkOrder.LstGodown = (List<HDBGodown>)ObjGR.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/Hdb_CWCExport/CreateCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult GetCartingWorkOrderList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingWorkOrder(BranchId,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstWorkOrder = (List<Hdb_CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/Hdb_CWCExport/CartingWorkOrderList.cshtml", LstWorkOrder);
        }


        [HttpGet]
        public JsonResult LoadListMoreDataWorkOrder(int Page)
        {
            IEnumerable<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingWorkOrder(BranchId,Page);
            if (ObjER.DBResponse.Data != null)
              LstWorkOrder = (List<Hdb_CartingWorkOrder>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetCartingWorkOrderListByDate(string CartingDate)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingWorkOrderByDate(CartingDate,BranchId);
            if (ObjER.DBResponse.Data != null)
            {
                LstWorkOrder = (List<Hdb_CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/Hdb_CWCExport/CartingWorkOrderList.cshtml", LstWorkOrder);
        }




        [HttpGet]
        public ActionResult GetCartingWorkOrderListByCHA(string ShiipingChA)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            List<Hdb_CartingWorkOrder> LstWorkOrder = new List<Hdb_CartingWorkOrder>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetAllCartingWorkOrderByCHA(ShiipingChA,BranchId);
            if (ObjER.DBResponse.Data != null)
            {
                LstWorkOrder = (List<Hdb_CartingWorkOrder>)ObjER.DBResponse.Data;
            }
            return View("/" + "/Areas/Export/Views/Hdb_CWCExport/CartingWorkOrderList.cshtml", LstWorkOrder);
        }




        [HttpGet]
        public ActionResult EditCartingWorkOrder(int CartingWorkOrderId)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];

            Hdb_CartingWorkOrder ObjWorkOrder = new Hdb_CartingWorkOrder();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDBMasterRepository ObjGR = new HDBMasterRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (CartingWorkOrderId > 0)
            {
                ObjER.GetCartingWorkOrder(CartingWorkOrderId, BranchId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjWorkOrder = (Hdb_CartingWorkOrder)ObjER.DBResponse.Data;
                    ObjGR.GetAllGodown();
                    if (ObjGR.DBResponse.Data != null)
                    {
                        ObjWorkOrder.LstGodown = (List<HDBGodown>)ObjGR.DBResponse.Data;
                    }
                }
            }
            return View("/Areas/Export/Views/Hdb_CWCExport/EditCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewCartingWorkOrder(int CartingWorkOrderId)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Hdb_CartingWorkOrder ObjWorkOrder = new Hdb_CartingWorkOrder();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            if (CartingWorkOrderId > 0)
            {
                ObjER.GetCartingWorkOrder(CartingWorkOrderId, BranchId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjWorkOrder = (Hdb_CartingWorkOrder)ObjER.DBResponse.Data;
                }
            }
            return View("/Areas/Export/Views/Hdb_CWCExport/ViewCartingWorkOrder.cshtml", ObjWorkOrder);
        }

        [HttpGet]
        public JsonResult GetCartingDetForWorkOrder(int CartingAppId)
        {
            if (CartingAppId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
        public JsonResult AddEditCartingWorkOrderDet(Hdb_CartingWorkOrder ObjWorkOrder)
        {
            if (ModelState.IsValid)
            {
                ObjWorkOrder.Remarks = ObjWorkOrder.Remarks == null ? null : ObjWorkOrder.Remarks.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjWorkOrder.Uid = ObjLogin.Uid;
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
                List<Hdb_CartingWOPrint> lstWODetails = new List<Hdb_CartingWOPrint>();
                objER.GetDetailsForPrint(CartingWorkOrderId);
                if (objER.DBResponse.Data != null)
                {
                    lstWODetails = (List<Hdb_CartingWOPrint>)objER.DBResponse.Data;
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
        public string GeneratePdfForWO(List<Hdb_CartingWOPrint> lstWO, int CartingWorkOrderId)
        {
            string CHAName = "", ExpName = "";
            string CompanyAddress = lstWO[0].CompanyAddress;
            string WODate = DateTime.Now.ToString("dd/MM/yyyy");
            int NoOfUnit = lstWO.Select(x => x.NoOfUnits).Sum();
            string GrossWeight = lstWO.Select(x => x.Weight).Sum().ToString() + " KG";
            string To = "";
            string TruckNo = "";
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
            lstWO.Select(x => new { TruckNo = x.TruckNo }).Distinct().ToList().ForEach(item =>
            {
                if (TruckNo == "")
                    TruckNo = item.TruckNo;
                else
                    TruckNo += " ," + item.TruckNo;
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
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + lstWO[0].WorkOrderNo + "</th><th style='text-align:right;'>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,<br/>" + To + "</th><th style='text-align:right;'>Dated: " + WODate + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ExpName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Carting " + lstWO[0].CartingNo + "</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfUnit + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + TruckNo + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: " + lstWO[0].GodownName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : </td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/CWorkOrder" + CartingWorkOrderId + ".pdf";
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
            Hdb_StuffingWorkOrder objWorkOrder = new Hdb_StuffingWorkOrder();
            objWorkOrder.WorkOrderDate = DateTime.Now.ToString("dd/MM/yyyy");

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetStuffingRequestList(0);
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstStuffingNoList = (List<Hdb_StuffingNoList>)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingNoList != null)
                objWorkOrder.StuffingNoListJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingNoList);

            objExport.ListOfGodown();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstGodownList = (List<Hdb_GodownList>)objExport.DBResponse.Data;
            objExport.ListOfCommodity();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstCommodity = (List<Areas.Export.Models.Commodity>)objExport.DBResponse.Data;

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public ActionResult GetStuffingWorkOrderList()
        {
            List<Hdb_StuffingWorkOrder> lstWorkOrder = new List<Hdb_StuffingWorkOrder>();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetStuffingWorkOrder(0);
            if (objExport.DBResponse.Data != null)
                lstWorkOrder = (List<Hdb_StuffingWorkOrder>)objExport.DBResponse.Data;

            return PartialView(lstWorkOrder);
        }


        [HttpGet]
        public JsonResult LoadListMoreDataStuffingWorkOrder(int Page)
        {
            IEnumerable<Hdb_StuffingWorkOrder> LstWorkOrder = new List<Hdb_StuffingWorkOrder>();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            ObjER.GetStuffingWorkOrder(Page);
            if (ObjER.DBResponse.Data != null)
                LstWorkOrder = (List<Hdb_StuffingWorkOrder>)ObjER.DBResponse.Data; ;

            return Json(LstWorkOrder, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditStuffingWorkOrder(int WorkOrderID)
        {
            Hdb_StuffingWorkOrder objWorkOrder = new Hdb_StuffingWorkOrder();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();

            objExport.GetStuffingWorkOrderById(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder = (Hdb_StuffingWorkOrder)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingWorkOrderDtl != null)
                objWorkOrder.StuffingWorkOrderDtlJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingWorkOrderDtl);


            /*objExport.GetStuffingRequestList(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstStuffingNoList = (List<StuffingNoList>)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingNoList != null)
                objWorkOrder.StuffingNoListJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingNoList);

            objExport.ListOfGodown();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstGodownList = (List<GodownList>)objExport.DBResponse.Data;
            objExport.ListOfCommodity();
            if (objExport.DBResponse.Data != null)
                objWorkOrder.lstCommodity = (List<Export.Models.Commodity>)objExport.DBResponse.Data;*/

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public ActionResult ViewStuffingWorkOrder(int WorkOrderID)
        {
            Hdb_StuffingWorkOrder objWorkOrder = new Hdb_StuffingWorkOrder();
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();

            objExport.GetStuffingWorkOrderById(WorkOrderID);
            if (objExport.DBResponse.Data != null)
                objWorkOrder = (Hdb_StuffingWorkOrder)objExport.DBResponse.Data;
            if (objWorkOrder.lstStuffingWorkOrderDtl != null)
                objWorkOrder.StuffingWorkOrderDtlJS = JsonConvert.SerializeObject(objWorkOrder.lstStuffingWorkOrderDtl);

            return PartialView(objWorkOrder);
        }

        [HttpGet]
        public JsonResult GetContainerListByStuffingReqId(int StuffingReqID)
        {
            try
            {
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                if (StuffingReqID > 0)
                    objExport.GetContainerListByStuffingReqId(StuffingReqID);
                return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditStuffingWorkOrder(Hdb_StuffingWorkOrder objStuffing)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objStuffing.lstStuffingWorkOrderDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_StuffingWorkOrderDtl>>(objStuffing.StuffingWorkOrderDtlJS);
                    string XML = Utility.CreateXML(objStuffing.lstStuffingWorkOrderDtl);
                    Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
                Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
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
            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th colspan='2' style='text-align:center;'><span style='font-size:16pt;'>CENTRAL WAREHOUSING CORPORATION</span><br/><span>(A GOVERNMENT OF INDIA UNDERTAKING)</span></th></tr><tr><th style='text-align:left;'><img style='max-width:50%;' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABICAYAAAAAjFAZAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAr0SURBVHhe7Z1psB1FFcdvFR8sFwoRMREEIQgCIaCEAEIWlhiQPQQEwyIQQMIWCIkYEQqS0hAFWWTfpAAFqiQIiqCoLAqyK7KVrNEAz+Cd3Y1P7fz7Oc958/4zc3q6574biw+/L2+6zzl3TndP9+nT/Trd+F31/4B/4w9VvPm2Kt7sMyq47mZaZnVg9XbIm++o4NxvqmSdTdTfO51hJGtvrIJzlqjuX/7K6/Ypq6VDvMeeUeHRX1FJwQkMlAmPPFZ5v32Syuo3ViuH+LffqaJd96QvXkI09fPKv/UOKrtf6H+H/C1WwbKLVLzxVvQlNyHecEsVnH+h6r4TcZ2jSN86xHvuZRWePD8dcj5MX6oLks6aKpw7T3nPvkRtGA36ziH+PferaN9Z9AW2SbTXAcq/+z5qUy/pG4cEV1yr4gk70JfVS+Lx26ng0quojb1gVB3ivbxChQvPSoeO9ejLGU2SzlgVnvE15b30OrW9LUbFIf4vH1bRQYfRF9GPRAd+Sfm/eJD+Ftf01CF6Nb3DNPqjXRDvvKuKJ+9Gn7kgnjRF+dffQn+bK9p3CFbT531LJet+iv5IW/RM6ajjlffgo0M6vYd+p8JjTkifrUXr2NJmFKA1h5isppsQb7uzCi65snot0U30BzrebgqVYYuOAnz5OOU98hTX3wDnDtGr6d2+QH+ALWjxaPnoAUx3Fd5vnlDhnBNTGWtT2bZE02Y4iQK4cQhaIlbTG42nxtqCFq6noqkeqt8E7x+DU+ztp1JdtsQbbGEVBbByyP9W0+5bHVbo4Zy5aW94jOp2ge41x56U6voItcEG3ZtPPM04CtDIIW2upnVvuOxqN71Biv/PVntNtPdMcRTAyCFtrabRQtFS0WKZ3jKwaMNUOpx/pooOOUJF+xyoogO+qGUF37542MxLCj7Q4QmnpjatS221IR4/abCxRf+muoHYIWwTyBa0SDgZLZTpLCO45kYV77QLlVkkWWsjFZ4yX3kvvkZllZK+tODq76t4R5keEzBtpjpTxA5hgpuQdD6mW6D36NNUTxX+j+6ymjjAMUxuHd7jf1DhSaento+lcpvA9ICeOQQr9OCqG6hsCeHxp1C5psTrfVp5z7xAdUgw6Z1VMNmgVYfY9IY80S57UPk22MamdK9JZ1FJZwyVXweTCVpxCFoQxt+qj5cULLiYDhfgpTKdpgTX3mQcQ2NygDOH6HB1Os66+pEgnLeA6nJF0vmg6gb/orqb4D35R/2dkmwnsPrA2iHR1OniPCjv+Vd12fC0hXpuHm/zORWPm6DjXiPKpsMc0+eaaP+DR+jOwDNsWEUz9tFDL6as3hPP0rJF/Bt+oJMqmE7A6oBGDhnqDQLjMN0Mv3GeirfafpiMPP59vx5RL95ka1q2Dcp6ddkKHuERRCgkQcWyXsPKAiOHxBMn6/GSPS+CVo+NnbwRZXgPPDKsLjawWLk68KKSzifosyqQWpTXnyFpFDq16K57af0i2EvJogHsORA7RDxV7CY6Ils0vIqiQ6L9DqLlGFhR6xcyEAzuT6RgaIwOO5qWL6O7YmCYDcCkl0Z77JvqfWWEDEZVfEvsEAn+HXenL8h8Glh0SNJZg5YrEm85cVDv/Q/p3UIE9NBDMJzi75jpsXoMvbeSswE0GTaDCy4dIccEZw7Bd4IZKCHvEMSzWBkGyiMsz55hJ1HblY717HmR6ODDh2zIaPodiw49coQsKU4cgjA5M0xK3iGIbbEyRYKlF+jy7FkGAo11ZTLwAc9syLCZWGBdUpQnwdoh2IVjBpmQXzUjm52VKaL36i/8Hn2Wx3vlzzqdB2N8Ffgd+d8Fkg99ksqUgnBRUWYdVg6Rvrw6vIcfH5IZLjqHlimCBZ1kOLIJkWCdxGSakPVSKY0dgh/KDKhDz+HTjy72n72nnx+cGeVWy8GSZbReEbR8TCLYszyQGc2arfdxqohmHjpkwxBve3pX1L/zHhWeeXZjB+kt3aLsEho7JOm8jyovA93XX/5TKisPVvKsfhGk/qA8e5YRXHR5bZmMZI0NhtlRBraU0eqZjCowFWfyijRyCNJ7mNIygsuvoXIY3u9fpDIYetPprW7aOEY+y1q8NOiXOViK/+OfpXrXp7IYOG7H5BQxdgi2TZlCBgxukuqPsD2TVwSO8P70hq6DCAI+zNjOzcL9JotD/6Zbh9kgIl0EI5uRyWNgiKVychg7RNpd8VLRepmMOkx7IKLCurcgMSId95EbFo/djJYtwyapQpqIh21wVj+PmUNWrqKKGN4LhnvYOdCrmMy2iA4/htphgrRX1836jBwinQG5OF8RTd+bym4D77U3qQ0mSLcLqsL9wMghSGNhSvJgqGB1TfHeeJvKdw3OpzD9TYhmHkJ15MF3j9XNkDsk/R4wBUWQGULrN8AkONgEXDLA9DYFPY3pKeLf+ytaH4gd4t+2nArPg5RSVtcGaXDQFESlu6tCqtMGSfYjohGsLhA7JDzrXCo8TzT7KFrXlvDUM6i+psRjNqX7Hy4IvnMJ1ZkHGZasLhA7BC+bCc9jsgA0JbjyeqrTFGx+MfmukGwfVC0S5Q4R3KBgm+tUBxalTcIWAJONnlxKI1gaIIpM66aIHSJZkUozMmzRSWpz56XfgfrQRbz+5q2fCyxSdzwD286sHpA7ZOJkKjyP99RztG5bIK2T2ZEHKUesbpsknY9SWzLwnNUD8iFr6nQqPE+T9H8bgsVLqR15sDZgdVtjIKB25KmKLMsdMms2FZ6n10NDeMQcakce7HWwum2BUYLZkQfJgawuEDskXLCICs+DcZ3VbQvpnrfp+RMbJMNotPtetC4QO0SycYSz6KxuG3ivv0VtYPjLf0JltIHkqB+i06wuEDsE95Iw4UVsjx5IkSxUM6papFPSnsj0F0HUg9ZPETsEYIXLFORpmv5iStL5ANVfBhoUk+OScOHXqe4iVSEbI4dgCskUFGl7gRie/lWqtwqcWWGynCEMvkZTduf1/4uRQ5C7ypQUwRlzVt8FNscUEH5hMl0Qf3YnqrOIf8vttH6GkUOAdLsSUU9W34oVA8ZDVRHkAVPZFuAYNtNVJEtvrcLYIUiDYcoYOBvIZDQBaTSuTsG6vMoPB3mYDkbw3cuojDzGDgGSVXsGjjHbzrxwOYDrW4VwZRTTJSbtrSaX7OikDyanQCOHwBimtAp94spw7xqh7GjP/ag8F2AVL0neGwau4ViyLH3BZomC/s8f4PIKNHNICvY+mOI6kPaPHCikgjK5OBgUXHyFUS/MQC+qC+wx9MWXi5cOXu3BDoGuXKUdFx53ciP5uFNrhMwSGjsE2N6bmLx/Q33oJt56RxVvuk36Y5ud+c7AUTiTVCUGZogYZnXO7/hJOpfKZrjE72Lvrgwrh4A271A0AYuyzCb/5ttomV6TdD5ufG+WtUPAaDsFe+5Fm9rOWKkDR+uQylS0qw4nDgFNt1ZtwQeW2QMkmTJtgKGuO+BTm+pw5hCAmzqZgW2AsV6SvIz9CZcX+deBPRpmhxSnDgGYqeDGUGasKzCZQFI1019Gk/iXCck64/roEkyCvsZo3ARqfFP0IX3hfJ6Bsyeub9TGDQ2YMjN9TWjNIRkYx5EYZrqQykBmyeBlmOZXw5ahL+9csEhnpDCdErDN0EawsnWHDDEQ6C6tL56ZNkNfJIZ0mWyOP7ioG6O3ZeHA4OzF9A4U1+BfIeEMIHoOFoho8fnGg4AghiNc9YeFIQ4Gea+upLJc0DuHvIeI9xzSV7yr/gMOCRG/i1UuogAAAABJRU5ErkJggg=='/></th><th></th></tr><tr><th style='text-align:left;'>Sl. No.: " + WorkOrderNo + " </th><th style='text-align:right;'>" + CompanyAddress + "</th></tr><tr><th style='text-align:left;padding-top:20pt;'>To,<br/>" + To + "</th><th style='text-align:right;'>Dated: " + WODate + "</th></tr></thead><tbody><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tbody><tr><td colspan='2' style='text-align:center;font-size:12pt;font-weight:600;padding-bottom:20pt;padding-top:40pt;'><span style='border-bottom:1px solid #000;'>WORK ORDER</span></td></tr><tr><td colspan='2'>Sir,</td></tr><tr><td style='width:5%;'></td><td><br/>Please arrange to execute the work mentioned below immediately :</td></tr><tr><td style='vertical-align: bottom;'><br/>1.</td><td><br/>Importer's / Exporter's Name: " + ExpName + "</td></tr><tr> <td style='vertical-align: bottom;'><br/>2.</td> <td><br/>CHA's Name: " + CHAName + "</td></tr><tr><td style='vertical-align: bottom;'><br/>3.</td><td><br/>Stuffing " + lstWO[0].StuffingReqNo + "</td></tr><tr><td style='vertical-align: bottom;'><br/>4.</td><td><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;font-weight:600;'><tr><td><br/>No. of packages: " + NoOfUnit + "</td><td><br/> Weight: " + GrossWeight + "</td></tr></table></td></tr><tr><td style='vertical-align: bottom;'><br/>5.</td><td><br/> Truck No.: " + "" + "</td></tr><tr><td style='vertical-align: bottom;'><br/>6.</td> <td><br/>Location: </td></tr><tr><td style='vertical-align: bottom;'><br/>7.</td><td><br/>Container no. : " + ContainerNumber + " </td></tr><tr><td colspan='2' style='text-align:right;padding-top:30pt;'>Signature of I/C</td></tr></tbody></table></td></tr></tbody></table>";
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/CWorkOrder" + StuffingWorkOrderId + ".pdf";
        }
        #endregion

        #region BTT Carting Register
        [HttpGet]
        public ActionResult CreateBTTCartingRegister()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Hdb_BTTCartingRegister objCR = new Hdb_BTTCartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd-MM-yyyy");

            objER.ContainerNoForBTTCartingRegister();
            if (objER.DBResponse.Data != null)
                ViewBag.RequestNo = (List<dynamic>)objER.DBResponse.Data;
            else ViewData["RequestNo"] = null;

            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Godown>)ObjGR.DBResponse.Data, "GodownId", "GodownName");
            }

            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
                ViewBag.ListOfCHA = (List<Hdb_CHA>)objER.DBResponse.Data;

            return PartialView(objCR);
        }
        [HttpGet]
        public ActionResult ListBTTCartingRegister()
        {
            List<Hdb_CartingRegister> objCR = new List<Hdb_CartingRegister>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllBTTRegisterDetails();
            if (objER.DBResponse.Data != null)
                objCR = (List<Hdb_CartingRegister>)objER.DBResponse.Data;
            return PartialView(objCR);
        }
        [HttpGet]
        public ActionResult ViewBTTCartingRegister(int CartingRegisterId)
        {
            Hdb_BTTCartingRegister objCR = new Hdb_BTTCartingRegister();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetBTTRegisterDetails(CartingRegisterId);
            if (objER.DBResponse.Data != null)
                objCR = (Hdb_BTTCartingRegister)objER.DBResponse.Data;
            return PartialView(objCR);
        }

        [HttpGet]
        public ActionResult EditBTTCartingRegister(int CartingRegisterId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Hdb_BTTCartingRegister ObjCartingReg = new Hdb_BTTCartingRegister();
            if (CartingRegisterId > 0)
            {
                ObjER.ListOfCHA();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.ListOfCHA = (List<Hdb_CHA>)ObjER.DBResponse.Data;

                ObjGR.GetAllGodown();
                if (ObjGR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Godown>)ObjGR.DBResponse.Data, "GodownId", "GodownName");
                }

                ObjER.GetBTTRegisterDetails(CartingRegisterId, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Hdb_BTTCartingRegister)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjCartingReg);
        }
        [HttpGet]
        public JsonResult GetBTTApplicationDetForRegister(string OldCFSCode)
        {
            int Status = 0;
            object obj = null;
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.ContainerDtlForBTTCartingRegister(OldCFSCode);
            if (objER.DBResponse.Data != null)
            {
                obj = objER.DBResponse.Data;
                Status = 1;
            }
            return Json(new { Status = Status, Data = obj }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBTTCartingRegister(Hdb_BTTCartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                IList<Hdb_BTTCartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Hdb_BTTCartingRegisterDtl>>(objCR.XMLData);
                string XML = Utility.CreateXML(LstCartingDtl);
                Hdb_ExportRepository objER = new Hdb_ExportRepository();
                objCR.Uid = ((Login)Session["LoginUser"]).Uid;
                objER.AddEditBTTCartingRegister(objCR, XML);
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
        public JsonResult DeleteBTTCartingRegister(int CartingRegisterId)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteBTTCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public JsonResult GetBTTGodownWiseLocation(int GodownId)
        {
            Hdb_ExportRepository objIR = new Hdb_ExportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintBTTCartingRegister(int CartingRegisterId)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.PrintBTTCartingReg(CartingRegisterId);
            if (ObjER.DBResponse.Data != null)
            {
                List<PrintCartingReg> LstCart = new List<PrintCartingReg>();
                LstCart = (List<PrintCartingReg>)ObjER.DBResponse.Data;
                string Path = GeneratePdfForCartReg(LstCart, CartingRegisterId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 1, Message = "Error" });
            }
        }
        [NonAction]
        public string GenerateBTTPdfForCartReg(List<PrintCartingReg> LstCart, int CartingRegisterId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CartingRegister" + CartingRegisterId + ".pdf";
            var Html = "";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /* if (System.IO.File.Exists(Path))
             {
                   System.IO.File.Delete(Path);
             } */
            int Index = 1;
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td width='80%' style='font-size: 10px;'></td><td width='10%' align='right'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='border:1px solid #333;'><div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/07</div></td></tr></tbody></table></td></tr></tbody></table></td></tr> <tr><td colspan='12'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Government of India Undertaking)</label><br/><span style='font-size:14px;'>Container Freight Station, Kukatpally <br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 14px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 16px; font-weight:bold;'>CARTING REGISTER</label></td><td valign='top' padding: 0 0 0 10px;><img align='right' src='ISO' width='100'/></td></tr></tbody></table></td></tr></tbody></table></td></tr>   <tr><td colspan='12'><table style='border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td><table style='width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px; padding:10px;'>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 130px; padding-top:10px;'>Carting No.<br/>& <br/>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 110px; padding:10px;'>S.B.<br/> No. & <br/>Date</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px; padding:10px;'>Name of Exporter</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>Name CHA</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 130px; padding:10px;'>Cargo</th><th colspan='3' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 150px;'><div style='padding:10px;'>Receipt</div></th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding:10px; width: 60px;'>FOB Value</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding:10px; width: 60px;'>Weight</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; padding:10px; text-align: center; width: 60px;'>Cargo Dect. & Ack. No.</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>As per S.B</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>Actual Recvd</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center; width: 50px;'>Balance</th></tr></thead> <tbody> ";
            foreach (PrintCartingReg item in LstCart)
            {
                Html += "<tr style='margin-bottom:10px;'> <td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 80px;'>" + item.RegisterDate + "</td><td style='border-right: 1px solid #000; font-size: 12px; text-align: center; width: 130px;'><div style='padding:5px;'>" + item.CartingRegisterNo + "</div></td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 110px;'><div style='padding:5px;'>" + item.ShipBillNo + "</div></td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>" + item.Exporter + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 150px;'>" + item.CHA + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 130px;'>" + item.CargoDescription + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.SBUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.ActualUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.BalanceUnits + "</td><td style='border-right: 1px solid #000; padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.FobValue + "</td><td style='border-right: 1px solid #000;padding:10px; font-size: 12px; text-align: center; width: 50px;'>" + item.Weight + "</td><td style='padding:10px; font-size: 12px; text-align: center; width: 50px;'>        </td></tr>";
                Index++;
            }
            Html += "</tbody> </table> </td></tr></tbody> </table> </td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            //using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/CartingRegister" + CartingRegisterId + ".pdf";
        }
        #endregion

        #region BTT Container Payment Sheet
        [HttpGet]
        public ActionResult CreateBTTContPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExp = new Hdb_ExportRepository();
            objExp.GetContainerListForBTTContPS();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objExp.GetPaymentPartyForExportnvoice();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            //objExp.GetPayeeForExportnvoice();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.PaymentPayee = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.PaymentPayee = null;

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPartyForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.PaymentParty = Jobject["lstParty"];
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
                ViewBag.PaymentPayee = Jobject["lstPayer"];
                ViewBag.StatePayer = Jobject["StatePayer"];
            }
            else
            {
                ViewBag.lstPayer = null;
            }

            return PartialView();
        }

        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType, int PartyId,
            List<Hdb_BttCont> lstPaySheetContainer, int CasualLabour,int Distance, int PayeeId, string ExportUnder = "", int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            Hdb_ExportRepository objHdbRepo = new Hdb_ExportRepository();
            objHdbRepo.GetBTTContPaymentSheet(InvoiceDate, XMLText, InvoiceType, PartyId, PayeeId, CasualLabour, Distance, InvoiceId, ExportUnder);
            var Output = (Hdb_BttContPS)objHdbRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "BTTCONT";

            Output.TotalNoOfPackages = Output.lstConatiner.Sum(o => o.NoOfPackages);
            Output.TotalGrossWt= Output.lstConatiner.Sum(o => o.GrossWt);
            Output.TotalWtPerUnit = Output.lstConatiner.Sum(o => o.WtPerUnit);
            Output.TotalSpaceOccupied = Output.lstConatiner.Sum(o => o.SpaceOccupied);
            Output.TotalValueOfCargo = Output.lstConatiner.Sum(o => o.CIFValue) + Output.lstConatiner.Sum(o => o.Duty);

            Output.TotalAmt = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Taxable);
            Output.TotalDiscount = 0;
            Output.TotalTaxable = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstContCharges.Where(o=>o.ChargeType=="CWC").ToList().Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Total);
            Output.InvoiceAmt = Math.Ceiling(Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Total));
            Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            Output.InvoiceType = InvoiceType;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditBTTContPaymentSheet(Hdb_BttContPS invoiceData)
        {
            try

            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                foreach (var item in invoiceData.lstConatiner)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" :Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd");
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstConatiner != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstConatiner);
                }
                if (invoiceData.lstPostContCharges != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostContCharges);
                }
                if (invoiceData.lstContWiseAmt != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmt);
                }
                if (invoiceData.lstOperationCode != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCode);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditBTTContInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTTCONT");

                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion
        #region LeoEntry
        public ActionResult LeoEntry()
        {
            return PartialView();
        }
        public JsonResult SearchMCIN(string SBNo, string SBDATE)
        {
            Hdb_ExportRepository objRepo = new Hdb_ExportRepository();
            objRepo.GetMCIN(SBNo, SBDATE);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLEO(LEOPage objLEOPage)
        {


            Hdb_ExportRepository objER = new Hdb_ExportRepository();

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
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();





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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();





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
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
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


        #region BTT Container Payment Sheet For Factory Stuff Container
        [HttpGet]
        public ActionResult CreateBTTContPaymentSheetForContainer(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Hdb_ExportRepository objExp = new Hdb_ExportRepository();
            objExp.GetContainerListForBTTContForFactoryStuffed();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
           

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPaymentPartyForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.PaymentParty = Jobject["lstParty"];
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
                ViewBag.PaymentPayee = Jobject["lstPayer"];
                ViewBag.StatePayer = Jobject["StatePayer"];
            }
            else
            {
                ViewBag.lstPayer = null;
            }

            return PartialView();
        }

        [HttpPost]
        public JsonResult GetContainerPaymentSheetForFactory(string InvoiceDate, int AppraisementId, string InvoiceType, int PartyId,
            List<Hdb_BttCont> lstPaySheetContainer, int CasualLabour, int PayeeId, string ExportUnder = "",string Printing="NO",int page=0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            Hdb_ExportRepository objHdbRepo = new Hdb_ExportRepository();
            objHdbRepo.GetBTTContPaymentSheetForFactoryStuffed(InvoiceDate, XMLText, InvoiceType, PartyId, PayeeId, CasualLabour, InvoiceId, ExportUnder, Printing, page);
            var Output = (Hdb_BttContPS)objHdbRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "BTTCONTFactoryStuff";

            Output.TotalNoOfPackages = Output.lstConatiner.Sum(o => o.NoOfPackages);
            Output.TotalGrossWt = Output.lstConatiner.Sum(o => o.GrossWt);
            Output.TotalWtPerUnit = Output.lstConatiner.Sum(o => o.WtPerUnit);
            Output.TotalSpaceOccupied = Output.lstConatiner.Sum(o => o.SpaceOccupied);
            Output.TotalValueOfCargo = Output.lstConatiner.Sum(o => o.CIFValue) + Output.lstConatiner.Sum(o => o.Duty);

            Output.TotalAmt = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Taxable);
            Output.TotalDiscount = 0;
            Output.TotalTaxable = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Taxable);
            Output.TotalCGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.CGSTAmt);
            Output.TotalSGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.SGSTAmt);
            Output.TotalIGST = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.IGSTAmt);
            Output.CWCTotal = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Total);
            Output.HTTotal = 0;
            Output.CWCTDS = 0;
            Output.HTTDS = 0;
            Output.CWCTDSPer = 0;
            Output.HTTDSPer = 0;
            Output.TDS = 0;
            Output.TDSCol = 0;
            Output.AllTotal = Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Total);
            Output.InvoiceAmt = Math.Ceiling(Output.lstContCharges.Where(o => o.ChargeType == "CWC").ToList().Sum(o => o.Total));
            Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            Output.InvoiceType = InvoiceType;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditBTTContPaymentSheetForFactory(Hdb_BttContPS invoiceData)
        {
            try

            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                foreach (var item in invoiceData.lstConatiner)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : Convert.ToDateTime(item.ArrivalDate).ToString("yyyy-MM-dd");
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                if (invoiceData.lstConatiner != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstConatiner);
                }
                if (invoiceData.lstPostContCharges != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostContCharges);
                }
                if (invoiceData.lstContWiseAmt != null)
                {
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmt);
                }
                if (invoiceData.lstOperationCode != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCode);
                }
                Hdb_ExportRepository objChargeMaster = new Hdb_ExportRepository();
                objChargeMaster.AddEditBTTContInvoiceForFactoryStuffed(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTTCONTStuff");

                invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        public ActionResult GetAllFactoryStuffInvoice()
        {
            Hdb_ExportRepository obj = new Hdb_ExportRepository();
            List<LoadedContainerStuffList> lstLoadedContainerStuffList = new List<LoadedContainerStuffList>();
            obj.GetFactoryStuffinvoiceList();
            lstLoadedContainerStuffList = (List<LoadedContainerStuffList>)obj.DBResponse.Data;
            return PartialView("GetAllFactoryStuffInvoice", lstLoadedContainerStuffList);
        }

        #endregion


        #region Ship Bill Amendment

        public ActionResult Amendment()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            //objER.GetAmenSBList();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfSBNoAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
            //else
            //{
            //    ViewBag.ListOfSBNoAmendment = null;
            //}
            ViewBag.ListOfSBNoAmendment = null;
            objER.ListOfExporterForSBAmend();
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


         //   objER.GetInvoiceListForShipbillAmend();
        //    if (objER.DBResponse.Data != null)
         //   {
          //      ViewBag.ListOfInv = JsonConvert.SerializeObject(objER.DBResponse.Data);
          //  }
          //  else
           // {
                ViewBag.ListOfInv = null;
           // }


            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);

            objER.ListOfPackUQCForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.PackUQCList = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            else
            {
                ViewBag.PackUQCList = null;
            }
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetShipbillNo()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAmenSBList();
            List<Hdb_Amendment> lstAmendment = new List<Hdb_Amendment>();
            lstAmendment = (List<Hdb_Amendment>)objER.DBResponse.Data;

            return Json(lstAmendment, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetShipbillNoSearch(string SB)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAmenSBListSearch(SB);
            List<Hdb_Amendment> lstAmendment = new List<Hdb_Amendment>();
            lstAmendment = (List<Hdb_Amendment>)objER.DBResponse.Data;

            return Json(lstAmendment, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSBDetailsBySBNo(string SBid, string SbDate)
        {
            Hdb_ExportRepository obj = new Hdb_ExportRepository();
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
        public JsonResult SaveAmendement(List<HdbOldInfoSb> vm, List<HdbNewInfoSb> newvm, string Date, string AmendmentNO, string FlagMerger)
        {
            Hdb_ExportRepository obj = new Hdb_ExportRepository();
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

                obj.AddEditAmendment(OldInfoSbXml, NewInfoSbSbXml, Date, FlagMerger);


                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

            }

        }


        public JsonResult GetAmendDetails(string AmendNo)
        {

            Hdb_ExportRepository obj = new Hdb_ExportRepository();
            obj.GetAmenSBDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmendDetailsByAmendNo(string AmendNo)
        {

            Hdb_ExportRepository obj = new Hdb_ExportRepository();
            obj.GetAmenDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SubmitAmedData(Hdb_AmendmentViewModel vm)
        {
            Hdb_ExportRepository obj = new Hdb_ExportRepository();
            obj.AddEditShipAmendment(vm);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListofAmendData()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            List<Hdb_AmendmentViewModel> lstdata = new List<Hdb_AmendmentViewModel>();
            objER.ListForShipbillAmend();
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Hdb_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }
        [HttpGet]
        public ActionResult ViewAmendData(int id)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            Hdb_AmendmentViewModel obj = new Hdb_AmendmentViewModel();
            objER.GetShipbillAmendDet(id);
            if (objER.DBResponse.Status == 1)
            {
                obj = (Hdb_AmendmentViewModel)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ViewMergeSplitData(string AmendNo)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            List<Hdb_Amendment> obj = new List<Hdb_Amendment>();
            objER.GetAmenDetailsByAmendNO(AmendNo);
            if (objER.DBResponse.Status == 1)
            {
                obj = (List<Hdb_Amendment>)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ListofMergeSplitAmendData()
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            List<Hdb_AmendmentViewModel> lstdata = new List<Hdb_AmendmentViewModel>();
            objER.GetAmenSBDetailsByAmendNO("");
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<Hdb_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        public JsonResult GetAllCommodityDetailsForAmendmend(string CommodityName, int Page)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetAllCommodityForPageAmendment(CommodityName, Page);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);

        }




        #endregion

        //public async Task<JsonResult> GetIRNForExportInvoice(String InvoiceNo)
        //{


        //    Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    objPpgRepo.GetIRNForExport(InvoiceNo);
        //    var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

        //    objPpgRepo.GetHeaderIRNForExport();

        //    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

        //    string jsonEInvoice = JsonConvert.SerializeObject(Output);
        //    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

        //    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);
        //    IrnResponse ERes = await Eobj.GenerateEinvoice();
        //    // var Images = LoadImage(ERes.QRCodeImageBase64);
        //    objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);

        //    return Json(objPpgRepo.DBResponse);
        //}

        //public async Task<JsonResult> GetIRNForExportInvoice(String InvoiceNo, string SupplyType)
        //{


        //    Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
        //    //objChrgRepo.GetAllCharges();
        //    if (SupplyType == "B2B" || SupplyType == "SEZ")
        //    {
        //        objPpgRepo.GetIRNForExport(InvoiceNo);
        //        var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

        //        objPpgRepo.GetHeaderIRNForExport();

        //        HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

        //        string jsonEInvoice = JsonConvert.SerializeObject(Output);
        //        string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

        //        Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

        //        IrnResponse ERes = await Eobj.GenerateEinvoice();
        //        if (ERes.Status == 1)
        //        {
        //            objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
        //        }
        //        else
        //        {
        //            objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
        //            objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
        //        }

        //    }
        //    else
        //    {
        //        Einvoice Eobj = new Einvoice();
        //        objPpgRepo.GetIRNB2CForExport(InvoiceNo);
        //        Hdb_IrnB2CDetails irnb2cobj = new Hdb_IrnB2CDetails();
        //        irnb2cobj = (Hdb_IrnB2CDetails)objPpgRepo.DBResponse.Data;

        //        IrnModel irnModelObj = new IrnModel();
        //        irnModelObj.DocumentDate = irnb2cobj.DocDt;
        //        irnModelObj.DocumentNo = irnb2cobj.DocNo;
        //        irnModelObj.DocumentType = irnb2cobj.DocTyp;
        //        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
        //        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
        //        var dt = DateTime.Now;
        //        QrCodeInfo obj = new QrCodeInfo();
        //        QrCodeData objQR = new QrCodeData();
        //        objQR.Irn = ERes;
        //        objQR.IrnDt = dt.ToString("dd/MM/yyyy");
        //        objQR.iss = "NIC";
        //        objQR.ItemCnt = irnb2cobj.ItemCnt;
        //        objQR.MainHsnCode = irnb2cobj.MainHsnCode;
        //        objQR.SellerGstin = irnb2cobj.SellerGstin;
        //        objQR.TotInvVal = irnb2cobj.TotInvVal;
        //        objQR.BuyerGstin = irnb2cobj.BuyerGstin;
        //        objQR.DocDt = irnb2cobj.DocDt;
        //        objQR.DocNo = irnb2cobj.DocNo;
        //        objQR.DocTyp = irnb2cobj.DocTyp;
        //        obj.Data = (QrCodeData)objQR;
        //        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
        //        objresponse = Eobj.GenerateB2cQRCode(obj);
        //        IrnResponse objERes = new IrnResponse();
        //        objERes.irn = ERes;
        //        objERes.SignedQRCode = objresponse.QrCodeBase64;
        //        objERes.SignedInvoice = objresponse.QrCodeJson;
        //        objERes.SignedQRCode = objresponse.QrCodeJson;

        //        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);

        //    }


        //    return Json(objPpgRepo.DBResponse);

        //}


        public async Task<JsonResult> GetIRNForExportInvoice(String InvoiceNo, string SupplyType)
        {


            Hdb_ExportRepository objPpgRepo = new Hdb_ExportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForExport(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;
                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForExport();

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
                    objPpgRepo.GetIRNB2CForExport(InvoiceNo);
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
                        idata.tn = tn;
                        idata.pa = irnb2cobj.pa;
                        idata.pn = irnb2cobj.pn;
                        idata.mc = "0000";
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

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForExport(InvoiceNo);
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
                    idata.tn = tn;
                    idata.pa = irnb2cobj.pa;
                    idata.pn = irnb2cobj.pn;
                    idata.mc = "0000";
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


        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<Hdb_ListOfExpInvoice> obj = new List<Hdb_ListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Hdb_ListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            Hdb_ExportRepository objExp = new Hdb_ExportRepository();
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
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Hdb_ExportRepository objCR = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }
        [HttpGet]
        public ActionResult EditContainerStuffingApproval(int ApprovalId)
        {
            PortOfCall ObjStuffing = new PortOfCall();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
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
                            throw new InvalidOperationException("Logfile cannot be read-only");
                        }
                        log.Error("call Class Libarary DOne .....");


                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Hdb_ReportfileCIMSF"];

                        log.Error("Done Hdb_ReportfileCIMSF .....");
                        string FileName = strFolderName + Filenm;
                        log.Info("strFolderName:"+ strFolderName);
                        log.Info("FullPath:"+ FileName);
                        if (!Directory.Exists(strFolderName))
                        {
                            Directory.CreateDirectory(strFolderName);
                        }
                        log.Info("Before File Creation");

                        System.IO.File.Create(FileName).Dispose();

                        System.IO.File.WriteAllText(FileName, JsonFile);
                        string output = "";
                        log.Info("After File Creation");



                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];

                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFile"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFile"];
                        string ArchiveInJsonFilePath = System.Configuration.ConfigurationManager.AppSettings["ArchiveInJsonFilePath"];
                        string DSCPATH = System.Configuration.ConfigurationManager.AppSettings["DSCPATH"];
                        string DSCPASSWORD = Convert.ToString(ds.Tables[6].Rows[0]["DSCPASSWORD"]);
                        log.Info("apiUrl:"+ apiUrl);
                        log.Info("InJsonFile:" + InJsonFile);
                        log.Info("OUTJsonFile:" + OUTJsonFile);
                        log.Info("ArchiveInJsonFilePath:" + ArchiveInJsonFilePath);
                        log.Info("DSCPATH:" + DSCPATH);
                        
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
                            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                            objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
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
            catch(Exception ex)
            {
                return Json(new { Status = 1, Message = "CIM SF File Send Fail." });

            }



















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
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                        Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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

        #region  Send ASR 
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> SendASR(int ContainerStuffingId)
        {
            try
            {
                int k = 0;
                int j = 1;
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                //PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
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
                            throw new InvalidOperationException("Logfile cannot be read-only");
                        }




                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMASR"];
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

                            //File Upload Fail
                            //if (status.ToString() == "Success")
                            //{
                            //    using (FileStream fs = System.IO.File.OpenRead(FinalOutPutPath))
                            //    {
                            //        log.Error("File Open:" + OUTJsonFile);
                            //        log.Info("FTP File read process has began");
                            //        byte[] buffer = new byte[fs.Length];
                            //        fs.Read(buffer, 0, buffer.Length);
                            //        fs.Close();
                            //        string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
                            //        log.Error("SCMTRPath:" + SCMTRPath);
                            //        // log.Error("SCMTR File Name:" + OUTJsonFile+ Filenm);
                            //        output = FtpFileManager.UploadFileToFtp(SCMTRPath, Filenm, buffer, "5000", FinalOutPutPath);
                            //        log.Info("FTP File read process has ended");
                            //    }
                            //}
                            //else
                            //{
                            //    output = "Error";
                            //}

                            //For Block If Develpoment Done Then Unlock

                        }






                        log.Error("output: " + output);









                        //if (output == "Success")
                        //{
                        //    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                        //    objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");

                    }
                    Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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
            Hdb_ExportRepository objExp = new Hdb_ExportRepository();
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
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
                Hdb_ExportRepository objCR = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetLoadContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion

        #region  Loaded Container SF Send

        [HttpGet]
        public ActionResult CreateLoadContainerSF()
        {
            Hdb_ExportRepository objExp = new Hdb_ExportRepository();
            Hdb_LoadContSF ObjPC = new Hdb_LoadContSF();            
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
            Hdb_LoadContSF ObjStuffing = new Hdb_LoadContSF();

            if (LoadContReqId > 0)
            {
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                ObjER.GetLoadContainerStuffingSFById(LoadContReqId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Hdb_LoadContSF)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingSF(Hdb_LoadContSF objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Hdb_ExportRepository objCR = new Hdb_ExportRepository();
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
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            List<Hdb_LoadContSF> LstStuffingApproval = new List<Hdb_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Hdb_LoadContSF>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingSF", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingSFist(int Page, string SearchValue = "")
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            var LstStuffingApproval = new List<Hdb_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Hdb_LoadContSF>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingSF(int ApprovalId)
        {
            Hdb_LoadContSF ObjStuffing = new Hdb_LoadContSF();
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetLoadContainerStuffingSFById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Hdb_LoadContSF)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion        

        #region  Loaded Container Send ASR 
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> LoadedContainerSendASR(int ContainerStuffingId)
        {

            try
            {
                int k = 0;
                int j = 1;
                Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
                //PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
                ObjER.GetLoadedContainerCIMASRDetails(ContainerStuffingId, "F");
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
                            throw new InvalidOperationException("Logfile cannot be read-only");
                        }




                        string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMASR"];
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

                            //File Upload Fail
                            //if (status.ToString() == "Success")
                            //{
                            //    using (FileStream fs = System.IO.File.OpenRead(FinalOutPutPath))
                            //    {
                            //        log.Error("File Open:" + OUTJsonFile);
                            //        log.Info("FTP File read process has began");
                            //        byte[] buffer = new byte[fs.Length];
                            //        fs.Read(buffer, 0, buffer.Length);
                            //        fs.Close();
                            //        string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
                            //        log.Error("SCMTRPath:" + SCMTRPath);
                            //        // log.Error("SCMTR File Name:" + OUTJsonFile+ Filenm);
                            //        output = FtpFileManager.UploadFileToFtp(SCMTRPath, Filenm, buffer, "5000", FinalOutPutPath);
                            //        log.Info("FTP File read process has ended");
                            //    }
                            //}
                            //else
                            //{
                            //    output = "Error";
                            //}

                            //For Block If Develpoment Done Then Unlock

                        }






                        log.Error("output: " + output);









                        //if (output == "Success")
                        //{
                        //    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                        //    objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}
                        log.Info("FTP File upload has been end");

                    }
                    Hdb_ExportRepository objExport = new Hdb_ExportRepository();
                    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);

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

        #region Download SCMTR File
        public FileResult DownloadFile(string Path, string FileName)
        {
            return File(Path, "application/json", FileName);
        }
        #endregion

        #region ACTUAL ARRIVAL DATE AND TIME 
        [HttpGet]
        public ActionResult ActualArrivalDateTime()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDB_ActualArrivalDatetime objActualArrival = new HDB_ActualArrivalDatetime();

            ObjER.GetContainerNoForActualArrival("",0);
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


        public async Task<JsonResult> SendAT(string CFSCode)
        {

            int k = 0;
            int j = 1;
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
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



                return Json(new { Status = 1, Message = "CIM AT File Send Successfully." },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpGet]
        public JsonResult LoadArrivalDatetimeContainerList(string ContainerBoxSearch, int Page)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchContainer(string ContainerBoxSearch,int Page)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditActualArrivalDatetime(HDB_ActualArrivalDatetime objActualArrivalDatetime)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
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
            List<HDB_ActualArrivalDatetime> lstActualArrivalDatetime = new List<HDB_ActualArrivalDatetime>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            //objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, 0);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<HDB_ActualArrivalDatetime>)objER.DBResponse.Data;
            return PartialView(lstActualArrivalDatetime);
        }

        [HttpGet]
        public JsonResult EditActualArrivalDatetime(int actualArrivalDatetimeId)
        {
            List<HDB_ActualArrivalDatetime> lstActualArrivalDatetime = new List<HDB_ActualArrivalDatetime>();
            Hdb_ExportRepository objER = new Hdb_ExportRepository();
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, actualArrivalDatetimeId);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<HDB_ActualArrivalDatetime>)objER.DBResponse.Data;
            //return Json(lstActualArrivalDatetime);
            return Json(lstActualArrivalDatetime, JsonRequestBehavior.AllowGet);
        }

         #region LoaderContUpdate
        public ActionResult LoaderContUpdate()
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            HDB_ActualArrivalDatetime objContainerLoad = new HDB_ActualArrivalDatetime();

            ObjER.GetContainerNoORLoadContReefer("", 0);
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
            objContainerLoad.ArrivalDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
         
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateContainerLoaderReefer(HDB_ActualArrivalDatetime objActualArrivalDatetime)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime dateValue;
            try
            {
                if (ModelState.IsValid)
                {
                    

                    ObjER.SaveUpdateContainerLoaderReefer(objActualArrivalDatetime);
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
        public JsonResult LoadContainerLoaderReeferList(string ContainerBoxSearch, int Page)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoORLoadContReefer(ContainerBoxSearch, Page);

            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ContainerNoList = Jobject["ContainerList"];
                ViewBag.StateCont = Jobject["State"];
            }
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
          
        }

        [HttpGet]
        public JsonResult SearchContainerLoad(string ContainerBoxSearch, int Page)
        {
            Hdb_ExportRepository ObjER = new Hdb_ExportRepository();
            ObjER.GetContainerNoORLoadContReefer(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}