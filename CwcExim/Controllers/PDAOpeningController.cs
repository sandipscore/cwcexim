using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.UtilityClasses;

namespace CwcExim.Controllers
{
    public class PDAOpeningController : BaseController
    {
        [HttpGet]
        public ActionResult CreatePDAOpening()
        {
            PDAOpeningRepository ObjPDAR = new PDAOpeningRepository();
            PDAOpening ObjPDA = new PDAOpening();
            ObjPDAR.GetEximTrader();
            if (ObjPDAR.DBResponse.Data != null)
            {
                ObjPDA.LstEximTrader = (List<PDAOpening>)ObjPDAR.DBResponse.Data;
            }
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.CurDate = DateTime.Today.ToString("dd/MM/yyyy");
            return View("CreatePDAOpening", ObjPDA);
        }

        [HttpGet]
        public ActionResult GetPDAList()
        {
            PDAOpeningRepository ObjPDAR = new PDAOpeningRepository();
            List<PDAOpening> LstPDA = new List<PDAOpening>();
            ObjPDAR.GetAllPDAOpening();
            if (ObjPDAR.DBResponse.Data != null)
            {
                LstPDA=(List<PDAOpening>)ObjPDAR.DBResponse.Data;
            }
            return View("PDAList", LstPDA);
        }

        [HttpGet]
        public ActionResult ViewPDAOpening(int PDAId)
        {
            PDAOpening ObjPDA = new PDAOpening();
            if (PDAId > 0)
            {
                PDAOpeningRepository ObjPDAR = new PDAOpeningRepository();
                ObjPDAR.GetPDAOpening(PDAId);
                if (ObjPDAR.DBResponse.Data != null)
                {
                    ObjPDA = (PDAOpening)ObjPDAR.DBResponse.Data;
                }
            }
            return View("ViewPDAOpening", ObjPDA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPDAOpeningDetail(PDAOpening ObjPDA)
        {
            if (ModelState.IsValid)
            {
                PDAOpeningRepository ObjPDAR = new PDAOpeningRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPDA.Uid = ObjLogin.Uid;
                ObjPDAR.AddPDAOpening(ObjPDA);
                ModelState.Clear();
                return Json(ObjPDAR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status=1,Message= ErrorMessage };
                return Json(Err);
            }
        }
    }
}