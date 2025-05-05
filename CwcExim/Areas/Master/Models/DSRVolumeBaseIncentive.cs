using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DSRVolumeBaseIncentive
    {
        //[Required(ErrorMessage = "Fill Out This Field")]
      
        public int IncentiveId { get; set; }

       

       
        [Display(Name = "TEUs Range")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(1, 9999999999, ErrorMessage = "TEUs Range must be greater then 1 and less then 9999999999")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Days Range From must be integer.")]
        public int DaysRangeFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 9999999999, ErrorMessage = "TEUs Range must be greater then 0 and less then 9999999999")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Days Range To must be integer.")]
        public int DaysRangeTo { get; set; }
        [Display(Name = "Free days")]
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0, 999, ErrorMessage = "Free Days must be less then 999")]
        public int FreeDays { get; set; }
       
      
        [Display(Name = "Operation Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int OperationType { get; set; }
       
        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        [Display(Name = "SacCode")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacCode { get; set; }
     
       
        
    }
}