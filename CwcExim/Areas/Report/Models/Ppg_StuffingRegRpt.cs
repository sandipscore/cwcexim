using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{
    public class Ppg_StuffingRegRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }


        public int SlNo { get; set; }
        public string Date { get; set; }

        public string CfsCode { get; set; }


        public string ContainerNo { get; set; }

        public string Size { get; set; }

        public string ExporterName { get; set; }

        public string ShippingLineName { get; set; }

        public string CHAName { get; set; }

        public string Cargo { get; set; }

        public int NoOfUnits { get; set; }

        public string shippingBillNo { get; set; }

        public string shippingBillDate { get; set; }

        public string shippingBillAndDate { get; set; }

        public string pod { get; set; }

        public decimal Fob { get; set; }

        public decimal Weight { get; set; }

        public string StfRegisterNo { get; set; }

        public string StfRegisterDate { get; set; }
        public string ForwarderName { get; set; }
        public string StuffType { get; set; }
        public string POL { get; set; }
        public decimal Area { get; set; }
        public int ContainerStuffingId { get; set; }
        public string GodownNo { get; set; }
        public List<Ppg_StuffingRegRpt> LstStuff { get; set; } = new List<Ppg_StuffingRegRpt>();
        public List<Ppg_StuffingDetail> LstStuffDetails { get; set; } = new List<Ppg_StuffingDetail>();
    }
    public class Ppg_StuffingDetail
    {
        public string ExporterName { get; set; }
        public string CHA { get; set; }
        public int NoOfUnit { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public decimal Area { get; set; }
        public string PortOfLoading { get; set; }
        public string CFSCode { get; set; }
        public int ContainerStuffingId { get; set; }
    }
}