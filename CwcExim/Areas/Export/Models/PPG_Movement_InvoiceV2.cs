using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class PPG_Movement_InvoiceV2 : PPG_InvoiceBase
    {

        public List<PpgPreInvoiceContainerV2> lstPrePaymentCont { get; set; } = new List<PpgPreInvoiceContainerV2>();
        public List<PpgPostPaymentContainerV2> lstPostPaymentCont { get; set; } = new List<PpgPostPaymentContainerV2>();
        public List<PpgPostPaymentChrgV2> lstPostPaymentChrg { get; set; } = new List<PpgPostPaymentChrgV2>();
        public IList<PpgContainerWiseAmountV2> lstContWiseAmount { get; set; } = new List<PpgContainerWiseAmountV2>();
        public string invoicenoo { get; set; }
        public string invoicenooo { get; set; }
        public string MovementNo { get; set; }
        public decimal? TareWeight { get; set; }
        public decimal? CargoWeight { get; set; }
        public List<PpgOperationCFSCodeWiseAmountV2> lstOperationCFSCodeWiseAmount { get; set; } = new List<PpgOperationCFSCodeWiseAmountV2>();
        public List<ppgCMMPostPaymentChargebreakupdateV2> lstPostPaymentChrgBreakup { get; set; } = new List<ppgCMMPostPaymentChargebreakupdateV2>();
        public List<PpgShipbillwiseAmountV2> lstShipbillwiseAmountV2 { get; set; } = new List<PpgShipbillwiseAmountV2>();

        public int Cargo { get; set; }
        public decimal? ElwbTareWeight { get; set; }
        public decimal? ElwbCargoWeight { get; set; }

        //--------------------------------------------------------------------------------------------------------------------
    }


    public class PpgPostPaymentContainerV2 : PostPaymentContainer
    {
        public string DeliveryDate { get; set; }
    }
    public class PpgPostPaymentChrgV2 : PostPaymentCharge
    {
        public int OperationId { get; set; }
    }

    public class PpgContainerWiseAmountV2 : ContainerWiseAmount
    {

    }

    public class PpgPreInvoiceContainerV2 : PreInvoiceContainer
    {

    }
    public class ppgCMMPostPaymentChargebreakupdateV2
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

    public class PpgOperationCFSCodeWiseAmountV2
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