using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_CartingRegisterDtl
    {
        public int CartingAppDtlId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public string CommInvcNo { get; set; }
        public string Exporter { get; set; }
        public string CargoDescription { get; set; }
        public string CommodityName { get; set; }
        public int CargoType { get; set; }
        public string MarksAndNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int NoOfUnits { get; set; }
        public decimal? Weight { get; set; }
        public decimal FoBValue { get; set; }
        public decimal CUM { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal? SQM { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int? ActualQty { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal? ActualWeight { get; set; }
        public string LocationDetails { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Location { get; set; }//For Location Name
        public int ExporterId { get; set; } = 0;
        public int CommodityId { get; set; } = 0;
        public int CartingRegisterDtlId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Storage { get; set; }
        public int ForwarderId { get; set; }
        public string ForwarderName { get; set; }
    }
}