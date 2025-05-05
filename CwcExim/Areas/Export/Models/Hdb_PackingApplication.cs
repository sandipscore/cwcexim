using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Export.Models
{
    public class Hdb_PackingApplication
    {
        public int Packingapplicationid { get; set; }
        public string   PackingNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]

        public string VehicleNo { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string EntryDate { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string PartyName { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public int? NoofItems { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string MaterialType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string Remarks { get; set; }
        public int Uid { get; set; }
        public string Others { get; set; }
    }
}