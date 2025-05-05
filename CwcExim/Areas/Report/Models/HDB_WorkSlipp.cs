using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_WorkSlipp
    {
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string WorkSlipType { get; set; }
        public List<HDB_WorkSlippDetails> WorkSlippDetailList { get; set; }
        
    }
    public class HDB_WorkSlippDetails
    {
        public int SerialNo { get; set; }
        public string invoiceid { get; set; }
        public string WorkOrderDate { get; set; }
        public string invoiceno { get; set; }
       
        public string ContainerNo { get; set; }

       
        public List<HDB_HTDetails> WorkSlipDetailList { get; set; } = new List<HDB_HTDetails>();

    }
    public class HDB_HTDetails
    {
        //public int serialNo { get; set; }
        public string invoiceid { get; set; }
        public string invoiceno { get; set; }
        public string clause { get; set; }
        public string Amount { get; set; }
        public string CWCMargin { get; set; }
        public string HTCMargin { get; set; }
        public string HTCContractorRate { get; set; }
        public string CRNo { get; set; }
        public string CRAmount { get; set; }
    }

    public class HDB_BondWorkslip
    {
        public int SerialNo { get; set; }
        public string invoiceid { get; set; }
        public string invoiceno { get; set; }
        public string Clause { get; set; }
        public string WorkOrderNo { get; set; }
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Remarks { get; set; }
        public string BondNo { get; set; }
        public string ExBoeNo { get; set; }
        public string SacNo { get; set; }
        public string Weight { get; set; }
        public string Units { get; set; }
    }
}
