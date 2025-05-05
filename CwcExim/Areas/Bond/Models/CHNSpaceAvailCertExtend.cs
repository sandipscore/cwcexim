using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Bond.Models
{
    public class CHNSpaceAvailCertExtend:CHNSpaceAvailableCert
    {
        public int SpaceAvailCertExtId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExtendUpto { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExtendOn { get; set; }
    }
}