using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using CwcExim.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.UtilityClasses;

namespace CwcExim.Areas.Master.Controllers
{
    public class AMDMasterController : BaseController
    {
        #region CWCCharges

        [HttpGet]
        public ActionResult CreateCWCCharges()
        {
            return PartialView("CreateCWCCharges");
        }
        #endregion

        #region Franchise
        [HttpGet]
        public ActionResult CreateFranchiseCharges()
        {
            AMDMasterRepository objCR = new AMDMasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditFranchise(int franchisechargeid)
        {
            AMDFranchiseCharges objFC = new AMDFranchiseCharges();
            AMDMasterRepository objRepo = new AMDMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (AMDFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(AMDFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                AMDMasterRepository objRepository = new AMDMasterRepository();
                objRepository.AddEditMstFranchiseCharges(objFC, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult FranchiseList()
        {
            IList<AMDFranchiseCharges> objList = new List<AMDFranchiseCharges>();
            AMDMasterRepository objCR = new AMDMasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<AMDFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            AMDMasterRepository objCR = new AMDMasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ReeferList()
        {
            IList<AMDReefer> objList = new List<AMDReefer>();
            AMDMasterRepository objCR = new AMDMasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<AMDReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(AMDReefer objRef)
        {
            if (ModelState.IsValid)
            {
                AMDMasterRepository objCR = new AMDMasterRepository();
                objCR.AddEditMstReefer(objRef);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditReefer(int ReeferChrgId)
        {
            AMDMasterRepository objCR = new AMDMasterRepository();
            AMDReefer objRef = new AMDReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (AMDReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Party Wise HTCharges  
        public ActionResult CreateHTChargesPtyWise()
        {
            AMDMasterRepository objPort = new AMDMasterRepository();
            HTChargesPtyWise objHT = new HTChargesPtyWise();
            List<CHNPort> LstPort = new List<CHNPort>();            
            OperationRepository objOR = new OperationRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
            {
                objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
            }
            objPort.GetAllPort();
            if (objPort.DBResponse.Data != null)
            {
                objHT.LstPort = (IList<CHNPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabDataPtyWise(string Size, string ChargesFor, string OperationCode)
        {
            AMDMasterRepository objHT = new AMDMasterRepository();
            objHT.GetSlabData(Size, ChargesFor, OperationCode);
            HTChargesPtyWise lstSlab = new HTChargesPtyWise();
            if (objHT.DBResponse.Data != null)
                lstSlab = (HTChargesPtyWise)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtlPtyWise(int HTChargesID)
        {
            AMDMasterRepository objHT = new AMDMasterRepository();
            objHT.GetHTSlabChargesDtl(HTChargesID);
            List<ChargeListPtyWise> lstSlab = new List<ChargeListPtyWise>();
            if (objHT.DBResponse.Data != null)
                lstSlab = (List<ChargeListPtyWise>)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartyNameForHTCharges(int Page, string PartyCode)
        {
            AMDMasterRepository obj = new AMDMasterRepository();
            obj.GetPartyNameForHTCharges(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditHTChargesPtyWise(int HTChargesId)
        {

            HTChargesPtyWise objHT = new HTChargesPtyWise();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<CHNPort> LstPort = new List<CHNPort>();
            AMDMasterRepository objHTRepo = new AMDMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (HTChargesPtyWise)objHTRepo.DBResponse.Data;
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
            objHTRepo.GetAllPort();
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT.LstPort = (IList<CHNPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult EditHTChargesOtherPtyWise(int HTChargesId)
        {
            HTChargesPtyWise objHT = new HTChargesPtyWise();
            AMDMasterRepository objHTRepo = new AMDMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTChargesPtyWise(int HTChargesId)
        {
            HTChargesPtyWise objHT = new HTChargesPtyWise();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            AMDMasterRepository objHTRepo = new AMDMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (HTChargesPtyWise)objHTRepo.DBResponse.Data;
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
            objHTRepo.GetAllPort();
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT.LstPort = (IList<CHNPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTChargesPtyWise(HTChargesPtyWise objCharges, String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<ChargeListPtyWise> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ChargeListPtyWise>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            AMDMasterRepository objHTRepo = new AMDMasterRepository();
            
            int Uid = ((Login)Session["LoginUser"]).Uid;
            objHTRepo.AddEditHTCharges(objCharges, ((Login)Session["LoginUser"]).Uid, ChargeListXML);
            return Json(objHTRepo.DBResponse);
            
        }
        
        [HttpGet]
        public JsonResult GetAllHTChargesPtyWise()
        {
            AMDMasterRepository objHT = new AMDMasterRepository();
            objHT.GetAllHTChargesPtyWise();
            HTChargesPtyWise lstCharges = new HTChargesPtyWise();
            if (objHT.DBResponse.Data != null)
                lstCharges = (HTChargesPtyWise)objHT.DBResponse.Data;
            return Json(lstCharges, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}