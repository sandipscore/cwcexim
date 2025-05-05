
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
    public class WLJMasterController : Controller
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
            WLJMasterRepository objCR = new WLJMasterRepository();
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
            WLJCWCFranchiseCharges objFC = new WLJCWCFranchiseCharges();
            WLJMasterRepository objRepo = new WLJMasterRepository();
            objRepo.ListOfSACCode();
            if (objRepo.DBResponse.Data != null)
                ViewBag.ListOfSAC = objRepo.DBResponse.Data;
            else
                ViewBag.ListOfSAC = null;
            if (franchisechargeid > 0)
            {
                objRepo.GetFranchiseCharge(franchisechargeid);
                if (objRepo.DBResponse.Data != null)
                    objFC = (WLJCWCFranchiseCharges)objRepo.DBResponse.Data;
            }
            return PartialView(objFC);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddEditFranchiseCharges(WLJCWCFranchiseCharges objFC)
        {
            if (ModelState.IsValid)
            {
                WLJMasterRepository objRepository = new WLJMasterRepository();
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
            IList<WLJCWCFranchiseCharges> objList = new List<WLJCWCFranchiseCharges>();
            WLJMasterRepository objCR = new WLJMasterRepository();
            objCR.GetAllFranchiseCharges();
            if (objCR.DBResponse.Data != null)
                objList = (List<WLJCWCFranchiseCharges>)objCR.DBResponse.Data;
            return PartialView(objList);
        }
        #endregion
    }


}