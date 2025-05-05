using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;

namespace CwcExim.Areas.CashManagement.Models
{
    public class LNSM_ChequeReturn
    {
       
        [Display(Name = "Party Name:")]
        public string PartyName { get; set; }
       
        [Display(Name = "Sd No:")]
        public string SdNo { get; set; }

        public string ReceiptNo { get; set; }
        [Display(Name = "Cheque Return Date:")]
        public string ChequeReturnDate { get; set; }
        public int PartyId { get; set; }
        public int DishonuredId { get; set; }
        public string AutoDisHonourRcptNo { get; set; }

        public decimal Balance { get; set; }
       
        [Display(Name = "Instrument No:")]
        [RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string ChequeNo { get; set; }
        [Display(Name = "Cheque Date:")]
        public string ChequeDate { get; set; }
        [Display(Name = "Drawee Bank:")]
        [RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string DraweeBank { get; set; }

        public decimal Amount { get; set; }
        [Display(Name = "Adjusted Balance:")]
        public decimal AdjustedBalance { get; set; }
        [StringLength(maximumLength: 500, ErrorMessage = "maximum 500 character long")]
        public string Narration { get; set; }
        public List<LNSM_ChequeDetails> lstCheque { get; set; } = new List<LNSM_ChequeDetails>();
        public bool State;
    }
    public class LNSM_ChequeDetail
    {
        public int Id { get; set; }
        public string Cheque { get; set; }


    }
    public class LNSM_Cheques
    {
        public string ChequeDate { get; set; }
        public string DraweeBank { get; set; }
        public string Amount { get; set; }
    }
    public class LNSM_ChequeDetails
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public int ChequeId { get; set; }
        public string ChequeName { get; set; }
        public decimal ChequeBalance { get; set; }
        public string ChequeSdNo { get; set; }
    }

}