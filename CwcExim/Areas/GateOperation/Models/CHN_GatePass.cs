using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.GateOperation.Models
{
    public class CHN_GatePass
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }
        public string StringifyData { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string DeliveryDate { get; set; }
       [Required(ErrorMessage = "Fill Out This Field")]
        public string DepartureDate { get; set; }
       [Required(ErrorMessage = "Fill Out This Field")]
        public string ArrivalDate { get; set; }
        public string Module { get; set; }
        public int PortId { get; set; }
        public string PortName { get; set; }
    }

    public class CHN_GPHdr : GPHdr
    {
        public string ValidTill { get; set; }
        public string PortName { get; set; }
        public string Module { get; set; }
        public string BondNo { get; set; }

        public int GodownId { get; set; }

        public string BondDate { get; set; }

        public string VehicleNo { get; set; }

        public string CargoDescription { get; set; }

        public int NoOfUnits { get; set; }

        public decimal Weight { get; set; }
        public string CompanyAdrress { get; set; } = string.Empty;
        public string CompanyMail { get; set; } = string.Empty;
        public string CompanyShortName { get; set; } = string.Empty;
        public string CompanyLocation { get; set; } = string.Empty;
        public IList<CHNGPDet> lstDet { get; set; } = new List<CHNGPDet>();

    }
    public class CHNGPDet
    {
        public string ContainerNo { get; set; }
        public decimal NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public int CargoType { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string LineNo { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string ShippingSealNo { get; set; }
        public string CustomSealNo { get; set; }
    }

    public class ListOfCHNGP
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ContainerNo { get; set; } = string.Empty;
        public string VehicleNo { get; set; } = string.Empty;
        public string IsCancelled { get; set; }
    }

    public class Chn_ContainerDet
    {
        public int GatePassDtlId { get; set; }
        public string ContainerNo { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ImpExpId { get; set; }
        public string ImpExpName { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }
        public string CargoDescription { get; set; }
        public int CargoType { get; set; }
        public int NoOfUnits { get; set; }
        public string VehicleNo { get; set; }
        public decimal Weight { get; set; }
        public string Location { get; set; }
        public string Via { get; set; }
        public string Vessel { get; set; }
        public string PortOfLoading { get; set; }
        public string CustomSeal { get; set; }
        public string ShippingSeal { get; set; }
        public string Module { get; set; }
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }

        public string DeliveryDate { get; set; }
    }
}