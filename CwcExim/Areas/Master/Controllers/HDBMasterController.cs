using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Areas.Master.Models;
using CwcExim.Controllers;
using CwcExim.Models;
using Newtonsoft.Json;
using CwcExim.Filters;
using CwcExim.UtilityClasses;

namespace CwcExim.Areas.Master.Controllers
{
    public class HDBMasterController : Controller
    {
        // GET: Master/HDBMaster
        public ActionResult Index()
        {
            return View();
        }
        #region Bank/Cash
        [HttpGet]
        public ActionResult CreateBank()
        {
           HDBMasterRepository objbank = new HDBMasterRepository();

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
            List<HDBBank> LstBank = new List<HDBBank>();
            HDBMasterRepository ObjBR = new HDBMasterRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<HDBBank>)ObjBR.DBResponse.Data;
            }
            return PartialView("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            HDBBank ObjBank = new HDBBank();
            if (BankId > 0)
            {
                HDBMasterRepository ObjBR = new HDBMasterRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank = (HDBBank)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(HDBBank ObjBank)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjBR = new HDBMasterRepository();
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
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            HDBMasterRepository ObjCC = new HDBMasterRepository();
            ObjCC.ListOfSACCode();
            if (ObjCC.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCC.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            HDBStorageCharge ObjStorageCharge = new HDBStorageCharge();
            HDBMasterRepository objMR = new HDBMasterRepository();
            if (StorageChargeId > 0)
            {
                objMR.GetStorageCharge(StorageChargeId);
                if (objMR.DBResponse.Data != null)
                {
                    ObjStorageCharge = (HDBStorageCharge)objMR.DBResponse.Data;
                }
            }
            return PartialView("EditStorageCharge", ObjStorageCharge);
        }

        [HttpGet]
        public ActionResult StorageChargeList()
        {
            HDBMasterRepository ObjCR = new HDBMasterRepository();
            List<HDBStorageCharge> LstStorageCharges = new List<HDBStorageCharge>();
            ObjCR.GetAllStorageCharge();
            if (ObjCR.DBResponse.Data != null)
            {
                LstStorageCharges = (List<HDBStorageCharge>)ObjCR.DBResponse.Data;
            }
            return PartialView("StorageChargeList", LstStorageCharges);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEditStorageCharge(HDBStorageCharge ObjStorageCharge)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjCR = new HDBMasterRepository();
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
           HDBMasterRepository objCR = new HDBMasterRepository();
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
            HDBMiscellaneous ObjMiscellaneous = new HDBMiscellaneous();
            if (MiscellaneousId > 0)
            {
                HDBMasterRepository ObjCWCR = new HDBMasterRepository();
                ObjCWCR.ListOfSACCode();
                if (ObjCWCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                HDBMasterRepository objRR = new HDBMasterRepository();
                objRR.GetMiscellaneous(MiscellaneousId);
                if (objRR.DBResponse.Data != null)
                {
                    ObjMiscellaneous = (HDBMiscellaneous)objRR.DBResponse.Data;
                }
            }
            return PartialView(ObjMiscellaneous);
        }

        [HttpGet]
        public ActionResult GetMiscellaneousList()
        {
            HDBMasterRepository ObjCWCR = new HDBMasterRepository();
            List<HDBMiscellaneous> LstMiscellaneous = new List<HDBMiscellaneous>();
            ObjCWCR.GetAllMiscellaneous();
            if (ObjCWCR.DBResponse.Data != null)
            {
                LstMiscellaneous = (List<HDBMiscellaneous>)ObjCWCR.DBResponse.Data;
            }
            return PartialView("MiscellaneousList", LstMiscellaneous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditMiscellaneousDetail(HDBMiscellaneous ObjMiscellaneous)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjCWCR = new HDBMasterRepository();
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
        #region Exim Trader Master

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


        public ActionResult GetEximTraderList()
        {
            HDBMasterRepository ObjETR = new HDBMasterRepository();
            List<HDBEximTrader> LstEximTrader = new List<HDBEximTrader>();
            ObjETR.GetAllEximTraderListPageWise(0);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<HDBEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }

        [HttpGet]
        public JsonResult LoadEximTraderMoreListData(int Page)
        {
            HDBMasterRepository ObjCR = new HDBMasterRepository();
            List<HDBEximTrader> LstCommodity = new List<HDBEximTrader>();
            ObjCR.GetAllEximTraderListPageWise(Page);
            if (ObjCR.DBResponse.Data != null)
            {
                LstCommodity = (List<HDBEximTrader>)ObjCR.DBResponse.Data;
            }
            return Json(LstCommodity, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetEximTraderListPartyCode(string PartyCode)
        {
            HDBMasterRepository ObjETR = new HDBMasterRepository();
            List<HDBEximTrader> LstEximTrader = new List<HDBEximTrader>();
            ObjETR.GetGetAllEximTraderPartyCode(PartyCode);
            if (ObjETR.DBResponse.Data != null)
            {
                LstEximTrader = (List<HDBEximTrader>)ObjETR.DBResponse.Data;
            }
            return PartialView("EximTraderList", LstEximTrader);
        }
        [HttpGet]
        public ActionResult EditEximTrader(int EximTraderId)
        {
            HDBEximTrader ObjEximTrader = new HDBEximTrader();
            ViewBag.Country = null;
            CountryRepository ObjCR = new CountryRepository();
            ObjCR.GetAllCountry();
            if (ObjCR.DBResponse.Data != null)
            {
                ViewBag.Country = ObjCR.DBResponse.Data;
            }
            if (EximTraderId > 0)
            {
                HDBMasterRepository ObjETR = new HDBMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (HDBEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("EditEximTrader", ObjEximTrader);
        }

        [HttpGet]
        public ActionResult ViewEximTrader(int EximTraderId)
        {
            HDBEximTrader ObjEximTrader = new HDBEximTrader();
            if (EximTraderId > 0)
            {
                HDBMasterRepository ObjETR = new HDBMasterRepository();
                ObjETR.GetEximTrader(EximTraderId);
                if (ObjETR.DBResponse.Data != null)
                {
                    ObjEximTrader = (HDBEximTrader)ObjETR.DBResponse.Data;
                }
            }
            return PartialView("ViewEximTrader", ObjEximTrader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditEximTraderDetail(HDBEximTrader ObjEximTrader)
        {
            if (ModelState.IsValid)
            {
                ObjEximTrader.EximTraderName = ObjEximTrader.EximTraderName.Trim();
                ObjEximTrader.Address = ObjEximTrader.Address == null ? null : ObjEximTrader.Address.Trim();
                ObjEximTrader.ContactPerson = ObjEximTrader.ContactPerson == null ? null : ObjEximTrader.ContactPerson.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjEximTrader.Uid = ObjLogin.Uid;
                HDBMasterRepository ObjETR = new HDBMasterRepository();
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
                HDBMasterRepository ObjETR = new HDBMasterRepository();
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
            HDBMasterRepository objPort = new HDBMasterRepository();
            HDBHTCharges objHT = new HDBHTCharges();
            List<HDBPort> LstPort = new List<HDBPort>();
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
                objHT.LstPort = (IList<HDBPort>)objPort.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult GetSlabData(string Size, string ChargesFor)
        {
            HDBMasterRepository objHT = new HDBMasterRepository();
            objHT.GetSlabData(Size, ChargesFor);
            HDBHTCharges lstSlab = new HDBHTCharges();
            if (objHT.DBResponse.Data != null)
                lstSlab = (HDBHTCharges)objHT.DBResponse.Data;
            return Json(lstSlab,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHTSlabChargesDtl(int HTChargesID)
        {
           HDBMasterRepository objHT = new HDBMasterRepository();
           objHT.GetHTSlabChargesDtl(HTChargesID);
           List<ChargeList> lstSlab = new List<ChargeList>();
           if (objHT.DBResponse.Data != null)
                lstSlab = (List<ChargeList>)objHT.DBResponse.Data;
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
            
            HDBHTCharges objHT = new HDBHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            List<HDBPort> LstPort = new List<HDBPort>();
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
            HDBMasterRepository objHTRepo = new HDBMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (HDBHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<HDBPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView(objHT);
        }
        [HttpGet]
        public JsonResult EditHTChargesOther(int HTChargesId)
        {
            HDBHTCharges objHT = new HDBHTCharges();
            HDBMasterRepository objHTRepo = new HDBMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            return Json(objHTRepo.DBResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewHTCharges(int HTChargesId)
        {
            HDBHTCharges objHT = new HDBHTCharges();
            ContractorRepository objCR = new ContractorRepository();
            OperationRepository objOR = new OperationRepository();
            HDBMasterRepository objHTRepo = new HDBMasterRepository();
            objHTRepo.GetHTChargesDetails(HTChargesId);
            if (objHTRepo.DBResponse.Data != null)
            {
                objHT = (HDBHTCharges)objHTRepo.DBResponse.Data;
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
                objHT.LstPort = (IList<HDBPort>)objHTRepo.DBResponse.Data;
            }
            return PartialView("ViewHTCharges", objHT);
        }

        [HttpPost]
        public JsonResult AddEditHTCharges(HDBHTCharges objCharges,String ChargeList)
        {
            string ChargeListXML = "";
            if (ChargeList != null)
            {
                IList<ChargeList> LstCharge = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ChargeList>>(ChargeList);
                ChargeListXML = Utility.CreateXML(LstCharge);
            }

            HDBMasterRepository objHTRepo = new HDBMasterRepository();
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
            HDBMasterRepository objHT = new HDBMasterRepository();
            objHT.GetAllHTCharges();
            //IList<HDBHTCharges> lstCharges = new List<HDBHTCharges>();
            HDBHTCharges lstCharges = new HDBHTCharges();
            if (objHT.DBResponse.Data != null)
                lstCharges = (HDBHTCharges)objHT.DBResponse.Data;
            return Json(lstCharges,JsonRequestBehavior.AllowGet);
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
           HDBMasterRepository ObjPR = new HDBMasterRepository();
            List<HDBPort> LstPort = new List<HDBPort>();
            ObjPR.GetAllPort();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<HDBPort>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetPortList", LstPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditPortDetail(HDBPort ObjPort)
        {
            if (ModelState.IsValid)
            {
                ObjPort.PortAlias = ObjPort.PortAlias.Trim();
                ObjPort.PortName = ObjPort.PortName.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjPort.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjPort.Uid = ObjLogin.Uid;
                HDBMasterRepository ObjPR = new HDBMasterRepository();
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
                HDBMasterRepository ObjPR = new HDBMasterRepository();
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
            HDBPort ObjPort = new HDBPort();
            ViewBag.Country = null;
            if (PortId > 0)
            {
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (HDBPort)ObjPR.DBResponse.Data;
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
            HDBPort ObjPort = new HDBPort();
            if (PortId > 0)
            {
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.GetPort(PortId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjPort = (HDBPort)ObjPR.DBResponse.Data;
                }
            }
            return View("ViewPort", ObjPort);
        }
        #endregion
        #region Insurance
        [HttpGet]
        public ActionResult CreateInsurance()
        {
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            List<HDBInsurance> LstInsuarance = new List<HDBInsurance>();
            HDBMasterRepository ObjCWCR = new HDBMasterRepository();
            ObjCWCR.GetAllInsurance();
            if (ObjCWCR.DBResponse.Data != null)
                LstInsuarance = (List<HDBInsurance>)ObjCWCR.DBResponse.Data;
            return PartialView(LstInsuarance);
        }

        [HttpGet]
        public ActionResult EditInsurance(int InsuranceId)
        {
            HDBMasterRepository ObjCWCR = new HDBMasterRepository();
            ObjCWCR.ListOfSACCode();
            if (ObjCWCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = ObjCWCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            HDBInsurance ObjInsurance = new HDBInsurance();
            if (InsuranceId > 0)
            {
                ObjCWCR.GetInsurance(InsuranceId);
                if (ObjCWCR.DBResponse.Data != null)
                    ObjInsurance = (HDBInsurance)ObjCWCR.DBResponse.Data;
            }
            return PartialView(ObjInsurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditInsurance(HDBInsurance ObjInsurance)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjCWCR = new HDBMasterRepository();
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
            HDBMasterRepository ObjSR = new HDBMasterRepository();
            HDBSac ObjSac = new HDBSac();
            ObjSR.GetSac(SACId);
            if (ObjSR.DBResponse.Data != null)
            {
                ObjSac = (HDBSac)ObjSR.DBResponse.Data;
            }
            return PartialView(ObjSac);
        }
        [HttpGet]
        public ActionResult GetAllSAC()
        {
            HDBMasterRepository ObjSR = new HDBMasterRepository();
            List<HDBSac> LstSac = new List<HDBSac>();
            ObjSR.GetAllSac();
            if (ObjSR.DBResponse.Data != null)
            {
                LstSac = (List<HDBSac>)ObjSR.DBResponse.Data;
            }
            return PartialView("GetAllSAC", LstSac);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSacDetail(HDBSac ObjSac)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjSR = new HDBMasterRepository();
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
        /*
        [HttpGet]
        public ActionResult CreateSDopening()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            HDBMasterRepository ObjRepo = new HDBMasterRepository();
            HDBSDOpening ObjSD = new HDBSDOpening();
            ObjRepo.GetEximTrader();
            if (ObjRepo.DBResponse.Data != null)
            {
                ObjSD.LstEximTrader = (List<HDBSDOpening>)ObjRepo.DBResponse.Data;
            }
            //var model = new HDBSDOpening();
            for (var i = 0; i < 5; i++)
            {
                ObjSD.Details.Add(new HdbReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            return PartialView("CreateSDopening", ObjSD);

        }

        [HttpGet]
        public ActionResult GetSDList()
        {
            HDBMasterRepository ObjSDR = new HDBMasterRepository();
            List<HDBSDOpening> LstSD = new List<HDBSDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<HDBSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }
        [HttpGet]
        public ActionResult EditSDOpening(int SDId)
        {
            HDBSDOpening ObjSD = new HDBSDOpening();
            HDBMasterRepository ObjSDR = new HDBMasterRepository();
            ObjSDR.GetEximTrader();
            if (ObjSDR.DBResponse.Data != null)
            {
                ObjSD.LstEximTrader = (List<HDBSDOpening>)ObjSDR.DBResponse.Data;
            }
            if (SDId > 0)
            {
                ObjSDR.ViewEditSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (HDBSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            for (var i = 0; i < 5; i++)
            {
                ObjSD.Details.Add(new HdbReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
            }
            var PaymentMode = new SelectList(new[]
           {
                new SelectListItem { Text = "--- Select ---", Value = ""},
                new SelectListItem { Text = "CASH", Value = "CASH",Selected=true},
                new SelectListItem { Text = "NEFT", Value = "NEFT"},
                new SelectListItem { Text = "CHEQUE", Value = "CHEQUE"},
                new SelectListItem { Text = "DRAFT", Value = "DRAFT"},
                new SelectListItem { Text = "PO", Value = "PO"},
                 new SelectListItem { Text = "CHALLAN", Value = "CHALLAN"},
                 new SelectListItem { Text = "CREDIT NOTE", Value = "CREDITNOTE"},
            }, "Value", "Text");
            ViewBag.Type = PaymentMode;
            return PartialView("EditSDOpening", ObjSD);
        }
        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            HDBSDOpening ObjSD = new HDBSDOpening();
            if (SDId > 0)
            {
                HDBMasterRepository ObjSDR = new HDBMasterRepository();
                ObjSDR.ViewEditSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (HDBSDOpening)ObjSDR.DBResponse.Data;
                }
                for (var i = 0; i < 5; i++)
                {
                    ObjSD.Details.Add(new HdbReceiptDetails { Amount = 0M });
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(HDBSDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                HDBMasterRepository ObjSDR = new HDBMasterRepository();
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

        */

        [HttpGet]
        public ActionResult CreateSDopening()
        {
            AccessRightsRepository ACCR = new AccessRightsRepository();
            ACCR.GetMenuWiseAccessRight(Convert.ToInt32(Session["MenuId"]), ((Login)Session["LoginUser"]).Role.RoleId, Convert.ToInt32(Session["ModuleId"]), Convert.ToInt32(Session["BranchId"]));
            ViewBag.RightsList = JsonConvert.SerializeObject(ACCR.DBResponse.Data);

            HDBMasterRepository ObjRepo = new HDBMasterRepository();
            HDBSDOpening ObjSD = new HDBSDOpening();
            ObjRepo.GetPartyForSDOpening("",0);
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
                ObjSD.Details.Add(new HdbReceiptDetails { Date = DateTime.Today.ToString("dd/MM/yyyy"), Amount = 0M });
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
            ObjSD.Date = DateTime.Now.Date.ToString("dd/MM/yyyy");
            return PartialView("CreateSDopening", ObjSD);
        }

        [HttpGet]
        public JsonResult SearchPartyNameByPartyCode(string PartyCode)
        {
            HDBMasterRepository objMaster = new HDBMasterRepository();
            objMaster.GetPartyForSDOpening(PartyCode, 0);
            return Json(objMaster.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadPartyList(string PartyCode, int Page)
        {
            HDBMasterRepository objMaster = new HDBMasterRepository();
            objMaster.GetPartyForSDOpening(PartyCode, Page);
            return Json(objMaster.DBResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetSDList()
        {
            HDBMasterRepository ObjSDR = new HDBMasterRepository();
            List<HDBSDOpening> LstSD = new List<HDBSDOpening>();
            ObjSDR.GetAllSDOpening();
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<HDBSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult GetSDListByParty(string PartyName)
        {
            HDBMasterRepository ObjSDR = new HDBMasterRepository();
            List<HDBSDOpening> LstSD = new List<HDBSDOpening>();
            ObjSDR.SearchSDByPartyName(PartyName);
            if (ObjSDR.DBResponse.Data != null)
            {
                LstSD = (List<HDBSDOpening>)ObjSDR.DBResponse.Data;
            }
            return PartialView("SDList", LstSD);
        }

        [HttpGet]
        public ActionResult ViewSDOpening(int SDId)
        {
            HDBSDOpening ObjSD = new HDBSDOpening();
            if (SDId > 0)
            {
                HDBMasterRepository ObjSDR = new HDBMasterRepository();
                ObjSDR.GetSDOpening(SDId);
                if (ObjSDR.DBResponse.Data != null)
                {
                    ObjSD = (HDBSDOpening)ObjSDR.DBResponse.Data;
                }
            }
            return PartialView("ViewSDOpening", ObjSD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditSDopening(HDBSDOpening ObjSD)
        {
            if (ModelState.IsValid)
            {
                var xml = Utility.CreateXML(ObjSD.Details.Where(o => o.Amount > 0).ToList());
                HDBMasterRepository ObjSDR = new HDBMasterRepository();
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
            HDBYardVM ObjYard = new HDBYardVM();
            if (YardId > 0)
            {
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (HDBYardVM)ObjYR.DBResponse.Data;
                    ObjYard.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjYard.LstYard);
                }
            }
            return PartialView("EditYard", ObjYard);
        }

        [HttpGet]
        public ActionResult ViewYard(int YardId)
        {
            HDBYardVM ObjYard = new HDBYardVM();
            if (YardId > 0)
            {
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.GetYard(YardId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (HDBYardVM)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewYardHDB", ObjYard);
        }

        [HttpGet]
        public ActionResult GetYardList()
        {
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllYard();
            List<HDBYard> LstYard = new List<HDBYard>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstYard = (List<HDBYard>)ObjYR.DBResponse.Data;
            }
            return PartialView("YardList", LstYard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditYardDetail(HDBYardVM ObjYard)
        {
            var DelLocationXML = "";
            string LocationXML;
            if (ModelState.IsValid)
            {
                if (ObjYard.LocationDetail != null)
                {
                    ObjYard.LstYard = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<HDBYardWiseLocation>>(ObjYard.LocationDetail);
                }
                if (ObjYard.DelLocationDetail != null)
                {
                    var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<HDBYardWiseLocation>>(ObjYard.DelLocationDetail);
                    DelLocationXML = Utility.CreateXML(DelLocationList);
                }
                LocationXML = Utility.CreateXML(ObjYard.LstYard);
                HDBMasterRepository ObjYR = new HDBMasterRepository();
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
                HDBMasterRepository ObjYR = new HDBMasterRepository();
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
            HDBMasterRepository objSR = new HDBMasterRepository();
            HDBOperation objOPR = new HDBOperation();
            objSR.GetAllSac();
            if (objSR.DBResponse.Data != null)
                objOPR.LstSac = (List<HDBSac>)objSR.DBResponse.Data;
            return PartialView("CreateOperation", objOPR);
        }

        [HttpGet]
        public ActionResult ViewOperation(int OperationId)
        {
            HDBOperation ObjOperation = new HDBOperation();
            if (OperationId > 0)
            {
               HDBMasterRepository ObjOR = new HDBMasterRepository();
                ObjOR.ViewMstOperation(OperationId);
                if (ObjOR.DBResponse.Data != null)
                {
                    ObjOperation = (HDBOperation)ObjOR.DBResponse.Data;
                }
            }
            return PartialView("ViewOperation", ObjOperation);
        }

        [HttpGet]
        public ActionResult GetOperationList()
        {
            List<HDBOperation> lstOperation = new List<HDBOperation>();
            HDBMasterRepository objOR = new HDBMasterRepository();
            objOR.GetAllMstOperation();
            if (objOR.DBResponse.Data != null)
                lstOperation = (List<HDBOperation>)objOR.DBResponse.Data;
            return PartialView("OperationList", lstOperation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOperation(HDBOperation ObjOperation)
        {
            if (ModelState.IsValid)
            {
                ObjOperation.ShortDescription = ObjOperation.ShortDescription == null ? null : ObjOperation.ShortDescription.Trim();
                ObjOperation.Description = ObjOperation.Description == null ? null : ObjOperation.Description.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjOperation.Uid = ObjLogin.Uid;
                HDBMasterRepository ObjOR = new HDBMasterRepository();
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
           HDBMasterRepository objCR = new HDBMasterRepository();
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
            List<HDBCWCEntryFees> lstEntryFees = new List<HDBCWCEntryFees>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetAllEntryFees(0);
            if (objCR.DBResponse.Data != null)
                lstEntryFees = (List<HDBCWCEntryFees>)objCR.DBResponse.Data;
            return PartialView("EntryFeesList", lstEntryFees);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditEntryFees(HDBCWCEntryFees objEF)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository objRepository = new HDBMasterRepository();
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
            HDBCWCEntryFees objEF = new HDBCWCEntryFees();
            HDBMasterRepository objRepo = new HDBMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (EntryFeeId > 0)
            {
                objRepo.GetAllEntryFees(EntryFeeId);
                if (objRepo.DBResponse.Data != null)
                    objEF = (HDBCWCEntryFees)objRepo.DBResponse.Data;
            }
            return PartialView("EditEntryFees", objEF);
        }
        #endregion
        #region Ground Rent
        [HttpGet]
        public ActionResult CreateGroundRent()
        {
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            IList<HDBCWCChargesGroundRent> objGR = new List<HDBCWCChargesGroundRent>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetAllGroundRentDet();
            if (objCR.DBResponse.Data != null)
                objGR = (List<HDBCWCChargesGroundRent>)objCR.DBResponse.Data;
            return PartialView(objGR);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstGroundRent(HDBCWCChargesGroundRent objCWC)
        {
            /*
            Container Type: 1.Empty Cnntainer 2.Loaded Container
            Commodity Type: 1.HAZ 2.Non HAZ
            Operation Type:1.Import 2.Export
            */
            if (ModelState.IsValid)
            {
                HDBMasterRepository objCR = new HDBMasterRepository();
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
            HDBMasterRepository objCR = new HDBMasterRepository();
            HDBCWCChargesGroundRent objCGR = new HDBCWCChargesGroundRent();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (GroundRentId > 0)
            {
                objCR.GetGroundRentDet(GroundRentId);
                if (objCR.DBResponse.Data != null)
                    objCGR = (HDBCWCChargesGroundRent)objCR.DBResponse.Data;
            }
            return PartialView(objCGR);
        }
        #endregion
        #region Reefer
        [HttpGet]
        public ActionResult CreateReefer()
        {
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            IList<HDBCWCReefer> objList = new List<HDBCWCReefer>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetAllReefer();
            if (objCR.DBResponse.Data != null)
                objList = (List<HDBCWCReefer>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstReefer(HDBCWCReefer objRef)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository objCR = new HDBMasterRepository();
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
            HDBMasterRepository objCR = new HDBMasterRepository();
            HDBCWCReefer objRef = new HDBCWCReefer();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (ReeferChrgId > 0)
            {
                objCR.GetReeferDet(ReeferChrgId);
                if (objCR.DBResponse.Data != null)
                    objRef = (HDBCWCReefer)objCR.DBResponse.Data;
            }
            return PartialView(objRef);
        }
        #endregion
        #region Weighment
        [HttpGet]
        public ActionResult CreateWeighment()
        {
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditWeighment(HDBCWCWeighment objCW)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository objCR = new HDBMasterRepository();
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
            List<HDBCWCWeighment> lstWeighment = new List<HDBCWCWeighment>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetWeighmentDet(0);
            if (objCR.DBResponse.Data != null)
                lstWeighment = (List<HDBCWCWeighment>)objCR.DBResponse.Data;
            return PartialView("WeighmentList", lstWeighment);
        }
        [HttpGet]
        public ActionResult EditWeighment(int WeighmentId)
        {
            HDBCWCWeighment objCW = new HDBCWCWeighment();
            HDBMasterRepository objCCR = new HDBMasterRepository();
            if (WeighmentId > 0)
            {
                objCCR.ListOfSACCode();
                if (objCCR.DBResponse.Data != null)
                    ViewBag.ListOfSAC = objCCR.DBResponse.Data;
                else
                    ViewBag.ListOfSAC = null;
                objCCR.GetWeighmentDet(WeighmentId);
                if (objCCR.DBResponse.Data != null)
                    objCW = (HDBCWCWeighment)objCCR.DBResponse.Data;
            }
            return PartialView(objCW);
        }
        #endregion
        #region TDS
        [HttpGet]
        public ActionResult CreateTds()
        {
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            IList<HDBCWCTds> objList = new List<HDBCWCTds>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetAllTDS();
            if (objCR.DBResponse.Data != null)
                objList = (List<HDBCWCTds>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddEditMstTds(HDBCWCTds objTds)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository objCR = new HDBMasterRepository();
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
           HDBMasterRepository objCR = new HDBMasterRepository();
            HDBCWCTds objTds = new HDBCWCTds();
            objCR.ListOfSACCode();
            if (objCR.DBResponse.Data != null)
                ViewBag.ListOfSAC = objCR.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (TdsId > 0)
            {
                objCR.GetTDSDet(TdsId);
                if (objCR.DBResponse.Data != null)
                    objTds = (HDBCWCTds)objCR.DBResponse.Data;
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
            HDBGodownVM ObjGodown = new HDBGodownVM();
            if (GodownId > 0)
            {
                HDBMasterRepository ObjGR = new HDBMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (HDBGodownVM)ObjGR.DBResponse.Data;
                    ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("EditGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult ViewGodown(int GodownId)
        {
            HDBGodownVM ObjGodown = new HDBGodownVM();
            if (GodownId > 0)
            {
                HDBMasterRepository ObjGR = new HDBMasterRepository();
                ObjGR.GetGodown(GodownId);
                if (ObjGR.DBResponse.Data != null)
                {
                    ObjGodown = (HDBGodownVM)ObjGR.DBResponse.Data;
                    //ObjGodown.LocationDetail = Newtonsoft.Json.JsonConvert.SerializeObject(ObjGodown.LstLocation);
                }
            }
            return View("ViewGodown", ObjGodown);
        }

        [HttpGet]
        public ActionResult GetGodownList()
        {
            HDBMasterRepository ObjGR = new HDBMasterRepository();
            ObjGR.GetAllGodown();
            List<HDBGodown> LstGodown = new List<HDBGodown>();
            if (ObjGR.DBResponse.Data != null)
            {
                LstGodown = (List<HDBGodown>)ObjGR.DBResponse.Data;
            }
            return View("GodownList", LstGodown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownDetail(HDBGodownVM ObjGodown)
        {
            var DelLocationXML = "";
            if (ObjGodown.LocationDetail != null)
            {
                ObjGodown.LstLocation = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<HDBGodownWiseLocation>>(ObjGodown.LocationDetail);
            }
            if (ObjGodown.DelLocationDetail != null)
            {
                var DelLocationList = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<HDBGodownWiseLocation>>(ObjGodown.DelLocationDetail);
                DelLocationXML = Utility.CreateXML(DelLocationList);
            }
            if (ModelState.IsValid)
            {
               HDBMasterRepository ObjGR = new HDBMasterRepository();
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
                HDBMasterRepository ObjGR = new HDBMasterRepository();
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
            HDBMasterRepository objCR = new HDBMasterRepository();
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
            HDBCWCFranchiseCharges objFC = new HDBCWCFranchiseCharges();
            HDBMasterRepository objRepo = new HDBMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (HDBCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(HDBCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository objRepository = new HDBMasterRepository();
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
            IList<HDBCWCFranchiseCharges> objList = new List<HDBCWCFranchiseCharges>();
            HDBMasterRepository objCR = new HDBMasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<HDBCWCFranchiseCharges>)objCR.DBResponse.Data;
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
            HDBMasterRepository ObjCh = new HDBMasterRepository();
            HDBChemical ObjChem = new HDBChemical();
            if (ChemicalId > 0)
            {
                
                ObjCh.GetChemical(ChemicalId);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (HDBChemical)ObjCh.DBResponse.Data;
                   
                }
            }
            return PartialView("EditChemical", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewChemical(int ChemicalId)
        {
            HDBChemical ObjYard = new HDBChemical();
            if (ChemicalId > 0)
            {
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.GetChemical(ChemicalId);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (HDBChemical)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewChemical", ObjYard);
        }

        [HttpGet]
        public ActionResult GetChemicalList()

        {
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllChemical();
            List<HDBChemical> LstChemical = new List<HDBChemical>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<HDBChemical>)ObjYR.DBResponse.Data;
            }
            return PartialView("ChemicalList", LstChemical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditChemicalDetail(HDBChemical ObjChem)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjCR = new HDBMasterRepository();
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
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteChemical(int ChemicalId)
        {
            HDBMasterRepository ObjCR = new HDBMasterRepository();
            if (ChemicalId > 0)
                ObjCR.DeleteChemical(ChemicalId);
            return Json(ObjCR.DBResponse);
        }


        #endregion

        #region Holiday
        public ActionResult CreateHoliday()
        {
            return PartialView("CreateHoliday");
        }

        [HttpGet]
        public ActionResult EditHoliday(int Id)
        {
            HDBMasterRepository ObjCh = new HDBMasterRepository();
            HDBHoliday ObjChem = new HDBHoliday();
            if (Id > 0)
            {

                ObjCh.GetHoliDay(Id);
                if (ObjCh.DBResponse.Data != null)
                {
                    ObjChem = (HDBHoliday)ObjCh.DBResponse.Data;

                }
            }
            return PartialView("EditHoliday", ObjChem);
        }

        [HttpGet]
        public ActionResult ViewHoliday(int Id)
        {
            HDBHoliday ObjYard = new HDBHoliday();
            if (Id > 0)
            {
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.GetHoliDay(Id);
                if (ObjYR.DBResponse.Data != null)
                {
                    ObjYard = (HDBHoliday)ObjYR.DBResponse.Data;
                }
            }
            return PartialView("ViewHoliday", ObjYard);
        }

        [HttpGet]
        public ActionResult GetHolidayList()

        {
            HDBMasterRepository ObjYR = new HDBMasterRepository();
            ObjYR.GetAllHoliDay();
            List<HDBHoliday> LstChemical = new List<HDBHoliday>();
            if (ObjYR.DBResponse.Data != null)
            {
                LstChemical = (List<HDBHoliday>)ObjYR.DBResponse.Data;
            }
            return PartialView("HolidayList", LstChemical);
        }

        [HttpPost]
       
        public ActionResult AddEditHolidayDetail(HDBHoliday ObjChem)
        {
            if (ModelState.IsValid)
            {
                HDBMasterRepository ObjCR = new HDBMasterRepository();
                ObjChem.Discription = ObjChem.Discription;
                ObjChem.Date = ObjChem.Date;
                //Login ObjLogin = (Login)Session["LoginUser"];
                //ObjChem.Uid = ObjLogin.Uid;
                //ObjChem.BranchId = Convert.ToInt32(Session["BranchId"].ToString());
                ObjCR.AddEditHoliDay(ObjChem);
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
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteHoliDayDetails(int Id)
        {
            if (Id > 0)
            {
                HDBMasterRepository ObjYR = new HDBMasterRepository();
                ObjYR.DeleteHoliDay(Id);
                return Json(ObjYR.DBResponse);
            }
            else
            {
                var Err = new { Status = 1, Message = "Error" };
                return Json(Err);
            }
        }



        #endregion


        #region Tds Ledger Code 

        public ActionResult LedgerCode()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditLedgerCodeDetail(LedgerCodeDetails ObjLedger)
        {
            if (ModelState.IsValid)
            {
               
                Login ObjLogin = (Login)Session["LoginUser"];

                ObjLedger.Uid = ObjLogin.Uid;
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.AddEditLedgerCode(ObjLedger);
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
        public ActionResult GetLedgerCodeList()
        {
            HDBMasterRepository ObjPR = new HDBMasterRepository();
            List<LedgerCodeDetails> LstPort = new List<LedgerCodeDetails>();
            ObjPR.GetAllLedgerCode();
            if (ObjPR.DBResponse.Data != null)
            {
                LstPort = (List<LedgerCodeDetails>)ObjPR.DBResponse.Data;
            }
            return PartialView("GetLedgerCodeList", LstPort);
        }
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public ActionResult DeleteLedgerCode(int LedgerId)
        {
            if (LedgerId > 0)
            {
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.DeleteLedgerCode(LedgerId);
                return Json(ObjPR.DBResponse);
            }
            else
            {
                var Err = new { Status = 0, Message = "Error" };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult EditLedgerCode(int LedgerId)
        {
            LedgerCodeDetails ObjLedgerCodeDetails = new LedgerCodeDetails();
            ViewBag.Country = null;
            if (LedgerId > 0)
            {
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.GetLedgerCode(LedgerId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjLedgerCodeDetails = (LedgerCodeDetails)ObjPR.DBResponse.Data;
                }
                CountryRepository ObjCR = new CountryRepository();
               
            }
            return PartialView("EditLedgerCode", ObjLedgerCodeDetails);
        }

        [HttpGet]
        public ActionResult ViewLedgerCode(int LedgerId)
        {
            LedgerCodeDetails ObjLedgerCodeDetails = new LedgerCodeDetails();
            if (LedgerId > 0)
            {
                HDBMasterRepository ObjPR = new HDBMasterRepository();
                ObjPR.GetLedgerCode(LedgerId);
                if (ObjPR.DBResponse.Data != null)
                {
                    ObjLedgerCodeDetails = (LedgerCodeDetails)ObjPR.DBResponse.Data;
                }
            }
            return PartialView("ViewLedgerCode", ObjLedgerCodeDetails);
        }
        #endregion

    }
}
