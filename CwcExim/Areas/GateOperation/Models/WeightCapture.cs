using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CwcExim.Areas.GateOperation.Models
{
    public class WeightCapture
    {
        public string ELEWId { get; set; }
        public string RefNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string RefNoDate { get; set; }
        public string Time { get; set; }

        [Display(Name = "Container No.")]
        public string ContNo { get; set; }
        public string ContainerId { get; set; }
        public string VContNo { get; set; }
        [Display(Name = "CFS Code")]
        public string CFSCode { get; set; }
        [Display(Name = "Veichel No.")]
        public string VeichelNo { get; set; }        
        public string VeichelNoOther { get; set; }
        public string VeichelNoList { get; set; }
        public string WeightFor { get; set; }
        public string VWeight { get; set; }
        public string GWeight { get; set; }
        public string TWeight { get; set; }
        public string Material { get; set; }
        public string CustomerName { get; set; }
        public string WeightInKg { get; set; }
        public string VVeichelNo { get; set; }
        public string Remarks { get; set; }
        public List<ELWBWCVechicleNoList> VechNoList { get; set; } = new List<ELWBWCVechicleNoList>();
        public List<WeightCaptureList> ListData { get; set; } = new List<WeightCaptureList>();
        public List<WCContList> ContNoList { get; set; } = new List<WCContList>();
    }
    public class WeightCaptureList
    {
        public string ELEWId { get; set; }
        public int SrlNo { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string RefNo { get; set; }
        public string ContNo { get; set; }        
        public string CFSCode { get; set; }
        public string Type { get; set; }
        public string Weight { get; set; }        
        public string WeightInKg { get; set; }
        public string GWeight { get; set; }
        public string TWeight { get; set; }
        public string VeichelNoP { get; set; }
        public string VeichelNoC { get; set; }
        public string Material { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
        public List<ELWBWCVechicleNoList> VechNoList { get; set; } = new List<ELWBWCVechicleNoList>();
    }
    public class WCContList
    {
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string Material { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
    }
    public class ELWBWCVechicleNoList
    {
        public string VecholNo { get; set; }
    }
}