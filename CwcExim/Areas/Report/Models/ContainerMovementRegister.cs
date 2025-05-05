using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class ContainerMovementRegister
    {
        //public string InvoiceNumber { get; set; }
        public string Date { get; set; }
      
        public string ContainerNo { get; set; }
        
        public string Size { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Type { get; set; }

        public string LoadedOrEmpty { get; set; }

        public string ShippingLine { get; set; }
        public string Party { get; set; }
        public string LoadedOrEmpty1 { get; set; }
        public string InOrOut { get; set; }

        public string ImportExport { get; set; }



    }

   


}