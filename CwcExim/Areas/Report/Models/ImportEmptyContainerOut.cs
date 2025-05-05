using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class ImportEmptyContainerOut
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string ICDCode { get; set; }
        public string ContainerNo { get; set; }
        public List<ImportEmptyContainerOut> lstContainerOutReport = new List<ImportEmptyContainerOut>();
        public string Size { get; set; }

        // public string ContainerNo { get; set; }
        public string ShippingLine { get; set; }

        public string Remarks { get; set; }
        public string VehicleNo { get; set; }
        public string SizeTwenty { get; set; }

        public string SizeFouirty { get; set; }

        public string Time { get; set; }

    }
}