using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_BIMonthlyReport
    {
        public int SerialNo { get; set; }
        public string ImporterName { get; set; }
        public string DestuffingDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string BOLNo { get; set; }
        public int NoOfPKG { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal ValueDuty { get; set; }
        public string LCLFCL { get; set; }
        public string GodownName { get; set; }
        public string DeliveryDate { get; set; }
        public int PreviousAmount { get; set; }
        public int CurrentAmount { get; set; }
        public int TaxableAmount { get; set; }
    }
}