using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.GateOperation.Models
{
    public class WljGatePassList
    {
        public int GatePassId { get; set; }
        public string GatePassNo { get; set; }

        public string GatePassDate { get; set; }
       
    }
 
}