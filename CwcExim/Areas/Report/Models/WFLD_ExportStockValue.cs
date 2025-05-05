using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ExportStockValue
    {
        public string SVDate { get; set; }
        public decimal FOBValue { get; set; }       
    }
    public class WFLD_ExportStockReportVM
    {
        public string SVDate { get; set; }
        public decimal FOBValue { get; set; }
        
    }
}