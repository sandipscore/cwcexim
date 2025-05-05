using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Wij_InsuranceRegister
    {
      
            public String TransactionDate { get; set; }
           
            public decimal ReceivedUnit { get; set; }
            public decimal ReceivedWeight { get; set; }
            public decimal DeliveryUnit { get; set; }
            public decimal DeliveryWeight { get; set; }
            public decimal ClosingUnit { get; set; }
            public decimal ClosingWeight { get; set; }

            public string Item { get; set; }
            public decimal Rate { get; set; }
            public decimal CommodityValue { get; set; }
            public decimal TotalValue { get; set; }

        public IList<Wij_InsuranceRegister> lstInsReg { get; set; } = new List<Wij_InsuranceRegister>();

    }
}