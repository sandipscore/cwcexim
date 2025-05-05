using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class LNSM_TrainSummaryUpload
    {
        //public List<TrainSummaryUpload> TrainSummaryUploadList { get; set; }
        public string TrainSummaryList { get; set; }
        public int TrainSummaryUploadId { get; set; }        
        public string TrainNo { get; set; }        
        public string TrainDate { get; set; }
        public int PortId { get; set; }        
        public string PortName { get; set; } 

        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$", ErrorMessage = "Only Excel files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }
        public string UploadDate { get; set; }
        public int SrNo { get; set; }
        public string NormalHub { get; set; }
        public string Wagon_No { get; set; }
        public string Stack { get; set; }
        public string Container_No { get; set; }
        public string LE { get; set; }
        public string CT_Size { get; set; }
        public string Cargo { get; set; }
        public decimal Cargo_Wt { get; set; }
        public decimal Ct_Tare { get; set; }
        public decimal Total_Wt { get; set; }
        public string Exim { get; set; }               
        public string StartLocation { get; set; }
        public string Destination { get; set; }        
        public string Remarks { get; set; }
        public int Status { get; set; }
    }
}