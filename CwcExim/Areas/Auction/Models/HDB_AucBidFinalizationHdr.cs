using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class HDB_AucBidFinalizationHdr
    {
        public int? BidIdHdr { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string BidDate { get; set; }

        public string BidNo { get; set; }
        public int PartyId { get; set; }

        public string RefNo { get; set; }
        public string RefDate { get; set; }

        public int RefFlag { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Party { get; set; }
        public string Address { get; set; }
        public string GstNo { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public Decimal? BidAmount { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
        public Decimal? EmdAmount { get; set; }
        public string OBL { get; set; }
        public string Shipbill { get; set; }

        public string Container { get; set; }
        public int uid { get; set; }

        public int BranchId { get; set; }
        public string StrTableDtls { get; set; }

        public string AuctionDate { get; set; }

        public string PaymentMode { get; set; }


        public string DraweeBank { get; set; }


        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal? Amount { get; set; }
        public string EmdReceiptNo { get; set; }

        public string CFSCode { get; set; }

        public int CommodityID { get; set; }

        public Decimal? Fob { get; set; }
        public Decimal? Duty { get; set; }
        public Decimal? CIF { get; set; }
        public Decimal? Area { get; set; }
        public int? GodownID { get; set; }
        public int? ImporterID { get; set; }
        public int? ShippingId { get; set; }
        public Decimal? Weight { get; set; }
        public int? Refid { get; set; }
        public int? Noofpkg { get; set; }

        public string EntryDate { get; set; }
        public string Size { get; set; }
    }
    public class HDB_PartyDetails
    {
        public int AuctionId { get; set; }

        public string AuctionNumber { get; set; }
        public int PartyId { get; set; }

        public string Party { get; set; }
        public string Address { get; set; }
        public string GstNo { get; set; }
        public string Pan { get; set; }
    }

}