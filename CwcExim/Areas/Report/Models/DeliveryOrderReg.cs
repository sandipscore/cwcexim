using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DeliveryOrderReg
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
    }

    public class DeliveryOrderRegList
    {
        public string WRNo { get; set; }
        public string WRDate { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string Units { get; set; }
        public string Weight { get; set; }
        public string Importer { get; set; }
        public string CIFValue { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string DeliveryOrderDate { get; set; }
        public string SacNo { get; set; }
        public string SacDate { get; set; }
    }
}