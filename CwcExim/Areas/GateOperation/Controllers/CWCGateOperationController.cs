using CwcExim.Areas.GateOperation.Models;
using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Filters;
namespace CwcExim.Areas.GateOperation.Controllers
{
    public class CWCGateOperationController : BaseController
    {
        #region Entry Through Gate

        [HttpGet]
        public ActionResult CreateEntryThroughGate()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult EntryThroughGateList()
        {
            //GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
            List<EntryThroughGate> LstGateEntry = new List<EntryThroughGate>();
            //ObjGOR.GetAllEntryThroughGate();
            //if (ObjGOR.DBResponse.Data != null)
            //{
            //    LstGateEntry = (List<EntryThroughGate>)ObjGOR.DBResponse.Data;
            //}
            return View(LstGateEntry);
        }

        [HttpGet]
        public ActionResult EditEntryThroughGate(int EntryId)
        {
            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            //if (EntryId > 0)
            //{
            //    GateOperationRepository ObjGOR = new GateOperationRepository();
            //    ObjGOR.GetEntryThroughGate();
            //    if (ObjGOR.DBResponse.Data != null)
            //    {
            //        ObjEntryGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
            //    }
            //}
            return View(ObjEntryGate);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddEditEntryThroughGate(EntryThroughGate ObjEntryGate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
        //        Login ObjLogin = (Login)Session["LoginUser"];
        //        ObjEntryGate.Uid=ObjLogin.Uid;
        //        ObjGOR.AddEditEntryThroughGate(ObjEntryGate);
        //        ModelState.Clear();
        //        return Json(ObjGOR.DBResponse);
        //    }
        //    else
        //    {
        //        var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e=>e.ErrorMessage));
        //        var Err = new { Status = 0, Message = ErrorMessage };
        //        return Json(Err);
        //    }
        //}

        [HttpGet]
        public ActionResult ViewEntryThroughGate(int EntryId)
        {
            EntryThroughGate ObjEntryGate = new EntryThroughGate();
            //if (EntryId > 0)
            //{
            //    GateOperationRepository ObjGOR = new GateOperationRepository();
            //    ObjGOR.GetEntryThroughGate();
            //    if (ObjGOR.DBResponse.Data != null)
            //    {
            //        ObjEntryGate = (EntryThroughGate)ObjGOR.DBResponse.Data;
            //    }
            //}
            return View(ObjEntryGate);
        }

        //[CustomValidateAntiForgeryToken]
        //[HttpGet]
        //public ActionResult DeleteEntryThroughGate(int EntryId)
        //{
        //    if(EntryId > 0)
        //    {
        //        GateOpeartionRepository ObjGOR = new GateOpeartionRepository();
        //        ObjGOR.DeleteEntryThroughGate(EntryId);
        //        return Json(ObjGOR.DBResponse);
        //    }
        //    else
        //    {
        //        var Err = new { Status = 1, Message = "Error" };
        //        return Json(Err);
        //    }
        //}
        #endregion
    }
}