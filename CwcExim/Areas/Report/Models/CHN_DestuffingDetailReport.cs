using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{

    public class CHN_DestuffingDetail
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PartyId { get; set; }
        public string Party { get; set; }
        public List<CHN_DestuffingDetailReport> lstDestuffingdetail { get; set; } = new List<CHN_DestuffingDetailReport>();
        public List<chn_destuffingrpt> lstchn_destuffingrpt { get; set; } = new List<chn_destuffingrpt>();
    }
    
    public class chn_destuffingrpt
    {
        public int serialNo { get; set; }
        public string ContainerNo { get; set; }

        public string Size { get; set; }
        public string ArrivalDate { get; set; }
        public string DestuffingDate { get; set; }
    }
    public class CHN_DestuffingDetailReport
    {
        //public int serialNo { get; set; }
        public string ContainerNo { get; set; }

        //public string Size { get; set; }
        //public string ArrivalDate { get; set; }
        //public string DestuffingDate { get; set; }
        public string OBLNo { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string DeliveryDate { get; set; }
        public int NoOfGrid { get; set; }
        public int StorageDays { get; set; }
        public string InvoiceNo { get; set; }
        public decimal StorageCharge { get; set; }
        public decimal DocumentationCharge { get; set; }
        public decimal FacilitationCharge { get; set; }
        public decimal AggregationCharge { get; set; }
        public decimal Weight { get; set; }
    
    }
}