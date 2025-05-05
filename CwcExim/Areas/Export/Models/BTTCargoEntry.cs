using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class BTTCargoEntry
    {  
        public int BTTId { get; set; }

        [DisplayName("BTT No.")]
        public string BTTNo { get; set; }
        
        [DisplayName("BTT Date")]
        public string BTTDate { get; set; }

        public int CartingId { get; set; }

       // [DisplayName("Carting No."), Required(ErrorMessage = "Fill Out This Field")]
        public string CartingNo { get; set; }

        [DisplayName("Carting Date")]
        public string CartingDate { get; set; }

        public int CHAId { get; set; }
        public int ShortCargoDtlId { get; set; }

       // [DisplayName("CHA"),Required(ErrorMessage ="Fill Out This Field")]
        public string CHAName { get; set; }

      //  [DisplayName("Remarks"),Required(ErrorMessage ="Fill Out This Field")]
        public string Remarks { get; set; }
        public IList<BTTCargoEntryDtl> lstBTTCargoEntryDtl { get; set; } = new List<BTTCargoEntryDtl>();
        public string BTTCargoEntryDtlJS { get; set; }
        public IList<BTTCartingList> lstCartingList { get; set; } = new List<BTTCartingList>();
        public string BTTCartingListJS { get; set; }
        public IList<BTTCartingDetailList> lstCartingDetailList { get; set; } = new List<BTTCartingDetailList>();
        public IList<CHAList> lstCHAList { get; set; } = new List<CHAList>();

        public string Flag { get; set; }
    }

    public class BTTCargoEntryDtl
    {
        public int BTTDetailId { get; set; }
        public int BTTId { get; set; }
        public int ShortCargoDtlId { get; set; }
        public int DestuffingEntryDtlId { get; set; }
        public int GodownId { get; set; }
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string GodownName { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CBM { get; set; }
        public int BTTQuantity { get; set; }
        public decimal BTTWeight { get; set; }
        public decimal BTTCBM { get; set; }
        public decimal BTTSQM { get; set; }
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string Remarks { get; set; }
        public decimal ActualSQM { get; set; }
    }
    public class BTTCartingList
    {
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
        public int ShortCargoDtlId { get; set; }
        public string CHAName { get; set; }

        public int CHAId { get; set; }
        public string CartShip { get; set; }

        public string Flag { get; set; }
    }
    public class BTTCartingDetailList
    {
        public int CartingRegisterId { get; set; }
        public int CartingDetailId { get; set; }

        public int DestuffingEntryDtlId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public int ShortCargoDtlId { get; set; }
        public string CommodityName { get; set; }
        public int ChaId { get; set; }
        public int GodownId { get; set; }
        public string ChaName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public string GodownName { get; set; }
        public decimal CBM { get; set; }
        public decimal SQM { get; set; }
    }
    public class CHAList
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
}