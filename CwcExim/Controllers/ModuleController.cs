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
  
    public class ModuleController : BaseController
    {
        
        #region CreateModule View
        public ActionResult Load()
        {

            // Module ObjModule = new Module();
            //DesignationRepository ObjDRR = new DesignationRepository();
            //ObjDRR.GetAllDesignation();
            //DesignationMaster ObjDesignation = new DesignationMaster();
            //if (ObjDRR.DBResponse.Data != null)
            //{
            //    ObjModule.HighestAppAuthList = (IEnumerable<DesignationMaster>)ObjDRR.DBResponse.Data;
            //}
            //else
            //{
            //    ObjModule.HighestAppAuthList = new List<DesignationMaster>();
            //}
            // return PartialView("CreateModule", ObjModule);
            return PartialView("CreateModule");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModule(Module ObjModule)
        {
            int _Status = 0;
            string _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                /// PURPOSE: INSERT Module Details in database
                ModuleRepository ObjMRR = new ModuleRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjModule.CreatedBy = ObjLogin.Uid;
                ObjModule.ModuleName = ObjModule.ModuleName.Trim();
                ObjModule.ModulePrefix = ObjModule.ModulePrefix.Trim();
                ObjMRR.AddEditModule(ObjModule);
                ModelState.Clear();
                _Status = ObjMRR.DBResponse.Status;
                _Message = ObjMRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion

        #region ModuleList View
        public ActionResult GetModuleList()
        {
           
            ModuleRepository ObjMRR = new ModuleRepository();
            ObjMRR.GetAllModule();
            IEnumerable<Module> lstModule = (IEnumerable<Module>)ObjMRR.DBResponse.Data;
            if (ObjMRR.DBResponse.Data != null)
            {
                return View("ModuleList", lstModule);
            }
            else
            {
                return PartialView("ModuleList", new List<Module>());
            }
        }

        #endregion

        #region EditModule View
        public ActionResult EditModule(int ModuleId)
        {
           
            Module ObjModule = new Module();
           // ObjModule.HighestAppAuthList= new List<DesignationMaster>();
            if(ModuleId>0)
            {
                ModuleRepository objMRR = new ModuleRepository();
                objMRR.GetModule(ModuleId);
                if (objMRR.DBResponse.Data != null)
                {
                    ObjModule = (Module)objMRR.DBResponse.Data;
                   // DesignationRepository ObjDRR = new DesignationRepository();
                   // ObjDRR.GetAllDesignation();
                    //if (ObjDRR.DBResponse.Data != null)
                    //{
                    //    ObjModule.HighestAppAuthList = (IEnumerable<DesignationMaster>)ObjDRR.DBResponse.Data;
                    //}
                }
            }

            return PartialView(ObjModule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule(Module ObjModule)
        {
           // ModelState.Remove("ModuleName");
           // ModelState.Remove("ModulePrefix");
            int _Status = 0;
            var _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {               
                ModuleRepository ObjMRR = new ModuleRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjModule.CreatedBy = ObjLogin.Uid;
                ObjMRR.AddEditModule(ObjModule);
                ModelState.Clear();
                _Status = ObjMRR.DBResponse.Status;
                _Message = ObjMRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }
        #endregion

        #region View Module
        public ActionResult ViewModule(int ModuleId)
        {

            Module ObjModule = new Module();
            if (ModuleId > 0)
            {
                ModuleRepository objMRR = new ModuleRepository();
                objMRR.GetModule(ModuleId);
                if (objMRR.DBResponse.Data != null)
                {
                    ObjModule = (Module)objMRR.DBResponse.Data;
                }
            }

            return PartialView(ObjModule);
        }
        #endregion

        #region Delete Module
        [HttpPost]
        [CustomValidateAntiForgeryToken]
        public JsonResult DeleteModule(int ModuleId)
        {
            ModuleRepository objMR = new ModuleRepository();
            objMR.DeleteModule(ModuleId);
            return Json(objMR.DBResponse);
        }
        #endregion

        #region UserDashboard

        #region ModuleListForUser View
        public ActionResult GetModuleForUser()
        {
            IEnumerable<Module> lstModule = new List<Module>();
            ModuleRepository ObjModuleRR = new ModuleRepository();
            Login ObjLogin = (Login)Session["LoginUser"];

            ObjModuleRR.GetModuleByRole(ObjLogin.Role.RoleId, ObjLogin.PartyType);
            if (ObjModuleRR.DBResponse.Data != null)
            {
                lstModule = (IEnumerable<Module>)ObjModuleRR.DBResponse.Data;
            }

            return PartialView("ModuleListForUser", lstModule);
        }

        #endregion

        #endregion
    }
}