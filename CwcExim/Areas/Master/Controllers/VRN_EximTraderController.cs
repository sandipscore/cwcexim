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
    public class VRN_EximTraderController : BaseController
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
            VRN_MasterRepository ObjETR = new VRN_MasterRepository();
            List<VRN_EximTrader> LstEximTrader = new List<VRN_EximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<VRN_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            VRN_MasterRepository ObjCR = new VRN_MasterRepository();
            List<VRN_EximTrader> LstCommodity = new List<VRN_EximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<VRN_EximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {           
            VRN_MasterRepository ObjETR = new VRN_MasterRepository();
            List<VRN_EximTrader> LstEximTrader = new List<VRN_EximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<VRN_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            VRN_EximTrader ObjEximTrader = new VRN_EximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                VRN_MasterRepository ObjETR = new VRN_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (VRN_EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            VRN_EximTrader ObjEximTrader = new VRN_EximTrader();
            if (EximTraderId > 0)
            {
                VRN_MasterRepository ObjETR = new VRN_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (VRN_EximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(VRN_EximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                VRN_MasterRepository ObjETR = new VRN_MasterRepository();
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
                VRN_MasterRepository ObjETR = new VRN_MasterRepository();
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
            var objRepo = new VRN_MasterRepository();
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
                VRN_MasterRepository objER = new VRN_MasterRepository();
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
            VRN_MasterRepository objER = new VRN_MasterRepository();
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
            VRN_MasterRepository objER = new VRN_MasterRepository();
            if (PartyWiseTDSDepositId > 0)
                objER.DeletePartyWiseTDSDeposit(PartyWiseTDSDepositId);
            return Json(objER.DBResponse);
        }

        #endregion

        #region Exim Trader Finance Control

        [HttpGet]
        public ActionResult CreateEximTraderFncControl()
        {
            VRN_EximTraderFinanceControl objFC = new VRN_EximTraderFinanceControl();
            VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
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
            VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
            ObjETFCR.GetEximTraderNew(EximTraderId);

            return Json(ObjETFCR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEximTraderFncControlList()
        {
            List<VRN_EximTraderFinanceControl> LstEximTrader = new List<VRN_EximTraderFinanceControl>();
            VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
            ObjETFCR.GetAllEximFinanceControl();
            if (ObjETFCR.DBResponse.Data != null)
            {
                LstEximTrader = (List<VRN_EximTraderFinanceControl>)ObjETFCR.DBResponse.Data;
            }
            return PartialView("EximTraderFncControlList", LstEximTrader);
        }

        [HttpGet]
        public ActionResult EditEximTraderFncControl(int FinanceControlId)
        {
            VRN_EximTraderFinanceControl ObjEximTrader = new VRN_EximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (VRN_EximTraderFinanceControl)ObjETFCR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTraderFncControl", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderFncControl(int FinanceControlId)
        {
            VRN_EximTraderFinanceControl ObjEximTrader = new VRN_EximTraderFinanceControl();
            if (FinanceControlId > 0)
            {
                VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
                ObjETFCR.GetEximFinanceControl(FinanceControlId);
                if (ObjETFCR.DBResponse.Data != null)
                {
                    ObjEximTrader = (VRN_EximTraderFinanceControl)ObjETFCR.DBResponse.Data;
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
                VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
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
        public ActionResult AddEditEximTraderFncControl(VRN_EximTraderFinanceControl ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                VRN_MasterRepository ObjETFCR = new VRN_MasterRepository();
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
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.GetEximTraderFinanceControl(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}