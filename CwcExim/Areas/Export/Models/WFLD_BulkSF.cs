using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_BulkSF
    {
        [Required]
        public string PeriodFrom { get; set; }

        [Required]
        public string PeriodTo { get; set; }

        public string ContainerNo { get; set; }

        public int ContainerStuffingDtlId { get; set; }

        public string Status { get; set; }

        public string CFSCode { get; set; }
    }
}