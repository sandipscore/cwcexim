using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Filters;
using CwcExim.Areas.Master.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Master.Controllers
{
    public class DSRCommodityController : Controller
    {
        [HttpGet]
        public ActionResult CreateCommodity() 
        {
            return PartialView("CreateCommodity");
        }

        [HttpGet]
        public ActionResult GetCommodityList()
        {
            DSRCommodityRepository ObjCR = new DSRCommodityRepository();
            ObjCR.GetAllCommodity(0);

            List<DSRCommodity> LstCommodity = new List<DSRCommodity>();
            if(ObjCR.DBResponse.Data!=null)
            {
                LstCommodity = (List<DSRCommodity>)ObjCR.DBResponse.Data;
            }
            return PartialView("CommodityList", LstCommodity);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            DSRCommodityRepository ObjCR = new DSRCommodityRepository();
            List<DSRCommodity> LstCommodity = new List<DSRCommodity>();
            ObjCR.GetAllCommodity(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<DSRCommodity>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCommodity(int CommodityId)
        {
            /*
            Commodity Type:1.HAZ 2.Non HAZ
            */
            DSRCommodityRepository ObjCR = new DSRCommodityRepository();
            DSRCommodity ObjCommodity = new DSRCommodity();
            if (CommodityId > 0)
            {
                ObjCR.GetCommodity(CommodityId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCommodity = (DSRCommodity)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewCommodity", ObjCommodity);
        }

        [HttpGet]
        public ActionResult EditCommodity(int CommodityId)
        {
            DSRCommodityRepository ObjCr = new DSRCommodityRepository();
            DSRCommodity ObjCommodity = new DSRCommodity();
            if(CommodityId>0)
            {
                ObjCr.GetCommodity(CommodityId);
                if(ObjCr.DBResponse.Data!=null)
                {
                    ObjCommodity=(DSRCommodity)ObjCr.DBResponse.Data;
                }
            }
            return PartialView("EditCommodity", ObjCommodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCommodityDetail(DSRCommodity ObjCommodity)
        {
            /*
             Commodity Type:1.HAZ 2.Non HAZ
             */
            if (ModelState.IsValid)
            {
                ObjCommodity.CommodityName = ObjCommodity.CommodityName.Trim();
                //ObjCommodity.CommodityAlias = ObjCommodity.CommodityAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCommodity.Uid=ObjLogin.Uid;
                ObjCommodity.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                DSRCommodityRepository ObjCR = new DSRCommodityRepository();
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
                DSRCommodityRepository ObjCR = new DSRCommodityRepository();
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