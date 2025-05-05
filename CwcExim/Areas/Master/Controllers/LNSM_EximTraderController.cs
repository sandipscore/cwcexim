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
    public class LNSM_EximTraderController : Controller
    {
        // GET: Master/LNSM_EximTrader
        public ActionResult Index()
        {
            return View();
        }

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
            LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
            List<LNSM_EximTrader> LstEximTrader = new List<LNSM_EximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<LNSM_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            LNSM_MasterRepository ObjCR = new LNSM_MasterRepository();
            List<LNSM_EximTrader> LstCommodity = new List<LNSM_EximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<LNSM_EximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
            List<LNSM_EximTrader> LstEximTrader = new List<LNSM_EximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<LNSM_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
      
        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            LNSM_EximTrader ObjEximTrader = new LNSM_EximTrader();
            if (EximTraderId > 0)
            {
                LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (LNSM_EximTrader)ObjETR.DBResponse.Data;
                }
                //ObjETR.GetEximTraderAddress(EximTraderId);
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    ObjEximTrader = (LNSM_EximTrader)ObjETR.DBResponse.Data;
                //}
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }


        #endregion

        #region Exim Trader Master

        [HttpGet]
        public ActionResult CreateEximTraderOther()
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

        public ActionResult GetEximTraderListOther()
        {
            LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
            List<LNSM_EximTrader> LstEximTrader = new List<LNSM_EximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<LNSM_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetEximTraderListOther", LstEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTraderOther(int EximTraderId)
        {
            LNSM_EximTrader ObjEximTrader = new LNSM_EximTrader();
            if (EximTraderId > 0)
            {
                LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (LNSM_EximTrader)ObjETR.DBResponse.Data;
                }
                //ObjETR.GetEximTraderAddress(EximTraderId);
                //if (ObjETR.DBResponse.Data != null)
                //{
                //    ObjEximTrader = (LNSM_EximTrader)ObjETR.DBResponse.Data;
                //}
            }
            return PartialView("ViewEximTraderOther", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult GetEximTraderListPartyCodeOther(string PartyCode)
        {
            LNSM_MasterRepository ObjETR = new LNSM_MasterRepository();
            List<LNSM_EximTrader> LstEximTrader = new List<LNSM_EximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<LNSM_EximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetEximTraderListOther", LstEximTrader);
        }
        #endregion
    }
}