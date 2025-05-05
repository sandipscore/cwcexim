using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_CHAList
    {
        public int CHAId { get; set; }
        public string CHAName { get; set; }
        public string CHACode { get; set; }
        public int? BillToParty { get; set; }
    }
}