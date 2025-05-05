using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class ReceiptVoucherModel
    {
        public int ReceiptId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Voucher No:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "invalid characted found")]
        [StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
        public string VoucherNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Payment Date:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9/]*$", ErrorMessage = "invalid characted found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string PaymentDate { get; set; }

        public int PartyId { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Party:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
        //[StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string Party { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Purpose:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string Purpose { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Amount:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9.]*$", ErrorMessage = "invalid characted found")]        
        public decimal Amount { get; set; }
                
        [Display(Name = "Narration:")]      
        public string Narration { get; set; }
    }
}