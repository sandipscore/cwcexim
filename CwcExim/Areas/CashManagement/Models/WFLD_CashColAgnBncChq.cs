using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class WFLD_CashColAgnBncChq: CashColAgnBncChq
    {
        public string Invoice_No { get; set; }
    }
    public class WFLD_cashBncChqInvoice
    {
        public int Status { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class WFLD_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }

    }

    public class WFLDBounceChk
    {
        public decimal BouncedCheque { get; set; }
    }
    public class WFLD_PaymentPartyNameGroup
    {
        public List<WFLD_PaymentPartyName> partylist { get; set; }
        public WFLDBounceChk bouncecheck { get; set; }
    }
}