using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using CwcExim.Filters;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;


namespace CwcExim.Areas.Master.Controllers
{
    public class LoniGodownController : BaseController
    {
        [HttpGet]
        public ActionResult CreateGodown()
        {
            return PartialView("CreateGodown");
        }

        [HttpGet]
        public ActionResult EditGodown(int GodownId)
        {
            PPGGodownVM ObjGodown = new PPGGodownVM();
            if (GodownId > 0)
            {
                LONIMasterRepository ObjGR = new LONIMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (PPGGodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            PPGGodownVM ObjGodown = new PPGGodownVM();
            if (GodownId > 0)
            {
                LONIMasterRepository ObjGR = new LONIMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (PPGGodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            LONIMasterRepository ObjGR = new LONIMasterRepository();
            ObjGR.GetAllGodown();
            List<PPGGodown> LstGodown = new List<PPGGodown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<PPGGodown>)ObjGR.DBResponse.Data;
            }
            return PartialView("GetGodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(PPGGodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGGodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<PPGGodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
                LONIMasterRepository ObjGR = new LONIMasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjGodown.MstGodwon.Uid = ObjLogin.Uid;
                string LocationXML = Utility.CreateXML(ObjGodown.LstLocation);
                ObjGodown.MstGodwon.GodownName = ObjGodown.MstGodwon.GodownName.Trim();
                // ObjGodown.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjGR.AddEditGodown(ObjGodown, LocationXML, DelLocationXML);
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
                LONIMasterRepository ObjGR = new LONIMasterRepository();
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
