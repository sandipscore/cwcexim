using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{

    public class WFLDRRCreditDebitInvoiceDetails
    {
        public int InvoiceId { get; set; } 
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }

        public List<WFLDPostPaymentContainerRRCD> lstContDetailsRRCD { get; set; } = new List<WFLDPostPaymentContainerRRCD>();
        public List<WFLDPostPaymentChrgRRCD> lstChrgDetailsRRCD { get; set; } = new List<WFLDPostPaymentChrgRRCD>();

    }



    public class WFLDInvoiceRRCreditDebit : WFLDInvoiceBase
    {
        public int InvoiceIdCRNote { get; set; } = 0;
        public List<WFLDPostPaymentContainerRRCD> lstPostPaymentContRRCD { get; set; } = new List<WFLDPostPaymentContainerRRCD>();
        public List<WFLDPostPaymentChrgRRCD> lstPostPaymentChrgRRCD { get; set; } = new List<WFLDPostPaymentChrgRRCD>();
    }
    public class WFLDPostPaymentContainerRRCD : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLDPostPaymentChrgRRCD : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }




}