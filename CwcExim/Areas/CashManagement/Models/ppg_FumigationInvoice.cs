using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.CashManagement.Models
{
    public class ppg_FumigationInvoice
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
        public string GSTNo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Container { get; set; } = string.Empty;

        public string CFSCode { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FumigationChargeType { get; set; } 
        public int PayeeId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Amount { get; set; } = 0;


        public decimal CGSTPer { get; set; } = 0;
        
        public decimal SGSTPer { get; set; } = 0;
        public decimal IGSTPer { get; set; } = 0;

        public decimal CGST { get; set; } = 0;
        [Display(Name = "SGST")]
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public IList<Chemical> ChemicalDetails { get; set; } = new List<Chemical>();
        public string ChemicalXML { get; set; }

        public string SEZ1 { get; set; }
    }

    public class Chemical
    {
        public int ChemicalId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ChemicalName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public Decimal Quantity { get; set; }
        public int FumigationId { get; set; }
    }
}
