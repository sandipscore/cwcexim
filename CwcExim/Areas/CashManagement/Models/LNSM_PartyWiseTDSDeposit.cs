using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_PartyWiseTDSDeposit
    {       
        public int Id { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CertificateNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CertificateDate { get; set; }
        public string ReceiptDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public decimal Amount { get; set; }
        public string DepositDate { get; set; }
        public decimal TDSBalance { get; set; }
        public string IsCan { get; set; }
        public int FinancialYear { get; set; } = 0;
        public int FinancialYearNext { get; set; } = 0;
        public string TdsQuarter { get; set; } = "";
        public string PeriodTo { get; set; } = "";
        public string PeriodFrom { get; set; } = "";
        public string Remarks { get; set; } = "";
        public string ReceiptNo { get; set; } = "";
        public decimal TotalTDSDeducted { get; set; }
        public decimal TDSCertAmount { get; set; }
        public decimal UnadjustedAmount { get; set; }
        public List<LNSM_TDSCertificateMapping> CertificateMappingList { get; set; } = new List<LNSM_TDSCertificateMapping>();
        public string CashReceiptInvDtlsHtml { get; set; }
    }
    public class LNSM_TDSCertificateMapping
    {
        public int CashReceiptId { get; set; }        
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public bool IsSelected { get; set; }
    }

}