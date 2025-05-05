using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wlj_RegisterOfOutward
    { 
        public int SlNo { get; set; }
        public string InvoiceType { get; set; }
        public string GST { get; set; }
        public string Name { get; set; }
        public string Place { get; set; } 
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string SACCode { get; set; }
        public string description { get; set; }
        public string Rate { get; set; }
        public decimal ServiceValue { get; set; }
        public decimal ITaxAmount { get; set; }
        public decimal CTaxAmount { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal Cess { get; set; }
        public decimal RoundUp { get; set; }
        public decimal Total { get; set; }
        
        /*****************Invoice Data***************/
        /*****************CashReceipt Data***************/
    
    }
}