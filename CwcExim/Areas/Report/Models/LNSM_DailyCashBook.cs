using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_DailyCashBook
    {
        
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
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


        public string MISC { get; set; }
        public string MFCHRG { get; set; }
        public string INS { get; set; }
        public string GRL { get; set; }
        public string TPT { get; set; }
        public string EIC { get; set; }
        public string THCCHRG { get; set; }
        public string GRE { get; set; }
        public string RENT { get; set; }
        public string RRCHRG { get; set; }
        public string STO { get; set; }
        public string TAC { get; set; }
        public string TIS { get; set; }
        public string GENSPACE { get; set; }

        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
       // public string Misc { get; set; }
       // public string MiscExcess { get; set; }
        public string TotalCash { get; set; }
        public string TotalCheque { get; set; }
        public string TotalOthers { get; set; }
        public string Tds { get; set; }
        public string CrTds { get; set; }
        public string TotalPDA { get; set; }
        public string Remarks { get; set; }
        public string TFUCharge { get; set; }       

    }

    public class LNSM_DailyCashBookXLS
    {
        //InvoiceId, InvoiceDate, InvoiceNo, InvoiceType, PartyName, PayeeName, ModeOfPay, ChequeNo, GenSpace, sto, Insurance, GroundRentEmpty, GroundRentLoaded, Mf, EntCharge, Fum, OtCharge, MISC, CGSTAmt, SGSTAmt, IGSTAmt, MiscExcess, TotalCash, TotalCheque, TotalPDA, tdsCol, crTDS, Remarks, CreatedOn

        public int SLNO { get; set; }

      
        public string ReceiptDate { get; set; }
        public string CRNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
     //   public string InvoiceType { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
      //  public string ModeOfPay { get; set; }
        public string ChqNo { get; set; }
        public string MISC { get; set; }
        public string MFCHRG { get; set; }
        public string INS { get; set; }
        public string GRL { get; set; }
        public string TPT { get; set; }
        public string EIC { get; set; }
        public string THCCHRG { get; set; }
        public string GRE { get; set; }
        public string RENT { get; set; }
        public string RRCHRG { get; set; }
        public string STO { get; set; }
        public string TAC { get; set; }
        public string TIS { get; set; }
        public string GENSPACE { get; set; }

        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string TotalCash { get; set; }
        public string TotalCheque { get; set; }
        public string TotalOthers { get; set; }
      //  public string TotalPDA { get; set; }

        public string Tds { get; set; }
        public string CrTds { get; set; }


        public string Remarks { get; set; }


        // public string EntryCharge { get; set; }






        //public string TDSPlus { get; set; }
        //public string Exempted { get; set; }
        //public string PdaPLus { get; set; }
        //public string TDSMinus { get; set; }
        //public string PdaMinus { get; set; }
        //public string HtAdjust { get; set; }

        //public string RoundOff { get; set; }
        //public string RowTotal { get; set; }





    }

}