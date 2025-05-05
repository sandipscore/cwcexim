using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CwcExim.Areas.Export.Models
{
    public class DSR_StuffingRequest
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
        public string MainLine { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string ContainerXML { get; set; }
        public List<DSR_StuffingRequestDtl> LstStuffing { get; set; } = new List<DSR_StuffingRequestDtl>();
        public List<DSR_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<DSR_StuffingReqContainerDtl>();
        public List<ForwarderList> LstForwarder { get; set; } = new List<ForwarderList>();

        public int? NoOfUnits { get; set; }
        public decimal? Fob { get; set; }
        public decimal? StuffWeight { get; set; }

        public int ShortCargoDtlId { get; set; }
        public int? GodownId { get; set; }

    }

    public class DSRCHN_StuffingRequest : DSR_StuffingRequest
    {
        public int? Movement { get; set; }
        public int? TypeOfTrip { get; set; }
        public decimal Distance { get; set; } = 0;
        public int? PortId { get; set; }
        public string PortName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
    }
}