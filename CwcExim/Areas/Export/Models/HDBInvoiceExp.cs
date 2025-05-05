using CwcExim.Areas.Import.Models;
using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class HDBInvoiceExp : Hdb_InvoiceBase
    {

        public string ExportUnder { get; set; }
        public int Distance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<HDBPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<HDBPreInvoiceContainer>();
        public List<HDBPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<HDBPostPaymentContainer>();
        public List<HDBPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<HDBPostPaymentChrg>();
        public IList<HDBContainerWiseAmount> lstContWiseAmount { get; set; } = new List<HDBContainerWiseAmount>();
        public List<HDBOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<HDBOperationCFSCodeWiseAmount>();
        public List<HDBPreInvoiceCargo> lstPreInvoiceCargo { get; set; } = new List<HDBPreInvoiceCargo>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();

        //--------------------------------------------------------------------------------------------------------------------
    }
    public class HDBPostPaymentContainer : Hdb_PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class HDBPostPaymentChrg : PostPaymentCharge
    {
        public int Operationd { get; set; }
    }

    public class HDBContainerWiseAmount : Hdb_ContainerWiseAmount
    {

    }

    public class HDBPreInvoiceContainer : Hdb_PreInvoiceContainer
    {

    }

    public class HDBOperationCFSCodeWiseAmount
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

        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public decimal Weight { get; set; }
        public decimal CIFValue { get; set; }

    }

    public class HDBPreInvoiceCargo : PreInvoiceCargo
    {
    }
}