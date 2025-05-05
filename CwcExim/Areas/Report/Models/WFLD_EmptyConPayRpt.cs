using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
   
    public class WFLD_EmptyConPayRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        List<WFLD_EmptyConPayRptDtl> lstEmptyContDtl = new List<WFLD_EmptyConPayRptDtl>();

    }

    public class WFLD_EmptyConPayRptDtl
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string ImporterName { get; set; }
        public string PayeeName { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public decimal LOE { get; set; }
        public decimal GRE { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public int TotalDays { get; set; }
        public decimal Total { get; set; }
    }
}