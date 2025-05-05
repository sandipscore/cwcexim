using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{


    public class DSRPcsPaymentReportHeader
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }        
        public List<DSRPcsPaymentReport> LstPCSReport { get; set; } = new List<DSRPcsPaymentReport>();

    }



    public class DSRPcsPaymentReport
    {
        public string CertificateNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string PartyDescription { get; set; }
        public decimal Amount { get; set; }       
       
    }

}