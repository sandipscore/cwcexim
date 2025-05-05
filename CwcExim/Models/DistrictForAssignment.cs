using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CwcExim.Models
{
    public class DistrictForAssignment
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictCode { get; set; }
        public bool IsSelected { get; set; }
       
    }
}