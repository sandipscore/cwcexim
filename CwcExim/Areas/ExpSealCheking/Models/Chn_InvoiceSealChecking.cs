using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class Chn_InvoiceSealChecking :Chn_InvoiceBase
    {

        //--------------------------------------------------------------------------------------------------------------------
        public List<Chn_PreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<Chn_PreInvoiceContainer>();
        public List<Chn_PostPaymentContainer> lstPostPaymentCont { get; set; } = new List<Chn_PostPaymentContainer>();
        public List<Chn_PostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<Chn_PostPaymentChrg>();
        public IList<Chn_ContainerWiseAmount> lstContWiseAmount { get; set; } = new List<Chn_ContainerWiseAmount>();

        public List<Chn_OperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<Chn_OperationCFSCodeWiseAmount>();

        public IList<string> ActualApplicable { get; set; } = new List<string>();

        public string SEZ { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
    }


    public class Chn_PostPaymentContainer : PostPaymentContainer
    {
        public string TruckSlipNo { get; set; }
        public string DeliveryDate { get; set; }
    }
    public class Chn_PostPaymentChrg : PostPaymentCharge
    {
       
    }

    public class Chn_ContainerWiseAmount : ContainerWiseAmount
    {
        public decimal RFIDEntryFee { get; set; } = 0M;
        public decimal DocumentVerificationFee { get; set; } = 0M;
        public decimal ContainerHandlingCharge { get; set; } = 0M;
        public decimal SealVerificationWith { get; set; } = 0M;
        public decimal SealVerificationWithout { get; set; } = 0M;
        public decimal ReSealing { get; set; } = 0M;
        public decimal Detention { get; set; } = 0M;
        public decimal Weighment { get; set; } = 0M;
        public decimal MiscCharge { get; set; } = 0M;        
    }

    public class Chn_PreInvoiceContainer : PreInvoiceContainer
    {
        public string TruckSlipNo { get; set; }
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
    }

    public class Chn_OperationCFSCodeWiseAmount
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