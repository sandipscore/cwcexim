using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class FCLtoLCLConversion
    {
        public int SALId { get; set; }
        public int FCLtoLCLConversionId { get; set; }
        [Display(Name = "Container Class")]
        public string ContainerClass { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public int? ContainerId { get; set; }

        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        [Display(Name = "Container No.")]
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        [Display(Name = "Gate In Date")]
        public string GateInDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

        [Display(Name = "ICD Code")]
        public string CFSCode { get; set; }
        public int ContainerClassId { get; set; }
      
        
        public string SAL { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Old OBL Type")]
        public String OldOBLType { get; set; }

        [Display(Name = "New OBL Type")]
        public String NewOBLType { get; set; }

        public int PartyPdaId { get; set; }
        [Display(Name = "Party SD Name")]
        public String PartyPdaCode { get; set; }

        public int DSTFOperationId { get; set; }
        
        public String DSTFChargeType { get; set; }
        public String DSTFChargeName { get; set; }
        [Display(Name = "DSTF Charge")]
        public decimal DSTFCharge { get; set; }
        public decimal DSTFCGSTCharge { get; set; }
        public decimal DSTFSGSTCharge { get; set; }
        public decimal DSTFIGSTCharge { get; set; }
        public decimal DSTFIGSTPer { get; set; }
        public decimal DSTFCGSTPer { get; set; }
        public decimal DSTFSGSTPer { get; set; }
        public decimal DSTFAmount { get; set; }
        public decimal DSTFTaxable { get; set; }
        public decimal DSTFTotalAmount { get; set; }

        public int AmmendOperationId { get; set; }
        public String AmmendChargeType { get; set; }
        public String DSTFSACCode { get; set; }
        public String AmmendSACCode { get; set; }
        public String AmmendChargeName { get; set; }
        public decimal AmmendCharge { get; set; }
        public decimal AmmendCGSTCharge { get; set; }
        public decimal AmmendSGSTCharge { get; set; }
        public decimal AmmendIGSTCharge { get; set; }
        public decimal AmmendIGSTPer { get; set; }
        public decimal AmmendCGSTPer { get; set; }
        public decimal AmmendSGSTPer { get; set; }
        public decimal AmmendAmount { get; set; }
        public decimal AmmendTaxable { get; set; }
        public decimal AmmendTotalAmount { get; set; }


        //add for this

        public int LOLOperationId { get; set; }
        public String LOLChargeType { get; set; }
      //  public String LOLSACCode { get; set; }
        public String LOLSACCode { get; set; }
        public String LOLChargeName { get; set; }
        public decimal LOLCharge { get; set; }
        public decimal LOLCGSTCharge { get; set; }
        public decimal LOLSGSTCharge { get; set; }
        public decimal LOLIGSTCharge { get; set; }
        public decimal LOLIGSTPer { get; set; }
        public decimal LOLCGSTPer { get; set; }
        public decimal LOLSGSTPer { get; set; }
        public decimal LOLAmount { get; set; }
        public decimal LOLTaxable { get; set; }
        public decimal LOLTotalAmount { get; set; }


        public decimal CGSTCharge { get; set; }
        public decimal SGSTCharge { get; set; }
        public decimal IGSTCharge { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal TotalAmount { get; set; }
        
        public int BranchId { get; set; }
        public int Uid { get; set; }

        public string SEZ { get; set; }

        public List<ppgFCLPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgFCLPostPaymentChargebreakupdate>();

    }
    public class FCLtoLCLContainerList
    {
        public int? ContainerId { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string GateInDate { get; set; }
        public int SALId { get; set; }
        public string SAL { get; set; }       
        public string CFSCode { get; set; }
        public int ContainerClassId { get; set; }
        public string ContainerClass { get; set; }
        public String OldOblType { get; set; }
        public String NewOblType { get; set; }
    }

    public class FCLtoLCLForwarderList
    {
        public int? PartyPdaId { get; set; }
        public string PartyPdaCode { get; set; }
        
    }
    public class FCLtoLCLSLAList
    {
        public int? SLAId { get; set; }
        public string SLA { get; set; }

    }


    public class ppgFCLPostPaymentChargebreakupdate
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