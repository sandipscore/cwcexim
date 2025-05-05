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
using System.Drawing;
using EinvoiceLibrary;
using System.Threading.Tasks;
using CwcExim.Areas.Import.Controllers;
using CwcExim.Areas.CashManagement.Controllers;


using CwcExim.Areas.CashManagement.Models;

namespace CwcExim.Areas.Report.Controllers
{

    public class VRN_ReportCWCController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }
        public VRN_ReportCWCController()
        {
            /*
             * CALL PROCEDURE AND SET VALUE OF PROPERTIES
             */
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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

       //   ExportRepository objExport = new ExportRepository();
        //    objExport.GetPaymentParty();
          //  if (objExport.DBResponse.Status > 0)
          //      ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
         //   else
          //      ViewBag.PaymentParty = null;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetImpPartyBulkParty()
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetPaymentPartyforBulkReport();
           
                if (ObjRR.DBResponse.Data != null)
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = -1, Message = "Error" });
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkInvoiceReport(BulkInvoiceReport ObjBulkInvoiceReport)
        {
            if (ObjBulkInvoiceReport.InvoiceNumber == null)
            {
                ObjBulkInvoiceReport.InvoiceNumber = "";
            }
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
                            case "EXPLod":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "LOADED CONTAINER PAYMENT SHEET";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforExportLoadedCont((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "BTT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "BTT PAYMENT SHEET";
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
                            default:
                                FilePath = GeneratingBulkPDFforVRNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
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
                            FilePath = GeneratingBulkPDFforVRNExpSealChecking(ds, ObjBulkInvoiceReport.InvoiceModuleName);
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
                        case "EC":
                            ObjBulkInvoiceReport.All = "";                             
                            FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        default:
                            FilePath = GeneratingBulkPDFforVRNAll(ds, ObjBulkInvoiceReport.InvoiceModuleName);
                            break;
                    }

                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
        }
        [NonAction]
        public string GeneratingBulkPDFforVRNExpSealChecking(DataSet ds, string InvoiceModuleName)
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
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");
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

            var BQRFileName = "";
            var BQRlocation = "";



            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();

            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);


            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                VRN_ExportRepository rep = new VRN_ExportRepository();
                VRN_BTTCargoDet objCargoDet = new VRN_BTTCargoDet();
                rep.GetCargoDetBTTById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCargoDet = (VRN_BTTCargoDet)rep.DBResponse.Data;
                }

                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }


                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");


                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 1px solid #000; width:100%; font-size: 6pt;' cellspacing='0' cellpadding='5'><tbody><tr>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + item.ExporterImporterName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> " + item.CHAName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b>" + item.ShippingLineName + "</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");


                int i = 1;
                Decimal totamt = 0;

                //------------------------------------------            
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Cargo Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Commodity</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/               
                
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.ShippingBillNo.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.ShippingBillDate.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.CommodityName.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.NoOfUnits.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + objCargoDet.GrossWeight.ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; text-align:right;'>" + objCargoDet.Fob.ToString() + "</td>");
                html.Append("</tr>");
                  
                /***************/
                html.Append("</tbody></table></td></tr>");
                //---------------------

                html.Append("<tr><td>");
                html.Append("</td></tr>");
                //------------------------------------------                

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center; ' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Exporter</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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
            var BQRFileName = "";
            var BQRlocation = "";

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstShippingBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            List<string> lstSB = new List<string>();


            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }


                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Details :</b> </th></tr>");

                html.Append("<tr><td colspan='12'><table cellpadding='5' style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align: center;' cellspacing='0' cellpadding='5'>");
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


                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Exporter Name</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int j = 1;

                lstShippingBill.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + j + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Exporter + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.NoOfUnits + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: right;'>" + elem.FobValue + "</td>");
                    html.Append("</tr>");
                    j = j + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();

            var BQRFileName = "";
            var BQRlocation = "";


            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b>" + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstStorageDate = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<string> lstSB = new List<string>();

            var BQRFileName = "";
            var BQRlocation = "";

            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);


            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }


                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 6pt; margin-top: 8px;'>Bond No :" + item.StuffingReqNo + "</b></th>");
                html.Append("<td colspan='6' width='50%' style='font-size:6pt; text-align: right'><label width='15%'><b>SAC validity :</b></label> From <u>" + lstContainer[0].SacApprovedDate + "</u> to <u>" + lstContainer[0].SacValidityDate + "</u></td></tr>");
                html.Append("<tr><th colspan='2' width='20%' style='text-align: left; font-size: 6pt; margin-top: 15px;' valign='top'>Storage Period :</th>");
                html.Append("<td colspan='10' width='80%'><table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='0'><tbody>");

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
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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
                html.Append("<table style='border-left: 1px solid #000; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();

            var BQRFileName = "";
            var BQRlocation = "";



            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);


            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {

                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='3'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='6' width='50%'><b style='text-align: left; font-size: 7pt;margin-top: 10px;'>SAC No :" + item.StuffingReqNo + "</b></th>");

                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<td colspan='6' width='50%' style='font-size:7pt;'><b>SAC validity :</b> From <u>" + elem.SacApprovedDate + "</u> to <u>" + elem.SacValidityDate + "</u></td></tr>");
                });

                html.Append("<tr><td colspan='12'>");
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                    html.Append("<td colspan='6' width='50%'>");
                    html.Append("<table style='border-right: 1px solid #000; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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
                    html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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
                    html.Append("</td></tr>");

                    html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 6pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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
                });
                i = 1;
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]); 
            List<dynamic> lstShippingBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]); 
            List<string> lstSB = new List<string>();
            var BQRFileName = "";
            var BQRlocation = "";


            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {

                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + (item.InvoiceType == "Tax" ? "Tax Invoice" : "Tax(0) Invoice") + "</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Container/Cargo Details :</b></th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Size</th>");
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
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
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
                //----------------------
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size: 6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SB Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Exporter Name</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Gross WT</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>No Of Pkg</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align:right;'>FOB Value</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int j = 1;

                lstShippingBill.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + j + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ShippingBillDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Exporter + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.GrossWt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.NoOfUnits + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:right;'>" + elem.FobValue + "</td>");
                    html.Append("</tr>");
                    j = j + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                //---------------------

                html.Append("<tr><td>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/Cargo Charges :</h3> </th></tr>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
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
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 80px;'>" + data.Taxable.ToString("0.00") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: right; width: 130px;'>" + data.Taxable.ToString("0.00") + "</td>");
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
                html.Append("<th rowspan='2' style='font-size: 6pt; text-align: right; width: 100px;'>" + item.InvoiceAmt.ToString("0.00") + "</th></tr>");
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

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Exporter</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

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

        public string GeneratingBulkPDFforVRNAll(DataSet ds, string InvoiceModuleName)
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

        [NonAction]
        public string GeneratingBulkPDFforGodown(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            var BQRFileName = "";
            var BQRlocation = "";

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);
            //List<dynamic> lstReassesbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();

            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            int cargotype = 0;
            string Container = "";
            string dt = "";
            string dttype = "Date of Seal Cutting";
            Einvoice obj = new Einvoice(new HeaderParam(), "");


            //lstCargoDetail.ToList().ForEach(item =>
            //{
            //    cargotype = (int)item.CargoType;
            //});

            //lstInvoice.ToList().ForEach(item =>
            //{
            //    Ppg_ReportRepository rep = new Ppg_ReportRepository();
            //    PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
            //    rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
            //    if (rep.DBResponse.Data != null)
            //    {
            //        objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
            //    }



            //    lstReassesment.ForEach(itemm =>
            //    {
            //        if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
            //        {
            //            Container = "(Re-Assessment)";

            //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
            //            dt = repp.GetPreviousInvDate(itemm.cfscode);
            //            dttype = "Previous Delivery Date";


            //        }

            //    });


            //    lstReassesbulk.ForEach(data =>
            //    {
            //        if (data.invoiceid == item.InvoiceId)
            //        {
            //            Container = "(Re-Assessment)";
            //            Ppg_ReportRepository repp = new Ppg_ReportRepository();
            //            dt = repp.GetPreviousInvDate(data.cfscode);
            //            dttype = "Previous Delivery Date";

            //        }
            //    });
            lstInvoice.ToList().ForEach(item =>
            {

                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }



                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Assessment No :" + item.StuffingReqNo + "</b></th></tr>");
                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'><b>Container / CBT Details :</b></th></tr>");
                
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Destuffing</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Days</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:5%;'>No of Week</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:8%;'>" + dttype + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; width:8%;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
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
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOENo).FirstOrDefault() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.BOEDate).FirstOrDefault() + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%' style='border-left: 1px solid #000;'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalSpaceOccupied.ToString("0.000") + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container / CBT Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th>");
                html.Append("<th style='width: 100px;'></th></tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> "+item.PayeeCode+"</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");                

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
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

        //[NonAction]
        //public string GeneratingBulkPDFforEmptyContainer(DataSet ds, string InvoiceModuleName, string All)
        //{
        //    var FileName = "";
        //    var location = "";
        //    CurrencyToWordINR objCurr = new CurrencyToWordINR();
        //    List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
        //    List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
        //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
        //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
        //    List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
        //    List<string> lstSB = new List<string>();
        //    List<dynamic> lstReassesmentec = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
        //    List<dynamic> lstReassesmentecbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
        //    string Container = "";
        //    string cfscode = "";





        //    lstInvoice.ToList().ForEach(item =>
        //    {
        //        VRN_ReportRepository rep = new VRN_ReportRepository();
        //        VRN_SDBalancePrint objSDBalance = new VRN_SDBalancePrint();
        //        rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
        //        if (rep.DBResponse.Data != null)
        //        {
        //            objSDBalance = (VRN_SDBalancePrint)rep.DBResponse.Data;
        //        }
        //        cfscode = "";
        //        lstContainer.ForEach(dat =>
        //        {
        //            if (dat.InoviceId == item.InvoiceId)

        //                cfscode = dat.CFSCode;


        //        });
        //        Container = "";


        //        string dt = "";
        //        string dttype = "Date of Destuffing";
        //        lstReassesmentec.ForEach(itemm =>
        //        {
        //            if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
        //            {
        //                Container = "(Re-Assessment)";

        //                VRN_ReportRepository repp = new VRN_ReportRepository();
        //                dt = repp.GetPreviousInvDate(itemm.cfscode);
        //                dttype = "Previous Delivery Date";


        //            }

        //        });


        //        lstReassesmentecbulk.ForEach(data =>
        //        {
        //            if (data.invoiceid == item.InvoiceId)
        //            {
        //                Container = "(Re-Assessment)";
        //                VRN_ReportRepository repp = new VRN_ReportRepository();
        //                dt = repp.GetPreviousInvDate(data.cfscode);
        //                dttype = "Previous Delivery Date";

        //            }
        //        });


        //        StringBuilder html = new StringBuilder();
        //        /*Header Part*/
        //        html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
        //        html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
        //        html.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
        //        html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
        //        html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
        //        html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label><label style = 'font-size: 14px; font-weight:bold;' > " + Container + "</label>");
        //        html.Append("</td></tr>");

        //        html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
        //        html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
        //        html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
        //        html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

        //        html.Append("<tr><td>");
        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
        //        html.Append("<td colspan='5' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='6' width='40%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
        //        html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");
        //        html.Append("</tr></tbody></table>");
        //        html.Append("</td></tr>");



        //        html.Append("<tr><td><hr /></td></tr>");
        //        html.Append("<tr><th><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Assessment No :" + item.StuffingReqNo + "</b> ");
        //        html.Append("<br /><b style='text-align: left; font-size: 13px;margin-top: 10px;'>Container Details :</b> </th></tr>");
        //        html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:8pt;' cellspacing='0' cellpadding='8'>");
        //        html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>SR No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>ICD Code</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Container No.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Size</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Arrival</th>");

        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + dttype + "</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Delivery</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Days</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>No of Week</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>Date of Seal Cutting</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; text-align: center;'>Cargo Type</th>");
        //        html.Append("</tr></thead><tbody>");
        //        /*************/
        //        /*Container Bind*/
        //        int i = 1;
        //        Decimal totamt = 0;
        //        lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
        //        {
        //            if (dt != "")
        //            {
        //                elem.DestuffingEntryDate = dt;
        //            }
        //            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + i.ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.CFSCode + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ContainerNo + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.Size + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.ArrivalDate + "</td>");
        //            // html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item.CartingDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.DestuffingEntryDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + item.DeliveryDate + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1)) / 7)).ToString() + "</td>");
        //            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + elem.SealCuttingDt.ToString() + "</td>");
        //            html.Append("<td style='border-bottom: 1px solid #000; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
        //            html.Append("</tr>");
        //            i = i + 1;
        //        });
        //        /***************/
        //        html.Append("</tbody></table></td></tr>");
        //        html.Append("<tr><td>");
        //        html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Shipping Line No.</td><td>:</td><td colspan='6' width='70%'></td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>OBL/HBL No.</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.OBLNo).FirstOrDefault() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>Godown Name</td><td>:</td><td colspan='6' width='70%'>" + lstCargoDetail.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.GodownName).FirstOrDefault() + "</td></tr>");

        //        html.Append("<tr><td colspan='6' width='30%'>Item No.</td><td>:</td><td colspan='6' width='70%'>" + lstContainer.Where(x => x.InoviceId == item.InvoiceId).Count().ToString() + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE No.</td><td>:</td><td colspan='6' width='70%'>" + item.BOENo + "</td></tr>");
        //        html.Append("<tr><td colspan='6' width='30%'>BOE Date</td><td>:</td><td colspan='6' width='70%'></td></tr>");
        //        html.Append("</tbody></table>");
        //        html.Append("</td>");

        //        html.Append("<td colspan='6' width='50%'>");
        //        html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
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

        //        html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td>");
        //        html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
        //        html.Append("<thead><tr>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>SR No.</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>Charge Code</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>Description</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>HSN Code</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'>Rate</th>");
        //        html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>Taxable Amt.</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>CGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>SGST</th>");
        //        html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>IGST</th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr><tr>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
        //        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
        //        html.Append("<tbody>");
        //        i = 1;
        //        /*Charges Bind*/

        //        lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
        //        {
        //            if (data.Taxable > 0)
        //            {
        //                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
        //                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0") + "</td>");
        //                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0") + "</td></tr>");
        //                i = i + 1;
        //                totamt = totamt + data.Taxable;
        //            }
        //        });
        //        html.Append("</tbody>");
        //        html.Append("</table></td></tr></tbody></table>");


        //        html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
        //        html.Append("<tbody>");

        //        html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + totamt.ToString("0") + "</th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
        //        html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0") + "</th></tr><tr>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0") + "</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0") + "</th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0") + "</th></tr>");

        //        html.Append("<tr>");
        //        html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
        //        html.Append("</tr>");
        //        html.Append("<tr>");
        //        html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
        //        html.Append("</tr>");
        //        html.Append("</tbody>");
        //        html.Append("</table>");

        //        html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
        //        html.Append("<tbody>");
        //        html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
        //        html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
        //        html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");


        //        html.Append("<tr><td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Signature CHA / Importer</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
        //        html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
        //        html.Append("</tr>");
        //        html.Append("<tr>");
        //        html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
        //        html.Append("</tr>");
        //        html.Append("</tbody></table>");

        //        html.Append("</td></tr></tbody></table>");
        //        /***************/

        //        html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
        //        lstSB.Add(html.ToString());
        //        Container = "";
        //        dt = "";
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
        //    /*if (System.IO.File.Exists(location))
        //    {
        //        System.IO.File.Delete(location);
        //    }*/
        //    using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f, false, true))
        //    {
        //        rp.HeadOffice = "";
        //        rp.HOAddress = "";
        //        rp.ZonalOffice = "";
        //        rp.ZOAddress = "";
        //        rp.GeneratePDF(location, lstSB);
        //    }
        //    return "/Docs/" + Session.SessionID + "/" + FileName;
        //}

        [NonAction]
        public string GeneratingBulkPDFforEmptyContainer(DataSet ds, string InvoiceModuleName, string All)
        {
            var BQRFileName = "";
            var BQRlocation = "";


            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();



            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            StringBuilder html = new StringBuilder();
            decimal totamt = 0;
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            lstInvoice.ToList().ForEach(item =>
            {
                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "/UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "/UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }

                /*Header Part*/
                html.Clear();
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Assessment No :" + item.StuffingReqNo + "</b></th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container Details :</b></th></tr>");                

                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>CFS Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Container No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>Date of Delivery</th>");
                html.Append("<th style='border-bottom: 1px solid #000;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> "+ item.PayeeCode+"</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");

                /***************/
                html.Replace("IMGSRC_IMG", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
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
        public string GeneratingBulkPDFforYard(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            var BQRFileName = "";
            var BQRlocation = "";

            string dtype = "Date of Arrival";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstOBL = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            //List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();

            UpiQRCodeInfo upiQRInfo = new UpiQRCodeInfo();
            upiQRInfo.ver = objCompany[0].ver;

            upiQRInfo.tn = objCompany[0].tn;
            upiQRInfo.tier = objCompany[0].tier;
            upiQRInfo.tid = objCompany[0].tid;
            upiQRInfo.qrMedium = Convert.ToInt32(objCompany[0].qrMedium);
            //  upiQRInfo.QRexpire = objCompany[0].QRexpire;
            upiQRInfo.pn = objCompany[0].pn;
            upiQRInfo.pa = objCompany[0].pa;
            // upiQRInfo.orgId = objCompany[0].orgId;
            upiQRInfo.mtid = objCompany[0].mtid;
            upiQRInfo.msid = objCompany[0].msid;
            upiQRInfo.mode = Convert.ToInt32(objCompany[0].mode);
            upiQRInfo.mid = objCompany[0].mid;
            upiQRInfo.mc = Convert.ToString(objCompany[0].mc);
            upiQRInfo.gstIn = Convert.ToString(objCompany[0].GstIn);
            upiQRInfo.merchant_id = Convert.ToInt32(objCompany[0].ccavenuemid);

            string Container = "";
            string cfscode = "";
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            //lstReasses.ForEach(item =>
            //{
            //    if (item.cfscode != "")
            //        Container = "(Re-Assessment)";

            //});

            lstInvoice.ToList().ForEach(item =>
            {
                VRN_ReportRepository rep = new VRN_ReportRepository();
                VRN_SDBalancePrint objSDBalance = new VRN_SDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (VRN_SDBalancePrint)rep.DBResponse.Data;
                }



                lstContainer.ForEach(dat =>
                {
                    if (dat.InoviceId == item.InvoiceId)

                        cfscode = dat.CFSCode;
                    else

                        cfscode = "";

                });
                Container = "";

                IrnResponse objERes = new IrnResponse();
                //if (item.SupplyType == "B2C")
                //{
                BQRFileName = item.InvoiceId + ".jpg";
                BQRlocation = Server.MapPath("~/Docs/") + "BQR/" + BQRFileName;
                log.Error("Path:" + BQRlocation);
                if (!System.IO.File.Exists(BQRlocation))
                {


                    upiQRInfo.order_id = Convert.ToInt32(item.InvoiceId);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.billing_name = Convert.ToString(item.PartyName);
                    upiQRInfo.billing_address = Convert.ToString(item.PartyAddress);
                    upiQRInfo.billing_zip = Convert.ToString(item.PinCode);
                    upiQRInfo.billing_tel = Convert.ToString(item.MobileNo);
                    upiQRInfo.billing_email = Convert.ToString(item.Email);

                    upiQRInfo.InvoiceDate = Convert.ToString(item.DocDt);
                    upiQRInfo.invoiceNo = Convert.ToString(item.InvoiceNo);
                    upiQRInfo.InvoiceName = Convert.ToString(item.PartyName);
                    upiQRInfo.mam = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.am = Convert.ToDecimal(item.InvoiceAmt);
                    upiQRInfo.CGST = Convert.ToDecimal(item.CGST);
                    upiQRInfo.SGST = Convert.ToDecimal(item.SGST);
                    upiQRInfo.IGST = Convert.ToDecimal(item.IGST);
                    upiQRInfo.GSTPCT = Convert.ToDecimal(item.IGSTPer);
                    upiQRInfo.QRexpire = Convert.ToString(item.DocDt);
                    upiQRInfo.tr = Convert.ToString(item.InvoiceId);
                    string Site = System.Configuration.ConfigurationManager.AppSettings["WebsiteUrl"];

                    upiQRInfo.redirect_url = Site + "UPIResponse/GetResponseBqr";
                    upiQRInfo.cancel_url = Site + "UPIResponse/CancelResponseBqr";
                    upiQRInfo.merchant_param1 = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["BranchId"].ToString());


                    Einvoice Eobj = new Einvoice();
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode_Wfld(upiQRInfo);



                    if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + "BQR"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + "BQR");
                    }

                    string strm = objresponse.QrCodeBase64;

                    //this is a simple white background image
                    var myfilename = string.Format(@"{0}", Guid.NewGuid());

                    //Generate unique filename
                    //  string filepath = Server.MapPath("~/Docs/QRCode/") + myfilename + ".jpeg";
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(BQRlocation, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }

                    VRN_ReportRepository wfldObj = new VRN_ReportRepository();
                    wfldObj.AddEditBQRCode(item.InvoiceId, BQRFileName, ((Login)(Session["LoginUser"])).Uid);


                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><span style='font-size: 7pt;'><b>Phone No:</b> " + objCompany[0].PhoneNo + " </span><br/><label style='font-size: 7pt;'>Email - " + objCompany[0].EmailAddress + "</label><br /><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("<td width='60%' valign='top'><img width='80px' align='right' src='SWACHBHARAT'/></td></tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                if (item.SupplyType == "B2C")
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                }
                else if (!string.IsNullOrEmpty(item.SignedQRCode))
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; width:100%;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td colspan='12'><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td colspan='12'>");
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
                html.Append("<tr><th colspan='6' width='35%'>Invoice Generated By</th><th>:</th><td colspan='6' width='65%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='35%'>Supply Type</th><th>:</th><td colspan='6' width='65%'>" + item.SupplyType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: left; font-size: 7pt; margin-top: 10px;'><b>Assessment No :</b> " + item.StuffingReqNo + "</th></tr>");
                html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 7pt; margin-top: 10px;'>Container/CBT Details :</b> </th></tr>");
                
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt; text-align:center;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:30px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:40px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>" + dtype + " </th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Weight</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width:80px;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; width:80px;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                //string OblNo = "";
                string RMS = "";
                string Movement = "";
                int i = 1;
                int flagvalue = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    //get no of days and weeks for IMPYard Payment Sheet
                    VRN_DaysWeeks objDaysWeeks = new VRN_DaysWeeks();

                    if (Container == "(Re-Assessment)")
                    {
                        elem.ArrivalDate = item.StuffingReqDate;
                        flagvalue = 1;
                    }

                    rep.GetDaysWeeksForIMPYard(item.InvoiceId, elem.CFSCode, flagvalue);
                    if (rep.DBResponse.Data != null)
                    {
                        objDaysWeeks = (VRN_DaysWeeks)rep.DBResponse.Data;

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
                    
                    //OblNo = OblNo + elem.OBL_No + ".";
                    RMS = elem.RMS;
                    Movement = elem.Movement;
                    i = i + 1;
                });
                string OblNo = "";
                if(ds.Tables[6].Rows.Count>0)
                {
                    OblNo = ds.Tables[6].Rows[0]["OBL_No"].ToString();
                }
               
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Importer</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Value</td><td>:</td><td colspan='6' width='70%'>" + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(m => (decimal)m.CIFValue)).ToString("0.00") + " + DUTY : " + (lstContainer.Where(z => z.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + " = " + (lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.CIFValue) + lstContainer.Where(m => m.InoviceId == item.InvoiceId).Sum(z => (decimal)z.Duty)).ToString("0.00") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Area (In Sqr Mtr)</td><td>:</td><td colspan='6' width='70%'></td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Movement</td><td>:</td><td colspan='6' width='70%'>" + Movement + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom: 0;'>Container/CBT Charges :</h3> </th></tr>");
                html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'>");
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
                decimal totalamt = 0;
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
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
                    totalamt = totalamt + data.Taxable;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border: 1px solid #000; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 100px; text-align:left;'>Total :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px; text-align:right;'>" + totalamt.ToString("0.00") + "</th>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; border-top:0; width:100%; font-size:6pt; text-align: center;' cellspacing='0' cellpadding='5'> ");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='width: 150px; text-align:left;'>Round Up :</th>");
                html.Append("<th rowspan='2' style='width: 24%;'></th>");
                html.Append("<th rowspan='2' style='width: 100px;'></th>");
                html.Append("<th rowspan='2' style='width: 80px;'></th>");
                html.Append("<th rowspan='2' style='width: 130px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th colspan='2' style='width: 200px;'></th>");
                html.Append("<th rowspan='2' style='width: 100px; text-align:right;'>" + item.RoundUp.ToString("0.00") + "</th></tr><tr>");
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

                html.Append("<table style='border: 1px solid #000; border-top:0; width: 100%; text-align:left; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width: 100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td colspan='12'><b>Payer Name : </b> " + item.PayeeName + "</td></tr>");
                html.Append("<tr><td colspan='12'><b>Payer Code : </b> " + item.PayeeCode + "</td></tr>");
                html.Append("<tr><td colspan='2' width='10%' valign='top'><b>Remarks : </b></td><td colspan='9' width='90%' align='left'>" + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width:100%; font-size:6pt; text-align:center;' cellspacing='0' cellpadding='5'> <tbody>");
                html.Append("<tr><th colspan='3' width='25%'><br/><br/><br/><br/>Signature CHA / Importer</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/AM Accounts<br/>(Signature)</th>");
                html.Append("<th colspan='3' width='25%'><br/><br/><br/><br/>SAM/SIO <br/>(Signature)</th>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr></tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + BQRlocation + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
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
        public ActionResult ListOfInvoiceDateWise(string FromDate, string ToDate, string invoiceType,int PartyId)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<invoiceLIst> LstinvoiceLIst = new List<invoiceLIst>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.GetInvoiceList(FromDate, ToDate, invoiceType, PartyId);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {               
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
            VRN_ReportCWCController obj = new VRN_ReportCWCController();
            
            string fileSavePath = "";
            fileSavePath = Server.MapPath("~/Docs/All/") + Session.SessionID;
            var filesCol = obj.GetFile(fileSavePath).ToList();
            string FileList = "";
            
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
        public string LoadImage(string img)
        {
            ////data:image/gif;base64,
            ////this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String(img);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            // image = Image.FromStream(ms);
            //}






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

        public string ResizeImage(String Source, String Target)
        {
            // string imgPath = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\Images1\49dc6397-31bb-43d7-b4d0-2db8f04f57c8.png";
            // string imgPathTarget = @"D:\TESTPROJECT\Einvoice\CwcExim\CwcExim\Content\InvQrcode2.png";
            Bitmap source = new Bitmap(Source);
            Bitmap t = CropWhiteSpace(source);
            t.Save(Target);

            return (Target);
        }
        public Bitmap CropWhiteSpace(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int white = 0xffffff;

            Func<int, bool> allWhiteRow = r =>
            {
                for (int i = 0; i < w; ++i)
                    if ((bmp.GetPixel(i, r).ToArgb() & white) != white)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = c =>
            {
                for (int i = 0; i < h; ++i)
                    if ((bmp.GetPixel(c, i).ToArgb() & white) != white)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (!allWhiteRow(row))
                    break;
                topmost = row;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (!allWhiteRow(row))
                    break;
                bottommost = row;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (!allWhiteColumn(col))
                    break;
                leftmost = col;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (!allWhiteColumn(col))
                    break;
                rightmost = col;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                    new RectangleF(0, 0, croppedWidth, croppedHeight),
                    new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                    GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                ex);
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
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
               
                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");


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
                
                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='150%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>CASH RECEIPT</label>");
                html.Append("</td></tr>");                

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
                
                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");

                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>SD RECEIPT</label>");
                html.Append("</td></tr>");
                

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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_DailyCashBook> LstDailyCashBook = new List<VRN_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.DailyCashBook(PeriodFrom, PeriodTo);
            if (ObjRR.DBResponse.Data != null)
            {
                LstDailyCashBook = (List<VRN_DailyCashBook>)ObjRR.DBResponse.Data;
                LstDailyCashBook = LstDailyCashBook.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                LstDailyCashBook.Add(new VRN_DailyCashBook()
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
                    //Reefer = LstDailyCashBook.ToList().Sum(o => o.Reefer),
                    //EscCharge = LstDailyCashBook.ToList().Sum(o => o.EscCharge),
                    //Print = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    //Royality = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    //HT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.HT)),

                    CstmExam = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CstmExam)),
                    Weighment = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.Weighment)),
                    CstmCl = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CstmCl)),
                    CBSC = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.CBSC)),
                    //EPCCh = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.EPCCh)),
                    OT = LstDailyCashBook.ToList().Sum(o => Convert.ToDecimal(o.OT)),

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
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/DailyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 40f, 40f, 40f, 40f, false, true))
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
        public ActionResult GetMonthlyCashBookReport(VRN_MonthlyCashBook ObjMonthlyCashBook)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_DailyCashBook> LstDailyCashBook = new List<VRN_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthlyCashBook(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {

                VRN_MonthlyCashBook VRNCashBook = new VRN_MonthlyCashBook();
                VRNCashBook = (VRN_MonthlyCashBook)ObjRR.DBResponse.Data;

                //  LstDailyCashBook = (List<VRN_DailyCashBook>)ObjRR.DBResponse.Data;
                VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.ToList();
             //   VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                VRNCashBook.lstCashReceipt.Add(new VRN_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Area),
                    OTCharge = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.OTCharge),
                    Weighment = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Weighment),
                    Other = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Other),

                    // STO = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.STO),
                    //INS = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.INS),
                    //  GRE = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRE),
                    // GRL = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRL),
                    // Reefer = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Reefer),
                    //  EscCharge = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.EscCharge),
                    // Print = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    //  Royality = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    //Franchiese = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    // HT = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.HT)),
                    //  EGM = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    // Documentation = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.Taxable))),
                    Cgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = VRNCashBook.lstCashReceipt.ToList().Sum(o =>(Convert.ToDecimal(o.Igst))),
                    TCgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TCgst)),
                    TSgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TSgst)),
                    TIgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.TIgst))),
                    TCGSTAmtSGSTAmt = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.TCGSTAmtSGSTAmt))),
                    CGSTAmtSGSTAmt = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.CGSTAmtSGSTAmt))),

                    Roundoff = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    TotalDay = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalDay)),
                    TotalBank = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalBank)),
                    BankCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.BankCash)),
                    BankChq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.BankChq)),
                    TotalSD = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalSD)),
                    OpCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpCash)),
                    OpChq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpChq)),
                    //cloCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.cloCash)),
                    //clochq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.clochq)),

                    cloCash = VRNCashBook.lstCashReceipt[VRNCashBook.lstCashReceipt.Count-1].cloCash,
                    clochq = VRNCashBook.lstCashReceipt[VRNCashBook.lstCashReceipt.Count - 1].clochq,
                    

                    Total = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Total)),

                    Tds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.CrTds)),
                    ReliazationagainstBilling = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.ReliazationagainstBilling))

                });
                ObjRR.DBResponse.Data = VRNCashBook;
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
                Pages[0] = fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/MonthlyCashBookReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A3Landscape, 20f, 20f, 20f, 20f, false, true))
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


        [HttpGet]
        public ActionResult MonthlyCashBookReportOld()
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
        public ActionResult GetMonthlyCashBookReportOld(VRN_MonthlyCashBook ObjMonthlyCashBook)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_DailyCashBook> LstDailyCashBook = new List<VRN_DailyCashBook>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.MonthlyCashBookOLD(ObjMonthlyCashBook);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {

                VRN_MonthlyCashBook VRNCashBook = new VRN_MonthlyCashBook();
                VRNCashBook = (VRN_MonthlyCashBook)ObjRR.DBResponse.Data;

                //  LstDailyCashBook = (List<VRN_DailyCashBook>)ObjRR.DBResponse.Data;
                VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.ToList();
               // VRNCashBook.lstCashReceipt = VRNCashBook.lstCashReceipt.OrderBy(o => o.ReceiptDate).ThenBy(o => o.CRNo).ToList();
                VRNCashBook.lstCashReceipt.Add(new VRN_DailyCashBook()
                {
                    ReceiptDate = "<strong>Total</strong>",
                    CRNo = "",
                    Depositor = "",
                    ChqNo = "",
                    Area = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Area),
                    STO = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.STO),
                    INS = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.INS),
                    GRE = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRE),
                    GRL = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.GRL),
                    Reefer = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.Reefer),
                    EscCharge = VRNCashBook.lstCashReceipt.ToList().Sum(o => o.EscCharge),
                    Print = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Print)),
                    Royality = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Royality)),
                    Franchiese = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Franchiese)),
                    HT = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.HT)),
                    EGM = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.EGM)),
                    Documentation = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Documentation)),
                    Taxable = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.Taxable))),
                    Cgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Cgst)),
                    Sgst = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Sgst)),
                    Igst = VRNCashBook.lstCashReceipt.ToList().Sum(o => (Convert.ToDecimal(o.Igst))),
                    Roundoff = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Roundoff)),
                    TotalCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCash)),
                    TotalCheque = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalCheque)),
                    TotalDay = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalDay)),
                    TotalBank = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalBank)),
                    BankCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.BankCash)),
                    BankChq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.BankChq)),
                    TotalSD = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.TotalSD)),
                    OpCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpCash)),
                    OpChq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.OpChq)),
                    cloCash = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.cloCash)),
                    clochq = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.clochq)),

                    Tds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.Tds)),
                    CrTds = VRNCashBook.lstCashReceipt.ToList().Sum(o => Convert.ToDecimal(o.CrTds))

                });
                ObjRR.DBResponse.Data = VRNCashBook;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
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
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetLiveBondReport(AsOnDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<VRN_TimeBarredReport> lstData = new List<VRN_TimeBarredReport>();
                lstData = (List<VRN_TimeBarredReport>)objRR.DBResponse.Data;
                Path = GenerateLiveBondReport(lstData, AsOnDate);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }

        [NonAction]
        public string GenerateLiveBondReport(List<VRN_TimeBarredReport> lstData, string AsOnDate)
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
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
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
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetTimeBarredReport(AsOnDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<VRN_TimeBarredReport> lstData = new List<VRN_TimeBarredReport>();
                lstData = (List<VRN_TimeBarredReport>)objRR.DBResponse.Data;
                Path = GenerateTimeBarredReport(lstData, AsOnDate);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }

        [NonAction]
        public string GenerateTimeBarredReport(List<VRN_TimeBarredReport> lstData, string AsOnDate)
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
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
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
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
                    var reportRepo = new VRN_ReportRepository();
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
            var reportRepo = new VRN_ReportRepository();
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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetEconomyReport(Month, Year);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEconomyReport(int Month, int Year, List<EconomyReport> lstRptData)
        {
            try
            {
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();

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
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
        public JsonResult GetSACRegister(VRN_SAC objSAC)
        {
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetSACDetails(objSAC.PeriodFrom, objSAC.PeriodTo);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                List<VRN_SACRegister> lstSAC = new List<VRN_SACRegister>();
                lstSAC = (List<VRN_SACRegister>)objRR.DBResponse.Data;
                Path = GetSACHtml(lstSAC);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });
        }
        [NonAction]
        public string GetSACHtml(List<VRN_SACRegister> lstSAC)
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
            sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
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
            sb.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

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
            VRN_ReportRepository objRR = new VRN_ReportRepository();
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
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetBondRegister(DepositeAppId, FromDate, ToDate);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                VRN_BondRegister objBR = new VRN_BondRegister();
                objBR = (VRN_BondRegister)objRR.DBResponse.Data;
                Path = GetBondHtml(objBR, DepositeAppId);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });
        }
        [NonAction]
        public string GetBondHtml(VRN_BondRegister objBR, int DepositeAppId)
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
                sb.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
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
                sb.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
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
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetGSTMISReport(PeriodFrom, PeriodTo);
            string Path = "";
            if (objRR.DBResponse.Data != null)
            {
                VRN_GSTMISReport lstGSTMIS = new VRN_GSTMISReport();
                lstGSTMIS = (VRN_GSTMISReport)objRR.DBResponse.Data;
                Path = GetGSTMISHtml(lstGSTMIS, PeriodFrom, PeriodTo);
                return Json(new { Message = Path, Status = 1 });
            }
            return Json(new { Message = "", Status = 0 });

        }



        public string GetGSTMISHtml(VRN_GSTMISReport lstGSTMIS, string PeriodFrom, string PeriodTo)
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
            sb.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
                var Type = fc["ddlType"].ToString();
                var excelName = "";
                var ObjRR = new VRN_ReportRepository();
                ObjRR.GetRegisterofOutwardSupplyVRN(date1, date2, Type);

                if (Type == "Inv") { Type = "Invoice"; }
                if (Type == "C") { Type = "Credit"; }
                if (Type == "D") { Type = "Debit"; }
                if (Type == "Unpaid") { Type = "Unpaid"; }
                if (Type == "CancelInv") { Type = "Cancel Invoice"; }

                excelName = "RegisterofOutwardSupply" + "_" + Type + ".xls";

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
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterofOutwardSupply.xls");
                }
            }
            // return null;
        }

        #endregion

        #region DetailsOfContainere
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
                    VRN_ReportRepository objR = new VRN_ReportRepository();
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
                        Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                        if (!Directory.Exists(LocalDirectory))
                        {
                            Directory.CreateDirectory(LocalDirectory);
                        }
                        if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                        {
                            System.IO.File.Delete(LocalDirectory + "/" + FileName);
                        }
                        //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                        //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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

        #region PV Report
        [HttpGet]
        public ActionResult PVReport()
        {
            HDBMasterRepository ObjGR = new HDBMasterRepository();
            ObjGR.GetAllGodown();
            if (ObjGR.DBResponse.Data != null)
                ViewBag.ListOfGodown = ObjGR.DBResponse.Data;
            else ViewBag.ListOfGodown = null;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPVReport(VRN_PV ObjPV)
        {
            if (ModelState.IsValid)
            {
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();
                if (ObjPV.Module == "Import")
                    ObjRR.GetPVReportImport(ObjPV);
                else if (ObjPV.Module == "Export")
                    ObjRR.GetPVReportExport(ObjPV);
                else
                    ObjRR.GetPVReportBond(ObjPV);
                string Path = "";
                if (ObjRR.DBResponse.Data != null && ObjPV.Module == "Import")
                {
                    List<VRN_ImpPVReport> lstData = new List<VRN_ImpPVReport>();
                    lstData = (List<VRN_ImpPVReport>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFImpPVReport(lstData, ObjPV.AsOnDate, ObjPV.GodownName);
                }
                else if (ObjRR.DBResponse.Data != null && ObjPV.Module == "Export")
                {
                    List<VRN_ExpPVReport> lstData = new List<VRN_ExpPVReport>();
                    lstData = (List<VRN_ExpPVReport>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFPExpPVReport(lstData, ObjPV.AsOnDate, ObjPV.GodownName);
                }
                else
                {
                    List<VRN_BondPVReport> lstData = new List<VRN_BondPVReport>();
                    lstData = (List<VRN_BondPVReport>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFBondPVReport(lstData, ObjPV.AsOnDate, ObjPV.GodownName);
                }
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFImpPVReport(List<VRN_ImpPVReport> lstData, string Date, string GodownName)
        {
            try
            {
                var FileName = "PVReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 12px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + ZonalOffice +"</span><br/><span style='font-size: 12px; padding-bottom: 10px;'>"+ZOAddress+"</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label>");
                Pages.Append("<br /><label style='font-size: 14px; font-weight:bold;'>Physical Verification Report for Import Cargo</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");


                Pages.Append("<tr>");
                Pages.Append("<td colspan='6' style='font-size:12px; width:50%;'><b>As On Date - </b> " + Date + "</td>");
                Pages.Append("<td colspan='6' style='font-size:12px; width:50%; text-align:right;'><b>Shed Cd - </b>" + GodownName + "</td>");
                Pages.Append("</tr>");

                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'><thead>");
                Pages.Append("<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>S No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>OBL No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Date of Arrival</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Dstf Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>ICS Code</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>LCL/FCL</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:15%;'>Importer</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:6%;'>Item No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>No Pkg</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Rcvd Pkg</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Gr W</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Area</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Slot No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:15%;'>Remarks</th></tr>");
                Pages.Append("</thead>");
                Pages.Append("<tbody>");

                lstData.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BOLNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.EntryDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.DestuffingEntryDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CFSCode + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.LCLFCL + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Importer + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CommodityAlias + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.NoOfUnits + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.NoOfUnitsRec + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Weight.ToString("0.00") + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Area.ToString("0.00") + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.LocationName + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;'>" + item.Remarks + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='8' style='text-align:left;border-right:1px solid #000;'>Total :</th>");
                Pages.Append("<th style='text-align:left;border-right:1px solid #000;'>" + lstData.Sum(x => x.NoOfUnits) + "</th>");
                Pages.Append("<th style='text-align:left;border-right:1px solid #000;'>" + lstData.Sum(x => x.NoOfUnitsRec) + "</th>");
                Pages.Append("<th style='text-align:left;border-right:1px solid #000;'>" + lstData.Sum(x => x.Weight).ToString("0.00") + "</th>");
                Pages.Append("<th style='text-align:left;border-right:1px solid #000;'>" + lstData.Sum(x => x.Area).ToString("0.00") + "</th>");
                Pages.Append("<th colspan='2' style='text-align:left;border-right:1px solid #000;'></th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
               
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
        [NonAction]
        public string GeneratePDFPExpPVReport(List<VRN_ExpPVReport> lstData, string Date, string GodownName)
        {
            try
            {
                var FileName = "PVReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 28px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 14px;'>(A Govt. of India Undertaking) </label><br />");
                //Pages.Append("<span style='font-size: 14px; padding-bottom: 10px;'>Container Freight Station, Kukatpally</span><br/><span style='font-size: 14px; padding-bottom: 10px;'>IDPL Road, Hyderabad - 37</span><br/><label style='font-size: 14px;'>Email - cwccfs@gmail.com</label>");
                Pages.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + ZonalOffice + "</span><br/><span style='font-size: 12px; padding-bottom: 10px;'>" + ZOAddress + "</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label>");
                Pages.Append("<br /><label style='font-size: 16px; font-weight:bold;'>Physical Verification Report for Export Cargo</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("<tr>");
                Pages.Append("<td colspan='6' style='font-size:14px; width:50%;'><b>Stock Register of Shed No...</b> " + GodownName + "</td>");
                Pages.Append("<td colspan='6' style='font-size:14px; width:50%; text-align:right;'><b>As on Date...</b> " + Date + "</td>");
                Pages.Append("</tr>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'>");
                Pages.Append("<thead><tr><td colspan='12'><table style='border-top:1px solid #000;border-left:1px solid #000;border-right:1px solid #000;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
                Pages.Append("<th style='text-align:left;padding:10px;width:6%;'>Sla Cd</th> ");
                Pages.Append("<th style='text-align:left;padding:10px;width:3%;'>Sr</th> ");
                Pages.Append("<th style='text-align:left;padding:10px;width:6%;'>Sb No</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:8%;'>Sb Date</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:12%;'>CHA Name</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:12%;'>Exporter Name</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:10%;'>Entry No</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:10%;'>Entry Date</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:8%;'>Carting Date</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:10%;'>Cargo Description</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:5%;'>No Pkg</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:8%;'>Gr Wt</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:10%;'>Fob</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:6%;'>Slot No</th>");
                Pages.Append("<th style='text-align:left;padding:10px;width:7%;'>Area</th>");
                Pages.Append("</tr></tbody></table></td></tr></thead>");

                Pages.Append("<tbody>");
                //Loop
                lstData.Select(x => new { ShippingLineId = x.ShippingLineId, EximTraderName = x.EximTraderName, EximTraderAlias = x.EximTraderAlias }).Distinct().ToList().ForEach(x =>
                {
                    Pages.Append("<tr><td colspan='12' style='padding:0;'>");
                    Pages.Append("<table style='margin:0 0 15px;border:1px solid #000;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
                    Pages.Append("<tr><td colspan='12'>");
                    Pages.Append("<table style='margin:0;width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
                    Pages.Append("<th style='text-align:left;padding:10px;width:5.5%;'>" + x.EximTraderAlias + "</th> ");
                    Pages.Append("<td style='text-align:left;padding:10px;width:100%;'>" + x.EximTraderName + "</td> ");
                    Pages.Append("</tr></tbody></table>");
                    Pages.Append("</td></tr>");
                    //Loop
                    int i = 1;
                    lstData.Where(y => y.ShippingLineId == x.ShippingLineId).ToList().ForEach(y =>
                    {
                        Pages.Append("<tr><td colspan='12'>");
                        Pages.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:6%;'></td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:3%;'>" + i + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:6%;'>" + y.ShippingBillNo + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:8%;'>" + y.ShippingBillDate + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:12%;'>" + y.ChaName + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:12%;'>" + y.ExporterName + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:10%;'>" + y.EntryNo + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:10%;'>" + y.EntryDate + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:8%;'>" + y.RegisterDate + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:10%;'>" + y.CargoDescription + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:5%;'>" + y.Units + "</td> ");
                        Pages.Append("<td style='text-align:left;padding:10px;width:8%;'>" + y.Weight.ToString("0.00") + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:10%;'>" + y.Fob + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:6%;'>" + y.LocationName + "</td>");
                        Pages.Append("<td style='text-align:left;padding:10px;width:7%;'>" + y.Area.ToString("0.00") + "</td>");
                        Pages.Append("</tr></tbody></table>");
                        Pages.Append("</td></tr>");
                        i++;
                    });
                    //loop end

                    Pages.Append("<tr><td colspan='12'>");
                    Pages.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:6%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:3%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:8%;'>Total :</th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:8%;'>" + lstData.Where(z => z.ShippingLineId == x.ShippingLineId).Count() + "</th> ");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:10%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:10%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:8%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:10%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:7%;'>" + lstData.Where(z => z.ShippingLineId == x.ShippingLineId).Sum(z => z.Units) + "</th> ");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:9%;'>" + lstData.Where(z => z.ShippingLineId == x.ShippingLineId).Sum(z => z.Weight).ToString("0.00") + "</th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:13%;'>" + lstData.Where(z => z.ShippingLineId == x.ShippingLineId).Sum(z => z.Fob) + "</th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:6%;'></th>");
                    Pages.Append("<th style='text-align:left;border-top:1px solid #000;padding:10px;width:7%;'>" + lstData.Where(z => z.ShippingLineId == x.ShippingLineId).Sum(z => z.Area).ToString("0.00") + "</th>");
                    Pages.Append("</tr></tbody></table>");
                    Pages.Append("</td></tr>");

                    Pages.Append("</tbody></table>");
                    Pages.Append("</td></tr>");
                });
                //loop  end

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:6%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:3%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:8%;'>Total :</th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:8%;'>" + lstData.Count() + "</th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:10%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:10%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:8%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:10%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:7%;'>" + lstData.Sum(x => x.Units) + "</th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:9%;'>" + lstData.Sum(x => x.Weight).ToString("0.00") + "</th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:13%;'>" + lstData.Sum(x => x.Fob) + "</th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:8%;'></th>");
                Pages.Append("<th style='text-align:left;border-top:1px solid #000;border-bottom:1px solid #333;padding:10px;width:5%;'>" + lstData.Sum(x => x.Area).ToString("0.00") + "</th>");
                Pages.Append("</tr></tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("</tbody>");
                Pages.Append("</table>");
                Pages.Append("</td></tr>");
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
               
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

        public string GeneratePDFBondPVReport(List<VRN_BondPVReport> lstData, string Date, string GodownName)
        {
            try
            {
                var FileName = "PVReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                decimal Value = 0, Duty = 0, Total = 0;


                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");

                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 12px;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + lstData[0].CompAddress + "</span><br/><label style='font-size: 12px;'>Email: -" + lstData[0].CompEmail + "</label><br/><label style='font-size: 12px;'>PV STATEMENT OF LIVE BONDS IN GODOWN NO : " + GodownName + " AS ON " + Date + "</label>");
                Pages.Append("<br /><label style='font-size: 14px; font-weight:bold;'>GODOWN NO : " + GodownName + "</label>");
                Pages.Append("</td>");
                Pages.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");

                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'><thead>");
                Pages.Append("<tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>SL. NO</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>BOND NO</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>BOND DATE</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>NAME OF THE IMPORTER</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>DESCRIPTION OF GOODS</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>NO OF PKGS</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>WT.IN. KGS</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>ACCESIBLE VALUE</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>DUTY</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>TOTAL</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>AREA IN SQ. MT</th><th style='border-bottom:1px solid #000;border-right:1px solid #000;'>CHA</th><th style='border-bottom:1px solid #000;'>Remarks</th></tr>");
                Pages.Append("</thead>");
                Pages.Append("<tbody>");

                lstData.ForEach(item =>
                {

                    Value = item.Value;
                    Duty = item.Duty;
                    Total = Math.Round((Value + Duty), 2);


                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.BondDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Importer + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.ItemDesc + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Units + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Weight + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Value + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Duty + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + (item.Value + item.Duty) + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.Area + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;'>" + item.CHA + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;'></td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='5' style='border-bottom:1px solid #000;text-align:left;'>Total :</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Units) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Weight) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Value) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Duty) + "</th>");
                Pages.Append("<th style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + (lstData.Sum(x => x.Value) + lstData.Sum(x => x.Duty)) + "</th>");
                Pages.Append("<th colspan='3' style='text-align:left;border-bottom:1px solid #000;border-right:1px solid #000;'>" + lstData.Sum(x => x.Area) + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                
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

        #region PDSummary



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult PDASummaryReport()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPdSummaryReport(VRN_PdSummary ObjPdSummary, int drpType)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_PdSummary> LstPdSummary = new List<VRN_PdSummary>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PdSummaryReport(ObjPdSummary, drpType);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstPdSummary = (List<VRN_PdSummary>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePdSummaryPDF(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "SdSummary.pdf";
                Pages[0] = fc["Page"].ToString();
                
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/SdSummary/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SdSummary/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion

        #region SD Details Report

        public ActionResult SDDetailsReport()
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet("", 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstParty = Jobject["LstParty"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }


        [HttpGet]
        public JsonResult SearchParty(string PartyCode)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadParty(string PartyCode, int Page)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForSDDet(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSDDetReport(string FromDate, string ToDate, int PartyId)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();

            string Fdt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetSDDetStatement(PartyId, Fdt, Tdt);
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                VRN_SDDetailsStatement SDData = new VRN_SDDetailsStatement();
                SDData = (VRN_SDDetailsStatement)ObjRR.DBResponse.Data;

                Path = GeneratePDFSDDetReport(SDData, FromDate, ToDate);
            }
            return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public string GeneratePDFSDDetReport(VRN_SDDetailsStatement SDData, string FromDate, string ToDate)
        {
            try
            {
                var FileName = "SdReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                //***************************************************************************************

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + SDData.CompanyAddress + "</span><br/><label style='font-size: 14px; font-weight:bold;'>SD STATEMENT</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append(" </td></tr>");
                Pages.Append("</thead>");

                Pages.Append(" <tbody>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Party :</b>" + SDData.PartyName + "</td></tr>");
                Pages.Append(" <tr><td colspan='12' style='font-size:12px;'><b>Folio No :</b> " + SDData.PartyCode + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>CWC GST No :</b> " + SDData.CompanyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Party GST No :</b> " + SDData.PartyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Period :</b> " + FromDate + " - " + ToDate + "</td></tr>");
                Pages.Append("</tbody></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>Sl. No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>Invoice No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Invoice Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>Receipt No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Receipt Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Pay Receipt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Transaction Type</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:12%;'>Transaction Amount</th></tr></thead>");
                Pages.Append("<tbody>");
                SDData.lstInvc.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>" + item.InvoiceNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.InvoiceDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>" + item.ReceiptNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.ReceiptDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:right;'>" + item.ReceiptAmt.ToString() + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.TranType + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.TranAmt.ToString() + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });
                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='border-right:1px solid #000;width:8%;text-align:right;'>Balance :</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + SDData.UtilizationAmount + "</th>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse;font-size:9pt;'><tbody>");
                Pages.Append("<tr><td><span><br/><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12'><p><b>N.B :</b> This is a computer generated statement. Doesn't require any signature.</p></td></tr>");
                Pages.Append("</tbody></table>");

                //***************************************************************************************
                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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

        #region Bill Cum SD Ledger
        public ActionResult BillCumSDLedger()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]),
                Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ImportRepository objImport = new ImportRepository();
            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }


        [HttpPost]
        public JsonResult GetBillCumSDLedgerReport(int partyId, string fromdate, string todate)
        {
            try
            {
                VRN_ReportRepository Repo = new VRN_ReportRepository();
                Repo.GetBillCumSDLedgerReport(partyId, Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd"), Convert.ToDateTime(todate).ToString("yyyy-MM-dd"), ZonalOffice, ZOAddress);
                object obj = Repo.DBResponse;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Data = "", Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region SD A/c Statement

        [HttpGet]
        public ActionResult SDStatement()
        {
            var Months = new Dictionary<int, string>();
            for (Int16 i = 1; i <= 12; i++)
            {
                Months.Add(i, System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames[i - 1]);
            }
            ViewBag.Months = new SelectList((IEnumerable)Months, "Key", "Value", DateTime.Today.Month);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetSDStatement(ChnSDStatement ObjSDStatement)
        {
            if (ModelState.IsValid)
            {
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();
                ObjRR.GetPDAStatement(ObjSDStatement.Month, ObjSDStatement.Year);
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePDFForPDAStatement(FormCollection Fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "SDStatement.pdf";
                Pages[0] = Fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report/SDStatement/";
                Pages[0] = Pages[0].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                
                return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SDStatement/" + FileName });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        #endregion

        #region SD Opening Print
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetBulkCashreceiptForSDOpening(BulkReceiptReport ObjBulkReceiptReport)
        {
           VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetBulkCashreceiptForSDOpening(ObjBulkReceiptReport.PeriodFrom, ObjBulkReceiptReport.PeriodTo, ObjBulkReceiptReport.ReceiptNumber);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateBulkSDOpening(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }


        [NonAction]
        public string GenerateBulkSDOpening(DataSet ds)
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
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICS Code</th>");
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
                html.Append("<tbody>");

                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:12px;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:12px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 12px;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 16px; font-weight:bold;'>Cash Receipt</label></td>");
                html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                //Header
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td colspan='12' style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>No.</label> <span>" + item.ReceiptNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Date : </label> <span>" + item.ReceiptDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>By : </label><span>" + item.PartyName + "</span></td></tr></tbody></table>");
                html.Append("</td></tr><tr><td colspan='12'><hr/></td></tr><tr><td colspan='12'>");

                //Invoice Nos and Amounts
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Invoice No</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>Importer/Exporter</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>Invoice Type</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>Amount</th>");
                html.Append("</tr></thead><tbody>");

                //Loop
                var InvoiceIds = item.InvoiceId.Split(',');
                var InvoiceNos = item.InvoiceNo.Split(',');
                var ImpExpName = item.ExporterImporterName.Split(',');
                var InvType = item.InvType.Split(',');
                var InvoiceAmts = item.Amt.Split(',');
                i = 0;
                foreach (var Invoice in InvoiceNos)
                {
                    html.Append("<tr>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>" + InvoiceNos[i] + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 140px;'>" + ImpExpName[i] + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 60px;'>" + InvType[i] + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; width: 50px;'>" + InvoiceAmts[i] + "</td>");
                    html.Append("</tr>");

                    i = i + 1;
                }

                html.Append("</tbody></table></td></tr>");

                //Banks
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; border-bottom: 0; width:70%;' align='center' cellspacing='0' cellpadding='5'>");
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
                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody><tr>");
                html.Append("<th style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'>In Words : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 80px;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td width='10%' valign='top' align='right' style='font-size:13px;'><b>Remarks : </b></td><td colspan='2' width='85%' style='font-size:12px; line-height:22px;'>" + item.Remarks + "</td></tr>");
                //html.Append("<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='12'><b>Remarks : </b> " + item.Remarks + "</td></tr>");
                html.Append("</tbody></table>");

                html.Append("<table style='width: 100%; margin-bottom: 10px;' cellspacing='0' cellpadding='10'><tbody><tr>");
                html.Append("<td style='font-size: 12px; width:60%;'><b>CWC GST NO -</b> " + objCompany[0].GstIn + " <br/> <b>CWC PAN NO -</b> " + objCompany[0].Pan + "</td>");
                html.Append("<th style='border-top: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>For Central Warehousing Corporation</th>");
                html.Append("</tr></tbody></table></td></tr></tbody></table>");

                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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

        #endregion

        #region SD Refund Receipt
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult GetSDRefundReceipt(string JournalNo)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetSDRefundReceiptDetails(JournalNo);
            if (ObjRR.DBResponse.Status == 1)
            {
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;
                string Path = GenerateSDRefundReceipt(ds);
                return Json(new { Status = 1, Message = Path });
            }
            return Json(new { Status = 0, Message = "No Data Found" });
        }
        [NonAction]
        public string GenerateSDRefundReceipt(DataSet ds)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstReceiptDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstMode = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstSB = new List<dynamic>();
            int i = 0;

            StringBuilder html = new StringBuilder();
            lstReceiptDetails.ToList().ForEach(item =>
            {
                string ClosingBalanceWord = "";
                if (lstReceiptDetails[0].ClosingAmount == 0)
                {
                    ClosingBalanceWord = "Zero";
                }
                else
                {
                    ClosingBalanceWord = objCurr.changeCurrencyToWords(lstReceiptDetails[0].ClosingAmount.ToString("0"));
                }
                //Page Header
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                html.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td width='70%' valign='top' align='center'><h1 style='font-size:16px; margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1><span style='font-size:7pt;'>(A Govt. of India Undertaking)</span><br/><span style='font-size:7pt;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - cwccfs@gmail.com</label><br/><label style='font-size: 8pt; font-weight:bold;'>Journal Voucher - Security Deposit <br/> (JV - SD)</label></td>");
                html.Append("<td valign='top'><img align='right' src='ISO' width='100'/></td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='font-size:9pt; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><td colspan='6' width='50%'><b>JV - SD Number :</b> " + lstReceiptDetails[0].JaurnalNo + "</td> <td colspan='6' width='50%' style='text-align:right;'><b>JV - SD Date :</b>  " + lstReceiptDetails[0].JaurnalDate + "</td></tr>");

                html.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='border:1px solid #000; font-size:9pt; width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>1. Payer Name</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].PartyName + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>2. Payer Address</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].Address + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>3. GST Number</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].GSTNo + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>4. Folio Number</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].FolioNo + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>5. RO Sanction Order Number</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].RoSanctionOrderNo + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>6. RO Sanction Order Date</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].RoSanctionDate + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>7. Opening Balance</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].Amount + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>8. Amount Withdrawn</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].RefundAmount + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>9. Mode of Withdrawal from SD</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstMode[0].Mode + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>10. Closing Balance</th><td width='60%' style='border-bottom:1px solid #000;'> " + lstReceiptDetails[0].ClosingAmount + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000; border-bottom:1px solid #000;'>11. Closing Balance (in words)</th><td width='60%' style='border-bottom:1px solid #000;'> " + ClosingBalanceWord + "</td></tr>");
                html.Append("<tr><th width='40%' style='border-right:1px solid #000;'>12. Remarks</th><td width='60%'> " + lstReceiptDetails[0].Remarks + "</td></tr>");
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><td colspan='6' width='50%'><b>CWC GST :</b> " + objCompany[0].GstIn + "</td> <td colspan='6' width='50%' style='text-align:right;'><b>CWC PAN :</b> " + objCompany[0].Pan + "</td></tr>");

                html.Append("<tr><td><span><br/><br/><br/><br/></span></td></tr>");

                html.Append("<tr><th colspan='12' style='text-align: right;'>Authorized Signatory <br/> For Central Warehousing Corporation</th></tr>");

                html.Append("</tbody></table>");

                html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                html.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
                rh.GeneratePDF(PdfDirectory + fileName, html.ToString());
            }
            return "/Docs/" + type + "/" + fileName;
        }
        #endregion



        #region PD Balance Summary



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult PDABalanceSummaryReport()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //WFLD_ReportRepository ObjReport = new WFLD_ReportRepository();
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
        public FileResult PDASummaryReport(PdSummary ObjPdSummary, int drpType)
        {
            try
            {
                //   var Type = fc["ddlType"].ToString();
                var excelName = "";
                var ObjRR = new VRN_ReportRepository();
                // WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
                List<PdSummary> LstPdSummary = new List<PdSummary>();
                Login objLogin = (Login)Session["LoginUser"];
                ObjRR.PdBalanceSummaryReport(ObjPdSummary, drpType);//, objLogin.Uid

                excelName = "PDSummaryExcel" + ".xls";

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
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PDSummaryExcel.xls");
                }
            }
            // return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPdSummaryReport(PdSummary ObjPdSummary, int drpType)
        {
            WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            List<PdSummary> LstPdSummary = new List<PdSummary>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.PdSummaryReport(ObjPdSummary, drpType);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstPdSummary = (List<PdSummary>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePdPDABalancePDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "SdSummary.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/SdSummary/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SdSummary/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion




        #region Party Ledger Report

        public ActionResult PartyLedgerStatement()
        {
           VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet("", 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstParty = Jobject["LstParty"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }

        [HttpGet]
        public JsonResult OASearchPartyLedgerParty(string PartyCode)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult OALoadPartyLedgerParty(string PartyCode, int Page)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForLedgerDet(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPartyLedgerReport(string FromDate, string ToDate, int PartyId)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();

            string Fdt = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetPartyLedgerStatement(PartyId, Fdt, Tdt);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePDFForPartyLedgerDetails(FormCollection Fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "OnAcStatementPartyLedger.pdf";
                Pages[0] = Fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report/OnAcStatementPartyLedger/";
                Pages[0] = Fc["Page"].ToString().Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages[0] = Pages[0].ToString().Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg")).Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));
                //Pages[0] = Pages[0].ToString();

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 30f, 30f, 30f, 30f, false, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }

                return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/OnAcStatementPartyLedger/" + FileName });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        #endregion


        #region Consolidate Party Ledger Statement



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult PLedgerConsolidate()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //WFLD_ReportRepository ObjReport = new WFLD_ReportRepository();
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
        public FileResult GetPLedgerConsolidate(WFLD_PartyLedCons ObjPdSummary)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_PartyLedCons> LstPdSummary = new List<VRN_PartyLedCons>();
            var excelName = "";
            try
            {

                Login objLogin = (Login)Session["LoginUser"];
                ObjRR.PdConsolidateReport(ObjPdSummary);//, objLogin.Uid
                excelName = "PartyLedgerConsolidate" + ".xls";

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
                var excelFile = Server.MapPath("~/Docs/Excel/" + name + ".xls");
                using (var exl = new DynamicExcel.ExcelGenerator(excelFile))
                {
                    exl.AddCell("A1", "No data found");
                    exl.Save();
                    return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PartyLedgerConsolidate.xls");
                }
            }
            // return null;
        }



        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GeneratePLedgerConsolidatePDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "PartyLedgerConsolidate.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/SdSummary/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/SdSummary/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion



        #region Party Wise Unpaid Invoice Details
        [HttpGet]
        public ActionResult GetUnpaidInvoice()
        {
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.GetPayeeNameforUnpaidInv();
            if (objRR.DBResponse.Data != null)
                ViewBag.GetPaymentParty = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(objRR.DBResponse.Data));
            else ViewBag.GetPaymentParty = null;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PartyWiseUnpaidAmout(PartyWiseUnpaid objUn)
        {
            if (ModelState.IsValid)
            {
                VRN_ReportRepository objIM = new VRN_ReportRepository();
                objIM.PartyWiseUnpaidAmt(objUn.PartyId, objUn.AsOnDate);
                if (objIM.DBResponse.Data != null)
                {
                    PartyWiseUnpaidDtl objDet = new PartyWiseUnpaidDtl();
                    objDet = (PartyWiseUnpaidDtl)objIM.DBResponse.Data;
                    string Path = GeneratePDFforUnpaidInvoiceDetails(objDet);
                    return Json(new { Status = 1, Message = Path });
                }
                return Json(new { Status = 0, Message = "No Data Found" });
            }
            else return Json(new { Status = 0, Message = "No Data Found" });
        }
        [NonAction]
        public string GeneratePDFforUnpaidInvoiceDetails(PartyWiseUnpaidDtl objDet)
        {
            StringBuilder objSB = new StringBuilder();
            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: center;' colspan='3'>");
            objSB.Append("<span style='font-size: 14px; font-weight: bold; line-height: 16px;'>Payee Unpaid Bill Status</span>");
            objSB.Append("</td></tr><tr><td style='text-align: left;' colspan='2'>");
            objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>Payee Name: <label>" + objDet.PartyName + "</label>");
            objSB.Append("</span></td><td style='text-align: right;'>");
            objSB.Append("<span style='display: block; font-size: 11px; line-height: 22px;padding-bottom: 10px;'>As On: <label>" + objDet.AsOnDate + "</label></span>");
            objSB.Append("</td></tr><tr><td colspan='3'><table style='width:100%; margin-bottom: 10px;' ><tbody><tr>");
            objSB.Append("<td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Invoice No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date</th>");
            objSB.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amount</th>");
            objSB.Append("</tr></thead>");
            objSB.Append("<tfoot><tr>");
            objSB.Append("<td colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; font-weight: bold; text-align: center; padding: 5px;'>Total</td>");
            objSB.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Convert.ToDecimal(objDet.lstDtl.Sum(x => x.InvoiceAmt)) + "</td></tr></tfoot><tbody>");
            objDet.lstDtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.InvoiceNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.InvoiceDate + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item.InvoiceAmt + "</td></tr>");
            });
            objSB.Append("</tbody></table></td></tr></tbody></table></td></tr></tbody></table>");
            string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID;
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);
            if (System.IO.File.Exists(LocalDirectory + "/UnpaidInvoiceDet.pdf"))
                System.IO.File.Delete(LocalDirectory + "/UnpaidInvoiceDet.pdf");
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
            {
                RH.HeadOffice = this.HeadOffice;
                RH.HOAddress = this.HOAddress;
                RH.ZonalOffice = this.ZonalOffice;
                RH.ZOAddress = this.ZOAddress;
                RH.GeneratePDF(LocalDirectory + "/UnpaidInvoiceDet.pdf", objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/UnpaidInvoiceDet.pdf";
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
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCRNote(FormCollection fc)
        {
            //objRR.GetBulkDebitNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"));
            VRN_ReportRepository objRepo = new VRN_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            objRepo.PrintDetailsForBulkCRNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "C");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR,"Credit Note");
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Record Not Found" });
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
            VRN_ReportRepository objRepo = new VRN_ReportRepository();
            PrintModelOfBulkCrCompany objCR = new PrintModelOfBulkCrCompany();
            objRepo.PrintDetailsForBulkCRNoteReport(Convert.ToDateTime(fc["PeriodFrom"]).ToString("yyyy/MM/dd"), Convert.ToDateTime(fc["PeriodTo"]).ToString("yyyy/MM/dd"), "D");
            if (objRepo.DBResponse.Data != null)
            {
                objCR = (PrintModelOfBulkCrCompany)objRepo.DBResponse.Data;
                string Path = GenerateDRNotePDF(objCR,"Debit Note");
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = -1, Message = "Record Not Found" });
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
        public string GenerateDRNotePDF(PrintModelOfBulkCrCompany objCR,String CRDR)
        {


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
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' width='100%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCR.CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCR.CompanyAddress + "</span>");
                html.Append("</td></tr>");
                html.Append("<tr><td colspan='9' width='100%' valign='top' align='center'><center><label align='center' style='font-size: 14px; font-weight:bold;text-align:center;'> " + CRDR  +"</label></center></td></tr>");
              
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
                decimal TTax = 0;
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
                    TTax += data.Taxable;
                    CGSTAmt += data.CGSTAmt;
                    SGSTAmt += data.SGSTAmt;
                    total += data.Total;
                });

                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 300px;'>Total</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + TTax +" </td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'>" + total + "</td></tr>");

                string AmountInWord = ConvertNumbertoWords((long)item.GrandTotal);
                html.Append("<tr><td style='border-bottom: 1px solid #000; font-size: 10px; text-align: left; padding: 5px;' colspan='10'><b>Round Up:</b>" + item.RoundUp + "</td></tr>");

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

        #region Collection Report
        [HttpGet]
        public ActionResult CollectionReport()
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
            ViewBag.TotalCash = 0;
            ViewBag.TotalPDA = 0;
            ViewBag.TotalBank = 0;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCollectionReport(VRN_CollectionReport ObjDebtorReport)
        {
           VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_CollectionReport> LstInvoiceReportDetails = new List<VRN_CollectionReport>();
            VRN_FinalCollectionReportTotal objFinalCollectionReportTotal = new VRN_FinalCollectionReportTotal();
            VRN_CollectionReportTotal objCollectionReportTotal = new VRN_CollectionReportTotal();
            //  Login objLogin = (Login)Session["LoginUser"];
            ObjRR.CollectionReportVRN(ObjDebtorReport);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                objFinalCollectionReportTotal = (VRN_FinalCollectionReportTotal)ObjRR.DBResponse.Data;
                LstInvoiceReportDetails = objFinalCollectionReportTotal.listCollectionReport.ToList();
                objCollectionReportTotal = objFinalCollectionReportTotal.objCollectionReportTotal;

                ViewBag.TotalCash = objCollectionReportTotal.TotalCash;
                ViewBag.TotalPDA = objCollectionReportTotal.TotalPDA;
                ViewBag.TotalBank = objCollectionReportTotal.TotalBank;

                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateCollectionReportPDF(FormCollection fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "CollectionReport.pdf";
                Pages[0] = fc["Page"].ToString();

                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/CollectionReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/CollectionReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion


        #region service code wise Invoice Details

        [HttpGet]
        public ActionResult ServiceCodeWiseInvDtls()
        {
            Hdb_ReportRepository ObjRR = new Hdb_ReportRepository();
            // List<SACList> LstSAC = new List<SACList>();
            // ObjRR.GetSAC();
            //if (ObjRR.DBResponse.Data != null)
            // {
            //  LstSAC = (List<SACList>)ObjRR.DBResponse.Data;
            // }
            // ViewBag.SACList = LstSAC;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetServiceCodeWiseInvDtls(Hdb_ServiceCodeWiseInvDtls ObjServiceCodeWiseInvDtls)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            List<VRN_ServiceCodeWiseInvDtls> LstInvoiceReportDetails = new List<VRN_ServiceCodeWiseInvDtls>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.ServiceCodeWiseInvDtls(ObjServiceCodeWiseInvDtls);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                LstInvoiceReportDetails = (List<VRN_ServiceCodeWiseInvDtls>)ObjRR.DBResponse.Data;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateServiceCodeWiseInvDtlsPDF(FormCollection fc)
        {
            try
            {

                string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "ServiceCodeWiseInvoiceDetails.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/ServiceCodeWiseInvoiceDetails/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/ServiceCodeWiseInvoiceDetails/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion



        #region PV Report Of Import Loaded Container
        [HttpGet]
        public ActionResult PVReportImpLoadedCont()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetImpLoadedCont(VRN_PVImpLoadedModel ObjPV)
        {
            if (ModelState.IsValid)
            {
                VRN_ReportRepository ObjRR = new VRN_ReportRepository();
                ObjRR.GetImpLoadedCont(ObjPV);
                string Path = "";
                if (ObjRR.DBResponse.Data != null)
                {
                    List<VRN_ExpPvReport> lstData = new List<VRN_ExpPvReport>();
                    lstData = (List<VRN_ExpPvReport>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFImpLoadedCont(lstData, ObjPV.AsOnDate);
                }
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFImpLoadedCont(List<VRN_ExpPvReport> lstData, string Date)
        {
            try
            {
                var FileName = "ImpLoadedContReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + lstData[0].CompanyLocation + "-" + lstData[0].CompanyBranch + "</span><br/><span style='font-size:12px;'><b>As On Date - </b> " + Date + "</span><br/><label style='font-size: 14px; font-weight:bold;'>Physical Verification Report for Import Loaded Container</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>S.No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>Entry No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;'>Container No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>In Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>Size</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Sla</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>Origin</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>No of Days</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;text-align:right;'>GRL Amount</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:20%;'>Remarks</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");
                lstData.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>" + item.CFSCode + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;'>" + item.ContainerNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.EntryDateTime + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:4%;'>" + item.Size + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + item.EximTraderAlias + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.TransportFrom + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:7%;'>" + item.Days + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:10%;text-align:right;'>" + item.Amount + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:20%;'>" + item.Remarks + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });

                Pages.Append("<tr>");
                Pages.Append("<th colspan='2' style='width:3%;'>Total :</th>");
                Pages.Append("<th style='width:11%;'></th>");
                Pages.Append("<th style='width:8%;'></th>");
                Pages.Append("<th style='width:3%;'></th>");
                Pages.Append("<th style='width:5%;'></th>");
                Pages.Append("<th colspan='3' style='width:15%;text-align:right;'>" + lstData.Sum(o => o.Amount).ToString() + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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

        #region PV Report Of Empty Container
        [HttpGet]
        public ActionResult PVReportEmptyCont()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetEmptyCont(VRN_PVImpLoadedModel ObjPV)
        {
            if (ModelState.IsValid)
            {
               VRN_ReportRepository ObjRR = new VRN_ReportRepository();
                ObjRR.GetEmptyCont(ObjPV);
                string Path = "";
                if (ObjRR.DBResponse.Data != null)
                {
                    List<VRN_PVReportImpEmptyCont> lstData = new List<VRN_PVReportImpEmptyCont>();
                    lstData = (List<VRN_PVReportImpEmptyCont>)ObjRR.DBResponse.Data;
                    Path = GeneratePDFEmptyCont(lstData, ObjPV.AsOnDate);
                }
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
        [NonAction]
        public string GeneratePDFEmptyCont(List<VRN_PVReportImpEmptyCont> lstData, string Date)
        {
            try
            {
                var FileName = "EmptyContReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + lstData[0].CompanyLocation + "-" + lstData[0].CompanyBranch + "</span><br/><span style='font-size:12px;'><b>As On - </b> " + Date + "</span><br/><label style='font-size: 14px; font-weight:bold;'>Physical Verification Report of Empty Containers</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append("</td></tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; border-bottom:none; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>Sno</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>Entry No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;'>Container No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>In Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>Size</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Sla Cd</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>In Date Ecy</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>Out Date Ecy</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>Days</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;width:12%;text-align:right;'>GRE Amount</th></tr></thead>");
                Pages.Append("<tbody>");
                lstData.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>" + item.CFSCode + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;'>" + item.ContainerNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.EntryDateTime + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + item.Size + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + item.EximTraderAlias + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;'>" + item.InDateEcy + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;'>" + item.OutDateEcy + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:5%;'>" + item.Days + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.Amount + "</td>");
                    Pages.Append("</tr>");
                    i++;
                });
                Pages.Append("<tr>");
                Pages.Append("<th colspan='2' style='width:3%;'>Total :</th>");
                Pages.Append("<th style='width:11%;'></th>");
                Pages.Append("<th style='width:8%;'></th>");
                Pages.Append("<th style='width:3%;'></th>");
                Pages.Append("<th style='width:5%;'></th>");
                Pages.Append("<th style='width:8%;'></th>");
                Pages.Append("<th colspan='3' style='width:12%;text-align:right;'>" + lstData.Sum(o => o.Amount).ToString() + "</th>");
                Pages.Append("</tr>");

                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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
        #region ContainerOutReport



        /*Invoice Report Detai;s Section-06.11.2017*/
        [HttpGet]
        public ActionResult ContainerOutReport()
        {
            //Login ObjLogin = (Login)Session["LoginUser"];
            //ViewBag.DistList = null;
            //ReportRepository ObjReport = new ReportRepository();
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
        public ActionResult GetContainerOutReport(ContainerOutReport ObjContainerOutReport)
        {


            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ContainerOutReport LstContainerOutReport = new ContainerOutReport();
            Login objLogin = (Login)Session["LoginUser"];
            ObjRR.ContainerOutport(ObjContainerOutReport);//, objLogin.Uid
            if (ObjRR.DBResponse.Data != null)
            {
                // LstContainerOutReport.lstContainerOutReport = (List<ContainerOutReportList>)ObjRR.DBResponse.Data;
                //LstContainerOutReport = (ContainerOutReport)ObjRR.DBResponse.Data;
                //if (LstContainerOutReport.lstContainerOutReport.Count > 1)
                //{
                //    LstContainerOutReport.lstContainerOutReport.ToList().ForEach(m =>
                //    {
                //        if (m.Size == "20" && m.Size != null && m.Size != "")
                //        {
                //            sizeTwenty += 1;
                //        }
                //        if (m.Size == "40" && m.Size != null && m.Size != "")
                //        {
                //            sizeFourty += 1;
                //        }

                //    });
                //}
                //ViewBag.sizeTwenty = sizeTwenty;
                //ViewBag.sizeFourty = sizeFourty;
                return Json(ObjRR.DBResponse);
            }
            else
            {
                return Json(ObjRR.DBResponse);
            }

        }

        [HttpPost, ValidateInput(false)]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateContainerOutReportPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "ContainerOutReport.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/VRN_Report/ContainerOutReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/VRN_Report/ContainerOutReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion

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
                var ObjRR = new VRN_ReportRepository();
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
            VRN_ReportRepository objPpgRepo = new VRN_ReportRepository();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (VRN_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            VRN_CWCImportController objvrnImp = new VRN_CWCImportController();
            VRN_CashManagementController objvrnCash = new VRN_CashManagementController();

            try
            {
                var invoiceData = JsonConvert.DeserializeObject<VRN_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            var result = await objvrnImp.GetIRNForYardInvoice(item.InvoiceNo, item.SupplyType);
                        }
                        else if (item.InvoiceType == "C")
                        {
                            var result1 = await objvrnCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                        }
                        else if (item.InvoiceType == "D")
                        {
                            var result2 = await objvrnCash.GetGenerateIRNCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
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




        #region Payer Ledger Statement

        public ActionResult PayerLedgerStatement()
        {
           VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForPartyLedger("", 0);
            if (ObjRR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstParty = Jobject["LstParty"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }


        [HttpGet]
        public JsonResult SearchPartyForPayer(string PartyCode)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForPartyLedger(PartyCode, 0);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyFoPayer(string PartyCode, int Page)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllPartyForPartyLedger(PartyCode, Page);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetPayerLedgerStatement(HDB_PayerStatement vm)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();

            string Fdt = Convert.ToDateTime(vm.FromDate).ToString("yyyy-MM-dd");
            string Tdt = Convert.ToDateTime(vm.ToDate).ToString("yyyy-MM-dd");

            ObjRR.GetPayerLedgerStatement(Convert.ToInt32(vm.PartyId), Fdt, Tdt);
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {
                HdbSDDetailsStatement SDData = new HdbSDDetailsStatement();
                SDData = (HdbSDDetailsStatement)ObjRR.DBResponse.Data;

                Path = GeneratePDFPayerLedgerStatement(SDData, vm.FromDate, vm.ToDate);
            }
            return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public string GeneratePDFPayerLedgerStatement(HdbSDDetailsStatement SDData, string FromDate, string ToDate)
        {
            try
            {
                var FileName = "LedgerReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";
                //***************************************************************************************


                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td colspan='12'>");
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                Pages.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                Pages.Append("<td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>Plot No. 50 to 54, Phase-III (B), Verna Indl. Estate, <br/> Verna Salgette, South Goa, GOA-403722</span><br/><label style='font-size: 14px; font-weight:bold;'>PAYER LEDGER STATEMENT</label></td></tr>");
                Pages.Append("</tbody></table>");
                Pages.Append(" </td></tr>");
                Pages.Append("</thead>");

                Pages.Append(" <tbody>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Payer :</b>" + SDData.PartyName + "</td></tr>");
                Pages.Append(" <tr><td colspan='12' style='font-size:12px;'><b>Folio No :</b> " + SDData.PartyCode + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>CWC GST No :</b> " + SDData.CompanyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Payer GST No :</b> " + SDData.PartyGst + "</td></tr>");
                Pages.Append("<tr><td colspan='12' style='font-size:12px;'><b>Period :</b> " + FromDate + " - " + ToDate + "</td></tr>");
                Pages.Append("</tbody></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse; border:1px solid #000; font-size:8pt;'>");
                Pages.Append("<thead><tr><th style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>Sl. No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>Invoice No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Invoice Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>Receipt No</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Receipt Date</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Pay Receipt</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>Transaction Type</th>");

                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:12%;text-align:center;'>Transaction Amount</th>");
                Pages.Append("<th style='border-bottom:1px solid #000;border-right:1px solid #000;width:12%;text-align:center;'>Closing Balance</th></tr></thead>");

                Pages.Append("<tbody>");
                int counter = 1;
                decimal utilisationamt = 0;
                decimal ReceiptAmt = 0;
                decimal balance = 0;
                SDData.lstInvc.ForEach(item =>
                {
                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:3%;'>" + i + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:9%;text-align:center;'>" + item.InvoiceNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.InvoiceDate + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:11%;text-align:center;'>" + item.ReceiptNo + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.ReceiptDate + "</td>");
                    if (item.ReceiptNo != "")
                    {
                        if (item.TranAmt == 0)
                        {
                            Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:right;'>" + item.ReceiptAmt.ToString() + "</td>");
                        }
                        else
                        {
                            Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:right;'>" + ReceiptAmt.ToString() + "</td>");

                        }

                    }
                    else
                    {
                        Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:right;'>" + ReceiptAmt.ToString() + "</td>");

                    }
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:8%;text-align:center;'>" + item.TranType + "</td>");
                    Pages.Append("<td style='border-bottom:1px solid #000;border-right:1px solid #000;width:12%;text-align:right;'>" + item.TranAmt.ToString() + "</td>");
                    if (counter == 1)
                    {
                        Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + item.ReceiptAmt.ToString() + "</td>");
                        utilisationamt = Convert.ToDecimal(item.ReceiptAmt);
                    }
                    else
                    {
                        if (item.ReceiptNo != "")
                        {
                            if (item.TranAmt == 0)
                            {
                                if (item.CrAdjust == 0)
                                {
                                    utilisationamt = utilisationamt + Convert.ToDecimal(item.ReceiptAmt);
                                    balance = balance - Convert.ToDecimal(item.TranAmt);
                                }
                            }
                            else
                            {
                                utilisationamt = utilisationamt + Convert.ToDecimal(item.TranAmt);
                                balance = balance - Convert.ToDecimal(item.TranAmt);
                            }
                        }
                        utilisationamt = utilisationamt - Convert.ToDecimal(item.TranAmt);
                        balance = balance + Convert.ToDecimal(item.TranAmt);
                        Pages.Append("<td style='border-bottom:1px solid #000;width:12%;text-align:right;'>" + utilisationamt.ToString() + "</td>");
                    }
                    Pages.Append("</tr>");
                    counter++;
                    i++;
                });
                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='width:8%;text-align:right;'>Closing Balance :</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + utilisationamt.ToString() + "</th>");
                Pages.Append("</tr>");



                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='width:8%;text-align:right;'>SD Balance :</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + SDData.SDBalance.ToString() + "</th>");
                Pages.Append("</tr>");

                Decimal UtiBalance = Convert.ToDecimal(SDData.SDBalance) - Math.Abs(utilisationamt);
                if (SDData.IsPda == 0)
                {
                    UtiBalance = 0;
                }
                Pages.Append("<tr>");
                Pages.Append("<th colspan='7' style='width:8%;text-align:right;'>Utilization Balance :</th>");
                Pages.Append("<th style='width:12%;text-align:right;'>" + UtiBalance.ToString() + "</th>");
                Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; border-collapse:collapse;font-size:9pt;'><tbody>");
                Pages.Append("<tr><td><span><br/><br/></span></td></tr>");
                Pages.Append("<tr><td colspan='12'><p><b>N.B :</b> This is a computer generated statement. Doesn't require any signature.</p></td></tr>");
                Pages.Append("</tbody></table>");

                //***************************************************************************************
                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                if (System.IO.File.Exists(LocalDirectory + "/" + FileName))
                {
                    System.IO.File.Delete(LocalDirectory + "/" + FileName);
                }
                //Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                //Pages.Replace("ISO", Server.MapPath("~/Content/Images/iso_logo.jpg"));
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


        #region UnRealized (Unpaid) Invoice Report

        [HttpGet]
        public ActionResult UnRealizedInvoiceSummary()
        {


            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUnRealizedInvSummary(string AsOnDate)
        {
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.GetUnRealizedInvSummary(AsOnDate);//, objLogin.Uid
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
        public JsonResult GenerateUnRealisedSummaryPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "UnrealizedInvoiceSummaryReport.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/UnrealizedInvoiceSummaryReport/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, true))
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
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/UnrealizedInvoiceSummaryReport/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

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
            VRN_ReportRepository ObjER = new VRN_ReportRepository();
            List<Vrn_E04Report> LstE04 = new List<Vrn_E04Report>();
            ObjER.ListofE04Report(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Vrn_E04Report>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfE04DetailsReport", LstE04);
        }
        [HttpGet]
        public JsonResult LoadMoreE04List(int Page)
        {
            VRN_ReportRepository ObjER = new VRN_ReportRepository();
            var LstE04 = new List<Vrn_E04Report>();
            ObjER.ListofE04Report(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Vrn_E04Report>)ObjER.DBResponse.Data;
            }
            return Json(LstE04, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewE04DetailsReport(int ID)
        {
            Vrn_E04Report ObjE04 = new Vrn_E04Report();
            VRN_ReportRepository ObjER = new VRN_ReportRepository();
            ObjER.GetE04DetailById(ID);
            if (ObjER.DBResponse.Data != null)
            {
                ObjE04 = (Vrn_E04Report)ObjER.DBResponse.Data;
            }
            return PartialView(ObjE04);
        }


        [HttpGet]
        public JsonResult GetE04Search(string SB_No = "", string SB_Date = "", string Exp_Name = "")
        {
            VRN_ReportRepository ObjER = new VRN_ReportRepository();
            List<Vrn_E04Report> LstE04 = new List<Vrn_E04Report>();
            ObjER.GetE04DetailSearch(SB_No, SB_Date, Exp_Name);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Vrn_E04Report>)ObjER.DBResponse.Data;
            }
            //return PartialView("ListOfE04DetailsReport", LstE04);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Stuffing Acknowledgement Search       

        [HttpGet]
        public ActionResult StfAckSearch()
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();

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
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchShipbill(string shipbill)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadContainerLists(string cont, int Page)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllContainerNoForContstufserach(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadShipbillLists(string shipbill, int Page)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getcontstuffingacksearch(string container, string shipbill, string cfscode)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_ContStufAckRes> Lststufack = new List<Vrn_ContStufAckRes>();
            ObjGR.GetStufAckResult(container, shipbill, cfscode);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Vrn_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SBill Query
        [HttpGet]
        public ActionResult SBQuery()
        {
            var ObjRR = new VRN_ReportRepository();
            ObjRR.GetAllSB();

            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfSB = new SelectList((List<VRN_SBQuery>)ObjRR.DBResponse.Data, "Id", "SBNODate");
            }
            return PartialView();
        }

        // [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetApplicationDetForSBQuery(string Id, string sbno, string sbdate)
        {
            int id = Convert.ToInt32(Id);
            string SBNO = Convert.ToString(sbno);
            VRN_SBQuery objCR = new VRN_SBQuery();
            VRN_ReportRepository objRR = new VRN_ReportRepository();
            objRR.SBQueryReport(id, sbno, sbdate);
            if (objRR.DBResponse.Data != null)
            {
                objCR = (VRN_SBQuery)objRR.DBResponse.Data;
                return Json(objCR, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = -1, Message = "No Data" }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
        #region ASR Acknowledgement Search       

        [HttpGet]
        public ActionResult StuffingASRAckSearch()
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();

            //ObjGR.GetAllShippingBillNoForASRACK("", 0);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGR.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstStuff = Jobject["LstStuff"];
            //    ViewBag.StateShipbill = Jobject["State"];
            //}
            //else
            //{
            //    ViewBag.LstStuff = null;
            //}

            return PartialView();
        }

        [HttpGet]
        public JsonResult ASRSearchContainerNo(string cont)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetCotainerNoForASRAck(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadContainerLists(string cont, int Page)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetCotainerNoForASRAck(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ASRSearchShipbill(string shipbill)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadShipbillLists(string shipbill, int Page)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetASRACKStatusSearch(string shipbill, string CFSCode, string container)

        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_ContStufAckRes> Lststufack = new List<Vrn_ContStufAckRes>();
            ObjGR.GetASRAckResult(shipbill, CFSCode, container);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Vrn_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DP Acknowledment Serach  

        [HttpGet]
        public ActionResult DPAckSearch()
        {
            return PartialView();
        }

        public JsonResult GetGatePassNoDPForAckSearch(string GatePassNo = "", int Page = 0)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_GatePassDPAckSearch> lstDPGPAck = new List<Vrn_GatePassDPAckSearch>();
            ObjGR.GetGatePassNoDPForAckSearch(GatePassNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTGPAck = (List<Dsr_GatePassDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDPAckSearch(string ContainerNo = "", int Page = 0)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_ContDPAckSearch> lstDPContACK = new List<Vrn_ContDPAckSearch>();
            ObjGR.GetContainerNoForDPAckSearch(ContainerNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTContACK = (List<Dsr_ContDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDPAckSearch(int GatePassId, string ContainerNo, int GatePassdtlId = 0)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_DPAckRes> lstDPAckRes = new List<Vrn_DPAckRes>();
            ObjGR.GetDPAckSearch(GatePassId, ContainerNo, GatePassdtlId);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDPAckRes = (List<Vrn_DPAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DT Acknowledment Serach  

        [HttpGet]
        public ActionResult DTAckSearch()
        {
            return PartialView();
        }

        public JsonResult GetGatePassNoDTForAckSearch(string GatePassNo = "", int Page = 0)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_GatePassDTAckSearch> lstDTGPAck = new List<Vrn_GatePassDTAckSearch>();
            ObjGR.GetGatePassNoDTForAckSearch(GatePassNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDTAckSearch(string ContainerNo = "", int Page = 0)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_ContDTAckSearch> lstDTContACK = new List<Vrn_ContDTAckSearch>();
            ObjGR.GetContainerNoForDTAckSearch(ContainerNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDTAckSearch(int GatePassId, string ContainerNo)
        {
            VRN_ReportRepository ObjGR = new VRN_ReportRepository();
            List<Vrn_DTAckRes> lstDTAckRes = new List<Vrn_DTAckRes>();
            ObjGR.GetDTAckSearch(GatePassId, ContainerNo);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDTAckRes = (List<Vrn_DTAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
