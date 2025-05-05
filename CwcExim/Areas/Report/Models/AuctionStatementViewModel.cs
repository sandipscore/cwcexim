using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class AuctionStatementViewModel
    {
        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }
        public string EntryNo { get; set; }
        public string EntryDate { get; set; }
        public string Obl { get; set; }
        public string ShippingBill { get; set; }
        public string ContainerNo { get; set; }
        public string Commodity { get; set; }
        public string CFSCode { get; set; }

        public string HSNCode { get; set; }

       
        public string Size { get; set; }
        public string InDate { get; set; }
        public string Shed { get; set; }
        public decimal Area { get; set; }
        public decimal Pkg { get; set; }
        public decimal Weight { get; set; }
        public decimal Bidamount { get; set; }
        public decimal valueCharge { get; set; }
        public decimal AuctionCharge { get; set; }
        public decimal MiscCharge { get; set; }

        public decimal CustomDuty { get; set; }
        public decimal CwcShare { get; set; }
        public string Remarks { get; set; }

        public decimal TDSAmount { get; set; }
    }
}