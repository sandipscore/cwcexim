using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace CwcExim.Areas.Export.Models
{
    public class DSRExportTrainSummaryUpload
    {
        public List<DSRExportTrainSummaryUpload> TrainSummaryUploadList { get; set; }

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
        public string Wagon { get; set; }
        public string ContainerNo { get; set; }
        public string SZ { get; set; }
        public string Status { get; set; }

        public string SLine { get; set; }
        public string Commodity { get; set; }
        public decimal TW { get; set; }
        public decimal CW { get; set; }
        public decimal GW { get; set; }
        public int PKGS { get; set; }
        public string LineSeal { get; set; }
        public string CustomSeal { get; set; }
        public string Shipper { get; set; }
        public string CHA { get; set; }
        public string CRRSBillingParty { get; set; }
        public string BillingParty { get; set; }
        public string StuffingMode { get; set; }
        public string SBillNo { get; set; }
        public DateTime Date { get; set; }
        public string Origin { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public DateTime DepDate { get; set; }

        public string StatusDesc { get; set; }
        public int StatusValue { get; set; }
        public string ForeignLiner { get; set; }
        public string VesselName { get; set; }
        public string VesselNo { get; set; }

    }
}