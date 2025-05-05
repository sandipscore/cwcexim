
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
    public class VRN_MasterController : Controller
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
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_CWCFranchiseCharges objFC = new VRN_CWCFranchiseCharges();
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (VRN_CWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(VRN_CWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objRepository = new VRN_MasterRepository();
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
            IList<VRN_CWCFranchiseCharges> objList = new List<VRN_CWCFranchiseCharges>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion

        #region Misc Charges

        public ActionResult CreateMiscCharges()
        {
            VRN_RateRepository objCR = new VRN_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<VRN_Sac>)objCR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objCR.ListOfChargeName();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<VRN_MiscCharge>)objCR.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEntryRateFees(VRN_StorageCharge objStorage)
        {
            if (ModelState.IsValid)
            {
                VRN_RateRepository objRepository = new VRN_RateRepository();
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
            List<VRN_StorageCharge> lstMiscRateFees = new List<VRN_StorageCharge>();
            VRN_RateRepository objCR = new VRN_RateRepository();
            objCR.GetAllMiscRateFees(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<VRN_StorageCharge>)objCR.DBResponse.Data;
            return PartialView("MiscRateFeesList", lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFees(int StorageChargeId)
        {
            VRN_StorageCharge objSC = new VRN_StorageCharge();
            VRN_RateRepository objRepo = new VRN_RateRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<VRN_Sac>)objRepo.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objRepo.ListOfChargeName();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<VRN_MiscCharge>)objRepo.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            if (StorageChargeId > 0)
            {
                objRepo.GetAllMiscRateFees(StorageChargeId);
                if (objRepo.DBResponse.Data != null)
                    objSC = (VRN_StorageCharge)objRepo.DBResponse.Data;
            }
            return PartialView("EditMiscRateFees", objSC);
        }

        #endregion

        #region Entry Fees
        [HttpGet]
        public ActionResult CreateEntryFees()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            List<VRN_CWCEntryFees> lstEntryFees = new List<VRN_CWCEntryFees>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<VRN_CWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(VRN_CWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objRepository = new VRN_MasterRepository();
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
            VRN_CWCEntryFees objEF = new VRN_CWCEntryFees();
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (VRN_CWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            IList<VRN_CWCChargesGroundRent> objGR = new List<VRN_CWCChargesGroundRent>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<VRN_CWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(VRN_CWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export


            */
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCChargesGroundRent objCGR = new VRN_CWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (VRN_CWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            IList<VRN_CWCReefer> objList = new List<VRN_CWCReefer>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(VRN_CWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCReefer objRef = new VRN_CWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (VRN_CWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(VRN_CWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            List<VRN_CWCWeighment> lstWeighment = new List<VRN_CWCWeighment>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<VRN_CWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            VRN_CWCWeighment objCW = new VRN_CWCWeighment();
            VRN_MasterRepository objCCR = new VRN_MasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (VRN_CWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            IList<VRN_CWCTds> objList = new List<VRN_CWCTds>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(VRN_CWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCTds objTds = new VRN_CWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (VRN_CWCTds)objCR.DBResponse.Data;
            }
            return PartialView(objTds);
        }
        #endregion

        #region Godown
        [HttpGet]
        public ActionResult CreateGodown()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditGodown(int GodownId)
        {
            VRN_GodownVM ObjGodown = new VRN_GodownVM();
            if (GodownId > 0)
            {
                VRN_MasterRepository ObjGR = new VRN_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (VRN_GodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            VRN_GodownVM ObjGodown = new VRN_GodownVM();
            if (GodownId > 0)
            {
                VRN_MasterRepository ObjGR = new VRN_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (VRN_GodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            VRN_MasterRepository ObjGR = new VRN_MasterRepository();
            ObjGR.GetAllGodown();
            List<VRN_Godown> LstGodown = new List<VRN_Godown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<VRN_Godown>)ObjGR.DBResponse.Data;
            }
            return PartialView("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(VRN_GodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_GodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<VRN_GodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjGR = new VRN_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjGodown.MstGodwon.Uid = ObjLogin.Uid;
                string LocationXML = Utility.CreateXML(ObjGodown.LstLocation);
                ObjGodown.MstGodwon.GodownName = ObjGodown.MstGodwon.GodownName.Trim();
                // ObjGodown.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjGR.AddEditGodown(ObjGodown, LocationXML, DelLocationXML);
                ModelState.Clear();
                return Json(ObjGR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteGodownDetail(int GodownId)
        {
            if (GodownId > 0)
            {
                VRN_MasterRepository ObjGR = new VRN_MasterRepository();
                ObjGR.DeleteGodown(GodownId);
                return Json(ObjGR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Chemical
        public ActionResult CreateChemical()
        {
            return PartialView("CreateChemical");
        }

        [HttpGet]
        public ActionResult EditChemical(int ChemicalId)
        {
            VRN_MasterRepository ObjCh = new VRN_MasterRepository();
            VRN_Chemical ObjChem = new VRN_Chemical();
            if (ChemicalId > 0)
            {

                ObjCh.GetChemical(ChemicalId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (VRN_Chemical)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditChemical", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemical(int ChemicalId)
        {
            VRN_Chemical ObjYard = new VRN_Chemical();
            if (ChemicalId > 0)
            {
                VRN_MasterRepository ObjYR = new VRN_MasterRepository();
                ObjYR.GetChemical(ChemicalId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (VRN_Chemical)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemical", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalList()

        {
            VRN_MasterRepository ObjYR = new VRN_MasterRepository();
            ObjYR.GetAllChemical();
            List<VRN_Chemical> LstChemical = new List<VRN_Chemical>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<VRN_Chemical>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(VRN_Chemical ObjChem)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCR = new VRN_MasterRepository();
                ObjChem.ChemicalName = ObjChem.ChemicalName;
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChem.Uid = ObjLogin.Uid;
                ObjChem.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditChemical(ObjChem);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
                //var Err = new { Status = 0, Message = "Please fill all the required details" };
                //return Json(Err);
            }
        }



        #endregion

        #region Storage Charge
        [HttpGet]
        public ActionResult CreateStorageCharge()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_MasterRepository ObjCC = new VRN_MasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            VRN_CWCStorageCharge ObjStorageCharge = new VRN_CWCStorageCharge();
            VRN_MasterRepository objMR = new VRN_MasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (VRN_CWCStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            VRN_MasterRepository ObjCR = new VRN_MasterRepository();
            List<VRN_CWCStorageCharge> LstStorageCharges = new List<VRN_CWCStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<VRN_CWCStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(VRN_CWCStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCR = new VRN_MasterRepository();
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

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            List<VRN_Insurance> LstInsuarance = new List<VRN_Insurance>();
            VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<VRN_Insurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            VRN_Insurance ObjInsurance = new VRN_Insurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (VRN_Insurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(VRN_Insurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
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

        #region Miscellaneous
        [HttpGet]
        public ActionResult CreateMiscellaneous()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
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
            VRN_Miscellaneous ObjMiscellaneous = new VRN_Miscellaneous();
            if (MiscellaneousId > 0)
            {
                VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                VRN_MasterRepository objRR = new VRN_MasterRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (VRN_Miscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
            List<VRN_Miscellaneous> LstMiscellaneous = new List<VRN_Miscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<VRN_Miscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(VRN_Miscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
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

        #region port
        [HttpGet]
        public ActionResult CreatePort()
        {
            CountryRepository ObjCR = new CountryRepository();
            ViewBag.Country = null;
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            return PartialView("CreatePort");
        }

        [HttpGet]
        public ActionResult GetPortList()
        {
            VRN_MasterRepository ObjPR = new VRN_MasterRepository();
            List<VRN_Port> LstPort = new List<VRN_Port>();
            ObjPR.GetAllPort();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<VRN_Port>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(VRN_Port ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias = ObjPort.PortAlias.Trim();
                ObjPort.PortName = ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid = ObjLogin.Uid;
                VRN_MasterRepository ObjPR = new VRN_MasterRepository();
                ObjPR.AddEditPort(ObjPort);
                ModelState.Clear();
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeletePortDetail(int PortId)
        {
            if (PortId > 0)
            {
                VRN_MasterRepository ObjPR = new VRN_MasterRepository();
                ObjPR.DeletePort(PortId);
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditPort(int PortId)
        {
            VRN_Port ObjPort = new VRN_Port();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                VRN_MasterRepository ObjPR = new VRN_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (VRN_Port)ObjPR.DBResponse.Data;
                }
                CountryRepository ObjCR = new CountryRepository();
                ObjCR.GetAllCountry();
                if (ObjCR.DBResponse.Data != null)
                {
                    ViewBag.Country = ObjCR.DBResponse.Data;
                }
            }
            return PartialView("EditPort", ObjPort);
        }

        [HttpGet]
        public ActionResult ViewPort(int PortId)
        {
            VRN_Port ObjPort = new VRN_Port();
            if (PortId > 0)
            {
                VRN_MasterRepository ObjPR = new VRN_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (VRN_Port)ObjPR.DBResponse.Data;
                }
            }
            return PartialView("ViewPort", ObjPort);
        }
        #endregion

        #region GST AGAINST SAC
        public ActionResult CreateSAC()
        {
            return PartialView();
        }
        public ActionResult ViewSAC(int SACId)
        {
            VRN_MasterRepository ObjSR = new VRN_MasterRepository();
            VRN_Sac ObjSac = new VRN_Sac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (VRN_Sac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC(string SacNo="")
        {
            VRN_MasterRepository ObjSR = new VRN_MasterRepository();
            List<VRN_Sac> LstSac = new List<VRN_Sac>();
            ObjSR.GetAllSac(SacNo);
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<VRN_Sac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(VRN_Sac ObjSac)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjSR = new VRN_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSac.Uid = ObjLogin.Uid;
                if (ObjSac.Description != null)
                    ObjSac.Description = ObjSac.Description.Trim();
                ObjSR.AddSac(ObjSac);
                ModelState.Clear();
                return Json(ObjSR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion

        #region SDOpening
     
        [HttpGet]
        public ActionResult CreateSDopening()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            VRN_MasterRepository ObjRepo = new VRN_MasterRepository();
            VRNSDOpening ObjSD = new VRNSDOpening();
            ObjRepo.GetPartyForSDOpening("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjRepo.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstParty = Jobject["lstParty"];
                ViewBag.State = Jobject["State"];
            }
            else
            {
                ViewBag.lstParty = null;
            }

            for (var i = 0; i < 5; i++)
            {
                ObjSD.Details.Add(new VRNReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH"},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView("CreateSDopening", ObjSD);
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            VRN_MasterRepository objMaster = new VRN_MasterRepository();
            objMaster.GetPartyForSDOpening(PartyCode, 0);
            return Json(objMaster.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            VRN_MasterRepository objMaster = new VRN_MasterRepository();
            objMaster.GetPartyForSDOpening(PartyCode, Page);
            return Json(objMaster.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetSDList()
        {
            VRN_MasterRepository ObjSDR = new VRN_MasterRepository();
            List<VRNSDOpening> LstSD = new List<VRNSDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<VRNSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult GetSDListByParty(string PartyName)
        {
            VRN_MasterRepository ObjSDR = new VRN_MasterRepository();
            List<VRNSDOpening> LstSD = new List<VRNSDOpening>();
            ObjSDR.SearchSDByPartyName(PartyName);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<VRNSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            VRNSDOpening ObjSD = new VRNSDOpening();
            if (SDId > 0)
            {
                VRN_MasterRepository ObjSDR = new VRN_MasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (VRNSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(VRNSDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                VRN_MasterRepository ObjSDR = new VRN_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSD.Uid = ObjLogin.Uid;
                ObjSDR.AddSDOpening(ObjSD, xml);
                ModelState.Clear();
                return Json(ObjSDR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 1, Message = ErrorMessage };
                return Json(Err);
            }
        }



        #endregion

        #region Party Wise CWCCharges

        [HttpGet]
        public ActionResult CreatePtyWiseCWCCharges()
        {
            return PartialView("CreatePtyWiseCWCCharges");
        }

        public JsonResult GetPartyNameForCWCCharges(int Page, string PartyCode)
        {
            VRN_MasterRepository obj = new VRN_MasterRepository();
            obj.GetPartyNameForCWCCharges(Page, PartyCode);
            return Json(obj.DBResponse, JsonRequestBehavior.AllowGet);
        }


        #region Entry Fees

        [HttpGet]
        public ActionResult CreatePtyWiseEntryFees()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetPtyWiseAllEntryFees()
        {
            List<VRN_CWCPtyWiseEntryFees> lstEntryFees = new List<VRN_CWCPtyWiseEntryFees>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllPtyWiseEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<VRN_CWCPtyWiseEntryFees>)objCR.DBResponse.Data;
            return PartialView("GetPtyWiseAllEntryFees", lstEntryFees);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditPtyWiseEntryFees(VRN_CWCPtyWiseEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objRepository = new VRN_MasterRepository();
                objRepository.AddEditPtyWiseMstEntryFees(objEF, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditPtyWiseEntryFees(int EntryFeeId)
        {
            VRN_CWCPtyWiseEntryFees objEF = new VRN_CWCPtyWiseEntryFees();
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllPtyWiseEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (VRN_CWCPtyWiseEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditPtyWiseEntryFees", objEF);
        }

        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRentPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult GroundRentListPtyWise()
        {
            IList<VRN_CWCChargesGroundRentPtyWise> objGR = new List<VRN_CWCChargesGroundRentPtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllGroundRentDetPtyWise();
            if (objCR.DBResponse.Data != null)
                objGR = (List<VRN_CWCChargesGroundRentPtyWise>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRentPtyWise(VRN_CWCChargesGroundRentPtyWise objCWC)
        {            
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
                objCR.AddEditMstGroundRentPtyWise(objCWC, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditGroundRentPtyWise(int GroundRentId)
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCChargesGroundRentPtyWise objCGR = new VRN_CWCChargesGroundRentPtyWise();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDetPtyWise(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (VRN_CWCChargesGroundRentPtyWise)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReeferPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult ReeferListPtyWise()
        {
            IList<VRN_CWCReeferPtyWise> objList = new List<VRN_CWCReeferPtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllReeferPtyWise();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCReeferPtyWise>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReeferPtyWise(VRN_CWCReeferPtyWise objRef)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
                objCR.AddEditMstReeferPtyWise(objRef);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditReeferPtyWise(int ReeferChrgId)
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCReeferPtyWise objRef = new VRN_CWCReeferPtyWise();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDetPtyWise(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (VRN_CWCReeferPtyWise)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Storage Charge
        [HttpGet]
        public ActionResult CreateStorageChargePtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView("CreateStorageChargePtyWise");
        }

        [HttpGet]
        public ActionResult EditStorageChargePtyWise(int StorageChargeId)
        {
            VRN_MasterRepository ObjCC = new VRN_MasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            VRN_CWCStorageChargePtyWise ObjStorageCharge = new VRN_CWCStorageChargePtyWise();
            VRN_MasterRepository objMR = new VRN_MasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageChargePtyWise(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (VRN_CWCStorageChargePtyWise)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageChargePtyWise", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeListPtyWise()
        {
            VRN_MasterRepository ObjCR = new VRN_MasterRepository();
            List<VRN_CWCStorageChargePtyWise> LstStorageCharges = new List<VRN_CWCStorageChargePtyWise>();
            ObjCR.GetAllStorageChargePtyWise();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<VRN_CWCStorageChargePtyWise>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeListPtyWise", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageChargePtyWise(VRN_CWCStorageChargePtyWise ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCR = new VRN_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjStorageCharge.Uid = ObjLogin.Uid;
                ObjCR.AddEditStorageChargePtyWise(ObjStorageCharge);
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

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurancePtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult InsuranceListPtyWise()
        {
            List<VRN_InsurancePtyWise> LstInsuarance = new List<VRN_InsurancePtyWise>();
            VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
            ObjCWCR.GetAllInsurancePtyWise();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<VRN_InsurancePtyWise>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurancePtyWise(int InsuranceId)
        {
            VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            VRN_InsurancePtyWise ObjInsurance = new VRN_InsurancePtyWise();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurancePtyWise(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (VRN_InsurancePtyWise)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurancePtyWise(VRN_InsurancePtyWise ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository ObjCWCR = new VRN_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjInsurance.Uid = ObjLogin.Uid;
                ObjCWCR.AddEditInsurancePtyWise(ObjInsurance);
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
        public ActionResult CreateWeighmentPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighmentPtyWise(VRN_CWCWeighmentPtyWise objCW)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
                objCR.AddEditMstWeighmentPtyWise(objCW, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = new { Message = "Error", Status = -1 };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult GetAllWeighmentPtyWise()
        {
            List<VRN_CWCWeighmentPtyWise> lstWeighment = new List<VRN_CWCWeighmentPtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetWeighmentDetPtyWise(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<VRN_CWCWeighmentPtyWise>)objCR.DBResponse.Data;
            return PartialView(lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighmentPtyWise(int WeighmentId)
        {
            VRN_CWCWeighmentPtyWise objCW = new VRN_CWCWeighmentPtyWise();
            VRN_MasterRepository objCCR = new VRN_MasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDetPtyWise(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (VRN_CWCWeighmentPtyWise)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTdsPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult TdsListPtyWise()
        {
            IList<VRN_CWCTdsPtyWise> objList = new List<VRN_CWCTdsPtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllTDSPtyWise();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCTdsPtyWise>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTdsPtyWise(VRN_CWCTdsPtyWise objTds)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objCR = new VRN_MasterRepository();
                objCR.AddEditMstTdsPtyWise(objTds);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditTdsPtyWise(int TdsId)
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_CWCTdsPtyWise objTds = new VRN_CWCTdsPtyWise();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDetPtyWise(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (VRN_CWCTdsPtyWise)objCR.DBResponse.Data;
            }
            return PartialView(objTds);
        }
        #endregion

        #region Franchise
        [HttpGet]
        public ActionResult CreateFranchiseChargesPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }

        [HttpGet]
        public ActionResult EditFranchisePtyWise(int franchisechargeid)
        {
            VRN_CWCFranchiseChargesPtyWise objFC = new VRN_CWCFranchiseChargesPtyWise();
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseChargePtyWise(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (VRN_CWCFranchiseChargesPtyWise)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseChargesPtyWise(VRN_CWCFranchiseChargesPtyWise objFC)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objRepository = new VRN_MasterRepository();
                objRepository.AddEditMstFranchiseChargesPtyWise(objFC, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult FranchiseListPtyWise()
        {
            IList<VRN_CWCFranchiseChargesPtyWise> objList = new List<VRN_CWCFranchiseChargesPtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllFranchiseChargesPtyWise();
            if (objCR.DBResponse.Data != null)
                objList = (List<VRN_CWCFranchiseChargesPtyWise>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion

        #region Misc Charges

        public ActionResult CreateMiscChargesPtyWise()
        {
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            VRN_RateRepository objCRR = new VRN_RateRepository();
            objCRR.ListOfSACCode();
            if (objCRR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<VRN_Sac>)objCRR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objCR.ListOfChargeName();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<VRN_MiscChargePtyWise>)objCR.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEntryRateFeesPtyWise(VRN_MiscChargePtyWise objMisc)
        {
            if (ModelState.IsValid)
            {
                VRN_MasterRepository objRepository = new VRN_MasterRepository();
                objRepository.AddEditMstRateFeesPtyWise(objMisc, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Err = new { State = -1, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetAllMiscRateFeesPtyWise()
        {
            List<VRN_MiscChargePtyWise> lstMiscRateFees = new List<VRN_MiscChargePtyWise>();
            VRN_MasterRepository objCR = new VRN_MasterRepository();
            objCR.GetAllMiscRateFeesPtyWise(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<VRN_MiscChargePtyWise>)objCR.DBResponse.Data;
            return PartialView(lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFeesPtyWise(int StorageChargeId)
        {
            VRN_MiscChargePtyWise objMisc = new VRN_MiscChargePtyWise();
            VRN_MasterRepository objRepo = new VRN_MasterRepository();
            VRN_RateRepository objCRR = new VRN_RateRepository();
            objCRR.ListOfSACCode();
            if (objCRR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<VRN_Sac>)objCRR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objRepo.ListOfChargeNamePtyWise();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<VRN_MiscChargePtyWise>)objRepo.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            if (StorageChargeId > 0)
            {
                objRepo.GetAllMiscRateFeesPtyWise(StorageChargeId);
                if (objRepo.DBResponse.Data != null)
                    objMisc = (VRN_MiscChargePtyWise)objRepo.DBResponse.Data;
            }
            return PartialView(objMisc);
        }

        #endregion



        #endregion


    }


}