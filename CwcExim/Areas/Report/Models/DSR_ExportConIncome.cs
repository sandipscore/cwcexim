using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_ExportConIncome
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string FromDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ToDate { get; set; }

    }
    
    public class DSR_ExportConIncomeDtl
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string PartyName { get; set; }
        public string PartyCode { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Size { get; set; }
        public string MovementType { get; set; }
        public string ContainerType { get; set; }
        public decimal CargoWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal TotalWeight { get; set; }
        public string SLACode { get; set; }
        public string PortCode { get; set; }
        public string  PortOfLoading { get; set; }
        public string StuffingDate { get; set; }
        public string MovementDate { get; set; }
        public string SealDate { get; set; }
        public string ShippingBillNo { get; set; }
        public int GodownId { get; set; }
        public int LocationIds { get; set; }
        public string ShedNo { get; set; }
        public string ShedArea { get; set; }
        public int Week { get; set; }
        public decimal FOBValue { get; set; }
        public decimal HND { get; set; }
        public decimal THC { get; set; }
        public decimal RR { get; set; }
        public decimal FNC { get; set; }
        public decimal WHT { get; set; }
        public decimal GRL { get; set; }
        public decimal GRE { get; set; }
        public decimal MO { get; set; }
        public decimal INS { get; set; }
        public decimal GEN { get; set; }
        public decimal HAZ { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal Total { get; set; }
    }
}