using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class OperationController : BaseController
    {
        [HttpGet]
        public ActionResult CreateOperation()
        {
            SacRepository objSR = new SacRepository();
            Operation objOPR = new Operation();
            objSR.GetAllSac();
            if (objSR.DBResponse.Data != null)
                objOPR.LstSac=(List<SAC>) objSR.DBResponse.Data;
            return View("CreateOperation",objOPR);
        }

        [HttpGet]
        public ActionResult ViewOperation(int OperationId)
        {
            Operation ObjOperation = new Operation();
            if (OperationId>0)
            {
                OperationRepository ObjOR = new OperationRepository();
                ObjOR.ViewMstOperation(OperationId);
                if (ObjOR.DBResponse.Data != null)
                {
                    ObjOperation=(Operation)ObjOR.DBResponse.Data;
                }
            }
            return View("ViewOperation", ObjOperation);
        }

        [HttpGet]
        public ActionResult GetOperationList()
        {
            List<Operation> lstOperation = new List<Operation>();
            OperationRepository objOR = new OperationRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
                lstOperation = (List<Operation>)objOR.DBResponse.Data;
            return View("OperationList", lstOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOperation(Operation ObjOperation)
        {
            if (ModelState.IsValid)
            {
                ObjOperation.ShortDescription=ObjOperation.ShortDescription == null ? null : ObjOperation.ShortDescription.Trim();
                ObjOperation.Description = ObjOperation.Description == null ? null : ObjOperation.Description.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjOperation.Uid=ObjLogin.Uid;
                OperationRepository ObjOR = new OperationRepository();
                ObjOR.AddMstOperation(ObjOperation);
                ModelState.Clear();
                return Json(ObjOR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m=>m.Errors).Select(e=>e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
    }
}