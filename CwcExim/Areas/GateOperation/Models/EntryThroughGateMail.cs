using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.GateOperation.Models
{
    public class EntryThroughGateMail
    {
        public string Email { get; set; }
        public string FileName { get; set; }
        public IList<ExcelData> lstExcelData = new List<ExcelData>();
    }
    public class ExcelData
    {
        public string Line { get; set; }
        public string ContainerNumber { get; set; }
        public string Size { get; set; }
        public string MoveCode { get; set; }
        public string EntryDateTime { get; set; }
        public string CurrentLocation { get; set; }
        public string ToLocation { get; set; }
        public string BookingRefNo { get; set; }
        public string Customer { get; set; }
        public string Transporter { get; set; }
        public string TruckNumber { get; set; }
        public string Condition { get; set; }
        public string ReportedBy { get; set; }
        public string ReportDate { get; set; }
        public string Remarks { get; set; }
        public string TransportMode { get; set; }
        public string JobOrder { get; set; }
    }

    public class EntryThroughGateMailPIL
    {
        public string Email { get; set; }
        public string FileName { get; set; }
        public IList<ExcelDataPIL> lstExcelData = new List<ExcelDataPIL>();
    }
    public class ExcelDataPIL
    {
        public int Sr { get; set; }
        public string Line { get; set; }
    }


}