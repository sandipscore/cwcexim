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
    public class DSR_PestController : BaseController
    {
        #region Chemical Stock In
        public ActionResult CreateChemicalStockIn()
        {
            DSR_PestRepository objCR = new DSR_PestRepository();
            objCR.ListOfChemicalNames();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChemical =objCR.DBResponse.Data;
            else
                ViewBag.ListOfChemical = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditChemicalStockIn(int ChemicalStockId)
        {
            DSR_PestRepository ObjCh = new DSR_PestRepository();
            DSR_ChemicalStockIn ObjChem = new DSR_ChemicalStockIn();

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
                    ObjChem = (DSR_ChemicalStockIn)ObjCh.DBResponse.Data;

                }
            }
            return PartialView(ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemicalStockIn(int ChemicalStockId)
        {
            DSR_ChemicalStockIn ObjYard = new DSR_ChemicalStockIn();
            if (ChemicalStockId > 0)
            {
                DSR_PestRepository ObjYR = new DSR_PestRepository();
                ObjYR.GetChemical(ChemicalStockId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (DSR_ChemicalStockIn)ObjYR.DBResponse.Data;
                }
            }
            return PartialView(ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalStockList()
        {
            DSR_PestRepository ObjYR = new DSR_PestRepository();
            ObjYR.GetAllChemical();
            List<DSR_ChemicalStockIn> LstChemical = new List<DSR_ChemicalStockIn>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<DSR_ChemicalStockIn>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(DSR_ChemicalStockIn ObjChem)
        {
            if (ModelState.IsValid)
            {
                DSR_PestRepository ObjCR = new DSR_PestRepository();
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
            DSR_PestRepository objCR = new DSR_PestRepository();
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
            DSR_PestRepository ObjCh = new DSR_PestRepository();
            DSRChemicalConsumption ObjChem = new DSRChemicalConsumption();

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
                    ObjChem = (DSRChemicalConsumption)ObjCh.DBResponse.Data;

                }
            }
            return PartialView(ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemicalConsumption(int ChemicalConsumpId)
        {
            DSRChemicalConsumption ObjYard = new DSRChemicalConsumption();
            if (ChemicalConsumpId > 0)
            {
                DSR_PestRepository ObjYR = new DSR_PestRepository();
                ObjYR.ListOfChemicalNamesforConsumptionforID(ChemicalConsumpId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (DSRChemicalConsumption)ObjYR.DBResponse.Data;
                }
            }
            return PartialView(ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalConsumptionList()
        {
            DSR_PestRepository ObjYR = new DSR_PestRepository();
            ObjYR.GetAllChemicalConsumption();
            List<DSRChemicalConsumption> LstChemical = new List<DSRChemicalConsumption>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<DSRChemicalConsumption>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalListConsumption", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalConsumptionDetail(DSRChemicalConsumption ObjChem)
        {
            if (ModelState.IsValid)
            {
                DSR_PestRepository ObjCR = new DSR_PestRepository();
                IList<DSRChemicalConsump> LstChemical = JsonConvert.DeserializeObject<IList<DSRChemicalConsump>>(ObjChem.ChemicalXML);
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
        public ActionResult EditChemicalConsumptionDetail(DSRChemicalConsumption ObjChem)
        {
            if (ModelState.IsValid)
            {
                DSR_PestRepository ObjCR = new DSR_PestRepository();
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


            DSR_PestRepository objER = new DSR_PestRepository();
            //   objER.GetCCINShippingLine();
            //   if (objER.DBResponse.Data != null)
            //   {
            //       ViewBag.check = Newtonsoft.Json.JsonConvert.SerializeObject(objER.DBResponse.Data).ToString();
            //     ViewBag.ListOfShippingLine = objER.DBResponse.Data;
            //  }
            objER.ListOfExporterforPESTInvoice();
            if (objER.DBResponse.Data != null)
            {
                ViewBag.ListOfExporter = objER.DBResponse.Data;
            }
            //objER.ListOfCHA();
            //if (objER.DBResponse.Data != null)
            //{
            //    ViewBag.ListOfCHA = objER.DBResponse.Data;
            //}
            DSR_ExportRepository ObjER = new DSR_ExportRepository();
            //ObjER.GetAllCommodityForPage("", 0);
            //if (ObjER.DBResponse.Data != null)
            //{
            //    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjER.DBResponse.Data);
            //    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
            //    ViewBag.LstCommodity = Jobject["LstCommodity"];
            //    ViewBag.CommodityState = Jobject["State"];
            //}
            DSR_CountryRepository ObjCR = new DSR_CountryRepository();
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
            DSR_PestRepository objChe = new DSR_PestRepository();
            objChe.ListOfChemicalNameforInvoice();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ChemicalLst = null;

            DSR_PestRepository objExport = new DSR_PestRepository();
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
        public JsonResult GetPestControlCharges(decimal Amount, decimal HandlingAmount, string InvoiceType, int PartyId, List<Chemical> ChemicalDetails, string DeliveryDate, string ExportUnder = "", int Distance=0, int Cane=0)
        {
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }
            DSR_PestRepository objChargeMaster = new DSR_PestRepository();
            objChargeMaster.GetPestControlCharges(Amount, HandlingAmount, InvoiceType, PartyId, XMLText, DeliveryDate, ExportUnder, Distance, Cane);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPestControl(DSRPestControl FumigationModel)
        {
         
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                  //  IList<DSRChemicalConsump> LstChemical = JsonConvert.DeserializeObject<IList<DSRChemicalConsump>>(FumigationModel.ChemicalInvXML);
                  //  string ChemicalXml = Utility.CreateXML(LstChemical);
                    String InsideContXml = "";
                    String OutsideContXml = "";
                    if (FumigationModel.FumiType != null)
                    {
                        if (FumigationModel.FumiType.Equals("Inside"))
                        {
                            IList<insidecontainer> LstInsideCont = JsonConvert.DeserializeObject<IList<insidecontainer>>(FumigationModel.InsideContainerXml);
                            InsideContXml = Utility.CreateXML(LstInsideCont);
                        }

                        if (FumigationModel.FumiType.Equals("Outside"))
                        {
                            IList<outsidecontainer> LstOutsideCont = JsonConvert.DeserializeObject<IList<outsidecontainer>>(FumigationModel.OutsideContainerXml);
                            OutsideContXml = Utility.CreateXML(LstOutsideCont);
                        }
                    }
                    DSR_PestRepository objChargeMaster = new DSR_PestRepository();
                    objChargeMaster.AddEditPestControl(FumigationModel, InsideContXml, OutsideContXml,BranchId, ((Login)(Session["LoginUser"])).Uid, "PEST");


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
            DSR_PestRepository objExport = new DSR_PestRepository();
            objExport.GetPaymentPartyForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetQuantity(int ChemicalId,string BatchNo,string expirydate)
        {
            DSR_PestRepository objExport = new DSR_PestRepository();
            objExport.GetChemicalStock(ChemicalId, BatchNo, expirydate);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            DSR_PestRepository objExport = new DSR_PestRepository();
            objExport.GetPaymentPartyForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPayerList(string PartyCode, int Page)
        {
            DSR_PestRepository objExport = new DSR_PestRepository();
            objExport.GetPaymentPayerForPage(PartyCode, Page);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchPayerNameByPayeeCode(string PartyCode)
        {
            DSR_PestRepository objExport = new DSR_PestRepository();
            objExport.GetPaymentPayerForPage(PartyCode, 0);
            return Json(objExport.DBResponse, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ListOfExpInvoice(string Module, string InvoiceNo = null, string InvoiceDate = null)
        {
            DSR_ExportRepository objER = new DSR_ExportRepository();
            objER.ListOfExpInvoice(Module, InvoiceNo, InvoiceDate);
            List<CwcExim.Areas.Export.Models.DSRListOfExpInvoice> obj = new List<CwcExim.Areas.Export.Models.DSRListOfExpInvoice>();
            if (objER.DBResponse.Data != null)
                obj = (List<CwcExim.Areas.Export.Models.DSRListOfExpInvoice>)objER.DBResponse.Data;
            return PartialView("ListOfExpInvoice", obj);
        }



        [HttpGet]
        public ActionResult SearchContainerWise()
        {
            insidecontainer objContainer = new insidecontainer();
            DSR_PestRepository rep = new DSR_PestRepository();
            rep.GetContainerForPestInvoice();
            List<insidecontainer> obj = (List<insidecontainer>)rep.DBResponse.Data;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}