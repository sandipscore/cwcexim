using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_TotalContainerReport
    {
        public string mstcompany { get; set; }
        public string GodownName { get; set; }
        public int GodownId { get; set; }
        public List<Dnd_TotalContainer> LstTotal { get; set; } = new List<Dnd_TotalContainer>();
      

    }
    public class Dnd_TotalContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string InDateTime { get; set; }
        public string Size { get; set; }
        public string ShippingLine { get; set; }
        public string Origin { get; set; }
        public string Status { get; set; }
        public string ContClass { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string OutDate { get; set; }
        public string ContainerType { get; set; }
        public string CustomSealNo { get; set; }
        public string DestuffingDate { get; set; }
        public string GatePassNo { get; set; }
        public string ExitDateTime { get; set; }
        public string ViaNo { get; set; }
        public string Vessel { get; set; }
        public string MovementNo { get; set; }
        public string MovementDate { get; set; }
        public string POD { get; set; }
        public string POL { get; set; }
        public string Via { get; set; }
        public string ViaId { get; set; }
        public string GatePassDate { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public decimal NoOfPkg { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
        public string ShedNo { get; set; }
        public string SlaSealNo { get; set; }
        public string Stuff { get; set; }
        public int StuffId { get; set; }
        public string ExpCode { get; set; }
        public string EntryNo { get; set; }
        public string CartingDate { get; set; }
        public string CHA { get; set; }
        public string Commodity { get; set; }
        public string ShippingLineName { get; set; }

        public decimal TotalPkg { get; set; }
        public decimal ReceivedPkg { get; set; }
        public string SlotNo { get; set; }
        public string GenReserved { get; set; }
        public decimal Area { get; set; }
        public decimal Balance { get; set; }
        public int ShippingLineId { get; set; }
        public string GodownName { get; set; }
        public string StuffingReqNo { get; set; }
        public string RequestDate { get; set; }
        public int NoOfSbs { get; set; }
        public decimal NoOfUnits { get; set; }
        public decimal SQM { get; set; }
        public string ShippingLineAlias { get; set; }
        public int TotalSbs { get; set; }
        public string Godown { get; set; }
        public string LoadedInDate { get; set; }         
    }
}