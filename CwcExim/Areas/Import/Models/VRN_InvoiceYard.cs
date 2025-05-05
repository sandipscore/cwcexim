using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_InvoiceYard :VRN_InvoiceBase
    {        

        //--------------------------------------------------------------------------------------------------------------------
        public List<VRN_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VRN_PreInvoiceContainer>();
        public List<VRN_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VRN_PostPaymentContainer>();
        public List<VRN_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VRN_PostPaymentChrg>();
        public IList<VRN_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VRN_ContainerWiseAmount>();
        public List<VRN_PostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VRN_PostPaymentChargebreakupdate>();

        public List<VRN_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VRN_OperationCFSCodeWiseAmount>();


        public string lstPrePaymentContXML { get; set; }
        public string lstPostPaymentContXML { get; set; }
        public string lstPostPaymentChrgXML { get; set; }
        public string lstContWiseAmountXML { get; set; }
        public string lstOperationCFSCodeWiseAmountXML { get; set; }
        public string lstPostPaymentChrgBreakupXML { get; set; }

        //--------------------------------------------------------------------------------------------------------------------

        public string PaymentMode { get; set; }

        public string NDays { get; set; }

        public bool IsLocalGST { get; set; }
    }
    
    public class VRN_PostPaymentChrg : PostPaymentCharge
    {
        public int OperationId { get; set; }        
    }

    
    public class VRN_PostPaymentChargebreakupdate
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