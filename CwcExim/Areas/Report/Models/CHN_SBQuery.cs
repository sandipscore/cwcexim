using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class CHN_SBQuery
    {
        public string SBNO { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public string PortOFLoad { get; set; }
        public string PortOFDischarge { get; set; }
        public string Comodity { get; set; }
        public string CHA { get; set; }
        public int Cargotype { get; set; }
        public int Package { get; set; }
        public string Vehicle { get; set; }
        public decimal Weight { get; set; }
        public decimal FOB { get; set; }
        public string ShippingLine { get; set; }
        public string Exporter { get; set; }
        public string GateinNo { get; set; }
        public string Country { get; set; }
        public string SBNODate { get; set; }
        public string InvoiceNo { get; set; }
        public string PCIN { get; set; }
    }
}