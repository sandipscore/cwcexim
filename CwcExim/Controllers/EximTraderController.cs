using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Filters;

namespace CwcExim.Controllers
{
    public class EximTraderController : BaseController
    {
        [HttpGet]
        public ActionResult CreateEximTrader()
        {
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            return View("CreateEximTrader");
            // return PartialView();
        }

        [HttpGet]
        public ActionResult GetEximTraderList()
        {
            EximTraderRepository ObjETR = new EximTraderRepository();
            List<EximTrader> LstEximTrader = new List<EximTrader>();
            ObjETR.GetAllEximTrader(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<EximTrader>)ObjETR.DBResponse.Data;
            }
            return View("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            EximTraderRepository ObjCR = new EximTraderRepository();
            List<EximTrader> LstCommodity = new List<EximTrader>();
            ObjCR.GetAllEximTrader(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<EximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            EximTraderRepository ObjCR = new EximTraderRepository();
            List<EximTrader> LstEximTrader = new List<EximTrader>();
            ObjCR.GetAllEximTraderPartyCode(PartyCode);
            if (ObjCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<EximTrader>)ObjCR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            EximTrader ObjEximTrader = new EximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                EximTraderRepository ObjETR = new EximTraderRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return View("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            EximTrader ObjEximTrader = new EximTrader();
            if (EximTraderId > 0)
            {
                EximTraderRepository ObjETR = new EximTraderRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return View("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(EximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                EximTraderRepository ObjETR = new EximTraderRepository();
                ObjETR.AddEditEximTrader(ObjEximTrader);
                ModelState.Clear();
                return Json(ObjETR.DBResponse);
            }
            else
            {
                ObjEximTrader.Password = ObjEximTrader.HdnPassword;
                ObjEximTrader.ConfirmPassword = ObjEximTrader.ConfirmPassword;
                var ErroMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErroMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteEximTraderDetail(int EximTraderId)
        {
            if (EximTraderId > 0)
            {
                EximTraderRepository ObjETR = new EximTraderRepository();
                ObjETR.DeleteEximTrader(EximTraderId);
                return Json(ObjETR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
    }
}