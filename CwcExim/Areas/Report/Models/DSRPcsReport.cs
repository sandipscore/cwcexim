using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{


    public class DSRPcsReportHeader
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string MbrType { get; set; }
        public List<DSRPcsReport> LstPCSReport { get; set; } = new List<DSRPcsReport>();

    }



    public class DSRPcsReport
    {
        public string CertificateNo { get; set; }
        public string IssueDate { get; set; }
        public string Chemical { get; set; }
        public string Dosages { get; set; }
        public string PKG { get; set; }
        public string NoCane { get; set; }
        public string Cargo { get; set; }
        public string Exporter { get; set; }
        public string Container { get; set; }
        public string Volume { get; set; }
        public decimal Amount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public string ReceiptNo { get; set; }
        public decimal Total { get; set; }
        public string Country { get; set; }
        public string InvoiceNo { get; set; }
        public string FumigationDate { get; set; }
        public int Size { get; set; }
        public string Place { get; set; }
    }

}