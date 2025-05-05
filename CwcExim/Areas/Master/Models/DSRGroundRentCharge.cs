using CwcExim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Master.Models
{
    public class DSRGroundRentCharge:CWCChargesGroundRent 
    {
        //[Required(ErrorMessage = "Fill Out This Field")]
        public string FclLcl { get; set; }
    }
}