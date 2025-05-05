using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class DSR_PaymentSheetContainerExpEmp
    {
        public string CFSCode { get; set; }
        public string ContainerNo { get; set; }
        public string EntryDateTime { get; set; }
        public string Size { get; set; }
        public string EximTraderAlias { get; set; }
        public string InDateEcy { get; set; }
        public string OutDateEcy { get; set; }
        public int Days { get; set; }
        public decimal Amount { get; set; }
        public bool Selected { get; set; }
    }
    public class DSR_PaymentParty
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string GSTNo { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string PartyAlias { get; set; }
    }
}