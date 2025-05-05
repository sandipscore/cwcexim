using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Report.Models
{
    public class Hdb_PartyWiseUnrelized
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public string PartyName { get; set; }

        public int PayeeId { get; set; }
        //  public int MyProperty { get; set; }

        public List<Hdb_PartyWiseUnrelizeddetails> Hdb_PartyWiseUnrelizeddetailsLst { get; set; }
     //   public List<Hdb_PartyWiseUnrelized> Hdb_PartyWiseUnrelizeddetailsLst { get; set; }

    }
    public class Hdb_PartyWiseUnrelizeddetails
    {
        public string PartyName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public int PayeeId { get; set; }
    }

}