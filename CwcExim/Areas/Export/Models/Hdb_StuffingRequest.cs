using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_StuffingRequest
    {
        public int StuffingReqId { get; set; }
        public int CartingRegisterId { get; set; }
        public string CartingRegisterNo { get; set; }
        public string StuffingReqNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string RequestDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public int CHAId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string CHA { get; set; }
        public int ShippingHdrLineId { get; set; }
        [Display(Name = "Shipping Line")]
        public string ShippingHdrLine { get; set; }

        [StringLength(100, ErrorMessage = "Remarks Cannot Be More Than 30 Characters")]
        public string Remarks { get; set; }
        public string StuffingType { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string ContainerXML { get; set; }

        [StringLength(30,ErrorMessage ="Gateway Port Cannot Be More Than 30 Characters")]
        public string Destination { get; set; }
        public int Movement { get; set; }
        public int TypeOfTrip { get; set; }
        public List<Hdb_StuffingRequestDtl> LstStuffing { get; set; } = new List<Hdb_StuffingRequestDtl>();
        public List<Hdb_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<Hdb_StuffingReqContainerDtl>();
        public List<Hdb_ForwarderList> LstForwarder { get; set; } = new List<Hdb_ForwarderList>();
        public int ttlQuantity { get; set; } = 0;
        public decimal ttlWeight { get; set; } = 0;
        public decimal ttlValue { get; set; } = 0;
        public string IsConfirm { get; set; }
    }
}

public class Hdb_ForwarderList
{
    public string Forwarder { get; set; }
    public int ForwarderId { get; set; }
}