using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Models
{
    [MetadataType(typeof(WbdedForcedUserMD))]
    public class WbdedForcedUser:UserBase
    {
        [Display(Name = "Sl No.")]
        public int GeneratedSerialNo { get; set; }
        public string RegistrationNo { get; set; }
        
    }
}