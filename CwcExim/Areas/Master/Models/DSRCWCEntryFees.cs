using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class DSRCWCEntryFees
    {
        public int EntryFeeId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container/CBT Type")]
        public int ContainerType { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Commodity Type")]
        public int CommodityType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Operation Type")]
        public int OperationType { get; set; }

        public bool Reefer { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(minimum: 0, maximum: 99999999.99, ErrorMessage = "Rate must be less than 99999999.99")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Size")]
        public string ContainerSize { get; set; }

        [Display(Name = "SAC Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }

        public decimal WeightSlab { get; set; }
    }
}