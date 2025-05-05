using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Master.Controllers
{
    public class SDOpeningController : BaseController
    {
        // GET: Master/SDOpening
        [HttpGet]
        public ActionResult CreateSDopening()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            PPGMasterRepository ObjRepo = new PPGMasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            SearchEximTraderData obj = new SearchEximTraderData();
            ObjRepo.GetEximTrader("",0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((SearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((SearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new SDOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new PPGReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.CurDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView("CreateSDopening", model);
        }

        [HttpGet]
        public ActionResult GetSDList()
        {
            PPGMasterRepository ObjSDR = new PPGMasterRepository();
            List<SDOpening> LstSD = new List<SDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<SDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult GetSDListPartyCode(string PartyCode)
        {
            PPGMasterRepository ObjSDR = new PPGMasterRepository();
            List<SDOpening> LstSD = new List<SDOpening>();
            ObjSDR.GetSDListPartyCode(PartyCode);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<SDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            SDOpening ObjSD = new SDOpening();
            if (SDId > 0)
            {
                PPGMasterRepository ObjSDR = new PPGMasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (SDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(SDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                PPGMasterRepository ObjSDR = new PPGMasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSD.Uid = ObjLogin.Uid;
                ObjSDR.AddSDOpening(ObjSD, xml);
                ModelState.Clear();
                return Json(ObjSDR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode,int Page)
        {
            PPGMasterRepository objRepo = new PPGMasterRepository();
            objRepo.GetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            PPGMasterRepository objRepo = new PPGMasterRepository();
            objRepo.SearchByPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

    }
}