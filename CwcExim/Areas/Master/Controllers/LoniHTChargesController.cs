using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Controllers;
using CwcExim.Areas.Master.Models;
using CwcExim.Repositories;
using CwcExim.Models;

namespace CwcExim.Areas.Master.Controllers
{
    public class LoniHTChargesController : BaseController
    {
        [HttpGet]
        public ActionResult CreateHTCharges()
        {
            PPGHTCharges objHT = new PPGHTCharges();
            /*ContractorRepository objCR = new ContractorRepository();
            objCR.GetAllContractor();
            if (objCR.DBResponse.Data != null)
            {
                objHT.LstContractor = (IList<Contractor>)objCR.DBResponse.Data;
            }*/
            OperationRepository objOR = new OperationRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
            {
                objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            PPGHTCharges objHTCharges = new PPGHTCharges();
            if (HTChargesId > 0)
            {
                LONIMasterRepository objHt = new LONIMasterRepository();
                objHt.GetHTChargesDetails(HTChargesId);
                if (objHt.DBResponse.Data != null)
                    objHTCharges = (PPGHTCharges)objHt.DBResponse.Data;
            }
            return PartialView(objHTCharges);
        }
        [HttpGet]
        public ActionResult EditHTCharges(int HTChargesId)
        {
            PPGHTCharges objHT = new PPGHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            LONIMasterRepository objHTRepo = new LONIMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (PPGHTCharges)objHTRepo.DBResponse.Data;
                objOR.GetAllMstOperation();
                if (objOR.DBResponse.Data != null)
                {
                    objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
                }
                /*objCR.GetAllContractor();
                if (objCR.DBResponse.Data != null)
                {
                    objHT.LstContractor = (IList<Contractor>)objCR.DBResponse.Data;
                }*/
            }
            return PartialView(objHT);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditHTCharges(PPGHTCharges objCharges)
        {
            LONIMasterRepository objHTRepo = new LONIMasterRepository();
            if (ModelState.IsValid)
            {
                int Uid = ((Login)Session["LoginUser"]).Uid;
                objHTRepo.AddEditHTCharges(objCharges, ((Login)Session["LoginUser"]).Uid);
                return Json(objHTRepo.DBResponse);
            }
            else
            {
                var Err = new { Status = -1 };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult GetAllHTCharges()
        {
            LONIMasterRepository objHT = new LONIMasterRepository();
            objHT.GetAllHTCharges();
            IList<PPGHTCharges> lstCharges = new List<PPGHTCharges>();
            if (objHT.DBResponse.Data != null)
                lstCharges = (List<PPGHTCharges>)objHT.DBResponse.Data;
            return PartialView("HTChargesList", lstCharges);
        }
    }
}