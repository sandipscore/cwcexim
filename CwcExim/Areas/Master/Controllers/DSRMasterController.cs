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
    public class DSRMasterController : Controller
    {
        // GET: Master/DSRMaster
        #region Misc Charges

        public ActionResult CreateDSRCharges()
        {
            DSR_RateRepository objCR = new DSR_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<DSRSac>)objCR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objCR.ListOfChargeName();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<DSRMiscCharge>)objCR.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEntryRateFees(DSRStorageCharge objStorage)
        {
            if (ModelState.IsValid)
            {
                DSR_RateRepository objRepository = new DSR_RateRepository();
                objRepository.AddEditMstRateFees(objStorage, ((Login)Session["LoginUser"]).Uid);
                return Json(objRepository.DBResponse,JsonRequestBehavior.AllowGet);
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
            List<DSRStorageCharge> lstMiscRateFees = new List<DSRStorageCharge>();
            DSR_RateRepository objCR = new DSR_RateRepository();
            objCR.GetAllMiscRateFees(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<DSRStorageCharge>)objCR.DBResponse.Data;
            return PartialView("MiscRateFeesList", lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFees(int StorageChargeId)
        {
            DSRStorageCharge objSC = new DSRStorageCharge();
            DSR_RateRepository objRepo = new DSR_RateRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<DSRSac>)objRepo.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objRepo.ListOfChargeName();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<DSRMiscCharge>)objRepo.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            if (StorageChargeId > 0)
            {
                objRepo.GetAllMiscRateFees(StorageChargeId);
                if (objRepo.DBResponse.Data != null)
                    objSC = (DSRStorageCharge)objRepo.DBResponse.Data;
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
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            DSR_MasterRepository ObjCC = new DSR_MasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            DSRCWCStorageCharge ObjStorageCharge = new DSRCWCStorageCharge();
            DSR_MasterRepository objMR = new DSR_MasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (DSRCWCStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            DSR_MasterRepository ObjCR = new DSR_MasterRepository();
            List<DSRCWCStorageCharge> LstStorageCharges = new List<DSRCWCStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<DSRCWCStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(DSRCWCStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjCR = new DSR_MasterRepository();
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
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            DSRMiscellaneous ObjMiscellaneous = new DSRMiscellaneous();
            if (MiscellaneousId > 0)
            {
                DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                DSR_MasterRepository objRR = new DSR_MasterRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (DSRMiscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
            List<DSRMiscellaneous> LstMiscellaneous = new List<DSRMiscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<DSRMiscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(DSRMiscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
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

        #region eximtrader
        [HttpGet]
        public ActionResult CreateEximTrader()
        {
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = (List<Country>)ObjCR.DBResponse.Data;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetEximTraderList()
        {
            DSR_MasterRepository ObjETR = new DSR_MasterRepository();
            List<DSREximTrader> LstEximTrader = new List<DSREximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DSREximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            DSR_MasterRepository ObjCR = new DSR_MasterRepository();
            List<DSREximTrader> LstCommodity = new List<DSREximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<DSREximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            DSR_MasterRepository ObjETR = new DSR_MasterRepository();
            List<DSREximTrader> LstEximTrader = new List<DSREximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<DSREximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }


        [HttpGet]
        public ActionResult GetPortListByPortData(string PortData)
        {
            DSR_MasterRepository ObjETR = new DSR_MasterRepository();
            List<DSRPort> LstPort = new List<DSRPort>();
                       
            ObjETR.GetGetAllPortByPortData(PortData);
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<DSRPort>)ObjETR.DBResponse.Data;
                
            }
            return PartialView("GetPortList", LstPort);
        }


        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            DSREximTrader ObjEximTrader = new DSREximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                DSR_MasterRepository ObjETR = new DSR_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DSREximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            DSREximTrader ObjEximTrader = new DSREximTrader();
            if (EximTraderId > 0)
            {
                DSR_MasterRepository ObjETR = new DSR_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (DSREximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(DSREximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
             //   Login ObjLogin = (Login)Session["LoginUser"];
              //  ObjEximTrader.Uid = ObjLogin.Uid;
                DSR_MasterRepository ObjETR = new DSR_MasterRepository();
                ObjETR.AddEditEximTrader(ObjEximTrader);
                ModelState.Clear();
                return Json(ObjETR.DBResponse);
            }
            else
            {
                ObjEximTrader.Password = ObjEximTrader.HdnPassword;
                ObjEximTrader.ConfirmPassword = ObjEximTrader.ConfirmPassword;
                var ErroMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErroMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteEximTraderDetail(int EximTraderId)
        {
            if (EximTraderId > 0)
            {
                DSR_MasterRepository ObjETR = new DSR_MasterRepository();
                ObjETR.DeleteEximTrader(EximTraderId);
                return Json(ObjETR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region HTCharges  
        public ActionResult CreateHTCharges()
        {
            DSR_MasterRepository objPort = new DSR_MasterRepository();
            DSRHTCharges objHT = new DSRHTCharges();
            List<DSRPort> LstPort = new List<DSRPort>();
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
                objHT.LstPort = (IList<DSRPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabData(string Size, string ChargesFor,string OperationCode)
        {
            DSR_MasterRepository objHT = new DSR_MasterRepository();
            objHT.GetSlabData(Size, ChargesFor, OperationCode);
            DSRHTCharges lstSlab = new DSRHTCharges();
            if (objHT.DBResponse.Data != null)
                lstSlab = (DSRHTCharges)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtl(int HTChargesID)
        {
            DSR_MasterRepository objHT = new DSR_MasterRepository();
            objHT.GetHTSlabChargesDtl(HTChargesID);
            List<DSRChargeList> lstSlab = new List<DSRChargeList>();
            if (objHT.DBResponse.Data != null)
                lstSlab = (List<DSRChargeList>)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult ViewHTCharges(int HTChargesId)
        //{
        //    PPGHTCharges objHTCharges = new PPGHTCharges();
        //    if (HTChargesId > 0)
        //    {
        //        DSR_MasterRepository objHt = new DSR_MasterRepository();
        //        objHt.GetHTChargesDetails(HTChargesId);
        //        if (objHt.DBResponse.Data != null)
        //            objHTCharges = (PPGHTCharges)objHt.DBResponse.Data;
        //    }
        //    return PartialView(objHTCharges);
        //}
        [HttpGet]
        public ActionResult EditHTCharges(int HTChargesId)
        {

            DSRHTCharges objHT = new DSRHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<DSRPort> LstPort = new List<DSRPort>();
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
            DSR_MasterRepository objHTRepo = new DSR_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (DSRHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<DSRPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult EditHTChargesOther(int HTChargesId)
        {
            DSRHTCharges objHT = new DSRHTCharges();
            DSR_MasterRepository objHTRepo = new DSR_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            DSRHTCharges objHT = new DSRHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            DSR_MasterRepository objHTRepo = new DSR_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (DSRHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<DSRPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView("ViewHTCharges", objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTCharges(DSRHTCharges objCharges, String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<DSRChargeList> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRChargeList>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            DSR_MasterRepository objHTRepo = new DSR_MasterRepository();
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
            DSR_MasterRepository objHT = new DSR_MasterRepository();
            objHT.GetAllHTCharges();
            //IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
            DSRHTCharges lstCharges = new DSRHTCharges();
            if (objHT.DBResponse.Data != null)
                lstCharges = (DSRHTCharges)objHT.DBResponse.Data;
            return Json(lstCharges, JsonRequestBehavior.AllowGet);
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
            DSR_MasterRepository ObjPR = new DSR_MasterRepository();
            List<DSRPort> LstPort = new List<DSRPort>();
            ObjPR.GetAllPort();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<DSRPort>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(DSRPort ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias = ObjPort.PortAlias.Trim();
                ObjPort.PortName = ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid = ObjLogin.Uid;
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
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
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
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
            DSRPort ObjPort = new DSRPort();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (DSRPort)ObjPR.DBResponse.Data;
                }
                CountryRepository ObjCR = new CountryRepository();
                ObjCR.GetAllCountry();
                if (ObjCR.DBResponse.Data != null)
                {
                    ViewBag.Country = ObjCR.DBResponse.Data;
                }
            }
            return View("EditPort", ObjPort);
        }

        [HttpGet]
        public ActionResult ViewPort(int PortId)
        {
            DSRPort ObjPort = new DSRPort();
            if (PortId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (DSRPort)ObjPR.DBResponse.Data;
                }
            }
            return View("ViewPort", ObjPort);
        }
        #endregion

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            List<DSRInsurance> LstInsuarance = new List<DSRInsurance>();
            DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<DSRInsurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            DSRInsurance ObjInsurance = new DSRInsurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (DSRInsurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(DSRInsurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjCWCR = new DSR_MasterRepository();
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

        #region GST AGAINST SAC
        public ActionResult CreateSAC()
        {
            return PartialView();
        }
        public ActionResult ViewSAC(int SACId)
        {
            DSR_MasterRepository ObjSR = new DSR_MasterRepository();
            DSRSac ObjSac = new DSRSac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (DSRSac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            DSR_MasterRepository ObjSR = new DSR_MasterRepository();
            List<DSRSac> LstSac = new List<DSRSac>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<DSRSac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(DSRSac ObjSac)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjSR = new DSR_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSac.BranchId = Convert.ToInt32(Session["BranchId"]);
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

            DSR_MasterRepository ObjRepo = new DSR_MasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            DSRSearchEximTraderData obj = new DSRSearchEximTraderData();
            ObjRepo.GetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((DSRSearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((DSRSearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new DSRSDOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new DSRReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
                 new SelectListItem { Text = "BANK GURANTEE", Value = "BANKGURANTEE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView("CreateSDopening", model);
        }

        [HttpGet]
        public ActionResult GetSDList()
        {
            DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
            List<DSRSDOpening> LstSD = new List<DSRSDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<DSRSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult GetSDListPartyCode(string PartyCode)
        {
            DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
            List<DSRSDOpening> LstSD = new List<DSRSDOpening>();
            ObjSDR.GetSDListPartyCode(PartyCode);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<DSRSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            DSRSDOpening ObjSD = new DSRSDOpening();
            if (SDId > 0)
            {
                DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (DSRSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(DSRSDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSD.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
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
        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.GetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.SearchByPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OnAccountOpening
        [HttpGet]
        public ActionResult CreateOAopening()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            DSR_MasterRepository ObjRepo = new DSR_MasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            DSRSearchEximTraderData obj = new DSRSearchEximTraderData();
            ObjRepo.OAGetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((DSRSearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((DSRSearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new DSROAOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new DSRReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
                 new SelectListItem { Text = "BANK GURANTEE", Value = "BANKGURANTEE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            return PartialView("CreateOAopening", model);
        }

        [HttpGet]
        public ActionResult GetOAList()
        {
            DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
            List<DSROAOpening> LstSD = new List<DSROAOpening>();
            ObjSDR.GetAllOAOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<DSROAOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("OnAccountList", LstSD);
        }

        [HttpGet]
        public ActionResult GetOAListPartyCode(string PartyCode)
        {
            DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
            List<DSROAOpening> LstSD = new List<DSROAOpening>();
            ObjSDR.GetOAListPartyCode(PartyCode);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<DSROAOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("OnAccountList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewOAOpening(int OnAcId)
        {
            DSROAOpening ObjSD = new DSROAOpening();
            if (OnAcId > 0)
            {
                DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
                ObjSDR.GetOAOpening(OnAcId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (DSROAOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewOAOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditOAopening(DSROAOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                DSR_MasterRepository ObjSDR = new DSR_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjSD.Uid = ObjLogin.Uid;
                ObjSDR.AddOAOpening(ObjSD, xml);
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
        [HttpGet]
        public JsonResult OALoadEximtradeList(string PartyCode, int Page)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.OAGetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult OASearchByPartyCode(string PartyCode)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.OASearchByPartyCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region YARD
        public ActionResult CreateYard()
        {
            return PartialView("CreateYard");
        }

        [HttpGet]
        public ActionResult EditYard(int YardId)
        {
            DSRYardVM ObjYard = new DSRYardVM();
            if (YardId > 0)
            {
                DSR_MasterRepository ObjYR = new DSR_MasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (DSRYardVM)ObjYR.DBResponse.Data;
                    ObjYard.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjYard.LstYard);
                }
            }
            return PartialView("EditYard", ObjYard);
        }

        [HttpGet]
        public ActionResult ViewYard(int YardId)
        {
            DSRYardVM ObjYard = new DSRYardVM();
            if (YardId > 0)
            {
                DSR_MasterRepository ObjYR = new DSR_MasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (DSRYardVM)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewYard", ObjYard);
        }

        [HttpGet]
        public ActionResult GetYardList()
        {
            DSR_MasterRepository ObjYR = new DSR_MasterRepository();
            ObjYR.GetAllYard();
            List<DSRYard> LstYard = new List<DSRYard>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstYard = (List<DSRYard>)ObjYR.DBResponse.Data;
            }
            return PartialView("YardList", LstYard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditYardDetail(DSRYardVM ObjYard)
        {
            var DelLocationXML = "";
            string LocationXML;
            if (ModelState.IsValid)
            {
                if (ObjYard.LocationDetail != null)
                {
                    ObjYard.LstYard = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRYardWiseLocation>>(ObjYard.LocationDetail);
                }
                if (ObjYard.DelLocationDetail != null)
                {
                    var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRYardWiseLocation>>(ObjYard.DelLocationDetail);
                    DelLocationXML = Utility.CreateXML(DelLocationList);
                }
                LocationXML = Utility.CreateXML(ObjYard.LstYard);
                DSR_MasterRepository ObjYR = new DSR_MasterRepository();
                ObjYard.MstYard.YardName = ObjYard.MstYard.YardName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjYard.MstYard.Uid = ObjLogin.Uid;
                ObjYR.AddEditYard(ObjYard, LocationXML, DelLocationXML);
                ModelState.Clear();
                return Json(ObjYR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteYardDetail(int YardId)
        {
            if (YardId > 0)
            {
                DSR_MasterRepository ObjYR = new DSR_MasterRepository();
                ObjYR.DeleteYard(YardId);
                return Json(ObjYR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }
        #endregion

        #region Operation
        [HttpGet]
        public ActionResult CreateOperation()
        {
            DSR_MasterRepository objSR = new DSR_MasterRepository();
            DSROperation objOPR = new DSROperation();
            objSR.GetAllSac();
            if (objSR.DBResponse.Data != null)
                objOPR.LstSac = (List<DSRSac>)objSR.DBResponse.Data;
            return PartialView("CreateOperation", objOPR);
        }

        [HttpGet]
        public ActionResult ViewOperation(int OperationId)
        {
            DSROperation ObjOperation = new DSROperation();
            if (OperationId > 0)
            {
                DSR_MasterRepository ObjOR = new DSR_MasterRepository();
                ObjOR.ViewMstOperation(OperationId);
                if (ObjOR.DBResponse.Data != null)
                {
                    ObjOperation = (DSROperation)ObjOR.DBResponse.Data;
                }
            }
            return PartialView("ViewOperation", ObjOperation);
        }

        [HttpGet]
        public ActionResult GetOperationList()
        {
            List<DSROperation> lstOperation = new List<DSROperation>();
            DSR_MasterRepository objOR = new DSR_MasterRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
                lstOperation = (List<DSROperation>)objOR.DBResponse.Data;
            return PartialView("OperationList", lstOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOperation(DSROperation ObjOperation)
        {
            if (ModelState.IsValid)
            {
                ObjOperation.ShortDescription = ObjOperation.ShortDescription == null ? null : ObjOperation.ShortDescription.Trim();
                ObjOperation.Description = ObjOperation.Description == null ? null : ObjOperation.Description.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjOperation.Uid = ObjLogin.Uid;
                DSR_MasterRepository ObjOR = new DSR_MasterRepository();
                ObjOR.AddMstOperation(ObjOperation);
                ModelState.Clear();
                return Json(ObjOR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }
        #endregion

        #region Entry Fees
        [HttpGet]
        public ActionResult CreateEntryFees()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            List<DSRCWCEntryFees> lstEntryFees = new List<DSRCWCEntryFees>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<DSRCWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(DSRCWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objRepository = new DSR_MasterRepository();
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
            DSRCWCEntryFees objEF = new DSRCWCEntryFees();
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (DSRCWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            IList<DSRCWCChargesGroundRent> objGR = new List<DSRCWCChargesGroundRent>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<DSRCWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(DSRCWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export


            */
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            DSRCWCChargesGroundRent objCGR = new DSRCWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (DSRCWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Incentive
        [HttpGet]
        public ActionResult CreateVolumeBasedIncentive()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpGet]
        public ActionResult VolumeBaseIncentiveList()
        {
            IList<DSRVolumeBaseIncentive> objGR = new List<DSRVolumeBaseIncentive>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllVolumeBaseIncentiveDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<DSRVolumeBaseIncentive>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstVolumeBaseIncentive(DSRVolumeBaseIncentive objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export


            */
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objCR = new DSR_MasterRepository();
                objCR.AddEditMstVolumeBaseIncentive(objCWC, ((Login)Session["LoginUser"]).Uid);
                return Json(objCR.DBResponse);
            }
            else
            {
                var Err = String.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(k => k.ErrorMessage));
                return Json(Err);
            }
        }
        [HttpGet]
        public ActionResult EditVolumeBasedIncentive(int IncentiveId)
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            DSRVolumeBaseIncentive objCGR = new DSRVolumeBaseIncentive();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (IncentiveId > 0)
            {
                objCR.GetVolumeBaseIncentiveDet(IncentiveId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (DSRVolumeBaseIncentive)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            IList<DSRCWCReefer> objList = new List<DSRCWCReefer>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<DSRCWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(DSRCWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            DSRCWCReefer objRef = new DSRCWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (DSRCWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
          
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)     
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else            
               ViewBag.ListOfSAC = null;
           

            
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(DSRCWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            List<DSRCWCWeighment> lstWeighment = new List<DSRCWCWeighment>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<DSRCWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            DSRCWCWeighment objCW = new DSRCWCWeighment();
            DSR_MasterRepository objCCR = new DSR_MasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (DSRCWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            IList<DSRCWCTds> objList = new List<DSRCWCTds>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<DSRCWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(DSRCWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objCR = new DSR_MasterRepository();
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
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            DSRCWCTds objTds = new DSRCWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (DSRCWCTds)objCR.DBResponse.Data;
            }
            return PartialView(objTds);
        }
        #endregion

        #region Godown
        [HttpGet]
        public ActionResult CreateGodown()
        {
            return PartialView("CreateGodown");
        }

        [HttpGet]
        public ActionResult EditGodown(int GodownId)
        {
            DSRGodownVM ObjGodown = new DSRGodownVM();
            if (GodownId > 0)
            {
                DSR_MasterRepository ObjGR = new DSR_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (DSRGodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            DSRGodownVM ObjGodown = new DSRGodownVM();
            if (GodownId > 0)
            {
                DSR_MasterRepository ObjGR = new DSR_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (DSRGodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            DSR_MasterRepository ObjGR = new DSR_MasterRepository();
            ObjGR.GetAllGodown();
            List<DSRGodown> LstGodown = new List<DSRGodown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<DSRGodown>)ObjGR.DBResponse.Data;
            }
            return PartialView("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(DSRGodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRGodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DSRGodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjGR = new DSR_MasterRepository();
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
                DSR_MasterRepository ObjGR = new DSR_MasterRepository();
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

        #region Franchise
        [HttpGet]
        public ActionResult CreateFranchiseCharges()
        {
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;

            List<DSRPort> LstPort = new List<DSRPort>();
            objCR.GetAllPort();
            if (objCR.DBResponse.Data != null)
            {
                ViewBag.LstPort = (IList<DSRPort>)objCR.DBResponse.Data;
            }
            else
            {
                ViewBag.LstPort = null;
            }

            return PartialView();
        }

        [HttpGet]
        public ActionResult EditFranchise(int franchisechargeid)
        {
            DSRCWCFranchiseCharges objFC = new DSRCWCFranchiseCharges();
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            List<DSRPort> LstPort = new List<DSRPort>();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (DSRCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            objRepo.GetAllPort();
            if (objRepo.DBResponse.Data != null)
            {
                ViewBag.LstPort = (IList<DSRPort>)objRepo.DBResponse.Data;
            }
            else
            {
                ViewBag.LstPort = null;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(DSRCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository objRepository = new DSR_MasterRepository();
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
            IList<DSRCWCFranchiseCharges> objList = new List<DSRCWCFranchiseCharges>();
            DSR_MasterRepository objCR = new DSR_MasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<DSRCWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion


        #region Party Insurance
      
        [HttpGet]
        public ActionResult CreatePartyInsurance()
        {
            
            DSR_MasterRepository ObjIR = new DSR_MasterRepository();
            ObjIR.ListOfPartyForPage("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
               
            }
            return PartialView("CreatePartyInsurance");
        }

        [HttpGet]
        public ActionResult GetPartyInsuranceList()
        {
            DSR_MasterRepository ObjPR = new DSR_MasterRepository();
            List<DSRPartyInsurance> LstPartyInsurance = new List<DSRPartyInsurance>();
            ObjPR.GetAllPartyInsurance();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPartyInsurance = (List<DSRPartyInsurance>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPartyInsuranceList", LstPartyInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPartyInsuranceDetail(DSRPartyInsurance ObjParty)
        {
           // ObjParty.PartyInsuranceId = 0;
            if (ModelState.IsValid)
            {
                ObjParty.PartyId = ObjParty.PartyId;
                ObjParty.PartyName = ObjParty.PartyName.Trim();
                ObjParty.InsuranceFrom = ObjParty.InsuranceFrom.Trim();
                ObjParty.InsuranceTo = ObjParty.InsuranceTo.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjParty.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjParty.Uid = ObjLogin.Uid;
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.AddEditPartyInsurance(ObjParty);
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
        public ActionResult DeletePartyInsuranceDetail(int PartyInsuranceId)
        {
            if (PartyInsuranceId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.DeletePartyInsurance(PartyInsuranceId);
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditPartyInsurance(int PartyInsuranceId)
        {
            DSRPartyInsurance ObjPartyInsurance = new DSRPartyInsurance();
            ViewBag.Country = null;
            if (PartyInsuranceId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPartyInsurance(PartyInsuranceId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyInsurance = (DSRPartyInsurance)ObjPR.DBResponse.Data;
                }
               
            }
            return PartialView("EditPartyInsurance", ObjPartyInsurance);
        }

        [HttpGet]
        public ActionResult ViewPartyInsurance(int PartyInsuranceId)
        {
            DSRPartyInsurance ObjPartyInsurance = new DSRPartyInsurance();
            if (PartyInsuranceId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPartyInsurance(PartyInsuranceId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyInsurance = (DSRPartyInsurance)ObjPR.DBResponse.Data;
                }
            }
            return PartialView("ViewPartyInsurance", ObjPartyInsurance);
        }
        //[HttpGet]
        //public JsonResult SearchByPartyCode(string PartyId)
        //{
        //    DSR_MasterRepository objRepo = new DSR_MasterRepository();
        //    objRepo.ListOfPartyForPage(PartyId, 0);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult LoadPartyList(string PartyId, int Page)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.ListOfPartyForPage(PartyId, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
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
            DSR_MasterRepository ObjCh = new DSR_MasterRepository();
            DSRChemical ObjChem = new DSRChemical();
            if (ChemicalId > 0)
            {

                ObjCh.GetChemical(ChemicalId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (DSRChemical)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditChemical", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemical(int ChemicalId)
        {
            DSRChemical ObjYard = new DSRChemical();
            if (ChemicalId > 0)
            {
                DSR_MasterRepository ObjYR = new DSR_MasterRepository();
                ObjYR.GetChemical(ChemicalId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (DSRChemical)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemical", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalList()

        {
            DSR_MasterRepository ObjYR = new DSR_MasterRepository();
            ObjYR.GetAllChemical();
            List<DSRChemical> LstChemical = new List<DSRChemical>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<DSRChemical>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(DSRChemical ObjChem)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjCR = new DSR_MasterRepository();
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

        #region Bank

        [HttpGet]
        public ActionResult CreateBank()
        {            

            return PartialView("CreateBank");
        }


        [HttpGet]
        public ActionResult GetBankList()
        {
            List<DSRBank> LstBank = new List<DSRBank>();
            DSR_MasterRepository ObjBR = new DSR_MasterRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<DSRBank>)ObjBR.DBResponse.Data;
            }
            return PartialView("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            DSRBank ObjBank = new DSRBank();
            if (BankId > 0)
            {
                DSR_MasterRepository ObjBR = new DSR_MasterRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank = (DSRBank)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(DSRBank ObjBank)
        {
            //if (ModelState.IsValid)
            //{
                DSR_MasterRepository ObjBR = new DSR_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjBank.Uid = ObjLogin.Uid;
                ObjBank.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjBank.AccountNo = ObjBank.AccountNo.Trim();
                ObjBank.LedgerName = ObjBank.LedgerName.Trim();
                ObjBank.LedgerNo = ObjBank.LedgerNo;
                ObjBank.Address = ObjBank.Address == null ? null : ObjBank.Address.Trim();
                ObjBank.Branch = ObjBank.Branch.Trim();
                ObjBR.AddBank(ObjBank);
                //ModelState.Clear();
                return Json(ObjBR.DBResponse);
           // }
            //else
            //{
               // var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
               // var Err = new { Status = 0, Message = ErrorMessage };
               // return Json(Err);
           // }

        }

        public JsonResult ListOfLedger()
        {
            DSR_MasterRepository objbank = new DSR_MasterRepository();
            objbank.GetLedger();                  
            return Json(objbank.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Party Wise Reservation

        [HttpGet]
        public ActionResult PartyWiseReservation()
        {
            DSR_MasterRepository ObjIR = new DSR_MasterRepository();
            ObjIR.ListOfPartyForPage("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];

            }  
                      
            return PartialView("PartyWiseReservation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPartyWiseReservation(DSRPartyWiseReservation ObjParty)
        {
            if (ModelState.IsValid)
            {
                //return Json("");
                ObjParty.GodownId = ObjParty.GodownId;
                ObjParty.OperationType = ObjParty.OperationType.Trim();
                ObjParty.GodownName = ObjParty.GodownName.Trim();
                ObjParty.PartyId = ObjParty.PartyId;
                ObjParty.PartyName = ObjParty.PartyName.Trim();
                ObjParty.ReservationFrom = ObjParty.ReservationFrom.Trim();
                ObjParty.ReservationTo = ObjParty.ReservationTo.Trim();
                if(ObjParty.AreaType != null)
                {
                    ObjParty.AreaType = ObjParty.AreaType.Trim();                    
                }
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjParty.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjParty.Uid = ObjLogin.Uid;
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.AddEditPartyWiseReservation(ObjParty);
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

        [HttpGet]
        public ActionResult GetPartyWiseReservationList()
        {
            DSR_MasterRepository ObjPR = new DSR_MasterRepository();
            List<DSRPartyWiseReservation> LstPartyInsurance = new List<DSRPartyWiseReservation>();
            ObjPR.GetAllPartyWiseReservation();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPartyInsurance = (List<DSRPartyWiseReservation>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPartyWiseReservationList", LstPartyInsurance);
        }

        [HttpGet]
        public JsonResult LoadPartyWiseReservationList(string PartyId, int Page)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.ListOfPartyWiseReservationForPage(PartyId, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchByPartyReservationCode(string PartyCode)
        {
            DSR_MasterRepository objRepo = new DSR_MasterRepository();
            objRepo.SearchByPartyReservationCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewPartyReservation(int PartyReservationId)
        {
            DSRPartyWiseReservation ObjPartyReservation = new DSRPartyWiseReservation();
            if (PartyReservationId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPartyReservation(PartyReservationId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyReservation = (DSRPartyWiseReservation)ObjPR.DBResponse.Data;
                }

                ObjPR.ListOfPartyForPage("", 0);
                if (ObjPR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjPR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstImporter = Jobject["lstImporter"];

                }
            }
            return PartialView("ViewPartyReservation", ObjPartyReservation);
        }

        [HttpGet]
        public ActionResult EditPartyReservation(int PartyReservationId)
        {
            DSRPartyWiseReservation ObjPartyReservation = new DSRPartyWiseReservation();
            ViewBag.Country = null;
            if (PartyReservationId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.GetPartyReservation(PartyReservationId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyReservation = (DSRPartyWiseReservation)ObjPR.DBResponse.Data;                   
                    ViewBag.OperationType = ObjPartyReservation.OperationType;                                  
                }

                //ListOfGodownData(ObjPartyReservation.OperationType);

                //List<DSRReservationGodownList> objImp = new List<DSRReservationGodownList>();
                //if (ObjPR.DBResponse.Data != null) { 
                    //ViewBag.GodownList = ObjPR.DBResponse.Data;
                //}
                ObjPR.ListOfPartyForPage("", 0);
                if (ObjPR.DBResponse.Data != null)
                {
                    var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjPR.DBResponse.Data);
                    var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                    ViewBag.lstImporter = Jobject["lstImporter"];

                }

            }
            return PartialView("EditPartyReservation", ObjPartyReservation);
        }

        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeletePartyReservationDetail(int PartyReservationId)
        {
            if (PartyReservationId > 0)
            {
                DSR_MasterRepository ObjPR = new DSR_MasterRepository();
                ObjPR.DeletePartyReservation(PartyReservationId);
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public JsonResult ListOfGodownData(string OperationType)
        {
            DSR_MasterRepository objImport = new DSR_MasterRepository();
            objImport.GodownList(OperationType,((Login)(Session["LoginUser"])).Uid);
           
            List<DSRReservationGodownList> objImp = new List<DSRReservationGodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRReservationGodownList>)objImport.DBResponse.Data;
            
            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ChargeName
        public ActionResult CreateChargeName()
        {
            
            return PartialView("CreateChargeName");
        }

        [HttpGet]
        public ActionResult GetChargeNameList()
        {
            DSR_MasterRepository ObjCR = new DSR_MasterRepository();
            List<DSRChargeName> LstChargeName = new List<DSRChargeName>();
            ObjCR.GetAllChargeName();
            if (ObjCR.DBResponse.Data != null)
            {
                LstChargeName = (List<DSRChargeName>)ObjCR.DBResponse.Data;
            }
            return PartialView("ChargeNameList", LstChargeName);
        }

        [HttpGet]
        public ActionResult EditChargeName(int ChargeNameId)
        {
            DSR_MasterRepository ObjCR = new DSR_MasterRepository();
            DSRChargeName ObjChargeName = new DSRChargeName();
            if (ChargeNameId > 0)
            {
                ObjCR.GetChargeName(ChargeNameId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjChargeName = (DSRChargeName)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("EditChargeName", ObjChargeName);
        }

        [HttpGet]
        public ActionResult ViewChargeName(int ChargeNameId)
        {
            DSR_MasterRepository ObjCR = new DSR_MasterRepository();
            DSRChargeName ObjChargeName = new DSRChargeName();
            if (ChargeNameId > 0)
            {
                ObjCR.GetChargeName(ChargeNameId);
                if (ObjCR.DBResponse.Data != null)
                {
                    ObjChargeName = (DSRChargeName)ObjCR.DBResponse.Data;
                }
            }
            return PartialView("ViewChargeName", ObjChargeName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChargeNameDetail(DSRChargeName ObjChargeName)
        {
            if (ModelState.IsValid)
            {
                DSR_MasterRepository ObjCR = new DSR_MasterRepository();
                ObjChargeName.ChargeName = ObjChargeName.ChargeName.Trim();
                if(ObjChargeName.ChargeCode !=null)
                {
                    ObjChargeName.ChargeCode = ObjChargeName.ChargeCode.Trim();
                }
                
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjChargeName.Uid = ObjLogin.Uid;                
                ObjCR.AddEditChargeName(ObjChargeName);
                ModelState.Clear();
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = -1, Message = ErrorMessage };
                return Json(Err);
               
            }
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteChargeNameDetail(int ChargeNameId)
        {
            if (ChargeNameId > 0)
            {
                DSR_MasterRepository ObjCR = new DSR_MasterRepository();
                ObjCR.DeleteChargeName(ChargeNameId);
                return Json(ObjCR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }
    }
    #endregion
}
