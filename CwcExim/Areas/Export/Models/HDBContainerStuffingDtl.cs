using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class HDBContainerStuffingDtl
    {
        public int ContainerStuffingDtlId { get; set; }
        public int ContainerStuffingId { get; set; }
        public int StuffingReqDtlId { get; set; }
        public string CartingRegNo { get; set; }
        
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeLocation { get; set; }
        public string MarksNo { get; set; }
        public int ? Insured { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public string Exporter { get; set; }
        public int ForwarderId { get; set; }
        public string Forwarder { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public int StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public decimal Fob { get; set; }
        public string Size { get; set; }
        public decimal CBM { get; set; }
        public string StuffingType { get; set; }
        public string CustomSeal { get; set; }
        public string ShippingSeal { get; set; }
        public string RequestDate { get; set; }
        public string CommodityName { get; set; }
        public int IsODC { get; set; }
        public int IsReefer { get; set; }

        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string MCINPCIN { get; set; }
        public int SEZ { get; set; }
        public int PacketsFrom { get; set; }
        public int PacketsTo { get; set; }
        public List<HDBContainerStuffingDtl> LstStuffing { get; set; } = new List<HDBContainerStuffingDtl>();

    }
}