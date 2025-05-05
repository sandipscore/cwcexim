using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class DSRFumigationMBR
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
       
        public string MbrType { get; set; }
        public List<MBRReport> LstMBRReport { get; set; } = new List<MBRReport>();
        public string OperationType { get; set; }
        public string Dosages { get; set; }

    }

    public class MBRReport
    {
        public string CertificateNo { get; set; }
        public string Commodity { get; set; }
        public string Quantity { get; set; }
        public string CountryImport { get; set; }
        public string Dosagesformbr { get; set; }
        public int QuantityUsedMBR { get; set; }
        public int Remarks { get; set; }
     

    }
}