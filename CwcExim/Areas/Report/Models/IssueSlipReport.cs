using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class IssueSlipReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

     

         public string ContainerNo { get; set; }


        public string ContainerSize { get; set; }

        public string VesselName { get; set; }

        public string BOENo { get; set; }
        public string BoeDate { get; set; }

        public string ShippingLine { get; set; }

        public string ImporterExporter { get; set; }



        public string CargoDescription { get; set; }

        public string MarksAndNo { get; set; }
        public string NoOfUnits { get; set; }
        public string Weight { get; set; }

        public string RotationNo { get; set; }
        public string DeliveryNo { get; set; }

        public string DateOfReceiptOfCont { get; set; }
        public string DestuffingDate { get; set; }
        

        public string GridNo { get; set; }
        
        
        public string TotalCWCDues { get; set; }
        public string CRNo { get; set; }
        public string CRDate { get; set; }

        public string ValidTillDate { get; set; }

        //23/3/2018 for kol subir


        public string ImportExport { get; set; }


        public string EmptyLoaded { get; set; }


        //public string value { get; set; }
    }

   

}