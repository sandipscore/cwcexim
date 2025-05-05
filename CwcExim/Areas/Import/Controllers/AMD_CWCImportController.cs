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

    public partial class AMD_CWCImportController : BaseController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        VRN_InvoiceGodown objreport;
        #region Train Summary Upload

        [HttpGet]
        public ActionResult TrainSummaryUpload()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            VRN_ExportRepository ObjRR = new VRN_ExportRepository();
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
        public ActionResult CheckUpload(string TrainNo)
        {
            int status = 0;
            List<TrainSummaryUpload> TrainSummaryUploadList = new List<TrainSummaryUpload>();
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
                                        TrainSummaryUpload objTrainSummaryUpload = new TrainSummaryUpload();
                                        objTrainSummaryUpload.Wagon_No = Convert.ToString(dr["Wagon_No"]);
                                        objTrainSummaryUpload.Container_No = Convert.ToString(dr["Container_No"]);
                                        objTrainSummaryUpload.CT_Size = Convert.ToString(dr["CT_Size"]);
                                        objTrainSummaryUpload.Line_Seal_No = Convert.ToString(dr["Line_Seal_No"]);
                                        objTrainSummaryUpload.Cont_Commodity = Convert.ToString(dr["Cont_Commodity"]);
                                        objTrainSummaryUpload.S_Line = Convert.ToString(dr["S_Line"]);
                                        objTrainSummaryUpload.Ct_Tare = Convert.ToDecimal(dr["Ct_Tare"]);
                                        objTrainSummaryUpload.Cargo_Wt = Convert.ToDecimal(dr["Cargo_Wt"]);
                                        objTrainSummaryUpload.Gross_Wt = Convert.ToDecimal(dr["Gross_Wt"]);
                                        objTrainSummaryUpload.Ct_Status = Convert.ToString(dr["Ct_Status"]);
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

                                VRN_ImportRepository objImport = new VRN_ImportRepository();
                                objImport.CheckTrainSummaryUpload(TrainNo, TrainSummaryUploadXML);
                                if (objImport.DBResponse.Status > -1)
                                {
                                    status = Convert.ToInt32(objImport.DBResponse.Status);
                                    TrainSummaryUploadList = (List<TrainSummaryUpload>)objImport.DBResponse.Data;
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
        public ActionResult SaveUploadData(TrainSummaryUpload objTrainSummaryUpload)
        {
            int data = 0;
            if (objTrainSummaryUpload.TrainSummaryList != null)
                objTrainSummaryUpload.TrainSummaryUploadList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainSummaryUpload>>(objTrainSummaryUpload.TrainSummaryList);
            if (objTrainSummaryUpload.TrainSummaryUploadList.Count > 0)
            {
                string TrainSummaryUploadXML = Utility.CreateXML(objTrainSummaryUpload.TrainSummaryUploadList);
                VRN_ImportRepository objImport = new VRN_ImportRepository();
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
            VRN_ImportRepository objER = new VRN_ImportRepository();
            List<TrainSummaryUpload> lstCargoSeize = new List<TrainSummaryUpload>();
            objER.ListOfTrainSummary();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<TrainSummaryUpload>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpGet]
        public ActionResult GetTrainSummaryDetails(int TrainSummaryUploadId)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetTrainSummaryDetails(TrainSummaryUploadId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }




        #endregion

        #region Custom Appraisement Application
        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            VRN_CustomAppraisement ObjAppraisement = new VRN_CustomAppraisement();
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

            ObjIR.GetContnrNoForCustomAppraise();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ContainerList = new SelectList((List<VRN_CustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
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

            ObjIR.ListOfImporterForm(0, "");
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.ImporterState = Jobject["State"];

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
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ImporterSearchByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfImporterForm(0, PartyCode);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfImporterForm(Page, PartyCode);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public JsonResult SearchByPartyCodeForCustApr(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeListForCustApr(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetBOEDetail(String BOENo)
        {
            VRN_CustomAppraisement objOBLContainer = new VRN_CustomAppraisement();
            VRN_ImportRepository rep = new VRN_ImportRepository();
            rep.GetBOEDetail(BOENo);
            VRN_CustomAppraisementBOECont obj = (VRN_CustomAppraisementBOECont)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOBLContainer(String OBlNo)
        {
            VRN_CustomAppraisement objOBLContainer = new VRN_CustomAppraisement();
            VRN_ImportRepository rep = new VRN_ImportRepository();
            rep.GetOBLContainer(OBlNo);
            if (rep.DBResponse.Data != null)
            {
                List<VRN_CustomAppraisementOBLCont> obj = (List<VRN_CustomAppraisementOBLCont>)rep.DBResponse.Data;
                if (obj.Count > 0)
                {
                    ViewBag.ContainerList = new SelectList((List<VRN_CustomAppraisementOBLCont>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                }
                else
                {
                    rep.GetContnrNoForCustomAppraise();
                    if (rep.DBResponse.Data != null)
                    {
                        ViewBag.ContainerList = new SelectList((List<VRN_CustomAppraisementDtl>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                    }
                }
            }
            return Json(objOBLContainer, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult GetBOEDetail(String BOENo)
        //{
        //    Ppg_ImportRepository rep = new Ppg_ImportRepository();
        //    rep.GetBOEDetail(BOENo);
        //    PPGCustomAppraisementBOECont obj = (PPGCustomAppraisementBOECont)rep.DBResponse.Data;
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetContainerBOL(String CFSCode)
        {
            VRN_CustomAppraisement objOBLContainer = new VRN_CustomAppraisement();
            VRN_ImportRepository rep = new VRN_ImportRepository();
            rep.GetContainerOBL(CFSCode);
            List<VRN_CustomAppraisementOBLCont> obj = (List<VRN_CustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode, string LineNo)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
        public JsonResult AddEditCustomAppraisement(VRN_CustomAppraisement ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                string CAOrdXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<VRN_CustomAppraisementDtl> LstAppraisement = JsonConvert.DeserializeObject<List<VRN_CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                if (ObjAppraisement.CAOrdDtlXml != null)
                {
                    List<VRN_CustomAppraisementOrdDtl> LstCAOrd = JsonConvert.DeserializeObject<List<VRN_CustomAppraisementOrdDtl>>(ObjAppraisement.CAOrdDtlXml);
                    CAOrdXML = Utility.CreateXML(LstCAOrd);
                }

                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjAppraisement.Uid = ((Login)Session["LoginUser"]).Uid;
                ObjIR.AddEditCustomAppraisement(ObjAppraisement, AppraisementXML, CAOrdXML);
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

        [HttpGet]
        public ActionResult ViewCustomAppraisement(int CustomAppraisementId)
        {
            VRN_CustomAppraisement ObjAppraisement = new VRN_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (VRN_CustomAppraisement)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
        }


        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            VRN_CustomAppraisement ObjAppraisement = new VRN_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (VRN_CustomAppraisement)ObjIR.DBResponse.Data;
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

                    ObjIR.ListOfImporterForm(0, "");
                    if (ObjIR.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstImporter = Jobject["lstImporter"];
                        ViewBag.ImporterState = Jobject["State"];

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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();

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


            List<VRN_CustomAppraisement> LstAppraisement = new List<VRN_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementApp();
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<VRN_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }

        [HttpGet]
        public ActionResult GetListOfAppraisementNo(string AppraisementNo)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();

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


            List<VRN_CustomAppraisement> LstAppraisement = new List<VRN_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementAppSearch(AppraisementNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<VRN_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }


        #endregion
        #region Job Order
        [HttpGet]

        public ActionResult CreateJobOrder()
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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


            objIR.ListOfShippingLinePartyCode("", 0);

            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }
            //ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            VRN_JobOrder objJO = new VRN_JobOrder();
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstpickup = (List<VRN_PickupModel>)objIR.DBResponse.Data;
            }
            else
            {
                objJO.lstpickup = null;
            }

            objJO.JobOrderDate = DateTime.Now;
            // objJO.JobOrderDate =Convert.ToDateTime(DateTime.Now.ToString("dd/MMM/yyyy hh:mm")) ;
            objJO.TrainDate = DateTime.Now;
            return PartialView(objJO);
        }

        [HttpGet]
        public ActionResult GetAllTrainNo()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllTrainNo();
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderDetails()
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            IList<VRN_ImportJobOrderList> lstIJO = new List<VRN_ImportJobOrderList>();
            objIR.GetAllImpJO(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<VRN_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView(lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderDetails(string ContainerNo)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            IList<VRN_ImportJobOrderList> lstIJO = new List<VRN_ImportJobOrderList>();
            objIR.GetAllImpJO(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<VRN_ImportJobOrderList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderDetails", lstIJO);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            VRN_ImportRepository ObjCR = new VRN_ImportRepository();
            List<VRN_ImportJobOrderList> LstJO = new List<VRN_ImportJobOrderList>();
            ObjCR.GetAllImpJO(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<VRN_ImportJobOrderList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetPort()
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetAllPortForJobOrderTrasport();
            VRN_JobOrder objJO = new VRN_JobOrder();
            if (objIR.DBResponse.Data != null)
            {
                objJO.lstPort = (List<VRN_TransformList>)objIR.DBResponse.Data;
            }
            return Json(objJO.lstPort, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditJobOrder(int ImpJobOrderId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            VRN_JobOrder objImp = new VRN_JobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (VRN_JobOrder)objIR.DBResponse.Data;
            ViewBag.jdate = objImp.JobOrderDate;
            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<VRN_PickupModel>)objIR.DBResponse.Data;
            }
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

            objIR.GetAllTrainNo();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfTrain = objIR.DBResponse.Data;
            objIR.GetAllPortForJobOrderTrasport();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstPort = (List<VRN_TransformList>)objIR.DBResponse.Data;
            }

            return PartialView(objImp);
        }
        [HttpGet]
        public ActionResult ViewJobOrder(int ImpJobOrderId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetImpJODetails(ImpJobOrderId);
            VRN_JobOrder objImp = new VRN_JobOrder();
            if (objIR.DBResponse.Data != null)
                objImp = (VRN_JobOrder)objIR.DBResponse.Data;

            objIR.GetAllPickupLocation();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstpickup = (List<VRN_PickupModel>)objIR.DBResponse.Data;
            }
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            objIR.GetAllPortForJobOrderTrasport();
            if (objIR.DBResponse.Data != null)
            {
                objImp.lstPort = (List<VRN_TransformList>)objIR.DBResponse.Data;
            }
            return PartialView(objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteJobOrder(int ImpJobOrderId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.DeleteImpJO(ImpJobOrderId);
            return Json(objIR.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrder(VRN_JobOrder objImp, String FormOneDetails)
        {

            List<VRN_TrainDtl> lstDtl = new List<VRN_TrainDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VRN_TrainDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);

            }

            objIR.AddEditImpJO(objImp, XML);
            return Json(objIR.DBResponse);


        }
        [HttpGet]
        public JsonResult GetTrainDetl(int TrainSumId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetTrainDetailsOnEditMode(ImpJobOrderId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetYardWiseLocation(int YardId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetYardWiseLocation(YardId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Job Order Print
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintJO(int ImpJobOrderId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetImportJODetailsFrPrint(ImpJobOrderId);
            if (objIR.DBResponse.Data != null)
            {
                VRN_PrintJOModel objMdl = new VRN_PrintJOModel();
                objMdl = (VRN_PrintJOModel)objIR.DBResponse.Data;
                string Path = GeneratePDFForJO(objMdl, ImpJobOrderId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForJO(VRN_PrintJOModel objMdl, int ImpJobOrderId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/JobOrder" + ImpJobOrderId + ".pdf";
            string ContainerNo = "", Size = "", Serial = "", TrainNo = "", TrainDate = "", ContainerLoadType = "", CargoType = ""; int Count = 0;
            int Count40 = 0;
            int Count20 = 0;
            string Sline = "";
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
            objMdl.lstDet.ToList().ForEach(item =>
            {
                ContainerNo = (ContainerNo == "" ? (item.ContainerNo) : (ContainerNo + "<br/>" + item.ContainerNo));
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
            if (System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
            if ((Convert.ToInt32(Session["BranchId"])) == 1)
            {
                Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:left;'><br/>To,<br/>The Kandla International Container Terminal(KICT),<br/>Kandla</td></tr><tr><td colspan='2' style='text-align:center;'><br/>Shift the Import from <span style='border-bottom:1px solid #000;'> " + objMdl.FromLocation + " </span> to<span style='border-bottom:1px solid #000;'> CFS-KPT </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ShippingLineName + "</td><td style='border:1px solid #000;padding:5px;'>" + objMdl.ContainerType + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span>M/s Abrar Forwarders <br/>Gate Incharge,CWC KPT <br/>Custom PO,KICT Gate</span></td><td><br/><br/>Authorised Signature</td></tr></tbody></table></td></tr></tbody></table>";
            }
            else
            {
                Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><thead>  <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>IMPORT JOB ORDER</span></th></tr></tbody></table></td></tr></thead>   <tbody> <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td style='text-align:left; width:50%;'>Job Order No.<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderNo + "</span></td><td style='text-align:right;width:50%;'>Job Order Date<span style='border-bottom:1px solid #000;'> " + objMdl.JobOrderDate + "</span></td></tr></tbody></table></td></tr>   <tr><td colspan='12'><table style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><th>TO,</th></tr> <tr><th>The Manager(CT/OPERATIONS)</th></tr> <tr><td><span><br/></span></td></tr> <tr><td>" + objMdl.FromLocation + "</td></tr> <tr><td><span>&nbsp;&nbsp;</span>SIR,</td></tr> <tr><td><span>&nbsp;&nbsp;</span>YOU ARE REQUESTED TO KINDLY ARRANGE TO DELIVER THE FOLLOWING IMPORT CONTAINERS / CBT TO ICD PATPARGANJ, DELHI.</td></tr> </tbody></table></td></tr>      <tr><td colspan='12' style='text-align:center;'><br/></td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:7pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center; width:25px;'>SL.NO</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>CONTAINER / CBT NO.</th><th style='border:1px solid #000;padding:5px;text-align:center; width:60px;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>G/HAZ</th><th style='border:1px solid #000;padding:5px;text-align:center; width:100px;'>SLA CODE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Train No</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Train DATE</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>Origin</th><th style='border:1px solid #000;padding:5px;text-align:center; width:80px;'>F/L</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + CargoType + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + Sline + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.TrainNo + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.TrainDate + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + objMdl.FromLocation + "</td><td style='border:1px solid #000;padding:5px;text-align:center;'>" + ContainerLoadType + "</td></tr></tbody> </table></td></tr> <tr><td><span><br/></span></td></tr> <tr><td colspan='1'></td><th colspan='10'>TOTAL CONTAINERS / CBT : 20x " + Count20 + " + 40x " + Count40 + "</th></tr> <tr><td colspan='4'><table cellspacing='0' cellpadding='5' style='width:100%; font-size:8pt; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td colspan='2'></td><td width='15%' valign='top' align='right'>Note :</td><td colspan='2' width='85%'>THE FOLLOWING CONTAINERS / CBT ARE REQUIRED TO BE SCANNED BEFORE ITS DELIVERY FROM THE PORT AS DESIRED BY THE CUSTOM SCANNING DIVISION</td></tr></tbody></table></td></tr>   <tr><td colspan='12'><span><br/><br/></span></td></tr> <tr><td colspan='12' style='text-align:right;'>FOR MANAGER <br/> ICD PATPARGANJ</td></tr>  <tr><td colspan='12'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody> <tr><td><span><br/></span></td></tr>   <tr><td colspan='12'><table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to <br/> 1- M/S Suman Forwarding Agency Pvt.Ltd - for arranging movement of the Containers / CBT from " + objMdl.FromLocation + " within time. failing which dwell time charges as per procedure will be debited to your account as per claim receive from line.<br/> 2-The Preventive Office, Customs,ICD Waluj.</td></tr></tbody></table></td></tr>    </tbody></table></td></tr></tbody></table>";
            }
            // string Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>CONTAINER FREIGHT STATION<br/>18, COAL DOCK ROAD, KOLKATA - 700 043</span></th></tr></thead><tbody><tr><td style='text-align:left;'>No.<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderNo+"</span></td><td style='text-align:right;'>Date<span style='border-bottom:1px solid #000;'> "+objMdl.JobOrderDate+"</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>JOB ORDER</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/>Please arrange to bring / shift the Import / Export Load / Empty container</td></tr><tr><td colspan='2' style='text-align:left;'>from<span style='border-bottom:1px solid #000;'> "+objMdl.FromLocation+" </span> to<span style='border-bottom:1px solid #000;'> "+objMdl.ToLocation+" </span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>S. LINE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>LOADED / EMPTY</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>"+ Serial + "</td><td style='border:1px solid #000;padding:5px;'>"+ContainerNo+"</td><td style='border:1px solid #000;padding:5px;'>"+Size+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ShippingLineName+"</td><td style='border:1px solid #000;padding:5px;'>"+objMdl.ContainerType+"</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td>Copy to:- <span></span></td><td><br/><br/>SR.ASSTT.MANAGER</td></tr></tbody></table></td></tr></tbody></table>";
            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                rh.GeneratePDF(Path, Html);
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
            VRN_SealCutting SC = new VRN_SealCutting();
            SC.TransactionDate = DateTime.Now.ToString("dd/MM/yyyy");


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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetListOfSealCuttingDetails(0);
            IList<VRN_SealCutting> lstSC = new List<VRN_SealCutting>();
            if (objIR.DBResponse.Data != null)
                lstSC = (List<VRN_SealCutting>)objIR.DBResponse.Data;
            return PartialView("ListOfSealCuttingDetails", lstSC);

        }
        [HttpGet]
        public JsonResult LoadListMoreDataForSealCutting(int Page)
        {
            VRN_ImportRepository ObjCR = new VRN_ImportRepository();
            List<VRN_SealCutting> LstJO = new List<VRN_SealCutting>();
            ObjCR.GetListOfSealCuttingDetails(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<VRN_SealCutting>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSealCuttingById(int SealCuttingId)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetSealCuttingById(SealCuttingId);
            VRN_SealCutting objImp = new VRN_SealCutting();
            if (objImport.DBResponse.Data != null)
                objImp = (VRN_SealCutting)objImport.DBResponse.Data;
            return PartialView("EditSealCutting", objImp);
        }
        [HttpGet]
        public ActionResult ViewSealCuttingDetailById(int SealCuttingId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetSealCuttingById(SealCuttingId);
            VRN_SealCutting objImp = new VRN_SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (VRN_SealCutting)objIR.DBResponse.Data;
            return PartialView("ViewSealCuttingDetailById", objImp);
        }

        [HttpGet]
        public JsonResult GetInvoiceDtlForSealCutting(String TransactionDate, String GateInDate, String ContainerNo, String size, int CHAShippingLineId, String CFSCode, int OBLType, int CargoType)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetInvoiceDtlForSealCutting(TransactionDate, GateInDate, ContainerNo, size, CHAShippingLineId, CFSCode, OBLType, CargoType);
            VRN_SealCutting objImp = new VRN_SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (VRN_SealCutting)objIR.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfContainer()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.ListOfContainer();
            List<VRN_SealCutting> objImp = new List<VRN_SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<VRN_SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfBLData(int OBLId, int impobldtlId, string OBLFCLLCL, string CFSCode)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.ListOfBL(OBLId, impobldtlId, OBLFCLLCL, CFSCode);


            List<SealCutting> objImp = new List<SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfGodownData()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.ListOfGodownRights(((Login)(Session["LoginUser"])).Uid);


            List<GodownList> objImp = new List<GodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<GodownList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCHAShippingLineData()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.ListOfCHAShippingLine();

            List<SealCuttingCHA> objImp = new List<SealCuttingCHA>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<SealCuttingCHA>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult SealCuttingInvoicePrint(int InvoiceId)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetSCInvoiceDetailsForPrint(InvoiceId);
            PpgInvoiceSealCutting objSC = new PpgInvoiceSealCutting();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objSC = (PpgInvoiceSealCutting)objGPR.DBResponse.Data;
                FilePath = GeneratingPDF(objSC, InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        private string GeneratingPDF(PpgInvoiceSealCutting objSC, int InvoiceId)
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
        public JsonResult AddEditSealCutting(VRN_SealCutting objSealCutting)
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
                    var ViewBLList = JsonConvert.DeserializeObject<List<VRN_SealCutting>>(objSealCutting.ViewBLList.ToString());
                    if (ViewBLList != null)
                    {
                        OBLXML = Utility.CreateXML(ViewBLList);
                    }
                }

                VRN_ImportRepository objImport = new VRN_ImportRepository();
                objImport.AddEditSealCutting(objSealCutting, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, OBLXML);
                //ModelState.Clear();
                return Json(objImport.DBResponse);

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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.DeleteSealCutting(SealCuttingId);

            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tally Sheet Generation For LCL
        [HttpGet]
        public ActionResult GetTallySheet()
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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
        public JsonResult AddEditTallySheet(VRN_TallySheet objSheet)
        {
            if (ModelState.IsValid)
            {
                VRN_ImportRepository objIR = new VRN_ImportRepository();
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.ListOfTallySheet(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid, 0);
            IList<TallySheetList> lstTally = new List<TallySheetList>();
            if (objIR.DBResponse.Data != null)
                lstTally = (List<TallySheetList>)objIR.DBResponse.Data;
            return PartialView(lstTally);
        }
        [HttpGet]
        public JsonResult GetTallySheetListForPage(int Page)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.ListOfTallySheet(Convert.ToInt32(Session["BranchId"]), ((Login)(Session["LoginUser"])).Uid, Page);
            IList<TallySheetList> lstTally = new List<TallySheetList>();
            if (objIR.DBResponse.Data != null)
                lstTally = (List<TallySheetList>)objIR.DBResponse.Data;
            return Json(lstTally, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewTallySheet(int TallySheetId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetTallySheet(Convert.ToInt32(Session["BranchId"]), TallySheetId, ((Login)(Session["LoginUser"])).Uid);
            VRN_TallySheet objTallySheet = new VRN_TallySheet();
            if (objIR.DBResponse.Data != null)
                objTallySheet = (VRN_TallySheet)objIR.DBResponse.Data;
            return PartialView(objTallySheet);
        }
        [HttpGet]
        public ActionResult EditTallySheet(int TallySheetId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetTallySheet(Convert.ToInt32(Session["BranchId"]), TallySheetId, ((Login)(Session["LoginUser"])).Uid);
            VRN_TallySheet objTallySheet = new VRN_TallySheet();
            if (objIR.DBResponse.Data != null)
                objTallySheet = (VRN_TallySheet)objIR.DBResponse.Data;
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetOblContDet(Convert.ToInt32(Session["BranchId"]), SealCuttingId, ((Login)(Session["LoginUser"])).Uid);
            if (objIR.DBResponse.Data != null)
                return Json(objIR.DBResponse.Data, JsonRequestBehavior.AllowGet);
            else return Json(objIR.DBResponse.Data, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteTallySheet(int TallySheetId)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.DeleteTallySheet(TallySheetId, Convert.ToInt32(Session["BranchId"]));
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult PrintTallySheet(int TallySheetId)
        {
            VRN_ImportRepository objIr = new VRN_ImportRepository();
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
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.getCompanyDetails();
            string Comp = "", Add = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                Comp = objCompanyDetails.CompanyName;
                Add = objCompanyDetails.CompanyAddress;
            }
            html += "<tr><td colspan='12'>";
            html += "<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody>";
            html += "<tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td>";
            // html += "<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>ICD Patparganj-Delhi</label><br/><label style='font-size: 16px; font-weight:bold;'>TALLY SHEET</label></td></tr>";
            html += ("<td colspan='10' width='90%' valign='top' align='center'><h1 style='font-size: 20px; line-height:30px; margin:0; padding:0;'>" + Comp + "</h1>");
            html += ("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html += ("<span style='font-size: 12px; padding-bottom: 10px;'>" + Add + "</span>");
            html += "</td></tr>";
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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
            VRN_ImportRepository objIR = new VRN_ImportRepository();
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            VRN_CustomAppraiseApproval ObjAppId = new VRN_CustomAppraiseApproval();
            ObjIR.GetCstmAppraiseApplication(CstmAppraiseAppId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjAppId = (VRN_CustomAppraiseApproval)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraisementApproval", ObjAppId);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApproval(int CstmAppraiseAppId, int IsApproved)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.UpdateCustomApproval(CstmAppraiseAppId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

        #region Destuffing LCL
        [HttpGet]
        public ActionResult CreateDestuffingEntry()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            VRN_DestuffingEntry ObjDestuffing = new VRN_DestuffingEntry();
            ObjDestuffing.DestuffingEntryDate = DateTime.Now.ToString("dd/MM/yyyy");
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
            ObjIR.GetAllCommodityForPage("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }

            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView("GetDestuffingEntry", ObjDestuffing);
        }


        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(int TallySheetId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetContrDetForDestuffingEntry(TallySheetId, Convert.ToInt32(Session["BranchId"]));
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntry(int Page)
        {
            VRN_ImportRepository ObjCR = new VRN_ImportRepository();
            List<VRN_DestuffingList> LstJO = new List<VRN_DestuffingList>();
            ObjCR.GetAllDestuffingEntry(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<VRN_DestuffingList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<VRN_DestuffingList> LstDestuffing = new List<VRN_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<VRN_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {

            VRN_DestuffingEntry ObjDestuffing = new VRN_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
                ObjIR.GetAllCommodityForPage("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "Edit");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (VRN_DestuffingEntry)ObjIR.DBResponse.Data;
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
            VRN_DestuffingEntry ObjDestuffing = new VRN_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "View");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (VRN_DestuffingEntry)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewDestuffingEntry", ObjDestuffing);
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
        public JsonResult AddEditDestuffingEntry(VRN_DestuffingEntry ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                List<VRN_DestuffingEntryDtl> LstDestuffingEntry = new List<VRN_DestuffingEntryDtl>();
                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VRN_DestuffingEntryDtl>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                }
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.AddEditDestuffingEntry(ObjDestuffing, DestuffingEntryXML /*, GodownXML, ClearLcoationXML*/ , Convert.ToInt32(Session["BranchId"]), Convert.ToInt32(((Login)(Session["LoginUser"])).Uid));
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetDestuffEntryForPrint(DestuffingEntryId);
            if (ObjIR.DBResponse.Data != null)
            {
                VRN_DestuffingSheet ObjDestuff = new VRN_DestuffingSheet();
                ObjDestuff = (VRN_DestuffingSheet)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForDestuffSheet(ObjDestuff, DestuffingEntryId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePDFForDestuffSheet(VRN_DestuffingSheet ObjDestuff, int DestuffingEntryId)
        {


            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            VRN_ReportRepository ObjRR = new VRN_ReportRepository();
            ObjRR.getCompanyDetails();
            string Comp = "", Add = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                Comp = objCompanyDetails.CompanyName;
                Add = objCompanyDetails.CompanyAddress;
            }

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
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION </h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'></label><label style='font-size: 14px;'>" + Add + "</label><br/><label style='font-size: 16px; font-weight:bold;'>DESTUFFING SHEET</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
            objSB.Append("<th style='font-size:13px;width:10%'>SHED CODE:</th><td style='font-size:12px;'>" + ObjDestuff.GodownName + "</td>");
            objSB.Append("<th style='font-size:13px; text-align:right;'>AS ON:</th><td style='font-size:12px; width:10%;'>" + ObjDestuff.DestuffingEntryDateTime + "</td>");
            objSB.Append("</tr></tbody></table></td></tr>");


            objSB.Append("<tr><td colspan='12'>");
            objSB.Append("<table style='width:100%; margin: 0;margin-bottom: 10px;'><tbody><tr><td style='font-size: 11px; padding-bottom:15px;'>");
            objSB.Append("<label style='font-weight: bold;'>DESTUFF SHEET NO.:</label> <span>" + ObjDestuff.DestuffingEntryNo + "</span></td>");
            objSB.Append("<td colspan='2' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>START DATE OF DESTUFFING : </label> <span>" + ObjDestuff.StartDate + "</span></td>");
            objSB.Append("<td colspan='3' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>FINAL DATE OF DESTUFFING : </label> <span>" + ObjDestuff.DestuffingEntryDate + "</span></td></tr>");
            objSB.Append("<tr><td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Container / CBT No.</label> <span>" + ObjDestuff.ContainerNo + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Size : </label> <span>" + ObjDestuff.Size + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>In Date : </label> <span>" + ObjDestuff.GateInDate + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Custom Seal No. : </label> <span>" + ObjDestuff.CustomSealNo + "</span></td>");
            objSB.Append("<td colspan='2' style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>Sla Seal no. : </label> <span>" + ObjDestuff.SlaSealNo + "</span></td></tr>");
            objSB.Append("<tr><td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>ICD Code</label> <span>" + ObjDestuff.CFSCode + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>IGM No. : </label> <span>" + ObjDestuff.IGMNo + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>OBL Type: </label> <span>" + ObjDestuff.MovementType + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>SLA : </label> <span>" + ObjDestuff.ShippingLine + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>POL : </label> <span>" + ObjDestuff.POL + "</span></td>");
            objSB.Append("<td style='font-size: 11px; padding-bottom:15px;'><label style='font-weight: bold;'>POD : </label> <span>" + ObjDestuff.POD + "</span></td></tr>");

            objSB.Append("</tbody></table></td></tr><tr><td colspan='12'><table style='width:100%; margin-bottom: 10px;'><tbody>");
            objSB.Append("<tr><td colspan='12'><table style='border:1px solid #000; font-size:8pt; border-bottom: 0; width:100%;border-collapse:collapse;'><thead><tr>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SR No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>SMTP No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>OBL No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Importer</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Cargo</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>Type</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>No. Pkg</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Pkg Rec</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Gr Wt</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>Slot No.</th>");
            objSB.Append("<th style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>Area</th>");
            objSB.Append("<th style=' border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>Remarks</th>");
            objSB.Append("</tr></thead><tfoot><tr>");
            objSB.Append("<td colspan='6' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-weight: bold; text-align: center; padding: 5px;'>");
            objSB.Append("</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.NoOfPkg)) + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px;'>" + Convert.ToInt32(ObjDestuff.lstDtl.Sum(x => x.PkgRec)) + "</td>");
            objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: left; padding: 5px; ' colspan='2'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Weight)) + "</td>");
            objSB.Append("<td style='border-bottom: 1px solid #000; text-align: left;' colspan='2'>" + Convert.ToDecimal(ObjDestuff.lstDtl.Sum(x => x.Area)) + "</td></tr></tfoot><tbody>");
            int Serial = 1;
            ObjDestuff.lstDtl.ToList().ForEach(item =>
            {
                objSB.Append("<tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + Serial + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.SMTPNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.OblNo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Importer + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Cargo + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 5%;'>" + item.Type + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.NoOfPkg + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.PkgRec + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.Weight + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 6%;'>" + item.GodownWiseLctnNames + "</td>");
                objSB.Append("<td style='border-right: 1px solid #000; border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 8%;'>" + item.Area + "</td>");
                objSB.Append("<td style='border-bottom: 1px solid #000; text-align: center; padding: 5px; width: 10%;'>" + item.Remarks.Replace("&", " and ").ToString() + "</td>");
                objSB.Append("</tr>");
                Serial++;
            });
            objSB.Append("</tbody></table></td></tr><tr>");
            objSB.Append("<td colspan='12' style=' font-size: 11px; padding-top: 15px; text-align: left;'>*GOODS RECEIVED ON S/C &amp; S/W BASIC - CWC IS NOT RESPONSIBLE FOR SHORT LANDING &amp; LEAKAGES IF ANY</td>");
            objSB.Append("</tr><tr><td colspan='12' style=' font-size: 12px; text-align: left;padding-top: 15px;'>Signature &amp; Designation :</td></tr></tbody>");
            objSB.Append("</table></td></tr>");
            objSB.Append("<tr><td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>H &amp; T Agent</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Consignee</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Shipping Line</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>ICD</td>");
            objSB.Append("<td style=' font-size: 12px; width: 20%; text-align: center;padding-top: 100px;'>Customs</td></tr>");
            objSB.Append("</tbody></table>");

            string HeadOffice = "", HOAddress = "", ZonalOffice = "", ZOAddress = "";
            if (ObjRR.DBResponse.Data != null)
            {
                objCompanyDetails = (CompanyDetailsForReport)ObjRR.DBResponse.Data;
                ZonalOffice = objCompanyDetails.CompanyName;
                ZOAddress = objCompanyDetails.CompanyAddress;
            }

            objSB.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var RH = new ReportingHelper(PdfPageSize.A4Landscape, 40f, 40f, 40f, 40f, false, true))
            {
                RH.HeadOffice = HeadOffice;
                RH.HOAddress = HOAddress;
                RH.ZonalOffice = ZonalOffice;
                RH.ZOAddress = ZOAddress;


                RH.GeneratePDF(Path, objSB.ToString());
            }
            return "/Docs/" + Session.SessionID + "/DestuffingSheet" + DestuffingEntryId + ".pdf";
        }
        [HttpGet]
        public JsonResult GetCIFandDutyForBOE(string BOENo, string BOEDate)
        {
            VRN_ImportRepository objIR = new VRN_ImportRepository();
            objIR.GetCIFandDutyForBOE(BOENo, BOEDate);
            if (objIR.DBResponse.Data != null)
                return Json(new { Status = 1, Message = "Success", Data = objIR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = 0, Message = "No Data", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            VRN_ImportRepository objRepo = new VRN_ImportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryListSearch(string ContainerNo)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<VRN_DestuffingList> LstDestuffing = new List<VRN_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(((Login)(Session["LoginUser"])).Uid, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<VRN_DestuffingList>)ObjIR.DBResponse.Data;
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
            AMD_ImportRepository objImport = new AMD_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;



            return PartialView();
        }
        [HttpGet]
        public JsonResult LoadPartyListFCL(int Page)
        {
            AMD_ImportRepository objimp = new AMD_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeFCL(string PartyCode)
        {
            AMD_ImportRepository objimp = new AMD_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string TaxType, string SEZ, string CustomExam,
            List<PaymentSheetContainer> lstPaySheetContainer, string Movement, int OTHours = 0, int PartyId = 0, int PayeeId = 0,
            int InvoiceId = 0, int isdirect = 0, int ReeferHours = 0, int Distance = 0, int IsFranchise = 0, int IsOnWheel = 0, int IsReworking = 0, 
            int IsCargoShifting = 0, int IsLiftOnOff = 0, int IsSweeping = 0, int IsHandling = 0)
        {            
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
            }

            AMD_ImportRepository objPpgRepo = new AMD_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, TaxType, XMLText, Movement, InvoiceId, OTHours, PartyId, PayeeId, isdirect, SEZ, ReeferHours, CustomExam, Distance,
                IsFranchise, IsOnWheel, IsReworking, IsCargoShifting, IsLiftOnOff, IsSweeping, IsHandling);
            var Output = (VRN_InvoiceYard)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
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
        public JsonResult AddEditContPaymentSheet(String InvoiceObj, int IsDirect, string Movement, string SEZ, string CustomExam,
            int IsFranchise, int IsOnWheel, int IsReworking, int IsCargoShifting, int IsLiftOnOff, int IsSweeping, int IsHandling,
            int ReeferHours = 0, int Distance = 0 )
        {
            try
            {
                //PpgInvoiceYard objForm;
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<VRN_InvoiceYard>(InvoiceObj);
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
                //if (invoiceData.lstPostPaymentChrg != null)
                //{
                //    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                //}

                if (invoiceData.lstPostPaymentChrgXML != null)
                {
                    // ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                    invoiceData.lstPostPaymentChrg = JsonConvert.DeserializeObject<List<VRN_PostPaymentChrg>>(invoiceData.lstPostPaymentChrgXML);
                    ChargesXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                //if (invoiceData.lstContWiseAmount != null)
                //{
                //    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);
                //}
                if (invoiceData.lstContWiseAmountXML != null)
                {
                    // ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                    invoiceData.lstContWiseAmount = JsonConvert.DeserializeObject<List<VRN_ContainerWiseAmount>>(invoiceData.lstContWiseAmountXML);
                    ContWiseCharg = Utility.CreateXML(invoiceData.lstContWiseAmount);

                }
                //if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                //{
                //    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                //}

                if (invoiceData.lstOperationCFSCodeWiseAmountXML != null)
                {

                    invoiceData.lstOperationCFSCodeWiseAmount = JsonConvert.DeserializeObject<List<VRN_OperationCFSCodeWiseAmount>>(invoiceData.lstOperationCFSCodeWiseAmountXML);
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                    //  OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }

                //if (invoiceData.lstPostPaymentChrgBreakup != null)
                //{
                //    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                //}

                if (invoiceData.lstPostPaymentChrgBreakupXML != null)
                {

                    invoiceData.lstPostPaymentChrgBreakup = JsonConvert.DeserializeObject<List<VRN_PostPaymentChargebreakupdate>>(invoiceData.lstPostPaymentChrgBreakupXML);
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                    //ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrgBreakup);
                }

                AMD_ImportRepository objChargeMaster = new AMD_ImportRepository();
                objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, IsDirect, SEZ, CustomExam, "IMPYard", Movement, ReeferHours, Distance,
                    IsFranchise, IsOnWheel, IsReworking, IsCargoShifting,IsLiftOnOff,IsSweeping,IsHandling);

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
        public ActionResult ListOfYardInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            AMD_ImportRepository objER = new AMD_ImportRepository();
            objER.ListOfYardInvoice(Module, InvoiceNo, InvoiceDate);
            List<CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice> obj = new List<CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CwcExim.Areas.Export.Models.VRN_ListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView(obj);
        }

        public async Task<JsonResult> GetIRNForYardInvoice(String InvoiceNo, string SupplyType)
        {


            VRN_ImportRepository objPpgRepo = new VRN_ImportRepository();
            //objChrgRepo.GetAllCharges();
            if (SupplyType == "B2B" || SupplyType == "SEZWP" || SupplyType == "SEZWOP")
            {
                objPpgRepo.GetIRNForYard(InvoiceNo);
                var Output = (EinvoicePayload)objPpgRepo.DBResponse.Data;

                if (Output.BuyerDtls.Gstin != "" && Output.BuyerDtls.Gstin != null)
                {
                    objPpgRepo.GetHeaderIRNForYard();

                    HeaderParam Hp = (HeaderParam)objPpgRepo.DBResponse.Data;

                    string jsonEInvoice = JsonConvert.SerializeObject(Output);
                    string jsonEInvoiceHeader = JsonConvert.SerializeObject(Hp);
                    log.Info("Before calling GenerateEinvoice");
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
                        log.Info("Before calling AddEditIRNB2C");
                        objPpgRepo.AddEditIRNResponsec(ERes, InvoiceNo);
                        log.Info("after calling AddEditIRNB2C");
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
                    VRN_IrnB2CDetails irnb2cobj = new VRN_IrnB2CDetails();
                    irnb2cobj = (VRN_IrnB2CDetails)objPpgRepo.DBResponse.Data;
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
                VRN_IrnB2CDetails irnb2cobj = new VRN_IrnB2CDetails();
                irnb2cobj = (VRN_IrnB2CDetails)objPpgRepo.DBResponse.Data;
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

        #region Internal Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
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
                ViewBag.BOENoList = new SelectList((List<PPG_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
            }
            else
            {
                ViewBag.BOENoList = null;
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.GetAllInternalMovement();
            List<PPG_Internal_Movement> LstMovement = new List<PPG_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<PPG_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            PPG_Internal_Movement ObjInternalMovement = new PPG_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (PPG_Internal_Movement)ObjIR.DBResponse.Data;
                ObjIR.ListOfGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.GodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }


                ObjIR.GetBOENoForInternalMovement();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.BOENoList = new SelectList((List<PPG_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
                }
                else
                {
                    ViewBag.BOENoList = null;
                }


            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public ActionResult ViewInternalMovement(int MovementId)
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

        [HttpGet]
        public JsonResult GetBOENoDetails(int DestuffingEntryDtlId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.GetBOENoDetForMovement(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int DestuffingId, string OBLNo, String MovementDate,
            string InvoiceType, int DestLocationIdiceId, int InvoiceId = 0)
        {

            Ppg_ImportRepository objChrgRepo = new Ppg_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objChrgRepo.GetInternalPaymentSheetInvoice(DestuffingId, OBLNo, MovementDate, InvoiceType, DestLocationIdiceId, InvoiceId);

            var Output = (PPGInvoiceGodown)objChrgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new PpgPostPaymentContainer
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
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "IMPMovement");
            PpgInvoiceYard objGP = new PpgInvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (PpgInvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceMovement(PpgInvoiceYard objGP, int InvoiceId)
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
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
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


            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='6'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");

            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.ShippingLineName.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");
            //html = "<table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; '><tbody> <tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>Central Warehousing Corporation</h1><label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span></td></tr><tr><td style='text-align: right;'><span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>CWC GST No. <label>07AAACC1206D1ZI</label></span></td></tr><tr><td><table style=' border: 1px solid #000; padding: 10px; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'><tbody><tr><td><h2 style='font-size: 18px; padding-bottom: 10px; text-align: center;margin: 0; padding: 0;'>Tax Invoice</h2> </td></tr><tr><td><table style='width:100%; margin-bottom: 10px;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice No.</label> <span>11071/96365</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Invoice Date : </label> <span>14-MAR-18</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Name : </label> <span>FREIGHTBRIDGE LOGISTICS PVT LTD</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State :</label> <span>Delhi</span> </td></tr><tr><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>Party Address :</label> <span>501, 5th Floor, 56 Eros Apartment</span></td><td style='font-size: 13px; line-height: 26px;'><label style='font-weight: bold;'>State Code :</label> <span>DL</span></td></tr><tr><td style='font-size: 13px; line-height: 26px;' colspan='2'><label style='font-weight: bold;'>Party GST :</label> <span>07AAACF8646G1ZJ</span></td></tr></tbody> </table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 10px;'>Container Details :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CFS Code</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Container No.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Size</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>From Date</th><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>To Date</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr><tr><th><h3 style='text-align: left; font-size: 14px;margin-top: 20px;'>Container Charges :</h3> </th></tr><tr><td><table style=' border: 1px solid #000; border-bottom: 0; width:100%;' cellspacing='0' cellpadding='0'><thead><tr><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SR No.</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Charge Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Description</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>HSN Code</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th rowspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Taxable Amt.</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>CGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>SGST</th><th colspan='2' style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>IGST</th><th rowspan='2' style='border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Total</th></tr><tr><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Rate</th><th style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>Amt.</th></tr></thead><tbody><tr><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>--</td><td style='border-right: 1px solid #000; border-bottom: 1px solid #000; font-size: 12px; text-align: center; padding: 5px;'>-</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: center;'>--</td></tr></tbody></table></td></tr></tbody></table><table style=' border: 1px solid #000; width:100%; margin-bottom: 10px;'cellspacing='0' cellpadding='0'> <tbody><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>7200</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>648</td><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>8496</td></tr><tr><th style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;'>Total Invoice (In Word) :</th><td style='border-bottom: 1px solid #000; font-size: 12px; text-align: left; padding: 5px;' colspan='4'>Eight Thousand Four Hundred Ninety Six Rupees Only</td></tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'>Amount of Tax Subject of Reverse :</th><td style='font-size: 12px; text-align: left; padding: 5px;' colspan='4'>0</td></tr></table><table style='width:100%;' cellspacing='0' cellpadding='0'><tbody><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>Receipt No.: <label style='font-weight: bold;'></label></td><td style='font-size: 11px; text-align: left; line-height: 30px;'>Party Code: <label style='font-weight: bold;'></label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table></td></tr></tbody></table>";

            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/MovementInvoice" + InvoiceId.ToString() + ".pdf";
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditInternalPaymentSheet(PPG_Internal_Movement objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);



                Ppg_ImportRepository objChargeMaster = new Ppg_ImportRepository();
                objChargeMaster.AddEditInvoiceMovement(objForm, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPMovement");

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
        public JsonResult AddEditInternalMovement(PPG_Internal_Movement ObjInternalMovement)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
                ObjIR.AddEditImpInternalMovement(ObjInternalMovement);
                return Json(ObjIR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }

        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DelInternalMovement(int MovementId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.DelInternalMovement(MovementId);
            return Json(ObjIR.DBResponse);
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

        #region Delivery Application

        [HttpGet]
        public ActionResult CreateDeliveryApplication()
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            BondRepository ObjBR = new BondRepository();
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            ObjIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.RightsList = JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
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
            Ppg_ImportRepository objcash = new Ppg_ImportRepository();
            objcash.ListOfIssueParty();
            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<PPGDeliveryOrdDtl>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { IssueBy = item.IssuedBy });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListOfCargoParty()
        {
            Ppg_ImportRepository objcash = new Ppg_ImportRepository();
            objcash.ListOfIssueParty();

            List<dynamic> objImp2 = new List<dynamic>();
            if (objcash.DBResponse.Data != null)
                ((List<PPGDeliveryOrdDtl>)objcash.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { IssueBy = item.IssuedBy });
                });

            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditDeliveryApplication(int DeliveryId)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            BondRepository ObjBR = new BondRepository();
            PPG_DeliverApplication ObjDelivery = new PPG_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (PPG_DeliverApplication)ObjIR.DBResponse.Data;
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            PPG_DeliverApplication ObjDelivery = new PPG_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (PPG_DeliverApplication)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryApplication()
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            List<PPGDeliveryApplicationList> LstDelivery = new List<PPGDeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<PPGDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDeliveryApp(int Page)
        {
            Ppg_ImportRepository ObjCR = new Ppg_ImportRepository();
            List<PPGDeliveryApplicationList> LstJO = new List<PPGDeliveryApplicationList>();
            ObjCR.GetAllDeliveryApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<PPGDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryApplication(PPG_DeliverApplication ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<PPGDeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<PPGDeliveryOrdDtl>>(ObjDelivery.DeliveryOrdDtlXml);
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCIFFromOOC(String BOE, String BOEDT)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.GetCIFFromOOCDelivery(BOE, BOEDT);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Delivery Payment Sheet
        [HttpGet]
        public ActionResult CreateDeliveryPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetDeliveryApplicationForImpPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;

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
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPaymentSheetDeliveryBOE(int AppraisementId)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetBOEForDeliveryPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.BOEList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.BOEList = null;

            return Json(ViewBag.BOEList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetDeliveryPaymentSheet(string InvoiceDate, string ExportUnder, string InvoiceType, int AppraisementId, int DeliveryType, string AppraisementNo, string AppraisementDate,
            int PartyId, string PartyName, string PartyAddress, string PartyState, string PartyStateCode, string PartyGST, int PayeeId,
            string PayeeName, List<VRN_PaymentSheetBOE> lstPaySheetBOE, int OTHours = 0, int InvoiceId = 0)
        {
            string XMLText = "";
            if (lstPaySheetBOE != null)
            {
                XMLText = Utility.CreateXML(lstPaySheetBOE);
            }

            VRN_ImportRepository objChrgRepo = new VRN_ImportRepository();

            objChrgRepo.GetDeliveryPaymentSheet_Patparganj(InvoiceDate, ExportUnder, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
                    PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceId, OTHours);


            var Output = (VRN_InvoiceGodown)objChrgRepo.DBResponse.Data;
            VRN_tentativeinvoice.InvoiceObj = (VRN_InvoiceGodown)objChrgRepo.DBResponse.Data;
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
                    Output.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
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
            VRN_tentativeinvoice.InvoiceObj = Output;
            return Json(Output, JsonRequestBehavior.AllowGet);




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);


                var invoiceData = JsonConvert.DeserializeObject<VRN_InvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
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
                VRN_ImportRepository objChargeMaster = new VRN_ImportRepository();
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



        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult GodownInvoicePrint(string InvoiceNo)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetInvoiceDetailsForGodownPrintByNo(InvoiceNo, "IMPDeli");
            VRN_InvoiceYard objGP = new VRN_InvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (VRN_InvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceGodown(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        private string GeneratingPDFInvoiceGodown(VRN_InvoiceYard objGP, int InvoiceId)
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
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
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

            html.Append("" + ctwObj.changeCurrencyToWords(objGP.TotalAmt.ToString()) + "</th>");
            html.Append("</tr><tr><th style='font-size: 12px; text-align: left; padding: 5px;'  colspan='6'>Amount of Tax Subject of Reverse :");
            html.Append("0</th>");

            html.Append("</tr></tbody></table><table style='width:100%;' cellspacing='0' cellpadding='10'>");
            html.Append("<tbody><tr><td style='font-size: 12px; text-align: left; line-height: 30px;'>Receipt No.: ");
            html.Append("<label style='font-weight: bold;'></label></td><td style='font-size: 12px; text-align: left; line-height: 30px;'>Party Code:");
            html.Append("<label style='font-weight: bold;'>" + objGP.PartyCode.ToString() + "</label></td></tr><tr><td style='font-size: 11px; text-align: left; line-height: 30px;'>");
            html.Append("*Cheques are subject to realisation</td><td style='font-size: 11px; text-align: left; line-height: 30px;'>SAM(A/C)</td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table>");


            using (var rp = new ReportingHelper(PdfPageSize.A4, 20f, 20f, 20f, 20f))
            {
                rp.GeneratePDF(location, html.ToString());
            }
            return "/Docs/" + Session.SessionID + "/GodownInvoice" + InvoiceId.ToString() + ".pdf";
        }


        #endregion

        #region FCL To LCL Conversion

        [HttpGet]
        public ActionResult AddFCLtoLCLConversion()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.ListOfContainerFCLtoLCL();
            List<FCLtoLCLContainerList> objImp = new List<FCLtoLCLContainerList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<FCLtoLCLContainerList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPartyPdaForFCLtoLCL()
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();


            objImport.GetPartyPdaForFCLtoLCL();
            List<FCLtoLCLForwarderList> objImp = new List<FCLtoLCLForwarderList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<FCLtoLCLForwarderList>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSLA()
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetPartyPdaDetailsForFCLtoLCL(Size, PartyPdaId, ContainerClassId, CFSCode);
            FCLtoLCLConversion objImp = new FCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (FCLtoLCLConversion)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddFCLtoLCLConversion(FCLtoLCLConversion objFCLLCL)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                string ChargesBreakupXML = "";
                if (objFCLLCL.lstPostPaymentChrgBreakup != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(objFCLLCL.lstPostPaymentChrgBreakup);
                }

                Ppg_ImportRepository objImport = new Ppg_ImportRepository();
                objImport.AddFCLtoLCLConversion(objFCLLCL, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid);
                ModelState.Clear();
                return Json(objImport.DBResponse);

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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetListOfFCLToLCLConversionDtl();
            List<FCLtoLCLConversion> objImp = new List<FCLtoLCLConversion>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<FCLtoLCLConversion>)objImport.DBResponse.Data;
            return PartialView("ListOfFCLtoLCLConversion", objImp);
        }

        [HttpGet]
        public ActionResult ViewFCLtoLCLConversionbyId(int FCLtoLCLConversionId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.ViewFCLtoLCLConversionbyId(FCLtoLCLConversionId);
            FCLtoLCLConversion objImp = new FCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (FCLtoLCLConversion)objImport.DBResponse.Data;
            return PartialView("ViewFCLtoLCLConversion", objImp);
        }
        [HttpGet]
        public ActionResult EditFCLtoLCLConversionbyId(int FCLtoLCLConversionId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.ViewFCLtoLCLConversionbyId(FCLtoLCLConversionId);
            FCLtoLCLConversion objImp = new FCLtoLCLConversion();
            if (objImport.DBResponse.Data != null)
                objImp = (FCLtoLCLConversion)objImport.DBResponse.Data;
            return PartialView("EditFCLtoLCLConversion", objImp);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult FCLToLCLConversionInvoicePrint(int InvoiceId)
        {
            PpgInvoiceRepository objGPR = new PpgInvoiceRepository();
            objGPR.GetFCLToLCLConversionInvoiceDtlForPrint(InvoiceId);
            PpgInvoiceFCLToLCLConversion objFCLLCL = new PpgInvoiceFCLToLCLConversion();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objFCLLCL = (PpgInvoiceFCLToLCLConversion)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFForFCLToLCLConversion(objFCLLCL, InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }

        [NonAction]
        private string GeneratingPDFForFCLToLCLConversion(PpgInvoiceFCLToLCLConversion objSC, int InvoiceId)
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
            Ppg_ImportRepository objImp = new Ppg_ImportRepository();
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
            PPG_FormOneModel objFormOne = new PPG_FormOneModel();
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objFormOne.FormOneDate = DateTime.Now.ToString("dd/MM/yyyy");
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int MenuId = Convert.ToInt32(Session["MenuId"]);
            int ModuleId = Convert.ToInt32(Session["ModuleId"]);
            objIR.MenuAccessRight(RoleId, BranchId, ModuleId, MenuId);
            if (objIR.DBResponse.Data != null)
            {

                ViewBag.RightsList = JsonConvert.SerializeObject(objIR.DBResponse.Data);

            }
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
                objFormOne.lstCommodity = (List<Import.Models.PPG_Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult GetFormOneList(string ContainerName = "")
        {
            if (ContainerName == null || ContainerName == "")
            {
                IEnumerable<PPG_FormOneModel> lstFormOne = new List<PPG_FormOneModel>();
                Ppg_ImportRepository objImportRepo = new Ppg_ImportRepository();
                objImportRepo.GetFormOne();
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<PPG_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
            else
            {
                IEnumerable<PPG_FormOneModel> lstFormOne = new List<PPG_FormOneModel>();
                Ppg_ImportRepository objImportRepo = new Ppg_ImportRepository();
                objImportRepo.GetFormOneByContainer(ContainerName);
                if (objImportRepo.DBResponse.Data != null)
                    lstFormOne = (IEnumerable<PPG_FormOneModel>)objImportRepo.DBResponse.Data;

                return PartialView(lstFormOne);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFormOne(PPG_FormOneModel objFormOne)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
                if (ModelState.IsValid)
                {

                    objFormOne.lstFormOneDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPG_FormOneDetail>>(objFormOne.FormOneDetailsJS);
                    objFormOne.lstFormOneDetail.ToList().ForEach(item =>
                    {
                        item.CargoDesc = string.IsNullOrEmpty(item.CargoDesc) ? "0" : item.CargoDesc.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                        item.CHAName = string.IsNullOrEmpty(item.CHAName) ? "0" : item.CHAName;
                        item.MarksNo = string.IsNullOrEmpty(item.MarksNo) ? "0" : item.MarksNo;
                        item.Remarks = string.IsNullOrEmpty(item.Remarks) ? "0" : item.Remarks;
                        item.DateOfLanding = string.IsNullOrEmpty(item.DateOfLanding) ? "0" : item.DateOfLanding;
                    });
                    string XML = Utility.CreateXML(objFormOne.lstFormOneDetail);
                    Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
            PPG_FormOneModel objFormOne = new PPG_FormOneModel();
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (PPG_FormOneModel)objImport.DBResponse.Data;

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
                objFormOne.lstCommodity = (List<Import.Models.PPG_Commodity>)objImport.DBResponse.Data;

            return PartialView(objFormOne);
        }

        [HttpGet]
        public ActionResult ViewFormOne(int FormOneId)
        {
            PPG_FormOneModel objFormOne = new PPG_FormOneModel();
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();

            objImport.GetFormOneById(FormOneId);
            if (objImport.DBResponse.Data != null)
                objFormOne = (PPG_FormOneModel)objImport.DBResponse.Data;

            if (objFormOne.lstFormOneDetail != null)
                objFormOne.FormOneDetailsJS = JsonConvert.SerializeObject(objFormOne.lstFormOneDetail);
            return PartialView(objFormOne);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteFormOne(int FormOneId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            if (FormOneId > 0)
                objImport.DeleteFormOne(FormOneId);
            return Json(objImport.DBResponse);
        }

        [HttpGet]
        public JsonResult PrintFormOne(int FormOneId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjIR.GetInvoiceNoForIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<VRN_IssueSlip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            //ObjIR = new VRN_ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssueSlipList()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<VRN_IssueSlip> LstIssueSlip = new List<VRN_IssueSlip>();
            ObjIR.GetAllIssueSlip(0, "");
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<VRN_IssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }

        [HttpGet]
        public ActionResult GetSearchIssueSlipList(string SearchValue)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<VRN_IssueSlip> LstIssueSlip = new List<VRN_IssueSlip>();
            ObjIR.GetAllIssueSlip(0, SearchValue);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<VRN_IssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }

        [HttpGet]
        public JsonResult LoadMoreIssueSlipList(int Page)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<VRN_IssueSlip> LstIssueSlip = new List<VRN_IssueSlip>();
            ObjIR.GetAllIssueSlip(Page, "");
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<VRN_IssueSlip>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditIssueSlip(int IssueSlipId)
        {
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            if (IssueSlipId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (VRN_IssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewIssueSlip(int IssueSlipId)
        {
            VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
            if (IssueSlipId > 0)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (VRN_IssueSlip)ObjIR.DBResponse.Data;
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
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
        public JsonResult AddEditIssueSlip(VRN_IssueSlip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetIssueSlipForPreview(IssueSlipId);
            if (ObjIR.DBResponse.Data != null)
            {
                VRN_IssueSlip ObjIssueSlip = new VRN_IssueSlip();
                ObjIssueSlip = (VRN_IssueSlip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForIssueSlip(ObjIssueSlip, IssueSlipId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForIssueSlip(VRN_IssueSlip ObjIssueSlip, int IssueSlipId)
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

                MarksNo = (MarksNo == "" ? (item.MarksNo) : (MarksNo + "<br/>" + item.MarksNo));

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

            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>" + ObjIssueSlip.CompanyLocation + "-" + ObjIssueSlip.CompanyBranch + "</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>C.H.A Name.</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + Weight + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

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
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            objER.ListOfOBLNo();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfOBLNo = objER.DBResponse.Data;
            }

            CargoSeize objCargoSeize = new CargoSeize();

            if (Id > 0)
            {
                Ppg_ImportRepository rep = new Ppg_ImportRepository();
                rep.GetCargoSeizeById(Id);
                if (rep.DBResponse.Data != null)
                {
                    objCargoSeize = (CargoSeize)rep.DBResponse.Data;
                }
            }

            return PartialView(objCargoSeize);
        }


        [HttpGet]
        public ActionResult GetOBLDetails(int DestuffingEntryDtlId)
        {
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.GetOBLDetails(DestuffingEntryDtlId);
            return Json(objImport.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditCargoSeize(CargoSeize objCargoSeize)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository objER = new Ppg_ImportRepository();

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
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
            List<CargoSeize> lstCargoSeize = new List<CargoSeize>();
            objER.GetAllCargoSeize();
            if (objER.DBResponse.Data != null)
                lstCargoSeize = (List<CargoSeize>)objER.DBResponse.Data;
            return PartialView(lstCargoSeize);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteCargoSeize(int CargoSeizeId)
        {
            Ppg_ImportRepository objER = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();

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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.GetContainersForIRR(TrainSummaryId);
            var Output = (List<PpgContainerDetailsIRR>)ObjIR.DBResponse.Data;
            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIRRPaymentSheet(string InvoiceDate, string CFSCode, string TaxType, int CargoType, int InvoiceId = 0)
        {
            Ppg_ImportRepository objPpgRepo = new Ppg_ImportRepository();
            objPpgRepo.GetIRRPaymentSheet(InvoiceDate, CFSCode, TaxType, CargoType, InvoiceId);
            var Output = (PpgPostPaymentChrg)objPpgRepo.DBResponse.Data;



            return Json(Output, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult AddEditIRRPaymentSheet(PpgIrrInvoice InvoiceObj)
        {
            try
            {

                int BranchId = Convert.ToInt32(Session["BranchId"]);
                Ppg_ImportRepository objChe = new Ppg_ImportRepository();

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

            VRN_ImportRepository objImport = new VRN_ImportRepository();

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

            return PartialView();
        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
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
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult PartyBinding()
        {
            VRN_ImportRepository objImport = new VRN_ImportRepository();
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
            VRN_ImportRepository objImport = new VRN_ImportRepository();
            //objImport.GetEmptyContForPaymentSheet(InvoiceFor, AppraisementId);
            objImport.GetEmptyContByEntryId(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEmptyContainerPaymentSheet(string InvoiceDate, string InvoiceType, int AppraisementId, int PartyId, int PayeeId,
            List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, string ExportUnder)
        {


            string XMLText = "";
            if (lstPaySheetContainer != null)
            {

                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            VRN_ImportRepository objImport = new VRN_ImportRepository();

            objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId, PayeeId, ExportUnder);
            var Output = (VRN_InvoiceYard)objImport.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new VRN_PostPaymentContainer
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

                var invoiceFor = "EC";
                var invoiceData = JsonConvert.DeserializeObject<VRN_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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

                string InvoiceFor = "EC";

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                VRN_ImportRepository objImport = new VRN_ImportRepository();

                objImport.AddEditEmptyContPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, ChargesBreakupXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);


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
        public string GeneratingTentativePDFforYard(PpgInvoiceYard invoiceDataobj)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + invoiceDataobj.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
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

            foreach (PpgPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
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

            foreach (PpgPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
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
        public JsonResult PrintTentativeYardInvoice(PpgInvoiceYard objForm)
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
        public string GeneratingTentativePDFforGodown(PPGInvoiceGodown invoiceDataobj)
        {
            CurrencyToWordINR objCurr = new CurrencyToWordINR();

            List<string> lstSB = new List<string>();
            StringBuilder html = new StringBuilder();

            html.Append("<table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px; '><tbody>");
            html.Append("<tr><td style='text-align: center;'><h1 style='font-size: 20px; line-height: 30px;margin: 0; padding: 0;'>" + invoiceDataobj.CompanyName + "</h1>");
            html.Append("<label style='display: block; font-size: 13px; line-height: 22px;'>(A Govt. of India Undertaking) </label><br />");
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD Patparganj-Delhi</span>");
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

            foreach (PpgPreInvoiceContainer obj in invoiceDataobj.lstPrePaymentCont)
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

            foreach (PpgPostPaymentChrg obj in invoiceDataobj.lstPostPaymentChrg)
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
        public JsonResult PrintTentativeGodowndInvoice(PPGInvoiceGodown objForm)
        {

            int BranchId = Convert.ToInt32(Session["BranchId"]);

            var invoiceData = tentativeinvoice.InvoiceObj;// JsonConvert.DeserializeObject<PostPaymentSheet>(objForm["PaymentSheetModelJson"].ToString());
            string Path = GeneratingTentativePDFforGodown(invoiceData);
            return Json(new { Status = 1, Message = Path });


        }



        #endregion


        #region Job Order By Road
        [HttpGet]
        public ActionResult CreateJobOrderByRoad()
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
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
            PPG_JobOrderByRoad objJO = new PPG_JobOrderByRoad();
            objJO.FormOneDate = DateTime.Now;
            return PartialView("CreateJobOrderByRoad", objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderByRoadDetails()
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            IList<PPG_ImportJobOrderByRoadList> lstIJO = new List<PPG_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadList();
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }

        [HttpGet]
        public ActionResult SearchListOfJobOrderByRoadDetails(string ContainerNo)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            IList<PPG_ImportJobOrderByRoadList> lstIJO = new List<PPG_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadList(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<PPG_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }

        [HttpGet]
        public ActionResult EditJobOrderByRoad(int ImpJobOrderId)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            PPG_JobOrderByRoad objImp = new PPG_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (PPG_JobOrderByRoad)objIR.DBResponse.Data;

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
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            PPG_JobOrderByRoad objImp = new PPG_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (PPG_JobOrderByRoad)objIR.DBResponse.Data;
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            if (ImpJobOrderId > 0)
                objImport.DeleteJobOrderByRoad(ImpJobOrderId);
            return Json(objImport.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrderByRoad(PPG_JobOrderByRoad objImp, String FormOneDetails)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            List<PPG_ImportJobOrderByRoadDtl> lstDtl = new List<PPG_ImportJobOrderByRoadDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PPG_ImportJobOrderByRoadDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            objIR.AddEditJobOrderByRoad(objImp, BranchId, ((Login)(Session["LoginUser"])).Uid, XML);
            return Json(objIR.DBResponse);


        }


        [HttpGet]
        public JsonResult GetJobOrderByRoadByOnEditMode(int ImpJobOrderId)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GetJobOrderByRoadByOnEditMode(ImpJobOrderId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = (List<PPG_ImportJobOrderByRoadDtl>)objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Empty Container Transfer
        [HttpGet]
        public ActionResult EmptyContainerTransferInv(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objImport = new Ppg_ImportRepository();
            objImport.CalculateEmptyContTransferInv(InvoiceDate, InvoiceType, CFSCode, ContainerNo, Size, EntryDate,
             EmptyDate, RefId, PartyId, PayeeId, InvoiceId);
            var Output = (PpgInvoiceYard)objImport.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new PpgPostPaymentContainer
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();

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

                var invoiceFor = "ECTrns";
                var invoiceData = JsonConvert.DeserializeObject<PpgInvoiceYard>(InvoiceObj); ;
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
                Ppg_ImportRepository objImport = new Ppg_ImportRepository();
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

            AMD_ImportRepository ObjIR = new AMD_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNo(((Login)(Session["LoginUser"])).Uid);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo });
                });

            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetImporterName()
        {

            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();

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

            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<CHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCHANAMEForPayment()
        {

            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();

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

        public JsonResult AddEditMergeDeliveryApplication(MergeDeliveryIssueViewModel ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<PPGDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<PPGDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
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
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GetDeliveryAppforMerge(DeliveryId);
            ppgdeliverydet pdet = new ppgdeliverydet();
            if (objIR.DBResponse.Data != null)
                pdet = (ppgdeliverydet)objIR.DBResponse.Data;
            return Json(pdet, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliverymergePaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);


                var invoiceData = JsonConvert.DeserializeObject<VRN_InvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
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
                VRN_ImportRepository objChargeMaster = new VRN_ImportRepository();
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR = new Ppg_ImportRepository();
            ObjIR.GetInvoiceDetForMergeIssueSlip(InvoiceNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMergeIssueSlip(MergeDeliveryIssueViewModel ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
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

        #region List of ExpInvoice
        [HttpGet]
        public ActionResult ListOfImpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {

            VRN_ImportRepository objER = new VRN_ImportRepository();
            objER.ListOfImpInvoice(Module, InvoiceNo, InvoiceDate);
            List<VRN_ListOfImpInvoice> obj = new List<VRN_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<VRN_ListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfImpInvoice", obj);
        }

        #endregion

        #region Import Bond Conversion Godown

        [HttpGet]
        public ActionResult ImportBondConversion()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<OBLNoForBondConversion> lstOBL = new List<OBLNoForBondConversion>();
            ObjIR.GetAllOBLNo();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.OBLList = ObjIR.DBResponse.Data;// new SelectList((List<OBLNoForBondConversion>)ObjIR.DBResponse.Data);
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

            List<ImportBondConversion> lstSac = new List<ImportBondConversion>();
            ObjIR.GetAllSacNo();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            ViewBag.SacList = lstSac;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSacNo(string SacNo = "")
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllSacNo(SacNo);
            List<ImportBondConversion> lstSac = new List<ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGodownByDestuffingId(int destuffingId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetLocationForBondTransfer(destuffingId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOBLDetailsByDestuffingId(int DestuffingEntryId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetOBLDetailsByDestuffingId(DestuffingEntryId);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBondMovementListSerch(string obl)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllInternalBondMovementSearch("GODN", obl);
            List<ImportBondConversion> LstMovement = new List<ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }


        [HttpGet]
        public ActionResult GetBondMovementList()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllInternalBondMovement("GODN");
            List<ImportBondConversion> LstMovement = new List<ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementList", LstMovement);
        }

        [HttpGet]
        public ActionResult EditBondConversionMovement(int MovementId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ImportBondConversion ObjInternalMovement = new ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (ImportBondConversion)ObjIR.DBResponse.Data;


                List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                }
                ViewBag.GodownList = lstGodown;

                List<ImportBondConversion> lstSac = new List<ImportBondConversion>();
                ObjIR.GetAllSacNo();
                if (ObjIR.DBResponse.Data != null)
                {
                    lstSac = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
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
        public JsonResult AddEditImportBondConversion(ImportBondConversion objForm)
        {
            try
            {
                VRN_ImportRepository objChargeMaster = new VRN_ImportRepository();
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
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ImportBondConversion ObjInternalMovement = new ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "GODN");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (ImportBondConversion)ObjIR.DBResponse.Data;
            }
            return PartialView("ViewImportBondConversion", ObjInternalMovement);
        }

        #endregion

        #region Import Bond Conversion Yard

        [HttpGet]
        public ActionResult ImportBondConversionYard()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            List<OBLNoForBondConversion> lstOBL = new List<OBLNoForBondConversion>();
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

            List<ImportBondConversion> lstSac = new List<ImportBondConversion>();
            ObjIR.GetAllSacNo();
            if (ObjIR.DBResponse.Data != null)
            {
                lstSac = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            ViewBag.SacList = lstSac;

            return PartialView();
        }
        [HttpGet]
        public JsonResult GetOBLDetailsByYard(int DestuffingEntryId, string OBLNo)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAppraisementDetById(DestuffingEntryId, OBLNo);
            if (ObjIR.DBResponse.Status > 0)
                return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBondMovementListForYard()
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllInternalBondMovement("YARD");
            List<ImportBondConversion> LstMovement = new List<ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementListYard", LstMovement);
        }
        [HttpGet]
        public ActionResult GetBondMovementListForYardSerch(string obl)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ObjIR.GetAllInternalBondMovementyardSearch("YARD", obl);
            List<ImportBondConversion> LstMovement = new List<ImportBondConversion>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
            }
            return PartialView("InternalMovementListYard", LstMovement);
        }

        [HttpGet]
        public ActionResult EditBondConversionMovementYard(int MovementId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ImportBondConversion ObjInternalMovement = new ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "YARD");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (ImportBondConversion)ObjIR.DBResponse.Data;


                List<Areas.Import.Models.Godown> lstGodown = new List<Areas.Import.Models.Godown>();
                ObjIR.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                if (ObjIR.DBResponse.Data != null)
                {
                    lstGodown = (List<Areas.Import.Models.Godown>)ObjIR.DBResponse.Data;
                }
                ViewBag.GodownList = lstGodown;

                List<ImportBondConversion> lstSac = new List<ImportBondConversion>();
                ObjIR.GetAllSacNo();
                if (ObjIR.DBResponse.Data != null)
                {
                    lstSac = (List<ImportBondConversion>)ObjIR.DBResponse.Data;
                }
                ViewBag.SacList = lstSac;
            }

            return PartialView("EditImportBondConversionYard", ObjInternalMovement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditImportBondConversionYard(ImportBondConversion objForm)
        {
            try
            {
                if (objForm.SQM > 0)
                {
                    VRN_ImportRepository objChargeMaster = new VRN_ImportRepository();
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
        public ActionResult ViewImportBondConversionYard(int MovementId)
        {
            VRN_ImportRepository ObjIR = new VRN_ImportRepository();
            ImportBondConversion ObjInternalMovement = new ImportBondConversion();
            ObjIR.GetBondInternalMovement(MovementId, "YARD");
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (ImportBondConversion)ObjIR.DBResponse.Data;
            }
            return PartialView("ViewImportBondConversionYard", ObjInternalMovement);
        }

        #endregion

        #region test page

        //PDF       
        public JsonResult GeneratePDFForTestPage(FormCollection Fc)
        {
            try
            {
                var Pages = new string[1];
                var FileName = "TestUpload.pdf";
                Pages[0] = Fc["Page"].ToString();
                string LocalDirectory = Server.MapPath("~/Docs/") + Session.SessionID + "/Import/TestUpload/";
                             
                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }
                using (var ObjPdf = new ReportingHelper(PdfPageSize.A4, 30f, 30f, 30f, 30f, false, true))
                {
                    
                    ObjPdf.GeneratePDF(LocalDirectory + FileName, Pages);

                }

                return Json(new { Status = 1, Message = "/Docs/" + Session.SessionID + "/Import/TestUpload/" + FileName });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }


        #endregion


        #region test page 2

        [HttpGet]
        public ActionResult AddDetailPage()
        {

            return PartialView();
        }


        #endregion
    }
}
