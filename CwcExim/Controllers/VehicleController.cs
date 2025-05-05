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
    public class VehicleController : BaseController
    {
        [HttpGet]
        public ActionResult EnterVehicleDtls()
        {
            return View("EnterVehicleDtls");
        }

        [HttpGet]
        public ActionResult GetVehicleList()
        {
            VehicleRepository ObjCR = new VehicleRepository();
            List<Vehicle> LstVehicle = new List<Vehicle>();
            ObjCR.GetAllVehicleMaster();
            if (ObjCR.DBResponse.Data != null)
            {
                LstVehicle = (List<Vehicle>)ObjCR.DBResponse.Data;
            }
            return View("VehicleList", LstVehicle);
        }

        [HttpGet]
        public ActionResult EditSingleVehicle(int VehicleId)
        {
            VehicleRepository ObjCR = new VehicleRepository();
            Vehicle ObjVehicle = new Vehicle();
            if (VehicleId > 0)
            {
                ObjCR.GetVehicle(VehicleId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjVehicle = (Vehicle)ObjCR.DBResponse.Data;
                }
            }
            return View("EditVehicle", ObjVehicle);
        }

        [HttpGet]
        public ActionResult ViewSingleVehicle(int VehicleId)
        {
            VehicleRepository ObjCR = new VehicleRepository();
            Vehicle ObjVehicle = new Vehicle();
            if(VehicleId > 0)
            {
                ObjCR.GetVehicle(VehicleId);
                if (ObjCR.DBResponse.Data!=null)
                {
                    ObjVehicle = (Vehicle)ObjCR.DBResponse.Data;
                }
            }
            return View("ViewVehicle", ObjVehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditVehicleMaster(Vehicle ObjVehicle)
        {
            if (ModelState.IsValid)
            {
                VehicleRepository ObjCR = new VehicleRepository();
                ObjVehicle.VehicleNumber = ObjVehicle.VehicleNumber.Trim();
                ObjVehicle.VehicleWeight = ObjVehicle.VehicleWeight.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjVehicle.Uid=ObjLogin.Uid;
                ObjCR.AddEditVehicle(ObjVehicle);
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
        public ActionResult DeleteVehicleDetail(int VehicleId)
        {
            if (VehicleId > 0)
            {
                VehicleRepository ObjCR = new VehicleRepository();
                ObjCR.DeleteVehicle(VehicleId);
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