using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class SEISDataViewModel
    {
        public int ID { get; set; }
        public string FOREIGNLINER { get; set; }
        public string VESSELNO { get; set; }
        public string VESSEL_NM { get; set; }

        public string INDIAN_AGENT_FOREIGN_LINER { get; set; }
        public string CONTAINER_NO { get; set; }

        public string INVOICE_BILL_NO { get; set; }

        public string INVOICE_BILL_DT { get; set; }

        public string RECEIPT_NO { get; set; }

        public string RECEIPT_DATE { get; set; }
        public decimal CARGO_HANDLING_AMT { get; set; }

        public decimal STORAGE_WAREHOUSING_AMT { get; set; }

        public decimal OTHER_CHARGES { get; set; }

        public decimal TAX_CHR { get; set; }

        public string Remarks { get; set; }


      

        [Required]
        public string PeriodFrom { get; set; }

        [Required]
        public string PeriodTo { get; set; }

        public string IsImportExport { get; set; }


    }
}