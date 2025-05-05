using CwcExim.Areas.ExpSealCheking.Models;
using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using CwcExim.Models;
using System.Web.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using CwcExim.Areas.Import.Models;
using System.Data;
using System.Text;
using System.IO;
using CwcExim.Areas.Export.Models;

namespace CwcExim.Areas.ExpSealCheking.Controllers
{
    public class ExpSealChekingController : BaseController
    {
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        // GET: ExpSealCheking/ExpSealCheking

        #region Gate Entry

        [HttpGet]
        public ActionResult CreateGateEntry()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ExpSealGetTime();
            CHN_EntryThroughGate objEntryThroughGate = new CHN_EntryThroughGate();
            if (ObjChnR.DBResponse.Data != null)
            {

                objEntryThroughGate = (CHN_EntryThroughGate)ObjChnR.DBResponse.Data;
                string ExitTime = objEntryThroughGate.EntryDateTime;
                string[] ExitTimeArray = ExitTime.Split(' ');
                objEntryThroughGate.EntryDateTime = ExitTimeArray[0];
                ViewBag.strTime = objEntryThroughGate.Time;
                string SystemTime = objEntryThroughGate.SystemDateTime;
                string[] SystemTimeArray = SystemTime.Split(' ');
                objEntryThroughGate.SystemDateTime = SystemTimeArray[0];
            }
            return PartialView("SealCheking", objEntryThroughGate);

        }


        [HttpGet]
        public JsonResult GetChaList()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfCHA();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExportList()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfExporter();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEditGateEntry(CHN_EntryThroughGate ObjEntry)
        {
            try
            {

                string Entrytime = Request.Form["time"];
                string SysTime = Request.Form["SysTime"];
                if (ObjEntry.EntryId == 0)
                {
                    Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
                }
                if (Entrytime.Length == 7)
                {
                    Entrytime = Entrytime.Replace("PM", " PM").Replace("AM", " AM");
                }

                if (ObjEntry.EntryId > 0)
                {


                    Entrytime = Entrytime.Replace("  PM", " PM").Replace("  AM", " AM");

                }
                string strEntryDateTime = ObjEntry.EntryDateTime + " " + Entrytime;
                if (SysTime != null)
                {
                    string[] SplitSysTime = SysTime.Split(':');
                    string SystemTime = SplitSysTime[2].Substring(SplitSysTime[2].Length - 2);
                    string SysHour = SplitSysTime[0].Length == 1 ? ("0" + SplitSysTime[0]) : SplitSysTime[0];
                    SysTime = SysHour + ":" + SplitSysTime[1] + "  " + SystemTime;
                    // SysTime = SplitSysTime[0] + ":" + SplitSysTime[1] + "  "+ SystemTime;
                    SysTime = SysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    string strSysDateTime = ObjEntry.SystemDateTime + " " + SysTime;
                    ObjEntry.SystemDateTime = strSysDateTime;

                }

                else
                {
                    ObjEntry.SystemDateTime = null;
                }
                ObjEntry.EntryDateTime = strEntryDateTime;
                if (ModelState.IsValid)
                {
                    CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
                    Login ObjLogin = (Login)Session["LoginUser"];
                    ObjEntry.Uid = ObjLogin.Uid;

                    ObjChnR.AddEditEntryThroughGate(ObjEntry);
                    //ModelState.Clear();
                    return Json(ObjChnR.DBResponse);

                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1, Message = ErrorMessage };
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
        public ActionResult EntryThroughGateList()
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            List<CHN_EntryThroughGate> LstEntryThroughGate = new List<CHN_EntryThroughGate>();
            ObjChnR.GetAllEntryThroughGate();
            if (ObjChnR.DBResponse.Data != null)
            {
                LstEntryThroughGate = (List<CHN_EntryThroughGate>)ObjChnR.DBResponse.Data;
            }
            return PartialView("EntryThroughGateList", LstEntryThroughGate);
        }

        [HttpGet]
        public ActionResult ViewEntryThroughGate(int EntryId)
        {

            CHN_EntryThroughGate ObjEntryGate = new CHN_EntryThroughGate();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
                ObjChnR.GetEntryThroughGate(EntryId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjEntryGate = (CHN_EntryThroughGate)ObjChnR.DBResponse.Data;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;

                    string EntryDateTime = ObjEntryGate.EntryDateTime;
                    string[] SplitEntryDateTime = EntryDateTime.Split(' ');
                    var ConvertEntryTime = DateTime.ParseExact(SplitEntryDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertEntryTime = ConvertEntryTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.EntryDateTime = SplitEntryDateTime[0] + " " + ConvertEntryTime;
                }
            }
            return PartialView("ViewEntryThroughGate", ObjEntryGate);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            CHN_EntryThroughGate ObjEntryGate = new CHN_EntryThroughGate();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();

            if (EntryId > 0)
            {
                ObjChnR.GetEntryThroughGate(EntryId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjEntryGate = (CHN_EntryThroughGate)ObjChnR.DBResponse.Data;

                    string strDateTime = ObjEntryGate.EntryDateTime;
                    string[] dateTimeArray = strDateTime.Split(' ');
                    string strDtae = dateTimeArray[0];

                    string strTime = dateTimeArray[1];
                    var convertTime = DateTime.ParseExact(strTime, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    string strTruckSlipDate = ObjEntryGate.TruckSlipDate;
                    string[] DateTruckSlipDate = strTruckSlipDate.Split(' ');
                    string TruckSlipDate = DateTruckSlipDate[0];

                    ObjEntryGate.EntryDateTime = strDtae;
                    ObjEntryGate.TruckSlipDate = TruckSlipDate;
                    ViewBag.strTime = convertTime;
                    string SystemDateTime = ObjEntryGate.SystemDateTime;
                    string[] SplitSystemDateTime = SystemDateTime.Split(' ');
                    var ConvertSysTime = DateTime.ParseExact(SplitSystemDateTime[1], "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToString("hh:mm tt");
                    ConvertSysTime = ConvertSysTime.Replace("  PM", " PM").Replace("  AM", " AM");
                    ObjEntryGate.SystemDateTime = SplitSystemDateTime[0] + " " + ConvertSysTime;
                }
            }
            return PartialView( ObjEntryGate);


        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteEntryThroughGate(int EntryId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteEntryThroughGate(EntryId);
            return Json(objChnR.DBResponse);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GateEntryTruckSlipPrint(int EntryId, string CBTContainer, string GateInNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GateEntryTruckSlipReport(EntryId);
            if (ObjChnR.DBResponse.Data != null)
            {
                PrintTruckSlip LstSeal = new PrintTruckSlip();
                LstSeal = (PrintTruckSlip)ObjChnR.DBResponse.Data;
                string Path = GeneratingPDF(LstSeal, EntryId, CBTContainer,GateInNo);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 1, Message = "Error" });
            }
        }
        [NonAction]
        private string GeneratingPDF(PrintTruckSlip LstSeal, int EntryId, string CBTContainer, string GateInNo)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/TruckSlip" + EntryId + ".pdf";
            int Index = 1;
            //string hdrCont = "";
            StringBuilder Html = new StringBuilder();
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            Html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            Html.Append("<td width='10%' align='right'>");
            Html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            Html.Append("<tr><td style='border:1px solid #333;'>");
            Html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/CFS/01</div>");
            Html.Append("</td></tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");

            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            Html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            Html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally<br/> IDPL Road, Chennai - 37</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>TRUCK SLIP</label></td>");
            Html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");

            Html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><th style='width:8%; font-size:12px;'>SL.No.</th>");
            Html.Append("<td style='font-size:12px;'><u>" + GateInNo + "</u></td><th style='font-size:13px; text-align:right;'>Date</th>");
            Html.Append("<td style='font-size:12px; width:10%;'><u>" + DateTime.Now.ToString("dd/MM/yyyy") + "</u></td></tr></tbody></table></td></tr>");

            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
            Html.Append("<td colspan='3' width='33.3%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            Html.Append("<tr><th style='text-align:left; font-size:12px;'>Truck No." + LstSeal.VehicleNo + "</th><td style='font-size:12px;'></td></tr>");
            Html.Append("</tbody></table></td>");
            Html.Append("<td colspan='3' width='33.3%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            Html.Append("<tr><th style='font-size:12px;'>Container No.</th><td style='font-size:12px;'>" + CBTContainer + "</td></tr>");
            Html.Append("</tbody></table></td>");
            Html.Append("<td colspan='3' width='33.3%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            Html.Append("<tr><th style='font-size:12px; text-align:right;'>Size</th><td style='font-size:12px; width:21%;'>" + LstSeal.Size + "</td></tr>");
            Html.Append("</tbody></table></td>");
            Html.Append("</tr></tbody></table>");
            Html.Append("</td></tr>");


            // Html.Append("<tr><td colspan='12'><span style='font-size:12px;padding:0 5px;'>Purpose</span> "+LstSeal.Remarks+"</td></tr>");
            // Html.Append("<tr><th style='font-size:12px;'>Purpose</th><td style='font-size:12px;'>" + LstSeal.Remarks + "</td></tr>");
            Html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><th style='width:8%; font-size:12px;'>Purpose:</th>");
            Html.Append("<td style='font-size:12px;'> "+ LstSeal.Remarks + "</td></tr></tbody></table></td></tr>");

            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table style='border: 1px solid #000;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
            Html.Append("<td colspan='6' width='50%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%; font-size:8pt; font-family:Verdana,Arial,San-serif; border-collapse:collapse;'>");
            Html.Append("<thead><tr><th colspan='12' style='border-bottom: 1px solid #000; font-size:14px; text-align:center; padding:10px;'>TO BE FILLED BY AGENT</th></tr>");
            Html.Append("<tr><th colspan='2' width='16.66666667%' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>Sl. No.</th>");
            Html.Append("<th colspan='4' width='33.3%' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>Sub No./ Carting PNR No.</th>");
            Html.Append("<th colspan='3' width='25%;' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>Cargo</th>");
            Html.Append("<th colspan='3' width='25%' style='border-bottom:1px solid #000;text-align:center;padding:10px;'>No. of Units</th></tr></thead>");
            Html.Append("<tbody>");
            
            Html.Append("<tr><td colspan='2' width='16.66666667%' style='border-right:1px solid #000;font-size:7pt; text-align:center; padding:10px;'>" + Index + "</td> ");
                Html.Append("<td colspan='4' width='33.3%' style='border-right:1px solid #000;font-size:7pt; text-align:center; padding:10px;'>" + "" + "</td>");
                Html.Append("<td colspan='3' width='25%' style='border-right:1px solid #000;font-size:7pt; text-align:center; padding:10px;'>" + LstSeal.Cargo + "</td>");
                Html.Append("<td colspan='3' width='25%' style='font-size:7pt; text-align:center; padding:10px;'>" + LstSeal.NoOfUnits + "</td></tr>");
                Index++;
            
            Html.Append("</tbody>");
            Html.Append("</table></td>");

            Html.Append("<td colspan='6' width='50%' valign='top' style='padding:0;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            Html.Append("<thead><tr><th colspan='12' style='font-size:14px;text-align:center;padding:10px;border-bottom:1px solid #000;'>TO BE FILLED BY CFS</th></tr>");
            Html.Append("<tr><th colspan='3' width='33.3%' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>Godown No.</th> ");
            Html.Append("<th colspan='3' width='33.3%' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>No. of Units</th>");
            Html.Append("<th colspan='3' width='33.3%' style='border-right:1px solid #000;border-bottom:1px solid #000;text-align:center;padding:10px;'>Gate Pass No.</th>");
            Html.Append("<th colspan='3' width='33.3%' style='border-bottom:1px solid #000;text-align:center;padding:10px;'>Signature of Shed Incharge</th></tr></thead>");
            Html.Append("<tbody>");
   
                Html.Append("<tr><td colspan='3' width='33.3%' style='border-right:1px solid #000;font-size:7pt; text-align:center; padding:10px;'>" + string.Empty + "</td>");
                Html.Append("<td colspan='3' width='33.3%' style='border-right:1px solid #000;font-size:7pt;text-align:center;padding:10px;'>" + LstSeal.NoOfUnits + "</td> ");
                Html.Append("<td colspan='3' width='33.3%' style='border-right:1px solid #000;font-size:7pt; text-align:center;padding:10px;'>" + string.Empty + "</td>");
                Html.Append("<td colspan='3' width='33.3%' style='font-size:7pt; text-align:center;padding:10px;'></td></tr>");
   
            Html.Append("</tbody>");
            Html.Append("</table></td>");
            Html.Append("</tr></tbody></table>");
            Html.Append("</td></tr>");

            Html.Append("<tr><td colspan='12'><span style='font-size:13px;'>Name of the Importer/Exporter : " + LstSeal.Exporter + "</span></td></tr>");
            Html.Append("<tr><td colspan='12'><span style='font-size:13px;'>Name and Signature of CHA : " + LstSeal.ChaName + "</span></td></tr>");
            Html.Append("<tr><th colspan='12' style='text-align: center;font-size: 15px;'><u>AT GATE</u></th></tr>");
            Html.Append("<tr><th colspan='6' style='text-align: center;font-size: 15px;'>ALLOWED IN</th><th colspan='6' style='text-align: center;font-size: 15px;'>ALLOWED OUT</th></tr>");

            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table cellspacing='0' cellpadding='0'  style='border:1px solid #000;width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            Html.Append("<tr>");
            Html.Append("<td colspan='6' width='50%' valign='top' style='border-right:1px solid #000;'><table cellpadding='5' style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Truck in time</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'>" + LstSeal.EntryTime + "</td></tr>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Date</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'>" + LstSeal.Entrydate + "</td></tr>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Seal No.</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'>" + LstSeal.CustomSealNo + "</td></tr>");
            Html.Append("<tr><td colspan='12'><span><br/><br/></span></td></tr>");
            Html.Append("<tr><th colspan='12' style='text-align: center;font-size: 13px;'>Signature of Gate I/C</th></tr>");
            Html.Append("</tbody></table></td>");
            Html.Append("<td colspan='6' width='50%' valign='top'><table cellpadding='5' style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Truck out time</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'></td></tr>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Date</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'></td></tr>");
            Html.Append("<tr><td colspan='1' style='font-size:12px;' width='50%' valign='top'>Seal No.</td><td>:</td><td colspan='2' style='font-size:12px;' width='40%' valign='top'></td></tr>");
            Html.Append("<tr><td colspan='12'><span><br/><br/></span></td></tr>");
            Html.Append("<tr><th colspan='12' style='text-align: center;font-size: 13px;'>Signature of Gate I/C</th></tr>");
            Html.Append("</tbody></table></td>");
            Html.Append("</tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("</tbody></table>");

            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.HeadOffice = this.HeadOffice;
                Rh.HOAddress = this.HOAddress;
                Rh.ZonalOffice = this.ZonalOffice;
                Rh.ZOAddress = this.ZOAddress;
                Rh.GeneratePDF(Path, Html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/TruckSlip" + EntryId + ".pdf";
        }
        #endregion

        #region Custom Checking Approval

        [HttpGet]
        public ActionResult CreateCustomChekingApproval()
        {
            return PartialView("CustomChekingApproval");
        }

        [HttpGet]
        public JsonResult GetTruckSlipListForCustomChecking()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfTrcukSlipForCustomChecking();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerDetailsForCustomChecking(string TruckSlipNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GetContainerDetForCustomChecking(TruckSlipNo);
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEditCustomChekingApproval(CHN_CustomChekingApproval ObjEntry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
                    Login ObjLogin = (Login)Session["LoginUser"];
                    ObjEntry.Uid = ObjLogin.Uid;

                    ObjChnR.AddEditCustomChekingApproval(ObjEntry);
                    //ModelState.Clear();
                    return Json(ObjChnR.DBResponse);

                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1, Message = ErrorMessage };
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
        public ActionResult CustomChekingApprovalList()
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            List<CHN_CustomChekingApproval> LstCustomCheking = new List<CHN_CustomChekingApproval>();
            ObjChnR.GetAllCustomChekingApproval();
            if (ObjChnR.DBResponse.Data != null)
            {
                LstCustomCheking = (List<CHN_CustomChekingApproval>)ObjChnR.DBResponse.Data;
            }
            return PartialView(LstCustomCheking);
        }

        [HttpGet]
        public ActionResult ViewCustomCheckingApproval(int CustomId)
        {


            CHN_CustomChekingApproval ObjCustomEntry = new CHN_CustomChekingApproval();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (CustomId > 0)
            {

                ObjChnR.GetCustomChekingApproval(CustomId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjCustomEntry = (CHN_CustomChekingApproval)ObjChnR.DBResponse.Data;


                }
            }
            return PartialView(ObjCustomEntry);
        }

        [HttpGet]
        public ActionResult EditCustomChekingApproval(int CustomId)
        {
            CHN_CustomChekingApproval ObjCustomEntry = new CHN_CustomChekingApproval();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();

            if (CustomId > 0)
            {
                ObjChnR.GetCustomChekingApproval(CustomId);
                ObjCustomEntry = (CHN_CustomChekingApproval)ObjChnR.DBResponse.Data;
            }
            return PartialView(ObjCustomEntry);


        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCustomCheckingApproval(int CustomId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteCustomCheckingApproval(CustomId);
            return Json(objChnR.DBResponse);
        }

        #endregion

        #region Job Order

        [HttpGet]
        public ActionResult CreateJobOrder()
        {
            CHN_SealChekingJobOrder ObjSc = new CHN_SealChekingJobOrder();
            ObjSc.JobOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.Operation = null;
            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();

            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            return PartialView(ObjSc);
        }

        [HttpGet]
        public JsonResult GetTruckSlipListForJobOrder()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfTrcukSlipForJobOrder();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerDetForJobOrder(string TruckSlipNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GetContainerDetForJobOrder(TruckSlipNo);
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddEditJobOrder(CHN_SealChekingJobOrder objImp)
        {
            //if (ModelState.IsValid)
            //{
             List<CHN_SealChekingJobOrder> lstDtl = new List<CHN_SealChekingJobOrder>();
            List<CHN_JobOrderClauseDtl> lstCDtl = new List<CHN_JobOrderClauseDtl>();

            List<int> lstLctn = new List<int>();
            string XML = "", lctnXML = "";
            String CXML = "";
            CHN_EntryThroughGateRepository objIR = new CHN_EntryThroughGateRepository();
            if (objImp.JobOrderDetailsJS != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CHN_SealChekingJobOrder>>(objImp.JobOrderDetailsJS);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            if (objImp.JobOrderClauseJS != null)
            {
                lstCDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CHN_JobOrderClauseDtl>>(objImp.JobOrderClauseJS);
                if (lstCDtl.Count > 0)
                    CXML = Utility.CreateXML(lstCDtl);
            }
            /*if (objImp.YardWiseLocationIds != null)
            {
                string[] lctns = objImp.YardWiseLocationIds.Split(',');
                foreach (string data in lctns)
                    lstLctn.Add(Convert.ToInt32(data));
                lctnXML = Utility.CreateXML(lstLctn);
            }*/
            objIR.AddEditSealCheckingJO(objImp, XML, lctnXML, CXML);
            return Json(objIR.DBResponse);
            //}
            //else
            //{
            //    var Err = new { Status = -1, Message = "Error" };
            //    return Json(Err);
            //}

        }


        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            CHN_EntryThroughGateRepository objIR = new CHN_EntryThroughGateRepository();
            IList<CHN_JobOrderList> lstIJO = new List<CHN_JobOrderList>();
            objIR.GetAllJobOrder();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<CHN_JobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]

        public ActionResult ViewJobOrder(int ImpJobOrderId)
        { 

            CHN_SealChekingJobOrder ObjSealChecking = new CHN_SealChekingJobOrder();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (ImpJobOrderId > 0)
            {
               
                ObjChnR.GetJobOrderById(ImpJobOrderId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjSealChecking=(CHN_SealChekingJobOrder)ObjChnR.DBResponse.Data;
                }
            }
            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            return PartialView(ObjSealChecking);
        }

        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            CHN_SealChekingJobOrder ObjSealChecking = new CHN_SealChekingJobOrder();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();

            if (ImpJobOrderId > 0)
            {
                ObjChnR.GetJobOrderById(ImpJobOrderId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjSealChecking = (CHN_SealChekingJobOrder)ObjChnR.DBResponse.Data;

              
                }
            }
            OperationRepository ObjCR = new OperationRepository();
            ObjCR.GetAllMstOperation();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Operation = ((List<Operation>)ObjCR.DBResponse.Data).ToList().Where(x => x.Type == 1 || x.Type == 3);
            }
            return PartialView(ObjSealChecking);


        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteSealCheckingJO(ImpJobOrderId);
            return Json(objChnR.DBResponse);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJobOrder(int JobOrderId, string JoborderNo, string TruckSlipNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.PrintSealChekingJobOrder(JobOrderId, TruckSlipNo);
            if (ObjChnR.DBResponse.Data != null)
            {
                PrintJobOrderSealChecking LstSeal = new PrintJobOrderSealChecking();
                LstSeal = (PrintJobOrderSealChecking)ObjChnR.DBResponse.Data;
                string Path = GeneratingSealChekingJOPDF(LstSeal, JobOrderId, JoborderNo, TruckSlipNo);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 1, Message = "Error" });
            }
        }

        public string GeneratingSealChekingJOPDF(PrintJobOrderSealChecking LstJobOrder, int JobOrderId, string JoborderNo, string TruckSlipNo)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SealCheckingJobOrder" + JobOrderId + ".pdf";
            List<PrintJobOrder> List = new List<PrintJobOrder>();
            StringBuilder Html = new StringBuilder();
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            Html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            Html.Append("</tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("<tr><td colspan='12'>");
            Html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            Html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            Html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Container Freight Station<br/> IDPL Road, Chennai - 37</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>JOB ORDER</label></td>");
            Html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            Html.Append("</tbody></table>");
            Html.Append("</td></tr>");
            Html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:13px; width:8%;'>SL.No.</th>");
            Html.Append("<td style='font-size:12px;'><u>" + LstJobOrder.JobOrderNo + "</u></td><th style='font-size:13px; text-align:right;'>Date</th>");
            Html.Append("<td style='font-size:12px; width:10%;'><u>" + DateTime.Now.ToString("dd/MM/yyyy") + "</u></td></tr></tbody></table></td></tr>");
            Html.Append("<tr>");
            Html.Append("<td colspan='12'>");
            Html.Append("<table style='border:1px solid #000;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
            Html.Append("<thead>");
            Html.Append("<tr>");
            Html.Append("<th style='font-weight:600;text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Name of Operation</th> ");
            Html.Append("<th style='font-weight:600; text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Location</th>");
            Html.Append("<th style='font-weight:600; text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Name of Party</th> ");
            Html.Append("<th style='font-weight:600; text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>Weight</th> ");
            Html.Append("<th style='font-weight:600; text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>BOE No.</th>");
            Html.Append("<th style='font-weight:600; text-align:center;border-bottom:1px solid #000;padding:10px;'>No. of Units</th>");
            Html.Append("</tr>");
            Html.Append("</thead>");
            Html.Append("<tbody>");

         

                Html.Append("<tr>");
                Html.Append("<td style='text-align:left;border-right:1px solid #000;padding:10px;'>Seal Checking</td> ");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;padding:10px;'></td> ");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;padding:10px;'>" + LstJobOrder.Exporter + "</td> ");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;padding:10px;'>" + LstJobOrder.weight + "</td> ");
                Html.Append("<td style='text-align:center;border-right:1px solid #000;padding:10px;'></td>");
                Html.Append("<td style='text-align:center;padding:10px;'>" + LstJobOrder.NoOfUnits + "</td> ");
                Html.Append("</tr>");

              


            Html.Append("</tbody>");
            Html.Append("</table>");
            Html.Append("</td>");
            Html.Append("</tr>");
            Html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' width='100%'><tbody><tr><td><span><br/></span></td></tr><tr><td colspan='8' width='70%' valign='top'><span style='font-size:12px;'>To,</span><br/><br/> <span style='font-size:12px;'>H&T Contractor<br/> CFS, Chennai</span> </td><td colspan='4' width='30%'><table cellspacing='0' cellpadding='5' width='100%'><tbody><tr><th style='font-size:14px;' align='left'>SHIFT INCHARGE</th></tr><tr><td style='font-size:12px;' align='left'>Date :</td></tr><tr><td style='font-size:12px;' align='left'>Time :</td></tr></tbody></table></td></tr></tbody></table></td></tr>");
            Html.Append("<tr><td colspan='12'><span style='font-size:13px;'>Copy to:</span></td></tr>");
            Html.Append("<tr><td colspan='12'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span style='font-size:13px;'>Shed I/O at Shed No. <u></u></span></td></tr>");
            Html.Append("</tbody></table>");
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = this.HeadOffice;
                rh.HOAddress = this.HOAddress;
                rh.ZonalOffice = this.ZonalOffice;
                rh.ZOAddress = this.ZOAddress;
                rh.GeneratePDF(Path, Html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/SealCheckingJobOrder" + JobOrderId + ".pdf";
        }

        #endregion

        #region Seal Change Entry

        [HttpGet]
        public ActionResult CreateSealChangeEntry()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetJobOrderNoList()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfJobOrderNo();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerDetailsByJobOrder(string JobOrderNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GetContainerDetByJobOrderNo(JobOrderNo);
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEditSealChangeEntry(CHN_SealChangeEntry ObjEntry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
                    Login ObjLogin = (Login)Session["LoginUser"];
                    ObjEntry.Uid = ObjLogin.Uid;

                    ObjChnR.AddEditSealChangeEntry(ObjEntry);
                    //ModelState.Clear();
                    return Json(ObjChnR.DBResponse);

                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1, Message = ErrorMessage };
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
        public ActionResult SealChangeEntryList()
        {
            ////GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            //List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            ////ObjGOR.GetEntryThroughGate();
            ////if (ObjGOR.DBResponse.Data != null)
            ////{
            ////    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            ////}
            //return View(LstGateEntry);

            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            List<CHN_SealChangeEntry> LstSealChange = new List<CHN_SealChangeEntry>();
            ObjChnR.GetAllSealChangeEntry();
            if (ObjChnR.DBResponse.Data != null)
            {
                LstSealChange = (List<CHN_SealChangeEntry>)ObjChnR.DBResponse.Data;
            }
            return PartialView(LstSealChange);
        }

        [HttpGet]
        public ActionResult ViewSealChangeEntry(int EntryId)
        {
           

            CHN_SealChangeEntry ObjSealChangeEntry = new CHN_SealChangeEntry();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (EntryId > 0)
            {
               
                ObjChnR.GetSealChangeEntry(EntryId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjSealChangeEntry = (CHN_SealChangeEntry)ObjChnR.DBResponse.Data;
                 
                
                }
            }
            return PartialView(ObjSealChangeEntry);
        }

        [HttpGet]
        public ActionResult EditSealChangeEntry(int EntryId)
        {
            CHN_SealChangeEntry ObjsealChange = new CHN_SealChangeEntry();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if(EntryId>0)
            {
                ObjChnR.GetSealChangeEntry(EntryId);
                if(ObjChnR.DBResponse.Data!=null)
                {
                    ObjsealChange = (CHN_SealChangeEntry)ObjChnR.DBResponse.Data;
                }
            }
            return PartialView(ObjsealChange);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteSealChangeEntry(int EntryId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteSealChangEntry(EntryId);
            return Json(objChnR.DBResponse);
        }
        #endregion

        #region Inspection and Weighment

        [HttpGet]
        public ActionResult CreateSealCheckingWeighment()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetTruckSlipListForWeighment()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.ListOfTrcukSlipForWeighment();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetContainerDetailsForWeighment(string TruckSlipNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
        ObjChnR.GetContainerDetForWeighment(TruckSlipNo);
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
    }

        [HttpPost]
        public ActionResult AddEditSealCheckingWeghment(CHN_Weighment ObjEntry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
                    Login ObjLogin = (Login)Session["LoginUser"];
                    ObjEntry.Uid = ObjLogin.Uid;

                    ObjChnR.AddEditSealCheckingweighent(ObjEntry);
                    //ModelState.Clear();
                    return Json(ObjChnR.DBResponse);

                }
                else
                {
                    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    var Err = new { Status = -1, Message = ErrorMessage };
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
        public ActionResult SealCheckingWeighmentList()
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            IList<CHN_WeighmentList> lstWeighment= new List<CHN_WeighmentList>();
            objChnR.GetAllSealCheckingWeighment();
            if (objChnR.DBResponse.Data != null)
                lstWeighment = ((List<CHN_WeighmentList>)objChnR.DBResponse.Data);
            return PartialView(lstWeighment);
        }

        public ActionResult ViewSealCheckingWeighment(int WeighmentId)
        {


            CHN_Weighment ObjWeighment = new CHN_Weighment();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (WeighmentId > 0)
            {

                ObjChnR.GetSealCheckingWeighmentById(WeighmentId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjWeighment = (CHN_Weighment)ObjChnR.DBResponse.Data;


                }
            }
            return PartialView(ObjWeighment);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteSealCheckingWeghment(int WeighmentId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteSealCheckingWeighment(WeighmentId);
            return Json(objChnR.DBResponse);
        }


        [HttpGet]
        public ActionResult EditSealCheckingWeighment(int WeighmentId)
        {
            CHN_Weighment ObjWeighment = new CHN_Weighment();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (WeighmentId > 0)
            {
                ObjChnR.GetSealCheckingWeighmentById(WeighmentId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjWeighment = (CHN_Weighment)ObjChnR.DBResponse.Data;
                }
            }
            return PartialView(ObjWeighment);
        }
        #endregion

        #region Shut Out

        // GET: LandingPage
        [HttpGet]
        public ActionResult CreateShutOutCargo()
        {
            //CHN_ShutOut ObjShutOut = new CHN_ShutOut();
            //CHN_EntryThroughGateRepository ObjCHNRR = new CHN_EntryThroughGateRepository();
            //ObjCHNRR.GetTruckSlipNo();
            //if (ObjCHNRR.DBResponse.Data != null)
            //{
            //    ViewBag.TruckSlipNoList = new SelectList((List<CHN_ShutOut>)ObjCHNRR.DBResponse.Data, "", "TruckSlipNo");
            //    ViewBag.ShutOut = true;
            //}
            //else
            //{
            //    ViewBag.TruckSlipNoList = null;
            //    ViewBag.ShutOut = true;
            //}
            //return PartialView("CreateShutOut", ObjShutOut);
            return PartialView("CreateShutOut");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditShutOut(CHN_ShutOut objShutOut)
        {
            ModelState.Remove("ShutOutId");
            CHN_EntryThroughGateRepository ObjCHNR = new CHN_EntryThroughGateRepository();
            if (ModelState.IsValid)
            {
                
                Login ObjLogin = (Login)Session["LoginUser"];
                objShutOut.UId = ObjLogin.Uid;
                ObjCHNR.AddEditShutOut(objShutOut);
                return Json(ObjCHNR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);

            }

        }

        [HttpGet]
        public ActionResult GetShutOutCargoList()
        {
            CHN_EntryThroughGateRepository ObjCHNR = new CHN_EntryThroughGateRepository();
            ObjCHNR.GetShutOutCargoList();
            IEnumerable<CHN_ShutOut > lstShutOut = new List<CHN_ShutOut>();
            if (ObjCHNR.DBResponse.Data != null)
            {
                lstShutOut = (IEnumerable<CHN_ShutOut>)ObjCHNR.DBResponse.Data;
            }
                return PartialView("ShutOutCargoList", lstShutOut);
        }

        public JsonResult GetTruckSlipDetails(string TruckSlipNo)
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GetTruckSlipNoDetails(TruckSlipNo);
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTruckSlipListForShutOutCargo()
        {
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            ObjChnR.GetTruckSlipNo();
            return Json(ObjChnR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ViewShutOut(int ShutOutId)
        {


            CHN_ShutOut ObjShuOut = new CHN_ShutOut();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (ShutOutId > 0)
            {

                ObjChnR.GetShutOutCargoById(ShutOutId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjShuOut = (CHN_ShutOut)ObjChnR.DBResponse.Data;


                }
            }
            return PartialView(ObjShuOut);
        }

        [HttpGet]
        public ActionResult EditShutOut(int ShutOutId)
        {
            CHN_ShutOut ObjShutOut = new CHN_ShutOut();
            CHN_EntryThroughGateRepository ObjChnR = new CHN_EntryThroughGateRepository();
            if (ShutOutId > 0)
            {
                ObjChnR.GetShutOutCargoById(ShutOutId);
                if (ObjChnR.DBResponse.Data != null)
                {
                    ObjShutOut = (CHN_ShutOut)ObjChnR.DBResponse.Data;
                }
            }
            return PartialView(ObjShutOut);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteShutOut(int ShutOutId)
        {
            CHN_EntryThroughGateRepository objChnR = new CHN_EntryThroughGateRepository();
            objChnR.DeleteShutOut(ShutOutId);
            return Json(objChnR.DBResponse);
        }

        #endregion

        #region Seal Checking Payment sheet

        public ActionResult CreateSealCheckingPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            CHN_ExportRepository objExport = new CHN_ExportRepository();

            objExport.GetTruckSlipNo();
            if (objExport.DBResponse.Status > 0)
                ViewBag.TruckSlipDetails = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            objExport.GetPaymentPartyForSealChekingInvoice();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        public JsonResult GetTruckSlipListForSealChekingPaymentSheet()
        {
            CHN_ExportRepository ObjChnExpR = new CHN_ExportRepository();
            ObjChnExpR.GetTruckSlipNo();
            return Json(ObjChnExpR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPaymentPartySealChekingPaymentSheet()
        {
            CHN_ExportRepository ObjChnExpR = new CHN_ExportRepository();
            ObjChnExpR.GetPaymentPartyForSealChekingInvoice();
            return Json(ObjChnExpR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayeeNameSealChekingPaymentSheet()
        {
            CHN_ExportRepository ObjChnExpR = new CHN_ExportRepository();
            ObjChnExpR.GetPaymentPartyForSealChekingInvoice();
            return Json(ObjChnExpR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int EntryId)
        {
            CHN_ExportRepository objExport = new CHN_ExportRepository();
            objExport.GetContainerForSealCheckingPaymentSheet(EntryId);
            if (objExport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int EntryId, string InvoiceType,string SEZ, int PartyId,
           List<Chn_PaymentSheetContainer> lstPaySheetContainer,
           int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            CHN_ExportRepository objChnRepo = new CHN_ExportRepository();
            //objChrgRepo.GetAllCharges();
            objChnRepo.GetSealCheckingPaymentSheet(InvoiceDate, EntryId, InvoiceType,SEZ, XMLText, InvoiceId, PartyId);
            var Output = (Chn_InvoiceSealChecking)objChnRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "EXPSEALCHEKING";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                //if (!Output.ShippingLineName.Contains(item.ShippingLineName))
                //    Output.ShippingLineName += item.ShippingLineName + ", ";
                if (!Output.CHAName.Contains(item.CHAName))
                    Output.CHAName += item.CHAName + ", ";
                //if (!Output.ImporterExporter.Contains(item.ImporterExporter))
                //    Output.ImporterExporter += item.ImporterExporter + ", ";
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
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new Chn_PostPaymentContainer
                    {
                        //CargoType = item.CargoType,
                        CartingDate = string.IsNullOrEmpty(item.CartingDate) ? (DateTime?)null : DateTime.Parse(item.CartingDate),
                        CFSCode = item.CFSCode,
                        //CIFValue = item.CIFValue,
                        ContainerNo = item.ContainerNo,
                        ArrivalDate = item.ArrivalDate,
                        ArrivalTime = item.ArrivalTime,
                        //DeliveryType = item.DeliveryType,
                        DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate) ? (DateTime?)null : DateTime.Parse(item.DestuffingDate),
                        //Duty = item.Duty,
                        GrossWt = item.GrossWeight,
                        //Insured = item.Insured,
                        NoOfPackages = item.NoOfPackages,
                        //Reefer = item.Reefer,
                        //RMS = item.RMS,
                        Size = item.Size,
                        SpaceOccupied = item.SpaceOccupied,
                        SpaceOccupiedUnit = item.SpaceOccupiedUnit,
                        StuffingDate = string.IsNullOrEmpty(item.StuffingDate) ? (DateTime?)null : DateTime.Parse(item.StuffingDate),
                        //WtPerUnit = item.WtPerPack,
                        AppraisementPerct = item.AppraisementPerct,
                        //HeavyScrap = item.HeavyScrap,
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

        [NonAction]
        private void CalculateCWCChargesContainer(Chn_PaymentSheetFinalModel finalModel, Chn_PaySheetChargeDetails baseModel)
        {
            try
            {
                //var A = JsonConvert.DeserializeObject<PaySheetChargeDetails>("{   \"lstPSContainer\": [     {       \"CFSCode\": \"CFSCode8\",       \"ContainerNo\": \"CONT1234\",       \"Size\": \"20\",       \"IsReefer\": false,       \"Insured\": \"No\"     },     {       \"CFSCode\": \"CFSCode9\",       \"ContainerNo\": \"CONT0001\",       \"Size\": \"40\",       \"IsReefer\": false,       \"Insured\": \"Yes\"     }   ],   \"lstEmptyGR\": [     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 2,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 38,       \"CFSCode\": \"CFSCode10\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 30,       \"RentAmount\": 0,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 31,       \"DaysRangeTo\": 40,       \"RentAmount\": 20.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"1\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 41,       \"DaysRangeTo\": 999,       \"RentAmount\": 60.00,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 52,       \"CFSCode\": \"CFSCode8\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"lstLoadedGR\": [     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 1,       \"DaysRangeTo\": 3,       \"RentAmount\": 0.10,       \"ElectricityCharge\": 1,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 4,       \"DaysRangeTo\": 15,       \"RentAmount\": 380.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     },     {       \"ContainerType\": \"2\",       \"CommodityType\": \"2\",       \"IsReefer\": false,       \"Size\": \"40\",       \"DaysRangeFrom\": 16,       \"DaysRangeTo\": 999,       \"RentAmount\": 500.00,       \"ElectricityCharge\": 0.10,       \"GroundRentPeriod\": 5,       \"CFSCode\": \"CFSCode9\",       \"FOBValue\": 10000000.00,       \"IsInsured\": 1     }   ],   \"InsuranceRate\": 12.15,   \"lstStorageRent\": [     {       \"CFSCode\": \"CFSCode9\",       \"ActualCUM\": 1500.00,       \"ActualSQM\": 1800.00,       \"ActualWeight\": 10000.00,       \"StuffCUM\": 225.000,       \"StuffSQM\": 270.000,       \"StuffWeight\": 1500.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     },     {       \"CFSCode\": \"CFSCode8\",       \"ActualCUM\": 5000.00,       \"ActualSQM\": 2500.00,       \"ActualWeight\": 5000.00,       \"StuffCUM\": 2000.000,       \"StuffSQM\": 1000.000,       \"StuffWeight\": 2000.00,       \"StorageDays\": 0,       \"StorageWeeks\": 0,       \"StorageMonths\": 0,       \"StorageMonthWeeks\": 0     }   ],   \"RateSQMPerWeek\": 456.00,   \"RateSQMPerMonth\": 56.00,   \"RateCUMPerWeek\": 4566.00,   \"RateMTPerDay\": 56.00 }");
                var EGRAmt = 0m;
                //baseModel.lstEmptyGR.GroupBy(o => o.CFSCode).ToList().ForEach(item =>
                //{
                //    foreach (var x in item.OrderBy(o => o.DaysRangeFrom))
                //    {
                //        var grp = x.GroundRentPeriod;
                //        var drf = x.DaysRangeFrom;
                //        var drt = x.DaysRangeTo;
                //        if (grp >= drt)
                //        {
                //            EGRAmt += x.RentAmount * ((drt - drf) + 1);
                //        }
                //        else
                //        {
                //            EGRAmt += x.RentAmount * ((grp - drf) + 1);
                //            break;
                //        }
                //    }
                //});
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
                //baseModel.lstStorageRent.ToList().ForEach(item =>
                //{
                //    var Amt1 = item.StuffCUM * item.StorageWeeks * baseModel.RateCUMPerWeek;
                //    var Amt2 = item.StuffWeight * item.StorageDays * baseModel.RateMTPerDay;
                //    var Amt3 = item.StorageDays < 30 ? (item.StuffSQM * item.StorageWeeks * baseModel.RateSQMPerWeek)
                //                                    : ((item.StuffSQM * item.StorageMonths * baseModel.RateSQMPerMonth) + (item.StuffSQM * item.StorageMonthWeeks * baseModel.RateSQMPerWeek));
                //    STAmt += Enumerable.Max(new[] { Amt1, Amt2, Amt3 });
                //});
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
        public JsonResult AddEditContPaymentSheet(FormCollection objForm)
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

                var invoiceData = JsonConvert.DeserializeObject<Chn_InvoiceSealChecking>(objForm["PaymentSheetModelJson"].ToString());
                // var invoiceData = objForm;
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    //item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.ArrivalDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
                CHN_ExportRepository objChargeMaster = new CHN_ExportRepository();
                objChargeMaster.AddEditContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "EXPSEALCHEKING");
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

    }
}
