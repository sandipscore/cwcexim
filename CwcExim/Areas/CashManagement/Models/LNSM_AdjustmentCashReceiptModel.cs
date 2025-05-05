using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_AdjustmentCashReceiptModel
    {
        public int AdjCashReceiptId { get; set; }
        public string ReceiptNo { get; set; }       
        public string ReceiptDate { get; set; }
        public string InvoiceDate { get; set; }       

        [Required(ErrorMessage = "Please Choose Document No.")]
        public string DocumentNo { get; set; }
        public int DocumentId { get; set; }
        public string PartyName { get; set; }
        public int PartyId { get; set; }
        public IList<LNSM_AdjustmentInvoice> InvoiceDetail { get; set; } = new List<LNSM_AdjustmentInvoice>();
        public IList<LNSM_PayBy> PayByDetail { get; set; } = new List<LNSM_PayBy>();

        public List<LNSM_Party> PartyDetail { get; set; } = new List<LNSM_Party>();

        public List<LNSM_CashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<LNSM_CashReceiptInvoiveMapping>();
        public List<LNSM_AdjustmentCashReceiptInvoiveMapping> AdjCashReceiptInvoiveMappingList { get; set; } = new List<LNSM_AdjustmentCashReceiptInvoiveMapping>();
        public List<LNSM_AdjustmentCashReceiptMapping> AdjustmentMappingList { get; set; } = new List<LNSM_AdjustmentCashReceiptMapping>();

        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }
        //public IList<LNSM_CashReceipt> CashReceiptDetail { get; set; } = new List<LNSM_CashReceipt>();
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

        public IList<LNSM_CwcChargesType> CWCChargeType { get; set; } = new List<LNSM_CwcChargesType>();
        public decimal TotalCwcCharges { get; set; } = 0;

        public IList<LNSM_HTChargesType> HTChargeType { get; set; } = new List<LNSM_HTChargesType>();
        public decimal TotalHTCharges { get; set; } = 0;

        public IList<LNSM_PdaAdjust> PdaAdjustdetail { get; set; } = new List<LNSM_PdaAdjust>();
        public IList<LNSM_Container> ContainerDetail { get; set; } = new List<LNSM_Container>();
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
        public string AdjustReceiptInvDtlsHtml { get; set; }
        public decimal TotalPayableAmt { get; set; } = 0;
        public string Type { get; set; } = "";
        public decimal BalanceAmt { get; set; } = 0;
    }
    public class LNSM_AdjustmentInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class LNSM_AdjustmentCashReceiptInvoiveMapping
    {
        public int CashReceiptId { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string DocType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal AdjustAmount { get; set; }
        public bool IsSelected { get; set; }
    }
    
    public class LNSM_AdjustmentCashReceipt
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
       
    }

    public class LNSM_AdjustmentCashReceiptMapping
    {
        public int Id { get; set; }       
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public decimal Amount { get; set; }
        public decimal DueAmount { get; set; }
        public decimal AdjustAmount { get; set; }
        public bool IsSelected { get; set; }
    }
}