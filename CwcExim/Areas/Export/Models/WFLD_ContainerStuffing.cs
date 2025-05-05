using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_ContainerStuffing : ContainerStuffing
    {
        public string SCMTRXML { get; set; }
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

        public List<WFLDGENSPOperationCFSCodeWiseAmt> GENSPOperationCFSCodeWiseAmtLst = new List<WFLDGENSPOperationCFSCodeWiseAmt>();
        public string InvoiceNoGENSP { get; set; }

        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }

        public int PolId { get; set; }
        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }

        public string POD { get; set; }
        public int PODId { get; set; }

        public List<WFLD_ContainerStuffingDtl> LstppgStuffingDtl = new List<WFLD_ContainerStuffingDtl>();
        public List<WFLD_ShippingBillNo> LstppgShipDtl = new List<WFLD_ShippingBillNo>();
        public List<WFLD_ShippingBillNoGen> LstppgShipDtlgen = new List<WFLD_ShippingBillNoGen>();
        public List<WFLD_ContainerStuffingCharge> LstppgCharge = new List<WFLD_ContainerStuffingCharge>();
        public List<WFLDGRLPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDGRLPostPaymentChargebreakupdate>();
        public List<WFLD_ContainerStuffingSCMTR> LstSCMTRDtl = new List<WFLD_ContainerStuffingSCMTR>();
        public List<ContainerWiseCustomShippingSeal> ContainerWiseCustomShippingSeal { get; set; } = new List<Models.ContainerWiseCustomShippingSeal>();
        public string WFLD_ShippingBillAmt { get; set; }
        public string WFLD_ShippingBillAmtGen { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoTypeId { get; set; }
        public decimal CBM { get; set; }

        public string CountryName { get; set; }
        public string CustodianCode { get; set; }
        public int CustodianId { get; set; }
        public string AmendmentDate { get; set; }
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;


    }

    public class WFLDContainerDetail
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public decimal FOB { get; set; }
        public int StuffingReqId { get; set; }
        public decimal StuffWeight { get; set; }
        public int Insured { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }


    }
    public class WFLD_ShippingBillNo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }
    public class WFLD_ShippingBillNoGen
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class WFLD_ContainerStuffingDtl : WFLD_ContainerStuffingDtlBase
    {
        public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }

        public string PortName { get; set; }
        public string PortDestination { get; set; }  
        public string PackUQC { get; set; }      

    }
    public class WFLD_ContainerStuffingCharge
    {
        public string Invoiceno { get; set; }
        public string InvoiceDate { get; set; }
        public string chargetype { get; set; }
        public string total { get; set; }
        public string eximtraderalias { get; set; }

    }
    public class WFLDGENSPOperationCFSCodeWiseAmt
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string CFSCode { get; set; }
        public int OperationID { get; set; }
        public int Quantity { get; set; }
        public string ChargeType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

    }


    public class WFLDGRLPostPaymentChargebreakupdate
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;

        public int OperationId { get; set; }
        public string CFSCode { get; set; }
        public string Startdate { get; set; }
        public string EndDate { get; set; }
    }


    public class ContainerWiseCustomShippingSeal
    {
        public string ContainerNo { get; set; }
        public string CustomSeal { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string PortName { get; set; }
        public string POD { get; set; }
        public string ShippingLine { get; set; }

        public string ShippingLineNo { get; set; }
        public string ForwardName { get; set; }

        public string CargoType { get; set; }
        public string EquipmentSealType { get; set; }

    }

    public class WFLD_ReqContainerStuffing
    {
        public int StuffingReqId { get; set; }
        public string StuffingReqNo { get; set; }
        public string RequestDate { get; set; }
    }
    public class WFLD_FinalDestination
    {
        public int CustodianId { get; set; }
        public string CustodianCode { get; set; }
        public string CustodianName { get; set; }
    }
}