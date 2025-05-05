using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Kdl_ImportIGMModel
    {
        public int ShippingLineId { get; set; }

        [Display(Name ="Shipping Line"),Required(ErrorMessage ="Fill Out This Field")]
        public string ShippingLineName { get; set; }

        [Display(Name = "Vessel No."), Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[a-zA-Z0-9._ ]*$", ErrorMessage = "invalid characters found")]
        public string VesselNo { get; set; }

        [Display(Name = "Voyage No."), Required(ErrorMessage = "Fill Out This Field")]
        [RegularExpression("^[a-zA-Z0-9._ ]*$", ErrorMessage = "invalid characters found")]
        public string VoyageNo { get; set; }

        [Display(Name ="Rotation No.")]
        public string RotationNo { get; set; }
        public string FileName { get; set; }
        public IList<ShippingLine> lstShippingLine { get; set; } = new List<ShippingLine>();
    }
}