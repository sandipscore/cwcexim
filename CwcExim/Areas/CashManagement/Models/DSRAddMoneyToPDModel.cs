using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_PartyDetails
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }

    public class DSRAddMoneyToPDModel
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Receipt No:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
        public string ReceiptNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Receipt Date:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9/-]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string TransDate { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Party:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9.\\-,()& ]*$", ErrorMessage = "invalid character found")]
        [RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string PartyName { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Address:")]        
        public string Address { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Folio No:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string FolioNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Opening:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9.\\-]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string OpBalance { get; set; }
        public IList<CashReceiptDetails> Details { get; set; } = new List<CashReceiptDetails>();

        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }

        public string closingBalance { get; set; }
        
        public string TDSDeduction { get; set; }
        public string DepositAmount { get; set; }
    }

    public class CashReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }


}