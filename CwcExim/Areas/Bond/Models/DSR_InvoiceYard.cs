using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{ 
    public class DSR_InvoiceYard: DSRInvoiceBase
    {
        public List<DSR_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSR_PreInvoiceContainer>();
        public List<DSR_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DSR_PostPaymentContainer>();
        public List<DSR_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DSR_PostPaymentChrg>();
        public IList<DSR_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DSR_ContainerWiseAmount>();

        public List<DSR_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSR_OperationCFSCodeWiseAmount>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();
    }

    public class DSR_PostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSR_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DSR_ContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class DSR_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
    } 

    public class DSR_OperationCFSCodeWiseAmount
    {
        public int InvoiceId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int OperationId { get; set; }
        public string ChargeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Clause { get; set; }
    }
}