using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_StuffingRequestDtl
    {
        public int StuffingReqDtlId { get; set; }
        public int StuffingReqId { get; set; }
        public int CartingRegisterDtlId { get; set; }

        // public string StuffingReqNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string CommInvNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public string Exporter { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        //  public string ContainerNo { get; set; }
        //  public int ContainerId { get; set; }
        //  public string Size { get; set; }
        //  public int ShippingLineId { get; set; }
        //  public string ShippingLine { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        //   public int StuffQuantity { get; set; }
        //   public decimal StuffWeight { get; set; }        
        public decimal Fob { get; set; }
        public string CartingRegisterNo { get; set; }
        public int CartingRegisterId { get; set; }
        // public string CFSCode { get; set; }
        public int RQty { get; set; }
        public decimal RWt { get; set; }

        public string CommodityName { get; set; }
        public int GodownId { get; set; }
        public string StuffRefType { get; set; }
    }

    public class Dnd_StuffingReqContainerDtl
    {
        public int StuffingReqContrId { get; set; }
        public string CFSCode { get; set; }
        public int StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public int CartRegDtlId { get; set; }
        public string CommodityName { get; set; }
        public decimal Fob { get; set; }
        public string ContClass { get; set; } = string.Empty;
        public int WithScanning { get; set; }
        public decimal StuffFOBValue { get; set; }
        public int IsCBT { get; set; } = 0;

    }
}