using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class SlineChaImpDailyActivity
    {
        //public string InvoiceNumber { get; set; }
       // public string Date { get; set; }
      
        public string ContainerNo { get; set; }
        
        public string Size { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public string Commodity { get; set; }

        public string CargoDescription { get; set; }

        public string VesselNo { get; set; }
        public string VoyageNo { get; set; }
        public string RotationNo { get; set; }
        public string LineNo { get; set; }

        public string GateInNo { get; set; }


        public string GateInDate { get; set; }

        public string ChaName { get; set; }
        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }
        public string GateOutNo { get; set; }
        public string GateOutDate { get; set; }


        public string EximTraderId { get; set; }

        public string EximTraderName { get; set; }

        public string ImportExport { get; set; }

    }

    public class SlineImpCHAList
    {

        public string id { get; set; }

        public string Name { get; set; }
    }
   


}