using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Master.Models;
using CwcExim.Models;
using System.Web.Mvc;
using CwcExim.Repositories;
using Newtonsoft.Json;
using CwcExim.Filters;
using CwcExim.UtilityClasses;


namespace CwcExim.Areas.Master.Controllers
{
    public class TTUNMasterController : Controller
    {
        // GET: Master/TTUNMaster
        public ActionResult Index()
        {
            return View();
        }

        #region Misc Charges

        public ActionResult CreateCHNCharges()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCodeRate();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<CHNSac>)objCR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objCR.ListOfChargeName();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<CHNMiscCharge>)objCR.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEntryRateFees(CHNStorageCharge objStorage)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objRepository = new TTUN_RateRepository();
                objRepository.AddEditMstRateFees(objStorage, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetAllMiscRateFees()
        {
            List<CHNStorageCharge> lstMiscRateFees = new List<CHNStorageCharge>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetAllMiscRateFees(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<CHNStorageCharge>)objCR.DBResponse.Data;
            return PartialView("MiscRateFeesList", lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFees(int StorageChargeId)
        {
            CHNStorageCharge objSC = new CHNStorageCharge();
            TTUN_RateRepository objRepo = new TTUN_RateRepository();
            objRepo.ListOfSACCodeRate();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<CHNSac>)objRepo.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objRepo.ListOfChargeName();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<CHNMiscCharge>)objRepo.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            if (StorageChargeId > 0)
            {
                objRepo.GetAllMiscRateFees(StorageChargeId);
                if (objRepo.DBResponse.Data != null)
                    objSC = (CHNStorageCharge)objRepo.DBResponse.Data;
            }
            return PartialView("EditMiscRateFees", objSC);
        }

        #endregion

        #region CWCCharges

        [HttpGet]
        public ActionResult CreateCWCCharges()
        {
            return PartialView("CreateCWCCharges");
        }

        #region Storage Charge
        [HttpGet]
        public ActionResult CreateStorageCharge()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView("CreateStorageCharge");
        }

        [HttpGet]
        public ActionResult EditStorageCharge(int StorageChargeId)
        {
            TTUN_RateRepository ObjCC = new TTUN_RateRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            CHNCWCStorageCharge ObjStorageCharge = new CHNCWCStorageCharge();
            TTUN_RateRepository objMR = new TTUN_RateRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (CHNCWCStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            TTUN_RateRepository ObjCR = new TTUN_RateRepository();
            List<CHNCWCStorageCharge> LstStorageCharges = new List<CHNCWCStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<CHNCWCStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(CHNCWCStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository ObjCR = new TTUN_RateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjStorageCharge.Uid = ObjLogin.Uid;
                ObjCR.AddEditStorageCharge(ObjStorageCharge);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion

        #region Miscellaneous
        [HttpGet]
        public ActionResult CreateMiscellaneous()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView("CreateMiscellaneous");
        }

        [HttpGet]
        public ActionResult EditMiscellaneous(int MiscellaneousId)
        {
            CHNMiscellaneous ObjMiscellaneous = new CHNMiscellaneous();
            if (MiscellaneousId > 0)
            {
                TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                TTUN_RateRepository objRR = new TTUN_RateRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (CHNMiscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
            List<CHNMiscellaneous> LstMiscellaneous = new List<CHNMiscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<CHNMiscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(CHNMiscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjMiscellaneous.Uid = ObjLogin.Uid;
                ObjCWCR.AddEditMiscellaneous(ObjMiscellaneous);
                ModelState.Clear();
                return Json(ObjCWCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion
        #endregion
        #region Entry Fees
        [HttpGet]
        public ActionResult CreateEntryFees()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult GetAllEntryFees()
        {
            List<CHNCWCEntryFees> lstEntryFees = new List<CHNCWCEntryFees>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<CHNCWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(CHNCWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objRepository = new TTUN_RateRepository();
                objRepository.AddEditMstEntryFees(objEF, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditEntryFees(int EntryFeeId)
        {
            CHNCWCEntryFees objEF = new CHNCWCEntryFees();
            TTUN_RateRepository objRepo = new TTUN_RateRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (CHNCWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult GroundRentList()
        {
            IList<CHNCWCChargesGroundRent> objGR = new List<CHNCWCChargesGroundRent>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<CHNCWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(CHNCWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export


            */
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objCR = new TTUN_RateRepository();
                objCR.AddEditMstGroundRent(objCWC, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditGroundRent(int GroundRentId)
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            CHNCWCChargesGroundRent objCGR = new CHNCWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (CHNCWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
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
            IList<CHNCWCReefer> objList = new List<CHNCWCReefer>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<CHNCWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(CHNCWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objCR = new TTUN_RateRepository();
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
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            CHNCWCReefer objRef = new CHNCWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (CHNCWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(CHNCWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objCR = new TTUN_RateRepository();
                objCR.AddEditMstWeighment(objCW, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = new { Message = "Error", Status = -1 };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult GetAllWeighment()
        {
            List<CHNCWCWeighment> lstWeighment = new List<CHNCWCWeighment>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<CHNCWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            CHNCWCWeighment objCW = new CHNCWCWeighment();
            TTUN_RateRepository objCCR = new TTUN_RateRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (CHNCWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion
        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult InsuranceList()
        {
            List<CHNInsurance> LstInsuarance = new List<CHNInsurance>();
            TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<CHNInsurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            CHNInsurance ObjInsurance = new CHNInsurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (CHNInsurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(CHNInsurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository ObjCWCR = new TTUN_RateRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjInsurance.Uid = ObjLogin.Uid;
                ObjCWCR.AddEditInsurance(ObjInsurance);
                ModelState.Clear();
                return Json(ObjCWCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion

        #region Franchise
        [HttpGet]
        public ActionResult CreateFranchiseCharges()
        {
            TTUN_RateRepository objCR = new TTUN_RateRepository();
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
            CHNCWCFranchiseCharges objFC = new CHNCWCFranchiseCharges();
            TTUN_RateRepository objRepo = new TTUN_RateRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (CHNCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(CHNCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                TTUN_RateRepository objRepository = new TTUN_RateRepository();
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
            IList<CHNCWCFranchiseCharges> objList = new List<CHNCWCFranchiseCharges>();
            TTUN_RateRepository objCR = new TTUN_RateRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<CHNCWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion

        #region HTCharges  
        public ActionResult CreateHTCharges()
        {
            TTUN_RateRepository objPort = new TTUN_RateRepository();
            CHNHTCharges objHT = new CHNHTCharges();
            List<CHNPort> LstPort = new List<CHNPort>();
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
            objPort.GetAllPort();
            if (objPort.DBResponse.Data != null)
            {
                objHT.LstPort = (IList<CHNPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabData(string Size, string ChargesFor, string OperationCode)
        {
            TTUN_RateRepository objHT = new TTUN_RateRepository();
            objHT.GetSlabData(Size, ChargesFor, OperationCode);
            CHNHTCharges lstSlab = new CHNHTCharges();
            if (objHT.DBResponse.Data != null)
                lstSlab = (CHNHTCharges)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtl(int HTChargesID)
        {
            TTUN_RateRepository objHT = new TTUN_RateRepository();
            objHT.GetHTSlabChargesDtl(HTChargesID);
            List<CHNChargeList> lstSlab = new List<CHNChargeList>();
            if (objHT.DBResponse.Data != null)
                lstSlab = (List<CHNChargeList>)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult ViewHTCharges(int HTChargesId)
        //{
        //    PPGHTCharges objHTCharges = new PPGHTCharges();
        //    if (HTChargesId > 0)
        //    {
        //        PPGMasterRepository objHt = new PPGMasterRepository();
        //        objHt.GetHTChargesDetails(HTChargesId);
        //        if (objHt.DBResponse.Data != null)
        //            objHTCharges = (PPGHTCharges)objHt.DBResponse.Data;
        //    }
        //    return PartialView(objHTCharges);
        //}
        [HttpGet]
        public ActionResult EditHTCharges(int HTChargesId)
        {

            CHNHTCharges objHT = new CHNHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<CHNPort> LstPort = new List<CHNPort>();
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
            TTUN_RateRepository objHTRepo = new TTUN_RateRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (CHNHTCharges)objHTRepo.DBResponse.Data;
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
        public JsonResult EditHTChargesOther(int HTChargesId)
        {
            CHNHTCharges objHT = new CHNHTCharges();
            TTUN_RateRepository objHTRepo = new TTUN_RateRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            CHNHTCharges objHT = new CHNHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            TTUN_RateRepository objHTRepo = new TTUN_RateRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (CHNHTCharges)objHTRepo.DBResponse.Data;
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
            return PartialView("ViewHTCharges", objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTCharges(CHNHTCharges objCharges, String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<CHNChargeList> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHNChargeList>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            TTUN_RateRepository objHTRepo = new TTUN_RateRepository();
            //ModelState.Remove("CommodityType");
            //if (ModelState.IsValid)
            //{
            int Uid = ((Login)Session["LoginUser"]).Uid;
            objHTRepo.AddEditHTCharges(objCharges, ((Login)Session["LoginUser"]).Uid, ChargeListXML);
            return Json(objHTRepo.DBResponse);
            //}
            //else
            //{
            //    var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            //    var Err = new { Status = -1 };
            //    return Json(Err);
            //}
        }
        //[HttpGet]
        //public ActionResult GetAllHTCharges()
        //{
        //    HDBMasterRepository objHT = new HDBMasterRepository();
        //    objHT.GetAllHTCharges();
        //    IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
        //    if (objHT.DBResponse.Data != null)
        //        lstCharges = (List<HDBHTCharges>)objHT.DBResponse.Data;
        //    return PartialView("HTChargesList", lstCharges);
        //}
        [HttpGet]
        public JsonResult GetAllHTCharges()
        {
            TTUN_RateRepository objHT = new TTUN_RateRepository();
            objHT.GetAllHTCharges();
            //IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
            CHNHTCharges lstCharges = new CHNHTCharges();
            if (objHT.DBResponse.Data != null)
                lstCharges = (CHNHTCharges)objHT.DBResponse.Data;
            return Json(lstCharges, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}