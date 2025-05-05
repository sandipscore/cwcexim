using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_DailyCashBook
    {
        public string CRNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Depositor { get; set; }
        public string ChqNo { get; set; }
        public decimal GRE { get; set; }
        public decimal GRL { get; set; }
        public decimal Reefer { get; set; }
        public decimal INS { get; set; }
        public decimal STO { get; set; }
        public decimal EscCharge { get; set; }
        public decimal Print { get; set; }
        public decimal Royality { get; set; }
        public decimal Franchiese { get; set; }
        public decimal HT { get; set; }
        public decimal LWB { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string TotalCash { get; set; }
        public string TotalCheque { get; set; }
        public string Tds { get; set; }
        public string CrTds { get; set; }
        public decimal Area { get; set; }
        public decimal Roundoff { get; set; } = 0;
        public decimal TotalNEFT { get; set; }
        public decimal TotalOth { get; set; }

        public string AddmonyToSd { get; set; }
        public string RefundFromSd { get; set; }


    }
}