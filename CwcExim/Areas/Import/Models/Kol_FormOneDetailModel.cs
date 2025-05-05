using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kol_FormOneDetailModel
    {
        public int FormOneDetailID { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public int Reefer { get; set; }
        public int FlatReck { get; set; }
        public string SealNo { get; set; }
        public string LineNo { get; set; }
        public string MarksNo { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string CargoDesc { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int CargoType { get; set; }
        public string DateOfLanding { get; set; }
        public string Remarks { get; set; }
    }
    
}