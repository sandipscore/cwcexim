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
    public class Hdb_CommodityController : Controller
    {
        [HttpGet]
        public ActionResult CreateCommodity()
        {
            return View("CreateCommodity");
        }

        [HttpGet]
        public ActionResult GetCommodityList()
        {
            Hdb_CommodityRepository ObjCR = new Hdb_CommodityRepository();
            ObjCR.GetAllCommodity(0);

            List<Hdb_Commodity> LstCommodity = new List<Hdb_Commodity>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<Hdb_Commodity>)ObjCR.DBResponse.Data;
            }
            return View("CommodityList", LstCommodity);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            Hdb_CommodityRepository ObjCR = new Hdb_CommodityRepository();
            List<Hdb_Commodity> LstCommodity = new List<Hdb_Commodity>();
            ObjCR.GetAllCommodity(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<Hdb_Commodity>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCommodity(int CommodityId)
        {
            /*
            Commodity Type:1.HAZ 2.Non HAZ
            */
            Hdb_CommodityRepository ObjCR = new Hdb_CommodityRepository();
            Hdb_Commodity ObjCommodity = new Hdb_Commodity();
            if (CommodityId > 0)
            {
                ObjCR.GetCommodity(CommodityId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCommodity = (Hdb_Commodity)ObjCR.DBResponse.Data;
                }
            }
            return View("ViewCommodity", ObjCommodity);
        }

        [HttpGet]
        public ActionResult EditCommodity(int CommodityId)
        {
            Hdb_CommodityRepository ObjCr = new Hdb_CommodityRepository();
            Hdb_Commodity ObjCommodity = new Hdb_Commodity();
            if (CommodityId > 0)
            {
                ObjCr.GetCommodity(CommodityId);
                if (ObjCr.DBResponse.Data != null)
                {
                    ObjCommodity = (Hdb_Commodity)ObjCr.DBResponse.Data;
                }
            }
            return View("EditCommodity", ObjCommodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCommodityDetail(Hdb_Commodity ObjCommodity)
        {
            /*
             Commodity Type:1.HAZ 2.Non HAZ
             */
            ModelState.Remove("CommodityAlias");
            if (ModelState.IsValid)
            {
                ObjCommodity.CommodityName = ObjCommodity.CommodityName.Trim();
                //ObjCommodity.CommodityAlias = ObjCommodity.CommodityAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCommodity.Uid = ObjLogin.Uid;
                ObjCommodity.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                Hdb_CommodityRepository ObjCR = new Hdb_CommodityRepository();
                ObjCR.AddEditCommodity(ObjCommodity);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteCommodityDetail(int CommodityId)
        {
            if (CommodityId > 0)
            {
                Hdb_CommodityRepository ObjCR = new Hdb_CommodityRepository();
                ObjCR.DeleteCommodity(CommodityId);
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