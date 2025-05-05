using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_TimeBarredReport
    {
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
        public int Units { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal Area { get; set; }
        public string ItemDesc { get; set; }
    }
    public class VRN_LiveBondReport
    {
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Importer { get; set; }
        public string CHA { get; set; }
        public int Units { get; set; }
        public decimal Value { get; set; }
        public decimal Duty { get; set; }
        public decimal Area { get; set; }
        public string ItemDesc { get; set; }
    }
}