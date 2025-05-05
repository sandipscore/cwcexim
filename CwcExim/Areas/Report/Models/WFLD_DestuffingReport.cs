using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    
    public class WFLD_DestuffingReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public string ContNo { get; set; }
        public string ArrivalDate { get; set; }
        public string TSANo { get; set; }
        public string TSADate { get; set; }
        public string SMTPNo { get; set; }
        public string SMTPDate { get; set; }
        public decimal WT { get; set; }
        public string DestuffingDate { get; set; }
        public string LineNo { get; set; }
        public decimal PKG { get; set; }
        public string SLOT { get; set; }
        public decimal Area { get; set; }
        public decimal CBM { get; set; }
        public string Description { get; set; }
        public string Importer { get; set; }
        public string PortName { get; set; }
        public decimal CIFValue { get; set; }
        public string SLA { get; set; }
        public string Remarks { get; set; }
    }
}