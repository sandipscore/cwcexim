using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class VRN_BillCumSDLedger
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        
        public string COMGST { get; set; }
        public string COMPAN { get; set; }

        public decimal OpenningBalance { get; set; } = 0;
        public decimal ClosingBalance { get; set; } = 0;
        
        public string CurDate { get; set; }
        public List<VRN_BillCumSDLedgerDetails> lstDetails { get; set; } = new List<VRN_BillCumSDLedgerDetails>();
    }

    public class VRN_BillCumSDLedgerDetails
    {
        public int Sr { get; set; }
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }


    }
}