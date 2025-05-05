using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using CwcExim.Filters;

namespace CwcExim.Controllers
{
    public class GodownController : BaseController
    {
        [HttpGet]
        public ActionResult CreateGodown()
        {
            return View("CreateGodown");
        }

        [HttpGet]
        public ActionResult EditGodown(int GodownId)
        {
            GodownVM ObjGodown = new GodownVM();
            if (GodownId > 0)
            {
                GodownRepository ObjGR = new GodownRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown=(GodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail=Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            GodownVM ObjGodown = new GodownVM();
            if(GodownId > 0)
            {
                GodownRepository ObjGR = new GodownRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown=(GodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            GodownRepository ObjGR = new GodownRepository();
            ObjGR.GetAllGodown();
            List<Godown> LstGodown = new List<Godown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown=(List<Godown>)ObjGR.DBResponse.Data;
            }
            return View("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(GodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<GodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if(ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<GodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML=Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
                GodownRepository ObjGR = new GodownRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjGodown.MstGodwon.Uid=ObjLogin.Uid;
                string LocationXML=Utility.CreateXML(ObjGodown.LstLocation);
                ObjGodown.MstGodwon.GodownName=ObjGodown.MstGodwon.GodownName.Trim();
               // ObjGodown.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjGR.AddEditGodown(ObjGodown,LocationXML, DelLocationXML);
                ModelState.Clear();
                return Json(ObjGR.DBResponse); 
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteGodownDetail(int GodownId)
        {
            if (GodownId > 0)
            {
                GodownRepository ObjGR = new GodownRepository();
                ObjGR.DeleteGodown(GodownId);
                return Json(ObjGR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
    }
}