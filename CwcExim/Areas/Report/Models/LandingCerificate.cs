using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class TrainDetl
    {
        public List<LandingCerificate> objTrainSummaryUpload = new List<LandingCerificate>();
        public string TrainNo { get; set; }
        public string TrainDate { get; set; }
        public string PortName { get; set; }
    }
    public class LandingCerificate
    {
        public int PortId { get; set; }
        public string PortName { get; set; }
        public int SrNo { get; set; } 
        //public string Wagon_No { get; set; }
        public string Container_No { get; set; }
        public string CT_Size { get; set; }
        public string S_Line { get; set; }
        public string TP_No { get; set; }
        public string TP_Date { get; set; }
        public string ArrivalDate { get; set; }
       
    }
    public class PPG_TrainDateList
    {
        public string TrainDate { get; set; }
        
    }
}