using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Chn_LCLDestuffingRegister
    {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string ContainerNo { get; set; }
        public string LineNo { get; set; }
        public string IGMNo { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public string GodownName { get; set; }
        public string CargoDescription { get; set; }
        public decimal Grid { get; set; }
        public string Importer { get; set; }
        public string DeliveryDate { get; set; }
        public string IssueSlipNo { get; set; }
        public string GatepassNo { get; set; }
        public int NoOfPkgdel { get; set; }
        public string CB { get; set; }
        public string Remarks { get; set; }

    }
}