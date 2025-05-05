using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Master.Controllers
{
    public class PPGThcFscChargesV2Controller : BaseController
    {
        // GET: Master/PPGThcFscChargesV2
        public ActionResult CreateFscThc()
        {
            PpgThcFscV2 objHT = new PpgThcFscV2();
            PpgThcFscChargeReposotoryV2 ObjCR = new PpgThcFscChargeReposotoryV2();
            ObjCR.GetAllLocation();
            if (ObjCR.DBResponse.Data != null)
            {
                objHT.LstLocation = (List<PPGLocationV2>)ObjCR.DBResponse.Data;
            }
            PpgThcFscChargeReposotoryV2 objOR = new PpgThcFscChargeReposotoryV2();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
            {
                objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
            }
            return PartialView(objHT);
        }

        
        [HttpGet]
        public ActionResult ViewFscThc(int FscThcId)
        {
            PpgThcFscV2 objHTCharges = new PpgThcFscV2();
            PpgThcFscChargeReposotoryV2 ObjCR = new PpgThcFscChargeReposotoryV2();
           
            if (FscThcId > 0)
            {
                PpgThcFscChargeReposotoryV2 objHt = new PpgThcFscChargeReposotoryV2();
                objHt.GetHTChargesDetails(FscThcId);

                if (objHt.DBResponse.Data != null)
                    objHTCharges = (PpgThcFscV2)objHt.DBResponse.Data;
                ObjCR.GetAllLocation();
                if (ObjCR.DBResponse.Data != null)
                {
                    objHTCharges.LstLocation = (List<PPGLocationV2>)ObjCR.DBResponse.Data;
                }
            }
            return PartialView(objHTCharges);
        }
        [HttpGet]
        public ActionResult EditFscThc(int FscThcId)
        {
            PpgThcFscV2 objHT = new PpgThcFscV2();
          
            OperationRepository objOR = new OperationRepository();
            PpgThcFscChargeReposotoryV2 objHTRepo = new PpgThcFscChargeReposotoryV2();
            PpgThcFscChargeReposotoryV2 ObjCR = new PpgThcFscChargeReposotoryV2();
            objHTRepo.GetHTChargesDetails(FscThcId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (PpgThcFscV2)objHTRepo.DBResponse.Data;
                objOR.GetAllMstOperation();
                if (objOR.DBResponse.Data != null)
                {
                    objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
                }
                ObjCR.GetAllLocation();
                if (ObjCR.DBResponse.Data != null)
                {
                    objHT.LstLocation = (List<PPGLocationV2>)ObjCR.DBResponse.Data;
                }
            }
            return PartialView(objHT);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFscThc(PpgThcFscV2 objCharges)
         {
            PpgThcFscChargeReposotoryV2 objHTRepo = new PpgThcFscChargeReposotoryV2();
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
        public ActionResult GetAllFscThc()
        {
            PpgThcFscChargeReposotoryV2 objHT = new PpgThcFscChargeReposotoryV2();
            objHT.GetAllHTCharges();
            IList<PpgThcFscV2> lstCharges = new List<PpgThcFscV2>();
            if (objHT.DBResponse.Data != null)
                lstCharges = (List<PpgThcFscV2>)objHT.DBResponse.Data;
            return PartialView("GetAllFscThc", lstCharges);
        }

    }
}