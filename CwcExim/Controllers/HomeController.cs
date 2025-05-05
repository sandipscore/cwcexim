using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Models;
namespace CwcExim.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Session["ModuleName"] = "";
            int RoleId = 0;
            if (HttpContext.Session["LoginUser"] == null)
                return Redirect(ConfigurationManager.AppSettings["MainDomainUrl"].ToString());
            else
            {
                //RoleId = ((Login)HttpContext.Session["LoginUser"]).Role.RoleId;
                //if (RoleId == 1 || RoleId == 2)
                //    return RedirectToAction("Index", "Admin");
                //else
                return RedirectToAction("Index", "UserDashBoard");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}