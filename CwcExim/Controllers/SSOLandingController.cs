using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Controllers
{
    public class SSOLandingController : Controller
    {
        // GET: SSOLanding
        public ActionResult Index(int BranchId)
        {
            return View();
        }
    }
}