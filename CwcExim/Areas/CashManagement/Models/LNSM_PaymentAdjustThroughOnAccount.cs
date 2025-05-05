using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_PaymentAdjustThroughOnAccount
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
        public List<LNSM_CashReceiptInvoiveMapping> CashReceiptInvoiveMappingList { get; set; } = new List<LNSM_CashReceiptInvoiveMapping>();
        public int PayByPdaId { get; set; }
        public string PayBy { get; set; }    
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

        public IList<LNSM_PdaAdjust> PdaAdjustdetail { get; set; } = new List<LNSM_PdaAdjust>();   

        public string GSTNo { get; set; }
        public string CartingDate { get; set; }
      
        public int PayByTraderId { get; set; }
        public string PayByName { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Remarks")]
        public string Remarks { get; set; }      

        public decimal TotalPayableAmt { get; set; } = 0;
        public string Type { get; set; } = "";

        public decimal BalOnAccount { get; set; }
        public decimal OnAcDepositAmt { get; set; } = 0;

        public decimal RemainingOnAccountBal { get; set; }

        public string CashReceiptInvoiveMappingListSubmit { get; set; }
    }

   
  

   
}