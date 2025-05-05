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
    public class StateController : Controller
    {
        [HttpGet]
        public ActionResult CreateState()
        {
            CountryRepository ObjCR = new CountryRepository();
            State ObjState = new State();
            ObjCR.GetAllCountry();
            if(ObjCR.DBResponse.Data!=null)
            {
                ObjState.LstCountry = (List<Country>)ObjCR.DBResponse.Data;
            }
            return View("CreateState", ObjState);
        }

        [HttpGet]
        public ActionResult GetStateList()
        {
            StateRepository ObjSR = new StateRepository();
            List<State> LstState = new List<State>();
            ObjSR.GetAllState();
            if (ObjSR.DBResponse.Data != null)
            {
                LstState = (List<State>)ObjSR.DBResponse.Data;
            }
            return View("StateList", LstState);
        }

        [HttpGet]
        public ActionResult EditState(int StateId)
        {
            State ObjState = new State();
            if (StateId > 0)
            {
                StateRepository ObjSR = new StateRepository();
                CountryRepository ObjCR = new CountryRepository();
                ObjSR.GetState(StateId);
                if (ObjSR.DBResponse.Data != null)
                {
                    ObjState = (State)ObjSR.DBResponse.Data;
                    ObjCR.GetAllCountry();
                    if (ObjCR.DBResponse.Data != null)
                    {
                        ObjState.LstCountry = (List<Country>)ObjCR.DBResponse.Data;
                    }
                }
            }
            return View("EditState", ObjState);
        }

        [HttpGet]
        public ActionResult ViewState(int StateId)
        {
            State ObjState = new State();
            if (StateId > 0)
            {
                StateRepository ObjSR = new StateRepository();
                ObjSR.GetState(StateId);
                if (ObjSR.DBResponse.Data != null)
                {
                    ObjState = (State)ObjSR.DBResponse.Data;
                }
            }
            return View("ViewState", ObjState);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditStateDetail(State ObjState)
        {
            if (ModelState.IsValid)
            {
                ObjState.StateAlias= ObjState.StateAlias.Trim();
                ObjState.StateName = ObjState.StateName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjState.Uid=ObjLogin.Uid;
                ObjState.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                StateRepository ObjSR = new StateRepository();
                ObjSR.AddEditState(ObjState);
                ModelState.Clear();
                return Json(ObjSR.DBResponse);
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
        public ActionResult DeleteStateDetail(int StateId)
        {
            if (StateId > 0)
            {
                StateRepository ObjSR = new StateRepository();
                ObjSR.DeleteState(StateId);
                return Json(ObjSR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
    }
}