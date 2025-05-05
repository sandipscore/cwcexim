using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WLJInvoiceYard :Wlj_InvoiceBase
    {        

        //--------------------------------------------------------------------------------------------------------------------
        public List<WLJPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<WLJPreInvoiceContainer>();
        public List<WLJPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<WLJPostPaymentContainer>();
        public List<WLJPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<WLJPostPaymentChrg>();
        public IList<WLJContainerWiseAmount> lstContWiseAmount { get; set; } = new List<WLJContainerWiseAmount>();
        public List<WLJPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<WLJPostPaymentChargebreakupdate>();

        public List<WLJOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<WLJOperationCFSCodeWiseAmount>();
        //--------------------------------------------------------------------------------------------------------------------

public string PaymentMode { get; set; }

        public string NDays { get; set; }
    }
    

    public class WLJPostPaymentContainer: PostPaymentContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class WLJPostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class WLJContainerWiseAmount: ContainerWiseAmount
    {

    }

    public class WLJPreInvoiceContainer: PreInvoiceContainer
    {

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }

    }

    public class WLJOperationCFSCodeWiseAmount
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



    public class WLJPostPaymentChargebreakupdate
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