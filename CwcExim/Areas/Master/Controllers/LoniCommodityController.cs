using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
using CwcExim.Filters;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
namespace CwcExim.Areas.Master.Controllers
{
    public class LoniCommodityController : BaseController
    {
        [HttpGet]
        public ActionResult CreateCommodity()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetCommodityList()
        {
            LONIMasterRepository ObjCR = new LONIMasterRepository();
            ObjCR.GetAllCommodity(0);

            List<PPGCommodity> LstCommodity = new List<PPGCommodity>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<PPGCommodity>)ObjCR.DBResponse.Data;
            }
            return PartialView("CommodityList", LstCommodity);

        }
        [HttpGet]

        public ActionResult GetComodityByListComodityCode(string ComodityCode)
        {
            LONIMasterRepository ObjCR = new LONIMasterRepository();
            ObjCR.GetMstComodityListByComodityCode(ComodityCode);

            List<PPGCommodity> LstCommodity = new List<PPGCommodity>();
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<PPGCommodity>)ObjCR.DBResponse.Data;
            }
            return PartialView("CommodityList", LstCommodity);

        }

        [HttpGet]
        public JsonResult LoadListMoreData(int Page)
        {
            LONIMasterRepository ObjCR = new LONIMasterRepository();
            List<PPGCommodity> LstCommodity = new List<PPGCommodity>();
            ObjCR.GetAllCommodity(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<PPGCommodity>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCommodity(int CommodityId)
        {
            /*
            Commodity Type:1.HAZ 2.Non HAZ
            */
            LONIMasterRepository ObjCR = new LONIMasterRepository();
            PPGCommodity ObjCommodity = new PPGCommodity();
            if (CommodityId > 0)
            {
                ObjCR.GetCommodity(CommodityId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjCommodity = (PPGCommodity)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewCommodity", ObjCommodity);
        }

        [HttpGet]
        public ActionResult EditCommodity(int CommodityId)
        {
            LONIMasterRepository ObjCr = new LONIMasterRepository();
            PPGCommodity ObjCommodity = new PPGCommodity();
            if (CommodityId > 0)
            {
                ObjCr.GetCommodity(CommodityId);
                if (ObjCr.DBResponse.Data != null)
                {
                    ObjCommodity = (PPGCommodity)ObjCr.DBResponse.Data;
                }
            }
            return PartialView("EditCommodity", ObjCommodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCommodityDetail(PPGCommodity ObjCommodity)
        {
            /*
             Commodity Type:1.HAZ 2.Non HAZ
             */
            if (ModelState.IsValid)
            {
                ObjCommodity.CommodityName = ObjCommodity.CommodityName.Trim();
                //ObjCommodity.CommodityAlias = ObjCommodity.CommodityAlias.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjCommodity.Uid = ObjLogin.Uid;
                ObjCommodity.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                LONIMasterRepository ObjCR = new LONIMasterRepository();
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
                LONIMasterRepository ObjCR = new LONIMasterRepository();
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


