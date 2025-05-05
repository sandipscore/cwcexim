using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class CHN_InvoiceYard: Hdb_InvoiceBase
    {
        public List<CHN_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<CHN_PreInvoiceContainer>();
        public List<CHN_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<CHN_PostPaymentContainer>();
        public List<CHN_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<CHN_PostPaymentChrg>();
        public IList<CHN_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<CHN_ContainerWiseAmount>();

        public List<CHN_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<CHN_OperationCFSCodeWiseAmount>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();
    }

    public class CHN_PostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class CHN_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class CHN_ContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class CHN_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
    }

    public class CHN_OperationCFSCodeWiseAmount
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