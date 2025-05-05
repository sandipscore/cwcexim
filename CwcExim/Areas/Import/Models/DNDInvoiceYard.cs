using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DNDInvoiceYard :DNDInvoiceBase
    {        

        //--------------------------------------------------------------------------------------------------------------------
        public List<DNDPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<DNDPreInvoiceContainer>();
        public List<DNDPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<DNDPostPaymentContainer>();
        public List<DNDPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<DNDPostPaymentChrg>();
        public IList<DNDContainerWiseAmount> lstContWiseAmount { get; set; } = new List<DNDContainerWiseAmount>();
        public List<DNDPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<DNDPostPaymentChargebreakupdate>();

        public List<DNDOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<DNDOperationCFSCodeWiseAmount>();
        public IList<string> ActualApplicable { get; set; } = new List<string>();
        public int DirectDeStuff { get; set; }
        //--------------------------------------------------------------------------------------------------------------------

        public string PaymentMode { get; set; }

        public string NDays { get; set; }
    }
    

    public class DNDPostPaymentContainer: PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }
        public int ISODC { get; set; } = 0;
    }
    public class DNDPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class DNDContainerWiseAmount: ContainerWiseAmount
    {

    }

    public class DNDPreInvoiceContainer: PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public int ISODC { get; set; } = 0;
        public string PayMode { get; set; }       
        public decimal SDBalance { get; set; }
    }

    public class DNDOperationCFSCodeWiseAmount
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
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
    }



    public class DNDPostPaymentChargebreakupdate
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