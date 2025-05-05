using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Areas.Bond.Models
{
    public class SpaceAvailableCert :BondApp
    {
        //[Required(ErrorMessage ="Fill Out This Field")]
       // [StringLength(30,ErrorMessage = "Sac No Cannot Be More Than 30 Characters In Length")]
        public string SacNo { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string SacDate { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        [Range(0,99999999.99,ErrorMessage = "Area Reserved Cannot Be More Than 99999999.99")]
        public decimal AreaReserved { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ValidUpto { get; set; }
        public int IsApproved { get; set; }
        public int IsSubmitted { get; set; }
        public int ApprovedBy { get; set; }
    }

    public class SpaceAvailCertPdf
    {
        public string SacNo { get; set; }
        public string SacDate { get; set; }
        public string BOLAWBNo { get; set; }
        public string BOENoDate { get; set; }
        public decimal AreaReserved { get; set; }
        public string ValidUpto { get; set; }
        public string Importer { get; set; }
        public string CHAName { get; set; }
    }
}