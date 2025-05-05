using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class WFLD_TDSReportModel
    {
        public int SR { get; set; }
        public string ReceiptNo { get; set; }

        public string ReceiptDate { get; set; }

        public string PartyName { get; set; }

        public string Amount { get; set; }

        public string Remarks { get; set; }
        public string FinancialYear { get; set; }
        public string TdsQuarter { get; set; }

    }
}