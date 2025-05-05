using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSRContainerBalanceInCFS
    { 
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

       

        public string CFsCode { get; set; }

        public string ContainerNo { get; set; }


        public string Size { get; set; }

        public string Type { get; set; }


        public string DaysAtCfs { get; set; }


        public string EntryDate { get; set; }
        public string ShippingLineName { get; set; }
        public string Rotation { get; set; }

        public string ImportExport { get; set; }

        //public string value { get; set; }
    }

   

}