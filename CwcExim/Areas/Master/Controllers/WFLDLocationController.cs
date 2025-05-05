using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Areas.Master.Models;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Filters;
namespace CwcExim.Areas.Master.Controllers
{
    public class WFLDLocationController : Controller 
    {
        // GET: Master/WFLDLocation
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult CreateLocation()
        {
            return PartialView("CreateLocation");
        }

        [HttpGet]
        public ActionResult GetLocationList()
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            List<WFLDLocation> LstLocation = new List<WFLDLocation>();
            ObjCR.GetAllLocation();
            if (ObjCR.DBResponse.Data != null)
            {
                LstLocation = (List<WFLDLocation>)ObjCR.DBResponse.Data;
            }
            return PartialView("LocationList", LstLocation);
        }

        [HttpGet]
        public ActionResult EditLocation(int LocationId)
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            WFLDLocation ObjLocation = new WFLDLocation();
            if (LocationId > 0)
            {
                ObjCR.GetLocation(LocationId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjLocation = (WFLDLocation)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("EditLocation", ObjLocation);
        }

        [HttpGet]
        public ActionResult ViewLocation(int LocationId)
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            WFLDLocation ObjLocation = new WFLDLocation();
            if (LocationId > 0)
            {
                ObjCR.GetLocation(LocationId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjLocation = (WFLDLocation)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewLocation", ObjLocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditLocationDetail(WFLDLocation ObjLocation)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjCR = new WHT_MasterRepository();
                ObjLocation.LocationName = ObjLocation.LocationName.Trim();
                ObjLocation.LocationAlias = ObjLocation.LocationAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjLocation.Uid = ObjLogin.Uid;
                ObjLocation.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditLocation(ObjLocation);
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
        public ActionResult DeleteLocationDetail(int LocationId)
        {
            if (LocationId > 0)
            {
                WHT_MasterRepository ObjCR = new WHT_MasterRepository();
                ObjCR.DeleteLocation(LocationId);
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