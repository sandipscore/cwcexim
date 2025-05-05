using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class BankController : BaseController
    {
        [HttpGet]
        public ActionResult CreateBank()
        {
            return View("CreateBank");
        }

        [HttpGet]
        public ActionResult GetBankList()
        {
            List<Bank> LstBank = new List<Bank>();
            BankRepository ObjBR = new BankRepository();
            ObjBR.GetAllBank();
            if (ObjBR.DBResponse.Data != null)
            {
                LstBank = (List<Bank>)ObjBR.DBResponse.Data;
            }
            return View("BankList", LstBank);
        }
        [HttpGet]
        public ActionResult ViewBank(int BankId)
        {
            Bank ObjBank = new Bank();
            if (BankId > 0)
            {
                BankRepository ObjBR = new BankRepository();
                ObjBR.GetBank(BankId);
                if (ObjBR.DBResponse.Data != null)
                {
                    ObjBank=(Bank)ObjBR.DBResponse.Data;
                }
            }
            return View("ViewBank", ObjBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddBankDetail(Bank ObjBank)
        {
            if (ModelState.IsValid)
            {
                BankRepository ObjBR = new BankRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjBank.Uid = ObjLogin.Uid;
                ObjBank.BankName=ObjBank.BankName.Trim();
                ObjBank.Address = ObjBank.Address==null?null:ObjBank.Address.Trim();
                ObjBank.Branch=ObjBank.Branch.Trim();
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