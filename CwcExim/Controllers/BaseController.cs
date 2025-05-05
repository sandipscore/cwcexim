using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CwcExim.Security;
namespace CwcExim.Controllers
{
    //[OutputCache(VaryByParam = "none", Duration = 0, NoStore = true)]
    public class BaseController : Controller
    {

        // GET: Base
        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        
{
            string StrUrl = filterContext.HttpContext.Request.RawUrl;
            //string SubDomain = System.Configuration.ConfigurationManager.AppSettings["SubDomain"]; + SubDomain
            //if (filterContext.HttpContext.Session["LoginUser"] == null && (StrUrl != "/" 
            //    && StrUrl != "/User/Login" && StrUrl != "/User/SignUp" &&
            //    !(filterContext.HttpContext.Request.Path == "/User/SignUp" && filterContext.HttpContext.Request.QueryString["LoginWithId"] != null)
            //    && StrUrl != "/User/GenerateCodeForMobile" && StrUrl != "/User/GenerateCodeForEmail" && StrUrl != "/User/UserCreation" && StrUrl != "/User/ForgotPassword" && StrUrl != "/User/CheckUser" && StrUrl != "/User/GeneratePassword" && StrUrl != "/Faqs" && StrUrl != "/Home/About" && StrUrl != "/Home/Contact" && StrUrl != "/Home" && StrUrl != "/MainDashBoard/Index"))
            //{
            //    //filterContext.Result = new RedirectResult("~/User/Login");
            //    //filterContext.Result = new RedirectResult("~/");
            //    filterContext.Result = new RedirectResult("/SessionExpired/Index");
            //    return;


            //}
            if (filterContext.HttpContext.Session["LoginUser"] == null && (StrUrl != "/"
                && StrUrl != "/User/Login" && StrUrl != "/User/SignUp" &&
                !(filterContext.HttpContext.Request.Path == "/User/SignUp" && filterContext.HttpContext.Request.QueryString["LoginWithId"] != null)
                && !(filterContext.HttpContext.Request.Path == "/MainDashBoard/Index" && filterContext.HttpContext.Request.QueryString["BranchId"] != null)
                && StrUrl!="/MainDashBoard/LoadCompany"
                && StrUrl != "/User/GenerateCodeForMobile" && StrUrl != "/User/GenerateCodeForEmail" && StrUrl != "/User/UserCreation" && StrUrl != "/User/ForgotPassword"
                 && StrUrl != "/User/CheckUser" && StrUrl != "/User/GeneratePassword" && StrUrl != "/Faqs" && StrUrl != "/Home/About" && StrUrl != "/Home/Contact" 
                 && StrUrl != "/Home" && StrUrl != "/Home/Index"))
            {
                //filterContext.Result = new RedirectResult("~/User/Login");
                //filterContext.Result = new RedirectResult("~/");
                filterContext.Result = new RedirectResult("/SessionExpired/Index");
                return;


            }
            base.OnActionExecuting(filterContext);


            
        }



        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}