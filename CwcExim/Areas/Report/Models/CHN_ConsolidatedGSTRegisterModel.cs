using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_ConsolidatedGSTRegisterModel
    {
        public int SlNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Name { get; set; }
        public string GST { get; set; }
        public string Nature { get; set; }
        public decimal ServiceValue { get; set; }
        public decimal Excempted { get; set; }
        public decimal STaxAmount { get; set; }
        public decimal CTaxAmount { get; set; }
     
        public decimal ITaxAmount { get; set; }
      
        
        public decimal Total { get; set; }
       
    }
}