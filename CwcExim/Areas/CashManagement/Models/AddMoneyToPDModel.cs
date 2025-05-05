using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class PartyDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Folio { get; set; }
        public decimal Balance { get; set; }
    }

    public class AddMoneyToPDModel
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

        // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Address:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
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
        public IList<ReceiptDetails> Details { get; set; } = new List<ReceiptDetails>();

        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }

        public string closingBalance { get; set; }

    }

    public class ReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class Chn_ReceiptDetails
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        [RegularExpression("^[a-zA-Z0-9/ ]*$", ErrorMessage = "Invalid Character.")]
        [StringLength(maximumLength: 45, ErrorMessage = "Contain Only 45 Character.")]
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class Chn_AddMoneyToPDModel
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

        // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Address:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
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
        public IList<Chn_ReceiptDetails> Details { get; set; } = new List<Chn_ReceiptDetails>();

        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }

        public string closingBalance { get; set; }

    }
    public class AddMoneyToPDModelHdb
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
        //[RegularExpression("^[0-9/-]*$", ErrorMessage = "invalid character found")]
        //[StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string TransDate { get; set; }
        public int PartyId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Party:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9.\\-,()& ]*$", ErrorMessage = "invalid character found")]
        [RegularExpression(@"^[a-zA-Z0-9.//\\_@#,&() -]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 100, ErrorMessage = "maximum 100 character long")]
        public string PartyName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Address:")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
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
        public IList<ReceiptDetails> Details { get; set; } = new List<ReceiptDetails>();

        [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
        [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        public string RefundAmount { get; set; }

        public string closingBalance { get; set; }
    }
    public class Wfld_AddMoneyToPDModel
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

    // [Required(ErrorMessage = "Fill Out This Field")]
    [Display(Name = "Address:")]
    //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
    //[RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "invalid characted found")]
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
    public IList<ReceiptDetails> Details { get; set; } = new List<ReceiptDetails>();

    [RegularExpression("^[0-9. ]*$", ErrorMessage = "invalid character found")]
    [StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
    public string RefundAmount { get; set; }

    public string closingBalance { get; set; }
    [StringLength(maximumLength: 500, ErrorMessage = "maximum 500 character long")]
    public string Remarks { get; set; }
}
}