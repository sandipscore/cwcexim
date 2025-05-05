using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;

namespace CwcExim.Controllers
{
    public class CompanyController : BaseController
    {
        [HttpGet]
        public ActionResult CreateCompany()
        {
            StateRepository ObjSR = new StateRepository();
            ViewBag.Sate = null;
            List<State> LstState = new List<State>();
            ObjSR.GetAllState();
            if (ObjSR.DBResponse.Data != null)
            {
                ViewBag.StateId = new SelectList((List<State>)ObjSR.DBResponse.Data, "StateId", "StateName");
            }
            else
            {
                ViewBag.StateId = null;
            }
            return View("GetCompany");
        }

        [HttpGet]
        public ActionResult GetCompanyList()
        {
            CompanyRepository ObjCR = new CompanyRepository();
            List<Company> LstCompany = new List<Company>();
            ObjCR.GetAllCompany();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCompany=(List<Company>)ObjCR.DBResponse.Data;
            }
            return View("CompanyList", LstCompany);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCompanyDetails(Company ObjCompany)
        {
            if (ModelState.IsValid)
            {
                CompanyRepository ObjCR = new CompanyRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCompany.Uid=ObjLogin.Uid;
                ObjCompany.CompanyName=ObjCompany.CompanyName.Trim();
                ObjCompany.CompanyAddress = ObjCompany.CompanyAddress.Trim();
                ObjCompany.CompanyShortName = ObjCompany.CompanyShortName == null ? null : ObjCompany.CompanyShortName.Trim();
                ObjCR.AddCompany(ObjCompany);
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
    }
}