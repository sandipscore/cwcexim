using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSRChemicalStock
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }

        public string ChemicalName { get; set; }
        public int ChemicalId { get; set; }
    }

    public class DSRStock
    {
        public string ChemicalName { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }
        public decimal Stock { get; set; }
    }
}