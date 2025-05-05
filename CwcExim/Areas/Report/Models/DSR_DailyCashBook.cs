using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_DailyCashBook
    {
        //InvoiceId, InvoiceDate, InvoiceNo, InvoiceType, PartyName, PayeeName, ModeOfPay, ChequeNo, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, Fum, OtCharge, MISC, CGSTAmt, SGSTAmt, IGSTAmt, MiscExcess, TotalCash, TotalCheque, TotalPDA, tdsCol, crTDS, Remarks, CreatedOn
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }


        //#for list
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }

        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }

        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string ModeOfPay { get; set; }
        public string ChqNo { get; set; }
        public string ChqDate { get; set; }
        public string GenSpace { get; set; }
        public string StorageCharge { get; set; }

        
        public string Insurance { get; set; }

        public string GroundRentEmpty { get; set; }

        public string GroundRentLoaded { get; set; }
        public string MfCharge { get; set; }
        public string EntryCharge { get; set; }

        public string Fumigation { get; set; }//

       
        public string OtherCharge { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string Misc { get; set; }
        public string MiscExcess { get; set; }
        public string TotalCash { get; set; }
        public string TotalCheque { get; set; }
        public string TotalOthers { get; set; }
        

        public string Tds { get; set; }
        public string CrTds { get; set; }

        public string TotalPDA { get; set; }
        public string TotalOnAccount { get; set; }
        public string Remarks { get; set; }
        public string CRNotes { get; set; }
        public string ChequeBank { get; set; }
        public string TotalOA { get; set; }

    }
    public class DSR_DailyCashBookCash
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyName { get; set; }
        public string CodeNo { get; set; }
        public string AdvDeposit { get; set; }
        public string ChequeNo { get; set; }
        public string CashReceiptId { get; set; }
        public string IMPSTO { get; set; }
        public string IMPINS { get; set; }
        public string IMPGRL { get; set; }
        public string IMPHT { get; set; }
        public string EXPSTO { get; set; }
        public string EXPINS { get; set; }
        public string EXPGRE { get; set; }
        public string EXPHTT { get; set; }
        public string EXPHTN { get; set; }
        public string BNDSTO { get; set; }
        public string BNDINS { get; set; }
        public string BNDHT { get; set; }
        public string OTHS { get; set; }
        public string MISCT { get; set; }
        public string MISCN { get; set; }
        public string PCS { get; set; }
        public string DOC { get; set; }
        public string ADM { get; set; }
        public string WET { get; set; }
        public string tdsCol { get; set; }
        public string TaxableAmt { get; set; }
        public string CGSTAmt { get; set; }
        public string SGSTAmt { get; set; }
        public string IGSTAmt { get; set; }
        public string RoundUp { get; set; }
        public string GrossAmt { get; set; }
        public string RecCash { get; set; }
        public string RecCheque { get; set; }
        public string RecOnline { get; set; }
        public string TDS { get; set; }
        public string ADJ { get; set; }
        public string RecCrNote { get; set; }
        public string NetAmt { get; set; }
        public string OpeningBalance { get; set; }
        public string BankDeposit { get; set; }
        public string OpeningRSQty { get; set; }
        public string CPurchaseRSQty { get; set; }
        public string CConsumRSQty { get; set; }
        public string OConsumRSQty { get; set; }

    }
   

}