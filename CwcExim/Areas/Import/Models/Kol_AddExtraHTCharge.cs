using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kol_AddExtraHTCharge
    {
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }

        public decimal Taxable { get; set; }
        public string ChargeName { get; set; }
        public string SACCode { get; set; }       
        public string Clause { get; set; }
        public string ChargeType { get; set; }
        //public IList<Kol_ItemAddExtraHTCharge> Kol_ItemAddExtraHTCharge { get; set; }
    }


    public class Kol_ItemAddExtraHTCharge
    {

       
    }
}