using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Pest.Models
{
    public class ChemicalStockIn
    {
        public int ChemicalStockId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Date { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ChemicalName { get; set; }

        public int Quantity { get; set; }

        public int Uid { get; set; }
        public int BranchId { get; set; }
   


    }
  
}