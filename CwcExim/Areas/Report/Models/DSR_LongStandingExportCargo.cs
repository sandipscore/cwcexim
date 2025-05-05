using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_LongStandingExportCargo
    {
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int days { get; set; }
    }
    public class DSR_LongStandingExportCargoDtl
    {
        public string ShippingBillNo { get; set; }
        public string ShippingBillNoDate { get; set; }
        public string CCINInvoiceNo { get; set; }
        public string CCINInvoiceDate { get; set; }
        public string ExporterName { get; set; }
        public string ExporterAddress { get; set; }
        public string CFSCode { get; set; }
        public string ChaName { get; set; }        
        public string StuffingDate { get; set; }
        public string ShippingLineCode { get; set; }
        public int NoOfPkg { get; set; }
        public string InDate { get; set; }      
        public decimal Area { get; set; }
        public decimal Fob { get; set; }
        public decimal GrWt { get; set; }
        public string Commodity { get; set; }
        public string GH { get; set; }
        public decimal StroageCharges { get; set; }
        public string Notice1 { get; set; }
        public string Date1 { get; set; }
        public string Notice2 { get; set; }
        public string Date2 { get; set; }
        public string Nocr { get; set; }

        public string NocDate { get; set; }
        public string SeizeDate { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
    }
}