using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_SDStatement
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

        public List<Hdb_SDList> LstSD { get; set; } = new List<Hdb_SDList>();
    }

    public class Hdb_SDList
    {
        public string PartyName { get; set; }
        public string SDAmount { get; set; }
        public string UnpaidAmount { get; set; }
        public string BalanceAmount { get; set; }
         
        public string DepositorName { get; set; }
        public decimal Amount { get; set; } = 0M;
        public decimal AdjustAmount { get; set; }
        public decimal UtilizationAmount { get; set; }
        public decimal RefundAmount { get; set; }
    }
}