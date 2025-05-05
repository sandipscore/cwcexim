using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.GateOperation.Models
{
    public class GatePass
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string InvoiceNo { get; set; }
        public int InvoiceId { get; set; }
        public string StringifyData { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string DeliveryDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DepartureDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string ArrivalDate { get; set; }
        public string Modul { get; set; }
    }
    public class InvoiceNoList
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class ContainerDet
    {
        public int GatePassDtlId { get; set; }
        public string ContainerNo { get; set; }
        public bool IsReefer { get; set; }
        public string Size { get; set; }
        /*public int CHAId { get; set; }
        public string CHAName { get; set; }
        public int ImpExpId { get; set; }
        public string ImpExpName { get; set; }
        public int ShippingLineId { get; set; }
        public string ShippingLineName { get; set; }*/
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
        public decimal ElwbTareWeight { get; set; }
        public decimal ElwbCargoWeight { get; set; }
    }
    public class ListOfGP
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string InvoiceNo { get; set; }
        public string IsCancelled { get; set; }
    }
    /*Gate Pass Model For Print*/
    public class GPHdr
    {
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public string Remarks { get; set; }
        public string CHAName { get; set; }
        public string ImpExpName { get; set; }
        public string ShippingLineName { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }
        public IList<GPDet> lstDet { get; set; } = new List<GPDet>();
    }
    public class GPDet
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
    }


}