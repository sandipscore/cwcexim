using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_CashReceiptModel
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
        public IList<DSRInvoice> InvoiceDetail { get; set; } = new List<DSRInvoice>();
        public IList<DSRPayBy> PayByDetail { get; set; } = new List<DSRPayBy>();

        public List<DSRParty> PartyDetail { get; set; } = new List<DSRParty>();

        public List<DSRCashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<DSRCashReceiptInvoiveMapping>();

        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }
        public IList<DSRCashReceipt> CashReceiptDetail { get; set; } = new List<DSRCashReceipt>();
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

        public IList<DSRCwcChargesType> CWCChargeType { get; set; } = new List<DSRCwcChargesType>();
        public decimal TotalCwcCharges { get; set; } = 0;

        public IList<DSRHTChargesType> HTChargeType { get; set; } = new List<DSRHTChargesType>();
        public decimal TotalHTCharges { get; set; } = 0;

        public IList<DSRPdaAdjust> PdaAdjustdetail { get; set; } = new List<DSRPdaAdjust>();
        public IList<DSRContainer> ContainerDetail { get; set; } = new List<DSRContainer>();

        public string GSTNo { get; set; }
        public string CartingDate { get; set; }
        public string StuffingDate { get; set; }
        public string CFSCode { get; set; }
        public string CIFValue { get; set; }

        public int PayByTraderId { get; set; }
        public string PayByName { get; set; }
        public string InvoiceHtml { get; set; }

        public string Remarks { get; set; }

        public string CashReceiptInvDtlsHtml { get; set; }

        public decimal TotalPayableAmt { get; set; } = 0;
        public string Type { get; set; } = "";

        public decimal BalOnAccount { get; set; }
        public decimal OnAcDepositAmt { get; set; } = 0;

    }
    public class DSRInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class DSRCashReceiptInvoiveMapping
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


    public class DSRPayBy
    {
        public int PayByEximTraderId { get; set; }
        public string PayByName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
    public class DSRPdaAdjust
    {
        public int PayByPdaId { get; set; }
        public int EximTraderId { get; set; }
        public string FolioNo { get; set; }
        //  public decimal Adjusted { get; set; }
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
    }
    public class DSRCashReceipt
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

    public class DSRCwcChargesType
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

    public class DSRHTChargesType
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
    public class DSRContainer
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ArrivalDate { get; set; }
    }
}
 