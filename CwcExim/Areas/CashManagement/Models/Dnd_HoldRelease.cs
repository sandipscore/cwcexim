using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.CashManagement.Models
{
    public class Dnd_HoldRelease
    {
        [Required(ErrorMessage = "Fill Out This Field")]
        public string DeliveryType { get; set; }
        public string CargoType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string StatusType { get; set; }
        [Required(ErrorMessage = "Fill Out This Field")]
        public string No { get; set; } 
        public string Date { get; set; }
     
        public string Reason { get; set; }
        public int Id { get; set; }
        public int Uid { get; set; }
        public string CFSCode { get; set; }

    }
}