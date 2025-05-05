using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_NewPaymentValucherModel
    {
        public int PVHeadId { get; set; }

        [Display(Name = "Voucher No:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "Invalid charactes found")]
        [StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
        public string VoucherNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Payment Date:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[0-9/]*$", ErrorMessage = "invalid charactes found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string PaymentDate { get; set; }

        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Party:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid charactes found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string Party { get; set; }

        [Display(Name = "Address:")]
        public string Address { get; set; }


        [Display(Name = "State:")]
        public string State { get; set; }

        [Display(Name = "State Code:")]
        public string StateCode { get; set; }

        [Display(Name = "City:")]
        public string City { get; set; }

        [Display(Name = "Pin:")]
        [RegularExpression("^[0-9/]*$", ErrorMessage = "invalid charactes found")]
        [StringLength(maximumLength: 6, ErrorMessage = "maximum 6 character long")]
        public string Pin { get; set; }

        [Display(Name = "GST No:")]
        public string GSTNo { get; set; }


        [Display(Name = "Pan No:")]
        public string PanNo { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Required(ErrorMessage = "No Expenses Detected !!")]
        public string ExpensesJson { get; set; }

        [Display(Name = "Unregister:")]
        public bool IsUnregister { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal RoundOff { get; set; }
        public decimal PayableAmount { get; set; }
        public IList<DSRExpensesDetails> expcharges { get; set; } = new List<DSRExpensesDetails>();


        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Purpose:")]
        public string Purpose { get; set; }

        [Display(Name = "Amount In Hand:")]
        public decimal Amt { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }

        public int CompanyStateCode { get; set; }
        public string CompanyGST { get; set; }

        public string CompanyPan { get; set; }
        public decimal TotalIGST { get; set; }
        public decimal TotalSGST { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalIGSTAmt { get; set; }
        public decimal TotalSGSTAmt { get; set; }
        public decimal TotalCGSTAmt { get; set; }
        public decimal TotalAmounts { get; set; }




    }

    public class DSRExpensesDetails
    {
        
        public int ExpId { get; set; }
       
        public int HsnId { get; set; }
        public decimal Taxable { get; set; }
        public decimal Amount { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public string ExpenseHead { get; set; }
        public string Expensecode { get; set; } = string.Empty;
    }
}