using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class DSR_PaymentSheetContainer
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public bool Selected { get; set; }
        public string Size { get; set; }
        public string ArrivalDt { get; set; }
        public string IsHaz { get; set; }
        public string LCLFCL { get; set; }
        public string LineNo { get; set; }
        public string BOENo { get; set; }
        public string BOEDate { get; set; }

        public string OBLNo { get; set; }
        public string SealCutDate { get; set; }
        public int NoOfPkg { get; set; }
        public decimal GrWait { get; set; }

        public bool IsBond { get; set; }

        public bool INS { get; set; }
        public bool LIN { get; set; }
        public bool LIO { get; set; }
        public bool REW { get; set; }
        public bool WET { get; set; }
    }
}