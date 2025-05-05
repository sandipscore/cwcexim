using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Ppg_ImportSeciIncRpt
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PayeeName { get; set; }
        public string PartyCode { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string EntryNo { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }

        public string ShippingLine { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string VesselNo { get; set; }
        public string InvoiceType { get; set; }

        public decimal GDV { get; set; }
        public decimal GCD { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public string SLACode { get; set; }
        public string PortCode { get; set; }
        public string DestuffingDate { get; set; }
        public string CustomSealNo { get; set; }
        public string ShedNo { get; set; }
        public decimal THC { get; set; }
        public decimal CHT { get; set; }
        public decimal CH { get; set; }
        public decimal SW { get; set; }
        public decimal OSI { get; set; }
        public decimal IRR { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
        //  public List<Ppg_ImportSeciIncRptDtl> lstContDtl { get; set; } = new List<Ppg_ImportSeciIncRptDtl>();
    }


   
}