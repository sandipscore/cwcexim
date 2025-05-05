using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Ppg_ExportDestuffing
    {  
        public int DestuffingId { get; set; }
        public string DestuffingNo { get; set; }
        public string Destuffingdate { get; set; }
        public string ContainerNo { get; set; } 
        public int Size { get; set; }
        public string CFSCode { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string Remarks { get; set; }
        public string SpaceType { get; set; }
        public int UId { get; set; }
        public string StringifiedText { get; set; }
        public string RefNo { get; set; } = string.Empty;

        public string OperationType { get; set; }
        public List<Dnd_ExportDestuffDetails> lstDestuffing { get; set; } = new List<Dnd_ExportDestuffDetails>();
    }

    public class Ppg_ExportDestuffDetails
    {
        public int DestuffingDtlId { get; set; } = 0;
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public int EXPId { get; set; }

        public int CHAId { get; set; }
        public string Exporter { get; set; }
        // public string DestuffingNo { get; set; }
        public string cargoDesc { get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public string CargoType { get; set; }
        public decimal GrWt { get; set; }
        public decimal CUM { get; set; }
        public int Unit { get; set; }
        public decimal FOB { get; set; }
        public decimal ReservedSQM { get; set; }
        public decimal UnReservedSQM { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }

        public string NewSpCCIN { get; set; }

        public int CartingRegisterDtlId  { get; set; }

    }

    public class Ppg_ExportDestuffingContainer
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string SType { get; set; }

        public int ContainerStuffingId { get; set; }

        public int CartingRegisterId { get; set; }
        public int GodownId { get; set; }
        public string GodownName { get; set; }

        public string OperationType { get; set; }
    }

    public class Ppg_ExportDestuffingList
    {
        public int DestuffingId { get; set; }
        public string DestuffingNo { get; set; }
        public string DestuffingDate { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string ShippingLine { get; set; }


    }
}