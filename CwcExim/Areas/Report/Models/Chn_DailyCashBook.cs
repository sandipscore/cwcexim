using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Chn_DailyCashBook
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }


        //#for list
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }
        public string CwcChargeTAX { get; set; }

        public string CwcChargeNonTAX { get; set; }
        public string CustomRevenueTAX { get; set; }


        public string CustomRevenueNonTAX { get; set; }

        public string InsuranceTAX { get; set; }

        public string InsuranceNonTAX { get; set; }
        public string PortTAX { get; set; }
        public string PortNonTAX { get; set; }

        public string LWB { get; set; }//


        public string CWCCGST { get; set; }
        public string CWCSGST { get; set; }
        public string CWCISGT { get; set; }
        public string HtTax { get; set; }
        public string HtNonTax { get; set; }
        public string HtCGST { get; set; }
        public string HtSGST { get; set; }
        public string HtISGT { get; set; }

        public string RoPdRefund { get; set; }
        public string MISC { get; set; }
        public string TDSPlus { get; set; }
        public string Exempted { get; set; }
        public string PdaPLus { get; set; }
        public string TDSMinus { get; set; }
        public string PdaMinus { get; set; }
        public string HtAdjust { get; set; }

        public string RoundOff { get; set; }
        public string RowTotal { get; set; }




    }

    public class Chn_DailyCashBookDtl
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int CRId { get; set; }
        //#for list
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }
        public string ChqNo { get; set; }

        public string GenSpace { get; set; }
        public string StorageCharge { get; set; }


        public string Insurance { get; set; }

        // public string GroundRentEmpty { get; set; }

        public string GroundRent { get; set; }
        public string GroundRentEmpty { get; set; }
        public string GroundRentLoaded { get; set; }
        public string MfCharge { get; set; }
        public string EntryCharge { get; set; }

        public string Fumigation { get; set; }
        public string RFIDCharge { get; set; }
        public string WeighmentCharge { get; set; }

        public string FacilitationCharge { get; set; }//


        public string OtherCharge { get; set; }
        public string DocumentCharge { get; set; }
        public string AggregationCharge { get; set; }
        public string HTCharge { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string Misc { get; set; }
        public string MiscExcess { get; set; }
        public string TotalCash { get; set; }
        public string TotalCheque { get; set; }

        public string Tds { get; set; }
        public string CrTds { get; set; }
        //public string TDSPlus { get; set; }
        //public string Exempted { get; set; }
        //public string PdaPLus { get; set; }
        //public string TDSMinus { get; set; }
        //public string PdaMinus { get; set; }
        //public string HtAdjust { get; set; }

        //public string RoundOff { get; set; }
        //public string RowTotal { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceType { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string ModeOfPay { get; set; }
        public string TotalOthers { get; set; }
        public string TotalPDA { get; set; }
        public string Remarks { get; set; }




    }
}