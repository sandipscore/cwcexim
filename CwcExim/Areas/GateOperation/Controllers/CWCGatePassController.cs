using CwcExim.Areas.GateOperation.Models;
using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Filters;
using CwcExim.Repositories;
using Newtonsoft.Json;
using CwcExim.UtilityClasses;
using System.Data;
using SCMTRLibrary;
using System.IO;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class CWCGatePassController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Gate Pass
        [HttpGet]
        public ActionResult CreateGatePass()
        {
            GatePassRepository objGPR = new GatePassRepository();
            objGPR.InvoiceListForGatePass();
            if (objGPR.DBResponse.Data != null)
                ViewBag.InvoiceNoList = new SelectList((List<InvoiceNoList>)objGPR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            else
                ViewBag.InvoiceNoList = null;
            GatePass objGP = new GatePass();
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
            GatePassRepository objGPR = new GatePassRepository();
            GatePass objGP = new GatePass();
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId);
            if (objGPR.DBResponse.Data != null)
                objGP = (GatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpGet]
        public ActionResult ViewGatePass(int GatePassId)
        {
            GatePassRepository objGPR = new GatePassRepository();
            GatePass objGP = new GatePass();
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId);
            if (objGPR.DBResponse.Data != null)
                objGP = (GatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteGatePass(int GatePassId)
        {
            GatePassRepository objGPR = new GatePassRepository();
            if (GatePassId > 0)
                objGPR.DeleteGatePass(GatePassId);
            return Json(objGPR.DBResponse);
        }
        [HttpGet]
        public JsonResult GetDetAgainstInvoice(int InvoiceId)
        {
            GatePassRepository objGPR = new GatePassRepository();
            if (InvoiceId > 0)
                objGPR.DetailsForGP(InvoiceId);
            return Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditGatePass(GatePass objGP)
        {
            if (objGP.Modul == "IMPDeli" || objGP.Modul == "IMPYard" || objGP.Modul == "EC" || objGP.Modul == "IMPDestuff" || objGP.Modul == "BNDadv" || objGP.Modul == "BND")
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
                GatePassRepository objGPR = new GatePassRepository();
                objGPR.AddEditGatePass(objGP, XMLdata);
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
            GatePassRepository objGPR = new GatePassRepository();
            List<ListOfGP> lstGP = new List<ListOfGP>();
            objGPR.ListOfGatePass();
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<ListOfGP>)objGPR.DBResponse.Data;
            return PartialView(lstGP);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GatePassPrint(int GatePassId)
        {
            GatePassRepository objGPR = new GatePassRepository();
            objGPR.GetDetailsForGatePassPrint(GatePassId);
            GPHdr objGP = new GPHdr();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (GPHdr)objGPR.DBResponse.Data;
                FilePath = GeneratingPDF(objGP, GatePassId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        private string GeneratingPDF(GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "";
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
            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/><br/>Received with Intact and Good Condition</th><th style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Kolkata</th><th style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Kolkata</th></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";
            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-size:9pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'>Central Warehousing Corporation</th><th colspan='6' width='50%' style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";            
            html += "<tr><th colspan='12' style='text-align:center;font-size:12pt'>Gate Pass</th></tr>";
            html += "<tr><td><span><br/><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:9pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. <span style='font-weight:normal;'></span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. <span style='font-weight:normal;'></span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date: <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000;'>Nature of Goods <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Kolkata</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Kolkata</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "</tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult CancelGatePass(int GatePassId)
        {
            if(GatePassId>0)
            {
                GatePassRepository objGP = new GatePassRepository();
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
            GatePassRepository ObjER = new GatePassRepository();
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
                    string FTPFolderName = System.Configuration.ConfigurationManager.AppSettings["SCMTRPath"];

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

                        output = FtpFileManager.UploadFileToFtp(FTPFolderName, Filenm, buffer, "5000", FileName);
                        log.Info("FTP File read process has ended");
                    }
                    if (output == "Success")
                    {
                        GatePassRepository objExport = new GatePassRepository();
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