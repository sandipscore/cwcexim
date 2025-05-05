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
    public class CityController : Controller
    {
        [HttpGet]
        public ActionResult CreateCity()
        {
            StateRepository ObjSR = new StateRepository();
            City ObjCity = new City();
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ObjCity.LstCountry=(List<Country>)ObjCR.DBResponse.Data;
            }
            return View("CreateCity", ObjCity);
        }

        [HttpGet]
        public ActionResult GetStatesByCountry(int CountryId)
        {
            if (CountryId > 0)
            {
                StateRepository ObjSR = new StateRepository();
                ObjSR.GetStateByCountryId(CountryId);
                return Json(ObjSR.DBResponse,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetCitiesByState(int StateId)
        {
            if (StateId > 0)
            {
                CityRepository ObjCR = new CityRepository();
                ObjCR.GetCityByStateId(StateId);
                return Json(ObjCR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetCityList()
        {
            CityRepository ObjCR = new CityRepository();
            List<City> LstCity = new List<City>();
            ObjCR.GetAllCity();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCity = (List<City>)ObjCR.DBResponse.Data;
            }
            return View("CityList", LstCity);
        }

        [HttpGet]
        public ActionResult ViewCity(int CityId)
        {
            City ObjCity = new City();
            if (CityId > 0)
            {
                CityRepository ObjCR = new CityRepository();
                ObjCR.GetCity(CityId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCity = (City)ObjCR.DBResponse.Data;
                }
            }
            return View("ViewCity", ObjCity);
        }

        [HttpGet]
        public ActionResult EditCity(int CityId)
        {
            City ObjCity = new City();
            CityRepository ObjCIR = new CityRepository();
            CountryRepository ObjCR = new CountryRepository();
            if (CityId > 0)
            {

                ObjCIR.GetCity(CityId);
                if (ObjCIR.DBResponse.Data != null)
                {
                    ObjCity=(City)ObjCIR.DBResponse.Data;
                    ObjCR.GetAllCountry();
                    if (ObjCR.DBResponse.Data != null)
                    {
                        ObjCity.LstCountry = (List<Country>)ObjCR.DBResponse.Data;
                    }
                }
            }
            return View("EditCity", ObjCity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCityDetail(City ObjCity)
        {
            if (ModelState.IsValid)
            {
                CityRepository ObjCR = new CityRepository();
                ObjCity.CityName = ObjCity.CityName.Trim();
                ObjCity.CityAlias = ObjCity.CityAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCity.Uid = ObjLogin.Uid;
                ObjCR.AddEditCity(ObjCity);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCityDetail(int CityId)
        {
            if (CityId > 0)
            {
                CityRepository ObjCR = new CityRepository();
                ObjCR.DeleteCity(CityId);
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
    }
}