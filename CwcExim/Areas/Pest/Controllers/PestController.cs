using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Areas.Export.Models;
using CwcExim.Models;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using CwcExim.Filters;
using CwcExim.Areas.Pest.Models;

namespace CwcExim.Areas.Pest.Controllers
{
    public class PestController:Controller
    {
        #region Chemical Stock In
        public ActionResult CreateChemicalStockIn()
        {
            PestRepository objCR = new PestRepository();
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
            PestRepository ObjCh = new PestRepository();
            ChemicalStockIn ObjChem = new ChemicalStockIn();
          
            if (ChemicalStockId > 0)
            {
                ObjCh.ListOfChemicalName();
                if (ObjCh.DBResponse.Data != null)
                    ViewBag.ListOfChemical = ObjCh.DBResponse.Data;
                else
                    ViewBag.ListOfChemical = null;

                ObjCh.GetChemical(ChemicalStockId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (ChemicalStockIn)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditChemicalStockIn", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemicalStockIn(int ChemicalStockId)
        {
            ChemicalStockIn ObjYard = new ChemicalStockIn();
            if (ChemicalStockId > 0)
            {
                PestRepository ObjYR = new PestRepository();
                ObjYR.GetChemical(ChemicalStockId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (ChemicalStockIn)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemicalStockIn", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalStockList()

        {
            PestRepository ObjYR = new PestRepository();
            ObjYR.GetAllChemical();
            List<ChemicalStockIn> LstChemical = new List<ChemicalStockIn>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<ChemicalStockIn>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(ChemicalStockIn ObjChem)
        {
            if (ModelState.IsValid)
            {
                PestRepository ObjCR = new PestRepository();
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
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }



        #endregion

        #region Pest Control Operation Invoice
        [HttpGet]
        public ActionResult CreatePestControlOperationInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            ImportRepository objImport = new ImportRepository();

            objImport.GetPaymentParty();
            if (objImport.DBResponse.Status > 0)
                ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            else
                ViewBag.PaymentParty = null;
            PestRepository objChe = new PestRepository();
            objChe.ListOfChemicalName();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ChemicalLst = null;


            return PartialView();
        }

        [HttpPost]

        public JsonResult GetFumigation(decimal Amount, string InvoiceType, int PartyId, List<Chemical> ChemicalDetails)
        {
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }
            PestRepository objChargeMaster = new PestRepository();
            objChargeMaster.GetFumigation(Amount, InvoiceType, PartyId, XMLText);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditFumigationInvoice(PestControlOperation FumigationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                    IList<Chemical> LstChemical = JsonConvert.DeserializeObject<IList<Chemical>>(FumigationModel.ChemicalXML);
                    string ChemicalXml = Utility.CreateXML(LstChemical);
                    PestRepository objChargeMaster = new PestRepository();
                    objChargeMaster.AddEditFumigationInv(FumigationModel, ChemicalXml, BranchId, ((Login)(Session["LoginUser"])).Uid, "Fumigation");


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
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        
        #endregion
    }
}