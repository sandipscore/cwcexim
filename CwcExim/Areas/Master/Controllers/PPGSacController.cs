using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;

namespace CwcExim.Areas.Master.Controllers
{
    public class PPGSacController : BaseController
    {
        [HttpGet]
        public ActionResult CreateSAC()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult ViewSAC(int SACId)
        {
            PPGMasterRepository ObjSR = new PPGMasterRepository();
            PPGSac ObjSac = new PPGSac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (PPGSac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            PPGMasterRepository ObjSR = new PPGMasterRepository();
            List<PPGSac> LstSac = new List<PPGSac>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<PPGSac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(PPGSac ObjSac)
        {
            if (ModelState.IsValid)
            {
                PPGMasterRepository ObjSR = new PPGMasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSac.Uid = ObjLogin.Uid;
                if (ObjSac.Description != null)
                    ObjSac.Description = ObjSac.Description.Trim();
                ObjSR.AddSac(ObjSac);
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
    }
}