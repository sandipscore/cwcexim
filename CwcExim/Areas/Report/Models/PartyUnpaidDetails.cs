using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class PartyUnpaidDetails
    {
        public string PartyName { get; set; }
        public decimal UnpaidAmt{ get; set; }
        public string AsOnDate { get; set; }
    }
    public class PartyWiseUnpaid
    {
        public int PartyId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public  string AsOnDate{ get; set; }
    }
    public class PartyWiseUnpaidDtl
    {
        public string PartyName { get; set; }
        public string AsOnDate { get; set; }
        public IList<InvoiceDtl> lstDtl { get; set; } = new List<InvoiceDtl>();
    }
    public class InvoiceDtl
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceAmt { get; set; }
    }
}