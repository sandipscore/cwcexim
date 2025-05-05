using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ImportDelivery
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string Date { get; set; }
        public int SlNo { get; set; }
        public string TSANo { get; set; }
        public string VehicleNo { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal Amount { get; set; }      

    }
    public class WFLD_ImportDestuffingCBT
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string Date { get; set; }
        public int SlNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public decimal Weight { get; set; }
        public decimal ResWeight { get; set; }
        public decimal Amount { get; set; }     

    }
    public class WFLD_ImportTransportation
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public string Date { get; set; }
        public int SlNo { get; set; }
        public string Size { get; set; }
        public string ContainerNo { get; set; }
        public string PortName { get; set; }
        public decimal GrossWt { get; set; } 
        public decimal Charges { get; set; }       

    }
}