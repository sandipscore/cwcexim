using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class StatementOfEmptyContainer
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string ContainerNo { get; set; }
        public string Size { get; set; }
      
        public string ShippingLine { get; set; }
        
        public string DateOfArrival { get; set; }
        public string ImportExport { get; set; }






    }

   

}