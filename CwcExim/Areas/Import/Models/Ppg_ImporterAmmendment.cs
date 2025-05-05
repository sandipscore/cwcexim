using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Import.Models
{
    public class Ppg_ImporterAmmendment
    {
        public string OBLNo { get; set; }

        public string OBLDate { get; set; }

        public int OBLDTLId { get; set; }

        public int OBLHDRId { get; set; }
        public int OldImporterId { get; set; }
        public string OldImporterName { get; set; }
        public int NewImporterId{ get; set; }

        public string NewImporterName { get; set; }

        public int AmmendmentId { get; set; }
        public string AmendNo { get; set; }
        public string AmendDate { get; set; }
    }
}