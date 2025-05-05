using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CwcExim.Repositories;
using CwcExim.Filters;
using CwcExim.Controllers;
using CwcExim.Areas.Master.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Master
{
    public class DSRExpenseHeadController : BaseController
    {
        [HttpGet]
        public ActionResult LoadTab()
        {
            return PartialView("Main");
        }

        [HttpGet]
        public ActionResult CreateExpenseHead()
        {
            return PartialView("CreateExpenseHead");
        }

        [HttpGet]
        public ActionResult GetExpenseHeadList()
        {
            DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
            List<DSRExpenseHead> LstExpense = new List<DSRExpenseHead>();
            ObjEHR.GetAllExpenseHead();
            if (ObjEHR.DBResponse.Data != null)
            {
                LstExpense = (List<DSRExpenseHead>)ObjEHR.DBResponse.Data;
            }
            return PartialView("ExpenseHeadList", LstExpense);
        }

        [HttpGet]
        public ActionResult EditExpenseHead(int ExpenseHeadId)
        {
            DSRExpenseHead ObjExpense = new DSRExpenseHead();
            if (ExpenseHeadId > 0)
            {
                DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
                ObjEHR.GetExpenseHead(ExpenseHeadId);
                if (ObjEHR.DBResponse.Data != null)
                {
                    ObjExpense = (DSRExpenseHead)ObjEHR.DBResponse.Data;
                }
            }
            return PartialView("EditExpenseHead", ObjExpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditExpenseHeadDetail(DSRExpenseHead ObjExpense)
        {
            if (ModelState.IsValid)
            {
                DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
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
                DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
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
            DSRExpenseCodeWiseHSN ObjExpense = new DSRExpenseCodeWiseHSN();
            DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
            ObjEHR.GetAllExpenseHead();
            if (ObjEHR.DBResponse.Data != null)
            {
                ObjExpense.LstExpenseCode = (List<DSRExpenseHead>)ObjEHR.DBResponse.Data;
            }
            ObjEHR.GetAllHSNCode();
            if (ObjEHR.DBResponse.Data != null)
            {
                ObjExpense.LstHSN = (List<DSRExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
            }
            return PartialView("CreateExpCodeWiseHSN", ObjExpense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpCodeWiseHSNDet(DSRExpenseCodeWiseHSN ObjExpense)
        {
            if (ModelState.IsValid)
            {
                DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
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
            DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
            List<DSRExpenseCodeWiseHSN> LstExpense = new List<DSRExpenseCodeWiseHSN>();
            ObjEHR.GetAllExpCodeWiseHSN();
            if (ObjEHR.DBResponse.Data != null)
            {
                LstExpense=(List<DSRExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
            }
            return PartialView("ExpCodeWiseHSNList", LstExpense);
        }


        [HttpGet]
        public ActionResult EditExpHSN(int ExpHSNId)
        {
            DSRExpenseCodeWiseHSN ObjExpense = new DSRExpenseCodeWiseHSN();
            DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();

            if (ExpHSNId > 0)
            {
                ObjEHR.GetExpHSNCode(ExpHSNId);
                if (ObjEHR.DBResponse.Data != null)
                {
                    ObjExpense = (DSRExpenseCodeWiseHSN)ObjEHR.DBResponse.Data;
                }
                ObjEHR.GetAllExpenseHead();
                if (ObjEHR.DBResponse.Data != null)
                {
                    ObjExpense.LstExpenseCode = (List<DSRExpenseHead>)ObjEHR.DBResponse.Data;
                }
                ObjEHR.GetAllHSNCode();
                if (ObjEHR.DBResponse.Data != null)
                {
                    ObjExpense.LstHSN = (List<DSRExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
                }
                
            }
            return PartialView("EditExpCodeWiseHSN", ObjExpense);
        }


        //[HttpGet]
        //public ActionResult EditExpHSN(int ExpHSNId) 
        //{
        //    DSRExpenseCodeWiseHSN ObjExpense = new DSRExpenseCodeWiseHSN();
        //    DSRExpenseHeadRepository ObjEHR = new DSRExpenseHeadRepository();
        //    ObjEHR.GetAllExpenseHead();
        //    if (ObjEHR.DBResponse.Data != null)
        //    {
        //        ObjExpense.LstExpenseCode = (List<DSRExpenseHead>)ObjEHR.DBResponse.Data;
        //    }
        //    ObjEHR.GetAllHSNCode();
        //    if (ObjEHR.DBResponse.Data != null)
        //    {
        //        ObjExpense.LstHSN = (List<DSRExpenseCodeWiseHSN>)ObjEHR.DBResponse.Data;
        //    }

        //    DSRExpenseHeadRepository ObjEHRd = new DSRExpenseHeadRepository();
        //    DSRExpenseCodeWiseHSN ExpHSN = new DSRExpenseCodeWiseHSN();
        //        ObjEHR.GetExpHSNCode(ExpHSNId);
        //    if (ObjEHR.DBResponse.Data != null)
        //    {
        //        ExpHSN = (DSRExpenseCodeWiseHSN)ObjEHR.DBResponse.Data;
        //    }
        //    return PartialView("EditExpCodeWiseHSN", ExpHSN);
        //}
    }
}