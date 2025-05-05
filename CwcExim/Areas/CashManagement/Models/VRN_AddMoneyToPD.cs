using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class VRN_AddMoneyToPD
    {
       
            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Receipt No:")]           
            [RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
            public string ReceiptNo { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Receipt Date:")]            
            [RegularExpression("^[0-9/-]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
            public string TransDate { get; set; }
            public int PartyId { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Party:")]
            
            [RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
            public string PartyName { get; set; }

            
            [Display(Name = "Address:")]
            
            public string Address { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Folio No:")]
            
            [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
            public string FolioNo { get; set; }

            [Required(ErrorMessage = "Fill Out This Field")]
            [Display(Name = "Opening:")]
            
            [RegularExpression("^[0-9.\\-]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
            public string OpBalance { get; set; }
            public IList<ReceiptDetails> Details { get; set; } = new List<ReceiptDetails>();

            [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
            [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
            public string RefundAmount { get; set; }

            public string closingBalance { get; set; }

             public decimal TDSDeduction { get; set; }
             public decimal DepositAmount { get; set; }


    }

    public class VRN_PartyDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }


    public class VRN_ReceiptDetails
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyName { get; set; }
        public decimal TotalValue { get; set; }
    }
}