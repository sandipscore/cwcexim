using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;
namespace CwcExim.Areas.Export.Models
{
    public class PPG_Movement_Invoice : PpgInvoiceBase
    {
       
        public List<PpgPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainer>();
        public List<PpgPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainer>();
        public List<PpgPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrg>();
        public IList<PpgContainerWiseAmount> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmount>();
        public string invoicenoo { get; set; }
        public string invoicenooo { get; set; }
        public string MovementNo { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? CargoWeight { get; set; }
        public List<PpgOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmount>();
        public List<ppgCMMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<ppgCMMPostPaymentChargebreakupdate>();
      
        public int Cargo { get; set;}
        //--------------------------------------------------------------------------------------------------------------------
    }


    public class PpgPostPaymentContainer : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmount : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainer : PreInvoiceContainer
    {

    }
    public class ppgCMMPostPaymentChargebreakupdate
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

    public class PpgOperationCFSCodeWiseAmount
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