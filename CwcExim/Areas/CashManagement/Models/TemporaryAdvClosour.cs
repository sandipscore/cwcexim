using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.CashManagement.Models
{
    public class TemporaryAdvClosour
    {
        public string Date { get; set; }
        [Display(Name = "Amount In Hand:")]
        public  decimal Amount { get; set; }
        [Display(Name = "Amount To TransFer:")]
        public decimal AmountTrans { get; set; }
    }
}