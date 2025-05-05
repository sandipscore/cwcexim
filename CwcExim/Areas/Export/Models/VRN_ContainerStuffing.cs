using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class VRN_ContainerStuffing
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

        public string Via { get; set; }
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public int GENSPPartyId { get; set; }
        [Display(Name = "Party Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public String GENSPPartyCode { get; set; }
        public int GENSPOperationId { get; set; }
        public String GENSPChargeType { get; set; }
        public String GENSPChargeName { get; set; }
        [Display(Name = "DSTF Charge")]
        public decimal GENSPCharge { get; set; }
        public decimal GENSPCGSTCharge { get; set; }
        public decimal GENSPSGSTCharge { get; set; }
        public decimal GENSPIGSTCharge { get; set; }
        public decimal GENSPIGSTPer { get; set; }
        public decimal GENSPCGSTPer { get; set; }
        public decimal GENSPSGSTPer { get; set; }
        public decimal GENSPAmount { get; set; }
        public decimal GENSPTaxable { get; set; }
        public string GENSPSACCode { get; set; }
        public decimal GENSPTotalAmount { get; set; }

        public string GENSPOperationCFSCodeWiseAmt { get; set; }

        public string InvoiceNoGENSP { get; set; }

        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }

        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }

        public string POD { get; set; }

        public List<VRN_ContainerStuffingDtl> LstStuffingDtl = new List<VRN_ContainerStuffingDtl>();
        public List<VRN_ShippingBillNo> LstppgShipDtl = new List<VRN_ShippingBillNo>();
        public List<VRN_ShippingBillNoGen> LstppgShipDtlgen = new List<VRN_ShippingBillNoGen>();
        public string PPG_ShippingBillAmt { get; set; }
        public string PPG_ShippingBillAmtGen { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoTypeId { get; set; }
        public string POLName { get; set; }
        public string CompanyAddress { get; set; } = "";
        public int TransportMode { get; set; } = 0;
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string SCMTRXML { get; set; }
        public List<VRN_ContainerStuffingSCMTR> LstSCMTR { get; set; } = new List<VRN_ContainerStuffingSCMTR>();
        public string CustodianCode { get; set; }
        public int CustodianId { get; set; }
        public string AmendmentDate { get; set; }
    }

    public class VRN_ShippingBillNo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }
    public class VRN_ContainerStuffingDtl
    {
        public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }

        public string PortName { get; set; }
        public string PortDestination { get; set; }
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
        //  public List<ContainerStuffingDtl> LstStuffing = new List<ContainerStuffingDtl> { get; set;}
        public List<VRN_ContainerStuffingDtl> LstStuffing { get; set; } = new List<VRN_ContainerStuffingDtl>();
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string MCINPCIN { get; set; }

    }
    public class VRN_ShippingBillNoGen
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class VRN_ContainerStuffingSCMTR
    {
        public int Id { get; set; }
        public int ContainerStuffingId { get; set; }
        public string ReportingpartyCode { get; set; }
        public string Equipmenttype { get; set; }
        public string ContainerID { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipmentLoadStatus { get; set; }
        public string FinalDestinationLocation { get; set; }
        public string EventDate { get; set; }
        public string EquipmentSealType { get; set; }
        public string EquipmentSealNo { get; set; }
        public string EquipmentStatus { get; set; }

        public Int32 CargoSequenceNo { get; set; }
        public string DocumentType { get; set; }
        public string ShipmentLoadStatus { get; set; }
        public string PackageType { get; set; }

        public Int32 EquipmentSerialNo { get; set; }
        public Int32 DocumentSerialNo { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentReferenceNo { get; set; }

    }

    public class VRN_FinalDestination
    {
        public int CustodianId { get; set; }
        public string CustodianCode { get; set; }
        public string CustodianName { get; set; }
    }
}