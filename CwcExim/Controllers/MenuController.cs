using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    [RoutePrefix("Menu")]
    public class MenuController : BaseController
    {
        #region CreateMenu View
        public ActionResult Load()
        {
            Menu ObjMenu = new Menu();
            ObjMenu.ParentMenuList = new List<Menu>();
            ObjMenu.ModuleList= new List<Module>();
            Module ObjModule = new Module();
            ModuleRepository objMRR = new ModuleRepository();
            objMRR.GetAllModule();
            if (objMRR.DBResponse.Data != null)
            {
                ObjMenu.ModuleList = (IEnumerable<Module>)objMRR.DBResponse.Data;
            }
            MenuRepository ObjMenuRR = new MenuRepository();
            ObjMenuRR.GetAllMenu();
            if(ObjMenuRR.DBResponse.Data!=null)
            {    
                ObjMenu.ParentMenuList = (IEnumerable<Menu>)ObjMenuRR.DBResponse.Data;
            }
            return PartialView("CreateMenu",ObjMenu);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateMenu(Menu ObjMenu)
        {
            int _Status = 0;
            var _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjMenu.CreatedBy = ObjLogin.Uid;
                ObjMenu.MenuName = ObjMenu.MenuName.Trim();
                ObjMenu.ParentMenuId = (ObjMenu.ParentMenuId == null ? 0 : ObjMenu.ParentMenuId);
                MenuRepository ObjMenuRR = new MenuRepository();
                ObjMenuRR.AddEditMenu(ObjMenu);
                ModelState.Clear();
                _Status = ObjMenuRR.DBResponse.Status;
                _Message = ObjMenuRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);

        }

        #endregion

        #region MenuList View
        public ActionResult GetMenuList()
        {
            MenuRepository ObjMenuRR = new MenuRepository();
            ObjMenuRR.GetAllMenu();
            IEnumerable<Menu> lstMenu = new List<Menu>();
            if(ObjMenuRR.DBResponse.Data!=null)
            {
               lstMenu = (IEnumerable<Menu>)ObjMenuRR.DBResponse.Data;
            }
            return PartialView("MenuList",lstMenu);
        }

        #endregion

        #region EditMenu View
        public ActionResult EditMenu(int MenuId)
        {
            Menu ObjMenu = new Menu();
            ObjMenu.ParentMenuList = new List<Menu>();
            ObjMenu.ModuleList = new List<Module>();
            if (MenuId > 0)
            {
                MenuRepository ObjMenuRR = new MenuRepository();
                ObjMenuRR.GetMenuByMenuID(MenuId);
                if (ObjMenuRR.DBResponse.Data != null)
                {
                    ObjMenu = (Menu)ObjMenuRR.DBResponse.Data;
                    Module ObjModule = new Module();
                    ModuleRepository objMRR = new ModuleRepository();
                    objMRR.GetAllModule();
                    if (objMRR.DBResponse.Data != null)
                    {
                        ObjMenu.ModuleList = (IEnumerable<Module>)objMRR.DBResponse.Data;
                    }
                    ObjMenuRR.GetAllMenu();
                    if (ObjMenuRR.DBResponse.Data != null)
                    {
                        ObjMenu.ParentMenuList = (IEnumerable<Menu>)ObjMenuRR.DBResponse.Data;
                    }
                }
            }

            return PartialView(ObjMenu);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditMenu(Menu ObjMenu)
        {
            int _Status = 0;
            var _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjMenu.UpdatedBy = ObjLogin.Uid;
                ObjMenu.MenuName = ObjMenu.MenuName.Trim();
                MenuRepository ObjMenuRR = new MenuRepository();
                ObjMenuRR.AddEditMenu(ObjMenu);
                _Status = ObjMenuRR.DBResponse.Status;
                _Message = ObjMenuRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion

        #region UserDashboard
        //[Route("Menu/GetMenuForUser/{ModuleId?}")]
        [Route("ForUser/{EncodedModuleId}")]
        public ActionResult GetMenuForUser(string EncodedModuleId)//Base 64 string for module id  TW9kdWxlSWQ
        {
            //string ParamVal = ModuleId.Substring(1, ModuleId.Length - 1);//remove extra character
            IEnumerable<Menu> lstMenu = new List<Menu>();
            try
            {
                byte[] Data = Convert.FromBase64String(EncodedModuleId);
                string DecodedString = Encoding.UTF8.GetString(Data);
                string ModuleId = DecodedString.Split('_')[0];
                string ModuleName= DecodedString.Split('_')[1];
                Login ObjLogin = (Login)Session["LoginUser"];
                Session["ModuleId"] = ModuleId;
                Session["ModuleName"] = ModuleName;
                MenuRepository ObjMenuRR = new MenuRepository();
                ObjMenuRR.GetMenuByRole(Convert.ToInt32(ModuleId), ObjLogin.Role.RoleId, ObjLogin.PartyType);

                if (ObjMenuRR.DBResponse.Data != null)
                {
                    lstMenu = (IEnumerable<Menu>)ObjMenuRR.DBResponse.Data;
                }
                return View("MenuForUser", lstMenu);
            }
            catch (Exception ex)
            {
                return View("MenuForUser", lstMenu);
            }
        }
        #endregion
    }
}