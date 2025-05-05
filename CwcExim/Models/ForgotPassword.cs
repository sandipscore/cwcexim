using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    [MetadataType(typeof(ForgotPasswordMD))]
    public class ForgotPassword : UserBase
    {
       
        public string OptType { get; set; }
       

    }
}