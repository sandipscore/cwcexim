using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Filters;

namespace CwcExim.Controllers
{
    public class ExpenseHeadController : BaseController
    {
        [HttpGet]
        public ActionResult LoadTab()
        {
            return View("Main");
        }

        [HttpGet]
        public ActionResult CreateExpenseHead()
        {
            return PartialView("CreateExpenseHead");
        }

        [HttpGet]
        public ActionResult GetExpenseHeadList()
        {
            ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
            List<ExpenseHead> LstExpense = new List<ExpenseHead>();
            ObjEHR.GetAllExpenseHead();
            if (ObjEHR.DBResponse.Data != null)
            {
                LstExpense = (List<ExpenseHead>)ObjEHR.DBResponse.Data;
            }
            return PartialView("ExpenseHeadList", LstExpense);
        }

        [HttpGet]
        public ActionResult EditExpenseHead(int ExpenseHeadId)
        {
            ExpenseHead ObjExpense = new ExpenseHead();
            if (ExpenseHeadId > 0)
            {
                ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
                ObjEHR.GetExpenseHead(ExpenseHeadId);
                if (ObjEHR.DBResponse.Data != null)
                {
                    ObjExpense = (ExpenseHead)ObjEHR.DBResponse.Data;
                }
            }
            return View("EditExpenseHead", ObjExpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExpenseHeadDetail(ExpenseHead ObjExpense)
        {
            if (ModelState.IsValid)
            {
                ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
                ObjExpense.ExpHead = ObjExpense.ExpHead.Trim();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjExpense.Uid = ObjLogin.Uid;
                ObjEHR.AddEditExpenseHead(ObjExpense);
                ModelState.Clear();
                return Json(ObjEHR.DBResponse);
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
        public ActionResult DeleteExpenseHeadDetail(int ExpenseHeadId)
        {
            if (ExpenseHeadId > 0)
            {
                ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
                ObjEHR.DeleteExpenseHead(ExpenseHeadId);
                return Json(ObjEHR.DBResponse);
            }
            else
            {
                return Json(new { Staus = 0, Message = "Error" });
            }
        }

        [HttpGet]
        public ActionResult CreateExpCodeWiseHSN()
        {
            ExpenseCodeWiseHSN ObjExpense = new ExpenseCodeWiseHSN();
            ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
            ObjEHR.GetAllExpenseHead();
            if (ObjEHR.DBResponse.Data != null)
            {
                ObjExpense.LstExpenseCode = (List<ExpenseHead>)ObjEHR.DBResponse.Data;
            }
            ObjEHR.GetAllHSNCode();
            if (ObjEHR.DBResponse.Data != null)
            {
                ObjExpense.LstHSN = (List<ExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
            }
            return View("CreateExpCodeWiseHSN", ObjExpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpCodeWiseHSNDet(ExpenseCodeWiseHSN ObjExpense)
        {
            if (ModelState.IsValid)
            {
                ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjExpense.Uid= ObjLogin.Uid;
                ObjEHR.AddExpenseCodeWiseHSN(ObjExpense);
                ModelState.Clear();
                return Json(ObjEHR.DBResponse);
            }
            else
            {
                var ErrorMessage = string.Join(",", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                var Err = new { Status = 0, Message = ErrorMessage };
                return Json(Err);
            }
        }

        [HttpGet]
        public ActionResult GetExpCodeWiseHSNList()
        {
            ExpenseHeadRepository ObjEHR = new ExpenseHeadRepository();
            List<ExpenseCodeWiseHSN> LstExpense = new List<ExpenseCodeWiseHSN>();
            ObjEHR.GetAllExpCodeWiseHSN();
            if (ObjEHR.DBResponse.Data != null)
            {
                LstExpense=(List<ExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
            }
            return PartialView("ExpCodeWiseHSNList", LstExpense);
        }
    }
}