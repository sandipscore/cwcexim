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

namespace CwcExim.Areas.Master.Controllers
{
    public class LoniRentReservationPartyController : BaseController
    {
        [HttpGet]
        public ActionResult CreateRentReservationParty()
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

        [HttpGet]
        public ActionResult GetRentReservationPartyList()
        {
            LONIMasterRepository ObjETR = new LONIMasterRepository();
            List<PPGRentReservationModel> LstRentReservation = new List<PPGRentReservationModel>();
            ObjETR.GetAllRentReservation();
            if (ObjETR.DBResponse.Data != null)
            {
                LstRentReservation = (List<PPGRentReservationModel>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetRentReservationPartyList", LstRentReservation);
        }
        [HttpGet]
        public ActionResult EditRentReservationParty(int RentReservationId)
        {
            PPGRentReservationModel ObjRentReservation = new PPGRentReservationModel();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (RentReservationId > 0)
            {
                LONIMasterRepository ObjETR = new LONIMasterRepository();
                ObjETR.GetRentReservation(RentReservationId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjRentReservation = (PPGRentReservationModel)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditRentReservationParty", ObjRentReservation);
        }

        [HttpGet]
        public ActionResult ViewRentReservationParty(int RentReservationId)
        {
            PPGRentReservationModel ObjRentReservation = new PPGRentReservationModel();
            if (RentReservationId > 0)
            {
                LONIMasterRepository ObjETR = new LONIMasterRepository();
                ObjETR.GetRentReservation(RentReservationId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjRentReservation = (PPGRentReservationModel)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewRentReservationParty", ObjRentReservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditRentReservationDetail(PPGRentReservationModel ObjRentReservation)
        {
            if (ModelState.IsValid)
            {
                ObjRentReservation.PartyName = ObjRentReservation.PartyName.Trim();
                ObjRentReservation.Address = ObjRentReservation.Address == null ? null : ObjRentReservation.Address.Trim();
                ObjRentReservation.ContactPerson = ObjRentReservation.ContactPerson == null ? null : ObjRentReservation.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjRentReservation.Uid = ObjLogin.Uid;
                LONIMasterRepository ObjETR = new LONIMasterRepository();
                ObjETR.AddEditRentReservation(ObjRentReservation);
                ModelState.Clear();
                return Json(ObjETR.DBResponse);
            }
            else
            {
                ObjRentReservation.Password = ObjRentReservation.HdnPassword;
                ObjRentReservation.ConfirmPassword = ObjRentReservation.ConfirmPassword;
                var ErroMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErroMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteRentReservationDetail(int RentReservationId)
        {
            if (RentReservationId > 0)
            {
                LONIMasterRepository ObjETR = new LONIMasterRepository();
                ObjETR.DeleteRentReservation(RentReservationId);
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