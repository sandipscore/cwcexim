using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Areas.Master.Models;
namespace CwcExim.Areas.Master.Controllers
{
    public class LONIRailFreightController : Controller
    {
        // GET: Master/PPGRailFreight
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult CreateRailFreight()
        {
            //  CWCChargesRepository objCR = new CWCChargesRepository();
            //  objCR.ListOfSACCode();
            //  if (objCR.DBResponse.Data != null)
            //      ViewBag.ListOfSAC = objCR.DBResponse.Data;
            //   else
            //       ViewBag.ListOfSAC = null;

            LONIMasterRepository ObjCR = new LONIMasterRepository();
            PPGRailFreight ObjPort = new PPGRailFreight();

            ObjCR.GetAllPort();

            if (ObjCR.DBResponse.Data != null)
            {
                ObjPort.LstPort = (List<PPGPost>)ObjCR.DBResponse.Data;
            }
            ObjCR.GetAllLocation();
            if (ObjCR.DBResponse.Data != null)
            {
                ObjPort.LstLocation = (List<PPGLocation>)ObjCR.DBResponse.Data;
            }
            return PartialView("CreateRailFreight", ObjPort);
        }
        [HttpGet]
        public ActionResult GetAllRailFreight()
        {
            List<PPGRailFreight> lstRailFreight = new List<PPGRailFreight>();
            LONIMasterRepository objCR = new LONIMasterRepository();
            objCR.GetAllRailFreight(0);
            if (objCR.DBResponse.Data != null)
                lstRailFreight = (List<PPGRailFreight>)objCR.DBResponse.Data;
            return PartialView("AllRailFreight", lstRailFreight);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditRailFreightFees(PPGRailFreight objEF)
        {
            if (ModelState.IsValid)
            {
                LONIMasterRepository objRepository = new LONIMasterRepository();
                objRepository.AddEditMstRailFreight(objEF, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditRailFreight(int RailFreightId)
        {
            PPGRailFreight objEF = new PPGRailFreight();
            LONIMasterRepository objRepo = new LONIMasterRepository();
            //objRepo.ListOfSACCode();
            // if (objRepo.DBResponse.Data != null)
            //     ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            // else
            //     ViewBag.ListOfSAC = null;


            LONIMasterRepository ObjCR = new LONIMasterRepository();
            PPGRailFreight ObjPort = new PPGRailFreight();
            // ObjCR.GetAllPort();
            // if (ObjCR.DBResponse.Data != null)
            //  {
            //      ObjPort.LstPort = (List<PPGPost>)ObjCR.DBResponse.Data;
            //  }
            //  ObjCR.GetAllLocation();
            //  if (ObjCR.DBResponse.Data != null)
            //   {
            //       ObjPort.LstLocation = (List<PPGLocation>)ObjCR.DBResponse.Data;
            //   }

            if (RailFreightId > 0)
            {
                objRepo.GetAllRailFreight(RailFreightId);

                if (objRepo.DBResponse.Data != null)
                    objEF = (PPGRailFreight)objRepo.DBResponse.Data;
                ObjCR.GetAllPort();
                if (ObjCR.DBResponse.Data != null)
                {
                    objEF.LstPort = (List<PPGPost>)ObjCR.DBResponse.Data;

                }

                ObjCR.GetAllLocation();
                if (ObjCR.DBResponse.Data != null)
                    objEF.LstLocation = (List<PPGLocation>)ObjCR.DBResponse.Data;
            }
            return PartialView("EditRailFreight", objEF);
        }


    }
}