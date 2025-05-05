using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class Vrn_E04Report
    {
        public int? ID { get; set; }      
        public string MSG_TYPE { get; set; }
        public string CUSTOM_CD { get; set; }
        public string SB_NO  { get; set; }
        public string SB_DATE  { get; set; }
        public string IEC_CD { get; set; }
        public string BI_NO { get; set; }
        public string EXP_NAME { get; set; }
        public string Address { get; set; }
        public string EXP_ADD1 { get; set; }
        public string EXP_ADD2 { get; set; }
        public string CITY { get; set; }
        public string PIN { get; set; }
        public string CHA_CODE { get; set; }
        public string FOB { get; set; }
        public string POD { get; set; }
        public string LEO_NO { get; set; }
        public string LEO_DATE { get; set; }
        public string ENTRY_NO { get; set; }
        public string G_DATE { get; set; }
        public string TRANSHIPPER_CODE { get; set; }
        public string GATEWAY_PORT { get; set; }
        public string PCIN { get; set; }


}
}