using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class OBLAmendment
    {
        public string OldOBLNo { get; set; }
        public string OBLDate { get; set; }
        public int OldNoOfPkg { get; set; }
        public decimal OldGRWT { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        public string NewOBLNo { get; set; }
        public int NewNoOfPkg { get; set; }
        public decimal NewGRWT { get; set; }
        public int RetValue { get; set; }
    }
    
    public class OBLNoForPage
    {
        public string OBLNo { get; set; }
        public string TSANo { get; set; }
        public string LINENo { get; set; }
    }

}