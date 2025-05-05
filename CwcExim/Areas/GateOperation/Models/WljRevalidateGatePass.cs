using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.GateOperation.Models
{
    public class WljRevalidateGatePass
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }

        public string ExpiryDT { get; set; }

        public string DeliveryDate { get; set; }

    }
 
}