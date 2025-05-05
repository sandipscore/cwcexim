using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class GodownRightsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Load()
        {
            GodownRight ObjRights = new GodownRight();
            RoleMasterRepository ObjRoleRR = new RoleMasterRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            GodownRightsRepository ObjUserRR = new GodownRightsRepository();
            ObjUserRR.GetAllUser(-2);
            if (ObjUserRR.DBResponse != null)
            {
                ObjRights.lstUser = (IEnumerable<User>)ObjUserRR.DBResponse.Data;
            }
            return View(ObjRights);

           
        }
        [HttpGet]
        public ActionResult GodownRightsList(int UserId) //,int PartyType)
        {
            GodownRight ObjRights = new GodownRight();
            ObjRights.lstGodown = new List<GodownRights>();
            ObjRights.UserId = UserId;
            GodownRight.UId = UserId;
            // ObjRightsVM.Ptype = PartyType;
            if (ModelState.IsValid)
            {
                GodownRightsRepository ObjGodownRightsRR = new GodownRightsRepository();
                ObjGodownRightsRR.GetGodownRights(UserId);
                //ObjAccessRightsRR.GetAccessRights(ModuleId, RoleId, PartyType);
                if (ObjGodownRightsRR.DBResponse.Data != null)
                {
                    ObjRights.lstGodown = (List<GodownRights>)ObjGodownRightsRR.DBResponse.Data;
                }
            }
            return View(ObjRights);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditGodownRightsList(GodownRight ObjAccessRightsVM)
        {
            int _Status = 0;
            UserRepository ObjUserRR = new UserRepository();
          
            if (ObjAccessRightsVM.lstGodown != null)
            {
                string strXML = CwcExim.UtilityClasses.Utility.CreateXML(ObjAccessRightsVM.lstGodown);
                GodownRightsRepository ObjGodownRightsRR = new GodownRightsRepository();
              //  ViewBag.lstUser= (IEnumerable<User>)ObjUserRR.DBResponse.Data;
                GodownRight ObjRights = new GodownRight();
               
                Login ObjLogin = (Login)Session["LoginUser"];
               
                //ObjAccessRightsRR.AddEditAccessRights(strXML, ObjAccessRightsVM.ModuleId, ObjAccessRightsVM.RoleId, ObjAccessRightsVM.Ptype, ObjLogin.Uid);
                ObjGodownRightsRR.AddEditGodownRights(strXML, GodownRight.UId);
                _Status = ObjGodownRightsRR.DBResponse.Status;
            }
            var Data = new { Status = _Status };
            return Json(Data);

        }


    }
}