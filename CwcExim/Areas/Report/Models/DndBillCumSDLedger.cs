using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class DndBillCumSDLedger
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
        public List<DndBillCumSDLedgerDetails> lstDetails { get; set; } = new List<DndBillCumSDLedgerDetails>();
        public List<DndBillCumSDLedgerSummary> lstSummary { get; set; } = new List<DndBillCumSDLedgerSummary>();

    }

    public class DndBillCumSDLedgerDetails
    {
        public int Sr { get; set; }
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
        public string Col6 { get; set; }


    }

    public class DndBillCumSDLedgerSummary
    {
        public int Sr { get; set; }
        public string Col7 { get; set; }
        public string Col8 { get; set; }
        public string Col9 { get; set; }
        public string Col10 { get; set; }

    }
}
