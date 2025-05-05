using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Filters;

namespace CwcExim.Controllers
{
    public class EximTraderFncCntrlController : BaseController
    {
        [HttpGet]
        public ActionResult CreateEximTraderFncControl()
       {
            EximTraderFinanceControl objFC = new EximTraderFinanceControl();
            EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
            ObjETFCR.GetEximTrader();
            if (ObjETFCR.DBResponse.Data != null)
            {
                objFC.LstEximTraders=(List<EximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return View("CreateEximTraderFncControl", objFC);
       }
        [HttpGet]
        public JsonResult GetEximTraderNew(int EximTraderId)
        {
            EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
            ObjETFCR.GetEximTraderNew(EximTraderId);

            return Json(ObjETFCR.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderFncControlList()
        {
            List<EximTraderFinanceControl> LstEximTrader = new List<EximTraderFinanceControl>();
            EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<EximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return View("EximTraderFncControlList", LstEximTrader);
        }

        [HttpGet]
        public ActionResult EditEximTraderFncControl(int FinanceControlId)
        {
            EximTraderFinanceControl ObjEximTrader = new EximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader=(EximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return View("EditEximTraderFncControl", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderFncControl(int FinanceControlId)
        {
            EximTraderFinanceControl ObjEximTrader = new EximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (EximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return View("ViewEximTraderFncControl", ObjEximTrader);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DelEximTraderFncControl(int FinanceControlId)
        {
            if (FinanceControlId > 0)
            {
                EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
                ObjETFCR.DeleteEximFinanceControl(FinanceControlId);
                return Json(ObjETFCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderFncControl(EximTraderFinanceControl ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid=ObjLogin.Uid;
                EximTraderFncCntrlRepository ObjETFCR = new EximTraderFncCntrlRepository();
                ObjETFCR.AddEditEximFinanceControl(ObjEximTrader);
                ModelState.Clear();
                return Json(ObjETFCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",",ModelState.Values.SelectMany(m=>m.Errors).Select(e=>e.ErrorMessage));
                var Err = new { Status=0,Message=ErrorMessage};
                return Json(Err);
            }
        }
    }
}