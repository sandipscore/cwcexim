using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class PpgConcorLedgerSheetViewModelV2
    {
        public int ID { get; set; }
        [Required]
        public string Date { get; set; }

        [Required]
        public string SlNo { get; set; }

        [Required]
        public string ConcorInvoiceNo { get; set; }

        public string InvoiceID { get; set; }

        [Required]
        public string InvoiceDate { get; set; }

        [Required]
        public string OperationType { get; set; }

        [Required]
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }

        [Required]
        public string ContainerNo { get; set; }
        public int Size { get; set; }

        public string POLPOD { get; set; }

        public decimal? NetWeight { get; set; }

        public decimal? TareWeight { get; set; }

       
        public string ContainerType { get; set; }

        public decimal? GrossWeight { get; set; }

        public decimal? IRR { get; set; }

        public decimal? THC { get; set; }

        public decimal? DOC { get; set; }

        public decimal? OtherChg { get; set; }

        [Required]
        public decimal CreditAmount { get; set; }

       
        public decimal? Balance { get; set; }


    }
}