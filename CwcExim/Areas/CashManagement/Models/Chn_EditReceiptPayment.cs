using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Chn_EditReceiptPayment
    {
        public int CashReceiptId { get; set; }

        public string ReceiptNo { get; set; }
        public int PartyId { get; set; }

        public int PayByPdaId { get; set; }

        public decimal PDAAdjusted { get; set; }

        public decimal TotalPaymentReceipt { get; set; }

        public string receiptTableJson { get; set; }
        public IList<Chn_CashReceiptEditDtls> CashReceiptDetail { get; set; } = new List<Chn_CashReceiptEditDtls>();

        public IList<PdaAdjustEdit> lstPdaAdjustEdit = new List<PdaAdjustEdit>();


        public string InvoiceHtml { get; set; }


        public int InvoiceId { get; set; }
    }

    public class Chn_CashReceiptEditDtls
    {
        //:: Cash Mode if pda no selected ::::::
        public int CashReceiptDtlId { get; set; }
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


        // public string CashReceiptHtml { get; set; }
        //::::::::::::::::::::::::::::::::::::
    }
}