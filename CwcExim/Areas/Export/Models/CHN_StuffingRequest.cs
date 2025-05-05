using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class CHN_StuffingRequest
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
        public List<CHN_StuffingRequestDtl> LstStuffing { get; set; } = new List<CHN_StuffingRequestDtl>();
        public List<CHN_StuffingReqContainerDtl> LstStuffingContainer { get; set; } = new List<CHN_StuffingReqContainerDtl>();
        public List<CHN_ForwarderList> LstForwarder { get; set; } = new List<CHN_ForwarderList>();

        public int? NoOfUnits { get; set; }
        public decimal? Fob { get; set; }
        public decimal? StuffWeight { get; set; }
        public string SBNO { get; set; }
        public string ContainerNo { get; set; }
        public int? Movement { get; set; }
        public int? TypeOfTrip { get; set; }
        public decimal Distance { get; set; } = 0;
        public int? PortId { get; set; }
        public string PortName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
    }
    public class CHN_StuffingRequestDtl
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

        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }

    }

    public class CHN_StuffingReqContainerDtl
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
    public class CHN_ForwarderList
    {
        public string Forwarder { get; set; }
        public int ForwarderId { get; set; }
    }
}