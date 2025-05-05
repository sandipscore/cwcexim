using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    [MetadataType(typeof(EditProfileMD))]
    public class EditProfile:UserBase
    {
        
        public string GeneratedCode { get; set; }
    }
}