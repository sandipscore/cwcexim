using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_DirectOnlinePayment
    {
        public decimal TransId { get; set; }
        public long OrderId { get; set; }
        public int OnlinePayAckId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal PaymentAmount { get; set; }
        public decimal OnlineFacilitationCharges { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public string Response { get; set; } = string.Empty;
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Area { get; set; }
        public decimal TDS { get; set; }
        public string Name { get; set; }
    }
}