using CwcExim.Areas.Pest.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Pest.Controllers
{
    public class Hdb_PestController : BaseController
    {
        #region Pest Control Operation Invoice
        [HttpGet]
        public ActionResult CreatePestControlOperationInvoice(string type = "Tax")
        {
            ViewData["InvType"] = type;
            //ImportRepository objImport = new ImportRepository();
            //objImport.GetPaymentParty();
            //if (objImport.DBResponse.Status > 0)
            //    ViewBag.PaymentParty = JsonConvert.SerializeObject(objImport.DBResponse.Data);
            //else
            //    ViewBag.PaymentParty = null;
            PestRepository objChe = new PestRepository();
            objChe.ListOfChemicalName();
            if (objChe.DBResponse.Status > 0)
                ViewBag.ChemicalLst = JsonConvert.SerializeObject(objChe.DBResponse.Data);
            else
                ViewBag.ChemicalLst = null;

            Hdb_ExportRepository objExport = new Hdb_ExportRepository();
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

        public JsonResult GetAllHTClause()
        {
            Hdb_PestRepository ObjER = new Hdb_PestRepository();
            ObjER.ListOfAllHTClause();
            List<Hdb_PestControl> lstPest = new List<Hdb_PestControl>();
            if (ObjER.DBResponse.Data != null)
            {
                lstPest = (List<Hdb_PestControl>)ObjER.DBResponse.Data;
            }

            return Json(lstPest, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetPestControlCharges(decimal Amount, decimal HandlingAmount, string InvoiceType, int PartyId, List<Chemical> ChemicalDetails,string ExportUnder="")
        {
            string XMLText = "";
            if (ChemicalDetails != null)
            {
                XMLText = Utility.CreateXML(ChemicalDetails);
            }
            Hdb_PestRepository objChargeMaster = new Hdb_PestRepository();
            objChargeMaster.GetPestControlCharges(Amount, HandlingAmount, InvoiceType, PartyId, XMLText, ExportUnder);
            return Json(objChargeMaster.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPestControl(Hdb_PestControl FumigationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int BranchId = Convert.ToInt32(Session["BranchId"]);

                    IList<Chemical> LstChemical = JsonConvert.DeserializeObject<IList<Chemical>>(FumigationModel.ChemicalXML);
                    string ChemicalXml = Utility.CreateXML(LstChemical);
                    Hdb_PestRepository objChargeMaster = new Hdb_PestRepository();
                    objChargeMaster.AddEditPestControl(FumigationModel, ChemicalXml, BranchId, ((Login)(Session["LoginUser"])).Uid, "PEST");


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