using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class Kdl_AddMoneyToPDList
    {
        public string PdaTranRecNo { get; set; }
        public string PdaTransDate { get; set; }
        public string EximTraderName { get; set; }
        public string Address { get; set; }
        public string FolioNo { get; set; }
        public string PdaPayType { get; set; }
        public string Amount { get; set; }
        public string PdaDrawBank { get; set; }
        public string PdaInsNo { get; set; }
        public string PdaChequeDate { get; set; }
    }
}