using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_OnlinePaymentAgainstInvoice
    {

        public decimal TransId { get; set; }
        public long OrderId { get; set; }
        public int OnlinePayAckId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Total { get; set; }
        public decimal OnlineFacilitaionCharge { get; set; }
        public decimal TotalPayAmount { get; set; }
        public string Response { get; set; } = string.Empty;


        public string lstInvoiceDetails { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DSR_OnlineInvoiceDetails
    {
        public int InvoiceId { get; set; }

        public string InvoiceNo { get; set; }

        public string InvoiceDate { get; set; }

        public decimal Amount { get; set; }

        public string PartyName { get; set; }

    }
}