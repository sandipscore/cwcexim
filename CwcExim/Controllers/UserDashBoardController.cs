using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Security;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class UserDashBoardController : BaseController
    {
        #region MenuId Assign
        public string SetMenuId(int MenuId, int ModuleId, string ModuleName)
        {
            Session["MenuId"] = MenuId;
            Session["ModuleId"] = ModuleId;
            Session["ModuleName"] = ModuleName;
            return "success";
        }

        #endregion

        #region User Dashboard
        public ActionResult Index()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            ViewBag.FirstLogin = ObjLogin.FirstLogin;
            EximDashboardMenu objEximMenu = new EximDashboardMenu();
            try
            {
                UserDashBoardRepository ObjMenuRR = new UserDashBoardRepository();
                ObjMenuRR.GetDashboardMenuForUser(ObjLogin.Role.RoleId, ObjLogin.PartyType);

                if (ObjMenuRR.DBResponse.Data != null)
                {
                    objEximMenu = (EximDashboardMenu)ObjMenuRR.DBResponse.Data;
                }
                return View("UserDashBoard", objEximMenu);
            }
            catch (Exception)
            {
                return View("UserDashBoard", objEximMenu);
            }
           
        }


        [HttpGet]
        public JsonResult GetLastMonthVolume()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            UserDashBoardRepository objDBR = new UserDashBoardRepository();
            objDBR.GetLastMonthVolume(ObjLogin.Uid);
            return Json(objDBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTopFiveContributor()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            UserDashBoardRepository objDBR = new UserDashBoardRepository();
            objDBR.GetTopFiveContributor(ObjLogin.Uid);
            return Json(objDBR.DBResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLastSixmonthCollection()
        {
            Login ObjLogin = (Login)Session["LoginUser"];
            UserDashBoardRepository objDBR = new UserDashBoardRepository();
            objDBR.GetLastSixmonthCollection(ObjLogin.Uid);
            return Json(objDBR.DBResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}