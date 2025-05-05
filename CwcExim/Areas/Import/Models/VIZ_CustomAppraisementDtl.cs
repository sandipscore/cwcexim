using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class VIZ_CustomAppraisementDtl
    {
        public int RMSValue { get; set; }
        public int CustomAppraisementDtlId { get; set; }
        public int CustomAppraisementId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public string OBLNo { get; set; }
        public string OBLDate { get; set; }
        public int CHANameId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }
        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public string WithoutDOSealNo { get; set; }
        public string Rotation { get; set; }
        public int? ContainerType { get; set; }
        public int? CargoType { get; set; }
        public int? Reefer { get; set; }
        public int? RMS { get; set; }
        public int? HeavyScrap { get; set; }
        public int? AppraisementPerct { get; set; }
        public int IsInsured { get; set; }

    }
}