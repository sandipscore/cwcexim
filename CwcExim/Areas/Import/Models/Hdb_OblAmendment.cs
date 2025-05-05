using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Import.Models
{
    public class Hdb_OblAmendment
    {
        public string Date { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string OOBLNo { get; set; }
        public string OBLDate { get; set; }
        public int ONoOfPkg { get; set; }
        public decimal OGRWT { get; set; }
        public decimal OCIFValue { get; set; }
        public decimal OGRDuty { get; set; }
        public string ContainerNo { get; set; }
        public string CFSCode { get; set; }
        public string IGMNo { get; set; }
        public string IGMDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string NOBLNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int NNoOfPkg { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public decimal NGRWT { get; set; }
        public decimal NCIFValue { get; set; }
        public decimal NGRDuty { get; set; }
        public int OImporterId { get; set; }
        public string OImporterName { get; set; }
        public int NImporterId { get; set; }
        public string NImporterName { get; set; }
        public int RetValue { get; set; }
    }

    public class Hdb_OBLNoForPage
    {
        public string OBLNo { get; set; }
    }

    public class Hdb_Importer
    {
        public int ImporterId { get; set; }
        public string ImporterName { get; set; }
        public string ImporterCode { get; set; }

    }
}