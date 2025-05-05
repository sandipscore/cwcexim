using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Controllers;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Filters;
using CwcExim.Areas.Master.Models;
using CwcExim.Areas.CashManagement.Models;

namespace CwcExim.Areas.Master.Controllers
{
    public class DndEximTraderController : BaseController
    {
        // GET: Master/DndEximTrader
        #region Exim Trader Finance Control

        [HttpGet]
        public ActionResult CreateEximTraderFncControl()
        {
            DndEximTraderFinanceControl objFC = new DndEximTraderFinanceControl();
            DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
            ObjETFCR.GetEximTraderFinanceControl("", 0);
            if (ObjETFCR.DBResponse.Data != null)
            {
                ViewBag.lstExim  = ((SearchEximTraderDataFinanceControl)ObjETFCR.DBResponse.Data).lstExim;
            }
            else
            {
                ViewBag.lstExim = null;
            }
            return PartialView("CreateEximTraderFncControl", objFC);
        }
        [HttpGet]
        public JsonResult GetEximTraderNew(int EximTraderId)
        {
            DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
            ObjETFCR.GetEximTraderNew(EximTraderId);

            return Json(ObjETFCR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEximTraderFncControlList()
        {
            List<DndEximTraderFinanceControl> LstEximTrader = new List<DndEximTraderFinanceControl>();
            DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DndEximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return PartialView("EximTraderFncControlList", LstEximTrader);
        }

        [HttpGet]
        public ActionResult EditEximTraderFncControl(int FinanceControlId)
        {
            DndEximTraderFinanceControl ObjEximTrader = new DndEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DndEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTraderFncControl", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderFncControl(int FinanceControlId)
        {
            DndEximTraderFinanceControl ObjEximTrader = new DndEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DndEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTraderFncControl", ObjEximTrader);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DelEximTraderFncControl(int FinanceControlId)
        {
            if (FinanceControlId > 0)
            {
                DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
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
        public ActionResult AddEditEximTraderFncControl(DndEximTraderFinanceControl ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
                ObjETFCR.AddEditEximFinanceControl(ObjEximTrader);
                ModelState.Clear();
                return Json(ObjETFCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            DNODEMasterRepository objRepo = new DNODEMasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            DNODEMasterRepository objRepo = new DNODEMasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchEximTraderFncCntrl(string EximName)
        {
            DNODEMasterRepository ETGR = new DNODEMasterRepository();
            ETGR.SearchEximFinanceControl(EximName);
            List<DndEximTraderFinanceControl> LstEximTrader = new List<DndEximTraderFinanceControl>();

            if (ETGR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DndEximTraderFinanceControl>)ETGR.DBResponse.Data;
            }
            return PartialView("EximTraderFncControlList", LstEximTrader);

        }

        #endregion

        #region Remove Credit Limit
        [HttpGet]
        public ActionResult RemoveEximFncCreditLimit()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCreditLimit()
        {
            
                DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
                ObjETFCR.RemoveEximFinanceCreditLimit();
            if(ObjETFCR.DBResponse.Status==1)
            { 
                return Json(ObjETFCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }



        [HttpGet]
        public ActionResult GetEximTraderFncCreditLimitList()
        {
            List<DndEximTraderFinanceControl> LstEximTrader = new List<DndEximTraderFinanceControl>();
            DNODEMasterRepository ObjETFCR = new DNODEMasterRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DndEximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return PartialView("EximTraderFncCreditLimitList", LstEximTrader);
        }

        #endregion

        #region Exim Trader Master

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
            return PartialView();
        }


        public ActionResult GetEximTraderList()
        {
            DNODEMasterRepository ObjETR = new DNODEMasterRepository();
            List<DndEximTrader> LstEximTrader = new List<DndEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DndEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            DNODEMasterRepository ObjCR = new DNODEMasterRepository();
            List<DndEximTrader> LstCommodity = new List<DndEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<DndEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        } 


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode, int TypeOfPartyId)
        {
            DNODEMasterRepository ObjETR = new DNODEMasterRepository();
            List<DndEximTrader> LstEximTrader = new List<DndEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode, TypeOfPartyId);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DndEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            DndEximTrader ObjEximTrader = new DndEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                DNODEMasterRepository ObjETR = new DNODEMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DndEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            DndEximTrader ObjEximTrader = new DndEximTrader();
            if (EximTraderId > 0)
            {
                DNODEMasterRepository ObjETR = new DNODEMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DndEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(DndEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                DNODEMasterRepository ObjETR = new DNODEMasterRepository();
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
                DNODEMasterRepository ObjETR = new DNODEMasterRepository();
                ObjETR.DeleteEximTrader(EximTraderId);
                return Json(ObjETR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        #endregion
    }
}