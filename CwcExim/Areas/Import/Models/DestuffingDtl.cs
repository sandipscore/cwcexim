using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DestuffingDtl
    {
        public int DestuffingDtlId { get; set; }
        public int? CustomAppraisementDtlId { get; set; }
        public int DestuffingId { get; set; }
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public int CHANameId { get; set; }
        public string CHA { get; set; }
        public int ImporterId { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }
        public int NoOfPackages { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
        public string WithoutDOSealNo { get; set; }
        public string Rotation { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public int? IsInsured { get; set; }
    }
}