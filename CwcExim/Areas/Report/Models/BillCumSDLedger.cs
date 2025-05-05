using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class BillCumSDLedger
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string EximTraderName { get; set; }
        public string EximTraderAlias { get; set; }
        /*
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string GSTNo { get; set; }
        public string PinCode { get; set; }
        */
        public string COMGST { get; set; }
        public string COMPAN { get; set; }

        public decimal OpenningBalance { get; set; } = 0;
        public decimal ClosingBalance { get; set; } = 0;
        /*
        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalCredit { get; set; } = 0;
        */
        public string CurDate { get; set; }
        public List<BillCumSDLedgerDetails> lstDetails { get; set; } = new List<BillCumSDLedgerDetails>();
    }

    public class BillCumSDLedgerDetails
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