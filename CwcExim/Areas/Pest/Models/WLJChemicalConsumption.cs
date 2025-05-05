using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Pest.Models
{
    public class WLJChemicalConsumption
    {
        public int ChemicalConsumptionId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Date { get; set; }
     
        public string ChemicalName { get; set; }
        public int ChemicalId { get; set; }
        public string BatchNo { get; set; }
      
        public string ExpiryDate { get; set; }
        public Decimal Quantity { get; set; }
       
        public string ChemicalSearchName { get; set; }
        public int Uid { get; set; }
        public int BranchId { get; set; }
        public string ChemicalXML { get; set; }
        public IList<WLJChemicalConsump> ChemicalDetails { get; set; } = new List<WLJChemicalConsump>();
        public string Remarks { get; set; }
    }



    public class WLJChemicalConsump
    {
        public int ChemicalId { get; set; }
        public string ChemicalName { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }
        public Decimal Quantity { get; set; }
        public string Remarks { get; set; }

    }
}