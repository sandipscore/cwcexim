using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Master.Models
{
    public class HDBSac
    {
        public int SACId { get; set; }
        [Display(Name = "SAC")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "SAC can contain only 6 Digits")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string SACCode { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000, ErrorMessage = "Description must be within 1000 characters")]
        [RegularExpression("^[a-zA-z0-9 .,'\\r\\n\\-]*$", ErrorMessage = "Description can contain Alphabets,Digits and Special character like . , -")]
        public string Description { get; set; }

        [Display(Name = "GST %")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "GST% must be less then 100")]
        public decimal GST { get; set; }

        [Display(Name = "CESS %")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "CESS% must be less then 100")]
        public decimal CESS { get; set; }

        public int Uid { get; set; }
    }
}