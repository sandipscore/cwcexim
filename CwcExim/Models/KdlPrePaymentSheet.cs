using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    public class KdlPrePaymentSheet
    {


        public IList<PreInvoiceHeader> lstPreInvoiceHdr { get; set; } = new List<PreInvoiceHeader>();
        public IList<KdlCWCPreInvoiceContainer> KdlCWClstPreInvoiceCont { get; set; } = new List<KdlCWCPreInvoiceContainer>();
        public IList<PreContainerWiseAmount> lstPreContWiseAmount { get; set; } = new List<PreContainerWiseAmount>();

        public IList<KdlCWCPreInvoiceCargo> KdlCWClstPreInvoiceCargo { get; set; } = new List<KdlCWCPreInvoiceCargo>();
    }
       
        public class KdlCWCPreInvoiceHeader
        {
            public int CHAId { get; set; }
            public string CHAName { get; set; }
            public string GSTNo { get; set; }
            public string Address { get; set; }
            public string StateCode { get; set; }
        }
        public class KdlCWCPreInvoiceContainer
        {
            public string ApproveOn { get; set; } = string.Empty;
            public string ContainerNo { get; set; }
            public string CFSCode { get; set; }
            public string ArrivalDate { get; set; }
            public string ArrivalTime { get; set; }
            public int CargoType { get; set; }
            public string LineNo { get; set; } = string.Empty;
            public string BOENo { get; set; }
            public string BOEDate { get; set; }
            public string Size { get; set; }
            public decimal NoOfPackages { get; set; } = 0M;
            public decimal GrossWeight { get; set; }
            public decimal WtPerPack { get; set; }
            public decimal CIFValue { get; set; }
            public decimal Duty { get; set; }
            public int Reefer { get; set; } = 0;
            public int RMS { get; set; } = 0;
            public int Insured { get; set; } = 0;
            public int HeavyScrap { get; set; } = 0;
                public int AppraisementPerct { get; set; } = 0;
                public int DeliveryType { get; set; } = 0;
            public string ShippingLineName { get; set; } = string.Empty;
            public string CHAName { get; set; }=
            string.Empty;
            public string ImporterExporter { get; set; }= string.Empty;
            public string DestuffingDate { get; set; } = string.Empty;
            public string CartingDate { get; set; }= string.Empty;
            public string StuffingDate { get; set; }= string.Empty;
            public decimal SpaceOccupied { get; set; }= 0M;
            public string SpaceOccupiedUnit { get; set; }= string.Empty;
            public decimal StuffCUM { get; set; } = 0M;
            public int OperationType { get; set; } = 0;
            public string LCLFCL { get; set; }
        }
        public class KdlCWCPreContainerWiseAmount
        {
            public string CFSCode { get; set; } = string.Empty;
            public decimal EntryFee { get; set; } = 0M;
            public decimal CstmRevenue { get; set; } = 0M;
            public decimal GrEmpty { get; set; } = 0M;
            public decimal GrLoaded { get; set; } = 0M;
            public decimal ReeferCharge { get; set; } = 0M;
            public decimal StorageCharge { get; set; } = 0M;
            public decimal InsuranceCharge { get; set; } = 0M;
            public decimal PortCharge { get; set; } = 0M;
            public decimal WeighmentCharge { get; set; } = 0M;
        }
       
        public class KdlCWCPreInvoiceCargo
        {
            public string BOENo { get; set; } = string.Empty;
            public string BOEDate { get; set; } = string.Empty;
            public string CargoDescription { get; set; } = string.Empty;
            public int GodownId { get; set; }
            public string GodownName { get; set; }
            public string GodownWiseLocationIds { get; set; }
            public string GodownWiseLctnNames { get; set; }
            public string BOLNo { get; set; }
            public string BOLDate { get; set; }
            public int CargoType { get; set; }
            public string DestuffingDate { get; set; }
            public string CartingDate { get; set; }
            public string StuffingDate { get; set; }
            public decimal NoOfPackages { get; set; } = 0M;
            public decimal GrossWeight { get; set; } = 0M;
            public decimal WtPerUnit { get; set; } = 0M;
            public decimal SpaceOccupied { get; set; } = 0M;
            public string SpaceOccupiedUnit { get; set; }
            public decimal CIFValue { get; set; }
            public decimal Duty { get; set; }
        }
    }

