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

namespace CwcExim.Areas.Import.Controllers
{
    public partial class Dnd_CWCImportController:BaseController
    {
    
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PPGInvoiceGodown objreport;
      

        #region Custom Appraisement Application
        [HttpGet]
        public ActionResult CreateCustomAppraisement()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            Dnd_CustomAppraisement ObjAppraisement = new Dnd_CustomAppraisement();
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
                ViewBag.ContainerList = new SelectList((List<Dnd_CustomAppraisementDtl>)ObjIR.DBResponse.Data, "CFSCode", "ContainerNo");
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

            //ObjIR.ListOfImporterForm();
            //if (ObjIR.DBResponse.Data != null)
            //{
            //    ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
            //}
            //else
            //{
            //    ViewBag.lstImporter = null;
            //}
            ObjIR.ListOfImporterCustForPage("", 0);
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
        public JsonResult SearchImporterCustByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterCustForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterCustList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterCustForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CHASearchByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCHAList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfChaForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeForCustApr(string PartyCode)
        {
            Ppg_ImportRepository objRepo = new Ppg_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadEximtradeListForCustApr(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfShippingLinePartyCode(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetBOEDetail(String BOENo)
        {
            Dnd_CustomAppraisement objOBLContainer = new Dnd_CustomAppraisement();
            Dnd_ImportRepository rep = new Dnd_ImportRepository();
            rep.GetBOEDetail(BOENo);
            Dnd_CustomAppraisementBOECont obj = (Dnd_CustomAppraisementBOECont)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOBLContainer(String OBlNo)
        {
            Dnd_CustomAppraisement objOBLContainer = new Dnd_CustomAppraisement();
            Dnd_ImportRepository rep = new Dnd_ImportRepository();
            rep.GetOBLContainer(OBlNo);
            if (rep.DBResponse.Data != null)
            {
                List<Dnd_CustomAppraisementOBLCont> obj = (List<Dnd_CustomAppraisementOBLCont>)rep.DBResponse.Data;
                if (obj.Count > 0)
                {
                    ViewBag.ContainerList = new SelectList((List<Dnd_CustomAppraisementOBLCont>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
                }
                else
                {
                    rep.GetContnrNoForCustomAppraise();
                    if (rep.DBResponse.Data != null)
                    {
                        ViewBag.ContainerList = new SelectList((List<Dnd_CustomAppraisementDtl>)rep.DBResponse.Data, "CFSCode", "ContainerNo");
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
            Dnd_CustomAppraisement objOBLContainer = new Dnd_CustomAppraisement();
            Dnd_ImportRepository rep = new Dnd_ImportRepository();
            rep.GetContainerOBL(CFSCode);
            List<Dnd_CustomAppraisementOBLCont> obj = (List<Dnd_CustomAppraisementOBLCont>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCntrDetForCstmAppraise(string CFSCode, string LineNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetContnrDetForCustomAppraise(CFSCode, LineNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCustomAppraisement(int CustomAppraisementId)
        {
            if (CustomAppraisementId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
        public JsonResult AddEditCustomAppraisement(Dnd_CustomAppraisement ObjAppraisement)
        {
            if (ModelState.IsValid)
            {
                string AppraisementXML = "";
                string CAOrdXML = "";
                if (ObjAppraisement.CustomAppraisementXML != null)
                {
                    List<Dnd_CustomAppraisementDtl> LstAppraisement = JsonConvert.DeserializeObject<List<Dnd_CustomAppraisementDtl>>(ObjAppraisement.CustomAppraisementXML);
                    AppraisementXML = Utility.CreateXML(LstAppraisement);
                }
                if (ObjAppraisement.CAOrdDtlXml != null)
                {
                    List<Dnd_CustomAppraisementOrdDtl> LstCAOrd = JsonConvert.DeserializeObject<List<Dnd_CustomAppraisementOrdDtl>>(ObjAppraisement.CAOrdDtlXml);
                    CAOrdXML = Utility.CreateXML(LstCAOrd);
                }

                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
            Dnd_CustomAppraisement ObjAppraisement = new Dnd_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (Dnd_CustomAppraisement)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewCustomAppraisement", ObjAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/ViewCustomAppraisement.cshtml", ObjAppraisement);
        }


        [HttpGet]
        public ActionResult EditCustomAppraisement(int CustomAppraisementId)
        {
            Dnd_CustomAppraisement ObjAppraisement = new Dnd_CustomAppraisement();
            if (CustomAppraisementId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetCustomAppraisement(CustomAppraisementId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjAppraisement = (Dnd_CustomAppraisement)ObjIR.DBResponse.Data;
                    ObjIR.ListOfChaForPage("", 0);

                    if (ObjIR.DBResponse.Data != null)
                    {
                        var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                        var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                        ViewBag.lstCHA = Jobject["lstCHA"];
                        ViewBag.CHAState = Jobject["State"];
                    }
                    //ObjIR.ListOfCHA();
                    //if (ObjIR.DBResponse.Data != null)
                    //{
                    //    ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                    //}
                    //else
                    //{
                    //    ViewBag.CHAList = null;
                    //}
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
                    //ObjIR.ListOfImporterForm();
                    //if (ObjIR.DBResponse.Data != null)
                    //    ViewBag.lstImporter = (List<Importer>)ObjIR.DBResponse.Data;
                    ObjIR.ListOfImporterCustForPage("", 0);
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

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


            List<Dnd_CustomAppraisement> LstAppraisement = new List<Dnd_CustomAppraisement>();
            ObjIR.GetAllCustomAppraisementAppForPage(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Dnd_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
            // return PartialView("/Areas/Import/Views/CWCImport/CustomAppraisementList.cshtml", LstAppraisement);
        }
        [HttpGet]
        public JsonResult LoadListMoreDataForAppraisement(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_CustomAppraisement> LstJO = new List<Dnd_CustomAppraisement>();
                ObjCR.GetAllCustomAppraisementAppForPage(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_CustomAppraisement>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAppraisementListSearch(string ContainerNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<Dnd_CustomAppraisement> LstAppraisement = new List<Dnd_CustomAppraisement>();
            ObjIR.GetAllAppraisementSearch(ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstAppraisement = (List<Dnd_CustomAppraisement>)ObjIR.DBResponse.Data;
            }
            return PartialView("CustomAppraisementList", LstAppraisement);
        }


        #endregion


        #region SealCutting

        [HttpGet]
        public ActionResult AddSealCuting()
        {
            string check = "";
            Dnd_SealCutting SC = new Dnd_SealCutting();
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
        public ActionResult GetListOfSealCuttingDetails()
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetListOfSealCuttingDetails(0);
            IList<Dnd_SealCutting> lstSC = new List<Dnd_SealCutting>();
            if (objIR.DBResponse.Data != null)
                lstSC = (List<Dnd_SealCutting>)objIR.DBResponse.Data;
            return PartialView("ListOfSealCuttingDetails", lstSC);

        }
        [HttpGet]
        public JsonResult LoadListMoreDataForSealCutting(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_SealCutting> LstJO = new List<Dnd_SealCutting>();
            ObjCR.GetListOfSealCuttingDetails(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_SealCutting>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditSealCuttingById(int SealCuttingId)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetSealCuttingById(SealCuttingId);
            Dnd_SealCutting objImp = new Dnd_SealCutting();
            if (objImport.DBResponse.Data != null)
                objImp = (Dnd_SealCutting)objImport.DBResponse.Data;
            return PartialView("EditSealCutting", objImp);
        }
        [HttpGet]
        public ActionResult ViewSealCuttingDetailById(int SealCuttingId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetSealCuttingById(SealCuttingId);
            Dnd_SealCutting objImp = new Dnd_SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (Dnd_SealCutting)objIR.DBResponse.Data;
            return PartialView("ViewSealCuttingDetailById", objImp);
        }

        [HttpGet]
        public JsonResult GetInvoiceDtlForSealCutting(String TransactionDate, String GateInDate, String ContainerNo, String size, int CHAShippingLineId, String CFSCode, int OBLType, int CargoType)
        {
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
            objIR.GetInvoiceDtlForSealCutting(TransactionDate, GateInDate, ContainerNo, size, CHAShippingLineId, CFSCode, OBLType, CargoType);
            SealCutting objImp = new SealCutting();
            if (objIR.DBResponse.Data != null)
                objImp = (SealCutting)objIR.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfContainer()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.ListOfContainer();
            List<SealCutting> objImp = new List<SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfBLData(int OBLId, int impobldtlId, string OBLFCLLCL)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.ListOfBL(OBLId, impobldtlId, OBLFCLLCL);
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.OBLList = objImport.DBResponse.Data;
            //else
            //{
            //    ViewBag.OBLList = null;
            //}

            List<Dnd_SealCutting> objImp = new List<Dnd_SealCutting>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<Dnd_SealCutting>)objImport.DBResponse.Data;
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfGodownData()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
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
        public JsonResult ListOfCHAShippingLineData()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.ListOfCHAShippingLine();
            //if (objImport.DBResponse.Data != null)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data).ToString();
            //else
            //{
            //    ViewBag.ContainerList = null;
            //}

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditSealCutting(Dnd_SealCutting objSealCutting)
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
                    var ViewBLList = JsonConvert.DeserializeObject<List<Dnd_SealCutting>>(objSealCutting.ViewBLList.ToString());
                    if (ViewBLList != null)
                    {
                        OBLXML = Utility.CreateXML(ViewBLList);
                    }
                }
                //if (objSealCutting.lstPostPaymentChrgBreakupAmt != null)
                //{
                //    var ViewBreakList = JsonConvert.DeserializeObject<List<ppgSealPostPaymentChargebreakupdate>>(objSealCutting.lstPostPaymentChrgBreakupAmt.ToString());

                //    ChargesBreakupXML = Utility.CreateXML(ViewBreakList);
                //}

                Dnd_ImportRepository objImport = new Dnd_ImportRepository();
                objImport.AddEditSealCutting(objSealCutting,  BranchId, ((Login)(Session["LoginUser"])).Uid, OBLXML);
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
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.DeleteSealCutting(SealCuttingId);

            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfSealCuttingSearch(string search)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            List<Dnd_SealCutting> lstSealCutSearch = new List<Dnd_SealCutting>();
            objER.GetListOfSealCuttingSearch(0,search);
            // objER.GetAllCCINEntryForPage(0);
            if (objER.DBResponse.Data != null)
                lstSealCutSearch = (List<Dnd_SealCutting>)objER.DBResponse.Data;
            return PartialView("ListOfSealCuttingDetails", lstSealCutSearch);
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
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
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
            Ppg_ImportRepository objIR = new Ppg_ImportRepository();
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
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            PPG_Custom_AppraiseApproval ObjAppId = new PPG_Custom_AppraiseApproval();
            ObjIR.GetCstmAppraiseApplication(CstmAppraiseAppId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjAppId = (PPG_Custom_AppraiseApproval)ObjIR.DBResponse.Data;
            }
            return PartialView("CstmAppraisementApproval", ObjAppId);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult AddCstmAppraiseApproval(int CstmAppraiseAppId, int IsApproved)
        {
            Ppg_ImportRepository ObjIR = new Ppg_ImportRepository();
            ObjIR.UpdateCustomApproval(CstmAppraiseAppId, IsApproved, ((Login)Session["LoginUser"]).Uid);
            return Json(ObjIR.DBResponse);
        }
        #endregion

        #region Destuffing LCL
        [HttpGet]
        public ActionResult CreateDestuffingEntry()
        {
            Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            Dnd_DestuffingEntry ObjDestuffing = new Dnd_DestuffingEntry();
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
            ObjER.GetAllCommodityForPage("", 0);
            if (ObjER.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.LstCommodity = Jobject["LstCommodity"];
                ViewBag.CommodityState = Jobject["State"];
            }
            List<CwcExim.Models.Godown> lstGodown = new List<CwcExim.Models.Godown>();
            ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
            if (ObjER.DBResponse.Data != null)
            {
                ViewBag.lstGodown = ObjER.DBResponse.Data;
            }
            //ObjER.GetAllCommodity();
            //if (ObjER.DBResponse.Data != null)
            //    ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;
            /* For maintaining access rights*/
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);
            /*******************************/
            return PartialView("GetDestuffingEntry", ObjDestuffing);
        }

        [HttpGet]
        public JsonResult GetCntrDetForDestuffingEntry(int CustomAppraisementId,string CFSCode)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetContrDetForDestuffingEntry(CustomAppraisementId, Convert.ToInt32(Session["BranchId"]),CFSCode);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDestuffingEntry(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_DestuffingList> LstJO = new List<Dnd_DestuffingList>();
            ObjCR.GetAllDestuffingEntry(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_DestuffingList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryList()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<Dnd_DestuffingList> LstDestuffing = new List<Dnd_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }

        [HttpGet]
        public ActionResult EditDestuffingEntry(int DestuffingEntryId)
        {

            Dnd_DestuffingEntry ObjDestuffing = new Dnd_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                Dnd_ExportRepository ObjER = new Dnd_ExportRepository();
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                /* ObjIR.ListOfCHA();
                 if (ObjIR.DBResponse.Data != null)
                 {
                     ViewBag.CHAList = new SelectList((List<CHA>)ObjIR.DBResponse.Data, "CHAId", "CHAName");
                 }
                 else
                 {
                     ViewBag.CHAList = null;
                 }*/
                ObjIR.ListOfChaForPage("", 0);
                if (ObjIR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstCHA = Jobject["lstCHA"];
                    ViewBag.CHAState = Jobject["State"];
                }
                ObjER.GetAllCommodityForPage("", 0);
                if (ObjER.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.LstCommodity = Jobject["LstCommodity"];
                    ViewBag.CommodityState = Jobject["State"];
                }
                // ObjER.GetAllCommodity();
                // if (ObjER.DBResponse.Data != null)
                //     ViewBag.CommodityList = (List<CwcExim.Areas.Export.Models.Commodity>)ObjER.DBResponse.Data;
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
                    ObjDestuffing = (Dnd_DestuffingEntry)ObjIR.DBResponse.Data;
                }
                List<CwcExim.Models.Godown> lstGodown = new List<CwcExim.Models.Godown>();
                ObjER.GetAllGodown(((Login)(Session["LoginUser"])).Uid);
                if (ObjER.DBResponse.Data != null)
                {
                    ViewBag.lstGodown = ObjER.DBResponse.Data;
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
            Dnd_DestuffingEntry ObjDestuffing = new Dnd_DestuffingEntry();
            if (DestuffingEntryId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetDestuffingEntry(DestuffingEntryId, Convert.ToInt32(Session["BranchId"]), "View");
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjDestuffing = (Dnd_DestuffingEntry)ObjIR.DBResponse.Data;
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
        public JsonResult AddEditDestuffingEntry(Dnd_DestuffingEntry ObjDestuffing)
        {
            if (ModelState.IsValid)
            {
                string DestuffingEntryXML = "";
                List<Dnd_DestuffingEntryDtl> LstDestuffingEntry = new List<Dnd_DestuffingEntryDtl>();
                if (ObjDestuffing.DestuffingEntryXML != null)
                {
                    LstDestuffingEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_DestuffingEntryDtl>>(ObjDestuffing.DestuffingEntryXML);
                    DestuffingEntryXML = Utility.CreateXML(LstDestuffingEntry);
                }
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetDestuffEntryForPrint(DestuffingEntryId);
            if (ObjIR.DBResponse.Data != null)
            {
                Dnd_DestuffingSheet ObjDestuff = new Dnd_DestuffingSheet();
                ObjDestuff = (Dnd_DestuffingSheet)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForDestuffSheet(ObjDestuff, DestuffingEntryId);
                return Json(new { Status = 1, Message = Path });
            }
            else
            {
                return Json(new { Status = 0, Message = "Error" });
            }
        }
        [NonAction]
        public string GeneratePDFForDestuffSheet(Dnd_DestuffingSheet ObjDestuff, int DestuffingEntryId)
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
            objSB.Append("<td width='300%' valign='top' align='center'><h1 style='font-size: 28px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 14px;'>(A Govt. of India Undertaking)</label><br/><label style='font-size: 14px;'>ICD Patparganj-Delhi</label><br/><label style='font-size: 16px; font-weight:bold;'>DESTUFFING SHEET</label></td></tr>");
            objSB.Append("</tbody></table>");
            objSB.Append("</td></tr>");

            objSB.Append("<tr><td colspan='12'><table cellspacing='0' cellpadding='0' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0;'><tbody><tr>");
            objSB.Append("<th style='font-size:13px;width:10%'>SHED CODE:</th><td style='font-size:12px;'>" + ObjDestuff.GodownName + "</td>");
            objSB.Append("<th style='font-size:13px; text-align:right;'>AS ON:</th><td style='font-size:12px; width:10%;'>" + ObjDestuff.DestuffingEntryDateTime + "</td>");
            objSB.Append("</tr></tbody></table></td></tr>");

            //objSB.Append("<tr><td style='text-align: left;'>");
            //objSB.Append("<span style='display: block; font-size: 11px; padding-bottom: 10px;'>SHED CODE: <label>" + ObjDestuff.GodownName + "</label>");
            //objSB.Append("</span></td><td colspan='3' style='text-align: center;'>");
            //objSB.Append("<span style='display: block; font-size: 14px; line-height: 22px;  padding-bottom: 10px; font-weight:bold;'>DESTUFFING SHEET</span>");
            //objSB.Append("</td><td colspan='2' style='text-align: left;'><span style='display: block; font-size: 11px; padding-bottom: 10px;'>");
            //objSB.Append("AS ON: <label>" + ObjDestuff.DestuffingEntryDateTime + "</label></span></td></tr>");

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
            CompanyDetailsForReport objCompanyDetails = new CompanyDetailsForReport();
            Ppg_ReportRepository ObjRR = new Ppg_ReportRepository();
            ObjRR.getCompanyDetails();
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
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetCIFandDutyForBOE(BOENo, BOEDate);
            if (objIR.DBResponse.Data != null)
                return Json(new { Status = 1, Message = "Success", Data = objIR.DBResponse.Data }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Status = 0, Message = "No Data", Data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchCommodityByPartyCode(string PartyCode)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCommodityList(string PartyCode, int Page)
        {
            Ppg_ExportRepository objRepo = new Ppg_ExportRepository();
            objRepo.GetAllCommodityForPage(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDestuffingEntryListSearch(string ContainerNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<Dnd_DestuffingList> LstDestuffing = new List<Dnd_DestuffingList>();
            ObjIR.GetAllDestuffingEntry(((Login)(Session["LoginUser"])).Uid, ContainerNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstDestuffing = (List<Dnd_DestuffingList>)ObjIR.DBResponse.Data;
            }
            return PartialView("DestuffingEntryList", LstDestuffing);
        }
        #endregion

 
        #region Job Order By Road
        [HttpGet]
        public ActionResult CreateJobOrderByRoad()
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
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
            Dnd_JobOrderByRoad objJO = new Dnd_JobOrderByRoad();
            objJO.FormOneDate = DateTime.Now;
            return PartialView("CreateJobOrderByRoad", objJO);
        }
        [HttpGet]
        public ActionResult ListOfJobOrderByRoadDetails()
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadListPageWise(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }
        [HttpGet]
        public JsonResult LoadListMoreJobOrder(int Page)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadListPageWise(Page);

            if (objIR.DBResponse.Data != null)
            {
                lstIJO = (List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data;
            }
            return Json(lstIJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchListOfJobOrderByRoadDetails(string ContainerNo)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadList(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderByRoad", lstIJO);
        }

        [HttpGet]
        public ActionResult EditJobOrderByRoad(int ImpJobOrderId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            Dnd_JobOrderByRoad objImp = new Dnd_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (Dnd_JobOrderByRoad)objIR.DBResponse.Data;

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
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadId(ImpJobOrderId);
            Dnd_JobOrderByRoad objImp = new Dnd_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (Dnd_JobOrderByRoad)objIR.DBResponse.Data;
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
           Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            if (ImpJobOrderId > 0)
                objImport.DeleteJobOrderByRoad(ImpJobOrderId);
            return Json(objImport.DBResponse);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrderByRoad(Dnd_JobOrderByRoad objImp, String FormOneDetails)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            List<Dnd_ImportJobOrderByRoadDtl> lstDtl = new List<Dnd_ImportJobOrderByRoadDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ImportJobOrderByRoadDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            objIR.AddEditJobOrderByRoad(objImp, BranchId, ((Login)(Session["LoginUser"])).Uid, XML);
            return Json(objIR.DBResponse);


        }


        [HttpGet]
        public JsonResult GetJobOrderByRoadByOnEditMode(int ImpJobOrderId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadByOnEditMode(ImpJobOrderId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = (List<Dnd_ImportJobOrderByRoadDtl>)objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        #endregion


        #region Job Order Amendment
        public ActionResult ListOfJobOrderAmendment()
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadAmendmentListPageWise(0);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderAmendment", lstIJO);
        }

        [HttpGet]
        public JsonResult LoadListOfJobOrderAmendment(int Page)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadAmendmentListPageWise(Page);

            if (objIR.DBResponse.Data != null)
            {
                lstIJO = (List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data;
            }
            return Json(lstIJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SearchListOfJobOrderAmendmentDetails(string ContainerNo)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            IList<Dnd_ImportJobOrderByRoadList> lstIJO = new List<Dnd_ImportJobOrderByRoadList>();
            objIR.GetJobOrderByRoadAmendmentList(ContainerNo);
            if (objIR.DBResponse.Data != null)
                lstIJO = ((List<Dnd_ImportJobOrderByRoadList>)objIR.DBResponse.Data);
            return PartialView("ListOfJobOrderAmendment", lstIJO);
        }
        [HttpGet]
        public ActionResult JobOrderByRoadAmendment(int Id = 0)
        {
            //Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            //objIR.GetJobOrderByRoadId(ImpJobOrderId);
            Dnd_JobOrderByRoad objImp = new Dnd_JobOrderByRoad();
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetAllJobOrder();
            if (objIR.DBResponse.Data != null)
            {
                ViewBag.ListOfJobOrder = (List<Dnd_JobOrderByRoad>)objIR.DBResponse.Data;
            }
          
            return PartialView("JobOrderByRoadAmendment", objImp);
        }

        [HttpGet]
        public ActionResult EditAmendment(int FormOneId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadId(FormOneId);
            Dnd_JobOrderByRoad objImp = new Dnd_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (Dnd_JobOrderByRoad)objIR.DBResponse.Data;
            objIR.GetAllJobOrder();
            if (objIR.DBResponse.Data != null)
            {
                ViewBag.ListOfJobOrder = (List<Dnd_JobOrderByRoad>)objIR.DBResponse.Data;
            }
            //objIR.ListOfShippingLine();
            //if (objIR.DBResponse.Data != null)
            //    ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            // Dnd_ImportRepository objIR = new Dnd_ImportRepository();

            objIR.ListOfShippingLinePartyCode("", 0);
            if (objIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstShippingLine = Jobject["lstShippingLine"];
                ViewBag.State = Jobject["State"];
            }

            ViewBag.View = "Edit";
            return PartialView("JobOrderByRoadAmendment", objImp);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult AddEditJobOrderAmendment(Dnd_JobOrderByRoad objImp, String FormOneDetails)
        {
            int BranchId = Convert.ToInt32(Session["BranchId"]);
            int RoleId = ((CwcExim.Models.Login)Session["LoginUser"]).Role.RoleId;
            List<Dnd_ImportJobOrderByRoadDtl> lstDtl = new List<Dnd_ImportJobOrderByRoadDtl>();
            List<int> lstLctn = new List<int>();
            string XML = "";
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            if (FormOneDetails != null)
            {
                lstDtl = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dnd_ImportJobOrderByRoadDtl>>(FormOneDetails);
                if (lstDtl.Count > 0)
                    XML = Utility.CreateXML(lstDtl);
            }

            objIR.AddEditJobOrderAmendment(objImp, BranchId, ((Login)(Session["LoginUser"])).Uid, XML);
            return Json(objIR.DBResponse);


        }

        [HttpGet]
        public JsonResult GetJobOrderAmendmentByOnEditMode(int FormOneId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadByOnEditMode(FormOneId);
            object data = null;
            if (objIR.DBResponse.Data != null)
                data = (List<Dnd_ImportJobOrderByRoadDtl>)objIR.DBResponse.Data;
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewJobOrderAmendment(int FormOneId)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetJobOrderByRoadId(FormOneId);
            Dnd_JobOrderByRoad objImp = new Dnd_JobOrderByRoad();
            if (objIR.DBResponse.Data != null)
                objImp = (Dnd_JobOrderByRoad)objIR.DBResponse.Data;
            objIR.ListOfShippingLine();
            if (objIR.DBResponse.Data != null)
                ViewBag.ListOfShippingLine = objIR.DBResponse.Data;
            ViewBag.View = "View";
            return PartialView("JobOrderByRoadAmendment", objImp);
        }
        #endregion

        #region YardInvoice
        [HttpGet]
        public ActionResult CreateYardPaymentSheet(string type = "Tax")
        {
            ViewData["InvType"] = type;
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            //objImport.GetPaymentPartyForImportInvoice();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            Dnd_ImportRepository objExport = new Dnd_ImportRepository();
            objExport.GetPaymentPartyForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }
            objExport.GetPaymentPayerForPage("", 0);
            if (objExport.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(objExport.DBResponse.Data);
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
        public JsonResult LoadPartyListFCL(int Page)
        {
            Dnd_ImportRepository objimp = new Dnd_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage("", Page);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetApprForYard()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetAppraismentRequestForPaymentSheet();
            if (objImport.DBResponse.Status > 0)
                ViewBag.StuffingReqList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.StuffingReqList = null;
            return Json(ViewBag.StuffingReqList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPartyCodeFCL(string PartyCode)
        {
            Dnd_ImportRepository objimp = new Dnd_ImportRepository();
            objimp.GetImpPaymentPartyForFCLPage(PartyCode, 0);
            return Json(objimp.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentSheetContainer(int AppraisementId)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetContainerForPaymentSheet(AppraisementId);
            if (objImport.DBResponse.Status > 0)
                ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.ContainerList = null;

            return Json(ViewBag.ContainerList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetContainerPaymentSheet(string InvoiceDate, int AppraisementId, string InvoiceType,
            List<Dnd_PaymentSheetContainer> lstPaySheetContainer, int CasualLabour = 0, int PartyId = 0,
            int InvoiceId = 0, int isdirect = 0, int PayeeId = 0)
        {
            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            Dnd_ImportRepository objPpgRepo = new Dnd_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objPpgRepo.GetYardPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, InvoiceId, CasualLabour, PartyId, isdirect,PayeeId);
            var Output = (DNDInvoiceYard)objPpgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new DNDPostPaymentContainer
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
                        StuffCUM = item.StuffCUM,
                        ISODC = item.ISODC
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
        public JsonResult AddEditYardPaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);

                var invoiceData = JsonConvert.DeserializeObject<DNDInvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
                // var invoiceData = objForm;
                string ContainerXML = "";
                string ChargesXML = "";
                string ContWiseCharg = "";
                string OperationCfsCodeWiseAmtXML = "";
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
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(result);
                }
                Dnd_ImportRepository objChargeMaster = new Dnd_ImportRepository();
                objChargeMaster.AddEditYardPaymentSheet(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, "IMPYard");
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
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objExport = new Dnd_ImportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            Dnd_ImportRepository objExport = new Dnd_ImportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objExport = new Dnd_ImportRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objExport = new Dnd_ImportRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListOfYardInvoice()
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            ObjCR.ListOfYardInvoice(0);
            List<Dnd_ListOfImpInvoice> LstJO = new List<Dnd_ListOfImpInvoice>();
            if (ObjCR.DBResponse.Data != null)
                LstJO = (List<Dnd_ListOfImpInvoice>)ObjCR.DBResponse.Data;
            return PartialView("ListOfYardInvoice", LstJO);
        }
        [HttpGet]
        public ActionResult LoadMoreListOfYardInvoice(int Page)
        {

            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_ListOfImpInvoice> LstJO = new List<Dnd_ListOfImpInvoice>();
            ObjCR.ListOfYardInvoice(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_ListOfImpInvoice>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfYardInvoiceSearch(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            objER.ListOfEmptyInvoiceSearch(Module, InvoiceNo, InvoiceDate);
            List<Dnd_ListOfImpInvoice> obj = new List<Dnd_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Dnd_ListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfEmptyContainerInvoice", obj);
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

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

            ObjIR.GetDestuffEntryNo(((Login)(Session["LoginUser"])).Uid);

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<DNDDestuffingEntryNoList>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { DestuffingId = item.DestuffingId, DestuffingEntryNo = item.DestuffingEntryNo });
                });

            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetImporterName()
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();

           ObjIR.ListOfImporterForMerge("",0);;

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Areas.Import.Models.ImporterForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { ImporterId = item.ImporterId, ImporterName = item.ImporterName, PartyCode = item.PartyCode });
                });

            }
            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCHANAME()
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.ListOfChaForMergeApp("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Dnd_CHAForPage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { CHAId = item.CHAId, CHAName = item.CHAName, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCHANAMEForPayment()
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<dynamic> objImp2 = new List<dynamic>();


            ObjIR.GetImpPaymentPartyForMergePage("");

            if (ObjIR.DBResponse.Data != null)
            {
                ((List<Dnd_ImpPartyForpage>)ObjIR.DBResponse.Data).ToList().ForEach(item =>
                {
                    objImp2.Add(new { PartyId = item.PartyId, PartyName = item.PartyName, Address = item.Address, State = item.State, StateCode = item.StateCode, GSTNo = item.GSTNo, PartyCode = item.PartyCode });
                });
            }



            return Json(objImp2, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult AddEditMergeDeliveryApplication(DND_MergeDeliveryIssueViewModel ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliApp.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DNDDeliveryApplicationDtl>>(ObjDelivery.DeliApp.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.DeliApp.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliApp.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.DeliApp.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DNDDeliveryOrdDtl>>(ObjDelivery.DeliApp.DeliveryOrdDtlXml);
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
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetDeliveryAppforMerge(DeliveryId);
            DNDdeliverydet pdet = new DNDdeliverydet();
            if (objIR.DBResponse.Data != null)
                pdet = (DNDdeliverydet)objIR.DBResponse.Data;
            return Json(pdet, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliverymergePaymentSheet(FormCollection objForm)
        {
            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);               

                var invoiceData = JsonConvert.DeserializeObject<DNDInvoiceGodown>(objForm["PaymentSheetModelJson"].ToString());
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
                Dnd_ImportRepository objChargeMaster = new Dnd_ImportRepository();
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR = new Dnd_ImportRepository();
            ObjIR.GetInvoiceDetForMergeIssueSlip(InvoiceNo);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMergeIssueSlip(DND_MergeDeliveryIssueViewModel ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
        [HttpGet]
        public JsonResult SearchImporterByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterForMergeSearch(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadImporterList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfImporterForMergeSearch(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchIssuedByPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfIssueParty(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadIssuedList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfIssueParty(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchCargoDeliveredPartyCode(string PartyCode)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfCargoDelivered(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCargoDeliveredList(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfCargoDelivered(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        
     
        #endregion

        #region Delivery Application

        [HttpGet]
        public ActionResult CreateDeliveryApplication()
        {
            Wfld_ImportRepository ObjIR = new Wfld_ImportRepository();
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




        //[HttpGet]
        //public JsonResult ListOfIssueParty(string PartyCode, int Page)
        //{
        //    Dnd_ImportRepository objcash = new Dnd_ImportRepository();
        //    objcash.ListOfIssuedBy(PartyCode, Page);
        //    //List<PPG_ContainerStuffing> objImp = new List<PPG_ContainerStuffing>();
        //    List<dynamic> objImp2 = new List<dynamic>();
        //    if (objcash.DBResponse.Data != null)
        //        ((List<Dnd_IssuedByForPage>)objcash.DBResponse.Data).ToList().ForEach(item =>
        //        {
        //            objImp2.Add(new { IssueBy = item.IssuedBy ,PartyCode=item.PartyCode});
        //        });

        //    return Json(objImp2, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult ListOfIssueParty(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfIssuedBy(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListOfCargoParty(string PartyCode, int Page)
        {
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.ListOfCargoDelivered(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditDeliveryApplication(int DeliveryId)
        {
            Wfld_ImportRepository ObjIR = new Wfld_ImportRepository();
            BondRepository ObjBR = new BondRepository();
            WFLD_DeliverApplication ObjDelivery = new WFLD_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (WFLD_DeliverApplication)ObjIR.DBResponse.Data;
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            DND_DeliverApplication ObjDelivery = new DND_DeliverApplication();
            ObjIR.GetDeliveryApplication(DeliveryId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjDelivery = (DND_DeliverApplication)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjDelivery);
        }

        [HttpGet]
        public ActionResult ListOfDeliveryApplication()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<DNDDeliveryApplicationList> LstDelivery = new List<DNDDeliveryApplicationList>();
            ObjIR.GetAllDeliveryApplication(0, ((Login)(Session["LoginUser"])).Uid);
            if (ObjIR.DBResponse.Data != null)
                LstDelivery = (List<DNDDeliveryApplicationList>)ObjIR.DBResponse.Data;
            return PartialView("DeliveryApplicationList", LstDelivery);
        }

        [HttpGet]
        public JsonResult LoadListMoreDataForDeliveryApp(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<DNDDeliveryApplicationList> LstJO = new List<DNDDeliveryApplicationList>();
            ObjCR.GetAllDeliveryApplication(Page, ((Login)(Session["LoginUser"])).Uid);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<DNDDeliveryApplicationList>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditDeliveryApplication(DND_DeliverApplication ObjDelivery)
        {
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                string DeliveryXml = "";
                string DeliveryOrdXml = "";
                if (ObjDelivery.DeliveryAppDtlXml != "")
                {
                    ObjDelivery.LstDeliveryAppDtl = JsonConvert.DeserializeObject<List<DNDDeliveryApplicationDtl>>(ObjDelivery.DeliveryAppDtlXml);
                    DeliveryXml = Utility.CreateXML(ObjDelivery.LstDeliveryAppDtl);
                }


                if (ObjDelivery.DeliveryOrdDtlXml != "")
                {
                    ObjDelivery.LstDeliveryordDtl = JsonConvert.DeserializeObject<List<DNDDeliveryOrdDtl>>(ObjDelivery.DeliveryOrdDtlXml);
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetBOELineNoDetForDelivery(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBOENoForDeliveryApp(int DestuffingId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetBOELineNoForDelivery(DestuffingId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCIFFromOOC(String BOE, String BOEDT)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetCIFFromOOCDelivery(BOE, BOEDT);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }



        #endregion


        #region Issue Slip

        [HttpGet]
        public ActionResult CreateIssueSlip()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            DND_Issueslip ObjIssueSlip = new DND_Issueslip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetInvoiceNoForIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<DND_Issueslip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR = new Dnd_ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssueSlipList()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<DND_Issueslip> LstIssueSlip = new List<DND_Issueslip>();
            ObjIR.GetAllIssueSlipPageWise(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<DND_Issueslip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }
        [HttpGet]
        public JsonResult LoadListMoreDataForIssueslip(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<DND_Issueslip> LstIssueSlip = new List<DND_Issueslip>();
            ObjCR.GetAllIssueSlipPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<DND_Issueslip>)ObjCR.DBResponse.Data;
            }
            return Json(LstIssueSlip, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetIssueSlipListSearch(string InvoiceNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<DND_Issueslip> LstIssueSlip = new List<DND_Issueslip>();
            ObjIR.GetAllIssueSlipSearch(InvoiceNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<DND_Issueslip>)ObjIR.DBResponse.Data;
            }
            return PartialView("IssueSlipList", LstIssueSlip);
        }
        [HttpGet]
        public ActionResult EditIssueSlip(int IssueSlipId)
        {
            DND_Issueslip ObjIssueSlip = new DND_Issueslip();
            if (IssueSlipId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (DND_Issueslip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewIssueSlip(int IssueSlipId)
        {
            DND_Issueslip ObjIssueSlip = new DND_Issueslip();
            if (IssueSlipId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (DND_Issueslip)ObjIR.DBResponse.Data;
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
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
        public JsonResult AddEditIssueSlip(DND_Issueslip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetIssueSlipForPreview(IssueSlipId);
            if (ObjIR.DBResponse.Data != null)
            {
                DND_Issueslip ObjIssueSlip = new DND_Issueslip();
                ObjIssueSlip = (DND_Issueslip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForIssueSlip(ObjIssueSlip, IssueSlipId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForIssueSlip(DND_Issueslip ObjIssueSlip, int IssueSlipId)
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
            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS-DRONAGIRI-NODE</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>IGM No/Item No</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>"+CHA+"</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS-DRONAGIRI-NODE</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'><span style='border-bottom:1px solid #000;'></span></td><td style='text-align:right;'><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>Issue Slip Of Container Freight Station.</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/></td></tr><tr><td colspan='2' style='text-align:left;'><span style='border-bottom:1px solid #000;'></span><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry No.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + BOENo + "</td><td style='border:1px solid #000;padding:5px;'>" + BOEDate + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td><span></span></td><td><br/><br/></td></tr></tbody></table></td></tr></tbody></table>";


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/IssueSlip" + IssueSlipId + ".pdf";
        }

        #endregion
        #region Container Wise Issue Slip

        [HttpGet]
        public ActionResult CreateContainerWiseIssueSlip()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            Dnd_ContIssueSlip ObjIssueSlip = new Dnd_ContIssueSlip();
            ObjIssueSlip.IssueSlipDate = DateTime.Now.ToString("dd-MM-yyyy");
            ObjIR.GetInvoiceNoForContIssueSlip();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.InvoiceNoList = new SelectList((List<Dnd_ContIssueSlip>)ObjIR.DBResponse.Data, "InvoiceId", "InvoiceNo");
            }
            else
            {
                ViewBag.InvoiceNoList = null;
            }
            return PartialView("CreateContainerWiseIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public JsonResult GetInvcDetForContainerWiseIssueSlip(int InvoiceId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR = new Dnd_ImportRepository();
            ObjIR.GetInvoiceDetForIssueSlip(InvoiceId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetContainerWiseIssueSlipList()
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            List<Dnd_ContIssueSlip> LstIssueSlip = new List<Dnd_ContIssueSlip>();
            ObjIR.GetAllContIssueSlip(0);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<Dnd_ContIssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("ListContIssueSlip", LstIssueSlip);
        }
       
        [HttpGet]
        public JsonResult LoadListMoreDataForContainerWiseIssueslip(int Page)
        {
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_ContIssueSlip> LstIssueSlip = new List<Dnd_ContIssueSlip>();
            ObjCR.GetAllContIssueSlip(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<Dnd_ContIssueSlip>)ObjCR.DBResponse.Data;
            }
            return Json(LstIssueSlip, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetContainerWiseIssueSlipListSearch(string InvoiceNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<Dnd_ContIssueSlip> LstIssueSlip = new List<Dnd_ContIssueSlip>();
            ObjIR.GetAllContIssueSlipSearch(InvoiceNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstIssueSlip = (List<Dnd_ContIssueSlip>)ObjIR.DBResponse.Data;
            }
            return PartialView("ListContIssueSlip", LstIssueSlip);
        }
        [HttpGet]
        public ActionResult EditContainerWiseIssueSlip(int IssueSlipId)
        {
            DND_Issueslip ObjIssueSlip = new DND_Issueslip();
            if (IssueSlipId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetIssueSlip(IssueSlipId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (DND_Issueslip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("EditContainerWiseIssueSlip", ObjIssueSlip);
        }

        [HttpGet]
        public ActionResult ViewContainerWiseIssueSlip(int IssueSlipdtlId)
        {
            Dnd_ContIssueSlip ObjIssueSlip = new Dnd_ContIssueSlip();
            if (IssueSlipdtlId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIR.GetContIssueSlip(IssueSlipdtlId);
                if (ObjIR.DBResponse.Data != null)
                {
                    ObjIssueSlip = (Dnd_ContIssueSlip)ObjIR.DBResponse.Data;
                }
            }
            return PartialView("ViewContainerWiseIssueSlip", ObjIssueSlip);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteContainerWiseIssueSlip(int IssueSlipId)
        {
            if (IssueSlipId > 0)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
        public JsonResult AddEditContainerWiseIssueSlip(Dnd_ContIssueSlip ObjIssueSlip)
        {
            if (ModelState.IsValid)
            {
                Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
                ObjIssueSlip.Uid = ((Login)Session["LoginUser"]).Uid;
                string IssueSlipXML = "";
                List<DND_IssueContainer> LstIssueSlip = new List<DND_IssueContainer>();
                if (ObjIssueSlip.IssueSlipXML != null)
                {
                    LstIssueSlip = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DND_IssueContainer>>(ObjIssueSlip.IssueSlipXML);
                    IssueSlipXML = Utility.CreateXML(LstIssueSlip);
                }
                ObjIR.AddEditContainerWiseIssueSlip(ObjIssueSlip, IssueSlipXML);
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
        public JsonResult PrintContainerWiseIssueSlip(int IssueSlipdtlId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetContIssueSlipForPreview(IssueSlipdtlId);
            if (ObjIR.DBResponse.Data != null)
            {
              Dnd_ContIssueSlip ObjIssueSlip = new Dnd_ContIssueSlip();
                ObjIssueSlip = (Dnd_ContIssueSlip)ObjIR.DBResponse.Data;
                string Path = GeneratePDFForContainerWiseIssueSlip(ObjIssueSlip, IssueSlipdtlId);
                return Json(new { Status = 1, Message = Path });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        [NonAction]
        public string GeneratePDFForContainerWiseIssueSlip(Dnd_ContIssueSlip ObjIssueSlip, int IssueSlipdtlId)
        {
            string Path = Server.MapPath("~/Docs/") + Session.SessionID + "/IssueSlip" + IssueSlipdtlId + ".pdf";
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
            Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>"+ObjIssueSlip.CompanyLocation+"</span><br/><label style='font-size: 14px; font-weight:bold;'>CONTAINER WISE ISSUE SLIP</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>IGM No/Item No</th><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + CHA + "</span></td><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //Html = "<table style='width:100%;font-size:8pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead>  <tr><td colspan='12'><table cellspacing='0' cellpadding='5' style='width:100%; font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 5px;'><tbody><tr><td valign='top'><img align='right' src='IMGSRC' width='90'/></td><td width='100%' valign='top' align='center'><h1 style='font-size: 22px; margin: 0; padding: 0;'>CENTRAL WAREHOUSING CORPORATION</h1><label style='font-size: 12px;'>(A Govt. of India Undertaking)</label><br/><span style='font-size:12px;'>CFS-DRONAGIRI-NODE</span><br/><label style='font-size: 14px; font-weight:bold;'>INVOICE CHECKING</label></td></tr></tbody></table></td></tr></thead> <tbody style='border:1px solid #000;'><tr>  <td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;width:20%;'>Container / CBT</th><th style='border-bottom:1px solid #000;text-align:center;width:15%;'>Size P.N.R No Via No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Vessel Name</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody style='border-bottom:1px solid #000;'><tr><td style='text-align:center;'><span>" + ContainerNo + "</span></td><td style='text-align:center;'><span>" + Size + "</span></td><td style='text-align:center;'><span>" + Vessel + "</span></td><td style='text-align:center;'><span>" + BOENo + "</span></td><td style='text-align:center;'><span>" + BOEDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shipping Agent</th><th style='border-bottom:1px solid #000;text-align:center;'>Importer</th><th style='border-bottom:1px solid #000;text-align:center;width:30%;'>Cargo Description</th><th style='border-bottom:1px solid #000;text-align:center;'>Marks & No.</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + ShippingLine + "</span></td><td style='text-align:center;'><span>" + Importer + "</span></td><td style='text-align:center;'><span>" + CargoDescription + "</span></td><td style='text-align:center;'><span>" + MarksNo + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Line No</th><th style='border-bottom:1px solid #000;text-align:center;'>Rotation</th><th style='border-bottom:1px solid #000;text-align:center;'>Weight</th><th style='border-bottom:1px solid #000;text-align:center;'>S/L Delivery Note No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of Receipt of Cont.</th><th style='border-bottom:1px solid #000;text-align:center;'>Date of De-Stuffing/Delivery</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + LineNo + "</span></td><td style='text-align:center;'><span>" + Rotation + "</span></td></td><td style='text-align:center;'><span>" + "" + "</span></td><td style='text-align:center;'><span>" + ArrivalDate + "</span></td><td style='text-align:center;'><span>" + DestuffingDate + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='border-bottom:1px solid #000;text-align:center;'>Shed/Grid No.</th><th style='border-bottom:1px solid #000;text-align:center;'>Total CWC Dues</th><th style='border-bottom:1px solid #000;text-align:center;'>CR No. & Date</th><th style='border-bottom:1px solid #000;text-align:center;'>Valid Till Date</th></tr></thead><tbody><tr><td style='text-align:center;'><span>" + Location + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.TotalCWCDues + "</span></td><td style='text-align:center;'><span>" + ObjIssueSlip.CRNoDate + "</span></td><td style='text-align:center;'><span>" + "" + "</span></td></tr></tbody></table></td></tr>  <tr><td colspan='12' style='border:1px solid #000;'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border-collapse:collapse;'><thead><tr><th style='text-align:left;'><br/><br/><br/>Name & Signature of Importer / Agent</th><th style='text-align:right;'><br/><br/><br/>Signature of CWC</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Delivered....................No of Units at Shed No...Grid No... ....... on <span>" + DateTime.Now.ToString("dd/MM/yyy") + "</span></th></tr><tr><th colspan='2' style='text-align:right;'><br/>Shed In-Charge</th></tr><tr><th colspan='2' style='text-align:left;'><br/>Received....... ....... No of Units/ Container in Good Condition.</th></tr><tr><th colspan='2' style='text-align:right;'><br/>Signature of Importer/Agent</th></tr></thead></table></td></tr></tbody></table>";

            //  Html = "<table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><thead><tr><th style='width:15%;text-align:left;'><img src='IMGSRC' width='50%;'/></th><th style='text-align:center;'><span style='font-size:16pt;'><strong>CENTRAL WAREHOUSING CORPORATION</strong></span><br/><span>(A GOVT. OF INDIA UNDERTAKING)<br/>" + CompanyAddress + "</span></th></tr></thead><tbody><tr><td style='text-align:left;'><span style='border-bottom:1px solid #000;'></span></td><td style='text-align:right;'><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2' style='text-align:center;'><br/><span style='border-bottom:1px solid #000;font-size:12pt;font-weight:600;'>Issue Slip Of Container Freight Station.</span></td></tr><tr><td colspan='2' style='text-align:center;'><br/></td></tr><tr><td colspan='2' style='text-align:left;'><span style='border-bottom:1px solid #000;'></span><span style='border-bottom:1px solid #000;'></span></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;border:1px solid #000;border-collapse:collapse;'><thead><tr><th style='border:1px solid #000;padding:5px;text-align:center;'>SL. NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>CONTAINER NO.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>SIZE</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry No.</th><th style='border:1px solid #000;padding:5px;text-align:center;'>Bill of Entry Date</th></tr></thead><tbody><tr><td style='border:1px solid #000;padding:5px;'>" + Serial + "</td><td style='border:1px solid #000;padding:5px;'>" + ContainerNo + "</td><td style='border:1px solid #000;padding:5px;'>" + Size + "</td><td style='border:1px solid #000;padding:5px;'>" + BOENo + "</td><td style='border:1px solid #000;padding:5px;'>" + BOEDate + "</td></tr></tbody> </table></td></tr><tr><td colspan='2'><table style='width:100%;font-size:9pt;font-family:Verdana,Arial,San-serif;'><tbody><tr><td><span></span></td><td><br/><br/></td></tr></tbody></table></td></tr></tbody></table>";


            Html = Html.Replace("IMGSRC", Server.MapPath("~/Content/Images/CWCPDF.PNG"));
            using (var Rh = new ReportingHelper(PdfPageSize.A4, 40f, 40f, 40f, 40f, false, true))
            {
                Rh.GeneratePDF(Path, Html);
            }
            return "/Docs/" + Session.SessionID + "/IssueSlip" + IssueSlipdtlId + ".pdf";
        }
        [HttpGet]
        public JsonResult GetIssueSlipContainer(int InvoiceId)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetContainerForIssueSlip(InvoiceId);
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.ContainerList = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.ContainerList = null;

            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Empty Container Payment Sheet

        [HttpGet]
        public ActionResult CreateEmptyContPaymentSheet(string type = "Godown:Tax")
        {
            ViewData["ForType"] = type.Split(':')[0];
            ViewData["InvType"] = type.Split(':')[1];

            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
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

            return PartialView();
        }

        [HttpGet]
        public JsonResult EmptyContainerdtlBinding()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
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
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, 0);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyLists(string PartyCode, int Page)
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            objImport.GetImpPaymentPartyForPage(PartyCode, Page);
            return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
        }







        /*
                [HttpGet]
                public JsonResult SearchPartyNameByPartyCodes(string PartyCode)
                {
                    Ppg_ImportRepository objImport = new Ppg_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, 0);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

                [HttpGet]
                public JsonResult LoadPartyLists(string PartyCode, int Page)
                {
                    Ppg_ImportRepository objImport = new Ppg_ImportRepository();
                    objImport.GetImpPaymentPartyForPage(PartyCode, Page);
                    return Json(objImport.DBResponse, JsonRequestBehavior.AllowGet);
                }

        */





        [HttpGet]
        public JsonResult PartyBinding()
        {
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
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
            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
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
            List<PaymentSheetContainer> lstPaySheetContainer, string InvoiceFor, int PayeeId)
        {
            //string XMLText = "";
            //if (lstPaySheetContainer != null)
            //{
            //    XMLText = Utility.CreateXML(lstPaySheetContainer);
            //}

            // ChargeMasterRepository objChrgRepo = new ChargeMasterRepository();
            //HDB_ImportRepository objChrgRepo = new HDB_ImportRepository();
            ////objChrgRepo.GetAllCharges();
            //objChrgRepo.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, DeliveryType, AppraisementNo, AppraisementDate, PartyId, PartyName, PartyAddress, PartyState,
            //        PartyStateCode, PartyGST, PayeeId, PayeeName, InvoiceType, XMLText, InvoiceFor, InvoiceId);

            //var Output = (Hdb_PostPaymentSheet)objChrgRepo.DBResponse.Data;
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
            ///***************BOL PRINT*************/
            //objChrgRepo.GetBOLForEmptyCont(InvoiceFor, Output.RequestId);
            //var BOL = "";
            //if (objChrgRepo.DBResponse.Status == 1)
            //    BOL = objChrgRepo.DBResponse.Data.ToString();
            ///************************************/
            //return Json(new { Output, BOL });

            string XMLText = "";
            if (lstPaySheetContainer != null)
            {
                //XMLText = Utility.CreateXML(lstPaySheetContainer.Where(o => o.Selected == true).ToList());
                XMLText = Utility.CreateXML(lstPaySheetContainer);
            }

            Dnd_ImportRepository objImport = new Dnd_ImportRepository();
            //objChrgRepo.GetAllCharges();
            objImport.GetEmptyContPaymentSheet(InvoiceDate, AppraisementId, InvoiceType, XMLText, 0, InvoiceFor, PartyId,PayeeId);
            var Output = (Dnd_InvoiceYard)objImport.DBResponse.Data;

            Output.InvoiceDate = InvoiceDate;
            /*if (InvoiceFor == "YARD")
            {
                Output.Module = "IMPYard";
            }
            else
            {
                Output.Module = "IMPDeli";
            }*/
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
                    Output.lstPostPaymentCont.Add(new Dnd_PostPaymentContainer
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
                var invoiceFor = "EC"; //objForm["InvoiceFor"].ToString();
                var invoiceData = JsonConvert.DeserializeObject<Dnd_InvoiceYard>(objForm["PaymentSheetModelJson"].ToString());
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
                if (invoiceData.lstPostPaymentChrg != null)
                {
                    ChargesBreakupXML = Utility.CreateXML(invoiceData.lstPostPaymentChrg);
                }
                //ChargeMasterRepository objChargeMaster = new ChargeMasterRepository();
                //string InvoiceFor = invoiceFor == "Yard" ? "ECYard" : "ECGodn";
                string InvoiceFor = "EC";
                //objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);

                if (invoiceData.lstOperationCFSCodeWiseAmount != null)
                {
                    OperationCfsCodeWiseAmtXML = Utility.CreateXML(invoiceData.lstOperationCFSCodeWiseAmount);
                }
                Dnd_ImportRepository objImport = new Dnd_ImportRepository();
                // objChargeMaster.AddEditInvoice(invoiceData, ContainerXML, ChargesXML, ContWiseCharg, OperationCfsCodeWiseAmtXML, BranchId, ((Login)(Session["LoginUser"])).Uid, InvoiceFor);
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
        [HttpGet]
        public ActionResult ListOfEmptyContainerInvoice()
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            objER.ListOfEmptyInvoice(0);
            List<Dnd_ListOfImpInvoice> obj = new List<Dnd_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Dnd_ListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfEmptyContainerInvoice", obj);
        }
        [HttpGet]
        public ActionResult LoadMoreListOfEmptyContainerInvoice(int Page)
        {
            
            Dnd_ImportRepository ObjCR = new Dnd_ImportRepository();
            List<Dnd_ListOfImpInvoice> LstJO = new List<Dnd_ListOfImpInvoice>();
            ObjCR.ListOfEmptyInvoice(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstJO = (List<Dnd_ListOfImpInvoice>)ObjCR.DBResponse.Data;
            }
            return Json(LstJO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListOfEmptyContainerInvoiceSearch(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            Dnd_ImportRepository objER = new Dnd_ImportRepository();
            objER.ListOfEmptyInvoiceSearch(Module, InvoiceNo, InvoiceDate);
            List<Dnd_ListOfImpInvoice> obj = new List<Dnd_ListOfImpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<Dnd_ListOfImpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfEmptyContainerInvoice", obj);
        }
        #endregion

        #region Internal Movement

        [HttpGet]
        public ActionResult CreateInternalMovement()
        {
           Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
                ViewBag.BOENoList = new SelectList((List<Dnd_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
            }
            else
            {
                ViewBag.BOENoList = null;
            }



            ObjIR.ListOfInvernalMovementGodown();
            if (ObjIR.DBResponse.Data != null)
            {
                ViewBag.ListOfGodown = new SelectList((List<Models.DndGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetAllInternalMovementPageWise(0);
            List<Dnd_Internal_Movement> LstMovement = new List<Dnd_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<Dnd_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("ImpInternalMovementList", LstMovement);
        }
        [HttpGet]
        public JsonResult LoadListMoreInternalMovementList(int Page)
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetAllInternalMovementPageWise(Page);
            List<Dnd_Internal_Movement> LstMovement = new List<Dnd_Internal_Movement>();
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<Dnd_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return Json(LstMovement, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetInternalMovementListSearch(string OBLNo)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();

            List<Dnd_Internal_Movement> LstMovement = new List<Dnd_Internal_Movement>();
            ObjIR.GetAllMovementListSearch(OBLNo);
            if (ObjIR.DBResponse.Data != null)
            {
                LstMovement = (List<Dnd_Internal_Movement>)ObjIR.DBResponse.Data;
            }
            return PartialView("ImpInternalMovementList", LstMovement);
        }
        [HttpGet]
        public ActionResult EditInternalMovement(int MovementId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            Dnd_Internal_Movement ObjInternalMovement = new Dnd_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (Dnd_Internal_Movement)ObjIR.DBResponse.Data;
                ObjIR.ListOfInvernalMovementGodown();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.ListOfGodown = new SelectList((List<Models.DndGodownList>)ObjIR.DBResponse.Data, "GodownId", "GodownName");
                }
                else
                {
                    ViewBag.ListOfGodown = null;
                }


                ObjIR.GetBOENoForInternalMovement();
                if (ObjIR.DBResponse.Data != null)
                {
                    ViewBag.BOENoList = new SelectList((List<Dnd_Internal_Movement>)ObjIR.DBResponse.Data, "DestuffingEntryDtlId", "BOENo");
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
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            Dnd_Internal_Movement ObjInternalMovement = new Dnd_Internal_Movement();
            ObjIR.GetInternalMovement(MovementId);
            if (ObjIR.DBResponse.Data != null)
            {
                ObjInternalMovement = (Dnd_Internal_Movement)ObjIR.DBResponse.Data;
            }
            return PartialView(ObjInternalMovement);
        }

        [HttpGet]
        public JsonResult GetBOENoDetails(int DestuffingEntryDtlId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.GetBOENoDetForMovement(DestuffingEntryDtlId);
            return Json(ObjIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInternalPaymentSheet(int DestuffingId, string OBLNo, String MovementDate,
             string InvoiceType, int DestLocationIdiceId, int InvoiceId = 0)
        {

            Dnd_ImportRepository objChrgRepo = new Dnd_ImportRepository();
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

            var Output = (DNDInvoiceGodown)objChrgRepo.DBResponse.Data;

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
                    Output.lstPostPaymentCont.Add(new DNDPostPaymentContainer
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
            Dnd_ImportRepository objGPR = new Dnd_ImportRepository();
            objGPR.GetInvoiceDetailsForMovementPrintByNo(InvoiceNo, "IMPMovement");
            Dnd_InvoiceYard objGP = new Dnd_InvoiceYard();
            string FilePath = "";
            if (objGPR.DBResponse.Data != null)
            {
                objGP = (Dnd_InvoiceYard)objGPR.DBResponse.Data;
                FilePath = GeneratingPDFInvoiceMovement(objGP, objGP.InvoiceId);
                return Json(new { Status = 1, Message = FilePath });
            }
            else
                return Json(new { Status = -1, Message = "Error" });
        }
        private string GeneratingPDFInvoiceMovement(Dnd_InvoiceYard objGP, int InvoiceId)
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
            html.Append("<span style='display: block; font-size: 13px; line-height: 22px; text-transform: uppercase; padding-bottom: 10px;'>ICD DNode</span>");
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
        public JsonResult AddEditInternalPaymentSheet(Dnd_Internal_Movement objForm)
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


                Dnd_ImportRepository objChargeMaster = new Dnd_ImportRepository();
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
        public JsonResult AddEditInternalMovement(Dnd_Internal_Movement ObjInternalMovement)
        {

            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
            ObjIR.AddEditImpInternalMovement(ObjInternalMovement);
            return Json(ObjIR.DBResponse);


        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DelInternalMovement(int MovementId)
        {
            Dnd_ImportRepository ObjIR = new Dnd_ImportRepository();
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
            Dnd_ImportRepository objRepo = new Dnd_ImportRepository();
            objRepo.GetGodownLocationById(GodownId);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public JsonResult GetOBLWiseGodownList(int DestuffingEntryDtlId, string OBLNo, string OBLDate)
        {
            Dnd_ImportRepository objIR = new Dnd_ImportRepository();
            objIR.GetOBLWiseGodownList(DestuffingEntryDtlId, OBLNo, OBLDate);
            return Json(objIR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}