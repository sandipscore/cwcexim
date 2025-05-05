using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class CargoDailyReport
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

        public IList<CargoDailyList> LstCargoDaily = new List<CargoDailyList>();

        public IList<CargoSummaryList> LstCargoSummary = new List<CargoSummaryList>();
    }
    public class CargoDailyList
    {
        public string RegisterDate { get; set; }
        public string VehicleNo { get; set; }
        public string NoOfPackages { get; set; }
        public string Weight { get; set; }
        public string Commodity { get; set; }
        public string Party { get; set; }
    }
    public class CargoSummaryList
    {
        public string Weight { get; set; }
        public string Commodity { get; set; }
        public string Party { get; set; }
    }
}