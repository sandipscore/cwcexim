using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CwcExim.Areas.Master.Models
{
    public class DSRChargeName
    {
        public int ChargeNameId { get; set; }

        [Required(ErrorMessage = "Fill Out This Field")]
        public string ChargeName { get; set; }
        public string ChargeCode { get; set; }
        public int Uid { get; set; }
        public bool IsMisc { get; set; }
    }
}