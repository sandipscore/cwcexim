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
    public class WFLDMasterController : Controller
    {
        // GET: Master/WFLDMaster
        #region Misc Charges

        public ActionResult CreateWFLDCharges()
        {
            WFLD_RateRepository objCR = new WFLD_RateRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<WFLDSac>)objCR.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objCR.ListOfChargeName();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<WFLDMiscCharge>)objCR.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditEntryRateFees(WFLDStorageCharge objStorage)
        {
            if (ModelState.IsValid)
            {
                WFLD_RateRepository objRepository = new WFLD_RateRepository();
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
            List<WFLDStorageCharge> lstMiscRateFees = new List<WFLDStorageCharge>();
            WFLD_RateRepository objCR = new WFLD_RateRepository();
            objCR.GetAllMiscRateFees(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<WFLDStorageCharge>)objCR.DBResponse.Data;
            return PartialView("MiscRateFeesList", lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFees(int StorageChargeId)
        {
            WFLDStorageCharge objSC = new WFLDStorageCharge();
            WFLD_RateRepository objRepo = new WFLD_RateRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = new SelectList((List<WFLDSac>)objRepo.DBResponse.Data, "SACId", "SacCode");
            else
                ViewBag.ListOfSAC = null;
            objRepo.ListOfChargeName();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfChargeName = new SelectList((List<WFLDMiscCharge>)objRepo.DBResponse.Data, "ChargeId", "ChargesName");
            else
                ViewBag.ListOfChargeName = null;

            if (StorageChargeId > 0)
            {
                objRepo.GetAllMiscRateFees(StorageChargeId);
                if (objRepo.DBResponse.Data != null)
                    objSC = (WFLDStorageCharge)objRepo.DBResponse.Data;
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WHT_MasterRepository ObjCC = new WHT_MasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            WFLDCWCStorageCharge ObjStorageCharge = new WFLDCWCStorageCharge();
            WHT_MasterRepository objMR = new WHT_MasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (WFLDCWCStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            List<WFLDCWCStorageCharge> LstStorageCharges = new List<WFLDCWCStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<WFLDCWCStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(WFLDCWCStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WFLDMiscellaneous ObjMiscellaneous = new WFLDMiscellaneous();
            if (MiscellaneousId > 0)
            {
                WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                WHT_MasterRepository objRR = new WHT_MasterRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (WFLDMiscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
            List<WFLDMiscellaneous> LstMiscellaneous = new List<WFLDMiscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<WFLDMiscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(WFLDMiscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
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
            WHT_MasterRepository ObjETR = new WHT_MasterRepository();
            List<WHTEximTrader> LstEximTrader = new List<WHTEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<WHTEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            WHT_MasterRepository ObjCR = new WHT_MasterRepository();
            List<WHTEximTrader> LstCommodity = new List<WHTEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<WHTEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            WHT_MasterRepository ObjETR = new WHT_MasterRepository();
            List<WHTEximTrader> LstEximTrader = new List<WHTEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<WHTEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            WHTEximTrader ObjEximTrader = new WHTEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WHTEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            WHTEximTrader ObjEximTrader = new WHTEximTrader();
            if (EximTraderId > 0)
            {
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (WHTEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(WHTEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
             //   Login ObjLogin = (Login)Session["LoginUser"];
              //  ObjEximTrader.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjETR = new WHT_MasterRepository();
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
            WHT_MasterRepository objPort = new WHT_MasterRepository();
            WFLDHTCharges objHT = new WFLDHTCharges();
            List<WFLDPort> LstPort = new List<WFLDPort>();
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
                objHT.LstPort = (IList<WFLDPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabData(string Size, string ChargesFor,string OperationCode)
        {
            WHT_MasterRepository objHT = new WHT_MasterRepository();
            objHT.GetSlabData(Size, ChargesFor, OperationCode);
            WFLDHTCharges lstSlab = new WFLDHTCharges();
            if (objHT.DBResponse.Data != null)
                lstSlab = (WFLDHTCharges)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtl(int HTChargesID)
        {
            WHT_MasterRepository objHT = new WHT_MasterRepository();
            objHT.GetHTSlabChargesDtl(HTChargesID);
            List<WFLDChargeList> lstSlab = new List<WFLDChargeList>();
            if (objHT.DBResponse.Data != null)
                lstSlab = (List<WFLDChargeList>)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult ViewHTCharges(int HTChargesId)
        //{
        //    PPGHTCharges objHTCharges = new PPGHTCharges();
        //    if (HTChargesId > 0)
        //    {
        //        WHT_MasterRepository objHt = new WHT_MasterRepository();
        //        objHt.GetHTChargesDetails(HTChargesId);
        //        if (objHt.DBResponse.Data != null)
        //            objHTCharges = (PPGHTCharges)objHt.DBResponse.Data;
        //    }
        //    return PartialView(objHTCharges);
        //}
        [HttpGet]
        public ActionResult EditHTCharges(int HTChargesId)
        {

            WFLDHTCharges objHT = new WFLDHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<WFLDPort> LstPort = new List<WFLDPort>();
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
            WHT_MasterRepository objHTRepo = new WHT_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (WFLDHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<WFLDPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult EditHTChargesOther(int HTChargesId)
        {
            WFLDHTCharges objHT = new WFLDHTCharges();
            WHT_MasterRepository objHTRepo = new WHT_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            WFLDHTCharges objHT = new WFLDHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            WHT_MasterRepository objHTRepo = new WHT_MasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (WFLDHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<WFLDPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView("ViewHTCharges", objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTCharges(WFLDHTCharges objCharges, String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<WFLDChargeList> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDChargeList>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            WHT_MasterRepository objHTRepo = new WHT_MasterRepository();
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
            WHT_MasterRepository objHT = new WHT_MasterRepository();
            objHT.GetAllHTCharges();
            //IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
            WFLDHTCharges lstCharges = new WFLDHTCharges();
            if (objHT.DBResponse.Data != null)
                lstCharges = (WFLDHTCharges)objHT.DBResponse.Data;
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
            WHT_MasterRepository ObjPR = new WHT_MasterRepository();
            List<WFLDPort> LstPort = new List<WFLDPort>();
            ObjPR.GetAllPort();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<WFLDPort>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(WFLDPort ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias = ObjPort.PortAlias.Trim();
                ObjPort.PortName = ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
            WFLDPort ObjPort = new WFLDPort();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (WFLDPort)ObjPR.DBResponse.Data;
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
            WFLDPort ObjPort = new WFLDPort();
            if (PortId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (WFLDPort)ObjPR.DBResponse.Data;
                }
            }
            return View("ViewPort", ObjPort);
        }
        #endregion

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            List<WFLDInsurance> LstInsuarance = new List<WFLDInsurance>();
            WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<WFLDInsurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            WFLDInsurance ObjInsurance = new WFLDInsurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (WFLDInsurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(WFLDInsurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjCWCR = new WHT_MasterRepository();
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
            WHT_MasterRepository ObjSR = new WHT_MasterRepository();
            WFLDSac ObjSac = new WFLDSac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (WFLDSac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            WHT_MasterRepository ObjSR = new WHT_MasterRepository();
            List<WFLDSac> LstSac = new List<WFLDSac>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<WFLDSac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(WFLDSac ObjSac)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjSR = new WHT_MasterRepository();
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

            WHT_MasterRepository ObjRepo = new WHT_MasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            WFLDSearchEximTraderData obj = new WFLDSearchEximTraderData();
            ObjRepo.GetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((WFLDSearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((WFLDSearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new WFLDSDOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new WFLDReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
                  new SelectListItem { Text = "POS", Value = "POS"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            ViewBag.ServerDate = Utility.GetServerDate();
            ViewBag.curDate = DateTime.Today.ToString("dd/MM/yyyy");
            return PartialView("CreateSDopening", model);
        }

        [HttpGet]
        public ActionResult GetSDList()
        {
            WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
            List<WFLDSDOpening> LstSD = new List<WFLDSDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<WFLDSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult GetSDListPartyCode(string PartyCode)
        {
            WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
            List<WFLDSDOpening> LstSD = new List<WFLDSDOpening>();
            ObjSDR.GetSDListPartyCode(PartyCode);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<WFLDSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            WFLDSDOpening ObjSD = new WFLDSDOpening();
            if (SDId > 0)
            {
                WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (WFLDSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(WFLDSDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
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
        [HttpGet]
        public JsonResult LoadEximtradeList(string PartyCode, int Page)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.GetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchByPartyCode(string PartyCode)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
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

            WHT_MasterRepository ObjRepo = new WHT_MasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            WFLDSearchEximTraderData obj = new WFLDSearchEximTraderData();
            ObjRepo.OAGetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((WFLDSearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((WFLDSearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new WFLDOAOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new WFLDReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
            List<WFLDOAOpening> LstSD = new List<WFLDOAOpening>();
            ObjSDR.GetAllOAOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<WFLDOAOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("OnAccountList", LstSD);
        }

        [HttpGet]
        public ActionResult GetOAListPartyCode(string PartyCode)
        {
            WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
            List<WFLDOAOpening> LstSD = new List<WFLDOAOpening>();
            ObjSDR.GetOAListPartyCode(PartyCode);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<WFLDOAOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("OnAccountList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewOAOpening(int OnAcId)
        {
            WFLDOAOpening ObjSD = new WFLDOAOpening();
            if (OnAcId > 0)
            {
                WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
                ObjSDR.GetOAOpening(OnAcId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (WFLDOAOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewOAOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditOAopening(WFLDOAOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                WHT_MasterRepository ObjSDR = new WHT_MasterRepository();
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
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.OAGetEximTrader(PartyCode, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult OASearchByPartyCode(string PartyCode)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
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
            WFLDYardVM ObjYard = new WFLDYardVM();
            if (YardId > 0)
            {
                WHT_MasterRepository ObjYR = new WHT_MasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (WFLDYardVM)ObjYR.DBResponse.Data;
                    ObjYard.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjYard.LstYard);
                }
            }
            return PartialView("EditYard", ObjYard);
        }

        [HttpGet]
        public ActionResult ViewYard(int YardId)
        {
            WFLDYardVM ObjYard = new WFLDYardVM();
            if (YardId > 0)
            {
                WHT_MasterRepository ObjYR = new WHT_MasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (WFLDYardVM)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewYardWFLD", ObjYard);
        }

        [HttpGet]
        public ActionResult GetYardList()
        {
            WHT_MasterRepository ObjYR = new WHT_MasterRepository();
            ObjYR.GetAllYard();
            List<WFLDYard> LstYard = new List<WFLDYard>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstYard = (List<WFLDYard>)ObjYR.DBResponse.Data;
            }
            return PartialView("YardList", LstYard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditYardDetail(WFLDYardVM ObjYard)
        {
            var DelLocationXML = "";
            string LocationXML;
            if (ModelState.IsValid)
            {
                if (ObjYard.LocationDetail != null)
                {
                    ObjYard.LstYard = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDYardWiseLocation>>(ObjYard.LocationDetail);
                }
                if (ObjYard.DelLocationDetail != null)
                {
                    var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDYardWiseLocation>>(ObjYard.DelLocationDetail);
                    DelLocationXML = Utility.CreateXML(DelLocationList);
                }
                LocationXML = Utility.CreateXML(ObjYard.LstYard);
                WHT_MasterRepository ObjYR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjYR = new WHT_MasterRepository();
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
            WHT_MasterRepository objSR = new WHT_MasterRepository();
            WFLDOperation objOPR = new WFLDOperation();
            objSR.GetAllSac();
            if (objSR.DBResponse.Data != null)
                objOPR.LstSac = (List<WFLDSac>)objSR.DBResponse.Data;
            return PartialView("CreateOperation", objOPR);
        }

        [HttpGet]
        public ActionResult ViewOperation(int OperationId)
        {
            WFLDOperation ObjOperation = new WFLDOperation();
            if (OperationId > 0)
            {
                WHT_MasterRepository ObjOR = new WHT_MasterRepository();
                ObjOR.ViewMstOperation(OperationId);
                if (ObjOR.DBResponse.Data != null)
                {
                    ObjOperation = (WFLDOperation)ObjOR.DBResponse.Data;
                }
            }
            return PartialView("ViewOperation", ObjOperation);
        }

        [HttpGet]
        public ActionResult GetOperationList()
        {
            List<WFLDOperation> lstOperation = new List<WFLDOperation>();
            WHT_MasterRepository objOR = new WHT_MasterRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
                lstOperation = (List<WFLDOperation>)objOR.DBResponse.Data;
            return PartialView("OperationList", lstOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOperation(WFLDOperation ObjOperation)
        {
            if (ModelState.IsValid)
            {
                ObjOperation.ShortDescription = ObjOperation.ShortDescription == null ? null : ObjOperation.ShortDescription.Trim();
                ObjOperation.Description = ObjOperation.Description == null ? null : ObjOperation.Description.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjOperation.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjOR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            List<WFLDCWCEntryFees> lstEntryFees = new List<WFLDCWCEntryFees>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<WFLDCWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(WFLDCWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objRepository = new WHT_MasterRepository();
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
            WFLDCWCEntryFees objEF = new WFLDCWCEntryFees();
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (WFLDCWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            IList<WFLDCWCChargesGroundRent> objGR = new List<WFLDCWCChargesGroundRent>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<WFLDCWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(WFLDCWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export


            */
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            WFLDCWCChargesGroundRent objCGR = new WFLDCWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (WFLDCWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            IList<WFLDCWCReefer> objList = new List<WFLDCWCReefer>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<WFLDCWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(WFLDCWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            WFLDCWCReefer objRef = new WFLDCWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (WFLDCWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(WFLDCWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            List<WFLDCWCWeighment> lstWeighment = new List<WFLDCWCWeighment>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<WFLDCWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            WFLDCWCWeighment objCW = new WFLDCWCWeighment();
            WHT_MasterRepository objCCR = new WHT_MasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (WFLDCWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            IList<WFLDCWCTds> objList = new List<WFLDCWCTds>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<WFLDCWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(WFLDCWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            WFLDCWCTds objTds = new WFLDCWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (WFLDCWCTds)objCR.DBResponse.Data;
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
            WFLDGodownVM ObjGodown = new WFLDGodownVM();
            if (GodownId > 0)
            {
                WHT_MasterRepository ObjGR = new WHT_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (WFLDGodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            WFLDGodownVM ObjGodown = new WFLDGodownVM();
            if (GodownId > 0)
            {
                WHT_MasterRepository ObjGR = new WHT_MasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (WFLDGodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return PartialView("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            WHT_MasterRepository ObjGR = new WHT_MasterRepository();
            ObjGR.GetAllGodown();
            List<WFLDGodown> LstGodown = new List<WFLDGodown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<WFLDGodown>)ObjGR.DBResponse.Data;
            }
            return PartialView("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(WFLDGodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDGodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<WFLDGodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjGR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjGR = new WHT_MasterRepository();
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
            WHT_MasterRepository objCR = new WHT_MasterRepository();
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
            WFLDCWCFranchiseCharges objFC = new WFLDCWCFranchiseCharges();
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (WFLDCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(WFLDCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository objRepository = new WHT_MasterRepository();
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
            IList<WFLDCWCFranchiseCharges> objList = new List<WFLDCWCFranchiseCharges>();
            WHT_MasterRepository objCR = new WHT_MasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<WFLDCWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion


        #region Party Insurance
      
        [HttpGet]
        public ActionResult CreatePartyInsurance()
        {
            
            WHT_MasterRepository ObjIR = new WHT_MasterRepository();
            ObjIR.ListOfPartyForPartyInsurance("", 0);
            if (ObjIR.DBResponse.Data != null)
            {
                var Jobj = Newtonsoft.Json.JsonConvert.SerializeObject(ObjIR.DBResponse.Data);
                var Jobject = Newtonsoft.Json.Linq.JObject.Parse(Jobj);
                ViewBag.lstImporter = Jobject["lstImporter"];
                ViewBag.State = Jobject["State"];

            }
            return PartialView("CreatePartyInsurance");
        }

        [HttpGet]
        public ActionResult GetPartyInsuranceList()
        {
            WHT_MasterRepository ObjPR = new WHT_MasterRepository();
            List<WFLDPartyInsurance> LstPartyInsurance = new List<WFLDPartyInsurance>();
            ObjPR.GetAllPartyInsurance();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPartyInsurance = (List<WFLDPartyInsurance>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPartyInsuranceList", LstPartyInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPartyInsuranceDetail(WFLDPartyInsurance ObjParty)
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
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
            WFLDPartyInsurance ObjPartyInsurance = new WFLDPartyInsurance();
            ViewBag.Country = null;
            if (PartyInsuranceId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPartyInsurance(PartyInsuranceId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyInsurance = (WFLDPartyInsurance)ObjPR.DBResponse.Data;
                }
               
            }
            return View("EditPartyInsurance", ObjPartyInsurance);
        }

        [HttpGet]
        public ActionResult ViewPartyInsurance(int PartyInsuranceId)
        {
            WFLDPartyInsurance ObjPartyInsurance = new WFLDPartyInsurance();
            if (PartyInsuranceId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPartyInsurance(PartyInsuranceId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyInsurance = (WFLDPartyInsurance)ObjPR.DBResponse.Data;
                }
            }
            return View("ViewPartyInsurance", ObjPartyInsurance);
        }
        //[HttpGet]
        //public JsonResult SearchByPartyCode(string PartyId)
        //{
        //    WHT_MasterRepository objRepo = new WHT_MasterRepository();
        //    objRepo.ListOfPartyForPage(PartyId, 0);
        //    return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult LoadPartyList(string PartyId, int Page)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.ListOfPartyForPartyInsurance(PartyId, Page);
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
            WHT_MasterRepository ObjCh = new WHT_MasterRepository();
            WFLDChemical ObjChem = new WFLDChemical();
            if (ChemicalId > 0)
            {

                ObjCh.GetChemical(ChemicalId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (WFLDChemical)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditChemical", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemical(int ChemicalId)
        {
            WFLDChemical ObjYard = new WFLDChemical();
            if (ChemicalId > 0)
            {
                WHT_MasterRepository ObjYR = new WHT_MasterRepository();
                ObjYR.GetChemical(ChemicalId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (WFLDChemical)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemical", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalList()

        {
            WHT_MasterRepository ObjYR = new WHT_MasterRepository();
            ObjYR.GetAllChemical();
            List<WFLDChemical> LstChemical = new List<WFLDChemical>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<WFLDChemical>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(WFLDChemical ObjChem)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjCR = new WHT_MasterRepository();
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
            WHT_MasterRepository objbank = new WHT_MasterRepository();

            objbank.GetLedger();
            if (objbank.DBResponse.Status > 0)
                ViewBag.LdgNM = JsonConvert.SerializeObject(objbank.DBResponse.Data);
            else
                ViewBag.LdgNM = null;

            return PartialView("CreateBank");
        }

        [HttpGet]
        public ActionResult GetBankList()
        {
            List<WFLDBank> LstBank = new List<WFLDBank>();
            WHT_MasterRepository ObjBR = new WHT_MasterRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<WFLDBank>)ObjBR.DBResponse.Data;
            }
            return PartialView("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            WFLDBank ObjBank = new WFLDBank();
            if (BankId > 0)
            {
                WHT_MasterRepository ObjBR = new WHT_MasterRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank = (WFLDBank)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(WFLDBank ObjBank)
        {
            if (ModelState.IsValid)
            {
                WHT_MasterRepository ObjBR = new WHT_MasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjBank.Uid = ObjLogin.Uid;
                ObjBank.LedgerName = ObjBank.LedgerName.Trim();
                ObjBank.LedgerNo = ObjBank.LedgerNo;
                ObjBank.Address = ObjBank.Address == null ? null : ObjBank.Address.Trim();
                ObjBank.Branch = ObjBank.Branch.Trim();
                ObjBR.AddBank(ObjBank);
                ModelState.Clear();
                return Json(ObjBR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }

        }
        #endregion

        #region Party Wise Reservation

        [HttpGet]
        public ActionResult PartyWiseReservation()
        {
           WHT_MasterRepository ObjIR = new WHT_MasterRepository();
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
        public ActionResult AddEditPartyWiseReservation(WFLDPartyWiseReservation ObjParty)
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
                if (ObjParty.AreaType != null)
                {
                    ObjParty.AreaType = ObjParty.AreaType.Trim();
                }
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjParty.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjParty.Uid = ObjLogin.Uid;
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
            WHT_MasterRepository ObjPR = new WHT_MasterRepository();
            List<WFLDPartyWiseReservation> LstPartyInsurance = new List<WFLDPartyWiseReservation>();
            ObjPR.GetAllPartyWiseReservation();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPartyInsurance = (List<WFLDPartyWiseReservation>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPartyWiseReservationList", LstPartyInsurance);
        }

        [HttpGet]
        public JsonResult LoadPartyWiseReservationList(string PartyId, int Page)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.ListOfPartyWiseReservationForPage(PartyId, Page);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
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
    

    [HttpGet]
        public JsonResult SearchByPartyReservationCode(string PartyCode)
        {
            WHT_MasterRepository objRepo = new WHT_MasterRepository();
            objRepo.SearchByPartyReservationCode(PartyCode, 0);
            return Json(objRepo.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewPartyReservation(int PartyReservationId)
        {
            WFLDPartyWiseReservation ObjPartyReservation = new WFLDPartyWiseReservation();
            if (PartyReservationId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPartyReservation(PartyReservationId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyReservation = (WFLDPartyWiseReservation)ObjPR.DBResponse.Data;
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
            WFLDPartyWiseReservation ObjPartyReservation = new WFLDPartyWiseReservation();
            ViewBag.Country = null;
            if (PartyReservationId > 0)
            {
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
                ObjPR.GetPartyReservation(PartyReservationId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPartyReservation = (WFLDPartyWiseReservation)ObjPR.DBResponse.Data;
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
                WHT_MasterRepository ObjPR = new WHT_MasterRepository();
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
            WHT_MasterRepository objImport = new WHT_MasterRepository();
            objImport.GodownList(OperationType, ((Login)(Session["LoginUser"])).Uid);

            List<DSRReservationGodownList> objImp = new List<DSRReservationGodownList>();
            if (objImport.DBResponse.Data != null)
                objImp = (List<DSRReservationGodownList>)objImport.DBResponse.Data;

            return Json(objImp, JsonRequestBehavior.AllowGet);
        }

        #endregion





    }
}