using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_DTRExp
    {
        //public IList<LNSM_CartingDetails> lstCartingDetails { get; set; } = new List<LNSM_CartingDetails>();
        //public IList<LNSM_CartingDetails> lstShortCartingDetails { get; set; } = new List<LNSM_CartingDetails>();
        //public IList<LNSM_CargoAcceptingDetails> lstCargoShifting { get; set; } = new List<LNSM_CargoAcceptingDetails>();
        //public IList<LNSM_CargoAcceptingDetails> lstCargoAccepting { get; set; } = new List<LNSM_CargoAcceptingDetails>();
        //public IList<LNSM_BTTDetails> lstBTTDetails { get; set; } = new List<LNSM_BTTDetails>();
        //public IList<LNSM_StuffingDetails> lstStuffingDetails { get; set; } = new List<LNSM_StuffingDetails>();
        //public IList<LNSM_StockDetails> StockOpening { get; set; } = new List<LNSM_StockDetails>();
        //public IList<LNSM_StockDetails> StockClosing { get; set; } = new List<LNSM_StockDetails>();

        public List<LNSM_CartingDetails> lstCartingDetails { get; set; }
        public List<LNSM_CartingDetails> lstShortCartingDetails { get; set; }
        public List<LNSM_CargoAcceptingDetails> lstCargoShifting { get; set; }
        public List<LNSM_CargoAcceptingDetails> lstCargoAccepting { get; set; }
        public List<LNSM_BTTDetails> lstBTTDetails { get; set; }
        public List<LNSM_StuffingDetails> lstStuffingDetails { get; set; }
        public List<LNSM_GateInInformation> lstGateInInfo { get; set; }
    }
    public class LNSM_CartingDetails
    {
        public string ShippingLineId { get; set; }
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
        public string NoOfPkg { get; set; }
        public string Remarks { get; set; }

    }
    public class LNSM_CargoAcceptingDetails
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
        public string Slot { get; set; }
        public decimal Area { get; set; }
    }
    public class LNSM_BTTDetails
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
    public class LNSM_StuffingDetails
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
    public class LNSM_StockDetails
    {
        public string ShippingBillNo { get; set; }
        public decimal Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Fob { get; set; }
    }

    public class LNSM_GateInInformation
    {
        public string CFSCode { get; set; }
        public string GateInDate { get; set; }
        public string GateInTime { get; set; }
        public string Cont_CBT { get; set; }
        public int ContSize { get; set; }
        public string VehicleNo { get; set; }
        public string ModeOfTransport { get; set; }
        public string ShippingLine { get; set; }
        public string ContLoadType { get; set; }
        public string OperationType { get; set; }
        public string CustomSealNo { get; set; }
        public string OtherSealNo { get; set; }
        public string TrainReceivedFrom { get; set; }
        public string ContType { get; set; }
    }

}