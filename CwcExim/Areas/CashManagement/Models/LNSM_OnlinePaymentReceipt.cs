using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_OnlinePaymentReceipt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string PayerName { get; set; }
        public string PaymentDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string ReferenceNo { get; set; }
        public string PayerRemarks { get; set; }
        public decimal AmountPaid { get; set; }
        public int CashReceiptId { get; set; }

        public decimal OnlineFacCharges { get; set; }

        public decimal PaymentAmount { get; set; }

        public string RefPaymentDate { get; set; }
        public int PayerId { get; set; }
    }
}