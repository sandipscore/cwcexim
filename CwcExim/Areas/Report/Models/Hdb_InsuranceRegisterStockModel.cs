using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_InsuranceRegisterStockModel
    {
        public    string _date { get; set; }
        public decimal openingfob { get; set; }
        public decimal receiptfob { get; set; }
        public decimal issuefob { get; set; }
        public decimal closefob { get; set; }
        public decimal closeweight { get; set; }

      
    }
    public class Hdb_InsuranceRegisterStockModelVM
    {
        public string _date { get; set; }
        public decimal openingfob { get; set; }
        public decimal receiptfob { get; set; }
        public decimal issuefob { get; set; }
        public decimal closefob { get; set; }
        public decimal closeweight { get; set; }

    }
}