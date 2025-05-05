using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    
    public class WFLD_FCLDailyReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string InvoiceNo { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string ITEMNo { get; set; }
        public decimal Package { get; set; }
        public string CHA { get; set; }
        public string SLA { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string Description { get; set; }
        public string GatePassNo { get; set; }
 

    }
}