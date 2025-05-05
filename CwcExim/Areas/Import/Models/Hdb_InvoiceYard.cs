using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_InvoiceYard : Hdb_InvoiceBase
    {
        public string ExportUnder { get; set; } = string.Empty;
        public string Printing { get; set; } = string.Empty;

        public string Distance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<Hdb_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Hdb_PreInvoiceContainer>();
        public List<Hdb_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Hdb_PostPaymentContainer>();
        public List<Hdb_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Hdb_PostPaymentChrg>();
        public IList<Hdb_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Hdb_ContainerWiseAmount>();

        public List<Hdb_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Hdb_OperationCFSCodeWiseAmount>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------------------
    }
    

    public class Hdb_PostPaymentContainer: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
        public int ISODC { get; set; } = 0;
    }
    public class Hdb_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class Hdb_ContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class Hdb_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public int ISODC { get; set; } = 0;
    }

    public class Hdb_OperationCFSCodeWiseAmount
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
        public string BOLNo { get; set; }
    }

}