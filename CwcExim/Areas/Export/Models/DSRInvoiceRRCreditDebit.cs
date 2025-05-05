using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{

    public class DSRRRCreditDebitInvoiceDetails
    {
        public int InvoiceId { get; set; } 
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }

        public List<DSRPostPaymentContainerRRCD> lstContDetailsRRCD { get; set; } = new List<DSRPostPaymentContainerRRCD>();
        public List<DSRPostPaymentChrgRRCD> lstChrgDetailsRRCD { get; set; } = new List<DSRPostPaymentChrgRRCD>();

    }



    public class DSRInvoiceRRCreditDebit : DSRInvoiceBase
    {
        public int InvoiceIdCRNote { get; set; } = 0;
        public List<DSRPostPaymentContainerRRCD> lstPostPaymentContRRCD { get; set; } = new List<DSRPostPaymentContainerRRCD>();
        public List<DSRPostPaymentChrgRRCD> lstPostPaymentChrgRRCD { get; set; } = new List<DSRPostPaymentChrgRRCD>();
    }
    public class DSRPostPaymentContainerRRCD : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSRPostPaymentChrgRRCD : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }




}