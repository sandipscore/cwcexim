using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwcExim.Models;
using CwcExim.Areas.Import.Models;

namespace CwcExim.Areas.Export.Models
{   
    public class VLDA_MovementInvoice : WFLD_InvoiceBase
    {
        public int hasSDAvailableBalance { get; set; }
        //--------------------------------------------------------------------------------------------------------------------
        public List<VLDAPreInvoiceContainer> lstPrePaymentCont { get; set; } = new List<VLDAPreInvoiceContainer>();
        public List<VLDAPostPaymentContainer> lstPostPaymentCont { get; set; } = new List<VLDAPostPaymentContainer>();
        public List<VLDAPostPaymentChrg> lstPostPaymentChrg { get; set; } = new List<VLDAPostPaymentChrg>();
        public IList<VLDAContainerWiseAmount> lstContWiseAmount { get; set; } = new List<VLDAContainerWiseAmount>();
        public List<VLDACMPostPaymentChargebreakupdate> lstPostPaymentChrgBreakup { get; set; } = new List<VLDACMPostPaymentChargebreakupdate>();
        public List<VLDAOperationCFSCodeWiseAmount> lstOperationCFSCodeWiseAmount { get; set; } = new List<VLDAOperationCFSCodeWiseAmount>();
        //--------------------------------------------------------------------------------------------------------------------
    }


    public class VLDAPostPaymentContainerGodown : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class VLDAPostPaymentChrgGodown : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class VLDAContainerWiseAmountGodown : ContainerWiseAmount
    {

    }

    public class VLDAPreInvoiceContainerGodown : PreInvoiceContainer
    {

    }
    public class VLDACMPostPaymentChargebreakupdate
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
    public class VLDATentativeInvoice
    {
        public static WFLD_MovementInvoice InvoiceObjW;
        public static WFLD_MovementInvoice InvoiceObjGR;
        public static WFLD_MovementInvoice InvoiceObjFMC;
    }



    public class VLDAOperationCFSCodeWiseAmountGodown
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