using CwcExim.Areas.Import.Models;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using CwcExim.Filters;

using DynamicExcel;
using System.Text;
using CwcExim.Areas.Report.Models;
using System.Threading.Tasks;
using EinvoiceLibrary;

namespace CwcExim.Areas.Import.Controllers
{

    public partial class DSR_CWCImportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DSRInvoiceGodown objreport;

        #region Train Summary Upload

        [HttpGet]
        public ActionResult TrainSummaryUpload()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            DSR_ExportRepository ObjRR = new DSR_ExportRepository();
            ObjRR.GetPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            //PortRepository objImport = new PortRepository();
            //objImport.GetAllPort();
            //if (objImport.DBResponse.Status > 0)
            //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetForeignPortList()
        {
            DSR_ImportRepository ObjRR = new DSR_ImportRepository();
            ObjRR.GetForeignPortOfLoading();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            //PortRepository objImport = new PortRepository();
            //objImport.GetAllPort();
            //if (objImport.DBResponse.Status > 0)
            //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckUpload(string TrainNo)
        {
            int status = 0;
            List<DSRTrainSummaryUpload> TrainSummaryUploadList = new List<DSRTrainSummaryUpload>();
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
                                    if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Container No#"])))
                                    {
                                        if (TrainSummaryUploadList != null)
                                        {
                                            for (int i = 0; i < TrainSummaryUploadList.Count; i++)
                                            {
                                                if (Convert.ToString(dr["Container No#"]) == TrainSummaryUploadList[i].Container_No)
                                                {
                                                    //TrainSummaryUploadList = null;
                                                    status = -6;
                                                    return Json(new { Status = status, Data = TrainSummaryUploadList[i].Container_No }, JsonRequestBehavior.AllowGet);
                                                }

                                            }
                                        }

                                        DSRTrainSummaryUpload objTrainSummaryUpload = new DSRTrainSummaryUpload();
                                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container No#"]);
                                        objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon No#"]);
                                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["Shipping Line"]);
                                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["Size"]);
                                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Seal No#"]);
                                        objTrainSummaryUpload.Ct_Type = Convert.ToString(dr["Type"]);
                                        objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Status"]);
                                        objTrainSummaryUpload.Destination = Convert.ToString(dr["Port of Destination"]);
                                        objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Weight"]);
                                        objTrainSummaryUpload.Smtp_No = Convert.ToString(dr["SMTP No#"]);
                                        objTrainSummaryUpload.Loading_Date = Convert.ToString(dr["Date"]);
                                        objTrainSummaryUpload.Port_of_Loading = Convert.ToString(dr["Port of Loading"]);
                                        objTrainSummaryUpload.Seal_Intact = Convert.ToString(dr["Seal Intact"]);
                                        objTrainSummaryUpload.Cargo_Desc = Convert.ToString(dr["Cargo Description"]);
                                        objTrainSummaryUpload.POD_Code = Convert.ToString(dr["POD Code"]);

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

                                DSR_ImportRepository objImport = new DSR_ImportRepository();
                                objImport.CheckTrainSummaryUpload(TrainNo, TrainSummaryUploadXML);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = (List<DSRTrainSummaryUpload>)objImport.DBResponse.Data;
                                }
                                else
                                {
                                    status = -6;

                                }
                            }
                            catch (Exception ex)
                            {
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
        public ActionResult SaveUploadData(DSRTrainSummaryUpload objTrainSummaryUpload)
        {
            int data = 0;
            if (objTrainSummaryUpload.TrainSummaryList != null)
                objTrainSummaryUpload.TrainSummaryUploadList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSRTrainSummaryUpload>>(objTrainSummaryUpload.TrainSummaryList);
            if (objTrainSummaryUpload.TrainSummaryUploadList.Count > 0)
            {
                string TrainSummaryUploadXML = Utility.CreateXML(objTrainSummaryUpload.TrainSummaryUploadList);
                DSR_ImportRepository objImport = new DSR_ImportRepository();
                objImport.AddUpdateTrainSummaryUpload(objTrainSummaryUpload, TrainSummaryUploadXML);
                if (objImport.DBResponse.Status > 0)
                {
                    data = Convert.ToInt32(objImport.DBResponse.Data);
                }
                else
                {
                    data = 0;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfTrainSummary()
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSRTrainSummaryUpload> lstCargoSeize = new List<DSRTrainSummaryUpload>();
            objER.ListOfTrainSummary();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<DSRTrainSummaryUpload>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetTrainSummaryDetails(TrainSummaryUploadId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region OBL Wise Custom Appraisement
        [HttpGet]
        public ActionResult OBLWiseCustomAppraisement(int CustomAppraisementId = 0)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSRCustomAppraisement ObjAppraisement = new DSRCustomAppraisement();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjAppraisement.AppraisementDateCheck = DateTime.Now.ToString("MM/dd/yyyy");

            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            /*ObjIR.ListOfImporterForm();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
            }
            else
            {
                ViewBag.lstImporter = null;
            }*/

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

            if (CustomAppraisementId > 0)
            {
                ObjIR.GetOBLCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (DSRCustomAppraisement)ObjIR.DBResponse.Data;
                    ViewBag.AppraisementDtl = (DSRCustomAppraisement)ObjIR.DBResponse.Data;
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.AppOrderDetails = Jobject["LstCustomAppraisementOrdDtl"];
                }
            }

            return PartialView("OBLWiseCustomAppraisement", ObjAppraisement);


        }

        [HttpGet]
        public JsonResult GetOBLWiseContainer(string OBLNo, string OBLDate)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetOBLWiseContainer(OBLNo, OBLDate);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditOBLCustomAppraisement(DSRCustomAppraisement ObjAppraisement, string ContAppraisement, string CAOrdDtl)
        {
            //ModelState.Remove("NoOfPackages");
            //ModelState.Remove("GrossWeight");
            // ModelState.Remove("RMSValue");
            //ModelState.Remove("ManualWT");
            //ModelState.Remove("MechanicalWT");

            //if (ModelState.IsValid)
            //{
            string AppraisementXML = "";
            string CAOrdXML = "";
            if (ContAppraisement != null)
            {
                List<DSRCustomAppraisementDtl> LstAppraisement = JsonConvert.DeserializeObject<List<DSRCustomAppraisementDtl>>(ContAppraisement);
                AppraisementXML = Utility.CreateXML(LstAppraisement);
            }
            if (CAOrdDtl != "")
            {
                List<DSRCustomAppraisementOrdDtl> LstCAOrd = JsonConvert.DeserializeObject<List<DSRCustomAppraisementOrdDtl>>(CAOrdDtl);
                CAOrdXML = Utility.CreateXML(LstCAOrd);
            }

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
            ObjIR.AddEditOBLCustomAppraisement(ObjAppraisement, AppraisementXML, CAOrdXML);
            ModelState.Clear();
            return Json(ObjIR.DBResponse);
            //}
            //else
            //{
            //    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
            //    var Err = new { Status = 0, Message = ErrorMessage };
            //    return Json(Err);
            //}
        }


        [HttpGet]
        public ActionResult GetOBLWiseCustomAppraisementList(int Page = 0)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.MenuRights = Jobjectt["lstMenu"];
            }


            List<DSRCustomAppraisement> LstAppraisement = new List<DSRCustomAppraisement>();
            ObjIR.LoadMoreCustomAppraisementApp(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<DSRCustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("OBLWiseCustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }
        [HttpGet]
        public JsonResult LoadMoreCustomAppraisementList(int Page = 0)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSRCustomAppraisement> LstAppraisement = new List<DSRCustomAppraisement>();
            ObjIR.LoadMoreCustomAppraisementApp(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<DSRCustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return Json(LstAppraisement,JsonRequestBehavior.AllowGet);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }
        [HttpGet]
        public JsonResult GetContainerDetailsOnEditMode(int CustomAppraisementId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetOBLCustomAppraisement(CustomAppraisementId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewOBLWiseCustomAppraisement(int CustomAppraisementId = 0)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSRCustomAppraisement ObjAppraisement = new DSRCustomAppraisement();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
            }
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjAppraisement.AppraisementDateCheck = DateTime.Now.ToString("MM/dd/yyyy");

            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            ObjIR.ListOfImporterForm();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
            }
            else
            {
                ViewBag.lstImporter = null;
            }

            if (CustomAppraisementId > 0)
            {
                ObjIR.GetOBLCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (DSRCustomAppraisement)ObjIR.DBResponse.Data;
                    ViewBag.AppraisementDtl = (DSRCustomAppraisement)ObjIR.DBResponse.Data;
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.AppOrderDetails = Jobject["LstCustomAppraisementOrdDtl"];
                }
            }

            return PartialView("ViewOBLWiseCustomAppraisement", ObjAppraisement);


        }

        [HttpGet]
        public ActionResult GetContainerBOLCustomApp(String CFSCode)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetContainerBOLCustomApp(CFSCode);
            List<DSRCustomAppraisementOBLCont> obj = (List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchOBLWiseCustomAppraisementList(string AppraisementNo)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            List<DSRCustomAppraisement> LstAppraisement = new List<DSRCustomAppraisement>();
            objIR.SearchOBLWiseCustomAppraisementList(AppraisementNo);
            if (objIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<DSRCustomAppraisement>)objIR.DBResponse.Data;
            }
            return PartialView("OBLWiseCustomAppraisementList", LstAppraisement);
        }

        [HttpGet]
        public ActionResult SearchContainerWiseCustomAppraisementList(string AppraisementNo)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            List<DSRCustomAppraisement> LstAppraisement = new List<DSRCustomAppraisement>();
            objIR.SearchOBLWiseCustomAppraisementList(AppraisementNo);
            if (objIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<DSRCustomAppraisement>)objIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintCustomAppraisement(int CustomAppraisementId)
        {
            DSR_ImportRepository objIr = new DSR_ImportRepository();
            objIr.PrintCustomAppraisement(CustomAppraisementId);
            string Path = "";
            if (objIr.DBResponse.Data != null)
            {
                DataSet objDS = (DataSet)objIr.DBResponse.Data;
                Path = GeneratedCustomAppraisementPrint(objDS, CustomAppraisementId);
                return Json(new { Status = 1, Message = "Done", data = Path });
            }
            return Json(new { Status = 0, Message = "Error", data = Path });
        }
        [NonAction]
        public string GeneratedCustomAppraisementPrint(DataSet objDS, int CustomAppraisementId)
        {
            StringBuilder objSB = new StringBuilder();
            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: right;' colspan='12'>");
            objSB.Append("<h1 style='font-size: 12px; line-height: 20px; font-weight: 300;margin: 0; padding: 0;'>");
            objSB.Append("</h1></td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>ICD DASHRATH-VADODARA</label><br/><label style='font-size: 16px; font-weight:bold;'>SEAL CUTTING SHEET</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");            
            
            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table style='width:100%; margin: 0;margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='9' width='70%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Name of Importer:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("IMP") + "</span></td>");
            objSB.Append("<td colspan='3' width='30%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Entry No. : </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("AppraisementNo") + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr>");
            objSB.Append("<td colspan='9' width='70%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Name of CHA:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("CHA") + "</span></td>");
            objSB.Append("<td colspan='3' width='30%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Entry Date : </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("AppraisementDate") + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr>");
            objSB.Append("<td colspan='9' width='70%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Name of Shipping:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("SLA") + "</span></td>");
            objSB.Append("<td colspan='3' width='30%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>CIF Value : </label> <span>" + objDS.Tables[0].Rows[0].Field<decimal>("Fob") + "</span></td>");
            objSB.Append("</tr>");


            objSB.Append("<tr>");
            objSB.Append("<td colspan='9' width='70%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Cargo Description:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("CargoDescription") + "</span></td>");
            objSB.Append("<td colspan='3' width='30%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Duty Value : </label> <span>" + objDS.Tables[0].Rows[0].Field<decimal>("GrossDuty") + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>OBL No:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("OBLNo") + "</span></td>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>OBL Type: </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("MoveType") + "</span></td>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Item No.: </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("ItemNo") + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("<tr>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>IGM No:</label> <span>" + objDS.Tables[0].Rows[0].Field<string>("IGMNo") + "</span></td>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>IGM Date: </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("IGMDate") + "</span></td>");
            objSB.Append("<td colspan='4' width='33%' style='font-size: 9pt; padding-bottom:15px;'><label style='font-weight: bold;'>Exam Mode: </label> <span>" + objDS.Tables[0].Rows[0].Field<string>("ExamMode") + "</span></td>");
            objSB.Append("</tr>");

            objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table style='border:1px solid #000; font-size:9pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%'>SR No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>CFS Code</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>Container No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>In Date</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>Size</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>Total pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%'>Exam Date</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Cust Seal No</th>");
            objSB.Append("</tr></thead>");
            objSB.Append("<tbody>");
            int Serial = 1;
            objDS.Tables[1].AsEnumerable().ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Serial + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("CFSCode") + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("ContainerNo") + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("Indate") + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("Size") + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<int>("NoOfPackages") + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("AppraisementDate") + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Field<string>("SealNo") + "</td>");
                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr>");
            
            objSB.Append("</tbody>");
            objSB.Append("</table></td></tr>");
            objSB.Append("<tr><td colspan='3' style='font-size: 12px; width: 25%; text-align: center;padding-top: 100px;'>H &amp; T / Surveyor</td>");
            objSB.Append("<td colspan='3' style='font-size: 12px; width: 25%; text-align: center;padding-top: 100px;'>Representative Importer / CHA</td>");
            objSB.Append("<td colspan='3' style='font-size: 12px; width: 25%; text-align: center;padding-top: 100px;'>Import Incharge</td>");
            objSB.Append("<td colspan='3' style='font-size: 12px; width: 25%; text-align: center;padding-top: 100px;'>Customs Officer</td></tr>");
            //objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Customs</td></tr>");
            objSB.Append("</tbody></table>");

            objSB = objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            var Path = Server.MapPath("~/Docs/" + Session.SessionID + "/CustomAppraisement" + CustomAppraisementId + ".pdf");
            if (!Directory.Exists(Server.MapPath("~/Docs/" + Session.SessionID)))
                Directory.CreateDirectory(Server.MapPath("~/Docs/" + Session.SessionID));
                        
            using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = "";
                rh.HOAddress = "";
                rh.ZonalOffice = "";
                rh.ZOAddress = "";
                rh.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/CustomAppraisement" + CustomAppraisementId + ".pdf";
        }
        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfImporterForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        // Issued by begin
        [HttpGet]
        public JsonResult SearchIssuedByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.LoadIssuedByList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadIssuedByList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.LoadIssuedByList(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        // Issued by end
        // Cargo Delivered To begin
        [HttpGet]
        public JsonResult SearchCargoDeliveredByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.LoadIssuedByList(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCargoDeliveredList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.LoadIssuedByList(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        // Cargo Delivered To end
        #endregion

        #region Custom Appraisement Application
        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSRCustomAppraisementNew ObjAppraisement = new DSRCustomAppraisementNew();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }
            ObjAppraisement.AppraisementDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjAppraisement.AppraisementDateCheck = DateTime.Now.ToString("MM/dd/yyyy");

            //ObjIR.GetContnrNoForCustomAppraise();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.ContainerList = new SelectList((List<DSRCustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
            //}
            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }

            /*ObjIR.ListOfImporterForm();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
            }
            else
            {
                ViewBag.lstImporter = null;
            }*/

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
            return PartialView("CreateCustomAppraisement", ObjAppraisement);


        }


        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfChaForPage(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfPayerForPage(string PartyCode,int Page=0)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfPayerForPage(PartyCode,Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ImporterSearchByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfNewImporterPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeForCustApr(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeListForCustApr(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetBOEDetail(String BOENo)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetBOEDetail(BOENo);
            DSRCustomAppraisementBOECont obj = (DSRCustomAppraisementBOECont)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOBLContainer(String OBlNo)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetOBLContainer(OBlNo);
            if (rep.DBResponse.Data != null)
            {
                List<DSRCustomAppraisementOBLCont> obj = (List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data;
                if (obj.Count > 0)
                {
                    ViewBag.ContainerList = new SelectList((List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                }
                else
                {
                    rep.GetContnrNoForCustomAppraise();
                    if (rep.DBResponse.Data != null)
                    {
                        ViewBag.ContainerList = new SelectList((List<DSRCustomAppraisementDtl>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                    }
                }
            }
            return Json(objOBLContainer, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetBOEDetail(String BOENo)
        //{
        //    DSR_ImportRepository rep = new DSR_ImportRepository();
        //    rep.GetBOEDetail(BOENo);
        //    PPGCustomAppraisementBOECont obj = (PPGCustomAppraisementBOECont)rep.DBResponse.Data;
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetContainerBOL(String CFSCode)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetContainerOBL(CFSCode);
            List<DSRCustomAppraisementOBLCont> obj = (List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchOBLWise(String CFSCode)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetContainerOBL(CFSCode);
            List<DSRCustomAppraisementOBLCont> obj = (List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchContainerWise(String OBL)
        {
            DSRCustomAppraisement objOBLContainer = new DSRCustomAppraisement();
            DSR_ImportRepository rep = new DSR_ImportRepository();
            rep.GetOBLContainer(OBL);
            List<DSRCustomAppraisementOBLCont> obj = (List<DSRCustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode, string LineNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.DelCustomAppraisement(CustomAppraisementId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCustomAppraisement(DSRCustomAppraisementNew ObjAppraisement)
        {
            ModelState.Remove("NoOfPackages");
            ModelState.Remove("GrossWeight");
            ModelState.Remove("RMSValue");
            ModelState.Remove("ManualWT");
            ModelState.Remove("MechanicalWT");

            //if (ModelState.IsValid)
            //{
                string AppraisementXML = "";
                string CAOrdXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<DSRCustomAppraisementDtl> LstAppraisement = JsonConvert.DeserializeObject<List<DSRCustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                if (ObjAppraisement.CAOrdDtlXml != null)
                {
                    List<DSRCustomAppraisementOrdDtl> LstCAOrd = JsonConvert.DeserializeObject<List<DSRCustomAppraisementOrdDtl>>(ObjAppraisement.CAOrdDtlXml);
                    CAOrdXML = Utility.CreateXML(LstCAOrd);
                }

                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCustomAppraisement(ObjAppraisement, AppraisementXML, CAOrdXML);
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            //}
            //else
            //{
            //    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
            //    var Err = new { Status = 0, Message = ErrorMessage };
            //    return Json(Err);
            //}
        }

        [HttpGet]
        public ActionResult ViewCustomAppraisement(int CustomAppraisementId)
        {
            DSRCustomAppraisementNew ObjAppraisement = new DSRCustomAppraisementNew();
            if (CustomAppraisementId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (DSRCustomAppraisementNew)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/ViewCustomAppraisement.cshtml", ObjAppraisement);
        }


        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            DSRCustomAppraisementNew ObjAppraisement = new DSRCustomAppraisementNew();
            if (CustomAppraisementId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (DSRCustomAppraisementNew)ObjIR.DBResponse.Data;
                    ObjIR.ListOfChaForPage("", 0);

                    if (ObjIR.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                    ObjIR.ListOfShippingLinePartyCode("", 0);
                    if (ObjIR.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                        ViewBag.State = Jobject["State"];
                    }
                    else
                    {
                        ViewBag.ShippingLineList = null;
                    }

                    /*ObjIR.ListOfImporterForm();
                    if (ObjIR.DBResponse.Data != null)
                    {
                        ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
                    }
                    else
                    {
                        ViewBag.lstImporter = null;
                    }*/

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
                }
            }
            return PartialView("EditCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/EditCustomAppraisement.cshtml", ObjAppraisement);
        }

        [HttpGet]
        public ActionResult GetCustomAppraisementList()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.MenuRights = Jobjectt["lstMenu"];
            }


            List<DSRCustomAppraisement> LstAppraisement = new List<DSRCustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<DSRCustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }
        #endregion
        #region Job Order
        
        [HttpGet]

        public ActionResult CreateJobOrder()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
          //  DSR_JobOrder objJobOrd = new DSR_JobOrder();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }

            //objIR.GetAllTrainNo();
            //if (objIR.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfTrain = objIR.DBResponse.Data;
            //    ViewBag.ListOfTrainPick = JsonConvert.SerializeObject(objIR.DBResponse.Data);

            //}

            //   objIR.GetAllTrainNoforReset();
            //   if (objIR.DBResponse.Data != null)
            // {
            //      ViewBag.ListOfTrainReset = objIR.DBResponse.Data;
            //   ViewBag.ListOfTrainPick = JsonConvert.SerializeObject(objIR.DBResponse.Data);

            //   }

            objIR.ListOfChaForPage("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }

            objIR.ListOfCHAPartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);

                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];

            }
            objIR.ListOfPayerPartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);

                ViewBag.lstPayer = Jobject["lstImporter"];
                ViewBag.PayerState = Jobject["State"];
            }


            objIR.ListOfNewImporterPartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);

                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.ImporterState = Jobject["State"];
            }

            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
           
            }
            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];

            }
            //ViewBag.ListOfShippingLine = objIR.DBResponse.Data;

            DSR_JobOrder objJO = new DSR_JobOrder();
            /*
            
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstpickup = (List<DSRPickupModel>)objIR.DBResponse.Data;
            }
            else
            {
                objJO.lstpickup = null;
            }*/



          //  objIR.GetAllPortForJobOrderTrasport();
           // if (objIR.DBResponse.Data != null)
           // {
           //     objJO.lstPort = (List<TransformList>)objIR.DBResponse.Data;
           // }

            // objJO.JobOrderDate = Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yy hh:mm"));
            objJO.JobOrderDate = DateTime.Now;
            // objJO.JobOrderDate =Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy hh:mm")) ;
            objJO.TrainDate = DateTime.Now;
           // objJobOrd = (DSR_JobOrder)objIR.DBResponse.Data;
            //objJobOrd.SelectPortId = objJobOrd.PortId;
            return PartialView(objJO);
        }

        [HttpGet]
        public ActionResult GetAllTrainNo(string SearchValue="")
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllTrainNo(SearchValue);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            IList<DSR_ImportJobOrderList> lstIJO = new List<DSR_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<DSR_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderDetails(string ContainerNo)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            IList<DSR_ImportJobOrderList> lstIJO = new List<DSR_ImportJobOrderList>();
            objIR.GetAllImpJO(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<DSR_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderDetails", lstIJO);
        }
        [HttpGet]
        public ActionResult GetPort()
        {
            DSR_ImportRepository ObjRR = new DSR_ImportRepository();
            ObjRR.GetPortList();
            if (ObjRR.DBResponse.Data != null)
            {
                return Json(ObjRR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            //PortRepository objImport = new PortRepository();
            //objImport.GetAllPort();
            //if (objImport.DBResponse.Status > 0)
            //    return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSR_ImportJobOrderList> LstJO = new List<DSR_ImportJobOrderList>();
            ObjCR.GetAllImpJO(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DSR_ImportJobOrderList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetPortList()
        //{
        //    PPGMasterRepository objImport = new PPGMasterRepository();
        //    objImport.GetAllPort();
        //    if (objImport.DBResponse.Status > 0)
        //        return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        //    else
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //}



        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            DSR_JobOrder objImp = new DSR_JobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_JobOrder)objIR.DBResponse.Data;
            ViewBag.jdate = objImp.JobOrderDate;
            objIR.ListOfCHAPartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);

                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];

            }
            objIR.ListOfPayerPartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);

                ViewBag.lstPayer = Jobject["lstImporter"];
                ViewBag.PayerState = Jobject["State"];
            }


            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];

            }

            //   objIR.GetAllTrainNo();
            // if (objIR.DBResponse.Data != null)
            //    ViewBag.ListOfTrain = objIR.DBResponse.Data;
            //  objIR.GetAllPortForJobOrderTrasport();
            //  if (objIR.DBResponse.Data != null)
            //  {
            //      objImp.lstPort = (List<DSRTransformList>)objIR.DBResponse.Data;
            //  }

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            DSR_JobOrder objImp = new DSR_JobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_JobOrder)objIR.DBResponse.Data;

            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<DSRPickupModel>)objIR.DBResponse.Data;
            }
          
            return PartialView(objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(DSR_JobOrder objImp, String InvoiceType, String FormOneDetails, String ConDetails,string SEZ)
        {

            List<DSR_ImportJobOrderDtl> lstDtl = new List<DSR_ImportJobOrderDtl>();
            List<DSR_ImportContainerJobOrderDtl> lstContDtl = new List<DSR_ImportContainerJobOrderDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            string ContXML = "";
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_ImportJobOrderDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);

            }


            if (ConDetails != null)
            {
                lstContDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_ImportContainerJobOrderDtl>>(ConDetails);
                if (lstContDtl.Count > 0)
                    ContXML = Utility.CreateXML(lstContDtl);

            }
            objIR.AddEditImpJO(objImp, InvoiceType, XML, ContXML,SEZ);
            return Json(objIR.DBResponse);


        }
        [HttpPost]
        public JsonResult PrintInvoiceService(String InvoiceType, String FormOneDetails,string SEZ)
        {

            List<DSR_ImportJobOrderDtl> lstDtl = new List<DSR_ImportJobOrderDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_ImportJobOrderDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);

            }

            objIR.PrintJobOrderInvoice(InvoiceType, SEZ, XML);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public JsonResult GetTrainDetl(int TrainSumId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetTrainDtl(TrainSumId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetTrainDetailsOnEditMode(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetTrainDetailsOnEditMode(ImpJobOrderId); 
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            ImportRepository objIR = new ImportRepository();
            objIR.GetYardWiseLocation(YardId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public JsonResult LoadImporterList(string PartyCode, int Page)
        //{
        //    DSR_ImportRepository objRepo = new DSR_ImportRepository();
        //    objRepo.ListOfImporterPage(PartyCode, Page);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public JsonResult LoadTraderList(string PartyCode, string TFlag, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfTrader(PartyCode, TFlag, Page);
            //ViewBag.Head = "hello I used JQuery";
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByTraderCode(string PartyCode, string TFlag, int Page)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.ListOfTrader(PartyCode, TFlag, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

       
        public async Task<JsonResult> IRNJO(int ImpJobOrderId)
        {

            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetImportJOIRN(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                List<DSRPrintJOModel> objMdl = new List<DSRPrintJOModel>();
                objMdl = (List<DSRPrintJOModel>)objIR.DBResponse.Data;
                string Message = "";
                foreach(var i in objMdl)
                {
                    var result =await GetIRNForYardInvoice(i.JobOrderNo, i.ContainerType);
                    Message = Message + JsonConvert.SerializeObject(result.Data);


                }
              
                return Json(new { Status = 1, Message = Message });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }






        #region Job Order Print


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJO(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetImportJODetailsFrPrint(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                DSRPrintJOModel objMdl = new DSRPrintJOModel();
                objMdl = (DSRPrintJOModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, ImpJobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        public string GeneratePDFForJO(DSRPrintJOModel objMdl, int ImpJobOrderId)
        {
            Einvoice obj = new Einvoice(new HeaderParam(), "");
            var SEZis = "";
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", TrainNo = "", TrainDate = "", ContainerLoadType = "", CargoType = ""; int Count = 0; 
            int Count40 = 0;
            int Count20 = 0;
            string Sline = "";
            string ImporterName = "";
            string ChaName = "";
            string Html = "";
            string CompanyAddress = "";
            StringBuilder html = new StringBuilder();
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            objMdl.lstDet.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                ImporterName = (ImporterName == "" ? (item.ImporterName) : (ImporterName + "<br/>" + item.ImporterName));
                ChaName = (ChaName == "" ? (item.ChaName) : (ChaName + "<br/>" + item.ChaName));
                Size = (Size == "" ? (item.ContainerSize) : (Size + "<br/>" + item.ContainerSize));
                Sline = (Sline == "" ? (item.Sline) : (Sline + "<br/>" + item.Sline));
                ContainerLoadType = (ContainerLoadType == "" ? (item.ContainerLoadType) : (ContainerLoadType + "<br/>" + item.ContainerLoadType));
                CargoType = (CargoType == "" ? (item.CargoType) : (CargoType + "<br/>" + item.CargoType));
                Serial = (Serial == "") ? (++Count).ToString() : (Serial + "<br/>" + (++Count).ToString());
            });

            Count20 = objMdl.lstDet.ToList().Where(item => item.ContainerSize == "20").Count();
            Count40 = objMdl.lstDet.ToList().Where(item => item.ContainerSize == "40").Count();

            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            //if (System.IO.File.Exists(Path))
            //{
            //    System.IO.File.Delete(Path);
            //}
            if ((Convert.ToInt32(Session["BranchId"])) == 1)
            {
                //Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";

                html.Append("<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>");
            }
            else
            {
                //Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>  <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>IMPORT JOB ORDER</span></th></tr></tbody></table></td></tr></thead>   <tbody> <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td style='text-align:left; width:50%;'>Job Order No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;width:50%;'>Job Order Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr></tbody></table></td></tr>   <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th>TO,</th></tr> <tr><th>The Manager(CT/OPERATIONS)</th></tr> <tr><td><span><br/></span></td></tr> <tr><td>" + objMdl.FromLocation + "</td></tr> <tr><td><span>&nbsp;&nbsp;</span>SIR,</td></tr> <tr><td><span>&nbsp;&nbsp;</span>YOU ARE REQUESTED TO KINDLY ARRANGE TO DELIVER THE FOLLOWING IMPORT CONTAINERS / CBT TO ICD DASHRATH,VADODARA.</td></tr> </tbody></table></td></tr>      <tr><td colspan='12' style='text-align:center;'><br/></td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center; width:25px;'>SL.NO</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>CONTAINER / CBT NO.</th><th style='border:1px solid #000;padding:5px;text-align:center; width:60px;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>G/HAZ</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>SLA CODE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>F/L</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + CargoType + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerLoadType + "</td></tr></tbody> </table></td></tr> <tr><td><span><br/></span></td></tr> <tr><td colspan='1'></td><th colspan='10'>TOTAL CONTAINERS / CBT : 20x " + Count20 + " + 40x " + Count40 + "</th></tr> <tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td colspan='2'></td><td width='15%' valign='top' align='right'>Note :</td><td colspan='2' width='85%'>THE FOLLOWING CONTAINERS / CBT ARE REQUIRED TO BE SCANNED BEFORE ITS DELIVERY FROM THE PORT AS DESIRED BY THE CUSTOM SCANNING DIVISION</td></tr></tbody></table></td></tr>   <tr><td colspan='12'><span><br/><br/></span></td></tr> <tr><td colspan='12' style='text-align:right;'>FOR MANAGER <br/> ICD DASHRATH</td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody> <tr><td><span><br/></span></td></tr>   <tr><td colspan='12'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to <br/> 1- M/S Suman Forwarding Agency Pvt.Ltd - for arranging movement of the Containers / CBT from " + objMdl.FromLocation + " within time. failing which dwell time charges as per procedure will be debited to your account as per claim receive from line.<br/> 2-The Preventive Office, Customs,ICD Dashrath.</td></tr></tbody></table></td></tr>    </tbody></table></td></tr></tbody></table>";

                html.Append("<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><thead>");
                html.Append("<tr><td colspan='12'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr>");
                html.Append("<td width='80%'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody>");
                html.Append("<tr><td width='60%' valign='top'><img align='right' src='IMGSRC'/></td>");
                html.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><span style='font-size: 7pt; padding-bottom: 10px;'>" + CompanyAddress + "</span><br/><label style='font-size: 7pt; font-weight: bold;'>IMPORT JOB ORDER</label></td>");
                html.Append("</tr>");
                //html.Append("<tr><td colspan='12' valign='top' style='font-size:8pt;'><b>IRN :</b> " + item.irn + " </td></tr>");
                html.Append("</tbody></table></td>");
               // html.Append("<td align='left' valign='top'><img style='width:115px; height:115px' valign='top' align='right' src='" + LoadImage(obj.GenerateQCCode(item.SignedQRCode)) + "'/> </td>");
                html.Append("</tr>");
                html.Append("</tbody></table>");
                html.Append("</td></tr>");
                html.Append("</thead></table>");

                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");


                //html.Append("<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'>");
                //html.Append("<thead>");
                //html.Append("<tr><td colspan='1' width='10%' valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
                //html.Append("<td colspan='8' width='150%' style='text-align: center;'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1>");
                //html.Append("<label style='display: block; font-size: 7pt;'>(A Govt. of India Undertaking) </label><br />");
                //html.Append("<span style='display: block; font-size: 7pt;text-transform: uppercase; padding-bottom: 10px;'>" + CompanyAddress + "</span>");
                //html.Append("<br/><label style='font-size: 7pt; font-weight:bold;'>IMPORT JOB ORDER</label>");
                //html.Append("</td></tr>");
                //html.Append("</thead>");

                //html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='12'>");
                html.Append("<table style='width:100%; font-size:7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td style='text-align:left; width:50%;'>Job Order No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td>");
                html.Append("<td style='text-align:right;width:50%;'>Job Order Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12'>");
                html.Append("<table style='width:100%; font-size:7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<th>TO,</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<th>The Manager(CT/OPERATIONS)</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><span><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td>" + objMdl.FromLocation + "</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><span>&nbsp;&nbsp;</span>SIR,</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><span>&nbsp;&nbsp;</span>YOU ARE REQUESTED TO KINDLY ARRANGE TO DELIVER THE FOLLOWING IMPORT CONTAINERS / CBT TO ICD DASHRATH,VADODARA.</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12' style='text-align:center;'>");
                html.Append("<br/>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12'>");
                html.Append("<table style='width:100%;font-size:6pt;font-family:Verdana,Arial,San-serif;border:1px solid #000; border-collapse:collapse;'>");
                html.Append("<thead>");
                html.Append("<tr>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:25px;'>SL.NO</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>CONTAINER / CBT NO.</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:40px;'>SIZE</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:60px;'>G/HAZ</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:180px;'>Importer Name</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:150px;'>CHA Name</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>Shipping Line Name</th>");
                html.Append("<th cellpadding='5' style='border:1px solid #000;padding:5px;text-align:center; width:60px;'>F/L</th>");
                html.Append("</tr>");
                html.Append("</thead>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + CargoType + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + ImporterName + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + ChaName + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td>");
                html.Append("<td cellpadding='5' valign='top' style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerLoadType + "</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td><span><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='1'></td>");
                html.Append("<th colspan='10' style='font-size:7pt;'>TOTAL CONTAINERS / CBT : 20x " + Count20 + " + 40x " + Count40 + "</th>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='4'>");
                html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-size:7pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td colspan='2'></td>");
                html.Append("<td width='15%' valign='top' align='right'>Note :</td>");
                html.Append("<td colspan='2' width='85%'>THE FOLLOWING CONTAINERS / CBT ARE REQUIRED TO BE SCANNED BEFORE ITS DELIVERY FROM THE PORT AS DESIRED BY THE CUSTOM SCANNING DIVISION</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12'><span><br/><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12' style='text-align:right; font-size:7pt;'>FOR MANAGER");
                html.Append("<br/> ICD DASHRATH</td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12'>");
                html.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'>");
                html.Append("<tbody>");
                html.Append("<tr>");
                html.Append("<td><span><br/></span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append("<td colspan='12'>");
                html.Append("<table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;'>");
                html.Append("<tbody>");
                html.Append(" <tr>");
                html.Append("<td>Copy to");
                html.Append("<br/> 1- M/S Shri Durga Crane Co. H & T Contractor - for arranging movement of the Containers / CBT from " + objMdl.FromLocation + " within time. failing which dwell time charges as per procedure will be debited to your account as per claim receive from line.");
                html.Append("<br/> 2-The Preventive Office, Customs,ICD Dashrath.</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
                html.Append("</td>");
                html.Append("</tr>");
                html.Append("</tbody>");
                html.Append("</table>");
            }
            // string Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>CONTAINER FREIGHT STATION<br/>18, COAL DOCK ROAD, KOLKATA - 700 043</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderNo+"</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderDate+"</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> "+objMdl.FromLocation+" </span> to<span style='border-bottom:1px solid #000;'> "+objMdl.ToLocation+" </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>"+ Serial + "</td><td style='border:1px solid #000;padding:5px;'>"+ContainerNo+"</td><td style='border:1px solid #000;padding:5px;'>"+Size+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ShippingLineName+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ContainerType+"</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 20f, 30f, 30f, 30f, false, true))
            {
                rh.GeneratePDF(Path, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
        }




        #endregion


        #endregion

        #region SealCutting

        [HttpGet]
        public ActionResult AddSealCuting()
        {
            string check = "";
            DSR_SealCutting SC = new DSR_SealCutting();
            SC.TransactionDate = DateTime.Now.ToString("dd/MM/yyyy");

            //DSR_ImportRepository objImport = new DSR_ImportRepository();
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
        public ActionResult GetListOfSealCuttingDetails()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetListOfSealCuttingDetails(0);
            IList<DSR_SealCutting> lstSC = new List<DSR_SealCutting>();
            if (objIR.DBResponse.Data != null)
                lstSC = (List<DSR_SealCutting>)objIR.DBResponse.Data;
            return PartialView("ListOfSealCuttingDetails", lstSC);

        }
        [HttpGet]
        public JsonResult LoadListMoreDataForSealCutting(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSR_SealCutting> LstJO = new List<DSR_SealCutting>();
            ObjCR.GetListOfSealCuttingDetails(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DSR_SealCutting>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSealCuttingById(int SealCuttingId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetSealCuttingById(SealCuttingId);
            DSR_SealCutting objImp = new DSR_SealCutting();
            if (objImport.DBResponse.Data != null)
                objImp = (DSR_SealCutting)objImport.DBResponse.Data;
            return PartialView("EditSealCutting", objImp);
        }
        [HttpGet]
        public ActionResult ViewSealCuttingDetailById(int SealCuttingId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetSealCuttingById(SealCuttingId);
            DSR_SealCutting objImp = new DSR_SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_SealCutting)objIR.DBResponse.Data;
            return PartialView("ViewSealCuttingDetailById", objImp);
        }

        [HttpGet]
        public JsonResult GetInvoiceDtlForSealCutting(String TransactionDate, String GateInDate, String ContainerNo, String size, int CHAShippingLineId, String CFSCode, int OBLType)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetInvoiceDtlForSealCutting(TransactionDate, GateInDate, ContainerNo, size, CHAShippingLineId, CFSCode, OBLType);
            DSR_SealCutting objImp = new DSR_SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_SealCutting)objIR.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfContainer()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfContainer();
            List<DSR_SealCutting> objImp = new List<DSR_SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSR_SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfBLData(int OBLId, int impobldtlId, string OBLFCLLCL)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfBL(OBLId, impobldtlId, OBLFCLLCL);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.OBLList = objImport.DBResponse.Data;
            //else
            //{
            //    ViewBag.OBLList = null;
            //}

            List<DSR_SealCutting> objImp = new List<DSR_SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSR_SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfGodownData()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfGodownRights(((Login)(Session["LoginUser"])).Uid);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            List<DSRGodownList> objImp = new List<DSRGodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRGodownList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCHAShippingLineData()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfCHAShippingLine();
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

            List<DSR_SealCuttingCHA> objImp = new List<DSR_SealCuttingCHA>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSR_SealCuttingCHA>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SealCuttingInvoicePrint(int InvoiceId)
        {
            DSR_InvoiceRepository objGPR = new DSR_InvoiceRepository();
            objGPR.GetSCInvoiceDetailsForPrint(InvoiceId);
            DSR_invoiceSealCutting objSC = new DSR_invoiceSealCutting();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objSC = (DSR_invoiceSealCutting)objGPR.DBResponse.Data;
                FilePath = GeneratingPDF(objSC, InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        private string GeneratingPDF(DSR_invoiceSealCutting objSC, int InvoiceId)
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
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span><br />" +
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
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html);
            }
            return "/Docs/" + Session.SessionID + "/SealCutting" + InvoiceId.ToString() + ".pdf";
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSealCutting(DSR_SealCutting objSealCutting)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //if (ModelState.IsValid)
                //{

                string OBLXML = "";
                string ChargesBreakupXML = "";
                if (objSealCutting.ViewBLList != null)
                {
                    var ViewBLList = JsonConvert.DeserializeObject<List<DSR_SealCutting>>(objSealCutting.ViewBLList.ToString());
                    if (ViewBLList != null)
                    {
                        OBLXML = Utility.CreateXML(ViewBLList);
                    }
                }
                if (objSealCutting.lstPostPaymentChrgBreakupAmt != null)
                {
                    var ViewBreakList = JsonConvert.DeserializeObject<List<DSR_SealPostPaymentChargebreakupdate>>(objSealCutting.lstPostPaymentChrgBreakupAmt.ToString());

                    ChargesBreakupXML = Utility.CreateXML(ViewBreakList);
                }

                DSR_ImportRepository objImport = new DSR_ImportRepository();
                objImport.AddEditSealCutting(objSealCutting, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, OBLXML);
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
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.DeleteSealCutting(SealCuttingId);

            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tally Sheet Generation For LCL
        [HttpGet]
        public ActionResult GetTallySheet()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetAllOblCont(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid);
            if (objIR.DBResponse.Data != null)
                ViewBag.ContainerNo = objIR.DBResponse.Data;
            else ViewBag.ContainerNo = null;
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            ViewBag.Currentdate = DateTime.Now.ToString("dd/MM/yyyy");
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditTallySheet(TallySheet objSheet)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository objIR = new DSR_ImportRepository();
                string XML = "";
                if (objSheet.StringifyXML != "")
                {
                    objSheet.lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<TallySheetDtl>>(objSheet.StringifyXML).ToList();
                    XML = Utility.CreateXML(objSheet.lstDtl);
                }
                objIR.AddEditTallySheet(objSheet, XML, Convert.ToInt32(Session["BranchId"]));
                return Json(objIR.DBResponse);
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [HttpGet]
        public ActionResult GetTallySheetList()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.ListOfTallySheet(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid, 0);
            IList<TallySheetList> lstTally = new List<TallySheetList>();
            if (objIR.DBResponse.Data != null)
                lstTally = (List<TallySheetList>)objIR.DBResponse.Data;
            return PartialView(lstTally);
        }
        [HttpGet]
        public JsonResult GetTallySheetListForPage(int Page)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.ListOfTallySheet(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid, Page);
            IList<TallySheetList> lstTally = new List<TallySheetList>();
            if (objIR.DBResponse.Data != null)
                lstTally = (List<TallySheetList>)objIR.DBResponse.Data;
            return Json(lstTally, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewTallySheet(int TallySheetId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetTallySheet(Convert.ToInt32(Session["BranchId"]), TallySheetId, ((Login)(Session["LoginUser"])).Uid);
            TallySheet objTallySheet = new TallySheet();
            if (objIR.DBResponse.Data != null)
                objTallySheet = (TallySheet)objIR.DBResponse.Data;
            return PartialView(objTallySheet);
        }
        [HttpGet]
        public ActionResult EditTallySheet(int TallySheetId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetTallySheet(Convert.ToInt32(Session["BranchId"]), TallySheetId, ((Login)(Session["LoginUser"])).Uid);
            TallySheet objTallySheet = new TallySheet();
            if (objIR.DBResponse.Data != null)
                objTallySheet = (TallySheet)objIR.DBResponse.Data;
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView(objTallySheet);
        }
        [HttpGet]
        public JsonResult GetObldataAgainstContId(int SealCuttingId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetOblContDet(Convert.ToInt32(Session["BranchId"]), SealCuttingId, ((Login)(Session["LoginUser"])).Uid);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else return Json(objIR.DBResponse.Data, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteTallySheet(int TallySheetId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.DeleteTallySheet(TallySheetId, Convert.ToInt32(Session["BranchId"]));
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintTallySheet(int TallySheetId)
        {
            DSR_ImportRepository objIr = new DSR_ImportRepository();
            objIr.PrintTallySheet(TallySheetId, Convert.ToInt32(Session["BranchId"]));
            string Path = "";
            if (objIr.DBResponse.Data != null)
            {
                TallySheetPrintHeader objTS = new TallySheetPrintHeader();
                objTS = (TallySheetPrintHeader)objIr.DBResponse.Data;
                Path = GeneratedTallySheetPrint(objTS, TallySheetId);
                return Json(new { Status = 1, Message = "Done", data = Path });
            }
            return Json(new { Status = 0, Message = "Done", data = Path });
        }
        [NonAction]
        public string GeneratedTallySheetPrint(TallySheetPrintHeader objTS, int TallySheetId)
        {

            string html = "<table cellspacing='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '> <tbody>";

            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>ICD DASHRATH</label><br/><label style='font-size: 16px; font-weight:bold;'>TALLY SHEET</label></td></tr>";
            html += "</tbody></table>";
            html += "</td></tr>";

            html += "<tr><td colspan='12' style='font-size:13px; text-align:right;'><b>ON DATE :</b> " + objTS.TallySheetDateTime + "</td></tr>";

            html += "<tr> <td colspan='12'> <table style='width:100%; margin-bottom: 10px;'cellspacing='0'> <tbody> <tr> <td> <table style='width:100%; margin-top: 10px; margin-bottom: 10px;' cellspacing='0' > <tbody> <tr> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Tally Sheet No.</label> <span>" + objTS.TallySheetNo + "</span> </td> <td colspan='2' style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Date of Tally: </label> <span>" + objTS.TallySheetDate + "</span> </td> <td colspan='3' style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Shed No.: </label> <span>" + objTS.GodownNo + "</span> </td> </tr> <tr> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Container / CBT No.: </label> <span>" + objTS.ContainerNo + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Size:</label> <span>" + objTS.Size + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>In Date:</label> <span>" + objTS.GateInDate + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Custom Seal No.:</label> <span>" + objTS.CustomSealNo + "</span> </td> <td colspan='2' style='font-size: 12px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>Sla Seal No.:</label> <span>" + objTS.SlaSealNo + "</span> </td> </tr> <tr> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>ICD Code: </label> <span>" + objTS.CFSCode + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>IGM No.:</label> <span>" + objTS.IGM_No + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>OBL Status:</label> <span>" + objTS.MovementType + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>SLA:</label> <span>" + objTS.ShippingLine + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>POL:</label> <span>" + objTS.POL + "</span> </td> <td style='font-size: 11px; line-height: 26px;padding:5px;'> <label style='font-weight: bold;'>POD:</label> <span>" + objTS.POD + "</span> </td> </tr> </tbody> </table> </td> </tr> <tr> <td> <table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' > <thead> <tr> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SMTP No.</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>OBL No.</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Importer</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Cargo</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Type</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No. Pkg</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Pkg Rec</th> <th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Gr Wt</th> <th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Pro Area</th> </tr> </thead>";
            html += "<tfoot> <tr> <td colspan='6' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; font-weight: bold; text-align: left; padding: 5px;'>Total</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + Convert.ToInt32(objTS.lstDetaiils.Sum(x => x.NoOfPkg)) + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + Convert.ToInt32(objTS.lstDetaiils.Sum(x => x.PkgRec)) + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + Convert.ToDecimal(objTS.lstDetaiils.Sum(x => x.Weight)) + "</td> <td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Convert.ToDecimal(objTS.lstDetaiils.Sum(x => x.Area)) + "</td> </tr></tfoot><tbody>";
            int Serial = 1;
            objTS.lstDetaiils.ForEach(item =>
            {
                html += "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + Serial++ + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.SMTPNo + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.OBL_No + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.Importer + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.Cargo + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.Type + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.NoOfPkg + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.PkgRec + "</td> <td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>" + item.Weight + "</td> <td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + item.Area + "</td> </tr>";
            });
            html += "</tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            html = html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            var Path = Server.MapPath("~/Docs/" + Session.SessionID + "/TallySheet" + TallySheetId + ".pdf");
            if (!Directory.Exists(Server.MapPath("~/Docs/" + Session.SessionID)))
                Directory.CreateDirectory(Server.MapPath("~/Docs/" + Session.SessionID));
            if (System.IO.File.Exists(Path))
                System.IO.File.Delete(Path);
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            ObjRR.getCompanyDetails();
            string HeadOffice = "", HOAddress = "", ZonalOffice = "", ZOAddress = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                ZonalOffice = objCompanyDetails.CompanyName;
                ZOAddress = objCompanyDetails.CompanyAddress;
            }

            using (var rh = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                rh.HeadOffice = HeadOffice;
                rh.HOAddress = HOAddress;
                rh.ZonalOffice = ZonalOffice;
                rh.ZOAddress = ZOAddress;
                rh.GeneratePDF(Path, html);
            }
            return "/Docs/" + Session.SessionID + "/TallySheet" + TallySheetId + ".pdf";
        }

        [HttpGet]
        public ActionResult GetTallySheetSearchByContainer(string ContainerNo)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.TallySheetSearchByContainer(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid, ContainerNo);
            IList<TallySheetList> lstTally = new List<TallySheetList>();
            if (objIR.DBResponse.Data != null)
                lstTally = (List<TallySheetList>)objIR.DBResponse.Data;
            return PartialView("GetTallySheetList", lstTally);
        }
        #endregion

        #region Custom Appraisement Approval

        public string SetMenuId(int Id)
        {

            Session["MenuId"] = Id;
            // Returns string "Electronic" or "Mail"
            return "success";
        }
        [HttpGet]
        public ActionResult ListOfApprsmntAppr()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            //Access Rights
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;

            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {
                var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.MenuRights = Jobjectt["lstMenu"];
            }

            objIR.NewCustomeAppraisement(0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.ListOfApp = Jobject["lstApproval"];

                // ViewBag.ListOfApp =objIR.DBResponse.Data;
                // var jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                //var jobject = Newtonsoft.Json.Linq.JObject.Parse(jobj);
                //ViewBag.ListOfApp = jobject["Details"];
            }
            else
            {
                ViewBag.ListOfApp = null;
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult LoadMoredata(int Skip, string status)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            if (status == "N")
            {
                objIR.NewCustomeAppraisement(Skip);
            }
            else
            {
                objIR.ApprovalHoldCustomAppraisement(Skip);
            }
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LoadApprovalPage(int CstmAppraiseAppId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Custom_AppraiseApproval ObjAppId = new DSR_Custom_AppraiseApproval();
            ObjIR.GetCstmAppraiseApplication(CstmAppraiseAppId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjAppId = (DSR_Custom_AppraiseApproval)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraisementApproval", ObjAppId);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApproval(int CstmAppraiseAppId, int IsApproved)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.UpdateCustomApproval(CstmAppraiseAppId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

        #region Destuffing LCL
        [HttpGet]

        public ActionResult CreateDestuffingEntry()
        {
            DSR_ExportRepository ObjER = new DSR_ExportRepository();
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSRDestuffingEntry ObjDestuffing = new DSRDestuffingEntry();
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjDestuffing.StartDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjIR.GetContrNoForDestuffingEntry(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ObjIR.DBResponse.Data));
            }
            else
            {
                ViewBag.ContainerList = null;
            }

            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjIR.ListOfShippingLinePartyCode("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.ShippingLineList = null;
            }
            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }


            ObjIR.ListOfGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }

            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            return PartialView("GetDestuffingEntry", ObjDestuffing);
        }

        //public ActionResult CreateDestuffingEntry()
        //{
        //    DSR_ExportRepository ObjER = new DSR_ExportRepository();
        //    DSR_ImportRepository ObjIR = new DSR_ImportRepository();
        //    DSRDestuffingEntry ObjDestuffing = new DSRDestuffingEntry();
        //    ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd/MM/yyyy");
        //    ObjIR.GetContrNoForDestuffingEntry(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid);
        //    if (ObjIR.DBResponse.Data != null)
        //    {
        //        ViewBag.ContainerList = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ObjIR.DBResponse.Data));
        //    }
        //    else
        //    {
        //        ViewBag.ContainerList = null;
        //    }

        //    ObjIR.ListOfChaForPage("", 0);

        //    if (ObjIR.DBResponse.Data != null)
        //    {
        //        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
        //        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        ViewBag.lstCHA = Jobject["lstCHA"];
        //        ViewBag.CHAState = Jobject["State"];
        //    }
        //    ObjIR.ListOfShippingLinePartyCode("", 0);
        //    if (ObjIR.DBResponse.Data != null)
        //    {
        //        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
        //        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        ViewBag.lstShippingLine = Jobject["lstShippingLine"];
        //        ViewBag.State = Jobject["State"];
        //    }
        //    else
        //    {
        //        ViewBag.ShippingLineList = null;
        //    }
        //    ObjER.GetAllCommodityForPage("", 0);
        //    if (ObjER.DBResponse.Data != null)
        //    {
        //        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
        //        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
        //        ViewBag.LstCommodity = Jobject["LstCommodity"];
        //        ViewBag.CommodityState = Jobject["State"];
        //    }

        //    ObjIR.ListOfGodown();
        //    if (ObjIR.DBResponse.Data != null)
        //    {
        //        ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
        //    }
        //    else
        //    {
        //        ViewBag.ListOfGodown = null;
        //    }

        //    AccessRightsRepository ACCR = new AccessRightsRepository();
        //    ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
        //    ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

        //    return PartialView("GetDestuffingEntry", ObjDestuffing);
        //}

        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(int TallySheetId, String CFSCode)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            ObjIR.GetContrDetForDestuffingEntry(TallySheetId, Convert.ToInt32(Session["BranchId"]), CFSCode);

            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOBLDetForDestuffingEntry(int TallySheetId, String CFSCode)
        {
            DSR_ImportRepository ObjOBL = new DSR_ImportRepository();
            ObjOBL.GetOBLforDestuffingEntry(TallySheetId, Convert.ToInt32(Session["BranchId"]), CFSCode);
            return Json(ObjOBL.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntry(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSR_DestuffingList> LstJO = new List<DSR_DestuffingList>();
            ObjCR.GetAllDestuffingEntry(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DSR_DestuffingList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_DestuffingList> LstDestuffing = new List<DSR_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<DSR_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {

            DSRDestuffingEntry ObjDestuffing = new DSRDestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                DSR_ExportRepository ObjER = new DSR_ExportRepository();
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                //ObjIR.ListOfCHA();
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                //}
                //else
                //{
                //    ViewBag.CHAList = null;
                //}

                ObjIR.ListOfChaForPage("", 0);

                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }

                ObjER.GetAllCommodity();
                if (ObjER.DBResponse.Data != null)
                    ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;
                //ObjIR.ListOfShippingLine();
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    ViewBag.ShippingLineList = new SelectList((List<ShippingLine>)ObjIR.DBResponse.Data, "ShippingLineId", "ShippingLineName");
                //}
                //else
                //{
                //    ViewBag.ShippingLineList = null;
                //}

                ObjIR.ListOfShippingLinePartyCode("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                    ViewBag.State = Jobject["State"];
                }
                else
                {
                    ViewBag.ShippingLineList = null;
                }

                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "Edit");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (DSRDestuffingEntry)ObjIR.DBResponse.Data;
                }

                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }

            }
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView("EditDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public ActionResult ViewDestuffingEntry(int DestuffingEntryId)
        {
            DSRDestuffingEntry ObjDestuffing = new DSRDestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "View");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (DSRDestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntry", ObjDestuffing);
        }



        [HttpGet]

        public JsonResult GetGodownLocationById(int GodownId)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.GetGodownLocationById(GodownId);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);


        }
        public JsonResult ListOfGodownByOBL(string OBL, int DestuffingEntryDtlId,string OBLDate)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();


            ObjIR.ListOfGodownByOBL(OBL, DestuffingEntryDtlId, OBLDate);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownListWithDestiffDetails>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
            }
            else
            {
                ViewBag.ListOfGodown = null;
            }



            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);


        }










        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteDestuffingEntry(int DestuffingEntryId)
        {
            if (DestuffingEntryId > 0)
            {
                ImportRepository ObjIR = new ImportRepository();
                ObjIR.DelDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]));
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDestuffingEntry(DSRDestuffingEntry ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                string DeliveryOrdDtlXml = "";
                List<DSR_DestuffingEntryDtl> LstDestuffingEntry = new List<DSR_DestuffingEntryDtl>();
                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_DestuffingEntryDtl>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                }

                //Added by Vineet
                if (ObjDestuffing.LstDeliveryordDtl != null)
                {
                    ObjDestuffing.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DSRDeliveryOrdDtl>>(ObjDestuffing.DeliveryOrdDtlXml);
                    DeliveryOrdDtlXml = Utility.CreateXML(ObjDestuffing.LstDeliveryordDtl);
                }
                //End

                //string DestufGodownEntryXML = "";
                //List<DSR_DestufGodownDetails> LstDestufGodownEntry = new List<DSR_DestufGodownDetails>();
                //if(ObjDestuffing.DestufGodownEntryXML!=null)
                //{
                //    LstDestufGodownEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_DestufGodownDetails>>(ObjDestuffing.DestufGodownEntryXML);
                //    DestufGodownEntryXML = Utility.CreateXML(LstDestufGodownEntry);
                //}

                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.AddEditDestuffingEntry(ObjDestuffing, DestuffingEntryXML, DeliveryOrdDtlXml /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]), Convert.ToInt32(((Login)(Session["LoginUser"])).Uid));
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
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
        public JsonResult PrintDestuffingSheet(int DestuffingEntryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetDestuffEntryForPrint(DestuffingEntryId);
            if (ObjIR.DBResponse.Data != null)
            {
                DSR_DestuffingSheet ObjDestuff = new DSR_DestuffingSheet();
                ObjDestuff = (DSR_DestuffingSheet)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForDestuffSheet(ObjDestuff, DestuffingEntryId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePDFForDestuffSheet(DSR_DestuffingSheet ObjDestuff, int DestuffingEntryId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
            StringBuilder objSB = new StringBuilder();
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            objSB.Append("<table style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '>");
            objSB.Append("<tbody><tr><td style='text-align: right;' colspan='12'>");
            objSB.Append("<h1 style='font-size: 12px; line-height: 20px; font-weight: 300;margin: 0; padding: 0;'>");
            objSB.Append("</h1></td></tr>");

            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>");
            objSB.Append("<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>");
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 16px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 7pt;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 7pt;'>ICD DASHRATH</label><br/><label style='font-size: 7pt; font-weight:bold;'>DESTUFFING SHEET</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
            //objSB.Append("<th style='font-size:13px;width:10%'>SHED CODE:</th><td style='font-size:12px;'>" + ObjDestuff.GodownName + "</td>");
            objSB.Append("<th style='font-size:7pt; text-align:right;'>" + ObjDestuff.DestuffingEntryDateTime + "</th>");
            objSB.Append("</tr></tbody></table></td></tr>");

            //objSB.Append("<tr><td style='text-align: left;'>");
            //objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>SHED CODE: <label>" + ObjDestuff.GodownName + "</label>");
            //objSB.Append("</span></td><td colspan='3' style='text-align: center;'>");
            //objSB.Append("<span style='display: block; font-size: 14px; line-height: 22px;  padding-bottom: 10px; font-weight:bold;'>DESTUFFING SHEET</span>");
            //objSB.Append("</td><td colspan='2' style='text-align: left;'><span style='display: block; font-size: 11px; padding-bottom: 10px;'>");
            //objSB.Append("AS ON: <label>" + ObjDestuff.DestuffingEntryDateTime + "</label></span></td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='border:1px solid #000; width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='6' width='50%'><b>Destuff Sheet No. :</b>" + ObjDestuff.DestuffingEntryNo + "</td> <td colspan='6' width='50%' align='right'><b>Destuffing Date :</b>" + ObjDestuff.StartDate + "</td> </tr></tbody></table></td></tr>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='3' width='25%'><b>Container No. :</b>" + ObjDestuff.ContainerNo + "</td> <td colspan='3' width='25%' align='center'><b>Size :</b>" + ObjDestuff.Size + "</td> <td colspan='3' width='25%'><b>ICD Code :</b>" + ObjDestuff.CFSCode + "</td> <td colspan='3' width='25%'><b>In Date :</b>" + ObjDestuff.GateInDate + "</td> </tr></tbody></table></td></tr>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='12' width='100%'><b>Shipping Line Name :</b>" + ObjDestuff.ShippingLine + "</td> </tr></tbody></table></td></tr>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='12' width='100%'><b>CHA Name :</b>" + ObjDestuff.CHAName + "</td> </tr></tbody></table></td></tr>");
            objSB.Append("<tr><td colspan='12'><table cellpadding='5' style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><tbody><tr> <td colspan='6' width='50%'><b>Destuffing Type :</b>" + ObjDestuff.DestuffingType + "</td> <td colspan='6' width='50%'><b>Examination Type :</b>" + ObjDestuff.RMS + "</td> </tr></tbody></table></td></tr>");
            objSB.Append("</tbody></table></td></tr>");


            //objSB.Append("<tr><td colspan='12'>");
            //objSB.Append("<table style='width:100%; margin: 0;margin-bottom: 10px;'><tbody><tr><td style='font-size: 11px; padding-bottom:15px;'>");
            //objSB.Append("<label style='font-weight: bold;'>DESTUFF SHEET NO.:</label> <span>" + ObjDestuff.DestuffingEntryNo + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>START DATE OF DESTUFFING : </label> <span>" + ObjDestuff.StartDate + "</span></td>");
            //objSB.Append("<td colspan='3' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>FINAL DATE OF DESTUFFING : </label> <span>" + ObjDestuff.DestuffingEntryDate + "</span></td></tr>");
            //objSB.Append("<tr><td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Container / CBT No.</label> <span>" + ObjDestuff.ContainerNo + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Size : </label> <span>" + ObjDestuff.Size + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>In Date : </label> <span>" + ObjDestuff.GateInDate + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Custom Seal No. : </label> <span>" + ObjDestuff.CustomSealNo + "</span></td>");
            //objSB.Append("<td colspan='2' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Sla Seal no. : </label> <span>" + ObjDestuff.SlaSealNo + "</span></td></tr>");
            //objSB.Append("<tr><td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>ICD Code</label> <span>" + ObjDestuff.CFSCode + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>IGM No. : </label> <span>" + ObjDestuff.IGMNo + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>OBL Type: </label> <span>" + ObjDestuff.MovementType + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>SLA : </label> <span>" + ObjDestuff.ShippingLine + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>POL : </label> <span>" + ObjDestuff.POL + "</span></td>");
            //objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>POD : </label> <span>" + ObjDestuff.POD + "</span></td></tr>");
            //objSB.Append("</tbody></table></td></tr>");

            objSB.Append("<tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table style='border:1px solid #000; font-size:6pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SR No.</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SMTP No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>IGM No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>OBL No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Importer</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Godown</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Cargo</th>");            
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px; width: 6%;'>No. Pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px; width: 8%;'>Gr. Wt</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px; width: 6%;'>Recd.pkg</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Slot No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px; width: 8%;'>Area</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Remarks</th>");
            objSB.Append("</tr></thead>");

            objSB.Append("<tfoot><tr>");
            objSB.Append("<th colspan='6' style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: left; padding: 5px;'>TOTAL :</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.NoOfPkg)) + "</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Weight)) + "</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.PkgRec)) + "</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Area)) + "</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px;'></th>");


            //objSB.Append("<th colspan='5' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-weight: bold; text-align: left; padding: 5px;'>TOTAL :</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.NoOfPkg)) + "</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.PkgRec)) + "</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; '>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Weight)) + "</th>");
            //objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; '>Dynamic</th>");
            //objSB.Append("<th style='border-bottom: 1px solid #000; text-align: center;' colspan='2'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Area)) + "</th>");
            objSB.Append("</tr></tfoot>");

            objSB.Append("<tbody>");
            int Serial = 1;
            ObjDestuff.lstDtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Serial + "</td>");
                //objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SMTPNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + ObjDestuff.IGMNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.OblNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Importer + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Type + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.GodownName + "</td>");
                //objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Cargo + "</td>");                
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + item.NoOfPkg + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + item.Weight + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + item.PkgRec + "</td>");
                //objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.GodownWiseLctnNames + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: right; padding: 5px;'>" + item.Area + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + item.Remarks.Replace("&", " and ").ToString() + "</td>");
                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr>");

            //objSB.Append("<tr><td colspan='12' style=' font-size: 11px; padding-top: 15px; text-align: left;'>*GOODS RECEIVED ON S/C &amp; S/W BASIC - CWC IS NOT RESPONSIBLE FOR SHORT LANDING &amp; LEAKAGES IF ANY</td></tr>");
            //objSB.Append("<tr><td colspan='12' style=' font-size: 12px; text-align: left;padding-top: 15px;'>Signature &amp; Designation :</td></tr>");
            objSB.Append("</tbody></table></td></tr>");
            objSB.Append("<tr><th colspan='3' style=' font-size: 7pt; width: 25%; text-align: center;padding-top: 100px;'>Sign of H &amp; T/Surveyour</th>");
            objSB.Append("<th colspan='3' style='font-size: 7pt; width: 25%; text-align: center;padding-top: 100px;'>Sign of Importer/CHA <br/> Representative</th>");
            objSB.Append("<th colspan='3' style='font-size: 7pt; width: 25%; text-align: center;padding-top: 100px;'>Shed Incharge <br/> CWC/CFS/ICD</th>");
            objSB.Append("<th colspan='3' style='font-size: 7pt; width: 25%; text-align: center;padding-top: 100px;'>Sign of Custome Officer</th></tr>");
            objSB.Append("</tbody></table>");
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            WFLD_ReportRepository ObjRR = new WFLD_ReportRepository();
            ObjRR.getCompanyDetails();
            string HeadOffice = "", HOAddress = "", ZonalOffice = "", ZOAddress = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                ZonalOffice = objCompanyDetails.CompanyName;
                ZOAddress = objCompanyDetails.CompanyAddress;
            }

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var RH = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                RH.HeadOffice = HeadOffice;
                RH.HOAddress = HOAddress;
                RH.ZonalOffice = ZonalOffice;
                RH.ZOAddress = ZOAddress;

                //RH.HeadOffice = ZonalOffice;
                //RH.HOAddress = "(A Govt.Of India Undertaking)";
                //RH.ZonalOffice = ZOAddress;
                //RH.ZOAddress = "";

                RH.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
        }
        [HttpGet]
        public JsonResult GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetCIFandDutyForBOE(BOENo, BOEDate);
            if (objIR.DBResponse.Data != null)
                return Json(new { Status = 1, Message = "Success", Data = objIR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = 0, Message = "No Data", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            DSR_ExportRepository objRepo = new DSR_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            DSR_ExportRepository objRepo = new DSR_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryListSearch(string ContainerNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_DestuffingList> LstDestuffing = new List<DSR_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(((Login)(Session["LoginUser"])).Uid, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<DSR_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }
        #endregion

        #region YardInvoice
        [HttpGet]
        public ActionResult CreateYardPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;


            /*DSR_ImportRepository objimp = new DSR_ImportRepository();
            objimp.GetImpPaymentPartyForPage("",0);
            if (objimp.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objimp.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;*/


            return PartialView();
        }

        [HttpGet]
        public JsonResult LoadYardCHAList(int Page)
        {
            DSR_ImportRepository objimp = new DSR_ImportRepository();
            objimp.GetImpCHAForYardPayment("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeYardCHA(string PartyCode)
        {
            DSR_ImportRepository objimp = new DSR_ImportRepository();
            objimp.GetImpCHAForYardPayment(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyListFCL(int Page)
        {
            DSR_ImportRepository objimp = new DSR_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeFCL(string PartyCode)
        {
            DSR_ImportRepository objimp = new DSR_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCheckOBLAmendment(int AppraisementId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetCheckOBLAmendment(AppraisementId);
           
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,
            List<DSR_PaymentSheetContainer> lstPaySheetContainer,string MovementType,string ExamType, int OTHours = 0, int PartyId = 0, int PayeeId = 0,
            int InvoiceId = 0, int isdirect = 0, int NoOfVehicles = 1, decimal Distance = 0, int PrivateMovement = 0,
            int InsuredParty = 0, int CWCMovement = 0, int Amendment = 0, int ReturnPort = 0, string SEZ = "")
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            DSR_ImportRepository objPpgRepo = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, MovementType, ExamType, InvoiceId, OTHours, PartyId,
                PayeeId, isdirect, NoOfVehicles, Distance, PrivateMovement, InsuredParty, CWCMovement, Amendment, ReturnPort, SEZ);
            var Output = (DSRInvoiceYard)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPYard";

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
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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



            return Json(Output, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditContPaymentSheet(String InvoiceObj, int IsDirect, string MovementType,string EmptyPort,int ExamType,string SEZ,  List<DSR_PaymentSheetContainer> lstPaySheetContainer, int Amendment = 0, int InsuredParty = 0)
        {
            try
            {
                //DSRInvoiceYard objForm;
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceYard>(InvoiceObj);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
                string CheckBoxXml = "";

                if (lstPaySheetContainer != null)
                {
                    CheckBoxXml = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                }


                if (invoiceData.lstPostPaymentContXML != null)
                {
                    invoiceData.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DSRPostPaymentContainer>>(invoiceData.lstPostPaymentContXML);
                    foreach (var item in invoiceData.lstPostPaymentCont)
                    {
                        item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                        item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                        item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                        item.LineNo = string.IsNullOrEmpty(item.LineNo) ? "-" : item.LineNo;
                        item.BOENo = string.IsNullOrEmpty(item.BOENo) ? "-" : item.BOENo;


                    }
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);

                }
                if (invoiceData.lstPostPaymentChrgXML != null)
                {
                    // ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                    invoiceData.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<DSRPostPaymentChrg>>(invoiceData.lstPostPaymentChrgXML);
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmountXML != null)
                {
                    // ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                    invoiceData.lstContWiseAmount = JsonConvert.DeserializeObject<List<DSRContainerWiseAmount>>(invoiceData.lstContWiseAmountXML);
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                }
                if (invoiceData.lstOperationCFSCodeWiseAmountXML != null)
                {

                    invoiceData.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<DSROperationCFSCodeWiseAmount>>(invoiceData.lstOperationCFSCodeWiseAmountXML);
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                    //  OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakupXML != null)
                {

                    invoiceData.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<DSRPostPaymentChargebreakupdate>>(invoiceData.lstPostPaymentChrgBreakupXML);
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                    //ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML,
                    ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, IsDirect, MovementType,  "IMPYard", CheckBoxXml, EmptyPort, ExamType,SEZ, Amendment,InsuredParty);

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


        /*
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult YardInvoicePrint(string InvoiceNo)
        {
            DSRInvoiceRepository objGPR = new DSRInvoiceRepository();
            objGPR.GetInvoiceDetailsForPrintByNo(InvoiceNo, "IMPYard");
            DSRInvoiceYard objGP = new DSRInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (DSRInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFYard(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFYard(DSRInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/YardInvoice" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span>");
            html.Append("<br />ASSESSMENT SHEET FCL");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");
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
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + objGP.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate)- Convert.ToDateTime(objGP.ArrivalDate)).TotalDays+1).ToString() + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (container.CargoType == 1 ? "Haz" : "Non-Haz") + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            html.Append("</tbody></table></td></tr>");

            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 12px;'>Shipping Line: " + objGP.ShippingLineName + " <br />");
            html.Append("Shipping No:  <br />");
            html.Append("OBL No:   &nbsp;&nbsp; ItemNo&nbsp;  BOE No&nbsp; : " + objGP.BOENo + "&nbsp;&nbsp;BOE Date: " + objGP.BOEDate + " <br />");
            html.Append("Importer:" + objGP.ImporterExporter + "   &nbsp;&nbsp; VALUE:" + objGP.lstPostPaymentCont.Sum(o => o.CIFValue).ToString() + "&nbsp;&nbsp;DUTY:" + objGP.lstPostPaymentCont.Sum(o => o.Duty).ToString() + "");
            html.Append("&nbsp;=&nbsp;" + (objGP.lstPostPaymentCont.Sum(o => o.CIFValue) + objGP.lstPostPaymentCont.Sum(o => o.Duty)).ToString() + "<br />");
            html.Append("CHA Name:&nbsp;" + objGP.CHAName + "<br />");
            html.Append("No Of Pkg:&nbsp;" + objGP.TotalNoOfPackages.ToString() + "&nbsp;Total Gross Wt.&nbsp;" + objGP.TotalGrossWt.ToString("0.00") + "<br />");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</table>");
            html.Append("</td></tr>");

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
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalAmt.ToString("0") + "</td>");
            html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            html.Append("Total Invoice (In Word) :");
            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='7'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");
            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/YardInvoice" + InvoiceId.ToString() + ".pdf";
        }
        */
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

        [HttpGet]
        public ActionResult ListOfImpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null,int Page=0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpInvoice(Module, InvoiceNo, InvoiceDate,Page);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfYardInvoice", obj);
        }

        [HttpGet]
        public ActionResult ListOfLoadMoreImpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return Json(obj,JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Re-assessment Yard Invoice
        [HttpGet]
        public ActionResult ReassessmentYardPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetAppraismentRequestForReassessment();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetReassessmentPaymentSheetContainer(int AppraisementId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetContainerForReassessmentPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetContainerReassessmentPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType,
            List<DSR_PaymentSheetContainer> lstPaySheetContainer, string MovementType, string ExamType, int OTHours = 0, int PartyId = 0, int PayeeId = 0,
            int InvoiceId = 0, int isdirect = 0, int NoOfVehicles = 1, decimal Distance = 0, int PrivateMovement = 0,
            int InsuredParty = 0, int CWCMovement = 0, int Amendment = 0, int ReturnPort = 0, string SEZ = "")
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            DSR_ImportRepository objPpgRepo = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetYardReassessmentPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, MovementType, ExamType, InvoiceId, OTHours, PartyId,
                PayeeId, isdirect, NoOfVehicles, Distance, PrivateMovement, InsuredParty, CWCMovement, Amendment, ReturnPort, SEZ);
            var Output = (DSRInvoiceYard)objPpgRepo.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPYard";

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
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditYardReassessmentPaymentSheet(String InvoiceObj, int IsDirect, string MovementType, string EmptyPort, int ExamType, List<DSR_PaymentSheetContainer> lstPaySheetContainer, string SEZ = "", int Amendment=0, int InsuredParty=0)
        {
            try
            {
                //DSRInvoiceYard objForm;
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceYard>(InvoiceObj);
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
                string CheckBoxXml = "";

                if (lstPaySheetContainer != null)
                {
                    CheckBoxXml = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                }


                if (invoiceData.lstPostPaymentContXML != null)
                {
                    invoiceData.lstPostPaymentCont = JsonConvert.DeserializeObject<List<DSRPostPaymentContainer>>(invoiceData.lstPostPaymentContXML);
                    foreach (var item in invoiceData.lstPostPaymentCont)
                    {
                        item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                        item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                        item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                        item.LineNo = string.IsNullOrEmpty(item.LineNo) ? "-" : item.LineNo;
                        item.BOENo = string.IsNullOrEmpty(item.BOENo) ? "-" : item.BOENo;


                    }
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);

                }
                if (invoiceData.lstPostPaymentChrgXML != null)
                {
                    // ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                    invoiceData.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<DSRPostPaymentChrg>>(invoiceData.lstPostPaymentChrgXML);
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                if (invoiceData.lstContWiseAmountXML != null)
                {
                    // ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                    invoiceData.lstContWiseAmount = JsonConvert.DeserializeObject<List<DSRContainerWiseAmount>>(invoiceData.lstContWiseAmountXML);
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                }
                if (invoiceData.lstOperationCFSCodeWiseAmountXML != null)
                {

                    invoiceData.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<DSROperationCFSCodeWiseAmount>>(invoiceData.lstOperationCFSCodeWiseAmountXML);
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                    //  OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstPostPaymentChrgBreakupXML != null)
                {

                    invoiceData.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<DSRPostPaymentChargebreakupdate>>(invoiceData.lstPostPaymentChrgBreakupXML);
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                    //ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditYardReassessmentInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML,
                    ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, IsDirect, MovementType, "IMPYard", CheckBoxXml, EmptyPort, ExamType,SEZ, Amendment,  InsuredParty);

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
        public ActionResult ListOfImpYardReassessmentInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpYardReassessmentInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfYardInvoice", obj);
        }
        #endregion

        #region Internal Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
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
            ObjIR.GetBOENoForInternalMovement();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.BOENoList = new SelectList((List<DSR_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
            }
            else
            {
                ViewBag.BOENoList = null;
            }



            ObjIR.ListOfInvernalMovementGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
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
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllInternalMovement();
            List<DSR_Internal_Movement> LstMovement = new List<DSR_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<DSR_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("ImpInternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult GetInternalMovementListByOBL(string OBLNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllInternalMovement(OBLNo);
            List<DSR_Internal_Movement> LstMovement = new List<DSR_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<DSR_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("ImpInternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Internal_Movement ObjInternalMovement = new DSR_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_Internal_Movement)ObjIR.DBResponse.Data;
                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }


                ObjIR.GetBOENoForInternalMovement();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.BOENoList = new SelectList((List<DSR_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
                }
                else
                {
                    ViewBag.BOENoList = null;
                }


                // ObjIR.GetLocationForInternalMovement();
                //  if (ObjIR.DBResponse.Data != null)
                //   {
                //       ViewBag.LocationNoList = new SelectList((List<DSR_Internal_Movement>)ObjIR.DBResponse.Data, "LocationId", "LocationName");
                //  }
                //  else
                //  {
                //     ViewBag.LocationNoList = null;
                // }

            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovement(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Internal_Movement ObjInternalMovement = new DSR_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_Internal_Movement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetBOENoDetails(int DestuffingEntryDtlId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetBOENoDetForMovement(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int DestuffingId, string OBLNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int InvoiceId = 0)
        {

            DSR_ImportRepository objChrgRepo = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(DestuffingId, OBLNo, MovementDate, InvoiceType, DestLocationIdiceId, InvoiceId);

            //var Output = (PostPaymentSheet)objChrgRepo.DBResponse.Data;
            //Output.TotalAmt = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);// Output.lstPostPaymentChrg.Sum(o => o.Amount);
            //Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
            //Output.TotalTaxable = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Taxable);//Output.lstPostPaymentChrg.Sum(o => o.Taxable);
            //Output.TotalCGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.CGSTAmt);
            //Output.TotalSGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.SGSTAmt);
            //Output.TotalIGST = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.IGSTAmt);
            //Output.CWCTotal = Output.lstPostPaymentChrg.Where(o => o.ChargeType == "CWC").Sum(o => o.Amount);
            //Output.HTTotal = 0;// Output.lstPostPaymentChrg.Where(o => o.ChargeType == "HT").Sum(o => o.Total);
            //Output.CWCTDS = 0;
            //Output.HTTDS = 0;
            //Output.TDS = 0;
            //Output.AllTotal = (Output.lstPostPaymentChrg.Sum(o => o.Total)) - Output.TDS;
            //Output.RoundUp = 0;
            //Output.InvoiceAmt = Output.AllTotal;
            //return Json(Output);

            var Output = (DSRInvoiceGodown)objChrgRepo.DBResponse.Data;

            Output.InvoiceDate = MovementDate;
            Output.Module = "IMPMovement";

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
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => o.Discount);
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => o.Total);
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
                Output.RoundUp = 0;
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => o.Total);

            });



            return Json(Output, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult MovementInvoicePrint(string InvoiceNo)
        {
            DSR_InvoiceRepository objGPR = new DSR_InvoiceRepository();
            objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "IMPMovement");
            DSRInvoiceYard objGP = new DSRInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (DSRInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceMovement(DSRInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/MovementInvoice" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span>");
            html.Append("<br />Tax Invoice");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");
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
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + objGP.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            //   html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Week</th>");

            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                // html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.CartingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DestuffingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                //     html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //   html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                //  html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");

                // html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (container.CargoType == 1 ? "Haz" : "Non-Haz") + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            html.Append("</tbody></table></td></tr>");

            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 12px;'>Shipping Line: " + objGP.ShippingLineName + " <br />");
            html.Append("Shipping No:  <br />");
            html.Append("OBL No:   &nbsp;&nbsp; ItemNo&nbsp;  BOE No&nbsp; : " + objGP.BOENo + "&nbsp;&nbsp;BOE Date: " + objGP.BOEDate + " <br />");
            html.Append("Importer:" + objGP.ImporterExporter + "   &nbsp;&nbsp; VALUE:" + objGP.lstPostPaymentCont.Sum(o => o.CIFValue).ToString() + "&nbsp;&nbsp;DUTY:" + objGP.lstPostPaymentCont.Sum(o => o.Duty).ToString() + "");
            html.Append("&nbsp;=&nbsp;" + (objGP.lstPostPaymentCont.Sum(o => o.CIFValue) + objGP.lstPostPaymentCont.Sum(o => o.Duty)).ToString() + "<br />");
            html.Append("CHA Name:&nbsp;" + objGP.CHAName + "<br />");
            html.Append("No Of Pkg:&nbsp;" + objGP.TotalNoOfPackages.ToString() + "&nbsp;Total Gross Wt.&nbsp;" + objGP.TotalGrossWt.ToString("0.00") + "<br />");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</table>");
            html.Append("</td></tr>");

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
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");



            html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;width:50%;'>Total :</th>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>");
            html.Append("Total Invoice (In Word) :");







            //   html.Append("</tbody>");
            //   html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            //   html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            //  html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            //  html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            //  html.Append("Total Invoice (In Word) :");
            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='6'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");




            //html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            //html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            //html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalAmt.ToString("0.00") + "</td>");
            //html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            //html.Append("Total Invoice (In Word) :");
            //html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            //html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='7'>Amount of Tax Subject of Reverse :");
            //html.Append("0</th>");
            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.ShippingLineName.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/MovementInvoice" + InvoiceId.ToString() + ".pdf";
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalPaymentSheet(DSR_Internal_Movement objForm)
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

                //var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //string ContWiseCharg = "";

                //foreach (var item in invoiceData.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                //}

                //if (invoiceData.lstPostPaymentCont != null)
                //{
                //    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                //}
                //if (invoiceData.lstPostPaymentChrg != null)
                //{
                //    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                //}
                //if (invoiceData.lstContWiseAmount != null)
                //{
                //    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                //}
                //if (invoiceData.lstCfsCodewiseRateHT != null)
                //{GetContainerPaymentSheet
                //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                //}
                //DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");
                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //   int BranchId = Convert.ToInt32(Session["BranchId"]);


                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoiceMovement(objForm, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPMovement");

                //   invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                //   invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                //  objChargeMaster.DBResponse.Data = invoiceData;
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovement(DSR_Internal_Movement ObjInternalMovement)
        {
            
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.AddEditImpInternalMovement(ObjInternalMovement);
            return Json(ObjIR.DBResponse);
          

        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DelInternalMovement(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
        }


        //[HttpGet]
        //public JsonResult GetGodownWiseLocation(int GodownId)
        //{
        //    DSR_ImportRepository objIR = new DSR_ImportRepository();
        //    objIR.GodownWiseLocation(GodownId);
        //    object objLctn = null;
        //    if (objIR.DBResponse.Data != null)
        //        objLctn = objIR.DBResponse.Data;
        //    return Json(objLctn, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult GetGodownWiseLocation(int GodownId)
        {
            DSR_ImportRepository objRepo = new DSR_ImportRepository();
            objRepo.GetGodownLocationById(GodownId);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult GetOBLWiseGodownList(int DestuffingEntryDtlId, string OBLNo,string OBLDate)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetOBLWiseGodownList(DestuffingEntryDtlId, OBLNo, OBLDate);            
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Internal Movement Application        

        [HttpGet]
        public ActionResult CreateInternalMovementApp()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
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
                ViewBag.ListOfGodown = new SelectList((List<Models.DSRGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
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
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetBOENoForInternalMovementApp();
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInternalMovementListApp()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllInternalMovementApp(0);
            List<DSR_Internal_Movement> LstMovement = new List<DSR_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<DSR_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementListApp", LstMovement);
        }
        [HttpGet]
        public JsonResult LoadMoreInternalMovementApp(int Page)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_Internal_Movement> LstInternalMovement = new List<DSR_Internal_Movement>();
            ObjIR.GetAllInternalMovementApp(Page);
            if (ObjIR.DBResponse.Data != null)
            {
                LstInternalMovement = (List<DSR_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInternalMovementApp(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Internal_Movement ObjInternalMovement = new DSR_Internal_Movement();
            ObjIR.GetInternalMovementApp(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_Internal_Movement)ObjIR.DBResponse.Data;
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
        public ActionResult ViewInternalMovementApp(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Internal_Movement ObjInternalMovement = new DSR_Internal_Movement();
            ObjIR.GetInternalMovementApp(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_Internal_Movement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetBOENoDetailsApp(int DestuffingEntryDtlId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetBOENoDetForMovementApp(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalMovementApp(DSR_Internal_Movement objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoiceMovementApp(objForm, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPMovement");
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }
        //[HttpGet]
        //public JsonResult GetGodownWiseLocation(int GodownId)
        //{
        //    DSR_ImportRepository objIR = new DSR_ImportRepository();
        //    objIR.GodownWiseLocation(GodownId);
        //    object objLctn = null;
        //    if (objIR.DBResponse.Data != null)
        //        objLctn = objIR.DBResponse.Data;
        //    return Json(objLctn, JsonRequestBehavior.AllowGet);
        //}

        #endregion


        #region Delivery Application

        [HttpGet]
        public ActionResult CreateDeliveryApplication()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            BondRepository ObjBR = new BondRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }


            ObjIR.GetDestuffEntryNo(((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                ViewBag.DestuffingEntryNoList = (List<DestuffingEntryNoList>)ObjIR.DBResponse.Data;

            else
                ViewBag.DestuffingEntryNoList = null;
            ObjIR.ListOfChaForPage("", 0);

            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstCHA = Jobject["lstCHA"];
                ViewBag.CHAState = Jobject["State"];
            }
            ObjBR.ListOfImporter();
            if (ObjBR.DBResponse.Data != null)
                ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
            else
                ViewBag.ImporterList = null;
            return PartialView();
        }




        [HttpGet]
        public JsonResult ListOfIssueParty()
        {
            DSR_ImportRepository objcash = new DSR_ImportRepository();
            objcash.ListOfIssueParty();
            //List<DSR_ContainerStuffing> objImp = new List<DSR_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<DSRDeliveryOrdDtl>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { IssueBy = item.IssuedBy });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListOfCargoParty()
        {
            DSR_ImportRepository objcash = new DSR_ImportRepository();
            objcash.ListOfIssueParty();
            //List<DSR_ContainerStuffing> objImp = new List<DSR_ContainerStuffing>();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<DSRDeliveryOrdDtl>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { IssueBy = item.IssuedBy });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditDeliveryApplication(int DeliveryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            BondRepository ObjBR = new BondRepository();
            DSR_DeliverApplication ObjDelivery = new DSR_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (DSR_DeliverApplication)ObjIR.DBResponse.Data;
                ObjIR.ListOfCHA();
                if (ObjIR.DBResponse.Data != null)
                    ViewBag.CHAList = new SelectList((IList<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                else
                    ViewBag.CHAList = null;
                ObjBR.ListOfImporter();
                if (ObjBR.DBResponse.Data != null)
                    ViewBag.ImporterList = new SelectList((IList<CwcExim.Areas.Bond.Models.Importer>)ObjBR.DBResponse.Data, "ImporterId", "ImporterName");
                else
                    ViewBag.ImporterList = null;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ViewDeliveryApplication(int DeliveryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_DeliverApplication ObjDelivery = new DSR_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (DSR_DeliverApplication)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryApplication()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSRDeliveryApplicationList> LstDelivery = new List<DSRDeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DSRDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDeliveryApp(int Page)
        {
            DSR_ImportRepository ObjCR = new DSR_ImportRepository();
            List<DSRDeliveryApplicationList> LstJO = new List<DSRDeliveryApplicationList>();
            ObjCR.GetAllDeliveryApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DSRDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryApplication(DSR_DeliverApplication ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DSRDeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DSRDeliveryOrdDtl>>(ObjDelivery.DeliveryOrdDtlXml);
                    DeliveryOrdXml = Utility.CreateXML(ObjDelivery.LstDeliveryordDtl);
                }


                ObjIR.AddEditDeliveryApplication(ObjDelivery, DeliveryXml, DeliveryOrdXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetBOEDetForDeliveryApp(int DestuffingEntryDtlId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId,string OBLNo="")
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId, OBLNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCIFFromOOC(String BOE, String BOEDT)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetCIFFromOOCDelivery(BOE, BOEDT);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Delivery Payment Sheet
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            //objImport.GetPaymentParty();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;

            objImport.GetImpPaymentPartyForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            return PartialView();
        }


        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }



        /*
        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryCont(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetDeliveryContForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }
        */

        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryBOE(int AppraisementId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetBOEForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.BOEList = null;

            return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);

           
        }

        [HttpPost]
        public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName, List<DSRPaymentSheetBOE> lstPaySheetBOE, int OTHours = 0, int InvoiceId = 0, string SEZ="")
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }


            DSR_ImportRepository objChrgRepo = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetDeliveryPaymentSheet_DSR(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, SEZ, InvoiceId, OTHours);

            var Output = (DSRInvoiceGodown)objChrgRepo.DBResponse.Data;
            DSRtentativeinvoice.InvoiceObj = (DSRInvoiceGodown)objChrgRepo.DBResponse.Data;
            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPDeli";

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
                    Output.BOEDate += item.BOEDate;
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
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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

                Output.TotalNoOfPackages = Output.lstPrePaymentCont.FirstOrDefault().NoOfPackages;
                Output.TotalGrossWt = Output.lstPrePaymentCont.FirstOrDefault().GrossWeight;
                Output.TotalWtPerUnit = Output.lstPrePaymentCont.FirstOrDefault().WtPerPack;
                Output.TotalSpaceOccupied = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupied;
                Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                Output.TotalValueOfCargo = Output.lstPrePaymentCont.FirstOrDefault().CIFValue
                    + Output.lstPrePaymentCont.FirstOrDefault().Duty;


                //Output.TotalNoOfPackages = Output.lstPrePaymentCont.Sum(o => o.NoOfPackages);
                //Output.TotalGrossWt = Output.lstPrePaymentCont.Sum(o => o.GrossWeight);
                //Output.TotalWtPerUnit = Output.lstPrePaymentCont.Sum(o => o.WtPerPack);
                //Output.TotalSpaceOccupied = Output.lstPrePaymentCont.Sum(o => o.SpaceOccupied);
                //Output.TotalSpaceOccupiedUnit = Output.lstPrePaymentCont.FirstOrDefault().SpaceOccupiedUnit;
                //Output.TotalValueOfCargo = Output.lstPrePaymentCont.Sum(o => o.CIFValue)
                //    + Output.lstPrePaymentCont.Sum(o => o.Duty);


                Output.TotalAmt = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Total : 0));
                Output.TotalDiscount = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Discount : 0));
                Output.TotalTaxable = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Total : 0));
                Output.TotalCGST = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.CGSTAmt : 0));
                Output.TotalSGST = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.SGSTAmt : 0));
                Output.TotalIGST = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.IGSTAmt : 0));
                Output.CWCTotal = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Total : 0));
                Output.HTTotal = 0;
                Output.CWCTDS = 0;
                Output.HTTDS = 0;
                Output.CWCTDSPer = 0;
                Output.HTTDSPer = 0;
                Output.TDS = 0;
                Output.TDSCol = 0;
                Output.AllTotal = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Total : 0));
                Output.RoundUp = 0;
                Output.InvoiceAmt = Output.lstPostPaymentChrg.Sum(o => (o.ChargeType == "CWC" ? o.Total : 0));

            });
            DSRtentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryPaymentSheet(FormCollection objForm)
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

                //var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //string ContWiseCharg = "";

                //foreach (var item in invoiceData.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                //}

                //if (invoiceData.lstPostPaymentCont != null)
                //{
                //    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                //}
                //if (invoiceData.lstPostPaymentChrg != null)
                //{
                //    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                //}
                //if (invoiceData.lstContWiseAmount != null)
                //{
                //    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                //}
                //if (invoiceData.lstCfsCodewiseRateHT != null)
                //{
                //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                //}
                //DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");
                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //   int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "", CargoXML = "";
                string ChargesBreakupXML = "";
                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                foreach (var item in invoiceData.lstInvoiceCargo)
                {
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? "1900-01-01" : item.StuffingDate;
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? "1900-01-01" : item.DestuffingDate;
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? "1900-01-01" : item.CartingDate;
                    //  item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstInvoiceCargo);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoiceGodown(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);

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



        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GodownInvoicePrint(string InvoiceNo)
        {
            DSR_InvoiceRepository objGPR = new DSR_InvoiceRepository();
            objGPR.GetInvoiceDetailsForGodownPrintByNo(InvoiceNo, "IMPDeli");
            DSRInvoiceYard objGP = new DSRInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (DSRInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceGodown(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceGodown(DSRInvoiceYard objGP, int InvoiceId)
        {
            // string html = "";
            CurrencyToWordINR ctwObj = new CurrencyToWordINR();
            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
            if (!System.IO.Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(location))
            {
                System.IO.File.Delete(location);
            }
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + objGP.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span>");
            html.Append("<br />ASSESSMENT SHEET LCL");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + objGP.CompGST + "</label></span></td></tr>");
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
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + objGP.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + objGP.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>ICD Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Carting</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Destuffing</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            // html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            //  html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            //   html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Week</th>");

            html.Append("</tr></thead><tbody>");
            int i = 1;
            foreach (var container in objGP.lstPostPaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.CartingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DestuffingDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                //    html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.GrossWt.ToString() + "</td>");
                //     html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + objGP.DeliveryDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + ((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1).ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + Math.Ceiling(((((Convert.ToDateTime(objGP.DeliveryDate) - Convert.ToDateTime(objGP.ArrivalDate)).TotalDays + 1)) / 7)).ToString() + "</td>");

                // html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (container.CargoType == 1 ? "Haz" : "Non-Haz") + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            html.Append("</tbody></table></td></tr>");

            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr>");
            html.Append("<td style='font-size: 12px;'>Shipping Line: " + objGP.ShippingLineName + " <br />");
            html.Append("Shipping No:  <br />");
            html.Append("OBL No:   &nbsp;&nbsp; ItemNo&nbsp;  BOE No&nbsp; : " + objGP.BOENo + "&nbsp;&nbsp;BOE Date: " + objGP.BOEDate + " <br />");
            html.Append("Importer:" + objGP.ImporterExporter + "   &nbsp;&nbsp; VALUE:" + objGP.lstPostPaymentCont.Sum(o => o.CIFValue).ToString() + "&nbsp;&nbsp;DUTY:" + objGP.lstPostPaymentCont.Sum(o => o.Duty).ToString() + "");
            html.Append("&nbsp;=&nbsp;" + (objGP.lstPostPaymentCont.Sum(o => o.CIFValue) + objGP.lstPostPaymentCont.Sum(o => o.Duty)).ToString() + "<br />");
            html.Append("CHA Name:&nbsp;" + objGP.CHAName + "<br />");
            html.Append("No Of Pkg:&nbsp;" + objGP.TotalNoOfPackages.ToString() + "&nbsp;Total Gross Wt.&nbsp;" + objGP.TotalGrossWt.ToString("0.00") + "<br />");
            html.Append("</td>");
            html.Append("</tr>");
            html.Append("</table>");
            html.Append("</td></tr>");

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
            foreach (var charge in objGP.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Clause + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Rate.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Taxable.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.CGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.SGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTPer.ToString("0.00") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.IGSTAmt.ToString("0.00") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center;'>" + charge.Total.ToString("0.00") + "</td></tr>");
                i = i + 1;
            }

            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'> ");
            html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;width:50%;'>Total :</th>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;width:10%;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='6'>");
            html.Append("Total Invoice (In Word) :");







            //   html.Append("</tbody>");
            //   html.Append("</table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='10'> ");
            //   html.Append("<tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'></td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Taxable).ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalCGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalSGST.ToString("0.00") + "</td>");
            //   html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.TotalIGST.ToString("0.00") + "</td>");
            //  html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: right; padding: 5px;'>" + objGP.lstPostPaymentChrg.Sum(o => o.Total).ToString("0.00") + "</td>");
            //  html.Append("</tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='7'>");
            //  html.Append("Total Invoice (In Word) :");
            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='6'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");

            //  html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            //   html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='7'>Amount of Tax Subject of Reverse :");
            //  html.Append("0</th>");
            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
        }

        [HttpGet]
        public ActionResult ListOfImpReAssessmentInvoice(int IsReAssess, string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpReAssessmentInvoice(0, IsReAssess, InvoiceNo, InvoiceDate);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfImpReAssessmentInvoice", obj);
        }

        [HttpGet]
        public JsonResult LoadMoreImpReAssessmentInvoice(int Page, int IsReAssess, string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpReAssessmentInvoice(Page, IsReAssess, InvoiceNo, InvoiceDate);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FCL To LCL Conversion

        [HttpGet]
        public ActionResult AddFCLtoLCLConversion()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfShippingLinePartyCode("", 0);

            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult ListOfContainerFCLtoLCL()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfContainerFCLtoLCL();
            List<DSRFCLtoLCLContainerList> objImp = new List<DSRFCLtoLCLContainerList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRFCLtoLCLContainerList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPartyPdaForFCLtoLCL()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();


            objImport.GetPartyPdaForFCLtoLCL();
            List<DSRFCLtoLCLForwarderList> objImp = new List<DSRFCLtoLCLForwarderList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRFCLtoLCLForwarderList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSLA()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfShippingLinePartyCode("", 0);

            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            return Json(objImport, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPartyPdaDetailsForFCLtoLCL(string Size, int PartyPdaId, int ContainerClassId, String CFSCode)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetPartyPdaDetailsForFCLtoLCL(Size, PartyPdaId, ContainerClassId, CFSCode);
            DSRFCLtoLCLConversion objImp = new DSRFCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (DSRFCLtoLCLConversion)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCargoData(String CFSCode)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetCargoForFCLtoLCL(CFSCode);
            DSRFCLtoLCLConversion objImp = new DSRFCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (DSRFCLtoLCLConversion)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddFCLtoLCLConversion(DSRFCLtoLCLConversion objFCLLCL)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                //if (ModelState.IsValid)
                //{
                string ChargesBreakupXML = "";
                if (objFCLLCL.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(objFCLLCL.lstPostPaymentChrgBreakup);
                }

                DSR_ImportRepository objImport = new DSR_ImportRepository();
                objImport.AddFCLtoLCLConversion(objFCLLCL, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                ModelState.Clear();
                return Json(objImport.DBResponse);
                //}
                //else
                //{
                //    var Err = new { Status = -1, Message = "Error" };
                //    return Json(Err);
                //}
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }


        [HttpGet]
        public ActionResult GetListOfFCLToLCLConversionDtl()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetListOfFCLToLCLConversionDtl();
            List<DSRFCLtoLCLConversion> objImp = new List<DSRFCLtoLCLConversion>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRFCLtoLCLConversion>)objImport.DBResponse.Data;
            return PartialView("ListOfFCLtoLCLConversion", objImp);
        }

        [HttpGet]
        public ActionResult ViewFCLtoLCLConversionbyId(int FCLtoLCLConversionId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ViewFCLtoLCLConversionbyId(FCLtoLCLConversionId);
            DSRFCLtoLCLConversion objImp = new DSRFCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (DSRFCLtoLCLConversion)objImport.DBResponse.Data;
            return PartialView("ViewFCLtoLCLConversion", objImp);
        }
        [HttpGet]
        public ActionResult EditFCLtoLCLConversionbyId(int FCLtoLCLConversionId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ViewFCLtoLCLConversionbyId(FCLtoLCLConversionId);
            DSRFCLtoLCLConversion objImp = new DSRFCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (DSRFCLtoLCLConversion)objImport.DBResponse.Data;
            return PartialView("EditFCLtoLCLConversion", objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult FCLToLCLConversionInvoicePrint(int InvoiceId)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetFCLToLCLConversionInvoiceDtlForPrint(InvoiceId);
            DSRInvoiceFCLToLCLConversion objFCLLCL = new DSRInvoiceFCLToLCLConversion();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objFCLLCL = (DSRInvoiceFCLToLCLConversion)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFForFCLToLCLConversion(objFCLLCL, InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        private string GeneratingPDFForFCLToLCLConversion(DSRInvoiceFCLToLCLConversion objSC, int InvoiceId)
        {
            string html = "";

            var location = Server.MapPath("~/Docs/") + Session.SessionID + "/FCLToLCLConversion" + InvoiceId.ToString() + ".pdf";
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
                "<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>FCL To LCL Conversion</span></td></tr>" +
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
                "<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>To Date</th>" +
                "<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th></tr></thead><tbody>";
            int i = 1;
            foreach (var container in objSC.LstContainersFCLToLCLConversion)
            {
                html = html + "<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ContainerNo + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CfsCode + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.Size + "</td>" +
                "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.FromDate + "</td>" +
                   "<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.ToDate + "</td>" +
                "<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + container.CargoType + "</td></tr>";
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
            foreach (var charge in objSC.LstChargesFCLToLCLConversion)
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
            return "/Docs/" + Session.SessionID + "/FCLToLCLConversion" + InvoiceId.ToString() + ".pdf";
        }

        [HttpGet]
        public ActionResult DeleteFCLtoLCLConversion(int FCLtoLCLConversionId)
        {
            DSR_ImportRepository objImp = new DSR_ImportRepository();
            objImp.DeleteFCLtoLCLConversion(FCLtoLCLConversionId, Convert.ToInt32(Session["BranchId"]));
            return Json(objImp.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Form One

        [HttpGet]
        public ActionResult CreateFormOne()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddFormOne()
        {
            DSR_FormOneModel objFormOne = new DSR_FormOneModel();
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {
                //  var Jobjj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                //  var Jobjectt = Newtonsoft.Json.Linq.JObject.Parse(Jobjj);
                ViewBag.RightsList = JsonConvert.SerializeObject(objIR.DBResponse.Data);
                // ViewBag.MenuRights = Jobjectt["lstMenu"];
            }
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.ListOfShippingLineForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;
            objImport.ListOfPODForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;
            objImport.ListOfCHAForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;
            objImport.ListOfImporterForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;
            objImport.ListOfCommodityForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.DSR_Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult GetFormOneList(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<DSR_FormOneModel> lstFormOne = new List<DSR_FormOneModel>();
                DSR_ImportRepository objImportRepo = new DSR_ImportRepository();
                objImportRepo.GetFormOne();
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<DSR_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<DSR_FormOneModel> lstFormOne = new List<DSR_FormOneModel>();
                DSR_ImportRepository objImportRepo = new DSR_ImportRepository();
                objImportRepo.GetFormOneByContainer(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<DSR_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOne(DSR_FormOneModel objFormOne)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {
                    // objFormOne.FormOneDetailsJS.Replace("\"DateOfLanding: \":\"\"", "\"DateOfLanding\":\"null\"");
                    objFormOne.lstFormOneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSR_FormOneDetail>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetail.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetail);
                    DSR_ImportRepository objImport = new DSR_ImportRepository();
                    objImport.AddEditFormOne(objFormOne, BranchId, XML, ((Login)(Session["LoginUser"])).Uid);
                    ModelState.Clear();
                    return Json(objImport.DBResponse);
                }
                else
                {
                    var Err = new { Status = -1, Message = "Error" };
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
        public ActionResult EditFormOne(int FormOneId)
        {
            DSR_FormOneModel objFormOne = new DSR_FormOneModel();
            DSR_ImportRepository objImport = new DSR_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (DSR_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);

            objImport.ListOfShippingLineForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstShippingLine = (List<ShippingLine>)objImport.DBResponse.Data;

            objImport.ListOfPODForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstPOD = (List<PortOfDischarge>)objImport.DBResponse.Data;

            objImport.ListOfCHAForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCHA = (List<CHA>)objImport.DBResponse.Data;

            objImport.ListOfImporterForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstImporter = (List<Importer>)objImport.DBResponse.Data;

            objImport.ListOfCommodityForm();
            if (objImport.DBResponse.Data != null)
                objFormOne.lstCommodity = (List<Import.Models.DSR_Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult ViewFormOne(int FormOneId)
        {
            DSR_FormOneModel objFormOne = new DSR_FormOneModel();
            DSR_ImportRepository objImport = new DSR_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (DSR_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);
            return PartialView(objFormOne);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOne(int FormOneId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOne(FormOneId);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public JsonResult PrintFormOne(int FormOneId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            if (FormOneId > 0)
                objImport.FormOnePrint(FormOneId);
            var model = (Kol_FormOnePrintModel)objImport.DBResponse.Data;
            var printableData = (IList<Kol_FormOnePrintDetailModel>)model.lstFormOnePrintDetail;

            var fileName = model.FormOneNo + ".pdf";
            string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
            string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

            if (!Directory.Exists(PdfDirectory))
                Directory.CreateDirectory(PdfDirectory);

            var cPdf = new CustomPdfGenerator();
            cPdf.Generate(PdfDirectory + "/" + fileName, model.ShippingLineNo, printableData);

            return Json(new { Status = 1, FileUrl = "../../Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GenerateFormOne(FormCollection fc)
        {
            string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"];
            try
            {
                var pages = new string[1];
                var fileName = fc["FormOne"].ToString() + ".pdf";
                string UID = ((Login)(Session["LoginUser"])).Uid.ToString();
                string PdfDirectory = Server.MapPath("~/Uploads/FormOne/") + UID;

                if (!Directory.Exists(PdfDirectory))
                    Directory.CreateDirectory(PdfDirectory);
                using (var rh = new ReportingHelper(PdfPageSize.A4Landscape))
                {
                    rh.GeneratePDF(PdfDirectory + "/" + fileName, fc["Page1"].ToString());
                }
                return Json(new { Status = 1, Message = "/Uploads/FormOne/" + UID + "/" + fileName }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Message = "" }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion


        #region Issue Slip

        [HttpGet]
        public ActionResult CreateIssueSlip()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_Issueslip ObjIssueSlip = new DSR_Issueslip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetInvoiceNoForIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<DSR_Issueslip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            }
            else
            {
                ViewBag.InvoiceNoList = null;
            }
            return PartialView("CreateIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public JsonResult GetInvcDetForIssueSlip(int InvoiceId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR = new DSR_ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssueSlipList()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_Issueslip> LstIssueSlip = new List<DSR_Issueslip>();
            ObjIR.GetAllIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<DSR_Issueslip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }

        [HttpGet]
        public ActionResult EditIssueSlip(int IssueSlipId)
        {
            DSR_Issueslip ObjIssueSlip = new DSR_Issueslip();
            if (IssueSlipId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (DSR_Issueslip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewIssueSlip(int IssueSlipId)
        {
            DSR_Issueslip ObjIssueSlip = new DSR_Issueslip();
            if (IssueSlipId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (DSR_Issueslip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewIssueSlip", ObjIssueSlip);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteIssueSlip(int IssueSlipId)
        {
            if (IssueSlipId > 0)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIR.DelIssueSlip(IssueSlipId);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditIssueSlip(DSR_Issueslip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIssueSlip.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditIssueSlip(ObjIssueSlip);
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
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
        public JsonResult PrintIssueSlip(int IssueSlipId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetIssueSlipForPreview(IssueSlipId);
            if (ObjIR.DBResponse.Data != null)
            {
                DSR_Issueslip ObjIssueSlip = new DSR_Issueslip();
                ObjIssueSlip = (DSR_Issueslip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForIssueSlip(ObjIssueSlip, IssueSlipId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForIssueSlip(DSR_Issueslip ObjIssueSlip, int IssueSlipId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", BOEDate = "", BOENo = "", Vessel = "", CHA = "", Importer = "", ShippingLine = "",
            CargoDescription = "", MarksNo = "", Weight = "", LineNo = "", Rotation = "", ArrivalDate = "", DestuffingDate = "", Location = "";
            int Count = 0;
            // decimal MarksNo = 0, Weight=0;
            string Html = "";
            string CompanyAddress = "";
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany = (List<Company>)ObjCR.DBResponse.Data;
                CompanyAddress = LstCompany[0].CompanyAddress;
            }
            ObjIssueSlip.LstIssueSlipRpt.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
                Size = (Size == "" ? (item.Size) : (Size + "<br/>" + item.Size));
                // BOEDate = (BOEDate == "" ? (item.BOEDate) : (BOEDate + "<br/>" + item.BOEDate));
                //  BOENo = (BOENo == "" ? (item.BOENo) : (BOENo + "<br/>" + item.BOENo));
                //  Vessel = (Vessel == "" ? (item.Vessel) : (Vessel + "<br/>" + item.Vessel));
                // CHA = (CHA == "" ? (item.CHA) : (CHA + "<br/>" + item.CHA));
                // Importer = (Importer == "" ? (item.Importer) : (Importer + "<br/>" + item.Importer));
                // CargoDescription = (CargoDescription == "" ? (item.CargoDescription) : (CargoDescription + "<br/>" + item.CargoDescription));
                MarksNo = (MarksNo == "" ? (item.MarksNo) : (MarksNo + "<br/>" + item.MarksNo));
                // Weight = (Weight == "" ? (item.Weight) : (Weight + "<br/>" + item.Weight));
                //  LineNo = (LineNo == "" ? (item.LineNo) : (LineNo + "<br/>" + item.LineNo));
                //  Rotation = (Rotation == "" ? (item.Rotation) : (Rotation + "<br/>" + item.Rotation));
                // ArrivalDate = (ArrivalDate == "" ? (item.ArrivalDate) : (ArrivalDate + "<br/>" + item.ArrivalDate));
                // DestuffingDate = (DestuffingDate == "" ? (item.DestuffingDate) : (DestuffingDate + "<br/>" + item.DestuffingDate));
                // Location = (Location == "" ? (item.Location) : (Location + "<br/>" + item.Location));
                // ShippingLine = (ShippingLine == "" ? (item.ShippingLine) : (ShippingLine + "<br/>" + item.ShippingLine));
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CHA = x.CHA }).Distinct().ToList().ForEach(item =>
            {
                if (CHA == "")
                    CHA = item.CHA;
                else
                    CHA += "<br/>" + item.CHA;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Weight = x.Weight }).Distinct().ToList().ForEach(item =>
            {
                if (Weight == "" || Weight == "0.0000")
                    Weight = item.Weight;
                else
                    Weight += "<br/>" + item.Weight;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { LineNo = x.LineNo }).Distinct().ToList().ForEach(item =>
            {
                if (LineNo == "")
                    LineNo = item.LineNo;
                else
                    LineNo += "<br/>" + item.LineNo;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Rotation = x.Rotation }).Distinct().ToList().ForEach(item =>
            {
                if (Rotation == "")
                    Rotation = item.Rotation;
                else
                    Rotation += "<br/>" + item.Rotation;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Location = x.Location }).Distinct().ToList().ForEach(item =>
            {
                if (Location == "")
                    Location = item.Location;
                else
                    Location += "<br/>" + item.Location;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Importer = x.Importer }).Distinct().ToList().ForEach(item =>
            {
                if (Importer == "")
                    Importer = item.Importer;
                else
                    Importer += "<br/>" + item.Importer;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { CargoDescription = x.CargoDescription }).Distinct().ToList().ForEach(item =>
            {
                if (CargoDescription == "")
                    CargoDescription = item.CargoDescription;
                else
                    CargoDescription += "<br/>" + item.CargoDescription;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ArrivalDate = x.ArrivalDate }).Distinct().ToList().ForEach(item =>
            {
                if (ArrivalDate == "")
                    ArrivalDate = item.ArrivalDate;
                else
                    ArrivalDate += "<br/>" + item.ArrivalDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { DestuffingDate = x.DestuffingDate }).Distinct().ToList().ForEach(item =>
            {
                if (DestuffingDate == "")
                    DestuffingDate = item.DestuffingDate;
                else
                    DestuffingDate += "<br/>" + item.DestuffingDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { ShippingLine = x.ShippingLine }).Distinct().ToList().ForEach(item =>
            {
                if (ShippingLine == "")
                    ShippingLine = item.ShippingLine;
                else
                    ShippingLine += "<br/>" + item.ShippingLine;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { Vessel = x.Vessel }).Distinct().ToList().ForEach(item =>
            {
                if (Vessel == "")
                    Vessel = item.Vessel;
                else
                    Vessel += "<br/>" + item.Vessel;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOEDate = x.BOEDate }).Distinct().ToList().ForEach(item =>
            {
                if (BOEDate == "")
                    BOEDate = item.BOEDate;
                else
                    BOEDate += "<br/>" + item.BOEDate;
            });
            ObjIssueSlip.LstIssueSlipRpt.Select(x => new { BOENo = x.BOENo }).Distinct().ToList().ForEach(item =>
            {
                if (BOENo == "")
                    BOENo = item.BOENo;
                else
                    BOENo += "<br/>" + item.BOENo;
            });
            if (!Directory.Exists(Server.MapPath("~/Docs/") + Session.SessionID))
            {
                Directory.CreateDirectory(Server.MapPath("~/Docs/") + Session.SessionID);
            }
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }

            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>ICD Dashrath</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>C.H.A Name.</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + Weight + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No <span>" +Location+"</span> Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'><span style='border-bottom:1px solid #000;'></span></td><td style='text-align:right;'><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>Issue Slip Of Container Freight Station.</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/></td></tr><tr><td colspan='2' style='text-align:left;'><span style='border-bottom:1px solid #000;'></span><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry No.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + BOENo + "</td><td style='border:1px solid #000;padding:5px;'>" + BOEDate + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td><span></span></td><td><br/><br/></td></tr></tbody></table></td></tr></tbody></table>";


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
        }

        #endregion

        #region Cargo Seize

        public ActionResult CargoSeize(int Id = 0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfOBLNo();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfOBLNo = objER.DBResponse.Data;
            }

            DSRCargoSeize objCargoSeize = new DSRCargoSeize();

            if (Id > 0)
            {
                DSR_ImportRepository rep = new DSR_ImportRepository();
                rep.GetCargoSeizeById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCargoSeize = (DSRCargoSeize)rep.DBResponse.Data;
                }
            }

            return PartialView(objCargoSeize);
        }


        [HttpGet]
        public ActionResult GetOBLDetails(int DestuffingEntryDtlId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetOBLDetails(DestuffingEntryDtlId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCargoSeize(DSRCargoSeize objCargoSeize)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository objER = new DSR_ImportRepository();

                if (objCargoSeize.IsSeize)
                {
                    objCargoSeize.SeizeHoldStatus = 2;
                }
                else if (objCargoSeize.IsHold)
                {
                    objCargoSeize.SeizeHoldStatus = 1;
                }
                else
                {
                    objCargoSeize.SeizeHoldStatus = 0;
                }

                objER.AddEditCargoSeize(objCargoSeize);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        [HttpGet]
        public ActionResult ListOfCargoSeize()
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            List<DSRCargoSeize> lstCargoSeize = new List<DSRCargoSeize>();
            objER.GetAllCargoSeize();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<DSRCargoSeize>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCargoSeize(int CargoSeizeId)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            if (CargoSeizeId > 0)
                objER.DeleteCargoSeize(CargoSeizeId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region IRR
        public ActionResult CreateIRR(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            //objImport.GetContainersForIRR();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.ContainersList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.ContainersList = null;
            objImport.GetTrainListForIRR();
            if (objImport.DBResponse.Status > 0)
                ViewBag.TrainsList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.TrainsList = null;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetContainersForIRR(int TrainSummaryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetContainersForIRR(TrainSummaryId);
            var Output = (List<DSRContainerDetailsIRR>)ObjIR.DBResponse.Data;
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIRRPaymentSheet(string InvoiceDate, string CFSCode, string TaxType, int CargoType, int InvoiceId = 0)
        {
            DSR_ImportRepository objPpgRepo = new DSR_ImportRepository();
            objPpgRepo.GetIRRPaymentSheet(InvoiceDate, CFSCode, TaxType, CargoType, InvoiceId);
            var Output = (PpgPostPaymentChrg)objPpgRepo.DBResponse.Data;



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditIRRPaymentSheet(DSRIrrInvoice InvoiceObj)
        {
            try
            {

                int BranchId = Convert.ToInt32(Session["BranchId"]);
                DSR_ImportRepository objChe = new DSR_ImportRepository();

                objChe.AddEditInvoiceIRR(InvoiceObj, BranchId, ((Login)(Session["LoginUser"])).Uid);


                return Json(objChe.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = ex.Message.ToString(), Data = "" };
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            DSR_ExportRepository objExp = new DSR_ExportRepository();
            /*
            objImport.GetApplicationForEmptyContainer(Convert.ToString(ViewData["ForType"]));
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            */
            //objImport.GetEmptyContainerListForInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.EmptyContList = null;

            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            objImport.GetImpPaymentPartyForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objImport.GetImpPaymentPartyForPage("", 0);
            if (objImport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objImport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            objExp.GetLocationForInternalMovement();
            if (objExp.DBResponse.Data != null)
            {
                ViewBag.LocationNoList = JsonConvert.SerializeObject(objExp.DBResponse.Data);//
                //new SelectList((List<DSR_ContainerMovement>)objExp.DBResponse.Data, "LocationId", "LocationName");
            }
            else
            {
                ViewBag.LocationNoList = null;
            }

            return PartialView();
        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetEmptyContainerListForInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.EmptyContList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.EmptyContList = null;

            return Json(ViewBag.EmptyContList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ListOfEmptyImpInvoice(string Module, string ContainerNo = null, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfEmptyImpInvoice(Module,ContainerNo, InvoiceNo, InvoiceDate,Page);
            List<DSRListOfImpInvoice> obj = new List<DSRListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<DSRListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfEmptyContainerInvoice", obj);
        }




        /*
                [HttpGet]
                public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
                {
                    DSR_ImportRepository objImport = new DSR_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, 0);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

                [HttpGet]
                public JsonResult LoadPartyLists(string PartyCode, int Page)
                {
                    DSR_ImportRepository objImport = new DSR_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, Page);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

        */





        [HttpGet]
        public JsonResult PartyBinding()
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetPaymentPartyForImportInvoice();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetEmptyCont(string InvoiceFor, int AppraisementId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            //objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);

            objImport.GetEmptyContByEntryId(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId,
            List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor,string MovementType, string ExportUnder,int PortId=0, int ISGREPaid=0)
        {

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            DSR_ImportRepository objImport = new DSR_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId, ISGREPaid, MovementType, ExportUnder, PortId);
            var Output = (DSRInvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;

            Output.Module = "EC";
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
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditECDeliveryPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
                var GREPaidById = Convert.ToInt32(objForm["GREPaidById"].ToString());
                var EmptyPort = objForm["PortName"].ToString();
                var MovementType = Convert.ToString(objForm["hdnEmptyMovement"]);

                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
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
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "EC";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                DSR_ImportRepository objImport = new DSR_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objImport.AddEditEmptyContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor, GREPaidById, EmptyPort, MovementType);


                invoiceData.InvoiceNo = Convert.ToString(objImport.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                objImport.DBResponse.Data = invoiceData;
                return Json(objImport.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion

        #region Tentative Yard Invoice
        [HttpGet]
        public ActionResult TentativeImpInvoice()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateTentativeYardPaymentSheet(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();
            objImport.GetAppraismentRequestForTentativePaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;


            return PartialView();
        }


        [HttpGet]
        public JsonResult GetPaymentSheetTentativeContainer(int AppraisementId)
        {
            ImportRepository objImport = new ImportRepository();
            objImport.GetContainerForTentativePaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }





        [NonAction]
        public string GeneratingTentativePDFforYard(DSRInvoiceYard invoiceDataobj)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            //   List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            //   List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + invoiceDataobj.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span>");
            html.Append("<br />");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + invoiceDataobj.CompanyGstNo + "</label></span></td></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
            html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span></span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + invoiceDataobj.InvoiceDate + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
            html.Append("<span>" + invoiceDataobj.PartyName + "</span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + invoiceDataobj.PartyState + "</span> </td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
            html.Append("Party Address :</label> <span>" + invoiceDataobj.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
            html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + invoiceDataobj.PartyStateCode + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + invoiceDataobj.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + invoiceDataobj.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/
            int i = 1;

            foreach (DSRPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.GrossWeight.ToString("0.000") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (obj.CargoType == 0 ? "" : (obj.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            /***************/
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + invoiceDataobj.ShippingLineName + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
            html.Append("<td style='font-size: 12px;'>OBL No :  </td>");
            html.Append("<td style='font-size: 12px;'>Item No. : </td>");
            html.Append("<td style='font-size: 12px;'>BOE No : " + invoiceDataobj.BOENo + " </td>");
            html.Append("<td style='font-size: 12px;'>BOE Date : " + invoiceDataobj.BOEDate + " </td>");
            html.Append("</tr>");
            html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + invoiceDataobj.ImporterExporter + " </td>");
            html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + invoiceDataobj.TotalValueOfCargo.ToString("0.00") + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;' colspan='5'>CHA Name : " + invoiceDataobj.CHAName + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + invoiceDataobj.TotalNoOfPackages.ToString() + " </td>");
            html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + invoiceDataobj.TotalGrossWt.ToString("0.000") + " </td>");
            html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) :  </td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("</tr>");
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

            foreach (DSRPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + obj.ChargeType + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + obj.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + obj.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + obj.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + obj.Total.ToString("0") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");

            html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody>");

            html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + invoiceDataobj.TotalTaxable.ToString("0") + "</th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + invoiceDataobj.InvoiceAmt.ToString("0") + "</th></tr><tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalCGST.ToString("0") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalSGST.ToString("0") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalIGST.ToString("0") + "</th></tr>");

            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(invoiceDataobj.InvoiceAmt.ToString("0")) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
            html.Append("<label style='font-weight: bold;'></label></td></tr>");
            html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>Assistant <br/>(Signature)</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/SIO <br/>(Signature)</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            /***************/

            lstSB.Add(html.ToString());
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }





        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintTentativeYardInvoice(DSRInvoiceYard objForm)
        {

            int BranchId = Convert.ToInt32(Session["BranchId"]);

            var invoiceData = objForm;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
            string Path = GeneratingTentativePDFforYard(invoiceData);
            return Json(new { Status = 1, Message = Path });


        }


        [HttpGet]
        public ActionResult CreateTentativeDeliveryPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return PartialView();
        }


        [NonAction]
        public string GeneratingTentativePDFforGodown(DSRInvoiceGodown invoiceDataobj)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();
            //   List<dynamic> objCompany = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[0]);
            //   List<dynamic> lstInvoice = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[1]);
            //    List<dynamic> lstContainer = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[2]);
            //    List<dynamic> lstCharges = ConvertingDataTabletoDynamic.ToDynamic(ds.Tables[3]);
            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + invoiceDataobj.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DASHRATH</span>");
            html.Append("<br />");
            html.Append("</td></tr>");
            html.Append("<tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>");
            html.Append("CWC GST No. <label>" + invoiceDataobj.CompanyGstNo + "</label></span></td></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='5'>");
            html.Append("<tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr>");
            html.Append("<tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='5'><tbody>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span></span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>" + invoiceDataobj.InvoiceDate + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label>");
            html.Append("<span>" + invoiceDataobj.PartyName + "</span></td>");
            html.Append("<td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>" + invoiceDataobj.PartyState + "</span> </td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>");
            html.Append("Party Address :</label> <span>" + invoiceDataobj.PartyAddress + "</span></td><td style='font-size: 13px; line-height: 26px;'>");
            html.Append("<label style='font-weight: bold;'>State Code :</label> <span>" + invoiceDataobj.PartyStateCode + "</span></td></tr>");
            html.Append("<tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>" + invoiceDataobj.PartyGST + "</span></td>");
            html.Append("</tr></tbody> ");
            html.Append("</table></td></tr><tr><td><hr /></td></tr><tr><th><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Assessment No :" + invoiceDataobj.RequestNo + "</b> ");
            html.Append("<br /><b style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</b> </th></tr>");
            html.Append("<tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Arrival</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Weight</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Date of Delivery</th>");
            html.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>No of Days</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>Cargo Type</th>");
            html.Append("</tr></thead><tbody>");
            /*************/
            /*Container Bind*/
            int i = 1;

            foreach (DSRPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.CFSCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ContainerNo + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.Size + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.ArrivalDate + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + obj.GrossWeight.ToString("0.000") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center;'></td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>" + (obj.CargoType == 0 ? "" : (obj.CargoType == 1 ? "Haz" : "Non-Haz")) + "</td>");
                html.Append("</tr>");
                i = i + 1;
            }

            /***************/
            html.Append("</tbody></table></td></tr>");
            html.Append("<tr><td>");
            html.Append("<table style=' border: 1px solid #000; width:100%;' cellspacing='0' cellpadding='5'>");
            html.Append("<tr><td style='font-size: 12px;' colspan='5'>Shipping Line : " + invoiceDataobj.ShippingLineName + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;'>Shipping Line No.:  </td>");
            html.Append("<td style='font-size: 12px;'>OBL No :  </td>");
            html.Append("<td style='font-size: 12px;'>Item No. : </td>");
            html.Append("<td style='font-size: 12px;'>BOE No : " + invoiceDataobj.BOENo + " </td>");
            html.Append("<td style='font-size: 12px;'>BOE Date : " + invoiceDataobj.BOEDate + " </td>");
            html.Append("</tr>");
            html.Append("<tr><td style='font-size: 12px;' colspan='3'>Importer : " + invoiceDataobj.ImporterExporter + " </td>");
            html.Append("<td style='font-size: 12px;' colspan='2'>VALUE : " + invoiceDataobj.TotalValueOfCargo.ToString("0.00") + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;' colspan='5'>CHA Name : " + invoiceDataobj.CHAName + " </td></tr>");
            html.Append("<tr><td style='font-size: 12px;'>No Of Pkg : " + invoiceDataobj.TotalNoOfPackages.ToString() + " </td>");
            html.Append("<td style='font-size: 12px;'>Total Gr.Wt (In Kg) : " + invoiceDataobj.TotalGrossWt.ToString("0.000") + " </td>");
            html.Append("<td style='font-size: 12px;'>Total Area (In Sqr Mtr) :  </td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("</tr>");
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

            foreach (DSRPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
            {
                html.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + i.ToString() + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'>" + obj.ChargeType + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'>" + obj.ChargeName + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + obj.SACCode + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + obj.Taxable.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.CGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.SGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTPer.ToString("0") + "</td>");
                html.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + obj.IGSTAmt.ToString("0") + "</td>");
                html.Append("<td style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 150px;'>" + obj.Total.ToString("0") + "</td></tr>");
                i = i + 1;
            }
            html.Append("</tbody>");
            html.Append("</table></td></tr></tbody></table>");

            html.Append("<table style='border-top: 1px solid #000; border-left: 1px solid #000; width: 100%; margin-bottom: 10px; border-right: 1px solid #000; border-bottom: 1px solid #000;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody>");

            html.Append("<tr><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>Total :</th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 140px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 24%;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 80px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 130px;'>" + invoiceDataobj.TotalTaxable.ToString("0") + "</th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th colspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 200px;'></th>");
            html.Append("<th rowspan='2' style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 100px;'>" + invoiceDataobj.InvoiceAmt.ToString("0") + "</th></tr><tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalCGST.ToString("0") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalSGST.ToString("0") + "</th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'></th>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 10px; text-align: center; width: 50px;'>" + invoiceDataobj.TotalIGST.ToString("0") + "</th></tr>");

            html.Append("<tr>");
            html.Append("<th style='border-bottom: 1px solid #000; font-size: 11px; text-align: left; padding: 5px;' colspan='13'>Total Invoice (In Word) : " + objCurr.changeCurrencyToWords(invoiceDataobj.InvoiceAmt.ToString("0")) + "</th>");
            html.Append("</tr>");
            html.Append("<tr>");
            html.Append("<th style='font-size: 11px; text-align: left; padding: 5px;' colspan='7'>Amount of Tax Subject of Reverse : 0</th>");
            html.Append("</tr>");
            html.Append("</tbody>");
            html.Append("</table>");

            html.Append("<table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'colspan='2'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;' colspan='2'>Party Code:");
            html.Append("<label style='font-weight: bold;'></label></td></tr>");
            html.Append("<tr><td style='font-size: 11px; text-align: left;'><br/><br/>Signature CHA / Importer</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>Assistant <br/>(Signature)</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/AM Accounts<br/>(Signature)</td>");
            html.Append("<td style='font-size: 11px; text-align: left;  '><br/><br/>SAM/SIO <br/>(Signature)</td>");
            html.Append("</tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            /***************/

            lstSB.Add(html.ToString());
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
                rp.GeneratePDF(location, lstSB);
            }
            return "/Docs/" + Session.SessionID + "/" + FileName;
        }





        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintTentativeGodowndInvoice(DSRInvoiceGodown objForm)
        {

            int BranchId = Convert.ToInt32(Session["BranchId"]);

            var invoiceData = DSRtentativeinvoice.InvoiceObj;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
            string Path = GeneratingTentativePDFforGodown(invoiceData);
            return Json(new { Status = 1, Message = Path });


        }



        #endregion


        #region Job Order By Road
        [HttpGet]
        public ActionResult CreateJobOrderByRoad()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(objIR.DBResponse.Data);
            }
            objIR.ListOfShippingLinePartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            DSR_JobOrderByRoad objJO = new DSR_JobOrderByRoad();
            objJO.FormOneDate = DateTime.Now;
            return PartialView("CreateJobOrderByRoad", objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderByRoadDetails()
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            IList<DSR_ImportJobOrderByRoadList> lstIJO = new List<DSR_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadList();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<DSR_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderByRoadDetails(string ContainerNo)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            IList<DSR_ImportJobOrderByRoadList> lstIJO = new List<DSR_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadList(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<DSR_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }

        [HttpGet]
        public ActionResult EditJobOrderByRoad(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            DSR_JobOrderByRoad objImp = new DSR_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_JobOrderByRoad)objIR.DBResponse.Data;

            //objIR.ListOfShippingLine();
            //if (objIR.DBResponse.Data != null)
            //    ViewBag.ListOfShippingLine = objIR.DBResponse.Data;

            objIR.ListOfShippingLinePartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }

            ViewBag.View = "Edit";
            return PartialView("CreateJobOrderByRoad", objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrderByRoad(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            DSR_JobOrderByRoad objImp = new DSR_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (DSR_JobOrderByRoad)objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            ViewBag.View = "View";
            return PartialView("CreateJobOrderByRoad", objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrderByRoad(int ImpJobOrderId)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            if (ImpJobOrderId > 0)
                objImport.DeleteJobOrderByRoad(ImpJobOrderId);
            return Json(objImport.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrderByRoad(DSR_JobOrderByRoad objImp, String FormOneDetails)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            List<DSR_ImportJobOrderByRoadDtl> lstDtl = new List<DSR_ImportJobOrderByRoadDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSR_ImportJobOrderByRoadDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            objIR.AddEditJobOrderByRoad(objImp, BranchId, ((Login)(Session["LoginUser"])).Uid, XML);
            return Json(objIR.DBResponse);


        }


        [HttpGet]
        public JsonResult GetJobOrderByRoadByOnEditMode(int ImpJobOrderId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetJobOrderByRoadByOnEditMode(ImpJobOrderId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = (List<DSR_ImportJobOrderByRoadDtl>)objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Empty Container Transfer
        [HttpGet]
        public ActionResult EmptyContainerTransferInv(string type = "Tax")
        {
            ViewData["InvType"] = type;
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.GetEmptyContaierListForTransfer();
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainersList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainersList = null;

            objImport.ListOfShippingLine();
            if (objImport.DBResponse.Status > 0)
                ViewBag.ShippingLine = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ShippingLine = null;

            return PartialView();
        }
        [HttpGet]
        public JsonResult CalculateEmptyContTransferInv(string InvoiceDate, string InvoiceType, string CFSCode, string ContainerNo, string Size, string EntryDate,
            string EmptyDate, int RefId, int PartyId, int PayeeId, int InvoiceId = 0)
        {
            DSR_ImportRepository objImport = new DSR_ImportRepository();
            objImport.CalculateEmptyContTransferInv(InvoiceDate, InvoiceType, CFSCode, ContainerNo, Size, EntryDate,
             EmptyDate, RefId, PartyId, PayeeId, InvoiceId);
            var Output = (DSRInvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "ECTrns";
            Output.InvoiceType = InvoiceType;
            Output.InvoiceId = 0;

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
                if (!Output.ArrivalDate.Contains(item.ArrivalDate))
                    Output.ArrivalDate += item.ArrivalDate + ", ";
                if (!Output.DestuffingDate.Contains(item.DestuffingDate))
                    Output.DestuffingDate += item.DestuffingDate + ", ";
                if (!Output.StuffingDate.Contains(item.StuffingDate))
                    Output.StuffingDate += item.StuffingDate + ", ";
                if (!Output.CartingDate.Contains(item.CartingDate))
                    Output.CartingDate += item.CartingDate + ", ";
                if (!Output.lstPostPaymentCont.Any(o => o.CFSCode == item.CFSCode))
                {
                    Output.lstPostPaymentCont.Add(new DSRPostPaymentContainer
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
                Output.AllTotal = Output.TotalAmt + Output.TotalCGST + Output.TotalSGST + Output.TotalIGST;
                Output.InvoiceAmt = Output.TotalAmt + Output.TotalCGST + Output.TotalSGST + Output.TotalIGST;
                Output.RoundUp = Output.InvoiceAmt - Output.AllTotal;
            });
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult GetEmptyContToShipLineForTransfer(string ContainerNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            ObjIR.GetEmptyContToShipLineForTransfer(ContainerNo);
            var Output = (List<dynamic>)ObjIR.DBResponse.Data;
            return Json(Output, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditEmtpyTranserPaymentSheet(String InvoiceObj)
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
                var invoiceFor = "ECTrns"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceYard>(InvoiceObj); ;
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
                string ChargesBreakupXML = "";
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
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "ECTrns";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                DSR_ImportRepository objImport = new DSR_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
                objImport.AddEditEmptyContTransferPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);


                invoiceData.InvoiceNo = Convert.ToString(objImport.DBResponse.Data);
                invoiceData.ROAddress = (invoiceData.ROAddress).Replace("|_br_|", "<br/>");
                invoiceData.CompanyAddress = (invoiceData.CompanyAddress).Replace("|_br_|", "<br/>");
                objImport.DBResponse.Data = invoiceData;
                return Json(objImport.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }


        #endregion

        #region Merge of Delivery App,Delivery Payment Sheet and Issue Slip
        [HttpGet]
        public ActionResult MergeDeliAppPaymentSheetIssueSlip(string type = "Tax")
        {
            ViewData["InvType"] = type;
            return PartialView();
        }
        [HttpGet]
        public ActionResult MergeSingleDeliAppPaymentSheetIssueSlip(string type = "Tax")
        {
            ViewData["InvType"] = type;
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetDestuffingNo()
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNo(((Login)(Session["LoginUser"])).Uid);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo,OBLNo=item.OBLNo });
                });

            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetImporterName()
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.ListOfImporterForMerge();

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Importer>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { ImporterId = item.ImporterId, ImporterName = item.ImporterName });
                });

            }
            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHANAME()
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DSRCHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode, BillToParty = item.BillToParty, IsInsured = item.IsInsured, IsTransporter = item.IsTransporter, InsuredFrmdate = item.InsuredFrmdate, InsuredTodate = item.InsuredTodate });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCHANAMEForPayment()
        {

            DSR_ImportRepository ObjIR = new DSR_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.GetImpPaymentPartyForMergePage("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<ImpPartyForpage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { PartyId = item.PartyId, PartyName = item.PartyName, Address = item.Address, State = item.State, StateCode = item.StateCode, GSTNo = item.GSTNo, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMergeDeliveryApplication(DSRMergeDeliveryIssueViewModel ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DSRDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DSRDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
                    DeliveryOrdXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryordDtl);
                }


                ObjIR.AddEditMergeDeliveryApplication(ObjDelivery, DeliveryXml, DeliveryOrdXml);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 1, Message = ErrorMessage });
            }
        }







        [HttpGet]
        public JsonResult GetDeliAppMerge(int DeliveryId)
        {
            DSR_ImportRepository objIR = new DSR_ImportRepository();
            objIR.GetDeliveryAppforMerge(DeliveryId);
            DSRdeliverydet pdet = new DSRdeliverydet();
            if (objIR.DBResponse.Data != null)
                pdet = (DSRdeliverydet)objIR.DBResponse.Data;
            return Json(pdet, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliverymergePaymentSheet(FormCollection objForm)
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

                //var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                //string ContainerXML = "";
                //string ChargesXML = "";
                //string ContWiseCharg = "";

                //foreach (var item in invoiceData.lstPostPaymentCont)
                //{
                //    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                //    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                //    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                //    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                //}

                //if (invoiceData.lstPostPaymentCont != null)
                //{
                //    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                //}
                //if (invoiceData.lstPostPaymentChrg != null)
                //{
                //    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                //}
                //if (invoiceData.lstContWiseAmount != null)
                //{
                //    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                //}
                //if (invoiceData.lstCfsCodewiseRateHT != null)
                //{
                //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                //}
                //DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli");
                //invoiceData.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);


                //   int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DSRInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "", CargoXML = "";
                string ChargesBreakupXML = "";

                foreach (var item in invoiceData.lstPostPaymentCont)
                {
                    item.ArrivalDate = string.IsNullOrEmpty(item.ArrivalDate) ? "0" : item.ArrivalDate;
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.StuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.DestuffingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? Convert.ToDateTime("01/01/1900") : DateTime.ParseExact(((DateTime)item.CartingDate).ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//item.CartingDate.ToString("yyyy-MM-dd");
                    item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
                }

                foreach (var item in invoiceData.lstInvoiceCargo)
                {
                    item.StuffingDate = string.IsNullOrEmpty(item.StuffingDate.ToString()) ? "1900-01-01" : item.StuffingDate;
                    item.DestuffingDate = string.IsNullOrEmpty(item.DestuffingDate.ToString()) ? "1900-01-01" : item.DestuffingDate;
                    item.CartingDate = string.IsNullOrEmpty(item.CartingDate.ToString()) ? "1900-01-01" : item.CartingDate;
                    //  item.SpaceOccupiedUnit = string.IsNullOrEmpty(item.SpaceOccupiedUnit) ? "0" : item.SpaceOccupiedUnit;
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                if (invoiceData.lstInvoiceCargo != null)
                {
                    CargoXML = Utility.CreateXML(invoiceData.lstInvoiceCargo);
                }
                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditInvoiceGodownMerge(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPDeli", CargoXML);

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
        public JsonResult GetInvcDetForMergeIssueSlip(string InvoiceNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR = new DSR_ImportRepository();
            ObjIR.GetInvoiceDetForMergeIssueSlip(InvoiceNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMergeIssueSlip(DSRMergeDeliveryIssueViewModel ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                DSR_ImportRepository ObjIR = new DSR_ImportRepository();
                ObjIssueSlip.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditMergeIssueSlip(ObjIssueSlip);
                ModelState.Clear();
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }


        #endregion

        #region Import Bond Conversion Godown

        [HttpGet]
        public ActionResult ImportBondConversion()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_OBLNoForBondConversion> lstOBL = new List<DSR_OBLNoForBondConversion>();
            ObjIR.GetAllOBLNo();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.OBLList = ObjIR.DBResponse.Data;// new SelectList((List<DSR_OBLNoForBondConversion>)ObjIR.DBResponse.Data);
            }
            else
            {
                ViewBag.OBLList = null;
            }

            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
            ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
            }
            ViewBag.GodownList = lstGodown;

            List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
            //ObjIR.GetAllSacNo();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            //}
            ViewBag.SacList = lstSac;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSacNo(string SacNo = "")
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllSacNo(SacNo);
            List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGodownByDestuffingId(int destuffingId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetGodwonById(destuffingId);
            if (ObjIR.DBResponse.Data != null)
            {
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetLocationForBondTransfer(int destuffingId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetLocationForBondTransfer(destuffingId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOBLDetailsByDestuffingId(int DestuffingEntryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetOBLDetailsByDestuffingIdList(DestuffingEntryId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSACNoByIMPCHA(int ChaId, int ImporterId, string OBLNo, string SacNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
            ObjIR.GetAllSacNo(SacNo, ChaId, ImporterId, OBLNo);
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return Json(lstSac, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownBySacNo(string SACNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
            ObjIR.GetAllGodownExpBond(((Login)(Session["LoginUser"])).Uid, SACNo);
            if (ObjIR.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
            }
            return Json(lstGodown, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocationDetailsByGodownId(int GodownId)
        {
            DSR_ExportRepository objER = new DSR_ExportRepository();
            objER.GetLocationDetailsByGodownId(GodownId);
            var obj = new List<Areas.Export.Models.DSRGodownWiseLocation>();
            if (objER.DBResponse.Data != null)
                obj = (List<Areas.Export.Models.DSRGodownWiseLocation>)objER.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetOBLDetailsByDestuffingIdOLD(int DestuffingEntryId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetOBLDetailsByDestuffingId(DestuffingEntryId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBondMovementList()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllInternalBondMovement("GODN");
            List<DSR_ImportBondConversion> LstMovement = new List<DSR_ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditBondConversionMovement(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_ImportBondConversion ObjInternalMovement = new DSR_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_ImportBondConversion)ObjIR.DBResponse.Data;
                try
                {
                    if (ObjInternalMovement.WRDate != null && ObjInternalMovement.WRDate != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(ObjInternalMovement.WRDate, out dDate))
                        {
                            if(dDate<Convert.ToDateTime("01/01/1970"))
                            {
                                ObjInternalMovement.WRDate = "";
                            }
                        }
                        else
                        {
                            ObjInternalMovement.WRDate = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjInternalMovement.WRDate = "";
                }

                //List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                //ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                //}
                //ViewBag.GodownList = lstGodown;

                List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                ObjIR.GetAllGodownExpBond(((Login)(Session["LoginUser"])).Uid, ObjInternalMovement.SACNo);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                }
                ViewBag.GodownList = lstGodown;

                List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
                //ObjIR.GetAllSacNo();
                ObjIR.GetAllSacNo("", ObjInternalMovement.CHAId, ObjInternalMovement.ImporterId);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
                }
                ViewBag.SacList = lstSac;
            }
            return PartialView("EditImportBondConversion", ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewBondConversionMovement(int MovementId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            PPG_Internal_Movement ObjInternalMovement = new PPG_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_Movement)ObjIR.DBResponse.Data;
                
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditImportBondConversion(DSR_ImportBondConversion objForm)
        {
            try
            {
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddEditImportBondConversion(objForm);
                return Json(objChargeMaster.DBResponse);
            }
            catch (Exception ex)
            {
                var Err = new { Status = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ViewBondInternalMovement(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_ImportBondConversion ObjInternalMovement = new DSR_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_ImportBondConversion)ObjIR.DBResponse.Data;
                try
                {
                    if (ObjInternalMovement.WRDate != null && ObjInternalMovement.WRDate != "")
                    {
                        DateTime dDate;
                        if (DateTime.TryParse(ObjInternalMovement.WRDate, out dDate))
                        {
                            if (dDate < Convert.ToDateTime("01/01/1970"))
                            {
                                ObjInternalMovement.WRDate = "";
                            }
                        }
                        else
                        {
                            ObjInternalMovement.WRDate = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjInternalMovement.WRDate = "";
                }
            }
            return PartialView("ViewImportBondConversion", ObjInternalMovement);
        }

        #endregion

        #region Import Bond Conversion Yard

        [HttpGet]
        public ActionResult ImportBondConversionYard()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            List<DSR_OBLNoForBondConversion> lstOBL = new List<DSR_OBLNoForBondConversion>();
            ObjIR.GetAllOBLNoForYard();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.OBLList = ObjIR.DBResponse.Data;
            }
            else
            {
                ViewBag.OBLList = null;
            }

            List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
            ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
            }
            ViewBag.GodownList = lstGodown;

            List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
            ObjIR.GetAllSacNo();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            ViewBag.SacList = lstSac;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetOBLDetailsByYard(int DestuffingEntryId, string OBLNo)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAppraisementDetById(DestuffingEntryId, OBLNo);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBondMovementListForYard()
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            ObjIR.GetAllInternalBondMovement("YARD");
            List<DSR_ImportBondConversion> LstMovement = new List<DSR_ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementListYard", LstMovement);
        }

        [HttpGet]
        public ActionResult EditBondConversionMovementYard(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_ImportBondConversion ObjInternalMovement = new DSR_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "YARD");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_ImportBondConversion)ObjIR.DBResponse.Data;
                //List<DSR_OBLNoForBondConversion> lstOBL = new List<DSR_OBLNoForBondConversion>();
                //ObjIR.GetAllOBLNo();
                //if (ObjIR.DBResponse.Data != null)
                //{
                //    ViewBag.OBLList = ObjIR.DBResponse.Data;// new SelectList((List<DSR_OBLNoForBondConversion>)ObjIR.DBResponse.Data);
                //}
                //else
                //{
                //    ViewBag.OBLList = null;
                //}

                List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                }
                ViewBag.GodownList = lstGodown;

                List<DSR_ImportBondConversion> lstSac = new List<DSR_ImportBondConversion>();
                ObjIR.GetAllSacNo();
                if (ObjIR.DBResponse.Data != null)
                {
                    lstSac = (List<DSR_ImportBondConversion>)ObjIR.DBResponse.Data;
                }
                ViewBag.SacList = lstSac;
            }

            return PartialView("EditImportBondConversionYard", ObjInternalMovement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDSR_ImportBondConversionYard(DSR_ImportBondConversion objForm)
        {
            try
            {
                if (objForm.SQM > 0)
                {
                    DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                    objChargeMaster.AddEditImportBondConversionYard(objForm);
                    return Json(objChargeMaster.DBResponse);
                }
                else
                {
                    var Err = new { Status = -1, Message = "SQM can't be zero" };
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
        public ActionResult ViewDSR_ImportBondConversionYard(int MovementId)
        {
            DSR_ImportRepository ObjIR = new DSR_ImportRepository();
            DSR_ImportBondConversion ObjInternalMovement = new DSR_ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "YARD");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (DSR_ImportBondConversion)ObjIR.DBResponse.Data;
            }
            return PartialView("ViewImportBondConversionYard", ObjInternalMovement);
        }

        #endregion

        #region RCT Sealing Invoice
        [HttpGet]
        public ActionResult CreateRCTSealingInvoice(string type = "Tax")
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            ViewData["InvType"] = type;

            return PartialView();
        }



        [HttpGet]
        public JsonResult GetPartyForRCTSealing()
        {
            DSR_ImportRepository objExport = new DSR_ImportRepository();
            objExport.GetPaymentParty();
            if (objExport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objExport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;

            return Json(ViewBag.PaymentParty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHAForRCTSealing()
        {
            var CHA = "CHA";
            DSR_ImportRepository objCHA = new DSR_ImportRepository();
            objCHA.GetCHAExporterForRCTSealing(CHA);
            if (objCHA.DBResponse.Status > 0)
                ViewBag.CHAList = JsonConvert.SerializeObject(objCHA.DBResponse.Data);
            else
                ViewBag.CHAList = null;

            return Json(ViewBag.CHAList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExporterForRCTSealing()
        {
            var Exporter = "Importer";
            DSR_ImportRepository objExporter = new DSR_ImportRepository();
            objExporter.GetCHAExporterForRCTSealing(Exporter);
            if (objExporter.DBResponse.Status > 0)
                ViewBag.ExporterList = JsonConvert.SerializeObject(objExporter.DBResponse.Data);
            else
                ViewBag.ExporterList = null;

            return Json(ViewBag.ExporterList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetShippingLineForRCTSealing()
        {
            var ShippingLine = "ShippingLine";
            DSR_ImportRepository objShippingLine = new DSR_ImportRepository();
            objShippingLine.GetCHAExporterForRCTSealing(ShippingLine);
            if (objShippingLine.DBResponse.Status > 0)
                ViewBag.ShippingLineList = JsonConvert.SerializeObject(objShippingLine.DBResponse.Data);
            else
                ViewBag.ShippingLineList = null;

            return Json(ViewBag.ShippingLineList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetImporterForRCTSealing()
        {
            var Exporter = "Importer";
            DSR_ExportRepository objExporter = new DSR_ExportRepository();
            objExporter.GetCHAExporterForRCTSealing(Exporter);
            if (objExporter.DBResponse.Status > 0)
                ViewBag.ExporterList = JsonConvert.SerializeObject(objExporter.DBResponse.Data);
            else
                ViewBag.ExporterList = null;

            return Json(ViewBag.ExporterList, JsonRequestBehavior.AllowGet);
        }

        [CustomValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult GetRCTSealingInvoice(string InvoiceDate, string TaxType, string ConatinerDetails, string OBLDetails, int PartyId, int PayeeId, int InvoiceId = 0, string SEZ = "")
        {
            DSR_ImportRepository objRCT = new DSR_ImportRepository();
            List<DSRPreInvoiceContainerRCTSealing> lstContinerDetails = new List<DSRPreInvoiceContainerRCTSealing>();
            List<DSRPreInvoiceOBLRCTSealing> lstOBLDetails = new List<DSRPreInvoiceOBLRCTSealing>();

            string XMLContText = "";
            if (ConatinerDetails != null)
            {
                lstContinerDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSRPreInvoiceContainerRCTSealing>>(ConatinerDetails);
                if (lstContinerDetails.Count > 0)
                    XMLContText = Utility.CreateXML(lstContinerDetails);

            }

            string XMLOBLText = "";
            if (OBLDetails != null)
            {
                lstOBLDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DSRPreInvoiceOBLRCTSealing>>(OBLDetails);
                if (lstOBLDetails.Count > 0)
                    XMLOBLText = Utility.CreateXML(lstOBLDetails);

            }

            objRCT.GetRCTSealingInvoice(InvoiceDate, TaxType, XMLContText, XMLOBLText, PartyId, PayeeId, InvoiceId,SEZ);
            var Output = (DSRInvoiceRCTSealing)objRCT.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            Output.Module = "IMPRCTSeal";

            Output.lstPrePaymentCont.ToList().ForEach(item =>
            {
                Output.lstPostPaymentCont.Add(new DSRPostPaymentContainerRCTSealing
                {
                    ForeignLine =item.ForeignLine,
                    Vessel=item.Vessel,
                    Voyage=item.Voyage,
                    ContainerNo = item.ContainerNo,
                    Size = item.Size,
                    PortId = item.PortId,
                    PortName = item.PortName,
                    DeliveryDate = InvoiceDate

                });

            });
            Output.lstPrePaymentOBL.ToList().ForEach(item =>
            {
                Output.lstPostPaymentOBL.Add(new DSRPostInvoiceOBLRCTSealing
                {
                    OBLNo = item.OBLNo,
                    OBLDate = item.OBLDate,
                    NoOfPkg = item.NoOfPkg,
                    GrossWT = item.GrossWT,
                    FOBValue = item.FOBValue
                });

            });

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


            return Json(Output);
        }


        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddRCTSealingPaymentSheet(DSRInvoiceRCTSealing objForm, String SEZ)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = objForm;
                string ContainerXML = "";
                string OBLXML = "";
                string ChargesXML = "";

                string ChargesBreakupXML = "";

                if (invoiceData.lstPostPaymentCont != null)
                {
                    ContainerXML = Utility.CreateXML(invoiceData.lstPostPaymentCont);
                }
                if (invoiceData.lstPostPaymentOBL != null)
                {
                    OBLXML = Utility.CreateXML(invoiceData.lstPostPaymentOBL);
                }
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }

                if (invoiceData.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }
                DSR_ImportRepository objChargeMaster = new DSR_ImportRepository();
                objChargeMaster.AddRCTSealingInvoice(invoiceData, ContainerXML, OBLXML, ChargesXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPRCTSeal", SEZ);
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
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            DSR_ExportRepository objER = new DSR_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate, Page);
            List<CwcExim.Areas.Export.Models.DSRListOfExpInvoice> obj = new List<Export.Models.DSRListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Export.Models.DSRListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        [HttpGet]
        public ActionResult ListOfImpRCTInvoice(string Module,string ContainerNo=null, string InvoiceNo = null, string InvoiceDate = null, int Page = 0)
        {
            DSR_ImportRepository objER = new DSR_ImportRepository();
            objER.ListOfImpRCTInvoice(Module, ContainerNo, InvoiceNo, InvoiceDate, Page);
            List<CwcExim.Areas.Export.Models.DSRListOfExpInvoice> obj = new List<Export.Models.DSRListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Export.Models.DSRListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }

        #endregion


        #region OBL Amendment Split 

        public ActionResult OBLSplit()
        {
           
           
            return PartialView();
        }


        public JsonResult GetOBLListForSpilt()
        {
            try
            {
                DSR_ImportRepository obj = new DSR_ImportRepository();
                obj.GetOBLListForSpilt();
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOBLDetailsForSpilt(string OBL, string OBLDate, int isFCL)
        {
            try
            {
                DSR_ImportRepository obj = new DSR_ImportRepository();
                obj.GetOBLDetailsForSpilt(OBL, OBLDate, isFCL);
                return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult AddEditOBLSpilt(DSR_OBLSpilt vm, List<DSR_OBLSpiltDetails> lstVM, List<DSR_ContainerSpiltList> LstContainer, int IsFCL, string SpiltDate)
        {
            try
            {


                List<DSR_OBLSpiltDetails> idata = new List<DSR_OBLSpiltDetails>();
                var totalPkg = vm.NoOfPkg;
                var totalWT = vm.GRWT;
                var spiltpkg = lstVM.Sum(x => x.SpiltPkg);
                var spiltwt = lstVM.Sum(x => x.SpiltWT);
                var spiltValue= lstVM.Sum(x => x.SpiltValue);
                if (totalPkg == spiltpkg && totalWT == spiltwt && vm.Value== spiltValue)
                {
                    foreach (var i in lstVM)
                    {
                        DSR_OBLSpiltDetails data = new DSR_OBLSpiltDetails();
                      
                        if (i.SpiltOBLDate != null)
                        {
                            DateTime dtSpiltDate = DateTime.ParseExact(i.SpiltOBLDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            data.SpiltOBLDate = dtSpiltDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            data.SpiltOBLDate = i.SpiltOBLDate;
                        }

                        data.SpiltOBL = i.SpiltOBL;
                        data.SpiltWT = i.SpiltWT;
                        data.SpiltPkg = i.SpiltPkg;
                        data.SpiltDuty = i.SpiltDuty;
                        data.SpiltValue = i.SpiltValue;
                     

                        idata.Add(data);
                    }
                    string OBLXML = "";
                    string ConXml = "";
                    OBLXML = Utility.CreateXML(idata);
                    ConXml = Utility.CreateXML(LstContainer);

                    OBLXML = OBLXML.Replace(">##<", ">null<");
                    DSR_ImportRepository obj = new DSR_ImportRepository();
                    obj.AddEditOBLSpilt(vm, OBLXML, ConXml,((Login)(Session["LoginUser"])).Uid, SpiltDate, IsFCL);
                    return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DAL.DatabaseResponse msg = new DAL.DatabaseResponse();
                    msg.Status = 0;
                    msg.Message = "OBL pkg and wt and duty should be equal split pkg and wt and duty";
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult ListOBLSpilt()
        {
            List<DSR_OBLSpilt> lstOblDetails = new List<DSR_OBLSpilt>();
            DSR_ImportRepository obj = new DSR_ImportRepository();
            obj.GetSpiltOBLDetails();
            if (obj.DBResponse.Data != null)
            {
                lstOblDetails = (List<DSR_OBLSpilt>)obj.DBResponse.Data;
            }
            return PartialView("ListOBLSpilt", lstOblDetails);
        }

        public ActionResult ViewSpiltDetails(int id)
        {
            DSR_OBLSpilt lstOblDetails = new DSR_OBLSpilt();
            DSR_ImportRepository obj = new DSR_ImportRepository();
            obj.GetViewSpiltOBLDetails(id);
            if (obj.DBResponse.Data != null)
            {
                lstOblDetails = (DSR_OBLSpilt)obj.DBResponse.Data;

                ViewBag.SpiltData = JsonConvert.SerializeObject(obj.DBResponse.Data);
               
               
            }
            else
            {
                ViewBag.SpiltData = "";
            }

            return PartialView("ViewSpiltDetails", lstOblDetails);
        }

        public JsonResult DeleteSpiltDetails(int id)
        {
            
            DSR_ImportRepository obj = new DSR_ImportRepository();
            obj.DeleteSpiltOBLDetails(id);
            return Json(obj.DBResponse,JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region ContainerWiseCustomApprisement

        public ActionResult ContainerWiseCustomApprisement()
        {
            return PartialView();
        }
        #endregion

        #region IRN

        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {



            DSR_ImportRepository objPpgRepo = new DSR_ImportRepository();
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
                    log.Info("after calling GenerateEinvoice");
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
                    DSR_IrnB2CDetails irnb2cobj = new DSR_IrnB2CDetails();
                    irnb2cobj = (DSR_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                    if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                    {
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                        objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }
                    else
                    {
                        var tn = "GST QR";
                        UpiQRCodeInfo idata = new UpiQRCodeInfo();
                        idata.ver = irnb2cobj.ver;
                        idata.mode = irnb2cobj.mode;
                        idata.mode = irnb2cobj.mode;
                        idata.tr = irnb2cobj.tr;
                        idata.tid = irnb2cobj.tid;
                        idata.tn = tn;
                        idata.pa = irnb2cobj.pa;
                        idata.pn = irnb2cobj.pn;
                        idata.mc = irnb2cobj.mc;
                        idata.am = irnb2cobj.TotInvVal;
                        idata.mam = irnb2cobj.TotInvVal;
                        idata.mid = irnb2cobj.mid;
                        idata.msid = irnb2cobj.msid;
                        idata.orgId = irnb2cobj.orgId;
                        idata.mtid = irnb2cobj.mtid;
                        idata.CESS = irnb2cobj.CESS;
                        idata.CGST = irnb2cobj.CGST;
                        idata.SGST = irnb2cobj.SGST;
                        idata.IGST = irnb2cobj.IGST;
                        idata.GSTIncentive = irnb2cobj.GSTIncentive;
                        idata.GSTPCT = irnb2cobj.GSTPCT;
                        idata.qrMedium = irnb2cobj.qrMedium;
                        idata.invoiceNo = irnb2cobj.DocNo;
                        idata.InvoiceDate = irnb2cobj.DocDt;
                        idata.InvoiceName = irnb2cobj.InvoiceName;
                        idata.QRexpire = irnb2cobj.DocDt;
                        idata.pinCode = irnb2cobj.pinCode;
                        idata.tier = irnb2cobj.tier;
                        idata.gstIn = irnb2cobj.gstIn;
                        B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                        objresponse = Eobj.GenerateB2cQRCode(idata);
                        IrnModel irnModelObj = new IrnModel();
                        irnModelObj.DocumentDate = irnb2cobj.DocDt;
                        irnModelObj.DocumentNo = irnb2cobj.DocNo;
                        irnModelObj.DocumentType = irnb2cobj.DocTyp;
                        irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                        string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                        IrnResponse objERes = new IrnResponse();
                        objERes.irn = ERes;
                        objERes.SignedQRCode = objresponse.QrCodeBase64;
                        objERes.SignedInvoice = objresponse.QrCodeJson;
                        objERes.SignedQRCode = objresponse.QrCodeJson;

                        objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                    }
                }

            }
            else
            {
                Einvoice Eobj = new Einvoice();
                objPpgRepo.GetIRNB2CForYard(InvoiceNo);
                DSR_IrnB2CDetails irnb2cobj = new DSR_IrnB2CDetails();
                irnb2cobj = (DSR_IrnB2CDetails)objPpgRepo.DBResponse.Data;
                if (irnb2cobj.mtid == "" || irnb2cobj.pa == "")
                {
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    string QRCodeInfo = "000201010211021644038499331959060415522024099331959061661000309933195920825HDFC00000015010030812820026560010A0000005240138centralwarehousingco.99331959@hdfcbank27430010A0000005240125STQ200318122305804I0401085204939953033565802IN5923CENTRAL WAREHOUSING NEW6009New Delhi610611001662410525STQ200318122305804I0401080708993319596304e9fc";
                    objresponse = Eobj.GenerateB2cQRCode(QRCodeInfo);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }
                else
                {
                    var tn = "GST QR";
                    UpiQRCodeInfo idata = new UpiQRCodeInfo();
                    idata.ver = irnb2cobj.ver;
                    idata.mode = irnb2cobj.mode;
                    idata.mode = irnb2cobj.mode;
                    idata.tr = irnb2cobj.tr;
                    idata.tid = irnb2cobj.tid;
                    idata.tn = tn;
                    idata.pa = irnb2cobj.pa;
                    idata.pn = irnb2cobj.pn;
                    idata.mc = irnb2cobj.mc;
                    idata.am = irnb2cobj.TotInvVal;
                    idata.mam = irnb2cobj.TotInvVal;
                    idata.mid = irnb2cobj.mid;
                    idata.msid = irnb2cobj.msid;
                    idata.orgId = irnb2cobj.orgId;
                    idata.mtid = irnb2cobj.mtid;
                    idata.CESS = irnb2cobj.CESS;
                    idata.CGST = irnb2cobj.CGST;
                    idata.SGST = irnb2cobj.SGST;
                    idata.IGST = irnb2cobj.IGST;
                    idata.GSTIncentive = irnb2cobj.GSTIncentive;
                    idata.GSTPCT = irnb2cobj.GSTPCT;
                    idata.qrMedium = irnb2cobj.qrMedium;
                    idata.invoiceNo = irnb2cobj.DocNo;
                    idata.InvoiceDate = irnb2cobj.DocDt;
                    idata.InvoiceName = irnb2cobj.InvoiceName;
                    idata.QRexpire = irnb2cobj.DocDt;
                    idata.pinCode = irnb2cobj.pinCode;
                    idata.tier = irnb2cobj.tier;
                    idata.gstIn = irnb2cobj.gstIn;
                    B2cQRCodeResponse objresponse = new B2cQRCodeResponse();
                    objresponse = Eobj.GenerateB2cQRCode(idata);
                    IrnModel irnModelObj = new IrnModel();
                    irnModelObj.DocumentDate = irnb2cobj.DocDt;
                    irnModelObj.DocumentNo = irnb2cobj.DocNo;
                    irnModelObj.DocumentType = irnb2cobj.DocTyp;
                    irnModelObj.SupplierGstNo = irnb2cobj.SellerGstin;
                    string ERes = Eobj.GenerateB2cIrn(irnModelObj);
                    IrnResponse objERes = new IrnResponse();
                    objERes.irn = ERes;
                    objERes.SignedQRCode = objresponse.QrCodeBase64;
                    objERes.SignedInvoice = objresponse.QrCodeJson;
                    objERes.SignedQRCode = objresponse.QrCodeJson;

                    objPpgRepo.AddEditIRNResponsec(objERes, InvoiceNo);
                }

            }


            return Json(objPpgRepo.DBResponse);
        }

        #endregion
    }
}