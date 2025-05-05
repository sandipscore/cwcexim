using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class WFLDOBLAmendment
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
        public int NewCargoType { get; set; }
        public int OldCargoType { get; set; }
        public string NewTSA { get; set; }
        public string OldTSA { get; set; }

        public string NewLine { get; set; }
        public string OldLine { get; set; }
        public decimal NewArea { get; set; }
        public decimal OldArea { get; set; }
        public decimal NewCBM { get; set; }
        public decimal OldCBM { get; set; }

        public int ODLImporterID { get; set; }
        public string ODLImporterName { get; set; }
        public int ODLShippingLineId { get; set; }

        public string ODLShippinglineName { get; set; }

        public string ODLCargoDesc { get; set; }



        public int NewImporterID { get; set; }
        public string NewImporterName { get; set; }
        public int NewShippingLineId { get; set; }

        public string NewShippinglineName { get; set; }

        public string NewCargoDesc { get; set; }

        public string NewRemarks { get; set; }
        public string OldRemarks { get; set; }
        public int RetValue { get; set; }

    }
    
    public class WFLDOBLNoForPage
    {
        public string OBLNo { get; set; }
    }

}