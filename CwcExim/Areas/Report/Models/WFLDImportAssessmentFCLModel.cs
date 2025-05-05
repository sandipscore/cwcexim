using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDImportAssessmentFCLModel
    {
        public int SR { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PayerName { get; set; }
        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }

        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string SLA_DESC { get; set; }
       
        public string Amount { get; set; }

    }
}