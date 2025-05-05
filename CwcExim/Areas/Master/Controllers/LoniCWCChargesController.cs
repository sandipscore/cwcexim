using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Areas.Master.Models;
using CwcExim.UtilityClasses;
using CwcExim.Controllers;

namespace CwcExim.Areas.Master.Controllers
{
    public class LoniCWCChargesController : BaseController
    {
        // GET: Master/LoniCWCCharges
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCWCCharges()
        {
            return PartialView("CreateCWCCharges");
        }

        #region Storage Charge
        [HttpGet]
        public ActionResult CreateStorageCharge()
        {
            LoniCWCChargesRepository objCR = new LoniCWCChargesRepository();
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
            LoniCWCChargesRepository ObjCC = new LoniCWCChargesRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            PpgStorageCharge ObjStorageCharge = new PpgStorageCharge();
            LONIMasterRepository objMR = new LONIMasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (PpgStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult GetStorageChargeList()
        {
            LONIMasterRepository ObjCR = new LONIMasterRepository();
            List<PpgStorageCharge> LstStorageCharges = new List<PpgStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<PpgStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(PpgStorageCharge ObjStorageCharge)
        {
            ModelState.Remove("RateSqMPerWeek");
            ModelState.Remove("RateCubMeterPerDay");
            if (ModelState.IsValid)
            {
                LONIMasterRepository ObjCR = new LONIMasterRepository();
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
            LoniCWCChargesRepository objCR = new LoniCWCChargesRepository();
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
            PPGMiscellaneous ObjMiscellaneous = new PPGMiscellaneous();
            if (MiscellaneousId > 0)
            {
                LoniCWCChargesRepository ObjCWCR = new LoniCWCChargesRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                LONIMasterRepository objRR = new LONIMasterRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (PPGMiscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            LONIMasterRepository ObjCWCR = new LONIMasterRepository();
            List<PPGMiscellaneous> LstMiscellaneous = new List<PPGMiscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<PPGMiscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(PPGMiscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                LONIMasterRepository ObjCWCR = new LONIMasterRepository();
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

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            LoniCWCChargesRepository objCR = new LoniCWCChargesRepository();
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
            IList<PpgGroundRentCharge> objGR = new List<PpgGroundRentCharge>();
            LONIMasterRepository objCR = new LONIMasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<PpgGroundRentCharge>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(PpgGroundRentCharge objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export
            */
            if (ModelState.IsValid)
            {
                LONIMasterRepository objCR = new LONIMasterRepository();
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
            LoniCWCChargesRepository ObjCC = new LoniCWCChargesRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;

            LONIMasterRepository objCR = new LONIMasterRepository();
            PpgGroundRentCharge objCGR = new PpgGroundRentCharge();
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (PpgGroundRentCharge)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Entry Fees
        [HttpGet]
        public ActionResult CreateEntryFees()
        {
            LoniCWCChargesRepository objCR = new LoniCWCChargesRepository();
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
            List<PpgCWCEntryFees> lstEntryFees = new List<PpgCWCEntryFees>();
            LONIMasterRepository objCR = new LONIMasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<PpgCWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(PpgCWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                LONIMasterRepository objRepository = new LONIMasterRepository();
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
            PpgCWCEntryFees objEF = new PpgCWCEntryFees();
            LONIMasterRepository objRepository = new LONIMasterRepository();
            LoniCWCChargesRepository objRepo = new LoniCWCChargesRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepository.GetAllEntryFees(EntryFeeId);
                if (objRepository.DBResponse.Data != null)
                    objEF = (PpgCWCEntryFees)objRepository.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Fumigation Charge

        [HttpGet]
        public ActionResult CreateFumigationCharge()
        {

            return PartialView("CreateFumigationCharge");
        }


        [HttpPost]
        public JsonResult AddEditFumigationChargeForCargo(PpgFumigationCharge objCargo)
        {
            if (ModelState.IsValid)
            {
                objCargo.lstChargeForCargo = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<FumigationChargeDetailsForCargo>>(objCargo.StringifyData);
                string XML = Utility.CreateXML(objCargo.lstChargeForCargo);
                LONIMasterRepository objER = new LONIMasterRepository();
                objCargo.StringifyData = XML;
                objER.AddEditFumigationChargeForCargo(objCargo, ((Login)(Session["LoginUser"])).Uid);
                ModelState.Clear();
                return Json(objER.DBResponse);
            }
            else
            {
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult CreateFumigationChargeContainer()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult AddEditFumigationChargeForContainer(PpgFumigationCharge objCargo)
        {
            if (ModelState.IsValid)
            {
                objCargo.lstChargeForContainer = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<FumigationChargeDetailsForContainer>>(objCargo.StringifyData);
                string XML = Utility.CreateXML(objCargo.lstChargeForContainer);
                LONIMasterRepository objER = new LONIMasterRepository();
                objCargo.StringifyData = XML;
                objER.AddEditFumigationChargeForContainer(objCargo, ((Login)(Session["LoginUser"])).Uid);
                ModelState.Clear();
                return Json(objER.DBResponse);
            }
            else
            {
                var Err = new { Statua = -1, Messgae = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult ListOfFumigationChargeDetails()
        {
            LONIMasterRepository ObjER = new LONIMasterRepository();
            List<PpgFumigationCharge> LstCharge = new List<PpgFumigationCharge>();
            ObjER.GetAllFumigationCharge();
            if (ObjER.DBResponse.Data != null)
            {
                LstCharge = (List<PpgFumigationCharge>)ObjER.DBResponse.Data;
            }
            return PartialView(LstCharge);
        }

        [HttpGet]
        public ActionResult EditFumigationChargeForCargo(int FumigationChargeId)
        {
            PpgFumigationCharge ObjCargoCharge = new PpgFumigationCharge();
            LONIMasterRepository ObjER = new LONIMasterRepository();

            if (FumigationChargeId > 0)
            {
                ObjER.GetFumigationChargebyIdForCargo(FumigationChargeId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCargoCharge = (PpgFumigationCharge)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjCargoCharge);
        }

        [HttpGet]
        public ActionResult EditFumigationChargeForContainer(int FumigationChargeId)
        {
            PpgFumigationCharge ObjCargoCharge = new PpgFumigationCharge();
            LONIMasterRepository ObjER = new LONIMasterRepository();

            if (FumigationChargeId > 0)
            {
                ObjER.GetFumigationChargebyIdForContainer(FumigationChargeId);
                if (ObjER.DBResponse.Data != null)
                {
                    ObjCargoCharge = (PpgFumigationCharge)ObjER.DBResponse.Data;
                }
            }
            return PartialView(ObjCargoCharge);
        }
        #endregion

        #region Movement Charge

        [HttpGet]
        public ActionResult CreateMovementCharge()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult MovementChargeList()
        {
            IList<PpgMovementCharge> objMC = new List<PpgMovementCharge>();
            LONIMasterRepository objCR = new LONIMasterRepository();
            objCR.GetAllMovementChargeDet();
            if (objCR.DBResponse.Data != null)
                objMC = (List<PpgMovementCharge>)objCR.DBResponse.Data;
            return PartialView(objMC);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstMovementCharge(PpgMovementCharge objCWC)
        {
            /*           
            Commodity Type: 1.HAZ 2.Non HAZ            
            */
            if (ModelState.IsValid)
            {
                LONIMasterRepository objCR = new LONIMasterRepository();
                objCR.AddEditMstMovementCharge(objCWC, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditMovementCharge(int MovementChargeId)
        {
            LONIMasterRepository objCR = new LONIMasterRepository();
            PpgMovementCharge objMC = new PpgMovementCharge();
            if (MovementChargeId > 0)
            {
                objCR.GetMovementChargeDet(MovementChargeId);
                if (objCR.DBResponse.Data != null)
                    objMC = (PpgMovementCharge)objCR.DBResponse.Data;
            }
            return PartialView(objMC);
        }

        #endregion

        //For 3.2 TKD charges(FAC and THC)
        #region TKD charges
        public ActionResult CreateFscThc()
        {
            PpgThcFscV2 objHT = new PpgThcFscV2();
            LONIMasterRepositoryV2 ObjCR = new LONIMasterRepositoryV2();
            ObjCR.GetAllLocation();
            if (ObjCR.DBResponse.Data != null)
            {
                objHT.LstLocation = (List<PPGLocationV2>)ObjCR.DBResponse.Data;
            }
            //LONIMasterRepositoryV2 objOR = new LONIMasterRepositoryV2();
            ObjCR.GetAllMstOperation();
            if (ObjCR.DBResponse.Data != null)
            {
                objHT.LstOperation = (IList<Operation>)ObjCR.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public ActionResult ViewFscThc(int FscThcId)
        {
            PpgThcFscV2 objHTCharges = new PpgThcFscV2();
            //PpgThcFscChargeReposotoryV2 ObjCR = new PpgThcFscChargeReposotoryV2();

            if (FscThcId > 0)
            {
                LONIMasterRepositoryV2 objHt = new LONIMasterRepositoryV2();
                objHt.GetHTChargesDetails(FscThcId);

                if (objHt.DBResponse.Data != null)
                    objHTCharges = (PpgThcFscV2)objHt.DBResponse.Data;
                objHt.GetAllLocation();
                if (objHt.DBResponse.Data != null)
                {
                    objHTCharges.LstLocation = (List<PPGLocationV2>)objHt.DBResponse.Data;
                }
            }
            return PartialView(objHTCharges);
        }
        [HttpGet]
        public ActionResult EditFscThc(int FscThcId)
        {
            PpgThcFscV2 objHT = new PpgThcFscV2();

            OperationRepository objOR = new OperationRepository();
            LONIMasterRepositoryV2 objHTRepo = new LONIMasterRepositoryV2();
            //PpgThcFscChargeReposotoryV2 ObjCR = new PpgThcFscChargeReposotoryV2();
            objHTRepo.GetHTChargesDetails(FscThcId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (PpgThcFscV2)objHTRepo.DBResponse.Data;
                objOR.GetAllMstOperation();
                if (objOR.DBResponse.Data != null)
                {
                    objHT.LstOperation = (IList<Operation>)objOR.DBResponse.Data;
                }
                objHTRepo.GetAllLocation();
                if (objHTRepo.DBResponse.Data != null)
                {
                    objHT.LstLocation = (List<PPGLocationV2>)objHTRepo.DBResponse.Data;
                }
            }
            return PartialView(objHT);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFscThc(PpgThcFscV2 objCharges)
        {
            LONIMasterRepositoryV2 objHTRepo = new LONIMasterRepositoryV2();
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
            LONIMasterRepositoryV2 objHT = new LONIMasterRepositoryV2();
            objHT.GetAllHTCharges();
            IList<PpgThcFscV2> lstCharges = new List<PpgThcFscV2>();
            if (objHT.DBResponse.Data != null)
                lstCharges = (List<PpgThcFscV2>)objHT.DBResponse.Data;
            return PartialView("GetAllFscThc", lstCharges);
        }
        #endregion
    }
}