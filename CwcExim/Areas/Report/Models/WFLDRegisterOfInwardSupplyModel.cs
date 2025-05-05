using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDRegisterOfInwardSupplyModel
    {
        public int SlNo { get; set; }
        public decimal OpeningAmount { get; set; }
        public string VoucherNo { get; set; }
        public string DateOfPay { get; set; }
        public string GST { get; set; }
        public string Place { get; set; }
        public string PartyName { get; set; }
      //  public string Supply { get; set; }
        public string VoucherNumber { get; set; }
        public string Date { get; set; }
        public int RateOfTax { get; set; }
        public decimal TaxableVal { get; set; }
       public decimal IGST { get; set; }
        public decimal CentralTax { get; set; }
        public decimal StateTax { get; set; }
        public decimal Cess { get; set; }
        public decimal TotalAmount { get; set; }
     
    }
}