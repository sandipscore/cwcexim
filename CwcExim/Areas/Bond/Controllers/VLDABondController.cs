using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Areas.Bond.Models;
using CwcExim.Filters;
using CwcExim.Models;
using System.IO;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using CwcExim.DAL;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Export.Models;
using System.Text;

namespace CwcExim.Areas.Bond.Controllers
{
    public class VLDABondController : BaseController
    {

        #region Space Availability Certificate

        [HttpGet]
        public ActionResult GetSpaceAvailList()
        {

            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSpaceAvailCert(1, 0);
            if (ObjBR.DBResponse.Data != null)
            {
                var JObject = Newtonsoft.Json.JsonConvert.SerializeObject(ObjBR.DBResponse.Data);
                var JObj = Newtonsoft.Json.Linq.JObject.Parse(JObject);
                ViewBag.SpaceAvailList = JObj["List"];
            }
            else
            {
                ViewBag.SpaceAvailList = null;
            }
            return PartialView("SpaceAvailList");
        }

        [HttpGet]
        public ActionResult LoadSpaceAvailCert(int SpaceappId, int Status)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            WFLDSpaceAvailableCert ObjSpaceAvail = new WFLDSpaceAvailableCert();
            ObjBR.GetSpaceAvailCertById(SpaceappId, Status);
            if (ObjBR.DBResponse.Data != null)
            {
                ObjSpaceAvail = (WFLDSpaceAvailableCert)ObjBR.DBResponse.Data;
            }
            return PartialView("SpaceAvailCert", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult LoadMoredata(int Status, int Skip)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSpaceAvailCert(Status, Skip);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSpaceAvailCert(WFLDSpaceAvailableCert ObjSpaceAvail)
        {
            ObjSpaceAvail.ApprovedBy = ((Login)Session["LoginUser"]).Uid;
            VLDABondRepository ObjBR = new VLDABondRepository();
            if (ModelState.IsValidField("SacNo") && ModelState.IsValidField("SacDate") && ModelState.IsValidField("AreaReserved") && ModelState.IsValidField("ValidUpto"))
            {
                ObjBR.AddEditBondSpaceAvailCert(ObjSpaceAvail);
                return Json(ObjBR.DBResponse);
            }
            /*else if(ObjSpaceAvail.IsApproved==2 && ModelState.IsValidField("NextApprovalDate"))
            {
                ObjBR.AddEditBondSpaceAvailCert(ObjSpaceAvail);
                return Json(ObjBR.DBResponse);
            }*/
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult PrintSpaceAvailCertApprove(int SpaceappId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSpaceAvailCertForPrint(SpaceappId);
            if (ObjBR.DBResponse.Data != null)
            {
                WFLDSpaceAvailCertPdf ObjSpaceAvail = new WFLDSpaceAvailCertPdf();
                ObjSpaceAvail = (WFLDSpaceAvailCertPdf)ObjBR.DBResponse.Data;
                string Path = GeneratePDFForSpaceAvailCertForApprove(ObjSpaceAvail, SpaceappId);
                return Json(new { Status = 1, Message = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PrintSpaceAvailCertReject(int SpaceappId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSpaceAvailCertForPrint(SpaceappId);
            if (ObjBR.DBResponse.Data != null)
            {
                WFLDSpaceAvailCertPdf ObjSpaceAvail = new WFLDSpaceAvailCertPdf();
                ObjSpaceAvail = (WFLDSpaceAvailCertPdf)ObjBR.DBResponse.Data;
                string Path = GeneratePDFForSpaceAvailCertForReject(ObjSpaceAvail, SpaceappId);
                return Json(new { Status = 1, Message = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GeneratePDFForSpaceAvailCertForApprove(WFLDSpaceAvailCertPdf ObjSpaceAvail, int SpaceappId)
        {
            string Html = "", Htmlnew;
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }*/
            //   Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.Importer + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOENoDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.ValidUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";

            //  Htmlnew= "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td valign='top'><img align='right' src='logo.png' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 22px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 17px; line-height: 22px;'>SPACE AVAILABILITY CERTIFICATE</label> </td><td valign='top'><img align='right' src='iso_logo.jpg' width='100'/></td></tr><tr><th style='font-size:13px;'>SAC No</th> <td style='font-size:12px;'><u>Dynamic part</u></td><th style='font-size:13px; text-align:right;'>Central Warehouse</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><th style='font-size:13px;'></th> <td style='font-size:12px;'></td><th style='font-size:13px; text-align:right;'>Date</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>No objection to receive the stock belonging to M/s. <u>Dynamice Part</u> (importer) and covered under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Area Reserved <u>Dynamice Part</u> sq.mtr. (Gross / Net)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Valid upto <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>Regretted</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Demand for space noted for the stock belonging to M/s. <u>Dynamice Part</u> (Imported) under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Space may be available only after <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>";

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='width:80%; font-size: 10px;'>'Warehousing for every one'</td>");
            html.Append("<td style='width:20%; text-align-right;'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 8pt; text-align: center;'>Document No. F/CD/23</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>" + ObjSpaceAvail.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + ObjSpaceAvail.EmailAddress + "</label><br/><label style='font-size: 7pt; font-weight:bold;'>SPACE AVAILABILITY CERTIFICATE</label></td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td colspan='6' style='font-size:7pt;'><b>SAC No :</b> <u>" + ObjSpaceAvail.SacNo + "</u></td>");
            html.Append("<td colspan='6' align='right' style='font-size:7pt; text-align:right;'><b>Central Warehouse :</b> <u>" + ObjSpaceAvail.Location + "</u></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='12' style='font-size:7pt; text-align:right;'><b>Date</b> <u>" + ObjSpaceAvail.SacDate + "</u></td></tr>");
            html.Append("");
            html.Append("<tr><td colspan='12' style='text-align: center;'><h2 style='font-size: 8pt; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2></td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>(i)</td><td colspan='2' width='85%' style='font-size:7.5pt; line-height:22px;'>No objection to receive the stock belonging to Mr./ Mrs./ Ms./ Miss " + ((ObjSpaceAvail.Importer == "" || ObjSpaceAvail.Importer == null) ? "_______________________" : "<u>" + ObjSpaceAvail.Importer + "</u>") + " (importer) and covered under BOL No. " + ((ObjSpaceAvail.BOLAWBNo == "" || ObjSpaceAvail.BOLAWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOLAWBNo + "</u>") + "  and BOL Date. " + ((ObjSpaceAvail.BOLAWBDate == "" || ObjSpaceAvail.BOLAWBDate == null) ? "_______________________" : " <u> " + ObjSpaceAvail.BOLAWBDate + " </u> ") + " and AWB No. " + ((ObjSpaceAvail.AWBNo == "" || ObjSpaceAvail.AWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.AWBNo + "</u>") + " and AWB Date " + ((ObjSpaceAvail.AWBDate == "" || ObjSpaceAvail.AWBDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.AWBDate + "</u>") + " and BOE No. " + ((ObjSpaceAvail.BOENo == "" || ObjSpaceAvail.BOENo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENo + "</u>") + " Date " + ((ObjSpaceAvail.BOENoDate == "" || ObjSpaceAvail.BOENoDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENoDate + "</u>") + " and IGM No. " + ((ObjSpaceAvail.IGMNo == "" || ObjSpaceAvail.IGMNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.IGMNo + "</u>") + " Date " + ((ObjSpaceAvail.IGMDate == "" || ObjSpaceAvail.IGMDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.IGMDate + "</u>") + " (if available)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>(ii)</td><td colspan='2' width='85%' style='font-size:7.5pt; line-height:22px;'>Area Reserved <u>" + ObjSpaceAvail.AreaReserved + "</u> sq.mtr. (Gross / Net)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>(iii)</td><td colspan='2' width='85%' style='font-size:7.5pt; line-height:22px; margin-bottom:15px;'>Valid upto <u>" + ObjSpaceAvail.ValidUpto + "</u></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>(iv)</td><td colspan='2' width='85%' style='font-size:7.5pt; line-height:22px; margin-bottom:15px;'>Cargo type <u>" + ObjSpaceAvail.CargoType + "</u></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>(v)</td><td colspan='2' width='85%' style='font-size:7.5pt; line-height:22px; margin-bottom:15px;'>Space type <u>" + ObjSpaceAvail.SpaceType + "</u></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td colspan='12'><span><br/><br/></span></td></tr>");
            html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>");
            html.Append("<tr><td colspan='12' style='font-size:7.5pt; text-align: right; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr>");
            html.Append("</tbody></table>");
            /*html.Append("</td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr>");
            html.Append("<td width='3%' valign='top' align='right' style='font-size:13px;'>(i)</td><td colspan='2' width='85%' style='font-size:14px;'>Regretted</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(ii)</td><td colspan='2' width='85%' style='font-size:14px;'>Demand for space noted for the stock belonging to Mr./ Mrs./ Ms./ Miss " + ((ObjSpaceAvail.Importer == "" || ObjSpaceAvail.Importer == null) ? "_______________________" : "<u>" + ObjSpaceAvail.Importer + "</u>") + " (Imported) under BOL/AWB No. " + ((ObjSpaceAvail.BOLAWBNo == "" || ObjSpaceAvail.BOLAWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOLAWBNo + "</u>") + "  and BOL/AWB Date. " + ((ObjSpaceAvail.BOLAWBDate == "" || ObjSpaceAvail.BOLAWBDate == null) ? "_______________________" : " <u> " + ObjSpaceAvail.BOLAWBDate + " </u> ") + " and BOE No. " + ((ObjSpaceAvail.BOENo == "" || ObjSpaceAvail.BOENo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENo + "</u>") + " Date " + ((ObjSpaceAvail.BOENoDate == "" || ObjSpaceAvail.BOENoDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENoDate + "</u>") + "(if available)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(iii)</td><td colspan='2' width='85%' style='font-size:14px;'>Space may be available only after <u></u></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>");
            html.Append("<tr><td width='55%' colspan='6'></td><td style='font-size:15px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>");
            */
            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            lstSB.Add(html.ToString());

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
        }

        [NonAction]
        public string GeneratePDFForSpaceAvailCertForReject(WFLDSpaceAvailCertPdf ObjSpaceAvail, int SpaceappId)
        {
            string Html = "", Htmlnew;
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }*/
            //   Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.Importer + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOENoDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.ValidUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";

            //  Htmlnew= "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td valign='top'><img align='right' src='logo.png' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 22px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 17px; line-height: 22px;'>SPACE AVAILABILITY CERTIFICATE</label> </td><td valign='top'><img align='right' src='iso_logo.jpg' width='100'/></td></tr><tr><th style='font-size:13px;'>SAC No</th> <td style='font-size:12px;'><u>Dynamic part</u></td><th style='font-size:13px; text-align:right;'>Central Warehouse</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><th style='font-size:13px;'></th> <td style='font-size:12px;'></td><th style='font-size:13px; text-align:right;'>Date</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>No objection to receive the stock belonging to M/s. <u>Dynamice Part</u> (importer) and covered under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Area Reserved <u>Dynamice Part</u> sq.mtr. (Gross / Net)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Valid upto <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>Regretted</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Demand for space noted for the stock belonging to M/s. <u>Dynamice Part</u> (Imported) under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Space may be available only after <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>";

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            html.Append("<td width='18%' align='right'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Document No. F/CD/23</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + ObjSpaceAvail.CompanyAddress + "</span><br/><label style='font-size: 14px;'>Email - " + ObjSpaceAvail.EmailAddress + "</label><br/><label style='font-size: 14px; font-weight:bold;'>SPACE AVAILABILITY CERTIFICATE</label></td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            // html.Append("<tr><th style='font-size:13px;' width='8%'>SAC No</th><td style='font-size:12px;'><u>" + ObjSpaceAvail.SacNo + "</u></td>");
            html.Append("<tr><td colspan='4' style='font-size:13px; text-align:right; font-weight:bold;'>Central Warehouse <span style='font-size:12px; width:13%; font-weight:normal;'><u>" + ObjSpaceAvail.Location + "</u></span></td> </tr>");
            /* html.Append("<tr><th style='font-size:13px;'></th> <td style='font-size:12px;'></td><th style='font-size:13px; text-align:right;'>Date</th>");
             html.Append("<td style='font-size:12px; width:10%;'><u>" + ObjSpaceAvail.SacDate + "</u></td></tr>");
             html.Append("<tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2></td></tr>");
             html.Append("<tr><td><span><br/></span></td></tr>");
             html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
             html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(i)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>No objection to receive the stock belonging to Mr./ Mrs./ Ms./ Miss " + ((ObjSpaceAvail.Importer == "" || ObjSpaceAvail.Importer == null) ? "_______________________" : "<u>" + ObjSpaceAvail.Importer + "</u>") + " (importer) and covered under BOL/AWB No. " + ((ObjSpaceAvail.BOLAWBNo == "" || ObjSpaceAvail.BOLAWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOLAWBNo + "</u>") + "  and BOL/AWB Date. " + ((ObjSpaceAvail.BOLAWBDate == "" || ObjSpaceAvail.BOLAWBDate == null) ? "_______________________" : " <u> " + ObjSpaceAvail.BOLAWBDate + " </u> ") + " and BOE No. " + ((ObjSpaceAvail.BOENo == "" || ObjSpaceAvail.BOENo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENo + "</u>") + " Date " + ((ObjSpaceAvail.BOENoDate == "" || ObjSpaceAvail.BOENoDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENoDate + "</u>") + " (if available)</td></tr>");
             html.Append("<tr><td><span><br/></span></td></tr>");
             html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(ii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>Area Reserved <u>" + ObjSpaceAvail.AreaReserved + "</u> sq.mtr. (Gross / Net)</td></tr>");
             html.Append("<tr><td><span><br/></span></td></tr>");
             html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(iii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px; margin-bottom:15px;'>Valid upto <u>" + ObjSpaceAvail.ValidUpto + "</u></td></tr></tbody></table></td></tr>");
             html.Append("<tr><td colspan='4'><span><br/><br/></span></td></tr>");
             html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td width='55%' colspan='6'></td>");
             html.Append("<td style='font-size:15px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table>");*/
            html.Append("<tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr>");
            html.Append("<td width='3%' valign='top' align='right' style='font-size:13px;'>(i)</td><td colspan='2' width='85%' style='font-size:14px;'>Regretted</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(ii)</td><td colspan='2' width='85%' style='font-size:14px;'>Demand for space noted for the stock belonging to Mr./ Mrs./ Ms./ Miss " + ((ObjSpaceAvail.Importer == "" || ObjSpaceAvail.Importer == null) ? "_______________________" : "<u>" + ObjSpaceAvail.Importer + "</u>") + " (Imported) under BOL No. " + ((ObjSpaceAvail.BOLAWBNo == "" || ObjSpaceAvail.BOLAWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOLAWBNo + "</u>") + "  and BOL Date. " + ((ObjSpaceAvail.BOLAWBDate == "" || ObjSpaceAvail.BOLAWBDate == null) ? "_______________________" : " <u> " + ObjSpaceAvail.BOLAWBDate + " </u> ") + " and AWB No. " + ((ObjSpaceAvail.AWBNo == "" || ObjSpaceAvail.AWBNo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.AWBNo + "</u>") + " and AWB Date " + ((ObjSpaceAvail.AWBDate == "" || ObjSpaceAvail.AWBDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.AWBDate + "</u>") + " and BOE No. " + ((ObjSpaceAvail.BOENo == "" || ObjSpaceAvail.BOENo == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENo + "</u>") + " Date " + ((ObjSpaceAvail.BOENoDate == "" || ObjSpaceAvail.BOENoDate == null) ? "_______________________" : "<u>" + ObjSpaceAvail.BOENoDate + "</u>") + "(if available)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(iii)</td><td colspan='2' width='85%' style='font-size:14px;'>Space may be available only after <u>" + ObjSpaceAvail.NextApprovalDate + "</u></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>");
            html.Append("<tr><td width='55%' colspan='6'></td><td style='font-size:15px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>");

            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            lstSB.Add(html.ToString());

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailCert" + SpaceappId + ".pdf";
        }

        [HttpGet]
        public JsonResult SearchUsingAppNoorAWBNo(string AWBNo, int State)
        {
            VLDABondRepository obj = new VLDABondRepository();
            obj.SearchUsingAWBNo(AWBNo, State);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadImporterReservationDetails(int CHAId, int ImpId, int GodownId, string SpaceType)
        {
            VLDABondRepository objRepo = new VLDABondRepository();
            objRepo.LoadReservationDetails(CHAId, ImpId, GodownId, SpaceType);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Application For Space Availability Certificate

        [HttpGet]
        public ActionResult CreateBondApp()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
            WFLDBondApp objBA = new WFLDBondApp();
            /*  ViewData["cha"] = ((Login)Session["LoginUser"]).CHA;
              if (Convert.ToBoolean(ViewData["cha"]) == false)
              {
                  objBR.ListOfCHA();
                  if (objBR.DBResponse.Data != null)    
                      ViewBag.ListOfCHA = objBR.DBResponse.Data;
                  else ViewBag.ListOfCHA = null;
              }
              else
              {
                  objBA.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                  objBA.CHAName = ((Login)Session["LoginUser"]).Name;
              }*/
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

            /*   ViewData["importer"] = ((Login)Session["LoginUser"]).Importer;
               if (Convert.ToBoolean(ViewData["importer"]) == false)
               {
                   objBR.ListOfImporter();
                   if (objBR.DBResponse.Data != null)
                       ViewBag.ListOfImporter = objBR.DBResponse.Data;
                   else ViewBag.ListOfImporter = null;
               }
               else
               {
                   objBA.ImporterId = ((Login)Session["LoginUser"]).EximTraderId;
                   objBA.ImporterName = ((Login)Session["LoginUser"]).Name;
               }*/
            Wfld_ImportRepository rep = new Wfld_ImportRepository();
            rep.ListOfImporterForPage("", 0);
            if (rep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(rep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.ImpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstImporter = null;
            }
            objBR.ListOfGodown();
            if (objBR.DBResponse.Data != null)
            {
                objBR.ListOfGodown();
                if (objBR.DBResponse.Data != null)
                    ViewBag.ListOfGodown = objBR.DBResponse.Data;
                else ViewBag.ListOfGodown = null;
            }
            objBA.ApplicationDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult EditBondApp(int SpaceappId)
        {
            // BondRepository objBR = new BondRepository();
            VLDABondRepository objHdb = new VLDABondRepository();
            WFLDBondApp objBA = new WFLDBondApp();
            WFLDBondApp objBANet = new WFLDBondApp();

            /*  ViewData["cha"] = ((Login)Session["LoginUser"]).CHA;
              if (Convert.ToBoolean(ViewData["cha"]) == false)
              {
                  objHdb.ListOfCHA();
                  if (objHdb.DBResponse.Data != null)
                      ViewBag.ListOfCHA = objHdb.DBResponse.Data;
                  else ViewBag.ListOfCHA = null;
              }
              else
              {
                  objBA.CHAId = ((Login)Session["LoginUser"]).EximTraderId;
                  objBA.CHAName = ((Login)Session["LoginUser"]).Name;
              }*/
            WFLD_ExportRepository ObjER = new WFLD_ExportRepository();
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
            /*    ViewData["importer"] = ((Login)Session["LoginUser"]).Importer;
                if (Convert.ToBoolean(ViewData["importer"]) == false)
                {
                    objHdb.ListOfImporter();
                    if (objHdb.DBResponse.Data != null)
                        ViewBag.ListOfImporter = objHdb.DBResponse.Data;
                    else ViewBag.ListOfImporter = null;
                }
                else
                {
                    objBA.ImporterId = ((Login)Session["LoginUser"]).EximTraderId;
                    objBA.ImporterName = ((Login)Session["LoginUser"]).Name;
                }*/
            Wfld_ImportRepository rep = new Wfld_ImportRepository();
            rep.ListOfImporterForPage("", 0);
            if (rep.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(rep.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.ImpState = Jobject["State"];
            }
            else
            {
                ViewBag.lstImporter = null;
            }
            objHdb.ListOfGodown();
            if (objHdb.DBResponse.Data != null)
            {
                objHdb.ListOfGodown();
                if (objHdb.DBResponse.Data != null)
                    ViewBag.ListOfGodown = objHdb.DBResponse.Data;
                else ViewBag.ListOfGodown = null;
            }
            objHdb.SpaceAvailabilityDetails(SpaceappId);
            if (objHdb.DBResponse.Data != null)
                objBA = (WFLDBondApp)objHdb.DBResponse.Data;

            objHdb.LoadReservationDetails(objBA.CHAId, objBA.ImporterId, objBA.GodownId, objBA.SpaceType);
            if (objHdb.DBResponse.Data != null)
                objBANet = (WFLDBondApp)objHdb.DBResponse.Data;

            if (objBANet.NetSpaceReservedCHA > 0)
            {
                objBA.NetSpaceReq = objBANet.NetSpaceReservedCHA;
                objBA.NetSpaceRemaining = objBANet.SpaceReservedCHA;
            }
            else
            {
                objBA.NetSpaceReq = objBANet.NetSpaceReservedIMP;
                objBA.NetSpaceRemaining = objBANet.SpaceReservedIMP;
            }

            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult ViewBondApp(int SpaceappId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondApp objBA = new WFLDBondApp();
            objBR.SpaceAvailabilityDetails(SpaceappId);
            if (objBR.DBResponse.Data != null)
                objBA = (WFLDBondApp)objBR.DBResponse.Data;
            return PartialView(objBA);
        }
        [HttpGet]
        public ActionResult ListOfBondApp()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WFLDListOfBondApp> lstApp = new List<WFLDListOfBondApp>();
            objBR.ListOfSpaceAvailability(0, "");
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WFLDListOfBondApp>)objBR.DBResponse.Data;
            return PartialView(lstApp);
        }
        [HttpGet]
        public JsonResult LoadMoreListOfBondApp(int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WFLDListOfBondApp> lstApp = new List<WFLDListOfBondApp>();
            objBR.ListOfSpaceAvailability(Page, "");
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WFLDListOfBondApp>)objBR.DBResponse.Data;
            return Json(lstApp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchOfBondApp(string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WFLDListOfBondApp> lstApp = new List<WFLDListOfBondApp>();
            objBR.ListOfSpaceAvailability(-1, Search);
            if (objBR.DBResponse.Data != null)
                lstApp = ((List<WFLDListOfBondApp>)objBR.DBResponse.Data);
            return PartialView("ListOfBondApp", lstApp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteBondApp(int SpaceappId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.DeleteBondApp(SpaceappId);
            return Json(objBR.DBResponse);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintBondApp(int SpaceappId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.GetSpaceAvailAppCertForPrint(SpaceappId);
            if (objBR.DBResponse.Data != null)
            {
                WFLDSpaceAvailAppCertPdf ObjSpaceAvail = new WFLDSpaceAvailAppCertPdf();
                ObjSpaceAvail = (WFLDSpaceAvailAppCertPdf)objBR.DBResponse.Data;
                string Path = GeneratePDFForSpaceAvailAppCert(ObjSpaceAvail, SpaceappId);
                return Json(new { Status = 1, Message = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GeneratePDFForSpaceAvailAppCert(WFLDSpaceAvailAppCertPdf ObjSpaceAvail, int SpaceappId)
        {
            string Html = "", Htmlnew;
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailAppCert" + SpaceappId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            //   Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.Importer + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.BOENoDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + ObjSpaceAvail.ValidUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";

            //  Htmlnew= "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td valign='top'><img align='right' src='logo.png' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 22px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='display: block; font-size: 17px; line-height: 22px;'>SPACE AVAILABILITY CERTIFICATE</label> </td><td valign='top'><img align='right' src='iso_logo.jpg' width='100'/></td></tr><tr><th style='font-size:13px;'>SAC No</th> <td style='font-size:12px;'><u>Dynamic part</u></td><th style='font-size:13px; text-align:right;'>Central Warehouse</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><th style='font-size:13px;'></th> <td style='font-size:12px;'></td><th style='font-size:13px; text-align:right;'>Date</th> <td style='font-size:12px; width:8%;'><u>Dynamic part</u></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>No objection to receive the stock belonging to M/s. <u>Dynamice Part</u> (importer) and covered under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Area Reserved <u>Dynamice Part</u> sq.mtr. (Gross / Net)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Valid upto <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(i)</td><td colspan='2' width='95%' style='font-size:12px;'>Regretted</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(ii)</td><td colspan='2' width='95%' style='font-size:12px;'>Demand for space noted for the stock belonging to M/s. <u>Dynamice Part</u> (Imported) under BOL/AWB No. <u>Dynamice Part</u> and BOE No. <u>Dynamice Part</u> Date <u>Dynamice Part</u> (if available)</td></tr><tr><td width='1%' valign='top' align='right' style='font-size:12px;'>(iii)</td><td colspan='2' width='95%' style='font-size:12px;'>Space may be available only after <u>Dynamice Part</u></td></tr></tbody></table></td></tr><tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td width='75%' colspan='7'></td><td style='font-size:17px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>";

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size: 7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            html.Append("<td width='18%' align='right'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 8pt; text-align: center;'>Document No. F/CD/22</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 7pt;'>Village-Valvada,NH-8,Taluka Umbargaon,Distt,Valsad-396105</label><br/><label style='font-size: 7pt;'>Email - " + ObjSpaceAvail.EmailAddress + "</label><br/><label style='font-size: 7pt; font-weight:bold;'>APPLICATION FOR AVAILABILITY OF SPACE</label></td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-size: 7pt; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            html.Append("<tr><td style='width:50%;'><b>SL.No. :</b> <span>" + ObjSpaceAvail.ApplicationNo + "</span>&nbsp;&nbsp;(to be put by the authorised Warehouse staff)</td>");
            html.Append("<td style='width:50%; text-align:right;'><b>Date :</b> <u>" + ObjSpaceAvail.SysDt + "</u></td></tr>");
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><td colspan='3' style='font-weight: bold;'><span>The Warehouse Manager,</span><br/><h5 style='margin:0; line-height: normal;'>" + ObjSpaceAvail.CompanyAddress + "</h5></td></tr>");
            html.Append("<tr><td colspan='4' style='font-weight: bold;'><span>Sir,</span><br/><label>Please issue space availability certificate for the under mentioned consignment</label></td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='font-size: 7pt; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right'>1.</td><td colspan='1' valign='top' width='50%'>Name of Importer</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.Importer + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>2.</td><td colspan='1' valign='top' width='50%''>Import Licence No.(Code)</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.ImportLicenseNo + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>3.</td><td colspan='1' valign='top' width='50%''>BOL No. & Date</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.BOLAWBNoDate + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>4.</td><td colspan='1' valign='top' width='50%''>AWB No. & Date</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.AWBNoDate + "</td></tr>");

            html.Append("<tr><td width='3%' valign='top' align='right'>4.</td><td colspan='1' valign='top' width='50%'>IGM No. & Date</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.IGMNoDate + "</td></tr>");

            html.Append("<tr><td width='3%' valign='top' align='right'>5.</td><td colspan='1' valign='top' width='50%'>In To Bond BOE No. & Date(Wherever available)</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.BOENoDate + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>6.</td><td colspan='1' valign='top' width='50%'>Description of Cargo</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.CargeDescrip + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>7.</td><td colspan='1' valign='top' width='50%'>No. of Units</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.Unit + " " + ObjSpaceAvail.DimensionUOM + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>8.</td><td colspan='1' valign='top' width='50%'>Nature of Packages (Metal, drum, fibre drum, wooden crate, bags, cartoons, etc.)</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.Packages + "," + ObjSpaceAvail.OthersValue + " </td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>9.</td><td colspan='1' valign='top' width='50%'>Dimensions per unit [UOM]</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.DUnit + " " + ObjSpaceAvail.DimensionUOM + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>10.</td><td colspan='1' valign='top' width='50%'>Weight [UOM]</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.Weight + " " + ObjSpaceAvail.WeightUOM + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>11.</td><td colspan='1' valign='top' width='50%'>Requirement of space unreserved in Sq. Mts. (gross or net to be specified)</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.AreaReserved + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>12.</td><td colspan='1' valign='top' width='50%'>Assessable / CIF Value</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.CIF + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>13.</td><td colspan='1' valign='top' width='50%'>Duty / Amount</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.Duty + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>14.</td><td colspan='1' valign='top' width='50%'>Nature of material (Non Hazardous, ODC, Hazardous, Chemical composition with flash point, if known)</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.NatureMaterial + "</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>15.</td><td colspan='1' valign='top' width='50%'>Expected date of warehousing</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.ExpDateofWarehouse + "</td></tr></tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right'>16.</td><td colspan='1' valign='top' width='50%'>Name of CHA</td><td>:</td><td colspan='2' valign='top' width='40%'>" + ObjSpaceAvail.CHAName + "</td></tr>");
            html.Append("</table></td></tr>");
            html.Append("<tr><td colspan='4'><p><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> I / We agree to abide by the provisions of the Customs Act, 1962 with the amendments in force. The terms & Conditions for storage of Bonded goods are also acceptable to me / us.</p></td></tr>");

            //new
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' width='100%' style='font-size: 7pt;'><tbody><tr><td colspan='8' width='70%' valign='top'><table cellspacing='0' cellpadding='5' width='100%' style='font-size: 7pt;'><tbody><tr><td valign='top' align='left'><em><b>Encls :</b></em></td><td colspan='2' valign='top' width='80%'>" + ObjSpaceAvail.Encls.TrimEnd(',') + "</td></tr></tbody></table></td><td colspan='4' width='30%'><table cellspacing='0' cellpadding='5' width='100%' style='font-size:7pt;'><tbody><tr><th align='left'>Sign. of Importer / Agent</th></tr><tr><th align='left'>Address :</th></tr><tr><td><span><br/></span></td></tr><tr><td align='left'>Phone :</td></tr><tr><td align='left'>Fax :</td></tr><tr><td align='left'>E-mail :</td></tr></tbody></table></td></tr></tbody></table></td></tr>");

            html.Append("<tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 8pt; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>FOR OFFICE USE</u></h2> </td></tr><tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size: 7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right'>1.</td><td colspan='2' width='95%'>Date of receipt of Application : " + ObjSpaceAvail.SysDt + " </td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>2.</td><td colspan='2' width='95%'>Sr. No. of SAC Register / Wait list No.</td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right'>3.</td><td colspan='2' width='95%'>Regret, if space not available</td></tr></tbody></table></td></tr><tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size: 7pt;'><tbody><tr><td width='65%' colspan='7'></td><td style='text-align: center; font-weight:bold;'>Sig. of Warehouse Manager / Authorised Person</td></tr></tbody></table></td></tr></tbody></table>");

            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            lstSB.Add(html.ToString());

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailAppCert" + SpaceappId + ".pdf";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondApp(WFLDBondApp objBA)
        {
            if (ModelState.IsValid)
            {
                VLDABondRepository objBR = new VLDABondRepository();
                objBR.AddEditBondApp(objBA);
                return Json(objBR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion

        #region Space Availability Certificate (Extend)

        [HttpGet]
        public ActionResult CreateSpaceAvailCertExtend()
        {
            WFLDSpaceAvailCertExtend ObjSpaceAvail = new WFLDSpaceAvailCertExtend();
            ObjSpaceAvail.ExtendOn = DateTime.Now.ToString("dd/MM/yyyy");
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSacNo();
            if (ObjBR.DBResponse.Data != null)
            {
                ViewBag.SacNoList = new SelectList((List<WFLDSpaceAvailableCert>)ObjBR.DBResponse.Data, "SpaceappId", "SacNo");
            }
            else
            {
                ViewBag.SacNoList = null;
            }
            return PartialView("CreateSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult EditSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            WFLDSpaceAvailCertExtend ObjSpaceAvail = new WFLDSpaceAvailCertExtend();
            if (SpaceAvailCertExtId > 0)
            {
                VLDABondRepository ObjBR = new VLDABondRepository();
                ObjBR.GetSpaceAvailCertExt(SpaceAvailCertExtId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjSpaceAvail = (WFLDSpaceAvailCertExtend)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("EditSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult ViewSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            WFLDSpaceAvailCertExtend ObjSpaceAvail = new WFLDSpaceAvailCertExtend();
            if (SpaceAvailCertExtId > 0)
            {
                VLDABondRepository ObjBR = new VLDABondRepository();
                ObjBR.GetSpaceAvailCertExt(SpaceAvailCertExtId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjSpaceAvail = (WFLDSpaceAvailCertExtend)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewSpaceAvailCertExtend", ObjSpaceAvail);
        }

        [HttpGet]
        public ActionResult GetSacNoDetails(int SpaceappId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSacNoDet(SpaceappId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSpaceAvailCertExtendList()
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetAllSpaceAvailCertExt();
            List<WFLDSpaceAvailCertExtend> LstSpaceAvail = new List<WFLDSpaceAvailCertExtend>();
            if (ObjBR.DBResponse.Data != null)
            {
                LstSpaceAvail = (List<WFLDSpaceAvailCertExtend>)ObjBR.DBResponse.Data;
            }
            return PartialView("SpaceAvailCertExtendList", LstSpaceAvail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSpaceAvailCertExtend(WFLDSpaceAvailCertExtend ObjSpaceAvail)
        {
            if (ModelState.IsValidField("AreaReserved") && ModelState.IsValidField("ExtendUpto") && ModelState.IsValidField("ExtendOn"))
            {
                VLDABondRepository ObjBR = new VLDABondRepository();
                ObjBR.AddEditBondSpaceAvailCertExt(ObjSpaceAvail);
                ModelState.Clear();
                return Json(ObjBR.DBResponse);
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
        public ActionResult DelSpaceAvailCertExtend(int SpaceAvailCertExtId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.DelBondSpaceAvailCertExt(SpaceAvailCertExtId);
            return Json(ObjBR.DBResponse);
        }


        #endregion

        #region  Approval for Space Availability Criteria (Extend)
        [HttpGet]
        public ActionResult AppSpcAvailExtend()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.ListOfSpaceAvailabilityExt("N", 0);
            if (objBR.DBResponse.Data != null)
            {
                var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objBR.DBResponse.Data);
                var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                ViewBag.Pending = jobject["List"];
                ViewBag.State = jobject["State"];
            }
            else
            {
                ViewBag.Pending = null;
                ViewBag.State = false;
            }
            return PartialView("AppSpcAvailExtend");
        }
        [HttpGet]
        public JsonResult LoadMoreSpcAvailExtend(string Status, int Skip)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.ListOfSpaceAvailabilityExt(Status, Skip);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchSpcAvailExtend(string Status, string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.SearchListSpaceAvailabilityExt(Status, Search);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSpaceAvailabilityExt(int SpaceAvailCertExtId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondAppExtendDetails objBD = new WFLDBondAppExtendDetails();
            objBR.GetSpaceAvailabilityExt(SpaceAvailCertExtId);
            if (objBR.DBResponse.Data != null)
                objBD = (WFLDBondAppExtendDetails)objBR.DBResponse.Data;
            return PartialView(objBD);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintExtendDetails(int SpaceAvailCertExtId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDSpaceAvailCertPdf objSAC = new WFLDSpaceAvailCertPdf();
            objBR.PrintSACExt(SpaceAvailCertExtId, Convert.ToInt32(Session["BranchId"]));
            if (objBR.DBResponse.Data != null)
            {
                objSAC = (WFLDSpaceAvailCertPdf)objBR.DBResponse.Data;
                string path = GenerateExtSAC(objSAC, SpaceAvailCertExtId);
                return Json(new { Status = 1, Message = path });
            }
            return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GenerateExtSAC(WFLDSpaceAvailCertPdf objSAC, int SpaceAvailCertExtId)
        {
            string Html = "";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/SpaceAvailCertExt" + SpaceAvailCertExtId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            //  Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;'><thead><tr><th colspan='2' style='text-align:left;'><span style='font-weight:600;font-size:11pt;border-bottom:1px solid #000;'>Space Availability Certificate(Extension)<br/><br/></span></th></tr></thead><tbody><tr><td style='width:70%;vertical-align: top;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Number</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.SacNo + "</span></td><td style='font-weight:600;text-align:left;padding:10px 5px;'>SAC Date</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.SacDate + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>No objection to receive the stock belonging to M/s:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.ImporterName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>CHA Name:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.CHAName + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And covered under BOL/AWB No.:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.BOLAWBNo + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>And BOE No & Date:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.BOE + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Area Reserved in Sq.Mtr:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.AreaReserved + "</span></td></tr><tr><td colspan='3' style='font-weight:600;text-align:left;padding:10px 5px;'>Valid Upto:</td><td style='text-align:left;padding:10px 5px;'><span>" + objSAC.ExtendUpto + "</span></td></tr></tbody></table></td><td style='width:30%;vertical-align: bottom;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td colspan='4'></td><td style='text-align:left;'><br/><br/><br/><br/><br/><br/><br/><br/>Signature of Warehouse<br/>Manager/Authorised Person</td></tr></tbody></table></td></tr> </tbody></table>";

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td colspan='4'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            html.Append("<td width='18%' align='right'>");
            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            html.Append("<tr><td style='border:1px solid #333;'>");
            html.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Document No.F/CD/23</div>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><td colspan='4'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>Container Freight Station, Kukatpally <br/> IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 14px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 14px; font-weight:bold;'>SPACE AVAILABILITY CERTIFICATE</label></td>");
            html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");

            html.Append("<tr><th style='font-size:13px;' width='8%'>SAC No</th> <td align='left' style='font-size:12px;'><u>" + objSAC.SacNo + "</u></td>");
            html.Append("<th style='font-size:13px; text-align:right;'>Central Warehouse</th> <td style='font-size:12px; width:10%;'><u>Hyderabad</u></td></tr>");
            html.Append("<tr><th style='font-size:13px;'></th> <td style='font-size:12px;'></td><th style='font-size:13px; text-align:right;'>Date</th>");
            html.Append("<td style='font-size:12px; width:10%;'><u>" + objSAC.SacDate + "</u></td></tr>");
            html.Append("<tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>SPACE AVILABILITY CERTIFICATE(SAC)</u></h2> </td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(i)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>No objection to receive the stock belonging to Mr./ Mrs./ Ms./ Miss <u>" + objSAC.Importer + "</u> (importer) and CHA Name:<u> " + objSAC.CHAName + "</u> covered under BOL/AWB No. <u>" + objSAC.BOLAWBNo + "</u> and BOE No. <u>" + objSAC.BOENo + "</u> Date <u>" + objSAC.BOENoDate + "</u>  (if available)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(ii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>Area Reserved <u>" + objSAC.AreaReserved + "</u> sq.mtr. (Gross / Net)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(iii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>Valid upto <u>" + objSAC.ValidUpto + "</u></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td colspan='4'><span><br/><br/></span></td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td width='55%' colspan='6'></td>");
            html.Append("<td style='font-size:15px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table>");
            html.Append("</td></tr><tr><td colspan='4' style='text-align: center;'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'><u>REGRET CERTIFICATE</u></h2> </td></tr>");
            html.Append("<tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr>");
            html.Append("<td width='3%' valign='top' align='right' style='font-size:13px;'>(i)</td><td colspan='2' width='85%' style='font-size:14px;'>Regretted</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(ii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>Demand for space noted for the stock belonging to Mr./ Mrs./ Ms./ Miss <u>" + objSAC.Importer + "</u> (Imported)  and CHA Name:<u> " + objSAC.CHAName + "</u> under BOL/AWB No. <u>" + objSAC.BOLAWBNo + "</u> and BOE No. <u>" + objSAC.BOENo + "</u> Date <u>" + objSAC.BOENoDate + "</u>  (if available)</td></tr>");
            html.Append("<tr><td><span><br/></span></td></tr>");
            html.Append("<tr><td width='3%' valign='top' align='right' style='font-size:13px;'>(iii)</td><td colspan='2' width='85%' style='font-size:14px; line-height:22px;'>Space may be available only after <u>" + objSAC.ValidUpto + "</u></td></tr></tbody></table></td></tr>");
            html.Append("<tr><td colspan='4'><span><br/><br/></span></td></tr><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>");
            html.Append("<tr><td width='55%' colspan='6'></td><td style='font-size:15px; text-align: left; font-weight:bold;'>Signature of Warehouse Manager / <br/>Authorised Person</td></tr></tbody></table></td></tr></tbody></table>");

            html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(html.ToString());

            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/SpaceAvailCertExt" + SpaceAvailCertExtId + ".pdf";
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult UpdateAppExtend(FormCollection fc)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.UpdateAppExtend(Convert.ToInt32(fc["SpaceAvailCertExtId"]), Convert.ToInt32(fc["IsApproved"]));
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Deposit Application
        [HttpGet]
        public ActionResult CreateDepositApp()
        {
            WFLDDepositApp ObjDepositApp = new WFLDDepositApp();
            ObjDepositApp.DepositDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjDepositApp.WRDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjDepositApp.BondDate = DateTime.Now.ToString("dd/MM/yyyy");

            // ObjDepositApp.CustomBondDate = DateTime.Now.ToString("dd/MM/yyyy");
            VLDABondRepository ObjHDB = new VLDABondRepository();
            BondRepository ObjBR = new BondRepository();
            ImportRepository ObjIR = new ImportRepository();
            ObjHDB.GetSacNoForDepositApp();
            if (ObjHDB.DBResponse.Data != null)
            {
                // ViewBag.SacNoList = new SelectList((List<WFLDDepositApp>)ObjHDB.DBResponse.Data, "SpaceappId", "SacNo","EntryId");
                //ViewBag.SacNoList =(List<WFLDDepositApp>)ObjHDB.DBResponse.Data;
                ViewBag.SacNoList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ObjHDB.DBResponse.Data));
            }
            else
            {
                ViewBag.SacNoList = null;
            }
            ObjBR.ListOfImporter();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
            else ViewBag.ListOfImporter = null;

            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
                ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            else
                ViewBag.GodownList = null;
            return PartialView("CreateDepositApp", ObjDepositApp);
        }

        [HttpGet]
        public ActionResult GetAllVehicle(string SacNo)
        {
            VLDABondRepository ObjCR = new VLDABondRepository();
            ObjCR.GetAllVehicle(SacNo);
            if (ObjCR.DBResponse.Status > 0)
                return Json(ObjCR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewDepositApp(int DepositAppId)
        {
            WFLDDepositApp ObjDepositApp = new WFLDDepositApp();
            ImportRepository ObjIR = new ImportRepository();
            if (DepositAppId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                VLDABondRepository ObjHDB = new VLDABondRepository();
                ObjHDB.GetDepositApp(DepositAppId);
                if (ObjHDB.DBResponse.Data != null)
                {
                    ObjDepositApp = (WFLDDepositApp)ObjHDB.DBResponse.Data;
                    ObjBR.ListOfImporter();
                    if (ObjBR.DBResponse.Data != null)
                        ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
                    else ViewBag.ListOfImporter = null;

                    ObjIR.ListOfGodown();
                    if (ObjIR.DBResponse.Data != null)
                        ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                    else
                        ViewBag.GodownList = null;
                }
            }
            return PartialView("ViewDepositApp", ObjDepositApp);
        }


        [HttpGet]
        public ActionResult GetSacNoDepositDetails(int SpaceappId, int EntryId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetSacNodepositDet(SpaceappId, EntryId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditDepositApp(int DepositAppId)
        {
            WFLDDepositApp ObjDepositApp = new WFLDDepositApp();
            ImportRepository ObjIR = new ImportRepository();
            if (DepositAppId > 0)
            {
                BondRepository ObjBR = new BondRepository();
                VLDABondRepository ObjHDB = new VLDABondRepository();
                ObjHDB.GetDepositApp(DepositAppId);
                if (ObjHDB.DBResponse.Data != null)
                {
                    ObjDepositApp = (WFLDDepositApp)ObjHDB.DBResponse.Data;
                    ObjBR.ListOfImporter();
                    if (ObjBR.DBResponse.Data != null)
                        ViewBag.ListOfImporter = ObjBR.DBResponse.Data;
                    else ViewBag.ListOfImporter = null;

                    ObjIR.ListOfGodown();
                    if (ObjIR.DBResponse.Data != null)
                        ViewBag.GodownList = new SelectList((List<Import.Models.Godown>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                    else
                        ViewBag.GodownList = null;

                }
            }
            return PartialView("EditDepositApp", ObjDepositApp);
        }

        //[HttpGet]
        //public ActionResult GetSacNoDetailForDepositApp(int SpaceappId)
        //{
        //    BondRepository ObjBR = new BondRepository();
        //   // ObjBR.GetSacNoDetForDepositApp(SpaceappId);
        //    return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetDepositAppList()
        {
            //BondRepository ObjBR = new BondRepository();
            VLDABondRepository ObjHDB = new VLDABondRepository();
            ObjHDB.GetAllDepositApp(0, "");
            List<WFLDDepositApp> LstDepositApp = new List<WFLDDepositApp>();
            if (ObjHDB.DBResponse.Data != null)
            {
                LstDepositApp = (List<WFLDDepositApp>)ObjHDB.DBResponse.Data;
            }
            return PartialView("DepositAppList", LstDepositApp);
        }
        [HttpGet]
        public JsonResult LoadMoreDepositAppList(int Page)
        {
            VLDABondRepository ObjHDB = new VLDABondRepository();
            ObjHDB.GetAllDepositApp(Page, "");
            List<WFLDDepositApp> LstDepositApp = new List<WFLDDepositApp>();
            if (ObjHDB.DBResponse.Data != null)
            {
                LstDepositApp = (List<WFLDDepositApp>)ObjHDB.DBResponse.Data;
            }
            return Json(LstDepositApp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchDepositAppList(string Search)
        {
            VLDABondRepository ObjHDB = new VLDABondRepository();
            ObjHDB.GetAllDepositApp(-1, Search);
            List<WFLDDepositApp> LstDepositApp = new List<WFLDDepositApp>();
            if (ObjHDB.DBResponse.Data != null)
            {
                LstDepositApp = (List<WFLDDepositApp>)ObjHDB.DBResponse.Data;
            }
            return PartialView("DepositAppList", LstDepositApp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditDepositApp(WFLDDepositApp ObjDepositApp)
        {
            if (ModelState.IsValid)
            {

                VLDABondRepository ObjBR = new VLDABondRepository();
                ObjBR.AddEditBondDepositApp(ObjDepositApp);
                ModelState.Clear();
                return Json(ObjBR.DBResponse);
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
        public JsonResult PrintDepositeApp(int DepositAppId)
        {
            if (DepositAppId > 0)
            {
                BondRepository objBR = new BondRepository();
                VLDABondRepository ObjHdb = new VLDABondRepository();
                WFLDPrintDA objDA = new WFLDPrintDA();
                ObjHdb.PrintDA(DepositAppId);
                if (ObjHdb.DBResponse.Data != null)
                {
                    objDA = (WFLDPrintDA)ObjHdb.DBResponse.Data;
                    string Remarks = "";
                    if (objDA != null && objDA.lstGodown != null && objDA.lstGodown.Count > 0)
                    {
                        Remarks = objDA.lstGodown[0].Remarks;
                    }
                    string html = "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td><td width='18%' align='right'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='border:1px solid #333;'><div style='padding: 5px 0; font-size: 7pt; text-align: center;'>Document No. F/CS/25</div></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
                    html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label>(A Government of India Undertaking)</label><br/><span style='font-size:12px;'>" + objDA.CompanyAddress + "</span><br/><label>Email - " + objDA.EmailAddress + "</label><br/><label style='font-weight:bold;'>DEPOSIT APPLICATION(CUSTOM BOND)</label><br/><label>(To be filled by Importer / Agent)</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></tbody></table></td></tr></tbody></table>";
                    html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody><tr><td colspan='6'><table cellspacing='0' cellpadding='5' width='100%' style='font-size:7pt;'><tbody> <tr><td colspan='12'><b>Deposit No. :</b><u>" + objDA.DepositeNo + "</u></td></tr> <tr><td colspan='11' style='font-size:11px;'>(To be put by the authorised warehouse staff)</td></tr></tbody></table></td> <td colspan='6' valign='top'><table cellspacing='0' cellpadding='5' width='100%' style='font-size:7pt; text-align:right;'><tbody><tr><td><b>Central Warehouse :</b><u>" + objDA.Location + "</u></td> </tr></tbody></table></td></tr> <tr><td colspan='10'></td><th style='font-size:7pt; text-align:right;'>Date</th> <td style='font-size:7pt; width:13%;'><u>" + objDA.DepositeDate + "</u></td></tr> </tbody></table>";
                    html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif; margin-top:10px;'><tbody><tr><td colspan='4'>Please accept the cargo covered under Bond No.<u>" + objDA.BondNo + "</u> Dated <u>" + objDA.BondDt + "</u></td></tr> <tr><td><span><br/></span></td></tr>  <tr><td colspan='4'>As per the enclosed into Bond Bill of Entry detailed below :</td></tr></tbody></table>";

                    //html += "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><th style='text-align:left;padding:5px;'>Deposit No:</th><td style='text-align:left;padding:5px;'>" + objDA.DepositeNo + "</td><th style='text-align:left;padding:5px;'>Deposit Date:</th><td style='text-align:left;padding:5px;'>" + objDA.DepositeDate + "</td></tr><tr><th style='text-align:left;padding:5px;'>Bond No:</th><td style='text-align:left;padding:5px;'>" + objDA.BondNo + "</td><th style='text-align:left;padding:5px;'>Bond Date:</th><td style='text-align:left;padding:5px;'>" + objDA.BondDt + "</td></tr><tr><th style='text-align:left;padding:5px;'>WR No:</th><td style='text-align:left;padding:5px;'>" + objDA.WRNo + "</td><th style='text-align:left;padding:5px;'>WR Date:</th><td style='text-align:left;padding:5px;'>" + objDA.WRDt + "</td></tr><tr><th style='text-align:left;padding:5px;'>In Bond BOE No:</th> <td style='text-align:left;padding:5px;'>" + objDA.BondBOENo + "</td> <th style='text-align:left;padding:5px;'>In Bond BOE Date:</th><td style='text-align:left;padding:5px;'>" + objDA.BondBOEDate + "</td></tr><tr><th style='text-align:left;padding:5px;'>Godown:</th><td style='text-align:left;padding:5px;' colspan='3'>" + objDA.GodownName + "</td></tr></tbody></table>";
                    html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Importer's Name</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>CHA</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Description of Cargo</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Units</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>SAC No. & Date</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Remarks</th></tr></thead><tbody>";
                    objDA.lstSAC.ForEach(item =>
                    {
                        html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ImporterName + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CHAName + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.NoOfUnits + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.SacNo + " & " + item.SacDate + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + Remarks + "</td></tr>";
                    });
                    html += "</tbody></table>";
                    html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt;'><tbody><tr><td><span><br/><br/></span></td></tr><tr><th colspan='12' style='text-align:right;'>Signature of Importer / Agent</th></tr></tbody></table>";
                    html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>";
                    html += "<tr><td colspan='4' style='text-align: center;'><h2 style='font-size:8pt; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>( AT OFFICE )</h2> </td></tr>";
                    html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'> A) </td><td colspan='2' width='85%' style='font-size:7pt;'>Entry in the Warehouse premises permitted</td></tr>";
                    html += "</tbody></table>";
                    html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/><br/></span></td></tr><tr><td width='50%' colspan='6' style='font-size:7.5pt;'><b>Gate I</b>/C</td><th width='50%' colspan='6' style='font-size:7.5pt; text-align: right; font-weight:bold;'>(Office Assistant)</th></tr></tbody></table>";
                    html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>";
                    html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'> B) </td><td colspan='2' width='85%' style='font-size:7pt;'>Please receive the cargo as detailded above & as per the in-To B/E enclosed, in the Godown No.<u>" + objDA.GodownName + "</u> on branded weight/after undertaking weighment.</td></tr>";
                    html += "</tbody></table>";
                    html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/><br/></span></td></tr> <tr><th colspan='12' style='font-size:7.5pt; text-align: right;'>(Office Assistant)</th></tr></tbody></table>";

                    //html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/></span></td></tr><tr><td width='65%' colspan='8' style='font-size:15px; font-weight:bold;'>C) At Godown</td></tr></tbody></table>";
                    //html += "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody><tr><td colspan='4'>Received Cargo as per details given below:- :</td></tr></tbody></table>";

                    //html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Godown/Stack No.</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Description of Cargo</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>No. of Units</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Weight(M. Ts)</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Area occupied in Sq.Mtrs.(net or gross)</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Condition of Packages</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Remarks</th></tr></thead><tbody>";
                    // Please change the loop as per godown
                    //objDA.lstGodown.ForEach(item =>
                    //{
                    //    html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.GodownName + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDesc + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Units + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Weight + "</td>  <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.AreaReserved + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.NatureOfPackages + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Remarks + "</td></tr>";
                    //});
                    //html += "</tbody></table>";
                    //html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/></span></td></tr><tr><td width='75%' colspan='10'></td><td style='font-size:14px; text-align: center; font-weight:bold;'>(Godown Incharge)</td></tr></tbody></table>";
                    //html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody> <tr><td width='3%' valign='top' align='right' style='font-size:12px;'> D) </td><td colspan='2' width='85%' style='font-size:12px;'>Agred with measurement of area as occupied and status of consignment.</td></tr> </tbody></table>";
                    //html += "<table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td><span><br/></span></td></tr><tr><td width='75%' colspan='10'></td><td style='font-size:14px; text-align: center; font-weight:bold;'>(Importer / CHA)</td></tr></tbody></table>";

                    //html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>";
                    //html += "<tr><td><span><br/></span></td></tr>";
                    //html += "<tr><td colspan='12'><h2 style='font-size: 16px; padding-bottom: 10px; text-align: center; margin: 0;'><u>FOR OFFICE USE</u></h2></td></tr>";
                    //html += "<tr><td style='font-size:12px;' width='3%' valign='top' align='right'>E)</td><td colspan='2' style='font-size:12px;' width='85%'> Entered in the Bond Register at Folio No. </td></tr>";
                    //html += "<tr><td><span><br/><br/></span></td></tr>";
                    //html += "<tr><td width='73%' colspan='10'></td><td style='font-size:14px; text-align: center; font-weight:bold;'>(OFFICE ASSISTANT)</td></tr>";
                    //html += "</tbody></table>";



                    html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                    html = html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                    if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf"))
                        System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf");

                    using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                    {
                        rh.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf", html);
                    }
                    return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/" + "DepositeApp" + DepositAppId + ".pdf" });
                }
                else return Json(new { Status = -1, Message = "Error" });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        #endregion

        #region Work Order For Unloading & Delivery
        [HttpGet]
        public ActionResult CreateWOForUnloading()
        {
            BondRepository objBR = new BondRepository();
            VLDABondRepository objHDB = new VLDABondRepository();
            objHDB.GetBondNoForWOUnloadingNew();
            if (objHDB.DBResponse.Data != null)
                ViewBag.BondNo = objHDB.DBResponse.Data;
            else
                ViewBag.BondNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.WoType = "U";
            return PartialView();
        }
        [HttpGet]
        public ActionResult CreateWOForDelivery()
        {
            BondRepository objBR = new BondRepository();
            VLDABondRepository objHDB = new VLDABondRepository();
            objHDB.GetBondNoForWODeliveryNew();
            if (objHDB.DBResponse.Data != null)
                ViewBag.BondNo = objHDB.DBResponse.Data;
            else
                ViewBag.BondNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.WoType = "D";
            return PartialView();
        }
        [HttpGet]
        public ActionResult EditWODeli(int BondWOId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondWODeli objBond = new WFLDBondWODeli();
            objBR.ListOfWODeliDetails(BondWOId);
            if (objBR.DBResponse.Data != null)
                objBond = (WFLDBondWODeli)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ViewWODeli(int BondWOId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondWODeli objBond = new WFLDBondWODeli();
            objBR.ListOfWODeliDetails(BondWOId);
            if (objBR.DBResponse.Data != null)
                objBond = (WFLDBondWODeli)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ListOfWODeli(string WoType)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListOfWODeli(WoType, 0, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpGet]
        public JsonResult LoadMoreListOfWODeli(string WoType, int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListOfWODeli(WoType, Page, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchListOfWODeli(string WoType, string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListOfWODeli(WoType, -1, Search);
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return PartialView("ListOfWODeli", objList);
        }

        [HttpGet]
        public ActionResult ListWOForUnloading(string WoType)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListWOForUnloading(WoType, 0, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return PartialView("ListOfWODeli", objList);
        }
        [HttpGet]
        public JsonResult LoadMoreListWOForUnloading(string WoType, int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListWOForUnloading(WoType, Page, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchWOForUnloading(string WoType, string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWFLDBondWODeli> objList = new List<ListOfWFLDBondWODeli>();
            objBR.ListWOForUnloading(WoType, -1, Search);
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWFLDBondWODeli>)objBR.DBResponse.Data;
            return PartialView("ListOfWODeli", objList);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditWODeli(FormCollection fc)
        {

            VLDABondRepository objBR = new VLDABondRepository();
            objBR.AddEditWODeliDetails(Convert.ToInt32(fc["BondWOId"]), Convert.ToInt32(fc["DepositAppId"]), fc["WorkOrderDate"].ToString(), fc["DeliveryNo"].ToString(),
                Convert.ToInt32(fc["CargoUnits"]), Convert.ToDecimal(fc["CargoWeight"]), fc["WOType"].ToString(), fc["BondNo"].ToString(), fc["BondDate"].ToString(), fc["SacNo"].ToString(),
                fc["SacDate"].ToString(), fc["WRNo"].ToString(), fc["WRDate"].ToString(), fc["GodownName"].ToString(), fc["CargoDescription"].ToString());
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDetailsAgainstBondNo(int DepositAppId, string WoType)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.GetDetailsforBondNo(DepositAppId, WoType);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Unloading at Bonded Warehouse
        [HttpGet]
        public ActionResult CreateWOUnloading()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.GetBondNoForWOUnloading();
            if (objBR.DBResponse.Data != null)
                ViewBag.WorkOrderNo = objBR.DBResponse.Data;
            else
                ViewBag.WorkOrderNo = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return PartialView();
        }
        [HttpGet]
        public ActionResult EditWOUnloading(int UnloadingId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondWOUnloading objBond = new WFLDBondWOUnloading();
            objBR.WOUnloadingDetails(UnloadingId, "EDIT");
            if (objBR.DBResponse.Data != null)
            {
                var jobj = JsonConvert.SerializeObject(objBR.DBResponse.Data);
                var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                ViewBag.Godown = jobject["lstGodown"];
                objBond = JsonConvert.DeserializeObject<WFLDBondWOUnloading>(JsonConvert.SerializeObject(jobject["objBond"]));
            }
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ViewWOUnloading(int UnloadingId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WFLDBondWOUnloading objBond = new WFLDBondWOUnloading();
            objBR.WOUnloadingDetails(UnloadingId);
            if (objBR.DBResponse.Data != null)
                objBond = (WFLDBondWOUnloading)objBR.DBResponse.Data;
            return PartialView(objBond);
        }
        [HttpGet]
        public ActionResult ListOfWOUnloading()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWOunloading> objList = new List<ListOfWOunloading>();
            objBR.ListForWOUnloading(0, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWOunloading>)objBR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpGet]
        public JsonResult LoadMoreListOfWOUnloading(int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWOunloading> objList = new List<ListOfWOunloading>();
            objBR.ListForWOUnloading(Page, "");
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWOunloading>)objBR.DBResponse.Data;
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchOfWOUnloading(string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            List<ListOfWOunloading> objList = new List<ListOfWOunloading>();
            objBR.ListForWOUnloading(-1, Search);
            if (objBR.DBResponse.Data != null)
                objList = (List<ListOfWOunloading>)objBR.DBResponse.Data;
            return PartialView("ListOfWOUnloading", objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditWOUnloading(WFLDBondWOUnloading obj)
        {
            if (ModelState.IsValid)
            {
                VLDABondRepository objBR = new VLDABondRepository();
                objBR.AddEditWOUnloading(obj);
                return Json(objBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                // return Json(new { Status = 1, Message = ErrorMessage });
                return Json(new { Status = -1, Message = "Error" });
            }
        }
        [HttpGet]
        public JsonResult GetDetailsfrUnloading(int DepositAppId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.GetBondNoForWOUnloadingDet(DepositAppId);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintWOUnloading(int UnloadingId)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.PrintWOUnloading(UnloadingId);
            if (objBR.DBResponse.Data != null)
            {
                WFLDBondWOUnloadingPDF ObjUnload = new WFLDBondWOUnloadingPDF();
                ObjUnload = (WFLDBondWOUnloadingPDF)objBR.DBResponse.Data;
                string Path = GeneratePDFForWOUnloading(ObjUnload, UnloadingId);
                return Json(new { Status = 1, Message = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GeneratePDFForWOUnloading(WFLDBondWOUnloadingPDF ObjUnload, int UnloadingId)
        {
            string Html = "";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/BondUnloading" + UnloadingId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }


            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td width='65%' style='font-size:8pt;'>'Warehousing for every one'</td><td width='18%' align='right'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='border:1px solid #333;'><div style='padding: 5px 0; font-size: 7pt; text-align: center;'>Document No. F/CS/25</div></td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; font-size:7pt;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label>(A Government of India Undertaking)</label><br/><span>" + ObjUnload.CompanyAddress + "</span><br/><label>Email - " + ObjUnload.EmailAddress + "</label><br/><label style='font-weight:bold;'>Bonded Warehouse Unloading</label></td><td valign='top'><img align='right' src='ISO' width='100'/></td></tr></tbody></table></td></tr></tbody></table>";

            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif; margin-top:10px;'><tbody> <tr><td width='70%'><table cellspacing='0' cellpadding='5' width='100%' style='font-size:7pt;'><tbody><tr><td><b>Deposit No. :</b> <u>" + ObjUnload.DepositNo + "</u></td></tr> <tr><td>(To be put by the authorised warehouse staff)</td></tr></tbody></table></td> <td width='30%' valign='top' style='text-align: right;'><table cellspacing='0' cellpadding='5' width='100%' style='font-size:7pt; text-align: right;'><tbody><tr> <td> <b>Central Warehouse :</b> <u>" + ObjUnload.Location + "</u></td> </tr></tbody></table></td></tr></tbody></table>";
            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'><tbody> <tr><td colspan='12' style='font-size:7pt; text-align:right;'> <b>Date</b> <u>" + ObjUnload.UnloadingDate + "</u></td></tr> </tbody></table>";

            Html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/></span></td></tr><tr><td width='65%' colspan='8' style='font-size:8; font-weight:bold;'>At Godown</td></tr></tbody></table>";
            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody><tr><td colspan='4'>Received Cargo as per details given below:- :</td></tr></tbody></table>";

            Html += "<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Godown/Stack No.</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Description of Cargo</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>No. of Units</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Weight(M. Ts)</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Area occupied in Sq.Mtrs.(net or gross)</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Condition of Packages</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;width:10%;'>Remarks</th></tr></thead><tbody>";
            //Please change the loop as per godown
            //objDA.lstGodown.ForEach(item =>
            //{
            Html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.GodownNo + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.NoOfUnits + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.Weight + "</td>  <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.Area + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.PkgCondition + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + ObjUnload.Remarks + "</td></tr>";
            //});
            Html += "</tbody></table>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody><tr><td><span><br/></span></td></tr> <tr><th colspan='12' style='font-size:7.5; text-align: right; font-weight:bold;'>(Godown Incharge)</th></tr></tbody></table>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody> <tr><td width='3%' valign='top' align='right' style='font-size:12px;'></td><td colspan='2' width='85%' style='font-size:7pt;'>Agreed with measurement of area as occupied and status of consignment.</td></tr> </tbody></table>";
            Html += "<table cellspacing='0' cellpadding='5' style='width:100%; border-bottom: 2px solid #333;'><tbody><tr><td><span><br/></span></td></tr> <tr><td colspan='12' style='font-size:7.5pt; text-align: right; font-weight:bold;'>(Importer / CHA)</td></tr></tbody></table>";

            Html += "<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>";
            Html += "<tr><td><span><br/></span></td></tr>";
            Html += "<tr><td colspan='12'><h2 style='font-size: 7.5pt; padding-bottom: 10px; text-align: center; margin: 0;'><u>FOR OFFICE USE</u></h2></td></tr>";
            Html += "<tr><td style='font-size:12px;' width='3%' valign='top' align='right'></td><td colspan='2' style='font-size:7pt;' width='85%'> Entered in the Bond Register at Folio No. </td></tr>";
            Html += "<tr><td><span><br/><br/></span></td></tr>";
            Html += "<tr><td width='73%' colspan='10'></td><td style='font-size:7.5pt; text-align: center; font-weight:bold;'>(OFFICE ASSISTANT)</td></tr>";
            Html += "</tbody></table>";

            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            Html = Html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));


            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/BondUnloading" + UnloadingId + ".pdf";
        }


        #endregion

        #region Bond SAC Payment Sheet

        [HttpGet]
        public ActionResult CreateBondAdvPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetSACNoForAdvBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondAdvPaymentSheet(string DeliveryDate, string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour, string ExportUnder)
        {

            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetBondAdvancePaymentSheet(DeliveryDate, InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, UptoDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, CasualLabour, InvoiceType, Area, 0, 0, ExportUnder);

            var objBondPostPaymentSheet = new WFLD_PostPaymentSheet();
            var Output = (WFLD_PostPaymentSheet)objBond.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "BNDadv";
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
                    //  Output.ArrivalDate += item.ArrivalDate + " " + item.ArrivalTime + ", ";
                    if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                        //   Output.DestuffingDate += item.DestuffingDate + ", ";
                        if (!Output.StuffingDate.Contains(item.StuffingDate))
                            //   Output.StuffingDate += item.StuffingDate + ", ";
                            if (!Output.CartingDate.Contains(item.CartingDate))
                                //   Output.CartingDate += item.CartingDate + ", ";
                                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                                {
                                    Output.lstPostPaymentCont.Add(new WFLD_PostPaymentContainer
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



            // return Json(Output, JsonRequestBehavior.AllowGet);
            /**********BOL PRINT**************/

            /********************************/
            // return Json(new { Output });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondAdvPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                int BillToParty = objForm["BillToParty"] == "" ? 0 : Convert.ToInt32(objForm["BillToParty"].ToString());
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

                //var invoiceData = JsonConvert.DeserializeObject<WFLD_PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                var invoiceData = JsonConvert.DeserializeObject<WFLD_LoadedContainerPayment>(objForm["PaymentSheetModelJson"].ToString());
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
                int Vno = 0;
                //CHNBondRepository objChargeMaster = new CHNBondRepository();
                VLDABondRepository objChargeMaster = new VLDABondRepository();
                objChargeMaster.AddEditInvoice(invoiceData, Vno, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDadv", BillToParty);

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
        public ActionResult ListofSACPaymentSheet()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldSACInvoiceList> lstApp = new List<WfldSACInvoiceList>();
            objBR.ListofSACPaymentSheet(0, "", null);
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WfldSACInvoiceList>)objBR.DBResponse.Data;
            return PartialView(lstApp);
        }
        [HttpGet]
        public JsonResult LoadMoreListofSACPaymentSheet(int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldSACInvoiceList> lstApp = new List<WfldSACInvoiceList>();
            objBR.ListofSACPaymentSheet(Page, "", null);
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WfldSACInvoiceList>)objBR.DBResponse.Data;
            return Json(lstApp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchListofSACPaymentSheet(string InvoiceNo, string InvoiceDate)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldSACInvoiceList> lstApp = new List<WfldSACInvoiceList>();
            objBR.ListofSACPaymentSheet(-1, InvoiceNo, InvoiceDate);
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WfldSACInvoiceList>)objBR.DBResponse.Data;
            return PartialView("ListofSACPaymentSheet", lstApp);
        }
        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            VLDABondRepository objExport = new VLDABondRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            VLDABondRepository objExport = new VLDABondRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            VLDABondRepository objExport = new VLDABondRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            VLDABondRepository objExport = new VLDABondRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Delivery Order

        [HttpGet]
        public ActionResult CreateDeliveryOrder()
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetWOForDeliveryOrder();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.lstBond = ObjBR.DBResponse.Data;
            else
                ViewBag.lstBond = null;
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditDeliveryOrder(int DeliveryOrderId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            WFLDDeliveryOrder ObjDeliveryOrder = new WFLDDeliveryOrder();
            ObjBR.GetDeliveryOrder(DeliveryOrderId);
            if (ObjBR.DBResponse.Data != null)
            {
                ObjDeliveryOrder = (WFLDDeliveryOrder)ObjBR.DBResponse.Data;
                //var jobj = JsonConvert.SerializeObject(ObjBR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.Godown = jobject["lstGodown"];
                //objBond = JsonConvert.DeserializeObject<BondWOUnloading>(JsonConvert.SerializeObject(jobject["objBond"]));
            }
            return PartialView(ObjDeliveryOrder);
        }

        [HttpGet]
        public ActionResult ViewDeliveryOrder(int DeliveryOrderId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            WFLDDeliveryOrder ObjDeliveryOrder = new WFLDDeliveryOrder();
            ObjBR.GetDeliveryOrder(DeliveryOrderId);
            if (ObjBR.DBResponse.Data != null)
                ObjDeliveryOrder = (WFLDDeliveryOrder)ObjBR.DBResponse.Data;
            return PartialView(ObjDeliveryOrder);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryOrder()
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            List<ListOfDeliveryOrder> LstDeliveryOrder = new List<ListOfDeliveryOrder>();
            ObjBR.GetAllDeliveryOrder(0, "");
            if (ObjBR.DBResponse.Data != null)
                LstDeliveryOrder = (List<ListOfDeliveryOrder>)ObjBR.DBResponse.Data;
            return PartialView("DeliveryOrderList", LstDeliveryOrder);
        }
        [HttpGet]
        public JsonResult LoadMoreListOfDeliveryOrder(int Page)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            List<ListOfDeliveryOrder> LstDeliveryOrder = new List<ListOfDeliveryOrder>();
            ObjBR.GetAllDeliveryOrder(Page, "");
            if (ObjBR.DBResponse.Data != null)
                LstDeliveryOrder = (List<ListOfDeliveryOrder>)ObjBR.DBResponse.Data;
            return Json(LstDeliveryOrder, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchOfDeliveryOrder(string Search)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            List<ListOfDeliveryOrder> LstDeliveryOrder = new List<ListOfDeliveryOrder>();
            ObjBR.GetAllDeliveryOrder(-1, Search);
            if (ObjBR.DBResponse.Data != null)
                LstDeliveryOrder = (List<ListOfDeliveryOrder>)ObjBR.DBResponse.Data;
            return PartialView("DeliveryOrderList", LstDeliveryOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryOrder(WFLDDeliveryOrder ObjDeliveryOrder)
        {
            if (ModelState.IsValid)
            {
                VLDABondRepository ObjBR = new VLDABondRepository();
                string DeliveryOrderXml = "";
                if (ObjDeliveryOrder.DeliveryOrderXml != "")
                {
                    ObjDeliveryOrder.LstDeliveryOrderhdb = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WFLDDeliveryOrderDtl>>(ObjDeliveryOrder.DeliveryOrderXml);
                    DeliveryOrderXml = Utility.CreateXML(ObjDeliveryOrder.LstDeliveryOrderhdb);
                }
                ObjBR.AddEditDeliveryOrder(ObjDeliveryOrder, DeliveryOrderXml);
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetDetForDeliveryOrder(int SpaceAppId, string BondNo = "")
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetWODetForDeliveryOrder(SpaceAppId, BondNo);

            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepositNoForDelvOrder(int SpaceAppId, string BondNo = "")
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetDepositNoForDelvOrder(SpaceAppId, BondNo);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepositNoDetForDelvOrder(int DepositAppId)
        {
            VLDABondRepository ObjBR = new VLDABondRepository();
            ObjBR.GetDepositDetForDelvOrder(DepositAppId);
            return Json(ObjBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintDeliveryApp(int DeliveryAppId)
        {
            if (DeliveryAppId > 0)
            {
                //  BondRepository objBR = new BondRepository();
                VLDABondRepository ObjHdb = new VLDABondRepository();
                WFLDDeliveryOrder objDA = new WFLDDeliveryOrder();
                ObjHdb.PrintDelivery(DeliveryAppId);
                if (ObjHdb.DBResponse.Data != null)
                {
                    objDA = (WFLDDeliveryOrder)ObjHdb.DBResponse.Data;
                    string html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td colspan='12'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>";
                    html += "<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td><td width='7%' align='right'><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='border:1px solid #333;'><div style='padding: 5px 0; font-size: 7pt; text-align: center;'>F/CD/28</div></td>";

                    html += "</tr></tbody></table></td></tr></tbody></table></td></tr><tr><td colspan='12'> <table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'> <tbody> <tr> <td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='70%' valign='top' align='center'>";

                    html += "<h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>" + objDA.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>" + objDA.CompanyEmail + "</label><br/><label style='font-size: 7pt; font-weight:bold;'>DELIVERY ORDER (CUSTOM BOND)</label><br/><label style='font-size: 7pt;'>(To be filled by Importer / Agent)</label></td>";

                    html += "<td valign='top'><img align='right' src='ISO' width='100'/></td></tr></tbody> </table> </td></tr><tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'> <tbody><tr><td colspan='6' width='50%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th width='35%' align='left' style='font-size:7pt;'>Delivery Order No.</th> <td colspan='7' style='font-size:12px;'><u>" + objDA.DeliveryOrderNo + "</u></td></tr>";
                    //objDA.LstDeliveryOrder.ForEach(item =>
                    //{
                    //    html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDescription + " & " + item.CargoDescription + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Remarks</td></tr>";
                    //});
                    //html += "</tbody></table>";
                    html += "</tbody></table></td><td colspan='6' width='50%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:7pt; text-align:right;'>Central Warehouse : <span>&nbsp;</span></th><td style='font-size:7pt; width:27%;'>" + objDA.CompanyLoaction + "</td></tr></tbody></table></td></tr><tr>";
                    html += "<td colspan='6' width='50%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td colspan='8' style='font-size:7pt;'>(To be put by the authorised warehouse staff)</td></tr></tbody></table></td><td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:7pt; text-align:right;'>Date</th> <td style='font-size:7pt; width:20%;'><u>" + objDA.DeliveryOrderDate + "</u></td></tr></tbody></table></td></tr>";
                    html += "<tr><td colspan='6' width='50%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td colspan='8' style='font-size:7pt;'>Please deliver the cargo covered under Bond No. <u>" + objDA.BondNo + "</u></td></tr></tbody></table></td><td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:7pt; text-align:right;'>Dated</th> <td style='font-size:7pt; width:20%;'><u>" + objDA.BondDate + "</u></td></tr></tbody></table></td></tr>";
                    html += "<tr><td colspan='6' width='50%'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td colspan='8' style='font-size:7pt;'>As per the Ex-Bond Bill of Entry No. <u>" + objDA.LstDeliveryOrderhdb[0].ExBoeNo + "</u></td></tr></tbody></table></td><td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><th style='font-size:7pt; text-align:right;'>Dated</th> <td style='font-size:7pt; width:20%;'><u>" + objDA.LstDeliveryOrderhdb[0].ExBoeDate + "</u></td></tr></tbody></table></td></tr></tbody></table></td></tr>";
                    html += "<tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Importer's Name</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>CHA</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Description of Cargo</th>";
                    html += "<th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>No. of Units</th> <th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Weight<br/>(Qtls/MT)(applicable)</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>SAC No. & Date</th><th style='font-weight:600;text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>Remarks</th></tr></thead><tbody>";
                    String sdt = "";
                    String ddt = "";
                    String Dno = "";
                    Decimal sreq = 0;
                    Decimal Arev = 0;
                    String Rno = "";
                    Decimal InsAmt = 0;
                    Decimal StoAmt = 0;
                    String Fdt = "";
                    String ToDate = "";
                    int days = 0;
                    int wk = 0;
                    Decimal InvInsAmt = 0;
                    Decimal InvStoAmt = 0;
                    Decimal Tax = 0;
                    Decimal SGST = 0;
                    Decimal CGST = 0;
                    Decimal IGST = 0;
                    Decimal Iamt = 0;
                    String Rdt = "";
                    Decimal Total = 0;
                    objDA.LstDeliveryPrintOrder.ForEach(item =>
                    {
                        html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Importer + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CHA + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.CargoDesc + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Units + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'> " + item.Weight + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.SacNo + "&nbsp;" + item.SacDate + "</td><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Remarks + "</td></tr>";

                    });



                    int Gid = 0;
                    Decimal unt = 0;
                    Decimal Wg = 0;
                    Decimal CUnt = 0;
                    Decimal CWg = 0;
                    decimal TotalUnt = 0;
                    String rem = "", carg = "";
                    objDA.LstDeliveryOrderhdb.ForEach(item =>
                    {
                        Gid = item.GodownId;
                        unt = item.Units;
                        Wg = item.Weight;
                        CUnt = item.ClosingUnits;
                        TotalUnt = unt + CUnt;
                        CWg = item.ClosingWeight;
                        rem = item.Remarks;
                        carg = item.CargoDescription;

                        //Tamt=item.

                    });
                    objDA.LstDeliveryOrderPayment.ForEach(item =>
                    {
                        sdt = item.SacDate;
                        ddt = item.DepositDate;
                        Dno = item.DepositNo;
                        sreq = item.SpaceReq;
                        Arev = item.AreaReserved;
                        Rno = item.ReceiptNo;
                        InsAmt = item.InsAmt;
                        StoAmt = item.StoAmt;
                        Fdt = item.FromDate;
                        ToDate = item.ToDate;
                        days = item.days;
                        wk = item.Weeks;
                        InvInsAmt = Math.Round(item.InvInsAmt, 2);
                        InvStoAmt = Math.Round(item.InvStoAmt, 2);
                        Total = Math.Round((InvInsAmt + InvStoAmt), 2);
                        Tax = (Total * 18) / 100;
                        Rdt = item.ReceiptDate;

                    });
                    objDA.LstGst.ForEach(item =>
                    {
                        SGST = item.SGSTAmt;
                        CGST = item.SGSTAmt;
                        IGST = item.IGSTAmt;
                        Iamt = Math.Round((InvInsAmt + InvStoAmt + SGST + CGST + IGST), 2);

                    });

                    html += "</tbody>";
                    html += "</table></td></tr><tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td><span><br/></span></td></tr><tr> <td width='60%' colspan='6' style='font-size:7pt; font-weight:bold;'>A) At Office</td><td style='font-size:7pt; text-align: right; font-weight:bold;'>Signature of Importer / Agent</td></tr></tbody></table></td></tr><tr><td colspan='12' style='text-align: center;'><h2 style='font-size: 7pt; text-align: center; margin: 0;'><u>PAYMENT DETAILS</u></h2> </td></tr><tr><td colspan='12'><p style='font-size: 7pt; margin: 0 0 5px;'>Date of issue of SAC <u>" + sdt + "</u> for <u>" + sreq + "</u> sq.mt. (net/gross)</p><p style='font-size: 7pt; margin: 0 0 5px;'>Date of Deposit <u>";
                    html += "" + ddt + "</u> D. No <u>" + Dno + "</u></p><p style='font-size: 7pt; margin: 0 0 5px;'>Area Occupied <u>" + Arev + "</u> sq.mt. (net / gross)</p><p style='font-size: 7pt; margin: 0 0 5px;'>St. Charges already collected upto <u>" + objDA.LstDeliveryPrintOrder[0].StorageUptoDate.ToString() + "</u> Rs. <u>" + objDA.LstDeliveryPrintOrder[0].PaidStorage + "</u> Vide CR No <u>" + Rno + "</u></p><p style='font-size: 7pt; margin: 0 0 5px;'>Insurance collected upto<u>" + objDA.LstDeliveryPrintOrder[0].InsuranceUptoDate + "</u> Rs. <u>" + objDA.LstDeliveryPrintOrder[0].Paidinsurance + "</u> Vide CR No <u>" + Rno + "</u></p></td>";
                    html += "</tr><tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>";
                    html += "<tr><td width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td width='5%' valign='top' align='right' style='font-size:7pt;'>1.</td><td colspan='2' width='90%' style='font-size:7pt;'><p style='margin:0; font-size:7pt;'>St. Charges payable for the period</p><p style='margin:0; font-size:7pt;'>from <u>" + objDA.LstDeliveryPrintOrder[0].StorageFromDate + "</u> to <u>" + objDA.LstDeliveryPrintOrder[0].StorageToDate + "</u> (<u>" + objDA.LstDeliveryPrintOrder[0].StorageDays + "</u>days / <u><u>" + objDA.LstDeliveryPrintOrder[0].StorageWeek + "</u></u> weeks) @ <u>" + objDA.LstDeliveryPrintOrder[0].Storagerate + "</u> sq.mtr./week.</p></td></tr></tbody></table></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right'></td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + InvStoAmt + "</u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td width='5%' valign='top' align='right' style='font-size:7pt;'>2.</td><td colspan='2' width='90%' style='font-size:7pt;'><p style='margin:0; font-size:7pt;'>Insurance charges payable for the period</p><p style='margin:0; font-size:7pt;'>from <u>" + objDA.LstDeliveryPrintOrder[0].InsuranceFromDate + "</u> to <u>" + objDA.LstDeliveryPrintOrder[0].InsuranceToDate + "</u> (<u>" + objDA.LstDeliveryPrintOrder[0].InsuranceDays + "</u>days / <u>" + objDA.LstDeliveryPrintOrder[0].InsuranceWeek + "</u> weeks)</p></td></tr></tbody></table></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right'></td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + InvInsAmt + "</u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right' style='font-size:7pt;'>Total :</td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + Total + "</u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right' style='font-size:7pt;'>SGST Amount :</td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + SGST + " </u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right' style='font-size:7pt;'>CGST Amount :</td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + CGST + "</u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><td colspan='6' width='60%' align='right' style='font-size:7pt;'>IGST Amount :</td><td colspan='6' width='40%' align='left'><p style='margin:0; font-size:7pt;'>Rs. <u>" + IGST + "</u> </p></td></tr></tbody></table></td></tr>";
                    html += "<tr><td width='50%'></td> <td colspan='6' width='50%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr><th colspan='6' width='60%' align='right' style='font-size:7pt;'>Grand Total :</th><td colspan='6' width='40%' align='left' style='font-size:7pt; border-top: 2px solid #333; border-bottom: 2px solid #333;'>Rs." + Iamt + "</td></tr></tbody></table></td></tr>";
                    //// Please change the loop as per godown
                    //objDA.LstDeliveryOrder.ForEach(item =>
                    //{
                    //    html += "<tr><td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.GodownId + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.DepositNo+ "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Units + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Weight + "</td>  <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.Duty + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.WRNo + "</td> <td style='text-align:center;border:1px solid #000;border-bottom:1px solid #000;padding:3px;'>" + item.ClosingUnits + "</td></tr>";
                    //});
                    //html += "</tbody></table>";

                    html += "</tbody></table></td></tr><tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr><td><span><br/></span></td></tr><tr> <td width='80%' colspan='10' style='font-size:7pt; font-weight:bold;'><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>OFFICE ASSISTANT</td><td style='font-size:7pt; text-align: right; font-weight:bold;'>CHECKED BY</td></tr></tbody></table></td></tr><tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>";
                    html += "<tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>3.</td><td colspan='2' width='95%' style='font-size:7pt;'>Amount collected vide CR No. <u>" + Rno + "</u> Date <u>" + Rdt + "</u></td></tr></tbody></table></td></tr><tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td width='80%' colspan='10'></td><td style='font-size:7pt; text-align: left; font-weight:bold;'>CASHIER</td></tr></tbody></table></td></tr>";

                    html += "<tr><td colspan='12' style='font-size:7pt; font-weight:bold;'>C) <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> Instructions to Godown Incharge</td></tr><tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody><tr><td width='4%' valign='top' align='right' style='font-size:7pt;'><span>&nbsp;&nbsp;&nbsp;</span> (i)</td><td colspan='2' width='95%' style='font-size:7pt;'>Please deliver <u>" + unt + "</u> units of <u>" + TotalUnt + "</u> weighing <u>" + Wg + "</u> (qtls/MTs) as per Ex-Bond Bill of Entry enclosed, from Godown No <u>" + Gid + "</u>";
                    html += "Stack No <u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u></td></tr><tr><td width='4%' valign='top' align='right' style='font-size:7pt;'>(ii)</td><td colspan='2' width='95%' style='font-size:7pt;'>Entry of Vehicle No <u></u> in the Warehouse premises is permitted.</td></tr></tbody></table></td></tr><tr><td colspan='12' style='font-size:7pt; font-weight:bold;'>Godown I/C Gate I/C</td></tr>";
                    html += "<tr><td><span><br/></span></td></tr><tr><td colspan='12' style='font-size:7pt; font-weight:bold;'>D) <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> (At Godown)</td></tr><tr><td colspan='6' width='50%' style='font-size:7pt;'>The undermentioned cargo has been delivered.</td><th colspan='6' width='50%' style='font-size:7pt;' align='right'>Manager - CFS / Authorised Person</th></tr>";
                    html += "<tr> <td colspan='12'> <table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'> <thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Description of Cargo</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Godown/Stack No.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Delivered / Released</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Balance</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Remarks</th></tr>";
                    html += "<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>No. of Units <br/>(Net/Gross)</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Area <br/>(Net/Gross) </th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>No. of <br/> Units </th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>Area <br/>(Net/Gross) </th></tr></thead> <tbody> <tr>";
                    html += "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + carg + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + Gid + " </td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + unt + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + Wg + "</td>";
                    html += "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + CUnt + "</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + CWg + "</td><td style='border-bottom: 1px solid #000; font-size: 7pt; text-align: center;'>" + rem + "</td></tr></tbody> </table> </td></tr><tr><td><span><br/>";
                    html += "</span></td></tr><tr><th colspan='12' align='right' style='font-size:7pt;' align='right'>GODOWN INCHARGE / ASST.</th></tr><tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; margin: 0;'><tbody><tr><td width='3%' valign='top' align='right' style='font-size:7pt;'>E)</td><td colspan='2' width='95%' style='font-size:7pt;'>Acknowledge recept of above cargo in the same condition as at the time of deposit and alsi concur with the balance area under utilisation.</td></tr>";

                    html += "</tbody></table></td></tr><tr><td><span><br/><br/></span></td></tr><tr><th colspan='6' width='50%' style='font-size:7pt;' align='left'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>Importer / CHA</th><th colspan='6' width='50%' style='font-size:7pt;' align='right'>GODOWN INCHARGE / ASST.</th></tr><tr><td><span><br/></span></td></tr><tr><td colspan='12'><hr style='border-bottom-color:#333;'/></td></tr><tr><td colspan='12' style='font-size:7pt;'>Entered Bond Register Folio No <u></u> </td></tr><tr><td><span><br/></span></td></tr><tr><th colspan='12' align='right' style='font-size:7pt;' align='right'>Office Assistant</th></tr></tbody></table>";



                    html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                    html = html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                    if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                        Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                    if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DeliveryApp" + DeliveryAppId + ".pdf"))
                        System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DeliveryApp" + DeliveryAppId + ".pdf");

                    using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                    {
                        rh.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + "DeliveryApp" + DeliveryAppId + ".pdf", html);
                    }
                    return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/" + "DeliveryApp" + DeliveryAppId + ".pdf" });
                }
                else return Json(new { Status = -1, Message = "Error" });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }


        #endregion

        #region Bond Delivery Payment Sheet

        [HttpGet]
        public ActionResult BondDeliPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetSACNoForDelBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
            {
                var BondList = (IList<BondSacDetails>)objBond.DBResponse.Data;
                TempData["SACList"] = BondList;

                IList<BondSacDetails> DistinctSacList = new List<BondSacDetails>();
                BondList.ToList().ForEach(item =>
                {
                    if (!DistinctSacList.Any(o => o.DepositAppId == item.DepositAppId))
                    {
                        DistinctSacList.Add(item);
                    }
                });
                ViewBag.SACList = JsonConvert.SerializeObject(DistinctSacList);

            }
            else
                ViewBag.SACList = null;

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            //WFLD_ExportRepository objExport = new WFLD_ExportRepository();
            objBond.GetPaymentPartyForPage("", 0);
            if (objBond.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objBond.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objBond.GetPaymentPayerForPage("", 0);
            if (objBond.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objBond.DBResponse.Data);
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
        public JsonResult GetBondDelPaymentSheet(string DeliveryDate, string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, int CasualLabour, int NoOfPrinting, decimal weight, decimal CIFValue, decimal Duty,
            int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate, string ImporterExporter, string CHAName, string ExportUnder, int InvoiceId = 0, int NoOfVahical = 0)
        {

            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetBondDelPaymentSheet(DeliveryDate, InvoiceDate, InvoiceType, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area,
             PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName, CasualLabour, NoOfPrinting, weight, CIFValue, Duty,
             Units, IsInsured, DepositDate, BOENo, BOEDate, ImporterExporter, CHAName, ExportUnder, InvoiceId, NoOfVahical);

            var objBondPostPaymentSheet = (WFLDBondPostPaymentSheet)objBond.DBResponse.Data;
            var objBondList = ((IList<BondSacDetails>)TempData.Peek("SACList")).Where(o => o.DepositAppId == StuffingReqId).ToList();

            var TotalValueofCargo = objBondList.Sum(x => x.CIFValue) + objBondList.Sum(x => x.Duty);
            TempData["TotalValueofCargo"] = TotalValueofCargo;
            TempData.Keep();

            objBondPostPaymentSheet.InvoiceType = InvoiceType;
            objBondPostPaymentSheet.DeliveryDate = DeliveryDate;
            objBondPostPaymentSheet.InvoiceDate = InvoiceDate;
            objBondPostPaymentSheet.RequestId = StuffingReqId;
            objBondPostPaymentSheet.RequestNo = StuffingReqNo;
            objBondPostPaymentSheet.RequestDate = StuffingReqDate;
            objBondPostPaymentSheet.UptoDate = UptoDate;
            objBondPostPaymentSheet.Area = Area;
            objBondPostPaymentSheet.PartyId = PartyId;
            objBondPostPaymentSheet.PartyName = PartyName;
            objBondPostPaymentSheet.PartyAddress = PartyAddress;
            objBondPostPaymentSheet.PartyState = PartyState;
            objBondPostPaymentSheet.PartyStateCode = PartyStateCode;
            //objBondPostPaymentSheet.IsLocalGST = IsLocalGST;
            objBondPostPaymentSheet.PartyGST = PartyGST;
            objBondPostPaymentSheet.PayeeId = PayeeId;
            objBondPostPaymentSheet.PayeeName = PayeeName;
            objBondPostPaymentSheet.DeliveryType = DeliveryType;
            objBondPostPaymentSheet.DepositDate = DepositDate;
            objBondPostPaymentSheet.BOEDate = BOEDate;
            objBondPostPaymentSheet.BOENo = BOENo;
            objBondPostPaymentSheet.ImporterExporter = ImporterExporter;
            objBondPostPaymentSheet.CHAName = CHAName;


            objBondPostPaymentSheet.CIFValue = objBondList.ToList().Sum(o => o.CIFValue);
            objBondPostPaymentSheet.Duty = objBondList.ToList().Sum(o => o.Duty);
            objBondPostPaymentSheet.Units = objBondList.ToList().Sum(o => o.Units);
            objBondPostPaymentSheet.TotalGrossWt = objBondList.ToList().Sum(o => o.Weight);
            objBondPostPaymentSheet.TotalNoOfPackages = objBondList.ToList().Sum(o => o.Units);

            if (objBondList.ToList().Sum(o => o.Units) <= 0)
            {
                objBondPostPaymentSheet.TotalWtPerUnit = 0;
            }
            else
            {
                objBondPostPaymentSheet.TotalWtPerUnit = (objBondList.ToList().Sum(o => o.Weight) / objBondList.ToList().Sum(o => o.Units));
            }

            objBondPostPaymentSheet.TotalAmt = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalDiscount = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Discount), 2);
            objBondPostPaymentSheet.TotalTaxable = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.Amount), 2);
            objBondPostPaymentSheet.TotalCGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.CGSTAmt), 2);
            objBondPostPaymentSheet.TotalSGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.SGSTAmt), 2);
            objBondPostPaymentSheet.TotalIGST = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Sum(o => o.IGSTAmt), 2);
            objBondPostPaymentSheet.CWCTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Total), 2);//tax calculated
            objBondPostPaymentSheet.HTTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total), 2);//tax calculated

            objBondPostPaymentSheet.CWCAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.HTAmtTotal = Math.Round(objBondPostPaymentSheet.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Amount), 2);//without tax total
            objBondPostPaymentSheet.CWCTDS = (objBondPostPaymentSheet.CWCAmtTotal * objBondPostPaymentSheet.CWCTDSPer) / 100;
            objBondPostPaymentSheet.HTTDS = (objBondPostPaymentSheet.HTAmtTotal * objBondPostPaymentSheet.HTTDSPer) / 100;
            objBondPostPaymentSheet.TDS = Math.Round(objBondPostPaymentSheet.CWCTDS + objBondPostPaymentSheet.HTTDS);
            objBondPostPaymentSheet.TDSCol = Math.Round(objBondPostPaymentSheet.TDS);
            objBondPostPaymentSheet.AllTotal = objBondPostPaymentSheet.CWCTotal + objBondPostPaymentSheet.HTTotal + objBondPostPaymentSheet.TDSCol - objBondPostPaymentSheet.TDS;
            objBondPostPaymentSheet.RoundUp = Math.Ceiling(objBondPostPaymentSheet.AllTotal) - objBondPostPaymentSheet.AllTotal;
            objBondPostPaymentSheet.InvoiceAmt = Math.Ceiling(objBondPostPaymentSheet.AllTotal);

            //objChrgRepo.GetBondAdvPaymentSheet(InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, StuffingReqDate, UptoDate, Area, PartyId, PartyName, PartyAddress, PartyState, PartyStateCode, PartyGST, PayeeId, PayeeName,
            //    InvoiceType);

            return Json(objBondPostPaymentSheet, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondDelPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                int BillToParty = objForm["BillToParty"] == "" ? 0 : Convert.ToInt32(objForm["BillToParty"].ToString());
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
                //int VNo = objForm[33] == "" || objForm[33]==null ? 0 : Convert.ToInt32(objForm[33]);
                int VNo = objForm["NoOfVehicle"] == "" || objForm["NoOfVehicle"] == null ? 0 : Convert.ToInt32(objForm["NoOfVehicle"]);
                var invoiceData = JsonConvert.DeserializeObject<WFLDBondPostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
                string ChargesXML = "";

                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                VLDABondRepository objChargeMaster = new VLDABondRepository();
                objChargeMaster.AddEditBondDelInvoice(invoiceData, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "BND", VNo, Convert.ToDecimal(TempData.Peek("TotalValueofCargo")), BillToParty);
                //objChargeMaster.AddEditInvoice(invoiceData, "", ChargesXML, "", "", BranchId, ((Login)(Session["LoginUser"])).Uid, "BND");

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

        public JsonResult GetBondRemainingDetails(int DepositAppId)
        {
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetSACNoForUnlodBondPaymentSheetRemaining(DepositAppId);
            var UnloadDtls = ((WFLDBondRemaining)objBond.DBResponse.Data);
            return Json(UnloadDtls, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Bond Unloading Payment Sheet

        [HttpGet]
        public ActionResult BondUnloadPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetSACNoForUnlodBondPaymentSheet();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            /*ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;*/
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDetailsBondUnloading(int DepositAppId)
        {
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetSACNoForUnlodBondPaymentSheet();
            var UnloadDtls = ((List<HdbBondSacDetails>)objBond.DBResponse.Data).Where(x => x.DepositAppId == DepositAppId).ToList();
            return Json(UnloadDtls, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetBondUnloadPaymentSheet(string DeliveryDate, string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal weight, decimal CIFValue, decimal Duty,
            int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate, int UnloadingId, int CasualLabour, string validUpTo, int InvoiceId = 0, int NoOfVahical = 0, string ExportUnder = "")
        {
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetBondUnloadingPaymentSheet(DeliveryDate, InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, UptoDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, CasualLabour, InvoiceType, Area, UnloadingId, validUpTo, InvoiceId, NoOfVahical, ExportUnder);

            var objBondPostPaymentSheet = new WFLD_PostPaymentSheet();
            var Output = (WFLD_PostPaymentSheet)objBond.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.DeliveryDate = DeliveryDate;
            Output.Module = "BND";
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
                    Output.lstPostPaymentCont.Add(new WFLD_PostPaymentContainer
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



            // return Json(Output, JsonRequestBehavior.AllowGet);
            /**********BOL PRINT**************/

            /********************************/
            // return Json(new { Output });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondUnloadPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                int Vno = objForm["NoOfVehicle"] == "0" ? 0 : Convert.ToInt32(objForm["NoOfVehicle"].ToString());
                int BillToParty = objForm["BillToParty"] == "" ? 0 : Convert.ToInt32(objForm["BillToParty"].ToString());
                var invoiceData = JsonConvert.DeserializeObject<WFLD_LoadedContainerPayment>(objForm["PaymentSheetModelJson"].ToString());
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

                VLDABondRepository objChargeMaster = new VLDABondRepository();
                objChargeMaster.AddEditBondUnloadingInvoice(invoiceData, Vno, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDUnloading", BillToParty);

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

        #region Bond Advance Payment Sheet

        [HttpGet]
        public ActionResult BondAdvancePaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            VLDABondRepository objBond = new VLDABondRepository();
            //objBond.GetSACNoForUnlodBondPaymentSheet();
            objBond.GetSACForBondAdvancePayment();
            if (objBond.DBResponse.Status > 0)
                ViewBag.SACList = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.SACList = null;

            objBond.GetPaymentPayer();
            if (objBond.DBResponse.Status > 0)
                ViewBag.PaymentPayer = JsonConvert.SerializeObject(objBond.DBResponse.Data);
            else
                ViewBag.PaymentPayer = null;


            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetBondAdvancePaymentSheetOptional(int DepositAppId, string InvoiceDate, string InvoiceType, int StuffingReqId, int DeliveryType, string StuffingReqNo, string StuffingReqDate, string UptoDate, decimal Area,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId, string PayeeName, decimal weight, decimal CIFValue, decimal Duty,
            int Units, int IsInsured, string DepositDate, string BOENo, string BOEDate, int CasualLabour, string validUpTo, int InvoiceId = 0, int NoOfVahical = 0, string ExportUnder = "", int UnloadingId = 0)
        {
            VLDABondRepository objBond = new VLDABondRepository();
            objBond.GetBondAdvancePaymentSheetOptional(DepositAppId, InvoiceDate, StuffingReqId, DeliveryType, StuffingReqNo, UptoDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, CasualLabour, InvoiceType, Area, UnloadingId, validUpTo, InvoiceId, NoOfVahical, ExportUnder, IsInsured);

            var objBondPostPaymentSheet = new WFLD_PostPaymentSheet();
            var Output = (WFLD_PostPaymentSheet)objBond.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "BND";
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
                    Output.lstPostPaymentCont.Add(new WFLD_PostPaymentContainer
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



            // return Json(Output, JsonRequestBehavior.AllowGet);
            /**********BOL PRINT**************/

            /********************************/
            // return Json(new { Output });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditBondAdvancePaymentSheetOptional(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                int BillToParty = objForm["BillToParty"] == "" ? 0 : Convert.ToInt32(objForm["BillToParty"].ToString());
                var invoiceData = JsonConvert.DeserializeObject<WFLD_LoadedContainerPayment>(objForm["PaymentSheetModelJson"].ToString());
                invoiceData.ExportUnder = Convert.ToString(objForm["ExportUnder"]);
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

                VLDABondRepository objChargeMaster = new VLDABondRepository();
                objChargeMaster.AddEditAdvanceBondInvoiceOptional(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, CfsWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "BNDAdvance", BillToParty);

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

        #region Bond Amendment
        [HttpGet]
        public ActionResult BondAmendment(int SpaceappId = 0)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WfldBondAmendmentViewNodel objBA = new WfldBondAmendmentViewNodel();
            if (SpaceappId > 0)
            {
                objBR.SACAmendDetails(SpaceappId);
                if (objBR.DBResponse.Data != null)
                    objBA = (WfldBondAmendmentViewNodel)objBR.DBResponse.Data;
            }
            objBA.AmendDate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView(objBA);
        }
        [HttpGet]
        public JsonResult ListOfSACForAmend()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            objBR.ListOfSACForAmend();
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BondAmendment(WfldBondAmendmentViewNodel objVM)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            string XML = (objVM.lstBondAmendDtl.Count() == 0 ? "" : Utility.CreateXML(objVM.lstBondAmendDtl));
            objBR.AddEditBondAmendment(objVM, XML);
            return Json(objBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfBondAmendment()
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldBondAmendmentList> lstApp = new List<WfldBondAmendmentList>();
            objBR.ListOfBondAmendment(0, "");
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WfldBondAmendmentList>)objBR.DBResponse.Data;
            return PartialView(lstApp);
        }
        [HttpGet]
        public JsonResult LoadMoreListOfBondAmendment(int Page)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldBondAmendmentList> lstApp = new List<WfldBondAmendmentList>();
            objBR.ListOfBondAmendment(Page, "");
            if (objBR.DBResponse.Data != null)
                lstApp = (List<WfldBondAmendmentList>)objBR.DBResponse.Data;
            return Json(lstApp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchOfBondAmendment(string Search)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            IList<WfldBondAmendmentList> lstApp = new List<WfldBondAmendmentList>();
            objBR.ListOfBondAmendment(-1, Search);
            if (objBR.DBResponse.Data != null)
                lstApp = ((List<WfldBondAmendmentList>)objBR.DBResponse.Data);
            return PartialView("ListOfBondAmendment", lstApp);
        }
        [HttpGet]
        public ActionResult ViewBondAmendment(string AmendNo)
        {
            VLDABondRepository objBR = new VLDABondRepository();
            WfldBondAmendmentViewNodel objApp = new WfldBondAmendmentViewNodel();
            objBR.ViewBondAmendment(AmendNo);
            if (objBR.DBResponse.Data != null)
                objApp = ((WfldBondAmendmentViewNodel)objBR.DBResponse.Data);
            return PartialView(objApp);
        }
        #endregion
    }
}