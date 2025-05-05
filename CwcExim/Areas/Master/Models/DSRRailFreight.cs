using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Master.Models
{
    public class DSRRailFreight
    {
        public int RailFreightId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Type")]
        public int ContainerType { get; set; }

        [Display(Name = "Commodity Type")]
        public int CommodityType { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]

      
        [Display(Name = "Operation Type")]
        public int OperationType { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Port Of Origin")]
        public int Port { get; set; }
        public bool Reefer { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Via")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "From(Ft/Metric)")]
        public decimal FromMetric { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "To(Ft/Metric)")]
        public decimal ToMetric { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(minimum: 0, maximum: 99999999.99, ErrorMessage = "Rate must be less than 99999999.99")]
        public decimal Rate { get; set; }

       public string portname { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Container Size")]
        public string ContainerSize { get; set; }
        public string LocationName { get; set; }
        public IList<DSRPost> LstPort = new List<DSRPost>();
        public IList<DSRLocation> LstLocation = new List<DSRLocation>();

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }
    }
}