using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_AbstractAccuredReport
    {

        public string InvType { get; set; }
        public string Headname { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal RoundOff { get; set; }
        public decimal SGSTCGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        public decimal CGSTTaxable { get; set; }
        public decimal IGSTTaxable { get; set; }
        public decimal BillTaxable { get; set; }
        public decimal CGSTRoundUp { get; set; }
        public decimal IGSTRoundUp { get; set; }
        public decimal BillRoundUp { get; set; }
        public decimal TotalRoundUp { get; set; }
        public List<Hdb_AbstractAccuredReport> lstInv { get; set; } = new List<Hdb_AbstractAccuredReport>();

        public List<Hdb_AbstractAccuredReport> lstCr { get; set; } = new List<Hdb_AbstractAccuredReport>();

    }
}