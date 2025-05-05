using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
namespace CwcExim.Areas.Export.Models
{
    public class WFLD_Movement_Invoice : WFLDInvoiceBase
    {
       
        public List<WFLDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WFLDPreInvoiceContainer>();
        public List<WFLDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WFLDPostPaymentContainer>();
        public List<WFLDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WFLDPostPaymentChrg>();
        public IList<WFLDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WFLDContainerWiseAmount>();
        public string invoicenoo { get; set; }
        public string invoicenooo { get; set; }
        public string MovementNo { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? CargoWeight { get; set; }
        public List<WFLDOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WFLDOperationCFSCodeWiseAmount>();
        public List<WFLDCMMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WFLDCMMPostPaymentChargebreakupdate>();
      
        public int Cargo { get; set;}
        //--------------------------------------------------------------------------------------------------------------------
    }


    public class WFLDPostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class WFLDPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WFLDContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class WFLDPreInvoiceContainer : PreInvoiceContainer
    {

    }
    public class WFLDCMMPostPaymentChargebreakupdate
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

    public class WFLDOperationCFSCodeWiseAmount
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