using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Icegate.Models
{
    public class IceGateResendData
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }
    }
   
}