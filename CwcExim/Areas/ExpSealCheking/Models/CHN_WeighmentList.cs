using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_WeighmentList
    {
        public int WeighmentId { get; set; }

        public string TruckSlipNo { get; set; }

        public string TruckSlipDate { get; set; }

        public string ContainerNo { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal TareWeight { get; set; }
    }
}