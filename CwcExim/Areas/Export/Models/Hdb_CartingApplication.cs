using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_CartingApplication: Hdb_CartingDetails
    {
     
          public string CartingNo { get; set; }

         [Required(ErrorMessage = "Fill Out This Field")]
         [Display(Name = "CHA")]
         public string CHAName { get; set; }

        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Custom House Code")]
        public string CustomHouseCode { get; set; }


        //[Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "IEC Code")]
        public string IECCode { get; set; }


      //  [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Port Of Destination")]
        public string PortOfDestination { get; set; }
       

       // [Required(ErrorMessage = "Fill Out This Field")]
        [Display(Name = "Package Type")]
        public string PackageType { get; set; }


        public string OtherPkgType { get; set; }


        public string SCMTRPackageType { get; set; }
        public int PackUQCId { get; set; }
        public string PackUQCCode { get; set; }
        public string PackUQCDescription { get; set; }
        public string GSTNo { get; set; }

        public string ForwarderName { get; set; }
           public int ForwarderId { get; set; }
           public string VehicleNo { get; set; }
         public IList<Hdb_Commodity> lstCommodity { get; set; } = new List<Hdb_Commodity>();
            public IList<Hdb_Exporter> lstExporter { get; set; } = new List<Hdb_Exporter>();
            public IList<Hdb_CHA> lstCHANames { get; set; } = new List<Hdb_CHA>();
            public IList<Hdb_ShippingDetails> lstShipping { get; set; } = new List<Hdb_ShippingDetails>();
        public IList<Hdb_ForwarderList> LstForwarder { get; set; } = new List<Hdb_ForwarderList>();
        public string StringifyData { get; set; }
        }
        public class Hdb_Commodity
        {
            public int CommodityId { get; set; }
            public string CommodityName { get; set; }
        }
        public class Hdb_Exporter
        {
            public int EXPEximTraderId { get; set; }
            public string ExporterName { get; set; }
        }
        public class Hdb_CHA
        {
            public int CHAEximTraderId { get; set; }
            public string CHAName { get; set; }
        }
        public class Hdb_PrintCA
        {
            public string ShipBillNo { get; set; }
            public string ShipBillDate { get; set; }
            public string Exporter { get; set; }
            public string Commodity { get; set; }
            public string MarksAndNo { get; set; }
            public int NoOfUnits { get; set; }
            public string PkgType { get; set; }
            public decimal Weight { get; set; }
            public string CartingNo { get; set; }
            public string CartingDt { get; set; }
            public string CHAName { get; set; }
            public decimal FoBValue { get; set; }
           public string CargoDescription { get; set; }
            public string SCMTRPackageType { get; set; }
    }

    public class Hdb_ForwarderList
    {
        public string Forwarder { get; set; }
        public int ForwarderId { get; set; }
    }

    public class Hdb_SBList
    {
        public int Id { get; set; }
        public string SBNo { get; set; }
    }
}
