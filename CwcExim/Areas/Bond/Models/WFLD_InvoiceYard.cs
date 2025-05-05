using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Areas.Import.Models;
using CwcExim.Models;

namespace CwcExim.Areas.Bond.Models
{
    public class WFLD_InvoiceYard: WFLDInvoiceBase
    {
        public List<WFLD_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLD_PreInvoiceContainer>();
        public List<WFLD_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WFLD_PostPaymentContainer>();
        public List<WFLD_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLD_PostPaymentChrg>();
        public IList<WFLD_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLD_ContainerWiseAmount>();

        public List<WFLD_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLD_OperationCFSCodeWiseAmount>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();
    }

    public class WFLD_PostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLD_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WFLD_ContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class WFLD_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
    }

    public class WFLD_OperationCFSCodeWiseAmount
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