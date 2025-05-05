using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Areas.GateOperation.Models;
using CwcExim.Models;

namespace CwcExim.Areas.GateOperation.Controllers
{
    public class WeighmentController : BaseController
    {
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            Weighment ObjWeighment = new Weighment();
            WeighmentRepository ObjWR = new WeighmentRepository();
            VehicleRepository ObjVR = new VehicleRepository();
            ObjWeighment.WeightmentDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjWR.GetContainerNoForWeighment();
            if (ObjWR.DBResponse.Data != null)
            {
               ViewBag.ContainerList=new SelectList((List<Weighment>)ObjWR.DBResponse.Data, "CFSCode", "ContainerNo") ;
            }
            else
            {
                ViewBag.ContainerList = null;
            }
            ObjVR.GetAllVehicleMaster();
            if (ObjVR.DBResponse.Data != null)
            {
                ViewBag.VehicleList = new SelectList((List<Vehicle>)ObjVR.DBResponse.Data, "VehicleMasterId", "VehicleNumber");
            }
            else
            {
                ViewBag.VehicleList = null;
            }
            return PartialView("CreateWeighment", ObjWeighment);
        }

        [HttpGet]
        public ActionResult GetWeighmentList()
        {
            WeighmentRepository ObjWR = new WeighmentRepository();
            ObjWR.GetWeighment();
            List<Weighment> LstWeighment = new List<Weighment>();
            if (ObjWR.DBResponse.Data != null)
            {
                LstWeighment = (List<Weighment>)ObjWR.DBResponse.Data;
            }
            return View("WeighmentList", LstWeighment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWeighmentDet(Weighment ObjWeighment)
        {
            if (ModelState.IsValid)
            {
                WeighmentRepository ObjWR = new WeighmentRepository();
                ObjWR.AddWeighment(ObjWeighment);
                ModelState.Clear();
                return Json(ObjWR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                return Json(new { Status = 0, Message = ErrorMessage });
            }
        }
    }
}