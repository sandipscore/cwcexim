using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_BTTCargoEntry
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

        
        public string Remarks { get; set; }
        public string IsPrinting { get; set; }
        public int NoOfPrint { get; set; }
        public IList<Hdb_BTTCargoEntryDtl> lstBTTCargoEntryDtl { get; set; } = new List<Hdb_BTTCargoEntryDtl>();
        public string BTTCargoEntryDtlJS { get; set; }
        public IList<Hdb_BTTCartingList> lstCartingList { get; set; } = new List<Hdb_BTTCartingList>();
        public string BTTCartingListJS { get; set; }
        public IList<Hdb_BTTCartingDetailList> lstCartingDetailList { get; set; } = new List<Hdb_BTTCartingDetailList>();
        public IList<Hdb_CHAList> lstCHAList { get; set; } = new List<Hdb_CHAList>();


        public string ExporterImporterName { get; set; }
        public decimal StorageAmt { get; set; }
        public decimal HTTotalAmt { get; set; }
        public decimal OthersAmt { get; set; }
        public decimal Total { get; set; }
        public string CrNo { get; set; }
        public string CrDate { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }

    }

    public class Hdb_BTTCargoEntryDtl
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
        public decimal InFob { get; set; }
        public decimal Fob { get; set; }
        public decimal Sqm { get; set; }
        public string CartingAppNo { get; set; }
        public string CartingDate { get; set; }
        

    }
    public class Hdb_BTTCartingList
    {
        public int CartingId { get; set; }
        public string CartingNo { get; set; }
        public string CartingDate { get; set; }
    }
    public class Hdb_BTTCartingDetailList
    {
        public int CartingDetailId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string CargoDescription { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
    }
    public class Hdb_CHAList
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
    }
}