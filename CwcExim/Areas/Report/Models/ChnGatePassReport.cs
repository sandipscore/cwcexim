using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class ChnGatePassReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }

        public string VehicleNo { get; set; }

        public string ContainerNo { get; set; }


        public string ContainerSize { get; set; }

        public string VesselName { get; set; }

        public string VoyageNo { get; set; }

        public string RotationNo { get; set; }

        public string LineNo { get; set; }

        public string ShippingSealLineNo { get; set; }
        public string CustomSealLineNo { get; set; }
        public string ImporterExporter { get; set; }

        public string CHAName { get; set; }
        public string ShippingLine { get; set; }
        public string BOENoOrSBNoOrWRNo { get; set; }
        public string Date { get; set; }
        public string NoOfPackages { get; set; }
        public string Weight { get; set; }
        public string LocationName { get; set; }

        public string NatureOfGoods { get; set; }

        public string InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string Rotation { get; set; }
    }
}