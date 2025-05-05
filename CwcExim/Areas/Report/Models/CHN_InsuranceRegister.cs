using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_InsuranceRegister
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string Format { get; set; }
        public string Date { get; set; }
        public string ContainerNo { get; set; }
        public string TruckNo { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal ReceivedWeight { get; set; }
        public decimal DeliveredQty { get; set; }
        public decimal DeliveredWeight { get; set; }
        public decimal BalanceQty { get; set; }
        public decimal BalanceWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public string Remarks { get; set; }
    }
}