using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class AMD_StuffingRequest
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
        public string ForeignLiner { get; set; }
        public string Vessel { get; set; }
        public string Via { get; set; }
        public string Voyage { get; set; }
        public int Uid { get; set; }
        public string StuffingXML { get; set; }
        public string ContainerXML { get; set; }
        public List<AMD_StuffingRequestDtl> LstStuffing { get; set; } = new List<AMD_StuffingRequestDtl>();
        public List<AMD_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<AMD_StuffingReqContainerDtl>();
        public List<ForwarderList> LstForwarder { get; set; } = new List<ForwarderList>();

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
        public string ContainerNo { get; set; }
        public string SBNO { get; set; } = string.Empty;
    }

    public class AMD_StuffingRequestDtl
    {
        public int StuffingReqDtlId { get; set; }
        public int StuffingReqId { get; set; }
        public int CartingRegisterDtlId { get; set; }

        // public string StuffingReqNo { get; set; }
        public string ShippingBillNo { get; set; }
        public string CommInvNo { get; set; }
        public string ShippingDate { get; set; }
        public int ExporterId { get; set; }
        public string Exporter { get; set; }
        public int CHAId { get; set; }
        public string CHA { get; set; }
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        //  public string ContainerNo { get; set; }
        //  public int ContainerId { get; set; }
        //  public string Size { get; set; }
        //  public int ShippingLineId { get; set; }
        //  public string ShippingLine { get; set; }
        public int NoOfUnits { get; set; }
        public decimal GrossWeight { get; set; }
        //   public int StuffQuantity { get; set; }
        //   public decimal StuffWeight { get; set; }        
        public decimal Fob { get; set; }
        public string CartingRegisterNo { get; set; }
        public int CartingRegisterId { get; set; }
        // public string CFSCode { get; set; }
        public int RQty { get; set; }
        public decimal RWt { get; set; }

        public string CommodityName { get; set; }

    }
    public class AMD_StuffingReqContainerDtl
    {
        public int StuffingReqContrId { get; set; }
        public string CFSCode { get; set; }
        public int StuffQuantity { get; set; }
        public decimal StuffWeight { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLine { get; set; }
        public int CartRegDtlId { get; set; }
        public string CommodityName { get; set; }
        public decimal Fob { get; set; }
    
    }
}