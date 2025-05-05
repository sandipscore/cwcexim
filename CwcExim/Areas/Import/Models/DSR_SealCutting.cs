using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_SealCutting
    {
        public int impobldtlId { get; set; }
        public int SealCuttingId { get; set; }
        public int BlDetailId { get; set; }
        public int OBLId { get; set; }
        public int CargoTypeId { get; set; }
        public string CargoType { get; set; }
        public string NO_PKG { get; set; }
        public decimal GR_WT { get; set; }
        public string DisplayOBLType { get; set; }
        [Display(Name = "Seal Cutting No.")]
        public string SealCuttingNo { get; set; }
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

        public int? BLId { get; set; }

        [Display(Name = "BL No")]
        public string BLNo { get; set; }
        [Display(Name = "BL Date")]
        public string BLDate { get; set; }

        [Display(Name = "OBL Type")]
        public string OBLType { get; set; }
        public int? ContainerId { get; set; }
        [Display(Name = "Container No")]
        public string ContainerNo { get; set; }
        public string size { get; set; }
        [Display(Name = "Gate In Date")]
        public string GateInDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

        [Display(Name = "ICD Code")]
        public string CFSCode { get; set; }
        public int? CustomSealId { get; set; }

        [Display(Name = "Custom Seal No")]
        public string CustomSealNo { get; set; }
        public int GodownId { get; set; } = 0;
        [Display(Name = "Godown No")]
        public string GodownNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

        public int CHAShippingLineId { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "CHA/Shipping Line :")]
        public string CHAShippingLine { get; set; }
        public String FolioNo { get; set; }
        public decimal Balance { get; set; }

        public int InvoiceId { get; set; }
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Ground Rent")]
        public decimal GroundRent { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal CGST { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal SGST { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal IGST { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public int BranchId { get; set; }
        public int Uid { get; set; }
        public string LCLFCL { get; set; }
        //public List<CustomAppraisementDtl> LstSealCuttingDtl = new List<CustomAppraisementDtl>();
        public List<DSR_SealCutting> lstObldt { get; set; } = new List<DSR_SealCutting>();
        public String ViewBLList { get; set; }
        public string lstPostPaymentChrgBreakupAmt { get; set; }
        public List<DSR_SealPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSR_SealPostPaymentChargebreakupdate>();

    }
    public class DSR_SealCuttingCHA
    {
        public int CHAShippingLineId { get; set; }
        public string CHAShippingLine { get; set; }
        public string FolioNo { get; set; }
        public decimal Balance { get; set; }
    }


    public class DSR_SealPostPaymentChargebreakupdate
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