using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_ContainerStuffing
    {
        public int ContainerStuffingId { get; set; }
        public int StuffingReqId { get; set; }
        public string StuffingNo { get; set; }
        public string StuffingDate { get; set; }
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string StuffingReqNo { get; set; }
        public string RequestDate { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string Size { get; set; }
        public string GodownName { get; set; }
        public bool DirectStuffing { get; set; }
        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }

        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }
        public string Mainline { get; set; }
        public string Via { get; set; }
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }
        public int TransportMode { get; set; }
        public string POD { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public string PODischarge { get; set; }

        public int PODischargeID { get; set; }
        public string POLName { get; set; }
        public string CompanyAddress { get; set; } = "";
        public int CargoTypeId { get; set; }
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public List<Dnd_ContainerStuffingDtl> LstStuffing { get; set; } = new List<Dnd_ContainerStuffingDtl>();
        public List<DndContainerForStuffingPrint> LstCont { get; set; } = new List<DndContainerForStuffingPrint>();

    }
    public class Dnd_ContainerStuffingDtl
    {
        public int ContainerStuffingDtlId { get; set; }
        public int ContainerStuffingId { get; set; }
        public int StuffingReqDtlId { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Consignee { get; set; }
        public string MarksNo { get; set; }
        public int? Insured { get; set; }
        public int? Refer { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public string Exporter { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public int StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public decimal Fob { get; set; }
        public string Size { get; set; }
        public string StuffingType { get; set; }
        public string CustomSeal { get; set; }
        public string ShippingSeal { get; set; }
        public string RequestDate { get; set; }
        public string CommodityName { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoType { get; set; }
        public string POLName { get; set; }
        public string CompanyAddress { get; set; } = "";
        public string Vessel { get; set; } = string.Empty;
        public string Voyage { get; set; } = string.Empty;
        public string Via { get; set; } = string.Empty;

        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }

        public string POD { get; set; }
        public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }

        public string PortName { get; set; }
        public string PortDestination { get; set; }
        public int? IsOdc { get; set; } = 0;
        public string RefType { get; set; } = string.Empty;



    }

    public class Dnd_ShippingBillNo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }
    public class Dnd_ShippingBillNoGen
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class DndContainerForStuffingPrint
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string CargoType { get; set; }
    }
}