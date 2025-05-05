using CwcExim.Areas.Report.Models;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Report.Controllers
{
    public class RptContainerStatusController : Controller
    {
        [HttpGet]
        public ActionResult LoadContainerStatusForPpg()
        {
            /*bool ShippingLine;*/
            //ContainerStatus ObjContainer = new ContainerStatus();
            //ReportRepository ObjRR = new ReportRepository();
           // ExportRepository ObjER = new ExportRepository();
            /*ShippingLine = ((Login)Session["LoginUser"]).ShippingLine;
             if(ShippingLine == true)
            {
                ObjContainer.ShippingLineName = ((Login)Session["LoginUser"]).Name;
                ObjContainer.ShippingLineId = ((Login)Session["LoginUser"]).EximTraderId;
                List<ShippingLineList> LstShippingLine = new List<ShippingLineList>();
                LstShippingLine.Add(new ShippingLineList
                {
                    ShippingLineName = ((Login)Session["LoginUser"]).Name,
                    ShippingLineId = ((Login)Session["LoginUser"]).EximTraderId
                });
                ViewBag.ShippingLineList = new SelectList(LstShippingLine, "ShippingLineId", "ShippingLineName");
                ObjRR.GetContainerNoForContStatus(ObjContainer.ShippingLineId);
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ContainerList = new SelectList((List<ContainerList>)ObjRR.DBResponse.Data, "ContainerNo", "ContainerNo");
                else
                    ViewBag.ContainerList = null;
            }
            else
            {*/
            //ViewBag.ContainerList = null;
            //    ObjER.GetShippingLine();
            //    if (ObjER.DBResponse.Data != null)
            //        ViewBag.ShippingLineList = new SelectList((List<Areas.Export.Models.ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            //    else
            //        ViewBag.ShippingLineList = null;
            /*}*/

            return PartialView(/*ObjContainer*/);
        }

        [HttpGet]
        public ActionResult LoadContainerStatus()
        {
            /*bool ShippingLine;*/
            //ContainerStatus ObjContainer = new ContainerStatus();
            //ReportRepository ObjRR = new ReportRepository();
            ExportRepository ObjER = new ExportRepository();
            /*ShippingLine = ((Login)Session["LoginUser"]).ShippingLine;
             if(ShippingLine == true)
            {
                ObjContainer.ShippingLineName = ((Login)Session["LoginUser"]).Name;
                ObjContainer.ShippingLineId = ((Login)Session["LoginUser"]).EximTraderId;
                List<ShippingLineList> LstShippingLine = new List<ShippingLineList>();
                LstShippingLine.Add(new ShippingLineList
                {
                    ShippingLineName = ((Login)Session["LoginUser"]).Name,
                    ShippingLineId = ((Login)Session["LoginUser"]).EximTraderId
                });
                ViewBag.ShippingLineList = new SelectList(LstShippingLine, "ShippingLineId", "ShippingLineName");
                ObjRR.GetContainerNoForContStatus(ObjContainer.ShippingLineId);
                if (ObjRR.DBResponse.Data != null)
                    ViewBag.ContainerList = new SelectList((List<ContainerList>)ObjRR.DBResponse.Data, "ContainerNo", "ContainerNo");
                else
                    ViewBag.ContainerList = null;
            }
            else
            {*/
            ViewBag.ContainerList = null;
            ObjER.GetShippingLine();
            if (ObjER.DBResponse.Data != null)
                ViewBag.ShippingLineList = new SelectList((List<Areas.Export.Models.ShippingLine>)ObjER.DBResponse.Data, "ShippingLineId", "ShippingLineName");
            else
                ViewBag.ShippingLineList = null;
            /*}*/

            return PartialView(/*ObjContainer*/);
        }


        [HttpGet]
        public JsonResult GetContainerNoListForPpg()
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContainerNoForTrackContStatus();
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContWiseICDList(string ContainerNo)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContWiseICDList(ContainerNo);
            if(ObjRR.DBResponse.Status>0)
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetContWiseLatestICD(string ContainerNo)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContWiseLatestICD(ContainerNo);
            if (ObjRR.DBResponse.Status > 0)
            {
                return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetContainerStatusDetailForPpg(string ICDCode)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContainerDetForTrackContStatus(ICDCode);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerNoList(int ShippingLineId)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContainerNoForContStatus(ShippingLineId);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContainerStatusDetail(int ShippingLineId, string ContainerNo)
        {
            ReportRepository ObjRR = new ReportRepository();
            ObjRR.GetContainerDetForContStatus(ShippingLineId, ContainerNo);
            return Json(ObjRR.DBResponse, JsonRequestBehavior.AllowGet);
        }

    }
}