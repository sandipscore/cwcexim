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
    public class WFLDCommodityController : Controller
    {
        [HttpGet]
        public ActionResult CreateCommodity() 
        {
            return PartialView("CreateCommodity");
        }

        [HttpGet]
        public ActionResult GetCommodityList()
        {
            WFLDCommodityRepository ObjCR = new WFLDCommodityRepository();
            ObjCR.GetAllCommodity(0);

            List<WFLDCommodity> LstCommodity = new List<WFLDCommodity>();
            if(ObjCR.DBResponse.Data!=null)
            {
                LstCommodity = (List<WFLDCommodity>)ObjCR.DBResponse.Data;
            }
            return PartialView("CommodityList", LstCommodity);
        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            WFLDCommodityRepository ObjCR = new WFLDCommodityRepository();
            List<WFLDCommodity> LstCommodity = new List<WFLDCommodity>();
            ObjCR.GetAllCommodity(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<WFLDCommodity>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCommodity(int CommodityId)
        {
            /*
            Commodity Type:1.HAZ 2.Non HAZ
            */
            WFLDCommodityRepository ObjCR = new WFLDCommodityRepository();
            WFLDCommodity ObjCommodity = new WFLDCommodity();
            if (CommodityId > 0)
            {
                ObjCR.GetCommodity(CommodityId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCommodity = (WFLDCommodity)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewCommodity", ObjCommodity);
        }

        [HttpGet]
        public ActionResult EditCommodity(int CommodityId)
        {
            WFLDCommodityRepository ObjCr = new WFLDCommodityRepository();
            WFLDCommodity ObjCommodity = new WFLDCommodity();
            if(CommodityId>0)
            {
                ObjCr.GetCommodity(CommodityId);
                if(ObjCr.DBResponse.Data!=null)
                {
                    ObjCommodity=(WFLDCommodity)ObjCr.DBResponse.Data;
                }
            }
            return PartialView("EditCommodity", ObjCommodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCommodityDetail(WFLDCommodity ObjCommodity)
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
                WFLDCommodityRepository ObjCR = new WFLDCommodityRepository();
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
                WFLDCommodityRepository ObjCR = new WFLDCommodityRepository();
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