using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
namespace CwcExim.Controllers
{
    public class CWCChargesController : BaseController
    {
        [HttpGet]
        public ActionResult CreateCWCCharges()
        {
            return View("CreateCWCCharges");
        }

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
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
            IList<CWCChargesGroundRent> objGR = new List<CWCChargesGroundRent>();
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<CWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(CWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export
            */
            if (ModelState.IsValid)
            {
                CWCChargesRepository objCR = new CWCChargesRepository();
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
            CWCChargesRepository objCR = new CWCChargesRepository();
            CWCChargesGroundRent objCGR = new CWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (CWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Storage Charge
        [HttpGet]
        public ActionResult CreateStorageCharge()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return View("CreateStorageCharge");
        }

        [HttpGet]
        public ActionResult EditStorageCharge(int StorageChargeId)
        {
            CWCChargesRepository ObjCC = new CWCChargesRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            StorageCharge ObjStorageCharge = new StorageCharge();
            if (StorageChargeId > 0)
            {
                ObjCC.GetStorageCharge(StorageChargeId);
                if (ObjCC.DBResponse.Data != null)
                {
                    ObjStorageCharge = (StorageCharge)ObjCC.DBResponse.Data;
                }
            }
            return View("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult GetStorageChargeList()
        {
            CWCChargesRepository ObjCR = new CWCChargesRepository();
            List<StorageCharge> LstStorageCharges = new List<StorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<StorageCharge>)ObjCR.DBResponse.Data;
            }
            return View("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(StorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository ObjCR = new CWCChargesRepository();
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
            CWCChargesRepository objCR = new CWCChargesRepository();
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
            Miscellaneous ObjMiscellaneous = new Miscellaneous();
            if (MiscellaneousId > 0)
            {
                CWCChargesRepository ObjCWCR = new CWCChargesRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                ObjCWCR.GetMiscellaneous(MiscellaneousId);
                if (ObjCWCR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (Miscellaneous)ObjCWCR.DBResponse.Data;
                }
            }
            return PartialView("EditMiscellaneous", ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            CWCChargesRepository ObjCWCR = new CWCChargesRepository();
            List<Miscellaneous> LstMiscellaneous = new List<Miscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<Miscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(Miscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository ObjCWCR = new CWCChargesRepository();
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

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(CWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository objCR = new CWCChargesRepository();
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
            List<CWCWeighment> lstWeighment = new List<CWCWeighment>();
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<CWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            CWCWeighment objCW = new CWCWeighment();
            CWCChargesRepository objCCR = new CWCChargesRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (CWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region Entry Fees
        [HttpGet]
        public ActionResult CreateEntryFees()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return View();
        }
        [HttpGet]
        public ActionResult GetAllEntryFees()
        {
            List<CWCEntryFees> lstEntryFees = new List<CWCEntryFees>();
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<CWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(CWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository objRepository = new CWCChargesRepository();
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
            CWCEntryFees objEF = new CWCEntryFees();
            CWCChargesRepository objRepo = new CWCChargesRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (CWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return View();
        }

        [HttpGet]
        public ActionResult InsuranceList()
        {
            List<Insurance> LstInsuarance = new List<Insurance>();
            CWCChargesRepository ObjCWCR = new CWCChargesRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
            LstInsuarance = (List<Insurance>)ObjCWCR.DBResponse.Data;
            return View(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            CWCChargesRepository ObjCWCR = new CWCChargesRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            Insurance ObjInsurance = new Insurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (Insurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(Insurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository ObjCWCR = new CWCChargesRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjInsurance.Uid=ObjLogin.Uid;
                ObjCWCR.AddEditInsurance(ObjInsurance);
                ModelState.Clear();
                return Json(ObjCWCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",",ModelState.Values.SelectMany(m=>m.Errors).Select(e=>e.ErrorMessage));
                var Err = new { Status=0,Message=ErrorMessage};
                return Json(Err);
            }
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
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
            IList<CWCReefer> objList = new List<CWCReefer>();
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<CWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(CWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository objCR = new CWCChargesRepository();
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
            CWCChargesRepository objCR = new CWCChargesRepository();
            CWCReefer objRef = new CWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (CWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult TdsList()
        {
            IList<CWCTds> objList = new List<CWCTds>();
            CWCChargesRepository objCR = new CWCChargesRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<CWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(CWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                CWCChargesRepository objCR = new CWCChargesRepository();
                objCR.AddEditMstTds(objTds);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditTds(int TdsId)
        {
            CWCChargesRepository objCR = new CWCChargesRepository();
            CWCTds objTds= new CWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (CWCTds)objCR.DBResponse.Data;
            }
            return PartialView(objTds);
        }
        #endregion
    }
}
