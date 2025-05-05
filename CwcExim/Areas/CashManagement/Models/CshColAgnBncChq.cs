using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
namespace CwcExim.Areas.CashManagement.Models
{
    public class CashColAgnBncChq
    {
        public int CashColAgnBncChqId { get; set; }

        [Display(Name = "Date :")]
        public string CashColAgnBncChqDate { get; set; }
        public int ImpExpChaId { get; set; }

        [Required(ErrorMessage = "Fill up this field.")]
        [Display(Name = "Import/Exporter/CHA :")]
        public string ImpExpChaName { get; set; }

        [Display(Name = "Cheque No. :")]
        public string BncChequeNo { get; set; }

        [Display(Name = "Date :")]
        public string BncChequeDate { get; set; }

        [Display(Name = "Amount :")]
        public decimal BncChequeAmt { get; set; } = 0M;

        [Display(Name = "Date of Bounce :")]
        public string BncChequeBounceDate { get; set; }

        [Display(Name = "Cheque/DD No. :")]
        public string ChequeDDNo { get; set; }

        [Display(Name = "Date :")]
        public string ChequeDate { get; set; }

        [Display(Name = "Amount :")]
        public decimal ChequeAmt { get; set; } = 0M;

        [Display(Name = "Date of Deposit:")]
        public string ChequeDepositDate { get; set; }

        [Required(ErrorMessage = "Fill up this field.")]
        [Display(Name = "Other Charges :")]
        public decimal OtherCharges { get; set; }

        [Required(ErrorMessage = "Fill up this field.")]
        [Display(Name = "Pay Mode :")]
        public string PayMode { get; set; }
        public string ReceiptNo { get; set; }

        [Display(Name = "Drawee Bank :")]
        public string DraweeBank { get; set; }

        [Display(Name = "Instrument No. :")]
        public string InstrumentNo { get; set; }

        [Display(Name = "Date :")]
        public string ChqDate { get; set; }

        [Display(Name = "Remarks :")]
        public string Remarks { get; set; }
        public List<ImpExpCha> lstImpExpCha { get; set; }
    }

    public class ImpExpCha
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }

    public class CashColAgnBncChqPrint
    {
        public IList<CashColAgnBncChqHeader> lstCashColAgnBncChqHeader { get; set; } = new List<CashColAgnBncChqHeader>();
        public IList<CashColAgnBncChqCharge> lstCashColAgnBncChqCharge { get; set; } = new List<CashColAgnBncChqCharge>();
        public IList<CashColAgnBncChqPayMode> lstCashColAgnBncChqPayMode { get; set; } = new List<CashColAgnBncChqPayMode>();
        public IList<CompanyDetails> lstCompanyDetails { get; set; } = new List<CompanyDetails>();
    }
    public class CashColAgnBncChqHeader
    {
        public string InvoiceType { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string PartyGSTNo { get; set; }
        public string PartyAddress { get; set; }
        public string PartyState { get; set; }
        public string PartyStateCode { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalIGST { get; set; }
        public decimal CWCTotal { get; set; }
        public decimal HTTotal { get; set; }
        public decimal CWCTDS { get; set; }
        public decimal HTTDS { get; set; }
        public decimal TDS { get; set; }
        public decimal TDSCol { get; set; }
        public decimal AllTotal { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceAmt { get; set; }
        public string CompGST { get; set; }
        public string CompPAN { get; set; }
        public string CompStateCode { get; set; }
        public string Remarks { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }

        public string SupplyType { get; set; } 
    }
    public class CashColAgnBncChqCharge
    {
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }
        public decimal Amount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Total { get; set; }
    }
    public class CashColAgnBncChqPayMode
    {
        public string PayMode { get; set; }
        public string InstrumentNo { get; set; }
        public string DraweeBank { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class CompanyDetails
    {
        public string ROAddress { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? StateId { get; set; }
        public string StateCode { get; set; }
        public int? CityId { get; set; }
        public string GstIn { get; set; }
        public string Pan { get; set; }
    }
}
		
