using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Ppg_TrainSummaryUploadV2
    {
        //public List<TrainSummaryUpload> TrainSummaryUploadList { get; set; }
        public string TrainSummaryList { get; set; }
        public int TrainSummaryUploadId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrainNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrainDate { get; set; }

        public int PortId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PortName { get; set; } 

        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$", ErrorMessage = "Only Excel files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }

        public string UploadDate { get; set; }

        public int SrNo { get; set; }
        public string Wagon_No { get; set; }
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string Line_Seal_No { get; set; }
        public string Cont_Commodity { get; set; }
        public string S_Line { get; set; }
        public decimal Ct_Tare { get; set; }
        public decimal Cargo_Wt { get; set; }
        public decimal Gross_Wt { get; set; }
        public string Ct_Status { get; set; }
        public string Destination { get; set; }
        public string Smtp_No { get; set; }
        public string Received_Date { get; set; }
        public string Genhaz { get; set; }
        public int Status { get; set; }
        public string StatusDesc { get; set; }
        public string Foreign_Liner { get; set; } = string.Empty;
        public string Vessel_Name { get; set; } = string.Empty;
        public string Vessel_No { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public int InvoiceId { get; set; } = 0;
        public decimal InvoiceAmt { get; set; } = 0;
        public string PayeeName { get; set; } = string.Empty;
        public int PayeeId { get; set; } = 0;
    }
}