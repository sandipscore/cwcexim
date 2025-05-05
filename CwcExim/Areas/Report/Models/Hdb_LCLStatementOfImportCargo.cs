using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Report.Models
{

    public class Hdb_LCLStatement
    {

        public int SlNo { get; set; }

        public string ImporterName { get; set; }

        public string ImporterAddress { get; set; }
        public string TSANoDate { get; set; }
        public string BOLNo { get; set; }
        public string EntryDate { get; set; }

        public string DestuffingEntryDate { get; set; }
        public int NoOfUnitsRec { get; set; }
        public decimal Weight { get; set; }
        public string CargoDescription { get; set; }
        public decimal CIF { get; set; }
        public decimal Duty { get; set; }
        public decimal CIFDuty { get; set; }
        public decimal Area { get; set; }
        public string CFSCode { get; set; }
        public string ForwarderName { get; set; }
        public string Remarks { get; set; }

    }
    public class Hdb_LCLStatementOfImportCargo
    {


        [Required(ErrorMessage = "Fill Out This Field")]
        public string AsOnDate { get; set; }
        public int GodownId { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string GodownName { get; set; }

     
       
    }
   
}



