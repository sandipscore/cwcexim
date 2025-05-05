using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_ContainerInOut
    {
        public int ImportCbtIn20Teus { get; set; }
        public int ImportCbtIn40Teus { get; set; }

        public int ImportContIn20Teus { get; set; }
        public int ImportContIn40Teus { get; set; }

        public int ExportCBTout20Teus { get; set; }
        public int ExportCBTout40Teus { get; set; }

        public int ExportContout20Teus { get; set; }
        public int ExportContout40Teus { get; set; }
        public int EmptyIn20Teus { get; set; }
        public int EmptyIn40Teus { get; set; }
        public int EmptyOut20Teus { get; set; }
        public int EmptyOut40Teus { get; set; }

        public int  BondContIn20Teus { get; set; }
        public int BondContIn40Teus { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodFrom { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PeriodTo { get; set; }

        public int TotalTues { get; set; }



    }
}