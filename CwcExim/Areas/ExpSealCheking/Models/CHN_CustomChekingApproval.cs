using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.ExpSealCheking.Models
{
    public class CHN_CustomChekingApproval
    {
     

        [Display(Name = "Truck Slip No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string TruckSlipNo { get; set; }


        [Display(Name = "Container No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ContainerNo { get; set; }


        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Size { get; set; }

        [Display(Name = "CHA:")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHAName { get; set; }
        public int ? CHAId { get; set; }


        [Display(Name = "Vehicle No")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string VehicleNo { get; set; }

        public string Time { get; set; }
        public string EntryTime { get; set; }

        public string SystemDateTime { get; set; }

        public int Uid { get; set; }

        [Display(Name = "Exam Required")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ExamRequired { get; set; }

        public int CustomId { get; set; }

        public string TruckSlipDate { get; set; }

        public int GateEntryId { get; set; }

        public int BranchId { get; set; }

        public string CFSCode { get; set; }
    }
    //public class CHA1
    //{
    //    public int CHAId { get; set; }
    //    public string CHAName { get; set; }
    //}

    //public class Exporter1
    //{
    //    public int ExporterId { get; set; }
    //    public string ExporterName { get; set; }
    //}

    public class TruckSlip
    {
        public string TruckSlipNo { get; set; }

        public int GateEntryId { get; set; }
    }


}
