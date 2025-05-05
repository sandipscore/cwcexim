using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLDExportEGME07Upload
    {
         
        public int? Totalfalure { get; set; }
        public int? TotalSusess { get; set; }
        public string ExcelFileName { get; set; }
        public string MESSAGETYPE { get; set; } 
        public string MODEOFTRANSPORT { get; set; }  
        public string CUSTOMSHOUSECODE { get; set; }

        public string SBNO { get; set; }
        public string VECHICLENO { get; set; }
        public string SBDATE { get; set; }
        public string VECHICLEDEPARTUREDATE { get; set; }
        public string CONTAINERNO { get; set; }
        public string SHIPPINGLINECODE { get; set; }
        public decimal WEIGHT { get; set; }
        public string DESTINATIONPORTCODE { get; set; }
        public string ORIGINRAILSTATIONCODE { get; set; }
        public string ISOCODE { get; set; }
        public string GATEWAYPORTCODE { get; set; }
        public string STATUSOFCONTAINER { get; set; }
        //public string ORIGINRAILSTATIONCODE { get; set; }
        public string EGMSummaryList { get; set; }
        public List<WFLDExportEGME07Upload> EGME07SummaryUploadList { get; set; }


    }
}