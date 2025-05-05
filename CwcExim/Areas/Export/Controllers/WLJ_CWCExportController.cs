using CwcExim.Areas.Export.Models;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using CwcExim.Controllers;
using CwcExim.Filters;
using System.IO;
using System.Text;
using CwcExim.Areas.Report.Models;
using SCMTRLibrary;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CwcExim.Areas.Export.Controllers
{
    public class WLJ_CWCExportController : BaseController
    {
        // GET: Export/WLJ_CWCExportController
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region CCINEntry
        public ActionResult CCINEntry(int Id = 0, int PartyId = 0)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ObjER.GetCCINEntryForEdit(Id);
                if (ObjER.DBResponse.Data != null)
                {
                    objCCINEntry = (Wlj_CCINEntry)ObjER.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetPortByCountry(int CountryId)
        {
            WLJ_ExportRepository ObjRR = new WLJ_ExportRepository();
            ObjRR.GetPortOfLoadingForCCIN(CountryId);
            if (ObjRR.DBResponse.Status > 0)
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult LoadCCINShippingLine(string PartyCode, int Page)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadExporterList(string PartyCode, int Page)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.ListOfExporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            WLJ_ExportRepository ObjRR = new WLJ_ExportRepository();
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



        [HttpGet]
        public JsonResult SearchCCINShippingLineByPartyCode(string PartyCode)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            WLJ_ExportRepository objRepo = new WLJ_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult LoadPackUQCList(string PartyCode, int Page)
        {
            VRN_ExportRepository objRepo = new VRN_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPackUQCByCode(string PartyCode)
        {
            VRN_ExportRepository objRepo = new VRN_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetGateEntryDetForCCIN(int EntryId)
        {
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetCCINByGateEntryId(EntryId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetCCINCharges(CCINEntryId, PartyId, Weight, FOB, CargoType);
            objCCINEntry = (Wlj_CCINEntry)objExport.DBResponse.Data;
            ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntry(Wlj_CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            ModelState.Remove("ExporterId");
            ModelState.Remove("ExporterName");
            ModelState.Remove("PartyId");
            ModelState.Remove("PartyName");
            if (ModelState.IsValid)
            {
                WLJ_ExportRepository objER = new WLJ_ExportRepository();
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
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.GetCCINEntry(Id);
            if (objER.DBResponse.Data != null)
                objCCINEntry = (Wlj_CCINEntry)objER.DBResponse.Data;
            return PartialView("ViewCCINEntry", objCCINEntry);
        }


        [HttpGet]
        public ActionResult ListOfCCINEntry()
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> lstCCINEntry = new List<Wlj_CCINEntry>();
            objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Wlj_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        #region CCINEntry Search
        [HttpGet]
        public ActionResult ListOfCCINEntrySearch(string search)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> lstCCINEntry = new List<Wlj_CCINEntry>();
            objER.GetAllCCINEntryForSearch(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Wlj_CCINEntry>)objER.DBResponse.Data;
            return PartialView("ListOfCCINEntry", lstCCINEntry);
        }




        #endregion

        [HttpGet]
        public JsonResult LoadMoreCCINEntryList(int Page)
        {
            WLJ_ExportRepository ObjCR = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> LstJO = new List<Wlj_CCINEntry>();
            ObjCR.GetAllCCINEntryForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Wlj_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntry(int CCINEntryId)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetCCINEntrySlipReport(String Ccinno)
        {

            //WFLD_ReportRepository objccin = new WFLD_ReportRepository();
            //objccin.GetContainerStuffForPrint(CCINId);
            //object data = null;
            //if (objccin.DBResponse.Data != null)
            //    data = objccin.DBResponse.Data;
            //return Json(data, JsonRequestBehavior.AllowGet);
            WLJ_ExportRepository objccin = new WLJ_ExportRepository();
            WLJ_CCINPrint ObjStuffing = new WLJ_CCINPrint();
            objccin.GetCcinEntrySlipForPrint(Ccinno);
            if (objccin.DBResponse.Data != null)
            {
                ObjStuffing = (WLJ_CCINPrint)objccin.DBResponse.Data;
                string Path = GeneratingCCINEntrySLIP(ObjStuffing);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

            //if (ObjBulkInvoiceReport.InvoiceNumber == null)
            //{
            //    ObjBulkInvoiceReport.InvoiceNumber = "";
            //}
            //WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            //Login objLogin = (Login)Session["LoginUser"];
            ////When Module is selected All Condition against a party


            //string ModuleName = "";
            ////delete all the files in the folder before creating it
            //if (System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
            //{
            //    string deletelocation = Server.MapPath("~/Docs/All/") + Session.SessionID + "/";
            //    DeleteDirectory(deletelocation);
            //}

            ////get all the distinct invoice module and invoices list with respect to party and date range 

            //string FilePath = "";



            //FilePath = GeneratingCCINSLIP(ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);



            //return Json(new { Status = 1, Data = FilePath });


        }
        public string GeneratingCCINEntrySLIP(WLJ_CCINPrint ObjStuffing)
        {

            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            var Address = "";
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            Address = objCompanyDetails.CompanyAddress;
            var FileName = "";
            var location = "";
            var GodownNo = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            //List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            //List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            //List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            //List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            //List<dynamic> lstSBDetails = new List<dynamic>();
            //if (ds.Tables.Count > 11)
            //{
            //    lstSBDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[11]);
            //}

            ObjStuffing.Lstshed.Select(x => new { GodownNo = x.GodownNo }).Distinct().ToList().ForEach(item =>
            {

                GodownNo = item.GodownNo;

            });
            List<string> lstSB = new List<string>();
            //lstInvoice.ToList().ForEach(item =>
            // {
            // WFLD_ReportRepository rep = new WFLD_ReportRepository();
            //  WFLDSDBalancePrint objSDBalance = new WFLDSDBalancePrint();
            //rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
            //if (rep.DBResponse.Data != null)
            //{
            //    objSDBalance = (WFLDSDBalancePrint)rep.DBResponse.Data;
            //}
            StringBuilder html = new StringBuilder();
            /*Header Part*/

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td width='65%'></td>");
            html.Append("<td width='10%' align='right'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 7pt; text-align: center;'>F/CD/CFS/08</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td colspan='8' width='75%' style='text-align: center;'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
            html.Append("<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 7pt;text-transform: uppercase; padding-bottom: 10px;'>" + Address + "</span>");
            html.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>CARGO CARTED IN - SLIP</label>");
            html.Append("</td>");
            html.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td>");
            html.Append("</tr>");

            //html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            //html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
            //html.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
            //html.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>107-109 , EPIP Zone , KIADB Industrial Area <br/> Whitefield, Bengaluru - 560066 </span>");
            //html.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>CARGO CARTED IN - SLIP</label>");
            //html.Append("</td>");
            //html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='border: 1px solid #000; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr>");
            html.Append("<td colspan='6' width='50%'>");
            html.Append("<table style='border-right: 1px solid #000; width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td colspan='6' width='30%'>CCIN No.</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.CCINNO + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>SB No.</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.SBNo + "</td></tr>");
            // html.Append("<tr><td colspan='6' width='30%'>Invoice No.</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.InvoiceNo + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>Name of Exporter</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.Exporter + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>Shed/Godown No.</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.GodownNo + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>No of Pkgs</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.NoofPkg + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>Gross Weight(Kg)</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.GrossWeight + "</td></tr>");
            // html.Append("<tr><td colspan='6' width='30%'>Cargo Invoice No.</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.CargoInvNo + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>Port Of Destination</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.PortDestName + "</td></tr>");
            html.Append("<tr><td colspan='6' width='30%'>Package Type</td><td>:</td><td colspan='6' width='70%'>" + ObjStuffing.PackageType + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("<td colspan='6' width='50%'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td colspan='6' width='40%'>CCIN Date</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.CCINDate + "</td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>SB Date.</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.SBDate + "</td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>CHA</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.CHA + "</td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>Country</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.Country + "</td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>Cargo Type</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.CargoType + "</td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>Cargo(RMG/GEN)</td><td>:</td><td colspan='6' width='60%'></td></tr>");
            html.Append("<tr><td colspan='6' width='40%'>FOB (INR)</td><td>:</td><td colspan='6' width='60%'>" + ObjStuffing.FOB + "</td></tr>");

            html.Append("<tr><td colspan='6' width='30%'></td><td></td><td colspan='6' width='70%'></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");


            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr>");
            html.Append("<td colspan='6' width='50%' valign='top'>");
            html.Append("<table style='width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead>");
            html.Append("<tr><th colspan='12' style='font-size:7pt; text-align:left; border-bottom:1px solid #000;'>Carting allowed subject to availability of warehousing space</th></tr>");

            html.Append("<tr><th colspan='2' style='border: 1px solid #000; border-top: 0; width: 70%;'>For Exporter/CHA</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 30%;'>Security</th></tr>");
            html.Append("<tr><th style='border: 1px solid #000; border-top: 0; width: 50%;'>Sr.no.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50%;'>Vehicle No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50%;'>In</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50%;'>Out</th></tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            html.Append("<tr><td style='border: 1px solid #000; border-top: 0;'></td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></td>");
            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'></td></tr>");
            html.Append("</tbody>");

            html.Append("<tr><th><span><br/><br/><br/><br/><br/></span></th></tr>");
            html.Append("<tr><th colspan='12' style='font-size:7pt; text-align:left;'>( Sign.of Exporter/CHA ID No. )</th></tr>");
            html.Append("</table>");
            html.Append("</td>");

            html.Append("<td colspan='6' width='50%'>");
            html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead>");
            html.Append("<tr><th colspan='12' style='font-size:7pt; text-align:left; border-bottom:1px solid #000;'>( Name & Sign. of Unit I/C )</th></tr>");
            html.Append("<tr><th colspan='12' style='font-size:6pt; text-align:left;'>For Gate Incharge</th></tr>");
            html.Append("<tr><th colspan='12' style='font-size:6pt; text-align:left;'>Entry No. :</th></tr>");

            html.Append("<tr><th style='width: 50%;'><u>In</u></th>");
            html.Append("<th style='width: 50%;'><u>Out</u></th></tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            html.Append("<tr><td><b>Date :</b></td>");
            html.Append("<td><b>Date :</b></td></tr>");
            html.Append("<tr><td><b>Time :</b></td>");
            html.Append("<td><b>Time :</b></td></tr>");
            html.Append("</tbody>");

            html.Append("<tr><th><span><br/><br/></span></th></tr>");
            html.Append("<tr><th colspan='12' style='font-size:7pt; text-align:left;'>( Sign.of Gate Incharge )</th></tr>");
            html.Append("</table>");
            html.Append("</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr>");


            html.Append("<tr><td colspan='12' cellpadding='5' style='text-align:center; font-size:7pt;'>SHED ENTRIES</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='border:1px solid #000; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr><th style='border-right:1px solid #000; width:8%;'>Sr. No.</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>Date of Carting</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>Open/Shed/Cont.</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>Shed/Godown Container No.</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>Bay/Grid No.</th>");
            html.Append("<th style='border-right:1px solid #000; width:15%;'>General/Reserved</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>Area(sq mt)</th>");
            html.Append("<th style='border-right:1px solid #000; width:8%;'>No. of Pkg</th>");
            html.Append("<th style='border-right:1px solid #000; width:15%;'>Gr.Wt.</th>");
            html.Append("<th style='border-right:1px solid #000; width:15%;'>Short Pkg</th>");
            html.Append("<th style='width:8%;'>Excess Pkg</th></tr></thead>");
            html.Append("<tbody>");

            int i = 1;


            if (GodownNo == "")
            {

                html.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                html.Append("<td style='border-top:1px solid #000;'></td></tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td></tr>");

            }
            else
            {
                ObjStuffing.Lstshed.ForEach(item =>
                {
                    html.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'>" + i++ + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.CartingDate + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.GodownNo + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.SpaceType + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.Area + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.NoOfPkg + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.GrossWeight + "</td>");
                    html.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.ShortPkg + "</td>");
                    html.Append("<td style='border-top:1px solid #000;'>" + item.ExcessPkg + "</td></tr>");
                    html.Append("</tbody>");
                    html.Append("</table>");
                    html.Append("</td></tr>");
                });
            }
            html.Append("<tr><td><span><br/></span></td></tr>");

            html.Append("<tr><td colspan='12' cellpadding='5' style='font-size:6pt;'><b>Condition of Cargo :</b></td></tr>");

            html.Append("<tr><td colspan='12' cellpadding='5' style='font-size:7pt;'><b>Remarks :</b> Cargo Received on said to contain and said to weight basis.</td></tr>");

            html.Append("<tr><td><span><br/></span></td></tr>");

            html.Append("<tr><th colspan='6' width='50%' style='font-size:7pt;'>( Sign. of Exporter/CHA )</th> <th colspan='6' width='50%' style='text-align:right;font-size:7pt;'>( Sign. of Shed Incharge )</th></tr>");

            html.Append("</tbody></table>");


            /***************/
            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));



            lstSB.Add(html.ToString());
            // });

            FileName = "CCINSLIPReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
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

        #region CCINEntry Approval
        public ActionResult CCINEntryApproval(int Id = 0)
        {
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            WLJ_ExportRepository objRepository = new WLJ_ExportRepository();
            objRepository.GetAllCCINEntry();
            if (objRepository.DBResponse.Data != null)
            {
                ViewBag.ListOfCCINNo = (List<Wlj_CCINEntry>)objRepository.DBResponse.Data;
            }

            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                WLJ_ExportRepository rep = new WLJ_ExportRepository();
                rep.GetCCINEntryById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (Wlj_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetCCINEntryApprovalDetails(int CCINEntryId)
        {
            Wlj_CCINEntry objCCINEntry = new Wlj_CCINEntry();
            if (CCINEntryId > 0)
            {
                WLJ_ExportRepository rep = new WLJ_ExportRepository();
                rep.GetCCINEntryById(CCINEntryId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (Wlj_CCINEntry)rep.DBResponse.Data;
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
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.AddEditCCINEntryApproval(Id, IsApproved);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntryApproval()
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> lstCCINEntry = new List<Wlj_CCINEntry>();
            objER.GetAllCCINEntryApprovalForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Wlj_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryApprovalList(int Page)
        {
            WLJ_ExportRepository ObjCR = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> LstJO = new List<Wlj_CCINEntry>();
            ObjCR.GetAllCCINEntryApprovalForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Wlj_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntrySearchApproval(string search)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_CCINEntry> lstCCINEntry = new List<Wlj_CCINEntry>();
            objER.GetAllCCINEntryForSearchApproval(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<Wlj_CCINEntry>)objER.DBResponse.Data;
            return PartialView("ListOfCCINEntryApproval", lstCCINEntry);
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


            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            Wlj_CartingRegister objCR = new Wlj_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("yyyy-MM-dd");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<ApplicationNoDet>)objER.DBResponse.Data;

            WLJ_ExportRepository objE = new WLJ_ExportRepository();
            objE.ListOfCHA();
            if (objE.DBResponse.Data != null)
            {
                ViewBag.CHAList = (List<CHA>)objE.DBResponse.Data;
            }

            objE.GetCCINShippingLine();
            if (objE.DBResponse.Data != null)
            {
                ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objE.DBResponse.Data).ToString();
                ViewBag.ListOfShippingLine = objE.DBResponse.Data;
            }

            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            objE.ListOfExporter();
            if (objE.DBResponse.Data != null)
                ViewBag.ExporterList = (List<Exporter>)objE.DBResponse.Data;

            objE.GetAllCommodityForPage("", 0);
            if (objE.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objE.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }


            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister()
        {
            List<Wlj_CartingRegister> objCR = new List<Wlj_CartingRegister>();
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.GetAllRegisterDetails(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
                objCR = (List<Wlj_CartingRegister>)objER.DBResponse.Data;

            return PartialView("ListCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId)
        {
            Wlj_CartingRegister objCR = new Wlj_CartingRegister();
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "view");
            if (objER.DBResponse.Data != null)
                objCR = (Wlj_CartingRegister)objER.DBResponse.Data;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_CartingRegister ObjCartingReg = new Wlj_CartingRegister();
            GodownRepository ObjGR = new GodownRepository();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (Wlj_CartingRegister)ObjER.DBResponse.Data;
                }
            }

            //***************************************************************************
            WLJ_ExportRepository ObjE = new WLJ_ExportRepository();
            ObjE.ListOfCHA();
            if (ObjE.DBResponse.Data != null)
                ViewBag.CHAList = (List<CHA>)ObjE.DBResponse.Data;


            List<Godown> lstGodown = new List<Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            ObjE.ListOfExporter();
            if (ObjE.DBResponse.Data != null)
                ViewBag.ExporterList = (List<Exporter>)ObjE.DBResponse.Data;

            ObjE.GetAllCommodity();
            if (ObjER.DBResponse.Data != null)
                ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjE.DBResponse.Data;


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
            Wlj_CartingRegister objCR = new Wlj_CartingRegister();
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (Wlj_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(Wlj_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                objCR.ApplicationDate = Convert.ToDateTime(objCR.ApplicationDate).ToString("dd/MM/yyyy");
                //List<int> lstLocation = new List<int>();
                IList<Wlj_CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Wlj_CartingRegisterDtl>>(objCR.XMLData);

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
                WLJ_ExportRepository objER = new WLJ_ExportRepository();
                objCR.Uid = ((Login)Session["LoginUser"]).Uid;
                objER.AddEditCartingRegister(objCR, XML);
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
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }


        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.GodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.GodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddShortCargoDetail(Wlj_ShortCargoDetails objSC, int CartingRegisterId, int CartingRegisterDtlId, string ShippingBillNo)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_ShortCargoDetails> lstShortCargoDetails = new List<Wlj_ShortCargoDetails>();
            objSC.CartingDate = Convert.ToDateTime(objSC.CartingDate).ToString("yyyy-MM-dd");
            lstShortCargoDetails.Add(objSC);
            objER.AddShortCargoDetail(Utility.CreateXML(lstShortCargoDetails), CartingRegisterId, CartingRegisterDtlId, ShippingBillNo);
            return Json(objER.DBResponse);
        }

        [HttpGet]
        public ActionResult ListOfCartingSearch(string search)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<Wlj_CartingRegister> lstCart = new List<Wlj_CartingRegister>();
            objER.GetAllCartingEntryForSearch(0, search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCart = (List<Wlj_CartingRegister>)objER.DBResponse.Data;
            return PartialView("ListCartingRegister", lstCart);
        }

        public JsonResult LoadMoreCartingList(int Page)
        {
            WLJ_ExportRepository ObjCR = new WLJ_ExportRepository();
            List<Wlj_CartingRegister> LstJO = new List<Wlj_CartingRegister>();
            ObjCR.GetAllCartingForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Wlj_CartingRegister>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            Wlj_StuffingRequest ObjSR = new Wlj_StuffingRequest();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();

            ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<Wlj_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
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
            { ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
            else
            { ViewBag.ShippingLineList = null; }

            ObjER.GetForwarder();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ForwarderList = new SelectList((List<ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            else
                ViewBag.ForwarderList = null;

            ObjER.GetAllContainerNo();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<Wlj_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
                ViewBag.ContainerList = null;

            ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = ObjER.DBResponse.Data;
            }
            else
                ViewBag.ListOfPOD = null;

            ObjER.ListOfCityForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListCity = ObjER.DBResponse.Data;
            }
            else
                ViewBag.ListCity = null;

            ObjER.GetPackUQCForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.PackUQCList = new SelectList((List<PackUQCForPage>)ObjER.DBResponse.Data, "PackUQCCode", "PackUQCDescription");
                ViewBag.PackUQCJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.PackUQCList = null;
            }
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStuffingReq(Wlj_StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
                IList<Wlj_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<Wlj_StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<Wlj_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<Wlj_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
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
            Wlj_StuffingRequest ObjStuffing = new Wlj_StuffingRequest();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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

            ObjER.GetForwarder();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ForwarderList = new SelectList((List<ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            else
                ViewBag.ForwarderList = null;
            ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = ObjER.DBResponse.Data;
            }
            else
                ViewBag.ListOfPOD = null;

            ObjER.ListOfCityForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListCity = ObjER.DBResponse.Data;
            }
            else
                ViewBag.ListCity = null;

            ObjER.GetPackUQCForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.PackUQCList = new SelectList((List<PackUQCForPage>)ObjER.DBResponse.Data, "PackUQCCode", "PackUQCDescription");
                ViewBag.PackUQCJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.PackUQCList = null;
            }

            if (StuffinfgReqId > 0)
            {
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Wlj_StuffingRequest)ObjER.DBResponse.Data;
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
            }
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            Wlj_StuffingRequest ObjStuffing = new Wlj_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
                ObjER.Kdl_GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Wlj_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            if (CartingRegisterId > 0)
            {
                objER.Kdl_GetCartRegDetForStuffingReq(CartingRegisterId);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }
        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            // StuffingReqContainerDtl ObjSRD = new StuffingReqContainerDtl();
            ObjER.GetContainerNoDet(CFSCode);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ObjSRD = (StuffingReqContainerDtl)ObjER.DBResponse.Data;
            //}
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchStuffingReq(string ContNo)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            ObjER.SearchStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }

        [HttpGet]
        public ActionResult LoadStuffingReqList(int Page)
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            List<Wlj_StuffingRequest> LstStuffing = new List<Wlj_StuffingRequest>();
            ObjER.GetAllStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Stuffing

        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianName = "")
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.ListOfFinalDestination(CustodianName);
            List<WLJ_FinalDestination> lstFinalDestination = new List<WLJ_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<WLJ_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_ContainerStuffing ObjCS = new Wlj_ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = ObjER.DBResponse.Data;
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
            ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }
            return PartialView(ObjCS);
        }

        [HttpGet]
        public JsonResult GetContainerNoOfStuffingReq(int StuffingReqId)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid,0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }
        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (Wlj_ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {

            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Wlj_ContainerStuffing)ObjER.DBResponse.Data;
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
                ObjER.ListOfPOD();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
                }
                else
                {
                    ViewBag.ListOfPOD = null;
                }
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<Wlj_ContainerStuffingDtl> LstStuffing = new List<Wlj_ContainerStuffingDtl>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
          

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(Wlj_ContainerStuffing ObjStuffing)
        {
           string SCMTRXML = "";

            ModelState.Remove("GENSPPartyCode");
            ModelState.Remove("GREPartyCode");
            ModelState.Remove("INSPartyCode");
            ModelState.Remove("HandalingPartyCode");
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<Wlj_ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Wlj_ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                //if (ObjStuffing.SCMTRXML != null && ObjStuffing.SCMTRXML != "")
                //{
                //    List<WLJContainerStuffingSCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WLJContainerStuffingSCMTR>>(ObjStuffing.SCMTRXML);
                //    SCMTRXML = Utility.CreateXML(LstSCMTR);
                //}               

                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML);//, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                //ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML);                                                                 //INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML);
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
                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Wlj_ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuff(Wlj_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "", Area = "", PortName = "", PortDestination = "", Remarks = "", EquipmentSealType="";

            String Consignee = ""; int SerialNo = 1;
            if (ObjStuffing.LstStuffingDtl.Count() > 0)
            {
                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingBillNo = x.ShippingBillNo }).Distinct().ToList().ForEach(item =>
                {
                    ShippingBillNo = (ShippingBillNo == "" ? ((item.ShippingBillNo) + " ") : (item.ShippingBillNo + "<br/>" + item.ShippingBillNo + " "));
                    /*   if (ShippingBillNo == "")
                           ShippingBillNo = item.ShippingBillNo + " ";
                       else
                           ShippingBillNo += "," + item.ShippingBillNo; */
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingDate = x.ShippingDate }).Distinct().ToList().ForEach(item =>
                {

                    ShippingDate = (ShippingDate == "" ? (item.ShippingDate) : (item.ShippingDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { EntryNo = x.EntryNo }).Distinct().ToList().ForEach(item =>
                {

                    EntryNo = (EntryNo == "" ? (item.EntryNo) : (item.EntryNo));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
                {

                    InDate = (InDate == "" ? (item.InDate) : (item.InDate));
                    /*  if (ShippingDate == "")
                          ShippingDate = item.ShippingDate;
                      else
                          ShippingDate += "," + item.ShippingDate;*/
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { Exporter = x.Exporter }).Distinct().ToList().ForEach(item =>
                {
                    if (Exporter == "")
                        Exporter = item.Exporter;
                    else
                        Exporter += "," + item.Exporter;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { PortName = x.PortName }).Distinct().ToList().ForEach(item =>
                {
                    if (PortName == "")
                        PortName = item.PortName;
                    else
                        PortName += "," + item.PortName;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { PortDestination = x.PortDestination }).Distinct().ToList().ForEach(item =>
                {
                    if (PortDestination == "")
                        PortDestination = item.PortDestination;
                    else
                        PortDestination += "," + item.PortDestination;
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { Consignee = x.Consignee }).Distinct().ToList().ForEach(item =>
                {
                    if (Consignee == "")
                        Consignee = item.Consignee;
                    else
                        Consignee += "," + item.Consignee;
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
                {

                    if (ShippingLine == "")
                        ShippingLine = item.ShippingLine;
                    else
                        ShippingLine += "," + item.ShippingLine;
                });

                ObjStuffing.LstStuffingDtl.Select(x => new { Remarks = x.Remarks }).Distinct().ToList().ForEach(item =>
                {

                    if (Remarks == "")
                        Remarks = item.Remarks;
                    else
                        Remarks += "," + item.Remarks;
                });
                ObjStuffing.LstStuffingDtl.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
                {
                    if (CHA == "")
                        CHA = item.CHA;
                    else
                        CHA += "," + item.CHA;
                });

                //StuffWeight = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffWeight)).ToString() : "";
                //Fob = (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.Fob)).ToString() : "";
                StuffQuantity = (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity) > 0) ? (ObjStuffing.LstStuffingDtl.Sum(x => x.StuffQuantity)).ToString() : "";
                ObjStuffing.LstStuffingDtl.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                    CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                    CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : CustomSeal == item.CustomSeal ? CustomSeal : (CustomSeal + "<br/>" + item.CustomSeal));
                    Commodity = (Commodity == "" ? (item.CommodityName) : Commodity == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));
                    EquipmentSealType = (EquipmentSealType == "" ? (item.EquipmentSealType) : EquipmentSealType == item.EquipmentSealType ? EquipmentSealType : (EquipmentSealType + "<br/>" + item.EquipmentSealType));
                    //SerialNo++;
                });
                //SLNo.Remove(SLNo.Length - 1);


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
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>CFS Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size :</b> <u>" + ObjStuffing.Size + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Forwarder Name :</b> <u>" + ObjStuffing.ForwarderName + "</u></td> </tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No :</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Equipment Seal Type :</b> <u>" + EquipmentSealType + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Cont Type :</b> <u>" + ObjStuffing.CargoType + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line :</b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Vessel :</b> <u>" + ObjStuffing.Vessel + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Voyage :</b> <u>" + ObjStuffing.Voyage + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Via :</b> <u>" + ObjStuffing.Via + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Final Destination Location :</b> <u>" + ObjStuffing.CustodianCode + "</u></td> <td colspan='3' width='25%'></td>  </tr></tbody></table> </td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='17%' style='margin:0 0 10px;'><b>Vessel :</b> <u>" + ObjStuffing.Vessel + "</u></td>  <td colspan='3' width='15%' style='margin:0 0 10px;'><b>Voyage :</b> <u>" + ObjStuffing.Voyage + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Via :</b> <u>" + ObjStuffing.Via + "</u></td></tr></tbody></table> </td></tr>";

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
                ObjStuffing.LstStuffingDtl.ToList().ForEach(item =>
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
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffingDtl.AsEnumerable().Sum(item => item.StuffQuantity) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffingDtl.AsEnumerable().Sum(item => item.StuffWeight) + "</th>";
                Html += "<th style='padding:3px;text-align:left;width:10%;'>" + ObjStuffing.LstStuffingDtl.AsEnumerable().Sum(item => item.Fob) + "</th>";
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

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='text-align:left;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td style='text-align:left;'>Shed Asst. <br/> CFS Waluj</td><td style='text-align:left;'>Shed I/C <br/> CFS Waluj</td><td style='text-align:center;'>Customs <br/> CFS Waluj</td></tr></tbody></table></td></tr>";
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

        [HttpGet]
        public ActionResult SearchContainerStuffing(string ContNo)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.SearchContainerStuffing(ContNo);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }




        #endregion

        #region Container Stuffing Amendment

        [HttpGet]
        public ActionResult AmendmentContainerStuffing()
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();

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
            ObjER.ListOfPOD();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }
            return PartialView("AmendmentContainerStuffing");
        }

        [HttpGet]
        public JsonResult GetStuffingNoForAmendment()
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.ListOfStuffingNoForAmendment();
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetStuffingDetailsForAmendment(int ContainerStuffingId)
        {
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingDetails(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Wlj_ContainerStuffing)ObjER.DBResponse.Data;
                }

            }

            return Json(ObjStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditAmendmentContainerStuffing(Wlj_ContainerStuffing ObjStuffing)
        {

            ModelState.Remove("GENSPPartyCode");
            ModelState.Remove("GREPartyCode");
            ModelState.Remove("INSPartyCode");
            ModelState.Remove("HandalingPartyCode");
            ModelState.Remove("SQM");

            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";

                if (ObjStuffing.StuffingXML != null)
                {
                    List<Wlj_ContainerStuffingDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Wlj_ContainerStuffingDtl>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }

                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(0, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView(LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadAmendmentContainerStuffingList(int Page, string SearchValue = "")
        {
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(Page, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export Payment Sheet
        [HttpGet]
        public ActionResult CreateExportPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            WLJ_ExportRepository objExp = new WLJ_ExportRepository();
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
            WLJ_ExportRepository objImport = new WLJ_ExportRepository();
            objImport.GetContDetForPaymentSheet(AppraisementId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,string SEZ, List<PaymentSheetContainer> lstPaySheetContainer,
            int PartyId, int PayeeId, int InvoiceId = 0, int OTHour=0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            WLJ_ExportRepository objPpgRepo = new WLJ_ExportRepository();
            objPpgRepo.GetExportPaymentSheet(InvoiceDate, AppraisementId, TaxType,SEZ, XMLText, InvoiceId, PartyId, PayeeId , OTHour);
            var Output = (WLJ_ExpPaymentSheet)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXP";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WLJ_ExpContainer();
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

                var invoiceData = JsonConvert.DeserializeObject<WLJ_ExpPaymentSheet>(objForm["PaymentSheetModelJson"]);
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

                WLJ_ExportRepository objChargeMaster = new WLJ_ExportRepository();
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
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<WLJListOfExpInvoice> obj = new List<WLJListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WLJListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }
        #endregion

        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            //objER.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.RightsList = JsonConvert.SerializeObject(objER.DBResponse.Data);
            //}
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
            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpGet]
        public ActionResult ViewLoadContainerRequest(int LoadContReqId)
        {
            WLJ_ExportRepository ObjRR = new WLJ_ExportRepository();
            Wlj_LoadContReq ObjContReq = new Wlj_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Wlj_LoadContReq)ObjRR.DBResponse.Data;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            WLJ_ExportRepository ObjRR = new WLJ_ExportRepository();
            Wlj_LoadContReq ObjContReq = new Wlj_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (Wlj_LoadContReq)ObjRR.DBResponse.Data;
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
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult ListLoadContainerRequest()
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            List<ListLoadContReq> lstCont = new List<ListLoadContReq>();
            objER.ListOfLoadCont();
            if (objER.DBResponse.Data != null)
                lstCont = (List<ListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(Wlj_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                WLJ_ExportRepository objER = new WLJ_ExportRepository();
                string XML = "";
                if (objReq.StringifyData != null)
                {
                    XML = Utility.CreateXML(JsonConvert.DeserializeObject<List<Wlj_LoadContReqDtl>>(objReq.StringifyData));
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
                WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
        public ActionResult LoadContainerStuffingList(int Page)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<Wlj_ContainerStuffing> LstStuffing = new List<Wlj_ContainerStuffing>();
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<Wlj_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Loaded Container Invoice

        [HttpGet]
        public ActionResult CreateLoadedContainerPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
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
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetContainerForLoadedContainerPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType,string SEZ, int StuffingReqId, List<PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
            int PartyId, int InvoiceId = 0, int OTHour = 0,int OnWheelHours=0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }
            WLJ_ExportRepository objChrgRepo = new WLJ_ExportRepository();
            objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, InvoiceDate, InvoiceType,SEZ, ContainerXML, PayeeId, PartyId, InvoiceId , OTHour, OnWheelHours);

            var Output = (WLJ_ExpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPLod";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WLJ_ExpContainer();
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
                obj.OnWheelHours = OnWheelHours;
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
                var invoiceData = JsonConvert.DeserializeObject<WLJ_ExpPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
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
                WLJ_ExportRepository objChargeMaster = new WLJ_ExportRepository();
                objChargeMaster.AddEditInvoiceContLoaded(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

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
            //User RightsList---------------------------------------------------------------------------------------
            //AccessRightsRepository ACCR = new AccessRightsRepository();
            //ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            //ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //------------------------------------------------------------------------------------------------------

            return PartialView();
        }

        [HttpGet]
        public ActionResult ListOfBTTCargo()
        {
            List<Wlj_BTTCargoEntry> lstBTTCargoEntry = new List<Wlj_BTTCargoEntry>();
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetBTTCargoEntry();
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<Wlj_BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult AddBTTCargo()
        {



            Wlj_BTTCargoEntry objBTTCargoEntry = new Wlj_BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy");

            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetCartingAppList(0);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Wlj_BTTCartingList>)objExport.DBResponse.Data;
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
            Wlj_BTTCargoEntry objBTTCargoEntry = new Wlj_BTTCargoEntry();
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Wlj_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Wlj_BTTCartingList>)objExport.DBResponse.Data;
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
            Wlj_BTTCargoEntry objBTTCargoEntry = new Wlj_BTTCargoEntry();
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();

            objExport.GetBTTCargoEntryById(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry = (Wlj_BTTCargoEntry)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstBTTCargoEntryDtl != null)
                objBTTCargoEntry.BTTCargoEntryDtlJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstBTTCargoEntryDtl);

            objExport.GetCartingAppList(BTTCargoEntryId);
            if (objExport.DBResponse.Data != null)
                objBTTCargoEntry.lstCartingList = (List<Wlj_BTTCartingList>)objExport.DBResponse.Data;
            if (objBTTCargoEntry.lstCartingList != null)
                objBTTCargoEntry.BTTCartingListJS = JsonConvert.SerializeObject(objBTTCargoEntry.lstCartingList);

            return PartialView(objBTTCargoEntry);
        }

        [HttpGet]
        public JsonResult GetCartingDetailList(int CartingId)
        {
            try
            {
                WLJ_ExportRepository objExport = new WLJ_ExportRepository();
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
                WLJ_ExportRepository objExport = new WLJ_ExportRepository();
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
        public JsonResult AddEditBTTCargo(Wlj_BTTCargoEntry objBTT)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    objBTT.lstBTTCargoEntryDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<Wlj_BTTCargoEntryDtl>>(objBTT.BTTCargoEntryDtlJS);
                    string XML = Utility.CreateXML(objBTT.lstBTTCargoEntryDtl);
                    WLJ_ExportRepository objExport = new WLJ_ExportRepository();
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

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            WLJ_ExportRepository objExp = new WLJ_ExportRepository();
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
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            WLJ_ExportRepository objExport = new WLJ_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                WLJ_ExportRepository objCR = new WLJ_ExportRepository();
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
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
            int k = 0;
            int j = 1;
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            Wlj_ContainerStuffing ObjStuffing = new Wlj_ContainerStuffing();
            ObjER.GetCIMSFDetails(ContainerStuffingId, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                string Filenm = Convert.ToString(ds.Tables[5].Rows[0]["FileName"]);
                string JsonFile = StuffingSBJsonFormat.ContStuffingDetJson(ds);




                string strFolderName = System.Configuration.ConfigurationManager.AppSettings["ReportfileCIMSF"];
                string FTPFolderName = System.Configuration.ConfigurationManager.AppSettings["SCMTRInboundFolder"];

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


                    if (status.ToString() == "Success")
                    {
                        using (FileStream fs = System.IO.File.OpenRead(FinalOutPutPath))
                        {
                            log.Error("File Open:" + OUTJsonFile);
                            log.Info("FTP File read process has began");
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            fs.Close();
                            string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
                            log.Error("SCMTRPath:" + SCMTRPath);
                            // log.Error("SCMTR File Name:" + OUTJsonFile+ Filenm);
                            output = FtpFileManager.UploadFileToFtp(SCMTRPath, Filenm, buffer, "5000", FinalOutPutPath);
                            log.Info("FTP File read process has ended");
                        }
                    }
                    else
                    {
                        output = "Error";
                    }

                    //For Block If Develpoment Done Then Unlock

                }



                log.Error("output: " + output);


                #endregion
                if (output == "Success")
                {
                    WLJ_ExportRepository objExport = new WLJ_ExportRepository();
                    objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                    return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                }
                log.Info("FTP File upload has been end");
                return Json(new { Status = 1, Message = "CIM SF File Send Fail." });
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
            }

        }
        #endregion


    }
}