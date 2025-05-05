using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Hdb_CashReceiptModel//: Kol_CashReceiptModel
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
        public string Tan { get; set; }
        public IList<Invoice> InvoiceDetail { get; set; } = new List<Invoice>();
        public IList<PayBy> PayByDetail { get; set; } = new List<PayBy>();

        public List<Hdb_Party> PartyDetail { get; set; } = new List<Hdb_Party>();

        public List<CashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<CashReceiptInvoiveMapping>();

        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }
        public IList<CashReceipt> CashReceiptDetail { get; set; } = new List<CashReceipt>();
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

        public IList<CwcChargesType> CWCChargeType { get; set; } = new List<CwcChargesType>();
        public decimal TotalCwcCharges { get; set; } = 0;

        public IList<HTChargesType> HTChargeType { get; set; } = new List<HTChargesType>();
        public decimal TotalHTCharges { get; set; } = 0;

        public IList<PdaAdjust> PdaAdjustdetail { get; set; } = new List<PdaAdjust>();
        public IList<Container> ContainerDetail { get; set; } = new List<Container>();

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
    }
    public class Hdb_Party
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Tan { get; set; }
    }

    public class ListOfPDReceipt
    {
        public int PDid { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Tan { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyAddress { get; set; }
        public decimal Amount { get; set; }
    }
}