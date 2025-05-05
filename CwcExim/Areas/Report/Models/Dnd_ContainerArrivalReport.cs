using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Dnd_ContainerArrivalReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }
        public string ContainerNo { get; set; }
        public string ShippingLine { get; set; }
      
        public string LoadOrEmpty { get; set; }
        
        public string SealNo { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }


        public string Commodity { get; set; }


        public string ImportExport { get; set; }


        public string EmptyLoaded { get; set; }


        public string Size { get; set; }


        public string SizeTewnty { get; set; }

        public string SizeFourty { get; set; }
        public string SizeFourtyFive { get; set; }
        public string ICDCode { get; set; } = "";
        public string VehicleNo { get; set; } = "";
        public string ModeOfTransport { get; set; } = "";

        public List<Dnd_ArrivalReportList> ListArrivalReport = new List<Dnd_ArrivalReportList>();





    }

    public class Dnd_ArrivalReportList

    {
        public string ContainerNo { get; set; }
        public string ShippingLine { get; set; }

        public string LoadOrEmpty { get; set; }

        public string SealNo { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }


        public string Commodity { get; set; }


        public string ImportExport { get; set; }


        public string EmptyLoaded { get; set; }


        public string Size { get; set; }

        public string ICDCode { get; set; } = "";
        public string VehicleNo { get; set; } = "";
        public string ModeOfTransport { get; set; } = "";

        public string CFSPORT { get; set; } = "";
    }

   

}