using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Dnd_InvoiceYard : Dnd_InvoiceBase
    {
        public string ExportUnder { get; set; } = string.Empty;
        public string Printing { get; set; } = string.Empty;
        //--------------------------------------------------------------------------------------------------------------------
        public List<Dnd_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Dnd_PreInvoiceContainer>();
        public List<Dnd_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Dnd_PostPaymentContainer>();
        public List<Dnd_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Dnd_PostPaymentChrg>();
        public IList<Dnd_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Dnd_ContainerWiseAmount>();

        public List<Dnd_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Dnd_OperationCFSCodeWiseAmount>();
        public List<Dnd_PostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<Dnd_PostPaymentChargebreakupdate>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        //--------------------------------------------------------------------------------------------------------------------
    }
    
     
    public class Dnd_PostPaymentContainer: PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
        public int ISODC { get; set; } = 0;
    }
    public class Dnd_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class Dnd_ContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class Dnd_PreInvoiceContainer : PreInvoiceContainer
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public int ISODC { get; set; } = 0;
        public string PayMode { get; set; }     
        public decimal SDBalance { get; set; }
    }

    public class Dnd_OperationCFSCodeWiseAmount
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
    public class Dnd_PostPaymentChargebreakupdate
    {
        public int ChargeId { get; set; } = 0;
        public string Clause { get; set; } = string.Empty;
        public int ClauseOrder { get; set; } = 0;
        public string ChargeName { get; set; } = string.Empty;
        public string ChargeType { get; set; } = string.Empty;
        public string SACCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal Rate { get; set; } = 0M;
        public decimal Amount { get; set; } = 0M;

        public int OperationId { get; set; }
        public string CFSCode { get; set; }
        public string Startdate { get; set; }
        public string EndDate { get; set; }
    }
}