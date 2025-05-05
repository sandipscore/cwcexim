using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class DSR_ThcRrReport
    {

        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }
      
        public string Date { get; set; }
        public string InvoiceNo { get; set; }

        public string ShippingLineName { get; set; }

        public string PayeeCode { get; set; }

        public string DestinationPort { get; set; }

        public string ContainerNo { get; set; }

        public string CFSCode { get; set; }

        public string ContainerSize { get; set; }

        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public decimal TotalWeight { get; set; }

        public decimal TareWeight { get; set; }

        public decimal RRAmount { get; set; }
        public decimal TotalRRGstAmount { get; set; }
        public decimal THCAmount { get; set; }

        public decimal TotalTHGstAmount { get; set; }

        public decimal GrandTotal { get; set; }
       
    }
}