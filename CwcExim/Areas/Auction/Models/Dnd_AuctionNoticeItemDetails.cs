using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_AuctionNoticeItemDetails
    {
        public int AuctionNoticeDtlId { get; set; }
        public string GodownLocation { get; set; }

        public string BLDate { get; set; }

        public string ShipBillDate { get; set; }
        public string BLNo { get; set; }
        public int NoOfPackages { get; set; }
        public string AuctionNoticeDate { get; set; }
        public string EntryDate { get; set; }
        public string CommodityName { get; set; }
        public string PartyName { get; set; }
        public string NoticeNo { get; set; }
        public string AuctionEligibleDate { get; set; }
        public int CommodityId { get; set; }
        public string Weight { get; set; }
        public string CUM { get; set; }
        public string SQM { get; set; }
        public string Duty { get; set; }
        public string Fob { get; set; }
        public string CIF { get; set; }
        public string IsInsured { get; set; }
        public string BOENo { get; set; }
        public string LineNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string CuttingDate { get; set; }
        public int GodownID { get; set; }
        public int RefId { get; set; }
    }

    public class Dnd_SavedAuctionNotice
    {
        public int AuctionNoticeDtlId { get; set; }
        public string NoticeNo { get; set; }
        public string AuctionNoticeDate { get; set; }
        public string PartyName { get; set; }
        public string Type { get; set; }
        public List<Dnd_SavedAuctionNoticeDetails> SavedAuctionNoticeDetailsList { get; set; }
    }

    public class Dnd_SavedAuctionNoticeDetails
    {
        public string CommodityName { get; set; }
        public string BOENo { get; set; }
        public string BOLDate { get; set; }
        public string ShipBillNo { get; set; }
        public string ShipBillDate { get; set; }
        public string ContainerNo { get; set; }
    }

    public class Dnd_AuctionNoticePrintViewModel
    {
        public string AuctionNoticeNo { get; set; }
        public string DocNo { get; set; }
        public string AuctionNoticeDocNo { get; set; }
        public string CompanyAddress { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string NoticeDate { get; set; }

        public string Shippingline { get; set; }

        public string ShippinglineAddress { get; set; }
        public string OperationType { get; set; }

        public string AuctionType { get; set; }
        public string AucNtcDaysAftrLanding { get; set; }
        public string DueDaysAftrAucNtc { get; set; }
        public string GodownLocation { get; set; }
        public string AuctionNoticeCC { get; set; }
        public string CommodityName { get; set; }

        public List<Dnd_ParticularsOfGoods> ParticularsOfGoodsList { get; set; }
    }

    public class Dnd_ParticularsOfGoods
    {
        public string ContainerNo { get; set; }
        public string ICDCode { get; set; }
        public string EntryDate { get; set; }
        public string BLNo { get; set; }
        public string CartingDate { get; set; }
        public string ShipBillNo { get; set; }
        public string NoOfPackages { get; set; }
        public string Weight { get; set; }
        public string ItemNo { get; set; }
    }
}