using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSR_ContainerStuffingDtlBase
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
        public int? ReeferHrs { get; set; }
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
        public decimal CBM { get; set; }
        public string spacetype { get; set; }
        public int CargoType { get; set; }
        //  public List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl> { get; set;}
        public List<DSR_ContainerStuffingDtlBase> LstStuffing { get; set; } = new List<DSR_ContainerStuffingDtlBase>();
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public decimal Palletaisation { get; set; }
        public decimal WashingOfCargo { get; set; }
        public decimal Weighment { get; set; }
        public decimal Reworking { get; set; }
        public decimal InternalShifting { get; set; }
        public decimal LiftOnEmpty { get; set; }
        public decimal LiftOffEmpty { get; set; }
        public string POD { get; set; }
        public int PODId { get; set; }
        public int POL { get; set; }
        public string ContPOL { get; set; }
        public int? RMSValue { get; set; }

        public int GodownId { get; set; }
        public string ContStuffingDate { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string MCINPCIN { get; set; }
        public int ShortCargoDtlId { get; set; }
        public int? SEZ { get; set; }
        public int PacketsFrom { get; set; }
        public int PacketsTo { get; set; }
    }
}