using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class LNSM_RegisterOfOutwardUpaidModel
    {
        public int SlNo { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal Total { get; set; }

        public string Remarks { get; set; }
    }
}