using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kol_LoadContReqDtl : LoadContReqDtl
    {
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }
        public string ContLoadType { get; set; }
        public string CustomSeal { get; set; }
        public bool IsSEZ { get; set; }

        public int PacketsTo { get; set; }
        public int PacketsFrom { get; set; }
    }
    public class Kol_LoadCont
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string EntryNo { get; set; }
        public string CargoType { get; set; }
    }
}