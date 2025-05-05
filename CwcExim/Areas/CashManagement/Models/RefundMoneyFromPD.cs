using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class RefundMoneyFromPD
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }

    public class AddMoneyToPDModelRefund
    {
        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Receipt No:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
        public string ReceiptNo { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Receipt Date:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[0-9/]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        //public string TransDate { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Party:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9._& ]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string PartyName { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Address:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        ////[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        //public string Address { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Folio No:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        //public string FolioNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Opening:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid characters found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string OpBalance { get; set; }
        public IList<ReceiptDetails> Details { get; set; } = new List<ReceiptDetails>();

        [RegularExpression("^[0-9. ]*$", ErrorMessage = "Invalid characters found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }

        public string closingBalance { get; set; }

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        public string Bank { get; set; }

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        public string Branch { get; set; }

        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        public string BankAndBranch { get; set; }

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        public string ChequeNo { get; set; }
        public string ChequeDate{ get; set; }
    }

    public class ReceiptDetailsRefund
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
}