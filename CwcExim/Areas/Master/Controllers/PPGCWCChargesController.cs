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
    public class PPGCWCChargesController : BaseController
    {
        // GET: Master/PPGCWCCharges
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
            CWCChargesRepository objCR = new CWCChargesRepository();
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
            CWCChargesRepository ObjCC = new CWCChargesRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            PpgStorageCharge ObjStorageCharge = new PpgStorageCharge();
            PPGMasterRepository objMR = new PPGMasterRepository();
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
            PPGMasterRepository ObjCR = new PPGMasterRepository();
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
                PPGMasterRepository ObjCR = new PPGMasterRepository();
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
            PPGMiscellaneous ObjMiscellaneous = new PPGMiscellaneous();
            if (MiscellaneousId > 0)
            {
                CWCChargesRepository ObjCWCR = new CWCChargesRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                PPGMasterRepository objRR = new PPGMasterRepository();
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
            PPGMasterRepository ObjCWCR = new PPGMasterRepository();
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
                PPGMasterRepository ObjCWCR = new PPGMasterRepository();
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
            IList<PpgGroundRentCharge> objGR = new List<PpgGroundRentCharge>();
            PPGMasterRepository objCR = new PPGMasterRepository();
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
                PPGMasterRepository objCR = new PPGMasterRepository();
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
            CWCChargesRepository ObjCC = new CWCChargesRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;

            PPGMasterRepository objCR = new PPGMasterRepository();
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
            CWCChargesRepository objCR = new CWCChargesRepository();
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
            PPGMasterRepository objCR = new PPGMasterRepository();
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
                PPGMasterRepository objRepository = new PPGMasterRepository();
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
            PPGMasterRepository objRepository = new PPGMasterRepository();
            CWCChargesRepository objRepo = new CWCChargesRepository();
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
                PPGMasterRepository objER = new PPGMasterRepository();
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
                PPGMasterRepository objER = new PPGMasterRepository();
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
            PPGMasterRepository ObjER = new PPGMasterRepository();
            List<PpgFumigationCharge>LstCharge = new List<PpgFumigationCharge>();
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
            PPGMasterRepository ObjER = new PPGMasterRepository();

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
            PPGMasterRepository ObjER = new PPGMasterRepository();

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
            PPGMasterRepository objCR = new PPGMasterRepository();
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
                PPGMasterRepository objCR = new PPGMasterRepository();
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
            PPGMasterRepository objCR = new PPGMasterRepository();
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
            PPGMasterRepositoryV2 ObjCR = new PPGMasterRepositoryV2();
            ObjCR.GetAllLocation();
            if (ObjCR.DBResponse.Data != null)
            {
                objHT.LstLocation = (List<PPGLocationV2>)ObjCR.DBResponse.Data;
            }
            //PPGMasterRepositoryV2 objOR = new PPGMasterRepositoryV2();
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
                PPGMasterRepositoryV2 objHt = new PPGMasterRepositoryV2();
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
            PPGMasterRepositoryV2 objHTRepo = new PPGMasterRepositoryV2();
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
            PPGMasterRepositoryV2 objHTRepo = new PPGMasterRepositoryV2();
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
            PPGMasterRepositoryV2 objHT = new PPGMasterRepositoryV2();
            objHT.GetAllHTCharges();
            IList<PpgThcFscV2> lstCharges = new List<PpgThcFscV2>();
            if (objHT.DBResponse.Data != null)
                lstCharges = (List<PpgThcFscV2>)objHT.DBResponse.Data;
            return PartialView("GetAllFscThc", lstCharges);
        }
        #endregion
    }
}