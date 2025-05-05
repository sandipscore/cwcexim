using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WLJ_CCINPrint
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string Module { get; set; }
        public string ModuleName { get; set; }
        public int CCINId { get; set; }
        public string CCINNO { get; set; }
        public string CCINDate { get; set; }
        public string SBNo { get; set; }
        public string SBDate { get; set; }
        public string CHA { get; set; }
        public string Exporter { get; set; }
        public string Country { get; set; }
        public string GodownNo { get; set; }
        public string CargoType { get; set; }
        public decimal NoofPkg { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal FOB { get; set; }
        public string Cargo { get; set; }
        public List<WLJ_ShedEntries> Lstshed = new List<WLJ_ShedEntries>();
        public string CargoInvNo { get; set; }
        public string CargoInvDt { get; set; }
        public string PortDestName { get; set; }
        public string PackageType { get; set; }
    }
    public class WLJ_ShedEntries
    {
        public string CartingDate { get; set; }
        public string Open { get; set; }
        public string GodownNo { get; set; }
        public string SpaceType { get; set; }
        public decimal Area { get; set; }
        public decimal NoOfPkg { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal ShortPkg { get; set; }
        public decimal ExcessPkg { get; set; }


    }
}