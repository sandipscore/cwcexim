using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.UtilityClasses;
using CwcExim.Filters;

namespace CwcExim.Controllers
{
    public class YardController : BaseController
    {
        [HttpGet]
        public ActionResult CreateYard()
        {
            return View("CreateYard");
        }

        [HttpGet]
        public ActionResult EditYard(int YardId)
        {
            YardVM ObjYard = new YardVM();
            if(YardId > 0)
            {
                YardRepository ObjYR = new YardRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (YardVM)ObjYR.DBResponse.Data;
                    ObjYard.LocationDetail=Newtonsoft.Json.JsonConvert.SerializeObject(ObjYard.LstYard);
                }
            }
            return View("EditYard", ObjYard);
        }

        [HttpGet]
        public ActionResult ViewYard(int YardId)
        {
            YardVM ObjYard = new YardVM();
            if (YardId > 0)
            {
                YardRepository ObjYR = new YardRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard=(YardVM)ObjYR.DBResponse.Data;
                }
            }
            return View("ViewYard", ObjYard);
        }

        [HttpGet]
        public ActionResult GetYardList()
        {
            YardRepository ObjYR = new YardRepository();
            ObjYR.GetAllYard();
            List<Yard> LstYard = new List<Yard>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstYard = (List<Yard>)ObjYR.DBResponse.Data;
            }
            return View("YardList", LstYard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditYardDetail(YardVM ObjYard)
        {
            var DelLocationXML="";
            string LocationXML;
            if (ModelState.IsValid)
            {
                if (ObjYard.LocationDetail != null)
                {
                    ObjYard.LstYard = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<YardWiseLocation>>(ObjYard.LocationDetail);
                }
                if (ObjYard.DelLocationDetail != null)
                {
                    var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<YardWiseLocation>>(ObjYard.DelLocationDetail);
                    DelLocationXML = Utility.CreateXML(DelLocationList);
                }
                LocationXML = Utility.CreateXML(ObjYard.LstYard);
                YardRepository ObjYR = new YardRepository();
                ObjYard.MstYard.YardName=ObjYard.MstYard.YardName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjYard.MstYard.Uid=ObjLogin.Uid;
                ObjYR.AddEditYard(ObjYard,LocationXML,DelLocationXML);
                ModelState.Clear();
                return Json(ObjYR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",",ModelState.Values.SelectMany(m=>m.Errors).Select(e=>e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteYardDetail(int YardId)
        {
            if (YardId > 0)
            {
                YardRepository ObjYR = new YardRepository();
                ObjYR.DeleteYard(YardId);
                return Json(ObjYR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
    }
}