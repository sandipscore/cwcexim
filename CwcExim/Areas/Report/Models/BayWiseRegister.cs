using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class BayWiseRegister
    {
        public int DestuffingEntryId { get; set; }
        public string DestuffingEntryDate { get; set; }
        public string ContainerNo { get; set; }
        public string SLine { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string CFSCode { get; set; }
        public int NoOfPackages { get; set; }
        public string GodownName { get; set; }
        public string GodownWiseLctnNames { get; set; }
        public string CargoDescription { get; set; }
        public string BOLNo { get; set; }
        public string ImporterName { get; set; }
        public string GatePassNo { get; set; }
        public string DeliveryDate { get; set; }
        public int NoOfPackagesDeli { get; set; }
        public string Remarks { get; set; }
    }
}