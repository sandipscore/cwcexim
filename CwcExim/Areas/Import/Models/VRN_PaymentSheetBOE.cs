using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class VRN_PaymentSheetBOE
    {
        public string CFSCode { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public bool Selected { get; set; }


    }
}