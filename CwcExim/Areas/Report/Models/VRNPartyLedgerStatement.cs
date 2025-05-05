using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class VRNPartyLedgerStatement
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
    }

    public class VRNPartyForLedger
    {
        public string Party { get; set; }

        public int PartyId { get; set; }

        public string PartyCode { get; set; }

    }

    public class VRNPartyLedger
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string PartyGst { get; set; }
        public List<VRNLedgerStatementList> LstOnAccountDtl { get; set; } = new List<VRNLedgerStatementList>();


        public List<VRNSummary> LstOnSum { get; set; } = new List<VRNSummary>();


        public decimal Deposit { get; set; }
        public decimal Invoice { get; set; }
        public decimal Balance { get; set; }
        public decimal Opening { get; set; }

        public decimal SDBalance { get; set; }

    }
    public class VRNLedgerStatementList
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

    public class VRNSummary
    {
        public String ReceiptNo { get; set; }
        public decimal Total { get; set; }
    }

}