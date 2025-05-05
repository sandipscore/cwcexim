using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSRTrainSummaryUpload
    {

        public List<DSRTrainSummaryUpload> TrainSummaryUploadList { get; set; }

        public string TrainSummaryList { get; set; }
        public int TrainSummaryUploadId { get; set; }


        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrainDate { get; set; }

        public int PortId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PortName { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$", ErrorMessage = "Only Excel files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }

        
        public int SrNo { get; set; }
        //public string Cont_Commodity { get; set; }
        //public decimal Ct_Tare { get; set; }
        //public decimal Cargo_Wt { get; set; }
        //public string Received_Date { get; set; }


        //public string Foreign_Liner { get; set; } = string.Empty;
        //public string Vessel_Name { get; set; } = string.Empty;
        //public string Vessel_No { get; set; } = string.Empty;
        //public string Genhaz { get; set; }

        public string StatusDesc { get; set; }
        public int Status { get; set; }



        [Required(ErrorMessage = "Fill Out This Field")]
        public string TrainNo { get; set; } //Train No
        public string Container_No { get; set; }
        public string S_Line { get; set; } //Shipping Line
        public string CT_Size { get; set; } //Size
        public string Ct_Type { get; set; } //Type
        public string Ct_Status { get; set; } //Status
        public string Destination { get; set; } //Port of Destination
        public string Wagon_No { get; set; }//Wagon No
        public string Seal_Intact { get; set; }//Seal Intact
        public string Port_of_Loading { get; set; }//Port of Loading
        public string Smtp_No { get; set; } //SMTP No
        public string UploadDate { get; set; }
        public string Loading_Date { get; set; }//  Date
        public string Cargo_Desc { get; set; }//Cargo_Description
        public decimal Gross_Wt { get; set; } //Weight
        public string Line_Seal_No { get; set; }// Seal No
        public string POD_Code { get; set; } //POD Code



    }

}