using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CwcExim.Areas.Import.Models  
{
    public class Hdb_FormoneLclDetailModel
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
        public string BLDate { get; set; }
        [DisplayName("BL No.")]
        public string BLNo { get; set; }

        public string IGMNo { get; set; }

        public string IGMDate { get; set; }
        [DisplayName("TSA No")]
        public string TSANo { get; set; }
        [DisplayName("TSA Date")]
        public string TSADate { get; set; }

        public string ShippingLineNameDet { get; set; }
        public int ShippingLineId_dtl { get; set; }
        public string Others { get; set; }
        public string VesselName { get; set; }
        public string VoyageNo { get; set; }
        public string RotationNo { get; set; }
        public string TPNo { get; set; } = string.Empty;
        public string TPDate { get; set; } = string.Empty;
        public int PortofLoading { get; set; } = 0;
        public string IgmImporter { get; set; } = string.Empty;
        [DisplayName("ICE Gate ContainerNo.")]
        public string IceGateContNo { get; set; }

        [DisplayName("ICE Gate ContainerSize.")]
        public string IceGateContSize { get; set; }

    }
}