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
    public class ContractorController : BaseController
    {
        [HttpGet]
        public ActionResult CreateContractor()
        {
            CountryRepository ObjCR = new CountryRepository();
            ViewBag.Country = null;
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            return View("CreateContractor");
        }

        [HttpGet]
        public ActionResult GetContractorList()
        {
            ContractorRepository ObjCR = new ContractorRepository();
            List<Contractor> LstContractor = new List<Contractor>();
            ObjCR.GetAllContractor();
            if (ObjCR.DBResponse.Data != null)
            {
                LstContractor=(List<Contractor>)ObjCR.DBResponse.Data;
            }
            return View("ContractorList", LstContractor);
        }

        [HttpGet]
        public ActionResult EditContractor(int ContractorId)
        {
            ViewBag.Country = null;
            Contractor ObjContractor = new Contractor();
            if (ContractorId >0)
            {
                ContractorRepository ObjCR = new ContractorRepository();
                ObjCR.GetContractor(ContractorId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjContractor=(Contractor)ObjCR.DBResponse.Data;
                    CountryRepository ObjCYR = new CountryRepository();
                    ObjCYR.GetAllCountry();
                    if (ObjCYR.DBResponse.Data != null)
                    {
                        ViewBag.Country=ObjCYR.DBResponse.Data;
                    }
                }
            }
            return View("EditContractor", ObjContractor);
        }

        [HttpGet]
        public ActionResult ViewContractor(int ContractorId)
        {
            ViewBag.Country = null;
            Contractor ObjContractor = new Contractor();
            if (ContractorId > 0)
            {
                ContractorRepository ObjCR = new ContractorRepository();
                ObjCR.GetContractor(ContractorId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjContractor = (Contractor)ObjCR.DBResponse.Data;
                    CountryRepository ObjCYR = new CountryRepository();
                    ObjCYR.GetAllCountry();
                    if (ObjCYR.DBResponse.Data != null)
                    {
                        ViewBag.Country = ObjCYR.DBResponse.Data;
                    }
                }
            }
            return View("ViewContractor", ObjContractor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditContractorDetail(Contractor ObjContractor)
        {
            if (ModelState.IsValid)
            {
                ObjContractor.ContractorName=ObjContractor.ContractorName.Trim();
                if (ObjContractor.ContactPerson != null)
                {
                    ObjContractor.ContactPerson = ObjContractor.ContactPerson.Trim();
                }
                if (ObjContractor.Address != null)
                {
                    ObjContractor.Address = ObjContractor.Address.Trim();
                }
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjContractor.Uid=ObjLogin.Uid;
                ContractorRepository ObjCR = new ContractorRepository();
                ObjCR.AddEditContractor(ObjContractor);
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
        public ActionResult DeleteContractorDetail(int ContractorId)
        {
            if (ContractorId > 0)
            {
                ContractorRepository ObjCR = new ContractorRepository();
                ObjCR.DeleteContractor(ContractorId);
                return Json(ObjCR.DBResponse); ;
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
    }
}