using CwcExim.Areas.Pest.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CwcExim.UtilityClasses;

namespace CwcExim.Areas.Pest.Controllers
{
    public class WLJ_PestController : BaseController
    {
        #region Chemical Stock In
        public ActionResult CreateChemicalStockIn()
        {
            WLJ_PestRepository objCR = new WLJ_PestRepository();
            objCR.ListOfChemicalNames();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChemical = objCR.DBResponse.Data;
            else
                ViewBag.ListOfChemical = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditChemicalStockIn(int ChemicalStockId)
        {
            WLJ_PestRepository ObjCh = new WLJ_PestRepository();
            WLJ_ChemicalStockIn ObjChem = new WLJ_ChemicalStockIn();

            if (ChemicalStockId > 0)
            {
                //ObjCh.ListOfChemicalName();
                //if (ObjCh.DBResponse.Data != null)
                //    ViewBag.ListOfChemical = ObjCh.DBResponse.Data;
                //else
                //    ViewBag.ListOfChemical = null;

                ObjCh.GetChemical(ChemicalStockId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (WLJ_ChemicalStockIn)ObjCh.DBResponse.Data;

                }
            }
            return PartialView(ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemicalStockIn(int ChemicalStockId)
        {
            WLJ_ChemicalStockIn ObjYard = new WLJ_ChemicalStockIn();
            if (ChemicalStockId > 0)
            {
                WLJ_PestRepository ObjYR = new WLJ_PestRepository();
                ObjYR.GetChemical(ChemicalStockId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (WLJ_ChemicalStockIn)ObjYR.DBResponse.Data;
                }
            }
            return PartialView(ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalStockList()
        {
            WLJ_PestRepository ObjYR = new WLJ_PestRepository();
            ObjYR.GetAllChemical();
            List<WLJ_ChemicalStockIn> LstChemical = new List<WLJ_ChemicalStockIn>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<WLJ_ChemicalStockIn>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(WLJ_ChemicalStockIn ObjChem)
        {
            if (ModelState.IsValid)
            {
                WLJ_PestRepository ObjCR = new WLJ_PestRepository();
                ObjChem.ChemicalName = ObjChem.ChemicalName;
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChem.Uid = ObjLogin.Uid;
                ObjChem.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditChemical(ObjChem);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion


        #region Chemical Consumption
        public ActionResult CreateChemicalConsumption()
        {
            WLJ_PestRepository objCR = new WLJ_PestRepository();
            objCR.ListOfChemicalNamesforConsumption();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChemical = objCR.DBResponse.Data;
            else
                ViewBag.ListOfChemical = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditChemicalConsumption(int ChemicalConsumpId)
        {
            WLJ_PestRepository ObjCh = new WLJ_PestRepository();
            WLJChemicalConsumption ObjChem = new WLJChemicalConsumption();

            if (ChemicalConsumpId > 0)
            {
                //ObjCh.ListOfChemicalName();
                //if (ObjCh.DBResponse.Data != null)
                //    ViewBag.ListOfChemical = ObjCh.DBResponse.Data;
                //else
                //    ViewBag.ListOfChemical = null;

                ObjCh.ListOfChemicalNamesforConsumptionforID(ChemicalConsumpId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (WLJChemicalConsumption)ObjCh.DBResponse.Data;

                }
            }
            return PartialView(ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemicalConsumption(int ChemicalConsumpId)
        {
            WLJChemicalConsumption ObjYard = new WLJChemicalConsumption();
            if (ChemicalConsumpId > 0)
            {
                WLJ_PestRepository ObjYR = new WLJ_PestRepository();
                ObjYR.ListOfChemicalNamesforConsumptionforID(ChemicalConsumpId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (WLJChemicalConsumption)ObjYR.DBResponse.Data;
                }
            }
            return PartialView(ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalConsumptionList()
        {
            WLJ_PestRepository ObjYR = new WLJ_PestRepository();
            ObjYR.GetAllChemicalConsumption();
            List<WLJChemicalConsumption> LstChemical = new List<WLJChemicalConsumption>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<WLJChemicalConsumption>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalListConsumption", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalConsumptionDetail(WLJChemicalConsumption ObjChem)
        {
            if (ModelState.IsValid)
            {
                WLJ_PestRepository ObjCR = new WLJ_PestRepository();
                IList<WLJChemicalConsump> LstChemical = JsonConvert.DeserializeObject<IList<WLJChemicalConsump>>(ObjChem.ChemicalXML);
                string ChemicalXML = Utility.CreateXML(LstChemical);

                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChem.Uid = ObjLogin.Uid;
                ObjChem.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditChemicalConsumption(ObjChem, ChemicalXML);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditChemicalConsumptionDetail(WLJChemicalConsumption ObjChem)
        {
            if (ModelState.IsValid)
            {
                WLJ_PestRepository ObjCR = new WLJ_PestRepository();
                ObjChem.ChemicalName = ObjChem.ChemicalName;
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChem.Uid = ObjLogin.Uid;
                ObjChem.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.EditChemicalConsumption(ObjChem);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion


        #region Pest Control Operation Invoice
        [HttpGet]
        public ActionResult CreatePestControlOperationInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;

            WLJ_PestRepository objER = new WLJ_PestRepository();
            //   objER.GetCCINShippingLine();
            //   if (objER.DBResponse.Data != null)
            //   {
            //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
            //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            //  }
            //objER.ListOfExporterforPESTInvoice();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfExporter = objER.DBResponse.Data;
            //}
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}
            WLJ_ExportRepository ObjER = new WLJ_ExportRepository();
            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }

            //ImportRepository objImport = new ImportRepository();
            //objImport.GetPaymentParty();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            //WLJ_PestRepository objChe = new WLJ_PestRepository();
            //objChe.ListOfChemicalNameforInvoice();
            //if (objChe.DBResponse.Status > 0)
            //    ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            //else
            //    ViewBag.ChemicalLst = null;

            WLJ_PestRepository objExport = new WLJ_PestRepository();
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

        [HttpPost]
        public JsonResult GetPestControlCharges(decimal Amount,  string InvoiceType,string SEZ, int PartyId, string SAC)
        {            
            WLJ_PestRepository objChargeMaster = new WLJ_PestRepository();
            objChargeMaster.GetPestControlCharges(Amount, InvoiceType,SEZ, PartyId, SAC);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public ActionResult ListOfPestApplication()
        //{

        //    DSR_ImportRepository ObjIR = new DSR_ImportRepository();
        //  //  List<DSRMergeDeliveryApplicationList> LstDelivery = new List<DSRMergeDeliveryApplicationList>();
        //    ObjIR.GetAllDeliveryMergeApplication(0, ((Login)(Session["LoginUser"])).Uid);
        //    if (ObjIR.DBResponse.Data != null)
        //  //      LstDelivery = (List<DSRMergeDeliveryApplicationList>)ObjIR.DBResponse.Data;
        //    return PartialView("DeliveryMergeApplicationList", LstDelivery);
        //}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPestControl(WLJPestControl FumigationModel)
        {

            try
            {
                int BranchId = Convert.ToInt32(Session["BranchId"]);
               
                String InsideContXml = "";               
                
                IList<WLJinsidecontainer> LstInsideCont = JsonConvert.DeserializeObject<IList<WLJinsidecontainer>>(FumigationModel.WLJInsideContainerXml);
                InsideContXml = Utility.CreateXML(LstInsideCont);                                     
                
                WLJ_PestRepository objChargeMaster = new WLJ_PestRepository();
                objChargeMaster.AddEditPestControl(FumigationModel, InsideContXml, BranchId, ((Login)(Session["LoginUser"])).Uid, "PEST");


                FumigationModel.InvoiceNo = Convert.ToString(objChargeMaster.DBResponse.Data);
                objChargeMaster.DBResponse.Data = FumigationModel;
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
            WLJ_PestRepository objExport = new WLJ_PestRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetQuantity(int ChemicalId, string BatchNo, string expirydate)
        {
            WLJ_PestRepository objExport = new WLJ_PestRepository();
            objExport.GetChemicalStock(ChemicalId, BatchNo, expirydate);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            WLJ_PestRepository objExport = new WLJ_PestRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            WLJ_PestRepository objExport = new WLJ_PestRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            WLJ_PestRepository objExport = new WLJ_PestRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            WLJ_ExportRepository objER = new WLJ_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<CwcExim.Areas.Export.Models.WLJListOfExpInvoice> obj = new List<CwcExim.Areas.Export.Models.WLJListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CwcExim.Areas.Export.Models.WLJListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }



        //[HttpGet]
        //public ActionResult SearchContainerWise()
        //{
        //    WLJinsidecontainer objContainer = new WLJinsidecontainer();
        //    WLJ_PestRepository rep = new WLJ_PestRepository();
        //    rep.GetContainerForPestInvoice();
        //    List<WLJinsidecontainer> obj = (List<WLJinsidecontainer>)rep.DBResponse.Data;
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}