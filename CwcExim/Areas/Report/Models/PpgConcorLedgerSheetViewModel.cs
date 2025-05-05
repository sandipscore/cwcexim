using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class PpgConcorLedgerSheetViewModel
    {


        public string SlNo { get; set; }
        public string Date { get; set; } 
        public string ConcorInvoiceNo { get; set; }   
        public string InvoiceDate { get; set; }      
        public string OperationType { get; set; }      
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }       
        public string ContainerNo { get; set; }
        public int Size { get; set; }

        public string POLPOD { get; set; }

        public decimal? NetWeight { get; set; }

        public decimal? TareWeight { get; set; }

        public decimal? GrossWeight { get; set; }
        public string ContainerType { get; set; }

       

        public decimal? IRR { get; set; }

        public decimal? THC { get; set; }

        public decimal? DOC { get; set; }

        public decimal? OtherChg { get; set; }

       
        public decimal CreditAmount { get; set; }


        public decimal? Balance { get; set; }

        public string Remarks { get; set; }


        [Required]
        public string FromDate { get; set; }

        [Required]
        public string ToDate { get; set; }
    }
}