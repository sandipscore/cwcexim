using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class OblEntry_Update
    {
        public string OldOBLNo { get; set; }
        public string OBLDate { get; set; }
        public int CargoType { get; set; }
        public int RetValue { get; set; }

    }

    public class OBLNoForPage1
    {
        public string OBLNo { get; set; }
        public string TSANo { get; set; }
        public string LINENo { get; set; }

    }
}