using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class EditReceiptPayment
    {
        public int CashReceiptId { get; set; }

        public string ReceiptNo { get; set; }
        public int PartyId { get; set; }

        public int PayByPdaId { get; set; }

        public decimal PDAAdjusted { get; set; }

        public decimal TotalPaymentReceipt { get; set; }

        public string receiptTableJson { get; set; }
        public IList<CashReceiptEditDtls> CashReceiptDetail { get; set; } = new List<CashReceiptEditDtls>();

        public IList<PdaAdjustEdit> lstPdaAdjustEdit = new List<PdaAdjustEdit>();


        public string InvoiceHtml { get; set; }


        public int InvoiceId { get; set; }


       // public int PartyId { get; set; }


    }
    public class CashReceiptEditDtls
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

    public class PdaAdjustEdit
    {
        public int PayByPdaId { get; set; }
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }
        public string FolioNo { get; set; }
        //  public decimal Adjusted { get; set; }
        public decimal Opening { get; set; }
        public decimal Closing { get; set; }
      //  
    }



    public class PDAListAndAddress
    {
        public IList<PdaAdjustEdit> _PdaAdjustEdit { get; set; } = new List<PdaAdjustEdit>();
        public IList<PayByEdit> PayByDetail { get; set; } = new List<PayByEdit>();
    }
    public class PayByEdit
    {
        public int PayByEximTraderId { get; set; }
        public string PayByName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }

}