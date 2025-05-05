using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_StuffingRequestDtl
    {
        public int StuffingReqDtlId { get; set; }
        public int StuffingReqId { get; set; }
        public int CartingRegisterDtlId { get; set; }
        public string ShippingBillNo { get; set; }
        public string CommInvNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public string Exporter { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }

        public int CargoType { get; set; }    
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Fob { get; set; }
        public string CartingRegisterNo { get; set; }
        public int CartingRegisterId { get; set; }
        public int ContComeFrom { get; set; }
        public int PortId { get; set; } = 0;
        public string PortName { get; set; } = string.Empty;
        public decimal Distance { get; set; } = 0;
        public decimal Cum { get; set; } = 0;
        public int ContainerType { get; set; }
        public string ForwarderName { get; set; }
        public int ForwarderId { get; set; }

        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }


    }

    public class Hdb_StuffingReqContainerDtl
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
        public string ForwarderName { get; set; }
        public string SealNo { get; set; }
        public int ForwarderId { get; set; }
        public decimal StuffFOBValue { get; set; }
        public int ContComeFrom { get; set; }
        public int PortId { get; set; } = 0;
        public string PortName { get; set; } = string.Empty;
        public decimal Distance { get; set; } = 0;
        public decimal Cum { get; set; } = 0;
        public int ContainerType { get; set; }      

    }
}