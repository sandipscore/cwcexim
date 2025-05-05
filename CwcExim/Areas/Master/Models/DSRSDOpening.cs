using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRSDOpening
    {
        public int SDId { get; set; }

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

        public List<DSRSDOpening> LstEximTrader { get; set; } = new List<DSRSDOpening>();
        [Display(Name = "Party Code")]
        public string PartyCode { get; set; }
        [Display(Name = "Receipt No.")]
        public string ReceiptNo { get; set; }
        public int BranchId { get; set; }


        public IList<DSRReceiptDetails> Details { get; set; } = new List<DSRReceiptDetails>();
    }

    public class DSRReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }


    public class DSRListOfEximTrader
    {
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }
        public string PartyCode { get; set; }
    }
    public class DSRSearchEximTraderData
    {
        public List<DSRListOfEximTrader> lstExim { get; set; } = new List<DSRListOfEximTrader>();
        public bool State { get; set; }
    }
}