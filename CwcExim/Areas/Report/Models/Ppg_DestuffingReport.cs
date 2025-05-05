using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_DestuffingReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }



        public string DeStuffingNo { get; set; }


        public string DeStuffingDate { get; set; }

        public string ContainerNo { get; set; }

        public string ContainerSize { get; set; }
        public string CFSCode { get; set; }

        public string ShippingLine { get; set; }

        public string VesselName { get; set; }



        public string VoyageNo { get; set; }

        public string Rotation { get; set; }
        public string SlSealNo { get; set; }
        public string CustomSealNO { get; set; }

        //public string ContainerSize { get; set; }
        public string LineNo { get; set; }

        public string BoeNo { get; set; }

        public string BoeDate { get; set; }

        public string BolNo { get; set; }
        public string BolDate { get; set; }


        public string MarksNo { get; set; }


        public string CHA { get; set; }
        public string Importer { get; set; }
        public string CargoDescription { get; set; }

        public string Commodity { get; set; }

        public string CargoType { get; set; }

        public string NoOfPackets { get; set; }
        public string YardLocation { get; set; }
        public string Grossweight { get; set; }
        public string CIFValue { get; set; }
        public string Duty { get; set; }


    }
}