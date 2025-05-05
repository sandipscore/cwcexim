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
    public class PortController : BaseController
    {
        [HttpGet]
        public ActionResult CreatePort()
        {
            CountryRepository ObjCR = new CountryRepository();
            ViewBag.Country = null;
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country=ObjCR.DBResponse.Data;
            }
            return View("CreatePort");
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            PortRepository ObjPR = new PortRepository();
            List<Port> LstPort = new List<Port>();
            ObjPR.GetAllPort();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<Port>)ObjPR.DBResponse.Data;
            }
            return View("PortList", LstPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(Port ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias=ObjPort.PortAlias.Trim();
                ObjPort.PortName=ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid=ObjLogin.Uid;
                PortRepository ObjPR = new PortRepository();
                ObjPR.AddEditPort(ObjPort);
                ModelState.Clear();
                return Json(ObjPR.DBResponse);
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
        public ActionResult DeletePortDetail(int PortId)
        {
            if (PortId > 0)
            {
                PortRepository ObjPR = new PortRepository();
                ObjPR.DeletePort(PortId);
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditPort(int PortId)
        {
            Port ObjPort = new Port();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                PortRepository ObjPR = new PortRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort=(Port)ObjPR.DBResponse.Data;
                }
                CountryRepository ObjCR = new CountryRepository();
                ObjCR.GetAllCountry();
                if (ObjCR.DBResponse.Data != null)
                {
                    ViewBag.Country = ObjCR.DBResponse.Data;
                }
            }
            return View("EditPort", ObjPort);
        }

        [HttpGet]
        public ActionResult ViewPort(int PortId)
        {
            Port ObjPort = new Port();
            if (PortId > 0)
            {
                PortRepository ObjPR = new PortRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (Port)ObjPR.DBResponse.Data;
                }
            }
            return View("ViewPort", ObjPort);
        }


    }
}