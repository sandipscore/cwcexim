using CwcExim.Areas.GateOperation.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using SCMTRLibrary;
using System.IO;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class Wlj_GatePassController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Gate Pass
        [HttpGet]
        public ActionResult CreateGatePass()
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            objGPR.InvoiceListForGatePass();
            if (objGPR.DBResponse.Data != null)
                ViewBag.InvoiceNoList = new SelectList((List<InvoiceNoList>)objGPR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            else
                ViewBag.InvoiceNoList = null;
            objGPR.GetPortOfLoading();
            if (objGPR.DBResponse.Data != null)
                ViewBag.PortNameList = new SelectList((List<Port>)objGPR.DBResponse.Data, "PortId", "PortName");
            else
                ViewBag.PortNameList = null;
            Wlj_GatePass objGP = new Wlj_GatePass();
            objGPR.GetServerDate();
            object datetime = null;
            if (objGPR.DBResponse.Data != null)
            {
                datetime = objGPR.DBResponse.Data;
                var joject = JsonConvert.SerializeObject(datetime);
                var jobj = Newtonsoft.Json.Linq.JObject.Parse(joject);
                string[] parseDate = jobj["date"].ToString().Split('/');
                DateTime exitDt = new DateTime(Convert.ToInt32(parseDate[2]), Convert.ToInt32(parseDate[1]), Convert.ToInt32(parseDate[0])).AddDays(1);
                objGP.GatePassDate = jobj["date"].ToString();
            }
            return PartialView(objGP);
        }
        [HttpGet]
        public ActionResult EditGatePass(int GatePassId)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            objGPR.GetPortOfLoading();
            if (objGPR.DBResponse.Data != null)
                ViewBag.PortNameList = new SelectList((List<Port>)objGPR.DBResponse.Data, "PortId", "PortName");
            else
                ViewBag.PortNameList = null;
            Wlj_GatePass objGP = new Wlj_GatePass();
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId);
            if (objGPR.DBResponse.Data != null)
                objGP = (Wlj_GatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpGet]
        public ActionResult ViewGatePass(int GatePassId)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            Wlj_GatePass objGP = new Wlj_GatePass();
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId);
            if (objGPR.DBResponse.Data != null)
                objGP = (Wlj_GatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteGatePass(int GatePassId)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            if (GatePassId > 0)
                objGPR.DeleteGatePass(GatePassId);
            return Json(objGPR.DBResponse);
        }
        [HttpGet]
        public JsonResult GetDetAgainstInvoice(int InvoiceId)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            if (InvoiceId > 0)
                objGPR.DetailsForGP(InvoiceId);
            return Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditGatePass(Wlj_GatePass objGP)
        {
            if (objGP.Module == "IMPDeli" || objGP.Module == "IMPYard" || objGP.Module == "EC" || objGP.Module == "IMPDestuff" || objGP.Module == "BNDadv" || objGP.Module == "BND")
            {
                ModelState.Remove("DepartureDate");
                ModelState.Remove("ArrivalDate");

            }
            if (ModelState.IsValid)
            {
                List<ContainerDet> lstContDet = new List<ContainerDet>();
                string XMLdata = null;
                if (objGP.StringifyData != null)
                {
                    lstContDet = JsonConvert.DeserializeObject<List<ContainerDet>>(objGP.StringifyData);
                    XMLdata = Utility.CreateXML(lstContDet);
                }
                List<WljGatepassVehicle> lstVehicle = new List<WljGatepassVehicle>();
                string VehicleXML = "";
                if (objGP.VehicleXml != null)
                {
                    lstVehicle = JsonConvert.DeserializeObject<List<WljGatepassVehicle>>(objGP.VehicleXml);
                    VehicleXML = Utility.CreateXML(lstVehicle);
                }
                Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
                objGPR.AddEditGatePass(objGP, XMLdata, VehicleXML);
                return Json(objGPR.DBResponse);
            }
            else
            {
                return Json(new { Status = -1 });
            }
        }
        [HttpGet]
        public ActionResult ListOfGatePass()
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            List<ListOfWljGP> lstGP = new List<ListOfWljGP>();
            objGPR.ListOfGatePass();
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<ListOfWljGP>)objGPR.DBResponse.Data;
            return PartialView(lstGP);
        }

        [HttpGet]
        public ActionResult SearchGatePass(string Value)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            List<ListOfWljGP> lstGP = new List<ListOfWljGP>();
            objGPR.SearchGatePass(Value);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<ListOfWljGP>)objGPR.DBResponse.Data;
            return PartialView("ListOfGatePass", lstGP);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GatePassPrint(int GatePassId)
        {
            Wlj_GatePassRepository objGPR = new Wlj_GatePassRepository();
            objGPR.GetDetailsForGatePassPrint(GatePassId);
            Wlj_GPHdr objGP = new Wlj_GPHdr();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (Wlj_GPHdr)objGPR.DBResponse.Data;
                if (objGP.Module == "bnd" || objGP.Module == "bndadv")
                    FilePath = GeneratingBondPDF(objGP, GatePassId);
                else
                    FilePath = GeneratingPDF(objGP, GatePassId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        private string GeneratingPDF(Wlj_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", ShippingSealNo = "", CustomSealNo = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "",FOB="",Commodity="";
            var ContainerNo = ""; var VehicleNo = "";
            objGP.lstDet.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerNo == "")
                    ContainerNo = item.ContainerNo;
                else
                    ContainerNo += "," + item.ContainerNo;
            });
            objGP.lstDet.Select(x => new { VehicleNo = x.VehicleNo }).Distinct().ToList().ForEach(item =>
            {
                if (VehicleNo == "")
                    VehicleNo = item.VehicleNo;
                else
                    VehicleNo += "," + item.VehicleNo;
            });
            objGP.lstDet.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "," + item.Vessel;
            });
            objGP.lstDet.Select(x => new { Voyage = x.Voyage }).Distinct().ToList().ForEach(item =>
            {
                if (Voyage == "")
                    Voyage = item.Voyage;
                else
                    Voyage += "," + item.Voyage;
            });
            objGP.lstDet.Select(x => new { Rotation = x.Rotation }).Distinct().ToList().ForEach(item =>
            {
                if (Rotation == "")
                    Rotation = item.Rotation;
                else
                    Rotation += "," + item.Rotation;
            });
            objGP.lstDet.Select(x => new { LineNo = x.LineNo }).Distinct().ToList().ForEach(item =>
            {
                if (LineNo == "")
                    LineNo = item.LineNo;
                else
                    LineNo += "," + item.LineNo;
            });
            objGP.lstDet.Select(x => new { Location = x.Location }).Distinct().ToList().ForEach(item =>
            {
                if (Location == "")
                    Location = item.Location;
                else
                    Location += "," + item.Location;
            });
            objGP.lstDet.Select(x => new { ShippingSealNo = x.ShippingSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingSealNo == "")
                    ShippingSealNo = item.ShippingSealNo;
                else
                    ShippingSealNo += "," + item.ShippingSealNo;
            });
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { Commodity = x.Commodity }).Distinct().ToList().ForEach(item =>
            {
                if (Commodity == "")
                    Commodity = item.Commodity;
                else
                    Commodity += "," + item.Commodity;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            if (CargoType != "")
                CargoType = CargoType + " Items";
            NoOfUnits = ((objGP.lstDet.Sum(x => x.NoOfUnits) > 0) ? objGP.lstDet.Sum(x => x.NoOfUnits).ToString() : "");
            Weight = ((objGP.lstDet.Sum(x => x.Weight) > 0) ? objGP.lstDet.Sum(x => x.Weight).ToString() : "");

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            string PortName = objGP.PortName;
            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td><td style='font-weight:600;text-align:right;'>Valid Upto</td><td><span>" + objGP.ValidTill + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Port Name</td><td  colspan='3'><br/><span>" + PortName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center;width:40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center;width:30%;'> PO<br/>CWC-"+objGP.CompanyLocation+"</th><th style='border-top:1px solid #000;text-align:center;width:40%;'> Delivered By<br/> Shed Incharge/CWC- "+objGP.CompanyLocation+"</th></tr></thead></table></td></tr></tbody></table></td></tr><tr><td colspan='2'><br/><br/><br/><br/><br/><br/><br/><br/>***Material handed over in good condition</td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/19</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + objGP.CompanyAdrress + "</span><br/><span style='font-size:12px;'>Email - " + objGP.CompanyMail + "</span><br/><label style='font-size: 14px; font-weight:bold;'>GATE PASS</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Valid Upto : <span style='font-weight:normal;'>" + objGP.ValidTill + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Port Name : <span style='font-weight:normal;'>" + PortName + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>FOB : <span style='font-weight:normal;'>" + objGP.lstDet[0].FOB + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Commodity Name : <span style='font-weight:normal;'>" + Commodity + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:50%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'>";

            html += "<thead><tr><th cellpadding='10' style='width: 10%; border-right: 1px solid #000;'>Container No</th><th cellpadding='10' style='width: 10%; border-right: 1px solid #000;'>Vechile No</th>";
            html += "<th cellpadding='10' style='width: 8%;'>No Of Units</th></tr></thead>";
            html += "<tbody>";
            objGP.lstVehicle.ToList().ForEach(item =>
            {
                html += "<tr><td cellpadding='10' style='border-right: 1px solid #000; border-top: 1px solid #000;'>"+item.ContainerNo+ "</td><td cellpadding='10' style='border-right: 1px solid #000;border-top: 1px solid #000;'>" + item.VehicleNo + "</td>";
                html += "<td cellpadding='10' style='border-top: 1px solid #000;'>"+item.Package+"</td></tr>";
            });
            html += "</tbody></table>";
                html += "</td></tr>";
            
            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO <br/> CWC -" + objGP.CompanyLocation + "</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC - " + objGP.CompanyLocation + "</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));









            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }



        [NonAction]
        private string GeneratingBondPDF(Wlj_GPHdr objGP, int GatePassId)
        {
            //  string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "";


            var ContainerNo = ""; var VehicleNo = "";
            objGP.lstDet.Select(x => new { VehicleNo = x.VehicleNo }).Distinct().ToList().ForEach(item =>
            {
                if (VehicleNo == "")
                    VehicleNo = item.VehicleNo;
                else
                    VehicleNo += "," + item.VehicleNo;
            });

            List<string> lstSB = new List<string>();

            System.Text.StringBuilder html = new System.Text.StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            html.Append("<td width='15%' align='right'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Doc No. F/CD/CFS/29</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + objGP.CompanyAdrress + "</span><br/><label style='font-size: 12px;'>" + objGP.CompanyMail + "</label><br/><label style='font-size: 14px; font-weight:bold;'>GATE PASS</label><br/><label style='font-size: 13px;'>(For Custom Bonded Goods)</label></td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:13px;' width='7%'>SL.No.</th>");
            html.Append("<td style='font-size:12px;'> <span>" + objGP.GatePassNo + " </span></td><th style='font-size:13px; text-align:right;'>Date</th>");
            html.Append("<td style='font-size:12px; width:10%;'><u>" + objGP.GatePassDate + "</u></td></tr></tbody></table></td></tr>");

            html.Append("<tr><td colspan='4' style='text-align: right;font-size:13px;'><b>Valid Upto :</b> " + objGP.ValidTill + "</td></tr>");

            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>1.</td><td colspan='1' valign='top' width='30%' style='font-size:12px;'>Vehicle No.</td><td>:</td><td colspan='2' valign='top' width='60%' style='font-size:12px;'>" + VehicleNo + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>2.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Bond No. & Date</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.BondNo + "," + objGP.BondDate + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>3.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Ex-Bond Bill of Entry No. & Date</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.BOENo + " &nbsp;" + objGP.BOEDate + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>4.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Description of Goods</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.CargoDescription + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>5.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>No. of Units</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.NoOfUnits + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>6.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Weight (No./Gross) (if applicable)</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.Weight + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>7.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Godown / Stack No.</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.GodownId + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>8.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Name of the importer</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.ImpExpName + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>9.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>CHA</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>" + objGP.CHAName + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:12px;'>10.</td><td colspan='1' valign='top' width='50%' style='font-size:12px;'>Name of the Driver</td><td>:</td><td colspan='2' valign='top' width='40%' style='font-size:12px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");

             html.Append("<tr><td colspan='12'>");
            html.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:50%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'>");

            html.Append("<thead><tr><th cellpadding='10' style='width: 10%; border-right: 1px solid #000;'>Container No</th><th cellpadding='10' style='width: 10%; border-right: 1px solid #000;'>Vechile No</th>");
            html.Append("<th cellpadding='10' style='width: 8%;'>No Of Units</th></tr></thead>");
            html.Append("<tbody>");
            objGP.lstVehicle.ToList().ForEach(item =>
            {
                html.Append ("< tr><td cellpadding='10' style='border-right: 1px solid #000; border-top: 1px solid #000;'>" + item.ContainerNo + "</td><td cellpadding='10' style='border-right: 1px solid #000;border-top: 1px solid #000;'>" + item.VehicleNo + "</td>");
                html.Append("<td cellpadding='10' style='border-top: 1px solid #000;'>" + item.Package + "</td></tr>");
            });
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4' style='font-size:12px;'>Received goods in Good Condition</td></tr>");
            html.Append("<tr><td><span><br/><br/></span></td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><th width='50%' colspan='6' style='font-size:14px; text-align: left;'>Signature of <br/> Importer / Agent</th><th width='50%' colspan='6' style='font-size:14px; text-align: right;'>Signature of <br/> Godown Incharge / Asst.</th></tr></tbody></table></td></tr>");
            html.Append("</tbody></table>");

           

            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            objGP.lstDet.Select(x => new { ContainerNo = x.ContainerNo }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerNo == "")
                    ContainerNo = item.ContainerNo;
                else
                    ContainerNo += "," + item.ContainerNo;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            if (CargoType != "")
                CargoType = CargoType + " Items";
            NoOfUnits = ((objGP.lstDet.Sum(x => x.NoOfUnits) > 0) ? objGP.lstDet.Sum(x => x.NoOfUnits).ToString() : "");
            Weight = ((objGP.lstDet.Sum(x => x.Weight) > 0) ? objGP.lstDet.Sum(x => x.Weight).ToString() : "");

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            string PortName = objGP.PortName;


            //  html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Port Name</td><td  colspan='3'><br/><span>" + PortName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center;width:40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center;width:30%;'> PO<br/> CWC CFS, Hyderabad</th><th style='border-top:1px solid #000;text-align:center;width:40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Hyderabad</th></tr></thead></table></td></tr></tbody></table></td></tr><tr><td colspan='2'><br/><br/><br/><br/><br/><br/><br/><br/>***Material handed over in good condition</td></tr></tbody></table>";

            lstSB.Add(html.ToString());

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.GeneratePDF(location, lstSB);
            }

            //  using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            //  {
            //      rp.GeneratePDF(location, html);
            //   }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult CancelGatePass(int GatePassId)
        {
            if (GatePassId > 0)
            {
                Wlj_GatePassRepository objGP = new Wlj_GatePassRepository();
                objGP.CancelGatePass(GatePassId);
                return Json(objGP.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        #endregion


        #region Send DP

        public JsonResult SendDP(int GatePassId = 0)
        {
            int k = 0;
            int j = 1;
            Wlj_GatePassRepository ObjER = new Wlj_GatePassRepository();
            // PPG_ContainerStuffingV2 ObjStuffing = new PPG_ContainerStuffingV2();
            ObjER.GetDPDetails(GatePassId, "F");
            DataSet ds = new DataSet();

            if (ObjER.DBResponse.Status == 1)
            {
                ds = (DataSet)ObjER.DBResponse.Data;

                foreach (DataRow dr in ds.Tables[7].Rows)
                {
                    string Filenm = Convert.ToString(dr["FileName"]);
                    int gatepassdtlid = Convert.ToInt32(dr["gatepassdtlid"]);
                    string JsonFile = DPJsonFormat.DPJsonCreation(ds, gatepassdtlid);
                    // string Filenm = Convert.ToString(ds.Tables[7].Rows[0]["FileName"]);



                    string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_ReportfileCIMDP"];
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
                        Wlj_GatePassRepository objExport = new Wlj_GatePassRepository();
                        objExport.GetCIMDPDetailsUpdateStatus(GatePassId);
                        return Json(new { Status = 1, Message = "CIM DP File Send Successfully." });
                    }
                    log.Info("FTP File upload has been end");
                }

                return Json(new { Status = 1, Message = "CIM DP File Send Fail." });
            }
            else
            {
                return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
            }


        }
        #endregion

    }
}