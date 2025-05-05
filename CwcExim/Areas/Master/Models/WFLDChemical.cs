using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class WFLDChemical
    {
        public int ChemicalId { get; set; }

        public string ChemicalName { get; set; }

        public int Uid { get; set; }
        public int BranchId { get; set; }
    }
}