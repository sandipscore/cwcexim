using CwcExim.Areas.Icegate.Models;
using CwcExim.Areas.Import.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using CwcExim.Areas.Report.Models;
using System.Globalization;

namespace CwcExim.Areas.Icegate.Controllers
{
    public class Wfld_IcegateController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public Wfld_IcegateController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
        }
        #region Resend IceGate Data
        public ActionResult ICEGateResendData()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResendIceGateData(String ContainerNo)
        {
            Wfld_ImportRepository objImport = new Wfld_ImportRepository();
            objImport.ResendIceGateData(ContainerNo);
            object data = null;
            if (objImport.DBResponse.Data != null)
                data = objImport.DBResponse;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ice gate report

        #region Import
        [HttpGet]
        public ActionResult IceGateImportReport(string Mode = "S")
        {
            ViewBag.Mode = Mode;
            return PartialView();
        }
        [HttpGet]
        public JsonResult Eximtraderlist(int Page, string PartyCode, int Exporter = 0, int Importer = 0, int ShippingLine = 0)
        {
            var objER = new Wfld_IceGateRepository();
            objER.EximtraderlistPopulation(Page, PartyCode, Exporter, Importer, ShippingLine);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContainerList(char Module)
        {
            var objER = new Wfld_IceGateRepository();
            objER.GetContainerList(Module);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult ImportI02Report(string FromDate, string ToDate, string Mode, string CFSCode = "", string OBL = "", int ShippingId = 0, int ImpExpId = 0)
        {
            var objER = new Wfld_IceGateRepository();
            objER.IcegateI02Report(FromDate, ToDate, Mode, CFSCode, OBL, ShippingId, ImpExpId);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ResendI02(string FileName, string FilePath, string ContainerNo, string CFSCode, string FileCode)
        {
            try
            {
                string Filenm = FileName;
                string strFolderName = FilePath.Replace("'", "").ToString();
                string Sts = "";
                FileName = strFolderName + FileName;
                using (FileStream fs = System.IO.File.OpenRead(FileName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    Sts = FtpFileManager.UploadFileToFtp("/live/inbound", Filenm, buffer, "5000", FileName);
                }
                if (Sts == "Success")
                {
                    var objER = new Wfld_IceGateRepository();
                    objER.SaveResentRecord(Filenm, FileCode, ContainerNo, CFSCode, "Import");
                }
                return Json(new { Status = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0 });
            }
        }
        [HttpPost]
        public JsonResult GenerateI02File(List<IcegateI02Model> LstCont, List<IcegateI02ModelAck> LstContAck, string FromDate, string ToDate, string Mode)
        {
            try
            {
                var FileName = ("ImportI02Report" + DateTime.Now.ToLongTimeString() + ".pdf").Replace(":", "").ToString();
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID;

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><span style='font-size:12px;'><b>" + (Mode == "S" ? "Sending" : "Receiving") + " Date " + FromDate + " To " + ToDate + "</b></span><br/><label style='font-size: 14px; font-weight:bold;'>IMPORT I02 REPORT</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:7pt;'><thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000; width:1%;'>#</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Date</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Container No.</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>OBL No.</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>Shipping Line</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:25%;'>Importer</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Last Message on</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;'>File Name</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>ICE Gate Status</th><th style='border-bottom:1px solid #000;width:5%;'>Remarks</th></tr></thead><tbody>");

                LstCont.Select(x =>new { CFSCode= x.CFSCode,FileCode=x.FileCode }).Distinct().ToList().ForEach(elem =>
                {
                    string OBL = "", Imp = "", SLA = "", FileCode = "";
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.OBLNo != "").Select(x => x.OBLNo).Distinct().ToList().ForEach(item =>
                      {
                          OBL = (OBL == "" ? item : OBL + " , " + item);
                      });
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.Importer != "").Select(x => x.Importer).Distinct().ToList().ForEach(item =>
                    {
                        Imp = (Imp == "" ? item : Imp + " , " + item);
                    });
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.SLA != "").Select(x => x.SLA).Distinct().ToList().ForEach(item =>
                    {
                        SLA = (SLA == "" ? item : SLA + " , " + item);
                    });
                    //FileCode = LstCont.Where(x => x.CFSCode == elem.CFSCode).Select(x => x.FileCode).FirstOrDefault();
                    FileCode = elem.FileCode;

                    Pages.Append("<tr>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode==elem.FileCode).FirstOrDefault().DateofMsg + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().ContainerNo + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + OBL + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + SLA + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + Imp + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().SendOn + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().FileName + "</td>");
                    Pages.Append("<td valign='top' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().AckStatus + "</td>");
                    var ErrCd = "";
                    LstContAck.Where(x => x.FileCode == FileCode && x.ErrorCode != "00").Select(x=>x.ErrorCode).Distinct().ToList().ForEach(item =>
                      {
                          ErrCd = ErrCd + "" + item.ToString();
                      });

                    Pages.Append("<td style='border-bottom:1px solid #000;'>" + (LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().AckStatus == "Successful" ? "" : ErrCd) + " </td>");
                    Pages.Append("</tr>");

                    if (LstContAck.Where(x => x.FileCode == FileCode).ToList().Count > 0)
                    {
                        Pages.Append("<tr><th colspan='2' cellpadding='5' style='border-right:1px solid #000;'></th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>Ack Date</th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>File Name</th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>Error Code</th>");
                        Pages.Append("<th colspan='2' cellpadding='5'></th></tr>");

                        LstContAck.Where(x => x.FileCode == FileCode).ToList().ForEach(item =>
                        {
                            Pages.Append("<tr><td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'></td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.AckRecvDate + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.FileName + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ErrorCode + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;'></td></tr>");
                        });
                    }

                    i++;
                });

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                /*if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }*/
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + FileName });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "" });
            }
        }
        [HttpPost]
        public JsonResult GenerateI02ExcelFile(List<IcegateI02Model> LstCont, List<IcegateI02ModelAck> LstContAck, string FromDate, string ToDate, string Mode)
        {
            try
            {
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
                var dt = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/" + Session.SessionID + "/" + dt + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    var title = @"Import I02 Report (" + (((Mode == "S") ? "Sending Date " : "Receiving Date ") + FromDate + " To " + ToDate + ")");
                    exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("A2:A4", "#", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("B2:B4", "Date", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("C2:C4", "Container No.", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("D2:D4", "OBL No.", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("E2:E4", "Shipping Line", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("F2:F4", "Importer", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("G2:G4", "Last Message On", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("H2:H4", "File Name", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("I2:I4", "Ice Gate Status", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("J2:J4", "Remarks", DynamicExcel.CellAlignment.Middle);

                    exl.MargeCell("K2:K4", "Ack Rcvd Date", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("L2:L4", "Ack File Name", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("M2:M4", "Error Code", DynamicExcel.CellAlignment.Middle);

                    for (var i = 65; i < 78; i++)
                    {
                        char character = (char)i;
                        string text = character.ToString();
                        exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                    }
                    /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                    exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                    int j = 0; int loc = 6;
                    LstCont.Select(x =>new { CFSCode = x.CFSCode, FileCode = x.FileCode }).Distinct().ToList().ForEach(item =>
                    {
                        var model = new IcegateI02ExcelModel();
                        model.SlNo = ++j;
                        model.Date = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode==item.FileCode).FirstOrDefault().DateofMsg;
                        model.ContainerNo = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().ContainerNo;
                        model.OBLNo = "";
                        LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode && m.OBLNo != "").Select(m => m.OBLNo)
                        .Distinct().ToList().ForEach(elem =>
                        {
                            model.OBLNo = (model.OBLNo == "" ? elem : (model.OBLNo + " , " + elem));
                        });

                        model.SLA = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().SLA;
                        model.Importer = "";
                        LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode && m.Importer != "").Select(m => m.Importer).Distinct().ToList().ForEach(elem =>
                        {
                            model.Importer = (model.Importer == "" ? elem : (model.Importer + " , " + elem));
                        });

                        model.Remarks = "";
                        LstContAck.Where(m => m.FileCode == item.FileCode).Select(m => m.ErrorCode)
                        .Distinct().ToList().ForEach(elem =>
                        {
                            model.Remarks = (model.Remarks == "" ? elem : (model.Remarks.ToString() + " , " + elem));
                        });

                        model.SendOn = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().SendOn;
                        model.FileName = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().FileName;
                        model.AckStatus = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().AckStatus;

                        model.Remarks = (model.AckStatus == "Successful" ? "" : model.Remarks);

                        var lstModel = new List<IcegateI02ExcelModel>();
                        lstModel.Add(model);

                        exl.AddTable<IcegateI02ExcelModel>("A", loc, lstModel, new[] { 5, 10, 15, 30, 30, 40, 10, 20, 15, 15, 15 });

                        var modelAck = new IcegateI02ExcelAck();
                        modelAck.AckRecvDate = "";
                        modelAck.FileName = "";
                        modelAck.ErrorCode = "";
                        LstContAck.Where(x => x.FileCode == item.FileCode).ToList().ForEach(data =>
                        {
                            modelAck.AckRecvDate = (modelAck.AckRecvDate == "" ? data.AckRecvDate : (modelAck.AckRecvDate + "\r\n" + data.AckRecvDate));
                            modelAck.FileName = (modelAck.FileName == "" ? data.FileName : (modelAck.FileName + "\r\n" + data.FileName));
                            modelAck.ErrorCode = (modelAck.ErrorCode == "" ? " " + data.ErrorCode.ToString() : (modelAck.ErrorCode + "\r\n" + data.ErrorCode));
                        });

                        var lstModelAck = new List<IcegateI02ExcelAck>();
                        lstModelAck.Add(modelAck);

                        exl.AddTable<IcegateI02ExcelAck>("K", loc, lstModelAck, new[] { 15, 48, 10 });
                        loc++;
                    });


                    exl.Save();
                }
                return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + dt + ".xlsx" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "" });
            }
        }
        #endregion

        #region Export
        [HttpGet]
        public ActionResult IceGateExportReport(string Mode = "S")
        {
            ViewBag.Mode = Mode;
            return PartialView();
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult ExportE07Report(string FromDate, string ToDate, string Mode, string CFSCode = "", string OBL = "", int ShippingId = 0, int ImpExpId = 0)
        {
            var objER = new Wfld_IceGateRepository();
            objER.IcegateE07Report(FromDate, ToDate, Mode, CFSCode, OBL, ShippingId, ImpExpId);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GenerateE07File(List<IcegateE07Model> LstCont, List<IcegateE07ModelAck> LstContAck, string FromDate, string ToDate, string Mode)
        {
            try
            {
                var FileName = ("ExportE07Report" + DateTime.Now.ToLongTimeString() + ".pdf").Replace(":", "").ToString();
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID;

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/><span style='font-size:12px;'><b>" + (Mode == "S" ? "Sending" : "Receiving") + " Date " + FromDate + " To " + ToDate + "</b></span><br/><label style='font-size: 14px; font-weight:bold;'>EXPORT E07 REPORT</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:7pt;'><thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000; width:1%;'>#</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>First Message On</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>Container No.</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>SB No.</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>Shipping Line</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:17%;'>Exporter</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Last Message On</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>File Name</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>ICE Gate Status</th><th style='border-bottom:1px solid #000;width:7%;'>Remarks</th></tr></thead><tbody>");

                LstCont.Select(x =>new { CFSCode= x.CFSCode,FileCode=x.FileCode }).Distinct().ToList().ForEach(elem =>
                {
                    string OBL = "", Imp = "", SLA = "", FileCode = "";
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode==elem.FileCode && x.OBLNo != "").Select(x => x.OBLNo).Distinct().ToList().ForEach(item =>
                    {
                        OBL = (OBL == "" ? item : OBL + " , " + item);
                    });
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode && x.Importer != "").Select(x => x.Importer).Distinct().ToList().ForEach(item =>
                    {
                        Imp = (Imp == "" ? item : Imp + " , " + item);
                    });
                    LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode && x.SLA != "").Select(x => x.SLA).Distinct().ToList().ForEach(item =>
                    {
                        SLA = (SLA == "" ? item : SLA + " , " + item);
                    });
                    FileCode = elem.FileCode;

                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().DateofMsg + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().ContainerNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + OBL + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + SLA + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + Imp + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().SendOn + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().FileName + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().AckStatus + "</td>");
                    var ErrCd = "";
                    LstContAck.Where(x => x.FileCode == FileCode && x.ErrorCode != "S").Select(x=>x.ErrorCode).Distinct().ToList().ForEach(item =>
                    {
                        ErrCd = ErrCd + "" + item;
                    });

                    Pages.Append("<td style='border-bottom:1px solid #000;'>" + (LstCont.Where(x => x.CFSCode == elem.CFSCode && x.FileCode == elem.FileCode).FirstOrDefault().AckStatus == "Successful" ? "" : ErrCd) + " </td>");
                    Pages.Append("</tr>");

                    if (LstContAck.Where(x => x.FileCode == FileCode).ToList().Count > 0)
                    {
                        Pages.Append("<tr><th colspan='2' cellpadding='5' style='border-right:1px solid #000;'></th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>Ack Date</th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>File Name</th>");
                        Pages.Append("<th colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>Error Code</th>");
                        Pages.Append("<th colspan='2' cellpadding='5'></th></tr>");

                        LstContAck.Where(x => x.FileCode == FileCode).ToList().ForEach(item =>
                        {
                            Pages.Append("<tr><td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'></td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.AckRecvDate + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.FileName + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ErrorCode + "</td>");
                            Pages.Append("<td colspan='2' cellpadding='5' style='border-bottom:1px solid #000;'></td></tr>");
                        });
                    }

                    i++;
                });

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                /*if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }*/
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 20f, 20f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + FileName });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "" });
            }
        }
        [HttpPost]
        public JsonResult GenerateE07ExcelFile(List<IcegateE07Model> LstCont, List<IcegateE07ModelAck> LstContAck, string FromDate, string ToDate, string Mode)
        {
            try
            {
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
                var dt = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/" + Session.SessionID + "/" + dt + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    var title = @"Export E07 Report (" + (((Mode == "S") ? "Sending Date " : "Receiving Date ") + FromDate + " To " + ToDate + ")");
                    exl.MargeCell("A1:M1", title, DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("A2:A4", "#", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("B2:B4", "First Message On", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("C2:C4", "Container No.", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("D2:D4", "SB No.", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("E2:E4", "Shipping Line", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("F2:F4", "Exporter", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("G2:G4", "Last Message On", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("H2:H4", "File Name", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("I2:I4", "Ice Gate Status", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("J2:J4", "Remarks", DynamicExcel.CellAlignment.Middle);

                    exl.MargeCell("K2:K4", "Ack Rcvd Date", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("L2:L4", "Ack File Name", DynamicExcel.CellAlignment.Middle);
                    exl.MargeCell("M2:M4", "Error Code", DynamicExcel.CellAlignment.Middle);

                    for (var i = 65; i < 78; i++)
                    {
                        char character = (char)i;
                        string text = character.ToString();
                        exl.AddCell(text + "5", (i - 64), DynamicExcel.CellAlignment.Middle);
                    }
                    /*exl.AddTable<InvoiceData>("A", 6, model.InvoiceData, new[] { 6, 20, 20, 40, 12, 16, 10, 10, 12, 12, 8, 14, 8, 14, 8, 14, 16 });
                    exl.AddTable<CashReceiptData>("R", 6, model.CashReceiptData, new[] { 12, 30, 14, 14, 14, 14, 14, 40 });*/
                    int j = 0; int loc = 6;
                    LstCont.Select(x =>new { CFSCode=x.CFSCode,FileCode=x.FileCode }).Distinct().ToList().ForEach(item =>
                    {
                        var model = new IcegateE07ExcelModel();
                        model.SlNo = ++j;
                        var DateofMsg = (LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode==item.FileCode).FirstOrDefault().DateofMsg.Replace('/', '.')).ToString();
                        model.Date = DateofMsg;
                        model.ContainerNo = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().ContainerNo;
                        model.OBLNo = "";
                        LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode && m.OBLNo != "").Select(m => m.OBLNo).Distinct().ToList().ForEach(elem =>
                        {
                            model.OBLNo = (model.OBLNo == "" ? elem : (model.OBLNo + " , " + elem));
                        });

                        model.SLA = LstCont.Where(m => m.CFSCode == item.CFSCode).FirstOrDefault().SLA;
                        model.Importer = "";
                        LstCont.Where(m => m.CFSCode == item.CFSCode && m.Importer != "").Select(m => m.Importer).Distinct().ToList().ForEach(elem =>
                            {
                                model.Importer = (model.Importer == "" ? elem : (model.Importer + " , " + elem));
                            });

                        model.Remarks = "";
                        LstContAck.Where(m => m.FileCode == item.FileCode).Select(m => m.ErrorCode)
                        .Distinct().ToList().ForEach(elem =>
                        {
                            model.Remarks = (model.Remarks == "" ? elem : (model.Remarks + " , " + elem));
                        });
                        DateofMsg = (LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode==item.FileCode).FirstOrDefault().SendOn.Replace('/', '.')).ToString();
                        model.SendOn = DateofMsg;
                        model.FileName = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().FileName;
                        model.AckStatus = LstCont.Where(m => m.CFSCode == item.CFSCode && m.FileCode == item.FileCode).FirstOrDefault().AckStatus;

                        model.Remarks = (model.AckStatus == "Successful" ? "" : model.Remarks);

                        var lstModel = new List<IcegateE07ExcelModel>();
                        lstModel.Add(model);

                        exl.AddTable<IcegateE07ExcelModel>("A", loc, lstModel, new[] { 5, 10, 15, 30, 30, 40, 10, 20, 15, 15, 15 });

                        var modelAck = new IcegateE07ExcelAck();
                        modelAck.AckRecvDate = "";
                        modelAck.FileName = "";
                        modelAck.ErrorCode = "";
                        LstContAck.Where(x => x.FileCode == item.FileCode).ToList().ForEach(data =>
                        {
                            modelAck.AckRecvDate = (modelAck.AckRecvDate == "" ? data.AckRecvDate : (modelAck.AckRecvDate + "\r\n" + data.AckRecvDate));
                            modelAck.FileName = (modelAck.FileName == "" ? data.FileName : (modelAck.FileName + "\r\n" + data.FileName));
                            modelAck.ErrorCode = (modelAck.ErrorCode == "" ? data.ErrorCode : (modelAck.ErrorCode + "\r\n" + data.ErrorCode));
                        });

                        var lstModelAck = new List<IcegateE07ExcelAck>();
                        lstModelAck.Add(modelAck);

                        exl.AddTable<IcegateE07ExcelAck>("K", loc, lstModelAck, new[] { 15, 48, 10 });
                        loc++;
                    });


                    exl.Save();
                }
                return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + dt + ".xlsx" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "" });
            }
        }
        [HttpPost]
        public JsonResult ResendE07(string FileName, string FilePath, string ContainerNo, string CFSCode, string FileCode)
        {
            try
            {
                string Filenm = FileName;
                string strFolderName = FilePath.Replace("'", "").ToString();
                string Sts = "";
                FileName = strFolderName + FileName;
                using (FileStream fs = System.IO.File.OpenRead(FileName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    Sts = FtpFileManager.UploadFileToFtp("/live/inbound", Filenm, buffer, "5000", FileName);
                }
                if (Sts == "Success")
                {
                    var objER = new Wfld_IceGateRepository();
                    objER.SaveResentRecord(Filenm, FileCode, ContainerNo, CFSCode, "Export");
                }
                return Json(new { Status = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0 });
            }
        }
        #endregion

        #endregion

        #region Download File
        [HttpPost]
        public JsonResult DownloadFile(string filePath, string FileName)
        {
            try
            {
                filePath = filePath.Replace("'", "").ToString();
                filePath = filePath + FileName;
                FileInfo fi = new FileInfo(filePath);
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
                fi.CopyTo(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName, true);
                return Json(new { Status = 1, Data = "/Docs/" + Session.SessionID + "/" + FileName });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "" });
            }
        }
        #endregion

        #region Manual I02
        [HttpGet]
        public ActionResult ManualI02FileCreate()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetContainerListForI02()
        {
            var objER = new Wfld_IceGateRepository();
            objER.GetContainerListForI02();
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContainerDtlForI02(string CFSCode)
        {
            var objER = new Wfld_IceGateRepository();
            objER.GetContainerDtlForI02(CFSCode);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult ManualI02FileCreate(IList<IcegateManualI02> lstData)
        {
            try
            {
                log.Info("Icegate Controller.Method called is ManualI02FileCreate.Begin");
                var objER = new Wfld_IceGateRepository();
                int FileCode = objER.GetFileCodeForI02();
                if (FileCode > 0)
                {
                    lstData = I02FileCreate(lstData, FileCode);
                    if (lstData != null)
                        objER.AddManualI02Dtl(Utility.CreateXML(lstData).ToString());
                    log.Info("Icegate Controller.Method called is ManualI02FileCreate.End");
                    return Json(objER.DBResponse);
                }
                else
                {
                    log.Info("Icegate Controller.Method called is ManualI02FileCreate.End");
                    return Json(new { Status = 0, Message = "Error" });
                }
            }        
            catch (Exception ex)
            {
                log.Info("Stack trace : " + ex.StackTrace + " Message : " + ex.Message);
                return Json(new { Status = 0, Message = "Error" });
            }

        }
        [NonAction]
        private IList<IcegateManualI02> I02FileCreate(IList<IcegateManualI02> lstData, int FileCode)
        {
            try
            {
                log.Info("I02 File creation Begin");
                string FileName = "";
                int idd = FileCode;

                DateTime dtt = DateTime.Now.Date;
                int YearValue = dtt.Year;
                int MonthValue = dtt.Month;
                int DayValue = dtt.Day;
                String dttvalue = YearValue.ToString() + (MonthValue.ToString().Length == 1 ? ("0" + MonthValue.ToString()) : MonthValue.ToString()) + (DayValue.ToString().Length == 1 ? ("0" + DayValue.ToString()) : DayValue.ToString());
                TimeSpan tt = DateTime.Now.TimeOfDay;
                int hourtmm = tt.Hours;
                int mintmm = tt.Minutes;
                String TimeVall = (hourtmm.ToString().Length == 1 ? ("0" + hourtmm.ToString()) : hourtmm.ToString()) + (mintmm.ToString().Length == 1 ? ("0" + mintmm.ToString()) : mintmm.ToString());
                String Filenm = "COCHI02_" + dttvalue + "_" + TimeVall + ".in";

                lstData.ToList().ForEach(m =>
                {
                    m.FileName = Filenm;
                    m.FileCode = FileCode;
                    m.BondNo = (!string.IsNullOrEmpty(m.BondNo) ? "" : m.BondNo);
                    m.ISOCode = (!string.IsNullOrEmpty(m.ISOCode) ? "" : m.ISOCode);
                    m.Train_VehicleDate = DateTime.ParseExact(m.Train_VehicleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    m.SMTPDate = DateTime.ParseExact(m.SMTPDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                });

                string strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_MsgFileContArr"];
                FileName = strFolderName + "COCHI02_" + dttvalue + "_" + TimeVall + ".in";
                if (!Directory.Exists(strFolderName))
                {
                    Directory.CreateDirectory(strFolderName);
                }

                char k1 = (char)29;
                System.IO.File.Create(FileName).Dispose();

                TextWriter tw = new StreamWriter(FileName);
                tw.Write("HREC");
                tw.Write(k1);
                tw.Write("ZZ");
                tw.Write(k1);
                tw.Write("INWFD6CWC1");//Receiver Id
                tw.Write(k1);
                tw.Write("ZZ");
                tw.Write(k1);
                tw.Write("INWFD6");//Sender Id
                tw.Write(k1);
                tw.Write("ICES1_5");//Version Number
                tw.Write(k1);
                tw.Write("P");//Production
                tw.Write(k1);
                tw.Write(k1);
                tw.Write("COCHI02");//MessageId
                tw.Write(k1);
                tw.Write(idd);
                tw.Write(k1);
                tw.Write(dttvalue);
                tw.Write(k1);
                tw.WriteLine(TimeVall);
                tw.WriteLine("<contarr>");
                lstData.ToList().ForEach(row =>
                {

                    if (!string.IsNullOrEmpty(row.SMTPNo.ToString()) &&
                        !string.IsNullOrEmpty(row.SMTPDate.ToString()) &&
                        !string.IsNullOrEmpty(row.Train_VehicleNo.ToString()) &&
                        !string.IsNullOrEmpty(row.Train_VehicleDate.ToString()) &&
                        !string.IsNullOrEmpty(row.ContainerNo.ToString()) &&
                        !string.IsNullOrEmpty(row.ShippingLineCode.ToString()) &&
                        !string.IsNullOrEmpty(row.GatewayPortCode.ToString()) &&
                        !string.IsNullOrEmpty(row.ContainerStatus.ToString()))
                    {
                        string tmode;
                        row.MessageType = "F";

                        tmode = row.ModeOfTransport;
                        row.CustomHouseCode = "INWFD6";

                        DateTime SMTPDate = DateTime.ParseExact(row.SMTPDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        String SMTPDateValue = SMTPDate.ToString("ddMMyyyy");

                        DateTime Train_VehicleDate = DateTime.ParseExact(row.Train_VehicleDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        String VehicleDateValue = Train_VehicleDate.ToString("ddMMyyyy");

                        tw.Write("F");
                        tw.Write(k1);
                        tw.Write(row.ModeOfTransport);
                        tw.Write(k1);
                        tw.Write(row.CustomHouseCode);
                        tw.Write(k1);
                        tw.Write(row.SMTPNo);
                        tw.Write(k1);
                        tw.Write(SMTPDateValue);
                        tw.Write(k1);
                        tw.Write(row.Train_VehicleNo);
                        tw.Write(k1);
                        tw.Write(VehicleDateValue);
                        tw.Write(k1);
                        tw.Write(row.WagonNo);
                        tw.Write(k1);
                        tw.Write(row.ContainerNo);
                        tw.Write(k1);
                        tw.Write(row.ShippingLineCode);
                        tw.Write(k1);
                        //tw.Write(row.Weight);
                        tw.Write(k1);
                        tw.Write(row.OriginPortCode);
                        //if (tmode == "T")
                        //{
                        //    tw.Write(row.PortCodeOrigin == "" ? "INNSA1" : row.PortCodeOrigin);//ContArrObj.PortCodeOrigin
                        //}
                        //else
                        //{
                        //    tw.Write(row.PortCodeOrigin == "" ? "NHV" : row.PortCodeOrigin);//ContArrObj.PortCodeOrigin
                        //}
                        tw.Write(k1);
                        //tw.Write("INLON6");
                        tw.Write(k1);
                        tw.Write(row.GatewayPortCode);
                        tw.Write(k1);
                        tw.Write(k1);
                        tw.Write(k1);
                        tw.Write(row.CarrierAgencyCode);
                        tw.Write(k1);
                        tw.WriteLine("I");
                    }
                });
                tw.WriteLine("<END-contarr>");
                tw.Write("TREC");
                tw.Write(k1);
                tw.Write(idd);
                tw.Write(Environment.NewLine);
                tw.Close();

                log.Info("I02 File creation end");
                log.Info("I02 File name "+ FileName);


                log.Info("FTP File upload has began");
                //File upload
                string output = "";
                using (FileStream fs = System.IO.File.OpenRead(FileName))
                {
                    log.Info("FTP File read process has began");
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    output = FtpFileManager.UploadFileToFtp("/live/inbound", Filenm, buffer, "5000", FileName);
                    log.Info("FTP File read process has ended");
                }
                log.Info("FTP File upload has been end");
                if (output == "Success")
                    return lstData;
                else return null;
            }
            catch (Exception ex)
            {
                log.Info("Stack trace : " + ex.StackTrace + " Message : " + ex.Message);
                return null;
            }            
        }
        [HttpGet]
        public ActionResult ListManualI02()
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new List<IcegateManualI02>();
            objER.ListManualI02(0, 0);
            if (objER.DBResponse.Data != null)
                objModel =(List<IcegateManualI02>)objER.DBResponse.Data;
            return PartialView(objModel);
        }
        [HttpGet]
        public JsonResult LoadMoreListManualI02(int Page,int id=0)
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new List<IcegateManualI02>();
            objER.ListManualI02(id, Page);
            if (objER.DBResponse.Data != null)
                objModel = (List<IcegateManualI02>)objER.DBResponse.Data;
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewManualI02FileCreate(int id)
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new IcegateManualI02ViewModel();
            objER.ViewManualI02FileCreate(id);
            if (objER.DBResponse.Data != null)
                objModel = (IcegateManualI02ViewModel)objER.DBResponse.Data;
            return PartialView(objModel);
        }
        #endregion

        #region Manual E07
        [HttpGet]
        public ActionResult ManualE07FileCreate()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetContainerListForE07()
        {
            var objER = new Wfld_IceGateRepository();
            objER.GetContainerListForE07();
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContainerDtlForE07(string CFSCode)
        {
            var objER = new Wfld_IceGateRepository();
            objER.GetContainerDtlForE07(CFSCode);
            return Json(objER.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, CustomValidateAntiForgeryToken]
        public JsonResult ManualE07ileCreate(IList<IcegateManualE07> lstData)
        {
            try
            {
                log.Info("E07 File creation controller start");
                var objER = new Wfld_IceGateRepository();
                int FileCode = objER.GetFileCodeForE07();
                if (FileCode > 0)
                {
                    lstData = E07FileCreate(lstData, FileCode);
                  
                    if (lstData != null)
                    {
                        string xml = Utility.CreateXML(lstData).ToString();
                        objER.AddManualE07Dtl(xml);
                        log.Info("E07 File creation controller end");
                        return Json(objER.DBResponse);
                    }
                    else
                        return Json(new { Status = 0, Message = " Connection Rejected By ICE Gate Server " });
                }
                else
                {
                    log.Info("E07 File code not found");
                    return Json(new { Status = 0, Message = "Error" });                    
                }
            }
            catch (Exception ex)
            {
                log.Info("Stack trace : " + ex.StackTrace + " Message : " + ex.Message);
                return null;
            }
        }
        [NonAction]
        private IList<IcegateManualE07> E07FileCreate(IList<IcegateManualE07> lstData, int FileCode)
        {
            try
            {
                log.Info("E07 file create start");
                string FileName = "", strFolderName = "";
                strFolderName = System.Configuration.ConfigurationManager.AppSettings["Ppg_MsgFileContExit"];
                DateTime dtnow = DateTime.Now.Date;
                int YearDt = dtnow.Year;
                int monthdt = dtnow.Month;
                int Daydt = dtnow.Day;
                String DtValue = YearDt.ToString() + monthdt.ToString() + Daydt.ToString();
                TimeSpan ttval = DateTime.Now.TimeOfDay;
                int hourtm = ttval.Hours;
                int mintm = ttval.Minutes;
                String TimeVal = hourtm.ToString() + mintm.ToString();

                DateTime dtt = DateTime.Now.Date;
                int YearValue = dtt.Year;
                int MonthValue = dtt.Month;
                int DayValue = dtt.Day;
                String dttvalue = YearValue.ToString() + (MonthValue.ToString().Length == 1 ? ("0" + MonthValue.ToString()) : MonthValue.ToString()) + (DayValue.ToString().Length == 1 ? ("0" + DayValue.ToString()) : DayValue.ToString());
                TimeSpan tt = DateTime.Now.TimeOfDay;
                int hourtmm = tt.Hours;
                int mintmm = tt.Minutes;
                String TimeVall = (hourtmm.ToString().Length == 1 ? ("0" + hourtmm.ToString()) : hourtmm.ToString()) + (mintmm.ToString().Length == 1 ? ("0" + mintmm.ToString()) : mintmm.ToString());
                String Filenm = "COCHE07_" + dttvalue + "_" + TimeVall + ".in";

                int idd = FileCode;
                char k1 = (char)29;

                if (!Directory.Exists(strFolderName))
                {
                    Directory.CreateDirectory(strFolderName);
                }
                FileName = strFolderName + "COCHE07_" + dttvalue + "_" + TimeVall + ".in";

                System.IO.File.Create(FileName).Dispose();
                idd = FileCode;

                lstData.ToList().ForEach(m =>
                {
                    m.FileName = Filenm;
                    m.FileCode = FileCode;
                    m.BondNo = (!string.IsNullOrEmpty(m.BondNo) ? "" : m.BondNo);
                    m.ISOCode = (!string.IsNullOrEmpty(m.ISOCode) ? "" : m.ISOCode);
                    m.Train_VehicleDate = DateTime.ParseExact(m.Train_VehicleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    m.SMTPDate = DateTime.ParseExact(m.SMTPDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                });

                TextWriter tw = new StreamWriter(FileName);
                tw.Write("HREC");
                tw.Write(k1);
                tw.Write("ZZ");
                tw.Write(k1);
                tw.Write("INWFD6CWC1");//Receiver Id
                tw.Write(k1);
                tw.Write("ZZ");
                tw.Write(k1);
                tw.Write("INWFD6");//Sender Id
                tw.Write(k1);
                tw.Write("ICES1_5");//Version Number
                tw.Write(k1);
                tw.Write("P");//Production
                tw.Write(k1);
                tw.Write(k1);
                tw.Write("COCHE07");//MessageId
                tw.Write(k1);
                tw.Write(idd);
                tw.Write(k1);
                tw.Write(dttvalue);
                tw.Write(k1);
                tw.WriteLine(TimeVall);
                tw.WriteLine("<cexit>");


                lstData.ToList().ForEach(row =>
                {
                    DateTime SMTPDate = DateTime.ParseExact(row.SMTPDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    String SMTPDateValue = SMTPDate.ToString("ddMMyyyy");

                    DateTime VehicleDate = DateTime.ParseExact(row.Train_VehicleDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    String VehicleDateValue = VehicleDate.ToString("ddMMyyyy");

                    tw.Write("F");
                    tw.Write(k1);
                    tw.Write(row.ModeOfTransport);
                    tw.Write(k1);
                    tw.Write(row.CustomHouseCode);
                    tw.Write(k1);
                    tw.Write(row.SMTPNo == "" ? "0" : row.SMTPNo);
                    tw.Write(k1);
                    tw.Write(SMTPDateValue);
                    tw.Write(k1);
                    tw.Write(row.Train_VehicleNo == "" ? "0" : row.Train_VehicleNo);
                    tw.Write(k1);
                    tw.Write(VehicleDateValue);
                    tw.Write(k1);
                    tw.Write(row.WagonNo);
                    tw.Write(k1);
                    tw.Write(row.ContainerNo);
                    tw.Write(k1);
                    tw.Write(row.ShippingLineCode);
                    tw.Write(k1);
                    tw.Write(row.Weight);
                    tw.Write(k1);
                    tw.Write(row.DestRailStationCode);
                    tw.Write(k1);
                    tw.Write(row.PortCodeOrigin);
                    tw.Write(k1);
                    tw.Write(row.GatewayPortCode);
                    tw.Write(k1);
                   // tw.Write("INBOM1");
                    tw.Write(k1);
                    tw.Write(k1);
                  
                    tw.Write(row.CarrierAgencyCode);
                    tw.Write(k1);
                    tw.WriteLine("I");

                });
                tw.WriteLine("<END-cexit>");
                tw.Write("TREC");
                tw.Write(k1);
                tw.Write(idd);
                tw.Write(Environment.NewLine);
                tw.Close();

                log.Info("E07 file create end");
                log.Info("E07 file name " + Filenm);

                //File upload
                string output = "";
                using (FileStream fs = System.IO.File.OpenRead(FileName))
                {
                    log.Info("File read start");
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    log.Info("File read end");
                    log.Info("File Upload start");
                    output = FtpFileManager.UploadFileToFtp("/live/inbound", Filenm, buffer, "5000", FileName);
                    //output = "Success";
                    
                    log.Info("File Upload end");
                }

                if (output == "Success")
                {
                    return lstData;
                }
                else
                {
                    log.Info("Error in file upload");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Info("Stack trace : " + ex.StackTrace + " Message : " + ex.Message);
                return null;
            }            
        }
        [HttpGet]
        public ActionResult ListManualE07() 
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new List<IcegateManualE07>();
            objER.ListManualE07(0, 0);
            if (objER.DBResponse.Data != null)
                objModel = (List<IcegateManualE07>)objER.DBResponse.Data;
            return PartialView(objModel);
        }
        [HttpGet]
        public JsonResult LoadMoreListManualE07(int page,int id=0)
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new List<IcegateManualE07>();
            objER.ListManualE07(id, page);
            if (objER.DBResponse.Data != null)
                objModel = (List<IcegateManualE07>)objER.DBResponse.Data;
            return Json(objModel,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewManualE07FileCreate(int id)
        {
            var objER = new Wfld_IceGateRepository();
            var objModel = new IcegateManualE07ViewModel();
            objER.ViewManualE07FileCreate(id);
            if (objER.DBResponse.Data != null)
                objModel = (IcegateManualE07ViewModel)objER.DBResponse.Data;
            return PartialView(objModel);
        }        
        #endregion

    }
}
