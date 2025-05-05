using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDEximTraderFinanceControl
    {
        public int FinanceControlId { get; set; }
        public int EximTraderId { get; set; }

        [Display(Name = "Previous Balance")]
        [Range(minimum: 0, maximum: 999999999999.99, ErrorMessage = "Previous Balance Should Not Be More Than 999999999999.99")]
        public decimal PreviousBalance { get; set; }

        [Display(Name = "Current Balance")]
        [Range(minimum: 0, maximum: 999999999999.99, ErrorMessage = "Current Balance Should Not Be More Than 999999999999.99")]
        public decimal CurrentBalance { get; set; }

        [Display(Name = "Credit Limit")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(minimum: 0, maximum: 999999999999.99, ErrorMessage = "Credit Limit Should Be More Than 0 And Less Than Or Equal To 999999999999.99")]
        public decimal CreditLimit { get; set; }

        [Display(Name = "Credit Period")]
        [Range(0, int.MaxValue, ErrorMessage = "Credit Period Should Be Grater Than 0 And Less Than 2147483648")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CreditPeriod { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EximTraderName { get; set; }

        [Display(Name = "PartyCode")]
        public string PartyCode { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "TAN")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{5}[A-Z]{1}$", ErrorMessage = "Invalid TAN")]
        public string Tan { get; set; }

        [Display(Name = "GST No")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST No")]
        public string GSTNo { get; set; }
        public int Uid { get; set; }
        public List<SearchEximTraderDataFinanceControl> LstEximTraders { get; set; } = new List<SearchEximTraderDataFinanceControl>();

    }

    public class WFLDListOfEximTraderFinanceControl
    {
        public int EximTraderId { get; set; }
        public string EximTraderName { get; set; }
        public string PartyCode { get; set; }
        public string GSTNo { get; set; }
        public string Tan { get; set; }
        public string Address { get; set; }
    }
    public class WFLDSearchEximTraderDataFinanceControl
    {

        public List<WFLDListOfEximTraderFinanceControl> lstExim { get; set; } = new List<WFLDListOfEximTraderFinanceControl>();
        public bool State { get; set; }
    }
}