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
    public class DNODEMasterController : Controller
    {
        // GET: Master/CHNMaster
        #region Misc Charges

        public ActionResult CreateDNDCharges()
        {
            DNODE_RateRepository objCR = new DNODE_RateRepository();
            objCR.ListOfSACCode();
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
        public JsonResult AddEditEntryRateFees(DNDMiscCharge objStorage)
        {
            if (ModelState.IsValid)
            {
                DNODE_RateRepository objRepository = new DNODE_RateRepository();
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
            List<DNDMiscCharge> lstMiscRateFees = new List<DNDMiscCharge>();
            DNODE_RateRepository objCR = new DNODE_RateRepository();
            objCR.GetAllMiscRateFees(0);
            if (objCR.DBResponse.Data != null)
                lstMiscRateFees = (List<DNDMiscCharge>)objCR.DBResponse.Data;
            return PartialView("MiscRateFeesList", lstMiscRateFees);
        }
        [HttpGet]
        public ActionResult EditMiscRateFees(int StorageChargeId)
        {
            DNDMiscCharge objSC = new DNDMiscCharge();
            DNODE_RateRepository objRepo = new DNODE_RateRepository();
            objRepo.ListOfSACCode();
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
                    objSC = (DNDMiscCharge)objRepo.DBResponse.Data;
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
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            DNODEMasterRepository ObjCC = new DNODEMasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            DNDCWCStorageCharge ObjStorageCharge = new DNDCWCStorageCharge();
            DNODEMasterRepository objMR = new DNODEMasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (DNDCWCStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            DNODEMasterRepository ObjCR = new DNODEMasterRepository();
            List<DNDCWCStorageCharge> LstStorageCharges = new List<DNDCWCStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<DNDCWCStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(DNDCWCStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository ObjCR = new DNODEMasterRepository();
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
            CHNMasterRepository objCR = new CHNMasterRepository();
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
                CHNMasterRepository ObjCWCR = new CHNMasterRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                CHNMasterRepository objRR = new CHNMasterRepository();
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
            CHNMasterRepository ObjCWCR = new CHNMasterRepository();
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
                CHNMasterRepository ObjCWCR = new CHNMasterRepository();
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
            CHNMasterRepository ObjETR = new CHNMasterRepository();
            List<CHNEximTrader> LstEximTrader = new List<CHNEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<CHNEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            CHNMasterRepository ObjCR = new CHNMasterRepository();
            List<CHNEximTrader> LstCommodity = new List<CHNEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<CHNEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            CHNMasterRepository ObjETR = new CHNMasterRepository();
            List<CHNEximTrader> LstEximTrader = new List<CHNEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<CHNEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            CHNEximTrader ObjEximTrader = new CHNEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                CHNMasterRepository ObjETR = new CHNMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (CHNEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            CHNEximTrader ObjEximTrader = new CHNEximTrader();
            if (EximTraderId > 0)
            {
                CHNMasterRepository ObjETR = new CHNMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (CHNEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(CHNEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
             //   Login ObjLogin = (Login)Session["LoginUser"];
              //  ObjEximTrader.Uid = ObjLogin.Uid;
                CHNMasterRepository ObjETR = new CHNMasterRepository();
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
                CHNMasterRepository ObjETR = new CHNMasterRepository();
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
            DNODEMasterRepository objPort = new DNODEMasterRepository();
            DNDHTCharges objHT = new DNDHTCharges();
            List<DNDPort> LstPort = new List<DNDPort>();
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
                objHT.LstPort = (IList<DNDPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabData(string Size, string ChargesFor,string OperationCode)
        {
            DNODEMasterRepository objHT = new DNODEMasterRepository();
            objHT.GetSlabData(Size, ChargesFor, OperationCode);
            DNDHTCharges lstSlab = new DNDHTCharges();
            if (objHT.DBResponse.Data != null)
                lstSlab = (DNDHTCharges)objHT.DBResponse.Data;
            return Json(lstSlab, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtl(int HTChargesID)
        {
            DNODEMasterRepository objHT = new DNODEMasterRepository();
            objHT.GetHTSlabChargesDtl(HTChargesID);
            List<DNDChargeList> lstSlab = new List<DNDChargeList>();
            if (objHT.DBResponse.Data != null)
                lstSlab = (List<DNDChargeList>)objHT.DBResponse.Data;
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

            DNDHTCharges objHT = new DNDHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<DNDPort> LstPort = new List<DNDPort>();
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
            DNODEMasterRepository objHTRepo = new DNODEMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (DNDHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<DNDPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult EditHTChargesOther(int HTChargesId)
        {
            DNDHTCharges objHT = new DNDHTCharges();
            DNODEMasterRepository objHTRepo = new DNODEMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            DNDHTCharges objHT = new DNDHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            DNODEMasterRepository objHTRepo = new DNODEMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (DNDHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<DNDPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView("ViewHTCharges", objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTCharges(DNDHTCharges objCharges, String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<DNDChargeList> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<DNDChargeList>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            DNODEMasterRepository objHTRepo = new DNODEMasterRepository();
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
            DNODEMasterRepository objHT = new DNODEMasterRepository();
            objHT.GetAllHTCharges();
            //IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
            DNDHTCharges lstCharges = new DNDHTCharges();
            if (objHT.DBResponse.Data != null)
                lstCharges = (DNDHTCharges)objHT.DBResponse.Data;
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

        //[HttpGet]
        //public ActionResult GetPortList()
        //{
        //    DNODEMasterRepository ObjPR = new DNODEMasterRepository();
        //    List<DNDPort> LstPort = new List<DNDPort>();
        //    ObjPR.GetAllPort();
        //    if (ObjPR.DBResponse.Data != null)
        //    {
        //        LstPort = (List<DNDPort>)ObjPR.DBResponse.Data;
        //    }
        //    return PartialView("GetPortList", LstPort);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(DNDPort ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias = ObjPort.PortAlias.Trim();
                ObjPort.PortName = ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid = ObjLogin.Uid;
                DNODEMasterRepository ObjPR = new DNODEMasterRepository();
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
                DNODEMasterRepository ObjPR = new DNODEMasterRepository();
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
            DNDPort ObjPort = new DNDPort();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                DNODEMasterRepository ObjPR = new DNODEMasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (DNDPort)ObjPR.DBResponse.Data;
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
            DNDPort ObjPort = new DNDPort();
            if (PortId > 0)
            {
                DNODEMasterRepository ObjPR = new DNODEMasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (DNDPort)ObjPR.DBResponse.Data;
                }
            }
            return PartialView("ViewPort", ObjPort);
        }
        [HttpGet]
        public ActionResult GetPortCodeListPortCode(string PortCode)
        {
            DNODEMasterRepository ObjETR = new DNODEMasterRepository();
            List<DNDPort> LstPort = new List<DNDPort>();
            ObjETR.GetAllPortCode(PortCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<DNDPort>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        public ActionResult GetPortList()
        {
            DNODEMasterRepository ObjETR = new DNODEMasterRepository();
            List<DNDPort> LstPort = new List<DNDPort>();
            // ObjETR.GetAllEximTraderListPageWise(0);
            ObjETR.GetAllPortList(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstPort = (List<DNDPort>)ObjETR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        [HttpGet]
        public JsonResult LoadPortMoreListData(int Page)
        {
            DNODEMasterRepository ObjCR = new DNODEMasterRepository();
            List<DNDPort> LstPort = new List<DNDPort>();
            ObjCR.GetAllPortList(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstPort = (List<DNDPort>)ObjCR.DBResponse.Data;
            }
            return Json(LstPort, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public ActionResult GetEximTraderListPartyCode(string PartyCode, int TypeOfPartyId)
        //{
        //    DNODEMasterRepository ObjETR = new DNODEMasterRepository();
        //    List<DndEximTrader> LstEximTrader = new List<DndEximTrader>();
        //    ObjETR.GetGetAllEximTraderPartyCode(PartyCode, TypeOfPartyId);
        //    if (ObjETR.DBResponse.Data != null)
        //    {
        //        LstEximTrader = (List<DndEximTrader>)ObjETR.DBResponse.Data;
        //    }
        //    return PartialView("EximTraderList", LstEximTrader);
        //}
        #endregion

        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            List<DNDInsurance> LstInsuarance = new List<DNDInsurance>();
            DNODEMasterRepository ObjCWCR = new DNODEMasterRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<DNDInsurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            DNODEMasterRepository ObjCWCR = new DNODEMasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            DNDInsurance ObjInsurance = new DNDInsurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (DNDInsurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(DNDInsurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository ObjCWCR = new DNODEMasterRepository();
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
            CHNMasterRepository ObjSR = new CHNMasterRepository();
            CHNSac ObjSac = new CHNSac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (CHNSac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            CHNMasterRepository ObjSR = new CHNMasterRepository();
            List<CHNSac> LstSac = new List<CHNSac>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<CHNSac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(CHNSac ObjSac)
        {
            if (ModelState.IsValid)
            {
                CHNMasterRepository ObjSR = new CHNMasterRepository();
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

            PPGMasterRepository ObjRepo = new PPGMasterRepository();
            /*SDOpening ObjSD = new SDOpening();*/
            SearchEximTraderData obj = new SearchEximTraderData();
            ObjRepo.GetEximTrader("", 0);
            if (ObjRepo.DBResponse.Data != null)
            {
                ViewBag.lstExim = ((SearchEximTraderData)ObjRepo.DBResponse.Data).lstExim;
                ViewBag.State = ((SearchEximTraderData)ObjRepo.DBResponse.Data).State;
            }



            var model = new SDOpening();
            for (var i = 0; i < 5; i++)
            {
                model.Details.Add(new PPGReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            return PartialView("CreateSDopening", model);

        }

        [HttpGet]
        public ActionResult GetSDList()
        {
            PPGMasterRepository ObjSDR = new PPGMasterRepository();
            List<SDOpening> LstSD = new List<SDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<SDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }
        [HttpGet]
        public ActionResult EditSDOpening(int SDId)
        {
            CHNSDOpening ObjSD = new CHNSDOpening();
            CHNMasterRepository ObjSDR = new CHNMasterRepository();
            ObjSDR.GetEximTrader();
            if (ObjSDR.DBResponse.Data != null)
            {
                ObjSD.LstEximTrader = (List<CHNSDOpening>)ObjSDR.DBResponse.Data;
            }
            if (SDId > 0)
            {
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (CHNSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("EditSDOpening", ObjSD);
        }
        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            SDOpening ObjSD = new SDOpening();
            if (SDId > 0)
            {
                PPGMasterRepository ObjSDR = new PPGMasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (SDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(SDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                PPGMasterRepository ObjSDR = new PPGMasterRepository();
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

        #region YARD
        public ActionResult CreateYard()
        {
            return PartialView("CreateYard");
        }

        [HttpGet]
        public ActionResult EditYard(int YardId)
        {
            CHNYardVM ObjYard = new CHNYardVM();
            if (YardId > 0)
            {
                CHNMasterRepository ObjYR = new CHNMasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (CHNYardVM)ObjYR.DBResponse.Data;
                    ObjYard.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjYard.LstYard);
                }
            }
            return PartialView("EditYard", ObjYard);
        }

        [HttpGet]
        public ActionResult ViewYard(int YardId)
        {
            CHNYardVM ObjYard = new CHNYardVM();
            if (YardId > 0)
            {
                CHNMasterRepository ObjYR = new CHNMasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (CHNYardVM)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewYardCHN", ObjYard);
        }

        [HttpGet]
        public ActionResult GetYardList()
        {
            CHNMasterRepository ObjYR = new CHNMasterRepository();
            ObjYR.GetAllYard();
            List<CHNYard> LstYard = new List<CHNYard>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstYard = (List<CHNYard>)ObjYR.DBResponse.Data;
            }
            return PartialView("YardList", LstYard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditYardDetail(CHNYardVM ObjYard)
        {
            var DelLocationXML = "";
            string LocationXML;
            if (ModelState.IsValid)
            {
                if (ObjYard.LocationDetail != null)
                {
                    ObjYard.LstYard = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHNYardWiseLocation>>(ObjYard.LocationDetail);
                }
                if (ObjYard.DelLocationDetail != null)
                {
                    var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHNYardWiseLocation>>(ObjYard.DelLocationDetail);
                    DelLocationXML = Utility.CreateXML(DelLocationList);
                }
                LocationXML = Utility.CreateXML(ObjYard.LstYard);
                CHNMasterRepository ObjYR = new CHNMasterRepository();
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
                CHNMasterRepository ObjYR = new CHNMasterRepository();
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
            CHNMasterRepository objSR = new CHNMasterRepository();
            CHNOperation objOPR = new CHNOperation();
            objSR.GetAllSac();
            if (objSR.DBResponse.Data != null)
                objOPR.LstSac = (List<CHNSac>)objSR.DBResponse.Data;
            return PartialView("CreateOperation", objOPR);
        }

        [HttpGet]
        public ActionResult ViewOperation(int OperationId)
        {
            CHNOperation ObjOperation = new CHNOperation();
            if (OperationId > 0)
            {
                CHNMasterRepository ObjOR = new CHNMasterRepository();
                ObjOR.ViewMstOperation(OperationId);
                if (ObjOR.DBResponse.Data != null)
                {
                    ObjOperation = (CHNOperation)ObjOR.DBResponse.Data;
                }
            }
            return PartialView("ViewOperation", ObjOperation);
        }

        [HttpGet]
        public ActionResult GetOperationList()
        {
            List<CHNOperation> lstOperation = new List<CHNOperation>();
            CHNMasterRepository objOR = new CHNMasterRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
                lstOperation = (List<CHNOperation>)objOR.DBResponse.Data;
            return PartialView("OperationList", lstOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOperation(CHNOperation ObjOperation)
        {
            if (ModelState.IsValid)
            {
                ObjOperation.ShortDescription = ObjOperation.ShortDescription == null ? null : ObjOperation.ShortDescription.Trim();
                ObjOperation.Description = ObjOperation.Description == null ? null : ObjOperation.Description.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjOperation.Uid = ObjLogin.Uid;
                CHNMasterRepository ObjOR = new CHNMasterRepository();
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
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            List<DNODECWCEntryFees> lstEntryFees = new List<DNODECWCEntryFees>();
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<DNODECWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(DNODECWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository objRepository = new DNODEMasterRepository();
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
            DNODECWCEntryFees objEF = new DNODECWCEntryFees();
            DNODEMasterRepository objRepo = new DNODEMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (DNODECWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion

        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            IList<DNDCWCChargesGroundRent> objGR = new List<DNDCWCChargesGroundRent>();
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<DNDCWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(DNDCWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export
            */
            if (ModelState.IsValid)
            {
                DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            DNDCWCChargesGroundRent objCGR = new DNDCWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (DNDCWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion

        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            IList<DNDCWCReefer> objList = new List<DNDCWCReefer>();
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<DNDCWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(DNDCWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            DNDCWCReefer objRef = new DNDCWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (DNDCWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion

        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(DNDCWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            List<DNDCWCWeighment> lstWeighment = new List<DNDCWCWeighment>();
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<DNDCWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            DNDCWCWeighment objCW = new DNDCWCWeighment();
            DNODEMasterRepository objCCR = new DNODEMasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (DNDCWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion

        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            CHNMasterRepository objCR = new CHNMasterRepository();
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
            IList<CHNCWCTds> objList = new List<CHNCWCTds>();
            CHNMasterRepository objCR = new CHNMasterRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<CHNCWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(CHNCWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                CHNMasterRepository objCR = new CHNMasterRepository();
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
            CHNMasterRepository objCR = new CHNMasterRepository();
            CHNCWCTds objTds = new CHNCWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (CHNCWCTds)objCR.DBResponse.Data;
            }
            return PartialView(objTds);
        }
        #endregion

        #region Godown
        [HttpGet]
        public ActionResult CreateGodown()
        {
            return View("CreateGodown");
        }

        [HttpGet]
        public ActionResult EditGodown(int GodownId)
        {
            CHNGodownVM ObjGodown = new CHNGodownVM();
            if (GodownId > 0)
            {
                CHNMasterRepository ObjGR = new CHNMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (CHNGodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            CHNGodownVM ObjGodown = new CHNGodownVM();
            if (GodownId > 0)
            {
                CHNMasterRepository ObjGR = new CHNMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (CHNGodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            CHNMasterRepository ObjGR = new CHNMasterRepository();
            ObjGR.GetAllGodown();
            List<CHNGodown> LstGodown = new List<CHNGodown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<CHNGodown>)ObjGR.DBResponse.Data;
            }
            return View("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(CHNGodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHNGodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<CHNGodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
                CHNMasterRepository ObjGR = new CHNMasterRepository();
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
                CHNMasterRepository ObjGR = new CHNMasterRepository();
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
            DNODEMasterRepository objCR = new DNODEMasterRepository();
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
            DNDCWCFranchiseCharges objFC = new DNDCWCFranchiseCharges();
            DNODEMasterRepository objRepo = new DNODEMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (DNDCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(DNDCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                DNODEMasterRepository objRepository = new DNODEMasterRepository();
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
            IList<DNDCWCFranchiseCharges> objList = new List<DNDCWCFranchiseCharges>();
            DNODEMasterRepository objCR = new DNODEMasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<DNDCWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
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
            CHNMasterRepository ObjCh = new CHNMasterRepository();
            CHNChemical ObjChem = new CHNChemical();
            if (ChemicalId > 0)
            {

                ObjCh.GetChemical(ChemicalId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (CHNChemical)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditChemical", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemical(int ChemicalId)
        {
            CHNChemical ObjYard = new CHNChemical();
            if (ChemicalId > 0)
            {
                CHNMasterRepository ObjYR = new CHNMasterRepository();
                ObjYR.GetChemical(ChemicalId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (CHNChemical)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemical", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalList()

        {
            CHNMasterRepository ObjYR = new CHNMasterRepository();
            ObjYR.GetAllChemical();
            List<CHNChemical> LstChemical = new List<CHNChemical>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<CHNChemical>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(CHNChemical ObjChem)
        {
            if (ModelState.IsValid)
            {
                CHNMasterRepository ObjCR = new CHNMasterRepository();
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
    }
}