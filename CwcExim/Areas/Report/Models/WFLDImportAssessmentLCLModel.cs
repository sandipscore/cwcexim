using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLDImportAssessmentLCLModel
    {
        public int SR { get; set; }
        public string ReceiptNo { get; set; }

        public string ReceiptDate { get; set; }

        public string CBM { get; set; }

        public string TSANo { get; set; }

        public string CHAName { get; set; }
        public string PartyName { get; set; }

        public string Amount { get; set; }

    }
}