using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_OnlinePaymentDetails
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string BankRefNo { get; set; }
        public string TypeOfPayment { get; set; }
        public decimal Amount { get; set; }
        public string PartyName { get; set; }

        public string PaymentDate { get; set; }

    }
}