using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_CashReceiptModel
    {
        public string ReceiptNo { get; set; }       
        public string ReceiptDate { get; set; }
        public string InvoiceDate { get; set; }       

        [Required(ErrorMessage = "Please Choose Invoice No.")]
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }
        public string PartyName { get; set; }
        public int PartyId { get; set; }
        public IList<LNSM_Invoice> InvoiceDetail { get; set; } = new List<LNSM_Invoice>();
        public IList<LNSM_PayBy> PayByDetail { get; set; } = new List<LNSM_PayBy>();

        public List<LNSM_Party> PartyDetail { get; set; } = new List<LNSM_Party>();

        public List<LNSM_CashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<LNSM_CashReceiptInvoiveMapping>();
        
        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }
        public IList<LNSM_CashReceipt> CashReceiptDetail { get; set; } = new List<LNSM_CashReceipt>();
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
        public decimal TotalPayableAmt { get; set; } = 0;
        public string Type { get; set; } = "";
        public decimal BalanceAmt { get; set; } = 0;
    }
    public class LNSM_Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class LNSM_CashReceiptInvoiveMapping
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


    public class LNSM_PayBy
    {
        public int PayByEximTraderId { get; set; }
        public string PayByName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }

    public class LNSM_Party
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
    }

    public class LNSM_PayeeForPage
    {
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string PartyCode { get; set; }
    }
    public class LNSM_PdaAdjust
    {
        public int PayByPdaId { get; set; }
        public int EximTraderId { get; set; }
        public string FolioNo { get; set; }    
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
    }
    public class LNSM_CashReceipt
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

    public class LNSM_CwcChargesType
    {       
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

    public class LNSM_HTChargesType
    {       
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
    public class LNSM_Container
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string ArrivalDate { get; set; }
    }

    //BulkReceipt

    public class LNSM_BulkReceipt
    {        

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string ReceiptNumber { get; set; }

    }
}