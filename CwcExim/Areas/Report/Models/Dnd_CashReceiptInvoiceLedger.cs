using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_CashReceiptInvoiceLedger
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
        public List<Dnd_CrInvLedgerSummary> lstDndLedgerSummary { get; set; } = new List<Dnd_CrInvLedgerSummary>();
        public List<Dnd_CrInvLedgerDetails> lstDndLedgerDetails { get; set; } = new List<Dnd_CrInvLedgerDetails>();
        public List<Dnd_CrInvLedgerFullDetails> lstDndLedgerDetailsFull { get; set; } = new List<Dnd_CrInvLedgerFullDetails>();
        public List<Dnd_CrInvLedgerBalance> lstDndLedgerBalance { get; set; } = new List<Dnd_CrInvLedgerBalance>();
        public List<Dnd_CrInvLedgerTotal> lstDndLedgerTotal { get; set; } = new List<Dnd_CrInvLedgerTotal>();
    }
   

    public class Dnd_CrInvLedgerSummary
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
        public List<Dnd_CrInvLedgerDetails> LedgerDetails { get; set; } = new List<Dnd_CrInvLedgerDetails>();
    }

    public class Dnd_CrInvLedgerDetails
    {
        public int InvCr { get; set; }
        public int InvCrId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ChargeName { get; set; }
        public string HSNCode { get; set; }
    }

    public class Dnd_CrInvLedgerFullDetails
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
        public string ChargeName { get; set; }
        public string HSNCode { get; set; }


    }
    public class Dnd_CrInvLedgerBalance
    {
        //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId

        public decimal Total { get; set; }

    }
    public class Dnd_CrInvLedgerTotal
    {
        //Sr, ReceiptDt, ReceiptNo, ChargeCode, ContNo, Size, Debit, Credit, Balance, GroupSr, InvCr, InvCrId
        public string Charge { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
     


    }
}