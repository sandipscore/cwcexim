using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Areas.Master.Models;
using CwcExim.Repositories;
using Newtonsoft.Json;
namespace CwcExim.Areas.Master.Controllers
{
    public class PPGBankController : Controller
    {
        // GET: Master/PPGBank
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateBank()
        {
            PPGMasterRepository objbank = new PPGMasterRepository();

            objbank.GetLedger();
            if (objbank.DBResponse.Status > 0)
                ViewBag.LdgNM = JsonConvert.SerializeObject(objbank.DBResponse.Data);
            else
                ViewBag.LdgNM = null;

            return View("CreateBank");
        }

        [HttpGet]
        public ActionResult GetBankList()
        {
            List<PPGBank> LstBank = new List<PPGBank>();
            PPGMasterRepository ObjBR = new PPGMasterRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<PPGBank>)ObjBR.DBResponse.Data;
            }
            return View("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            PPGBank ObjBank = new PPGBank();
            if (BankId > 0)
            {
                PPGMasterRepository ObjBR = new PPGMasterRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank = (PPGBank)ObjBR.DBResponse.Data;
                }
            }
            return View("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(PPGBank ObjBank)
        {
            if (ModelState.IsValid)
            {
                PPGMasterRepository ObjBR = new PPGMasterRepository();
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

    }
}