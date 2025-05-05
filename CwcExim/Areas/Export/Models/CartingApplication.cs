using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class CartingApplication : CartingDetails
    { 
        public string CartingNo { get; set; }
        [Required(ErrorMessage="Fill Out This Field")]
        [Display(Name ="CHA")]
        public string CHAName { get; set; }
        public IList<Commodity> lstCommodity { get; set; } = new List<Commodity>();
        public IList<Exporter> lstExporter { get; set; } = new List<Exporter>();
        public IList<CHA> lstCHANames { get; set; } = new List<CHA>();
        public IList<ShippingDetails> lstShipping { get; set; } = new List<ShippingDetails>();
        public string StringifyData { get; set; }
        public string SCMTRPackageType { get; set; }

        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
    }
    public class Commodity
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }

      

    }


    public class ShipingLine
    {
        public int Id { get; set; }
        public string ShippingLineName { get; set; }

    }
    public class Exporter
    {
        public int EXPEximTraderId { get; set; }
        public string ExporterName { get; set; }
    }
    public class CHA
    {
        public int CHAEximTraderId { get; set; }
        public string CHAName { get; set; }
    }
    public class PrintCA
    {
        public string ShipBillNo { get; set; }
        public string ShipBillDate { get; set; }
        public string Exporter { get; set; }
        public string Commodity { get; set; }
        public string MarksAndNo { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Weight { get; set; }
        public string CartingNo { get; set; }
        public string CartingDt { get; set; }
        public string CHAName { get; set; }
        public string SCMTRPackageType { get; set; }
    }
}