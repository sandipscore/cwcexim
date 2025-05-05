using CwcExim.Areas.CashManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Auction.Models
{
    public class Dnd_EMDReceived
    {
        public int EMDReceivedId { get; set; }
        public string EMDReceivedNo { get; set; }
        public int BIDId { get; set; }

        [Required(ErrorMessage = "Please Choose BID No.")]
        public string BIDNo { get; set; }
        public string BIDDate { get; set; }
        public string BIDAmount { get; set; }
        public string EMDAmount { get; set; }
        public int AuctionNoticeId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public Decimal? AdvancePaid { get; set; }
        public string BidderID { get; set; }
        public string BidderName { get; set; }

        public string OBL { get; set; }

        public string ShippBillNo { get; set; }

        public string Container { get; set; }
        public string ExtraPrice { get; set; }
        public string PartyAddress { get; set; }
        public string ReceiptDate { get; set; }
        public string ValidUpTo { get; set; }

        public string AuctionDate { get; set; }
        public List<Dnd_BIDInfo> BIDDetail { get; set; } = new List<Dnd_BIDInfo>();
        public IList<Dnd_CashReceipt> CashReceiptDetail { get; set; } = new List<Dnd_CashReceipt>();
    }

    public class Dnd_BIDInfo
    {
        public int BIDId { get; set; }
        public string BIDNo { get; set; }
    }

    public class Dnd_CashReceipt
    {
        //:: Cash Mode if pda no selected ::::::
        public string PaymentMode { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Invalid Character.")]
        [StringLength(maximumLength: 45, ErrorMessage = "Contain Only 45 Character.")]
        public string DraweeBank { get; set; }

        [RegularExpression("^[a-zA-Z0-9/ ]*$", ErrorMessage = "Invalid Character.")]
        [StringLength(maximumLength: 45, ErrorMessage = "Contain Only 45 Character.")]
        public string InstrumentNo { get; set; }
        public string Date { get; set; }

        [RegularExpression("^[0-9.]*$", ErrorMessage = "Invalid Character.")]
        public decimal? Amount { get; set; }
        //::::::::::::::::::::::::::::::::::::
    }

}