using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CwcExim.Areas.Report.Models
{
    public class Chn_LongStandingImpCon
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int days { get; set; }
    }
    public class Chn_LongStandingImpConDtl
    {
        public string BOLNo { get; set; }
        public string BOLDate { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string ImporterName { get; set; }
        public string ImporterAddress { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string InDate { get; set; }
        public string SlaCd { get; set; }
        public int Days { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrWt { get; set; }
        public string Commodity { get; set; }
        public string CargoType { get; set; }
        public string Notice1 { get; set; }
        public string Date1 { get; set; }
        public string Notice2 { get; set; }
        public string Date2 { get; set; }
        public string Nocr { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
    }
}