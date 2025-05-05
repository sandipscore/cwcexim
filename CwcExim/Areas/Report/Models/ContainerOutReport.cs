using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class ContainerOutReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public List<ContainerOutReportList> lstContainerOutReport = new List<ContainerOutReportList>();


        public string Date { get; set; }
        public string ContainerNo { get; set; }

        public string Size { get; set; }

        // public string ContainerNo { get; set; }


        public string Remarks { get; set; }

        public string Time { get; set; }

        public string LoadedEmpty { get; set; }


        public string ImportExport { get; set; }

        public string SizeTwenty { get; set; }

        public string SizeFouirty { get; set; }

        //public string value { get; set; }
    }

    public class ContainerOutReportList

    {
        //[Required(ErrorMessage = "Fill Out This Field")]
        //public string PeriodFrom { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        //public string PeriodTo { get; set; }
        public string Date { get; set; }
        public string ContainerNo { get; set; }

        public string Size { get; set; }

        // public string ContainerNo { get; set; }


        public string Remarks { get; set; }

        public string Time { get; set; }

        public string LoadedEmpty { get; set; }


        public string ImportExport { get; set; }

    }




}