using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_BTTCargoEntry
    {
        public int BTTId { get; set; }

        [DisplayName("BTT No.")]
        public string BTTNo { get; set; }
        
        [DisplayName("BTT Date")]
        public string BTTDate { get; set; }

        public int CartingId { get; set; }

        [DisplayName("Carting No."), Required(ErrorMessage = "Fill Out This Field")]
        public string CartingNo { get; set; }

        [DisplayName("Carting Date")]
        public string CartingDate { get; set; }

        public int CHAId { get; set; }

        [DisplayName("CHA"),Required(ErrorMessage ="Fill Out This Field")]
        public string CHAName { get; set; }

        [DisplayName("Remarks"),Required(ErrorMessage ="Fill Out This Field")]
        public string Remarks { get; set; }
        public IList<Dnd_BTTCargoEntryDtl> lstBTTCargoEntryDtl { get; set; } = new List<Dnd_BTTCargoEntryDtl>();
        public string Dnd_BTTCargoEntryDtlJS { get; set; }
        public IList<Dnd_BTTCartingList> lstCartingList { get; set; } = new List<Dnd_BTTCartingList>();
        public string Dnd_BTTCartingListJS { get; set; }
        public IList<Dnd_BTTCartingDetailList> lstCartingDetailList { get; set; } = new List<Dnd_BTTCartingDetailList>();
        public IList<Dnd_CHAList> lstCHAList { get; set; } = new List<Dnd_CHAList>();
    }

    public class Dnd_BTTCargoEntryDtl
    {
        public int BTTDetailId { get; set; }
        public int BTTId { get; set; }
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public int BTTQuantity { get; set; }
        public decimal BTTWeight { get; set; }
        public decimal BTTArea { get; set; }
        public decimal Area { get; set; }
        public decimal Fob { get; set; }
    }
    public class Dnd_BTTCartingList
    {
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
    }
    public class Dnd_BTTCartingDetailList
    {
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Area { get; set; }
    }
    public class Dnd_CHAList
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
}