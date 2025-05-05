using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_PartyLedgerStatement
    {
        public string Party { get; set; }
        public int PartyId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class Ppg_PartyForLedger
    {
        public string Party { get; set; }
        public int PartyId { get; set; }
        public string PartyCode { get; set; }

    }

    public class Ppg_PartyLedger
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public List<Ppg_LedgerStatementList> LstOnAccountDtl { get; set; } = new List<Ppg_LedgerStatementList>();
        public List<Ppg_Summary> LstOnSum { get; set; } = new List<Ppg_Summary>();
        public decimal Deposit { get; set; }
        public decimal Invoice { get; set; }
        public decimal Balance { get; set; }
        public decimal Opening { get; set; }

        public decimal SDBalance { get; set; }

    }
    public class Ppg_LedgerStatementList
    {
        public string ReceivedDate { get; set; }
        public string ReceiptNo { get; set; }
        public string RefInvoiceNo { get; set; }
        public decimal DepositAmt { get; set; }
        public string InvoiceNo { get; set; }
        public decimal InvAmt { get; set; }

        public string Narration { get; set; }
        public string ContainerNo { get; set; }
        public string ChargeCode { get; set; }

        public string Cheque_No { get; set; }

    }

    public class Ppg_Summary
    {
        public String ReceiptNo { get; set; }
        public decimal Total { get; set; }
    }
}