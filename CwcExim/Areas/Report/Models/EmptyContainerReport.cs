using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class EmptyContainerReport
    {

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        // [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }



        //#for list
        public string ImporterName { get; set; }
        public string PayeeName { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }

        public string NoOfDays { get; set; }
        public string LiftOfEmpty { get; set; }


        public string GroundRentEmpty { get; set; }

        public string CGST { get; set; }

        public string SGST { get; set; }


        public string IGST { get; set; } 
        public decimal Total { get; set; }



        public IList<EmptyContainerReport> ObjTDSReporPartyWise = new List<EmptyContainerReport>();

    }

  
}
