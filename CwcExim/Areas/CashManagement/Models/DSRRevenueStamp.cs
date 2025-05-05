using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSRRevenueStamp
    {
        public int RSPurchaseId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PurchaseDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal PurchaseAmount { get; set; }
        public int PurchaseUnit { get; set; }
        public int Uid { get; set; }
        public string Operation { get; set; }
    }
}