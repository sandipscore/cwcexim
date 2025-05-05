using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
namespace CwcExim.Controllers
{
           /*
           Container Type: 1.Empty Container 2.Loaded Container 3.Cargo 4.RMS
           Type: 1.General 2.Heavy/Scrap
           Commodity Type: 1.HAZ 2.Non HAZ
           */
    public class HTChargesController : BaseController
    {
        [HttpGet]
        public ActionResult CreateHTCharges()
        {
            HTChargesVM objHT = new HTChargesVM();
            ContractorRepository objCR = new ContractorRepository();
            objCR.GetAllContractor();
            if (objCR.DBResponse.Data != null)
            {
                objHT.LstContractor = (IList<Contractor>)objCR.DBResponse.Data;
            }
            OperationRepository objOR = new OperationRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
            {
                objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
            }
            return View(objHT);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            HTCharges objHTCharges = new HTCharges();
            if (HTChargesId > 0)
            {
                HTRepository objHt = new HTRepository();
                objHt.GetHTChargesDetails(HTChargesId);
                if (objHt.DBResponse.Data != null)
                    objHTCharges = (HTChargesVM)objHt.DBResponse.Data;
            }
            return View(objHTCharges);
        }
        [HttpGet]
        public ActionResult EditHTCharges(int HTChargesId)
        {
            HTChargesVM objHT = new HTChargesVM();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            //objCR.GetAllContractor();
            //if (objCR.DBResponse.Data != null)
            //{
            //    objHT.LstContractor = (IList<Contractor>)objCR.DBResponse.Data;
            //}
            //objOR.GetAllMstOperation();
            //if (objOR.DBResponse.Data != null)
            //{
            //    objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
            //}
            HTRepository objHTRepo = new HTRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (HTChargesVM)objHTRepo.DBResponse.Data;
                objOR.GetAllMstOperation();
                if (objOR.DBResponse.Data != null)
                {
                    objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
                }
                objCR.GetAllContractor();
                if (objCR.DBResponse.Data != null)
                {
                    objHT.LstContractor = (IList<Contractor>)objCR.DBResponse.Data;
                }
            }
            return View(objHT);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditHTCharges(HTChargesVM objCharges)
        {
            HTRepository objHTRepo = new HTRepository();
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
            HTRepository objHT = new HTRepository();
            objHT.GetAllHTCharges();
            IList<HTCharges> lstCharges = new List<HTCharges>();
            if (objHT.DBResponse.Data != null)
                lstCharges = (List<HTCharges>)objHT.DBResponse.Data;
            return PartialView("HTChargesList", lstCharges);
        }
    }
}