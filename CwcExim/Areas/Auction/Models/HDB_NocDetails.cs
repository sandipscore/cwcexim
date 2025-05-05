using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class HDB_NocDetails
    {
        public int? NocID { get; set; }
        public string NocNo { get; set; }

        public string AuctionDestruction { get; set; }
        public int Flag { get; set; }
        public string Type { get; set; }

        [Required]
        public string RefNo { get; set; }

        public string RefDate { get; set; }

        public string NocDate { get; set; }

       
        public string Remarks1 { get; set; }
       
        public string Remarks2 { get; set; }
    }
}