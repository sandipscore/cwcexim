using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class HDB_AucBidFinalizationDtl
    {
        public int BidFinalizationID { get; set; }
        public int NoticeID { get; set; }

        public string NoticeNumber { get; set; }
        public string Size { get; set; }
        public string ContainerNo { get; set; }
        public string Boe { get; set; }
        public string Commodity { get; set; }

        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string BOLNo { get; set; }

        public string BolDate { get; set; }
        public string ShipBillNo { get; set; }

        public string ShipBillDate { get; set; }
        public Decimal? Weight { get; set; }

        public string cum { get; set; }
        public string sqm { get; set; }

        public string CFSCode { get; set; }

        public int CommodityID { get; set; }

        public Decimal? Fob { get; set; }
        public Decimal? Duty { get; set; }
        public Decimal? CIF { get; set; }
        public Decimal? Area { get; set; }

        public int GodownID { get; set; }
        public int ImporterID { get; set; }
        public int ShippingId { get; set; }

        public int Refid { get; set; }
        public int Noofpkg { get; set; }

        public string EntryDate { get; set; }
    }
}