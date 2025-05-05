using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Kdl_LoadContReq: LoadContReq
    {
        public int FinalDestinationLocationID { get; set; }
        public string FinalDestinationLocation { get; set; }        
    }
    public class Kdl_LoadContReqDtl : LoadContReqDtl
    {
        public string EquipmentSealType { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentQUC { get; set; }
        public string PackageType { get; set; }

        public int PacketsFrom { get; set; }
        public int PacketsTo { get; set; }
    }
}