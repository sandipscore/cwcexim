using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class PpgThcFscV2
    {
        public int THCFSCChargesId { get; set; }
        [Display(Name = "Operation Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int OperationType { get; set; }
        [Display(Name = "Operation Code")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string OperationCode { get; set; }
        public int OperationId { get; set; }
        [Display(Name = "Container Type")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int ContainerType { get; set; }
        [Display(Name = "Type")]
        //[Required(ErrorMessage = "Fill Out This Field")]
        public int Type { get; set; }
        /*[Display(Name = "Description")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Description { get; set; }*/
        [Display(Name = "Size")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int Size { get; set; }
        [Display(Name = "Max Distance")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal MaxDistance { get; set; }
        [Display(Name = "Rate CWC")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal RateCWC { get; set; }

        [Display(Name = "Commodity Type")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int CommodityType { get; set; }

        /*[Display(Name = "Contractor")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public int ContractorId { get; set; }*/

        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EffectiveDate { get; set; }
        /*public string ContractorName { get; set; }*/
        [Display(Name = "Contractor Rate")]
        public decimal ContractorRate { get; set; } = 0M;
        public string OperationDesc { get; set; }
        public string ContainerLoadType { get; set; }
        public string TransportFrom { get; set; }
        public string EximType { get; set; }
        public IList<Operation> LstOperation { get; set; } = new List<Operation>();

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Via")]
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "From(Ft/Metric)")]
        public decimal FromMetric { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "To(Ft/Metric)")]
        public decimal ToMetric { get; set; }

        public IList<PPGLocationV2> LstLocation = new List<PPGLocationV2>();
    }
}