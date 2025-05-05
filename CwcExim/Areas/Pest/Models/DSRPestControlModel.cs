using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Pest.Models
{
    public class DSRPestControlModel
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
       
        public string Container { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PayeeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal Amount { get; set; } = 0;
        public decimal CGST { get; set; } = 0;
        [Display(Name = "SGST")]
        public decimal SGST { get; set; } = 0;
        public decimal IGST { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Round_up { get; set; } = 0;
        public string Naration { get; set; } = string.Empty;
        public IList<dsrChemical> ChemicalDetails { get; set; } = new List<dsrChemical>();
        public string ChemicalXML { get; set; }

        public decimal QuantityChemical { get; set; } = 0;

    }

    public class dsrChemical
    {
        public int ChemicalId { get; set; }
       
        public string ChemicalName { get; set; }
       
        public decimal Quantity { get; set; }
        public int FumigationId { get; set; }
    }
}