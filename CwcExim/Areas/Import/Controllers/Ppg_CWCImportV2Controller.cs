using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using CwcExim.Filters;
using CwcExim.Models;
using EinvoiceLibrary;

namespace CwcExim.Areas.Import.Controllers
{
    //For SRS Version 3.2 
    public class Ppg_CWCImportV2Controller : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Train Summary Upload TKD

        [HttpGet]
        public ActionResult TrainSummaryUploadTKD()
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.TrainSummryPayeeList(0, "");
            ViewBag.ShippingLine = null;
            ViewBag.State = false;
            if (objImport.DBResponse.Data != null)
            {
                dynamic obj = objImport.DBResponse.Data;
                ViewBag.ShippingLine = Newtonsoft.Json.JsonConvert.SerializeObject(obj.lstShiping);
                ViewBag.State = obj.State;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            Ppg_ImportRepositoryV2 ObjRR = new Ppg_ImportRepositoryV2();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CheckUpload(string TrainNo, string TrainDate)
        {
            int status = 0;
            List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();
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
                                            if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Container_No"])))
                                            {
                                                Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                                                objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                                                objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                                                objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"].ToString()[0] == '2' ? "20" : (dr["CT_Size"].ToString()[0] == '4' ? "40" : ""));
                                                objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                                                objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                                                objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                                                objTrainSummaryUpload.Foreign_Liner = Convert.ToString(dr["Foreign_Liner"]);
                                                objTrainSummaryUpload.Vessel_Name = Convert.ToString(dr["Vessel_Name"]);
                                                objTrainSummaryUpload.Vessel_No = Convert.ToString(dr["Vessel_No"]);
                                                objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                                                objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                                                objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                                                string FCLLCL = "";
                                                if (dr["Ct_Status"].ToString().Length > 0)
                                                {
                                                    if (dr["Ct_Status"].ToString()[0] == 'F')
                                                        FCLLCL = "FCL";
                                                    else if (dr["Ct_Status"].ToString()[0] == 'L')
                                                        FCLLCL = "LCL";
                                                }
                                                objTrainSummaryUpload.Ct_Status = FCLLCL;
                                                objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                                                objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                                                objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                                                objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

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

                                Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
                                objImport.CheckTrainSummaryUploadTKD(TrainNo, TrainSummaryUploadXML, TrainDate);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = ((List<Ppg_TrainSummaryUploadV2>)objImport.DBResponse.Data).Where(x => x.Status == 0).ToList();
                                }
                                else
                                {
                                    status = -6;

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message + " :\r\n" + ex.StackTrace);
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
        public ActionResult SaveUploadData(Ppg_TrainSummaryUploadV2 objTrainSummaryUpload,string SEZ)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.AddUpdateTrainSummaryUploadTKD(objTrainSummaryUpload,SEZ);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public ActionResult ListOfTrainSummaryTKD()
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<Ppg_TrainSummaryUploadV2> lstCargoSeize = new List<Ppg_TrainSummaryUploadV2>();
            objER.ListOfTrainSummaryTKD();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<Ppg_TrainSummaryUploadV2>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetailsTKD(int TrainSummaryUploadId)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetTrainSummaryDetailsTKD(TrainSummaryUploadId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TrainSummryPayeeList(int Page, string PartyCode)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.TrainSummryPayeeList(Page, PartyCode);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PrintTrainSummary(string TrainNo,int TrainSummaryUploadId)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.BulkInvoiceDetailsofTrainForPrint(TrainNo, TrainSummaryUploadId);
            DataSet ds = new DataSet();
            string Path = ""; int Status = 0;
            if (objImport.DBResponse.Data != null)
            {
                Status = 1;
                ds = (DataSet)objImport.DBResponse.Data;
                Path = GeneratingBulkPDFforTrainSummary(ds, "IMPORT TRAIN SUMMARY", "");
            }
            return Json(new { Status = Status, Message = Path });
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
        public string GeneratingBulkPDFforTrainSummary(DataSet ds, string InvoiceModuleName, string All)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            //If any changes required then also do the chages in Ppg_ReportCWCController with same method 
            var FileName = "";
            var location = "";
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            // List<dynamic> lstHeader = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[7]);
            List<string> lstSB = new List<string>();
            decimal version = 10;
            string logo = "";


            objCompany.ToList().ForEach(comobj =>
            {
                version = comobj.Version;
                logo= comobj.Effectlogofile;

            });
                lstInvoice.ToList().ForEach(item =>
            {
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
                html.Append("<td width='90%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>IRR INVOICE</label></td>");
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
                /*
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
                }*/

                html.Append("<tr><th style='text-align: left; font-size: 13px;margin-top: 10px;'><b>Container / CBT Details :</b> </th></tr>");
                html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
                html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Train No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Train Dt</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Gross Wt.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Port</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Via</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                html.Append("</tr></thead><tbody>");
                /*************/
                /*Container Bind*/
                int i = 1;
                lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                {
                    html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.TrainNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.TrainDate + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.GrossWt.ToString("0.####") + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.PortName + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Via + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                    html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
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
                rp.Version = version;
                rp.Effectlogofile = logo;


                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }
        #endregion

        #region Train Summary Upload LONI

        [HttpGet]
        public ActionResult TrainSummaryUploadLONI()
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.TrainSummryPayeeListLONI(0, "");
            ViewBag.ShippingLine = null;
            ViewBag.State = false;
            if (objImport.DBResponse.Data != null)
            {
                dynamic obj = objImport.DBResponse.Data;
                ViewBag.ShippingLine = Newtonsoft.Json.JsonConvert.SerializeObject(obj.lstShiping);
                ViewBag.State = obj.State;
            }
            return PartialView();
        }

        [HttpPost]
        public JsonResult CheckUploadLONI(string TrainNo, string TrainDate)
        {
            int status = 0;
            List<Ppg_TrainSummaryUploadV2> TrainSummaryUploadList = new List<Ppg_TrainSummaryUploadV2>();
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
                                    if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Container_No"])))
                                    {
                                        Ppg_TrainSummaryUploadV2 objTrainSummaryUpload = new Ppg_TrainSummaryUploadV2();
                                        objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"].ToString()[0] == '2' ? "20" : (dr["CT_Size"].ToString()[0] == '4' ? "40" : ""));
                                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                                        objTrainSummaryUpload.Foreign_Liner = Convert.ToString(dr["Foreign_Liner"]);
                                        objTrainSummaryUpload.Vessel_Name = Convert.ToString(dr["Vessel_Name"]);
                                        objTrainSummaryUpload.Vessel_No = Convert.ToString(dr["Vessel_No"]);
                                        objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                                        objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                                        objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                                        string FCLLCL = "";
                                        if (dr["Ct_Status"].ToString().Length > 0)
                                        {
                                            if (dr["Ct_Status"].ToString()[0] == 'F')
                                                FCLLCL = "FCL";
                                            else if (dr["Ct_Status"].ToString()[0] == 'L')
                                                FCLLCL = "LCL";
                                        }
                                        objTrainSummaryUpload.Ct_Status = FCLLCL;
                                        objTrainSummaryUpload.Destination = Convert.ToString(dr["Destination"]);
                                        objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["Smtp_No"]);
                                        objTrainSummaryUpload.Received_Date = Convert.ToString(dr["Received_Date"]);
                                        objTrainSummaryUpload.Genhaz = Convert.ToString(dr["Genhaz"]);

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

                                Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
                                objImport.CheckTrainSummaryUploadLONI(TrainNo, TrainSummaryUploadXML, TrainDate);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = ((List<Ppg_TrainSummaryUploadV2>)objImport.DBResponse.Data).Where(x => x.Status == 0).ToList();
                                }
                                else
                                {
                                    status = -6;

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message + " :\r\n" + ex.StackTrace);
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
        public ActionResult SaveUploadDataLONI(Ppg_TrainSummaryUploadV2 objTrainSummaryUpload,string SEZ)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.AddUpdateTrainSummaryUploadLONI(objTrainSummaryUpload,SEZ);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public ActionResult ListOfTrainSummaryLONI()
        {
            Ppg_ImportRepositoryV2 objER = new Ppg_ImportRepositoryV2();
            List<Ppg_TrainSummaryUploadV2> lstCargoSeize = new List<Ppg_TrainSummaryUploadV2>();
            objER.ListOfTrainSummaryLONI();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<Ppg_TrainSummaryUploadV2>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetailsLONI(int TrainSummaryUploadId)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetTrainSummaryDetailsLONI(TrainSummaryUploadId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TrainSummryPayeeListLONI(int Page, string PartyCode)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.TrainSummryPayeeListLONI(Page, PartyCode);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region SealCutting

        [HttpGet]
        public ActionResult AddSealCuting()
        {
            string check = "";
            SealCuttingV2 SC = new SealCuttingV2();
            SC.TransactionDate = DateTime.Now.ToString("dd/MM/yyyy");

            //Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            //objImport.ListOfGodown();
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.GodownList = objImport.DBResponse.Data; // JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.GodownList = null;
            //}
            //objImport.ListOfBL();
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.BLList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.BLList = null;
            //}

            //objImport.ListOfContainer();
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            //objImport.ListOfCHAShippingLine();
            //if (objImport.DBResponse.Data != null)
            //{
            //    check = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //    ViewBag.CHAShippingLineList = check;
            //}
            ////Newtonsoft.Json.JsonConvert.SerializeObject( objImport.DBResponse.Data);
            //// var a= Json.
            //else
            //{
            //    ViewBag.CHAShippingLineList = null;
            //}
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView(SC);
        }
        [HttpGet]
        public ActionResult GetListOfSealCuttingDetails(string Module, string InvoiceNo = null, string multi = null, string InvoiceDate = null, int Page = 0)
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetListOfSealCuttingDetails(Module, InvoiceNo, multi, InvoiceDate,0);
            IList<SealCuttingV2> lstSC = new List<SealCuttingV2>();
            if (objIR.DBResponse.Data != null)
                lstSC = (List<SealCuttingV2>)objIR.DBResponse.Data;
            return PartialView("ListOfSealCuttingDetails", lstSC);

        }
        [HttpGet]
        public JsonResult LoadListMoreDataForSealCutting(string Module, string InvoiceNo = null, string multi = null, string InvoiceDate = null, int Page=0)
        {
            Ppg_ImportRepositoryV2 ObjCR = new Ppg_ImportRepositoryV2();
            List<SealCuttingV2> LstJO = new List<SealCuttingV2>();
            ObjCR.GetListOfSealCuttingDetails(Module, InvoiceNo, multi, InvoiceDate, Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<SealCuttingV2>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSealCuttingById(int SealCuttingId)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetSealCuttingById(SealCuttingId);
            SealCuttingV2 objImp = new SealCuttingV2();
            if (objImport.DBResponse.Data != null)
                objImp = (SealCuttingV2)objImport.DBResponse.Data;
            return PartialView("EditSealCutting", objImp);
        }
        [HttpGet]
        public ActionResult ViewSealCuttingDetailById(int SealCuttingId)
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetSealCuttingById(SealCuttingId);
            SealCuttingV2 objImp = new SealCuttingV2();
            if (objIR.DBResponse.Data != null)
                objImp = (SealCuttingV2)objIR.DBResponse.Data;
            return PartialView("ViewSealCuttingDetailById", objImp);
        }

        [HttpGet]
        public JsonResult GetInvoiceDtlForSealCutting(String TransactionDate, String GateInDate, String ContainerNo, String size, int CHAShippingLineId, String CFSCode, int OBLType, int CargoType,string SEZ)
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.GetInvoiceDtlForSealCutting(TransactionDate, GateInDate, ContainerNo, size, CHAShippingLineId, CFSCode, OBLType, CargoType,SEZ);
            SealCuttingV2 objImp = new SealCuttingV2();
            if (objIR.DBResponse.Data != null)
                objImp = (SealCuttingV2)objIR.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfContainer()
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.ListOfContainer();
            List<SealCuttingV2> objImp = new List<SealCuttingV2>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<SealCuttingV2>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfBLData(int OBLId, int impobldtlId, string OBLFCLLCL)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.ListOfBL(OBLId, impobldtlId, OBLFCLLCL);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.OBLList = objImport.DBResponse.Data;
            //else
            //{
            //    ViewBag.OBLList = null;
            //}

            List<SealCuttingV2> objImp = new List<SealCuttingV2>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<SealCuttingV2>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfGodownData()
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.ListOfGodownRights(((Login)(Session["LoginUser"])).Uid);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            List<GodownList> objImp = new List<GodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<GodownList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCHAShippingLineData(int Page,string PartyCode)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.ListOfCHAShippingLine(Page, PartyCode);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            //List<SealCuttingCHAV2> objImp = new List<SealCuttingCHAV2>();
            //if (objImport.DBResponse.Data != null)
             //   objImp = (List<SealCuttingCHAV2>)objImport.DBResponse.Data;
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SealCuttingInvoicePrint(int InvoiceId)
        {
            Ppg_ImportRepositoryV2 objGPR = new Ppg_ImportRepositoryV2();
            objGPR.GetSCInvoiceDetailsForPrint(InvoiceId);
            PpgInvoiceSealCuttingV2 objSC = new PpgInvoiceSealCuttingV2();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objSC = (PpgInvoiceSealCuttingV2)objGPR.DBResponse.Data;
                FilePath = GeneratingPDF(objSC, InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        private string GeneratingPDF(PpgInvoiceSealCuttingV2 objSC, int InvoiceId)
        {
            string html = "";

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/SealCutting" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            html = "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>" +
                "<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objSC.CompanyName + "</h1>" +
                "<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />" +
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span><br />" +
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>Seal Cutting</span></td></tr>" +
                "<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>" +
                "CWC GST No. <label>" + objSC.CompanyGstNo + "</label></span></td></tr>" +
                "<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>" +
                "<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>" +
                "<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>" + objSC.InvoiceNo + "</span></td>" +
                "<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + objSC.InvoiceDate + "</span></td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>" +
                "<span>" + objSC.PartyName + "</span></td>" +
                "<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + objSC.PartyState + "</span> </td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>" +
                "Party Address :</label> <span>" + objSC.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>" +
                "<label style='font-weight: bold;'>State Code :</label> <span>" + objSC.PartyStateCode + "</span></td></tr>" +
                "<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objSC.PartyGstNo + "</span></td>" +
                "</tr></tbody> " +
                "</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>" +
                "<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:80%;' cellspacing='0' cellpadding='10'>" +
                "<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>" +
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>" +
                "<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody>";
            int i = 1;
            foreach (var container in objSC.LstContainersSealCutting)
            {
                html = html + "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CfsCode + "</td>" +
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
            foreach (var charge in objSC.LstChargesSealCutting)
            {
                html = html + "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeSD + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeDesc + "</td>" +
                    "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.HsnCode + "</td>" +
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
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objSC.TotalTax.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objSC.TotalCGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objSC.TotalSGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objSC.TotalIGST.ToString("0") + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objSC.TotalAmt.ToString("0") + "</td>" +
                "</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>" +
                "Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>" +
                "" + ConvertNumbertoWords(Convert.ToInt32(objSC.TotalAmt)) + "</td>" +
                "</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th>" +
                "<td style='font-size: 12px; text-align: left; padding: 5px;' colspan='6'>0</td>" +
                "</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>" +
                "<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: " +
                "<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:" +
                "<label style='font-weight: bold;'>" + objSC.PartyCode + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>" +
                "*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>" +
                "</td></tr></tbody></table>";
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/SealCutting" + InvoiceId.ToString() + ".pdf";
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSealCutting(SealCuttingV2 objSealCutting)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //if (ModelState.IsValid)
                //{

                string OBLXML = "";
                string ChargesBreakupXML = "";
                string ChargesXML = "";
                if (objSealCutting.ViewBLList != null)
                {
                    var ViewBLList = JsonConvert.DeserializeObject<List<SealCuttingV2>>(objSealCutting.ViewBLList.ToString());
                    if (ViewBLList != null)
                    {
                        OBLXML = Utility.CreateXML(ViewBLList);
                    }
                }
                if (objSealCutting.lstPostPaymentChrgBreakupAmt != null)
                {
                    var ViewBreakList = JsonConvert.DeserializeObject<List<ppgSealPostPaymentChargebreakupdateV2>>(objSealCutting.lstPostPaymentChrgBreakupAmt.ToString());

                    ChargesBreakupXML = Utility.CreateXML(ViewBreakList);
                }
                if (objSealCutting.lstPostPaymentChrgAmt != null)
                {
                    var ViewBreakList = JsonConvert.DeserializeObject<List<PostPaymentChargeV2>>(objSealCutting.lstPostPaymentChrgAmt.ToString());

                    ChargesXML = Utility.CreateXML(ViewBreakList.Where(x => x.Total > 0).ToList());
                }

                Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
                objImport.AddEditSealCutting(objSealCutting, ChargesBreakupXML, ChargesXML, BranchId, ((Login)(Session["LoginUser"])).Uid, OBLXML, Request.UserHostAddress);
                if (!string.IsNullOrEmpty((string)objImport.DBResponse.Data))
                    SendSealCuttingMail((string)objImport.DBResponse.Data, "SEAL CUTTING");
                //ModelState.Clear();
                return Json(objImport.DBResponse);
                //}
                //else
                //{
                //var Err = new { Status = -1, Message = "Error" };
                // return Json(Err);
                // }
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteSealCutting(int SealCuttingId)
        {
            Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
            objIR.DeleteSealCutting(SealCuttingId);

            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        public int SendSealCuttingMail(string InvoiceNo, string InvoiceModuleName)
        {
            try
            {
                Report.Models.BulkInvoiceReport ObjBulkInvoiceReport = new Report.Models.BulkInvoiceReport();
                ObjBulkInvoiceReport.InvoiceModule = "IMPSC";
                ObjBulkInvoiceReport.PeriodFrom = DateTime.Now.ToString("dd/MM/yyyy");
                ObjBulkInvoiceReport.PeriodTo = DateTime.Now.ToString("dd/MM/yyyy");
                ObjBulkInvoiceReport.InvoiceNumber = InvoiceNo;

                Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
                ObjRR.GenericBulkInvoiceDetailsForPrint(ObjBulkInvoiceReport);
                DataSet ds = (DataSet)ObjRR.DBResponse.Data;

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
                    Ppg_ImportRepository rep = new Ppg_ImportRepository();
                    PPGSealCuttingDateForReport objseal = new PPGSealCuttingDateForReport();
                    rep.GetSealCuttingDateId(item.StuffingReqNo);
                    if (rep.DBResponse.Data != null)
                    {
                        objseal = (PPGSealCuttingDateForReport)rep.DBResponse.Data;
                    }

                    StringBuilder html = new StringBuilder();
                    /*Header Part*/
                    html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");

                    html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                    html.Append("<td colspan='8' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objCompany[0].CompanyName + "</h1>");
                    html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
                    html.Append("<span style='font-size: 12px; padding-bottom: 10px;'>" + objCompany[0].CompanyAddress + "</span>");
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
                    html.Append("</tbody></table>");
                    html.Append("</td>");

                    html.Append("<td colspan='6' width='40%'>");
                    html.Append("<table style='width:100%; font-size:9pt;' cellspacing='0' cellpadding='5'><tbody>");
                    html.Append("<tr><th colspan='6' width='50%'>Invoice Date</th><th>:</th><td colspan='6' width='50%'>" + item.InvoiceDate + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>State</th><th>:</th><td colspan='6' width='50%'>" + item.PartyState + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>State Code</th><th>:</th><td colspan='6' width='50%'>" + item.PartyStateCode + "</td></tr>");
                    html.Append("<tr><th colspan='6' width='50%'>Invoice Generated By</th><th>:</th><td colspan='6' width='50%'>" + item.PaymentMode + "</td></tr>");
                    html.Append("</tbody></table>");
                    html.Append("</td>");
                    html.Append("</tr></tbody></table>");
                    html.Append("</td></tr>");

                    html.Append("<tr><td><hr/></td></tr>");
                    html.Append("<tr><th style='text-align: left; font-size: 13px;margin-top: 10px;'><b>Container / CBT Details :</b> </th></tr>");
                    html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:90%;' cellspacing='0' cellpadding='10'>");
                    html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container / CBT No.</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
                    html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
                    html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
                    html.Append("</tr></thead><tbody>");
                    /*************/
                    /*Container Bind*/
                    int i = 1;
                    lstContainer.Where(x => x.InoviceId == item.InvoiceId).ToList().ForEach(elem =>
                    {
                        html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.CFSCode + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.ContainerNo + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + elem.Size + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objseal.GateInDate + "</td>");
                        html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objseal.TranscationDate + "</td>");
                        html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (elem.CargoType == 0 ? "" : (elem.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
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
                FileName = InvoiceModuleName + DateTime.Now.ToOADate().ToString().Replace(".", string.Empty) + ".pdf";
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
                    rp.GeneratePDF(location, lstSB);
                }
                string EmailId = lstInvoice[0].Email.ToString();
                string[] mailTo = { };
                if (!string.IsNullOrEmpty(EmailId))
                {
                    mailTo = EmailId.Replace(" ", "").Split(',').ToArray();
                    mailTo = mailTo.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    string emailBody = "Dear Customer,<br/><br/>Kindly find the attached invoice that has been generated against you.If you have further queries regarding this invoice then drop an email at cwcdli.icdppg@gmail.com.";
                    emailBody += "<br/><br/>This a system generated email, so please do not reply to this email.";
                    emailBody += "<br/><br/>Thanks & Regards";
                    emailBody += "<br/><br/>ICD - Patparganj<br/>";
                    string status = UtilityClasses.CommunicationManager.SendMail("SEAL CUTTING INVOICE", emailBody, mailTo,
                            new[] { Server.MapPath("~/Docs/" + Session.SessionID) + "\\" + FileName } // new[] { FolderPath + "/" + FileName }
                            );
                    if (status == "Success")
                    {
                        Ppg_ImportRepositoryV2 objRR = new Ppg_ImportRepositoryV2();
                        objRR.SealCuttingEmailUpdate(InvoiceNo);
                    }
                    else
                    {
                        string FolderPath = Server.MapPath("~/Uploads/SealCuttingError/" + DateTime.Now.ToString("ddMMYYYY"));
                        if (!System.IO.Directory.Exists(FolderPath))
                        {
                            System.IO.Directory.CreateDirectory(FolderPath);
                        }
                        FileName = Path.Combine(FolderPath, InvoiceNo + "_ErrorEntryEmail.txt");
                        string MailIds = String.Join(",", mailTo.Select(p => p.ToString()).ToArray());

                        using (var tw = new StreamWriter(FileName, true))
                        {
                            tw.WriteLine("For Invoice No :" + InvoiceNo + " .Email not Sent To :" + MailIds + "\r\n Error:" + status);
                            tw.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }
        #endregion

        #region Internal Movement Application

        [HttpGet]
        public ActionResult CreateInternalMovementApp()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetBOENoForApp()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetBOENoForInternalMovementApp();
            return Json(ObjIR.DBResponse,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInternalMovementListApp()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetAllInternalMovementApp(0);
            List<PPG_Internal_MovementAppV2> LstMovement = new List<PPG_Internal_MovementAppV2>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_Internal_MovementAppV2>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementListApp", LstMovement);
        }
        [HttpGet]
        public JsonResult LoadMoreInternalMovementApp(int Page)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            List<PPG_Internal_MovementAppV2> LstInternalMovement = new List<PPG_Internal_MovementAppV2>();
            ObjIR.GetAllInternalMovementApp(Page);
            if(ObjIR.DBResponse.Data!=null)
            {
                LstInternalMovement = (List<PPG_Internal_MovementAppV2>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInternalMovementApp(int MovementId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            PPG_Internal_MovementAppV2 ObjInternalMovement = new PPG_Internal_MovementAppV2();
            ObjIR.GetInternalMovementApp(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_MovementAppV2)ObjIR.DBResponse.Data;
                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }


                //ObjIR.GetBOENoForInternalMovementApp();
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    ViewBag.BOENoList = new SelectList((List<PPG_Internal_MovementAppV2>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
                //}
                //else
                //{
                //    ViewBag.BOENoList = null;
                //}
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovementApp(int MovementId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            PPG_Internal_MovementAppV2 ObjInternalMovement = new PPG_Internal_MovementAppV2();
            ObjIR.GetInternalMovementApp(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_MovementAppV2)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetBOENoDetailsApp(int DestuffingEntryDtlId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetBOENoDetForMovementApp(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovementApp(PPG_Internal_MovementAppV2 objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                
                Ppg_ImportRepositoryV2 objChargeMaster = new Ppg_ImportRepositoryV2();
                objChargeMaster.AddEditInvoiceMovementApp(objForm, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPMovement");
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GodownWiseLocation(GodownId);
            object objLctn = null;
            if (objIR.DBResponse.Data != null)
                objLctn = objIR.DBResponse.Data;
            return Json(objLctn, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Internal movement approval
        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }
            
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetInternalMovementList()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetAllInternalMovement(0);
            List<PPG_Internal_MovementV2> LstMovement = new List<PPG_Internal_MovementV2>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_Internal_MovementV2>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }
        [HttpGet]
        public JsonResult LoadMoreInternalMovement(int Page)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            List<PPG_Internal_MovementV2> LstMovement = new List<PPG_Internal_MovementV2>();
            ObjIR.GetAllInternalMovement(Page);
            if(ObjIR.DBResponse.Data!=null)
            {
                LstMovement = (List<PPG_Internal_MovementV2>)ObjIR.DBResponse.Data;
            }
            return Json(LstMovement, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            PPG_Internal_MovementV2 ObjInternalMovement = new PPG_Internal_MovementV2();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_MovementV2)ObjIR.DBResponse.Data;
                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovement(int MovementId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            PPG_Internal_MovementV2 ObjInternalMovement = new PPG_Internal_MovementV2();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_MovementV2)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }
        [HttpGet]
        public JsonResult GetTransferNo()
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetBOENoForInternalMovement();
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTransferNoDetails(int TransferId)
        {
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetBOENoDetForMovement(TransferId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovement(PPG_Internal_MovementV2 ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
                ObjIR.AddEditImpInternalMovement(ObjInternalMovement,Convert.ToInt32(Session["BranchId"]),((Login)Session["LoginUser"]).Uid,"Import");
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }

        }
        #endregion
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



        #region OBL Amendment Split 

        public ActionResult OBLSplit()
        {
            OBLWiseContainerEntryV2 objOBLEntry = new OBLWiseContainerEntryV2();
            try
            {
                Ppg_ImportRepositoryV2 ObjER = new Ppg_ImportRepositoryV2(); 
                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
                ObjIR.ListOfShippingLinePartyCode("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
               
                ObjIR.ListOfImporterForPage("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstImporter = Jobject["lstImporter"];
                    ViewBag.ImpState = Jobject["State"];
                }
                else
                {
                    ViewBag.lstImporter = null;
                }
              
                List<Areas.Export.Models.Commodity> objImp = new List<Areas.Export.Models.Commodity>();

                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }

                
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }


        public JsonResult GetOBLListForSpilt()
        {
            try
            {
                Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();             
                obj.GetOBLListForSpilt();                            
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOBLDetailsForSpilt(string OBL,string OBLDate,int isFCL)
        {
            try
            {
                Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
                obj.GetOBLDetailsForSpilt(OBL,OBLDate,isFCL);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }


       
        [HttpPost]
        public JsonResult AddEditOBLSpilt(Ppg_OBLSpilt vm,List<Ppg_OBLSpiltDetails> lstVM,int IsFCL,string SpiltDate)
        {
            try
            {
                List<Ppg_OBLSpiltDetails> idata = new List<Ppg_OBLSpiltDetails>();


                var totalPkg = vm.NoOfPkg;
                var totalWT = vm.GRWT;
                var spiltpkg = lstVM.Sum(x => x.SpiltPkg);
                var spiltwt = lstVM.Sum(x => x.SpiltWT);
                if (totalPkg == spiltpkg && totalWT == spiltwt)
                {



                    foreach (var i in lstVM)
                    {
                        Ppg_OBLSpiltDetails data = new Ppg_OBLSpiltDetails();
                        if (i.SpiltIGMDate != null)
                        {
                            DateTime dtSpiltDate = DateTime.ParseExact(i.SpiltIGMDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            data.SpiltIGMDate = dtSpiltDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            data.SpiltIGMDate = "##";
                        }
                        if (i.SpiltOBLDate != null)
                        {
                            DateTime dtSpiltDate = DateTime.ParseExact(i.SpiltOBLDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            data.SpiltOBLDate = dtSpiltDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            data.SpiltOBLDate = i.SpiltOBLDate;
                        }
                        if (i.SpiltSMPTDate != null)
                        {
                            DateTime dtSpiltDate = DateTime.ParseExact(i.SpiltSMPTDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            data.SpiltSMPTDate = dtSpiltDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            data.SpiltSMPTDate = "##";
                        }
                        data.SpiltWT = i.SpiltWT;
                        data.SpiltPkg = i.SpiltPkg;
                        data.SpiltSMPTNo = i.SpiltSMPTNo;
                        data.SpiltOBL = i.SpiltOBL;
                        data.SpiltLineNo = i.SpiltLineNo;
                        data.SpiltImporterID = i.SpiltImporterID;
                        data.SpiltImporter = i.SpiltImporter;
                        data.SpiltIGMNo = i.SpiltIGMNo;
                        data.SpiltCommodityName = i.SpiltCommodityName;
                        data.SpiltCommodityId = i.SpiltCommodityId;
                        data.SpiltCargoDesc = i.SpiltCargoDesc;

                        idata.Add(data);
                    }

                    string OBLXML = "";
                    OBLXML = Utility.CreateXML(idata);

                    OBLXML = OBLXML.Replace(">##<", ">null<");
                    Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
                    obj.AddEditOBLSpilt(vm, OBLXML, ((Login)(Session["LoginUser"])).Uid, SpiltDate, IsFCL);
                    return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DAL.DatabaseResponse msg = new DAL.DatabaseResponse();
                    msg.Status = 0;
                    msg.Message = "OBL pkg and wt should be equal Split pkg and wt";
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
            }
            catch(Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult ListOBLSpilt(string OBLNo)
        {
            List<Ppg_OBLSpilt> lstOblDetails = new List<Ppg_OBLSpilt>();
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            if (String.IsNullOrEmpty(OBLNo)==true)
            {
                OBLNo = null;
            }
            obj.GetSpiltOBLDetails(OBLNo);
            if(obj.DBResponse.Data!=null)
            {
                lstOblDetails = (List<Ppg_OBLSpilt>)obj.DBResponse.Data;
            }
            return PartialView("ListOBLSpilt", lstOblDetails);
        }


       

        public ActionResult ViewSpiltDetails(int id)
        {
            Ppg_OBLSpilt lstOblDetails = new Ppg_OBLSpilt();
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.GetViewSpiltOBLDetails(id);
            if (obj.DBResponse.Data != null)
            {
                lstOblDetails = (Ppg_OBLSpilt)obj.DBResponse.Data;
            }
            
            return PartialView("ViewSpiltDetails", lstOblDetails);
        }

        public JsonResult DeleteSplitdetails(int id)
        {
           
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.DeleteSplitdetails(id);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region BEO Query
        public ActionResult BEODetailsFromICEGate()
        {
            try
            {
                Ppg_ImportRepositoryV2 objIR = new Ppg_ImportRepositoryV2();
                objIR.ListOfICEGateBEONo("", 0);
                if (objIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstObl = Jobject["LstObl"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.LstObl = null;
                    ViewBag.State = null;
                }
            }
            catch (Exception ex)
            {
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult SearchICEGateBEONo(string BEONo)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfICEGateBEONo(BEONo, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadICEGateBEONo(string BEONo, int Page)
        {
            Ppg_ImportRepositoryV2 objRepo = new Ppg_ImportRepositoryV2();
            objRepo.ListOfICEGateBEONo(BEONo, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBOEICEGateDetail(string BEONo, string BEODate)
        {
            Ppg_ImportRepositoryV2 objImport = new Ppg_ImportRepositoryV2();
            objImport.GetBEODetails(BEONo, BEODate);
            BEODetails beoDetails = new BEODetails();
            if (objImport.DBResponse.Data != null)
                beoDetails =(BEODetails)objImport.DBResponse.Data;
            return Json(beoDetails, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IGM Data Populate by Container
        [HttpGet]
        public ActionResult IGMData() 
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetContainerList(string Year)
        {
            Ppg_ImportRepositoryV2 ppgRepo = new Ppg_ImportRepositoryV2();
            ppgRepo.GetContainerList(Year);
            return Json(ppgRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYearList()
        {
            Ppg_ImportRepositoryV2 ppgRepo = new Ppg_ImportRepositoryV2();
            ppgRepo.GetYearList();
            return Json(ppgRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerCargoInfo(string Year,string ContainerNo)
        {
            Ppg_ImportRepositoryV2 ppgRepo = new Ppg_ImportRepositoryV2();
            ppgRepo.GetContainerCargoInfo(Year,ContainerNo);
            return Json(ppgRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Know Your Storage Charge
        public ActionResult KnowYourStorageCharge()
        {
            return PartialView();
        }
        public JsonResult GetOBLListForKnowStorageCharge()
        {
            List<Ppg_ApproveDeliveryOrder> lstOBLStatus = new List<Ppg_ApproveDeliveryOrder>();
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.GetOBLListForKnowStorageCharge(((Login)(Session["LoginUser"])).EximTraderId);
            if (obj.DBResponse.Data != null)
            {
                lstOBLStatus = (List<Ppg_ApproveDeliveryOrder>)obj.DBResponse.Data;
            }
            return Json(lstOBLStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSBListForKnowStorageCharge()
        {
            List<Ppg_ApproveDeliveryOrder> lstOBLStatus = new List<Ppg_ApproveDeliveryOrder>();
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.GetSBListForKnowStorageCharge(((Login)(Session["LoginUser"])).EximTraderId);
            if (obj.DBResponse.Data != null)
            {
                lstOBLStatus = (List<Ppg_ApproveDeliveryOrder>)obj.DBResponse.Data;
            }
            return Json(lstOBLStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStorageCharge(string OBL, string SBNo, string Date, int RefId)
        {
            Ppg_ImportRepositoryV2 obj = new Ppg_ImportRepositoryV2();
            obj.GetKnowStorageCharge(OBL, SBNo, Date, RefId);
            decimal StorageCharge = 0M;
            if (obj.DBResponse.Data != null)
            {
                StorageCharge = Convert.ToDecimal(obj.DBResponse.Data);
            }
            return Json(StorageCharge, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Self Assessment For External User
        public ActionResult SelfAssessment()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDestuffingNoForSelfAssessment()
        {

            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNoForSelfAssessment(((Login)(Session["LoginUser"])).EximTraderId);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo, OBLDate = item.OBLDate, ContainerNo = item.ContainerNo });
                });

            }

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public JsonResult GetOBLStatus(int DestuffingId)
        {
            List<Ppg_OBLStatusDetails> lstOBLStatus = new List<Ppg_OBLStatusDetails>();
            Ppg_ImportRepositoryV2 impobj = new Ppg_ImportRepositoryV2();
            impobj.GetOBLStatus(DestuffingId);
            if (impobj.DBResponse.Data != null)
            {
                lstOBLStatus = (List<Ppg_OBLStatusDetails>)impobj.DBResponse.Data;
            }
            return Json(lstOBLStatus, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Approve Delivery Order For External User

        public ActionResult ApproveDeliveryOrder()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDestuffingNoForApproveDeliveryOrder()
        {

            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNoForDeliveryOrder(((Login)(Session["LoginUser"])).EximTraderId);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo ,OBLDate = item.OBLDate, ContainerNo = item.ContainerNo });
                });

            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomValidateAntiForgeryToken]
        public JsonResult GenerateOTP()
        {

            Login objlogin = new Login();

            objlogin = (Login)(Session["LoginUser"]);
            Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
            ObjIR.GetMobileNoForDeliveryOrder(objlogin.Uid);
            if (ObjIR.DBResponse.Data.ToString() != "")
            {
                string MobileNo = ObjIR.DBResponse.Data.ToString();
                Random r = new Random();
                int MobileCode = r.Next(1000, 10000); //for ints
                                                      //int range = 100;
                                                      //double rDouble = r.NextDouble() * range;          

                Session["GeneratedMobileCode"] = MobileCode;
                SmsDataModel ObjSmsDataModel = new SmsDataModel();
                ObjSmsDataModel.MsgRecepient = MobileNo.Trim();
                ObjSmsDataModel.MsgText = "Greetings, your one time password for phone verification is " + Convert.ToString(MobileCode) + " Please note this verification is for mobile number verification purpose in CWC-CFS.COM Portal CWC";               
                UtilityClasses.CommunicationManager.SendSMS(ObjSmsDataModel);
                return Json(new { Status = 1, Msg = "OTP send your register mobile no. " }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = 0, Msg = "Invalid Mobile No" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult VarifyOTPDeliveryApproved(Ppg_ApproveDeliveryOrder vm)
        {
            if (Session["GeneratedMobileCode"].ToString() == Convert.ToString(vm.MobileGenerateCode))
            {

                vm.Uid = ((Login)(Session["LoginUser"])).Uid;
                Ppg_ImportRepositoryV2 ObjIR = new Ppg_ImportRepositoryV2();
                ObjIR.AddeditApprovedDeliveryOrder(vm);
                return Json(new { Status = ObjIR.DBResponse.Status, Msg = ObjIR.DBResponse.Message }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { Status = 0, Msg = "Invalid OTP" }, JsonRequestBehavior.AllowGet);
            }
            //return Json("");
        }

        #endregion

        
    }
}
