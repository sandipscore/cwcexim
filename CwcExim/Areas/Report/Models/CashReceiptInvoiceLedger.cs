using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CashReceiptInvoiceLedger
    {
        
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EximTraderName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string GSTNo { get; set; }
        public string PinCode { get; set; }
        public string COMGST { get; set; }
        public string COMPAN { get; set; }

        public decimal OpenningBalance { get; set; } = 0;
        public decimal ClosingBalance { get; set; } = 0;

        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalCredit { get; set; } = 0;

        public string CurDate { get; set; }
        public List<CrInvLedgerSummary> lstLedgerSummary { get; set; } = new List<CrInvLedgerSummary>();
        public List<CrInvLedgerDetails> lstLedgerDetails { get; set; } = new List<CrInvLedgerDetails>();
        public List<CrInvLedgerFullDetails> lstLedgerDetailsFull { get; set; } = new List<CrInvLedgerFullDetails>();
        public List<CrInvLedgerBalance> lstLedgerBalance { get; set; } = new List<CrInvLedgerBalance>();
    }

    public class CrInvLedgerSummary
    {
        public int InvCr { get; set; }
        public int InvCrId { get; set; }
        public string InvCrDate { get; set; }
        public string InvCrNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string CreatedOn { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public List<CrInvLedgerDetails> LedgerDetails { get; set; } = new List<CrInvLedgerDetails>();
    }

    public class CrInvLedgerDetails
    {
        public int InvCr { get; set; }
        public int InvCrId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class CrInvLedgerFullDetails
    {
        //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
        public int Sr { get; set; }
        public string ReceiptDt { get; set; }
        public string ReceiptNo { get; set; }
        public string ChargeCode { get; set; }
        public string ContNo { get; set; }
        public string Size { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string GroupSr { get; set; }
       
    }
    public class CrInvLedgerBalance
    {
        //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
       
        public decimal Total { get; set; }
     
    }
}