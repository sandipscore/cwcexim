using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CwcExim.Areas.Bond.Models
{
    public class DSRGodownWiseReserveSpace
    {
        public string ValidUpto { get; set; } 
        public decimal TotalSpace { get; set; }
        public string ReservationTo { get; set; } 
        public DSRSpaceReservedtl DSRSpaceReservedtl { get; set; } = new DSRSpaceReservedtl();
    }
}