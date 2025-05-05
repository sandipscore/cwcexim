using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CargoStuffingRegister
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Date { get; set; }
        public string ShippingBillNo { get; set; }

        public string ShippingBillDate { get; set; }


        public string ShippingBillSeal { get; set; }

        public string Party { get; set; }

        // public string ContainerNo { get; set; }


        public string ContainerNO { get; set; }

        public string Commodity { get; set; }

        public string NoOfPackage { get; set; }

        public string Remarks { get; set; }
        public string Fob { get; set; }

        public string StuffWeight { get; set; }
            }

   

}