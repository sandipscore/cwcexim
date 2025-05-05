using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_CashBookWithSDExcel
    {

        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public string PartyName { get; set; }
        public string PayeeName { get; set; }
        public string ModeOfPay { get; set; }
        public string ChequeNo { get; set; }
        public decimal GenSpace { get; set; }
        public decimal sto { get; set; }
        public decimal Insurance { get; set; }
        public decimal GroundRentEmpty { get; set; }
        public decimal GroundRentLoaded { get; set; }
        public decimal Mf { get; set; }
        public decimal EntCharge { get; set; }  
        public decimal Fum { get; set; }
        public decimal OtCharge { get; set; }
        public decimal MISC { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal MiscExcess { get; set; }
        public decimal TotalCash { get; set; }
        public decimal TotalCheque { get; set; }
        public decimal TotalOther { get; set; }
        public decimal TotalPDA { get; set; }
        public decimal tdsCol { get; set; }
        public decimal crTDS { get; set; }
        public string Remarks { get; set; }
 
    }
}