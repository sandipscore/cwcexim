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
using CwcExim.Areas.Import.Models;
using System.Text;
using SCMTRLibrary;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CwcExim.Areas.Export.Controllers
{
    //For V2
    public class Loni_CWCExportV2Controller : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Export/Ppg_CWCExportV2
        #region CCINEntry
        public ActionResult CCINEntry(int Id = 0, int PartyId = 0)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            //   objER.GetCCINShippingLine();
            //   if (objER.DBResponse.Data != null)
            //   {
            //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
            //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            //  }
            //objER.ListOfExporter();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfExporter = objER.DBResponse.Data;
            //}
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();


            ObjER.ListOfPackUQCForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPackUQC = Jobject["lstPackUQC"];
                ViewBag.PackUQCState = Jobject["State"];
            }
            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            Loni_ExportRepositoryV2 ObjCR = new Loni_ExportRepositoryV2();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            Loni_ExportRepositoryV2 ObjRR = new Loni_ExportRepositoryV2();
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

            CCINEntryV2 objCCINEntry = new CCINEntryV2();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
                objCCINEntry.PortOfLoadingName = "ICD PPG";
            }
            else
            {
                Loni_ExportRepositoryV2 rep = new Loni_ExportRepositoryV2();
                rep.GetCCINEntryForEdit(Id, PartyId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntryV2)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
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
        public JsonResult GetShippingBill()
        {
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();
            objRepo.GetSBList();
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.ListOfSBNo = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            }


            var jsonResult = Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //    return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetInvParty()
        {
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetCCINCharges(int CCINEntryId, int PartyId, decimal Weight, decimal FOB, string CargoType)
        {
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetCCINCharges(CCINEntryId, PartyId, Weight, FOB, CargoType);
            objCCINEntry = (CCINEntryV2)objExport.DBResponse.Data;
            ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntry(CCINEntryV2 objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            ModelState.Remove("CCINNo");
            ModelState.Remove("GodownName");
            ModelState.Remove("GodownName");
            ModelState.Remove("IsApproved");
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
                objCCINEntry.IP = Request.UserHostAddress;
                //IList<PostPaymentChargeV2> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentChargeV2>>(objCCINEntry.PaymentSheetModelJson);
                string XML = Utility.CreateXML("");
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
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            List<CCINEntryV2> lstCCINEntry = new List<CCINEntryV2>();
            objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<CCINEntryV2>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryList(int Page)
        {
            Loni_ExportRepositoryV2 ObjCR = new Loni_ExportRepositoryV2();
            List<CCINEntryV2> LstJO = new List<CCINEntryV2>();
            ObjCR.GetAllCCINEntryForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<CCINEntryV2>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntry(int CCINEntryId)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult PrintCCINEntry(int CCINEntryId)
        {
            Loni_ExportRepositoryV2 rep = new Loni_ExportRepositoryV2();
            rep.PrintCCINEntry(CCINEntryId);
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            string Path = "";
            if (rep.DBResponse.Data != null)
            {
                objCCINEntry = ((CCINEntryV2)rep.DBResponse.Data);
                Path = CCINEntryPDFGeneration(objCCINEntry);
            }
            return Json(new { Status = 1, Data = Path });
        }
        [NonAction]
        public string CCINEntryPDFGeneration(CCINEntryV2 objCCINEntry)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CCINEntry" + objCCINEntry.Id + ".pdf";
            string CargoType = objCCINEntry.CargoType == 1 ? "HAZ" : (objCCINEntry.CargoType == 2 ? "NON HAZ" : "");
            string SBType = objCCINEntry.SBType == 1 ? "Baggage" : (objCCINEntry.SBType == 2 ? "Duty Free Goods" : (objCCINEntry.SBType == 3 ? "Cargo in Drawback" : ""));
            string Html = "";
            Html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='150%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'>CCIN ENTRY SLIP</label></td></tr> <tr><td><span><br/></span></td></tr> </tbody></table>";

            Html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CCIN App No. : <span style='font-weight:normal;'>" + objCCINEntry.CCINNo + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CCIN App Date : <span style='font-weight:normal;'>" + objCCINEntry.CCINDate + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>SB No. : <span style='font-weight:normal;'>" + objCCINEntry.SBNo + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SB Date : <span style='font-weight:normal;'>" + objCCINEntry.SBDate + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SB Type : <span style='font-weight:normal;'>" + SBType + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Exporter : <span style='font-weight:normal;'>" + objCCINEntry.ExporterName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objCCINEntry.ShippingLineName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA : <span style='font-weight:normal;'>" + objCCINEntry.CHAName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Consignee : <span style='font-weight:normal;'>" + objCCINEntry.ConsigneeName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Consignee Address : <span style='font-weight:normal;'>" + objCCINEntry.ConsigneeAdd + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Country of Destination : <span style='font-weight:normal;'>" + objCCINEntry.CountryName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Port of Destination : <span style='font-weight:normal;'>" + objCCINEntry.PortOfDestName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Commodity : <span style='font-weight:normal;'>" + objCCINEntry.CommodityName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Port of Loading : <span style='font-weight:normal;'>" + objCCINEntry.PortOfLoadingName + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Port of Discharge : <span style='font-weight:normal;'>" + objCCINEntry.PortOfDischarge + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No of Package : <span style='font-weight:normal;'>" + objCCINEntry.Package + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Weight (kg) : <span style='font-weight:normal;'>" + objCCINEntry.Weight.ToString("0.###") + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word;border-bottom:1px solid #000;' '>FOB : <span style='font-weight:normal;'>" + objCCINEntry.FOB.ToString("0.###") + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; '>Package Type : <span style='font-weight:normal;'>" + objCCINEntry.PackageType.ToString() + "</span></th></tr>";

            Html += "</tbody></table>";

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/CCINEntry" + objCCINEntry.Id + ".pdf";
        }
        #endregion

        #region CCINEntry Approval
        public ActionResult CCINEntryApproval(int Id = 0)
        {
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            /*objER.GetShippingLine();
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
            }*/
            objER.GetAllCommodity();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfCommodity = objER.DBResponse.Data;
            }

            Loni_ExportRepositoryV2 ObjCR = new Loni_ExportRepositoryV2();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            Loni_ExportRepositoryV2 ObjRR = new Loni_ExportRepositoryV2();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfPort = ObjRR.DBResponse.Data;
                objCCINEntry.PortOfLoadingName = "ICD PPG";
                objCCINEntry.PortOfLoadingId = ((List<Port>)ObjRR.DBResponse.Data).Where(x => x.PortName == "ICD PPG").FirstOrDefault().PortId;
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

            Loni_ExportRepositoryV2 objRepository = new Loni_ExportRepositoryV2();
            objRepository.GetAllCCINEntry("A");
            if (objRepository.DBResponse.Data != null)
            {
                ViewBag.ListOfCCINNo = (List<CCINEntryV2>)objRepository.DBResponse.Data;
            }

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                Loni_ExportRepositoryV2 rep = new Loni_ExportRepositoryV2();
                rep.GetCCINEntryById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntryV2)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetCCINEntryApprovalDetails(int CCINEntryId)
        {
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            if (CCINEntryId > 0)
            {
                Loni_ExportRepositoryV2 rep = new Loni_ExportRepositoryV2();
                rep.GetCCINEntryById(CCINEntryId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntryV2)rep.DBResponse.Data;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntryApproval(CCINEntryV2 vm)
        {


            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            if (vm.IsApproved == true)
            {
                vm.Approved = 1;
            }
            else
            {
                vm.Approved = 0;
            }
            objER.AddEditCCINEntryApproval(vm);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntryApproval()
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            List<CCINEntryV2> lstCCINEntry = new List<CCINEntryV2>();
            objER.GetAllCCINEntryApprovalForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<CCINEntryV2>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryApprovalList(int Page)
        {
            Loni_ExportRepositoryV2 ObjCR = new Loni_ExportRepositoryV2();
            List<CCINEntryV2> LstJO = new List<CCINEntryV2>();
            ObjCR.GetAllCCINEntryApprovalForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<CCINEntryV2>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfGodownName()
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            List<Areas.Export.Models.GodownList> lstGownName = new List<Areas.Export.Models.GodownList>();
            objER.GetMstGodownList();
            if (objER.DBResponse.Data != null)
                lstGownName = (List<Areas.Export.Models.GodownList>)objER.DBResponse.Data;
            return Json(lstGownName, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult PrintCCINEntryApproval(int CCINEntryId)
        {
            Loni_ExportRepositoryV2 rep = new Loni_ExportRepositoryV2();
            rep.PrintCCINEntryApproval(CCINEntryId);
            CCINEntryV2 objCCINEntry = new CCINEntryV2();
            string Path = "";
            if (rep.DBResponse.Data != null)
            {
                objCCINEntry = ((CCINEntryV2)rep.DBResponse.Data);
                Path = CCINEntryApprovalPDFGeneration(objCCINEntry);
            }
            return Json(new { Status = 1, Data = Path });
        }
        [NonAction]
        public string CCINEntryApprovalPDFGeneration(CCINEntryV2 objCCINEntry)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CCINEntryApproval" + objCCINEntry.Id + ".pdf";
            string CargoType = objCCINEntry.CargoType == 1 ? "HAZ" : (objCCINEntry.CargoType == 2 ? "NON HAZ" : "");
            string SBType = objCCINEntry.SBType == 1 ? "Baggage" : (objCCINEntry.SBType == 2 ? "Duty Free Goods" : (objCCINEntry.SBType == 3 ? "Cargo in Drawback" : ""));
            string Html = "";
            Html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody> <tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='150%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><label style='font-size: 14px; font-weight:bold;'>CCIN ENTRY APPROVAL SLIP</label></td></tr> <tr><td><span><br/></span></td></tr> </tbody></table>";

            Html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CCIN App No. : <span style='font-weight:normal;'>" + objCCINEntry.CCINNo.Split('-')[1] + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CCIN App Date : <span style='font-weight:normal;'>" + objCCINEntry.CCINDate.Split('-')[1] + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CCIN No. : <span style='font-weight:normal;'>" + objCCINEntry.CCINNo.Split('-')[0] + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CCIN Date : <span style='font-weight:normal;'>" + objCCINEntry.CCINDate.Split('-')[0] + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>SB No. : <span style='font-weight:normal;'>" + objCCINEntry.SBNo + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SB Date : <span style='font-weight:normal;'>" + objCCINEntry.SBDate + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SB Type : <span style='font-weight:normal;'>" + SBType + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Exporter : <span style='font-weight:normal;'>" + objCCINEntry.ExporterName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objCCINEntry.ShippingLineName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA : <span style='font-weight:normal;'>" + objCCINEntry.CHAName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Consignee : <span style='font-weight:normal;'>" + objCCINEntry.ConsigneeName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Consignee Address : <span style='font-weight:normal;'>" + objCCINEntry.ConsigneeAdd + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Country of Destination : <span style='font-weight:normal;'>" + objCCINEntry.CountryName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Port of Destination : <span style='font-weight:normal;'>" + objCCINEntry.PortOfDestName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Commodity : <span style='font-weight:normal;'>" + objCCINEntry.CommodityName + "</span></th></tr>";
            Html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Port of Loading : <span style='font-weight:normal;'>" + objCCINEntry.PortOfLoadingName + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Port of Discharge : <span style='font-weight:normal;'>" + objCCINEntry.PortOfDischarge + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No of Package : <span style='font-weight:normal;'>" + objCCINEntry.Package + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Weight (kg) : <span style='font-weight:normal;'>" + objCCINEntry.Weight.ToString("0.###") + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Godown Name : <span style='font-weight:normal;'>" + objCCINEntry.GodownName + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; '>FOB : <span style='font-weight:normal;'>" + objCCINEntry.FOB.ToString("0.###") + "</span></th></tr>";

            Html += "<tr><th cellpadding='10' align='left' colspan='6' width='100%' style='overflow:hidden; word-wrap:break-word;  border-right:1px solid #000;'>Approved By : <span style='font-weight:normal;'>" + objCCINEntry.ApprovedBy + "</span></th>";
            Html += "<th cellpadding='10' align='left' colspan='6' width='100%' style='overflow:hidden; word-wrap:break-word;'>Approved On : <span style='font-weight:normal;'>" + objCCINEntry.ApprovedOn + "</span></th></tr>";

            Html += "</tbody></table>";

            Html += "<table style='width:100%; font-size:10pt; font-family:Verdana,Arial,San-serif; margin: 0; padding: 5px;'><tr><td width='60%'><br/><br/>Signature of Exporter / CHA</td><td width='40%' align='center'><br/><br/><br/>For Central Warehousing Corporation<br/>Authorized Signatories</td></tr></table>";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/CCINEntryApproval" + objCCINEntry.Id + ".pdf";
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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            ObjIR.GetAllInternalMovement();
            List<PPG_ContainerMovementV2> LstMovement = new List<PPG_ContainerMovementV2>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_ContainerMovementV2>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public JsonResult GetContainerNoForMovement()
        {
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();



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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();

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
        public JsonResult GetPartyForMovement(int Page, string PartyCode)
        {
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            ObjIR.GetPaymentParty(Page, PartyCode);
            if (ObjIR.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;





            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            PPG_ContainerMovementV2 ObjInternalMovement = new PPG_ContainerMovementV2();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_ContainerMovementV2)ObjIR.DBResponse.Data;
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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            PPG_ContainerMovementV2 ObjInternalMovement = new PPG_ContainerMovementV2();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_ContainerMovementV2)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetConDetails(int ContainerStuffingDtlId, String ContainerNo)
        {
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            ObjIR.GetConDetForMovement(ContainerStuffingDtlId, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                PPG_ContainerMovementV2 ObjInternalMovement = new PPG_ContainerMovementV2();
                ObjInternalMovement = (PPG_ContainerMovementV2)ObjIR.DBResponse.Data;
                ViewBag.ShippingBill = new SelectList((List<PPG_ShippingBillV2>)ObjInternalMovement.ShipBill, "shippingBillNo", "shippingBillNo");
            }
            else
            {
                ViewBag.ShippingBill = null;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int ContainerStuffingDtlId, int ContainerStuffingId, string ContainerNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int portvalue, decimal tareweight, string cargotype, int PayeeId, string SEZ, int InvoiceId = 0)
        {

            Loni_ExportRepositoryV2 objChrgRepo = new Loni_ExportRepositoryV2();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(ContainerStuffingDtlId, ContainerStuffingId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, ctype, portvalue, tareweight, cargotype, PayeeId, SEZ, InvoiceId);



            var Output = (PPG_MovementInvoiceV2)objChrgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new PpgPostPaymentContainerV2
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
                PPGTentativeInvoiceV2.InvoiceObjW = Output;

            if (ctype == "GR")
                PPGTentativeInvoiceV2.InvoiceObjGR = Output;

            if (ctype == "FMC")
                PPGTentativeInvoiceV2.InvoiceObjFMC = Output;

            return Json(Output, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult MovementInvoicePrint(string InvoiceNo)
        {
            Loni_ExportRepositoryV2 objGPR = new Loni_ExportRepositoryV2();
            if (InvoiceNo == "")
            {
                return Json(new { Status = -1, Message = "Error" });

            }
            else
            {
                objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "EXPMovement");
                PPG_Movement_InvoiceV2 objGP = new PPG_Movement_InvoiceV2();
                string FilePath = "";
                if (objGPR.DBResponse.Data != null)
                {
                    objGP = (PPG_Movement_InvoiceV2)objGPR.DBResponse.Data;
                    FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                    return Json(new { Status = 1, Message = FilePath });
                }

            }





            return Json(new { Status = -1, Message = "Error" });



        }
        private string GeneratingPDFInvoiceMovement(PPG_Movement_InvoiceV2 objGP, int InvoiceId)
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
                string SEZ = Convert.ToString(objForm["SEZ1"]);
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

                var invoiceData = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJson"].ToString());
                //var invoiceDataa = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJsonn"].ToString());

                //var invoiceDataaa = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJsonnn"].ToString());
                var Shipbillwiseamount = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJson"].ToString());
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
                string SbXML = "";
                string OperationCfsCodeWiseAmtXMLLL = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }


                //foreach (var item in invoiceDataaa.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                //}
                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                invoiceData.TotalCGST = invoiceData.lstPostPaymentChrg.Sum(x => x.CGSTAmt);
                invoiceData.TotalSGST = invoiceData.lstPostPaymentChrg.Sum(x => x.SGSTAmt);
                invoiceData.TotalIGST = invoiceData.lstPostPaymentChrg.Sum(x => x.IGSTAmt);
                invoiceData.TotalAmt = invoiceData.lstPostPaymentChrg.Sum(x => x.Amount);
                invoiceData.CWCTotal = invoiceData.TotalAmt;
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

                //if (invoiceDataa.lstPostPaymentCont != null)
                //{
                //    ContainerXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentCont);
                //}
                //if (invoiceDataa.lstPostPaymentChrg != null)
                //{
                //    ChargesXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrg);
                //}
                //if (invoiceDataa.lstContWiseAmount != null)
                //{
                //    ContWiseChargg = Utility.CreateXML(invoiceDataa.lstContWiseAmount);
                //}
                //if (invoiceDataa.lstOperationCFSCodeWiseAmount != null)
                //{
                //    OperationCfsCodeWiseAmtXMLL = Utility.CreateXML(invoiceDataa.lstOperationCFSCodeWiseAmount);
                //}

                //if (invoiceDataaa.lstPostPaymentCont != null)
                //{
                //    ContainerXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentCont);
                //}
                //if (invoiceDataaa.lstPostPaymentChrg != null)
                //{
                //    ChargesXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrg);
                //}
                //if (invoiceDataaa.lstContWiseAmount != null)
                //{
                //    ContWiseCharggg = Utility.CreateXML(invoiceDataaa.lstContWiseAmount);
                //}
                //if (invoiceDataaa.lstOperationCFSCodeWiseAmount != null)
                //{
                //    OperationCfsCodeWiseAmtXMLLL = Utility.CreateXML(invoiceDataaa.lstOperationCFSCodeWiseAmount);
                //}

                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                //if (invoiceDataa.lstPostPaymentChrgBreakup != null)
                //{
                //    ChargesBreakupXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrgBreakup);
                //}
                //if (invoiceDataaa.lstPostPaymentChrgBreakup != null)
                //{
                //    ChargesBreakupXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrgBreakup);
                //}
                if (Shipbillwiseamount.lstShipbillwiseAmountV2 != null)
                {
                    SbXML = Utility.CreateXML(Shipbillwiseamount.lstShipbillwiseAmountV2);
                }
                decimal TareWeight = 0, CargoWeight = 0;
                if (objForm["TareWeight"] != null)
                    TareWeight = Convert.ToDecimal(objForm["TareWeight"]);
                if (objForm["CargoWeight"] != null)
                    CargoWeight = Convert.ToDecimal(objForm["CargoWeight"]);
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.AddEditInvoiceMovement(invoiceData, null, null, ContainerXML, ContainerXMLL, ContainerXMLLL, ChargesXML, ChargesXMLL, ChargesXMLLL, ContWiseCharg, ContWiseChargg, ContWiseCharggg, OperationCfsCodeWiseAmtXML, OperationCfsCodeWiseAmtXMLL, OperationCfsCodeWiseAmtXMLLL, ChargesBreakupXML, ChargesBreakupXMLL, ChargesBreakupXMLLL, SbXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPMovement", TareWeight, CargoWeight, SEZ, "");
                PPG_InvoiceListV2 inv = new PPG_InvoiceListV2();
                //inv = (PPG_InvoiceListV2)objChargeMaster.DBResponse.Data;

                //invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //if (inv != null)
                //{
                //    invoiceData.InvoiceNo = inv.invoiceno;
                //    //invoiceData.invoicenoo = inv.invoicenoo;
                //    //invoiceData.invoicenooo = inv.invoicenooo;
                //    invoiceData.MovementNo = inv.MovementNo;
                //}
                //invoiceDataa.ROAddress = (invoiceDataa.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceDataa.CompanyAddress = (invoiceDataa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    // invoiceDataa.InvoiceNo = inv.invoicenoo;
                }
                //invoiceDataaa.ROAddress = (invoiceDataaa.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceDataaa.CompanyAddress = (invoiceDataaa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    // invoiceDataaa.InvoiceNo = inv.invoicenooo;
                }

                //objChargeMaster.DBResponse.Data = invoiceData;
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
        public JsonResult AddEditInternalMovement(PPG_ContainerMovementV2 ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();

            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }


        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            Loni_ExportRepositoryV2 objIR = new Loni_ExportRepositoryV2();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfMovementInv(int Page = 0)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.ListOfMovementInv(Page);
            List<Ppg_ContMovementList> LstMovement = new List<Ppg_ContMovementList>();
            if (objER.DBResponse.Data != null)
                LstMovement = ((List<Ppg_ContMovementList>)objER.DBResponse.Data);
            if (Page == 0) return PartialView(LstMovement);
            else return Json(LstMovement, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Concor ledger Sheet

        public ActionResult ConcorLedgerSheet()
        {


            return PartialView("ConcorLedgerSheet");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitConcorLedgerSheet(PpgConcorLedgerSheetViewModelV2 vm)
        {
            Loni_ExportRepositoryV2 obj = new Loni_ExportRepositoryV2();


            try
            {
                obj.AddEditConcorledgersheet(vm);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                obj.DBResponse.Message = ex.Message;
                obj.DBResponse.Status = 0;
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult GetContainerList(string TrainNo, string OperationType)
        {
            Loni_ExportRepositoryV2 obj = new Loni_ExportRepositoryV2();
            obj.GetContainerList(TrainNo, OperationType);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContainerDetails(string TrainNo, string OperationType, string ContainerNo)
        {
            Loni_ExportRepositoryV2 obj = new Loni_ExportRepositoryV2();
            obj.GetContainerDetails(ContainerNo, TrainNo, OperationType);

            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllConcorLedgerSheet(int id)
        {
            Loni_ExportRepositoryV2 obj = new Loni_ExportRepositoryV2();
            obj.GetAllConcorLedgerSheet(id);
            List<PpgConcorLedgerSheetViewModelV2> lstLedgerSheet = new List<PpgConcorLedgerSheetViewModelV2>();
            if (obj.DBResponse.Status == 1)
                lstLedgerSheet = ((List<PpgConcorLedgerSheetViewModelV2>)obj.DBResponse.Data);

            return PartialView("GetAllConcorLedgerSheet", lstLedgerSheet);
        }


        public ActionResult Edit(int id)
        {
            PpgConcorLedgerSheetViewModelV2 obj = new PpgConcorLedgerSheetViewModelV2();
            if (id > 0)
            {
                Loni_ExportRepositoryV2 exportrepositoryobj = new Loni_ExportRepositoryV2();
                exportrepositoryobj.GetAllConcorLedgerSheetEdit(id);
                obj = ((PpgConcorLedgerSheetViewModelV2)exportrepositoryobj.DBResponse.Data);
            }
            return PartialView("Edit", obj);
        }





        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult deleteConcorLedgerSheet(int id)
        {
            Loni_ExportRepositoryV2 obj = new Loni_ExportRepositoryV2();


            try
            {
                obj.DeleteConcorledgersheet(id);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                obj.DBResponse.Message = ex.Message;
                obj.DBResponse.Status = 0;
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }

        }


        #endregion

        #region Container Stuffing

        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianName = "")
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.ListOfFinalDestination(CustodianName);
            List<PPG_FinalDestination> lstFinalDestination = new List<PPG_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<PPG_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPortOfDestination()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.ListOfPortOfDestinationStuffingSearch("");
            List<Port> lstPort = new List<Port>();
            if (ObjER.DBResponse.Data != null)
            {
                lstPort = (List<Port>)ObjER.DBResponse.Data;
            }

            return Json(lstPort, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStuffingReqForAmendment(string sra)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.ListOfStuffingReqForAmendment(sra);
            List<ContainerStuffingV2> Lstsr = new List<ContainerStuffingV2>();
            if (ObjER.DBResponse.Data != null)
            {
                Lstsr = (List<ContainerStuffingV2>)ObjER.DBResponse.Data;
            }

            return Json(Lstsr, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPortOfDestinationSearch(string pod)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.ListOfPortOfDestinationStuffingSearch(pod);
            List<Port> lstPort = new List<Port>();
            if (ObjER.DBResponse.Data != null)
            {
                lstPort = (List<Port>)ObjER.DBResponse.Data;
            }

            return Json(lstPort, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            return PartialView(ObjCS);
        }

        [HttpGet]
        public JsonResult LoadPayeeList(int Page, string Alias)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.LoadPayeeList(Page, Alias);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadPartyList(int Page, string Alias)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.LoadPartyList(Page, Alias);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRequestNo()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();

            ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.LstRequestNo = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data).ToString();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();


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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        /*    [HttpGet]
            public ActionResult GetContainerStuffingList()
            {
                string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjER.GetAllContainerStuffing(((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    LstStuffing = (List<PPG_ContainerStuffingV2>)ObjER.DBResponse.Data;
                }
                return PartialView("ContainerStuffingList", LstStuffing);
            }*/
        [HttpGet]
        public ActionResult GetContainerStuffingList()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetAllContainerStuffingPage(((Login)Session["LoginUser"]).Uid, 0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_ContainerStuffingV2>)ObjER.DBResponse.Data;
            }
            return PartialView("ContainerStuffingList", LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadContainerStuffingList(int Page)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<PPG_ContainerStuffingV2> LstStuffing = new List<PPG_ContainerStuffingV2>();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetAllContainerStuffingPage(((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_ContainerStuffingV2>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (PPG_ContainerStuffingV2)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_ContainerStuffingV2)ObjER.DBResponse.Data;
                }
                /*ObjER.ListOfCHA();
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
                }*/
                /*ObjER.GetShippingLine();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ShippingLineList = new SelectList((List<Areas.Export.Models.ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }*/
                ObjStuffing.ChargesXML = JsonConvert.SerializeObject(ObjStuffing.lstChargs);
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult GetAmndContStuffing(int ContainerStuffingId)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingAmendment(ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_ContainerStuffingV2)ObjER.DBResponse.Data;
                }
                /*ObjER.ListOfCHA();
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
                }*/
                /*ObjER.GetShippingLine();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ShippingLineList = new SelectList((List<Areas.Export.Models.ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }*/
                ObjStuffing.ChargesXML = JsonConvert.SerializeObject(ObjStuffing.lstChargs);
            }
            return PartialView("GetAmendmentContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult AmendmentContainerStuffing()
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjCS = new PPG_ContainerStuffingV2();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd-MM-yyyy HH:MM");
            return PartialView(ObjCS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AmndContainerStuffingDet(PPG_ContainerStuffingV2 ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                // string SCMTRXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<ContainerStuffingDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingDtlV2>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }



                //if (ObjStuffing.SCMTRXML != null && (ObjStuffing.SCMTRXML != "") )
                //{
                //    List<ContainerStuffingV2SCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingV2SCMTR>>(ObjStuffing.SCMTRXML);
                //    SCMTRXML = Utility.CreateXML(LstSCMTR);
                //}
                string GREOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GREOperationCFSCodeWiseAmt != null)
                {
                    List<GREOperationCFSCodeWiseAmtV2> LstStuffingGRE1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREOperationCFSCodeWiseAmtV2>>(ObjStuffing.GREOperationCFSCodeWiseAmt.ToString());
                    GREOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGRE1);
                }

                string GREContainerWiseAmtXML = "";
                if (ObjStuffing.GREContainerWiseAmt != null)
                {
                    List<GREContainerWiseAmtV2> LstStuffingGRE2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREContainerWiseAmtV2>>(ObjStuffing.GREContainerWiseAmt.ToString());
                    GREContainerWiseAmtXML = Utility.CreateXML(LstStuffingGRE2);
                }

                string INSOperationCFSCodeWiseAmtLstXML = "";
                if (ObjStuffing.INSOperationCFSCodeWiseAmt != null)
                {
                    List<INSOperationCFSCodeWiseAmtV2> LstStuffingINS1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSOperationCFSCodeWiseAmtV2>>(ObjStuffing.INSOperationCFSCodeWiseAmt.ToString());
                    INSOperationCFSCodeWiseAmtLstXML = Utility.CreateXML(LstStuffingINS1);
                }

                string INSContainerWiseAmtXML = "";
                if (ObjStuffing.INSContainerWiseAmt != null)
                {
                    List<INSContainerWiseAmtV2> LstStuffingINS2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSContainerWiseAmtV2>>(ObjStuffing.INSContainerWiseAmt.ToString());
                    INSContainerWiseAmtXML = Utility.CreateXML(LstStuffingINS2);
                }

                string STOContainerWiseAmtXML = "";
                if (ObjStuffing.STOinvoicecargodtl != null)
                {
                    List<STOinvoicecargodtlV2> LstStuffingSTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOinvoicecargodtlV2>>(ObjStuffing.STOinvoicecargodtl.ToString());
                    STOContainerWiseAmtXML = Utility.CreateXML(LstStuffingSTO2);
                }
                string STOOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.STOOperationCFSCodeWiseAmt != null)
                {
                    List<STOOperationCFSCodeWiseAmtV2> LstStuffingSTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOOperationCFSCodeWiseAmtV2>>(ObjStuffing.STOOperationCFSCodeWiseAmt.ToString());
                    STOOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingSTO1);
                }

                string HNDOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.HNDOperationCFSCodeWiseAmt != null)
                {
                    List<HNDOperationCFSCodeWiseAmtV2> LstStuffingHND = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HNDOperationCFSCodeWiseAmtV2>>(ObjStuffing.HNDOperationCFSCodeWiseAmt.ToString());
                    HNDOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingHND);
                }

                string GENSPOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GENSPOperationCFSCodeWiseAmt != null)
                {
                    List<GENSPOperationCFSCodeWiseAmtV2> LstStuffingGENSP = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GENSPOperationCFSCodeWiseAmtV2>>(ObjStuffing.GENSPOperationCFSCodeWiseAmt.ToString());
                    GENSPOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGENSP);
                }

                string ShippingBillAmtXML = "";
                if (ObjStuffing.PPG_ShippingBillAmt != null)
                {
                    List<PPG_ShippingBillNoV2> LstShippingBill = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoV2>>(ObjStuffing.PPG_ShippingBillAmt.ToString());
                    ShippingBillAmtXML = Utility.CreateXML(LstShippingBill);
                }

                string ShippingBillAmtGenXML = "";
                if (ObjStuffing.PPG_ShippingBillAmtGen != null)
                {
                    List<PPG_ShippingBillNoGenV2> LstShippingBillGen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoGenV2>>(ObjStuffing.PPG_ShippingBillAmtGen.ToString());
                    ShippingBillAmtGenXML = Utility.CreateXML(LstShippingBillGen);
                }

                string ChargesXML = "";
                if (ObjStuffing.ChargesXML != null)
                {
                    List<Ppg_ContStuffChargesV2> lstCharges = JsonConvert.DeserializeObject<List<Ppg_ContStuffChargesV2>>(ObjStuffing.ChargesXML);
                    ChargesXML = Utility.CreateXML(lstCharges);
                }

                string BreakUpdateXML = "";
                if (ObjStuffing.BreakUpdateXML != null)
                {
                    List<ppgGRLPostPaymentChargebreakupdateV2> lstBreakUpdate = JsonConvert.DeserializeObject<List<ppgGRLPostPaymentChargebreakupdateV2>>(ObjStuffing.BreakUpdateXML);
                    BreakUpdateXML = Utility.CreateXML(lstBreakUpdate);
                }
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;

                //SCMTR
                //   ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, SCMTRXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                //  INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);
                //without SCMTR
                ObjER.AmndContainerStuffing(ObjStuffing, ContainerStuffingXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                 INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);

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
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<ContainerStuffingDtlV2> LstStuffing = new List<ContainerStuffingDtlV2>();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(PPG_ContainerStuffingV2 ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                // string SCMTRXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<ContainerStuffingDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingDtlV2>>(ObjStuffing.StuffingXML);

                    var LstStuffingSBSorted = from c in LstStuffing group c by c.ShippingBillNo into grp select grp.Key;



                    // LstStuffingSBSorted = (List<ContainerStuffingDtl>)lstListOfContainer;

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



                //if (ObjStuffing.SCMTRXML != null && (ObjStuffing.SCMTRXML != "") )
                //{
                //    List<ContainerStuffingV2SCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingV2SCMTR>>(ObjStuffing.SCMTRXML);
                //    SCMTRXML = Utility.CreateXML(LstSCMTR);
                //}
                string GREOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GREOperationCFSCodeWiseAmt != null)
                {
                    List<GREOperationCFSCodeWiseAmtV2> LstStuffingGRE1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREOperationCFSCodeWiseAmtV2>>(ObjStuffing.GREOperationCFSCodeWiseAmt.ToString());
                    GREOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGRE1);
                }

                string GREContainerWiseAmtXML = "";
                if (ObjStuffing.GREContainerWiseAmt != null)
                {
                    List<GREContainerWiseAmtV2> LstStuffingGRE2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREContainerWiseAmtV2>>(ObjStuffing.GREContainerWiseAmt.ToString());
                    GREContainerWiseAmtXML = Utility.CreateXML(LstStuffingGRE2);
                }

                string INSOperationCFSCodeWiseAmtLstXML = "";
                if (ObjStuffing.INSOperationCFSCodeWiseAmt != null)
                {
                    List<INSOperationCFSCodeWiseAmtV2> LstStuffingINS1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSOperationCFSCodeWiseAmtV2>>(ObjStuffing.INSOperationCFSCodeWiseAmt.ToString());
                    INSOperationCFSCodeWiseAmtLstXML = Utility.CreateXML(LstStuffingINS1);
                }

                string INSContainerWiseAmtXML = "";
                if (ObjStuffing.INSContainerWiseAmt != null)
                {
                    List<INSContainerWiseAmtV2> LstStuffingINS2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSContainerWiseAmtV2>>(ObjStuffing.INSContainerWiseAmt.ToString());
                    INSContainerWiseAmtXML = Utility.CreateXML(LstStuffingINS2);
                }

                string STOContainerWiseAmtXML = "";
                if (ObjStuffing.STOinvoicecargodtl != null)
                {
                    List<STOinvoicecargodtlV2> LstStuffingSTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOinvoicecargodtlV2>>(ObjStuffing.STOinvoicecargodtl.ToString());
                    STOContainerWiseAmtXML = Utility.CreateXML(LstStuffingSTO2);
                }
                string STOOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.STOOperationCFSCodeWiseAmt != null)
                {
                    List<STOOperationCFSCodeWiseAmtV2> LstStuffingSTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOOperationCFSCodeWiseAmtV2>>(ObjStuffing.STOOperationCFSCodeWiseAmt.ToString());
                    STOOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingSTO1);
                }

                string HNDOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.HNDOperationCFSCodeWiseAmt != null)
                {
                    List<HNDOperationCFSCodeWiseAmtV2> LstStuffingHND = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HNDOperationCFSCodeWiseAmtV2>>(ObjStuffing.HNDOperationCFSCodeWiseAmt.ToString());
                    HNDOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingHND);
                }

                string GENSPOperationCFSCodeWiseAmtXML = "";
                if (ObjStuffing.GENSPOperationCFSCodeWiseAmt != null)
                {
                    List<GENSPOperationCFSCodeWiseAmtV2> LstStuffingGENSP = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GENSPOperationCFSCodeWiseAmtV2>>(ObjStuffing.GENSPOperationCFSCodeWiseAmt.ToString());
                    GENSPOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGENSP);
                }

                string ShippingBillAmtXML = "";
                if (ObjStuffing.PPG_ShippingBillAmt != null)
                {
                    List<PPG_ShippingBillNoV2> LstShippingBill = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoV2>>(ObjStuffing.PPG_ShippingBillAmt.ToString());
                    ShippingBillAmtXML = Utility.CreateXML(LstShippingBill);
                }

                string ShippingBillAmtGenXML = "";
                if (ObjStuffing.PPG_ShippingBillAmtGen != null)
                {
                    List<PPG_ShippingBillNoGenV2> LstShippingBillGen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoGenV2>>(ObjStuffing.PPG_ShippingBillAmtGen.ToString());
                    ShippingBillAmtGenXML = Utility.CreateXML(LstShippingBillGen);
                }

                string ChargesXML = "";
                if (ObjStuffing.ChargesXML != null)
                {
                    List<Ppg_ContStuffChargesV2> lstCharges = JsonConvert.DeserializeObject<List<Ppg_ContStuffChargesV2>>(ObjStuffing.ChargesXML);
                    ChargesXML = Utility.CreateXML(lstCharges);
                }

                string BreakUpdateXML = "";
                if (ObjStuffing.BreakUpdateXML != null)
                {
                    List<ppgGRLPostPaymentChargebreakupdateV2> lstBreakUpdate = JsonConvert.DeserializeObject<List<ppgGRLPostPaymentChargebreakupdateV2>>(ObjStuffing.BreakUpdateXML);
                    BreakUpdateXML = Utility.CreateXML(lstBreakUpdate);
                }
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;

                //SCMTR
                //   ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, SCMTRXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                //  INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);
                //without SCMTR
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                 INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);

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
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PPG_ContainerStuffingV2)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuff(PPG_ContainerStuffingV2 ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CargoType = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "", Area = "", PortName = "", PortDestination = "", Remarks = "", chargetype = "", total = "", EquipmentSealType = "";

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
                    //CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : CustomSeal == item.CustomSeal ? CustomSeal : (CustomSeal + "<br/>" + item.CustomSeal));
                    if (!CustomSeal.Contains(item.CustomSeal))
                        CustomSeal += "<br/>" + item.CustomSeal;
                    if (!EquipmentSealType.Contains(item.EquipmentSealType))
                        EquipmentSealType += "<br/>" + item.EquipmentSealType;
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

                    CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));

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
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Final Destination location :</b> <u>" + ObjStuffing.CustodianCode + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size</b> <u>" + ObjStuffing.Size + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line </b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";

                Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Equipment Seal Type :</b> <u>" + EquipmentSealType + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'></td>  </tr></tbody></table> </td></tr>";


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


        [HttpGet]
        public JsonResult ListOfGREParty()
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();
            objImport.ListOfGREParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                ((List<PPG_ContainerStuffingV2>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GREPartyId = item.GREPartyId, GREPartyCode = item.GREPartyCode });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGroundRentEmpty(String StuffingDate, String ArrayOfContainer, int PartyId, int StuffingReqId)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGroundRentEmpty(StuffingDate, ContainerStuffingXML, PartyId, StuffingReqId);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfINSParty()
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();
            objImport.ListOfINSParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffingV2>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { INSPartyId = item.INSPartyId, INSPartyCode = item.INSPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateINS(String StuffingDate, String ArrayOfContainer, int INSPartyId)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }
            objImport.CalculateINS(StuffingDate, ContainerStuffingXML, INSPartyId);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfSTOParty()
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();
            objImport.ListOfSTOParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffingV2>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { STOPartyId = item.STOPartyId, STOPartyCode = item.STOPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateSTO(String StuffingDate, String ArrayOfContainer, int STOPartyId)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateSTO(StuffingDate, ContainerStuffingXML, STOPartyId);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfHandalingParty()
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();
            objImport.ListOfHandalingParty();
            //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<PPG_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<PPG_ContainerStuffingV2>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { HandalingPartyId = item.HandalingPartyId, HandalingPartyCode = item.HandalingPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateHandaling(String StuffingDate, String Origin, String Via, String ArrayOfContainer, int HandalingPartyId, String CargoType)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateHandaling(StuffingDate, Origin, Via, ContainerStuffingXML, HandalingPartyId, CargoType);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
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
                ((List<PPG_ContainerStuffingV2>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { GENSPPartyId = item.GENSPPartyId, GENSPPartyCode = item.GENSPPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGENSP(String StuffingDate, String ArrayOfContainer, int GENSPPartyId)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGENSP(StuffingDate, ContainerStuffingXML, GENSPPartyId);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
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
            return PartialView();
        }


        [HttpGet]
        public JsonResult GetShippingForCargo()
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.getOnlyRightsGodown();
            // GodownRepository ObjGR = new GodownRepository();
            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();

            // ObjGR.GetAllGodown();
            if (objExport.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)objExport.DBResponse.Data;
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
            List<CwcExim.Models.Godown> lstGodownF = new List<CwcExim.Models.Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodownF = (List<CwcExim.Models.Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);

            return Json(ViewBag.GodownListF, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
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

        /*[HttpPost]
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
        }*/
        [HttpPost]
        public JsonResult AddEditCargoShifting(/*Ppg_CargoShiftingV2 objForm,*/ List<CargoShiftingShipBillDetails> lstShipbills,
            int FromGodownId, int ToGodownId, int ToShippingId, int ShiftingType, int FromShippingLineId, string ShiftingDate, string Remarks, int CargoShiftingId)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                //var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string CartingRgisterDtlXML = "";
                /*string ChargesXML = "";
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
               }*/
                if (lstShipbills.Count > 0)
                {
                    CartingRgisterDtlXML = Utility.CreateXML(lstShipbills.Where(x => x.IsChecked == true).Select(x => x.CartingRegisterDtlId).ToList());
                }
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.AddEditCargoShiftInvoice(BranchId, ((Login)(Session["LoginUser"])).Uid,
                    CartingRgisterDtlXML, FromGodownId, ToGodownId, ToShippingId, ShiftingType, FromShippingLineId, ShiftingDate, Remarks, CargoShiftingId);

                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ListOfCargoshiting()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_ListOfCargoShiftV2> lstCargo = new List<Ppg_ListOfCargoShiftV2>();
            ObjER.ListOfCargoshifting(0);
            if (ObjER.DBResponse.Data != null)
            {
                lstCargo = (List<Ppg_ListOfCargoShiftV2>)ObjER.DBResponse.Data;
            }
            return PartialView(lstCargo);
        }
        [HttpGet]
        public JsonResult LoadMoreCargoshiting(int Page)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_ListOfCargoShiftV2> lstCargo = new List<Ppg_ListOfCargoShiftV2>();
            ObjER.ListOfCargoshifting(Page);
            if (ObjER.DBResponse.Data != null)
            {
                lstCargo = (List<Ppg_ListOfCargoShiftV2>)ObjER.DBResponse.Data;
            }
            return Json(lstCargo, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewCargoShifting(int CargoShiftingId)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            Ppg_CargoShifting objAppr = new Ppg_CargoShifting();
            objER.CargoShiftingDet(CargoShiftingId);
            if (objER.DBResponse.Data != null)
                objAppr = (Ppg_CargoShifting)objER.DBResponse.Data;
            return PartialView(objAppr);
        }
        #endregion

        #region Shipping line,exporter,cha,forwarder list and search by party code
        [HttpGet]
        public JsonResult Eximtraderlist(int Page, string PartyCode, int Exporter = 0, int ShippingLine = 0, int CHA = 0, int Forwarder = 0)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.EximtraderlistPopulation(Page, PartyCode, Exporter, ShippingLine, CHA, Forwarder);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ShppingLineReservedGodown(int EximTraderId)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.ShppingLineReservedGodown(EximTraderId);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            PPG_StuffingRequest ObjSR = new PPG_StuffingRequest();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();

            ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = new SelectList((List<PPG_StuffingRequest>)ObjER.DBResponse.Data, "CartingRegisterId", "CartingRegisterNo");
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }

            ObjER.GetPackUQCForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.PackUQCList = new SelectList((List<PackUQCForPage>)ObjER.DBResponse.Data, "PackUQCCode", "PackUQCDescription");
                ViewBag.PackUQCJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }

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
            /*if (CHA == true)
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
            }*/
            if (Exporter == true)
            {
                ViewData["IsExporter"] = true;
                ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
            }
            /*else
            {
                ViewData["IsExporter"] = false;
                ObjER.ListOfExporter();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                else
                    ViewBag.ListOfExporter = null;
            }*/

            ObjSR.RequestDate = DateTime.Now.ToString("dd-MM-yyyy");
            return PartialView("CreateStuffingRequest", ObjSR);
        }

        [HttpGet]
        public JsonResult ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStuffingReq(PPG_StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            //ObjER.ListOfCHA();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            //else
            //    ViewBag.CHAList = null;
            //ObjER.ListOfExporter();
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
                ObjER.GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_StuffingRequest)ObjER.DBResponse.Data;
                }
                /*If User is External Or Non CWC User*/
                bool Exporter, CHA;
                Exporter = ((Login)Session["LoginUser"]).Exporter;
                //CHA = ((Login)Session["LoginUser"]).CHA;
                //if (CHA == true)
                //{
                //    ViewData["IsCHA"] = true;
                //    ViewData["CHA"] = ((Login)Session["LoginUser"]).Name;
                //    ViewData["CHAId"] = ((Login)Session["LoginUser"]).EximTraderId;
                //    //ObjStuffing.CHA = ((Login)Session["LoginUser"]).Name;
                //    //ObjStuffing.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                //}
                //else
                //{
                //    ViewData["IsCHA"] = false;
                //    ObjER.ListOfCHA();
                //    if (ObjER.DBResponse.Data != null)
                //        ViewBag.ListOfCHA = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
                //    else
                //        ViewBag.ListOfCHA = null;
                //}


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
                if (Exporter == true)
                {
                    ViewData["IsExporter"] = true;
                    ViewData["ExporterName"] = ((Login)Session["LoginUser"]).Name;
                    ViewData["EximTraderId"] = ((Login)Session["LoginUser"]).EximTraderId;
                }
                /*else
                {
                    ViewData["IsExporter"] = false;
                    ObjER.ListOfExporter();
                    if (ObjER.DBResponse.Data != null)
                        ViewBag.ListOfExporter = new SelectList((List<Exporter>)ObjER.DBResponse.Data, "EXPEximTraderId", "ExporterName");
                    else
                        ViewBag.ListOfExporter = null;
                }*/

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
            return PartialView(ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId)
        {
            PPG_StuffingRequest ObjStuffing = new PPG_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjER.GetStuffingRequest(StuffinfgReqId, 0, 0);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PPG_StuffingRequest)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjER.DeleteStuffingRequest(StuffinfgReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetCartRegDetForStuffingReq(int CartingRegisterId, string flag)
        {
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            if (CartingRegisterId > 0)
            {
                objER.GetCartRegDetForStuffingReq(CartingRegisterId, flag);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<PPG_StuffingRequest> LstStuffing = new List<PPG_StuffingRequest>();
            ObjER.ListofStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }
        [HttpGet]
        public JsonResult LoadMoreStuffingReq(int Page)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            var LstStuffing = new List<PPG_StuffingRequest>();
            ObjER.ListofStuffingRequest(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<PPG_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerNoDet(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
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
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetShipBillForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, int PartyId,
            List<CwcExim.Areas.Export.Models.PaymentSheetContainer> lstPaySheetContainer, string SEZ,
            int InvoiceId = 0)
        {
            //AppraisementId ----> StuffingReqID

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            Loni_ExportRepositoryV2 objPpgRepo = new Loni_ExportRepositoryV2();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetBTTPaymentSheet(InvoiceDate, AppraisementId, TaxType, PartyId, XMLText, InvoiceId, SEZ);
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
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.AddEditBTTInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", CargoXML, invoiceData.SEZ);

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


        #region Edit Container Movement

        [HttpGet]
        public ActionResult EditContainerMovementInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();


            //    objImport.GetInvoiceForEdit("EXPMovement");
            //   if (objImport.DBResponse.Status > 0)
            //      ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //  else
            //     ViewBag.InvoiceList = null;
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();

            objExport.GetPaymentPartyforEditMovement();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditContMovementPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.EditContainerMovementInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

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
        public JsonResult GetContainerMovementInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Loni_ExportRepositoryV2 objCashManagement = new Loni_ExportRepositoryV2();
                objCashManagement.GetContainerMovementInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = (Areas.Export.Models.PPG_MovementInvoice)objCashManagement.DBResponse.Data;

                    IList<CwcExim.Areas.Import.Models.PaymentSheetContainer> containers = new List<CwcExim.Areas.Import.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new CwcExim.Areas.Import.Models.PaymentSheetContainer
                        {
                            ContainerNo = item.ContainerNo,
                            ArrivalDt = item.ArrivalDate,
                            CFSCode = item.CFSCode,
                            IsHaz = (item.CargoType == 1 ? "Yes" : "No"),
                            Selected = true,
                            Size = item.Size
                        });
                    });


                    /***************BOL PRINT******************/
                    var BOL = "";
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


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditInternalPaymentSheetForEditInvoice(String InvoiceObj)
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

                var invoiceData = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(InvoiceObj);



                //var invoiceDataa = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJsonn"].ToString());

                //var invoiceDataaa = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(objForm["PaymentSheetModelJsonnn"].ToString());
                var Shipbillwiseamount = JsonConvert.DeserializeObject<PPG_Movement_InvoiceV2>(InvoiceObj);
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
                string SbXML = "";
                string OperationCfsCodeWiseAmtXMLLL = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }


                //foreach (var item in invoiceDataaa.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "01/01/1900" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                //}
                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                invoiceData.TotalCGST = invoiceData.lstPostPaymentChrg.Sum(x => x.CGSTAmt);
                invoiceData.TotalSGST = invoiceData.lstPostPaymentChrg.Sum(x => x.SGSTAmt);
                invoiceData.TotalIGST = invoiceData.lstPostPaymentChrg.Sum(x => x.IGSTAmt);
                invoiceData.TotalAmt = invoiceData.lstPostPaymentChrg.Sum(x => x.Amount);
                invoiceData.CWCTotal = invoiceData.TotalAmt;
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

                //if (invoiceDataa.lstPostPaymentCont != null)
                //{
                //    ContainerXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentCont);
                //}
                //if (invoiceDataa.lstPostPaymentChrg != null)
                //{
                //    ChargesXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrg);
                //}
                //if (invoiceDataa.lstContWiseAmount != null)
                //{
                //    ContWiseChargg = Utility.CreateXML(invoiceDataa.lstContWiseAmount);
                //}
                //if (invoiceDataa.lstOperationCFSCodeWiseAmount != null)
                //{
                //    OperationCfsCodeWiseAmtXMLL = Utility.CreateXML(invoiceDataa.lstOperationCFSCodeWiseAmount);
                //}

                //if (invoiceDataaa.lstPostPaymentCont != null)
                //{
                //    ContainerXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentCont);
                //}
                //if (invoiceDataaa.lstPostPaymentChrg != null)
                //{
                //    ChargesXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrg);
                //}
                //if (invoiceDataaa.lstContWiseAmount != null)
                //{
                //    ContWiseCharggg = Utility.CreateXML(invoiceDataaa.lstContWiseAmount);
                //}
                //if (invoiceDataaa.lstOperationCFSCodeWiseAmount != null)
                //{
                //    OperationCfsCodeWiseAmtXMLLL = Utility.CreateXML(invoiceDataaa.lstOperationCFSCodeWiseAmount);
                //}

                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                //if (invoiceDataa.lstPostPaymentChrgBreakup != null)
                //{
                //    ChargesBreakupXMLL = Utility.CreateXML(invoiceDataa.lstPostPaymentChrgBreakup);
                //}
                //if (invoiceDataaa.lstPostPaymentChrgBreakup != null)
                //{
                //    ChargesBreakupXMLLL = Utility.CreateXML(invoiceDataaa.lstPostPaymentChrgBreakup);
                //}
                if (Shipbillwiseamount.lstShipbillwiseAmountV2 != null)
                {
                    SbXML = Utility.CreateXML(Shipbillwiseamount.lstShipbillwiseAmountV2);
                }
                decimal TareWeight = 0, CargoWeight = 0;
                if (invoiceData.TareWeight != null)
                    TareWeight = Convert.ToDecimal(invoiceData.TareWeight);
                if (invoiceData.CargoWeight != null)
                    CargoWeight = Convert.ToDecimal(invoiceData.CargoWeight);
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.AddEditInvoiceMovement(invoiceData, null, null, ContainerXML, ContainerXMLL, ContainerXMLLL, ChargesXML, ChargesXMLL, ChargesXMLLL, ContWiseCharg, ContWiseChargg, ContWiseCharggg, OperationCfsCodeWiseAmtXML, OperationCfsCodeWiseAmtXMLL, OperationCfsCodeWiseAmtXMLLL, ChargesBreakupXML, ChargesBreakupXMLL, ChargesBreakupXMLLL, SbXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPMovement", TareWeight, CargoWeight, "");
                PPG_InvoiceListV2 inv = new PPG_InvoiceListV2();
                //inv = (PPG_InvoiceListV2)objChargeMaster.DBResponse.Data;

                //invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //if (inv != null)
                //{
                //    invoiceData.InvoiceNo = inv.invoiceno;
                //    //invoiceData.invoicenoo = inv.invoicenoo;
                //    //invoiceData.invoicenooo = inv.invoicenooo;
                //    invoiceData.MovementNo = inv.MovementNo;
                //}
                //invoiceDataa.ROAddress = (invoiceDataa.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceDataa.CompanyAddress = (invoiceDataa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    // invoiceDataa.InvoiceNo = inv.invoicenoo;
                }
                //invoiceDataaa.ROAddress = (invoiceDataaa.ROAddress).Replace("|_br_|", "<br/>");
                //invoiceDataaa.CompanyAddress = (invoiceDataaa.CompanyAddress).Replace("|_br_|", "<br/>");
                if (inv != null)
                {
                    // invoiceDataaa.InvoiceNo = inv.invoicenooo;
                }

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


        #region LeoEntry
        public ActionResult LeoEntry()
        {
            return PartialView();
        }
        public JsonResult SearchMCIN(string SBNo, string SBDATE)
        {
            Loni_ExportRepositoryV2 objRepo = new Loni_ExportRepositoryV2();
            objRepo.GetMCIN(SBNo, SBDATE);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLEO(LEOPage objLEOPage)
        {


            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();

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
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();





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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();





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
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
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



        #region Edit Container Stuffing 3.2

        [HttpGet]
        public ActionResult EditContainerStuffingInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();


            //   objImport.GetInvoiceForEdit("EXPCSGRE");
            //   if (objImport.DBResponse.Status > 0)
            //      ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //   else
            //      ViewBag.InvoiceList = null;
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();

            objExport.GetPaymentPartyforEditStuffing();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }


        [HttpGet]
        public JsonResult GetContainerStuffingInvoiceDetails(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Loni_ExportRepositoryV2 objCashManagement = new Loni_ExportRepositoryV2();
                objCashManagement.GetContainerStuffingInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Export.Models.PPG_MovementInvoice objPostPaymentSheet = (Areas.Export.Models.PPG_MovementInvoice)objCashManagement.DBResponse.Data;

                    IList<ContainerStuffingDtlV2> containers = new List<ContainerStuffingDtlV2>();
                    objPostPaymentSheet.LstStuffingDtl.ToList().ForEach(item =>
                    {
                        containers.Add(new ContainerStuffingDtlV2
                        {
                            ContainerStuffingDtlId = item.ContainerStuffingDtlId,
                            ContainerStuffingId = item.ContainerStuffingId,
                            StuffingReqDtlId = item.StuffingReqDtlId,
                            CFSCode = item.CFSCode,
                            StuffingType = item.StuffingType,
                            Consignee = item.Consignee,
                            MarksNo = item.MarksNo,
                            Insured = item.Insured,
                            ContainerNo = item.ContainerNo,
                            ShippingBillNo = item.ShippingBillNo,
                            ShippingDate = item.ShippingDate,
                            ExporterId = item.ExporterId,
                            Exporter = item.Exporter,
                            CHAId = item.CHAId,
                            CHA = item.CHA,
                            CargoDescription = item.CargoDescription,
                            CustomSeal = item.CustomSeal,
                            ShippingSeal = item.ShippingSeal,
                            Fob = item.Fob,
                            ShippingLineId = item.ShippingLineId,
                            ShippingLine = item.ShippingLine,
                            Size = item.Size,
                            StuffQuantity = item.StuffQuantity,
                            StuffWeight = item.StuffWeight
                        });
                    });


                    /***************BOL PRINT******************/
                    var BOL = "";
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


        [HttpPost]
        public JsonResult CalculateGroundRentEmptyforEdit(String StuffingDate, String ArrayOfContainer, int PartyId, int StuffingReqId, int invoiceid)
        {
            Loni_ExportRepositoryV2 objImport = new Loni_ExportRepositoryV2();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<PPGContainerDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGContainerDtlV2>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGroundRentEmptyForEdit(StuffingDate, ContainerStuffingXML, PartyId, StuffingReqId, invoiceid);
            PPG_ContainerStuffingV2 objImp = new PPG_ContainerStuffingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (PPG_ContainerStuffingV2)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDetForEdit(PPG_ContainerStuffingV2 ObjStuffing)
        {

            string ContainerStuffingXML = "";
            string SCMTRXML = "";
            if (ObjStuffing.StuffingXML != null)
            {
                List<ContainerStuffingDtlV2> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingDtlV2>>(ObjStuffing.StuffingXML);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }



            if (ObjStuffing.SCMTRXML != null && (ObjStuffing.SCMTRXML != ""))
            {
                List<ContainerStuffingV2SCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContainerStuffingV2SCMTR>>(ObjStuffing.SCMTRXML);
                SCMTRXML = Utility.CreateXML(LstSCMTR);
            }
            string GREOperationCFSCodeWiseAmtXML = "";
            if (ObjStuffing.GREOperationCFSCodeWiseAmt != null)
            {
                List<GREOperationCFSCodeWiseAmtV2> LstStuffingGRE1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREOperationCFSCodeWiseAmtV2>>(ObjStuffing.GREOperationCFSCodeWiseAmt.ToString());
                GREOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGRE1);
            }

            string GREContainerWiseAmtXML = "";
            if (ObjStuffing.GREContainerWiseAmt != null)
            {
                List<GREContainerWiseAmtV2> LstStuffingGRE2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREContainerWiseAmtV2>>(ObjStuffing.GREContainerWiseAmt.ToString());
                GREContainerWiseAmtXML = Utility.CreateXML(LstStuffingGRE2);
            }

            string INSOperationCFSCodeWiseAmtLstXML = "";
            if (ObjStuffing.INSOperationCFSCodeWiseAmt != null)
            {
                List<INSOperationCFSCodeWiseAmtV2> LstStuffingINS1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSOperationCFSCodeWiseAmtV2>>(ObjStuffing.INSOperationCFSCodeWiseAmt.ToString());
                INSOperationCFSCodeWiseAmtLstXML = Utility.CreateXML(LstStuffingINS1);
            }

            string INSContainerWiseAmtXML = "";
            if (ObjStuffing.INSContainerWiseAmt != null)
            {
                List<INSContainerWiseAmtV2> LstStuffingINS2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSContainerWiseAmtV2>>(ObjStuffing.INSContainerWiseAmt.ToString());
                INSContainerWiseAmtXML = Utility.CreateXML(LstStuffingINS2);
            }

            string STOContainerWiseAmtXML = "";
            if (ObjStuffing.STOinvoicecargodtl != null)
            {
                List<STOinvoicecargodtlV2> LstStuffingSTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOinvoicecargodtlV2>>(ObjStuffing.STOinvoicecargodtl.ToString());
                STOContainerWiseAmtXML = Utility.CreateXML(LstStuffingSTO2);
            }
            string STOOperationCFSCodeWiseAmtXML = "";
            if (ObjStuffing.STOOperationCFSCodeWiseAmt != null)
            {
                List<STOOperationCFSCodeWiseAmtV2> LstStuffingSTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOOperationCFSCodeWiseAmtV2>>(ObjStuffing.STOOperationCFSCodeWiseAmt.ToString());
                STOOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingSTO1);
            }

            string HNDOperationCFSCodeWiseAmtXML = "";
            if (ObjStuffing.HNDOperationCFSCodeWiseAmt != null)
            {
                List<HNDOperationCFSCodeWiseAmtV2> LstStuffingHND = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HNDOperationCFSCodeWiseAmtV2>>(ObjStuffing.HNDOperationCFSCodeWiseAmt.ToString());
                HNDOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingHND);
            }

            string GENSPOperationCFSCodeWiseAmtXML = "";
            if (ObjStuffing.GENSPOperationCFSCodeWiseAmt != null)
            {
                List<GENSPOperationCFSCodeWiseAmtV2> LstStuffingGENSP = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GENSPOperationCFSCodeWiseAmtV2>>(ObjStuffing.GENSPOperationCFSCodeWiseAmt.ToString());
                GENSPOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGENSP);
            }

            string ShippingBillAmtXML = "";
            if (ObjStuffing.PPG_ShippingBillAmt != null)
            {
                List<PPG_ShippingBillNoV2> LstShippingBill = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoV2>>(ObjStuffing.PPG_ShippingBillAmt.ToString());
                ShippingBillAmtXML = Utility.CreateXML(LstShippingBill);
            }

            string ShippingBillAmtGenXML = "";
            if (ObjStuffing.PPG_ShippingBillAmtGen != null)
            {
                List<PPG_ShippingBillNoGenV2> LstShippingBillGen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ShippingBillNoGenV2>>(ObjStuffing.PPG_ShippingBillAmtGen.ToString());
                ShippingBillAmtGenXML = Utility.CreateXML(LstShippingBillGen);
            }

            string ChargesXML = "";
            if (ObjStuffing.ChargesXML != null)
            {
                List<Ppg_ContStuffChargesV2> lstCharges = JsonConvert.DeserializeObject<List<Ppg_ContStuffChargesV2>>(ObjStuffing.ChargesXML);
                ChargesXML = Utility.CreateXML(lstCharges);
            }

            string BreakUpdateXML = "";
            if (ObjStuffing.BreakUpdateXML != null)
            {
                List<ppgGRLPostPaymentChargebreakupdateV2> lstBreakUpdate = JsonConvert.DeserializeObject<List<ppgGRLPostPaymentChargebreakupdateV2>>(ObjStuffing.BreakUpdateXML);
                BreakUpdateXML = Utility.CreateXML(lstBreakUpdate);
            }
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;

            //SCMTR
            //   ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, SCMTRXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
            //  INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);
            //without SCMTR
            ObjER.AddEditContainerStuffingForEdit(ObjStuffing, ContainerStuffingXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
             INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML, ChargesXML, BreakUpdateXML);

            return Json(ObjER.DBResponse);
        }



        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult EditContStuffingPaymentSheet(Export.Models.PPG_MovementInvoice objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";

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

                //Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                Loni_ExportRepositoryV2 objChargeMaster = new Loni_ExportRepositoryV2();
                objChargeMaster.EditContainerStuffInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

                //    invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


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

        #region BTT Payment Sheet 3.2 Edit

        [HttpGet]
        public ActionResult EditBTTPaymentInvoice()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            Ppg_CashManagementRepository objCashManagement = new Ppg_CashManagementRepository();

            //   objImport.GetInvoiceForEdit("BTT");
            //   if (objImport.DBResponse.Status > 0)
            //      ViewBag.InvoiceList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //   else
            //      ViewBag.InvoiceList = null;
            Ppg_ExportRepository objExport = new Ppg_ExportRepository();

            objExport.GetPaymentPartyforBTTEdit();
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
        public JsonResult GetBTTInvoiceDetailsForEdit(int InvoiceId)
        {
            try
            {
                ChargeMasterRepository objCharge = new ChargeMasterRepository();
                ImportRepository objImportRepo = new ImportRepository();

                //objCharge.GetAllCharges();
                Loni_ExportRepositoryV2 objCashManagement = new Loni_ExportRepositoryV2();
                objCashManagement.GetBTTInvoiceDetailsForEdit(InvoiceId);
                if (objCashManagement.DBResponse.Status == 1)
                {
                    Areas.Import.Models.PpgInvoiceYard objPostPaymentSheet = (Areas.Import.Models.PpgInvoiceYard)objCashManagement.DBResponse.Data;

                    IList<CwcExim.Areas.Export.Models.PaymentSheetContainer> containers = new List<CwcExim.Areas.Export.Models.PaymentSheetContainer>();
                    objPostPaymentSheet.lstPostPaymentCont.ToList().ForEach(item =>
                    {
                        containers.Add(new CwcExim.Areas.Export.Models.PaymentSheetContainer
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
                    List<CwcExim.Areas.Export.Models.PaymentSheetContainer> containersAll = new List<CwcExim.Areas.Export.Models.PaymentSheetContainer>();
                    //      objImportRepo.GetCFSCodeForEdit(objPostPaymentSheet.RequestId, "Yard");
                    //     if (objImportRepo.DBResponse.Status == 1)
                    //   {

                    //     containersAll = JsonConvert.DeserializeObject<List<PaymentSheetContainer>>(JsonConvert.SerializeObject(objImportRepo.DBResponse.Data));
                    //    containersAll.Where(o1 => !containers.Any(o2 => o1.CFSCode == o2.CFSCode)).ToList().ForEach(item =>
                    //    {
                    //     containers.Add(new PaymentSheetContainer
                    //    {
                    //    ContainerNo = item.ContainerNo,
                    //    ArrivalDt = item.ArrivalDt,
                    //    CFSCode = item.CFSCode,
                    //   IsHaz = (item.IsHaz == "Haz" ? "Yes" : "No"),
                    //  Selected = false,
                    //  Size = item.Size
                    // });
                    //   });

                    return Json(new { Status = 1, Data = objPostPaymentSheet, Containers = containers }, JsonRequestBehavior.AllowGet);

                }

            }

            catch (Exception ex)
            {
                return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = -1 }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region Container Destuffing
        public ActionResult CreateExportDestuffing(int DestuffingEntryId = 0)
        {
            log.Info("after calling ");
            Ppg_ExportDestuffing ObjDestuffing = new Ppg_ExportDestuffing();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            GodownRepository ObjGR = new GodownRepository();
            List<Import.Models.Godown> lstGodown = new List<Import.Models.Godown>();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                ViewBag.GodownList = ObjGR.DBResponse.Data;
            }
            else
            {
                ViewBag.GodownList = null;
            }
            //log.Info("after GetAllGodown ");
            //log.Info(DestuffingEntryId);

            if (DestuffingEntryId > 0)
            {

                ObjER.GetDestuffingEntryDetailsById(DestuffingEntryId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjDestuffing = (Ppg_ExportDestuffing)ObjER.DBResponse.Data;
                }
            }
            else
            {
                ObjDestuffing.Destuffingdate = DateTime.Now.ToString("dd/mm/yyyy");
            }
            //log.Info("after final ");
            return PartialView(ObjDestuffing);

        }

        [HttpGet]
        public JsonResult GetContainerNoForExportDestuffing()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainersForExpDestuffing();
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSBDetForExportDestuffing(string CFSCode, string OperationType)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetSBDetForExpDestuffing(CFSCode, OperationType);
            if (ObjER.DBResponse.Status > 0)
                return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult AddEditExpDestuffing(Ppg_ExportDestuffing objDestuff, String lstDestuffDetail)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
                List<Ppg_ExportDestuffDetails> lstDestuff = new List<Ppg_ExportDestuffDetails>();
                if (lstDestuffDetail.Length > 0)
                {
                    lstDestuff = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ppg_ExportDestuffDetails>>(lstDestuffDetail);
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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntry(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Ppg_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }



        [HttpGet]
        public ActionResult DestuffingEntrySr(string search)
        {

            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            ObjIR.DestuffingEntrySr(search);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Ppg_ExportDestuffingList>)ObjIR.DBResponse.Data;
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
            Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
            List<Ppg_ExportDestuffingList> LstDestuffing = new List<Ppg_ExportDestuffingList>();
            ObjIR.GetAllDestuffingEntry(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Ppg_ExportDestuffingList>)ObjIR.DBResponse.Data;
            }
            return Json(LstDestuffing, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ViewExportDestuffingEntry(int DestuffingEntryId)
        {
            Ppg_ExportDestuffing ObjDestuffing = new Ppg_ExportDestuffing();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetDestuffingEntryDetailsById(DestuffingEntryId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjDestuffing = (Ppg_ExportDestuffing)ObjER.DBResponse.Data;
            }
            return PartialView(ObjDestuffing);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                Loni_ExportRepositoryV2 ObjIR = new Loni_ExportRepositoryV2();
                ObjIR.DelDestuffingEntry(DestuffingEntryId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            Loni_ExportRepositoryV2 objExp = new Loni_ExportRepositoryV2();
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
        public ActionResult EditContainerStuffingApproval(int ApprovalId)
        {
            PortOfCall ObjStuffing = new PortOfCall();

            if (ApprovalId > 0)
            {
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjER.GetContainerStuffingApprovalById(ApprovalId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
                }
            }
            return PartialView("EditContainerStuffingApproval", ObjStuffing);

        }

        [HttpGet]
        public JsonResult SearchPortOfCallByPortCode(string PortCode)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 objCR = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<PortOfCall> LstStuffingApproval = new List<PortOfCall>();
            ObjER.GetAllContainerStuffingApprovalSearch(SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<PortOfCall>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfContainerStuffingApproval", LstStuffingApproval);
        }

        #endregion
        #region  Container Delivery

        [HttpGet]
        public ActionResult CreateContainerDeliveryInformation()
        {
            //Loni_ExportRepositoryV2 objExp = new Loni_ExportRepositoryV2();
            //objExp.GetContStuffingForApproval();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.StuffingReqList = null;

            //objExp.GetPortOfCallForPage("", 0);
            //if (objExp.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstPortOfCall = Jobject["lstPortOfCall"];
            //    ViewBag.StatePortOfCall = Jobject["StatePortOfCall"];
            //}
            //else
            //{
            //    ViewBag.lstPortOfCall = null;
            //}

            //objExp.GetNextPortOfCallForPage("", 0);
            //if (objExp.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstNextPortOfCall = Jobject["lstNextPortOfCall"];
            //    ViewBag.StateNextPortOFCall = Jobject["StateNextPortOFCall"];
            //}
            //else
            //{
            //    ViewBag.lstNextPortOfCall = null;
            //}


            return PartialView();
        }


        [HttpGet]
        public JsonResult LoadContainer()
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetContainerNoforContDelivery();
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadContainerdetails(string GatePassId)
        {
            Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
            objExport.GetContainerNoforContDeliveryDetails(GatePassId);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerDelivery(Ppg_ContainerDeliverySystem Containerdelivery)
        {
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 objCR = new Loni_ExportRepositoryV2();
                objCR.AddEditContainerDelivery(Containerdelivery, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult GetContainerDeliveryList()
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_ContainerDeliverySystem> LstStuffingApproval = new List<Ppg_ContainerDeliverySystem>();
            ObjER.ListofContainerDelivery(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Ppg_ContainerDeliverySystem>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfContainerDelivery", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadContainerDeliveryList(int Page)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_ContainerDeliverySystem> LstStuffingApproval = new List<Ppg_ContainerDeliverySystem>();
            ObjER.ListofContainerDelivery(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Ppg_ContainerDeliverySystem>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetContainerDeliverySearch(string SearchValue = "")
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_ContainerDeliverySystem> LstStuffingApproval = new List<Ppg_ContainerDeliverySystem>();
            ObjER.GetAllContainerContainerDeliverySearch(SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Ppg_ContainerDeliverySystem>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfContainerDelivery", LstStuffingApproval);
        }
        [HttpGet]
        public ActionResult ViewContainerDelivery(int id)
        {
            Ppg_ContainerDeliverySystem ObjStuffing = new Ppg_ContainerDeliverySystem();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerDeliveryId(id);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Ppg_ContainerDeliverySystem)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }
        #endregion


        #region  Loaded Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateLoadContainerStuffingApproval()
        {
            Loni_ExportRepositoryV2 objExp = new Loni_ExportRepositoryV2();
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
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
                Loni_ExportRepositoryV2 objCR = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetLoadContainerStuffingApprovalById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (PortOfCall)ObjER.DBResponse.Data;
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

                log.Error("SendASR Method Start .....");
                int k = 0;
                int j = 1;
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                //WFLD_ContainerStuffing ObjStuffing = new WFLD_ContainerStuffing();
                log.Error("Repository Call .....");
                ObjER.GetLoadedContainerCIMASRDetails(ContainerStuffingId, "F");
                log.Error("Repository Done .....");
                DataSet ds = new DataSet();

                if (ObjER.DBResponse.Status == 1)
                {
                    ds = (DataSet)ObjER.DBResponse.Data;

                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        log.Error("Read File Name .....");
                        //string Filenm = Convert.ToString(ds.Tables[6].Rows[0]["FileName"]);
                        string Filenm = Convert.ToString(dr["FileName"]);
                        log.Error("Call Class Libarary File .....");
                        string JsonFile = StuffingCIMACRJsonFormat.StuffingCIMACRJson(ds, Convert.ToInt32(dr["ContainerStuffingDtlId"]));

                        log.Error("Call Class Libarary Done .....");


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
                        string DSCPASSWORD = System.Configuration.ConfigurationManager.AppSettings["DSCPASSWORD"];
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


                            //if (status.ToString() == "Success")
                            //{
                            //    using (FileStream fs = System.IO.File.OpenRead(FinalOutPutPath))
                            //    {
                            //        log.Info("FTP File read process has began");
                            //        byte[] buffer = new byte[fs.Length];
                            //        fs.Read(buffer, 0, buffer.Length);
                            //        fs.Close();
                            //        string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
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
                        //    objExport.GetLoadContCIMASRDetailsUpdateStatus(ContainerStuffingId);
                        //    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                        //}

                        log.Info("FTP File upload has been end");

                    }
                    Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
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
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
                        //  {
                        Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
                        objExport.GetCIMSFDetailsUpdateStatus(ContainerStuffingId);
                        //return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                        // }
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
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
                        Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
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


        #region Send SF Amendment

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SendAmendmentSF(int ContainerStuffingId)
        {
            int k = 0;
            int j = 1;
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetCIMSFDetails(ContainerStuffingId, "A");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                string Filenm = Convert.ToString(ds.Tables[5].Rows[0]["FileName"]);
                string JsonFile = StuffingSBJsonFormat.ContStuffingDetJson(ds);




                string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMSF"];
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
                    string SCMTRPath = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];
                    output = FtpFileManager.UploadFileToFtp(SCMTRPath, Filenm, buffer, "5000", FileName);
                    log.Info("FTP File read process has ended");
                }
                if (output == "Success")
                {
                    Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
                    objExport.GetAmenCIMSFDetailsUpdateStatus(ContainerStuffingId);
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


        #region  Send ASR 
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public async Task<JsonResult> SendASR(int ContainerStuffingId)
        {
            try
            {
                int k = 0;
                int j = 1;
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
                    Loni_ExportRepositoryV2 objExport = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            Ppg_ActualArrivalDatetime objActualArrival = new Ppg_ActualArrivalDatetime();

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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchContainer(string ContainerBoxSearch, int Page)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditActualArrivalDatetime(Ppg_ActualArrivalDatetime objActualArrivalDatetime)
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            List<Ppg_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Ppg_ActualArrivalDatetime>();
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            //objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, 0);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Ppg_ActualArrivalDatetime>)objER.DBResponse.Data;
            return PartialView(lstActualArrivalDatetime);
        }

        [HttpGet]
        public JsonResult EditActualArrivalDatetime(int actualArrivalDatetimeId)
        {
            List<Ppg_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Ppg_ActualArrivalDatetime>();
            Loni_ExportRepositoryV2 objER = new Loni_ExportRepositoryV2();
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, actualArrivalDatetimeId);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Ppg_ActualArrivalDatetime>)objER.DBResponse.Data;
            //return Json(lstActualArrivalDatetime);
            return Json(lstActualArrivalDatetime, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SendAT(string CFSCode)
        {


            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 objExp = new Loni_ExportRepositoryV2();
            Ppg_LoadContSF ObjPC = new Ppg_LoadContSF();
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
            Ppg_LoadContSF ObjStuffing = new Ppg_LoadContSF();

            if (LoadContReqId > 0)
            {
                Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
                ObjER.GetLoadContainerStuffingSFById(LoadContReqId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (Ppg_LoadContSF)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingSF(Ppg_LoadContSF objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                Loni_ExportRepositoryV2 objCR = new Loni_ExportRepositoryV2();
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
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            List<Ppg_LoadContSF> LstStuffingApproval = new List<Ppg_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Ppg_LoadContSF>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingSF", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingSFList(int Page, string SearchValue = "")
        {
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            var LstStuffingApproval = new List<Ppg_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<Ppg_LoadContSF>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingSF(int ApprovalId)
        {
            Ppg_LoadContSF ObjStuffing = new Ppg_LoadContSF();
            Loni_ExportRepositoryV2 ObjER = new Loni_ExportRepositoryV2();
            ObjER.GetLoadContainerStuffingSFById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (Ppg_LoadContSF)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion        
    }
}