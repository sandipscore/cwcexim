using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Chn_LoadContReqDtl : LoadContReqDtl
    {
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }
        public string ContLoadType { get; set; }
        public string CustomSeal { get; set; }
        public int PacketsTo { get; set; }
        public int PacketsFrom { get; set; }

    }
}