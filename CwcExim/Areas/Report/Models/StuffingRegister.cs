using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class StuffingRegister
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

       
        public int SlNo { get; set; }
        public string Date { get; set; }

        public string CfsCode { get; set; }


        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string ExporterName { get; set; }

        public string ShippingLineName { get; set; }

        public string CHAName { get; set; }

        public string Cargo { get; set; }

        public string NoOfUnits { get; set; }

        public string shippingBillNo { get; set; }

        public string shippingBillDate { get; set; }

        public string shippingBillAndDate { get; set; }

        public string pod { get; set; }

        public string Fob { get; set; }

        public string Weight { get; set; }

        public string StfRegisterNo { get; set; }

        public string StfRegisterDate { get; set; }

        public string Forwarder { get; set; }
        public string POL { get; set; }

        public string TotalSB { get; set; }
        public string CBM { get; set; }

    }

   

}