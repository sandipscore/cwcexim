using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Ppg_CancelInvoice
    {

        [Required(ErrorMessage = "Please Select InvoiceNo")]
        public string InvoiceNo { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceDate { get; set; }
        public string Irn { get; set; }
        public string CancelReason { get; set; }
        [Required(ErrorMessage = "Please Enter Remarks")]
        public string CancelRemarks { get; set; }
        public string CancelDate { get; set; }
        public string SupplyType { get; set; }
    }
}