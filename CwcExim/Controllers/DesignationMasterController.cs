using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    //[Authorize]
    public class DesignationMasterController : BaseController
    {
        // GET: DesignationMaster
        public ActionResult Index()
        {
            return View();
        }

        #region CreateDesignation View      
        public ActionResult CreateDesignation()
        {
            DesignationRepository ObjDRR = new DesignationRepository();
            ObjDRR.GetAllDesignation();
            DesignationMaster ObjDesignation = new DesignationMaster();
            if (ObjDRR.DBResponse.Data != null)
            {
                ObjDesignation.HigherAuthorityList = (IEnumerable<DesignationMaster>)ObjDRR.DBResponse.Data;               
            }
            else
            {
                ObjDesignation.HigherAuthorityList = new List<DesignationMaster>();
            }
            return View(ObjDesignation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDesignation(DesignationMaster ObjDesignation)
        {
            int _Status = 0;
            string _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                DesignationRepository ObjDRR = new DesignationRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjDesignation.CreatedBy = ObjLogin.Uid;
                ObjDesignation.Designation = ObjDesignation.Designation.Trim();
                ObjDRR.AddEditDesignation(ObjDesignation);
                ModelState.Clear();
                _Status = ObjDRR.DBResponse.Status;
                _Message = ObjDRR.DBResponse.Message;
            }

            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion

        #region DesignationList View
        public ActionResult GetDesignationList()
        {
            DesignationRepository ObjDR = new DesignationRepository();
            ObjDR.GetAllDesignation();
            IEnumerable<DesignationMaster> lstDesignation = (IEnumerable<DesignationMaster>)ObjDR.DBResponse.Data;
            if (ObjDR.DBResponse.Data != null)
            {
                return View("DesignationList", lstDesignation);
            }
            else
            {
                return View("DesignationList", new List<DesignationMaster>());
            }
        }

        #endregion

        #region EditDesignation View
        public ActionResult EditDesignation(int DesignationId)
        {
            DesignationRepository ObjDR = new DesignationRepository();
            ObjDR.GetDesignation(DesignationId);
            DesignationMaster ObjDesignation = (DesignationMaster)ObjDR.DBResponse.Data;
            if (ObjDesignation != null)
            {
                ObjDR.GetAllDesignation();
                if (ObjDR.DBResponse.Data != null)
                {
                    ObjDesignation.HigherAuthorityList = (IEnumerable<DesignationMaster>)ObjDR.DBResponse.Data;
                }
                else
                {
                    ObjDesignation.HigherAuthorityList = new List<DesignationMaster>();
                }
                return View(ObjDesignation);
            }
            else
            {
                return View(new List<DesignationMaster>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDesignation(DesignationMaster ObjDesignation)
        {
            int _Status = 0;
            string _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                DesignationRepository ObjDRR = new DesignationRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjDesignation.CreatedBy = ObjLogin.Uid;
                ObjDesignation.Designation = ObjDesignation.Designation.Trim();
                ObjDRR.AddEditDesignation(ObjDesignation);
                _Status = ObjDRR.DBResponse.Status;
                _Message = ObjDRR.DBResponse.Message;
            }

            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion
    }
}