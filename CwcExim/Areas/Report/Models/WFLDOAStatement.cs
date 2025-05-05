using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDOAStatement
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int Year { get; set; }
        public decimal TotalAmount { get; set; } = 0M;
        public decimal OpeningAmount { get; set; } = 0M;
        public decimal Collections { get; set; } = 0M;
        public decimal Adjustment { get; set; } = 0M;

        public decimal closingAmount { get; set; } = 0M;

        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string Party { get; set; }
        public int PartyId { get; set; }
        public List<WFLDOAList> LstOnAccount { get; set; } = new List<WFLDOAList>();
    }

    public class WFLDOAList
    {
        public string PartyName { get; set; }
        public decimal OpeningAmount { get; set; }
        //public string UnpaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }

        public string DepositorName { get; set; }
        public decimal Amount { get; set; } = 0M;

        public decimal AdjustAmount { get; set; }
        public decimal ReceiptAmount { get; set; }
        //public decimal RefundAmount { get; set; }
    }
    


}