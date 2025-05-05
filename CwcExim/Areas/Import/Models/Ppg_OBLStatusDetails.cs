using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Ppg_OBLStatusDetails
    {
        public string BOLNo { get; set; }
        public int NOPKG { get; set; }
        public decimal GRWT { get; set; }
        public string CARGO_DESC { get; set; }
        public string IMP_NAME { get; set; }
        public string CONTAINER_NO { get; set; }
        public decimal CIFValue { get; set; }
        public decimal Duty { get; set; }
    }
}