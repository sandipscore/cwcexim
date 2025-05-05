using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_AcknowledgementViewList
    {
        public string TransId { get; set; }
        public string OrderId { get; set; }

        public decimal TotalPaymentAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal OnlineFacilitationCharges { get; set; }

        public string Response { get; set; }

        public string TransactionDate { get; set; }

        public string BankRef { get; set; }
    }
}