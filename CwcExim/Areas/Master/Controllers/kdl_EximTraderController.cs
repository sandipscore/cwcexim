using CwcExim.Areas.Master.Models;
using CwcExim.Filters;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Master.Controllers
{
    public class kdl_EximTraderController : Controller
    {
        // GET: Master/kdl_EximTrader
        [HttpGet]
        public ActionResult CreateEximTrader() //test
        {
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            return View("CreateEximTrader");
        }

        [HttpGet]
        public ActionResult GetEximTraderList()
        {
            kdl_EximTraderRepository ObjETR = new kdl_EximTraderRepository();
            List<kdl_EximTrader> LstEximTrader = new List<kdl_EximTrader>();
            ObjETR.GetAllEximTrader();
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<kdl_EximTrader>)ObjETR.DBResponse.Data;
            }
            return View("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            kdl_EximTrader ObjEximTrader = new kdl_EximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                kdl_EximTraderRepository ObjETR = new kdl_EximTraderRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (kdl_EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return View("EditEximTrader", ObjEximTrader);
        } 

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            kdl_EximTrader ObjEximTrader = new kdl_EximTrader();
            if (EximTraderId > 0)
            {
                kdl_EximTraderRepository ObjETR = new kdl_EximTraderRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (kdl_EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return View("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(kdl_EximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                kdl_EximTraderRepository ObjETR = new kdl_EximTraderRepository();
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
                kdl_EximTraderRepository ObjETR = new kdl_EximTraderRepository();
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