using CwcExim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.CashManagement.Controllers
{
    public class PaymentSheetController : BaseController
    {
        [HttpGet]
        public ActionResult PyamentInvoice()
        {
            return PartialView();
        }
    }
}