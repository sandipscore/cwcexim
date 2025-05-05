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

namespace CwcExim.Areas.Master
{
    public class WHTEximTraderController : BaseController
    {
        // GET: Master/WFLDEximTrader
        public ActionResult Index()
        {
            return View();
        }


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
            WHT_MasterRepository ObjETR = new WHT_MasterRepository();
            List<WHTEximTrader> LstEximTrader = new List<WHTEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<WHTEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            List<WHTEximTrader> LstCommodity = new List<WHTEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<WHTEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            WHT_MasterRepository ObjETR = new WHT_MasterRepository();
            List<WHTEximTrader> LstEximTrader = new List<WHTEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<WHTEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            WHTEximTrader ObjEximTrader = new WHTEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WHTEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            WHTEximTrader ObjEximTrader = new WHTEximTrader();
            if (EximTraderId > 0)
            {
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WHTEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(WHTEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
                ObjETR.DeleteEximTrader(EximTraderId);
                return Json(ObjETR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }

        #region Party Wise TDS Deposit

        public ActionResult PartyWiseTDSDeposit(int Id = 0)
        {
            var objRepo = new WHT_MasterRepository();
            objRepo.GetAllEximTraderFilterWise("All");
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.lstParty = (List<Party>)objRepo.DBResponse.Data;
            }

            WFLDPartyWiseTDSDeposit objPartyWiseTDSDeposit = new WFLDPartyWiseTDSDeposit();

            if (Id > 0)
            {
                objRepo.GetPartyWiseTDSDepositDetails(Id);
                if (objRepo.DBResponse.Data != null)
                {
                    objPartyWiseTDSDeposit = (WFLDPartyWiseTDSDeposit)objRepo.DBResponse.Data;
                }
            }

            return PartialView(objPartyWiseTDSDeposit);
        }

        public ActionResult ViewPartyWiseTDSDeposit(int Id = 0)
        {
            var objRepo = new WHT_MasterRepository();
            WFLDPartyWiseTDSDeposit objPartyWiseTDSDeposit = new WFLDPartyWiseTDSDeposit();
            if (Id > 0)
            {
                objRepo.GetPartyWiseTDSDepositDetails(Id);
                if (objRepo.DBResponse.Data != null)
                {
                    objPartyWiseTDSDeposit = (WFLDPartyWiseTDSDeposit)objRepo.DBResponse.Data;
                }
            }

            return PartialView(objPartyWiseTDSDeposit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPartyWiseTDSDeposit(WFLDPartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objER = new WHT_MasterRepository();
                objER.AddEditPartyWiseTDSDeposit(objPartyWiseTDSDeposit);
                return Json(objER.DBResponse);
            }
            else
            {
                var data = new { Status = -1 };
                return Json(data);
            }
        }

        [HttpGet]
        public ActionResult ListOfPartyWiseTDSDeposit()
        {
            WHT_MasterRepository objER = new WHT_MasterRepository();
            List<WFLDPartyWiseTDSDeposit> lstPartyWiseTDSDeposit = new List<WFLDPartyWiseTDSDeposit>();
            objER.GetAllPartyWiseTDSDeposit();
            if (objER.DBResponse.Data != null)
                lstPartyWiseTDSDeposit = (List<WFLDPartyWiseTDSDeposit>)objER.DBResponse.Data;
            return PartialView(lstPartyWiseTDSDeposit);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeletePartyWiseTDSDeposit(int PartyWiseTDSDepositId, string Amount,string ReceiptNo)
        {
            WHT_MasterRepository objER = new WHT_MasterRepository();
            if (PartyWiseTDSDepositId > 0)
                objER.DeletePartyWiseTDSDeposit(PartyWiseTDSDepositId, Amount, ReceiptNo);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Exim Trader Finance Control

        [HttpGet]
        public ActionResult CreateEximTraderFncControl()
        {
            WFLDEximTraderFinanceControl objFC = new WFLDEximTraderFinanceControl();
            WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
            //ObjETFCR.GetEximTraderFinanceControl("", 0);
            //if (ObjETFCR.DBResponse.Data != null)
            //{
            //    ViewBag.lstExim  = ((SearchEximTraderDataFinanceControl)ObjETFCR.DBResponse.Data).lstExim;
            //}
            return PartialView("CreateEximTraderFncControl", objFC);
        }
        [HttpGet]
        public JsonResult GetEximTraderNew(int EximTraderId)
        {
            WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
            ObjETFCR.GetEximTraderNew(EximTraderId);

            return Json(ObjETFCR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEximTraderFncControlList()
        {
            List<WFLDEximTraderFinanceControl> LstEximTrader = new List<WFLDEximTraderFinanceControl>();
            WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<WFLDEximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return PartialView("EximTraderFncControlList", LstEximTrader);
        }

        [HttpGet]
        public ActionResult EditEximTraderFncControl(int FinanceControlId)
        {
            WFLDEximTraderFinanceControl ObjEximTrader = new WFLDEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WFLDEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTraderFncControl", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderFncControl(int FinanceControlId)
        {
            WFLDEximTraderFinanceControl ObjEximTrader = new WFLDEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WFLDEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
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
                WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
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
        public ActionResult AddEditEximTraderFncControl(WFLDEximTraderFinanceControl ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjETFCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}