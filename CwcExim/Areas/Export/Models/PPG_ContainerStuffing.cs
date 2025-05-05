using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{  
    public class PPG_ContainerStuffing: ContainerStuffing
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

        public List<GENSPOperationCFSCodeWiseAmt> GENSPOperationCFSCodeWiseAmtLst = new List<GENSPOperationCFSCodeWiseAmt>();
        public string InvoiceNoGENSP { get; set; }
        
        public string ContOrigin { get; set; }
        public string ContVia { get; set; }
        public string ContPOL { get; set; }
        
        public string CargoType { get; set; }
        public string ShippingLineNo { get; set; }
        public string ForwarderName { get; set; }

        public string POD { get; set; }
        
        public List<PPG_ContainerStuffingDtl> LstppgStuffingDtl = new List<PPG_ContainerStuffingDtl>();
        public List<PPG_ShippingBillNo> LstppgShipDtl = new List<PPG_ShippingBillNo>();
        public List<PPG_ShippingBillNoGen> LstppgShipDtlgen = new List<PPG_ShippingBillNoGen>();
        public List<PPG_ContainerStuffingCharge> LstppgCharge = new List<PPG_ContainerStuffingCharge>();
        public List<ppgGRLPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgGRLPostPaymentChargebreakupdate>();

        public string PPG_ShippingBillAmt { get; set; }
        public string PPG_ShippingBillAmtGen { get; set; }
        public decimal SQM { get; set; }
        public string spacetype { get; set; }

        public int CargoTypeId { get; set; }
    }

    public class PPGContainerDetail
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
    public class PPG_ShippingBillNo
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }

        public string CargoType { get; set; } = string.Empty;


    }

    public class PPG_ShippingBillNoGen
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }

        public string ShippingBillNo { get; set; }

        public string ShippingDate { get; set; }

        public decimal FOB { get; set; }

        public decimal Amount { get; set; }




    }

    public class PPG_ContainerStuffingDtl : ContainerStuffingDtl
    {
       public string EntryNo { get; set; }
        public string InDate { get; set; }

        public decimal Area { get; set; }
        public string Remarks { get; set; }

        public string PortName { get; set; }
        public string PortDestination { get; set; }

    }
    public class PPG_ContainerStuffingCharge
    {
        public string Invoiceno { get; set; }
        public string  InvoiceDate { get; set; }
        public string chargetype { get; set; }
        public string total { get; set; }
        public string eximtraderalias { get; set; }

    }
    public class GENSPOperationCFSCodeWiseAmt
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


    public class ppgGRLPostPaymentChargebreakupdate
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

}