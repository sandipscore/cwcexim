using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{

    public class PpgRRCreditDebitInvoiceDetails
    {
        public int InvoiceId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }

        public List<PpgPostPaymentContainerRRCD> lstContDetailsRRCD { get; set; } = new List<PpgPostPaymentContainerRRCD>();
        public List<PpgPostPaymentChrgRRCD> lstChrgDetailsRRCD { get; set; } = new List<PpgPostPaymentChrgRRCD>();

    }



    public class PpgInvoiceRRCreditDebit: PpgInvoiceBase
    {
        public int InvoiceIdCRNote { get; set; } = 0;
        public List<PpgPostPaymentContainerRRCD> lstPostPaymentContRRCD { get; set; } = new List<PpgPostPaymentContainerRRCD>();
        public List<PpgPostPaymentChrgRRCD> lstPostPaymentChrgRRCD { get; set; } = new List<PpgPostPaymentChrgRRCD>();
    }
    public class PpgPostPaymentContainerRRCD : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgRRCD : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }




}