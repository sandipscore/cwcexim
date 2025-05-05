using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class VRN_SDRefundModel
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
    public class VRN_AddMoneyToPDModelRefund
    {

        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Receipt No:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[a-zA-Z0-9/]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 50, ErrorMessage = "maximum 50 character long")]
        public string JournalNo { get; set; }


        public string JournalDate { get; set; }






        public string PartyGSTNo { get; set; }

        public string FolioNo { get; set; }

        public string RoSanctionOrderNo { get; set; }
        public string RoSanctionOrderDate { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        //[Display(Name = "Receipt Date:")]
        ////[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "InitialReading must be greater than or Equal to 0.")]
        //[RegularExpression("^[0-9/]*$", ErrorMessage = "invalid characters found")]
        //[StringLength(maximumLength: 10, ErrorMessage = "maximum 10 character long")]
        //public string TransDate { get; set; }
        public int PartyId { get; set; }

        public int PdaId { get; set; }

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
        public IList<VRN_ReceiptDetailsRefund> Details { get; set; } = new List<VRN_ReceiptDetailsRefund>();

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
        public string ChequeDate { get; set; }
        public decimal UnPaidAmount { get; set; }
        public string PName { get; set; }

        public string PartyAddress { get; set; }

        public string Remarks { get; set; }
    }

    public class VRN_ReceiptDetailsRefund
    {
        public string Type { get; set; }
        public string Bank { get; set; }
        public string InstrumentNo { get; set; }
        public string Date { get; set; }
        public decimal? Amount { get; set; }
    }


    public class VRN_SDRefundList
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string ClosureDate { get; set; }
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
    }

    public class VRN_ViewSDRefund
    {
        public int PDAACId { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string ClosureDate { get; set; }
        public string RecieptNo { get; set; }
        public decimal RefundAmt { get; set; }
        public decimal OpeningAmt { get; set; }
        public decimal ClosingAmt { get; set; }
        public string ChqDate { get; set; }
        public string ChqNo { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string Remarks { get; set; }
    }
}