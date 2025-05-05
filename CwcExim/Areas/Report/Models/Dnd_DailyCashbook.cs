namespace CwcExim.Areas.Report.Models
{
    public class Dnd_DailyCashbook
    {
        public string Sr { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyName { get; set; }
        public string ModeOfPay { get; set; }
        public decimal ENT { get; set; }
        public decimal GRE { get; set; }
        public decimal GRL { get; set; }
        public decimal Reefer { get; set; }
        public decimal Monitoring { get; set; }
        public decimal STO { get; set; }
        public decimal INS { get; set; }
        public decimal WHT { get; set; }
        public decimal OTH { get; set; }
        public decimal HT { get; set; }
        public decimal IGST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal RoundUp { get; set; }
        public string CHQNo { get; set; }
        public decimal TotalCASH { get; set; }
        public decimal TotalCHQ { get; set; }
        public decimal Others { get; set; }
        public string Remarks { get; set; }
        public decimal TotalPDA { get; set; }
        public decimal Tds { get; set; }
        public decimal CrTds { get; set; }
            
    }
}