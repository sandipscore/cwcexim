using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{   
    public class LNSM_AddMoneyToPDModel
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
        
        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }
        public string closingBalance { get; set; }
        public IList<LNSM_ReceiptDetails> Details { get; set; } = new List<LNSM_ReceiptDetails>();

    }
    public class LNSM_ReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class LNSM_PartyDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }
    public class LNSM_PaymentPartyName
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyCode { get; set; }
    }
    public class LNSM_AddMoneyToPDListModel
    {
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Amount { get; set; }
    }

}