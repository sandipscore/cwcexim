using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_ImportConIncome
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

    }
    
    public class DSR_ImportConIncomeDtl
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string EntryNo { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public string SLACode { get; set; }
        public string PortCode { get; set; }
        public string DestuffingDate { get; set; }
        public string CustomSealNo { get; set; }
        public string ShedNo { get; set; }
        public decimal THC { get; set; }
        public decimal TPT { get; set; }
        public decimal ECC { get; set; }
        public decimal DTF { get; set; }
        public decimal LOL { get; set; }
        public decimal IRR { get; set; }
        public decimal HAZ { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
    }
}