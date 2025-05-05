using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    [MetadataType(typeof(SignupWithoutIdMD))]
    public class SignUpWithoutId:UserBase
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GeneratedMobileCode { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string GeneratedEmailCode { get; set; }
    }
}