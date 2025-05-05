using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Chn_MonthlyTimeBarredReport
    {
        public int NoOfStatrtTbb { get; set; }

        public decimal StartTbbDueAmt { get; set; }
        public decimal StrtTbbDueAccrdAmt { get; set; }
        public int NoOfAddedTbb { get; set; }
        public decimal AddedTbbDueAmt { get; set; }
        public int TotalTbb { get; set; }
        public decimal TotalDueAccrdAmt { get; set; }
        public int NoOfDisposeTbb { get; set; }
        public decimal RealisedAmt { get; set; }
        public decimal TotalDisposeAmt { get; set; }
        public int NoOfBalancedTbb { get; set; }
        public decimal BalancedAccrdAmt { get; set; }
        public decimal Area { get; set; }
        public string StartUnlddate { get; set; }
        public List<UptoLastMonthTimeBarred> LstPrevMnth { get; set; } = new List<UptoLastMonthTimeBarred>();
        public List<CurrMonthTimeBarred> LstCurrMnth { get; set; } = new List<CurrMonthTimeBarred>();


    }

    public class UptoLastMonthTimeBarred
    {
        public string LstMnthBondNo { get; set; }
        public decimal LstMnthStAmt { get; set; }
    }

    public class CurrMonthTimeBarred
    {
        public string CurrMnthBondNo { get; set; }
        public decimal CurrMnthStAmt { get; set; }
    }

}