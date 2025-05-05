using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_CashColAgnBncChq: CashColAgnBncChq
    {
        public string Invoice_No { get; set; }
        public string SEZ { get; set; }
    }
    public class DSR_cashBncChqInvoice
    {
        public int Status { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class DSR_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

    }

    public class DSRBounceChk
    {
        public decimal BouncedCheque { get; set; }
    }
    public class DSR_PaymentPartyNameGroup
    {
        public List<DSR_PaymentPartyName> partylist { get; set; }
        public DSRBounceChk bouncecheck { get; set; }
    }
}