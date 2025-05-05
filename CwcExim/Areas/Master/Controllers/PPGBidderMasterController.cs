using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
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
    public class PPGBidderMasterController : BaseController
    {
        // GET: Master/PPGBidderMaster
        public ActionResult BidderRegistration()
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditBidderDetail(PPGBidder vmBidder)
        {
            if (ModelState.IsValid)
            {
                vmBidder.BidderName = vmBidder.BidderName.Trim();
                vmBidder.Address = vmBidder.Address == null ? null : vmBidder.Address.Trim();
                vmBidder.ContactPerson = vmBidder.ContactPerson == null ? null : vmBidder.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                vmBidder.Uid = ObjLogin.Uid;
                PPGBidderRepository ObjETR = new PPGBidderRepository();
                ObjETR.AddEditEximTrader(vmBidder);
                ModelState.Clear();
                return Json(ObjETR.DBResponse);
            }
            else
            {
                vmBidder.Password = vmBidder.HdnPassword;
                vmBidder.ConfirmPassword = vmBidder.ConfirmPassword;
                var ErroMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErroMessage };
                return Json(Err);
            }
        }


        public ActionResult GetBidderList()
        {
            PPGBidderRepository ObjETR = new PPGBidderRepository();
            List<PPGBidder> LstBidder = new List<PPGBidder>();
            ObjETR.GetAllBidderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstBidder = (List<PPGBidder>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetBidderList", LstBidder);
        }


        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            PPGBidderRepository ObjCR = new PPGBidderRepository();
            List<PPGBidder> LstCommodity = new List<PPGBidder>();
            ObjCR.GetAllBidderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<PPGBidder>)ObjCR.DBResponse.Data;
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
        public ActionResult EditBidder(int BidderId)
        {
            PPGBidder ObjBidder = new PPGBidder();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (BidderId > 0)
            {
                PPGBidderRepository ObjETR = new PPGBidderRepository();
                ObjETR.GetBidder(BidderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjBidder = (PPGBidder)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditBidder", ObjBidder);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            PPGBidder ObjEximTrader = new PPGBidder();
            if (EximTraderId > 0)
            {
                PPGMasterRepository ObjETR = new PPGMasterRepository();
                ObjETR.GetBidderTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (PPGBidder)ObjETR.DBResponse.Data;
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
        public ActionResult DeleteBidderDetail(int EximTraderId)
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





        /// for SD Opening Bidder 
        //public ActionResult CreateSDOpen()
        //{
        //    //AccessRightsRepository ACCR = new AccessRightsRepository();
        //    //ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
        //    //ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

        //    PPGMasterRepository ObjRepo = new PPGMasterRepository();
        //    /*SDOpening ObjSD = new SDOpening();*/
        //    SearchBidderData obj = new SearchBidderData();
        //    ObjRepo.GetEximTrader("", 0);
        //    if (ObjRepo.DBResponse.Data != null)
        //    {
        //        ViewBag.lstExim = ((SearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
        //        ViewBag.State = ((SearchEximTraderData)ObjRepo.DBResponse.Data).State;
        //    }



        //    var model = new SDOpening();
        //    for (var i = 0; i < 5; i++)
        //    {
        //        model.Details.Add(new PPGReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
        //    }
        //    var PaymentMode = new SelectList(new[]
        //   {
        //        new SelectListItem { Text = "--- Select ---", Value = ""},
        //        new SelectListItem { Text = "CASH", Value = "CASH"},
        //        new SelectListItem { Text = "NEFT", Value = "NEFT"},
        //        new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
        //        new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
        //        new SelectListItem { Text = "PO", Value = "PO"},
        //         new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
        //         new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
        //    }, "Value", "Text");
        //    ViewBag.Type = PaymentMode;
        //    return PartialView("CreateSDopening", model);
           
        //} 




    }
}