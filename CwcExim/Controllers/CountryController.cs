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
    public class CountryController : BaseController
    {
        [HttpGet]
        public ActionResult CreateCountry()
        {
            return View("CreateCountry");
        }

        [HttpGet]
        public ActionResult GetCountryList()
        {
            CountryRepository ObjCR = new CountryRepository();
            List<Country> LstCountry = new List<Country>();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCountry = (List<Country>)ObjCR.DBResponse.Data;
            }
            return View("CountryList", LstCountry);
        }

        [HttpGet]
        public ActionResult EditCountry(int CountryId)
        {
            CountryRepository ObjCR = new CountryRepository();
            Country ObjCountry = new Country();
            if (CountryId>0)
            {
                ObjCR.GetCountry(CountryId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCountry=(Country)ObjCR.DBResponse.Data;
                }
            }
            return View("EditCountry", ObjCountry);
        }

        [HttpGet]
        public ActionResult ViewCountry(int CountryId)
        {
            CountryRepository ObjCR = new CountryRepository();
            Country ObjCountry = new Country();
            if(CountryId>0)
            {
                ObjCR.GetCountry(CountryId);
                if (ObjCR.DBResponse.Data!=null)
                {
                    ObjCountry = (Country)ObjCR.DBResponse.Data;
                }
            }
            return View("ViewCountry", ObjCountry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCountryDetail(Country ObjCountry)
        {
            if (ModelState.IsValid)
            {
                CountryRepository ObjCR = new CountryRepository();
                ObjCountry.CountryName = ObjCountry.CountryName.Trim();
                ObjCountry.CountryAlias = ObjCountry.CountryAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCountry.Uid=ObjLogin.Uid;
                ObjCountry.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditCountry(ObjCountry);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCountryDetail(int CountryId)
        {
            if (CountryId > 0)
            {
                CountryRepository ObjCR = new CountryRepository();
                ObjCR.DeleteCountry(CountryId);
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0,Message="Error" };
                return Json(Err);
            }
        }
    }
}