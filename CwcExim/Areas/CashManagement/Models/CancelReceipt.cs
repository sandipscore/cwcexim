using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.CashManagement.Models
{
    public class CancelReceipt
    {
        public int ReceiptId{ get; set; }
        public string CancelledReason { get; set; }
    }
    public class CancelReceiptList
    {
        public int ReceiptId { get; set; }
        [Display(Name = "Receipt No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Cancelled On")]
        public string CancelledOn { get; set; }
        [Display(Name = "Cancelled Reason")]
        public string CancelledReason { get; set; }
    }
}