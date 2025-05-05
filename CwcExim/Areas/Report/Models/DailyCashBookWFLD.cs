using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DailyCashBookWFLD
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
        public string TotalPOS { get; set; }
        public string TotalOnline { get; set; }
        public string Tds { get; set; }
        public string CrTds { get; set; }

        public string TotalPDA { get; set; }
        public string TotalOnAccount { get; set; }
        public string Remarks { get; set; }
        public string CRNotes { get; set; }
        public string ChequeBank { get; set; }
        public string POS { get; set; }
    }

}