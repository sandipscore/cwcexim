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
using CwcExim.Areas.GateOperation.Models;

using System.Text;
using SCMTRLibrary;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CwcExim.Areas.Export.Controllers
{
    public class WFLD_CWCExportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Export/WFLD_CWCExport
        #region CCINEntry
        public ActionResult CCINEntry(int Id = 0, int PartyId = 0)
        {
            ExportRepository objER = new ExportRepository();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            //   objER.GetCCINShippingLine();
            //   if (objER.DBResponse.Data != null)
            //   {
            //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
            //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            //  }
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
            ObjER.GetSBListpagewise("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfSBNo = Jobject["LstSB"];
                ViewBag.StateShipbill = Jobject["State"];
            }
            else
            {
                ViewBag.ListOfSBNo = null;
            }
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}

            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            WFLD_CountryRepository ObjCR = new WFLD_CountryRepository();
            /*ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }*/

            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
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

            WFLD_CCINEntry objCCINEntry = new WFLD_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                WFLD_ExportRepository rep = new WFLD_ExportRepository();
                rep.GetCCINEntryForEdit(Id, PartyId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (WFLD_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }
        [HttpGet]
        public JsonResult LoadCCINShippingLine(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCCINShippingLineByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetCCINShippingLineForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadExporterList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfExporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchExporterByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfExporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPackUQCList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPackUQCByCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfPackUQCForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchShipbill(string sb)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetSBListpagewise(sb, 0);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadSbLists(string sb, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetSBListpagewise(sb, Page);
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        /*  [HttpGet]
          public JsonResult GetShippingBill()
          {
              WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
              objRepo.GetSBList();
              if (objRepo.DBResponse.Data != null)
              {
                  ViewBag.ListOfSBNo = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
              }

              var jsonResult = Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
              jsonResult.MaxJsonLength = int.MaxValue;
              return jsonResult;
             // return Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
          }*/


        [HttpGet]
        public JsonResult GetInvParty()
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSBDetailsBySBId(int SBId)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetSBDetailsBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetAllCountry()
        {
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Status > 0)
                return Json(ObjCR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCCINCharges(String CCINNo, int Pos, int PartyId, int PayeeId, string InvType, string InvDate, int VehicleNo, string SEZ, int InvoiceId = 0)
        {
            WFLD_CCINInvoice objCCINEntry = new WFLD_CCINInvoice();
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetCCINCharges(CCINNo, Pos, PartyId, PayeeId, InvType, InvDate, VehicleNo, SEZ, InvoiceId);
            objCCINEntry = (WFLD_CCINInvoice)objExport.DBResponse.Data;
            //    ViewBag.PaymentMode = objCCINEntry.PaymentMode;
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntry(WFLD_CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository objER = new WFLD_ExportRepository();

                //  objCCINEntry.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                //  string PostPaymentXML = Utility.CreateXML(objCCINEntry.lstPostPaymentChrg);


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
        public ActionResult ListOfCCINEntry(string SearchValue = "", string SearchCCIN = "")
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> lstCCINEntry = new List<WFLD_CCINEntry>();
            objER.GetAllCCINEntryForPage(0, SearchValue, SearchCCIN);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<WFLD_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }


        [HttpGet]
        public JsonResult LoadMoreCCINEntryList(int Page, string SearchValue)
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
        public JsonResult DeleteCCINEntry(int CCINEntryId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCartingApplication(string InvoiceNo)
        {
            WFLD_ReportRepository obrr = new WFLD_ReportRepository();
            WFLD_CartingApp obcartapp = new WFLD_CartingApp();
            obrr.GenericInvoiceDetailsForPrint(InvoiceNo);
            if (obrr.DBResponse.Data != null)
            {
                obcartapp = (WFLD_CartingApp)obrr.DBResponse.Data;
                string Path = GeneratePdfForCartingApplication(obcartapp, InvoiceNo);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [NonAction]
        public string GeneratePdfForCartingApplication(WFLD_CartingApp obcartapp, string InvoiceNo)
        {
            int SerialNo = 1;
            string Html = "";
            String Cargo;

            Html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";

            Html += "<tr><td colspan='12'>";
            Html += "<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>";
            Html += "<tr><td width='65%'></td>";
            Html += "<td width='10%' align='right'>";
            Html += "<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>";
            Html += "<tr><td style='border:1px solid #333;'>";
            Html += "<div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/04</div>";
            Html += "</td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            Html += "<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>CONTAINER FREIGHT STATION</span><br/><label style='font-size: 12px;'>107/109, EPIP Zone, KIADB Industrial Area, Whitefield, Bangalore-560 066</label><br/><label style='font-size: 14px; font-weight:bold;'>CARTING APPLICATION</label></td>";
            Html += "<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-size:9.5pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            Html += "<tr><td colspan='6' width='50%'><b>Carting No. </b> Dynamic <br/> <b>(To be assigned by CWC, CFS Officials)</b></td>";
            Html += "<td colspan='6' width='50%' style='text-align:right;'><b>Date:</b> <u>............</u></td></tr>";
            Html += "<tr><td colspan='12'><b>Please receive the following cargo for Export as FCL / LCL</b></td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='10' style='width:100%; text-align:center; border:1px solid #000; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";
            Html += "<thead>";
            Html += "<tr><th style='border-right:1px solid #000;'>Sl.No.</th>";
            Html += "<th style='border-right:1px solid #000;'>Shipping Bill No</th>";
            Html += "<th style='border-right:1px solid #000;'>Name of Exporter</th>";
            Html += "<th style='border-right:1px solid #000;'>Cargo</th>";
            Html += "<th style='border-right:1px solid #000;'>Marks & Nos.</th>";
            Html += "<th style='border-right:1px solid #000;'>No.of Units</th>";
            Html += "<th style='border-right:1px solid #000;'>Weight</th>";
            Html += "<th>No.of TEUs</th></tr>";
            Html += "</thead>";
            //LOOP//

            Html += "<tbody>";
            obcartapp.lstcartapp.ToList().ForEach(item =>
            {
                Cargo = item.CargoType == 1 ? "Haz" : "Non Haz";
                Html += "<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'>" + SerialNo++ + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.SBNo + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.ExporterImporterName + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + Cargo + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'></td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.Package + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.Weight + "</td>";
                Html += "<td style='border-top:1px solid #000;'></td></tr>";
            });
            Html += "</tbody>";
            Html += "</table>";
            Html += "</td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-size:9.5pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            Html += "<tr><td colspan='6' width='50%' style='border-bottom: 1px solid #000; padding-bottom:5px;'>The Cargo is insured by the shipper right from the factory to the final destination ncluding the storage period at CFS, Whitefield, Bangalore.</td>";
            Html += "<td colspan='6' width='50%' valign='bottom' style='text-align:right;border-bottom: 1px solid #000; padding-bottom:5px;'><b>Signature of Exporter / CHA</b></td></tr>";
            Html += "<tr><td colspan='12' style='text-align:center;'><b>AT OFFICE</b></td></tr>";
            Html += "<tr><td><span><br/></span></td></tr>";
            Html += "<tr><td colspan='6' width='50%'>Accepted for carting at Godown No. <u>...................</u></td>";
            Html += "<td colspan='6' width='50%' valign='bottom' style='text-align:right;'><b>Shift Incharge</b></td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";

            Html += "<tr><td colspan='12' style='text-align:center;font-size:9.5pt;'><b>AT OFFICE</b></td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='10' style='width:100%; text-align:center; border:1px solid #000; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";
            Html += "<thead>";
            Html += "<tr><th style='border-right:1px solid #000;'>Receipt</th>";
            Html += "<th style='border-right:1px solid #000;'>Cargo</th>";
            Html += "<th style='border-right:1px solid #000;'>Marks & Nos.</th>";
            Html += "<th style='border-right:1px solid #000;'>No.of Units</th>";
            Html += "<th>Remarks</th></tr>";
            Html += "</thead>";
            //LOOP//
            Html += "<tbody>";
            obcartapp.lstcartapp.ToList().ForEach(item =>
            {
                Cargo = item.CargoType == 1 ? "Haz" : "Non Haz";
                Html += "<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'>.............</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + Cargo + "</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>................</td>";
                Html += "<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.Package + "</td>";

                Html += "<td style='border-top:1px solid #000;'>...............</td></tr>";
            });
            Html += "</tbody>";
            Html += "</table>";
            Html += "</td></tr>";

            Html += "<tr><td><span><br/></span></td></tr>";

            Html += "<tr><td colspan='12' style='text-align:right;font-size:9.5pt;'><b>Shed Incharge</b></td></tr>";

            Html += "<tr><td><span><br/></span></td></tr>";

            Html += "<tr><td colspan='12'>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-size:9.5pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            Html += "<tr><td colspan='12' style='text-align:center;'><b>AT OFFICE</b></td></tr>";
            Html += "<tr><td><span><br/></span></td></tr>";
            Html += "<tr><td colspan='12'>Entered in the carting Register at folio No <u>............</u></td></tr>";
            Html += "<tr><td colspan='12' style='text-align:right;'><b>Office Assistant</b></td></tr>";
            Html += "</tbody></table>";
            Html += "</td></tr>";

            Html += "</tbody></table>";



            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/CartingApplication.pdf";
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/CartingApplication.pdf";
        }

        #endregion


        #region CCINEntry secondary user
        public ActionResult CCINEntrySecondaryUser(int Id = 0, int PartyId = 0)
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            WFLD_CCINEntry objCCINEntry = new WFLD_CCINEntry();
            ExportRepository objER = new ExportRepository();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            //   objER.GetCCINShippingLine();
            //   if (objER.DBResponse.Data != null)
            //   {
            //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
            //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            //  }
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

            ObjER.ListOfChaForPageSecondaryuser(Convert.ToString(ObjLogin.EximTraderId), 0);
            if (ObjER.DBResponse.Data != null)
            {
                //IList<CwcExim.Areas.Import.Models.CHAForPage> lstCHA = new List<CwcExim.Areas.Import.Models.CHAForPage>();
                CwcExim.Areas.Import.Models.CHAForPage chapage = new CwcExim.Areas.Import.Models.CHAForPage();
                chapage = (CwcExim.Areas.Import.Models.CHAForPage)ObjER.DBResponse.Data;
              //  var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                //var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            
                  objCCINEntry.CHAName = chapage.CHAName;
                  objCCINEntry.CHAId = chapage.CHAId;

               
               // ViewBag.lstCHA = Jobject["lstCHA"];
                //ViewBag.CHAState = Jobject["State"];
            }
            else
            {
                //ViewBag.lstCHA = null;
            }
            ObjER.GetSBListpagewise("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfSBNo = Jobject["LstSB"];
                ViewBag.StateShipbill = Jobject["State"];
            }
            else
            {
                ViewBag.ListOfSBNo = null;
            }
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}

            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            WFLD_CountryRepository ObjCR = new WFLD_CountryRepository();
            /*ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }*/

            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
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

            

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                WFLD_ExportRepository rep = new WFLD_ExportRepository();
                rep.GetCCINEntryForEdit(Id, PartyId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (WFLD_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }
       

        /*  [HttpGet]
          public JsonResult GetShippingBill()
          {
              WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
              objRepo.GetSBList();
              if (objRepo.DBResponse.Data != null)
              {
                  ViewBag.ListOfSBNo = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
              }

              var jsonResult = Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
              jsonResult.MaxJsonLength = int.MaxValue;
              return jsonResult;
             // return Json(ViewBag.ListOfSBNo, JsonRequestBehavior.AllowGet);
          }*/


       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINEntryForSecondaryUser(WFLD_CCINEntry objCCINEntry)
        {
            ModelState.Remove("CityId");
            ModelState.Remove("SelectCityId");
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository objER = new WFLD_ExportRepository();

                //  objCCINEntry.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                //  string PostPaymentXML = Utility.CreateXML(objCCINEntry.lstPostPaymentChrg);


                // IList<PostPaymentCharge> PostPaymentChargeList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PostPaymentCharge>>(objCCINEntry.PaymentSheetModelJson);
                // string XML = Utility.CreateXML(PostPaymentChargeList);
                // objCCINEntry.PaymentSheetModelJson = XML;
                objER.AddEditCCINEntryForSecondaryUser(objCCINEntry);
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
        public ActionResult ListOfCCINEntryForSecondaryUser(string SearchValue = "", string SearchCCIN = "")
        {

            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> lstCCINEntry = new List<WFLD_CCINEntry>();
            Login objLogin = (Login)Session["LoginUser"];
            objER.GetAllCCINEntryForPageForSecondary(0, objLogin.Uid, SearchValue, SearchCCIN);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<WFLD_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }


        [HttpGet]
        public JsonResult LoadMoreCCINEntryListSecondaryUser(int Page, string SearchValue)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> LstJO = new List<WFLD_CCINEntry>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjCR.GetAllCCINEntryForPageForSecondary(Page, objLogin.Uid, SearchValue);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLD_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCCINEntryForSecondaryUser(int CCINEntryId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (CCINEntryId > 0)
                objER.DeleteCCINEntry(CCINEntryId);
            return Json(objER.DBResponse);
        }
     

       

        #endregion




        #region LeoEntry
        public ActionResult LeoEntry()
        {
           return PartialView();
        }
        public JsonResult SearchMCIN(string SBNo, string SBDATE)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetMCIN(SBNo, SBDATE);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLEO(LEOPage objLEOPage)
        {
            
           
                WFLD_ExportRepository objER = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();





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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();





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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<LEOPage> lstLEOPage = new List<LEOPage>();
            objER.GetAllLEOEntryBYSBMCIN(SearchValue);
            if (objER.DBResponse.Data != null)
                lstLEOPage = (List<LEOPage>)objER.DBResponse.Data;
            return PartialView("ListOfLEO",lstLEOPage);
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

            WFLD_CountryRepository ObjCR = new WFLD_CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
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

            WFLD_ExportRepository objRepository = new WFLD_ExportRepository();
            objRepository.GetAllCCINEntry("A");
            if (objRepository.DBResponse.Data != null)
            {
                ViewBag.ListOfCCINNo = (List<WFLD_CCINEntry>)objRepository.DBResponse.Data;
            }

            WFLD_CCINEntry objCCINEntry = new WFLD_CCINEntry();

            if (Id == 0)
            {
                objCCINEntry.CCINDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                WFLD_ExportRepository rep = new WFLD_ExportRepository();
                rep.GetCCINEntryById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (WFLD_CCINEntry)rep.DBResponse.Data;
                    objCCINEntry.SelectStateId = objCCINEntry.StateId;
                    objCCINEntry.SelectCityId = objCCINEntry.CityId;
                }
            }

            return PartialView(objCCINEntry);
        }

        [HttpGet]
        public ActionResult GetCCINEntryApprovalDetails(int CCINEntryId)
        {
            WFLD_CCINEntry objCCINEntry = new WFLD_CCINEntry();
            if (CCINEntryId > 0)
            {
                WFLD_ExportRepository rep = new WFLD_ExportRepository();
                rep.GetCCINEntryById(CCINEntryId);
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (WFLD_CCINEntry)rep.DBResponse.Data;
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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.AddEditCCINEntryApproval(Id, IsApproved);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfCCINEntryApproval()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> lstCCINEntry = new List<WFLD_CCINEntry>();
            objER.GetAllCCINEntryApprovalForPage(0);
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<WFLD_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }

        [HttpGet]
        public JsonResult LoadMoreCCINEntryApprovalList(int Page)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> LstJO = new List<WFLD_CCINEntry>();
            ObjCR.GetAllCCINEntryApprovalForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLD_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region CCIN Invoice
        [HttpGet]
        public ActionResult CreateCCINPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            // objExp.GetPaymentPartyCCINInvoice();

            // if (objExp.DBResponse.Status > 0)
            // ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            // else
            ViewBag.PaymentParty = null;

            //  objExp.GetPaymentPayerCCINInvoice();

            // if (objExp.DBResponse.Status > 0)
            //  ViewBag.PaymentPayer= JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            ViewBag.PaymentPayer = null;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetCCINForParty()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetPaymentforPartyCCINInvoice("");
            return Json(objExp.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCCINForPartySearch(string PartyCode)
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetPaymentforPartyCCINInvoice(PartyCode);
            return Json(objExp.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCCINForPayee(int PartyId)
        {

            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetPaymentPayerCCINInvoice();
            return Json(objExp.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetCCINForInvoice(int PartyId)
        {
            List<CCINForInvoice> lstCCINNo = new List<CCINForInvoice>();
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetCCINForInvoice(PartyId);
            if (objExp.DBResponse.Data != null)
                lstCCINNo = (List<CCINForInvoice>)objExp.DBResponse.Data;
            return Json(objExp.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCCINForCHA(string PartyCode, int Page)
        {
            List<WFLD_CHAList> lstCCINNo = new List<WFLD_CHAList>();
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetCCINForCHAList(PartyCode, Page);
            //if (objExp.DBResponse.Data != null)
            //    lstCCINNo = (List<WFLD_CHAList>)objExp.DBResponse.Data;
            return Json(objExp.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCCINPaymentSheet(FormCollection objForm)
        {
            string obj = objForm["PaymentSheetModelJson"].ToString().Substring(0, objForm["PaymentSheetModelJson"].ToString().Length - 1);
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //    var invoiceData = JsonConvert.DeserializeObject<WFLD_CCINInvoice>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceData = JsonConvert.DeserializeObject<WFLD_CCINInvoice>(obj);

                string ChargesXML = "";



                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditCCINInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "CCIN");

                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
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
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
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
                WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAllCartingApp(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid);
            if (objER.DBResponse.Data != null)
                lstCartingApp = (List<CartingList>)objER.DBResponse.Data;
            return PartialView(lstCartingApp);
        }
        [HttpGet]
        public ActionResult ViewCartingApp(int CartingAppId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (CartingAppId > 0)
                objER.DeleteCartingApp(CartingAppId);
            return Json(objER.DBResponse);
        }
        [HttpGet]
        public ActionResult EditCartingApp(int CartingAppId)
        {
            CartingApplication objCA = new CartingApplication();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
                WFLD_ExportRepository objER = new WFLD_ExportRepository();
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


            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            WFLD_CartingRegister objCR = new WFLD_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd/MM/yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<WFLDApplicationNoDet>)objER.DBResponse.Data;


            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.CHAList = (List<CHA>)objER.DBResponse.Data;
            }

            ExportRepository objER1 = new ExportRepository();
            objER1.GetCCINShippingLine();
            if (objER1.DBResponse.Data != null)
            {
                ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER1.DBResponse.Data).ToString();
                ViewBag.ListOfShippingLine = objER1.DBResponse.Data;
            }

            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
                ViewBag.ExporterList = (List<Exporter>)objER.DBResponse.Data;

            objER.GetAllCommodityForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            objER.GetTimeCarting();
            WFLD_CartingRegister objCRTime = new WFLD_CartingRegister();
            if (objER.DBResponse.Data != null)
            {

                objCRTime = (WFLD_CartingRegister)objER.DBResponse.Data;
                string CartingTime = objCRTime.Time;
                string[] ExitTimeArray = CartingTime.Split(' ');

                ViewBag.strTime = objCRTime.Time;

            }


            return PartialView("CreateCartingRegister", objCR);
        }
        [HttpGet]
        public ActionResult ListCartingRegister(string SearchValue = "")
        {
            List<WFLD_CartingRegister> objCR = new List<WFLD_CartingRegister>();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAllRegisterDetails(((Login)(Session["LoginUser"])).Uid, SearchValue);
            if (objER.DBResponse.Data != null)
                objCR = (List<WFLD_CartingRegister>)objER.DBResponse.Data;


            return PartialView("ListCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditListCartingRegister(string SearchValue = "")
        {
            List<WFLD_CartingRegister> objCR = new List<WFLD_CartingRegister>();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAllRegisterDetails(((Login)(Session["LoginUser"])).Uid, SearchValue);
            if (objER.DBResponse.Data != null)
                objCR = (List<WFLD_CartingRegister>)objER.DBResponse.Data;

            return PartialView("EditListCartingRegister", objCR);
        }



        [HttpGet]
        public ActionResult ViewCartingRegister(int CartingRegisterId, string Mode = "")
        {
            WFLD_CartingRegister objCR = new WFLD_CartingRegister();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "view");


            // string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

            // ObjEntryGate.EntryDateTime = strDtae;
            //ObjEntryGate.ReferenceDate = ReferenceDate;

            if (objER.DBResponse.Data != null)
                objCR = (WFLD_CartingRegister)objER.DBResponse.Data;
            objCR.ViewMode = Mode;

            string strDateTime = objCR.Time;
            string[] dateTimeArray = strDateTime.Split(' ');

            string strTime = dateTimeArray[0];
            var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
            ViewBag.strTime = convertTime;
            return PartialView("ViewCartingRegister", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegisterMenu()
        {
            //User RightsList---------------------------------------------------------------------------------------
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            //------------------------------------------------------------------------------------------------------


            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            WFLD_CartingRegister objCR = new WFLD_CartingRegister();
            objCR.RegisterDate = DateTime.Now.ToString("dd/MM/yyyy");
            objER.GetAllApplicationNo();
            if (objER.DBResponse.Data != null)
                objCR.lstAppNo = (List<WFLDApplicationNoDet>)objER.DBResponse.Data;


            objER.ListOfCHA();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.CHAList = (List<CHA>)objER.DBResponse.Data;
            }

            ExportRepository objER1 = new ExportRepository();
            objER1.GetCCINShippingLine();
            if (objER1.DBResponse.Data != null)
            {
                ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER1.DBResponse.Data).ToString();
                ViewBag.ListOfShippingLine = objER1.DBResponse.Data;
            }

            List<Godown> lstGodown = new List<Godown>();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);


            objER.ListOfExporter();
            if (objER.DBResponse.Data != null)
                ViewBag.ExporterList = (List<Exporter>)objER.DBResponse.Data;

            objER.GetAllCommodityForPage("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }


            return PartialView("EditCartingRegisterMenu", objCR);
        }

        [HttpGet]
        public ActionResult EditCartingRegister(int CartingRegisterId)
        {

            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLD_CartingRegister ObjCartingReg = new WFLD_CartingRegister();
            GodownRepository ObjGR = new GodownRepository();
            if (CartingRegisterId > 0)
            {
                ObjER.GetRegisterDetails(CartingRegisterId, ((Login)(Session["LoginUser"])).Uid, "edit");
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCartingReg = (WFLD_CartingRegister)ObjER.DBResponse.Data;
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
            string strDateTime = ObjCartingReg.Time;
            string[] dateTimeArray = strDateTime.Split(' ');

            string strTime = dateTimeArray[0];
            var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mmtt");

            // string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

            // ObjEntryGate.EntryDateTime = strDtae;
            //ObjEntryGate.ReferenceDate = ReferenceDate;
            ViewBag.strTime = convertTime;



            //***************************************************************************
            return PartialView("EditCartingRegister", ObjCartingReg);
        }
        public JsonResult GetApplicationDetForRegister(int CartingAppId)
        {
            WFLD_CartingRegister objCR = new WFLD_CartingRegister();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAppDetForCartingRegister(CartingAppId, Convert.ToInt32(Session["BranchId"]));
            if (objER.DBResponse.Data != null)
                objCR = (WFLD_CartingRegister)objER.DBResponse.Data;
            return Json(objCR, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCartingRegister(WFLD_CartingRegister objCR)
        {
            /*
             Carting Type:  1.Manual    2.Mechanical
             Commodity Type:    1.General   2.Heavy/Scrape
             */
            if (ModelState.IsValid)
            {
                objCR.ApplicationDate = Convert.ToDateTime(objCR.ApplicationDate).ToString("dd/MM/yyyy");
                objCR.RegisterDate = Convert.ToDateTime(objCR.RegisterDate).ToString("dd/MM/yyyy");
                //List<int> lstLocation = new List<int>();
                IList<WFLD_CartingRegisterDtl> LstCartingDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLD_CartingRegisterDtl>>(objCR.XMLData);

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
                string SysTime = Request.Form["time"];


                string Entrytime = SysTime.Replace("PM", " PM").Replace("AM", " AM");

                string strSysDateTime = objCR.RegisterDate + " " + Entrytime;
                objCR.RegisterDate = strSysDateTime;

                string XML = Utility.CreateXML(LstCartingDtl);
                WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (CartingRegisterId > 0)
                objER.DeleteCartingRegister(CartingRegisterId);
            return Json(objER.DBResponse);
        }


        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.WFLDGodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.WFLDGodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddShortCargoDetail(WFLD_ShortCargoDetails objSC, int CartingRegisterId, int CartingRegisterDtlId, string ShippingBillNo)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_ShortCargoDetails> lstShortCargoDetails = new List<WFLD_ShortCargoDetails>();
            objSC.CartingDate = Convert.ToDateTime(objSC.CartingDate).ToString("yyyy-MM-dd HH:mm");
            lstShortCargoDetails.Add(objSC);
            objER.AddShortCargoDetail(Utility.CreateXML(lstShortCargoDetails), CartingRegisterId, CartingRegisterDtlId, ShippingBillNo);
            return Json(objER.DBResponse);
        }

        #endregion  

        #region Stuffing Request

        [HttpGet]
        public ActionResult CreateStuffingRequest()
        {
            WFLD_StuffingRequest ObjSR = new WFLD_StuffingRequest();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();

            /*  ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
              if (ObjER.DBResponse.Data != null)
              {
                  ViewBag.CartingRegNoList = new SelectList((List<WFLD_StuffingRequest>)ObjER.DBResponse.Data, "ShortCargoDtlId", "CartingRegisterNo");
              }
              else
              {
                  ViewBag.CartingRegNoList = null;
              }*/
            ObjER.ListOfCHA();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            }
            else
            {
                ViewBag.CHAList = null;
            }
            //ObjER.GetShippingLine();
            //if (ObjER.DBResponse.Data != null)
            //{ ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName"); }
            //else
            //{ ViewBag.ShippingLineList = null; }

            //ObjER.GetForwarder();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ForwarderList = new SelectList((List<CwcExim.Areas.Export.Models.ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            //else
            //    ViewBag.ForwarderList = null;

            // ObjER.GetAllContainerNo();
            //Add Chiranjit 
            ObjER.GetAllContainerNoForStuffingRequest();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<WFLD_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
                ViewBag.ContainerList = null;

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
            WFLD_StuffingRequest ObjSRtime = new WFLD_StuffingRequest();
            ObjER.GetTime();
            if (ObjER.DBResponse.Data != null)
            {
                ObjSRtime = (WFLD_StuffingRequest)ObjER.DBResponse.Data;
                string StuffingTime = ObjSRtime.Time;
                string[] ExitTimeArray = StuffingTime.Split(' ');

                ViewBag.strTime = ObjSRtime.Time;

            }
            //ObjER.ListOfForeignLiner();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.ForeignLinerList = (List<string>)ObjER.DBResponse.Data;
            //else
            //    ViewBag.ForeignLinerList = null;
            ObjSR.RequestDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView("CreateStuffingRequest", ObjSR);
        }

        //Susmita-17-06-23
        [HttpGet]
        public JsonResult ShippingDetail()
        {            
            
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetShippingLine();
            return Json(objRepo.DBResponse,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ForwarderDetail()
        {

            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetForwarder();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ForeignerListDetail()
        {

            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfForeignLiner();
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

       //End Susmita

        [HttpGet]
        public JsonResult GetCartingNo()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
            //  WFLD_ExportRepository objRepo = new WFLD_ExportRepository();



            // if (objRepo.DBResponse.Data != null)
            //{
            //   ViewBag.ListOfCHA = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            // }
            return Json(ViewBag.CartingRegNoList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCartingNoSearch(string SBNo)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetCartRegNoForStuffingReqSearch(((Login)(Session["LoginUser"])).Uid, SBNo);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
           
            return Json(ViewBag.CartingRegNoList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackUQCForStuffingReq()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetPackUQCForStuffingReq();
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.packuqcList = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.packuqcList = null;
            }
            //  WFLD_ExportRepository objRepo = new WFLD_ExportRepository();



            // if (objRepo.DBResponse.Data != null)
            //{
            //   ViewBag.ListOfCHA = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            // }
            return Json(ViewBag.packuqcList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ShippinglineDtlAfterEmptyCont(string CFSCode)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.ShippinglineDtlAfterEmptyCont(CFSCode);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStuffingReq(WFLD_StuffingRequest ObjStuffing)
        {
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                IList<WFLD_StuffingRequestDtl> LstStuffing = JsonConvert.DeserializeObject<IList<WFLD_StuffingRequestDtl>>(ObjStuffing.StuffingXML);
                IList<WFLD_StuffingReqContainerDtl> LstStuffConatiner = JsonConvert.DeserializeObject<IList<WFLD_StuffingReqContainerDtl>>(ObjStuffing.ContainerXML);
                var flag = 0;
                var varLstStuffing = from line in LstStuffing
                                     where line.StuffingReqId == 0
                                     group line by line.CartingRegisterNo into g
                                     select new WFLD_StuffingRequestDtl
                                     {

                                         StuffQty = g.Sum(pc => pc.StuffQty),
                                         RQty = g.First().RQty
                                     };
                foreach (var idata in varLstStuffing)
                {
                    if (idata.RQty < idata.StuffQty)
                    {
                        flag = 1;
                    }
                }

                if (flag == 0)
                {
                    string StuffingXML = Utility.CreateXML(LstStuffing);
                    string StuffingContrXML = Utility.CreateXML(LstStuffConatiner);
                    ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;

                    string SysTime = Request.Form["time"];


                    string Entrytime = SysTime.Replace("PM", " PM").Replace("AM", " AM");

                    string strSysDateTime = ObjStuffing.RequestDate + " " + Entrytime;
                    ObjStuffing.RequestDate = strSysDateTime;





                    string strEntryDateTime = ObjStuffing.RequestDate + " " + Entrytime;
                    //if (SysTime != null && SysTime!="")
                    //{
                    //    string[] SplitSysTime = SysTime.Split(':');
                    //    int i = SplitSysTime[2].Length;
                    //    string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                    //    string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                    //    SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                    //    SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    //    string strSysDateTime = ObjStuffing.RequestDate + " " + SysTime;
                    //    ObjStuffing.RequestDate = strSysDateTime;

                    //}


                    ObjER.AddEditStuffingRequest(ObjStuffing, StuffingXML, StuffingContrXML);
                    ModelState.Clear();
                    return Json(ObjER.DBResponse);

                }
                else
                {
                    var ErrorMessage = string.Join(",", "Stuff Qty Should be less than or equal Remaining Qty");
                    var Err = new { Status = 0, Message = ErrorMessage };
                    return Json(Err);
                }


            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditStuffingRequest(int StuffinfgReqId, string SearchValue = "")
        {
            WFLD_StuffingRequest ObjStuffing = new WFLD_StuffingRequest();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();




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
                ViewBag.ForwarderList = new SelectList((List<CwcExim.Areas.Export.Models.ForwarderList>)ObjER.DBResponse.Data, "ForwarderId", "Forwarder");
            else
                ViewBag.ForwarderList = null;
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
                ObjER.WFLD_GetStuffingRequest(0,StuffinfgReqId, 0, 0, SearchValue);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (WFLD_StuffingRequest)ObjER.DBResponse.Data;
                }

                string strDateTime = ObjStuffing.Requesttime;
                string[] dateTimeArray = strDateTime.Split(' ');

                string strTime = dateTimeArray[0];
                var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mmtt");

                // string ReferenceDate = strReferenceDate;//DateReferenceDate[0];

                // ObjEntryGate.EntryDateTime = strDtae;
                //ObjEntryGate.ReferenceDate = ReferenceDate;
                ViewBag.strTime = convertTime;


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

                ObjER.GetAllContainerNoForStuffingRequest();
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.ContainerList = new SelectList((List<WFLD_StuffingReqContainerDtl>)ObjER.DBResponse.Data, "CFSCode", "ContainerNo");
                    ViewBag.ContainerListJson = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                }
                else
                    ViewBag.ContainerList = null;


                ObjER.GetCartRegNoForStuffingReq(((Login)(Session["LoginUser"])).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.CartingRegNoList = new SelectList((List<WFLD_StuffingRequest>)ObjER.DBResponse.Data, "ShortCargoDtlId", "CartingRegisterNo");
                }
                else
                {
                    ViewBag.CartingRegNoList = null;
                }
                ObjER.ListOfForeignLiner();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.ForeignLinerList = (List<string>)ObjER.DBResponse.Data;
                else
                    ViewBag.ForeignLinerList = null;


            }
            return PartialView("EditStuffingRequest", ObjStuffing);
        }

        [HttpGet]
        public ActionResult ViewStuffingRequest(int StuffinfgReqId, string SearchValue = "")
        {
            WFLD_StuffingRequest ObjStuffing = new WFLD_StuffingRequest();
            if (StuffinfgReqId > 0)
            {
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                ObjER.WFLD_GetStuffingRequest(0,StuffinfgReqId, 0, 0, SearchValue);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (WFLD_StuffingRequest)ObjER.DBResponse.Data;
                    string strDateTime = ObjStuffing.Requesttime;
                    string[] dateTimeArray = strDateTime.Split(' ');

                    string strTime = dateTimeArray[0];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");


                    ViewBag.strTime = convertTime;
                }
            }
            return PartialView("ViewStuffingRequest", ObjStuffing);
        }

        [HttpPost]
        public JsonResult DeleteStuffingRequest(int StuffinfgReqId)
        {
            if (StuffinfgReqId > 0)
            {
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                ObjER.DeleteStuffingRequest(StuffinfgReqId);
                return Json(ObjER.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetCartRegDetForStuffingReq(int ShortCargoDtlId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            if (ShortCargoDtlId > 0)
            {
                objER.Wfld_GetCartRegDetForStuffingReq(ShortCargoDtlId);
            }
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStuffingReqList(string SearchValue = "")
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            List<WFLD_StuffingRequest> LstStuffing = new List<WFLD_StuffingRequest>();
            ObjER.GetAllStuffingRequest(0, ((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return PartialView("StuffingRequestList", LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadMoreStuffingReqList(int Page, string SearchValue="")
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            List<WFLD_StuffingRequest> LstStuffing = new List<WFLD_StuffingRequest>();
            ObjER.GetAllStuffingRequest(Page,((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_StuffingRequest>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerDet(string CFSCode)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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

        [HttpGet]
        public JsonResult GetFinalDestination(string CustodianName = "")
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.ListOfFinalDestination(CustodianName);
            List<WFLD_FinalDestination> lstFinalDestination = new List<WFLD_FinalDestination>();
            if (ObjER.DBResponse.Data != null)
            {
                lstFinalDestination = (List<WFLD_FinalDestination>)ObjER.DBResponse.Data;
            }

            return Json(lstFinalDestination, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateContainerStuffing()
        {
            //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLD_ContainerStuffing ObjCS = new WFLD_ContainerStuffing();
            ObjCS.StuffingDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            //ObjER.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.LstRequestNo = ObjER.DBResponse.Data;
            //}
            //else
            //{
            //    ViewBag.LstRequestNo = null;
            //}

            //ObjER.ListOfCHA();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.CHAList = new SelectList((List<CHA>)ObjER.DBResponse.Data, "CHAEximTraderId", "CHAName");
            //}
            //else
            //{
            //    ViewBag.CHAList = null;
            //}
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
            //ObjER.ListOfPOD();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            //}
            //else
            //{
            //    ViewBag.ListOfPOD = null;
            //}

            return PartialView(ObjCS);
        }

        [HttpGet]
        public JsonResult GetReqNoForContainerStuffing()
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.GetReqNoForContainerStuffing(((Login)Session["LoginUser"]).Uid);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        //--------------------------------------------------        

        [HttpGet]
        public JsonResult LoadTraderList(string PartyCode, string TFlag, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfTrader(PartyCode, TFlag, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByTraderCode(string PartyCode, string TFlag, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfTrader(PartyCode, TFlag, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        //-------------------------------------------------



        public JsonResult GetPortOfDestination()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.ListOfPortOfDestinationStuffing();
            List<Port> lstPort = new List<Port>();
            if (ObjER.DBResponse.Data != null)
            {
                lstPort = (List<Port>)ObjER.DBResponse.Data;
            }

            return Json(lstPort, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountryByPODID(int Portid)
        {
            WFLD_CountryRepository ObjCR = new WFLD_CountryRepository();
            ObjCR.GetCountryByDestinationID(Portid);
            Country lstCountry = new Country();
            if (ObjCR.DBResponse.Data != null)
            {
                lstCountry = (Country)ObjCR.DBResponse.Data;
            }
            return Json(lstCountry, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRequestNo()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();

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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();


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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();

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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();

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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerDetOfStuffingReq(int StuffingReqId)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetContainerDetForStuffing(StuffingReqId);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerStuffingList(string SearchValue = "")
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            List<WFLD_ContainerStuffing> LstStuffing = new List<WFLD_ContainerStuffing>();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetAllContainerStuffing(0,((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView("/Areas/Export/Views/WFLD_CWCExport/ContainerStuffingList.cshtml", LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadMoreContDetOfStuffReqList(int Page, string SearchValue = "")
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            List<WFLD_ContainerStuffing> LstStuffing = new List<WFLD_ContainerStuffing>();
            ObjER.GetAllContainerStuffing(Page,((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewContainerStuffing(int ContainerStuffingId)
        {
            WFLD_ContainerStuffing ObjStuffing = new WFLD_ContainerStuffing();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(0,ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                    ObjStuffing = (WFLD_ContainerStuffing)ObjER.DBResponse.Data;
            }
            return PartialView("ViewContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public ActionResult EditContainerStuffing(int ContainerStuffingId)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLD_ContainerStuffing ObjStuffing = new WFLD_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffing(0,ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (WFLD_ContainerStuffing)ObjER.DBResponse.Data;
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
                //ObjER.ListOfPOD();
                //if (ObjER.DBResponse.Data != null)
                //{
                //    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjER.DBResponse.Data, "PortId", "PortName");
                //}
                //else
                //{
                //    ViewBag.ListOfPOD = null;
                //}
            }
            return PartialView("EditContainerStuffing", ObjStuffing);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int StuffingReqId)
        {
            List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl>();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetContainerNoByStuffingReq(StuffingReqId);
            //if (ObjER.DBResponse.Data != null)
            // {
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
            //}
            // LstStuffing = (List<ContainerStuffingDtl>)ObjER.DBResponse.Data;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingDet(WFLD_ContainerStuffing ObjStuffing)
        {

            ModelState.Remove("GENSPPartyCode");
            ModelState.Remove("GREPartyCode");
            ModelState.Remove("INSPartyCode");
            ModelState.Remove("HandalingPartyCode");
            ModelState.Remove("SQM");

            if (ModelState.IsValid)
            {
                string ContainerStuffingXML = "";
                //string SCMTRXML = "";
                if (ObjStuffing.StuffingXML != null)
                {
                    List<WFLD_ContainerStuffingDtlBase> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_ContainerStuffingDtlBase>>(ObjStuffing.StuffingXML);
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
                //if (ObjStuffing.SCMTRXML != null)
                //{
                //    List<WFLD_ContainerStuffingSCMTR> LstSCMTR = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_ContainerStuffingSCMTR>>(ObjStuffing.SCMTRXML);
                //    SCMTRXML = Utility.CreateXML(LstSCMTR);
                //}
                //string GREOperationCFSCodeWiseAmtXML = "";
                //if (ObjStuffing.GREOperationCFSCodeWiseAmt != null)
                //{
                //    List<GREOperationCFSCodeWiseAmt> LstStuffingGRE1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREOperationCFSCodeWiseAmt>>(ObjStuffing.GREOperationCFSCodeWiseAmt.ToString());
                //    GREOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGRE1);
                //}

                //string GREContainerWiseAmtXML = "";
                //if (ObjStuffing.GREContainerWiseAmt != null)
                //{
                //    List<GREContainerWiseAmt> LstStuffingGRE2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GREContainerWiseAmt>>(ObjStuffing.GREContainerWiseAmt.ToString());
                //    GREContainerWiseAmtXML = Utility.CreateXML(LstStuffingGRE2);
                //}

                //string INSOperationCFSCodeWiseAmtLstXML = "";
                //if (ObjStuffing.INSOperationCFSCodeWiseAmt != null)
                //{
                //    List<INSOperationCFSCodeWiseAmt> LstStuffingINS1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSOperationCFSCodeWiseAmt>>(ObjStuffing.INSOperationCFSCodeWiseAmt.ToString());
                //    INSOperationCFSCodeWiseAmtLstXML = Utility.CreateXML(LstStuffingINS1);
                //}

                //string INSContainerWiseAmtXML = "";
                //if (ObjStuffing.INSContainerWiseAmt != null)
                //{
                //    List<INSContainerWiseAmt> LstStuffingINS2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<INSContainerWiseAmt>>(ObjStuffing.INSContainerWiseAmt.ToString());
                //    INSContainerWiseAmtXML = Utility.CreateXML(LstStuffingINS2);
                //}

                //string STOContainerWiseAmtXML = "";
                //if (ObjStuffing.STOinvoicecargodtl != null)
                //{
                //    List<STOinvoicecargodtl> LstStuffingSTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOinvoicecargodtl>>(ObjStuffing.STOinvoicecargodtl.ToString());
                //    STOContainerWiseAmtXML = Utility.CreateXML(LstStuffingSTO2);
                //}
                //string STOOperationCFSCodeWiseAmtXML = "";
                //if (ObjStuffing.STOOperationCFSCodeWiseAmt != null)
                //{
                //    List<STOOperationCFSCodeWiseAmt> LstStuffingSTO1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<STOOperationCFSCodeWiseAmt>>(ObjStuffing.STOOperationCFSCodeWiseAmt.ToString());
                //    STOOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingSTO1);
                //}

                //string HNDOperationCFSCodeWiseAmtXML = "";
                //if (ObjStuffing.HNDOperationCFSCodeWiseAmt != null)
                //{
                //    List<HNDOperationCFSCodeWiseAmt> LstStuffingHND = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HNDOperationCFSCodeWiseAmt>>(ObjStuffing.HNDOperationCFSCodeWiseAmt.ToString());
                //    HNDOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingHND);
                //}

                //string GENSPOperationCFSCodeWiseAmtXML = "";
                //if (ObjStuffing.GENSPOperationCFSCodeWiseAmt != null)
                //{
                //    List<GENSPOperationCFSCodeWiseAmt> LstStuffingGENSP = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GENSPOperationCFSCodeWiseAmt>>(ObjStuffing.GENSPOperationCFSCodeWiseAmt.ToString());
                //    GENSPOperationCFSCodeWiseAmtXML = Utility.CreateXML(LstStuffingGENSP);
                //}

                //string ShippingBillAmtXML = "";
                //if (ObjStuffing.WFLD_ShippingBillAmt != null)
                //{
                //    List<WFLD_ShippingBillNo> LstShippingBill = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_ShippingBillNo>>(ObjStuffing.WFLD_ShippingBillAmt.ToString());
                //    ShippingBillAmtXML = Utility.CreateXML(LstShippingBill);
                //}

                //string ShippingBillAmtGenXML = "";
                //if (ObjStuffing.WFLD_ShippingBillAmtGen != null)
                //{
                //    List<WFLD_ShippingBillNoGen> LstShippingBillGen = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_ShippingBillNoGen>>(ObjStuffing.WFLD_ShippingBillAmtGen.ToString());
                //    ShippingBillAmtGenXML = Utility.CreateXML(LstShippingBillGen);
                //}
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                ObjStuffing.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML);
                // ObjER.AddEditContainerStuffing(ObjStuffing, ContainerStuffingXML, GREOperationCFSCodeWiseAmtXML, GREContainerWiseAmtXML,
                // INSOperationCFSCodeWiseAmtLstXML, INSContainerWiseAmtXML, STOContainerWiseAmtXML, STOOperationCFSCodeWiseAmtXML, HNDOperationCFSCodeWiseAmtXML, GENSPOperationCFSCodeWiseAmtXML, ShippingBillAmtXML, ShippingBillAmtGenXML);
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLD_ContainerStuffing ObjStuffing = new WFLD_ContainerStuffing();
            ObjER.GetContainerStuffForPrint(ContainerStuffingId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (WFLD_ContainerStuffing)ObjER.DBResponse.Data;
                string Path = GeneratePdfForContainerStuff(ObjStuffing, ContainerStuffingId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        public string GeneratePdfForContainerStuff(WFLD_ContainerStuffing ObjStuffing, int ContainerStuffingId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
            string Html = "";string movement = "";
            string ShippingBillNo = "", ShippingDate = "", StuffWeight = "", Exporter = "", ShippingLine = "", Fob = "", CHA = "",
            StuffQuantity = "", SLNo = "", CFSCode = "", ContainerNo = "", CargoType = "", CustomSeal = "", Commodity = "", EntryNo = "", InDate = "", Area = "", PortName = "", PortDestination = "", Remarks = "", chargetype = "", total = "";
            if (ObjStuffing.TransportMode == 1)
                {
             movement  = "CWC Movement";
                 }
           else if (ObjStuffing.TransportMode == 2)
                {
                movement = "Self Movement";
                }
            else
            {
                movement = "CWC Movement";
            }
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
                //Block Chiranjit 2129 2137
                //ObjStuffing.LstppgStuffingDtl.ToList().ForEach(item =>
                //{
                //    //SLNo = SLNo + SerialNo + "<br/>";
                //    // CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                //    // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));
                //    CustomSeal = (CustomSeal == "" ? (item.CustomSeal) : CustomSeal);// == item.CustomSeal ? CustomSeal : (CustomSeal + "<br/>" + item.CustomSeal));
                //    Commodity = (Commodity == "" ? (item.CommodityName) : Commodity);// == item.CommodityName ? Commodity : (Commodity + "<br/>" + item.CommodityName));
                //    //SerialNo++;
                //});
                //SLNo.Remove(SLNo.Length - 1);
                //Block Chiranjit 2140 2151
                ObjStuffing.LstppgShipDtl.ToList().ForEach(item =>
                {
                    //SLNo = SLNo + SerialNo + "<br/>";
                    // CFSCode = (CFSCode == "" ? (item.CFSCode) : CFSCode == item.CFSCode ? CFSCode : (CFSCode + "<br/>" + item.CFSCode));
                    // ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo +'-'+item.CargoType : (ContainerNo + '-' + item.CargoType + "<br/>" + item.ContainerNo + '-' + item.CargoType));
                    //ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : ContainerNo == item.ContainerNo ? ContainerNo : (ContainerNo + "<br/>" + item.ContainerNo));

                    CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));

                    //SerialNo++;
                });


                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>";

                Html += "<thead>";

                Html += "<tr><td colspan='4'>";
                Html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
                Html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
                Html += "<td width='100%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS WHITEFIELD BANGALORE</span><br/><label style='font-size: 7pt; font-weight:bold;'>CONTAINER STUFFING SHEET</label><br/><label style='font-size: 7pt;'> <b>Shed No :</b> " + ObjStuffing.GodownName + "</label></td>";
                Html += "<td width='20%' align='right' valign='top'>";
                Html += "<table style='width:100%;' cellspacing='0' cellpadding='0' valign='top'><tbody>";
                Html += "<tr><td style='border:1px solid #333;' valign='top'>";
                Html += "<div valign='top' style='padding: 5px 10px; font-size: 7pt; text-align: center;'>F/ICDWFLD/09</div>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";
                Html += "</thead>";

                Html += "<tbody>";

                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td><b>CWC PAN NO:</b> AAACC1206D</td></tr>  <tr><td><span><br/></span></td></tr> <tr><td><b>CWC STX REG NO:</b> AAACC1206DST005</td></tr>  </tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <th colspan='1' width='10%' style='padding:3px;text-align:left;'>Stuff Req No :</th><td colspan='10' width='15%' style='padding:3px;text-align:left;'>" + ObjStuffing.StuffingNo + "</td><th colspan='10' width='8%'></th><th colspan='10' width='40%' style='padding:3px;text-align:right;'>Stuffing Date :</th><td colspan='1' width='8%' style='padding:3px;text-align:right;'>" + ObjStuffing.StuffingDate + "</td></tr></tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Container No. :</b> <u>" + ContainerNo + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>ICD Code No. :</b> <u>" + CFSCode + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Cont Type :</b> <u>" + CargoType + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Forwarder Name :</b> <u>" + ObjStuffing.ForwarderName + "</u></td> </tr>  </tbody></table></td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>POL :</b> <u>" + PortName + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Port Of Destination :</b> <u>" + ObjStuffing.POD + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td> <td colspan='3' width='25%' style='margin:0 0 10px;'></td>  </tr></tbody></table> </td></tr>";
                //Html += "<tr><td colspan='4'><table style='width:100%;font-size:8.5pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Sla Seal No :</b> <u>" + ObjStuffing.ShippingLineNo + "</u></td>  <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Custom Seal No</b> <u>" + CustomSeal + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px;'><b>Size</b> <u>" + ObjStuffing.Size + "</u></td> <td colspan='3' width='25%' style='margin:0 0 10px; text-align: right;'><b>Main Line </b> <u>" + ShippingLine + "</u></td>  </tr></tbody></table> </td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='5' style='border:1px solid #000;border-bottom:0;width:100%;font-size:6pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:10%;'>Container No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>ICD Code No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:5%;'>Cont Type</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>Forwarder Name</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>POL</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>Port Of Destination</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>Sla Seal No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:7%;'>Custom Seal No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>Equipment Seal Type</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:3%;'>Size</th>";
                Html += "<th style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>Main Line</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                ObjStuffing.ContainerWiseCustomShippingSeal.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:10%;'>" + item.ContainerNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.CFSCode + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:5%;'>" + item.CargoType + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ForwardName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.PortName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.POD + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingLineNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:7%;'>" + item.CustomSeal + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.EquipmentSealType + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;padding:3px;text-align:left;width:3%;'>" + item.Size + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;padding:3px;text-align:left;width:8%;'>" + item.ShippingLine + "</td></tr>";

                });
                //LOOP END
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='5' style='border:1px solid #000;width:100%;font-size:6pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;text-align:left;'><tbody>";
                Html += "<tr><td colspan='6' style='padding:3px;text-align:left; font-size:7pt; width:50%;'><b>Final Destination location :</b>" + ObjStuffing.CustodianCode + "</td><td colspan='6' style='padding:3px;text-align:left; font-size:7pt; width:50%;'><b>Transport Mode :</b>" + movement + "</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table cellspacing='0' cellpadding='5' style='border:1px solid #000;width:100%;font-size:6pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;text-align:left;'>";
                Html += "<thead>";
                Html += "<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>S. No.</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;'>Entry No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;'>In Date</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;'>Sb No</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;'>Sb Date</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>Exporter</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>Comdty</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Pkts</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;width:7%;'>Pack UQC</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;width:7%;'>Gr Wt</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;width:14%;'>FOB</th>";
                Html += "<th style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;width:7%;'>CBM</th>";
     
                Html += "<th style='border-bottom:1px solid #000;width:13%;'>Remarks</th></tr>";
                Html += "</thead>";
                Html += "<tbody>";

                //LOOP START
                ObjStuffing.LstppgStuffingDtl.ToList().ForEach(item =>
                {
                    Html += "<tr><td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + SerialNo++ + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.EntryNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.InDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ShippingBillNo + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ShippingDate + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Exporter + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CommodityName + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.StuffQuantity + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;'>" + item.PackUQC + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;'>" + item.StuffWeight + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;'>" + item.Fob + "</td>";
                    Html += "<td style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:right;'>" + item.Area + "</td>";
    
                    Html += "<td style='border-bottom:1px solid #000;'>" + item.Remarks + "</td></tr>";
                });
                //LOOP END

                Html += "<tr><th colspan='7' style='border-right:1px solid #000;'>Total :</th>";
                Html += "<th style='border-right:1px solid #000;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.StuffQuantity) + "</th>";
                Html += "<th style='border-right:1px solid #000;text-align:right;'></th>";
                Html += "<th style='border-right:1px solid #000;text-align:right;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.StuffWeight) + "</th>";
                Html += "<th style='border-right:1px solid #000;text-align:right;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.Fob) + "</th>";
                Html += "<th style='border-right:1px solid #000;text-align:right;'>" + ObjStuffing.LstppgStuffingDtl.AsEnumerable().Sum(item => item.Area) + "</th>";
                Html += "<th></th></tr>";

                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:20pt;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td colspan='3' width='25%' style='padding:3px;text-align:left;font-size:6pt;' valign='top'>Signature and designation</td>";
                Html += "<td colspan='5' width='41.66666666666667%' style='padding:3px;text-align:left;'>";
                Html += "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>1</td><td colspan='2' width='85%' style='font-size:6pt;'>All activities including those at incomming and in process stages have been satisfactorily completed.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>2</td><td colspan='2' width='85%' style='font-size:6pt;'>All the necessary records have been completed and verified with Date and seal.</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>3</td><td colspan='2' width='85%' style='font-size:6pt;'>Cargo has been stuffed in good condition in terms of quality and quantity</td></tr>";
                Html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>4</td><td colspan='2' width='85%' style='font-size:6pt;'>The number of Packages have been counted by CWC/ Surveyor/ Freight forwarder and have been verified and stuffed accordingly</td></tr>";
                Html += "</tbody></table>";
                Html += "</td>";
                Html += "<td colspan='1' width='8.333333333333333%'></td>";
                Html += "<td colspan='4' width='33.33333333333333%' style='padding:3px;text-align:left;font-size:6pt;' valign='top'>The container is allowed to be moved to gateway ports</td></tr>";
                Html += "</tbody></table>";
                Html += "</td></tr>";

                Html += "<tr><td colspan='4'><span><br/><br/><br/></span></td></tr>";

                Html += "<tr><td colspan='4' style='padding-top:100pt;'><table style='width:100%;font-size:6pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:center;'>Representative/Surveyor <br/> of Shipping Agent/Line</td><td colspan='3' width='25%' style='text-align:center;'>Representative/Surveyor <br/> of H&T contractor</td><td colspan='2' width='‭16.66666666666667‬%' style='text-align:center;'>Shed Asst. <br/> CFS WHITEFIELD</td><td colspan='2' width='‭16.66666666666667‬%' style='text-align:center;'>Shed I/C <br/> CFS WHITEFIELD</td><td colspan='2' width='‭16.66666666666667‬%' style='text-align:center;'>Customs <br/> CFS WHITEFIELD</td></tr></tbody></table></td></tr>";
                Html += "<tr><td colspan='4'><span><br/></span></td></tr>";

                ObjStuffing.LstppgCharge.ToList().ForEach(item =>
                {
                    Html += "<tr><td colspan='4'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='3' width='25%' style='text-align:left;'><label style='width:10%; float: left;'><b>" + item.chargetype + " <span>&nbsp;</span>:</b></label> <label style='width:90%; float: left;'>" + item.total + "</label></td> <td colspan='3' width='25%' style='text-align:center;'>" + item.Invoiceno + "</td> <td colspan='3' width='25%' style='text-align:left;'>" + item.InvoiceDate + "</td><td colspan='3' width='25%' style='text-align:left;'>" + item.eximtraderalias + "</td><td colspan='3' width='25%'></td></tr></tbody></table></td></tr>";
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

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {

                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/ContainerStuffing" + ContainerStuffingId + ".pdf";
        }
        //[NonAction]
        //public string GeneratePdfForContainerStuff(WFLD_ContainerStuffing ObjStuffing, int ContainerStuffingId)
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
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.ListOfGREParty();
            //List<WFLD_ContainerStuffing> objImp = new List<WFLD_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                ((List<WFLD_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GREPartyId = item.GREPartyId, GREPartyCode = item.GREPartyCode });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGroundRentEmpty(String StuffingDate, String ArrayOfContainer, int GREPartyId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<WFLDContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGroundRentEmpty(StuffingDate, ContainerStuffingXML, GREPartyId);
            WFLD_ContainerStuffing objImp = new WFLD_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (WFLD_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfINSParty()
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.ListOfINSParty();
            //List<WFLD_ContainerStuffing> objImp = new List<WFLD_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<WFLD_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<WFLD_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { INSPartyId = item.INSPartyId, INSPartyCode = item.INSPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateINS(String StuffingDate, String ArrayOfContainer, int INSPartyId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<WFLDContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }
            objImport.CalculateINS(StuffingDate, ContainerStuffingXML, INSPartyId);
            WFLD_ContainerStuffing objImp = new WFLD_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (WFLD_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfSTOParty()
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.ListOfSTOParty();
            //List<WFLD_ContainerStuffing> objImp = new List<WFLD_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<WFLD_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<WFLD_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { STOPartyId = item.STOPartyId, STOPartyCode = item.STOPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateSTO(String StuffingDate, String ArrayOfContainer, int STOPartyId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateSTO(StuffingDate, ContainerStuffingXML, STOPartyId);
            WFLD_ContainerStuffing objImp = new WFLD_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (WFLD_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfHandalingParty()
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.ListOfHandalingParty();
            //List<WFLD_ContainerStuffing> objImp = new List<WFLD_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<WFLD_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<WFLD_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { HandalingPartyId = item.HandalingPartyId, HandalingPartyCode = item.HandalingPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateHandaling(String StuffingDate, String Origin, String Via, String ArrayOfContainer, int HandalingPartyId, String CargoType)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<ContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateHandaling(StuffingDate, Origin, Via, ContainerStuffingXML, HandalingPartyId, CargoType);
            WFLD_ContainerStuffing objImp = new WFLD_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (WFLD_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListOfGENSPParty()
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.ListOfGENSPParty();
            //List<WFLD_ContainerStuffing> objImp = new List<WFLD_ContainerStuffing>();
            List<dynamic> objImp = new List<dynamic>();
            if (objImport.DBResponse.Data != null)
                //objImp = (List<WFLD_ContainerStuffing>)objImport.DBResponse.Data;
                ((List<WFLD_ContainerStuffing>)objImport.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp.Add(new { GENSPPartyId = item.GENSPPartyId, GENSPPartyCode = item.GENSPPartyCode });
                });
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateGENSP(String StuffingDate, String ArrayOfContainer, int GENSPPartyId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();

            string ContainerStuffingXML = "";
            if (ArrayOfContainer != null)
            {
                IList<WFLDContainerDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDContainerDtl>>(ArrayOfContainer);
                ContainerStuffingXML = Utility.CreateXML(LstStuffing);
            }

            objImport.CalculateGENSP(StuffingDate, ContainerStuffingXML, GENSPPartyId);
            WFLD_ContainerStuffing objImp = new WFLD_ContainerStuffing();
            if (objImport.DBResponse.Data != null)
                objImp = (WFLD_ContainerStuffing)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Stuffing Amendment
        
        [HttpGet]      
        public ActionResult AmendmentContainerStuffing()
        {
            
            return PartialView("AmendmentContainerStuffing");
        }

        [HttpGet]
        public JsonResult GetStuffingNoForAmendment()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.ListOfStuffingNoForAmendment();           
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetStuffingDetailsForAmendment(int ContainerStuffingId)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLD_ContainerStuffing ObjStuffing = new WFLD_ContainerStuffing();
            if (ContainerStuffingId > 0)
            {
                ObjER.GetContainerStuffingDetails(0,ContainerStuffingId, ((Login)Session["LoginUser"]).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (WFLD_ContainerStuffing)ObjER.DBResponse.Data;
                }
                
            }

            return Json(ObjStuffing, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditAmendmentContainerStuffing(WFLD_ContainerStuffing ObjStuffing)
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
                    List<WFLD_ContainerStuffingDtlBase> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_ContainerStuffingDtlBase>>(ObjStuffing.StuffingXML);
                    ContainerStuffingXML = Utility.CreateXML(LstStuffing);
                }
                
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            List<WFLD_ContainerStuffing> LstStuffing = new List<WFLD_ContainerStuffing>();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(0,((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return PartialView(LstStuffing);
        }

        [HttpGet]
        public JsonResult LoadAmendmentContainerStuffingList(int Page,string SearchValue="")
        {
            List<WFLD_ContainerStuffing> LstStuffing = new List<WFLD_ContainerStuffing>();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetAllAmendmentContainerStuffing(Page, ((Login)Session["LoginUser"]).Uid, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffing = (List<WFLD_ContainerStuffing>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffing, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Container Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
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
            //    ViewBag.LstContainerNo = new SelectList((List<WFLD_ContainerMovement>)ObjIR.DBResponse.Data, "ContainerStuffingId", "Container");
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
            //    ViewBag.LocationNoList = new SelectList((List<WFLD_ContainerMovement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            ObjIR.GetAllInternalMovement();
            List<WFLD_ContainerMovement> LstMovement = new List<WFLD_ContainerMovement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<WFLD_ContainerMovement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public JsonResult GetContainerNoForMovement()
        {
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();

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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();



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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();

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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            WFLD_ContainerMovement ObjInternalMovement = new WFLD_ContainerMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (WFLD_ContainerMovement)ObjIR.DBResponse.Data;
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            WFLD_ContainerMovement ObjInternalMovement = new WFLD_ContainerMovement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (WFLD_ContainerMovement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetConDetails(int ContainerStuffingDtlId, String ContainerNo)
        {
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            ObjIR.GetConDetForMovement(ContainerStuffingDtlId, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                WFLD_ContainerMovement ObjInternalMovement = new WFLD_ContainerMovement();
                ObjInternalMovement = (WFLD_ContainerMovement)ObjIR.DBResponse.Data;
                ViewBag.ShippingBill = new SelectList((List<WFLD_ShippingBill>)ObjInternalMovement.ShipBill, "shippingBillNo", "shippingBillNo");
            }
            else
            {
                ViewBag.ShippingBill = null;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int ContainerStuffingDtlId, int ContainerStuffingId, string ContainerNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int Partyid, string ctype, int portvalue, decimal tareweight, string cargotype, int InvoiceId = 0)
        {

            WFLD_ExportRepository objChrgRepo = new WFLD_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(ContainerStuffingDtlId, ContainerStuffingId, ContainerNo, MovementDate, InvoiceType, DestLocationIdiceId, Partyid, ctype, portvalue, tareweight, cargotype, InvoiceId);

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

            var Output = (WFLD_MovementInvoice)objChrgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new WFLDPostPaymentContainer
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
                WFLDTentativeInvoice.InvoiceObjW = Output;

            if (ctype == "GR")
                WFLDTentativeInvoice.InvoiceObjGR = Output;

            if (ctype == "FMC")
                WFLDTentativeInvoice.InvoiceObjFMC = Output;

            return Json(Output, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult MovementInvoicePrint(string InvoiceNo)
        {
            WFLD_ExportRepository objGPR = new WFLD_ExportRepository();
            if (InvoiceNo == "")
            {
                return Json(new { Status = -1, Message = "Error" });

            }
            else
            {
                objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "EXPMovement");
                WFLD_Movement_Invoice objGP = new WFLD_Movement_Invoice();
                string FilePath = "";
                if (objGPR.DBResponse.Data != null)
                {
                    objGP = (WFLD_Movement_Invoice)objGPR.DBResponse.Data;
                    FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                    return Json(new { Status = 1, Message = FilePath });
                }

            }





            return Json(new { Status = -1, Message = "Error" });



        }
        private string GeneratingPDFInvoiceMovement(WFLD_Movement_Invoice objGP, int InvoiceId)
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
                //WFLD_ImportRepository objChargeMaster = new WFLD_ImportRepository();
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");
                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //   int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<WFLD_Movement_Invoice>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceDataa = JsonConvert.DeserializeObject<WFLD_Movement_Invoice>(objForm["PaymentSheetModelJsonn"].ToString());

                var invoiceDataaa = JsonConvert.DeserializeObject<WFLD_Movement_Invoice>(objForm["PaymentSheetModelJsonnn"].ToString());
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

                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditInvoiceMovement(invoiceData, invoiceDataa, invoiceDataaa, ContainerXML, ContainerXMLL, ContainerXMLLL, ChargesXML, ChargesXMLL, ChargesXMLLL, ContWiseCharg, ContWiseChargg, ContWiseCharggg, OperationCfsCodeWiseAmtXML, OperationCfsCodeWiseAmtXMLL, OperationCfsCodeWiseAmtXMLLL, ChargesBreakupXML, ChargesBreakupXMLL, ChargesBreakupXMLLL, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPMovement");
                WFLD_InvoiceList inv = new WFLD_InvoiceList();
                inv = (WFLD_InvoiceList)objChargeMaster.DBResponse.Data;

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
        public JsonResult AddEditInternalMovement(WFLD_ContainerMovement ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();

            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }


        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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

            objExport.GetTransporterPaymentParty();

            if (objExport.DBResponse.Status > 0)
                ViewBag.TransporterPaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.TransporterPaymentParty = null;

            //objExport.ListOfCHA();
            objExport.GetLoadedContainerCHA();
            if (objExport.DBResponse.Data != null)
                ViewBag.ListOfCHA = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ListOfCHA = null;

            WFLD_CountryRepository ObjCR = new WFLD_CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetRequestNoForLoadedContainer()
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetContainerForLoadedContainerPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoadedContainerPaymentSheet(string DeliveryDate, string InvoiceDate, string InvoiceType, int StuffingReqId, List<WFLD_PaymentSheetContainer> lstPaySheetContainer, int PayeeId,
            int PartyId, int IntercartingApplicable, int ICDDestuffing, int OnWheel, int NoVehicle, decimal Distance, string SEZ, int Shifting, int InvoiceId = 0)
        {
            string ContainerXML = "";
            if (lstPaySheetContainer != null)
            {
                ContainerXML = Utility.CreateXML(lstPaySheetContainer);
            }
            String[] dt = DeliveryDate.Split(' ');
            String[] idt = InvoiceDate.Split(' ');
            WFLD_ExportRepository objChrgRepo = new WFLD_ExportRepository();
            objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, DeliveryDate, idt[0], InvoiceType, ContainerXML, PayeeId, PartyId, IntercartingApplicable, ICDDestuffing, OnWheel, SEZ, Shifting, NoVehicle, Distance, InvoiceId);

            var Output = (WFLD_ExpPaymentSheet)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "EXPLod";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WFLD_ExpContainer();
                obj.CFSCode = item;
                obj.ContainerNo = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerNo;
                obj.ShippingBillNo = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ShippingBillNo;
                obj.ShippingBillDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ShippingBillDate;
                obj.PortDest = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().PortDest;

                obj.Size = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().Size.ToString();
                obj.IsReefer = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Reefer);
                obj.Insured = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Insured);
                obj.RMS = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.RMS);
                obj.CargoType = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Min(x => x.CargoType);
                obj.ArrivalDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ArrivalDate;
                obj.StuffingDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().StuffingDate;
                obj.NoOfPackages = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.NoOfPackages);
                obj.GrossWt = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.GrossWt);
                obj.WtPerUnit = Math.Round(Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.WtPerUnit), 4);
                obj.SpaceOccupied = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.SpaceOccupied);
                obj.SpaceOccupiedUnit = "SQM";
                obj.CIFValue = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.CIFValue);
                obj.FobValue = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Sum(x => x.FobValue);

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
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.RoundUp = 0; //Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetLoadedContainerPaymentSheet(string InvoiceDate, string InvoiceType, int StuffingReqId, List<WFLD_PaymentSheetContainer> lstPaySheetContainer, int PayeeId, int PartyId, int InvoiceId = 0)
        //{
        //    string ContainerXML = "";
        //    if (lstPaySheetContainer != null)
        //    {
        //        ContainerXML = Utility.CreateXML(lstPaySheetContainer);
        //    }

        //    String[] dt = InvoiceDate.Split(' ');

        //    WFLD_ExportRepository objChrgRepo = new WFLD_ExportRepository();

        //    objChrgRepo.GetLoadedPaymentSheetInvoice(StuffingReqId, dt[0], InvoiceType, ContainerXML, PayeeId, PartyId, InvoiceId);


        //    var Output = (WFLD_MovementInvoice)objChrgRepo.DBResponse.Data;

        //    Output.InvoiceDate = InvoiceDate;
        //    Output.Module = "EXPLod";

        //    Output.lstPrePaymentCont.ToList().ForEach(item =>
        //    {
        //        if (!Output.ShippingLineName.Contains(item.ShippingLineName))
        //            Output.ShippingLineName += item.ShippingLineName + ", ";
        //        if (!Output.CHAName.Contains(item.CHAName))
        //            Output.CHAName += item.CHAName + ", ";
        //        if (!Output.ImporterExporter.Contains(item.ImporterExporter))
        //            Output.ImporterExporter += item.ImporterExporter + ", ";

        //        if (!Output.CFSCode.Contains(item.CFSCode))
        //            Output.CFSCode += item.CFSCode + ", ";
        //        if (!Output.ArrivalDate.Contains(item.ArrivalDate + " " + item.ArrivalTime))
        //            Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
        //        if (!Output.DestuffingDate.Contains(item.DestuffingDate))
        //            Output.DestuffingDate += item.DestuffingDate + ", ";
        //        if (!Output.StuffingDate.Contains(item.StuffingDate))
        //            Output.StuffingDate += item.StuffingDate + ", ";

        //        if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
        //        {
        //            Output.lstPostPaymentCont.Add(new WFLDPostPaymentContainer
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



        //    return Json(Output, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadedContPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                var invoiceData = JsonConvert.DeserializeObject<WFLD_ExpPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                int TransporterId = 0;
                if (objForm["TransporterId"] != "")
                {
                    TransporterId = Convert.ToInt32(objForm["TransporterId"]);
                }
                else
                {
                    TransporterId = 0;
                }


                int Vno = 0;
                if (objForm["NoVehicle"] != "")
                {
                    Vno = Convert.ToInt32(objForm["NoVehicle"]);
                }

                int CountryId = 0;
                if (objForm["CountryId"] != "")
                {
                    CountryId = Convert.ToInt32(objForm["CountryId"]);
                }
                String CustomSeal = "";
                String LinerSeal = "";
                if (objForm["SealNumber"] != "")
                {
                    CustomSeal = objForm["SealNumber"];
                }
                if (objForm["LinerSeal"] != "")
                {
                    LinerSeal = objForm["LinerSeal"];
                }

                string TransporterName = objForm["TransporterName"];
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

                int CHAId = 0;
                if (objForm["CHAId"] != "")
                {
                    CHAId = Convert.ToInt32(objForm["CHAId"]);
                }
                string CHAName = "";
                if (objForm["CHAName"] != "")
                {
                    CHAName = Convert.ToString(objForm["CHAName"]);
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
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditInvoiceContLoaded(invoiceData, Vno, CountryId, CHAId, CHAName, CustomSeal, LinerSeal, TransporterId, TransporterName, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        //public JsonResult AddEditLoadedContPaymentSheet(FormCollection objForm)
        //{
        //    try
        //    {
        //        int BranchId = Convert.ToInt32(Session["BranchId"]);
        //        //var formData = JsonConvert.DeserializeObject<PaymentSheetFinalModel>(objForm["ChargesJson"].ToString());
        //        //string ContainerXML = "";
        //        //string ChargesXML = "";
        //        //if (formData.lstPSContainer != null)
        //        //{
        //        //    ContainerXML = Utility.CreateXML(formData.lstPSContainer);
        //        //}
        //        //if (formData.lstChargesType != null)
        //        //{
        //        //    ChargesXML = Utility.CreateXML(formData.lstChargesType);
        //        //}

        //        //ExportRepository objExport = new ExportRepository();
        //        //objExport.AddEditExpInvoice(formData, ContainerXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
        //        //return Json(objExport.DBResponse);

        //        var invoiceData = JsonConvert.DeserializeObject<WFLD_MovementInvoice>(objForm["PaymentSheetModelJson"].ToString());
        //        string ContainerXML = "";
        //        string ChargesXML = "";
        //        string ContWiseCharg = "";
        //        string OperationCfsCodeWiseAmtXML = "";
        //        string ChargesBreakupXML = "";
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
        //        if (invoiceData.lstPostPaymentChrgBreakup != null)
        //        {
        //            ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
        //        }

        //        WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
        //        objChargeMaster.AddEditInvoiceContLoaded(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPLod");

        //        invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
        //        invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");

        //        invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


        //        objChargeMaster.DBResponse.Data = invoiceData;
        //        return Json(objChargeMaster.DBResponse);


        //    }
        //    catch (Exception ex)
        //    {
        //        var Err = new { Status = -1, Message = "Error" };
        //        return Json(Err);
        //    }
        //}




        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult LoadContInvoicePrint(string InvoiceNo)
        {
            WFLD_ExportRepository objGPR = new WFLD_ExportRepository();
            objGPR.GetInvoiceDetailsForContLoadedPrintByNo(InvoiceNo, "EXPLod");
            WFLD_MovementInvoice objGP = new WFLD_MovementInvoice();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {


                objGP = (WFLD_MovementInvoice)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceContLoaded(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceContLoaded(WFLD_MovementInvoice objGP, int InvoiceId)
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
        public ActionResult ListOfLoadedInvoice(string Module, int EditFlag = 0, string InvoiceNo = null, string InvoiceDate = null, string ContainerNo = null)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfLoadedInvoice(Module, EditFlag, InvoiceNo, InvoiceDate, ContainerNo);
            List<WFLD_ListLoadedInvoice> obj = new List<WFLD_ListLoadedInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLD_ListLoadedInvoice>)objER.DBResponse.Data;
            return PartialView("ListofLoadedInvoice", obj);
        }
        #endregion

        #region Edit Loaded Container Invoice
        [HttpGet]
        public ActionResult EditLoadedContainerPaymentSheet()
        {

            WFLD_ExportRepository objExp = new WFLD_ExportRepository();

            objExp.GetLoadContainerInvoiceForEdit("EXPLod");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;


            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;


            return PartialView();
        }

        [HttpGet]
        public JsonResult GetEditLoadContDtlForPaymentSheet(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetEditLoadContDtlForPaymentSheet(InvoiceId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLoadContainerIvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetLoadContainerIvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Load Container Request
        [HttpGet]
        public ActionResult CreateLoadContainerRequest()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            WFLD_LoadContReq ObjLR = new WFLD_LoadContReq();
            ObjLR.LoadContReqDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

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
            ViewBag.Currentdt = DateTime.Now.ToString("dd/MM/yyyy");

            objER.ListOfPOD();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)objER.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }

            objER.GetForwarder();
            if (objER.DBResponse.Data != null)
                ViewBag.ForwarderList = new SelectList((List<CwcExim.Areas.Export.Models.ForwarderList>)objER.DBResponse.Data, "ForwarderId", "Forwarder");
            else
                ViewBag.ForwarderList = null;
            objER.ListOfForeignLiner();
            if (objER.DBResponse.Data != null)
                ViewBag.ForeignLinerList = (List<string>)objER.DBResponse.Data;
            else
                ViewBag.ForeignLinerList = null;
            //  List<container> Lstcontainer = new List<container>();
            //   List<Port> LstPort = new List<Port>();
            List<container> Lstcontainer = new List<container>();

            objER.GetLoadedContainer();


            if (objER.DBResponse.Data != null)
            {
                Lstcontainer = (List<CwcExim.Areas.GateOperation.Models.container>)objER.DBResponse.Data;
            }
            ViewBag.Lstcontainer = Lstcontainer;

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
            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
            WFLD_LoadContReq ObjContReq = new WFLD_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (WFLD_LoadContReq)ObjRR.DBResponse.Data;
            }
            ObjRR.ListOfPOD();
            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfPOD = new SelectList((List<Port>)ObjRR.DBResponse.Data, "PortId", "PortName");
            }
            else
            {
                ViewBag.ListOfPOD = null;
            }
            return PartialView(ObjContReq);
        }
        [HttpGet]
        public ActionResult EditLoadContainerRequest(int LoadContReqId)
        {
            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
            WFLD_LoadContReq ObjContReq = new WFLD_LoadContReq();
            ObjRR.GetLoadContDetails(LoadContReqId);
            if (ObjRR.DBResponse.Data != null)
            {
                ObjContReq = (WFLD_LoadContReq)ObjRR.DBResponse.Data;
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

                ObjRR.ListOfPOD();
                if (ObjRR.DBResponse.Data != null)
                {
                    ViewBag.ListOfPOD = new SelectList((List<Port>)ObjRR.DBResponse.Data, "PortId", "PortName");
                }
                else
                {
                    ViewBag.ListOfPOD = null;
                }
                ObjRR.GetForwarder();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ForwarderList = new SelectList((List<CwcExim.Areas.Export.Models.ForwarderList>)ObjRR.DBResponse.Data, "ForwarderId", "Forwarder");
                else
                    ViewBag.ForwarderList = null;

                ObjRR.ListOfForeignLiner();
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ForeignLinerList = (List<string>)ObjRR.DBResponse.Data;
                else
                    ViewBag.ForeignLinerList = null;

                ObjRR.ListOfPackUQCForPage("", 0);
                if (ObjRR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
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
        public ActionResult ListLoadContainerRequest(string SearchValue = "")
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLDListLoadContReq> lstCont = new List<WFLDListLoadContReq>();
            objER.ListOfLoadCont(SearchValue);
            if (objER.DBResponse.Data != null)
                lstCont = (List<WFLDListLoadContReq>)objER.DBResponse.Data;
            return PartialView(lstCont);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContReq(WFLD_LoadContReq objReq)
        {
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository objER = new WFLD_ExportRepository();



                List<WFLDLoadContReqDtl> LstLoadContReqDtl = JsonConvert.DeserializeObject<List<WFLDLoadContReqDtl>>(objReq.StringifyData);

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
                    XML = Utility.CreateXML(LstLoadContReqDtl); //Utility.CreateXML(JsonConvert.DeserializeObject<List<WFLDLoadContReqDtl>>(objReq.StringifyData));
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                ObjER.GetAutoPopulateLoadedData(ContainerName);

                return Json(ObjER.DBResponse.Data, JsonRequestBehavior.AllowGet);
                // ViewBag.JSONResult = ObjGOR.DBResponse.Data;
                //EntryThroughGate objEntryThroughGate = new EntryThroughGate();
                //objEntryThroughGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
                //string strDate = objEntryThroughGate.ReferenceDate;
                //string[] arrayDate = strDate.Split(' ');
                //objEntryThroughGate.ReferenceDate = arrayDate[0];
                //ViewBag.strTime = objEntryThroughGate.EntryTime;

                //return Json(objEntryThroughGate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
                // ViewBag.JSONResult = "Error";
                //return PartialView("CreateEntryThroughGate");
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
        public ActionResult ListOfBTTCargo(string SearchValue = "")
        {
            List<BTTCargoEntry> lstBTTCargoEntry = new List<BTTCargoEntry>();
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetBTTCargoEntry(SearchValue);
            if (objExport.DBResponse.Data != null)
                lstBTTCargoEntry = (List<BTTCargoEntry>)objExport.DBResponse.Data;

            return PartialView(lstBTTCargoEntry);
        }

        [HttpGet]
        public ActionResult AddBTTCargo()
        {



            BTTCargoEntry objBTTCargoEntry = new BTTCargoEntry();
            objBTTCargoEntry.BTTDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();

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
        public JsonResult GetCartingDetailList(int ShortCarId)
        {
            try
            {
                WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                if (ShortCarId > 0)
                    objExport.GetCartingDetailList(ShortCarId);
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
                WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
                    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            //WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetShipBillForPaymentSheet(StuffingReqId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBTTPaymentSheet(string DeliveryDate, string InvoiceDate, int AppraisementId, string TaxType,
            List<PaymentSheetContainer> lstPaySheetContainer, int PartyId, int PayeeId, string SEZ,
            int InvoiceId = 0, int NoOfVehicles = 0)
        {
            //AppraisementId ----> StuffingReqID

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetBTTPaymentSheet(DeliveryDate, InvoiceDate, AppraisementId, TaxType, XMLText, PartyId, PayeeId, SEZ, InvoiceId, NoOfVehicles);
            var Output = (WFLDInvoiceBTT)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
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
                    Output.lstPostPaymentCont.Add(new WFLDPostPaymentContainerBTT
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
                //Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                //Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
                Output.InvoiceAmt = (Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = 0;
            });



            return Json(Output, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditBTTPaymentSheet(WFLDInvoiceBTT objForm, int Vehicle, string VehicleNumber)
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
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditBTTInvoice(invoiceData, Vehicle, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTT", VehicleNumber, CargoXML);

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


        public ActionResult GetBTTPaymentSheetList(int EditFlag = 0)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            List<BTTPaymentSheetViewModel> lstBTTPaymentSheet = new List<BTTPaymentSheetViewModel>();
            obj.GetBTTPaymentSheetList(EditFlag);
            if (obj.DBResponse.Data != null)
            {
                lstBTTPaymentSheet = (List<BTTPaymentSheetViewModel>)obj.DBResponse.Data;
            }

            //
            return PartialView("GetBTTPaymentSheetList", lstBTTPaymentSheet);
        }

        #endregion

        #region Edit BTT Invoice
        [HttpGet]
        public ActionResult EditBTTPaymentSheet()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            WFLD_ExportRepository objExp = new WFLD_ExportRepository();

            objExp.GetBTTInvoiceForEdit("BTT");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetEditBTTDtlForPaymentSheet(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetEditBTTDtlForPaymentSheet(InvoiceId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBTTInvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetBTTInvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBTTPaymentSheetSearchList(string Module, int EditFlag = 0, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            List<BTTPaymentSheetViewModel> lstBTTContainerInvoiceList = new List<BTTPaymentSheetViewModel>();
            obj.GetBTTPaymentSheetSearchList(Module, EditFlag, ContainerNo, InvoiceNo, InvoiceDate);
            if (obj.DBResponse.Data != null)
            {
                lstBTTContainerInvoiceList = (List<BTTPaymentSheetViewModel>)obj.DBResponse.Data;
            }

            return PartialView("GetBTTPaymentSheetList", lstBTTContainerInvoiceList);
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
            Wfld_EntryThroughGateRepository objImport = new Wfld_EntryThroughGateRepository();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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

            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetExportDestuffingPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId);
            var Output = (WFLDInvoiceExpDestuf)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new WFLDPostPaymentContainerExpDestuf
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
        public JsonResult AddEditExportDestuffing(WFLDInvoiceExpDestuf objForm)
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
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();

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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            GodownRepository ObjGR = new GodownRepository();
            List<Godown> lstGodown = new List<Godown>();

            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)ObjGR.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);



            return Json(ViewBag.GodownList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetToGodownForCargo()
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.getOnlyRightsGodown();
            List<Godown> lstGodownF = new List<Godown>();
            if (objExport.DBResponse.Data != null)
            {
                lstGodownF = (List<Godown>)objExport.DBResponse.Data;
            }
            ViewBag.GodownListF = JsonConvert.SerializeObject(lstGodownF);

            return Json(ViewBag.GodownListF, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPartyForCargo()
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetShipBillDetails(int ShippingLineId, int ShiftingType, int GodownId)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            objPpgRepo.GetCargoShiftingPaymentSheet(InvoiceDate, ShippingLineId, XMLText, InvoiceId, TaxType, PayeeId);
            var Output = (WFLDInvoiceCargoShifting)objPpgRepo.DBResponse.Data;

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
        public JsonResult AddEditCargoShifting(WFLDInvoiceCargoShifting objForm, List<CargoShiftingShipBillDetails> lstShipbills,
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
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
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
                ViewBag.LstContainerNo = new SelectList((List<WFLD_ContainerMovement>)ObjIR.DBResponse.Data, "ContainerStuffingId", "Container");
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
                ViewBag.LocationNoList = new SelectList((List<WFLD_ContainerMovement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
            }
            else
            {
                ViewBag.LocationNoList = null;
            }
            return PartialView();
        }





        [NonAction]
        public string GeneratingTentativePDFforContainerMovement(WFLD_MovementInvoice invoiceDataobj)
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

                foreach (WFLDPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
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

                foreach (WFLDPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
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
            var invoiceData = WFLDTentativeInvoice.InvoiceObjW;

            if (ctype == "W")
                invoiceData = WFLDTentativeInvoice.InvoiceObjW;
            if (ctype == "GR")
                invoiceData = WFLDTentativeInvoice.InvoiceObjGR;
            if (ctype == "FMC")
                invoiceData = WFLDTentativeInvoice.InvoiceObjFMC;


            string Path = GeneratingTentativePDFforContainerMovement(invoiceData);
            return Json(new { Status = 1, Message = Path });


        }
        #endregion

        #region Export Container RR Credit Debit
        [HttpGet]
        public ActionResult CreateRRCreditDebitPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetContainerDetForExpRR(InvoiceId);
            WFLDRRCreditDebitInvoiceDetails result = new WFLDRRCreditDebitInvoiceDetails();
            if (objExport.DBResponse.Status > 0)
                result = (WFLDRRCreditDebitInvoiceDetails)objExport.DBResponse.Data;
            else
                result = null;
            return Json(new { Data = result, Status = (result == null ? 0 : 1) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExportRRPaymentSheet(int InvoiceId, int ShippingLineId)
        {
            WFLD_ExportRepository objChrgRepo = new WFLD_ExportRepository();
            objChrgRepo.GetEXPRRCDSheetInvoice(InvoiceId, ShippingLineId);
            int Status = 0;
            WFLDInvoiceRRCreditDebit Output = new WFLDInvoiceRRCreditDebit();
            if (objChrgRepo.DBResponse.Data != null)
            {
                Output = (WFLDInvoiceRRCreditDebit)objChrgRepo.DBResponse.Data;
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
        public JsonResult AddEditExportRRCreditDebitModule(WFLDInvoiceRRCreditDebit objForm)
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
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objGPR = new WFLD_ExportRepository();
            objGPR.GetInvoiceDetailsForContLoadedPrintByNo(InvoiceNo, "EXPLod");
            WFLD_MovementInvoice objGP = new WFLD_MovementInvoice();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {


                objGP = (WFLD_MovementInvoice)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceContLoaded(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceContLoaded(WFLD_MovementInvoice objGP, int InvoiceId)
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
            WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
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
            List<WFLDExportTrainSummaryUpload> TrainSummaryUploadList = new List<WFLDExportTrainSummaryUpload>();
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
                                        WFLDExportTrainSummaryUpload objTrainSummaryUpload = new WFLDExportTrainSummaryUpload();
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

                                WFLD_ExportRepository objImport = new WFLD_ExportRepository();
                                objImport.CheckTrainSummaryUpload(TrainNo, TrainSummaryUploadXML);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = (List<WFLDExportTrainSummaryUpload>)objImport.DBResponse.Data;
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
        public ActionResult SaveUploadData(WFLDExportTrainSummaryUpload objTrainSummaryUpload)
        {
            int data = 0;
            if (objTrainSummaryUpload.TrainSummaryList != null)
                objTrainSummaryUpload.TrainSummaryUploadList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLDExportTrainSummaryUpload>>(objTrainSummaryUpload.TrainSummaryList);
            if (objTrainSummaryUpload.TrainSummaryUploadList.Count > 0)
            {
                string TrainSummaryUploadXML = Utility.CreateXML(objTrainSummaryUpload.TrainSummaryUploadList);
                WFLD_ExportRepository objImport = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLDExportTrainSummaryUpload> lstCargoSeize = new List<WFLDExportTrainSummaryUpload>();
            objER.ListOfTrainSummary();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<WFLDExportTrainSummaryUpload>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
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
        //        WFLD_ImportRepository objImport = new WFLD_ImportRepository();
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

        //                        WFLD_ImportRepository objImport = new WFLD_ImportRepository();
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




        #region Export E07 Excel upload

        [HttpGet]
        public ActionResult ICEGATEE07ExcelUpload()
        {
            return PartialView();
        }

        //[HttpGet]
        //public ActionResult GetPortList()
        //{
        //    WFLD_ExportRepository ObjRR = new WFLD_ExportRepository();
        //    ObjRR.GetPortOfLoading();
        //    if (ObjRR.DBResponse.Data != null)
        //    {
        //        return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }

        //    //PortRepository objImport = new PortRepository();
        //    //objImport.GetAllPort();
        //    //if (objImport.DBResponse.Status > 0)
        //    //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //    //else
        //    //    return Json(null, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult CheckUploadEO7()
        {
            int status = 0;
            List<WFLDExportEGME07Upload> EGME07SummaryUploadList = new List<WFLDExportEGME07Upload>();
            log.Info("Request.Files.Count :" + Request.Files.Count);
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase File = Request.Files[0];
                string extension = Path.GetExtension(File.FileName);
                String filename = File.FileName;
                log.Info("Filename :" + filename);
                log.Info("File Length :" + filename.Length);

                if (filename.Length <= 50)
                {

                    log.Info("File extension :" + extension);
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        DataTable dt = Utility.GetExcelData(File);
                        foreach (DataColumn column in dt.Columns)
                        {
                            log.Info("Column Name" + column.ColumnName);
                        }
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                try
                                {

                                    log.Info("data Table row count :" + dt.Rows.Count);
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["CONTAINERNO"])))
                                        {
                                            WFLDExportEGME07Upload objEGME07SummaryUpload = new WFLDExportEGME07Upload();
                                            objEGME07SummaryUpload.MESSAGETYPE = Convert.ToString(dr["MESSAGETYPE"]);
                                            objEGME07SummaryUpload.MODEOFTRANSPORT = Convert.ToString(dr["MODEOFTRANSPORT"]);
                                            objEGME07SummaryUpload.CUSTOMSHOUSECODE = Convert.ToString(dr["CUSTOMSHOUSECODE"]);
                                            objEGME07SummaryUpload.SBNO = Convert.ToString(dr["SBNO"]);
                                            //             CultureInfo culture = new CultureInfo("en-US", true);




                                            objEGME07SummaryUpload.SBDATE = Convert.ToString(dr["SBDATE"]);
                                            //             DateTime dateVal = DateTime.ParseExact(objEGME07SummaryUpload.SBDATE, "yyyy-MM-dd", culture);
                                            //DateTime dateVal = DateTime.pa(objEGME07SummaryUpload.SBDATE, "YYYY-dd-mm", culture);

                                            //objEGME07SummaryUpload.SBDATE = Convert.ToString(DateTime.ParseExact(dateVal,"YYYY-dd-mm", culture);

                                            objEGME07SummaryUpload.VECHICLENO = Convert.ToString(dr["VECHICLENO"]);
                                            objEGME07SummaryUpload.VECHICLEDEPARTUREDATE = Convert.ToString(dr["VECHICLEDEPARTUREDATE"]);
                                            objEGME07SummaryUpload.CONTAINERNO = Convert.ToString(dr["CONTAINERNO"]);
                                            objEGME07SummaryUpload.SHIPPINGLINECODE = Convert.ToString(dr["SHIPPINGLINECODE"]);

                                            objEGME07SummaryUpload.WEIGHT = Convert.ToDecimal(dr["WEIGHT"]);
                                            objEGME07SummaryUpload.DESTINATIONPORTCODE = Convert.ToString(dr["DESTINATIONPORTCODE"]);
                                            objEGME07SummaryUpload.ORIGINRAILSTATIONCODE = Convert.ToString(dr["ORIGINRAILSTATIONCODE"]);
                                            objEGME07SummaryUpload.GATEWAYPORTCODE = Convert.ToString(dr["GATEWAYPORTCODE"]);

                                            objEGME07SummaryUpload.ISOCODE = Convert.ToString(dr["ISOCODE"]);
                                            objEGME07SummaryUpload.STATUSOFCONTAINER = Convert.ToString(dr["STATUSOFCONTAINER"]);
                                            objEGME07SummaryUpload.ExcelFileName = filename;

                                            EGME07SummaryUploadList.Add(objEGME07SummaryUpload);
                                        }
                                        else
                                        {
                                            EGME07SummaryUploadList = null;
                                            status = -5;
                                            return Json(new { Status = status, Data = EGME07SummaryUploadList }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    string EGME07SummaryUploadListXML = Utility.CreateXML(EGME07SummaryUploadList);

                                    WFLD_ExportRepository objImport = new WFLD_ExportRepository();
                                    //objImport.CheckTrainSummaryUpload(TrainNo, TrainSummaryUploadXML);
                                    //if (objImport.DBResponse.Status > -1)
                                    //{
                                    //    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    //    EGME07SummaryUploadList = (List<WFLDExportTrainSummaryUpload>)objImport.DBResponse.Data;
                                    //}
                                    //else
                                    //{
                                    //    status = -6;

                                    //}
                                }

                                catch (Exception ex)
                                {
                                    log.Info("Error msg :" + ex.Message + " " + ex.StackTrace);
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
                    status = 52;
                }
            }

            else
            {
                status = -4;
            }

            if (status < 0)
            {
                EGME07SummaryUploadList = null;
            }
            if (EGME07SummaryUploadList != null)
            {
                if (EGME07SummaryUploadList.Count > 0)
                {
                    status = 1;
                }
            }
            return Json(new { Status = status, Data = EGME07SummaryUploadList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveUploadE07Data(WFLDExportEGME07Upload objEgmSummaryUpload)
        {
            int status = 0;
            WFLDExportEGME07Upload objEgmSummary = new WFLDExportEGME07Upload();
            if (objEgmSummaryUpload.EGMSummaryList != null)
                objEgmSummaryUpload.EGME07SummaryUploadList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLDExportEGME07Upload>>(objEgmSummaryUpload.EGMSummaryList);
            if (objEgmSummaryUpload.EGME07SummaryUploadList.Count > 0)
            {
                string EGMSummaryUploadXML = Utility.CreateXML(objEgmSummaryUpload.EGME07SummaryUploadList);
                WFLD_ExportRepository objImport = new WFLD_ExportRepository();
                objImport.AddUpdateEGMSummaryUpload(objEgmSummaryUpload, ((Login)(Session["LoginUser"])).Uid, EGMSummaryUploadXML);
                if (objImport.DBResponse.Status > 0)

                {
                    status = Convert.ToInt32(objImport.DBResponse.Status);
                    objEgmSummary = (WFLDExportEGME07Upload)(objImport.DBResponse.Data);
                }
                else
                {
                    status = 0;
                }
            }
            return Json(new { Status = status, Data = objEgmSummary }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult ListOfTrainSummary()
        //{
        //    WFLD_ExportRepository objER = new WFLD_ExportRepository();
        //    List<WFLDExportTrainSummaryUpload> lstCargoSeize = new List<WFLDExportTrainSummaryUpload>();
        //    objER.ListOfTrainSummary();
        //    if (objER.DBResponse.Data != null)
        //        lstCargoSeize = (List<WFLDExportTrainSummaryUpload>)objER.DBResponse.Data;
        //    return PartialView(lstCargoSeize);
        //}

        //[HttpGet]
        //public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        //{
        //    WFLD_ExportRepository objImport = new WFLD_ExportRepository();
        //    objImport.GetTrainSummaryDetails(TrainSummaryUploadId);
        //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //}

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
        //        WFLD_ImportRepository objImport = new WFLD_ImportRepository();
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

        //                        WFLD_ImportRepository objImport = new WFLD_ImportRepository();
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
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
            WFLDExportJobOrder objJO = new WFLDExportJobOrder();
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstpickup = (List<WFLDPickupModel>)objIR.DBResponse.Data;
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
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            ObjIR.GetAllTrainNo();
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objRepo = new WFLD_ExportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            IList<WFLD_ImportJobOrderList> lstIJO = new List<WFLD_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<WFLD_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderDetails(string ContainerNo)
        {
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            IList<WFLD_ImportJobOrderList> lstIJO = new List<WFLD_ImportJobOrderList>();
            objIR.GetAllImpJO(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<WFLD_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderDetails", lstIJO);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_ImportJobOrderList> LstJO = new List<WFLD_ImportJobOrderList>();
            ObjCR.GetAllImpJO(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLD_ImportJobOrderList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPort()
        {
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            objIR.GetAllPortForJobOrderTrasport();
            WFLDExportJobOrder objJO = new WFLDExportJobOrder();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstPort = (List<TransformList>)objIR.DBResponse.Data;
            }
            return Json(objJO.lstPort, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            WFLDExportJobOrder objImp = new WFLDExportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (WFLDExportJobOrder)objIR.DBResponse.Data;
            ViewBag.jdate = objImp.JobOrderDate;
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<WFLDPickupModel>)objIR.DBResponse.Data;
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            WFLDExportJobOrder objImp = new WFLDExportJobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (WFLDExportJobOrder)objIR.DBResponse.Data;

            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<WFLDPickupModel>)objIR.DBResponse.Data;
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(WFLDExportJobOrder objImp, String FormOneDetails)
        {

            List<WFLD_TrainDtl> lstDtl = new List<WFLD_TrainDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_TrainDtl>>(FormOneDetails);
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objIR = new WFLD_ExportRepository();
            objIR.GetImportJODetailsFrPrint(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                WFLDPrintJOModel objMdl = new WFLDPrintJOModel();
                objMdl = (WFLDPrintJOModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, ImpJobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForJO(WFLDPrintJOModel objMdl, int ImpJobOrderId)
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

        #region Edit Export Payment Sheet
        [HttpGet]
        public ActionResult EditExportPaymentSheet()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            //ViewData["InvType"] = type;
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();

            objExp.GetContMovementInvoiceForEdit("EXP");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            //objExp.GetContStuffingForPaymentSheet();
            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.StuffingReqList = null;



            //objExp.GetPaymentPartyForPageExport("", 0);
            //if (objExp.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstParty = Jobject["lstParty"];
            //    ViewBag.State = Jobject["State"];
            //}
            //else
            //{
            //    ViewBag.lstParty = null;
            //}
            //objExp.GetPaymentPayerForPageForPD("", 0);
            //if (objExp.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstPayer = Jobject["lstPayer"];
            //    ViewBag.StatePayer = Jobject["StatePayer"];
            //}
            //else
            //{
            //    ViewBag.lstPayer = null;
            //}
            //objExp.GetPaymentPayerForMovementInvoice();

            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.PaymentPayer = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.PaymentPayer = null;


            //objExp.GetTransporterPaymentParty();

            //if (objExp.DBResponse.Status > 0)
            //    ViewBag.TransporterPaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            //else
            //    ViewBag.TransporterPaymentParty = null;

            //objExp.GetLocationForInternalMovement();
            //if (objExp.DBResponse.Data != null)
            //{
            //    ViewBag.LocationNoList = JsonConvert.SerializeObject(objExp.DBResponse.Data);//               
            //}
            //else
            //{
            //    ViewBag.LocationNoList = null;
            //}
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetContainerMovementInvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetContainerMovementInvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEditContDetForPaymentSheet(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetEditContDetForPaymentSheet(InvoiceId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        public ActionResult CreateExportPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetContStuffingForPaymentSheet();
            if (objExp.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            /* objExp.GetPaymentPartyForMovementInvoice();

             if (objExp.DBResponse.Status > 0)
                 ViewBag.PaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
             else
                 ViewBag.PaymentParty = null;*/

            objExp.GetPaymentPartyForPageExport("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }
            objExp.GetPaymentPayerForPageForPD("", 0);
            if (objExp.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExp.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstPayer = Jobject["lstPayer"];
                ViewBag.StatePayer = Jobject["StatePayer"];
            }
            else
            {
                ViewBag.lstPayer = null;
            }
            objExp.GetPaymentPayerForMovementInvoice();

            if (objExp.DBResponse.Status > 0)
                ViewBag.PaymentPayer = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.PaymentPayer = null;


            objExp.GetTransporterPaymentParty();

            if (objExp.DBResponse.Status > 0)
                ViewBag.TransporterPaymentParty = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.TransporterPaymentParty = null;

            objExp.GetLocationForInternalMovement();
            if (objExp.DBResponse.Data != null)
            {
                ViewBag.LocationNoList = JsonConvert.SerializeObject(objExp.DBResponse.Data);//
                //new SelectList((List<WFLD_ContainerMovement>)objExp.DBResponse.Data, "LocationId", "LocationName");
            }
            else
            {
                ViewBag.LocationNoList = null;
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                int Vno = objForm["noOfvehicles"] == "" ? 0 : Convert.ToInt32(objForm["noOfvehicles"].ToString());

                var invoiceData = JsonConvert.DeserializeObject<WFLD_ExpPaymentSheet>(objForm["PaymentSheetModelJson"]);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                int TransporterId = 0;
                if (objForm["TransporterId"] != "")
                {
                    TransporterId = Convert.ToInt32(objForm["TransporterId"]);
                }
                else
                {
                    TransporterId = 0;
                }

                string TransporterName = objForm["TransporterName"];
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

                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditExpInvoice(invoiceData, Vno, TransporterId, TransporterName, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXP");

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
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetContDetForPaymentSheet(AppraisementId);
            object obj = null;
            if (objImport.DBResponse.Status > 0)
                obj = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                obj = null;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string DeliveryDate, string InvoiceDate, int AppraisementId, string TaxType, List<PaymentSheetContainer> lstPaySheetContainer,
           int PartyId, int PayeeId, int IntercartingApplicable, string SEZ, int InvoiceId = 0, int ICDDestuffing = 0, int distance = 0, int othrs = 0, int insured = 0, int movetoport = 0, int noOfvehicles = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }
            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            objPpgRepo.GetExportPaymentSheet(DeliveryDate, InvoiceDate, AppraisementId, TaxType, XMLText, InvoiceId, PartyId, PayeeId, IntercartingApplicable, SEZ, ICDDestuffing, distance, othrs, insured, movetoport, noOfvehicles);
            var Output = (WFLD_ExpPaymentSheet)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "EXP";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WFLD_ExpContainer();
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
                //Output.InvoiceAmt = Math.Ceiling(Output.lstPostPaymentChrg.Sum(o => o.Total));
                //Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;

                Output.InvoiceAmt = (Output.lstPostPaymentChrg.Sum(o => o.Total));
                Output.RoundUp = 0;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfContMovementInvoice(string Module, int EditFlag = 0, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null,int Page=0)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfContMovementInvoice(Module, EditFlag, ContainerNo, InvoiceNo, InvoiceDate,Page);
            List<WFLDListOfContMovementInvoice> obj = new List<WFLDListOfContMovementInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfContMovementInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfContMovementInvoice", obj);
        }
        [HttpGet]
        public ActionResult ListOfLoadMoreContMovementInvoice(string Module, int EditFlag = 0, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfContMovementInvoice(Module, EditFlag, ContainerNo, InvoiceNo, InvoiceDate, Page);
            List<WFLDListOfContMovementInvoice> obj = new List<WFLDListOfContMovementInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfContMovementInvoice>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null,int Page=0)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate,Page);
            List<WFLDListOfExpInvoice> obj = new List<WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        [HttpGet]
        public ActionResult ListLoadMoreExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListLoadMoreExpInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<WFLDListOfExpInvoice> obj = new List<WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #region Party and payee modal
        [HttpGet]
        public JsonResult SearchPartyNameByPartyAlias(string PartyCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPartyForPageExport(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyCodeList(string PartyCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPartyForPageExport(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCodeForPD(string PartyCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPayerForPageForPD(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerListForPD(string PartyCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPayerForPageForPD(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bond Advance Payment Sheet
        public ActionResult ListOfInvoiceBondAdv(string InvoiceNo = null, string InvoiceDate = null, string Modulename = "BNDAdv")
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfExpInvoice(Modulename, InvoiceNo, InvoiceDate);
            List<WFLDListOfExpInvoice> obj = new List<WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfBNDInvoice", obj);
        }
        #endregion

        #region Bond Delivery Payment Sheet
        public ActionResult ListOfInvoiceBondDelivery(string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfExpInvoice("BND", InvoiceNo, InvoiceDate);
            List<WFLDListOfExpInvoice> obj = new List<WFLDListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLDListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfBNDInvoice", obj);
        }
        #endregion



        //#region Ship Bill Amendment

        //public ActionResult Amendment()
        //{
        //    WFLD_ExportRepository objER = new WFLD_ExportRepository();
        //    objER.GetAmenSBList();
        //    if (objER.DBResponse.Data != null)
        //    {
        //        ViewBag.ListOfSBNoAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //    }
        //    else
        //    {
        //        ViewBag.ListOfSBNoAmendment = null;
        //    }
        //    objER.ListOfExporter();
        //    if (objER.DBResponse.Data != null)
        //    {
        //        ViewBag.ListOfExporterForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //    }
        //    else
        //    {
        //        ViewBag.ListOfExporterForAmendment = null;
        //    }

        //    objER.GetAllCommodityForPageAmendment("", 0);
        //    if (objER.DBResponse.Data != null)
        //    {
        //        ViewBag.ListOfCommodityForAmendment = JsonConvert.SerializeObject(objER.DBResponse.Data);
        //        //var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
        //        // var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        //  ViewBag.LstCommodity = Jobject["LstCommodity"];
        //        // ViewBag.CommodityState = Jobject["State"];
        //    }
        //    else
        //    {
        //        ViewBag.ListOfCommodityForAmendment = null;
        //    }
        //    return PartialView();
        //}
        //[HttpGet]

        //public JsonResult GetSBDetailsBySBNo(string SBid, string SbDate)
        //{
        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.GetAmenSBDetailsBySbNoDate(SBid, SbDate);
        //    if (obj.DBResponse.Status > 0)
        //    {
        //        return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //[CustomValidateAntiForgeryToken]
        //public JsonResult SaveAmendement(List<OldInfoSb> vm, List<NewInfoSb> newvm, string Date, string AmendmentNO)
        //{
        //    string OldInfoSbXml = Utility.CreateXML(vm);
        //    string NewInfoSbSbXml = Utility.CreateXML(newvm);
        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.AddEditAmendment(OldInfoSbXml, NewInfoSbSbXml, Date);


        //    return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetAmendDetails(string AmendNo)
        //{

        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.GetAmenSBDetailsByAmendNO(AmendNo);

        //    return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetAmendDetailsByAmendNo(string AmendNo)
        //{

        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.GetAmenDetailsByAmendNO(AmendNo);

        //    return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[CustomValidateAntiForgeryToken]
        //public JsonResult SubmitAmedData(AmendmentViewModel vm)
        //{
        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.AddEditShipAmendment(vm);
        //    return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetAmendList()
        //{

        //    WFLD_ExportRepository obj = new WFLD_ExportRepository();
        //    obj.lstAmendDate();

        //    return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        //}

        //#endregion


        //Add Chiranjit Das 14-08-2019

        #region Edit Palletization CBM
        public ActionResult PostPalletizationCBMUpdate()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSbNoEditPalletizationCBM()
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetSbNoEditPalletizationCBM();
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveEditPalletizationCBM(WFLD_PostPalletizationCBM vm)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();

            obj.SaveEditPalletizationCBM(vm);
            return Json(obj.DBResponse);
        }
        [HttpGet]
        public ActionResult GetGetPostPalletizationCBMList(string SearchValue = "", int Page = 0)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_PostPalletizationCBM> lstPostPalletizationCBM = new List<WFLD_PostPalletizationCBM>();
            ObjCR.GetPostPalletizationCBM(SearchValue, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstPostPalletizationCBM = (List<WFLD_PostPalletizationCBM>)ObjCR.DBResponse.Data;
            }
            return PartialView("PostPalletizationCBMList", lstPostPalletizationCBM);
        }

        [HttpGet]
        public ActionResult GetLoadMoreGetPostPalletizationCBMList(string SearchValue = "", int Page = 0)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_PostPalletizationCBM> lstPostPalletizationCBM = new List<WFLD_PostPalletizationCBM>();
            ObjCR.GetPostPalletizationCBM(SearchValue, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                lstPostPalletizationCBM = (List<WFLD_PostPalletizationCBM>)ObjCR.DBResponse.Data;
            }
            return Json(lstPostPalletizationCBM, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pallatisation

        public ActionResult Pallatisation()
        {
            List<Godown> lstGodown = new List<Godown>();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (objER.DBResponse.Data != null)
            {
                lstGodown = (List<Godown>)objER.DBResponse.Data;
            }
            ViewBag.GodownList = JsonConvert.SerializeObject(lstGodown);
            return PartialView();
        }


        public JsonResult GetSbNoForPallatisation()
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetSBListPallatisation();
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSbNoDetailsForPallatisation(int CartingRegisterId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetSBDetailsPallatisation(CartingRegisterId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartyNameForPallatisation(int Page, string PartyCode)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetPartyNameForPallatisation(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetPallatisationCharge(int PartyId, int Pallet, string SEZ)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetChargeForPallatisation(PartyId, Pallet, SEZ);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditPallatisation(WLFD_Pallatisation vm)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            vm.PallatisationChargeDetails = JsonConvert.DeserializeObject<List<WFLD_PallasationChargeBase>>(vm.PallatisationCharge);
            vm.PallatisationSBDetails = JsonConvert.DeserializeObject<List<WLFD_PallatisationSBDetails>>(vm.PalletSBDetails);
            vm.PallatisationNewSBDetails = JsonConvert.DeserializeObject<List<WLFD_PallatisationSBDetails>>(vm.PalletSBNewDetails);

            string Xml = Utility.CreateXML(vm.PallatisationChargeDetails);
            string XmlSb = Utility.CreateXML(vm.PallatisationSBDetails);
            string XmlNewSb = Utility.CreateXML(vm.PallatisationNewSBDetails);

            obj.AddEditPallatisation(vm, Xml, XmlSb, XmlNewSb);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllPallatisationList(int EditFlag = 0, string InvoiceNo = null, string InvoiceDate = null, string ShippingBillNo = null)
        {
            List<WLFD_Pallatisation> lstPallatisation = new List<WLFD_Pallatisation>();
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAllPallatisationList(EditFlag, InvoiceNo, InvoiceDate, ShippingBillNo);
            if (obj.DBResponse.Status == 1)
            {
                lstPallatisation = (List<WLFD_Pallatisation>)obj.DBResponse.Data;
            }
            return PartialView("GetAllPallatisationList", lstPallatisation);
        }

        //[HttpGet]
        //public ActionResult ListOfCCINEntry(string SearchValue = "")
        //{
        //    WFLD_ExportRepository objER = new WFLD_ExportRepository();
        //    List<WFLD_CCINEntry> lstCCINEntry = new List<WFLD_CCINEntry>();
        //    objER.GetAllCCINEntryForPage(0, SearchValue);
        //    if (objER.DBResponse.Data != null)
        //        lstCCINEntry = (List<WFLD_CCINEntry>)objER.DBResponse.Data;
        //    return PartialView(lstCCINEntry);
        //}

        public ActionResult GetInvoicePrint(int InvoiceId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.PrintPallatisationInvoice(InvoiceId);
            string Path = "";
            if (obj.DBResponse.Status == 1)
            {
                // DataSet ds = new DataSet();
                DataSet ds = (DataSet)obj.DBResponse.Data;
                Path = GeneratePdfForPallatisation(ds, InvoiceId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }


        [NonAction]
        public string GeneratePdfForPallatisation(DataSet ds, int InvoiceID)
        {

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/Pallatisation" + InvoiceID + ".pdf";
            var SEZis = "";
            Decimal totamt = 0;
            StringBuilder html = new StringBuilder();
            /*Header Part*/
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td width='40%' valign='top'><img align='right' src='IMGSRC'/></td>");
            html.Append("<td width='200%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label>");
            //html.Append("<br/><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label>");
            html.Append("</td>");
            html.Append("<td width='40%' valign='top'><img align='right' src='ISO'/></td>");
            html.Append("<td width='40%' valign='top'><img align='right' src='SWACHBHARAT'/></td>");
            html.Append("</tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            //html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            //html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
            //html.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
            //html.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label>");
            //html.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>PALLATISATION</label>");
            //html.Append("</td>");
            //html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
            //html.Append("</tr>");

            html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 7pt; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");

            html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center; margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");

            html.Append("<tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='5'><tbody><tr>");

            html.Append("<td colspan='5' width='50%'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + lstInvoice[0].InvoiceNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + lstInvoice[0].PartyName + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + lstInvoice[0].PartyAddress + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + lstInvoice[0].PartyGSTNo + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + lstInvoice[0].PartyState + "(" + lstInvoice[0].PartyStateCode + ")" + "</td></tr>");
            html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>" + "Yes" + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            if (lstInvoice[0].SEZ == "Yes")
            {
                SEZis = "Yes";
            }


            html.Append("<td colspan='6' width='40%'>");
            html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].InvoiceDate + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].PartyState + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].PartyStateCode + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].PaymentMode + "</td></tr>");
            //html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].PartyState +" ("+ lstInvoice[0].PartyStateCode+")" + "</td></tr>");
            //html.Append("<tr><th colspan='6' width='50%'>SEZ</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].SEZ + "</td></tr>");
            html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + lstInvoice[0].SupplyType + "</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td>");

            html.Append("</tr></tbody></table></td></tr>");

            html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt;'>Shipbill Details :</b> </th></tr>");
            html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SB No</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SB Date</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Godown Name</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Location</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Pallet</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Pkg Type</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>No OF PKG</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Exporter</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>CHA Name</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/
            int i = 1;
            lstContainer.ForEach(elem =>
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ShippingBillNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.SbDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.GodownName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.LocationName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Units + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.PkgType + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Weight + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.NOOFPKG + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.PartyName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CHAName + "</td>");
                html.Append("</tr>");
                i = i + 1;
            });
            /***************/
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt;'>Container Charges :</h3> </th></tr>");
            html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead><tr>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
            html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>Taxable Amt.</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
            html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 100px;'>Total</th></tr><tr>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>Amt.</th></tr></thead>");
            html.Append("<tbody>");
            i = 1;
            /*Charges Bind*/
            lstCharges.ForEach(data =>
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 150px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
                totamt = totamt + data.Taxable;
            });
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");


            html.Append("<table style='border: 1px solid #000; border-top: 0; width: 100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
            html.Append("<th rowspan='2' style='width: 140px;'></th>");
            html.Append("<th rowspan='2' style='width: 24%;'></th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 80px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + lstInvoice[0].AllTotal.ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + lstCharges[0].CGSTAmt.ToString("0.00") + "</th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + lstCharges[0].SGSTAmt.ToString("0.00") + "</th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + lstCharges[0].IGSTAmt.ToString("0.00") + "</th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            //html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
            //html.Append("<tbody>");
            //html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
            //html.Append("<th rowspan='2' style='width: 140px;'></th>");
            //html.Append("<th rowspan='2' style='width: 24%;'></th>");
            //html.Append("<th rowspan='2' style='width: 100px;'></th>");
            //html.Append("<th rowspan='2' style='width: 80px;'></th>");
            //html.Append("<th rowspan='2' style='width: 130px;'></th>");
            //html.Append("<th colspan='2' style='width: 200px;'></th>");
            //html.Append("<th colspan='2' style='width: 200px;'></th>");
            //html.Append("<th colspan='2' style='width: 200px;'></th>");
            //html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + lstInvoice[0].RoundUp.ToString("0.00") +"</th></tr><tr>");
            //html.Append("<th style='width: 50px;'></th>");
            //html.Append("<th style='width: 50px;'></th>");
            //html.Append("<th style='width: 50px;'></th>");
            //html.Append("<th style='width: 50px;'></th>");
            //html.Append("<th style='width: 50px;'></th>");
            //html.Append("<th style='width: 50px;'></th></tr>");
            //html.Append("</tbody>");
            //html.Append("</table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
            html.Append("<th rowspan='2' style='width: 140px;'></th>");
            html.Append("<th rowspan='2' style='width: 24%;'></th>");
            html.Append("<th rowspan='2' style='width: 100px;'></th>");
            html.Append("<th rowspan='2' style='width: 80px;'></th>");
            html.Append("<th rowspan='2' style='width: 130px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th colspan='2' style='width: 200px;'></th>");
            html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + lstInvoice[0].InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th>");
            html.Append("<th style='width: 50px;'></th></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody>");
            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(lstCharges[0].Total.ToString("0.00")) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            if (SEZis == "Yes")
            {
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='8'> SUPPLY MEANT FOR EXPORT/ SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS ON PAYMENT OF INTEGRATED TAX </th>");

                html.Append("</tr>");
            }

            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td style='font-size: 6pt; text-align: left;'>Receipt No.: <label style='font-weight: bold;'></label></td>");
            html.Append("<td style='font-size: 6pt; text-align: left;'>Party Code:<label style='font-weight: bold;'>" + "</label></td></tr>");
            // html.Append("<tr><td style='font-size: 6pt; text-align: left;' colspan='8' width='75%'>Remarks<label style='font-weight: bold;'>" + lstInvoice[0].Remarks.ToString() + "</label></td></tr>");

            html.Append("<tr><th style='font-size: 6pt; text-align: left;' colspan='4' width='33.33333333333333%'></th>");
            html.Append("<th style='font-size: 6pt; text-align: left;' colspan='4' width='33.33333333333333%'></th>");
            html.Append("<th style='font-size: 6pt; text-align: left;' colspan='5' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/><br/><br/>(Authorized Signatories)</th></tr>");

            html.Append("<tr><td style='font-size: 6pt; text-align: left;' colspan='6' width='50%'>*Cheques are subject to realisation</td></tr>");
            //html.Append("<td style='font-size: 6pt; text-align: left;' colspan='6' width='50%'>SAM(A/C)</td>");

            html.Append("</tbody></table>");
            html.Append("</td></tr></tbody></table>");
            /***************/

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 20f, 20f, false, true))
            {
                Rh.Version = Convert.ToDecimal(objCompany[0].Version);
                Rh.Effectlogofile = objCompany[0].Effectlogofile.ToString();
                Rh.GeneratePDF(Path, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/Pallatisation" + InvoiceID + ".pdf";
        }
        #endregion

        #region Edit Palletization Invoice
        public ActionResult EditPalletizationInvoice()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            objExp.GetPalletizationnvoiceForEdit("Pallatisation");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetPalletizationInvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetPalletizationInvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEditPalletizationDetails(int InvoiceId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetEditPalletizationDetails(InvoiceId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePalletizationInvoice(WFLD_EditPalletization vm)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            vm.PallatisationChargeDetails = JsonConvert.DeserializeObject<List<WFLD_PallasationChargeBase>>(vm.PallatisationCharge);

            string Xml = Utility.CreateXML(vm.PallatisationChargeDetails);

            obj.UpdatePalletizationInvoice(vm, Xml);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BTT Container 
        public ActionResult BTTContainer(string type = "Tax")
        {

            ViewData["InvType"] = type;
            return PartialView();
        }

        public JsonResult GetPartyNameForBTTContainer(int Page, string PartyCode)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetPartyNameForBTTContainer(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetPayerNameForBTTContainer(int Page, string PartyCode)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetPayeeNameForBTTContainer(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContainerDetailsForBTT(int ExpoterId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetSBDetailsBTTContainer(ExpoterId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetBTTContanerCharge(List<WFLD_BTTContainerDetails> vm, string InvoiceDate, String InvoiceType, int PartyId, int InvoiceId, int PayeeId, string SEZ)
        {
            WFLD_ExportRepository objRepository = new WFLD_ExportRepository();
            string ContainerXML = UtilityClasses.Utility.CreateXML(vm);
            objRepository.GetBTTPaymetSheetCharge(ContainerXML, InvoiceDate, InvoiceType, PartyId, InvoiceId, PayeeId, SEZ);
            var Output = (WFLD_BBTContainer)objRepository.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "BTTCon";

            var cont = Output.lstPostPaymentCont.Select(x => x.CFSCode).Distinct().ToList();
            cont.ForEach(item =>
            {
                var obj = new WFLD_ExpContainer();
                obj.CFSCode = item;
                obj.ContainerNo = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ContainerNo;
                obj.Size = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().Size.ToString();
                obj.IsReefer = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Reefer);
                obj.Insured = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.Insured);
                obj.RMS = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Max(x => x.RMS);
                obj.CargoType = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).Min(x => x.CargoType);
                obj.ArrivalDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().ArrivalDate;
                //obj.StuffingDate = Output.lstPostPaymentCont.Where(x => x.CFSCode == item).FirstOrDefault().StuffingDate;
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
                // if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                //Output.ShippingLineName += item.ShippingLineName + ", ";
                // if (!Output.CHAName.Contains(item.CHAName))
                // Output.CHAName += item.CHAName + ", ";
                //  if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                // Output.ImporterExporter += item.ImporterExporter + ", ";
                if (!Output.BOENo.Contains(item.BOENo))
                    Output.BOENo += item.BOENo + ", ";
                //if (!Output.BOEDate.Contains(item.BOEDate))
                // Output.BOEDate += item.BOEDate + ", ";
                if (!Output.CFSCode.Contains(item.CFSCode))
                    Output.CFSCode += item.CFSCode + ", ";
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                //if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                // Output.DestuffingDate += item.DestuffingDate + ", ";
                // if (!Output.StuffingDate.Contains(item.StuffingDate))
                //  Output.StuffingDate += item.StuffingDate + ", ";
                // if (!Output.CartingDate.Contains(item.CartingDate))
                // Output.CartingDate += item.CartingDate + ", ";
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
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.RoundUp = 0; //Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddEditBTTContainer(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<WFLD_BBTContainer>(objForm["PaymentSheetModelJson"]);

                invoiceData.Remarks = objForm["Remarks"].ToString();
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

                if (invoiceData.ContainerDetails != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.ContainerDetails);
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
                string Sez = Convert.ToString(objForm["SezValue"]);
                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
                objChargeMaster.AddEditCBTTInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BTTCon", Sez);


                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }



        public ActionResult GetBTTContainerList(int EditFlag = 0)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            List<WFLD_BTTContainerInvoiceList> lstBTTContainerInvoiceList = new List<WFLD_BTTContainerInvoiceList>();
            obj.GetAllBTTContainerInvoiceList(EditFlag);
            if (obj.DBResponse.Data != null)
            {
                lstBTTContainerInvoiceList = (List<WFLD_BTTContainerInvoiceList>)obj.DBResponse.Data;
            }

            return PartialView("GetBTTContainerList", lstBTTContainerInvoiceList);
        }





        public ActionResult GetBTTContainerSearchList(string Module, int EditFlag = 0, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            List<WFLD_BTTContainerInvoiceList> lstBTTContainerInvoiceList = new List<WFLD_BTTContainerInvoiceList>();
            obj.GetBTTInvoiceList(Module, EditFlag, ContainerNo, InvoiceNo, InvoiceDate);
            if (obj.DBResponse.Data != null)
            {
                lstBTTContainerInvoiceList = (List<WFLD_BTTContainerInvoiceList>)obj.DBResponse.Data;
            }

            return PartialView("GetBTTContainerList", lstBTTContainerInvoiceList);
        }


        #endregion
        #region Edit BTT Container
        public ActionResult EditBTTContainer()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();

            objExp.GetBTTContnvoiceForEdit("BTTCon");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetBTTContInvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetBTTContInvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEditBTTContainerDetails(int InvoiceId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetEditBTTContainerDetails(InvoiceId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region CCIN Invoice
        [HttpGet]
        public ActionResult ListOfCCIN(string Module, int EditFlag = 0, string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.ListOfCCIN(Module, EditFlag, InvoiceNo, InvoiceDate);
            List<WFLD_CCINPrint> obj = new List<WFLD_CCINPrint>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLD_CCINPrint>)objER.DBResponse.Data;
            return PartialView("ListOfCCIN", obj);
        }
        #endregion

        #region Edit CCIN Payment Sheet
        public ActionResult EditCCINPaymentSheet()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();

            objExp.GetCCINnvoiceForEdit("CCIN");
            if (objExp.DBResponse.Status > 0)
                ViewBag.InvoiceList = JsonConvert.SerializeObject(objExp.DBResponse.Data);
            else
                ViewBag.InvoiceList = null;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetCCINInvoiceDetails(int InvoiceId)
        {
            WFLD_ExportRepository objImport = new WFLD_ExportRepository();
            objImport.GetCCINInvoiceDetails(InvoiceId);

            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEditCCINDetails(int InvoiceId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetEditCCINDetails(InvoiceId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CreateReeferPluginRequest


        public ActionResult CreateReeferPluginRequest(string type = "Tax")
        {
            ViewData["InvType"] = type;

            return PartialView();
        }


        public JsonResult GetPartyList(string PartyName, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentPartyForPage(PartyName, Page);
            if (objExport.DBResponse.Data != null)
            {
                return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetExporterList(string ExporterName, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPaymentExporterForPage(ExporterName, Page);
            if (objExport.DBResponse.Data != null)
            {
                return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetReeferPluginRequest()
        {
            WFLD_ExportRepository ObjIR = new WFLD_ExportRepository();
            ObjIR.GetReeferPluginRequest();
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReeferPaymentSheet(string InvoiceDate, string InvoiceType, int PartyId, List<WFLD_ContainerDetailsReefer> lstPaySheetContainer, int InvoiceId, int PayeeId, string PayeeName, string SEZ,
            string ExportUnder = "")
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                lstPaySheetContainer.ForEach(x =>
                {
                    x.PlugInDatetime = Convert.ToDateTime(x.PlugInDatetime).ToString("yyyy-MM-dd HH:mm:ss");
                    x.PlugOutDatetime = Convert.ToDateTime(x.PlugOutDatetime).ToString("yyyy-MM-dd HH:mm:ss");
                });
                XMLText = Utility.CreateXML(lstPaySheetContainer.ToList());
            }
            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            objPpgRepo.GetReeferPaymentSheet(InvoiceDate, InvoiceType, PartyId, XMLText, InvoiceId, PartyId, SEZ, ExportUnder);
            var Output = (WFLD_ReeferInv)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new WFLD_PostPaymentContainerRef
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

            });
            Output.AllTotal = Convert.ToDecimal(Output.lstPostPaymentChrg.Sum(o => o.Total));
            Output.InvoiceAmt = Convert.ToDecimal(Output.lstPostPaymentChrg.Sum(o => o.Total));
            Output.RoundUp = 0;// Convert.ToDecimal(Output.InvoiceAmt - Output.AllTotal);

            return Json(Output, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetContainerDetforReefer(int Id)
        {
            WFLD_ExportRepository objPpgRepo = new WFLD_ExportRepository();
            objPpgRepo.GetContainerDetforReeferPayment(Id);

            return Json(objPpgRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditReeferPaymentSheet(WFLD_ReeferInv objForm)
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

                WFLD_ExportRepository objChargeMaster = new WFLD_ExportRepository();
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

        [HttpGet]
        public ActionResult GetReeferInvoiceList(string Module, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetReeferInvoiceList(Module, ContainerNo, InvoiceNo, InvoiceDate);
            List<WFLD_ReeferInvoiceList> lstReeferInvoiceLst = new List<WFLD_ReeferInvoiceList>();
            if (obj.DBResponse.Status == 1)
            {
                lstReeferInvoiceLst = (List<WFLD_ReeferInvoiceList>)obj.DBResponse.Data;
            }
            return PartialView("GetReeferInvoiceList", lstReeferInvoiceLst);
        }


        #endregion


        #region Ship Bill Amendment

        public ActionResult Amendment()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
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


            objER.GetInvoiceListForShipbillAmend();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfInv = JsonConvert.SerializeObject(objER.DBResponse.Data);
            }
            else
            {
                ViewBag.ListOfInv = null;
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

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSBDetailsBySBNo(string SBid, string SbDate)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
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
        public JsonResult SaveAmendement(List<WFLD_OldInfoSb> vm, List<WFLD_NewInfoSb> newvm, string Date, string AmendmentNO, int InvoiceId, string InvoiceNo, string InvoiceDate, string FlagMerger, int NoOfGround)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            if (FlagMerger == "Split" && vm.Count > 1)
            {
                return Json(new { Message = "Only One Shipbill Split", Status = 0 }, JsonRequestBehavior.AllowGet);

            }
            else if (FlagMerger == "Merger" && vm.Count == 1)
            {
                return Json(new { Message = "Merge Operation Can't Be Done With Single Shipping Bill", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (FlagMerger == "Merger" && newvm.Count > 1)
            {
                return Json(new { Message = "Merge Operation Can't Be Done With Multiple New Shipping Bill", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (FlagMerger == "Split" && NoOfGround > 1)
            {
                return Json(new { Message = "Split Operation Can't Be Done With Multiple Godown", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (vm.Sum(x => Convert.ToDecimal(x.OldPkg)) > newvm.Sum(x => Convert.ToDecimal(x.NewInfoPkg)))
            {
                return Json(new { Message = "Old SB Pkg and New SB Pkg  should be same  ", Status = 0 }, JsonRequestBehavior.AllowGet);
            }
            //else if (vm.Sum(x => Convert.ToDecimal(x.OldWeight)) != newvm.Sum(x => Convert.ToDecimal(x.NewInfoWeight)))
            //{
            //    return Json(new { Message = "Old SB Weight and New SB Weight  should be same  ", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}
            //else if (vm.Sum(x => Convert.ToDecimal(x.OldArea)) != newvm.Sum(x => Convert.ToDecimal(x.NewInfoArea)))
            //{
            //    return Json(new { Message = "Old SB Area and New SB Area  should be same  ", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}
            //else if (vm.Sum(x => Convert.ToDecimal(x.OldFOB)) != newvm.Sum(x => Convert.ToDecimal(x.NewInfoFOB)))
            //{
            //    return Json(new { Message = "Old SB FOB and New SB FOB should be same ", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}

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

            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAmenSBDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAmendDetailsByAmendNo(string AmendNo)
        {

            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAmenDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SubmitAmedData(WFLD_AmendmentViewModel vm)
        {

            //if(vm.OldPkg>vm.Pkg)
            //{
            //    return Json(new { Message = "New pkg should be greater then or equal to old pkg", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}
            //if (vm.OldWeight > vm.Weight)
            //{
            //    return Json(new { Message = "New Weight should be greater then or equal to old Weight", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}
            //if ( Convert.ToDecimal(vm.OldFOB) > Convert.ToDecimal(vm.FOB))
            //{
            //    return Json(new { Message = "New FOB should be  greater then or equal to old FOB", Status = 0 }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.AddEditShipAmendment(vm);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            //}

        }
        [HttpGet]
        public ActionResult ListofAmendData()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_AmendmentViewModel> lstdata = new List<WFLD_AmendmentViewModel>();
            objER.ListForShipbillAmend();
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<WFLD_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }
        [HttpGet]
        public ActionResult ViewAmendData(int id)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            WFLD_AmendmentViewModel obj = new WFLD_AmendmentViewModel();
            objER.GetShipbillAmendDet(id);
            if (objER.DBResponse.Status == 1)
            {
                obj = (WFLD_AmendmentViewModel)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ViewMergeSplitData(string AmendNo)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_Amendment> obj = new List<WFLD_Amendment>();
            objER.GetAmenDetailsByAmendNO(AmendNo);
            if (objER.DBResponse.Status == 1)
            {
                obj = (List<WFLD_Amendment>)objER.DBResponse.Data;
            }
            return PartialView(obj);
        }
        [HttpGet]
        public ActionResult ListofMergeSplitAmendData()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_AmendmentViewModel> lstdata = new List<WFLD_AmendmentViewModel>();
            objER.GetAmenSBDetailsByAmendNO("");
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<WFLD_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        public JsonResult GetAllCommodityDetailsForAmendmend(string CommodityName, int Page)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetAllCommodityForPageAmendment(CommodityName, Page);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSBDetailsBySBNoAmendment(string SBid, string SbDate)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAmenSBDetailsBySbNoDateByAmend(SBid, SbDate);
            if (obj.DBResponse.Status > 0)
            {
                return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }



        }


        #endregion

        #region IWB Details
        public ActionResult CreateIWBDetails()
        {
            return PartialView();
        }
        public ActionResult GetAllIWBDetailsList()
        {
            WFLD_ExportRepository IWBDtl = new WFLD_ExportRepository();
            IWBDtl.GetAllIWBDetailsList(0);
            List<WFLDIWBDetails> listIWBDetails = new List<WFLDIWBDetails>();

            if (IWBDtl.DBResponse.Data != null)
            {
                listIWBDetails = (List<WFLDIWBDetails>)IWBDtl.DBResponse.Data;
            }
            return PartialView("IWBDetailsList", listIWBDetails);

        }

        public ActionResult GetLoadMoreIWBDetailsList(int Page)
        {
            WFLD_ExportRepository IWBDtl = new WFLD_ExportRepository();
            IWBDtl.GetAllIWBDetailsList(Page);
            List<WFLDIWBDetails> listIWBDetails = new List<WFLDIWBDetails>();

            if (IWBDtl.DBResponse.Data != null)
            {
                listIWBDetails = (List<WFLDIWBDetails>)IWBDtl.DBResponse.Data;
            }
            return Json(listIWBDetails, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetIWBGatePass()
        {
            WFLD_ExportRepository ObjETR = new WFLD_ExportRepository();

            List<dynamic> objImp2 = new List<dynamic>();
            ObjETR.GetIWBGatePassLst();
            if (ObjETR.DBResponse.Data != null)
            {
                ((List<WFLDIWBDetails>)ObjETR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { GatePassId = item.GatePassId, GatePassNo = item.GatePassNo });
                });

            }

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetIWBDetailsByGatePassId(int GatePassId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetIWBDetailsByGatePassId(GatePassId);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        //[CustomValidateAntiForgeryToken]
        public ActionResult AddEditIWBDetails(WFLDIWBDetails objIWBD)
        {
            WFLD_ExportRepository objIWB = new WFLD_ExportRepository();
            objIWB.AddEditIWBDetails(objIWBD, ((Login)(Session["LoginUser"])).Uid);

            return Json(objIWB.DBResponse);

        }


        public ActionResult EditIWBDetailsById(int IWBId, string IsOperation)
        {
            WFLDIWBDetails objIWB = new WFLDIWBDetails();
            WFLD_ExportRepository obj = new WFLD_ExportRepository();

            if (IWBId > 0)
            {
                obj.GetIWBDetailsById(IWBId);
                if (obj.DBResponse.Data != null)
                    objIWB = (WFLDIWBDetails)obj.DBResponse.Data;
                objIWB.IsOperation = IsOperation;
            }
            return PartialView("CreateIWBDetails", objIWB);

        }

        public ActionResult ViewIWBDetailsById(int IWBId, string IsOperation)
        {
            WFLDIWBDetails objIWB = new WFLDIWBDetails();
            WFLD_ExportRepository obj = new WFLD_ExportRepository();

            if (IWBId > 0)
            {
                obj.GetIWBDetailsById(IWBId);
                if (obj.DBResponse.Data != null)
                    objIWB = (WFLDIWBDetails)obj.DBResponse.Data;
                //   objIWB.ISOperation = ISOperation;
            }
            return PartialView("CreateIWBDetails", objIWB);

        }
        [HttpGet]
        public ActionResult SearchIWBDetailsList(string SearchValue)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLDIWBDetails> lstIWBDetails = new List<WFLDIWBDetails>();
            objER.SearchIWBDetailsList(SearchValue);
            if (objER.DBResponse.Data != null)
                lstIWBDetails = (List<WFLDIWBDetails>)objER.DBResponse.Data;
            return PartialView("IWBDetailsList", lstIWBDetails);
        }
        #endregion

        #region  Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateContainerStuffingApproval()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
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
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPortOfCall(string PortCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchNextPortOfCallByPortCode(string PortCode)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadNextPortOfCall(string PortCode, int Page)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetNextPortOfCallForPage(PortCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditContainerStuffingApproval(PortOfCall objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository objCR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                        //if (output == "Success")
                        //{
                            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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
            catch(Exception ex)
            {
                return Json(new { Status =0,  Message = "CIM SF File Send Fail." });
            }
           


        }


   
        #endregion

        #region Download SCMTR File
        public FileResult DownloadFile(string Path,string FileName)
        {
            return File(Path, "application/json", FileName);
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                        if(JsonFile=="")
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
                    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                    objExport.GetCIMASRDetailsUpdateStatus(ContainerStuffingId);

                    return Json(new { Status = 1, Message = "CIM ASR File Send Successfully." });
                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch(Exception ex)
            {
                return Json(new { Status = 1, Message = "CIM ASR File Send Fail." });
            }

        }

        #endregion

        #region  Loaded Container Stuffing Approval

        [HttpGet]
        public ActionResult CreateLoadContainerStuffingApproval()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                WFLD_ExportRepository objCR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                int k = 0;
                int j = 1;
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();               
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
                    WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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


        #region Bulk SF
        public ActionResult BulkSF()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult BulkSF(WFLD_BulkSF vm)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetBulkSFDetails(vm.PeriodFrom, vm.PeriodTo);
            List<WFLD_BulkSF> lstSF = new List<WFLD_BulkSF>();
            if(obj.DBResponse.Status==1)
            {
                lstSF = (List<WFLD_BulkSF>)obj.DBResponse.Data;
            }
            return Json(lstSF,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public async Task<JsonResult> SendBulkSF(List<WFLD_BulkSF> vm)
        {
            foreach(var i in vm)
            {
                try
                {
                    log.Error("SendSF Method Start .....");

                    int k = 0;
                    int j = 1;
                    WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                    // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
                    log.Error("Repository Call .....");
                    ObjER.GetBulkCIMSFDetails(i.CFSCode, "F");
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
                            //using (var client = new HttpClient())
                            //{
                            //    log.Error("Json String Before Post api url:" + apiUrl);
                            //    HttpResponseMessage response = await client.PostAsync(apiUrl, data);
                            //    log.Error("Json String after Post:");
                            //    string content = await response.Content.ReadAsStringAsync();
                            //    log.Error("After Return Response:" + content);
                            //    //log.Info(content);
                            //    JObject joResponse = JObject.Parse(content);
                            //    log.Error("After Return Response Value:" + joResponse);
                            //    var status = joResponse["Status"];
                            //    log.Error("Status:" + status);




                            //    //For Block If Develpoment Done Then Unlock

                            //}







                           
                                WFLD_ExportRepository objExport = new WFLD_ExportRepository();
                                objExport.GetBulkCIMSFDetailsUpdateStatus(i.CFSCode);
                                //return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                         
                        }


                       // return Json(new { Status = 1, Message = "CIM SF File Send Successfully." });
                    }
                    else
                    {
                       // return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                    }
                }
                catch (Exception ex)
                {
                   // return Json(new { Status = 0, Message = "CIM SF File Send Fail." });
                }


            }

            return Json("CIM SF File Send Successfully.",JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region ACTUAL ARRIVAL DATE AND TIME 
        [HttpGet]
        public ActionResult ActualArrivalDateTime()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            Wfld_ActualArrivalDatetime objActualArrival = new Wfld_ActualArrivalDatetime();

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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchContainer(string ContainerBoxSearch, int Page)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetContainerNoForActualArrival(ContainerBoxSearch, Page);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditActualArrivalDatetime(Wfld_ActualArrivalDatetime objActualArrivalDatetime)
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            List<Wfld_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Wfld_ActualArrivalDatetime>();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            //objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Role.RoleId, ((Login)Session["LoginUser"]).Uid, 0);
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, 0);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Wfld_ActualArrivalDatetime>)objER.DBResponse.Data;
            return PartialView(lstActualArrivalDatetime);
        }

        [HttpGet]
        public JsonResult EditActualArrivalDatetime(int actualArrivalDatetimeId)
        {
            List<Wfld_ActualArrivalDatetime> lstActualArrivalDatetime = new List<Wfld_ActualArrivalDatetime>();
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            objER.GetListOfArrivalDatetime(((Login)Session["LoginUser"]).Uid, actualArrivalDatetimeId);
            if (objER.DBResponse.Data != null)
                lstActualArrivalDatetime = (List<Wfld_ActualArrivalDatetime>)objER.DBResponse.Data;
            //return Json(lstActualArrivalDatetime);
            return Json(lstActualArrivalDatetime, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SendAT(string CFSCode)
        {

    
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
                        WFLD_ExportRepository objExport = new WFLD_ExportRepository();
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

        #region  Loaded Container SF Send

        [HttpGet]
        public ActionResult CreateLoadContainerSF()
        {
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();
            WFLD_LoadContSF ObjPC = new WFLD_LoadContSF();
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
            WFLD_LoadContSF ObjStuffing = new WFLD_LoadContSF();

            if (LoadContReqId > 0)
            {
                WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
                ObjER.GetLoadContainerStuffingSFById(LoadContReqId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjStuffing = (WFLD_LoadContSF)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjStuffing);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditLoadContainerStuffingSF(WFLD_LoadContSF objPortOfCall)
        {
            if (ModelState.IsValid)
            {
                WFLD_ExportRepository objCR = new WFLD_ExportRepository();
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
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            List<WFLD_LoadContSF> LstStuffingApproval = new List<WFLD_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(0, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<WFLD_LoadContSF>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfLoadContainerStuffingSF", LstStuffingApproval);
        }
        [HttpGet]
        public JsonResult LoadMoreLoadContainerStuffingSFist(int Page, string SearchValue = "")
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            var LstStuffingApproval = new List<WFLD_LoadContSF>();
            ObjER.ListofLoadContainerStuffingSF(Page, SearchValue);
            if (ObjER.DBResponse.Data != null)
            {
                LstStuffingApproval = (List<WFLD_LoadContSF>)ObjER.DBResponse.Data;
            }
            return Json(LstStuffingApproval, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewLoadContainerStuffingSF(int ApprovalId)
        {
            WFLD_LoadContSF ObjStuffing = new WFLD_LoadContSF();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetLoadContainerStuffingSFById(ApprovalId);
            if (ObjER.DBResponse.Data != null)
            {
                ObjStuffing = (WFLD_LoadContSF)ObjER.DBResponse.Data;
            }
            return PartialView(ObjStuffing);
        }


        #endregion



        #region CCIN Invoice For External User
        [HttpGet]
        public ActionResult CreateCCINPaymentSheetForExternalUser(string type = "Tax")
        {
            ViewData["InvType"] = type;
            WFLD_ExportRepository objExp = new WFLD_ExportRepository();         
            ViewBag.PaymentParty = null;
            ViewBag.PaymentPayer = null;
            Login objLogin = (Login)Session["LoginUser"];
            ViewBag.PayeeName = objLogin.Name;
            ViewBag.PayeeId = objLogin.EximTraderId;




            return PartialView();
        }










        #endregion

        #region CCIN Invoice For ExternalUser
        [HttpGet]
        public ActionResult ListOfCCINForExternalUser(string Module, int EditFlag = 0, string InvoiceNo = null, string InvoiceDate = null)
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            objER.ListOfCCINForExternalUser(Module, objLogin.Uid, EditFlag, InvoiceNo, InvoiceDate);
            List<WFLD_CCINPrint> obj = new List<WFLD_CCINPrint>();
            if (objER.DBResponse.Data != null)
                obj = (List<WFLD_CCINPrint>)objER.DBResponse.Data;
            return PartialView("ListOfCCIN", obj);
        }
        #endregion

        #region Stuffing Plan
        public ActionResult StuffingPlan()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetCartingNoForStuffingPlan()
        {
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            ObjER.GetCartRegNoForStuffingPlan(((Login)(Session["LoginUser"])).EximTraderId);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.CartingRegNoList = JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            }
            else
            {
                ViewBag.CartingRegNoList = null;
            }
            //  WFLD_ExportRepository objRepo = new WFLD_ExportRepository();



            // if (objRepo.DBResponse.Data != null)
            //{
            //   ViewBag.ListOfCHA = JsonConvert.SerializeObject(objRepo.DBResponse.Data);
            // }
            return Json(ViewBag.CartingRegNoList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
      

        public JsonResult AddEditStuffingPlan(WFLD_StuffingPlan vm)
        {
            log.Info("Method AddEditStuffingPlan started");
            try
            {
                string XML = "";
                Login objLogin = (Login)Session["LoginUser"];
                vm.Uid = objLogin.Uid;
                List<WFLD_StuffingPlanDtl> LstStuffing = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLD_StuffingPlanDtl>>(vm.StuffingPlanDtl);
                XML = Utility.CreateXML(LstStuffing);

                log.Info("XML string :"+ XML);

                WFLD_ExportRepository obj = new WFLD_ExportRepository();
                log.Info("Before  AddEditStuffingPlan ExportRepository");
                obj.AddEditStuffingPlan(vm, XML);
                log.Info("After  AddEditStuffingPlan ExportRepository");

                log.Info("Method AddEditStuffingPlan End");
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error("Err  :"+ex.Message + "\r\n"+ex.StackTrace);
                return Json("", JsonRequestBehavior.AllowGet);
            }
           
        }


        [HttpGet]
        public ActionResult StuffingPlanList()
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAllStuffingPlan(((Login)(Session["LoginUser"])).Uid);
            List<WFLD_StuffingPlan> lstStuffingPlan = new List<WFLD_StuffingPlan>();
            if(obj.DBResponse.Data!=null)
            {
                lstStuffingPlan = (List<WFLD_StuffingPlan>)obj.DBResponse.Data;
            }
            return PartialView(lstStuffingPlan);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditStuffingPlanSubmit(int StuffingPlanId)
        {
            Login objLogin = (Login)Session["LoginUser"];           
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.AddEditStuffingPlanSubmit(StuffingPlanId, objLogin.Uid);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditStuffingPlan(int StuffingPlanId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.EditStuffingPlan(StuffingPlanId);
            WFLD_StuffingPlan lstStuffingPlan = new WFLD_StuffingPlan();
            if(obj.DBResponse.Data!=null)
            {
                lstStuffingPlan = (WFLD_StuffingPlan)obj.DBResponse.Data;
                lstStuffingPlan.StuffingPlanDtl = JsonConvert.SerializeObject(lstStuffingPlan.lstStuffingPlan);
            }

            return PartialView(lstStuffingPlan);
        }
        public ActionResult ViewStuffingPlan(int StuffingPlanId)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.EditStuffingPlan(StuffingPlanId);
            WFLD_StuffingPlan lstStuffingPlan = new WFLD_StuffingPlan();
            if (obj.DBResponse.Data != null)
            {
                lstStuffingPlan = (WFLD_StuffingPlan)obj.DBResponse.Data;
                lstStuffingPlan.StuffingPlanDtl = JsonConvert.SerializeObject(lstStuffingPlan.lstStuffingPlan);
            }

            return PartialView(lstStuffingPlan);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteStuffingPlanSubmit(int StuffingPlanId)
        {
            Login objLogin = (Login)Session["LoginUser"];
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.DeleteStuffingPlanSubmit(StuffingPlanId, objLogin.Uid);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetStuffingPlanNoForRequest()
        {
            List<WFLD_StuffingPlan> lstStuffingPlan = new List<WFLD_StuffingPlan>();
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAllStuffingPlanForRequest();
            if(obj.DBResponse.Data!=null)
            {
                lstStuffingPlan = (List<WFLD_StuffingPlan>)obj.DBResponse.Data;
            }
            return Json(lstStuffingPlan, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStuffingPlanNoDetails(int StuffingPlanId)
        {
            WFLD_StuffingPlan lstStuffingPlan = new WFLD_StuffingPlan();
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAllStuffingPlanDetailsForRequest(StuffingPlanId);           
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Ship Bill Amendment Date

        public ActionResult AmendmentDate()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();

            objER.GetSBListAmdPageWise("", 0);
            if (objER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfSBNo = Jobject["LstSB"];
                ViewBag.StateShipbill = Jobject["State"];
            }
            else
            {
                ViewBag.ListOfSBNo = null;
            }

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSBDetailsBySBNoDate(string SBid, string SbDate)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
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



        [HttpGet]
        public ActionResult GetSBNoDetailsAmdBySBId(int SBId)
        {
            WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objExport.GetSBNoDetailsAmdBySBId(SBId);
            return Json(objExport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSBNoDate(WFLD_SBAMD model)
        {
            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.UpdateSBDate(model.SBNo, model.SBDate, model.OLDSBDATE);
            if (obj.DBResponse.Status > 0)
            {
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }



        }
        [HttpGet]
        public ActionResult ListOfSBNO()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> lstCCINEntry = new List<WFLD_CCINEntry>();
            objER.GetAllSBNO();
            if (objER.DBResponse.Data != null)
                lstCCINEntry = (List<WFLD_CCINEntry>)objER.DBResponse.Data;
            return PartialView(lstCCINEntry);
        }


        [HttpGet]
        public JsonResult LoadMoreSBNOList(string Page)
        {
            WFLD_ExportRepository ObjCR = new WFLD_ExportRepository();
            List<WFLD_CCINEntry> LstJO = new List<WFLD_CCINEntry>();
            ObjCR.GetAllSBNOForPage(Convert.ToInt32(Page));
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<WFLD_CCINEntry>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAmendDetailsDate(string AmendNo)
        {

            WFLD_ExportRepository obj = new WFLD_ExportRepository();
            obj.GetAmenSBDetailsByAmendNO(AmendNo);

            return Json(obj.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ListofAmendDataDate()
        {
            WFLD_ExportRepository objER = new WFLD_ExportRepository();
            List<WFLD_AmendmentViewModel> lstdata = new List<WFLD_AmendmentViewModel>();
            objER.ListForShipbillAmend();
            if (objER.DBResponse.Status == 1)
            {
                lstdata = (List<WFLD_AmendmentViewModel>)objER.DBResponse.Data;
            }
            return PartialView(lstdata);
        }

        #endregion

     

    }
}
