using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class HDB_PartyWiseSdStatement
    {
        public decimal Opening { get; set; }
        public string PartyName { get; set; }

        public List<HDB_PartyWiseSdStatementDetails> LstSdStatement { get; set; } = new List<HDB_PartyWiseSdStatementDetails>();
    }


  public class HDB_PartyWiseSdStatementDetails
    {
        public int SrNo { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptDate { get; set; }
        public string Mode { get; set; }
        public string JVNo { get; set; }
        public string JVDate { get; set; }

        public decimal Amount { get; set; }
    }
}