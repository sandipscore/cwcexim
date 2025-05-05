using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Dnd_StuffingRequest
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
        [Required(ErrorMessage ="Fill Out This Field")]
        public string Forwarder { get; set; }


        public string Remarks { get; set; }
        public string StuffingType { get; set; }
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public int ViaId { get; set; } = 0;
        [Required(ErrorMessage ="Fill Out This Field")]
        public string Via { get; set; }
        public string Voyage { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string ContainerXML { get; set; }
        public List<Dnd_StuffingRequestDtl> LstStuffing { get; set; } = new List<Dnd_StuffingRequestDtl>();
        public List<Dnd_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<Dnd_StuffingReqContainerDtl>();
        public List<ForwarderList> LstForwarder { get; set; } = new List<ForwarderList>();
        public IList<Dnd_StuffingRequest> lstCartingDetailList { get; set; } = new List<Dnd_StuffingRequest>();
        public List<MainlineList> LstMln { get; set; } = new List<MainlineList>();
        public int? NoOfUnits { get; set; }
        public decimal? Fob { get; set; }
        public decimal? StuffWeight { get; set; }
        public int? Movement { get; set; }
        public int? TypeOfTrip { get; set; }
        public decimal Distance { get; set; } = 0;
        public int? PortId { get; set; }
        public string PortName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        [Required(ErrorMessage ="Fill Out This Field")]
        public string POD { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string SBNO { get; set; } = string.Empty;
        public int MyProperty { get; set; }
        public int MainlineId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Mainline { get; set; }
    }
    public class MainlineList
    {
        public string Mainline { get; set; }
        public int MainlineId { get; set; }
    }

}