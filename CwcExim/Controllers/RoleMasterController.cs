using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
using CwcExim.Repositories;
using CwcExim.Security;
namespace CwcExim.Controllers
{
    //[CustomAuthorize]
    
    public class RoleMasterController : BaseController
    {
        //[Authorize]
        //[ValidateAntiForgeryToken]

        #region AddRole View
        public ActionResult AddRole()
        {
           return PartialView();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddRole(RoleMaster ObjRoleMaster)
        {
            int _Status = 0;
            var _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {
                RoleMasterRepository ObjRR = new RoleMasterRepository();
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjRoleMaster.CreatedBy = ObjLogin.Uid;
                ObjRoleMaster.RoleName = ObjRoleMaster.RoleName.Trim();
                ObjRR.AddEditRoles(ObjRoleMaster);
                _Status = ObjRR.DBResponse.Status;
                _Message = ObjRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion

        #region RoleList View
        public ActionResult GetRoleList()
        {
            RoleMasterRepository ObjRR = new RoleMasterRepository();
            ObjRR.GetAllRoles();
            IEnumerable<RoleMaster> lstRoleMaster = (IEnumerable<RoleMaster>)ObjRR.DBResponse.Data;
            if (lstRoleMaster != null)
            {
                return PartialView("RoleList",lstRoleMaster);
            }
            else
            {
                return PartialView("RoleList", new List<RoleMaster>());
            }           
        }

        #endregion

        #region EditRole View

        public ActionResult EditRole(int id)
        {
            RoleMaster ObjRole = new RoleMaster();
            if(id>0)
            {
                RoleMasterRepository ObjRR = new RoleMasterRepository();
                ObjRR.GetAllRoles(id);
                ObjRole= (RoleMaster)ObjRR.DBResponse.Data;
            }
           
            return PartialView(ObjRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditRole(RoleMaster ObjRole)
        {
            int _Status = 0;
            var _Message = "Please fill the required fields";
            if (ModelState.IsValid)
            {                
                Login ObjLogin = (Login)Session["LoginUser"];
                ObjRole.UpdatedBy = ObjLogin.Uid;
                ObjRole.RoleName = ObjRole.RoleName.Trim();
                RoleMasterRepository ObjRR = new RoleMasterRepository();
                ObjRR.AddEditRoles(ObjRole);
                _Status = ObjRR.DBResponse.Status;
                _Message = ObjRR.DBResponse.Message;
            }
            var Data = new { Status = _Status, Message = _Message };
            return Json(Data);
        }

        #endregion

    }
}