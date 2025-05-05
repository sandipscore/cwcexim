using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Areas.Report.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Filters;
using CwcExim.UtilityClasses;
using System.Reflection;
using System.Text;
using CwcExim.Areas.Import.Models;
using System.Data;
using CwcExim.Areas.Export.Models;
using EinvoiceLibrary;
using System.Threading.Tasks;
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Import.Controllers;
using CwcExim.Areas.CashManagement.Controllers;

namespace CwcExim.Areas.Report.Controllers
{

    public class Wlj_ReportCWCController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }


        public Wlj_ReportCWCController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.getCompanyDetails();
            objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
            HeadOffice = ""; //objCompanyDetails.CompanyName;
            HOAddress = "";//objCompanyDetails.RoAddress;
            ZonalOffice = objCompanyDetails.CompanyName;
            ZOAddress = objCompanyDetails.CompanyAddress;
            EffectVersion = Convert.ToDecimal(objCompanyDetails.EffectVersion);
            EffectVersionLogoFile = objCompanyDetails.VersionLogoFile.ToString();

        }

        #region Bulk Invoice

        [HttpGet]
        public ActionResult BulkInvoiceReport()
        {

            ExportRepository objExport = new ExportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReport(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            Login objLogin = (Login)Session["LoginUser"];
            //When Module is selected All Condition against a party

            if (ObjBulkInvoiceReport.InvoiceModule == "All")
            {
                string ModuleName = "";
                //delete all the files in the folder before creating it
                if (System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    string deletelocation = Server.MapPath("~/Docs/All/") + Session.SessionID + "/";
                    DeleteDirectory(deletelocation);
                }

                //get all the distinct invoice module and invoices list with respect to party and date range 
                ObjRR.ModuleListWithInvoice(ObjBulkInvoiceReport);
                if (ObjRR.DBResponse.Status == 1)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    List<dynamic> lstModule = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
                    IList lstDistModule = lstModule.Select(x => x.Module).Distinct().ToList();
                    string FilePath = "";


                    foreach (string Mod in lstDistModule)
                    {
                        //lstModule.Where(x => x.Module == Mod).ToList().ForEach(itemInv =>
                        //{
                        //    String ModuleName= itemInv.Module;
                        switch (Mod)
                        {
                            //Here ds is list of invoice of a module between two dates 
                            case "IMPYard":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "ASSESSMENT SHEET FCL";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforYard((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "IMPDeli":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "ASSESSMENT SHEET LCL";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforGodown((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "BTT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "BTT";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforBTT((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EC":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "EMPTY CONTAINER PAYMENT SHEET";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforEmptyContainer((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "ECTrns":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "EMPTY CONTAINER TRANSFER";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforEmptyContainer((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "BTTCONT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "BTT";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforBTTCONT((DataSet)ObjRR.DBResponse.Data, ModuleName);
                                break;
                            case "MiscInv":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "Misc Invoice";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforMisc((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;                                                              
                                
                            default:
                                FilePath = GeneratingBulkPDFforCHNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                                break;
                        }
                        //});

                    }
                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }

            else
            {
                // List<PpgInvoiceGate> PpgInvoiceGateList = ObjRR.GetBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid
                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);//, objLogin.Uid

                if (ObjRR.DBResponse.Status == 1)
                {
                    DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                    //string FilePath = GeneratingBulkPDF(PpgInvoiceGateList, ObjBulkInvoiceReport.InvoiceModuleName);
                    string FilePath = "";
                    switch (ObjBulkInvoiceReport.InvoiceModule)
                    {
                        case "IMPYard":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforYard(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "IMPDeli":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforGodown(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXPSEALCHEKING":
                            FilePath = GeneratingBulkPDFforChnExpSealChecking(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "BTT":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforBTT(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXP":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforExport(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "MiscInv":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforMisc(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "BND":
                            FilePath = GeneratingBulkPDFforBND(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "BNDAdv":
                            FilePath = GeneratingBulkPDFforBondHdb(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "EXPLod":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforExportLoadedCont(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "BTTCONT":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforBTTCONT(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                        case "PEST":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforPEST(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;                        
                        default:
                            FilePath = GeneratingBulkPDFforCHNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                    }

                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
        }
        [NonAction]
        public string GeneratingBulkPDFforChnExpSealChecking(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>Seal Checking Payment Sheet</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='3' style='text-align: right;'><span style='display: block; font-size: 12px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyGSTNo.Substring(0, 2) + "</span></td></tr>");
                html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                html.Append("</tbody>");
                html.Append("</table></td></tr><tr><td><hr /></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;

                    /***************/
                    html.Append("</tbody></table></td></tr>");
                    html.Append("<tr><td>");
                    html.Append("</td></tr>");


                    html.Append("<tr><td>");
                    html.Append("<table cellspacing='0' cellpadding='5' style='border: 1px solid #000;width:100%; margin: 0;'><tbody>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Previous Seal No. :</label> <span>" + elem.PresentSealNo + "</span></td>");
                    html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>New Seal No. : </label> <span>" + elem.NewSealNo + "</span></td></tr>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Detention From :</label> <span>" + elem.Days + " Days</span></td>");
                    html.Append("<td colspan='4' width='30%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Lock Provided : </label> <span>" + elem.LockProvided + "</span></td></tr>");
                    html.Append("<tr><td colspan='8' width='70%' style='font-size: 12px; line-height: 26px;'><label style='font-weight: bold;'>Custom Examinatiom :</label> <span>" + elem.ExamRequired + "</span></td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td></tr>");
                });
                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin-top: 20px;'>Container/Cargo Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
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

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + totamt.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");

                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-bottom:0; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 170px;'><span>&nbsp;&nbsp;</span>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th></tr>");


                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'><span>&nbsp;&nbsp;&nbsp;</span>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'><span>&nbsp;&nbsp;&nbsp;</span>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<span><br/></span>");
                html.Append("<p style='display: block; font-size: 11px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span>" + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span>" + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid #333; border-bottom: 0; border-right: 0; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
                html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
                html.Append("<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforBTT(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                CHN_ExportRepository rep = new CHN_ExportRepository();
                CHNBTTCargoDet objCargoDet = new CHNBTTCargoDet();
                rep.GetCargoDetBTTById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCargoDet = (CHNBTTCargoDet)rep.DBResponse.Data;
                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size: 12px;'>Container Freight Station,</span><br /><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 12px;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br /><span style='font-size: 12px;'><b>Email:</b> " + objCompany[0].EmailAddress + " </span><br/><label style='font-size: 16px; font-weight:bold;'>EXPORT BTT PAYMENT SHEET</label></td>");
                html.Append("<td valign='top'><img align='right' src='SWACHBHARAT' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");


                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CFS Madhavvaram-Chennai</span>");
                //html.Append("<br />EXPORT BTT PAYMENT SHEET");
                //html.Append("</td></tr>");
                html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + " </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:9pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%; font-size:9pt; border:1px solid #333; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody><tr>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + item.ExporterImporterName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> " + item.CHAName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b>" + item.ShippingLineName + "</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                //html.Append("<span>" + item.PartyName + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>Invoice Generated By :</label> <span>" + item.PaymentMode + "</span></td></tr>");
                //html.Append("</tbody> ");
                //html.Append("</table></td></tr>");

                //html.Append("<tr><td><hr/></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%; font-size:8pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });

                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:8pt;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:5%;'>Entry No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>SB No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>Commodity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>No of pkg</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px; width:10%;'>GR WT</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:center; width:10%;'>FOB</th>");
                html.Append("</tr></thead><tbody>");
                i = 1;

                // objCargoDet.ForEach(elem =>
                // {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:5%;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'> " + objCargoDet.ShippingBillNo.ToString() + " </td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.ShippingBillDate.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.CommodityName.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.NoOfUnits.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + objCargoDet.GrossWeight.ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; text-align:center; width:10%;'> " + objCargoDet.Fob.ToString() + "</td>");
                html.Append("</tr>");
                //   i = i + 1;
                // });
                html.Append("</tbody></table></td></tr>");


                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px; margin-top: 20px;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 150px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style=' border: 1px solid #000;border-bottom:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 10px; text-align: center; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 10px; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='font-size: 10px; text-align: center; width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 170px;'><span>&nbsp;&nbsp;</span>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th></tr>");


                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'><span>&nbsp;&nbsp;&nbsp;</span>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'><span>&nbsp;&nbsp;&nbsp;</span>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<span><br/></span>");
                html.Append("<p style='display: block; font-size: 11px; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid #333; border-bottom: 0; border-right: 0; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 11px; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
                html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
                html.Append("<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });
            FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforExport(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>EXPORT PAYMENT SHEET</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center; margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='11'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom:1px solid #000; padding-bottom:5px;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='5' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Container/Cargo Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 7pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt;margin-top: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
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
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforMisc(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>MISCELLANEOUS INVOICE</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style='border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='width:100%; border-bottom:1px solid #000; padding-bottom:5px;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='5' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");


                //html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");

                /*html.Append("<tr><th><b style='text-align: left; font-size: 13px; margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");*/
                /*************/
                /*Container Bind*/
                /*int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });*/
                /***************/
                /*html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");*/

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top:0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SAC Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                int i = 1; Decimal totamt = 0;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.CGSTPer.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.SGSTPer.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.IGSTPer.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 170px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='left' style='font-size:7pt;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:7pt; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforBND(DataSet ds, string InvoiceModuleName)
        {
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstStorageDate = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>BOND DELIVERY PAYMENTSHEET</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style='border: 1px solid #000; padding: 10px; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center; margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + " </h2> </td></tr>");
                
                html.Append("<tr><td colspan='11'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom:1px solid #000; padding-bottom:5px;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='5' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 7.5pt;margin-top: 8px;'>Bond No :" + item.StuffingReqNo + "</b></th>");
                html.Append("<td colspan='6' width='50%' style='font-size:7.5pt; text-align: right'><label width='15%'><b>SAC validity :</b></label> From <u>" + lstContainer[0].SacApprovedDate + "</u> to <u>" + lstContainer[0].SacValidityDate + "</u></td></tr>");
                html.Append("<tr><th colspan='2' width='20%' style='text-align: left; font-size: 7.5pt;margin-top: 15px;' valign='top'>Storage Period :</th>");
                html.Append("<td colspan='10' width='80%'><table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='0'><tbody>");

                var SacId = 0;
                var DeliId = 0;
                if (lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).Count() > 0)
                {
                    SacId = lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).ToList().Select(y => y.SacId).ToArray()[0];
                    DeliId = lstStorageDate.Where(x => x.InvoiceId == item.InvoiceId).ToList().Select(y => y.SacDelivryId).ToArray()[0];
                }

                lstStorageDate.Where(x => x.SacId == SacId && x.InvoiceId <= item.InvoiceId && x.SacDelivryId <= DeliId).ToList().ForEach(storage =>
                {
                    int j = 1;
                    html.Append("<tr><td colspan='12' cellpadding='5' valign='top'>From <u>" + storage.StorageFromDt + "</u> to <u>" + storage.StorageToDt + "</u></td></tr>");
                    j = j + 1;
                });
                html.Append("</tbody></table></td></tr>");


                html.Append("<tr><td colspan='12'>");
                int i = 1;
                Decimal totamt = 0;
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(a =>
                {
                    html.Append("<tr><td colspan='6' width='30%'>Bond No.</td><td>:</td><td colspan='6' width='70%'>" + a.BondNo.ToString().TrimEnd(',') + " </td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Bond Date</td><td>:</td><td colspan='6' width='70%'>" + a.BondDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL No.</td><td>:</td><td colspan='6' width='70%'>" + a.BOLNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL Date</td><td>:</td><td colspan='6' width='70%'>" + a.BOLDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + a.BOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE Date.</td><td>:</td><td colspan='6' width='70%'>" + a.BOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB No.</td><td>:</td><td colspan='6' width='70%'>" + a.AWBNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB Date</td><td>:</td><td colspan='6' width='70%'>" + a.AWBDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Godown No.</td><td>:</td><td colspan='6' width='70%'>" + a.GodownName.ToString().TrimEnd(',') + "</td></tr>");
                });
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='border-left: 1px solid #000; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                lstContainer.Where(z => z.InvoiceId == item.InvoiceId).ToList().ForEach(b =>
                {
                    html.Append("<tr><td colspan='6' width='40%'>Ex-BOE No.</td><td>:</td><td colspan='6' width='60%'>" + b.ExBOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Ex-BOE Date.</td><td>:</td><td colspan='6' width='60%'>" + b.ExBOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Importer</td><td>:</td><td colspan='6' width='60%'>" + item.ExporterImporterName.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CHA Name</td><td>:</td><td colspan='6' width='60%'>" + item.CHAName.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CIF Value</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIF)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>DUTY</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIF) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>No Of Pkg</td><td>:</td><td colspan='6' width='60%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                });
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
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
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        [NonAction]
        public string GeneratingBulkPDFforBondHdb(DataSet ds, string InvoiceModuleName)
        {
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>BOND ADVANCE PAYMENTSHEET</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style='border: 1px solid #000; padding: 10px; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center; margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + " </h2> </td></tr>");
                //html.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                //html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                //html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                //html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label><span>" + item.PartyName + "</span></td>");
                //html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                //html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>" + item.PartyAddress + "</span></td>");
                //html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                //html.Append("<tr><td colspan='8' width='70%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                //html.Append("<td colspan='4' width='30%' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Payee Name :</label> <span>" + item.PayeeName + "</span></td></tr>");
                //html.Append("</tbody>");
                //html.Append("</table></td></tr>");

                html.Append("<tr><td colspan='11'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom:1px solid #000; padding-bottom:5px;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='5' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 7.5pt;margin-top: 10px;'>SAC No :" + item.StuffingReqNo + "</b></th>");

                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<td colspan='6' width='50%' style='font-size:7.5pt;'><b>SAC validity :</b> From <u>" + elem.SacApprovedDate + "</u> to <u>" + elem.SacValidityDate + "</u></td>");
                });

                html.Append("</tr>");

                //html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container/Cargo Details :</b> </th></tr>");
                //html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                //html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>SR No.</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>CFS Code</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Container No.</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Size</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Arrival</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Carting</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>Date of Delivery</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>No of Days</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; padding: 5px;'>No of Week</th>");
                //html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Cargo Type</th>");
                //html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/

                // lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                //{
                //    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + i + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'></td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + item.DeliveryDate + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(item.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");
                //    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + Convert.ToDateTime(elem.DeliveryDate)+ "</td>");
                //    html.Append("<td style='border-bottom: 1px solid #000; font-size: 11px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                //    html.Append("</tr>");
                //    i = i + 1;
                //});
                /***************/
                //html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");

                //html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + item.ShippingLineName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
                //html.Append("<td style='font-size: 12px;'>OBL No :  </td>");
                //html.Append("<td style='font-size: 12px;'>Item No. : " + lstContainer.Where(x => x.InvoiceId == item.InvoiceId).Count().ToString() + "</td>");
                //html.Append("<td style='font-size: 12px;'>BOE No : " + item.BOENo.ToString().TrimEnd(',') + " </td>");
                //html.Append("<td style='font-size: 12px;'>BOE Date : " + item.BOEDate.ToString().TrimEnd(',') + " </td>");
                //html.Append("</tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + item.ExporterImporterName + " </td>");
                //html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>CHA Name : " + item.CHAName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + item.TotalNoOfPackages.ToString() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + item.TotalGrossWt.ToString("0.000") + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) : " + item.TotalSpaceOccupied.ToString("0.000") + "</td>");
                //html.Append("<td></td>");
                //html.Append("<td></td>");
                //html.Append("</tr>");
                //html.Append("</table>");
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='border-right: 1px solid #000; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL No.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOLNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOL Date</td><td>:</td><td colspan='6' width='70%'>" + elem.BOLDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOENo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>BOE Date.</td><td>:</td><td colspan='6' width='70%'>" + elem.BOEDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB No.</td><td>:</td><td colspan='6' width='70%'>" + elem.AWBNo.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>AWB Date</td><td>:</td><td colspan='6' width='70%'>" + elem.AWBDate.ToString().TrimEnd(',') + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Godown No.</td><td>:</td><td colspan='6' width='70%'>" + elem.GodownName + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><td colspan='6' width='40%'>CHA Name</td><td>:</td><td colspan='6' width='60%'>" + item.CHAName + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>CIF Value</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>DUTY</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(z => z.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total</td><td>:</td><td colspan='6' width='60%'>" + (lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InvoiceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>No Of Pkg</td><td>:</td><td colspan='6' width='60%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                    html.Append("<tr><td colspan='6' width='40%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='60%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("</tr></tbody></table>");
                });
                html.Append("</td></tr>");

                    html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                    html.Append("<thead><tr>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                    //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                    html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                    html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                    html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
                    html.Append("<tbody>");
                    i = 1;
                    /*Charges Bind*/
                
                i = 1;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                        //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
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
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforExportLoadedCont(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>LOADED CONTAINER PAYMENT SHEET</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");          

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Bill Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='11'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom:1px solid #000; padding-bottom:5px;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='5' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");

                html.Append("<tr><td colspan='6' width='50%' style='text-align: left; font-size: 7pt; margin: 0;'><b>Container/Cargo Details :</b></td> <td colspan='6' width='50%' style='text-align:right;'><span style='text-align: left; font-size: 7pt; margin-top: 10px;'><b> On Wheel Hours : </b> " + lstContainer[0].OnWheelHours + "</span></td> </tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt ;margin: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 200px;'>IGST</th>");
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

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {

                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: right; width: 100px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 7pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; border-top:0; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; font-size: 7pt; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 7pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 7pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 7pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");             
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid #333; border-bottom: 0; border-right: 0; border-top: 0; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        public string GeneratingBulkPDFforCHNAll(DataSet ds, string InvoiceModuleName)
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


                        if (data.SACCode == Sac[ii] && data.InoviceId == item.InvoiceId)
                        {
                            Taxa[ii] = Taxa[ii] + data.Taxable;
                            cgstamt = cgstamt + data.CGSTAmt;
                            sgstamt = sgstamt + data.SGSTAmt;
                            igstamt = igstamt + data.IGSTAmt;
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

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
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
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; font-size: 11px; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        //[NonAction]
        //public string GeneratingBulkPDFforGodown(DataSet ds, string InvoiceModuleName, string All)
        //{
        //    var FileName = "";
        //    var location = "";
        //    Einvoice obj = new Einvoice();
        //    CurrencyToWordINR objCurr = new CurrencyToWordINR();
        //    List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
        //    List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
        //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
        //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
        //    List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
        //    //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);
        //    //List<dynamic> lstReassesbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
        //    List<string> lstSB = new List<string>();
        //    int cargotype = 0;
        //    string Container = "";
        //    string dt = "";
        //    string dttype = "Date of Seal Cutting";



        //    //lstCargoDetail.ToList().ForEach(item =>
        //    //{
        //    //    cargotype = (int)item.CargoType;
        //    //});

        //    //lstInvoice.ToList().ForEach(item =>
        //    //{
        //    //    Ppg_ReportRepository rep = new Ppg_ReportRepository();
        //    //    PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
        //    //    rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
        //    //    if (rep.DBResponse.Data != null)
        //    //    {
        //    //        objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
        //    //    }



        //    //    lstReassesment.ForEach(itemm =>
        //    //    {
        //    //        if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
        //    //        {
        //    //            Container = "(Re-Assessment)";

        //    //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
        //    //            dt = repp.GetPreviousInvDate(itemm.cfscode);
        //    //            dttype = "Previous Delivery Date";


        //    //        }

        //    //    });


        //    //    lstReassesbulk.ForEach(data =>
        //    //    {
        //    //        if (data.invoiceid == item.InvoiceId)
        //    //        {
        //    //            Container = "(Re-Assessment)";
        //    //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
        //    //            dt = repp.GetPreviousInvDate(data.cfscode);
        //    //            dttype = "Previous Delivery Date";

        //    //        }
        //    //    });
        //    lstInvoice.ToList().ForEach(item =>
        //    {
        //        StringBuilder html = new StringBuilder();
        //        /*Header Part*/
        //        html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
        //        html.Append("<tr><td colspan='12'>");
        //        html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
        //        html.Append("<tr>");
        //        html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
        //        html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
        //        html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>LOADED CONTAINER PAYMENT SHEET</label></td>");
        //        html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
        //        html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
        //        html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
        //        html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
        //        html.Append("</tbody></table></td>");
        //        //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

        //        if (item.SignedQRCode == "")
        //        { }
        //        else
        //        {
        //            if (item.SupplyType == "B2C")
        //            {
        //                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

        //            }
        //            else
        //            {
        //                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
        //            }
        //        }

        //        html.Append("</tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td></tr>");
        //        html.Append("</thead></table>");

        //        html.Append("<tr><td colspan='10'>");
        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom:1px solid #000; padding-bottom:5px;'><tbody><tr>");
        //        html.Append("<td colspan='5' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='5' width='40%'>");
        //        html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='30%''>Invoice Date</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceDate + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%''>State</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%''>State Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyStateCode + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Supply Type</th><th>:</th><td colspan='6' width='70%'>" + item.SupplyType + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");
        //        html.Append("</tr></tbody></table>");
        //        html.Append("</td></tr>");

        //        html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 10px;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
        //        html.Append("<br/><b>Container / CBT Details :</b> </th></tr>");
        //        html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align: center;' cellspacing='0' cellpadding='8'>");
        //        html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>SR No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>ICD Code</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>Container / CBT No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:6%;'>Size</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Arrival</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Destuffing</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Delivery</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Days</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Week</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>" + dttype + "</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; width:8%;'>Cargo Type</th>");
        //        html.Append("</tr></thead><tbody>");
        //        /*************/
        //        /*Container Bind*/
        //        int i = 1;
        //        Decimal totamt = 0;
        //        lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
        //        {
        //            if (dt != "")
        //            {
        //                elem.SealCuttingDt = dt;
        //            }
        //            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DestuffingDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1)) / 7)).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.SealCuttingDt.ToString() + "</td>");
        //            html.Append("<td style='border-bottom: 1px solid #000;'>" + (cargotype == 0 ? "" : (cargotype == 1 ? "Haz" : "Non-Haz")) + "</td>");
        //            html.Append("</tr>");
        //            i = i + 1;
        //        });
        //        /***************/
        //        html.Append("</tbody></table></td></tr>");
        //        html.Append("<tr><td colspan='12'>");
        //        html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");

        //        html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOENo).FirstOrDefault() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOEDate).FirstOrDefault() + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("</tr></tbody></table>");


        //        html.Append("</td></tr>");

        //        html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Charges :</h3> </th></tr>");
        //        html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<thead><tr>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
        //        html.Append("<tbody>");
        //        i = 1;
        //        /*Charges Bind*/

        //        lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
        //        {
        //            if (data.Taxable > 0)
        //            {
        //                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.OperationDesc + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SACCode + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.Taxable.ToString("0.00") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.CGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.IGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
        //                html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
        //                i = i + 1;
        //                totamt = totamt + data.Taxable;
        //            }
        //        });
        //        html.Append("</tbody>");
        //        html.Append("</table></td></tr></table>");


        //        html.Append("<table style='border: 1px solid #000; width: 100%;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
        //        html.Append("<th rowspan='2' style='width: 24%;'></th>");
        //        html.Append("<th rowspan='2' style='width: 100px;'></th>");
        //        html.Append("<th rowspan='2' style='width: 80px;'></th>");
        //        html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.00") + "</th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
        //        html.Append("<tr><th style='width: 50px;'></th>");
        //        html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
        //        html.Append("<th rowspan='2' style='width: 24%;'></th>");
        //        html.Append("<th rowspan='2' style='width: 100px;'></th>");
        //        html.Append("<th rowspan='2' style='width: 80px;'></th>");
        //        html.Append("<th rowspan='2' style='width: 130px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
        //        html.Append("<tr><th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th></tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
        //        html.Append("<th rowspan='2' style='width: 24%;'></th>");
        //        html.Append("<th rowspan='2' style='width: 100px;'></th>");
        //        html.Append("<th rowspan='2' style='width: 80px;'></th>");
        //        html.Append("<th rowspan='2' style='width: 130px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='width: 200px;'></th>");
        //        html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
        //        html.Append("<tr><th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th>");
        //        html.Append("<th style='width: 50px;'></th></tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
        //        html.Append("</tr>");
        //        html.Append("<tr>");
        //        html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
        //        html.Append("</tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr>");
        //        html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
        //        html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
        //        html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
        //        html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
        //        html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
        //        html.Append("</td>");
        //        html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr>");
        //        html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
        //        html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
        //        html.Append("<span><br/><br/></span>");
        //        html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
        //        html.Append("</td>");
        //        html.Append("</tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");
        //        html.Append("</td>");
        //        html.Append("</tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
        //        html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
        //        html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
        //        html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
        //        html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
        //        html.Append("</tr></tbody></table>");
        //        html.Append("</td></tr></tbody></table>");

        //        /***************/

        //        html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
        //        html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
        //        lstSB.Add(html.ToString());

        //    });
        //    if (All != "All")
        //    {
        //        FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
        //        location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
        //        if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //        }
        //    }
        //    else
        //    {
        //        FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
        //        location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
        //        if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
        //        }
        //    }

        //    using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
        //    {
        //        rp.HeadOffice = "";
        //        rp.HOAddress = "";
        //        rp.ZonalOffice = "";
        //        rp.ZOAddress = "";
        //        rp.Version = EffectVersion;
        //        rp.Effectlogofile = EffectVersionLogoFile;
        //        rp.GeneratePDF(location, lstSB); 
        //    }
        //    return "/Docs/" + Session.SessionID + "/" + FileName;
        //}

        [NonAction]
        public string GeneratingBulkPDFforGodown(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var SEZis = "";
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);
            //List<dynamic> lstReassesbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            int cargotype = 0;
            string Container = "";
            string dt = "";
            //string dttype = "Date of Seal Cutting";

            //lstCargoDetail.ToList().ForEach(item =>
            //{
            //    cargotype = (int)item.CargoType;
            //});

            lstInvoice.ToList().ForEach(item =>
            {
                DSR_ReportRepository rep = new DSR_ReportRepository();
                DSRSDBalancePrint objSDBalance = new DSRSDBalancePrint();
                //rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                //if (rep.DBResponse.Data != null)
                //{
                //    objSDBalance = (DSRSDBalancePrint)rep.DBResponse.Data;
                //}

                //lstReassesment.ForEach(itemm =>
                //{
                //    if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
                //    {
                //        Container = "(Re-Assessment)";

                //        DSR_ReportRepository repp = new DSR_ReportRepository();
                //        dt = repp.GetPreviousInvDate(itemm.cfscode);
                //        //dttype = "Previous Delivery Date";
                //    }
                //});

                //lstReassesbulk.ForEach(data =>
                //{
                //    if (data.invoiceid == item.InvoiceId)
                //    {
                //        Container = "(Re-Assessment)";
                //        DSR_ReportRepository repp = new DSR_ReportRepository();
                //        dt = repp.GetPreviousInvDate(data.cfscode);
                //        //dttype = "Previous Delivery Date";

                //    }
                //});

                //if (item.Bond == 1)
                //{
                //    InvoiceModuleName = "Cargo Shifting In PBW";
                //}
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='80%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label> <br/> <label style='font-size: 14px; font-weight:bold;'>" + Container + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border:1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000; padding-bottom:5px;'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%' style='border-bottom: 1px solid #000; padding-bottom:5px;'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                //html.Append("<span>" + item.PartyName + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>Invoice Generated By :</label> <span>" + item.PaymentMode + "</span></td></tr>");
                //html.Append("</tbody>");
                //html.Append("</table></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br/><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:50px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:100px;'>ICD Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:100px;'>Container / CBT No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>Size</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>Date of Arrival</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>Date of Destuffing</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>Date of Delivery</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:200px;'>No of Days</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:200px;'>No of Weeks</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>ODC Type</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:80px;'>" + dttype + "</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; text-align: center; width:80px;'>Cargo Type</th>");
                html.Append("</tr>");

                html.Append("<tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>Arrival to Destuff</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>Destuff to Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>Arrival to Destuff</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>Destuff to Delivery</th>");
                html.Append("</tr>");

                html.Append("</thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                         {
                             if (dt != "")
                             {
                                 elem.SealCuttingDt = dt;
                             }
                             html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DestuffingDate + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1).ToString() + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1)) / 7)).ToString() + "</td>");
                             html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.SealCuttingDt.ToString() + "</td>");
                             html.Append("<td style='border-bottom: 1px solid #000;'>" + (cargotype == 0 ? "" : (cargotype == 1 ? "Haz" : "Non-Haz")) + "</td>");
                             html.Append("</tr>");
                             i = i + 1;
                         });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                // html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");

                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOENo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOEDate).FirstOrDefault() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Max(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Max(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Max(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Max(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Gr.Wt (KG)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Area (SQM)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.00") + "</td></tr>");
                //html.Append("<tr><td colspan='6' width='30%'>CBM</td><td>:</td><td colspan='6' width='70%'>" + item.SpaceOccupiedCBM.ToString("0.00") + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");

                //html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + item.ShippingLineName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
                //html.Append("<td style='font-size: 12px;'>OBL No : " + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Item No. : " + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td>");
                //html.Append("<td style='font-size: 12px;'>BOE No : " + item.BOENo + " </td>");
                //html.Append("<td style='font-size: 12px;'>BOE Date : " + item.BOEDate + " </td>");
                //html.Append("</tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + item.ExporterImporterName + " </td>");
                //html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>CHA Name : " + item.CHAName + " </td><td style='font-size: 12px;' colspan='2'>Godown No. : " + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + item.TotalNoOfPackages.ToString() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + item.TotalGrossWt.ToString("0.000") + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) :" + item.TotalSpaceOccupied.ToString("0.000") + " </td>");
                //html.Append("<td></td>");
                //html.Append("<td></td>");
                //html.Append("</tr>");
                //html.Append("</table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size:7pt;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                //html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
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

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "%</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "%</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "%</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 100px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border:1px solid #000; border-top:0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                //html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totamt.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + ((Convert.ToDecimal(totamt) + Convert.ToDecimal(item.TotalCGST) + Convert.ToDecimal(item.TotalSGST) + Convert.ToDecimal(item.TotalIGST))).ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                //html.Append("<tr>");
                //html.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; padding: 5px;' colspan='13'>Round Up : " + (Convert.ToDecimal(item.InvoiceAmt) - (Convert.ToDecimal(totamt) + Convert.ToDecimal(item.TotalCGST) + Convert.ToDecimal(item.TotalSGST) + Convert.ToDecimal(item.TotalIGST))).ToString("0.##") + "</th>");
                //html.Append("</tr>");

                //html.Append("<tr>");
                //html.Append("<th style='border-top:0; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; padding: 5px;' colspan='13'>Total Invoice : " + item.InvoiceAmt.ToString("0.##") + "</th>");
                //html.Append("</tr>");

                //html.Append("<tr>");
                //html.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                //html.Append("</tr>");
                //html.Append("<tr>");
                //html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                //html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size:6pt; text-align: right; width: 100px;'>" + (Convert.ToDecimal(item.InvoiceAmt) - (Convert.ToDecimal(totamt) + Convert.ToDecimal(item.TotalCGST) + Convert.ToDecimal(item.TotalSGST) + Convert.ToDecimal(item.TotalIGST))).ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : <span style='font-size:7pt;'>" + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</span></th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='8' width='75%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 7pt; text-align: left;' colspan='3' width='25%'>Payer Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='8' width='75%'>Payer Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/><br/><br/>(Authorized Signatories)</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
                Container = "";
                dt = "";
                //dttype = "Date of Seal Cutting";
            });
            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforEmptyContainer(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();
            List<dynamic> lstReassesmentec = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
            List<dynamic> lstReassesmentecbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
            string Container = "";
            string cfscode = "";





            lstInvoice.ToList().ForEach(item =>
            {
                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                cfscode = "";
                lstContainer.ForEach(dat =>
                {
                    if (dat.InoviceId == item.InvoiceId)

                        cfscode = dat.CFSCode;








                });
                Container = "";


                string dt = "";
                string dttype = "Date of Destuffing";
                lstReassesmentec.ForEach(itemm =>
                {
                    if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
                    {
                        Container = "(Re-Assessment)";

                        Ppg_ReportRepository repp = new Ppg_ReportRepository();
                        dt = repp.GetPreviousInvDate(itemm.cfscode);
                        dttype = "Previous Delivery Date";


                    }

                });


                lstReassesmentecbulk.ForEach(data =>
                {
                    if (data.invoiceid == item.InvoiceId)
                    {
                        Container = "(Re-Assessment)";
                        Ppg_ReportRepository repp = new Ppg_ReportRepository();
                        dt = repp.GetPreviousInvDate(data.cfscode);
                        dttype = "Previous Delivery Date";

                    }
                });


                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label><label style = 'font-size: 14px; font-weight:bold;' > " + Container + "</label>");
                html.Append("</td></tr>");

                html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                //html.Append("<span>" + item.PartyName + "</span></td>");
                //html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                //html.Append("Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                //html.Append("<label style='font-weight: bold;'>Invoice Generated By :</label> <span>" + item.PaymentMode + "</span></td></tr>");                
                //html.Append("</tbody> ");
                //html.Append("</table></td></tr>");

                html.Append("<tr><td><hr /></td></tr>");
                html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br /><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:8pt;' cellspacing='0' cellpadding='8'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Arrival</th>");
                //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Carting</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + dttype + "</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Days</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Week</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Seal Cutting</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    if (dt != "")
                    {
                        elem.DestuffingEntryDate = dt;
                    }
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    // html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item.CartingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.DestuffingEntryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1).ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1)) / 7)).ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.SealCuttingDt.ToString() + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");

                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + item.BOENo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");

                //html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + item.ShippingLineName + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
                //html.Append("<td style='font-size: 12px;'>OBL No : " + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Item No. : " + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td>");
                //html.Append("<td style='font-size: 12px;'>BOE No : " + item.BOENo + " </td>");
                //html.Append("<td style='font-size: 12px;'>BOE Date : " + item.BOEDate + " </td>");
                //html.Append("</tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + item.ExporterImporterName + " </td>");
                //html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                //html.Append("<tr><td style='font-size: 12px;' colspan='3'>CHA Name : " + item.CHAName + " </td><td style='font-size: 12px;' colspan='2'>Godown No. : " + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + " </td></tr>");
                //html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + item.TotalNoOfPackages.ToString() + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + item.TotalGrossWt.ToString("0.000") + " </td>");
                //html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) :" + item.TotalSpaceOccupied.ToString("0.000") + " </td>");
                //html.Append("<td></td>");
                //html.Append("<td></td>");
                //html.Append("</tr>");
                //html.Append("</table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
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

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + totamt.ToString("0") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("</td></tr></tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
                Container = "";
                dt = "";
            });
            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforYard(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            Einvoice obj = new Einvoice();
            string dtype = "Date of Arrival";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            //List<dynamic> lstReasses = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            string Container = "";
            string cfscode = "";
            //lstReasses.ForEach(item =>
            //{
            //    if (item.cfscode != "")
            //        Container = "(Re-Assessment)";

            //});

            lstInvoice.ToList().ForEach(item =>
            {
                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }



                lstContainer.ForEach(dat =>
                {
                    if (dat.InoviceId == item.InvoiceId)

                        cfscode = dat.CFSCode;
                    else

                        cfscode = "";

                });
                Container = "";

                //lstReassesment.ForEach(data =>
                //{
                //    if (data.cfscode == cfscode)
                //    {
                //        Container = "(Re-Assessment)";
                //        dtype = "Previous Delivery Date";
                //    }


                //});
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }
                
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center; margin: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;border-bottom:1px solid #000; padding-bottom:5px;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%''>Payee Name</th><th>:</th><td colspan='6' width='70%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Code</th><th>:</th><td colspan='6' width='70%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");
                
                html.Append("<tr><th style='font-size:7pt; text-align: left; margin-top: 10px;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br /><b style='margin-top: 10px;'>Container / CBT Details :</b> </th></tr>");

                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align: center;' cellspacing='0' cellpadding='8'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>" + dtype + " </th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Weight</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; width:8%;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                string OblNo = "";
                string RMS = "";
                int i = 1;
                int flagvalue = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    //get no of days and weeks for IMPYard Payment Sheet
                    PpgDaysWeeks objDaysWeeks = new PpgDaysWeeks();

                    if (Container == "(Re-Assessment)")
                    {
                        elem.ArrivalDate = item.StuffingReqDate;
                        flagvalue = 1;
                    }

                    rep.GetDaysWeeksForIMPYard(item.InvoiceId, elem.CFSCode, flagvalue);
                    if (rep.DBResponse.Data != null)
                    {
                        objDaysWeeks = (PpgDaysWeeks)rep.DBResponse.Data;

                    }

                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt.ToString("0.000") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objDaysWeeks.Days + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");

                    OblNo = OblNo + elem.OBL_No + ".";
                    RMS = elem.RMS;

                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + OblNo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + item.BOENo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + item.BOEDate + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>RMS</td><td>:</td><td colspan='6' width='70%'>" + RMS + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align: right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align: right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px; text-align: right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                decimal totalamt = 0;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.Taxable.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.CGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.SGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + data.IGSTPer.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                    i = i + 1;
                    totalamt = totalamt + data.Taxable;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 130px;'>" + totalamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
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
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                /***************/

                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());
            });

            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforPEST(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            Einvoice obj = new Einvoice();

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);

            List<string> lstSB = new List<string>();
            int cargotype = 0;
            string Container = "";
            string dt = "";
            string InvoiceMonth = "";
            
            lstInvoice.ToList().ForEach(item =>
            {
                InvoiceMonth = item.InvoiceDate.Split('-')[1].ToString();

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + (item.SupplyType == "B2C" ? LoadImage(obj.GenerateQCCode(item.SignedQRCode)) : LoadImage(obj.GenerateQCCode(item.SignedQRCode))) + "'/> </td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                    }
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; text-align: center; margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply (POS)</th><th>:</th><td colspan='6' width='70%'>" + item.PlaceOfSupply + " </td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'> " + item.IsService + "  </td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='20%'>Invoice Date</th><th>:</th><td colspan='6' width='80%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='20%'>State</th><th>:</th><td colspan='6' width='80%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='20%'>State Code</th><th>:</th><td colspan='6' width='80%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='20%''>Payee Name</th><th>:</th><td colspan='6' width='80%'>" + item.PayeeName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='20%'>Supply Type</th><th>:</th><td colspan='6' width='80%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='20%'>Party Code</th><th>:</th><td colspan='6' width='80%'>" + item.PartyCode.ToString() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                if(InvoiceMonth=="Jan") { InvoiceMonth = "JANUARY"; }
                else if (InvoiceMonth == "Feb") { InvoiceMonth = "FEBRUARY"; }
                else if (InvoiceMonth == "Mar") { InvoiceMonth = "MARCH"; }
                else if (InvoiceMonth == "Apr") { InvoiceMonth = "APRIL"; }
                else if (InvoiceMonth == "May") { InvoiceMonth = "MAY"; }
                else if (InvoiceMonth == "Jun") { InvoiceMonth = "JUNE"; }
                else if (InvoiceMonth == "Jul") { InvoiceMonth = "JULY"; }
                else if (InvoiceMonth == "Aug") { InvoiceMonth = "AUGUST"; }
                else if (InvoiceMonth == "Sep") { InvoiceMonth = "SEPTEMBER"; }
                else if (InvoiceMonth == "Oct") { InvoiceMonth = "OCTOBER"; }
                else if (InvoiceMonth == "Nov") { InvoiceMonth = "NOVEMBER"; }
                else if (InvoiceMonth == "Dec") { InvoiceMonth = "DECEMBER"; }


                html.Append("<tr><td style='text-align: left; font-size: 8pt; margin-top: 10px;'><b>Nature of Invoice:  Pest Control Services Bill for Fumigation/Spray/Termite Control For The Month of  " + InvoiceMonth + "  </b></td></tr>");

                html.Append("<tr><td style='text-align: left; font-size: 8pt; margin-top: 10px;'><b>Pest Control Detail:</b></td></tr>");

                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='8'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Place Of Workdone</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Date Of Workdone</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>Quantity Fumigated</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Amount</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {                    
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.PlaceOfWorkDone + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.DateOfWorkDone + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + elem.QunatityFumigated + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.Rate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.Amount + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><td>");
                
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 8pt;margin-top: 0;'>Pest Control Charges :</h3> </th></tr>");

                html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; text-align: right; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 140px;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; width: 50px;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align: right; width: 100px;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
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
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.AllTotal.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='font-size: 6pt; text-align: right; width: 50px;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 150px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 170px;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 140px;'></th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
                html.Append("<tr><th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th>");
                html.Append("<th style='width: 50px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.00")) + "</th></tr>");
                html.Append("<tr><th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 7.5pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> " + objCompany[0].BankName + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> " + objCompany[0].AccountNo + "</p>");
                html.Append("<p style='display: block; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span>" + objCompany[0].Branch + " & " + objCompany[0].IFSC + "</p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border-left: 1px solid #333; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 7.5pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:7pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                html.Append("<tr><th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th style='font-size: 7.5pt; text-align: left;'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                //html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                //html.Append("<tbody>");                               
                //html.Append("<tr><th style='font-size: 7.5pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/><br/><br/>Signature CHA / Importer</th>");
                //html.Append("<th style='font-size: 7.5pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/><br/><br/>Assistant <br/>(Signature)</th>");
                //html.Append("<th style='font-size: 7.5pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/><br/><br/>For Central Warehousing Corporation<br/><br/><br/>(Authorized Signatories)</th></tr>");
                //html.Append("</tbody></table>");
                //html.Append("</td></tr></tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html = html.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                lstSB.Add(html.ToString());

            });
            if (All != "All")
            {
                FileName = "BulkReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
                }
            }
            else
            {
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                location = Server.MapPath("~/Docs/All/") + Session.SessionID + "/" + FileName;
                if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/All/") + Session.SessionID))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/All/") + Session.SessionID);
                }
            }

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
            {
                rp.HeadOffice = "";
                rp.HOAddress = "";
                rp.ZonalOffice = "";
                rp.ZOAddress = "";
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [NonAction]
        public string GeneratingBulkPDFforBTTCONT(DataSet ds, string InvoiceModuleName)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwccfs@gmail.com</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style=' border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px;text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + "(" + item.PartyStateCode + ")</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Is Service</th><th>:</th><td colspan='6' width='70%'>YES</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Date</th><th>:</th><td colspan='6' width='65%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State</th><th>:</th><td colspan='6' width='65%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>State Code</th><th>:</th><td colspan='6' width='65%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'></td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 7pt;'>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br /><b style='text-align: left; font-size: 7pt;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Size</th>");
                //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>ODC</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>From Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>To Date</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                List<string> lstCFSCode = new List<string>();
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    if (!lstCFSCode.Any(y => y == elem.CFSCode))
                    {
                        lstCFSCode.Add(elem.CFSCode);
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                        //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ISODC + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ArrivalDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + item.DeliveryDate + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    }
                });

                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin:0;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/

                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    if (data.Taxable > 0)
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 130px; text-align:right;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.CGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.SGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.IGSTAmt.ToString("0.00") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; width: 100px; text-align:right;'>" + data.Total.ToString("0.00") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totamt.ToString("0.00") + "</th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.AllTotal.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalCGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalSGST.ToString("0.00") + "</th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px; text-align:right;'>" + item.TotalIGST.ToString("0.00") + "</th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%;'cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='font-size: 6pt; text-align: left; width: 200px;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: center; width: 100px;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style=' border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 170px; text-align:left;'>Total Invoice :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; margin-bottom: 10px; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:7pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b>  " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; border: 1px solid #333; margin-bottom:3px;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td cellpadding='5' style='font-size: 6pt; text-align: left; width: 50%; vertical-align: bottom;'>");
                html.Append("<p style='display: block; font-size: 6pt; margin:0;'>Company's Bank Details</p>");
                html.Append("<p style='display: block; font-size: 6pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Bank Name:</span> </p>");
                html.Append("<p style='display: block; font-size: 6pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>A/c No:</span> </p>");
                html.Append("<p style='display: block; font-size: 6pt; margin:0; font-weight: bold;'><span style='display: inline-block; width: 150px; font-weight: normal;'>Branch & IFS Code:</span></p>");
                html.Append("</td>");
                html.Append("<td style='width: 50%; padding-right: 0; padding-bottom: 0;'>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='border: 1px solid #333; border-top: 0; border-bottom: 0; border-right: 0; padding: 0 15px;'>");
                html.Append("<p style='text-align: right; font-size: 6pt; margin:0; font-weight: bold;'>for Central Warehousing Corporation.</p>");
                html.Append("<span><br/><br/></span>");
                html.Append("<p style='text-align: right; font-size: 6pt; margin:0;'>Authorised Signatory</p>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                //html.Append("<table style='width:100%;' cellspacing='0' cellpadding='6'>");
                //html.Append("<tbody>");
                //html.Append("<tr>");
                //html.Append("<td style='font-size: 6pt; text-align: left;'colspan='2'>Receipt No.: ");
                //html.Append("<label style='font-weight: bold;'></label>");
                //html.Append("</td>");
                //html.Append("<td style='font-size: 6pt; text-align: left;' colspan='2'>Party Code:");
                //html.Append("<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label>");
                //html.Append("</td>");
                //html.Append("</tr>");
                //html.Append("<tr>");
                //html.Append("<td style='font-size: 6pt; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
                //html.Append("<td style='font-size: 6pt; text-align: left;'><br/><br/>Assistant <br/>(Signature)</td>");
                //html.Append("<td style='font-size: 6pt; text-align: left;'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                //html.Append("<td style='font-size: 6pt; text-align: left;'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                //html.Append("</tr><tr><td colspan=4 style='font-size: 6pt; text-align: left;'>If any changes in INVOICE will be done within 15 days.After 15 days changes will not be done</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                /***************/
                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
                rp.Version = EffectVersion;
                rp.Effectlogofile = EffectVersionLogoFile;
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateBulkInvoiceReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "BulkInvoice.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/BulkInvoice/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                {

                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/BulkInvoice/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }


        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType)
        {
            Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
            List<invoiceLIst> LstinvoiceLIst = new List<invoiceLIst>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetInvoiceList(FromDate, ToDate, invoiceType);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                //LstinvoiceLIst = (List<invoiceLIst>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Download()
        {
            String session = Session.SessionID;
            Ppg_ReportCWCController obj = new Ppg_ReportCWCController();
            //////int CurrentFileID = Convert.ToInt32(FileID); 
            string fileSavePath = "";
            fileSavePath = Server.MapPath("~/Docs/All/") + Session.SessionID;
            var filesCol = obj.GetFile(fileSavePath).ToList();
            string FileList = "";
            //string FileList = "/Docs/All/" + Session.SessionID + "/";
            for (int i = 0; i < filesCol.Count; i++)
            {
                FileList = FileList + "/Docs/All/" + Session.SessionID + "/" + (filesCol[i].FileName) + ",";
            }
            var ObjResult = new { Status = 1, Message = FileList };
            return Json(ObjResult);
        }

        [NonAction]
        public List<FileList> GetFile(string fileSavePath)
        {
            List<FileList> listFiles = new List<FileList>();
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                listFiles.Add(new FileList()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + "/" + item.Name
                });
                i = i + 1;
            }
            return listFiles;
        }

        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        #endregion 

        #region Invoice Print

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKHS ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                //if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }
        [NonAction]
        private string GeneratingPDF(PpgInvoiceGate objGP, int InvoiceId, string InvoiceModeleName)
        {
            string html = "";

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/BulkReport" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>" +
                "<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>" +
                "<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />" +
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span><br/>" +
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" + InvoiceModeleName + "</span></td></tr>" +
                "<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" +
                "CWC GST No. <label>" + objGP.CompanyGstNo + "</label></span></td></tr>" +
                "<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>" +
                "<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>" +
                "<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + objGP.InvoiceNo + "</span></td>" +
                "<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + objGP.InvoiceDate + "</span></td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>" +
                "<span>" + objGP.PartyName + "</span></td>" +
                "<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + objGP.PartyState + "</span> </td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>" +
                "Party Address :</label> <span>" + objGP.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>" +
                "<label style='font-weight: bold;'>State Code :</label> <span>" + objGP.PartyStateCode + "</span></td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGstNo + "</span></td>" +
                "</tr></tbody> " +
                "</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>" +
                "<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:80%;' cellspacing='0' cellpadding='10'>" +
                "<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>" +
                "<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody>";
            int i = 1;
            foreach (var container in objGP.LstContainersGate)
            {
                html = html + "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CfsCode + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.FromDate + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ToDate + "</td></tr>";
                i = i + 1;
            }

            html = html + "</tbody></table></td></tr>" +
            "<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>" +
            "<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>" +
            "<thead><tr>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SR No.</th>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Charge Code</th>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Description</th>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>HSN Code</th>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>" +
            "<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Taxable Amt.</th>" +
            "<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>CGST</th>" +
            "<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SGST</th>" +
            "<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>IGST</th>" +
            "<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Total</th></tr><tr>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>" +
            "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th></tr></thead>" +
            "<tbody>";
            i = 1;
            foreach (var charge in objGP.LstChargesGate)
            {
                html = html + "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeSD + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 8px; text-align: center;'>" + charge.ChargeDesc + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 8px; text-align: center;'>" + charge.HsnCode + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.TaxableAmt.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTRate.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTRate.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTRate.ToString("0") + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0") + "</td>" +
                    "<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0") + "</td></tr>";
                i = i + 1;
            }
            html = html + "</tbody>" +
                "</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> " +
                "<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalTax.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalAmt.ToString("0") + "</td>" +
                "</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>" +
                "Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>" +
                "" + ConvertNumbertoWords(Convert.ToInt32(objGP.TotalAmt)) + "</td>" +
                "</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th>" +
                "<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='6'>0</td>" +
                "</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>" +
                "<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: " +
                "<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:" +
                "<label style='font-weight: bold;'>" + objGP.PartyCode + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>" +
                "*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>" +
                "</td></tr></tbody></table>";
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/BulkReport" + InvoiceId.ToString() + ".pdf";
        }

        private string GeneratingBulkPDF(List<PpgInvoiceGate> objGPList, string InvoiceModeleName)
        {

            List<string> HtmlList = new List<string>();

            string currDateTime = DateTime.Now.ToString("ddMMyyyyhhmmtt");

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/BulkReport_" + currDateTime + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }

            foreach (PpgInvoiceGate objGP in objGPList.OrderBy(x => x.InvoiceNo).ToList())
            {
                StringBuilder html = new StringBuilder();
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span><br/>");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" + InvoiceModeleName + "</span></td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objGP.CompanyGstNo + "</label></span></td></tr>");
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
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGstNo + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:80%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody>");
                int i = 1;
                foreach (var container in objGP.LstContainersGate)
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CfsCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.FromDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ToDate + "</td></tr>");
                    i = i + 1;
                }

                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                foreach (var charge in objGP.LstChargesGate)
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeSD + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 8px; text-align: center;'>" + charge.ChargeDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 8px; text-align: center;'>" + charge.HsnCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.TaxableAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTRate.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTRate.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTRate.ToString("0") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0") + "</td></tr>");
                    i = i + 1;
                }
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
                html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalTax.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalAmt.ToString("0") + "</td>");
                html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>");
                html.Append("Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>");
                html.Append("" + ConvertNumbertoWords(Convert.ToInt32(objGP.TotalAmt)) + "</td>");
                html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th>");
                html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='6'>0</td>");
                html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
                html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
                html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
                html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                HtmlList.Add(html.ToString());
            }


            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, HtmlList);
            }
            return "/Docs/" + Session.SessionID + "/BulkReport_" + currDateTime + ".pdf";
        }
        #endregion

        #region Bulk Receipt  Report
        [HttpGet]
        public ActionResult BulkReceiptReport()
        {
            return PartialView();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkReceiptReport(BulkReceiptReport ObjBulkReceiptReport)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetTDSDepositReport(BulkReceiptReport ObjBulkReceiptReport)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateTDSDepositReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkSDReceiptReport(BulkReceiptReport ObjBulkReceiptReport)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.GetBulkCashreceipt(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkSDReceiptReport(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }
        [NonAction]
        public string GenerateBulkReceiptReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                /*
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + item.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                */
                /*************/
                /*Container Bind*/
                /*
                int i = 1;
                var InvoceIds = item.InvoiceId.Split(',');
                foreach (var InvId in InvoceIds)
                {
                    lstContainer.Where(x => x.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.FromDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ToDate + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                }
                */
                /***************/
                /*
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 60px;'>SR No.</th>");
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
                */
                /*Charges Bind*/
                /*
                foreach (var InvId in InvoceIds)
                {
                    lstCharges.Where(y => y.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(data =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Rate.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Total.ToString("0") + "</td></tr>");
                        i = i + 1;
                    });
                }
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; width: 120px;'>Total :</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; center; width: 144px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 90px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 160px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalIGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 120px;'>" + item.InvoiceAmt.ToString("0") + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("</td></tr></tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Mode</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Drawee Bank</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Instrument No</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                });
                html.Append("</tbody></table>");
                */
                /***************/



                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");

                //html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                //html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='9' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td><td colspan='3' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Folio No : </label><span>" + item.PartyCode + "</span></td></tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address : </label><span>" + item.PartyAddress + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [NonAction]
        public string GenerateTDSDepositReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                /*
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + item.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                */
                /*************/
                /*Container Bind*/
                /*
                int i = 1;
                var InvoceIds = item.InvoiceId.Split(',');
                foreach (var InvId in InvoceIds)
                {
                    lstContainer.Where(x => x.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.FromDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ToDate + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                }
                */
                /***************/
                /*
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 60px;'>SR No.</th>");
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
                */
                /*Charges Bind*/
                /*
                foreach (var InvId in InvoceIds)
                {
                    lstCharges.Where(y => y.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(data =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Rate.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Total.ToString("0") + "</td></tr>");
                        i = i + 1;
                    });
                }
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; width: 120px;'>Total :</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; center; width: 144px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 90px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 160px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalIGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 120px;'>" + item.InvoiceAmt.ToString("0") + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("</td></tr></tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Mode</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Drawee Bank</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Instrument No</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                });
                html.Append("</tbody></table>");
                */
                /***************/



                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");

                //html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                //html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Certificate No.</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [NonAction]
        public string GenerateBulkSDReceiptReport(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                /*
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + item.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                */
                /*************/
                /*Container Bind*/
                /*
                int i = 1;
                var InvoceIds = item.InvoiceId.Split(',');
                foreach (var InvId in InvoceIds)
                {
                    lstContainer.Where(x => x.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.FromDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ToDate + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                }
                */
                /***************/
                /*
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 60px;'>SR No.</th>");
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
                */
                /*Charges Bind*/
                /*
                foreach (var InvId in InvoceIds)
                {
                    lstCharges.Where(y => y.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(data =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Rate.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Total.ToString("0") + "</td></tr>");
                        i = i + 1;
                    });
                }
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; width: 120px;'>Total :</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; center; width: 144px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 90px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 160px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalIGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 120px;'>" + item.InvoiceAmt.ToString("0") + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("</td></tr></tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Mode</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Drawee Bank</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Instrument No</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                });
                html.Append("</tbody></table>");
                */
                /***************/



                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>SD RECEIPT</label>");
                html.Append("</td></tr>");

                //html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                //html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                //html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='9' width='80%' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td colspan='3' width='20%' style='font-size: 13px; line-height: 26px; float:right;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                if (item.InvoiceNo.ToString() != "")
                {
                    var InvoiceIds = item.InvoiceId.Split(',');
                    var InvoiceNos = item.InvoiceNo.Split(',');
                    var InvoiceAmts = item.Amt.Split(',');
                    i = 0;
                    foreach (var Invoice in InvoiceNos)
                    {
                        html.Append("<tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                        html.Append("</tr>");

                        i = i + 1;
                    }
                }


                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 80px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 100px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop
                decimal totalpaymentreceiptAmt = 0;
                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                    totalpaymentreceiptAmt = totalpaymentreceiptAmt + data.Amount;

                    i = i + 1;

                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>SD Balance</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total Payment Receipt Amount</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + totalpaymentreceiptAmt + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(totalpaymentreceiptAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetChequeBounceReceiptReport(String ReceiptNo)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.GetChequeBounceCashreceipt(ReceiptNo);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulk(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }


        [NonAction]
        public string GenerateBulk(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            int i = 0;
            lstInvoice.ToList().ForEach(item =>
            {
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                /*
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Receipt Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + item.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td>");
                html.Append("</tr></tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                */
                /*************/
                /*Container Bind*/
                /*
                int i = 1;
                var InvoceIds = item.InvoiceId.Split(',');
                foreach (var InvId in InvoceIds)
                {
                    lstContainer.Where(x => x.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.FromDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ToDate + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                }
                */
                /***************/
                /*
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 60px;'>SR No.</th>");
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
                */
                /*Charges Bind*/
                /*
                foreach (var InvId in InvoceIds)
                {
                    lstCharges.Where(y => y.InoviceId == Convert.ToInt32(InvId)).ToList().ForEach(data =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Rate.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Total.ToString("0") + "</td></tr>");
                        i = i + 1;
                    });
                }
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; width: 120px;'>Total :</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; center; width: 144px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 90px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 160px;'>" + item.TotalTaxable.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalCGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalSGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 210px;'>" + item.TotalIGST.ToString("0") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 10px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: right; width: 120px;'>" + item.InvoiceAmt.ToString("0") + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='12'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("</td></tr></tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%; border: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Mode</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Drawee Bank</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Instrument No</th>");
                html.Append("<th style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Amount</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; border-right: 1px solid #000; font-size: 12px; text-align: left; line-height: 15px;'>" + data.Amount + "</td>");
                    html.Append("</tr>");
                });
                html.Append("</tbody></table>");
                */
                /***************/



                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '>");
                html.Append("<tbody><tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br/>");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
                html.Append("<h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Cash Receipt</h2> </td></tr>");

                //Header
                html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr></tbody></table>");
                html.Append("</td></tr><tr><td><hr/></td></tr><tr><td>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:50%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                var InvoiceIds = item.InvoiceId.Split(',');
                var InvoiceNos = item.InvoiceNo.Split(',');
                var InvoiceAmts = item.Amt.Split(',');
                i = 0;
                foreach (var Invoice in InvoiceNos)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceNos[i] + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + InvoiceAmts[i] + "</td>");
                    html.Append("</tr>");

                    i = i + 1;
                }

                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
                html.Append("<thead>");
                html.Append("<tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Mode</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Drawee Bank</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Instrument</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Date</th>");
                html.Append("</tr></thead><tbody>");

                //loop

                i = 1;
                lstMode.Where(z => z.CashReceiptId == item.CashReceiptId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.PayMode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.DraweeBank + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.InstrumentNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Date + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + data.Amount + "</td>");
                    html.Append("</tr>");

                    i = i + 1;
                });

                //TDS
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>TDS</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TdsAmount.ToString() + "</td></tr>");


                //Total
                html.Append("<tr>");
                html.Append("<td colspan='4' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: right; font-weight: bold;'>Total</td>");
                html.Append("<td colspan='1' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.InvoiceAmt.ToString() + "</td></tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");
                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 80px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");
                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<th style='width:60%;'></th>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");


                lstSB.Add(html.ToString());
            });
            var type = "bulkreport";
            var id = "BulkReceipt" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
            var fileName = id + ".pdf";
            string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(PdfDirectory + fileName, lstSB);
            }
            return "/Docs/" + type + "/" + fileName;
        }
        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult ListOfReceiptDateWise(string FromDate, string ToDate)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetReceiptList(FromDate, ToDate);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DailyCashBookReport
        [HttpGet]
        public ActionResult DailyCashBookReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDailyCashBookReport(string PeriodFrom, string PeriodTo)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            List<Wlj_DailyCashBook> LstDailyCashBook = new List<Wlj_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBook(PeriodFrom, PeriodTo);
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<Wlj_DailyCashBook>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                LstDailyCashBook.Add(new Wlj_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = LstDailyCashBook.ToList().Sum(o => o.Area),
                    STO = LstDailyCashBook.ToList().Sum(o => o.STO),
                    INS = LstDailyCashBook.ToList().Sum(o => o.INS),
                    GRE = LstDailyCashBook.ToList().Sum(o => o.GRE),
                    GRL = LstDailyCashBook.ToList().Sum(o => o.GRL),
                    Reefer = LstDailyCashBook.ToList().Sum(o => o.Reefer),
                    EscCharge = LstDailyCashBook.ToList().Sum(o => o.EscCharge),
                    Print = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    Royality = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    HT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.HT)),
                    EGM = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    Documentation = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Taxable)),
                    Cgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Igst)),
                    Roundoff = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    Tds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CrTds))

                });
                ObjRR.DBResponse.Data = LstDailyCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateDailyCashBookReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "DailyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                Pages[0] = Pages[0].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                //using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f))
                //{

                //    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                //}
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/DailyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }





        #endregion


        #region MonthlyCashBookReport



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult MonthlyCashBookReport()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //Kdl_ReportRepository ObjReport = new Kdl_ReportRepository();
            //ObjReport.GetDistricts(ObjLogin.Uid);
            //if (ObjReport.DBResponse.Data != null)
            //{
            //    var JObj = JObject.Parse(JsonConvert.SerializeObject(ObjReport.DBResponse.Data));
            //    ViewBag.DistList = new SelectList(JObj["Dist"], "DistrictId", "DistrictName");
            //}

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMonthlyCashBookReport(MonthlyCashBook ObjMonthlyCashBook)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            List<Wlj_DailyCashBook> LstDailyCashBook = new List<Wlj_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthlyCashBook(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {

                Wlj_MonthlyCashBook WljCashBook = new Wlj_MonthlyCashBook();
                WljCashBook = (Wlj_MonthlyCashBook)ObjRR.DBResponse.Data;

                //  LstDailyCashBook = (List<Wlj_DailyCashBook>)ObjRR.DBResponse.Data;
                WljCashBook.lstCashReceipt = WljCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                WljCashBook.lstCashReceipt = WljCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                WljCashBook.lstCashReceipt.Add(new Wlj_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = WljCashBook.lstCashReceipt.ToList().Sum(o => o.Area),
                    STO = WljCashBook.lstCashReceipt.ToList().Sum(o => o.STO),
                    INS = WljCashBook.lstCashReceipt.ToList().Sum(o => o.INS),
                    GRE = WljCashBook.lstCashReceipt.ToList().Sum(o => o.GRE),
                    GRL = WljCashBook.lstCashReceipt.ToList().Sum(o => o.GRL),
                    Reefer = WljCashBook.lstCashReceipt.ToList().Sum(o => o.Reefer),
                    EscCharge = WljCashBook.lstCashReceipt.ToList().Sum(o => o.EscCharge),
                    Print = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    Royality = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    HT = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.HT)),
                    EGM = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    Documentation = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = WljCashBook.lstCashReceipt.ToList().Sum(o => Math.Ceiling(Convert.ToDecimal(o.Taxable))),
                    Cgst = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = WljCashBook.lstCashReceipt.ToList().Sum(o => Math.Ceiling(Convert.ToDecimal(o.Igst))),
                    Roundoff = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    TotalSD = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalSD)),
                    OpCash = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpCash)),
                    OpChq = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpChq)),
                    cloCash = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.cloCash)),
                    clochq = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.clochq)),

                    Tds = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = WljCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.CrTds))

                });
                ObjRR.DBResponse.Data = WljCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateMonthlyCashBookPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "MonthlyCashBookReport.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/MonthlyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 20f, 20f, 20f, true))
                {
                    ObjPdf.HeadOffice = "";
                    ObjPdf.HOAddress = "";
                    ObjPdf.ZonalOffice = "";
                    ObjPdf.ZOAddress = "";
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/MonthlyCashBookReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }





        #endregion


        #region Time Barred Report

        public ActionResult TimeBarredReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLiveBondReport(string AsOnDate)
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetLiveBondReport(AsOnDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<Wlj_TimeBarredReport> lstData = new List<Wlj_TimeBarredReport>();
                lstData = (List<Wlj_TimeBarredReport>)objRR.DBResponse.Data;
                Path = GenerateLiveBondReport(lstData, AsOnDate);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }

        [NonAction]
        public string GenerateLiveBondReport(List<Wlj_TimeBarredReport> lstData, string AsOnDate)
        {

            try
            {
                var FileName = "LiveBondReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                decimal Value = 0, Duty = 0, Total = 0;

                DateTime date = Convert.ToDateTime(AsOnDate);
                int year = date.Year;
                int nextYear = year + 1;
                string nextYr = Convert.ToString(nextYear);
                string nYr = nextYr.Substring(nextYr.Length - 2);


                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='80%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 14px; padding-bottom: 10px;'>H-22, M I D C Waluj,</span><br/><span style='font-size: 14px; padding-bottom: 10px;'>Opposite Nilason Corporate, Aurangabad, Maharashtra -431136</span><br/><label style='font-size: 14px;'>Email - rmmum@cewacor.nic.in</label>");
                Pages.Append("<br /><label style='font-size: 16px; font-weight:bold;'>STATEMENT SHOWING LIVE BONDS AS ON : " + AsOnDate + "</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("<tr><td><br/><br/></td></tr>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><th colspan='6' width='50%' style='text-align: left; font-size:12px;'>NO.CWC/CFS-MVM/Bond-staff/" + year + "-" + nYr + "/</th> <th colspan='6' width='50%' style='text-align: right; font-size:12px;'> Date :" + AsOnDate + "</th></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'><thead>");
                Pages.Append("<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>SL. NO</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>NAME OF THE IMPORTER</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Name Of the CHA</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Bond No. & Date</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Name Of The Cargo</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Units</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>CIF VALUE</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Duty</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Total</th><th style='border-bottom:1px solid #000;'>Area Occupied (SQM)</th></tr>");
                Pages.Append("</thead>");
                Pages.Append("<tbody>");

                lstData.ForEach(item =>
                {

                    Value = item.Value;
                    Duty = item.Duty;
                    Total = Math.Round((Value + Duty), 2);


                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Importer + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CHA + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondNo + " / " + item.BondDate + "</td>");
                    // Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ItemDesc + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Units + "</td>");
                    //Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Weight + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Value + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Duty + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + Total + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;'>" + item.Area + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='5' style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:left;'>Total :</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Units) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Value) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Duty) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + Convert.ToDecimal(lstData.Sum(x => x.Value) + lstData.Sum(x => x.Duty)) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Area).ToString("0.00") + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; font-size:9pt;'><tbody>");
                Pages.Append("<tr><td><br/><br/><br/></td></tr>");
                Pages.Append("<tr><td colspan='8' width='80%'></td>");
                Pages.Append("<td colspan='4' width='20%' style='text-align: center;'>MANAGER <br/> CFS. Waluj</td></tr>");
                Pages.Append("<tr><td><br/><br/><br/></td></tr>");
                Pages.Append("<tr><td colspan='8' width='80%'>Submitted to: <br/> The Regional Manager, <br/> Central Warehousing Corporation, <br/> Regional Office, Maharashtra -431136</td>");
                Pages.Append("<td colspan='4' width='20%'></td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }



        [HttpPost]
        public JsonResult GetTimeBarredReport(string AsOnDate)
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetTimeBarredReport(AsOnDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<Wlj_TimeBarredReport> lstData = new List<Wlj_TimeBarredReport>();
                lstData = (List<Wlj_TimeBarredReport>)objRR.DBResponse.Data;
                Path = GenerateTimeBarredReport(lstData, AsOnDate);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }

        [NonAction]
        public string GenerateTimeBarredReport(List<Wlj_TimeBarredReport> lstData, string AsOnDate)
        {

            try
            {
                var FileName = "TimeBarredReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                decimal Value = 0, Duty = 0, Total = 0;


                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='80%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 14px; padding-bottom: 10px;'>H-22, M I D C Waluj,</span><br/><span style='font-size: 14px; padding-bottom: 10px;'>Opposite Nilason Corporate, Aurangabad, Maharashtra -431136</span><br/><label style='font-size: 14px;'>Email - rmmum@cewacor.nic.in</label>");
                Pages.Append("<br /><label style='font-size: 16px; font-weight:bold;'>STATEMENT SHOWING TIME BARRED BONDS AS ON : " + AsOnDate + "</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("<tr><th colspan='12' style='text-align: right; font-size:12px;'> Date :" + AsOnDate + "</th></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'><thead>");
                Pages.Append("<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>SL. NO</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>NAME OF THE IMPORTER</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Name Of the CHA</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Bond No. & Date</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Name Of The Cargo</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Units</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>CIF VALUE</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Duty</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>Total</th><th style='border-bottom:1px solid #000;'>Area Occupied (SQM)</th></tr>");
                Pages.Append("</thead>");
                Pages.Append("<tbody>");

                lstData.ForEach(item =>
                {

                    Value = item.Value;
                    Duty = item.Duty;
                    Total = Math.Round((Value + Duty), 2);


                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Importer + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CHA + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondNo + " / " + item.BondDate + "</td>");
                    // Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ItemDesc + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Units + "</td>");
                    //Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Weight + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Value + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Duty + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + Total + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;'>" + item.Area + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='5' style='border-bottom:1px solid #000;border-right:1px solid #000;text-align:left;'>Total :</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Units) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Value) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Duty) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + Convert.ToDecimal(lstData.Sum(x => x.Value) + lstData.Sum(x => x.Duty)) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Area).ToString("0.00") + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; font-size:9pt;'><tbody>");
                Pages.Append("<tr><td><br/><br/><br/></td></tr>");
                Pages.Append("<tr><td colspan='8' width='80%'></td>");
                Pages.Append("<td colspan='4' width='20%' style='text-align: center;'>MANAGER <br/> CFS. Waluj</td></tr>");
                Pages.Append("<tr><td><br/><br/><br/></td></tr>");
                Pages.Append("<tr><td colspan='8' width='80%'>Submitted to: <br/> The Regional Manager, <br/> Central Warehousing Corporation, <br/> Regional Office, Maharashtra -431136</td>");
                Pages.Append("<td colspan='4' width='20%'></td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region-- WEEKLY REPORT --

        [HttpGet]
        public ActionResult WeeklyVCReport()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetVCDetails(string date1, string date2)
        {
            try
            {
                var dt1 = DateTime.ParseExact(date1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var dt2 = DateTime.ParseExact(date2, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                /*if (dt2.DayOfWeek.ToString().ToLower() != System.Configuration.ConfigurationManager.AppSettings["VCReportDay"].ToString().ToLower())
                {
                    return Json(new { Status = 2, Message = "Report Only Show on " + System.Configuration.ConfigurationManager.AppSettings["VCReportDay"].ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                {*/



                if (dt1 > dt2)
                {
                    return Json(new { Status = 2, Message = "To Date should be greater than From Date " }, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    var doa = Enumerable.Range(0, 1 + dt2.Subtract(dt1).Days).Select(offset => dt1.AddDays(offset)).ToArray();
                    IList<string> formatted = new List<string>();
                    doa.ToList().ForEach(item =>
                    {
                        formatted.Add(item.ToString("yyyyMMdd"));
                    });
                    var XMLText = Utility.CreateXML(formatted);
                    var reportRepo = new Wlj_ReportRepository();
                    reportRepo.VCCapacityDetails(XMLText, date1, date2);

                    return Json(new { Status = 1, Message = "Data Detected", Data = reportRepo.DBResponse.Data }, JsonRequestBehavior.AllowGet);
                    /*}*/
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = -1, Message = "Internal Error Occurs !!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult WeeklyVCReport(FormCollection fc)
        {
            var date = Convert.ToDateTime(fc["VCTdate"].ToString());
            var cfscapopen = Convert.ToDecimal(fc["TWcfsopen"].ToString());
            var cfscapcovered = Convert.ToDecimal(fc["TWcfscov"].ToString());
            var bndcapcovered = Convert.ToDecimal(fc["TWbndcov"].ToString());
            var total1 = Convert.ToDecimal(fc["Totalcap"].ToString());
            var cfsutilopen = Convert.ToDecimal(fc["TWcfsopenutl"].ToString());
            var cfsutilcover = Convert.ToDecimal(fc["hdncfsutil"].ToString());
            var bndutilcover = Convert.ToDecimal(fc["hdnbndutil"].ToString());
            var total2 = Convert.ToDecimal(fc["Totalutil"].ToString());
            var reportRepo = new Wlj_ReportRepository();
            reportRepo.AddVCDetails(date, cfscapopen, cfscapcovered, bndcapcovered, total1, cfsutilopen, cfsutilcover, bndutilcover, total2);

            return Json(new { Status = 1, Message = "VC Report Saved Succesfully", Data = reportRepo.DBResponse.Data }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GeneratePDF(FormCollection fc)
        {
            try
            {
                var pages = new string[2];
                var type = fc["type"].ToString();
                var id = fc["id"].ToString();
                pages[0] = fc["page"].ToString();
                if (id == "")
                {
                    //string ad = "AB1CE12354F";
                    //Random rnd = new Random();
                    //id = new string(Enumerable.Repeat(ad, ad.Length).Select(s => s[rnd.Next(ad.Length)]).ToArray());
                    id = ((Login)(Session["LoginUser"])).Uid.ToString() + "" + DateTime.Now.ToString().Replace('/', '_').Replace(' ', '_').Replace(':', '_');
                }
                id = id.Replace('/', '_');
                var fileName = id + ".pdf";
                //  var fileName = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
                string PdfDirectory = Server.MapPath("~/Docs") + "/" + type + "/";
                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                //using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
                //{
                //    rh.GeneratePDF(PdfDirectory + fileName, pages);
                //}

                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(PdfDirectory + fileName, pages);

                }
                return Json(new { Status = 1, Message = "/Docs/" + type + "/" + fileName }, JsonRequestBehavior.DenyGet);// Data = fileName 
            }
            catch (Exception ex)
            {
                return Json(new { Status = "1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion


        #region ECONOMY REPORT
        public ActionResult EconomyReport()
        {
            return PartialView();
        }

        public JsonResult GetEconomyReport(int Month, int Year)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            ObjRR.GetEconomyReport(Month, Year);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEconomyReport(int Month, int Year, List<EconomyReport> lstRptData)
        {
            try
            {
                Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();

                decimal result;
                List<EconomyReport> lst = new List<EconomyReport>();
                lstRptData.ForEach(item =>
                {
                    //  if (decimal.TryParse(item.Amount, out result))
                    // {
                    lst.Add(item);
                    //}
                });

                string xml = "";
                if (lst != null)
                {
                    xml = Utility.CreateXML(lst);
                }

                ObjRR.SaveEconomyReport(Month, Year, xml, ((Login)(Session["LoginUser"])).Uid);

                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Status = -1, Message = "Internal Error Occurs !!", Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EconomyReportPrint(int Month, int Year)
        {
            try
            {
                Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
                ObjRR.GetEconomyReportForPrint(Month, Year);
                if (ObjRR.DBResponse.Status == 1)
                {
                    return Json(new { Status = 1, Message = "Data Detected", Data = GeneratingPDFforEcoRpt((EconomyRptPrint)ObjRR.DBResponse.Data) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = -1, Message = ObjRR.DBResponse.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { Status = -1, Message = "Internal Error Occurs !!" }, JsonRequestBehavior.AllowGet);
            }
        }
        [NonAction]
        public string GeneratingPDFforEcoRpt(EconomyRptPrint data)
        {

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();
            StringBuilder html2 = new StringBuilder();
            StringBuilder html3 = new StringBuilder();
            StringBuilder html4 = new StringBuilder();
            StringBuilder html5 = new StringBuilder();
            //html.Append("<h1>Test</h1>");
            //Page 1*********************************************************************************************************************
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif;'><thead>");
            html.Append("<tr><td colspan='12'>");
            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%;'><tbody>");
            html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            html.Append("<td width='80%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
            html.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='font-size: 14px; padding-bottom: 10px;'>H-22, M I D C Waluj,</span><br/><span style='font-size: 14px; padding-bottom: 10px;'>Opposite Nilason Corporate, Aurangabad, Maharashtra -431136</span><br/><label style='font-size: 14px;'>Email - rmmum@cewacor.nic.in</label><br/>");

            //    html.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Patparganj-Delhi</span><br/>");
            html.Append("<label style='font-size: 14px; font-weight:bold;'>ECONOMY REPORT FOR THE PERIOD OF " + Convert.ToDateTime(data.FormDate).ToString("dd-MMM-yyyy") + " TO " + Convert.ToDateTime(data.ToDate).ToString("dd-MMM-yyyy") + "</label></td></tr>");
            html.Append("<tr><th><br/><br/></th></tr>");
            html.Append("<tr><th colspan='12' style='text-align: right; font-size:8pt;'>DATED : " + Convert.ToDateTime(data.CreatedOn).ToString("dd-MMM-yyyy") + "</th></tr>");
            html.Append("</tbody></table>");
            html.Append("</td></tr>");
            html.Append("</thead></table>");

            html.Append("<table style='width:100%; margin-bottom:20px; font-size: 8pt;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead>");
            html.Append("<tr><th cellpadding='5' colspan='12' style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 9pt; text-align: center;'>COVERED AREA AND ITS CAPACITY</th></tr>");
            html.Append("<tr><th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Gross in <br/> Sq.Mtrs</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Net(Usable) in Sq.Mtrs</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Grids/Slot(Nos)</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Std.Bags(Nos)</th>");
            html.Append("</tr>");
            html.Append("</thead>");

            html.Append("<tbody>");
            html.Append("<tr><td colspan='3' style='padding:15px 5px;'>" + data.SqmCovered + "</td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'></td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'></td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'>" + data.BagCovered + "</td></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%; margin-bottom:20px; font-size: 8pt;' cellspacing='0' cellpadding='5'>");
            html.Append("<thead>");
            html.Append("<tr><th colspan='12' style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 9pt; text-align: center;'>OPEN AREA AND ITS CAPACITY</th></tr>");
            html.Append("<tr><th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Gross in <br/> Sq.Mtrs</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Net(Usable) in Sq.Mtrs</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>TEU's(Nos.)</th>");
            html.Append("<th colspan='3' style='border-bottom: 1px solid #000; text-align:left;'>Std.Bags(Nos)</th>");
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            html.Append("<tr><td colspan='3' style='padding:15px 5px;'>" + data.SqmOpen + "</td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'></td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'></td>");
            html.Append("<td colspan='3' style='padding:15px 5px;'>" + data.BagOpen + "</td></tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:9pt;'><tbody>");
            html.Append("<tr><td><br/><br/><br/><br/></td></tr>");
            html.Append("<tr><td colspan='12' style='text-align: left;'>Submmited to:- <br/> <b>The Regional Manager</b> <br/> CWC, RO, DELHI</td></tr>");
            html.Append("</tbody></table>");

            //html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            lstSB.Add(html.ToString());
            //Page 2*********************************************************************************************************************

            html2.Append("<table style='border-top: 1px solid #000;width:100%; font-size:8pt;' cellspacing='0' cellpadding='5'>");
            html2.Append("<thead>");
            html2.Append("<tr><th style='border-bottom: 1px solid #000; text-align:left; width:50%;'>Head of Income</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>Amount <br/> RS   P.</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>Cumulative <br/> Since 1st April</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>Progressive Cumulative <br/> RS   P.</th></tr>");

            html2.Append("<tr><th style='border-bottom: 1px solid #000; text-align:left; width:50%;'>1</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>2</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>3</th>");
            html2.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:17%;'>4</th></tr>");
            html2.Append("</thead>");
            html2.Append("<tbody>");

            data.RptDetails.Where(x => x.ItemType == "I" && x.PageNo == 2).ToList().ForEach(i =>
            {
                html2.Append("<tr><pre><td cellpadding='5'>" + i.ItemLabel + "</td>");
                html2.Append("<td cellpadding='5'>" + i.Amount + "</td>");
                html2.Append("<td cellpadding='5'>" + i.CumAmount + "</td>");
                html2.Append("<td cellpadding='5'>" + i.ProCumAmount + "</td></pre></tr>");
            });
            html2.Append("</tbody>");
            html2.Append("</table>");

            lstSB.Add(html2.ToString());

            //Page 3**********************************************************************************************************************          
            html3.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'>");
            html3.Append("<thead>");
            html3.Append("<tr><th style='border-top:1px solid #000; border-bottom: 1px solid #000;text-align:left; width:50%;''>1</th>");
            html3.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:17%;'>2</th>");
            html3.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:17%;'>3</th>");
            html3.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:17%;'>4</th></tr>");
            html3.Append("</thead>");
            html3.Append("<tbody>");

            data.RptDetails.Where(x => x.ItemType == "I" && (x.PageNo == 3 || x.PageNo == 4)).ToList().ForEach(i =>
            {
                html3.Append("<tr><td cellpadding='4'><pre>" + i.ItemLabel + "</pre></td>");
                html3.Append("<td cellpadding='4'>" + i.Amount + "</td>");
                html3.Append("<td cellpadding='4'>" + i.CumAmount + "</td>");
                html3.Append("<td cellpadding='4'>" + i.ProCumAmount + "</td></tr>");
            });
            html3.Append("</tbody>");
            html3.Append("</table>");
            lstSB.Add(html3.ToString());

            //Page 4**********************************************************************************************************************            
            html4.Append("<table style='width:100%; font-size: 8pt;' cellspacing='0' cellpadding='5'>");
            html4.Append("<thead>");
            html4.Append("<tr><th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:8%;'>Code No</th>");
            html4.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:47%;'>Head of Expenses</th>");
            html4.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Amount <br/> RS   P.</th>");
            html4.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Cumulative <br/> Since 1st April</th>");
            html4.Append("<th style='border-top:1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Progressive Cumulative <br/> RS   P.</th></tr>");

            html4.Append("<tr><th style='border-bottom: 1px solid #000; text-align:left; width:8%;'>5</th>");
            html4.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:47%;'>6</th>");
            html4.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:15%;'>7</th>");
            html4.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:15%;'>8</th>");
            html4.Append("<th style='border-bottom: 1px solid #000; text-align:left; width:15%;'>9</th></tr>");
            html4.Append("</thead>");
            html4.Append("<tbody>");

            int maxSr = data.RptDetails.Where(x => x.ItemType == "E").Max(x => x.Sr);
            data.RptDetails.Where(x => x.ItemType == "E" && x.PageNo == 4).ToList().ForEach(i =>
            {

                html4.Append("<tr>");
                if (i.Sr == maxSr)
                {
                    html4.Append("<td cellpadding='5'></td>");
                    html4.Append("<td cellpadding='5' >" + i.ItemLabel + "</td>");
                    //html4.Append("<td style='font-size: 9pt; padding:10px;'>" + i.ItemLabel + "</td>");
                }
                else
                {
                    html4.Append("<td cellpadding='5'>" + i.ItemCodeNo + "</td>");
                    html4.Append("<td cellpadding='5'>" + i.ItemLabel + "</td>");
                }
                html4.Append("<td cellpadding='5'>" + i.Amount + "</td>");
                html4.Append("<td cellpadding='5'>" + i.CumAmount + "</td>");
                html4.Append("<td cellpadding='5'>" + i.ProCumAmount + "</td></tr>");
            });

            html4.Append("</tbody>");
            /*html4.Append("<tfoot>");
            html4.Append("<tr><th colspan='2' style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Excess of Income over Expenditure</th>");
            html4.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th>");
            html4.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th>");
            html4.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th></tr>");
            html4.Append("</tfoot>");*/
            html4.Append("</table>");
            lstSB.Add(html4.ToString());

            //Page last**********************************************************************************************************************    

            html5.Append("<table style='width:100%; font-size:8pt;' cellspacing='0' cellpadding='5'>");
            html5.Append("<thead><tr><th colspan='12' style='text-align: left; font-size:9pt;'>Summary of Financial Performance for the period of " + Convert.ToDateTime(data.FormDate).ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(data.ToDate).ToString("dd-MMM-yyyy") + "</th></tr>");
            html5.Append("<tr><th style='border-top: 1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Code No</th>");
            html5.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; text-align:left; width:40%;'>Head of Income</th>");
            html5.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Amount <br /> RS   P.</th>");
            html5.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Cumulative <br /> Since 1st April</th>");
            html5.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; text-align:left; width:15%;'>Progressive Cumulative <br /> RS   P.</th></tr>");
            html5.Append("</thead>");
            html5.Append("<tbody>");
            data.RptSummary.ForEach(i =>
            {
                html5.Append("<tr><td cellpadding='5'>" + i.CodeNo + "</td>");
                html5.Append("<td cellpadding='5'><pre>" + i.ItemLabel + "</pre></td>");
                html5.Append("<td cellpadding='5'>" + i.Amount + "</td>");
                html5.Append("<td cellpadding='5'>" + i.CumAmount + "</td>");
                html5.Append("<td cellpadding='5'>" + i.ProCumAmount + "</td></tr>");
            });
            html5.Append("</tbody>");
            /*html5.Append("<tfoot>");
            html5.Append("<tr><th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'></th>");
            html5.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Total(A+B)</th>");
            html5.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th>");
            html5.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th>");
            html5.Append("<th style='border-top:1px solid #000; font-size: 11pt; padding:10px; text-align:left;'>Dynamic</th></tr>");
            html5.Append("</tfoot>");*/
            html5.Append("</table>");

            html5.Append("<table style='border-top:1px solid #000; width:100%;' cellspacing='0' cellpadding='10'>");
            html5.Append("<tbody>");
            html5.Append("<tr><td width='3%' align='right' style='font-size:8pt;'>1.</td><td colspan='2' width='85%' style='font-size:9pt;'>Indicate total number of time barred Bond and income involved(with vearwise break-up)</td></tr>");
            html5.Append("<tr><td width='3%' align='right' style='font-size:8pt;'>2.</td><td colspan='2' width='85%' style='font-size:9pt;'>Number of contaiers seized abandanded and their income involved(with vearwise break-up)</td></tr>");
            html5.Append("</tbody>");
            html5.Append("</table>");

            lstSB.Add(html5.ToString());

            var FileName = "EcoReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName;
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            /*if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }*/

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 40f, 20f, 20f, false, true))
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
        #region SAC Register
        [HttpGet]
        public ActionResult SACRegister()
        {
            return PartialView("SACReport");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetSACRegister(Wlj_SAC objSAC)
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetSACDetails(objSAC.PeriodFrom, objSAC.PeriodTo);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<Wlj_SACRegister> lstSAC = new List<Wlj_SACRegister>();
                lstSAC = (List<Wlj_SACRegister>)objRR.DBResponse.Data;
                Path = GetSACHtml(lstSAC);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });
        }
        [NonAction]
        public string GetSACHtml(List<Wlj_SACRegister> lstSAC)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            sb.Append("<tr><td colspan='4'>");
            sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            sb.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
            sb.Append("<td width='10%' align='right'>");
            sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
            sb.Append("<tr><td style='border:1px solid #333;'>");
            sb.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>Document No.F/CD/CFS/24</div>");
            sb.Append("</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");

            sb.Append("<tr><td colspan='4'>");
            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            sb.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:14px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:14px;'>H-22, M I D C Waluj, Aurangabad, <br/> Maharashtra - 431136</span><br/><label style='font-size: 14px;'>Email -  rmmum@cewacor.nic.in</label><br/><label style='font-size: 16px; font-weight:bold;'>SAC REGISTER</label></td>");
            sb.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td></tr>");
            sb.Append("</tbody></table>");

            sb.Append("<table cellspacing='0' cellpadding='5' style='border:1px solid #000; border-bottom:none; font-size:8pt; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'>");
            sb.Append("<thead><tr>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>S No.</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>SAC No & Date</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Date upto which valid</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Name of the Importer</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Name of CHA</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>custom Seal No.</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>BOL/AWB No.</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>BOE No. & Date</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Description of Cargo</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>No of Units</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Space allotted in sq. mtrs(Gross or Net)</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Advanced Charges Collected (Rs.)</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>C.R No. & Date</th>");
            sb.Append("<th style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>Signature Of the Office Assisatant</th>");
            sb.Append("<th style='border-bottom:1px solid #000; text-align:center;'>Remarks</th>");
            sb.Append("</tr></thead>");
            sb.Append("<tbody>");
            int i = 1;
            lstSAC.ForEach(item =>
            {
                sb.Append("<tr>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + i + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.SacNo + " " + item.SacDate + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.ValidUpto + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.IMPName + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.CHAName + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.SealNo + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.BOLAWBNo + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.BOENo + " " + item.BOEDate + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.CargoDescription + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.NoOfUnits + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.AreaReserved + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.InvoiceAmt + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'>" + item.ReceiptNo + " " + item.ReceiptDate + "</td>");
                sb.Append("<td style='border-bottom:1px solid #000; border-right:1px solid #000; text-align:center;'></td>");
                sb.Append("<td style='border-bottom:1px solid #000; text-align:center;'>" + item.Remarks + "</td>");
                sb.Append("</tr>");
                i++;
            });
            sb.Append("</tbody></table>");

            sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            sb.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));

            string FileName = "SacRegister.pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName))
            {
                System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName);
            }
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                RH.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName, sb.ToString());
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        #endregion

        #region Bond Register
        [HttpGet]  
        public ActionResult BondRegister()
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetBondNo();
            if (objRR.DBResponse.Data != null)
                ViewBag.GetBondNo = objRR.DBResponse.Data;
            else
                ViewBag.GetBondNo = null;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetBondRegister(int DepositeAppId, string FromDate, string ToDate)
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetBondRegister(DepositeAppId, FromDate, ToDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                Wlj_BondRegister objBR = new Wlj_BondRegister();
                objBR = (Wlj_BondRegister)objRR.DBResponse.Data;
                Path = GetBondHtml(objBR, DepositeAppId);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });
        }
        [NonAction]
        public string GetBondHtml(Wlj_BondRegister objBR, int DepositeAppId)
        {
            List<string> lstSB = new List<string>();
            objBR.lstSACDetails.ForEach(item =>
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                sb.Append("<tr><td width='65%' style='font-size: 10px;'>'Warehousing for every one'</td>");
                sb.Append("<td width='4%' align='right'>");
                sb.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                sb.Append("<tr><td style='border:1px solid #333;'>");
                sb.Append("<div style='padding: 5px 0; font-size: 12px; text-align: center;'>F/CD/26</div>");
                sb.Append("</td></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                sb.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                sb.Append("<td width='160%' valign='top' align='center'><h1 style='font-size: 30px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><span style='font-size:14px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:14px;'>" + objBR.lstSACDetails[0].CompanyAdd + "<br/>" + objBR.lstSACDetails[0].CompanyEmail + "</span><br/><label style='font-size: 14px; font-weight:bold;'>BOND REGISTER</label></td>");
                sb.Append("<td valign='top'><img align='right' src='ISO_IMG' width='100'/></td></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");

                string InvAmt = "", ReceiptNoandDate = "";
                objBR.lstAdvPayment.Where(y => y.SacNo == item.SacNo).ToList().ForEach(i =>
                {
                    if (i.InvoiceAmt > 0)
                    {
                        if (InvAmt == "")
                            InvAmt = i.InvoiceAmt.ToString();
                        else
                            InvAmt += ", " + i.InvoiceAmt.ToString();
                    }
                    if (i.ReceiptNo != "")
                    {
                        if (ReceiptNoandDate == "")
                            ReceiptNoandDate = i.ReceiptNo + " " + i.ReceiptDate;
                        else
                            ReceiptNoandDate += ", " + i.ReceiptNo + " " + i.ReceiptDate;
                    }
                });

                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table cellpadding='0' cellspacing='0' style='width:100%; font-size:9pt; font-family:Verdana,Arial,San-serif;'><tbody>");
                sb.Append("<tr><th cellpadding='5' align='left' colspan='8' width='70%'>Name & Address of Importer <span style='font-weight:normal;'><u>" + item.IMPName + " " + item.IMPAdd + "</u></span></th>");
                sb.Append("<th cellpadding='5' align='left' colspan='4' width='30%'>Godown No. / Stack No. <span style='font-weight:normal;'><u>" + item.GodownName + "</u></span></th></tr>");
                sb.Append("<tr><th cellpadding='5' align='left' colspan='8' width='70%'>Name & Address of CHA <span style='font-weight:normal;'><u>" + item.CHAName + " " + item.CHAdd + "</u></span></th>");
                sb.Append("<th cellpadding='5' align='left' colspan='4' width='30%'>Bond No. & Date <span style='font-weight:normal;'><u>" + item.BondNo + " - " + item.BondDate + "</u></span></th></tr>");
                sb.Append("<tr><th cellpadding='5' align='left' colspan='8' width='70%'>Sac No. & Date <span style='font-weight:normal;'><u>" + item.SacNo + " " + item.SacDate + "</u></span> Area Booked<span style='font-weight:normal;'><u>" + item.AreaReserved + "</u></span></th>");
                sb.Append("<th cellpadding='5' align='left' colspan='4' width='30%'>Expiry date of inital <br/> bond period <span style='font-weight:normal;'><u>" + item.ValidUpto + "</u></span></th></tr>");
                sb.Append("<tr><th cellpadding='5' align='left' colspan='8' width='70%'>Advance charges <br/> received of sac <span style='font-weight:normal;'><u>" + InvAmt + "</u></span>  CR No. & Date <span style='font-weight:normal;'><u>" + ReceiptNoandDate + "</u></span></th>");
                sb.Append("<th cellpadding='5' align='left' colspan='4' width='30%'>In-To Bond B.O.E. No. & Date <span style='font-weight:normal;'><u>" + item.BondBOENo + " " + item.BondBOEDate + "</u></span></th></tr>");
                sb.Append("<tr><th cellpadding='5' align='left' colspan='8' width='70%'></th>");
                sb.Append("<th cellpadding='5' align='left' colspan='4' width='30%'>I.G.M No. & Date <span style='font-weight:normal;'><u>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</u></span></th></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr>");

                sb.Append("<tr><td colspan='12'>");
                sb.Append("<table style='border:1px solid #000;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");

                sb.Append("<td colspan='4' width='33.33%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                sb.Append("<thead><tr><th colspan='12' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>RECEIPTS</th></tr>");
                sb.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                sb.Append("<tr><th width='5%' style='text-align:center;padding:10px;border-right:1px solid #000;'>Date</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>No. of Units</th> ");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>Weight <br/> (M.T.)</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>Area in sq. Mtrs. <br/>(Gross or Net)</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>CIF Value</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>Duty</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;'>Total Value</th>");
                sb.Append("<th width='15%' style='text-align:center;padding:10px;'>Condition of Packages</th></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></thead>");
                sb.Append("<tbody><tr><td cellspacing='0' cellpadding='0' colspan='12'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                if (objBR.lstUnloadingDetails.Where(y => y.DepositappId == item.DepositappId).ToList().Count > 0)
                {
                    objBR.lstUnloadingDetails.Where(y => y.DepositappId == item.DepositappId).ToList().ForEach(data =>
                    {
                        sb.Append("<tr><td width='5%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + data.UnloadedDate + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + data.UnloadedUnits.ToString() + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + (data.UnloadedWeights / 1000).ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + data.AreaOccupied.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + data.Value.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + data.Duty.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'>" + (data.Value + data.Duty).ToString("0.000") + "</td>");
                        sb.Append("<td width='15%' style='text-align:center;padding:10px;border-bottom:1px solid #000;'>" + data.PackageCondition + "</td></tr>");
                    });
                }
                else
                {
                    sb.Append("<tr><td width='5%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;padding:10px;border-right:1px solid #000;border-bottom:1px solid #000;'></td>");
                    sb.Append("<td width='15%' style='text-align:center;padding:10px;border-bottom:1px solid #000;'></td></tr>");

                }
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></tbody>");
                sb.Append("</table></td>");

                sb.Append("<td colspan='3' width='25%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                sb.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>ISSUES</th></tr>");
                sb.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                sb.Append("<tr><th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Ex-Bond BOE No & Date</th> ");
                sb.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No. of Units</th> ");
                sb.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Weight <br/> (M.T.)</th>");
                sb.Append("<th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Area released in sq. Mtrs. <br/>(Gross or Net)</th>");
                sb.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>CIF Value</th>");
                sb.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Duty</th>");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;'>Total Value</th></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></thead>");
                sb.Append("<tbody><tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                if (objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).ToList().Count > 0)
                {
                    objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).ToList().ForEach(j =>
                    {
                        sb.Append("<tr><td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + j.BondBOENo + " - " + j.BondBOEDate + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + j.Units.ToString("") + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + Convert.ToDecimal(j.Weight / 1000).ToString("0.000") + "</td>");
                        sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + j.SQM.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + j.Value.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + j.Duty.ToString("0.000") + "</td>");
                        sb.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + (j.Value + j.Duty).ToString("0.000") + "</td></tr>");
                    });
                }
                else
                {
                    sb.Append("<tr><td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");

                }
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></tbody>");
                sb.Append("</table></td>");

                sb.Append("<td colspan='1' width='10%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                sb.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>CHARGES REALISED</th></tr>");
                sb.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                sb.Append("<tr><th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Amount <br/> (Rs.)</th> ");
                sb.Append("<th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Period <br/> Upto</th> ");
                sb.Append("<th width='10%' style='text-align:center;padding:10px;'>CR. No. & Date</th></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></thead>");
                sb.Append("<tbody><tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                if (objBR.lstDeliveryPaymentDet.Where(m => m.DepositappId == item.DepositappId).ToList().Count > 0)
                {
                    objBR.lstDeliveryPaymentDet.Where(m => m.DepositappId == item.DepositappId).ToList().ForEach(i =>
                    {
                        sb.Append("<tr><td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + i.InvoiceAmt.ToString("0.000") + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + i.DeliveryDate + "</td> ");
                        sb.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'>" + i.ReceiptNo + " " + i.ReceiptDate + "</td></tr>");
                    });
                }
                else
                {
                    sb.Append("<tr><td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='10%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");
                }
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></tbody>");
                sb.Append("</table></td>");

                sb.Append("<td colspan='4' width='33.33333333333333%' valign='top' style='padding:0; border-right: 1px solid #000;'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                sb.Append("<thead><tr><th colspan='12' width='100%' style='font-size:14px;text-align:center;border-bottom:1px solid #000;padding:10px;'>CLOSING BALANCE</th></tr>");
                sb.Append("<tr><td colspan='12' style='padding:0;border-bottom:1px solid #000;'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                sb.Append("<tr><th width='10%' style='text-align:center;border-right:1px solid #000;padding:10px;'>No. of Units</th> ");
                sb.Append("<th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Qty. <br/> (M.T.)</th> ");
                sb.Append("<th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Balance area in sq. Mtrs. <br/>(Gross or Net)</th>");
                sb.Append("<th width='15%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Balance Value</th>");
                sb.Append("<th width='15%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Balance Duty</th>");
                sb.Append("<th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Signature of Custom Officer</th>");
                sb.Append("<th width='20%' style='text-align:center;border-right:1px solid #000;padding:10px;'>Signature of Custom Assistant</th>");
                sb.Append("<th width='20%' style='text-align:center;padding:10px;'>Remarks</th></tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td></tr></thead>");
                sb.Append("<tbody><tr><td colspan='12'>");
                sb.Append("<table cellspacing='0' style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");

                if (objBR.lstDeliveryDetails.Where(n => n.DepositappId == item.DepositappId).ToList().Count > 0)
                {
                    objBR.lstDeliveryDetails.Where(x => x.DepositappId == item.DepositappId).ToList().ForEach(elem =>
                    {
                        int CBUnits = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (int)x.UnloadedUnits) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId && m.DeliveryOrderDtlId <= elem.DeliveryOrderDtlId).Sum(x => (int)x.Units);
                        decimal CBWeight = (objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.UnloadedWeights) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId && m.DeliveryOrderDtlId <= elem.DeliveryOrderDtlId).Sum(x => (decimal)x.Weight)) / 1000;
                        decimal CBSq = (objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.AreaOccupied) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId && m.DeliveryOrderDtlId <= elem.DeliveryOrderDtlId).Sum(x => (decimal)x.SQM));
                        decimal CBValue = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Value) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId && m.DeliveryOrderDtlId <= elem.DeliveryOrderDtlId).Sum(x => (decimal)x.Value);
                        decimal CBDuty = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Duty) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId && m.DeliveryOrderDtlId <= elem.DeliveryOrderDtlId).Sum(x => (decimal)x.Duty);

                        sb.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBUnits.ToString("") + "</td> ");
                        sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBWeight.ToString("0.000") + "</td> ");
                        sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBSq.ToString("0.000") + "</td>");
                        sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBValue.ToString("0.000") + "</td>");
                        sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBDuty.ToString("0.000") + "</td>");
                        sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                        sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                        sb.Append("<td width='20%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");
                    });
                }
                else
                {
                    sb.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td> ");
                    sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                    sb.Append("<td width='20%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");
                }
                /*int CBUnits = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (int)x.UnloadedUnits) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (int)x.Units);
                decimal CBWeight = (objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.UnloadedWeights) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Weight)) / 1000;
                decimal CBSq = (objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.AreaOccupied) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.SQM));
                decimal CBValue = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Value) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Value);
                decimal CBDuty = objBR.lstUnloadingDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Duty) - objBR.lstDeliveryDetails.Where(m => m.DepositappId == item.DepositappId).Sum(x => (decimal)x.Duty);

                sb.Append("<tr><td width='10%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBUnits.ToString("") + "</td> ");
                sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBWeight.ToString("0.000") + "</td> ");
                sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBSq.ToString("0.000") + "</td>");
                sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBValue.ToString("0.000") + "</td>");
                sb.Append("<td width='15%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'>" + CBDuty.ToString("0.000") + "</td>");
                sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                sb.Append("<td width='20%' style='text-align:center;border-right:1px solid #000;border-bottom:1px solid #000;padding:10px;'></td>");
                sb.Append("<td width='20%' style='text-align:center;border-bottom:1px solid #000;padding:10px;'></td></tr>");*/



                sb.Append("</tbody></table>");
                sb.Append("</td></tr></tbody>");
                sb.Append("</table></td>");

                sb.Append("</tr></tbody></table>");
                sb.Append("</td></tr>");
                sb.Append("</tbody></table>");

                sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                sb.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                lstSB.Add(sb.ToString());
            });
            string FileName = "BondRegister" + DepositeAppId + ".pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName))
            {
                System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName);
            }
            using (var RH = new ReportingHelper(PdfPageSize.A3Landscape, 40f, 40f, 10f, 10f, false, true))
            {
                RH.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }

        #endregion



        #region Insurance Register



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult InsuranceRegisterReport()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //Kdl_ReportRepository ObjReport = new Kdl_ReportRepository();
            //ObjReport.GetDistricts(ObjLogin.Uid);
            //if (ObjReport.DBResponse.Data != null)
            //{
            //    var JObj = JObject.Parse(JsonConvert.SerializeObject(ObjReport.DBResponse.Data));
            //    ViewBag.DistList = new SelectList(JObj["Dist"], "DistrictId", "DistrictName");
            //}

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetInsuranceRegisterReport(MonthlyCashBook ObjMonthlyCashBook)
        {
            Wlj_ReportRepository ObjRR = new Wlj_ReportRepository();
            IList<Wij_InsuranceRegister> LsInsRes = new List<Wij_InsuranceRegister>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.InsuranceRegister(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {


                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateInsuranceRegisterPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "InsuranceRegisterReport.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/InsuranceRegisterReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = "";
                    ObjPdf.HOAddress = "";
                    ObjPdf.ZonalOffice = "";
                    ObjPdf.ZOAddress = "";
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/InsuranceRegisterReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }





        #endregion

        #region GST AND MIS Report



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult GSTMISReport()
        {
            return PartialView();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GSTMISReport(string PeriodFrom, string PeriodTo)
        {
            Wlj_ReportRepository objRR = new Wlj_ReportRepository();
            objRR.GetGSTMISReport(PeriodFrom, PeriodTo);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                Wlj_GSTMISReport lstGSTMIS = new Wlj_GSTMISReport();
                lstGSTMIS = (Wlj_GSTMISReport)objRR.DBResponse.Data;
                Path = GetGSTMISHtml(lstGSTMIS, PeriodFrom, PeriodTo);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }



        public string GetGSTMISHtml(Wlj_GSTMISReport lstGSTMIS, string PeriodFrom, string PeriodTo)
        {
            var PeriodFromMonth = PeriodFrom.Split('/')[1];
            var PeriodFromYear = PeriodFrom.Split('/')[2];

            var PeriodToMonth = PeriodTo.Split('/')[1];
            var PeriodToYear = PeriodTo.Split('/')[2];

            var printMonth = "";
            var printYear = "";

            if (PeriodFromMonth == PeriodToMonth && PeriodFromYear == PeriodToYear)
            {
                switch (PeriodFromMonth)
                {
                    //Here ds is list of invoice of a module between two dates 
                    case "01":
                        printMonth = "JANUARY";
                        printYear = PeriodFromYear;
                        break;
                    case "02":
                        printMonth = "FEBRUARY";
                        printYear = PeriodFromYear;
                        break;
                    case "03":
                        printMonth = "MARCH";
                        printYear = PeriodFromYear;
                        break;
                    case "04":
                        printMonth = "APRIL";
                        printYear = PeriodFromYear;
                        break;
                    case "05":
                        printMonth = "MAY";
                        printYear = PeriodFromYear;
                        break;
                    case "06":
                        printMonth = "JUNE";
                        printYear = PeriodFromYear;
                        break;
                    case "07":
                        printMonth = "JULY";
                        printYear = PeriodFromYear;
                        break;
                    case "08":
                        printMonth = "AUGUST";
                        printYear = PeriodFromYear;
                        break;
                    case "09":
                        printMonth = "SEPTEMBER";
                        printYear = PeriodFromYear;
                        break;
                    case "10":
                        printMonth = "OCTOBER";
                        printYear = PeriodFromYear;
                        break;
                    case "11":
                        printMonth = "NOVEMBER";
                        printYear = PeriodFromYear;
                        break;
                    case "12":
                        printMonth = "DECEMBER";
                        printYear = PeriodFromYear;
                        break;
                }
            }
            else
            {
                printMonth = "";
                printYear = "";
            }

            // CASh Total
            var CashValueTotal = lstGSTMIS.lstCash.Sum(x => x.Amount);
            var CashCGSTTotal = lstGSTMIS.lstCash.Sum(x => x.CGST);
            var CashSGSTTotal = lstGSTMIS.lstCash.Sum(x => x.SGST);
            var CashIGSTTotal = lstGSTMIS.lstCash.Sum(x => x.IGST);
            var CashTotal = lstGSTMIS.lstCash.Sum(x => x.Total);

            // SD Total
            var SDValueTotal = lstGSTMIS.lstSD.Sum(x => x.Amount);
            var SDCGSTTotal = lstGSTMIS.lstSD.Sum(x => x.CGST);
            var SDSGSTTotal = lstGSTMIS.lstSD.Sum(x => x.SGST);
            var SDIGSTTotal = lstGSTMIS.lstSD.Sum(x => x.IGST);
            var SDTotal = lstGSTMIS.lstSD.Sum(x => x.Total);


            // cash and sd total
            //var valuetotal = lstGSTMIS.lstCash.Sum(x => x.Amount) + lstGSTMIS.lstSD.Sum(x => x.Amount);
            //var CGSTTotal = lstGSTMIS.lstCash.Sum(x => x.CGST) + lstGSTMIS.lstSD.Sum(x => x.CGST);
            //var SGSTTotal = lstGSTMIS.lstCash.Sum(x => x.SGST) + lstGSTMIS.lstSD.Sum(x => x.SGST);
            //var IGSTTotal = lstGSTMIS.lstCash.Sum(x => x.IGST) + lstGSTMIS.lstSD.Sum(x => x.IGST);
            //var Total = lstGSTMIS.lstCash.Sum(x => x.Total) + lstGSTMIS.lstSD.Sum(x => x.Total);

            var valuetotal = CashValueTotal + SDValueTotal;
            var CGSTTotal = CashCGSTTotal + SDCGSTTotal;
            var SGSTTotal = CashSGSTTotal + SDSGSTTotal;
            var IGSTTotal = CashIGSTTotal + SDIGSTTotal;
            var Total = CashTotal + SDTotal;

            List<string> lstSB = new List<string>();
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
            sb.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            sb.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 16px; margin:0; padding:0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
            sb.Append("<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
            sb.Append("<span style='font-size: 7pt;'>H-22, M I D C Waluj, <br/> Opposite Nilason Corporate, Aurangabad,<br/> Maharashtra - 431136</ span>");
            sb.Append("<br/><label style='font-size: 7pt; font-weight:bold;'><b>GST</b> and <b>MIS Report</b> for the month of " + printMonth + " - " + printYear + "</label>");
            sb.Append("<br/><label style='font-size: 7pt;'>( To be faxed to Regional Office. A/c Section before 2<sup>nd</sup> of the succeeding month without fail )</label>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td><span><br/></span></td></tr>");
            sb.Append("</tbody></table>");

            sb.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt; border:1px solid #000; text-align:center;'><tbody>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000;'>Information for GST Purpose</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5'>This coloum should be filled up with details of GST payable</td></tr>");

            sb.Append("<tr><td colspan='2' width='c%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>Sr No (A)</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount realized on Cash basis during the month other than billing</td>");
            sb.Append("<td colspan='5' width='45%' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td rowspan='2' valign='top' cellpadding='5' style='border-right:1px solid #000; width: 16.5%;'>Value</td>");
            sb.Append("<td colspan='3' valign='top' cellpadding='5' style='border-right:1px solid #000; border-bottom:1px solid #000; width:60%;'>GST</td>");
            sb.Append("<td rowspan='2' valign='top' cellpadding='5' style='width: 17%;'>Total</td></tr>");
            sb.Append("<tr><td cellpadding='5' style='border-right:1px solid #000;'>CGST <br/> 9%</td>");
            sb.Append("<td cellpadding='5' style='border-right:1px solid #000;'>SGST <br/> 9%</td>");
            sb.Append("<td cellpadding='5' style='border-right:1px solid #000;'>IGST <br/> 18%</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Cash Book Collection</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-top:1px solid #000;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount received against storage charges on Bond/General warehousing/other commodities</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstCash[0].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'>" + lstGSTMIS.lstCash[0].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>" + lstGSTMIS.lstCash[0].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:40px;'>" + lstGSTMIS.lstCash[0].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstCash[0].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>2</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount received against DESS/PCS/OT Ch</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[1].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + lstGSTMIS.lstCash[1].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + lstGSTMIS.lstCash[1].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + lstGSTMIS.lstCash[1].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[1].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>3</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount received against H & T charges</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[2].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + lstGSTMIS.lstCash[2].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + lstGSTMIS.lstCash[2].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + lstGSTMIS.lstCash[2].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[2].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>4</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Round Up</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[3].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + lstGSTMIS.lstCash[3].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>Total</th>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + CashValueTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + CashCGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + CashSGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + CashIGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + CashTotal + "</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>P.D.A/c Adjustment</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-top:1px solid #000;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amt received against storage charges on Bond/General warehousing/other commodities</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[0].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'>" + lstGSTMIS.lstSD[0].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>" + lstGSTMIS.lstSD[0].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:40px;'>" + lstGSTMIS.lstSD[0].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[0].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>2</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount received against DESS/PCS/Misc Ch (Room Rent + Electricity Ch) & OT Charges</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[1].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'>" + lstGSTMIS.lstSD[1].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>" + lstGSTMIS.lstSD[1].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:40px;'>" + lstGSTMIS.lstSD[1].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[1].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>3</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount received against H&T Charges Franchise charges for Import/Export</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[2].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'>" + lstGSTMIS.lstSD[2].CGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>" + lstGSTMIS.lstSD[2].SGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:40px;'>" + lstGSTMIS.lstSD[2].IGST + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; height:40px; text-align:right;'>" + lstGSTMIS.lstSD[2].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>4</td>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Round Up</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + lstGSTMIS.lstSD[3].Amount + "</td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + lstGSTMIS.lstSD[3].Total + "</td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>Total</th>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + SDValueTotal + " </th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + SDCGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + SDSGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + SDIGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + SDTotal + "</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>Grand Total (A)</th>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + valuetotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + CGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + SGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + IGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + Total + "</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><th colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'>B</th>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Amount realized against the bill raised 31.01.2019 (GST)</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-top:1px solid #000;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 30%;'>Depositor Name</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 30%;'>Nature of Bills (St Ch/Ins Ch/PCS/H&T)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 25%;'>Bill No & Date</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 15%;'>Net Amt Recd</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 43%; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right; height:40px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            //START LOOP//
            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 30%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 30%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 25%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 15%; height:30px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            //Start Blank Data//
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 43%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right; height:30px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            //End Blank Data//
            sb.Append("</tr>");
            //END LOOP//

            sb.Append("<tr><th colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'>C</th>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'>GST</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-top:1px solid #000;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 30%;'>Depositor Name</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 30%;'>Nature of Bills (St Ch/Ins Ch/PCS/H&T)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 25%;'>Nature of Bills (St Ch/Ins Ch/PCS/H&T)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 15%;'>Bill Amount</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 43%; height:40px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right; height:40px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            //sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            //sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            //sb.Append("<tr><th valign='top' cellpadding='10' style='border-right:1px solid #000; width: 25%; height:70px;'></th>");
            //sb.Append("<th cellpadding='10' valign='top' style='border-right:1px solid #000; width: 25%; height:70px;'></th>");
            //sb.Append("<th cellpadding='10' valign='top' style='border-right:1px solid #000; width: 25%; height:70px;'></th>");
            //sb.Append("<th cellpadding='10' valign='top' style='width: 25%; height:70px;'></th></tr>");
            //sb.Append("</tbody></table>");
            //sb.Append("</td>");
            sb.Append("</tr>");

            //START LOOP//
            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 30%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 30%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 25%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 15%; height:30px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            //Start Blank Data//
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 43%; height:30px;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right; height:30px;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            //End Blank Data//
            sb.Append("</tr>");
            //END LOOP//


            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000;'>Sr No</td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Less Credit note during the month</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border-top:1px solid #000;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 38.33333333333333%; height:40px;'>Depositor Name</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 38.33333333333333%; height:40px;'>Credit Note No</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 23.33333333333333%; height:40px;'>Amount</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("<td colspan='5' width='45%' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th rowspan='2' valign='top' cellpadding='5' style='border-right:1px solid #000; width: 17%;'>Value</th>");
            sb.Append("<th colspan='3' valign='top' cellpadding='5' style='border-right:1px solid #000; border-bottom:1px solid #000; width:62%;'>GST</th>");
            sb.Append("<th rowspan='2' valign='top' cellpadding='5' style='width: 17%;'>Total</th></tr>");
            sb.Append("<tr><th cellpadding='5' style='border-right:1px solid #000;'>CGST</th>");
            sb.Append("<th cellpadding='5' style='border-right:1px solid #000;'>SGST</th>");
            sb.Append("<th cellpadding='5' style='border-right:1px solid #000;'>IGST</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>1</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 38.33333333333333%;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 38.33333333333333%;'></th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 23.33333333333333%;'></th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; text-align:right;'></td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='2%' cellpadding='5' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>E</td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border-right:1px solid #000; border-top:1px solid #000; text-align:left;'>Net Service Tax payable(A+B+C) = D</th>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; text-align:right;'>" + valuetotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%;'>" + CGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%;'>" + SGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%;'>" + IGSTTotal + "</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 20%; text-align:right;'>" + Total + "</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("</tbody></table>");

            sb.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt; text-align:center;'><tbody>");
            sb.Append("<tr><td colspan='12' cellpadding='5'><span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <b>Information for MIS Purpose only</b> (Please furnish all bills realized during the month other than what is recorded at B Pre page)</td></tr>");

            sb.Append("<tr><th colspan='2' width='2%' cellpadding='5' style='border:1px solid #000; border-right:0; border-bottom:0;'>Sr No</th>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border:1px solid #000; border-bottom:0; border-right:0; text-align:left;'>Amount against the bills which was raised after 30.06.2018(including S Tax and non S Tax billing)</th>");
            sb.Append("<td colspan='5' width='45%' cellpadding='5' style='border:1px solid #000; border-bottom:0;'></td></tr>");

            sb.Append("<tr><td colspan='2' width='6%' cellpadding='5' valign='top' style='border:1px solid #000; border-top:1px solid #000;'></td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-right:1px solid #000; border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 38.33333333333333%; height:40px;'>Depositor Name</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 38.33333333333333%; height:40px;'>Nature of bills (St Ch/Ins Ch/PCS/H&T)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='width: 23.33333333333333%; height:40px;'>Bill No & Date</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border-top:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><th valign='top' cellpadding='5' style='border-right:1px solid #000; width: 20%; height:40px;'>Net deduction <br/> (b)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:40px;'>TDS Deduction</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>Total (a+b)</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:40px;'>CR No & Dt</th>");
            sb.Append("<th cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:40px;'>Remark</th></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='2' width='6%' cellpadding='5' valign='top' style='border:1px solid #000; border-top:0; border-right:0;'></td>");
            sb.Append("<th colspan='5' width='45%' cellpadding='5' style='border:1px solid #000; border-right:0;'>Total</th>");
            sb.Append("<td colspan='5' width='45%' valign='top' style='border:1px solid #000;'>");
            sb.Append("<table cellspacing='0' valign='top' cellpadding='0' style='width:100%; font-size:7pt;'><tbody>");
            sb.Append("<tr><td valign='top' cellpadding='5' style='width: 20%; height:30px; text-align:right;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 21%; height:30px;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 20%; height:30px;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='border-right:1px solid #000; width: 19%; height:30px;'></td>");
            sb.Append("<td cellpadding='5' valign='top' style='width: 20%; height:30px; text-align:right;'></td></tr>");
            sb.Append("</tbody></table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            sb.Append("<tr><td colspan='12' cellpadding='5'>CERTIFICATE</td></tr>");

            sb.Append("<tr><td colspan='12' cellpadding='5' style='text-align:left;'>Certified that all the bills for the month of <b>" + printMonth + " " + printYear + " </b> have been raised on depositors during the month itself and GST claimed correctly as per Sr No C above. Also GST on realization basis stated at Sr No. B & C have been verified and nothing is omitted for the purpose of GST payable for the month.</td></tr>");

            sb.Append("<tr><td colspan='12' cellpadding='5'><span><br/><br/></span></td></tr>");

            sb.Append("<tr><th colspan='12' cellpadding='5' style='text-align:right;'>MANAGER, ICD, WALUJ</th></tr>");

            sb.Append("<tr><th colspan='12' cellpadding='5' style='text-align:left;'>Submitted to :-</th></tr>");
            sb.Append("<tr><td colspan='1' width='10%'></td> <td colspan='11' width='90%' cellpadding='5' style='text-align:left;'>The Manager(A/cs) / Asstt Manager(A/cs), <br/> CWC, Regional Office, <br/> Mumbai</td></tr>");



            sb.Append("</tbody></table>");



            sb.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            sb.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
            lstSB.Add(sb.ToString());

            string FileName = "GSt and MIS Report.pdf";
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName))
            {
                System.IO.File.Delete(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName);
            }
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                RH.GeneratePDF(Server.MapPath("~/Docs/") + Session.SessionID + "/" + FileName, sb.ToString());
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }


        #endregion
        #region register of outward supply

        [HttpGet]
        public ActionResult RegisterOfOutwardSupply()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult RegisterOfOutwardSupply(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                var ObjRR = new Wlj_ReportRepository();
                ObjRR.GetRegisterofOutwardSupply(date1, date2);
                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))    
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xlsx");
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                    using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                    {
                        exl.AddCell("A1", "No data found");
                        exl.Save();
                        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xlsx");
                    }
                }
            }
            catch
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xlsx");
                }
            }
            // return null;
        }

        #endregion

# region DetailsOfContainere
        public ActionResult DetailsOfContainer()
        {
            List<SelectListItem> lstYear = new List<SelectListItem>();
            int FrmYr = DateTime.Today.Year - 20;
            int Toyr = DateTime.Today.Year + 10;
            for (int i = FrmYr; i <= Toyr; i++)
            {
                lstYear.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.lstyear = lstYear;
            return PartialView();
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetContainerDetails(int Month, int Year)
        {
            Wlj_ReportRepository objR = new Wlj_ReportRepository();
           objR.GetImportExporContainer(Month, Year);
            string Path = "";
            //if (objR.DBResponse.Data != null)
            //{
            //Hdb_AbstractAccuredReport objAbR = new Hdb_AbstractAccuredReport();
            //objAbR = (Hdb_AbstractAccuredReport)objR.DBResponse.Data;
            //Path = GenerateAbstractAccuredReportTaxable(objAbR, Month, Year);
            Path = GenerateContainerDetailsPRint((DataSet)objR.DBResponse.Data, Month, Year);
            return Json(new { Message = Path, Status = 1 });
           // }
            //else
            //{
               // return Json(new { Message = "", Status = 0 });
            //}

        }

        [NonAction]
        public string GenerateContainerDetailsPRint(DataSet ds,  int Month, int Year)
        {
            try
            {
                List<dynamic> LstImport = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
                List<dynamic> Lstexport = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
                string strMonth = new DateTime(1900, Month, 01).ToString("MMMM").ToUpper();
                var FileName = "ContainerDetailsReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 16px; margin:0; padding:0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 7pt;'>H-22, M I D C Waluj, Opposite Nilason Corporate, Aurangabad, <br/> Maharashtra - 431136</span>");
                Pages.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>DETAILS OF IMPORT/EXPORT/CONTAINER</label>");
                Pages.Append("</td></tr>");

                Pages.Append("<tr><th><span><br/></span></th></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:8pt;'><b>MONTH :</b> "+ System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Month) +"</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; margin-top:10px;'><thead>");                
                Pages.Append("<tr><th colspan='12'>IMPORTS</th></tr>");
                Pages.Append("</thead></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; font-size:7.5pt; text-align:center;'><thead>");
                Pages.Append("<tr><th rowspan='2' style='border-right:1px solid #000; width:30px;'>Sr.No</th>");
                Pages.Append("<th rowspan='2' style='border-right:1px solid #000; width:150px;'>Name of Importer (M/s)</th>");
                Pages.Append("<th colspan='2' style='border-right:1px solid #000; width:100px;'>No of Containers</th>");
                Pages.Append("<th style='width:150px;'>Commodity</th></tr>");

                Pages.Append("<tr><th style='border-right:1px solid #000; border-top:1px solid #000; width:50px;'>20'</th>");
                Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000; width:50px;'>40'</th></tr>");
                Pages.Append("</thead>");
                i = 0;
                Pages.Append("<tbody>");
                i = 0;
                int SUMOFIMPTWENTY = 0;
                int SUMOFIMPFORTY = 0;
                LstImport.ToList().ForEach(item =>
                {
                    i = 1;
                    Pages.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.ImporterName + "</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.Twenty + "</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.fourty + "</td>");
                    Pages.Append("<td style='border-top:1px solid #000;'>" + item.Comodity + "</td></tr>");
                    i++;
                    SUMOFIMPTWENTY = Convert.ToInt32(item.Twenty) + SUMOFIMPTWENTY;
                    SUMOFIMPFORTY = Convert.ToInt32(item.fourty) + SUMOFIMPFORTY;

                });
                 Pages.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>TOTAL :</th>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>"+ SUMOFIMPTWENTY + "</th>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>"+ SUMOFIMPFORTY + "</th>");
                    Pages.Append("<td style='border-top:1px solid #000;'></td></tr>");
             
                    
                Pages.Append("</tbody></table>");


                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt;'><thead>");
                Pages.Append("<tr><th><span><br/></span></th></tr>");
                Pages.Append("<tr><th colspan='12'>EXPORTS</th></tr>");
                Pages.Append("</thead></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; font-size:7.5pt; text-align:center;'><thead>");
                Pages.Append("<tr><th rowspan='2' style='border-right:1px solid #000; width:30px;'>Sr.No</th>");
                Pages.Append("<th rowspan='2' style='border-right:1px solid #000; width:150px;'>Name of Exporters (M/s)</th>");
                Pages.Append("<th colspan='2' style='border-right:1px solid #000; width:100px;'>No of Containers</th>");
                Pages.Append("<th style='width:150px;'>Name of CHA</th></tr>");

                Pages.Append("<tr><th style='border-right:1px solid #000; border-top:1px solid #000; width:50px;'>20'</th>");
                Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000; width:50px;'>40'</th></tr>");
                Pages.Append("</thead>");

                Pages.Append("<tbody>");
                int SUMOFEXPTWENTY = 0;
                int SUMOFEXPFORTY = 0;
                Lstexport.ToList().ForEach(item =>
                {
                    int j = 1;
                    Pages.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'>"+j+"</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.ExporterName + "</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>"+item.Twenty + "</td>");
                    Pages.Append("<td style='border-right:1px solid #000; border-top:1px solid #000;'>" + item.fourty + "</td>");
                    Pages.Append("<td style='border-top:1px solid #000;'>" + item.CHA + "</td></tr>");
                    j++;
                    SUMOFEXPTWENTY = Convert.ToInt32(item.Twenty) + SUMOFEXPTWENTY;
                    SUMOFEXPFORTY = Convert.ToInt32(item.fourty)+ SUMOFEXPFORTY;
                });
                Pages.Append("<tr><td style='border-right:1px solid #000; border-top:1px solid #000;'></td>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>TOTAL :</th>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>"+SUMOFEXPTWENTY+"</th>");
                    Pages.Append("<th style='border-right:1px solid #000; border-top:1px solid #000;'>"+SUMOFEXPFORTY+"</th>");
                    Pages.Append("<td style='border-top:1px solid #000;'></td></tr>");
              
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + "/" + FileName, Pages.ToString());
                }
                return "/Docs/" + Session.SessionID + "/Report/" + FileName;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region BulkDebitnoteReport
        [HttpGet]
        public ActionResult BulkDebitnoteReport()
        {

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]  
        public JsonResult PrintDRNote(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            Wlj_ReportRepository objRepo = new Wlj_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany(); 
            objRepo.PrintDetailsForBulkCRNote(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "D");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }

        //    public string GenerateCRNotePDF(PrintModelOfBulkCrCompany objCR)
        //    {
        //        string Note = "";
        //        string SACCode = "", note = "", fileName = "";
        //        objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
        //        {
        //            if (SACCode == "")
        //                SACCode = item.SACCode;
        //            else
        //                SACCode = SACCode + "," + item.SACCode;
        //        });

        //            note = (Note == "C") ? "CREDIT NOTE" : "DEBIT NOTE";
        //        fileName = (Note == "C") ? ("CreditNote" + CRNoteId + ".pdf") : ("DebitNote" + CRNoteId + ".pdf");
        //        string Path = Server.MapPath("~/Docs/") + Session.SessionID;//+ "/CreditNote" + CRNoteId + ".pdf";
        //        if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
        //        {
        //            Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
        //        }
        //        if (System.IO.File.Exists(Path + "/" + fileName))
        //        {
        //            System.IO.File.Delete(Path + "/" + fileName);

        //        }
        //        objCR.lstCrParty.ToList().ForEach(item =>
        //        {
        //            string html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th colspan='2' style='text-align:center;padding:8px;'>Principle Place of Business: <span style='border-bottom:1px solid #000;'>______________________</span><br/>" + note + "</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Provider</th><th style='border:1px solid #000;text-align:center;padding:8px;width:50%;'>Details of Service Receiver</th></tr></thead><tbody><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: " + objCR.CompanyName + "</td><td style='border:1px solid #000;text-align:left;padding:8px;'>Name: <span>" + item.PartyName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>Warehouse Address: <span>" + objCR.CompanyAddress + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>Address: <span>" + item.PartyAddress + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + objCR.CompCityName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>City: <span>" + item.PartyCityName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + objCR.CompStateName + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State: <span>" + item.PartyStateName + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + objCR.CompStateCode + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'>State Code: <span>" + item.PartyStateCode + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>GSTIN: <span>" + objCR.CompGstIn + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span>GSTIN(if registered):" + item.PartyGSTIN + "</span></td></tr><tr><td style='border:1px solid #000;text-align:left;padding:8px;'>PAN:<span>" + objCR.CompPan + "</span></td><td style='border:1px solid #000;text-align:left;padding:8px;'><span></span></td></tr><tr><td style='text-align:left;padding:8px;'>Debit/Credit Note Serial No: <span style='border-bottom:1px solid #000;'>" + item.CRNoteNo + "</span><br/><br/>Date of Issue: <span style='border-bottom:1px solid #000;'>" + item.CRNoteDate + "</span></td><td style='text-align:left;padding:8px;'>Accounting Code of <span>" + SACCode + "</span><br/><br/>Description of Services: <span>Other Storage & Warehousing Services</span></td></tr><tr><td colspan='2' style='text-align:left;padding:8px;'>Original Bill of Supply/Tax Invoice No: <span style='border-bottom:1px solid #000;'>" + item.InvoiceNo + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Date: <span style='border-bottom:1px solid #000;'>" + Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy") + "</span></td></tr><tr><td colspan='2'>";
        //        string htmltable = "<table style='width:100%;font-family:Verdana,Arial,San-serif;border-collapse:collapse;border:1px solid #000;font-size:7pt;'><thead><tr><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Sl No.</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:20%;'>Particulars</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;width:7%;'>Taxable Value</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>CGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>SGST</th><th colspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>IGST</th><th rowspan='2' style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Total Amount</th></tr><tr><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Reasons for increase / decrease in original invoice</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Rate</th><th style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>Amt</th></tr></thead><tbody>";
        //        string tr = "";
        //        int Count = 1;
        //        decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
        //            objCR.lstCharges.Where(y => y.CRNoteId == item.CRNoteId).ToList().ForEach(data =>
        //           {
        //                tr += "<tr><td style='border:1px solid #000;text-align:center;padding:5px;font-size:7pt;'>" + Count + "</td><td style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'>" + item.ChargeName + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Taxable + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTPer + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;font-size:7pt;'>" + item.Total + "</td></tr>";
        //            IGSTAmt += item.IGSTAmt;
        //            CGSTAmt += item.CGSTAmt;
        //            SGSTAmt += item.SGSTAmt;
        //            Count++;
        //        });
        //        string AmountInWord = ConvertNumbertoWords((long)objCR.GrandTotal);
        //        string Remarks = objCR.Remarks;
        //        string tfoot = "<tr><td style='border:1px solid #000;text-align:center;padding:5px;'></td><td style='border:1px solid #000;text-align:left;padding:5px;'></td><td style='border:1px solid #000;text-align:center;padding:5px;font-weight:600;'>Total</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + CGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + SGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'></td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + IGSTAmt + "</td><td style='border:1px solid #000;text-align:right;padding:5px;'>" + objCR.TotalAmt + "</td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in figure)</span> <span>" + objCR.GrandTotal + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Total Debit/Credit Note Value (in words)</span> <span>" + AmountInWord + "</span></td></tr><tr><td colspan='10' style='border:1px solid #000;text-align:left;padding:5px;font-size:7pt;'><span style='border-bottom:1px solid #000;font-weight:600;'>Remarks</span> <span>" + Remarks + "</span></td></tr></tbody></table></td></tr><tr><td colspan='2' style='text-align:left;padding:5px;'>Note:<br/><span style='padding:8px;'>1. The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</span><br/><span style='padding:8px;'>2. Credit Note is to be issued where excess amount cliamed in original invoice.</span><br/><span style='padding:8px;'>3. Debit Note is to be issued where less amount claimed in original invoice.</span></td></tr><tr><td></td><td style='text-align:left;padding:8px;font-weight:600;'>Signature: ____________________________<br/><br/>Name of the Signatory: __________________ <br/><br/>Designation/Status: ____________________ </td></tr><tr><td style='text-align:left;padding:5px;'>To,<br/><span style='border-bottom:1px solid #000;'>____________________________ <br/>____________________________<br/>____________________________<br/></span><br/><br/>Copy To:<br/>1. Duplicate Copy for RM, CWC,RO -<br/>2. Triplicate Copy for Warehouse</td></tr></tbody></table>";
        //        html = html + htmltable + tr + tfoot;
        //        using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f))
        //        {
        //            RH.GeneratePDF(Path + "/" + fileName, html);
        //        }
        //        return "/Docs/" + Session.SessionID + "/" + fileName;
        //    }


        //}

        [NonAction]
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR)
        {
            Einvoice obj = new Einvoice();
            string note = "Debit Note";

            List<string> lstSB = new List<string>();

            string SACCode = "";
            objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            {
                if (SACCode == "")
                    SACCode = item.SACCode;
                else
                    SACCode = SACCode + "," + item.SACCode;
            });

            objCR.lstCrParty.ToList().ForEach(item =>
            {

                //    /*Header Part*/
                StringBuilder html = new StringBuilder();

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr>");

                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                //html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwcwfdcfs@gmail.com</label></td>");
                //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO_IMG'/></td></tr>");
                html.Append("<tr><td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");

                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + "  </td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                }







                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src=''/> </td>");

                html.Append("</tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                //html.Append("<br /><label style='font-size: 14px; font-weight:bold;'></label>");
                //html.Append("</td></tr>");

                html.Append("</tbody></table>");

                html.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='6' cellspacing='0' style='width:50%; border-right: 1px solid #000;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Provider</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b>" + objCR.CompanyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Warehouse Address:</b>" + objCR.CompanyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + objCR.CompCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + objCR.CompStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b>" + objCR.CompStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN:</b> " + objCR.CompGstIn + "</td></tr>");
                html.Append("<tr><td cellpadding='10'><b>PAN:</b>" + objCR.CompPan + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' cellspacing='0' Valign='top' style='width:50%;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Receiver</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b> " + item.PartyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Address:</b> " + item.PartyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + item.PartyCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + item.PartyStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b> " + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN(if registered):</b> " + item.PartyGSTIN + "</td></tr>");
                html.Append("<tr><td cellpadding='10'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='6' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Debit/Credit Note Serial No :</b> " + item.CRNoteNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Accounting Code of</b> " + SACCode + "</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Date of Issue :</b> " + Convert.ToDateTime(item.CRNoteDate).ToString("dd/MM/yyyy") + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Description of Services :</b> Other Storage & Warehousing Services</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Original Bill of Supply/Tax Invoice No :</b>  " + item.InvoiceNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Date :</b>  " + Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy") + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Sl No.</th>");
                html.Append("<th colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Particulars</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Value</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>Total Amount</th></tr>");
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Reason for increase/decrease in original invoice</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                int i = 1;
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                decimal total = 0;
                objCR.lstCharges.Where(y => y.CRNoteId == item.CRNoteId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>" + data.ChargeName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTAmt + "</td>");

                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + data.Total + "</td></tr>");
                    i = i + 1;
                    IGSTAmt += data.IGSTAmt;
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in figure):</b>" + item.GrandTotal + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in words):</b> " + AmountInWord + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Remarks:</b>" + item.Remarks + "</td></tr>");

                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><th>Note:</th></tr>");
                html.Append("<tr><td colspan='12'><b>1.</b> The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</td></tr>");
                html.Append("<tr><td colspan='12'><b>2.</b> Credit Note is to be issued where excess amount cliamed in original invoice.</td></tr>");
                html.Append("<tr><td colspan='12'><b>3.</b> Debit Note is to be issued where less amount claimed in original invoice.</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><td colspan='6' width='80%'></td>");
                html.Append("<th colspan='5' width='50%'>Signature: ____________________________ <br/><br/> Name of the Signatory: __________________ <br/><br/> Designation/Status: ____________________</th></tr>");

                html.Append("<tr><td colspan='6' width='50%'>To, <br/> ____________________________ <br/>____________________________<br/>____________________________<br/><br/> Copy To: <br/> Duplicate Copy for RM, CWC,RO - <br/> 2.Triplicate Copy for Warehouse</td>");
                html.Append("<th colspan='6' width='50%'></th></tr>");


                html.Append("</tbody></table>");
                // html.Append("<div style='margin-top:10px;'><br/><br/><br/><br/></div>");
                // html.Append("<div style='margin-top:10px;'><br/><br/></div>");

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());

            });



            var FileName = "BulkDebitReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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
        #region BulkCreaditNoteReport
        [HttpGet]
        public ActionResult BulkCreaditNoteReport()
        {

            //ExportRepository objExport = new ExportRepository();
            //objExport.GetPaymentParty();
            //if (objExport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            return PartialView();
        }



        [NonAction]
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR, string note)
        {
            Einvoice obj = new Einvoice();

            List<string> lstSB = new List<string>();

            string SACCode = "";
            objCR.lstCharges.Select(x => new { SACCode = x.SACCode }).Distinct().ToList().ForEach(item =>
            {
                if (SACCode == "")
                    SACCode = item.SACCode;
                else
                    SACCode = SACCode + "," + item.SACCode;
            });

            objCR.lstCrParty.ToList().ForEach(item =>
            {

                //    /*Header Part*/
                StringBuilder html = new StringBuilder();

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr>");

                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                //html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                // html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwcwfdcfs@gmail.com</label></td>");
                //html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='ISO_IMG'/></td></tr>");
                html.Append("<tr><td width='800%' valign='top' align='center'><label style='font-size: 10pt; font-weight: bold;'>Principle Place of Business: <span style='border-bottom: 1px solid #000;'>______________________</span></label><br /><label style='font-size: 10pt; font-weight: bold;'>" + note + "</label></td></tr>");

                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + "  </td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SignedQRCode == "")
                { }
                else
                {
                    if (item.SupplyType == "B2C")
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                    else
                    {
                        html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                    }
                }







                html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src=''/> </td>");

                html.Append("</tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                //html.Append("<br /><label style='font-size: 14px; font-weight:bold;'></label>");
                //html.Append("</td></tr>");

                html.Append("</tbody></table>");

                html.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed; width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif;'><tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='6' cellspacing='0' style='width:50%; border-right: 1px solid #000;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Provider</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b>" + objCR.CompanyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Warehouse Address:</b>" + objCR.CompanyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + objCR.CompCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + objCR.CompStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b>" + objCR.CompStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN:</b> " + objCR.CompGstIn + "</td></tr>");
                html.Append("<tr><td cellpadding='10'><b>PAN:</b>" + objCR.CompPan + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' cellspacing='0' Valign='top' style='width:50%;'>");
                html.Append("<table cellspacing='0' style='width:100%;font-size:8pt;'><tbody>");
                html.Append("<tr><th cellpadding='10' style='border-bottom: 1px solid #000;'>Details of Service Receiver</th></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Name:</b> " + item.PartyName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>Address:</b> " + item.PartyAddress + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>City:</b>" + item.PartyCityName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State:</b>" + item.PartyStateName + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>State Code:</b> " + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><td cellpadding='10' style='border-bottom: 1px solid #000;'><b>GSTIN(if registered):</b> " + item.PartyGSTIN + "</td></tr>");
                html.Append("<tr><td cellpadding='10'></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='6' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'><tbody>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Debit/Credit Note Serial No :</b> " + item.CRNoteNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Accounting Code of</b> " + SACCode + "</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Date of Issue :</b> " + Convert.ToDateTime(item.CRNoteDate).ToString("dd/MM/yyyy") + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Description of Services :</b> Other Storage & Warehousing Services</td></tr>");
                html.Append("<tr><td colspan='6' style='width:50%;'><b>Original Bill of Supply/Tax Invoice No :</b>  " + item.InvoiceNo + "</td> <td colspan='6' style='width:50%; text-align: right;'><b>Date :</b>  " + Convert.ToDateTime(item.InvoiceDate).ToString("dd/MM/yyyy") + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; border:1px solid #000; font-size:8pt; font-family:Verdana,Arial,San-serif; margin-top:20px;'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Sl No.</th>");
                html.Append("<th colspan='1' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Particulars</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Value</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>Total Amount</th></tr>");
                html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Reason for increase/decrease in original invoice</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                int i = 1;
                decimal IGSTAmt = 0, CGSTAmt = 0, SGSTAmt = 0;
                decimal total = 0;
                objCR.lstCharges.Where(y => y.CRNoteId == item.CRNoteId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>" + data.ChargeName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.CGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.IGSTAmt + "</td>");

                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + data.Total + "</td></tr>");
                    i = i + 1;
                    IGSTAmt += data.IGSTAmt;
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in figure):</b>" + item.GrandTotal + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Total Debit/Credit Note Value (in words):</b> " + AmountInWord + "</td></tr>");
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Remarks:</b>" + item.Remarks + "</td></tr>");

                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><th>Note:</th></tr>");
                html.Append("<tr><td colspan='12'><b>1.</b> The invoice issued earlier can be modified/cancelled by way of Debit Note/Credit Note.</td></tr>");
                html.Append("<tr><td colspan='12'><b>2.</b> Credit Note is to be issued where excess amount cliamed in original invoice.</td></tr>");
                html.Append("<tr><td colspan='12'><b>3.</b> Debit Note is to be issued where less amount claimed in original invoice.</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table cellpadding='5' cellspacing='0' style='width:100%; font-size:9pt; margin-top:20px;'><tbody>");
                html.Append("<tr><td><span><br/></span></td></tr>");
                html.Append("<tr><td colspan='6' width='80%'></td>");
                html.Append("<th colspan='5' width='50%'>Signature: ____________________________ <br/><br/> Name of the Signatory: __________________ <br/><br/> Designation/Status: ____________________</th></tr>");

                html.Append("<tr><td colspan='6' width='50%'>To, <br/> ____________________________ <br/>____________________________<br/>____________________________<br/><br/> Copy To: <br/> Duplicate Copy for RM, CWC,RO - <br/> 2.Triplicate Copy for Warehouse</td>");
                html.Append("<th colspan='6' width='50%'></th></tr>");


                html.Append("</tbody></table>");
                // html.Append("<div style='margin-top:10px;'><br/><br/><br/><br/></div>");
                // html.Append("<div style='margin-top:10px;'><br/><br/></div>");

                //html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());

            });



            var FileName = "BulkDebitReport" + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            Wlj_ReportRepository objRepo = new Wlj_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            string Note = "CREDIT NOTE";

            objRepo.PrintDetailsForBulkCRNote(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "C");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR, Note);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "error" });
            }
        }
        #endregion

        public string LoadImage(string img)
        {

            if (img != "")
            {

                string strm = img;

                //this is a simple white background image
                var myfilename = string.Format(@"{0}", Guid.NewGuid());

                //Generate unique filename
                string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                var bytess = Convert.FromBase64String(strm);
                using (var imageFile = new FileStream(filepath, FileMode.Create))
                {
                    imageFile.Write(bytess, 0, bytess.Length);
                    imageFile.Flush();
                }


                string targetpath = Server.MapPath("~/Docs/QRCode/") + myfilename + "crop" + ".jpeg";
                String newfilepath = Utility.ResizeImage(filepath, targetpath);
                return newfilepath;
            }
            else
            {
                return "";

            }
        }

        #region E-Invoice Excel Generation

        [HttpGet]
        public ActionResult RegisterOfEInvoice()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult EInvoiceExcel(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                var excelName = "";
                var ObjRR = new Wlj_ReportRepository();
                ObjRR.GetRegisterofEInvoice(date1, date2);

                excelName = "EInvoice.xls";

                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xls");
                    using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                    {
                        exl.AddCell("A1", "No data found");
                        exl.Save();
                        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EInvoice.xls");
                }
            }
            // return null;
        }

        #endregion

        #region Bulk IRN Generation

        [HttpGet]
        public ActionResult BulkIRNGeneration()
        {
            //WFLD_ReportRepository objPpgRepo = new WFLD_ReportRepository();
            //objPpgRepo.GetBulkIrnDetails();
            //if (objPpgRepo.DBResponse.Status > 0)
            //    ViewBag.InvoiceList = JsonConvert.SerializeObject(objPpgRepo.DBResponse.Data);
            //else
            //    ViewBag.InvoiceList = null;
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBulkIrnDetails()
        {
            Wlj_ReportRepository objPpgRepo = new Wlj_ReportRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (WLJ_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            Wlj_CWCImportController objWljImp = new Wlj_CWCImportController();
            Wlj_CashManagementController objwljCash = new Wlj_CashManagementController();

            try
            {
                var invoiceData = JsonConvert.DeserializeObject<WLJ_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            var result = await objWljImp.GetIRNForYardInvoice(item.InvoiceNo, item.SupplyType);
                        }
                        if (item.InvoiceType == "C")
                        {
                            var result1 = await objwljCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                        }
                        if (item.InvoiceType == "D")
                        {
                            var result2 = await objwljCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                
                return Json(new { Status = 1, Message = "IRN Generated" });

            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion



        #region OUTWardreristerSupply By Type
        [HttpGet]
        public ActionResult RegisterOfOutwardSupplyByType()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileResult RegisterOfOutwardSupplyByType(FormCollection fc)
        {
            try
            {
                var date1 = Convert.ToDateTime(fc["PeriodFrom"].ToString());
                var date2 = Convert.ToDateTime(fc["PeriodTo"].ToString());
                var Type = fc["ddlType"].ToString();
                var excelName = "";
                var ObjRR = new Wlj_ReportRepository();
                ObjRR.GetRegisterofOutwardSupplybyINVTYPE(date1, date2, Type);

                if (Type == "Inv") { Type = "Invoice"; }
                if (Type == "C") { Type = "Credit"; }
                if (Type == "D") { Type = "Debit"; }
                if (Type == "Unpaid") { Type = "Unpaid"; }
                if (Type == "CancelInv") { Type = "Cancel Invoice"; }

                excelName = "RegisterofOutwardSupply" + "_" + Type + ".xlsx";

                if (!string.IsNullOrEmpty(ObjRR.DBResponse.Data.ToString()))
                    return File(ObjRR.DBResponse.Data.ToString(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                else
                {
                    string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                    var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                    using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                    {
                        exl.AddCell("A1", "No data found");
                        exl.Save();
                        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch
            {
                string name = DateTime.Now.ToOADate().ToString().Replace(".", string.Empty);
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xlsx");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xlsx");
                }
            }
            // return null;
        }
        #endregion

        #region E04
        public ActionResult E04DetailsReport()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetE04List()
        {
            Wlj_ReportRepository ObjER = new Wlj_ReportRepository();
            List<Wlj_E04Report> LstE04 = new List<Wlj_E04Report>();
            ObjER.ListofE04Report(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Wlj_E04Report>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfE04DetailsReport", LstE04);
        }
        [HttpGet]
        public JsonResult LoadMoreE04List(int Page)
        {
            Wlj_ReportRepository ObjER = new Wlj_ReportRepository();
            var LstE04 = new List<Wlj_E04Report>();
            ObjER.ListofE04Report(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Wlj_E04Report>)ObjER.DBResponse.Data;
            }
            return Json(LstE04, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewE04DetailsReport(int ID)
        {
            Wlj_E04Report ObjE04 = new Wlj_E04Report();
            Wlj_ReportRepository ObjER = new Wlj_ReportRepository();
            ObjER.GetE04DetailById(ID);
            if (ObjER.DBResponse.Data != null)
            {
                ObjE04 = (Wlj_E04Report)ObjER.DBResponse.Data;
            }
            return PartialView(ObjE04);
        }


        [HttpGet]
        public JsonResult GetE04Search(string SB_No = "", string SB_Date = "", string Exp_Name = "")
        {
            Wlj_ReportRepository ObjER = new Wlj_ReportRepository();
            List<Wlj_E04Report> LstE04 = new List<Wlj_E04Report>();
            ObjER.GetE04DetailSearch(SB_No, SB_Date, Exp_Name);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Wlj_E04Report>)ObjER.DBResponse.Data;
            }
            //return PartialView("ListOfE04DetailsReport", LstE04);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Stuffing Acknowledgement Search       

        [HttpGet]
        public ActionResult StfAckSearch()
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();

            ObjGR.GetAllContainerNoForContstufserach("", 0);

            if (ObjGR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstStuffing = Jobject["LstStuffing"];
                ViewBag.StateCont = Jobject["State"];
            }
            else
            {
                ViewBag.LstStuffing = null;
            }


            ObjGR.GetAllShippingBillNoForContstufserach("", 0);
            if (ObjGR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstStuff = Jobject["LstStuff"];
                ViewBag.StateShipbill = Jobject["State"];
            }
            else
            {
                ViewBag.LstStuff = null;
            }

            return PartialView();
        }
        [HttpGet]
        public JsonResult SearchContainerNo(string cont)
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchShipbill(string shipbill)
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadContainerLists(string cont, int Page)
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadShipbillLists(string shipbill, int Page)
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getcontstuffingacksearch(string container, string shipbill)
        {
            Wlj_ReportRepository ObjGR = new Wlj_ReportRepository();
            List<Wlj_ContStufAckRes> Lststufack = new List<Wlj_ContStufAckRes>();
            ObjGR.GetStufAckResult(container, shipbill);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Wlj_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
