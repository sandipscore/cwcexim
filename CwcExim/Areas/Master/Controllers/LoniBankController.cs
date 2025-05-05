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
    public class LoniBankController : Controller
    {
        // GET: Master/PPGBank
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult CreateBank()
        {
            LONIMasterRepository objbank = new LONIMasterRepository();

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
            List<PPGBank> LstBank = new List<PPGBank>();
            LONIMasterRepository ObjBR = new LONIMasterRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<PPGBank>)ObjBR.DBResponse.Data;
            }
            return PartialView("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            PPGBank ObjBank = new PPGBank();
            if (BankId > 0)
            {
                LONIMasterRepository ObjBR = new LONIMasterRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank = (PPGBank)ObjBR.DBResponse.Data;
                }
            }
            return PartialView("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(PPGBank ObjBank)
        {
            if (ModelState.IsValid)
            {
                LONIMasterRepository ObjBR = new LONIMasterRepository();
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