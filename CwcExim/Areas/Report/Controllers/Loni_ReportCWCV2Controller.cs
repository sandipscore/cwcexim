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
using CwcExim.Areas.CashManagement.Models;
using CwcExim.Areas.Auction.Models;
using EinvoiceLibrary;
using System.Threading.Tasks;
using CwcExim.Areas.Import.Controllers;
using CwcExim.Areas.CashManagement.Controllers;

namespace CwcExim.Areas.Report.Controllers
{
    public class Loni_ReportCWCV2Controller : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string HeadOffice { get; set; }
        private string HOAddress { get; set; }
        public string ZonalOffice { get; set; }
        public string ZOAddress { get; set; }
        public decimal EffectVersion { get; set; }
        public string EffectVersionLogoFile { get; set; }

        private string strQRCode = "000201010211021645992400041983220415545927000419832061661000500041983220827PUNB0042000420001210003366226410010A00000052401230514200A0000151.mab@pnb27260010A00000052401080601414A5204939953033565802IN5923CENTRAL WAREHOUSING COR6008GHAZIPUR6106110096621207080601414A6304b8e4";

        public Loni_ReportCWCV2Controller()
        {
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
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
            Loni_ReportRepositoryV2 ObjRR = new Loni_ReportRepositoryV2();


            AuctionInvoiceViewModel ObjBulk = new AuctionInvoiceViewModel();
            ObjBulk.InvoiceModule = ObjBulkInvoiceReport.InvoiceModule;
            ObjBulk.InvoiceModuleName = ObjBulkInvoiceReport.InvoiceModuleName;
            ObjBulk.InvoiceNumber = ObjBulkInvoiceReport.InvoiceNumber;
            ObjBulk.PartyId = Convert.ToString(ObjBulkInvoiceReport.PartyId);
            ObjBulk.PeriodFrom = ObjBulkInvoiceReport.PeriodFrom;
            ObjBulk.PeriodTo = ObjBulkInvoiceReport.PeriodTo;
            ObjBulk.All = ObjBulkInvoiceReport.All;


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
                                FilePath = GeneratingBulkPDFforContainer((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EXPMovement":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "CONTAINER MOVEMENT";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforContainerMovement((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "CCIN":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "CCIN ENTRY";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforCCINEntry((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "BTT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "BTT";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforBTT((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "AUC":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "Auction";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrintAuction(ObjBulk);
                                FilePath = GeneratingAssessmentSheetReportAuction((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
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
                            case "IMPSC":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "SEAL CUTTING";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforSealCutting((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EXPCRGSHFT":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "CARGO SHIFTING";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforCargoSF((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "Fumigation":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "FUMIGATION";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforFumigationInvoice((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "EXPCS":
                            case "EXPCSGRE":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "EXPORT CONTAINER STUFFING";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforContainerStuffingInvoice((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            case "IMPTS":
                            case "IMPTSL":
                            case "IMPTST":
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                ModuleName = "IMPORT TRAIN SUMMARY";
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforTrainSummary((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
                                break;
                            default:
                                ObjBulkInvoiceReport.InvoiceModule = Mod;
                                //case statement for printing name of invoices                                
                                if (Mod == "GE") { ModuleName = "GATE ENTRY"; }
                                else if (Mod == "IMPFCLLCL") { ModuleName = "FCL To LCL CONVERSION"; }
                                else if (Mod == "MiscInv") { ModuleName = "MISCELLANEOUS"; }
                                /*  else if (Mod == "EXPCS") { ModuleName = "EXPORT CONTAINER STUFFING"; }*/
                                else if (Mod == "RENT") { ModuleName = "RENT"; }
                                else if (Mod == "RESERV") { ModuleName = "RESERVATION"; }
                                else if (Mod == "IRR") { ModuleName = "IRR"; }
                                else if (Mod == "INVDEBT") { ModuleName = "CONTAINER DEBIT INVOICE"; }
                                else if (Mod == "EXPRRCD") { ModuleName = "EXPORT RR CREDIT DEBIT"; }
                                else ModuleName = Mod;
                                ObjBulkInvoiceReport.All = "All";
                                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                                FilePath = GeneratingBulkPDFforGE((DataSet)ObjRR.DBResponse.Data, ModuleName, ObjBulkInvoiceReport.All);
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
                    DataSet dss = (DataSet)ObjRR.DBResponse.Data;
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
                        case "EXPLod":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXPMovement":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforContainerMovement(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "CCIN":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforCCINEntry(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "BTT":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforBTT(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "AUC":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingAssessmentSheetReportAuction(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EC":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "ECTrns":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforEmptyContainer(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "IMPSC":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforSealCutting(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXPCRGSHFT":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforCargoSF(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "Fumigation":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforFumigationInvoice(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "EXPCS":
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforContainerStuffingInvoice(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        case "IMPTS":
                            ObjBulkInvoiceReport.All = "";
                            ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                            FilePath = GeneratingBulkPDFforTrainSummary(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                        default:
                            ObjBulkInvoiceReport.All = "";
                            FilePath = GeneratingBulkPDFforGE(ds, ObjBulkInvoiceReport.InvoiceModuleName, ObjBulkInvoiceReport.All);
                            break;
                    }

                    return Json(new { Status = 1, Data = FilePath });
                }
                else
                    return Json(new { Status = -1, Data = "No Record Found." });
            }
        }

        [NonAction]
        public string GeneratingBulkPDFforDebitInvoice(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            Einvoice obj = new Einvoice();

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstHeader = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[7]);
            List<string> lstSB = new List<string>();
            String RemarksValue = "";
            string Container = "";
            lstHeader.ForEach(item =>
            {
                if (item.ContainerType == "Empty" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Empty Container Gate In";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 2 && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Road";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 1 && item.TransportFrom == "L" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD Loni)";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 1 && item.TransportFrom == "T" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD TKD)";
                else if (item.ContainerType == "LoadedVehicle" && item.TransportMode == 1 && item.TransportFrom == "T" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD TKD)";
                else if (item.ContainerType == "LoadedVehicle" && item.TransportMode == 1 && item.TransportFrom == "L" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD Loni)";
                else if (item.CBT == " 1" && InvoiceModuleName == "GATE ENTRY")
                    Container = "CBT";

            });
            lstInvoice.ToList().ForEach(item =>
            {
                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                RemarksValue = item.Remarks;

                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label><label style = 'font-size: 7pt; font-weight:bold;'> " + Container + " </label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + lstInvoice[0].irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(lstInvoice[0].SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(strQRCode)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");


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
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

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
                //html.Append("</tbody>");
                //html.Append("</table></td></tr>");
                html.Append("<tr><td><hr/></td></tr>");

                if (InvoiceModuleName == "IRR")
                {
                    lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                        html.Append("<thead><tr>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Train No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Wagon No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>VIA</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Gross Wt</th>");
                        html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Tare Wt</th>");
                        html.Append("</tr></thead>");
                        html.Append("<tbody><tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.TrainNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.WagonNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.VIA + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.GrossWt + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.TareWt + "</td>");
                        html.Append("</tr></tbody>");
                        html.Append("</table></td></tr>");
                    });
                }

                html.Append("<tr><th style='text-align: left; font-size: 13px;margin-top: 10px;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                // html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    if (InvoiceModuleName == "GATE ENTRY")
                    {
                        elem.ToDate = elem.ArrivalDate;
                    }
                    if (InvoiceModuleName == "FCL To LCL CONVERSION")
                    {
                        elem.ToDate = item.DeliveryDate;
                    }


                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ToDate + "</td>");
                    // html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px;margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
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
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
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



                if (InvoiceModuleName == "MISCELLANEOUS" || InvoiceModuleName == "CONTAINER DEBIT INVOICE")
                {
                    html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='12'>Remarks:");
                    html.Append("<label style='font-weight: bold;'>" + RemarksValue + " </label></td></tr>");
                }


                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:9px;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");
                html.Append("</td></tr></tbody></table>");
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
        public string GeneratingBulkPDFforContainerStuffingInvoice(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstbreakup = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
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
            String RemarksValue = "";
            lstInvoice.ToList().ForEach(item =>
            {
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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;


                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                //PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                RemarksValue = item.Remarks;
                /*rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }*/
                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");


                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");

                if (InvoiceModuleName == "IRR")
                {
                    lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                        html.Append("<thead><tr>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Train No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Wagon No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>VIA</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Gross Wt</th>");
                        html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Tare Wt</th>");
                        html.Append("</tr></thead>");
                        html.Append("<tbody><tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TrainNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.WagonNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.VIA + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.GrossWt + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TareWt + "</td>");
                        html.Append("</tr></tbody>");
                        html.Append("</table></td></tr>");
                    });
                }

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Remarks</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.FromDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ToDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Export Purpose</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/

                //lstbreakup.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                //    {

                //    });


                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");



                if (InvoiceModuleName == "MISCELLANEOUS" || InvoiceModuleName == "CONTAINER DEBIT INVOICE")
                {
                    html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='12'>Remarks:");
                    html.Append("<label style='font-weight: bold;'>" + RemarksValue + " </label></td></tr>");
                }


                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
        public string GeneratingBulkPDFforGE(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            Einvoice obj = new Einvoice();

            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstHeader = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[7]);
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
            String RemarksValue = "";
            string Container = "";
            lstHeader.ForEach(item =>
            {
                if (item.ContainerType == "Empty" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Empty Container Gate In";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 2 && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Road";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 1 && item.TransportFrom == "L" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD Loni)";
                else if (item.ContainerType == "Loaded" && item.TransportMode == 1 && item.TransportFrom == "T" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD TKD)";
                else if (item.ContainerType == "LoadedVehicle" && item.TransportMode == 1 && item.TransportFrom == "T" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD TKD)";
                else if (item.ContainerType == "LoadedVehicle" && item.TransportMode == 1 && item.TransportFrom == "L" && InvoiceModuleName == "GATE ENTRY")
                    Container = "Loaded Container By Train(By ICD Loni)";
                else if (item.CBT == " 1" && InvoiceModuleName == "GATE ENTRY")
                    Container = "CBT";

            });
            lstInvoice.ToList().ForEach(item =>
            {

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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                RemarksValue = item.Remarks;

                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    //html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");

                if (InvoiceModuleName == "IRR")
                {
                    lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                        html.Append("<thead><tr>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Train No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Wagon No</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>VIA</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Gross Wt</th>");
                        html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Tare Wt</th>");
                        html.Append("</tr></thead>");
                        html.Append("<tbody><tr>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TrainNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.WagonNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.VIA + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.GrossWt + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TareWt + "</td>");
                        html.Append("</tr></tbody>");
                        html.Append("</table></td></tr>");
                    });
                }

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    if (InvoiceModuleName == "GATE ENTRY")
                    {
                        elem.ToDate = elem.ArrivalDate;
                    }
                    if (InvoiceModuleName == "FCL To LCL CONVERSION")
                    {
                        elem.ToDate = item.DeliveryDate;
                    }


                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ToDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");

                if (InvoiceModuleName == "MISCELLANEOUS" || InvoiceModuleName == "CONTAINER DEBIT INVOICE")
                {
                    html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='12'>Remarks:");
                    html.Append("<label style='font-weight: bold;'>" + RemarksValue + " </label></td></tr>");
                }


                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:6pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
        public string GeneratingBulkPDFforFumigationInvoice(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            Einvoice obj = new Einvoice();
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargo = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[5]);
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
            lstInvoice.ToList().ForEach(item =>
            {
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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px; width: 2%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px; width: 15%;'>Chemical Name</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 10%;'>Area</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstCargo.Where(x => x.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 2%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 15%;'>" + elem.Chemical + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 10%;'>" + elem.Area + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + item.StuffingReqDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ToDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt;margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                if (InvoiceModuleName == "MISCELLANEOUS" || InvoiceModuleName.ToUpper() == "FUMIGATION")
                {
                    html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='12'>Remarks: ");
                    html.Append("<label style='font-weight: bold;'>" + item.Remarks + " </label></td></tr>");
                }


                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:6pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
        public string GeneratingBulkPDFforCargoSF(DataSet ds, string InvoiceModuleName, string All)
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
                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                PPGBTTCargoDet objCargoDet = new PPGBTTCargoDet();
                List<PPGBTTCargoDet> SbNoList = new List<PPGBTTCargoDet>();
                rep.GetCargoDetShiftById(item.InvoiceNo);
                if (rep.DBResponse.Data != null)
                {
                    /// objCargoDet = (PPGBTTCargoDet)rep.DBResponse.Data;
                    SbNoList = (List<PPGBTTCargoDet>)rep.DBResponse.Data;
                }

                var exporter = SbNoList.Select(x => x.exporter).Distinct();
                String ExportName = exporter.FirstOrDefault();
                // String Exporter = exporter[0].toString();

                Ppg_ReportRepository repp = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                repp.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (repp.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)repp.DBResponse.Data;
                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi , Pin - 110096</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label>");
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
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

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
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + ExportName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b></td>");
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

                SbNoList.ForEach(elemCargo =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'> " + elemCargo.ShippingBillNo.ToString() + " </td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + elemCargo.ShippingBillDate.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + elemCargo.CommodityName.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + elemCargo.NoOfUnits.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; width:10%;'>" + elemCargo.GrossWeight.ToString() + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align:center; width:10%;'> " + elemCargo.Fob.ToString() + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                html.Append("</tbody></table></td></tr>");


                html.Append("<tr><th><h3 style='text-align: left; font-size: 13px; margin-top: 20px;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
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
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
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
                //html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr></tbody></table>");
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


        [NonAction]
        public string GeneratingBulkPDFforSealCutting(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
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
            lstInvoice.ToList().ForEach(item =>
            {
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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                Ppg_ImportRepository rep = new Ppg_ImportRepository();
                PPGSealCuttingDateForReport objseal = new PPGSealCuttingDateForReport();
                rep.GetSealCuttingDateId(item.StuffingReqNo);
                if (rep.DBResponse.Data != null)
                {
                    objseal = (PPGSealCuttingDateForReport)rep.DBResponse.Data;
                }
                Ppg_ReportRepository repp = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                repp.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (repp.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)repp.DBResponse.Data;
                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");
                html.Append("<tr><th style='text-align: left; font-size: 13px;margin-top: 0;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>From Date</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>To Date</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + objseal.GateInDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + objseal.TranscationDate + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt;margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                //html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
            var SEZis = "";
            string dtype = "Date of Arrival";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstReasses = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
            List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            string Container = "";
            string cfscode = "";
            lstReasses.ForEach(item =>
            {
                if (item.cfscode != "")
                    Container = "(Re-Assessment)";

            });

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




            lstInvoice.ToList().ForEach(item =>
            {

                //for Bulk

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



                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;


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

                lstReassesment.ForEach(data =>
                {
                    if (data.cfscode == cfscode)
                    {
                        Container = "(Re-Assessment)";
                        dtype = "Previous Delivery Date";
                    }


                });
                StringBuilder html = new StringBuilder();
                /*Header Part*/

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label><label style='font-size: 7pt; font-weight:bold;' >( " + Container + " )</ label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 0;'cellspacing='0' cellpadding='5'>");

                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");


                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Place Of Supply</th><th>:</th><td colspan='6' width='70%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");

                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'></th></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th style='border-top: 1px solid #000;'><b style='text-align: left; font-size: 7pt;margin-top: 0;'>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br /><b style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + dtype + " </th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Weight</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Days</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>Cargo Type</th>");
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

                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.GrossWt.ToString("0.000") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + objDaysWeeks.Days + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: center; width:8%;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
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
                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 6pt;margin-top: 0; margin-bottom: 0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: center; font-size: 6pt; width: 100px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");

                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 0; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='3' width='25%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='3' width='25%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");                

                html.Append("<tr><th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 7pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/>(Authorized Signatories)</th></tr>");
                //html.Append("<td style='font-size: 11px; text-align: left;' colspan='5' width='41.66666666666667%'><br/><br/>For Central Warehousing Corporation <br/>(Authorized Signatories)</td>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:7pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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

            Einvoice obj = new Einvoice(new HeaderParam(), "");

            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstCargoDetail = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[4]);
            List<dynamic> lstReassesment = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);
            List<dynamic> lstReassesbulk = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
            List<string> lstSB = new List<string>();
            int cargotype = 0;
            string Container = "";
            string dt = "";
            string dttype = "Date of Seal Cutting";

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
            lstCargoDetail.ToList().ForEach(item =>
            {
                cargotype = (int)item.CargoType;
            });

            lstInvoice.ToList().ForEach(item =>
            {
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


                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;




                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }



                lstReassesment.ForEach(itemm =>
                {
                    if ((itemm.cfscode != "") && itemm.invoiceid == item.InvoiceId)
                    {
                        Container = "(Re-Assessment)";

                        Ppg_ReportRepository repp = new Ppg_ReportRepository();
                        dt = repp.GetPreviousInvDate(itemm.cfscode);
                        dttype = "Previous Delivery Date";


                    }

                });


                lstReassesbulk.ForEach(data =>
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
                //html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                //html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
                //html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label>< label style = 'font-size: 14px; font-weight:bold;' > " + Container + " </ label > ");
                //html.Append("</td></tr>");

                //html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                //html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label><label style='font-size:7pt; font-weight:bold;'>( " + Container + " )</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    //html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom: 1px solid #000;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><th style='text-align: left; font-size: 7px; margin-top: 10px;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br/><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Arrival</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Carting</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Destuffing</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Days</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Week</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + dttype + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>Cargo Type</th>");
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
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.ArrivalDate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.CartingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DestuffingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1).ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.SealCuttingDt)).TotalDays + 1)) / 7)).ToString() + "</td>");
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

                html.Append("<td colspan='6' width='50%'>");
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

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom:0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
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
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + totamt.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='8'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='3' width='25%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='75%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='3' width='25%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");


                html.Append("<tr><th style='font-size: 6pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Signature CHA / Importer</th>");
                html.Append("<th style='font-size: 6pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>Assistant <br/>(Signature)</th>");
                html.Append("<th style='font-size: 6pt; text-align: left;' colspan='4' width='33.33333333333333%'><br/>For Central Warehousing Corporation<br/>(Authorized Signatories)</th></tr>");
                //html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>For Central Warehousing Corporation <br/>(Authorized Signatories)</td>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:6pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
                /***************/

                html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                lstSB.Add(html.ToString());
                Container = "";
                dt = "";
                dttype = "Date of Seal Cutting";
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
        public string GeneratingBulkPDFforContainer(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstShipBill = null;
            if (ds.Tables[6].Rows.Count > 0)
            {
                lstShipBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            }
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



            lstInvoice.ToList().ForEach(item =>
            {


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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;



                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br/><br/><b style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Details :</b> </th></tr>");

                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Arrival</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Carting</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Destuffing</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Delivery</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Days</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Week</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.ArrivalDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.CartingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DestuffingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DeliveryDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + item.TotDays + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + item.TotWeeks + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: center; width:8%;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                //Shipping bill wise amount for 3.2
                if (lstShipBill != null)
                {
                    if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId).ToList().Count > 0)
                    {
                        html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                        html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Shipping Bill No.</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Cargo Type</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Gate IN Date</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>INS</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Storage</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Shifting</th>");
                        html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>HND</th>");
                        html.Append("</tr></thead><tbody>");

                        lstShipBill.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.shippingbillno).Distinct().ToList().ForEach(data =>
                        {
                            string CargoType = "";
                            if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.CargoType).FirstOrDefault() == 1)
                                CargoType = "Haz";
                            else if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.CargoType).FirstOrDefault() == 2)
                                CargoType = "Non-Haz";
                            else CargoType = "";
                            decimal INSCharge = 0.00M, STOCharge = 0.00M, SHIFTCharge = 0.00M, HNDCharge = 0.00M;
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "INS").ToList().ForEach(chrg =>
                            {
                                INSCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "GEN").ToList().ForEach(chrg =>
                            {
                                STOCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "SHIFT").ToList().ForEach(chrg =>
                            {
                                SHIFTCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "HND").ToList().ForEach(chrg =>
                            {
                                HNDCharge += chrg.amount;
                            });

                            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + data + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + CargoType + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.GateInDate).FirstOrDefault() + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + INSCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + STOCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + SHIFTCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-bottom: 1px solid #000; text-align: center; width:8%;'>" + HNDCharge.ToString("0.##") + "</td>");
                            html.Append("</tr>");
                        });

                        html.Append("</tbody></table></td></tr>");
                    }
                }
                html.Append("<tr><td><span><br/></span></td></tr>");

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
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
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + totamt.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");


                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
        public string GeneratingBulkPDFforCCINEntry(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
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


            lstInvoice.ToList().ForEach(item =>
            {
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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;



                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                CCINEntry objCCINEntry = new CCINEntry();
                rep.GetCCINEntryById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCCINEntry = (CCINEntry)rep.DBResponse.Data;
                }

                Ppg_ReportRepository repp = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                repp.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (repp.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)repp.DBResponse.Data;
                }
                // 'SB Types 1. Baggage 2. Duty Free Goods 3. Cargo in Drawback',
                string SBTypeLabel = "";
                string cargodesc = "";


                cargodesc = objCCINEntry.CommodityName;
                if (objCCINEntry.SBType == 1)
                {
                    SBTypeLabel = "Baggage";
                }
                else if (objCCINEntry.SBType == 2)
                {
                    SBTypeLabel = "Duty Free Goods";
                }
                else if (objCCINEntry.SBType == 3)
                {
                    SBTypeLabel = "Cargo in Drawback";
                }
                StringBuilder html = new StringBuilder();
                int i = 1;
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi , Pin - 110096</span>");
                html.Append("<br /><label style='font-size: 14px; font-weight:bold;'>" + InvoiceModuleName + "</label>");
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
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

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
                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Invoice Generated : By </label> <span>" + item.PaymentMode + "</span></td>");
                //html.Append("</tbody> ");
                // <tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + item.StuffingReqNo + "</b>
                //html.Append("</table></td></tr>");

                html.Append("<tr><td>");

                html.Append("<table style='border:1px solid #000; width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>CCIN NO</td><td>:</td><td colspan='6' width='70%'>" + item.StuffingReqNo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CCIN DT</td><td>:</td><td colspan='6' width='70%'>" + Convert.ToDateTime(item.StuffingReqDate).ToString("dd/MM/yyyy") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>SB NO</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.SBNo + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>SB DATE</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.SBDate + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>SB Type</td><td>:</td><td colspan='6' width='70%'>" + SBTypeLabel + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shipping Line</td><td>:</td><td colspan='6' width='70%'>" + item.ShippingLineName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>FOB</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.FOB + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Cargo Description</td><td>:</td><td colspan='6' width='70%'>" + cargodesc + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Cargo Type</td><td>:</td><td colspan='6' width='70%'>" + (objCCINEntry.CargoType == 0 ? "" : (objCCINEntry.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='50%'>");
                html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td colspan='6' width='30%'>Exporter</td><td>:</td><td colspan='6' width='70%'>" + item.ExporterImporterName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>CHA Name</td><td>:</td><td colspan='6' width='70%'>" + item.CHAName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>No Of Pkg</td><td>:</td><td colspan='6' width='70%'>" + item.TotalNoOfPackages.ToString() + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Total Gr.Wt (In Kg)</td><td>:</td><td colspan='6' width='70%'>" + item.TotalGrossWt.ToString("0.000") + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>SB Processed At</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.PortOfLoadingName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Shed / Godown No.</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.GodownName + "</td></tr>");
                html.Append("<tr><td colspan='6' width='30%'>Carting Type</td><td>:</td><td colspan='6' width='70%'>" + objCCINEntry.CartingType + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("</tr></tbody></table>");

                html.Append("</td></tr>");


                html.Append("<tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Cargo Charges :</h3> </th></tr><tr><td>");
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
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td>");

                html.Append("<tr><td colspan='12' style='font-size: 11px; text-align: left;'><br/><br/><b>Remarks</b> : " + objCCINEntry.Remarks + "</td></tr>");
                html.Append("<tr><td colspan='3' width='25%' style='font-size: 11px; text-align: left;'><br/><br/>&nbsp;</td>");
                html.Append("<td colspan='3' width='25%' style='font-size: 11px; text-align: left;'><br/><br/>&nbsp;</td>");
                html.Append("<td colspan='3' width='25%' style='font-size: 11px; text-align: right;'><br/><br/>(Sign. of Exporter / CHA)</td>");
                html.Append("<td colspan='3' width='25%' style='font-size: 11px; text-align: right;'><br/><br/>(Sign. of Shed Incharge)</td></tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");

                html.Append("</tbody></table>");
                html.Append("</td></tr></tbody></table>");


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
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
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

            lstInvoice.ToList().ForEach(item =>
            {


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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;



                Ppg_ExportRepository rep = new Ppg_ExportRepository();
                PPGBTTCargoDet objCargoDet = new PPGBTTCargoDet();
                rep.GetCargoDetBTTById(Convert.ToInt32(item.StuffingReqId));
                if (rep.DBResponse.Data != null)
                {
                    objCargoDet = (PPGBTTCargoDet)rep.DBResponse.Data;
                }

                Ppg_ReportRepository repp = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                repp.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (repp.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)repp.DBResponse.Data;
                }

                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "<br /> " + item.TransportMode + " " + item.Via + " </label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
                html.Append("<table style='width:100%; font-size:7pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
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
                html.Append("<table style='width:100%; font-size:7pt; border:1px solid #333; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody><tr>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Exporter :</b> " + item.ExporterImporterName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>CHA :</b> " + item.CHAName + " </td>");
                html.Append("<td colspan='4' width='33.33333333333333%'><b>Shipping Line :</b>" + item.ShippingLineName + "</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                html.Append("<tr><th><b style='text-align: left; font-size: 7pt; margin-top: 0;'>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
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

                html.Append("<tr><th><b style='text-align: left; font-size: 6pt; margin-top: 0;'>Cargo Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
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


                html.Append("<tr><th><h3 style='text-align: left; font-size: 6pt; margin-top: 0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                //html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
        public string GeneratingBulkPDFforAuction(DataSet ds, string InvoiceModuleName, string All)
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
                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }
                StringBuilder html = new StringBuilder();
                int i = 1;
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
                html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi , Pin - 110096</span>");
                html.Append("<br />" + InvoiceModuleName);
                html.Append("</td></tr>");
                html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");
                html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + item.InvoiceNo + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + item.InvoiceDate + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
                html.Append("<span>" + item.PartyName + "</span></td>");
                html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + item.PartyState + "</span> </td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party Address :</label> <span>" + item.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + item.PartyStateCode + "</span></td></tr>");
                html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
                html.Append("Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
                html.Append("<label style='font-weight: bold;'>Invoice Generated By :</label> <span>" + item.PaymentMode + "</span></td></tr>");
                html.Append("<label style='font-weight: bold;'>Place Of Supply :</label> <span>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</span></td></tr>");
                html.Append("<label style='font-weight: bold;'>Is Service :</label> <span>" + "Yes" + "</span></td></tr>");
                html.Append("<label style='font-weight: bold;'>Supply Type :</label> <span>" + item.SupplyType + "</span></td></tr>");



                //html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + item.PartyGSTNo + "</span></td><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Invoice Generated : By </label> <span>" + item.PaymentMode + "</span></td>");
                html.Append("</tbody> ");
                html.Append("</table></td></tr><tr><td><hr /></td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tr><td style='font-size: 12px;'>BID No : " + item.StuffingReqNo + " </td>");
                html.Append("<td style='font-size: 12px;'>BID Date : " + Convert.ToDateTime(item.StuffingReqDate).ToString("dd/MM/yyyy") + "</td></tr>");
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
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
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
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='2'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;'colspan='2'></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 9px; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");

                html.Append("</td></tr></tbody></table>");
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
            Einvoice obj = new Einvoice(new HeaderParam(), "");
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

            int flagfordate = 0;
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


            lstInvoice.ToList().ForEach(item =>
            {

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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;


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
                flagfordate = 0;

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
                        flagfordate = 1;

                    }
                });


                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight:bold;'>" + InvoiceModuleName + "</label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0' style='border-bottom: 1px solid #000;'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");
                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><th><b style='text-align: left; font-size: 7pt;margin-top: 0;'>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br /><b style='text-align: left; font-size: 7pt;margin-top: 0;'>Container Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
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
                    if (flagfordate == 0)
                    {
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays + 1).ToString() + "</td>");
                    }
                    if (flagfordate == 1)
                    {
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center;'>" + ((Convert.ToDateTime(item.DeliveryDate) - Convert.ToDateTime(elem.DestuffingEntryDate)).TotalDays).ToString() + "</td>");
                    }
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
                html.Append("<table style='width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'><tbody>");
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

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom:0;'> Container Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
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
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + totamt.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr><td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>Signature CHA / Importer</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");

                if (item.InvoiceType == "Tax(0)")
                {
                    html.Append("<tr><td colspan='12' style='font-size:6pt;'><p>SUPPLY MEANT FOR EXPORT/SUPPLY TO SEZ UNIT OR SEZ DEVELOPER FOR AUTHORISED OPERATIONS UNDER BOND OR LETTER OF UNDERTAKING WITHOUT PAYMENT OF INTEGRATED TAX </p></td></tr>");
                }

                html.Append("</tbody></table>");

                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");
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

        //Update Auction Report
        [NonAction]
        public string GeneratingAssessmentSheetReportAuction(DataSet ds, string InvoiceModuleName, string All)
        {
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            if (ds != null)
            {
                List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
                List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
                List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[10]);
                List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
                List<dynamic> lstBidderDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[9]);
                List<dynamic> lstContainerBidderDetails = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[8]);

                List<string> lstSB = new List<string>();


                #region for Bid Details
                lstInvoice.ToList().ForEach(item =>
                {
                    Ppg_ReportRepository rep = new Ppg_ReportRepository();
                    PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                    rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                    if (rep.DBResponse.Data != null)
                    {
                        objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                    }
                    StringBuilder html = new StringBuilder();
                    int i = 1;
                    /*Header Part*/
                    html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                    html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                    html.Append("<td colspan='8' width='90%' style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                    html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                    html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi , Pin - 110096</span>");
                    html.Append("<br />" + InvoiceModuleName);
                    html.Append("</td></tr>");

                    html.Append("<tr><td colspan='12' style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
                    html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");

                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><td colspan='12'><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                    html.Append("<tr><td colspan='12'>");
                    html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                    html.Append("<td colspan='5' width='50%'>");
                    html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].InvoiceNo + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Assessment Sheet No</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].AssessmentSheet + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].Party + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].Address + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].GstNo + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>BID No.</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].BidNo + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].StateName + " (" + lstBidderDetails[0].PartyStateCode + ")" + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");

                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("<td colspan='6' width='40%'>");
                    html.Append("<table style='width:100%; font-size:9pt; margin:0 0 20px;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><th colspan='6' width='50%'>Tax Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].InvoiceDate + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='30%'>Date</th><th>:</th><td colspan='6' width='70%'>" + lstBidderDetails[0].AssesmentDate + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].StateName + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'> </th><th></th><td colspan='6' width='50%'></td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Shed No/Godown No.</th><th>:</th><td colspan='6' width='50%'>" + lstBidderDetails[0].GodownName + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td></tr>");

                    html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>B. PARTICULARS OF CONTAINER :</b> </th></tr>");
                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7.5pt;' cellspacing='0' cellpadding='10'>");
                    html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>CFS Code</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Container No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Size</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Date of Arrival</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Free Dt. From</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Free Dt Upto</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Date of Delivery</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>No of Days</th>");
                    html.Append("<th style='border-bottom: 1px solid #000; text-align:center;'>No Week</th>");
                    html.Append("</tr></thead><tbody>");
                    /*************/
                    i = 1;
                    lstContainerBidderDetails.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Arrival + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.FeeDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.FreeUpto + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.DeliveryDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Days + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + elem.Weeks + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                    html.Append("</tbody></table></td></tr>");
                    html.Append("<tr><th colspan='12'><b style='text-align: left; font-size: 13px;margin-top: 10px;'>B. PARTICULARS OF CARGO :</b> </th></tr>");
                    html.Append("<tr><td colspan='12'><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:7pt;' cellspacing='0' cellpadding='10'>");
                    html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SR No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>IGM No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>OBL No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>SB No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>BOE No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>BEO Date</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>CIF/FOB Value</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Duty</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Cargo Description</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>No Of Pkg.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Total Gr Wt.(In Kg)</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Total Area</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center; padding: 5px;'>Type Of Cargo</th>");

                    html.Append("</tr></thead><tbody>");
                    /*************/
                    i = 1;
                    lstContainer.Where(y => y.InvoiceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'></td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.OBL + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.SB + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Boe + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.BoeDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CIFFOB + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Duty + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.CargoDescription + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Noofpkg + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Weight + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align:center;'>" + elem.Area + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; text-align:center;'>" + elem.CommodityType + "</td>");
                        html.Append("</tr>");
                        i = i + 1;
                    });
                    html.Append("</tbody></table></td></tr>");
                    html.Append("<tr><td><span><br/></span></td></tr>");
                    html.Append("<tr><th colspan='12'><h3 style='text-align: left; font-size: 13px;'>Container Charges :</h3> </th></tr>");
                    html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
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
                    html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total</th></tr>");
                    html.Append("<tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Rate</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                    html.Append("<tbody>");
                    i = 1;
                    /*Charges Bind*/

                    decimal Taxable = 0M;
                    decimal CGSTPer = 0M;
                    decimal CGSTAmt = 0M;
                    decimal SGSTPer = 0M;
                    decimal SGSTAmt = 0M;
                    decimal IGSTPer = 0M;
                    decimal IGSTAmt = 0M;
                    decimal Total = 0M;
                    decimal Rate = 0M;

                    lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.Rate.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + data.Taxable.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + data.Total.ToString("0") + "</td></tr>");
                        Rate = Rate + Convert.ToDecimal(data.Rate.ToString("0"));
                        Taxable = Taxable + Convert.ToDecimal(data.Taxable.ToString("0"));
                        CGSTPer = CGSTPer + Convert.ToDecimal(data.CGSTPer.ToString("0"));
                        CGSTAmt = CGSTAmt + Convert.ToDecimal(data.CGSTAmt.ToString("0"));
                        SGSTPer = SGSTPer + Convert.ToDecimal(data.SGSTPer.ToString("0"));
                        SGSTAmt = SGSTAmt + Convert.ToDecimal(data.SGSTAmt.ToString("0"));
                        IGSTPer = IGSTPer + Convert.ToDecimal(data.IGSTPer.ToString("0"));
                        IGSTAmt = IGSTAmt + Convert.ToDecimal(data.IGSTAmt.ToString("0"));
                        Total = Total + Convert.ToDecimal(data.Total.ToString("0"));
                        i = i + 1;
                    });
                    html.Append("<tr><th colspan='4' style='border-right: 1px solid #000; font-size: 10px; text-align: left; width: 100px;'>TOTAL</th>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + Rate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + Taxable + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + CGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + SGSTAmt + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTPer + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + IGSTAmt + "</td>");
                    html.Append("<td style='font-size: 10px; text-align: center; width: 100px;'>" + Total + "</td></tr>");
                    html.Append("</tbody>");
                    html.Append("</table></td></tr></tbody></table></td></tr>");
                    html.Append("<tr>");
                    html.Append("<th style='border-top: 1px solid #000; border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0")) + "</th>");
                    html.Append("</tr>");
                    html.Append("</tbody></table>");

                    html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody>");
                    html.Append("<tr><th><span><br/><br/></span></th></tr>");
                    html.Append("<tr><th colspan='7' width='70%' style='font-size: 8pt;'>RECEIPT/DD No.</th> <th colspan='3' width='30%' style='font-size: 8pt;'>Mode of Receipt.</th></tr>");
                    html.Append("<tr><th><span><br/></span></th></tr>");
                    html.Append("<tr><th colspan='12' style='font-size: 8pt; text-align: right;'>For Central Warehousing Corporation<br/>Authorized Signatories</th></tr>");
                    html.Append("</tbody></table>");

                    /***************/

                    html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));

                    lstSB.Add(html.ToString());
                });

                #endregion

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
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;

        }

        [NonAction]
        public string GeneratingBulkPDFforTrainSummary(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            //If any changes required then also do the chages in Ppg_CWCImportV2Controller with same method 
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            // List<dynamic> lstHeader = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[7]);
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


            lstInvoice.ToList().ForEach(item =>
            {

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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;


                //Ppg_ReportRepository rep = new Ppg_ReportRepository();
                //PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                // RemarksValue = item.Remarks;

                //rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                //if (rep.DBResponse.Data != null)
                // {
                //     objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                // }
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "<br /> " + item.TransportMode + " " + item.Via + " </label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");

                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%'>");
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

                html.Append("<tr><td><hr/></td></tr>");

                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Train No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Train Dt</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Gross Wt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Port</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Via</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TrainNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.TrainDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.GrossWt.ToString("0.####") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.PortName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Via + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");
                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin: 0;'>Container / CBT Charges :</h3></th></tr><tr><td>");
                html.Append("<table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
                html.Append("<tbody>");
                i = 1;
                /*Charges Bind*/
                lstCharges.Where(y => y.InoviceId == item.InvoiceId).ToList().ForEach(data =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>" + data.OperationSDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>" + data.OperationDesc + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + data.SACCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                    i = i + 1;
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + item.TotalTaxable.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");

                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>*Cheques are subject to realisation</td>");
                html.Append("<td style='font-size: 6pt; text-align: left; line-height: 30px;' colspan='6' width='50%'>SAM(A/C)</td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'><br/><br/>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
            Loni_ReportRepositoryV2 ObjRR = new Loni_ReportRepositoryV2();
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
        //public ActionResult Download()
        //{
        //    String session = Session.SessionID;
        //    Ppg_ReportCWCController obj = new Ppg_ReportCWCController();
        //    //////int CurrentFileID = Convert.ToInt32(FileID); 
        //    string fileSavePath = "";
        //    fileSavePath = Server.MapPath("~/Docs/All/") + Session.SessionID;
        //    var filesCol = obj.GetFile(fileSavePath).ToList();
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            for (int i = 0; i < filesCol.Count; i++)
        //            {
        //                ziparchive.CreateEntryFromFile(filesCol[i].FilePath, filesCol[i].FileName);
        //            }
        //        }
        //        return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
        //    }
        //}

        public JsonResult Download()
        {
            String session = Session.SessionID;
            Ppg_ReportCWCV2Controller obj = new Ppg_ReportCWCV2Controller();
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
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    System.IO.File.Delete(file);
                }
                //Delete all child Directories
                //foreach (string directory in Directory.GetDirectories(path))
                //{
                //    DeleteDirectory(directory);
                //}
                ////Delete a Directory
                //Directory.Delete(path);
            }
        }
        [NonAction]
        public string GeneratingBulkPDFforContainerMovement(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<dynamic> lstShipBill = null;
            if (ds.Tables[6].Rows.Count > 0)
            {
                lstShipBill = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[6]);
            }
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

            lstInvoice.ToList().ForEach(item =>
            {
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

                Einvoice Eobj = new Einvoice();
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(upiQRInfo);
                IrnResponse objERes = new IrnResponse();
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                Ppg_ReportRepository rep = new Ppg_ReportRepository();
                /*PPGSDBalancePrint objSDBalance = new PPGSDBalancePrint();
                rep.GetSDBalanceforReport(item.PayeeId, item.InvoiceId);
                if (rep.DBResponse.Data != null)
                {
                    objSDBalance = (PPGSDBalancePrint)rep.DBResponse.Data;
                }*/
                StringBuilder html = new StringBuilder();
                /*Header Part*/
                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='83%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>" + InvoiceModuleName + "<br /> " + item.TransportMode + " " + item.Via + " </label></td>");
                html.Append("</tr>");
                html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("<tr><td colspan='12'><span style='display: block; font-size: 7pt; text-transform: uppercase;'>");
                html.Append("CWC GST No. <label>" + objCompany[0].GstIn + "</label></span></td></tr>");
                html.Append("</tbody></table></td>");
                //html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");

                if (item.SupplyType == "B2C")
                {
                    html.Append("<td width='200px'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; font-size:7pt'><tbody>");
                    html.Append("<tr><td align='center' valign='top'>Scan & Pay through any Bharat QR/UPI App</td></tr>");
                    html.Append("<tr><td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td></tr>");
                    html.Append("</tbody></table></td>");
                }
                else
                {
                    html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                }

                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");

                html.Append("<tr><td colspan='12'><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
                html.Append("<tbody><tr><td><h2 style='font-size: 13px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>" + item.InvoiceType + " Invoice</h2> </td></tr>");


                html.Append("<tr><td>");
                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr>");
                html.Append("<td colspan='5' width='50%' style='border-bottom: 1px solid #000;'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='30%'>Invoice No</th><th>:</th><td colspan='6' width='70%'>" + item.InvoiceNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Name</th><th>:</th><td colspan='6' width='70%'>" + item.PartyName + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party Address</th><th>:</th><td colspan='6' width='70%'>" + item.PartyAddress + "</td></tr>");
                html.Append("<tr><th colspan='6' width='30%'>Party GST</th><th>:</th><td colspan='6' width='70%'>" + item.PartyGSTNo + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Place Of Supply</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + " (" + item.PartyStateCode + ")" + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Is Service</th><th>:</th><td colspan='6' width='50%'>" + "Yes" + "</td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");

                html.Append("<td colspan='6' width='40%' style='border-bottom: 1px solid #000;'>");
                html.Append("<table style='width:100%; font-size:7pt;' cellspacing='0' cellpadding='5'><tbody>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'>Supply Type</th><th>:</th><td colspan='6' width='50%'>" + item.SupplyType + "</td></tr>");
                html.Append("<tr><th colspan='6' width='50%'></th><th></th><td colspan='6' width='50%'></td></tr>");

                html.Append("</tbody></table>");
                html.Append("</td>");
                html.Append("</tr></tbody></table>");
                html.Append("</td></tr>");

                //html.Append("<tr><td><hr/></td></tr>");
                html.Append("<tr><th style='text-align: left; font-size: 7pt; margin-top: 0;'><b>Assessment No :" + item.StuffingReqNo + "</b> ");
                html.Append("<br/><br/><b style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom:0;'>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style='border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>ICD Code</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>Size</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Arrival</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Carting</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Stuffing</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Date of Delivery</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Days</th>");
                //html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>No of Week</th>");
                html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                Decimal totamt = 0;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.CFSCode + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.ArrivalDate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.CartingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + elem.StuffingDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + item.DeliveryDate + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + item.TotDays + "</td>");
                    //html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:5%;'>" + item.TotWeeks + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; text-align: center; width:8%;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                    html.Append("</tr>");
                    i = i + 1;
                });
                /***************/
                html.Append("</tbody></table></td></tr>");

                //Shipping bill wise amount for 3.2
                if (lstShipBill != null)
                {
                    if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId).ToList().Count > 0)
                    {
                        html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%; font-size:6pt;' cellspacing='0' cellpadding='5'>");
                        html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Shipping Bill No.</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Cargo Type</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>Gate IN Date</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>CCIN Date</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>INS</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>GEN</th>");
                        html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>Shifting</th>");
                        html.Append("<th style='border-bottom: 1px solid #000; text-align: center; width:8%;'>HND</th>");
                        html.Append("</tr></thead><tbody>");

                        lstShipBill.Where(x => x.InvoiceId == item.InvoiceId).Select(x => x.shippingbillno).Distinct().ToList().ForEach(data =>
                        {
                            string CargoType = "";
                            if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.CargoType).FirstOrDefault() == 1)
                                CargoType = "Haz";
                            else if (lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.CargoType).FirstOrDefault() == 2)
                                CargoType = "Non-Haz";
                            else CargoType = "";
                            decimal INSCharge = 0.00M, STOCharge = 0.00M, SHIFTCharge = 0.00M, HNDCharge = 0.00M;
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "INS").ToList().ForEach(chrg =>
                            {
                                INSCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "GEN").ToList().ForEach(chrg =>
                            {
                                STOCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "SHIFT").ToList().ForEach(chrg =>
                            {
                                SHIFTCharge += chrg.amount;
                            });
                            lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data && x.chargetype == "HND").ToList().ForEach(chrg =>
                            {
                                HNDCharge += chrg.amount;
                            });

                            html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + data + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + CargoType + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.GateInDate).FirstOrDefault() + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:10%;'>" + lstShipBill.Where(x => x.InvoiceId == item.InvoiceId && x.shippingbillno == data).Select(x => x.CCINDate).FirstOrDefault() + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:6%;'>" + INSCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + STOCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; width:8%;'>" + SHIFTCharge.ToString("0.##") + "</td>");
                            html.Append("<td style='border-bottom: 1px solid #000; text-align: center; width:8%;'>" + HNDCharge.ToString("0.##") + "</td>");
                            html.Append("</tr>");
                        });

                        html.Append("</tbody></table></td></tr>");
                    }
                }

                html.Append("<tr><th><h3 style='text-align: left; font-size: 7pt; margin-top: 0; margin-bottom:0;'>Container / CBT Charges :</h3> </th></tr><tr><td>");
                html.Append("<table style='border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<thead><tr>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>SR No.</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'>Charge Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'>Description</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>HSN Code</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'>Rate</th>");
                html.Append("<th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>Taxable Amt.</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>CGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>SGST</th>");
                html.Append("<th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'>IGST</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total</th></tr><tr>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Rate</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>Amt.</th></tr></thead>");
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
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + data.Taxable.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.CGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.SGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTPer.ToString("0.##") + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + data.IGSTAmt.ToString("0.##") + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 150px;'>" + data.Total.ToString("0.##") + "</td></tr>");
                        i = i + 1;
                        totamt = totamt + data.Taxable;
                    }
                });
                html.Append("</tbody>");
                html.Append("</table></td></tr></tbody></table>");


                html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; margin-top:0; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
                html.Append("<tbody>");

                html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>Total :</th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 140px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 24%;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 80px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 130px;'>" + totamt.ToString("0.##") + "</th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 200px;'></th>");
                html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 100px;'>" + item.InvoiceAmt.ToString("0.##") + "</th></tr><tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalCGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalSGST.ToString("0.##") + "</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'></th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: center; width: 50px;'>" + item.TotalIGST.ToString("0.##") + "</th></tr>");

                html.Append("<tr>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 6pt; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(item.InvoiceAmt.ToString("0.##")) + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th style='font-size: 6pt; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");


                html.Append("<table style='width:100%;' cellspacing='0' cellpadding='5'>");
                html.Append("<tbody>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left;' colspan='8' width='80%'>Receipt No.: <label style='font-weight: bold;'></label></td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='4' width='20%'>Payee Code:<label style='font-weight: bold;'>" + item.PartyCode.ToString() + "</label></td></tr>");
                html.Append("<tr><td style='font-size: 6pt; text-align: left;' colspan='8' width='80%'>Payee Name:<label style='font-weight: bold;'>" + item.PayeeName.ToString() + "</label></td></tr>");
                // html.Append("<td style='font-size: 11px; text-align: left; line-height: 30px;' colspan='4' width='20%'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td>");

                //html.Append("<tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'></td>");
                //html.Append("<td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>SD Balance:<label style='font-weight: bold;'>" + objSDBalance.SDBalance.ToString() + "</label></td></tr>");

                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='4' width='25%'><br/><br/>Signature CHA / Importer</td>");
                //html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>Assistant <br/>(Signature)</td>");
                //html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
                //html.Append("<td style='font-size: 11px; text-align: left;' colspan='3' width='25%'><br/><br/>SAM/SIO <br/>(Signature)</td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='3' width='35%'><br/><br/><br/></td>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='5' width='40%'><br/>For Central Warehousing Corporation,<br/>Authorized Signatories<br/></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td style='font-size: 6pt; text-align: left;' colspan='12' width='100%'>THIS IS A COMPUTER-GENERATED TAX INVOICE AND IT DOES NOT REQUIRE A SIGNATURE</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td></tr>");

                if (item.SupplyType != "B2C")
                {
                    html.Append("<tr>");
                    html.Append("<td align='left' valign='top' colspan='6' width='50%'><p style='font-size:7pt; font-weight:bold;'>Scan for Payment</p><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(objERes.SignedInvoice)) + "'/> </td>");
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
                var ObjRR = new Loni_ReportRepositoryV2();
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
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetBulkIrnDetails()
        {
            Loni_ReportRepositoryV2 objPpgRepo = new Loni_ReportRepositoryV2();
            objPpgRepo.GetBulkIrnDetails();
            var Output = (PPG_BulkIRN)objPpgRepo.DBResponse.Data;

            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddEditBulkIRN(FormCollection objForm)
        {
            //Ppg_CWCImportController objPpgIrn = new Ppg_CWCImportController();

            try
            {

                var invoiceData = JsonConvert.DeserializeObject<PPG_BulkIRN>(objForm["PaymentSheetModelJson"]);

                foreach (var item in invoiceData.lstPostPaymentChrg)
                {
                    try
                    {
                        if (item.InvoiceType == "Inv")
                        {
                            var result = await GetIRNForBulkInvoice(item.InvoiceNo, item.SupplyType);
                        }
                        if (item.InvoiceType == "C")
                        {
                            var result1 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "CRN", "C");
                        }
                        if (item.InvoiceType == "D")
                        {
                            var result2 = await GetGenerateIRNForBulkCreditNote(item.InvoiceNo, item.SupplyType, "DBN", "D");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                //foreach (var item in invoiceData.lstOperationContwiseAmt)
                //{
                //    if (item.DocumentDate != "")
                //        item.DocumentDate = Convert.ToDateTime(item.DocumentDate).ToString("yyyy-MM-dd");
                //} 
                return Json(new { Status = 1, Message = "IRN Generated" });

            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        public async Task<JsonResult> GetIRNForBulkInvoice(String InvoiceNo, string SupplyType)
        {

            Ppg_ImportRepository objPpgRepo = new Ppg_ImportRepository();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);

                    Einvoice Eobj = new Einvoice(Hp, jsonEInvoice);

                    IrnResponse ERes = await Eobj.GenerateEinvoice();
                    if (ERes.Status == 0)
                    {
                        log.Info(ERes.ErrorDetails.ErrorMessage);
                        log.Info(ERes.ErrorDetails.ErrorCode);
                        log.Info("Invoice No:" + InvoiceNo);
                        log.Info(ERes.Status);
                    }
                    if (ERes.Status == 1)
                    {
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
                    }
                    else
                    {
                        objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                        objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                    }
                }
                else
                {

                    Einvoice Eobj = new Einvoice();
                    objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                    Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                    irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    var dt = DateTime.Now;
                    Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                    Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                    objQR.Irn = ERes;
                    objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                    objQR.iss = "NIC";
                    objQR.ItemCnt = irnb2cobj.ItemCnt;
                    objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                    objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                    objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                    objQR.IFSC = irnb2cobj.IFSC;
                    objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                    objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                    objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                    objQR.InvoiceValue = irnb2cobj.InvoiceValue;
                    objQR.IGST = irnb2cobj.IGST;
                    objQR.CESS = irnb2cobj.CESS;

                    objQR.CGST = irnb2cobj.CGST;
                    objQR.SGST = irnb2cobj.SGST;



                    //objQR.SellerGstin = irnb2cobj.SellerGstin;
                    //objQR.TotInvVal = irnb2cobj.TotInvVal;
                    //objQR.BuyerGstin = irnb2cobj.BuyerGstin;
                    //objQR.DocDt = irnb2cobj.DocDt;
                    //objQR.DocNo = irnb2cobj.DocNo;
                    //objQR.DocTyp = irnb2cobj.DocTyp;
                    obj.Data = (Ppg_QrCodeData)objQR;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(strQRCode);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                Ppg_IrnB2CDetails irnb2cobj = new Ppg_IrnB2CDetails();
                irnb2cobj = (Ppg_IrnB2CDetails)objPpgRepo.DBResponse.Data;

                IrnModel irnModelObj = new IrnModel();
                irnModelObj.DocumentDate = irnb2cobj.DocDt;
                irnModelObj.DocumentNo = irnb2cobj.DocNo;
                irnModelObj.DocumentType = irnb2cobj.DocTyp;
                irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                var dt = DateTime.Now;
                Ppg_QrCodeInfo obj = new Ppg_QrCodeInfo();
                Ppg_QrCodeData objQR = new Ppg_QrCodeData();
                objQR.Irn = ERes;
                objQR.IrnDt = dt.ToString("dd/MM/yyyy");
                objQR.iss = "NIC";
                objQR.ItemCnt = irnb2cobj.ItemCnt;
                objQR.MainHsnCode = irnb2cobj.MainHsnCode;
                objQR.BankAccountNo = irnb2cobj.BankAccountNo;
                objQR.SupplierGstNo = irnb2cobj.SellerGstin;
                objQR.IFSC = irnb2cobj.IFSC;
                objQR.InvoiceDate = irnb2cobj.InvoiceDate;
                objQR.SupplierUPIID = irnb2cobj.SupplierUPIID;
                objQR.InvoiceNo = irnb2cobj.InvoiceNo;
                objQR.InvoiceValue = irnb2cobj.InvoiceValue;
                objQR.IGST = irnb2cobj.IGST;
                objQR.CESS = irnb2cobj.CESS;

                objQR.CGST = irnb2cobj.CGST;
                objQR.SGST = irnb2cobj.SGST;



                //objQR.SellerGstin = irnb2cobj.SellerGstin;
                //objQR.TotInvVal = irnb2cobj.TotInvVal;
                //objQR.BuyerGstin = irnb2cobj.BuyerGstin;
                //objQR.DocDt = irnb2cobj.DocDt;
                //objQR.DocNo = irnb2cobj.DocNo;
                //objQR.DocTyp = irnb2cobj.DocTyp;
                obj.Data = (Ppg_QrCodeData)objQR;
                B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                objresponse = Eobj.GenerateB2cQRCode(strQRCode);
                IrnResponse objERes = new IrnResponse();
                objERes.irn = ERes;
                objERes.SignedQRCode = objresponse.QrCodeBase64;
                objERes.SignedInvoice = objresponse.QrCodeJson;
                objERes.SignedQRCode = objresponse.QrCodeJson;

                objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);

            }


            return Json(objPpgRepo.DBResponse);
        }

        public async Task<JsonResult> GetGenerateIRNForBulkCreditNote(string CrNoteNo, string SupplyType, string Type, string CRDR)
        {
            Einvoice Eobj;
            IrnResponse ERes = null;

            Ppg_CashManagementRepository objPpgRepo = new Ppg_CashManagementRepository();



            if (SupplyType == "B2C")
            {
                Eobj = new Einvoice();
                IrnModel m1 = new IrnModel();

                Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                //   QrCodeData qdt = new QrCodeData();
                objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                var Output = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;

                m1.DocumentNo = Output.InvoiceNo;
                m1.DocumentDate = Output.InvoiceDate;
                m1.SupplierGstNo = Output.SupplierGstNo;
                m1.DocumentType = Type;
                String IRN = Eobj.GenerateB2cIrn(m1);
                Output.Irn = IRN;
                Output.IrnDt = Output.InvoiceDate;
                Output.iss = "NIC";
                q1.Data = Output;
                B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(strQRCode);
                objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);

                //   return Json(objPpgRepo.DBResponse.Status);
                //   IrnResponse ERes = await Eobj.GenerateB2cIrn();
            }

            else
            {
                objPpgRepo.GetIRNForDebitCredit(CrNoteNo, Type, CRDR);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;
                if (Output.BuyerDtls.Gstin != "" || Output.BuyerDtls.Gstin != null)
                {

                    objPpgRepo.GetHeaderIRNForCreditDebitNote();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);

                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
                    Eobj = new Einvoice(Hp, jsonEInvoice);
                    ERes = await Eobj.GenerateEinvoice();
                    if (ERes.Status == 1)
                    {
                        objPpgRepo.AddEditIRNResponsecForCreditDebitNote(ERes, CrNoteNo, CRDR);
                    }
                    else
                    {
                        objPpgRepo.DBResponse.Message = ERes.ErrorDetails.ErrorMessage;
                        objPpgRepo.DBResponse.Status = Convert.ToInt32(ERes.ErrorDetails.ErrorCode);
                    }
                }
                else
                {

                    Eobj = new Einvoice();
                    IrnModel m1 = new IrnModel();

                    Ppg_QrCodeInfo q1 = new Ppg_QrCodeInfo();
                    //   QrCodeData qdt = new QrCodeData();
                    objPpgRepo.GetIRNForB2CInvoiceForCreditDebitNote(CrNoteNo, Type, CRDR);
                    var OutputData = (Ppg_QrCodeData)objPpgRepo.DBResponse.Data;

                    m1.DocumentNo = OutputData.InvoiceNo;
                    m1.DocumentDate = OutputData.InvoiceDate;
                    m1.SupplierGstNo = OutputData.SupplierGstNo;
                    m1.DocumentType = Type;
                    String IRN = Eobj.GenerateB2cIrn(m1);
                    OutputData.Irn = IRN;
                    OutputData.IrnDt = OutputData.InvoiceDate;
                    OutputData.iss = "NIC";
                    q1.Data = OutputData;
                    B2cQRCodeResponse QRCode = Eobj.GenerateB2cQRCode(strQRCode);
                    objPpgRepo.AddEditIRNB2CCreditDebitNote(IRN, QRCode, CrNoteNo, CRDR);
                }

            }
            // var Images = LoadImage(ERes.QRCodeImageBase64);

            return Json(objPpgRepo.DBResponse);
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
            Loni_ReportRepositoryV2 ObjER = new Loni_ReportRepositoryV2();
            List<Ppg_E04Report> LstE04 = new List<Ppg_E04Report>();
            ObjER.ListofE04Report(0);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Ppg_E04Report>)ObjER.DBResponse.Data;
            }
            return PartialView("ListOfE04DetailsReport", LstE04);
        }
        [HttpGet]
        public JsonResult LoadMoreE04List(int Page)
        {
            Loni_ReportRepositoryV2 ObjER = new Loni_ReportRepositoryV2();
            var LstE04 = new List<Ppg_E04Report>();
            ObjER.ListofE04Report(Page);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Ppg_E04Report>)ObjER.DBResponse.Data;
            }
            return Json(LstE04, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewE04DetailsReport(int ID)
        {
            Ppg_E04Report ObjE04 = new Ppg_E04Report();
            Loni_ReportRepositoryV2 ObjER = new Loni_ReportRepositoryV2();
            ObjER.GetE04DetailById(ID);
            if (ObjER.DBResponse.Data != null)
            {
                ObjE04 = (Ppg_E04Report)ObjER.DBResponse.Data;
            }
            return PartialView(ObjE04);
        }


        [HttpGet]
        public JsonResult GetE04Search(string SB_No = "", string SB_Date = "", string Exp_Name = "")
        {
            Loni_ReportRepositoryV2 ObjER = new Loni_ReportRepositoryV2();
            List<Ppg_E04Report> LstE04 = new List<Ppg_E04Report>();
            ObjER.GetE04DetailSearch(SB_No, SB_Date, Exp_Name);
            if (ObjER.DBResponse.Data != null)
            {
                LstE04 = (List<Ppg_E04Report>)ObjER.DBResponse.Data;
            }
            //return PartialView("ListOfE04DetailsReport", LstE04);
            return Json(ObjER.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region SCMTR
        public ActionResult SCMTRReport()
        {
            return PartialView();
        }
        #endregion
        #region Stuffing Acknowledgement Search       

        [HttpGet]
        public ActionResult StfAckSearch()
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();

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
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllContainerNoForContstufserach(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchShipbill(string shipbill)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadContainerLists(string cont, int Page)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllContainerNoForContstufserach(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadShipbillLists(string shipbill, int Page)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getcontstuffingacksearch(string container, string shipbill, string cfscode)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_ContStufAckRes> Lststufack = new List<Ppg_ContStufAckRes>();
            ObjGR.GetStufAckResult(container, shipbill, cfscode);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Ppg_ContStufAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region SBill Query
        [HttpGet]
        public ActionResult SBQuery()
        {
            var ObjRR = new Loni_ReportRepositoryV2();
            ObjRR.GetAllSB();

            if (ObjRR.DBResponse.Data != null)
            {
                ViewBag.ListOfSB = new SelectList((List<PPG_SBQuery>)ObjRR.DBResponse.Data, "Id", "SBNODate");
            }
            return PartialView();
        }

        // [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetApplicationDetForSBQuery(string Id, string sbno, string sbdate)
        {
            int id = Convert.ToInt32(Id);
            string SBNO = Convert.ToString(sbno);
            PPG_SBQuery objCR = new PPG_SBQuery();
            Loni_ReportRepositoryV2 objRR = new Loni_ReportRepositoryV2();
            objRR.SBQueryReport(id, sbno, sbdate);
            if (objRR.DBResponse.Data != null)
            {
                objCR = (PPG_SBQuery)objRR.DBResponse.Data;
                return Json(objCR, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = -1, Message = "No Data" }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
        #region PortwiseTeus
        public ActionResult PpgPortwiseTeus()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPortWiseTEUs(PPG_teus_search ObjTEUS)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<PPG_teus_search> Lsteus = new List<PPG_teus_search>();
            Login objLogin = (Login)Session["LoginUser"];
            ObjGR.PortTeus(ObjTEUS);//, objLogin.Uid
            if (ObjGR.DBResponse.Data != null)
            {
                Lsteus = (List<PPG_teus_search>)ObjGR.DBResponse.Data;
                return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GenerateTEUSPDF(FormCollection fc)
        {
            try
            {

                //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
                // string FtpIdPath = "";
                //string LocalIdPath = "";
                var Pages = new string[1];
                var FileName = "TEUS.pdf";
                Pages[0] = fc["Page"].ToString();
                // var GovtImg = Server.MapPath("~/Content/Images/gov.png");
                //Pages[0] = fc["Page"].ToString().Replace("GOVT_IMG", GovtImg).Replace("MODEL", DateTime.Today.ToShortDateString()).Replace("DIR/DED.............", fc["WaiverOrderNo"]);
                //int WavOrdrIssueId = Convert.ToInt32(fc["WavOrdrIssueId"]);
                //FtpIdPath = "WBDED/Docs/Waiver/WaiverOrder/" + WavOrdrIssueId;
                // LocalIdPath = Server.MapPath("~/Docs") + "/Report/RenewalPending/";
                string LocalDirectory = Server.MapPath("~/Docs") + "/" + Session.SessionID + "/Report/TEUS/";
                if (!Directory.Exists(LocalDirectory))
                    Directory.CreateDirectory(LocalDirectory);
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, true))
                {
                    ObjPdf.HeadOffice = this.HeadOffice;
                    ObjPdf.HOAddress = this.HOAddress;
                    ObjPdf.ZonalOffice = this.ZonalOffice;
                    ObjPdf.ZOAddress = this.ZOAddress;
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }
                var ObjResult = new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Report/TEUS/" + FileName };
                return Json(ObjResult);

            }
            catch (Exception ex)
            {
                return Json(new { Status = "-1", Message = "", }, JsonRequestBehavior.DenyGet);
            }

        }
        #endregion

        #region ASR Acknowledgement Search       

        [HttpGet]
        public ActionResult StuffingASRAckSearch()
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();

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
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetCotainerNoForASRAck(cont, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadContainerLists(string cont, int Page)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetCotainerNoForASRAck(cont, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRSearchShipbill(string shipbill)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, 0);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ASRLoadShipbillLists(string shipbill, int Page)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            ObjGR.GetAllShippingBillNoForASRACK(shipbill, Page);
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetASRACKStatusSearch(string shipbill, string CFSCode, string container)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_ContStufAckRes> Lststufack = new List<Ppg_ContStufAckRes>();
            ObjGR.GetASRAckResult(shipbill, CFSCode, container);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Ppg_ContStufAckRes>)ObjGR.DBResponse.Data;
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
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_GatePassDPAckSearch> lstDPGPAck = new List<Ppg_GatePassDPAckSearch>();
            ObjGR.GetGatePassNoDPForAckSearch(GatePassNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTGPAck = (List<Ppg_GatePassDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDPAckSearch(string ContainerNo = "", int Page = 0)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_ContDPAckSearch> lstDPContACK = new List<Ppg_ContDPAckSearch>();
            ObjGR.GetContainerNoForDPAckSearch(ContainerNo, Page);
            //if (ObjGR.DBResponse.Data != null)
            //{
            //    lstDTContACK = (List<Ppg_ContDTAckSearch>)ObjGR.DBResponse.Data;
            //}
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDPAckSearch(int GatePassId, string ContainerNo, int GatePassdtlId = 0)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_DPAckRes> lstDPAckRes = new List<Ppg_DPAckRes>();
            ObjGR.GetDPAckSearch(GatePassId, ContainerNo, GatePassdtlId);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDPAckRes = (List<Ppg_DPAckRes>)ObjGR.DBResponse.Data;
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
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_GatePassDTAckSearch> lstDTGPAck = new List<Ppg_GatePassDTAckSearch>();
            ObjGR.GetGatePassNoDTForAckSearch(GatePassNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerNoForDTAckSearch(string ContainerNo = "", int Page = 0)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_ContDTAckSearch> lstDTContACK = new List<Ppg_ContDTAckSearch>();
            ObjGR.GetContainerNoForDTAckSearch(ContainerNo, Page);

            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDTAckSearch(int GatePassId, string ContainerNo)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Ppg_DTAckRes> lstDTAckRes = new List<Ppg_DTAckRes>();
            ObjGR.GetDTAckSearch(GatePassId, ContainerNo);

            if (ObjGR.DBResponse.Data != null)
            {
                lstDTAckRes = (List<Ppg_DTAckRes>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stuffing Loaded Search
        [HttpGet]
        public ActionResult StfLoadSearch()
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();

            /* ObjGR.GetAllContainerNoForContstufserach("", 0);

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
             }*/

            return PartialView();
        }
        /* [HttpGet]
         public JsonResult SearchContainerNo(string cont)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllContainerNoForContstufserach(cont, 0);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }
         [HttpGet]
         public JsonResult SearchShipbill(string shipbill)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllShippingBillNoForContstufserach(shipbill, 0);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }
         [HttpGet]
         public JsonResult LoadContainerLists(string cont, int Page)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllContainerNoForContstufserach(cont, Page);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }


         [HttpGet]
         public JsonResult LoadShipbillLists(string shipbill, int Page)
         {
             ReportRepository ObjGR = new ReportRepository();
             ObjGR.GetAllShippingBillNoForContstufserach(shipbill, Page);
             return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
         }*/
        [HttpGet]
        public JsonResult getloadstufsearch(string jobno)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Kol_loadstuf> Lststufack = new List<Kol_loadstuf>();
            ObjGR.GetStufloadResult(jobno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Kol_loadstuf>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufasrsearch(string jobasrno)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Kol_loadstufasr> Lststufack = new List<Kol_loadstufasr>();
            ObjGR.GetStufloadasrResult(jobasrno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Kol_loadstufasr>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufdpsearch(string jobdpno)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Kol_loadstufdp> Lststufack = new List<Kol_loadstufdp>();
            ObjGR.GetStufloaddpResult(jobdpno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Kol_loadstufdp>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getloadstufdtsearch(string jobdtno)
        {
            Loni_ReportRepositoryV2 ObjGR = new Loni_ReportRepositoryV2();
            List<Kol_loadstufdt> Lststufack = new List<Kol_loadstufdt>();
            ObjGR.GetStufloaddpResult(jobdtno);


            if (ObjGR.DBResponse.Data != null)
            {
                Lststufack = (List<Kol_loadstufdt>)ObjGR.DBResponse.Data;
            }
            return Json(ObjGR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region   Tally Response Report 
        //25-OCT-2021
        public ActionResult TallyResponseReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TallyResponseReport(TallyResponse vm)
        {
            Loni_ReportRepositoryV2 ObjRR = new Loni_ReportRepositoryV2();
            List<TallyResponse> lstData = new List<TallyResponse>();

            ObjRR.GetTallyResponse(vm);//, objLogin.Uid
            string Path = "";
            if (ObjRR.DBResponse.Data != null)
            {

                lstData = (List<TallyResponse>)ObjRR.DBResponse.Data;
                Path = GenerateTallyResponseReport(lstData, vm.FromDate, vm.ToDate);
                return Json(new { Status = 1, Data = Path }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Message = "No Record Found.." }, JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public string GenerateTallyResponseReport(List<TallyResponse> lstData, string FromDate, string ToDate)
        {
            try
            {

                CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
                Loni_ReportRepositoryV2 ObjRR = new Loni_ReportRepositoryV2();
                ObjRR.getCompanyDetails();
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                var FileName = "TallyResponseReport.pdf";
                StringBuilder Pages = new StringBuilder();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Report";

                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><thead>");
                Pages.Append("<tr><td valign='top' width='10%'><img align='right' src='IMGSRC'/></td>");
                Pages.Append("<td width='70%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                Pages.Append("<label style='font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                Pages.Append("<span style='font-size: 7pt; padding-bottom: 10px;'>107-109 , EPIP Zone , KIADB Industrial Area <br/>" + objCompanyDetails.CompanyAddress + "</span><br/><label style='font-size: 7pt;'>Email - " + objCompanyDetails.EmailAddress + "</label>");
                Pages.Append("<br /><label style='font-size: 7pt; font-weight:bold;'>Tally Response Report</label> <br/> <label style='font-size: 7pt; font-weight:bold;' ><b>From Date :</b> " + FromDate + " <b>To Date :</b> " + ToDate + "</label>");
                Pages.Append("</td>");
                Pages.Append("<td width='10%' valign='top'><img align='right' src='ISO_IMG'/></td>");
                Pages.Append("<td width='10%' valign='top'><img align='right' src='SWACHBHARAT'/></td>");
                Pages.Append("</tr>");
                Pages.Append("</thead></table>");

                int i = 1;
                Pages.Append("<table cellspacing='0' cellpadding='5' style='width:100%; margin-bottom:10px; border-collapse:collapse; border:1px solid #000; border-bottom:0; text-align: center; font-size:7pt;'>");
                Pages.Append("<thead><tr>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Sl.No.</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 10%;'>Date</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 15%;'>Bill</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 12%;'>Invoice</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 5%;'>Debit Note</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 12%;'>Credit Note</th>");
                Pages.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; width: 15%;'>Receipt Voucher</th>");
                Pages.Append("</tr></thead>");
                Pages.Append("<tbody>");


                lstData.ForEach(item =>
                {

                    Pages.Append("<tr>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + i + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Date + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Bill + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Invoice + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Dr + "</td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Cr + " </td>");
                    Pages.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000;'>" + item.Receipt + "</td>");
                    Pages.Append("</tr>");
                    i++;

                });



                // Pages.Append("</tr>");
                Pages.Append("</tbody></table>");

                Pages.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
                Pages.Replace("ISO_IMG", Server.MapPath("~/Content/Images/iso_logo.jpg"));
                Pages.Replace("SWACHBHARAT", Server.MapPath("~/Content/Images/SwachhBharat-Logo.png"));



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
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4Landscape, 20f, 30f, 20f, 30f, false, true))
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


    }
}