using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class MarkForNotice
    {
        public int AuctionNoticeId { get; set; }
        public string NoticeNo { get; set; }
        public string AuctionNoticeDate { get; set; }
        public string PartyName { get; set; }
        public List<MarkForNoticeDetails> MarkForNoticeDetailsList { get; set; }
    }

    public class MarkForNoticeDetails
    {
        public int AuctionNoticeDtlId { get; set; }
        public string AuctionNoticeDate { get; set; }
        public string CommodityName { get; set; }
        public string Weight { get; set; }
    }

    public class MarkedNotice
    {
        public string LOTNo { get; set; }
        public string MarkedOn { get; set; }  
        public List<MarkedNoticeDetails> MarkedNoticeDetailsList { get; set; }
    }

    public class MarkedNoticeDetails 
    {
        public int AuctionNoticeId { get; set; }
        public string NoticeNo { get; set; }
        public string AuctionNoticeDate { get; set; }
        public string PartyName { get; set; }
        public string OBL { get; set; }

        public string Shipbill { get; set; }



    }
}