using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class CHNBondWOUnloading: BondWOUnloading
    {
        public string BondNo { get; set; }

        public string BondDate { get; set; }

        public decimal ReservedArea { get; set; } = 0;

    }

    public class ListOfCHNBOENo
    {
        //public string WorkOrderNo { get; set; }
        //public int BondWOId { get; set; }
        public string BondNo { get; set; }


        public int DepositAppId { get; set; }
    }
}