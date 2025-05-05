using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
namespace CwcExim.Areas.Export.Models
{
    public class DSR_Movement_Invoice : DSRInvoiceBase
    {
       
        public List<DSRPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DSRPreInvoiceContainer>();
        public List<DSRPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DSRPostPaymentContainer>();
        public List<DSRPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DSRPostPaymentChrg>();
        public IList<DSRContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DSRContainerWiseAmount>();
        public string invoicenoo { get; set; }
        public string invoicenooo { get; set; }
        public string MovementNo { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? CargoWeight { get; set; }
        public List<DSROperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DSROperationCFSCodeWiseAmount>();
        public List<DSRCMMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DSRCMMPostPaymentChargebreakupdate>();
      
        public int Cargo { get; set;}
        //--------------------------------------------------------------------------------------------------------------------
    }

    public class DSRPostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class DSRPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DSRContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class DSRPreInvoiceContainer : PreInvoiceContainer
    {

    }
    public class DSRCMMPostPaymentChargebreakupdate
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

    public class DSROperationCFSCodeWiseAmount
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
    }

}