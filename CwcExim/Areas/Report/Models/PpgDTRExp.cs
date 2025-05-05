using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class PpgDTRExp
    {
        public IList<CartingDetails> lstCartingDetails { get; set; } = new List<CartingDetails>();
        public IList<CartingDetails> lstShortCartingDetails { get; set; } = new List<CartingDetails>();
        public IList<CargoAcceptingDetails> lstCargoShifting { get; set; } = new List<CargoAcceptingDetails>();
        public IList<CargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<CargoAcceptingDetails>();
        public IList<BTTDetails> lstBTTDetails { get; set; } = new List<BTTDetails>();
        public IList<StuffingDetails> lstStuffingDetails { get; set; } = new List<StuffingDetails>();
        public IList<StockDetails> StockOpening { get; set; } =new List<StockDetails>();
        public IList<StockDetails> StockClosing { get; set; } = new List<StockDetails>();
    }
    public class CartingDetails
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
    public class CargoAcceptingDetails
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
    public class BTTDetails
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
    public class StuffingDetails
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
    public class StockDetails
    {
        public string ShippingBillNo { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
    }

    public class Ppg_PartyWiseTuesDone
    {        
        public string PartyName { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string GateInDate { get; set; }        
    }
}