using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class SealCuttingV2
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
        public decimal DTFIGSTPer { get; set; }
        public decimal DTFCGSTPer { get; set; }
        public decimal DTFSGSTPer { get; set; }
        [Display(Name = "CBT DTF Amt")]
        public decimal CBTDTF { get; set; }
        [Display(Name = "CBT DTF CGST")]
        public decimal DTFCGST { get; set; }
        [Display(Name = "CBT DTF SGST")]
        public decimal DTFSGST { get; set; }
        [Display(Name = "CBT DTF IGST")]
        public decimal DTFIGST { get; set; }
        [Display(Name = "CBT DTF Amount")]
        public decimal DTFTotalAmount { get; set; }
        [Display(Name = "Invoice Amount")]
        public decimal InvoiceAmt { get; set; }
        public decimal TotalTaxable { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalIGST { get; set; }

        public List<SealCuttingV2> lstObldt { get; set; } = new List<SealCuttingV2>();
        public String ViewBLList { get; set; }
        public string lstPostPaymentChrgBreakupAmt { get; set; }
        public List<ppgSealPostPaymentChargebreakupdateV2> lstPostPaymentChrgBreakup { get; set; } = new List<ppgSealPostPaymentChargebreakupdateV2>();     
        public List<PostPaymentChargeV2> lstPostPaymentChrg { get; set; } = new List<PostPaymentChargeV2>();
        public string lstPostPaymentChrgAmt { get; set; }

        public string SEZ { get; set; }
    }
    
   
}