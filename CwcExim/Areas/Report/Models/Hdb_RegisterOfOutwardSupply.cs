using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_RegisterOfOutwardSupply
    {
        public int SlNo { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string Period { get; set; }
        public string Nature { get; set; } = "N/A";
        public decimal Rate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal ServiceValue { get; set; }
        public string ITaxPercent { get; set; }
        public decimal ITaxAmount { get; set; }
        public string CTaxPercent { get; set; }
        public decimal CTaxAmount { get; set; }
        public string STaxPercent { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal RoundUp { get; set; }
        public decimal Total { get; set; }
        /*****************Invoice Data***************/
        /*****************CashReceipt Data***************/
        public string WH { get; set; } = "WH";
        public string CRNoDate { get; set; }
        public string ChequeNoDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Received { get; set; }
        public decimal Adjustment { get; set; }
        public decimal Balance { get; set; }
        public string Remarks { get; set; }
       
        /*****************CashReceipt Data***************/
    }
}