using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class CartingRegisterDtl
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
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public decimal FoBValue { get; set; }
        public decimal CUM { get; set; }
        public decimal SQM { get; set; }
        public int ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public string LocationDetails { get; set; }
        public string Location { get; set; }//For Location Name
        public int ExporterId { get; set; } = 0;
        public int CommodityId { get; set; } = 0;
        public int CartingRegisterDtlId { get; set; }
        public string StorageType { get; set; }
    }
}