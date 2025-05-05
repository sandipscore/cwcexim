using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSRBTTCargoEntry
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

        //[DisplayName("Remarks"),Required(ErrorMessage ="Fill Out This Field")]
        public string Remarks { get; set; }
        public IList<DSRBTTCargoEntryDtl> lstBTTCargoEntryDtl { get; set; } = new List<DSRBTTCargoEntryDtl>();
        public string BTTCargoEntryDtlJS { get; set; }
        public IList<DSRBTTCartingList> lstCartingList { get; set; } = new List<DSRBTTCartingList>();
        public string BTTCartingListJS { get; set; }
        public IList<DSRBTTCartingDetailList> lstCartingDetailList { get; set; } = new List<DSRBTTCartingDetailList>();
        public IList<DSRCHAList> lstCHAList { get; set; } = new List<DSRCHAList>();
    }

    public class DSRBTTCargoEntryDtl
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
        public decimal ManualWT { get; set; }
        public decimal MechanicalWT { get; set; }
        public decimal FOBValue { get; set; }
        public decimal SQM { get; set; }
        public decimal TFOBValue { get; set; }
    }
    public class DSRBTTCartingList
    {
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
    }
    public class DSRBTTCartingDetailList
    {
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal FOBValue { get; set; }
    }
    public class DSRCHAList
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
}