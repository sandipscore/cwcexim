using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_BondCargo
    {
        public int SlNo { get; set; }        
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Importer { get; set; }
        public string ItemDesc { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal TotalValueDuty { get; set; }
        public decimal StorageAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Location { get; set; }
        public decimal Area { get; set; }
        public string CHA { get; set; }
        public string Remarks { get; set; }
        public string CompAddress { get; set; } = string.Empty;
        public string CompEmail { get; set; } = string.Empty;
        public string GodownName { get; set; } = string.Empty;
    }

    public class Hdb_BondCargoExcel
    {
        public int SlNo { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Importer { get; set; }
        public string ItemDesc { get; set; }
        public int Units { get; set; }
        public decimal Weight { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal TotalValueDuty { get; set; }
        public decimal StorageAmount { get; set; }
        public decimal InsuranceAmount { get; set; }
        public string Location { get; set; }
        public decimal Area { get; set; }
        public string CHA { get; set; }
        public string Remarks { get; set; }        
    }
}