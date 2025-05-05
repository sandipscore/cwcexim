using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CwcExim.Areas.Export.Models
{
    public class CartingList
    {
        public int CartingAppId { get; set; }
        [Display(Name = "Carting No")]
        public string CartingNo { get; set; }
        public string ApplicationDate { get; set; }
        [Display(Name = "CHA")]
        public string CHAName { get; set; }
    }
}