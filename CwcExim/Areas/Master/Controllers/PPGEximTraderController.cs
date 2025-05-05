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
    public class PPGEximTraderController : BaseController
    {
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
            PPGMasterRepository ObjETR = new PPGMasterRepository();
            List<PPGEximTrader> LstEximTrader = new List<PPGEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<PPGEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            PPGMasterRepository ObjCR = new PPGMasterRepository();
            List<PPGEximTrader> LstCommodity = new List<PPGEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<PPGEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {           
            PPGMasterRepository ObjETR = new PPGMasterRepository();
            List<PPGEximTrader> LstEximTrader = new List<PPGEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<PPGEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            PPGEximTrader ObjEximTrader = new PPGEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                PPGMasterRepository ObjETR = new PPGMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (PPGEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            PPGEximTrader ObjEximTrader = new PPGEximTrader();
            if (EximTraderId > 0)
            {
                PPGMasterRepository ObjETR = new PPGMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (PPGEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(PPGEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                PPGMasterRepository ObjETR = new PPGMasterRepository();
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
                PPGMasterRepository ObjETR = new PPGMasterRepository();
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

        #region Party Wise TDS Deposit

        public ActionResult PartyWiseTDSDeposit(int Id = 0)
        {
            var objRepo = new PPGMasterRepository();
            objRepo.GetAllEximTraderFilterWise("All");
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.lstParty = (List<Party>)objRepo.DBResponse.Data;
            }

            PartyWiseTDSDeposit objPartyWiseTDSDeposit = new PartyWiseTDSDeposit();

            if (Id > 0)
            {
                objRepo.GetPartyWiseTDSDepositDetails(Id);
                if (objRepo.DBResponse.Data != null)
                {
                    objPartyWiseTDSDeposit = (PartyWiseTDSDeposit)objRepo.DBResponse.Data;
                }
            }

            return PartialView(objPartyWiseTDSDeposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditPartyWiseTDSDeposit(PartyWiseTDSDeposit objPartyWiseTDSDeposit)
        {
            if (ModelState.IsValid)
            {
                PPGMasterRepository objER = new PPGMasterRepository();
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
            PPGMasterRepository objER = new PPGMasterRepository();
            List<PartyWiseTDSDeposit> lstPartyWiseTDSDeposit = new List<PartyWiseTDSDeposit>();
            objER.GetAllPartyWiseTDSDeposit();
            if (objER.DBResponse.Data != null)
                lstPartyWiseTDSDeposit = (List<PartyWiseTDSDeposit>)objER.DBResponse.Data;
            return PartialView(lstPartyWiseTDSDeposit);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeletePartyWiseTDSDeposit(int PartyWiseTDSDepositId) 
        {
            PPGMasterRepository objER = new PPGMasterRepository();
            if (PartyWiseTDSDepositId > 0)
                objER.DeletePartyWiseTDSDeposit(PartyWiseTDSDepositId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Exim Trader Finance Control

        [HttpGet]
        public ActionResult CreateEximTraderFncControl()
        {
            PpgEximTraderFinanceControl objFC = new PpgEximTraderFinanceControl();
            PPGMasterRepository ObjETFCR = new PPGMasterRepository();
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
            PPGMasterRepository ObjETFCR = new PPGMasterRepository();
            ObjETFCR.GetEximTraderNew(EximTraderId);

            return Json(ObjETFCR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEximTraderFncControlList()
        {
            List<PpgEximTraderFinanceControl> LstEximTrader = new List<PpgEximTraderFinanceControl>();
            PPGMasterRepository ObjETFCR = new PPGMasterRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<PpgEximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return PartialView("EximTraderFncControlList", LstEximTrader);
        }

        [HttpGet]
        public ActionResult EditEximTraderFncControl(int FinanceControlId)
        {
            PpgEximTraderFinanceControl ObjEximTrader = new PpgEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                PPGMasterRepository ObjETFCR = new PPGMasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (PpgEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTraderFncControl", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderFncControl(int FinanceControlId)
        {
            PpgEximTraderFinanceControl ObjEximTrader = new PpgEximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                PPGMasterRepository ObjETFCR = new PPGMasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (PpgEximTraderFinanceControl)ObjETFCR.DBResponse.Data;
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
                PPGMasterRepository ObjETFCR = new PPGMasterRepository();
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
        public ActionResult AddEditEximTraderFncControl(PpgEximTraderFinanceControl ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                PPGMasterRepository ObjETFCR = new PPGMasterRepository();
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
            PPGMasterRepository objRepo = new PPGMasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            PPGMasterRepository objRepo = new PPGMasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}