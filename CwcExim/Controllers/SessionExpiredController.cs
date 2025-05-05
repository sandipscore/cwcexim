using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Controllers
{
    public class SessionExpiredController : Controller
    {
        // GET: SessionExpired
        public ActionResult Index()
        {
            return View("SessionExpired");
        }
    }
}