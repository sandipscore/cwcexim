using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Repositories;
using CwcExim.Models;
namespace CwcExim.Controllers
{
   
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            // (Login)Session["LoginUser"]
            Login LoginUser = (Login)HttpContext.Session["LoginUser"];
            MenuRepository Mr = new MenuRepository();
            Mr.GetMenu(LoginUser.Role.RoleId);
            IEnumerable<Menu> Menu = (IEnumerable<Menu>)Mr.DBResponse.Data;
            if (Menu == null)
                Menu = new List<Menu>();
            return View("AdminDashBoard", Menu);
        }
    }
}