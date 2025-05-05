using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{  
    public class DSR_ContainerStuffing : ContainerStuffing
    {
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

        public List<DSRGENSPOperationCFSCodeWiseAmt> GENSPOperationCFSCodeWiseAmtLst = new List<DSRGENSPOperationCFSCodeWiseAmt>();
        public string InvoiceNoGENSP { get; set; }
        
        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public string ContPOL { get; set; }

       
        public int PolId { get; set; }
        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }
        public int PODId { get; set; }
        [Required(ErrorMessage = "Select POD")]
        public string POD { get; set; }
        public int PortofDischargeId { get; set; }

        public List<DSR_ContainerStuffingDtl> LstppgStuffingDtl = new List<DSR_ContainerStuffingDtl>();
        public List<DSR_ShippingBillNo> LstppgShipDtl = new List<DSR_ShippingBillNo>();
        public List<DSR_ShippingBillNoGen> LstppgShipDtlgen = new List<DSR_ShippingBillNoGen>();
        public List<DSR_ContainerStuffingCharge> LstppgCharge = new List<DSR_ContainerStuffingCharge>();
        public List<DSRGRLPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRGRLPostPaymentChargebreakupdate>();
        public string WFLD_ShippingBillAmt { get; set; }
        public string WFLD_ShippingBillAmtGen { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoTypeId { get; set; }
        public decimal CBM { get; set; }
        public string MainLine { get; set; }
        public string ShippingSeal { get; set; }
        public string CustomSeal { get; set; }
        public string CustodianCode { get; set; }
        public int CustodianId { get; set; }
        public string ShipBillNo { get; set; } = string.Empty;
        public string ContainerNo { get; set; } = string.Empty;
        public string AmendmentDate { get; set; }
        public int ShortCargoDtlId { get; set; }
        
    }

    public class DSRContainerDetail
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

        public int RMSValue { get; set; }


    }
    public class DSR_ShippingBillNo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }
    public class DSR_ShippingBillNoGen
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class DSR_ContainerStuffingDtl : DSR_ContainerStuffingDtlBase
    {
       public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }
    
        public string PortName { get; set; }
        public string PortDestination { get; set; }
        public string CartingDate { get; set; }        
        public string CartingType { get; set; }
        public string CargoTypeName { get; set; }

    }
    public class DSR_ContainerStuffingCharge
    {
        public string Invoiceno { get; set; }
        public string  InvoiceDate { get; set; }
        public string chargetype { get; set; }
        public string total { get; set; }
        public string eximtraderalias { get; set; }

    }
    public class DSRGENSPOperationCFSCodeWiseAmt
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


    public class DSRGRLPostPaymentChargebreakupdate
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
    public class DSR_FinalDestination
    {
        public int CustodianId { get; set; }
        public string CustodianCode { get; set; }
        public string CustodianName { get; set; }
    }

}