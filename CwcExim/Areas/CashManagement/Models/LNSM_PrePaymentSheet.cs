using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{ 
    public class LNSM_PrePaymentSheet
    {
        public IList<LNSM_PreInvoiceHeader> lstPreInvoiceHdr { get; set; } = new List<LNSM_PreInvoiceHeader>();
        public IList<LNSM_PreInvoiceContainer> lstPreInvoiceCont { get; set; } = new List<LNSM_PreInvoiceContainer>();
        public IList<LNSM_PreContainerWiseAmount> lstPreContWiseAmount { get; set; } = new List<LNSM_PreContainerWiseAmount>();
        public IList<LNSM_PreInvoiceCargo> lstPreInvoiceCargo { get; set; } = new List<LNSM_PreInvoiceCargo>();

    }
    public class LNSM_PreInvoiceHeader
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public int RequestId { get; set; } = 0;
        public string RequestNo { get; set; } = string.Empty;
        public string RequestDate { get; set; } = string.Empty;
        public int FreePeriod { get; set; } = 0;
    }
    public class LNSM_PreInvoiceContainer
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
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal WtPerPack { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public int Reefer { get; set; }
        public int RMS { get; set; }
        public int Insured { get; set; }
        public int HeavyScrap { get; set; }
        public int AppraisementPerct { get; set; }
        public int DeliveryType { get; set; }
        public string ShippingLineName { get; set; }
        public string CHAName { get; set; }
        public string ImporterExporter { get; set; }
        public string DestuffingDate { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public decimal SpaceOccupied { get; set; }
        public string SpaceOccupiedUnit { get; set; }
        public decimal StuffCUM { get; set; } = 0M;
        public int OperationType { get; set; }
        public string LCLFCL { get; set; }
        public decimal NoOfPackagesDel { get; set; }
        public decimal Clauseweight { get; set; }
        public string StorageType { get; set; } = string.Empty;
    }
    public class LNSM_PreContainerWiseAmount
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
    public class LNSM_PreInvoiceCargo
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
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; } = 0M;
        public decimal WtPerUnit { get; set; } = 0M;
        public decimal SpaceOccupied { get; set; } = 0M;
        public string SpaceOccupiedUnit { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }       
        public decimal NoOfPackagesDel { get; set; }
    }

}