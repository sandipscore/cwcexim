using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDIWBDetails
    {
       
        public int GatePassId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GatePassNo { get; set; }
        public string GatePassDate { get; set; }
        public int InvoiceId { get; set; }
        public int IWBId { get; set; }
        public string IWBNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string IWBDate { get; set; }
        public string Consignor { get; set; }
        public string Liner { get; set; }
        public string Forwarder { get; set; }
        public string Transporter { get; set; }
        public string CFSCode { get; set; }
        public string Container { get; set; }
        public string TrailerNo { get; set; }
        public string OTLNo { get; set; }
        public string SealNo { get; set; }
        public int NoOfPkg { get; set; }
        public decimal Weight { get; set; }
        public decimal CIFValue { get; set; }
        public string PlaceOfStuffing { get; set; }
        public string PortOfLoading { get; set; }
        public string CHAName { get; set; }
        public string ExporterName { get; set; }
        public string ShippingLineName { get; set; }
        public string CargoDesc { get; set; }
        public string ShippingSeal { get; set; }
        public string ShippBillNo { get; set; }
        public string ShippBillDate { get; set; }
        public string Destination { get; set; }
        public string Country { get; set; }
        public int Uid { get; set; }
        public string IsOperation { get; set; }
        public string Module { get; set; }
    }
}