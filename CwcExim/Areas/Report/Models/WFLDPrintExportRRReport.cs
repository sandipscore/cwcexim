using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDPrintExportRRReport
    {
       
        public string IWBNo { get; set; }

        public List<WFLDContDetails> LstContDetails { get; set; } = new List<WFLDContDetails>();
        public List<WFLDtDetails> LstDetails { get; set; } = new List<WFLDtDetails>();
    }
    public class WFLDtDetails
    { 
    public string PartyName { get; set; }
    public string PartyAddress { get; set; }
    public string PayeeName { get; set; }

    public string PayeeAddress { get; set; }
    public string ForwarderName { get; set; }
    public string ShippingLineName { get; set; }
    public string PortOfLoading { get; set; }
    public string CHA { get; set; }
    public string Exporter { get; set; }
    public string ShippBill { get; set; }
    public string POD { get; set; }
    }
    public class WFLDContDetails
    {
        public string ContainerNo { get; set; }
        public string Size { get; set; }
        public string Loaded { get; set; }
        public Int32 NoOfPackages { get; set; }
        public Decimal GrossWt { get; set; }
        public string CFSCode { get; set; }

       

    }


   
  
}