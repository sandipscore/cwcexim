using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class AuctionCashBookViewModel
    {
        [Required]
        public string ToDate { get; set; }

        [Required]
        public string FromDate { get; set; }
        
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }

        public string BidderName { get; set; }

        public string ChqDDUTRNo { get; set; }

        public string BidNo { get; set; }
        public decimal BidAmount { get; set; }

        public decimal EmdAmount { get; set; }
        public decimal AdvanceAmountPaid { get; set; }
        public decimal AdvanceAmountAdjust { get; set; }

        public decimal TotalGST { get; set; }
        public decimal NetAmount { get; set; }

        public decimal IGSTAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }

        public decimal TotalPayable { get; set; }

        public decimal AuctionCharges { get; set; }

        public decimal TDS { get; set; }


        public decimal EMDAmountAdjust { get; set; }
        public decimal TotalPaid { get; set; }

        //
    }
}