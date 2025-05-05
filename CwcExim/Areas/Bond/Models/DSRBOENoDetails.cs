using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRBOENoDetails : BOENoDetails 
    {
        public decimal AreaReserved { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }

        public string DepositNo { get; set; }
        public string Remarks { get; set; }
    }
}