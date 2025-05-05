using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DSRPartyWiseTDSDeposit
    {
        public int Id { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CirtificateNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CirtificateDate { get; set; }
        public string ReceiptDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public decimal Amount { get; set; } 
        public string DepositDate { get; set; }
        public decimal TDSBalance { get; set; } 
        public string IsCan { get; set; }
        public int FinancialYear { get; set; } = 0;
        public int FinancialYearNext { get; set; } = 0;
        public string TdsQuarter { get; set; } = "";
        public string PeriodTo { get; set; } = "";
        public string PeriodFrom { get; set; } = "";
        public string Remarks { get; set; } = "";
        public string ReceiptNo { get; set; }
    }
}