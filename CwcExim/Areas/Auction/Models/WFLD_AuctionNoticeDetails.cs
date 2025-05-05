using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class WFLD_AuctionNoticeDetails
    {
        public int SrNo { get; set; }
        public int NoOfPackages { get; set; }
        public string GodownLocation { get; set; }
        public string BLNo { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string CFSCode { get; set; }
        public string BOENo { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string Weight { get; set; }
        public string CUM { get; set; }
        public string SQM { get; set; }
        public string Duty { get; set; }
        public string Fob { get; set; }
        public string CIF { get; set; }


        public int RefId { get; set; }
        public int GodownID { get; set; }

        public string ShippingLineNo { get; set; }
        public string EntryDate { get; set; }

        public string BLDate { get; set; }

        public string ShipBillDate { get; set; }
        public string IsInsured { get; set; }
        public string LineNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string ContainerNo { get; set; }

        public string CuttingDate { get; set; }
        public string Size { get; set; }
        public string AuctionEligibleDate { get; set; }

        public string OperationType { get; set; }

        public string AuctionType { get; set; }
        public string TSANo { get; set; }

        public string IGM { get; set; }
    }
}