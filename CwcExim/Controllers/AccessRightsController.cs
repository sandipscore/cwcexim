using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;

namespace CwcExim.Controllers
{
    public class AccessRightsController : Controller
    {
       
        // GET: AccessRights
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load()
        {
            AccessRightsVM ObjAccessRightsVM = new AccessRightsVM();
            RoleMasterRepository ObjRoleRR = new RoleMasterRepository();
            Login ObjLogin = (Login)Session["LoginUser"];
            
            //ObjRoleRR.GetAllRoles();
            ObjRoleRR.GetAllRolesForAssignments(Convert.ToInt32(ObjLogin.Role.RoleId));
            if (ObjRoleRR.DBResponse!=null)
            {
                ObjAccessRightsVM.RoleList = (IEnumerable<RoleMaster>)ObjRoleRR.DBResponse.Data;
            }
            ModuleRepository objModuleRR = new ModuleRepository();
            objModuleRR.GetModuleForAccessRights();
            if (objModuleRR.DBResponse!=null)
            {
                ObjAccessRightsVM.ModuleList = (IEnumerable<Module>)objModuleRR.DBResponse.Data;
            } 
            return View(ObjAccessRightsVM);
        }

        public ActionResult AccessRightsList(int RoleId, int ModuleId) //,int PartyType)
        {
            AccessRightsVM ObjRightsVM = new AccessRightsVM();
            ObjRightsVM.lstAccessRights = new List<AccessRights>();
            ObjRightsVM.ModuleId = ModuleId;
            ObjRightsVM.RoleId = RoleId;
           // ObjRightsVM.Ptype = PartyType;
            if (ModelState.IsValid)
            {
                AccessRightsRepository ObjAccessRightsRR = new AccessRightsRepository();
                ObjAccessRightsRR.GetAccessRights(ModuleId, RoleId);
                //ObjAccessRightsRR.GetAccessRights(ModuleId, RoleId, PartyType);
                if (ObjAccessRightsRR.DBResponse.Data != null)
                {
                    ObjRightsVM.lstAccessRights = (List<AccessRights>)ObjAccessRightsRR.DBResponse.Data;
                }
            }
            return View(ObjRightsVM);
        }

        [HttpPost]
        public ActionResult AccessRightsList(AccessRightsVM ObjAccessRightsVM)
        {
            int _Status = 0;
            if (ObjAccessRightsVM.lstAccessRights != null)
            {
                string strXML = CwcExim.UtilityClasses.Utility.CreateXML(ObjAccessRightsVM.lstAccessRights);
                AccessRightsRepository ObjAccessRightsRR = new AccessRightsRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                //ObjAccessRightsRR.AddEditAccessRights(strXML, ObjAccessRightsVM.ModuleId, ObjAccessRightsVM.RoleId, ObjAccessRightsVM.Ptype, ObjLogin.Uid);
                ObjAccessRightsRR.AddEditAccessRights(strXML, ObjAccessRightsVM.ModuleId, ObjAccessRightsVM.RoleId,ObjLogin.Uid);
                _Status = ObjAccessRightsRR.DBResponse.Status;
            }
            var Data = new { Status = _Status };
            return Json(Data);
            
        }
    }
}