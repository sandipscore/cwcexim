using CwcExim.Areas.Export.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.CashManagement.Models
{
    public class DSR_RentParty

    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        //public  int logic1 { get; set; }
        //public   int logic2 { get; set; }
        //public  int logic3 { get; set; }
        public string CompStateCode { get; set; }
        public string SacCode { get; set; }
        public string GSTPer { get; set; }



    }
}