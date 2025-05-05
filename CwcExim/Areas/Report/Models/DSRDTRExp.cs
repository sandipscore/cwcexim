using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRDTRExp
    {
        public IList<DSRCartingDetails> lstCartingDetails { get; set; } = new List<DSRCartingDetails>();
        public IList<DSRCartingDetails> lstShortCartingDetails { get; set; } = new List<DSRCartingDetails>();
        public IList<DSRCargoAcceptingDetails> lstCargoShifting { get; set; } = new List<DSRCargoAcceptingDetails>();
        public IList<DSRCargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<DSRCargoAcceptingDetails>();
        public IList<DSRBTTDetails> lstBTTDetails { get; set; } = new List<DSRBTTDetails>();
        public IList<DSRStuffingDetails> lstStuffingDetails { get; set; } = new List<DSRStuffingDetails>();
        public IList<DSRStockDetails> StockOpening { get; set; } =new List<DSRStockDetails>();
        public IList<DSRStockDetails> StockClosing { get; set; } = new List<DSRStockDetails>();
    }
    public class DSRCartingDetails
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
    public class DSRCargoAcceptingDetails
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
    public class DSRBTTDetails
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
    public class DSRStuffingDetails
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
    public class DSRStockDetails
    {
        public string ShippingBillNo { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
    }
}