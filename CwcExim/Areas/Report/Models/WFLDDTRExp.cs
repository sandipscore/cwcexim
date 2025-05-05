using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDDTRExp
    {
        public IList<WFLDCartingDetails> lstCartingDetails { get; set; } = new List<WFLDCartingDetails>();
        public IList<WFLDCartingDetails> lstShortCartingDetails { get; set; } = new List<WFLDCartingDetails>();
        public IList<WFLDCargoAcceptingDetails> lstCargoShifting { get; set; } = new List<WFLDCargoAcceptingDetails>();
        public IList<WFLDCargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<WFLDCargoAcceptingDetails>();
        public IList<WFLDBTTDetails> lstBTTDetails { get; set; } = new List<WFLDBTTDetails>();
        public IList<WFLDStuffingDetails> lstStuffingDetails { get; set; } = new List<WFLDStuffingDetails>();
        public IList<WFLDStockDetails> StockOpening { get; set; } =new List<WFLDStockDetails>();
        public IList<WFLDStockDetails> StockClosing { get; set; } = new List<WFLDStockDetails>();
    }
    public class WFLDCartingDetails
    {
        public int ShippingLineId { get; set; }
        public string SLA { get; set; }
        public string ShippingLineName { get; set; }
        public string CFSCode { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CargoDescription { get; set; }
        public decimal ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public decimal FobValue { get; set; }
        public string Slot { get; set; }
        public string GR { get; set; }
        public decimal Area { get; set; }
        public string Remarks { get; set; }
    }
    public class WFLDCargoAcceptingDetails
    {
        public string CFSCode { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CargoDescription { get; set; }
        public decimal ActualQty { get; set; }
        public decimal ActualWeight { get; set; }
        public decimal FobValue { get; set; }
        public string LocationName { get; set; }
        public string GR { get; set; }
        public decimal SQM { get; set; }
        public string FromGodown { get; set; }
        public string ToGodown { get; set; }
        public string FromShippingLine { get; set; }
        public string ToShippingLine { get; set; }
    }
    public class WFLDBTTDetails
    {
        public string CFSCode { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string ExporterName { get; set; }
        public string CargoDescription { get; set; }
        public decimal BTTQuantity { get; set; }
        public decimal BTTWeight { get; set; }
        public decimal Fob { get; set; }
        public string LocationName { get; set; }
        public string GR { get; set; }
        public decimal Area { get; set; }
    }
    public class WFLDStuffingDetails
    {
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string SLA { get; set; }
        public string EntryDateTime { get; set; }
        public int ShippingBillNo { get; set; }
        public decimal StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public decimal Fob { get; set; }
    }
    public class WFLDStockDetails
    {
        public string ShippingBillNo { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
    }
}