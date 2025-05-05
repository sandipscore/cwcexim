using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Hdb_DebitInvoice
    {
        public string GstIn { get; set; }

        public int InvoiceId { get; set; } = 0;
        public string InvoiceType { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string DeliveryDate { get; set; } = string.Empty;
        public int PartyId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; } = string.Empty;

        public string PartyGST { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Container { get; set; } = string.Empty;
        public int ContainerInfoId { get; set; }
        public string Size { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string OperationType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; } = string.Empty;

        public string CFSCode { get; set; } = string.Empty;
        public string CFSCD { get; set; } = string.Empty;

        public decimal CGST { get; set; } = 0;

        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal CGSTPER { get; set; } = 0;

        public decimal SGSTPER { get; set; } = 0;
        public decimal IGSTPER { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public IList<Charge> ChargeDetails { get; set; } = new List<Charge>();
        public string ChargeXML { get; set; }
        public decimal TotalChrgAmount { get; set; } = 0;
    }
}