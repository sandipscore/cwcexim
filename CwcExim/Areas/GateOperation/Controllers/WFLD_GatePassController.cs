using CwcExim.Areas.GateOperation.Models;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
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
using CwcExim.Areas.Export.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class WFLD_GatePassController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }

        public WFLD_GatePassController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;

        }

        #region Gate Pass
        [HttpGet]
        public ActionResult CreateGatePass()
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            //WFLD_ExportRepository ObjER = new WFLD_ExportRepository();

            //objGPR.InvoiceListForGatePass();
            //if (objGPR.DBResponse.Data != null)
            //    ViewBag.InvoiceNoList = objGPR.DBResponse.Data;
            //else
            //    ViewBag.InvoiceNoList = null;

            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            /*   objGPR.InvoiceListForGatePass(ObjLogin.Uid);
               if (objGPR.DBResponse.Data != null)
                   ViewBag.InvoiceNoList = objGPR.DBResponse.Data;
               else
                   ViewBag.InvoiceNoList = null;*/
            //   objGPR.ListOfInvoiceForPage(ObjLogin.Uid,"", 0);

            //   if (objGPR.DBResponse.Data != null)
            //   {
            //     var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objGPR.DBResponse.Data);
            //     var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.lstInvoice = Jobject["lstInvoice"];
            //     ViewBag.lstState = Jobject["State"];
            // }
            WFLDGatePass objGP = new WFLDGatePass();
            objGPR.GetServerDate();
            object datetime = null;
            if (objGPR.DBResponse.Data != null)
            {
                datetime = objGPR.DBResponse.Data;
                var joject = JsonConvert.SerializeObject(datetime);
                var jobj = Newtonsoft.Json.Linq.JObject.Parse(joject);
                string[] parseDate = jobj["date"].ToString().Split('/');
                DateTime exitDt = new DateTime(Convert.ToInt32(parseDate[2]), Convert.ToInt32(parseDate[1]), Convert.ToInt32(parseDate[0])).AddDays(1);
                //objGP.GatePassDate = jobj["date"].ToString();
            }
            objGP.GatePassDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            //ObjER.ListOfPOD();
            //if (ObjER.DBResponse.Data != null)
            //{
            //    ViewBag.PortOfDispatch = new SelectList((List<CwcExim.Models.Port>)ObjER.DBResponse.Data, "PortId", "PortName");
            //}
            //else
            //{
            //    ViewBag.PortOfDispatch = null;
            //}

            return PartialView(objGP);
        }




        [HttpGet]
        public JsonResult GetInvNoforGatePass()
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];

            objGPR.ListOfInvoiceForPage(ObjLogin.Uid, "", 0);


            if (objGPR.DBResponse.Data != null)
                return Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpGet]
        public ActionResult EditGatePass(int GatePassId)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            WFLDGatePass objGP = new WFLDGatePass();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId, ObjLogin.Uid);
            if (objGPR.DBResponse.Data != null)
                objGP = (WFLDGatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpGet]
        public ActionResult ViewGatePass(int GatePassId)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            WFLDGatePass objGP = new WFLDGatePass();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            if (GatePassId > 0)
                objGPR.GetDetForGatePass(GatePassId, ObjLogin.Uid);
            if (objGPR.DBResponse.Data != null)
                objGP = (WFLDGatePass)objGPR.DBResponse.Data;
            return PartialView(objGP);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteGatePass(int GatePassId)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            if (GatePassId > 0)
                objGPR.DeleteGatePass(GatePassId);
            return Json(objGPR.DBResponse);
        }
        [HttpGet]
        public JsonResult GetDetAgainstInvoice(int InvoiceId)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            if (InvoiceId > 0)
                objGPR.DetailsForGP(InvoiceId);
            return Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditGatePass(WFLDGatePass objGP)
        {
            if (objGP.Module == "IMPDeli" || objGP.Module == "IMPYard" || objGP.Module == "EC" || objGP.Module == "ECGodn" || objGP.Module == "BNDadv" || objGP.Module == "BND")
            {
                ModelState.Remove("DepartureDate");
                ModelState.Remove("ArrivalDate");

            }


            if (ModelState.IsValid)
            {
                List<WFLD_ContainerDet> lstContDet = new List<WFLD_ContainerDet>();
                string XMLdata = null;
                if (objGP.StringifyData != null)
                {
                    lstContDet = JsonConvert.DeserializeObject<List<WFLD_ContainerDet>>(objGP.StringifyData);
                    XMLdata = Utility.CreateXML(lstContDet);
                }

                List<WFLDGatepassVehicle> lstVehicle = new List<WFLDGatepassVehicle>();
                string VehicleXML = "";
                if (objGP.VehicleXml != null)
                {
                    lstVehicle = JsonConvert.DeserializeObject<List<WFLDGatepassVehicle>>(objGP.VehicleXml);
                    VehicleXML = Utility.CreateXML(lstVehicle);
                }

                WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
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
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            List<WFLDListOfGP> lstGP = new List<WFLDListOfGP>();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGPR.ListOfGatePass(0, ObjLogin.Uid);
            //objGPR.ListOfGatePass(0);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<WFLDListOfGP>)objGPR.DBResponse.Data;
            return PartialView(lstGP);
        }

        [HttpGet]
        public ActionResult LoadMoreListOfGatePass(int Page)
        {
            //TempData["lstFlag"] = lstFlag;
            //TempData.Keep();
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            List<WFLDListOfGP> lstGP = new List<WFLDListOfGP>();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGPR.LoadMoreListOfGatePass(Page, ObjLogin.Uid);
            // objGPR.LoadMoreListOfGatePass(Page);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<WFLDListOfGP>)objGPR.DBResponse.Data;
            //return PartialView(lstGP);
            return Json(lstGP, JsonRequestBehavior.AllowGet);
        }
        /* [HttpPost]
         [CustomValidateAntiForgeryToken]
         public JsonResult GatePassPrint(int GatePassId)
         {
             WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
             objGPR.GetDetailsForGatePassPrint(GatePassId);
             WFLD_GPHdr objGP = new WFLD_GPHdr();
             string FilePath = "";
             if (objGPR.DBResponse.Data != null)
             {
                 objGP = (WFLD_GPHdr)objGPR.DBResponse.Data;
                 FilePath = GeneratingPDF(objGP, GatePassId);
                 return Json(new { Status = 1, Message = FilePath });
             }
             else
                 return Json(new { Status = -1, Message = "Error" });
         }*/
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GatePassPrint(int GatePassId)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            objGPR.GetDetailsForGatePassPrint(GatePassId);
            WFLD_GPHdr objGP = new WFLD_GPHdr();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (WFLD_GPHdr)objGPR.DBResponse.Data;
                if (objGP.Module == "exp")
                    FilePath = GeneratingPDFExport(objGP, GatePassId);
                else
                     if (objGP.Module == "explod")
                    FilePath = GeneratingPDFLoaded(objGP, GatePassId);
                else
                     if (objGP.Module == "btt")
                    FilePath = GeneratingPDFBTT(objGP, GatePassId);

                else
                     if (objGP.Module == "bttcon")
                    FilePath = GeneratingPDFBTTCont(objGP, GatePassId);


                else if (objGP.Module == "impdeli")
                    FilePath = GeneratingPDFImportdeli(objGP, GatePassId);
                else if (objGP.Module == "impyard")
                    FilePath = GeneratingPDFImportYard(objGP, GatePassId);
                else
                    FilePath = GeneratingPDF(objGP, GatePassId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        private string GeneratingPDF(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "";



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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
               {
                   if (CargoDescription == "")
                       CargoDescription = item.CargoDescription;
                   else
                       CargoDescription += "," + item.CargoDescription;
               });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
             {
                 if (NoOfShipBill == "")
                     NoOfShipBill = item.NoOfShipBill;
                 else
                     NoOfShipBill += "," + item.NoOfShipBill;
             });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>CFS Whitefield</span><br/><label style='font-size: 14px; font-weight:bold;'>GATE PASS</label><label style='font-size: 14px; font-weight:bold;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";


            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>ICD Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>No Of ShippingBill : <span style='font-weight:normal;'>" + NoOfShipBill + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";


            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [NonAction]
        private string GeneratingPDFExport(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "",
            Size = "", ContainerType = "", POD = "", Forwarder = "";


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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
            {
                if (NoOfShipBill == "")
                    NoOfShipBill = item.NoOfShipBill;
                else
                    NoOfShipBill += "," + item.NoOfShipBill;
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstDet.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
            {
                if (Size == "")
                    Size = item.Size;
                else
                    Size += "," + item.Size;
            });
            objGP.lstDet.Select(x => new { ContainerType = x.ContainerType }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerType == "")
                    ContainerType = item.ContainerType;
                else
                    ContainerType += "," + item.ContainerType;
            });
            objGP.lstDet.Select(x => new { POD = x.POD }).Distinct().ToList().ForEach(item =>
            {
                if (POD == "")
                    POD = item.POD;
                else
                    POD += "," + item.POD;
            });
            objGP.lstDet.Select(x => new { Forwarder = x.Forwarder }).Distinct().ToList().ForEach(item =>
            {
                if (Forwarder == "")
                    Forwarder = item.Forwarder;
                else
                    Forwarder += "," + item.Forwarder;
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS Whitefield</span><br/><label style='font-size: 7pt; font-weight:bold;'>GATE PASS</label><label style='font-size: 7pt; font-weight:bold; text-transform:uppercase;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "<tr><th colspan='6' width='50%' style='font-size:8pt;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th colspan='6' width='50%'  style='font-size:8pt; text-align:right;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";

            html += "<tr><th colspan='12' style='font-size:8pt;'>Movement Of Loaded Container From CWC To " + POD + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>NVOCC/FF : <span style='font-weight:normal;'>" + Forwarder + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>I.S.O Code : <span style='font-weight:normal;'></span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Size : <span style='font-weight:normal;'>" + Size + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>POD : <span style='font-weight:normal;'>" + POD + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Movement Order No. : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Temp To Be Maintained <br/> (For Reffer Container) : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Custom Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SLA Seal No. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container Type : <span style='font-weight:normal;'>" + ContainerType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No Of ShippingBill : <span style='font-weight:normal;'>" + NoOfShipBill + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><th colspan='12' style='text-align:right; font-size:7pt;'>Signature :</th></tr>";
            html += "</tbody></table>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            //html += "<tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";            
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";            

            //html += "<tr><td colspan='12'>";
            //html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            //html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            //html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            //html += "</thead></table>";
            //html += "</td></tr>";
            //html += "<tr><td><br/><br/><br/><br/></td></tr>";
            //html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            //html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }

        [NonAction]
        private string GeneratingPDFBTT(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "",
            Size = "", ContainerType = "", POD = "", Forwarder = "";


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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
            {
                if (NoOfShipBill == "")
                    NoOfShipBill = item.NoOfShipBill;
                else
                    NoOfShipBill += "," + item.NoOfShipBill;
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstDet.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
            {
                if (Size == "")
                    Size = item.Size;
                else
                    Size += "," + item.Size;
            });
            objGP.lstDet.Select(x => new { ContainerType = x.ContainerType }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerType == "")
                    ContainerType = item.ContainerType;
                else
                    ContainerType += "," + item.ContainerType;
            });
            objGP.lstDet.Select(x => new { POD = x.POD }).Distinct().ToList().ForEach(item =>
            {
                if (POD == "")
                    POD = item.POD;
                else
                    POD += "," + item.POD;
            });
            objGP.lstDet.Select(x => new { Forwarder = x.Forwarder }).Distinct().ToList().ForEach(item =>
            {
                if (Forwarder == "")
                    Forwarder = item.Forwarder;
                else
                    Forwarder += "," + item.Forwarder;
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS Whitefield</span><br/><label style='font-size: 7pt; font-weight:bold;'>GATE PASS</label><label style='font-size: 7pt; font-weight:bold; text-transform:uppercase;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "<tr><th colspan='6' width='50%' style='font-size:8pt;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th colspan='6' width='50%'  style='font-size:8pt; text-align:right;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";

            html += "<tr><th colspan='12' style='font-size:8pt;'>Movement Of Loaded Container From CWC To " + POD + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>NVOCC/FF : <span style='font-weight:normal;'>" + Forwarder + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>I.S.O Code : <span style='font-weight:normal;'></span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Size : <span style='font-weight:normal;'>" + Size + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>POD : <span style='font-weight:normal;'>" + POD + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Movement Order No. : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Temp To Be Maintained <br/> (For Reffer Container) : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Custom Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SLA Seal No. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container Type : <span style='font-weight:normal;'>" + ContainerType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No Of ShippingBill : <span style='font-weight:normal;'>" + NoOfShipBill + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><th colspan='12' style='text-align:right; font-size:7pt;'>Signature :</th></tr>";
            html += "</tbody></table>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            //html += "<tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";            
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";            

            //html += "<tr><td colspan='12'>";
            //html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            //html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            //html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            //html += "</thead></table>";
            //html += "</td></tr>";
            //html += "<tr><td><br/><br/><br/><br/></td></tr>";
            //html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            //html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [NonAction]
        private string GeneratingPDFLoaded(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "",
            Size = "", ContainerType = "", POD = "", Forwarder = "";


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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
            {
                if (NoOfShipBill == "")
                    NoOfShipBill = item.NoOfShipBill;
                else
                    NoOfShipBill += "," + item.NoOfShipBill;
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstDet.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
            {
                if (Size == "")
                    Size = item.Size;
                else
                    Size += "," + item.Size;
            });
            objGP.lstDet.Select(x => new { ContainerType = x.ContainerType }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerType == "")
                    ContainerType = item.ContainerType;
                else
                    ContainerType += "," + item.ContainerType;
            });
            objGP.lstDet.Select(x => new { POD = x.POD }).Distinct().ToList().ForEach(item =>
            {
                if (POD == "")
                    POD = item.POD;
                else
                    POD += "," + item.POD;
            });
            objGP.lstDet.Select(x => new { Forwarder = x.Forwarder }).Distinct().ToList().ForEach(item =>
            {
                if (Forwarder == "")
                    Forwarder = item.Forwarder;
                else
                    Forwarder += "," + item.Forwarder;
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS Whitefield</span><br/><label style='font-size: 7pt; font-weight:bold;'>GATE PASS</label><label style='font-size: 7pt; font-weight:bold; text-transform:uppercase;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "<tr><th colspan='6' width='50%' style='font-size:8pt;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th colspan='6' width='50%'  style='font-size:8pt; text-align:right;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";

            html += "<tr><th colspan='12' style='font-size:8pt;'>Movement Of Loaded Container From CWC To " + POD + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>NVOCC/FF : <span style='font-weight:normal;'>" + Forwarder + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>I.S.O Code : <span style='font-weight:normal;'></span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Size : <span style='font-weight:normal;'>" + Size + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>POD : <span style='font-weight:normal;'>" + POD + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Movement Order No. : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Temp To Be Maintained <br/> (For Reffer Container) : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Custom Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SLA Seal No. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container Type : <span style='font-weight:normal;'>" + ContainerType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No Of ShippingBill : <span style='font-weight:normal;'>" + NoOfShipBill + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><th colspan='12' style='text-align:right; font-size:7pt;'>Signature :</th></tr>";
            html += "</tbody></table>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            //html += "<tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";            
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";            

            //html += "<tr><td colspan='12'>";
            //html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            //html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            //html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            //html += "</thead></table>";
            //html += "</td></tr>";
            //html += "<tr><td><br/><br/><br/><br/></td></tr>";
            //html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            //html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }



        [NonAction]
        private string GeneratingPDFBTTCont(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "",
            Size = "", ContainerType = "", POD = "", Forwarder = "";


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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
            {
                if (NoOfShipBill == "")
                    NoOfShipBill = item.NoOfShipBill;
                else
                    NoOfShipBill += "," + item.NoOfShipBill;
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstDet.Select(x => new { Size = x.Size }).Distinct().ToList().ForEach(item =>
            {
                if (Size == "")
                    Size = item.Size;
                else
                    Size += "," + item.Size;
            });
            objGP.lstDet.Select(x => new { ContainerType = x.ContainerType }).Distinct().ToList().ForEach(item =>
            {
                if (ContainerType == "")
                    ContainerType = item.ContainerType;
                else
                    ContainerType += "," + item.ContainerType;
            });
            objGP.lstDet.Select(x => new { POD = x.POD }).Distinct().ToList().ForEach(item =>
            {
                if (POD == "")
                    POD = item.POD;
                else
                    POD += "," + item.POD;
            });
            objGP.lstDet.Select(x => new { Forwarder = x.Forwarder }).Distinct().ToList().ForEach(item =>
            {
                if (Forwarder == "")
                    Forwarder = item.Forwarder;
                else
                    Forwarder += "," + item.Forwarder;
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "BTTCon")
                    InvoiceType = "(BTT Container)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS Whitefield</span><br/><label style='font-size: 7pt; font-weight:bold;'>GATE PASS</label><label style='font-size: 7pt; font-weight:bold; text-transform:uppercase;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='font-size:7pt; width: 100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "<tr><th colspan='6' width='50%' style='font-size:8pt;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th colspan='6' width='50%'  style='font-size:8pt; text-align:right;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span></th></tr>";

            html += "<tr><th colspan='12' style='font-size:8pt;'>Movement Of Loaded Container From CWC To " + POD + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>NVOCC/FF : <span style='font-weight:normal;'>" + Forwarder + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='12' width='100%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container No. : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>I.S.O Code : <span style='font-weight:normal;'></span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Size : <span style='font-weight:normal;'>" + Size + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>POD : <span style='font-weight:normal;'>" + POD + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Movement Order No. : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Type : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Temp To Be Maintained <br/> (For Reffer Container) : <span style='font-weight:normal;'></span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Custom Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>SLA Seal No. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Container Type : <span style='font-weight:normal;'>" + ContainerType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No Of ShippingBill : <span style='font-weight:normal;'>" + NoOfShipBill + "</span></th></tr>";

            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><th colspan='12' style='text-align:right; font-size:7pt;'>Signature :</th></tr>";
            html += "</tbody></table>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer/Exporter : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No./S.B. No./WR No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            //html += "<tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";            
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'> : <span style='font-weight:normal;'></span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";            

            //html += "<tr><td colspan='12'>";
            //html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            //html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            //html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            //html += "<th width='1%'></th>";
            //html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            //html += "</thead></table>";
            //html += "</td></tr>";
            //html += "<tr><td><br/><br/><br/><br/></td></tr>";
            //html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            //html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [NonAction]
        private string GeneratingPDFImportdeli(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "", NoOfShipBill = "";



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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });
            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
                else if (item.CargoType == 3)
                    CargoType = (CargoType == "" ? "ODC" : CargoType + ",ODC");
            });
            objGP.lstDet.Select(x => new { NoOfShipBill = x.NoOfShipBill }).Distinct().ToList().ForEach(item =>
            {
                if (NoOfShipBill == "")
                    NoOfShipBill = item.NoOfShipBill;
                else
                    NoOfShipBill += "," + item.NoOfShipBill;
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(CONTAINER MOVEMENT)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];
            string gpdate = objGP.GatePassDate;
            DateTime date = Convert.ToDateTime(gpdate);
            string year = Convert.ToString(date.Year);
            string yr = year.Substring(2);

            //string strIssueDateTime = CreatedTime;
            //string[] dateIssueTimeArray = strDateTime.Split(' ');
            //string strIssueDtae = dateTimeArray[0];
            //string strIssueTime = dateTimeArray[1];



            //html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='text-align:left;'>Central Warehousing Corporation</th><th style='text-align:right;'>Doc. No.F/CD/CFS/19</th></tr><tr><th colspan='2' style='text-align:center;font-size:12pt'>Gate Pass</th></tr></thead><tbody><tr><td colspan='2'><table style='width:90%;font-size:9pt;font-family:Verdana,Arial,San-serif;margin-left:5%;'><tbody><tr><td style='font-weight:600;text-align:right;'>Gate Pass No.</td><td><span>" + objGP.GatePassNo + "</span></td><td style='font-weight:600;text-align:right;'>Gate Pass Date</td><td><span>" + objGP.GatePassDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'>Vehicle No.</td><td colspan='3'><span>" + VehicleNo + "</span></td></tr><tr><td style='padding-top:40pt;padding-bottom:100pt;font-weight:600;text-align:right;'>Container No. & size</td><td colspan='3' style='padding-top:40pt;padding-bottom:100pt;'><span>" + ContainerNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Vessel Name</td><td><br/><span>" + Vessel + "</span></td><td style='font-weight:600;text-align:right;'><br/>Voyage No.</td><td><br/><span>" + Voyage + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Rotation no.</td><td><br/><span>" + Rotation + "</span></td><td style='font-weight:600;text-align:right;'><br/>Line No.</td><td><br/><span>" + LineNo + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line Seal no.</td><td><span></span></td><td style='font-weight:600;text-align:right;'><br/>Customs Seal No.</td><td><br/><span></span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Importer/Exporter</td><td colspan='3'><br/><span>" + objGP.ImpExpName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>CHA Name</td><td colspan='3'><br/><span>" + objGP.CHAName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Shipping Line</td><td colspan='3'><br/><span>" + objGP.ShippingLineName + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>BOE No./S.B. No./WR No.</td><td><br/><span>" + objGP.BOENo + "</span></td><td style='font-weight:600;text-align:right;'><br/>Date</td><td><br/><span>" + objGP.BOEDate + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>No. of Packages</td><td><br/><span>" + NoOfUnits + "</span></td><td style='font-weight:600;text-align:right;'><br/>Weight</td><td><br/><span>" + Weight + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Location Name</td><td colspan='3'><br/><span>" + Location + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Nature of Goods</td><td colspan='3'><br/><span>" + CargoType + "</span></td></tr><tr><td style='font-weight:600;text-align:right;'><br/>Remarks</td><td colspan='3'><br/><span>" + objGP.Remarks + "</span></td></tr><tr><td colspan='4' style='padding-top:60pt;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;padding-top:80pt;'><thead><tr><th style='border-top:1px solid #000;text-align:center; width: 40%;'>Signature of CHA</th><th style='border-top:1px solid #000;text-align:center; width: 30%;'> PO<br/> CWC CFS, Patparganj </th><th style='border-top:1px solid #000;text-align:center; width: 40%;'> Delivered By<br/> Shed Incharge/CWC CFS, Patparganj </th></tr><tr><td colspan='3'><br/><br/><br/><br/><br/><br/><br/><br/>****Material handed over in good condition</td></tr></thead></table></td></tr></tbody></table></td></tr></tbody></table>";

            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/CFS/" + yr + "</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>CFS Whitefield</span><br/><label style='font-size: 14px; font-weight:bold;'>GATE PASS</label><label style='font-size: 14px; font-weight:bold;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span> <span style='font-weight:normal;'>" + strTime + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>ICD Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No :<span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            // html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'></span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>BOE No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'> Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            // html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";



            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            // html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No :<span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>OBL NO :<span style='font-weight:normal;'>" + OBLNO + "</span></th>";
            // html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;border-right:1px solid #000; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";
            //  html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'><span style='font-weight:normal;'></span></th></tr>";
            // html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>ICD Code : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'></span></th></tr>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>IGM No : <span style='font-weight:normal;'></span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";


            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS, Whitefield</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [NonAction]
        private string GeneratingPDFImportYard(WFLD_GPHdr objGP, int GatePassId)
        {
            string html = "";
            string Vessel = "", Voyage = "", Rotation = "", LineNo = "", NoOfUnits = "", Weight = "", CargoType = "", Location = "", PortOfDispatch = "", EntryDate = "", ICDCode = "", ShippingLineNo = "", CargoDescription = "", OBLNO = "", IGMNo = "", InDate = "", DestuffingDate = "", CustomSealNo = "", InvoiceNo = "", IssueSlipNo = "", IssueSlipDate = "", CreatedTime = "", InvoiceType = "", CFSCode = "";



            var ContainerNo = ""; var VehicleNo = ""; var EmptyPort = "";
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

            objGP.lstDet.Select(x => new { ICDCode = x.ICDCode }).Distinct().ToList().ForEach(item =>
            {
                if (ICDCode == "")
                    ICDCode = item.ICDCode;
                else
                    ICDCode += "," + item.ICDCode;
            });
            objGP.lstDet.Select(x => new { ShippingLineNo = x.ShippingLineNo }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLineNo == "")
                    ShippingLineNo = item.ShippingLineNo;
                else
                    ShippingLineNo += "," + item.ShippingLineNo;
            });

            objGP.lstDet.Select(x => new { InvoiceNo = x.InvoiceNo }).Distinct().ToList().ForEach(item =>
            {
                if (InvoiceNo == "")
                    InvoiceNo = item.InvoiceNo;
                else
                    InvoiceNo += "," + item.InvoiceNo;
            });

            objGP.lstDet.Select(x => new { IssueSlipNo = x.IssueSlipNo }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipNo == "")
                    IssueSlipNo = item.IssueSlipNo;
                else
                    IssueSlipNo += "," + item.IssueSlipNo;
            });


            objGP.lstDet.Select(x => new { IssueSlipDate = x.IssueSlipDate }).Distinct().ToList().ForEach(item =>
            {
                if (IssueSlipDate == "")
                    IssueSlipDate = item.IssueSlipDate;
                else
                    IssueSlipDate += "," + item.IssueSlipDate;
            });

            objGP.lstDet.Select(x => new { CreatedTime = x.CreatedTime }).Distinct().ToList().ForEach(item =>
            {
                if (CreatedTime == "")
                    CreatedTime = item.CreatedTime;
                else
                    CreatedTime += "," + item.CreatedTime;
            });
            objGP.lstDet.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "," + item.CargoDescription;
            });
            objGP.lstDet.Select(x => new { OBLNO = x.OBLNO }).Distinct().ToList().ForEach(item =>
            {
                if (OBLNO == "")
                    OBLNO = item.OBLNO;
                else
                    OBLNO += "," + item.OBLNO;
            });
            objGP.lstDet.Select(x => new { IGMNo = x.IGMNo }).Distinct().ToList().ForEach(item =>
            {
                if (IGMNo == "")
                    IGMNo = item.IGMNo;
                else
                    IGMNo += "," + item.IGMNo;
            });
            objGP.lstDet.Select(x => new { InDate = x.InDate }).Distinct().ToList().ForEach(item =>
            {
                if (InDate == "")
                    InDate = item.InDate;
                else
                    InDate += "," + item.InDate;
            });
            objGP.lstDet.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "," + item.DestuffingDate;
            });
            objGP.lstDet.Select(x => new { EntryDate = x.EntryDate }).Distinct().ToList().ForEach(item =>
            {
                if (EntryDate == "")
                    EntryDate = item.EntryDate;
                else
                    EntryDate += "," + item.EntryDate;
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
            objGP.lstDet.Select(x => new { CustomSealNo = x.CustomSealNo }).Distinct().ToList().ForEach(item =>
            {
                if (CustomSealNo == "")
                    CustomSealNo = item.CustomSealNo;
                else
                    CustomSealNo += "," + item.CustomSealNo;
            });
            objGP.lstDet.Select(x => new { PortOfDispatch = x.PortOfDispatch }).Distinct().ToList().ForEach(item =>
            {
                if (PortOfDispatch == "")
                    PortOfDispatch = item.PortOfDispatch;
                else
                    PortOfDispatch += "," + item.PortOfDispatch;
            });

            objGP.lstDet.Select(m => new { CargoType = m.CargoType }).Distinct().ToList().ForEach(item =>
            {
                if (item.CargoType == 1)
                    CargoType = (CargoType == "" ? "Hazardous" : CargoType + ",Hazardous");
                else if (item.CargoType == 2)
                    CargoType = (CargoType == "" ? "Non Hazardous" : CargoType + ",Non Hazardous");
            });
            objGP.lstHed.Select(m => new { CFSCode = m.CFSCode }).Distinct().ToList().ForEach(item =>
            {
                if (item.CFSCode == "")
                    CFSCode = "-Factory Destuffing)";
                else
                    CFSCode = "-Direct Destuffing)";
            });
            objGP.lstHed.Select(m => new { InvoiceType = m.InvoiceType }).Distinct().ToList().ForEach(item =>
            {
                if (item.InvoiceType == "IMPDeli")
                    InvoiceType = "(Godown)";  //InvoiceType = "(LCL)";
                else if (item.InvoiceType == "IMPYard")
                    InvoiceType = "(Yard " + CFSCode;// InvoiceType ="(FCL"+ CFSCode;
                else if (item.InvoiceType == "EC")
                    InvoiceType = "(Empty Container Gate Out)";
                else if (item.InvoiceType == "BTT")
                    InvoiceType = "(BTT)";
                else if (item.InvoiceType == "EXP")
                    InvoiceType = "(Export Payment Sheet)";
                else if (item.InvoiceType == "EXPLod")
                    InvoiceType = "(Export Loaded Container)";

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
            string strDateTime = objGP.EntryDate;
            string[] dateTimeArray = strDateTime.Split(' ');
            string strDtae = dateTimeArray[0];
            string strTime = dateTimeArray[1];


            html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'>";
            html += "<thead>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='width:100%; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th colspan='6' width='50%' style='text-align:left;'></th><th colspan='6' width='50%' style='text-align:right; font-size:10px;'>Doc. No.F/CD/ICD/19</th></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";
            html += "<tr><th colspan='12' style='text-align:center;font-size:14px'></th></tr>";
            html += "<tr><td><span><br/></span></td></tr>";
            html += "</thead>";
            html += "<tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>CFS Whitefield</span><br/><label style='font-size: 8pt; font-weight:bold;'>GATE PASS</label><label style='font-size: 8pt; font-weight:bold;'>" + InvoiceType + "</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td><span><br/></span></td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass No. : <span style='font-weight:normal;'>" + objGP.GatePassNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Date Time : <span style='font-weight:normal;'>" + objGP.GatePassDate + "</span> <span style='font-weight:normal;'>" + strTime + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vehicle No. : <span style='font-weight:normal;'>" + VehicleNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container No. & size : <span style='font-weight:normal;'>" + ContainerNo + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CFS Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Container In Date. : <span style='font-weight:normal;'>" + InDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Custom's Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th></tr>";

            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Name : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>BOE No : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> No. of Packages :<span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            // html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Empty Return Port : <span style='font-weight:normal;'>" + EmptyPort + "</span></th>";
            //  html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Invoice Date : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th>";
            html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'> <span style='font-weight:normal;'>" + " " + "</span></th></tr>";
            html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";




            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Vessel Name : <span style='font-weight:normal;'>" + Vessel + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Voyage No. : <span style='font-weight:normal;'>" + Voyage + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Rotation no. : <span style='font-weight:normal;'>" + Rotation + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Line No. : <span style='font-weight:normal;'>" + LineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line Seal no. : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Customs Seal No. : <span style='font-weight:normal;'>" + CustomSealNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Importer : <span style='font-weight:normal;'>" + objGP.ImpExpName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>CHA Name : <span style='font-weight:normal;'>" + objGP.CHAName + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Shipping Line : <span style='font-weight:normal;'>" + objGP.ShippingLineName + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>BOE No. : <span style='font-weight:normal;'>" + objGP.BOENo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>BOE Date : <span style='font-weight:normal;'>" + objGP.BOEDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>No. of Packages : <span style='font-weight:normal;'>" + NoOfUnits + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Weight : <span style='font-weight:normal;'>" + Weight + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Location Name : <span style='font-weight:normal;'>" + Location + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Port of Dispatch : <span style='font-weight:normal;'>" + PortOfDispatch + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Nature of Goods : <span style='font-weight:normal;'>" + CargoType + "</span></th></tr>";


            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>ICD Code : <span style='font-weight:normal;'>" + ICDCode + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Shipping Line No : <span style='font-weight:normal;'>" + ShippingLineNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Cargo Description : <span style='font-weight:normal;'>" + CargoDescription + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>OBL NO : <span style='font-weight:normal;'>" + OBLNO + "</span></th></tr>";
            ////html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>Gate Pass Time : <span style='font-weight:normal;'>" + strTime + "</span></th>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000; border-right:1px solid #000;'>IGM No : <span style='font-weight:normal;'>" + IGMNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Empty Return  : <span style='font-weight:normal;'>" + EmptyPort + "</span></th></tr>";

            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Container In Date : <span style='font-weight:normal;'>" + InDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Invoice No : <span style='font-weight:normal;'>" + InvoiceNo + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip No : <span style='font-weight:normal;'>" + IssueSlipNo + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Issue Slip Date : <span style='font-weight:normal;'>" + IssueSlipDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Issue Slip Created Date Time : <span style='font-weight:normal;'>" + CreatedTime + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Destuffing Date : <span style='font-weight:normal;'>" + DestuffingDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-right:1px solid #000; border-bottom:1px solid #000;'>Invoice Created Date Time : <span style='font-weight:normal;'>" + objGP.InvoiceDate + "</span></th>";
            //html += "<th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word; border-bottom:1px solid #000;'>Gate Pass Validity Date Time : <span style='font-weight:normal;'>" + objGP.ExpiryDate + "</span></th></tr>";
            //html += "<tr><th cellpadding='10' align='left' colspan='6' width='50%' style='overflow:hidden; word-wrap:break-word;'>Remarks : <span style='font-weight:normal;'>" + objGP.Remarks + "</span></th></tr>";


            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12'>";
            html += "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>";
            html += "<tr><td><br/><br/><br/><br/><br/><br/><br/></td></tr>";
            html += "<tr><th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'>Signature of CHA <br/>Received with Intact and Good Condition</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> PO<br/> CWC CFS, Whitefield</th>";
            html += "<th width='1%'></th>";
            html += "<th colspan='3' width='30%' style='border-top:1px solid #000;text-align:center;'> Delivered By<br/> Shed Incharge/CWC CFS Whitefield</th></tr>";
            html += "</thead></table>";
            html += "</td></tr>";
            html += "<tr><td><br/><br/><br/><br/></td></tr>";
            html += "<tr><td colspan='12'>****Material handed over in good condition</td></tr>";
            html += "</tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = this.ZonalOffice;
                rp.HOAddress = "(A Govt.Of India Undertaking)";
                rp.ZonalOffice = this.ZOAddress;
                rp.ZOAddress = "";
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/GatePass" + GatePassId + ".pdf";
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult CancelGatePass(int GatePassId)
        {
            if (GatePassId > 0)
            {
                WFLDGatePassRepository objGP = new WFLDGatePassRepository();
                objGP.CancelGatePass(GatePassId);
                return Json(objGP.DBResponse);
            }
            else
            {
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult InvoiceSearchByContainer(string Container)
        {
            WFLDGatePassRepository objGP = new WFLDGatePassRepository();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGP.ListOfInvoiceForPage(ObjLogin.Uid, Container, 0);
            return Json(objGP.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadInvoiceList(string Container, int Page)
        {
            WFLDGatePassRepository objGP = new WFLDGatePassRepository();
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGP.ListOfInvoiceForPage(ObjLogin.Uid, Container, Page);
            return Json(objGP.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchGatePass(string Value)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            List<WFLDListOfGP> lstGP = new List<WFLDListOfGP>();
            objGPR.SearchGatePass(Value);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<WFLDListOfGP>)objGPR.DBResponse.Data;
            return PartialView("ListOfGatePass", lstGP);
        }

        #endregion


        #region Send DP

        public async Task<JsonResult> SendDP(int GatePassId = 0)
        {
            try
            {
                int k = 0;
                int j = 1;
                WFLDGatePassRepository ObjER = new WFLDGatePassRepository();
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
                        #region Digital Signature

                        string apiUrl = System.Configuration.ConfigurationManager.AppSettings["DscApiUrl"];
                        string InJsonFile = System.Configuration.ConfigurationManager.AppSettings["InJsonFileDP"];
                        string OUTJsonFile = System.Configuration.ConfigurationManager.AppSettings["OUTJsonFileDP"];
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

                        WFLDGatePassRepository objExport = new WFLDGatePassRepository();
                        objExport.GetCIMDPDetailsUpdateStatus(GatePassId);


                        log.Info("FTP File upload has been end");
                    }

                    return Json(new { Status = 1, Message = "CIM DP File Send Successfully." });
                }
                else
                {
                    return Json(new { Status = ObjER.DBResponse.Status, Message = ObjER.DBResponse.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = "CIM DP File Send Fail." });
            }



        }
        #endregion


        #region Gate Pass
        [HttpGet]
        public ActionResult GatepassVehicleUpdation(int GatePassId=0)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            WFLDGatePass objGP = new WFLDGatePass();
         /*   objGPR.ListOfGatepassForPage("", 0);


            if (objGPR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objGPR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstGatePass = Jobject["lstGatePass"];
                ViewBag.VhState = Jobject["State"];
            }
            else
            {
                ViewBag.lstGatePass = null;
            }*/
           
           
            CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            if (GatePassId > 0)
            {
                objGPR.GetDetForGatePass(GatePassId, ObjLogin.Uid);
                if (objGPR.DBResponse.Data != null)
                    objGP = (WFLDGatePass)objGPR.DBResponse.Data;
            }
            return PartialView(objGP);
        

        }

     /*   [HttpGet]
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
        }*/
        [HttpGet]
        public JsonResult LoadGateList(string PartyCode, int Page)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            objGPR.ListOfGatepassForPage(PartyCode, Page);
            var jsonResult = Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
          /*  jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);*/
             if (objGPR.DBResponse.Data != null)
                return Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        
        }

        [HttpGet]
        public JsonResult SearchGateList(string PartyCode)
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            objGPR.ListOfGatepassForPage(PartyCode, 0);
            var jsonResult = Json(objGPR.DBResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditGatePassupdate(WFLDGatePass objGP)
        {

            if (objGP.Module == "IMPDeli" || objGP.Module == "IMPYard" || objGP.Module == "EC" || objGP.Module == "ECGodn" || objGP.Module == "BNDadv" || objGP.Module == "BND" || objGP.Module == "Exp" || objGP.Module == "ExpLod" || objGP.Module == "EXPLod" ||objGP.Module == "EXP")
            {
                ModelState.Remove("DepartureDate");
                ModelState.Remove("ArrivalDate");

            }


            if (ModelState.IsValid)
            {
                List<WFLD_ContainerDet> lstContDet = new List<WFLD_ContainerDet>();
                string XMLdata = null;
                if (objGP.StringifyData != null)
                {
                    lstContDet = JsonConvert.DeserializeObject<List<WFLD_ContainerDet>>(objGP.StringifyData);
                    XMLdata = Utility.CreateXML(lstContDet);
                }

                List<WFLDGatepassVehicle> lstVehicle = new List<WFLDGatepassVehicle>();
                string VehicleXML = "";
                if (objGP.VehicleXml != null)
                {
                    lstVehicle = JsonConvert.DeserializeObject<List<WFLDGatepassVehicle>>(objGP.VehicleXml);
                    VehicleXML = Utility.CreateXML(lstVehicle);
                }

                WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
                objGPR.AddEditGatePassUpdate(objGP, XMLdata, VehicleXML);
                return Json(objGPR.DBResponse);
            }
            else
            {
                return Json(new { Status = -1 });
            }
        }

        [HttpGet]
        public ActionResult ListOfGatePassVehicleUpdation()
        {
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            List<WFLD_GatePassUpdation> lstGP = new List<WFLD_GatePassUpdation>();
           // CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGPR.ListOfGatePassVehicle(0);
            //objGPR.ListOfGatePass(0);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<WFLD_GatePassUpdation>)objGPR.DBResponse.Data;
            return PartialView(lstGP);
        }

        [HttpGet]
        public ActionResult LoadMoreListOfGatePassVehicle(int Page)
        {
            //TempData["lstFlag"] = lstFlag;
            //TempData.Keep();
            WFLDGatePassRepository objGPR = new WFLDGatePassRepository();
            List<WFLD_GatePassUpdation> lstGP = new List<WFLD_GatePassUpdation>();
          //  CwcExim.Models.Login ObjLogin = (CwcExim.Models.Login)Session["LoginUser"];
            objGPR.LoadMoreListOfGatePassVehicle(Page);
            // objGPR.LoadMoreListOfGatePass(Page);
            if (objGPR.DBResponse.Data != null)
                lstGP = (List<WFLD_GatePassUpdation>)objGPR.DBResponse.Data;
            //return PartialView(lstGP);
            return Json(lstGP, JsonRequestBehavior.AllowGet);
        }
        #endregion



    }
}