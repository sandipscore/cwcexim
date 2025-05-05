using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Kdl_CashReceiptModel
    {
        public string ReceiptNo { get; set; }
        // public int BranchId { get; set; }
        public string ReceiptDate { get; set; }
        public string InvoiceDate { get; set; }

        [Required(ErrorMessage = "Please Choose Invoice No.")]
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }
        public string PartyName { get; set; }
        public int PartyId { get; set; }
        public IList<KdlInvoice> InvoiceDetail { get; set; } = new List<KdlInvoice>();
        public IList<KdlPayBy> PayByDetail { get; set; } = new List<KdlPayBy>();

        public List<KdlParty> PartyDetail { get; set; } = new List<KdlParty>();

        public List<KdlCashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<KdlCashReceiptInvoiveMapping>();

        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }
        public IList<KdlCashReceipt> CashReceiptDetail { get; set; } = new List<KdlCashReceipt>();
        public bool PdaAdjust { get; set; }
        public string FolioNo { get; set; }

        [Range(0, 99999999999999.99, ErrorMessage = "Value should be 0 or above.")]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Value Can't be negative.")]
        public decimal Adjusted { get; set; }
        public decimal Opening { get; set; }

        [Range(0, 99999999999999.99, ErrorMessage = "Value should be 0 or above.")]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Value Can't be negative.")]
        public decimal Closing { get; set; }

        [Range(0, 99999999999999.99, ErrorMessage = "Value should be 0 or above.")]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Value Can't be negative.")]
        public decimal TotalPaymentReceipt { get; set; }
        public decimal TotalValue { get; set; } // Total CWC and HT Charges
        public decimal RoundUp { get; set; }

        [Range(0, 99999999999999.99, ErrorMessage = "Value should be 0 or above.")]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "Value Can't be negative.")]
        public decimal TdsAmount { get; set; }
        public decimal TdsBalanceAmount { get; set; }
        public decimal InvoiceValue { get; set; }

        public IList<KdlCwcChargesType> CWCChargeType { get; set; } = new List<KdlCwcChargesType>();
        public decimal TotalCwcCharges { get; set; } = 0;

        public IList<KdlHTChargesType> HTChargeType { get; set; } = new List<KdlHTChargesType>();
        public decimal TotalHTCharges { get; set; } = 0;

        public IList<KdlPdaAdjust> PdaAdjustdetail { get; set; } = new List<KdlPdaAdjust>();
        public IList<KdlContainer> ContainerDetail { get; set; } = new List<KdlContainer>();

        public string GSTNo { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public string CFSCode { get; set; }
        public string CIFValue { get; set; }

        public int PayByTraderId { get; set; }
        public string PayByName { get; set; }
        public string InvoiceHtml { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Remarks")]

        public string Remarks { get; set; }

        public string CashReceiptInvDtlsHtml { get; set; }

        public decimal TotalPayableAmt { get; set; } = 0;
        public string Type { get; set; } = "";

        public decimal BalOnAccount { get; set; }
        public decimal OnAcDepositAmt { get; set; } = 0;

    }

    public class KdlInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class KdlCashReceiptInvoiveMapping
    {
        public int CashReceiptId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal AllTotalAmt { get; set; }
        public decimal RoundUp { get; set; }
        public decimal InvoiceAmt { get; set; }
        public decimal DueAmt { get; set; }
        public decimal AdjustmentAmt { get; set; }
        public bool IsSelected { get; set; }
    }


    public class KdlPayBy
    {
        public int PayByEximTraderId { get; set; }
        public string PayByName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
    public class KdlPdaAdjust
    {
        public int PayByPdaId { get; set; }
        public int EximTraderId { get; set; }
        public string FolioNo { get; set; }
        //  public decimal Adjusted { get; set; }
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
    }
    public class KdlCashReceipt
    {
        //:: Cash Mode if pda no selected ::::::
        public string PaymentMode { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Invalid Character.")]
        [StringLength(maximumLength: 45, ErrorMessage = "Contain Only 45 Character.")]
        public string DraweeBank { get; set; }

        [RegularExpression("^[a-zA-Z0-9/ ]*$", ErrorMessage = "Invalid Character.")]
        [StringLength(maximumLength: 45, ErrorMessage = "Contain Only 45 Character.")]
        public string InstrumentNo { get; set; }
        public string Date { get; set; }

        [RegularExpression("^[0-9.]*$", ErrorMessage = "Invalid Character.")]
        public decimal? Amount { get; set; }
        //::::::::::::::::::::::::::::::::::::
    }

    public class KdlCwcChargesType
    {
        // public int DBChargeID { get; set; }
        // public string ChargeId { get; set; }
        // public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGSTAmt { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGSTAmt { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGSTAmt { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }

    public class KdlHTChargesType
    {
        // public int DBChargeID { get; set; }
        // public string ChargeId { get; set; }
        // public string ChargeType { get; set; }
        public string ChargeName { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;
        public decimal IGSTAmt { get; set; } = 0;
        public decimal CGSTPer { get; set; } = 0;
        public decimal CGSTAmt { get; set; } = 0;
        public decimal SGSTPer { get; set; } = 0;
        public decimal SGSTAmt { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }
    public class KdlContainer
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ArrivalDate { get; set; }
    }

    public class KdlParty
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string GSTNo { get; set; }
    }

}