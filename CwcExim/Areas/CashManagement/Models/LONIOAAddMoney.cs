using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    
    public class LONIOAAddMoney
    {
        public int OnAcId { get; set; }
        public string FolioNo { get; set; }
        public int EximTraderId { get; set; }

        [Display(Name = "Party")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EximTraderName { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Date { get; set; }

        [Display(Name = "Amount")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        //   [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Amount Can Contain Only Numeric Digits And Special Character '.'")]
        //  [Range(minimum: 0, maximum: 999999999999.99, ErrorMessage = "Amount Should Be Greater Than or equal to 0 And Less Than Or Equal To 999999999999.99")]
        public decimal Amount { get; set; }
        public int Uid { get; set; }

        public List<WFLDOAAddMoney> LstEximTrader { get; set; } = new List<WFLDOAAddMoney>();
        [Display(Name = "Party Code")]
        public string PartyCode { get; set; }
        [Display(Name = "Receipt No.")]
        public string ReceiptNo { get; set; } 
        public string TransDate { get; set; }
        public string Remarks { get; set; }

        public IList<LONIOAReceiptDetails> Details { get; set; } = new List<LONIOAReceiptDetails>();
    }

    public class LONIOAReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        
    }
    public class LONIOAListOfEximTrader
    {
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }
        public string PartyCode { get; set; }
    }
    public class LONIOASearchEximTraderData
    {
        public List<LONIOAListOfEximTrader> lstExim { get; set; } = new List<LONIOAListOfEximTrader>();
        public bool State { get; set; }
    }

    public class LONIListOfOAAddMoney
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }

    }
    
}