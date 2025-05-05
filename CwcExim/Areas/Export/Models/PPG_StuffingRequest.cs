using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CwcExim.Areas.Export.Models
{
    public class PPG_StuffingRequest
    {
        public int StuffingReqId { get; set; }
        public int CartingRegisterId { get; set; }       
        public string CartingRegisterNo { get; set; }
        public string StuffingReqNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string RequestDate { get; set; }

        //[Required(ErrorMessage ="Fill Out This Field")]
        public int CHAId { get; set; }

        //[Required(ErrorMessage ="Fill Out This Field")]
        public string CHA { get; set; }

        public int ShippingHdrLineId { get; set; }
        [Display(Name = "Shipping Line")]
        public string ShippingHdrLine { get; set; }
        public int ForwarderId { get; set; }
        public string Forwarder { get; set; }


        public string Remarks { get; set; }
        public string StuffingType { get; set; }
        public string  ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Via { get; set; }
        public string Voyage { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string ContainerXML { get; set; }
        public List<PPG_StuffingRequestDtl> LstStuffing { get; set; } = new List<PPG_StuffingRequestDtl>();
        public List<PPG_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<PPG_StuffingReqContainerDtl>();
        public List<ForwarderList> LstForwarder { get; set; } = new List<ForwarderList>();

        public int? NoOfUnits { get; set; }
        public decimal? Fob { get; set; }
        public decimal? StuffWeight { get; set; }

        public int StuffingPlanId { get; set; }

        [Display(Name = "Stuffing PlanNo")]
        public string StuffingPlanNo { get; set; }

    }

   
}