using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    //public class WFLDSDOpening
    //{
    //    public int SDId { get; set; }

    //    [Display(Name = "Folio No")]
    //    [Required(ErrorMessage = "Fill Out This Field")]
    //    [RegularExpression(@"^[a-zA-Z0-9//-]+$", ErrorMessage = "Folio No Can Contain Alphabets,Numeric Digits And Special Character - And /")]
    //    [StringLength(30, ErrorMessage = "Folio No Cannot Be More Than 30 Characters In Length")]
    //    public string FolioNo { get; set; }
    //    public int EximTraderId { get; set; }

    //    [Display(Name = "Party")]
    //    [Required(ErrorMessage = "Fill Out This Field")]
    //    public string EximTraderName { get; set; }

    //    [Display(Name = "Date")]
    //    [Required(ErrorMessage = "Fill Out This Field")]
    //    public string Date { get; set; }

    //    [Display(Name = "Amount")]
    //    [Required(ErrorMessage = "Fill Out This Field")]
    //    [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Amount Can Contain Only Numeric Digits And Special Character '.'")]
    //    [Range(minimum: 0, maximum: 999999999999.99, ErrorMessage = "Amount Should Be Greater Than or equal to 0 And Less Than Or Equal To 999999999999.99")]
    //    public decimal Amount { get; set; }
    //    public int Uid { get; set; }

    //    public List<WFLDSDOpening> LstEximTrader { get; set; } = new List<WFLDSDOpening>();
    //}

    public class WFLDSDOpening
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

        public List<WFLDSDOpening> LstEximTrader { get; set; } = new List<WFLDSDOpening>();
        [Display(Name = "Party Code")]
        public string PartyCode { get; set; }
        [Display(Name = "Receipt No.")]
        public string ReceiptNo { get; set; }

        public string Remarks { get; set; }
        public IList<WFLDReceiptDetails> Details { get; set; } = new List<WFLDReceiptDetails>();
    }

    public class WFLDReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }


    public class WFLDListOfEximTrader
    {
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }
        public string PartyCode { get; set; }
    }
    public class WFLDSearchEximTraderData
    {
        public List<WFLDListOfEximTrader> lstExim { get; set; } = new List<WFLDListOfEximTrader>();
        public bool State { get; set; }
    }
}