using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_FormOneDetailModel
    {
        public int FormOneDetailID { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public int IsODC { get; set; }
        public int Reefer { get; set; }
        public int FlatReck { get; set; }
        public string SealNo { get; set; }
        public string LineNo { get; set; }
        public string MarksNo { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public int ForwarderId { get; set; }
        public string ForwarderName { get; set; }
        public string CargoDesc { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int CargoType { get; set; }
        public string DateOfLanding { get; set; }
        public string Remarks { get; set; }
      
        public string LCLFCL { get; set; }
        public string VehicleNo { get; set; }

        public decimal CIFValue { get; set; }
        public decimal GrossDuty { get; set; }
        public string PackageType { get; set; }
        public int Noofpkg { get; set; }
        public decimal GrossWeight { get; set; }
        public string OBLNO { get; set; }
        public string OBLDate { get; set; }
        public string ShippingLineNameDet { get; set; }
        public int ShippingLineId_dtl { get; set; }
        public string Others { get; set; }
        public string IGMNo { get; set; } = string.Empty;
        public string IGMdate { get; set; } = string.Empty;
        public string TPNo { get; set; } = string.Empty;
        public string TPdate { get; set; } = string.Empty;
        public int PortOfLoading { get; set; } = 0;
        public string IgmImporter { get; set; } = string.Empty;

    }

}