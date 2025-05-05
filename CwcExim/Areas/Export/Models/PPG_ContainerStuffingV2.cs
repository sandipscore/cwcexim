using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_ContainerStuffingV2 : ContainerStuffingV2
    {
        public int GENSPPartyId { get; set; }
        [Display(Name = "Party Code")]
        //[Required(ErrorMessage = "Fill Out This Field")]
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

        public List<GENSPOperationCFSCodeWiseAmtV2> GENSPOperationCFSCodeWiseAmtLst = new List<GENSPOperationCFSCodeWiseAmtV2>();
        public string InvoiceNoGENSP { get; set; }

        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }

        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }

        public string POD { get; set; }

        public List<PPG_ContainerStuffingDtlV2> LstppgStuffingDtl = new List<PPG_ContainerStuffingDtlV2>();
        public List<PPG_ShippingBillNoV2> LstppgShipDtl = new List<PPG_ShippingBillNoV2>();
        public List<PPG_ShippingBillNoGenV2> LstppgShipDtlgen = new List<PPG_ShippingBillNoGenV2>();
        public List<PPG_ContainerStuffingChargeV2> LstppgCharge = new List<PPG_ContainerStuffingChargeV2>();
        public List<ppgGRLPostPaymentChargebreakupdateV2> lstPostPaymentChrgBreakup { get; set; } = new List<ppgGRLPostPaymentChargebreakupdateV2>();

        public string PPG_ShippingBillAmt { get; set; }
        public string PPG_ShippingBillAmtGen { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoTypeId { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string ChargesXML { get; set; }
        public string BreakUpdateXML { get; set; }
        public string ContainerNo { get; set; }
        public List<Ppg_ContStuffChargesV2> lstChargs { get; set; } = new List<Ppg_ContStuffChargesV2>();
        
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string CustodianCode { get; set; }
        public int CustodianId { get; set; }       
    }
    public class PPGContainerDetailV2
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
    public class PPG_ShippingBillNoV2
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }

    public class PPG_ShippingBillNoGenV2
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class PPG_ContainerStuffingDtlV2 : ContainerStuffingDtlV2
    {
        public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }

        public string PortName { get; set; }
        public string PortDestination { get; set; }

    }
    public class PPG_ContainerStuffingChargeV2
    {
        public string Invoiceno { get; set; }
        public string InvoiceDate { get; set; }
        public string chargetype { get; set; }
        public string total { get; set; }
        public string eximtraderalias { get; set; }

    }
    public class GENSPOperationCFSCodeWiseAmtV2
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


    public class ppgGRLPostPaymentChargebreakupdateV2
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

    public class PPG_FinalDestination
    {
        public int CustodianId { get; set; }
        public string CustodianCode { get; set; }
        public string CustodianName { get; set; }
    }
}