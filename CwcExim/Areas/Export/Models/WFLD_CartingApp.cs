using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class WFLD_CartingApp
    {
       
        public string InvoiceNo { get; set; }

        public int? MarksandNo { get; set; }
        public int? NoOfTEUS { get; set; }
        public string SBNo { get; set; }
        public string ExporterImporterName { get; set; }
        public int CargoType { get; set; }
        public int Package { get; set; }
        public int Weight { get; set; }

        public List<WFLD_CartingApp> lstcartapp = new List<WFLD_CartingApp>();
        


    }
 
   
}