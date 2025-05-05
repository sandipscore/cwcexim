using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSRRefundMoneyFromPD
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }
        public string PartyAddress { get; set; }
    }


    public class DSRAddMoneyToPDModelRefund
    {
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }

        public string PartyAddress { get; set; }

        public string Remarks { get; set; }
        
        public string ReceiptNo { get; set; }
        
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Party:")]
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Opening:")]        
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
       
        public string BankAndBranch { get; set; }

        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characters found")]
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }

    }

    public class DSRSDRefundList
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string ClosureDate { get; set; }
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
    }
    

}