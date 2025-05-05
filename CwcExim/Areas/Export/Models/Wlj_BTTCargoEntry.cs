using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Wlj_BTTCargoEntry
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

        [DisplayName("CHA"), Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }

        [DisplayName("Remarks"), Required(ErrorMessage = "Fill Out This Field")]
        public string Remarks { get; set; }
        public IList<Wlj_BTTCargoEntryDtl> lstBTTCargoEntryDtl { get; set; } = new List<Wlj_BTTCargoEntryDtl>();
        public string BTTCargoEntryDtlJS { get; set; }
        public IList<Wlj_BTTCartingList> lstCartingList { get; set; } = new List<Wlj_BTTCartingList>();
        public string BTTCartingListJS { get; set; }
        public IList<Wlj_BTTCartingDetailList> lstCartingDetailList { get; set; } = new List<Wlj_BTTCartingDetailList>();
        public IList<CHAList> lstCHAList { get; set; } = new List<CHAList>();
    }

    public class Wlj_BTTCargoEntryDtl
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
    }
    public class Wlj_BTTCartingList
    {
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
    }
    public class Wlj_BTTCartingDetailList
    {
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
    }
}