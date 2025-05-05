using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class PPG_JobOrderByRoad
    {
        public int FormOneId { get; set; }
        [Display (Name="Transport By")]
        public string TransportBy { get; set; }
        [Display(Name = "Job Order No")]
        public string FormOneNo { get; set; }
        //public int JobOrderId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Job Order Date")]
        public DateTime FormOneDate { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string VesselNo { get; set; }
    
    public string PartyCode { get; set; }
        public string StringifyXML { get; set; }
        public string CONTCBT { get; set; }
        public String Remarks { get; set; }

        public List<PPG_ImportJobOrderByRoadDtl> lstFormOneDetail = new List<PPG_ImportJobOrderByRoadDtl>();
        public int? TrainSummaryID { get; set; }

        public string TrainNo { get; set; }

        public string TNo { get; set; }

        public string TrainDt { get; set; }

        public string FormOneDetailId { get; set; }
    }
    public class PPG_ImportJobOrderByRoadDtl
    {
        public int FormOneDetailID { get; set; }
        public int FormOneId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerSize { get; set; }
        public string LCLFCL { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string VesselNo { get; set; }
    }
    public class PPG_ImportJobOrderByRoadList
    {
        public int FormOneDetailID { get; set; }
        public int FormOneId { get; set; }
        public string FormOneNo { get; set; }
        public string FormOneDate { get; set; }
        public string TransportBy { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string CONTCBT { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string VesselNo { get; set; }

    } 
}