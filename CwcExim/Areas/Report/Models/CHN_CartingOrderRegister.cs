using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class CHN_CartingOrderRegister
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int SlNo { get; set; }

        public string Date { get; set; }

        public string ExporterName { get; set; }


        public string ChaName { get; set; }

        public string Cargo { get; set; }

        public string NoOfUnits { get; set; }

        public string Weight { get; set; }

        public string FobValue { get; set; }

        public string Destination { get; set; }

        public string vessel { get; set; }

        public string SignaturOfParty { get; set; }
        public string ShippingBillNo { get; set; }
        public string ShippingBillDate { get; set; }
        public string GodownName { get; set; }
        public string RegisterNo { get; set; }
        public string Location { get; set; }
    }
}