using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.Report.Controllers
{
    public class TelerikReportController : Controller
    {
        // GET: Report/TelerikReport
        public ActionResult DayToDayDestuffingReport(string FromDt,string ToDt)
        {

            ViewBag.FromDt = FromDt;
            ViewBag.ToDt = ToDt;

            return View();
        }
    }
}