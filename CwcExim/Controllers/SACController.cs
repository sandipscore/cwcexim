using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class SACController : BaseController
    {
        // GET: SAC
        [HttpGet]
        public ActionResult CreateSAC()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ViewSAC(int SACId)
        {
            SacRepository ObjSR = new SacRepository();
            SAC ObjSac = new SAC();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac =(SAC)ObjSR.DBResponse.Data;
            }
            return View(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            SacRepository ObjSR = new SacRepository();
            List<SAC> LstSac = new List<SAC>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac=(List<SAC>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(SAC ObjSac)
        {
            if (ModelState.IsValid)
            {
                SacRepository ObjSR = new SacRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSac.Uid=ObjLogin.Uid;
                if(ObjSac.Description!=null)
                    ObjSac.Description=ObjSac.Description.Trim();
                ObjSR.AddSac(ObjSac);
                ModelState.Clear();
                return Json(ObjSR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e=>e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
    }
}